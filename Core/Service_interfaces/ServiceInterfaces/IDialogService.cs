using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportClasses.IDialog;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.ItemMaster;

namespace LSOne.Services.Interfaces
{
    public interface IDialogService : IService
    {
        /// <summary>
        /// Displays a dialog for choosing a sales person
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// <param name="selectedSalesPerson">Returns the sales person selected in the dialog</param>
        /// <returns></returns>
        DialogResult ShowSalesPersonDialog(IConnectionManager entry, ref Employee selectedSalesPerson);

        void ShowJournalDialog(IConnectionManager entry, ref JournalDialogResults dialogResult, ref object dialogResultObject, JournalOperations operations, IPosTransaction posTransaction);
        DialogResult ShowMessage(string message, MessageDialogType dialogType = MessageDialogType.Generic);
        DialogResult ShowMessage(string message, string caption, MessageDialogType dialogType = MessageDialogType.Generic);
        DialogResult ShowMessage(string message, string caption, MessageBoxButtons msgBoxButtons, MessageDialogType dialogType = MessageDialogType.Generic);
        DialogResult ShowMessage(string message, string caption, MessageBoxButtons msgBoxButtons, Point parentCenter, MessageDialogType dialogType = MessageDialogType.Generic);
        DialogResult ShowMessage(string message, MessageBoxButtons msgBoxButtons, MessageDialogType dialogType = MessageDialogType.Generic);
        DialogResult ShowMessage(string message, MessageBoxButtons msgBoxButtons, Point parentCenter, MessageDialogType dialogType = MessageDialogType.Generic);
        DialogResult ItemSearch(int howManyRows, ref string selectedItemID, ItemSearchViewModeEnum viewMode, RecordIdentifier retailGroupID, IPosTransaction posTransaction, OperationInfo operationInfo = null);

        /// <summary>
        /// Shows the item search dialog with the items filtered based on a given filter type and ID
        /// </summary>
        /// <param name="howManyRows">The maximum number of rows to show at one time</param>
        /// <param name="selectedItemID">When the dialog closes, this will be the item ID that the user selected</param>
        /// <param name="selectedItemName">When the dialog closes, this will be the name of the item that the user selected</param>
        /// <param name="viewMode">Sets the mode that the item search dialog shold show in"/></param>
        /// <param name="searchFilterType">The type of filter to set on the search results. <see cref="RetailItemSearchEnum"/></param>
        /// <param name="searchFilterID">The ID to use for the filter. E.g. a retail group ID, special group ID etc.</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="operationInfo">Contains information about the current operation (i.e if it's a return)</param>
        /// <param name="canBeSold">Determines if the function returns items that can be sold or not</param>
        /// <returns></returns>
        DialogResult ItemSearch(int howManyRows, ref string selectedItemID, ref string selectedItemName, ItemSearchViewModeEnum viewMode, RetailItemSearchEnum searchFilterType, RecordIdentifier searchFilterID, IPosTransaction posTransaction, OperationInfo operationInfo = null, bool canBeSold = false);
        DialogResult BarcodeSelect(List<BarCode> barcodes, ref BarCode selectedBarcode, string itemName);
        DialogResult BarcodeItemSelect(List<BarCode> barcodes, ref BarCode selectedBarcode, string itemName);
        DialogResult PriceCheck(ISession session, IConnectionManager entry, bool useScanner, IPosTransaction posTransaction, bool showInventoryStatus, ref BarCode barcode);
        DialogResult TenderDeclaration(IConnectionManager entry, IPosTransaction posTransaction);
        DialogResult KeyboardInput(ref string inputText, string promptText, string ghostText, int maxLength, InputTypeEnum inputType, bool inputRequired = false);
        DialogResult NumpadInput(ISettings settings, ref decimal input, string prompText, string ghostText, bool hasDecimals, DecimalSettingEnum decimalSettings);
        DialogResult DateSelection(IConnectionManager entry, ref string inputText, string promptText, bool inputRequired);
        DialogResult DateSelection(string caption, ref DateTime selectedDate, DateTime maxDate, DateTime minDate, bool inputRequired);
        DialogResult PopUpDialog(IConnectionManager entry, ref PopUpFormData popUpFormData);
        DialogResult InventoryLookup(IConnectionManager entry, RecordIdentifier itemId, ISession session, IPosTransaction posTransaction, bool showPriceCheck, ref BarCode barcode);
        DialogResult ShowReturnItemsDialog(IConnectionManager entry, IPosTransaction transaction, ReturnTransactionDialogBehaviourEnum behaviour, bool showReasonCodesSelectList, string defaultReasonCodeID, ref LinkedList<ReturnedItemReason> returnedItems);
        DialogResult ShowReasonCodeSelectDialog(IConnectionManager entry, ref ReasonCode reasonCode);

        /// <summary>
        /// Shows a dialog that gives the option of removing all customer discounts due to the customer having gone over his maximum discounted purchases limit
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="maxDiscountedPurchases">The customers maximum discounted purchases limit</param>
        /// <param name="currentDiscountedPurchases">The customers current discounted purchases amount</param>
        /// <param name="currencySymbol">The currency used by the customer</param>
        DialogResult RemoveCustomerDiscounts(IConnectionManager entry, string maxDiscountedPurchases, string currentDiscountedPurchases, string currencySymbol);

        /// <summary>
        /// Shows a dialog that allows changing customer for recall transaction.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="firstCustomerName"></param>
        /// <param name="secondCustomerName"></param>
        /// <returns></returns>
        DialogResult ShowOverrideCustomer(IConnectionManager entry, Name firstCustomerName, Name secondCustomerName);

        /// <summary>
        /// Shows the a modeless message dialog. This dialog does not contain buttons and must be 
        /// closed by using the CloseStatusDialog function
        /// </summary>        
        /// <param name="message">The message to display on the dialog</param>
        /// <param name="caption">The caption, if any is desired</param>
        void ShowStatusDialog(string message, string caption = "");

        /// <summary>
        /// Shows a modeless dialog with a message and an icon. This dialog does not contain buttons and must be
        /// closed by using the CloseStatusDialog function
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="caption">The caption on the dialog</param>
        /// <param name="icon">The icon on the caption bar</param>
        void ShowStatusDialog(string message, string caption, StatusDialogIcon icon);

        /// <summary>
        /// Shows a dialog with a message and an icon. This dialog does not contain buttons and must be
        /// closed by using the CloseStatusDialog function
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="caption">The caption on the dialog</param>
        /// <param name="icon">The icon on the caption bar</param>
        /// <param name="showModal">If true the status dialog is displayed as modal otherwise modeless</param>
        void ShowStatusDialog(string message, string caption, StatusDialogIcon icon, bool showModal);

        /// <summary>
        /// Shows a dialog with a message and an icon. This dialog does not contain buttons and must be
        /// closed by using the CloseStatusDialog function
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="caption">The caption on the dialog</param>
        /// <param name="icon">The icon on the caption bar</param>
        /// <param name="showModal">If true the status dialog is displayed as modal otherwise modeless</param>
        /// <param name="buttons">Any buttons to display on the dialog</param>
        void ShowStatusDialog(string message, string caption, StatusDialogIcon icon, bool showModal, Control[] buttons);

        /// <summary>
        /// Updates the modeless message dialog. This dialog does not contain buttons and must be 
        /// closed by using the CloseStatusDialog function. This function will instatiate the dialog by using <see cref="ShowStatusDialog(string,string)"/> if it hasn't already been done
        /// </summary>
        /// <param name="message">The message that should replace the previous one displayed</param>
        /// <param name="caption">The caption of the dialog if any is desired</param>
        void UpdateStatusDialog(string message, string caption = "");

        /// <summary>
        /// Updates the modeless dialog with a new message. This dialog does not contain buttons and must be
        /// closed by using the CloseStatusDialog function. This function will instatiate the dialog by using <see cref="ShowStatusDialog(string,string)"/> if it hasn't already been done
        /// </summary>
        /// <param name="message">The message that should replace the previous one displayed</param>
        /// <param name="caption">The caption on the dialog</param>
        /// <param name="icon">The icon on the caption bar</param>
        void UpdateStatusDialog(string message, string caption, StatusDialogIcon icon);

        /// <summary>
        /// Updates the modeless dialog with a new message. This dialog does not contain buttons and must be
        /// closed by using the CloseStatusDialog function. This function will instatiate the dialog by using <see cref="ShowStatusDialog(string,string)"/> if it hasn't already been done
        /// </summary>
        /// <param name="message">The message that should replace the previous one displayed</param>
        /// <param name="caption">The caption on the dialog</param>
        /// <param name="icon">The icon on the caption bar</param>
        /// <param name="showModal">If true, any status dialog that is created is created as modal</param>
        void UpdateStatusDialog(string message, string caption, StatusDialogIcon icon, bool showModal);

        /// <summary>
        /// Closes the status dialog if it has not already been closed
        /// </summary>        
        void CloseStatusDialog();

        /// <summary>
        /// Create a touch friendly button to display on a status dialog
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Control CreateStatusDialogButton(string text);

        /// <summary>
        /// Shows an error dialog with the given message and details
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="details">The details text to display</param>
        void ShowErrorMessage(string message, string details);

        /// <summary>
        /// Shows an error dialog with the given exception details
        /// </summary>
        /// <param name="ex">Exception</param>
        void ShowExceptionMessage(Exception ex);

        /// <summary>
        /// Shows a modal message dialog which does not close until the drawer is closed. This dialog will automatically close
        /// if called when the drawer is not open. A default message text is used
        /// </summary>
        void ShowCloseDrawerMessage();

        /// <summary>
        /// Shows a modal message dialog which does not close until the drawer is closed. This dialog will automatically close
        /// if called when the drawer is not open.
        /// </summary>
        /// <param name="message">The message body</param>
        void ShowCloseDrawerMessage(string message);

        /// <summary>
        /// If the dialog service has any queued messages for display, then this will return true
        /// </summary>
        bool HasQueuedMessages { get; }

        /// <summary>
        /// Queue a message for later display, e.g. when the POS is ready
        /// </summary>
        /// <param name="message">Message to queue</param>
        void EnqueueMessage(QueuedMessage message);

        /// <summary>
        /// Dequeue a queued message. Will return null if there are no queueed messages
        /// </summary>
        /// <returns>A previously queued message, or null if no queued message exists</returns>
        QueuedMessage DequeueMessage();

        /// <summary>
        /// Gets a standard warning string - in the default locale this is simply 'Warning'
        /// </summary>
        string Warning { get; }

        /// <summary>
        /// Gets a standard error string - in the default locale this is simply 'Error'
        /// </summary>
        string Error { get; }

        /// <summary>
        /// Gets a standard form to collect EFT info
        /// </summary>
        /// <param name="terminal">The terminal to use as reference</param>
        /// <returns></returns>
        IEFTSetupForm GetEftSetupForm(Terminal terminal);

        /// <summary>
        /// Shows a spinner dialog while the given action is running. The dialog closes once the action has completed.
        /// </summary>
        /// <param name="action">The action to perform while the spinner dialog is displayed</param>
        /// <param name="caption">The dialog caption to display</param>
        /// <param name="message">The message to display</param>
        /// <param name="taskEvents">Returns an exception if the action in the spinner dialog fails</param> 
        void ShowSpinnerDialog(Action action, string caption, string message, out Exception taskEvents);
        
        /// <summary>
        /// Displays a dialog where an email address can be entered.
        /// </summary>
        /// <param name="emailAddress">If it is set then this email address will be displayed when the dialog is open. Returns the email address entered in the dialog</param>
        /// <returns>If the user clicked OK or Cancel</returns>
        DialogResult EmailAddressInput(ref string emailAddress);
    }
}
