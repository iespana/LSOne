using System;
using System.Collections.Generic;
using System.Reflection;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders.Contacts;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        /// <summary>
        /// Returns a list of retail items for the given vendor ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID"></param>
        /// <param name="searchString"></param>
        /// <param name="itemGroupId"></param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="beginsWith"></param>
        /// <returns></returns>
        public virtual List<DataEntity> SearchRetailItemsForVendor(LogonInfo logonInfo, RecordIdentifier vendorID, string searchString,
            RecordIdentifier itemGroupId, int rowFrom, int rowTo, bool beginsWith)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}, {nameof(searchString)}: {searchString}, {nameof(itemGroupId)}: {itemGroupId}, {nameof(rowFrom)}: {rowFrom}, {nameof(rowTo)}: {rowTo}, {nameof(beginsWith)}: {beginsWith}");

                return Providers.VendorItemData.SearchRetailItemsForVendor(entry, vendorID, searchString, itemGroupId, rowFrom, rowTo, beginsWith);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets information about a specific vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID for the vendor being retrieved</param>
        /// <param name="deleted">If true we fetch vendors that have been deleted and if true vendors that are active</param>
        /// <returns>An object with information about the Vendor</returns>
        public virtual Vendor GetVendor(LogonInfo logonInfo, RecordIdentifier vendorID, bool? deleted = null)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}, {nameof(deleted)}: {deleted}");

                return Providers.VendorData.GetVendor(entry, vendorID, deleted);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns the sales tax group for the given vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID for the vendor that is being checked</param>
        /// <returns>The ID of the tax group on the vendor</returns>
        public virtual RecordIdentifier GetVendorsSalesTaxGroupID(LogonInfo logonInfo, RecordIdentifier vendorID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}");

                return Providers.VendorData.GetVendorsSalesTaxGroupID(entry, vendorID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets a list of all vendors
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns>A list of vendors</returns>
        public virtual List<DataEntity> GetVendorList(LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.VendorData.GetList(entry);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes a list of vendors. When deleting, a deleted flag is set to true on the vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="toDeleteList">A list of unique vendor IDs to be deleted</param>
        public virtual void DeleteVendors(LogonInfo logonInfo, List<RecordIdentifier> toDeleteList)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                foreach (RecordIdentifier vendorID in toDeleteList)
                {
                    Providers.VendorData.Delete(entry, vendorID);
                }
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes all vendor items for the given vendor ID.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="logonInfo">Entry into the database</param>
        /// <param name="toDeleteList">The IDs of the retail items</param>
        /// <param name="vendorID">The ID of the vendor</param>
        public virtual void DeleteVendorItemByRetailItemID(LogonInfo logonInfo, List<MasterIDEntity> toDeleteList, RecordIdentifier vendorID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}");

                foreach (MasterIDEntity itemID in toDeleteList)
                {
                    Providers.VendorItemData.DeleteByRetailItemID(entry, itemID.ReadadbleID, vendorID);
                }
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes a list of vendor items
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="toDeleteList">list of contact ID's to delete</param>
        public virtual void DeleteVendorItems(LogonInfo logonInfo, List<RecordIdentifier> toDeleteList)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                foreach (RecordIdentifier itemID in toDeleteList)
                {
                    Providers.VendorItemData.Delete(entry, itemID);
                }
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes the vendor item with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="toDelete"></param>
        public virtual void DeleteVendorItem(LogonInfo logonInfo, RecordIdentifier toDelete)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(toDelete)}: {toDelete}");

                Providers.VendorItemData.Delete(entry, toDelete);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes a list of vendor contacts
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="contactsToDelete">IDs of the contacts to delete</param>
        public virtual void DeleteVendorContact(LogonInfo logonInfo, List<RecordIdentifier> contactsToDelete)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                foreach (RecordIdentifier contactID in contactsToDelete)
                {
                    Providers.ContactData.Delete(entry, contactID, ContactRelationTypeEnum.Vendor);
                }
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns the list of vendor contacts for the given vendor ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">ID of the vendor</param>
        /// <returns></returns>
        public virtual List<Contact> GetVendorContactList(LogonInfo logonInfo, RecordIdentifier vendorID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}");

                List<Contact> result = Providers.ContactData.GetList(entry, ContactRelationTypeEnum.Vendor, vendorID, ContactSorting.Name, false);
                return result;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns the vendor contact with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="contactID">ID of the contact</param>
        /// <returns></returns>
        public virtual Contact GetVendorContact(LogonInfo logonInfo, RecordIdentifier contactID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(contactID)}: {contactID}");

                return Providers.ContactData.Get(entry, contactID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Saves or updates a vendor contact. If the this is the only contact on the vendor it is set as the default contact
        /// </summary>        
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="contact">Contact to save or update</param>
        public virtual SaveContactResultEnum SaveVendorContact(LogonInfo logonInfo, Contact contact)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.ContactData.Save(entry, contact);

                // Check if this is the only contact on the vendor
                List<Contact> contacts = Providers.ContactData.GetList(entry, ContactRelationTypeEnum.Vendor, contact.OwnerID, ContactSorting.Name, false);

                // If it is then set this contact as the default contact
                if (contacts.Count == 1)
                {
                    Providers.VendorData.SetDefaultContact(entry, contact.OwnerID, contact.ID);

                    return SaveContactResultEnum.ContactSavedSetAsDefault;
                }

                return SaveContactResultEnum.ContactSaved;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Undeletes a list of vendors. The deleted flag is set to false on the vendors
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="toActivateList">A list of unique vendor IDs to be activated</param>
        public virtual void RestoreVendors(LogonInfo logonInfo, List<RecordIdentifier> toActivateList)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                foreach (RecordIdentifier vendorID in toActivateList)
                {
                    Providers.VendorData.Activate(entry, vendorID);
                }
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a list of vendors that fulfill the conditions in the search criteria parameter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="searchCriteria">The conditions that should be applied to the search</param>
        /// <returns>A list of items that apply to the search conditions</returns>
        public virtual List<Vendor> GetVendors(LogonInfo logonInfo, VendorSearch searchCriteria)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.VendorData.AdvancedSearch(entry, searchCriteria);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a list of vendors that have not been deleted
        /// </summary>      
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of vendors</returns>
        public virtual List<Vendor> GetVendorsList(LogonInfo logonInfo, VendorSorting sortBy, bool sortBackwards)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(sortBy)}: {sortBy}, {nameof(sortBackwards)}: {sortBackwards}");

                return Providers.VendorData.GetVendors(entry, sortBy, sortBackwards);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Checks if a specific ID already exists for a vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID to check</param>
        /// <returns>True if the vendor ID already exists</returns>
        public virtual bool VendorExists(LogonInfo logonInfo, RecordIdentifier vendorID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}");

                return Providers.VendorData.Exists(entry, vendorID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Saves the vendor information
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendor">The information to be saved</param>
        public virtual void SaveVendor(LogonInfo logonInfo, Vendor vendor)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.VendorData.Save(entry, vendor);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Saves and returns the vendor information
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendor">The information to be saved</param>
        /// <returns>Vendor</returns>
        public virtual Vendor SaveAndReturnVendor(LogonInfo logonInfo, Vendor vendor)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.VendorData.SaveAndReturnVendor(entry, vendor);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets the vendor item by internal ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="internalID"></param>
        /// <returns></returns>
        public virtual VendorItem GetVendorItem(LogonInfo logonInfo, RecordIdentifier internalID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(internalID)}: {internalID}");
                return Providers.VendorItemData.Get(entry, internalID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual VendorItem GetVendorItem(LogonInfo logonInfo, RecordIdentifier vendorID, RecordIdentifier itemID, RecordIdentifier unitID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}, {nameof(itemID)}: {itemID}, {nameof(unitID)}: {unitID}");
                return Providers.VendorItemData.Get(entry, vendorID, itemID, unitID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Each item can be added multiple times to a vendor with different unit ID's. This function returns the first one found 
        /// and it should only be used when the unit ID is not available.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="itemID">The unique ID for the item</param>
        /// <returns>The first vendor item found</returns>
        public virtual VendorItem GetFirstVendorItem(LogonInfo logonInfo, RecordIdentifier vendorID, RecordIdentifier itemID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}, {nameof(itemID)}: {itemID}");

                return Providers.VendorItemData.Get(entry, vendorID, itemID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets a list of DataEntities that contains distinct list of retail item ID's and Item Description for a given vendorID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">ID of the vendor to get the list for</param>
        /// <returns></returns>
        public virtual List<MasterIDEntity> GetDistinctRetailItemsForVendor(LogonInfo logonInfo, RecordIdentifier vendorID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}");

                return Providers.VendorItemData.GetDistinctRetailItemsForVendor(entry, vendorID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID and vendor item ID exists, excluding current record from the check
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="vendorItemID">ID of the vendor item to check for (this is external vendor specific id)</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <returns>True if the vendor item exists, else false</returns>
        public virtual bool VendorItemExistsExcludingCurrentRecord(LogonInfo logonInfo, RecordIdentifier vendorID, RecordIdentifier vendorItemID, RecordIdentifier oldRecordID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}, {nameof(vendorItemID)}: {vendorItemID}, {nameof(oldRecordID)}: {oldRecordID}");

                return Providers.VendorItemData.Exists(entry, vendorID, vendorItemID, oldRecordID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID, retailID and unitID, excluding current record from the check
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="retailID">ID of the retail item</param>
        /// <param name="unitID">ID of the variant</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <returns>True if the vendor item exists, else false</returns>
        public virtual bool VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitID(
            LogonInfo logonInfo,
            RecordIdentifier vendorID,
            RecordIdentifier retailID,
            RecordIdentifier unitID,
            RecordIdentifier oldRecordID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}, {nameof(retailID)}: {retailID}, {nameof(unitID)}: {unitID}, {nameof(oldRecordID)}: {oldRecordID}");

                return Providers.VendorItemData.Exists(entry, vendorID, retailID, unitID, oldRecordID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

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
        public bool VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitIDAndVendorItemID(LogonInfo logonInfo,
            RecordIdentifier vendorID, RecordIdentifier retailID, RecordIdentifier unitID, RecordIdentifier vendorItemID,
            RecordIdentifier oldRecordID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}, {nameof(retailID)}: {retailID}, {nameof(unitID)}: {unitID}, {nameof(vendorItemID)}: {vendorItemID}, {nameof(oldRecordID)}: {oldRecordID}");

                return Providers.VendorItemData.Exists(entry, vendorID, retailID, unitID, vendorItemID, oldRecordID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a list of items that has been assigned to a specific vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="sorting">Sorting of the vendor item list</param>
        /// <param name="sortBackwards">Ascending or descending</param>
        /// <returns></returns>
        public virtual List<VendorItem> GetItemsForVendor(LogonInfo logonInfo, RecordIdentifier vendorID, VendorItemSorting sorting, bool sortBackwards)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}, {nameof(sorting)}: {sorting}, {nameof(sortBackwards)}: {sortBackwards}");

                return Providers.VendorItemData.GetItemsForVendor(entry, vendorID, sorting, sortBackwards);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a paginated list of items that has been assigned to a specific vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="sorting">Sorting of the vendor item list</param>
        /// <param name="sortBackwards">Ascending or descending</param>
        /// <param name="startRecord">Pagination start index</param>
        /// <param name="endRecord">Pagination end index</param>
        /// <param name="totalRecords">Total number of records for the given vendor</param>
        /// <returns></returns>
        public virtual List<VendorItem> GetPagedItemsForVendor(LogonInfo logonInfo,
                                                          RecordIdentifier vendorID,
                                                          VendorItemSorting sorting, bool sortBackwards,
                                                          int startRecord, int endRecord,
                                                          out int totalRecords)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}, {nameof(sorting)}: {sorting}, {nameof(sortBackwards)}: {sortBackwards}, {nameof(startRecord)}: {startRecord}, {nameof(endRecord)}: {endRecord}");

                return Providers.VendorItemData.GetItemsForVendor(entry, vendorID, sorting, sortBackwards, startRecord, endRecord, out totalRecords);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Saves a vendor item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorItem">The vendor item to be saved</param>
        public virtual RecordIdentifier SaveVendorItem(LogonInfo logonInfo, VendorItem vendorItem)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.VendorItemData.Save(entry, vendorItem);
                return vendorItem.ID;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a list of all units that are available for the given item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <param name="itemID">The unique ID for the item</param>
        /// <returns></returns>
        public virtual List<Unit> GetDistinctUnitsForVendorItem(LogonInfo logonInfo, RecordIdentifier vendorID, RecordIdentifier itemID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}, {nameof(itemID)}: {itemID}");

                return Providers.VendorItemData.GetDistinctUnitsForVendorItem(entry, vendorID, itemID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns true if a vendor item exist for the given filter; otherwise returns false
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID"></param>
        /// <param name="retaiID"></param>
        /// <param name="unitID"></param>
        /// <param name="oldRecordID"></param>
        /// <returns></returns>
        public virtual bool VendorItemExists(LogonInfo logonInfo, RecordIdentifier vendorID, RecordIdentifier retaiID, RecordIdentifier unitID, RecordIdentifier oldRecordID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}, {nameof(retaiID)}: {retaiID}, {nameof(unitID)}: {unitID}, {nameof(oldRecordID)}: {oldRecordID}");

                return Providers.VendorItemData.Exists(entry, vendorID, retaiID, unitID, oldRecordID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes a vendor item that matches the given filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="retailItemID"></param>
        /// <param name="vendorID"></param>
        /// <param name="unitID"></param>
        public virtual void DeleteByRetailItemID(LogonInfo logonInfo, RecordIdentifier retailItemID, RecordIdentifier vendorID, RecordIdentifier unitID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}, {nameof(unitID)}: {unitID}");

                Providers.VendorItemData.DeleteByRetailItemID(entry, retailItemID, vendorID, unitID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets a list of vendors for the given retail item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="retailItemID"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortBackwards"></param>
        /// <returns></returns>
        public virtual List<VendorItem> GetVendorsForItem(LogonInfo logonInfo, RecordIdentifier retailItemID, VendorItemSorting sortBy, bool sortBackwards)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(retailItemID)}: {retailItemID}, {nameof(sortBy)}: {sortBy}, {nameof(sortBackwards)}: {sortBackwards}");

                return Providers.VendorItemData.GetVendorsForItem(entry, retailItemID, sortBy, sortBackwards);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Sets the default contact on the vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID of the vendor to be updated</param>
        /// <param name="contactID">The unique ID of the contact that is to be the default contact on the vendor</param>
        public virtual void SetDefaultContactOnVendor(LogonInfo logonInfo, RecordIdentifier vendorID, RecordIdentifier contactID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            DoWork(entry, () => Providers.VendorData.SetDefaultContact(entry, vendorID, contactID), MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Retrieves the default purchase price for a vendor item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID of the vendor to be updated</param>
        /// <param name="itemID">The unique ID of the vendor item being checked</param>
        /// <param name="unitID">The unit ID on the vendor item being checked</param>
        public virtual decimal GetDefaultPurchasePrice(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier vendorID, RecordIdentifier unitID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            decimal result = decimal.Zero;
            DoWork(entry, () => result = Providers.VendorItemData.GetDefaultPurchasePrice(entry, itemID, vendorID, unitID), MethodBase.GetCurrentMethod().Name);
            return result;
        }

        /// <summary>
        /// Retrieves the latest purchase price for a vendor item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The unique ID of the vendor to be updated</param>
        /// <param name="retailItemID">The unique ID of the vendor item being checked</param>
        /// <param name="unitID">The unit ID on the vendor item being checked</param>
        public virtual decimal GetLatestPurchasePrice(LogonInfo logonInfo, RecordIdentifier retailItemID, RecordIdentifier vendorID, RecordIdentifier unitID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(retailItemID)}: {retailItemID}, {nameof(vendorID)}: {vendorID}, {nameof(unitID)}: {unitID}");

                return Providers.VendorItemData.GetLatestPurchasePrice(
                                    entry,
                                    retailItemID,
                                    vendorID,
                                    unitID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns true if the given item has an associated vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="retailItemID"></param>
        /// <param name="vendorID"></param>
        /// <returns></returns>
        public virtual bool VendorHasItem(LogonInfo logonInfo, RecordIdentifier retailItemID, RecordIdentifier vendorID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            bool result = false;
            DoWork(entry, () => result = Providers.VendorItemData.VendorHasItem(entry, retailItemID, vendorID), MethodBase.GetCurrentMethod().Name);
            return result;
        }

        /// <summary>
        /// Deletes a list of vendors. When deleting, a deleted flag is set to true on the vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="toDeleteList">A list of unique vendor IDs to be deleted</param>
        public virtual bool DeleteVendorsCanExecute(LogonInfo logonInfo, List<RecordIdentifier> vendors, out List<RecordIdentifier> linkedVendors)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.VendorData.DeleteVendorsCanExecute(entry, vendors, out linkedVendors);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Verifies if a vendor is linked to at least one item or is the default vendor for an item.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">Unique ID of the vendor</param>
        /// <returns></returns>
        public virtual VendorItemsLinkedType VendorHasLinkedItems(LogonInfo logonInfo, RecordIdentifier vendorID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}");

                bool hasItems = Providers.VendorItemData.VendorHasItems(entry, vendorID);
                bool isDefault = Providers.VendorItemData.VendorIsDefaultVendor(entry, vendorID); 

                if(hasItems && isDefault)
                {
                    return VendorItemsLinkedType.DefaultVendorAndVendorItems;
                }
                else if(hasItems)
                {
                    return VendorItemsLinkedType.VendorItems;
                }
                else if (isDefault)
                {
                    return VendorItemsLinkedType.DefaultVendor;
                }

                return VendorItemsLinkedType.None;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes all vendors - item links and removes vendor as default vendor from items.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">Unique ID of the vendor</param>
        public virtual void DeleteVendorItemLinks(LogonInfo logonInfo, RecordIdentifier vendorID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(vendorID)}: {vendorID}");

                var vendorItems = Providers.VendorItemData.GetItemsForVendor(entry, vendorID, VendorItemSorting.ID, false);
                foreach(var vi in vendorItems)
                {
                    Providers.RetailItemData.SetItemsDefaultVendor(entry, vi.RetailItemID, string.Empty);
                    Providers.VendorItemData.Delete(entry, vi.ID);
                }
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

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
        public virtual List<VendorItem> AdvancedSearch(LogonInfo logonInfo,
                                                        VendorItemSearch searchCriteria,
                                                        VendorItemSorting sortBy, bool sortBackwards,
                                                        int startRecord, int endRecord,
                                                        out int totalRecords)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(sortBy)}: {sortBy}, {nameof(sortBackwards)}: {sortBackwards}, {nameof(startRecord)}: {startRecord}, {nameof(endRecord)}: {endRecord}");

                return Providers.VendorItemData.AdvancedSearch(entry, searchCriteria, sortBy, sortBackwards, startRecord, endRecord, out totalRecords);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }
    }
}