using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.StoreManagement
{
    public class StoreCardTypesData : SqlServerDataProviderBase, IStoreCardTypesData
    {
        private string ResolveSort(StoreCardTypeSorting sortEnum, bool backwards)
        {
            string sort = "";

            switch (sortEnum)
            {
                case StoreCardTypeSorting.ID:
                    sort = "CARDTYPEID";
                    break;

                case StoreCardTypeSorting.Name:
                    sort = "NAME";
                    break;
            }

            return sort + " " + (backwards ? "DESC" : "ASC");
        }

        private static void PopulateCardType(IDataReader reader, StorePaymentTypeCardType cardType)
        {
            cardType.ID = (string)reader["TENDERTYPEID"];
            cardType.Text = (string)reader["NAME"];
            cardType.CardTypeID = (string)reader["CARDTYPEID"];
        }

        public virtual List<StorePaymentTypeCardType> GetList(IConnectionManager entry, RecordIdentifier storeID, StoreCardTypeSorting sortBy = StoreCardTypeSorting.Name, bool sortBackwards = false)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"select TENDERTYPEID, ISNULL(NAME, '') as NAME, CARDTYPEID
                      from RBOSTORETENDERTYPECARDTABLE
                      where STOREID = @storeId and DATAAREAID = @dataAreaId
                      order by " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "storeId", (string)storeID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<StorePaymentTypeCardType>(entry, cmd, CommandType.Text, PopulateCardType);
            }
        }

        /// <summary>
        /// Gets all card types for a given store and payment type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="paymentMethodID">The ID of the payment type</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        public virtual List<StorePaymentTypeCardType> GetList(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier paymentMethodID, StoreCardTypeSorting sortBy = StoreCardTypeSorting.Name, bool sortBackwards = false)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"select TENDERTYPEID, ISNULL(NAME, '') as NAME, CARDTYPEID
                      from RBOSTORETENDERTYPECARDTABLE
                      where STOREID = @storeId and TENDERTYPEID = @tenderTypeId and DATAAREAID = @dataAreaId
                      order by " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "storeId", (string)storeID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "tenderTypeId", paymentMethodID);

                return Execute<StorePaymentTypeCardType>(entry, cmd, CommandType.Text, PopulateCardType);
            }
        }
       

        /// <summary>
        /// Gets the store payment type card for the given card number. This will search for the correct store card type for
        /// a given card number, where the card number is represented by the first four digits in a credit card number.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardNumber">The first four digits in a card number</param>
        /// <returns></returns>
        public virtual StorePaymentTypeCardType GetCardTypeByCardNumber(IConnectionManager entry, string cardNumber)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"select r.TENDERTYPEID, ISNULL(r.NAME, '') as NAME, r.CARDTYPEID
                      from RBOSTORETENDERTYPECARDTABLE r
                      join RBOTENDERTYPECARDNUMBERS c on c.CARDTYPEID = r.CARDTYPEID and c.DATAAREAID = r.DATAAREAID
                      where @cardNumber >= c.CARDNUMBERFROM and @cardNumber <= c.CARDNUMBERTO and r.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "cardNumber", cardNumber);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var results = Execute<StorePaymentTypeCardType>(entry, cmd, CommandType.Text, PopulateCardType);

                return results.Count > 0 ? results[0] : null;
            }
        }
    }
}
