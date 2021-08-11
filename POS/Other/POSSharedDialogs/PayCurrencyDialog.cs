using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using LSOne.Controls.Dialogs.Properties;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Controls.Dialogs
{
    public partial class PayCurrencyDialog : TouchBaseForm
    {
        public decimal RegisteredAmount { get; set; }
        public bool OperationDone { get; set; }
        public decimal ExchangeRate { get; set; }
        public RecordIdentifier CurrentCurrencyCode { get; set; }

        private StorePaymentMethod tenderInformation;
        private decimal balanceAmount;
        private decimal convertedBalanceAmount;
        private decimal convertedBalanceAmountRounded;
        private bool manuallyEntered;

        public PayCurrencyDialog()
        {
            InitializeComponent();
            RelocateControls();
        }

        public PayCurrencyDialog(decimal balanceAmount, StorePaymentMethod tenderInformation)
        {
            InitializeComponent();
            RelocateControls();
            this.balanceAmount = balanceAmount;
            this.tenderInformation = tenderInformation;
            CurrentCurrencyCode = DLLEntry.Settings.Store.Currency;
            touchScrollButtonPanel1.SetButtonsForeignCurrency(tenderInformation, DLLEntry.Settings.Store.Currency);
            ntbAmount.DecimalLetters = tenderInformation.Rounding.DigitsBeforeFirstSignificantDigit();
            ntbAmount.AllowDecimal = ntbAmount.DecimalLetters > 0;
            ntbAmount.Text = "";
            lblCurrency.Text = "";
            lblCurrency.HeaderText = Resources.SelectCurrency;
            touchDialogBanner1.BannerText = tenderInformation.Text;
            touchDialogBanner1.BannerText += " - ";
            touchDialogBanner1.BannerText += Resources.AmountDue.Replace("#1",
                                                                Services.Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel,
                                                                                            balanceAmount,
                                                                                            DLLEntry.Settings.Store.Currency,
                                                                                            true,
                                                                                            CacheType.CacheTypeApplicationLifeTime));
            touchScrollButtonPanel1.AddButton(Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
            touchNumPad1.ClearPressed += (sender, args) => { ntbAmount.Text = ""; };
            touchNumPad1.TouchKeyPressed += (sender, args) => ntbAmount.Focus();

            touchNumPad1.PlusMinusPressed += TouchNumPad1OnPlusMinusPressed;

            SetNegativeMode(balanceAmount);

            manuallyEntered = false;
        }

        private void RelocateControls()
        {
            touchDialogBanner1.Location = new Point(1, 1);
            touchDialogBanner1.Width = Width - 2;
        }

        private void SetNegativeMode(decimal balanceAmount)
        {
            if (balanceAmount < 0)
            {
                touchNumPad1.NegativeMode = true;
                touchNumPad1.ShowPlusMinusToggle = true;
                ntbAmount.AllowNegative = true;
                ntbAmount.Value = (double)balanceAmount;
            }
        }

        private void TouchNumPad1OnPlusMinusPressed(object sender, EventArgs eventArgs)
        {
            int oldSelection = ntbAmount.SelectionStart;

            if (ntbAmount.Text.StartsWith("-"))
            {
                ntbAmount.Text = ntbAmount.Text.Right(ntbAmount.Text.Length - 1);

                if (oldSelection > 0)
                {
                    ntbAmount.SelectionStart = oldSelection - 1;
                }
            }
            else
            {
                ntbAmount.Text = "-" + ntbAmount.Text;

                ntbAmount.SelectionStart = oldSelection + 1;
            }

            ntbAmount.UpdateToolTip();
        }

        private void touchNumPad1_EnterPressed(object sender, EventArgs e)
        {
            RegisteredAmount = 0;

            if (!string.IsNullOrEmpty(ntbAmount.Text))
            {
                RegisteredAmount = manuallyEntered ? (decimal)ntbAmount.Value : convertedBalanceAmount;
            }

            if (RegisteredAmount == 0)
            {
                ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.PleaseEnterAmountToPay, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return;
            }

            if ((touchNumPad1.NegativeMode == true))
            {
                if ((RegisteredAmount > 0))
                {
                    ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.OnlyNegativeAmountsAllowed, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return;
                }

                if (RegisteredAmount < convertedBalanceAmountRounded && manuallyEntered )
                {
                    ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.NegativePaymentsNotBelowBalance, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return;
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void touchScrollButtonPanel1_Click(object sender, ScrollButtonEventArguments args)
        {
            if (args.Tag == null)
            {
                touchScrollButtonPanel1.SetButtonsForeignCurrency(tenderInformation, DLLEntry.Settings.Store.Currency);
                touchScrollButtonPanel1.AddButton(Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
                lblCurrency.Enabled = ntbAmount.Enabled = false;
                lblCurrency.Text = "";
                lblCurrency.HeaderText = Resources.SelectCurrency;
                ntbAmount.Focus();
                return;
            }
            if (args.Tag is DialogResult && ((DialogResult) args.Tag) == DialogResult.OK)
            {
                touchNumPad1_EnterPressed(this, EventArgs.Empty);
                return;
            }
            if (args.Tag is DialogResult)
            {
                DialogResult = (DialogResult) args.Tag;
                Close();
                return;
            }
            if (args.Tag is DataEntity)
            {
                lblCurrency.Enabled = ntbAmount.Enabled = true;

                IRoundingService rounding = (IRoundingService) DLLEntry.DataModel.Service(ServiceType.RoundingService);

                ExchangeRate = Services.Interfaces.Services.CurrencyService(DLLEntry.DataModel).ExchangeRate(DLLEntry.DataModel, ((DataEntity)args.Tag).ID);
                CurrentCurrencyCode = ((DataEntity) args.Tag).ID;
                Currency currency = Providers.CurrencyData.Get(DLLEntry.DataModel, ((DataEntity) args.Tag).ID, CacheType.CacheTypeApplicationLifeTime);
                if (currency != null)
                {
                    ntbAmount.DecimalLetters = currency.RoundOffAmount.DigitsBeforeFirstSignificantDigit();
                    ntbAmount.AllowDecimal = ntbAmount.DecimalLetters > 0;
                    ntbAmount.Text = "";
                }

                //Use rounded amount only for display, return full value for proper calculations
                convertedBalanceAmount = Services.Interfaces.Services.CurrencyService(DLLEntry.DataModel).CurrencyToCurrencyNoRounding(DLLEntry.DataModel, DLLEntry.Settings.Store.Currency, ((DataEntity)args.Tag).ID, DLLEntry.Settings.CompanyInfo.CurrencyCode, balanceAmount);
                convertedBalanceAmountRounded = rounding.Round(DLLEntry.DataModel, convertedBalanceAmount, ((DataEntity)args.Tag).ID, CacheType.CacheTypeTransactionLifeTime);

                lblCurrency.Text = rounding.RoundString(DLLEntry.DataModel, convertedBalanceAmountRounded, ((DataEntity)args.Tag).ID, true,CacheType.CacheTypeTransactionLifeTime);
                lblCurrency.HeaderText = Resources.AmountIn + " " + (string) ((DataEntity) args.Tag).ID;

                ntbAmount.Value = (double)convertedBalanceAmount;

                touchScrollButtonPanel1.SetButtonsCurrency(convertedBalanceAmount, tenderInformation, ((DataEntity)args.Tag).ID);
                touchScrollButtonPanel1.AddButton(Resources.Currencies, null, "", TouchButtonType.Action, DockEnum.DockEnd);
                if (touchScrollButtonPanel1.ButtonCount > 1)
                {
                    touchScrollButtonPanel1.AddButton(Resources.Pay, DialogResult.OK, "", TouchButtonType.OK, DockEnum.DockEnd);
                }
                touchScrollButtonPanel1.AddButton(Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);

                SetNegativeMode(convertedBalanceAmountRounded);

                ntbAmount.Select();
                manuallyEntered = false;
                return;
            }
            if (args.Tag is decimal)
            {
                RegisteredAmount = (decimal) args.Tag;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void numericTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                touchNumPad1_EnterPressed(sender, e);
            }
        }

        private void numericTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator && ntbAmount.Text == "")
            {
                ntbAmount.Text = "0" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                ntbAmount.Select(ntbAmount.Text.Length, 0);
            }
        }

        private void ntbAmount_ValueChanged(object sender, EventArgs e)
        {
            manuallyEntered = true;
        }
    }
}
