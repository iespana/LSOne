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
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class GoodsReceivingDocumentDialog : DialogBase
    {
        RecordIdentifier ID;
        private SiteServiceProfile siteServiceProfile;
        private Setting searchBarSetting;
        private static Guid BarSettingID = new Guid("B7C5EFB1-5C0D-45B7-9777-61C390A8B999");
        private List<DataEntity> vendorsList;

        public GoodsReceivingDocumentDialog()
        {
            ID = RecordIdentifier.Empty;            

            InitializeComponent();
            
            siteServiceProfile = PluginOperations.GetSiteServiceProfile();

            searchBar.BuddyControl = lvPurchaseOrders;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            searchBar.FocusFirstInput();

            IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            try
            {
                vendorsList = service.GetVendorList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

            searchBar_SearchClicked(this, EventArgs.Empty);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier SelectedID
        {
            get { return ID; }
        }

        private void LoadPurchaseOrders()
        {
            List<string> idOrDescription = null;
            bool idOrDescriptionBeginsWith = true;
            RecordIdentifier storeID = PluginEntry.DataModel.IsHeadOffice ? null : PluginEntry.DataModel.CurrentStoreID;
            RecordIdentifier vendorID = null;
            Date deliveryDateFrom = Date.Empty;
            Date deliveryDateTo = Date.Empty;
            Date creationDateFrom = Date.Empty;
            Date creationDateTo = Date.Empty;

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "idOrDescription":
                        idOrDescription = new List<string> { result.StringValue };
                        idOrDescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
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

            int itemCount;

            try
            {
                lvPurchaseOrders.ClearRows();

                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                List<PurchaseOrder> purchaseOrders = service.PurchaseOrderAdvancedSearch(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    true,
                    0,
                    PluginEntry.DataModel.PageSize,
                    InventoryPurchaseOrderSortEnums.ID, 
                    false,
                    out itemCount,
                    idOrDescription,
                    idOrDescriptionBeginsWith,
                    storeID,
                    vendorID,
                    PurchaseStatusEnum.Open,
                    deliveryDateFrom,
                    deliveryDateTo,
                    creationDateFrom,
                    creationDateTo,
                    true
                    );

                Row row;

                foreach (PurchaseOrder purchaseOrder in purchaseOrders)
                {
                    row = new Row();
                    row.AddText((string)purchaseOrder.PurchaseOrderID);
                    row.AddText(purchaseOrder.Description);
                    row.AddText(purchaseOrder.StoreName);
                    row.AddText(purchaseOrder.VendorName);
                    row.AddCell(new DateTimeCell(purchaseOrder.CreatedDate.ToShortDateString(), purchaseOrder.CreatedDate));
                    row.AddCell(new DateTimeCell(purchaseOrder.DeliveryDate.ToShortDateString(), purchaseOrder.DeliveryDate));

                    row.Tag = purchaseOrder;

                    lvPurchaseOrders.AddRow(row);
                }

                lvPurchaseOrders.AutoSizeColumns();
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
        }

        private void Store_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            List<DataEntity> stores = Providers.StoreData.GetList(PluginEntry.DataModel);
            stores.Insert(0, new DataEntity(null, Resources.AllStores));
            ((DualDataComboBox)sender).SetData(stores, null);
        }

        private void Vendor_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(vendorsList, null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                GoodsReceivingDocument goodsReceivingDocument;

                goodsReceivingDocument = new GoodsReceivingDocument();
                goodsReceivingDocument.PurchaseOrderID = (string)((PurchaseOrder)lvPurchaseOrders.Row(lvPurchaseOrders.Selection.FirstSelectedRow).Tag).PurchaseOrderID;
                goodsReceivingDocument.GoodsReceivingID = tbGoodsReceivingDocumentID.Text;

                var service = (IInventoryService) PluginEntry.DataModel.Service(ServiceType.InventoryService);

                service.SaveGoodsReceivingDocument(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument, true);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "GoodsReceivingDocument", goodsReceivingDocument.ID, goodsReceivingDocument);

                ID = goodsReceivingDocument.ID;

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            LoadPurchaseOrders();
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> statusList = new List<object>();
            //statusList.Add(Resources.AllStatuses);
            statusList.Add(Resources.Open);
            statusList.Add(Resources.Closed);
            statusList.Add(Resources.PartiallyReceived);
            statusList.Add(Resources.Placed);

            //searchBar.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.ComboBox, statusList, 1));
            searchBar.AddCondition(new ConditionType(Resources.Description, "idOrDescription", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Vendor, "Vendor", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.CreationDate, "CreationDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));
            searchBar.AddCondition(new ConditionType(Resources.DeliveryDate, "DeliveryDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));            
            searchBar_LoadDefault(this, EventArgs.Empty);
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

                    if (PluginEntry.DataModel.IsHeadOffice)
                    {
                        ((DualDataComboBox)args.UnknownControl).Enabled = true;
                        ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity(null, Resources.AllStores);
                        ((DualDataComboBox)args.UnknownControl).RequestData += Store_RequestData;
                    }
                    else
                    {                        
                        ((DualDataComboBox)args.UnknownControl).SelectedData = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
                        ((DualDataComboBox)args.UnknownControl).Enabled = false;                        
                    }
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
                    if (PluginEntry.DataModel.IsHeadOffice)
                    {
                        entity = Providers.StoreData.Get(PluginEntry.DataModel, args.Selection) ?? new DataEntity(null, Resources.AllStores);
                    }
                    else
                    {
                        entity = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
                    }
                    break;
                case "Vendor":
                    entity = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), args.Selection, true);
                    break;
            }
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void lvPurchaseOrders_SelectionChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = lvPurchaseOrders.Selection.Count > 0;

            if (lvPurchaseOrders.Selection.Count > 0)
            {
                tbGoodsReceivingDocumentID.Text = (string)((PurchaseOrder)(lvPurchaseOrders.Row(lvPurchaseOrders.Selection.FirstSelectedRow).Tag)).PurchaseOrderID;
            }
            else
            {
                tbGoodsReceivingDocumentID.Text = "";
            }
        }
    }
}