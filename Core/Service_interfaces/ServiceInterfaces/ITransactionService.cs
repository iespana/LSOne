using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public interface ITransactionService : IService
    {
        /// <summary>
        /// Calculates the price, tax and discount amounts based on the current transaction information. This will not refresh information on trade agreements of prices from the database, but only
        /// use information that have already been cached. To re-calculate the prices (e.g. after a customer change or to refresh trade agreements) use <see cref="RecalculatePriceTaxDiscount(IConnectionManager, IPosTransaction, bool, bool)"/>
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="transaction">The transaction to calculate the amounts for</param>
        void CalculatePriceTaxDiscount(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>
        /// Calculates the price, tax and discount amounts. This will read all price and trade-agreement information from the database.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="transaction">The transaction to calculate</param>
        /// <param name="restoreItemPrices">If true then price- and tax amounts are re-calculated</param>
        /// <param name="calculateDiscountsNow">If true then the current discount information is cleared and re-fetched from the database</param>
        void RecalculatePriceTaxDiscount(IConnectionManager entry, IPosTransaction transaction, bool restoreItemPrices, bool calculateDiscountsNow = false);
        
        /// <summary>
        /// Adds the customer with the given ID to the transaction and re-calculates all prices/discounts for that customer
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="session">The current session</param>
        /// <param name="transaction">The transaction to add the customer to</param>
        /// <param name="customerID">The ID of the customer to add</param>
        /// <param name="processInfocodes">If true the infocodes defined for that customer are also processed, otherwise they are skipped</param>
        void AddCustomerToTransaction(IConnectionManager entry, ISession session, IPosTransaction transaction, RecordIdentifier customerID, bool processInfocodes);

        /// <summary>
        /// Executes the "Clear customer" operation and removes the current customer from the tranasction.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="transaction">The transaction to clear the customer from</param>
        void ClearCustomerFromTransaction(IConnectionManager entry, ref IPosTransaction transaction);

        /// <summary>
        /// Concludes the transaction and saves it to the database after all payments have been registered (if applicable for the current transaction). This executes the final steps of the transaction like printing, generating a receipt-id, calling any services
        /// to perform the final steps etc. 
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="transaction">The tranasction to conclude</param>        
        void ConcludeTransaction(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>
        /// Initialises the header information for the transaction such as store-, terminal- and transaction ID.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="transaction">The transaction to initialize</param>
        /// <param name="rebuildingTransaction">If true then <paramref name="transaction"/> should be treated as a transaction that is being re-built from the posted transaction tables. Transaction ID and receipt ID information should not be refreshed if this is set to true.</param>
        /// <param name="generateTransactionID">If true then a new transaction ID will always be generated for the given transaction</param>
        void LoadTransactionStatus(IConnectionManager entry,IPosTransaction transaction, bool rebuildingTransaction = false, bool generateTransactionID = false);

        /// <summary>
        /// Executes the return process (i.e. show the return dialog) for the given transaction
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="session">The current session</param>
        /// <param name="transaction">The transaction instance that should be populated with the items to return</param>
        /// <param name="receiptID">The receipt ID of the transaction to return</param>
        /// <param name="transactionID">The transaction ID of the transaction to return</param>
        /// <param name="useCentralReturns">If true the transaction information is retrieved from the Site Service and if a connection cannot be made then the return process is cancelled. If set to false then the transaction is retrieved from the local database and the Site Service connection is not verified.</param>
        /// <param name="showReasonCodesSelectList">If true the user is prompted to select a reason code for the returned transaction.</param>
        /// <param name="defaultReasonCodeID">If <paramref name="showReasonCodesSelectList"/> is set to <see langword="false"/> then this value will be used for the reason code</param>
        void ReturnTransaction(IConnectionManager entry, ISession session, IPosTransaction transaction, RecordIdentifier receiptID, RecordIdentifier transactionID, bool useCentralReturns, bool showReasonCodesSelectList, string defaultReasonCodeID);

        /// <summary>
        /// Shows the return-transaction dialog so the user can set a reason code for individual sale lines
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="session">The current session</param>
        /// <param name="transaction">The transaction to set reason codes for</param>
        void SetReasonCode(IConnectionManager entry, ISession session, IPosTransaction transaction);

        /// <summary>
        /// Prints a gift receipt for the given tranasction
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="transaction">The transaction instance that should be populated with the items to print the gift receipt for</param>
        /// <param name="receiptID">The receipt ID of the transaction to get</param>
        /// <param name="transactionID">The transaction ID of the transaction to get</param>
        void PrintGiftReceipt(IConnectionManager entry, IPosTransaction transaction, RecordIdentifier receiptID, RecordIdentifier transactionID);

        /// <summary>
        /// Returns the sequenced numbers/IDs on the transaction back to the number sequence
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="posTransaction">The transaction to return the sequenced numbers/IDs for</param>
        /// <param name="returnReceiptID">If true the <see cref="IPosTransaction.ReceiptId"/> for the <paramref name="posTransaction"/> is returned to the number sequence</param>
        /// <param name="returnTransactionID">If true the <see cref="IPosTransaction.TransactionId"/> for the <paramref name="posTransaction"/> is returned to the number sequence</param>
        void ReturnSequencedNumbers(IConnectionManager entry, IPosTransaction posTransaction, bool returnReceiptID, bool returnTransactionID);

        /// <summary>
        /// Goes through the transaction and checks if the current status of the cash-drawer matches the limits that have been defined for the current payments. If any limits have been exceeded this returns the a message that describes what action needs to be taken.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="posTransaction">The transaction to check</param>
        /// <returns></returns>
        string CheckTenderStatusInDrawer(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// Creates an instance of <see cref="ICloneTransactions"/>
        /// </summary>
        /// <returns>An new instance that implements the <see cref="ICloneTransactions"/> interface</returns>
        ICloneTransactions CreateCloneTransactions();

        /// <summary>
        /// Creates and sends an email with the receipt for the given transaction based on the current email configuration for the store.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="session">The current session</param>
        /// <param name="transaction">The transaction to email a receipt for</param>
        /// <param name="currentOption">Controls what receipt to send</param>
        /// <param name="operationInfo">Information about the current operation</param>
        void EmailReceipt(IConnectionManager entry, ISession session, IPosTransaction transaction, ReceiptEmailParameterEnum currentOption, OperationInfo operationInfo);

        /// <summary>
        /// Reprints a receipt
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="session">The current session</param>
        /// <param name="transaction">The current transaction</param>
        /// <param name="operationInfo">Information about the current operation</param>
        /// <param name="currentReprintType">Controls the type of receipt to reprint</param>
        /// <param name="customText"></param>
        void ReprintReceipt(IConnectionManager entry, ISession session, IPosTransaction transaction, OperationInfo operationInfo, ReprintReceiptEnum currentReprintType, string customText);

        /// <summary>
        /// Sends the receipt for the given transaction via email
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="transaction">The tranasction to create a receipt for</param>
        void CreateAndSendEmailReceipt(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>
        /// Creates a file name for the receipt attachment that is going to be emailed. If the transaction is a RetailTransaction then the receipt ID
        /// is used for the file name otherwise the file name is set to "TRID-" + transaction ID.
        /// </summary>
        /// <param name="transaction">The current transaction</param>
        /// <param name="formInfo">The form that is going to be emailed</param>
        /// <returns></returns>
        string CreateEmailAttachmentName(IPosTransaction transaction, FormInfo formInfo);

        /// <summary>
        /// If true then the transaction is allowed to send an email based on the current email configuration for the store.
        /// </summary>
        /// <param name="transaction">The current transaction</param>
        /// <returns></returns>
        bool TransactionCanSendEmail(IPosTransaction transaction);

        /// <summary>
        /// Checks if the given transaction requires a Site Service connection
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="transaction">The transaction to check</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <returns>True if a connection is needed, false otherwise</returns>
        bool SiteServiceConnectionIsNeededandAlive(IConnectionManager entry, IPosTransaction transaction, ISettings settings);

        /// <summary>
        /// Called from the price override operation when the item selected needs special price override functionality.
        /// In all versions including LS One 2018 this function is only called for a sales order item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        /// <param name="operationInfo">Information about what is going on in the POS i.e. the item selected, payment line selected and etc</param>
        /// <returns></returns>
        bool OnPriceOverride(IConnectionManager entry, IPosTransaction transaction, OperationInfo operationInfo);

        /// <summary>
        /// Set a sales person on the current item in the transaction
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="session">POS session</param>
        /// <param name="transaction">Current transaction</param>
        /// <param name="operationInfo">Current operation info</param>
        /// <param name="salesPersonID">ID of the sales person to set. If not set, a dialog is displayed.</param>
        /// <param name="hereAfter">True if all further items added to the transaction will have the same sales person</param>
        void SetSalesPerson(IConnectionManager entry, ISession session, IPosTransaction transaction, OperationInfo operationInfo, RecordIdentifier salesPersonID, bool hereAfter);

        /// <summary>
        /// Decide if a sales person operation can be triggered based on the functionality profile settings and the current operation executed
        /// </summary>
        /// <param name="settings">Current settings</param>
        /// <param name="transaction">Current transaction after the main operation was executed</param>
        /// <param name="operationID">The operation that was executed</param>
        /// <param name="saleItemsBeforeOperation">The list of sale items from the transaction before the main operation was executed</param>
        /// <returns>True if a sales operation must be triggered</returns>
        bool CanRunSalesPersonOperation(ISettings settings, IPosTransaction transaction, POSOperations operationID, LinkedList<ISaleLineItem> saleItemsBeforeOperation);

        /// <summary>
        /// Returns true if a sale line item is eligible for return
        /// </summary>
        /// <param name="item">Sale line item</param>
        /// <returns></returns>
        bool CanReturnSaleLineItem(ISaleLineItem item);
    }
}
