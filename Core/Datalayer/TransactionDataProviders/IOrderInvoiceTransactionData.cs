using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesInvoiceItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesOrderItem;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IOrderInvoiceTransactionData : IDataProviderBase<DataEntity>
    {
        void InsertSalesOrderEntry(IConnectionManager entry, RetailTransaction transaction, SalesOrderLineItem salesOrderLine);
        void InsertSalesInvoiceEntry(IConnectionManager entry, RetailTransaction transaction, SalesInvoiceLineItem salesInvoiceLine);
        LinkedList<SaleLineItem> GetSalesOrderSalesInvoiceItems(IConnectionManager entry, RecordIdentifier transactionId, RetailTransaction transaction, RecordIdentifier storeCurrency);
    }
}