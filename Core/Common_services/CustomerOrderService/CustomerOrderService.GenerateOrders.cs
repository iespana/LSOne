using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{

    class ECommerceInfo
    {
        public RecordIdentifier ItemID;
        public decimal Qty;
        public decimal Price; //with tax, including discount
        public RecordIdentifier StoreID;
        public RecordIdentifier Source;
        public RecordIdentifier Delivery;
        public RecordIdentifier Reference;
        public decimal TotalPayment;
        public int Status;
        public RecordIdentifier CustomerID;

        public ECommerceInfo(RecordIdentifier itemID, decimal qty, decimal price, RecordIdentifier storeID, RecordIdentifier source, RecordIdentifier delivery,
            RecordIdentifier reference, decimal totalPayment, RecordIdentifier customerID)
        {
            ItemID = itemID;
            Qty = qty;
            Price = price;
            StoreID = storeID;
            Source = source;
            Delivery = delivery;
            Reference = reference;
            TotalPayment = totalPayment;
            Status = 0;
            CustomerID = customerID;
        }
    }

    public partial class CustomerOrderService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public virtual void GenerateCustomerOrders(IConnectionManager entry)
        {
            entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Generating customer orders", ToString());

            LinkedList<ISaleLineItem> saleLines = new LinkedList<ISaleLineItem>();
            List<ITenderLineItem> tenderLines = new List<ITenderLineItem>();

            List<ECommerceInfo> eCommerce = new List<ECommerceInfo>();
            eCommerce = GenerateECommerceInfoForTest();

            List<CustomerOrderAdditionalConfigurations> configs = Providers.CustomerOrderAdditionalConfigData.GetList(entry);
            //StorePaymentMethod payment = Providers.StorePaymentMethodData.Get(entry, entry.CurrentStoreID, PaymentMethodDefaultFunctionEnum.Normal);

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            foreach (ECommerceInfo info in eCommerce)
            {
                if (info.Status == 0)
                {
                    List<ECommerceInfo> allItems = eCommerce.FindAll(f => f.Reference == info.Reference);
                    CustomerOrderAdditionalConfigurations source = configs.FirstOrDefault(f => f.ID == info.Source);
                    CustomerOrderAdditionalConfigurations delivery = configs.FirstOrDefault(f => f.ID == info.Delivery);

                    CustomerOrderItem order = new CustomerOrderItem();
                    order.CurrentAction = CustomerOrderAction.None;
                    order.OrderType = CustomerOrderType.CustomerOrder;
                    order.Status = CustomerOrderStatus.Open;
                    order.Delivery = delivery;
                    order.Source = source;
                    order.Reference = info.Reference;
                    order.ID = Guid.NewGuid();
                    order.ExpirationDate = new Date(DateTime.Now.AddYears(1));

                    IRetailTransaction transaction = new RetailTransaction((string)info.StoreID, settings.Store.Currency, settings.TaxIncludedInPrice);

                    foreach (ECommerceInfo item in allItems)
                    {
                        ISaleLineItem saleLine = ProcessItem(entry, item, transaction);
                        saleLines.AddLast(saleLine);
                        item.Status = 1;
                    }
                    //tenderLines.Add(CreateTenderLine(entry, settings, payment, false, info.TotalPayment));
                    CreateCustomerOrder(entry, info.CustomerID, saleLines, tenderLines, order);
                    info.Status = 1;
                }
            }
            entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Finished generating customer orders", ToString());
        }

        private ISaleLineItem ProcessItem(IConnectionManager entry, ECommerceInfo info, IRetailTransaction transaction)
        {
            ISaleLineItem saleLineItem = new SaleLineItem(transaction);

            saleLineItem.ItemId = (string)info.ItemID;

            RecordIdentifier saleItemMasterID = Providers.RetailItemData.GetMasterIDFromItemID(entry, saleLineItem.ItemId);

            if (!RecordIdentifier.IsEmptyOrNull(saleItemMasterID))
            {
                saleLineItem.MasterID = (Guid)saleItemMasterID;
            }

            IItemSale sale = Interfaces.Services.ItemService(entry).ProcessItem(entry, saleLineItem, transaction);

            return sale.Item;
        }

        private List<ECommerceInfo> GenerateECommerceInfoForTest()
        {
            List<ECommerceInfo> eList = new List<ECommerceInfo>();

            ECommerceInfo e = new ECommerceInfo("60080", 1, 128, "S0001", new Guid("3b681966-9f22-4f35-9f97-51b4a02416e9"), new Guid("02dda3f1-a0d8-45c1-8fff-6d6ee84f428b"), "1", 356, "00001");
            eList.Add(e);
            e = new ECommerceInfo("20076", 1, 228, "S0001", new Guid("3b681966-9f22-4f35-9f97-51b4a02416e9"), new Guid("02dda3f1-a0d8-45c1-8fff-6d6ee84f428b"), "1", 356, "00002");
            eList.Add(e);
            e = new ECommerceInfo("30090", 1, 328, "S0001", new Guid("3b681966-9f22-4f35-9f97-51b4a02416e9"), new Guid("02dda3f1-a0d8-45c1-8fff-6d6ee84f428b"), "3", 328, "00003");
            eList.Add(e);
            e = new ECommerceInfo("60040", 1, 428, "S0001", new Guid("3b681966-9f22-4f35-9f97-51b4a02416e9"), new Guid("02dda3f1-a0d8-45c1-8fff-6d6ee84f428b"), "4", 428, "00004");
            eList.Add(e);
            e = new ECommerceInfo("10030", 1, 528, "S0001", new Guid("3b681966-9f22-4f35-9f97-51b4a02416e9"), new Guid("02dda3f1-a0d8-45c1-8fff-6d6ee84f428b"), "5", 528, "00005");
            eList.Add(e);
            e = new ECommerceInfo("32070", 1, 628, "S0001", new Guid("3b681966-9f22-4f35-9f97-51b4a02416e9"), new Guid("02dda3f1-a0d8-45c1-8fff-6d6ee84f428b"), "6", 628, "00006");
            eList.Add(e);
            e = new ECommerceInfo("60110", 1, 728, "S0001", new Guid("3b681966-9f22-4f35-9f97-51b4a02416e9"), new Guid("02dda3f1-a0d8-45c1-8fff-6d6ee84f428b"), "7", 728, "00007");
            eList.Add(e);
            e = new ECommerceInfo("60120", 1, 828, "S0001", new Guid("3b681966-9f22-4f35-9f97-51b4a02416e9"), new Guid("02dda3f1-a0d8-45c1-8fff-6d6ee84f428b"), "8", 828, "00008");
            eList.Add(e);
            e = new ECommerceInfo("60100", 1, 928, "S0001", new Guid("3b681966-9f22-4f35-9f97-51b4a02416e9"), new Guid("02dda3f1-a0d8-45c1-8fff-6d6ee84f428b"), "9", 928, "00009");
            eList.Add(e);
            e = new ECommerceInfo("60220", 1, 1028, "S0001", new Guid("3b681966-9f22-4f35-9f97-51b4a02416e9"), new Guid("02dda3f1-a0d8-45c1-8fff-6d6ee84f428b"), "10", 2156, "00010");
            eList.Add(e);
            e = new ECommerceInfo("60210", 1, 1128, "S0001", new Guid("3b681966-9f22-4f35-9f97-51b4a02416e9"), new Guid("02dda3f1-a0d8-45c1-8fff-6d6ee84f428b"), "11", 2156, "00011");
            eList.Add(e);

            return eList;

        }
    }

}
