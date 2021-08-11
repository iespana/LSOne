using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="receiptID"></param>
        /// <param name="storeID"></param>
        /// <param name="useCentralReturns"></param>
        /// <param name="closeConnection"></param>
        /// <param name="terminalID"></param>
        /// <returns></returns>
        public virtual List<ReceiptListItem> GetTransactionListForReceiptID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier receiptID, RecordIdentifier terminalID, RecordIdentifier storeID, bool useCentralReturns, bool closeConnection = true)
        {
            List<ReceiptListItem> list = new List<ReceiptListItem>();

            try
            {
                if (!isClosed)
                {
                    Disconnect(entry);
                }

                if (isClosed)
                {
                    if (useCentralReturns)
                    {
                        Connect(entry, siteServiceProfile);
                    }
                }

                list = useCentralReturns ?
                        server.GetTransactionListForReceiptID(CreateLogonInfo(entry), receiptID, storeID, terminalID) :
                        TransactionProviders.PosTransactionData.GetGetTransactionsForReceiptID(entry, receiptID, storeID, terminalID, false);

                if (closeConnection)
                {
                    Disconnect(entry);
                }

                return list;
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public virtual XElement GetTransactionXML(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, bool useCentralReturns, bool closeConnection = true)
        {
            XElement xml = null;
            try
            {
                ISettings settings = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication));

                if (!isClosed)
                {
                    Disconnect(entry);
                }

                if (isClosed)
                {
                    if (useCentralReturns)
                    {
                        Connect(entry, siteServiceProfile);
                    }
                }

                xml = useCentralReturns ?
                        server.GetTransactionXML(CreateLogonInfo(entry), transactionID, storeID, terminalID, settings.Store.Currency, settings.TaxIncludedInPrice) :
                        GetLocalTransactionXML(entry, transactionID, storeID, terminalID, settings.Store.Currency, settings.TaxIncludedInPrice);

                if (closeConnection)
                {
                    Disconnect(entry);
                }

                return xml;
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public virtual void MarkItemsReturned(IConnectionManager entry, SiteServiceProfile siteServiceProfile, IRetailTransaction transaction, bool useCentralReturns, bool closeConnection = true)
        {
            try
            {
                if (transaction.SaleItems.Count(c => c.ReceiptReturnItem) == 0)
                {
                    return;
                }

                if (!isClosed)
                {
                    Disconnect(entry);
                }

                if (isClosed)
                {
                    if (useCentralReturns)
                    {
                        Connect(entry, siteServiceProfile);
                    }
                }

                if (useCentralReturns)
                {
                    // All the information needed to save the items except LineID and Quantity are in the ReceiptLineItem
                    // The sale line item cannot be sent as an object through to the site service which is why this fix is done this way
                    List<ReturnItemInfo> itemList = CreateItemList(transaction.SaleItems);
                    server.MarkItemsAsReturned(CreateLogonInfo(entry), itemList);
                }
                else
                {
                    TransactionProviders.SaleLineItemData.MarkItemsAsReturned(entry, transaction.SaleItems.Where(x => !x.Voided && x.ReceiptReturnItem).ToList());
                }

                if (closeConnection)
                {
                    Disconnect(entry);
                }
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        private List<ReturnItemInfo> CreateItemList(LinkedList<ISaleLineItem> items)
        {
            List<ReturnItemInfo> list = new List<ReturnItemInfo>();
            foreach (var sli in items.Where(w => w.ReceiptReturnItem))
            {
                if (!sli.Voided)
                {
                    var returnInfo = new ReturnItemInfo()
                    {
                        ReturnTransId = sli.ReturnTransId,
                        ReturnLineId = sli.ReturnLineId,
                        ReturnTerminalId = sli.ReturnTerminalId,
                        ReturnStoreId = sli.ReturnStoreId,
                        ReturnedQty = sli.ReturnedQty
                    };

                    list.Add(returnInfo);
                }
            }
            return list;
        }

        private ReceiptListItem CreateReceiptItem(IRetailTransaction transaction)
        {
            ReceiptListItem item = new ReceiptListItem();
            item.ReceiptID = transaction.OrgReceiptId;
            item.StoreID = transaction.OrgStore;
            item.TerminalID = transaction.OrgTerminal;
            item.ID = transaction.OrgTransactionId;
            return item;
        }

        private XElement GetLocalTransactionXML(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier storeCurrency, bool taxIncludedInPrice)
        {
            TypeOfTransaction transType = Providers.TransactionData.GetTransactionType(entry, transactionID, storeID, terminalID);

            if (transType != TypeOfTransaction.Sales)
            {
                return null;
            }

            PosTransaction transaction = new RetailTransaction((string)storeID, (string)storeCurrency, taxIncludedInPrice);

            TransactionProviders.PosTransactionData.GetTransaction(entry, transactionID, storeID, terminalID, transaction, taxIncludedInPrice);

            return transaction.ToXML();
        }
    }
}