using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using LSOne.ViewPlugins.Inventory.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    internal partial class TransferRequestReceivingPage : UserControl, ITabView
    {
        private List<DataEntity> filterStoreDataEntities;
        private DecimalLimit quantityLimiter;
        private bool hasEditRequestsPermission;
        private bool hasViewOrdersPermission;
        private List<RecordIdentifier> newTransferRequestIdsFromHeadOffice;
        private List<RecordIdentifier> deletedTransferRequestIdsFromHeadOffice;
        private bool visualComponentsLoaded;
        private SiteServiceProfile siteServiceProfile;

        public TransferRequestReceivingPage()
        {
            InitializeComponent();

            lvRequests.ContextMenuStrip = new ContextMenuStrip();
            lvRequests.ContextMenuStrip.Opening += lvRequests_RightClick;

            quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
            newTransferRequestIdsFromHeadOffice = new List<RecordIdentifier>();
            deletedTransferRequestIdsFromHeadOffice = new List<RecordIdentifier>();
            visualComponentsLoaded = false;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.TransferRequestReceivingPage();
        }

        private InventoryTransferRequest SelectedTransferRequest { get; set; }
        private InventoryTransferRequestLine SelectedTransferRequestLine { get; set; }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            filterStoreDataEntities = (List<DataEntity>)internalContext;

            Parameters paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

            if (context != null && !context.IsEmpty && context != "")
            {
                InventoryTransferRequest initalTransferRequest = Providers.InventoryTransferRequestData.Get(PluginEntry.DataModel, context);
                SelectedTransferRequest = initalTransferRequest;
            }
            LoadRequests();
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
                LoadRequests();
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

            // This logic needs to run after the visual components are drawn on the screen
            if (!DesignMode)
            {
                lvRequests.AutoSizeColumns(true);

                BeginInvoke((MethodInvoker) delegate
                {
                    if (deletedTransferRequestIdsFromHeadOffice.Count > 0)
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
                lvRequests.AutoSizeColumns(true);
            }
        }

        private void LoadRequests()
        {
            hasEditRequestsPermission = PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests);
            hasViewOrdersPermission = PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferOrders);

            newTransferRequestIdsFromHeadOffice = new List<RecordIdentifier>();
            deletedTransferRequestIdsFromHeadOffice = new List<RecordIdentifier>();

            lvRequests.ClearRows();
            List<InventoryTransferRequest> requests = new List<InventoryTransferRequest>(); 
            List<RecordIdentifier> storeIds = filterStoreDataEntities.Select(store => store.ID).ToList();
            if (storeIds.Count > 0)
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                // Refresh inventory requests from HO if we are not on HO
                if (!PluginEntry.DataModel.IsHeadOffice)
                {
                    inventoryService.UpdateTransferRequestsForStores(
                       PluginEntry.DataModel,
                       storeIds,
                       InventoryTransferType.ToReceive,
                       siteServiceProfile,
                       out newTransferRequestIdsFromHeadOffice,
                       out deletedTransferRequestIdsFromHeadOffice);
                }

                if (deletedTransferRequestIdsFromHeadOffice.Count > 0 && visualComponentsLoaded)
                {
                    ShowDeletedFromHeadOfficeMessage();
                }
                
                requests = Providers.InventoryTransferRequestData.GetListForStore(
                    PluginEntry.DataModel, 
                    storeIds,
                    InventoryTransferType.ToReceive, InventoryTransferOrderSortEnum.Id, false);
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

                row.AddText((string)request.ID);
                row.AddCell(new DateTimeCell(request.SentDate.ToShortDateString() + " - " + request.SentDate.ToShortTimeString(), request.SentDate));
                row.AddText(request.SendingStoreName);
                row.AddText(request.ReceivingStoreName);
                row.AddCell(new CheckBoxCell(request.InventoryTransferOrderCreated, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));

                IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, DeleteRequestButtonEnabled(request));
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));

                lvRequests.AddRow(row);
                if (SelectedTransferRequest != null && request.ID == SelectedTransferRequest.ID)
                {
                    lvRequests.Selection.Set(lvRequests.RowCount - 1);
                }
            }

            lvRequests.AutoSizeColumns(true);
            lvRequests.OnSelectionChanged(EventArgs.Empty);
        }

        private void LoadItems()
        {
            lvItems.ClearRows();
            
            List<InventoryTransferRequestLine> transferItems = Providers.InventoryTransferRequestLineData.GetListForInventoryTransferRequest(
                PluginEntry.DataModel,
                SelectedTransferRequest.ID,
                InventoryTransferOrderLineSortEnum.ItemName,
                false);

            foreach (InventoryTransferRequestLine item in transferItems)
            {
                Row row = new Row {Tag = item};
                row.AddText((string)item.ItemId);
                row.AddText(item.ItemName);
                row.AddText(item.VariantName);
                row.AddCell(new NumericCell(item.QuantityRequested.FormatWithLimits(quantityLimiter) + " " + item.UnitName, item.QuantityRequested));
                lvItems.AddRow(row);
                if (SelectedTransferRequestLine != null && item.ID == SelectedTransferRequestLine.ID)
                {
                    lvItems.Selection.Set(lvRequests.RowCount - 1);
                }
            }
            lvItems.AutoSizeColumns(true);
            lvItems.OnSelectionChanged(EventArgs.Empty);
        }

        private void lvTransfers_SelectionChanged(object sender, EventArgs e)
        {
            int rowsSelected = lvRequests.Selection.Count;
            if (rowsSelected <= 0)
            {
                lblNoSelection.Visible = true;
                btnCreateTransfer.Enabled = false;
                btnViewTransferOrder.Enabled = false;
            }
            else if (rowsSelected == 1)
            {
                SelectedTransferRequest = (InventoryTransferRequest) lvRequests.Row(lvRequests.Selection.FirstSelectedRow).Tag;
                lblNoSelection.Visible = false;
                btnCreateTransfer.Enabled = !SelectedTransferRequest.InventoryTransferOrderCreated && hasEditRequestsPermission && !PluginEntry.DataModel.IsHeadOffice;
                btnViewTransferOrder.Enabled = SelectedTransferRequest.InventoryTransferOrderCreated && hasViewOrdersPermission;
                btnRemoveRequest.Enabled = DeleteRequestButtonEnabled(SelectedTransferRequest);
                LoadItems();
            }
            else
            {
                SelectedTransferRequest = null;
                lblNoSelection.Visible = false;
                btnCreateTransfer.Enabled = false;
                btnViewTransferOrder.Enabled = false;
            }

            lvItems.Visible = !lblNoSelection.Visible;
            lblItemHeader.Visible = lvItems.Visible;
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            int rowsSelected = lvItems.Selection.Count;
            if (rowsSelected <= 0)
            {
                SelectedTransferRequestLine = null;
            }
            else if (rowsSelected == 1)
            {
                SelectedTransferRequestLine = (InventoryTransferRequestLine)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag;
            }
            else
            {
                SelectedTransferRequestLine = null;
            }
        }

        private void btnCreateTransfer_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Resources.CreateInventoryTransferQuestion, Resources.Active) == DialogResult.Yes)
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                RecordIdentifier newInventoryTransferOrderId;

                try
                {
                    newInventoryTransferOrderId = inventoryService.CreateInventoryTransferRequest(PluginEntry.DataModel, SelectedTransferRequest.ID, siteServiceProfile);
                }
                catch (DataException)
                {
                    MessageDialog.Show(Properties.Resources.CannotCreateInventoryTransferRequestDescription, Properties.Resources.CannoCreateInventoryTransferRequest, MessageBoxIcon.Exclamation);
                    LoadRequests();
                    return;
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit,
                                                                       "InventoryTransferRequest",
                                                                       SelectedTransferRequest.ID, SelectedTransferRequest);
                LoadRequests();
                PluginOperations.ShowInventoryTransferOrdersView(newInventoryTransferOrderId, true);
            }
        }

        private void lvRequests_RightClick(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvRequests.ContextMenuStrip;

            menu.Items.Clear();

            ExtendedMenuItem item = new ExtendedMenuItem(
                   btnCreateTransfer.Text,
                   100,
                   btnCreateTransfer_Click)
            {
                Enabled = btnCreateTransfer.Enabled
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   btnViewTransferOrder.Text,
                   200,
                   btnViewTransferOrder_Click)
            {
                Enabled = btnViewTransferOrder.Enabled
            };
            menu.Items.Add(item);
            item = new ExtendedMenuItem(
                  Resources.Delete,
                  300,
                  btnRemoveRequest_Click)
            {
                Enabled = btnRemoveRequest.Enabled,
                Image = btnRemoveRequest.Image
            };
            menu.Items.Add(item);
            PluginEntry.Framework.ContextMenuNotify("InventoryRequestsReceivingList", lvRequests.ContextMenuStrip, lvRequests);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnViewTransferOrder_Click(object sender, EventArgs e)
        {
            bool showOrderSuccessful = PluginOperations.ShowInventoryTransferOrdersView(SelectedTransferRequest.InventoryTransferOrderId, true);
            if (!showOrderSuccessful)
            {
                MessageDialog.Show(Properties.Resources.NoInventoryTransferOrderFound, MessageBoxIcon.Information);
            }
        }

        private void btnRemoveRequest_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Properties.Resources.DeleteInventoryTransferRequestQuestion, Properties.Resources.DeleteInventoryTransferRequest) == DialogResult.Yes)
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                bool result = inventoryService.DeleteTransferRequestByStore(
                    PluginEntry.DataModel,
                    SelectedTransferRequest.ID,
                    PluginEntry.DataModel.CurrentStoreID,
                    siteServiceProfile
                    );

                if (result)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "InventoryTransferRequest", SelectedTransferRequest.ID, null);
                }
                else
                {
                    MessageDialog.Show(Properties.Resources.UnableToDeleteRequestMessage, Properties.Resources.UnableToDeleteRequest, MessageBoxIcon.Error);
                }
                LoadRequests();
            }
        }

        private bool DeleteRequestButtonEnabled(InventoryTransferRequest inventoryTransferRequest)
        {
            return hasEditRequestsPermission
                && !inventoryTransferRequest.InventoryTransferOrderCreated
                && !PluginEntry.DataModel.IsHeadOffice;
        }

        private void lvRequests_CellAction(object sender, CellEventArgs args)
        {
            if (btnRemoveRequest.Enabled)
            {
                btnRemoveRequest_Click(null, EventArgs.Empty);
            }
        }
    }
}
