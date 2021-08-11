using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.SalesOrder;
using LSOne.Services.Interfaces.SupportClasses;
using LSRetail.SiteService.SiteServiceInterface.DTO;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual SalesOrderResult CreatePackingSlip(LogonInfo logonInfo, SalesOrderRequest salesOrderRequest)
        {
            throw new NotImplementedException();
        }

        public virtual SalesOrderResult CreatePickingList(LogonInfo logonInfo, SalesOrderRequest salesOrderRequest)
        {
            throw new NotImplementedException();            
        }               

        public virtual SalesOrderResult GetSalesOrder(LogonInfo logonInfo, out SalesOrder salesOrder)
        {
            throw new NotImplementedException();
        }
        
        public virtual SalesOrderResult GetSalesOrderList(LogonInfo logonInfo, SalesOrderRequest salesOrderRequest, out List<SalesOrder> salesOrders)
        {
            throw new NotImplementedException();
        }        

        public virtual void GetTransaction(ref bool retVal, ref string comment, ref bool uniqueReceiptId, ref DataTable transHeader, ref DataTable transItems, ref DataTable transPayments, string receiptId, string storeId, string terminalId, LogonInfo logonInfo)
        {
            throw new NotImplementedException();
        }

        public virtual void InventoryLookup(ref bool retVal, ref string comment, ref DataTable inventoryTable, string itemId, string variantId, LogonInfo logonInfo)
        {
            throw new NotImplementedException();
        }

        public virtual void PaySalesInvoice(ref bool retVal, ref string comment, string invoiceId, decimal amount, string posID, string storeId, string transactionId, LogonInfo logonInfo)
        {
            throw new NotImplementedException();
        }

        public virtual SalesOrderResult PaySalesOrder(LogonInfo logonInfo, RecordIdentifier salesOrderID, decimal amount, RecordIdentifier terminalID, RecordIdentifier storeID, RecordIdentifier transactionID, SalesOrderRequest salesOrderRequest)
        {
            throw new NotImplementedException();
        }        

        public virtual void ValidateCustomerStatus(ref CustomerStatusValidationEnum retVal, ref string comment, string customerId, string amount, string currencyCode, LogonInfo logonInfo)
        {
            throw new NotImplementedException();
        }
    }
}
