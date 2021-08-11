using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
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
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.Controls;
using System.Linq;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class InventoryInTransitView : ViewBase
    {
        private bool hasViewTransferOrderPermission;

        public InventoryInTransitView()
        {
            InitializeComponent();
            
            lvInventoryInTransfer.ContextMenuStrip = new ContextMenuStrip();
            lvInventoryInTransfer.ContextMenuStrip.Opening += lvInventoryInTransit_RightClick;

            HeaderText = Resources.InventoryInTransit;
            lvInventoryInTransfer.Columns[0].Tag = InventoryTransferOrderSortEnum.Id;
            lvInventoryInTransfer.Columns[1].Tag = InventoryTransferOrderSortEnum.Description;
            lvInventoryInTransfer.Columns[2].Tag = InventoryTransferOrderSortEnum.CreatedDate;
            lvInventoryInTransfer.Columns[3].Tag = InventoryTransferOrderSortEnum.SendingStore;
            lvInventoryInTransfer.Columns[4].Tag = InventoryTransferOrderSortEnum.ReceivingStore;
            lvInventoryInTransfer.SetSortColumn(lvInventoryInTransfer.Columns[0], false);
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.InventoryInTransit;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "InventoryTransferOrder")
            {
                LoadLines();
            }
        }

        protected override void LoadData(bool isRevert)
        {
            LoadLines();
        }

        private void LoadLines()
        {
            hasViewTransferOrderPermission =
                PluginEntry.DataModel.HasPermission(
                    Permission.ViewInventoryTransferOrders);

            try
            {
                var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                lvInventoryInTransfer.ClearRows();

                //Show inventory for current store if you don't have permission
                RecordIdentifier storeID = (PluginEntry.DataModel.IsHeadOffice || PluginEntry.DataModel.HasPermission(Permission.ManageTransfersForAllStores)) ? null : PluginEntry.DataModel.CurrentStoreID;

                List<InventoryTransferOrder> inventoryTransferOrders =
                    service.GetInventoryInTransit(PluginEntry.DataModel,
                        PluginOperations.GetSiteServiceProfile(), (InventoryTransferOrderSortEnum)lvInventoryInTransfer.SortColumn.Tag,
                        !lvInventoryInTransfer.SortedAscending, storeID, false);

                Dictionary<RecordIdentifier, decimal> totalUnreceivedItems = 
                    service.GetTotalUnreceivedItemForTransferOrders(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), inventoryTransferOrders.Select(x => x.ID).ToList(), true);

                DecimalLimit quantityLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

                foreach (var transfer in inventoryTransferOrders)
                {
                    Row row = new Row();
                    row.Tag = transfer.ID;
                    row.AddText((string)transfer.ID);
                    row.AddText(transfer.Text);
                    row.AddText(transfer.SentDate.ToShortDateString() + " - " + transfer.SentDate.ToShortTimeString());
                    row.AddText(transfer.SendingStoreName);
                    row.AddText(transfer.ReceivingStoreName);
                    row.AddText(totalUnreceivedItems[transfer.ID].FormatWithLimits(quantityLimit));
                    lvInventoryInTransfer.AddRow(row);
                }

                service.Disconnect(PluginEntry.DataModel);
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            lvInventoryInTransfer.AutoSizeColumns();
        }

        private void lvInventoryInTransfer_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (lvInventoryInTransfer.SortColumn == args.Column)
            {
                lvInventoryInTransfer.SetSortColumn(args.Column, !lvInventoryInTransfer.SortedAscending);
            }
            else
            {
                lvInventoryInTransfer.SetSortColumn(args.Column, true);
            }

            LoadLines();
        }

        private void lvInventoryInTransfer_DoubleClick(object sender, System.EventArgs e)
        {
            if (btnViewInventoryTransfer.Enabled)
            {
                btnViewInventoryTransfer_Click(sender, e);
            }
        }

        private void btnViewInventoryTransfer_Click(object sender, System.EventArgs e)
        {
            var selectedInventoryTransferId =
                (RecordIdentifier) lvInventoryInTransfer.Row(lvInventoryInTransfer.Selection.FirstSelectedRow).Tag;

            try
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                InventoryTransferOrder order = inventoryService.GetInventoryTransferOrder(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), selectedInventoryTransferId, true);

                if(order != null)
                {
                    InventoryTransferType type = InventoryTransferType.Outgoing;
                    if(order.Rejected || order.Received)
                    {
                        type = InventoryTransferType.Finished;
                    }
                    else if(order.Sent && !order.Received)
                    {
                        type = InventoryTransferType.Incoming;
                    }

                    PluginEntry.Framework.ViewController.Add(new StoreTransferView(order, type));
                }
                else
                {
                    MessageDialog.Show(Resources.NoInventoryTransferOrderFound, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
        }

        private void lvInventoryInTransit_RightClick(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvInventoryInTransfer.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                   btnViewInventoryTransfer.Text,
                   200,
                   btnViewInventoryTransfer_Click)
            {
                Enabled = btnViewInventoryTransfer.Enabled,
                Default = true
            };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InventoryInTransitList", lvInventoryInTransfer.ContextMenuStrip, lvInventoryInTransfer);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvInventoryInTransfer_SelectionChanged(object sender, System.EventArgs e)
        {
            btnViewInventoryTransfer.Enabled = (lvInventoryInTransfer.Selection.Count > 0) && hasViewTransferOrderPermission;
        } 
    }
}
