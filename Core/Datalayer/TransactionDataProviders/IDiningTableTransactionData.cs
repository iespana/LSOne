using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IDiningTableTransactionData : IDataProviderBase<DiningTableTransaction>
    {
        bool Exists(IConnectionManager entry, RecordIdentifier diningTableTransactionID);

        /// <summary>
        /// Saves the dining table transaction into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The <see cref="DiningTableTransaction"/> business object to save</param>
        void Save(IConnectionManager entry, DiningTableTransaction transaction);
    }
}