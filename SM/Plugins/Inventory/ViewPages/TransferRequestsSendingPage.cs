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
    internal partial class TransferRequestsSendingPage : UserControl, ITabView
    {
        private List<DataEntity> filterStoreDataEntities;
        private DecimalLimit quantityLimiter;
        private bool ableToUpdateRequests;
        private bool hasEditRequestsPermission;
        private bool hasViewOrdersPermission;
        private List<RecordIdentifier> newTransferRequestIdsFromHeadOffice;
        private List<RecordIdentifier> deletedTransferRequestIdsFromHeadOffice;
        private bool visualComponentsLoaded;
        private SiteServiceProfile siteServiceProfile;

        public TransferRequestsSendingPage()
        {
            InitializeComponent();

            lvRequests.ContextMenuStrip = new ContextMenuStrip();
            lvRequests.ContextMenuStrip.Opening += lvRequests_RightClick;
            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += lvItems_RightClick;

            quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
            newTransferRequestIdsFromHeadOffice = new List<RecordIdentifier>();
            deletedTransferRequestIdsFromHeadOffice = new List<RecordIdentifier>();
            visualComponentsLoaded = false;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.TransferRequestsSendingPage();
        }

        private InventoryTransferRequest SelectedTransferRequest { get; set; }
        private InventoryTransferRequestLine SelectedTransferRequestLine { get; set; }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            filterStoreDataEntities = (List<DataEntity>) internalContext;

            siteServiceProfile = PluginOperations.GetSiteServiceProfile(PluginEntry.DataModel);

            if (context != null && !context.IsEmpty && context != "")
            {
                InventoryTransferRequest initalTransferRequest = Providers.InventoryTransferRequestData.Get(PluginEntry.DataModel, context);
                SelectedTransferRequest = initalTransferRequest;
            }

            LoadRequests(false);
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

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "InventoryTransferRequest")
            {
                LoadRequests(true);
            }
        }

        #endregion

        private void ShowDeletedFromHeadOfficeMessage()
        {
            MessageDialog.Show(
                        Properties.Resources.TransferRequestsDeletedFromHeadOfficeDescription.Replace("#1", deletedTransferRequestIdsFromHeadOffice.Count.ToString()),
                        Properties.Resources.TransferRequestsDeletedFromHeadOffice,
                        MessageBoxIcon.Exclamation);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!DesignMode)
            {
                lvRequests.AutoSizeColumns(true);
                BeginInvoke((MethodInvoker)delegate
                {
                    visualComponentsLoaded = true;
                    if (deletedTransferRequestIdsFromHeadOffice.Count > 0)
                    {
                        ShowDeletedFromHeadOfficeMessage();
                    }
                });
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (!DesignMode)
            {
                lvRequests.AutoSizeColumns(true);
            }
        }

        private void LoadRequests(bool autoSize)
        {
            hasEditRequestsPermission = PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests);

            hasViewOrdersPermission = PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferOrders);

            btnsRequests.AddButtonEnabled = hasEditRequestsPermission && !PluginEntry.DataModel.IsHeadOffice;

            newTransferRequestIdsFromHeadOffice = new List<RecordIdentifier>();
            deletedTransferRequestIdsFromHeadOffice = new List<RecordIdentifier>();

            lvRequests.ClearRows();
            List<InventoryTransferRequest> requests = new List<InventoryTransferRequest>();
            List<RecordIdentifier> storeIds = filterStoreDataEntities.Select(store => store.ID).ToList();
            if (storeIds.Count > 0)
            {
                IInventoryService inventoryService = (IInventoryService) PluginEntry.DataModel.Service(ServiceType.InventoryService);

                // Refresh inventory requests from HO if we are not on HO
                if (!PluginEntry.DataModel.IsHeadOffice)
                {

                    ableToUpdateRequests = inventoryService.UpdateTransferRequestsForStores(
                        PluginEntry.DataModel,
                        storeIds,
                        InventoryTransferType.Sending,
                        siteServiceProfile,
                        out newTransferRequestIdsFromHeadOffice,
                        out deletedTransferRequestIdsFromHeadOffice);
                }
                else
                {
                    ableToUpdateRequests = true;
                }

                if (!ableToUpdateRequests)
                {
                    errorProvider1.SetError(btnSendRequest, Properties.Resources.UnableToConnectToSiteServiceSendingDisabled);
                }
                else
                {
                    errorProvider1.Clear();
                }

                if (deletedTransferRequestIdsFromHeadOffice.Count > 0 && visualComponentsLoaded)
                {
                    ShowDeletedFromHeadOfficeMessage();
                }


                requests = Providers.InventoryTransferRequestData.GetListForStore(
                    PluginEntry.DataModel,
                    storeIds,
                    InventoryTransferType.Sending, InventoryTransferOrderSortEnum.Id, false);
            }

            foreach (InventoryTransferRequest request in requests)
            {
                Row row = new Row {Tag = request};

                if (newTransferRequestIdsFromHeadOffice.Contains(request.ID))
                {
                    IconButton newButton = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider), Properties.Resources.New);
                    row.AddCell(new IconButtonCell(newButton, IconButtonCell.IconButtonIconAlignmentEnum.HorizontalCenter, "", false));
                }
                else
                {
                    row.AddText("");
                }

                row.AddText((string) request.ID);
                row.AddCell(new DateTimeCell(request.CreationDate.ToShortDateString() + " - " + request.CreationDate.ToShortTimeString(), request.CreationDate));
                row.AddText(request.ReceivingStoreName);
                row.AddText(request.SendingStoreName);
                row.AddCell(new CheckBoxCell(request.Sent, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                row.AddCell(new CheckBoxCell(request.FetchedByReceivingStore, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, DeleteRequestButtonEnabled(request));
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));

                lvRequests.AddRow(row);
                if (SelectedTransferRequest != null && request.ID == SelectedTransferRequest.ID)
                {
                    lvRequests.Selection.Set(lvRequests.RowCount - 1);
                }
            }

            if (autoSize)
            {
                lvRequests.AutoSizeColumns(true);
            }
            lvRequests.OnSelectionChanged(EventArgs.Empty);
        }

        private void LoadItems()
        {
            lvItems.ClearRows();

            List<InventoryTransferRequestLine> transferItems = null;
            try
            {
                //transferItems = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetListForInventoryTransferRequest(
                //                PluginEntry.DataModel, 
                //                PluginOperations.GetSiteServiceProfile(), 
                //                SelectedTransferRequest.ID,
                //                InventoryTransferOrderLineSortEnum.ItemName,
                //                false, 
                //                true);

                transferItems =  Providers.InventoryTransferRequestLineData.GetListForInventoryTransferRequest(
                    PluginEntry.DataModel,
                    SelectedTransferRequest.ID,
                    InventoryTransferOrderLineSortEnum.ItemName,
                    false);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
          
            foreach (InventoryTransferRequestLine item in transferItems)
            {
                Row row = new Row {Tag = item};
                row.AddText((string)item.ItemId);
                row.AddText(item.ItemName);
                row.AddText(item.VariantName);
                row.AddCell(new NumericCell(item.QuantityRequested.FormatWithLimits(quantityLimiter) + " " + item.UnitName, item.QuantityRequested));
                row.AddCell(new CheckBoxCell(item.Sent, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, DeleteItemButtonEnabled(item));
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));
                
                lvItems.AddRow(row);
                if (SelectedTransferRequestLine != null && item.ID == SelectedTransferRequestLine.ID)
                {
                    lvItems.Selection.Set(lvItems.RowCount - 1);
                }
            }

            bool anyUnsentItem = transferItems.Any(item => !item.Sent);

            btnSendRequest.Enabled = 
                !SelectedTransferRequest.FetchedByReceivingStore && 
                transferItems.Count > 0 &&
                anyUnsentItem &&
                ableToUpdateRequests &&
                hasEditRequestsPermission &&
                !PluginEntry.DataModel.IsHeadOffice;

            lvItems.AutoSizeColumns(true);
            lvItems.OnSelectionChanged(EventArgs.Empty);
        }

        private void lvRequests_RightClick(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvRequests.ContextMenuStrip;

            menu.Items.Clear();

            ExtendedMenuItem item = new ExtendedMenuItem(
                   Resources.Add,
                   100,
                   btnsRequests_AddButtonClicked)
            {
                Enabled = btnsRequests.AddButtonEnabled,
                Image = btnsRequests.AddButtonImage
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Edit,
                   200,
                   btnsRequests_EditButtonClicked)
            {
                Enabled = btnsRequests.EditButtonEnabled,
                Image = btnsRequests.EditButtonImage,
                Default = true
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Delete,
                   300,
                   btnsRequests_RemoveButtonClicked)
            {
                Enabled = btnsRequests.RemoveButtonEnabled,
                Image = btnsRequests.RemoveButtonImage
            };
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                   btnSendRequest.Text,
                   500,
                   btnRequest_Click)
            {
                Enabled = btnSendRequest.Enabled
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   btnViewTransferOrder.Text,
                   500,
                   btnViewTransferOrder_Click)
            {
                Enabled = btnViewTransferOrder.Enabled
            };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InventoryRequestsSendingList", lvRequests.ContextMenuStrip, lvRequests);

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

            PluginEntry.Framework.ContextMenuNotify("InventoryRequestsSendingItemsList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvRequests_SelectionChanged(object sender, EventArgs e)
        {
            int rowsSelected = lvRequests.Selection.Count;
            if (rowsSelected <= 0)
            {
                lblNoSelection.Visible = true;
                btnSendRequest.Enabled = false;
                btnsRequests.EditButtonEnabled = false;
                btnsRequests.RemoveButtonEnabled = false;
                btnViewTransferOrder.Enabled = false;
            }
            else if (rowsSelected == 1)
            {
                SelectedTransferRequest = (InventoryTransferRequest) lvRequests.Row(lvRequests.Selection.FirstSelectedRow).Tag;
                lblNoSelection.Visible = false;

                btnsRequests.EditButtonEnabled = !SelectedTransferRequest.Sent && hasEditRequestsPermission && !PluginEntry.DataModel.IsHeadOffice;
                btnsRequests.RemoveButtonEnabled = DeleteRequestButtonEnabled(SelectedTransferRequest);
                btnsItems.AddButtonEnabled = btnsRequests.RemoveButtonEnabled;

                btnViewTransferOrder.Enabled = 
                    SelectedTransferRequest.InventoryTransferOrderId != "" 
                    && hasViewOrdersPermission
                    && Providers.InventoryTransferOrderData.Get(PluginEntry.DataModel, SelectedTransferRequest.InventoryTransferOrderId) != null ;

                LoadItems();
            }
            else
            {
                lblNoSelection.Visible = false;
                btnSendRequest.Enabled = false;
                btnsRequests.RemoveButtonEnabled = AllSelectedTransfersDeletable();
                btnsRequests.EditButtonEnabled = false;
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
                SelectedTransferRequestLine = null;
                btnsItems.EditButtonEnabled = false;
                btnsItems.RemoveButtonEnabled = false;
            }
            else if (rowsSelected == 1)
            {
                SelectedTransferRequestLine = (InventoryTransferRequestLine)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag;
                btnsItems.RemoveButtonEnabled = DeleteItemButtonEnabled(SelectedTransferRequestLine);
                btnsItems.EditButtonEnabled = btnsItems.RemoveButtonEnabled;
            }
            else
            {
                SelectedTransferRequestLine = null;
                btnsItems.EditButtonEnabled = false;
                btnsItems.RemoveButtonEnabled = true;
            }
        }

        private bool DeleteRequestButtonEnabled(InventoryTransferRequest inventoryTransferRequest)
        {
            return hasEditRequestsPermission
                && (!inventoryTransferRequest.Sent || (!inventoryTransferRequest.FetchedByReceivingStore && ableToUpdateRequests))
                && !PluginEntry.DataModel.IsHeadOffice;
        }

        private bool DeleteItemButtonEnabled(InventoryTransferRequestLine inventoryTransferRequestLine)
        {
            return !inventoryTransferRequestLine.Sent && hasEditRequestsPermission && !PluginEntry.DataModel.IsHeadOffice;
        }

        private bool AllSelectedTransfersDeletable()
        {
            return false;
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Resources.SendInventoryTransferRequestQuestion, Resources.SendInventoryTransferRequest) == DialogResult.Yes)
            {
                // We both send the send message to the store server and also perform the operation locally
                IInventoryService inventoryService = (IInventoryService) PluginEntry.DataModel.Service(ServiceType.InventoryService);

                try
                {
                    bool sendingSuccessfull = inventoryService.SendTransferRequest(PluginEntry.DataModel, SelectedTransferRequest.ID, siteServiceProfile);
                    if (!sendingSuccessfull)
                    {
                        MessageDialog.Show(Resources.UnableToSendRequest, MessageBoxIcon.Error);
                    }
                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.ErrorSendingInventoryTransferRequest, MessageBoxIcon.Error);
                    return;
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", SelectedTransferRequest.ID, SelectedTransferRequest);
            }
        }

        private void btnsRequests_AddButtonClicked(object sender, EventArgs e)
        {
            InventoryTransferRequestDialog dlg;

            if (!PluginEntry.DataModel.IsHeadOffice && filterStoreDataEntities.Count == 1)
            {
                dlg = new InventoryTransferRequestDialog(filterStoreDataEntities.FirstOrDefault());
            }
            else
            {
                dlg = new InventoryTransferRequestDialog();
            }
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SelectedTransferRequest = dlg.TransferRequest;
                LoadRequests(true);
            }
        }

        private void btnsRequests_EditButtonClicked(object sender, EventArgs e)
        {
            InventoryTransferRequestDialog dlg = new InventoryTransferRequestDialog(SelectedTransferRequest.ID);
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadRequests(true);
            }
        }

        private void btnsRequests_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Properties.Resources.DeleteInventoryTransferRequestQuestion, Properties.Resources.DeleteInventoryTransferRequest) == DialogResult.Yes)
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                bool result = inventoryService.DeleteTransferRequestByStore(
                    PluginEntry.DataModel, 
                    SelectedTransferRequest.ID, 
                    PluginEntry.DataModel.CurrentStoreID, 
                    siteServiceProfile);

                if (result)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "InventoryTransferRequest", SelectedTransferRequest.ID, null);
                }
                else
                {
                    MessageDialog.Show(Resources.UnableToDeleteRequestMessage, Properties.Resources.UnableToDeleteRequest, MessageBoxIcon.Error);
                }
                LoadRequests(true);
            }
        }

        private void btnsItems_AddButtonClicked(object sender, EventArgs e)
        {
            InventoryTransferRequestItemDialog dlg = new InventoryTransferRequestItemDialog(SelectedTransferRequest.ID);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SelectedTransferRequestLine = dlg.InventoryTransferRequestLine;
                LoadItems();
            }
        }

        private void btnsItems_EditButtonClicked(object sender, EventArgs e)
        {
            InventoryTransferRequestItemDialog dlg = new InventoryTransferRequestItemDialog(SelectedTransferRequestLine);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void btnsItems_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvItems.Selection.Count == 1)
            {
                RemoveRequestLine(SelectedTransferRequestLine.ID);
            }
            else
            {
                List<RecordIdentifier> selectedIDs = new List<RecordIdentifier>();

                for (int i = 0; i < lvItems.Selection.Count; i++)
                {
                    selectedIDs.Add(((InventoryTransferRequestLine)lvItems.Selection[i].Tag).ID);
                }

                RemoveRequestLines(selectedIDs);
            }
        }

        private void RemoveRequestLine(RecordIdentifier lineID)
        {
            if (QuestionDialog.Show(Properties.Resources.RemoveLineQuestion, Properties.Resources.RemoveLine) == DialogResult.Yes)
            {
                Providers.InventoryTransferRequestLineData.Delete(PluginEntry.DataModel, SelectedTransferRequestLine.ID);
                LoadItems();
            }
        }

        private void RemoveRequestLines(List<RecordIdentifier> lineIDs)
        {
            if (QuestionDialog.Show(Properties.Resources.RemoveLinesQuestion, Properties.Resources.RemoveLines) == DialogResult.Yes)
            {
                try
                {
                    foreach (RecordIdentifier lineID in lineIDs)
                    {
                        Providers.InventoryTransferRequestLineData.Delete(PluginEntry.DataModel, lineID);
                    }
                    
                    //Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).
                    //    DeleteInventoryTransferRequestLines(
                    //        PluginEntry.DataModel,
                    //        PluginOperations.GetSiteServiceProfile(),
                    //        lineIDs,
                    //        true);
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                    return;
                }

                LoadItems();
            }
        }

        private void lvRequests_CellAction(object sender, CellEventArgs args)
        {
            if (btnsRequests.RemoveButtonEnabled)
            {
                btnsRequests_RemoveButtonClicked(null, EventArgs.Empty);
            }
        }

        private void lvItems_CellAction(object sender, CellEventArgs args)
        {
            if (btnsItems.RemoveButtonEnabled)
            {
                btnsItems_RemoveButtonClicked(null, EventArgs.Empty);
            }
        }

        private void btnViewTransferOrder_Click(object sender, EventArgs e)
        {
            bool showOrderSuccessful = PluginOperations.ShowInventoryTransferOrdersView(SelectedTransferRequest.InventoryTransferOrderId, false);
            if (!showOrderSuccessful)
            {
                MessageDialog.Show(Properties.Resources.NoInventoryTransferOrderFound, MessageBoxIcon.Information);
            }
        }

        private void lvRequests_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsRequests.EditButtonEnabled)
            {
                btnsRequests_EditButtonClicked(this, EventArgs.Empty);
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
