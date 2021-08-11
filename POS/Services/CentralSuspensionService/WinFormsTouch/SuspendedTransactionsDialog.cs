using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Peripherals;
using LSOne.POS.Processes.WinControls;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    /// <summary>
    /// Suspended transactions dialog
    /// </summary>
    public partial class SuspendedTransactionsDialog : TouchBaseForm
    {
        private enum Buttons
        {
            PageUp,
            SelectionUp,
            SelectionDown,
            PageDown,
            MoveToCloud,
            ExpandCollapse,
            Search,
            Clear,
            Select,
            Close
        }

        private ReceiptItems receiptItems;
        private ISettings settings;
        private IConnectionManager dlgEntry;

        private Lazy<SearchDialog> searchDialog;

        private List<SuspendedTransaction> transactionList;
        private List<SuspendedTransaction> filteredTransactionList;
        private List<SuspendedTransactionAnswer> answers;
        private bool useCentralSuspension;
        private bool doEvents;
        private bool rowsExpanded;
        private RecordIdentifier selectedTransactionID;
        private ColumnCollection subRowColumnCollection;

        private Buttons? lastPressedButton;
        private int lastScrollValue;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SuspendedTransactionsDialog(IConnectionManager entry, ISettings settings, List<SuspendedTransaction> suspendedTransactions,
                                List<SuspendedTransactionAnswer> answers, bool useCentralSuspension)
        {
            InitializeComponent();

            DoubleBuffered = true;
            rowsExpanded = false;
            doEvents = false;

            if (!DesignMode)
            {
                receiptItems = new ReceiptControlFactory().CreateReceiptItemsControl(pnlReceipt);
            }

            transactionList = suspendedTransactions;
            dlgEntry = entry;
            this.answers = answers;
            this.settings = settings;
            this.useCentralSuspension = useCentralSuspension;

            AddButtons();

            Width = settings.MainFormInfo.MainWindowWidth;
            Height = settings.MainFormInfo.MainWindowHeight;
            searchDialog = new Lazy<SearchDialog>(() => new SearchDialog(dlgEntry));
        }

        /// <summary>
        /// Selected suspended transaction
        /// </summary>
        public SuspendedTransaction SelectedSuspendedTransaction { get; private set; }

        private void AddButtons()
        {
            panelButtons.AddButton("", Buttons.PageUp, "", image: Properties.Resources.Doublearrowupthin_32px);
            panelButtons.AddButton("", Buttons.SelectionUp, "", image: Properties.Resources.Arrowupthin_32px);
            panelButtons.AddButton("", Buttons.SelectionDown, "", image: Properties.Resources.Arrowdownthin_32px);
            panelButtons.AddButton("", Buttons.PageDown, "", image: Properties.Resources.Doublearrowdownthin_32px);

            if (useCentralSuspension)
            {
                panelButtons.AddButton("", Buttons.MoveToCloud, Conversion.ToStr((int)Buttons.MoveToCloud), image: Properties.Resources.Movetocloud2_32px);
            }

            panelButtons.AddButton(Properties.Resources.ExpandAll, Buttons.ExpandCollapse, Conversion.ToStr((int)Buttons.ExpandCollapse));
            panelButtons.AddButton(Properties.Resources.Search, Buttons.Search, "", TouchButtonType.Action);
            panelButtons.AddButton(Properties.Resources.Clear, Buttons.Clear, Conversion.ToStr((int)Buttons.Clear));
            panelButtons.AddButton(Properties.Resources.Select, Buttons.Select, "", TouchButtonType.OK, DockEnum.DockEnd);
            panelButtons.AddButton(Properties.Resources.Close, Buttons.Close, "", TouchButtonType.Cancel, DockEnum.DockEnd);

            panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.MoveToCloud), false);
            panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), false);
        }

        private void panelButtons_Click(object sender, ScrollButtonEventArguments args)
        {
            lastPressedButton = (Buttons)args.Tag;

            switch ((Buttons)args.Tag)
            {
                case Buttons.PageUp:
                    lvSuspendedTransactions.MovePageUp();
                    break;
                case Buttons.SelectionUp:
                    lvSuspendedTransactions.MoveSelectionUp();
                    break;
                case Buttons.SelectionDown:
                    lvSuspendedTransactions.MoveSelectionDown();
                    break;
                case Buttons.PageDown:
                    lvSuspendedTransactions.MovePageDown();
                    break;
                case Buttons.MoveToCloud:
                    MoveTransactionToCloud();
                    break;
                case Buttons.ExpandCollapse:
                    ExpandOrCollapseAnswers();
                    break;
                case Buttons.Search:
                    Search();
                    break;
                case Buttons.Clear:
                    ClearSearch();
                    break;
                case Buttons.Select:
                    SelectTransaction();
                    break;
                case Buttons.Close:
                    DialogResult = DialogResult.Cancel;
                    Close();
                    break;
            }
        }

        private void Search()
        {
            SearchDialog dlg = searchDialog.Value;
            
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), true);

                filteredTransactionList.Clear();
                filteredTransactionList.AddRange(
                    from t in transactionList
                    where (string.IsNullOrEmpty(dlg.SearchText) || t.ID.StringValue.Contains(dlg.SearchText))
                    && (RecordIdentifier.IsEmptyOrNull(dlg.StaffID) || t.StaffID == dlg.StaffID)
                    && (RecordIdentifier.IsEmptyOrNull(dlg.TerminalID) || t.TerminalID == dlg.TerminalID)
                    && (!dlg.SelectedFromDate.HasValue || t.TransactionDate >= dlg.SelectedFromDate.Value)
                    && (!dlg.SelectedToDate.HasValue || t.TransactionDate <= dlg.SelectedToDate.Value)
                    && (!dlg.MinAmount.HasValue || t.BalanceWithTax >= dlg.MinAmount.Value)
                    && (!dlg.MaxAmount.HasValue || t.BalanceWithTax <= dlg.MaxAmount.Value)
                    select t);

                //Filter all answers
                if(dlg.SearchText != "")
                {
                    filteredTransactionList.AddRange(transactionList.Except(filteredTransactionList).Where(x => answers.Any(y => y.TransactionID == x.ID && y.ToString().Contains(dlg.SearchText))).ToList());
                }

                LoadItems();

                if(filteredTransactionList.Any())
                {
                    lvSuspendedTransactions.MoveSelectionUp();
                }
                else
                {
                    DisplayItemsOnReceipt(null);
                }
            }
        }

        private void ClearSearch()
        {
            searchDialog = new Lazy<SearchDialog>(() => new SearchDialog(dlgEntry));
            panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), false);
            filteredTransactionList.Clear();
            filteredTransactionList.AddRange(transactionList);
            LoadItems();
        }

        private void MoveTransactionToCloud()
        {
            bool connectionFailed = false;

            ISiteServiceService service = null;

            try
            {
                Interfaces.Services.DialogService(dlgEntry).ShowStatusDialog(Properties.Resources.TestingConnection);
                service = Interfaces.Services.SiteServiceService(dlgEntry);
                ConnectionEnum result = service.TestConnectionWithFeedback(dlgEntry, dlgEntry.SiteServiceAddress, dlgEntry.SiteServicePortNumber);
                Interfaces.Services.DialogService(dlgEntry).CloseStatusDialog();

                if (result != ConnectionEnum.Success)
                {
                    connectionFailed = true;
                }
            }
            catch
            {
                Interfaces.Services.DialogService(dlgEntry).CloseStatusDialog();
                connectionFailed = true;
            }

            if (connectionFailed)
            {                
                return;
            }

            SuspendedTransaction selectedTransaction = (SuspendedTransaction)lvSuspendedTransactions.Selection[0].Tag;
            RecordIdentifier suspendedTransactionID = RecordIdentifier.Empty;

            try
            {

                List<SuspendedTransactionAnswer> answers = Providers.SuspendedTransactionAnswerData.GetList(dlgEntry, selectedTransaction.ID);
                Interfaces.Services.DialogService(dlgEntry).ShowStatusDialog(Properties.Resources.TransferingTransaction);

                suspendedTransactionID = service.SuspendTransaction(dlgEntry,
                                                         settings.SiteServiceProfile,
                                                         RecordIdentifier.Empty,
                                                         selectedTransaction.SuspensionTypeID,
                                                         selectedTransaction.TransactionXML,
                                                         selectedTransaction.Balance,
                                                         selectedTransaction.BalanceWithTax,
                                                         answers,
                                                         true);

                Interfaces.Services.DialogService(dlgEntry).CloseStatusDialog();

                Providers.SuspendedTransactionData.Delete(dlgEntry, selectedTransaction.ID);

                foreach (SuspendedTransactionAnswer answer in answers)
                {
                    Providers.SuspendedTransactionAnswerData.Delete(dlgEntry, answer.ID);
                }

                selectedTransaction.ID = suspendedTransactionID;
                selectedTransaction.IsLocallySuspended = false;
                panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.MoveToCloud), false);

                LoadItems();
            }
            catch
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.UnableToTransferTransaction, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                service.DeleteSuspendedTransaction(dlgEntry, settings.SiteServiceProfile, suspendedTransactionID, true);
            }
            finally
            {
                Interfaces.Services.DialogService(dlgEntry).CloseStatusDialog();
            }
        }

        private void ExpandOrCollapseAnswers()
        {
            if(rowsExpanded)
            {
                panelButtons.SetButtonCaption(Conversion.ToStr((int)Buttons.ExpandCollapse), Properties.Resources.ExpandAll);
                rowsExpanded = false;
            }
            else
            {
                panelButtons.SetButtonCaption(Conversion.ToStr((int)Buttons.ExpandCollapse), Properties.Resources.CollapseAll);
                rowsExpanded = true;
            }

            LoadItems();
        }

        private void SelectTransaction()
        {
            if (lvSuspendedTransactions.Selection.Count > 0)
            {
                SelectedSuspendedTransaction = (SuspendedTransaction)lvSuspendedTransactions.Selection[0].Tag;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void SuspendedTransactionsDialog_Load(object sender, EventArgs e)
        {
            Top = settings.MainFormInfo.MainWindowTop;
            Left = settings.MainFormInfo.MainWindowLeft;

            try
            {
                Scanner.ScannerMessageEvent += ProcessScannedItem;
                Scanner.ReEnableForScan();
            }
            catch { }

            if(transactionList.Count > 0)
            {
                SuspendedTransaction suspendedTransaction = transactionList[0];
                selectedTransactionID = suspendedTransaction.ID;
                DisplayItemsOnReceipt(suspendedTransaction);
                panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.MoveToCloud), suspendedTransaction.IsLocallySuspended);
            }
            else
            {
                DisplayItemsOnReceipt(null);
            }

            subRowColumnCollection = new ColumnCollection();
            short answerHeaderColumnWidth = (short)Math.Min(300, lvSuspendedTransactions.Width * 0.40);
            Column answerHeaderColumn = new Column("", answerHeaderColumnWidth, answerHeaderColumnWidth, 0, 0, true) { EmphasizeText = true };
            Column answerColumn = new Column("", (short)(lvSuspendedTransactions.Width - answerHeaderColumnWidth), (short)(lvSuspendedTransactions.Width - answerHeaderColumnWidth), 0, 0, true);

            Style headerStyle = new Style(lvSuspendedTransactions.DefaultHeaderStyle)
            {
                BackColor = ColorPalette.POSSelectedRowColor,
                BorderColor = ColorPalette.POSControlBorderColor,
                BorderBottom = true,
                BorderRight = true,
                BorderWith = 1
            };

            Style style = new Style(lvSuspendedTransactions.DefaultStyle)
            {
                BackColor = ColorPalette.POSDialogBackgroundColor,
                BorderColor = ColorPalette.POSControlBorderColor,
                BorderBottom = true,
                BorderRight = true,
                BorderLeft = true,
                BorderWith = 1
            };

            answerHeaderColumn.DefaultStyle = headerStyle;
            answerColumn.DefaultStyle = style;

            subRowColumnCollection.Add(answerHeaderColumn);
            subRowColumnCollection.Add(answerColumn);

            filteredTransactionList = new List<SuspendedTransaction>();
            filteredTransactionList.AddRange(transactionList);
            LoadItems();
            lvSuspendedTransactions.Focus();
        }

        private void LoadItems()
        {
            doEvents = false;
            lvSuspendedTransactions.ClearRows();

            DecimalLimit limit = dlgEntry.GetDecimalSetting(DecimalSettingEnum.Prices);

            Row row;
            List<SubRow> subrows;
            bool rowIsExpanded;
            bool hasAnswers;

            foreach(SuspendedTransaction suspendedTransaction in filteredTransactionList)
            {
                row = new Row();
                subrows = new List<SubRow>();
                rowIsExpanded = false;
                hasAnswers = false;

                List<SuspendedTransactionAnswer> transactionAnswers = answers.FindAll(x => x.TransactionID == suspendedTransaction.ID && !x.IsEmpty());
                hasAnswers = transactionAnswers.Any();

                panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.ExpandCollapse), hasAnswers);

                //Show "expanded" subrows if there are any answers
                if (hasAnswers && (suspendedTransaction.ID == selectedTransactionID || rowsExpanded))
                {
                    foreach (SuspendedTransactionAnswer suspendedTransactionAnswer in transactionAnswers)
                    {
                        SubRow subRow = new SubRow(subRowColumnCollection, 0);
                        subRow.Height = 35;
                        subRow.Selectable = false;
                        subRow.AddText(suspendedTransactionAnswer.Prompt);
                        subRow.AddText(suspendedTransactionAnswer.ToString(dlgEntry.Settings.LocalizationContext));
                        subrows.Add(subRow);
                    }

                    rowIsExpanded = true;
                }

                row.AddCell(new ImageCell(hasAnswers ? rowIsExpanded ? Properties.Resources.Arrowupthin_24px : Properties.Resources.Arrowdownthin_24px : null, 0, 0, false));
                row.AddText(suspendedTransaction.ID.StringValue);
                row.AddText(suspendedTransaction.TransactionDate.ToShortDateString() + " " + suspendedTransaction.TransactionDate.ToShortTimeString());
                row.AddText(suspendedTransaction.TerminalName);
                row.AddText(suspendedTransaction.StaffID.StringValue);
                row.AddText(suspendedTransaction.BalanceWithTax.FormatWithLimits(limit, false));
                row.AddCell(new ImageCell(suspendedTransaction.IsLocallySuspended ? Properties.Resources.Local_24px : Properties.Resources.Cloud_24px, 0, 0, false));

                row.Tag = suspendedTransaction;
                lvSuspendedTransactions.AddRow(row);

                if (suspendedTransaction.ID == selectedTransactionID)
                {
                    lvSuspendedTransactions.Selection.Set(lvSuspendedTransactions.RowCount - 1);
                }

                subrows.ForEach(x => lvSuspendedTransactions.AddRow(x));

                if (suspendedTransaction.ID == selectedTransactionID)
                {
                    lvSuspendedTransactions.ScrollRowIntoView(lvSuspendedTransactions.RowCount - 1);
                }
            }

            lvSuspendedTransactions.AutoSizeColumns();
            doEvents = true;
        }

        private void ProcessScannedItem(ScanInfo scanInfo)
        {
            try
            {
                Scanner.DisableForScan();

                SelectedSuspendedTransaction = transactionList.FirstOrDefault(x => x.ID.StringValue == scanInfo.ScanDataLabel);

                if(SelectedSuspendedTransaction != null)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.TransactionScannedNotFound, MessageBoxButtons.OK, MessageDialogType.Generic);
                }
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Scanner.ScannerMessageEvent -= ProcessScannedItem;
            Scanner.DisableForScan();
            base.OnFormClosing(e);
        }

        private void lvSuspendedTransactions_SelectionChanged(object sender, EventArgs e)
        {
            if (!doEvents) return;

            SuspendedTransaction selectedTransaction = (SuspendedTransaction)lvSuspendedTransactions.Selection[0].Tag;
            
            if(selectedTransaction != null && selectedTransaction.ID != selectedTransactionID)
            {
                lastScrollValue = lvSuspendedTransactions.VerticalScrollbarValue;
                selectedTransactionID = selectedTransaction.ID;
                LoadItems();

                if(useCentralSuspension)
                {
                    panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.MoveToCloud), selectedTransaction.IsLocallySuspended);
                }

                DisplayItemsOnReceipt(selectedTransaction);

                ScrollToSelectedRow();
            }
        }

        private void DisplayItemsOnReceipt(SuspendedTransaction transaction)
        {
            if(transaction != null)
            {
                RetailTransaction tempTransaction = new RetailTransaction((string)dlgEntry.CurrentStoreID, settings.Store.Currency, settings.TaxIncludedInPrice);
                tempTransaction.NetAmount = transaction.Balance;
                tempTransaction.NetAmountWithTax = transaction.BalanceWithTax;
                LinkedList<ISaleLineItem> saleLineItems = CentralSuspensionService.CreateSaleLineItems(dlgEntry, transaction.TransactionXML, tempTransaction);

                receiptItems.DisplayRTItems(tempTransaction, saleLineItems);
            }
            else
            {
                receiptItems.DisplayRTItems(null);
            }
        }

        private void lvSuspendedTransactions_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            SelectTransaction();
        }

        private void ScrollToSelectedRow()
        {
            if (selectedTransactionID != null)
            {
                lvSuspendedTransactions.VerticalScrollbarValue = lastScrollValue;

                if(lastPressedButton.HasValue)
                {
                    if (lastPressedButton.Value == Buttons.SelectionDown)
                    {
                        // Add the answers to the selection to always be visible when scrolling down
                        int transactionAnswersCount = answers.Count(x => x.TransactionID == selectedTransactionID && !x.IsEmpty());
                        lvSuspendedTransactions.ScrollRowIntoView(lvSuspendedTransactions.Selection.FirstSelectedRow + transactionAnswersCount);
                    }
                    else if (lastPressedButton.Value == Buttons.PageUp || lastPressedButton == Buttons.PageDown)
                    {
                        lvSuspendedTransactions.ScrollRowIntoView(lvSuspendedTransactions.Selection.FirstSelectedRow);
                    }

                    lastPressedButton = null;
                }
            }
        }

        private void lvSuspendedTransactions_KeyDown(object sender, KeyEventArgs e)
        {
            // These keys will trigger the SelectedChanged event on the list view and are used to determine how to scroll to the new selected item
            if(e.KeyCode == Keys.PageUp)
            {
                lastPressedButton = Buttons.PageUp;
            }
            else if(e.KeyCode == Keys.PageDown)
            {
                lastPressedButton = Buttons.PageDown;
            }
            else if (e.KeyCode == Keys.Up)
            {
                lastPressedButton = Buttons.SelectionUp;
            }
            else if (e.KeyCode == Keys.Down)
            {
                lastPressedButton = Buttons.SelectionDown;
            }
        }
    }
}
