using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using System;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    public partial class GetGiftCardBalanceDialog : TouchBaseForm
    {
        private decimal giftCardBalance;
        private string oldText;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        public GetGiftCardBalanceDialog(IConnectionManager entry)
        {
            InitializeComponent();

            ClearInformation();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            GiftCardActivated = false;
            giftCardBalance = 0;
            oldText = txtGiftCardID.Text;

            if(!DesignMode)
            {
                txtGiftCardID.StartCharacter = dlgSettings.HardwareProfile.StartTrack1;
                txtGiftCardID.EndCharacter = dlgSettings.HardwareProfile.EndTrack1;
                txtGiftCardID.Seperator = dlgSettings.HardwareProfile.Separator1;
                txtGiftCardID.TrackSeperation = TrackSeperation.Before;
                Scanner.ScannerMessageEvent += ScannerOnScannerMessageEvent;
                Scanner.ReEnableForScan();
            }
        }

        private void ScannerOnScannerMessageEvent(ScanInfo scanInfo)
        {
            try
            {
                txtGiftCardID.Text = scanInfo.ScanDataLabel;
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        /// <summary>
        /// The ID of the gift card
        /// </summary>
        public RecordIdentifier GiftCardId
        {
            get
            {
                return txtGiftCardID.Text;
            }
        }

        /// <summary>
        /// If true, then the POS user has elected to refill the gift card
        /// </summary>
        public bool RefillingGiftcard { get; set; }

        public GiftCard ExistingGiftCard { get; private set; }

        /// <summary>
        /// If using this dialog for activating gift cards, this value will be true if the gift card
        /// was activated successfully, false otherwise.
        /// </summary>
        public bool GiftCardActivated { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!DesignMode)
            {
                MSR.MSRMessageEvent += new MSR.MSRMessageDelegate(MSR_MSRMessageEvent);
                MSR.EnableForSwipe();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            txtGiftCardID.Focus();
        }
        protected override void OnClosed(EventArgs e)
        {
            if (!DesignMode)
            {
                MSR.MSRMessageEvent -= new MSR.MSRMessageDelegate(MSR_MSRMessageEvent);
                MSR.DisableForSwipe();
                Scanner.ScannerMessageEvent -= ScannerOnScannerMessageEvent;
                Scanner.DisableForScan();
            }

            base.OnClosed(e);
        }

        void MSR_MSRMessageEvent(string track2)
        {
            txtGiftCardID.Text = track2;
        }

        private void touchKeyboard_EnterPressed(object sender, EventArgs e)
        {
            if (ActiveControl == txtGiftCardID && btnGet.Enabled)
            {
                btnGet_Click(this, EventArgs.Empty);
            }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            bool textChanged = oldText != txtGiftCardID.Text;
            btnGet.Enabled = txtGiftCardID.Text.Length > 0 && textChanged;

            if (textChanged)
            {
                oldText = txtGiftCardID.Text;
                ClearInformation();
            }
        }

        private void ClearInformation()
        {
            lblBalance.Text = "-";
            lblIssued.Text = "-";
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

        private void btnGet_Click(object sender, EventArgs e)
        {
            txtGiftCardID.Text = txtGiftCardID.Text.TrackBeforeSeparator(dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);

            if (txtGiftCardID.Text == "")
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.NoGiftCardIDPleaseEnter, MessageBoxButtons.OK, MessageDialogType.Generic);
                txtGiftCardID.Focus();
                return;
            }

            GiftCard giftCard = null;
            ISiteServiceService service = (ISiteServiceService)dlgEntry.Service(ServiceType.SiteServiceService);
            try
            {
                Interfaces.Services.DialogService(dlgEntry).ShowStatusDialog(Resources.CheckingGiftCard);
                giftCard = service.GetGiftCard(dlgEntry, dlgSettings.SiteServiceProfile, txtGiftCardID.Text, true);
            }
            catch (Exception ex)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Resources.CouldNotConnectToStoreServer, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return;
            }
            finally
            {
                Interfaces.Services.DialogService(dlgEntry).CloseStatusDialog();
            }

            if (giftCard != null)
            {
                if ((string)giftCard.Currency != dlgSettings.Store.Currency)
                {
                    string errorMessage = Resources.GiftCardCurrencyMismatchError.Replace("#1", dlgSettings.Store.Currency);
                    errorMessage = errorMessage.Replace("#2", (string)giftCard.Currency);

                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(errorMessage, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    txtGiftCardID.Focus();
                    return;
                }
                if (!giftCard.Active)
                {
                    // The card isn't active
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.GiftCardNotActive, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return;
                }
                giftCardBalance = giftCard.Balance;
            }
            else
            {
                // The card does not exist
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.GiftCardNotFound, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return;
            }

            //Format the balance value and show it
            IRoundingService rounding = (IRoundingService)dlgEntry.Service(ServiceType.RoundingService);
            Currency currency = Providers.CurrencyData.Get(dlgEntry, giftCard.Currency, CacheType.CacheTypeApplicationLifeTime);
            string balanceforDisplay = rounding.RoundString(
                    dlgEntry,
                    giftCardBalance,
                    currency.RoundOffAmount.DigitsBeforeFirstSignificantDigit(),
                    true,
                    dlgSettings.Store.Currency,
                    CacheType.CacheTypeTransactionLifeTime);
            lblBalance.Text = balanceforDisplay;
            lblIssued.Text = giftCard?.CreatedDate?.ToShortDateString();
            txtGiftCardID.Focus();
            CheckEnabled(sender, e);
        }

        private void txtGiftCardID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (btnGet.Enabled)
                {
                    btnGet_Click(sender, e);
                }
            }
        }
    }
}