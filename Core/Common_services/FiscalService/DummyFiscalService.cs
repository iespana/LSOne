using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Fiscal;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Peripherals.OPOS;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSRetailPosis;

namespace LSOne.Services
{
#if FISCALTEST
	public partial class FiscalService : IFiscalService
	{

		private int MaxNoOfReceipts = 2;
		private int MaxNoOfInvoices = 1;

		public bool IsActive()
		{
			/************************************************************************************************************
			 * 
			 * NOTE - To activate ALL fiscal functionality in the POS this function needs to return true.             
			 *      
			 ************************************************************************************************************/

			return true;
		}

		public IErrorLog ErrorLog
		{
			set
			{
			}
		}

		/// <summary>
		///  Get fiscal service about info - Manufacturer Name - e.g. LS Retail ehf.
		/// </summary>
		public virtual string ManufacturerName
		{
			get
			{
				return "LS Retail ehf.";
			}
		}

		/// <summary>
		///  Get fiscal service about info - Code Version - e.g. 9.13.2019.0
		/// </summary>
		public virtual string CodeVersion
		{
			get
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				return assembly.GetName().Version.ToString();
			}
		}

		/// <summary>
		///  Get fiscal service about info - Local Version - e.g. SE
		/// </summary>
		public virtual string LocalVersion
		{
			get
			{
				return string.Empty;
			}
		}

		public void Init(IConnectionManager entry)
		{
			DLLEntry.DataModel = entry;
		}

		public void PrintFiscalLog(IConnectionManager entry, List<DataLayer.BusinessObjects.Fiscal.FiscalLogEntity> logEntities)
		{
		}

		public bool PrintReceiptCopy(IConnectionManager entry, IPosTransaction transaction, ReprintTypeEnum reprintType)
		{
			if (reprintType == ReprintTypeEnum.Invoice && ((RetailTransaction) transaction).Reprints.Count(c => c.ReprintType == ReprintTypeEnum.Invoice) >= MaxNoOfInvoices)
			{
				Interfaces.Services.DialogService(entry).ShowMessage(Resources.MaximumNumberOfInvoicesPrinted.Replace("#1", Conversion.ToStr(MaxNoOfInvoices)));
				return false;
			}

			if (reprintType == ReprintTypeEnum.ReceiptCopy && ((RetailTransaction) transaction).Reprints.Count(c => c.ReprintType == ReprintTypeEnum.ReceiptCopy) >= MaxNoOfReceipts)
			{
				Interfaces.Services.DialogService(entry).ShowMessage(Resources.MaximumNumberOfReceiptsPrinted.Replace("#1", Conversion.ToStr(MaxNoOfReceipts)));
				return false;
			}

			return true;
		}

		public void SaveFiscalLog(IConnectionManager entry, List<DataLayer.BusinessObjects.Fiscal.FiscalLogEntity> logEntities, string saveFilePath)
		{
		}

		public void SaveFiscalLog(IConnectionManager entry, FiscalLogEntity log)
		{
			Providers.FiscalLogData.Save(DLLEntry.DataModel, log);
		}

		public void SaveFiscalLog(IConnectionManager entry, string logString, POSOperations operation)
		{
			FiscalLogEntity log = new FiscalLogEntity {PrintString = logString, Operation = operation};
			SaveFiscalLog(DLLEntry.DataModel, log);
		}

		public JournalLogExportResults ShowJournalLogExportDialog(IConnectionManager entry, ref DateTime fromDate, ref DateTime toDate, ref string saveFilePath, IPosTransaction posTransaction)
		{
			fromDate = DateTime.Today.AddDays(-1);
			toDate = DateTime.Today;
			return JournalLogExportResults.PrintLog;
		}

		public bool StartupCheck(IConnectionManager entry)
		{
			/************************************************************************************************************
			 * 
			 * NOTE - If this function returns false the POS will not start itself up 
			 *        This is only called from the initial loading of the POS
			 *      
			 ************************************************************************************************************/

			return true;
		}

		public bool TransactionCompleted(IConnectionManager entry, IPosTransaction transaction, string receiptID)
		{
			transaction.ReceiptId = receiptID;
			return true;
		}
		
		public bool RunFiscalOperations(IConnectionManager entry, IPosTransaction transaction)
		{
			return false;
		}

		public bool SaveReceiptCopy(IConnectionManager entry, IPosTransaction transaction, bool copyReceipt)
		{
			/************************************************************************************************************
			 * 
			 * NOTE - this code was in the Journal dialog itself and the Printing service             *        
			 *        
			 * Changes done to the code from previous version:
			 *      GetFormattedReceipt is now a private function in Fiscal service             
			 *      
			 ************************************************************************************************************/

			// Formatting the transaction...
			string fiscalFormattedTransaction = GetFormattedReceipt(DLLEntry.DataModel, transaction, true);

			// Saving the transaction to database...
			if (fiscalFormattedTransaction != string.Empty)
			{
				SaveFiscalLog(DLLEntry.DataModel, fiscalFormattedTransaction, POSOperations.NoOperation);
				return true;
			}

			return false;
		}

		public void JournalLogExport(IConnectionManager entry, IPosTransaction transaction)
		{

			/************************************************************************************************************
			 * 
			 * NOTE - this code was in the Journal Export operation within the core. 
			 *        The operation now just checks if the fiscal service IsActive() and then calls this function. 
			 *        No other functionality remains in the operation
			 *        
			 * Changes done to the code from core:
			 *      ShowJournalExportDialog - return the JournalLogExportResults instead of it being a ref parameter
			 *      Resource texts all moved to Fiscal service {0} changed to #1 to match up with LS One tradition
			 *      
			 * PLEASE NOTE!! ShowJournalExportDialog, SaveFiscalLog or PrintFiscalLog don't have any implementation - this function bascily does nothing without them being implemented
			 *      
			 ************************************************************************************************************/
			DateTime fromDate = DateTime.MinValue;
			DateTime toDate = DateTime.MinValue;
			string saveFilePath = "";

			JournalLogExportResults result = ShowJournalLogExportDialog(DLLEntry.DataModel, ref fromDate, ref toDate, ref saveFilePath, transaction);

			if (result != JournalLogExportResults.Cancel)
			{
				var lines = Providers.FiscalLogData.GetList(entry, fromDate, toDate);

				if (lines.Count > 0)
				{
					if (result == JournalLogExportResults.PrintLog)
					{
						PrintFiscalLog(entry, lines);
						Interfaces.Services.DialogService(entry).ShowMessage(Resources.PrintXFiscalLogLines.Replace("#1", Conversion.ToStr(lines.Count())));
					}
					else if (result == JournalLogExportResults.SaveLog)
					{
						SaveFiscalLog(entry, lines, saveFilePath);
						Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.SavedXFiscalLogLines.Replace("#1", Conversion.ToStr(lines.Count())).Replace("#2", saveFilePath));
					}
				}
				else
					Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.NoExportLines, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
			}
		}

	#region Private functions

		private string GetFormattedReceipt(IConnectionManager entry, IPosTransaction transaction, bool copyReceipt)
		{
			char esc = Convert.ToChar(27);

			try
			{
				DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Trace, "FiscalService.GetFormattedReceipt", this.ToString());

				if (transaction is RetailTransaction)
				{
					FormInfo formInfo = new FormInfo();
					formInfo = Services.Interfaces.Services.PrintingService(DLLEntry.DataModel).GetTransformedTransaction(FormSystemType.Receipt, transaction, formInfo, copyReceipt);

					string fiscalString = formInfo.Header + formInfo.Details + formInfo.Footer;

					/************************************************************************************************************
					 * 
					 * NOTE - if variables that use the OPOSConstants in FormItemInfo have been changed for a customization 
					 *        the code below could possibly need to be changed too
					 *      
					 ************************************************************************************************************/

					// performing cleanup...
					fiscalString = fiscalString.Replace(esc + OPOSConstants.RegularFont, "");
					fiscalString = fiscalString.Replace(esc + OPOSConstants.BoldFont, "");
					fiscalString = fiscalString.Replace(esc + OPOSConstants.DoubleHighAndWideFont, "");
					fiscalString = fiscalString.Replace(esc + OPOSConstants.DoubleHighFont, "");
					fiscalString = fiscalString.Replace(esc + OPOSConstants.DoubleUnderLineFont, "");
					fiscalString = fiscalString.Replace(esc + OPOSConstants.DoubleWideFont, "");
					fiscalString = fiscalString.Replace(esc + OPOSConstants.EndFontSequence, "");
					fiscalString = fiscalString.Replace(esc + OPOSConstants.ItalicFont, "");
					fiscalString = fiscalString.Replace(esc + OPOSConstants.ReverseVideoFont, "");
					fiscalString = fiscalString.Replace(esc + OPOSConstants.ShadedFont, "");
					fiscalString = fiscalString.Replace(esc + OPOSConstants.SingleUnderLineFont, "");
					return fiscalString;
				}
				return "";
			}
			catch (Exception ex)
			{
				Interfaces.Services.DialogService(DLLEntry.DataModel).ShowErrorMessage(ex.Message, ex.StackTrace);
				return "";
			}
		}

	#endregion
	}
#endif
}
