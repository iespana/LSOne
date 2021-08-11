using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Fiscal;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

using System;
using System.Collections.Generic;

namespace LSOne.Services.Interfaces
{
	public enum BarcodeTextPosition
	{
		None,
		Below
	};

	/// <summary>
	/// Supported barcode symbologies for the fiscal printer
	/// </summary>
	public enum FiscalBarcodeSymbology
	{
		EAN13,
		EAN8,
		Interleaved2Of5,
		Code128,
		Cod39,
		Code93,
		UniversalProductCodeA,
		CodaBar,
		MSI
	};

	public interface IFiscalService : IService
	{
		/// <summary>
        ///  Get fiscal service about info - Manufacturer Name - e.g. LS Retail ehf.
        /// </summary>
        string ManufacturerName { get; }

        /// <summary>
		///  Get fiscal service about info - Code Version - e.g. 1.0
		/// </summary>
		string CodeVersion { get; }

        /// <summary>
		///  Get fiscal service about info - Local Version - e.g. SE
		/// </summary>
		string LocalVersion { get; }
		
		bool StartupCheck(IConnectionManager entry);

		bool TransactionCompleted(IConnectionManager entry, IPosTransaction transaction);

		bool RunFiscalOperations(IConnectionManager entry, IPosTransaction transaction);

		/// <summary>
		/// Every time a copy of the receipt is printed this function is called to decide if the copy can be printed
		/// </summary>
		/// <param name="entry">Instance of the connection manager</param>
		/// <param name="transaction">The transaction being printed</param>
		/// <param name="reprintType">What type of copy is being printed; receipt copy or invoice</param>
		/// <returns></returns>
		bool PrintReceiptCopy(IConnectionManager entry, IPosTransaction transaction, ReprintTypeEnum reprintType);

		/// <summary>
		/// Prints out selected fiscal log entities
		/// </summary>
		/// <param name="entry">Instance of the connection manager</param>
		/// <param name="logEntities">Instances of log lines to be printed</param>
		void PrintFiscalLog(IConnectionManager entry, List<FiscalLogEntity> logEntities);

		/// <summary>
		/// Saves selected fiscal log entities to a file
		/// </summary>
		/// <param name="entry">Instance of the connection manager</param>
		/// <param name="logEntities">Instances of log lines to be printed</param>
		/// <param name="saveFilePath">The path to the log file</param>
		void SaveFiscalLog(IConnectionManager entry, List<FiscalLogEntity> logEntities, string saveFilePath);

		/// <summary>
		/// Saves a log entry to the database
		/// </summary>
		/// <param name="entry">Connection to the database</param>
		/// <param name="log">The log to be saved</param>
		void SaveFiscalLog(IConnectionManager entry, FiscalLogEntity log);

		/// <summary>
		/// Creates a FiscalLogEntry and saves it to the database
		/// </summary>
		/// <param name="entry">Connection to the database</param>
		/// <param name="logString">Log string to be saved</param>
		/// <param name="operation">The operation saving the log</param>
		void SaveFiscalLog(IConnectionManager entry, string logString, POSOperations operation);

		/// <summary>
		/// Defines if the fiscal implementation is active
		/// </summary>
		bool IsActive();

		/// <summary>
		/// Shows a dialog to select a date range and then decide if the fiscal journal log should be printed or saved to a file
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="saveFilePath"></param>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <param name="posTransaction"></param>
		JournalLogExportResults ShowJournalLogExportDialog(IConnectionManager entry, ref DateTime fromDate, ref DateTime toDate, ref string saveFilePath, IPosTransaction posTransaction);

		/// <summary>
		/// Save a receipt copy
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="transaction"></param>
		/// <param name="copyReceipt"></param>
		/// <returns></returns>
		bool SaveReceiptCopy(IConnectionManager entry, IPosTransaction transaction, bool copyReceipt);

		/// <summary>
		/// Exports the journal log.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="transaction"></param>
		void JournalLogExport(IConnectionManager entry, IPosTransaction transaction);

		void HashTransaction(IConnectionManager entry, IPosTransaction transaction);

		void VerifyHashAndSignOfTransaction(IConnectionManager entry, IPosTransaction transaction, FiscalTrans fiscalTrans);
		
		bool ExportFiscalTransToXML(IConnectionManager entry, RecordIdentifier storeId, RecordIdentifier terminalId, RecordIdentifier employeeId, DateTime startDateTime, DateTime endDateTime, string filename);

		bool IsOperationAllowed(IConnectionManager entry, POSOperations operation);

		bool IsOperationAllowed(IConnectionManager entry, POSOperations operation, OperationInfo operationInfo, IPosTransaction transaction);

		/// <summary>
		///  If POS is in training mode then if needed can customize on Receipt Header the two lines 
		///  T R A I N I N I N G  M O D E This is not a valid receipt
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="formInfo"></param>
		/// <param name="prnSettings"></param>
		/// <param name="trans"></param>
		/// <param name="systemType"></param>
		string GetHeaderTextForTrainingMode(IConnectionManager entry, FormInfo formInfo, ISettings prnSettings, IPosTransaction trans, FormSystemType systemType);

		/// <summary>
		///  If POS is in training mode then if needed can customize on Receipt Footer the two lines 
		///  T R A I N I N I N G  M O D E This is not a valid receipt
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="formInfo"></param>
		/// <param name="prnSettings"></param>
		/// <param name="trans"></param>
		/// <param name="systemType"></param>
		string GetFooterTextForTrainingMode(IConnectionManager entry, FormInfo formInfo, ISettings prnSettings, IPosTransaction trans, FormSystemType systemType);

		/// <summary>
		///  Get the fiscal signature for a sale
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="prnSettings"></param>
		/// <param name="trans"></param>
		string GetFiscalSignature(IConnectionManager entry, ISettings prnSettings, IPosTransaction trans);

		/// <summary>
		/// Gets the value/display text for a printing variable in the Fiscalization category.
		/// </summary>
		/// <returns></returns>
		string FiscalGetInfoFromTransaction(string variable, IConnectionManager entry, ISettings prnSettings, IPosTransaction trans, out bool variableChanged);

		/// <summary>
		/// Gets the value/display text for a printing text-only variable. 
		/// </summary>
		/// <param name="variable">Name of the printing parameter.</param>
		/// <param name="entry"></param>
		/// <param name="prnSettings"></param>
		/// <param name="trans">Current POS transaction.</param>
		/// <param name="variableChanged"></param>
		/// <returns></returns>
		string FiscalGetText(string variable, IConnectionManager entry, ISettings prnSettings, IPosTransaction trans, out bool variableChanged);
    }
}
