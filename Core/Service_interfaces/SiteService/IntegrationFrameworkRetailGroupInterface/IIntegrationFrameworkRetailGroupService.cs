using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSRetail.SiteService.IntegrationFrameworkRetailGroupInterface
{
    //TODO: SessionMode should be required. Changed to Allowed for http binding
    [ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "LSRetail.IntegrationFramework")]
    public partial interface IIntegrationFrameworkRetailGroupService
    {
        [OperationContract]
        bool Ping();

        /// <summary>
        /// Saves a single <see cref="RetailGroup"/> to the database. If it does not exists it will be created.
        /// </summary>
        /// <param name="retailGroup">The retail group to save</param>
        [OperationContract]
        void Save(RetailGroup retailGroup);

        /// <summary>
        /// Saves a list of <see cref="RetailGroup"/> objects to the database. retail groups that do not exist will be created.
        /// </summary>
        /// <param name="retailGroups">The list of retail groups to save</param>
        [OperationContract]
        SaveResult SaveList(List<RetailGroup> retailGroups);

        /// <summary>
        /// Gets a single <see cref="RetailGroup"/> from the databse for the given <paramref name="retailGroupID"/>
        /// </summary>
        /// <param name="retailGroupID">The ID of the retail group to get. </param>
        /// <returns></returns>
        [OperationContract]
        RetailGroup Get(RecordIdentifier retailGroupID);

        /// <summary>
        /// Deletes the retail group with the given <paramref name="retailGroupID"/> from the database
        /// </summary>
        /// <param name="retailGroupID">The ID of the item to delete</param>
        [OperationContract]
        void Delete(RecordIdentifier retailGroupID);
    }
}
