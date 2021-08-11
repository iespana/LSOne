using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Drawing;
using System.ServiceModel;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    public partial class PayGiftCardDialog : TouchBaseForm
    {
        private decimal amountDue;
        private decimal giftCardBalance;
        private StorePaymentMethod tenderInformation;
        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        public RecordIdentifier GiftCardID { get; set; }
        public GiftCard GiftCard { get; private set; }
        public decimal RegisteredAmount { get; private set; }
        public decimal UnRoundedRegisteredAmount { get; private set; }

        public PayGiftCardDialog(IConnectionManager entry)
        {
            InitializeComponent();

            ClearPanelInformation();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
        }

        public PayGiftCardDialog(IConnectionManager entry, decimal amountDue, StorePaymentMethod tenderInformation, string giftCardID = "")
            : this(entry)
        {
            this.amountDue = Interfaces.Services.RoundingService(dlgEntry).RoundAmount(dlgEntry, amountDue, dlgEntry.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            this.tenderInformation = tenderInformation;
            ntbAmount.DecimalLetters =  tenderInformation.Rounding.DigitsBeforeFirstSignificantDigit();
            ntbAmount.AllowDecimal =  ntbAmount.DecimalLetters > 0;
            ntbAmount.Value = (double)this.amountDue;
            GiftCardID = tbGiftCardID.Text = giftCardID;
            touchDialogBanner.BannerText = tenderInformation.Text;
            touchDialogBanner.BannerText += " - ";
            touchDialogBanner.BannerText += Resources.AmountDue.Replace("#1",
                                                               Interfaces.Services.RoundingService(dlgEntry).RoundString(dlgEntry,
                                                                                            this.amountDue,
                                                                                            tenderInformation.Rounding.DigitsBeforeFirstSignificantDigit(),
                                                                                            true,
                                                                                            dlgSettings.Store.Currency,
                                                                                            CacheType.CacheTypeApplicationLifeTime));
            if (!DesignMode)
            {
                MSR.MSRMessageEvent += MSROnMsrMessageEvent;
                MSR.EnableForSwipe();
                tbGiftCardID.StartCharacter = dlgSettings.HardwareProfile.StartTrack1;
                tbGiftCardID.EndCharacter = dlgSettings.HardwareProfile.EndTrack1;
                tbGiftCardID.Seperator = dlgSettings.HardwareProfile.Separator1;
                tbGiftCardID.TrackSeperation = TrackSeperation.Before;
                Scanner.ScannerMessageEvent += ScannerOnScannerMessageEvent;
                Scanner.ReEnableForScan();
            }
            if (!RecordIdentifier.IsEmptyOrNull(GiftCardID))
            {
                btnGet_Click(null, EventArgs.Empty);
            }
        }

        private void ScannerOnScannerMessageEvent(ScanInfo scanInfo)
        {
            try
            {
                tbGiftCardID.Text = scanInfo.ScanDataLabel;
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (!DesignMode)
            {
                MSR.MSRMessageEvent -= MSROnMsrMessageEvent;
                MSR.DisableForSwipe();
                Scanner.ScannerMessageEvent -= ScannerOnScannerMessageEvent;
                Scanner.DisableForScan();
            }

            base.OnClosed(e);
        }

        private void MSROnMsrMessageEvent(string track2)
        {
            tbGiftCardID.Text = track2;
            btnGet_Click(this, EventArgs.Empty);
        }

        private void tbGiftCardID_TextChanged(object sender, EventArgs e)
        {
            if (tbGiftCardID.Text != "" && !btnGet.Enabled)
            {
                btnGet.Enabled = true;
            }
            else if (tbGiftCardID.Text == "" && btnGet.Enabled)
            {
                btnGet.Enabled = false;
            }
            GiftCardID = RecordIdentifier.Empty;
            ClearPanelInformation();
            btnOk.Enabled = false;
            ntbAmount.Value = (double)amountDue;
            ntbAmount.Enabled = true;
        }

        private void ClearPanelInformation()
        {
            lblBalance.Text = "-";
            lblIssued.Text = "-";
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbGiftCardID.Text.Trim() == "")
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.NoGiftCardIDPleaseEnter, MessageBoxButtons.OK, MessageDialogType.Generic);
                    return;
                }

                string giftCardId = tbGiftCardID.Text;

                ISiteServiceService service = (ISiteServiceService) dlgEntry.Service(ServiceType.SiteServiceService);
                service.StaffID = dlgSettings.POSUser.ID;
                service.TerminalID = dlgEntry.CurrentTerminalID;

                decimal giftCardAmount = 0;
                bool giftCardCurrencyMismatch = false;

                // Check for result and currency
                Interfaces.Services.DialogService(dlgEntry).ShowStatusDialog(Resources.ValidatingGiftCard);

                // First make sure that the currency of the store and currency of the gift card match
                GiftCard giftCard = service.GetGiftCard(dlgEntry, dlgSettings.SiteServiceProfile, giftCardId, false);

                // null if the gift card was not found
                if (giftCard == null)
                {
                    GiftCardID = RecordIdentifier.Empty;
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.GiftCardNotFound, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    tbGiftCardID.Focus();
                    return;
                }

                if ((string) giftCard.Currency != dlgSettings.Store.Currency)
                {
                    giftCardCurrencyMismatch = true;
                }

                GiftCardValidationEnum result = service.ValidateGiftCard(dlgEntry, dlgSettings.SiteServiceProfile, ref giftCardAmount, new RecordIdentifier(giftCardId), true);

                if (result != GiftCardValidationEnum.ValidationSuccess || giftCardCurrencyMismatch)
                {
                    GiftCardID = RecordIdentifier.Empty;
                    GiftCard = null;
                }

                // If the currency does not match, we display an error message at this point and skip evaluating the result
                if (giftCardCurrencyMismatch)
                {
                    string message = Resources.GiftCardCurrencyMismatchError.Replace("#1", dlgSettings.Store.Currency);
                    message = message.Replace("#2", (string) giftCard.Currency);

                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return;
                }

                IRoundingService rounding = (IRoundingService) dlgEntry.Service(ServiceType.RoundingService);
                this.giftCardBalance = giftCardAmount;
                Currency currency = Providers.CurrencyData.Get(dlgEntry, giftCard.Currency, CacheType.CacheTypeApplicationLifeTime);
                string balanceForDisplay = rounding.RoundString(
                    dlgEntry,
                    giftCardAmount,
                    currency.RoundOffAmount.DigitsBeforeFirstSignificantDigit(),
                    true,
                    dlgSettings.Store.Currency,
                    CacheType.CacheTypeTransactionLifeTime);

                switch (result)
                {
                    case GiftCardValidationEnum.ValidationBalanceToLow:
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.GiftCardBalanceToLow + " " + balanceForDisplay, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        break;

                    case GiftCardValidationEnum.ValidationCardNotActive:
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.GiftCardNotActive, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        break;

                    case GiftCardValidationEnum.ValidationCardNotFound:
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.GiftCardNotFound, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        break;

                    case GiftCardValidationEnum.ValidationSuccess:
                        lblBalance.Text = balanceForDisplay;
                        lblIssued.Text = giftCard.CreatedDate?.ToShortDateString();
                        GiftCardID = giftCardId;
                        GiftCard = giftCard;
                        
                        btnOk.Enabled = true;
                        btnGet.Enabled = false;
                        if (!GiftCard.Refillable)
                        {
                            ntbAmount.Value = (double)GiftCard.Balance;
                            ntbAmount.Enabled = false;
                        }
                        else
                        {
                            ntbAmount.Value = (double) (amountDue < giftCard.Balance ? amountDue : giftCard.Balance);
                        }
                        break;

                    case GiftCardValidationEnum.ValidationUnknownError:
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.UnknownErrorOccurred, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        break;

                    case GiftCardValidationEnum.ValidationCardHasZeroBalance:
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.GiftCardHasZeroBalance, MessageBoxButtons.OK, MessageDialogType.Generic);
                        lblBalance.Text = balanceForDisplay;
                        lblIssued.Text = giftCard.CreatedDate?.ToShortDateString();
                        break;
                }
            }
            catch (ClientTimeNotSynchronizedException ex)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(ex.Message, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.Attention);
            }
            catch (EndpointNotFoundException)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CouldNotConnectToStoreServer, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                
            }
            catch (Exception x)
            {

                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.GiftCardNotFound, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                
                dlgEntry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
            }
            finally
            {
                Interfaces.Services.DialogService(dlgEntry).CloseStatusDialog();
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            IRoundingService rounding = Interfaces.Services.RoundingService(dlgEntry);
            decimal roundedGiftCardBalance = rounding.RoundAmount(dlgEntry, giftCardBalance, dlgEntry.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            decimal roundedAmountDue = rounding.RoundAmount(dlgEntry, amountDue, dlgEntry.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            if (ntbAmount.Value <= 0)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.NoAmountPleaseEnter, MessageBoxButtons.OK, MessageDialogType.Generic);
                return;
            }
            if (ntbAmount.Value > (double)roundedGiftCardBalance)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.GiftCardBalanceToLow + " " + 
                    Interfaces.Services.RoundingService(dlgEntry).RoundString(dlgEntry, 
                                                                                            giftCardBalance, 
                                                                                            tenderInformation.Rounding.DigitsBeforeFirstSignificantDigit(),
                                                                                            true,
                                                                                            dlgSettings.Store.Currency,
                                                                                            CacheType.CacheTypeApplicationLifeTime), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return;
            }
            if (GiftCard.Refillable && (decimal)ntbAmount.Value > roundedAmountDue)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.OverTenderNotAllowedOnRefillable, MessageBoxButtons.OK, MessageDialogType.Attention);
                return;
            }
            decimal UnRounded = rounding.RoundAmount(dlgEntry, GiftCard.Balance, dlgEntry.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            RegisteredAmount = rounding.RoundAmount(dlgEntry, (decimal)ntbAmount.Value, dlgEntry.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            UnRoundedRegisteredAmount = UnRounded == RegisteredAmount ? GiftCard.Balance : 0;
            DialogResult = DialogResult.OK;
            Close();
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

        private void tbGiftCardID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnGet_Click(sender, e);
            }
        }

        private void pnlInfo_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, pnlInfo.Width - 1, pnlInfo.Height - 1);
            p.Dispose();
        }
    }
}