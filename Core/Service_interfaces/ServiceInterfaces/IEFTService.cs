using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces
{
    public interface IEFTService : IService
    {
        /// <summary>
        /// Should return a default instance of the implementation of <see cref="IEFTExtraInfo"/> that this implementation of <see cref="IEFTService"/> uses. If you do not intend to use this simply return <see langword="null"/>
        /// </summary>
        /// <returns></returns>
        IEFTExtraInfo EFTExtraInfo { get; }

        /// <summary>
        /// Should return a default instance of the implementation of <see cref="IEFTTransactionExtraInfo"/>  that this implementation of <see cref="IEFTService"/> uses. If you do not intend to use this simply return <see langword="null"/>
        /// </summary>
        IEFTTransactionExtraInfo EFTTransactionExtraInfo { get; }

        /// <summary>
        /// Processes the card payment.
        /// Takes decision whether this transaction is about voiding a transaction. If this is not the case,
        /// then a connection to the broker is established and it is attempted to achieve authorisation for the card payment.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="eftInfo">The reference to an eftInfo object.</param>
        /// <param name="posTransaction">The current transaction.</param>
        /// <returns>A boolean, giving information about whether the authorisation succeeded or not.</returns>
        void ProcessCardPayment(IConnectionManager entry, IEFTInfo eftInfo, IPosTransaction posTransaction);

        /// <summary>
        /// When the authentication is done manually. Accesses and communicates with the broker. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="eftInfo">The reference to an eftInfo object.</param>
        /// <param name="posTransaction">The current transaction.</param>
        /// <returns>A boolean, giving information about whether the authorisation succeeded or not.</returns>
        bool ManualAuthCodeTransaction(IConnectionManager entry, IEFTInfo eftInfo, IPosTransaction posTransaction);

        /// <summary>
        /// For debet cards, the transaction number must be entered manually
        /// and must match the original transaction number.
        /// <br></br>
        /// On the other hand, if a debetcard payment in the current transaction is being voided,
        /// then it is provided and therefore does not have to be entered manually.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="eftInfo">The reference to an eftInfo object.</param>
        /// <param name="posTransaction">The current transaction.</param>
        /// <returns></returns>
        bool VoidTransaction(IConnectionManager entry, IEFTInfo eftInfo, IPosTransaction posTransaction);

        /// <summary>
        /// Refund a card payment transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="eftInfo">The reference to an eftInfo object.</param>
        /// <param name="posTransaction">The current transaction.</param>
        void RefundTransaction(IConnectionManager entry, IEFTInfo eftInfo, IPosTransaction posTransaction);


        /// <summary>
        /// Loads the eftInfo class with necessay information from the cardInfo class: <br></br>
        /// Whether the card was read with a stripe reader or the card data entered manually, <br></br>
        /// assigns the new WinPOSXClass instance the transaction type and passes it as an argument to the EFT payment form.
        /// After the broker identified the card type, we populate the cardInfo class with the proper information (CardType, TenderTypeId).
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// <param name="cardInfo">Information about the card being checked</param>
        /// <param name="eftInfo">EFT information</param>
        void IdentifyCard(IConnectionManager entry, CardInfo cardInfo, IEFTInfo eftInfo);

        /// <summary>
        /// To fetch the amount of the current batch for the current terminal Id from the broker.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="eftInfo">The reference to an eftInfo object.</param>
        /// <param name="posTransaction">The current transaction.</param>
        void GetBatchAmount(IConnectionManager entry, IEFTInfo eftInfo, IPosTransaction posTransaction);

        /// <summary>
        /// Get the current batch number
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="eftInfo">The reference to an eftInfo object.</param>
        /// <param name="posTransaction">The current transaction.</param>
        void GetCurrentBatchNumber(IConnectionManager entry, IEFTInfo eftInfo, IPosTransaction posTransaction);

        /// <summary>
        /// Increment batch number
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        void IncrementBatchNumber(IConnectionManager entry);

        /// <summary>
        /// Intended to be used to start listening to external EFT hardware.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        /// <param name="parameter"></param>
        void StartListening(IConnectionManager entry, IPosTransaction posTransaction, object parameter);

        /// <summary>
        /// Intended to be used to stop listening to an external EFT hardware.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        void StopListening(IConnectionManager entry);


        /// <summary>
        /// Allows for the customization of Chip and pin or other external payment terminals. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name = "session" > The current session</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="tenderID">The ID of the tender selected on the payment button</param>
        /// <param name="paymentInfo">Payment information for the transaction</param>
        /// <param name="authorizeQuick">Indicates wether this is the AuthorizeCardQuick operation. If true then the implementation should skip the payment amount dialog and send the amount directly to the EFT device</param>
        /// <param name="manual">Indicates if this is a manual entry. Must be supported by the EFT device.</param>
        /// <param name="offline">Indicates if this is an offline payment. Must be supported by the EFT device.</param>
        void EMV_AuthorizeCard(IConnectionManager entry, ISession session, IPosTransaction posTransaction, RecordIdentifier tenderID, PaymentInfo paymentInfo, bool authorizeQuick, bool manual, bool offline);

        /// <summary>
        /// Allows for voiding of a card payment the customization of Chip and pin or other external payment terminals. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="tenderLine">The selected tender line to be voided</param>
        /// <param name="parameter">An unspecific parameter that can be used in customization</param>
        bool EMV_VoidCardPayment(IConnectionManager entry, IPosTransaction posTransaction, ICardTenderLineItem tenderLine, object parameter);

        /// <summary>
        /// Returns a form enabling the user to setup EFT from the POS
        /// </summary>
        /// <param name="terminal">A reference to the terminal to use as reference</param>
        /// <returns></returns>
        IEFTSetupForm GetEftSetupForm(Terminal terminal);

        /// <summary>
        /// Perform a X report operation on the EFT device, if the device supports it
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        EFTReportResult XReport(IConnectionManager entry);

        /// <summary>
        /// Perform a Z report operation on the EFT device, if the device supports it
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        EFTReportResult ZReport(IConnectionManager entry);

        /// <summary>
        /// Try to recover a payment from the payment terminal in case something bad happened and the POS didn't register the payment
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">Current session</param>
        /// <param name="posTransaction">Current transaction</param>
        /// <returns></returns>
        void RecoverTransaction(IConnectionManager entry, ISession session, IPosTransaction posTransaction);
    }
}
