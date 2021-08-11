using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSRetail.SiteService.IntegrationFrameworkRetailDivisionInterface
{
    //TODO: SessionMode should be required. Changed to Allowed for http binding
    [ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "LSRetail.IntegrationFramework")]
    public partial interface IIntegrationFrameworkRetailDivisionService
    {
        [OperationContract]
        bool Ping();

        /// <summary>
        /// Saves a single <see cref="RetailDivision"/> to the database. If it does not exists it will be created.
        /// </summary>
        /// <param name="retailDivision">The retail division to save</param>
        [OperationContract]
        void Save(RetailDivision retailDivision);

        /// <summary>
        /// Saves a list of <see cref="RetailDivision"/> objects to the database. retail divisions that do not exist will be created.
        /// </summary>
        /// <param name="retailDivisions">The list of retail divisions to save</param>
        [OperationContract]
        SaveResult SaveList(List<RetailDivision> retailDivisions);

        /// <summary>
        /// Gets a single <see cref="RetailDivision"/> from the databse for the given <paramref name="retailDivisionID"/>
        /// </summary>
        /// <param name="retailDivisionID">The ID of the retail division to get. </param>
        /// <returns></returns>
        [OperationContract]
        RetailDivision Get(RecordIdentifier retailDivisionID);

        /// <summary>
        /// Deletes the retail division with the given <paramref name="retailDivisionID"/> from the database
        /// </summary>
        /// <param name="retailDivisionID">The ID of the item to delete</param>
        [OperationContract]
        void Delete(RecordIdentifier retailDivisionID);
    }
}
