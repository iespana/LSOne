using LSOne.Controls.Dialogs.Properties;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace LSOne.Controls.Dialogs
{
    public partial class PayCashDialog : TouchBaseForm
    {
        private StorePaymentMethod tenderInformation;
        private decimal balanceAmount;
        public decimal RegisteredAmount { get; set; }
        public bool OperationDone { get; set; }
        private Currency currency;
        public PayCashDialog()
        {
            InitializeComponent();
        }

        public PayCashDialog(decimal balanceAmount, StorePaymentMethod tenderInformation, decimal maxValue = 0, TouchScrollButtonsCurrencyHelper.ViewOptions? viewOptions = null)
            :this()
        {
            this.tenderInformation = tenderInformation;
            this.balanceAmount = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundAmount(DLLEntry.DataModel,
                                                                                            balanceAmount,
                                                                                            DLLEntry.DataModel.CurrentStoreID,
                                                                                            tenderInformation.ID.SecondaryID,
                                                                                            CacheType.CacheTypeTransactionLifeTime);

            touchScrollButtonPanel1.SetButtonsCurrency(this.balanceAmount, tenderInformation, DLLEntry.Settings.Store.Currency, viewOption: viewOptions);
            touchDialogBanner1.BannerText = tenderInformation.Text;
            currency = Providers.CurrencyData.Get(DLLEntry.DataModel, DLLEntry.Settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
            ntbAmount.DecimalLetters = GetNumberOfDecimals(currency.RoundOffAmount);
            ntbAmount.AllowDecimal = ntbAmount.DecimalLetters > 0;

            if (maxValue > 0)
            {
                ntbAmount.MaxValue = (double)maxValue;
            }

            ntbAmount.Text = "";
            ntbAmount.Value = (double)this.balanceAmount;
            ntbAmount.SelectAll();


            touchDialogBanner1.BannerText += " - ";
            touchDialogBanner1.BannerText += Resources.AmountDue.Replace("#1", Services.Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel,
                                                                                            this.balanceAmount,
                                                                                            DLLEntry.Settings.Store.Currency,
                                                                                            true,
                                                                                            CacheType.CacheTypeApplicationLifeTime));


            if (touchScrollButtonPanel1.ButtonCount > 0 || (viewOptions.HasValue && viewOptions.Value == TouchScrollButtonsCurrencyHelper.ViewOptions.None))
            {
                touchScrollButtonPanel1.AddButton(Resources.Pay, DialogResult.OK, "", TouchButtonType.OK, DockEnum.DockEnd);
            }
            touchScrollButtonPanel1.AddButton(Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
            touchNumPad1.ClearPressed += (sender, args) => { ntbAmount.Text = ""; };
            touchNumPad1.TouchKeyPressed += (sender, args) => ntbAmount.Focus();

            touchNumPad1.PlusMinusPressed += TouchNumPad1OnPlusMinusPressed;

            if (balanceAmount < 0)
            {
                touchNumPad1.NegativeMode = true;
                touchNumPad1.ShowPlusMinusToggle = true;                
                ntbAmount.AllowNegative = true;

                if (maxValue < 0)
                {
                    ntbAmount.MinValue = (double)maxValue;
                    ntbAmount.HasMinValue = true;
                }

                // Re-set the value since we have now allowed negative numbers
                ntbAmount.Value = (double)this.balanceAmount;
                ntbAmount.Select();
                ntbAmount.SuppressNextSelectAll();
            }
        }

        private int GetNumberOfDecimals(decimal currencyUnit)
        {
            int numberOfDecimals = 0;

            if (Math.Round(currencyUnit) > 0)
            {
                return numberOfDecimals;
            }

            for (int i = 1; i < 9; i++)
            {
                decimal factor = (decimal)Math.Pow(10, i);
                decimal multipl = currencyUnit * factor;
                if (multipl == Math.Round(multipl))
                {
                    numberOfDecimals = i;
                    break;
                }
            }
            return numberOfDecimals;
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
            RegisteredAmount = (decimal)(ntbAmount.Text != "" ? ntbAmount.Value : 0);
            var rounding = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel);
            RegisteredAmount = rounding.RoundAmount(DLLEntry.DataModel, RegisteredAmount, DLLEntry.DataModel.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            if (RegisteredAmount != 0 || balanceAmount == RegisteredAmount)
            {
                if ((touchNumPad1.NegativeMode == true))
                {
                    if ((RegisteredAmount > 0))
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.OnlyNegativeAmountsAllowed, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return;
                    }
                    if (RegisteredAmount < balanceAmount)
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.NegativePaymentsNotBelowBalance, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return;
                    }
                }
                
                DialogResult = DialogResult.OK;
                System.Threading.Thread.Sleep(50);
                ClearMouseClickQueue();
                Close();
            }
            else
            {
                ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.PleaseEnterAmountToPay, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        private void touchScrollButtonPanel1_Click(object sender, ScrollButtonEventArguments args)
        {
            if (args.Tag is DialogResult && ((DialogResult)args.Tag) == DialogResult.OK)
            {
                touchNumPad1_EnterPressed(this, EventArgs.Empty);
                return;
            }
            else if (args.Tag is DialogResult)
            {
                DialogResult = (DialogResult)args.Tag;
                ClearMouseClickQueue();
                System.Threading.Thread.Sleep(50);
                Close();
                return;
            }

            IRoundingService rounding = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel);
            RegisteredAmount = rounding.RoundAmount(DLLEntry.DataModel, (decimal)args.Tag, DLLEntry.DataModel.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            DialogResult = DialogResult.OK;
            System.Threading.Thread.Sleep(50);
            ClearMouseClickQueue();
            Close();
        }

        private void numericTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                System.Threading.Thread.Sleep(50);
                ClearMouseClickQueue();
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
    }
}
