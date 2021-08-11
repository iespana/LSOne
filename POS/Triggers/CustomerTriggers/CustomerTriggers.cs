using System;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using LSOne.Services.Interfaces.SupportClasses;

namespace LSOne.Triggers
{
    public class CustomerTriggers : ICustomerTriggers
    {

        #region Constructor - Destructor

        public CustomerTriggers(IConnectionManager entry)
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for CustomerTriggers are reserved at 65000 - 65999
        }

        ~CustomerTriggers()
        {

        }

        #endregion

        #region ICustomerTriggers Members

        public void PreCustomerClear(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Prior to clearing a customer from the transaction.", "ICustomerTriggers.PreCustomerClear");
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PostCustomerClear(IConnectionManager entry, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After clearing a customer from the transaction.", "ICustomerTriggers.PostCustomerClear");
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
