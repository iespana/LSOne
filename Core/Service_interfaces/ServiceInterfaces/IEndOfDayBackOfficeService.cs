using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public interface IEndOfDayBackOfficeService : IService
    {
        /// <summary>
        /// Returns a list of flags appropriate for the given statement 
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="statement">The statement</param>
        List<AllowEODEnums> AllowedToPostStatement(IConnectionManager entry, StatementInfo statement);

        /// <summary>
        /// Calculate statement lines for the given statement parameters. This will create statement lines.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="statementID">ID of the statement</param>
        /// <param name="storeID">The store we are creating the statement on</param>
        /// <param name="startTime">Starting time of the statement. Transactions between startTime and endTime are considered</param>
        /// <param name="endTime">Ending time of the statement. Transactions between startTime and endTime are considered</param>
        void CalculateStatement(
            IConnectionManager entry, 
            RecordIdentifier statementID, 
            RecordIdentifier storeID, 
            DateTime startTime, 
            DateTime endTime);

        /// <summary>
        /// Posts the selected statement
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="statementID">ID of the statement to post</param>
        /// <param name="storeID">ID of the store we are posting in</param>
        /// <param name="postingDate">Date of posting</param>
        void PostStatement(
            IConnectionManager entry,
            RecordIdentifier statementID,
            RecordIdentifier storeID,
            DateTime postingDate);

    }
}
