using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IReceiptSequence : IDataProviderBase<DataEntity>, ISequenceable
    {
        RecordIdentifier DefaultReceiptSequence { get; }

        RecordIdentifier ReceiptNumberSequenceID { get; set; }

        string CreateNewID(IConnectionManager connection);

        /// <summary>
        /// Returns the last issued RECEIPTID for a transaction for the given terminal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="terminalID">The ID of the terminal</param>
        /// <returns></returns>
        string GetLastReceiptID(IConnectionManager entry, RecordIdentifier terminalID);
    }
}