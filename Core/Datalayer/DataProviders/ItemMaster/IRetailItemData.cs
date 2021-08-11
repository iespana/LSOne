using System.Collections.Generic;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.ListItems;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;

namespace LSOne.DataLayer.DataProviders.ItemMaster
{
	public partial interface IRetailItemData : IDataProvider<RetailItem>, ICompareListGetter<RetailItem>, ISequenceable
	{
		/// <summary>
		/// Gets the identified retailItem 
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">Id of the retail item in the database</param>
		/// <param name="evenIfDeleted">If deleted items should be allowed</param>
		/// <param name="cacheType"></param>
		/// <returns></returns>
		RetailItem Get(IConnectionManager entry, RecordIdentifier itemID, bool evenIfDeleted,
			CacheType cacheType = CacheType.CacheTypeNone);

		/// <summary>
		/// Gets unit id's for all items in dictionary
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="unitType"></param>
		/// <returns>dictionary with item and matching unit id</returns>
		Dictionary<string, string> GetUnitIDsForItems(IConnectionManager entry, RetailItem.UnitTypeEnum unitType);

		/// <summary>
		/// Looks up the unit id for a item by a given id. The type of unit depends on the module type.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">Id of the retail item in the database</param>
		/// <param name="unitType">Unit type enum which determines what type of unit id we are returning</param>
		/// <returns>The unit ID of an item depending on the module type</returns>
		RecordIdentifier GetItemUnitID(IConnectionManager entry, RecordIdentifier itemID, RetailItem.UnitTypeEnum unitType);

		/// <summary>
		/// Gets the header item ID for an item
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="masterID">The master ID for the item</param>
		/// <returns></returns>
		RecordIdentifier GetItemIDFromMasterID(IConnectionManager entry, RecordIdentifier masterID);

		/// <summary>
		/// Gets the header item ID for an item
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The master ID for the item</param>
		/// <param name="cacheType">Type of cache to be used</param>
		/// <returns></returns>
		RecordIdentifier GetHeaderItemID(IConnectionManager entry, RecordIdentifier itemID, CacheType cacheType = CacheType.CacheTypeNone);

		/// <summary>
		/// Gets tax group id's for a list of items
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemIDs"></param>
		/// <returns>Returns a dictionary with item id's and tax group id's</returns>
		Dictionary<string, string> GetTaxCodesForItems(IConnectionManager entry, List<RecordIdentifier> itemIDs);

		/// <summary>
		/// Updates the unit information on a specific item
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The unique ID of the item to update</param>
		/// <param name="unitID">The new unit ID information</param>
		/// <param name="module">Which module information to update (inventory, purchase,
		/// sales)</param>
		void UpdateUnitID(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier unitID, RetailItem.UnitTypeEnum module);

		/// <summary>
		/// Get a list of retail items used for inventory operation. This includes the inventory on hand for each item, for the specified store.
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemIDs">List of item IDs to get</param>
		/// <param name="barcodes">List of barcodes from which to find item IDs</param>
		/// <param name="storeID">Store ID for which to get the inventory on hand</param>
		/// <param name="includeInventoryOnHand">If true, get the inventory on hand for the specified store. Otherwise, inventory on hand is 0.</param>
		/// <returns>List of inventory retail items</returns>
		List<InventoryRetailItem> GetInventoryRetailItems(IConnectionManager entry, List<RecordIdentifier> itemIDs, List<RecordIdentifier> barcodes, RecordIdentifier storeID, bool includeInventoryOnHand);

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
			RecordIdentifier specialGroup = null,
			bool includeVariants = true,
			string variantDescription = null,
			bool variantDescriptionBeginsWith = false,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false,
			List<SearchFlagEntity> searchFlags = null,
			bool isSearchBarControlSearch = false);

		/// <summary>
		/// Returns batches of item IDs that matches the search criterias.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="rowFrom"></param>
		/// <param name="rowTo"></param>
		/// <param name="totalRecords"></param>
		/// <returns></returns>
		List<RecordIdentifier> AdvancedSearchIDOnly(IConnectionManager entry,
			int rowFrom,
			int rowTo,
			out int totalRecords,
			string idOrDescription = null,
			bool idOrDescriptionBeginsWith = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null, // TODO remove this one
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
			bool includeVariants = true,
			string variantDescription = null,
			bool variantDescriptionBeginsWith = false,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false,
			List<SearchFlagEntity> searchFlags = null,
			bool isSearchBarControlSearch = false
			);

		List<SimpleRetailItem> AdvancedSearchOptimized(IConnectionManager entry,
			int rowFrom,
			int rowTo,
			SortEnum sort,
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
			RecordIdentifier specialGroup = null,
			bool includeVariants = true,
			SortEnum secondarySearch = SortEnum.None);

		List<RetailItem> AdvancedSearchFull(IConnectionManager entry,
			int rowFrom,
			int rowTo,
			SortEnum sort,
			out int totalRecordsMatching,
			string idOrDescription = null,
			bool idOrDescriptionBeginsWith = true,
			bool includeHeaders = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null,
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
			bool includeVariants = true,
			string VariantDescription = null,
			bool VariantDescriptionBeginsWith = false,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false,
			List<SearchFlagEntity> searchFlags = null,
			bool excludeItemswithSerialNumber = false);
		List<SimpleRetailItem> AdvancedSearch(IConnectionManager entry,
			int rowFrom,
			int rowTo,
			SortEnum sort,
			out int totalRecordsMatching,
			string idOrDescription = null,
			bool idOrDescriptionBeginsWith = true,
			bool includeHeaders = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null,
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
			bool includeVariants = true,
			string VariantDescription = null,
			bool VariantDescriptionBeginsWith = false,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false,
			List<SearchFlagEntity> searchFlags = null,
			SortEnum secondarySearch = SortEnum.None,
			ItemTypeEnum? itemTypeFilter = null,
			bool excludeItemswithSerialNumber = false,
			bool includeCannotBeSold = true,
			bool isSearchBarControlSearch = false
			);

		/// <summary>
		/// Searched for retail items that contain a given search text, and returns the results as a List of <see cref="SimpleRetailItem"/> . 
		/// Additionally this search function accepts a Dictionary with a combination of <see cref="RetailItemSearchEnum"/> and a string and resolves this list to add additional 
		/// search filters to the query
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="searchString">The item id or item name to search for</param>
		/// <param name="rowFrom"></param>
		/// <param name="rowTo"></param>
		/// <param name="beginsWith">Indicates wether you only want to search for items that begin with the given item name or item id</param>
		/// <param name="sort">The name of the column you want to sort by</param>
		/// <param name="advancedSearchParameters">Additional search parameters</param>
		/// <param name="includeVariants"></param>
		/// <returns></returns>
		List<SimpleRetailItem> AdvancedSearch(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith, SortEnum sort, Dictionary<RetailItemSearchEnum, string> advancedSearchParameters, bool includeVariants = true);

		List<SimpleRetailItem> TokenSearch(
			IConnectionManager entry,
			int rowFrom,
			int rowTo,
			SortEnum sort,
			out int totalRecordsMatching,
			List<string> searchTokens,
			bool idOrDescriptionBeginsWith = true,
			string culture = null,
			bool includeVariants = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null,
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null);

		List<DataEntity> TokenSearchDataEntity(
			IConnectionManager entry,
			int rowFrom,
			int rowTo,
			SortEnum sort,
			out int totalRecordsMatching,
			List<string> searchTokens,
			bool idOrDescriptionBeginsWith = true,
			string culture = null,
			bool includeVariants = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null,
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
			bool includeHeaders = true,
			bool includeServiceItems = true);


		List<MasterIDEntity> TokenSearchMasterDataEntity(
			IConnectionManager entry,
			int rowFrom,
			int rowTo,
			SortEnum sort,
			out int totalRecordsMatching,
			List<string> searchTokens,
			bool idOrDescriptionBeginsWith = true,
			string culture = null,
			bool includeVariants = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null,
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
			bool includeHeaders = true,
			bool includeServiceItems = true);
		List<DataEntity> GetForecourtItems(IConnectionManager entry, RecordIdentifier gradeID);


		/// <summary>
		/// Gets retail items in the system for a specific ID using a LIKE query
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">Item ID</param>
		/// <returns>The retail item, or null if not found</returns>
		List<RetailItem> GetItemsByItemPattern(IConnectionManager entry, string itemID);

		/// <summary>
		/// Gets all variants for a item as a list of DataEntities
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="itemID"></param>
		/// <returns></returns>
		List<DataEntity> GetItemVariantList(IConnectionManager entry, RecordIdentifier itemID);

		/// <summary>
		/// Retrieves all items that match the supplied attribute
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="attributesID">The attributeID</param>
		/// <returns></returns>
		List<SimpleRetailItem> GetRetailItemsFromAttribute(IConnectionManager entry, RecordIdentifier attributesID);

		/// <summary>
		/// Gets all variants for a item as a list of SimpleRetailItem
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="itemID"></param>
		/// <param name="sort"></param>
		/// <param name="showDeleted"></param>
		/// <returns></returns>
		List<SimpleRetailItem> GetItemVariants(
			IConnectionManager entry,
			RecordIdentifier itemID,
			SortEnum sort,
			bool showDeleted = false);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="itemID"></param>
		/// <param name="variants"></param>
		/// <param name="showDeleted"></param>
		/// <returns></returns>
		List<SimpleRetailItem> GetSpecificItemVariants(IConnectionManager entry,
			RecordIdentifier itemID,
			List<RecordIdentifier> variants,
			bool showDeleted = false);

		/// <summary>
		/// Gets all variants for a item as a list of MasterIDEntity
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="itemID"></param>
		/// <param name="attributeDescription"></param>
		/// <param name="attributeDescriptionStartsWith"></param>
		/// <returns></returns>
		List<MasterIDEntity> GetItemVariantsMasterID(IConnectionManager entry, RecordIdentifier itemID,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false);

		/// <summary>
		/// Gets all variants for a item as a list of MasterIDEntity
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="totalRecordsMatching"></param>
		/// <param name="itemID"></param>
		/// <param name="attributeDescription"></param>
		/// <param name="attributeDescriptionStartsWith"></param>
		/// <param name="rowFrom"></param>
		/// <param name="rowTo"></param>
		/// <param name="doCount"></param>
		/// <returns></returns>
		List<MasterIDEntity> GetItemVariantsMasterID(IConnectionManager entry,
			int rowFrom,
			int rowTo,
			bool doCount,
			out int totalRecordsMatching,
			RecordIdentifier itemID,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false);

		List<SearchFlagEntity> ItemSearchFlags { get; }
		/// <summary>
		/// Gets all variants for a item as a list of MasterIDDataEntity
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="itemID"></param>
		/// <returns></returns>
		List<MasterIDEntity> GetItemVariantMasterIDList(IConnectionManager entry, RecordIdentifier itemID);

		/// <summary>
		/// Gets all variants for a item
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		List<SimpleRetailItem> GetHeaderItems(IConnectionManager entry);

		/// <summary>
        /// Returns a list of SimpleRetailItem which only retrieves information from tables RetailItem, RetailGroup and RetailDepartment
        /// Deleted items are not included in this list
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        List<SimpleRetailItem> GetSimpleItems(IConnectionManager entry);

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
		RecordIdentifier GetItemsDefaultVendor(IConnectionManager entry, RecordIdentifier itemID);

		/// <summary>
		/// Returns true if the item has a default vendor
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The unique ID of the item</param>
		/// <returns>
		/// Returns true if the item has a default vendor
		/// </returns>
		bool ItemHasDefaultVendor(IConnectionManager entry, RecordIdentifier itemID);


        /// <summary>
        /// Sets a vendor as a default on a given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="vendorID">The unique ID of the vendor</param>
        void SetItemsDefaultVendor(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier vendorID);

		/// <summary>
		/// Returns item ids for all items that have a tax group with the given tax code ID
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="taxCodeID">ID of the tax code</param>
		/// <param name="rowFrom">startrow</param>
		/// <param name="rowTo">endrow</param>
		/// <param name="totalRecordsMatching">Total row count</param>

		List<RetailItem> GetItemsFromTaxCode(IConnectionManager entry, RecordIdentifier taxCodeID, int rowFrom, int rowTo, out int totalRecordsMatching);
		List<RetailItem> GetItemsFromTaxGroup(IConnectionManager entry, RecordIdentifier itemSalesTaxGroupID, int rowFrom, int rowTo, out int totalRecordsMatching);
		List<RetailItem> GetItemsWithTaxGroup(IConnectionManager entry, int rowFrom, int rowTo, out int totalRecordsMatching);

		List<DataEntity> FindItemDepartment(IConnectionManager entry, string searchText);
		List<DataEntity> FindItem(IConnectionManager entry, string searchText);

		RetailItemPrice GetItemPrice(IConnectionManager entry, RecordIdentifier itemID);

		/// <summary>
		/// Gets sales unit from given itemID.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">itemID, this can either be normal itemID or MasterID</param>
		/// <returns></returns>
		RecordIdentifier GetSalesUnitID(IConnectionManager entry, RecordIdentifier itemID);

		RetailItem Get(IConnectionManager entry, RecordIdentifier ID, CacheType cacheType = CacheType.CacheTypeNone);

		/// <summary>
		/// Return number of retail items
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <returns>The count of items</returns>
		int ItemCount(IConnectionManager entry);

		/// <summary>
		/// Adds an item - attribute connection for the given item master ID and attribute ID
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="retailItemID">The retail item master ID</param>
		/// <param name="attributeID">The ID of the dimension attribute</param>
		void AddDimensionAttribute(IConnectionManager entry, RecordIdentifier retailItemID, RecordIdentifier attributeID);

		/// <summary>
		/// Removes the item - attribute connection for the given item master ID and attribute ID
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="retailItemID">The retail item master ID</param>
		/// <param name="attributeID">The ID of the dimension attribute</param>
		void RemoveDimensionAttribute(IConnectionManager entry, RecordIdentifier retailItemID, RecordIdentifier attributeID);

		/// <summary>
		/// Searched for retail items that contain a given search text, and returns the results as a List of <see cref="DataEntity"/>
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="searchString">The text to search for. Searches in item name, in the ID field and the search alias field</param>
		/// <param name="rowFrom">The number of the first row to fetch</param>
		/// <param name="rowTo">The number of the last row to fetch</param>
		/// <param name="beginsWith">Specifies if the search text is the beginning of the text or if the text may contain the search text.</param>
		/// <param name="sort">A string defining the sort column</param>
		/// <param name="includeVariants"></param>
		/// <returns>A list of found items</returns>
		List<SimpleRetailItem> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith, SortEnum sort, bool includeVariants = true);

		/// <summary>
		/// Searches for the given search text, and returns the results as a list of <see
		/// cref="DataEntity" />
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="searchString">The value to search for</param>
		/// <param name="rowFrom">The number of the first row to fetch</param>
		/// <param name="rowTo">The number of the last row to fetch</param>
		/// <param name="culture">Language code of the included item translations</param>
		/// <param name="beginsWith">Specifies if the search text is the beginning of the
		/// text or if the text may contain the search text.</param>
		/// <param name="includeVariants"></param>
		List<DataEntity> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, string culture, bool beginsWith, bool includeVariants = true, bool includeServiceItems = true);

		/// <summary>
		/// Finds the item that matches the given attributes and returns the master ID of that item.
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="attributesIDs">A list of attribute IDs to match the item. This should be the unique combination of attributes that one and only one item matches</param>
		/// <returns>The ID of the item that matches the given attribute combination or null if no item is found</returns>
		RecordIdentifier GetMasterIDFromAttributes(IConnectionManager entry, List<RecordIdentifier> attributesIDs);

		/// <summary>
		/// Gets the master ID for the given item ID
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The item ID to get the master ID for</param>
		/// <returns></returns>
		RecordIdentifier GetMasterIDFromItemID(IConnectionManager entry, RecordIdentifier itemID);

		/// <summary>
		/// Undeletes the given item. This set the DELETED flag on the item to 0
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The ID of the item to undelete</param>
		void Undelete(IConnectionManager entry, RecordIdentifier itemID);

		/// <summary>
		/// Returns true if the item is marked as deleted
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The ID of the item to check</param>
		/// <returns>True if the item is marked as deleted, false otherwise</returns>
		bool IsDeleted(IConnectionManager entry, RecordIdentifier itemID);

		/// <summary>
		/// Returns the item type<see cref="ItemTypeEnum"/> for the given item ID
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The ID of the item to get the type for</param>
		/// <returns></returns>
		ItemTypeEnum GetItemType(IConnectionManager entry, RecordIdentifier itemID);

		/// <summary>
		/// Updates the type on a specific item
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The unique ID of the item to update</param>
		/// <param name="itemType">The new type</param>
		void UpdateItemType(IConnectionManager entry, RecordIdentifier itemID, ItemTypeEnum itemType);
	}
}