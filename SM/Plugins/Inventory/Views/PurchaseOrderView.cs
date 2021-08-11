using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.ViewPlugins.Inventory.ViewPages;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class PurchaseOrderView : ViewBase
    {
        RecordIdentifier selectedID;

        PurchaseOrder purchaseOrder;

        WeakReference printingHandler;
        WeakReference inventorySettingsEditor;

        private SiteServiceProfile siteServiceProfile;

        bool lockEvents = true;

        private TabControl.Tab settingsTab;
        private TabControl.Tab itemsTab;
        private TabControl.Tab miscChargesTab;

        public PurchaseOrderView(RecordIdentifier purchaseOrderID)
            : this()
        {
            selectedID = purchaseOrderID;

            siteServiceProfile = PluginOperations.GetSiteServiceProfile();

            try
            {
                purchaseOrder = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetPurchaseOrder(PluginEntry.DataModel, siteServiceProfile, selectedID, true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }

            lockEvents = false;
            HeaderText = Resources.PurchaseOrder + ": " + purchaseOrder.Text;
        }

        private PurchaseOrderView()
        {
            IPlugin plugin;

            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help | 
                ViewAttributes.Save |
                ViewAttributes.Delete;

            plugin = PluginEntry.Framework.FindImplementor(this, "CanDisplayXtraReport", null);
            printingHandler = plugin != null ? new WeakReference(plugin) : null;

            if (printingHandler != null && (PluginEntry.DataModel.HasPermission(Permission.ViewReports) || PluginEntry.DataModel.HasPermission(Permission.ViewInventoryReports)))
            {
                Attributes |= ViewAttributes.PrintPreview;
                Attributes |= ViewAttributes.Print;
            }

            plugin = PluginEntry.Framework.FindImplementor(this, "CanViewAdministrationTab", "InventorySettingsTab");
            inventorySettingsEditor = (plugin != null) ? new WeakReference(plugin) : null;
            
            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PurchaseOrder", purchaseOrder.ID, Resources.PurchaseOrder, true));
            contexts.Add(new AuditDescriptor("PurchaseOrderLines", purchaseOrder.ID, Resources.PurchaseOrderLines, false));
            contexts.Add(new AuditDescriptor("PurchaseOrderMiscCharges", purchaseOrder.PurchaseOrderID, Resources.MiscCharges, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.PurchaseOrder;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        {
                return selectedID;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            try
            {
                purchaseOrder = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetPurchaseOrder(PluginEntry.DataModel, siteServiceProfile, selectedID, true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }

            if (!isRevert)
            {
                itemsTab = new TabControl.Tab(Resources.Items, ViewPages.PurchaseOrderItemsPage.CreateInstance);
                settingsTab = new TabControl.Tab(Resources.StockCountingDetailViewSummary, ViewPages.PurchaseOrderSettingsPage.CreateInstance);
                miscChargesTab = new TabControl.Tab(Resources.MiscCharges, ViewPages.PurchaseOrderMiscChargesPage.CreateInstance);

                tabSheetTabs.AddTab(itemsTab);
                tabSheetTabs.AddTab(settingsTab);
                tabSheetTabs.AddTab(miscChargesTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, purchaseOrder.ID, purchaseOrder);
            }

            tabSheetTabs.SetData(isRevert, purchaseOrder.ID, purchaseOrder);
        }

        protected override bool DataIsModified()
        {
            return tabSheetTabs.IsModified() || purchaseOrder.Dirty ;
        }

        protected override bool SaveData()
        {
            tabSheetTabs.GetData();

            try
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SavePurchaseOrder(PluginEntry.DataModel, siteServiceProfile, purchaseOrder, true);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PurchaseOrder", purchaseOrder.PurchaseOrderID, null);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

            return true;
        }

        protected override void OnDelete()
        {
            if (PluginOperations.DeletePurchaseOrder(purchaseOrder.PurchaseOrderID) == PurchaseOrderLinesDeleteResult.CanBeDeleted)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "PurchaseOrder", purchaseOrder.PurchaseOrderID, null);
                PluginEntry.Framework.ViewController.DiscardCurrentView(this);
            }
        }

        public override List<IDataEntity> GetListSelection()
        {
            return (itemsTab.View as PurchaseOrderItemsPage).GetSelectedItems();
        }

        public override void AddExtendedMenuItem(ExtendedMenuItem item)
        {
            (itemsTab.View as PurchaseOrderItemsPage).AddOpenInItemViewMenuItem(item);
        }

        protected override void OnPageSetup()
        {
            if (printingHandler.IsAlive)
            {
                ((IPlugin)printingHandler.Target).Message(this, "PageSetup", null);
            }
        }

        protected override void OnPrint()
        {
            Guid reportID = new Guid(SystemReportConstants.PurchaseOrder);

            IPlugin reportViewer = PluginEntry.Framework.FindImplementor(null, "CanDisplayReport", reportID);

            if (reportViewer != null)
            {
                Dictionary<string, ProcedureParameter>
                parameters = new Dictionary<string, ProcedureParameter>();
                parameters.Add("@PURCHASEORDERID", new ProcedureParameter { Value = (string)purchaseOrder.ID });
                PluginOperationArguments args = new PluginOperationArguments(reportID, parameters, true);

                reportViewer.Message(null, "ShowReportPrint", args);
            }
            else
            {
                MessageDialog.Show(Resources.SystemReportNotFound.Replace("#1", Resources.PurchaseOrder), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnPrintPreview()
        {
            Guid reportID = new Guid(SystemReportConstants.PurchaseOrder);

            IPlugin reportViewer = PluginEntry.Framework.FindImplementor(null, "CanDisplayReport", reportID);

            if (reportViewer != null)
            {
                Dictionary<string, ProcedureParameter>
                parameters = new Dictionary<string, ProcedureParameter>();
                parameters.Add("@PURCHASEORDERID", new ProcedureParameter { Value = (string)purchaseOrder.ID });
                PluginOperationArguments args = new PluginOperationArguments(reportID, parameters, true);

                reportViewer.Message(null, "ShowReportPrintPreview", args);
            }
            else
            {
                MessageDialog.Show(Resources.SystemReportNotFound.Replace("#1", Resources.PurchaseOrder), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "PurchaseOrderStatus" && changeIdentifier.PrimaryID == purchaseOrder.PurchaseOrderID)
            {
                purchaseOrder.PurchaseStatus = (PurchaseStatusEnum)(int) param;
                RefreshContextBar();
                tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
                PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PurchaseOrderDashBoardItemID);
            }
            else if (objectName == "PurchaseOrder" && changeIdentifier == purchaseOrder.PurchaseOrderID)
            {
                tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
            }
            else if (objectName == "PurchaseOrderChangeDiscount" && changeIdentifier == purchaseOrder.PurchaseOrderID)
            {
                tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
            }
            else if (objectName == "GoodsReceivingDocument" && changeHint == DataEntityChangeType.Add)
            {
                RefreshContextBar();
            }
            else if (objectName == "VendorChanged" && changeIdentifier == purchaseOrder.VendorID)
            {
                tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
            }
            else if (changeIdentifier == purchaseOrder.PurchaseOrderID)
            {
                tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            try
            {
                siteServiceProfile = PluginOperations.GetSiteServiceProfile();

                if (arguments.CategoryKey == base.GetType().ToString() + ".View")
                {
                    bool purchaseOrderHasGrDocument = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).PurchaseOrderHasGoodsReceivingDocument(PluginEntry.DataModel, siteServiceProfile, purchaseOrder.ID, true);
                    bool showPostAndReceiveLink = (purchaseOrder.PurchaseStatus == PurchaseStatusEnum.Open) && !purchaseOrderHasGrDocument;

                    if (PluginEntry.DataModel.HasPermission(Permission.PostAndReceivePO))
                    {
                        arguments.Add(new ContextBarItem(Resources.PostAndReceive, "P&R", showPostAndReceiveLink, PostAndReceivePO), 5000000);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders) && purchaseOrderHasGrDocument && purchaseOrder.PurchaseStatus == PurchaseStatusEnum.Open)
                    {
                        arguments.Add(new ContextBarItem(Resources.PostPurchaseOrder, "PostPurchaseOrder", !showPostAndReceiveLink, PostPurchaseOrder), 6000000);
                    }
                }


                if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings))
                    {
                        arguments.Add(new ContextBarItem(Resources.PurchaseOrderSettings, EditPurchaseOrderSettings), 100);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.VendorView))
                    {
                        arguments.Add(new ContextBarItem(Resources.ViewVendor + ": " + purchaseOrder.VendorName, purchaseOrder.VendorID, true, PluginOperations.ShowVendorView), 200);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingDocuments))
                    {
                        if (Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GoodsReceivingDocumentExists(PluginEntry.DataModel, siteServiceProfile, purchaseOrder.PurchaseOrderID, true))
                        {
                            arguments.Add(new ContextBarItem(Resources.GoodsReceivingDocument, (string) purchaseOrder.PurchaseOrderID, true, PluginOperations.ShowGoodsReceivingDocument), 300);
                        }
                        else
                        {
                            arguments.Add(new ContextBarItem(Resources.GoodsReceivingDocuments, PluginOperations.ShowGoodsReceivingDocuments), 300);
                        }
                    }

                    if (PluginEntry.Framework.CanRunOperation("ShowImageBank"))
                    {
                        arguments.Add(new ContextBarItem(Properties.Resources.ImageBank, PluginOperations.ShowImageBankHandler), 400);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }

        private void EditPurchaseOrderSettings(object sender, EventArgs e)
        {
            if (inventorySettingsEditor.IsAlive)
            {
                ((IPlugin)inventorySettingsEditor.Target).Message(this, "ViewInventorySettingsTab", null);
            }
        }

        private void PostPurchaseOrder(object sender, EventArgs e)
        {
            try
            {
                PurchaseOrderLineSearch search = new PurchaseOrderLineSearch();
                search.PurchaseOrderID = purchaseOrder.ID;
                search.StoreID = purchaseOrder.StoreID;

                List<PurchaseOrderLine> POLines = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetPurchaseOrderLines(PluginEntry.DataModel, siteServiceProfile, search, PurchaseOrderLineSorting.ItemName, false, out int totalRecords, false);

                if (POLines.Count == 0)
                {
                    MessageDialog.Show(Resources.NoLinesInPO);
                    return;
                }

                purchaseOrder.PurchaseStatus = PurchaseStatusEnum.Closed;
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SavePurchaseOrder(PluginEntry.DataModel, siteServiceProfile, purchaseOrder, true);

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseOrderStatus", purchaseOrder.ID, purchaseOrder.PurchaseStatus);

                RefreshContextBar();

                MessageDialog.Show(Resources.PurchaseOrderHasBeenPosted);

            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        private void PostAndReceivePO(object sender, EventArgs e)
        {
            try
            {
                PurchaseOrderLineSearch search = new PurchaseOrderLineSearch();
                search.PurchaseOrderID = purchaseOrder.ID;
                search.StoreID = purchaseOrder.StoreID;

                List<PurchaseOrderLine> POLines = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetPurchaseOrderLines(PluginEntry.DataModel, siteServiceProfile, search, PurchaseOrderLineSorting.ItemName, false, out int totalRecords, false);

                if (POLines.Count == 0)
                {
                    MessageDialog.Show(Resources.NoLinesInPO);
                    return;
                }

                // Show a dialog confirming the operation
                if (QuestionDialog.Show(Resources.PostAndReceiveQuestion, Resources.PostAndReceive) == DialogResult.Yes)
                {
                    GoodsReceivingPostResult result = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).PostAndReceiveAPurchaseOrder(PluginEntry.DataModel, siteServiceProfile, purchaseOrder, true);

                    PluginOperations.DisplayGoodsReceivingPostingResult(result);

                    // Notify the framework that all the items in the purchase order have had an inventory update
                    foreach (PurchaseOrderLine poLine in POLines)
                    {
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "ItemInventory", poLine.ItemID, null);
                    }

                    if (result == GoodsReceivingPostResult.Success)
                    {
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseOrderStatus", purchaseOrder.ID, PurchaseStatusEnum.Closed);
                    }

                    RefreshContextBar();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        private void RefreshContextBar()
        {
            if (!lockEvents)
            {
                try
                {
                    bool purchaseOrderHasGrDocument = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).PurchaseOrderHasGoodsReceivingDocument(PluginEntry.DataModel, siteServiceProfile, purchaseOrder.ID, true);

                    if (PluginEntry.DataModel.HasPermission(Permission.PostAndReceivePO))
                    {
                        bool showPostAndReceiveLink = (purchaseOrder.PurchaseStatus == PurchaseStatusEnum.Open) && !purchaseOrderHasGrDocument;
                        SetContextBarItemEnabled(this.GetType().ToString() + ".View", "P&R", showPostAndReceiveLink);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders))
                    {
                        if (purchaseOrderHasGrDocument && purchaseOrder.PurchaseStatus == PurchaseStatusEnum.Open)
                        {
                            GoodsReceivingDocument grDocument = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).
                                GetGoodsReceivingDocument(
                                    PluginEntry.DataModel,
                                    siteServiceProfile,
                                    purchaseOrder.PurchaseOrderID,
                                    true);

                            if (grDocument.Status == GoodsReceivingStatusEnum.Posted)
                            {
                                SetContextBarItemEnabled(this.GetType().ToString() + ".View", "PostPurchaseOrder", true);
                            }
                        }
                        else
                        {
                            SetContextBarItemEnabled(this.GetType().ToString() + ".View", "PostPurchaseOrder", false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                }

                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }
        }
    }
}