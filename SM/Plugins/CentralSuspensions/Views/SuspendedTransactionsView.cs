using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.ViewPlugins.CentralSuspensions.Properties;
using LSOne.DataLayer.GenericConnector;
using System.Drawing;
using System.Linq;
using LSOne.Controls.Rows;

namespace LSOne.ViewPlugins.CentralSuspensions.Views
{
    public partial class SuspendedTransactionsView : ViewBase
    {
        private static Guid BarSettingID = new Guid("32b0df29-2629-4882-92bf-47b38c89d570");
        List<SuspendedTransaction> suspendedTransactionInfo;
        List<SuspendedTransactionAnswer> suspendedTransactionAnswer;
        RecordIdentifier selectedID = "";
        RecordIdentifier suspendedTransactionID;

        private SiteServiceProfile siteServiceProfile;
        private Setting searchBarSetting;
        private DataEntity defaultStore;

        ISiteServiceService service = null;

        public SuspendedTransactionsView(RecordIdentifier suspendedTransactionID)
            : this()
        {
            this.suspendedTransactionID = suspendedTransactionID;
        }

        public SuspendedTransactionsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help |
                ViewAttributes.Audit;

            //HeaderIcon = Properties.Resources.Transaction16;
            HeaderText = Resources.SuspendedTransactions;

            defaultStore = null;
            if (PluginEntry.DataModel.CurrentStoreID != null && PluginEntry.DataModel.CurrentStoreID != RecordIdentifier.Empty)
            {
                searchBar.DefaultNumberOfSections = 2;
                defaultStore = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
            }

            lvSuspendedTransactions.ContextMenuStrip = new ContextMenuStrip();
            lvSuspendedTransactions.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            lvSuspendedTransactions.SetSortColumn(0, true);

            lvSuspendedTransactions.Columns[0].Tag = SuspendedTransaction.SortEnum.TransactionDate;
            lvSuspendedTransactions.Columns[1].Tag = SuspendedTransaction.SortEnum.SuspensionTypeID;
            lvSuspendedTransactions.Columns[2].Tag = SuspendedTransaction.SortEnum.TransactionID;
            lvSuspendedTransactions.Columns[3].Tag = SuspendedTransaction.SortEnum.Description;
            lvSuspendedTransactions.Columns[4].Tag = SuspendedTransaction.SortEnum.StoreID;
            lvSuspendedTransactions.Columns[5].Tag = SuspendedTransaction.SortEnum.TerminalID;

            btnRemove.Enabled = false;

            searchBar.BuddyControl = lvSuspendedTransactions;
            searchBar.FocusFirstInput();

            var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("SuspendTransaction", RecordIdentifier.Empty, Resources.SuspendedTransactions, false));
            contexts.Add(new AuditDescriptor("POSSUSPENDTRANSADDINFO", RecordIdentifier.Empty, Resources.SuspendedTransaction, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.SuspensionsType;
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
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "SuspendedTransaction":
                    ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
                    break;
                case "SuspensionsTypeAdditionalInfo":
                    ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
                    break;
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvSuspendedTransactions.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
                    Resources.Delete,
                    100,
                    new EventHandler(btnRemove_Click))
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnRemove.Enabled
            };
            
            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteSuspendedTransaction(suspendedTransactionID);
        }

        private void lvSuspendedTransactions_SelectionChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = (lvSuspendedTransactions.Selection.Count >= 1) && PluginEntry.DataModel.HasPermission(Permission.ManageSuspensionSettings);

            selectedID = (lvSuspendedTransactions.Selection.Count == 1) ? ((SuspendedTransaction)lvSuspendedTransactions.Selection[0].Tag).ID : RecordIdentifier.Empty;
            lvSuspendedTransAddInfo.ClearRows();

            if (lvSuspendedTransactions.Selection.Count > 0)
            {
                if (!lblGroupHeader.Visible)
                {
                    lblGroupHeader.Visible = true;
                    lvSuspendedTransAddInfo.Visible = true;
                    lblNoSelection.Visible = false;
                }
            }
            else if (lblGroupHeader.Visible)
            {
                lblGroupHeader.Visible = false;
                lvSuspendedTransAddInfo.Visible = false;
                lblNoSelection.Visible = true;
            }

            RecordIdentifier selectedEntity = (lvSuspendedTransactions.Selection.Count == 1) ? ((SuspendedTransaction)lvSuspendedTransactions.Selection[0].Tag).ID : RecordIdentifier.Empty;

            if(selectedEntity != RecordIdentifier.Empty)
            {
                try
                {
                    service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                    suspendedTransactionAnswer = service.GetSuspendedTransactionAnswers(PluginEntry.DataModel, siteServiceProfile, selectedEntity, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                    return;
                }

                foreach (SuspendedTransactionAnswer item in suspendedTransactionAnswer)
                {
                    Row row = new Row();
                    row.AddText(item.Prompt);
                    row.AddText(item.ToString(PluginEntry.DataModel.Settings.LocalizationContext));
                    row.Tag = item;
                    lvSuspendedTransAddInfo.AddRow(row);
                }
            }

            lvSuspendedTransAddInfo.AutoSizeColumns();
        }

        private void LoadItems()
        {
            lvSuspendedTransactions.ClearRows();
            lvSuspendedTransAddInfo.ClearRows();

            lblGroupHeader.Visible = false;
            lvSuspendedTransAddInfo.Visible = false;
            lblNoSelection.Visible = true;

            RecordIdentifier suspendedtransactionTypeID = RecordIdentifier.Empty;
            RecordIdentifier storeID = RecordIdentifier.Empty;
            RecordIdentifier terminalID = RecordIdentifier.Empty;
            Date dateFrom = Date.Empty;
            Date dateTo = Date.Empty;

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "SuspensionType":
                        suspendedtransactionTypeID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;

                    case "Store":
                        storeID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;

                    case "Terminal":
                        terminalID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;

                    case "Date":
                        if (result.Date.Checked)
                        {
                            dateFrom.DateTime = result.Date.Value.Date;
                        }

                        if (result.DateTo.Checked)
                        {
                            dateTo.DateTime = result.DateTo.Value.Date.AddDays(1).AddSeconds(-1);
                        }
                        break;
                }
            }

            try
            {
                service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                suspendedTransactionInfo = service.GetSuspendedTransactionList(PluginEntry.DataModel,
                                                                               siteServiceProfile,
                                                                               suspendedtransactionTypeID,
                                                                               storeID,
                                                                               terminalID,
                                                                               dateFrom,
                                                                               dateTo,
                                                                               MapColumnToEnum(lvSuspendedTransactions.Columns.IndexOf(lvSuspendedTransactions.SortColumn)),
                                                                               !lvSuspendedTransactions.SortedAscending,
                                                                               true);
            }
            catch (Exception ex)
            {
                MessageBox.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                return;
            }

            foreach (SuspendedTransaction item in suspendedTransactionInfo)
            {
                Row row = new Row();
                row.AddText(item.TransactionDate.ToShortDateString());
                row.AddText(item.SuspensionTypeName);
                row.AddText(item.ID.ToString());
                row.AddText(item.Text);
                row.AddText(item.StoreName);
                row.AddText(item.TerminalName);
                row.Tag = item;
                lvSuspendedTransactions.AddRow(row);
            }

            lvSuspendedTransactions_SelectionChanged(null, null);
            lvSuspendedTransactions.AutoSizeColumns();
            HideProgress();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            SuspendedTransaction selectedEntity = (SuspendedTransaction)lvSuspendedTransactions.Selection[0].Tag;
            RecordIdentifier selectedID = selectedEntity.ID;

            if (lvSuspendedTransactions.Selection.Count == 1)
            {
                PluginOperations.DeleteSuspendedTransaction(selectedID);
            }
            else
            {
                PluginOperations.DeleteSuspendedTransactions(GetSelectedTrasactions());
            }
        }

        private List<IDataEntity> GetSelectedTrasactions()
        {
            List<IDataEntity> selectedData = new List<IDataEntity>();

            for (int i = 0; i < lvSuspendedTransactions.Selection.Count; i++)
            {
                selectedData.Add((IDataEntity)lvSuspendedTransactions.Selection[i].Tag);
            }

            return selectedData;
        }

        private void lvSuspendedTransactions_HeaderClick(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvSuspendedTransactions.SortColumn == args.Column)
            {
                lvSuspendedTransactions.SetSortColumn(args.Column, !lvSuspendedTransactions.SortedAscending);
            }
            else
            {
                lvSuspendedTransactions.SetSortColumn(args.Column, true);
            }

            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private SuspendedTransaction.SortEnum MapColumnToEnum(int column)
        {
            switch (column)
            {
                case 0: return SuspendedTransaction.SortEnum.TransactionDate;
                case 1: return SuspendedTransaction.SortEnum.SuspensionTypeID;
                case 2: return SuspendedTransaction.SortEnum.TransactionID;
                case 3: return SuspendedTransaction.SortEnum.Description;
                case 4: return SuspendedTransaction.SortEnum.StoreID;
                case 5: return SuspendedTransaction.SortEnum.TerminalID;
                default: return SuspendedTransaction.SortEnum.TransactionID;
            }
        }

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.SuspensionType, "SuspensionType", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Terminal, "Terminal", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Date, "Date", ConditionType.ConditionTypeEnum.DateRange, true, DateTime.Today.AddMonths(-1), true, DateTime.Today));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar_SearchOptionChanged(object sender, EventArgs e)
        {
            searchBar_SearchClicked(sender, e);
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            if (args.TypeKey == "SuspensionType" || args.TypeKey == "Store" || args.TypeKey == "Terminal")
            {
                args.UnknownControl = new DualDataComboBox();
                args.UnknownControl.Size = new Size(200, 21);
                args.MaxSize = 200;
                args.AutoSize = false;
                ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                
                if(args.TypeKey == "Store" && defaultStore != null)
                {
                    ((DualDataComboBox)args.UnknownControl).SelectedData = defaultStore;
                }
                else
                {
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
                }

                switch (args.TypeKey)
                {
                    case "SuspensionType":
                        ((DualDataComboBox)args.UnknownControl).RequestData += new EventHandler(SuspensionType_DropDown);
                        break;
                    case "Store":
                        ((DualDataComboBox)args.UnknownControl).RequestData += new EventHandler(Store_DropDown);
                        ((DualDataComboBox)args.UnknownControl).SelectedDataChanged += new EventHandler(Store_SelectedDataChanged);
                        break;
                    case "Terminal":
                        ((DualDataComboBox)args.UnknownControl).RequestData += new EventHandler(Terminal_DropDown);
                        break;
                }
            }
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            if (args.TypeKey == "SuspensionType" || args.TypeKey == "Store" || args.TypeKey == "Terminal")
            {
                args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            if (args.TypeKey == "SuspensionType" || args.TypeKey == "Store" || args.TypeKey == "Terminal")
            {
                args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
            }
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            if (args.TypeKey == "SuspensionType")
            {
                ((DualDataComboBox)args.UnknownControl).RequestData -= SuspensionType_DropDown;
            }

            if (args.TypeKey == "Store")
            {
                ((DualDataComboBox)args.UnknownControl).RequestData -= Store_DropDown;
                ((DualDataComboBox)args.UnknownControl).SelectedDataChanged -= Store_SelectedDataChanged;
            }

            if (args.TypeKey == "Terminal")
            {
                ((DualDataComboBox)args.UnknownControl).RequestData -= Terminal_DropDown;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;

            switch (args.TypeKey)
            {
                case "SuspensionType":
                    entity = Providers.SuspendedTransactionData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Store":
                    entity = Providers.StoreData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Terminal":
                    entity = Providers.TerminalData.Get(PluginEntry.DataModel, args.Selection, RecordIdentifier.Empty);
                    break;
            }

            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void Terminal_DropDown(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            RecordIdentifier selectedStoreId = GetSelectedStore();
            ((DualDataComboBox)sender).SetData(selectedStoreId != RecordIdentifier.Empty 
                ? Providers.TerminalData.GetTerminals(PluginEntry.DataModel, selectedStoreId) 
                : Providers.TerminalData.GetAllTerminals(PluginEntry.DataModel, true, TerminalListItem.SortEnum.STORENAME), null);
        }

        private void Store_DropDown(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void SuspensionType_DropDown(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.SuspendedTransactionTypeData.GetList(PluginEntry.DataModel), null);
        }

        private void Store_SelectedDataChanged(object sender, EventArgs e)
        {
            SearchParameterResult searchBand = searchBar.SearchParameterResults.SingleOrDefault(x => x.ParameterKey == "Terminal");

            if(searchBand != null) // repopulate Terminals when selecting a store
            {
                ((DualDataComboBox)searchBand.UnknownControl).SelectedData = new DataEntity("", "");
                Terminal_DropDown(searchBand.UnknownControl, e);
            }
        }

        private RecordIdentifier GetSelectedStore()
        {
            // This should not be called while the search bar is initializing
            SearchParameterResult searchBand = searchBar.SearchParameterResults.SingleOrDefault(x => x.ParameterKey == "Store");
            return searchBand == null ? RecordIdentifier.Empty : ((DualDataComboBox)searchBand.UnknownControl).SelectedData.ID;
        }
    }
}