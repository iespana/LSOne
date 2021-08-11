using System;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using LSOne.Services.Interfaces.SupportClasses;

namespace LSOne.Triggers
{
    public class TransactionTriggers : ITransactionTriggers
    {

        #region Constructor - Destructor

        public TransactionTriggers(IConnectionManager entry)
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for TransactionTriggers are reserved at 50300 - 50349
        }

        ~TransactionTriggers()
        {

        }

        #endregion

        #region ITransactionTriggers Members

        public void BeginTransaction(IConnectionManager entry, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "When starting the transaction...", "TransactionTriggers.BeginTransaction");


            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PreEndTransaction(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "When concluding the transaction, prior to printing and saving...", "TransactionTriggers.PreEndTransaction");
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PostEndTransaction(IConnectionManager entry, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "When concluding the transaction, after printing and saving", "TransactionTriggers.PostEndTransaction");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PreVoidTransaction(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Before voiding the transaction...", "TransactionTriggers.PreVoidTransaction");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PostVoidTransaction(IConnectionManager entry, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After voiding the transaction...", "TransactionTriggers.PostVoidTransaction");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PreReturnTransaction(IConnectionManager entry, PreTriggerResults results, IRetailTransaction originalTransaction, IPosTransaction posTransaction) 
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Before returning the transaction...", "TransactionTriggers.PreReturnTransaction");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PostReturnTransaction(IConnectionManager entry, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After returning the transaction...", "TransactionTriggers.PostReturnTransaction");
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
