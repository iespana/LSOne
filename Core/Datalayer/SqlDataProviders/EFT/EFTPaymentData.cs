using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.DataProviders.EFT;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.SqlDataProviders.EFT
{
    public class EFTPaymentData : SqlServerDataProviderBase, IEFTPaymentData
    {
        public RecordIdentifier SequenceID => "EFTPAYMENT";

        public void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            DeleteRecord(entry, "RBOTRANSACTIONEFTEXTRAINFO", "PAYMENTID", ID, "");
        }

        public bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            return RecordExists(entry, "RBOTRANSACTIONEFTEXTRAINFO", "PAYMENTID", ID, false);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOTRANSACTIONEFTEXTRAINFO", "PAYMENTID", sequenceFormat, startingRecord, numberOfRecords);
        }

        public void Save(IConnectionManager entry, EFTPayment item)
        {
            throw new NotImplementedException("The payment ID is saved in the EFT extra info tables when concluding transactions.");
        }

        public bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }
    }
}
