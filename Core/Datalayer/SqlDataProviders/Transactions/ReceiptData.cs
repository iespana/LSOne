using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Transactions
{
    public class ReceiptData : SqlServerDataProviderBase, IReceiptData
    {
        private static void PopulateReceiptListItem(IDataReader dr, ReceiptListItem item)
        {
            item.ID = (string)dr["TRANSACTIONID"];
            item.Text = (string)dr["TRANSACTIONID"];
            
            item.ReceiptID = (string)dr["RECEIPTID"];
            item.TransactionDate = new Date(dr["TRANSDATE"]);

            item.StoreID = (string)dr["STORE"];
            item.TerminalID = (string)dr["TERMINAL"];
            item.EmployeeID = (string)dr["STAFF"];
            item.EmployeeLogin = (string)dr["STAFFLOGIN"];
            item.PaidAmount = (decimal)dr["PAYMENTAMOUNT"];
            item.Currency = (string)dr["CURRENCY"];

            item.StoreDescription = (string)dr["StoreName"];
            item.TerminalDescription = (string)dr["TerminalName"];
            item.EmployeeDescription = (string)dr["StaffName"];
        }

        public List<ReceiptListItem> Find(IConnectionManager entry, Date dateFrom, Date dateTo, string idOrReceiptNumber, bool idOrReceiptNumberBeginsWith, RecordIdentifier employeeLogin, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier currency, decimal paidAmount, int recordFrom, int recordTo, string sort, out int totalReceiptsMatching)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            using (var cmdCount = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM (" +
                                  "SELECT TRANSACTIONID, " +
                                  "RECEIPTID, " +
                                  "TRANSDATE, " +
                                  "STORE, " +
                                  "TERMINAL, " +
                                  "STAFF, " +
                                  "PAYMENTAMOUNT, " +
                                  "CURRENCY, " +
                                  "StoreName, " +
                                  "TerminalName, " +
                                  "StaffName, " +
                                  "STAFFLOGIN, " +
                                  "ROW_NUMBER() over(order by " +sort+") as rownum FROM ( " +
                                  "Select t.TRANSACTIONID,ISNULL(t.RECEIPTID,'') as RECEIPTID,t.TRANSDATE," +
                                  "ISNULL(t.STORE,'') as STORE, ISNULL(t.TERMINAL,'') as TERMINAL,ISNULL(t.STAFF,'') as STAFF," +
                                  "ISNULL(term.NAME,'') as TerminalName,ISNULL(store.NAME,'') as StoreName,ISNULL(staff.Name,'') as StaffName, ISNULL(u.Login,'') as STAFFLOGIN, " +
                                  "ISNULL(t.PAYMENTAMOUNT, 0) + ISNULL(t.MARKUPAMOUNT, 0) as PAYMENTAMOUNT, t.CURRENCY " +
                                  "from RBOTRANSACTIONTABLE t " +
                                  "left outer join RBOTERMINALTABLE term on t.TERMINAL = term.TerminalID and t.STORE = term.STOREID and term.DATAAREAID = t.DATAAREAID " +
                                  "left outer join RBOSTORETABLE store on t.STORE = store.STOREID and store.DATAAREAID = t.DATAAREAID " +
                                  "left outer join RBOSTAFFTABLE staff on t.STAFF = staff.STAFFID and staff.DATAAREAID = staff.DATAAREAID " +
                                  "left outer join USERS u on t.STAFF = u.STAFFID " +
                                  "where t.DATAAREAID = @dataAreaId and EntryStatus = 0 and Type = 2";

                cmdCount.CommandText = @"select count(*) from RBOTRANSACTIONTABLE t " +
                                        "left outer join USERS u on t.STAFF = u.STAFFID " +
                                        "where t.DATAAREAID = @dataAreaId and EntryStatus = 0 and Type = 2";

                if (idOrReceiptNumber != null)
                {
                    idOrReceiptNumber = PreProcessSearchText(idOrReceiptNumber, true, idOrReceiptNumberBeginsWith);
                    string where = " and ( t.RECEIPTID like @receiptID )";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "receiptID", idOrReceiptNumber);
                    MakeParam(cmdCount, "receiptID", idOrReceiptNumber);
                }

                if (dateFrom != Date.Empty)
                {
                    string where = " and t.TRANSDATE >= @dateFrom";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "dateFrom", dateFrom.DateTime, SqlDbType.DateTime);
                    MakeParam(cmdCount, "dateFrom", dateFrom.DateTime, SqlDbType.DateTime);
                }

                if (dateTo != Date.Empty)
                {
                    string where = " and t.TRANSDATE <= @dateTo";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "dateTo", dateTo.DateTime, SqlDbType.DateTime);
                    MakeParam(cmdCount, "dateTo", dateTo.DateTime, SqlDbType.DateTime);
                }

                if (employeeLogin != RecordIdentifier.Empty && employeeLogin != "")
                {
                    string where = " and u.LOGIN = @staffID";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "staffID", (string)employeeLogin);
                    MakeParam(cmdCount, "staffID", (string)employeeLogin);
                }

                if (storeID != RecordIdentifier.Empty && storeID != "")
                {
                    string where = " and t.STORE = @storeID";
                    string whereSecond = " and t.STORE = @storeID";
                    cmd.CommandText += where;
                    cmd.CommandText += whereSecond;
                    cmdCount.CommandText += where;
                    cmdCount.CommandText += whereSecond;

                    MakeParam(cmd, "storeID", (string) storeID);
                    MakeParam(cmdCount, "storeID", (string)storeID);
                }

                if (terminalID != RecordIdentifier.Empty && terminalID != "")
                {
                    string where = " and t.TERMINAL = @terminalID";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "terminalID", (string) terminalID);
                    MakeParam(cmdCount, "terminalID", (string)terminalID);
                }

                if (currency != RecordIdentifier.Empty && currency != "")
                {
                    string where = " and t.CURRENCY = @currency";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "currency", (string)currency);
                    MakeParam(cmdCount, "currency", (string)currency);
                }

                if (paidAmount != 0)
                {
                    string where = " and t.PAYMENTAMOUNT = @paidAmount";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where + "))";
                    MakeParam(cmd, "paidAmount", paidAmount);
                    MakeParam(cmdCount, "paidAmount", paidAmount);
                }

                cmd.CommandText += ") AS QUERY) AS QUERY2 WHERE rownum BETWEEN @recordFrom AND @recordTo ORDER BY rownum ";
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "recordFrom", recordFrom, SqlDbType.Int);
                MakeParam(cmd, "recordTo", recordTo, SqlDbType.Int);

                MakeParam(cmdCount, "dataAreaId", entry.Connection.DataAreaId);
                totalReceiptsMatching = (int)entry.Connection.ExecuteScalar(cmdCount);

                return Execute<ReceiptListItem>(entry, cmd, CommandType.Text, PopulateReceiptListItem);
            }
        }
    }
}