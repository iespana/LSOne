using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Transactions
{
    public interface ISuspendedTransactionAnswerData : IDataProvider<SuspendedTransactionAnswer>
    {
        List<SuspendedTransactionAnswer> GetList(
            IConnectionManager entry,
            RecordIdentifier transactionID);

        /// <summary>
        /// Gets all answers for a given suspension type.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="suspensionTypeID">The suspension type ID</param>
        /// <returns></returns>
        List<SuspendedTransactionAnswer> GetListForSuspensionType(IConnectionManager entry, RecordIdentifier suspensionTypeID);

        void DeleteForTransaction(IConnectionManager entry, RecordIdentifier transactionID);
    }
}