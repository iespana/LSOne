using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSRetail.SiteService.IntegrationFrameworkVendorInterface
{
    //TODO: SessionMode should be required. Changed to Allowed for http binding
    [ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "LSRetail.IntegrationFramework")]
    public partial interface IIntegrationFrameworkVendorService
    {
        [OperationContract]
        bool Ping();

        /// <summary>
        /// Saves a single <see cref="Vendor"/> to the database. If it does not exists it will be created.
        /// </summary>
        /// <param name="vendor">The vendor to save</param>
        [OperationContract]
        void Save(Vendor vendor);

        /// <summary>
        /// Saves a list of <see cref="Vendor"/> objects to the database. Vendors that do not exist will be created.
        /// </summary>
        /// <param name="vendors">The list of vendors to save</param>
        [OperationContract]
        SaveResult SaveList(List<Vendor> vendors);

        /// <summary>
        /// Gets a single <see cref="Vendor"/> from the database for the given <paramref name="vendorID"/>
        /// </summary>
        /// <param name="vendorID">The ID of the vendor to get</param>
        /// <returns>Vendor</returns>
        [OperationContract]
        Vendor Get(RecordIdentifier vendorID);

        /// <summary>
        /// Deletes the vendor with the given <paramref name="vendorID"/> from the database
        /// </summary>
        /// <param name="vendorID">The ID of the item to delete</param>
        [OperationContract]
        bool Delete(RecordIdentifier vendorID);
    }
}
