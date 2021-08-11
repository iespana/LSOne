using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.POS.Processes.WinControls;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using Customer = LSOne.DataLayer.BusinessObjects.Customers.Customer;

namespace LSOne.Services.Panels
{
    /// <summary>
    /// The panel that displays the list of customer ordres / quotes and the information on the transaction
    /// </summary>
    public partial class SearchPanel : UserControl
    {
        private IConnectionManager dlgEntry;
        private bool multiSelectionActive;

        private List<CustomerOrderAdditionalConfigurations> configurations;
        private List<DataEntity> stores;
        private List<CustomerOrder> currentCustomerOrderList;
        private Receipt receipt;

        /// <summary>
        /// The customer order that was selected
        /// </summary>
        public CustomerOrder SelectedOrder;
        /// <summary>
        /// The transaction that was selected
        /// </summary>
        public RetailTransaction SelectedTransaction;
        /// <summary>
        /// the current search criteria being used
        /// </summary>
        public CustomerOrderSearch SearchCritiera;
        /// <summary>
        /// The type of customer order being displayed
        /// </summary>
        public CustomerOrderType orderType;
        private bool selectAll;

        /// <summary>
        /// If set to true selects all the customer orders in the list if false unselects them
        /// </summary>
        public bool SelectAll
        {
            get { return selectAll; }

            set
            {
                selectAll = value;
                foreach (CustomerOrder order in currentCustomerOrderList)
                {
                    order.Selected = SelectAll;
                }
                SetSelectedCheckMarks();
            }
        }

        /// <summary>
        /// Sets and gets if the list allows multi selection.
        /// </summary>
        public bool MultiSelectionActive
        {
            get { return multiSelectionActive; }
            set
            {
                multiSelectionActive = value;
                int selectedRow = -1;

                if (!multiSelectionActive)
                {
                    selectedRow = lvCustomerOrders.Selection.FirstSelectedRow;
                }
                LoadOrdersIntoList();

                if (!multiSelectionActive && selectedRow > -1)
                {
                    lvCustomerOrders.Selection.Clear();
                    lvCustomerOrders.Selection.AddRows(selectedRow, selectedRow);
                    lvCustomerOrders_SelectionChanged(null, EventArgs.Empty);
                }
            }
        }
        

        /// <summary>
        /// Constructor that initializes the panel for the customer order type that is being displayed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="orderType">The type of customer order</param>
        public SearchPanel(IConnectionManager entry, CustomerOrderType orderType)
        {
            InitializeComponent();

            dlgEntry = entry;

            configurations = Providers.CustomerOrderAdditionalConfigData.GetList(dlgEntry);
            stores = Providers.StoreData.GetList(dlgEntry);

            this.orderType = orderType;
            
            SearchCritiera = new CustomerOrderSearch();
            currentCustomerOrderList = new List<CustomerOrder>();

            multiSelectionActive = false;
            selectAll = false;

            lblCustomer.Text = "";
            lblAddress.Text = "";
            lblComment.Text = "";
            lblNoCustomerInfo.ForeColor = ColorPalette.POSErrorDialogLineColor;
            lvCustomerOrders.HorizontalScrollbar = true;

            if (!DesignMode)
            {
                receipt = new ReceiptControlFactory().CreateReceiptControl(pnlReceipt);
            }

            DisplayNoCustomerInfo(false);
            
            SetStyle(ControlStyles.Selectable, true);

            DoubleBuffered = true;
            
            LoadOrders(SearchCritiera);
        }

        /// <summary>
        /// Sets a specific status on all customer orders / quotes that are selected
        /// </summary>
        /// <param name="status">the status to be set</param>
        public void SetStatusOnSelectedOrders(CustomerOrderStatus status)
        {
            List<CustomerOrder> selectedList = GetSelectedOrders();
            if (selectedList.Count == 0)
            {
                return;
            }

            if (status == CustomerOrderStatus.Delivered)
            {
                if (Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.AllItemsWillBeMarkedAsFullyReceived + " " + Resources.DoYouWantToContinue, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                bool displayBalanceMsg = false;
                Interfaces.Services.DialogService(dlgEntry).UpdateStatusDialog(Resources.UpdatingCustomerOrders);

                foreach (CustomerOrder order in selectedList)
                {
                    order.Status = status;
                    if (status == CustomerOrderStatus.Delivered)
                    {
                        RetailTransaction retailTransaction = GetTransactionFromOrder(order, false);

                        if (!displayBalanceMsg && retailTransaction.TransSalePmtDiff != decimal.Zero)
                        {
                            displayBalanceMsg = true;
                        }

                        if (retailTransaction.TransSalePmtDiff == decimal.Zero && retailTransaction.SaleItems.Count(c => !c.Voided && c.Order.ToPickUp > 0) > 0)
                        {
                            foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => !w.Voided))
                            {
                                item.Order.Received = item.Order.Ordered;
                                item.Order.FullyReceived = true;
                                item.Order.ToPickUp = decimal.Zero;
                            }

                            Interfaces.Services.CustomerOrderService(dlgEntry).SaveCustomerOrder(dlgEntry, retailTransaction, CustomerOrderStatus.Delivered);
                        }
                    }
                    else
                    {
                        Interfaces.Services.CustomerOrderService(dlgEntry).SaveCustomerOrderDetails(dlgEntry, order);
                    }
                }

                if (displayBalanceMsg)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.SomeOrdersHaveBalanceDueStatusNotUpdated);
                }
                LoadOrders(SearchCritiera);
            }
            finally
            {
                Interfaces.Services.DialogService(dlgEntry).CloseStatusDialog();
            }
        }

        /// <summary>
        /// Prints out a picking list for all the selected customer orders
        /// </summary>
        public void PrintPickingList()
        {
            List<CustomerOrder> selectedList = GetSelectedOrders();
            if (selectedList.Count == 0)
            {
                return;
            }

            try
            {
                bool displayBalanceMsg = false;
                Interfaces.Services.DialogService(dlgEntry).UpdateStatusDialog(Resources.UpdatingCustomerOrders);

                foreach (CustomerOrder order in selectedList)
                {
                    RetailTransaction retailTransaction = GetTransactionFromOrder(order, false);

                    if (!displayBalanceMsg && retailTransaction.TransSalePmtDiff != decimal.Zero)
                    {
                        displayBalanceMsg = true;
                    }

                    if (retailTransaction.SaleItems.Count(c => !c.Voided || !c.Order.FullyReceived || c.Order.ToPickUp > 0) > 0)
                    {
                        RetailTransaction copyOfRetailTransaction = (RetailTransaction)retailTransaction.Clone();

                        List<ISaleLineItem> toRemove = copyOfRetailTransaction.SaleItems.Where(w => w.Voided || w.Order.FullyReceived).ToList();

                        foreach (ISaleLineItem item in toRemove)
                        {
                            copyOfRetailTransaction.SaleItems.Remove(item);
                        }

                        if (copyOfRetailTransaction.SaleItems.Any())
                        {
                            Interfaces.Services.CustomerOrderService(dlgEntry).PrintCustomerOrder(dlgEntry, copyOfRetailTransaction, FormSystemType.CustomerOrderPickingList, false);
                            Interfaces.Services.CustomerOrderService(dlgEntry).SaveCustomerOrder(dlgEntry, retailTransaction, CustomerOrderStatus.Printed);
                        }
                    }
                }

                LoadOrders(SearchCritiera);
            }
            finally
            {
                Interfaces.Services.DialogService(dlgEntry).CloseStatusDialog();
            }
        }

        /// <summary>
        /// Calls the site service with the search criteria and displays the list of customer orders / quotes that is returned
        /// </summary>
        /// <param name="searchCriteria">The search parameters</param>
        public void LoadOrders(CustomerOrderSearch searchCriteria)
        {
            searchCriteria.ReferenceBeginsWith = false;

            //If search criteria has no status set then use the default values that should be displayed
            if (searchCriteria.Status == -1)
            {
                searchCriteria.Status = (int) CustomerOrderStatus.Open;
                searchCriteria.Status |= (int) CustomerOrderStatus.Printed;
                searchCriteria.Status |= (int) CustomerOrderStatus.Ready;
                //searchCriteria.Status |= (int) CustomerOrderStatus.Delivered;
            }

            searchCriteria.OrderType = orderType;
            searchCriteria.RetrieveOrderXML = false;

            this.SearchCritiera = searchCriteria;
            
            int numerOfRowsToReturn = 150;

            int orderCount;

            currentCustomerOrderList = Interfaces.Services.CustomerOrderService(dlgEntry).Search(dlgEntry,
                                                                out orderCount,
                                                                numerOfRowsToReturn,
                                                                searchCriteria
                                                                );

            LoadOrdersIntoList();
            
        }

        private void LoadOrdersIntoList()
        {
            lvCustomerOrders.ClearRows();

            Row row;
            DataEntity store = null;
            CustomerOrderAdditionalConfigurations config = null;

            foreach (CustomerOrder order in currentCustomerOrderList)
            {
                row = new Row();

                LargeCheckBoxCell selectionCell = new LargeCheckBoxCell(order.Selected, MultiSelectionActive, CheckBoxCell.CheckBoxAlignmentEnum.Center);
                row.AddCell(selectionCell);

                row.AddText((string)order.Reference);

                config = configurations.FirstOrDefault(f => f.ID == order.Delivery);
                row.AddText(config == null ? "" : config.Text);

                config = configurations.FirstOrDefault(f => f.ID == order.Source);
                row.AddText(config == null ? "" : config.Text);

                store = stores.FirstOrDefault(f => f.ID == order.DeliveryLocation);
                row.AddText(store == null ? "" : store.Text);

                LargeCheckBoxCell commentCell = new LargeCheckBoxCell(!string.IsNullOrEmpty(order.Comment), false, CheckBoxCell.CheckBoxAlignmentEnum.Center);
                row.AddCell(commentCell);

                row.AddText(order.StatusText());

                if (order.ExpiryDate.DateTime.Date < DateTime.Now.Date)
                {
                    row.BackColor = ColorPalette.POSRedLight;
                }

                row.Tag = order;
                lvCustomerOrders.AddRow(row);
            }

            if (lvCustomerOrders.Rows.Count > 0)
            {
                lvCustomerOrders.Selection.AddRows(0, 0);
            }

            lvCustomerOrders_SelectionChanged(this, EventArgs.Empty);

            lvCustomerOrders.AutoSizeColumns();
        }

        /// <summary>
        /// Returns a list of selected customer orders
        /// </summary>
        /// <returns></returns>
        public List<CustomerOrder> GetSelectedOrders()
        {
            List<CustomerOrder> list = new List<CustomerOrder>();
            foreach (Row row in lvCustomerOrders.Rows)
            {
                if (((CheckBoxCell) row[0]).Checked)
                {
                    list.Add((CustomerOrder) row.Tag);
                }
            }
            return list;
        }

        /// <summary>
        /// Updates the check marks in the list
        /// </summary>
        public void SetSelectedCheckMarks()
        {
            foreach (Row row in lvCustomerOrders.Rows)
            {
                CustomerOrder rowOrder = (CustomerOrder) row.Tag;
                CustomerOrder order = currentCustomerOrderList.Find(f => f == rowOrder);
                if (order != null)
                {
                    if (((CheckBoxCell) row[0]).Checked != order.Selected)
                    {
                        ((CheckBoxCell) row[0]).Checked = order.Selected;
                    }
                }
            }

            lvCustomerOrders.InvalidateContent();
        }

        private void lvCustomerOrders_SelectionChanged(object sender, EventArgs e)
        {
            //If multiple selection is NOT active
            if (!multiSelectionActive)
            {
                //Unselect all those that are selected
                foreach (CustomerOrder order in currentCustomerOrderList)
                {
                    order.Selected = false;
                }
            }

            if (lvCustomerOrders.Selection.Count > 0)
            {
                SelectedOrder = (CustomerOrder) lvCustomerOrders.Rows[lvCustomerOrders.Selection.FirstSelectedRow].Tag;
                SelectedTransaction = GetTransactionFromOrder(SelectedOrder, true);

                receipt.ShowTransaction(SelectedTransaction);
                ShowCustomerInformation();
                ShowComment();

                SelectedTransaction.CustomerOrder.ID = SelectedOrder.ID;


                CustomerOrder currentlySelected = currentCustomerOrderList.Find(f => f == SelectedOrder);
                if (currentlySelected != null)
                {
                    currentlySelected.Selected = !currentlySelected.Selected;
                }
            }
            else
            {
                receipt.ShowTransaction(null);
            }

            SetSelectedCheckMarks();
        }

        #region Load transaction from order

        /// <summary>
        /// Retrieves the transaction XML for a specific customer order to be displayed in the search panel
        /// </summary>
        /// <param name="order">The order to be displayed</param>
        /// <param name="clearDeliveryInfo">If true then the delivery infomration is cleared</param>
        /// <returns></returns>
        public RetailTransaction GetTransactionFromOrder(CustomerOrder order, bool clearDeliveryInfo)
        {
            if (order.OrderXML == "")
            {
                CustomerOrderSearch orderSearch = new CustomerOrderSearch();
                orderSearch.RetrieveOrderXML = true;
                orderSearch.ID = order.ID;

                int orderCount = 0;
                int numberOfRowsToReturn = 150;
                List<CustomerOrder> getOrderXML = Interfaces.Services.CustomerOrderService(dlgEntry).Search(dlgEntry,
                                                                out orderCount,
                                                                numberOfRowsToReturn,
                                                                orderSearch
                                                                );
                if (getOrderXML != null)
                {
                    CustomerOrder temp = getOrderXML.FirstOrDefault(f => f.ID == order.ID);
                    if (temp != null)
                    {
                        order.OrderXML = temp.OrderXML;
                    }
                }
            }

            RetailTransaction retailTransaction = new RetailTransaction("", "", true);
            retailTransaction = (RetailTransaction)PosTransaction.CreateTransFromXML(order.OrderXML, (PosTransaction)retailTransaction, Interfaces.Services.ApplicationService(dlgEntry).PartnerObject);

            retailTransaction.CustomerOrder.CreatedAtStoreID = order.StoreID;
            retailTransaction.CustomerOrder.CreatedAtTerminalID = order.TerminalID;
            retailTransaction.CustomerOrder.CreatedByStaffID = order.StaffID;

            UpdateOrderedInformation(retailTransaction, order.OrderType, clearDeliveryInfo);
            MarkTenderLinesAsDeposits(retailTransaction);

            return retailTransaction;
        }

        private void UpdateOrderedInformation(RetailTransaction retailTransaction, CustomerOrderType orderType, bool clearPickups)
        {
            if (orderType != CustomerOrderType.CustomerOrder)
            {
                return;
            }

            foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => !w.Voided))
            {
                if (item.Order.Received == item.Order.Ordered)
                {
                    item.Order.FullyReceived = true;
                }

                if (item.Order.Received < item.Order.Ordered)
                {
                    item.Order.Ordered -= item.Order.Received;
                }

                if (clearPickups && item.Order.ToPickUp > 0)
                {
                    item.Order.ToPickUp = 0;
                }

                item.Order.ReservationDone = false;
            }
        }

        private void MarkTenderLinesAsDeposits(RetailTransaction retailTransaction)
        {
            foreach (ITenderLineItem item in retailTransaction.TenderLines.Where(w => !w.Voided))
            {
                item.PaidDeposit = true;
            }
        }

        #endregion

        private void ShowComment()
        {
            lblComment.Text = SelectedOrder.Comment ?? "";
        }

        private void ShowCustomerInformation()
        {
            if (SelectedTransaction.Customer.ID != SelectedOrder.CustomerID)
            {
                DisplayNoCustomerInfo(true);
            }
            else
            {
                DisplayNoCustomerInfo(false);
                string customerName = SelectedTransaction.Customer.FirstName != "" ? SelectedTransaction.Customer.GetFormattedName( dlgEntry.Settings.NameFormatter) : SelectedTransaction.Customer.Text;

                lblCustomer.Text = customerName;

                string address = dlgEntry.Settings.LocalizationContext.FormatMultipleLines(SelectedTransaction.Customer.DefaultShippingAddress, dlgEntry.Cache, "\n");
                lblAddress.Text = address;
            }
        }

        private void DisplayNoCustomerInfo(bool visible)
        {
            if (!visible)
            {
                pnlNoCustomerInfo.SendToBack();
            }
            else
            {
                pnlNoCustomerInfo.BringToFront();

                Customer transCustomer = Providers.CustomerData.Get(dlgEntry, SelectedTransaction.Customer.ID, UsageIntentEnum.Normal, CacheType.CacheTypeApplicationLifeTime);
                Customer orderCustomer = Providers.CustomerData.Get(dlgEntry, SelectedOrder.CustomerID, UsageIntentEnum.Normal, CacheType.CacheTypeApplicationLifeTime);

                string transCustomerName = "";
                string orderCustomerName = "";
                if (transCustomer != null)
                {
                    transCustomerName = transCustomer.FirstName != "" ? transCustomer.GetFormattedName( dlgEntry.Settings.NameFormatter) : transCustomer.Text;
                }
                
                if (orderCustomer != null)
                {
                    orderCustomerName = orderCustomer.FirstName != "" ? orderCustomer.GetFormattedName( dlgEntry.Settings.NameFormatter) : orderCustomer.Text;
                }

                lblNoCustomerInfo.Text = Resources.CustomerInformationOnSaleOrderNotTheSame + "\r\n" + "\r\n";
                lblNoCustomerInfo.Text += Resources.OnOrder + ": " + orderCustomerName + "\r\n";
                lblNoCustomerInfo.Text += Resources.OnSale + ": " + transCustomerName + "\r\n";

            }

            pnlNoCustomerInfo.Visible = visible;

        }

        private void lvCustomerOrders_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            if (args.ColumnNumber == 0)
            {
                Row row = lvCustomerOrders.Rows[args.RowNumber];
                CustomerOrder order = (CustomerOrder) row.Tag;

                if (((CheckBoxCell) row[0]).Checked != order.Selected)
                {
                    order.Selected = ((CheckBoxCell) row[0]).Checked;
                }
            }
        }

        private void pnlCustomerInfo_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, pnlCustomerInfo.Width - 1, pnlCustomerInfo.Height - 1);
            p.Dispose();
        }
    }
}
