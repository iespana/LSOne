using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public virtual RecordIdentifier SaveCustomerOrderDetails(IConnectionManager entry, SiteServiceProfile siteServiceProfile, CustomerOrder customerOrder)
        {
            RecordIdentifier result = RecordIdentifier.Empty;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveCustomerOrderDetails(CreateLogonInfo(entry), customerOrder), true);

            return result;
        }

        public virtual RecordIdentifier SaveCustomerOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, IRetailTransaction retailTransaction)
        {
            RecordIdentifier result = RecordIdentifier.Empty;
            CustomerOrder order = CreateCustomerOrder(entry, retailTransaction);

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveCustomerOrder(CreateLogonInfo(entry), order), true);

            return result;
        }

        public virtual void SaveCustomerOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, CustomerOrder order)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveCustomerOrder(CreateLogonInfo(entry), order), true);
        }

        private CustomerOrder CreateCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            CustomerOrderItem orderInfo = retailTransaction.CustomerOrder;

            CustomerOrder order = new CustomerOrder();
            order.ID = orderInfo.ID;
            order.Reference = orderInfo.Reference;
            order.Source = new Guid((string)orderInfo.Source.ID);
            order.Delivery = new Guid((string) orderInfo.Delivery.ID);
            order.Comment = orderInfo.Comment;
            order.DeliveryLocation = orderInfo.DeliveryLocation;
            order.ExpiryDate = orderInfo.ExpirationDate;
            order.OrderType = orderInfo.OrderType;
            order.OrderXML = retailTransaction.ToXML().ToString();
            order.StoreID = RecordIdentifier.IsEmptyOrNull(orderInfo.CreatedAtStoreID) ? entry.CurrentStoreID : orderInfo.CreatedAtStoreID;
            order.TerminalID = RecordIdentifier.IsEmptyOrNull(orderInfo.CreatedAtTerminalID) ? entry.CurrentTerminalID : orderInfo.CreatedAtTerminalID;
            order.StaffID = RecordIdentifier.IsEmptyOrNull(orderInfo.CreatedByStaffID) ? entry.CurrentStaffID : orderInfo.CreatedByStaffID;
            order.CustomerID = retailTransaction.Customer.ID;
            order.Status = retailTransaction.CustomerOrder.Status;

            return order;
        }

        public virtual RecordIdentifier CreateReferenceNo(IConnectionManager entry, SiteServiceProfile siteServiceProfile, CustomerOrderType orderType)
        {
            RecordIdentifier result = RecordIdentifier.Empty;
            
            DoRemoteWork(entry, siteServiceProfile, () => result = server.CreateReferenceNo(CreateLogonInfo(entry), orderType), true);

            return result;
        }

        public virtual List<CustomerOrder> CustomerOrderSearch(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            out int totalRecordsMatching,
            int numberOfRecordsToReturn,
            CustomerOrderSearch searchCriteria,
            bool closeConnection = true
            )
        {
            List<CustomerOrder> result = new List<CustomerOrder>();

            try
            {
                if (!isClosed)
                {
                    Disconnect(entry);
                }

                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                result = server.CustomerOrderSearch(CreateLogonInfo(entry), out totalRecordsMatching, numberOfRecordsToReturn, searchCriteria);

                if (closeConnection)
                {
                    Disconnect(entry);
                }

                return result;

            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public virtual void GenerateCustomerOrders(IConnectionManager entry, SiteServiceProfile siteServiceProfile)
        {
            try
            {
                if (!isClosed)
                {
                    Disconnect(entry);
                }

                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                server.GenerateCustomerOrders(CreateLogonInfo(entry));

                Disconnect(entry);
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }
    }
}
