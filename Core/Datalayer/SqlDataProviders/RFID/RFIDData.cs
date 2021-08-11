using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.DataProviders.RFID;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.RFID
{
    public class RFIDData : SqlServerDataProviderBase, IRFIDData
    {
        private static void PopulateRFID(IDataReader dr, BusinessObjects.RFID.RFID rfid)
        {
            rfid.ID = (string)dr["RFID"];
            rfid.ScannedTime = (DateTime)dr["SCANNEDTIME"];
            rfid.TransactionID = (string)dr["TRANSACTIONID"];
        }

        private static void PopulateRFIDSerial(IDataReader dr, BusinessObjects.RFID.RFID rfid)
        {
            rfid.ID = (string)dr["RFID"];
            rfid.ItemID = (string) dr["ITEMID"];
            rfid.TransactionID = dr["TRANSACTIONID"] is DBNull ? null : (string)dr["TRANSACTIONID"];
        }

        public virtual BusinessObjects.RFID.RFID Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT RFID, ISNULL(TRANSACTIONID, '') AS TRANSACTIONID, SCANNEDTIME
                                    FROM POSISRFIDTABLE WHERE RFID = @rfid AND DATAAREAID = @dataAreaID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "rfid", id);
                return Get<BusinessObjects.RFID.RFID>(entry, cmd, id, PopulateRFID, cache, UsageIntentEnum.Normal);
            }
        }

        public virtual List<BusinessObjects.RFID.RFID> GetSerialList(IConnectionManager entry)
        {
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @" SELECT R.RFID, ISNULL(S.ITEMID, '') AS ITEMID FROM POSISRFIDTABLE R 
                                     LEFT JOIN INVENTSERIAL S ON S.RFIDTAGID = R.RFID 
                                     WHERE TRANSACTIONID IS NULL AND DATAAREAID = @dataAreaID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                return Execute<BusinessObjects.RFID.RFID>(entry, cmd, CommandType.Text, PopulateRFIDSerial);
            }     
        }

        public virtual RecordIdentifier GetItemId(IConnectionManager entry, RecordIdentifier rfid)
        {
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ITEMID FROM INVENTSERIAL WHERE RFIDTAGID = @rfid AND DATAAREAID = @dataAreaID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "rfid", rfid);
                return (string)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<BusinessObjects.RFID.RFID>(entry, "POSISRFIDTABLE", "RFID", id);
        }

        public virtual void Delete(IConnectionManager entry, BusinessObjects.RFID.RFID rfid)
        {
            ValidateSecurity(entry);
            DeleteRecord<BusinessObjects.RFID.RFID>(entry, "POSISRFIDTABLE", "RFID", rfid.ID, "");
        }

        public virtual void PurgeRFIDTable(IConnectionManager entry)
        {
            ValidateSecurity(entry);
            var statement = new SqlServerStatement("POSISRFIDTABLE") {StatementType = StatementType.Delete};
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void Save(IConnectionManager entry, BusinessObjects.RFID.RFID rfid, bool includeScannedTime)
        {
            ValidateSecurity(entry);
            rfid.Validate();
            var statement = new SqlServerStatement("POSISRFIDTABLE");
            if (Exists(entry, rfid.ID))
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("RFID", (string)rfid.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("RFID", (string)rfid.ID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            statement.AddField("TRANSACTIONID", (string)rfid.TransactionID);
            if (includeScannedTime)
            {
                statement.AddField("SCANNEDTIME", rfid.ScannedTime, SqlDbType.DateTime);
            }

            Save(entry, rfid, statement);
        }
    }
}
