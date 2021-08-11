using LSOne.Controls;
using LSOne.Controls.Dialogs.SelectionDialog;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.SalesOrder;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SalesOrderItem;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.Dialogs
{
    public partial class SalesOrderDialog : TouchBaseForm
    {
        private enum Buttons
        {
            PrepayOrder,
            PrintPickingList,
            PrintPackingSlip,
            GetSalesOrder,
            Cancel
        }

        private string salesOrderCustomerId;
        private SalesOrder currentSalesOrder;
        private RecordIdentifier customerID;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        
        public SalesOrderDialog(IConnectionManager entry, RetailTransaction transaction)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            Transaction = transaction;
            AddButtons();
            LoadSalesOrder();
        }

        public RetailTransaction Transaction { get; set; }

        public SalesOrderLineItem LineItem { get; set; }

        private void AddButtons()
        {
            btnPanel.AddButton(Resources.PrepayOrder, Buttons.PrepayOrder, Conversion.ToStr((int)Buttons.PrepayOrder));
            btnPanel.AddButton(Resources.PrintPickingList, Buttons.PrintPickingList, Conversion.ToStr((int)Buttons.PrintPickingList));
            btnPanel.AddButton(Resources.PrintPackingSlip, Buttons.PrintPackingSlip, Conversion.ToStr((int)Buttons.PrintPackingSlip));
            btnPanel.AddButton(Resources.GetSalesOrder, Buttons.GetSalesOrder, Conversion.ToStr((int)Buttons.GetSalesOrder), TouchButtonType.OK, DockEnum.DockEnd);
            btnPanel.AddButton(Resources.Cancel, Buttons.Cancel, Conversion.ToStr((int)Buttons.Cancel), TouchButtonType.Cancel, DockEnum.DockEnd);

            btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.PrepayOrder), false);
            btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.PrintPickingList), false);
            btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.PrintPackingSlip), false);
            btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.GetSalesOrder), false);
        }

        private void LoadSalesOrder()
        {
            if (Transaction.Customer.ID != RecordIdentifier.Empty)
            {
                customerID = (string)Transaction.Customer.ID;
                tbCustomerId.Text = Transaction.Customer.FirstName != ""
                    ? Transaction.Customer.GetFormattedName(dlgEntry.Settings.NameFormatter)
                    : Transaction.Customer.Text;
            }
            else
            {
                customerID = RecordIdentifier.Empty;
                tbCustomerId.Text = "";
            }

            ClearSalesOrderInput();
        }

        private void btnSearchSalesOrder_Click(object sender, EventArgs e)
        {
            try
            {
                List<SalesOrder> salesOrder = GetSalesOrderForCustomer();

                using (SelectionDialog salesOrdersDialog = new SelectionDialog(new SalesOrderSelectionList(salesOrder, null), Resources.SalesOrder, false, false, tbSalesOrderId.Text))
                {
                    if (salesOrdersDialog.ShowDialog() == DialogResult.OK)
                    {
                        SalesOrder order = (SalesOrder)salesOrdersDialog.SelectedItem;
                        tbSalesOrderId.Text = (string)order.ID;
                        PopulateSalesOrder(order);
                        PopulateCustomerInfo(order.CustomerId);
                    }
                    else
                    {
                        tbSalesOrderId.Text = "";
                    }
                }
            }
            catch (Exception x)
            {
                dlgEntry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(x is ClientTimeNotSynchronizedException ? x.Message : Resources.ErrorConnectingToSiteService, MessageDialogType.ErrorWarning);
            }
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            RetailTransaction tempTrans = new RetailTransaction((string)dlgEntry.CurrentStoreID, dlgSettings.Store.Currency, dlgSettings.TaxIncludedInPrice);
            tempTrans = (RetailTransaction)Interfaces.Services.CustomerService(dlgEntry).Search(dlgEntry, tempTrans, true);

            if (tempTrans.Customer.ID != RecordIdentifier.Empty)
            {
                tbCustomerId.Text = tempTrans.Customer.FirstName != ""
                    ? tempTrans.Customer.GetFormattedName( dlgEntry.Settings.NameFormatter)
                    : tempTrans.Customer.Text;

                if (customerID != tempTrans.Customer.ID)
                {
                    ClearSalesOrderInput();
                }

                customerID = tempTrans.Customer.ID;
            }
        }

        private void GetSalesOrder()
        {
            if (!CheckForSalesOrder())
            {
                return;
            }
            
            if (!CheckForSalesOrderAdded())
            {
                return;
            }

            if (!AddCustomerToTransaction())
            {
                return;
            }

            LineItem.SalesOrderItemType = SalesOrderItemType.FullPayment;
            LineItem.Description = Resources.SalesOrderPayment;
            LineItem.SalesOrderId = currentSalesOrder.ID.ToString();
            LineItem.Balance = currentSalesOrder.Balance;
            LineItem.Amount = currentSalesOrder.Balance;
            LineItem.Price = currentSalesOrder.Balance;
            LineItem.PriceWithTax = currentSalesOrder.Balance;
            LineItem.StandardRetailPrice = currentSalesOrder.Balance;
            LineItem.Quantity = 1;
            LineItem.TaxRatePct = 0;
            LineItem.Comment = Resources.SalesOrder + ": " + LineItem.SalesOrderId;
            LineItem.NoDiscountAllowed = true;
            LineItem.Found = true;

            Transaction.Add(LineItem);

            Transaction.SalesOrderAmounts += LineItem.Amount;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void PrepaySalesOrder()
        {
            if (!CheckForSalesOrder())
            {
                return;
            }

            if (currentSalesOrder.Prepayment == 0)
            {                
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.AmountNotCalculated);
                return;
            }

            if (!CheckForSalesOrderAdded())
            {
                return;
            }

            if (!AddCustomerToTransaction())
            {
                return;
            }

            LineItem.SalesOrderItemType = SalesOrderItemType.PrePayment;
            LineItem.Description = Resources.SalesOrderPrepayment;
            LineItem.SalesOrderId = currentSalesOrder.ID.ToString();
            LineItem.Balance = currentSalesOrder.Balance;
            LineItem.Amount = currentSalesOrder.Prepayment;
            LineItem.Price = currentSalesOrder.Prepayment;
            LineItem.PriceWithTax = currentSalesOrder.Prepayment;
            LineItem.StandardRetailPrice = currentSalesOrder.Prepayment;
            LineItem.Quantity = 1;
            LineItem.TaxRatePct = 0;
            LineItem.Comment = Resources.SalesOrder + ": " + LineItem.SalesOrderId;
            LineItem.NoDiscountAllowed = true;
            LineItem.Found = true;

            Transaction.Add(LineItem);
            Transaction.SalesInvoiceAmounts += LineItem.Amount;
        }

        private void CreatePickingList()
        {
            try
            {
                if (!CheckForSalesOrder())
                {
                    return;
                }

                if (!AddCustomerToTransaction())
                {
                    return;
                }

                SalesOrderRequest soRequest = new SalesOrderRequest();
                soRequest.ID = LineItem.SalesOrderId;

                if (Interfaces.Services.SiteServiceService(dlgEntry).CreatePickingList(dlgEntry, dlgSettings.SiteServiceProfile, soRequest, true) != SalesOrderResult.Success)
                {
                    return;
                }

                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.PickingListCreated);
            }
            catch (Exception x)
            {
                dlgEntry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.ErrorCreatingPickingList);
            }
        }

        private void CreatePackingSlip()
        {
            try
            {
                if (!CheckForSalesOrder())
                {
                    return;
                }

                if (!AddCustomerToTransaction())
                {
                    return;
                }

                SalesOrderRequest soRequest = new SalesOrderRequest();
                soRequest.ID = LineItem.SalesOrderId;
                if (Interfaces.Services.SalesOrderService(dlgEntry).CreatePackingSlip(dlgEntry, dlgSettings.SiteServiceProfile, soRequest) != SalesOrderResult.Success)
                {
                    return;
                }

                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.PackingSlipCreated);
            }
            catch (Exception x)
            {
                dlgEntry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.ErrorCreatingPackingSlip);
            }
        }

        private bool CheckForSalesOrder()
        {
            if (LineItem == null)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.SalesOrderIdMissingError);
                return false;
            }

            return true;
        }

        private bool CheckForSalesOrderAdded()
        {
            if (Transaction.SaleItems.Any(a => a is SalesOrderLineItem && !a.Voided && ((SalesOrderLineItem)a).SalesOrderId == LineItem.SalesOrderId))
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.SalesOrderAlreadyAdded);
                return false;
            }
            
            return true;
        }

        private bool AddCustomerToTransaction()
        {
            Customer tempCust = Providers.CustomerData.Get(dlgEntry, salesOrderCustomerId, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime);

            if (tempCust != null)
            {                
                if (Transaction.Add(tempCust))
                {
                    Transaction.AddInvoicedCustomer(Providers.CustomerData.Get(dlgEntry, tempCust.AccountNumber, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime));
                }

                return true;
            }
            
            Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CustomerNotFound);
            return false;
        }

        private List<SalesOrder> GetSalesOrderForCustomer()
        {
            dlgEntry.ErrorLogger.LogMessage(LogMessageType.Trace, "Getting the list of sales orders for a customer", "SalesOrder.GetSalesOrdersForCustomer");            

            try
            {
                SalesOrderRequest soRequest = new SalesOrderRequest();
                soRequest.CustomerID = tbCustomerId.Text.Trim();
                soRequest.ID = tbSalesOrderId.Text.Trim();
                List<SalesOrder> salesOrders = new List<SalesOrder>();
                if (Interfaces.Services.SalesOrderService(dlgEntry).GetSalesOrderList(dlgEntry, dlgSettings.SiteServiceProfile, soRequest, salesOrders) == SalesOrderResult.Success)
                {
                    return salesOrders;
                }

                return new List<SalesOrder>();
            }
            catch (Exception x)
            {
                dlgEntry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                throw x;
            }
        }

        private void ClearSalesOrderInput()
        {
            tbSalesOrderId.Text = "";
            lblCreated.Text = "-";
            lblPrepaid.Text = "-";
            lblForPrepayment.Text = "-";
            lblTotal.Text = "-";
            lblBalance.Text = "-";
        }

        private void PopulateSalesOrder(SalesOrder salesOrder)
        {
            currentSalesOrder = salesOrder;
            LineItem = new SalesOrderLineItem(Transaction);
            IRoundingService rounding = Interfaces.Services.RoundingService(dlgEntry);

            salesOrderCustomerId = salesOrder.CustomerId.ToString();

            // Populate the sales order part of the Sales Order dialog...
            tbSalesOrderId.Text = salesOrder.ID.ToString();
            lblCreated.Text = salesOrder.Created.ToShortDateString();
            lblPrepaid.Text = rounding.RoundString(dlgEntry, salesOrder.Prepaid, dlgSettings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);
            lblForPrepayment.Text = rounding.RoundString(dlgEntry, salesOrder.Prepayment, dlgSettings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);
            lblTotal.Text = rounding.RoundString(dlgEntry, salesOrder.Total, dlgSettings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);
            lblBalance.Text = rounding.RoundString(dlgEntry, salesOrder.Balance, dlgSettings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);

            // Enable the buttons that work with the selected sales order
            btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.PrintPickingList), true);
            btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.PrintPackingSlip), true);
            btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.GetSalesOrder), true);

            if (salesOrder.Prepayment != 0)
            {
                btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.PrepayOrder), true);
            }
        }

        private void PopulateCustomerInfo(RecordIdentifier customerId)
        {
            // Populate the customer part of the Sales Order dialog...            
            Customer customer = Interfaces.Services.SiteServiceService(dlgEntry).GetCustomer(dlgEntry, dlgSettings.SiteServiceProfile, customerId, true, true);

            tbCustomerId.Text = customer.Text;
            customerID = customer.ID;
        }

        private void tbSalesOrderId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) //enter
            {
                btnSearchSalesOrder_Click(this, e);
                e.Handled = true;
            }
        }

        private void tbCustomerId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) //enter
            {
                btnSearchCustomer_Click(this, e);
                e.Handled = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnPanel_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((Buttons)args.Tag)
            {
                case Buttons.PrepayOrder:
                    PrepaySalesOrder();
                    break;
                case Buttons.PrintPickingList:
                    CreatePickingList();
                    break;
                case Buttons.PrintPackingSlip:
                    CreatePackingSlip();
                    break;
                case Buttons.GetSalesOrder:
                    GetSalesOrder();
                    break;
                case Buttons.Cancel:
                    DialogResult = DialogResult.Cancel;
                    Close();
                    break;
            }
        }
    }
}