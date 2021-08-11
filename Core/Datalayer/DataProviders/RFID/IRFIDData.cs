using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.RFID
{
    public interface IRFIDData : IDataProviderBase<BusinessObjects.RFID.RFID>
    {
        List<BusinessObjects.RFID.RFID> GetSerialList(IConnectionManager entry);
        RecordIdentifier GetItemId(IConnectionManager entry, RecordIdentifier rfid);
        void Delete(IConnectionManager entry, BusinessObjects.RFID.RFID rfid);
        void PurgeRFIDTable(IConnectionManager entry);
        void Save(IConnectionManager entry, BusinessObjects.RFID.RFID rfid, bool includeScannedTime);
    }
}