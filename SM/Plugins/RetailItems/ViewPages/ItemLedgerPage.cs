using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.RetailItems.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    public partial class ItemLedgerPage : UserControl, ITabView
    {
        private RecordIdentifier itemID;

        private bool sortAsc = false;
        private Setting searchBarSetting;
        private Setting sortSetting;

        private const string searchBarSettingGuid = "5629bf4d-b89a-4841-aa50-7fb70220bc9b";

        private DataEntity cmbStoreSelectedItem;

        private Dictionary<string, Name> names;

        public ItemLedgerPage()
        {
            InitializeComponent();
            DoubleBuffered = true;
            lvLedger.AutoSizeColumns();
            lvLedger.SetSortColumn(0, sortAsc);
            names = new Dictionary<string, Name>();
            searchBar1.BuddyControl = lvLedger;
            if (!DesignMode && !PluginEntry.DataModel.IsHeadOffice)
            {
                cmbStoreSelectedItem = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
            }
            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            itemID = ((RetailItem)internalContext).ID;

            sortSetting = PluginEntry.DataModel.Settings.GetSetting(
                PluginEntry.DataModel,
                new Guid("D83918FD-69CB-4468-8690-48CAAB6E1C7C"),
                SettingType.UISetting, lvLedger.CreateSortSetting(0, sortAsc));

            lvLedger.SortSetting = sortSetting.Value;

            searchBar1_SearchClicked(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
            string newSortSetting = lvLedger.SortSetting;

            if (newSortSetting != sortSetting.Value)
            {
                sortSetting.Value = newSortSetting;
                sortSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, new Guid("D83918FD-69CB-4468-8690-48CAAB6E1C7C"), SettingsLevel.User, sortSetting);
            }
        }

        private void LoadLedgers()
        {
            ItemLedgerSearchParameters parameters = new ItemLedgerSearchParameters
            {
                ItemID = itemID,
                Source = ItemLedgerSearchOptions.All,
                rowFrom = itemDataScroll.StartRecord,
                rowTo = itemDataScroll.EndRecord + 1,
                SortAscending = sortAsc
            };

            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Store":
                        parameters.StoreID = ((DualDataComboBox) result.UnknownControl).SelectedData.ID;
                        break;

                    case "Terminal":
                        parameters.TerminalID = ((DualDataComboBox) result.UnknownControl).SelectedData.ID;
                        break;

                    case "Date":
                        if (result.Date.Checked)
                        {
                            parameters.FromDateTime = result.Date.Value.Date;
                        }
                        if (result.DateTo.Checked)
                        {
                            parameters.ToDateTime = result.DateTo.Value.Date.AddDays(1).AddSeconds(-1);
                        }
                        break;

                    case "Types":
                        ItemLedgerSearchOptions options = ItemLedgerSearchOptions.All;
                        if ((result.CheckedValues[0] && result.CheckedValues[1]) || (!result.CheckedValues[0] && !result.CheckedValues[1]))
                        {
                            options = ItemLedgerSearchOptions.All;
                        }
                        else if (result.CheckedValues[0])
                        {
                            options = ItemLedgerSearchOptions.Sales;
                        }
                        else if (result.CheckedValues[1])
                        {
                            options = ItemLedgerSearchOptions.Inventory;
                        }
                        parameters.Source = options;;
                        break;
                    case "Options":
                        parameters.IncludeVoided = result.CheckedValues[0];
                        parameters.DoNotAggregatePostedSales = !result.CheckedValues[1];
                        parameters.ShowObsoleteEntries = false;
                        break;
                }
            }

            if (!PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ViewInventoryForAllStores) && !PluginEntry.DataModel.IsHeadOffice)
            {
                parameters.StoreID = PluginEntry.DataModel.CurrentStoreID;
            }

            List<ItemLedger> ledgers;
            try
            {
                ledgers = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetItemLedgerList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), parameters, true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
            lvLedger.ClearRows();
            PopulateListView(ledgers);
            ((ViewBase)Parent.Parent.Parent).HideProgress();
        }

        private void PopulateListView(List<ItemLedger> ledgers)
        {
            DecimalLimit priceLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            itemDataScroll.RefreshState(ledgers);

            foreach (ItemLedger itemLedger in ledgers)
            {
                DecimalLimit unitDecimaLimit = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, itemLedger.UnitName), CacheType.CacheTypeApplicationLifeTime);

                var row = new Row();
                row.AddText(itemLedger.Time.ToString());
                switch (itemLedger.LedgerType)
                {
                    case ItemLedgerType.Adjustment:
                        row.AddText(Resources.Adjustment);
                        break;
                    case ItemLedgerType.Purchase:
                        row.AddText(Resources.Purchase);
                        break;
                    case ItemLedgerType.Sale:
                        row.AddText(Resources.Sale);
                        break;
                    case ItemLedgerType.VoidedLine:
                        row.AddText(Resources.VoidedLine);
                        break;
                    case ItemLedgerType.VoidedSale:
                        row.AddText(Resources.VoidedSale);
                        break;
                    case ItemLedgerType.StockCount:
                        row.AddText(Resources.StockCount);
                        break;
                    case ItemLedgerType.TransferIn:
                        row.AddText(Resources.TransferIn);
                        break;
                    case ItemLedgerType.TransferOut:
                        row.AddText(Resources.TransferOut);
                        break;
                    case ItemLedgerType.PostedStatement:
                        row.AddText(Resources.PostedStatement);
                        break;
                    case ItemLedgerType.StockReservation:
                        row.AddText(Resources.StockReservation);
                        break;
                    case ItemLedgerType.ParkedInventory:
                        row.AddText(Resources.ParkedInventory);
                        break;
                }
                row.AddText(itemLedger.StoreName);
                row.AddText(itemLedger.TerminalName);
                row.AddText(itemLedger.Operator);
                row.AddText((string) itemLedger.Reference);
                row.AddCell(new NumericCell(itemLedger.Quantity.FormatWithLimits(unitDecimaLimit), itemLedger.Quantity));
                row.AddText($"{itemLedger.UnitName}({itemLedger.Adjustment.FormatWithLimits(unitDecimaLimit)})" );
                row.AddCell(new NumericCell(itemLedger.CostPrice.FormatWithLimits(priceLimit), itemLedger.CostPrice));
                row.AddText(itemLedger.ReasonCode);

                lvLedger.AddRow(row);
            }

            lvLedger.AutoSizeColumns();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ItemLedgerPage();
        }

        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            ((ViewBase)Parent.Parent.Parent).ShowProgress((sender1, e1) => LoadLedgers(), ((ViewBase)Parent.Parent.Parent).GetLocalizedSearchingText());
        }

        private void lvLedger_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (args.ColumnNumber == 0)
            {
                sortAsc = !sortAsc;
                itemDataScroll.Reset();
                ((ViewBase)Parent.Parent.Parent).ShowProgress((sender1, e1) => LoadLedgers(), ((ViewBase)Parent.Parent.Parent).GetLocalizedSearchingText());
                lvLedger.SetSortColumn(args.Column, sortAsc);
                lvLedger.Invalidate();
            }
        }

        private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= cmbStoreRequestData;
                    ((DualDataComboBox)args.UnknownControl).SelectedDataChanged -= cmbStoreSelectionChanged;
                    break;
                case "Terminal":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= cmbTerminalRequestData;
                    break;
            }
        }

        private void searchBar1_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.HasSelection = true;
        }

        private void searchBar1_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                case "Terminal":
                    args.Selection = (string) ((DualDataComboBox) args.UnknownControl).SelectedData.ID;
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
            }
            ((DualDataComboBox) args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
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

                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox)args.UnknownControl).RequestData += cmbStoreRequestData;
                    ((DualDataComboBox)args.UnknownControl).SelectedDataChanged += cmbStoreSelectionChanged;
                    break;
                case "Terminal":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;

                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).RequestData += cmbTerminalRequestData;
                    break;
            }
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
            ((ViewBase)Parent.Parent.Parent).ShowTimedProgress(searchBar1.GetLocalizedSavingText());
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            if (Parent == null)
                return;
            itemDataScroll.Reset();
            ((ViewBase)Parent.Parent.Parent).ShowProgress((sender1, e1) => LoadLedgers(), ((ViewBase)Parent.Parent.Parent).GetLocalizedSearchingText());
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            searchBar1.AddCondition(new ConditionType(Resources.Date, "Date", ConditionType.ConditionTypeEnum.DateRange, true, DateTime.Now.AddMonths(-1), true, DateTime.Now));
            searchBar1.AddCondition(new ConditionType(Resources.Types, "Types", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Sales, true, Resources.Inventory, true));
            searchBar1.AddCondition(new ConditionType(Resources.Options,"Options", ConditionType.ConditionTypeEnum.Checkboxes, Resources.ShowVoided, true, Resources.ShowPostedStatements, true));
            searchBar1.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Terminal, "Terminal", ConditionType.ConditionTypeEnum.Unknown));

            searchBar1_LoadDefault(this, EventArgs.Empty);
        }

        private void cmbTerminalRequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox) sender).SkipIDColumn = true;

            ((DualDataComboBox) sender).SetData(cmbStoreSelectedItem != null ? Providers.TerminalData.GetTerminals(PluginEntry.DataModel, cmbStoreSelectedItem.ID) : 
                                                Providers.TerminalData.GetAllTerminals(PluginEntry.DataModel, true, TerminalListItem.SortEnum.STORENAME), null);
        }

        private void cmbStoreRequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox) sender).SkipIDColumn = true;

            List<DataEntity> stores = new List<DataEntity>();

            if(PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ViewInventoryForAllStores) || PluginEntry.DataModel.IsHeadOffice)
            {
                stores = Providers.StoreData.GetList(PluginEntry.DataModel);
            }
            else
            {
                stores.Add(Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID));
            }

            ((DualDataComboBox) sender).SetData(stores, null);
        }

        private void cmbStoreSelectionChanged(object sender, EventArgs e)
        {
            cmbStoreSelectedItem = (DataEntity) ((DualDataComboBox) sender).SelectedData;
        }
    }
}