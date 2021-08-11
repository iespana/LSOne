using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlConnector.DataProviders
{
	internal class SqlServerCacheProvider : SqlServerDataProviderBase, ICacheProvider
	{
		public Dictionary<string, DecimalLimit> GetSystemDecimals(IConnectionManager entry)
		{
			var decimals = new Dictionary<string, DecimalLimit>();
			
			ValidateSecurity(entry);

			IDataReader dr = null;
			try
			{
				using (var cmd = entry.Connection.CreateCommand())
				{
					cmd.CommandText = "Select ID,ISNULL(NAME,'') as NAME," +
						"ISNULL(MINDECIMALS,0) as MINDECIMALS,ISNULL(MAXDECIMALS,0) as MAXDECIMALS " +
						"from DECIMALSETTINGS where DATAAREAID = @dataAreaID";

					MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

					dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

					while (dr.Read())
					{
						var limit = new DecimalLimit((int)dr["MINDECIMALS"], (int)dr["MAXDECIMALS"]);

						decimals.Add((string)dr["ID"], limit);
					}
				}
			}
			finally
			{
				if (dr != null)
				{
					dr.Close();
				}
			}

			return decimals;
		}

		public List<string> GetNamePrefixes(IConnectionManager entry)
		{
			List<string> namePrefixes = new List<string>();

			if(System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "en")
			{
				namePrefixes.Add(GenericConnector.Properties.Resources.Dr);
				namePrefixes.Add(GenericConnector.Properties.Resources.Miss);
				namePrefixes.Add(GenericConnector.Properties.Resources.Mr);
				namePrefixes.Add(GenericConnector.Properties.Resources.Mrs);
				namePrefixes.Add(GenericConnector.Properties.Resources.Ms);
				namePrefixes.Add(GenericConnector.Properties.Resources.Rev);
			}
			else
			{
				//ONE-5732 - Add customer titles only if they are translated.
				ResourceManager manager = new ResourceManager(typeof(GenericConnector.Properties.Resources));

				string drPrefix = GetNamePrefixLocalization("Dr", manager);
				string missPrefix = GetNamePrefixLocalization("Miss", manager);
				string mrPrefix = GetNamePrefixLocalization("Mr", manager);
				string mrsPrefix = GetNamePrefixLocalization("Mrs", manager);
				string msPrefix = GetNamePrefixLocalization("Ms", manager);
				string revPrefix = GetNamePrefixLocalization("Rev", manager);

				if (drPrefix != string.Empty)
					namePrefixes.Add(drPrefix);
				if (missPrefix != string.Empty)
					namePrefixes.Add(missPrefix);
				if (mrPrefix != string.Empty)
					namePrefixes.Add(mrPrefix);
				if (mrsPrefix != string.Empty)
					namePrefixes.Add(mrsPrefix);
				if (msPrefix != string.Empty)
					namePrefixes.Add(msPrefix);
				if (revPrefix != string.Empty)
					namePrefixes.Add(revPrefix);
			}

			return namePrefixes;
		}

		private string GetNamePrefixLocalization(string key, ResourceManager manager)
		{
			string localizedString = manager.GetString(key, System.Globalization.CultureInfo.CurrentUICulture);
			string defaultString = manager.GetString(key, System.Globalization.CultureInfo.InvariantCulture);
			return localizedString == defaultString ? string.Empty : localizedString;
		}

		public List<Country> GetCountries(IConnectionManager entry)
		{
			var cmd = entry.Connection.CreateCommand();

			ValidateSecurity(entry);

			cmd.CommandText = @"Select NAME, COUNTRYID from COUNTRY where DATAAREAID = @dataAreaId order by NAME";
			MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

			return Execute<Country>(entry, cmd, CommandType.Text, "NAME", "COUNTRYID");
		}

		/// <summary>
		/// Returns the states/provinces of the given country.
		/// </summary>
		/// <param name="entry">Database connection</param>
		/// <param name="countryID">Country ID</param>
		/// <returns></returns>
		public List<IDataEntity> GetStates(IConnectionManager entry, RecordIdentifier countryID)
		{
			var cmd = entry.Connection.CreateCommand();

			ValidateSecurity(entry);

			cmd.CommandText = @"Select ABBREVIATEDNAME, NAME 
								from STATES 
								where DATAAREAID = @dataAreaId AND COUNTRYID = @countryId 
								order by NAME";
			MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
			MakeParam(cmd, "countryId", (string)countryID);

			return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "ABBREVIATEDNAME").Cast<IDataEntity>().ToList();
		}
	}
}
