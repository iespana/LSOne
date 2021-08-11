using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual List<ReceiptListItem> GetTransactionListForReceiptID(LogonInfo logonInfo, RecordIdentifier receiptID, RecordIdentifier storeID, RecordIdentifier terminalID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(receiptID)}: {receiptID}, {nameof(storeID)}: {storeID}, {nameof(terminalID)}: {terminalID}", LogLevel.Trace);
                return TransactionProviders.PosTransactionData.GetGetTransactionsForReceiptID(dataModel, receiptID, storeID, terminalID, false);

            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return new List<ReceiptListItem>();
        }

        public virtual XElement GetTransactionXML(LogonInfo logonInfo, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier storeCurrency, bool taxIncludedInPrice)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting + " transactionID: " + transactionID + ", storeID: " + storeID + ", terminalID: " + terminalID + ", storeCurrency: " + storeCurrency + ", taxIncludedInPrice: " + (taxIncludedInPrice ? "True" : "False"), LogLevel.Trace);

                TypeOfTransaction transType = Providers.TransactionData.GetTransactionType(dataModel, transactionID, storeID, terminalID);

                if (transType != TypeOfTransaction.Sales)
                {
                    Utils.Log(this, "Transaction not a sales transaction", LogLevel.Trace);
                    return null;
                }

                PosTransaction transaction = new RetailTransaction((string)storeID, (string)storeCurrency, taxIncludedInPrice);

                TransactionProviders.PosTransactionData.GetTransaction(dataModel, transactionID, storeID, terminalID, transaction, taxIncludedInPrice);

                return transaction.ToXML();

            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return null;
        }

        public virtual bool MarkItemsAsReturned(LogonInfo logonInfo, List<ReturnItemInfo> returnedItems)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting, LogLevel.Trace);

                List<ISaleLineItem> itemsToReturn = new List<ISaleLineItem>();

                foreach (var item in returnedItems)
                {
                    SaleLineItem sli = new SaleLineItem(null)
                    {
                        ReturnTransId = item.ReturnTransId,
                        ReturnLineId = item.ReturnLineId,
                        ReturnTerminalId = item.ReturnTerminalId,
                        ReturnStoreId = item.ReturnStoreId,
                        ReturnedQty = item.ReturnedQty,
                        ReceiptReturnItem = true
                    };
                    itemsToReturn.Add(sli);
                }

                if (itemsToReturn.Count == 0)
                {
                    Utils.Log(this, "No items to return", LogLevel.Trace);
                    return false;
                }

                TransactionProviders.SaleLineItemData.MarkItemsAsReturned(dataModel, itemsToReturn);
                Utils.Log(this, "Items marked as returned", LogLevel.Trace);

                return true;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return false;
        }
    }
}