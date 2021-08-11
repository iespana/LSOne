using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.ViewPlugins.Inventory.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    internal partial class TransferOrdersToReceivePage : UserControl, ITabView
    {
        private List<DataEntity> filterStoreDataEntities;
        private DecimalLimit quantityLimiter;
        private bool ableToUpdateTransfers;
        private bool hasEditOrdersPermission;
        private bool hasViewRequestsPermission;
        private List<RecordIdentifier> newTransferOrderIdsFromHeadOffice;
        private List<RecordIdentifier> deletedTransferOrderIdsFromHeadOffice;
        private bool visualComponentsLoaded;
        private SiteServiceProfile siteServiceProfile;

        public TransferOrdersToReceivePage()
        {
            InitializeComponent();

            lvTransfers.ContextMenuStrip = new ContextMenuStrip();
            lvTransfers.ContextMenuStrip.Opening += lvTransfers_RightClick;
            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += lvItems_RightClick;

            quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
            newTransferOrderIdsFromHeadOffice = new List<RecordIdentifier>();
            deletedTransferOrderIdsFromHeadOffice = new List<RecordIdentifier>();
            visualComponentsLoaded = false;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.TransferOrdersToReceivePage();
        }

        private InventoryTransferOrder SelectedTransferOrder { get; set; }
        private InventoryTransferOrderLine SelectedTransferOrderLine { get; set; }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            filterStoreDataEntities = (List<DataEntity>)internalContext;

            siteServiceProfile = PluginOperations.GetSiteServiceProfile(PluginEntry.DataModel);


            if (context != null && !context.IsEmpty && context != "")
            {
                InventoryTransferOrder initalTransferOrder = Providers.InventoryTransferOrderData.Get(PluginEntry.DataModel, context);
                SelectedTransferOrder = initalTransferOrder; 
            }
            
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

        private void ShowDeletedFromHeadOfficeMessage()
        {
            MessageDialog.Show(
                        Properties.Resources.TransferOrdersDeletedFromHeadOfficeDescription.Replace("#1", deletedTransferOrderIdsFromHeadOffice.Count.ToString()),
                        Properties.Resources.TransferOrdersDeletedFromHeadOffice,
                        MessageBoxIcon.Exclamation);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // This logic needs to run after the visual components are drawn on the screen
            if (!DesignMode)
            {
                lvTransfers.AutoSizeColumns(true);

                BeginInvoke((MethodInvoker) delegate
                                                {
                                                    if (deletedTransferOrderIdsFromHeadOffice.Count > 0)
                                                    {
                                                        ShowDeletedFromHeadOfficeMessage();
                                                    }

                                                    visualComponentsLoaded = true;
                                                });
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (!DesignMode)
            {
                lvTransfers.AutoSizeColumns(true);
            }
        }

        private void LoadTransfers()
        {
            hasEditOrdersPermission = PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders);
            hasViewRequestsPermission = PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferRequests);

            newTransferOrderIdsFromHeadOffice =  new List<RecordIdentifier>();
            deletedTransferOrderIdsFromHeadOffice = new List<RecordIdentifier>();
            
            lvTransfers.ClearRows();
            List<InventoryTransferOrder> transfers = new List<InventoryTransferOrder>(); 
            List<RecordIdentifier> storeIds = filterStoreDataEntities.Select(store => store.ID).ToList();
            if (storeIds.Count > 0)
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                // Refresh inventory transfers from HO if we are not on HO
                if (!PluginEntry.DataModel.IsHeadOffice)
                {                    
                    ableToUpdateTransfers = inventoryService.UpdateTransferOrdersForStores(
                       PluginEntry.DataModel,
                       storeIds,
                       InventoryTransferType.ToReceive,
                       siteServiceProfile,
                       out newTransferOrderIdsFromHeadOffice,
                       out deletedTransferOrderIdsFromHeadOffice);
                }
                else
                {
                    ableToUpdateTransfers = true;
                }

                if (!ableToUpdateTransfers)
                {
                    errorProvider1.SetError(btnReceive, Properties.Resources.UnableToConnectToSiteServiceReceivingDisabled);
                }
                else
                {
                    errorProvider1.Clear();
                }

                if (deletedTransferOrderIdsFromHeadOffice.Count > 0 && visualComponentsLoaded)
                {
                    ShowDeletedFromHeadOfficeMessage();
                }

                transfers = Providers.InventoryTransferOrderData.GetListForStore(
                    PluginEntry.DataModel, 
                    storeIds,
                    InventoryTransferType.ToReceive, InventoryTransferOrderSortEnum.Id, false);
            }

            foreach (InventoryTransferOrder transfer in transfers)
            {
                Row row = new Row {Tag = transfer};

                if (newTransferOrderIdsFromHeadOffice.Contains(transfer.ID))
                {
                    IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider), Properties.Resources.New);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.HorizontalCenter, "", false));
                }
                else
                {
                    row.AddText("");
                }
                
                row.AddText((string)transfer.ID);
                row.AddCell(new DateTimeCell(transfer.SentDate.ToShortDateString() + " - " + transfer.SentDate.ToShortTimeString(), transfer.SentDate));
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
            List<InventoryTransferOrderLine> transferItems = Providers.InventoryTransferOrderLineData.GetListForInventoryTransfer(PluginEntry.DataModel,
                                                                                            SelectedTransferOrder.ID, InventoryTransferOrderLineSortEnum.ItemName,
                                                                                            false);
            foreach (InventoryTransferOrderLine item in transferItems)
            {
                Row row = new Row {Tag = item};
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
                if (SelectedTransferOrderLine != null && item.ID == SelectedTransferOrderLine.ID)
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
                btnReceive.Enabled = false;
            }
            else if (rowsSelected == 1)
            {
                
                SelectedTransferOrder =
                    (InventoryTransferOrder) lvTransfers.Row(lvTransfers.Selection.FirstSelectedRow).Tag;
                lblNoSelection.Visible = false;
                btnReceive.Enabled = !SelectedTransferOrder.Received && ableToUpdateTransfers && hasEditOrdersPermission && !PluginEntry.DataModel.IsHeadOffice;
                btnViewTransferRequest.Enabled = hasViewRequestsPermission && SelectedTransferOrder.CreatedFromTransferRequest;
                LoadItems();
            }
            else
            {
                SelectedTransferOrder = null;
                lblNoSelection.Visible = false;
                btnReceive.Enabled = false;
                btnViewTransferRequest.Enabled = false;
            }

            lvItems.Visible = !lblNoSelection.Visible;
            btnEditItem.Visible = lvItems.Visible;
            lblItemHeader.Visible = lvItems.Visible;
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            int rowsSelected = lvItems.Selection.Count;
            if (rowsSelected <= 0)
            {
                SelectedTransferOrderLine = null;
                btnEditItem.Enabled = false;
            }
            else if (rowsSelected == 1)
            {
                SelectedTransferOrderLine = (InventoryTransferOrderLine)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag;
                btnEditItem.Enabled = btnReceive.Enabled;
            }
            else
            {
                SelectedTransferOrderLine = null;
                btnEditItem.Enabled = false;
            }
        }

        private void lvTransfers_RightClick(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvTransfers.ContextMenuStrip;

            menu.Items.Clear();

            ExtendedMenuItem item = new ExtendedMenuItem(
                   btnReceive.Text,
                   100,
                   btnReceive_Click)
            {
                Enabled = btnReceive.Enabled
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   btnViewTransferRequest.Text,
                   200,
                   btnViewTransferRequest_Click)
            {
                Enabled = btnViewTransferRequest.Enabled
            };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InventoryTransfersSendingList", lvTransfers.ContextMenuStrip, lvTransfers);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvItems_RightClick(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            ExtendedMenuItem item = new ExtendedMenuItem(
                   Resources.Edit,
                   200,
                   btnEditItem_Click)
            {
                Enabled = btnEditItem.Enabled,
                Image = btnEditItem.Image,
                Default = true
            };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InventoryTransfersToReceiveItemsList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private List<ReasonCode> GetReasonCodes()
        {
            try
            {
                List<ReasonCode> reasonCodes = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetReasonCodes(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(PluginEntry.DataModel),
                    InventoryJournalTypeEnum.Transfer,
                    true);

                UpdateReasonCodes(reasonCodes);

                return reasonCodes;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return null;
            }
        }

        protected virtual void UpdateReasonCodes(List<ReasonCode> reasonCodes)
        {
            bool updateHO = false;
            ReasonCode toUpdate = reasonCodes.FirstOrDefault(f => f.ID == MissingInTransferReasonConstants.ConstMissingInTransferReasonID && f.Text == MissingInTransferReasonConstants.ConstMissingInTransferReasonID);
            if (toUpdate != null)
            {
                toUpdate.Text = Properties.Resources.ReasonCodeMissingInTransit;
                updateHO = true;
            }
            

            if (updateHO)
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).UpdateReasonCodes(
                    PluginEntry.DataModel, 
                    PluginOperations.GetSiteServiceProfile(PluginEntry.DataModel), 
                    reasonCodes, 
                    InventoryJournalTypeEnum.Transfer, 
                    true);
            }
        }

        private bool ContinueWithReceiving()
        {
            List<InventoryTransferOrderLine> transferItems = Providers.InventoryTransferOrderLineData.GetListForInventoryTransfer(PluginEntry.DataModel,
                                                                                            SelectedTransferOrder.ID, InventoryTransferOrderLineSortEnum.ItemName,
                                                                                            false);
            if (transferItems.Count(c => c.QuantitySent != c.QuantityReceived) > 0)
            {
                if (QuestionDialog.Show(Resources.ReceivedQuantityNotSameAsSentQuantity + " "
                                        + Resources.InventoryAdjustmentWillBeCreatedForTheDifference + "\r\n"
                                        + Resources.DoYouWantToContinue, Resources.ReceiveInventoryTransferOrder) == DialogResult.Yes)
                {
                    GetReasonCodes();
                    return true;
                }
            }

            return QuestionDialog.Show(Resources.ReceiveInventoryTransferQuestion, Resources.ReceiveInventoryTransferOrder) == DialogResult.Yes;
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
            IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            try
            {

                if (!ContinueWithReceiving())
                {
                    return;
                }

                try
                {
                    inventoryService.ReceiveTransferOrder(PluginEntry.DataModel, SelectedTransferOrder.ID, siteServiceProfile);
                }
                catch (Exception)
                {
                    MessageDialog.Show(Properties.Resources.ErrorReceivingInventoryTransferOrder, MessageBoxIcon.Error);
                    return;
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", SelectedTransferOrder.ID, SelectedTransferOrder);
            }
            finally
            {
                inventoryService.Disconnect(PluginEntry.DataModel);
            }

        }

        private void btnEditItem_Click(object sender, EventArgs e)
        {
            InventoryTransferOrderItemDialog dlg = new InventoryTransferOrderItemDialog(SelectedTransferOrderLine, true);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void btnViewTransferRequest_Click(object sender, EventArgs e)
        {
            bool showOrderSuccessful = PluginOperations.ShowInventoryTransferRequestsView(SelectedTransferOrder.InventoryTransferRequestId, true);
            if (!showOrderSuccessful)
            {
                MessageDialog.Show(Properties.Resources.NoInventoryTransferRequestFound, MessageBoxIcon.Information);
            }
        }

        private void lvItems_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnEditItem.Enabled)
            {
                btnEditItem_Click(sender, args);
            }
        }

    }
}
