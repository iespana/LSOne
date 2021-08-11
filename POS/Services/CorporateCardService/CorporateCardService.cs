using System;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.POS.Core;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    /// <summary>
    /// A service that handles all activity related to corporate cards. Is only for backwards compatability and is not used in any functionality
    /// </summary>
    public partial class CorporateCardService : ICorporateCardService
    {
        /// <summary>
        /// The current transaction
        /// </summary>
        protected RetailTransaction retailTransaction;
        /// <summary>
        /// The information about the card retrieved from the card swipe
        /// </summary>
        protected CardInfo cardInfo;
        /// <summary>
        /// The amount that is to be charged to the corporate card
        /// </summary>
        protected decimal amount;

        #region ICorporateCard Members


        /// <summary>
        /// Processes the corporate card payment
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current entry</param>
        /// <param name="cardInfo">The information about the card that was swiped</param>
        /// <param name="amount">the amount to be charged</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <returns></returns>
        public virtual ITenderLineItem ProcessCardPayment(IConnectionManager entry, ISettings settings, CardInfo cardInfo, decimal amount, object posTransaction)
        {
            try
            {
                TenderLineItem tenderLine = null;

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Processing payment", "CorporateCard.ProcessCardPayment");

                IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

                this.cardInfo = cardInfo;
                this.amount = rounding.RoundAmount(entry, amount, entry.CurrentStoreID, cardInfo.TenderTypeId, CacheType.CacheTypeTransactionLifeTime);
                
                retailTransaction = (RetailTransaction)posTransaction;

                decimal payableAmt = SetTenderRestrictions(entry, settings, (RetailTransaction)posTransaction, cardInfo.TenderTypeId);
                payableAmt = rounding.RoundAmount(entry, payableAmt, entry.CurrentStoreID, cardInfo.TenderTypeId, CacheType.CacheTypeTransactionLifeTime);

                if (payableAmt > 0)
                {
                    this.amount = payableAmt; 
                   
                    // Process the payment...
                    tenderLine = ProcessCorporateCardPayment(entry);
                    
                }
                return tenderLine;
            }
            catch(Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }


        /// <summary>
        /// Voids the card payment. Is not implemented
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardInfo">Information about the card that was swiped</param>
        /// <param name="posTransaction">The current transaction</param>
        public virtual void VoidCardPayment(IConnectionManager entry, CardInfo cardInfo, object posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Voiding payment", "CorporateCard.VoidCardPayment");
                
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        #endregion


        /// <summary>
        /// Processes the corporate card payment
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A corporate card tender line</returns>
        protected virtual TenderLineItem ProcessCorporateCardPayment(IConnectionManager entry)
        {
            try
            {

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "", "CorporateCard.ProcessCorporateCardPayment");

                // Calculate a markup amount and add a markup item to the transaction
                //AddCorporateCardMarkup();

                // Add the corporate card tenderline to the transaction
                return AddCorporateCardTenderLine(entry);

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        /// <summary>
        /// Creates a corporate card tender lien item and returns it
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        protected virtual CorporateCardTenderLineItem AddCorporateCardTenderLine(IConnectionManager entry)
        {

            try
            {

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Adding the tender line to the transaction", "CorporateCard.AddCorporateCardTenderLine");

                bool getAdditionalInfo = false;

                // Generate the tender line for the card
                //CardTenderLineItem cardTenderLine = new CardTenderLineItem();
                CorporateCardTenderLineItem cardTenderLine = new CorporateCardTenderLineItem();
                cardTenderLine.TenderTypeId = (string)cardInfo.ID;
                cardTenderLine.CardName = cardInfo.CardName;
                cardTenderLine.Description = Properties.Resources.CardPayment + " " + cardInfo.CardName; 
                cardTenderLine.Amount = amount;

                if (cardInfo.CardEntryType == CardEntryTypesEnum.MagneticStripeRead)
                {
                    // We have the track, from which we need to get the card number and exp date
                    cardTenderLine.CardNumber = cardInfo.Track2.Substring(0,cardInfo.Track2.IndexOf("-"));
                    cardTenderLine.ExpiryDate = cardInfo.Track2.Substring(cardInfo.Track2.IndexOf("-") + 1, 4);
                    //getAdditionalInfo = cardInfo.Track2.Substring(...);
                }
                else
                {
                    // We have the card number and the exp. date
                    cardTenderLine.CardNumber = cardInfo.CardNumber;
                    cardTenderLine.ExpiryDate = cardInfo.ExpDate;
                    getAdditionalInfo = true;
                }

                // Get information about the tender...
                StorePaymentMethod tenderInfo = Providers.StorePaymentMethodData.Get(entry, new RecordIdentifier(entry.CurrentStoreID, cardInfo.TenderTypeId), CacheType.CacheTypeApplicationLifeTime);
                if (tenderInfo == null)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoPaymentInformationOnButton);
                    return null;
                }
                if (tenderInfo.ID.SecondaryID == RecordIdentifier.Empty)
                {
                    // Invalid tender option
                    ((IDialogService)entry.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.TheCashPaymentHasNotBeenSetUp, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return null;
                }
                cardTenderLine.OpenDrawer = tenderInfo.OpenDrawer;
                cardTenderLine.ChangeTenderID = (string)tenderInfo.ChangeTenderID;
                cardTenderLine.MinimumChangeAmount = tenderInfo.MinimumChangeAmount;
                cardTenderLine.AboveMinimumTenderId = (string)tenderInfo.AboveMinimumTenderID;

                //Get additional information on corporate card if needed
                cardTenderLine = GetAdditionalInformation(entry, cardTenderLine, getAdditionalInfo);

                ISettings settings = ((ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication));

                //convert from the store-currency to the company-currency...
                cardTenderLine.CompanyCurrencyAmount = Services.Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                    entry,
                    settings.Store.Currency,
                    settings.CompanyInfo.CurrencyCode,
                    settings.CompanyInfo.CurrencyCode,
                    amount);
                // the exchange rate between the store amount(not the paid amount) and the company currency
                cardTenderLine.ExchrateMST = Services.Interfaces.Services.CurrencyService(entry).ExchangeRate(
                    entry,
                    settings.Store.Currency)*100;

                retailTransaction.Add(cardTenderLine);

                retailTransaction.LastRunOperationIsValidPayment = true;
                return cardTenderLine;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        /// <summary>
        /// Gets additional information for the corporate card
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardTenderLine">The card tender line that should hold the additional information</param>
        /// <param name="getAdditionalInformation">If true the additinal information dialog is displayed</param>
        /// <returns></returns>
        protected virtual CorporateCardTenderLineItem GetAdditionalInformation(IConnectionManager entry, CorporateCardTenderLineItem cardTenderLine, bool getAdditionalInformation)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Additional fleet card information", "CorporateCard.GetAdditionalInformation");
                ISettings settings = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication));

                if (getAdditionalInformation)
                {
                    InformationDialog frmAddInfo = new InformationDialog(entry, true);

                    if (frmAddInfo.ShowDialog() == DialogResult.OK)
                    {
                        cardTenderLine.DriverId = frmAddInfo.DriverId;
                        cardTenderLine.VehicleId = frmAddInfo.VehicleId;
                        cardTenderLine.OdometerReading = Convert.ToInt32(frmAddInfo.Odometer);
                    }
                }
                return cardTenderLine;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        /// <summary>
        /// Checks if any tender restrictions are on the card and displays a dialog to tell the user which items have not been paid for
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current settings</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="tenderTypeID">The tender type that is being paid with</param>
        /// <returns></returns>
        protected virtual decimal SetTenderRestrictions(IConnectionManager entry, ISettings settings, RetailTransaction posTransaction, RecordIdentifier tenderTypeID)
        {
            try
            {
                decimal payableAmt = decimal.Zero;

                IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

                StorePaymentMethod paymentMethod = Providers.StorePaymentMethodData.Get(entry, new RecordIdentifier(settings.Store.ID, tenderTypeID), CacheType.CacheTypeApplicationLifeTime);
                
                TenderRestrictionResult tenderRestrictionResult = Interfaces.Services.TenderRestrictionService(entry).DisplayTenderRestrictionInformation(entry, settings, posTransaction, paymentMethod, ref payableAmt);
                if (tenderRestrictionResult != TenderRestrictionResult.Continue && tenderRestrictionResult != TenderRestrictionResult.NoTenderRestrictions)
                {
                    switch (tenderRestrictionResult)
                    {
                        case TenderRestrictionResult.NothingCanBePaidFor:
                            Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.PaymentMethodCannotBeUsedToPayForItems, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                            return decimal.Zero;
                        case TenderRestrictionResult.CancelledByUser:
                            //Do nothing here as the user clicked cancel
                            return decimal.Zero;
                    }
                }
                payableAmt = rounding.Round(entry, payableAmt, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime); 
                 
                if (payableAmt != posTransaction.NetAmountWithTax)
                {
                    tenderRestrictionResult = Interfaces.Services.TenderRestrictionService(entry).DisplayTenderRestrictionInformation(entry, settings, posTransaction, paymentMethod, ref payableAmt);

                    if (tenderRestrictionResult == TenderRestrictionResult.CancelledByUser)
                    {
                        Interfaces.Services.TenderRestrictionService(entry).ClearTenderRestriction(entry, retailTransaction);
                        return decimal.Zero;
                    }
                }
                return payableAmt;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        /// <summary>
        /// Access to the error log functionality
        /// </summary>
        public virtual IErrorLog ErrorLog
        {
            set 
            {  
            
            }
        }

        /// <summary>
        /// Initializes the Corporate card service and sets the database connection for the service
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618

        }
    }
}
