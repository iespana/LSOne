using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders.Settings;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Settings
{
    public class DecimalSettingsData : SqlServerDataProviderBase, IDecimalSettingsData
    {
        private static void PopulateDecimal(IDataReader dr, DecimalSetting setting)
        {
            setting.ID = (string)dr["ID"];
            setting.Text = (string)dr["NAME"];

            setting.Min = (int)dr["MINDECIMALS"];
            setting.Max = (int)dr["MAXDECIMALS"];
        }

        public virtual DecimalSetting Get(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select ID, ISNULL(NAME,0) as NAME,ISNULL(MINDECIMALS,0) as MINDECIMALS,ISNULL(MAXDECIMALS,0) as MAXDECIMALS " +
                    "from DECIMALSETTINGS " +
                    "where DATAAREAID = @dataAreaId and ID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                var result = Execute<DecimalSetting>(entry, cmd, CommandType.Text, PopulateDecimal);
                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual List<DecimalSetting> Get(IConnectionManager entry, int sortColumn, bool backwardsSort)
        {
            string sort;

            ValidateSecurity(entry);

            switch (sortColumn)
            {
                case 0:
                    sort = "ID ASC";
                    break;

                case 1:
                    sort = "NAME ASC";
                    break;

                case 2:
                    sort = "MINDECIMALS ASC";
                    break;

                case 3:
                    sort = "MAXDECIMALS ASC";
                    break;

                default:
                    sort = "";
                    break;
            }

            if (sort != "")
            {
                if (backwardsSort)
                {
                    sort = sort.Replace("ASC", "DESC");
                }

                sort = " order by " + sort;
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select ID, ISNULL(NAME,0) as NAME,ISNULL(MINDECIMALS,0) as MINDECIMALS,ISNULL(MAXDECIMALS,0) as MAXDECIMALS " +
                    "from DECIMALSETTINGS " +
                    "where DATAAREAID = @dataAreaId " + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<DecimalSetting>(entry, cmd, CommandType.Text, PopulateDecimal);
            }
        }

        public virtual void Save(IConnectionManager entry, DecimalSetting decimalSetting)
        {
            var statement = new SqlServerStatement("DECIMALSETTINGS");

            ValidateSecurity(entry, BusinessObjects.Permission.AdministrationMaintainSettings);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("ID", (string)decimalSetting.ID);

            statement.AddField("NAME", decimalSetting.Text);
            statement.AddField("MINDECIMALS", decimalSetting.Min, SqlDbType.Int);
            statement.AddField("MAXDECIMALS", decimalSetting.Max, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);

            entry.Cache.InvalidateSystemDecimalCache();
        }
    }
}
