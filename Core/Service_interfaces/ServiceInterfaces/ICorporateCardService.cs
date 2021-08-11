using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public interface ICorporateCardService : IService
    {
        /// <summary>
        /// Called from instance of class 'PayCorporateCard'.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings"></param>
        /// <param name="cardInfo">The card information as stored in the instance of class CardInfo.</param>
        /// <param name="amount">Amount receivable.</param>
        /// <param name="posTransaction">The complete POS transaction.</param>
        /// <example>
        /// <code>LSRetailPosis.ApplicationServices.ICorporateCard.ProcessCardPayment(cardInfo, paidAmount, this.posTransaction);</code>
        /// </example>
        ITenderLineItem ProcessCardPayment(IConnectionManager entry, ISettings settings, CardInfo cardInfo, decimal amount, object posTransaction);

        /// <summary>
        /// Voiding payment of a corporate card. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardInfo">The card information as stored in the instance of class CardInfo.</param>
        /// <param name="posTransaction">The complete POS transaction.</param>
        void VoidCardPayment(IConnectionManager entry, CardInfo cardInfo, object posTransaction);
    }
}
