using System.Collections.Generic;

using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.GenericConnector.Interfaces
{
	public interface ICacheProvider
	{
		Dictionary<string, DecimalLimit> GetSystemDecimals(IConnectionManager entry);

		List<string> GetNamePrefixes(IConnectionManager entry);

		List<Country> GetCountries(IConnectionManager entry);

		/// <summary>
		/// Returns the states/provinces of the given country.
		/// </summary>
		/// <param name="entry">Database connection</param>
		/// <param name="countryID">Country ID</param>
		/// <returns></returns>
		List<IDataEntity> GetStates(IConnectionManager entry, RecordIdentifier countryID);

	}
}
