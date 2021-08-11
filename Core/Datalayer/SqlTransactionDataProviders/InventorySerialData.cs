using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class InventorySerialData : SqlServerDataProviderBase, IInventorySerialData
    {
        private static void PopulateInventorySerial(IDataReader dr, InventorySerial serial)
        {
            serial.ID = (string) dr["INVENTSERIALID"];
            serial.Text = (string) dr["DESCRIPTION"];
            serial.ItemID = (string) dr["ITEMID"];
            serial.ProductionDate = (DateTime) dr["PRODDATE"];
            serial.RFIDTag = (string) dr["RFIDTAGID"];
        }

        /// <summary>
        /// Gets a list for the itemID and RFIDTagID, the itemID is mandatory however the RFIDTagID is not
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemIDAndRFID">List constraints</param>
        /// <returns></returns>
        public virtual List<InventorySerial> Get(IConnectionManager entry, RecordIdentifier itemIDAndRFID)
        {
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT INVENTSERIALID, ISNULL(RFIDTAGID, '') AS RFIDTAGID, ITEMID,
                                    ISNULL(PRODDATE, '1900-01-01') AS PRODDATE, ISNULL(DESCRIPTION, '') AS DESCRIPTION 
                                   FROM INVENTSERIAL WHERE DATAAREAID = @dataAreaID AND ITEMID = @itemID";
                if (itemIDAndRFID.HasSecondaryID)
                {
                    cmd.CommandText += @" AND RFIDTAGID = @RFIDTagID";
                    MakeParam(cmd, "RFIDTagID", itemIDAndRFID.SecondaryID);
                }
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", itemIDAndRFID.PrimaryID);
                return Execute<InventorySerial>(entry, cmd, CommandType.Text, PopulateInventorySerial);
            }     
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier inventorySerialID)
        {
            return RecordExists(entry, "INVENTSERIAL", "INVENTSERIALID", inventorySerialID);
        }

        public virtual void Save(IConnectionManager entry, InventorySerial serial)
        {
            ValidateSecurity(entry);
            var statement = new SqlServerStatement("INVENTSERIAL");

            if(Exists(entry, serial.ID))
            {
                statement.AddCondition("INVENTSERIALID", (string)serial.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ITEMID", (string)serial.ItemID);
            }
            else
            {
                statement.AddKey("INVENTSERIALID", (string)serial.ID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ITEMID", (string)serial.ItemID);
            }
            statement.AddField("PRODDATE", serial.ProductionDate, SqlDbType.DateTime);
            statement.AddField("DESCRIPTION", serial.Text, SqlDbType.Text);
            statement.AddField("RFIDTAGID", (string)serial.RFIDTag);
            entry.Connection.ExecuteStatement(statement);
        }
    }
}
