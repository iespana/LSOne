using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Triggers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDiscountTriggers
    {

        /// <summary>
        /// Summary pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="transaction"></param>
        /// <param name="LineId"></param>
        void PreLineDiscountAmount(IConnectionManager entry, PreTriggerResults results, IPosTransaction transaction, int LineId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="transaction"></param>
        void PostLineDiscountAmount(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>
        /// Summary pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="transaction"></param>
        /// <param name="LineId"></param>
        void PreLineDiscountPercent(IConnectionManager entry, PreTriggerResults results, IPosTransaction transaction, int LineId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="transaction"></param>
        void PostLineDiscountPercent(IConnectionManager entry, IPosTransaction transaction);
    }
}
