using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IVendorItemData : IDataProvider<VendorItem>, ISequenceable
    {
        /// <summary>
        /// Gets a list of DataEntity that contains Vendor item ID and Item Description for a given vendorID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">ID of the vendor to get the list for</param>
        /// <returns>List of the vendor items from a given vendor</returns>
        List<DataEntity> GetList(IConnectionManager entry,RecordIdentifier vendorID);

        /// <summary>
        /// Gets a list of DataEntities that contains distinct list of retail item ID's and Item Description for a given vendorID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">ID of the vendor to get the list for</param>
        /// <returns>List of the retail items from a given vendor</returns>
        List<MasterIDEntity> GetDistinctRetailItemsForVendor(IConnectionManager entry, RecordIdentifier vendorID);

      
        /// <summary>
        /// Searches for vendor items
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="vendorID"></param>
        /// <param name="searchString"></param>
        /// <param name="itemGroupId"></param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="beginsWith"></param>
        /// <returns>Return a list of <see cref="DataEntity"/> with description and retail item ID and vendoritemID on the secondary ID </returns>
        List<DataEntity> SearchRetailItemsForVendor(IConnectionManager entry, RecordIdentifier vendorID,
            string searchString, RecordIdentifier itemGroupId, int rowFrom, int rowTo, bool beginsWith);


        /// <summary>
        /// Returns a list of all units that are available for the item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="itemID">The unique ID for the item</param>
        /// <returns></returns>
        List<Unit> GetDistinctUnitsForVendorItem(IConnectionManager entry, RecordIdentifier vendorId, RecordIdentifier retailItemId);

        VendorItem Get(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Gets a single vendor item by a given vendor ID and given vendor item ID (External Vendor item ID)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">ID of the Vendor</param>
        /// <param name="vendorItemID">ID of item as the vendor knows it (External Vendor item id)</param>
        /// <returns>The requested vendor item or null if not found</returns>
        VendorItem Get(IConnectionManager entry, RecordIdentifier vendorID, RecordIdentifier vendorItemID);

        /// <summary>
        /// Gets a single vendor item by a given vendorID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">ID of the vendor</param>
        /// <param name="retailItemID">Item of the retail item</param>
        /// <param name="unitID">Unit ID of the item</param>
        /// <returns>The requested vendor item or null if not found</returns>
        VendorItem Get(IConnectionManager entry, RecordIdentifier vendorID,RecordIdentifier retailItemID,
            RecordIdentifier unitID);

        /// <summary>
        /// Gets all vendor items for a given vendor ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">ID of the vendor to fetch records for</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>All vendors in the database</returns>
        List<VendorItem> GetItemsForVendor(IConnectionManager entry, RecordIdentifier vendorID, VendorItemSorting sortBy, 
            bool sortBackwards);

        /// <summary>
        /// Gets paginated vendor items for a given vendor ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">ID of the vendor to fetch records for</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <param name="startRecord">Pagination start index</param>
        /// <param name="endRecord">Pagination end index</param>
        /// <param name="totalRecords">Total number of records for the given vendor</param>
        /// <returns>All vendors in the database</returns>
        List<VendorItem> GetItemsForVendor(IConnectionManager entry, 
                                           RecordIdentifier vendorID, 
                                           VendorItemSorting sortBy, bool sortBackwards, 
                                           int startRecord, int endRecord,
                                           out int totalRecords);

        /// <summary>
        /// Gets a vendor item with given retailItemId and vendorId with minimal data (no derrived descriptions).
        /// This function is should be used to get vendor from a given default vendor ID on a item.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="retailItemID">ID of the retail item</param>
        /// <param name="vendorID">ID of the vendor or default vendor</param>
        /// <returns>The requested vendor or null if not found</returns>
        VendorItem GetVendorForItem(IConnectionManager entry, RecordIdentifier retailItemID, RecordIdentifier vendorID);

        List<VendorItem> GetVendorsForItem(IConnectionManager entry, RecordIdentifier retailItemID, VendorItemSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets the latest purchase price of an item
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="itemID">The item we want the purchase price for</param>
        /// <param name="vendorID">ID of the vendor</param>
        /// <param name="unitID">The unit of the item we want the purchase price for</param>
        /// <returns>The default cost price of an item</returns>
        decimal GetDefaultPurchasePrice(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier vendorID, RecordIdentifier unitID);

        decimal GetLatestPurchasePrice(
            IConnectionManager entry,
            RecordIdentifier retailItemID,
            RecordIdentifier vendorID,
            RecordIdentifier unitID);

        /// <summary>
        /// Returns true if there's a link between the given vendorID and itemID in the VENDORITEMS table.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="retailItemID">Unique ID of the item</param>
        /// <param name="vendorID">Unique ID of the vendor</param>
        /// <returns></returns>
        bool VendorHasItem(IConnectionManager entry, RecordIdentifier retailItemID, RecordIdentifier vendorID);

        /// <summary>
        /// Returns true if there's at least one item linked to the given vendorID in VENDORITEMS table.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">Unique ID for the vendor</param>
        /// <returns></returns>
        bool VendorHasItems(IConnectionManager entry, RecordIdentifier vendorID);

        /// <summary>
        /// Returns true if there is at least one item that has the given vendorID as default vendor.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">The unique ID of the vendor</param>
        /// <returns></returns>
        bool VendorIsDefaultVendor(IConnectionManager entry, RecordIdentifier vendorID);

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID and vendor item ID exists, excluding current record from the check
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="vendorItemID">ID of the vendor item to check for (this is external vendor specific id)</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <returns>True if the vendor item exists, else false</returns>
        bool Exists(IConnectionManager entry, RecordIdentifier vendorID,RecordIdentifier vendorItemID,RecordIdentifier oldRecordID);

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID, retailID, variantID and unitID, excluding current record from the check
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="retaiID">ID of the retail item</param>
        /// <param name="unitID">ID of the variant</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <returns>True if the vendor item exists, else false</returns>
        bool Exists(IConnectionManager entry, RecordIdentifier vendorID, RecordIdentifier retaiID, RecordIdentifier unitID, RecordIdentifier oldRecordID);

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID, retailID, variantID and unitID, excluding current record from the check
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="retaiID">ID of the retail item</param>
        /// <param name="unitID">ID of the variant</param>
        /// <param name="vendorItemID">ID of the vendor item to check for (this is external vendor specific id)</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <returns>True if the vendor item exists, else false</returns>
        bool Exists(IConnectionManager entry, RecordIdentifier vendorID, RecordIdentifier retaiID,
            RecordIdentifier unitID, RecordIdentifier vendorItemID, RecordIdentifier oldRecordID);

        /// <summary>
        /// Deletes all vendor items for the given retail item ID.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="retailItemID">The ID of the retail item</param>
        /// <param name="vendorID">The ID of the vendor</param>
        void DeleteByRetailItemID(IConnectionManager entry, RecordIdentifier retailItemID,RecordIdentifier vendorID);


        /// <summary>
        /// Deletes all vendor items for the given retail item ID.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="retailItemID">The ID of the retail item</param>
        /// <param name="vendorID">The ID of the vendor</param>
        /// <param name="unitID">The ID of the unit</param>
        void DeleteByRetailItemID(IConnectionManager entry, RecordIdentifier retailItemID, RecordIdentifier vendorID, RecordIdentifier unitID);

        /// <summary>
        /// Searches the vendor item for the given criteria
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchCriteria">Criterias for search</param>
        /// <param name="sortBy">Sort field for returned vendor items</param>
        /// <param name="sortBackwards">Sort direction of the returned vendor items</param>
        /// <param name="startRecord">Pagination start index</param>
        /// <param name="endRecord">Pagination end index</param>
        /// <param name="totalRecords">Total number of records that match the search criteria</param>
        /// <returns></returns>
        List<VendorItem> AdvancedSearch(IConnectionManager entry, VendorItemSearch searchCriteria,
                                        VendorItemSorting sortBy, bool sortBackwards,
                                        int startRecord, int endRecord,
                                        out int totalRecords);
    }
}