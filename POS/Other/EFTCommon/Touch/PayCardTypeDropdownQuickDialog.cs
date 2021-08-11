using LSOne.Controls;
using LSOne.Controls.Dialogs;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionObjects.EFT;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Services.EFT.Common.Touch
{
    public partial class PayCardTypeDropdownQuickDialog : TouchBaseForm
    {
        private CardInfo cardInformation;
        private StorePaymentMethod tenderInformation;
        private List<StorePaymentTypeCardType> cardTypes;
        private decimal balanceAmount;
        private RecordIdentifier paymentTypeID;

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

        public bool ShowCardNumberField { get; set; }

        public RecordIdentifier PaymentTypeID
        {
            get
            {
                return paymentTypeID;
            }
            set
            {
                paymentTypeID = value;
            }
        }

        public PayCardTypeDropdownQuickDialog()
        {
            InitializeComponent();
            cardTypes = new List<StorePaymentTypeCardType>();
            ShowCardNumberField = false;
            paymentTypeID = RecordIdentifier.Empty;
        }

        public PayCardTypeDropdownQuickDialog(decimal balanceAmount)
            : this()
        {
            this.balanceAmount = balanceAmount; 
            touchScrollButtonPanel1.TabStop = false;
            touchScrollButtonPanel1.AddButton(Properties.Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
            
            cardInformation = new CardInfo();
        }

        private void touchNumPad1_EnterPressed(object sender, EventArgs e)
        {
            if (!(ActiveControl is ShadeNumericTextBox))
            {
                SelectNextControl(ActiveControl, true, true, false, true);
                return;
            }
            Pay();
        }

        private void Pay()
        {
            if (tenderInformation == null && !RecordIdentifier.IsEmptyOrNull(comboBox1.SelectedDataID))
            {
                var tenderTypeID = new RecordIdentifier(DLLEntry.DataModel.CurrentStoreID, comboBox1.SelectedDataID);
                tenderInformation = Providers.StorePaymentMethodData.Get(DLLEntry.DataModel, tenderTypeID);
            }

            if (!ValidateEntry())
                return;

            var rounding = (IRoundingService)DLLEntry.DataModel.Service(ServiceType.RoundingService);

            CaptureCardInfo();

            RegisteredAmount = balanceAmount;

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateEntry()
        {
            if (ShowCardNumberField && string.IsNullOrEmpty(tbCardNumber.Text.Trim()))
            {
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.PleaseEnterCardNumber,
                    MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return false;
            }

            if (tenderInformation == null || RecordIdentifier.IsEmptyOrNull(comboBox1.SelectedDataID))
            {
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.PleaseSelectCardTypeFirst,
                   MessageBoxButtons.OK, MessageDialogType.ErrorWarning); //The amount entered is higher than the maximum amount allowed          
                return false;
            }

            return true;
        }

        private void touchScrollButtonPanel1_Click(object sender, ScrollButtonEventArguments args)
        {
            if (args.Tag is DialogResult && ((DialogResult) args.Tag) == DialogResult.OK)
            {
                Pay();
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
                if (!ValidateEntry())
                    return;
                var rounding = Interfaces.Services.RoundingService(DLLEntry.DataModel);
                RegisteredAmount = rounding.RoundAmount(DLLEntry.DataModel, (decimal)args.Tag, 
                    DLLEntry.DataModel.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
                CaptureCardInfo();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void CaptureCardInfo()
        {
            if (cardInformation == null)
            {
                cardInformation = new CardInfo();
            }

            var paymentCardType = comboBox1.SelectedData as StorePaymentTypeCardType;

            cardInformation.TenderTypeId = tenderInformation.StoreAndTenderTypeID.SecondaryID;
            cardInformation.ID = (string)paymentCardType.CardTypeID;
            if (ShowCardNumberField)
                cardInformation.CardNumber = tbCardNumber.Text;
            cardInformation.CardName = paymentCardType.Text;
        }

        private void touchNumPad1_ClearPressed(object sender, EventArgs e)
        {
            if (ActiveControl is TextBox)
            {
                ((TextBox) ActiveControl).Clear();
            }
        }

        private void numericTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                touchNumPad1_EnterPressed(sender, e);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!ShowCardNumberField)
            {
                lblCardNumber.Visible = tbCardNumber.Visible = false;
                Size = new Size(Size.Width, Size.Height - 37);
            }

            MSR.MSRMessageEvent += OnMsrMessageEvent;
            MSR.EnableForSwipe();

            cardTypes = paymentTypeID == RecordIdentifier.Empty ? Providers.StoreCardTypesData.GetList(DLLEntry.DataModel, DLLEntry.Settings.Store.ID) : Providers.StoreCardTypesData.GetList(DLLEntry.DataModel, DLLEntry.Settings.Store.ID, paymentTypeID);
            if (cardTypes.Count > 0)
            {
                comboBox1.SelectedData = cardTypes[0];
                SetPaymentInformation();
            }

            Currency currency = Providers.CurrencyData.Get(DLLEntry.DataModel, DLLEntry.Settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
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
                cardInformation.Track2 = track2;
                cardInformation.CardEntryType = CardEntryTypesEnum.MagneticStripeRead;

                Services.Interfaces.Services.CardService(DLLEntry.DataModel).GetCardType(DLLEntry.DataModel, cardInformation);

                if (cardInformation.CardType == CardTypesEnum.Unknown)
                {
                    // The card was not recognized  by the POS, so we try to identify the card through the EFT broker
                    var eftInfo = new EFTInfo();
                    Services.Interfaces.Services.EFTService(DLLEntry.DataModel).IdentifyCard(DLLEntry.DataModel, cardInformation, eftInfo);

                    if (cardInformation.TenderTypeId != "")
                    {
                        // Getting the correct tenderId according to a table mapping
                        cardInformation.TenderTypeId = Providers.CardToTenderMappingData.Get(DLLEntry.DataModel, DLLEntry.Settings.HardwareProfile.EftDescription, cardInformation.TenderTypeId).Text;
                    }
                }


                if (cardInformation.TenderTypeId != "")
                {
                    // The card has been identified...

                    // Gathering tender information
                    tenderInformation = Providers.StorePaymentMethodData.Get(DLLEntry.DataModel, new RecordIdentifier(DLLEntry.DataModel.CurrentStoreID, cardInformation.TenderTypeId), CacheType.CacheTypeApplicationLifeTime);

                    // Display the name of the tender type
                    SetBannerText(tenderInformation.Text);

                    touchScrollButtonPanel1.Clear();
                    touchScrollButtonPanel1.AddButton(Properties.Resources.Pay, DialogResult.OK, "", TouchButtonType.OK, DockEnum.DockEnd);
                    touchScrollButtonPanel1.AddButton(Properties.Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
                }
                else
                {
                    // The card was not identified..
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.CardUnidentified, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);

                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                throw;
            }
        }

        private void SetBannerText(string text)
        {
            touchDialogBanner1.BannerText = text;
            touchDialogBanner1.BannerText += " - ";
            touchDialogBanner1.BannerText += Properties.Resources.AmountDue.Replace("#1",
                Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel,
                balanceAmount,
                DLLEntry.Settings.Store.Currency,
                true,
                CacheType.CacheTypeApplicationLifeTime));
        }


        private void SetPaymentInformation()
        {
            try
            {
                var tenderTypeID = (string)comboBox1.SelectedDataID;
                tenderInformation = Providers.StorePaymentMethodData.Get(DLLEntry.DataModel, new RecordIdentifier(DLLEntry.DataModel.CurrentStoreID, tenderTypeID));
                if (tenderInformation != null)
                {
                    SetBannerText(tenderInformation.Text);
                    touchScrollButtonPanel1.Clear();
                    touchScrollButtonPanel1.AddButton(Properties.Resources.Pay, DialogResult.OK, "", TouchButtonType.OK, DockEnum.DockEnd);
                    touchScrollButtonPanel1.AddButton(Properties.Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
                }
                else
                {
                    SetBannerText(Properties.Resources.Card);
                }
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                throw;
            }
        }

        private void comboBox1_SelectedDataChanged(object sender, EventArgs e)
        {
            SetPaymentInformation();
        }

        private void comboBox1_DropDown(object sender, DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                cardTypes,
                null,
                true,
                comboBox1.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }
    }
}
