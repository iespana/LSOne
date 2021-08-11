using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Triggers    
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInfocodeTriggers
    {
        /// <summary>
        /// Before the infocode is processed this trigger is called. If the infocode should not be processed 
        /// after the trigger has finished running then PreTriggerResults needs to be filled out accordingly.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="transaction"></param>
        /// <param name="tableRefId"></param>
        void PreProcessInfocode(IConnectionManager entry, PreTriggerResults results, IPosTransaction transaction, InfoCodeLineItem.TableRefId tableRefId);

        /// <summary>
        /// After the infocode has been processed this trigger is called. Any checking and/or additional input validation and etc. can be done here.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="transaction"></param>
        /// <param name="tableRefId"></param>        
        void PostProcessInfocode(IConnectionManager entry, PreTriggerResults results, IPosTransaction transaction, InfoCodeLineItem.TableRefId tableRefId);
    }
}