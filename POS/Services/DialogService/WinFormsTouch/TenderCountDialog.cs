using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.SupportClasses;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    public partial class TenderCountDialog : TouchBaseForm
    {
        private enum Buttons
        {
            Cash,
            Currency,
            Other,
            Submit,
            Cancel
        }

        private enum CashEntryMethod
        {
            /// <summary>
            /// Display only one cash line
            /// </summary>
            Single,
            /// <summary>
            /// Display one line for each cash denomination
            /// </summary>
            Multiple
        }

        private enum TenderEntryType
        {
            Cash,
            Currency,
            Other,
            Subtotal
        }

        private enum CountValidation
        {
            Valid,
            Invalid,
            InvalidContinue
        }

        private TenderEntryType activePage;
        private TenderCountTransaction transaction;
        private List<StorePaymentMethod> paymentMethodsRequiredForCounting;
        private List<PaymentTransaction> requiredPayments;
        private List<StorePaymentMethod> storePaymentMethods;
        private List<int> attemptsDone;
        private Currency storeCurrency;
        private CashEntryMethod cashEntryMethod;
        private Dictionary<TenderEntryType, List<TenderCountEntry>> tenderCounts;
        private IRoundingService rounding;
        private bool countingCash;
        private bool countingCurrency;
        private bool countingOther;
        private Style greyBackColorStyle;

        private const string SubtotalCashID = "CASH";
        private const string SubtotalCurrencyID = "CURRENCY";
        private const string SubtotalOtherID = "OTHER";
        private const string SubtotalTotalID = "TOTAL";

        private RecordIdentifier cashListSelectedID;
        private RecordIdentifier currencyListSelectedID;
        private RecordIdentifier otherListSelectedID;

        public TenderCountDialog(TenderCountTransaction transaction)
        {
            InitializeComponent();

            this.transaction = transaction;

            SetFormSize();

            cashEntryMethod = CashEntryMethod.Single;
            attemptsDone = new List<int>();

            storeCurrency = Providers.CurrencyData.Get(DLLEntry.DataModel, DLLEntry.Settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
            storePaymentMethods = Providers.StorePaymentMethodData.GetRecords(DLLEntry.DataModel, DLLEntry.DataModel.CurrentStoreID, false);

            if (!DesignMode)
            {
                rounding = (IRoundingService)DLLEntry.DataModel.Service(ServiceType.RoundingService);
            }

            InitializeFormInfo();

            activePage = TenderEntryType.Cash;
            SetupButtons();

            if(!countingCash)
            {
                if(countingCurrency)
                {
                    ShowListView(TenderEntryType.Currency);
                }
                else
                {
                    ShowListView(TenderEntryType.Other);
                }
            }
        }

        private void InitializeFormInfo()
        {
            greyBackColorStyle = new Style(lvCash.DefaultStyle)
            {
                BackColor = ColorPalette.POSSelectedRowColor
            };

            requiredPayments = new List<PaymentTransaction>();

            if (transaction is TenderDeclarationTransaction)
            {
                requiredPayments =
                    Providers.PaymentTransactionData.GetRequiredDropAmounts(DLLEntry.DataModel,
                    Providers.PaymentTransactionData.GetNextCountingDate(DLLEntry.DataModel,
                    (int)TypeOfTransaction.TenderDeclaration, DLLEntry.DataModel.CurrentTerminalID, DLLEntry.DataModel.CurrentStoreID),
                    DLLEntry.DataModel.CurrentTerminalID, DLLEntry.DataModel.CurrentStoreID);

                paymentMethodsRequiredForCounting = storePaymentMethods.FindAll(f => f.CountingRequired && f.DefaultFunction != PaymentMethodDefaultFunctionEnum.FloatTender);

                cashEntryMethod = DLLEntry.Settings.FunctionalityProfile.TenderDeclUsesDenomination ? CashEntryMethod.Multiple : CashEntryMethod.Single;
            }
            else if (transaction is BankDropTransaction)
            {
                paymentMethodsRequiredForCounting = storePaymentMethods.FindAll(f => f.AllowBankDrop);
                touchDialogBanner1.BannerText = Properties.Resources.BankDrop;
                cashEntryMethod = DLLEntry.Settings.FunctionalityProfile.BankDropUsesDenomination ? CashEntryMethod.Multiple : CashEntryMethod.Single;
            }
            else if (transaction is SafeDropTransaction)
            {
                paymentMethodsRequiredForCounting = storePaymentMethods.FindAll(f => f.AllowSafeDrop);
                touchDialogBanner1.BannerText = Properties.Resources.SafeDrop;
                cashEntryMethod = DLLEntry.Settings.FunctionalityProfile.SafeDropUsesDenomination ? CashEntryMethod.Multiple : CashEntryMethod.Single;
            }
            else if (transaction is SafeDropReversalTransaction)
            {
                paymentMethodsRequiredForCounting = storePaymentMethods.FindAll(f => f.AllowSafeDrop);
                touchDialogBanner1.BannerText = Properties.Resources.SafeDropReversion;
                cashEntryMethod = DLLEntry.Settings.FunctionalityProfile.SafeDropRevUsesDenomination ? CashEntryMethod.Multiple : CashEntryMethod.Single;
            }
            else if (transaction is BankDropReversalTransaction)
            {
                paymentMethodsRequiredForCounting = storePaymentMethods.FindAll(f => f.AllowBankDrop);
                touchDialogBanner1.BannerText = Properties.Resources.BankDropReversion;
                cashEntryMethod = DLLEntry.Settings.FunctionalityProfile.BankDropRevUsesDenomination ? CashEntryMethod.Multiple : CashEntryMethod.Single;
            }

            if (cashEntryMethod == CashEntryMethod.Single)
            {
                lvCash.Columns.RemoveAt(0);
                lvCash.Columns[0].HeaderText = Properties.Resources.Type;
                lvCash.Columns[0].DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Left;
                lvCash.Columns[0].RelativeSize = 25;
                lvCash.Columns[1].RelativeSize = 75;
            }

            lvCash.Columns[0].DefaultStyle = greyBackColorStyle;
            lvCurrency.Columns[0].DefaultStyle = greyBackColorStyle;
            lvCurrency.Columns[1].DefaultStyle = greyBackColorStyle;
            lvOther.Columns[0].DefaultStyle = greyBackColorStyle;
            lvSubtotal.Columns[0].DefaultStyle = greyBackColorStyle;

            lvSubtotal.Enabled = false;
            lvSubtotal.DisabledBackColor = ColorPalette.POSDialogBackgroundColor;

            InitTenderCounts();
        }

        private void InitTenderCounts()
        {
            tenderCounts = new Dictionary<TenderEntryType, List<TenderCountEntry>>();
            tenderCounts.Add(TenderEntryType.Cash, new List<TenderCountEntry>());
            tenderCounts.Add(TenderEntryType.Currency, new List<TenderCountEntry>());
            tenderCounts.Add(TenderEntryType.Other, new List<TenderCountEntry>());
            tenderCounts.Add(TenderEntryType.Subtotal, new List<TenderCountEntry>());

            tenderCounts[TenderEntryType.Subtotal].Add(new TenderCountEntry(SubtotalCashID) { Description = Properties.Resources.Cash, CurrencyCode = DLLEntry.Settings.Store.Currency });
            tenderCounts[TenderEntryType.Subtotal].Add(new TenderCountEntry(SubtotalCurrencyID) { Description = Properties.Resources.Currency, CurrencyCode = DLLEntry.Settings.Store.Currency });
            tenderCounts[TenderEntryType.Subtotal].Add(new TenderCountEntry(SubtotalOtherID) { Description = Properties.Resources.Other, CurrencyCode = DLLEntry.Settings.Store.Currency });
            tenderCounts[TenderEntryType.Subtotal].Add(new TenderCountEntry(SubtotalTotalID) { Description = Properties.Resources.Total, CurrencyCode = DLLEntry.Settings.Store.Currency });

            foreach (StorePaymentMethod storePaymentMethod in paymentMethodsRequiredForCounting)
            {
                if ((POSOperations)(int)storePaymentMethod.PosOperation == POSOperations.PayCash)
                {
                    countingCash = true;

                    if (cashEntryMethod == CashEntryMethod.Single)
                    {
                        tenderCounts[TenderEntryType.Cash].Add(new TenderCountEntry("")
                        {
                            Description = Properties.Resources.Cash,
                            CurrencyCode = storeCurrency.ID,
                            TenderID = storePaymentMethod.ID.SecondaryID.StringValue,
                            MaxDecimals = GetNumberOfDecimals(storeCurrency.RoundOffSales),
                            TenderName = storePaymentMethod.Text,
                            PosOperationID = (int)storePaymentMethod.PosOperation
                        });
                    }
                    else
                    {
                        CurrencyInfo currencyInfo = Interfaces.Services.CurrencyService(DLLEntry.DataModel).DetailedCurrencyInfo(DLLEntry.DataModel, DLLEntry.Settings.Store.Currency);
                        DecimalLimit decimalSetting = DLLEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

                        foreach (CashDenominator denomination in currencyInfo.CurrencyItems)
                        {
                            if (denomination.CashType == CashDenominator.Type.Coin || denomination.CashType == CashDenominator.Type.Bill)
                            {
                                tenderCounts[TenderEntryType.Cash].Add(new TenderCountEntry(denomination.ID)
                                {
                                    Description = denomination.Denomination != "" ?
                                        denomination.Denomination :
                                        rounding.RoundString(DLLEntry.DataModel, denomination.Amount, denomination.Amount < 1 ? decimalSetting.Max : decimalSetting.Min, true, DLLEntry.Settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime),
                                    CurrencyCode = storeCurrency.ID,
                                    DenominationAmount = denomination.Amount,
                                    TenderID = storePaymentMethod.ID.SecondaryID.StringValue,
                                    TenderName = storePaymentMethod.Text,
                                    PosOperationID = (int)storePaymentMethod.PosOperation
                                });
                            }
                        }
                    }
                }
                else if ((POSOperations)(int)storePaymentMethod.PosOperation == POSOperations.PayCurrency)
                {
                    countingCurrency = true;

                    List<Currency> foreignCurrencies = Providers.CurrencyData.GetNonStoreCurrencies(DLLEntry.DataModel, DLLEntry.Settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);

                    foreach (Currency currency in foreignCurrencies)
                    {
                        tenderCounts[TenderEntryType.Currency].Add(new TenderCountEntry(currency.ID)
                        {
                            Description = currency.Text,
                            CurrencyCode = currency.ID,
                            MaxDecimals = GetNumberOfDecimals(currency.RoundOffSales),
                            TenderID = storePaymentMethod.ID.SecondaryID.StringValue,
                            TenderName = storePaymentMethod.Text,
                            PosOperationID = (int)storePaymentMethod.PosOperation
                        });
                    }
                }
                else
                {
                    countingOther = true;

                    tenderCounts[TenderEntryType.Other].Add(new TenderCountEntry(storePaymentMethod.ID)
                    {
                        Description = storePaymentMethod.Text,
                        CurrencyCode = DLLEntry.Settings.Store.Currency,
                        MaxDecimals = GetNumberOfDecimals(storeCurrency.RoundOffSales),
                        TenderID = storePaymentMethod.ID.SecondaryID.StringValue,
                        TenderName = storePaymentMethod.Text,
                        PosOperationID = (int)storePaymentMethod.PosOperation
                    });
                }
            }
        }

        private void SetFormSize()
        {
            Width = DLLEntry.Settings.MainFormInfo.MainWindowWidth;
            Height = DLLEntry.Settings.MainFormInfo.MainWindowHeight;
            Top = DLLEntry.Settings.MainFormInfo.MainWindowTop;
            Left = DLLEntry.Settings.MainFormInfo.MainWindowLeft;
        }

        private void SetupButtons()
        {
            pnlButtons.Clear();

            if(countingCash)
            {
                pnlButtons.AddButton(Properties.Resources.Cash, Buttons.Cash, "", activePage == TenderEntryType.Cash ? TouchButtonType.Action : TouchButtonType.Normal);
            }

            if(countingCurrency)
            {
                pnlButtons.AddButton(Properties.Resources.Currency, Buttons.Currency, "", activePage == TenderEntryType.Currency ? TouchButtonType.Action : TouchButtonType.Normal);
            }

            if(countingOther)
            {
                pnlButtons.AddButton(Properties.Resources.Other, Buttons.Other, "", activePage == TenderEntryType.Other ? TouchButtonType.Action : TouchButtonType.Normal);
            }

            pnlButtons.AddButton(Properties.Resources.Submit, Buttons.Submit, "", TouchButtonType.OK, DockEnum.DockEnd);
            pnlButtons.AddButton(Properties.Resources.Cancel, Buttons.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
        }

        private void pnlButtons_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((Buttons)args.Tag)
            {
                case Buttons.Cash:
                    ShowListView(TenderEntryType.Cash);
                    break;
                case Buttons.Currency:
                    ShowListView(TenderEntryType.Currency);
                    break;
                case Buttons.Other:
                    ShowListView(TenderEntryType.Other);
                    break;
                case Buttons.Submit:
                    touchNumPad.Focus(); //Force leave the list view to update the last value
                    if(CreateTransaction())
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    break;
                case Buttons.Cancel:
                    DialogResult = DialogResult.Cancel;
                    Close();
                    break;
            }
        }

        private void LoadCashListView()
        {
            lvCash.ClearRows();

            Row row;
            foreach (TenderCountEntry entry in tenderCounts[TenderEntryType.Cash])
            {
                if (cashEntryMethod == CashEntryMethod.Single)
                {
                    row = new Row();
                    row.AddText(entry.TenderName);

                    if(entry.Counted)
                    {
                        row.AddCell(new Cell(rounding.RoundForDisplay(DLLEntry.DataModel, entry.Value, true, true, entry.CurrencyCode, CacheType.CacheTypeTransactionLifeTime), greyBackColorStyle));
                    }
                    else
                    {
                        row.AddCell(new EditableNumericCellTouch(entry.Value, rounding.RoundForDisplay(DLLEntry.DataModel, entry.Value, true, true, entry.CurrencyCode, CacheType.CacheTypeTransactionLifeTime), new DecimalLimit(0, entry.MaxDecimals)));
                    }

                    row.Tag = entry;
                    lvCash.AddRow(row);
                }
                else
                {
                    row = new Row();
                    row.AddText(entry.Description);

                    if (entry.Counted)
                    {
                        row.AddCell(new Cell(rounding.RoundForDisplay(DLLEntry.DataModel, entry.Value, true, true, entry.CurrencyCode, CacheType.CacheTypeTransactionLifeTime), greyBackColorStyle));
                    }
                    else
                    {
                        row.AddCell(new EditableNumericCellTouch(entry.Quantity, entry.Quantity.ToString(), new DecimalLimit(0, 0)));
                    }
                    
                    row.AddText(rounding.RoundForDisplay(DLLEntry.DataModel, entry.Value, true, true, entry.CurrencyCode, CacheType.CacheTypeTransactionLifeTime));
                    row.Tag = entry;
                    lvCash.AddRow(row);
                }

                if(cashListSelectedID == entry.ID)
                {
                    lvCash.Selection.Set(lvCash.Rows.Count - 1);
                }
            }

            lvCash.AutoSizeColumns(true);
        }

        private void LoadCurrencyListView()
        {
            lvCurrency.ClearRows();

            Row row;
            foreach (TenderCountEntry entry in tenderCounts[TenderEntryType.Currency])
            {
                row = new Row();
                row.AddText(entry.Description);
                row.AddText(entry.ID.StringValue);

                if (entry.Counted)
                {
                    row.AddCell(new Cell(rounding.RoundForDisplay(DLLEntry.DataModel, entry.Value, true, true, entry.CurrencyCode, CacheType.CacheTypeTransactionLifeTime), greyBackColorStyle));
                }
                else
                {
                    row.AddCell(new EditableNumericCellTouch(entry.Value, rounding.RoundForDisplay(DLLEntry.DataModel, entry.Value, true, true, entry.CurrencyCode, CacheType.CacheTypeTransactionLifeTime), new DecimalLimit(0, entry.MaxDecimals)));
                }

                row.AddText(rounding.RoundForDisplay(DLLEntry.DataModel, entry.ValueInStoreCurrency, true, true, DLLEntry.Settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime));
                row.Tag = entry;
                lvCurrency.AddRow(row);

                if (currencyListSelectedID == entry.ID)
                {
                    lvCurrency.Selection.Set(lvCurrency.Rows.Count - 1);
                }
            }

            lvCurrency.AutoSizeColumns(true);
        }

        private void LoadOtherListView()
        {
            lvOther.ClearRows();

            Row row;
            foreach (TenderCountEntry entry in tenderCounts[TenderEntryType.Other])
            {
                row = new Row();
                row.AddText(entry.Description);

                if (entry.Counted)
                {
                    row.AddCell(new Cell(rounding.RoundForDisplay(DLLEntry.DataModel, entry.Value, true, true, entry.CurrencyCode, CacheType.CacheTypeTransactionLifeTime), greyBackColorStyle));
                }
                else
                {
                    row.AddCell(new EditableNumericCellTouch(entry.Value, rounding.RoundForDisplay(DLLEntry.DataModel, entry.Value, true, true, entry.CurrencyCode, CacheType.CacheTypeTransactionLifeTime), new DecimalLimit(0, entry.MaxDecimals)));
                }

                row.Tag = entry;
                lvOther.AddRow(row);

                if (otherListSelectedID == entry.ID)
                {
                    lvOther.Selection.Set(lvOther.Rows.Count - 1);
                }
            }

            lvOther.AutoSizeColumns(true);
        }

        private void LoadSubtotalListView()
        {
            tenderCounts[TenderEntryType.Subtotal][0].Value = tenderCounts[TenderEntryType.Cash].Sum(x => x.Value);
            tenderCounts[TenderEntryType.Subtotal][1].Value = tenderCounts[TenderEntryType.Currency].Sum(x => x.ValueInStoreCurrency);
            tenderCounts[TenderEntryType.Subtotal][2].Value = tenderCounts[TenderEntryType.Other].Sum(x => x.Value);
            tenderCounts[TenderEntryType.Subtotal][3].Value = tenderCounts[TenderEntryType.Subtotal].Take(3).Sum(x => x.Value);

            lvSubtotal.ClearRows();

            Row row;
            foreach(TenderCountEntry entry in tenderCounts[TenderEntryType.Subtotal])
            {
                if((entry.ID == SubtotalCashID && !countingCash)
                    || (entry.ID == SubtotalCurrencyID && !countingCurrency)
                    || (entry.ID == SubtotalOtherID && !countingOther))
                {
                    continue;
                }

                row = new Row();

                if (entry.ID == SubtotalTotalID)
                {
                    row.AddCell(new Cell(entry.Description, new Style(lvSubtotal.Columns[0].DefaultStyle) { Font = new Font(lvSubtotal.Font, FontStyle.Bold), BackColor = ColorPalette.POSSelectedRowColor }));
                    row.AddCell(new Cell(rounding.RoundForDisplay(DLLEntry.DataModel, entry.Value, true, true, entry.CurrencyCode, CacheType.CacheTypeTransactionLifeTime), new Style(lvSubtotal.Columns[0].DefaultStyle) { Font = new Font(lvSubtotal.Font, FontStyle.Bold) }));
                }
                else
                {
                    row.AddText(entry.Description);
                    row.AddText(rounding.RoundForDisplay(DLLEntry.DataModel, entry.Value, true, true, entry.CurrencyCode, CacheType.CacheTypeTransactionLifeTime));
                }

                lvSubtotal.AddRow(row);
            }

            lvSubtotal.AutoSizeColumns(true);
        }

        private void ShowListView(TenderEntryType listViewType)
        {
            if(activePage == listViewType)
            {
                return;
            }

            activePage = listViewType;

            if(activePage == TenderEntryType.Cash)
            {
                lvCash.Visible = true;
                lvCurrency.Visible = lvOther.Visible = false;
                lvCash.Focus();
            }
            else if (activePage == TenderEntryType.Currency)
            {
                lvCurrency.Visible = true;
                lvCash.Visible = lvOther.Visible = false;
                lvCurrency.Focus();
            }
            else
            {
                lvOther.Visible = true;
                lvCash.Visible = lvCurrency.Visible = false;
                lvOther.Focus();
            }

            SetupButtons();
        }

        private void TenderCountDialog_Load(object sender, EventArgs e)
        {
            LoadCashListView();
            LoadCurrencyListView();
            LoadOtherListView();
            LoadSubtotalListView();

            if(lvCash.RowCount > 0) { lvCash.Selection.Set(0); }
            if(lvCurrency.RowCount > 0) { lvCurrency.Selection.Set(0); }
            if(lvOther.RowCount > 0) { lvOther.Selection.Set(0); }
        }

        private int GetNumberOfDecimals(decimal value)
        {
            string convertedValue = value.ToString("G29");
            int index = convertedValue.IndexOf(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);

            if(index == -1)
            {
                return 0;
            }
            else
            {
                return convertedValue.Substring(index + 1).Length;
            }
        }

        private void lvCash_CellAction(object sender, LSOne.Controls.EventArguments.CellEventArgs args)
        {
            TenderCountEntry entry = (TenderCountEntry)lvCash.Row(args.RowNumber).Tag;
            
            if(cashEntryMethod == CashEntryMethod.Multiple)
            {
                entry.Quantity = (int)((EditableNumericCellTouch)args.Cell).Value;
                entry.Value = entry.Quantity * entry.DenominationAmount;
            }
            else
            {
                entry.Value = ((EditableNumericCellTouch)args.Cell).Value;
            }

            LoadCashListView();
            LoadSubtotalListView();
        }

        private void lvCurrency_CellAction(object sender, LSOne.Controls.EventArguments.CellEventArgs args)
        {
            TenderCountEntry entry = (TenderCountEntry)lvCurrency.Row(args.RowNumber).Tag;
            entry.Value = ((EditableNumericCellTouch)args.Cell).Value;
            entry.ValueInStoreCurrency = Interfaces.Services.CurrencyService(DLLEntry.DataModel).CurrencyToCurrencyNoRounding(DLLEntry.DataModel, entry.CurrencyCode, storeCurrency.ID, DLLEntry.Settings.CompanyInfo.CurrencyCode, entry.Value);

            LoadCurrencyListView();
            LoadSubtotalListView();
        }

        private void lvOther_CellAction(object sender, LSOne.Controls.EventArguments.CellEventArgs args)
        {
            TenderCountEntry entry = (TenderCountEntry)lvOther.Row(args.RowNumber).Tag;
            entry.Value = ((EditableNumericCellTouch)args.Cell).Value;

            LoadOtherListView();
            LoadSubtotalListView();
        }


        private void ResetTenderValuesToZero(string tenderID, string currencyCode, int posOperation)
        {
            if (posOperation == (int)POSOperations.PayCash)
            {
                tenderCounts[TenderEntryType.Cash].ForEach(x => { x.Value = 0; x.Quantity = 0; });
                LoadCashListView();
            }
            else if (posOperation == (int)POSOperations.PayCurrency)
            {
                foreach (TenderCountEntry currencyTender in tenderCounts[TenderEntryType.Currency])
                {
                    if (currencyTender.TenderID == tenderID && currencyTender.CurrencyCode == currencyCode)
                    {
                        currencyTender.Value = 0;
                        currencyTender.ValueInStoreCurrency = 0;
                        break;
                    }
                }

                LoadCurrencyListView();
            }

            else
            {
                foreach (TenderCountEntry currencyTender in tenderCounts[TenderEntryType.Other])
                {
                    if (currencyTender.TenderID == tenderID)
                    {
                        currencyTender.Value = 0;
                        break;
                    }
                }

                LoadOtherListView();
            }

            LoadSubtotalListView();
        }

        private bool CreateTransaction()
        {
            if (tenderCounts[TenderEntryType.Subtotal][3].Value != 0)
            {
                CountValidation countValidation = ValidateCounts();

                if (countValidation == CountValidation.InvalidContinue ||
                    (countValidation == CountValidation.Valid && Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.AreYouSureYouWantToWriteTheseAmountsToTheDatabase, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes))
                {
                    int attemptsIndex = 0;

                    bool cashCounted = CountCash(ref attemptsIndex);
                    bool countSuccessful = false;
                    
                    if(cashCounted)
                    {
                        bool otherCounted = CountOther(ref attemptsIndex);

                        if(otherCounted)
                        {
                            countSuccessful = CountCurrency(ref attemptsIndex);
                        }
                    }

                    return countSuccessful;
                }
                else return false;
            }

            // No amount has been entered yet
            Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.NoAmountHasBennEnteredYet, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            return false;
        }

        private CountValidation ValidateCounts()
        {
            CountValidation countValidation = CountValidation.Valid;
            TenderCountEntry requiredTenderCount = null;

            TenderCountEntry cashEntry = tenderCounts[TenderEntryType.Cash][0];
            
            if(!cashEntry.Counted 
                && GetPaymentMethodRecount(cashEntry.TenderID) == 0 
                && tenderCounts[TenderEntryType.Subtotal][0].Value == 0
                && requiredPayments.Any(x => x.TenderType == cashEntry.TenderID && x.AmountTenderd != 0))
            {
                requiredTenderCount = cashEntry;
            }

            if(requiredTenderCount == null)
            {
                foreach(TenderCountEntry tenderCountEntry in tenderCounts[TenderEntryType.Currency].Where(x => !x.Counted))

                if (GetPaymentMethodRecount(tenderCountEntry.TenderID) == 0
                    && tenderCountEntry.Value == 0
                    && requiredPayments.Any(x => x.TenderType == tenderCountEntry.TenderID && x.Currency == tenderCountEntry.CurrencyCode && x.AmountOfCurrency != 0))
                {
                        requiredTenderCount = tenderCountEntry;
                        break;
                }
            }

            if (requiredTenderCount == null)
            {
                foreach(TenderCountEntry tenderCountEntry in tenderCounts[TenderEntryType.Other].Where(x => !x.Counted))
                {
                    if (GetPaymentMethodRecount(tenderCountEntry.TenderID) == 0 
                        && tenderCountEntry.Value == 0 
                        && requiredPayments.Any(x => x.TenderType == tenderCountEntry.TenderID && x.AmountTenderd != 0))
                    {
                        requiredTenderCount = tenderCountEntry;
                        break;
                    }
                }
            }

            if(requiredTenderCount != null)
            {
                countValidation = CountValidation.Invalid;
                string tenderDescription = requiredTenderCount.PosOperationID == (int)POSOperations.PayCash
                    || requiredTenderCount.PosOperationID == (int)POSOperations.PayCurrency
                    ? requiredTenderCount.CurrencyCode.StringValue
                    : requiredTenderCount.Description;

                string message = Properties.Resources.NoAmountEnteredForTender.Replace("#1", tenderDescription)
                                + Environment.NewLine
                                + Properties.Resources.SubmitTenderCountAnywayQuestion;

                if (Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(
                    message, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
                {
                    countValidation = CountValidation.InvalidContinue;
                }
            }

            return countValidation;
        }

        private int GetPaymentMethodRecount(RecordIdentifier tenderID)
        {
            return storePaymentMethods.Find(f => f.PaymentTypeID == tenderID).MaxRecount;
        }

        private bool CountCash(ref int attemptsIndex)
        {
            if (countingCash)
            {
                decimal totalCashAmount = tenderCounts[TenderEntryType.Subtotal][0].Value;
                TenderCountEntry cashEntry = tenderCounts[TenderEntryType.Cash][0];

                if (!cashEntry.Counted)
                {
                    if (transaction.GetType() == typeof(TenderDeclarationTransaction))
                    {
                        if (NeedForRecount(cashEntry.TenderID, totalCashAmount, cashEntry.CurrencyCode.StringValue, ref attemptsIndex, cashEntry.PosOperationID, cashEntry.CurrencyCode.StringValue))
                        {
                            ShowListView(TenderEntryType.Cash);
                            return false;
                        }
                        else
                        {
                            tenderCounts[TenderEntryType.Cash].ForEach(x => x.Counted = true);
                            LoadCashListView();
                            attemptsIndex++;
                        }
                    }

                    if (totalCashAmount != 0)
                    {
                        TenderLineItem tenderLine = new TenderLineItem();
                        tenderLine.Amount = totalCashAmount;
                        tenderLine.TenderTypeId = cashEntry.TenderID;
                        tenderLine.Description = cashEntry.TenderName;
                        tenderLine.ExchrateMST = Interfaces.Services.CurrencyService(DLLEntry.DataModel).ExchangeRate(DLLEntry.DataModel, transaction.StoreCurrencyCode) * 100;
                        tenderLine.ExchangeRate = Interfaces.Services.CurrencyService(DLLEntry.DataModel).GetExchangeRateRatio(DLLEntry.DataModel, cashEntry.CurrencyCode.StringValue, ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.Currency);
                        tenderLine.CompanyCurrencyAmount = Interfaces.Services.CurrencyService(DLLEntry.DataModel).CurrencyToCurrency(
                            DLLEntry.DataModel,
                            transaction.StoreCurrencyCode,
                            ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).CompanyInfo.CurrencyCode,
                            ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).CompanyInfo.CurrencyCode,
                            totalCashAmount);
                        tenderLine.CurrencyCode = ((TenderCountTransaction)transaction).StoreCurrencyCode;
                        tenderLine.ForeignCurrencyAmount = totalCashAmount;
                        tenderLine.ExpectedTenderDeclarationAmount = GetExpectedAmount(cashEntry.TenderID, cashEntry.CurrencyCode.StringValue, cashEntry.PosOperationID);
                        transaction.Add(tenderLine);
                    }
                }
            }

            return true;
        }

        private bool CountCurrency(ref int attemptsIndex)
        {
            if (countingCurrency)
            {
                decimal currencyAmount = 0M;

                foreach (TenderCountEntry currencyCount in tenderCounts[TenderEntryType.Currency].Where(x => !x.Counted))
                {
                    currencyAmount = currencyCount.Value;
                    decimal currencyAmountInStoreCurrency = currencyCount.ValueInStoreCurrency;

                    if (transaction.GetType() == typeof(TenderDeclarationTransaction))
                    {
                        if (NeedForRecount(currencyCount.TenderID, currencyAmount, currencyCount.CurrencyCode.StringValue, ref attemptsIndex, currencyCount.PosOperationID, currencyCount.CurrencyCode.StringValue))
                        {
                            ShowListView(TenderEntryType.Currency);
                            return false;
                        }
                        else
                        {
                            currencyCount.Counted = true;
                            attemptsIndex++;
                        }
                    }

                    LoadCurrencyListView();

                    if (currencyAmount != 0)
                    {
                        TenderLineItem tenderLine = new TenderLineItem();
                        tenderLine.Amount = currencyCount.ValueInStoreCurrency;
                        tenderLine.ForeignCurrencyAmount = currencyAmount;
                        tenderLine.CompanyCurrencyAmount = Interfaces.Services.CurrencyService(DLLEntry.DataModel).CurrencyToCurrency(
                            DLLEntry.DataModel,
                            currencyCount.CurrencyCode.StringValue,
                            ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).CompanyInfo.CurrencyCode,
                            ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).CompanyInfo.CurrencyCode,
                            currencyAmount);
                        tenderLine.ExchangeRate = Interfaces.Services.CurrencyService(DLLEntry.DataModel).GetExchangeRateRatio(DLLEntry.DataModel, currencyCount.CurrencyCode.StringValue, ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.Currency);
                        tenderLine.ExchrateMST = Interfaces.Services.CurrencyService(DLLEntry.DataModel).ExchangeRate(DLLEntry.DataModel, currencyCount.CurrencyCode.StringValue) * 100;
                        tenderLine.TenderTypeId = currencyCount.TenderID;
                        tenderLine.Description = currencyCount.TenderName;
                        tenderLine.CurrencyCode = currencyCount.CurrencyCode.StringValue;
                        tenderLine.ExpectedTenderDeclarationAmount = GetExpectedAmount(currencyCount.TenderID, currencyCount.CurrencyCode.StringValue, currencyCount.PosOperationID);
                        transaction.Add(tenderLine);
                    }
                }
            }

            return true;
        }

        private bool CountOther(ref int attemptsIndex)
        {
            if (countingOther)
            {
                decimal otherAmount = 0M;

                foreach (TenderCountEntry otherCount in tenderCounts[TenderEntryType.Other].Where(x => !x.Counted))
                {
                    otherAmount = otherCount.Value;

                    if (transaction.GetType() == typeof(TenderDeclarationTransaction))
                    {
                        if (NeedForRecount(otherCount.TenderID, otherAmount, otherCount.CurrencyCode.StringValue, ref attemptsIndex, otherCount.PosOperationID, otherCount.Description))
                        {
                            ShowListView(TenderEntryType.Other);
                            return false;
                        }
                        else
                        {
                            otherCount.Counted = true;
                            attemptsIndex++;
                        }
                    }

                    LoadOtherListView();

                    if (otherAmount != 0)
                    {
                        TenderLineItem tenderLine = new TenderLineItem();
                        tenderLine.Amount = otherAmount;
                        tenderLine.TenderTypeId = otherCount.TenderID;
                        tenderLine.Description = otherCount.TenderName;
                        tenderLine.ExchrateMST = Interfaces.Services.CurrencyService(DLLEntry.DataModel).ExchangeRate(DLLEntry.DataModel, transaction.StoreCurrencyCode) * 100;
                        tenderLine.CompanyCurrencyAmount = Interfaces.Services.CurrencyService(DLLEntry.DataModel).CurrencyToCurrency(
                            DLLEntry.DataModel,
                            transaction.StoreCurrencyCode,
                            ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).CompanyInfo.CurrencyCode,
                            ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).CompanyInfo.CurrencyCode,
                            otherAmount);
                        tenderLine.CurrencyCode = transaction.StoreCurrencyCode;
                        tenderLine.ExpectedTenderDeclarationAmount = GetExpectedAmount(otherCount.TenderID, otherCount.CurrencyCode.StringValue, otherCount.PosOperationID);
                        transaction.Add(tenderLine);
                    }
                }
            }

            return true;
        }

        private bool NeedForRecount(string tenderID, decimal currentAmount, string currencyCode, ref int counter, int posOperationNumber, string tenderDescription)
        {
            //Adds new entries for new tenders.
            while (counter >= attemptsDone.Count)
            {
                attemptsDone.Add(0);
            }
            //Get allowances for that Tender from In-Memory table. The values are fetched before the counting dialog is opened.
            StorePaymentMethod paymentMethod = storePaymentMethods.Find(f => f.PaymentTypeID == tenderID);

            decimal expectedAmount = GetExpectedAmount(tenderID, currencyCode, posOperationNumber);

            if (attemptsDone[counter] < paymentMethod.MaxRecount)
            {
                attemptsDone[counter]++;
                if (Math.Abs((currentAmount - expectedAmount)) > paymentMethod.MaxCountingDifference)
                {
                    ResetTenderValuesToZero(tenderID, currencyCode, posOperationNumber);
                    transaction.TenderLines.Clear();

                    string message = Properties.Resources.PleaseCountTheTenderAgain.Replace("#1", tenderDescription);
                    if (paymentMethod.BlindRecountAllowed)
                    {
                        Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                    else
                    {
                        string newMessage = message + " " + Properties.Resources.ErrorAmount.Replace
                            ("#2", rounding.RoundForDisplay(DLLEntry.DataModel, expectedAmount, false, false, DLLEntry.Settings.Store.Currency));

                        Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(newMessage, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);

                    }

                    return true;  //yes, recount is necessary....
                }
            }

            return false; // no need for recount
        }

        private decimal GetExpectedAmount(string tenderID, string currencyCode, int posOperationNumber)
        {
            decimal exp = 0M;

            if (requiredPayments == null)
            {
                return exp;
            }

            foreach (PaymentTransaction current in requiredPayments)
            {
                if (current.TenderType == tenderID)
                {
                    if (posOperationNumber != (int)POSOperations.PayCurrency)
                    {
                        return current.AmountTenderd;
                    }
                    else if (posOperationNumber == (int)POSOperations.PayCurrency)
                    {
                        if (current.Currency == currencyCode) // this is important if more than one foreign currency exists!
                        {
                            exp = current.AmountOfCurrency;
                        }
                    }
                }
            }
            return exp;
        }

        private void touchNumPad_EnterPressed(object sender, EventArgs e)
        {
            switch (activePage)
            {
                case TenderEntryType.Cash:
                    if(lvCash.ContainsFocus)
                    {
                        SendKeys.Send("{ENTER}");
                    }
                    break;
                case TenderEntryType.Currency:
                    if (lvCurrency.ContainsFocus)
                    {
                        SendKeys.Send("{ENTER}");
                    }
                    break;
                case TenderEntryType.Other:
                    if (lvOther.ContainsFocus)
                    {
                        SendKeys.Send("{ENTER}");
                    }
                    break;
            }
        }

        private void touchNumPad_ClearPressed(object sender, EventArgs e)
        {
            switch (activePage)
            {
                case TenderEntryType.Cash:
                    if(lvCash.BuddyControl != null && !lvCash.BuddyControl.IsDisposed && lvCash.BuddyControl is ShadeNumericTextBox)
                    {
                        ((ShadeNumericTextBox)lvCash.BuddyControl).Text = "";
                    }
                    else if(lvCash.Selection.Count > 0)
                    {
                        TenderCountEntry tenderCountEntry = (TenderCountEntry)lvCash.Selection[0].Tag;
                        tenderCountEntry.Quantity = 0;
                        tenderCountEntry.Value = 0;

                        LoadCashListView();
                        LoadSubtotalListView();
                    }
                    break;
                case TenderEntryType.Currency:
                    if (lvCurrency.BuddyControl != null && !lvCurrency.BuddyControl.IsDisposed && lvCurrency.BuddyControl is ShadeNumericTextBox)
                    {
                        ((ShadeNumericTextBox)lvCurrency.BuddyControl).Text = "";
                    }
                    else if (lvCurrency.Selection.Count > 0)
                    {
                        TenderCountEntry tenderCountEntry = (TenderCountEntry)lvCurrency.Selection[0].Tag;
                        tenderCountEntry.Value = 0;
                        tenderCountEntry.ValueInStoreCurrency = 0;

                        LoadCurrencyListView();
                        LoadSubtotalListView();
                    }
                    break;
                case TenderEntryType.Other:
                    if (lvOther.BuddyControl != null && !lvOther.BuddyControl.IsDisposed && lvOther.BuddyControl is ShadeNumericTextBox)
                    {
                        ((ShadeNumericTextBox)lvOther.BuddyControl).Text = "";
                    }
                    else if (lvOther.Selection.Count > 0)
                    {
                        TenderCountEntry tenderCountEntry = (TenderCountEntry)lvOther.Selection[0].Tag;
                        tenderCountEntry.Value = 0;

                        LoadOtherListView();
                        LoadSubtotalListView();
                    }
                    break;
            }
        }

        private void lvCash_SelectionChanged(object sender, EventArgs e)
        {
            if(lvCash.Selection.Count > 0)
            {
                cashListSelectedID = ((TenderCountEntry)lvCash.Selection[0].Tag).ID;
            }
        }

        private void lvCurrency_SelectionChanged(object sender, EventArgs e)
        {
            if (lvCurrency.Selection.Count > 0)
            {
                currencyListSelectedID = ((TenderCountEntry)lvCurrency.Selection[0].Tag).ID;
            }
        }

        private void lvOther_SelectionChanged(object sender, EventArgs e)
        {
            if (lvOther.Selection.Count > 0)
            {
                otherListSelectedID = ((TenderCountEntry)lvOther.Selection[0].Tag).ID;
            }
        }

        private void TenderCountDialog_Shown(object sender, EventArgs e)
        {
            switch (activePage)
            {
                case TenderEntryType.Cash:
                    lvCash.Focus();
                    break;
                case TenderEntryType.Currency:
                    lvCurrency.Focus();
                    break;
                case TenderEntryType.Other:
                    lvOther.Focus();
                    break;
            }
        }
    }
}
