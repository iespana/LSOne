using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.EOD;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Enums;
using LSOne.DataLayer.TransactionObjects.EOD;
using LSOne.POS.Core;
using LSOne.Services.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Controls.SupportClasses;

namespace LSOne.Services
{
    public class ReportLogic
    {
        protected int lineWidth;
        protected int defaultPadding;
        protected string doubleLine;
        protected string singleLine;

        private ReportLogic()
            : this(40, 15)
        {
        }

        public ReportLogic(int lineWidth, int defaultPadding)
        {
            this.lineWidth = lineWidth;
            this.defaultPadding = defaultPadding;
            doubleLine = "".PadLeft(lineWidth, '=');
            singleLine = "".PadLeft(lineWidth, '-');
        }

        public EODInfo CalculateReport(IConnectionManager entry, PosTransaction transaction, ReportType repType)
        {
            EODInfo reportInfo = new EODInfo();
            reportInfo.CompanyInformation = Providers.CompanyInfoData.Get(entry, true);

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            reportInfo = TransactionProviders.EODInfoData.GetReportData(entry, reportInfo, transaction, repType, settings.Store.Currency);
            if (reportInfo.CancelReport)
            {
                if (Providers.TransactionData.ZReportExists(entry, reportInfo.CurrentZReport.ID))
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ZReportIDAlreadyExists, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }
                else
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoZReportID, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }
            }
            return reportInfo;
        }

        public string GetReportLayout(IConnectionManager entry, EODInfo reportInfo)
        {
            StringBuilder reportLayout = new StringBuilder();

            GetReportHeader(entry, reportInfo, ref reportLayout);
            GetZReportChanges(entry, reportInfo, ref reportLayout);
            GetCombinedSalesInfo(entry, reportInfo, ref reportLayout);

            if (reportInfo.PrintSalesReport())
            {
                GetSalesReport(entry, reportInfo, ref reportLayout);
                GetStatisticReport(entry, reportInfo, ref reportLayout);
                GetOtherInformation(entry, reportInfo, ref reportLayout);
                GetOverShortAmountInformation(entry, reportInfo, ref reportLayout);
                GetCustomerDepositInformation(entry, reportInfo, ref reportLayout);
                GetIncomeExpenseReport(entry, reportInfo, ref reportLayout);
                GetTenderReport(entry, reportInfo, ref reportLayout);
                GetRoundingReport(entry, reportInfo, ref reportLayout);
                GetCurrenciesReport(entry, reportInfo, ref reportLayout);
                GetVatReport(entry, reportInfo, ref reportLayout);
                GetNoSaleReport(entry, reportInfo, ref reportLayout);
                GetTenderDeclarationReport(entry, reportInfo, ref reportLayout);
                //GetChangeBackReport(reportInfo, ref reportLayout);
            }

            return reportLayout.ToString();
        }

        #region Get Income/Expense Report

        private void GetIncomeExpenseReport(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            if (reportInfo.IncomeExpenseAccountLines.Count == 0)
            {
                return;
            }

            //1       10        20        30        40        50   55
            //
            //=======================================================
            //Description                              Amount                            
            //--------------------------------------------------------
            //Window Washer                            2000
            int statCol1 = lineWidth - 15;  //Text
            const int statCol2 = 15;  //Amount/number

            reportLayout.AppendLine();
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.IncomeExpenseDescr, statCol1, PadDirection.Right));
            reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.Amount, statCol2, PadDirection.Left));
            reportLayout.AppendLine(singleLine);

            ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            foreach (IncomeExpenseAccount ie in reportInfo.IncomeExpenseAccountLines)
            {
                reportLayout.Append(StringFunctions.FormatString(ie.Name, statCol1, PadDirection.Right));
                reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, ie.Amount, statCol2));
            }

            reportLayout.AppendLine(singleLine);
        }

        #endregion

        #region Get Z Report Changes
        private void GetZReportChanges(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            //If not Z reports are in the report i.e. X report
            if (reportInfo.ZReports.Count == 0 || reportInfo.ReportType != ReportType.ZReport)
                return;

            //If no manually changed Z reports exist then no need to go through this
            if (reportInfo.ZReports.Count(c => c.EntryType != 0) == 0)
                return;

            reportLayout.AppendLine();
            //1       10        20        30        40        50   55
            //Combined sales report changes         Amount       
            //--------------------------------------------------------
            //Staff ID   Date      Gross amt      Net amt            

            
            const int dateCol = 10;
            const int grossAmtCol = 13;
            const int netAmtCol = 13;
            int staffIDCol = lineWidth - (dateCol + grossAmtCol + netAmtCol);

            reportLayout.AppendLine(Properties.Resources.TotalPOSSalesAmtChanges);
            reportLayout.AppendLine(doubleLine);

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.StaffID, staffIDCol));
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.Date, dateCol));
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.AmtInclTax, grossAmtCol, PadDirection.Left));
            reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.NetAmt, netAmtCol, PadDirection.Left));

            reportLayout.AppendLine(singleLine);

            IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            // Adding one line for each changed Z report            
            foreach (ZReport rpt in reportInfo.ZReports.Where(w => w.EntryType != 0).OrderBy(o => o.ID))
            {
                reportLayout.Append(StringFunctions.FormatString(rpt.Login, staffIDCol));
                reportLayout.Append(StringFunctions.FormatString(rpt.CreatedDate.ToShortDateString(), dateCol));
                reportLayout.Append(StringFunctions.FormatString(rounding.RoundForDisplay(entry, rpt.GrossAmount, false, false, settings.Store.Currency), grossAmtCol, PadDirection.Left));
                reportLayout.AppendLine(StringFunctions.FormatString(rounding.RoundForDisplay(entry, rpt.NetAmount, false, false, settings.Store.Currency), grossAmtCol, PadDirection.Left));
            }
            reportLayout.AppendLine(singleLine);
        }
        #endregion

        #region Get Combined Sales Info
        private void GetCombinedSalesInfo(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            //If no manually changed Z reports exist then no need to go through this
            if (reportInfo.ZReports.Count() == 0)
            {
                if (!reportInfo.ZReports.Any())
                {
                    return;
                }
            }

            reportLayout.AppendLine();

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!settings.FunctionalityProfile.ZReportConfig.PrintGrandTotals)
            {
                return;
            }


            //1       10        20        30        40        50   55
            //Combined sales report              Amount       
            //--------------------------------------------------------
            //

            int col1 = lineWidth - 20; //Text;
            int col2 = 20;  //Amount/number

            ZReport rpt = reportInfo.ZReports.LastOrDefault();
            if (rpt != null)
            {
                IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

                if (settings.FunctionalityProfile.ZReportConfig.CombineGrandTotalSalesandReturns)
                {
                    //Total sales incl tax:
                    if (settings.FunctionalityProfile.ZReportConfig.GrandTotalAmountDisplay == GrandTotalAmtDisplay.WithTax ||
                        settings.FunctionalityProfile.ZReportConfig.GrandTotalAmountDisplay == GrandTotalAmtDisplay.Both)
                    {
                        reportLayout.AppendLine(Properties.Resources.TotalPOSSalesAmt + " " + Properties.Resources.InclTax);
                        reportLayout.AppendLine(doubleLine);

                        reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalAmountAbbreviated, col1, PadDirection.Right));
                        reportLayout.AppendLine(StringFunctions.FormatString(rounding.RoundForDisplay(entry, rpt.TotalGrossAmount, false, false, settings.Store.Currency), col2, PadDirection.Left));
                    }

                    if (settings.FunctionalityProfile.ZReportConfig.GrandTotalAmountDisplay == GrandTotalAmtDisplay.Both)
                    {
                        reportLayout.AppendLine(singleLine);
                        reportLayout.AppendLine();
                    }

                    //Total sales without tax
                    if (settings.FunctionalityProfile.ZReportConfig.GrandTotalAmountDisplay == GrandTotalAmtDisplay.WithoutTax ||
                        settings.FunctionalityProfile.ZReportConfig.GrandTotalAmountDisplay == GrandTotalAmtDisplay.Both)
                    {

                        reportLayout.AppendLine(Properties.Resources.TotalPOSSalesAmt + " " + Properties.Resources.ExclTax);
                        reportLayout.AppendLine(doubleLine);

                        reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalAmountAbbreviated, col1, PadDirection.Right));
                        reportLayout.AppendLine(StringFunctions.FormatString(rounding.RoundForDisplay(entry, rpt.TotalNetAmount, false, false, settings.Store.Currency), col2, PadDirection.Left));
                    }
                }
                else
                {
                    //Total returns incl tax:
                    if (settings.FunctionalityProfile.ZReportConfig.GrandTotalAmountDisplay == GrandTotalAmtDisplay.WithTax ||
                        settings.FunctionalityProfile.ZReportConfig.GrandTotalAmountDisplay == GrandTotalAmtDisplay.Both)
                    {

                        reportLayout.AppendLine(Properties.Resources.TotalPOSSalesAmt + " " + Properties.Resources.InclTax);
                        reportLayout.AppendLine(doubleLine);

                        reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalSalesAmt, col1, PadDirection.Right));
                        reportLayout.AppendLine(StringFunctions.FormatString(rounding.RoundForDisplay(entry, rpt.TotalGrossAmount - rpt.TotalReturnGrossAmount, false, false, settings.Store.Currency), col2, PadDirection.Left));

                        reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalReturnAmount, col1, PadDirection.Right));
                        reportLayout.AppendLine(StringFunctions.FormatString(rounding.RoundForDisplay(entry, rpt.TotalReturnGrossAmount, false, false, settings.Store.Currency), col2, PadDirection.Left));

                        reportLayout.Append(StringFunctions.FormatString("", col1));
                        reportLayout.AppendLine(StringFunctions.FormatString("", +col2, PadDirection.Right, '='));

                        reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalAmountAbbreviated, col1, PadDirection.Right));
                        reportLayout.AppendLine(StringFunctions.FormatString(rounding.RoundForDisplay(entry, rpt.TotalGrossAmount, false, false, settings.Store.Currency), col2, PadDirection.Left));
                    }

                    if (settings.FunctionalityProfile.ZReportConfig.GrandTotalAmountDisplay == GrandTotalAmtDisplay.Both)
                    {
                        reportLayout.AppendLine(singleLine);
                        reportLayout.AppendLine();
                    }

                    //Total Net Amount:
                    if (settings.FunctionalityProfile.ZReportConfig.GrandTotalAmountDisplay == GrandTotalAmtDisplay.WithoutTax ||
                        settings.FunctionalityProfile.ZReportConfig.GrandTotalAmountDisplay == GrandTotalAmtDisplay.Both)
                    {
                        reportLayout.AppendLine(Properties.Resources.TotalPOSSalesAmt + " " + Properties.Resources.ExclTax);
                        reportLayout.AppendLine(doubleLine);

                        reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalSalesAmt, col1, PadDirection.Right));
                        reportLayout.AppendLine(StringFunctions.FormatString(rounding.RoundForDisplay(entry, rpt.TotalNetAmount - rpt.TotalReturnNetAmount, false, false, settings.Store.Currency), col2, PadDirection.Left));

                        reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalReturnAmount, col1, PadDirection.Right));
                        reportLayout.AppendLine(StringFunctions.FormatString(rounding.RoundForDisplay(entry, rpt.TotalReturnNetAmount, false, false, settings.Store.Currency), col2, PadDirection.Left));

                        reportLayout.Append(StringFunctions.FormatString("", col1));
                        reportLayout.AppendLine(StringFunctions.FormatString("", +col2, PadDirection.Right, '='));

                        reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalAmountAbbreviated, col1, PadDirection.Right));
                        reportLayout.AppendLine(StringFunctions.FormatString(rounding.RoundForDisplay(entry, rpt.TotalNetAmount, false, false, settings.Store.Currency), col2, PadDirection.Left));
                    }
                }
                
                reportLayout.AppendLine(doubleLine);
            }
        }
        #endregion

        #region Get Currencies Report

        private void GetCurrenciesReport(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            reportLayout.AppendLine();
            //1       10        20        30        40        50   55
            //Currency report                            Amount       
            //--------------------------------------------------------
            //British Pound             GBP                 2,342            

            if (reportInfo.CurrencyInfoLines.Count > 0)  //when no rows are there, then the section is omitted
            {
                int tenderCol1 = lineWidth - 30;
                const int tenderCol2 = 15;
                const int tenderCol3 = 15;

                reportLayout.AppendLine(Properties.Resources.CurrencyReport); //"Currency report:"
                reportLayout.AppendLine(doubleLine);
                
                reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.Amount, tenderCol1 + tenderCol2 + tenderCol3, PadDirection.Left));     //"Amount"

                reportLayout.AppendLine(singleLine);
                // Adding one line for each currency found
                foreach (CurrenciesInfo currencyInfo in reportInfo.CurrencyInfoLines)
                {
                    reportLayout.Append(StringFunctions.FormatString(currencyInfo.currencyName, tenderCol1));
                    reportLayout.Append(StringFunctions.FormatString(currencyInfo.currencyCode, tenderCol2));
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, currencyInfo.amountCur, tenderCol3));
                }
                reportLayout.AppendLine(StringFunctions.FormatString("", tenderCol1));  //inserts empty space into tenderCol1

                reportLayout.AppendLine(singleLine);
            }
        }

        #endregion

        #region Get Report Header

        private void GetReportHeader(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (reportInfo.ReportType == ReportType.XReport)
                reportLayout.AppendLine(Properties.Resources.XReport); 
            else if (reportInfo.ReportType == ReportType.ZReport)
                reportLayout.AppendLine(Properties.Resources.ZReport); 

            reportLayout.AppendLine(doubleLine);
            reportLayout.AppendLine();

            bool addExtraLine = false;
            if (reportInfo.CompanyInformation.Text != "")
            {
                reportLayout.AppendLine(reportInfo.CompanyInformation.Text);
                addExtraLine = true;
            }

            if (reportInfo.CompanyInformation.TaxNumber != "")
            {
                reportLayout.AppendLine(Properties.Resources.TaxNumber + " " + reportInfo.CompanyInformation.TaxNumber);
                addExtraLine = true;
            }

            if (addExtraLine)
            {
                reportLayout.AppendLine();
            }

            reportLayout.AppendLine(Properties.Resources.Operator.PadRight(defaultPadding, '.') + ": " + reportInfo.OperatorID);  
            if (!string.IsNullOrEmpty(reportInfo.BusinessDay) && settings.FunctionalityProfile.UseStartOfDay)
            {
                reportLayout.AppendLine(Properties.Resources.BusinessDay.PadRight(defaultPadding, '.') + ": " + reportInfo.BusinessDay);        //"   Business day.:" 
            }
            reportLayout.AppendLine(Properties.Resources.Date.PadRight(defaultPadding, '.') + ": " + reportInfo.Date);      
            reportLayout.AppendLine(Properties.Resources.Store.PadRight(defaultPadding, '.') + ": " + reportInfo.StoreID);     
            reportLayout.AppendLine(Properties.Resources.Time.PadRight(defaultPadding, '.') + ": " + reportInfo.Time);     

            if (reportInfo.ReportType == ReportType.ZReport)
            {
                reportLayout.AppendLine(Properties.Resources.ZReportID.PadRight(defaultPadding, '.') + ": " + reportInfo.CurrentZReport.ID.ToString());  //Z Report ID....:
            }

            reportLayout.AppendLine(Properties.Resources.Terminal.PadRight(defaultPadding, '.') + ": " + reportInfo.Terminal);  
            if (!string.IsNullOrEmpty(reportInfo.ExtraHeaderText))
            {
                reportLayout.AppendLine(reportInfo.ExtraHeaderText);
            }
            reportLayout.AppendLine(singleLine);
        }

        #endregion

        #region Get Sales Report

        private void GetCombinedSalesReport(IConnectionManager entry, ISettings settings, EODInfo reportInfo, ref StringBuilder reportLayout, bool withTax, int statCol1, int statCol2)
        {
            GetSalesReturnHeader(entry, settings, reportInfo, ref reportLayout, XZDisplayAmounts.Combined, statCol1, statCol2);

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.GrossSales, statCol1, PadDirection.Right));

            reportLayout.AppendLine(withTax
                ? StringFunctions.FormatString(entry, settings, reportInfo.GrossSalesInclTax, statCol2)
                : StringFunctions.FormatString(entry, settings, reportInfo.GrossSales, statCol2));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.CombinedReturns, statCol1, PadDirection.Right));

            reportLayout.AppendLine(withTax
                ? StringFunctions.FormatString(entry, settings, reportInfo.GrossReturnsInclTax, statCol2)
                : StringFunctions.FormatString(entry, settings, reportInfo.GrossReturns, statCol2));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.LineDiscount, statCol1, PadDirection.Right));

            reportLayout.AppendLine(withTax
                ? StringFunctions.FormatString(entry, settings, (reportInfo.LineDiscountAmountInclTax + reportInfo.PeriodicDiscountAmountInclTax) * -1, statCol2)
                : StringFunctions.FormatString(entry, settings, (reportInfo.LineDiscountAmount + reportInfo.PeriodicDiscountAmount) * -1, statCol2));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalDiscounts, statCol1, PadDirection.Right));

            reportLayout.AppendLine(withTax
                ? StringFunctions.FormatString(entry, settings, reportInfo.TotalDiscountAmountInclTax * -1, statCol2)
                : StringFunctions.FormatString(entry, settings, reportInfo.TotalDiscountAmount * -1, statCol2));

            reportLayout.Append(StringFunctions.FormatString("", statCol1));
            reportLayout.AppendLine(StringFunctions.FormatString("", +statCol2, PadDirection.Right, '='));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NetSales, statCol1, PadDirection.Right));      

            reportLayout.AppendLine(withTax
                ? StringFunctions.FormatString(entry, settings, reportInfo.TotalNetSaleInclTax, statCol2)
                : StringFunctions.FormatString(entry, settings, reportInfo.TotalNetSale, statCol2));

            // Add deposits to the print out if there are any deposits
            if (reportInfo.TotalDeposits != decimal.Zero || reportInfo.TotalRedeemedDepositsAmount != decimal.Zero)
            {

                if (reportInfo.TotalDeposits != decimal.Zero)
                {
                    reportLayout.Append(StringFunctions.FormatString(Properties.Resources.CustomerOrderDeposits, statCol1, PadDirection.Right));
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.TotalDeposits, statCol2));
                }

                if (reportInfo.TotalRedeemedDepositsAmount != decimal.Zero)
                {
                    reportLayout.Append(StringFunctions.FormatString(Properties.Resources.RedeemedDeposits, statCol1, PadDirection.Right));
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.TotalRedeemedDepositsAmount, statCol2));
                }

                reportLayout.Append(StringFunctions.FormatString("", statCol1));
                reportLayout.AppendLine(StringFunctions.FormatString("", +statCol2, PadDirection.Right, '='));

                reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalAmount, statCol1, PadDirection.Right));

                decimal totalAmount = (withTax ? reportInfo.TotalNetSaleInclTax : reportInfo.TotalNetSale) + reportInfo.TotalDeposits + reportInfo.TotalRedeemedDepositsAmount;
                reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, totalAmount, statCol2));
            }

            reportLayout.AppendLine(singleLine);
            reportLayout.AppendLine();
        }

        private void GetSalesReturnHeader(IConnectionManager entry, ISettings settings, EODInfo reportInfo, ref StringBuilder reportLayout, XZDisplayAmounts displayAmt, int statCol1, int statCol2)
        {
            string taxStr = settings.FunctionalityProfile.ZReportConfig.SalesReportAmountDisplay == SalesReportAmtdisplay.WithTax ? Properties.Resources.InclTax : Properties.Resources.ExclTax;

            if (displayAmt == XZDisplayAmounts.Combined || displayAmt == XZDisplayAmounts.OnlySale)
            {
                reportLayout.Append(StringFunctions.FormatString(Properties.Resources.SalesReport.Replace("#1", taxStr), statCol1, PadDirection.Right)); //"Sales report"
            }
            else if (displayAmt == XZDisplayAmounts.OnlyReturn)
            {
                reportLayout.Append(StringFunctions.FormatString(Properties.Resources.ReturnReport.Replace("#1", taxStr), statCol1, PadDirection.Right)); //"Sales report"
            }

            reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.Amount, statCol2, PadDirection.Left));
            reportLayout.AppendLine(doubleLine);
        }

        private void GetOnlySalesReturnReport(IConnectionManager entry, ISettings settings, EODInfo reportInfo, ref StringBuilder reportLayout, bool withTax, XZDisplayAmounts displayAmt, int statCol1, int statCol2)
        {
            if (displayAmt == XZDisplayAmounts.Combined)
            {
                return;
            }

            DiscountInfo discInfo = reportInfo.DiscountLines.FirstOrDefault(f => f.SalesReturns == displayAmt) ?? new DiscountInfo();

            GetSalesReturnHeader(entry, settings, reportInfo, ref reportLayout, displayAmt, statCol1, statCol2);

            if (displayAmt == XZDisplayAmounts.OnlySale)
            {
                reportLayout.Append(StringFunctions.FormatString(Properties.Resources.GrossSales, statCol1, PadDirection.Right));      //Description

                reportLayout.AppendLine(withTax
                    ? StringFunctions.FormatString(entry, settings, reportInfo.TotalNetAmountInclTax + discInfo.CombinedDiscountIncludingTax, statCol2)
                    : StringFunctions.FormatString(entry, settings, reportInfo.TotalNetAmount + discInfo.CombinedDiscount, statCol2));
            }
            else if (displayAmt == XZDisplayAmounts.OnlyReturn)
            {
                reportLayout.Append(StringFunctions.FormatString(Properties.Resources.GrossReturns, statCol1, PadDirection.Right));      //Description

                reportLayout.AppendLine(withTax
                    ? StringFunctions.FormatString(entry, settings, reportInfo.GrossReturnsInclTax - discInfo.CombinedDiscountIncludingTax, statCol2)
                    : StringFunctions.FormatString(entry, settings, reportInfo.GrossReturns - discInfo.CombinedDiscount, statCol2));
            }

            int negative = discInfo.SalesReturns == XZDisplayAmounts.OnlyReturn ? 1 : -1;

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.LineDiscount, statCol1, PadDirection.Right));

            reportLayout.AppendLine(withTax
                ? StringFunctions.FormatString(entry, settings, (discInfo.LineDiscountIncludingTax + discInfo.PeriodicDiscountIncludingTax) * negative, statCol2)
                : StringFunctions.FormatString(entry, settings, (discInfo.LineDiscount + discInfo.PeriodicDiscount) * negative, statCol2));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalDiscounts, statCol1, PadDirection.Right));

            reportLayout.AppendLine(withTax
                ? StringFunctions.FormatString(entry, settings, discInfo.TotalDiscountIncludingTax * negative, statCol2)
                : StringFunctions.FormatString(entry, settings, discInfo.TotalDiscount * negative, statCol2));

            reportLayout.Append(StringFunctions.FormatString("", statCol1));
            reportLayout.AppendLine(StringFunctions.FormatString("", +statCol2, PadDirection.Right, '='));

            if (displayAmt == XZDisplayAmounts.OnlySale)
            {
                reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NetSales, statCol1, PadDirection.Right));      //Description

                reportLayout.AppendLine(withTax
                    ? StringFunctions.FormatString(entry, settings, reportInfo.TotalNetAmountInclTax, statCol2)
                    : StringFunctions.FormatString(entry, settings, reportInfo.TotalNetAmount, statCol2));

                // Add deposits to the print out if there are any deposits
                if (reportInfo.TotalDeposits != decimal.Zero)
                {
                    reportLayout.Append(StringFunctions.FormatString(Properties.Resources.CustomerOrderDeposits, statCol1, PadDirection.Right));

                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.TotalDeposits, statCol2));

                    reportLayout.Append(StringFunctions.FormatString("", statCol1));
                    reportLayout.AppendLine(StringFunctions.FormatString("", +statCol2, PadDirection.Right, '='));

                    reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalAmount, statCol1, PadDirection.Right));

                    decimal totalAmount = (withTax ? reportInfo.TotalNetSaleInclTax : reportInfo.TotalNetSale) + reportInfo.TotalDeposits;
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, totalAmount, statCol2));
                }
            }
            else
            {
                reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NetReturns, statCol1, PadDirection.Right));      //Description

                reportLayout.AppendLine(withTax
                    ? StringFunctions.FormatString(entry, settings, reportInfo.TotalReturnNetAmountInclTax, statCol2)
                    : StringFunctions.FormatString(entry, settings, reportInfo.TotalReturnNetAmount, statCol2));
            }
        }

        private void GetSalesReport(IConnectionManager entry, ISettings settings, EODInfo reportInfo, ref StringBuilder reportLayout, bool withTax, XZDisplayAmounts displayAmts)
        {
            reportLayout.AppendLine();

            //1       10        20        30        40        50   55
            //Sales report (incl tax)
            //=======================================================
            //Gross sales                                    1.500,00
            //Line discount                                     25,00
            //Total discount                                   100,00
            //--------------------------------------------------------
            //Total net sales                                1.375,00
            //Customer order deposits                          135,00
            //--------------------------------------------------------
            //Total                                          1.510,00

            int statCol1 = lineWidth - 20;  //Text
            const int statCol2 = 20;  //Amount/number

            if (displayAmts == XZDisplayAmounts.Combined)
            {
                GetCombinedSalesReport(entry, settings, reportInfo, ref reportLayout, withTax, statCol1, statCol2);
            }

            else if (displayAmts == XZDisplayAmounts.OnlySale)
            {
                GetOnlySalesReturnReport(entry, settings, reportInfo, ref reportLayout, withTax, XZDisplayAmounts.OnlySale, statCol1, statCol2);
            }

            else if (displayAmts == XZDisplayAmounts.OnlyReturn)
            {
                GetOnlySalesReturnReport(entry, settings, reportInfo, ref reportLayout, withTax, XZDisplayAmounts.OnlyReturn, statCol1, statCol2);
            }
        }

        private void GetSalesReport(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (settings.FunctionalityProfile.ZReportConfig.CombineSaleAndReturnXZReport)
            {
                GetSalesReport(entry, settings, reportInfo, ref reportLayout, settings.FunctionalityProfile.ZReportConfig.SalesReportAmountDisplay == SalesReportAmtdisplay.WithTax, XZDisplayAmounts.Combined);
            }
            else
            {
                GetSalesReport(entry, settings, reportInfo, ref reportLayout, settings.FunctionalityProfile.ZReportConfig.SalesReportAmountDisplay == SalesReportAmtdisplay.WithTax, XZDisplayAmounts.OnlySale);
                GetSalesReport(entry, settings, reportInfo, ref reportLayout, settings.FunctionalityProfile.ZReportConfig.SalesReportAmountDisplay == SalesReportAmtdisplay.WithTax, XZDisplayAmounts.OnlyReturn);
            }
        }

        #endregion

        #region Get Statistic Report

        private void GetStatisticReport(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            reportLayout.AppendLine();
            //1       10        20        30        40        50   55
            //Statistics                                          Qty
            //=======================================================
            //Total no. of transactions                           137
            // ....

            int statCol1 = lineWidth - 20;  //Text
            const int statCol2 = 20;  //Amount/number

            //Header 
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.StatisticDescription, statCol1, PadDirection.Right));      //Description
            reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.Qty, statCol2, PadDirection.Left));  //Amt/Qty
            reportLayout.AppendLine(doubleLine);

            /*
                Statistics list begins
            */
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalNoOfTransactions, statCol1, PadDirection.Right));
            reportLayout.AppendLine(StringFunctions.FormatString(reportInfo.TotalNumberOfTransactions.ToString(), statCol2, PadDirection.Left));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NoOfSalesTransactions, statCol1, PadDirection.Right));
            reportLayout.AppendLine(StringFunctions.FormatString(reportInfo.NumberOfNormalTransactions.ToString(), statCol2, PadDirection.Left));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NoOfVoidedTransactions, statCol1, PadDirection.Right));
            reportLayout.AppendLine(StringFunctions.FormatString(reportInfo.NumberOfVoidTransactions.ToString(), statCol2, PadDirection.Left));

            if (settings.FunctionalityProfile.ZReportConfig.DisplayReturnInfo)
            {
                reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NoOfReturnedTransactions, statCol1, PadDirection.Right));
                reportLayout.AppendLine(StringFunctions.FormatString(reportInfo.NumberOfReturnTransactions.ToString(), statCol2, PadDirection.Left));

                reportLayout.Append(StringFunctions.FormatString(Properties.Resources.SumOfReturnedTransactions, statCol1, PadDirection.Right));
                reportLayout.AppendLine(StringFunctions.FormatString(StringFunctions.FormatString(entry, settings, reportInfo.TotalReturnNetAmountInclTax, statCol2), statCol2, PadDirection.Left));
            }

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NoOfDepositTransactions, statCol1, PadDirection.Right));
            reportLayout.AppendLine(StringFunctions.FormatString(reportInfo.NumberOfDepositTransactions.ToString(), statCol2, PadDirection.Left));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NoOfCustomerTransactions, statCol1, PadDirection.Right));
            reportLayout.AppendLine(StringFunctions.FormatString(reportInfo.NumberOfCustomerTransactions.ToString(), statCol2, PadDirection.Left));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NoOfItems, statCol1, PadDirection.Right));
            reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.NumberOfItemsSold, statCol2));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NoOfItemsReturned, statCol1, PadDirection.Right));
            reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.NumberOfItemsReturned, statCol2));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NoOfDrawerOpenings, statCol1, PadDirection.Right));
            reportLayout.AppendLine(StringFunctions.FormatString(reportInfo.NumberOfDrawerOpenings.ToString(), statCol2, PadDirection.Left));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NoOfLogins, statCol1, PadDirection.Right));
            reportLayout.AppendLine(StringFunctions.FormatString(reportInfo.NumberOfLogins.ToString(), statCol2, PadDirection.Left));

            if (settings.FunctionalityProfile.ZReportConfig.DisplaySuspendedInfo)
            {
                reportLayout.Append(StringFunctions.FormatString(Properties.Resources.NoOfSuspendedTransactions, statCol1, PadDirection.Right));
                reportLayout.AppendLine(StringFunctions.FormatString(GetSuspensionCount(entry, settings).ToString(), statCol2, PadDirection.Left));
            }

            reportLayout.AppendLine(singleLine);
        }

        #endregion

        #region Get tender declaration report
        private void GetTenderDeclarationReport(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            if (reportInfo.ReportType == ReportType.XReport || !settings.FunctionalityProfile.ZReportConfig.IncludeTenderDeclaration)
            {
                return;
            }

            reportLayout.AppendLine();

            int tenderCol1 = lineWidth - 15;
            const int tenderCol2 = 15;

            reportLayout.AppendLine(Properties.Resources.TenderDeclarationReport);
            reportLayout.AppendLine(doubleLine);
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TenderName, tenderCol1));
            reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.Amount, tenderCol2, PadDirection.Left));
            reportLayout.AppendLine(singleLine);

            foreach (TenderDeclarationLine tenderDeclarationLine in reportInfo.TenderDeclarationLines)
            {
                reportLayout.Append(StringFunctions.FormatString(tenderDeclarationLine.TenderName, tenderCol1));
                reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, tenderDeclarationLine.CountedAmount, tenderCol2));
            }

            reportLayout.AppendLine(singleLine);
        }
        #endregion

        #region Get change back report
        private void GetChangeBackReport(IConnectionManager entry, ISettings settings, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            reportLayout.AppendLine();

            int tenderCol1 = lineWidth - 15;
            const int tenderCol2 = 15;

            reportLayout.AppendLine(Properties.Resources.ChangeBackReport);
            reportLayout.AppendLine(doubleLine);
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TenderName, tenderCol1));
            reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.WTax, tenderCol2, PadDirection.Left));
            reportLayout.AppendLine(singleLine);

            // Adding one line for each tender type
            decimal total = 0;
            foreach (ChangeBackLine changeBackLine in reportInfo.ChangeBackLines)
            {
                reportLayout.Append(StringFunctions.FormatString(changeBackLine.TenderName, tenderCol1));
                reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, changeBackLine.Amount, tenderCol2));
                total += changeBackLine.Amount;
            }
            reportLayout.Append(StringFunctions.FormatString("", tenderCol1));  //inserts empty space into tenderCol1
            reportLayout.AppendLine(StringFunctions.FormatString("", tenderCol2, PadDirection.Right, '='));
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalAmount, tenderCol1, PadDirection.Left));
            reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, total, tenderCol2));
            reportLayout.AppendLine(singleLine);
        }
        #endregion

        #region Get VAT Report
        private void GetVatReport(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (settings.FunctionalityProfile.ZReportConfig.CombineSaleAndReturnXZReport)
            {
                GetVatReport(entry, settings, reportInfo, ref reportLayout, XZDisplayAmounts.Combined);
            }
            else
            {
                GetVatReport(entry, settings, reportInfo, ref reportLayout, XZDisplayAmounts.OnlySale);
                reportLayout.AppendLine();
                GetVatReport(entry, settings, reportInfo, ref reportLayout, XZDisplayAmounts.OnlyReturn);
            }
        }

        private void GetVatReport(IConnectionManager entry, ISettings settings, EODInfo reportInfo, ref StringBuilder reportLayout, XZDisplayAmounts displayAmts)
        {
            reportLayout.AppendLine();
            //1       10        20        30        40        50   55
            //Tax group      WO/Tax            Tax              W/Tax 
            //--------------------------------------------------------
            //25.00%       2,342.12         735,44           3,243,66   
            int vatCol1 = lineWidth -  35;  //VAT 
            const int vatCol2 = 12;  //WO/VAT
            const int vatCol3 = 10;  //VAT
            const int vatCol4 = 13;  //W/VAT

            if (displayAmts == XZDisplayAmounts.Combined)
            {
                reportLayout.AppendLine(Properties.Resources.TaxReport.Replace("#1", "")); 
            }
            else if (displayAmts == XZDisplayAmounts.OnlySale)
            {
                reportLayout.AppendLine(Properties.Resources.TaxReport.Replace("#1", Properties.Resources.Sales)); //"Tax report sales"
            }
            else if (displayAmts == XZDisplayAmounts.OnlyReturn)
            {
                reportLayout.AppendLine(Properties.Resources.TaxReport.Replace("#1", Properties.Resources.Returns)); //"Tax report return"
            }

            reportLayout.AppendLine(doubleLine);
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TaxGroup, vatCol1));     
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.WithoutTaxAbbreviated, vatCol2, PadDirection.Left));     
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.Tax, vatCol3, PadDirection.Left));      
            reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.WithTaxAbbreviation, vatCol4, PadDirection.Left));  

            reportLayout.AppendLine(singleLine);
            // Adding one line for each vat type
            decimal totalCol2 = 0;
            decimal totalCol3 = 0;
            decimal totalCol4 = 0;
            foreach (VatInfo vatinfo in reportInfo.VatInfoLines.Where(w => w.SaleReturn == displayAmts))
            {
                reportLayout.Append(StringFunctions.FormatString(vatinfo.vatGroupName, vatCol1));
                reportLayout.Append(StringFunctions.FormatString(entry, settings, vatinfo.netAmount, vatCol2));
                reportLayout.Append(StringFunctions.FormatString(entry, settings, vatinfo.vatAmount, vatCol3));
                reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, vatinfo.grossAmount, vatCol4));
                totalCol2 += vatinfo.netAmount;
                totalCol3 += vatinfo.vatAmount;
                totalCol4 += vatinfo.grossAmount;
            }
            reportLayout.Append(StringFunctions.FormatString("", vatCol1));
            reportLayout.AppendLine(StringFunctions.FormatString("", +vatCol2 + vatCol3 + vatCol4, PadDirection.Right, '='));
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalAmount, vatCol1, PadDirection.Right));
            reportLayout.Append(StringFunctions.FormatString(entry, settings, totalCol2, vatCol2));
            reportLayout.Append(StringFunctions.FormatString(entry, settings, totalCol3, vatCol3));
            reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, totalCol4, vatCol4));
            reportLayout.AppendLine(singleLine);
        }
        #endregion

        #region Get Tender Report
        private void GetTenderReport(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            reportLayout.AppendLine();
            //1       10        20        30        40        50   55
            //Tendername                            Amount       
            //--------------------------------------------------------
            //Foreign Currency                      2,342.12            


            int tenderCol1 = lineWidth - 15;
            const int tenderCol2 = 15;
            //const int tenderCol3 = 15;

            reportLayout.AppendLine(Properties.Resources.TenderReport); 
            reportLayout.AppendLine(doubleLine);

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TenderName, tenderCol1));    
            reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.TenderAmount, tenderCol2, PadDirection.Left));     //"Amount tendered"            

            reportLayout.AppendLine(singleLine);
            // Adding one line for each tender type
            decimal total = 0;
            foreach (TenderInfo tenderInfo in reportInfo.TenderInfoLines)
            {
                if (!settings.FunctionalityProfile.ZReportConfig.IncludeFloatInCashSummary && tenderInfo.operationID == POSOperations.PayCash)
                {
                    tenderInfo.amountTendered -= reportInfo.TotalFloatEntry + reportInfo.TotalTenderRemoval;
                }

                reportLayout.Append(StringFunctions.FormatString(tenderInfo.tenderName, tenderCol1));
                reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, tenderInfo.amountTendered, tenderCol2));

                foreach (TenderInfo subLine in tenderInfo.SubLines)
                {
                    reportLayout.Append(StringFunctions.FormatString(">" + subLine.tenderName, tenderCol1));
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, subLine.amountTendered, tenderCol2 - 1) + "<");
                }

                total += tenderInfo.amountTendered;
            }
            reportLayout.Append(StringFunctions.FormatString("", tenderCol1));  //inserts empty space into tenderCol1
            reportLayout.AppendLine(StringFunctions.FormatString("", tenderCol2, PadDirection.Right, '='));
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalAmount, tenderCol1, PadDirection.Left));
            reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, total, tenderCol2));
            reportLayout.AppendLine(singleLine);
        }
        #endregion

        #region Get rounding difference report
        private void GetRoundingReport(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (reportInfo.RoundingDifference == 0m)
                return;

            reportLayout.AppendLine();
            //1       10        20        30        40        50   55
            //Rounding difference                   Amount       
            //--------------------------------------------------------

            int tenderCol1 = lineWidth - 15;
            const int tenderCol2 = 15;
            //const int tenderCol3 = 15;

            reportLayout.AppendLine(Properties.Resources.RoundingReport);
            reportLayout.AppendLine(doubleLine);

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.RoundingDifference, tenderCol1));
            reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.RoundingDifference, tenderCol2));

            reportLayout.AppendLine(singleLine);
        }
        #endregion

        #region Get No Sale Report
        private void GetNoSaleReport(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            reportLayout.AppendLine();
            //1       10        20        30        40        50   55
            //NosaleText                                        Amount       
            //--------------------------------------------------------
            //Tender removal                                  -2,342.12            

            int noSaleCol1 = lineWidth - 15;
            const int noSaleCol2 = 15;

            if (reportInfo.TotalFloatEntry > 0 ||
                reportInfo.TotalTenderRemoval < 0 ||
                reportInfo.TotalSalesOrder > 0 ||
                reportInfo.TotalSalesInvoice > 0 ||
                reportInfo.TotalSafeDrop < 0 ||
                reportInfo.TotalSafeDropRev > 0 ||
                reportInfo.TotalBankDrop < 0 ||
                reportInfo.TotalBankDropRev > 0 ||
                reportInfo.StartAmountDeclaration > 0
                )
            {

                reportLayout.AppendLine(Properties.Resources.NoSaleReport); 
                reportLayout.AppendLine(doubleLine);

                reportLayout.Append(StringFunctions.FormatString(Properties.Resources.InOut, noSaleCol1, PadDirection.Right));    
                reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.Amount, noSaleCol2, PadDirection.Left));

                reportLayout.AppendLine(singleLine);
                // Adding one line for nosale text
                decimal total = 0;

                // Float entry
                if (reportInfo.TotalFloatEntry > 0)
                {
                    decimal justFloat = reportInfo.TotalFloatEntry - reportInfo.StartAmountDeclaration;
                    reportLayout.Append(StringFunctions.FormatString(Properties.Resources.FloatEntry, noSaleCol1, PadDirection.Right));
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, justFloat, noSaleCol2));
                    total += justFloat;
                }
                // Tender removal
                if (reportInfo.TotalTenderRemoval < 0)
                {
                    reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TenderRemoval, noSaleCol1, PadDirection.Right));
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.TotalTenderRemoval, noSaleCol2));
                    total += reportInfo.TotalTenderRemoval;
                }
                // Sales order
                if (reportInfo.TotalSalesOrder > 0)
                {
                    reportLayout.Append(StringFunctions.FormatString(Properties.Resources.SalesOrder, noSaleCol1, PadDirection.Right));
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.TotalSalesOrder, noSaleCol2));
                    total += reportInfo.TotalSalesOrder;
                }
                // Sales invoice
                if (reportInfo.TotalSalesInvoice > 0)
                {
                    reportLayout.Append(StringFunctions.FormatString(Properties.Resources.SalesInvoice, noSaleCol1, PadDirection.Right));
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.TotalSalesInvoice, noSaleCol2));
                    total += reportInfo.TotalSalesInvoice;
                }
                if (reportInfo.TotalSafeDrop < 0)
                {
                    reportLayout.Append(StringFunctions.FormatString(Properties.Resources.SafeDrop, noSaleCol1, PadDirection.Right));
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.TotalSafeDrop, noSaleCol2));
                    total += reportInfo.TotalSafeDrop;
                }
                if (reportInfo.TotalBankDrop < 0)
                {
                    reportLayout.Append(StringFunctions.FormatString(Properties.Resources.BankDrop, noSaleCol1, PadDirection.Right));
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.TotalBankDrop, noSaleCol2));
                    total += reportInfo.TotalBankDrop;
                }
                if (reportInfo.TotalSafeDropRev > 0)
                {
                    reportLayout.Append(StringFunctions.FormatString(Properties.Resources.SafeDropReversal, noSaleCol1, PadDirection.Right));
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.TotalSafeDropRev, noSaleCol2));
                    total += reportInfo.TotalSafeDropRev;
                }
                if (reportInfo.TotalBankDropRev > 0)
                {
                    reportLayout.Append(StringFunctions.FormatString(Properties.Resources.BankDropReversal, noSaleCol1, PadDirection.Right));
                    reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.TotalBankDropRev, noSaleCol2));
                    total += reportInfo.TotalBankDropRev;
                }


                // Total
                reportLayout.Append(StringFunctions.FormatString("", noSaleCol1));
                reportLayout.AppendLine(StringFunctions.FormatString("", noSaleCol2, PadDirection.Right, '='));
                reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalAmount, noSaleCol1, PadDirection.Left));
                reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, total, noSaleCol2));
                total += reportInfo.TotalSalesOrder;


                reportLayout.AppendLine(singleLine);
            }
        }
        #endregion

        #region Get other information
        private void GetOtherInformation(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!settings.FunctionalityProfile.ZReportConfig.DisplayOtherInfoSection)
            {
                return;
            }

            reportLayout.AppendLine();
            //1       10        20        30        40        50   55
            //Other information               
            //-------------------------------------------------------
            //-------------------------------------------------------
            //Description                     Qty              Amount       
            //-------------------------------------------------------
            //Receipt copies                   56            2.342,12            
            //Invoices                         12              778,00
            //Training transactions           118    1.000.033.893,47    

            int otherInfoCol1 = lineWidth - 25;
            const int otherInfoCol2 = 5;
            const int otherInfoCol3 = 20;


            reportLayout.AppendLine(Properties.Resources.OtherInformation);
            reportLayout.AppendLine(doubleLine);

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.Description, otherInfoCol1, PadDirection.Right));
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.Qty, otherInfoCol2, PadDirection.Left));
            reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.Amount, otherInfoCol3, PadDirection.Left));

            reportLayout.AppendLine(singleLine);

            OtherInfo noInfo = new OtherInfo();

            // Receipt copies
            OtherInfo info = reportInfo.OtherInfoLines.FirstOrDefault(f => f.InfoType == ZReportOtherInfoEnum.ReceiptCopy) ?? noInfo;

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.ReceiptCopies, otherInfoCol1, PadDirection.Right));
            reportLayout.Append(StringFunctions.FormatString(StringFunctions.FormatString(entry, settings, info.Qty, otherInfoCol2), otherInfoCol2, PadDirection.Left));
            reportLayout.AppendLine(StringFunctions.FormatString(StringFunctions.FormatString(entry, settings, info.AmountInclTax, otherInfoCol3), otherInfoCol3, PadDirection.Left));

            // Invoices
            info = reportInfo.OtherInfoLines.FirstOrDefault(f => f.InfoType == ZReportOtherInfoEnum.Invoice) ?? noInfo;

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.Invoices, otherInfoCol1, PadDirection.Right));
            reportLayout.Append(StringFunctions.FormatString(StringFunctions.FormatString(entry, settings, info.Qty, otherInfoCol2), otherInfoCol2, PadDirection.Left));
            reportLayout.AppendLine(StringFunctions.FormatString(StringFunctions.FormatString(entry, settings, info.AmountInclTax, otherInfoCol3), otherInfoCol3, PadDirection.Left));

            // Training transaction
            info = reportInfo.OtherInfoLines.FirstOrDefault(f => f.InfoType == ZReportOtherInfoEnum.Training) ?? noInfo;

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TrainingTransactions, otherInfoCol1, PadDirection.Right));
            reportLayout.Append(StringFunctions.FormatString(StringFunctions.FormatString(entry, settings, info.Qty, otherInfoCol2), otherInfoCol2, PadDirection.Left));
            reportLayout.AppendLine(StringFunctions.FormatString(StringFunctions.FormatString(entry, settings, info.AmountInclTax, otherInfoCol3), otherInfoCol3, PadDirection.Left));

            // Deposit transaction
            info = reportInfo.OtherInfoLines.FirstOrDefault(f => f.InfoType == ZReportOtherInfoEnum.Deposits) ?? noInfo;

            if (info.AmountInclTax != decimal.Zero)
            {
                reportLayout.Append(StringFunctions.FormatString(info.Text, otherInfoCol1, PadDirection.Right));
                reportLayout.Append(StringFunctions.FormatString(StringFunctions.FormatString(entry, settings, info.Qty, otherInfoCol2), otherInfoCol2, PadDirection.Left));
                reportLayout.AppendLine(StringFunctions.FormatString(StringFunctions.FormatString(entry, settings, info.AmountInclTax, otherInfoCol3), otherInfoCol3, PadDirection.Left));
            }

            reportLayout.AppendLine(singleLine);
        }
        #endregion

        #region Get over short amount information
        private void GetOverShortAmountInformation(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!settings.FunctionalityProfile.ZReportConfig.DisplayOverShortAmount || !settings.FunctionalityProfile.ZReportConfig.PrintGrandTotals)
            {
                return;
            }

            reportLayout.AppendLine();
            //1       10        20        30        40        50   55
            //Tender name                                      Amount
            //-------------------------------------------------------
            //Cash                                             137.00
            //=======================================================
            //Total amount                                     137.00
            //-------------------------------------------------------

            int statCol1 = lineWidth - 25;  //Text
            int statCol2 = 15;  //Amount/number
            int statCol3 = 10;

            //Header 
            reportLayout.AppendLine(Properties.Resources.OverShortReport);
            reportLayout.AppendLine(doubleLine);

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TenderName, statCol1, PadDirection.Right));  //Description
            reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.Amount, statCol2, PadDirection.Left));  //Amount
            reportLayout.AppendLine(singleLine);

            decimal total = 0;

            //Lines
            foreach(TenderInfo tenderInfo in reportInfo.OverShortTenderInfoLines)
            {
                reportLayout.Append(StringFunctions.FormatString(tenderInfo.tenderName, statCol1));
                reportLayout.Append(StringFunctions.FormatString(entry, settings, tenderInfo.amountTendered, statCol2));
                reportLayout.AppendLine(StringFunctions.FormatString(GetOverShortEvenString(tenderInfo.amountTendered), statCol3));

                total += tenderInfo.amountTendered;
            }

            //Total
            reportLayout.Append(StringFunctions.FormatString("", statCol1));
            reportLayout.Append(StringFunctions.FormatString("", statCol2, PadDirection.Right, '='));
            reportLayout.AppendLine(StringFunctions.FormatString("", statCol3, PadDirection.Right, '='));

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.TotalAmount, statCol1, PadDirection.Right));
            reportLayout.Append(StringFunctions.FormatString(entry, settings, total, statCol2, PadDirection.Left));
            reportLayout.AppendLine(StringFunctions.FormatString(GetOverShortEvenString(total), statCol3));

            reportLayout.AppendLine(singleLine);
        }

        private string GetOverShortEvenString(decimal value)
        {
            if(value > 0)
            {
                return string.Format("({0})", Properties.Resources.Over);
            }
            else if (value < 0)
            {
                return string.Format("({0})", Properties.Resources.Short);
            }
            else
            {
                return string.Format("({0})", Properties.Resources.Even);
            }
        }
        #endregion

        #region Get customer deposit information
        private void GetCustomerDepositInformation(IConnectionManager entry, EODInfo reportInfo, ref StringBuilder reportLayout)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!settings.FunctionalityProfile.ZReportConfig.DisplayDepositInfo)
            {
                return;
            }

            if (!reportInfo.CustomerDepositLines.Any())
            {
                return;
            }

            reportLayout.AppendLine();
            //1       10        20        30        40        50   55
            //Customer deposits               Qty             Amount   
            //-------------------------------------------------------
            //-------------------------------------------------------
            //0000000001 - Holly Flynn         2            2.342,12            
            //0000000002 - Brand and sons      3              778,00


            const int otherInfoCol2 = 12;
            const int otherInfoCol3 = 12;
            int otherInfoCol1 = lineWidth - (otherInfoCol2 + otherInfoCol3);


            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.CustomerDeposits, otherInfoCol1, PadDirection.Right));
            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.Qty, otherInfoCol2, PadDirection.Right));
            reportLayout.AppendLine(StringFunctions.FormatString(Properties.Resources.Amount, otherInfoCol3, PadDirection.Left));
            reportLayout.AppendLine(doubleLine);

            if (settings.FunctionalityProfile.ZReportConfig.ShowIndividualDeposits)
            {
                List<CustomerDepositLine> tempCustDepositLines = new List<CustomerDepositLine>();

                //Group together the deposits with the same account ID
                var groupedDepositList = reportInfo.CustomerDepositLines
                        .GroupBy(x => x.Account)
                        .Select(grp => grp.ToList())
                        .ToList();

                //Check quantity and total amount of deposits on same account
                foreach (var custDepositGroup in groupedDepositList)
                {
                    decimal total = 0;
                    int count = 0;
                    foreach (CustomerDepositLine custLine in custDepositGroup)
                    {
                        total += custLine.Amount;
                        count++;
                    }

                    tempCustDepositLines.Add(new CustomerDepositLine {
                        Account = custDepositGroup[0].Account,
                        Name = custDepositGroup[0].Name,
                        Amount = total,
                        Quantity = count
                    });
                }


                switch (settings.FunctionalityProfile.ZReportConfig.OrderByDepositInfo)
                {
                    case DepositOrderBy.Name:
                        if (entry.Settings.NameFormat == NameFormat.LastNameFirst)
                        {
                            tempCustDepositLines = new List<CustomerDepositLine>(tempCustDepositLines.OrderBy(o => o.Name.Last));
                        }
                        else
                        {
                            tempCustDepositLines = new List<CustomerDepositLine>(tempCustDepositLines.OrderBy(o => o.Name.First));
                        }

                        break;
                    case DepositOrderBy.Amount:
                        tempCustDepositLines = new List<CustomerDepositLine>(tempCustDepositLines.OrderBy(o => o.Amount));
                        break;
                }

                foreach(CustomerDepositLine custLine in tempCustDepositLines)
                {
                    reportLayout.Append(StringFunctions.FormatString((string)custLine.Account + " - " + entry.Settings.NameFormatter.Format(custLine.Name), otherInfoCol1, PadDirection.Right));
                    reportLayout.Append(StringFunctions.FormatString(custLine.Quantity.ToString(), otherInfoCol2, PadDirection.Right));
                    reportLayout.AppendLine(StringFunctions.FormatString(StringFunctions.FormatString(entry, settings, custLine.Amount, otherInfoCol3), otherInfoCol3, PadDirection.Left));
                }

                reportLayout.AppendLine(singleLine);
            }

            reportLayout.Append(StringFunctions.FormatString(Properties.Resources.Total, otherInfoCol1, PadDirection.Right));
            reportLayout.Append(StringFunctions.FormatString(reportInfo.CustomerDepositLines.Count.ToString(), otherInfoCol2, PadDirection.Right));  //inserts empty space into otherInfoCol2
            reportLayout.AppendLine(StringFunctions.FormatString(entry, settings, reportInfo.CustomerDepositLines.Sum(s => s.Amount), otherInfoCol3, PadDirection.Left));

            reportLayout.AppendLine(singleLine);
        }
        #endregion

        #region Suspension counts

        private int GetSuspensionCount(IConnectionManager entry, ISettings settings)
        {
            try
            {
                ICentralSuspensionService service = (ICentralSuspensionService)entry.Service(ServiceType.CentralSuspensionService);
                IConnectionManagerTemporary tempConnection = entry.CreateTemporaryConnection();
                int count = service.GetSuspendedTransactionCount(tempConnection, settings, true);
                tempConnection.Close();
                return count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

       

        #endregion
    }
}
