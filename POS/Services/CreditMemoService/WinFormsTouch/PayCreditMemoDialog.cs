using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    public partial class PayCreditMemoDialog : TouchBaseForm
    {
        private PosTransaction posTransaction;
        private StorePaymentMethod tenderInformation;
        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        public RecordIdentifier CreditMemoID { get; private set; }

        public decimal CreditMemoBalance { get; private set; }

        public bool CreditMemoValid { get; private set; }

        public PayCreditMemoDialog()
        {
            InitializeComponent();

            ClearPanelInformation();
        }

        public PayCreditMemoDialog(IConnectionManager entry, PosTransaction posTransaction, StorePaymentMethod tenderInfo, decimal amountDue, string creditMemoID = "") : this()
        {
            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            this.posTransaction = posTransaction;
            tenderInformation = tenderInfo;
            touchDialogBanner.BannerText = tenderInfo.Text;
            CreditMemoID = tbCreditMemoID.Text = creditMemoID;
            tbCreditMemoID.StartCharacter = dlgSettings.HardwareProfile.StartTrack1;
            tbCreditMemoID.EndCharacter = dlgSettings.HardwareProfile.EndTrack1;
            tbCreditMemoID.Seperator = dlgSettings.HardwareProfile.Separator1;
            tbCreditMemoID.TrackSeperation = TrackSeperation.Before;
            tbAmount.DecimalLetters = tenderInformation.Rounding.DigitsBeforeFirstSignificantDigit();
            tbAmount.AllowDecimal = tbAmount.DecimalLetters > 0;
            tbAmount.Value = (double)Interfaces.Services.RoundingService(dlgEntry).RoundAmount(dlgEntry, amountDue, dlgEntry.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            if (!RecordIdentifier.IsEmptyOrNull(CreditMemoID))
            {
                btnGet_Click(null, EventArgs.Empty);
            }
        }

        private void tbCreditMemo_TextChanged(object sender, EventArgs e)
        {
            if (tbCreditMemoID.Text != "" && !btnGet.Enabled)
            {
                btnGet.Enabled = true;
            }
            else if (tbCreditMemoID.Text == "" && btnGet.Enabled)
            {
                btnGet.Enabled = false;
            }
            CreditMemoID = "";
            btnOk.Enabled = lblCreditMemoAmount.Enabled = false;
            ClearPanelInformation();
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
                Interfaces.Services.DialogService(dlgEntry).ShowStatusDialog(Properties.Resources.ValidatingCreditMemo);

                if (tbCreditMemoID.Text.Trim() == "")
                {
                    tbCreditMemoID.Focus();
                    return;
                }

                CreditMemoID = tbCreditMemoID.Text.Trim();

                // Check if we already have a payment line with the same credit memo ID                                               
                bool creditVoucherInUse = false;

                List<ITenderLineItem> tenderLines = (posTransaction is RetailTransaction)
                                                    ? ((RetailTransaction)posTransaction).TenderLines
                                                    : ((CustomerPaymentTransaction)posTransaction).TenderLines.Select(x => (ITenderLineItem)x).ToList();

                foreach (TenderLineItem tenderItem in tenderLines)
                {
                    if (tenderItem is CreditMemoTenderLineItem)
                    {
                        if (((CreditMemoTenderLineItem)tenderItem).SerialNumber == (string)CreditMemoID && !tenderItem.Voided)
                        {
                            Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.CreditMemoAlreadyInUse, MessageBoxButtons.OK, MessageDialogType.Attention);

                            creditVoucherInUse = true;
                            break;
                        }
                    }
                }

                if (creditVoucherInUse)
                {
                    tbCreditMemoID.Focus();
                    tbCreditMemoID.Select(tbCreditMemoID.Text.Length, 0);
                    return;
                }

                ISiteServiceService service = Interfaces.Services.SiteServiceService(dlgEntry);
                service.StaffID = (string)dlgSettings.POSUser.ID;
                service.TerminalID = (string)dlgEntry.CurrentTerminalID;

                decimal totalCreditVoucherBalance = CreditMemoBalance;

                CreditVoucherValidationEnum result = service.ValidateCreditVoucher(dlgEntry, dlgSettings.SiteServiceProfile, ref totalCreditVoucherBalance, CreditMemoID, false);

                CreditVoucher creditVoucher;
                IRoundingService rounding = Interfaces.Services.RoundingService(dlgEntry);
                if (result == CreditVoucherValidationEnum.ValidationSuccess)
                {
                    CreditMemoValid = true;
                    creditVoucher = service.GetCreditVoucher(dlgEntry, dlgSettings.SiteServiceProfile, CreditMemoID, true);

                    Interfaces.Services.DialogService(dlgEntry).CloseStatusDialog();

                    if (creditVoucher.Currency != dlgSettings.Store.Currency) 
                    {
                        string message  = Properties.Resources.CreditMemoCurrencyAndStoreCurrencyNotSame;
                        message = message.Replace("#1", dlgSettings.Store.Currency);
                        message = message.Replace("#2", (string)creditVoucher.Currency);

                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(message, Properties.Resources.InconsistentCurrencies, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        CreditMemoValid = false;
                        return;
                    }

                    CreditMemoBalance = rounding.RoundAmount(dlgEntry, totalCreditVoucherBalance, dlgEntry.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);

                    // Check if there is a difference between the total amount on the credit memo and on the rounded credit memo balance.
                    // This is done to see if we have an old credit memo where it was issued with another rounding setting than the current
                    // payment method rounding setting
                    if (CreditMemoBalance != totalCreditVoucherBalance)
                    {
                        decimal difference = CreditMemoBalance - totalCreditVoucherBalance;

                        service.AddToCreditVoucher(dlgEntry, dlgSettings.SiteServiceProfile, CreditMemoID, difference, false);
                    }
                }
                else
                {
                    Interfaces.Services.DialogService(dlgEntry).CloseStatusDialog();
                    Interfaces.Services.CreditMemoService(dlgEntry).HandleCreditVoucherValidationEnum(dlgEntry, result);
                    CreditMemoValid = false;
                    service.Disconnect(dlgEntry);
                    return;
                }

                Currency currency = Providers.CurrencyData.Get(dlgEntry, creditVoucher.Currency, CacheType.CacheTypeApplicationLifeTime);
                tbAmount.Value = (double)rounding.RoundAmount(dlgEntry, CreditMemoBalance, dlgEntry.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);

                lblCreditMemoAmount.Enabled = btnOk.Enabled = true;
                tbCreditMemoID.Focus();
                tbCreditMemoID.Select(tbCreditMemoID.Text.Length, 0);

                lblBalance.Text = rounding.RoundString(dlgEntry,
                                        CreditMemoBalance,
                                        currency.RoundOffAmount.DigitsBeforeFirstSignificantDigit(),
                                        true,
                                        dlgSettings.Store.Currency,
                                        CacheType.CacheTypeTransactionLifeTime);
                lblIssued.Text = creditVoucher.CreatedDate?.ToShortDateString();
            }
            catch (ClientTimeNotSynchronizedException exception)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(exception.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
            catch (EndpointNotFoundException)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.CouldNotConnectToSiteService, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
            catch (Exception x)
            {
                dlgEntry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(dlgEntry).Disconnect(dlgEntry);
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

        void MSR_MSRMessageEvent(string track2)
        {
            tbCreditMemoID.Text = StringExtensions.TrackBeforeSeparator(track2, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);

            btnGet_Click(null, EventArgs.Empty);
        }

        private void PayCreditMemoDialog_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                Scanner.ScannerMessageEvent += Scanner_ScannerMessageEvent;
                Scanner.ReEnableForScan();
                MSR.MSRMessageEvent += MSR_MSRMessageEvent;
                MSR.EnableForSwipe();
            }
        }

        private void Scanner_ScannerMessageEvent(DataLayer.BusinessObjects.BarCodes.ScanInfo scanInfo)
        {
            try
            {
                Scanner.DisableForScan();

                tbCreditMemoID.Text = scanInfo.ScanDataLabel;
                btnGet_Click(null, EventArgs.Empty);
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        private void PayCreditMemoDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!DesignMode)
            {
                Scanner.ScannerMessageEvent -= Scanner_ScannerMessageEvent;
                Scanner.DisableForScan();
                MSR.MSRMessageEvent -= MSR_MSRMessageEvent;
                MSR.DisableForSwipe();
            }
        }

        private void tbCreditMemoID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbCreditMemoID.Text = StringExtensions.TrackBeforeSeparator(tbCreditMemoID.Text, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);

                btnGet_Click(sender, EventArgs.Empty);
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