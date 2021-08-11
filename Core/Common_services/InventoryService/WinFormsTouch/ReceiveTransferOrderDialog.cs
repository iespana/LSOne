using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    /// <summary>
    /// See list of incoming transfer requests.
    /// </summary>
    public partial class ReceiveTransferOrderDialog : TouchBaseForm
    {
        /// <summary>
        /// Incoming transfer requests dialog buttons
        /// </summary>
        public enum Buttons
        {
            /// <summary>
            /// Search transfer requests
            /// </summary>
            Search,
            /// <summary>
            /// Clear transfer request search
            /// </summary>
            ClearSearch,
            /// <summary>
            /// Create new transfer request
            /// </summary>
            Receive,
            /// <summary>
            /// Cancel search
            /// </summary>
            Cancel
        }

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        private IPosTransaction currentTransaction;
        private OperationInfo operationInfo;
        private InventoryTransferFilterExtended searchCriteria;
        private List<InventoryTransferOrder> transferOrderList;
        private InventoryTransferOrder selectedOrder;

        /// <summary>
        /// Initialize window
        /// </summary>
        /// <param name="entry"></param>
        public ReceiveTransferOrderDialog(IConnectionManager entry, IPosTransaction transaction, OperationInfo operationInfo)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            currentTransaction = transaction;
            this.operationInfo = operationInfo;
            searchCriteria = new InventoryTransferFilterExtended();
            searchCriteria.TransferFilterType = InventoryTransferType.Incoming;

            panel.ButtonHeight = 50;
            banner.BannerText = Resources.ReceiveTransferOrder;
        }

        private void Search()
        {
            SearchInventoryTransfer search = new SearchInventoryTransfer(dlgEntry, StoreTransferTypeEnum.Order, searchCriteria);
            if (search.ShowDialog() == DialogResult.OK)
            {
                searchCriteria = search.SearchCriteria;
                searchCriteria.ReceivingStoreID = dlgEntry.CurrentStoreID;
                searchCriteria.TransferFilterType = InventoryTransferType.Incoming;
                panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), true);

                transferOrderList = GetInventoryTransferOrders(searchCriteria);
                PopulateListView(transferOrderList);
            }
        }

        private void panel_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((int)args.Tag)
            {
                case (int)Buttons.Search: { Search(); break; }
                case (int)Buttons.ClearSearch:
                    {
                        searchCriteria = new InventoryTransferFilterExtended();
                        LvTransferOrders_Load(null, null);
                        panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), false);
                        break;
                    }
                case (int)Buttons.Receive: { ReceiveTransferOrder(); break; }
                case (int)Buttons.Cancel: { Close(); break; }
            }
        }

        private void ReceiveTransferOrder()
        {
            if (selectedOrder != null)
            {
                using (StoreTransferDialog dlg = new StoreTransferDialog(dlgEntry, new StoreTransferWrapper(selectedOrder, InventoryTransferType.Incoming), currentTransaction, operationInfo))
                {
                    dlg.ShowDialog();
                    LvTransferOrders_Load(this, EventArgs.Empty);
                }   
            }
        }

        private void PopulateListView(List<InventoryTransferOrder> orders)
        {
            lvTransferOrders.ClearRows();
            Row row;
            foreach (InventoryTransferOrder order in orders)
            {
                row = new Row();
                row.AddText(order.ID.StringValue);
                row.AddText(order.Text);
                row.AddText(order.SendingStoreName);
                row.AddText(order.CreationDate.ToShortDateString());
                row.AddText(order.ExpectedDelivery.ToShortDateString());

                row.Tag = order.ID;
                lvTransferOrders.AddRow(row);
            }
            lvTransferOrders.AutoSizeColumns(true);
        }

        private void PopulateInfoBox()
        {
            lblTransferID.Text = selectedOrder.ID.StringValue;
            lblDescription.Text = selectedOrder.Text;
            lblFromStore.Text = selectedOrder.SendingStoreName;
            lblSentDate.Text = selectedOrder.CreationDate.ToShortDateString();
            lblDueDate.Text = selectedOrder.ExpectedDelivery.ToShortDateString();
        }

        private void PopulateItemListView()
        {
            lvItemsList.ClearRows();
            List<InventoryTransferOrderLine> orderLines = new List<InventoryTransferOrderLine>();

            try
            {
                Exception ex = null;
                Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => orderLines = Interfaces.Services.SiteServiceService(dlgEntry).GetOrderLinesForInventoryTransfer(dlgEntry, dlgSettings.SiteServiceProfile, selectedOrder.ID, InventoryTransferOrderLineSortEnum.ItemName, false, false, true), "", Resources.ThisMayTakeAMoment, out ex);

                if (ex != null)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }
            }
            catch(Exception e)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }

            DecimalLimit quantityLimit;
            Row row;
            foreach (InventoryTransferOrderLine orderLine in orderLines)
            {
                quantityLimit = Providers.UnitData.GetNumberLimitForUnit(dlgEntry, Providers.UnitData.GetIdFromDescription(dlgEntry, orderLine.UnitName), CacheType.CacheTypeApplicationLifeTime);

                row = new Row();
                row.AddText(orderLine.ItemName);
                row.AddText(orderLine.QuantitySent.FormatWithLimits(quantityLimit));
                row.AddText(orderLine.UnitName);
                row.Tag = orderLine.ID;
                lvItemsList.AddRow(row);
            }
        }

        private void ClearOrderInfo()
        {
            lvItemsList.ClearRows();
            lblTransferID.Text = string.Empty;
            lblDescription.Text = string.Empty;
            lblFromStore.Text = string.Empty;
            lblSentDate.Text = string.Empty;
            lblDueDate.Text = string.Empty;
        }

        private List<InventoryTransferOrder> GetInventoryTransferOrders(InventoryTransferFilterExtended filter)
        {
            List<InventoryTransferOrder> orders = new List<InventoryTransferOrder>();

            try
            {
                Exception ex = null;
                Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => orders = Interfaces.Services.SiteServiceService(dlgEntry).SearchInventoryTransferOrdersExtended(dlgEntry, dlgSettings.SiteServiceProfile, filter, true), "", Resources.ThisMayTakeAMoment, out ex);

                if (ex != null)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }
            }
            catch(Exception e)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }

            return orders;
        }

        private void LvTransferOrders_Load(object sender, EventArgs e)
        {
            InventoryTransferFilterExtended filter = new InventoryTransferFilterExtended
            {
                TransferFilterType = InventoryTransferType.Incoming,
                ReceivingStoreID = dlgEntry.CurrentStoreID
            };

            transferOrderList = GetInventoryTransferOrders(filter);
            PopulateListView(transferOrderList);
            ClearOrderInfo();
        }

        private void SelectOrder(InventoryTransferOrder transferOrder)
        {
            selectedOrder = transferOrder;
            PopulateInfoBox();
            PopulateItemListView();
        }

        private void ReceiveTransferOrderDialog_Load(object sender, EventArgs e)
        {
            panel.AddButton(Resources.Search, Buttons.Search, Conversion.ToStr((int)Buttons.Search));
            panel.AddButton(Resources.ClearSearch, Buttons.ClearSearch, Conversion.ToStr((int)Buttons.ClearSearch));
            panel.AddButton(Resources.Open, Buttons.Receive, Conversion.ToStr((int)Buttons.Receive), TouchButtonType.OK, DockEnum.DockEnd);
            panel.AddButton(Resources.Close, Buttons.Cancel, Conversion.ToStr((int)Buttons.Cancel), TouchButtonType.Cancel, DockEnum.DockEnd);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), false);

            //Set the size of the form the same as the main form
            this.Width = dlgSettings.MainFormInfo.MainWindowWidth;
            this.Height = dlgSettings.MainFormInfo.MainWindowHeight;
            this.Top = dlgSettings.MainFormInfo.MainWindowTop;
            this.Left = dlgSettings.MainFormInfo.MainWindowLeft;

            //Make sure the item list view scales with screen to 30% and order list view fills the rest
            int totalWidthItemsAndRequests = Width - panel.Width - 30; // Margins: 10 + 5 + 5 + 10 = 30 px
            lvItemsList.Width = pnlTransferInfo.Width = 3 * (totalWidthItemsAndRequests / 10);
            upDownButton.Width = lvItemsList.Width - 1;
            lvTransferOrders.Width = totalWidthItemsAndRequests - lvItemsList.Width;
            lvTransferOrders.Location = new Point(lvItemsList.Width + 15, lvTransferOrders.Location.Y);

            lvTransferOrders.AutoSizeColumns(true);
            lvItemsList.AutoSizeColumns(true);

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Receive), lvTransferOrders.Selection.Count > 0);
        }

        private void LvTransferOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (transferOrderList.Count > 0 && lvTransferOrders.Selection.FirstSelectedRow >= 0)
            {
                SelectOrder(transferOrderList[lvTransferOrders.Selection.FirstSelectedRow]);
            }
            else
            {
                selectedOrder = null;
                ClearOrderInfo();
            }

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Receive), selectedOrder != null);
        }

        private void lvTransferOrders_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            ReceiveTransferOrder();
        }

        private void pnlTransferInfo_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, pnlTransferInfo.Width - 2, pnlTransferInfo.Height - 1);
            p.Dispose();
        }

        private void upDownButton_DownButtonClick(object sender, MouseEventArgs e)
        {
            lvItemsList.MoveSelectionDown();
        }

        private void upDownButton_UpButtonClick(object sender, MouseEventArgs e)
        {
            lvItemsList.MoveSelectionUp();
        }
    }
}
