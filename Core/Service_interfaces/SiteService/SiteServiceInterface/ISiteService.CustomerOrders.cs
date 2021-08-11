using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {

        /// <summary>
        /// Saves the customer order except the order XML field is not updated
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="customerOrder">The customer order to be saved</param>
        /// <returns>The ID of the customer order that was saved</returns>
        [OperationContract]
        RecordIdentifier SaveCustomerOrderDetails(LogonInfo logonInfo, CustomerOrder customerOrder);

        /// <summary>
        /// Saves the customer order including the order XML field
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="customerOrder">The customer order to be saved</param>
        /// <returns>The ID of the customer order that was saved</returns>
        [OperationContract]
        RecordIdentifier SaveCustomerOrder(LogonInfo logonInfo, CustomerOrder customerOrder);

        [OperationContract]
        CustomerOrder GetCustomerOrder(LogonInfo logonInfo, RecordIdentifier reference);

        [OperationContract]
        RecordIdentifier CreateReferenceNo(LogonInfo logonInfo, CustomerOrderType orderType);

        [OperationContract]
        List<CustomerOrder> CustomerOrderSearch(LogonInfo logonInfo,
            out int totalRecordsMatching,
            int numberOfRecordsToReturn,
            CustomerOrderSearch searchCriteria
            );

        [OperationContract]
        void GenerateCustomerOrders(LogonInfo logonInfo);
    }
}
