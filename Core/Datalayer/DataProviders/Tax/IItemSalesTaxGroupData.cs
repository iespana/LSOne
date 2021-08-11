using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Tax
{
    public interface IItemSalesTaxGroupData : IDataProvider<ItemSalesTaxGroup>, ICompareListGetter<ItemSalesTaxGroup>, ISequenceable
    {
        /// <summary>
        /// Check if Tax Group is in use by retail item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemSalesTaxGroupID">The ID of the item sales tax group to get</param>
        /// <returns>True if Tax Group is in use</returns>
        bool TaxGroupInUse(IConnectionManager entry, RecordIdentifier itemSalesTaxGroupID);

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all item sales tax groups
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities containing IDs and names of all item sales tax groups</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all item sales tax groups that have tax codes
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities containing IDs and names of all item sales tax groups</returns>
        List<DataEntity> GetListWithTaxCodes(IConnectionManager entry);

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all item sales tax groups, ordered by 
        /// a sort enum and a reversed ordering based on a parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">The enum that determines what to sort by</param>
        /// <param name="backwardsSort">Whether the result set is ordered backwards</param>
        /// <param name="cacheType">Cache</param>
        /// <returns>A list of data entities containing IDs and names of all item sales tax groups, meeting the above criteria</returns>
        List<ItemSalesTaxGroup> GetItemSalesTaxGroups(IConnectionManager entry, ItemSalesTaxGroup.SortEnum sortEnum, bool backwardsSort, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Adds a tax code to an item sales tax group.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="item">Contains IDs of the tax code and the item sales tax group</param>
        void AddTaxCodeToItemSalesTaxGroup(IConnectionManager entry, TaxCodeInItemSalesTaxGroup item);

        /// <summary>
        /// Removes a tax code from an item sales tax group
        /// </summary>
        /// <remarks>Requires the 'Edit sales tax setup' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">Contains (item sales tax group ID, tax code ID)</param>
        void RemoveTaxCodeFromItemSalesTaxGroup(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Gets a list of tax codes in an item sales tax group. Returns data entities of the tax codes.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemSalesTaxGroupID">The ID of the item sales tax group</param>
        /// <param name="cacheType">Cache</param>
        /// <returns>DataEntities of tax codes in an item sales tax group</returns>
        List<DataEntity> GetTaxCodesInItemSalesTaxGroupList(IConnectionManager entry, RecordIdentifier itemSalesTaxGroupID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets a list of TaxCode-ItemSalesTaxGroup connections for a given item sales tax group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemSalesTaxGroupID">ID of the item sales tax group</param>
        /// <param name="sortEnum">Determines the sort ordering of the results</param>
        /// <param name="backwardsSort">Determines if the results will be in reversed ordering</param>
        /// <param name="cacheType">Cache</param>
        /// <returns>A list of TaxCode-ItemSalesTaxGroup connections for a given item sales tax group</returns>
        List<TaxCodeInItemSalesTaxGroup> GetTaxCodesInItemSalesTaxGroup(
            IConnectionManager entry,
            RecordIdentifier itemSalesTaxGroupID,
            TaxCodeInItemSalesTaxGroup.SortEnum sortEnum,
            bool backwardsSort,
            CacheType cacheType = CacheType.CacheTypeNone);

        ItemSalesTaxGroup Get(IConnectionManager entry, RecordIdentifier itemSalesTaxGroupID, CacheType cacheType = CacheType.CacheTypeNone);
    }
}