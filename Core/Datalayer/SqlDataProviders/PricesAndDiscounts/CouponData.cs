using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.PricesAndDiscounts
{
    public class CouponData : SqlServerDataProviderBase, ICouponData
    {
        private static string BaseCouponSql
        {
            get 
            {
                return @"SELECT c.COUPONID, 
                        c.TXT, 
                        ISNULL(c.DETAILS, '') as DETAILS,
                        ISNULL(c.PERCENTVALUE,0.0) as PERCENTVALUE, 
                        ISNULL(c.DISCVALIDPERIODID,'') as DISCVALIDPERIODID, 
                        ISNULL(p.DESCRIPTION, '') as DISCVALIDPERIOD, 
                        ISNULL(c.MAXUSAGES, 0) as MAXUSAGES, 
                        ISNULL(c.GIVETONEW, 1) as GIVETONEW, 
                        c.IMAGE
                        FROM COUPONTABLE c 
                        LEFT OUTER JOIN POSDISCVALIDATIONPERIOD p ON c.DISCVALIDPERIODID = p.ID "; 
            }
        }

        private static string BaseItemSql
        {
            get 
            {
                return @"SELECT ci.COUPONID, 
                        ci.ITEMRELATION,
                        ISNULL(ci.TYPE, 0) as TYPE, 
                        ci.ITEMQUANTITY, 
                        ISNULL(it.ITEMNAME, '') as ITEMNAME,
                        ISNULL(rg.NAME, '') as RETAILGROUPNAME 
                        FROM COUPONITEMS ci 
                        LEFT OUTER JOIN RETAILITEM IT ON IT.ITEMID = CI.ITEMRELATION
                        LEFT OUTER JOIN RETAILGROUP rg on rg.GROUPID = ci.ITEMRELATION "; 
            }
        }

        private static string BaseCouponCustomerLinkSql
        {
            get
            {
                return @"SELECT ccl.CUSTOMERID,
                        ccl.COUPONID,
                        ISNULL(ccl.USAGES, 0) as USAGES, 
                        c.TXT,
                        ISNULL(ct.NAME, '') as CUSTOMERNAME 
                        FROM COUPONCUSTOMERLINK ccl 
                        LEFT OUTER JOIN COUPONTABLE c on c.DATAAREAID = ccl.DATAAREAID AND c.COUPONID = ccl.COUPONID 
                        LEFT OUTER JOIN CUSTOMER ct on ct.DATAAREAID = c.DATAAREAID AND ct.ACCOUNTNUM = ccl.CUSTOMERID ";
            }
        }

        private static void PopulateCoupon(IDataReader dr, Coupon coupon)
        {
            coupon.ID = (string)dr["COUPONID"];
            coupon.Text = (string)dr["TXT"];
            coupon.Details = (string) dr["DETAILS"];
            coupon.GiveToNewCustomers = AsBool(dr["GIVETONEW"]);
            coupon.DiscountPercent = (decimal)dr["PERCENTVALUE"];
            coupon.ValidationPeriodID = (string)dr["DISCVALIDPERIODID"];
            coupon.ValidationPeriod = (string) dr["DISCVALIDPERIOD"];
            coupon.MaxUsages = (int)dr["MAXUSAGES"];
            coupon.Image = AsImage(dr["IMAGE"]);
        }

        private static void PopulateItem(IDataReader dr, CouponItem item)
        {
            item.CouponID = (string)dr["COUPONID"];
            item.ItemRelation = (string)dr["ITEMRELATION"];
            item.ItemQuantity = (int)dr["ITEMQUANTITY"];
            item.Type = (CouponItem.TypeEnum)(byte)dr["TYPE"];
            item.Text = (string)(item.Type == CouponItem.TypeEnum.Item ? dr["ITEMNAME"] : dr["RETAILGROUPNAME"]);
        }

        private static void PopulateCouponCustomerLink(IDataReader dr, CouponCustomerLink link)
        {
            link.CustomerID = (string)dr["CUSTOMERID"];
            link.CouponID = (string)dr["COUPONID"];
            link.Usages = (int)dr["USAGES"];
            link.Text = (string)dr["TXT"];
            link.CustomerDescription = (string)dr["CUSTOMERNAME"];
        }

        /// <summary>
        /// Retruns a coupon with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="couponID">ID of the coupon</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public Coupon Get(IConnectionManager entry, RecordIdentifier couponID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseCouponSql +
                    @"WHERE c.DATAAREAID = @dataAreaId AND c.COUPONID = @couponID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "couponID", (string)couponID);

                return Get<Coupon>(entry, cmd, couponID, PopulateCoupon, cacheType, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Returns a list of all coupons
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public List<Coupon> GetCoupons(IConnectionManager entry, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseCouponSql + "WHERE c.DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return GetList<Coupon>(entry, cmd, "AllCoupons", PopulateCoupon, cacheType);
            }
        }

        /// <summary>
        /// Returns the coupon item with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="couponItemID">ID of the coupon item (couponID, itemID)</param>
        /// <param name="cacheType"></param>
        /// <returns>Coupon item with couponItemID</returns>
        public CouponItem GetCouponItem(IConnectionManager entry, RecordIdentifier couponItemID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseItemSql +
                                    @"WHERE ci.DATAAREAID = @dataAreaId 
                                        AND ci.COUPONID = @couponID
                                        AND ci.ITEMRELATION = @itemRelation";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "couponID", (string)couponItemID.PrimaryID);
                MakeParam(cmd, "itemRelation", (string) couponItemID.SecondaryID);

                return Get<CouponItem>(entry, cmd, couponItemID, PopulateItem, cacheType, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Returns a list of the items for a given coupon
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="couponID">ID of the coupon to get items for</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public List<CouponItem> GetCouponItems(IConnectionManager entry, RecordIdentifier couponID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseItemSql +
                                    @"WHERE ci.DATAAREAID = @dataAreaId AND ci.COUPONID = @couponID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "couponID", (string)couponID);

                return GetList<CouponItem>(entry, cmd, new RecordIdentifier(couponID, "CouponItems"), PopulateItem, cacheType);
            }
        }

        /// <summary>
        /// Gets all coupon-customer links for a coupon
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="couponID">ID of the coupon</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public List<CouponCustomerLink> GetCouponCustomerLinks(IConnectionManager entry, RecordIdentifier couponID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseCouponCustomerLinkSql +
                                    @"WHERE ccl.DATAAREAID = @dataAreaId AND ccl.COUPONID = @couponID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "couponID", (string) couponID);

                return GetList<CouponCustomerLink>(entry, cmd, new RecordIdentifier(couponID, "CouponCustomerLinks"), PopulateCouponCustomerLink, cacheType);
            }
        }

        /// <summary>
        /// Gets all coupon-customer links for a customer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">ID of the customer</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public List<CouponCustomerLink> GetCustomerCouponLinks(IConnectionManager entry, RecordIdentifier customerID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseCouponCustomerLinkSql +
                                    @"WHERE ccl.DATAAREAID = @dataAreaId AND ccl.CUSTOMERID = @customerId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "customerId", (string)customerID);

                return GetList<CouponCustomerLink>(entry, cmd, new RecordIdentifier(customerID, "CouponCustomerLinks"), PopulateCouponCustomerLink, cacheType);
            }
        }

        /// <summary>
        /// Gets a specific coupon-customer link
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="linkID">ID of the coupon-customer link (couponID, customerID)</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public CouponCustomerLink GetCouponCustomerLink(IConnectionManager entry, RecordIdentifier linkID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseCouponCustomerLinkSql +
                                    @"WHERE ccl.DATAAREAID = @dataAreaId AND ccl.CUSTOMERID = @customerId AND ccl.COUPONID = @couponId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "couponId", (string)linkID.PrimaryID);
                MakeParam(cmd, "customerId", (string)linkID.SecondaryID);

                return Get<CouponCustomerLink>(entry, cmd, linkID, PopulateCouponCustomerLink, cacheType, UsageIntentEnum.Normal);
            }
        }

        public void Delete(IConnectionManager entry, RecordIdentifier couponID)
        {
            DeleteRecord(entry, "COUPONTABLE", "COUPONID", couponID, BusinessObjects.Permission.ManageDiscounts);
            DeleteRecord(entry, "COUPONITEMS", "COUPONID", couponID, BusinessObjects.Permission.ManageDiscounts);
            DeleteRecord(entry, "COUPONCUSTOMERLINK", "COUPONID", couponID, Permission.ManageDiscounts);
        }
        public void DeleteItem(IConnectionManager entry, RecordIdentifier couponItemID)
        {
            DeleteRecord(entry, "COUPONITEMS", new[] { "COUPONID", "ITEMRELATION" }, couponItemID, Permission.ManageDiscounts);
        }
        public void DeleteLink(IConnectionManager entry, RecordIdentifier linkID)
        {
            DeleteRecord(entry, "COUPONCUSTOMERLINK", new[] { "COUPONID", "CUSTOMERID" }, linkID, Permission.ManageDiscounts);
        }

        public bool Exists(IConnectionManager entry, RecordIdentifier couponID)
        {
            return RecordExists(entry, "COUPONTABLE", "COUPONID", couponID);
        }
        public bool ExistsItem(IConnectionManager entry, RecordIdentifier couponItemID, int type)
        {
            return RecordExists(entry, "COUPONITEMS", new[] { "COUPONID", "ITEMRELATION", "TYPE" }, new RecordIdentifier(couponItemID.PrimaryID, new RecordIdentifier(couponItemID.SecondaryID, type)));
        }
        public bool ExistsLink(IConnectionManager entry, RecordIdentifier linkID)
        {
            return RecordExists(entry, "COUPONCUSTOMERLINK", new[] { "COUPONID", "CUSTOMERID" }, linkID);
        }

        public void SaveCoupon(IConnectionManager entry, Coupon coupon)
        {
            var statement = new SqlServerStatement("COUPONTABLE");

            ValidateSecurity(entry, Permission.ManageDiscounts);

            bool isNew = false;
            if (coupon.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                coupon.ID = DataProviderFactory.Instance.GenerateNumber(entry, this);
            }

            if (isNew || !Exists(entry, coupon.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("COUPONID", (string)coupon.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("COUPONID", (string)coupon.ID);
            }

            statement.AddField("TXT", coupon.Text);
            statement.AddField("DETAILS", coupon.Details);
            statement.AddField("GIVETONEW", coupon.GiveToNewCustomers, SqlDbType.Bit);
            statement.AddField("PERCENTVALUE", coupon.DiscountPercent, SqlDbType.Decimal);
            statement.AddField("DISCVALIDPERIODID", (string)coupon.ValidationPeriodID);
            statement.AddField("MAXUSAGES", coupon.MaxUsages, SqlDbType.Int);
            statement.AddField("IMAGE", FromImage(coupon.Image), SqlDbType.VarBinary);

            Save(entry, coupon, statement);
        }
        public void SaveItem(IConnectionManager entry, CouponItem item)
        {
            var statement = new SqlServerStatement("COUPONITEMS");

            ValidateSecurity(entry, Permission.ManageDiscounts);

            if (!ExistsItem(entry, item.ID, (int)item.Type))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("COUPONID", (string)item.CouponID);
                statement.AddKey("ITEMRELATION", (string)item.ItemRelation);
                statement.AddKey("TYPE", (int)item.Type, SqlDbType.TinyInt);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("COUPONID", (string)item.CouponID);
                statement.AddCondition("ITEMRELATION", (string)item.ItemRelation);
                statement.AddCondition("TYPE", (int)item.Type, SqlDbType.TinyInt);
            }

            statement.AddField("ITEMQUANTITY", item.ItemQuantity, SqlDbType.Int);

            Save(entry, item, statement);
        }
        public void SaveLink(IConnectionManager entry, CouponCustomerLink link)
        {
            var statement = new SqlServerStatement("COUPONCUSTOMERLINK");

            ValidateSecurity(entry, Permission.ManageDiscounts);

            if (!ExistsLink(entry, link.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("COUPONID", (string)link.CouponID);
                statement.AddKey("CUSTOMERID", (string)link.CustomerID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("COUPONID", (string)link.CouponID);
                statement.AddCondition("CUSTOMERID", (string)link.CustomerID);
            }

            statement.AddField("USAGES", link.Usages, SqlDbType.Int);

            Save(entry, link, statement);
        }

        #region ISequenceable Members

        public bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "COUPON"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "COUPONTABLE", "COUPONID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion        
    }
}
