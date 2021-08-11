using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.Services.Interfaces
{
    public interface ITenderService : IService
    {
        /// <summary>
        /// Returns the error text (if any) that was created when the tender was being checked in IsTenderAllowed
        /// </summary>
        string ErrorText { get; set; }

        /// <summary>
        /// Goes through all the configurations on the payment type and tender type and decides if the tender can be used for this particular payment.
        /// To view the reason why it is not allowed use property ErrorText
        /// </summary>
        /// <param name="dataModel">Entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        /// <param name="storeCurrencyID">The currency ID for the store</param>
        /// <param name="tenderInfo">The payment method information</param>
        /// <param name="paidAmount">The amount being paid</param>
        /// <param name="manuallyEnteredAmount">Was the amount entered manually</param>
        /// <param name="balance">The current balance of the transaction</param>
        /// <param name="transactionTotal">The total balance of the transaction</param>
        /// <param name="payment">Total amount paid already on the transaction</param>
        /// <param name="restrictedAmount">The restricted amount for the ongoing payment</param>
        /// <returns></returns>
        bool IsTenderAllowed(IConnectionManager dataModel, IPosTransaction transaction, RecordIdentifier storeCurrencyID, StorePaymentMethod tenderInfo, decimal paidAmount, bool manuallyEnteredAmount, decimal balance, decimal transactionTotal, decimal payment = 0, decimal restrictedAmount = 0);

        /// <summary>
        /// Returns a string with details about the tender line. Is used when printing the receipts
        /// </summary>
        /// <param name="dataModel">Entry into the database</param>
        /// <param name="tenderLine">The tender line being printed</param>
        /// <returns></returns>
        string GetTenderDetails(IConnectionManager dataModel, ITenderLineItem tenderLine);
    }
}
