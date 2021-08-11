using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.IncomeExpenseItem;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class IncomeExpenseItemData : SqlServerDataProviderBase, IIncomeExpenseItemData
    {
        private static IncomeExpenseItem Populate(IConnectionManager entry, IDataReader dr, RetailTransaction transaction)
        {
            var item = new IncomeExpenseItem(transaction)
                {
                    LineId = (int) ((decimal) dr["LINENUM"]),
                    AccountNumber = (string) dr["INCOMEEXEPENSEACCOUNT"],
                    Voided = ((int) dr["TRANSACTIONSTATUS"] != 0),
                    Amount = (decimal) dr["AMOUNT"],
                    AccountType = (IncomeExpenseAccountType) (int) dr["ACCOUNTTYPE"],
                    AccountName = (string) dr["ACCOUNTNAME"],
                    Transaction =
                        {
                            TransactionId = (string) dr["TRANSACTIONID"],
                            StoreId = (string) dr["STORE"],
                            TerminalId = (string) dr["TERMINAL"]
                        },                    
                };
            item.SalesPerson.ID = (string)dr["STAFF"];
            item.Transaction.BeginDateTime = (DateTime)dr["TRANSDATE"];
            ((RetailTransaction)item.Transaction).ReceiptId = (string)dr["RECEIPTID"];
            if (item.AccountType == IncomeExpenseAccountType.EXPENSE)
            {
                item.Price = item.Amount * -1;
                item.PriceWithTax = item.Amount * -1;
                item.StandardRetailPrice = item.Amount * -1;
                item.NetAmount = item.Amount * -1;
                item.NetAmountWithTax = item.Amount * -1;
                item.GrossAmount = item.Amount * -1;
                item.GrossAmountWithTax = item.Amount * -1;
            }
            else
            {
                item.Price = item.Amount;
                item.PriceWithTax = item.Amount;
                item.StandardRetailPrice = item.Amount;
                item.NetAmount = item.Amount;
                item.NetAmountWithTax = item.Amount;
                item.GrossAmount = item.Amount;
                item.GrossAmountWithTax = item.Amount;
            }
            return item;
        }

        public virtual IncomeExpenseItem Get(IConnectionManager entry, RetailTransaction transaction, decimal lineNumber)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TRANSACTIONID, LINENUM, ISNULL(RECEIPTID, '') AS RECEIPTID,
                                    ISNULL(INCOMEEXEPENSEACCOUNT, '') AS INCOMEEXEPENSEACCOUNT,
                                    ISNULL(STORE, '') AS STORE, ISNULL(TERMINAL, '') AS TERMINAL, 
                                    ISNULL(STAFF, '') AS STAFF, ISNULL(TRANSACIONSTATUS, 0) AS TRASACTIONSTATUS,
                                    ISNULL(AMOUNT, 0) AS AMOUNT, ISNULL(ACCOUNTTYPE, 0) AS ACCOUNTTYPE,
                                    ISNULL(TRANSDATE, '1900-01-01') AS TRANSDATE, ISNULL(ACCOUNTNAME, '') AS ACCOUNTNAME
                                    FROM RBOTRANSACTIONINCOMEEXPEN20158
                                    WHERE DATAAREAID = @dataAreaID AND TRANSACTIONID = @transactionID AND LINENUM = @lineNumber";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", transaction.TransactionId);
                MakeParam(cmd, "lineNumber", lineNumber, SqlDbType.Decimal);
                
                var items = Execute(entry, cmd, CommandType.Text, transaction, Populate);
                return items.Count > 0 ? items[0] : null;
            }
        }

        public virtual List<IncomeExpenseItem> Get(IConnectionManager entry, RetailTransaction transaction)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TRANSACTIONID, LINENUM, ISNULL(RECEIPTID, '') AS RECEIPTID,
                                    ISNULL(INCOMEEXEPENSEACCOUNT, '') AS INCOMEEXEPENSEACCOUNT,
                                    ISNULL(STORE, '') AS STORE, ISNULL(TERMINAL, '') AS TERMINAL, 
                                    ISNULL(STAFF, '') AS STAFF, ISNULL(TRANSACTIONSTATUS, 0) AS TRANSACTIONSTATUS,
                                    ISNULL(AMOUNT, 0) AS AMOUNT, ISNULL(ACCOUNTTYPE, 0) AS ACCOUNTTYPE,
                                    ISNULL(TRANSDATE, '1900-01-01') AS TRANSDATE, ISNULL(ACCOUNTNAME, '') AS ACCOUNTNAME
                                    FROM RBOTRANSACTIONINCOMEEXPEN20158
                                    WHERE DATAAREAID = @dataAreaID AND TRANSACTIONID = @transactionID
                                    ORDER BY LINENUM";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", transaction.TransactionId);
                return Execute(entry, cmd, CommandType.Text, transaction, Populate);
            }            
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            string[] fields = { "TRANSACTIONID", "LINENUM"};
            return RecordExists(entry, "RBOTRANSACTIONINCOMEEXPEN20158", fields, id);
        }

        public virtual void Delete(IConnectionManager entry, IncomeExpenseItem item)
        {
            string[] fields = { "TRANSACTIONID", "LINENUM" };
            var id = new RecordIdentifier(item.Transaction.TransactionId, (decimal)item.LineId);
            DeleteRecord(entry, "RBOTRANSACTIONINCOMEEXPEN20158", fields, id, "");
        }

        public virtual void Save(IConnectionManager entry, IncomeExpenseItem item)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONINCOMEEXPEN20158");
            if (Exists(entry, new RecordIdentifier(item.Transaction.TransactionId, (decimal)item.LineId)))
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("TRANSACTIONID", item.Transaction.TransactionId);
                statement.AddCondition("LINENUM", (decimal)item.LineId, SqlDbType.Decimal);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("TRANSACTIONID", item.Transaction.TransactionId);
                statement.AddKey("LINENUM", (decimal)item.LineId, SqlDbType.Decimal);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            statement.AddField("INCOMEEXEPENSEACCOUNT", item.AccountNumber);
            statement.AddField("TRANSACTIONSTATUS", item.Voided ? 1 : 0, SqlDbType.Int);
            statement.AddField("AMOUNT", item.Amount, SqlDbType.Decimal);
            statement.AddField("ACCOUNTTYPE", (int)item.AccountType, SqlDbType.Int);
            statement.AddField("ACCOUNTNAME", item.AccountName);
            statement.AddField("STORE", item.Transaction.StoreId);
            statement.AddField("TERMINAL", item.Transaction.TerminalId);
            statement.AddField("STAFF", (string) ((RetailTransaction)item.Transaction).SalesPerson.ID);
            statement.AddField("RECEIPTID", ((RetailTransaction)item.Transaction).ReceiptId);
            statement.AddField("TRANSDATE", item.Transaction.BeginDateTime, SqlDbType.DateTime);
            statement.AddField("TRANSTIME", (item.Transaction.BeginDateTime.Hour * 3600) + (item.Transaction.BeginDateTime.Minute * 60) + item.Transaction.BeginDateTime.Second, SqlDbType.Int);
            entry.Connection.ExecuteStatement(statement);
        }
    }
}
