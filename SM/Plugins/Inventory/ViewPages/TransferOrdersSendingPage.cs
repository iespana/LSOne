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
    internal partial class TransferOrdersSendingPage : UserControl, ITabView
    {
        private List<DataEntity> filterStoreDataEntities;
        private DecimalLimit quantityLimiter;
        private bool ableToUpdateTransfers;
        private bool hasEditTransferOrdersPermission;
        private bool hasViewTransferRequestsPermission;
        private List<RecordIdentifier> newTransferOrderIdsFromHeadOffice;
        private List<RecordIdentifier> deletedTransferOrderIdsFromHeadOffice;
        private bool visualComponentsLoaded;
        private SiteServiceProfile siteServiceProfile;

        public TransferOrdersSendingPage()
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
            return new ViewPages.TransferOrdersSendingPage();
        }

        private InventoryTransferOrder SelectedTransferOrder { get; set; }
        private InventoryTransferOrderLine SelectedTransferOrderLine { get; set; }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            filterStoreDataEntities = (List<DataEntity>) internalContext;

            Parameters paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

            if (context != null && !context.IsEmpty && context != "")
            {
                InventoryTransferOrder initalTransferOrder = Providers.InventoryTransferOrderData.Get(PluginEntry.DataModel, context);
                SelectedTransferOrder = initalTransferOrder;
            }
            LoadTransfers(false);
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
                LoadTransfers(true);
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

        private void LoadTransfers(bool autoSize)
        {
            hasEditTransferOrdersPermission = PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders);
            hasViewTransferRequestsPermission = PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferRequests);

            btnsTransfers.AddButtonEnabled = hasEditTransferOrdersPermission && !PluginEntry.DataModel.IsHeadOffice;

            newTransferOrderIdsFromHeadOffice = new List<RecordIdentifier>();
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
                           InventoryTransferType.Sending,
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
                    errorProvider1.SetError(btnSend, Properties.Resources.UnableToConnectToSiteServiceSendingDisabled);
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
                    InventoryTransferType.Sending, InventoryTransferOrderSortEnum.Id, false);
                }

            foreach (InventoryTransferOrder transfer in transfers)
            {
                Row row = new Row {Tag = transfer};

                if (newTransferOrderIdsFromHeadOffice.Contains(transfer.ID))
                {
                    IconButton newButton = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider), Properties.Resources.New);
                    row.AddCell(new IconButtonCell(newButton, IconButtonCell.IconButtonIconAlignmentEnum.HorizontalCenter, "", false));
                }
                else
                {
                    row.AddText("");
                }

                row.AddText((string)transfer.ID);
                row.AddCell(new DateTimeCell(transfer.CreationDate.ToShortDateString() + " - " + transfer.CreationDate.ToShortTimeString(), transfer.CreationDate));
                row.AddText(transfer.ReceivingStoreName);
                row.AddText(transfer.SendingStoreName);
                row.AddCell(new CheckBoxCell(transfer.Sent, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                row.AddCell(new CheckBoxCell(transfer.FetchedByReceivingStore, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                IconButton deleteButton = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, DeleteOrderButtonEnabled(transfer));
                row.AddCell(new IconButtonCell(deleteButton, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));
                
                lvTransfers.AddRow(row);
                if (SelectedTransferOrder != null && transfer.ID == SelectedTransferOrder.ID)
                {
                    lvTransfers.Selection.Set(lvTransfers.RowCount - 1);
                }
            }

            if (autoSize)
            {
                lvTransfers.AutoSizeColumns(true);
            }
            lvTransfers.OnSelectionChanged(EventArgs.Empty);
        }

        private void LoadItems()
        {
            lvItems.ClearRows();
            List<InventoryTransferOrderLine> transferItems = Providers.InventoryTransferOrderLineData.GetListForInventoryTransfer(
                PluginEntry.DataModel,
                SelectedTransferOrder.ID, InventoryTransferOrderLineSortEnum.ItemName, false);

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
                row.AddCell(new NumericCell(item.QuantityRequested.FormatWithLimits(quantityLimiter) + " " + item.UnitName, item.QuantityRequested));
                row.AddCell(new CheckBoxCell(item.Sent, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, DeleteItemButtonEnabled(item));
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));
                
                lvItems.AddRow(row);
                if (SelectedTransferOrderLine != null && item.ID == SelectedTransferOrderLine.ID)
                {
                    lvItems.Selection.Set(lvItems.RowCount - 1);
                }
            }

            bool anyUnsentItem = transferItems.Any(item => !item.Sent);
            btnSend.Enabled = 
                !SelectedTransferOrder.FetchedByReceivingStore && 
                transferItems.Count > 0 && 
                anyUnsentItem &&
                ableToUpdateTransfers &&
                hasEditTransferOrdersPermission &&
                !PluginEntry.DataModel.IsHeadOffice;
            lvItems.AutoSizeColumns(true);
            lvItems.OnSelectionChanged(EventArgs.Empty);
        }

        private void lvTransfers_RightClick(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvTransfers.ContextMenuStrip;

            menu.Items.Clear();

            ExtendedMenuItem item = new ExtendedMenuItem(
                   Resources.Add,
                   100,
                   btnsTransfers_AddButtonClicked)
            {
                Enabled = btnsTransfers.AddButtonEnabled,
                Image = btnsTransfers.AddButtonImage
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Edit,
                   200,
                   btnsTransfers_EditButtonClicked)
            {
                Enabled = btnsTransfers.EditButtonEnabled,
                Image = btnsTransfers.EditButtonImage,
                Default = true
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Delete,
                   300,
                   btnsTransfers_RemoveButtonClicked)
            {
                Enabled = btnsTransfers.RemoveButtonEnabled,
                Image = btnsTransfers.RemoveButtonImage
            };
            menu.Items.Add(item);
            
            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                   btnSend.Text,
                   500,
                   btnSend_Click)
            {
                Enabled = btnSend.Enabled
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   btnViewTransferRequest.Text,
                   600,
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
                   Resources.Add,
                   100,
                   btnsItems_AddButtonClicked)
            {
                Enabled = btnsItems.AddButtonEnabled,
                Image = btnsItems.AddButtonImage
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Edit,
                   200,
                   btnsItems_EditButtonClicked)
            {
                Enabled = btnsItems.EditButtonEnabled,
                Image = btnsItems.EditButtonImage,
                Default = true
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Delete,
                   300,
                   btnsItems_RemoveButtonClicked)
            {
                Enabled = btnsItems.RemoveButtonEnabled,
                Image = btnsItems.RemoveButtonImage
            };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InventoryTransfersSendingItemsList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvTransfers_SelectionChanged(object sender, EventArgs e)
        {
            int rowsSelected = lvTransfers.Selection.Count;
            if (rowsSelected <= 0)
            {
                lblNoSelection.Visible = true;
                btnSend.Enabled = false;
                btnsTransfers.EditButtonEnabled = false;
                btnsTransfers.RemoveButtonEnabled = false;
            }
            else if (rowsSelected == 1)
            {
                SelectedTransferOrder = (InventoryTransferOrder) lvTransfers.Row(lvTransfers.Selection.FirstSelectedRow).Tag;
                lblNoSelection.Visible = false;
                btnsTransfers.EditButtonEnabled = !SelectedTransferOrder.Sent && hasEditTransferOrdersPermission && !PluginEntry.DataModel.IsHeadOffice;
                btnsTransfers.RemoveButtonEnabled = DeleteOrderButtonEnabled(SelectedTransferOrder);
                btnsItems.AddButtonEnabled = btnsTransfers.RemoveButtonEnabled;
                btnViewTransferRequest.Enabled = SelectedTransferOrder.InventoryTransferRequestId != "" && hasViewTransferRequestsPermission;
                LoadItems();
            }
            else
            {
                lblNoSelection.Visible = false;
                btnSend.Enabled = false;
                btnsTransfers.RemoveButtonEnabled = AllSelectedTransfersDeletable();
                btnsTransfers.EditButtonEnabled = false;
            }

            lvItems.Visible = !lblNoSelection.Visible;
            btnsItems.Visible = lvItems.Visible;
            lblItemHeader.Visible = lvItems.Visible;
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            int rowsSelected = lvItems.Selection.Count;
            if (rowsSelected <= 0)
            {
                SelectedTransferOrderLine = null;
                btnsItems.EditButtonEnabled = false;
                btnsItems.RemoveButtonEnabled = false;
            }
            else if (rowsSelected == 1)
            {
                SelectedTransferOrderLine = (InventoryTransferOrderLine)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag;
                btnsItems.RemoveButtonEnabled = DeleteItemButtonEnabled(SelectedTransferOrderLine);
                btnsItems.EditButtonEnabled = btnsItems.RemoveButtonEnabled;
            }
            else
            {
                SelectedTransferOrderLine = null;
                btnsItems.EditButtonEnabled = false;
                btnsItems.RemoveButtonEnabled = true;
            }
        }

        private bool DeleteOrderButtonEnabled(InventoryTransferOrder inventoryTransferOrder)
        {
            return hasEditTransferOrdersPermission
                   && (!inventoryTransferOrder.Sent || (!inventoryTransferOrder.FetchedByReceivingStore && ableToUpdateTransfers))
                   && !PluginEntry.DataModel.IsHeadOffice;
        }

        private bool DeleteItemButtonEnabled(InventoryTransferOrderLine inventoryTransferOrderLine)
        {
            return !inventoryTransferOrderLine.Sent && hasEditTransferOrdersPermission && !PluginEntry.DataModel.IsHeadOffice;
        }

        private bool AllSelectedTransfersDeletable()
        {
            return false;
        }

        private void lvTransfers_HeaderClicked(object sender, ColumnEventArgs args)
        {

        }

        private void  btnSend_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Resources.SendInventoryTransferQuestion, Resources.SendInventoryTransfer) == DialogResult.Yes)
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                try
                {
                    bool sendingSuccessfull = inventoryService.SendTransferOrder(PluginEntry.DataModel, SelectedTransferOrder.ID, siteServiceProfile);
                    if (!sendingSuccessfull)
                    {
                        MessageDialog.Show(Properties.Resources.UnableToSendTransferOrder, MessageBoxIcon.Error);
                    }
                }
                catch (Exception)
                {
                    MessageDialog.Show(Properties.Resources.ErrorSendingInventoryTransferOrder, MessageBoxIcon.Error);
                    return;
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", SelectedTransferOrder.ID, SelectedTransferOrder);
            }
        }

        private void btnsTransfers_AddButtonClicked(object sender, EventArgs e)
        {
            InventoryTransferSendingDialog dlg;

            if (!PluginEntry.DataModel.IsHeadOffice && filterStoreDataEntities.Count == 1)
            {
                dlg = new InventoryTransferSendingDialog(filterStoreDataEntities[0]);
            }
            else
            {
                dlg = new InventoryTransferSendingDialog();
            }
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SelectedTransferOrder = dlg.InventoryTransferOrder;
                LoadTransfers(true);
            }
        }

        private void btnsTransfers_EditButtonClicked(object sender, EventArgs e)
        {
            InventoryTransferSendingDialog dlg = new InventoryTransferSendingDialog(SelectedTransferOrder);
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadTransfers(true);
            }
        }

        private void btnsTransfers_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Properties.Resources.DeleteInventoryTransferQuestion, Properties.Resources.DeleteInventoryTransfer) == DialogResult.Yes)
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                bool result = inventoryService.DeleteTransferOrder(PluginEntry.DataModel, SelectedTransferOrder.ID, siteServiceProfile);

                if (result)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "InventoryTransferOrder", SelectedTransferOrder.ID, null);
                }
                else
                {
                    MessageDialog.Show(Properties.Resources.UnableToDeleteTransferMessage, Properties.Resources.UnableToDeleteTransfer, MessageBoxIcon.Error);
                }

                LoadTransfers(true);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "InventoryTransfer", SelectedTransferOrder.ID, null);
            }
        }

        private void btnsItems_AddButtonClicked(object sender, EventArgs e)
        {
            InventoryTransferOrderItemDialog dlg = new InventoryTransferOrderItemDialog(SelectedTransferOrder.ID);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SelectedTransferOrderLine = dlg.InventoryTransferOrderLine;
                LoadItems();
            }
        }

        private void btnsItems_EditButtonClicked(object sender, EventArgs e)
        {
            InventoryTransferOrderItemDialog dlg = new InventoryTransferOrderItemDialog(SelectedTransferOrderLine);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void btnsItems_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvItems.Selection.Count == 1)
            {
                RemoveRequestLine(SelectedTransferOrderLine.ID);
            }
            else
            {
                List<RecordIdentifier> selectedIDs = new List<RecordIdentifier>();

                for (int i = 0; i < lvItems.Selection.Count; i++)
                {
                    selectedIDs.Add(((InventoryTransferOrderLine)lvItems.Selection[i].Tag).ID);
                }

                RemoveRequestLines(selectedIDs);
            }
        }

        private void RemoveRequestLine(RecordIdentifier lineID)
        {
            if (QuestionDialog.Show(Properties.Resources.RemoveLineQuestion, Properties.Resources.RemoveLine) == DialogResult.Yes)
            {
                Providers.InventoryTransferOrderLineData.Delete(PluginEntry.DataModel, lineID);
                LoadItems();
            }
        }

        private void RemoveRequestLines(List<RecordIdentifier> lineIDs)
        {
            if (QuestionDialog.Show(Properties.Resources.RemoveLinesQuestion, Properties.Resources.RemoveLines) == DialogResult.Yes)
            {
                foreach (RecordIdentifier line in lineIDs)
                {
                    Providers.InventoryTransferOrderLineData.Delete(PluginEntry.DataModel, line);
                }
                LoadItems();
            }
        }

        private void lvTransfers_CellAction(object sender, CellEventArgs args)
        {
            if (btnsTransfers.RemoveButtonEnabled)
            {
                btnsTransfers_RemoveButtonClicked(null, EventArgs.Empty);
            }
        }

        private void lvItems_CellAction(object sender, CellEventArgs args)
        {
            if (btnsItems.RemoveButtonEnabled)
            {
                btnsItems_RemoveButtonClicked(null, EventArgs.Empty);
            }
        }

        private void btnViewTransferRequest_Click(object sender, EventArgs e)
        {
            bool showOrderSuccessful = PluginOperations.ShowInventoryTransferRequestsView(SelectedTransferOrder.InventoryTransferRequestId, false);
            if (!showOrderSuccessful)
            {
                MessageDialog.Show(Properties.Resources.NoInventoryTransferRequestFound, MessageBoxIcon.Information);
            }
        }

        private void lvTransfers_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsTransfers.EditButtonEnabled)
            {
                btnsTransfers_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvItems_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsItems.EditButtonEnabled)
            {
                btnsItems_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}
