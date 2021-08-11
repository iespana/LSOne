using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSRetail.SiteService.IntegrationFrameworkRetailDepartmentInterface
{
    //TODO: SessionMode should be required. Changed to Allowed for http binding
    [ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "LSRetail.IntegrationFramework")]
    public partial interface IIntegrationFrameworkRetailDepartmentService
    {
        [OperationContract]
        bool Ping();

        /// <summary>
        /// Saves a single <see cref="RetailDepartment"/> to the database. If it does not exists it will be created.
        /// </summary>        
        /// <param name="retailDepartment">The retail department to save</param>
        [OperationContract]
        void Save(RetailDepartment retailDepartment);

        /// <summary>
        /// Saves a list of <see cref="RetailDepartment"/> objects to the database. retail departments that do not exist will be created.
        /// </summary>        
        /// <param name="retailDepartments">The list of retail departments to save</param>
        [OperationContract]
        SaveResult SaveList(List<RetailDepartment> retailDepartments);

        /// <summary>
        /// Gets a single <see cref="RetailDepartment"/> from the databse for the given <paramref name="retailDepartmentID"/>
        /// </summary>        
        /// <param name="retailDepartmentID">The ID of the retail department to get. </param>
        /// <returns></returns>
        [OperationContract]
        RetailDepartment Get(RecordIdentifier retailDepartmentID);

        /// <summary>
        /// Deletes the retail department with the given <paramref name="retailDepartmentID"/> from the database
        /// </summary>        
        /// <param name="retailDepartmentID">The ID of the item to delete</param>
        [OperationContract]
        void Delete(RecordIdentifier retailDepartmentID);
    }
}
