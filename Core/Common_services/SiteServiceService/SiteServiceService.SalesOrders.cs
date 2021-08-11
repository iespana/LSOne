using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Invoice;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.SalesOrder;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {   
        public virtual SalesOrderResult GetSalesOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrder salesOrder, bool closeConnection)
        {
            SalesOrderResult result = SalesOrderResult.ErrorHandlingSalesOrder;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetSalesOrder(CreateLogonInfo(entry), out salesOrder), closeConnection);
                        
            return result;
        }

        public virtual SalesOrderResult GetSalesOrderList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrderRequest salesOrderRequest, List<SalesOrder> salesOrders, bool closeConnection)
        {
            SalesOrderResult result = SalesOrderResult.Success;
            List<SalesOrder> orders = new List<SalesOrder>();          

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetSalesOrderList(CreateLogonInfo(entry), salesOrderRequest, out orders), closeConnection);
                        
            salesOrders.AddRange(orders); //In order to retain the memory reference this needs to be done to return the list

            return result;            
        }

        public virtual SalesOrderResult PaySalesOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier salesOrderID, decimal amount, RecordIdentifier transactionID, SalesOrderRequest salesOrderRequest, bool closeConnection)
        {
            SalesOrderResult result = SalesOrderResult.Success;            

            DoRemoteWork(entry, siteServiceProfile, () => result = server.PaySalesOrder(CreateLogonInfo(entry), salesOrderID, amount, entry.CurrentTerminalID, entry.CurrentStoreID, transactionID, salesOrderRequest), closeConnection);
                        
            return result;
        }        

        public virtual SalesOrderResult CreatePickingList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrderRequest salesOrderRequest, bool closeConnection)
        {
            SalesOrderResult result = SalesOrderResult.Success;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.CreatePickingList(CreateLogonInfo(entry), salesOrderRequest), closeConnection);

            return result;            
        }

        public virtual SalesOrderResult CreatePackingSlip(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrderRequest salesOrderRequest, bool closeConnection)
        {
            SalesOrderResult result = SalesOrderResult.Success;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.CreatePackingSlip(CreateLogonInfo(entry), salesOrderRequest), closeConnection);

            return result;            
        }
    }
}
