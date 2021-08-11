using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual CustomerOrder GetCustomerOrder(LogonInfo logonInfo, RecordIdentifier reference)
        {
            var overWritePermissions = new HashSet<string>
                {
                    Permission.ManageCustomerOrders
                };
            var contextGuid = Guid.NewGuid();
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                dataModel.BeginPermissionOverride(contextGuid, overWritePermissions);

                return Providers.CustomerOrderData.Get(dataModel, reference);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return null;
        }

        /// <summary>
        /// Saves the customer order except the order XML field is not updated
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="customerOrder">The customer order to be saved</param>
        /// <returns>The ID of the customer order that was saved</returns>
        public virtual RecordIdentifier SaveCustomerOrderDetails(LogonInfo logonInfo, CustomerOrder customerOrder)
        {
            try
            {
                Utils.Log(this, Utils.Starting);
                return SaveCustomerOrderInformation(logonInfo, customerOrder, true);
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Saves the customer order including the order XML field
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="customerOrder">The customer order to be saved</param>
        /// <returns>The ID of the customer order that was saved</returns>
        public virtual RecordIdentifier SaveCustomerOrder(LogonInfo logonInfo, CustomerOrder customerOrder)
        {
            try
            {
                Utils.Log(this, Utils.Starting);
                return SaveCustomerOrderInformation(logonInfo, customerOrder, false);
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Does the actual customer order saving, excludeOrderXML tells the function if the orderXML property should be saved or not
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="customerOrder">The customer order to be saved</param>
        /// <param name="excludeOrderXML">If true then the orderXML property should not be saved</param>
        /// <returns></returns>
        protected virtual RecordIdentifier SaveCustomerOrderInformation(LogonInfo logonInfo, CustomerOrder customerOrder, bool excludeOrderXML)
        {
            var overWritePermissions = new HashSet<string>
                {
                    Permission.ManageCustomerOrders
                };
            var contextGuid = Guid.NewGuid();

            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting + " reference " + customerOrder.Reference);
                dataModel.BeginPermissionOverride(contextGuid, overWritePermissions);

                Providers.CustomerOrderData.Save(dataModel, customerOrder, excludeOrderXML);

                if (customerOrder.Status == CustomerOrderStatus.Cancelled ||
                    customerOrder.Status == CustomerOrderStatus.Closed ||
                    customerOrder.Status == CustomerOrderStatus.Deleted ||
                    customerOrder.Status == CustomerOrderStatus.Delivered)
                {
                    CloseInventoryAdjustment(logonInfo, new Guid((string)customerOrder.ID));
                    Utils.Log(this, "Inventory adjustment was closed");
                }

                return customerOrder.ID;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return RecordIdentifier.Empty;
        }

        public virtual RecordIdentifier CreateReferenceNo(LogonInfo logonInfo, CustomerOrderType orderType)
        {            
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                CustomerOrderSettings orderSettings = Providers.CustomerOrderSettingsData.Get(dataModel, orderType);

                return Providers.CustomerOrderData.GenerateReference(dataModel, orderSettings);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual List<CustomerOrder> CustomerOrderSearch(LogonInfo logonInfo,
            out int totalRecordsMatching,
            int numberOfRecordsToReturn,
            CustomerOrderSearch searchCriteria
            )
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.CustomerOrderData.AdvancedSearch(dataModel,
                                                                out totalRecordsMatching,
                                                                numberOfRecordsToReturn,
                                                                searchCriteria
                                                                );
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void GenerateCustomerOrders(LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                Services.Interfaces.Services.CustomerOrderService(dataModel).GenerateCustomerOrders(dataModel);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }
    }
}