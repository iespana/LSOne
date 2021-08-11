using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.ReceiptBrowser.Properties;

namespace LSOne.ViewPlugins.ReceiptBrowser.Views
{
    public partial class ReceiptBrowserView : ViewBase
    {
        private Setting searchBarSetting;
        private DataEntity cmbStoreSelectedItem;

        private const string sortSettingGuid = "DA7E07D8-5ACA-4D90-860C-8AD9535A42EF";
        private const string searchBarSettingGuid = "DB4B6855-9E49-4DC4-A27C-9B55DB2E0CF0";

        IPlugin reportPlugin;
        Setting sortSetting;

        public ReceiptBrowserView(RecordIdentifier staffID, string staffDescription, RecordIdentifier storeID, string storeDescription, RecordIdentifier terminalID, string terminalDescription)
            : this()
        {
        }

        public ReceiptBrowserView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Help | 
                         ViewAttributes.Close | 
                         ViewAttributes.ContextBar;

            lvReceipts.ContextMenuStrip = new ContextMenuStrip();
            lvReceipts.ContextMenuStrip.Opening += lvReceipt_Opening;

            HeaderText = Resources.ReceiptBrowser;
            searchBar1.BuddyControl = lvReceipts;
            receiptsDataScroll.PageSize = PluginEntry.DataModel.PageSize;
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.FindReceipt;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            receiptsDataScroll.Reset();

            sortSetting = PluginEntry.DataModel.Settings.GetSetting(
                PluginEntry.DataModel,
                new Guid(sortSettingGuid),
                SettingType.UISetting, lvReceipts.CreateSortSetting(0, true));

            lvReceipts.SortSetting = sortSetting.Value;

            reportPlugin = PluginEntry.Framework.FindImplementor(this, "CanDisplayXtraReport", null);

            if (reportPlugin != null)
            {
                btnView.Visible = true;
            }
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        public override void SaveUserInterface()
        {
            string newSortSetting = lvReceipts.SortSetting;

            if (newSortSetting != sortSetting.Value)
            {
                sortSetting.Value = newSortSetting;
                sortSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, new Guid(sortSettingGuid), SettingsLevel.User, sortSetting);
            }
        }

        private void LoadItems()
        {
            string sort = "";
            
            string idOrReceiptNumber = null;
            bool idOrReceiptNumberBeginsWith = true;
            int countReceipts; 
            RecordIdentifier employeeLogin = RecordIdentifier.Empty;
            RecordIdentifier storeId = RecordIdentifier.Empty;
            RecordIdentifier terminalId = RecordIdentifier.Empty;
            RecordIdentifier currency = RecordIdentifier.Empty;
            decimal paidAmount = 0;
            Date dateFrom = Date.Empty;
            Date dateTo = Date.Empty;

            lvReceipts.ClearRows();

            string[] columns = { "TRANSDATE", "RECEIPTID", "STAFF", "TERMINAL", "STORE", "PAYMENTAMOUNT" };

            if (lvReceipts.SortColumn == null)
            {
                lvReceipts.SetSortColumn(lvReceipts.Columns[0], true);
            }

            int sortColumnIndex = lvReceipts.Columns.IndexOf(lvReceipts.SortColumn);

            if (sortColumnIndex < columns.Length)
            {
                sort = columns[sortColumnIndex] + (lvReceipts.SortedAscending ? " ASC" : " DESC");
            }

            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "ReceiptNumber":
                        idOrReceiptNumber = result.StringValue;
                        idOrReceiptNumberBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    
                    case "Employee":
                        employeeLogin = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;

                    case "Store":
                        storeId = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;

                    case "Terminal":
                        terminalId = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;

                    case "DateRange":
                        if (result.Date.Checked && !result.Time.Checked)
                        {
                            dateFrom.DateTime = result.Date.Value.Date;
                        }
                        if (result.DateTo.Checked && !result.TimeTo.Checked)
                        {
                            dateTo.DateTime = result.DateTo.Value.Date.AddDays(1).AddSeconds(-1);
                        }
                        if (result.Date.Checked && result.Time.Checked)
                        {
                            dateFrom.DateTime = new DateTime(result.Date.Value.Year, result.Date.Value.Month,
                                result.Date.Value.Day, result.Time.Value.Hour, result.Time.Value.Minute,
                                result.Time.Value.Second);
                        }
                        if (result.DateTo.Checked && result.TimeTo.Checked)
                        {
                            dateTo.DateTime = new DateTime(result.DateTo.Value.Year, result.DateTo.Value.Month,
                                result.DateTo.Value.Day, result.TimeTo.Value.Hour, result.TimeTo.Value.Minute,
                                result.TimeTo.Value.Second);
                        }
                        break;
                }
            }

            var items = Providers.ReceiptData.Find(PluginEntry.DataModel,
                                                    dateFrom,
                                                    dateTo,
                                                    idOrReceiptNumber,
                                                    idOrReceiptNumberBeginsWith,
                                                    employeeLogin, 
                                                    storeId,
                                                    (string)terminalId,
                                                    currency, 
                                                    paidAmount,
                                                    receiptsDataScroll.StartRecord,
                                                    receiptsDataScroll.EndRecord + 1,
                                                    sort,
                                                    out countReceipts);
            lvReceipts.ClearRows();
            receiptsDataScroll.RefreshState(items, countReceipts);
            PopulateListView(items);
            HideProgress();

        }

        private void PopulateListView(List<ReceiptListItem> items)
        {
            Row row;
            
            IRoundingService service = (IRoundingService)PluginEntry.DataModel.Service(ServiceType.RoundingService);
            foreach (ReceiptListItem receiptData in items)
            {
                row = new Row();
                row.AddText(receiptData.TransactionDate.DateTime.ToString());
                row.AddText(receiptData.ReceiptID);
                row.AddText(receiptData.EmployeeID == "" ? "" : receiptData.EmployeeLogin + " - " + receiptData.EmployeeDescription);
                row.AddText(receiptData.TerminalID == "" ? "" : receiptData.TerminalID + " - " + receiptData.TerminalDescription);
                row.AddText(receiptData.StoreID == "" ? "" : receiptData.StoreID + " - " + receiptData.StoreDescription);
                row.AddText(service.RoundString(PluginEntry.DataModel, receiptData.PaidAmount, receiptData.Currency, true, CacheType.CacheTypeApplicationLifeTime));
                row.Tag = receiptData;
                lvReceipts.AddRow(row);
            }
            lvReceipts.AutoSizeColumns();
        }

        private void searchBar1_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, new Guid(searchBarSettingGuid), SettingType.UISetting, "");

            if (searchBarSetting.LongUserSetting != "")
            {
                searchBar1.LoadFromSetupString(searchBarSetting.Value);
            }
        }

        private void searchBar1_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar1.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, new Guid(searchBarSettingGuid), SettingsLevel.User, searchBarSetting);
            }
            ShowTimedProgress(searchBar1.GetLocalizedSavingText());
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            receiptsDataScroll.Reset();
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            searchBar1.AddCondition(new ConditionType(Resources.DateRange, "DateRange", ConditionType.ConditionTypeEnum.DateAndTimeRange, false, DateTime.Now.Date, false, DateTime.Now.Date));
            searchBar1.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Terminal, "Terminal", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.ReceiptNumber, "ReceiptNumber", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.Employee, "Employee", ConditionType.ConditionTypeEnum.Unknown));

            searchBar1_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = PluginEntry.DataModel.IsHeadOffice ? new DataEntity("", "") : cmbStoreSelectedItem ?? new DataEntity("", "");
                    args.UnknownControl.Enabled = PluginEntry.DataModel.IsHeadOffice || cmbStoreSelectedItem == null;

                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox)args.UnknownControl).RequestData += cmbStore_RequestData;
                    ((DualDataComboBox)args.UnknownControl).SelectedDataChanged += cmbStoreSelectionChanged;
                    break;

                case "Terminal":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).RequestData += cmbTerminal_RequestData;
                    break;

                case "Employee":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).DropDown += cmbEmployee__DropDown;
                    break;
            }
        }

        private void cmbEmployee__DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new EmployeeSearchPanel(PluginEntry.DataModel, e.DisplayText);
        }

        private void searchBar1_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                case "Terminal":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
                case "Employee":
                    args.Selection = ((DualDataComboBox) args.UnknownControl).SelectedData.Text;
                    break;
            }
        }

        private void searchBar1_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.HasSelection = true;
        }

        private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= cmbStore_RequestData;
                    ((DualDataComboBox)args.UnknownControl).SelectedDataChanged -= cmbStoreSelectionChanged;
                    break;
                
                case "Terminal":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= cmbTerminal_RequestData;
                    break;
                
                case "Employee":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= cmbEmployee_DropDown;
                    break;
            }
        }

        private void searchBar1_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "Store":
                    entity = Providers.StoreData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Terminal":
                    RecordIdentifier storeID = cmbStoreSelectedItem != null ? cmbStoreSelectedItem.ID : "";
                    entity = Providers.TerminalData.Get(PluginEntry.DataModel, args.Selection, storeID);
                    break;
                case "Employee":
                    Guid guid = Guid.Empty;
                    Guid.TryParse(args.Selection, out guid);
                    entity = Providers.UserData.Get(PluginEntry.DataModel, guid);
                    break;
            }
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void cmbTerminal_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;

            ((DualDataComboBox)sender).SetData(cmbStoreSelectedItem != null ? Providers.TerminalData.GetTerminals(PluginEntry.DataModel, cmbStoreSelectedItem.ID) :
                Providers.TerminalData.GetAllTerminals(PluginEntry.DataModel, true, TerminalListItem.SortEnum.STORENAME), null);
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;

            ((DualDataComboBox)sender).SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbStoreSelectionChanged(object sender, EventArgs e)
        {
            cmbStoreSelectedItem = (DataEntity)((DualDataComboBox)sender).SelectedData;
        }

        private void receiptsDataScroll_PageChanged(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void cmbEmployee_DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new EmployeeSearchPanel(
                PluginEntry.DataModel, e.DisplayText);
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        void lvReceipt_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvReceipts.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.View,
                    100,
                    btnView_Click)
            {
                //Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnView.Enabled,
                Default = true
            };
            menu.Items.Add(item);
            PluginEntry.Framework.ContextMenuNotify("ReceiptList", lvReceipts.ContextMenuStrip, this);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            ReceiptListItem item = (ReceiptListItem)lvReceipts.Selection[0].Tag;

            Reports.ReportReceipt report = new Reports.ReportReceipt(item.ID.PrimaryID, item.StoreID, item.TerminalID);

            RecordIdentifier reportID = "RECEIPT: " + (string)((ReceiptListItem)lvReceipts.Selection[0].Tag).ID;

            reportPlugin.Message(this, "ShowReport", new object[] { reportID, report, Resources.Receipt + ":" + item.ReceiptID, Resources.ReceiptSmallImage });
        }

        private void lvReceipt_SelectionChanged(object sender, EventArgs e)
        {
            btnView.Enabled = (lvReceipts.Selection.Count > 0);
        }

        private void lvReceipts_DoubleClick(object sender, EventArgs e)
        {
            if (lvReceipts.Selection.Count > 0 && btnView.Visible && btnView.Enabled)
            {
                btnView_Click(this, EventArgs.Empty);
            }
        }

        private void lvReceipt_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
        }

        private void lvReceipt_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvReceipts.SortColumn == args.Column)
            {
                lvReceipts.SetSortColumn(args.Column, !lvReceipts.SortedAscending);
            }
            else
            {
                lvReceipts.SetSortColumn(args.Column, true);
            }

            receiptsDataScroll.Reset();

            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void lvReceipt_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
        }

        private void lvReceipts_Load(object sender, EventArgs e)
        {
        }

        private void searchBar1_Load(object sender, EventArgs e)
        {
        }

        private void pnlBottom_Paint(object sender, PaintEventArgs e)
        {
        }

        private void receiptsDataScroll_Load(object sender, EventArgs e)
        {
        }
    }
}