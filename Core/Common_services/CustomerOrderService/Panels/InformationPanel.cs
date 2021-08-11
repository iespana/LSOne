using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.Line.CustomerOrder;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Panels
{
    /// <summary>
    /// A panel that displays information about the quote / customer order and where the items can be selected for pickup
    /// </summary>
    public partial class InformationPanel : UserControl
    {
        private IConnectionManager dlgEntry;
        private ISettings settings; 
        private IRetailTransaction retailTransaction;
        private CustomerOrderSettings orderSettings;
        private Dictionary<Guid, ISaleLineItem> grouppedItems;

        private enum Buttons
        {
            PickupAll,
            OverrideDeposit,
            AdditionalPayment,
            EditDetails,
            Finish,
            Cancel
        }

        /// <summary>
        /// The constructor that takes in the information for display
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="action">The action that has been selected by the user (if any)</param>
        public InformationPanel(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderAction action, CustomerOrderSettings orderSettings)
        {
            InitializeComponent();

            dlgEntry = entry;
            settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            this.retailTransaction = retailTransaction;
            this.orderSettings = orderSettings;

            SetStyle(ControlStyles.Selectable, true);

            DoubleBuffered = true;
            
            if(retailTransaction.CustomerOrder.OrderType == CustomerOrderType.Quote)
            {
                lblBalance.Visible = false;
                lblPayNow.Visible = false;

                //Remove last three columns
                lvOrder.Columns.RemoveAt(lvOrder.Columns.Count - 1);
                lvOrder.Columns.RemoveAt(lvOrder.Columns.Count - 1);
                lvOrder.Columns.RemoveAt(lvOrder.Columns.Count - 1);
                lvOrder.Columns[2].HeaderText = Properties.Resources.Quantity;
                lvOrder.Columns[3].HeaderText = Properties.Resources.Price;
            }

            SetCustomerOrderInfo();
            GroupItems();
            InitializeButtons();

            if (action == CustomerOrderAction.FullPickup)
            {
                PickUpAll();
            }
            else
            {
                LoadItems();
            }
        }

        /// <summary>
        /// Displays the information from the customer order / quote
        /// </summary>
        public void SetCustomerOrderInfo()
        {
            lblReference.Text = (string) retailTransaction.CustomerOrder.Reference;
            lblDelivery.Text = retailTransaction.CustomerOrder.Delivery.Text;
            lblSource.Text = retailTransaction.CustomerOrder.Source.Text;
            lblExpire.Text = retailTransaction.CustomerOrder.ExpirationDate.ToShortDateString();

            lblCustomer.Text = "-";
            lblAddress.Text = "-";

            if (retailTransaction.Customer.ID != RecordIdentifier.Empty)
            {
                lblCustomer.Text = retailTransaction.Customer.GetFormattedName(dlgEntry.Settings.NameFormatter);
                lblAddress.Text = dlgEntry.Settings.SystemAddressFormatter.FormatMultipleLines(retailTransaction.Customer.DefaultShippingAddress, dlgEntry.Cache, "\n");
            }

            UpdateTotals();
        }

        private void UpdateTotals()
        {
            IRoundingService rounding = Interfaces.Services.RoundingService(dlgEntry);
            lblTotal.Text = rounding.RoundString(dlgEntry, retailTransaction.NetAmountWithTax, settings.Store.Currency, true, CacheType.CacheTypeApplicationLifeTime);
            lblBalance.Text = rounding.RoundString(dlgEntry, retailTransaction.TransSalePmtDiff, settings.Store.Currency, true, CacheType.CacheTypeApplicationLifeTime);
            lblPayNow.Text = rounding.RoundString(dlgEntry, GetAmountToPay(), settings.Store.Currency, true, CacheType.CacheTypeApplicationLifeTime);
        }

        private decimal GetAmountToPay()
        {
            if(retailTransaction.CustomerOrder.Status == CustomerOrderStatus.New)
            {
                return retailTransaction.CustomerOrder.MinimumDeposit;
            }

            if(retailTransaction.CustomerOrder.HasAdditionalPayment)
            {
                return retailTransaction.CustomerOrder.AdditionalPayment;
            }

            return decimal.Zero;
        }

        private void GroupItems()
        {
            grouppedItems = new Dictionary<Guid, ISaleLineItem>();

            foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => !w.Voided))
            {
                if (item.Order.Ordered == decimal.Zero)
                {
                    item.Order.Ordered = item.Quantity;
                }

                if(item.Order.SplitIdentifier == Guid.Empty)
                {
                    item.Order.SplitIdentifier = Guid.NewGuid();
                }

                if(grouppedItems.ContainsKey(item.Order.SplitIdentifier))
                {
                    ISaleLineItem saleItem = grouppedItems[item.Order.SplitIdentifier];
                    saleItem.Order.Ordered += item.Order.Ordered;
                    saleItem.Order.Received += item.Order.Received;
                    saleItem.Order.ToPickUp += item.Order.ToPickUp;
                    saleItem.Order.ReservationQty += item.Order.ReservationQty;
                    saleItem.Order.FullyReceived &= item.Order.FullyReceived;
                }
                else
                {
                    ISaleLineItem itemClone = (SaleLineItem)item.Clone();
                    itemClone.Order = new OrderItem
                    {
                        Ordered = item.Order.Ordered,
                        Received = item.Order.Received,
                        ToPickUp = item.Order.ToPickUp,
                        FullyReceived = item.Order.FullyReceived,
                        ReservationQty = item.Order.ReservationQty,
                        SplitIdentifier = item.Order.SplitIdentifier
                    };

                    grouppedItems.Add(item.Order.SplitIdentifier, itemClone);
                }
            }
        }

        private void LoadItems()
        {
            lvOrder.ClearRows();

            foreach (Guid key in grouppedItems.Keys)
            {
                if(grouppedItems[key].IsAssemblyComponent)
                {
                    continue;
                }

                Row row = new Row();
                PopulateItemRow(ref row, key);
                lvOrder.AddRow(row);
            }

            lvOrder.AutoSizeColumns(true);
            UpdateButtonState();
        }

        private void ReloadItem(ref Row row)
        {
            row.Clear();
            PopulateItemRow(ref row, (Guid)row.Tag);

            lvOrder.AutoSizeColumns(true);
            UpdateButtonState();
        }

        private void PopulateItemRow(ref Row row, Guid key)
        {
            IRoundingService rounding = Interfaces.Services.RoundingService(dlgEntry);
            ISaleLineItem item = grouppedItems[key];

            row.AddText(item.ItemId);
            row.AddText(item.Description);

            if (retailTransaction.CustomerOrder.OrderType == CustomerOrderType.CustomerOrder)
            {
                row.AddText(rounding.RoundQuantity(dlgEntry, item.Order.Ordered, item.SalesOrderUnitOfMeasure, settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime));
                row.AddText(rounding.RoundQuantity(dlgEntry, item.Order.Received, item.SalesOrderUnitOfMeasure, settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime));
                row.AddCell(new PlusMinusCell(rounding.RoundQuantity(dlgEntry, item.Order.ToPickUp, item.SalesOrderUnitOfMeasure, settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime), !item.Order.FullyReceived, item.Order.ToPickUp > 0, item.Order.ToPickUp < item.Order.Ordered - item.Order.Received));
                row.AddText(rounding.RoundQuantity(dlgEntry, item.Order.Ordered - item.Order.Received - item.Order.ToPickUp, item.SalesOrderUnitOfMeasure, settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime));

                if (!item.Order.FullyReceived)
                {
                    row.AddCell(new ImageCell(Properties.Resources.Edit_24px, 0, 0, false, true));
                }
            }
            else
            {
                row.AddText(rounding.RoundQuantity(dlgEntry, item.Quantity, item.SalesOrderUnitOfMeasure, settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime));
                row.AddText(rounding.RoundString(dlgEntry, item.GetCalculatedPrice(withTax: true), settings.Store.Currency, true, CacheType.CacheTypeApplicationLifeTime));
            }

            row.Tag = key;
        }

        private void InitializeButtons()
        {
            panelButtons.Clear();

            if (retailTransaction.CustomerOrder.OrderType == CustomerOrderType.CustomerOrder && !retailTransaction.CustomerOrder.HasAdditionalPayment)
            {
                panelButtons.AddButton(Properties.Resources.PickUpAll, Buttons.PickupAll, Conversion.ToStr((int)Buttons.PickupAll), TouchButtonType.Normal, DockEnum.DockEnd);
            }

            if (dlgEntry.HasPermission(Permission.OverrideMinDeposit) && retailTransaction.CustomerOrder.Status == CustomerOrderStatus.New && orderSettings.AcceptsDeposits)
            {
                panelButtons.AddButton(Properties.Resources.OverrideDeposit, Buttons.OverrideDeposit, "", TouchButtonType.Normal, DockEnum.DockEnd);
            }

            if (retailTransaction.CustomerOrder.Status == CustomerOrderStatus.Open)
            {
                panelButtons.AddButton(Properties.Resources.AdditionalPayment, Buttons.AdditionalPayment, "", TouchButtonType.Normal, DockEnum.DockEnd);
            }

            panelButtons.AddButton(Properties.Resources.EditDetails, Buttons.EditDetails, "", TouchButtonType.Normal, DockEnum.DockEnd);
            panelButtons.AddButton(Properties.Resources.OK, Buttons.Finish, "Finish", TouchButtonType.OK, DockEnum.DockEnd);
            panelButtons.AddButton(Properties.Resources.Cancel, Buttons.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
        }
        
        private void panelButtons_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((Buttons)(int)args.Tag)
            {
                case Buttons.PickupAll:
                    PickUpAll();
                    break;
                case Buttons.OverrideDeposit:
                    GetParent().ClickOverrideDeposit();
                    break;
                case Buttons.AdditionalPayment:
                    GetParent().ClickAdditionalPayments();
                    UpdateButtonState();
                    break;
                case Buttons.EditDetails:
                    GetParent().ClickEditDetails();
                    break;
                case Buttons.Finish:
                    GetParent().ClickFinish();
                    break;
                case Buttons.Cancel:
                    GetParent().ClickCancel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private CustomerOrderDetails GetParent()
        {
            return this.Parent as CustomerOrderDetails;
        }

        private void PickUpAll()
        {
            foreach (ISaleLineItem item in grouppedItems.Values)
            {
                if(!item.Order.FullyReceived)
                {
                    item.Order.ToPickUp = item.Order.Ordered - item.Order.Received;
                }
            }

            LoadItems();
        }

        private void lvOrder_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            Row currentRow = lvOrder.Row(args.RowNumber);
            ISaleLineItem item = grouppedItems[(Guid)currentRow.Tag];

            if (item.Order.FullyReceived)
            {
                return;
            }

            if (args.Cell is ImageCell)
            {
                Unit currentUnit = Providers.UnitData.Get(dlgEntry, item.SalesOrderUnitOfMeasure, CacheType.CacheTypeApplicationLifeTime);
                PickQuantityDialog dlg = new PickQuantityDialog(item.Order.ToPickUp, item.Order.Ordered - item.Order.Received, currentUnit.MaximumDecimals);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    decimal input = dlg.Quantity;
                    if (input > item.Order.Ordered - item.Order.Received)
                    {
                        if (Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.SelectedPickupToHigh + " " + Properties.Resources.AllItemsOnOrderSetToPickUp, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
                        {
                            item.Order.ToPickUp = item.Order.Ordered - item.Order.Received;
                        }
                    }
                    else
                    {
                        item.Order.ToPickUp = input;
                    }

                    ReloadItem(ref currentRow);
                }
            }
            else if(args.Cell is PlusMinusCell)
            {
                PlusMinusCell cell = (PlusMinusCell)args.Cell;

                if(cell.PlusPressed)
                {
                    if(item.Order.ToPickUp == item.Order.Ordered - item.Order.Received)
                    {
                        return;
                    }

                    if(item.Order.ToPickUp + 1 >= item.Order.Ordered - item.Order.Received)
                    {
                        item.Order.ToPickUp = item.Order.Ordered - item.Order.Received;
                    }
                    else
                    {
                        item.Order.ToPickUp += 1;
                    }

                    ReloadItem(ref currentRow);
                }
                else if(cell.MinusPressed)
                {
                    if(item.Order.ToPickUp == 0)
                    {
                        return;
                    }

                    if(item.Order.ToPickUp - 1 <= 0)
                    {
                        item.Order.ToPickUp = 0;
                    }
                    else
                    {
                        item.Order.ToPickUp -= 1;
                    }

                    ReloadItem(ref currentRow);
                }
            }

            if(item.IsAssembly)
            {
                PickUpAssemblyComponents(item);
            }
        }

        private void PickUpAssemblyComponents(ISaleLineItem assemblyItem)
        {
            List<RetailItemAssemblyComponent> components = assemblyItem.ItemAssembly.AssemblyComponents;

            foreach (ISaleLineItem componentSaleLine in retailTransaction.ISaleItems.Where(x => x.IsAssemblyComponent && x.AssemblyParentLineID == assemblyItem.LineId && !x.Voided))
            {
                decimal componentQuantity = components.Find(x => x.ItemID == componentSaleLine.ItemId || x.ItemID == componentSaleLine.HeaderItemID)?.Quantity ?? 0;
                grouppedItems[componentSaleLine.Order.SplitIdentifier].Order.ToPickUp = componentQuantity * assemblyItem.Order.ToPickUp;
            }
        }

        internal void SplitItems()
        {
            List<ISaleLineItem> splitItemsToAdd = new List<ISaleLineItem>();
            List<ISaleLineItem> itemsToRemove = new List<ISaleLineItem>();

            foreach (IGrouping<Guid, ISaleLineItem> saleLineGroup in retailTransaction.SaleItems.Where(w => !w.Voided).GroupBy(x => x.Order.SplitIdentifier))
            {
                ISaleLineItem grouppedItem = grouppedItems[saleLineGroup.Key];

                int notReceivedItemsCounts = saleLineGroup.Count(x => !x.Order.FullyReceived);

                //If there is only 1 item in the group and it's being partially picked up, we need to split the item
                if (grouppedItem.Order.ToPickUp > 0 && notReceivedItemsCounts == 1)
                {
                    ISaleLineItem originalItem = saleLineGroup.First();
                    if(grouppedItem.Order.ToPickUp < grouppedItem.Quantity)
                    {
                        ISaleLineItem splitItem = (ISaleLineItem)originalItem.Clone();
                        splitItem.Quantity = grouppedItem.Order.ToPickUp;
                        if (splitItem.QuantityDiscounted > 0) 
                        {
                            splitItem.QuantityDiscounted = splitItem.Quantity;
                        }
                        splitItem.UnitQuantity = splitItem.Quantity * originalItem.UnitQuantityFactor;
                        splitItem.Order.Ordered = grouppedItem.Order.ToPickUp;
                        splitItem.Order.Received = decimal.Zero;
                        splitItem.Order.ToPickUp = grouppedItem.Order.ToPickUp;
                        splitItem.Order.ReservationQty = grouppedItem.Order.ToPickUp;

                        originalItem.Quantity = originalItem.Quantity - grouppedItem.Order.ToPickUp;
                        if (originalItem.QuantityDiscounted > 0)
                        {
                            originalItem.QuantityDiscounted = originalItem.Quantity;
                        }
                        originalItem.UnitQuantity = originalItem.Quantity * originalItem.UnitQuantityFactor;
                        originalItem.Order.Ordered = originalItem.Quantity;
                        originalItem.Order.ToPickUp = decimal.Zero;
                        originalItem.Order.ReservationQty = grouppedItem.Order.ReservationQty - splitItem.Order.ReservationQty;

                        //Calculate the lines again so that NetAmounts are correct for the deposit calculations
                        Interfaces.Services.CalculationService(dlgEntry).CalculateLine(dlgEntry, originalItem, retailTransaction);
                        Interfaces.Services.CalculationService(dlgEntry).CalculateLine(dlgEntry, splitItem, retailTransaction);
                        CalculateDeposits(originalItem, splitItem);

                        splitItemsToAdd.Add(splitItem);
                    }
                    else
                    {
                        //No split, full pickup
                        originalItem.Order.ToPickUp = grouppedItem.Order.ToPickUp;
                        originalItem.Order.ReservationQty = grouppedItem.Order.ToPickUp;
                    }
                }
                else if(notReceivedItemsCounts > 1)
                {
                    //If there are more items in the group that are not fully received,
                    // we need to merge the remaining quantity that is not received back into the main item
                    // and remove the split item if it's no longer being picked up

                    //Since we always merge the remaining quantity back to the original item, this list should always contain 2 items, the original and the split item
                    List<ISaleLineItem> splitItems = saleLineGroup.Where(x => !x.Order.FullyReceived).OrderBy(x => x.LineId).ToList();
                    ISaleLineItem originalItem = splitItems[0];
                    ISaleLineItem splitItem = splitItems[1];

                    //The user decided to either pickup the entire quantity for this item or not pick anything, so we delete the split item
                    if(grouppedItem.Order.ToPickUp == 0 || grouppedItem.Order.ToPickUp == grouppedItem.Order.Ordered - grouppedItem.Order.Received)
                    {
                        originalItem.Quantity = originalItem.Quantity + splitItem.Quantity;
                        originalItem.UnitQuantity = originalItem.Quantity * originalItem.UnitQuantityFactor;
                        originalItem.Order.Ordered = originalItem.Quantity;
                        originalItem.Order.ToPickUp = grouppedItem.Order.ToPickUp;
                        originalItem.Order.ReservationQty = grouppedItem.Order.ToPickUp;

                        itemsToRemove.Add(splitItem);
                        Interfaces.Services.CalculationService(dlgEntry).CalculateLine(dlgEntry, originalItem, retailTransaction);

                        bool depositPaid = originalItem.Order.DepositAlreadyPaid() != 0;

                        originalItem.Order.Deposits.Clear();
                        CalculationHelper.CalculateDeposit(dlgEntry, originalItem, retailTransaction.CustomerOrder.OrderType, retailTransaction);

                        if(depositPaid)
                        {
                            originalItem.Order.SetAllDepositsAsPaid();
                        } 
                    }
                    else if(grouppedItem.Order.ToPickUp != splitItem.Order.Ordered) //Adjust quantities
                    {
                        originalItem.Quantity = originalItem.Quantity - (grouppedItem.Order.ToPickUp - splitItem.Order.Ordered);
                        originalItem.UnitQuantity = originalItem.Quantity * originalItem.UnitQuantityFactor;

                        splitItem.Quantity = grouppedItem.Order.ToPickUp;
                        splitItem.UnitQuantity = splitItem.Quantity * originalItem.UnitQuantityFactor;
                        splitItem.Order.Ordered = grouppedItem.Order.ToPickUp;
                        splitItem.Order.Received = decimal.Zero;
                        splitItem.Order.ToPickUp = grouppedItem.Order.ToPickUp;
                        splitItem.Order.ReservationQty = grouppedItem.Order.ToPickUp;

                        originalItem.Order.Ordered = originalItem.Quantity;
                        originalItem.Order.ToPickUp = decimal.Zero;
                        originalItem.Order.ReservationQty = grouppedItem.Order.ReservationQty - splitItem.Order.ReservationQty;

                        Interfaces.Services.CalculationService(dlgEntry).CalculateLine(dlgEntry, originalItem, retailTransaction);
                        Interfaces.Services.CalculationService(dlgEntry).CalculateLine(dlgEntry, splitItem, retailTransaction);
                        CalculateDeposits(originalItem, splitItem);
                    }
                }
            }

            foreach (ISaleLineItem item in splitItemsToAdd)
            {
                retailTransaction.Add(item);
            }

            foreach(ISaleLineItem item in itemsToRemove)
            {
                retailTransaction.SaleItems.Remove(item);
            }

            if (splitItemsToAdd.Count > 0 || itemsToRemove.Count > 0)
            {
                Interfaces.Services.CalculationService(dlgEntry).CalculateTotals(dlgEntry, retailTransaction);
            }
        }

        private void CalculateDeposits(ISaleLineItem originalItem, ISaleLineItem splitItem)
        {
            //If a deposit has already been paid then it needs to be split up between the items and any difference attached to the item not being picked up
            if (originalItem.Order.DepositAlreadyPaid() != 0)
            {
                IOrderItem orgOrderItem = (IOrderItem)originalItem.Order.Clone();

                originalItem.Order.Deposits.Clear();
                splitItem.Order.Deposits.Clear();

                CalculationHelper.CalculateDeposit(dlgEntry, originalItem, retailTransaction.CustomerOrder.OrderType, retailTransaction);
                CalculationHelper.CalculateDeposit(dlgEntry, splitItem, retailTransaction.CustomerOrder.OrderType, retailTransaction);

                if ((originalItem.Order.TotalDepositAmount() + splitItem.Order.TotalDepositAmount()) != orgOrderItem.TotalDepositAmount())
                {
                    decimal diff = orgOrderItem.TotalDepositAmount() - (originalItem.Order.TotalDepositAmount() + splitItem.Order.TotalDepositAmount());
                    IDepositItem deposit = originalItem.Order.Deposits.FirstOrDefault();
                    if (deposit != null)
                    {
                        deposit.Deposit += diff;
                    }
                }

                originalItem.Order.SetAllDepositsAsPaid();
                splitItem.Order.SetAllDepositsAsPaid();
            }
            //If nothing has been paid then clear the deposit fields and calculated them again
            else
            {
                originalItem.Order.Deposits.Clear();
                splitItem.Order.Deposits.Clear();

                CalculationHelper.CalculateDeposit(dlgEntry, originalItem, retailTransaction.CustomerOrder.OrderType, retailTransaction);
                CalculationHelper.CalculateDeposit(dlgEntry, splitItem, retailTransaction.CustomerOrder.OrderType, retailTransaction);
            }
        }

        private void UpdateButtonState()
        {
            bool finshEnabled = retailTransaction.CustomerOrder.Status == CustomerOrderStatus.New
                || retailTransaction.CustomerOrder.HasAdditionalPayment
                || (grouppedItems?.Values.Any(x => x.Order.ToPickUp > 0) ?? false);

            bool pickupAllEnabled = grouppedItems?.Values.Any(x => x.Order.ToPickUp < x.Order.Ordered - x.Order.Received) ?? false;

            panelButtons.SetButtonEnabled("Finish", finshEnabled);
            panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.PickupAll), pickupAllEnabled);
        }
    }
}