using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.Store.Views
{
    public partial class StoresView : ViewBase
    {
        private static Guid BarSettingID = new Guid("94CC75D2-F96E-41F5-AEB3-752341563A03");
        private Setting searchBarSetting;
        RecordIdentifier selectedID = "";

        public StoresView(RecordIdentifier storeId)
            :base()
        {
            selectedID = storeId;
        }

        public StoresView()
        {
            InitializeComponent();

            HeaderText = Properties.Resources.Stores;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.StoreEdit);

            lvStores.ContextMenuStrip = new ContextMenuStrip();
            lvStores.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.StoreEdit);

            lvStores.Columns[0].Tag = StoreSorting.ID;
            lvStores.Columns[1].Tag = StoreSorting.Name;
            lvStores.Columns[2].Tag = StoreSorting.City;
            lvStores.Columns[3].Tag = StoreSorting.Region;
            lvStores.Columns[4].Tag = StoreSorting.DefaultCustomer;
            lvStores.Columns[5].Tag = StoreSorting.Currency;
            lvStores.Columns[6].Tag = StoreSorting.FunctionalityProfile;
            lvStores.Columns[7].Tag = StoreSorting.FormProfile;
            lvStores.Columns[8].Tag = StoreSorting.SiteServiceProfile;
            lvStores.Columns[9].Tag = StoreSorting.TouchButtons;
            lvStores.Columns[10].Tag = StoreSorting.SalesTaxGroup;
            lvStores.Columns[11].Tag = StoreSorting.PriceSetting;
            lvStores.Columns[12].Tag = StoreSorting.Terminals;

            lvStores.SetSortColumn(0, true);

            searchBar.BuddyControl = lvStores;
            searchBar.FocusFirstInput();
        }        

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Stores", 0, Properties.Resources.Stores, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Stores;
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

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Store":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedID = changeIdentifier;
                    }                    

                    LoadItems();
                    break;
            }
        }

        private void LoadItems()
        {
            lvStores.ClearRows();

            if (lvStores.SortColumn == null)
            {
                lvStores.SetSortColumn(lvStores.Columns[0], true);
            }

            StoreListSearchFilterExtended filter = GetSearchFilter();
            List<StoreListItemExtended> stores = Providers.StoreData.SearchExtended(PluginEntry.DataModel, filter);

            foreach (var store in stores)
            {
                Row row = new Row();

                string priceSetting = "-";

                switch (store.PriceSetting)
                {
                    case DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.UsePriceGroupSettings:
                        priceSetting = Properties.Resources.UsePriceGroupSetting;
                        break;
                    case DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.PricesIncludeTax:
                        priceSetting = Properties.Resources.WithTax;
                        break;
                    case DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.PricesExcludeTax:
                        priceSetting = Properties.Resources.WithoutTax;
                        break;
                }

                row.AddText((string)store.ID);
                row.AddText(store.Text);
                row.AddText(store.City);
                row.AddText(FormatListViewText(store.Region));
                row.AddText(FormatListViewText(store.DefaultCustomer));
                row.AddText(FormatListViewText(store.Currency));
                row.AddText(FormatListViewText(store.FunctionalityProfile));
                row.AddText(FormatListViewText(store.FormProfile));
                row.AddText(FormatListViewText(store.SiteServiceProfile));
                row.AddText(FormatListViewText(store.TouchButtons));
                row.AddText(FormatListViewText(store.SalesTaxGroup));
                row.AddText(priceSetting);
                row.AddText(FormatListViewText(store.TerminalsCount));

                row.Tag = store.ID;

                lvStores.AddRow(row);

                if (selectedID == (RecordIdentifier)row.Tag)
                {
                    lvStores.Selection.Set(lvStores.RowCount - 1);
                }
            }

            lvStores.AutoSizeColumns();
            lvStores_SelectionChanged(this, EventArgs.Empty);

            HideProgress();
        }

        private string FormatListViewText(object obj)
        {
            if((obj is int && (int)obj == 0) || (obj is string && (string)obj == ""))
            {
                return "-";
            }

            return obj.ToString();
        }

        private StoreListSearchFilterExtended GetSearchFilter()
        {
            StoreListSearchFilterExtended filter = new StoreListSearchFilterExtended
            {
                Sort = (StoreSorting)lvStores.SortColumn.Tag,
                SortBackwards = !lvStores.SortedAscending
            };

            foreach(SearchParameterResult result in searchBar.SearchParameterResults)
            {
                switch(result.ParameterKey)
                {
                    case "Description":
                        filter.DescriptionOrID = result.StringValue;
                        filter.DescriptionOrIDBeginsWith = result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith;
                        break;
                    case "City":
                        filter.City = result.StringValue;
                        filter.CityBeginsWith = result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith;
                        break;
                    case "Region":
                        filter.RegionID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Customer":
                        filter.CustomerID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Currency":
                        filter.CurrencyCode = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "FunctionalityProfile":
                        filter.FunctionalityProfileID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "FormProfile":
                        filter.FormProfileID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "SiteServiceProfile":
                        filter.SiteServiceProfileID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "TouchButtons":
                        filter.TouchButtonsLayoutID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "SalesTaxGroup":
                        filter.SalesTaxGroupID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                }
            }

            return filter;
        }

        private void lvStores_SelectionChanged(object sender, EventArgs e)
        {
            bool hasViewPermission = PluginEntry.DataModel.HasPermission(Permission.StoreView);
            bool hasEditPermission = PluginEntry.DataModel.HasPermission(Permission.StoreEdit);

            btnsEditAddRemove.EditButtonEnabled = lvStores.Selection.Count > 0 && hasViewPermission;

            // We only want to be able to delete stores from the head office
            btnsEditAddRemove.RemoveButtonEnabled =
                lvStores.Selection.Count > 0
                && hasEditPermission
                && PluginEntry.DataModel.CurrentStoreID.IsEmpty;
        }

        private void lvStores_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvStores.SortColumn == args.Column)
            {
                lvStores.SetSortColumn(args.Column, !lvStores.SortedAscending);
            }
            else
            {
                lvStores.SetSortColumn(args.Column, true);
            }

            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowStore((RecordIdentifier)lvStores.Selection[0].Tag);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewStore(this, EventArgs.Empty);          
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteStore((RecordIdentifier)lvStores.Selection[0].Tag);
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvStores.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   new EventHandler(btnsEditAddRemove_AddButtonClicked));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("StoresList", lvStores.ContextMenuStrip, lvStores);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.StoreEdit))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Add, ContextButtons.GetAddButtonImage(), btnsEditAddRemove_AddButtonClicked), 10);
                }
            }
        }

        protected override void OnClose()
        {
            
        }

        private void lvStores_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            args.UnknownControl = new DualDataComboBox();
            args.UnknownControl.Size = new Size(200, 21);
            args.MaxSize = 200;
            args.AutoSize = false;
            ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
            ((DualDataComboBox)args.UnknownControl).SkipIDColumn = true;
            ((DualDataComboBox)args.UnknownControl).RequestClear += Control_RequestClear;
            ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity(RecordIdentifier.Empty, "");

            switch (args.TypeKey)
            {
                case "Region":
                    ((DualDataComboBox)args.UnknownControl).RequestData += Region_RequestData;
                    break;
                case "Customer":
                    ((DualDataComboBox)args.UnknownControl).RequestData += Customer_RequestData;
                    break;
                case "Currency":
                    ((DualDataComboBox)args.UnknownControl).RequestData += Currency_RequestData;
                    break;
                case "FunctionalityProfile":
                    ((DualDataComboBox)args.UnknownControl).RequestData += FunctionalityProfile_RequestData;
                    break;
                case "FormProfile":
                    ((DualDataComboBox)args.UnknownControl).RequestData += FormProfile_RequestData;
                    break;
                case "SiteServiceProfile":
                    ((DualDataComboBox)args.UnknownControl).RequestData += SiteServiceProfile_RequestData;
                    break;
                case "TouchButtons":
                    ((DualDataComboBox)args.UnknownControl).RequestData += TouchButtons_RequestData;
                    break;
                case "SalesTaxGroup":
                    ((DualDataComboBox)args.UnknownControl).RequestData += SalesTaxGroup_RequestData;
                    break;
            }
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.HasSelection = !RecordIdentifier.IsEmptyOrNull(((DualDataComboBox)args.UnknownControl).SelectedData.ID);
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            ((DualDataComboBox)args.UnknownControl).RequestClear -= Control_RequestClear;

            switch (args.TypeKey)
            {
                case "Region":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Region_RequestData;
                    break;
                case "Customer":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Customer_RequestData;
                    break;
                case "Currency":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Currency_RequestData;
                    break;
                case "FunctionalityProfile":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= FunctionalityProfile_RequestData;
                    break;
                case "FormProfile":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= FormProfile_RequestData;
                    break;
                case "SiteServiceProfile":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= SiteServiceProfile_RequestData;
                    break;
                case "TouchButtons":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= TouchButtons_RequestData;
                    break;
                case "SalesTaxGroup":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= SalesTaxGroup_RequestData;
                    break;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;

            switch (args.TypeKey)
            {
                case "Region":
                    entity = Providers.RegionData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Customer":
                    entity = Providers.CustomerData.Get(PluginEntry.DataModel, args.Selection, UsageIntentEnum.Minimal);
                    break;
                case "Currency":
                    entity = Providers.CurrencyData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "FunctionalityProfile":
                    entity = Providers.FunctionalityProfileData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "FormProfile":
                    entity = args.Selection == string.Empty ? new DataEntity(RecordIdentifier.Empty, "") : Providers.FormProfileData.Get(PluginEntry.DataModel, Guid.Parse(args.Selection));
                    break;
                case "SiteServiceProfile":
                    entity = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "TouchButtons":
                    entity = Providers.TouchLayoutData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "SalesTaxGroup":
                    entity = Providers.SalesTaxGroupData.Get(PluginEntry.DataModel, args.Selection);
                    break;
            }

            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity(RecordIdentifier.Empty, "");
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

            ShowTimedProgress(searchBar.GetLocalizedSavingText());
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Properties.Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Properties.Resources.City, "City", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Properties.Resources.Region, "Region", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Properties.Resources.Customer, "Customer", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Properties.Resources.Currency, "Currency", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Properties.Resources.FunctionalityProfile, "FunctionalityProfile", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Properties.Resources.FormProfile, "FormProfile", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Properties.Resources.SiteServiceProfile, "SiteServiceProfile", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Properties.Resources.TouchButtons, "TouchButtons", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Properties.Resources.SalesTaxGroup, "SalesTaxGroup", ConditionType.ConditionTypeEnum.Unknown));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void Control_RequestClear(object sender, EventArgs e)
        {
            
        }

        private void SalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.SalesTaxGroupData.GetList(PluginEntry.DataModel), null);
        }

        private void TouchButtons_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.TouchLayoutData.GetList(PluginEntry.DataModel), null);
        }

        private void SiteServiceProfile_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.SiteServiceProfileData.GetList(PluginEntry.DataModel), null);
        }

        private void FormProfile_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.FormProfileData.GetList(PluginEntry.DataModel), null);
        }

        private void FunctionalityProfile_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.FunctionalityProfileData.GetList(PluginEntry.DataModel), null);
        }

        private void Currency_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        private void Customer_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.CustomerData.GetList(PluginEntry.DataModel, "", DataLayer.DataProviders.Customers.CustomerSorting.Name, false), null);
        }

        private void Region_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.RegionData.GetList(PluginEntry.DataModel, DataLayer.BusinessObjects.StoreManagement.Region.SortEnum.Description, false), null);
        }
    }
}
