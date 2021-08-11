using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IVendorData : IDataProvider<Vendor>, ICompareListGetter<Vendor>, ISequenceable
    {
        /// <summary>
        /// Gets a list of DataEntity that contains Vendor ID and Vendor Description. The list is sorted by Vendor description.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Gets a list of DataEntity that contains CurrencyCode and Currency Description</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of DataEntity that contains Vendor ID and Vendor Description. The list is sorted by the column specified in the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>Gets a list of DataEntity that contains CurrencyCode and Currency Description</returns>
        List<DataEntity> GetList(IConnectionManager entry, VendorSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets all vendors
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>All vendors in the database</returns>
        List<Vendor> GetVendors(IConnectionManager entry, VendorSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Checks if any vendor is using a given tax group id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxgroupID">ID of the tax group</param>
        /// <returns>True if any vendor uses the tax group, else false</returns>
        bool TaxGroupExists(IConnectionManager entry, RecordIdentifier taxgroupID);

        /// <summary>
        /// Returns the sales tax group for the vendor
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <returns></returns>
        RecordIdentifier GetVendorsSalesTaxGroupID(IConnectionManager entry, RecordIdentifier vendorID);

        /// <summary>
        /// Returns information about a specific vendor
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">The unique ID for the vendor</param>
        /// <returns>Information about a specific vendor</returns>
        Vendor Get(IConnectionManager entry, RecordIdentifier vendorID);

        /// <summary>
        /// Saves and returns the vendor information
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendor">The information to be saved</param>
        /// <returns>Vendor</returns>
        Vendor SaveAndReturnVendor(IConnectionManager entry, Vendor vendor);

        /// <summary>
        /// Gets a vendor with a given ID and deleted status
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">The ID of the vendor to get</param>
        /// <param name="deleted">The deleted status of the vendor</param>
        /// <returns>A vendor with a given ID</returns>
        Vendor GetVendor(IConnectionManager entry, RecordIdentifier vendorID, bool? deleted = null);

        /// <summary>
        /// Activates a vendor that has been deleted
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="ID">The unique ID of the vendor to be activated</param>
        void Activate(IConnectionManager entry, RecordIdentifier ID);

        /// <summary>
        /// Takes in a <see cref="VendorSearch"/> object with all the available search parameters. The ones that are empty or null will
        /// be ignored the other search criterias will be used when searching
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="sortBy">How to sort the result list</param>
        /// <param name="sortBackwards">If true then the list is sorted backwards</param>
        /// <returns></returns>
        List<Vendor> AdvancedSearch(IConnectionManager entry, VendorSearch searchCriteria, VendorSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Takes in a <see cref="VendorSearch"/> object with all the available search parameters. The ones that are empty or null will
        /// be ignored the other search criterias will be used when searching
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchCriteria">The search criteria</param>
        /// <returns></returns>
        List<Vendor> AdvancedSearch(IConnectionManager entry, VendorSearch searchCriteria);

        /// <summary>
        /// Sets the default contact on the vendor
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">The unique ID of the vendor to be updated</param>
        /// <param name="contactID">The unique ID of the contact that is to be the default contact on the vendor</param>
        void SetDefaultContact(IConnectionManager entry, RecordIdentifier vendorID, RecordIdentifier contactID);
        /// <summary>
        /// This procedure checks if vendor can be deleted.
        /// A vendor cannot be deleted if a purchase order, a goods receiving document or purchase order worksheet that is open and is attached to the vendor
        /// A vendor can not be deleted if it is attached to a purchase order that is posted and there is no goods receiving document attached to the purchase order i.e.both the purchase order and goods receiving document have to be posted for the vendor to be deleted.
        /// </summary>
        /// <param name="vendors">vendor ids to be checked</param>
        /// <param name="linkedVendors">list of vendors that cannot be deleted </param>
        /// <returns>True if all vendors from the list can be deleted. False otherwise</returns>
        bool DeleteVendorsCanExecute(IConnectionManager entry, List<RecordIdentifier> vendors, out List<RecordIdentifier> linkedVendors);
    }
}