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
    /// See list of outgoing transfer orders.
    /// </summary>
    public partial class SendTransferOrderDialog : TouchBaseForm
    {
        /// <summary>
        /// Outgoing transfer orders dialog buttons
        /// </summary>
        public enum Buttons
        {
            /// <summary>
            /// Search transfer orders
            /// </summary>
            Search,
            /// <summary>
            /// Clear transfer order search
            /// </summary>
            ClearSearch,
            /// <summary>
            /// Create new transfer order
            /// </summary>
            Add,
            /// <summary>
            /// Edit transfer order
            /// </summary>
            Edit,
            /// <summary>
            /// Delete transfer order
            /// </summary>
            Delete,
            /// <summary>
            /// Send transfer order
            /// </summary>
            Send,
            /// <summary>
            /// Cancel search
            /// </summary>
            Cancel
        }

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        private IPosTransaction currentTransaction;
        private OperationInfo operationInfo;
        private List<InventoryTransferOrder> transferOrderList;
        private InventoryTransferOrder selectedOrder;
        private InventoryTransferFilterExtended searchCriteria;

        /// <summary>
        /// Initialize window
        /// </summary>
        /// <param name="entry"></param>
        public SendTransferOrderDialog(IConnectionManager entry, IPosTransaction transaction, OperationInfo operationInfo)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            currentTransaction = transaction;
            this.operationInfo = operationInfo;
            searchCriteria = new InventoryTransferFilterExtended();
            searchCriteria.TransferFilterType = InventoryTransferType.Outgoing;

            panel.ButtonHeight = 50;
            banner.BannerText = Resources.SendTransferOrder;
        }

        private void Delete()
        {
            if (selectedOrder != null)
            {
                // Ask user if they are sure they want to delete
                if (Interfaces.Services.DialogService(dlgEntry).ShowMessage(
                    Resources.DeleteOrderQuestion, Resources.DeleteOrder, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Exception exception = new Exception();
                    DeleteTransferResult result = DeleteTransferResult.Success;
                    try
                    {
                        IInventoryService inventoryService = (IInventoryService)dlgEntry.Service(ServiceType.InventoryService);
                        Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => result = inventoryService.DeleteTransferOrder(dlgEntry, selectedOrder.ID, false, dlgSettings.SiteServiceProfile),
                            Resources.SendInventoryTransferOrder, Resources.ThisMayTakeAMoment, out exception);

                        if (result == DeleteTransferResult.Success)
                        {
                            lvTransferOrders.RemoveRow(lvTransferOrders.Selection.FirstSelectedRow);
                            lvTransferOrders.Selection.Clear();
                            selectedOrder = null;
                        }
                        else
                        {
                            string errorMessage = GetDeleteOrderErrorMessage(result);
                            Interfaces.Services.DialogService(dlgEntry).ShowErrorMessage(errorMessage, exception.ToString());
                        }
                    }
                    catch
                    {
                        Interfaces.Services.DialogService(dlgEntry).ShowErrorMessage(Resources.ErrorDeletingTransferOrder, exception.ToString());
                    }
                }
            }
        }

        private void Search()
        {
            SearchInventoryTransfer search = new SearchInventoryTransfer(dlgEntry, StoreTransferTypeEnum.Order, searchCriteria);
            if (search.ShowDialog() == DialogResult.OK)
            {
                searchCriteria = search.SearchCriteria;
                searchCriteria.SendingStoreID = dlgEntry.CurrentStoreID;
                searchCriteria.TransferFilterType = InventoryTransferType.Outgoing;
                searchCriteria.Sent = false;
                searchCriteria.Status = TransferOrderStatusEnum.New;
                panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), true);

                transferOrderList = GetInventoryTransferOrders(searchCriteria);
                PopulateListView(transferOrderList);
            }
        }

        private void Send()
        {
            if (selectedOrder != null)
            {
                // Ask user if they are sure they want to send
                if (Interfaces.Services.DialogService(dlgEntry).ShowMessage(
                    Resources.SendTransferOrderQuestion, Resources.SendInventoryTransferOrder, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Exception exception = null;
                    SendTransferOrderResult result = SendTransferOrderResult.Success;
                    try
                    {
                        IInventoryService inventoryService = (IInventoryService)dlgEntry.Service(ServiceType.InventoryService);
                        Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => result = inventoryService.SendTransferOrder(dlgEntry, selectedOrder.ID, dlgSettings.SiteServiceProfile), 
                            Resources.SendInventoryTransferOrder, Resources.ThisMayTakeAMoment, out exception);
                        
                        if(exception != null)
                        {
                            Interfaces.Services.DialogService(dlgEntry).ShowMessage(exception is ClientTimeNotSynchronizedException ? exception.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                            return;
                        }

                        if (result == SendTransferOrderResult.Success)
                        {
                            lvTransferOrders.Selection.Clear();
                            selectedOrder = null;
                            LvTransferOrders_Load(this, EventArgs.Empty);
                        }
                        else
                        {
                            string errorMessage = GetSendOrderErrorMessage(result);
                            Interfaces.Services.DialogService(dlgEntry).ShowMessage(errorMessage, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        }
                    }
                    catch(Exception e)
                    {
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                }
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
                case (int)Buttons.Add: { AddTransferOrder(); break; }
                case (int)Buttons.Edit: { EditTransferOrder(); break; }
                case (int)Buttons.Delete: { Delete(); break; }
                case (int)Buttons.Send: { Send(); break; }
                case (int)Buttons.Cancel: { Close(); break; }
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
                row.AddText(order.ReceivingStoreName);
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
            lblToStore.Text = selectedOrder.ReceivingStoreName;
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
            lblToStore.Text = string.Empty;
            lblDueDate.Text = string.Empty;
        }

        private List<InventoryTransferOrder> GetInventoryTransferOrders(InventoryTransferFilterExtended filter)
        {
            List<InventoryTransferOrder> orders = new List<InventoryTransferOrder>();

            try
            {
                Exception ex = null;
                Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => orders = Interfaces.Services.SiteServiceService(dlgEntry).SearchInventoryTransferOrdersExtended(dlgEntry, dlgSettings.SiteServiceProfile, filter, true), "", Resources.ThisMayTakeAMoment, out ex);

                if(ex != null)
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
                TransferFilterType = InventoryTransferType.Outgoing,
                SendingStoreID = dlgEntry.CurrentStoreID,
                Sent = false,
                Status = TransferOrderStatusEnum.New
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

        private void SendTransferOrderDialog_Load(object sender, EventArgs e)
        {
            panel.AddButton(Resources.Search, Buttons.Search, Conversion.ToStr((int)Buttons.Search));
            panel.AddButton(Resources.ClearSearch, Buttons.ClearSearch, Conversion.ToStr((int)Buttons.ClearSearch));
            panel.AddButton(Resources.Add, Buttons.Add, Conversion.ToStr((int)Buttons.Add), TouchButtonType.Normal, DockEnum.DockNone, Resources.Plusincircle_16px, ImageAlignment.Left);
            panel.AddButton(Resources.Edit, Buttons.Edit, Conversion.ToStr((int)Buttons.Edit), TouchButtonType.Normal, DockEnum.DockNone, Resources.Edit_16px, ImageAlignment.Left);
            panel.AddButton(Resources.Delete, Buttons.Delete, Conversion.ToStr((int)Buttons.Delete), TouchButtonType.Normal, DockEnum.DockNone, Resources.Clear_16px, ImageAlignment.Left);
            panel.AddButton(Resources.Send, Buttons.Send, Conversion.ToStr((int)Buttons.Send), TouchButtonType.OK, DockEnum.DockEnd);
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

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Edit), lvTransferOrders.Selection.Count > 0);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Delete), lvTransferOrders.Selection.Count > 0);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Send), lvTransferOrders.Selection.Count > 0);
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

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Edit), selectedOrder != null);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Delete), selectedOrder != null);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Send), selectedOrder != null);
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            lvItemsList.MoveSelectionUp();
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            lvItemsList.MoveSelectionDown();
        }

        private string GetDeleteOrderErrorMessage(DeleteTransferResult result)
        {
            switch (result)
            {
                case DeleteTransferResult.NotFound:
                    return Resources.UnableToDeleteTransferOrder + " " + Resources.TransferOrderNotFound;
                case DeleteTransferResult.Sent:
                    return Resources.UnableToDeleteTransferOrder + " " + Resources.TransferOrderAlreadySent;
                case DeleteTransferResult.FetchedByReceivingStore:
                    return Resources.UnableToDeleteTransferOrder + " " + Resources.TransferOrderHasBeenFetchedCantBeDeleted;
                case DeleteTransferResult.Received:
                    return Resources.UnableToDeleteTransferOrder + " " + Resources.ReceivedTransferOrderCantBeDeleted;
                case DeleteTransferResult.ErrorDeletingTransfer:
                    return Resources.UnableToDeleteTransferOrder + " " + Resources.ErrorDeletingTransferOrder;
                default:
                    return Resources.UnableToDeleteTransferOrder;
            }
        }

        private string GetSendOrderErrorMessage(SendTransferOrderResult result)
        {
            switch(result)
            {
                case SendTransferOrderResult.ErrorSendingTransferOrder:
                    return Resources.UnableToSendTransferOrder;
                case SendTransferOrderResult.FetchedByReceivingStore:
                    return Resources.UnableToSendTransferOrder + " " + Resources.TransferOrderAlreadySentOrContainsNoItems;
                case SendTransferOrderResult.NotFound:
                    return Resources.TransferOrderNotFound;
                case SendTransferOrderResult.TransferAlreadySent:
                    return Resources.UnableToSendTransferOrder + " " + Resources.TransferOrderAlreadySent;
                case SendTransferOrderResult.NoItemsOnTransfer:
                    return Resources.UnableToSendTransferOrder + " " + Resources.TransferOrderHasNoLines;
                case SendTransferOrderResult.UnitConversionError:
                    return Resources.UnableToSendTransferOrder + " " + Resources.MissingUnitConversion;
                case SendTransferOrderResult.TransferOrderIsRejected:
                    return Resources.UnableToSendTransferOrder + " " + Resources.RejectedTransferOrderCantBeSent;
                default:
                    return Resources.UnableToSendTransferOrder;
            }
        }

        private void AddTransferOrder()
        {
            using (StoreTransferDialog dlg = new StoreTransferDialog(dlgEntry, new StoreTransferWrapper(StoreTransferTypeEnum.Order, InventoryTransferType.Outgoing), currentTransaction, operationInfo))
            {
                dlg.ShowDialog();
                LvTransferOrders_Load(this, EventArgs.Empty);
            }
        }

        private void EditTransferOrder()
        {
            if(selectedOrder != null)
            {
                using (StoreTransferDialog dlg = new StoreTransferDialog(dlgEntry, new StoreTransferWrapper(selectedOrder, InventoryTransferType.Outgoing), currentTransaction, operationInfo))
                {
                    dlg.ShowDialog();
                    LvTransferOrders_Load(this, EventArgs.Empty);
                }
            }
        }

        private void lvTransferOrders_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            EditTransferOrder();
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
