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
    public class CreditVoucherLineData : SqlServerDataProviderBase, ICreditVoucherLineData
    {
        private static void PopulateCreditVoucherLine(IDataReader dr, CreditVoucherLine creditVoucherLine)
        {
            creditVoucherLine.CreditVoucherID = (string)dr["VOUCHERID"];
            creditVoucherLine.CreditVoucherLineID = (Guid)dr["VOUCHERLINEID"];
            creditVoucherLine.StoreID = (string)dr["STOREID"];
            creditVoucherLine.TerminalID = (string)dr["TERMINALID"];
            creditVoucherLine.TransactionNumber = (string)dr["TRANSACTIONNUMBER"];
            creditVoucherLine.ReceiptID = (string)dr["RECEIPTID"];
            creditVoucherLine.StaffID = (string)dr["STAFFID"];
            creditVoucherLine.UserID = (dr["USERID"] != DBNull.Value) ? (Guid)dr["USERID"] : Guid.Empty;

            if (dr["TRANSACTIONDATE"] != DBNull.Value)
            {
                creditVoucherLine.TransactionDateTime = (DateTime)dr["TRANSACTIONDATE"];
            }

            if (dr["TRANSACTIONTIME"] != DBNull.Value)
            {
                var time = (TimeSpan)dr["TRANSACTIONTIME"];

                creditVoucherLine.TransactionDateTime = new DateTime(
                    creditVoucherLine.TransactionDateTime.Year,
                    creditVoucherLine.TransactionDateTime.Month,
                    creditVoucherLine.TransactionDateTime.Day,
                    time.Hours,
                    time.Minutes,
                    time.Seconds,
                    time.Milliseconds);
            }
            else
            {
                creditVoucherLine.TransactionDateTime = new DateTime(
                    creditVoucherLine.TransactionDateTime.Year,
                    creditVoucherLine.TransactionDateTime.Month,
                    creditVoucherLine.TransactionDateTime.Day,
                    0,
                    0,
                    0,
                    0);
            }

            creditVoucherLine.Amount = (decimal)dr["AMOUNT"];
            creditVoucherLine.Operation = (CreditVoucherLine.CreditVoucherLineEnum)(int)dr["OPERATION"];

            creditVoucherLine.UserName.First = (string)dr["FirstName"];
            creditVoucherLine.UserName.Middle = (string)dr["MiddleName"];
            creditVoucherLine.UserName.Last = (string)dr["LastName"];
            creditVoucherLine.UserName.Prefix = (string)dr["NamePrefix"];
            creditVoucherLine.UserName.Suffix = (string)dr["NameSuffix"];

            creditVoucherLine.TerminalName = (string)dr["TerminalName"];
            creditVoucherLine.StoreName = (string)dr["StoreName"];
            creditVoucherLine.StaffName = (string)dr["StaffName"];
        }

        /// <summary>
        /// Gets a credit voucher line by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="creditVoucherLineID">ID of the credit voucher line to fetch. (double identifier, VOUCHERID and VOUCHERLINEID)</param>
        /// <returns>The requested credit voucher line or null if it was not found</returns>
        public virtual CreditVoucherLine Get(IConnectionManager entry, RecordIdentifier creditVoucherLineID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT g.VOUCHERID,g.VOUCHERLINEID,ISNULL(g.STOREID,'') as STOREID,ISNULL(g.TERMINALID,'') as TERMINALID," +
                    "ISNULL(g.TRANSACTIONNUMBER,'') as TRANSACTIONNUMBER,ISNULL(g.RECEIPTID,'') as RECEIPTID,ISNULL(g.STAFFID,'') as STAFFID,g.USERID," +
                    "g.TRANSACTIONDATE,g.TRANSACTIONTIME,g.OPERATION,g.AMOUNT, " +
                    "ISNULL(u.FirstName,'') as FirstName,ISNULL(u.MiddleName,'') as MiddleName,ISNULL(u.LastName,'') as LastName," +
                    "ISNULL(u.NamePrefix,'') as NamePrefix,ISNULL(u.NameSuffix,'') as NameSuffix," +
                    "ISNULL(t.NAME,'') as TerminalName,ISNULL(s.NAME,'') as StoreName,ISNULL(st.NAME,'') as StaffName " +
                    "from RBOCREDITVOUCHERTRANSACTIONS g " +
                    "left outer join USERS u on u.GUID = g.USERID and u.DATAAREAID = g.DATAAREAID " +
                    "left outer join RBOTERMINALTABLE t on t.TERMINALID = g.TERMINALID and t.STOREID = g.STOREID and t.DATAAREAID = g.DATAAREAID " +
                    "left outer join RBOSTORETABLE s on s.STOREID = g.STOREID and s.DATAAREAID = s.DATAAREAID " +
                    "left outer join RBOSTAFFTABLE st on st.STAFFID = g.STAFFID and st.DATAAREAID = s.DATAAREAID " +
                    "where g.VOUCHERID = @creditVoucherID AND g.VOUCHERLINEID = @creditVoucherLineID and g.DATAAREAID = @dataareaid";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "creditVoucherID", (string)creditVoucherLineID.PrimaryID);
                MakeParam(cmd, "creditVoucherLineID", (string)creditVoucherLineID.SecondaryID);

                var result = Execute<CreditVoucherLine>(entry, cmd, CommandType.Text, PopulateCreditVoucherLine);
                return (result.Count > 0) ? result[0] : null;
            }
        }

        /// <summary>
        /// Gets a list of credit voucher lines for a given crdit voucher.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="creditVoucherID">ID of the credit voucher to get credit voucher lines for</param>
        /// <returns>List of credit voucher lines for the given ID</returns>
        public virtual List<CreditVoucherLine> GetList(IConnectionManager entry, RecordIdentifier creditVoucherID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT g.VOUCHERID,g.VOUCHERLINEID,ISNULL(g.STOREID,'') as STOREID,ISNULL(g.TERMINALID,'') as TERMINALID," +
                    "ISNULL(g.TRANSACTIONNUMBER,'') as TRANSACTIONNUMBER,ISNULL(g.RECEIPTID,'') as RECEIPTID,ISNULL(g.STAFFID,'') as STAFFID,g.USERID," +
                    "g.TRANSACTIONDATE,g.TRANSACTIONTIME,g.OPERATION,g.AMOUNT, " +
                    "ISNULL(u.FirstName,'') as FirstName,ISNULL(u.MiddleName,'') as MiddleName,ISNULL(u.LastName,'') as LastName," +
                    "ISNULL(u.NamePrefix,'') as NamePrefix,ISNULL(u.NameSuffix,'') as NameSuffix," +
                    "ISNULL(t.NAME,'') as TerminalName,ISNULL(s.NAME,'') as StoreName,ISNULL(st.NAME,'') as StaffName " +
                    "from RBOCREDITVOUCHERTRANSACTIONS g " +
                    "left outer join USERS u on u.GUID = g.USERID and u.DATAAREAID = g.DATAAREAID " +
                    "left outer join RBOTERMINALTABLE t on t.TERMINALID = g.TERMINALID and t.STOREID = g.STOREID and t.DATAAREAID = g.DATAAREAID " +
                    "left outer join RBOSTORETABLE s on s.STOREID = g.STOREID and s.DATAAREAID = s.DATAAREAID " +
                    "left outer join RBOSTAFFTABLE st on st.STAFFID = g.STAFFID and st.DATAAREAID = s.DATAAREAID " +
                    "where g.VOUCHERID = @creditVoucherID and g.DATAAREAID = @dataareaid";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "creditVoucherID", (string)creditVoucherID);

                return Execute<CreditVoucherLine>(entry, cmd, CommandType.Text, PopulateCreditVoucherLine);
            }
        }

        /// <summary>
        /// Returns true if a CreditVoucherLine with a given ID exists. Note that the id is double identifier taking VOUCHERID and VOUCHERLINEID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the CreditVoucherLine (double identifier, VOUCHERID and VOUCHERLINEID)</param>
        /// <returns>True if the record existed, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RBOCREDITVOUCHERTRANSACTIONS", new[] { "VOUCHERID", "VOUCHERLINEID" }, id);
        }

        /// <summary>
        /// Saves a credit voucher line, creating new one if the CreditVoucherLineID was empty or record did not exist, else updates.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="creditVoucherLine">The credit voucher line to save</param>
        public virtual void Save(IConnectionManager entry, CreditVoucherLine creditVoucherLine)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ManageCreditVouchers);

            if (creditVoucherLine.CreditVoucherLineID == RecordIdentifier.Empty)
            {
                creditVoucherLine.CreditVoucherLineID = Guid.NewGuid();
            }

            if (creditVoucherLine.CreditVoucherID == RecordIdentifier.Empty)
            {
                throw new Exception("CreditVoucherID cannot be empty");
            }

            creditVoucherLine.Validate();

            var statement = new SqlServerStatement("RBOCREDITVOUCHERTRANSACTIONS");

            bool isNew = false;
            if (creditVoucherLine.CreditVoucherLineID == RecordIdentifier.Empty)
            {
                isNew = true;
                creditVoucherLine.CreditVoucherLineID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, creditVoucherLine.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("VOUCHERID", (string) creditVoucherLine.CreditVoucherID);
                statement.AddKey("VOUCHERLINEID", (Guid) creditVoucherLine.CreditVoucherLineID,
                                 SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("VOUCHERID", (string) creditVoucherLine.CreditVoucherID);
                statement.AddKey("VOUCHERLINEID", (Guid) creditVoucherLine.CreditVoucherLineID,
                                 SqlDbType.UniqueIdentifier);
            }

            statement.AddField("STOREID", (string) creditVoucherLine.StoreID);
            statement.AddField("TERMINALID", (string) creditVoucherLine.TerminalID);
            statement.AddField("TRANSACTIONNUMBER", (string) creditVoucherLine.TransactionNumber);
            statement.AddField("RECEIPTID", (string) creditVoucherLine.ReceiptID);
            statement.AddField("STAFFID", (string) creditVoucherLine.StaffID);

            if (creditVoucherLine.UserID == RecordIdentifier.Empty)
            {
                statement.AddField("USERID", DBNull.Value, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.AddField("USERID", (Guid) creditVoucherLine.UserID, SqlDbType.UniqueIdentifier);
            }
            statement.AddField("TRANSACTIONDATE", creditVoucherLine.TransactionDateTime.Date, SqlDbType.Date);
            statement.AddField("TRANSACTIONTIME", creditVoucherLine.TransactionDateTime.TimeOfDay, SqlDbType.Time);
            statement.AddField("AMOUNT", creditVoucherLine.Amount, SqlDbType.Decimal);
            statement.AddField("OPERATION", (int) creditVoucherLine.Operation, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
