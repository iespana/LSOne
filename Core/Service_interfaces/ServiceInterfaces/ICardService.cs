using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// Implemented by class card.cs
    /// Several validity checks are involved when selecting a card payment.
    /// Also it is checked, whether the system recognizes the card type and 
    /// can figure out which tendertype it belongs to.
    /// 
    /// The class \SystemFramework\ApplicationServices.cs calls the constructor with the following parameters:
    /// <code>
    /// cardParameters[0] = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;  // SqlConnection
    /// cardParameters[1] = LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID;// string
    /// cardParameters[2] = LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreId;  // string 
    /// </code>
    /// 
    /// </summary>
    public interface ICardService : IService
    {
        /// <summary>
        /// In the current implementation, this method includes another method that first retrieves the data of all cards available.
        /// Then it is for each retrieved card type it is tested whether the given cardnumber (with the length as stored in the database)
        /// is within the range:
        /// <code>foreach (CardInfo cardInfo in cardTypes)(...)</code>
        /// If a valid number range has been found, the card number, the track2 data and the entering method are assigned to a CardInfo object 
        /// i.e. 'correctCardType'.
        /// \POSProcesses\Operations\PayCard.Execute() will evaluate, whether it is an international credit cart, a debit card, 
        /// a loyality card or a corporate card. An instance of PayCorporateCard is created and it's method RunOperation() called.
        /// (Further background: PayCard.cs is derived from TenderOperation.cs which is derived from Operation.cs.)
        /// \POSProcesses\Operations\PayCorporateCard.Execute() calls then the interface method of 
        /// \ServiceInterfaces\CorporateCardInterface\ICorporateCard.cs.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardInfo">If not null, it contains all data, i.e. card number, card name, checkings data etc. </param>
        CardInfo GetCardType(IConnectionManager entry, CardInfo cardInfo);

        /// <summary>
        /// First of the checks to determine whether entered cardnumber is of a valid length. 
        /// Length is an estimate (i.e. between 11 and 16 characters).
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardNumber">The cardNumber attached to the cardInfo instance, created from class frmPayCard. </param>
        /// <returns>True when the length of the card number is within the given limits.</returns>
        bool IsCardLengthValid(IConnectionManager entry, string cardNumber);

        /// <summary>
        /// Check for date validity.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="expirationDate"></param>
        /// <returns>True when the expiry date has not yet passed.</returns>
        bool IsExpiryDateValid(IConnectionManager entry, string expirationDate);

        /// <summary>
        /// Returns an array of all the card types.
        /// </summary>        
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of all card types.</returns>
        List<CardInfo> GetAllCardTypes(IConnectionManager entry);

    }
}
