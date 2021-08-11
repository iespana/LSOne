using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Dialogs;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.WinFormsTouch
{
    public partial class SalesPersonDialog : TouchBaseForm
    {        
        private List<Employee> allSalesPerson;
        private List<Employee> allSalesPersonInCurrentStore;
        private bool checkToken;

        public Employee SelectedSalesPerson { get; set; }

        public enum Buttons
        {
            CurrentUser,
            CurrentStore,
            RecentUsers
        }

        public SalesPersonDialog()
        {
            InitializeComponent();

            banner.BannerText = Properties.Resources.SelectASalesPerson;
            SetButtons();
            allSalesPerson = new List<Employee>();
            checkToken = true;

            if (!DesignMode)
            {
                tbSearch.StartCharacter = DLLEntry.Settings.HardwareProfile.StartTrack1;
                tbSearch.EndCharacter = DLLEntry.Settings.HardwareProfile.EndTrack1;
                tbSearch.Seperator = DLLEntry.Settings.HardwareProfile.Separator1;
                tbSearch.TrackSeperation = TrackSeperation.Before;
            }

            lvSelection.SetSortColumn(1, true);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                Scanner.ScannerMessageEvent += Scanner_ScannerMessageEvent;
                Scanner.ReEnableForScan();
                MSR.MSRMessageEvent += MSR_MSRMessageEvent;
                MSR.EnableForSwipe();
            }

            List<POSUser> users = Providers.POSUserData.GetList(DLLEntry.DataModel, 
                        RecordIdentifier.Empty, 
                        UsageIntentEnum.Normal, 
                        CacheType.CacheTypeApplicationLifeTime);

            foreach (POSUser user in users)
            {
                Employee employee = new Employee(user, DLLEntry.DataModel.Settings.NameFormatter);
                allSalesPerson.Add(employee);
            }

            LoadCurrentStoreUsers();
        }

        private void Scanner_ScannerMessageEvent(ScanInfo scanInfo)
        {
            try
            {
                BarCode barcode = Interfaces.Services.BarcodeService(DLLEntry.DataModel).ProcessBarcode(DLLEntry.DataModel, scanInfo);

                if(barcode.InternalType == BarcodeInternalType.SalesPerson && barcode.SalespersonId != null)
                {
                    checkToken = true;
                    tbSearch.Text = barcode.SalespersonId;
                    SearchList();
                    checkToken = false;
                }
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

        void MSR_MSRMessageEvent(string track2)
        {
            string track = StringExtensions.TrackBeforeSeparator(track2, DLLEntry.Settings.HardwareProfile.StartTrack1, DLLEntry.Settings.HardwareProfile.Separator1, DLLEntry.Settings.HardwareProfile.EndTrack1);

            BarCode barcode = Interfaces.Services.BarcodeService(DLLEntry.DataModel).ProcessBarcode(DLLEntry.DataModel, BarCode.BarcodeEntryType.ManuallyEntered, track);

            if (barcode.InternalType == BarcodeInternalType.SalesPerson && barcode.SalespersonId != null)
            {
                checkToken = true;
                tbSearch.Text = barcode.SalespersonId;
                SearchList();
                checkToken = false;
            }
            else
            {
                tbSearch.Text = track;
            }
        }

        private void touchKeyboard1_EnterPressed(object sender, EventArgs e)
        {
            navigationBtns.SetButtonEnabled(Conversion.ToStr((int)NavigationBtnEnum.Clear), tbSearch.Text.Trim() != "");
            SearchList();
        }

        private void touchKeyboard1_ObtainCultureName(object sender, CultureEventArguments args)
        {
            if (DLLEntry.DataModel.Settings == null)
            {
                return;
            }
            if (((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.KeyboardCode != "")
            {
                args.CultureName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.KeyboardCode;
                args.LayoutName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.KeyboardCode;
                args.LayoutName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.KeyboardLayoutName;
            }
        }

        private void SearchList()
        {
            List<Employee> displayedEmployees = LoadList(tbSearch.Text.Trim());
            tbSearch.Focus();
            tbSearch.SelectAll();

            if(displayedEmployees.Count == 1 && displayedEmployees[0].Login.StringValue == tbSearch.Text.Trim())
            {
                lvSelection.Selection.Set(0);
                SetSelected();
            }
        }

        private void SetSelected()
        {
            if (lvSelection.Selection.Count > 0)
            {
                SelectedSalesPerson = (Employee)lvSelection.Row(lvSelection.Selection.FirstSelectedRow).Tag;
                DLLEntry.Settings.AddEmployeeToRecentList(SelectedSalesPerson);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void SetButtons()
        {
            navigationBtns.AddButton(Properties.Resources.CurrentUser, Buttons.CurrentUser, Conversion.ToStr((int)Buttons.CurrentUser), TouchButtonType.OK);
            navigationBtns.AddButton(Properties.Resources.StoreUsers, Buttons.CurrentStore, Conversion.ToStr((int)Buttons.CurrentStore));
            navigationBtns.AddButton(Properties.Resources.RecentUsers, Buttons.RecentUsers, Conversion.ToStr((int)Buttons.RecentUsers));
            navigationBtns.SetAllFunctionButtons();

            navigationBtns.SetButtonEnabled(Conversion.ToStr((int)NavigationBtnEnum.Select), false);
            navigationBtns.SetButtonEnabled(Conversion.ToStr((int)NavigationBtnEnum.Clear), false);
        }

        private void navigationBtns_Click(object sender, ScrollButtonEventArguments args)
        {
            if(args.Tag is NavigationBtnEnum)
            {
                NavigationBtnEnum btn = (NavigationBtnEnum)args.Tag;

                switch (btn)
                {
                    case NavigationBtnEnum.Clear:
                        tbSearch.Text = "";
                        navigationBtns.SetButtonEnabled(Conversion.ToStr((int)NavigationBtnEnum.Clear), false);
                        SearchList();
                        break;
                    case NavigationBtnEnum.Select:
                        SetSelected();
                        break;
                    case NavigationBtnEnum.Cancel:
                        DialogResult = DialogResult.Cancel;
                        Close();
                        break;
                    case NavigationBtnEnum.Search:
                        navigationBtns.SetButtonEnabled(Conversion.ToStr((int)NavigationBtnEnum.Clear), tbSearch.Text.Trim() != "");
                        SearchList();
                        break;
                    default:
                        break;
                }
            }
            else if (args.Tag is Buttons)
            {
                Buttons btn = (Buttons)args.Tag;

                switch (btn)
                {
                    case Buttons.CurrentUser:
                        POSUser currentUser = Providers.POSUserData.Get(DLLEntry.DataModel, DLLEntry.DataModel.CurrentStaffID, UsageIntentEnum.Normal);

                        if (currentUser != null)
                        {
                            Employee employee = new Employee(currentUser, DLLEntry.DataModel.Settings.NameFormatter);
                            SelectedSalesPerson = employee;
                            DLLEntry.Settings.AddEmployeeToRecentList(SelectedSalesPerson);
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.ErrorRetrievingCurrentUserDescription, Properties.Resources.ErrorRetrievingCurrentUser, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        }
                        break;
                    case Buttons.CurrentStore:
                        LoadCurrentStoreUsers();
                        break;
                    case Buttons.RecentUsers:
                        LoadList("", DLLEntry.Settings.GetRecentEmployees());
                        break;
                    default:
                        break;
                }
            }
        }

        private void LoadCurrentStoreUsers()
        {
            if(allSalesPersonInCurrentStore == null)
            {
                allSalesPersonInCurrentStore = new List<Employee>();
                List<POSUser> users = Providers.POSUserData.GetList(DLLEntry.DataModel,
                        DLLEntry.DataModel.CurrentStoreID,
                        UsageIntentEnum.Normal,
                        CacheType.CacheTypeApplicationLifeTime);

                foreach (POSUser user in users)
                {
                    Employee employee = new Employee(user, DLLEntry.DataModel.Settings.NameFormatter);
                    allSalesPersonInCurrentStore.Add(employee);
                }
            }

            LoadList("", allSalesPersonInCurrentStore);
        }

        private List<Employee> LoadList(string searchString, List<Employee> displayList = null)
        {            
            lvSelection.ClearRows();

            if(displayList == null)
            {
                displayList = new List<Employee>();

                if (searchString != "")
                {
                    displayList = allSalesPerson.FindAll(f => ((string)f.ID).ToLower().Contains(searchString) 
                                                                || ((string)f.Login).ToLower().Contains(searchString) 
                                                                || f.Name.ToLower().Contains(searchString)
                                                                || f.NameOnReceipt.ToLower().Contains(searchString));
                }

                if (searchString == "")
                {
                    displayList = allSalesPerson;
                }

                if (displayList == null)
                {
                    return new List<Employee>();
                }

                displayList = displayList.OrderBy(x => x.Name).ToList();
            }
            
            foreach (Employee employee in displayList)
            {
                var row = new Row();                
                
                row.AddText((string)employee.Login);
                row.AddText(employee.Name); 
                row.AddText(employee.NameOnReceipt);
                row.Tag = employee;
                
                lvSelection.AddRow(row);
            }

            lvSelection.AutoSizeColumns(true);
            return displayList;
        }

        private void lvSelection_DoubleClick(object sender, EventArgs e)
        {
            SetSelected();
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                if(!string.IsNullOrWhiteSpace(tbSearch.Text))
                {
                    checkToken = false;
                    tbSearch.Text = StringExtensions.TrackBeforeSeparator(tbSearch.Text, DLLEntry.Settings.HardwareProfile.StartTrack1, DLLEntry.Settings.HardwareProfile.Separator1, DLLEntry.Settings.HardwareProfile.EndTrack1);
                    checkToken = true;

                    BarCode barcode = Interfaces.Services.BarcodeService(DLLEntry.DataModel).ProcessBarcode(DLLEntry.DataModel, BarCode.BarcodeEntryType.ManuallyEntered, tbSearch.Text.Trim());

                    if (barcode.InternalType == BarcodeInternalType.SalesPerson && barcode.SalespersonId != null)
                    {
                        checkToken = true;
                        tbSearch.Text = barcode.SalespersonId;
                        checkToken = false;
                    }
                }

                navigationBtns.SetButtonEnabled(Conversion.ToStr((int)NavigationBtnEnum.Clear), tbSearch.Text.Trim() != "");
                SearchList();                
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {            
            if (!checkToken || (tbSearch.Text == "" || tbSearch.LastTrack == null))
            {
                return;
            }

            //Get the user from the token                
            AuthenticationToken token = AuthenticationToken.FromRawFeed(RecordIdentifier.Empty, RecordIdentifier.Empty, "", tbSearch.Text);                

            if (token != null)
            {
                token = Providers.AuthenticationTokenData.GetUserFromToken(DLLEntry.DataModel, token);
                if (token.UserID == "")
                {
                    tbSearch.Text = "";
                    return;
                }

                User usr = Providers.UserData.Get(DLLEntry.DataModel, (Guid)token.UserID);
                if (usr != null)
                {
                    checkToken = false;
                    tbSearch.Text = (string)usr.Login;
                    SearchList();
                    checkToken = true;
                }
            }            
        }

        private void lvSelection_SelectionChanged(object sender, EventArgs e)
        {
            navigationBtns.SetButtonEnabled(Conversion.ToStr((int)NavigationBtnEnum.Select), lvSelection.Selection.Count > 0);
        }
    }
}
