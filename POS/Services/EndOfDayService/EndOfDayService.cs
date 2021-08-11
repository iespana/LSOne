using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using LSOne.Controls.Dialogs;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.EOD;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Enums;
using LSOne.DataLayer.TransactionObjects.EOD;
using LSOne.Peripherals;
using LSOne.POS.Core.Exceptions;
using LSOne.POS.Processes.Common;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using LSRetailPosis;

namespace LSOne.Services
{
    public partial class EndOfDayService : IEndOfDayService
    {
              
        #region IEOD Members

        public virtual void EndOfDay(IConnectionManager entry, IEndOfDayTransaction transaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Performing End of Day...", "EOD.EndOfDay");
                // Show the "Are you sure?" dialog
                DialogResult dialogResult = DialogResult.Cancel;
  
                // Are you sure you want to perform an End of Day operation?
                dialogResult = Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.EndOfDayOperationQuestion, MessageBoxButtons.YesNo, MessageDialogType.Attention);

                if (dialogResult == DialogResult.Yes)
                {
                    PartnerEndOfDay(entry, transaction);   
                    // Perform extra EOD functionality here...
                    // Such as a tender declaration or incrementing the batch id at the EFT broker...
                    ApplicationServices.ShutDownEFT();
                }
                else
                {
                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "End of Day operation cancelled", "EOD.EndOfDay");
                    transaction.EntryStatus = TransactionStatus.Cancelled;
                    return;
                }

            }
            catch (POSException px)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                throw px;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public virtual void EndOfShift(IConnectionManager entry, IEndOfShiftTransaction transaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Performing End of Shift...", "EOD.EndOfShift");

                ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                // Show the "Are you sure?" dialog
                System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.DialogResult.Cancel;
                if (settings.VisualProfile.TerminalType == VisualProfile.HardwareTypes.Touch)
                {
                    // Touch screen hardware

                    // Are you sure you want to perform an End of Shift operation?
                    dialogResult = Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.EndOfShiftQuestion, System.Windows.Forms.MessageBoxButtons.YesNo, MessageDialogType.Attention);
                }
                else
                {
                    // Keyboard hardware
                }

                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    // Perform extra EOS functionality here...
                    // Such as a tender declaration or incrementing the batch id at the EFT broker...

                    // Create a new transaction of the type LogOnOffTransaction
                    LogOnOffTransaction logOnOffTrans = new LogOnOffTransaction();
                    // Call LoadTransactionStatus to load the new transaction with basic values
                    Interfaces.Services.TransactionService(entry).LoadTransactionStatus(entry,logOnOffTrans);
                    // To specify that the we are working with a "LogOff" transaction, not "Logon"
                    logOnOffTrans.Logon = false;

                    // Run the core logic to logoff
                    Login loginOp = new Login();
                    loginOp.Logout(logOnOffTrans);

                    // Create an instance of the TransactionData class and call Save passing the transaction object in as a parameter

                    TransactionProviders.PosTransactionData.Save(entry, settings, logOnOffTrans);
                }
                else
                {

                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "End of Shift operation cancelled", "EOD.EndOfShift");

                    transaction.EntryStatus = TransactionStatus.Cancelled;
                    return;
                }

            }
            catch (POSException px)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                throw px;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public virtual void PrintItemSaleReport(IConnectionManager entry, IPosTransaction transaction, ItemSaleReportGroupEnum printGroup)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Printing item sales report...", "EOD.PrintItemSaleReport");
                ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                if (settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.None)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoPrinterConfiguredNoXRpt, MessageBoxButtons.OK, MessageDialogType.Generic);
                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "No Printer - operation cancelled.", "EOD.PrintItemSaleReport");

                    transaction.EntryStatus = TransactionStatus.Cancelled;
                    return;
                }

                List<ItemSaleReportLine> reportInfoSales = new List<ItemSaleReportLine>();
                List<ItemSaleReportLine> reportInfoReturns = new List<ItemSaleReportLine>();

                reportInfoSales = TransactionProviders.ItemReportInfoData.GetItemSaleReportLines(entry, transaction.BeginDateTime, transaction.EndDateTime, transaction.StoreId, transaction.TerminalId, SalesOrReturnsEnum.Sales, printGroup);
                reportInfoReturns = TransactionProviders.ItemReportInfoData.GetItemSaleReportLines(entry, transaction.BeginDateTime, transaction.EndDateTime, transaction.StoreId, transaction.TerminalId, SalesOrReturnsEnum.Returns, printGroup);
                
                ItemSaleReportLayout reportLayout = new ItemSaleReportLayout();

                string printString = reportLayout.GetReportLayout(entry, reportInfoSales, reportInfoReturns, transaction, printGroup);
                Printer.PrintReceipt(entry,printString, null);
                
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public virtual void PrintXReport(IConnectionManager entry, IPosTransaction transaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Printing X report...", "EOD.PrintXReport");
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                if (settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.None)
                {
                    if (!settings.SuppressUI)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoPrinterConfiguredNoXRpt, MessageBoxButtons.OK, MessageDialogType.Generic);
                    }

                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "No Printer - operation cancelled.", "EOD.PrintXReport");

                    transaction.EntryStatus = TransactionStatus.Cancelled;
                    return;
                }

                string printString = GetXReportPrintString(entry, transaction);
                Printer.PrintReceipt(entry, printString, null);

                var fiscalService = (IFiscalService)entry.Service(ServiceType.FiscalService);
                if (fiscalService != null && fiscalService.IsActive())
                {
                    fiscalService.SaveFiscalLog(entry, printString.Trim(), POSOperations.PrintX);
                }

                PrintEFTReport(entry, ReportType.XReport);
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public virtual string GetXReportPrintString(IConnectionManager entry, IPosTransaction transaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            EODInfo reportInfo = new EODInfo();

            reportInfo.CompanyInformation = Providers.CompanyInfoData.Get(entry, true);
            reportInfo = TransactionProviders.EODInfoData.GetReportData(entry, reportInfo, transaction, ReportType.XReport, ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.Currency);
            ReportLogic reportLogic = GetReportLogic(entry) ?? new ReportLogic(settings.FunctionalityProfile.ZReportConfig.ReportWidth, settings.FunctionalityProfile.ZReportConfig.DefaultPadding);

            GetExtraHeaderText(reportInfo);
            string printString = reportLogic.GetReportLayout(entry, reportInfo);
            printString = printString + "\r\n\r\n\r\n\r\n";
            return printString;
        }

        public virtual bool GetZReportPrintString(IConnectionManager entry, IPosTransaction transaction, ZReport zReport, out string printString)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            EODInfo reportInfo = new EODInfo();
            printString = "";

            reportInfo.CompanyInformation = Providers.CompanyInfoData.Get(entry, true);
            if (zReport != null)
            {
                reportInfo.CurrentZReport = zReport;
            }

            if (ValidateZReports(entry, reportInfo))
            {
                reportInfo.CompanyInformation = Providers.CompanyInfoData.Get(entry, true);
                reportInfo = TransactionProviders.EODInfoData.GetReportData(entry, reportInfo, transaction, ReportType.ZReport, settings.Store.Currency);
                if (reportInfo.CancelReport)
                {

                    if (Providers.TransactionData.ZReportExists(entry, reportInfo.CurrentZReport.ID))
                    {
                        //Return the ID created
                        Providers.NumberSequenceData.ReturnNumberToSequence(entry, Providers.ZReportData.SequenceID, reportInfo.CurrentZReport.ID);

                        if (!settings.SuppressUI)
                        {
                            Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ZReportIDAlreadyExists, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        }
                    }
                    else if (!settings.SuppressUI)
                    {

                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoZReportID, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }

                    return false;
                }

                //Update the Z report list
                reportInfo.ZReports = Providers.ZReportData.GetList(entry, entry.CurrentStoreID, entry.CurrentTerminalID);
                ReportLogic reportLogic = GetReportLogic(entry) ?? new ReportLogic(settings.FunctionalityProfile.ZReportConfig.ReportWidth, settings.FunctionalityProfile.ZReportConfig.DefaultPadding);
                GetExtraHeaderText(reportInfo);
                printString = reportLogic.GetReportLayout(entry, reportInfo);
                printString = printString + "\r\n\r\n\r\n\r\n";
                return true;
            }

            return false;
        }

        private void PrintZReport(IConnectionManager entry, IPosTransaction transaction, ZReport zReport, bool displayConfirmation)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Printing Z report...", "EOD.PrintZReport");

                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                if (settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.None)
                {
                    if (!settings.SuppressUI)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoPrinterConfiguredNoZRpt, MessageBoxButtons.OK, MessageDialogType.Generic);
                    }

                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "No Printer - operation cancelled.", "EOD.PrintZReport");

                    transaction.EntryStatus = TransactionStatus.Cancelled;
                    return;
                }

                bool printReport = true;
                if (displayConfirmation && !settings.SuppressUI)
                {
                    DialogResult dlgResult = Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ConfirmZReportPrinting, MessageBoxButtons.YesNo, MessageDialogType.Attention);
                    printReport = (dlgResult == DialogResult.Yes);
                }

                if (!printReport)
                {
                    return;
                }

                string printString = "";

                if(GetZReportPrintString(entry, transaction, zReport, out printString))
                {
                    Printer.PrintReceipt(entry, printString, null);

                    var fiscalService = (IFiscalService)entry.Service(ServiceType.FiscalService);
                    if (fiscalService != null && fiscalService.IsActive())
                    {
                        fiscalService.SaveFiscalLog(entry, printString.Trim(), POSOperations.PrintZ);
                    }

                    PrintEFTReport(entry, ReportType.ZReport);
                }
            }
            catch (POSException px)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                throw px;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public virtual void PrintZReport(IConnectionManager entry, IPosTransaction transaction, bool displayConfirmation)
        {
            PrintZReport(entry, transaction, null, displayConfirmation);
        }

        private bool ValidateZReports(IConnectionManager entry, EODInfo reportInfo)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            //Get the list of all Z reports that have been created
            reportInfo.ZReports = Providers.ZReportData.GetList(entry, entry.CurrentStoreID, entry.CurrentTerminalID);

            var numberSeq = Providers.NumberSequenceData.Get(entry,
                Providers.ZReportData.SequenceID);

            //If the number sequence is at 1 and there are more than one Z reports already in existance then don't allow the Z report to go forward
            if (numberSeq.NextRecord == 1 && reportInfo.ZReports.Count > 0)
            {
                if(!settings.SuppressUI)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.PreviousZNotValid, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }

                return false;
            }            

            //Make sure that the Z reports all add up - if they don't the operation is cancelled
            decimal totalGross = decimal.Zero;
            decimal totalNet = decimal.Zero;                    

            foreach (ZReport zrpt in reportInfo.ZReports)
            {
                totalGross += zrpt.GrossAmount;
                totalNet += zrpt.NetAmount;                

                if (zrpt.TotalGrossAmount != totalGross || zrpt.TotalNetAmount != totalNet)
                {
                    if (!settings.SuppressUI)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.PreviousZAmtNotValid, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }

                    return false;
                }                    
            }            

            return true;
        }

        public virtual void InitializeZReport(IConnectionManager entry, IPosTransaction transaction, ZReport newZReport)
        {       
            InitializeZReportDialog dlg = new InitializeZReportDialog(entry, newZReport);
            dlg.ShowDialog();
        }

        public virtual void PrintInitialZReport(IConnectionManager entry, IPosTransaction transaction, ZReport zReport)
        {
            PrintZReport(entry, transaction, zReport, false);
        }

        private void PrintEFTReport(IConnectionManager entry, ReportType reportType)
        {
            IEFTService service = Interfaces.Services.EFTService(entry);
            if (service == null) return;
            
            EFTReportResult result = reportType == ReportType.XReport ? service.XReport(entry) : service.ZReport(entry);

            if(result.CanPrint && !result.PrintedOnDevice && !string.IsNullOrEmpty(result.ReceiptText))
            {
                Printer.PrintReceipt(entry, result.ReceiptText, null);
            }
        }

        #endregion

        public void Init(IConnectionManager entry)
        {
            #pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
            #pragma warning restore 0612, 0618
        }

        public virtual IErrorLog ErrorLog { set; private get; }
    }
}
