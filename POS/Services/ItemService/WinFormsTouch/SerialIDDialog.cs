using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;
using LSOne.Controls.Progress;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.Controls.SupportClasses;
using LSOne.Services.Interfaces.Enums;
using LSOne.Peripherals;

namespace LSOne.Services.WinFormsTouch
{
    public enum Buttons
    {
        PageUp,
        ArrowUp,
        ArrowDown,
        PageDown,
        KeyInSerialNumber,
        Clear,
        Select,
        Close,
        Search
    }

    public partial class SerialIDDialog : TouchBaseForm
    {
        public SerialNumber SelectedSerialNumber { get; private set; }

        public List<string> SerialNumbersToSkip { get; set; }
        public string ManualSerialNumber { get; private set; }

        public bool ManuallyEntered { get; set; }
        public bool ForceClose { get; private set; }

        private List<SerialNumber> SerialNumbers = new List<SerialNumber>();
        private int NumberOfItemsPerPage;
        private ISaleLineItem SaleLineItem { get; set; }

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        private LoadingIndicator progress;
        private int lastEntry;
        private int loadedRows;
        private bool reachedTheEnd;
        private bool checkScanResult;
        private bool clearingRows;

        public SerialIDDialog(IConnectionManager entry)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
        }

        public SerialIDDialog(IConnectionManager entry, ISaleLineItem saleLineItem,  List<string> serialsToSkip, int numberOfItemsPerPage = 100) : this(entry)
        {
            NumberOfItemsPerPage = numberOfItemsPerPage;
            SaleLineItem = saleLineItem;
            ManuallyEntered = false;
            ForceClose = false;
            SerialNumbersToSkip = serialsToSkip;

            try
            {
                panel.AddButton("", Buttons.PageUp, Conversion.ToStr((int)Buttons.PageUp), image: Resources.Doublearrowupthin_32px);
                panel.AddButton("", Buttons.ArrowUp, Conversion.ToStr((int)Buttons.ArrowUp), image: Resources.Arrowupthin_32px);
                panel.AddButton("", Buttons.ArrowDown, Conversion.ToStr((int)Buttons.ArrowDown), image: Resources.Arrowdownthin_32px);
                panel.AddButton("", Buttons.PageDown, Conversion.ToStr((int)Buttons.PageDown), image: Resources.Doublearrowdownthin_32px);
                panel.AddButton(Resources.Add, Buttons.KeyInSerialNumber, Conversion.ToStr((int)Buttons.KeyInSerialNumber), TouchButtonType.Normal, DockEnum.DockEnd, Resources.Plusincircle_16px, ImageAlignment.Left);
                panel.AddButton(Resources.Clear, Buttons.Clear, Conversion.ToStr((int)Buttons.Clear), TouchButtonType.Normal, DockEnum.DockEnd);
                panel.AddButton(Resources.Search, Buttons.Search, Conversion.ToStr((int)Buttons.Search), TouchButtonType.Action, DockEnum.DockEnd);
                panel.AddButton(Resources.Select, Buttons.Select, Conversion.ToStr((int)Buttons.Select), TouchButtonType.OK, DockEnum.DockEnd);
                panel.AddButton(Resources.Close, Buttons.Close, Conversion.ToStr((int)Buttons.Close), TouchButtonType.Cancel, DockEnum.DockEnd);
                panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), false);
                panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Select), false);
                touchKeyboard1.BuddyControl = tbSearch;
                lvSelection.ApplyRelativeColumnSize();

                if(saleLineItem.ReturnItem)
                {
                    panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.KeyInSerialNumber), false);
                }
            }
            catch { }

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            LoadData();
            tbSearch.Focus();
        }

        public void LoadFromDataBase()
        {
            try
            {
                int rows;

                List<SerialNumber> list = new List<SerialNumber>();
                bool sortAscending = lvSelection.SortedAscending;
                try
                {
                    ISiteServiceService service = (ISiteServiceService)dlgEntry.Service(ServiceType.SiteServiceService);

                    if (SaleLineItem.ReturnItem)
                    {
                        list = service.GetSoldSerialNumbersByItem(dlgEntry, dlgSettings.SiteServiceProfile,
                            SaleLineItem.MasterID,
                            tbSearch.Text,
                            lastEntry + 1,
                            lastEntry + NumberOfItemsPerPage,
                            DataLayer.BusinessObjects.Enums.SerialNumberSorting.SerialNumber,
                            sortAscending,
                            out rows,
                            true);
                    }
                    else
                    {
                        list = service.GetActiveSerialNumbersByItem(dlgEntry, dlgSettings.SiteServiceProfile,
                            SaleLineItem.MasterID,
                            tbSearch.Text,
                            lastEntry + 1,
                            lastEntry + NumberOfItemsPerPage,
                            DataLayer.BusinessObjects.Enums.SerialNumberSorting.SerialNumber,
                            sortAscending,
                            out rows,
                            true);
                    }
                }
                catch (Exception e)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Properties.Resources.CouldNotConnectToStoreServer, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return;
                }
                finally
                {
                    Interfaces.Services.DialogService(dlgEntry).CloseStatusDialog();
                }

                SerialNumbers.AddRange(list);

                foreach (SerialNumber serial in list)
                {
                    if (!SerialNumbersToSkip.Contains(serial.SerialNo))
                    {
                        var row = new Row();
                        row.AddText((string)serial.SerialNo);
                        row.Tag = serial;
                        lvSelection.AddRow(row);
                    }
                }

                loadedRows = list.Count;
                lastEntry += list.Count;
                reachedTheEnd = loadedRows < NumberOfItemsPerPage;
            }
            catch (Exception ex)
            {
                HideProgress();
                Interfaces.Services.DialogService(dlgEntry).ShowExceptionMessage(ex);
                loadedRows = 0;
            }
            finally
            {
                HideProgress();
            }
        }
        private void HideProgress()
        {
            if (progress != null)
            {
                Controls.Remove(progress);
                progress.Dispose();
                progress = null;
            }

            if(checkScanResult)
            {
                if (SerialNumbers.Count == 1 && SerialNumbers[0].SerialNo == tbSearch.Text)
                {
                    SelectedSerialNumber = SerialNumbers[0];
                    ManuallyEntered = false;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void LoadData(bool checkScanResult = false)
        {
            if (progress == null)
            {
                EventHandler handler = (sender, args) =>
                {
                    this.checkScanResult = checkScanResult;
                    LoadFromDataBase();
                };
                progress = LoadingIndicator.ShowOnParent(this, handler);
            }
        }

        private void Clear()
        {
            clearingRows = true;
            SerialNumbers.Clear();
            lvSelection.ClearRows();
            reachedTheEnd = false;
            lastEntry = 0;
            loadedRows = 0;
            ManuallyEntered = false;
            ManualSerialNumber = string.Empty;
            clearingRows = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), tbSearch.Text != "");

            Clear();
            LoadData(true);
        }

        private void btnPageUp_Click(object sender, EventArgs e)
        {
            lvSelection.MovePageUp();
        }

        private void btnPageDown_Click(object sender, EventArgs e)
        {
            lvSelection.MovePageDown();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            lvSelection.MoveSelectionDown();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            lvSelection.MoveSelectionUp();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbSearch.Text = "";
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), false);
            Clear();
            LoadData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(SaleLineItem.ReturnItem && lvSelection.Rows.Count == 0) //User is not able to select a serial number
            {
                ForceClose = true;
            };

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (lvSelection.Selection.Count > 0)
            {
                SelectedSerialNumber = (SerialNumber)lvSelection.Row(lvSelection.Selection.FirstSelectedRow).Tag;
                ManuallyEntered = false;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void lvSelection_RowDoubleClick(object sender, RowEventArgs args)
        {
            SelectedSerialNumber = (SerialNumber)args.Row.Tag;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void touchKeyboard1_ObtainCultureName(object sender, CultureEventArguments args)
        {
            if (dlgSettings.UserProfile.KeyboardCode != "")
            {
                args.CultureName = dlgSettings.UserProfile.KeyboardCode;
                args.LayoutName = dlgSettings.UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = dlgSettings.Store.KeyboardCode;
                args.LayoutName = dlgSettings.Store.KeyboardLayoutName;
            }
        }

        private void lvSelection_VerticalScrollValueChanged(object sender, EventArgs e)
        {
            if (!clearingRows && (lvSelection.FirstRowOnScreen + lvSelection.RowCountOnScreen) >= (lvSelection.RowCount - 1))
            {
                GetMoreItems();
            }
        }

        private void GetMoreItems()
        {
            if (reachedTheEnd)
            {
                return;
            }
            LoadData();
        }

        private void ShowKeyboardInput()
        {
            string dialogText = "";
            DialogResult result = Interfaces.Services.DialogService(dlgEntry).KeyboardInput(ref dialogText, Resources.EnterSerialNumber, Resources.SerialNumber, 300, InputTypeEnum.Normal);
            if (result != DialogResult.Cancel)
            {
                ManualSerialNumber = dialogText;
                ManuallyEntered = true;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void panel_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((int)args.Tag)
            {
                case (int)Buttons.PageUp: { btnPageUp_Click(null, null); break; }
                case (int)Buttons.ArrowUp: { btnUp_Click(null, null); break; }
                case (int)Buttons.ArrowDown: { btnDown_Click(null, null); break; }
                case (int)Buttons.PageDown: { btnPageDown_Click(null, null); break; }
                case (int)Buttons.KeyInSerialNumber: { ShowKeyboardInput(); break; }
                case (int)Buttons.Search: { btnSearch_Click(null, null); break; }
                case (int)Buttons.Clear: { btnClear_Click(null, null); break; }
                case (int)Buttons.Select: { btnSelect_Click(null, null); break; }
                case (int)Buttons.Close: { btnCancel_Click(null, null); break; }
            }
        }

        private void lvSelection_HeaderClicked(object sender, ColumnEventArgs args)
        {
            Clear();
            LoadData();
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                Clear();
                LoadData(true);
            }
        }

        private void lvSelection_SelectionChanged(object sender, EventArgs e)
        {
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Select), lvSelection.Selection.Count > 0);
        }

        private void SerialIDDialog_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                tbSearch.StartCharacter = dlgSettings.HardwareProfile.StartTrack1;
                tbSearch.EndCharacter = dlgSettings.HardwareProfile.EndTrack1;
                tbSearch.Seperator = dlgSettings.HardwareProfile.Separator1;
                tbSearch.TrackSeperation = TrackSeperation.Before;
                MSR.MSRMessageEvent += MSR_MSRMessageEvent;
                MSR.EnableForSwipe();
                Scanner.ScannerMessageEvent += Scanner_ScannerMessageEvent;
                Scanner.ReEnableForScan();
            }
        }

        private void Scanner_ScannerMessageEvent(DataLayer.BusinessObjects.BarCodes.ScanInfo scanInfo)
        {
            try
            {
                Scanner.DisableForScan();
                tbSearch.Text = scanInfo.ScanDataLabel;

                Clear();
                LoadData(true);
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        private void MSR_MSRMessageEvent(string track2)
        {
            tbSearch.Text = StringExtensions.TrackBeforeSeparator(track2, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);

            Clear();
            LoadData(true);
        }

        protected override void OnClosed(EventArgs e)
        {
            Scanner.ScannerMessageEvent -= Scanner_ScannerMessageEvent;
            Scanner.DisableForScan();
            MSR.MSRMessageEvent -= MSR_MSRMessageEvent;
            MSR.DisableForSwipe();

            base.OnClosed(e);
        }
    }
}
