using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Triggers
{
    public interface ICustomerTriggers
    {
        /// <summary>
        /// Triggered prior to clearing a customer from the transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PreCustomerClear(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction);

        /// <summary>
        /// Triggered after clearing a customer from the transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PostCustomerClear(IConnectionManager entry, IPosTransaction posTransaction);

    }
}
