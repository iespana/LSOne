using System;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using LSOne.Services.Interfaces.SupportClasses;

namespace LSOne.Triggers
{
    public class CashManagementTriggers : ICashManagementTriggers
    {

        #region Constructor - Destructor

        public CashManagementTriggers(IConnectionManager entry)
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for CashManagementTriggers are reserved at 63000 - 63999

        }

        ~CashManagementTriggers()
        {

        }

        #endregion

        #region ICashManagementTriggers Members

        public void PreTenderDeclaration(IConnectionManager entry, PreTriggerResults results, IPosTransaction transction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Before running the Tender Declaration operation...", "CashManagementTriggers.PreTenderDeclaration");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PostTenderDeclaration(IConnectionManager entry, IPosTransaction transction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After running the Tender Declaration operation...", "CashManagementTriggers.PostTenderDeclaration");

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
