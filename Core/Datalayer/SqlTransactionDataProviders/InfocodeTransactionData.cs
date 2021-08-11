using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class InfocodeTransactionData : SqlServerDataProviderBase, IInfocodeTransactionData
    {
        private static void Populate(IDataReader dr, InfoCodeLineItem infocode)
        {
            infocode.Type = (InfoCodeLineItem.InfocodeType)dr["TYPE"];
            infocode.InfocodeId = (string)dr["INFOCODEID"];
            infocode.InputType = (InfoCodeLineItem.InputTypes)(int)dr["INPUTTYPE"];
            if (infocode.InputType == InfoCodeLineItem.InputTypes.SubCodeButtons || infocode.InputType == InfoCodeLineItem.InputTypes.SubCodeList)
            {
                infocode.Subcode = (string)dr["INFORMATION"];
                infocode.Information = (string)dr["SUBCODEDESCRIPTION"];
            }
            else
            {
                infocode.Information = (string)dr["INFORMATION"];
            }
            infocode.BeginDateTime = (DateTime)dr["TRANSDATE"];
            infocode.Amount = (decimal)dr["AMOUNT"];
            infocode.Subcode = (string)dr["SUBINFOCODEID"];
            infocode.RefRelation = (string)dr["SOURCECODE"];
            infocode.RefRelation2 = (string)dr["SOURCECODE2"];
            infocode.RefRelation3 = (string)dr["SOURCECODE3"];
            infocode.LineId = (int)dr["COUNTER"];
            infocode.Prompt = (string)dr["PROMPT"];
            infocode.PrintPromptOnReceipt = ((byte)dr["PRINTPROMPTONRECEIPT"] != 0);
            infocode.PrintInputOnReceipt = ((byte)dr["PRINTINPUTONRECEIPT"] != 0);
            infocode.PrintInputNameOnReceipt = ((byte)dr["PRINTINPUTNAMEONRECEIPT"] != 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="id">LineID, TransactionID, Type, TerminalID, StoreID</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual List<InfoCodeLineItem> Get(IConnectionManager entry, RecordIdentifier id, PosTransaction transaction)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ISNULL(r.TYPE, 0) AS TYPE, 
                                           ISNULL(r.INFOCODEID, '') AS INFOCODEID,
                                           ISNULL(r.INPUTTYPE, 0) AS INPUTTYPE, 
                                           ISNULL(r.INFORMATION, '') AS INFORMATION,
                                           ISNULL(r.TRANSDATE, '1900-01-01') AS TRANSDATE, 
                                           ISNULL(r.AMOUNT, 0) AS AMOUNT,
                                           ISNULL(r.SUBINFOCODEID, '') AS SUBINFOCODEID, 
                                           ISNULL(r.SOURCECODE, '') AS SOURCECODE,
                                           ISNULL(r.SOURCECODE2, '') AS SOURCECODE2, 
                                           ISNULL(r.SOURCECODE3, '') AS SOURCECODE3,
                                           ISNULL(r.COUNTER, 0) AS COUNTER, 
                                           ISNULL(r.PROMPT, '') AS PROMPT,
                                           ISNULL(r.PRINTPROMPTONRECEIPT, 0) AS PRINTPROMPTONRECEIPT, 
                                           ISNULL(r.PRINTINPUTONRECEIPT, 0) AS PRINTINPUTONRECEIPT,
                                           ISNULL(r.PRINTINPUTNAMEONRECEIPT, 0) AS PRINTINPUTNAMEONRECEIPT,
                                           ISNULL(s.DESCRIPTION, '') as SUBCODEDESCRIPTION
                                    FROM RBOTRANSACTIONINFOCODETRANS r
                                    LEFT JOIN RBOINFORMATIONSUBCODETABLE s on s.INFOCODEID = r.INFOCODEID and s.SUBCODEID = r.SUBINFOCODEID and s.DATAAREAID = r.DATAAREAID
                                    WHERE r.DATAAREAID = @dataAreaID 
                                    AND r.TERMINAL = @terminalID 
                                    AND r.TRANSACTIONID = @transactionID 
                                    AND r.STORE = @storeID 
                                    AND r.LINEID = @lineID 
                                    AND r.TYPE = @type 
                                    ORDER BY r.LINENUM";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", id[0]);
                MakeParam(cmd, "lineID", (int)id[1], SqlDbType.Int);
                MakeParam(cmd, "type", (int)id[2], SqlDbType.Int);
                MakeParam(cmd, "terminalID", id[3]);
                MakeParam(cmd, "storeID", id[4]);
                return Execute<InfoCodeLineItem>(entry, cmd, CommandType.Text, Populate);
            }
        }

        private static int GetMaxLineId(IConnectionManager entry,RecordIdentifier transactionId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"select ISNULL(MAX(LINENUM), 0)+1 AS LINEID 
                                    from RBOTRANSACTIONINFOCODETRANS
                                    where TRANSACTIONID = @transactionID and DATAAREAID = @dataAreaID";

                MakeParam(cmd, "transactionID", (string)transactionId);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                object result = entry.Connection.ExecuteScalar(cmd);

                return (result is DBNull) ? 1 : (int)(decimal)result;
            }
        }

        public virtual void Insert(IConnectionManager entry, InfoCodeLineItem infocodeItem, PosTransaction transaction, int saleLineId)
        {
            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONINFOCODETRANS", StatementType.Insert, false);

            int lineId = GetMaxLineId(entry, transaction.TransactionId);

            statement.AddKey("TRANSACTIONID", transaction.TransactionId);
            statement.AddKey("LINENUM", (decimal)lineId, SqlDbType.Decimal);
            statement.AddKey("TYPE", (int)infocodeItem.Type, SqlDbType.Int);
            statement.AddKey("INFOCODEID", infocodeItem.InfocodeId);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

            statement.AddField("LINEID", saleLineId, SqlDbType.Int);

            if ((infocodeItem.InputType == InfoCodeLineItem.InputTypes.SubCodeButtons) || (infocodeItem.InputType == InfoCodeLineItem.InputTypes.SubCodeList))
            {
                 statement.AddField("INFORMATION", infocodeItem.Subcode); 
            }
            else
            {
                 statement.AddField("INFORMATION", infocodeItem.Information);
            }

            statement.AddField("INFOAMOUNT", 0M, SqlDbType.Decimal); //TODO: ATH!!

            statement.AddField("TRANSDATE", infocodeItem.BeginDateTime, SqlDbType.DateTime);
            statement.AddField("TRANSTIME", Conversion.TimeToInt(infocodeItem.BeginDateTime), SqlDbType.Int);
            statement.AddField("STAFF", (string)transaction.Cashier.ID);
            statement.AddField("STORE", transaction.StoreId);
            statement.AddField("TERMINAL", transaction.TerminalId);

            statement.AddField("ITEMTENDER", ""); //TODO: This is not in the Navision table ??
            statement.AddField("AMOUNT", infocodeItem.Amount, SqlDbType.Decimal); //TODO: Sales: Negative; Payment: Positive
            statement.AddField("INPUTTYPE", (int)infocodeItem.InputType, SqlDbType.Int);
            statement.AddField("SUBINFOCODEID", infocodeItem.Subcode ?? ""); //null state is used and therefore it can be at this point

            statement.AddField("STATEMENTID", "");
            statement.AddField("STATEMENTCODE", ""); //TODO: This is the staff id in Navision ??

            statement.AddField("SOURCECODE", infocodeItem.RefRelation);
            statement.AddField("SOURCECODE2", infocodeItem.RefRelation2);
            statement.AddField("SOURCECODE3", infocodeItem.RefRelation3);

            statement.AddField("COUNTER", infocodeItem.LineId, SqlDbType.Int);
            statement.AddField("REPLICATED", (byte)0, SqlDbType.TinyInt);

            statement.AddField("PROMPT", infocodeItem.Prompt ?? "");
            statement.AddField("PRINTPROMPTONRECEIPT", infocodeItem.PrintPromptOnReceipt ? (byte)1 : (byte)0, SqlDbType.TinyInt);
            statement.AddField("PRINTINPUTONRECEIPT", infocodeItem.PrintInputOnReceipt ? (byte)1 : (byte)0, SqlDbType.TinyInt);
            statement.AddField("PRINTINPUTNAMEONRECEIPT", infocodeItem.PrintInputNameOnReceipt ? (byte)1 : (byte)0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
