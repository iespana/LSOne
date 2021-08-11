using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlTransactionDataProviders.Properties;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Enums;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesInvoiceItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesOrderItem;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class OrderInvoiceTransactionData : SqlServerDataProviderBase, IOrderInvoiceTransactionData
    {
        public virtual void InsertSalesOrderEntry(IConnectionManager entry, RetailTransaction transaction, SalesOrderLineItem salesOrderLine)
        {
            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONORDERINVOICETRANS", StatementType.Insert, false);

            statement.AddKey("TRANSACTIONID", transaction.TransactionId);
            statement.AddKey("LINENUM", salesOrderLine.LineId, SqlDbType.Decimal);
            statement.AddKey("STOREID", transaction.StoreId);
            statement.AddKey("TERMINALID", transaction.TerminalId);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

            statement.AddField("SALESID", salesOrderLine.SalesOrderId);
            statement.AddField("INVOICEID", "");
            statement.AddField("AMOUNTCUR", salesOrderLine.Amount, SqlDbType.Decimal);
            statement.AddField("SALESORDERINVOICETYPE", (int)SalesOrderInvoice.SalesOrder, SqlDbType.Int);

            statement.AddField("TRANSACTIONSTATUS", salesOrderLine.Voided ? (int)TransactionStatus.Voided : (int)TransactionStatus.Normal, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void InsertSalesInvoiceEntry(IConnectionManager entry, RetailTransaction transaction, SalesInvoiceLineItem salesInvoiceLine)
        {
            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONORDERINVOICETRANS", StatementType.Insert, false);

            statement.AddKey("TRANSACTIONID", transaction.TransactionId);
            statement.AddKey("LINENUM", salesInvoiceLine.LineId, SqlDbType.Decimal);
            statement.AddKey("STOREID", transaction.StoreId);
            statement.AddKey("TERMINALID", transaction.TerminalId);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

            statement.AddField("SALESID", "");
            statement.AddField("INVOICEID", salesInvoiceLine.SalesInvoiceId);
            statement.AddField("AMOUNTCUR", salesInvoiceLine.Amount, SqlDbType.Decimal);
            statement.AddField("SALESORDERINVOICETYPE", (int)SalesOrderInvoice.SalesOrder, SqlDbType.Int);

            statement.AddField("TRANSACTIONSTATUS", salesInvoiceLine.Voided ? (int)TransactionStatus.Voided : (int)TransactionStatus.Normal, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        private static SaleLineItem PopulateSalesOrderLineItem(IConnectionManager entry, IDataReader dr, RetailTransaction transaction, RecordIdentifier storeCurrency)
        {
            if (((SalesOrderInvoice)(int)dr["SALESORDERINVOICETYPE"]) == SalesOrderInvoice.SalesOrder)
            {
                var salesOrder = new SalesOrderLineItem(transaction)
                    {
                        SalesOrderId = (string) dr["SALESID"],
                        Amount = (decimal) dr["AMOUNTCUR"],
                        LineId = (int) (decimal) dr["LINENUM"],
                        Voided = (((TransactionStatus) (int) dr["TRANSACTIONSTATUS"]) == TransactionStatus.Voided),
                        SalesOrderItemType = SalesOrderItemType.FullPayment,
                        Description = Resources.SalesOrderPayment
                    };

                // Set the correct properties and add it to the transaction

                // Necessary property settings for the the sales order "item"...
                salesOrder.Price = salesOrder.Amount;
                salesOrder.PriceWithTax = salesOrder.Amount;
                salesOrder.NetAmount = salesOrder.Amount;
                salesOrder.NetAmountWithTax = salesOrder.Amount;
                salesOrder.StandardRetailPrice = salesOrder.Amount;
                salesOrder.Quantity = 1;
                salesOrder.TaxRatePct = 0;
                salesOrder.Comment = salesOrder.SalesOrderId;
                salesOrder.NoDiscountAllowed = true;
                salesOrder.Found = true;

                /*
                 *  Sales order items cannot have discount lines on them -> no need to check
                 *  Sales order items cannot have tax lines on them -> no need to check
                 *  Sales order items cannot have infocode lines on them -> no need to check
                */

                return salesOrder;
            }
            
            if (((SalesOrderInvoice)(int)dr["SALESORDERINVOICETYPE"]) == SalesOrderInvoice.SalesInvoice)
            {
                var salesInvoice = new SalesInvoiceLineItem(transaction)
                    {
                        SalesInvoiceId = (string) dr["INVOICEID"],
                        Amount = (decimal) dr["AMOUNTCUR"],
                        LineId = (int) (decimal) dr["LINENUM"],
                        Voided = (((TransactionStatus) (int) dr["TRANSACTIONSTATUS"]) == TransactionStatus.Voided),
                        Description = Resources.SalesInvoice
                    };

                // Populate the salesInvoiceLineItem with the respective values...                        

                // Necessary property settings for the the sales invoice "item"...
                salesInvoice.Price = salesInvoice.Amount;
                salesInvoice.PriceWithTax = salesInvoice.Amount;
                salesInvoice.NetAmount = salesInvoice.Amount;
                salesInvoice.NetAmountWithTax = salesInvoice.Amount;
                salesInvoice.StandardRetailPrice = salesInvoice.Amount;
                salesInvoice.Quantity = 1;
                salesInvoice.TaxRatePct = 0;
                salesInvoice.Comment = salesInvoice.SalesInvoiceId;
                salesInvoice.NoDiscountAllowed = true;
                salesInvoice.Found = true;

                /*
                 *  Sales invoice items cannot have discount lines on them -> no need to check
                 *  Sales invoice items cannot have tax lines on them -> no need to check
                 *  Sales invoice items cannot have infocode lines on them -> no need to check
                */

                return salesInvoice;
            }
            
            return null;
        }

        public virtual LinkedList<SaleLineItem> GetSalesOrderSalesInvoiceItems(IConnectionManager entry, RecordIdentifier transactionId, RetailTransaction transaction, RecordIdentifier storeCurrency)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"select INVOICEID,SALESID, SALESORDERINVOICETYPE, AMOUNTCUR, LINENUM, TRANSACTIONSTATUS
                                    from RBOTRANSACTIONORDERINVOICETRANS 
                                    where TRANSACTIONID = @transactionID and STOREID = @storeID and TERMINALID = @terminalID and DATAAREAID = @dataAreaID
                                    order by LINENUM";

                MakeParam(cmd, "transactionID", (string)transactionId);
                MakeParam(cmd, "storeID", transaction.StoreId);
                MakeParam(cmd, "terminalID", transaction.TerminalId);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                var result = new LinkedList<SaleLineItem>();
                Execute(entry, result, cmd, CommandType.Text, transaction, storeCurrency, PopulateSalesOrderLineItem);

                return result;
            }
        }
    }
}
