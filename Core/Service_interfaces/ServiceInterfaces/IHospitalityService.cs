using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using static LSOne.DataLayer.BusinessObjects.Hospitality.HospitalityType;

namespace LSOne.Services.Interfaces
{
    public interface IHospitalityService : IService
    {
        event RunOperationDelegate RunOperation;
        event SetMainViewIndexDelegate SetMainViewIndex;
        event SetTransactionDelegate SetTransactionEvent;
        event SetInputAbilityDelegate SetInputAbilityEvent;
        event LoadPosDesignDelegate LoadPosDesignEvent;
        event LogOffUserDelegate LogOffUserEvent;

        /// <summary>
        /// Transfers a transaction between tables
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="FromTransaction">The being transferred</param>
        /// <param name="ToTransaction">To transaction on the table being transferred to</param>
        /// <param name="FromTableId">The table ID being transferred</param>
        /// <param name="ToTableId">The table ID begin transferred to</param>
        /// <returns>HospitalityResult</returns>
        HospitalityResult TransferTable(IConnectionManager entry, IRetailTransaction FromTransaction, IRetailTransaction ToTransaction, int FromTableId, int ToTableId);

        /// <summary>
        /// Sends information from the retail transaction to a station printer.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="sendAllRemainingItems">If true then no items are excluded from printing. This applies to <see cref="HospitalityType.StationPrinting"/> when it is set to <see cref="StationPrintingEnum.AtItemAddedOneDelay"/></param>
        /// <param name="isPaymentOperation">If true then we are printing from a payment operation</param>
        bool SendToStationPrinter(IConnectionManager entry, IRetailTransaction retailTransaction, bool sendAllRemainingItems, bool isPaymentOperation);

        /// <summary>
        /// Prepare the transaction for payment and check if Site Service is available
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The retail transaction</param>
        /// <returns></returns>
        bool PreparationForPayment(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// Prepare the transaction for void and check if Site Service is available
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The retail transaction</param>
        /// <returns></returns>
        bool PreparationForVoid(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// Returns the table panel for the table view.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="setMainViewIndexHandler">A delegate to switch the between the main dialog and the table view.</param>
        /// <param name="runOperationHandler">A delegate that runs the operations.</param>
        /// <param name="setTransactionHandler">A delegate to set a transaction to the POS</param>
        /// <param name="setInputAbilityHandler">A delegate to enable or disable input into the POS.</param>
        /// <param name="loadPosDesignHandler">A designer for loading the POS design</param>
        /// <param name="logOffUserHandler">A delegate for logging out the user</param>
        /// <returns>TableLayoutPanel.</returns>
        TableLayoutPanel GetHospPanel(IConnectionManager entry, SetMainViewIndexDelegate setMainViewIndexHandler, RunOperationDelegate runOperationHandler, SetTransactionDelegate setTransactionHandler,
            SetInputAbilityDelegate setInputAbilityHandler, LoadPosDesignDelegate loadPosDesignHandler, LogOffUserDelegate logOffUserHandler);

        /// <summary>
        /// Sets the hospitality type text.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="text">The text.</param>
        void SetHospitalityTypeText(IConnectionManager entry, string text);

        /// <summary>
        /// Sets the selected table text.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="table"></param>
        void SetSelectedTableText(IConnectionManager entry, TableInfo table);

        /// <summary>
        /// Initializes the hospitality
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        bool Initialize(IConnectionManager entry);

        /// <summary>
        /// Displays the hospitality table view
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The pos transaction.</param>
        /// <param name="cancelStationPrinting">if true then no station printing is done</param>
        /// <param name="autoLogOff">if true then the POS is trying to automatically log off</param>
        /// <param name="forceLogoffUser">if true then the logoff functionality in the table view is run</param>
        void RunHospitalityPart(IConnectionManager entry, IPosTransaction posTransaction, bool forceLogoffUser, bool cancelStationPrinting, bool autoLogOff = false);

        /// <summary>
        /// An operation to split the bill
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The pos transaction.</param>
        /// <param name="origin">From where does is the operation being called; table view or POS</param>
        void SplitBill(IConnectionManager entry, ref IPosTransaction posTransaction, MainViewEnum origin);

        /// <summary>
        /// Finalizes the transaction after the split bill
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The pos transaction.</param>
        /// <param name="cancelStationPrinting">if true then no station printing is done</param>
        void FinalizeSplitBill(IConnectionManager entry, IPosTransaction posTransaction, bool cancelStationPrinting);

        /// <summary>
        /// Returns the selected table ID.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Returns a table ID</returns>
        int GetSelectedTableId(IConnectionManager entry);

        /// <summary>
        /// Returns the selected table description.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Returns a table description</returns>
        string GetSelectedTableDescription(IConnectionManager entry);

        /// <summary>
        /// Returns the number of guests on the selected table
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Returns the number of guests</returns>
        int GetNoOfGuests(IConnectionManager entry);

        /// <summary>
        /// Gets the active hospitality type in the table view
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>The description of the hospitality type</returns>
        string GetActiveHospSalesType(IConnectionManager entry);

        /// <summary>
        /// Sets the active hospitality type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="salesType">Type of the sales.</param>
        void SetActiveHospitalityType(IConnectionManager entry, string salesType);

        /// <summary>
        /// If the selected sales type (hospitality type) has a specific tax group then the function returns UseTaxGroupFrom.SalesType otherwise the default system value
        /// </summary>
        /// <returns>How the POS should calculate the tax for the selected hospitality type</returns>
        UseTaxGroupFromEnum GetHospitalityTaxGroupFrom(IConnectionManager entry);

        /// <summary>
        /// Sets the printing status on all items on the transaction depending on actions that have been taken already
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        void SetPrintingStatus(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// Sets the printing status on the items depending on actions that have been taken
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="lineItem">The currently selected item</param>
        /// <param name="linkedItemsGetChangedStatus">If the item has linked items should the linked items get a "Changed" status with the header item?</param>
        void SetPrintingStatus(IConnectionManager entry, IPosTransaction posTransaction, IBaseSaleItem lineItem, bool linkedItemsGetChangedStatus);

        /// <summary>
        /// Sets the printing status on the items depending on changes in item comments
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="lineItem">The currently selected item</param>
        /// <param name="newComment">The item line comments after the user has edited them</param>
        /// <param name="originalComment">The item line comments before the user edited them</param>
        void SetPrintingStatus(IConnectionManager entry, IPosTransaction posTransaction, IBaseSaleItem lineItem, string originalComment, string newComment);

        /// <summary>
        /// Sets the printing status on the items depending on changes in item quantity
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="lineItem">The currently selected item</param>
        /// <param name="newQty">The item line quantity after the user has edited them</param>
        /// <param name="originalQty">The item line quantity before the user edited them</param>
        void SetPrintingStatus(IConnectionManager entry, IPosTransaction posTransaction, IBaseSaleItem lineItem, decimal originalQty, decimal newQty);

        /// <summary>
        /// Called when an item has been added to the sales transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The transactio the item was added to</param>        
        void ItemAddedToSale(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Called when the transaction has been voided
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The voided transaction</param>
        void TransactionVoided(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Called when an item is voided
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The transaction that the item belongs to</param>
        /// <param name="lineItem">The voided item</param>
        void ItemVoided(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem lineItem);

        /// <summary>
        /// Called when an items quantity is changed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The transaction that the item belongs to</param>
        /// <param name="lineItem">The item who's quantity changed</param>
        void ItemQuantityChanged(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem lineItem);

        /// <summary>
        /// Bumps the order with the given order id and transaction id
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        /// <param name="transactionId">Id of the transaction</param>
        void BumpOrder(RecordIdentifier transactionId, RecordIdentifier orderId);

        /// <summary>
        /// Refreshes the Kitchen Service that the POS is connected to. This means that settings for kitchen displays are resent and orders are resent to all devices
        /// </summary>
        void RefreshKitchenService();

        void PostPayment(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Deselects any table that is selected.
        /// </summary>
        void ClearTableSelection();

        /// <summary>
        /// Updates the currently selected table with the transaction as it is now.
        /// Will only save the transaction if the POS is active (not table view)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The transaction to save to the selected table</param>
        /// <param name="useTableUpdate">If true then the transaction is saved if the "Table update timer interval" has been reached</param>
        /// /// <param name="secondsIdle">How much time has passed since the user clicked any operation on the POS</param>
        void UpdateSelectedTableTransaction(IConnectionManager entry, IPosTransaction posTransaction, bool useTableUpdate = false, double secondsIdle = 0);

        /// <summary>
        /// Checks if the table opened in the POS is possibly locked by another terminal. This can happen if more than one people click a table at the exact same time
        /// This check is only done once and if more than one terminal has a hospitality profile otherwise it's not done
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        bool IsTableLockedByCurrentTerminal(IConnectionManager entry);

        /// <summary>
        /// Checks if the table open in the POS is unlocked by another terminal.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        bool IsTableUnlocked(IConnectionManager entry);

        /// <summary>
        /// Stops all timers currently active in the Hospitality Service
        /// </summary>
        void StopHospitalityTimers();

        /// <summary>
        /// Called when a tranasction is being concluded. Before it has been saved.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="posTransaction"></param>
        void ConcludeTransaction(IConnectionManager entry, IPosTransaction posTransaction);

        void PrintHospitalityMenuType(IConnectionManager entry, IPosTransaction retailTransaction);

        void ItemCommentAdded(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem saleItem, string comment);
        void ItemCommentsAdded(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem saleItem, IEnumerable<string> comments);

        void ClearItemComment(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem saleItem, string comment);

        /// <summary>
        /// Use this to manually set the private instance of the connection for this service. When using the POSEngine always 
        /// call this function prior to creating a new transaction.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        void SetDataModel(IConnectionManager entry);

        /// <summary>
        /// Gets the currently selected hospitality type
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <returns></returns>
        HospitalityType GetActiveHospitalityType(IConnectionManager entry);

        /// <summary>
        /// Gets the specified dining table information
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="diningTableLayoutID">The dining table layout that the table belongs to</param>
        /// <param name="tableID">The table ID</param>
        /// <returns>The specified table is returned if it is found, otherwise returns <see langword="null"/></returns>
        TableInfo GetDiningTableInfo(IConnectionManager entry, RecordIdentifier diningTableLayoutID, int tableID);

        /// <summary>
        /// Gets the dining table information for the currently selected table on the table view
        /// </summary>
        /// <param name="entry">The connection to the database</param>                        
        /// <returns>The currently selected table, otherwise returns <see langword="null"/> if no table is currently selected</returns>
        TableInfo GetDiningTableInfo(IConnectionManager entry);

        /// <summary>
        /// Checks if the given transaction is part of a split-transaction and combines the split part back into <paramref name="posTransaction"/>
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="posTransaction">The tranasction to combine the split into</param>
        /// <param name="combineSplitTransactions">If true, then the split part will be combined into <paramref name="posTransaction"/></param>
        /// <param name="overrideTransactionID">If not <see langword="null"/> then the transaction ID of <paramref name="posTransaction"/> will be overwritten with this value</param>
        void CheckForSplitTransaction(IConnectionManager entry, IPosTransaction posTransaction, bool combineSplitTransactions, RecordIdentifier overrideTransactionID = null);

        /// <summary>
        /// Shows a dialog allowing the user to select a menu type
        /// </summary>
        /// <param name="menuTypeNames">List of menu type names to display</param>
        /// <param name="caption">Dialog header title</param>
        /// <returns>The selected menu type</returns>
        string SelectMenuType(List<string> menuTypeNames, string caption);
    }
}
