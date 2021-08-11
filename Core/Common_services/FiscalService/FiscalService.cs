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
using LSOne.DataLayer.BusinessObjects;
using LSOne.Services.Interfaces.Constants;
using LSOne.DataLayer.GenericConnector.Enums;
using System.Reflection;
using System.IO;
using LSOne.Controls.SupportClasses;

namespace LSOne.Services
{
#if (!FISCALTEST)

	public partial class FiscalService : IFiscalService
	{

		public virtual bool IsActive()
		{
			/************************************************************************************************************
			 * 
			 * NOTE - To activate ALL fiscal functionality in the POS this function needs to return true.             
			 *      
			 ************************************************************************************************************/

			return false;
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
		///  Get fiscal service about info - Code Version - e.g. 9.13.2019.0.
		/// </summary>
		public virtual string CodeVersion
		{
			get
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				return assembly?.GetName().Version.ToString();
			}
		}

		/// <summary>
		///  Get fiscal service about info - Local Version - e.g. No for Norway or SE for Sweeden.
		/// </summary>
		public virtual string LocalVersion
		{
			get
			{
				return Resources.FiscalLocalVersion;
			}
		}

		public void Init(IConnectionManager entry)
		{
#pragma warning disable 0612, 0618
			DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618

		}

		public virtual void PrintFiscalLog(IConnectionManager entry, List<FiscalLogEntity> logEntities)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "PrintFiscalLog - Not implemented", this.ToString());
		}

		public virtual bool PrintReceiptCopy(IConnectionManager entry, IPosTransaction transaction, ReprintTypeEnum reprintType)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "PrintReceiptCopy - Not implemented", this.ToString());
			return true;
		}

		public virtual void SaveFiscalLog(IConnectionManager entry, List<FiscalLogEntity> logEntities, string saveFilePath)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "SaveFiscalLog - Not implemented", this.ToString());
		}

		public virtual void SaveFiscalLog(IConnectionManager entry, FiscalLogEntity log)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "SaveFiscalLog", this.ToString());
			Providers.FiscalLogData.Save(entry, log);
		}

		public virtual void SaveFiscalLog(IConnectionManager entry, string logString, POSOperations operation)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "SaveFiscalLog", this.ToString());
			FiscalLogEntity log = new FiscalLogEntity {PrintString = logString, Operation = operation};
			SaveFiscalLog(entry, log);
		}

		public virtual JournalLogExportResults ShowJournalLogExportDialog(IConnectionManager entry, ref DateTime fromDate, ref DateTime toDate, ref string saveFilePath, IPosTransaction posTransaction)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "ShowJournalLogExportDialog - Not implemented", this.ToString());
			return JournalLogExportResults.Cancel;
		}

		public virtual bool StartupCheck(IConnectionManager entry)
		{
			/************************************************************************************************************
			 * 
			 * NOTE - If this function returns false the POS will not start itself up 
			 *        This is only called from the initial loading of the POS
			 *      
			 ************************************************************************************************************/
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "StartupCheck - Not implemented", this.ToString());
			return true;
		}

		public virtual bool TransactionCompleted(IConnectionManager entry, IPosTransaction transaction)
		{
			/************************************************************************************************************
			 * 
			 * NOTE - Here you need to set the receipt ID on the transaction if you don't want to use the one the POS created for you
			 *        If another receipt ID is to be used then you need to return the receipt ID back into the numbersequence
			 *        using ReturnSequencedNumbers in the TransactionService and set the new receipt ID to the transaction
			 *                   
			 * Example of how to create a receipt ID             
			 * 
					ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
					string receiptID = ((IApplicationService)entry.Service(ServiceType.ApplicationService)).GetNextReceiptId(entry, (string)settings.Terminal.ReceiptIDNumberSequence);
			 *
			 * 
			************************************************************************************************************/
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "TransactionCompleted - Not implemented",
				this.ToString());
			return true;
		}

		public virtual bool RunFiscalOperations(IConnectionManager entry, IPosTransaction transaction)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "RunFiscalOperations - Not implemented", this.ToString());
			return false;
		}

		public virtual bool SaveReceiptCopy(IConnectionManager entry, IPosTransaction transaction, bool copyReceipt)
		{
			/************************************************************************************************************
			 * 
			 * NOTE - this code was in the Journal dialog itself and the Printing service             *        
			 *        
			 * Changes done to the code from previous version:
			 *      GetFormattedReceipt is now a private function in Fiscal service             
			 *      
			 ************************************************************************************************************/
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "SaveReceiptCopy", this.ToString());

			// Formatting the transaction...
			string fiscalFormattedTransaction = GetFormattedReceipt(entry, transaction, true);

			// Saving the transaction to database...
			if (fiscalFormattedTransaction != string.Empty)
			{
				SaveFiscalLog(entry, fiscalFormattedTransaction, POSOperations.NoOperation);
				entry.ErrorLogger.LogMessage(LogMessageType.Trace, "SaveReceiptCopy - receipt saved", this.ToString());
				return true;
			}

			return false;
		}

		public virtual void JournalLogExport(IConnectionManager entry, IPosTransaction transaction)
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
			 * PLEASE NOTE!! ShowJournalLogExportDialog, SaveFiscalLog or PrintFiscalLog don't have any implementation - this function bascily does nothing without them being implemented
			 *      
			 ************************************************************************************************************/
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "JournalLogExport", this.ToString());

			DateTime fromDate = DateTime.MinValue;
			DateTime toDate = DateTime.MinValue;
			string saveFilePath = "";

			JournalLogExportResults result = ShowJournalLogExportDialog(entry, ref fromDate, ref toDate, ref saveFilePath, transaction);

			if (result != JournalLogExportResults.Cancel)
			{
				entry.ErrorLogger.LogMessage(LogMessageType.Trace, "JournalLogExport - Log to be exported/printed", this.ToString());
				var lines = Providers.FiscalLogData.GetList(entry, fromDate, toDate);

				if (lines.Count > 0)
				{
					if (result == JournalLogExportResults.PrintLog)
					{
						PrintFiscalLog(entry, lines);
						Interfaces.Services.DialogService(entry).ShowMessage(Resources.PrintXFiscalLogLines.Replace("#1", Conversion.ToStr(lines.Count())));
						entry.ErrorLogger.LogMessage(LogMessageType.Trace, "JournalLogExport - Log printed", this.ToString());
					}
					else if (result == JournalLogExportResults.SaveLog)
					{
						SaveFiscalLog(entry, lines, saveFilePath);
						Interfaces.Services.DialogService(entry).ShowMessage(Resources.SavedXFiscalLogLines.Replace("#1", Conversion.ToStr(lines.Count())).Replace("#2", saveFilePath));
						entry.ErrorLogger.LogMessage(LogMessageType.Trace, "JournalLogExport - Log saved", this.ToString());
					}
				}
				else
					Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoExportLines, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
			}
		}

		public virtual void HashTransaction(IConnectionManager entry, IPosTransaction transaction)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "HashTransaction - Not implemented", this.ToString());
		}

		public virtual void VerifyHashAndSignOfTransaction(IConnectionManager entry, IPosTransaction transaction, FiscalTrans fiscalTrans)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "VerifyHashAndSignOfTransaction - Not implemented", this.ToString());
		}

		public virtual bool ExportFiscalTransToXML(IConnectionManager entry, RecordIdentifier storeId, RecordIdentifier terminalId, RecordIdentifier employeeId, DateTime startDateTime, DateTime endDateTime, string filename)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "ExportFiscalTransToXML - Not implemented", this.ToString());
			return true;
		}

		public virtual bool IsOperationAllowed(IConnectionManager entry, POSOperations operation)
		{
			/************************************************************************************************************
			 * 
			 * NOTE - This operation is only called in the POS from within the AuthorizeOperationsPage.cs
			 *        For any fiscalization customization this operation can be called from any where an operation is being run
			 *        directly from code or from other customizations
			 *      
			 ************************************************************************************************************/
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "IsOperationAllowed - Not implemented", this.ToString());

			return true;
		}

		public virtual bool IsOperationAllowed(IConnectionManager entry, POSOperations operation, OperationInfo operationInfo, IPosTransaction transaction)
		{
			/************************************************************************************************************
			 * 
			 * NOTE - This operation is called before the operation the user clicked on the POS layout is executed within the POS core
			 *        Within the POS core there is no message displayed to the user, the operation is simply aborted
			 *        Any information you would want to display to the user if this operation is not allowed needs
			 *        to be done within this implementation             
			 *      
			 ************************************************************************************************************/

			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "IsOperationAllowed - Not implemented", this.ToString());

			return true;
		}

		public virtual string GetHeaderTextForTrainingMode(IConnectionManager entry, FormInfo formInfo, ISettings prnSettings, IPosTransaction trans, FormSystemType systemType)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "GetHeaderTextForTrainingMode - not implemented", this.ToString());

			return string.Empty;
		}

		/// <summary>
		///  If POS is in training mode then if needed can customize on Receipt Footer the two lines 
		///  T R A I N I N I N G  M O D E This is not a valid receipt
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="formInfo"></param>
		/// <param name="prnSettings"></param>
		/// <param name="trans"></param>
		/// <param name="systemType"></param>
		public virtual string GetFooterTextForTrainingMode(IConnectionManager entry, FormInfo formInfo, ISettings prnSettings, IPosTransaction trans, FormSystemType systemType)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "GetFooterTextForTrainingMode - not implemented", this.ToString());

			return string.Empty;
		}

		/// <summary>
		///  Get the fiscal signature for a sale
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="prnSettings"></param>
		/// <param name="trans"></param>
		public virtual string GetFiscalSignature(IConnectionManager entry, ISettings prnSettings, IPosTransaction trans)
		{
			entry.ErrorLogger.LogMessage(LogMessageType.Trace, "GetFiscalSignature - not implemented", this.ToString());

			return string.Empty;
		}

		/// <summary>
		/// Gets the value/display text for a printing variable in the Fiscalization category.
		/// </summary>
		/// <returns></returns>
		public virtual string FiscalGetInfoFromTransaction(string variable, IConnectionManager entry, ISettings prnSettings, IPosTransaction trans, out bool variableChanged)
		{
			// the method requires at least a valid variable name, valid settings and a valid POS transaction
			if (string.IsNullOrEmpty(variable)) throw new ArgumentNullException(nameof(variable));
			if (prnSettings is null) throw new ArgumentNullException(nameof(prnSettings));
			if (trans is null) throw new ArgumentNullException(nameof(trans));

			string returnValue = string.Empty;
			variableChanged = true;

			switch (variable)
			{
				case "TRANSACTIONFISCALSIGNATURE":
					returnValue = GetFiscalSignature(entry, prnSettings, trans);
//#if DEBUG
//					returnValue = $"{variable}-FISCALVALUE";
//					variableChanged = true;
//#endif

					break;
				case "FISCALINFO1":
				case "FISCALINFO2":
				case "FISCALINFO3":
				case "FISCALINFO4":
					entry.ErrorLogger.LogMessage(LogMessageType.Trace, $"FiscalGetInfoFromTransaction({variable} - no customization", this.ToString());
					returnValue = string.Empty;

//#if DEBUG
//					returnValue = $"{variable}-FISCALVALUE";
//					variableChanged = true;
//#endif

					break;
				default:
					variableChanged = false;
					break;
			}

			return returnValue;
		}

		/// <summary>
		/// Gets the value/display text for a printing text-only variable. 
		/// </summary>
		/// <param name="variable">Name of the printing parameter.</param>
		/// <param name="entry"></param>
		/// <param name="prnSettings"></param>
		/// <param name="trans">Current POS transaction.</param>
		/// <param name="variableChanged"></param>
		/// <returns></returns>
		public virtual string FiscalGetText(string variable, IConnectionManager entry, ISettings prnSettings, IPosTransaction trans, out bool variableChanged)
		{
			// the method requires at least a valid variable name, valid settings and a valid POS transaction
			if (string.IsNullOrEmpty(variable)) throw new ArgumentNullException(nameof(variable));
			if (prnSettings is null) throw new ArgumentNullException(nameof(prnSettings));
			if (trans is null) throw new ArgumentNullException(nameof(trans));

			string returnValue = string.Empty;
			variableChanged = true;

			switch (variable)
			{
				case "Sale":
				case "SaleCopy":
				case "ReturnSale":
				case "ReturnSaleCopy":
					entry.ErrorLogger.LogMessage(LogMessageType.Trace, $"FiscalGetText({variable} - no customization", this.ToString());
					returnValue = string.Empty;

//#if DEBUG
//					returnValue = $"{variable}-FISCALVALUE";
//					variableChanged = true;
//#endif

					break;
				default:
					variableChanged = false;
					break;
			}

			return returnValue;
		}

		#region Protected functions

		protected virtual string GetFormattedReceipt(IConnectionManager entry, IPosTransaction transaction, bool copyReceipt)
		{
			char esc = Convert.ToChar(27);

			try
			{
				entry.ErrorLogger.LogMessage(LogMessageType.Trace, "GetFormattedReceipt", this.ToString());

				if (transaction is RetailTransaction)
				{
					FormInfo formInfo = new FormInfo();

					formInfo = Interfaces.Services.PrintingService(entry).GetTransformedTransaction(entry, FormSystemType.Receipt, (PosTransaction)transaction, formInfo, copyReceipt);

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
				Interfaces.Services.DialogService(entry).ShowErrorMessage(ex.Message, ex.StackTrace);
				return "";
			}
		}      

		#endregion
	}
#endif
}
