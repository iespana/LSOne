using System;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Services.Interfaces.SupportClasses;

namespace LSOne.Triggers
{
    public class ApplicationTriggers : IApplicationTriggers
    {
        #region IApplicationTriggers Members

        public ApplicationTriggers(IConnectionManager entry)
        {

        }

        public void ApplicationStart(IConnectionManager entry)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Application has started", "IApplicationTriggers.ApplicationStart");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void ApplicationStop()
        {
            // Note that no database connection exists at this point.
        }

        public void PostLogon(IConnectionManager entry, bool loginSuccessful, string operatorId, string name)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After the user has logged on...", "IApplicationTriggers.PostLogon");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PreLogon(IConnectionManager entry, PreTriggerResults results, string operatorId, string name)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Before the user has been logged on...", "IApplicationTriggers.PreLogon");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void Logoff(IConnectionManager entry, string operatorId, string name)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After the user has logged off...", "IApplicationTriggers.Logoff");
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void LoginWindowVisible(IConnectionManager entry)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "When the login window is visible", "IApplicationTriggers.LoginWindowVisible");
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public bool AuthorizeExternalManagerAccess(IConnectionManager entry, POSOperations operation, string operatorId, string password, ref bool passwordMatches)
        {
            bool externalLogonIsUsed = false;

            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Logon executed", "IApplicationTriggers.Logon");

                //validate Username and Password. If this way of evaluation has been used, then it is important to return "true".
                //otherwise, the POS will use the internal way of userVerification

                //the value for "passMatches" should reflect whether or not the Logon-attempt has been successful.

                return externalLogonIsUsed;
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public void PreOperationRun(IConnectionManager entry, PreTriggerResults results, POSOperations operation, OperationInfo operationInfo, IPosTransaction posTransaction)
        {
        }

        public void PostOperationRun(IConnectionManager entry, POSOperations operation, OperationInfo operationInfo, IPosTransaction posTransaction)
        {
        }        

        #endregion
    }
}
