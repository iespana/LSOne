using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.StoreManagement
{
    public interface IStoreCardTypesData : IDataProviderBase<StorePaymentTypeCardType>
    {
        List<StorePaymentTypeCardType> GetList(IConnectionManager entry, RecordIdentifier storeID, StoreCardTypeSorting sortBy = StoreCardTypeSorting.Name, bool sortBackwards = false);

        /// <summary>
        /// Gets the store payment type card for the given card number. This will search for the correct store card type for
        /// a given card number, where the card number is represented by the first four digits in a credit card number.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardNumber">The first four digits in a card number</param>
        /// <returns></returns>
        StorePaymentTypeCardType GetCardTypeByCardNumber(IConnectionManager entry, string cardNumber);

        /// <summary>
        /// Gets all card types for a given store and payment type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="paymentMethodID">The ID of the payment type</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        List<StorePaymentTypeCardType> GetList(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier paymentMethodID, StoreCardTypeSorting sortBy = StoreCardTypeSorting.Name, bool sortBackwards = false);
    }
}