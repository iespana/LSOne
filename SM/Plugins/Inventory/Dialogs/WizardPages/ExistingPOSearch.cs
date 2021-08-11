using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class ExistingPOSearch : UserControl, IWizardPage
    {
        private static Guid BarSettingID = new Guid("0C6542E5-18AC-4021-B73E-E05AD468A47E");
        WizardBase parent;
        private InventoryTypeAction inventoryTypeAction;
        private InventoryPurchaseOrderSortEnums secondarySort;
        private Setting searchBarSetting;
        private List<DataEntity> vendorsList;
        private bool hasFinishButton;
        private Store defaultStore;

        public ExistingPOSearch(WizardBase parent, InventoryTypeAction inventoryTypeAction, bool hasFinishButton) : this(parent, inventoryTypeAction)
        {
            this.hasFinishButton = hasFinishButton;
        }

        public ExistingPOSearch(WizardBase parent, InventoryTypeAction inventoryTypeAction) : base()
        {
            InitializeComponent();

            this.parent = parent;
            this.inventoryTypeAction = inventoryTypeAction;
            this.hasFinishButton = false;
            
            IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            try
            {
                vendorsList = service.GetVendorList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

            searchBar.BuddyControl = lvItems;
            searchBar.FocusFirstInput();

            defaultStore = null;
            if (!RecordIdentifier.IsEmptyOrNull(PluginEntry.DataModel.CurrentStoreID))
            {
                defaultStore = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
            }
        }

        #region IWizardPage Members

        public bool HasFinish
        {
            get { return hasFinishButton; }
        }

        public bool HasForward
        {
            get { return true; }
        }

        public Control PanelControl
        {
            get { return this; }
        }

        public void Display()
        {
            searchBar_SearchClicked(this, EventArgs.Empty);
            CheckState();
        }

        private void CheckState()
        {
            parent.NextEnabled = lvItems.Selection.Count >= 1;
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            canUseFromForwardStack = false;
            return true;
        }

        public IWizardPage RequestNextPage()
        {
            return new NewEmptyPO(parent, (PurchaseOrder)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag, inventoryTypeAction);
        }

        public void ResetControls()
        {

        }

        #endregion

        public RecordIdentifier PurchaseOrderID
        {
            get { return ((PurchaseOrder)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag).ID; }
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> statusList = new List<object>();
            statusList.Add(Resources.AllStatuses);
            statusList.Add(Resources.Open);
            statusList.Add(Resources.Posted);

            searchBar.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.ComboBox, statusList, 1, 0, false));
            searchBar.AddCondition(new ConditionType(Resources.Description, "idOrDescription", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Vendor, "Vendor", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.CreationDate, "CreationDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));
            searchBar.AddCondition(new ConditionType(Resources.DeliveryDate, "DeliveryDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));

            searchBar_LoadDefault(this, EventArgs.Empty);
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

            searchBar.GetLocalizedSavingText();
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity(null, Resources.AllStores);
                    ((DualDataComboBox)args.UnknownControl).RequestData += Store_RequestData;
                    break;
                case "Vendor":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox)args.UnknownControl).RequestData += Vendor_RequestData;
                    break;
            }
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void LoadItems()
        {
            InventoryPurchaseOrderSortEnums sort = InventoryPurchaseOrderSortEnums.None;
            secondarySort = InventoryPurchaseOrderSortEnums.None;

            List<string> idOrDescription = null;
            bool idOrDescriptionBeginsWith = true;
            PurchaseStatusEnum? status = null;
            RecordIdentifier storeID = PluginEntry.DataModel.IsHeadOffice ? null : PluginEntry.DataModel.CurrentStoreID;
            RecordIdentifier vendorID = null;
            Date deliveryDateFrom = Date.Empty;
            Date deliveryDateTo = Date.Empty;
            Date creationDateFrom = Date.Empty;
            Date creationDateTo = Date.Empty;

            InventoryPurchaseOrderSortEnums?[] columns =
            {
                InventoryPurchaseOrderSortEnums.ID,
                InventoryPurchaseOrderSortEnums.Store,
                InventoryPurchaseOrderSortEnums.Vendor,
                InventoryPurchaseOrderSortEnums.Description,
                InventoryPurchaseOrderSortEnums.Status,
                InventoryPurchaseOrderSortEnums.CreationDate,
                InventoryPurchaseOrderSortEnums.DeliveryDate,
                InventoryPurchaseOrderSortEnums.None
            };

            if (lvItems.SortColumn == null)
            {
                lvItems.SetSortColumn(lvItems.Columns[1], true);
            }

            int sortColumnIndex = lvItems.Columns.IndexOf(lvItems.SortColumn);

            if (sortColumnIndex < columns.Length)
            {
                if (columns[sortColumnIndex] == InventoryPurchaseOrderSortEnums.None)
                {
                    sort = InventoryPurchaseOrderSortEnums.None;
                }
                else
                {
                    sort = (InventoryPurchaseOrderSortEnums)((int)columns[sortColumnIndex]);
                }
            }

            SetSecondarySort(lvItems.SortColumn.SecondarySortColumn, lvItems.SortedAscending);

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "idOrDescription":
                        idOrDescription = new List<string> { result.StringValue };
                        idOrDescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Status":
                        switch (result.ComboSelectedIndex)
                        {
                            case 1:
                                status = PurchaseStatusEnum.Open;
                                break;
                            case 2:
                                status = PurchaseStatusEnum.Closed;
                                break;
                            case 3:
                                status = PurchaseStatusEnum.PartiallyRecieved;
                                break;
                            case 4:
                                status = PurchaseStatusEnum.Placed;
                                break;
                            default:
                                status = null;
                                break;
                        }
                        break;
                    case "Store":
                        storeID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Vendor":
                        vendorID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "DeliveryDate":
                        deliveryDateFrom = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        deliveryDateTo = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
                        break;
                    case "CreationDate":
                        creationDateFrom = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        creationDateTo = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
                        break;
                }
            }

            if (!PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrdersForAllStores) && !PluginEntry.DataModel.IsHeadOffice)
            {
                storeID = PluginEntry.DataModel.CurrentStoreID;
            }

            int itemCount;

            try
            {
                lvItems.ClearRows();

                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                List<PurchaseOrder> purchaseOrders = service.PurchaseOrderAdvancedSearch(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    true,
                    0,
                    PluginEntry.DataModel.PageSize,
                    sort,
                    !lvItems.SortedAscending,
                    out itemCount,
                    idOrDescription,
                    idOrDescriptionBeginsWith,
                    storeID,
                    vendorID,
                    status,
                    deliveryDateFrom,
                    deliveryDateTo,
                    creationDateFrom,
                    creationDateTo
                    );

                Row row;

                foreach (PurchaseOrder purchaseOrder in purchaseOrders)
                {
                    row = new Row();
                    row.AddText((string)purchaseOrder.PurchaseOrderID);
                    row.AddText(purchaseOrder.StoreName);
                    row.AddText(purchaseOrder.VendorName);
                    row.AddText(purchaseOrder.Description);
                    row.AddText(purchaseOrder.PurchaseStatusText);
                    row.AddCell(new DateTimeCell(purchaseOrder.CreatedDate.ToShortDateString(), purchaseOrder.CreatedDate));
                    row.AddCell(new DateTimeCell(purchaseOrder.DeliveryDate.ToShortDateString(), purchaseOrder.DeliveryDate));

                    row.Tag = purchaseOrder;

                    lvItems.AddRow(row);
                }

                lvItems.AutoSizeColumns();
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
        }

        private void Store_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            List<DataEntity> stores = new List<DataEntity>();

            var hasAccessToAllStores = GetHasAccessToAllStores();
            if (hasAccessToAllStores)
            {
                stores = Providers.StoreData.GetList(PluginEntry.DataModel);
                stores.Insert(0, new DataEntity(null, Resources.AllStores));
            }
            else
            {
                stores.Add(Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID));
            }

            ((DualDataComboBox)sender).SetData(stores, null);
        }

        private void Vendor_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(vendorsList, null);
        }

        private void SetSecondarySort(short secondarySortColumn, bool sortedAscending)
        {
            switch (secondarySortColumn)
            {
                case -1:
                    secondarySort = InventoryPurchaseOrderSortEnums.None;
                    break;
                case 2:
                    secondarySort = InventoryPurchaseOrderSortEnums.Description;
                    break;
                default:
                    secondarySort = InventoryPurchaseOrderSortEnums.Description;
                    break;
            }
            if (!sortedAscending && secondarySort != InventoryPurchaseOrderSortEnums.None)
            {
                secondarySort += 100;
            }
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                case "Vendor":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                case "Vendor":
                    args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Store_RequestData;
                    break;
                case "Vendor":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Vendor_RequestData;
                    break;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "Store":
                    var hasAccessToAllStores = GetHasAccessToAllStores();
                    entity = hasAccessToAllStores ? Providers.StoreData.Get(PluginEntry.DataModel, args.Selection) ?? new DataEntity(null, Resources.AllStores) :
                                                    new DataEntity(defaultStore.ID, defaultStore.Text);
                    break;
                case "Vendor":
                    entity = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), args.Selection, true);
                    break;
            }
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void lvItems_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvItems.SortColumn == args.Column)
            {
                lvItems.SetSortColumn(args.Column, !lvItems.SortedAscending);
            }
            else
            {
                lvItems.SetSortColumn(args.Column, true);
            }

            LoadItems();
        }

        private bool GetHasAccessToAllStores()
        {
            return PluginEntry.DataModel.IsHeadOffice || PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrdersForAllStores);
        }
    }
}