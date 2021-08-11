using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.POS.Processes.Common;
using LSOne.Services.EFT.Common.Keyboard;
using LSOne.Services.EFT.Common.Touch;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Services.EFT.Common;
using LSOne.Services.Interfaces.Constants;
using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.BusinessObjects.StoreManagement;

namespace LSOne.Services
{
#if EFTSIMULATION
    public class EFTService : EFTBase,  IEFTService
    {
        public EFTService()
        {

        }

        #region IEFT Members

        public IErrorLog ErrorLog
        {
            set
            {

            }
        }

        public void Init(IConnectionManager entry)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            base.Init(entry, settings);

            DLLEntry.DataModel = entry;
            DLLEntry.Settings = settings;
        }

        /// <summary>
        /// Get the card information and / or just the amount to charge to the card.
        /// </summary>        
        /// <param name="amount"></param>
        /// <returns>True if the system should continue with the authorisation process and false otherwise.</returns>
        public bool GetCardInfoAndAmount(IConnectionManager entry, EFTInfo eftInfo, ref decimal amount)
        {
            // Default behaviour:  The connection to the EFT service provider has not been implemented
            //throw new LSRetailPosis.PosisException(50000, new Exception("The connection to an EFT service provider has not been implemented."));


            // In this function the Pay Card dialog is to be displayed and/or all information regarding the Credit Card retrieved. 
            // The following code is provided as a reference and demonstration on how the dialog would be displayed.

            //Example code:
            bool retVal = false;

            DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Trace, "GetCardInfoAndAmount()", this.ToString());

            if (DLLEntry.Settings.VisualProfile.TerminalType == VisualProfile.HardwareTypes.Touch)
            {
                //Show the Card dialog.
                using (PayCardDialog dialog = new PayCardDialog(amount))
                {
                    retVal = dialog.ShowDialog() == DialogResult.OK;
                    dialog.CardInformation.Populate(eftInfo.CardInformation);
                    eftInfo.CardInformation.TenderTypeId = "3";
                    amount = dialog.RegisteredAmount;
                }
            }
            else
            {
                //Show the Card dialog.
                frmPayCard cardDialog = new frmPayCard(amount);
                POSFormsManager.ShowPOSForm(cardDialog);

                cardDialog.CardInfo.Populate(eftInfo.CardInformation);
                retVal = cardDialog.OperationDone;
                amount = cardDialog.RegisteredAmount;

                cardDialog.Dispose();
            }
            return retVal;

        }

        public void ProcessCardPayment(IConnectionManager entry, EFTInfo eftInfo, IPosTransaction posTransaction)
        {
            // This is where card payments are being authorized.
            // From this point, the external EFT device / service can be called, passing in information from the eftInfo object.
            // Should the payment be authorized, set the eftInfo.Authorized variable to true and return true, else set the variable
            // to false and return false;

            //Example:
            eftInfo.Authorized = true; // Set this value to True if the request was authorized;
            return; // Return true if the request was authorized

            // Default behaviour:  The connection to the EFT service provider has not been implemented
            //throw new LSRetailPosis.PosisException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }

        public bool VoidTransaction(IConnectionManager entry, EFTInfo eftInfo, IPosTransaction posTransaction)
        {
            // Since cards are authorized through an external EFT terminal we must display a dialog to tell the operator to return
            // the proper amount to the card.

            IRoundingService rounding = (IRoundingService) entry.Service(ServiceType.RoundingService);

            string amount = rounding.RoundString(entry, (eftInfo.AmountInCents/100), DLLEntry.Settings.Store.Currency, true);
            string strMessage = "Please return " + amount + " to the card: ****-" + eftInfo.CardNumber.Substring(eftInfo.CardNumber.Length - 4, 4);

            ((IDialogService) entry.Service(ServiceType.DialogService)).ShowMessage(strMessage, MessageBoxButtons.OK, MessageDialogType.Generic);

            eftInfo.Authorized = true;
            return true;
        }

        public void GetBatchAmount(IConnectionManager entry, EFTInfo eftInfo, IPosTransaction posTransaction)
        {
            // Error!  The connection to the EFT service provider has not been implemented
            //throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }

        public void GetCurrentBatchNumber(IConnectionManager entry, EFTInfo eftInfo, IPosTransaction posTransaction)
        {
            // Error!  The connection to the EFT service provider has not been implemented
            //throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }

        public void IdentifyCard(IConnectionManager entry, CardInfo cardInfo, EFTInfo eftInfo)
        {
            eftInfo.CardInformation.TenderTypeId = "3";
            cardInfo.TenderTypeId = "3";
            // Error!  The connection to the EFT service provider has not been implemented
            //throw new LSRetailPosis.PosisException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }

        public void IncrementBatchNumber(IConnectionManager entry)
        {
            // Error!  The connection to the EFT service provider has not been implemented
            //throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }

        public bool ManualAuthCodeTransaction(IConnectionManager entry, EFTInfo eftInfo, IPosTransaction posTransaction)
        {
            // Error!  The connection to the EFT service provider has not been implemented
            //throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
            return true;
        }

        public void RefundTransaction(IConnectionManager entry, EFTInfo eftInfo, IPosTransaction posTransaction)
        {
            // Error!  The connection to the EFT service provider has not been implemented
            ((IDialogService) entry.Service(ServiceType.DialogService)).ShowMessage("Refund Transaction: Amount: " + eftInfo.Amount.ToString(), MessageBoxButtons.OK, MessageDialogType.Generic);
        }

        public void StartListening(IConnectionManager entry, IPosTransaction transaction, object parameter)
        {
            //throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }

        public void StopListening(IConnectionManager entry)
        {
            //throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }

        public void EMV_AuthorizeCard(IConnectionManager entry, IPosTransaction posTransaction, RecordIdentifier tenderID, PaymentInfo paymentInfo, bool authorizeQuick)
        {
            //throw new NotImplementedException();
        }

        public bool EMV_VoidCardPayment(IConnectionManager entry, IPosTransaction posTransaction, ICardTenderLineItem tenderLine, object parameter)
        {
            return false;
        }

        public IEFTSetupForm GetEftSetupForm(Terminal terminal)
        {
            return Services.Interfaces.Services.DialogService(DLLEntry.DataModel).GetEftSetupForm(terminal);

        }

        #endregion
    }
#endif
}
