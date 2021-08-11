using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Fiscal;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.SqlDataProviders.Fiscal
{
    public partial class FiscalData
    {
        #region Norwegian localisation
        
        public List<FiscalTrans> GetList(IConnectionManager entry)
        {
            List<FiscalTrans> fiscalTrans = new List<FiscalTrans>();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                          @"Select
	                            RECEIPTID, TRANSACTIONID,  STORE,
                                TERMINAL,  FISCALUNITID, FISCALCONTROLID,
                                TYPE, TRANSDATE, GROSSAMOUNT, NETAMOUNT,
                                PRIVATEKEYVER, SIGNATURE
                            From RBOTRANSACTIONFISCALTRANS";

                fiscalTrans = Execute<FiscalTrans>(entry, cmd, CommandType.Text, PopulateFiscalTrans);
            }

            return fiscalTrans;
        }

        public FiscalTrans Get(IConnectionManager entry, RecordIdentifier id)
        {
            FiscalTrans fiscalTrans = new FiscalTrans();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                          @"Select
	                            RECEIPTID, TRANSACTIONID,  STORE,
                                TERMINAL,  FISCALUNITID, FISCALCONTROLID,
                                TYPE, TRANSDATE, GROSSAMOUNT, NETAMOUNT,
                                PRIVATEKEYVER, SIGNATURE,
                                '' AS STAFF, '' AS CURRENCY, '' AS StoreName, '' AS TerminalName,
                                '' AS StaffName
                            From RBOTRANSACTIONFISCALTRANS
                            Where RECEIPTID = @receipt and STORE = @store and TERMINAL = @terminal and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "receipt", id[0]);
                MakeParam(cmd, "store", id[1]);
                MakeParam(cmd, "terminal", id[2]);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var result = Execute<FiscalTrans>(entry, cmd, CommandType.Text, PopulateFiscalTrans);
                fiscalTrans = (result.Count > 0) ? result[0] : null;
            }

            return fiscalTrans;
        }

        

        private string EncodeBase64(string inString)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            string outString = System.Convert.ToBase64String(encoder.GetBytes(inString));

            return outString;
        }

        private string DecodeBase64(string inString)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            string outString = encoder.GetString(System.Convert.FromBase64String(inString));

            return outString;
        }

        private static void PopulateLastSignature(IDataReader dr, FiscalTrans fiscalTrans)
        {
            fiscalTrans.Signature = (string)dr["SIGNATURE"];
        }

        private static void PopulateFiscalTrans(IDataReader dr, FiscalTrans fiscalTrans)
        {
            fiscalTrans.ReceiptId = (string)dr["RECEIPTID"];
            fiscalTrans.TransactionId = (string)dr["TRANSACTIONID"];
            fiscalTrans.Store = (string)dr["STORE"];
            fiscalTrans.Terminal = (string)dr["TERMINAL"];
            fiscalTrans.FiscalUnitId = (string)dr["FISCALUNITID"];
            fiscalTrans.FiscalControlId = (string)dr["FISCALCONTROLID"];
            fiscalTrans.Type = (int)dr["TYPE"];
            fiscalTrans.TransDate = (DateTime)dr["TRANSDATE"];
            fiscalTrans.GrossAmount = (decimal)dr["GROSSAMOUNT"];
            fiscalTrans.NetAmount = (decimal)dr["NETAMOUNT"];
            fiscalTrans.PrivateKeyVersion = (string)dr["PRIVATEKEYVER"];
            fiscalTrans.Signature = (string)dr["SIGNATURE"];
            fiscalTrans.Currency = (string)dr["CURRENCY"];

            fiscalTrans.StoreDescription = (string)dr["StoreName"];
            fiscalTrans.TerminalDescription = (string)dr["TerminalName"];
            fiscalTrans.EmployeeID = (string)dr["STAFF"];
            fiscalTrans.EmployeeDescription = (string)dr["StaffName"];
        }

        private string ResolveSort(FiscalSort sort, bool sortBackwards)
        {
            string direction = sortBackwards ? "DESC" : "ASC";
            string column = "";

            switch (sort)
            {
                case FiscalSort.TransDate:
                    column = "TRANSDATE"; break;
                case FiscalSort.ReceiptID:
                    column = "RECEIPTID"; break;
                case FiscalSort.Staff:
                    column = "STAFF"; break;
                case FiscalSort.Terminal:
                    column = "TERMINAL"; break;
                case FiscalSort.Store:
                    column = "STORE"; break;
                case FiscalSort.GrossAmount:
                    column = "GROSSAMOUNT"; break;
            }

            return column + " " + direction;
        }

        public List<FiscalTrans> Find(IConnectionManager entry, FiscalTransSearchFilter searchFilter, out int totalReceiptsMatching)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            using (var cmdCount = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM(" +
                                  "SELECT TRANSACTIONID, " +
                                  "RECEIPTID, " +
                                  "TRANSDATE, " +
                                  "STORE, " +
                                  "TERMINAL, " +
                                  "STAFF, " +
                                  "GROSSAMOUNT, " +
                                  "NETAMOUNT, " +
                                  "CURRENCY, " +
                                  "StoreName, " +
                                  "TerminalName, " +
                                  "StaffName, " +
                                  "PRIVATEKEYVER, " +
                                  "SIGNATURE, " +
                                  "FISCALCONTROLID, " +
                                  "FISCALUNITID, " +
                                  "TYPE, " +
                                  "ROW_NUMBER() over(order by " + ResolveSort(searchFilter.Sort, searchFilter.SortBackwards) + ") as rownum FROM ( " +
                                  "Select Top 2147483647 t.TRANSACTIONID,ISNULL(t.RECEIPTID,'') as RECEIPTID,t.TRANSDATE," +
                                  "ISNULL(t.STORE,'') as STORE, ISNULL(t.TERMINAL,'') as TERMINAL,ISNULL(trans.STAFF,'') as STAFF," +
                                  "ISNULL(term.NAME,'') as TerminalName,ISNULL(store.NAME,'') as StoreName,ISNULL(staff.Name,'') as StaffName," +
                                  "ISNULL(t.GROSSAMOUNT, 0) as GROSSAMOUNT, ISNULL(t.NETAMOUNT, 0) as NETAMOUNT, trans.CURRENCY, " +
                                  "ISNULL(PRIVATEKEYVER, '') AS PRIVATEKEYVER, ISNULL(SIGNATURE, '') AS SIGNATURE, " +
                                  "ISNULL(FISCALCONTROLID,'') AS FISCALCONTROLID, ISNULL(FISCALUNITID,'') AS FISCALUNITID, t.TYPE " +
                                  "from RBOTRANSACTIONFISCALTRANS t " +
                                  "left outer join RBOTRANSACTIONTABLE trans on t.STORE = trans.STORE and t.TERMINAL = trans.TERMINAL and t.TRANSACTIONID = trans.TRANSACTIONID " +
                                  "and t.DATAAREAID = trans.DATAAREAID " +
                                  "left outer join RBOTERMINALTABLE term on t.TERMINAL = term.TerminalID and t.STORE = term.STOREID and term.DATAAREAID = t.DATAAREAID " +
                                  "left outer join RBOSTORETABLE store on t.STORE = store.STOREID and store.DATAAREAID = t.DATAAREAID " +
                                  "left outer join RBOSTAFFTABLE staff on trans.STAFF = staff.STAFFID and staff.DATAAREAID = staff.DATAAREAID " +
                                  "where t.DATAAREAID = @dataAreaId AND t.TYPE = " + (int)TypeOfTransaction.Sales;


                cmdCount.CommandText = @"select count(*) from RBOTRANSACTIONFISCALTRANS t " +
                                  "left outer join RBOTRANSACTIONTABLE trans on t.STORE = trans.STORE and t.TERMINAL = trans.TERMINAL and t.TRANSACTIONID = trans.TRANSACTIONID " +
                                  "and t.DATAAREAID = trans.DATAAREAID " +
                                  "left outer join RBOTERMINALTABLE term on t.TERMINAL = term.TerminalID and t.STORE = term.STOREID and term.DATAAREAID = t.DATAAREAID " +
                                  "left outer join RBOSTORETABLE store on t.STORE = store.STOREID and store.DATAAREAID = t.DATAAREAID " +
                                  "left outer join RBOSTAFFTABLE staff on trans.STAFF = staff.STAFFID and staff.DATAAREAID = staff.DATAAREAID " +
                                  "where t.DATAAREAID = @dataAreaId AND t.TYPE = " + (int)TypeOfTransaction.Sales;

                if (!string.IsNullOrWhiteSpace(searchFilter.IdOrReceiptNumber))
                {
                    searchFilter.IdOrReceiptNumber = PreProcessSearchText(searchFilter.IdOrReceiptNumber, true, searchFilter.IdOrReceiptNumberBeginsWith);
                    string where = " and ( t.RECEIPTID like @receiptID )";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "receiptID", searchFilter.IdOrReceiptNumber);
                    MakeParam(cmdCount, "receiptID", searchFilter.IdOrReceiptNumber);
                }

                if (searchFilter.DateFrom != Date.Empty)
                {
                    string where = " and t.TRANSDATE >= @dateFrom";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "dateFrom", searchFilter.DateFrom.DateTime, SqlDbType.DateTime);
                    MakeParam(cmdCount, "dateFrom", searchFilter.DateFrom.DateTime, SqlDbType.DateTime);
                }

                if (searchFilter.DateTo != Date.Empty)
                {
                    string where = " and t.TRANSDATE <= @dateTo";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "dateTo", searchFilter.DateTo.DateTime, SqlDbType.DateTime);
                    MakeParam(cmdCount, "dateTo", searchFilter.DateTo.DateTime, SqlDbType.DateTime);
                }

                if (!RecordIdentifier.IsEmptyOrNull(searchFilter.EmployeeID))
                {
                    string where = " and trans.STAFF = @staffID";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "staffID", (string)searchFilter.EmployeeID);
                    MakeParam(cmdCount, "staffID", (string)searchFilter.EmployeeID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(searchFilter.StoreID))
                {
                    string where = " and t.STORE = @storeID";
                    string whereSecond = " and term.STOREID = @storeID";
                    cmd.CommandText += where;
                    cmd.CommandText += whereSecond;
                    cmdCount.CommandText += where;
                    cmdCount.CommandText += whereSecond;

                    cmd.CommandText += " and term.STOREID = @storeID";
                    MakeParam(cmd, "storeID", (string)searchFilter.StoreID);
                    MakeParam(cmdCount, "storeID", (string)searchFilter.StoreID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(searchFilter.TerminalID))
                {
                    string where = " and t.TERMINAL = @terminalID";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "terminalID", (string)searchFilter.TerminalID);
                    MakeParam(cmdCount, "terminalID", (string)searchFilter.TerminalID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(searchFilter.Currency))
                {
                    string where = " and trans.CURRENCY = @currency";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "currency", (string)searchFilter.Currency);
                    MakeParam(cmdCount, "currency", (string)searchFilter.Currency);
                }

                if (searchFilter.PaidAmount != 0)
                {
                    string where = " and t.GROSSAMOUNT = @paidAmount";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where + "))";
                    MakeParam(cmd, "paidAmount", searchFilter.PaidAmount);
                    MakeParam(cmdCount, "paidAmount", searchFilter.PaidAmount);
                }

                if (!string.IsNullOrWhiteSpace(searchFilter.Signature))
                {
                    searchFilter.Signature = PreProcessSearchText(searchFilter.Signature, true, searchFilter.SignatureBeginsWith);
                    string where = " and ( t.SIGNATURE like @signature )";
                    cmd.CommandText += where;
                    cmdCount.CommandText += where;
                    MakeParam(cmd, "signature", searchFilter.Signature);
                    MakeParam(cmdCount, "signature", searchFilter.Signature);
                }

                cmd.CommandText += ") AS QUERY) AS QUERY2 WHERE rownum BETWEEN @recordFrom AND @recordTo ORDER BY rownum ";
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "recordFrom", searchFilter.RecordFrom, SqlDbType.Int);
                MakeParam(cmd, "recordTo", searchFilter.RecordTo, SqlDbType.Int);

                MakeParam(cmdCount, "dataAreaId", entry.Connection.DataAreaId);
                totalReceiptsMatching = (int)entry.Connection.ExecuteScalar(cmdCount);

                return Execute<FiscalTrans>(entry, cmd, CommandType.Text, PopulateFiscalTrans);
            }
        }
        #endregion
    }
}
