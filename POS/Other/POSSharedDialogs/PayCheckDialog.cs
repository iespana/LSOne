using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using LSOne.Controls.Dialogs.Properties;
using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Controls.Dialogs
{
    public partial class PayCheckDialog : TouchBaseForm
    {
        private decimal balanceAmount;
        private StorePaymentMethod tenderInformation;

        public decimal RegisteredAmount { get; set; }
        public string CheckID { get; set; }

        public PayCheckDialog()
        {
            InitializeComponent();
        }

        public PayCheckDialog(decimal balanceAmount, decimal numpadValue, StorePaymentMethod tenderInformation)
        {
            InitializeComponent();

            this.balanceAmount = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundAmount(DLLEntry.DataModel,
                                                                                            balanceAmount,
                                                                                            DLLEntry.DataModel.CurrentStoreID,
                                                                                            tenderInformation.ID.SecondaryID,
                                                                                            CacheType.CacheTypeTransactionLifeTime);
            this.tenderInformation = tenderInformation;
            touchScrollButtonPanel.SetButtonsCurrency(this.balanceAmount, tenderInformation, DLLEntry.Settings.Store.Currency);
            touchDialogBanner.BannerText = tenderInformation.Text;
            ntbAmount.DecimalLetters = tenderInformation.Rounding.DigitsBeforeFirstSignificantDigit();
            ntbAmount.AllowDecimal = ntbAmount.DecimalLetters > 0;
            ntbAmount.AllowNegative = this.balanceAmount < decimal.Zero;
            ntbAmount.Value = numpadValue != 0 ? (double)numpadValue : (double)balanceAmount;

            if (ntbAmount.AllowNegative)
            {
                ntbAmount.Select();
                ntbAmount.SuppressNextSelectAll();
            }

            touchDialogBanner.BannerText += " - ";
            touchDialogBanner.BannerText += Resources.AmountDue.Replace("#1",
                                                                Services.Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel,
                                                                                            this.balanceAmount,
                                                                                            DLLEntry.Settings.Store.Currency,
                                                                                            true,
                                                                                            CacheType.CacheTypeApplicationLifeTime));

            CheckID = "";
        }

        private void PayButtonPressed(object sender, EventArgs e)
        {
            CheckID = tbCheckID.Text.Trim();
            RegisteredAmount = (decimal)(ntbAmount.Text != "" ? ntbAmount.Value : 0);
            var rounding = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel);
            RegisteredAmount = rounding.RoundAmount(DLLEntry.DataModel, RegisteredAmount, DLLEntry.DataModel.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            if (RegisteredAmount != 0 || balanceAmount == RegisteredAmount)
            {
                if (ntbAmount.AllowNegative)
                {
                    if ((RegisteredAmount > 0))
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Resources.OnlyNegativeAmountsAllowed, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return;
                    }
                    if (RegisteredAmount < balanceAmount)
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Resources.NegativePaymentsNotBelowBalance, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return;
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                ((IDialogService) DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Resources.PleaseEnterAmountToPay, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        private void touchScrollButtonPanel_Click(object sender, ScrollButtonEventArguments args)
        {
            if (args.Tag is DialogResult && ((DialogResult) args.Tag) == DialogResult.OK)
            {
                PayButtonPressed(this, EventArgs.Empty);
                return;
            }
            if (args.Tag is DialogResult) 
            {
                DialogResult = (DialogResult) args.Tag;
                Close();
                return;
            }
            var rounding = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel);
            RegisteredAmount = rounding.RoundAmount(DLLEntry.DataModel, (decimal)args.Tag, DLLEntry.DataModel.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            ntbAmount.Value = (double)RegisteredAmount;
        }

        private void ntbAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                PayButtonPressed(sender, e);
            }
        }

        private void ntbAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator && ntbAmount.Text == "")
            {
                ntbAmount.Text = "0" + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                ntbAmount.Select(ntbAmount.Text.Length, 0);
            }
        }

        private void touchKeyboard_ObtainCultureName(object sender, CultureEventArguments args)
        {
            if (DLLEntry.Settings.UserProfile.KeyboardCode != "")
            {
                args.CultureName = DLLEntry.Settings.UserProfile.KeyboardCode;
                args.LayoutName = DLLEntry.Settings.UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = DLLEntry.Settings.Store.KeyboardCode;
                args.LayoutName = DLLEntry.Settings.Store.KeyboardLayoutName;
            }
        }

        private void touchKeyboard_EnterPressed(object sender, EventArgs e)
        {
            if (ActiveControl is Button)
            {
                ((Button)ActiveControl).PerformClick();
            }
            else if (ActiveControl is ShadeTextBoxTouch)
            {
                SelectNextControl(ActiveControl, true, true, false, true);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ntbAmount_ValueChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = (decimal)ntbAmount.Value != decimal.Zero;
        }

        private void PayCheckDialog_Load(object sender, EventArgs e)
        {            
            ntbAmount.Select();
        }
    }
}