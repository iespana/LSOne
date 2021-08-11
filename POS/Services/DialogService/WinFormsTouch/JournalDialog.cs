using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Receipts;
using LSOne.POS.Processes.Common;
using LSOne.POS.Processes.WinControls;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.EventArguments;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    public partial class JournalDialog : TouchBaseForm
    {
        private enum Buttons
        {
            PageUp,
            SelectionUp,
            SelectionDown,
            PageDown,
            Select,
            Search,
            Clear,
            Close,
            Invoice,
            Receipt,
            GiftReceipt,
            EmailReceipt,
            ReturnTransaction,
            TaxFree
        }

        private List<Transaction> journalEntries;
        private bool noMoreRows = false;
        private PosTransaction posTransaction;
        private RecordIdentifier selectedTransactionId = RecordIdentifier.Empty;
        private const int maxRowsAtEachQuery = 25;
        private bool customerPanelIsVisible = true;
        private JournalDialogResults journalDialogResults;
        private Object journalDialogResultObject;
        private RecordIdentifier lowestTransactionID;
        private DateTime? searchFromTime;
        private DateTime? searchToTime;
        private TypeOfTransaction? type;
        private string staff;
        private decimal? minAmount;
        private decimal? maxAmount;
        private bool useTerminal = false;
        private int customerPanelHeight = 0;
        private Receipt receipt;
        private Lazy<frmJournalSearch> frmJournalSearchLazy = new Lazy<frmJournalSearch>();

        public JournalDialog()
        {
            InitializeComponent();              
        }

        public JournalDialog(JournalOperations operations)
            : this()
        {
            panel.AddButton("", Buttons.PageUp, Conversion.ToStr((int)Buttons.PageUp), image: Resources.Doublearrowupthin_32px);
            panel.AddButton("", Buttons.SelectionUp, Conversion.ToStr((int)Buttons.SelectionUp), image: Resources.Arrowupthin_32px);
            panel.AddButton("", Buttons.SelectionDown, Conversion.ToStr((int)Buttons.SelectionDown), image: Resources.Arrowdownthin_32px);
            panel.AddButton("", Buttons.PageDown, Conversion.ToStr((int)Buttons.PageDown), image: Resources.Doublearrowdownthin_32px);

            if ((operations & JournalOperations.Invoice) == JournalOperations.Invoice)
            {
                panel.AddButton(Resources.Invoice, Buttons.Invoice, Conversion.ToStr((int)Buttons.Invoice));
            }

            if ((operations & JournalOperations.Receipt) == JournalOperations.Receipt)
            {
                panel.AddButton(Resources.Receipt, Buttons.Receipt, Conversion.ToStr((int)Buttons.Receipt));
                panel.AddButton(Resources.GiftReceipt, Buttons.GiftReceipt, Conversion.ToStr((int)Buttons.GiftReceipt));
                panel.AddButton(Resources.EmailReceipt, Buttons.EmailReceipt, Conversion.ToStr((int)Buttons.EmailReceipt));
            }

            if ((operations & JournalOperations.Return) == JournalOperations.Return)
            {
                panel.AddButton(Resources.ReturnTransaction, Buttons.ReturnTransaction, Conversion.ToStr((int)Buttons.ReturnTransaction));
            }

            if ((operations & JournalOperations.TaxFree) == JournalOperations.TaxFree && Providers.TaxFreeConfigData.HasEntries(DLLEntry.DataModel))
            {
                panel.AddButton(Resources.TaxFree, Buttons.TaxFree, Conversion.ToStr((int)Buttons.TaxFree));
            }

            if ((operations & JournalOperations.Select) == JournalOperations.Select)
            {
                panel.AddButton(Resources.Select, Buttons.Select, Conversion.ToStr((int) Buttons.Select), TouchButtonType.OK, DockEnum.DockEnd);
            }

            panel.AddButton(Resources.Search, Buttons.Search, Conversion.ToStr((int)Buttons.Search), TouchButtonType.Action, DockEnum.DockEnd);
            panel.AddButton(Resources.Clear, Buttons.Clear, Conversion.ToStr((int)Buttons.Clear), dock: DockEnum.DockEnd);
            panel.AddButton(Resources.Close, Buttons.Close, Conversion.ToStr((int)Buttons.Close), TouchButtonType.Cancel, DockEnum.DockEnd);
            
            useTerminal = (operations & JournalOperations.NotDownToTerminal) != JournalOperations.NotDownToTerminal;

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ReturnTransaction), false);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.TaxFree), false);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.GiftReceipt), false);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), false);

            customerPanelHeight = pnlCustomerInfo.Height + 5; //5 is space between customer panel and receipt control

            if(!DesignMode)
            {
                receipt = new ReceiptControlFactory().CreateReceiptControl(pnlReceipt);
            }
        }

        public PosTransaction PosTransaction
        {
            get { return posTransaction; }
            set { posTransaction = value; }
        }

        public JournalDialogResults JournalDialogResults
        {
            get { return journalDialogResults; }
        }

        public Object JournalDialogResultObject
        {
            get { return journalDialogResultObject; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                lowestTransactionID = "";
                //Set the size of the form the same as the main form
                this.Width = ((ISettings) DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).MainFormInfo.MainWindowWidth;
                this.Height = ((ISettings) DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).MainFormInfo.MainWindowHeight;

                HideCustomerPanel();

                Currency currency = Providers.CurrencyData.Get(DLLEntry.DataModel, DLLEntry.Settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
                receipt.DefaultCurrencySymbol = (DLLEntry.Settings.VisualProfile.ShowCurrencySymbolOnColumns) ? currency.Symbol : "";

                //If the store doesn't display balance with tax then the journal doesn't display all the amounts correctly
                //because the user cannot add columns to display the amounts that are missing from the default configuration of the receipt
                //So we display both total columns and the sales balance with tax
                if (!DLLEntry.Settings.Store.DisplayBalanceWithTax)
                {
                    receipt.ReceiptItems.DisplayTotalWithAndWithoutTax(true);
                    receipt.ReceiptItems.DisplaySalesBalanceWithTax(true);
                }

                this.Top = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).MainFormInfo.MainWindowTop;
                this.Left = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).MainFormInfo.MainWindowLeft;
                lvJournal.ApplyRelativeColumnSize();
                journalEntries = new List<Transaction>();
                if (LoadListView() <= 0)
                {
                    Close();
                }

                receipt.PreDisplayReceiptItemEvent += ReceiptOnPreDisplayReceiptItemEvent;

                lvJournal.AutoSelectOnFocus = true;
                lvJournal.Focus();
            }
            catch (Exception ex)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(ex.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                this.Close();
            }
        }

        private int LoadListView(List<Transaction> optionalList = null)
        {
            lvJournal.ClearRows();
            journalEntries.Clear();
            if (optionalList == null)
            {
                journalEntries = new List<Transaction>();
                optionalList = Providers.TransactionData.GetJournalTransactions(DLLEntry.DataModel, maxRowsAtEachQuery, useTerminal ? DLLEntry.DataModel.CurrentTerminalID : null);
                noMoreRows = false;
            }
            if (optionalList.Count <= 0)
            {
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.NoRecords, MessageBoxButtons.OK, MessageDialogType.Generic);
                return -1;
            }
            AddRowsToListView(optionalList);
            lvJournal.MoveSelectionUp();
            return optionalList.Count;
        }

        private void AddRowsToListView(List<Transaction> list)
        {
            foreach (Transaction current in list)
            {
                var row = new Row();
                var info = Providers.CurrencyData.Get(DLLEntry.DataModel, current.CurrencyID, CacheType.CacheTypeApplicationLifeTime);
                row.AddText(current.TransactionDate.DateTime.ToString("d", ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).CultureInfo) + " " + current.TransactionDate.DateTime.ToString("t", ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).CultureInfo));
                row.AddText((string)current.Login);
                row.AddText((string)current.ReceiptID);
                row.AddText(current.TypeDescription);

                row.AddText((info.CurrencyPrefix + " " + DecimalExtensions.FormatWithLimits(-current.NetAmountWithTax, new DecimalLimit(DecimalExtensions.DigitsBeforeFirstSignificantDigit(info.RoundOffSales))) + " " + info.CurrencySuffix).Trim());
                
                lvJournal.AddRow(row);
            }
            lowestTransactionID = list[list.Count - 1].TransactionID;
            journalEntries.AddRange(list);
        }

        private void GetMoreTransactions()
        {
            if (!noMoreRows)
            {
                List<Transaction> list = Providers.TransactionData.GetJournalTransactions(
                    DLLEntry.DataModel, maxRowsAtEachQuery, useTerminal ? DLLEntry.DataModel.CurrentTerminalID : null, 
                    fromDate: searchFromTime, toDate: searchToTime, fromTransaction: lowestTransactionID,
                    type: type, staff: staff, minAmount: minAmount, maxAmount: maxAmount);
                if (list.Count > 0)
                {
                    list.RemoveAt(0);   //One of error
                }
                if ((list.Count == 0) || (lowestTransactionID == list[0].TransactionID))
                {
                    noMoreRows = true;
                    return;
                }
                AddRowsToListView(list);
            }
        }

        private PosTransaction LoadTransaction(RecordIdentifier transactionId, RecordIdentifier storeID, RecordIdentifier terminalID, bool asRetail = false)
        {
            try
            {
                TypeOfTransaction transactionType = TransactionProviders.PosTransactionData.GetTransactionType(DLLEntry.DataModel, transactionId, storeID, terminalID);
                PosTransaction transaction = new RetailTransaction((string)storeID,
                                                                          DLLEntry.Settings.Store.Currency,
                                                                          DLLEntry.Settings.TaxIncludedInPrice);

                

                transaction.TerminalId = (string)terminalID;

                if (transactionType == TypeOfTransaction.Sales || asRetail )
                {
                    TransactionProviders.PosTransactionData.GetTransaction(DLLEntry.DataModel, transactionId,
                                                             storeID,
                                                             terminalID,
                                                             transaction,
                                                             DLLEntry.Settings.TaxIncludedInPrice);

                    bool hasReturnableSaleLine = false;

                    ITransactionService transactionService = Interfaces.Services.TransactionService(DLLEntry.DataModel);

                    foreach(ISaleLineItem saleItem in ((RetailTransaction)transaction).ISaleItems)
                    {
                        Interfaces.Services.CalculationService(DLLEntry.DataModel).CalculatePeriodicDiscountPercent(saleItem, (RetailTransaction)transaction);

                        if(!hasReturnableSaleLine)
                        {
                            hasReturnableSaleLine = transactionService.CanReturnSaleLineItem(saleItem);
                        }
                    }

                    panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ReturnTransaction), hasReturnableSaleLine);
                    panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.TaxFree), hasReturnableSaleLine);
                    panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.GiftReceipt), hasReturnableSaleLine);
                }
                else if (transactionType == TypeOfTransaction.Deposit)
                {
                    TransactionProviders.PosTransactionData.GetTransaction(DLLEntry.DataModel, transactionId,
                                                             storeID,
                                                             terminalID,
                                                             transaction,
                                                             DLLEntry.Settings.TaxIncludedInPrice);

                    DepositTransaction newTrans = new DepositTransaction(transaction.StoreId, transaction.StoreCurrencyCode, ((RetailTransaction)transaction).TaxIncludedInPrice);
                    transaction = newTrans.Clone(((RetailTransaction)transaction));

                    panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ReturnTransaction), false);
                    panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.TaxFree), false);
                    panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.GiftReceipt), false);
                }
                else if (transactionType == TypeOfTransaction.Payment)
                {
                    transaction = new CustomerPaymentTransaction(((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.Currency);
                    TransactionProviders.PosTransactionData.GetTransaction(DLLEntry.DataModel, transactionId,
                                                             storeID,
                                                             terminalID,
                                                             transaction,
                                                             DLLEntry.Settings.TaxIncludedInPrice);

                    panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ReturnTransaction), false);
                    panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.TaxFree), false);
                    panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.GiftReceipt), false);
                }
                return transaction;
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private bool PrintReceipt(PosTransaction posTransaction)
        {
            bool printedOK = false;
            try
            {
                if (posTransaction != null)
                {
                    printedOK = Interfaces.Services.PrintingService(DLLEntry.DataModel).PrintTransaction(DLLEntry.DataModel, posTransaction, true, true);
                    if (printedOK)
                    {
                        var fiscalService = (IFiscalService)DLLEntry.DataModel.Service(ServiceType.FiscalService);
                        if (fiscalService != null && fiscalService.IsActive())
                        {
                            fiscalService.SaveReceiptCopy(DLLEntry.DataModel, posTransaction, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }

            return printedOK;
        }

        private bool PrintGiftReceipt(PosTransaction posTransaction)
        {
            bool printedOK = false;
            try
            {
                if (posTransaction != null)
                {
                    printedOK = Interfaces.Services.PrintingService(DLLEntry.DataModel).PrintTransaction(DLLEntry.DataModel, posTransaction, true, true);
                    if (printedOK)
                    {
                        var fiscalService = (IFiscalService)DLLEntry.DataModel.Service(ServiceType.FiscalService);
                        if (fiscalService != null && fiscalService.IsActive())
                        {
                            fiscalService.SaveReceiptCopy(DLLEntry.DataModel, posTransaction, true);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }

            return printedOK;
        }

        private bool PrintSlip(PosTransaction posTransaction)
        {
            try
            {
                if (posTransaction != null)
                {
                    try
                    {
                        DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Trace, "PrintTransaction()", this.ToString());

                        if (posTransaction is RetailTransaction)
                        {
                            return Interfaces.Services.PrintingService(DLLEntry.DataModel).PrintInvoice(DLLEntry.DataModel, posTransaction, false, false);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }

            return false;
        }

        private void DisplayCustomerInfo(PosTransaction posTransaction)
        {
            bool customerExists = false;

            if (posTransaction is RetailTransaction)
            {
                customerExists = !RecordIdentifier.IsEmptyOrNull(((RetailTransaction) posTransaction).Customer.ID);
                var resources = new ComponentResourceManager(typeof (JournalDialog));
                lblCustomer.Text = ((RetailTransaction) posTransaction).Customer.FirstName != "" ?
                    ((RetailTransaction)posTransaction).Customer.GetFormattedName(DLLEntry.DataModel.Settings.NameFormatter) :
                    ((RetailTransaction) posTransaction).Customer.Text;

                lblAddress.Text = DLLEntry.DataModel.Settings.LocalizationContext.FormatMultipleLines(
                    ((RetailTransaction) posTransaction).Customer.DefaultShippingAddress,
                    DLLEntry.DataModel.Cache,
                    "\n"); //Address      
            }
            else if (posTransaction is CustomerPaymentTransaction)
            {
                customerExists = !RecordIdentifier.IsEmptyOrNull(((CustomerPaymentTransaction) posTransaction).Customer.ID);
                var resources = new ComponentResourceManager(typeof (JournalDialog));
                lblCustomer.Text = ((CustomerPaymentTransaction) posTransaction).Customer.FirstName != "" ?
                    ((CustomerPaymentTransaction)posTransaction).Customer.GetFormattedName(DLLEntry.DataModel.Settings.NameFormatter) :
                    ((CustomerPaymentTransaction) posTransaction).Customer.Text;

                lblAddress.Text = DLLEntry.DataModel.Settings.LocalizationContext.FormatMultipleLines(
                    ((CustomerPaymentTransaction) posTransaction).Customer.DefaultShippingAddress,
                    DLLEntry.DataModel.Cache,
                    "\n"); //Address                 
            }

            if (customerExists)
            {
                if (!customerPanelIsVisible)
                {
                    ShowCustomerPanel();
                }
            }
            else if (customerPanelIsVisible)
            {
                HideCustomerPanel();
            }
        }

        private void HideCustomerPanel()
        {
            customerPanelIsVisible = false;

            pnlReceipt.Height += customerPanelHeight;
            pnlReceipt.Location = new Point(11, 11);

            pnlCustomerInfo.Visible = false;
            this.Refresh();
            receipt.Refresh();
        }

        private void ShowCustomerPanel()
        {
            customerPanelIsVisible = true;

            pnlReceipt.Height -= customerPanelHeight;
            pnlReceipt.Location = new Point(11, 11 + customerPanelHeight);
            pnlCustomerInfo.Visible = true;

            this.Refresh();
            receipt.Refresh();
        }

        private void lvJournal_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvJournal.Selection.FirstSelectedRow >= 0)
                {
                    selectedTransactionId = new RecordIdentifier(journalEntries[lvJournal.Selection.FirstSelectedRow].TransactionID, 
                                                                 journalEntries[lvJournal.Selection.FirstSelectedRow].StoreID, 
                                                                 journalEntries[lvJournal.Selection.FirstSelectedRow].TerminalID);
                    PosTransaction transaction = LoadTransaction(selectedTransactionId.PrimaryID, selectedTransactionId.SecondaryID.PrimaryID, selectedTransactionId.SecondaryID.SecondaryID.PrimaryID);
                    DisplayCustomerInfo(transaction);
                    receipt.ShowTransaction(transaction);
                }
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
            }

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Select), lvJournal.Selection.Count > 0);
        }

        private void lvJournal_VerticalScrollValueChanged(object sender, EventArgs e)
        {
            if ((lvJournal.FirstRowOnScreen + lvJournal.RowCountOnScreen) >= (journalEntries.Count - 1))
            {
                GetMoreTransactions();
            }
        }

        private void btnSearch_Click()
        {
            var searchJournal = frmJournalSearchLazy.Value;

            searchJournal.ShowDialog();

            if (searchJournal.DialogResult != DialogResult.OK)
            {
                return;
            }

            searchFromTime = searchJournal.SelectedFromDate;
            searchToTime = searchJournal.SelectedToDate;
            type = searchJournal.Type;
            staff = searchJournal.Staff;
            // We negate the value when we display it (see AddRowsToListView()), so negate the filter values, too
            minAmount = -searchJournal.MaxAmount;
            maxAmount = -searchJournal.MinAmount;

            var searchResult = minAmount > maxAmount ? 
                                new List<Transaction>() :
                                Providers.TransactionData.GetJournalTransactions(DLLEntry.DataModel,
                                    maxRowsAtEachQuery, 
                                    useTerminal ? DLLEntry.DataModel.CurrentTerminalID : null,
                                    toDate: searchToTime,
                                    fromDate: searchFromTime,
                                    receipt: searchJournal.SelectedTransactionId,
                                    type: type,
                                    staff: staff,
                                    minAmount: minAmount,
                                    maxAmount: maxAmount);

            noMoreRows = false;
            LoadListView(searchResult);
            searchToTime = searchJournal.SelectedToDate;

            panel.SetButtonEnabled(Conversion.ToStr((int) Buttons.Clear), true);
            
            if(lvJournal.RowCount > 0)
            {                
                lvJournal.Selection.Set(0);
                lvJournal.Focus(); // another control is stealing the focus after we show the search dialog so we have to set the focus manually
            }
        }

        private void btnClear_Click()
        {
            frmJournalSearchLazy.Value.ResetFilter();
            LoadListView();
            panel.SetButtonEnabled(Conversion.ToStr((int) Buttons.Clear), false);
            searchFromTime = null;
            searchToTime = null;
        }

        private void btnReceipt_Click()
        {
            try
            {
                PosTransaction transaction = LoadTransaction(selectedTransactionId.PrimaryID, selectedTransactionId.SecondaryID.PrimaryID, selectedTransactionId.SecondaryID.SecondaryID.PrimaryID);

                if (transaction is DepositTransaction)
                {
                    GetCustomerOrderInformation(transaction, ReprintTypeEnum.ReceiptCopy);
                }
                
                bool doPrinting = true;

                var fiscalService = (IFiscalService)DLLEntry.DataModel.Service(ServiceType.FiscalService);
                if (fiscalService != null && fiscalService.IsActive())
                {
                    doPrinting = fiscalService.PrintReceiptCopy(DLLEntry.DataModel, transaction, ReprintTypeEnum.ReceiptCopy);
                }

                bool btnOkWasClickedOnPrintReceipt = PrintReceipt(transaction);
                if (doPrinting && btnOkWasClickedOnPrintReceipt)
                {
                    SaveReceiptCopyInformation(transaction, ReprintTypeEnum.ReceiptCopy);
                }

                posTransaction.EntryStatus = TransactionStatus.Cancelled;

                journalDialogResults = JournalDialogResults.PrintReceipt;
                journalDialogResultObject = selectedTransactionId;

                if (btnOkWasClickedOnPrintReceipt)
                {
                    ISettings settings = (ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                    if (!settings.FunctionalityProfile.KeepDailyJournalOpenAfterPrintingReceipt)
                    {
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(ex.ToString(), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        private void btnEmailReceipt_Click()
        {
            try
            {
                if (Interfaces.Services.SiteServiceService(DLLEntry.DataModel).TestConnectionWithFeedback(DLLEntry.DataModel, DLLEntry.Settings.SiteServiceProfile.SiteServiceAddress, (ushort)DLLEntry.Settings.SiteServiceProfile.SiteServicePortNumber) != ConnectionEnum.Success)
                {                    
                    DialogResult = DialogResult.None;
                    return;
                }

                if (DLLEntry.Settings.SiteServiceProfile.SendReceiptEmails == ReceiptEmailOptionsEnum.Never)
                {
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.POSHasBeenConfiguredToNeverSendEmails);
                    DialogResult = DialogResult.None;
                    return;
                }

                PosTransaction transaction = LoadTransaction(selectedTransactionId.PrimaryID, selectedTransactionId.SecondaryID.PrimaryID, selectedTransactionId.SecondaryID.SecondaryID.PrimaryID);

                if (transaction is DepositTransaction)
                {
                    GetCustomerOrderInformation(transaction, ReprintTypeEnum.ReceiptCopy);
                }

                Interfaces.Services.TransactionService(DLLEntry.DataModel).CreateAndSendEmailReceipt(DLLEntry.DataModel, transaction);
                
                posTransaction.EntryStatus = TransactionStatus.Cancelled;

                journalDialogResults = JournalDialogResults.PrintReceipt;
                journalDialogResultObject = selectedTransactionId;

                Close();
            }
            catch (Exception ex)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(ex.ToString(), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        private void GetCustomerOrderInformation(PosTransaction transaction, ReprintTypeEnum reprintType)
        {
            if (!(transaction is DepositTransaction))
            {
                return;
            }

            bool searchForOrder = true;
            List<CustomerOrder> currentCustomerOrderList = new List<CustomerOrder>();

            if (Interfaces.Services.SiteServiceService(DLLEntry.DataModel).TestConnectionWithFeedback(DLLEntry.DataModel, DLLEntry.Settings.SiteServiceProfile.SiteServiceAddress, (ushort)DLLEntry.Settings.SiteServiceProfile.SiteServicePortNumber, Resources.CustomerOrderInfoWillBeMissing.Replace("#1", reprintType == ReprintTypeEnum.ReceiptCopy ? Resources.ReceiptCopyLowerCase : Resources.InvoiceLowerCase)) != ConnectionEnum.Success)
            {                                                                                                
                searchForOrder = false;
            }

            int orderCount = 0;

            if (searchForOrder)
            {
                CustomerOrderSearch searchCriteria = new CustomerOrderSearch();

                searchCriteria.ID = ((DepositTransaction)transaction).CustomerOrder.ID;

                currentCustomerOrderList = Interfaces.Services.CustomerOrderService(DLLEntry.DataModel).Search(DLLEntry.DataModel,
                    out orderCount,
                    300,
                    searchCriteria
                    );

                CustomerOrder order = currentCustomerOrderList.FirstOrDefault(f => f.ID == searchCriteria.ID);
                if (order == null)
                {
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.NoCustomerOrderInfoAvailable + " "
                                                                                      + Resources.CustomerOrderInfoWillBeMissing.Replace("#1", reprintType == ReprintTypeEnum.ReceiptCopy ? Resources.ReceiptCopyLowerCase : Resources.InvoiceLowerCase));
                }
                else if (order != null)
                {
                    List<CustomerOrderAdditionalConfigurations> configurations = Providers.CustomerOrderAdditionalConfigData.GetList(DLLEntry.DataModel);
                    ((DepositTransaction)transaction).CustomerOrder.Reference = order.Reference;

                    CustomerOrderAdditionalConfigurations config = configurations.FirstOrDefault(f => f.ID == order.Delivery);
                    if (config != null)
                    {
                        ((DepositTransaction) transaction).CustomerOrder.Delivery = new CustomerOrderAdditionalConfigurations(config.ID, config.Text, config.AdditionalType);
                    }

                    config = configurations.FirstOrDefault(f => f.ID == order.Source);
                    if (config != null)
                    {
                        ((DepositTransaction)transaction).CustomerOrder.Source = new CustomerOrderAdditionalConfigurations(config.ID, config.Text, config.AdditionalType);
                    }

                    ((DepositTransaction)transaction).CustomerOrder.DeliveryLocation = order.DeliveryLocation;
                    
                    Store store = Providers.StoreData.Get(DLLEntry.DataModel, order.DeliveryLocation, CacheType.CacheTypeApplicationLifeTime);
                    if (store != null)
                    {
                        ((DepositTransaction) transaction).CustomerOrder.DeliveryLocationText = store.Text;
                    }

                    ((DepositTransaction)transaction).CustomerOrder.ExpirationDate = order.ExpiryDate;
                    ((DepositTransaction)transaction).CustomerOrder.Comment = order.Comment;
                }
            }
        }

        private void btnInvoice_Click()
        {
            try
            {
                PosTransaction transaction = LoadTransaction(selectedTransactionId.PrimaryID, selectedTransactionId.SecondaryID.PrimaryID, selectedTransactionId.SecondaryID.SecondaryID.PrimaryID);

                if (transaction is DepositTransaction)
                {
                    GetCustomerOrderInformation(transaction, ReprintTypeEnum.Invoice);
                }

                bool doPrinting = true;

                var fiscalService = (IFiscalService)DLLEntry.DataModel.Service(ServiceType.FiscalService);
                if (fiscalService != null && fiscalService.IsActive())
                {
                    doPrinting = fiscalService.PrintReceiptCopy(DLLEntry.DataModel, transaction, ReprintTypeEnum.Invoice);
                }

                if (doPrinting && PrintSlip(transaction))
                {
                    SaveReceiptCopyInformation(transaction, ReprintTypeEnum.Invoice);
                }

                posTransaction.EntryStatus = TransactionStatus.Cancelled;

                journalDialogResults = JournalDialogResults.PrintInvoice;
                journalDialogResultObject = selectedTransactionId;

                Close();
            }
            catch (Exception ex)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(ex.ToString(), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        private void btnReturnTransaction_Click()
        {
            string receiptId = (string)journalEntries[lvJournal.Selection.FirstSelectedRow].ReceiptID;

            journalDialogResults = JournalDialogResults.ReturnTransaction;
            journalDialogResultObject = receiptId;

            Close();
        }

        private void btnTaxFree_Click()
        {
            journalDialogResults = JournalDialogResults.TaxFree;
            journalDialogResultObject = selectedTransactionId;

            Close();
        }

        private void btnClose_Click()
        {
            journalDialogResults = JournalDialogResults.Close;
            journalDialogResultObject = null;
            Close();
        }

        private void ReceiptOnPreDisplayReceiptItemEvent(PreDisplayReceiptItemArgs e)
        {
            Interfaces.Services.EventService(DLLEntry.DataModel).PreDisplayReceiptItem(e);
        }

        private void btnSelect_Click()
        {
            journalDialogResults = JournalDialogResults.Ok;
            journalDialogResultObject = journalEntries[lvJournal.Selection.FirstSelectedRow];

            Close();
        }

        private void panel_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((Buttons)args.Tag)
            {
                case Buttons.PageUp: { lvJournal.MovePageUp(); break; }
                case Buttons.SelectionUp: { lvJournal.MoveSelectionUp(); break; }
                case Buttons.SelectionDown: { lvJournal.MoveSelectionDown(); break; }
                case Buttons.PageDown: { lvJournal.MovePageDown(); break; }
                case Buttons.Clear: { btnClear_Click(); break; }
                case Buttons.Select: { btnSelect_Click(); break; }
                case Buttons.Close: { btnClose_Click(); break; }
                case Buttons.Search: { btnSearch_Click(); break; }
                case Buttons.Invoice: { btnInvoice_Click(); break; }
                case Buttons.Receipt: { btnReceipt_Click(); break; }
                case Buttons.GiftReceipt: { btnGiftReceipt_Click(); break; }
                case Buttons.EmailReceipt: { btnEmailReceipt_Click(); break; }
                case Buttons.ReturnTransaction: { btnReturnTransaction_Click(); break; }
                case Buttons.TaxFree: { btnTaxFree_Click(); break; }
            }
        }

        private void SaveReceiptCopyInformation(PosTransaction transaction, ReprintTypeEnum reprintType)
        {
            //Save down information of the reprint 
            ReprintInfo reprintInfo = new ReprintInfo();
            if (!(transaction is RetailTransaction))
            {
                transaction = LoadTransaction(transaction.TransactionId, transaction.StoreId, transaction.TerminalId, true);                
            }
            reprintInfo.LineID = ((RetailTransaction)transaction).Reprints.Count + 1;
            reprintInfo.Staff = DLLEntry.DataModel.CurrentStaffID;
            reprintInfo.Store = DLLEntry.DataModel.CurrentStoreID;
            reprintInfo.Terminal = DLLEntry.DataModel.CurrentTerminalID;
            reprintInfo.ReprintDate = Date.Now;
            reprintInfo.ReprintType = reprintType;

            TransactionProviders.ReprintTransactionData.Insert(DLLEntry.DataModel, reprintInfo, transaction);
        }

        private void btnGiftReceipt_Click()
        {
            string receiptId = (string)journalEntries[lvJournal.Selection.FirstSelectedRow].ReceiptID;

            this.journalDialogResults = JournalDialogResults.GiftReceipt;
            this.journalDialogResultObject = receiptId;

            Close();
        }

        private void pnlCustomerInfo_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, pnlCustomerInfo.Width - 1, pnlCustomerInfo.Height - 1);
            p.Dispose();
        }
    }
}