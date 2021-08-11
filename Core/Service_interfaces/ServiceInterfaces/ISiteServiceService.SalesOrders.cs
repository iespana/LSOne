using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.SalesOrder;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService
    {

        /// <summary>
        /// Returns information about a specific sales order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="salesOrder">The information about the sales order being retrieved</param>
        /// <returns></returns>
        SalesOrderResult GetSalesOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrder salesOrder, bool closeConnection);

        /// <summary>
        /// Returns a list of sales orders for a specific customer. 
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="salesOrderRequest">The information needed to get the sales order information. </param>
        /// <param name="salesOrdes">The sales orders found in the search </param>
        /// <returns></returns>
        SalesOrderResult GetSalesOrderList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrderRequest salesOrderRequest, List<SalesOrder> salesOrdes, bool closeConnection);


        /// <summary>
        /// Called when the user is concluding the payment for the sale including the sales order item. 
        /// TransactionID, TerminalID and StoreID together are a unique identifier for a sale in LS One
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="salesOrderID">The ID of the sales order being paid for</param>
        /// <param name="amount">The amount that is being paid onto the sales order</param>        
        /// <param name="transactionID">The transaction ID of the sale</param>
        /// <param name="salesOrderRequest">Any other information that is needed for implementation - not used in default implementation</param>   
        /// <returns></returns>
        SalesOrderResult PaySalesOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier salesOrderID, decimal amount, RecordIdentifier transactionID, SalesOrderRequest salesOrderRequest, bool closeConnection);

        /// <summary>
        /// Sends a message to the 3rd party ERP system that a picking list should be printed
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="salesOrderRequest">Any information needed about the sales order</param>        
        /// <returns></returns>
        SalesOrderResult CreatePickingList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrderRequest salesOrderRequest, bool closeConnection);

        /// <summary>
        /// Sends a message to the 3rd party ERP system that a packing slip should be printed
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="salesOrderRequest">Any information needed about the sales order</param>        
        /// <returns></returns>
        SalesOrderResult CreatePackingSlip(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrderRequest salesOrderRequest, bool closeConnection);
        
    }
}
