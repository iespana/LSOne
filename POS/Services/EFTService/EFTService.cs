using LSOne.Controls.Dialogs;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.EFT;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.POS.Core.Exceptions;
using LSOne.POS.Processes.Common;
using LSOne.Services.EFT.Common;
using LSOne.Services.EFT.Common.Keyboard;
using LSOne.Services.EFT.Common.Touch;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSRetailPosis;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace LSOne.Services
{
#if (!EFTSIMULATION && !EFTSIMULATIONDROPDOWN)
    // This class was copied to Applications.IntegrationTests.[...].No_EFT_Module_EFTService and they should be kept in sync
    public partial class EFTService : EFTBase,  IEFTService
    {
        public EFTService()
        {
           
        }

        #region IEFT Members

        public virtual IErrorLog ErrorLog
        {
            set
            {

            }
        }

        public IEFTExtraInfo EFTExtraInfo => null;

        public IEFTTransactionExtraInfo EFTTransactionExtraInfo => null;

        public void Init(IConnectionManager entry)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            base.Init(entry, settings);

            DLLEntry.DataModel = entry;
            DLLEntry.Settings = settings;
        }

        public virtual void ProcessCardPayment(IConnectionManager entry, IEFTInfo eftInfo, IPosTransaction posTransaction)
        {
            // This is where card payments are being authorized.
            // From this point, the external EFT device / service can be called, passing in information from the eftInfo object.
            // Should the payment be authorized, set the eftInfo.Authorized variable to true and return true, else set the variable
            // to false and return false;

            //Example:
            //eftInfo.Authorized = true;   // Set this value to True if the request was authorized;            

            // Default behaviour:  The connection to the EFT service provider has not been implemented

            /*
             *
             * !!!!!NOTE!!!!!
             * 
             * The tender type that is assigned to the EFTInfo.TenderType has to exists in the data and it has to be configured properly. 
             * The code here below will tell you if there is something wrong with the tender type configuration - ideally if this code is used then the
             * code should be able to cancel the payment on the payment terminal or "fix" the tender before sending it back to the POS.
             * 
             * tenderInfo is null if the tender is not found in the database
             * tenderInfo.ID.SecondaryID is the StoreID which needs to be assigned to the tender
             * 
             * 
                StorePaymentMethod tenderInfo = Providers.StorePaymentMethodData.Get(entry, new RecordIdentifier(entry.CurrentStoreID, new RecordIdentifier(tenderID)), CacheType.CacheTypeApplicationLifeTime);
                if (tenderInfo == null)
                {
                    ((IDialogService)entry.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.TenderInformationNotFound.Replace("#1", (string)tenderID));
                    return;
                }
                if (tenderInfo.ID.SecondaryID == RecordIdentifier.Empty)
                {
                    // Invalid tender option
                    ((IDialogService)entry.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.TenderNotProperlyConfigured.Replace("#1", (string)tenderID), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return;
                }
             *      
             * 
             *  !!!!!NOTE!!!!!
             * 
             *  The below code is also NOT performed on the tender - if you need to check tender properties such as:
             *  AllowNegativePaymentAmounts, PaymentTypeCanBePartOfSplitPayment,MaximumAmountAllowed, MinimumAmountAllowed, MaximumOverTenderAmount and more 
             *  then this checks needs to be done somewhere in the card process
             *  
             
                LSRetailPosis.POSProcesses.Common.TenderRequirement tenderReq = new LSRetailPosis.POSProcesses.Common.TenderRequirement();
                if (tenderReq.IsTenderAllowed(tenderInfo, tenderedAmount, true, balanceAmount, totalTransactionAmount))
                {
                    ...
                }
             * 
             * */

            Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage("!!!!THIS IS A DEMO OF CARD AUTHORIZATION! NO ACTUAL PAYMENT IS BEING DONE!!!!");

            Interfaces.Services.DialogService(DLLEntry.DataModel).UpdateStatusDialog(Properties.Resources.ConnectingToTerminal);
            Thread.Sleep(3000);
            Interfaces.Services.DialogService(DLLEntry.DataModel).UpdateStatusDialog(Properties.Resources.WaitingForCardAndPin);
            Thread.Sleep(3000);
            Interfaces.Services.DialogService(DLLEntry.DataModel).UpdateStatusDialog(Properties.Resources.TakeTheCard);
            Thread.Sleep(2000);
            Interfaces.Services.DialogService(DLLEntry.DataModel).CloseStatusDialog();

            eftInfo.Authorized = true;
            eftInfo.CardInformation.ID = "00000001"; //The card type ID from table RBOSTORETENDERTYPECARDTABLE will be written to RBOTRANSACTIONPAYMENTTRANS.CARDTYPEID
            eftInfo.CardName = "Visa Debet"; //Used for printouts and receipt display

            //REMEMBER TO UPDATE THE EFTINFO.TENDERTYPE TO BE THE CORRECT TENDERTYPE
            //eftInfo.TenderType = //TENDER TYPE FROM PAYMENT TERMINAL - MUST EXIST IN POS DATABASE

            //throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }

        public virtual bool VoidTransaction(IConnectionManager entry, IEFTInfo eftInfo, IPosTransaction posTransaction)
        {
            // Error!  The connection to the EFT service provider has not been implemented
            throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }        

        public virtual void IdentifyCard(IConnectionManager entry, CardInfo cardInfo, IEFTInfo eftInfo)
        {
            // Error!  The connection to the EFT service provider has not been implemented
            throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }       

        public virtual bool ManualAuthCodeTransaction(IConnectionManager entry, IEFTInfo eftInfo, IPosTransaction posTransaction)
        {
            // Error!  The connection to the EFT service provider has not been implemented
            throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }

        public virtual void RefundTransaction(IConnectionManager entry, IEFTInfo eftInfo, IPosTransaction posTransaction)
        {
            // Error!  The connection to the EFT service provider has not been implemented
            throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }

        /// <summary>
        /// Intended to be used to start listening to external EFT hardware.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current POS transaction</param>
        /// <param name="parameter"></param>
        public virtual void StartListening(IConnectionManager entry, IPosTransaction posTransaction, object parameter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Intended to be used to stop listening to an external EFT hardware.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        public virtual void StopListening(IConnectionManager entry)
        {
            throw new NotImplementedException();
        }

        public virtual bool EMV_VoidCardPayment(IConnectionManager entry, IPosTransaction posTransaction,
                                        ICardTenderLineItem tenderLine, object parameter)
        {
            return false; //the old code will be used instead of this new function - see comments here below

            /*
             * 
             *  NOTE!!!
             *  
             * RETURN TRUE: this code takes care of the voiding of the card payment the POS core should not do anything further
             * RETURN FALSE: the POS core should run the older void payment code which calls IEFT.IdentifyCard and IEFT.VoidTransaction
             *  
             * NB!! The POS core doesn't do anything AT ALL in voiding the card payment. 
             *      All the work needs to be done here including:
             *             ** Voiding the actual payment line         
             *             ** Printing out the cancellation printout
             *             ** Displaying a message in message box for the user
             *             
             *      SEE REFERENCE CODE HERE BELOW TO SEE HOW THIS WAS DONE IN THE CORE
             */

            /*
             
            Some of the original void card code from the POS core - is only here for reference

            // Void the card transaction through the EFT service
            var eftInfo = tenderLine.EFTInfo;

            eftInfo.PreviousSequenceCode = tenderLine.EFTInfo.SequenceCode;

            if (!ProcessVoidUsingPaymentTerminal(...))
            //if (!ApplicationServices.IEFT.VoidTransaction(DLLEntry.DataModel, eftInfo, posTransaction))
            {
                //EMV_VoidCardPayment needs to always return true during this code so that the POS core knows it shouldn't do anything afterwareds
                return true;
            }

            if (eftInfo.Authorized)
            {
                tenderLine.EFTInfo = eftInfo;

                // ....and mark it as voided in our transaction.
                ((RetailTransaction) posTransaction).VoidPaymentLine(tenderLine.LineId);

                // Message: Successfully voided the card payment
                POSFormsManager.ShowPOSStatusPanelText(ApplicationLocalizer.Language.Translate(3611)); //Payment was voided
                ApplicationServices.IPrinting.PrintCardReceipt(DLLEntry.DataModel, FormType.EFTMessage, posTransaction, tenderLine, false);
              
                NOTE! Calculating the balance after this is taken care of by the POS core
            }

            return true;
              
             */
        }

        public virtual IEFTSetupForm GetEftSetupForm(Terminal terminal)
        {
            return null;
        }
        
        ///<inhericdoc cref="IEFTService"/>
        public virtual void EMV_AuthorizeCard(IConnectionManager entry, ISession session, IPosTransaction posTransaction, RecordIdentifier tenderID, PaymentInfo paymentInfo, bool authorizeQuick, bool manual, bool offline)
        {
            entry.ErrorLogger.LogMessage(LogMessageType.Trace, "AuthorizeCard - Started.", this.ToString());

            IRoundingService rounding = (IRoundingService)DLLEntry.DataModel.Service(ServiceType.RoundingService);

            StorePaymentMethod storePaymentMethod = Providers.StorePaymentMethodData.Get(DLLEntry.DataModel, new RecordIdentifier(DLLEntry.DataModel.CurrentStoreID, new RecordIdentifier(tenderID)), CacheType.CacheTypeApplicationLifeTime);

            decimal amountToPay = GetPayableAmountWithLimitation(posTransaction, paymentInfo, storePaymentMethod);

            decimal rndtransSalePmtDiff = rounding.RoundAmount(DLLEntry.DataModel, amountToPay, DLLEntry.DataModel.CurrentStoreID, tenderID, CacheType.CacheTypeTransactionLifeTime);

            decimal tenderAmount = decimal.Zero;
            bool continueWithPayment = false;

            // This is necessary for when the remainder of the payment is f.ex. 0,02 and due to rounding on the cash tender the payment is rounded to 0 
            if (rndtransSalePmtDiff == 0)
                return;

            // Select an amount to pay.

            // Display the cashpayment dialog

            if (DLLEntry.Settings.VisualProfile.TerminalType == VisualProfile.HardwareTypes.Touch)
            {
                using (var amountDialog = new PayCashDialog(amountToPay, storePaymentMethod))
                {
                    continueWithPayment = amountDialog.ShowDialog() == DialogResult.OK;
                    tenderAmount = amountDialog.RegisteredAmount;
                }
            }
            //else
            //{
            //    //If customer is using Dynakey then that code needs to go here
            //}

            if (!continueWithPayment || (tenderAmount == 0m))
            {
                Interfaces.Services.TenderRestrictionService(DLLEntry.DataModel).CancelUnconfirmedTenderRestriction(DLLEntry.DataModel, DLLEntry.Settings, posTransaction, storePaymentMethod);

                return;
            }

            /*
            * 
            * Note that all information that is added to the EFTInfo and CardInfo class is saved to the database
            * 
            */

            var cardInfo = new CardInfo { CardType = CardTypesEnum.Unknown };

            var eftInfo = new EFTInfo
            {
                Amount = tenderAmount,
                AmountInCents = Convert.ToDecimal(tenderAmount * 100),
                StaffId = (string)entry.CurrentStaffID,
                TenderType = (string)tenderID,
                CardInformation = cardInfo
            };

            //Process the card payment itself through the payment terminal
            ProcessCardPayment(entry, eftInfo, posTransaction);

            entry.ErrorLogger.LogMessage(LogMessageType.Trace, "AuthorizeCard - ProcessCardPayment finished.", this.ToString());

            // Exit if the card was not authorized
            if (eftInfo.Authorized == false)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "AuthorizeCard - Print receipt and exit as card was not authorized.", this.ToString());

                Interfaces.Services.TenderRestrictionService(DLLEntry.DataModel).CancelUnconfirmedTenderRestriction(DLLEntry.DataModel, DLLEntry.Settings, posTransaction, storePaymentMethod);

                CardTenderLineItem cardTender = new CardTenderLineItem { EFTInfo = eftInfo };
                Interfaces.Services.PrintingService(DLLEntry.DataModel).PrintCardReceipt(DLLEntry.DataModel, FormSystemType.EFTMessage, (PosTransaction)posTransaction, cardTender, false);

                return;
            }

            /*
                * 
                * If the payment was authorized - create the tender line and add it to the transaction             
                * 
                */

            entry.ErrorLogger.LogMessage(LogMessageType.Trace, "AuthorizeCard - Card was authorized.", this.ToString());

            // Generate the tender line for the card
            CardTenderLineItem tenderLine = new CardTenderLineItem
            {
                TenderTypeId = eftInfo.TenderType,
                CardName = eftInfo.CardName
            };

            tenderLine.Description = Properties.Resources.CardPayment + " " + tenderLine.CardName;

            //The tender description displayed on receipt and printed

            tenderLine.CardNumber = eftInfo.CardNumber;
            tenderLine.CardNumberHidden = eftInfo.CardNumberHidden;
            tenderLine.ExpiryDate = eftInfo.ExpDate;

            tenderLine.Amount = rounding.Round(entry, Convert.ToDecimal(eftInfo.AmountInCents) / 100,
                                                DLLEntry.Settings.Store.Currency,
                                                CacheType.CacheTypeTransactionLifeTime);
            tenderLine.OpenDrawer = storePaymentMethod.OpenDrawer;
            tenderLine.ChangeTenderID = (string)storePaymentMethod.ChangeTenderID;
            tenderLine.MinimumChangeAmount = storePaymentMethod.MinimumChangeAmount;
            tenderLine.AboveMinimumTenderId = (string)storePaymentMethod.AboveMinimumTenderID;
            tenderLine.EFTInfo = eftInfo;

            /*
                * 
                *  !!!!NOTE!!!!
                *  
                * ((CardTenderLineItem)tenderLine).CardTypeID value is saved to field RboTransactionPaymentTrans.CardTypeID
                * These card types are assigned to the tender in the Site Manager in Setup -> Stores -> Store -> Allowed Payments -> Select tender -> Allowed cards types
                * and created in Site Manager in General setup -> Payments -> Card types
                * 
                */

            tenderLine.CardTypeID = (string)eftInfo.CardInformation.ID;

            // Convert from the store-currency to the company-currency...
            tenderLine.CompanyCurrencyAmount = Services.Interfaces.Services.CurrencyService(DLLEntry.DataModel).CurrencyToCurrency(
                DLLEntry.DataModel,
                DLLEntry.Settings.Store.Currency,
                DLLEntry.Settings.CompanyInfo.CurrencyCode,
                DLLEntry.Settings.CompanyInfo.CurrencyCode,
                tenderAmount);
            // The exchange rate between the store amount(not the paid amount) and the company currency
            tenderLine.ExchrateMST = Interfaces.Services.CurrencyService(DLLEntry.DataModel).ExchangeRate(
                DLLEntry.DataModel,
                DLLEntry.Settings.Store.Currency) * 100;

            entry.ErrorLogger.LogMessage(LogMessageType.Trace, "AuthorizeCard - tenderline created.", this.ToString());

            if (posTransaction is RetailTransaction)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Trace,
                                                            "AuthorizeCard - Add a payment line to the retail transaction.",
                                                            this.ToString());

                ((RetailTransaction)posTransaction).Add(tenderLine);
                Interfaces.Services.TenderRestrictionService(DLLEntry.DataModel).UpdatePaymentIndex(DLLEntry.DataModel, DLLEntry.Settings, posTransaction, storePaymentMethod, tenderLine.LineId);

                //Split lines based on the registered amount
                Services.Interfaces.Services.TenderRestrictionService(DLLEntry.DataModel).SplitLines(DLLEntry.DataModel, DLLEntry.Settings, posTransaction, storePaymentMethod, tenderLine.LineId);
                posTransaction.LastRunOperationIsValidPayment = true;

                // Get infocodes if needed                                
                ProcessInfoCode(entry, session, posTransaction, posTransaction.StoreId, tenderLine);

                // Trigger: OnPayment trigger for a payment 
                ApplicationTriggers.IPaymentTriggers.OnPayment(entry, (RetailTransaction)posTransaction);
            }
            else if (posTransaction is CustomerPaymentTransaction)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Trace,
                                                            "AuthorizeCard - Add a payment line to the customer payment transaction ",
                                                            this.ToString());
                ((CustomerPaymentTransaction)posTransaction).Add(tenderLine);
                Interfaces.Services.TenderRestrictionService(DLLEntry.DataModel).UpdatePaymentIndex(DLLEntry.DataModel, DLLEntry.Settings, posTransaction, storePaymentMethod, tenderLine.LineId);
                posTransaction.LastRunOperationIsValidPayment = true;

                // Get infocodes if needed
                ProcessInfoCode(entry, session, posTransaction, posTransaction.StoreId, tenderLine);
            }
        }

        public virtual EFTReportResult XReport(IConnectionManager entry)
        {
            return new EFTReportResult();
        }

        public virtual EFTReportResult ZReport(IConnectionManager entry)
        {
            return new EFTReportResult();
        }

        public void RecoverTransaction(IConnectionManager entry, ISession session, IPosTransaction posTransaction)
        {
            //EFT plugin doesn't support or does not implement recovering transactions
            //Do not throw error since this is called at POS login
        }
        #endregion
    }
#endif
}