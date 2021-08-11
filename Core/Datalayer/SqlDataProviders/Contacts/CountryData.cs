using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders.Contacts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Contacts
{
    /// <summary>
    /// A data provider class for countries
    /// </summary>
    public class CountryData : SqlServerDataProviderBase, ICountryData
    {
        /// <summary>
        /// Gets a country ID from a country name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="countryName">The name of the country</param>
        /// <returns>The country ID if found, else a empty string</returns>
        public virtual string GetIDFromName(IConnectionManager entry,string countryName)
        {
            ValidateSecurity(entry);

            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select COUNTRYID from COUNTRY where NAME = @name and DATAAREAID = @dataAreaID";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "name", countryName.ToUpperInvariant());

                object value = entry.Connection.ExecuteScalar(cmd);

                return (value == DBNull.Value) ? "" : (string)value;
            }
        }

        public string GetNameFromID(IConnectionManager entry, RecordIdentifier coutryID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select NAME from COUNTRY where COUNTRYID = @id and DATAAREAID = @dataAreaID";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", coutryID.StringValue);

                object value = entry.Connection.ExecuteScalar(cmd);

                return (value == DBNull.Value) ? "" : (string)value;
            }
        }
    }
}
