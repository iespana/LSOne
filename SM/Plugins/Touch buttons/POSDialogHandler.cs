using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportClasses.IDialog;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.ItemMaster;

namespace LSOne.ViewPlugins.TouchButtons
{
    class POSDialogHandler : IDialogService
    {
        #region IDialog Members

        IErrorLog errorLog;

        public DialogResult ItemSearch(int howManyRows, ref string selectedItemID, ItemSearchViewModeEnum viewMode,
            RecordIdentifier retailGroupID, IPosTransaction posTransaction, OperationInfo operationInfo = null)
        {
            Dialogs.ItemSearchDialog dlg = new Dialogs.ItemSearchDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedItemID = (string)dlg.ItemID;
                return DialogResult.OK;
            }
             
            return DialogResult.Cancel;
        }
        

        public void ShowJournalDialog(IConnectionManager entry, ref JournalDialogResults dialogResult, ref object dialogResultObject, JournalOperations operations, IPosTransaction posTransaction)
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowMessage(string message, System.Windows.Forms.MessageBoxButtons msgBoxButtons, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowMessage(string message, System.Windows.Forms.MessageBoxButtons msgBoxButtons, Point parentCenter, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowMessage(int messageId, System.Windows.Forms.MessageBoxButtons msgBoxButtons, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowMessage(int messageId, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowMessage(int messageId)
        {
            throw new NotImplementedException();
        }

        public DialogResult TenderDeclaration(IConnectionManager entry, IPosTransaction posTransaction)
        {
            throw new NotImplementedException();
        }

        public DialogResult ScannerInput(ref string inputText, string promptText, int maxLength)
        {
            throw new NotImplementedException();
        }

        public DialogResult KeyboardInput(ref string inputText, string promptText, string ghostText, int maxLength, InputTypeEnum inputType, bool inputRequired = false)
        {
            throw new NotImplementedException();
        }

        public DialogResult PopUpDialog(IConnectionManager entry, ref PopUpFormData popUpFormData)
        {
            throw new NotImplementedException();
        }

        public DialogResult InventoryLookup(IConnectionManager entry, RecordIdentifier itemId, ISession session, IPosTransaction posTransaction, bool showPriceCheck, ref BarCode barcode)
        {
            throw new NotImplementedException();
        }

        public DialogResult DateSelection(IConnectionManager entry, ref string inputText, string promptText, bool inputRequired)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatusDialog(string message, string caption = "")
        {
            throw new NotImplementedException();
        }

        public void UpdateStatusDialog(string message, string caption, StatusDialogIcon icon)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatusDialog(string message, string caption, StatusDialogIcon icon, bool showModal)
        {
            throw new NotImplementedException();
        }

        public void CloseStatusDialog()
        {
            throw new NotImplementedException();
        }

        #endregion        
    
        public DialogResult BarcodeSelect(List<BarCode> barcodes, ref BarCode selectedBarcode, string itemName)
        {
            throw new NotImplementedException();
        }

        public IErrorLog ErrorLog
        {
            set {errorLog = value; }
        }

        public void Init(IConnectionManager entry)
        {
            
        }


        public DialogResult ShowMessage(string message, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowMessage(string message, string caption, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons msgBoxButtons, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons msgBoxButtons, Point parentCenter, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            throw new NotImplementedException();
        }

        public void ShowStatusDialog(string message, string caption = "")
        {
            throw new NotImplementedException();
        }

        public void ShowStatusDialog(string message, string caption, StatusDialogIcon icon)
        {
            throw new NotImplementedException();
        }

        public void ShowStatusDialog(string message, string caption, StatusDialogIcon icon, bool showModal)
        {
            throw new NotImplementedException();
        }

        public void ShowStatusDialog(string message, string caption, StatusDialogIcon icon, bool showModal, Control[] buttons)
        {
            throw new NotImplementedException();
        }

        public Control CreateStatusDialogButton(string text)
        {
            throw new NotImplementedException();
        }

        public void ShowErrorMessage(string message, string details)
        {
            throw new NotImplementedException();
        }

        public void ShowExceptionMessage(Exception ex)
        {
            throw new NotImplementedException();
        }

        public DialogResult PriceCheck(ISession session, IConnectionManager entry, bool useScanner, IPosTransaction posTransaction, bool showInventoryStatus, ref BarCode barcode)
        {
            throw new NotImplementedException();
        }

        public void ShowCloseDrawerMessage()
        {
            throw new NotImplementedException();
        }

        public void ShowCloseDrawerMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void ShowSpinnerDialog(Action<object> action, string caption, string message, out Exception exception)
        {
            throw new NotImplementedException();
        }

        public bool HasQueuedMessages { get; private set; }
        public void EnqueueMessage(QueuedMessage message)
        {
            throw new NotImplementedException();
        }

        public QueuedMessage DequeueMessage()
        {
            throw new NotImplementedException();
        }

        public string Warning { get; private set; }
        public string Error { get; private set; }

        public DialogResult ShowSalesPersonDialog(IConnectionManager entry, ref Employee selectedSalesPerson)
        {
            throw new NotImplementedException();
        }

        public DialogResult RemoveCustomerDiscounts(IConnectionManager entry, string maxDiscountedPurchases,
            string currentDiscountedPurchases, string currencySymbol)
        {
            throw new NotImplementedException();            
        }

        public DialogResult ShowOverrideCustomer(IConnectionManager entry, Name firstCustomerName, Name secondCustomerName)
        {
            throw new NotImplementedException();
        }

        public IEFTSetupForm GetEftSetupForm(DataLayer.BusinessObjects.StoreManagement.Terminal terminal)
        {
            throw new NotImplementedException();
        }

        public void ShowSpinnerDialog(Action action, string caption, string message, out Exception exception)
        {
            throw new NotImplementedException();
        }

        public DialogResult NumpadInput(ISettings settings, ref decimal input, string prompText, string ghostText, bool hasDecimals, DecimalSettingEnum decimalSettings)
        {
            throw new NotImplementedException();
        }

        public DialogResult EmailAddressInput(ref string emailAddress)
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowReturnItemsDialog(IConnectionManager entry, IPosTransaction transaction, ReturnTransactionDialogBehaviourEnum behaviour, bool showReasonCodesSelectList, string defaultReasonCodeID, ref LinkedList<ReturnedItemReason> returnedItems)
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowReasonCodeSelectDialog(IConnectionManager entry, ref ReasonCode reasonCode)
        {
            throw new NotImplementedException();
        }

        public DialogResult DateSelection(string caption, ref DateTime selectedDate, DateTime maxDate, DateTime minDate, bool inputRequired)
        {
            throw new NotImplementedException();
        }

        public DialogResult ItemSearch(int howManyRows, ref string selectedItemID, ref string selectedItemName, ItemSearchViewModeEnum viewMode, RetailItemSearchEnum searchFilterType, RecordIdentifier searchFilterID, IPosTransaction posTransaction, OperationInfo operationInfo = null, bool canBeSold = false)
        {
            throw new NotImplementedException();
        }

        DialogResult IDialogService.BarcodeItemSelect(List<BarCode> barcodes, ref BarCode selectedBarcode, string itemName)
        {
            throw new NotImplementedException();
        }
    }
}
