using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Triggers
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransactionTriggers
    {

        /// <summary>
        /// Triggered at the start of a new transaction, but after loading the transaction with initialisation 
        /// data, such as the store, terminal number, date, etc...
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void BeginTransaction(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// Triggered at the end a transaction, before saving the transaction and printing of receipts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="posTransaction"></param>
        void PreEndTransaction(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction);

        /// <summary>
        /// Triggered at the end a transaction, after saving the transaction and printing of receipts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PostEndTransaction(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// Summary pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="posTransaction"></param>
        void PreVoidTransaction(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction);
        
        /// <summary>
        /// Pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PostVoidTransaction(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// Pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results">The return parameter</param>
        /// <param name="originalTransaction">The original transaction</param>
        /// <param name="posTransaction">The transaction containing only the selected items to be returned</param>
        void PreReturnTransaction(IConnectionManager entry, PreTriggerResults results, IRetailTransaction originalTransaction, IPosTransaction posTransaction);
        
        /// <summary>
        /// Pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PostReturnTransaction(IConnectionManager entry, IPosTransaction posTransaction);
    }
}
