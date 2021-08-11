using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Triggers
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICashManagementTriggers
    {

        #region Tender Declaration

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="transction"></param>
        void PreTenderDeclaration(IConnectionManager entry, PreTriggerResults results, IPosTransaction transction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transction"></param>
        void PostTenderDeclaration(IConnectionManager entry, IPosTransaction transction);


        #endregion

    }
}
