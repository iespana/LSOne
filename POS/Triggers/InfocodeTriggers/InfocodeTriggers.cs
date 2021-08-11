using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Triggers
{
    public class InfocodeTriggers : IInfocodeTriggers    
    {

        #region Constructor - Destructor

        public InfocodeTriggers(IConnectionManager entry)
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for InfocodeTriggers are reserved at 59000 - 59999
        }

        ~InfocodeTriggers()
        {

        }

        #endregion

        #region IInfocodeTriggers Members

        /// <summary>
        /// Before the infocode is processed this trigger is called. If the infocode should not be processed 
        /// after the trigger has finished running then PreTriggerResults needs to be filled out accordingly.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="posTransaction"></param> 
        /// <param name="tableRefId">What table does the infocode apply to</param>
        public void PreProcessInfocode(IConnectionManager entry, PreTriggerResults results, IPosTransaction transaction, InfoCodeLineItem.TableRefId tableRefId)
        {

        }

        /// <summary>
        /// After the infocode has been processed this trigger is called. Any checking and/or additional input validation and etc. can be done here.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="posTransaction"></param>
        /// <param name="tableRefId">What table does te infode apply to</param>
        public void PostProcessInfocode(IConnectionManager entry, PreTriggerResults results, IPosTransaction transaction, InfoCodeLineItem.TableRefId tableRefId)
        {

        }
        

        #endregion
    }
}
