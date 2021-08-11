using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.SVGUtilities;
using LSOne.Utilities.GUI;
using LSOne.Utilities.IO;
using Timer = System.Windows.Forms.Timer;
using LSOne.Services.Interfaces.EventArguments;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.POS.Processes;
using LSOne.Controls.SupportClasses;
using LSOne.POS.Processes.WinControls;

namespace LSOne.Services
{
    public partial class DualDisplayForm : TouchBaseForm
    {
        private decimal receiptPercent;
        private Timer pictureTimer;
        private int imageIndex;
        private Image originalPicture;
        private Image scaledPicture;
        private Thread thread;
        private bool threadRunning;
        private Thread multiCastThread;
        private bool multiCastThreadStop;
        private IPEndPoint multiCastEndPoint;
        private UdpClient multicastServer;
        private bool closing;
        private TouchLayout currentLayout;
        private Receipt receipt;
        private TotalAmounts totals;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        public DualDisplayForm(IConnectionManager entry, Screen screen)
        {
            closing = false;

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            InitializeComponent();
            receiptPercent = dlgSettings.HardwareProfile.DualDisplayReceiptPrecentage;
            SetFormLocation(screen);
            DoubleBuffered = true;

            ReceiptControlFactory receiptControlFactory = new ReceiptControlFactory();
            receipt = receiptControlFactory.CreateReceiptControl(pnlReceipt);
            totals = receiptControlFactory.CreateTotalAmountsControl(true, pnlTotals);

            var currency = Providers.CurrencyData.Get(dlgEntry, dlgSettings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
            receipt.DefaultCurrencySymbol = (dlgSettings.VisualProfile.ShowCurrencySymbolOnColumns) ? currency.Symbol : "";

            receipt.ReceiptItems.DisplayTotalWithoutTax(dlgSettings.Store.DisplayBalanceWithTax);

            var total = Providers.PosStyleData.GetByName(dlgEntry, SystemStylesEnum.DualDisplayTotal.ToString());
            var line = Providers.PosStyleData.GetByName(dlgEntry, SystemStylesEnum.DualDisplayLine.ToString());
            var lineSub = Providers.PosStyleData.GetByName(dlgEntry, SystemStylesEnum.DualDisplayLineSub.ToString());
            var totalNegative = Providers.PosStyleData.GetByName(dlgEntry, SystemStylesEnum.DualDisplayTotalLabelNegative.ToString());
            var totalValue = Providers.PosStyleData.GetByName(dlgEntry, SystemStylesEnum.DualDisplayTotalValue.ToString());
            var totalValueNegative = Providers.PosStyleData.GetByName(dlgEntry, SystemStylesEnum.DualDisplayTotalValueNegative.ToString());
            receipt.ReceiptItems.ChangeStyles(total, totalNegative, totalValue, totalValueNegative, line, lineSub);
            receipt.ReceiptPayments.ChangeStyles(total, totalNegative, totalValue, totalValueNegative, line);
            receipt.ButtonsVisible = false;

            receipt.PreDisplayReceiptItemEvent += ReceiptOnPreDisplayReceiptItemEvent;

            totals.Translate();
            totals.Subject = (POSApp)dlgSettings.POSApp;
            RecordIdentifier layoutId = Providers.TouchLayoutData.GetPOSTouchLayoutID(entry, dlgSettings.POSUser.ID, RecordIdentifier.Empty, entry.CurrentTerminalID, entry.CurrentStoreID);
            currentLayout = Providers.TouchLayoutData.Get(entry, layoutId, CacheType.CacheTypeApplicationLifeTime);

            SetTotalsLayout(currentLayout.TotalsLayoutXML);
        }

        public void SetTotalsLayout(string layoutXML)
        {
            if (layoutXML != "")
            {
                try
                {
                    totals.SetLayout(Stream.GetStream(POS.Processes.Common.Utility.TrimXML(layoutXML)));
                    totals.HideHeader();
                }
                catch (Exception x)
                {
                    dlgEntry.ErrorLogger.LogMessage(LogMessageType.Error, x.Message, x);
                }
            }
        }

        private void InitTimer(int interval)
        {
            pictureTimer = new Timer {Interval = interval*1000};
            pictureTimer.Tick += TimerTick;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            switch (dlgSettings.HardwareProfile.DualDisplayType)
            {
                case HardwareProfile.DisplayType.ImageRotator:
                    if (dlgSettings.HardwareProfile.DualDisplayImageInterval > 0)
                    {
                        InitTimer(dlgSettings.HardwareProfile.DualDisplayImageInterval);
                        LoadNextImage();
                    }
                    else
                    {
                        var folderItem = new FolderItem(dlgSettings.HardwareProfile.DualdisplayImagePath);
                        for (int i = 0; i < folderItem.Children.Count; i++)
                        {
                            originalPicture = GetBitmapFromFile(folderItem, i);
                            if (originalPicture != null)
                            {
                                break;
                            }
                        }
                    }
                    break;
                case HardwareProfile.DisplayType.Logo:
                    ShowLogo();
                    ScalePicture();
                    break;
                case HardwareProfile.DisplayType.WebPage:
                    try
                    {
                        ShowWebPage(dlgSettings.HardwareProfile.DualDisplayBrowserUrl);
                    }
                    catch (Exception)
                    {

                        ((IDialogService) dlgEntry.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.UnableToLoadWebPage,
                            MessageBoxButtons.OK, MessageDialogType.Attention);
                    }
                    
                    break;
                case HardwareProfile.DisplayType.SyncronizedImageRotator :
                    multiCastThread = new Thread(MulticastListener)
                    {
                        IsBackground = true,
                        Priority = ThreadPriority.BelowNormal
                    };
                    multicastServer = new UdpClient(9050);     
                    multicastServer.JoinMulticastGroup(IPAddress.Parse("224.100.0.1"));
                    multiCastThread.Start();
                    break;
            }
            if (!DesignMode && pictureTimer != null)
            {
                pictureTimer.Start();
            }
            SetReceiptWidth(receiptPercent);
        }

        protected override void OnClosed(EventArgs e)
        {
            closing = true;

            base.OnClosed(e);            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            closing = true;
            multiCastThreadStop = true;
            threadRunning = true;
            try
            {
                if (multiCastThread != null)
                {
                    multiCastThread.Join(500);
                }
            }
            catch (Exception)
            {
            }

            try
            {
                if (thread != null)
                {
                    thread.Join(500);
                }
            }
            catch (Exception)
            {
            }

            if (multicastServer != null)
            {
                multicastServer.DropMulticastGroup(IPAddress.Parse("224.100.0.1"));
                multicastServer.Close();
                multicastServer = null;
            }

            base.OnClosing(e);
        }

        internal void SetFormLocation(Screen screen)
        {
            Left = screen.Bounds.X;
            Top = screen.Bounds.Y;
            Width = screen.Bounds.Width;
            Height = screen.Bounds.Height;
        }

        private void MulticastListener()
        {
            while (!multiCastThreadStop)
            {
                try
                {
                    byte[] data = null;
                    while (multicastServer.Available > 0)
                    {
                        data = multicastServer.Receive(ref multiCastEndPoint);
                    }

                    if (data != null)
                    {
                        var stringData = Encoding.UTF8.GetString(data, 0, data.Length);

                        var fields = stringData.Split('|');

                        if (fields[0] == "e043e160-3a17-11e2-81c1-0800200c9a66")
                        {
                            LoadImageWithSeedNumber(Int32.Parse(fields[1]));

                            if (!IsDisposed && !pnlImages.IsDisposed && pnlImages.Created)
                            {
                                Invoke((Action)(() => pnlImages.Invalidate()));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // We get here if the server has been closed
                }
            }
        }

        private void ShowWebPage(string location)
        {
            pnlImages.Visible = false;
            webBrowser.Visible = true;
            webBrowser.Navigate(location);      
        }

        private void ShowLogo()
        {
            webBrowser.Visible = false;
            pnlImages.Visible = true;

            var assembly = this.GetType().Assembly;
            var stream = assembly.GetManifestResourceStream("LSOne.Services.Resources.LSRetail_Logo.svg");

            if (stream != null)
            {
                originalPicture = SVG.ImageFromSVGStream(stream, pnlImages.Width-100, pnlImages.Height-100);
                stream.Close();
            }
        }

        private void SetReceiptWidth(decimal percent)
        {
            if (percent < 0 || percent > 100)
            {
                return;
            }
            receiptPercent = percent;

            pnlReceipt.Width = (int)(Width * (percent/100));
            pnlTotals.Width = pnlReceipt.Width;

            if (webBrowser.Visible)
            {
                webBrowser.Width = Width - pnlReceipt.Width;
                webBrowser.Location = new Point(pnlTotals.Width, 0);
            } 
            else if (pnlImages.Visible)
            {
                pnlImages.Width = Width - pnlReceipt.Width;
                pnlImages.Location = new Point(pnlTotals.Width, 0);
            }
        }

        private void HideReceipt()
        {
            pnlReceipt.Visible = false;
            pnlTotals.Visible = false;

            if (webBrowser.Visible)
            {
                webBrowser.Width = Width;
                webBrowser.Location = new Point(0, 0);
            }
            else if (pnlImages.Visible)
            {
                pnlImages.Width = Width;
                pnlImages.Location = new Point(0, 0);
            }
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale certification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public void ShowTransaction(PosTransaction transaction)
        {
            bool triggerTotalsUpdate = false;

            if (transaction == null || (transaction is RetailTransaction && !((RetailTransaction)transaction).TenderLines.Any() && !((RetailTransaction)transaction).SaleItems.Any()))
            {
                if (receipt.Visible)
                {
                    HideReceipt();
                }
                else
                {
                    return;
                }
            }
            else
            {
                if(!pnlTotals.Visible)
                {
                    //If the totals are not visible yet, manually trigger the update after it's visible. 
                    //Control doesn't update if it's not visible, but it automatically updates if it's already visible
                    triggerTotalsUpdate = true;
                }

                pnlReceipt.Visible = true;
                pnlTotals.Visible = true;
                SetReceiptWidth(receiptPercent);
            }

            receipt.ShowTransaction(transaction);

            if(triggerTotalsUpdate)
            {
                totals.TriggerTransactionChanged(transaction);
            }
        }

        internal void TimerTick(object sender, EventArgs args)
        {
            LoadNextImage();       
        }

        private void LoadNextImage()
        {
            if (!threadRunning && !closing)
            {
                threadRunning = true;
                thread = new Thread(() => LoadImage());
                thread.Start();
            }

            void LoadImage()
            {
                var folderItem = new FolderItem(dlgSettings.HardwareProfile.DualdisplayImagePath);
                int startingIndex = imageIndex + 1;
                if (originalPicture != null)
                {
                    originalPicture.Dispose();
                }

                do
                {
                    if (imageIndex >= folderItem.Children.Count)
                    {
                        imageIndex = 0;
                    }
                    originalPicture = GetBitmapFromFile(folderItem, imageIndex);
                    imageIndex++;
                } while (originalPicture == null && imageIndex != startingIndex);

                if (originalPicture != null)
                {
                    ScalePicture();
                }

                threadRunning = false;

                if (!IsDisposed && !pnlImages.IsDisposed && pnlImages.Created)
                {
                    Invoke((Action)(() => pnlImages.Invalidate()));
                }
            }
        }

        private void LoadImageWithSeedNumber(int seedNumber)
        {
            if (!threadRunning && !closing)
            {
                threadRunning = true;
                thread = new Thread(() =>
                {
                    var folderItem = new FolderItem(dlgSettings.HardwareProfile.DualdisplayImagePath);
                    if (originalPicture != null)
                    {
                        originalPicture.Dispose();
                    }
                    originalPicture = GetBitmapFromFile(folderItem, seedNumber % folderItem.Count);
                    if (originalPicture != null)
                    {
                        ScalePicture();
                    }
                    threadRunning = false;
                });
                thread.Start();
            }
        }

        //Supports BMP, EXIF, GIF, JPG, JPEG, ICO, PNG, TIFF, WDP
        private Bitmap GetBitmapFromFile(FolderItem folder, int indexOfFile)
        {
            if (folder.Children.Count <= indexOfFile)
                return null;
            return BitmapHelper.GetBitmapFromFile(folder.Children[indexOfFile]);
        }

        private void ScalePicture()
        {
            try
            {
                lock(originalPicture)
                {
                    if (originalPicture.Height > pnlImages.Height ||
                                    originalPicture.Width > pnlImages.Width) //scaling reqired
                    {
                        float xScale = pnlImages.Width / (float)originalPicture.Width;
                        float yScale = pnlImages.Height / (float)originalPicture.Height;
                        float scale = xScale < yScale ? xScale : yScale;
                        var tempPicture = new Bitmap((int)(originalPicture.Width * scale), (int)(originalPicture.Height * scale));
                        var oldPicture = scaledPicture;
                        var g = Graphics.FromImage(tempPicture);
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(originalPicture, 0, 0, (int)(originalPicture.Width * scale), (int)(originalPicture.Height * scale));
                        g.Dispose();
                        scaledPicture = tempPicture;
                        if (oldPicture != null)
                        {
                            oldPicture.Dispose();
                        }
                    }
                    else
                    {
                        if ((originalPicture.Flags / 65536) == 1) //Read only flag
                        {
                            var tempPicture = new Bitmap(originalPicture.Width, originalPicture.Height);
                            var oldPicture = scaledPicture;
                            var g = Graphics.FromImage(tempPicture);
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.DrawImage(originalPicture, 0, 0, originalPicture.Width, originalPicture.Height);
                            g.Dispose();
                            scaledPicture = tempPicture;
                            if (oldPicture != null)
                            {
                                oldPicture.Dispose();
                            }
                        }
                        else
                        {
                            var oldPicture = scaledPicture;
                            scaledPicture = originalPicture;
                            if (oldPicture != null)
                            {
                                oldPicture.Dispose();
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {
            }
            
        }

        private void pnlImages_Paint(object sender, PaintEventArgs e)
        {
            if (scaledPicture != null)
            {
                try
                {
                    lock (scaledPicture)
                    {
                        int xCenter = (pnlImages.Width - scaledPicture.Width)/2;
                        int yCenter = (pnlImages.Height - scaledPicture.Height)/2;
                        e.Graphics.DrawImageUnscaled(scaledPicture, xCenter, yCenter);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void doubleBufferedPanel1_Resize(object sender, EventArgs e)
        {
            if (dlgSettings.HardwareProfile.DualDisplayType == HardwareProfile.DisplayType.Logo)
                ShowLogo();

            if (originalPicture != null)
            {
                ScalePicture();
                pnlImages.Invalidate();
            }            
        }

        private void ReceiptOnPreDisplayReceiptItemEvent(PreDisplayReceiptItemArgs e)
        {
            Services.Interfaces.Services.EventService(dlgEntry).PreDisplayReceiptItem(e);
        }
    }
}
