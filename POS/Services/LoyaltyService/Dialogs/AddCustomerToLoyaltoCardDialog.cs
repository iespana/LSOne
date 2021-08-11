using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Services.Dialogs
{
    public partial class AddCustomerToLoyaltoCardDialog : TouchBaseForm
    {
        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        private HardwareProfile hardwareProfile;
        private IPosTransaction transaction;

        private LoyaltyPointStatus pointStatus = new LoyaltyPointStatus();
        private RecordIdentifier lastCheckedCardNumber;
        private bool cardNotValid = false;
        private bool cardNeedsValidation = false;
        private bool cardHasBeenValidatedOnce = false; // Used to make sure we don't display an error before we have actually entered/validated information

        private bool customerNotValid = false;
        private bool customerNeedsValidation = false;
        private bool customerHasBeenValidatedOnce = false; // Used to make sure we don't display an error before we have actually entered/validated information

        public RecordIdentifier LoyaltyCardNumer { get; set; }
        public RecordIdentifier CustomerID { get; set; }

        public AddCustomerToLoyaltoCardDialog(IConnectionManager entry, Customer customer, IPosTransaction transaction)
        {
            InitializeComponent();

            ClearLoyaltyInformation();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            hardwareProfile = dlgSettings.HardwareProfile;

            this.transaction = transaction;

            LoyaltyCardNumer = RecordIdentifier.Empty;
            CustomerID = RecordIdentifier.Empty;
            if (!DesignMode)
            {
                tbLoyaltyCardNumber.StartCharacter = hardwareProfile.StartTrack1;
                tbLoyaltyCardNumber.EndCharacter = hardwareProfile.EndTrack1;
                tbLoyaltyCardNumber.Seperator = hardwareProfile.Separator1;
                tbLoyaltyCardNumber.TrackSeperation = TrackSeperation.Before;

                tbCustomer.StartCharacter = hardwareProfile.StartTrack1;
                tbCustomer.EndCharacter = hardwareProfile.EndTrack1;
                tbCustomer.Seperator = hardwareProfile.Separator1;
                tbCustomer.TrackSeperation = TrackSeperation.Before;
            }

            if(customer != null)
            {                
                CustomerID = (string)customer.ID;

                SetCustomerControlText(customer);

                btnCustomerSearch.Enabled = false;
                tbCustomer.ReadOnly = true;
            }
        }

        private void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            customerHasBeenValidatedOnce = true;
            customerNeedsValidation = false;
            Customer customer = tbCustomer.Text.Trim() == "" ? Interfaces.Services.CustomerService(dlgEntry).Search(dlgEntry, true, transaction) : FindCustomer(tbCustomer.Text, new ScanInfo(tbCustomer.Text));

            if (customer != null)
            {
                if (customer.Blocked != BlockedEnum.Nothing)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CustomerIsBlockedAndCannotBeAdded);
                    btnCustomerSearch.Focus();
                }

                CustomerID = (string)customer.ID;

                SetCustomerControlText(customer);

                customerNotValid = false;
            }
            else
            {
                customerNotValid = true;
            }

            ValidateData();

            CheckEnabled(this, EventArgs.Empty);
        }

        private void SetCustomerControlText(Customer customer)
        {
            string customerName = customer.GetFormattedName(dlgEntry.Settings.NameFormatter);
            if (customerName == string.Empty)
            {
                customerName = customer.Text;
            }

            tbCustomer.Text = customerName;
        }

        private Customer FindCustomer(RecordIdentifier customerID, ScanInfo scanInfo)
        {            
            Customer customer = Providers.CustomerData.Get(dlgEntry, customerID, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);

            if (customer == null && scanInfo != null)
            {
                customer = Providers.CustomerData.GetByCardNumber(dlgEntry, scanInfo.ScanDataLabel, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);
            }

            if (customer == null)
            {
                List<CustomerListItem> customers = Providers.CustomerData.Search(dlgEntry, tbCustomer.Text, 0, 2, false, DataLayer.DataProviders.Customers.CustomerSorting.ID, false);

                if (customers.Count > 0)
                {
                    if (customers.Count == 1)
                    {
                        customer = Providers.CustomerData.Get(dlgEntry, customers[0].ID, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);
                    }
                    else
                    {
                        customer = Interfaces.Services.CustomerService(dlgEntry).Search(dlgEntry, true, transaction, tbCustomer.Text.Trim());
                    }
                }
            }

            return customer;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            btnOk.DialogResult = DialogResult.OK;            

            if (!DesignMode)
            {
                Scanner.ScannerMessageEvent += ScannerOnScannerMessageEvent;
                Scanner.ReEnableForScan();
                MSR.MSRMessageEvent += MSR_MSRMessageEvent;
                MSR.EnableForSwipe();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (!DesignMode)
            {
                Scanner.ScannerMessageEvent -= ScannerOnScannerMessageEvent;
                Scanner.DisableForScan();
                MSR.MSRMessageEvent -= MSR_MSRMessageEvent;
                MSR.DisableForSwipe();
            }

            base.OnClosed(e);
        }

        void MSR_MSRMessageEvent(string track2)
        {
            string track = StringExtensions.TrackBeforeSeparator(track2, hardwareProfile.StartTrack1, hardwareProfile.Separator1, hardwareProfile.EndTrack1);

            if (tbCustomer.Focused)
            {
                tbCustomer.Text = track;
                btnCustomerSearch_Click(this, EventArgs.Empty);
            }
            else
            {
                tbLoyaltyCardNumber.Text = track;
                btnGet_Click(this, EventArgs.Empty);
            }
        }

        private void ScannerOnScannerMessageEvent(ScanInfo scanInfo)
        {
            try
            {
                Scanner.DisableForScan();

                if(tbCustomer.Focused)
                {
                    tbCustomer.Text = scanInfo.ScanDataLabel;
                    btnCustomerSearch_Click(this, EventArgs.Empty);
                }
                else
                {
                    tbLoyaltyCardNumber.Text = scanInfo.ScanDataLabel;
                    btnGet_Click(this, EventArgs.Empty);
                }
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(cardNeedsValidation)
            {
                GetAndDisplayLoyaltyInformation();

                if(cardNotValid)
                {
                    tbLoyaltyCardNumber.Focus();
                    DialogResult = DialogResult.None;
                    return;
                }
            }

            if(customerNeedsValidation)
            {
                btnCustomerSearch_Click(this, EventArgs.Empty);

                if(customerNotValid)
                {
                    tbCustomer.Focus();
                    DialogResult = DialogResult.None;
                    return;
                }
            }

            if (tbLoyaltyCardNumber.Text.Trim() == "")
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.NoLoyaltyCardHasBeenEntered);
                tbLoyaltyCardNumber.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            if (RecordIdentifier.IsEmptyOrNull(CustomerID))
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.NoCustomerHasBeenSelected);
                btnCustomerSearch.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            LoyaltyCardNumer = tbLoyaltyCardNumber.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            GetAndDisplayLoyaltyInformation();            
        }

        private void GetAndDisplayLoyaltyInformation()
        {
            cardHasBeenValidatedOnce = true;
            cardNeedsValidation = false;

            tbLoyaltyCardNumber.Text = tbLoyaltyCardNumber.Text.Trim();
            pointStatus.CardNumber = tbLoyaltyCardNumber.Text == "" ? RecordIdentifier.Empty : tbLoyaltyCardNumber.Text;

            if (pointStatus.CardNumber == RecordIdentifier.Empty || pointStatus.CardNumber == lastCheckedCardNumber)
            {
                return;
            }

            lastCheckedCardNumber = pointStatus.CardNumber;

            GetLoyaltyInformation();

            ValidateData();

            CheckEnabled(this, EventArgs.Empty);
        }

        private void GetLoyaltyInformation()
        {
            cardNotValid = false;
            LoyaltyItem loyaltyItem = new LoyaltyItem();

            loyaltyItem.CardNumber = tbLoyaltyCardNumber.Text;

            loyaltyItem.CurrentValue = pointStatus.CurrentValue;

            CalculationHelper calculationHelper = new CalculationHelper();
            LoyaltyMSRCard card = calculationHelper.GetLoyaltyInfo(dlgEntry, loyaltyItem);

            if (card == null)
            {
                cardNotValid = true;
                ClearLoyaltyInformation();
                return;
            }

            IRoundingService rounding = (IRoundingService)dlgEntry.Service(ServiceType.RoundingService);

            lblCardScheme.Text = loyaltyItem.SchemeExists ? card.SchemeDescription : Resources.SchemeNotFound;

            lblPointUseLimit.Text = card.UsePointsLimit == decimal.Zero ? Resources.None : rounding.RoundString(dlgEntry, card.UsePointsLimit, 0, false, "");

            ISiteServiceService service = (ISiteServiceService)dlgEntry.Service(ServiceType.SiteServiceService);
            LoyaltyPoints rule = service.GetPointsExchangeRate(dlgEntry, dlgSettings.SiteServiceProfile, card.SchemeID, false);

            pointStatus.CurrentValue = decimal.Zero;
            if (rule != null)
            {
                // Because earlier loyalty implementations allowed negative points we need to do this here
                rule.Points = rule.Points < decimal.Zero ? rule.Points * -1 : rule.Points;
                pointStatus.CurrentValue = rule.Points > decimal.Zero ? (rule.QtyAmountLimit / rule.Points * card.StartingPoints) : decimal.Zero;
            }

            Currency storeCurrency = Providers.CurrencyData.Get(dlgEntry, dlgSettings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
            lblStartingPoints.Text = rounding.RoundString(dlgEntry, card.StartingPoints, 0, false, "");
            lblValue.Text = $"{storeCurrency.Symbol} {rounding.RoundString(dlgEntry, pointStatus.CurrentValue, storeCurrency.ID, false, CacheType.CacheTypeTransactionLifeTime)}";
        }

        private void ClearLoyaltyInformation()
        {
            lblCardScheme.Text = "-";
            lblStartingPoints.Text = "-";
            lblPointUseLimit.Text = "-";
            lblValue.Text = "-";
        }

        private void pnlInfo_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, pnlInfo.Width - 1, pnlInfo.Height - 1);
            p.Dispose();
        }

        private void touchKeyboard_ObtainCultureName(object sender, Controls.EventArguments.CultureEventArguments args)
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

        private void tbLoyaltyCardNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btnGet.Enabled)
            {
                tbLoyaltyCardNumber.Text = StringExtensions.TrackBeforeSeparator(tbLoyaltyCardNumber.Text, hardwareProfile.StartTrack1, hardwareProfile.Separator1, hardwareProfile.EndTrack1);
                btnGet_Click(sender, e);
            }
        }

        private void tbCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbCustomer.Text = StringExtensions.TrackBeforeSeparator(tbCustomer.Text, hardwareProfile.StartTrack1, hardwareProfile.Separator1, hardwareProfile.EndTrack1);
                btnCustomerSearch_Click(null, EventArgs.Empty);
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

        private void CheckEnabled(object sender, EventArgs args)
        {
            btnOk.Enabled = !String.IsNullOrEmpty(tbCustomer.Text)
                            && !String.IsNullOrEmpty(tbLoyaltyCardNumber.Text)
                            && !tbLoyaltyCardNumber.HasError
                            && !tbCustomer.HasError;
        }

        private void ValidateData()
        {
            touchErrorProvider.Clear();
            bool loyaltyCardHasError = false;
            bool customerHasError = false;

            // Loyalty card
            if (cardHasBeenValidatedOnce && string.IsNullOrEmpty(tbLoyaltyCardNumber.Text))
            {                
                touchErrorProvider.AddErrorMessage(Properties.Resources.PleaseEnterLoyaltyCardNumber);
                loyaltyCardHasError = true;
            }

            else if (cardNotValid)
            {                
                touchErrorProvider.AddErrorMessage(Properties.Resources.ErrLoyaltyCardNotFound);
                loyaltyCardHasError = true;
            }

            // Customer
            if(customerHasBeenValidatedOnce && string.IsNullOrEmpty(tbCustomer.Text))
            {
                touchErrorProvider.AddErrorMessage(Properties.Resources.NoCustomerHasBeenSelected);
                customerHasError = true;
            }
            else if(customerNotValid)
            {
                touchErrorProvider.AddErrorMessage(Properties.Resources.ErrLoyaltyCustomerNotFound);
                customerHasError = true;
            }

            tbLoyaltyCardNumber.HasError = loyaltyCardHasError;
            tbCustomer.HasError = customerHasError;

            touchErrorProvider.Visible = loyaltyCardHasError || customerHasError;
        }

        private void tbLoyaltyCardNumber_LostFocus(object sender, EventArgs e)
        {
            if(cardNeedsValidation)
            {
                GetAndDisplayLoyaltyInformation();
            }
        }

        private void tbLoyaltyCardNumber_TextChanged(object sender, EventArgs e)
        {
            cardNeedsValidation = true;

            CheckEnabled(this, EventArgs.Empty);
        }

        private void tbCustomer_TextChanged(object sender, EventArgs e)
        {
            customerNeedsValidation = true;

            CheckEnabled(this, EventArgs.Empty);
        }
    }
}