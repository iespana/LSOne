using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Invoice;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesInvoiceItem;
using LSOne.POS.Core.Exceptions;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Controls.SupportClasses;
using LSOne.Controls.Dialogs.SelectionDialog;

namespace LSOne.Services
{
    public partial class SalesInvoiceService : ISalesInvoiceService
    {
        #region Constructor

        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618
        }

        public virtual IErrorLog ErrorLog { set; private get; }

        #endregion

        #region ISalesInvoice Members

        public virtual bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction)
        {
            //ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            //if (transaction is RetailTransaction)
            //{
            //    return ((RetailTransaction)transaction).SaleItems.Any(a => a is SalesInvoiceLineItem);

            //}            
            return false;
        }

        public virtual void SalesInvoices(IConnectionManager entry, ref IPosTransaction posTransaction)
        {

            // The sales invoice functionality is only allowed if a customer has already been added to the transaction
            if (((RetailTransaction)posTransaction).Customer.ID == RecordIdentifier.Empty)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.PleaseAddCustomer, MessageBoxButtons.OK, MessageDialogType.Attention);                
                return;
            }

            PaySalesInvoice(entry, ref posTransaction);

        }



        protected virtual void PaySalesInvoice(IConnectionManager entry, ref IPosTransaction posTransaction)
        {

            try
            {
                List<Invoice> salesInvoices = GetSalesInvoicesForCustomer(entry, posTransaction);

                if (salesInvoices.Count == 0)
                {

                    // There are no sales invoices in the database for this customer....
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.AlreadyHasSalesInvoice, MessageBoxButtons.OK, MessageDialogType.Attention);                    

                    return;
                }

                // Show the available sales invoices for selection...
                using (SelectionDialog salesInvoicesDialog = new SelectionDialog(new InvoiceSelectionList(salesInvoices), Properties.Resources.SalesInvoice, false))
                {
                    if (salesInvoicesDialog.ShowDialog() == DialogResult.OK)
                    {
                        Invoice invoice = (Invoice)salesInvoicesDialog.SelectedItem;
                        // Check if this Sales Invoice has already been added to the transaction
                        foreach (SaleLineItem salesInvoiceInTrans in ((RetailTransaction)posTransaction).SaleItems)
                        {
                            if (((object)salesInvoiceInTrans).GetType() == typeof(SalesInvoiceLineItem))
                            {
                                if (salesInvoiceInTrans.Voided == false)
                                {
                                    if (((SalesInvoiceLineItem)salesInvoiceInTrans).SalesInvoiceId == invoice.ID)
                                    {
                                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.AlreadyHasSalesInvoice, MessageBoxButtons.OK, MessageDialogType.ErrorWarning); // This sales invoice has already been added to the transaction                                    
                                        return;
                                    }
                                }
                            }
                        }

                        // There is a valid sales invoice selected and it's not been already added to the transaction. So let's get the details for it...
                        SalesInvoiceLineItem salesInvoiceLineItem = new SalesInvoiceLineItem((RetailTransaction)posTransaction);

                        GetSalesInvoice(entry, ref posTransaction, ref salesInvoiceLineItem, (string)invoice.ID);

                        // And add it to the transaction
                        ((RetailTransaction)posTransaction).Add(salesInvoiceLineItem);
                        ((RetailTransaction)posTransaction).SalesInvoiceAmounts += salesInvoiceLineItem.Amount;
                    }
                }

            }
            catch (POSException px)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                throw px;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
            finally
            {
                Services.Interfaces.Services.CalculationService(entry).CalculateTotals(entry, posTransaction);                
            }
        }



        /// <summary>
        /// Get a list of sales invoices for a specific customer...
        /// </summary>
        /// <returns></returns>
        protected virtual List<Invoice> GetSalesInvoicesForCustomer(IConnectionManager entry, IPosTransaction posTransaction)
        {
            entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Getting the list of sales invoices for a customer", "SalesOrder.GetSalesInvoicesForCustomer");

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            try
            {                
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                return service.GetSalesInvoiceList(entry, settings.SiteServiceProfile, (string)((RetailTransaction)posTransaction).Customer.ID);

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }


        protected virtual void GetSalesInvoice(IConnectionManager entry, ref IPosTransaction posTransaction, ref SalesInvoiceLineItem salesInvoiceLineItem, string salesInvoiceId)
        {

            try
            {
                bool retval = false;
                string comment = "";

                string invoiceId = salesInvoiceId;
                decimal totalPaidAmount = 0M;
                decimal totalInvoiceAmount = 0M;
                string customerAccount = "";
                string customerName = "";
                DateTime creationDate = new DateTime();


                // Begin by checking if there is a connection to the Transaction Service
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                service.GetSalesInvoice(entry, settings.SiteServiceProfile, ref retval, ref comment, ref invoiceId, ref totalPaidAmount, ref totalInvoiceAmount,
                                                                 ref customerAccount, ref customerName, ref creationDate);


                // Populate the salesInvoiceLineItem with the respective values...
                salesInvoiceLineItem.SalesInvoiceId = invoiceId;
                salesInvoiceLineItem.Description = Properties.Resources.SalesInvoice; // Sales Invoice
                salesInvoiceLineItem.CreationDate = creationDate;
                salesInvoiceLineItem.Amount = totalInvoiceAmount - totalPaidAmount;  // The balance/remainder of the sales invoice
               

                // Necessary property settings for the the sales invoice "item"...
                salesInvoiceLineItem.Price = salesInvoiceLineItem.Amount;
                salesInvoiceLineItem.PriceWithTax = salesInvoiceLineItem.Amount;
                salesInvoiceLineItem.StandardRetailPrice = salesInvoiceLineItem.Amount;
                salesInvoiceLineItem.Quantity = 1;
                salesInvoiceLineItem.TaxRatePct = 0;
                salesInvoiceLineItem.Comment = salesInvoiceLineItem.SalesInvoiceId;
                salesInvoiceLineItem.NoDiscountAllowed = true;
                salesInvoiceLineItem.Found = true;
                
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }


        #endregion
    }
}
