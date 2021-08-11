using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses.Forecourt;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    internal class DiscountData : SqlServerDataProviderBase, IDiscountData
    {
        /// <summary>
        /// Gets the discount data.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="relation">The relation (line,mulitline,total)</param>
        /// <param name="itemRelation">The item relation</param>
        /// <param name="accountRelation">The account relation</param>
        /// <param name="itemCode">The item code (table,group,all)</param>
        /// <param name="accountCode">The account code(table,group,all)</param>
        /// <param name="quantityAmount">The quantity or amount that sets the minimum quantity or amount needed</param>
        /// <param name="storeCurrencyCode">The store currency</param>
        /// <param name="itemID"></param>
        /// <param name="unitID"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        public DataTable GetPriceDiscData(IConnectionManager entry, 
                                                 PriceDiscType relation, 
                                                 string itemRelation, 
                                                 string accountRelation, 
                                                 int itemCode, 
                                                 int accountCode, 
                                                 decimal quantityAmount, 
                                                 string storeCurrencyCode, 
                                                 RecordIdentifier itemID,
                                                 RecordIdentifier unitID,
                                                 CacheType cache = CacheType.CacheTypeNone)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (accountRelation == null) { accountRelation = ""; }
            if (itemRelation == null) { itemRelation = ""; }
            if (storeCurrencyCode == null) { storeCurrencyCode = ""; }
            IDataReader dr = null;
            RecordIdentifier id = null;
            if (cache == CacheType.CacheTypeTransactionLifeTime) //Application cache not allowed.
            {
                id = new RecordIdentifier((int)relation, 
                                           itemRelation, 
                                           accountRelation, 
                                           itemCode, 
                                           accountCode, 
                                           quantityAmount, 
                                           storeCurrencyCode,
                                           itemID,
                                           unitID);
                CacheBucket bucket = (CacheBucket)entry.Cache.GetEntityFromCache(typeof (CacheBucket), id);
                if (bucket != null)
                {
                    return (DataTable) bucket.BucketData;
                }
            }
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = " SELECT ISNULL(PERCENT1, 0) AS PERCENT1, ISNULL(PERCENT2, 0) AS PERCENT2, ISNULL(AMOUNT, 0) AS AMOUNT, " +
                                     " ISNULL(QUANTITYAMOUNT, 0) AS QUANTITYAMOUNT, ISNULL(SEARCHAGAIN, 0) AS SEARCHAGAIN" +
                                     " FROM PriceDiscTable P " +
                                     " left join RETAILITEM inv on inv.ITEMID = itemrelation" +
                                     " WHERE Relation = @Relation " +
                                     " AND ItemCode = @ItemCode  " +
                                     " AND (@UNITID = '' OR UNITID = '' OR UNITID = @UNITID)  " +
                                     " AND (ItemRelation = @ItemRelation " +
                                     " OR  inv.masterid =  @itemID or inv.HEADERITEMID = @itemID)" +
                                     " AND AccountCode = @AccountCode " +
                                     " AND AccountRelation = @AccountRelation " +
                                     " AND QuantityAmount <= @QuantityAmount " +
                                     " AND Currency = @Currency " +
                                     " AND P.DATAAREAID = @DATAAREAID ";

              

                cmd.CommandText  += " AND ((@ToDay >= FromDate OR FromDate < '01.01.1901' ) AND (@ToDay <= ToDate OR ToDate < '01.01.1901')) " +
                               " ORDER BY Relation, ItemCode, ItemRelation, AccountCode," +
                               " AccountRelation, Currency, QuantityAmount desc ";
                MakeParam(cmd, "Relation", (int) relation, SqlDbType.Int);
                MakeParam(cmd, "ItemCode", itemCode, SqlDbType.Int);
                MakeParam(cmd, "ItemRelation", itemRelation);
                MakeParam(cmd, "AccountCode", accountCode, SqlDbType.Int);
                MakeParam(cmd, "AccountRelation", accountRelation);
                MakeParam(cmd, "QuantityAmount", quantityAmount, SqlDbType.Decimal);
                MakeParam(cmd, "Currency", storeCurrencyCode);
                MakeParam(cmd, "itemID", RecordIdentifier.IsEmptyOrNull(itemID)
                    ? Guid.Empty
                    :(Guid)itemID,SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "UNITID", unitID.StringValue);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ToDay", DateTime.Now, SqlDbType.DateTime);

                DataTable priceDiscTable = new DataTable();

                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);
                    priceDiscTable.Load(dr);
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }

                if (cache == CacheType.CacheTypeTransactionLifeTime)
                {
                    CacheBucket newData = new CacheBucket(id, "discountData", priceDiscTable);
                    entry.Cache.AddEntityToCache(id, newData, cache);
                }

                return priceDiscTable;
            }
        }

        public DataTable GetPeriodicDiscountData(IConnectionManager entry, string itemId, string itemGroupId, string itemDepartmentId, CacheType cache = CacheType.CacheTypeNone)
        {
            if (itemId == null) { itemId = ""; }
            if (itemGroupId == null) { itemGroupId = ""; }
            if (itemDepartmentId == null) { itemDepartmentId = ""; }
            IDataReader dr = null;
            RecordIdentifier id = null;
            if (cache == CacheType.CacheTypeTransactionLifeTime) //application cache not allowed
            {
                id = new RecordIdentifier(itemId, itemGroupId, itemDepartmentId);
                CacheBucket bucket = (CacheBucket)entry.Cache.GetEntityFromCache(typeof (CacheBucket), id);
                if (bucket != null)
                {
                    return (DataTable) bucket.BucketData;
                }
            }
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT * FROM " +
                                     "POSPERIODICDISCOUNT P INNER JOIN " +
                                     "POSPERIODICDISCOUNTLINE L ON P.OfferId = L.OfferId AND P.DATAAREAID = L.DATAAREAID LEFT OUTER JOIN " +
                                     "POSMMLineGroups M ON L.LineGroup = M.LineGroup AND L.DATAAREAID = M.DATAAREAID AND P.OfferId = M.OfferId " +
                                     "WHERE (P.Status = 1) AND (P.DATAAREAID = @DATAAREAID) " +
                                     "AND (Id = '' or Id = @ItemID OR Id = @ItemGroupId " +
                                     "or Id = @DepartmentId or Id IN (SELECT GROUPID FROM RBOSPECIALGROUPITEMS WHERE ITEMID = @ItemID)) " +
                                     "ORDER BY P.OfferId,L.LineId";

                MakeParam(cmd, "ItemID", itemId);
                MakeParam(cmd, "ItemGroupId", itemGroupId);
                MakeParam(cmd, "DepartmentId", itemDepartmentId);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                DataTable periodicDiscount = new DataTable();
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);
                    periodicDiscount.Load(dr);
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }

                if (cache == CacheType.CacheTypeTransactionLifeTime)
                {
                    CacheBucket newData = new CacheBucket(id, "periodicDiscount", periodicDiscount);
                    entry.Cache.AddEntityToCache(id, newData, cache);
                }

                return periodicDiscount;
            }
        }

        public List<PeriodicDiscount> GetPeriodicDiscountList(IConnectionManager entry, string itemId, string unitId,
            string itemGroupId, 
            string itemDepartmentId, string transactionID, CacheType cache = CacheType.CacheTypeNone)
        {
            //todo add cache
            if (itemId == null)
            {
                itemId = "";
            }
            if (unitId == null)
            {
                unitId = "";
            }
            if (itemGroupId == null)
            {
                itemGroupId = "";
            }
            if (itemDepartmentId == null)
            {
                itemDepartmentId = "";
            }

            RecordIdentifier id = null;
            if (cache == CacheType.CacheTypeTransactionLifeTime) // application cache not allowed
            {
                id = new RecordIdentifier(itemId, unitId, itemGroupId, itemDepartmentId);
                CacheBucket bucket = (CacheBucket) entry.Cache.GetEntityFromCache(typeof (CacheBucket), id);
                if (bucket != null)
                {
                    return (List<PeriodicDiscount>) bucket.BucketData;
                }
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();

                columns.Add(new TableColumn { ColumnName = "OfferID", TableAlias = "P" });

                columns.Add(new TableColumn { ColumnName = "Description", TableAlias = "P" });
                columns.Add(new TableColumn { ColumnName = "PDType" });
                columns.Add(new TableColumn { ColumnName = "Priority" });
                columns.Add(new TableColumn { ColumnName = "DiscountType" });
                columns.Add(new TableColumn { ColumnName = "DiscValidPeriodId" });
                
                columns.Add(new TableColumn { ColumnName = "NoOfLinesToTrigger" });
                columns.Add(new TableColumn { ColumnName = "DealPriceValue" });
                columns.Add(new TableColumn { ColumnName = "DiscountPctValue" });
                columns.Add(new TableColumn { ColumnName = "DiscountAmountValue" });
                columns.Add(new TableColumn { ColumnName = "NoOfLeastExpItems" });
                columns.Add(new TableColumn { ColumnName = "LineId" });
                columns.Add(new TableColumn { ColumnName = "ProductType" });
                columns.Add(new TableColumn { ColumnName = "PriceGroup" });
                columns.Add(new TableColumn { ColumnName = "ACCOUNTCODE" });
                columns.Add(new TableColumn { ColumnName = "ACCOUNTRELATION" });
                columns.Add(new TableColumn { ColumnName = "TRIGGERED" });
                columns.Add(new TableColumn { ColumnName = "TargetId" });
                columns.Add(new TableColumn { ColumnName = "TargetMasterID" });
                columns.Add(new TableColumn { ColumnName = "UNIT", TableAlias = "L", IsNull = true, NullValue = "''"});
                columns.Add(new TableColumn { ColumnName = "DealPriceOrDiscPct" });
                columns.Add(new TableColumn { ColumnName = "LINEGROUP", TableAlias = "L" });
                columns.Add(new TableColumn { ColumnName = "DiscType" });
                columns.Add(new TableColumn { ColumnName = "NoOfItemsNeeded" });

                columns.Add(new TableColumn { ColumnName = "@ItemID", ColumnAlias = "ItemID" });
                columns.Add(new TableColumn { ColumnName = "@transactionID", ColumnAlias = "TransactionID" });                

                List<Join> joins = new List<Join>();
                joins.Add(new Join
                {
                    Condition = " P.OFFERID = L.OFFERID AND L.DELETED = 0 ",
                    JoinType = "INNER",
                    Table = "PERIODICDISCOUNTLINE",
                    TableAlias = "L"
                });
                joins.Add(new Join
                {
                    Condition = " L.LineGroup = M.LineGroup AND P.OfferId = M.OfferId ",
                    JoinType = "LEFT OUTER",
                    Table = "POSMMLineGroups",
                    TableAlias = "M"
                });

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "P.Status = 1 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "P.DELETED = 0 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = @"(
                        TargetId = '' 
                        OR TargetId = @ItemID 
                        OR TargetId = @ItemGroupId 
                        OR TargetId = @DepartmentId 
                        OR TargetId IN (SELECT GROUPID FROM SPECIALGROUPITEMS WHERE ITEMID = @ItemID)
                        OR TargetId = @HeaderItemID)" });

                var setHeaderItemIDQuery = @"DECLARE @HeaderItemID NVARCHAR(20)

                            SET @HeaderItemID = (SELECT
                                ITHEADER.ITEMID
                                FROM RETAILITEM ITVARIANT
                                JOIN RETAILITEM ITHEADER ON ITHEADER.MASTERID = ITVARIANT.HEADERITEMID
                                WHERE ITVARIANT.ITEMID = @ItemID) ";

                cmd.CommandText =   setHeaderItemIDQuery + 
                                    string.Format(
                                        QueryTemplates.BaseQuery("PERIODICDISCOUNT", "P"),
                                        QueryPartGenerator.InternalColumnGenerator(columns),
                                        QueryPartGenerator.JoinGenerator(joins),
                                        QueryPartGenerator.ConditionGenerator(conditions),
                                        "ORDER BY P.OfferId, L.LineId"
                                        );

                MakeParam(cmd, "ItemID", itemId);
                MakeParam(cmd, "transactionID", transactionID) ;
                MakeParam(cmd, "ItemGroupId", itemGroupId);
                MakeParam(cmd, "DepartmentId", itemDepartmentId);

                List<PeriodicDiscount> reply = Execute<PeriodicDiscount>(entry, cmd, CommandType.Text, PopulatePeriodicDiscount);
                if (cache == CacheType.CacheTypeTransactionLifeTime)
                {
                    CacheBucket newData = new CacheBucket(id, "periodicDiscount", reply);
                    entry.Cache.AddEntityToCache(id, newData, cache);
                }

                return reply;
            }
        }

        private static void PopulatePeriodicDiscount(IDataReader dr, PeriodicDiscount discount)
        {
            discount.TransactionID = (string)dr["TransactionID"];
            discount.OfferId = (string)dr["OfferId"];
            discount.UnitId = (string)dr["UNIT"];
            discount.Description = (string)dr["Description"];
            discount.PDType = (PeriodicDiscOfferType)(int)dr["PDType"];
            discount.Priority = (int)dr["Priority"];
            discount.DiscValidPeriodId = (string)dr["DiscValidPeriodId"];
            discount.DiscountType = (DiscountService.DiscountType)(int)dr["DiscountType"];
            discount.NoOfLinesToTrigger = dr["NoOfLinesToTrigger"] == DBNull.Value ? 0: (int)dr["NoOfLinesToTrigger"];
            discount.DealPriceValue = (decimal)dr["DealPriceValue"];
            discount.DiscountPctValue = (decimal)dr["DiscountPctValue"];
            discount.DiscountAmountValue = (decimal)dr["DiscountAmountValue"];
            discount.NoOfLeastExpItems = (int)dr["NoOfLeastExpItems"];
            discount.LineId = (int)dr["LineId"];
            discount.ProductType = (int)dr["ProductType"];
            discount.PriceGroup = (string)dr["PriceGroup"];
            discount.AccountCode = (DiscountOffer.AccountCodeEnum)((dr["ACCOUNTCODE"] == System.DBNull.Value ? 0 : (int)dr["ACCOUNTCODE"]));
            discount.AccountRelation = dr["ACCOUNTRELATION"] == System.DBNull.Value ? "" : (string)dr["ACCOUNTRELATION"];
            discount.Triggering = (DiscountOffer.TriggeringEnum)((dr["TRIGGERED"] == System.DBNull.Value ? 0 : (int)dr["TRIGGERED"]));

            string relation = dr["TargetId"] == System.DBNull.Value ? "" : (string)dr["TargetId"];
            string itemId = dr["ItemID"] == DBNull.Value ? string.Empty : (string) dr["ItemID"];            

            switch ((DiscountOfferLine.DiscountOfferTypeEnum)discount.ProductType)
            {
                case DiscountOfferLine.DiscountOfferTypeEnum.Item:
                    discount.ItemId = relation;                    
                    break;
                case DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup:
                    discount.RetailGroupId = relation;
                    break;
                case DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment:
                    discount.RetailDepartmentId = relation;
                    break;
                case DiscountOfferLine.DiscountOfferTypeEnum.BarCodeBasedVariant:
                    discount.BarcodeId = relation;
                    break;
                case DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup:
                    discount.SpecialGroup = relation;
                    discount.ItemId = itemId;
                    break;
                case DiscountOfferLine.DiscountOfferTypeEnum.Variant:
                    discount.VariationNumber = relation;
                    discount.ItemId = itemId;
                    break;
            }

            discount.TargetMasterID = dr["TargetMasterID"] == System.DBNull.Value ? Guid.Empty : (Guid)dr["TargetMasterID"];
            discount.DealPriceOrDiscPct = (decimal)dr["DealPriceOrDiscPct"];
            discount.LineGroup = (string)dr["LineGroup"];
            discount.DiscType = (DiscountService.DiscountType)(int)dr["DiscType"];
            try
            {
                object value = dr["NoOfItemsNeeded"];
                discount.NoOfItemsNeeded = (value == DBNull.Value) ? 0 : (int)value;
            }
            catch
            {
                discount.NoOfItemsNeeded = 0;
            }
          
        }
    }
}
