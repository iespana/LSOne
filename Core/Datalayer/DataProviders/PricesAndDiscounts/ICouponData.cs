using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.PricesAndDiscounts
{
    public interface ICouponData : IDataProviderBase<Coupon>, ISequenceable
    {
        /// <summary>
        /// Retruns a coupon with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="couponID">ID of the coupon</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        Coupon Get(IConnectionManager entry, RecordIdentifier couponID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns a list of all coupons
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        List<Coupon> GetCoupons(IConnectionManager entry, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns the coupon item with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="couponItemID">ID of the coupon item (couponID, itemID)</param>
        /// <param name="cacheType"></param>
        /// <returns>Coupon item with couponItemID</returns>
        CouponItem GetCouponItem(IConnectionManager entry, RecordIdentifier couponItemID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns a list of the items for a given coupon
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="couponID">ID of the coupon to get items for</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        List<CouponItem> GetCouponItems(IConnectionManager entry, RecordIdentifier couponID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets all coupon-customer links for a coupon
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="couponID">ID of the coupon</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        List<CouponCustomerLink> GetCouponCustomerLinks(IConnectionManager entry, RecordIdentifier couponID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets all coupon-customer links for a customer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">ID of the customer</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        List<CouponCustomerLink> GetCustomerCouponLinks(IConnectionManager entry, RecordIdentifier customerID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets a specific coupon-customer link
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="linkID">ID of the coupon-customer link (couponID, customerID)</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        CouponCustomerLink GetCouponCustomerLink(IConnectionManager entry, RecordIdentifier linkID, CacheType cacheType = CacheType.CacheTypeNone);

        void Delete(IConnectionManager entry, RecordIdentifier couponID);
        void DeleteItem(IConnectionManager entry, RecordIdentifier couponItemID);
        void DeleteLink(IConnectionManager entry, RecordIdentifier linkID);

        bool ExistsItem(IConnectionManager entry, RecordIdentifier couponItemID, int type);
        bool ExistsLink(IConnectionManager entry, RecordIdentifier linkID);

        void SaveCoupon(IConnectionManager entry, Coupon coupon);
        void SaveItem(IConnectionManager entry, CouponItem item);
        void SaveLink(IConnectionManager entry, CouponCustomerLink link);
    }
}