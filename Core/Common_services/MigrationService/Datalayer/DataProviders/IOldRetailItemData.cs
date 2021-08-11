using System.Collections.Generic;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Services.Datalayer.BusinessObjects.ListItems;
using LSOne.Utilities.DataTypes;
using RetailItemSearchEnum = LSOne.DataLayer.BusinessObjects.ItemMaster.RetailItemSearchEnum;

namespace LSOne.Services.Datalayer.DataProviders
{
    public interface IOldRetailItemData : IDataProvider<OldRetailItem>, ISequenceable
    {
        /// <summary>
        /// Looks up the unit id for a item by a given id. The type of unit depends on the module type.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">Id of the retail item in the database</param>
        /// <param name="module">Module type enum which determines what type of unit id we are returning</param>
        /// <returns>The unit ID of an item depending on the module type</returns>
        RecordIdentifier GetItemUnitID(IConnectionManager entry, RecordIdentifier itemID, OldRetailItem.ModuleTypeEnum module);

        /// <summary>
        /// Updates the unit information on a specific item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item to update</param>
        /// <param name="unitID">The new unit ID information</param>
        /// <param name="module">Which module information to update (inventory, purchase,
        /// sales)</param>
        void UpdateUnitID(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier unitID, OldRetailItem.ModuleTypeEnum module);

        /// <summary>
        /// Searched for retail items that contain a given search text, and returns the results as a List of <see cref="ItemListItem"/>
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchString">The text to search for. Searches in item name, in the ID field and the search alias field</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        /// <returns>A list of found items</returns>
        List<OldItemListItem> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith, string sort);

        List<RecordIdentifier> AdvancedSearchIDOnly(IConnectionManager entry,
            string idOrDescription = null,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier retailGroupID = null,
            RecordIdentifier retailDepartmentID = null,
            RecordIdentifier taxGroupID = null,
            RecordIdentifier variantGroupID = null,
            RecordIdentifier vendorID = null,
            string barCode = null,
            bool barCodeBeginsWith = true,
            RecordIdentifier specialGroup = null);

        List<OldItemListItem> AdvancedSearchOptimized(IConnectionManager entry,
            int rowFrom, int rowTo, string sort,
            out int totalRecordsMatching,
            string idOrDescription = null,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier retailGroupID = null,
            RecordIdentifier retailDepartmentID = null,
            RecordIdentifier taxGroupID = null,
            RecordIdentifier variantGroupID = null,
            RecordIdentifier vendorID = null,
            string barCode = null,
            bool barCodeBeginsWith = true,
            RecordIdentifier specialGroup = null);

        List<OldItemListItem> AdvancedSearch(IConnectionManager entry,
            int rowFrom, int rowTo, string sort,
            out int totalRecordsMatching,
            string idOrDescription = null,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier retailGroupID = null,
            RecordIdentifier retailDepartmentID = null,
            RecordIdentifier taxGroupID = null,
            RecordIdentifier variantGroupID = null,
            RecordIdentifier vendorID = null,
            string barCode = null,
            bool barCodeBeginsWith = true,
            RecordIdentifier specialGroup = null);

        /// <summary>
        /// Searched for retail items that contain a given search text, and returns the results as a List of <see cref="ItemListItem"/> . 
        /// Additionally this search function accepts a Dictionary with a combination of <see cref="DataLayer.BusinessObjects.ItemMaster.RetailItemSearchEnum"/> and a string and resolves this list to add additional 
        /// search filters to the query
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The item id or item name to search for</param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="beginsWith">Indicates wether you only want to search for items that begin with the given item name or item id</param>
        /// <param name="sort">The name of the column you want to sort by</param>
        /// <param name="advancedSearchParameters">Additional search parameters</param>
        /// <returns></returns>
        List<OldItemListItem> AdvancedSearch(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith, string sort, Dictionary<OldRetailItemSearchEnum, string> advancedSearchParameters);

        List<DataEntity> GetForecourtItems(IConnectionManager entry, RecordIdentifier gradeID);

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="ItemListItem" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="culture">Language code of the included item translations</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        List<DataEntity> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, string culture, bool beginsWith);

        /// <summary>
        /// Gets retail items in the system for a specific ID using a LIKE query
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">Item ID</param>
        /// <returns>The retail item, or null if not found</returns>
        List<OldRetailItem> GetItemsByItemPattern(IConnectionManager entry, string itemID);

        /// <summary>
        /// Gets all retail items in the system
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>The retail item, or null if not found</returns>
        List<OldRetailItem> GetAllItems(IConnectionManager entry);

        /// <summary>
        /// Checks if any item is using a given tax group id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxgroupID">ID of the tax group</param>
        /// <returns>True if any item uses the tax group, else false</returns>
        bool TaxGroupExists(IConnectionManager entry, RecordIdentifier taxgroupID);

        /// <summary>
        /// Gets an items item sales tax group. Returns an empty record identifier if item has no item sales tax group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">The items ID</param>
        /// <returns>Items item sales tax group</returns>
        RecordIdentifier GetItemsItemSalesTaxGroupID(IConnectionManager entry, RecordIdentifier itemId);

        /// <summary>
        /// Gets the latest purchase price of an item
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="itemID">The item who's purchase price we want</param>
        /// <returns>The latest purchase price of an item</returns>
        decimal GetLatestPurchasePrice(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Returns the default vendor for a given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <returns>
        /// Returns the vendor ID
        /// </returns>
        string GetItemsDefaultVendor(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Returns true if the item has a default vendor
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <returns>
        /// Returns true if the item has a default vendor
        /// </returns>
        bool ItemHasDefaultVendor(IConnectionManager entry, RecordIdentifier itemID);

        bool ItemHasDimentionGroup(IConnectionManager entry, OldItemListItem itemID);

        /// <summary>
        /// Sets a vendor as a default on a given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="vendorItemID">The unique ID of the vendor item</param>
        void SetItemsDefaultVendor(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier vendorItemID);

        OldRetailItem.RetailItemModule GetPriceModule(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Returns item ids for all items that have a tax group with the given tax code ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeID">ID of the tax code</param>
        List<RecordIdentifier> GetItemIDsFromTaxCode(IConnectionManager entry, RecordIdentifier taxCodeID);

        List<RecordIdentifier> GetItemIDsFromTaxGroup(IConnectionManager entry, RecordIdentifier itemSalesTaxGroupID);
        List<RecordIdentifier> GetItemIDsOfItemsWithTaxGroup(IConnectionManager entry);
        int Count(IConnectionManager entry);

        /// <summary>
        /// Fetches the default image for a given retail item.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of the retail item to fetch the image for</param>
        /// <returns>The image for the given retail item or null if no image was found</returns>
        Image GetDefaultImage(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Fetches all images for a given retail item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of the retail item to fetch the image for</param>
        /// <returns>The image for the given retail item or null if no image was found</returns>
        List<OldItemImage> GetImages(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Saves an image as the default image for a given retail item
        /// </summary>
        /// <remarks>Edit retail items permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of the retail item to save the image for</param>
        /// /// <param name="image">The image to be saved</param>
        void SaveImage(IConnectionManager entry, RecordIdentifier itemID, Image image);

        /// <summary>
        /// Saves an image as the default image for a given retail item
        /// </summary>
        /// <remarks>Edit retail items permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of the retail item to save the image for</param>
        /// /// <param name="image">The image to be saved</param>
        /// <param name="index">Index of image. If set to -1, the next available index is found and used</param>
        void SaveImage(IConnectionManager entry, RecordIdentifier itemID, Image image, int index);

        /// <summary>
        /// Saves an image as the default image for a given retail item
        /// </summary>
        /// <remarks>Edit retail items permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemImage">The item image to be saved</param>
        void SaveImage(IConnectionManager entry, OldItemImage itemImage);

        /// <summary>
        /// Checks if item has dependant entries preventing it from being deleted
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool ItemCanBeDeleted(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Delete an item image from the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemImage"></param>
        void DeleteImage(IConnectionManager entry, OldItemImage itemImage);

        /// <summary>
        /// Delete all images for an item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of retail item</param>
        void DeleteImages(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Resequence all images for a particular item in the order they ara passed in itemImages
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemImages">Images to save</param>
        void ResequenceImages(IConnectionManager entry, List<OldItemImage> itemImages);

        List<DataEntity> FindItemDepartment(IConnectionManager entry, string searchText);
        List<DataEntity> FindItem(IConnectionManager entry, string searchText);

        OldRetailItem Get(IConnectionManager entry, RecordIdentifier ID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Return number of retail items
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>The count of items</returns>
        int ItemCount(IConnectionManager entry);
         
        /// <summary>
        /// Updates Department and Division for items to the correct ones according to values in the hierarchy
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        void ReAssignItemDepartmentAndDivision(IConnectionManager entry);
    }
}