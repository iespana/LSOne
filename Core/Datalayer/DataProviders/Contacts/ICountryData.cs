using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Contacts
{
    public interface ICountryData : IDataProviderBase<DataEntity>
    {
        /// <summary>
        /// Gets a country ID from a country name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="countryName">The name of the country</param>
        /// <returns>The country ID if found, else a empty string</returns>
        string GetIDFromName(IConnectionManager entry, string countryName);

        /// <summary>
        /// Gets a country name from a country ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name=")">The ID of the country</param>
        /// <returns>The country name if found, else a empty string</returns>
        string GetNameFromID(IConnectionManager entry, RecordIdentifier coutryID);
    }
}