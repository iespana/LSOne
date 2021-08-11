using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.PricesAndDiscounts
{
    public class DiscountParametersData: SqlServerDataProviderBase, IDiscountParametersData
    {
        private static void PopulateDiscountCalculations(IDataReader dr, DiscountParameters discountParameters)
        {
            discountParameters.PeriodicLine = (PeriodicLineEnum)dr["PERIODICLINE"];
            discountParameters.PeriodicTotal = (PeriodicTotalEnum)dr["PERIODICTOTAL"];
            discountParameters.LineTotal = (CustomerLineTotalEnum)dr["LINETOTAL"];
        }

        public virtual DiscountParameters Get(IConnectionManager entry, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select ISNULL(PERIODICLINE, 0) PERIODICLINE, ISNULL(PERIODICTOTAL, 0) PERIODICTOTAL, ISNULL(LINETOTAL, 0) LINETOTAL " +
                    "from DISCOUNTPARAMETERS " +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var result = Get<DiscountParameters>(entry, cmd, 0, PopulateDiscountCalculations, cacheType, UsageIntentEnum.Normal);
                return result ?? new DiscountParameters();
            }
        }

        public virtual bool Exists(IConnectionManager entry)
        {
            return RecordExists(entry, "DISCOUNTPARAMETERS", "KEY_", 0);
        }

        public virtual void Save(IConnectionManager entry, DiscountParameters discountParameters)
        {
            var statement = new SqlServerStatement("DISCOUNTPARAMETERS");

            ValidateSecurity(entry, BusinessObjects.Permission.TerminalEdit);

            if (!Exists(entry))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddField("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddField("KEY_", 0, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("KEY_", 0, SqlDbType.Int);
            }

            statement.AddField("PERIODICLINE", (int)discountParameters.PeriodicLine, SqlDbType.Int);
            statement.AddField("PERIODICTOTAL", (int)discountParameters.PeriodicTotal, SqlDbType.Int);
            statement.AddField("LINETOTAL", (int)discountParameters.LineTotal, SqlDbType.Int);               

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
