using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService
    {
        /// <summary>
        /// Gets information about a specific vendor
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID for the vendor being retrieved</param>
        /// <param name="deleted">If true we fetch vendors that have been deleted and if true vendors that are active</param>
        /// <returns>An object with information about the Vendor</returns>
        Vendor GetVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection, bool? deleted = null);

        /// <summary>
        /// Returns the sales tax group for the vendor
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID for the vendor that is being checked</param>
        /// <returns>The ID of the tax group on the vendor</returns>
        RecordIdentifier GetVendorsSalesTaxGroupID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection);

        /// <summary>
        /// Gets a list of all vendors
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>A list of vendors</returns>
        List<DataEntity> GetVendorList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);

        /// <summary>
        /// Deletes a list of vendors. When deleting a deleted flag is set to true on the vendor
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="toDeleteList">A list of unique vendor IDs to be deleted</param>
        void DeleteVendors(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> toDeleteList, bool closeConnection);

        /// <summary>
        /// Deletes all vendor items for the given retail item ID.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="toDeleteList">The IDs of the retail items</param>
        /// <param name="vendorID">The ID of the vendor</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void DeleteVendorItemByRetailItemID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<MasterIDEntity> toDeleteList,
            RecordIdentifier vendorID, bool closeConnection);

        /// <summary>
        /// Deletes a list of items
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="toDeleteList">A list of unique item IDs to be deleted</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void DeleteVendorItems(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> toDeleteList, bool closeConnection);

        void DeleteVendorItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier toDelete, bool closeConnection);

        /// <summary>
        /// Delete a vendor contact
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="contactsToDelete">IDs of the contacts to delete</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void DeleteVendorContact(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> contactsToDelete, bool closeConnection);

        /// <summary>
        /// Returns a list of vendor contacts
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="vendorID">ID of the vendor</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        List<Contact> GetVendorContactList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection);

        /// <summary>
        /// Returns a vendor contact
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="contactID">ID of the contact</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        Contact GetVendorContact(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier contactID, bool closeConnection);

        /// <summary>
        /// Saves or updates a vendor contact
        /// </summary>          
        /// /// <param name="entry">The entry into the database</param>   
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>   
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="contact">Contact to save or update</param>
        SaveContactResultEnum SaveVendorContact(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Contact contact, bool closeConnection);

        /// <summary>
        /// Activates a list of vendors. When activating the  deleted flag is set to false on the vendor
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="toActivateList">A list of unique vendor IDs to be activated</param>
        void RestoreVendors(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> toActivateList, bool closeConnection);

        /// <summary>
        /// Returns a list of vendors that fulfill the conditions in the search criteria paramteer
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="searchCriteria">The conditions that should be applied to the search</param>
        /// <returns>A list of items that apply to the search conditions</returns>
        List<Vendor> GetVendors(IConnectionManager entry, SiteServiceProfile siteServiceProfile, VendorSearch searchCriteria, bool closeConnection);

        /// <summary>
        /// Returns a list of vendors that have not been deleted
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>A list of vendors</returns>
        List<Vendor> GetVendorsList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, VendorSorting sortBy, bool sortBackwards, bool closeConnection);

        /// <summary>
        /// Checks if a specific ID already exists for a vendor
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID to check</param>
        /// <returns>True if the vendor ID already exists</returns>
        bool VendorExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection);

        /// <summary>
        /// Saves the vendor information
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendor">The information to be saved</param>
        void SaveVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Vendor vendor, bool closeConnection);

        /// <summary>
        /// Saves and returns the vendor information
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="vendor">The information to be saved</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Vendor</returns>
        Vendor SaveAndReturnVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Vendor vendor, bool closeConnection);

        /// <summary>
        /// Get a vendor item based on the internal vendor ID
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="internalID">The unique ID for the vendor item</param>
        /// <returns></returns>
        VendorItem GetVendorItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier internalID, bool closeConnection);

        /// <summary>
        /// Get a vendor item based on vendor, item and unit ID
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="itemID">The unique ID for the item</param>
        /// <param name="unitID">The unique ID for the unit</param>
        /// <returns></returns>
        VendorItem GetVendorItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, RecordIdentifier itemID, RecordIdentifier unitID, bool closeConnection);

        /// <summary>
        /// Each item can be added multiple times to a vendor with different unit ID's. This function returns the first one found 
        /// and should only be used when the unit ID is not available.
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="itemID">The unique ID for the item</param>
        /// <returns>The first vendor item found</returns>
        VendorItem GetFirstVendorItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, RecordIdentifier itemID, bool closeConnection);

        /// <summary>
        /// Gets a list of DataEntities that contains distinct list of retail item ID's and Item Description for a given vendorID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="vendorID">ID of the vendor to get the list for</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        List<MasterIDEntity> GetDistinctRetailItemsForVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection);

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID and vendor item ID exists, excluding current record from the check
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="vendorItemID">ID of the vendor item to check for (this is external vendor specific id)</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>True if the vendor item exists, else false</returns>
        bool VendorItemExistsExcludingCurrentRecord(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, RecordIdentifier vendorItemID, RecordIdentifier oldRecordID, bool closeConnection);

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID, retailID and unitID, excluding current record from the check
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="retailID">ID of the retail item</param>
        /// <param name="unitID">ID of the variant</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>True if the vendor item exists, else false</returns>
        bool VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitID(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier vendorID, RecordIdentifier retailID, RecordIdentifier unitID, RecordIdentifier oldRecordID, bool closeConnection);

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID, retailID and unitID, excluding current record from the check
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="retailID">ID of the retail item</param>
        /// <param name="unitID">ID of the variant</param>
        /// <param name="vendorItemID">ID of the vendor item to check for (this is external vendor specific id)</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>True if the vendor item exists, else false</returns>
        bool VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitIDAndVendorItemID(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier vendorID, RecordIdentifier retailID, RecordIdentifier unitID, RecordIdentifier vendorItemID, RecordIdentifier oldRecordID, bool closeConnection);

        /// <summary>
        /// Returns a list of items that has been assigned to a specific vendor
        /// </summary>
        /// <param name="siteServiceProfile"></param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="sorting">Sorting of the vendor item list</param>
        /// <param name="sortBackwards">Ascending or descending</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        List<VendorItem> GetItemsForVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier vendorID, VendorItemSorting sorting, bool sortBackwards, bool closeConnection);

        /// <summary>
        /// Returns a paginated list of items that has been assigned to a specific vendor
        /// </summary>
        /// <param name="siteServiceProfile"></param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="sorting">Sorting of the vendor item list</param>
        /// <param name="sortBackwards">Ascending or descending</param>
        /// <param name="startRecord">Pagination start index</param>
        /// <param name="endRecord">Pagination end index</param>
        /// <param name="totalRecords">Total number of records for the given vendor</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        List<VendorItem> GetItemsForVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
                                           RecordIdentifier vendorID, 
                                           VendorItemSorting sorting, bool sortBackwards, 
                                           int startRecord, int endRecord, 
                                           out int totalRecords,
                                           bool closeConnection);

        /// <summary>
        /// Saves a vendor item 
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorItem">The vendor item to be saved</param>
        /// <returns>Internal ID of the saved vendor</returns>
        RecordIdentifier SaveVendorItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, VendorItem vendorItem, bool closeConnection);

        /// <summary>
        /// Returns a list of all units that are available for the item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="itemID">The unique ID for the item</param>
        /// <returns></returns>
        List<Unit> GetDistinctUnitsForVendorItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, RecordIdentifier itemID, bool closeConnection);


        /// <summary>
        /// Searches for items that are part of the purchase order
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="purchaseOrderID">The ID of the purchase order</param>
        /// <param name="searchString">The string to search for</param>
        /// <param name="rowFrom">The start row</param>
        /// <param name="rowTo">The end row</param>
        /// <param name="beginsWith">If the way to compare the search string </param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        List<DataEntity> SearchItemsInPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, string searchString, int rowFrom,
            int rowTo, bool beginsWith, bool closeConnection);

        /// <summary>
        /// Checks if the vendor has the item availble
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="vendorID">The vendor</param>
        /// <param name="retaiID">The item</param>
        /// <param name="unitID">The unit </param>
        /// <param name="oldRecordID">Ids to exclude</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        bool VendorItemExists(IConnectionManager entry,
         SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, RecordIdentifier retaiID,
        RecordIdentifier unitID, RecordIdentifier oldRecordID, bool closeConnection);

        /// <summary>
        /// Removes the supplied retil item from the supplied vendor
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>      
        /// <param name="retailItemID">The item</param>
        /// <param name="vendorID">The vendor</param>
        /// <param name="unitID">The unit of the item to remove</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void DeleteByRetailItemID(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier retailItemID, RecordIdentifier vendorID,
            RecordIdentifier unitID, bool closeConnection);

        /// <summary>
        /// Gets the vendors for the supplied item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>      
        /// <param name="retailItemID">The retail item</param>
        /// <param name="sortBy">How to sort</param>
        /// <param name="sortBackwards">The sort direction</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        List<VendorItem> GetVendorsForItem(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier retailItemID, VendorItemSorting sortBy,
            bool sortBackwards, bool closeConnection);

        /// <summary>
        /// Sets the default contact on the vendor
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID of the vendor to be updated</param>
        /// <param name="contactID">The unique ID of the contact that is to be the default contact on the vendor</param>
        void SetDefaultContactOnVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, RecordIdentifier contactID, bool closeConnection);

        void SetItemsDefaultVendor(IConnectionManager entry,
                SiteServiceProfile siteServiceProfile, RecordIdentifier itemID,
                RecordIdentifier vendorItemID, bool closeConnection);


        bool ItemHasDefaultVendor(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, bool closeConnection);

        /// <summary>
        /// Returns the default vendor for a given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>
        /// Returns the default vendor ID
        /// </returns>
        RecordIdentifier GetItemsDefaultVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, bool closeConnection);

        /// <summary>
        /// Retrieves the default purchase price for a vendor item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID of the vendor to be updated</param>
        /// <param name="itemID">The unique ID of the vendor item being checked</param>
        /// <param name="unitID">The unit ID on the vendor item being checked</param>
        decimal GetDefaultPurchasePrice(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier vendorID, RecordIdentifier unitID, bool closeConnection);

        decimal GetLatestPurchasePrice(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier retailItemID, RecordIdentifier vendorID, RecordIdentifier unitID, bool closeConnection);
        bool VendorHasItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier retailItemID, RecordIdentifier vendorID, bool closeConnection);

        /// <summary>
        /// This procedure checks if vendor can be deleted.
        /// A vendor cannot be deleted if a purchase order, a goods receiving document or purchase order worksheet that is open and is attached to the vendor
        /// A vendor can not be deleted if it is attached to a purchase order that is posted and there is no goods receiving document attached to the purchase order i.e.both the purchase order and goods receiving document have to be posted for the vendor to be deleted.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="vendors">vendor ids to be checked</param>
        /// <param name="linkedVendors">list of vendors that cannot be deleted </param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>True if all vendors from the list can be deleted. False otherwise</returns>
        bool DeleteVendorsCanExecute(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> vendors, out List<RecordIdentifier> linkedVendors, bool closeConnection);

        /// <summary>
        /// Verifies if a vendor is linked to at least one item or is the default vendor for an item.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">Unique ID of the vendor</param>
        /// <returns></returns>
        VendorItemsLinkedType VendorHasLinkedItems(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection);

        /// <summary>
        /// Deletes all vendor - item links and removes vendor as default vendor from items.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">Unique ID of the vendor</param>
        void DeleteVendorItemLinks(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection);

        /// <summary>
        /// Searches the vendor item for the given criteria
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="searchCriteria">Criterias for search</param>
        /// <param name="sortBy">Sort field for returned vendor items</param>
        /// <param name="sortBackwards">Sort direction of the returned vendor items</param>
        /// <param name="startRecord">Pagination start index</param>
        /// <param name="endRecord">Pagination end index</param>
        /// <param name="totalRecords">Total number of records that match the search criteria</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        List<VendorItem> AdvancedSearch(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
                                        VendorItemSearch searchCriteria,
                                        VendorItemSorting sortBy, bool sortBackwards,
                                        int startRecord, int endRecord,
                                        out int totalRecords,
                                        bool closeConnection);
    }
}
