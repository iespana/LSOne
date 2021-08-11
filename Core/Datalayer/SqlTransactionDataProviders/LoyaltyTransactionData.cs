using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    /// <summary>
    /// Manages the Loyalty points data for the transaction in table RboTransactionLoyaltyTrans
    /// </summary>
    public class LoyaltyTransactionData : SqlServerDataProviderBase, ILoyaltyTransactionData
    {
        private static string BaseSQL
        {
            get
            {
                return @"  SELECT ISNULL(TRANSACTIONID, '') TRANSACTIONID,
                                    ISNULL(LINENUM, 0) LINENUM,
                                    ISNULL(RECEIPTID, '') RECEIPTID,
                                    ISNULL(POINTS, 0) POINTS,
                                    ISNULL(DATEOFISSUE, 0) DATEOFISSUE,
                                    ISNULL(STOREID, '') STOREID,
                                    ISNULL(TERMINALID, '') TERMINALID,
                                    ISNULL(CARDNUMBER, '') CARDNUMBER,                                            
                                    ISNULL(LOYALTYCUSTID, '') LOYALTYCUSTID,
                                    ISNULL(ENTRYTYPE, 0) ENTRYTYPE,
                                    ISNULL(EXPIRATIONDATE, 0) EXPIRATIONDATE,
                                    ISNULL(LOYALTYSCHEMEID, '') LOYALTYSCHEMEID,                                            
                                    ISNULL(STAFFID, '') STAFFID,
                                    ISNULL(ACCUMULATEDPOINTS, 0) ACCUMULATEDPOINTS,
                                    ISNULL(RULEID, '00000000-0000-0000-0000-000000000000') RULEID,
                                    ISNULL(RELATION, 0) RELATION
                                    FROM RBOTRANSACTIONLOYALTYPOINTTRANS "; }
        }

        /// <summary>
        /// Inserts a points line for a sale line loyalty points 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction currently being saved</param>
        /// <param name="saleLineItem">The item currently being saved</param>
        public virtual void Insert(IConnectionManager entry, PosTransaction transaction, ISaleLineItem saleLineItem)
        {
            Insert(entry, transaction, saleLineItem, null, saleLineItem.LineId, saleLineItem.LoyaltyPoints.Relation);
        }

        /// <summary>
        /// Inserts a points line for a tender line loyalty points 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction currently being saved</param>
        /// <param name="tenderLineItem">The tender line currently being saved</param>
        public virtual void Insert(IConnectionManager entry, PosTransaction transaction, ITenderLineItem tenderLineItem)
        {
            Insert(entry, transaction, null, tenderLineItem, tenderLineItem.LineId, LoyaltyPointsRelation.Tender);
        }

        /// <summary>
        /// Inserts a summary points line for the entire transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction currently being saved</param>
        public virtual void Insert(IConnectionManager entry, PosTransaction transaction)
        {
            Insert(entry, transaction, null, null, 1, LoyaltyPointsRelation.Header);
        }

        private static void Insert(IConnectionManager entry, PosTransaction transaction, ISaleLineItem saleLineItem, ITenderLineItem tenderLineItem, int lineNum, LoyaltyPointsRelation relation)
        {
            if (transaction == null || transaction.GetType() != typeof (RetailTransaction))
                return;
            
            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONLOYALTYPOINTTRANS", StatementType.Insert, false);

            // Primary key
            statement.AddKey("TRANSACTIONID", transaction.TransactionId);
            statement.AddKey("LINENUM", (decimal)lineNum, SqlDbType.Decimal);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddField("STOREID", transaction.StoreId);
            statement.AddField("TERMINALID", transaction.TerminalId);
            
            //The relation of the points lines and different points depending on which relation is being saved
            statement.AddField("RELATION", (int)relation, SqlDbType.Int);
            //PK end

            if (((RetailTransaction)transaction).LoyaltyItem.CardNumber.Length > 30)
            {
                statement.AddField("CARDNUMBER", ((RetailTransaction)transaction).LoyaltyItem.CardNumber.Substring(0, 30));
            }
            else
            {
                statement.AddField("CARDNUMBER", ((RetailTransaction)transaction).LoyaltyItem.CardNumber);
            }

            statement.AddField("RECEIPTID", transaction.ReceiptId);
            statement.AddField("LOYALTYCUSTID", (string)((RetailTransaction)transaction).LoyaltyItem.CustomerID);
            statement.AddField("ENTRYTYPE", (int)transaction.EntryStatus, SqlDbType.Int);
            statement.AddField("EXPIRATIONDATE", GetExpireDate((RetailTransaction)transaction), SqlDbType.DateTime);
            statement.AddField("DATEOFISSUE", transaction.BeginDateTime, SqlDbType.DateTime);
            statement.AddField("LOYALTYSCHEMEID", (string)((RetailTransaction)transaction).LoyaltyItem.SchemeID);
            statement.AddField("STAFFID", (string)transaction.Cashier.ID);
            statement.AddField("ACCUMULATEDPOINTS", ((RetailTransaction)transaction).LoyaltyItem.AccumulatedPoints, SqlDbType.Decimal);

            switch (relation)
            {
                case LoyaltyPointsRelation.Header:
                    statement.AddField("POINTS", ((RetailTransaction)transaction).LoyaltyItem.CalculatedPoints, SqlDbType.Decimal);
                    break;
                case LoyaltyPointsRelation.Item:
                    statement.AddField("POINTS", saleLineItem.LoyaltyPoints.CalculatedPoints, SqlDbType.Decimal);
                    statement.AddField("RULEID", (Guid)saleLineItem.LoyaltyPoints.RuleID, SqlDbType.UniqueIdentifier);
                    break;
                case LoyaltyPointsRelation.Discount:
                    statement.AddField("POINTS", saleLineItem.LoyaltyPoints.CalculatedPoints, SqlDbType.Decimal);
                    statement.AddField("RULEID", ((RetailTransaction)transaction).LoyaltyItem.RuleID.DBType != SqlDbType.UniqueIdentifier ? new Guid((string)((RetailTransaction)transaction).LoyaltyItem.RuleID) : (Guid)((RetailTransaction)transaction).LoyaltyItem.RuleID, SqlDbType.UniqueIdentifier);
                    break;
                case LoyaltyPointsRelation.Tender:
                    statement.AddField("POINTS", tenderLineItem.LoyaltyPoints.CalculatedPoints, SqlDbType.Decimal);
                    statement.AddField("RULEID", tenderLineItem.LoyaltyPoints.RuleID.DBType != SqlDbType.UniqueIdentifier ? new Guid((string)tenderLineItem.LoyaltyPoints.RuleID) : (Guid)tenderLineItem.LoyaltyPoints.RuleID, SqlDbType.UniqueIdentifier);
                    break;
            }

            entry.Connection.ExecuteStatement(statement);
        }

        private static void PopulateLoyaltyItem(IDataReader dr, LoyaltyItem item)
        {
            item.TransactionID = (string)dr["TRANSACTIONID"];
            item.LineNum = (decimal)dr["LINENUM"];
            item.ReceiptID = (string)dr["RECEIPTID"];
            item.CalculatedPoints = (decimal)dr["POINTS"];
            item.DateIssued = (DateTime)dr["DATEOFISSUE"];
            item.StoreID = (string)dr["STOREID"];
            item.TerminalID = (string)dr["TERMINALID"];
            item.CardNumber = (string)dr["CARDNUMBER"];
            item.CustomerID = (string)dr["LOYALTYCUSTID"];
            item.EntryStatus = (TransactionStatus)(int)dr["ENTRYTYPE"];
            item.ExpirationDate = (DateTime)dr["EXPIRATIONDATE"];
            item.SchemeID = (string)dr["LOYALTYSCHEMEID"];
            item.StaffID = (string)dr["STAFFID"];
            item.AccumulatedPoints = (decimal)dr["ACCUMULATEDPOINTS"];
            item.RuleID = (Guid)dr["RULEID"];
            item.Relation = (LoyaltyPointsRelation)(int)dr["RELATION"];
        }

        private static DateTime GetExpireDate(RetailTransaction retailTransaction)
        {
            var beginDate = retailTransaction.BeginDateTime;
            var expValue = retailTransaction.LoyaltyItem.ExpireValue;

            switch (retailTransaction.LoyaltyItem.ExpireUnit)
            {
                case TimeUnitEnum.Day : return beginDate.AddDays(expValue);
                case TimeUnitEnum.Month: return beginDate.AddMonths(expValue);
                case TimeUnitEnum.Year: return beginDate.AddYears(expValue);
                case TimeUnitEnum.Week: return beginDate.AddDays(expValue * 7);
                default: return beginDate;
            }
        }

        public virtual LoyaltyItem GetTransactionLoyaltyItem(IConnectionManager entry,RecordIdentifier transactionID, RetailTransaction transaction)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL +
                                 @" WHERE TRANSACTIONID = @transactionID 
                                    AND DATAAREAID = @dataAreaID
                                    AND STOREID = @storeID 
                                    AND TERMINALID = @terminalID 
                                    AND (RELATION = @relation OR RELATION = @RELATION2)
                                    ORDER BY LINENUM DESC";

                MakeParam(cmd, "transactionID", (string)transactionID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", transaction.StoreId);
                MakeParam(cmd, "terminalID", transaction.TerminalId);
                MakeParam(cmd, "relation", (int) LoyaltyPointsRelation.Header, SqlDbType.Int);
                MakeParam(cmd, "RELATION2", (int)LoyaltyPointsRelation.Tender, SqlDbType.Int);

                var result = Execute<LoyaltyItem>(entry, cmd, CommandType.Text, PopulateLoyaltyItem);
                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual List<LoyaltyItem> GetList(IConnectionManager entry, RecordIdentifier transactionId, RecordIdentifier storeId, RecordIdentifier terminalId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSQL + 
                                    @" WHERE DATAAREAID = @dataAreaID 
                                       AND TRANSACTIONID = @transactionID 
                                       AND STOREID = @storeID 
                                       AND TERMINALID = @terminalID";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", (string)transactionId);
                MakeParam(cmd, "storeID", (string)storeId);
                MakeParam(cmd, "terminalID", (string)terminalId);

                return Execute<LoyaltyItem>(entry, cmd, CommandType.Text, PopulateLoyaltyItem);
            }
        }

        public virtual LoyaltyItem Get(IConnectionManager entry, RecordIdentifier transactionId, RecordIdentifier storeId, RecordIdentifier terminalId, decimal lineNum,  LoyaltyPointsRelation relation)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSQL +
                                    @" WHERE DATAAREAID = @dataAreaID 
                                       AND TRANSACTIONID = @transactionID 
                                       AND STOREID = @storeID 
                                       AND RELATION = @relation
                                       AND LINENUM = @linenum
                                       AND TERMINALID = @terminalID";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", (string)transactionId);
                MakeParam(cmd, "storeID", (string)storeId);
                MakeParam(cmd, "terminalID", (string)terminalId);
                MakeParam(cmd, "relation", (int)relation, SqlDbType.Int);
                MakeParam(cmd, "linenum", lineNum, SqlDbType.Decimal);

                var loyaltyItems = Execute<LoyaltyItem>(entry, cmd, CommandType.Text, PopulateLoyaltyItem);

                return loyaltyItems.Count > 0 ? loyaltyItems[0] : null;
            }
        }
    }
}
