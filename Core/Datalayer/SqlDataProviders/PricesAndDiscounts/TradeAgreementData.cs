using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.SqlDataProviders.PricesAndDiscounts
{
    /// <summary>
    /// Data provider class for trade agreement entries
    /// </summary>
    public class TradeAgreementData : SqlServerDataProviderBase, ITradeAgreementData
    {
        private static List<TableColumn> selectionColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "ID", TableAlias = "T"},
            new TableColumn {ColumnName = "ITEMCODE", TableAlias = "T"},
            new TableColumn {ColumnName = "ACCOUNTCODE", TableAlias = "T"},
            new TableColumn {ColumnName = "ITEMRELATION", TableAlias = "T"},
            new TableColumn {ColumnName = "ISNULL(T.PRICEID, '')", ColumnAlias = "PRICEID"},
            new TableColumn {ColumnName = "ACCOUNTRELATION", TableAlias = "T"},
            new TableColumn {ColumnName = "QUANTITYAMOUNT", TableAlias = "T"},
            new TableColumn {ColumnName = "FROMDATE", TableAlias = "T"},
            new TableColumn {ColumnName = "TODATE", TableAlias = "T"},
            new TableColumn {ColumnName = "AMOUNT", TableAlias = "T"},
            new TableColumn {ColumnName = "CURRENCY", TableAlias = "T"},
            new TableColumn {ColumnName = "ISNULL(T.PERCENT1, 0)", ColumnAlias = "PERCENT1"},
            new TableColumn {ColumnName = "ISNULL(T.PERCENT2, 0)", ColumnAlias = "PERCENT2"},
            new TableColumn {ColumnName = "ISNULL(T.SEARCHAGAIN, 0)", ColumnAlias = "SEARCHAGAIN"},
            new TableColumn {ColumnName = "RELATION", TableAlias = "T"},
            new TableColumn {ColumnName = "UNITID", TableAlias = "T"},
            new TableColumn {ColumnName = "INVENTDIMID", TableAlias = "T"},
            new TableColumn {ColumnName = "ISNULL(AMOUNTINCLTAX, 0)",ColumnAlias = "AMOUNTINCLTAX"},
            new TableColumn {ColumnName = "ISNULL(T.MARKUP, 0)", ColumnAlias = "MARKUP"},
            new TableColumn {ColumnName = "COALESCE(CURR.TXT, '')",ColumnAlias = "CURRENCYDESCRIPTION"},
            new TableColumn {ColumnName = "COALESCE(U.TXT, '')", ColumnAlias = "UNITDESCRIPTION"},
            new TableColumn {ColumnName = "COALESCE(CU.NAME, PR.NAME, '')", ColumnAlias = "ACCOUNTNAME"},
        };

        private static List<TableColumn> extraColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "inv.ITEMNAME", ColumnAlias = "ITEMNAME"},
            new TableColumn {ColumnName = "inv.ITEMID", ColumnAlias = "ITEMID"},
            new TableColumn {ColumnName = "COALESCE(pr2.NAME,'')", ColumnAlias = "ITEMRELATIONNAME"},
            new TableColumn {ColumnName = "inv.VARIANTNAME", ColumnAlias = "VARIANTNAME"},
            new TableColumn {ColumnName = "inv.HEADERITEMID"},
        };

        private static string BaseColumnSelection
        {
            get
            {
                return @"t.ID,  
                    t.ITEMCODE,  
                    t.ACCOUNTCODE,  
                    t.ITEMRELATION,  
                    t.PRICEID,  
                    t.ACCOUNTRELATION,  
                    t.QUANTITYAMOUNT,  
                    t.FROMDATE,  
                    t.TODATE,  
                    t.AMOUNT,  
                    t.CURRENCY,  
                    ISNULL(t.PERCENT1,0) as PERCENT1,  
                    ISNULL(t.PERCENT2,0) as PERCENT2,  
                    ISNULL(t.SEARCHAGAIN,0) as SEARCHAGAIN,  
                    t.RELATION,  
                    t.UNITID,  
                    t.INVENTDIMID,  
                    ISNULL(AMOUNTINCLTAX,0) as AMOUNTINCLTAX,  
                    ISNULL(t.MARKUP,0) as MARKUP,  
                    COALESCE(curr.TXT,'') as CURRENCYDESCRIPTION,
                    COALESCE(u.TXT,'') as UNITDESCRIPTION,  
                    COALESCE(cu.NAME,pr.NAME,'') as ACCOUNTNAME ";
            }
        }

        private List<Join> fixedJoins(int relationEnum)
        {
            string priceDiscType;
            Condition priceDiscTypeCondition;
            Condition priceDiscType2Condition;
            if (relationEnum == -1)
            {
                priceDiscTypeCondition = Condition.Empty;
                priceDiscType2Condition = Condition.Empty;
            }
            else
            {
                TradeAgreementRelation relation = (TradeAgreementRelation)relationEnum;

                if (relation == TradeAgreementRelation.PriceSales)
                {
                    priceDiscType = "0";
                }
                else if (relation == TradeAgreementRelation.LineDiscSales)
                {
                    priceDiscType = "1";
                }
                else
                {
                    priceDiscType = "2"; // Multi line discount
                }

                priceDiscTypeCondition = new Condition
                {
                    ConditionValue = "pr.TYPE = " + priceDiscType,
                    Operator = "AND"
                };
                priceDiscType2Condition = new Condition
                {
                    ConditionValue = "pr2.TYPE = " + priceDiscType,
                    Operator = "AND"
                };
            }
            List<Condition> priceDisGroupConditions = new List<Condition>
            {
                new Condition {ConditionValue = "t.ACCOUNTCODE = 1", Operator = "AND"},
                new Condition {ConditionValue = "pr.MODULE = 1", Operator = "AND"},
                new Condition {ConditionValue = "pr.GROUPID = t.ACCOUNTRELATION", Operator = "AND"},
                new Condition {ConditionValue = "pr.DATAAREAID = t.DATAAREAID", Operator = "AND"},
                priceDiscTypeCondition
            };
            List<Condition> priceDisGroup2Conditions = new List<Condition>
            {
                new Condition {ConditionValue = "t.ITEMRELATION = pr2.GROUPID", Operator = "AND"},
                new Condition {ConditionValue = "t.DATAAREAID = pr2.DATAAREAID", Operator = "AND"},
                new Condition {ConditionValue = "pr2.MODULE = 0", Operator = "AND"},
                priceDiscType2Condition
            };
            var reply = new List<Join>
            {
                new Join
                {
                    Condition = " curr.CURRENCYCODE = t.CURRENCY and curr.DATAAREAID = t.DATAAREAID",
                    JoinType = "LEFT OUTER",
                    Table = "CURRENCY",
                    TableAlias = "curr"
                },
                new Join
                {
                    Condition =
                        " t.ACCOUNTCODE = 0 and cu.ACCOUNTNUM = t.ACCOUNTRELATION and cu.DATAAREAID = t.DATAAREAID ",
                    JoinType = "LEFT OUTER",
                    Table = "CUSTOMER",
                    TableAlias = "cu"
                },
                new Join
                {
                    Condition = QueryPartGenerator.ConditionGenerator(priceDisGroupConditions, true),
                    JoinType = "LEFT OUTER",
                    Table = "PRICEDISCGROUP",
                    TableAlias = "pr"
                },
                new Join
                {
                    Condition = @" inv.ITEMID = t.ITEMRELATION ",
                    JoinType = "LEFT OUTER",
                    Table = "RETAILITEM",
                    TableAlias = "inv"
                },
                new Join
                {
                    Condition = " u.UNITID = t.UNITID and u.DATAAREAID = t.DATAAREAID",
                    JoinType = "LEFT OUTER",
                    Table = "UNIT",
                    TableAlias = "u"
                },
                new Join
                {
                    Condition = QueryPartGenerator.ConditionGenerator(priceDisGroup2Conditions, true),
                    JoinType = "LEFT OUTER",
                    Table = "PRICEDISCGROUP",
                    TableAlias = "pr2"
                }

            };
            return reply;
        }

        private static void PopulateTradeAgreement(IDataReader dr, TradeAgreementEntry agreement)
        {
            agreement.ItemCode = (TradeAgreementEntry.TradeAgreementEntryItemCode)dr["ITEMCODE"];
            agreement.AccountCode = (TradeAgreementEntryAccountCode)dr["ACCOUNTCODE"];

            agreement.ID = (Guid)dr["ID"];
            agreement.ItemRelation = (string)dr["ITEMRELATION"];
            agreement.PriceID = (string)dr["PRICEID"];
            agreement.AccountRelation = (string)dr["ACCOUNTRELATION"];
            agreement.QuantityAmount = (decimal)dr["QUANTITYAMOUNT"];
            agreement.FromDate = Date.FromAxaptaDate(dr["FROMDATE"]);
            agreement.ToDate = Date.FromAxaptaDate(dr["TODATE"]);
            agreement.Amount = (decimal)dr["AMOUNT"];
            agreement.AmountIncludingTax = (decimal)dr["AMOUNTINCLTAX"];
            agreement.Currency = (string)dr["CURRENCY"];
            agreement.CurrencyDescription = (string)dr["CURRENCYDESCRIPTION"];
            agreement.Percent1 = (decimal)dr["PERCENT1"];
            agreement.Percent2 = (decimal)dr["PERCENT2"];
            agreement.SearchAgain = ((byte)dr["SEARCHAGAIN"] != 0);
            agreement.Relation = (TradeAgreementEntry.TradeAgreementEntryRelation)dr["RELATION"];
            agreement.UnitID = (string)dr["UNITID"];
            agreement.UnitDescription = (string)dr["UNITDESCRIPTION"];
            agreement.Markup = (decimal)dr["MARKUP"];
            agreement.AccountName = (string)dr["ACCOUNTNAME"];
            agreement.ItemRelationName = (string)dr["ITEMRELATIONNAME"];
        }

        private static void PopulateTradeAgreementWithVariantID(IDataReader dr, TradeAgreementEntry agreement)
        {
            PopulateTradeAgreement(dr, agreement);

            agreement.VariantName = Conversion.ToStr(dr["VARIANTNAME"]);
            agreement.IsVariantItem = !(dr["HEADERITEMID"] is DBNull);
            agreement.ItemName = Conversion.ToStr(dr["ITEMNAME"]);
            agreement.ItemID = Conversion.ToStr(dr["ITEMID"]);
        }

        private static string ResolveSort(TradeAgreementRelation relation)
        {
            if (relation == TradeAgreementRelation.PriceSales)
            {
                return " ORDER BY t.Amount ASC, t.ItemCode, t.ItemRelation, t.AccountCode," +
                       " t.AccountRelation, t.Currency , t.QuantityAmount ";
            }
            return " ORDER BY t.ItemCode, t.ItemRelation, t.AccountCode," +
                   " t.AccountRelation, t.Currency , t.QuantityAmount ";
        }

        /// <summary>
        /// Gets a trade agreement entry with the a give ID and a given type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="agreementID">The unique ID of trade agreement entry</param>
        /// <param name="relation">The type of trade agreement</param>
        public virtual TradeAgreementEntry Get(IConnectionManager entry, RecordIdentifier agreementID, TradeAgreementRelation relation)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = selectionColumns.ToList();
                columns.AddRange(extraColumns);
                List<Join> joins = fixedJoins((int)relation);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "t.DATAAREAID = @dataAreaId", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ID = @ID " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PRICEDISCTABLE ", "t"),
                   QueryPartGenerator.InternalColumnGenerator(columns),
                   QueryPartGenerator.JoinGenerator(joins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                   string.Empty);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (Guid)agreementID, SqlDbType.UniqueIdentifier);

                var results = Execute<TradeAgreementEntry>(entry, cmd, CommandType.Text,
                                                           PopulateTradeAgreementWithVariantID);

                return results.Count > 0 ? results[0] : null;
            }
        }

        /// <summary>
        /// Gets a trade agreement entry with the a give ID and a total discount type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="agreementID">The unique ID of trade agreement entry</param>
        /// <returns>A trade agreement entry with the a give ID and a total discount type</returns>
        public virtual TradeAgreementEntry GetTotalDiscount(IConnectionManager entry, RecordIdentifier agreementID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = selectionColumns.ToList();
                columns.Add(new TableColumn { ColumnName = "COALESCE(pr.NAME,'')", ColumnAlias = "ITEMRELATIONNAME", });
                List<Join> joins = new List<Join>();
                joins.Add(new Join { Table = "CURRENCY", Condition = "curr.CURRENCYCODE = t.CURRENCY and curr.DATAAREAID = t.DATAAREAID", TableAlias = "curr", JoinType = "left outer" });
                joins.Add(new Join { Table = "CUSTOMER", Condition = "t.ACCOUNTCODE = 0 and cu.ACCOUNTNUM = t.ACCOUNTRELATION and cu.DATAAREAID = t.DATAAREAID", TableAlias = "cu", JoinType = "left outer" });
                joins.Add(new Join { Table = "PRICEDISCGROUP", Condition = "t.ACCOUNTCODE = 1 and pr.MODULE = 1 and pr.TYPE = 3 and pr.GROUPID = t.ACCOUNTRELATION and pr.DATAAREAID = t.DATAAREAID", TableAlias = "pr", JoinType = "left outer" });
                joins.Add(new Join { Table = "UNIT", Condition = "u.UNITID = t.UNITID and u.DATAAREAID = t.DATAAREAID", TableAlias = "u", JoinType = "left outer" });

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "t.DATAAREAID = @dataAreaId", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ID = @ID " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PRICEDISCTABLE ", "t"),
                   QueryPartGenerator.InternalColumnGenerator(columns),
                   QueryPartGenerator.JoinGenerator(joins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                   string.Empty);


                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (Guid)agreementID, SqlDbType.UniqueIdentifier);

                var results = Execute<TradeAgreementEntry>(entry, cmd, CommandType.Text, PopulateTradeAgreement);

                return results.Count > 0 ? results[0] : null;
            }
        }

        

        /// <summary>
        /// Gets all trade agreement entries for an item that have a specific trade agreement type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="relation">The type of trade agreement entries to get</param>
        /// <param name="accountRelation">Search for specific account relation</param>
        /// <param name="priceCustomerItem">Include or exclude the specific account relation</param>
        /// <returns>A list of trade agreement entries for an item and a trade agreement type</returns>
        ///
        public List<TradeAgreementEntry> GetForItem(IConnectionManager entry, RecordIdentifier itemID, TradeAgreementRelation relation, bool priceCustomerItem, RecordIdentifier accountRelation)
        {
            ValidateSecurity(entry);
            RecordIdentifier targetID = RecordIdentifier.Empty;
            if (!itemID.IsGuid)
            {
                targetID = GetMasterID(entry, itemID, "RETAILITEM", "ITEMID");
            }
            else
            {
                targetID = itemID;
            }
            string sort = ResolveSort(relation);
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = selectionColumns.ToList();
                columns.AddRange(extraColumns);
                List<Join> joins = fixedJoins((int)relation);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "t.DATAAREAID = @dataAreaId", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ITEMCODE = 0 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "inv.masterid = @itemID" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.RELATION = @relation " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTCODE <> 1 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "(t.ACCOUNTCODE = 2 OR t.ACCOUNTRELATION = @accountRelation)" });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PRICEDISCTABLE ", "t"),
                 QueryPartGenerator.InternalColumnGenerator(columns),
                 QueryPartGenerator.JoinGenerator(joins),
                 QueryPartGenerator.ConditionGenerator(conditions),
                 sort);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (Guid)targetID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "relation", (int)relation, SqlDbType.Int);
                MakeParam(cmd, "accountRelation", priceCustomerItem ? accountRelation.StringValue : "");

                return Execute<TradeAgreementEntry>(entry, cmd, CommandType.Text, PopulateTradeAgreementWithVariantID);
            }
        }

        /// <summary>
        /// Gets all trade agreement entries for an item that have a specific trade agreement type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="relation">The type of trade agreement entries to get</param>
        /// <returns>A list of trade agreement entries for an item and a trade agreement type</returns>
        ///
        public List<TradeAgreementEntry> GetForItem(
            IConnectionManager entry,
            RecordIdentifier itemID,
            TradeAgreementRelation relation)
        {
            ValidateSecurity(entry);
            RecordIdentifier targetID = RecordIdentifier.Empty;
            if (!itemID.IsGuid)
            {
                targetID = GetMasterID(entry, itemID, "RETAILITEM", "ITEMID");
            }
            else
            {
                targetID = itemID;
            }
            string sort = ResolveSort(relation);
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = selectionColumns.ToList();
                columns.AddRange(extraColumns);
                List<Join> joins = fixedJoins((int)relation);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "t.DATAAREAID = @dataAreaId", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ITEMCODE = 0 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "inv.masterid =  @itemID" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.RELATION = @relation " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PRICEDISCTABLE ", "t"),
                 QueryPartGenerator.InternalColumnGenerator(columns),
                 QueryPartGenerator.JoinGenerator(joins),
                 QueryPartGenerator.ConditionGenerator(conditions),
                 sort);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (Guid)targetID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "relation", (int)relation, SqlDbType.Int);

                return Execute<TradeAgreementEntry>(entry, cmd, CommandType.Text, PopulateTradeAgreementWithVariantID);
            }
        }

        /// <summary>
        /// Gets all trade agreement entries for an item for all trade agreement type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="lineDiscountGroupID">The unique ID of the line discount group this item belongs to</param>
        /// <param name="multilineDiscountGroupID">The unique ID of the multiline discount group this item belongs to</param>
        /// <returns>A list of trade agreement entries for an item</returns>
        public List<TradeAgreementEntry> GetForItemAndItemGroups(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier lineDiscountGroupID,
            RecordIdentifier multilineDiscountGroupID)
        {
            RecordIdentifier targetID = RecordIdentifier.Empty;
            if (!itemID.IsGuid)
            {
                targetID = GetMasterID(entry, itemID, "RETAILITEM", "ITEMID");
            }
            else
            {
                targetID = itemID;
            }
            using (var cmd = entry.Connection.CreateCommand())
            {

                ValidateSecurity(entry);
                List<TableColumn> columns = selectionColumns.ToList();
                columns.AddRange(extraColumns);
                List<Join> joins = fixedJoins(-1);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "t.DATAAREAID = @dataAreaId", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "(t.RELATION = 5 OR t.RELATION = 6) " });

                List<Condition> codeConditions = new List<Condition>();
                codeConditions.Add(new Condition
                {
                    ConditionValue = "(t.ITEMCODE = 0 AND inv.masterid = @itemID)",
                    Operator = "OR"
                });
                codeConditions.Add(new Condition
                {
                    ConditionValue = "(t.ITEMCODE = 1 AND t.ITEMRELATION = @lineID)",
                    Operator = "OR"
                });
                codeConditions.Add(new Condition
                {
                    ConditionValue = "(t.ITEMCODE = 1 AND t.ITEMRELATION = @multiID)",
                    Operator = "OR"
                });
                codeConditions.Add(new Condition { ConditionValue = "(t.ITEMCODE = 2)", Operator = "OR" });


                conditions.Add(new Condition
                {
                    ConditionValue = $"({QueryPartGenerator.ConditionGenerator(codeConditions, true)})",
                    Operator = "AND"
                });
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PRICEDISCTABLE ", "t"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (Guid)targetID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "lineID", (string)lineDiscountGroupID);
                MakeParam(cmd, "multiID", (string)multilineDiscountGroupID);

                return Execute<TradeAgreementEntry>(entry, cmd, CommandType.Text, PopulateTradeAgreementWithVariantID);
            }
        }

        /// <summary>
        /// Gets all trade agreements for a given item discount group that have a specific
        /// trade agreement type 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemDiscountGroupID">The unique ID of the item discount
        /// group</param>
        /// <param name="relation">The type of trade agreement entries to get</param>
        /// <returns>All trade agreements for a given item discount group that have a specific
        /// trade agreement type</returns>
        public List<TradeAgreementEntry> GetForItemDiscountGroup(
            IConnectionManager entry,
            RecordIdentifier itemDiscountGroupID,
            TradeAgreementRelation relation)
        {
            string sort = ResolveSort(relation);
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                List<TableColumn> columns = selectionColumns.ToList();
                columns.AddRange(extraColumns);
                List<Join> joins = fixedJoins((int)relation);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "t.DATAAREAID = @dataAreaId", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ITEMCODE = 1 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ITEMRELATION = @itemDiscountGroupID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.RELATION = @relation " });


                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PRICEDISCTABLE ", "t"),
                 QueryPartGenerator.InternalColumnGenerator(columns),
                 QueryPartGenerator.JoinGenerator(joins),
                 QueryPartGenerator.ConditionGenerator(conditions),
                 sort);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemDiscountGroupID", (string)itemDiscountGroupID);
                MakeParam(cmd, "relation", (int)relation, SqlDbType.Int);

                return Execute<TradeAgreementEntry>(entry, cmd, CommandType.Text, PopulateTradeAgreementWithVariantID);
            }
        }

        /// <summary>
        /// Gets all trade agreements for a given customer that have a specific
        /// trade agreement type 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">The unique ID of the customer</param>
        /// <param name="relation">The type of trade agreement entries to get</param>
        /// <param name="allCustomers">Explicitly include or exclude agreements that apply to all customers</param>
        /// <returns>All trade agreements for a given customer that have a specific trade agreement type</returns>
        public List<TradeAgreementEntry> GetForCustomer(
            IConnectionManager entry,
            RecordIdentifier customerID,
            TradeAgreementRelation relation,
            bool? allCustomers = null)
        {
            ValidateSecurity(entry);
            string sort = ResolveSort(relation);
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = selectionColumns.ToList();
                columns.AddRange(extraColumns);
                List<Join> joins = fixedJoins((int)relation);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "t.DATAAREAID = @dataAreaId", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTCODE = 0 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTRELATION = @customerID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.RELATION = @relation " });

                if (allCustomers != null && allCustomers.Value)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ITEMCODE = 2 " });
                }
                else if (allCustomers != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = " t.ITEMCODE != 2 " });
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PRICEDISCTABLE ", "t"),
                 QueryPartGenerator.InternalColumnGenerator(columns),
                 QueryPartGenerator.JoinGenerator(joins),
                 QueryPartGenerator.ConditionGenerator(conditions),
                 sort);


                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "customerID", (string)customerID);
                MakeParam(cmd, "relation", (int)relation, SqlDbType.Int);

                return Execute<TradeAgreementEntry>(entry, cmd, CommandType.Text, PopulateTradeAgreementWithVariantID);
            }
        }

        /// <summary>
        /// Gets all trade agreements for a given customer that have a specific
        /// trade agreement type 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">The unique ID of the customer</param>
        /// <param name="groupID">The unique ID of the group the customer is in</param>
        /// <param name="relation">The type of trade agreement entries to get</param>
        /// <returns>All trade agreements for a given customer and customer group that customer is in that have a specific trade agreement type</returns>
        public List<TradeAgreementEntry> GetForCustomerAndGroup(
            IConnectionManager entry,
            RecordIdentifier customerID,
            RecordIdentifier groupID,
            TradeAgreementRelation relation)
        {
            ValidateSecurity(entry);
            string sort = ResolveSort(relation);
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = selectionColumns.ToList();
                columns.AddRange(extraColumns);
                List<Join> joins = fixedJoins((int)relation);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "t.DATAAREAID = @dataAreaId", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTCODE = 0 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTRELATION = @customerID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.RELATION = @relation " });

                conditions.Add(new Condition { Operator = "OR", ConditionValue = "t.ACCOUNTCODE = 1 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTRELATION = @groupID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.RELATION = @relation " });

                if (relation == TradeAgreementRelation.LineDiscSales ||
                    relation == TradeAgreementRelation.MultiLineDiscSales)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "OR",
                        ConditionValue = "t.ACCOUNTCODE = 2 and t.RELATION = @relation "
                    });
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PRICEDISCTABLE ", "t"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    sort);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "customerID", (string)customerID);
                MakeParam(cmd, "groupID", (string)groupID);
                MakeParam(cmd, "relation", (int)relation, SqlDbType.Int);

                return Execute<TradeAgreementEntry>(entry, cmd, CommandType.Text, PopulateTradeAgreementWithVariantID);
            }
        }

        /// <summary>
        /// Gets all total discount trade agreements for a given customer 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">The unique ID of the customer</param>
        /// <returns>All total discount trade agreements for a given customer </returns>
        public List<TradeAgreementEntry> GetTotalDiscForCustomer(
            IConnectionManager entry,
            RecordIdentifier customerID)
        {
            string sort = ResolveSort(TradeAgreementRelation.TotalDiscount);
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> columns = selectionColumns.ToList();
                columns.Add(new TableColumn { ColumnName = "COALESCE(pr.NAME,'')", ColumnAlias = "ITEMRELATIONNAME", });
                List<Join> joins = new List<Join>();
                joins.Add(new Join { Table = "CURRENCY", Condition = "curr.CURRENCYCODE = t.CURRENCY and curr.DATAAREAID = t.DATAAREAID", TableAlias = "curr", JoinType = "left outer" });
                joins.Add(new Join { Table = "CUSTOMER", Condition = "t.ACCOUNTCODE = 0 and cu.ACCOUNTNUM = t.ACCOUNTRELATION and cu.DATAAREAID = t.DATAAREAID", TableAlias = "cu", JoinType = "left outer" });
                joins.Add(new Join { Table = "PRICEDISCGROUP", Condition = "t.ACCOUNTCODE = 1 and pr.MODULE = 1 and pr.TYPE = 3 and pr.GROUPID = t.ACCOUNTRELATION and pr.DATAAREAID = t.DATAAREAID", TableAlias = "pr", JoinType = "left outer" });
                joins.Add(new Join { Table = "UNIT", Condition = "u.UNITID = t.UNITID and u.DATAAREAID = t.DATAAREAID", TableAlias = "u", JoinType = "left outer" });

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "t.DATAAREAID = @dataAreaId", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTCODE = 0 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTRELATION = @customerID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.RELATION = 7 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PRICEDISCTABLE ", "t"),
                   QueryPartGenerator.InternalColumnGenerator(columns),
                   QueryPartGenerator.JoinGenerator(joins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                   sort);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "customerID", (string)customerID);

                return Execute<TradeAgreementEntry>(entry, cmd, CommandType.Text, PopulateTradeAgreement);
            }
        }

        /// <summary>
        /// Gets all total discount trade agreements for a given customer group 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupID">The unique ID of the customer group</param>
        /// <returns>All total discount trade agreements for a given customer group</returns>
        public List<TradeAgreementEntry> GetTotalDiscForGroup(
            IConnectionManager entry,
            RecordIdentifier groupID)
        {
            string sort = ResolveSort(TradeAgreementRelation.TotalDiscount);
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                List<TableColumn> columns = selectionColumns.ToList();
                columns.Add(new TableColumn { ColumnName = "COALESCE(pr.NAME,'')", ColumnAlias = "ITEMRELATIONNAME", });
                List<Join> joins = new List<Join>();
                joins.Add(new Join { Table = "CURRENCY", Condition = "curr.CURRENCYCODE = t.CURRENCY and curr.DATAAREAID = t.DATAAREAID", TableAlias = "curr", JoinType = "left outer" });
                joins.Add(new Join { Table = "CUSTOMER", Condition = "t.ACCOUNTCODE = 0 and cu.ACCOUNTNUM = t.ACCOUNTRELATION and cu.DATAAREAID = t.DATAAREAID", TableAlias = "cu", JoinType = "left outer" });
                joins.Add(new Join { Table = "PRICEDISCGROUP", Condition = "t.ACCOUNTCODE = 1 and pr.MODULE = 1 and pr.TYPE = 3 and pr.GROUPID = t.ACCOUNTRELATION and pr.DATAAREAID = t.DATAAREAID", TableAlias = "pr", JoinType = "left outer" });
                joins.Add(new Join { Table = "UNIT", Condition = "u.UNITID = t.UNITID and u.DATAAREAID = t.DATAAREAID", TableAlias = "u", JoinType = "left outer" });

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "t.DATAAREAID = @dataAreaId", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTCODE = 1 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTRELATION = @groupID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.RELATION = 7 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PRICEDISCTABLE ", "t"),
                   QueryPartGenerator.InternalColumnGenerator(columns),
                   QueryPartGenerator.JoinGenerator(joins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                   sort);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "groupID", (string)groupID);

                return Execute<TradeAgreementEntry>(entry, cmd, CommandType.Text, PopulateTradeAgreement);
            }
        }

        /// <summary>
        /// Gets all total discount trade agreements for a given customer 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">The unique ID of the customer</param>
        /// <param name="groupID">The unique ID of the group the customer is in</param>
        /// <returns>All total discount trade agreements for a given customer </returns>
        public List<TradeAgreementEntry> GetTotalDiscForCustomer(
            IConnectionManager entry,
            RecordIdentifier customerID,
            RecordIdentifier groupID)
        {
            string sort = ResolveSort(TradeAgreementRelation.TotalDiscount);
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                List<TableColumn> columns = selectionColumns.ToList();
                columns.Add(new TableColumn { ColumnName = "COALESCE(pr.NAME,'')", ColumnAlias = "ITEMRELATIONNAME", });
                List<Join> joins = new List<Join>();
                joins.Add(new Join { Table = "CURRENCY", Condition = "curr.CURRENCYCODE = t.CURRENCY and curr.DATAAREAID = t.DATAAREAID", TableAlias = "curr", JoinType = "left outer" });
                joins.Add(new Join { Table = "CUSTOMER", Condition = "t.ACCOUNTCODE = 0 and cu.ACCOUNTNUM = t.ACCOUNTRELATION and cu.DATAAREAID = t.DATAAREAID", TableAlias = "cu", JoinType = "left outer" });
                joins.Add(new Join { Table = "PRICEDISCGROUP", Condition = "t.ACCOUNTCODE = 1 and pr.MODULE = 1 and pr.TYPE = 3 and pr.GROUPID = t.ACCOUNTRELATION and pr.DATAAREAID = t.DATAAREAID", TableAlias = "pr", JoinType = "left outer" });
                joins.Add(new Join { Table = "UNIT", Condition = "u.UNITID = t.UNITID and u.DATAAREAID = t.DATAAREAID", TableAlias = "u", JoinType = "left outer" });

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "t.DATAAREAID = @dataAreaId", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTCODE = 0 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTRELATION = @customerID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.RELATION = 7 " });

                conditions.Add(new Condition { Operator = "OR", ConditionValue = "t.ACCOUNTCODE = 1 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTRELATION = @groupID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.RELATION = 7 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PRICEDISCTABLE ", "t"),
                  QueryPartGenerator.InternalColumnGenerator(columns),
                  QueryPartGenerator.JoinGenerator(joins),
                  QueryPartGenerator.ConditionGenerator(conditions),
                  sort);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "customerID", (string)customerID);
                MakeParam(cmd, "groupID", (string)groupID);

                return Execute<TradeAgreementEntry>(entry, cmd, CommandType.Text, PopulateTradeAgreement);
            }
        }

        /// <summary>
        /// Gets all trade agreements for a given customer group that have a specific
        /// trade agreement type 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupID">The unique ID of customer group</param>
        /// <param name="relation">The type of trade agreement entries to get</param>
        /// <param name="itemID">Optional if the aggrements retuned should be limited to a single item</param>
        /// <returns>All trade agreements for a given customer group that have a specific
        /// trade agreement type </returns>
        public List<TradeAgreementEntry> GetForGroup(
            IConnectionManager entry,
            RecordIdentifier groupID,
            TradeAgreementRelation relation,
            RecordIdentifier itemID = null)
        {
            string sort = ResolveSort(relation);
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);


                List<TableColumn> columns = selectionColumns.ToList();
                columns.AddRange(extraColumns);
                List<Join> joins = fixedJoins((int)relation);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "t.DATAAREAID = @dataAreaId", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTCODE = 1 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.ACCOUNTRELATION = @groupID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "t.RELATION = @relation " });

                if (itemID != null && itemID != RecordIdentifier.Empty)
                {
                    RecordIdentifier targetID = RecordIdentifier.Empty;
                    if (!itemID.IsGuid)
                    {
                        targetID = GetMasterID(entry, itemID, "RETAILITEM", "ITEMID");
                    }
                    else
                    {
                        targetID = itemID;
                    }

                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "inv.masterid =  @itemID"
                    });

                    MakeParam(cmd, "itemID", (Guid)targetID, SqlDbType.UniqueIdentifier); ;
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PRICEDISCTABLE ", "t"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    sort);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "groupID", (string)groupID);
                MakeParam(cmd, "relation", (int)relation, SqlDbType.Int);

                return Execute<TradeAgreementEntry>(entry, cmd, CommandType.Text, PopulateTradeAgreementWithVariantID);
            }
        }

        /// <summary>
        /// Returns true if a trade agreement entry exists with the given oldeAgreementID ID (excludedNewAgreementID ID is excluded). Used when updating a trade agreement 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="oldAgreementID">The old trade agreement ID, which had 10 primary keys (see OldID in TradeAgreementEntry class)</param>
        /// <param name="excludedNewAgreementID">The unique ID of the trade agreement to exlucde</param>
        /// <returns>True if a trade agreement entry exists with the given oldeAgreementID ID (excludedNewAgreementID ID is excluded)</returns>
        public virtual bool DataContentExists(IConnectionManager entry, RecordIdentifier oldAgreementID, RecordIdentifier excludedNewAgreementID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select 'x' from PRICEDISCTABLE " +
                    "Where DATAAREAID = @dataAreaID " +
                    "And ITEMCODE = @itemCode " +
                    "And ACCOUNTCODE = @accountCode " +
                    "And ITEMRELATION = @itemRelation " +
                    "And ACCOUNTRELATION = @accountRelation " +
                    "And QUANTITYAMOUNT = @quantityAmount " +
                    "And FROMDATE = @fromDate " +
                    "And CURRENCY = @currency " +
                    "And RELATION = @relation " +
                    "And UNITID = @unitID " +
                    "And ID <> @ID";


                MakeParam(cmd, "itemCode", (int)oldAgreementID[0], SqlDbType.Int);
                MakeParam(cmd, "accountCode", (int)oldAgreementID[1], SqlDbType.Int);
                MakeParam(cmd, "itemRelation", (string)oldAgreementID[2]);
                MakeParam(cmd, "quantityAmount", (decimal)oldAgreementID[3], SqlDbType.Decimal);
                MakeParam(cmd, "fromDate", ((Date)oldAgreementID[5]).ToAxaptaSQLDate(), SqlDbType.DateTime);
                MakeParam(cmd, "currency", (string)oldAgreementID[6]);
                MakeParam(cmd, "unitID", (string)oldAgreementID[8]);
                MakeParam(cmd, "relation", (int)oldAgreementID[7], SqlDbType.Int);
                MakeParam(cmd, "accountRelation", (string)oldAgreementID[4]);
                MakeParam(cmd, "ID", (Guid)excludedNewAgreementID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        return true;
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if a trade agreement entry exists with the given oldeAgreementID ID. Used when creating a new trade agreement
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="oldAgreementID">The old trade agreement ID, which had 10 primary keys (see OldID in TradeAgreementEntry class)</param>
        /// <returns>Returns true if a trade agreement entry exists with the given oldeAgreementID ID</returns>
        public virtual bool DataContentExists(IConnectionManager entry, RecordIdentifier oldAgreementID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select 'x' from PRICEDISCTABLE " +
                                  "Where DATAAREAID = @dataAreaID " +
                                  "And ITEMCODE = @itemCode " +
                                  "And ACCOUNTCODE = @accountCode " +
                                  "And ITEMRELATION = @itemRelation " +
                                  "And ACCOUNTRELATION = @accountRelation " +
                                  "And QUANTITYAMOUNT = @quantityAmount " +
                                  "And FROMDATE = @fromDate " +
                                  "And CURRENCY = @currency " +
                                  "And RELATION = @relation " +
                                  "And UNITID = @unitID ";


                MakeParam(cmd, "itemCode", (int)oldAgreementID[0], SqlDbType.Int);
                MakeParam(cmd, "accountCode", (int)oldAgreementID[1], SqlDbType.Int);
                MakeParam(cmd, "itemRelation", (string)oldAgreementID[2]);
                MakeParam(cmd, "quantityAmount", (decimal)oldAgreementID[3], SqlDbType.Decimal);
                MakeParam(cmd, "fromDate", ((Date)oldAgreementID[5]).ToAxaptaSQLDate(), SqlDbType.DateTime);
                MakeParam(cmd, "currency", (string)oldAgreementID[6]);
                MakeParam(cmd, "unitID", (string)oldAgreementID[8]);
                MakeParam(cmd, "relation", (int)oldAgreementID[7], SqlDbType.Int);
                MakeParam(cmd, "accountRelation", (string)oldAgreementID[4]);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        return true;
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="agreementID">The unique ID of the trade agreement entry</param>
        /// <remarks></remarks>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier agreementID)
        {
            if (RecordIdentifier.IsEmptyOrNull(agreementID)) return false;

            if (agreementID.IsGuid)
            {
                return RecordExists(entry,
                    "PRICEDISCTABLE",
                    "ID",
                    agreementID);
            }
            else
            {
                return RecordExists(entry,
                    "PRICEDISCTABLE",
                    "PRICEID",
                    agreementID);
            }
        }

        /// <summary>
        /// Deletes a tradeagreement record based on the main ID.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="agreementID">The unique ID of the trade agreement</param>
        /// <param name="permission">Permission string use to validate user permission. 
        /// Use BusinessObjects.Permisson.ManageTradeAgreementPrices for price trade agreements or
        /// BusinessObjects.Permisson.ManageDiscounts for discount trade agreements</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier agreementID, string permission)
        {
            DeleteRecord(entry,
                "PRICEDISCTABLE",
                "ID",
                agreementID,
                permission);
        }

        /// <summary>
        /// Deletes a tradeagreement record based on all the logical keys, not based on the main ID.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="agreement">TradeAgreementEntry data entity, the price fields or main ID do not need to be populated</param>
        public virtual void Delete(IConnectionManager entry, TradeAgreementEntry agreement)
        {
            RecordIdentifier id = new RecordIdentifier(
                (int)agreement.ItemCode,
                (int)agreement.AccountCode,
                agreement.ItemRelation,
                agreement.AccountRelation,
                agreement.QuantityAmount,
                agreement.FromDate,
                agreement.Currency,
                (int)agreement.Relation,
                agreement.UnitID);

            DeleteRecord(entry, "PRICEDISCTABLE", new string[] { "ITEMCODE", "ACCOUNTCODE", "ITEMRELATION", "ACCOUNTRELATION", "QUANTITYAMOUNT", "FROMDATE", "CURRENCY", "RELATION", "UNITID" }, id, Permission.ManageTradeAgreementPrices);
        }

        /// <summary>
        /// Gets tradeagreement id for logical keys of a tradeagreement ID. This is used for example in multiediting
        /// to know if we need to update or add a record.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="agreement"></param>
        /// <returns></returns>
        public virtual RecordIdentifier GetTradeAgreementID(IConnectionManager entry, TradeAgreementEntry agreement)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select ID from PRICEDISCTABLE where 
                                    ITEMCODE = @itemCode and ACCOUNTCODE = @accountCode and ITEMRELATION = @itemRelation and ACCOUNTRELATION = @accountRelation and 
                                    QUANTITYAMOUNT = @qtyAmount and FROMDATE = @fromDate and CURRENCY = @currency and RELATION = @releation and UNITID = @unitID";

                MakeParam(cmd, "itemCode", (int)agreement.ItemCode, SqlDbType.Int);
                MakeParam(cmd, "accountCode", (int)agreement.AccountCode, SqlDbType.Int);
                MakeParam(cmd, "itemRelation", (string)agreement.ItemRelation);
                MakeParam(cmd, "accountRelation", (string)agreement.AccountRelation);
                MakeParam(cmd, "qtyAmount", agreement.QuantityAmount, SqlDbType.Decimal);
                MakeParam(cmd, "fromDate", agreement.FromDate.ToAxaptaSQLDate(false), SqlDbType.DateTime);
                MakeParam(cmd, "currency", (string)agreement.Currency);
                MakeParam(cmd, "releation", (int)agreement.Relation, SqlDbType.Int);
                MakeParam(cmd, "unitID", (string)agreement.UnitID);

                object result = entry.Connection.ExecuteScalar(cmd);

                if (result == null || result is DBNull)
                {
                    return RecordIdentifier.Empty;
                }

                return (RecordIdentifier)(Guid)result;
            }
        }

        /// <summary>
        /// Saves the given class to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="agreement">The trade agreement to save</param>
        /// <param name="permission">Permission string use to validate user permission. 
        /// Use BusinessObjects.Permisson.ManageTradeAgreementPrices for price trade agreements or
        /// BusinessObjects.Permisson.ManageDiscounts for discount trade agreements</param>
        public virtual void Save(IConnectionManager entry, TradeAgreementEntry agreement, string permission)
        {
            var statement = new SqlServerStatement("PRICEDISCTABLE");

            ValidateSecurity(entry, permission);

            statement.UpdateColumnOptimizer = agreement;

            var originalPriceIDIsEmpty = RecordIdentifier.IsEmptyOrNull(agreement.PriceID);
            var originalIDIsEmpty = RecordIdentifier.IsEmptyOrNull(agreement.ID);

            if (originalPriceIDIsEmpty && originalIDIsEmpty)
            {
                agreement.PriceID = DataProviderFactory.Instance.GenerateNumber<ITradeAgreementData, TradeAgreementEntry>(entry);
                agreement.ID = Guid.NewGuid();
            }
            else if (!originalPriceIDIsEmpty && originalIDIsEmpty)
            {
                agreement.ID = GetID(entry, agreement.PriceID);

                if (RecordIdentifier.IsEmptyOrNull(agreement.ID))
                {
                    agreement.ID = Guid.NewGuid();
                }
            }
            else if (originalPriceIDIsEmpty && !originalIDIsEmpty)
            {
                agreement.PriceID = GetPriceID(entry, agreement.ID);

                if (RecordIdentifier.IsEmptyOrNull(agreement.PriceID))
                {
                    agreement.PriceID = DataProviderFactory.Instance.GenerateNumber<ITradeAgreementData, TradeAgreementEntry>(entry);
                }
            }

            if ((originalPriceIDIsEmpty && originalIDIsEmpty) || !Exists(entry, agreement.PriceID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)agreement.ID, SqlDbType.UniqueIdentifier);
                statement.AddKey("PRICEID", (string)agreement.PriceID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)agreement.ID, SqlDbType.UniqueIdentifier);
                statement.AddCondition("PRICEID", (string)agreement.PriceID);
            }

            statement.AddField("ITEMCODE", (int)agreement.ItemCode, SqlDbType.Int);
            statement.AddField("ACCOUNTCODE", (int)agreement.AccountCode, SqlDbType.Int);
            statement.AddField("ITEMRELATION", (string)agreement.ItemRelation);
            statement.AddField("ACCOUNTRELATION", (string)agreement.AccountRelation);
            statement.AddField("QUANTITYAMOUNT", agreement.QuantityAmount, SqlDbType.Decimal);
            statement.AddField("FROMDATE", agreement.FromDate.ToAxaptaSQLDate(false), SqlDbType.DateTime);
            statement.AddField("CURRENCY", (string)agreement.Currency);
            statement.AddField("UNITID", (string)agreement.UnitID);
            statement.AddField("INVENTDIMID", ""); // No longer used
            statement.AddField("RELATION", (int)agreement.Relation, SqlDbType.Int);
            statement.AddField("TODATE", agreement.ToDate.ToAxaptaSQLDate(false), SqlDbType.DateTime);
            statement.AddField("AMOUNT", agreement.Amount, SqlDbType.Decimal);
            statement.AddField("AMOUNTINCLTAX", agreement.AmountIncludingTax, SqlDbType.Decimal);
            statement.AddField("PERCENT1", agreement.Percent1, SqlDbType.Decimal);
            statement.AddField("PERCENT2", agreement.Percent2, SqlDbType.Decimal);
            statement.AddField("SEARCHAGAIN", agreement.SearchAgain ? 1 : 0, SqlDbType.TinyInt);

            statement.AddField("MARKUP", agreement.Markup, SqlDbType.Decimal);
            statement.AddField("MODULE", 1, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Gets all the items from the database matching the list of <see cref="itemsToCompare" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items <see cref="itemsToCompare"</param>
        /// <returns>List of items</returns>
        public virtual List<TradeAgreementEntry> GetCompareList(IConnectionManager entry, List<TradeAgreementEntry> itemsToCompare)
        {
            var columns = selectionColumns.ToList();
            columns.AddRange(extraColumns);

            var res = new List<TradeAgreementEntry>();

            res.AddRange(GetCompareListInBatches(entry,
                itemsToCompare.Where(x => x.ID != RecordIdentifier.Empty).ToList(),
                "PRICEDISCTABLE", "ID", columns, fixedJoins(-1), PopulateTradeAgreement));
            // GetCompareListInBatches() gets the items from the database in batches and in an optimized way
            // The optimization that we care about here is that this method removes all redundant parameters as follows:
            // The WHERE clause is formed this way: "Column1 IN (Val1, Val2, Val3)"
            // If Val1 = Val2 = Val3, then the final WHERE clause looks like "Column1 IN (Val1)" (Val2 and Val3 are removed from the list in order to speed up the query)
            // In order to help this method to run in an optimized way, we sort the list based on some columns;
            // This way we take the maximum advantage of removing the redundant parameters.
            // Eg. Let's suppose we have 2 batches of 6 elements, with param values 1, 2, 1, 2, 1 and 2
            // In this case, if we don't sort, we would have two batches with the where clauses "Col1 in (1, 2)" and "Col1 in (2, 1)" (pretty optimized, but we can do more)
            // But since we sort the values, now we have "Col1 in (1)" and "Col1 in (2)" (now we removed even more redundant parameters)
            res.AddRange(GetCompareListInBatches(entry,
                itemsToCompare.Where(x => x.ID == RecordIdentifier.Empty)
                              .OrderBy(x => x.ItemCode)
                              .ThenBy(x => x.AccountCode)
                              .ThenBy(x => x.Relation)
                              .ThenBy(x => x.Currency)
                              .ThenBy(x => x.UnitID)
                              .ThenBy(x => x.FromDate)
                              .ToList(),
                "PRICEDISCTABLE",
                new string[] { "ITEMCODE", "ACCOUNTCODE", "ITEMRELATION", "QUANTITYAMOUNT", "ACCOUNTRELATION", "FROMDATE", "CURRENCY", "RELATION", "UNITID" },
                columns, fixedJoins(-1), PopulateTradeAgreement));

            return res;
        }

        public bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "PRICEDISCTABLE", "PRICEID", sequenceFormat, startingRecord, numberOfRecords);
        }

        public RecordIdentifier SequenceID => "PRICEID";

        private Guid GetID(IConnectionManager entry, RecordIdentifier priceID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT ID FROM PRICEDISCTABLE WHERE PriceID = @PriceID";

                MakeParam(cmd, "PriceID", (string)priceID);

                object result = entry.Connection.ExecuteScalar(cmd);

                return (result is DBNull || result == null) ? Guid.Empty : (Guid)result;
            }
        }

        private string GetPriceID(IConnectionManager entry, RecordIdentifier ID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT PRICEID FROM PRICEDISCTABLE WHERE ID = @ID";

                MakeParam(cmd, "ID", (Guid)ID, SqlDbType.UniqueIdentifier);

                object result = entry.Connection.ExecuteScalar(cmd);

                return (result is DBNull) ? null : (string)result;
            }
        }
    }
}