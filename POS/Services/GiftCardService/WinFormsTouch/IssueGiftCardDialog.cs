using LSOne.Controls;
using LSOne.Controls.Dialogs;
using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    public partial class IssueGiftCardDialog : TouchBaseForm
    {
        SiteServiceProfile.IssueGiftCardOptionEnum giftCardOptions;
        public bool IsChangeBack { get; set; }
        private StorePaymentMethod tenderInfo;
        private IPosTransaction POSTransaction;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        /// <summary>
        /// Default constructor for designer and Babylon
        /// </summary>
        public IssueGiftCardDialog(IConnectionManager entry)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
        }

        public IssueGiftCardDialog(IConnectionManager entry, StorePaymentMethod tenderInfo, bool cancelAllowed = true, IPosTransaction posTransaction = null)
            :this(entry)
        {
            GiftCardActivated = false;
            this.tenderInfo = tenderInfo;

            Currency storeCurrency = Providers.CurrencyData.Get(dlgEntry, dlgSettings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);

            DecimalLimit limiter = new DecimalLimit(DecimalExtensions.DigitsBeforeFirstSignificantDigit(storeCurrency.RoundOffSales));
            
            ntbAmount.SetValueWithLimit(0.0M, limiter);
            ntbAmount.Text = "";
            btnCancel.Enabled = cancelAllowed;
            if(!DesignMode)
            {
                txtGiftCardID.StartCharacter = dlgSettings.HardwareProfile.StartTrack1;
                txtGiftCardID.EndCharacter = dlgSettings.HardwareProfile.EndTrack1;
                txtGiftCardID.Seperator = dlgSettings.HardwareProfile.Separator1;
                txtGiftCardID.TrackSeperation = TrackSeperation.Before;
                Scanner.ScannerMessageEvent += ScannerOnScannerMessageEvent;
                Scanner.ReEnableForScan();
            }

            POSTransaction = posTransaction;
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
        /// The amount to create a gift card with
        /// </summary>
        public decimal Amount 
        {
            get
            {
                return (decimal)ntbAmount.Value;
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

        public SiteServiceProfile.IssueGiftCardOptionEnum GiftCardOptions
        {
            get
            {
                return giftCardOptions;
            }
            set
            {
                giftCardOptions = value;

                switch (giftCardOptions)
                {
                    // Only display the gift card textbox and the OK/Cancel button
                    case SiteServiceProfile.IssueGiftCardOptionEnum.ID:
                        ntbAmount.Visible = false;
                        lblAmount.Visible = false;

                        lblGiftCardID.Visible = txtGiftCardID.Visible = true;
                        touchScrollButtonPanel.Clear();

                        Height = 555;

                        break;

                    // Only display numpad and OK/Cancel buttons
                    case SiteServiceProfile.IssueGiftCardOptionEnum.Amount:                        
                        lblGiftCardID.Visible = txtGiftCardID.Visible = false;

                        lblAmount.Visible = true;
                        ntbAmount.Visible = true;
                        touchScrollButtonPanel.SetCurrencyBills(dlgSettings.Store.Currency, dlgSettings.SiteServiceProfile.MaximumGiftCardAmount);

                        Height = 555;

                        break;

                    case SiteServiceProfile.IssueGiftCardOptionEnum.IDAndAmount:
                        lblGiftCardID.Visible = txtGiftCardID.Visible = true;

                        lblAmount.Visible = true;
                        ntbAmount.Visible = true;
                        touchScrollButtonPanel.SetCurrencyBills(dlgSettings.Store.Currency, dlgSettings.SiteServiceProfile.MaximumGiftCardAmount);
                        break;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!DesignMode)
            {
                MSR.MSRMessageEvent += new MSR.MSRMessageDelegate(MSR_MSRMessageEvent);
                MSR.EnableForSwipe();
            }
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

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            txtGiftCardID.Focus();
        }

        private void MSR_MSRMessageEvent(string track2)
        {
            if (giftCardOptions == SiteServiceProfile.IssueGiftCardOptionEnum.ID || giftCardOptions == SiteServiceProfile.IssueGiftCardOptionEnum.IDAndAmount)
            {
                txtGiftCardID.Text = StringExtensions.TrackBeforeSeparator(track2, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);

                if (giftCardOptions == SiteServiceProfile.IssueGiftCardOptionEnum.IDAndAmount)
                {
                    ntbAmount.Focus();
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            GiftCard giftCard = null;

            txtGiftCardID.Text = txtGiftCardID.Text.TrackBeforeSeparator(dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);

            IDialogService dialogService = Interfaces.Services.DialogService(dlgEntry);

            switch (giftCardOptions)
            {
                case SiteServiceProfile.IssueGiftCardOptionEnum.ID:
                    if (txtGiftCardID.Text == "")
                    {
                        dialogService.ShowMessage(Resources.NoGiftCardIDPleaseEnter, MessageBoxButtons.OK, MessageDialogType.Generic);
                        txtGiftCardID.Focus();
                        return;
                    }

                    try
                    {
                        dialogService.ShowStatusDialog(Resources.CheckingGiftCard);
                        var service = (ISiteServiceService)dlgEntry.Service(ServiceType.SiteServiceService);
                        giftCard = service.GetGiftCard(dlgEntry, dlgSettings.SiteServiceProfile, txtGiftCardID.Text, true);
                    }
                    catch (Exception ex)
                    {
                        dialogService.ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Resources.CouldNotConnectToStoreServer, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return;
                    }
                    finally
                    {
                        dialogService.CloseStatusDialog();
                    }

                    if (giftCard != null)
                    {
                        if (IsChangeBack && !giftCard.Refillable)
                        {
                            dialogService.ShowMessage(Resources.GiftcardExistsNotRefillable, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                            return;
                        }

                        if ((string)giftCard.Currency != dlgSettings.Store.Currency)
                        {
                            string errorMessage = Resources.GiftCardCurrencyMismatchError.Replace("#1", dlgSettings.Store.Currency);
                            errorMessage = errorMessage.Replace("#2", (string)giftCard.Currency);

                            dialogService.ShowMessage(errorMessage, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                            txtGiftCardID.Focus();
                            return;
                        }
                    }

                    break;
                case SiteServiceProfile.IssueGiftCardOptionEnum.Amount:
                    if (Amount == 0)
                    {
                        dialogService.ShowMessage(Resources.NoAmountPleaseEnter, MessageBoxButtons.OK, MessageDialogType.Generic);
                        ntbAmount.Focus();
                        return;
                    }
                    if (Amount > dlgSettings.SiteServiceProfile.MaximumGiftCardAmount && dlgSettings.SiteServiceProfile.MaximumGiftCardAmount != 0)
                    {
                        string roundedMaximum = Interfaces.Services.RoundingService(dlgEntry).RoundForDisplay(dlgEntry, 
                                                                                                             dlgSettings.SiteServiceProfile.MaximumGiftCardAmount, 
                                                                                                             true, 
                                                                                                             false, 
                                                                                                             dlgSettings.Store.Currency, 
                                                                                                             CacheType.CacheTypeApplicationLifeTime);
                        dialogService.ShowMessage(Resources.CurrentAmountExceedsMaximum.Replace("#1", roundedMaximum), MessageBoxButtons.OK, MessageDialogType.Generic);
                        ntbAmount.Focus();
                        return;
                    }
                    break;
                case SiteServiceProfile.IssueGiftCardOptionEnum.IDAndAmount:
                    if (Amount == 0)
                    {
                        dialogService.ShowMessage(Resources.NoAmountPleaseEnter, MessageBoxButtons.OK, MessageDialogType.Generic);
                        ntbAmount.Focus();
                        return;
                    }
                    if (Amount > dlgSettings.SiteServiceProfile.MaximumGiftCardAmount && dlgSettings.SiteServiceProfile.MaximumGiftCardAmount != 0)
                    {
                        string roundedMaximum = Interfaces.Services.RoundingService(dlgEntry).RoundForDisplay(dlgEntry,
                                                                                                             dlgSettings.SiteServiceProfile.MaximumGiftCardAmount,
                                                                                                             true,
                                                                                                             false,
                                                                                                             dlgSettings.Store.Currency,
                                                                                                             CacheType.CacheTypeApplicationLifeTime);
                        dialogService.ShowMessage(Resources.CurrentAmountExceedsMaximum.Replace("#1", roundedMaximum), MessageBoxButtons.OK, MessageDialogType.Generic);
                        ntbAmount.Focus();
                        return;
                    }
                    if (txtGiftCardID.Text == "")
                    {
                        dialogService.ShowMessage(Resources.NoGiftCardIDPleaseEnter, MessageBoxButtons.OK, MessageDialogType.Generic);
                        txtGiftCardID.Focus();
                        return;
                    }

                    // Check for existing gift card, and if it is refillable
                    try
                    {
                        dialogService.ShowStatusDialog(Resources.CheckingGiftCard);
                        var service = (ISiteServiceService)dlgEntry.Service(ServiceType.SiteServiceService);
                        giftCard = service.GetGiftCard(dlgEntry, dlgSettings.SiteServiceProfile,  txtGiftCardID.Text, true);
                    }
                    catch (Exception ex)
                    {
                        dialogService.ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Resources.CouldNotConnectToStoreServer, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return;
                    }
                    finally
                    {
                        dialogService.CloseStatusDialog();
                    }

                    if (giftCard != null)
                    {
                        /*
                        // When using "ID and amount" option, you should only be creating new gift cards, not activating existing ones
                        if (!giftCard.Active)
                        {
                            Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.GiftCardIDAlreadyExists, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                            return;
                        }*/
                        if (IsChangeBack && !giftCard.Refillable)
                        {
                            dialogService.ShowMessage(Resources.GiftcardExistsNotRefillable, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                            return;
                        }
                        if (!giftCard.Active)
                        {
                            RefillingGiftcard = false;
                            ExistingGiftCard = giftCard;
                            break;
                        }
                        if (!giftCard.Refillable)
                        {
                            dialogService.ShowMessage(Resources.GiftCardNotRefillable, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                            return;
                        }
                        else
                        {
                            string balanceDisplay = Interfaces.Services.RoundingService(dlgEntry).RoundForDisplay(dlgEntry, giftCard.Balance, true, false, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                            string amountToAddDisplay = Interfaces.Services.RoundingService(dlgEntry).RoundForDisplay(dlgEntry, Amount, true, false, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                            decimal newBalance = Amount + giftCard.Balance;
                            string newBalanceDisplay = Interfaces.Services.RoundingService(dlgEntry).RoundForDisplay(dlgEntry, newBalance, true, false, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                            if (newBalance > dlgSettings.SiteServiceProfile.MaximumGiftCardAmount && dlgSettings.SiteServiceProfile.MaximumGiftCardAmount != 0)
                            {
                                string roundedMaximum = Interfaces.Services.RoundingService(dlgEntry).RoundForDisplay(dlgEntry,
                                                                                                             dlgSettings.SiteServiceProfile.MaximumGiftCardAmount,
                                                                                                             true,
                                                                                                             false,
                                                                                                             dlgSettings.Store.Currency,
                                                                                                             CacheType.CacheTypeApplicationLifeTime);
                                dialogService.ShowMessage(Resources.CurrentAmountExceedsMaximum.Replace("#1", roundedMaximum), MessageBoxButtons.OK, MessageDialogType.Generic);
                                ntbAmount.Focus();
                                return;
                            }
                            string message =
                                Resources.CurrentBalance + " " + balanceDisplay + "\r\n" +
                                Resources.AmountToAdd + " " + amountToAddDisplay + " \r\n \r\n" +
                                Resources.FinalBalance + " " + newBalanceDisplay + "\r\n\r\n" +
                                Resources.AddAmountQuestion;

                            if (dialogService.ShowMessage(message, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
                            {
                                RefillingGiftcard = true;
                                ExistingGiftCard = giftCard;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }

                    break;                
            }

            // Check Gift Card is duplicate
            if (POSTransaction != null && ((RetailTransaction)POSTransaction).SaleItems.Any(x => x.Transaction.SaleItems.Any(y => y.Comment == GiftCardId && y.Description == Properties.Resources.GiftCard)))
            {
                if (dlgSettings.SiteServiceProfile.IssueGiftCardOption == SiteServiceProfile.IssueGiftCardOptionEnum.ID)
                {
                    dialogService.ShowMessage(Resources.GiftCardDuplicateIdForId, MessageBoxButtons.OK, MessageDialogType.Generic);
                }
                else if (dlgSettings.SiteServiceProfile.IssueGiftCardOption == SiteServiceProfile.IssueGiftCardOptionEnum.IDAndAmount)
                {
                    dialogService.ShowMessage(Resources.GiftCardDuplicateIdForIdAmount, MessageBoxButtons.OK, MessageDialogType.Generic);
                }

                txtGiftCardID.Focus();
                
                return;
            }

            this.DialogResult = DialogResult.OK;            
        }

        private void touchKeyboard_EnterPressed(object sender, EventArgs e)
        {
            switch (giftCardOptions)
            {
                case SiteServiceProfile.IssueGiftCardOptionEnum.Amount:
                    if (ActiveControl == ntbAmount)
                    {
                        btnOk_Click(this, EventArgs.Empty);
                    }
                    break;

                case SiteServiceProfile.IssueGiftCardOptionEnum.ID:
                    if (ActiveControl == txtGiftCardID)
                    {
                        btnOk_Click(this, EventArgs.Empty);
                    }
                    break;

                case SiteServiceProfile.IssueGiftCardOptionEnum.IDAndAmount:
                    if (ActiveControl == txtGiftCardID)
                    {
                        ntbAmount.Focus();
                    }
                    else if (ActiveControl == ntbAmount)
                    {
                        btnOk_Click(this, EventArgs.Empty);
                    }
                    break;
            }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            switch (giftCardOptions)
            {
                case SiteServiceProfile.IssueGiftCardOptionEnum.Amount:
                    btnOk.Enabled = ntbAmount.Value > 0;
                    break;

                case SiteServiceProfile.IssueGiftCardOptionEnum.ID:
                    btnOk.Enabled = txtGiftCardID.Text.Length > 0;
                    break;

                case SiteServiceProfile.IssueGiftCardOptionEnum.IDAndAmount:
                    btnOk.Enabled = ntbAmount.Value > 0 && txtGiftCardID.Text.Length > 0;
                    break;
            }
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

        private void touchScrollButtonPanel_Click(object sender, ScrollButtonEventArguments args)
        {
            ntbAmount.Value = (double)((CashDenominator)args.Tag).Amount;
        }

        private void txtGiftCardID_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                txtGiftCardID.Text = StringExtensions.TrackBeforeSeparator(txtGiftCardID.Text, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);
            }
        }
    }
}