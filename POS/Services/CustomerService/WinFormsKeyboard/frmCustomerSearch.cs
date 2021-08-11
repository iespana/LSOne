using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSOne.Controls;
using LSOne.Controls.Columns;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.POS.Core;
using LSOne.POS.Processes.WinControlsKeyboard;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using ListView = LSOne.Controls.ListView;

namespace LSOne.Services.WinFormsKeyboard
{
    /// <summary>
    /// Summary description for frmCustomerSearch.
    /// </summary>
    public class frmCustomerSearch : frmKeyboardBase
    {
        private const int maxRowsAtEachQuery = 30;
        private int loadedCount = 0;
        private string sortBy = "Name";
        private bool sortAsc = true;
        private int nameColumnIndex = 2;
        private string lastSearch = "";
        private KeyboardButton btnTransfereLocalCustomers;
        private List<CustomerListItem> customerList;
        private PanelControl panelControl1;
        private TextBox tbSearch;
        private TouchKeyboard touchKeyboard1;
        private ListView lvCustomer;
        private Column colAccount;
        private Column colOrgID;
        private Column colName;
        private Column colAddress;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public CustomerListItem SelectedCustomer { get; private set; }

        public frmCustomerSearch()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            
            try
            {

                //
                // Get all text through the Translation function in the ApplicationLocalizer
                //
                // TextID's for frmCustomerSearch are reserved at 5020 - 5039
                // In use now are ID's: 5021 - 5031
                //

                //Set the size of the form the same as the main form
                this.Width = DLLEntry.Settings.MainFormInfo.MainWindowWidth;
                this.Height = DLLEntry.Settings.MainFormInfo.MainWindowHeight;
                touchKeyboard1.BuddyControl = tbSearch;

                if (!(DLLEntry.Settings.CultureInfo.Name == "is" || DLLEntry.Settings.CultureInfo.Name == "is-IS"))
                {
                    lvCustomer.Columns.RemoveAt(1);
                    nameColumnIndex--;
                }
                
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomerSearch));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.touchKeyboard1 = new TouchKeyboard();
            this.lvCustomer = new ListView();
            this.colAccount = new Column();
            this.colOrgID = new Column();
            this.colName = new Column();
            this.colAddress = new Column();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // keyboardButtonControl
            // 
            resources.ApplyResources(this.keyboardButtonControl, "keyboardButtonControl");
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tbSearch);
            this.panelControl1.Controls.Add(this.touchKeyboard1);
            this.panelControl1.Controls.Add(this.lvCustomer);
            resources.ApplyResources(this.panelControl1, "panelControl1");
            this.panelControl1.Name = "panelControl1";
            // 
            // tbSearch
            // 
            resources.ApplyResources(this.tbSearch, "tbSearch");
            this.tbSearch.Name = "tbSearch";
            // 
            // touchKeyboard1
            // 
            resources.ApplyResources(this.touchKeyboard1, "touchKeyboard1");
            this.touchKeyboard1.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard1.BuddyControl = null;
            this.touchKeyboard1.Name = "touchKeyboard1";
            this.touchKeyboard1.TabStop = false;
            this.touchKeyboard1.EnterPressed += new System.EventHandler(this.touchKeyboard1_Enter);
            this.touchKeyboard1.ObtainCultureName += new CultureNameHandler(this.touchKeyboard1_ObtainCultureName);
            // 
            // lvCustomer
            // 
            resources.ApplyResources(this.lvCustomer, "lvCustomer");
            this.lvCustomer.BuddyControl = null;
            this.lvCustomer.Columns.Add(this.colAccount);
            this.lvCustomer.Columns.Add(this.colOrgID);
            this.lvCustomer.Columns.Add(this.colName);
            this.lvCustomer.Columns.Add(this.colAddress);
            this.lvCustomer.ContentBackColor = System.Drawing.Color.White;
            this.lvCustomer.DefaultRowHeight = ((short)(50));
            this.lvCustomer.EvenRowColor = System.Drawing.Color.White;
            this.lvCustomer.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lvCustomer.HeaderHeight = ((short)(27));
            this.lvCustomer.Name = "lvCustomer";
            this.lvCustomer.OddRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCustomer.RowLineColor = System.Drawing.Color.Transparent;
            this.lvCustomer.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.lvCustomer.TouchScroll = true;
            this.lvCustomer.VerticalScrollbar = false;
            this.lvCustomer.HeaderClicked += new HeaderDelegate(this.lvCustomer_HeaderClicked);
            this.lvCustomer.RowDoubleClick += new RowClickDelegate(this.lvCustomer_RowDoubleClick);
            this.lvCustomer.VerticalScrollValueChanged += new System.EventHandler(this.lvCustomer_VerticalScrollValueChanged);
            // 
            // colAccount
            // 
            this.colAccount.AutoSize = false;
            this.colAccount.Clickable = true;
            this.colAccount.DefaultStyle = null;
            resources.ApplyResources(this.colAccount, "colAccount");
            this.colAccount.MaximumWidth = ((short)(0));
            this.colAccount.MinimumWidth = ((short)(10));
            this.colAccount.RelativeSize = 19;
            this.colAccount.Sizable = true;
            this.colAccount.Tag = null;
            this.colAccount.Width = ((short)(139));
            // 
            // colOrgID
            // 
            this.colOrgID.AutoSize = false;
            this.colOrgID.Clickable = true;
            this.colOrgID.DefaultStyle = null;
            resources.ApplyResources(this.colOrgID, "colOrgID");
            this.colOrgID.MaximumWidth = ((short)(0));
            this.colOrgID.MinimumWidth = ((short)(10));
            this.colOrgID.RelativeSize = 18;
            this.colOrgID.Sizable = true;
            this.colOrgID.Tag = null;
            this.colOrgID.Width = ((short)(127));
            // 
            // colName
            // 
            this.colName.AutoSize = false;
            this.colName.Clickable = true;
            this.colName.DefaultStyle = null;
            resources.ApplyResources(this.colName, "colName");
            this.colName.MaximumWidth = ((short)(0));
            this.colName.MinimumWidth = ((short)(10));
            this.colName.RelativeSize = 30;
            this.colName.Sizable = true;
            this.colName.Tag = null;
            this.colName.Width = ((short)(242));
            // 
            // colAddress
            // 
            this.colAddress.AutoSize = false;
            this.colAddress.Clickable = true;
            this.colAddress.DefaultStyle = null;
            resources.ApplyResources(this.colAddress, "colAddress");
            this.colAddress.MaximumWidth = ((short)(0));
            this.colAddress.MinimumWidth = ((short)(10));
            this.colAddress.RelativeSize = 33;
            this.colAddress.Sizable = true;
            this.colAddress.Tag = null;
            this.colAddress.Width = ((short)(278));
            // 
            // frmCustomerSearch
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelControl1);
            this.Name = "frmCustomerSearch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_FormClosing);
            this.Load += new System.EventHandler(this.frmCustomerSearch_Load);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion


        private void frmCustomerSearch_Load(object sender, EventArgs e)
        {
            this.Top = DLLEntry.Settings.MainFormInfo.MainWindowTop;
            this.Left = DLLEntry.Settings.MainFormInfo.MainWindowLeft;

            GenerateFormButtons();

            

            customerList = WinFormsTouch.CustomerSearchDialog.GetCustomerList("", loadedCount, loadedCount + maxRowsAtEachQuery, sortBy, sortAsc);

            PopulateListView(customerList);
            loadedCount = maxRowsAtEachQuery;

            lvCustomer.SetSortColumn(nameColumnIndex, true);
        }

        private void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //keyboard.ResetDefaultKeyboard();
        }

        private void PopulateListView(List<CustomerListItem> items)
        {
            lvCustomer.ClearRows();
            foreach (CustomerListItem item in items)
            {
                var row = new Row();
                row.AddText(item.AccountNumber);
                if (nameColumnIndex == 2)
                {
                    row.AddText(item.IdentificationNumber);
                }
                row.AddText(DLLEntry.DataModel.Settings.NameFormatter.Format(item.Name));
                row.AddText(DLLEntry.DataModel.Settings.SystemAddressFormatter.FormatSingleLine(item.DefaultAddress, DLLEntry.DataModel.Cache));
                lvCustomer.AddRow(row);
            }
        }

        private void AddToListView(List<CustomerListItem> items)
        {
            foreach (CustomerListItem item in items)
            {
                var row = new Row();
                row.AddText(item.AccountNumber);
                if (nameColumnIndex == 2)
                {
                    row.AddText(item.IdentificationNumber);
                }
                row.AddText(DLLEntry.DataModel.Settings.NameFormatter.Format(item.Name));
                row.AddText(DLLEntry.DataModel.Settings.SystemAddressFormatter.FormatSingleLine(item.DefaultAddress, DLLEntry.DataModel.Cache));
                lvCustomer.AddRow(row);
            }
        }

        private void GetMoreCustomers()
        {
            if (loadedCount % maxRowsAtEachQuery == 0)
            {
                List<CustomerListItem> list = WinFormsTouch.CustomerSearchDialog.GetCustomerList(lastSearch, loadedCount + 1, loadedCount + maxRowsAtEachQuery + 1, sortBy, sortAsc);
                AddToListView(list);
                customerList.AddRange(list);
                loadedCount += maxRowsAtEachQuery;
            }
        }

        private void SelectCustomer()
        {
            if (customerList.Count > 0 && lvCustomer.Selection.FirstSelectedRow >= 0)
            {
                SelectedCustomer = customerList[lvCustomer.Selection.FirstSelectedRow];
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void txtSearchString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tbSearch.Text.Trim() != "")
                {
                    // If a search string has been entered then we search for that string...
                    lastSearch = tbSearch.Text;
                    loadedCount = 0;
                    customerList = WinFormsTouch.CustomerSearchDialog.GetCustomerList(tbSearch.Text, loadedCount, loadedCount + maxRowsAtEachQuery, sortBy, sortAsc);
                    loadedCount = customerList.Count;
                    PopulateListView(customerList);  
                }
                else
                {
                    // If a search string has not been entered, Enter selects from the grid
                    SelectCustomer();
                }

            }
            else if (e.KeyCode == Keys.Up)
            {
                btnUp_KeyboardButtonClickEvent(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.Down)
            {
                btnDown_KeyboardButtonClickEvent(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                btnPageDown_KeyboardButtonClickEvent(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                btnPageUp_KeyboardButtonClickEvent(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.F1)
            {
                btnSelect_KeyboardButtonClickEvent(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.F2)
            {
                btnPageUp_KeyboardButtonClickEvent(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.F3)
            {
                btnPageDown_KeyboardButtonClickEvent(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.F4)
            {
                btnUp_KeyboardButtonClickEvent(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.F5)
            {
                btnDown_KeyboardButtonClickEvent(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.F6)
            {
                btnTransfereLocalCustomers_KeyboardButtonClickEvent(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.F7)
            {
                btnClear_KeyboardButtonClickEvent(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.F8)
            {
                btnClose_KeyboardButtonClickEvent(this, new EventArgs());
            }
        }

        #region Keyboard buttons

        private void GenerateFormButtons()
        {
            try
            {
                // Create the buttons for the form...
                // F1
                KeyboardButton btnSelect = new KeyboardButton(Properties.Resources.Select, null);
                // F2
                KeyboardButton btnPageUp = new KeyboardButton(Properties.Resources.PageUp, null);
                // F3
                KeyboardButton btnPageDown = new KeyboardButton(Properties.Resources.PageDown, null);
                // F4
                KeyboardButton btnUp = new KeyboardButton(Properties.Resources.Up, null);
                // F5
                KeyboardButton btnDown = new KeyboardButton(Properties.Resources.Down, null);
                // F6
                btnTransfereLocalCustomers = new KeyboardButton(Properties.Resources.SearchCriteria, null);
                // F7
                KeyboardButton btnClear = new KeyboardButton(Properties.Resources.Clear, null);
                // F8
                KeyboardButton btnClose = new KeyboardButton(Properties.Resources.Close, null);


                // Generate event handlers for the buttons...
                // F1
                btnSelect.KeyboardButtonClickEvent += new KeyboardButton.KeyboardButtonClickDelegate(btnSelect_KeyboardButtonClickEvent);
                // F2
                btnPageUp.KeyboardButtonClickEvent += new KeyboardButton.KeyboardButtonClickDelegate(btnPageUp_KeyboardButtonClickEvent);
                // F3
                btnPageDown.KeyboardButtonClickEvent += new KeyboardButton.KeyboardButtonClickDelegate(btnPageDown_KeyboardButtonClickEvent);
                // F4
                btnUp.KeyboardButtonClickEvent += new KeyboardButton.KeyboardButtonClickDelegate(btnUp_KeyboardButtonClickEvent);
                // F5
                btnDown.KeyboardButtonClickEvent += new KeyboardButton.KeyboardButtonClickDelegate(btnDown_KeyboardButtonClickEvent);
                // F6
                btnTransfereLocalCustomers.KeyboardButtonClickEvent += new KeyboardButton.KeyboardButtonClickDelegate(btnTransfereLocalCustomers_KeyboardButtonClickEvent);
                // F7
                btnClear.KeyboardButtonClickEvent += new KeyboardButton.KeyboardButtonClickDelegate(btnClear_KeyboardButtonClickEvent);
                // F8
                btnClose.KeyboardButtonClickEvent += new KeyboardButton.KeyboardButtonClickDelegate(btnClose_KeyboardButtonClickEvent);


                // Add the buttons to the arraylist
                ArrayList buttons = new ArrayList();
                buttons.Add(btnSelect);                 // F1
                buttons.Add(btnPageUp);                 // F2
                buttons.Add(btnPageDown);               // F3
                buttons.Add(btnUp);                     // F4
                buttons.Add(btnDown);                   // F5
                buttons.Add(btnTransfereLocalCustomers);         // F6
                buttons.Add(btnClear);                  // F7
                buttons.Add(btnClose);                  // F8

                btnTransfereLocalCustomers.Visible = Providers.CustomerData.LocallySavedCustomersExist(DLLEntry.DataModel);

                // Feed the arraylist to the control
                keyboardButtonControl.ShowMenuButtons(buttons);
            }
            catch (Exception)
            {

            }
        }

        void btnTransfereLocalCustomers_KeyboardButtonClickEvent(object sender, EventArgs e)
        {
            List<CustomerListItem> transferList = Providers.CustomerData.GetLocallySavedCustomers(DLLEntry.DataModel);
            try
            {
                IStoreServerService service = (IStoreServerService)DLLEntry.DataModel.Service(ServiceType.SiteServiceService);
                foreach (CustomerListItem current in transferList)
                {
                    Customer customer = Providers.CustomerData.Get(DLLEntry.DataModel,
                        current.ID,
                        UsageIntentEnum.Normal,
                        CacheType.CacheTypeApplicationLifeTime);
                    customer.LocallySaved = false;
                    //Save remotly
                    service.SaveCustomer(DLLEntry.DataModel, customer, true, false);
                    //Update locally
                    Providers.CustomerData.Save(DLLEntry.DataModel, customer);
                }
                keyboardButtonControl.Visible = false;
            }
            catch
            {
                LSOne.Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.CouldNotTransferCustomers, MessageBoxButtons.OK, MessageDialogType.Attention);
            }
        }

        void btnClose_KeyboardButtonClickEvent(object sender, EventArgs e)
        {
            Close();
        }

        void btnClear_KeyboardButtonClickEvent(object sender, EventArgs e)
        {
            tbSearch.Clear();
            customerList = WinFormsTouch.CustomerSearchDialog.GetCustomerList("", 0, maxRowsAtEachQuery, sortBy, sortAsc);
            loadedCount = customerList.Count;
            PopulateListView(customerList);
            lvCustomer.SetSortColumn(nameColumnIndex, true);
            lastSearch = "";
            sortBy = "Name";
            sortAsc = true;
        }

        void btnSelect_KeyboardButtonClickEvent(object sender, EventArgs e)
        {
            SelectCustomer();
        }

        void btnDown_KeyboardButtonClickEvent(object sender, EventArgs e)
        {
            lvCustomer.MoveSelectionDown();
        }

        void btnUp_KeyboardButtonClickEvent(object sender, EventArgs e)
        {
            lvCustomer.MoveSelectionUp();
        }

        void btnPageDown_KeyboardButtonClickEvent(object sender, EventArgs e)
        {
            lvCustomer.ScrollPageDown();
        }

        void btnPageUp_KeyboardButtonClickEvent(object sender, EventArgs e)
        {
            lvCustomer.ScrollPageUp();
        }
        #endregion

        private void grCustomers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                btnSelect_KeyboardButtonClickEvent(null, null);
                e.Handled = true;
            }
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
            lvCustomer.SetSortColumn(args.Column, lvCustomer.SortColumn == args.Column ? !lvCustomer.SortedAscending : true);
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
            customerList = WinFormsTouch.CustomerSearchDialog.GetCustomerList(lastSearch, 0, maxRowsAtEachQuery, sortBy, sortAsc);
            loadedCount = customerList.Count;
            PopulateListView(customerList);
        }

        private void touchKeyboard1_Enter(object sender, EventArgs e)
        {
            txtSearchString_KeyDown(this, new KeyEventArgs(Keys.Enter));
        }

        private void touchKeyboard1_ObtainCultureName(object sender, CultureEventArguments args)
        {
            if (DLLEntry.Settings.POSUser.KeyboardCode != "")
            {
                args.CultureName = DLLEntry.Settings.POSUser.KeyboardCode;
                args.LayoutName = DLLEntry.Settings.POSUser.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = DLLEntry.Settings.Store.KeyboardCode;
                args.LayoutName = DLLEntry.Settings.Store.KeyboardLayoutName;
            }
        }
    }
}

