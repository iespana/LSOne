using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Columns;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Localization;
using Customer = LSOne.DataLayer.BusinessObjects.Customers.Customer;
using ListView = LSOne.Controls.ListView;
using LSRetailPosis;
using LSOne.Services.Properties;
using LSOne.Peripherals;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    public enum Buttons
    {
        PageUp,
        ArrowUp,
        ArrowDown,
        PageDown,
        NewCustomer,
        EditCustomer,
        TransferCustomer,
        Clear,
        Select,
        Close
    }

    /// <summary>
    /// Summary description for frmCustomerSearch.
    /// </summary>
    public class CustomerSearchDialog : TouchBaseForm
    {
        private const int maxRowsAtEachQuery = 30;
        private int loadedCount = 0;
        private int nameColumnIndex = 2;
        private string sortBy = "Name";
        private bool sortAsc = true;
        private string lastSearch = "";
        private static List<CustomerListItem> customerList;
        private ListView lvCustomer;
        private Column colAccount;
        private Column colAlias;
        private Column colName;
        private Column colAddress;
        private TouchKeyboard touchKeyboard1;
        private LSOne.Controls.MSRTextBoxTouch tbSearch;
        private TouchScrollButtonPanel panel;
        private bool returnNewCustomer;
        private bool hasInvoiceCustomer;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        private IPosTransaction transaction;

        public CustomerSearchDialog(IConnectionManager entry, bool returnNewCustomer, IPosTransaction transaction, string initialSearch)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            touchKeyboard1.BuddyControl = tbSearch;
            tbSearch.Text = initialSearch;

            this.returnNewCustomer = returnNewCustomer;
            this.transaction = transaction;

            //Set the size of the form the same as the main form
            this.Width = dlgSettings.MainFormInfo.MainWindowWidth;
            this.Height = dlgSettings.MainFormInfo.MainWindowHeight;

            if (!DesignMode)
            {
                tbSearch.StartCharacter = dlgSettings.HardwareProfile.StartTrack1;
                tbSearch.EndCharacter = dlgSettings.HardwareProfile.EndTrack1;
                tbSearch.Seperator = dlgSettings.HardwareProfile.Separator1;
                tbSearch.TrackSeperation = TrackSeperation.Before;
            }

            try
            {
                panel.AddButton("", Buttons.PageUp, Conversion.ToStr((int)Buttons.PageUp), image: Resources.Doublearrowupthin_32px);
                panel.AddButton("", Buttons.ArrowUp, Conversion.ToStr((int)Buttons.ArrowUp), image: Resources.ArrowUp32);
                panel.AddButton("", Buttons.ArrowDown, Conversion.ToStr((int)Buttons.ArrowDown), image: Resources.ArrowDown32);
                panel.AddButton("", Buttons.PageDown, Conversion.ToStr((int)Buttons.PageDown), image: Resources.Doublearrowdownthin_32px);
                panel.AddButton(Resources.NewCustomer, Buttons.NewCustomer, Conversion.ToStr((int)Buttons.NewCustomer), TouchButtonType.Normal, DockEnum.DockEnd, Resources.Plusincircle_16px, ImageAlignment.Left);
                panel.AddButton(Resources.Edit, Buttons.EditCustomer, Conversion.ToStr((int)Buttons.EditCustomer), TouchButtonType.Normal, DockEnum.DockEnd, Resources.Edit_16px, ImageAlignment.Left);
                panel.AddButton("", Buttons.TransferCustomer, Conversion.ToStr((int)Buttons.TransferCustomer), TouchButtonType.Normal, DockEnum.DockEnd, Resources.Movetocloud_32px);
                panel.AddButton(Resources.Clear, Buttons.Clear, Conversion.ToStr((int)Buttons.Clear), dock: DockEnum.DockEnd);
                panel.AddButton(Resources.Select, Buttons.Select, Conversion.ToStr((int)Buttons.Select), TouchButtonType.OK, DockEnum.DockEnd);
                panel.AddButton(Resources.Close, Buttons.Close, Conversion.ToStr((int)Buttons.Close), TouchButtonType.Cancel, DockEnum.DockEnd);
                
                lvCustomer.ApplyRelativeColumnSize();
                lvCustomer.SetSortColumn(nameColumnIndex, true);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerSearchDialog));
            this.tbSearch = new LSOne.Controls.MSRTextBoxTouch();
            this.touchKeyboard1 = new LSOne.Controls.TouchKeyboard();
            this.lvCustomer = new LSOne.Controls.ListView();
            this.colAccount = new LSOne.Controls.Columns.Column();
            this.colAlias = new LSOne.Controls.Columns.Column();
            this.colName = new LSOne.Controls.Columns.Column();
            this.colAddress = new LSOne.Controls.Columns.Column();
            this.panel = new LSOne.Controls.TouchScrollButtonPanel();
            this.SuspendLayout();
            // 
            // tbSearch
            // 
            resources.ApplyResources(this.tbSearch, "tbSearch");
            this.tbSearch.BackColor = System.Drawing.Color.White;
            this.tbSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbSearch.MaxLength = 60;
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            // 
            // touchKeyboard1
            // 
            resources.ApplyResources(this.touchKeyboard1, "touchKeyboard1");
            this.touchKeyboard1.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard1.BuddyControl = null;
            this.touchKeyboard1.Name = "touchKeyboard1";
            this.touchKeyboard1.TabStop = false;
            this.touchKeyboard1.EnterPressed += new System.EventHandler(this.touchKeyboard1_EnterPressed);
            this.touchKeyboard1.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard1_ObtainCultureName);
            // 
            // lvCustomer
            // 
            resources.ApplyResources(this.lvCustomer, "lvCustomer");
            this.lvCustomer.ApplyVisualStyles = false;
            this.lvCustomer.BackColor = System.Drawing.Color.White;
            this.lvCustomer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvCustomer.BuddyControl = null;
            this.lvCustomer.Columns.Add(this.colAccount);
            this.lvCustomer.Columns.Add(this.colAlias);
            this.lvCustomer.Columns.Add(this.colName);
            this.lvCustomer.Columns.Add(this.colAddress);
            this.lvCustomer.ContentBackColor = System.Drawing.Color.White;
            this.lvCustomer.DefaultRowHeight = ((short)(50));
            this.lvCustomer.EvenRowColor = System.Drawing.Color.White;
            this.lvCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvCustomer.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvCustomer.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvCustomer.HeaderHeight = ((short)(30));
            this.lvCustomer.HideVerticalScrollbarWhenDisabled = true;
            this.lvCustomer.Name = "lvCustomer";
            this.lvCustomer.OddRowColor = System.Drawing.Color.White;
            this.lvCustomer.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvCustomer.RowLines = true;
            this.lvCustomer.SecondarySortColumn = ((short)(-1));
            this.lvCustomer.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvCustomer.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCustomer.SortSetting = "0:1";
            this.lvCustomer.TouchScroll = true;
            this.lvCustomer.UseFocusRectangle = false;
            this.lvCustomer.VerticalScrollbarValue = 0;
            this.lvCustomer.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvCustomer.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvCustomer_HeaderClicked);
            this.lvCustomer.SelectionChanged += new System.EventHandler(this.lvCustomer_SelectionChanged);
            this.lvCustomer.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvCustomer_RowDoubleClick);
            this.lvCustomer.VerticalScrollValueChanged += new System.EventHandler(this.lvCustomer_VerticalScrollValueChanged);
            // 
            // colAccount
            // 
            this.colAccount.AutoSize = true;
            this.colAccount.DefaultStyle = null;
            resources.ApplyResources(this.colAccount, "colAccount");
            this.colAccount.MaximumWidth = ((short)(0));
            this.colAccount.MinimumWidth = ((short)(10));
            this.colAccount.RelativeSize = 19;
            this.colAccount.SecondarySortColumn = ((short)(-1));
            this.colAccount.Tag = null;
            this.colAccount.Width = ((short)(139));
            // 
            // colAlias
            // 
            this.colAlias.AutoSize = true;
            this.colAlias.DefaultStyle = null;
            resources.ApplyResources(this.colAlias, "colAlias");
            this.colAlias.MaximumWidth = ((short)(0));
            this.colAlias.MinimumWidth = ((short)(10));
            this.colAlias.RelativeSize = 18;
            this.colAlias.SecondarySortColumn = ((short)(-1));
            this.colAlias.Tag = null;
            this.colAlias.Width = ((short)(127));
            // 
            // colName
            // 
            this.colName.AutoSize = true;
            this.colName.DefaultStyle = null;
            this.colName.EmphasizeText = true;
            resources.ApplyResources(this.colName, "colName");
            this.colName.MaximumWidth = ((short)(0));
            this.colName.MinimumWidth = ((short)(10));
            this.colName.RelativeSize = 30;
            this.colName.SecondarySortColumn = ((short)(-1));
            this.colName.Tag = null;
            this.colName.Width = ((short)(242));
            // 
            // colAddress
            // 
            this.colAddress.AutoSize = true;
            this.colAddress.DefaultStyle = null;
            resources.ApplyResources(this.colAddress, "colAddress");
            this.colAddress.MaximumWidth = ((short)(0));
            this.colAddress.MinimumWidth = ((short)(10));
            this.colAddress.RelativeSize = 33;
            this.colAddress.SecondarySortColumn = ((short)(-1));
            this.colAddress.Tag = null;
            this.colAddress.Width = ((short)(278));
            // 
            // panel
            // 
            resources.ApplyResources(this.panel, "panel");
            this.panel.BackColor = System.Drawing.Color.Transparent;
            this.panel.ButtonHeight = 50;
            this.panel.Name = "panel";
            this.panel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.panel_Click);
            // 
            // CustomerSearchDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel);
            this.Controls.Add(this.touchKeyboard1);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.lvCustomer);
            this.Name = "CustomerSearchDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_FormClosing);
            this.Load += new System.EventHandler(this.frmCustomerSearch_Load);
            this.Shown += new System.EventHandler(this.frmCustomerSearch_Shown);
            this.ResumeLayout(false);

        }
        #endregion

        public CustomerListItem SelectedCustomer { get; private set; }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PopulateListView(List<CustomerListItem> items)
        {
            lvCustomer.ClearRows();
            hasInvoiceCustomer = false;
            RemoveInvoiceCustomerColumn();
            Row row;
            foreach (CustomerListItem item in items)
            {
                if (!hasInvoiceCustomer && !string.IsNullOrEmpty(item.InvoiceAccountName))
                {
                    hasInvoiceCustomer = true;
                    AddInvoiceCustomerColumn();
                }

                row = new Row();
                row.AddText((string)item.ID);
                row.AddText(item.SearchName);
                row.AddText(item.FirstName != "" ? item.GetFormattedName( dlgEntry.Settings.NameFormatter) : item.Text);
                row.AddText(dlgEntry.Settings.SystemAddressFormatter.FormatSingleLine(item.DefaultShippingAddress, dlgEntry.Cache));
                
                if(hasInvoiceCustomer)
                {
                    row.AddText(item.InvoiceAccountName);
                }

                row.Tag = item.ID;
                lvCustomer.AddRow(row);
            }

            lvCustomer.AutoSizeColumns(true);
        }

        private void AddToListView(List<CustomerListItem> items)
        {
            foreach (CustomerListItem item in items)
            {
                if (!hasInvoiceCustomer && !string.IsNullOrEmpty(item.InvoiceAccountName))
                {
                    hasInvoiceCustomer = true;
                    AddInvoiceCustomerColumn();
                }

                Row row = new Row();
                row.AddText((string)item.ID);
                row.AddText(item.SearchName);
                row.AddText(item.FirstName != "" ? item.GetFormattedName( dlgEntry.Settings.NameFormatter) : item.Text);
                row.AddText(dlgEntry.Settings.SystemAddressFormatter.FormatSingleLine(item.DefaultShippingAddress, dlgEntry.Cache));

                if (hasInvoiceCustomer)
                {
                    row.AddText(item.InvoiceAccountName);
                }

                row.Tag = item.ID;
                lvCustomer.AddRow(row);
            }

            lvCustomer.AutoSizeColumns(true);
        }

        private void GetMoreCustomers()
        {
            if (loadedCount % maxRowsAtEachQuery == 0)
            {
                List<CustomerListItem> list = GetCustomerList(lastSearch);
                AddToListView(list);
                customerList.AddRange(list);
                loadedCount += maxRowsAtEachQuery;
            }
        }

        private List<CustomerListItem> GetCustomerList(string search)
        {
            // If this returns one field, then we want to search in NAME, FIRSTNAME and LASTNAME with this single string
            string[] fields = search.Split(new[] { " ", "," }, StringSplitOptions.None);
            int numberOfFields = fields.Length;

            int recordFrom = loadedCount + 1;
            int recordTo = loadedCount + maxRowsAtEachQuery;

            List<CustomerListItem> customers = new List<CustomerListItem>();
            if (numberOfFields == 1)
            {
                string searchString = fields[0];
                customers = Providers.CustomerData.SearchWithAddress(dlgEntry, searchString, recordFrom, recordTo, false, ResolveSort(sortBy), !sortAsc);
            }
            else
            {
                Name name = NameParser.ParseName(search, dlgEntry.Settings.NameFormat == NameFormat.LastNameFirst);

                string displayName = dlgEntry.Settings.NameFormatter.Format(name);

                customers = Providers.CustomerData.Search(dlgEntry, displayName, name.First, name.Last, recordFrom, recordTo, false, ResolveSort(sortBy), !sortAsc);
            }
            if (customerList == null)
            {
                customerList = customers;
            }
            return customers;
        }

        private void AddInvoiceCustomerColumn()
        {
            if(lvCustomer.Columns.Count == 4)
            {
                Column col = new Column(Properties.Resources.InvoiceAccount)
                {
                    RelativeSize = 19,
                    Clickable = false
                };

                lvCustomer.Columns.Add(col);
            }
        }

        private void RemoveInvoiceCustomerColumn()
        {
            if(lvCustomer.Columns.Count == 5)
            {
                lvCustomer.Columns.RemoveAt(4);
            }
        }

        private CustomerSorting ResolveSort(string sortBy)
        {
            CustomerSorting sortCode;
            switch (sortBy)
            {
                case "AccountNum":
                    sortCode = CustomerSorting.ID;
                    break;

                case "Name":
                    if (dlgEntry.Settings.NameFormat == NameFormat.FirstNameFirst)
                    {
                        sortCode = CustomerSorting.FirstName;
                    }
                    else if (dlgEntry.Settings.NameFormat == NameFormat.LastNameFirst)
                    {
                        sortCode = CustomerSorting.LastName;
                    }
                    else
                    {
                        sortCode = CustomerSorting.Name;
                    }
                    break;

                case "Address":
                    sortCode = CustomerSorting.Address;
                    break;

                default:
                    sortCode = CustomerSorting.Name;
                    break;

            }
            return sortCode;
        }

        private void frmCustomerSearch_Load(object sender, EventArgs e)
        {
            this.Top = dlgSettings.MainFormInfo.MainWindowTop;
            this.Left = dlgSettings.MainFormInfo.MainWindowLeft;

            if (!DesignMode)
            {
                Scanner.ScannerMessageEvent += Scanner_ScannerMessageEvent;
                Scanner.ReEnableForScan();
                MSR.MSRMessageEvent += MSR_MSRMessageEvent;
                MSR.EnableForSwipe();
            }

            customerList = GetCustomerList(tbSearch.Text);
            loadedCount = customerList.Count;

            PopulateListView(customerList);

            lvCustomer.SetSortColumn(nameColumnIndex, true);

            //Get a list of all locally saved customers - customerList only has the first 30 customers displayed in the customer search dialog
            List<CustomerListItem> locallySavedCustomers = Providers.CustomerData.GetLocallySavedCustomers(dlgEntry);

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.TransferCustomer), (locallySavedCustomers.Any() && dlgSettings.SiteServiceProfile.CheckCustomer));
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.EditCustomer), lvCustomer.Selection.Count > 0);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Select), lvCustomer.Selection.Count > 0);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), tbSearch.Text != "");

            SetFormFocus();
        }

        private void MSR_MSRMessageEvent(string track2)
        {
            string track = StringExtensions.TrackBeforeSeparator(track2, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);

            BarCode barcode = Interfaces.Services.BarcodeService(dlgEntry).ProcessBarcode(dlgEntry, BarCode.BarcodeEntryType.ManuallyEntered, track);

            if (barcode.InternalType == BarcodeInternalType.Customer && barcode.CustomerId != null)
            {
                tbSearch.Text = barcode.CustomerId;
            }
            else
            {
                tbSearch.Text = track;
            }

            touchKeyboard1_EnterPressed(this, EventArgs.Empty);
            CheckSingleRecordMatch();
        }

        private void Scanner_ScannerMessageEvent(ScanInfo scanInfo)
        {
            try
            {
                Scanner.DisableForScan();
                BarCode barcode = Interfaces.Services.BarcodeService(dlgEntry).ProcessBarcode(dlgEntry, scanInfo);

                if (barcode.InternalType == BarcodeInternalType.Customer && barcode.CustomerId != null)
                {
                    tbSearch.Text = barcode.CustomerId;
                }
                else
                {
                    tbSearch.Text = scanInfo.ScanDataLabel;
                }

                touchKeyboard1_EnterPressed(this, EventArgs.Empty);
                CheckSingleRecordMatch();
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (!DesignMode)
            {
                Scanner.ScannerMessageEvent -= Scanner_ScannerMessageEvent;
                Scanner.DisableForScan();
                MSR.MSRMessageEvent -= MSR_MSRMessageEvent;
                MSR.DisableForSwipe();
            }

            base.OnClosed(e);
        }

        private void CheckSingleRecordMatch()
        {
            if (loadedCount == 1 && ((RecordIdentifier)lvCustomer.Rows[0].Tag).StringValue == tbSearch.Text)
            {
                lvCustomer.Selection.Set(0);
                SelectCustomer();
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SelectCustomer();
        }

        private void SelectCustomer(CustomerListItem toSelect)
        {
            SelectedCustomer = toSelect;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void SelectCustomer()
        {
            if (customerList.Count > 0 && lvCustomer.Selection.FirstSelectedRow >= 0)
            {
                SelectCustomer(customerList[lvCustomer.Selection.FirstSelectedRow]);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbSearch.Text = "";
            loadedCount = 0;
            lvCustomer.SetSortColumn(nameColumnIndex, true);
            lastSearch = "";
            sortBy = "Name";
            sortAsc = true;

            customerList = GetCustomerList(lastSearch);
            loadedCount = customerList.Count;
            PopulateListView(customerList);
        }

        private void btnNewCustomer_Click(object sender, EventArgs e)
        {
            Customer newCustomer = new Customer();
            if (Interfaces.Services.CustomerService(dlgEntry).AddNewWithDialog(dlgEntry, ref newCustomer))
            {
                if (!returnNewCustomer)
                {
                    tbSearch.Text = newCustomer.FirstName + " " + newCustomer.LastName;
                    lastSearch = tbSearch.Text;
                    sortBy = "Name";
                    sortAsc = true;
                    customerList = GetCustomerList(tbSearch.Text);

                    PopulateListView(customerList);
                    if (customerList.Count == 1)
                    {
                        // Add new customer to selection to proceed with the original SelectCustomer
                        lvCustomer.Selection.AddRows(0, 0);
                    }

                    // Select new customer and exit
                    SelectCustomer();
                }
                else
                {
                    SelectCustomer(newCustomer);
                }

            }
            else
            {
                //If new customer was not added then the customer search (if any) is cleared.
                tbSearch.Text = "";
                loadedCount = 0;
                sortBy = "Name";
                sortAsc = true;
                customerList = GetCustomerList(tbSearch.Text);
                loadedCount = customerList.Count;
                PopulateListView(customerList);

            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                ICustomerService service = (ICustomerService)dlgEntry.Service(ServiceType.CustomerService);

                RecordIdentifier customerID = (RecordIdentifier)lvCustomer.Selection[0].Tag;

                if (service.EditWithDialog(dlgEntry, customerID, transaction))
                {
                    CustomerListItem editedCustomer = Providers.CustomerData.SearchWithAddress(dlgEntry, customerID.StringValue, 0, loadedCount, true, CustomerSorting.ID, false).SingleOrDefault(x => x.ID == customerID);

                    if (editedCustomer != null)
                    {
                        Row row = lvCustomer.Selection[0];

                        row[0].Text = (string)editedCustomer.ID;
                        row[1].Text = editedCustomer.SearchName;
                        row[2].Text = editedCustomer.FirstName != "" ? editedCustomer.GetFormattedName(dlgEntry.Settings.NameFormatter) : editedCustomer.Text;
                        row[3].Text = dlgEntry.Settings.SystemAddressFormatter.FormatSingleLine(editedCustomer.DefaultShippingAddress, dlgEntry.Cache);

                        if (!string.IsNullOrEmpty(editedCustomer.InvoiceAccountName))
                        {
                            AddInvoiceCustomerColumn();
                            row[4].Text = editedCustomer.InvoiceAccountName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(ex.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        private void btnTransferCustomers_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {                    
                    if (dlgSettings.TrainingMode)
                    {
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.TransferCustomerNotAllowedInTrainingMode, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return;
                    }

                    Interfaces.Services.DialogService(dlgEntry).UpdateStatusDialog(Properties.Resources.TransferLocalCustomers);

                    List<CustomerListItem> transferList = Providers.CustomerData.GetLocallySavedCustomers(dlgEntry);

                    ISiteServiceService service = (ISiteServiceService)dlgEntry.Service(ServiceType.SiteServiceService);
                    foreach (CustomerListItem current in transferList)
                    {
                        Customer customer = Providers.CustomerData.Get(dlgEntry,
                            current.ID,
                            UsageIntentEnum.Normal,
                            CacheType.CacheTypeApplicationLifeTime);
                        customer.LocallySaved = false;

                        //Save remotely
                        service.SaveCustomer(dlgEntry, dlgSettings.SiteServiceProfile, customer, true, false);

                        //Update locally
                        Providers.CustomerData.Save(dlgEntry, customer);
                    }
                    service.Disconnect(dlgEntry);
                    panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.TransferCustomer), false);
                    ((Control)dlgSettings.POSApp.POSMainWindow).Invoke(ApplicationFramework.PosShowStatusBarInfoDelegate, new object[] { "", null, TaskbarSection.LocalCustomers });
                }
                finally
                {
                    Interfaces.Services.DialogService(dlgEntry).CloseStatusDialog();
                }
            }
            catch
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.CouldNotTransferCustomers, MessageBoxButtons.OK, MessageDialogType.Attention);
            }
        }

        private void SetFormFocus()
        {
            tbSearch.Focus();
        }

        private void frmCustomerSearch_Shown(object sender, EventArgs e)
        {
            tbSearch.Focus();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            try
            {
                if (touchKeyboard1 != null)
                {
                    if (Width > 1400)
                    {
                        touchKeyboard1.Location = new Point((Width - 1400)/2, touchKeyboard1.Location.Y);
                        touchKeyboard1.Width = 1400;
                    }
                    else
                    {
                        touchKeyboard1.Location = new Point(11, touchKeyboard1.Location.Y);
                        touchKeyboard1.Width = Width - 22;
                    }
                }
            }
            catch
            {
                // We suppress this form sizing exeption
            }
        }

        private void frm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void lvCustomer_RowDoubleClick(object sender, RowEventArgs args)
        {
            SelectCustomer();
        }

        private void lvCustomer_VerticalScrollValueChanged(object sender, EventArgs e)
        {
            if ((lvCustomer.FirstRowOnScreen + lvCustomer.RowCountOnScreen) >= (customerList.Count - 1))
            {
                GetMoreCustomers();
            }
        }

        private void lvCustomer_HeaderClicked(object sender, ColumnEventArgs args)
        {
            lvCustomer.SetSortColumn(args.Column, lvCustomer.SortColumn != args.Column || !lvCustomer.SortedAscending);
            if (args.ColumnNumber == 0)
            {
                sortBy = "AccountNum";
            }
            else if (args.ColumnNumber == nameColumnIndex)
            {
                sortBy = "Name";
            }
            else if (args.ColumnNumber == nameColumnIndex + 1)
            {
                sortBy = "Address";
            }
            else
            {
                return;
            }
            sortAsc = lvCustomer.SortedAscending;
            loadedCount = 0;
            customerList = GetCustomerList(lastSearch);
            loadedCount = customerList.Count;
            PopulateListView(customerList);
        }

        private void touchKeyboard1_EnterPressed(object sender, EventArgs e)
        {
            if (tbSearch.Text == "" || tbSearch.Text == lastSearch)
            {
                btnSelect_Click(null, null);
            }
            else
            {

                lastSearch = tbSearch.Text;
                loadedCount = 0;
                customerList = GetCustomerList(tbSearch.Text);
                loadedCount = customerList.Count;
                PopulateListView(customerList);
            }
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

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                tbSearch.Text = StringExtensions.TrackBeforeSeparator(tbSearch.Text, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);

                e.SuppressKeyPress = true;
                e.Handled = true;
                touchKeyboard1_EnterPressed(this, EventArgs.Empty);
                CheckSingleRecordMatch();
            }
        }

        private void panel_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((int)args.Tag)
            {
                case (int)Buttons.PageUp: { lvCustomer.MovePageUp(); break; }
                case (int)Buttons.ArrowUp: { lvCustomer.MoveSelectionUp(); break; }
                case (int)Buttons.ArrowDown: { lvCustomer.MoveSelectionDown(); break; }
                case (int)Buttons.PageDown: { lvCustomer.MovePageDown(); break; }
                case (int)Buttons.NewCustomer: { btnNewCustomer_Click(null, null); break; }
                case (int)Buttons.EditCustomer: { btnEditCustomer_Click(null, null); break; }
                case (int)Buttons.TransferCustomer: { btnTransferCustomers_Click(null, null); break; }
                case (int)Buttons.Clear: { btnClear_Click(null, null); break; }
                case (int)Buttons.Select: { btnSelect_Click(null, null); break; }
                case (int)Buttons.Close: { btnClose_Click(null, null); break; }
            }
        }

        private void lvCustomer_SelectionChanged(object sender, EventArgs e)
        {
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.EditCustomer), lvCustomer.Selection.Count > 0 );
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Select), lvCustomer.Selection.Count > 0);
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), tbSearch.Text != "");
        }
    }
}

