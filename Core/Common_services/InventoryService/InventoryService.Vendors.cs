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
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class InventoryService
    {
        /// <summary>
        /// Gets information about a specific vendor
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID for the vendor being retrieved</param>
        /// <returns>An object with information about the Vendor</returns>
        public virtual Vendor GetVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetVendor(entry, siteServiceProfile, vendorID, closeConnection);
        }

        /// <summary>
        /// Returns the sales tax group for the vendor
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID for the vendor that is being checked</param>
        /// <returns>The ID of the tax group on the vendor</returns>
        public virtual RecordIdentifier GetVendorsSalesTaxGroupID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetVendorsSalesTaxGroupID(entry, siteServiceProfile, vendorID, closeConnection);
        }

        /// <summary>
        /// Gets a list of all vendors
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>A list of vendors</returns>
        public virtual List<DataEntity> GetVendorList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetVendorList(entry, siteServiceProfile, closeConnection);
        }

        /// <summary>
        /// Deletes a list of vendors. When deleting a deleted flag is set to true on the vendor
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="toDeleteList">A list of unique vendor IDs to be deleted</param>
        public virtual void DeleteVendors(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> toDeleteList, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteVendors(entry, siteServiceProfile, toDeleteList, closeConnection);
        }

        /// <summary>
        /// Deletes all vendor items for the given retail item ID.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="toDeleteList">The IDs of the retail items</param>
        /// <param name="vendorID">The ID of the vendor</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        public virtual void DeleteVendorItemByRetailItemID(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            List<MasterIDEntity> toDeleteList,
            RecordIdentifier vendorID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteVendorItemByRetailItemID(entry, siteServiceProfile, toDeleteList, vendorID, closeConnection);
        }

        /// <summary>
        /// Deletes a list of items
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="toDeleteList">A list of unique item IDs to be deleted</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        public virtual void DeleteVendorItems(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> toDeleteList, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteVendorItems(entry, siteServiceProfile, toDeleteList, closeConnection);
        }

        public virtual void DeleteVendorItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier toDelete,
            bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteVendorItem(entry, siteServiceProfile, toDelete, closeConnection);
        }

        /// <summary>
        /// Delete a vendor contact
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="contactsToDelete">IDs of the contacts to delete</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        public virtual void DeleteVendorContact(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> contactsToDelete, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteVendorContact(entry, siteServiceProfile, contactsToDelete, closeConnection);
        }

        /// <summary>
        /// Returns a list of vendor contacts
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="vendorID">ID of the vendor</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual List<Contact> GetVendorContactList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetVendorContactList(entry, siteServiceProfile, vendorID, closeConnection);
        }

        /// <summary>
        /// Returns a vendor contact
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="contactID">ID of the contact</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual Contact GetVendorContact(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier contactID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetVendorContact(entry, siteServiceProfile, contactID, closeConnection);
        }

        /// <summary>
        /// Saves or updates a vendor contact
        /// </summary>          
        /// /// <param name="entry">The entry into the database</param>   
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>   
        /// <param name="contact">Contact to save or update</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual SaveContactResultEnum SaveVendorContact(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Contact contact, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).SaveVendorContact(entry, siteServiceProfile, contact, closeConnection);
        }

        /// <summary>
        /// Activates a list of vendors. When activating the  deleted flag is set to false on the vendor
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="toActivateList">A list of unique vendor IDs to be activated</param>
        public virtual void RestoreVendors(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> toActivateList, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).RestoreVendors(entry, siteServiceProfile, toActivateList, closeConnection);
        }

        /// <summary>
        /// Returns a list of vendors that fulfill the conditions in the search criteria paramteer
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="searchCriteria">The conditions that should be applied to the search</param>
        /// <returns>A list of items that apply to the search conditions</returns>
        public virtual List<Vendor> GetVendors(IConnectionManager entry, SiteServiceProfile siteServiceProfile, VendorSearch searchCriteria, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetVendors(entry, siteServiceProfile, searchCriteria, closeConnection);
        }

        /// <summary>
        /// Returns a list of vendors that have not been deleted
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns></returns>
        public virtual List<Vendor> GetVendorsList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, VendorSorting sortBy, bool sortBackwards, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetVendorsList(entry, siteServiceProfile, sortBy, sortBackwards, closeConnection);
        }


        /// <summary>
        /// Checks if a specific ID already exists for a vendor
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID to check</param>
        /// <returns>True if the vendor ID already exists</returns>
        public virtual bool VendorExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).VendorExists(entry, siteServiceProfile, vendorID, closeConnection);
        }

        /// <summary>
        /// Saves the vendor information
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendor">The information to be saved</param>
        public virtual void SaveVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Vendor vendor, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).SaveVendor(entry, siteServiceProfile, vendor, closeConnection);
        }

        /// <summary>
        /// Saves and returns the vendor information
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="vendor">The information to be saved</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Vendor</returns>
        public virtual Vendor SaveAndReturnVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            Vendor vendor, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).SaveAndReturnVendor(entry, siteServiceProfile, vendor, closeConnection);
        }

        /// <summary>
        /// Get a vendor item based on the internal vendor ID
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="internalID">The unique ID for the vendor item</param>
        /// <returns></returns>
        public virtual VendorItem GetVendorItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier internalID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetVendorItem(entry, siteServiceProfile, internalID, closeConnection);
        }

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
        public VendorItem GetVendorItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, RecordIdentifier itemID, RecordIdentifier unitID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetVendorItem(entry, siteServiceProfile, vendorID, itemID, unitID, closeConnection);
        }

        /// <summary>
        /// Saves a vendor item 
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorItem">The vendor item to be saved</param>
        /// <returns>Internal ID of the saved vendor</returns>
        public virtual RecordIdentifier SaveVendorItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, VendorItem vendorItem, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).SaveVendorItem(entry, siteServiceProfile, vendorItem, closeConnection);
        }

        /// <summary>
        /// Returns a list of all units that are available for the item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="itemID">The unique ID for the item</param>
        /// <returns></returns>
        public virtual List<Unit> GetDistinctUnitsForVendorItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, RecordIdentifier itemID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetDistinctUnitsForVendorItem(entry, siteServiceProfile, vendorID, itemID, closeConnection);
        }

        /// <summary>
        /// Sets the default contact on the vendor
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID of the vendor to be updated</param>
        /// <param name="contactID">The unique ID of the contact that is to be the default contact on the vendor</param>
        public virtual void SetDefaultContactOnVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, RecordIdentifier contactID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).SetDefaultContactOnVendor(entry, siteServiceProfile, vendorID, contactID, closeConnection);
        }

        /// <summary>
        /// Gets a list of DataEntities that contains distinct list of retail item ID's and Item Description for a given vendorID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="vendorID">ID of the vendor to get the list for</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual List<MasterIDEntity> GetDistinctRetailItemsForVendor(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetDistinctRetailItemsForVendor(entry, siteServiceProfile, vendorID, closeConnection);
        }

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
        public virtual VendorItem GetFirstVendorItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, RecordIdentifier itemID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetFirstVendorItem(entry, siteServiceProfile, vendorID, itemID, closeConnection);
        }

        public virtual List<VendorItem> GetItemsForVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, VendorItemSorting sorting, bool sortBackwards, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetItemsForVendor(entry, siteServiceProfile, vendorID, sorting, sortBackwards, closeConnection);
        }

        public virtual List<VendorItem> GetItemsForVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
                                                          RecordIdentifier vendorID,
                                                          VendorItemSorting sorting, bool sortBackwards,
                                                          int startRecord, int endRecord,
                                                          out int totalRecords,
                                                          bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetItemsForVendor(entry, siteServiceProfile,
                                                                                   vendorID,
                                                                                   sorting, sortBackwards,
                                                                                   startRecord, endRecord,
                                                                                   out totalRecords,
                                                                                   closeConnection);
        }

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID, retailID, unitID and vendorid, excluding current record from the check
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

        public bool VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitIDAndVendorItemID(IConnectionManager entry,
         SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, RecordIdentifier retailID, RecordIdentifier unitID,
         RecordIdentifier vendorItemID, RecordIdentifier oldRecordID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitIDAndVendorItemID(entry, siteServiceProfile, vendorID, retailID, unitID, vendorItemID, oldRecordID, closeConnection);
        }
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
        public virtual bool VendorItemExistsExcludingCurrentRecord(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, RecordIdentifier vendorItemID,
            RecordIdentifier oldRecordID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).VendorItemExistsExcludingCurrentRecord(entry, siteServiceProfile, vendorID, vendorItemID, oldRecordID, closeConnection);
        }

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
        public virtual bool VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitID(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier vendorID, RecordIdentifier retailID, RecordIdentifier unitID, RecordIdentifier oldRecordID,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitID(entry, siteServiceProfile, vendorID, retailID, unitID, oldRecordID, closeConnection);
        }

        public virtual void DeleteByRetailItemID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier retailItemID,
            RecordIdentifier vendorID, RecordIdentifier unitID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteByRetailItemID(entry, siteServiceProfile, retailItemID, vendorID, unitID, closeConnection);

        }

        public virtual List<VendorItem> GetVendorsForItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier retailItemID,
            VendorItemSorting sortBy, bool sortBackwards, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetVendorsForItem(entry, siteServiceProfile, retailItemID, sortBy, sortBackwards, closeConnection);
        }

        public virtual bool VendorItemExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID,
        RecordIdentifier retaiID, RecordIdentifier unitID, RecordIdentifier oldRecordID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).VendorItemExists(entry, siteServiceProfile, vendorID, retaiID, unitID, oldRecordID, closeConnection);
        }

        public virtual void SetItemsDefaultVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID,
            RecordIdentifier vendorItemID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).SetItemsDefaultVendor(entry, siteServiceProfile, itemID, vendorItemID, closeConnection);
        }

        public virtual RecordIdentifier GetItemsDefaultVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetItemsDefaultVendor(entry, siteServiceProfile, itemID, closeConnection);
        }

        public virtual bool ItemHasDefaultVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).ItemHasDefaultVendor(entry, siteServiceProfile, itemID, closeConnection);
        }

        /// <summary>
        /// Retrieves the default purchase price for a vendor item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">The unique ID of the vendor to be updated</param>
        /// <param name="itemID">The unique ID of the vendor item being checked</param>
        /// <param name="unitID">The unit ID on the vendor item being checked</param>
        public virtual decimal GetDefaultPurchasePrice(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier vendorID, RecordIdentifier unitID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetDefaultPurchasePrice(entry, siteServiceProfile, itemID, vendorID, unitID, closeConnection);
        }

        public virtual decimal GetLatestPurchasePrice(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier retailItemID, RecordIdentifier vendorID, RecordIdentifier unitID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry)
                .GetLatestPurchasePrice(entry, siteServiceProfile, retailItemID, vendorID, unitID, closeConnection);
        }

        public virtual bool VendorHasItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier retailItemID,
            RecordIdentifier vendorID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry)
                    .VendorHasItem(entry, siteServiceProfile, retailItemID, vendorID, closeConnection);
        }

        public virtual bool DeleteVendorsCanExecute(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> vendors, out List<RecordIdentifier> linkedVendors, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry)
                    .DeleteVendorsCanExecute(entry, siteServiceProfile, vendors, out linkedVendors, closeConnection);
        }

        /// <summary>
        /// Verifies if a vendor is linked to at least one item or is the default vendor for an item.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">Unique ID of the vendor</param>
        /// <returns></returns>
        public virtual VendorItemsLinkedType VendorHasLinkedItems(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).VendorHasLinkedItems(entry, siteServiceProfile, vendorID, closeConnection);
        }

        /// <summary>
        /// Deletes all vendor - item links and removes vendor as default vendor from items.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="vendorID">Unique ID of the vendor</param>
        public virtual void DeleteVendorItemLinks(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier vendorID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteVendorItemLinks(entry, siteServiceProfile, vendorID, closeConnection);
        }

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
        public virtual List<VendorItem> AdvancedSearch(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
                                        VendorItemSearch searchCriteria,
                                        VendorItemSorting sortBy, bool sortBackwards,
                                        int startRecord, int endRecord,
                                        out int totalRecords,
                                        bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).AdvancedSearch(entry, siteServiceProfile, 
                                                                                searchCriteria, 
                                                                                sortBy, sortBackwards, 
                                                                                startRecord, endRecord, 
                                                                                out totalRecords, 
                                                                                closeConnection);
        }
    }
}
