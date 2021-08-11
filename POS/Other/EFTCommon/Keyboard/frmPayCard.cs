using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Card;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionObjects.EFT;
using LSOne.Peripherals;
using LSOne.POS.Core;
using LSOne.POS.Core.Exceptions;
using LSOne.POS.Processes.Common;
using LSOne.POS.Processes.Enums;
using LSOne.POS.Processes.WinControls;
using LSOne.POS.Processes.WinControlsKeyboard;
using LSOne.POS.Processes.WinFormsKeyboard;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services.EFT.Common.Keyboard
{
    public enum InputBox
    {

        CardNumber = 1,
        ExpireDate = 2,
        Amount = 3
    }
    /// <summary>
    /// Summary description for frmPayCard.
    /// </summary>
    public partial class frmPayCard : frmKeyboardBase
    {
        private CardInfo cardInfo;
        private bool promptExpDate;      // Should the form prompt for an Exp. Date

        //private string track2;

        private int controlIndex;
        private string cardNumber;
        private string expireDate;
        private decimal amount;
        private bool operationDone;
        private decimal registeredAmount;
        private StorePaymentMethod tenderInfo;
        private decimal balanceAmount;

        private ArrayList amounts;
        private ArrayList buttons = new ArrayList();
        //private string previousCardNumber;

        #region Properties

        public string PromptText
        {
            set { stringPad1.PromptText = value; }
        }

        public CardInfo CardInfo
        {
            get { return cardInfo; }
        }

        public bool OperationDone
        {
            get
            {
                return operationDone;
            }
            set
            {
                operationDone = value;
            }
        }

        public decimal RegisteredAmount
        {
            get
            {
                return registeredAmount;
            }
            set
            {
                registeredAmount = value;
            }
        }

        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                cardNumber = value;
                lblCardNumber.Text = cardNumber;
            }
        }
        public string ExpireDate
        {
            get
            {
                return expireDate;
            }
            set
            {
                expireDate = value;
                lblExpirationDate.Text = expireDate;
            }
        }
        public decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
            }
        }

        #endregion

        public frmPayCard(decimal balanceAmount)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.operationDone = false;
            this.balanceAmount = balanceAmount;

            SetAmountViewer();
                        
            cardInfo = new CardInfo();
            this.operationDone = false;

            lblAmountToPay.Text = "";
            lblCardNumber.Text = "";
            lblExpirationDate.Text = "";
            controlIndex = (int)InputBox.CardNumber;
            SetButtonStatus();
            amount = 0;
            registeredAmount = 0;
            //track2 = "";
            promptExpDate = true;


            amtCardAmounts = new AmountViewer();
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for frmPayCard are reserved at 5180 - 5199
            // In use now are ID's: 5180 - 
            //
        }

        private void frmPayCard_Load(object sender, EventArgs e)
        {
            // Get the correct amounts for the buttons...
            amounts = amtCardAmounts.AmountStrings;
            GenerateFormButtons(true);

            MSR.MSRMessageEvent += this.MSR_MSRMessageEvent;
            MSR.EnableForSwipe();
        }

        private void txtAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                   //RegisterPayment(Convert.ToDecimal(stringPad1..Text.Trim()));
                }
                catch (Exception)
                {
                    // An invalid amount was entered
                    string message = Properties.Resources.PleaseEnterAnAmount;  
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                    
                    SetFormFocus();

                    return;
                }
            }
            else if (e.KeyCode == Keys.F1)
            {
                if (buttons[0] != null) { btnSelect_KeyboardButtonClickEvent(((KeyboardButton)buttons[0]).BaseButton, new EventArgs()); }
            }
            else if (e.KeyCode == Keys.F2)
            {
                if (buttons.Count == 3)
                {
                    btnManualInput_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count > 3)
                {

                }
            }
            else if (e.KeyCode == Keys.F3)
            {
                if (buttons.Count == 3)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count == 4)
                {
                    btnManualInput_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count > 4)
                {

                }
            }
            else if (e.KeyCode == Keys.F4)
            {
                if (buttons.Count == 4)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count == 5)
                {
                    btnManualInput_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count > 5)
                {

                }
            }
            else if (e.KeyCode == Keys.F5)
            {
                if (buttons.Count == 5)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count == 6)
                {
                    btnManualInput_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count > 6)
                {

                }
            }
            else if (e.KeyCode == Keys.F6)
            {
                if (buttons.Count == 6)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count == 7)
                {
                    btnManualInput_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count > 7)
                {

                }
            }
            else if (e.KeyCode == Keys.F7)
            {
                if (buttons.Count == 7)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count > 7)
                {
                    btnManualInput_KeyboardButtonClickEvent(this, new EventArgs());
                }
            }
            else if (e.KeyCode == Keys.F8)
            {
                if (buttons.Count == 8)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }

            }
        }

        private void RegisterPayment(decimal amount)
        {
            if (lblCardNumber.Text != "" && lblExpirationDate.Text != "")
            {
                registeredAmount = amount;
                operationDone = true;
                Close();
            }
            else
            {
                // Card information is missing
                string message = "Card information is missing";
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                
                SetFormFocus();

                return;

            }
        }

        void MSR_MSRMessageEvent(string track2)
        {
            this.StringPad1_CardSwept(track2);
        }

        #region Keyboard buttons

        private void GenerateFormButtons(bool justCancel)
        {
            try
            {
                // Create the buttons for the form...
                buttons.Clear();

                int buttonCounter = 1;

                IRoundingService rounding = (IRoundingService)DLLEntry.DataModel.Service(ServiceType.RoundingService);

                foreach (string amount in amounts)
                {
                    KeyboardButton btnAmount = new KeyboardButton(rounding.RoundString(
                        DLLEntry.DataModel,
                        Convert.ToDecimal(amount),
                        DLLEntry.Settings.Store.Currency, 
                        false,
                        CacheType.CacheTypeTransactionLifeTime), null);
                    btnAmount.KeyboardButtonClickEvent += new KeyboardButton.KeyboardButtonClickDelegate(btnAmount_KeyboardButtonClickEvent);
                    if (justCancel == false)
                        buttons.Add(btnAmount);

                    buttonCounter++;

                    if (buttonCounter == 8) { break; } // Button 8 is reserved for the Cancel action
                }

                for (int i = buttonCounter; i < 8; i++)
                {
                    KeyboardButton btnDummy = new KeyboardButton(null, null);
                    btnDummy.Enabled = false;
                    btnDummy.Visible = false;
                    buttons.Add(btnDummy);
                }

                // The last button
                KeyboardButton btnCancel = new KeyboardButton(Properties.Resources.Cancel, null); 
                btnCancel.KeyboardButtonClickEvent += new KeyboardButton.KeyboardButtonClickDelegate(btnCancel_KeyboardButtonClickEvent);
                buttons.Add(btnCancel);


                // Feed the arraylist to the control
                keyboardButtonControl.ShowMenuButtons(buttons);
            }
            catch (Exception)
            {
            }
        }

        void btnAmount_KeyboardButtonClickEvent(object sender, EventArgs e)
        {
            decimal amount = Convert.ToDecimal(((SimpleButton)sender).Text);
            RegisterPayment(amount);
        }

        void btnManualInput_KeyboardButtonClickEvent(object sender, EventArgs e)
        {
            frmInputCardInfo inputDialog = new frmInputCardInfo();
            POSFormsManager.ShowPOSForm(inputDialog);

            // Quit if cancel is pressed...
            if (inputDialog.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                // Get the card info and show it
                this.CardNumber = inputDialog.CardNo;
                this.ExpireDate = inputDialog.ExpDate;
            }
            else
            {
                // Clear the card info
                lblCardNumber.Text = "";
                lblExpirationDate.Text = "";
            }
            
            inputDialog.Dispose();
            SetFormFocus();
        }


        void btnSelect_KeyboardButtonClickEvent(object sender, EventArgs e)
        {
            if (stringPad1.EnteredValue != "")
                StringPad1_EnterButtonPressed();
            stringPad1.Focus();
            //gera einhvern fjandan sem gert er �egar �tt er � velja takkann.
        }

        void btnCancel_KeyboardButtonClickEvent(object sender, EventArgs e)
        {
            Close();
        }


        private void SetFormFocus()
        {
            stringPad1.Focus();
            //txtAmount.Focus();
        }


        #endregion

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtAmount_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    pictureEdit1.Focus();
                   // RegisterPayment(Convert.ToDecimal(txtAmount.Text.Trim()));
                }
                catch (Exception)
                {
                    // An invalid amount was entered
                    string message = Properties.Resources.PleaseEnterAnAmount;  //Please enter an amount
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                    
                    SetFormFocus();

                    return;
                }
            }
            else if (e.KeyCode == Keys.F1)
            {
                if (buttons[0] != null) { btnSelect_KeyboardButtonClickEvent(((KeyboardButton)buttons[0]).BaseButton, new EventArgs()); }
            }
            else if (e.KeyCode == Keys.F2)
            {
                if (buttons.Count == 2)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count > 3)
                {
                    if (buttons[1] != null) { btnSelect_KeyboardButtonClickEvent(((KeyboardButton)buttons[1]).BaseButton, new EventArgs()); }
                }
            }
            else if (e.KeyCode == Keys.F3)
            {
                if (buttons.Count == 3)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count == 4)
                {
                    btnManualInput_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count > 4)
                {
                    if (buttons[2] != null) { btnSelect_KeyboardButtonClickEvent(((KeyboardButton)buttons[2]).BaseButton, new EventArgs()); }
                }
            }
            else if (e.KeyCode == Keys.F4)
            {
                if (buttons.Count == 4)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count == 5)
                {
                    btnManualInput_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count > 5)
                {
                    if (buttons[3] != null) { btnSelect_KeyboardButtonClickEvent(((KeyboardButton)buttons[3]).BaseButton, new EventArgs()); }
                }
            }
            else if (e.KeyCode == Keys.F5)
            {
                if (buttons.Count == 5)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count == 6)
                {
                    btnManualInput_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count > 6)
                {

                }
            }
            else if (e.KeyCode == Keys.F6)
            {
                if (buttons.Count == 6)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count == 7)
                {
                    btnManualInput_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count > 7)
                {

                }
            }
            else if (e.KeyCode == Keys.F7)
            {
                if (buttons.Count == 7)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }
                else if (buttons.Count > 7)
                {
                    btnManualInput_KeyboardButtonClickEvent(this, new EventArgs());
                }
            }
            else if (e.KeyCode == Keys.F8)
            {
                if (buttons.Count == 8)
                {
                    btnCancel_KeyboardButtonClickEvent(this, new EventArgs());
                }

            }
        }

        private void txtAmount_Validating(object sender, CancelEventArgs e)
        {
            //if (txtAmount.Text.Length > 14)
             //   e.Cancel = true;
        }
        //evaluate entered value in stringpad1 control, amount or card number. 
        private void StringPad1_EnterButtonPressed()
        {
            try
            {
                if (controlIndex == (int)InputBox.Amount)
                {
                    IRoundingService rounding = (IRoundingService)DLLEntry.DataModel.Service(ServiceType.RoundingService);

                    decimal roundedAmount = 0M;
                    roundedAmount = rounding.RoundAmount(DLLEntry.DataModel, stringPad1.EnteredDecimalValue, DLLEntry.DataModel.CurrentStoreID, tenderInfo.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
                    Action(roundedAmount.ToString());
                }
                else if (controlIndex == (int)InputBox.CardNumber)
                {
                    // Check if we recognize the card type and can figure out which tendertype it belongs to.

                    cardInfo.CardEntryType = CardEntryTypesEnum.ManuallyEntered;
                    cardInfo.CardNumber = stringPad1.EnteredValue.Replace("-", "");
                    CardInfo.Track2 = "";

                    if (Services.Interfaces.Services.CardService(DLLEntry.DataModel).IsCardLengthValid(DLLEntry.DataModel, cardInfo.CardNumber) == true)
                    {
                        Services.Interfaces.Services.CardService(DLLEntry.DataModel).GetCardType(DLLEntry.DataModel, cardInfo);

                        cardInfo.ExpDateCheck = true;

                        if (cardInfo.CardType == CardTypesEnum.Unknown)
                        {
                            // The card was not recognized by the POS, so we continue to get the exp. date in order to be able to send the
                            // card to the EFT broker for identification.                            
                            Action(stringPad1.EnteredValue);
                        }
                        else
                        {
                            // The card was recognized by the POS 

                            // Is it legal to manually enter the cardnumber for this card type?
                            if (cardInfo.AllowManualInput == false)
                            {
                                // Manual input for this cardtype is not allowed...
                                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.CardTypeCannotManuallyEnter, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                                
                                stringPad1.TryAgain();
                                return;
                            }

                            // Should we prompt for the exp date?
                            promptExpDate = cardInfo.ExpDateCheck;

                            // Gathering tender information

                            tenderInfo = Providers.StorePaymentMethodData.Get(DLLEntry.DataModel, new RecordIdentifier(DLLEntry.DataModel.CurrentStoreID, new RecordIdentifier(cardInfo.TenderTypeId)), CacheType.CacheTypeApplicationLifeTime);

                            // Display the name of the tender type
                            lblCard.Text = tenderInfo.Text;

                            // Setting the AmountViewer to reflect on the selected tender type
                            SetAmountViewer();

                            // Get the correct amounts for the buttons...
                            amounts = amtCardAmounts.AmountStrings;
                            GenerateFormButtons(false);

                            Action(stringPad1.EnteredValue);
                        }
                    }
                    else  //Cardnumber to short or too long
                    {
                        Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.InvalidCardLength, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                        
                        stringPad1.TryAgain();
                    }

                }
                else if (controlIndex == (int)InputBox.ExpireDate)
                {
                    cardInfo.ExpDate = stringPad1.EnteredValue.Replace("/", "");

                    if (Services.Interfaces.Services.CardService(DLLEntry.DataModel).IsExpiryDateValid(DLLEntry.DataModel, cardInfo.ExpDate) == true)
                    {
                        if (cardInfo.CardType == CardTypesEnum.Unknown)
                        {
                            // The card was not recognized by the POS system (not found in the POSISCardTable) so we try to 
                            // let the broker identify the card.
                            IEFTInfo eftInfo = new EFTInfo();
                            Services.Interfaces.Services.EFTService(DLLEntry.DataModel).IdentifyCard(DLLEntry.DataModel, cardInfo, eftInfo);

                            if (cardInfo.TenderTypeId != "")
                            {
                                // The broker identified the card...

                                // Getting the correct tenderId according to a table mapping
                                cardInfo.TenderTypeId = Providers.CardToTenderMappingData.Get(DLLEntry.DataModel, DLLEntry.Settings.HardwareProfile.EftDescription, cardInfo.TenderTypeId).Text;

                                // Gathering tender information
                                tenderInfo = Providers.StorePaymentMethodData.Get(DLLEntry.DataModel, new RecordIdentifier(DLLEntry.DataModel.CurrentStoreID, new RecordIdentifier(cardInfo.TenderTypeId)), CacheType.CacheTypeApplicationLifeTime);

                                if (tenderInfo.AllowOverTender)
                                {
                                    amtCardAmounts.ViewOption = AmountViewer.ViewOptions.HigherAmounts;
                                }
                                else
                                {
                                    amtCardAmounts.ViewOption = AmountViewer.ViewOptions.ExcactAmountOnly;
                                }
                                amtCardAmounts.HighestOptionAmount = tenderInfo.MaximumAmountAllowed;
                                amtCardAmounts.LowesetOptionAmount = tenderInfo.MinimumAmountAllowed;


                                // Display the name of the tender type
                                lblCard.Text = tenderInfo.Text;

                                // Setting the AmountViewer to reflect on the selected tender type
                                //  SetAmountViewer();
                            }
                            else
                            {
                                // The broker did not identify the card.
                                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.CardUnidentified, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                                

                                Close();
                            }
                        }

                        Action(stringPad1.EnteredValue);
                    }
                    else  //Expiry date is not valid
                    {
                        Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.ExpiryDateNotValid, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                        
                    }
                }
                else
                {
                    Action(stringPad1.EnteredValue);
                }
            }
            catch (POSException px)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowErrorMessage(px.Message, px.InnerException.ToString());
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        private void SetAmountViewer()
        {
            try
            {
                decimal roundedAmount = 0M;

                IRoundingService rounding = (IRoundingService)DLLEntry.DataModel.Service(ServiceType.RoundingService);

                if (tenderInfo != null)
                {
                    roundedAmount = rounding.RoundAmount(DLLEntry.DataModel, balanceAmount, DLLEntry.DataModel.CurrentStoreID, tenderInfo.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);

                    if (tenderInfo.AllowOverTender)
                    {
                        amtCardAmounts.ViewOption = AmountViewer.ViewOptions.HigherAmounts;
                    }
                    else
                    {
                        amtCardAmounts.ViewOption = AmountViewer.ViewOptions.ExcactAmountOnly;
                    }
                    amtCardAmounts.HighestOptionAmount = tenderInfo.MaximumAmountAllowed;
                    amtCardAmounts.LowesetOptionAmount = tenderInfo.MinimumAmountAllowed;
                    amtCardAmounts.SetButtons(roundedAmount, DLLEntry.Settings.Store.Currency);
                }
                else
                {
                    roundedAmount = rounding.Round(
                        DLLEntry.DataModel, 
                        balanceAmount,
                        DLLEntry.Settings.Store.Currency,
                        CacheType.CacheTypeTransactionLifeTime);

                    amtCardAmounts.SetAmountLabel(roundedAmount, DLLEntry.Settings.Store.Currency, "");
                }

                LineDisplay.DisplayBalance(rounding.RoundString(
                    DLLEntry.DataModel,
                    roundedAmount,
                    DLLEntry.Settings.Store.Currency,
                    true,
                    CacheType.CacheTypeTransactionLifeTime));
            }
            catch (POSException px)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowErrorMessage(px.Message, px.InnerException.ToString());
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        //get card number from stringpad1
        private void StringPad1_CardSwept(string track2)
        {
            try
            {
                cardInfo.Track2 = track2;
                cardInfo.CardEntryType = CardEntryTypesEnum.MagneticStripeRead;

                Services.Interfaces.Services.CardService(DLLEntry.DataModel).GetCardType(DLLEntry.DataModel, cardInfo);

                if (cardInfo.CardType == CardTypesEnum.Unknown)
                {
                    // The card was not recognized  by the POS, so we try to identify the card through the EFT broker
                    EFTInfo eftInfo = new EFTInfo();
                    Services.Interfaces.Services.EFTService(DLLEntry.DataModel).IdentifyCard(DLLEntry.DataModel, cardInfo, eftInfo);

                    if (cardInfo.TenderTypeId != "")
                    {
                        // Getting the correct tenderId according to a table mapping
                        cardInfo.TenderTypeId = Providers.CardToTenderMappingData.Get(DLLEntry.DataModel, DLLEntry.Settings.HardwareProfile.EftDescription, cardInfo.TenderTypeId).Text;
                    }
                }
                if (cardInfo.TenderTypeId != "")
                {
                    // The card has been identified...

                    // Gathering tender information
                    tenderInfo = Providers.StorePaymentMethodData.Get(DLLEntry.DataModel, new RecordIdentifier(DLLEntry.DataModel.CurrentStoreID, new RecordIdentifier(cardInfo.TenderTypeId)), CacheType.CacheTypeApplicationLifeTime);

                    // Display the name of the tender type
                    lblCard.Text = tenderInfo.Text;

                    //show new amount buttons. 
                   // ShowPaymentButtons(cardInfo.TenderTypeId);
                }
                else
                {                    
                    // The card was not identified..
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.CardUnidentified, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                    

                    Close();
                }
                if (cardInfo.CardNumberHidden == true)
                {
                    if ((cardInfo.CardNumber == "") && (cardInfo.Track2.Length > 16))
                    {
                        string separator = DLLEntry.Settings.HardwareProfile.Separator1;
                        this.CardNumber = "****-****-**" + cardInfo.Track2.Substring(cardInfo.Track2.IndexOf(separator) - 6, 6);
                        this.CardNumber = this.CardNumber.Insert(14, "-");
                    }
                    else if (cardInfo.CardNumber.Length >= 16)
                    {
                        this.CardNumber = "****-****-**" + cardInfo.CardNumber.Substring(cardInfo.CardNumber.Length - 6, 6);
                        this.CardNumber = this.CardNumber.Insert(14, "-");
                    }
                    else
                        this.CardNumber = "****-****-****-****";
                    this.ExpireDate = "**/**";
                }
                else
                {
                    if ((cardInfo.CardNumber == "") && (cardInfo.Track2.Length > 16))
                    {
                        int sepPos = cardInfo.Track2.IndexOf(DLLEntry.Settings.HardwareProfile.Separator1);
                        this.CardNumber = cardInfo.Track2.Substring(0, sepPos);
                        if (sepPos > 0)
                        {
                            string month = cardInfo.Track2.Substring(sepPos + 3, 2);
                            string year = cardInfo.Track2.Substring(sepPos + 1, 2);
                            this.ExpireDate = month + "/" + year;
                        }
                    }
                    else if (cardInfo.CardNumber.Length >= 16)
                    {
                        this.CardNumber = cardInfo.CardNumber;
                    }
                }

                controlIndex = (int)InputBox.Amount;
                SetButtonStatus();
            }
            catch (POSException px)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowErrorMessage(px.Message, px.InnerException.ToString());
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void SetButtonStatus()
        {            
            try
            {
                switch (controlIndex)
                {

                    case (int)InputBox.CardNumber:
                        stringPad1.EntryType = StringPadEntryTypes.CardValidation;
                        stringPad1.PromptText = Properties.Resources.EnterCardNumber;
                        break;
                    case (int)InputBox.ExpireDate:
                        if (promptExpDate == true)
                        {
                            stringPad1.EntryType = StringPadEntryTypes.CardExpireValidation;
                            stringPad1.PromptText = Properties.Resources.EnterCardNumber;
                        }
                        else
                        {
                            controlIndex++;
                            SetButtonStatus();
                        }
                        break;
                    case (int)InputBox.Amount:
                        stringPad1.NegativeMode = this.balanceAmount < 0;
                        stringPad1.EntryType = StringPadEntryTypes.Price;
                        stringPad1.MaxNumberOfDigits = 7;
                        stringPad1.MaskChar = "";
                        stringPad1.MaskInterval = 0;
                        stringPad1.PromptText = Properties.Resources.EnterAmount;
                        break;

                }
            }
            catch (POSException px)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowErrorMessage(px.Message, px.InnerException.ToString());
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        private void Action(string value)
        {
            try
            {
                // If reading is successful index is incremented or form is close
                if (SetValue(value))
                {
                    // Set the necessary input validation... 
                    if (cardInfo.ExpDateCheck == false)
                    {
                        if (lblAmountToPay.Text != "" && lblCardNumber.Text != "")
                        {
                            operationDone = true;
                            registeredAmount = amount;
                            Close();
                        }
                    }
                    else
                    {
                        if (lblAmountToPay.Text != "" && lblCardNumber.Text != "" && lblExpirationDate.Text != "")
                        {
                            operationDone = true;
                            registeredAmount = amount;
                            this.DialogResult = DialogResult.OK;
                            Close();
                        }
                    }
                }
                // in the end numpad value is cleared and buttons set ready for next input
                controlIndex++;
                if (controlIndex > 3)
                {
                    controlIndex = 1;
                    stringPad1.EntryType = StringPadEntryTypes.CardValidation;
                    stringPad1.PromptText = Properties.Resources.EnterCardNumber;
                        
                }
                                
                stringPad1.Clear();
                SetButtonStatus();
            }
            catch (POSException px)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowErrorMessage(px.Message, px.InnerException.ToString());
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        private bool SetValue(string value)
        {
            Color selectColor = ColorPalette.POSTextboxBackgroundColor;
            bool valueOK = false;
            try
            {
                if (value != "")
                {
                    switch (controlIndex)
                    {
                        case (int)InputBox.Amount:
                            Amount = Convert.ToDecimal(value);
                            valueOK = (((Amount > 0) && (this.balanceAmount >= 0)) || ((Amount < 0) && (this.balanceAmount < 0)));
                            break;
                        case (int)InputBox.CardNumber:
                            CardNumber = value;
                            valueOK = (CardNumber != "");
                            break;
                        case (int)InputBox.ExpireDate:
                            ExpireDate = value;
                            if (promptExpDate == true)
                                valueOK = (ExpireDate != "");
                            else
                                valueOK = true;
                            break;

                    }
                }
            }
            catch (FormatException)
            {

            }
            return valueOK;
        }

        private void frmPayCard_FormClosed(object sender, FormClosedEventArgs e)
        {
            MSR.MSRMessageEvent -= MSR_MSRMessageEvent;
            MSR.DisableForSwipe();
        }
    }
}

