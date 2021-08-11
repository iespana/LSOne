using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.ReceiptBrowser.Reports
{
    public partial class ReportReceipt : DevExpress.XtraReports.UI.XtraReport, IReport
    {
        RecordIdentifier id;
        RecordIdentifier storeID;
        RecordIdentifier terminalID;

        public ReportReceipt(RecordIdentifier id, RecordIdentifier storeID, RecordIdentifier terminalID)
            : this()
        {
            this.id = id;
            this.storeID = storeID;
            this.terminalID = terminalID;
        }

        public ReportReceipt()
        {
            this.id = RecordIdentifier.Empty;
            this.storeID = RecordIdentifier.Empty;
            this.terminalID = RecordIdentifier.Empty;

            InitializeComponent();
        }

        
        public void Run()
        {
            string storeCurrencyCode;
            RetailTransaction posTransaction;
           
            if (id == RecordIdentifier.Empty)
            {
                return;
            }

            storeCurrencyCode = Providers.StoreData.GetStoreCurrencyCode(PluginEntry.DataModel, storeID);

            var rounding = Services.Interfaces.Services.RoundingService(PluginEntry.DataModel);

            posTransaction = new RetailTransaction("", "", true);
            TransactionProviders.PosTransactionData.GetTransaction(PluginEntry.DataModel, id, storeID, terminalID, posTransaction, false);

            if (posTransaction == null)
            {
                return;
            }

            DataRow datarow = TransactionDataset.Header.NewRow();

            datarow["TransactionID"] = posTransaction.TransactionId;
            datarow["ReceiptID"] = posTransaction.ReceiptId;
            datarow["PosID"] = posTransaction.TerminalId;
            datarow["CashierName"] = ((RetailTransaction)posTransaction).Cashier.Name;
            datarow["DateTime"] = posTransaction.BeginDateTime;
            datarow["TotalAmount"] = rounding.RoundString(PluginEntry.DataModel, ((RetailTransaction)posTransaction).NetAmountWithTax + posTransaction.MarkupItem.Amount,storeCurrencyCode, false);

            xtcTotalRounding.Visible = posTransaction.RoundingSalePmtDiff != 0;

            if (posTransaction.RoundingSalePmtDiff != 0)
            {
                datarow["TotalRounding"] = rounding.RoundString(PluginEntry.DataModel, ((RetailTransaction)posTransaction).RoundingSalePmtDiff * -1, storeCurrencyCode, false);  
            }
            
            datarow["InvoiceComment"] = ((RetailTransaction)posTransaction).InvoiceComment;
            datarow["PurchRequestId"] = ((RetailTransaction)posTransaction).CustomerPurchRequestId;

            datarow["CopyText"] = Properties.Resources.ReceiptCopy;

            TransactionDataset.Header.Rows.Add(datarow);

            datarow = TransactionDataset.InvoicedCustomer.NewRow();

            //Figure out if we use customer or InvoicedCustomer
            if (((RetailTransaction)posTransaction).InvoicedCustomer.ID == "" || (((RetailTransaction)posTransaction).InvoicedCustomer.ID.IsEmpty))
            {
                datarow["Account"] = ((RetailTransaction)posTransaction).Customer.ID;
                datarow["Name"] = ((RetailTransaction)posTransaction).Customer.GetFormattedName( PluginEntry.DataModel.Settings.NameFormatter) ;
                datarow["Address"] = PluginEntry.DataModel.Settings.LocalizationContext.FormatSingleLine(
                    ((RetailTransaction)posTransaction).Customer.DefaultShippingAddress,
                    PluginEntry.DataModel.Cache);
                datarow["OrgId"] = ((RetailTransaction)posTransaction).Customer.IdentificationNumber;
            }
            else
            {
                // Load data into InvoicedCustomer
                datarow["Account"] = ((RetailTransaction)posTransaction).InvoicedCustomer.ID;
                datarow["Name"] =
                    ((RetailTransaction) posTransaction).InvoicedCustomer.GetFormattedName(PluginEntry.DataModel.Settings.NameFormatter);
                datarow["Address"] = PluginEntry.DataModel.Settings.LocalizationContext.FormatSingleLine(
                    ((RetailTransaction)posTransaction).InvoicedCustomer.DefaultShippingAddress,
                    PluginEntry.DataModel.Cache);
                datarow["OrgId"] = ((RetailTransaction)posTransaction).InvoicedCustomer.IdentificationNumber;
            }

            TransactionDataset.InvoicedCustomer.Rows.Add(datarow);


            // Load data into customer
            datarow = TransactionDataset.Customer.NewRow();
            datarow["Account"] = ((RetailTransaction)posTransaction).Customer.ID;
            datarow["Name"] =
                ((RetailTransaction) posTransaction).Customer.GetFormattedName(
                    PluginEntry.DataModel.Settings.NameFormatter);

            datarow["Address"] = PluginEntry.DataModel.Settings.LocalizationContext.FormatSingleLine(
                ((RetailTransaction)posTransaction).Customer.DefaultShippingAddress,
                PluginEntry.DataModel.Cache);
            datarow["OrgId"] = ((RetailTransaction)posTransaction).Customer.IdentificationNumber;

            TransactionDataset.Customer.Rows.Add(datarow);

            // Load data into itemlines
            int lineID = 0;
            foreach (SaleLineItem item in ((RetailTransaction)posTransaction).SaleItems)
            {
                Services.Interfaces.Services.CalculationService(PluginEntry.DataModel).CalculatePeriodicDiscountPercent(item, posTransaction);

                if (item.Voided == false && (!item.IsAssemblyComponent || item.ParentAssembly.ShallDisplayWithComponents(ExpandAssemblyLocation.OnReceipt)))
                {
                    ItemSalesTaxGroup itemSalesTaxGroup = Providers.ItemSalesTaxGroupData.Get(PluginEntry.DataModel, item.TaxGroupId);
                    string itemSalesTaxGroupDescription = "";
                    if (itemSalesTaxGroup != null)
                    {
                        if (itemSalesTaxGroup.ReceiptDisplay != "")
                        {
                            itemSalesTaxGroupDescription = itemSalesTaxGroup.ReceiptDisplay;
                        }
                        else
                        {
                            itemSalesTaxGroupDescription = itemSalesTaxGroup.Text;
                        }
                    }
                    

                    datarow = TransactionDataset.Lines.NewRow();
                    datarow["TaxCode"] = itemSalesTaxGroupDescription; // Description is put here as a temp fix until report get an overhaul
                    datarow["ItemID"] = item.ItemId;
                    //datarow["ItemID"] = item.Dimension.VariantNumber == "" ? item.ItemId : (string)item.Dimension.VariantNumber;
                    datarow["LineID"] = lineID += 1;
                    datarow["ItemName"] = item.Description;
                    datarow["Quantity"] = rounding.RoundQuantity(PluginEntry.DataModel,item.Quantity, item.SalesOrderUnitOfMeasure,storeCurrencyCode);
                    if (item.ShouldCalculateAndDisplayAssemblyPrice())
                    {
                        datarow["Price"] = rounding.RoundString(PluginEntry.DataModel, item.PriceWithTax, storeCurrencyCode, false);
                        datarow["TotalPrice"] = rounding.RoundString(PluginEntry.DataModel, item.GrossAmountWithTax, storeCurrencyCode, false);
                    }
                    TransactionDataset.Lines.Rows.Add(datarow);

                    //Show periodic discount if needed
                    if (item.PeriodicDiscountWithTax != 0)
                    {
                        datarow = TransactionDataset.ItemDiscount.NewRow();
                        datarow["LineID"] = lineID;
                        datarow["ItemID"] = item.ItemId.PadRight(13, '0');
                        datarow["DiscountAmount"] = rounding.RoundString(PluginEntry.DataModel, item.PeriodicDiscountWithTax, storeCurrencyCode, false);



                        //45851  //": Afsl�ttur "
                        datarow["DiscountText"] = item.PeriodicDiscountOfferName + ":" + Properties.Resources.Discount + " "
                            + rounding.RoundString(PluginEntry.DataModel, item.PeriodicDiscountWithTax, storeCurrencyCode, false) + " ("
                            + rounding.RoundString(PluginEntry.DataModel, item.PeriodicPctDiscount, storeCurrencyCode, false) + " %)"; //Discount "Afsl�ttur (" + ApplicationSettings.IRounding.Round(discItem.Percentage, 2, false);  
                        TransactionDataset.ItemDiscount.Rows.Add(datarow);
                    }

                    //Show line discount if needed.

                    if (item.LineDiscountWithTax != 0)
                    {
                        datarow = TransactionDataset.ItemDiscount.NewRow();
                        datarow["LineID"] = lineID;
                        datarow["ItemID"] = item.ItemId.PadRight(13, '0');
                        datarow["DiscountAmount"] = rounding.RoundString(PluginEntry.DataModel, item.LineDiscountWithTax, storeCurrencyCode, false);

                        //                            comment = (string)itemRow["Comment"];
                        //                            if (comment != "") { comment += "\r\n"; }

                        //45852  //"L�nuafsl�ttur: "
                        datarow["DiscountText"] = Properties.Resources.LineDiscount + ": "
                            + rounding.RoundString(PluginEntry.DataModel, item.LineDiscountWithTax, storeCurrencyCode, false) +
                            " (" + rounding.RoundString(PluginEntry.DataModel, item.LinePctDiscount, storeCurrencyCode, false) + " %)"; //Line discount
                        TransactionDataset.ItemDiscount.Rows.Add(datarow);
                    }

                    if (item.LoyaltyDiscountWithTax != 0)
                    {
                        datarow = TransactionDataset.ItemDiscount.NewRow();
                        datarow["LineID"] = lineID;
                        datarow["ItemID"] = item.ItemId.PadRight(13, '0');
                        datarow["DiscountAmount"] = rounding.RoundString(PluginEntry.DataModel, item.LoyaltyDiscountWithTax, storeCurrencyCode, false);

                        //45852  //"L�nuafsl�ttur: "
                        datarow["DiscountText"] = Properties.Resources.LoyaltyPointDiscount + ": "
                            + rounding.RoundString(PluginEntry.DataModel, item.LoyaltyDiscountWithTax, storeCurrencyCode, false) +
                            " (" + rounding.RoundString(PluginEntry.DataModel, item.LoyaltyPctDiscount, storeCurrencyCode, false) + " %)"; //loyalty discount
                        TransactionDataset.ItemDiscount.Rows.Add(datarow);
                    }

                    //Show total discount if needed
                    if (item.TotalDiscountWithTax != 0)
                    {
                        datarow = TransactionDataset.ItemDiscount.NewRow();
                        datarow["LineID"] = lineID;
                        datarow["ItemID"] = item.ItemId.PadRight(13, '0');
                        datarow["DiscountAmount"] = rounding.RoundString(PluginEntry.DataModel, item.TotalDiscountWithTax, storeCurrencyCode, false);

                        //45853  //"Heildarafsl�ttur: "
                        datarow["DiscountText"] = Properties.Resources.TotalDiscount + ": "
                            + rounding.RoundString(PluginEntry.DataModel, item.TotalDiscountWithTax, storeCurrencyCode, false) + " ("
                            + rounding.RoundString(PluginEntry.DataModel, item.TotalPctDiscount, storeCurrencyCode, false) + " %)"; //Total discount
                        TransactionDataset.ItemDiscount.Rows.Add(datarow);
                    }

                    //Show item comment if needed
                    if ((item.Comment != null) && (item.Comment != ""))
                    {
                        datarow = TransactionDataset.ItemComment.NewRow();
                        datarow["LineID"] = lineID;
                        datarow["ItemID"] = item.ItemId.PadRight(13, '0');
                        datarow["ItemComment"] = item.Comment;
                        TransactionDataset.ItemComment.Rows.Add(datarow);
                    }
                }
            }

            // Display markup item if available
            if (posTransaction.MarkupItem.Amount != decimal.Zero)
            {
                datarow = TransactionDataset.Lines.NewRow();

                datarow["TaxCode"] = "";
                datarow["ItemID"] = "";
                datarow["LineID"] = lineID + 1;
                datarow["ItemName"] = posTransaction.MarkupItem.Description;
                datarow["Price"] = rounding.RoundString(PluginEntry.DataModel, posTransaction.MarkupItem.Amount, storeCurrencyCode, false);
                datarow["Quantity"] = 1;
                datarow["TotalPrice"] = rounding.RoundString(PluginEntry.DataModel, posTransaction.MarkupItem.Amount, storeCurrencyCode, false);
                TransactionDataset.Lines.Rows.Add(datarow);
                

            }

            // Starting with the rounding difference if there is any...
            if (((RetailTransaction)posTransaction).RoundingDifference != 0M)
            {

                datarow = PaymentSubReport.PaymentDataset.Payments.NewRow();
                datarow["PaymentName"] = Properties.Resources.Rounding;
                datarow["ExtraInfo"] = "";
                datarow["PaymentAmount"] = rounding.RoundString(PluginEntry.DataModel, ((RetailTransaction)posTransaction).RoundingDifference, storeCurrencyCode, false);
                PaymentSubReport.PaymentDataset.Payments.Rows.Add(datarow);
            }


            foreach (TenderLineItem tenderItem in ((RetailTransaction)posTransaction).TenderLines.Where(w => !w.Voided))
            {
                datarow = PaymentSubReport.PaymentDataset.Payments.NewRow();
                datarow["PaymentName"] = tenderItem.Description;
                if (tenderItem.GetType() == typeof(TenderLineItem))
                {
                    if (tenderItem.ForeignCurrencyAmount > 0)
                        datarow["ExtraInfo"] = rounding.RoundString(PluginEntry.DataModel, tenderItem.ForeignCurrencyAmount, 2, false, tenderItem.CurrencyCode) + " " + tenderItem.CurrencyCode + " * " + rounding.RoundString(PluginEntry.DataModel, tenderItem.ExchangeRate/100, 5, false,tenderItem.CurrencyCode);
                    else
                        datarow["ExtraInfo"] = "";
                }
                else if (tenderItem.GetType() == typeof(CardTenderLineItem))
                {
                    CardTenderLineItem cardTender = (CardTenderLineItem)tenderItem;
                    string cardExpiryDate;
                    try
                    {
                        cardExpiryDate = cardTender.ExpiryDate.Insert(2, "/");
                    }
                    catch
                    {
                        cardExpiryDate = "";
                    }
                    if (cardTender.CardNumberHidden == true)
                    {
                        string cardEndNumber = cardTender.CardNumber.Substring(cardTender.CardNumber.Length - 6, 6);
                        cardEndNumber = cardEndNumber.Insert(2, "-");
                        datarow["ExtraInfo"] = "xx" + cardEndNumber;
                    }
                    else
                        datarow["ExtraInfo"] = cardTender.CardNumber;

                }
                else if (tenderItem.GetType() == typeof(CorporateCardTenderLineItem))
                {
                    CorporateCardTenderLineItem cardTender = (CorporateCardTenderLineItem)tenderItem;
                    string cardSubString = cardTender.CardNumber.Substring(cardTender.CardNumber.Length - 6, 6);
                    cardSubString = cardSubString.Insert(2, "-");
                    datarow["ExtraInfo"] = "xxxx-xxxx-xx" + cardSubString + " " + cardTender.ExpiryDate;
                }
                else if (tenderItem.GetType() == typeof(CustomerTenderLineItem))
                {
                    CustomerTenderLineItem customerTender = (CustomerTenderLineItem)tenderItem;
                    datarow["ExtraInfo"] = customerTender.CustomerId;

                }

                datarow["PaymentAmount"] = rounding.RoundString(PluginEntry.DataModel, tenderItem.Amount, storeCurrencyCode, false);
                PaymentSubReport.PaymentDataset.Payments.Rows.Add(datarow);
            }

            // Load data into tax lines

            bool hasTaxlines = false;
            foreach (TaxItem tax in ((RetailTransaction)posTransaction).TaxLines)
            {
                hasTaxlines = true;
                TaxCode taxCode = Providers.TaxCodeData.Get(PluginEntry.DataModel, tax.TaxCode);
                string taxCodeDescription = "";
                if (taxCode != null)
                {
                    if (taxCode.ReceiptDisplay != "")
                    {
                        taxCodeDescription = taxCode.ReceiptDisplay;
                    }
                    else
                    {
                        taxCodeDescription = taxCode.Text;
                    }
                }

                // Load data into taxlines
                datarow = TaxSubReport.TaxDataset.Tax.NewRow();
                datarow["TaxCode"] = taxCodeDescription; // Description put here as a temp fix until entire report gets an overhaul
                datarow["TaxPercentage"] = rounding.RoundString(PluginEntry.DataModel, tax.Percentage, storeCurrencyCode, false);
                datarow["TaxOf"] = rounding.RoundString(PluginEntry.DataModel, tax.PriceWithTax, storeCurrencyCode, false);
                datarow["TaxAmount"] = rounding.RoundString(PluginEntry.DataModel, tax.Amount, storeCurrencyCode, false);
                TaxSubReport.TaxDataset.Tax.Rows.Add(datarow);
            }

            if (!hasTaxlines)
            {
                TaxSubReport.Visible = false;
            }

        }

        private void DetailReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            System.Data.DataRowView row = (System.Data.DataRowView)this.GetCurrentRow();
            if (row != null)
            {
                System.Data.DataRow[] rows = this.memoryData1.Tables[4].Select("LineID=" + row[1]);
                if (rows.Length < 1)
                    e.Cancel = true;
            }
        }

        private void DetailReport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            System.Data.DataRowView row = (System.Data.DataRowView)this.GetCurrentRow();
            if (row != null)
            {
                System.Data.DataRow[] rows = this.memoryData1.Tables[5].Select("LineID=" + row[1]);
                if (rows.Length < 1)
                    e.Cancel = true;
            }
        }       
    }
}
