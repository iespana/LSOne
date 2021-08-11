using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.EOD;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// <p/>
    /// In order for the store owner to divide the store’s sale into countable sizes, an End of day or End of shift  must be performed.  
    /// This is mainly done because an employee that uses the Pos, has to account for every transaction made be him 
    /// (and the employees he is reposible for).  
    /// Basically End of day and End of shift does the same thing in relation to the transaction tables.  
    /// <p/>
    /// There are three tables involved in the End of day operation:<br></br><br></br>
    /// <b>RBOTransactionTable</b><br></br> Contains a row for each transaction; the 'master' or 'header' table.<br></br><br></br>
    /// <b>RBOTransactionSalesTrans </b><br></br> Contains a row for each item within a transaction.<br></br><br></br>
    /// <b>RBOTransactionPaymentTrans</b><br></br> Contains a row for each payment operation (it is possible to pay with several tenders).<br></br><br></br>
    /// <p/>
    /// Each Terminal in the store registers transactions using it’s own terminal ID.  Completed transactions are then 
    /// automatically transferred from each terminal to identical datatables int the central store’s database. 
    /// 
    /// <p/>
    /// The constructor is called from \SystemFramework\ApplicationServices.cs, then the following method 
    /// <code>static public void LoadServices()</code>,
    /// tries to load the EOD.dll assembly. The two parameters needed are retrieved from:
    /// <code>LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection</code> and
    /// <code>LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID</code>
    /// </summary>
    public interface IEndOfDayService : IService
    {
        /// <summary>
        /// When a complete working day is finished. The instance of the interface is in the ApplicationServices, 
        /// but the method itself called from within the execute() method in \POSProcesses\Operations\EndOfDay.cs.
        /// <example><code>LSRetailPosis.ApplicationServices.IEOD.EndOfDay((EndOfDayTransaction)this.posTransaction);</code></example>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction as a value object which has been expanded to the operation-value of an type 
        /// 'EndOfDayTransaction'.</param>
        void EndOfDay(IConnectionManager entry, IEndOfDayTransaction transaction);

        /// <summary>
        /// When a complete working shift is finished. The instance of the interface is in the ApplicationServices, 
        /// but the method itself called from within the execute() method in \POSProcesses\Operations\EndOfShift.cs.  
        /// <example><code>LSRetailPosis.ApplicationServices.IEOD.EndOfShift((EndOfShiftTransaction)posTransaction);</code></example>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction as a value object which has been expanded to the operation-value of an type 
        /// 'EndOfShiftTransaction'.</param>
        void EndOfShift(IConnectionManager entry, IEndOfShiftTransaction transaction);

        /// <summary>
        /// Prints the item sale report. It will include a summary of items sold and quantity
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction instance</param>
        /// <param name="printGroup">The group depth used for printing</param>
        void PrintItemSaleReport(IConnectionManager entry, IPosTransaction transaction, ItemSaleReportGroupEnum printGroup);

        /// <summary>        
        /// Prints the X report. It will include all transactions that have not been picked up by a Z report
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction instance.</param>
        void PrintXReport(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>        
        /// Prints the Z report. It will include all transactions that have not already been picked up by a previous Z report        
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction instance.</param>
        /// <param name="displayConfirmation">If true a confirmation dialog is displayed before printing the Z report</param>
        void PrintZReport(IConnectionManager entry, IPosTransaction transaction, bool displayConfirmation);

        /// <summary>
        /// Initializes the "Total POS sales amount". Will only be called if no Z reports are in the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        /// <param name="newZReport">Information about the new Z Report</param>
        void InitializeZReport(IConnectionManager entry, IPosTransaction transaction, ZReport newZReport);

        /// <summary>
        /// Prints an empty Z report wich will include the initialized numbers just entered in the Initialize Z Report operation
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        /// <param name="zReport">Information about the initial Z report</param>
        void PrintInitialZReport(IConnectionManager entry, IPosTransaction transaction, ZReport zReport);

        /// <summary>
        /// Get the X report string to print
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="transaction">Internal transaction with minimal data</param>
        /// <returns>String to be printed representing the X report</returns>
        string GetXReportPrintString(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>
        /// Validate and get the Z report string to print
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="transaction">Internal transaction with minimal data</param>
        /// <param name="zReport">Current Z report. Can be null.</param>
        /// <param name="printString">OUT - string representing the Z report</param>
        /// <returns>True if the Z report is validated and can be printed.</returns>
        /// <remarks>If the result is false then the print string is empty.</remarks>
        bool GetZReportPrintString(IConnectionManager entry, IPosTransaction transaction, ZReport zReport, out string printString);
    }
}
