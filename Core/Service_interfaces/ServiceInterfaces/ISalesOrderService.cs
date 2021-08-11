using LSOne.DataLayer.GenericConnector.Interfaces;
using System;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.SalesOrder;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;

namespace SalesOrderInterface
{
    public interface ISalesOrderService: IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="operationInfo">The parameters and information for the Price override operation</param>
        bool PriceOverride(IConnectionManager entry, IPosTransaction posTransaction, OperationInfo operationInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        void SalesOrders(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// Returns true if the Site service is needed to conclude a transaction that includes sales order item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        /// <returns></returns>
        bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction);

        SalesOrderResult ConcludeTransaction(IConnectionManager entry, ISettings settings, IPosTransaction posTransaction);

        /// <summary>
        /// Sends a message to the 3rd party ERP system that a picking list should be printed
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>        
        /// <param name="salesOrderRequest">Any information needed about the sales order</param>        
        /// <returns></returns>
        SalesOrderResult CreatePickingList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrderRequest salesOrderRequest);

        /// <summary>
        /// Sends a message to the 3rd party ERP system that a packing slip should be printed
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>        
        /// <param name="salesOrderRequest">Any information needed about the sales order</param>        
        /// <returns></returns>
        SalesOrderResult CreatePackingSlip(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrderRequest salesOrderRequest);

        /// <summary>
        /// Returns a list of sales orders for a specific customer. 
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>        
        /// <param name="salesOrderRequest">The information needed to get the sales order information</param>
        /// /// <param name="salesOrders">the list of sales orders found in the search</param>
        /// <returns></returns>
        SalesOrderResult GetSalesOrderList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrderRequest salesOrderRequest, List<SalesOrder> salesOrders);
    }
}
