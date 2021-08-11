using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSRetail.SiteService.IntegrationFrameworkStatementsInterface
{
    //TODO: SessionMode should be required. Changed to Allowed for http binding
    [ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "LSRetail.IntegrationFramework")]
    public partial interface IIntegrationFrameworkStatementsService
    {
        [OperationContract]
        bool Ping();

        /// <summary>
        /// Create EOD statements for all stores
        /// </summary>
        /// <param name="allowPostingWithoutEodTransaction">True if it is allowed to create EOD statements if any terminal does not have an EOD transaction</param>
        /// <returns>A list of created statements from all stores</returns>
        [OperationContract]
        List<StatementInfo> CreateStatements(bool allowPostingWithoutEodTransaction);

        /// <summary>
        /// Create EOD statement for a store
        /// </summary>
        /// <param name="storeID">The store ID for which to create EOD statements</param>
        /// <param name="allowPostingWithoutEodTransaction">True if it is allowed to create EOD statements if any terminal does not have an EOD transaction</param>
        /// <returns>The created statement for the store</returns>
        [OperationContract]
        StatementInfo CreateStatement(RecordIdentifier storeID, bool allowPostingWithoutEodTransaction);

        /// <summary>
        /// Get all statements created after a specified date 
        /// </summary>
        /// <param name="date">The date from which to get the statements</param>
        /// <returns>A list of statements</returns>
        [OperationContract]
        List<StatementInfo> GetStatements(DateTime date);

        /// <summary>
        /// Get all statements created after a specified date, for a store
        /// </summary>
        /// <param name="date">The date from which to get the statements</param>
        /// <param name="storeID">The store ID from which to get the statements</param>
        /// <returns>A list of statements</returns>
        [OperationContract]
        List<StatementInfo> GetStatementsForStore(DateTime date, RecordIdentifier storeID);
    }
}
