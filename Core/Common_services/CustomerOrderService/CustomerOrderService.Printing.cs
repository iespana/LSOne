using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services
{
    public partial class CustomerOrderService
    {
        /// <summary>
        /// Prints customer order / quote information receipts for a selected customer order / quote
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="copyReceipt">If true then this is a copy of a previously printed receipt</param>
        public virtual void PrintCustomerOrderInformation(IConnectionManager entry, IRetailTransaction retailTransaction, bool copyReceipt)
        {
            PrintCustomerOrder(entry, 
                               retailTransaction, 
                               retailTransaction.CustomerOrder.OrderType == CustomerOrderType.CustomerOrder ? FormSystemType.CustomerOrderInformation : FormSystemType.QuoteInformation, 
                               copyReceipt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="formType">The form type to be printed</param>
        /// <param name="copyReceipt">If true then this is a copy of a previusly printed receipt</param>
        public virtual void PrintCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, FormSystemType formType, bool copyReceipt)
        {
            if (string.IsNullOrEmpty(retailTransaction.CustomerOrder.DeliveryLocationText))
            {
                Store store = Providers.StoreData.Get(entry, retailTransaction.CustomerOrder.DeliveryLocation, CacheType.CacheTypeTransactionLifeTime);
                if (store != null)
                {
                    retailTransaction.CustomerOrder.DeliveryLocationText = store.Text;
                }
            }
            Interfaces.Services.PrintingService(entry).PrintReceipt(entry, formType, retailTransaction, copyReceipt);
        }
    }
}
