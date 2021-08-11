using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Triggers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPaymentTriggers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="posTransaction"></param>
        /// <param name="amount"></param>
        void PrePayCustomerAccount(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, decimal amount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="posTransaction"></param>
        /// <param name="cardInfo"></param>
        /// <param name="amount"></param>
        void PrePayCardAuthorization(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, CardInfo cardInfo, ref decimal amount);

        /// <summary>
        /// Triggered when the payment has been added to the transaction.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction"></param>
        void OnPayment(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Triggered on each payment operation, before setting an amount, swiping any cards, entering any input or showing any dialogs.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="posOperation">The POS operation calling the trigger</param>
        /// <param name="tenderId">The tender id</param>
        void PrePayment(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, POSOperations posOperation, object tenderId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        /// <param name="tenderLineItem"></param>
        void PostPayment(IConnectionManager entry, IPosTransaction posTransaction, ITenderLineItem tenderLineItem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="posTransaction"></param>
        /// <param name="tenderLineItem"></param>
        void PreVoidPayment(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, ITenderLineItem tenderLineItem);

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        /// <param name="tenderLineItem"></param>
        void PostVoidPayment(IConnectionManager entry, IPosTransaction posTransaction, ITenderLineItem tenderLineItem);
    }
}
