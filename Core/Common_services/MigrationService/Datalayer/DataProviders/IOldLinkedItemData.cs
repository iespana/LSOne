using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders
{
    public interface IOldLinkedItemData : IDataProvider<OldLinkedItem>
    {
        /// <summary>
        /// Returns a list of linked items
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item to search for</param>
        /// <param name="sortEnum">Controls which columns will be sorted by in the SQL
        /// statement</param>
        /// <param name="sortBackwards">If true then the ordering will be descending
        /// otherwise it will be ascending</param>
        List<OldLinkedItem> GetLinkedItems(
            IConnectionManager entry, 
            RecordIdentifier itemID,
            OldLinkedItem.SortEnum sortEnum, 
            bool sortBackwards);

        /// <summary>
        /// Returns information about a linked item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="linkedItemID">The unique ID of the linked item</param>
        /// <param name="cache">Optional parameter to specify if cache may be used</param>
        /// <returns>
        /// Returns an instance of <see cref="LinkedItem"/>
        /// </returns>
        OldLinkedItem Get(IConnectionManager entry, RecordIdentifier linkedItemID, CacheType cache = CacheType.CacheTypeNone);
    }
}