using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Tax
{
    public interface ISalesTaxGroupData : IDataProvider<SalesTaxGroup>, ICompareListGetter<SalesTaxGroup>, ISequenceable
    {
        /// <summary>
        /// Gets a list of data entities containing IDs and names of all sales tax groups
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupType">Filter results by group type</param>
        /// <returns>A list of data entities containing IDs and names of all sales tax groups</returns>
        List<DataEntity> GetList(IConnectionManager entry, TaxGroupTypeFilter groupType = TaxGroupTypeFilter.All);

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all sales tax groups that have sales tax codes
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupType">Filter results by group type</param>
        /// <returns>A list of data entities containing IDs and names of all sales tax groups</returns>
        List<DataEntity> GetListWithTaxCodes(IConnectionManager entry, TaxGroupTypeFilter groupType = TaxGroupTypeFilter.All);

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all sales tax groups, ordered by 
        /// a sort enum and a reversed ordering based on a parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">The enum that determines what to sort by</param>
        /// <param name="backwardsSort">Whether the result set is ordered backwards</param>
        /// <param name="cacheType">Cache</param>
        /// <returns>A list of data entities containing IDs and names of all sales tax groups, meeting the above criteria</returns>
        List<SalesTaxGroup> GetSalesTaxGroups(IConnectionManager entry, SalesTaxGroup.SortEnum sortEnum, bool backwardsSort, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets a list either of tax codes in a sales tax group or tax codes not in a sales tax group. Returns data entities of the tax codes.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="salesTaxGroupID">The ID of the item sales tax group</param>
        /// <param name="inTaxGroup">Wether to search for tax codes in group or tax codes not in group</param>
        /// <param name="cacheType">Cache</param>
        List<DataEntity> GetTaxCodesInSalesTaxGroupList(IConnectionManager entry,
            RecordIdentifier salesTaxGroupID, bool inTaxGroup, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Check if Tax Group is in use in a store, a customer or a sales type
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="salesTaxGroupID">The ID of the sales tax group to get</param>
        /// <returns>True if Tax Group is in use</returns>
        bool TaxGroupInUse(IConnectionManager entry, RecordIdentifier salesTaxGroupID);

        /// <summary>
        /// Gets a list of TaxCode-SalesTaxGroup connections for a given sales tax group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="salesTaxGroupID">ID of the item sales tax group</param>
        /// <param name="sortEnum">Determines the sort ordering of the results</param>
        /// <param name="backwardsSort">Determines if the results will be in reversed ordering</param>
        /// <param name="cacheType">Cache</param>
        /// <returns>A list of TaxCode-SalesTaxGroup connections for a given sales tax group</returns>
        List<TaxCodeInSalesTaxGroup> GetTaxCodesInSalesTaxGroup(IConnectionManager entry,
            RecordIdentifier salesTaxGroupID, TaxCodeInSalesTaxGroup.SortEnum sortEnum, bool backwardsSort,
            CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Adds a tax code to a sales tax group.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="item">Contains IDs of the tax code and the item sales tax group</param>
        void AddTaxCodeToSalesTaxGroup(IConnectionManager entry, TaxCodeInSalesTaxGroup item);

        /// <summary>
        /// Removes a tax code from a sales tax group
        /// </summary>
        /// <remarks>Requires the 'Edit sales tax setup' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">Contains (sales tax group ID, tax code ID)</param>
        void RemoveTaxCodeFromSalesTaxGroup(IConnectionManager entry, RecordIdentifier id);

        bool TaxCodeIsInDefaultStoreTaxGroup(IConnectionManager entry, RecordIdentifier taxCodeID, CacheType cacheType = CacheType.CacheTypeNone);

        SalesTaxGroup Get(IConnectionManager entry, RecordIdentifier salesTaxGroupID, CacheType cacheType = CacheType.CacheTypeNone);
    }
}