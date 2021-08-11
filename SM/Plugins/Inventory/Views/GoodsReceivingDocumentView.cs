using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
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
    public partial class GoodsReceivingDocumentView : ViewBase
    {
        private SiteServiceProfile siteServiceProfile;
        IInventoryService service = null;

        private GoodsReceivingDocument goodsReceivingDocument;

        WeakReference inventorySettingsEditor;

        private TabControl.Tab itemsTab;

        public GoodsReceivingDocumentView(RecordIdentifier goodsReceivingDocumentID)
            : this()
        {
            siteServiceProfile = PluginOperations.GetSiteServiceProfile();
            service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            try
            {
                goodsReceivingDocument = service.GetGoodsReceivingDocument(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocumentID, true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

            IPlugin pluginReference = PluginEntry.Framework.FindImplementor(this, "CanViewAdministrationTab", "InventorySettingsTab");
            inventorySettingsEditor = (pluginReference != null) ? new WeakReference(pluginReference) : null;
        }

        public GoodsReceivingDocumentView()
        {
            InitializeComponent();

            Attributes =
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingDocuments);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("GoodsReceivingDocument", goodsReceivingDocument.ID, Resources.GoodsReceivingDocument, true));
            contexts.Add(new AuditDescriptor("GoodsReceivingDocumentLines", goodsReceivingDocument.ID, Resources.GoodsReceivingDocumentLines, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.GoodsReceiving;
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
            HeaderText = Resources.GoodsReceivingDocument;
            TabControl.Tab gerneralTab = new TabControl.Tab(Resources.StockCountingDetailViewSummary, ViewPages.GoodsRecieveingDocumentGeneralPage.CreateInstance);

            itemsTab = new TabControl.Tab(Resources.StockCountingDetailViewItems, ViewPages.GoodsRecieveingDocumentItemPage.CreateInstance);

            tabSheetTabs.AddTab(itemsTab);
            tabSheetTabs.AddTab(gerneralTab);

            tabSheetTabs.SetData(isRevert, goodsReceivingDocument.ID, goodsReceivingDocument);
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            try
            {
                service.SaveGoodsReceivingDocument(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument, true);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "GoodsReceivingDocument", goodsReceivingDocument.GoodsReceivingID, null);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

            return true;
        }

        protected override void OnDelete()
        {
            try
            {
                service.DeleteGoodsReceivingDocument(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.GoodsReceivingID, true);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "GoodsReceivingDocument", goodsReceivingDocument.GoodsReceivingID, null);

                PluginEntry.Framework.ViewController.DiscardCurrentView(this);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void UpdateContextBarItem()
        {
            try
            {
                SetContextBarItemEnabled(
                    GetType().ToString() + ".View",
                    "AutoPopulateLines",
                    !service.GoodsReceivingDocumentHasLines(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.ID, true));

                SetContextBarItemEnabled(
                    GetType().ToString() + ".View",
                    "CloseGoodsReceivingDocument",
                    CanGoodsReceivingDocumentBePosted());
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private bool CanGoodsReceivingDocumentBePosted()
        {
            try
            {
                if (goodsReceivingDocument.Status == GoodsReceivingStatusEnum.Active)
                {
                    return !service.GoodsReceivingDocumentFullyReceived(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.GoodsReceivingID, false)
                        && service.GoodsReceivingDocumentHasLines(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.GoodsReceivingID, false);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
            finally
            {
                service.Disconnect(PluginEntry.DataModel);
            }

            return false;
        }

        public override List<IDataEntity> GetListSelection()
        {
            return (itemsTab.View as GoodsRecieveingDocumentItemPage).GetSelectedItems();
        }

        public override void AddExtendedMenuItem(ExtendedMenuItem item)
        {
            (itemsTab.View as GoodsRecieveingDocumentItemPage).AddOpenInItemViewMenuItem(item);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if ((changeHint == DataEntityChangeType.Enable || changeHint == DataEntityChangeType.Disable) && objectName == "GoodsReceivingDocumentLine" && changeIdentifier == goodsReceivingDocument.ID)
            {
                UpdateContextBarItem();
            }
            if (objectName == "GoodsReceivingDocumentRefreshSearch")
            {
                UpdateContextBarItem();
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            try
            {
                if (arguments.CategoryKey == GetType().ToString() + ".View")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.AutoPopulateGRDLines))
                    {
                        arguments.Add(new ContextBarItem(
                            Resources.AutoPopulateLines,
                            "AutoPopulateLines",
                            !service.GoodsReceivingDocumentHasLines(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.ID, true),
                            AutoPopulateLines), 500000);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingDocuments))
                    {
                        arguments.Add(new ContextBarItem(
                            Resources.CloseGoodsReceivingDocument,
                            "CloseGoodsReceivingDocument",
                            CanGoodsReceivingDocumentBePosted(),
                            PostGoodsReceivingDocument), 600000);
                    }
                }

                if (arguments.CategoryKey == GetType().ToString() + ".Related")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.VendorEdit))
                    {
                        arguments.Add(new ContextBarItem(
                            Resources.ViewVendor + ": " + goodsReceivingDocument.VendorName,
                            (string)goodsReceivingDocument.VendorID,
                            true,
                            PluginOperations.ShowVendorView), 200);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders))
                    {
                        arguments.Add(new ContextBarItem(
                            Resources.PurchaseOrder,
                            (string)goodsReceivingDocument.PurchaseOrderID,
                            true,
                            PluginOperations.ShowPurchaseOrder), 300);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings))
                    {
                        arguments.Add(new ContextBarItem(Resources.GoodsReceivingSettings, EditGoodsReceivingSettings), 100);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void EditGoodsReceivingSettings(object sender, EventArgs e)
        {
            if (inventorySettingsEditor.IsAlive)
            {
                ((IPlugin)inventorySettingsEditor.Target).Message(this, "ViewInventorySettingsTab", null);
            }
        }

        private void PostGoodsReceivingDocument(object sender, EventArgs e)
        {
            try
            {
                bool fullyRecieved = service.GoodsReceivingDocumentFullyReceived(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.GoodsReceivingID, false);
                bool allLinesPosted = service.GoodsReceivingDocumentAllLinesPosted(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.GoodsReceivingID, false);
                List<GoodsReceivingDocumentLine> lines = service.GetGoodsReceivingDocumentLines(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.GoodsReceivingID, false);
                bool closeDocument = false;

                if (!lines.Any())
                {
                    MessageDialog.Show(Resources.GRHasNoLinesToBePosted, Resources.CloseGoodsReceivingDocument);
                    return;
                }

                if (!allLinesPosted)
                {
                    MessageDialog.Show(Resources.GRCannotBeClosedLinesNotPosted, Resources.CloseGoodsReceivingDocument);
                    return;
                }

                if (allLinesPosted && !fullyRecieved)
                {
                    closeDocument = (QuestionDialog.Show(Resources.StillItemLinesOnPONotFullyRecieved + "\r\n" + Resources.DoYouWantToContinue, Resources.CloseGoodsReceivingDocument, Resources.Yes, Resources.No) == DialogResult.Yes);
                }

                if (closeDocument)
                {
                    //update the status of the 
                    goodsReceivingDocument.Status = GoodsReceivingStatusEnum.Posted;
                    service.SaveGoodsReceivingDocument(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument, false);

                    //Update the purchase order as closed
                    PurchaseOrder purchaseOrder = service.GetPurchaseOrder(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.PurchaseOrderID, false);
                    purchaseOrder.PurchaseStatus = PurchaseStatusEnum.Closed;
                    service.SavePurchaseOrder(PluginEntry.DataModel, siteServiceProfile, purchaseOrder, false);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseOrderStatus", purchaseOrder.ID, PurchaseStatusEnum.Closed);

                }

                UpdateContextBarItem();

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "GoodsReceivingDocument", goodsReceivingDocument.ID, goodsReceivingDocument);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseOrder", goodsReceivingDocument.PurchaseOrderID, null);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
            finally
            {
                service.Disconnect(PluginEntry.DataModel);
            }
        }

        private void AutoPopulateLines(object sender, EventArgs e)
        {
            try
            {
                service.CreateGoodsReceivingDocumentLinesFromPurchaseOrder(
                    PluginEntry.DataModel, siteServiceProfile,
                    goodsReceivingDocument.PurchaseOrderID, true);

                UpdateContextBarItem();

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit,
                    "GoodsReceivingDocumentLine",
                    goodsReceivingDocument.ID,
                    null);

                tabSheetTabs.BroadcastChangeInformation(DataEntityChangeType.Edit, "GoodsReceivingDocumentLine", goodsReceivingDocument.ID, null);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }
    }
}