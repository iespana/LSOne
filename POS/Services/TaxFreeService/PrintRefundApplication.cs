using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportClasses.IDialog;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.Tools;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Text;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Controls.Dialogs.SelectionDialog;

namespace LSOne.Services
{
    public class PrintRefundApplication : IDisposable
    {
        private int currentPageIndex;
        private IList<Stream> streams;

        private Tourist tourist;
        private List<Transaction> transactions;
        private TaxRefund refund;

        private string selectedPrinter;

        public PrintRefundApplication(Tourist tourist, List<Transaction> transactions, TaxRefund refund)
        {
            this.tourist = tourist;
            this.transactions = transactions;
            this.refund = refund;
        }

        private LocalReport CreateMainReport(IConnectionManager entry, List<ISaleLineItem> items, int numberOfPages, decimal reminderFromOtherPages)
        {
            LocalReport report = GetReportFromAssembly("LSOne.Services.HM_Tax_refund.rdlc");

            CompanyInfo company = Providers.CompanyInfoData.Get(entry, false);

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            IRoundingService rounding = (IRoundingService) entry.Service(ServiceType.RoundingService);
            report.SetParameters(new ReportParameter("totalPayment", rounding.RoundForReceipt(entry, refund.Total, 2))); //TODO:resolve decimal places
            report.SetParameters(new ReportParameter("taxPayerIdentification", settings.Store.FormInfoField1.Replace("-","")));
            report.SetParameters(new ReportParameter("branchNo", settings.Store.FormInfoField4));
            report.SetParameters(new ReportParameter("nameOfRetailer", company.Text));
            report.SetParameters(new ReportParameter("retailerAddress", settings.Store.Address.Address1));
            report.SetParameters(new ReportParameter("retailerAddress2", settings.Store.Address.Address2));
            report.SetParameters(new ReportParameter("retailerPhone", settings.Store.FormInfoField2));
            report.SetParameters(new ReportParameter("totalTax", rounding.RoundForReceipt(entry, refund.Tax, 2)));//TODO:resolve decimal places
            report.SetParameters(new ReportParameter("totalTaxText","("+ DecimalToText.ChangeNumericToWords(refund.TaxRefundValue) + ")"));
            report.SetParameters(new ReportParameter("totalTaxRefundable", rounding.RoundForReceipt(entry, refund.TaxRefundValue, 2)));//TODO:resolve decimal places
            report.SetParameters(new ReportParameter("touristName", tourist.Name));
            report.SetParameters(new ReportParameter("touristNationality", tourist.Nationality));
            report.SetParameters(new ReportParameter("touristPassportNo", tourist.PassportNumber));
            report.SetParameters(new ReportParameter("touristPassportIssuedBy", tourist.PassportIssuedBy));
            report.SetParameters(new ReportParameter("touristPassportIssuedOn", tourist.PassportIssuedOn.ToShortDateString()));
            report.SetParameters(new ReportParameter("touristArrivalDate", tourist.ArrivalDate.ToShortDateString()));
            report.SetParameters(new ReportParameter("touristDepartureDate", tourist.DepartureDate.ToShortDateString()));
            report.SetParameters(new ReportParameter("touristDepartureFlight", tourist.FlightNumber));
            report.SetParameters(new ReportParameter("touristAddress1", tourist.Address.Address1));
            report.SetParameters(new ReportParameter("touristCity", tourist.Address.City));
            report.SetParameters(new ReportParameter("touristState", tourist.Address.State));
            report.SetParameters(new ReportParameter("touristZipCode", tourist.Address.Zip));
            report.SetParameters(new ReportParameter("touristCountry", tourist.Address.County));
            report.SetParameters(new ReportParameter("touristEmail", tourist.Email));
            report.SetParameters(new ReportParameter("numberOfPages", numberOfPages != 0 ? numberOfPages.ToString() : ""));
            report.SetParameters(new ReportParameter("currentDate", DateTime.Now.ToShortDateString()));
            report.SetParameters(new ReportParameter("remiderFromOtherPages", reminderFromOtherPages != 0M ? rounding.RoundForReceipt(entry, reminderFromOtherPages, 2) : ""));//todo:resolve decimal places
            for (int i = 1; i <= 12; i++)
            {
                if (i <= items.Count)
                {
                    report.SetParameters(new ReportParameter("TaxInvoiceNo" + i, (string)entry.CurrentTerminalID + "-" + items[i-1].Transaction.ReceiptId));
                    report.SetParameters(new ReportParameter("DescriptionNo" + i, items[i-1].Description));
                    report.SetParameters(new ReportParameter("QuantityNo" + i, rounding.RoundQuantity(entry, 
                                                                                                      items[i-1].Quantity, 
                                                                                                      items[i-1].SalesOrderUnitOfMeasure, 
                                                                                                      settings.Store.Currency, 
                                                                                                      CacheType.CacheTypeApplicationLifeTime)));
                    report.SetParameters(new ReportParameter("PriceNo" + i, rounding.RoundForReceipt(entry, items[i - 1].NetAmountWithTax, 2)));//todo:resolve decimal places
                }
                else
                {
                    report.SetParameters(new ReportParameter("TaxInvoiceNo" + i, ""));
                    report.SetParameters(new ReportParameter("DescriptionNo" + i, ""));
                    report.SetParameters(new ReportParameter("QuantityNo" + i, ""));
                    report.SetParameters(new ReportParameter("PriceNo" + i, ""));
                }
            }
            return report;
        }

        private LocalReport CreateAttachment(IConnectionManager entry, List<ISaleLineItem> items, int currentPage, int totalNumberOfPages, ref int itemNumber)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            LocalReport report = GetReportFromAssembly("LSOne.Services.HM_Tax_refund_attachment.rdlc");
            CompanyInfo company = Providers.CompanyInfoData.Get(entry, false);
            IRoundingService rounding = (IRoundingService) entry.Service(ServiceType.RoundingService);

            report.SetParameters(new ReportParameter("running", refund.Running));
            report.SetParameters(new ReportParameter("booking", refund.Booking));
            report.SetParameters(new ReportParameter("taxpayerIdentificationNo", settings.Store.FormInfoField1.Replace("-", "")));
            report.SetParameters(new ReportParameter("branchNo", settings.Store.FormInfoField4));
            report.SetParameters(new ReportParameter("retailerName", company.Text));
            report.SetParameters(new ReportParameter("pageNumber", totalNumberOfPages > 1 ? currentPage.ToString() : ""));
            report.SetParameters(new ReportParameter("totalPageNumber", totalNumberOfPages > 1 ? totalNumberOfPages.ToString() : ""));
            report.SetParameters(new ReportParameter("currentDate", DateTime.Now.ToShortDateString()));
            decimal subTotal = 0M;
            for (int i = 1; i <= 16; i++)
            {
                if (i <= items.Count)
                {
                    report.SetParameters(new ReportParameter("itemNo" + i, itemNumber.ToString()));
                    itemNumber++;
                    report.SetParameters(new ReportParameter("taxInvoiceNo" + i, (string)entry.CurrentTerminalID + "-" + items[i-1].Transaction.ReceiptId));
                    report.SetParameters(new ReportParameter("itemDescription" + i, items[i-1].Description));
                    report.SetParameters(new ReportParameter("quantity" + i, rounding.RoundQuantity(entry, 
                                                                                                      items[i-1].Quantity, 
                                                                                                      items[i-1].SalesOrderUnitOfMeasure, 
                                                                                                      settings.Store.Currency, 
                                                                                                      CacheType.CacheTypeApplicationLifeTime)));
                    report.SetParameters(new ReportParameter("itemPrice" + i, rounding.RoundForReceipt(entry, items[i - 1].NetAmountWithTax, 2)));//todo:resolve decimal places
                    subTotal += items[i - 1].NetAmountWithTax;
                }
                else
                {
                    report.SetParameters(new ReportParameter("itemNo" + i,""));
                    report.SetParameters(new ReportParameter("taxInvoiceNo" + i, ""));
                    report.SetParameters(new ReportParameter("itemDescription" + i, ""));
                    report.SetParameters(new ReportParameter("quantity" + i, ""));
                    report.SetParameters(new ReportParameter("itemPrice" + i, ""));
                }
            }
            report.SetParameters(new ReportParameter("subTotal", rounding.RoundForReceipt(entry, subTotal, 2)));//todo:resolve decimal places
            return report;
        }

        private LocalReport GetReportFromAssembly(string reportName)
        {
            LocalReport report = new LocalReport();
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(reportName);
            report.LoadReportDefinition(stream);
            return report;
        }

        private List<LocalReport> PopulateReports(IConnectionManager entry)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            List<LocalReport> reports = new List<LocalReport>();
            List<List<ISaleLineItem>> itemLists = new List<List<ISaleLineItem>>();
            int listIndex = 0;
            int itemIndex = 0;
            int itemCount = 0;
            decimal attachmentTotals = 0M;
            itemLists.Add(new List<ISaleLineItem>());
            foreach (Transaction transaction in transactions)
            {
                RetailTransaction retailTransaction = new RetailTransaction((string)transaction.StoreID, (string)transaction.CurrencyID, transaction.TaxIncludedInPrice);
                TransactionProviders.PosTransactionData.GetTransaction(entry, 
                                                                       transaction.ID, 
                                                                       transaction.StoreID,
                                                                       transaction.TerminalID,
                                                                       retailTransaction,
                                                                       settings.TaxIncludedInPrice);
                foreach (ISaleLineItem saleLineItem in retailTransaction.ISaleItems)
                {
                    if (saleLineItem.Voided || saleLineItem.ReceiptReturnItem)
                    {
                        continue;
                    }
                    if ((listIndex != 0 && itemIndex < 16) || itemIndex < 12)
                    {
                        itemLists[listIndex].Add(saleLineItem);
                        itemIndex++;
                    }
                    else
                    {
                        itemLists.Add(new List<ISaleLineItem>());
                        listIndex++;
                        itemLists[listIndex].Add(saleLineItem);
                        itemIndex = 1;
                    }
                    itemCount++;
                    if (itemCount > 12)
                    {
                        attachmentTotals += saleLineItem.NetAmountWithTax;
                    }
                }
            }

            reports.Add(CreateMainReport(entry, itemLists[0], itemLists.Count - 1, attachmentTotals));
            int itemNumber = 13;
            for (int i = 1; i < itemLists.Count; i++)
            {
                reports.Add(CreateAttachment(entry, itemLists[i], i, itemLists.Count - 1, ref itemNumber));
            }

            return reports;
        }

        private void Export(IConnectionManager entry, LocalReport report)
        {
            Setting topMargin = entry.Settings.GetSetting(entry, new Guid("67599422-85E1-4976-9161-689EFF3B78FB"), SettingType.Generic, "3.0cm");
            Setting leftMargin = entry.Settings.GetSetting(entry, new Guid("E5D8F523-F183-4DFE-8CFF-1DF561C07B6C"), SettingType.Generic, "0.8cm");
            string deviceInfo =
                @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>21cm</PageWidth>
                <PageHeight>29.7cm</PageHeight>
                <MarginTop>";
            deviceInfo += topMargin.SystemSetting;
            deviceInfo += "</MarginTop><MarginLeft>";
            deviceInfo += leftMargin.SystemSetting;
            deviceInfo += @"</MarginLeft>
                <MarginRight>0.0cm</MarginRight>
                <MarginBottom>0.0cm</MarginBottom>
            </DeviceInfo>";

            if (streams != null)
            {
                foreach (Stream stream in streams)
                {
                    stream.Close();
                }
            }
            Warning[] warnings;
            streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);

            foreach (Stream stream in streams)
            {
                stream.Position = 0;
            }
        }

        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            streams.Add(stream);
            return stream;
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new Metafile(streams[currentPageIndex]);

            ev.Graphics.DrawImage(pageImage, ev.PageBounds);

            currentPageIndex++;
        }

        private void print()
        {
            if (streams == null || streams.Count == 0)
                throw new Exception("Nothing to print.");

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = selectedPrinter;
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Invalid printer selection");
            }
            printDoc.PrintPage += PrintPage;
            currentPageIndex = 0;
            printDoc.Print();
        }

        public void Print(IConnectionManager entry)
        {
            List<LocalReport> reports = PopulateReports(entry);

            List<DataEntity> printers = new List<DataEntity>();
            string defaultPrinter = new PrinterSettings().PrinterName;
            DataEntity selected = null;
            foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
            {
                DataEntity entity = new DataEntity(installedPrinter, installedPrinter);
                if (installedPrinter == defaultPrinter)
                {
                    selected = entity;
                }
                printers.Add(entity);
            }

            using (SelectionDialog dlg = new SelectionDialog(new DataEntitySelectionList(printers, selected), Resources.SelectPrinter, true, true))
            {
                dlg.ShowDialog();
                selected = (DataEntity)dlg.SelectedItem;
            }

            selectedPrinter = selected != null ? selected.Text : "";

            IDialogService dialogService = (IDialogService)entry.Service(ServiceType.DialogService);

            for (int i = 0; i < reports.Count; i++)
            {
                Export(entry, reports[i]);
                print();
                if (reports.Count > i + 1)
                {
                    dialogService.ShowMessage(Resources.ClickOKWhenReady, Resources.PrepareTheNextPage);
                }
            }
        }

        public void Dispose()
        {
            if (streams != null)
            {
                foreach (Stream stream in streams)
                {
                    stream.Close();
                }
                streams = null;
            }
        }
    }
}
