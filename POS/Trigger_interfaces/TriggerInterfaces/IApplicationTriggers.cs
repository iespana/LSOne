using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Triggers
{
    public interface IApplicationTriggers
    {
        #region Application triggers

        /// <summary>
        /// Triggers once, whenever the application starts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        void ApplicationStart(IConnectionManager entry);

        /// <summary>
        /// Triggers once, just before the application stops. Note that no database connection is valid at this point
        /// </summary>
        void ApplicationStop();

        /// <summary>
        /// Triggers just before the login operation is about to be run
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        void PreLogon(IConnectionManager entry, PreTriggerResults results, string operatorId, string name);

        /// <summary>
        /// Triggers after the login operation has been executed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        void PostLogon(IConnectionManager entry, bool loginSuccessful, string operatorId, string name);

        /// <summary>
        /// Triggers when logoff operation is run
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        void Logoff(IConnectionManager entry, string operatorId, string name);

        /// <summary>
        /// Triggers when the login window gets visible
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        void LoginWindowVisible(IConnectionManager entry);

        /// <summary>
        /// Can be used to externally verify the access rights in the case where the logged on user rights do not suffice.
        /// That way operatorId and password can be encrypted by the implementer as demanded.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="operatorId">The operator Id as specified by the user.</param>
        /// <param name="password">The password as specified by the user matching the operatorId.</param>
        /// <param name="passwordMatches">Reflects whether Operator Id and given password match. If so, then logon is granted.</param>
        /// <returns>True, if the external method was used. Wrong to continue using the previous (internal) POS handling.</returns>
        bool AuthorizeExternalManagerAccess(IConnectionManager entry, POSOperations operation, string operatorId, string password, ref bool passwordMatches);

        /// <summary>
        /// Triggers just before an operation is about to be run
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        void PreOperationRun(IConnectionManager entry, PreTriggerResults results, POSOperations operation, OperationInfo operationInfo, IPosTransaction posTransaction);

        /// <summary>
        /// Triggers just after an operation has been run
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        void PostOperationRun(IConnectionManager entry, POSOperations operation, OperationInfo operationInfo, IPosTransaction posTransaction);
        
        #endregion
    }
}
