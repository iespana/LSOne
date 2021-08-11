using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Dialogs;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportClasses.IDialog;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Services.WinFormsTouch;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using Timer = System.Windows.Forms.Timer;
using LSOne.POS.Processes.Common;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSRetailPosis;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Controls.SupportClasses;
using LSOne.Controls.Dialogs.SelectionDialog;
using System.Linq;

namespace LSOne.Services
{
    public partial class DialogService : IDialogService
    {
        private static object queueSyncLock = new object();
        private Timer closeStatusDialogTimer;
        private bool closingStatusDialog;
        private StatusDialog dlgStatusDialog;
        private IErrorLog errorLog;
        private Queue<QueuedMessage> queue = new Queue<QueuedMessage>();

        public DialogService(IConnectionManager entry, ISettings settings)
        {
            DLLEntry.DataModel = entry;
        }

        public DialogService()
        {
            closeStatusDialogTimer = new Timer();
            closeStatusDialogTimer.Interval = 500;
            closeStatusDialogTimer.Tick += CloseStatusDialogTimerOnTick;
        }

        private bool IsTouch
        {
            get
            {
                var settings =(ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                return settings.VisualProfile.TerminalType == VisualProfile.HardwareTypes.Touch;
            }
        }

        private Form GetCurrentMainForm()
        {
            Form foundForm = null;

            foreach (Form form in Application.OpenForms)
            {
                if ((form.Modal && form.Name == "frmMain" && form.Visible) || (form.Name == "LogOnForm" && form.Visible))
                {
                    foundForm = form;
                    break;
                }
            }

            return foundForm;
        }

        private void CloseStatusDialogTimerOnTick(object sender, EventArgs eventArgs)
        {
            closingStatusDialog = false;
            closeStatusDialogTimer.Stop();
            if (dlgStatusDialog != null && !dlgStatusDialog.IsDisposed)
            {
                Form foundForm = null;

                foreach (Form form in Application.OpenForms)
                {
                    if ((form.Modal && form.Name == "frmMain" && form.Visible) || (form.Name == "LogOnForm" && form.Visible))
                    {
                        foundForm = form;
                        break;
                    }
                }
                if (foundForm == null) //If it is not found you are not showing any dialogs either.
                {
                    return;
                }
                
                foundForm.Invoke(ApplicationFramework.POSCloseAndDisposeFormDelegate, new object[] { dlgStatusDialog });
                
                foundForm.Invoke((Action)(() => EnableAndFocus(foundForm)));
            }
        }

        private void EnableAndFocus(Form oForm)
        {
            if (oForm == null)
            {
                return;
            }

            oForm.Enabled = true;
            oForm.Focus();
        }

        #region IDialog Members

        public DialogResult ShowSalesPersonDialog(IConnectionManager entry, ref Employee selectedSalesPerson)
        {
            DLLEntry.DataModel = entry;

            if (IsTouch)
            {
                using (SalesPersonDialog dialog = new SalesPersonDialog())
                {
                    DialogResult result = dialog.ShowDialog();
                    selectedSalesPerson = dialog.SelectedSalesPerson;
                    return result;
                }
            }

            return DialogResult.None;
        }

        public DialogResult ShowReturnItemsDialog(IConnectionManager entry, IPosTransaction transaction, ReturnTransactionDialogBehaviourEnum behaviour, bool showReasonCodesSelectList, string defaultReasonCodeID, ref LinkedList<ReturnedItemReason> returnedItems)
        {
            if(showReasonCodesSelectList)
            {
                ReasonCodeSelectDialog selectDialog = new ReasonCodeSelectDialog(entry);

                if(selectDialog.DialogResult != DialogResult.Abort && selectDialog.ShowDialog() == DialogResult.OK)
                {
                    defaultReasonCodeID = selectDialog.SelectedReasonCode.ID.StringValue;
                }
                else
                {
                    return DialogResult.Cancel;
                }
            }

            using (ReturnTransactionDialog dialog = new ReturnTransactionDialog(transaction, entry, behaviour, defaultReasonCodeID))
            {
                DialogResult result = dialog.ShowDialog();
                returnedItems = dialog.ReturnedItems;
                return result;
            }
        }

        public DialogResult ShowReasonCodeSelectDialog(IConnectionManager entry, ref ReasonCode reasonCode)
        {
            using (ReasonCodeSelectDialog selectDialog = new ReasonCodeSelectDialog(entry))
            {
                DialogResult result = selectDialog.DialogResult == DialogResult.Abort ? DialogResult.Cancel : selectDialog.ShowDialog();

                if(result == DialogResult.OK)
                {
                    reasonCode = selectDialog.SelectedReasonCode;
                }

                return result;
            }
        }

        public void ShowJournalDialog(IConnectionManager entry, ref JournalDialogResults journalDialogResult, ref object journalDialogResultObject, JournalOperations operations, IPosTransaction posTransaction)
        {
            DLLEntry.DataModel = entry;
            
            using (JournalDialog dialog = new JournalDialog(operations))
            {
                dialog.PosTransaction = (PosTransaction) posTransaction;
                dialog.ShowDialog();
                journalDialogResult = dialog.JournalDialogResults;
                journalDialogResultObject = dialog.JournalDialogResultObject;
            }

        }

        public DialogResult ShowMessage(string message, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            return TouchMessageDialog.Show(new Point(DLLEntry.Settings.MainFormInfo.MainWindowHCenter, DLLEntry.Settings.MainFormInfo.MainWindowVCenter), message, dialogType);
        }

        public DialogResult ShowMessage(string message, string caption, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            return TouchMessageDialog.Show(new Point(DLLEntry.Settings.MainFormInfo.MainWindowHCenter, DLLEntry.Settings.MainFormInfo.MainWindowVCenter), message, caption, dialogType);
        }

        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons msgBoxButtons, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            return TouchMessageDialog.Show(new Point(DLLEntry.Settings.MainFormInfo.MainWindowHCenter, DLLEntry.Settings.MainFormInfo.MainWindowVCenter), message, caption, msgBoxButtons, dialogType);
        }

        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons msgBoxButtons, Point parentCenter, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            return TouchMessageDialog.Show(parentCenter, message, caption, msgBoxButtons, dialogType);
        }

        //public DialogResult ShowMessage(string message, string caption, MessageBoxButtons msgBoxButtons)
        //{
        //    return TouchMessageDialog.Show(new Point(DLLEntry.Settings.MainFormInfo.MainWindowHCenter, DLLEntry.Settings.MainFormInfo.MainWindowVCenter), message, caption, msgBoxButtons);
        //}

        public DialogResult ShowMessage(string message, MessageBoxButtons msgBoxButtons, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            return TouchMessageDialog.Show(new Point(DLLEntry.Settings.MainFormInfo.MainWindowHCenter, DLLEntry.Settings.MainFormInfo.MainWindowVCenter), message, " ", msgBoxButtons, dialogType);
        }

        public DialogResult ShowMessage(string message, MessageBoxButtons msgBoxButtons, Point parentCenter, MessageDialogType dialogType = MessageDialogType.Generic)
        {
            return TouchMessageDialog.Show(parentCenter, message, " ", msgBoxButtons, dialogType);
        }

        public DialogResult ItemSearch(int howManyRows, ref string selectedItemID, ItemSearchViewModeEnum viewMode, RecordIdentifier retailGroupID, IPosTransaction posTransaction, OperationInfo operationInfo = null)
        {
            var selectedItemName = "";
            return ItemSearch(howManyRows, ref selectedItemID, ref selectedItemName, viewMode, retailGroupID, posTransaction, operationInfo);
        }

        public DialogResult ItemSearch(int howManyRows, ref string selectedItemID, ref string selectedItemName, ItemSearchViewModeEnum viewMode, RecordIdentifier retailGroupID, IPosTransaction posTransaction, OperationInfo operationInfo = null)
        {

            // Since the item search buttons can be configured to show Images even if the functionality profile is set to disable image search we override
            // whatever the setting is on the buttons here
            if (!DLLEntry.Settings.FunctionalityProfile.AllowImageViewInItemLookup)
            {
                if (viewMode == ItemSearchViewModeEnum.Images )
                {
                    viewMode = ItemSearchViewModeEnum.List;
                }
            }

            using (ItemSearchDialog itemSearchNew = new ItemSearchDialog(howManyRows, viewMode, retailGroupID, posTransaction, operationInfo))
            {
                POSFormsManager.ShowPOSForm(itemSearchNew);
                selectedItemID = (string)itemSearchNew.SelectedItem.ID;
                selectedItemName = itemSearchNew.SelectedItem.NameAlias;
                return itemSearchNew.DialogResult;
            }

        }

        public DialogResult ItemSearch(int howManyRows, ref string selectedItemID, ref string selectedItemName, ItemSearchViewModeEnum viewMode, RetailItemSearchEnum searchFilterType, RecordIdentifier searchFilterID, IPosTransaction posTransaction, OperationInfo operationInfo = null, bool includeCannotBeSold = false)
        {
            // Since the item search buttons can be configured to show Images even if the functionality profile is set to disable image search we override
            // whatever the setting is on the buttons here
            if (!DLLEntry.Settings.FunctionalityProfile.AllowImageViewInItemLookup)
            {
                if (viewMode == ItemSearchViewModeEnum.Images)
                {
                    viewMode = ItemSearchViewModeEnum.List;
                }
            }

            using (ItemSearchDialog itemSearchNew = new ItemSearchDialog(howManyRows, viewMode,searchFilterType, searchFilterID, posTransaction, operationInfo, includeCannotBeSold))
            {
                POSFormsManager.ShowPOSForm(itemSearchNew);
                selectedItemID = (string)itemSearchNew.SelectedItem.ID;
                selectedItemName = itemSearchNew.SelectedItem.NameAlias;
                return itemSearchNew.DialogResult;
            }
        }

        public DialogResult BarcodeSelect(List<BarCode> barcodes, ref BarCode selectedBarcode, string itemName)
        {
            BarCode defaultBarcode = null;

            if(barcodes.Count > 0)
            {
                defaultBarcode = barcodes.FirstOrDefault(x => x.ShowForItem);

                if(defaultBarcode == null)
                {
                    defaultBarcode = barcodes[0];
                }
            }

            using (SelectionDialog dlg = new SelectionDialog(new BarCodeSelectionList(barcodes, defaultBarcode), itemName, false))
            {
                DialogResult result = dlg.ShowDialog();
                selectedBarcode = result == DialogResult.OK ? (BarCode)dlg.SelectedItem : new BarCode();
                return result;
            }
        }

        public DialogResult BarcodeItemSelect(List<BarCode> barcodes, ref BarCode selectedBarcode, string itemName)
        {
            BarCode defaultBarcode = null;

            if (barcodes.Count > 0)
            {
                defaultBarcode = barcodes.FirstOrDefault(x => x.ShowForItem);

                if (defaultBarcode == null)
                {
                    defaultBarcode = barcodes[0];
                }
            }

            using (SelectionDialog dlg = new SelectionDialog(new BarCodeSelectionList(barcodes, defaultBarcode,true), itemName, false))
            {
                DialogResult result = dlg.ShowDialog();
                selectedBarcode = result == DialogResult.OK ? (BarCode)dlg.SelectedItem : new BarCode();
                return result;
            }
        }



        public DialogResult PriceCheck(ISession session,IConnectionManager entry, bool useScanner, IPosTransaction posTransaction, bool showInventoryStatus,
            ref BarCode barcode)
        {            
            DialogResult result = DialogResult.Cancel;

            if (showInventoryStatus && !((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).SiteServiceProfile.UseInventoryLookup)
            {
                ShowMessage(Resources.InventoryLookupNotSetUp);
                return result;
            }

            using (var dlgPriceCheck = new InventoryPriceCheckLookupDialog(session, posTransaction, true, showInventoryStatus))
            {
                if (useScanner)
                {
                    dlgPriceCheck.EnableScanner();
                }

                result = dlgPriceCheck.ShowDialog();
                barcode = dlgPriceCheck.BarCode;
            }

            return result;
        }

        public DialogResult TenderDeclaration(IConnectionManager entry, IPosTransaction posTransaction)
        {
            DialogResult result = DialogResult.Cancel;

            if (IsTouch)
            {
                using (TenderCountDialog dlg = new TenderCountDialog((TenderCountTransaction)posTransaction))
                {
                    result = dlg.ShowDialog();
                }
            }

            return result;
        }

        public IEFTSetupForm GetEftSetupForm(Terminal terminal)
        {
            return new SetupPOSEFTDialog(terminal);
        }

        public void ShowSpinnerDialog(Action action, string caption, string message, out Exception taskEvents)
        {
            Task task = Task.Run(action);

            task.Wait(100);            

            if (task.Status != TaskStatus.RanToCompletion)
            {
                using (SpinnerDialog dlg = new SpinnerDialog(task, caption, message))
                {
                    dlg.ShowDialog();
                    taskEvents = dlg.TaskEvents;
                }
            }
            else
            {
                taskEvents = null;
            }
        }

        public DialogResult EmailAddressInput(ref string emailAddress)
        {
            DialogResult result = DialogResult.Cancel;
            EmailAddressDialog emailDialog = new EmailAddressDialog();
            emailDialog.EmailAddress = emailAddress;

            if (emailDialog.ShowDialog() == DialogResult.OK)
            {
                emailAddress = emailDialog.EmailAddress;
                result = DialogResult.OK;
            }

            return result;
        }

        public DialogResult KeyboardInput(ref string inputText, string promptText, string ghostText, int maxLength, InputTypeEnum inputType, bool inputRequired = false)
        {
            var result = DialogResult.Cancel;
            if (IsTouch)
            {
                using (InputDialog inputDialog = new InputDialog
                    {
                        MaxLength = maxLength,
                        PromptText = promptText,
                        GhostText = ghostText,
                        InputText = inputText,
                        InputType = inputType,
                        InputRequired = inputRequired
                    })
                {
                    inputDialog.ShowDialog();

                    if (inputDialog.DialogResult == DialogResult.OK)
                    {
                        inputText = inputDialog.InputText;
                    }

                    result = inputDialog.DialogResult;
                }
            }
            return result;
        }

        public DialogResult DateSelection(string caption, ref DateTime selectedDate, DateTime maxDate, DateTime minDate, bool inputRequired, bool buttonsVisisble)
        {
            DialogResult result = DialogResult.Cancel;
            using (DatePickerDialog dialog = new DatePickerDialog(selectedDate))
            {                
                dialog.MaximumDate = maxDate;
                dialog.MinimumDate = minDate;
                dialog.InputRequired = inputRequired;                
                dialog.Caption = caption;
                result = dialog.ShowDialog();
                selectedDate = dialog.SelectedDate;
            }
            return result;
        }

        public DialogResult DateSelection(string caption, ref DateTime selectedDate, DateTime maxDate, DateTime minDate, bool inputRequired)
        {
            DialogResult result = DialogResult.Cancel;
            using (DatePickerDialog dialog = new DatePickerDialog(selectedDate))
            {
                dialog.MaximumDate = maxDate;
                dialog.MinimumDate = minDate;
                dialog.InputRequired = inputRequired;
                dialog.Caption = caption;
                result = dialog.ShowDialog();
                selectedDate = dialog.SelectedDate;
            }
            return result;
        }

        public DialogResult DateSelection(IConnectionManager entry, ref string inputText, string promptText, bool inputRequired)
        {
            using(DatePickerDialog dlgDate = new DatePickerDialog())
            {
                DialogResult result = DialogResult.Cancel;
                dlgDate.Caption = promptText;
                dlgDate.InputRequired = inputRequired;

                result = dlgDate.ShowDialog();

                if(result == DialogResult.OK)
                {
                    inputText = dlgDate.SelectedDate.ToString(((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).CultureInfo.DateTimeFormat);
                }

                return result;
            }
        }

        public DialogResult NumpadInput(ISettings settings, ref decimal input, string prompText, string ghostText, bool hasDecimals, DecimalSettingEnum decimalSettings)
        {
            using (var inputDialog = new NumpadAmountQtyDialog())
            {
                inputDialog.HasDecimals = hasDecimals;
                inputDialog.NumberOfDecimals = DLLEntry.DataModel.GetDecimalSetting(decimalSettings).Max;
                inputDialog.PromptText = prompText;
                inputDialog.GhostText = ghostText;

                if (decimalSettings == DecimalSettingEnum.Prices || decimalSettings == DecimalSettingEnum.Tax)
                {
                    inputDialog.SetMaxInputValue((double)settings.FunctionalityProfile.MaximumPrice);
                }
                else if (decimalSettings == DecimalSettingEnum.Quantity)
                {
                    inputDialog.SetMaxInputValue((double)settings.FunctionalityProfile.MaximumQTY);
                }
                else if (decimalSettings == DecimalSettingEnum.DiscountPercent)
                {
                    inputDialog.SetMaxInputValue(100);
                }


                // Quit if cancel is pressed...
                if (inputDialog.ShowDialog() == DialogResult.Cancel)
                {
                    return DialogResult.Cancel;
                }

                input = (decimal)inputDialog.Value;
            }

            return DialogResult.OK;
        }

        public DialogResult PopUpDialog(IConnectionManager entry, ref PopUpFormData popUpFormData)
        {
            var result = DialogResult.Cancel;
            if (IsTouch)
            {
                using (frmPopUpDialog frmPopUpDialog = new frmPopUpDialog(ref popUpFormData))
                {
                    frmPopUpDialog.ShowDialog();
                    result = frmPopUpDialog.DialogResult;
                }
            }
            return result;
        }

        public DialogResult InventoryLookup(IConnectionManager entry, RecordIdentifier itemId, ISession session, IPosTransaction posTransaction, bool showPriceCheck, ref BarCode barCode)
        {
            var result = DialogResult.Cancel;
            if (IsTouch)
            {
                if (!((ISettings) DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).SiteServiceProfile.UseInventoryLookup)
                {
                    ShowMessage(Resources.InventoryLookupNotSetUp);
                    return result;
                }

                using (var inventoryLookupDialog = new InventoryPriceCheckLookupDialog(session, posTransaction, false, showPriceCheck))
                {
                    RetailItem item = Providers.RetailItemData.Get(entry, itemId);

                    if (item != null)
                    {
                        inventoryLookupDialog.AddItem(item);
                    }

                    inventoryLookupDialog.EnableScanner();

                    result = inventoryLookupDialog.ShowDialog();
                    barCode = inventoryLookupDialog.BarCode;
                }
            }
            return result;
        }

        public DialogResult RemoveCustomerDiscounts(IConnectionManager entry, string maxDiscountedPurchases, string currentDiscountedPurchases, string currencySymbol)
        {
            return RemoveDiscountsDialog.Show(maxDiscountedPurchases, currentDiscountedPurchases, currencySymbol);
        }

        /// <summary>
        /// Displays the <see cref="OverrideMessageDialog"/> 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="firstCustomerName"></param>
        /// <param name="secondCustomerName"></param>
        /// <returns></returns>
        public DialogResult ShowOverrideCustomer(IConnectionManager entry, Name firstCustomerName, Name secondCustomerName)
        {
            using (var dialog = new OverrideMessageDialog(
                                        string.Format(Resources.TransactionHasCustomer, entry.Settings.NameFormatter.Format(firstCustomerName)) + "\n\n" +
                                        string.Format(Resources.OverrideCustomer, entry.Settings.NameFormatter.Format(secondCustomerName)) + "\n\n" +
                                        Resources.LoyaltyDiscountsFollowCustomer,
                                        Resources.OverrideCustomerDialogHeader))
            {
                return dialog.ShowDialog();
            }
        }

        public void UpdateStatusDialog(string message, string caption = "")
        {
            UpdateStatusDialog(message, caption, StatusDialogIcon.Message);
        }

        private delegate void UpdateStatusDialogHandler(string message, string caption, StatusDialogIcon icon);

        public void UpdateStatusDialog(string message, string caption, StatusDialogIcon icon)
        {
            UpdateStatusDialog(message, caption, icon, false);
        }

        public void UpdateStatusDialog(string message, string caption, StatusDialogIcon icon, bool showModal)
        {
            if (dlgStatusDialog == null || dlgStatusDialog.IsDisposed)
            {
                ShowStatusDialog(message, caption, icon, showModal);
            }
            else
            {
                if (dlgStatusDialog.InvokeRequired)
                {
                    dlgStatusDialog.BeginInvoke(new UpdateStatusDialogHandler(UpdateStatusDialog), message, caption, icon);
                    return;
                }
                dlgStatusDialog.Message = !string.IsNullOrEmpty(message) ? message : Resources.CommuncatingWithStoreServer;
                Application.DoEvents();
            }
        }

        public void ShowStatusDialog(string message, string caption = "")
        {
            ShowStatusDialog(message, caption, StatusDialogIcon.Message, false);
        }        

        public void ShowStatusDialog(string message, string caption, StatusDialogIcon icon)
        {
            ShowStatusDialog(message, caption, icon, false);
        }

        public void ShowStatusDialog(string message, string caption, StatusDialogIcon icon, bool showModal)
        {
            ShowStatusDialog(message, caption, icon, showModal, null);
        }

        public void ShowStatusDialog(string message, string caption, StatusDialogIcon icon, bool showModal, Control[] buttons)
        {
            if (closingStatusDialog)
            {
                closingStatusDialog = false;
                closeStatusDialogTimer.Stop();
                UpdateStatusDialog(message, caption, icon, showModal);
                return;
            }

            if (dlgStatusDialog == null || dlgStatusDialog.IsDisposed)
            {
                dlgStatusDialog = new StatusDialog(string.IsNullOrEmpty(message) ? Resources.CommuncatingWithStoreServer : message,
                    caption, buttons)
                    {
                        IsModal = showModal
                    };
            }

            Form foundForm = null;

            foreach (Form form in Application.OpenForms)
            {
                if ((form.Modal && form.Name == "frmMain" && form.Visible) || (form.Name == "LogOnForm" && form.Visible))
                {
                    foundForm = form;
                    break;
                }
            }
            if (foundForm == null) //If it is not found you are not showing any dialogs either.
            {
                return;
            }

            if (showModal)
            {
                foundForm.Enabled = false;
                foundForm.Invoke(ApplicationFramework.POSShowFormDelegate, new object[] { dlgStatusDialog });
            }
            else
            {
                foundForm.Invoke((Action)(() => foundForm.Enabled = false));
                foundForm.Invoke(ApplicationFramework.POSShowFormModelessDelegate, new object[] { dlgStatusDialog });
            } 

        }

        public void CloseStatusDialog()
        {
            Form foundForm = GetCurrentMainForm();
            
            if (foundForm == null)
            {
                return;
            }

            closingStatusDialog = true;

            // This is needed in case we call this function from a thread
            foundForm.Invoke((MethodInvoker)(() => closeStatusDialogTimer.Start()));
        }

        public Control CreateStatusDialogButton(string text)
        {
            var button = new Button
            {
                Text = text,
                MinimumSize = new Size(120, 50),
                Font = new Font("Microsoft Sans Serif", 12)
            };
            return button;
        }

        /// <summary>
        /// Shows an error dialog with the given message and details
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="details">The details text to display</param>
        public void ShowErrorMessage(string message, string details)
        {
            var dlg = new ErrorDialog(message, details);
            dlg.ShowDialog();
        }

        /// <summary>
        /// Shows an error dialog with the given exception details
        /// </summary>
        /// <param name="ex">Exception</param>
        public void ShowExceptionMessage(Exception ex)
        {
            string caption = ex.Message;
            string details = "";
            ex = ex.InnerException;
            while (ex != null)
            {
                details += ex.Message + Environment.NewLine;
                ex = ex.InnerException;
            }
            ShowErrorMessage(caption, details);
        }

        /// <summary>
        /// Shows a modal message dialog which does not close until the drawer is closed. This dialog will automatically close
        /// if called when the drawer is not open.
        /// </summary>
        public void ShowCloseDrawerMessage()
        {
            if (!CashDrawer.DrawerOpen())
            {
                return;
            }

            using (CloseDrawerDialog dlg = new CloseDrawerDialog())
            {
                dlg.ShowDialog();
            }
        }

        /// <summary>
        /// Shows a modal message dialog which does not close until the drawer is closed. This dialog will automatically close
        /// if called when the drawer is not open.
        /// </summary>
        /// <param name="message">The message body</param>
        public void ShowCloseDrawerMessage(string message)
        {
            if (!CashDrawer.DrawerOpen())
            {
                return;
            }

            using (CloseDrawerDialog dlg = new CloseDrawerDialog(message))
            {
                dlg.ShowDialog();
            }
        }

        public bool HasQueuedMessages 
        {
            get { return queue.Count > 0; }
        }

        public void EnqueueMessage(QueuedMessage message)
        {
            lock (queueSyncLock)
                queue.Enqueue(message);
        }

        public QueuedMessage DequeueMessage()
        {
            if (queue.Count == 0)
                return null;
            lock (queueSyncLock)
                return queue.Dequeue();
        }

        public string Warning { get { return Resources.Warning; } }
        public string Error { get { return Resources.Error; } }

        #endregion

        public IErrorLog ErrorLog 
        { 
            set { errorLog = value; }
        }

        public void Init(IConnectionManager entry)
        {
            DLLEntry.DataModel = entry;
        }
    }
}
