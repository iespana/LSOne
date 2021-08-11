using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;
using LSOne.DataLayer.BusinessObjects.SalesOrder;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        /// <summary>
        /// Sends a message to the 3rd party ERP system that a picking list should be printed
        /// </summary>
        /// <param name="logonInfo">Database and logon information for the site service</param>
        /// <param name="salesOrderRequest">Any information needed about the sales order</param>
        /// <returns></returns>
        [OperationContract]
        SalesOrderResult CreatePickingList(LogonInfo logonInfo, SalesOrderRequest salesOrderRequest);
        /// <summary>
        /// Sends a message to the 3rd party ERP system that a packing slip should be printed
        /// </summary>
        /// <param name="logonInfo">Database and logon information for the site service</param>
        /// <param name="salesOrderRequest">Any information needed about the sales order</param>
        /// <returns></returns>
        [OperationContract]
        SalesOrderResult CreatePackingSlip(LogonInfo logonInfo, SalesOrderRequest salesOrderRequest);

        /// <summary>
        /// Returns information about a specific sales order
        /// </summary>
        /// <param name="logonInfo">Database and logon information for the site service</param>
        /// <param name="salesOrder">The sales order that was being retrieved</param>
        /// <returns></returns>
        [OperationContract]
        SalesOrderResult GetSalesOrder(LogonInfo logonInfo, out SalesOrder salesOrder);

        /// <summary>
        /// Returns a list of sales orders for a specific customer. 
        /// </summary>
        /// <param name="salesOrderRequest">Has information about the request i.e. customer id, sales order id</param>
        /// <param name="logonInfo">Database and logon information for the site service</param>
        /// <param name="salesOrders">The list of sales orders found</param>
        /// <returns></returns>
        [OperationContract]
        SalesOrderResult GetSalesOrderList(LogonInfo logonInfo, SalesOrderRequest salesOrderRequest, out List<SalesOrder> salesOrders);

        /// <summary>
        /// Called when the user is concluding the payment for the sale including the sales order item. 
        /// TransactionID, TerminalID and StoreID together are a unique identifier for a sale in LS One
        /// </summary>
        /// <param name="logonInfo">Database and logon information for the site service</param>
        /// <param name="salesOrderID">The ID of the sales order being paid for</param>
        /// <param name="amount">The amount that is being paid onto the sales order</param>
        /// <param name="terminalID">The ID of the terminal where the payment is being made</param>
        /// <param name="storeID">The ID of the store where the payment is being made</param>
        /// <param name="transactionID">The transaction ID of the sale</param>
        /// <param name="salesOrderRequest">Any other information that is needed for implementation - not used in default implementation</param>
        /// <returns></returns>
        [OperationContract]
        SalesOrderResult PaySalesOrder(LogonInfo logonInfo, RecordIdentifier salesOrderID, decimal amount, RecordIdentifier terminalID, RecordIdentifier storeID, RecordIdentifier transactionID, SalesOrderRequest salesOrderRequest);
    }
}
