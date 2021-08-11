using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    internal interface IDiscountData : IDataProviderBase<DataEntity>
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
        /// <param name="itemID">Item specific identifier</param>
        /// <param name="unitID">Unit ID in which the item is sold</param>
        /// <param name="cache">Cache settings</param>
        /// <returns></returns>
        DataTable GetPriceDiscData(IConnectionManager entry, 
            PriceDiscType relation, 
            string itemRelation, 
            string accountRelation, 
            int itemCode, 
            int accountCode, 
            decimal quantityAmount, 
            string storeCurrencyCode, 
            RecordIdentifier itemID,
            RecordIdentifier unitID,
            CacheType cache = CacheType.CacheTypeNone);

        DataTable GetPeriodicDiscountData(IConnectionManager entry, string itemId, string itemGroupId,  string itemDepartmentId, CacheType cache = CacheType.CacheTypeNone);

        List<PeriodicDiscount> GetPeriodicDiscountList(IConnectionManager entry, string itemId, string unitId, string itemGroupId, string itemDepartmentId, string transactionID, CacheType cache = CacheType.CacheTypeNone);
    }
}