using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
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
    public partial class OutgoingTransferOrdersDialog : TouchBaseForm
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

        private List<InventoryTransferOrder> transferOrderList;
        private InventoryTransferOrder selectedOrder;
        private InventoryTransferFilterExtended searchCriteria;
        private DecimalLimit quantityLimiter;

        /// <summary>
        /// Initialize window
        /// </summary>
        /// <param name="entry"></param>
        public OutgoingTransferOrdersDialog(IConnectionManager entry)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            searchCriteria = new InventoryTransferFilterExtended();
            searchCriteria.TransferFilterType = InventoryTransferType.Outgoing;

            lvTransferOrders.AutoSizeColumns();
            lvItemsList.AutoSizeColumns(true);

            quantityLimiter = dlgEntry.GetDecimalSetting(DecimalSettingEnum.Quantity);

            InitializeLabels();
        }

        private void InitializeLabels()
        {
            // Banner
            banner.BannerText = Resources.TransferOrdersToSend;

            // Order info box
            lblTransferOrderID.Text = Resources.TransferOrder + ":";
            lblDescription.Text = Resources.Description + ":";
            lblSendingTo.Text = Resources.SendingTo + ":";
            lblExpectedDelivery.Text = Resources.ExpectedDelivery + ":";

            // Order list view header
            colID.HeaderText = Resources.ID;
            colItem.HeaderText = Resources.Item;
            colSendingTo.HeaderText = Resources.ReceivingFrom;
            colCreatedDate.HeaderText = Resources.CreatedDate;

            // Order item list view header
            colItem.HeaderText = Resources.Item;
            colQuantity.HeaderText = Resources.Qty;
            colUnit.HeaderText = Resources.Unit;
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
                            Resources.SendTransferOrder, Resources.FewMinutesMessage, out exception);

                        if (result == DeleteTransferResult.Success)
                        {
                            lvTransferOrders.Selection.Clear();
                            lvTransferOrders.RemoveRow(lvTransferOrders.Selection.FirstSelectedRow);
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
                searchCriteria.Status = TransferOrderStatusEnum.New;

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
                    Resources.SendTransferOrderQuestion, Resources.SendTransferOrder, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Exception exception = null;
                    SendTransferOrderResult result = SendTransferOrderResult.Success;
                    try
                    {
                        IInventoryService inventoryService = (IInventoryService)dlgEntry.Service(ServiceType.InventoryService);
                        Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => result = inventoryService.SendTransferOrder(dlgEntry, selectedOrder.ID, dlgSettings.SiteServiceProfile), 
                            Resources.SendTransferOrder, Resources.FewMinutesMessage, out exception);
                        
                        if(exception != null)
                        {
                            Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CouldNotConnectToSiteService, Resources.ErrorSendingTransferOrder, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
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
                    catch
                    {
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CouldNotConnectToSiteService, Resources.ErrorSendingTransferOrder, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                }
            }
        }

        private void panel_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((int)args.Tag)
            {
                case (int)Buttons.Search: { Search(); break; }
                case (int)Buttons.ClearSearch: { LvTransferOrders_Load(null, null); break; }
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
                row.AddText(order.CreationDate.ToShortDateString());

                row.Tag = order.ID;
                lvTransferOrders.AddRow(row);
            }
            lvTransferOrders.AutoSizeColumns();
        }

        private void PopulateInfoBox()
        {
            lblTransferOrderIDValue.Text = selectedOrder.ID.StringValue;
            lblDescriptionValue.Text = selectedOrder.Text;
            lblSendingToValue.Text = selectedOrder.ReceivingStoreName;
            lblExpectedDeliveryValue.Text = selectedOrder.ExpectedDelivery.ToShortDateString();
        }

        private void PopulateItemListView()
        {
            lvItemsList.ClearRows();

            List<InventoryTransferOrderLine> orderLines = new List<InventoryTransferOrderLine>();

            try
            {
                Exception ex = null;
                Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => orderLines = Interfaces.Services.SiteServiceService(dlgEntry).GetOrderLinesForInventoryTransfer(dlgEntry, dlgSettings.SiteServiceProfile, selectedOrder.ID, InventoryTransferOrderLineSortEnum.ItemName, false, false, true), "", Resources.FewMinutesMessage, out ex);

                if (ex != null)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CouldNotConnectToSiteService, Resources.ErrorSendingTransferOrder, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }
            }
            catch
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CouldNotConnectToSiteService, Resources.ErrorSendingTransferOrder, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }

            Row row;
            foreach (InventoryTransferOrderLine orderLine in orderLines)
            {
                row = new Row();
                row.AddText(orderLine.ItemName);
                row.AddText(orderLine.QuantitySent.FormatWithLimits(quantityLimiter));
                row.AddText(orderLine.UnitName);
                row.Tag = orderLine.ID;
                lvItemsList.AddRow(row);
            }
        }

        private void ClearOrderInfo()
        {
            lvItemsList.ClearRows();
            lblTransferOrderIDValue.Text = string.Empty;
            lblDescriptionValue.Text = string.Empty;
            lblSendingToValue.Text = string.Empty;
            lblExpectedDeliveryValue.Text = string.Empty;
        }

        private List<InventoryTransferOrder> GetInventoryTransferOrders(InventoryTransferFilterExtended filter)
        {
            List<InventoryTransferOrder> orders = new List<InventoryTransferOrder>();

            try
            {
                Exception ex = null;
                Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => orders = Interfaces.Services.SiteServiceService(dlgEntry).SearchInventoryTransferOrdersExtended(dlgEntry, dlgSettings.SiteServiceProfile, filter, true), "", Resources.FewMinutesMessage, out ex);

                if(ex != null)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CouldNotConnectToSiteService, Resources.ErrorSendingTransferOrder, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }
            }
            catch
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CouldNotConnectToSiteService, Resources.ErrorSendingTransferOrder, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }

            return orders;
        }

        private void LvTransferOrders_Load(object sender, EventArgs e)
        {
            InventoryTransferFilterExtended filter = new InventoryTransferFilterExtended
            {
                TransferFilterType = InventoryTransferType.Outgoing,
                SendingStoreID = dlgEntry.CurrentStoreID,
                Status = TransferOrderStatusEnum.New
            };

            transferOrderList = GetInventoryTransferOrders(filter);
            PopulateListView(transferOrderList);
        }

        private void SelectOrder(InventoryTransferOrder transferOrder)
        {
            selectedOrder = transferOrder;
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Edit), true);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Delete), true);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Send), true);
            PopulateInfoBox();
            PopulateItemListView();
        }

        private void OutgoingTransferOrdersDialog_Load(object sender, EventArgs e)
        {
            panel.AddButton(Resources.Search, Buttons.Search, Conversion.ToStr((int)Buttons.Search));
            panel.AddButton(Resources.ClearSearch, Buttons.ClearSearch, Conversion.ToStr((int)Buttons.ClearSearch));
            panel.AddButton("", Buttons.Add, Conversion.ToStr((int)Buttons.Add), DockEnum.DockEnd, Resources.plus_32);
            panel.AddButton("", Buttons.Edit, Conversion.ToStr((int)Buttons.Edit), DockEnum.DockEnd, Resources.edit_32);
            panel.AddButton("", Buttons.Delete, Conversion.ToStr((int)Buttons.Delete), DockEnum.DockEnd, Resources.trash_can_32);
            panel.AddButton(Resources.Send, Buttons.Send, Conversion.ToStr((int)Buttons.Send), DockEnum.DockEnd, Color.White, ColorPalette.BlueLight, ColorPalette.BlueLight);
            panel.AddButton(Resources.Cancel, Buttons.Cancel, Conversion.ToStr((int)Buttons.Cancel), DockEnum.DockEnd);

            //Set the size of the form the same as the main form
            this.Width = dlgSettings.MainFormInfo.MainWindowWidth;
            this.Height = dlgSettings.MainFormInfo.MainWindowHeight;
            this.Top = dlgSettings.MainFormInfo.MainWindowTop;
            this.Left = dlgSettings.MainFormInfo.MainWindowLeft;

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Edit), lvTransferOrders.Selection.Count > 0);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Delete), lvTransferOrders.Selection.Count > 0);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Send), lvTransferOrders.Selection.Count > 0);
        }

        private void LvTransferOrders_RowClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (transferOrderList.Count > 0 && lvTransferOrders.Selection.FirstSelectedRow >= 0)
            {
                SelectOrder(transferOrderList[lvTransferOrders.Selection.FirstSelectedRow]);
            }
        }

        private void LvTransferOrders_SelectionChanged(object sender, EventArgs e)
        {
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Edit), lvTransferOrders.Selection.Count > 0);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Delete), lvTransferOrders.Selection.Count > 0);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Send), lvTransferOrders.Selection.Count > 0);
            if (lvTransferOrders.Selection.Count == 0)
            {
                ClearOrderInfo();
            }
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
            using (StoreTransferDialog dlg = new StoreTransferDialog(dlgEntry, new StoreTransferWrapper(StoreTransferTypeEnum.Order, InventoryTransferType.Outgoing)))
            {
                dlg.ShowDialog();
                LvTransferOrders_Load(this, EventArgs.Empty);
            }
        }

        private void EditTransferOrder()
        {
            if(selectedOrder != null)
            {
                using (StoreTransferDialog dlg = new StoreTransferDialog(dlgEntry, new StoreTransferWrapper(selectedOrder, InventoryTransferType.Outgoing)))
                {
                    dlg.ShowDialog();
                    LvTransferOrders_Load(this, EventArgs.Empty);
                }
            }
        }
    }
}
