using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Dialogs
{
    public partial class TaxRefundTransactionsDialog : TouchBaseForm
    {
        private IPosTransaction transaction;
        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        private enum Buttons
        {
            PageUp,
            SelectionUp,
            SelectionDown,
            PageDown,
            Search,
            Remove,
            OK,
            Cancel
        }

        public List<Transaction> SelectedTransaction { get; private set; }

        public TaxRefundTransactionsDialog(IConnectionManager entry)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            lvSelection.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
        }

        public TaxRefundTransactionsDialog(IConnectionManager entry, IPosTransaction transaction) : this(entry)
        {
            this.transaction = transaction;
            AddButtons();
        }

        private void touchKeyboard_ObtainCultureName(object sender, CultureEventArguments args)
        {
            if (dlgSettings.UserProfile.KeyboardCode != "")
            {
                args.CultureName = dlgSettings.UserProfile.KeyboardCode;
                args.LayoutName = dlgSettings.UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = dlgSettings.Store.KeyboardCode;
                args.LayoutName = dlgSettings.Store.KeyboardLayoutName;
            }
        }

        private void Search()
        {
            object result = null;

            IDialogService service = (IDialogService)dlgEntry.Service(ServiceType.DialogService);
			if (tbSearch.Text != "")            {
                
                BarcodeReceiptParseInfo parseInfo = Interfaces.Services.BarcodeService(dlgEntry).ParseBarcodeReceipt(dlgEntry, tbSearch.Text);

                var transactions = Providers.TransactionData.GetJournalTransactions(dlgEntry,
                    1,
                    dlgSettings.Terminal.ID,
                    DateTime.Today,
                    DateTime.Today.AddDays(1),
                    parseInfo.ReceiptID,
                    null,
                    dlgSettings.Store.ID);
                if (transactions != null && transactions.Count > 0)
                {
                    if (!IsDateToday(transactions[0].TransactionDate.DateTime))
                    {
                        service.ShowMessage(Resources.OnlyTodaysTransaction, Resources.TaxRefund, MessageBoxButtons.OK);
                        tbSearch.Text = "";
                        return;
                    }
                    result = transactions[0];
                }
            }
            else
            {
                if (service != null)
                {
                    var results = JournalDialogResults.Close;
                    service.ShowJournalDialog(dlgEntry, ref results, ref result, JournalOperations.Select, transaction);
                    if (results != JournalDialogResults.Ok)
                    {
                        return;
                    }
                    if (result != null && !IsDateToday(((Transaction)result).TransactionDate.DateTime))
                    {
                        service.ShowMessage(Resources.OnlyTodaysTransaction, Resources.TaxRefund, MessageBoxButtons.OK);
                        return;
                    }
                }
            }
            if (result != null)
            {
                
                var trans = result as Transaction;

                foreach (Row row in lvSelection.Rows)
                {
                    if (((Transaction) row.Tag).TransactionID == trans.TransactionID &&
                        ((Transaction) row.Tag).StoreID == trans.StoreID &&
                        ((Transaction) row.Tag).TerminalID == trans.TerminalID)
                    {
                        service.ShowMessage(Resources.TransactionBeenAdded);
                        tbSearch.Text = "";
                        return;
                    }
                }

                if (Providers.TaxRefundTransactionData.ExistsForTransactionID(dlgEntry, trans.TransactionID, false))
                {
                    TaxRefundTransaction oldRefundTransaction = Providers.TaxRefundTransactionData.GetForTransactionID(dlgEntry, trans.TransactionID,false, CacheType.CacheTypeApplicationLifeTime);
                    if (!OldReportRemoved(oldRefundTransaction)) return;
                }
                bool priceWithTax = dlgSettings.Store.StorePriceSetting == Store.StorePriceSettingsEnum.PricesIncludeTax;
                if (trans != null && ((priceWithTax && trans.NetAmountWithTax < 0) || (!priceWithTax && trans.NetAmount < 0)))
                {
                    var row = new Row();
                    row.AddText((string) trans.ReceiptID);
                    row.AddText(trans.TransactionDate.DateTime.ToString("d", dlgSettings.CultureInfo) + " " + trans.TransactionDate.DateTime.ToString("t", dlgSettings.CultureInfo));
                    
                    var roundingService = (IRoundingService) dlgEntry.Service(ServiceType.RoundingService);
                    row.AddText(roundingService.RoundForDisplay(dlgEntry, (priceWithTax ? trans.NetAmountWithTax : trans.NetAmount)*-1, true, true, trans.CurrencyID, CacheType.CacheTypeApplicationLifeTime));
                    row.Tag = trans;
                    lvSelection.AddRow(row);
                    tbSearch.Text = "";
                }
                lvSelection.ApplyRelativeColumnSize();
                if (lvSelection.Selection.Count < 1)
                {
                    lvSelection.MoveSelectionUp();
                }
            }
        }

        private bool OldReportRemoved(TaxRefundTransaction oldRefundTransaction)
        {
            IDialogService service = (IDialogService)dlgEntry.Service(ServiceType.DialogService);
            var result = service.ShowMessage(Resources.TransactionAlreadyRefunded, Resources.TransactionRefunded, MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return false;
            TaxRefund oldRefund = Providers.TaxRefundData.Get(dlgEntry, oldRefundTransaction.TaxRefundID);
            oldRefund.Status = TaxRefundStatus.Canceled;
            ISiteServiceService storeServerService = Interfaces.Services.SiteServiceService(dlgEntry);
            if (storeServerService != null)
            {
                storeServerService.SaveTaxRefund(dlgEntry, dlgSettings.SiteServiceProfile, oldRefund);
            }
            return true;
        }

        private bool IsDateToday(DateTime dateTime)
        {
            return dateTime >= DateTime.Today && dateTime < DateTime.Today.AddDays(1);
        }

        private void lvSelection_SelectionChanged(object sender, EventArgs e)
        {
            touchScrollButtonPanel1.SetButtonEnabled(Conversion.ToStr((int)Buttons.Remove), lvSelection.Selection.Count > 0);
            touchScrollButtonPanel1.SetButtonEnabled(Conversion.ToStr((int)Buttons.OK), lvSelection.Selection.Count > 0);
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                Search();
            }
        }

        private void OkClicked()
        {
            var result = new List<Transaction>();
            foreach (Row row in lvSelection.Rows)
            {
                result.Add((Transaction)row.Tag);
            }
            SelectedTransaction = result;
            
            //List<TaxRefundTransaction> transactions = null;
            //decimal totalAmount = 0M;
            //decimal totalTax = 0M;
            //var range = RefundDialog.GetRangeForTransactions(dlgEntry, result, ref transactions, ref totalAmount, ref totalTax);

            TaxRefundRange range = new TaxRefundRange() { ValueFrom = 0, ValueTo = 5000, TaxRefundPercentage = 5 };

            if (range != null)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                var service = (IDialogService) dlgEntry.Service(ServiceType.DialogService);
                service.ShowMessage(Resources.RefundRangeNotFound, MessageBoxButtons.OK, MessageDialogType.Attention);
            }
        }

        private void AddButtons()
        {
            touchScrollButtonPanel1.AddButton("", Buttons.PageUp, "", image: Resources.Doublearrowupthin_32px);
            touchScrollButtonPanel1.AddButton("", Buttons.SelectionUp, "", image: Resources.Arrowupthin_32px);
            touchScrollButtonPanel1.AddButton("", Buttons.SelectionDown, "", image: Resources.Arrowdownthin_32px);
            touchScrollButtonPanel1.AddButton("", Buttons.PageDown, "", image: Resources.Doublearrowdownthin_32px);

            touchScrollButtonPanel1.AddButton(Resources.Search, Buttons.Search, "", TouchButtonType.Action, DockEnum.DockEnd);
            touchScrollButtonPanel1.AddButton(Resources.Remove, Buttons.Remove, Conversion.ToStr((int)Buttons.Remove), TouchButtonType.Normal, DockEnum.DockEnd);
            touchScrollButtonPanel1.AddButton(Resources.OK, Buttons.OK, Conversion.ToStr((int)Buttons.OK), TouchButtonType.OK, DockEnum.DockEnd);
            touchScrollButtonPanel1.AddButton(Resources.Cancel, Buttons.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);

            touchScrollButtonPanel1.SetButtonEnabled(Conversion.ToStr((int)Buttons.OK), false);
            touchScrollButtonPanel1.SetButtonEnabled(Conversion.ToStr((int)Buttons.Remove), false);
        }

        private void touchScrollButtonPanel1_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((Buttons)args.Tag)
            {
                case Buttons.PageUp:
                    lvSelection.MovePageUp();
                    break;
                case Buttons.SelectionUp:
                    lvSelection.MoveSelectionUp();
                    break;
                case Buttons.SelectionDown:
                    lvSelection.MoveSelectionDown();
                    break;
                case Buttons.PageDown:
                    lvSelection.MovePageDown();
                    break;
                case Buttons.Search:
                    Search();
                    break;
                case Buttons.Remove:
                    lvSelection.RemoveRow(lvSelection.Selection.FirstSelectedRow);
                    break;
                case Buttons.OK:
                    OkClicked();
                    break;
                case Buttons.Cancel:
                    DialogResult = DialogResult.Cancel;
                    Close();
                    break;
            }
        }
    }
}
