using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;
using LSOne.ViewPlugins.EndOfDay.Properties;
using LSOne.ViewPlugins.EndOfDay.Reports;
using LSOne.ViewPlugins.EndOfDay.Views;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.ViewCore.Interfaces;


namespace LSOne.ViewPlugins.EndOfDay
{
    internal class PluginOperations
    {
        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Resources.Retail, "Retail", null), 50);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Retail")
            {
                args.Add(new Item(Resources.EOD, "EOD", null), 500);
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Retail" && args.ItemKey == "EOD")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.RunEndOfDay))
                {
                    args.Add(new ItemButton(Resources.UnpostedStatements, Resources.UnpostedStatementsDescription, new EventHandler(ShowUnpostedStatements)), 10);
                    args.Add(new ItemButton(Resources.PostedStatements, Resources.PostedStatementsDescription, new EventHandler(ShowPostedStatements)), 20);
                }
                
                if(PluginEntry.DataModel.HasPermission(Permission.TenderDeclaration))
                {
                    args.Add(new ItemButton(Resources.TenderDeclaration, Resources.TenderDeclarationsDescription, new EventHandler(PerformTenderDeclaration)), 30);
                }
            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Resources.Retail, "Sales"), 300);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Sales")
            {
                args.Add(new PageCategory(Resources.EOD, "EOD"), 200);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Sales" && args.CategoryKey == "EOD")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.RunEndOfDay))
                {
                    args.Add(new CategoryItem(
                        Resources.UnpostedStatements,
                        Resources.UnpostedStatements,
                        Resources.UnpostedStatementsTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        null,
                        new EventHandler(ShowUnpostedStatements),
                        "UnpostedStatements"), 10);

                    args.Add(new CategoryItem(
                        Resources.PostedStatements,
                        Resources.PostedStatements,
                        Resources.PostedStatementsTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        null,
                        new EventHandler(ShowPostedStatements),
                        "PostedStatements"), 20);
                }

                if(PluginEntry.DataModel.HasPermission(Permission.TenderDeclaration))
                {
                    args.Add(new CategoryItem(
                        Resources.TenderDeclaration,
                        Resources.TenderDeclaration,
                        Resources.TenderDeclarationsTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        null,
                        new EventHandler(PerformTenderDeclaration),
                        "TenderDeclaration"), 30);
                }
            }
        }

        
        internal static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
           
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.RetailItems.Views.ItemView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.RunEndOfDay) && !arguments.View.MultiEditMode)
                {
                    arguments.Add(new ContextBarItem(
                        Resources.UnpostedStatements,
                        new ContextbarClickEventHandler(PluginOperations.ShowUnpostedStatements)), 500);
                }
            }
        }
        
        internal static void ShowUnpostedStatements(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new StatementsView(StatementTypeEnum.UnpostedStatements));          
        }

        internal static void ShowPostedStatements(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new StatementsView(StatementTypeEnum.PostedStatements));
        }

        internal static void ShowItemSalesReport(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ReportItemSalesView());
        }

        internal static void PerformTenderDeclaration(object sender, EventArgs args)
        {
            var dlg = new Dialogs.TenderdeclarationDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Providers.TenderDeclarationData.Save(PluginEntry.DataModel, dlg.Tenderdeclaration);

                foreach (var SCTDLine in dlg.Tenderdeclaration.TenderDeclarationLines)
                {
                    Providers.TenderDeclarationLineData.Save(PluginEntry.DataModel, SCTDLine);
                }
            }
        }

        internal static bool PostStatement(StatementInfo statement, RecordIdentifier storeID, DateTime postingDate)
        {
            var endOfDayService = (IEndOfDayBackOfficeService)PluginEntry.DataModel.Service(ServiceType.EndOfDayBackOfficeService);

            var statementFlags = endOfDayService.AllowedToPostStatement(PluginEntry.DataModel, statement);
            // Check if you are allowed to post this statement based on its difference

            if (statementFlags.Contains(AllowEODEnums.DisallowEodMarkMissingOnTerminal))
            {
                MessageDialog.Show(
                        Resources.EndOfDayTransactionMissingForTerminalQuestion,
                        Resources.EndOfDayTransactionMissingForTerminal,
                        MessageBoxIcon.Error);
                return false;
            }

            if (statementFlags.Contains(AllowEODEnums.DisallowCountingIncorrect))
            {
                if (QuestionDialog.Show(
                    Resources.StatementCountingDifferenceQuestion, 
                    Resources.StatementCountingDifference) == DialogResult.No)
                {
                    return false;
                }
            }

            if (statementFlags.Contains(AllowEODEnums.DisallowSuspendedTransaction))
            {
                if (QuestionDialog.Show(
                    Resources.SuspendedTransactionsQuestion,
                    Resources.SuspendedTransactions) == DialogResult.No)
                {
                    return false;
                }
            }

            endOfDayService.PostStatement(PluginEntry.DataModel, statement.ID, storeID, postingDate);
            
            if (PluginEntry.Framework.CanRunOperation("ProcessPostedStatement"))
            {
                PluginEntry.Framework.RunOperation("ProcessPostedStatement", null, new PluginOperationArguments(storeID, statement));    
            }

            return true;
        }

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.Store.Views.StoreView")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.RunEndOfDay))
                {
                    args.Add(new TabControl.Tab(Resources.StatementSettings,
                        "StoreStatementSettings",
                        ViewPages.StoreStatementSettingsPage.CreateInstance), 
                       300);
                }
                
                if(PluginEntry.DataModel.HasPermission(Permission.TenderDeclaration))
                {
                    args.Add(new TabControl.Tab(Resources.TenderDeclaration,
                        "TenderDeclaration",
                        ViewPages.StoreTenderDeclarationPage.CreateInstance),
                        500);
                }
            }

            if (args.ContextName == "LSOne.ViewPlugins.Terminals.Views.TerminalView")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.RunEndOfDay))
                {
                    args.Add(new TabControl.Tab(Resources.EndOfDay, ViewPages.TerminalEODSettingsPage.CreateInstance), 600);
                }
            }

            if (args.ContextName == "LSOne.ViewPlugins.Profiles.Views.FunctionalityProfileView")
            {
                args.Add(new TabControl.Tab(Resources.XZReport, ViewPages.FunctionalityProfileSettingsPage.CreateInstance), 100);
            }
        }

        public static DevExpress.XtraReports.UI.XtraReport GetFinancialReport(string storeId, DateTime fromDate, DateTime toDate, RecordIdentifier statementID, ReportIntervalType reportInterval, string headerText)
        {
            int numOfTransactions = 0;
            decimal numOfItemsSold = 0M;
            decimal amount = 0M;

            DecimalLimit decimalLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            CompanyInfo companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel, true);

            var reportData = DataProviderFactory.Instance.Get<DataLayer.IReportData, DataEntity>();
            List<HourlyDataLine> hourlyDataLines = reportData.GetHourlyDataLines(PluginEntry.DataModel, true, storeId, fromDate, toDate, statementID, reportInterval);
            List<CurrencyDataLine> currencyDataLines = reportData.GetCurrencyDataLines(PluginEntry.DataModel, true, storeId, fromDate, toDate, statementID, reportInterval);

            var taxGroupLinesForSubReport = Providers.FinancialReportData.GetTaxGroupLines(PluginEntry.DataModel, storeId, fromDate, toDate);
            var taxGroupSubReport = new ReportFinVAT(taxGroupLinesForSubReport, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

            ReportFin finReport = new ReportFin(companyInfo, hourlyDataLines, currencyDataLines, taxGroupSubReport);

            finReport.StoreID = storeId;

            // Header
            finReport.PeriodDefinition = headerText;

            //Hourly Data
            if (hourlyDataLines.Count < 1)
            {
                finReport.TotalHourlyCust = "0";
                finReport.TotalHourlyAmount = "0";
                finReport.TotalHourlyUnits = "0";
            }
            else
            {

                foreach (HourlyDataLine line in hourlyDataLines)
                {
                    numOfTransactions += line.NumberOfTransactions;
                    numOfItemsSold += line.NumberOfItemsSold;
                    amount += line.Amount;
                }

                finReport.TotalHourlyCust = numOfTransactions.ToString();
                finReport.TotalHourlyAmount = amount.FormatWithLimits(HourlyDataLine.PriceLimiter);
                finReport.TotalHourlyUnits = numOfItemsSold.ToString("n0");
            }

            #region Fetching currency data

            if (currencyDataLines.Count < 1)
            {
                finReport.totalCurrencyTransactions.Text = "0";
                finReport.totalCurrencyAmount.Text = "0";
            }
            else
            {
                int totalCurrencyTransactions = 0;
                foreach (CurrencyDataLine line in currencyDataLines)
                {
                    totalCurrencyTransactions += line.SalesNumbers;
                }

                finReport.totalCurrencyTransactions.Text = totalCurrencyTransactions.ToString();
            }

            #endregion

            #region Fetching sales data
            //has to be <= otherwise when searching for a date interval, it will not go into the function.
            //if (((Convert.ToInt32(cmbStatement.SelectedData.ID)) <= (Convert.ToInt32(cmbStatement.SelectedData.ID))) || (GetReportIntervalType() == ReportIntervalType.CurrentSaleOnly))
            //{
            DataLayer.DataEntities.Report report = reportData.GetReport(PluginEntry.DataModel, storeId, fromDate, toDate, statementID, reportInterval);

            //TODO: Decypher the names of this variables and give them proper names in english.
            finReport.Dagsala = report.DagsSala;

            // Heildarsummur
            finReport.DagsalaFjVi = report.DagsSalaAfgreidslur;
            finReport.DagsalaFjEin = report.DagsSalaEiningar;
            finReport.Dagsala = report.DagsSala;

            finReport.HeildInnstFjVi = report.HeildarInnstAfgreidslur;
            finReport.HeildInnstFjEin = report.HeildarInnstEiningar;
            finReport.HeildInnstimplun = report.HeildarInnst;

            //finReport.LagadFjVi = report.LagadAfgreidslur;
            //finReport.LagadFjEin = report.LagadEiningar;
            //finReport.Lagad = report.Lagad;

            finReport.HaettVidFjVid = report.HaettVidAfgreidslur;
            finReport.HaettVidFjEin = report.HaettVidEiningar;
            finReport.HaettVid = report.HaettVid;

            finReport.LinuAfslFjVid = report.LinuAfslFjVid;
            finReport.LinuAfsl = report.LinuAfsl;

            finReport.HeildAfslFjVid = report.HeildAfslFjVid;
            finReport.HeildAfsl = report.HeildAfsl;
            // }
            #endregion

            #region Fetching the payment table
            DataTable tenderTable = reportData.GetTenderTable(PluginEntry.DataModel, storeId, fromDate, toDate, statementID, reportInterval);
            if (tenderTable != null)
            {
                decimal totalPaymentAmount = 0;
                foreach (DataRow paymentRow in tenderTable.Rows)
                {
                    decimal tenderAmount = Conversion.ToDecimal(paymentRow[3]);

                    DataRow newrow = finReport.PaymentSubreport.PaymentDataset.Payments.NewRow();
                    string tenderId = (string)paymentRow[0];
                    newrow["TenderID"] = tenderId;
                    newrow["TenderText"] = paymentRow[1];
                    newrow["TenderUnits"] = paymentRow[2];
                    newrow["TenderAmount"] = tenderAmount.FormatWithLimits(decimalLimiter);
                    totalPaymentAmount += Convert.ToDecimal(paymentRow[3]);
                    finReport.PaymentSubreport.PaymentDataset.Payments.Rows.Add(newrow);

                    #region Fetching the payment detail table
                    DataTable tenderDetailTable = reportData.GetTenderDetailTable(PluginEntry.DataModel, tenderId, storeId, fromDate, toDate, statementID, reportInterval);
                    if (tenderDetailTable != null)
                    {
                        foreach (DataRow paymentDetailRow in tenderDetailTable.Rows)
                        {
                            decimal cardTypeAmount = Conversion.ToDecimal(paymentDetailRow[4]);
                            var cardTenderID = Conversion.ToStr(paymentDetailRow[1]);
                            if (string.IsNullOrEmpty(cardTenderID))
                                continue; // Skip where no card tender is is given

                            newrow = finReport.PaymentSubreport.PaymentDataset.CardTypes.NewRow();
                            newrow["TenderID"] = paymentDetailRow[0];
                            newrow["CardTenderID"] = cardTenderID;
                            newrow["CardTypeName"] = (string)paymentDetailRow[2];
                            newrow["CardTypeUnits"] = paymentDetailRow[3];
                            newrow["CardTypeAmount"] = cardTypeAmount.FormatWithLimits(decimalLimiter);
                            finReport.PaymentSubreport.PaymentDataset.CardTypes.Rows.Add(newrow);
                        }
                    }
                    #endregion
                }
                if (tenderTable.Rows.Count < 1)
                {
                    DataRow newrow = finReport.PaymentSubreport.PaymentDataset.Payments.NewRow();
                    newrow["TenderID"] = 1;
                    newrow["TenderText"] = Resources.NoPaymentInformation;
                    finReport.PaymentSubreport.PaymentDataset.Payments.Rows.Add(newrow);
                }
                finReport.PaymentSubreport.TotalPaymentAmount = totalPaymentAmount.FormatWithLimits(decimalLimiter);
            }


            #endregion

            #region Fetching statistic data
            decimal numberOfManual = reportData.GetNumberOfManuallyEnteredItems(PluginEntry.DataModel, storeId, fromDate, toDate, statementID, reportInterval);
            finReport.InnsleginnFjoldi = numberOfManual.ToString("n0");
            decimal numberOfScanned = reportData.GetNumberOfScannedItems(PluginEntry.DataModel, storeId, fromDate, toDate, statementID, reportInterval);
            decimal numberTotalItems = numberOfManual + numberOfScanned;  //this makes 100%


            finReport.ScanFjoldi = numberOfScanned.ToString("n0");
            if ((numberOfManual + numberOfScanned) != 0)
            {
                decimal innsleginPercent = (numberOfManual / numberTotalItems) * 100;
                finReport.InnsleginFjPercent = innsleginPercent.FormatWithLimits(decimalLimiter) + "%";
                decimal scannedPercent = (numberOfScanned / numberTotalItems) * 100;
                finReport.ScanFjPercent = scannedPercent.FormatWithLimits(decimalLimiter) + "%";
            }
            finReport.SkuffaOpnudAnSoluEin = reportData.GetNumberOfOpenedSales(PluginEntry.DataModel, storeId, fromDate, toDate, statementID, reportInterval).ToString("n0");
            finReport.FjoldiAfgrIMinusEin = reportData.GetNumOfNegative(PluginEntry.DataModel, storeId, fromDate, toDate, statementID, reportInterval).ToString("n0");
            finReport.FjoldiAfgrIMinus = reportData.GetSumOfNegative(PluginEntry.DataModel, storeId, fromDate, toDate, statementID, reportInterval).FormatWithLimits(decimalLimiter);
            finReport.FjoldiVidskVina = reportData.GetNumOfTransactions(PluginEntry.DataModel, storeId, fromDate, toDate, statementID, reportInterval).ToString("n0");
            finReport.FjoldiEininga = reportData.GetNumberOfItemsSold(PluginEntry.DataModel, storeId, fromDate, toDate, statementID, reportInterval).ToString("n0");
            #endregion

            return finReport;
        }
    }
}
