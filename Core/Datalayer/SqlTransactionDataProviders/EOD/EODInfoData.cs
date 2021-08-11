using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.EOD;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.EOD;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders.EOD;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Enums;
using LSOne.DataLayer.TransactionObjects.EOD;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.DataLayer.SqlTransactionDataProviders.EOD
{
    public class EODInfoData : SqlServerDataProviderBase, IEODInfoData
    {
        public virtual EODInfo GetReportData(IConnectionManager entry, EODInfo reportInfo, IPosTransaction transaction, ReportType repType, RecordIdentifier currencyCode)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            // Collecting transaction data for the header
            reportInfo.ReportType = repType;
            if (transaction.BusinessDay.Year > 2000)
            {
                reportInfo.BusinessDay = transaction.BusinessDay.ToString("d");
            }
            reportInfo.Time = transaction.BeginDateTime.ToString("t");
            reportInfo.Date = transaction.BeginDateTime.ToString("d");
            reportInfo.OperatorID = (string) transaction.Cashier.ID;
            reportInfo.OperatorName = transaction.Cashier.Name;
            reportInfo.StoreID = transaction.StoreId;
            reportInfo.Terminal = transaction.TerminalId;
            reportInfo.DataAreaId = entry.Connection.DataAreaId;
            reportInfo.TenderDeclarationCalculation = settings.Store.TenderDeclarationCalculation;

            if (reportInfo.ReportType == ReportType.ZReport)
            {
                if (reportInfo.CurrentZReport.ID.IsEmpty)
                {
                    reportInfo.CurrentZReport.ID = DataProviderFactory.Instance.GenerateNumber<IZReportData, ZReport>(entry); 
                }
            }

            if (reportInfo.ReportType == ReportType.ZReport)
            {
                if (reportInfo.CurrentZReport.ID.IsEmpty)
                {
                    reportInfo.CurrentZReport.ID = DataProviderFactory.Instance.GenerateNumber<IZReportData, ZReport>(entry); 
                }
                if (reportInfo.CurrentZReport.ID.IsEmpty)
                {
                    reportInfo.CancelReport = true;
                    return reportInfo;
                }
                
                //Make sure that the next Z report ID doesn't already exist                    
                if (Providers.TransactionData.ZReportExists(entry, reportInfo.CurrentZReport.ID))
                {
                    reportInfo.CancelReport = true;
                    // Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.ZReportIDAlreadyExists, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return reportInfo;
                }
            }

            if (reportInfo.PrintSalesReport())
            {
                // Getting all the calculated values                
                reportInfo = ReadXZTotalsInfo(entry, reportInfo);
                // Collecting the VAT calculation
                if (settings.FunctionalityProfile.ZReportConfig.CombineSaleAndReturnXZReport)
                {
                    reportInfo = ReadXZVATInfo(entry, reportInfo, currencyCode, XZDisplayAmounts.Combined);
                }
                else
                {
                    reportInfo = ReadXZVATInfo(entry, reportInfo, currencyCode, XZDisplayAmounts.OnlyReturn);
                    reportInfo = ReadXZVATInfo(entry, reportInfo, currencyCode, XZDisplayAmounts.OnlySale);
                }

                if (settings.FunctionalityProfile.ZReportConfig.CombineSaleAndReturnXZReport)
                {
                    reportInfo = ReadXZDiscountInfo(entry, reportInfo, XZDisplayAmounts.Combined);
                }
                else
                {
                    reportInfo = ReadXZDiscountInfo(entry, reportInfo, XZDisplayAmounts.OnlyReturn);
                    reportInfo = ReadXZDiscountInfo(entry, reportInfo, XZDisplayAmounts.OnlySale);
                }

                // Collecting the Tenders calculation
                reportInfo = ReadXZTenderInfo(entry, reportInfo);

                reportInfo = ReadIncomeExpenseInfo(entry, reportInfo);

                reportInfo = ReadTrainingTransInfo(entry, reportInfo);

                reportInfo = ReadRedeemedDepositTransInfo(entry, reportInfo);

                reportInfo = ReadReprintCopiesInfo(entry, reportInfo);

                reportInfo = ReadXZCurrenciesInfo(entry, reportInfo);

                reportInfo = ReadZChangeBackInfo(entry, reportInfo);

                reportInfo = ReadTenderDeclarationInfo(entry, reportInfo);

                reportInfo = ReadCustomerDepositInfo(entry, reportInfo);

                reportInfo = ReadXZOverShortTenderInfo(entry, reportInfo);
            }

            return reportInfo;
        }

        

        private static EODInfo ReadIncomeExpenseInfo(IConnectionManager entry, EODInfo reportInfo)
        {
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT_INCOMEEXPENSE";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string)reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                
                var list = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateIncomeExpenseInfo);
                return list.Count > 0 ? list[0] : reportInfo;
            }
        }

        private static EODInfo PopulateIncomeExpenseInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {
            var incomeExpenseInfo = new IncomeExpenseAccount
                {
                    AccountNum = (string) dr["INCOMEEXEPENSEACCOUNT"],
                    Name = (string) dr["ACCOUNTNAME"],
                    AccountType = (IncomeExpenseAccount.AccountTypeEnum) ((int) dr["ACCOUNTTYPE"]),
                    Amount = (decimal) dr["AMOUNT"]
                };
            reportInfo.IncomeExpenseAccountLines.Add(incomeExpenseInfo);
            return reportInfo;
        }

        #region X/Z Currencies info        

        private static EODInfo ReadXZCurrenciesInfo(IConnectionManager entry, EODInfo reportInfo)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT_CURRENCIES";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string)reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                
                var list = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateCurrencyInfo);
                return list.Count > 0 ? list[0] : reportInfo;
            }
        }

        private static EODInfo PopulateCurrencyInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {
            var currenciesInfo = new CurrenciesInfo
                {
                    amountCur = (decimal) dr["AMOUNTCUR"],
                    currencyCode = (string) dr["CURRENCYCODE"],
                    currencyName = (string) dr["CURRENCYNAME"],
                    defaultTenderFunction = (int) dr["DEFAULTFUNCTION"]
                };
            reportInfo.CurrencyInfoLines.Add(currenciesInfo);
            return reportInfo;
        }

        #endregion

        #region X/Z report Totals Info        

        private static EODInfo ReadXZTotalsInfo(IConnectionManager entry, EODInfo reportInfo)
        {
            var settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "STORE", reportInfo.StoreID);
                MakeParam(cmd, "TERMINAL", reportInfo.Terminal);
                MakeParam(cmd, "STAFF", reportInfo.OperatorID);
                MakeParam(cmd, "ISZREPORT", reportInfo.ReportType == ReportType.ZReport ? 1 : 0, SqlDbType.TinyInt);
                MakeParam(cmd, "NEWZREPORTID", reportInfo.CurrentZReport.ID.IsEmpty ? "" : reportInfo.CurrentZReport.ID);                
                MakeParam(cmd, "COMBINESALESANDRETURN", settings.FunctionalityProfile.ZReportConfig.CombineGrandTotalSalesandReturns ? 1 : 0, SqlDbType.TinyInt);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                
                var list = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateTotalsInfo);
                return list.Count > 0 ? list[0] : reportInfo;
            }                           
        }

        private static EODInfo PopulateTotalsInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {
            reportInfo.NumberOfNormalTransactions = (int) dr["NORMALTRANSCOUNT"];
            reportInfo.NumberOfReturnTransactions = (int) dr["RETURNTRANSCOUNT"];
            reportInfo.NumberOfVoidTransactions = (int) dr["VOIDEDTRANSCOUNT"];
            reportInfo.NumberOfDepositTransactions = (int)dr["DEPOSITTRANSCOUNT"];
            reportInfo.TotalNetAmountInclTax = (decimal) dr["TOTALNETAMOUNTINCLTAX"];
            reportInfo.TotalFloatEntry = (decimal) dr["TotalFloatEntry"];
            reportInfo.TotalTenderRemoval = (decimal) dr["TotalTenderRemoval"];
            reportInfo.TotalSalesOrder = (decimal) dr["TotalSalesOrder"];
            reportInfo.TotalSalesInvoice = (decimal) dr["TotalSalesInvoice"];
            reportInfo.TotalBankDrop = (decimal) dr["TotalBankDrop"];
            reportInfo.TotalSafeDrop = (decimal) dr["TotalSafeDrop"];
            reportInfo.TotalBankDropRev = (decimal) dr["TotalBankDropReversal"];
            reportInfo.TotalSafeDropRev = (decimal) dr["TotalSafeDropReversal"];
            reportInfo.TotalDeposits = (decimal)dr["TOTALDEPOSITS"];
            reportInfo.TotalRedeemedDepositsAmount = (decimal)dr["TOTALREDEEMEDDEPOSITSAMOUNT"];

            reportInfo.NumberOfItemsSold = (decimal) dr["TOTALITEMSSOLD"];
            reportInfo.NumberOfItemsReturned = (decimal) dr["TOTALRETURNEDITEMS"];
            reportInfo.NumberOfDrawerOpenings = (int)(decimal) dr["TOTALDRAWEROPENINGS"];
            reportInfo.NumberOfOpenDrawerOperations = (int)(decimal) dr["TOTALOPENDRAWEROPERATIONS"];
            reportInfo.TotalNetAmount = (decimal) dr["TOTALNETAMOUNT"];
            reportInfo.NumberOfLogins = (int)(decimal) dr["TOTALLOGINS"];
            reportInfo.TotalNumberOfTransactions = (int) dr["TOTALALLTRANSACTIONS"];

            reportInfo.CompleteDiscountAmount = (decimal) dr["ALLDISCAMTS"];
            reportInfo.TotalDiscountAmount = (decimal) dr["TOTALDISCAMT"];
            reportInfo.PeriodicDiscountAmount = (decimal) dr["PERIODICDISCAMT"];
            reportInfo.LineDiscountAmount = (decimal) dr["LINEDISCAMT"];

            reportInfo.CompleteDiscountAmountInclTax = (decimal) dr["ALLDISCAMTSINCLTAX"];
            reportInfo.TotalDiscountAmountInclTax = (decimal) dr["TOTALDISCAMTINCLTAX"];
            reportInfo.PeriodicDiscountAmountInclTax = (decimal) dr["PERIODICDISCAMTINCLTAX"];
            reportInfo.LineDiscountAmountInclTax = (decimal) dr["LINEDISCAMTINCLTAX"];

            reportInfo.NumberOfCustomerTransactions = (int)(decimal) dr["CUSTOMERTRANSCOUNT"];

            reportInfo.TotalReturnNetAmountInclTax = (decimal)dr["TOTALRETURNGROSSAMOUNTINCLTAX"];
            reportInfo.TotalReturnNetAmount = (decimal)dr["TOTALRETURNNETAMOUNT"];

            reportInfo.StartAmountDeclaration = (decimal)dr["DECLARESTARTAMOUNT"];

            return reportInfo;
        }

        #endregion

        #region Reprint information

        private static EODInfo ReadReprintCopiesInfo(IConnectionManager entry, EODInfo reportInfo)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT_REPRINTS";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string)reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                var list = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateReprintsInfo);
                return list.Count > 0 ? list[0] : reportInfo;
            }
        }

        private static EODInfo PopulateReprintsInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {
            var reprintInfo = new OtherInfo();

            reprintInfo.AmountInclTax = (decimal)dr["SUM_GROSSAMOUNT"];
            reprintInfo.Qty = (int)dr["NUMBEROFTRANSACTIONS"];
            reprintInfo.Amount = (decimal)dr["SUM_NETAMOUNT"];
            reprintInfo.InfoType = (ZReportOtherInfoEnum)(int)dr["REPRINTTYPE"];

            reportInfo.OtherInfoLines.Add(reprintInfo);
            return reportInfo;
        }

        #endregion

        #region Training transaction information

        private static EODInfo ReadTrainingTransInfo(IConnectionManager entry, EODInfo reportInfo)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT_TRAINING";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string)reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                var list = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateTrainingInfo);
                return list.Count > 0 ? list[0] : reportInfo;
            }
        }

        private static EODInfo PopulateTrainingInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {
            var reprintInfo = new OtherInfo();

            reprintInfo.AmountInclTax = (decimal)dr["SUM_GROSSAMOUNT"];
            reprintInfo.Qty = (int)dr["NUMBEROFTRANSACTIONS"];
            reprintInfo.Amount = (decimal)dr["SUM_NETAMOUNT"];
            reprintInfo.InfoType = ZReportOtherInfoEnum.Training;

            reportInfo.OtherInfoLines.Add(reprintInfo);
            return reportInfo;
        }

        #endregion

        #region Deposit transaction information

        private static EODInfo ReadRedeemedDepositTransInfo(IConnectionManager entry, EODInfo reportInfo)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT_REDEEMEDDEPOSITS";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string)reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                var list = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateDepositInfo);
                return list.Count > 0 ? list[0] : reportInfo;
            }
        }

        private static EODInfo PopulateDepositInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {
            var depositInfo = new OtherInfo();

            depositInfo.Text = (string) dr["TENDERNAME"];
            depositInfo.AmountInclTax = (decimal)dr["AMOUNT"];
            depositInfo.Qty = (decimal)dr["QTY"];
            depositInfo.InfoType = ZReportOtherInfoEnum.Deposits;

            reportInfo.OtherInfoLines.Add(depositInfo);
            return reportInfo;
        }

        #endregion

        #region Discount information

        private static EODInfo ReadXZDiscountInfo(IConnectionManager entry, EODInfo reportInfo, XZDisplayAmounts displayAmts)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT_DISCOUNTS";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string)reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "DISPLAYAMT", (int)displayAmts);

                var list = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateDiscountInfo);

                if (list.Count > 0)
                {
                    return list[0];
                }

                return reportInfo;
            }
        }

        private static EODInfo PopulateDiscountInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {
            var discInfo = new DiscountInfo();

            discInfo.SalesReturns = (XZDisplayAmounts)(int)dr["DISPLAYAMT"];

            int negative = discInfo.SalesReturns == XZDisplayAmounts.OnlyReturn ? -1 : 1;

            discInfo.CombinedDiscountIncludingTax = (decimal)dr["ALL_DISCOUNTS_INCLTAX"] * negative;
            discInfo.TotalDiscountIncludingTax = (decimal)dr["TOTAL_DISCOUNTS_INCLTAX"] * negative;
            discInfo.LineDiscountIncludingTax = (decimal)dr["LINE_DISCOUNTS_INCLTAX"] * negative;
            discInfo.PeriodicDiscountIncludingTax = (decimal)dr["PERIODIC_DISCOUNTS_INCLTAX"] * negative;
            discInfo.CombinedDiscount = (decimal)dr["ALL_DISCOUNTS"] * negative;
            discInfo.TotalDiscount = (decimal)dr["TOTAL_DISCOUNTS"] * negative;
            discInfo.LineDiscount = (decimal)dr["LINE_DISCOUNTS"] * negative;
            discInfo.PeriodicDiscount = (decimal)dr["PERIODIC_DISCOUNTS"] * negative;

            reportInfo.DiscountLines.Add(discInfo);
            return reportInfo;
        }

        #endregion

        #region X/Z report VAT info        

        private static EODInfo ReadXZVATInfo(IConnectionManager entry, EODInfo reportInfo, RecordIdentifier currencyCode, XZDisplayAmounts displayAmts)
        {            
            var rounding = (IRoundingService) entry.Service(ServiceType.RoundingService);
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT_VAT";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string) reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "DISPLAYAMT", (int)displayAmts);
              
                var list = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateVATInfo);
                if (list.Count > 0)
                {
                    if (list[0].VatInfoLines.Last().vatAmount > 0 && list[0].VatInfoLines.Last().netAmount > 0)
                    {
                        list[0].VatInfoLines.Last().vatValue =
                            rounding.Round(entry,
                                           ((list[0].VatInfoLines.Last().vatAmount/list[0].VatInfoLines.Last().netAmount)*100),
                                           1,
                                           currencyCode,
                                           CacheType.CacheTypeTransactionLifeTime);
                    }
                    else
                    {
                        list[0].VatInfoLines.Last().vatValue = 0;
                    }
                    return list[0];
                }
                return reportInfo;
            }
        }

        private static EODInfo PopulateVATInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {      
            var vatInfo = new VatInfo
                {
                    vatType = (string) dr["TAXGROUP"],
                    vatGroupName = (string) dr["TAXGROUPNAME"],
                    netAmount = (decimal) dr["NETAMOUNT"],
                    vatAmount = (decimal) dr["TAXAMOUNT"],
                    grossAmount = (decimal)dr["TOTALAMOUNT"],
                    SaleReturn = (XZDisplayAmounts)(int)dr["DISPLAYAMT"]
                };
            reportInfo.VatInfoLines.Add(vatInfo);
            return reportInfo;
        }

        #endregion

        #region X/Z report Tender info        

        private static EODInfo ReadXZTenderInfo(IConnectionManager entry, EODInfo reportInfo)
        {
            EODInfo result;
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT_TENDERS";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string) reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                
                var list = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateTenderInfo);
                result = list.Count > 0 ? list[0] : reportInfo;
            }

            foreach (var tenderInfoLine in result.TenderInfoLines)
            {
                // See what the LSR_ZREPORT_TENDER_DETAILS sp is doing. I hope I can use it as is and fetch the data I need
                using (var cmd = entry.Connection.CreateCommand())
                {
                    cmd.CommandText = @"LSR_ZREPORT_TENDER_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string)reportInfo.CurrentZReport.ID : "");
                    MakeParam(cmd, "TENDERID", tenderInfoLine.tenderID);
                    MakeParam(cmd, "STOREID", reportInfo.StoreID);
                    MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                    MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                    TenderInfo subLines = new TenderInfo();
                    var list = Execute(entry, cmd, CommandType.StoredProcedure, subLines, PopulateCardSubLines);
                    tenderInfoLine.SubLines = list;
                }
            }

            // Get the total rounding difference
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT SUM(SALESPAYMENTDIFFERENCE) FROM RBOTRANSACTIONTABLE 
                            WHERE 
                            ZREPORTID=@ZREPORTID 
                            AND STORE=@STOREID
                            AND TERMINAL=@TERMINALID
                            AND DATAAREAID=@DATAAREAID";

                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string)reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                object res = entry.Connection.ExecuteScalar(cmd);

                if (res != DBNull.Value)
                {
                    result.RoundingDifference = Convert.ToDecimal(res);
                }
            }

            return result;
        }

        private static EODInfo ReadXZOverShortTenderInfo(IConnectionManager entry, EODInfo reportInfo)
        {
            EODInfo result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT_OVERSHORT";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string)reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                var list = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateOverShortTenderInfo);
                result = list.Count > 0 ? list[0] : reportInfo;
            }

            return result;
        }

        private static EODInfo PopulateTenderInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {
            var tenderInfo = new TenderInfo
                {
                    amountTendered = (decimal) dr["AmountTendered"],
                    tenderID = (string) dr["Tender"],
                    tenderName = (string) dr["TenderName"],
                    operationID = (POSOperations)(int)dr["POSOPERATION"]
                };
            reportInfo.TenderInfoLines.Add(tenderInfo);

            return reportInfo;
        }

        private static EODInfo PopulateOverShortTenderInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {
            var tenderInfo = new TenderInfo
            {
                amountTendered = (decimal)dr["AMOUNT"],
                tenderID = (string)dr["TENDER"],
                tenderName = (string)dr["TENDERNAME"],
                operationID = (POSOperations)(int)dr["POSOPERATION"]
            };
            reportInfo.OverShortTenderInfoLines.Add(tenderInfo);

            return reportInfo;
        }

        private static TenderInfo PopulateCardSubLines(IConnectionManager entry, IDataReader dr, TenderInfo subLines)
        {
            var tenderInfo = new TenderInfo
            {
                amountTendered = (decimal)dr["AmountTendered"],
                tenderID = (string)dr["CARDTYPEID"],
                tenderName = (string)dr["CARDNAME"]
            };

            return tenderInfo;
        }

        #endregion

        #region X/Z report Change back info
        private static EODInfo ReadZChangeBackInfo(IConnectionManager entry, EODInfo reportInfo)
        {
            EODInfo result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT_CHANGEBACKS";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string)reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                var reportInfoFromDb = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateChangeBackInfo);
                result = reportInfoFromDb.Count > 0 ? reportInfoFromDb[0] : reportInfo;
            }

            return result;


        }

        private static EODInfo PopulateChangeBackInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {
            var tenderInfo = new ChangeBackLine()
            {
                Amount = (decimal)dr["CHANGEBACKAMOUNT"],
                TenderId = (string)dr["TENDERID"],
                TenderName= (string)dr["TENDERNAME"]
            };
            reportInfo.ChangeBackLines.Add(tenderInfo);

            return reportInfo;
        }

        #endregion
        
        #region Tender Declaration info

        private static EODInfo ReadTenderDeclarationInfo(IConnectionManager entry, EODInfo reportInfo)
        {
            EODInfo result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT_TENDERDECLARATION";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string)reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "TENDERDECLARATIONCALCULATION", (int)reportInfo.TenderDeclarationCalculation, SqlDbType.Bit);

                var reportInfoFromDb = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateTenderDeclarationInfo);
                result = reportInfoFromDb.Count > 0 ? reportInfoFromDb[0] : reportInfo;
            }

            return result;
        }

        private static EODInfo PopulateTenderDeclarationInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {
            var tenderInfo = new TenderDeclarationLine()
            {
                CountedAmount = (decimal)dr["COUNTEDAMOUNT"],
                TenderId = (string)dr["TENDERID"],
                TenderName = (string)dr["TENDERNAME"]
            };

            reportInfo.TenderDeclarationLines.Add(tenderInfo);
            return reportInfo;
        }

        #endregion

        #region X/Z Customer deposit info

        private static EODInfo ReadCustomerDepositInfo(IConnectionManager entry, EODInfo reportInfo)
        {
            EODInfo result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"LSR_ZREPORT_CUSTDEPOSITS";
                cmd.CommandType = CommandType.StoredProcedure;
                MakeParam(cmd, "ZREPORTID", reportInfo.ReportType == ReportType.ZReport ? (string)reportInfo.CurrentZReport.ID : "");
                MakeParam(cmd, "STOREID", reportInfo.StoreID);
                MakeParam(cmd, "TERMINALID", reportInfo.Terminal);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                var reportInfoFromDb = Execute(entry, cmd, CommandType.StoredProcedure, reportInfo, PopulateCustomerDepositInfo);
                result = reportInfoFromDb.Count > 0 ? reportInfoFromDb[0] : reportInfo;
            }

            return result;
        }

        private static EODInfo PopulateCustomerDepositInfo(IConnectionManager entry, IDataReader dr, EODInfo reportInfo)
        {
            var custInfo = new CustomerDepositLine()
            {
                Amount = (decimal)dr["AMOUNTTENDERED"],
                Account = (string)dr["CUSTACCOUNT"],
                Name = new Name((string)dr["NAMEPREFIX"], (string)dr["FIRSTNAME"], (string)dr["MIDDLENAME"], (string)dr["LASTNAME"], (string)dr["NAMESUFFIX"])
            };

            reportInfo.CustomerDepositLines.Add(custInfo);
            return reportInfo;
        }

        #endregion

    }
}

