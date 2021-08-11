using System;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Utilities.DataTypes;
using LSRetail.Forecourt;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class FuelSalesLineItemData : SqlServerDataProviderBase, IFuelSalesLineItemData
    {
        private static FuelSalesLineItem Populate(IConnectionManager entry, IDataReader dr, RetailTransaction transaction)
        {
            var item = new FuelSalesLineItem(new BarCode(), transaction)
                {
                    Voided = ((int) dr["TRANSACTIONSTATUS"] == (int) TransactionStatus.Voided),
                    FuelingPointId = (int) dr["FUELLINGPOINTID"],
                    GradeId = (int) dr["GRADEID"],
                    TransSeqNo = (int) dr["TRANSSEQNO"],
                    NozzleId = (int) dr["NOZZLEID"],
                    Volume = (decimal) dr["VOLUME"],
                    UnitPrice = (decimal) dr["UNITPRICE"],
                    TotalPrice = (decimal) dr["TOTALPRICE"],
                    Oiltax = (decimal) dr["OILTAX"],
                    FpType = (FuellingPointType) dr["FUELLINGPOINTTYPE"],
                    OriginatesFromForecourt = ((byte) dr["ORIGINATESFROMFORECOURT"] != 0),
                    FuellingPointTransaction = new FuellingPointTransaction
                            {
                                TransLockID = (int) dr["TRANSLOCKID"],
                                AmountDue = (decimal) dr["AMOUNTDUE"],
                                AmountDueValid = ((byte) dr["AMOUNTDUEVALID"] != 0),
                                FormattedMoney = dr["FORMATTEDMONEY"].ToString(),
                                ServiceModeID = (int) dr["SERVICEMODEID"],
                                AuthID = (int) dr["AUTHID"],
                                FuellingModeID = (int) dr["FUELLINGMODEID"],
                                PriceGroupID = (int) dr["PRICEGROUPID"],
                                PriceSetID = (int) dr["PRICESETID"],
                                GradeOptionNo = (int) dr["GRADEOPTIONNO"],
                                StartTime = (DateTime) dr["STARTTIME"],
                                FinishTime = (DateTime) dr["FINISHTIME"],
                                TransErrorCode = (int) dr["TRANSERRORCODE"],
                                StartLimitType = (int) dr["STARTLIMITTYPE"],
                                Supervised = ((byte) dr["SUPERVISED"] != 0),
                                ItemID = (string) dr["ITEMID"],
                                FuelName = (string) dr["FUELNAME"]
                            }
                };
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="id">TransactionID, LineNumber</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual FuelSalesLineItem Get(IConnectionManager entry, RecordIdentifier id, RetailTransaction transaction)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ISNULL(TRANSACTIONSTATUS, 0) AS TRANSACTIONSTATUS, 
                                    ISNULL(FUELLINGPOINTID, 0) AS FUELLINGPOINTID, ISNULL(GRADEID, 0) AS GRADEID, 
                                    ISNULL(TRANSSEQNO, 0) AS TRANSSEQNO, ISNULL(NOZZLEID, 0) AS NOZZLEID, 
                                    ISNULL(VOLUME, 0) AS VOLUME, ISNULL(UNITPRICE, 0) AS UNITPRICE, ISNULL(TOTALPRICE, 0) AS TOTALPRICE, 
                                    ISNULL(OILTAX, 0) AS OILTAX, ISNULL(FUELLINGPOINTTYPE, 0) AS FUELLINGPOINTTYPE, 
                                    ISNULL(ORIGINATESFROMFORECOURT, 0) AS ORIGINATESFROMFORECOURT, ISNULL(TRANSLOCKID, 0) AS TRANSLOCKID,
                                    ISNULL(AMOUNTDUE, 0) AS AMOUNTDUE, ISNULL(AMOUNTDUEVALID, 0) AS AMOUNTDUEVALID, 
                                    ISNULL(FORMATTEDMONEY, '') AS FORMATTEDMONEY, ISNULL(SERVICEMODEID, 0) AS SERVICEMODEID, 
                                    ISNULL(AUTHID, 0) AS AUTHID, ISNULL(FUELLINGMODEID, 0) AS FUELLINGMODEID, 
                                    ISNULL(PRICEGROUPID, 0) AS PRICEGROUPID, ISNULL(PRICESETID, 0) AS PRICESETID, 
                                    ISNULL(GRADEOPTIONNO, 0) AS GRADEOPTIONNO, ISNULL(STARTTIME, '1900-01-01') AS STARTTIME, 
                                    ISNULL(FINISHTIME, '1900-01-01') AS FINISHTIME, ISNULL(TRANSERRORCODE, 0) AS TRANSERRORCODE, 
                                    ISNULL(STARTLIMITTYPE, 0) AS STARTLIMITTYPE, ISNULL(SUPERVISED, 0) AS SUPERVISED, 
                                    ISNULL(ITEMID, '') AS ITEMID, ISNULL(FUELNAME, '') AS FUELNAME 
                                    FROM RBOTRANSACTIONFUELTRANS
                                    WHERE DATAAREAID = @dataAreaID AND TRANSACTIONID = @transactionID AND LINENUM = @lineNumber 
                                    AND TERMINAL = @terminalID AND STORE = @storeID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", id[0]);
                MakeParam(cmd, "lineNumber", (int)id[1], SqlDbType.Decimal);
                MakeParam(cmd, "terminalID", transaction.TerminalId);
                MakeParam(cmd, "storeID", transaction.StoreId);
                
                var list = Execute(entry, cmd, CommandType.Text, transaction, Populate);
                return list.Count > 0 ? list[0] : null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">TransactionID, LineNumber, Terminal, Store</param>
        /// <returns></returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            string[] fields = { "TRANSACTIONID", "LINENUM", "TERMINAL",  "STORE"};
            return RecordExists(entry, "RBOTRANSACTIONFUELTRANS", fields, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="item">TransactionID, LineNumber,Terminal, Store</param>
        public virtual void Delete(IConnectionManager entry, FuelSalesLineItem item)
        {
            string[] fields = { "TRANSACTIONID", "LINENUM", "TERMINAL", "STORE" };
            var id = new RecordIdentifier(item.Transaction.TransactionId, 
                new RecordIdentifier((decimal)item.LineId, 
                    new RecordIdentifier(entry.CurrentStoreID, entry.CurrentTerminalID)));
            DeleteRecord(entry, "RBOTRANSACTIONFUELTRANS", fields, id, "");
        }

        public virtual void Save(IConnectionManager entry, FuelSalesLineItem item, RetailTransaction transaction)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONFUELTRANS");
            if (Exists(entry, new RecordIdentifier(transaction.TransactionId, new RecordIdentifier(item.LineNumber, new RecordIdentifier(transaction.TerminalId, transaction.StoreId)))))
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("TRANSACTIONID", transaction.TransactionId);
                statement.AddCondition("LINENUM", item.LineId, SqlDbType.Decimal);
                statement.AddCondition("STORE", transaction.StoreId);
                statement.AddCondition("TERMINAL", transaction.TerminalId);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("TRANSACTIONID", transaction.TransactionId);
                statement.AddKey("LINENUM", item.LineId, SqlDbType.Decimal);
                statement.AddKey("STORE", transaction.StoreId);
                statement.AddKey("TERMINAL", transaction.TerminalId);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            statement.AddField("TRANSACTIONSTATUS", item.Voided ? 1 : 0, SqlDbType.Int);
            statement.AddField("FUELLINGPOINTID", item.FuelingPointId, SqlDbType.Int);
            statement.AddField("GRADEID", item.GradeId, SqlDbType.Int);
            statement.AddField("TRANSSEQNO", item.TransSeqNo, SqlDbType.Int);
            statement.AddField("NOZZLEID", item.NozzleId, SqlDbType.Int);
            statement.AddField("VOLUME", item.Volume, SqlDbType.Decimal);
            statement.AddField("UNITPRICE", item.UnitPrice, SqlDbType.Decimal);
            statement.AddField("TOTALPRICE", item.TotalPrice, SqlDbType.Decimal);
            statement.AddField("OILTAX", item.Oiltax, SqlDbType.Decimal);
            statement.AddField("FUELLINGPOINTTYPE", (int)item.FpType, SqlDbType.Int);
            statement.AddField("ORIGINATESFROMFORECOURT", item.OriginatesFromForecourt ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("TRANSLOCKID", item.FuellingPointTransaction.TransLockID, SqlDbType.Int);
            statement.AddField("SUPERVISED", item.FuellingPointTransaction.Supervised ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("AMOUNTDUEVALID", item.FuellingPointTransaction.AmountDueValid ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("FORMATTEDMONEY", item.FuellingPointTransaction.FormattedMoney);
            statement.AddField("SERVICEMODEID", item.FuellingPointTransaction.ServiceModeID, SqlDbType.Int);
            statement.AddField("FUELLINGMODEID", item.FuellingPointTransaction.FuellingModeID, SqlDbType.Int);
            statement.AddField("AUTHID", item.FuellingPointTransaction.AuthID, SqlDbType.Int);
            statement.AddField("PRICEGROUPID", item.FuellingPointTransaction.PriceGroupID, SqlDbType.Int);
            statement.AddField("PRICESETID", item.FuellingPointTransaction.PriceSetID, SqlDbType.Int);
            statement.AddField("GRADEOPTIONNO", item.FuellingPointTransaction.GradeOptionNo, SqlDbType.Int);
            if (item.FpType == FuellingPointType.FpTransaction)
            {              
                statement.AddField("AMOUNTDUE", item.FuellingPointTransaction.AmountDue, SqlDbType.Decimal);                          
                statement.AddField("STARTTIME", item.FuellingPointTransaction.StartTime, SqlDbType.DateTime);
                statement.AddField("FINISHTIME", item.FuellingPointTransaction.FinishTime, SqlDbType.DateTime);
                statement.AddField("TRANSERRORCODE", item.FuellingPointTransaction.TransErrorCode, SqlDbType.Int);
                statement.AddField("STARTLIMITTYPE", item.FuellingPointTransaction.StartLimitType, SqlDbType.Int);               
                statement.AddField("ITEMID", item.FuellingPointTransaction.ItemID);
                statement.AddField("FUELNAME", item.FuellingPointTransaction.FuelName);
            }
            else if (item.FpType == FuellingPointType.ForecourtManager && !item.OriginatesFromForecourt)
            {
                statement.AddField("AMOUNTDUE", item.GrossAmountWithTax, SqlDbType.Decimal);
                statement.AddField("ITEMID", item.ItemId);
                statement.AddField("FUELNAME", item.Description);
                statement.AddField("STARTTIME", transaction.BeginDateTime, SqlDbType.DateTime);
                statement.AddField("FINISHTIME", transaction.EndDateTime, SqlDbType.DateTime);
            }
            else if (item.FpType == FuellingPointType.ForecourtManager && item.OriginatesFromForecourt)
            {
                DateTime lowOutOfBoundTime = new DateTime(1753, 1, 1, 12, 00, 00);
                statement.AddField("AMOUNTDUE", item.FuellingPointTransaction.Amount, SqlDbType.Decimal);
                statement.AddField("STARTTIME", item.FuellingPointTransaction.StartTime < lowOutOfBoundTime ? DateTime.Now : item.FuellingPointTransaction.StartTime, SqlDbType.DateTime);
                statement.AddField("FINISHTIME", item.FuellingPointTransaction.StartTime < lowOutOfBoundTime ? DateTime.Now : item.FuellingPointTransaction.FinishTime, SqlDbType.DateTime);
                statement.AddField("STARTLIMITTYPE", item.FuellingPointTransaction.StartLimitType, SqlDbType.Int);
                statement.AddField("ITEMID", item.FuellingPointTransaction.ItemID);
                statement.AddField("FUELNAME", item.FuellingPointTransaction.FuelName);
            }
            entry.Connection.ExecuteStatement(statement);
        }
    }
}
