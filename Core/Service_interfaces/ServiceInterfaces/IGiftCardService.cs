using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGiftCardService : IService
    {
        /// <summary>
        /// Issues or updated a gift card that should be created as a change back when payment is made in the POS
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="giftCardTenderId">The tender type ID used for the gift card tender</param>
        /// <param name="amount">The amount that is to be issued</param>
        ITenderLineItem GiftCardChangeBack(IConnectionManager entry, IRetailTransaction retailTransaction, string giftCardTenderId, decimal amount);

        /// <summary>
        /// Issues a gift card. Depends on configurations on the Site service profile how the gift card is issued ID/Amount/ID and Amount
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="giftCardTenderId">The tender type ID used for the gift card tender</param>
        /// <param name="prefix">The prefix that is to be used when creating the gift card ID</param>
        /// <param name="numberSequenceLowest">The first value of the number sequence that should create the gift cards</param>
        void IssueGiftCard(IConnectionManager entry, IPosTransaction posTransaction, string giftCardTenderId, string prefix, int? numberSequenceLowest = null);

        /// <summary>
        /// Voids a gift card both in the transaction and at head office
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// <param name="giftCardId">The gift card ID that is to be voided</param>
        /// <param name="orginalBalance">The original balance of the gift card</param>
        bool VoidGiftCard(IConnectionManager entry, string giftCardId, decimal orginalBalance);

        /// <summary>
        /// Returns if the gift card can be used for the payment
        /// </summary>
        /// <param name="entry">The entry into the database</param>                
        /// <param name="giftCardId">The gift card ID that is to be authorized</param>
        /// <param name="amount">The amount to be authorized</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="paymentId">The tender type ID of the payment used</param>
        /// <param name="restrictedAmount">Maximum amount that can be paid in case of limitations</param>
        bool AuthorizeGiftCardPayment(IConnectionManager entry, ref string giftCardId, ref decimal amount, IPosTransaction posTransaction, string paymentId, decimal restrictedAmount);

        /// <summary>
        /// Voids a gift card payment that has already been authorized
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// <param name="giftCardId">The gift card ID to be voided</param>
        /// <param name="retailTransaction">The current transaction in the POS</param>
        bool VoidGiftCardPayment(IConnectionManager entry, string giftCardId, IRetailTransaction retailTransaction);

        /// <summary>
        /// Updates the gift certificate information if it has changed on the POS
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// <param name="giftCardId"></param>
        /// <param name="amount"></param>
        /// <param name="retailTransaction"></param>
        bool UpdateGiftCertificate(IConnectionManager entry, string giftCardId, decimal amount, IPosTransaction retailTransaction);

        /// <summary>
        /// Run when a gift card has been paid
        /// </summary>
        /// <param name="entry">Connection to the database</param>
        /// <param name="giftCard">The gift card that was paid</param>
        /// <param name="receiptID">The receipt ID of the transaction paying for the gift card</param>
        void GiftCardPaid(IConnectionManager entry, IGiftCertificateItem giftCard, RecordIdentifier receiptID);

        /// <summary>
        /// Get the balance of the gift card with a given ID
        /// </summary>
        /// <param name="entry">Connection to the database</param>
        void GetGiftCardBalance(IConnectionManager entry);

        /// <summary>
        /// Updates an already existing gift card payment with a receipt ID once it has been created.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="tenderLine">The gift card tenderline</param>
        /// <param name="transaction">The current transaction</param>
        void UpdateGiftCardPaymentReceipt(IConnectionManager entry, IGiftCertificateTenderLineItem tenderLine, IRetailTransaction transaction);

        /// <summary>
        /// Returns true if the Site service is needed to conclude a transaction that includes gift card information
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        /// <returns></returns>
        bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>
        /// Creates a gift card from information on a tender line item. This is used when a gift card is created to give change back and the gift card information
        /// needs to be printed when the rest of the sale information is being printed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        /// <param name="giftCertificateTenderLine">The gift card change back tender line</param>
        /// <returns></returns>
        IGiftCertificateItem CreateGiftCardFromTenderLine(IConnectionManager entry, IPosTransaction transaction, IGiftCertificateTenderLineItem giftCertificateTenderLine);
    }
}
