using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Loyalty
{
	public class LoyaltyCustomerParamsData : SqlServerDataProviderBase, ILoyaltyCustomerParamsData
	{
		private static string BaseSelectString
		{
			get
			{
				return
					"SELECT [KEY_]"
					+ ", ISNULL([DATASOURCE], 0) DATASOURCE"
					+ ", ISNULL([FILTERSTORE], '') FILTERSTORE"
					+ ", ISNULL([FILTERTERMINAL], '') FILTERTERMINAL"
					+ ", ISNULL([FILTERLOYALTYSCHEME], '') FILTERLOYALTYSCHEME"
                    + ", ISNULL([OMNILOYALTYSCHEME], '') OMNILOYALTYSCHEME"
                    + ", ISNULL([OMNILOYALTYPOINTS], 0) OMNILOYALTYPOINTS"
					+ " FROM CUSTLOYALTYPARAMETERS ";
			}
		}

		private static void PopulateLoyaltyCustomerParams(IDataReader dr, LoyaltyCustomerParams loyaltyCustomerParams)
		{
			loyaltyCustomerParams.Key = dr["KEY_"].ToString();
			loyaltyCustomerParams.DefaultStore = (string)dr["FILTERSTORE"];
			loyaltyCustomerParams.DefaultTerminal = (string)dr["FILTERTERMINAL"];
			loyaltyCustomerParams.DefaultLoyaltyScheme = (string)dr["FILTERLOYALTYSCHEME"];
            loyaltyCustomerParams.OmniLoyaltyScheme = (string)dr["OMNILOYALTYSCHEME"];
            loyaltyCustomerParams.OmniLoyaltyPoints = (int)dr["OMNILOYALTYPOINTS"];
		}

		/// <summary>
		/// Gets the specified entry.
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="Key">The loyality parameters key.</param>
		/// <returns>An instance of <see cref="LoyaltyCustomerParams"/>.</returns>
		public virtual LoyaltyCustomerParams Get(IConnectionManager entry, RecordIdentifier Key = null)
		{
			List<LoyaltyCustomerParams> result;

			using (SqlCommand cmd = new SqlCommand())
			{
				cmd.CommandText =
					BaseSelectString +
					"where [KEY_] = @key and DATAAREAID = @dataAreaId";

				if (Key == null)
				{
					MakeParam(cmd, "key", "0");
				}
				else
				{
					MakeParam(cmd, "key", (string)Key);
				}

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				result = Execute<LoyaltyCustomerParams>(entry, cmd, CommandType.Text, PopulateLoyaltyCustomerParams);
			}

            if (result.Count <= 0)
            {
                LoyaltyCustomerParams prm = new LoyaltyCustomerParams();
                prm.Key = (Key == null ? "0" : Key);
                prm.DefaultLoyaltyScheme = "";
                prm.DefaultStore = "";
                prm.DefaultTerminal = "";

                return prm;
            }
            else
            {
                return result[0];
            }
		}

		/// <summary>
		/// Returns true if information about the given class exists in the database
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="Key">The unique Key of the parameters to check on</param>
		public virtual bool Exists(IConnectionManager entry, RecordIdentifier Key)
		{
			return RecordExists(entry, "CUSTLOYALTYPARAMETERS", new[] {"KEY_"}, Key);
		}

		/// <summary>
		/// Saves the given class to the database
		/// </summary>
		/// <param name="entry">The entry into the database</param>
        /// <param name="loyaltyCustomerParams">The parameters to be saved to the database</param>
		public virtual void Save(IConnectionManager entry, LoyaltyCustomerParams loyaltyCustomerParams)
		{
			SqlServerStatement statement = new SqlServerStatement("CUSTLOYALTYPARAMETERS");

            ValidateSecurity(entry, BusinessObjects.Permission.AdministrationMaintainSettings);

			if (!Exists(entry, loyaltyCustomerParams.Key))
			{
				statement.StatementType = StatementType.Insert;

				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddKey("KEY_", (string)loyaltyCustomerParams.Key);
			}
			else
			{
				statement.StatementType = StatementType.Update;

				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddCondition("KEY_", (string)loyaltyCustomerParams.Key);
			}

			statement.AddField("FILTERSTORE", (string)loyaltyCustomerParams.DefaultStore);
			statement.AddField("FILTERTERMINAL", (string)loyaltyCustomerParams.DefaultTerminal);
			statement.AddField("FILTERLOYALTYSCHEME", (string)loyaltyCustomerParams.DefaultLoyaltyScheme);
            statement.AddField("OMNILOYALTYSCHEME", (string)loyaltyCustomerParams.OmniLoyaltyScheme);
            statement.AddField("OMNILOYALTYPOINTS", loyaltyCustomerParams.OmniLoyaltyPoints, SqlDbType.Int);

			entry.Connection.ExecuteStatement(statement);
		}

	}
}
