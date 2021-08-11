using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.DataLayer.SqlTransactionDataProviders.Properties;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.CreditMemoItem;
using LSOne.DataLayer.TransactionObjects.Line.CustomerDepositItem;
using LSOne.DataLayer.TransactionObjects.Line.IFSaleItem;
using LSOne.DataLayer.TransactionObjects.Line.IFTenderItem;
using LSOne.DataLayer.TransactionObjects.Line.IncomeExpenseItem;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesInvoiceItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesOrderItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class PosTransactionData : SqlServerDataProviderBase, IPosTransactionData
    {
        private class SalesSequenceProvider : ISequenceable
        {
            public bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
            {
                return false;
            }

            public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
            {
                throw new NotImplementedException();
            }

            public RecordIdentifier SequenceID 
            { get { return "SALESSEQUENCE"; } }
        }

        private static string BaseSQL
        {
            get { return @"SELECT ISNULL(TYPE, 0) AS TYPE,
                             ISNULL(SUBTYPE, 0) AS SUBTYPE, 
                             ISNULL(CURRENCY, '') AS CURRENCY, 
                             TRANSACTIONID, 
                             ISNULL(SALESSEQUENCEID,'') as SALESSEQUENCEID, 
                             ISNULL(RECEIPTID, '') AS RECEIPTID, 
                             STORE, 
                             TERMINAL, 
                             ISNULL(STAFF, '') AS STAFF, 
                             ISNULL(CREATEDONPOSTERMINAL, '') AS CREATEDONPOSTERMINAL, 
                             ISNULL(ENTRYSTATUS, 0) AS ENTRYSTATUS, 
                             ISNULL(TRANSDATE, '1900-01-01') AS TRANSDATE, 
                             ISNULL(SHIFT, '') AS SHIFT, 
                             ISNULL(SHIFTDATE, '1900-01-01') AS SHIFTDATE, 
                             ISNULL(OPENDRAWER, 0) AS OPENDRAWER, 
                             ISNULL(EXCHRATE, 0) AS EXCHRATE, 
                             ISNULL(CUSTACCOUNT, '') AS CUSTACCOUNT, 
                             ISNULL(CUSTPURCHASEORDER, '') AS CUSTPURCHASEORDER, 
                             ISNULL(NETAMOUNT, 0) AS NETAMOUNT, 
                             ISNULL(GROSSAMOUNT, 0) AS GROSSAMOUNT, 
                             ISNULL(ISNETAMOUNTWITHTAXROUNDED, 0) AS ISNETAMOUNTWITHTAXROUNDED,
                             ISNULL(SALESORDERAMOUNT, 0) AS SALESORDERAMOUNT, 
                             ISNULL(SALESINVOICEAMOUNT, 0) AS SALESINVOICEAMOUNT, 
                             ISNULL(INCOMEEXPENSEAMOUNT, 0) AS INCOMEEXPENSEAMOUNT, 
                             ISNULL(ROUNDEDAMOUNT, 0) AS ROUNDEDAMOUNT, 
                             ISNULL(SALESPAYMENTDIFFERENCE, 0) AS SALESPAYMENTDIFFERENCE, 
                             ISNULL(PAYMENTAMOUNT, 0) AS PAYMENTAMOUNT, 
                             ISNULL(MARKUPAMOUNT, 0) AS MARKUPAMOUNT, 
                             ISNULL(MARKUPDESCRIPTION, '') AS MARKUPDESCRIPTION, 
                             ISNULL(AMOUNTTOACCOUNT, 0) AS AMOUNTTOACCOUNT, 
                             ISNULL(TOTALDISCAMOUNT, 0) AS TOTALDISCAMOUNT, 
                             ISNULL(NUMBEROFITEMS, 0) AS NUMBEROFITEMS, 
                             ISNULL(COMMENT, '') AS COMMENT, 
                             ISNULL(INVOICECOMMENT, '') AS INVOICECOMMENT, 
                             ISNULL(RECEIPTEMAIL, '') AS RECEIPTEMAIL, 
                             ISNULL(OILTAX, 0) AS OILTAX, 
                             TAXINCLINPRICE, 
                             ISNULL(TAXEXEMPT, 0) AS TAXEXEMPT, 
                             ISNULL(STATEMENTCODE, '') AS STATEMENTCODE,
                             ISNULL(CREATEDDATE, '1900-01-01') as CREATEDDATE,
                             ISNULL(BUSINESSDAY, '1900-01-01') AS BUSINESSDAY,
                             ISNULL(BUSINESSSYSTEMDAY, '1900-01-01') AS BUSINESSSYSTEMDAY,
                             ISNULL(ORIGNUMOFTRANSACTIONLINES, 0) AS ORIGNUMOFTRANSACTIONLINES,
                             ISNULL(CUSTOMERORDERID, '00000000-0000-0000-0000-000000000000') AS CUSTOMERORDERID
                             FROM RBOTRANSACTIONTABLE ";
            }
        }


        private static PosTransaction PopulateTransaction(IConnectionManager entry, IDataReader dr, PosTransaction transaction)
        {
            transaction.CreatedOnTerminalId = (string)dr["CREATEDONPOSTERMINAL"];
            transaction.EntryStatus = (TransactionStatus) ((int) dr["ENTRYSTATUS"]);
            transaction.BeginDateTime = (DateTime)dr["TRANSDATE"];
            transaction.EndDateTime = (DateTime)dr["CREATEDDATE"];         
            transaction.ShiftId = (string) dr["SHIFT"];
            transaction.ShiftDate = (DateTime)dr["SHIFTDATE"];
            transaction.OpenDrawer = ((byte)dr["OPENDRAWER"] != 0);
            transaction.StoreExchangeRate = (decimal)dr["EXCHRATE"];
            transaction.StoreCurrencyCode = (string)dr["CURRENCY"];
            transaction.TransactionId = (string)dr["TRANSACTIONID"];
            transaction.SalesSequenceID = AsString(dr["SALESSEQUENCEID"]);
            transaction.ReceiptId = (string)dr["RECEIPTID"];
            transaction.StoreId = (string)dr["STORE"];
            transaction.TerminalId = (string)dr["TERMINAL"];
            transaction.Cashier.ID = (string)dr["STAFF"];
            transaction.BusinessDay = (DateTime) dr["BUSINESSDAY"];
            transaction.BusinessSystemDay = (DateTime) dr["BUSINESSSYSTEMDAY"];
            transaction.Training = transaction.EntryStatus == TransactionStatus.Training;
            transaction.TransactionSubType = (int)dr["SUBTYPE"];
            transaction.OriginalNumberOfTransactionLines = (int)dr["ORIGNUMOFTRANSACTIONLINES"];
            transaction.Comment = (string)dr["COMMENT"];

            if (transaction is RetailTransaction)
            {
                PopulateRetailTransaction(dr, (RetailTransaction)transaction);
            }
            else if(transaction is CustomerPaymentTransaction)
            {
                PopulateCustomerPaymentTransaction(dr, (CustomerPaymentTransaction)transaction);
            }
            return transaction;
        }

        private static void PopulateCustomerPaymentTransaction(IDataReader dr, CustomerPaymentTransaction transaction)
        {
            if ((TypeOfTransaction)dr["TYPE"] != TypeOfTransaction.Payment)
            {
                transaction = null;
            }

            //If there is a customer on the transaction - get information about the customer
            var cust = new Customer {ID = (string) dr["CUSTACCOUNT"]};
            transaction.Add(cust);
            transaction.StoreExchangeRate = (decimal)dr["EXCHRATE"];
            transaction.Amount = (decimal)dr["GROSSAMOUNT"] * -1;
            transaction.Payment = (decimal)dr["PAYMENTAMOUNT"] * -1;
            transaction.RoundingSalePmtDiff = (decimal) dr["SALESPAYMENTDIFFERENCE"];
        }

        private static void PopulateRetailTransaction(IDataReader dr, RetailTransaction transaction)
        {
            if ((TypeOfTransaction)dr["TYPE"] != TypeOfTransaction.Sales && (TypeOfTransaction)dr["TYPE"] != TypeOfTransaction.Payment && (TypeOfTransaction)dr["TYPE"] != TypeOfTransaction.Deposit)
            {
                return;
            }

            transaction.CustomerPurchRequestId = (string) dr["CUSTPURCHASEORDER"];
            transaction.NetAmount = (decimal) dr["NETAMOUNT"]*-1;
            transaction.NetAmountWithTax = (decimal) dr["GROSSAMOUNT"]*-1;
            transaction.IsNetAmountWithTaxRounded = (bool)dr["ISNETAMOUNTWITHTAXROUNDED"];
            transaction.SalesOrderAmounts = (decimal) dr["SALESORDERAMOUNT"];
            transaction.SalesInvoiceAmounts = (decimal) dr["SALESINVOICEAMOUNT"];
            transaction.IncomeExpenseAmounts = (decimal) dr["INCOMEEXPENSEAMOUNT"];
            transaction.RoundingDifference = (decimal) dr["ROUNDEDAMOUNT"]*-1;
            transaction.RoundingSalePmtDiff = (decimal) dr["SALESPAYMENTDIFFERENCE"];
            transaction.CustomerOrder.ID = (Guid)dr["CUSTOMERORDERID"];
            if (transaction.CustomerOrder.ID != Guid.Empty)
            {
                transaction.CustomerOrder.OrderType = CustomerOrderType.CustomerOrder;
            }

            transaction.Payment = (decimal) dr["PAYMENTAMOUNT"];           
            if (transaction.Payment != 0M)
            {
                if (transaction.Payment < 0M)
                {
                    transaction.Payment = ((transaction.NetAmountWithTax*-1) + transaction.RoundingSalePmtDiff)*-1;
                }
                else
                {
                    transaction.Payment = transaction.NetAmountWithTax - transaction.RoundingSalePmtDiff;
                }
                if (transaction.Payment > (decimal) dr["PAYMENTAMOUNT"])
                {
                    //In older versions (pre 7.0) MarkupItem.Amount is saved into PaymentAmount with trans.Payment
                    //with trans.Payment: transAction.Payment - transAction.MarkupItem.Amount);
                    //trans.Payment has already been determined previously in the code
                    transaction.MarkupItem.Amount = transaction.Payment - (decimal) dr["PAYMENTAMOUNT"];
                    transaction.MarkupItem.Description = "";
                }
            }

            var customer = new Customer {ID = (string) dr["CUSTACCOUNT"]};
            transaction.Add(customer);
            transaction.MarkupItem.Amount = (decimal) dr["MARKUPAMOUNT"];
            transaction.MarkupItem.Description = (string) dr["MARKUPDESCRIPTION"];
            if (transaction.Customer.ID != "")
            {
                //When saved AmountToAccount is calculated: transAction.Payment - transAction.SalesInvoiceAmounts - transAction.SalesOrderAmounts
                transaction.Payment = (decimal) dr["AMOUNTTOACCOUNT"];
                transaction.Payment += transaction.SalesOrderAmounts + transaction.SalesInvoiceAmounts;
            }
            //transaction.TotalDiscount = (decimal) dr["TOTALDISCAMOUNT"];
            transaction.NoOfItems = (decimal) dr["NUMBEROFITEMS"];
            transaction.InvoiceComment = (string) dr["INVOICECOMMENT"];
            transaction.ReceiptEmailAddress = (string) dr["RECEIPTEMAIL"];
            transaction.Oiltax = (decimal) dr["OILTAX"];
            if (dr["TAXINCLINPRICE"] != DBNull.Value)
            {
                transaction.TaxIncludedInPrice = (byte) dr["TAXINCLINPRICE"] != 0;
            }
            transaction.TaxExempt = (byte) dr["TAXEXEMPT"] != 0;
        }

        public virtual List<PosTransaction> GetTransactionsForIntegrationService(IConnectionManager entry,
                                                                                List<TypeOfTransaction> types,
                                                                                List<TransactionStatus> statuses,
                                                                                List<RecordIdentifier> storeIDs,
                                                                                List<RecordIdentifier> terminalIDs,                                                                                
                                                                                DateTime? fromDate, 
                                                                                DateTime? toDate,
                                                                                bool includePreviouslyExported,
                                                                                List<RecordIdentifier> transactionIDs = null)
        {
            List<PosTransaction> transactions;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL + @" WHERE ";
                int constraintCount = 0;
                string whereConstraint = "";
                if (types != null && types.Count > 0)
                {
                    string condition = GetConstraint(constraintCount++, " TYPE IN (");
                    for (int i = 0; i < types.Count; i++)
                    {
                        condition += "@type" + i + ",";
                        MakeParam(cmd, "type" + i, (int) types[i], SqlDbType.Int);
                    }
                    condition = condition.TrimEnd(',');
                    condition += ") ";
                    whereConstraint += condition;
                }

                if (statuses != null && statuses.Count > 0)
                {
                    string condition = GetConstraint(constraintCount++, " ENTRYSTATUS IN (");
                    for (int i = 0; i < statuses.Count; i++)
                    {
                        condition += "@status" + i + ",";
                        MakeParam(cmd, "status" + i, (int)statuses[i], SqlDbType.Int);
                    }
                    condition = condition.TrimEnd(',');
                    condition += ") ";
                    whereConstraint += condition;
                }
                
                if (storeIDs != null && storeIDs.Count > 0)
                {
                    string condition = GetConstraint(constraintCount++, " STORE IN (");
                    for (int i = 0; i < storeIDs.Count; i++)
                    {
                        condition += "@storeID" + i + ",";
                        MakeParam(cmd, "storeID" + i, (string)storeIDs[i]);
                    }
                    condition = condition.TrimEnd(',');
                    condition += ") ";
                    whereConstraint += condition;
                }

                if (terminalIDs != null && terminalIDs.Count > 0)
                {
                    string condition = GetConstraint(constraintCount++, " TERMINAL IN (");
                    for (int i = 0; i < terminalIDs.Count; i++)
                    {
                        condition += "@terminalID" + i + ",";
                        MakeParam(cmd, "terminalID" + i, (string)terminalIDs[i]);
                    }
                    condition = condition.TrimEnd(',');
                    condition += ") ";
                    whereConstraint += condition;
                }

                if (transactionIDs != null && transactionIDs.Count > 0)
                {
                    string condition = GetConstraint(constraintCount++, " TRANSACTIONID IN (");
                    for (int i = 0; i < transactionIDs.Count; i++)
                    {
                        condition += "@transactionID" + i + ",";
                        MakeParam(cmd, "transactionID" + i, (string)transactionIDs[i]);
                    }
                    condition = condition.TrimEnd(',');
                    condition += ") ";
                    whereConstraint += condition;
                }

                if (fromDate != null && fromDate > new DateTime(1900, 1, 1))
                {
                    whereConstraint += " AND TRANSDATE > @fromDate";
                    MakeParam(cmd, "fromDate", fromDate.Value, SqlDbType.DateTime);
                }
                
                if (toDate != null && toDate > new DateTime(1900, 1, 1))
                {
                    whereConstraint += GetConstraint(constraintCount++, " TRANSDATE < @toDate");
                    MakeParam(cmd, "toDate", toDate.Value, SqlDbType.DateTime);
                }

                whereConstraint += GetConstraint(constraintCount++, " DATAAREAID = @dataAreaID"); 
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                if (!includePreviouslyExported)
                {
                    whereConstraint += GetConstraint(constraintCount++, " WS_EXPORTED IS NULL"); 
                }

                cmd.CommandText += whereConstraint;

                transactions = Execute<PosTransaction, object>(entry, cmd, CommandType.Text, null, PopulateIntegrationTransaction);
            }

            if (transactions == null)
            {
                return null;
            }

            foreach (PosTransaction posTransaction in transactions)
            {
                if (posTransaction == null)
                {
                    continue;
                }
                POSUser user = Providers.POSUserData.GetPerStore(entry, posTransaction.Cashier.ID, posTransaction.StoreId, false);
                if (user != null)
                {
                    posTransaction.Cashier.Name = entry.Settings.NameFormatter.Format(user.Name);
                    posTransaction.Cashier.NameOnReceipt = user.NameOnReceipt;
                }
                if (posTransaction is RetailTransaction)
                {
                    GetRetailTransaction(entry, (RetailTransaction)posTransaction);

                }
                else if (posTransaction is CustomerPaymentTransaction)
                {
                    GetCustomerPaymentTransaction(entry, (CustomerPaymentTransaction)posTransaction);
                }
            }
            return transactions;
        }

        private static string GetConstraint(int constraintCount, string constraint)
        {
            return ((constraintCount > 0) ? " AND" : "") + constraint;
        }

        //currently only support retail and customer deposit transactions.
        private static PosTransaction PopulateIntegrationTransaction(IConnectionManager entry, IDataReader dr, object notUsed)
        {
            PosTransaction transaction = null;
            string storeCurrencyCode = (string)dr["CURRENCY"]; 

            if ((TypeOfTransaction) dr["TYPE"] == TypeOfTransaction.Sales)
            {
                string store = (string)dr["STORE"];
                bool taxIncludedInPrice = (byte) dr["TAXINCLINPRICE"] == 1;
                transaction = new RetailTransaction(store,  storeCurrencyCode, taxIncludedInPrice);
                PopulateTransaction(entry, dr, transaction);
            }
            else if((TypeOfTransaction)dr["TYPE"] == TypeOfTransaction.Payment)
            {
                transaction = new CustomerPaymentTransaction(storeCurrencyCode);
            }
            return transaction;
        }


        public virtual void GetTransaction(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, PosTransaction transaction, bool taxIncludedInPrice)
        {
            if(transaction is RetailTransaction)
            {
                ((RetailTransaction) transaction).TaxIncludedInPrice = taxIncludedInPrice;
            }          
            var mysettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            if (entry.CurrentTerminalID == "" || mysettings == null)
            {
                transaction.Training = false; // We are probably in the Site Manager
                var store = Providers.StoreData.Get(entry, storeID, CacheType.CacheTypeApplicationLifeTime, UsageIntentEnum.Minimal);
                transaction.CalculateDiscountFrom = store.CalculateDiscountsFrom;
            }
            else
            {
                transaction.Training = mysettings.TrainingMode;
                transaction.StatementMethod = StatementGroupingMethod.POSTerminal; // Currently we only support Terminal grouping
                transaction.TransactionIdNumberSequence = mysettings.Terminal.TransactionIDNumberSequence;
                transaction.StoreTaxGroup = mysettings.Store.TaxGroup;
                transaction.KeyedInPriceContainsTax = mysettings.Store.KeyedInPriceIncludesTax;
                transaction.CalculateDiscountFrom = mysettings.Store.CalculateDiscountsFrom;
                transaction.DisplayAmountsIncludingTax = mysettings.Store.DisplayAmountsWithTax;
                transaction.CalcCustomerDiscounts = mysettings.DiscountCalculation.CalculateCustomerDiscounts;
                transaction.CalcPeriodicDiscounts = mysettings.DiscountCalculation.CalculatePeriodicDiscounts;
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL + " WHERE DATAAREAID = @dataAreaID AND STORE = @storeID AND TERMINAL = @terminalID AND TRANSACTIONID = @transactionID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", storeID);
                MakeParam(cmd, "terminalID", terminalID);
                MakeParam(cmd, "transactionID", transactionID);
                Execute(entry, cmd, CommandType.Text, transaction, PopulateTransaction);
            }

            var user = Providers.POSUserData.GetPerStore(entry, transaction.Cashier.ID, transaction.StoreId, false);
            if (user != null)
            {
                transaction.Cashier.Name = entry.Settings.NameFormatter.Format(user.Name);
                transaction.Cashier.NameOnReceipt = user.NameOnReceipt;
            }

            if (transaction is RetailTransaction)

            {
                GetRetailTransaction(entry, (RetailTransaction) transaction);
            }
            else if (transaction is CustomerPaymentTransaction)
            {
                GetCustomerPaymentTransaction(entry,(CustomerPaymentTransaction) transaction);
            }

            transaction.Receipts = new List<IReceiptInfo>(TransactionProviders.ReceiptTransactionData.GetReceiptInfo(entry, transaction.TransactionId, transaction));
        }

        private static IFRetailTransaction PopulateTransactionBuilder(IConnectionManager entry, IDataReader dr, object notUsed)
        {
            IFRetailTransaction transaction = null;
            string storeCurrencyCode = (string)dr["CURRENCY"];

            transaction = new IFRetailTransaction();
            PopulateIFRetailTransactionBuilder(entry, dr, transaction);

            return transaction;
        }
        private static void PopulateIFRetailTransactionBuilder(IConnectionManager entry, IDataReader dr, IFRetailTransaction transaction)
        {
            transaction.EntryStatus = (TransactionStatus)((int)dr["ENTRYSTATUS"]);
            transaction.TypeOfTransaction = (TypeOfTransaction)((int)dr["TYPE"]);
            transaction.TransactionSubType = (int)dr["SUBTYPE"];
            transaction.StoreCurrencyCode = (string)dr["CURRENCY"];
            transaction.NetAmount = (decimal)dr["NETAMOUNT"] * -1;
            transaction.NetAmountWithTax = (decimal)dr["GROSSAMOUNT"] * -1;
            transaction.SalesOrderAmounts = (decimal)dr["SALESORDERAMOUNT"];
            transaction.SalesInvoiceAmounts = (decimal)dr["SALESINVOICEAMOUNT"];
            transaction.IncomeExpenseAmounts = (decimal)dr["INCOMEEXPENSEAMOUNT"];
            transaction.RoundingDifference = (decimal)dr["ROUNDEDAMOUNT"] * -1;
            transaction.RoundingSalePmtDiff = (decimal)dr["SALESPAYMENTDIFFERENCE"];
            transaction.Payment = (decimal)dr["PAYMENTAMOUNT"];
            transaction.MarkupItem.Amount = (decimal)dr["MARKUPAMOUNT"];
            transaction.TotalDiscount = (decimal)dr["TOTALDISCAMOUNT"];
            transaction.NoOfItems = (decimal)dr["NUMBEROFITEMS"];
            transaction.Oiltax = (decimal)dr["OILTAX"];
            transaction.BeginDateTime = (DateTime)dr["BEGINTIME"];
            transaction.EndDateTime = (DateTime)dr["ENDTIME"];
        }

        private static IFRetailTransaction PopulateIFTransaction(IConnectionManager entry, IDataReader dr, object notUsed)
        {
            IFRetailTransaction transaction = null;
            string storeCurrencyCode = (string)dr["CURRENCY"];

            transaction = new IFRetailTransaction();
            PopulateIFRetailTransaction(entry, dr, transaction);

            return transaction;
        }
        private static void PopulateIFRetailTransaction(IConnectionManager entry, IDataReader dr, IFRetailTransaction transaction)
        {
            transaction.TransactionID = (string)dr["TRANSACTIONID"];
            transaction.EntryStatus = (TransactionStatus)((int)dr["ENTRYSTATUS"]);
            transaction.TypeOfTransaction = (TypeOfTransaction)((int)dr["TYPE"]);
            transaction.StoreCurrencyCode = (string)dr["CURRENCY"];
            transaction.NetAmount = (decimal)dr["NETAMOUNT"] * -1;
            transaction.NetAmountWithTax = (decimal)dr["GROSSAMOUNT"] * -1;
            transaction.SalesOrderAmounts = (decimal)dr["SALESORDERAMOUNT"];
            transaction.SalesInvoiceAmounts = (decimal)dr["SALESINVOICEAMOUNT"];
            transaction.IncomeExpenseAmounts = (decimal)dr["INCOMEEXPENSEAMOUNT"];
            transaction.RoundingDifference = (decimal)dr["ROUNDEDAMOUNT"] * -1;
            transaction.RoundingSalePmtDiff = (decimal)dr["SALESPAYMENTDIFFERENCE"];
            transaction.Payment = (decimal)dr["PAYMENTAMOUNT"];
            transaction.MarkupItem.Amount = (decimal)dr["MARKUPAMOUNT"];
            transaction.TotalDiscount = (decimal)dr["TOTALDISCAMOUNT"];
            transaction.NoOfItems = (decimal)dr["NUMBEROFITEMS"];
            transaction.Oiltax = (decimal)dr["OILTAX"];
            transaction.BeginDateTime = (DateTime)dr["BEGINTIME"];
            transaction.EndDateTime = (DateTime)dr["ENDTIME"];
        }

        private static IFSaleLineItem PopulateTransactionSaleLineItemsBuilder(IConnectionManager entry, IDataReader dr, object notUsed)
        {
            IFSaleLineItem saleLineItems = null;
            string storeCurrencyCode = (string)dr["CURRENCY"];

            saleLineItems = new IFSaleLineItem();
            PopulateIFRetailTransactionSaleLineItemsBuilder(entry, dr, saleLineItems);

            return saleLineItems;
        }
        private static void PopulateIFRetailTransactionSaleLineItemsBuilder(IConnectionManager entry, IDataReader dr, IFSaleLineItem saleLineItems)
        {
            saleLineItems.ItemID = (string)dr["ITEMID"];
            saleLineItems.DepartmentID = (string)dr["ITEMDEPARTMENTID"];
            saleLineItems.ItemGroupID = (string)dr["ITEMGROUPID"];
            saleLineItems.TaxGroupID = (string)dr["TAXGROUP"];
            saleLineItems.Unit = (string)dr["UNIT"];
            saleLineItems.Currency = (string)dr["CURRENCY"];
            saleLineItems.Price = (decimal)dr["PRICE"];
            saleLineItems.NetPrice = (decimal)dr["NETPRICE"];
            saleLineItems.Quantity = (decimal)dr["QTY"];
            saleLineItems.DiscountAmount = (decimal)dr["DISCAMOUNT"];
            saleLineItems.CostAmount = (decimal)dr["COSTAMOUNT"];
            saleLineItems.NetAmount = (decimal)dr["NETAMOUNT"];
            saleLineItems.DiscountAmountFromPrice = (decimal)dr["DISCAMOUNTFROMSTDPRICE"];
            saleLineItems.TotalRoundedAmount = (decimal)dr["TOTALROUNDEDAMOUNT"];
            saleLineItems.LineDiscountAmount = (decimal)dr["LINEDSCAMOUNT"];
            saleLineItems.CustomerDiscountAmount = (decimal)dr["CUSTDISCAMOUNT"];
            saleLineItems.InfocodeDiscountAmount = (decimal)dr["INFOCODEDISCAMOUNT"];
            saleLineItems.CustomerInvoiceDiscountAmount = (decimal)dr["CUSTINVOICEDISCAMOUNT"];
            saleLineItems.UnitQuantity = (decimal)dr["UNITQTY"];
            saleLineItems.TaxAmount = (decimal)dr["TAXAMOUNT"];
            saleLineItems.TotalDiscountAmount = (decimal)dr["TOTALDISCAMOUNT"];
            saleLineItems.PeriodiclDiscountAmount = (decimal)dr["PERIODICDISCAMOUNT"];
            saleLineItems.WoleDiscountAmountWithTax = (decimal)dr["WHOLEDISCAMOUNTWITHTAX"];
            saleLineItems.TotalDiscountAmountWithTax = (decimal)dr["TOTALDISCAMOUNTWITHTAX"];
            saleLineItems.LineDiscountAmountWithTax = (decimal)dr["LINEDISCAMOUNTWITHTAX"];
            saleLineItems.PeriodicDiscountAmountWithTax = (decimal)dr["PERIODICDISCAMOUNTWITHTAX"];
            saleLineItems.ReturnQuantity = (decimal)dr["RETURNQTY"];
            saleLineItems.Oiltax = (decimal)dr["OILTAX"];
            saleLineItems.PriceUnit = (decimal)dr["PRICEUNIT"];
            saleLineItems.NetAmountIncludingTax = (decimal)dr["NETAMOUNTINCLTAX"];
        }

        private static IFTenderLineItem PopulateTransactionTenderLineItemsBuilder(IConnectionManager entry, IDataReader dr, object notUsed)
        {
            IFTenderLineItem tenderLineItems = null;
            string storeCurrencyCode = (string)dr["CURRENCY"];

            tenderLineItems = new IFTenderLineItem();
            PopulateIFRetailTransactionTenderLineItemsBuilder(entry, dr, tenderLineItems);

            return tenderLineItems;
        }
        private static void PopulateIFRetailTransactionTenderLineItemsBuilder(IConnectionManager entry, IDataReader dr, IFTenderLineItem tenderLineItems)
        {
            tenderLineItems.TenderType = (string)dr["TENDERTYPE"];
            tenderLineItems.Currency = (string)dr["CURRENCY"];
            tenderLineItems.AmountTendered = (decimal)dr["AMOUNTTENDERED"];
            tenderLineItems.AmountCurrency = (decimal)dr["AMOUNTCUR"];
            tenderLineItems.Quantity = (decimal)dr["QTY"];
            tenderLineItems.LoyaltyPoints = (decimal)dr["LOYALTYPOINTS"];
        }

        public virtual IFRetailTransaction GetSaleTransactionSumForDatePeriod(IConnectionManager entry, bool forDefaultCustomers, Date startDateTime = null, Date endDateTime = null, string storeID = null)
        {

            IFRetailTransaction transaction = new IFRetailTransaction();
            transaction = TransactionBuilderTransactionHeader(entry, forDefaultCustomers, startDateTime, endDateTime, storeID);
            if (transaction != null)
            {
                transaction = TransactionBuilderSaleLines(entry, transaction, forDefaultCustomers, startDateTime, endDateTime, storeID);
                transaction = TransactionBuilderTenderLines(entry, transaction, forDefaultCustomers, startDateTime, endDateTime, storeID);
            }

            return transaction;
        }

        private static IFRetailTransaction TransactionBuilderTransactionHeader(IConnectionManager entry, bool forDefaultCustomers, Date startDateTime = null, Date endDateTime = null, string storeID = null)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> selectionColumns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "CURRENCY", TableAlias = "T", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "ENTRYSTATUS", TableAlias = "T"},
                    new TableColumn {ColumnName = "TYPE", TableAlias = "T"},
                    new TableColumn {ColumnName = "SUBTYPE", TableAlias = "T"},
                    new TableColumn {ColumnName = "NETAMOUNT", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "GROSSAMOUNT", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "SALESORDERAMOUNT", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "SALESINVOICEAMOUNT", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "INCOMEEXPENSEAMOUNT", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "ROUNDEDAMOUNT", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "SALESPAYMENTDIFFERENCE", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "PAYMENTAMOUNT", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "MARKUPAMOUNT", TableAlias = "T", ColumnAlias = "MARKUPAMOUNT",  AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "AMOUNTTOACCOUNT", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TOTALDISCAMOUNT", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "NUMBEROFITEMS", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "OILTAX", ColumnAlias = "OILTAX", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TRANSDATE", ColumnAlias = "BEGINTIME", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "MIN"},
                    new TableColumn {ColumnName = "TRANSDATE", ColumnAlias = "ENDTIME", TableAlias = "T", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "MAX"}
                };

                List<TableColumn> groupByColumns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "CURRENCY", TableAlias = "T", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "ENTRYSTATUS", TableAlias = "T"},
                    new TableColumn {ColumnName = "TYPE", TableAlias = "T"},
                    new TableColumn {ColumnName = "SUBTYPE", TableAlias = "T"}
                };

                List<Condition> conditions = new List<Condition>
                {
                    new Condition { Operator = "AND", ConditionValue = "TYPE = 2"}
                };

                List<Join> TransactionBuilderJoins = new List<Join>();

                if (startDateTime != null && startDateTime != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSDATE >= CONVERT(datetime, @startDateTime, 103) " });

                    MakeParam(cmd, "startDateTime", startDateTime.DateTime.Date, SqlDbType.DateTime);
                }

                if (endDateTime != null && endDateTime != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSDATE <= CONVERT(datetime, @endDateTime, 103) " });

                    MakeParam(cmd, "endDateTime", endDateTime.DateTime.Date, SqlDbType.DateTime);
                }

                if (storeID != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.STORE = @storeID " });

                    MakeParam(cmd, "storeID", storeID);
                }

                if (forDefaultCustomers)
                {
                    TransactionBuilderJoins.Add
                    (
                        new Join
                        {
                            Condition = "RS.STOREID = T.STORE AND (T.CUSTACCOUNT <> RS.DEFAULTCUSTACCOUNT OR T.CUSTACCOUNT = '')",
                            Table = "RBOSTORETABLE",
                            TableAlias = "RS"
                        }
                    );
                }

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.ENTRYSTATUS = 0" });

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("RBOTRANSACTIONTABLE", "T"),
                  QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                  QueryPartGenerator.JoinGenerator(TransactionBuilderJoins),
                  QueryPartGenerator.ConditionGenerator(conditions),
                  "GROUP BY " + QueryPartGenerator.GroupByColumnGenerator(groupByColumns)
                  );

                List<IFRetailTransaction> transactions = Execute<IFRetailTransaction, object>(entry, cmd, CommandType.Text, null, PopulateTransactionBuilder);
                return (transactions.Count > 0) ? transactions[0] : null;
            }
        }

        private static IFRetailTransaction TransactionBuilderSaleLines(IConnectionManager entry, IFRetailTransaction transaction, bool forDefaultCustomers, Date startDateTime = null, Date endDateTime = null, string storeID = null)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> selectionColumns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "ITEMID", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "ITEMDEPARTMENTID", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "ITEMGROUPID", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "TAXGROUP", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "UNIT", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "CURRENCY", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "PRICE", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "NETPRICE", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "QTY", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "DISCAMOUNT", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "COSTAMOUNT", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "NETAMOUNT", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "DISCAMOUNTFROMSTDPRICE", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TOTALROUNDEDAMOUNT", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "LINEDSCAMOUNT", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "CUSTDISCAMOUNT", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "INFOCODEDISCAMOUNT", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "CUSTINVOICEDISCAMOUNT", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "UNITQTY", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TAXAMOUNT", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TOTALDISCAMOUNT", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "PERIODICDISCAMOUNT", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "WHOLEDISCAMOUNTWITHTAX", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TOTALDISCAMOUNTWITHTAX", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "LINEDISCAMOUNTWITHTAX", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "PERIODICDISCAMOUNTWITHTAX", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "RETURNQTY", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "OILTAX", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "PRICEUNIT", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "NETAMOUNTINCLTAX", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "CASE WHEN QTY > 0 THEN 'RETURN' ELSE 'SALE' END ", ColumnAlias = "SALEORRETURN"}
                };

                List<TableColumn> groupByColumns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "ITEMID", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "ITEMDEPARTMENTID", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "ITEMGROUPID", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "TAXGROUP", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "UNIT", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "CURRENCY", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "CASE WHEN QTY > 0 THEN 'RETURN' ELSE 'SALE' END ", ColumnAlias = "SALEORRETURN"}
                };

                List<Condition> conditions = new List<Condition>
                {
                    new Condition { Operator = "AND", ConditionValue = "TYPE = 2"}
                };

                List<Join> TransactionBuilderJoins = new List<Join>
                {
                    new Join
                    {
                        Condition = "T.TRANSACTIONID = ST.TRANSACTIONID AND T.STORE = ST.STORE AND T.TERMINAL = ST.TERMINALID",
                        Table = "RBOTRANSACTIONTABLE",
                        TableAlias = "T"
                    }
                };

                if (startDateTime != null && startDateTime != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSDATE >= CONVERT(datetime, @startDateTime, 103) " });

                    MakeParam(cmd, "startDateTime", startDateTime.DateTime.Date, SqlDbType.DateTime);
                }

                if (endDateTime != null && endDateTime != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSDATE <= CONVERT(datetime, @endDateTime, 103) " });

                    MakeParam(cmd, "endDateTime", endDateTime.DateTime.Date, SqlDbType.DateTime);
                }

                if (storeID != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.STORE = @storeID " });

                    MakeParam(cmd, "storeID", storeID);
                }

                if (forDefaultCustomers)
                {
                    TransactionBuilderJoins.Add
                    (
                        new Join
                        {
                            Condition = "RS.STOREID = T.STORE AND (T.CUSTACCOUNT <> RS.DEFAULTCUSTACCOUNT OR T.CUSTACCOUNT = '')",
                            Table = "RBOSTORETABLE",
                            TableAlias = "RS"
                        }
                    );
                }

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.ENTRYSTATUS = 0" });

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("RBOTRANSACTIONSALESTRANS", "ST"),
                  QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                  QueryPartGenerator.JoinGenerator(TransactionBuilderJoins),
                  QueryPartGenerator.ConditionGenerator(conditions),
                  "GROUP BY " + QueryPartGenerator.GroupByColumnGenerator(groupByColumns)
                  );

                transaction.SaleItems = Execute<IFSaleLineItem, object>(entry, cmd, CommandType.Text, null, PopulateTransactionSaleLineItemsBuilder);
                return transaction;
            }
        }

        private IFRetailTransaction TransactionBuilderTenderLines(IConnectionManager entry, IFRetailTransaction transaction, bool forDefaultCustomers, Date startDateTime = null, Date endDateTime = null, string storeID = null)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> selectionColumns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "TENDERTYPE", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "CURRENCY", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "AMOUNTTENDERED", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "AMOUNTCUR", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "QTY", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "LOYALTYPOINTS", TableAlias = "ST", AggregateExternally = AggregationSetting.InternalAggregation, AggregateFunction = "SUM", IsNull = true, NullValue = "0.0"},
                };

                List<TableColumn> groupByColumns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "TENDERTYPE", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "CURRENCY", TableAlias = "ST", IsNull = true, NullValue = "''"},
                };

                List<Condition> conditions = new List<Condition>
                {
                    new Condition { Operator = "AND", ConditionValue = "TYPE = 2"}
                };

                List<Join> TransactionBuilderJoins = new List<Join>
                {
                    new Join
                    {
                        Condition = "T.TRANSACTIONID = ST.TRANSACTIONID AND T.STORE = ST.STORE AND T.TERMINAL = ST.TERMINAL",
                        Table = "RBOTRANSACTIONTABLE",
                        TableAlias = "T"
                    }
                };

                if (startDateTime != null && startDateTime != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSDATE >= CONVERT(datetime, @startDateTime, 103) " });

                    MakeParam(cmd, "startDateTime", startDateTime.DateTime.Date, SqlDbType.DateTime);
                }

                if (endDateTime != null && endDateTime != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSDATE <= CONVERT(datetime, @endDateTime, 103) " });

                    MakeParam(cmd, "endDateTime", endDateTime.DateTime.Date, SqlDbType.DateTime);
                }

                if (storeID != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.STORE = @storeID " });

                    MakeParam(cmd, "storeID", storeID);
                }

                if (forDefaultCustomers)
                {
                    TransactionBuilderJoins.Add
                    (
                        new Join
                        {
                            Condition = "RS.STOREID = T.STORE AND (T.CUSTACCOUNT <> RS.DEFAULTCUSTACCOUNT OR T.CUSTACCOUNT = '')",
                            Table = "RBOSTORETABLE",
                            TableAlias = "RS"
                        }
                    );
                }

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.ENTRYSTATUS = 0" });

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("RBOTRANSACTIONPAYMENTTRANS", "ST"),
                  QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                  QueryPartGenerator.JoinGenerator(TransactionBuilderJoins),
                  QueryPartGenerator.ConditionGenerator(conditions),
                  "GROUP BY " + QueryPartGenerator.GroupByColumnGenerator(groupByColumns)
                  );

                transaction.TenderLines = Execute<IFTenderLineItem, object>(entry, cmd, CommandType.Text, null, PopulateTransactionTenderLineItemsBuilder);
                return transaction;
            }
        }
        
        public virtual List<IFRetailTransaction> GetIFTransactions(IConnectionManager entry, bool? onlyForDefaultCustomers = null, Date startDateTime = null, Date endDateTime = null, int? replicationFrom = null, string storeID = null, string terminalID = null, string transactionID = null, string customerID = null)
        {
            List<IFRetailTransaction> transactions = GetIFRetailTransactionList(entry, onlyForDefaultCustomers, startDateTime, endDateTime, replicationFrom, storeID, terminalID, transactionID, customerID);
            if (transactions.Count > 0)
            {
                foreach (IFRetailTransaction transaction in transactions)
                {
                    transaction.SaleItems = GetIFSaleLines(entry, transaction.TransactionID);
                    transaction.TenderLines = GetIFTenderLines(entry, transaction.TransactionID);
                }
            }

            return transactions;
        }

        private List<IFRetailTransaction> GetIFRetailTransactionList(IConnectionManager entry, bool? excludeDefaultCustomers = null, Date startDateTime = null, Date endDateTime = null, int? replicationFrom = null, string storeID = null, string terminalID = null, string transactionID = null, string customerID = null)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> selectionColumns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "TRANSACTIONID", TableAlias = "T", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "CURRENCY", TableAlias = "T", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "ENTRYSTATUS", TableAlias = "T"},
                    new TableColumn {ColumnName = "TYPE", TableAlias = "T"},
                    new TableColumn {ColumnName = "NETAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "GROSSAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "SALESORDERAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "SALESINVOICEAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "INCOMEEXPENSEAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "ROUNDEDAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "SALESPAYMENTDIFFERENCE", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "PAYMENTAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "MARKUPAMOUNT", TableAlias = "T", ColumnAlias = "MARKUPAMOUNT", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "AMOUNTTOACCOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TOTALDISCAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "NUMBEROFITEMS", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "OILTAX", ColumnAlias = "OILTAX", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TRANSDATE", ColumnAlias = "BEGINTIME", TableAlias = "T"},
                    new TableColumn {ColumnName = "TRANSDATE", ColumnAlias = "ENDTIME", TableAlias = "T"}
                };

                List<Condition> conditions = new List<Condition>
                {
                    new Condition { Operator = "AND", ConditionValue = "T.TYPE = 2"}
                };

                List<Join> TransactionBuilderJoins = new List<Join>();

                if (startDateTime != null && startDateTime != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSDATE >= CONVERT(datetime, @startDateTime, 103) " });

                    MakeParam(cmd, "startDateTime", startDateTime.DateTime.Date, SqlDbType.DateTime);
                }

                if (endDateTime != null && endDateTime != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSDATE <= CONVERT(datetime, @endDateTime, 103) " });

                    MakeParam(cmd, "endDateTime", endDateTime.DateTime.Date, SqlDbType.DateTime);
                }

                if (storeID != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.STORE = @storeID " });

                    MakeParam(cmd, "storeID", storeID);
                }

                if (terminalID != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TERMINAL = @terminalID " });

                    MakeParam(cmd, "terminalID", terminalID);
                }

                if (transactionID != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSACTIONID = @transactionID " });

                    MakeParam(cmd, "transactionID", transactionID);
                }

                if (customerID != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.CUSTACCOUNT = @customerID " });

                    MakeParam(cmd, "customerID", customerID);
                }

                if (replicationFrom != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.REPLICATIONCOUNTER >= @replicationFrom" });

                    MakeParam(cmd, "replicationFrom", replicationFrom);
                }

                if (excludeDefaultCustomers != null)
                {
                    if (!excludeDefaultCustomers.Value)
                    {
                        cmd.CommandText += "JOIN RBOSTORETABLE RS ON RS.STOREID = T.STORE AND (T.CUSTACCOUNT = RS.DEFAULTCUSTACCOUNT)";
                    }
                    else
                    {
                        cmd.CommandText += "JOIN RBOSTORETABLE RS ON RS.STOREID = T.STORE AND (T.CUSTACCOUNT <> RS.DEFAULTCUSTACCOUNT OR T.CUSTACCOUNT = '')";
                    }
                }

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.ENTRYSTATUS = 0" });

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("RBOTRANSACTIONTABLE", "T"),
                  QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                  QueryPartGenerator.JoinGenerator(TransactionBuilderJoins),
                  QueryPartGenerator.ConditionGenerator(conditions), 
                  string.Empty
                  );

                List<IFRetailTransaction> transactions = Execute<IFRetailTransaction, object>(entry, cmd, CommandType.Text, null, PopulateIFTransaction);
                return transactions;
            }
        }

        public virtual List<IFRetailTransaction> GetIFTransactionsByType(IConnectionManager entry, TypeOfTransaction type, Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> selectionColumns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "TRANSACTIONID", TableAlias = "T", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "CURRENCY", TableAlias = "T", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "ENTRYSTATUS", TableAlias = "T"},
                    new TableColumn {ColumnName = "TYPE", TableAlias = "T"},
                    new TableColumn {ColumnName = "NETAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "GROSSAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "SALESORDERAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "SALESINVOICEAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "INCOMEEXPENSEAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "ROUNDEDAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "SALESPAYMENTDIFFERENCE", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "PAYMENTAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "MARKUPAMOUNT", TableAlias = "T", ColumnAlias = "MARKUPAMOUNT", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "AMOUNTTOACCOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TOTALDISCAMOUNT", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "NUMBEROFITEMS", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "OILTAX", ColumnAlias = "OILTAX", TableAlias = "T", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TRANSDATE", ColumnAlias = "BEGINTIME", TableAlias = "T"},
                    new TableColumn {ColumnName = "TRANSDATE", ColumnAlias = "ENDTIME", TableAlias = "T"}
                };

                List<Condition> conditions = new List<Condition>
                {
                    new Condition { Operator = "AND", ConditionValue = "T.ENTRYSTATUS = 0" },
                    new Condition { Operator = "AND", ConditionValue = $"T.TYPE = {(int)type}" }
                };

                List<Join> TransactionBuilderJoins = new List<Join>();

                if (startDateTime != null && startDateTime != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSDATE >= CONVERT(datetime, @startDateTime, 103) " });

                    MakeParam(cmd, "startDateTime", startDateTime.DateTime.Date, SqlDbType.DateTime);
                }

                if (endDateTime != null && endDateTime != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSDATE <= CONVERT(datetime, @endDateTime, 103) " });

                    MakeParam(cmd, "endDateTime", endDateTime.DateTime.Date, SqlDbType.DateTime);
                }

                if (storeID != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.STORE = @storeID " });

                    MakeParam(cmd, "storeID", storeID);
                }

                if (statementID != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.STATEMENTID = @statementID " });

                    MakeParam(cmd, "statementID", statementID);
                }

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("RBOTRANSACTIONTABLE", "T"),
                  QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                  QueryPartGenerator.JoinGenerator(TransactionBuilderJoins),
                  QueryPartGenerator.ConditionGenerator(conditions),
                  string.Empty
                  );

                List<IFRetailTransaction> transactions = Execute<IFRetailTransaction, object>(entry, cmd, CommandType.Text, null, PopulateIFTransaction);
                return transactions;
            }
        }

        private static List<IFSaleLineItem> GetIFSaleLines(IConnectionManager entry, RecordIdentifier transactionID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> selectionColumns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "ITEMID", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "ITEMDEPARTMENTID", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "ITEMGROUPID", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "TAXGROUP", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "UNIT", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "CURRENCY", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "PRICE", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "NETPRICE", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "QTY", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "DISCAMOUNT", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "COSTAMOUNT", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "NETAMOUNT", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "DISCAMOUNTFROMSTDPRICE", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TOTALROUNDEDAMOUNT", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "LINEDSCAMOUNT", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "CUSTDISCAMOUNT", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "INFOCODEDISCAMOUNT", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "CUSTINVOICEDISCAMOUNT", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "UNITQTY", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TAXAMOUNT", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TOTALDISCAMOUNT", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "PERIODICDISCAMOUNT", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "WHOLEDISCAMOUNTWITHTAX", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "TOTALDISCAMOUNTWITHTAX", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "LINEDISCAMOUNTWITHTAX", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "PERIODICDISCAMOUNTWITHTAX", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "RETURNQTY", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "OILTAX", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "PRICEUNIT", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "NETAMOUNTINCLTAX", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                };

                List<Condition> conditions = new List<Condition>
                {
                    new Condition { Operator = "AND", ConditionValue = "TYPE = 2"}
                };

                List<Join> TransactionBuilderJoins = new List<Join>
                {
                    new Join
                    {
                        Condition = "T.TRANSACTIONID = ST.TRANSACTIONID AND T.STORE = ST.STORE AND T.TERMINAL = ST.TERMINALID",
                        Table = "RBOTRANSACTIONTABLE",
                        TableAlias = "T"
                    }
                };

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSACTIONID = @transactionID " });
                MakeParam(cmd, "transactionID", (string)transactionID);

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.ENTRYSTATUS = 0" });

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("RBOTRANSACTIONSALESTRANS", "ST"),
                  QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                  QueryPartGenerator.JoinGenerator(TransactionBuilderJoins),
                  QueryPartGenerator.ConditionGenerator(conditions),
                  string.Empty
                  );

                return Execute<IFSaleLineItem, object>(entry, cmd, CommandType.Text, null, PopulateTransactionSaleLineItemsBuilder);
            }
        }

        private static List<IFTenderLineItem> GetIFTenderLines(IConnectionManager entry, RecordIdentifier transactionID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> selectionColumns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "TENDERTYPE", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "CURRENCY", TableAlias = "ST", IsNull = true, NullValue = "''"},
                    new TableColumn {ColumnName = "AMOUNTTENDERED", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "AMOUNTCUR", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "QTY", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                    new TableColumn {ColumnName = "LOYALTYPOINTS", TableAlias = "ST", IsNull = true, NullValue = "0.0"},
                };

                List<Condition> conditions = new List<Condition>
                {
                    new Condition { Operator = "AND", ConditionValue = "T.TYPE = 2"}
                };

                List<Join> TransactionBuilderJoins = new List<Join>
                {
                    new Join
                    {
                        Condition = "T.TRANSACTIONID = ST.TRANSACTIONID AND T.STORE = ST.STORE AND T.TERMINAL = ST.TERMINAL",
                        Table = "RBOTRANSACTIONTABLE",
                        TableAlias = "T"
                    }
                };

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.TRANSACTIONID = @transactionID " });
                MakeParam(cmd, "transactionID", (string)transactionID);

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.ENTRYSTATUS = 0" });

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("RBOTRANSACTIONPAYMENTTRANS", "ST"),
                  QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                  QueryPartGenerator.JoinGenerator(TransactionBuilderJoins),
                  QueryPartGenerator.ConditionGenerator(conditions),
                  string.Empty
                  );

                return Execute<IFTenderLineItem, object>(entry, cmd, CommandType.Text, null, PopulateTransactionTenderLineItemsBuilder);
            }
        }

        private static void GetCustomerPaymentTransaction(IConnectionManager entry, CustomerPaymentTransaction transaction)
        {
            var store = Providers.StoreData.Get(entry, transaction.StoreId, CacheType.CacheTypeApplicationLifeTime);
            if (store.ID != RecordIdentifier.Empty)
            {
                transaction.StoreName = store.Text;
                transaction.StoreAddress = store.Address.Address2;
                transaction.StoreTaxGroup = store.TaxGroup;
            }
            if (transaction.Customer.ID != "" && transaction.Customer.ID != RecordIdentifier.Empty)
            {
                transaction.Add(
                    Providers.CustomerData.Get(entry, transaction.Customer.ID, UsageIntentEnum.Minimal,
                                     CacheType.CacheTypeTransactionLifeTime), true);
            }
            var depositItem = new CustomerDepositItem
                {
                    Description = Resources.CustomerAccountDeposit,
                    Amount = transaction.Amount
                };
            //Customer Account Deposit

            if (depositItem.Amount != 0)
            {
                // Add the customer deposit line to the transaction
                transaction.CustomerDepositItem = depositItem;
            }
            var infoCodeID = new RecordIdentifier(transaction.TransactionId,
                new RecordIdentifier(1,
                    new RecordIdentifier((int)InfoCodeLineItem.InfocodeType.Header,
                        new RecordIdentifier(transaction.TerminalId,transaction.StoreId))));
            transaction.InfoCodeLines = TransactionProviders.InfocodeTransactionData.Get(entry, infoCodeID, transaction);

            var id = new RecordIdentifier(transaction.TransactionId, 
                new RecordIdentifier(transaction.TerminalId, transaction.StoreId));
            transaction.TenderLines = TransactionProviders.TenderLineItemData.Get(entry, id, transaction);
            Services.Interfaces.Services.CalculationService(entry).CalculateTotals(entry, transaction, transaction.StoreCurrencyCode);
        }

        private static void GetRetailTransaction(IConnectionManager entry, RetailTransaction transaction)
        {
            var store = Providers.StoreData.Get(entry, transaction.StoreId, CacheType.CacheTypeApplicationLifeTime);
            if (store != null && store.ID != RecordIdentifier.Empty)
            {
                transaction.StoreName = store.Text;
                transaction.StatementMethod = StatementGroupingMethod.POSTerminal; // Currently we only support terminal grouping
                transaction.StoreAddress = store.Address.Address2;
                if (transaction.StoreAddress == "")
                {
                    string addr = "";
                    if (store.Address.Address1 != "")
                    {
                        addr = store.Address.Address1;
                    }

                    if (store.Address.Zip != "" && store.Address.City != "")
                    {
                        if (addr != "")
                        {
                            addr += ", ";
                        }
                        addr += store.Address.Zip + " " + store.Address.City;
                    }

                    if (store.Address.Country != "")
                    {
                        if (addr != "")
                        {
                            addr += ", ";
                        }
                        addr += store.Address.Country;
                    }

                    if (addr != "")
                    {
                        transaction.StoreAddress = addr;
                    }
                }
                transaction.StoreTaxGroup = store.TaxGroup;
            }

            transaction.OrgReceiptId = transaction.ReceiptId;
            transaction.OrgStore = transaction.StoreId;
            transaction.OrgTerminal = transaction.TerminalId;
            transaction.OrgTransactionId = transaction.TransactionId;
            
            if (transaction.Customer.ID != "" && transaction.Customer.ID != RecordIdentifier.Empty)
            {
                var customer = Providers.CustomerData.Get(entry, transaction.Customer.ID, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime);
                if (transaction.Add(customer, true))
                {
                    transaction.AddInvoicedCustomer(Providers.CustomerData.Get(entry, customer.AccountNumber, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime));
                }
            }
            var parameter = Providers.ParameterData.Get(entry);
            if (parameter.ID != RecordIdentifier.Empty)
            {
                transaction.ReceiptSettings = parameter.ReceiptSettings;
            }
            else
            {
                transaction.ReceiptSettings = ReceiptSettingsEnum.Printed;
            }
            transaction.LoyaltyItem = TransactionProviders.LoyaltyTransactionData.GetTransactionLoyaltyItem(entry, transaction.TransactionId, transaction) ?? new LoyaltyItem();

            transaction.SaleItems = new LinkedList<ISaleLineItem>(TransactionProviders.SaleLineItemData.GetLineItemsForRetailTransaction(entry, transaction, false));

            transaction.Reprints = TransactionProviders.ReprintTransactionData.GetReprintInfo(entry, transaction.TransactionId, transaction);

            if (transaction.LoyaltyItem.Relation == LoyaltyPointsRelation.Discount)
            {
                foreach (SaleLineItem saleLineItem in transaction.SaleItems.Where(w => w.DiscountLines.Any(a => a.DiscountType == DiscountTransTypes.LoyaltyDisc)))
                {
                    saleLineItem.LoyaltyPoints = TransactionProviders.LoyaltyTransactionData.Get(entry, transaction.TransactionId, transaction.StoreId, transaction.TerminalId, saleLineItem.LineId, LoyaltyPointsRelation.Discount) ?? new LoyaltyItem();
                    transaction.LoyaltyItem.CalculatedPointsAmount += saleLineItem.DiscountLines.Sum(s => transaction.CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.PriceWithTax ? s.AmountWithTax : s.Amount);
                }
            }

            TransactionProviders.TaxTransactionData.PopulateTaxLinesForTransaction(transaction);
            var infoCodeID = new RecordIdentifier(transaction.TransactionId, new RecordIdentifier(1, new RecordIdentifier((int)InfoCodeLineItem.InfocodeType.Header, new RecordIdentifier(transaction.TerminalId, transaction.StoreId))));
            transaction.InfoCodeLines = TransactionProviders.InfocodeTransactionData.Get(entry, infoCodeID, transaction);

            var id = new RecordIdentifier(transaction.TransactionId, new RecordIdentifier(transaction.TerminalId, transaction.StoreId));
            transaction.TenderLines = new List<ITenderLineItem>(TransactionProviders.TenderLineItemData.Get(entry, id, transaction));
            transaction.OriginalTenderLines = CollectionHelper.Clone<ITenderLineItem, List<ITenderLineItem>>(transaction.TenderLines);

            var line = transaction.TenderLines.Find(f => f is CreditMemoTenderLineItem);
            transaction.CreditMemoItem = new CreditMemoItem(transaction);
            if (line != null)
            {
                transaction.CreditMemoItem.CreditMemoNumber = ((CreditMemoTenderLineItem)line).SerialNumber;
                transaction.CreditMemoItem.Amount = line.Amount * -1;
            }

            if (transaction.Cashier.ID == transaction.SalesPerson.ID)
            {
                transaction.SalesPerson = (Employee)transaction.Cashier.Clone();                
            }
            else if (transaction.SalesPerson.Exists)
            {
                var user = Providers.POSUserData.GetPerStore(entry, transaction.SalesPerson.ID, transaction.StoreId, false);

                if (user != null)
                {
                    transaction.SalesPerson.Name = entry.Settings.NameFormatter.Format(user.Name);
                    transaction.SalesPerson.NameOnReceipt = user.NameOnReceipt;
                    transaction.SalesPerson.Login = user.Login;
                }
            }
            else if (!transaction.SalesPerson.Exists)
            {
                transaction.SalesPerson.Clear();
            }
            
            var mysettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            if (entry.CurrentTerminalID == "" || mysettings == null)
            {
                return;//Site manager
            }

            transaction.PartnerObject = Services.Interfaces.Services.ApplicationService(entry).PartnerObject;
            if (transaction.PartnerObject != null)
            {
                transaction.PartnerObject.Rebuild(transaction);
            }

            if(mysettings.HardwareProfile.EftConnected)
            {
                Services.Interfaces.Services.EFTService(entry).EFTExtraInfo?.Rebuild(entry, transaction);
                Services.Interfaces.Services.EFTService(entry).EFTTransactionExtraInfo?.Rebuild(entry, transaction);
            }
        }

        public virtual RecordIdentifier GetTransactionID(IConnectionManager entry, RecordIdentifier receiptID, RecordIdentifier storeID, RecordIdentifier terminalID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TRANSACTIONID FROM RBOTRANSACTIONTABLE
                                    WHERE DATAAREAID = @dataAreaID AND STORE = @storeID AND TERMINAL = @terminalID 
                                    AND RECEIPTID = @receiptID AND ENTRYSTATUS = 0";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", storeID);
                MakeParam(cmd, "terminalID", terminalID);
                MakeParam(cmd, "receiptID", receiptID);
                return (string)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual List<ReceiptListItem> GetGetTransactionsForReceiptID(IConnectionManager entry, RecordIdentifier receiptID, RecordIdentifier storeID, RecordIdentifier terminalID, bool includeCanceldRefunds)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT RTT.STORE, 
                                    RTT.TERMINAL, 
                                    RTT.TRANSACTIONID, 
                                    RTT.TRANSDATE, 
                                    ISNULL(RTT.NETAMOUNT, 0) NETAMOUNT, 
                                    ISNULL(RTT.RECEIPTID, '') RECEIPTID, 
                                    ISNULL(RTT.STAFF, '') STAFF,
                                    ISNULL(TRT.TAXREFUNDDATAID, '00000000-0000-0000-0000-000000000000') as TAXREFUNDDATAID 
                                    FROM RBOTRANSACTIONTABLE RTT
                                    LEFT OUTER JOIN TAXREFUNDTRANSACTION TRT ON RTT.TRANSACTIONID = TRT.TRANSACTIONID 
                                    LEFT OUTER JOIN TAXREFUND TR ON TR.ID = TRT.TAXREFUNDDATAID ";
                if (!includeCanceldRefunds)
                {
                    cmd.CommandText += @" AND TR.STATUS = 0 ";
                }

                cmd.CommandText += @"WHERE RTT.DATAAREAID = @DATAAREAID 
                                    AND RTT.RECEIPTID = @RECEIPTID ";

                if (storeID != null && !storeID.IsEmpty && (string) storeID != "")
                {
                    cmd.CommandText += "AND RTT.STORE = @storeID ";
                    MakeParam(cmd, "storeID", storeID);
                }

                if (terminalID != null && !terminalID.IsEmpty && (string) terminalID != "")
                {
                    cmd.CommandText += "AND TERMINAL = @terminalID ";
                    MakeParam(cmd, "terminalID", terminalID);
                }

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "RECEIPTID", (string)receiptID);
                return Execute<ReceiptListItem>(entry, cmd, CommandType.Text, PopulateTransactionsForReceipts);
            }
        }

        private void PopulateTransactionsForReceipts(IDataReader dr, ReceiptListItem transactionInfo)
        {
            transactionInfo.ReceiptID = (string)dr["RECEIPTID"];
            transactionInfo.StoreID = (string)dr["STORE"];
            transactionInfo.TerminalID = (string)dr["TERMINAL"];
            transactionInfo.EmployeeID = (string)dr["STAFF"];
            transactionInfo.ID = (string)dr["TRANSACTIONID"];
            transactionInfo.TransactionDate = Date.FromAxaptaDate(dr["TRANSDATE"]);
            transactionInfo.PaidAmount = (decimal)dr["NETAMOUNT"];
            transactionInfo.TaxRefundID = (Guid) dr["TAXREFUNDDATAID"];
        }

        public virtual TypeOfTransaction GetTransactionType(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TYPE AS TRANSACTIONTYPE FROM RBOTRANSACTIONTABLE
                                    WHERE DATAAREAID = @dataAreaID AND TRANSACTIONID = @transactionID AND STORE = @storeID AND TERMINAL = @terminalID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", transactionID);
                MakeParam(cmd, "storeID", storeID);
                MakeParam(cmd, "terminalID", terminalID);
                return (TypeOfTransaction)(int)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual bool Exists(IConnectionManager entry, IPosTransaction transaction, bool ignoreTransactionType = false)
        {
            string[] fields = ignoreTransactionType 
                ? new string[] { "TRANSACTIONID", "STORE", "TERMINAL" } 
                : new string[] { "TRANSACTIONID", "STORE", "TERMINAL", "TYPE" };

            RecordIdentifier id = new RecordIdentifier(
                transaction.TransactionId,
                ignoreTransactionType
                    ? new RecordIdentifier(transaction.StoreId, transaction.TerminalId)
                    : new RecordIdentifier(transaction.StoreId, transaction.TerminalId, (int)transaction.TransactionType())
            );

            return RecordExists(entry, "RBOTRANSACTIONTABLE", fields, id);
        }

        public virtual void Save(IConnectionManager entry, ISettings settings, PosTransaction transaction)
        {
            if (transaction.Training)
            {
                transaction.EntryStatus = TransactionStatus.Training;
            }

            IConnectionManager dbTransaction;
            if (!(entry is IConnectionManagerTransaction))
            {
                // If we already have a transaction then we may not create transactions since SqlServer does not support that.
                dbTransaction = entry.CreateTransaction();
            }
            else
            {
                dbTransaction = entry;
            }

            try
            {
                // There are two cases when we might need a new transaction id
                //     * The transaction id is for some reason empty (can happen in isolated instances)
                //     * The transaction has an older id from before the number sequence was added to the POS. 
                //       This could be an older suspended transaction or a recalled transaction after a crash or shutdown. In this
                //       case we must check if the transaction id already exists and in that case assign a new one.
                if (String.IsNullOrEmpty(transaction.TransactionId) || new PosTransactionData().SequenceExists(dbTransaction, transaction.TransactionId))
                {
                    transaction.TransactionId = (string)DataProviderFactory.Instance.GenerateNumber<IPosTransactionData, PosTransaction>(dbTransaction); 
                }

                if (!transaction.Training && (transaction is RetailTransaction || transaction is CustomerPaymentTransaction))
                {
                    if (transaction is RetailTransaction &&
                        String.IsNullOrEmpty((string) settings.Terminal.ReceiptIDNumberSequence))
                    {
                        ((RetailTransaction) transaction).ReceiptIdNumberSequence = (string) settings.Terminal.ReceiptIDNumberSequence;
                    }
                    else if (transaction is CustomerPaymentTransaction &&
                             String.IsNullOrEmpty((string) settings.Terminal.ReceiptIDNumberSequence))
                    {
                        ((CustomerPaymentTransaction) transaction).ReceiptIdNumberSequence = (string) settings.Terminal.ReceiptIDNumberSequence;
                    }

                    if (transaction.EntryStatus != TransactionStatus.Voided &&
                         (String.IsNullOrEmpty(transaction.ReceiptId) ||
                        ((IApplicationService)dbTransaction.Service(ServiceType.ApplicationService)).ReceiptIDExists(dbTransaction, transaction.ReceiptId)))
                    {
                        transaction.ReceiptId = ((IApplicationService)dbTransaction.Service(ServiceType.ApplicationService)).GetNextReceiptId(dbTransaction, (string)settings.Terminal.ReceiptIDNumberSequence);
                    }
                }
                else 
                {
                    ((ITransactionService)dbTransaction.Service(ServiceType.TransactionService)).ReturnSequencedNumbers(dbTransaction, transaction, true, false);
                }

                SaveTransactionHeader(dbTransaction, transaction);
                SaveTransactionAudit(dbTransaction, transaction);
                SaveTransaction(dbTransaction, settings, transaction);

                if (!(entry is IConnectionManagerTransaction))
                {
                    ((IConnectionManagerTransaction) dbTransaction).Commit();
                }

                if (transaction.TransactionType() == TypeOfTransaction.LogOn)
                {
                    Providers.ClearPosisLogData.ClearLog(entry);
                }
            }
            catch (Exception ex)
            {
                if (!(entry is IConnectionManagerTransaction))
                {
                    ((IConnectionManagerTransaction) dbTransaction).Rollback();
                }

                throw ex;
            }
        }

        private static void SaveTransaction(IConnectionManager entry, ISettings settings, PosTransaction transaction)
        {
            var cashTenderLine = new TenderLineItem();
            var balanceTenderLine = new TenderLineItem();
            var paymentTypes = new List<PaymentMethod>();

            ValidateSecurity(entry);

            switch (transaction.TransactionType())
            {
                case TypeOfTransaction.Payment:
                    foreach (var line in ((CustomerPaymentTransaction) transaction).TenderLines)
                    {
                        TransactionProviders.TenderTransactionData.Insert(entry, line, transaction);
                    }
                    ////Save a customer transaction if needed for standalone pos
                    //if (((CustomerPaymentTransaction) transaction).Customer.ID != RecordIdentifier.Empty)
                    //{
                    //    TransactionProviders.CustomerTransactionData.Save(entry, settings, transaction);
                    //}                    

                    break;
                case TypeOfTransaction.Deposit:
                    
                    //Go through all the tender lines on the transaction and save them to the database
                    foreach (var line in ((RetailTransaction)transaction).TenderLines.Where(w => !w.PaidDeposit))
                    {
                        TransactionProviders.TenderTransactionData.Insert(entry, line, transaction);
                    }

                    //Go through all the infocode lines on the transaction and save them to the database                
                    foreach (var infocodeLine in ((RetailTransaction)transaction).InfoCodeLines)
                    {
                        TransactionProviders.InfocodeTransactionData.Insert(entry, infocodeLine, transaction, -1);
                    }

                    break;
                case TypeOfTransaction.Sales:
                    if (transaction is RetailTransaction && (string)((RetailTransaction)transaction).Hospitality.ActiveHospitalitySalesType != "")
                    {
                        DiningTableTransaction diningTableTransaction = new DiningTableTransaction();
                        diningTableTransaction.TransactionID = transaction.TransactionId;
                        diningTableTransaction.StoreID = transaction.StoreId;
                        diningTableTransaction.TerminalID = transaction.TerminalId;
                        diningTableTransaction.NoOfGuests = ((RetailTransaction)transaction).Hospitality.NoOfGuests;
                        diningTableTransaction.DiningTableNo = ((RetailTransaction)transaction).Hospitality.TableInformation.TableID;
                        diningTableTransaction.HospitalitySalesType = ((RetailTransaction)transaction).Hospitality.ActiveHospitalitySalesType;

                        TransactionProviders.DiningTableTransactionData.Save(entry, diningTableTransaction);
                    }

                    foreach (var line in ((RetailTransaction) transaction).SaleItems)
                    {
                        if (line.GetType() == typeof (SalesOrderLineItem))
                        {
                            TransactionProviders.OrderInvoiceTransactionData.InsertSalesOrderEntry(entry, (RetailTransaction)transaction, (SalesOrderLineItem)line);
                        }
                        else if (line is SalesInvoiceLineItem)
                        {
                            TransactionProviders.OrderInvoiceTransactionData.InsertSalesInvoiceEntry(entry, (RetailTransaction)transaction, (SalesInvoiceLineItem)line);
                        }
                        else if (line is IncomeExpenseItem)
                        {
                            TransactionProviders.IncomeExpenseItemData.Save(entry, (IncomeExpenseItem)line);
                        }
                        else
                        {
                            if (!line.Excluded)
                            {
                                TransactionProviders.SaleLineItemData.Save(entry, line, transaction);
                            }
                        }
                    }
                    //Go through all the tender lines on the transaction and save them to the database
                    foreach (var line in ((RetailTransaction) transaction).TenderLines)
                    {
                        TransactionProviders.TenderTransactionData.Insert(entry, line, transaction);
                    }
                    //Go through all the infocode lines on the transaction and save them to the database                
                    foreach (var infocodeLine in ((RetailTransaction) transaction).InfoCodeLines)
                    {
                        TransactionProviders.InfocodeTransactionData.Insert(entry, infocodeLine, transaction, -1);
                    }
                    // Save the loyalty line
                    if (!((RetailTransaction) transaction).LoyaltyItem.Empty)
                    {
                        if (((RetailTransaction) transaction).LoyaltyItem.PointsAdded)
                        {
                            TransactionProviders.LoyaltyTransactionData.Insert(entry, transaction);
                        }
                    }
                    //Save a customer transaction if needed for standalone pos
                    //if (((RetailTransaction) transaction).Customer.ID != RecordIdentifier.Empty)
                    //{
                    //    TransactionProviders.CustomerTransactionData.Save(entry, settings, transaction);
                    //}      
              

                    break;
                case TypeOfTransaction.FloatEntry:
                    cashTenderLine.Amount = ((FloatEntryTransaction) transaction).Amount;
                    cashTenderLine.LineId = 1;
                    cashTenderLine.TenderTypeId = (string) ResolveFloatBalanceTenderID(entry, transaction.StoreId);
                    cashTenderLine.CompanyCurrencyAmount = ((FloatEntryTransaction) transaction).AmountMST;
                    cashTenderLine.ExchrateMST = ((FloatEntryTransaction) transaction).ExchrateMST;
                    TransactionProviders.RemoveTenderTransactionData.Insert(entry, cashTenderLine, transaction);

                    balanceTenderLine.Amount = ((FloatEntryTransaction) transaction).Amount*-1;
                    balanceTenderLine.LineId = 2;
                    paymentTypes = Providers.PaymentMethodData.GetListForFunction(entry, transaction.StoreId, PaymentMethodDefaultFunctionEnum.FloatTender);
                    balanceTenderLine.TenderTypeId = paymentTypes.Count > 0 ? (string) paymentTypes[0].ID : "";
                    balanceTenderLine.CompanyCurrencyAmount = ((FloatEntryTransaction) transaction).AmountMST*-1;
                    balanceTenderLine.ExchrateMST = ((FloatEntryTransaction) transaction).ExchrateMST;
                    TransactionProviders.RemoveTenderTransactionData.Insert(entry, balanceTenderLine, transaction);
                    
                    break;
                case TypeOfTransaction.BankDropReversal:
                    foreach (var line in ((BankDropReversalTransaction) transaction).TenderLines)
                    {
                        //this goes into the RBOTRANSACTIONBANKEDTENDE20338 table
                        TransactionProviders.BankDropReversalTransactionData.Insert(entry, line, (BankDropReversalTransaction)transaction);
                    }
                    break;
                case TypeOfTransaction.RemoveTender:
                    if (!(transaction is RemoveTenderTransaction))
                    {
                        return;
                    }
                    cashTenderLine.Amount = ((RemoveTenderTransaction) transaction).Amount*-1;
                    cashTenderLine.LineId = 1;
                    cashTenderLine.TenderTypeId = (string) ResolveFloatBalanceTenderID(entry, transaction.StoreId);

                    cashTenderLine.CompanyCurrencyAmount = ((RemoveTenderTransaction) transaction).AmountMST*-1;
                    cashTenderLine.ExchrateMST = ((RemoveTenderTransaction) transaction).ExchrateMST;

                    balanceTenderLine.Amount = ((RemoveTenderTransaction) transaction).Amount;
                    balanceTenderLine.LineId = 2;
                    paymentTypes = Providers.PaymentMethodData.GetListForFunction(entry, transaction.StoreId, PaymentMethodDefaultFunctionEnum.FloatTender);
                    balanceTenderLine.TenderTypeId = paymentTypes.Count > 0 ? (string) paymentTypes[0].ID : "";
                    balanceTenderLine.CompanyCurrencyAmount = ((RemoveTenderTransaction) transaction).AmountMST;
                    balanceTenderLine.ExchrateMST = ((RemoveTenderTransaction) transaction).ExchrateMST;

                    TransactionProviders.RemoveTenderTransactionData.Insert(entry, cashTenderLine, transaction);
                    //TODO: Check where the status comes from
                    TransactionProviders.RemoveTenderTransactionData.Insert(entry, balanceTenderLine, transaction);
                    //TODO: Check where the status comes from
                    break;
                case TypeOfTransaction.SafeDropReversal:
                    foreach (var line in ((SafeDropReversalTransaction) transaction).TenderLines)
                    {
                        //this goes into the RBOTRANSACTIONSAFETENDERTRANS table
                        TransactionProviders.SafeDropReversalTransactionData.Insert(entry, line, (SafeDropReversalTransaction)transaction);
                    }
                    break;
                case TypeOfTransaction.TenderDeclaration:
                    if (!(transaction is TenderDeclarationTransaction))
                    {
                        return;
                    }
                    //Go through all the tenders in the transaction and save them in the database
                    foreach (var line in ((TenderDeclarationTransaction) transaction).TenderLines)
                    {
                        TransactionProviders.TenderDeclarationData.Save(entry, (TenderDeclarationTransaction)transaction, line);
                    }
                    break;
                case TypeOfTransaction.SafeDrop:
                    foreach (var line in ((SafeDropTransaction) transaction).TenderLines)
                    {
                        TransactionProviders.SafeDropTransactionData.Insert(entry, line, (SafeDropTransaction)transaction); //TODO: Check where the status comes from
                    }
                    break;
                case TypeOfTransaction.BankDrop:
                    foreach (var line in ((BankDropTransaction) transaction).TenderLines)
                    {
                        TransactionProviders.BankDropTransactionData.Insert(entry, line, (BankDropTransaction)transaction); //TODO: Check where the status comes from
                    }
                    break;
                case TypeOfTransaction.Log:
                    foreach (var line in ((ILogTransaction) transaction).LogLines)
                    {
                        TransactionProviders.LogTransactionData.Insert(entry, line, (ILogTransaction)transaction);
                    }
                    break;
            }
        }

        private void SaveTransactionHeader(IConnectionManager entry, PosTransaction transaction)
        {
            var grossAmountTemp = Decimal.Zero;

            var statement = new SqlServerStatement("RBOTRANSACTIONTABLE", StatementType.Insert, false);
            int timeNowInInt = ((DateTime.Now.Hour*3600) + (DateTime.Now.Minute*60) + DateTime.Now.Second);
            if (Exists(entry, transaction))
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("TRANSACTIONID", transaction.TransactionId);
                statement.AddCondition("STORE", transaction.StoreId);
                statement.AddCondition("TERMINAL", transaction.TerminalId);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.AddKey("TRANSACTIONID", transaction.TransactionId);
                statement.AddKey("STORE", transaction.StoreId);
                statement.AddKey("TERMINAL", transaction.TerminalId);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            statement.AddField("TYPE", (int)transaction.TransactionType(), SqlDbType.Int);
            statement.AddField("SUBTYPE", transaction.TransactionSubType, SqlDbType.Int);
            statement.AddField("STAFF", (string)transaction.Cashier.ID);
            statement.AddField("TRANSDATE", transaction.BeginDateTime, SqlDbType.DateTime);
            statement.AddField("TRANSTIME", timeNowInInt, SqlDbType.Int);
            statement.AddField("SHIFT", transaction.ShiftId);
            statement.AddField("SHIFTDATE",string.IsNullOrEmpty(transaction.ShiftId) ? DateTime.Now : transaction.ShiftDate, SqlDbType.DateTime);
            statement.AddField("WRONGSHIFT", 0, SqlDbType.TinyInt);
            statement.AddField("CREATEDONPOSTERMINAL", transaction.CreatedOnTerminalId);
            statement.AddField("STATEMENTCODE", ""); //Not used anymore
            statement.AddField("RECEIPTID", transaction.ReceiptId);
            statement.AddField("INFOCODEDISCGROUP", "");
            statement.AddField("TRANSCODE", "");
            statement.AddField("OPENDRAWER", transaction.OpenDrawer ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("CUSTDISCAMOUNT", 0, SqlDbType.Decimal);
            statement.AddField("ENTRYSTATUS", (int) transaction.EntryStatus, SqlDbType.Int);
            statement.AddField("NUMBEROFINVOICES", 0, SqlDbType.Int);
            statement.AddField("STATEMENTID", "");
            statement.AddField("COUNTER", 0, SqlDbType.Int);
            statement.AddField("TIMEWHENTRANSCLOSED", timeNowInInt, SqlDbType.Int);
            statement.AddField("TRANSTABLEID", 0, SqlDbType.Int);
            statement.AddField("REPLICATED", 0, SqlDbType.TinyInt);
            statement.AddField("INCLUDEDINSTATISTICS", 0, SqlDbType.TinyInt);
            statement.AddField("RETRIEVEDFROMRECEIPTID", "");
            statement.AddField("ZREPORTID", "");
            statement.AddField("ITEMSPOSTED", 0, SqlDbType.TinyInt);
            statement.AddField("MODIFIEDDATE", DateTime.Now, SqlDbType.DateTime);
            statement.AddField("MODIFIEDTIME", timeNowInInt, SqlDbType.Int);
            statement.AddField("MODIFIEDBY", "");
            statement.AddField("CREATEDDATE", DateTime.Now, SqlDbType.DateTime);
            statement.AddField("CREATEDTIME", Conversion.TimeToInt(DateTime.Now), SqlDbType.Int);
            statement.AddField("UNCONCLUDEDTRANS", transaction.UnconcludedTransaction ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("BUSINESSDAY", transaction.BusinessDay, SqlDbType.DateTime);
            statement.AddField("BUSINESSSYSTEMDAY", transaction.BusinessSystemDay, SqlDbType.DateTime);
            statement.AddField("ORIGNUMOFTRANSACTIONLINES", transaction.OriginalNumberOfTransactionLines, SqlDbType.Int);
            if (transaction is RetailTransaction)
            {
                statement.AddField("CUSTACCOUNT", (string) ((RetailTransaction) transaction).Customer.ID);
                statement.AddField("SALESPAYMENTDIFFERENCE", ((RetailTransaction) transaction).RoundingSalePmtDiff,SqlDbType.Decimal);
                statement.AddField("NETAMOUNT", (((RetailTransaction) transaction).NetAmount*-1), SqlDbType.Decimal);
                statement.AddField("PAYMENTAMOUNT",((RetailTransaction) transaction).Payment - ((RetailTransaction) transaction).MarkupItem.Amount, SqlDbType.Decimal);
                statement.AddField("DISCAMOUNT", ((RetailTransaction) transaction).LineDiscount + ((RetailTransaction) transaction).PeriodicDiscountAmount, SqlDbType.Decimal);
                statement.AddField("TOTALDISCAMOUNT", ((RetailTransaction) transaction).TotalDiscount, SqlDbType.Decimal);
                statement.AddField("NUMBEROFITEMS", ((RetailTransaction) transaction).NoOfItems, SqlDbType.Decimal);
                if (((RetailTransaction) transaction).Customer.ID != RecordIdentifier.Empty)
                {
                    statement.AddField("AMOUNTTOACCOUNT",
                                       ((RetailTransaction) transaction).Payment -
                                       ((RetailTransaction) transaction).SalesInvoiceAmounts -
                                       ((RetailTransaction) transaction).SalesOrderAmounts, SqlDbType.Decimal);
                    statement.AddField("TOACCOUNT", 1, SqlDbType.TinyInt); //Amount to be added to customer account
                }
                else
                {
                    statement.AddField("AMOUNTTOACCOUNT", 0, SqlDbType.Decimal);
                    statement.AddField("TOACCOUNT", 0, SqlDbType.TinyInt);
                }
                statement.AddField("ROUNDEDAMOUNT", (((RetailTransaction) transaction).RoundingDifference*-1), SqlDbType.Decimal);
                statement.AddField("NUMBEROFITEMLINES", ((RetailTransaction) transaction).NoOfItemLines, SqlDbType.Decimal);
                statement.AddField("REFUNDRECEIPTID", ((RetailTransaction) transaction).RefundReceiptId);
                statement.AddField("JOURNALID", ((RetailTransaction) transaction).JournalID);
                statement.AddField("SALEISRETURNSALE", ((RetailTransaction) transaction).SaleIsReturnSale, SqlDbType.TinyInt);
                statement.AddField("TIMEWHENTOTALPRESSED", ((RetailTransaction) transaction).TimeWhenTotalPressed, SqlDbType.Int);
                statement.AddField("POSTASSHIPMENT", ((RetailTransaction) transaction).PostAsShipment, SqlDbType.TinyInt);
                statement.AddField("SALESORDERAMOUNT", ((RetailTransaction) transaction).SalesOrderAmounts, SqlDbType.Decimal);
                statement.AddField("SALESINVOICEAMOUNT", ((RetailTransaction) transaction).SalesInvoiceAmounts, SqlDbType.Decimal);
                statement.AddField("INCOMEEXPENSEAMOUNT", ((RetailTransaction) transaction).IncomeExpenseAmounts, SqlDbType.Decimal);
            }
            else
            {
                statement.AddField("NETAMOUNT", 0, SqlDbType.Decimal);
                statement.AddField("COSTAMOUNT", 0, SqlDbType.Decimal);
                statement.AddField("DISCAMOUNT", 0, SqlDbType.Decimal);
                statement.AddField("TOTALDISCAMOUNT", 0, SqlDbType.Decimal);
                statement.AddField("NUMBEROFITEMS", 0, SqlDbType.Decimal);
                statement.AddField("AMOUNTTOACCOUNT", 0, SqlDbType.Decimal);
                statement.AddField("ROUNDEDAMOUNT", 0, SqlDbType.Decimal);
                statement.AddField("NUMBEROFITEMLINES", 0, SqlDbType.Decimal);
                statement.AddField("REFUNDRECEIPTID", "");
                statement.AddField("TOACCOUNT", 0, SqlDbType.TinyInt);
                statement.AddField("SALEISRETURNSALE", 0, SqlDbType.TinyInt);
                statement.AddField("TIMEWHENTOTALPRESSED", 0, SqlDbType.Int);
                statement.AddField("POSTASSHIPMENT", 0, SqlDbType.TinyInt);
                statement.AddField("SALESORDERAMOUNT", 0, SqlDbType.Decimal);
                statement.AddField("SALESINVOICEAMOUNT", 0, SqlDbType.Decimal);
                statement.AddField("EXCHRATE", transaction.StoreExchangeRate*100, SqlDbType.Decimal);
                statement.AddField("INCOMEEXPENSEAMOUNT", 0, SqlDbType.Decimal);                
            }
            switch (transaction.TransactionType())
            {
                case TypeOfTransaction.Sales:
                case TypeOfTransaction.Deposit:

                    if (String.IsNullOrEmpty(transaction.SalesSequenceID))
                    {
                        transaction.SalesSequenceID = (string) DataProviderFactory.Instance.GenerateNumber(entry, new SalesSequenceProvider());
                    }

                    statement.AddField("GROSSAMOUNT", (((RetailTransaction) transaction).NetAmountWithTax*-1), SqlDbType.Decimal);
                    statement.AddField("ISNETAMOUNTWITHTAXROUNDED", ((RetailTransaction)transaction).IsNetAmountWithTaxRounded, SqlDbType.Bit);
                    statement.AddField("NUMBEROFPAYMENTLINES", ((RetailTransaction) transaction).NoOfPaymentLines, SqlDbType.Int);
                    statement.AddField("CURRENCY", transaction.StoreCurrencyCode);
                    statement.AddField("CUSTPURCHASEORDER", ((RetailTransaction) transaction).CustomerPurchRequestId);
                    statement.AddField("RECEIPTEMAIL", ((RetailTransaction) transaction).ReceiptEmailAddress);
                    statement.AddField("COMMENT", transaction.Comment);
                    statement.AddField("INVOICECOMMENT", ((RetailTransaction) transaction).InvoiceComment);
                    statement.AddField("MARKUPAMOUNT", ((RetailTransaction) transaction).MarkupItem.Amount, SqlDbType.Decimal);
                    statement.AddField("MARKUPDESCRIPTION", ((RetailTransaction) transaction).MarkupItem.Description);
                    statement.AddField("ORGTRANSSTORE", ((RetailTransaction) transaction).OrgStore);
                    statement.AddField("ORGTRANSTERMINAL", ((RetailTransaction) transaction).OrgTerminal);
                    statement.AddField("ORGTRANSACTIONID", ((RetailTransaction) transaction).OrgTransactionId);
                    statement.AddField("ORGRECEIPTID", ((RetailTransaction) transaction).OrgReceiptId);
                    statement.AddField("OILTAX", ((RetailTransaction) transaction).Oiltax, SqlDbType.Decimal);
                    statement.AddField("TAXINCLINPRICE", ((RetailTransaction) transaction).TaxIncludedInPrice ? 1 : 0, SqlDbType.TinyInt);

                    if (transaction.TransactionType() == TypeOfTransaction.Deposit)
                    {
                        statement.AddField("CUSTOMERORDERID", (Guid) ((RetailTransaction)transaction).CustomerOrder.ID, SqlDbType.UniqueIdentifier);
                    }

                    if (transaction.TransactionType() == TypeOfTransaction.Sales && !((RetailTransaction) transaction).CustomerOrder.Empty())
                    {
                        statement.AddField("CUSTOMERORDERID", (Guid)((RetailTransaction)transaction).CustomerOrder.ID, SqlDbType.UniqueIdentifier);
                    }

                    break;
                case TypeOfTransaction.BankDropReversal:
                    grossAmountTemp += ((BankDropReversalTransaction) transaction).TenderLines.Sum(line => line.CompanyCurrencyAmount);
                    statement.AddField("CUSTACCOUNT", "");
                    statement.AddField("GROSSAMOUNT", grossAmountTemp*-1, SqlDbType.Decimal);
                    statement.AddField("CURRENCY", transaction.StoreCurrencyCode);
                    statement.AddField("NUMBEROFPAYMENTLINES", 0, SqlDbType.Int);
                    statement.AddField("PAYMENTAMOUNT", 0, SqlDbType.Decimal);
                    statement.AddField("SALESPAYMENTDIFFERENCE", 0, SqlDbType.Decimal);
                    break;
                case TypeOfTransaction.SafeDropReversal:
                    grossAmountTemp += ((SafeDropReversalTransaction) transaction).TenderLines.Sum(line => line.CompanyCurrencyAmount);
                    statement.AddField("CUSTACCOUNT", "");
                    statement.AddField("GROSSAMOUNT", grossAmountTemp*-1, SqlDbType.Decimal);
                    statement.AddField("CURRENCY", transaction.StoreCurrencyCode);
                    statement.AddField("NUMBEROFPAYMENTLINES", 0, SqlDbType.Int);
                    statement.AddField("PAYMENTAMOUNT", 0, SqlDbType.Decimal);
                    statement.AddField("SALESPAYMENTDIFFERENCE", 0, SqlDbType.Decimal);
                    break;
                case TypeOfTransaction.FloatEntry:
                    statement.AddField("GROSSAMOUNT", ((FloatEntryTransaction) transaction).Amount*-1, SqlDbType.Decimal);
                    statement.AddField("CUSTACCOUNT", "");
                    statement.AddField("CURRENCY", transaction.StoreCurrencyCode);
                    statement.AddField("NUMBEROFPAYMENTLINES", 2, SqlDbType.Int);
                    statement.AddField("DESCRIPTION", ((FloatEntryTransaction) transaction).Description);
                    statement.AddField("PAYMENTAMOUNT", 0, SqlDbType.Decimal);
                    statement.AddField("SALESPAYMENTDIFFERENCE", 0, SqlDbType.Decimal);
                    break;
                case TypeOfTransaction.EndOfDay:
                case TypeOfTransaction.EndOfShift:
                case TypeOfTransaction.OpenDrawer:
                case TypeOfTransaction.LogOff:
                case TypeOfTransaction.LogOn:
                case TypeOfTransaction.Inventory:
                    statement.AddField("CUSTACCOUNT", "");
                    statement.AddField("GROSSAMOUNT", 0, SqlDbType.Decimal);
                    statement.AddField("CURRENCY", "");
                    statement.AddField("NUMBEROFPAYMENTLINES", 0, SqlDbType.Int);
                    statement.AddField("PAYMENTAMOUNT", 0, SqlDbType.Decimal);
                    statement.AddField("SALESPAYMENTDIFFERENCE", 0, SqlDbType.Decimal);
                    break;
                case TypeOfTransaction.TenderDeclaration:
                    statement.AddField("CUSTACCOUNT", "");
                    statement.AddField("GROSSAMOUNT", 0, SqlDbType.Decimal);
                    statement.AddField("NUMBEROFPAYMENTLINES", 0, SqlDbType.Int);
                    statement.AddField("CURRENCY", transaction is TenderDeclarationTransaction ? transaction.StoreCurrencyCode : "");
                    statement.AddField("PAYMENTAMOUNT", 0, SqlDbType.Decimal);
                    statement.AddField("SALESPAYMENTDIFFERENCE", 0, SqlDbType.Decimal);
                    break;
                case TypeOfTransaction.RemoveTender: //RemoveTenderTransaction
                    if (transaction is RemoveTenderTransaction)
                    {
                        statement.AddField("GROSSAMOUNT", ((RemoveTenderTransaction) transaction).Amount, SqlDbType.Decimal);
                        statement.AddField("CURRENCY", transaction.StoreCurrencyCode);
                        statement.AddField("NUMBEROFPAYMENTLINES", 2, SqlDbType.Int);
                        statement.AddField("DESCRIPTION", ((RemoveTenderTransaction) transaction).Description);
                    }
                    else
                    {
                        statement.AddField("GROSSAMOUNT", 0, SqlDbType.Decimal);
                        statement.AddField("CURRENCY", "");
                        statement.AddField("NUMBEROFPAYMENTLINES", 0, SqlDbType.Int);
                    }
                    statement.AddField("CUSTACCOUNT", "");
                    statement.AddField("PAYMENTAMOUNT", 0, SqlDbType.Decimal);
                    statement.AddField("SALESPAYMENTDIFFERENCE", 0, SqlDbType.Decimal);
                    break;
                case TypeOfTransaction.Payment:
                    statement.AddField("CUSTACCOUNT", (string)((CustomerPaymentTransaction)transaction).Customer.ID);
                    statement.AddField("GROSSAMOUNT", ((CustomerPaymentTransaction) transaction).Amount*-1, SqlDbType.Decimal);
                    statement.AddField("PAYMENTAMOUNT", ((CustomerPaymentTransaction) transaction).Payment*-1, SqlDbType.Decimal);
                    statement.AddField("NUMBEROFPAYMENTLINES", 0, SqlDbType.Int);
                    statement.AddField("CURRENCY", transaction.StoreCurrencyCode);
                    statement.AddField("SALESPAYMENTDIFFERENCE", ((CustomerPaymentTransaction) transaction).RoundingSalePmtDiff, SqlDbType.Decimal);
                    break;
                case TypeOfTransaction.SafeDrop:
                    foreach (TenderLineItem line in ((SafeDropTransaction) transaction).TenderLines)
                    {
                        grossAmountTemp += line.CompanyCurrencyAmount;
                    }
                    statement.AddField("CUSTACCOUNT", "");
                    statement.AddField("GROSSAMOUNT", grossAmountTemp, SqlDbType.Decimal);
                    statement.AddField("CURRENCY", transaction.StoreCurrencyCode);
                    statement.AddField("NUMBEROFPAYMENTLINES", 0, SqlDbType.Int);
                    statement.AddField("PAYMENTAMOUNT", 0, SqlDbType.Decimal);
                    statement.AddField("SALESPAYMENTDIFFERENCE", 0, SqlDbType.Decimal);
                    break;
                case TypeOfTransaction.BankDrop:
                    foreach (TenderLineItem line in ((BankDropTransaction) transaction).TenderLines)
                    {
                        grossAmountTemp += line.CompanyCurrencyAmount;
                    }
                    statement.AddField("CUSTACCOUNT", "");
                    statement.AddField("GROSSAMOUNT", grossAmountTemp, SqlDbType.Decimal);
                    statement.AddField("CURRENCY", transaction.StoreCurrencyCode);
                    statement.AddField("NUMBEROFPAYMENTLINES", 0, SqlDbType.Int);
                    statement.AddField("PAYMENTAMOUNT", 0, SqlDbType.Decimal);
                    statement.AddField("SALESPAYMENTDIFFERENCE", 0, SqlDbType.Decimal);
                    break;
            }

            if (transaction.SalesSequenceID == null)
                transaction.SalesSequenceID = string.Empty;
            statement.AddField("SALESSEQUENCEID", transaction.SalesSequenceID);

            entry.Connection.ExecuteStatement(statement);
        }

        private void SaveTransactionAudit(IConnectionManager entry, PosTransaction transaction)
        {
            if (transaction.AuditingLines != null && transaction.AuditingLines.Count > 0)
            {
                var auditData = TransactionProviders.OperationAuditingData;
                foreach (var auditLine in transaction.AuditingLines)
                {
                    auditData.Save(entry, auditLine);
                }
            }
        }

        private static RecordIdentifier ResolveFloatBalanceTenderID(IConnectionManager entry, RecordIdentifier storeID)
        {
            var identifier = Providers.StorePaymentMethodData.GetChangeTenderForFunction(entry, storeID, 
                PaymentMethodDefaultFunctionEnum.FloatTender);
            if (identifier == null)
            {
                identifier = Providers.StorePaymentMethodData.GetCashTender(entry, storeID);
            }
            return identifier;
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RBOTRANSACTIONTABLE", "TRANSACTIONID", id);
        }

        public virtual RecordIdentifier SequenceID
        {
            get { return "TRANSACTIONID"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOTRANSACTIONTABLE", "TRANSACTIONID", sequenceFormat, startingRecord, numberOfRecords);
        }
    }
}
