using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Vouchers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGiftCardData : IDataProvider<GiftCard>, ISequenceable
    {
        /// <summary>
        /// Searches for gift cards
        /// </summary>
        /// <param name="entry">Entry info</param>
        /// <param name="itemCount">Number of items found</param>
        /// <param name="filter">Search filter</param>
        /// <returns>List of found gift cards</returns>
        List<GiftCard> AdvancedSearch(IConnectionManager entry, GiftCardFilter filter, out int itemCount);

        /// <summary>
        /// Adds a given amount to a gift card with a given id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardID">ID of the gift card to add to</param>
        /// <param name="amount">The amount to add to the gift card</param>
        /// <param name="storeID">ID of the store where the operation is done or empty string if head office</param>
        /// <param name="terminalID">ID of the terminal where the operation is done or empty string if on store or headoffice level</param>
        /// <param name="userID">ID of the Site Manager user, or Guid.Empty if not avalible</param>
        /// <param name="staffID">ID of the POS staff or empty string if not avalible</param>
        /// <returns>Balance on the gift card after the transaction</returns>
        decimal AddToGiftCard(IConnectionManager entry, RecordIdentifier giftCardID, decimal amount,
            RecordIdentifier storeID,
            RecordIdentifier terminalID, RecordIdentifier userID, RecordIdentifier staffID);

        /// <summary>
        /// Activates a gift card
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardID">ID of the giftcard</param>
        /// <param name="storeID">ID of the store where the operation is done or empty string if head office</param>
        /// <param name="terminalID">ID of the terminal where the operation is done or empty string if on store or headoffice level</param>
        /// <param name="userID">ID of the Site Manager user, or Guid.Empty if not avalible</param>
        /// <param name="staffID">ID of the POS staff or empty string if not avalible</param>
        /// <param name="transactionID">The ID of the transaction if there is any</param>
        /// /// <param name="receiptID">The receipt ID of the transaction if there is any</param>
        /// <returns></returns>
        bool Activate(IConnectionManager entry, RecordIdentifier giftCardID, RecordIdentifier storeID,
            RecordIdentifier terminalID,
            RecordIdentifier userID, RecordIdentifier staffID, RecordIdentifier transactionID,
            RecordIdentifier receiptID);

        /// <summary>
        /// Activates a gift card
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardID">ID of the giftcard</param>
        /// <returns></returns>
        bool MarkIssued(IConnectionManager entry, RecordIdentifier giftCardID);

        /// <summary>
        /// Deactivates a gift card
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardID">ID of the giftcard</param>
        /// <param name="storeID">ID of the store where the operation is done or empty string if head office</param>
        /// <param name="terminalID">ID of the terminal where the operation is done or empty string if on store or headoffice level</param>
        /// <param name="userID">ID of the Site Manager user, or Guid.Empty if not avalible</param>
        /// <param name="staffID">ID of the POS staff or empty string if not avalible</param>
        /// <param name="transactionNumber">The ID of the transaction if there is any</param>
        /// <returns></returns>
        bool Deactivate(IConnectionManager entry, RecordIdentifier giftCardID, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier userID, RecordIdentifier staffID, RecordIdentifier transactionNumber);

        /// <summary>
        /// Gets a <see cref="GiftCard"/> with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="giftCardID">The ID of the gift card to get</param>
        /// <returns></returns>
        GiftCard Get(IConnectionManager entry, RecordIdentifier giftCardID);

        /// <summary>
        /// Saves a gift card, creating new one if the ID was empty or record did not exist, else updates.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCard">The gift card to save</param>
        /// <param name="prefix">Prefix for the ID added for barcode usage</param>
        /// <param name="numberSequenceLowest">The point where you want your number sequence start</param>
        void Save(IConnectionManager entry, GiftCard giftCard, string prefix, int? numberSequenceLowest = null);

    }
}