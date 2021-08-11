using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory
{
    public class PluginEntry : IPlugin, IPluginDashboardProvider
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        internal static Guid ExcelFolderLocationSettingID = new Guid("EB2EE2BD-5A52-4A9A-B84B-176458668AF9");
        internal static Guid PurchaseOrderDashBoardItemID = new Guid("08111544-A2B6-4D90-8B29-4989835B2A12");

        private SiteServiceProfile siteServiceProfile;

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.Inventory; }
        }

        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "CanChangeItemsUnits":
                    return DataModel.HasPermission(Permission.ItemsEdit);

                case "CanAddItemToVendor":
                    return DataModel.HasPermission(Permission.VendorEdit);

                case "CanViewPurchaseOrder":
                    return DataModel.HasPermission(Permission.ManagePurchaseOrders);

                case "CanViewPurchaseOrders":
                    return DataModel.HasPermission(Permission.ManagePurchaseOrders);

                case "CanZeroOutInventory":
                    return DataModel.HasPermission(Permission.EditInventoryAdjustments) || DataModel.HasPermission(Permission.ManageItemTypes);
                default:
                    return false;
            }
        }

        public object Message(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "ChangeItemsUnits":
                    ItemUnitDialog dlg = new ItemUnitDialog((RetailItem)parameters);
                    dlg.ShowDialog();
                    break;
                case "CanAddItemToVendor":
                    using (ItemVendorDialog dialog = new ItemVendorDialog((RecordIdentifier) parameters))
                    {
                        dialog.ShowDialog();
                    }
                    break;
                case "ViewPurchaseOrder":
                    var purchaseOrderId = (RecordIdentifier) parameters;
                    PluginOperations.ShowPurchaseOrder(purchaseOrderId);
                    break;
                case "ViewPurchaseOrders":
                    if (parameters is RecordIdentifier)
                    {
                        PluginOperations.ShowPurchaseOrders((RecordIdentifier)parameters);
                    }
                    else
                    {
                        PluginOperations.ShowPurchaseOrderWizard(null, EventArgs.Empty);
                    }
                    break;
                case "ZeroOutInventory":
                    if (parameters is object[])
                    {
                        var param = (object[])parameters;
                        PluginOperations.ZeroOutInventory((RecordIdentifier)param[0], (RecordIdentifier)param[1], (RecordIdentifier)param[2]);
                    }
                    break;
            }

            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            // We want to add tabs on tab panels in other plugins
            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;

            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);

            frameworkCallbacks.AddMenuConstructionConstructionHandler(PluginOperations.ConstructMenus);
            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Resources.ViewAllVendors,  PluginOperations.ShowVendorsView, Permission.VendorView);
            operations.AddOperation(Resources.PurchaseOrders, PluginOperations.ShowPurchaseOrderWizard, Permission.ManagePurchaseOrders);
            operations.AddOperation(Resources.GoodsReceivingDocuments, PluginOperations.ShowGoodsReceivingDocuments, Permission.ManageGoodsReceivingDocuments);
            operations.AddOperation(Resources.StockCounting, PluginOperations.ShowStockCountingView, Permission.StockCounting);
            operations.AddOperation(Resources.InventoryAdjustments, PluginOperations.ShowInventoryAdjustmentsView, new string[] { Permission.ViewInventoryAdjustments, Permission.ManageInventoryAdjustments, Permission.ManageInventoryAdjustmentsForAllStores });
            operations.AddOperation(Resources.StockReservations, PluginOperations.ShowStockReservationsView, Permission.ManageStockReservations);
            operations.AddOperation(Resources.ParkedInventory, PluginOperations.ShowParkedInventoryView, Permission.ManageParkedInventory);
            operations.AddOperation(Resources.InventoryTransferOrders, PluginOperations.ShowInventoryTransferOrderWizard, Permission.ViewInventoryTransferOrders);
            operations.AddOperation(Resources.InventoryTransferRequests, PluginOperations.ShowInventoryTransferRequestWizard, Permission.ViewInventoryTransferRequests);
            operations.AddOperation(Resources.InventoryInTransit, PluginOperations.ShowInventoryInTransitView);
            operations.AddOperation(Resources.ReasonCodes, PluginOperations.ShowReasonCodesView, Permission.ManageParkedInventory);
        }

        #endregion

        public void LoadDashboardItem(IConnectionManager threadedEntry, DashboardItem item)
        {
            if (item.ID == PurchaseOrderDashBoardItemID)
            {
                item.State = DashboardItem.ItemStateEnum.Info;


                DataEntity storeEntity = threadedEntry.IsHeadOffice ? null : Providers.StoreData.GetStoreEntity(threadedEntry, threadedEntry.CurrentStoreID);

                if (PluginOperations.TestSiteService(threadedEntry, false).Item1)
                {
                    siteServiceProfile = PluginOperations.GetSiteServiceProfile(threadedEntry);

                    int purchaseOrderCount;
                    Services.Interfaces.Services.InventoryService(threadedEntry)
                        .PurchaseOrderAdvancedSearch(threadedEntry, siteServiceProfile, true, 0, 1,
                            InventoryPurchaseOrderSortEnums.Status, false, out purchaseOrderCount, status: PurchaseStatusEnum.Open, storeID: storeEntity?.ID);

                    item.InformationText =
                                        threadedEntry.IsHeadOffice
                                        ? Resources.NumberOfOpenPurchaseOrders.Replace("#1", purchaseOrderCount.ToString())
                                        : (storeEntity != null)
                                          ? Resources.OpenPurchaseOrdersOnStore.Replace("#1", purchaseOrderCount.ToString()).Replace("#2", storeEntity.Text)
                                          : (string)threadedEntry.CurrentStoreID;

                    if (threadedEntry.HasPermission(Permission.ManagePurchaseOrders))
                    {
                        item.SetButton(0, Resources.Manage, PluginOperations.ShowPurchaseOrdersView);
                    }
                }
                else
                {
                    item.InformationText = Resources.ToCountPurchaseOrdersConnectToSiteService;
                }
            }
        }

        public void RegisterDashBoardItems(DashboardItemArguments args)
        {

        }
    }
}
