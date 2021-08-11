using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class ReceiptSequence : SqlServerDataProviderBase, IReceiptSequence
    {
        public ReceiptSequence()
        {
            ReceiptNumberSequenceID = RecordIdentifier.Empty;
        }

        public virtual RecordIdentifier DefaultReceiptSequence { get { return "RECEIPTID"; } }

        public virtual RecordIdentifier ReceiptNumberSequenceID { get; set; }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RBOTRANSACTIONTABLE", "RECEIPTID", id);
        }

        public virtual RecordIdentifier SequenceID
        {
            get { return ReceiptNumberSequenceID == RecordIdentifier.Empty ? DefaultReceiptSequence : ReceiptNumberSequenceID; }
        }

        public virtual string CreateNewID(IConnectionManager connection)
        {
            return (string)DataProviderFactory.Instance.GenerateNumber(connection, this);
        }

        public string GetLastReceiptID(IConnectionManager entry, RecordIdentifier terminalID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT TOP(1) RECEIPTID
                      from RBOTRANSACTIONTABLE
                      where RECEIPTID <> ''
                      and TERMINAL = @terminalID
                      order by RECEIPTID desc";

                MakeParam(cmd, "terminalID", terminalID);

                object result = entry.Connection.ExecuteScalar(cmd);

                return result != null ? (string) result : string.Empty;
            }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOTRANSACTIONTABLE", "RECEIPTID", sequenceFormat, startingRecord, numberOfRecords);
        }
    }
}
