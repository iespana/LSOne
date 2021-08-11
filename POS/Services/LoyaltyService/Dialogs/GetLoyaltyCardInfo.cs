using LSOne.Controls;
using LSOne.Controls.Dialogs;
using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

namespace LSOne.Services.Dialogs
{
    public enum BannerButtons
    {
        None = 0,
        Points = 1,
        Amounts = 2,
        DisplayAmounts = 3,
        DisplayPoints = 4
    }

    public partial class GetLoyaltyCardInfo : TouchBaseForm
    {
        public LoyaltyPointStatus PointStatus { get; set; }
        public bool LoyaltyInfoRetrieved { get; set; }
        
        private CardInfo cardInfo;
        private IRetailTransaction retailTransaction;
        private RecordIdentifier lastCheckedCardNumber;
        private CalculationHelper calculationHelper;
        private StorePaymentMethod tenderInformation;
        private decimal balanceAmount;
        private decimal orgBalanceAmount;

        private UseDialogEnum useDlg;
        private decimal retrievePointsDue;
        private LoyaltyItem orgLoyaltyItem;
        private Customer orgCustomer;

        private decimal usePointsLimit;

        private BannerButtons currentlyDisplayed;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        public decimal PaidAmount
        {
            get
            {
                if (useDlg == UseDialogEnum.PointsDiscount || useDlg == UseDialogEnum.PointsPayment)
                {
                    return Conversion.ToDecimal(ntbAmount.Value);
                }
                else
                {
                    return decimal.Zero;
                }
            }
        }

        public GetLoyaltyCardInfo(IConnectionManager entry,  CardInfo cardInfo, decimal balanceAmount, RecordIdentifier tenderTypeID, 
            bool displayOKCancel, UseDialogEnum dialogUsage, IRetailTransaction retailTransaction)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            PointStatus = new LoyaltyPointStatus();
            calculationHelper = new CalculationHelper(retailTransaction, dialogUsage);
            currentlyDisplayed = BannerButtons.DisplayAmounts;

            orgLoyaltyItem = (LoyaltyItem)retailTransaction.LoyaltyItem.Clone();
            orgCustomer = retailTransaction.Customer;
            
            this.cardInfo = cardInfo;
            this.retailTransaction = retailTransaction;
            this.balanceAmount = dialogUsage == UseDialogEnum.PointsDiscount ? calculationHelper.GetTotalAmtUsedForDiscount(retailTransaction, true) : balanceAmount;
            this.orgBalanceAmount = balanceAmount;
            this.retrievePointsDue = decimal.Zero;
            this.usePointsLimit = decimal.Zero;

            lastCheckedCardNumber = RecordIdentifier.Empty;
            LoyaltyInfoRetrieved = false;

            this.useDlg = dialogUsage;

            SetExistingLoyaltyInfo();

            if (useDlg == UseDialogEnum.PointsDiscount || useDlg == UseDialogEnum.PointsPayment)
            {
                tenderInformation = Providers.StorePaymentMethodData.Get(dlgEntry, new RecordIdentifier(dlgSettings.Store.ID,tenderTypeID));

                Currency storeCurrency = Providers.CurrencyData.Get(dlgEntry, dlgSettings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
                DecimalLimit limiter = new DecimalLimit(DecimalExtensions.DigitsBeforeFirstSignificantDigit(storeCurrency.RoundOffSales));
                ntbAmount.SetValueWithLimit(0.0M, limiter);
                
                ntbAmount.MaxValue = (int)dlgSettings.FunctionalityProfile.MaximumPrice;
                
                ntbAmount.MinValue = tenderInformation.AllowNegativePaymentAmounts ? (double)dlgSettings.FunctionalityProfile.MaximumPrice*-1 : 0;
                ntbAmount.AllowNegative = tenderInformation.AllowNegativePaymentAmounts;

                if (retailTransaction.Customer.ID != RecordIdentifier.Empty)
                {
                    tbLoyaltyCardNumber.Enabled = !retailTransaction.Customer.ReturnCustomer;
                    btnGet.Enabled = !retailTransaction.Customer.ReturnCustomer;
                }

                if (useDlg == UseDialogEnum.PointsDiscount && retailTransaction.LoyaltyItem.CalculatedPointsAmount != decimal.Zero)
                {
                    decimal toDisplay = OverUsePointsLimit();
                    if (toDisplay > decimal.Zero)
                    {
                        ntbAmount.Value = (double)toDisplay;
                    }
                    else
                    {
                        ntbAmount.Value = (double)retailTransaction.LoyaltyItem.CalculatedPointsAmount;
                    }
                }

                DisplayPointsDueInHeader(retrievePointsDue);
                DisplayPointInformation();

                SetButtonPanel(currentlyDisplayed);

                if (!LoyaltyInfoRetrieved && PointStatus.ResultCode == LoyaltyCustomer.ErrorCodes.NoConnectionTried && PointStatus.Points != decimal.Zero && PointStatus.CardNumber != "")
                {
                    LoyaltyInfoRetrieved = true;
                }

                if(cardInfo.CardNumber != "")
                {
                    tbLoyaltyCardNumber.Text = cardInfo.CardNumber;
                    btnGet_Click(this, EventArgs.Empty);
                }
            }
            else
            {
                touchDialogBanner.BannerText = Resources.LoyaltyCardInformation;
            }

            if (!DesignMode)
            {
                tbLoyaltyCardNumber.StartCharacter = dlgSettings.HardwareProfile.StartTrack1;
                tbLoyaltyCardNumber.EndCharacter = dlgSettings.HardwareProfile.EndTrack1;
                tbLoyaltyCardNumber.Seperator = dlgSettings.HardwareProfile.Separator1;
                tbLoyaltyCardNumber.TrackSeperation = TrackSeperation.Before;
                tbLoyaltyCardNumber.MaxLength = 30;
            }

            btnOk.Visible = displayOKCancel;
            btnClearDiscount.Visible = useDlg == UseDialogEnum.PointsDiscount;
            btnClearDiscount.Enabled = retailTransaction.LoyaltyItem.Relation == LoyaltyPointsRelation.Discount;

            if (!displayOKCancel)
            {
                btnCancel.Text = GetClose();
            }
        }

        private static string GetClose()
        {
            return Resources.Close;
        }

        private bool TenderAmountAllowed(decimal amount)
        {
            IRoundingService rounding = (IRoundingService)dlgEntry.Service(ServiceType.RoundingService);
            Currency storeCurrency = Providers.CurrencyData.Get(dlgEntry, dlgSettings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);

            if (tenderInformation.MaximumAmountAllowed > decimal.Zero && Math.Abs(amount) > tenderInformation.MaximumAmountAllowed)
            {
                if (useDlg == UseDialogEnum.PointsDiscount)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.MaximumDiscountAllowed.Replace("#1", rounding.RoundString(dlgEntry, tenderInformation.MaximumAmountAllowed, storeCurrency.ID, true, CacheType.CacheTypeTransactionLifeTime)), MessageBoxButtons.OK, MessageDialogType.Generic);
                }
                else
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.MaximumPaymentAllowed.Replace("#1", rounding.RoundString(dlgEntry, tenderInformation.MaximumAmountAllowed, storeCurrency.ID, true, CacheType.CacheTypeTransactionLifeTime)), MessageBoxButtons.OK, MessageDialogType.Generic);
                }
                return false;
            }

            if (tenderInformation.MinimumAmountAllowed > decimal.Zero && Math.Abs(amount) < tenderInformation.MinimumAmountAllowed)
            {
                if (useDlg == UseDialogEnum.PointsDiscount)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.MinimumDiscountAllowed.Replace("#1", rounding.RoundString(dlgEntry, tenderInformation.MinimumAmountAllowed, storeCurrency.ID, true, CacheType.CacheTypeTransactionLifeTime)), MessageBoxButtons.OK, MessageDialogType.Generic);
                }
                else
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.MinimumPaymentAllowed.Replace("#1", rounding.RoundString(dlgEntry, tenderInformation.MinimumAmountAllowed, storeCurrency.ID, true, CacheType.CacheTypeTransactionLifeTime)), MessageBoxButtons.OK, MessageDialogType.Generic);
                }
                return false;
            }

            if (tenderInformation.AllowOverTender)
            {
                if (tenderInformation.MaximumOverTenderAmount > decimal.Zero && Math.Abs(amount) > (tenderInformation.MaximumOverTenderAmount + Math.Abs(balanceAmount)))
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.MaximumPaymentAllowed.Replace("#1", rounding.RoundString(dlgEntry, tenderInformation.MaximumOverTenderAmount + Math.Abs(balanceAmount), storeCurrency.ID, true, CacheType.CacheTypeTransactionLifeTime)), MessageBoxButtons.OK, MessageDialogType.Generic);
                    return false;
                }
            }
            else
            {
                if (tenderInformation.UnderTenderAmount > decimal.Zero && Math.Abs(amount) > (Math.Abs(balanceAmount) - tenderInformation.UnderTenderAmount))
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.MinimumPaymentAllowed.Replace("#1", rounding.RoundString(dlgEntry, Math.Abs(balanceAmount) - tenderInformation.UnderTenderAmount, storeCurrency.ID, true, CacheType.CacheTypeTransactionLifeTime)), MessageBoxButtons.OK, MessageDialogType.Generic);
                    return false;
                }
            }
            
            decimal maxPayment = OverUsePointsLimit();
            if (maxPayment > decimal.Zero)
            {
                string msg = useDlg == UseDialogEnum.PointsDiscount ? Resources.DiscountOverUsePointsLimit : Resources.PaymentOverUsePointsLimit;
                Interfaces.Services.DialogService(dlgEntry)
                        .ShowMessage(msg.Replace("#1", Interfaces.Services.RoundingService(dlgEntry).RoundString(dlgEntry, usePointsLimit, 0, false, "")).
                                         Replace("#2", Interfaces.Services.RoundingService(dlgEntry).RoundString(dlgEntry, maxPayment, storeCurrency.ID, true, CacheType.CacheTypeTransactionLifeTime)));
                ntbAmount.Value = (double)Interfaces.Services.RoundingService(dlgEntry).Round(dlgEntry, maxPayment, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);

                return true;
            }

            return true;
        }
        
        private decimal OverUsePointsLimit(bool alwaysReturnMaxPayment)
        {
            decimal maxPayment = (Math.Abs(retailTransaction.LoyaltyItem.CalculatedPointsAmount) + 
                                            (useDlg == UseDialogEnum.PointsPayment ? Math.Abs(retailTransaction.NetAmountWithTax) : 
                                                dlgSettings.Store.CalculateDiscountsFrom == Store.CalculateDiscountsFromEnum.PriceWithTax ? Math.Abs(retailTransaction.NetAmountWithTax) : Math.Abs(retailTransaction.NetAmount)))
                                * (usePointsLimit / 100);

            if (useDlg == UseDialogEnum.PointsDiscount)
            {
                maxPayment = calculationHelper.GetTotalAmtUsedForDiscount(retailTransaction, true) * usePointsLimit / 100;
            }

            if(maxPayment > PointStatus.CurrentValue)
            {
                maxPayment = PointStatus.CurrentValue;
            }

            maxPayment = Interfaces.Services.RoundingService(dlgEntry).Round(dlgEntry, maxPayment, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);

            decimal previousPaidAmt = decimal.Zero;
            if (useDlg == UseDialogEnum.PointsPayment)
            {
                previousPaidAmt = retailTransaction.TenderLines.Where(w => w is LoyaltyTenderLineItem && w.Voided == false).Sum(s => s.Amount);
                maxPayment = Math.Abs(retailTransaction.TransSalePmtDiff) < Math.Abs(maxPayment) ? Math.Abs(retailTransaction.TransSalePmtDiff) : Math.Abs(maxPayment);
            }
            
            if (usePointsLimit > decimal.Zero && ntbAmount.Text != "" && (Math.Abs(PaidAmount + previousPaidAmt) > Math.Abs(maxPayment)))
            {
                if (useDlg == UseDialogEnum.PointsPayment)
                {
                    return previousPaidAmt >= maxPayment ? decimal.Zero : maxPayment - previousPaidAmt;
                }
                return maxPayment;
            }
            
            return alwaysReturnMaxPayment ? maxPayment : decimal.Zero;
        }

        private decimal OverUsePointsLimit()
        {
            return OverUsePointsLimit(false);
        }

        private void SetExistingLoyaltyInfo()
        {
            if (!retailTransaction.LoyaltyItem.Empty)
            {
                PointStatus.CardNumber = retailTransaction.LoyaltyItem.CardNumber;
                PointStatus.Points = retailTransaction.LoyaltyItem.AccumulatedPoints;
                PointStatus.CurrentValue = retailTransaction.LoyaltyItem.CurrentValue;
            }

            if (PointStatus.CardNumber != RecordIdentifier.Empty)
            {
                tbLoyaltyCardNumber.Text = (string)PointStatus.CardNumber;
                GetLoyaltyInformation();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ClearLoyaltyInformation();

            if (btnOk.Visible)
            {
                btnOk.DialogResult = DialogResult.OK;
                CheckEnabled();
            }

            SetControlLocations();

            if (!DesignMode)
            {
                Scanner.ScannerMessageEvent += ScannerOnScannerMessageEvent;
                Scanner.ReEnableForScan();
                MSR.MSRMessageEvent += MSR_MSRMessageEvent;
                MSR.EnableForSwipe();

                RefreshLoyaltyStatus();
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

        private void ScannerOnScannerMessageEvent(ScanInfo scanInfo)
        {
            try
            {
                tbLoyaltyCardNumber.Text = scanInfo.ScanDataLabel;

                if (btnGet.Enabled)
                {
                    btnGet_Click(this, EventArgs.Empty);
                }
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        void MSR_MSRMessageEvent(string track2)
        {
            tbLoyaltyCardNumber.Text = StringExtensions.TrackBeforeSeparator(track2, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);

            if (btnGet.Enabled)
            {
                btnGet_Click(this, EventArgs.Empty);
            }
        }

        private void SetControlLocations()
        {
            if (!(useDlg == UseDialogEnum.PointsPayment || useDlg == UseDialogEnum.PointsDiscount))
            {
                buttonPanel.Visible = false;
                buttonPanel.TabStop = false;
                lblAmount.Visible = false;
                lblPoints.Visible = false;
                ntbAmount.Visible = false;
                ntbPoints.Visible = false;
                lblAmount.TabStop = false;
                lblPoints.TabStop = false;
                ntbAmount.TabStop = false;
                ntbPoints.TabStop = false;

                this.Size = new Size(this.Size.Width, this.Size.Height - buttonPanel.Height - 10);
            }
        }

        private void DisplayPointsDueInHeader(decimal pointsDue)
        {
            if (useDlg == UseDialogEnum.PointsPayment || useDlg == UseDialogEnum.PointsDiscount)
            {
                if (pointsDue == decimal.Zero)
                {
                    pointsDue = GetPointsDue(balanceAmount);
                }

                if (pointsDue != decimal.Zero)
                {
                    Currency storeCurrency = Providers.CurrencyData.Get(dlgEntry, dlgSettings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);

                    if (useDlg == UseDialogEnum.PointsPayment)
                    {
                        touchDialogBanner.BannerText = tenderInformation.Text;
                        touchDialogBanner.BannerText += " - ";
                        if (balanceAmount > decimal.Zero)
                        {
                            touchDialogBanner.BannerText += Resources.PointsDue.Replace("#1",
                                Interfaces.Services.RoundingService(dlgEntry).RoundAmount(dlgEntry, pointsDue, dlgSettings.Store.ID, tenderInformation.PaymentTypeID, false, storeCurrency.ID, CacheType.CacheTypeApplicationLifeTime));
                        }
                        else
                        {
                            touchDialogBanner.BannerText += Resources.PointsToBeReturned.Replace("#1",
                                Interfaces.Services.RoundingService(dlgEntry).RoundAmount(dlgEntry, pointsDue, dlgSettings.Store.ID, tenderInformation.PaymentTypeID, false, storeCurrency.ID, CacheType.CacheTypeApplicationLifeTime));
                        }
                    }
                    else
                    {
                        touchDialogBanner.BannerText = Resources.LoyaltyDiscount;
                    }
                }
                else
                {
                    touchDialogBanner.BannerText = useDlg == UseDialogEnum.PointsPayment ? Resources.SelectLoyaltyCardForPayment : Resources.SelectLoyaltyCardForDiscount;
                }
            }
        }

        private decimal GetPointsDue(decimal amount)
        {
            if (useDlg == UseDialogEnum.PointsPayment || useDlg == UseDialogEnum.PointsDiscount)
            {
                decimal pointsDue = decimal.Zero;
                calculationHelper.GetPointsForAllLoyaltyTenderLines(dlgEntry, ref pointsDue, tenderInformation.PaymentTypeID, amount);

                // Before displaying the points we need to make sure that they are a positive number
                return pointsDue < decimal.Zero ? pointsDue * -1 : pointsDue;
            }

            return decimal.Zero;
        }

        private void CardAndPointInformation()
        {
            PointStatus.CardNumber = tbLoyaltyCardNumber.Text.Trim() == "" ? RecordIdentifier.Empty : tbLoyaltyCardNumber.Text.Trim();
            cardInfo.CardEntryType = tbLoyaltyCardNumber.ManualEntryOfTrack ? CardEntryTypesEnum.ManuallyEntered : CardEntryTypesEnum.MagneticStripeRead;
            cardInfo.CardNumber = (string)PointStatus.CardNumber;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (PointStatus.CardNumber == RecordIdentifier.Empty)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.NoLoyaltyCardHasBeenEntered);
                tbLoyaltyCardNumber.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            RefreshLoyaltyStatus();
        }

        private void RefreshLoyaltyStatus()
        {
            if (retailTransaction.Customer.ReturnCustomer)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CustomerCannotBeClearedTransactionIsAReturn);
                return;
            }
            GetAndDisplayLoyaltyInformation();
            SetButtonPanel(currentlyDisplayed);
        }

        private void GetAndDisplayLoyaltyInformation()
        {
            CardAndPointInformation();

            // If no card number is entered
            if (PointStatus.CardNumber == RecordIdentifier.Empty)
            {
                return;
            }

            if (PointStatus.CardNumber == lastCheckedCardNumber)
            {
                return;
            }

            lastCheckedCardNumber = PointStatus.CardNumber;

            RecalculateBalance();

            LoyaltyInfoRetrieved = GetPointStatus();
            GetLoyaltyInformation();
            
            DisplayPointsDueInHeader(GetPointsDue(balanceAmount));
            DisplayPointInformation();
            CheckEnabled();
        }

        private void RecalculateBalance()
        {
            if (!(useDlg == UseDialogEnum.PointsPayment || useDlg == UseDialogEnum.PointsDiscount))
            {
                return;
            }

            LoyaltyMSRCard card = Providers.LoyaltyMSRCardData.Get(dlgEntry, PointStatus.CardNumber);

            if (card != null && card.LinkID != "" && lblCustomer.Text != card.CustomerName)
            {
                retailTransaction.LoyaltyItem.CustomerID = card.LinkID;
                calculationHelper.AddCustomerToTransaction(dlgEntry, retailTransaction);
                
                balanceAmount = (useDlg == UseDialogEnum.PointsDiscount) ? calculationHelper.GetTotalAmtUsedForDiscount(retailTransaction, true) : retailTransaction.TransSalePmtDiff;
            }
        }

        private void GetLoyaltyInformation()
        {
            retailTransaction.LoyaltyItem.CardNumber = tbLoyaltyCardNumber.Text.Trim();
            retailTransaction.LoyaltyItem.AccumulatedPoints = PointStatus.Points;

            if (PointStatus.Points > 0 && PointStatus.CurrentValue == 0)
            {
                LoyaltyInfoRetrieved = GetPointStatus();
            }

            retailTransaction.LoyaltyItem.CurrentValue = PointStatus.CurrentValue;

            LoyaltyMSRCard card = calculationHelper.GetLoyaltyInfo(dlgEntry, retailTransaction.LoyaltyItem);

            if (card == null)
            {
                ClearLoyaltyInformation();
                return;
            }

            if (string.IsNullOrEmpty(retailTransaction.LoyaltyItem.CustomerID.StringValue))
            {
                retailTransaction.LoyaltyItem.CustomerID = string.IsNullOrEmpty(card.CustomerID.StringValue) ? PointStatus.CustomerID : card.CustomerID;
                card.CustomerID = string.IsNullOrEmpty(card.CustomerID.StringValue) ? PointStatus.CustomerID : card.CustomerID;
            }

            Customer customer = new Customer();
            if (card.CustomerID != null)
            {
                customer = Providers.CustomerData.Get(dlgEntry, card.CustomerID, UsageIntentEnum.Normal);
                if (customer != null && customer.Blocked != BlockedEnum.Nothing)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CusotmerIsBlockedAndCannotUsePoints);
                    ClearLoyaltyInformation();
                    return;
                }
            }

            IRoundingService rounding = (IRoundingService)dlgEntry.Service(ServiceType.RoundingService);

            lblCustomer.Text = customer != null ? customer.GetFormattedName(dlgEntry.Settings.NameFormatter) : card.CustomerName;

            lblCardScheme.Text = retailTransaction.LoyaltyItem.SchemeExists ? card.SchemeDescription : Resources.SchemeNotFound;

            lblPointUseLimit.Text = card.UsePointsLimit == decimal.Zero ? Resources.None : rounding.RoundString(dlgEntry, card.UsePointsLimit, 0, false, "");
            usePointsLimit = card.UsePointsLimit;

            if (LoyaltyInfoRetrieved || PointStatus.Points > decimal.Zero)
            {
                Currency storeCurrency = Providers.CurrencyData.Get(dlgEntry, dlgSettings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
                lblPointBalance.Text = rounding.RoundString(dlgEntry, PointStatus.Points, 0, false, "");
                lblCurrentValue.Text = $"{storeCurrency.Symbol} {rounding.RoundString(dlgEntry, PointStatus.CurrentValue,  storeCurrency.ID, false, CacheType.CacheTypeTransactionLifeTime)}";
            }
            else
            {
                lblPointBalance.Text = Resources.PointBalanceNotAvailable;
                lblCurrentValue.Text = Resources.PointBalanceNotAvailable;
            }
        }

        private void ClearLoyaltyInformation()
        {
            retailTransaction.LoyaltyItem.Clear();
            lblCustomer.Text = "-";
            lblCardScheme.Text = "-";
            lblPointBalance.Text = "-";
            lblPointUseLimit.Text = "-";
            lblCurrentValue.Text = "-";
        }

        private bool GetPointStatus()
        {
            try
            {                
                void GetPointStatusHelper()
                {
                    PointStatus.Comment = "";

                    ISiteServiceService service = (ISiteServiceService)dlgEntry.Service(ServiceType.SiteServiceService);
                    PointStatus = service.GetLoyaltyPointsStatus(dlgEntry, dlgSettings.SiteServiceProfile, PointStatus);
                }

                btnOk.Enabled = true;
                Exception ex;
                Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(new Action(GetPointStatusHelper), Resources.RetrievingLoyaltyStatus, Resources.RetrievingLoyaltyStatus, out ex);

                if(ex != null)
                {
                    throw ex;
                }                
               
            }
            catch (Exception e)
            {
                PointStatus.ResultCode = e is EndpointNotFoundException ? LoyaltyCustomer.ErrorCodes.CouldNotConnectToSiteService : LoyaltyCustomer.ErrorCodes.UnknownError;
            }
            
            if (PointStatus.ResultCode != LoyaltyCustomer.ErrorCodes.NoErrors)
            {
                PointStatus.Comment = (PointStatus.Comment == "") ? Resources.ErrorConnectingToLoyaltyServer : PointStatus.Comment;
                btnOk.Enabled = PointStatus.ResultCode != LoyaltyCustomer.ErrorCodes.CouldNotConnectToSiteService;
                lastCheckedCardNumber = RecordIdentifier.Empty;

                return false;
            }

            return true;
        }

        private void tbLoyaltyCardNumber_TextChanged(object sender, EventArgs e)
        {
            LoyaltyInfoRetrieved = false;
        }

        private void tbLoyaltyCardNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbLoyaltyCardNumber.Text = StringExtensions.TrackBeforeSeparator(tbLoyaltyCardNumber.Text, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);

                if (btnGet.Enabled)
                {
                    btnGet_Click(sender, e);
                }
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

        private void SetButtonPanel(BannerButtons displayButtons)
        {
            RecordIdentifier normalTender = Providers.StorePaymentMethodData.GetCashTender(dlgEntry, dlgSettings.Store.ID);
            StorePaymentMethod cashTender = Providers.StorePaymentMethodData.Get(dlgEntry, new RecordIdentifier(dlgSettings.Store.ID, normalTender));

            currentlyDisplayed = displayButtons;

            decimal maxAmountAllowed = OverUsePointsLimit(true);

            if (displayButtons == BannerButtons.DisplayAmounts && tenderInformation != null)
            {
                buttonPanel.SetButtonsCurrency(maxAmountAllowed, cashTender, dlgSettings.Store.Currency, false, TouchScrollButtonsCurrencyHelper.ViewOptions.LowerAmounts, tenderInformation.MinimumAmountAllowed, tenderInformation.MaximumAmountAllowed);
                
                // Add a Points button at the end
                buttonPanel.AddButton(Resources.Points, BannerButtons.DisplayPoints, "", TouchButtonType.Action, DockEnum.DockEnd);
            }
            else if (displayButtons == BannerButtons.DisplayPoints && tenderInformation != null)
            {
                buttonPanel.SetButtonsLoyaltyPoints(maxAmountAllowed, calculationHelper.TenderPointsMultiplier, tenderInformation, dlgSettings.Store.Currency, 1M, (int)calculationHelper.GetRoundMethod(), false, TouchScrollButtonsCurrencyHelper.ViewOptions.LowerAmounts, tenderInformation.MinimumAmountAllowed, tenderInformation.MaximumAmountAllowed);

                // Add an Amounts button at the end
                buttonPanel.AddButton(Resources.Amounts, BannerButtons.DisplayAmounts, "", TouchButtonType.Action, DockEnum.DockEnd);
            }
        }

        private void touchScrollButtonPanel_Click(object sender, ScrollButtonEventArguments args)
        {
            if (args.Tag is BannerButtons && (BannerButtons)args.Tag == BannerButtons.DisplayPoints)
            {
                //Display points buttons
                SetButtonPanel(BannerButtons.DisplayPoints);
            }
            else if (args.Tag is BannerButtons && (BannerButtons) args.Tag == BannerButtons.DisplayAmounts)
            {
                // Display Amounts
                SetButtonPanel(BannerButtons.DisplayAmounts);
            }
            else
            {
                // Retrieve amount as payment
                ntbAmount.Value = (double)Conversion.ToDecimal(args.Tag);
                DisplayPointInformation();
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

        private void ntbAmount_Leave(object sender, EventArgs e)
        {
            if (ntbAmount.Text != "")
            {
                GetPointsDue((decimal) ntbAmount.Value);
            }
            DisplayPointInformation();
        }

        private void DisplayPointInformation()
        {
            errorProvider.Clear();
            errorProvider.Visible = false;
            ntbPoints.Value = 0;

            if (ntbAmount.Text == "")
            {
                if (balanceAmount == decimal.Zero)
                {
                    errorProvider.ErrorText = Resources.NoItemToDiscountDlg;
                    errorProvider.Visible = true;
                }
                else if (string.IsNullOrEmpty(tbLoyaltyCardNumber.Text))
                {
                    errorProvider.Clear();
                    errorProvider.Visible = false;
                    return;
                }
            }

            IRoundingService rounding = (IRoundingService)dlgEntry.Service(ServiceType.RoundingService);
            Currency storeCurrency = Providers.CurrencyData.Get(dlgEntry, dlgSettings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
            string msg = "";

            if (calculationHelper.TenderPointsMultiplier == decimal.Zero || (PointStatus.ResultCode != LoyaltyCustomer.ErrorCodes.NoErrors && PointStatus.ResultCode != LoyaltyCustomer.ErrorCodes.NoConnectionTried))
            {
                switch ( PointStatus.ResultCode)
                {
                    case LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardNotFound:
                        msg = Resources.ErrLoyaltyCardNotFound;
                        break;
                    case LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardBlocked:
                        msg = Resources.ErrLoyaltyCardBlocked;
                        break;
                    case LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardIsNoTenderCard:
                        msg = Resources.ErrLoyaltyCardIsNoTenderCard;
                        break;
                    case LoyaltyCustomer.ErrorCodes.ErrLoyaltyIsNotActivated:
                        msg = Resources.ErrLoyaltyIsNotActivated;
                        break;
                    default:
                        msg = Resources.NoLoyaltyInformationAvailable;
                        break;
                }

                if (!calculationHelper.TenderRuleFound && (decimal) ntbAmount.Value < calculationHelper.MinimumPaymentForTender)
                {
                    if (useDlg == UseDialogEnum.PointsDiscount)
                    {
                        msg = Resources.SchemeMinDiscountIs.Replace("#1",
                                        Interfaces.Services.RoundingService(dlgEntry).RoundAmount(dlgEntry, calculationHelper.MinimumPaymentForTender, dlgSettings.Store.ID, tenderInformation.PaymentTypeID, false, storeCurrency.ID, CacheType.CacheTypeApplicationLifeTime));  
                    }
                    else
                    {
                        msg = Resources.SchemeMinPaymentIs.Replace("#1",
                                        Interfaces.Services.RoundingService(dlgEntry).RoundAmount(dlgEntry, calculationHelper.MinimumPaymentForTender, dlgSettings.Store.ID, tenderInformation.PaymentTypeID, false, storeCurrency.ID, CacheType.CacheTypeApplicationLifeTime));  
                    }
                }
                else if (!calculationHelper.TenderRuleFound && calculationHelper.TenderPointsMultiplier == decimal.Zero && (PointStatus.ResultCode == LoyaltyCustomer.ErrorCodes.NoErrors || PointStatus.ResultCode == LoyaltyCustomer.ErrorCodes.NoConnectionTried)) 
                {
                    switch (useDlg)
                    {
                        case UseDialogEnum.GetInformation:
                            return;
                        case UseDialogEnum.PointsDiscount:
                            msg = Resources.NoTenderRuleForLoyaltyDiscountExists;
                            break;
                        default:
                            msg = Resources.NoTenderRuleForLoyaltyPaymentExists;
                            break;
                    }
                }

                errorProvider.Visible = true;

                if (msg != "")
                {
                    errorProvider.ErrorText = msg;
                    return;
                }

                errorProvider.ErrorText = Resources.ValueOfPoints.Replace("#1", "0") + " - " + Resources.NoLoyaltyInformationAvailable; 
                return;
            }

            calculationHelper.GetPointsForTender(dlgEntry, tenderInformation.PaymentTypeID, (decimal) ntbAmount.Value, null);

            decimal points = ((decimal)ntbAmount.Value * calculationHelper.TenderPointsMultiplier);
            if (!calculationHelper.TenderRuleFound && (decimal)ntbAmount.Value < calculationHelper.MinimumPaymentForTender && ntbAmount.Value != 0)
            {
                if (useDlg == UseDialogEnum.PointsDiscount)
                {
                    msg = Resources.SchemeMinDiscountIs.Replace("#1", Interfaces.Services.RoundingService(dlgEntry).RoundAmount(dlgEntry, calculationHelper.MinimumPaymentForTender, dlgSettings.Store.ID, tenderInformation.PaymentTypeID, true, storeCurrency.ID, CacheType.CacheTypeApplicationLifeTime));
                }
                else
                {
                    msg = Resources.SchemeMinPaymentIs.Replace("#1", Interfaces.Services.RoundingService(dlgEntry).RoundAmount(dlgEntry, calculationHelper.MinimumPaymentForTender, dlgSettings.Store.ID, tenderInformation.PaymentTypeID, true, storeCurrency.ID, CacheType.CacheTypeApplicationLifeTime));
                }
            }
            else if (!calculationHelper.TenderRuleFound && calculationHelper.TenderPointsMultiplier == decimal.Zero && PointStatus.ResultCode == LoyaltyCustomer.ErrorCodes.NoErrors)
            {
                if (useDlg == UseDialogEnum.PointsDiscount)
                {
                    msg = Resources.NoTenderRuleForLoyaltyDiscountExists;
                }
                else
                {
                    msg = Resources.NoTenderRuleForLoyaltyPaymentExists;
                }
            }

            if (msg != "")
            {
                errorProvider.ErrorText = msg;
                errorProvider.Visible = true;
                CheckEnabled();
                return;
            }

            decimal previousPaidPoints = retailTransaction.TenderLines.Where(w => w is LoyaltyTenderLineItem && w.Voided == false).Sum(s => ((LoyaltyTenderLineItem)s).Points);
            if (!retailTransaction.Customer.ReturnCustomer && retailTransaction.TransSalePmtDiff > decimal.Zero && ((points + (previousPaidPoints*-1)) > PointStatus.Points))
            {
                errorProvider.ErrorText = Resources.NotEnoughPoints;
                errorProvider.Visible = true;
                btnOk.Enabled = false;
            }
            else
            {
                errorProvider.Clear();
                errorProvider.Visible = false;
                ntbPoints.Value = (double)Interfaces.Services.RoundingService(dlgEntry).RoundAmount(dlgEntry, points, 1M, calculationHelper.GetRoundMethod());

                decimal orgPointsAmount = (decimal)ntbAmount.Value;
                CheckEnabled();
                if (orgPointsAmount != (decimal)ntbAmount.Value)
                {
                    points = (decimal)ntbAmount.Value * calculationHelper.TenderPointsMultiplier;
                    errorProvider.ErrorText = Resources.ValueOfPoints.Replace("#1",
                                              Interfaces.Services.RoundingService(dlgEntry).RoundAmount(dlgEntry, points, 1M, calculationHelper.GetRoundMethod(), false, storeCurrency.ID, CacheType.CacheTypeApplicationLifeTime));
                }
            }
        }

        private void CheckEnabled()
        {
            if (useDlg == UseDialogEnum.PointsPayment || useDlg == UseDialogEnum.PointsDiscount)
            {
                btnOk.Enabled = TenderAmountAllowed((decimal)ntbAmount.Value) &&
                                LoyaltyInfoRetrieved &&
                                PointStatus.Points != decimal.Zero &&
                                calculationHelper.TenderRuleFound &&
                                retailTransaction.LoyaltyItem.SchemeExists &&
                                ntbAmount.Text != "" && calculationHelper.TenderRuleFound && (decimal) Math.Abs(ntbAmount.Value) >= calculationHelper.MinimumPaymentForTender;

            }
            else
            {
                btnOk.Enabled = LoyaltyInfoRetrieved && retailTransaction.LoyaltyItem.SchemeExists;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            retailTransaction.LoyaltyItem = (LoyaltyItem)orgLoyaltyItem.Clone();

            if (retailTransaction.Customer.ID != orgCustomer.ID)
            {
                retailTransaction.Add(orgCustomer);
                retailTransaction.AddInvoicedCustomer(orgCustomer);
                Interfaces.Services.TransactionService(dlgEntry).RecalculatePriceTaxDiscount(dlgEntry, retailTransaction, true, true);
                Interfaces.Services.CalculationService(dlgEntry).CalculateTotals(dlgEntry, retailTransaction);
            }
        }

        private void pnlInfo_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, pnlInfo.Width - 1, pnlInfo.Height - 1);
            p.Dispose();
        }

        private void ntbAmount_TextChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();
            errorProvider.Visible = false;
            DisplayPointInformation();
        }
    }
}