using LSOne.Controls.Dialogs.Properties;
using LSOne.Controls.EventArguments;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace LSOne.Controls.Dialogs
{
    public partial class CashManagementDialog : TouchBaseForm
    {
        private TenderDeclarationType type;
        private Currency currency;
        private decimal startLimit;

        public bool InputRequired
        {
            get
            {
                return !btnCancel.Visible;
            }
            set
            {
                btnCancel.Visible = !value;
                btnOk.Location = btnCancel.Visible ? new Point(btnCancel.Location.X - btnOk.Width, btnOk.Location.Y) : btnCancel.Location;
            }
        }

        public decimal TenderedAmount
        {
            get
            {
                return (decimal)ntbAmount.Value;
            }
        }

        public decimal AddedAmount
        {
            get
            {
                return (decimal) ntbAddedAmount.Value;
            }
        }

        public string Description
        {
            get
            {
                return tbDescription.Text;
            }
        }

        public CashManagementDialog()
        {
            InitializeComponent();
        }

        public CashManagementDialog(TenderDeclarationType type, decimal startLimit = 0M)
            : this()
        {
            this.type = type;
            this.startLimit = startLimit;
            switch (type)
            {
                case TenderDeclarationType.DeclareStartAmount:
                    touchDialogBanner.BannerText = Resources.DeclareStartAmount;
                    break;
                case TenderDeclarationType.Float:
                    touchDialogBanner.BannerText = Resources.DeclareFloatAmount;
                    break;
                case TenderDeclarationType.TenderRemoval:
                    touchDialogBanner.BannerText = Resources.TenderRemoval;
                    lblAddedAmount.Text = Resources.TenderRemovalLabel;
                    tbDescription.Text = Resources.TenderRemovalDescription;

                    tbDescription.Multiline = false;
                    tbDescription.Location = ntbAmount.Location;
                    lblDescription.Location = lblAmount.Location;
                    tbDescription.Size = new Size(tbDescription.Width + ntbAmount.Width + 5, 50);
                    ntbAddedAmount.Width = tbDescription.Width;

                    lblAmount.Visible = false;
                    ntbAmount.Visible = false;
                    break;
            }

            currency = Providers.CurrencyData.Get(DLLEntry.DataModel, DLLEntry.Settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
            ntbAmount.DecimalLetters = ntbAddedAmount.DecimalLetters = currency.RoundOffAmount.DigitsBeforeFirstSignificantDigit();

            if (type != TenderDeclarationType.TenderRemoval)
            {
                decimal amount;
                if (type == TenderDeclarationType.DeclareStartAmount)
                {
                    amount = Services.Interfaces.Services.CashManagementService(DLLEntry.DataModel).GetDeclaredStartOfDayAmount(DLLEntry.DataModel);
                }
                else
                {
                    amount = Services.Interfaces.Services.CashManagementService(DLLEntry.DataModel).GetLastTenderDeclarationAmount(DLLEntry.DataModel);
                }

                ntbAmount.Value = (double) amount;
            }

            ntbAmount.LostFocus += ntbAmountLostFocus;
            ntbAddedAmount.LostFocus += ntbAmountLostFocus;
        }

        private void ntbAmountLostFocus(object sender, EventArgs eventArgs)
        {
            bool valid = startLimit == 0M ||
                         (decimal)(ntbAmount.Value + ntbAddedAmount.Value) <= startLimit;

            ntbAmount.HasError = !valid;
        }

        private void touchKeyboard1_EnterPressed(object sender, EventArgs e)
        {
            if (ActiveControl is Button)
            {
                ((Button) ActiveControl).PerformClick();
            }
            else if (ActiveControl is TextBox)
            {
                SelectNextControl(ActiveControl, true, true, false, true);
            }
        }

        private void touchKeyboard1_ObtainCultureName(object sender, CultureEventArguments args)
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

        private void ntbAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator && ntbAmount.Text == "")
            {
                ntbAmount.Text = "0" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                ntbAmount.Select(ntbAmount.Text.Length, 0);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if ((DLLEntry.Settings.Store.StartAmount == 0) || (ntbAmount.Value + ntbAddedAmount.Value) <= (double)DLLEntry.Settings.Store.StartAmount)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
