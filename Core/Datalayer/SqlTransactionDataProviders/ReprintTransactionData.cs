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
    public class ReprintTransactionData : SqlServerDataProviderBase, IReprintTransactionData
    {
        public void Insert(IConnectionManager entry, ReprintInfo reprintInfo, IPosTransaction transaction)
        {
            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONREPRINTTRANS", StatementType.Insert, false);

            statement.AddKey("TRANSACTIONID", transaction.TransactionId);
            statement.AddKey("LINENUM", (int)reprintInfo.LineID, SqlDbType.Int);
            statement.AddKey("STORE", transaction.StoreId);
            statement.AddKey("TERMINAL", transaction.TerminalId);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

            statement.AddField("PRINTSTORE", (string) reprintInfo.Store);
            statement.AddField("PRINTTERMINAL", (string) reprintInfo.Terminal);
            statement.AddField("PRINTDATE", reprintInfo.ReprintDate.DateTime, SqlDbType.DateTime);
            statement.AddField("STAFF", (string)transaction.Cashier.ID);
            statement.AddField("REPRINTTYPE", (int)reprintInfo.ReprintType, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        private void PopulateReprint(IDataReader dr, ReprintInfo reprint)
        {
            reprint.Store = (string)dr["PRINTSTORE"];
            reprint.Terminal = (string)dr["PRINTTERMINAL"];
            reprint.ReprintDate.DateTime = (DateTime)dr["PRINTDATE"];
            reprint.Staff = (string)dr["STAFF"];
            reprint.ReprintType = (ReprintTypeEnum)(int)dr["REPRINTTYPE"];
        }

        public List<ReprintInfo> GetReprintInfo(IConnectionManager entry, RecordIdentifier transactionID, IRetailTransaction transaction)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT [PRINTSTORE]
                                          ,[PRINTTERMINAL]                                          
                                          ,[LINENUM]                                          
                                          ,[PRINTDATE]
                                          ,[STAFF]
                                          ,[REPRINTTYPE]
                                    FROM RBOTRANSACTIONREPRINTTRANS
                                    WHERE TRANSACTIONID = @transactionID and STORE = @storeID and TERMINAL = @terminalID
                                    AND DATAAREAID = @dataAreaID ";
                                    

                MakeParam(cmd, "transactionID", (string)transactionID);
                MakeParam(cmd, "storeID", transaction.StoreId);
                MakeParam(cmd, "terminalID", transaction.TerminalId);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<ReprintInfo>(entry, cmd, CommandType.Text, PopulateReprint);
            }
        }
    }
}
