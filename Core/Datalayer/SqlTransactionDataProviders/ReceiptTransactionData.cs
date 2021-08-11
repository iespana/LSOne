using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Receipts;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;


namespace LSRetail.POS.DataProviders.Transaction
{
    public class ReceiptTransactionData : SqlServerDataProviderBase, IReceiptTransactionData
    {
        public void Insert(IConnectionManager entry, ReceiptInfo receiptInfo, IPosTransaction transaction)
        {
            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONRECEIPTS", StatementType.Insert, false);
            
            statement.AddKey("TRANSACTIONID", transaction.TransactionId);
            statement.AddKey("LINENUM", (int)receiptInfo.LineID, SqlDbType.Int);
            statement.AddKey("STORE", transaction.StoreId);
            statement.AddKey("TERMINAL", transaction.TerminalId);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            
            statement.AddField("FORMTYPE", (Guid)receiptInfo.FormType, SqlDbType.UniqueIdentifier);
            statement.AddField("PRINTSTRING", receiptInfo.PrintString);
            statement.AddField("FORMWIDTH", receiptInfo.FormWidth, SqlDbType.Int);
            statement.AddField("ISEMAILRECEIPT", receiptInfo.IsEmailReceipt, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        private void PopulateReceiptInfo(IDataReader dr, ReceiptInfo receiptInfo)
        {
            receiptInfo.FormType = (Guid)dr["FORMTYPE"];
            receiptInfo.LineID = (int)dr["LINENUM"];
            receiptInfo.PrintString = (string)dr["PRINTSTRING"];
            receiptInfo.FormWidth = (int)dr["FORMWIDTH"];
            receiptInfo.IsEmailReceipt = (bool)dr["ISEMAILRECEIPT"];
        }

        public List<ReceiptInfo> GetReceiptInfo(IConnectionManager entry, RecordIdentifier transactionID, IPosTransaction transaction)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT [FORMTYPE]
                                          ,[PRINTSTRING]                                          
                                          ,[LINENUM]    
                                          ,[FORMWIDTH]
                                          ,[ISEMAILRECEIPT]
                                    FROM RBOTRANSACTIONRECEIPTS
                                    WHERE TRANSACTIONID = @transactionID and STORE = @storeID and TERMINAL = @terminalID
                                    AND DATAAREAID = @dataAreaID ";
                                    

                MakeParam(cmd, "transactionID", (string)transactionID);
                MakeParam(cmd, "storeID", transaction.StoreId);
                MakeParam(cmd, "terminalID", transaction.TerminalId);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<ReceiptInfo>(entry, cmd, CommandType.Text, PopulateReceiptInfo);
            }
        }
    }
}
