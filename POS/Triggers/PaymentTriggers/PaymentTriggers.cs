using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using LSOne.Services.Interfaces.SupportClasses;

namespace LSOne.Triggers
{
    public class PaymentTriggers : IPaymentTriggers
    {        

        #region Constructor - Destructor

        public PaymentTriggers(IConnectionManager entry)
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for PaymentTriggers are reserved at 50400 - 50449
        }

        ~PaymentTriggers()
        {

        }

        #endregion

        #region IPaymentTriggers Members

        public void PrePayCustomerAccount(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, decimal amount)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Before charging to a customer account", "PaymentTriggers.PrePayCustomerAccount");


            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }


        public void PrePayCardAuthorization(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, CardInfo cardInfo, ref decimal amount)
        {

            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Before the EFT authorization", "PaymentTriggers.PrePayCardAuthorization");


            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }

        }

        /// <summary>
        /// <example><code>
        /// // In order to delete the already-added payment you use the following code:
        /// if (retailTransaction.TenderLines.Count > 0)
        /// {
        ///     retailTransaction.TenderLines.RemoveLast();
        ///     retailTransaction.LastRunOperationIsValidPayment = false;
        /// }
        /// </code></example>
        /// </summary>
        public void OnPayment(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "On the addition of a tender...",
                    "PaymentTriggers.OnPayment");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }


        public void PrePayment(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, POSOperations posOperation, object tenderId)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "On the start of a payment operation...", "PaymentTriggers.PrePayment");
              
                switch((POSOperations)posOperation)
                {
                    case POSOperations.PayCash:
                        // Insert code here...
                        break;
                    case POSOperations.PayCard:
                        // Insert code here...
                        break;
                    case POSOperations.PayCheque:
                        // Insert code here...
                        break;
                    case POSOperations.PayCorporateCard:
                        // Insert code here...
                        break;
                    case POSOperations.PayCreditMemo:
                        // Insert code here...
                        break;
                    case POSOperations.PayCurrency:
                        // Insert code here...
                        break;
                    case POSOperations.PayCustomerAccount:
                        // Insert code here...
                        break;
                    case POSOperations.PayGiftCertificate:
                        // Insert code here...
                        break;
                    case POSOperations.PayLoyalty:
                        // Insert code here...
                        break;

                    // etc.....
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PostPayment(IConnectionManager entry, IPosTransaction posTransaction, ITenderLineItem tenderLineItem)
        {
        }

        public void PreVoidPayment(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, ITenderLineItem tenderLineItem)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "When voiding a payment line", "PaymentTriggers.PreVoidPayment");
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PostVoidPayment(IConnectionManager entry, IPosTransaction posTransaction, ITenderLineItem tenderLineItem)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After voiding a payment line", "PaymentTriggers.PostVoidPayment");
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
