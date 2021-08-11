using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DataProviders.Vouchers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Vouchers
{
    public class GiftCardLineData : SqlServerDataProviderBase, IGiftCardLineData
    {
        private static void PopulateGiftCardLine(IDataReader dr, GiftCardLine giftCardLine)
        {
            giftCardLine.GiftCardID = (string)dr["GIFTCARDID"];
            giftCardLine.GiftCardLineID = (Guid)dr["GIFTCARDLINEID"];
            giftCardLine.StoreID = (string)dr["STOREID"];
            giftCardLine.TerminalID = (string)dr["TERMINALID"];
            giftCardLine.TransactionID = (string)dr["TRANSACTIONNUMBER"];
            giftCardLine.ReceiptID = (string)dr["RECEIPTID"];
            giftCardLine.StaffID = (string)dr["STAFFID"];
            giftCardLine.UserID = (dr["USERID"] != DBNull.Value) ? (Guid)dr["USERID"] : Guid.Empty;

            if(dr["TRANSACTIONDATE"] != DBNull.Value)
            {
                giftCardLine.TransactionDateTime = (DateTime)dr["TRANSACTIONDATE"];
            }

            if (dr["TRANSACTIONTIME"] != DBNull.Value)
            {
                var time = (TimeSpan)dr["TRANSACTIONTIME"];

                giftCardLine.TransactionDateTime = new DateTime(
                    giftCardLine.TransactionDateTime.Year,
                    giftCardLine.TransactionDateTime.Month,
                    giftCardLine.TransactionDateTime.Day,
                    time.Hours,
                    time.Minutes,
                    time.Seconds,
                    time.Milliseconds);
            }
            else
            {
                giftCardLine.TransactionDateTime = new DateTime(
                    giftCardLine.TransactionDateTime.Year,
                    giftCardLine.TransactionDateTime.Month,
                    giftCardLine.TransactionDateTime.Day,
                    0,
                    0,
                    0,
                    0);
            }

            giftCardLine.Amount = (decimal)dr["AMOUNT"];
            giftCardLine.Operation = (GiftCardLine.GiftCardLineEnum)(int)dr["OPERATION"];

            giftCardLine.UserName.First = (string)dr["FirstName"];
            giftCardLine.UserName.Middle = (string)dr["MiddleName"];
            giftCardLine.UserName.Last = (string)dr["LastName"];
            giftCardLine.UserName.Prefix = (string)dr["NamePrefix"];
            giftCardLine.UserName.Suffix = (string)dr["NameSuffix"];

            giftCardLine.TerminalName = (string)dr["TerminalName"];
            giftCardLine.StoreName = (string)dr["StoreName"];
            giftCardLine.StaffName = (string)dr["StaffName"];
        }

        /// <summary>
        /// Gets a gift card line by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardLineID">ID of the gift card line to fetch. (double identifier, GIFTCARDID and GIFTCARDLINEID)</param>
        /// <returns>The requested gift card line or null if it was not found</returns>
        public virtual GiftCardLine Get(IConnectionManager entry, RecordIdentifier giftCardLineID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT g.GIFTCARDID,g.GIFTCARDLINEID,ISNULL(g.STOREID,'') as STOREID,ISNULL(g.TERMINALID,'') as TERMINALID," +
                    "ISNULL(g.TRANSACTIONNUMBER,'') as TRANSACTIONNUMBER,ISNULL(g.RECEIPTID,'') as RECEIPTID,ISNULL(g.STAFFID,'') as STAFFID,g.USERID," +
                    "g.TRANSACTIONDATE,g.TRANSACTIONTIME,g.OPERATION,g.AMOUNT, " +
                    "ISNULL(u.FirstName,'') as FirstName,ISNULL(u.MiddleName,'') as MiddleName,ISNULL(u.LastName,'') as LastName,"+
                    "ISNULL(u.NamePrefix,'') as NamePrefix,ISNULL(u.NameSuffix,'') as NameSuffix," +
                    "ISNULL(t.NAME,'') as TerminalName,ISNULL(s.NAME,'') as StoreName,ISNULL(st.NAME,'') as StaffName " +
                    "from RBOGIFTCARDTRANSACTIONS g " +
                    "left outer join USERS u on u.GUID = g.USERID and u.DATAAREAID = g.DATAAREAID " +
                    "left outer join RBOTERMINALTABLE t on t.TERMINALID = g.TERMINALID and t.STOREID = g.STOREID and t.DATAAREAID = g.DATAAREAID " +
                    "left outer join RBOSTORETABLE s on s.STOREID = g.STOREID and s.DATAAREAID = s.DATAAREAID " +
                    "left outer join RBOSTAFFTABLE st on st.STAFFID = g.STAFFID and st.DATAAREAID = s.DATAAREAID " +
                    "where g.GIFTCARDID = @giftCardID AND g.GIFTCARDLINEID = @giftCardLineID and g.DATAAREAID = @dataareaid";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "giftCardID", (string)giftCardLineID.PrimaryID);
                MakeParam(cmd, "giftCardLineID", (string)giftCardLineID.SecondaryID);

                var result = Execute<GiftCardLine>(entry, cmd, CommandType.Text, PopulateGiftCardLine);
                return (result.Count > 0) ? result[0] : null;
            }
        }

        /// <summary>
        /// Gets a list of gift card lines for a given gift card.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardID">ID of the gift card to get gift card lines for</param>
        /// <returns>List of gift card lines for the given ID</returns>
        public virtual List<GiftCardLine> GetList(IConnectionManager entry, RecordIdentifier giftCardID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT g.GIFTCARDID,g.GIFTCARDLINEID,ISNULL(g.STOREID,'') as STOREID,ISNULL(g.TERMINALID,'') as TERMINALID," +
                    "ISNULL(g.TRANSACTIONNUMBER,'') as TRANSACTIONNUMBER,ISNULL(g.RECEIPTID,'') as RECEIPTID,ISNULL(g.STAFFID,'') as STAFFID,g.USERID," +
                    "g.TRANSACTIONDATE,g.TRANSACTIONTIME,g.OPERATION,g.AMOUNT, " +
                    "ISNULL(u.FirstName,'') as FirstName,ISNULL(u.MiddleName,'') as MiddleName,ISNULL(u.LastName,'') as LastName," +
                    "ISNULL(u.NamePrefix,'') as NamePrefix,ISNULL(u.NameSuffix,'') as NameSuffix," +
                    "ISNULL(t.NAME,'') as TerminalName,ISNULL(s.NAME,'') as StoreName,ISNULL(st.NAME,'') as StaffName " +
                    "from RBOGIFTCARDTRANSACTIONS g " +
                    "left outer join USERS u on u.GUID = g.USERID and u.DATAAREAID = g.DATAAREAID " +
                    "left outer join RBOTERMINALTABLE t on t.TERMINALID = g.TERMINALID and t.STOREID = g.STOREID and t.DATAAREAID = g.DATAAREAID " +
                    "left outer join RBOSTORETABLE s on s.STOREID = g.STOREID and s.DATAAREAID = s.DATAAREAID " +
                    "left outer join RBOSTAFFTABLE st on st.STAFFID = g.STAFFID and st.DATAAREAID = s.DATAAREAID " +
                    "where g.GIFTCARDID = @giftCardID and g.DATAAREAID = @dataareaid " +
                    "order by g.TRANSACTIONDATE, g.TRANSACTIONTIME";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "giftCardID", (string)giftCardID);
 
                return Execute<GiftCardLine>(entry, cmd, CommandType.Text, PopulateGiftCardLine);
            }
        }

        /// <summary>
        /// Returns true if a GiftCardLine with a given ID exists. Note that the id is double identifier taking GIFTCARDID and GIFTCARDLINEID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the GiftCardLine (double identifier, GIFTCARDID and GIFTCARDLINEID)</param>
        /// <returns>True if the record existed, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RBOGIFTCARDTRANSACTIONS", new[]{"GIFTCARDID","GIFTCARDLINEID"}, id);
        }

        /// <summary>
        /// Saves a gift card line, creating new one if the GiftCardLineID was empty or record did not exist, else updates.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardLine">The gift card line to save</param>
        public virtual void Save(IConnectionManager entry, GiftCardLine giftCardLine)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ManageGiftCards);

            if (giftCardLine.GiftCardLineID == RecordIdentifier.Empty)
            {
                giftCardLine.GiftCardLineID = Guid.NewGuid();
            }

            giftCardLine.Validate();

            var statement = new SqlServerStatement("RBOGIFTCARDTRANSACTIONS");

            var isNew = false;
            if (giftCardLine.GiftCardLineID == RecordIdentifier.Empty)
            {
                isNew = true;
                giftCardLine.GiftCardLineID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, giftCardLine.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("GIFTCARDID", (string) giftCardLine.GiftCardID);
                statement.AddKey("GIFTCARDLINEID", (Guid) giftCardLine.GiftCardLineID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("GIFTCARDID", (string) giftCardLine.GiftCardID);
                statement.AddCondition("GIFTCARDLINEID", (Guid)giftCardLine.GiftCardLineID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("STOREID", (string) giftCardLine.StoreID);
            statement.AddField("TERMINALID", (string) giftCardLine.TerminalID);
            statement.AddField("TRANSACTIONNUMBER", (string) giftCardLine.TransactionID);
            statement.AddField("RECEIPTID", (string) giftCardLine.ReceiptID);
            statement.AddField("STAFFID", (string) giftCardLine.StaffID);

            if (giftCardLine.UserID == RecordIdentifier.Empty)
            {
                statement.AddField("USERID", DBNull.Value, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.AddField("USERID", (Guid) giftCardLine.UserID, SqlDbType.UniqueIdentifier);
            }
            statement.AddField("TRANSACTIONDATE", giftCardLine.TransactionDateTime.Date, SqlDbType.Date);
            statement.AddField("TRANSACTIONTIME", giftCardLine.TransactionDateTime.TimeOfDay, SqlDbType.Time);
            statement.AddField("AMOUNT", giftCardLine.Amount, SqlDbType.Decimal);
            statement.AddField("OPERATION", (int) giftCardLine.Operation, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
