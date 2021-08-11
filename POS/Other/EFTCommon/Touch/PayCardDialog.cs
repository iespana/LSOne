using LSOne.Controls;
using LSOne.Controls.Dialogs;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionObjects.EFT;
using LSOne.Peripherals;
using LSOne.POS.Core.Exceptions;
using LSOne.Services.EFT.Common.Properties;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Services.EFT.Common.Touch
{
    public partial class PayCardDialog : TouchBaseForm
    {
        private CardInfo cardInformation;
        private StorePaymentMethod tenderInformation;
        private decimal balanceAmount;
        private bool lastDateValid;
        private bool chipAndPin;

        public CardInfo CardInformation
        {
            get
            {
                return cardInformation;
            }
        }

        public decimal RegisteredAmount { get; set; }
        public bool OperationDone { get; set; }
        public string CardNumber { get; set; }
        public string ExpireDate { get; set; }
        public string CVV { get; set; }

        public PayCardDialog()
        {
            InitializeComponent();
        }

        public PayCardDialog(decimal balanceAmount)
            : this(balanceAmount, false)
        {
        }

        public PayCardDialog(decimal balanceAmount, bool chipAndPin)
        {
            this.chipAndPin = chipAndPin;
            InitializeComponent();

            this.balanceAmount = balanceAmount;
            tbExpireDate.Text = "";
            Currency currency = Providers.CurrencyData.Get(DLLEntry.DataModel, DLLEntry.Settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
            ntbAmount.DecimalLetters = currency.RoundOffAmount.DigitsBeforeFirstSignificantDigit();
            ntbAmount.AllowDecimal = ntbAmount.DecimalLetters > 0;
            ntbAmount.Value = (double)this.balanceAmount;
            SetDefaultBannerText();
            touchScrollButtonPanel.TabStop = false; 
            touchScrollButtonPanel.AddButton(Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
            //Field validation
            tbCardNumber.LostFocus += tbCardNumber_LostFocus;
            tbExpireDate.LostFocus += tbExpireDate_LostFocus;
            tbCVV.LostFocus += tbCVV_LostFocus;
            ntbAmount.LostFocus += (sender, args) => { ((TextBox) sender).BackColor = ColorPalette.POSTextboxBackgroundColor; };
            tbCardNumber.GotFocus += textBoxGotFocus;
            tbExpireDate.GotFocus += textBoxGotFocus;
            tbCVV.GotFocus += textBoxGotFocus;
            ntbAmount.GotFocus += textBoxGotFocus;
            //Escape and enter handling
            tbCardNumber.KeyDown += numericTextBox_KeyDown;
            ntbAmount.KeyDown += numericTextBox_KeyDown;
            tbExpireDate.KeyDown += numericTextBox_KeyDown;
            tbCVV.KeyDown += numericTextBox_KeyDown;
            cardInformation = new CardInfo();

            if (!DesignMode)
            {
                tbCardNumber.StartCharacter = DLLEntry.Settings.HardwareProfile.StartTrack1;
                tbCardNumber.EndCharacter = DLLEntry.Settings.HardwareProfile.EndTrack1;
                tbCardNumber.Seperator = DLLEntry.Settings.HardwareProfile.Separator1;
                tbCardNumber.TrackSeperation = TrackSeperation.Before;
            }

            SetNegativeMode(balanceAmount);
        }

        public void DisableAmountEntry()
        {
            ntbAmount.Enabled = false;
        }

        private void tbCardNumber_LostFocus(object sender, EventArgs e)
        {
            try
            {
                ValidateCardNumber();
                ((TextBox)sender).BackColor = ColorPalette.POSTextboxBackgroundColor;
            }
            catch {}
        }

        private void tbExpireDate_LostFocus(object sender, EventArgs e)
        {
            try
            {
                ValidateExpirationDate();
                ((TextBox) sender).BackColor = ColorPalette.POSTextboxBackgroundColor;
            }
            catch {}
        }
        
        private void tbCVV_LostFocus(object sender, EventArgs e)
        {
            try
            {
                ValidateCVV();
                ((TextBox)sender).BackColor = ColorPalette.POSTextboxBackgroundColor;
            }
            catch {}
        }
                  
        private void textBoxGotFocus(object sender, EventArgs e)
        {
            ((TextBox) sender).BackColor = ColorPalette.TextBoxGotFocus;
        }
        
        private bool ValidateEntry()
        {
            return ValidateCardNumber() && ValidateExpirationDate() && ValidateCVV() && ValidateAmount();
        }

        private void touchNumPad1_EnterPressed(object sender, EventArgs e)
        {
            if (ActiveControl == ntbAmount)
            {
                if (ValidateEntry())
                {
                    return;
                }
            }
            SelectNextControl(ActiveControl, true, true, false, true);
        }

        private void touchScrollButtonPanel1_Click(object sender, ScrollButtonEventArguments args)
        {
            if (args.Tag is DialogResult && ((DialogResult) args.Tag) == DialogResult.OK)
            {
                if (ValidateEntry())
                    return;
            }
            if (args.Tag is DialogResult)
            {
                DialogResult = (DialogResult) args.Tag;
                Close();
                return;
            }
            if (args.Tag is decimal)
            {
                var rounding = Interfaces.Services.RoundingService(DLLEntry.DataModel);
                RegisteredAmount = rounding.RoundAmount(DLLEntry.DataModel, (decimal)args.Tag, DLLEntry.DataModel.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
                DialogResult = DialogResult.OK;
                Close();
            }
            if (args.Tag is bool)
            {
                touchScrollButtonPanel.Clear();
                touchScrollButtonPanel.AddButton(Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
                tbCardNumber.Enabled = tbExpireDate.Enabled = true;
                tbCardNumber.Text = tbExpireDate.Text = "";
                tbCardNumber.Focus();
            }
        }

        private void touchNumPad1_ClearPressed(object sender, EventArgs e)
        {
            if (ActiveControl is TextBox)
            {
                ((TextBox) ActiveControl).Text = "";
            }
        }

        private void numericTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            if (e.KeyCode == Keys.Enter && tbCardNumber.Enabled)
            {
                touchNumPad1_EnterPressed(sender, e);
            }
        }

        private void PayCardDialog_Load(object sender, EventArgs e)
        {
            MSR.MSRMessageEvent += OnMsrMessageEvent;
            MSR.EnableForSwipe();
        }

        private void PayCardDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            MSR.MSRMessageEvent -= OnMsrMessageEvent;
            MSR.DisableForSwipe();
        }

        private void OnMsrMessageEvent(string track2)
        {
            try
            {
                CardInformation.Track2 = track2;
                CardInformation.CardEntryType = CardEntryTypesEnum.MagneticStripeRead;

                Services.Interfaces.Services.CardService(DLLEntry.DataModel).GetCardType(DLLEntry.DataModel, CardInformation);

                if (CardInformation.CardType == CardTypesEnum.Unknown)
                {
                    // The card was not recognized  by the POS, so we try to identify the card through the EFT broker
                    if (!chipAndPin)
                    {
                        var eftInfo = new EFTInfo();
                        Services.Interfaces.Services.EFTService(DLLEntry.DataModel).IdentifyCard(DLLEntry.DataModel, CardInformation, eftInfo);

                        if (CardInformation.TenderTypeId != "")
                        {
                            // Getting the correct tenderId according to a table mapping
                            CardInformation.TenderTypeId = Providers.CardToTenderMappingData.Get(DLLEntry.DataModel,
                                    DLLEntry.Settings.HardwareProfile.EftDescription, CardInformation.TenderTypeId).Text;
                        }
                    }
                }

                if (CardInformation.TenderTypeId != "")
                {
                    tenderInformation = Providers.StorePaymentMethodData.Get(DLLEntry.DataModel, new RecordIdentifier(DLLEntry.DataModel.CurrentStoreID, CardInformation.TenderTypeId), CacheType.CacheTypeApplicationLifeTime);

                    touchDialogBanner.BannerText = tenderInformation.Text + " - " + touchDialogBanner.BannerText;
                    touchScrollButtonPanel.SetButtonsCurrency(balanceAmount, tenderInformation, DLLEntry.Settings.Store.Currency);
                    touchScrollButtonPanel.AddButton(Resources.Pay, DialogResult.OK, "", TouchButtonType.OK, DockEnum.DockEnd);
                    touchScrollButtonPanel.AddButton(Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
                    ntbAmount.DecimalLetters = tenderInformation.Rounding.DigitsBeforeFirstSignificantDigit();
                    ntbAmount.AllowDecimal = ntbAmount.DecimalLetters > 0;
                }
                else
                {
                    // The card was not identified..
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.CardUnidentified, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);//Card type unrecognized by the EFT broker

                    Close();
                }
                if (CardInformation.CardNumberHidden)
                {
                    if ((CardInformation.CardNumber == "") && (CardInformation.Track2.Length > 16))
                    {
                        string separator = DLLEntry.Settings.HardwareProfile.Separator1;
                        CardNumber = "****-****-**" + CardInformation.Track2.Substring(CardInformation.Track2.IndexOf(separator) - 6, 6);
                        CardNumber = CardNumber.Insert(14, "-");
                    }
                    else if (CardInformation.CardNumber.Length >= 16)
                    {
                        CardNumber = "****-****-**" + CardInformation.CardNumber.Substring(CardInformation.CardNumber.Length - 6, 6);
                        CardNumber = CardNumber.Insert(14, "-");
                    }
                    else
                    {
                        CardNumber = "****-****-****-****";
                    }
                    ExpireDate = "**/**";
                }
                else
                {
                    if ((CardInformation.CardNumber == "") && (CardInformation.Track2.Length > 16))
                    {
                        int sepPos = CardInformation.Track2.IndexOf(DLLEntry.Settings.HardwareProfile.Separator1);
                        CardNumber = CardInformation.Track2.Substring(0, sepPos);
                        if (sepPos > 0)
                        {
                            string month = CardInformation.Track2.Substring(sepPos + 3, 2);
                            string year = CardInformation.Track2.Substring(sepPos + 1, 2);
                            ExpireDate = month + "/" + year;
                        }
                    }
                    else if (CardInformation.CardNumber.Length >= 16)
                    {
                        CardNumber = CardInformation.CardNumber;
                    }
                }
            }
            catch (POSException px)
            {
                ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(px.InnerException.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private void SetDefaultBannerText()
        {
            touchDialogBanner.BannerText = Properties.Resources.Card;
            touchDialogBanner.BannerText += " - ";
            if (tenderInformation == null)
            {
                touchDialogBanner.BannerText += Properties.Resources.AmountDue.Replace("#1",
                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel,
                        balanceAmount,
                        DLLEntry.Settings.Store.Currency,
                        true,
                        CacheType.CacheTypeApplicationLifeTime));
            }
            else
            {
                touchDialogBanner.BannerText += Properties.Resources.AmountDue.Replace("#1",
                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel,
                        Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundAmount(DLLEntry.DataModel,
                            balanceAmount,
                            DLLEntry.DataModel.CurrentStoreID,
                            tenderInformation.ID.SecondaryID,
                            CacheType.CacheTypeTransactionLifeTime),
                        DLLEntry.Settings.Store.Currency,
                        true,
                        CacheType.CacheTypeApplicationLifeTime));

            }
        }

        private void SetBannerText(string text)
        {
            touchDialogBanner.BannerText = text;
            touchDialogBanner.BannerText += " - ";
            touchDialogBanner.BannerText += Properties.Resources.AmountDue.Replace("#1",
                Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel,
                    balanceAmount,
                    DLLEntry.Settings.Store.Currency,
                    true,
                    CacheType.CacheTypeApplicationLifeTime));
        }

        private bool ValidateCardNumber()
        {
            try
            {
                // Check if we recognize the card type and can figure out which tendertype it belongs to.

                cardInformation = new CardInfo();
                cardInformation.CardNumber = tbCardNumber.Text.Replace("-", "");

                if (Services.Interfaces.Services.CardService(DLLEntry.DataModel).IsCardLengthValid(DLLEntry.DataModel, cardInformation.CardNumber))
                {
                    Services.Interfaces.Services.CardService(DLLEntry.DataModel).GetCardType(DLLEntry.DataModel, cardInformation);

                    // setting default values
                    SetDefaultBannerText();
                    tbExpireDate.Enabled = true;
                    tenderInformation = null;

                    if (cardInformation.CardType != CardTypesEnum.Unknown)
                    {
                        // The card was recognized by the POS 

                        // Is it legal to manually enter the cardnumber for this card type?
                        if (!cardInformation.AllowManualInput)
                        {
                            // Manual input for this cardtype is not allowed...
                            Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.ManuallyEnteredNotAllowed, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);//The card number for this card type cannot be manually entered
                            tbCardNumber.Text = "";
                            return false;
                        }
                        // Should we prompt for the exp date?
                        tbExpireDate.Enabled = cardInformation.ExpDateCheck;

                        // Gathering tender information
                        tenderInformation = Providers.StorePaymentMethodData.Get(DLLEntry.DataModel, new RecordIdentifier(DLLEntry.DataModel.CurrentStoreID, cardInformation.TenderTypeId), CacheType.CacheTypeApplicationLifeTime);

                        // Display the name of the tender type
                        SetBannerText(tenderInformation.Text);
                    }
                    if (tenderInformation != null)
                    {
                        touchScrollButtonPanel.SetButtonsCurrency(balanceAmount, tenderInformation,DLLEntry.Settings.Store.Currency);
                        touchScrollButtonPanel.AddButton(Resources.Pay, DialogResult.OK, "", TouchButtonType.OK, DockEnum.DockEnd);
                        touchScrollButtonPanel.AddButton(Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);

                        if (!string.IsNullOrEmpty(CardNumber))
                        {
                            if (ntbAmount.Text != "" && tbCardNumber.Text != "" &&
                                (tbExpireDate.Text != "" || !cardInformation.ExpDateCheck))
                            {
                                var rounding = Interfaces.Services.RoundingService(DLLEntry.DataModel);
                                RegisteredAmount = rounding.RoundAmount(DLLEntry.DataModel, (decimal) ntbAmount.Value,
                                                                        DLLEntry.DataModel.CurrentStoreID, tenderInformation.ID.SecondaryID,
                                                                        CacheType.CacheTypeTransactionLifeTime);
                                DialogResult = DialogResult.OK;
                                Close();
                            }
                        }
                    }
                }
                else //Cardnumber to short or too long
                {
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.CardNumberLengthNotValid, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);//Card number length is not valid.
                    return false;
                }
                return true;
            }
            catch (POSException px)
            {
                ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(px.InnerException.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return false;
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private bool ValidateExpirationDate()
        {
            try
            {
                cardInformation.ExpDate = tbExpireDate.Text.Replace("/", "");
                if (ExpireDate == cardInformation.ExpDate)
                {
                    return lastDateValid; //Already been checked
                }
                if (Services.Interfaces.Services.CardService(DLLEntry.DataModel).IsExpiryDateValid(DLLEntry.DataModel, cardInformation.ExpDate))
                {
                    ExpireDate = cardInformation.ExpDate;
                    if (cardInformation.CardType == CardTypesEnum.Unknown && !chipAndPin)
                    {
                        // The card was not recognized by the POS system (not found in the POSISCardTable) so we try to 
                        // let the broker identify the card.
                        var eftInfo = new EFTInfo();
                        Services.Interfaces.Services.EFTService(DLLEntry.DataModel).IdentifyCard(DLLEntry.DataModel, cardInformation, eftInfo);

                        if (cardInformation.TenderTypeId != "")
                        {
                            // The broker identified the card...

                            // Getting the correct tenderId according to a table mapping
                            var cardTender = Providers.CardToTenderMappingData.Get(DLLEntry.DataModel, DLLEntry.Settings.HardwareProfile.EftDescription, cardInformation.TenderTypeId);
                            if (cardTender != null)
                            {
                                cardInformation.TenderTypeId = cardTender.Text;
                            }

                            // Gathering tender information
                            tenderInformation = Providers.StorePaymentMethodData.Get(DLLEntry.DataModel, new RecordIdentifier(DLLEntry.DataModel.CurrentStoreID, cardInformation.TenderTypeId), CacheType.CacheTypeApplicationLifeTime);

                            if (tenderInformation.ID.SecondaryID == RecordIdentifier.Empty)
                            {
                                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage("Tender type cannot be found. Please make sure that the TenderTypeId on CardInfo is not empty (cardInfo.TenderTypeId). Operation cancelled", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Tender type cannot be found. Please make sure that the TenderTypeId on CardInfo is not empty (cardInfo.TenderTypeId). Operation cancelled", "EFT.Touch.frmPayCard.numPad1_EnterButtonPressed");
                                lastDateValid = false;
                                return false;
                            }

                            // Display the name of the tender type
                            SetBannerText(tenderInformation.Text);

                            // Setting the AmountViewer to reflect on the selected tender type
                            touchScrollButtonPanel.SetButtonsCurrency(balanceAmount, tenderInformation, DLLEntry.Settings.Store.Currency);
                            touchScrollButtonPanel.AddButton(Resources.Pay, DialogResult.OK, "", TouchButtonType.OK, DockEnum.DockEnd);
                            touchScrollButtonPanel.AddButton(Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
                        }
                        else
                        {
                            // The broker did not identify the card.
                            Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.CardUnidentified, MessageBoxButtons.OK, MessageDialogType.ErrorWarning); //Card type unrecognized by the EFT broker

                            // Registering the expire date entered into the text box
                            if (tbExpireDate.Text != "" || !cardInformation.ExpDateCheck)
                            {
                                if (ntbAmount.Text != "" && tbCardNumber.Text != "" && (tbExpireDate.Text != "" || !cardInformation.ExpDateCheck))
                                {
                                    var rounding = Interfaces.Services.RoundingService(DLLEntry.DataModel);
                                    RegisteredAmount = rounding.RoundAmount(DLLEntry.DataModel, (decimal)ntbAmount.Value, DLLEntry.DataModel.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
                                    DialogResult = DialogResult.OK;
                                    Close();
                                }
                            }
                        }
                    }
                }
                else //Expiry date is not valid
                {
                    ExpireDate = cardInformation.ExpDate;
                    if (cardInformation.ExpDate != "")
                    {
                        Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.ExpiryDateNotValid, MessageBoxButtons.OK, MessageDialogType.ErrorWarning); //Expiry date is not valid.   
                    }
                    lastDateValid = false;
                    return false;
                }
                lastDateValid = true;
                return true;
            }
            catch (POSException px)
            {
                ((IDialogService) DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(px.InnerException.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                lastDateValid = false;
                return false;
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private bool ValidateCVV()
        {
            CVV = "";
            if (tbCVV.Text.Trim().Length == 0)
                return true;
            try
            {
                int cvv;
                if (!Int32.TryParse(tbCVV.Text, out cvv))
                {
                    ((IDialogService) DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.CVVNotValid, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return false;
                }
                if (cvv < 100 || cvv > 9999)
                {
                    // cvv can only hold values of 3 or four digits
                    ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.CVVNotValid, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return false;
                }
                CVV = tbCVV.Text;
                return true;
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private bool ValidateAmount()
        {
            try
            {
                decimal roundedAmount;
                // If we have information about the card, the thereby the tender, we have grounds to round the amount, else no rounding will be done
                if (tenderInformation != null)
                {
                    IRoundingService rounding = (IRoundingService) DLLEntry.DataModel.Service(ServiceType.RoundingService);
                    roundedAmount = rounding.RoundAmount(DLLEntry.DataModel, (decimal) ntbAmount.Value, DLLEntry.DataModel.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
                }
                else
                {
                    roundedAmount = (decimal) ntbAmount.Value;
                }

                if (((roundedAmount > 0) && (balanceAmount >= 0)) || ((roundedAmount < 0) && (balanceAmount < 0)))
                {
                    if ((touchNumPad.NegativeMode == true))
                    {
                        if ((roundedAmount > 0))
                        {
                            ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Resources.OnlyNegativeAmountsAllowed, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                            return false;
                        }
                        if (roundedAmount < balanceAmount)
                        {
                            ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Resources.NegativePaymentsNotBelowBalance, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                            return false;
                        }
                    }

                    if (ntbAmount.Text != "" && tbCardNumber.Text != "" && (tbExpireDate.Text != "" || !cardInformation.ExpDateCheck))
                    {
                        RegisteredAmount = roundedAmount;
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                else
                {
                    ntbAmount.Text = "";
                    return false;
                }
                return true;
            }
            catch (POSException px)
            {
                ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(px.InnerException.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return false;
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private void tbCardNumber_TextChanged(object sender, EventArgs e)
        {
            string text = ((TextBox) sender).Text;
            if (text.Length > 15 && !text.StartsWith(DLLEntry.Settings.HardwareProfile.StartTrack1) && !text.Contains("-"))
            {
                text = text.Insert(12, "-");
                text = text.Insert(8, "-");
                text = text.Insert(4, "-");
                ((TextBox) sender).Text = text;
                text = ((MSRTextBoxTouch) sender).GetTrack(((MSRTextBoxTouch) sender).LastTrack, TrackSeperation.After);
                tbCardNumber.Enabled = false;

                if (text != null && text.Length >= 4)
                {
                    tbExpireDate.Text = new string(new[] {text[2], text[3], '/', text[0], text[1]});
                    tbExpireDate.Enabled = false;
                }
                else
                    tbExpireDate.Text = "";

                touchScrollButtonPanel.Clear();
                if (chipAndPin)
                {
                    touchScrollButtonPanel.AddButton(Resources.Pay, DialogResult.OK, "" , TouchButtonType.OK, DockEnum.DockEnd);
                }
                touchScrollButtonPanel.AddButton(Resources.ClearCard, true, "", TouchButtonType.Normal, DockEnum.DockEnd);
                touchScrollButtonPanel.AddButton(Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
            }
        }

        private void touchNumPad1_TouchKeyPressed(object sender, Controls.EventArguments.TouchKeyKeyEventArgs args)
        {
            if (ActiveControl is TextBox)
            {
                ((TextBox) ActiveControl).Focus();
            }
        }

        private void touchNumPad1_PlusMinusPressed(object sender, EventArgs e)
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

        private void SetNegativeMode(decimal balanceAmount)
        {
            if (balanceAmount < 0)
            {
                touchNumPad.NegativeMode = true;
                touchNumPad.ShowPlusMinusToggle = true;
                ntbAmount.AllowNegative = true;
                ntbAmount.Text = "-";
                ntbAmount.SelectionStart = 1;
                ntbAmount.SelectionLength = 0;
                ntbAmount.SuppressNextSelectAll();
            }
        }
    }
}
