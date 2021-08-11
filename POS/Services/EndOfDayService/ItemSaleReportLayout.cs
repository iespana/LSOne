using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Enums;
using LSOne.DataLayer.TransactionObjects.EOD;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.Services
{
    public class ItemSaleReportLayout
    {
        protected int lineWidth;
        private int defaultPadding;
        private string doubleLine;
        private string singleLine;
        private bool salesPrinted;
        private decimal totalSalesQuantity;
        private decimal totalSalesAmount;
        private decimal totalReturnsQuantity;
        private decimal totalReturnsAmount;

        public ItemSaleReportLayout()
            : this(55, 30)
        {
        }


        public ItemSaleReportLayout(int lineWidth, int defaultPadding)
        {
            this.lineWidth = lineWidth;
            this.defaultPadding = defaultPadding;
            doubleLine = "".PadLeft(lineWidth, '=');
            singleLine = "".PadLeft(lineWidth, '-');

        }


        public string GetReportLayout(IConnectionManager entry, List<ItemSaleReportLine> reportInfoSales, List<ItemSaleReportLine> reportInfoReturns, IPosTransaction transaction, ItemSaleReportGroupEnum printGroup)
        {
            StringBuilder reportLayout = new StringBuilder();

            GetReportHeader(ref reportLayout, transaction);

            ReportLayout(entry, reportInfoSales, reportLayout, printGroup);
            ReportLayout(entry, reportInfoReturns, reportLayout, printGroup);

            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();

            return reportLayout.ToString();

        }


        private void ReportLayout(IConnectionManager entry, List<ItemSaleReportLine> reportInfo, StringBuilder reportLayout, ItemSaleReportGroupEnum printGroup)
        {
            
            //Item                            Qty              Amount       
            //-------------------------------------------------------
            //Coffee                            5               1.000
            //With milk
            //Milk                              3                 300            
            //Sugar                             2                 100
            //-------------------------------------------------------
            //TOTAL SALES:                     10               1.400
            //
            //
            //Coffee                          - 2               - 400
            //Milk                            - 1               - 100            
            //Sugar                           - 1                - 50
            //-------------------------------------------------------
            //TOTAL RETURNS:                  - 4               - 550
            //
            //=======================================================
            //GRAND TOTAL:                      6                 850

            const int otherInfoCol1 = 26;
            const int otherInfoCol2 = 8;
            const int otherInfoCol3 = 1;
            const int otherInfoCol4 = 7;
            const int otherInfoCol5 = 13;

            reportLayout.AppendLine();
            
            if (!salesPrinted)
            {
                reportLayout.Append(StringFunctions.FormatString(ItemSaleReportGroupHelper.ItemSaleReportGroupToString(printGroup), otherInfoCol1, PadDirection.Right));
                reportLayout.Append(StringFunctions.FormatString(Resources.Qty, otherInfoCol2, PadDirection.Left));
                reportLayout.Append(StringFunctions.FormatString("", otherInfoCol3, PadDirection.Right));
                reportLayout.Append(StringFunctions.FormatString("Unit", otherInfoCol4, PadDirection.Right));
                reportLayout.AppendLine(StringFunctions.FormatString(Resources.Amount, otherInfoCol5, PadDirection.Left));

                reportLayout.AppendLine(singleLine);                
            }

            ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            foreach (ItemSaleReportLine item in reportInfo)
            {
                reportLayout.Append(StringFunctions.FormatString(item.ItemDescription, otherInfoCol1, PadDirection.Right));
                reportLayout.Append(StringFunctions.FormatString(entry, settings, item.Quantity, otherInfoCol2, PadDirection.Left));
                reportLayout.Append(StringFunctions.FormatString("", otherInfoCol3, PadDirection.Left));
                reportLayout.Append(StringFunctions.FormatString(item.Unit, otherInfoCol4, PadDirection.Right));
                reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, item.Amount, otherInfoCol5, PadDirection.Left));
                if (!string.IsNullOrEmpty(item.VariantName))
                {
                    reportLayout.AppendLine(StringFunctions.FormatString("  " + item.VariantName, otherInfoCol1, PadDirection.Right));
                }
            }

            reportLayout.AppendLine(singleLine);

            if (!salesPrinted)
            {
                totalSalesQuantity = reportInfo.Sum(p => p.Quantity);
                totalSalesAmount = reportInfo.Sum(p => p.Amount);
            }
            else
            {
                totalReturnsQuantity = reportInfo.Sum(p => p.Quantity);
                totalReturnsAmount = reportInfo.Sum(p => p.Amount);
            }

            if (!salesPrinted)
            {
                reportLayout.Append(StringFunctions.FormatString(Resources.TotalSales, otherInfoCol1, PadDirection.Right));
                reportLayout.Append(StringFunctions.FormatString(entry, settings, totalSalesQuantity, otherInfoCol2, PadDirection.Left));
                reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, totalSalesAmount, otherInfoCol5 + otherInfoCol3 + otherInfoCol4, PadDirection.Left));
            }
            else
            {
                reportLayout.Append(StringFunctions.FormatString(Resources.TotalReturns, otherInfoCol1, PadDirection.Right));
                reportLayout.Append(StringFunctions.FormatString(entry, settings, totalReturnsQuantity, otherInfoCol2, PadDirection.Left));
                reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, totalReturnsAmount, otherInfoCol5 + otherInfoCol3 + otherInfoCol4, PadDirection.Left));
            }

            reportLayout.AppendLine();

            if (salesPrinted)
            {
                reportLayout.AppendLine(doubleLine);
                reportLayout.Append(StringFunctions.FormatString(Resources.GrandTotal, otherInfoCol1, PadDirection.Right));
                reportLayout.Append(StringFunctions.FormatString(entry, settings, totalReturnsQuantity + totalSalesQuantity, otherInfoCol2, PadDirection.Left));
                reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, totalReturnsAmount + totalSalesAmount, otherInfoCol5 + otherInfoCol3 + otherInfoCol4, PadDirection.Left));
            }
            
            if (!salesPrinted)
            {
                salesPrinted = true;
            }

        }


        private void GetReportHeader(ref StringBuilder reportLayout, IPosTransaction transaction)
        {

            //Item sale report    
            //=======================================================
            //Aurora Supermarket
            //
            //Store ID......................: S0001             
            //Terminal ID...................: P0001
            //Operator ID...................: 101
            //Date..........................: 11/8/2012
            //Printed on....................: 11/15/2012 11:05
            //

            reportLayout.AppendLine(Resources.ItemSalesReport);
            reportLayout.AppendLine(doubleLine);

            if (transaction.StoreName != "")
            {
                reportLayout.AppendLine(transaction.StoreName);
            }

            reportLayout.AppendLine();
            
            reportLayout.AppendLine(Resources.StoreID.PadRight(defaultPadding, '.') + ": " + transaction.StoreId);                          //"Store ID......:"
            reportLayout.AppendLine(Resources.TerminalID.PadRight(defaultPadding, '.') + ": " + transaction.TerminalId);                      //TerminalID
            reportLayout.AppendLine(Resources.OperatorID.PadRight(defaultPadding, '.') + ": " + transaction.Cashier.Login);                    //"Operator ID...:"
            reportLayout.AppendLine(Resources.Date.PadRight(defaultPadding, '.') + ": " + transaction.BeginDateTime.ToShortDateString());   //"   Date.:" 
            reportLayout.AppendLine(Resources.PrintedOn.PadRight(defaultPadding, '.') + ": " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());//"Printed on.:" 

            reportLayout.AppendLine();

        }

    }

}
