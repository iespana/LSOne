using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        /// <summary>
        /// Gets information about a specific vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID for the vendor being retrieved</param>
        /// <param name="deleted">If true we fetch vendors that have been deleted and if true vendors that are active</param>
        /// <returns>An object with information about the Vendor</returns>
        [OperationContract]
        Vendor GetVendor(LogonInfo logonInfo, RecordIdentifier vendorID, bool? deleted = null);

        /// <summary>
        /// Gets a list of all vendors
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns>A list of vendors</returns>
        [OperationContract]
        List<DataEntity> GetVendorList(LogonInfo logonInfo);


        /// <summary>
        /// Returns the sales tax group for the vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID for the vendor that is being checked</param>
        /// <returns>The ID of the tax group on the vendor</returns>
        [OperationContract]
        RecordIdentifier GetVendorsSalesTaxGroupID(LogonInfo logonInfo, RecordIdentifier vendorID);

        /// <summary>
        /// Deletes a list of vendors. When deleting a deleted flag is set to true on the vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="toDeleteList">A list of unique vendor IDs to be deleted</param>
        [OperationContract]
        void DeleteVendors(LogonInfo logonInfo, List<RecordIdentifier> toDeleteList);

        /// <summary>
        /// Deletes all vendor items for the given retail item ID.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="logonInfo">Entry into the database</param>
        /// <param name="toDeleteList">The IDs of the retail items</param>
        /// <param name="vendorID">The ID of the vendor</param>
        [OperationContract]
        void DeleteVendorItemByRetailItemID(LogonInfo logonInfo, List<MasterIDEntity> toDeleteList,
            RecordIdentifier vendorID);

        /// <summary>
        /// Deletes a list of items
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="toDeleteList">A list of unique item IDs to be deleted</param>
        [OperationContract]
        void DeleteVendorItems(LogonInfo logonInfo, List<RecordIdentifier> toDeleteList);

        /// <summary>
        /// Deletes an item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="toDelete">A unique item ID to be deleted</param>
        [OperationContract]
        void DeleteVendorItem(LogonInfo logonInfo, RecordIdentifier toDelete);

        /// <summary>
        /// Returns a list of vendor contacts
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">ID of the vendor</param>
        /// <returns></returns>
        [OperationContract]
        List<Contact> GetVendorContactList(LogonInfo logonInfo, RecordIdentifier vendorID);

        /// <summary>
        /// Returns a vendor contact
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="contactID">ID of the contact</param>
        /// <returns></returns>
        [OperationContract]
        Contact GetVendorContact(LogonInfo logonInfo, RecordIdentifier contactID);

        /// <summary>
        /// Delete a vendor contact
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="contactsToDelete">IDs of the contacts to delete</param>
        [OperationContract]
        void DeleteVendorContact(LogonInfo logonInfo, List<RecordIdentifier> contactsToDelete);

        /// <summary>
        /// Saves or updates a vendor contact
        /// </summary>        
        /// /// <param name="logonInfo">The login information for the database</param>
        /// <param name="contact">Contact to save or update</param>
        [OperationContract]
        SaveContactResultEnum SaveVendorContact(LogonInfo logonInfo, Contact contact);

        /// <summary>
        /// Activates a list of vendors. When activating the  deleted flag is set to false on the vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="toActivateList">A list of unique vendor IDs to be activated</param>
        [OperationContract]
        void RestoreVendors(LogonInfo logonInfo, List<RecordIdentifier> toActivateList);

        /// <summary>
        /// Returns a list of vendors that fulfill the conditions in the search criteria paramteer
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="searchCriteria">The conditions that should be applied to the search</param>
        /// <returns>A list of items that apply to the search conditions</returns>
        [OperationContract]
        List<Vendor> GetVendors(LogonInfo logonInfo, VendorSearch searchCriteria);

        /// <summary>
        /// Returns a list of vendors that have not been deleted
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of vendors</returns>
        [OperationContract]
        List<Vendor> GetVendorsList(LogonInfo logonInfo, VendorSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Checks if a specific ID already exists for a vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID to check</param>
        /// <returns>True if the vendor ID already exists</returns>
        [OperationContract]
        bool VendorExists(LogonInfo logonInfo, RecordIdentifier vendorID);

        /// <summary>
        /// Saves the vendor information
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendor">The information to be saved</param>
        [OperationContract]
        void SaveVendor(LogonInfo logonInfo, Vendor vendor);

        /// <summary>
        /// Saves and returns the vendor information
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendor">The information to be saved</param>
        /// <returns>Vendor</returns>
        [OperationContract]
        Vendor SaveAndReturnVendor(LogonInfo logonInfo, Vendor vendor);

        /// <summary>
        ///  Get a vendor item based on the internal vendor ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="internalID">The unique ID for the vendor item</param>
        /// <returns></returns>
        [OperationContract(Name="GetVendorItem")]
        VendorItem GetVendorItem(LogonInfo logonInfo, RecordIdentifier internalID);

        /// <summary>
        /// Get a vendor item based on vendor, item and unit ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="itemID">The unique ID for the item</param>
        /// <param name="unitID">The unique ID for the unit</param>
        /// <returns></returns>
        [OperationContract(Name="GetVendorItemByItemAndUnit")]
        VendorItem GetVendorItem(LogonInfo logonInfo, RecordIdentifier vendorID, RecordIdentifier itemID, RecordIdentifier unitID);

        /// <summary>
        /// Each item can be added multiple times to a vendor with different unit ID's. This function returns the first one found 
        /// and should only be used when the unit ID is not available.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">>The unique ID for the vendor</param>
        /// <param name="itemID">The unique ID for the item</param>
        /// <returns></returns>
        [OperationContract]
        VendorItem GetFirstVendorItem(LogonInfo logonInfo, RecordIdentifier vendorID, RecordIdentifier itemID);

        /// <summary>
        /// Gets a list of DataEntities that contains distinct list of retail item ID's and Item Description for a given vendorID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">ID of the vendor to get the list for</param>
        /// <returns></returns>
        [OperationContract]
        List<MasterIDEntity> GetDistinctRetailItemsForVendor(LogonInfo logonInfo, RecordIdentifier vendorID);


        /// <summary>
        /// Checks if a vendor item by a given Vendor ID and vendor item ID exists, excluding current record from the check
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="vendorItemID">ID of the vendor item to check for (this is external vendor specific id)</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <returns>True if the vendor item exists, else false</returns>
        [OperationContract]
        bool VendorItemExistsExcludingCurrentRecord(LogonInfo logonInfo, RecordIdentifier vendorID, RecordIdentifier vendorItemID, RecordIdentifier oldRecordID);

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID, retailID and unitID, excluding current record from the check
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="retailID">ID of the retail item</param>
        /// <param name="unitID">ID of the variant</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <returns>True if the vendor item exists, else false</returns>
        [OperationContract]
        bool VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitID(LogonInfo logonInfo,
            RecordIdentifier vendorID, RecordIdentifier retailID, RecordIdentifier unitID, RecordIdentifier oldRecordID);

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID, retailID, unitID and vendorid, excluding current record from the check
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="retailID">ID of the retail item</param>
        /// <param name="unitID">ID of the variant</param>
        /// <param name="vendorItemID">ID of the vendor item to check for (this is external vendor specific id)</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <returns>True if the vendor item exists, else false</returns>
        [OperationContract]
        bool VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitIDAndVendorItemID(LogonInfo logonInfo,
            RecordIdentifier vendorID, RecordIdentifier retailID, RecordIdentifier unitID, RecordIdentifier vendorItemID, RecordIdentifier oldRecordID);

        /// <summary>
        /// Returns a list of items that has been assigned to a specific vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="sorting">Sorting of the vendor item list</param>
        /// <param name="sortBackwards">Ascending or descending</param>
        /// <returns></returns>
        [OperationContract]
        List<VendorItem> GetItemsForVendor(LogonInfo logonInfo, RecordIdentifier vendorID, VendorItemSorting sorting,
            bool sortBackwards);

        /// <summary>
        /// Returns a paged list of items that has been assigned to a specific vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="sorting">Sorting of the vendor item list</param>
        /// <param name="sortBackwards">Ascending or descending</param>
        /// <param name="startRecord">Pagination start index</param>
        /// <param name="endRecord">Pagination end index</param>
        /// <param name="totalRecords">Total number of records for the given vendor</param>
        /// <returns></returns>
        [OperationContract]
        List<VendorItem> GetPagedItemsForVendor(LogonInfo logonInfo,
                                               RecordIdentifier vendorID,
                                               VendorItemSorting sorting, bool sortBackwards,
                                               int startRecord, int endRecord,
                                               out int totalRecords);

        /// <summary>
        /// Saves a vendor item 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorItem">The vendor item to be saved</param>
        /// <returns>Internal ID of the saved vendor</returns>
        [OperationContract]
        RecordIdentifier SaveVendorItem(LogonInfo logonInfo, VendorItem vendorItem);

        /// <summary>
        /// Returns a list of all units that are available for the item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="itemID">The unique ID for the item</param>
        /// <returns></returns>
        [OperationContract]
        List<Unit> GetDistinctUnitsForVendorItem(LogonInfo logonInfo, RecordIdentifier vendorID, RecordIdentifier itemID);


        /// <summary>
        /// Checks if the vendor has the item availble
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The vendor</param>
        /// <param name="retaiID">The item</param>
        /// <param name="unitID">The unit </param>
        /// <param name="oldRecordID">Ids to exclude</param>
        /// <returns></returns>
        [OperationContract]
        bool VendorItemExists(LogonInfo logonInfo, RecordIdentifier vendorID, RecordIdentifier retaiID,
            RecordIdentifier unitID, RecordIdentifier oldRecordID);

        /// <summary>
        /// Sets the default Vendor for an item 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID">The item </param>
        /// <param name="vendorItemID">The vendor</param>
        [OperationContract]
        void SetItemsDefaultVendor(LogonInfo logonInfo, RecordIdentifier itemID,
            RecordIdentifier vendorItemID);

        /// <summary>
        /// Checks if the item has a default vendor
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        [OperationContract]
        bool ItemHasDefaultVendor(LogonInfo logonInfo, RecordIdentifier itemID);

        /// <summary>
        /// Get the default vendor of a retail item
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        [OperationContract]
        RecordIdentifier GetItemsDefaultVendor(LogonInfo logonInfo, RecordIdentifier itemID);

        /// <summary>
        /// Removes the supplied retil item from the supplied vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="retailItemID">The item</param>
        /// <param name="vendorID">The vendor</param>
        /// <param name="unitID">The unit of the item to remove</param>
        [OperationContract]
        void DeleteByRetailItemID(LogonInfo logonInfo, RecordIdentifier retailItemID, RecordIdentifier vendorID,
            RecordIdentifier unitID);

        /// <summary>
        /// Gets the vendors for the supplied item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="retailItemID">The retail item</param>
        /// <param name="sortBy">How to sort</param>
        /// <param name="sortBackwards">The sort direction</param>
        /// <returns></returns>
        [OperationContract]
        List<VendorItem> GetVendorsForItem(LogonInfo logonInfo, RecordIdentifier retailItemID, VendorItemSorting sortBy,
            bool sortBackwards);

        /// <summary>
        /// Sets the default contact on the vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID of the vendor to be updated</param>
        /// <param name="contactID">The unique ID of the contact that is to be the default contact on the vendor</param>
        [OperationContract]
        void SetDefaultContactOnVendor(LogonInfo logonInfo, RecordIdentifier vendorID, RecordIdentifier contactID);

        /// <summary>
        /// Retrieves the default purchase price for a vendor item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID of the vendor to be updated</param>
        /// <param name="itemID">The unique ID of the vendor item being checked</param>
        /// <param name="unitID">The unit ID on the vendor item being checked</param>
        [OperationContract]
        decimal GetDefaultPurchasePrice(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier vendorID, RecordIdentifier unitID);

        /// <summary>
        /// Retrieves the latest purchase price for a vendor item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID of the vendor to be updated</param>
        /// <param name="retailItemID">The unique ID of the vendor item being checked</param>
        /// <param name="unitID">The unit ID on the vendor item being checked</param>
        [OperationContract]
        decimal GetLatestPurchasePrice(LogonInfo logonInfo, RecordIdentifier retailItemID, RecordIdentifier vendorID, RecordIdentifier unitID);

        [OperationContract]
        bool VendorHasItem(LogonInfo logonInfo, RecordIdentifier retailItemID, RecordIdentifier vendorID);

        /// <summary>
        /// This procedure checks if vendor can be deleted.
        /// A vendor cannot be deleted if a purchase order, a goods receiving document or purchase order worksheet that is open and is attached to the vendor
        /// A vendor can not be deleted if it is attached to a purchase order that is posted and there is no goods receiving document attached to the purchase order i.e.both the purchase order and goods receiving document have to be posted for the vendor to be deleted.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendors">vendor ids to be checked</param>
        /// <param name="linkedVendors">list of vendors that cannot be deleted </param>
        /// <returns>True if all vendors from the list can be deleted. False otherwise</returns>
        [OperationContract]
        bool DeleteVendorsCanExecute(LogonInfo logonInfo, List<RecordIdentifier> vendors, out List<RecordIdentifier> linkedVendors);

        /// <summary>
        /// Verifies if a vendor is linked to at least one item or is the default vendor for an item.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">Unique ID of the vendor</param>
        /// <returns></returns>
        [OperationContract]
        VendorItemsLinkedType VendorHasLinkedItems(LogonInfo logonInfo, RecordIdentifier vendorID);

        /// <summary>
        /// Deletes all vendor - item links and removes vendor as default vendor from items.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">Unique ID of the vendor</param>
        [OperationContract]
        void DeleteVendorItemLinks(LogonInfo logonInfo, RecordIdentifier vendorID);

        /// <summary>
        /// Searches the vendor item for the given criteria
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="searchCriteria">Criterias for search</param>
        /// <param name="sortBy">Sort field for returned vendor items</param>
        /// <param name="sortBackwards">Sort direction of the returned vendor items</param>
        /// <param name="startRecord">Pagination start index</param>
        /// <param name="endRecord">Pagination end index</param>
        /// <param name="totalRecords">Total number of records that match the search criteria</param>
        /// <returns></returns>
        [OperationContract]
        List<VendorItem> AdvancedSearch(LogonInfo logonInfo,
                                        VendorItemSearch searchCriteria,
                                        VendorItemSorting sortBy, bool sortBackwards,
                                        int startRecord, int endRecord,
                                        out int totalRecords);
    }
}
