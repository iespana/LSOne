using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    internal partial class TransferOrdersFinishedPage : UserControl, ITabView
    {
        private List<DataEntity> filterStoreDataEntities;
        private DecimalLimit quantityLimiter;

        public TransferOrdersFinishedPage()
        {
            InitializeComponent();

            lvTransfers.ContextMenuStrip = new ContextMenuStrip();
            lvTransfers.ContextMenuStrip.Opening += lvTransfers_RightClick;

            quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.TransferOrdersFinishedPage();
        }

        private InventoryTransferOrder SelectedTransferOrder { get; set; }
        private InventoryTransferOrderLine SelectedTransferOrderOrderLine { get; set; }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            filterStoreDataEntities = (List<DataEntity>)internalContext;
            //InventoryTransferOrder initalTransferOrder = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTransferOrder(
            //                    PluginEntry.DataModel,
            //                    PluginOperations.GetSiteServiceProfile(PluginEntry.DataModel),
            //                    context,
            //                    true);
            
            InventoryTransferOrder initalTransferOrder = Providers.InventoryTransferOrderData.Get(PluginEntry.DataModel, context);
            SelectedTransferOrder = initalTransferOrder;
            LoadTransfers();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            //PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PurchaseOrderTaxSettings", null, null);

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "InventoryTransferOrder")
            {
                LoadTransfers();
            }
        }

        #endregion

        private void LoadTransfers()
        {
            lvTransfers.ClearRows();
            List<InventoryTransferOrder> transfers = new List<InventoryTransferOrder>(); 
            List<RecordIdentifier> storeIds = filterStoreDataEntities.Select(store => store.ID).ToList();
            if (storeIds.Count > 0)
            {
                //transfers = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTransferOrderListForStore(
                //    PluginEntry.DataModel,
                //    PluginOperations.GetSiteServiceProfile(PluginEntry.DataModel),
                //    storeIds,
                //    InventoryTransferType.Finished,
                //    InventoryTransferOrderSortEnum.Id,
                //    false,
                //    true);
                
                transfers = Providers.InventoryTransferOrderData.GetListForStore(
                        PluginEntry.DataModel
                        , storeIds
                        , InventoryTransferType.Finished
                        , InventoryTransferOrderSortEnum.Id, false);
            }

            foreach (InventoryTransferOrder transfer in transfers)
            {
                Row row = new Row();
                row.Tag = transfer;
                row.AddText((string)transfer.ID);
                row.AddCell(new DateTimeCell(transfer.CreationDate.ToShortDateString() + " - " + transfer.CreationDate.ToShortTimeString(), transfer.CreationDate));
                row.AddCell(new DateTimeCell(transfer.SentDate.ToShortDateString() + " - " + transfer.SentDate.ToShortTimeString(), transfer.SentDate));
                row.AddCell(new DateTimeCell(transfer.ReceivingDate.ToShortDateString() + " - " + transfer.ReceivingDate.ToShortTimeString(), transfer.ReceivingDate));
                row.AddText(transfer.SendingStoreName);
                row.AddText(transfer.ReceivingStoreName);
                lvTransfers.AddRow(row);
                if (SelectedTransferOrder != null && transfer.ID == SelectedTransferOrder.ID)
                {
                    lvTransfers.Selection.Set(lvTransfers.RowCount - 1);
                }
            }

            lvTransfers.AutoSizeColumns(true);
            lvTransfers.OnSelectionChanged(EventArgs.Empty);
        }

        private void LoadItems()
        {
            lvItems.ClearRows();
            //List<InventoryTransferOrderLine> transferItems = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetOrderLinesForInventoryTransfer(
            //                PluginEntry.DataModel,
            //                PluginOperations.GetSiteServiceProfile(PluginEntry.DataModel),
            //                SelectedTransferOrder.ID,
            //                InventoryTransferOrderLineSortEnum.ItemName,
            //                false,
            //                false,
            //                true);
            
            List<InventoryTransferOrderLine> transferItems = Providers.InventoryTransferOrderLineData.GetListForInventoryTransfer(PluginEntry.DataModel,
                                                                                            SelectedTransferOrder.ID,
                                                                                            InventoryTransferOrderLineSortEnum.ItemName,
                                                                                            false);

            foreach (InventoryTransferOrderLine item in transferItems)
            {
                Row row = new Row();
                row.Tag = item;
                row.AddText((string)item.ItemId);

                if (string.IsNullOrEmpty(item.ItemName))
                {
                    RetailItem retailItem = PluginOperations.GetRetailItem(item.ItemId, false);
                    item.ItemName = retailItem == null ? Properties.Resources.NotAvailable : retailItem.Text;
                    item.VariantName = retailItem == null ? Properties.Resources.NotAvailable : retailItem.VariantName;
                }

                row.AddText(item.ItemName);
                row.AddText(item.VariantName);
                row.AddCell(new NumericCell(item.QuantitySent.FormatWithLimits(quantityLimiter) + " " + item.UnitName, item.QuantitySent));
                row.AddCell(new NumericCell(item.QuantityReceived.FormatWithLimits(quantityLimiter) + " " + item.UnitName, item.QuantityReceived));
                row.AddCell(new NumericCell(item.QuantityRequested.FormatWithLimits(quantityLimiter) + " " + item.UnitName, item.QuantityRequested));
                lvItems.AddRow(row);
                if (SelectedTransferOrderOrderLine != null && item.ID == SelectedTransferOrderOrderLine.ID)
                {
                    lvItems.Selection.Set(lvTransfers.RowCount - 1);
                }
            }
            lvItems.AutoSizeColumns(true);
            lvItems.OnSelectionChanged(EventArgs.Empty);
        }

        private void lvTransfers_SelectionChanged(object sender, EventArgs e)
        {
            int rowsSelected = lvTransfers.Selection.Count;
            if (rowsSelected <= 0)
            {
                lblNoSelection.Visible = true;
            }
            else if (rowsSelected == 1)
            {
                SelectedTransferOrder =
                    (InventoryTransferOrder) lvTransfers.Row(lvTransfers.Selection.FirstSelectedRow).Tag;
                lblNoSelection.Visible = false;
                btnViewTransferRequest.Enabled = SelectedTransferOrder.InventoryTransferRequestId != "" &&
                                                 PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferRequests);
                LoadItems();
            }
            else
            {
                SelectedTransferOrder = null;
                lblNoSelection.Visible = false;
            }

            lvItems.Visible = !lblNoSelection.Visible;
            lblItemHeader.Visible = lvItems.Visible;
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            int rowsSelected = lvItems.Selection.Count;
            if (rowsSelected <= 0)
            {
                SelectedTransferOrderOrderLine = null;
            }
            else if (rowsSelected == 1)
            {
                SelectedTransferOrderOrderLine = (InventoryTransferOrderLine)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag;
            }
            else
            {
                SelectedTransferOrderOrderLine = null;
            }
        }

        private void lvTransfers_RightClick(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvTransfers.ContextMenuStrip;

            menu.Items.Clear();

            ExtendedMenuItem item = new ExtendedMenuItem(
                   btnViewTransferRequest.Text,
                   100,
                   btnViewTransferRequest_Click)
            {
                Enabled = btnViewTransferRequest.Enabled
            };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InventoryTransfersSendingList", lvTransfers.ContextMenuStrip, lvTransfers);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnViewTransferRequest_Click(object sender, EventArgs e)
        {
            bool showSendingOrder = PluginEntry.DataModel.IsHeadOffice ||
                                    PluginEntry.DataModel.CurrentStoreID == SelectedTransferOrder.ReceivingStoreId;
            bool showOrderSuccessful = PluginOperations.ShowInventoryTransferRequestsView(SelectedTransferOrder.InventoryTransferRequestId, showSendingOrder);
            if (!showOrderSuccessful)
            {
                MessageDialog.Show(Properties.Resources.NoInventoryTransferRequestFound, MessageBoxIcon.Information);
            }
        }

    }
}
