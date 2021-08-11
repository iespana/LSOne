using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;

namespace LSOne.DataLayer.SqlDataProviders.PricesAndDiscounts
{
    public class DiscountCalculationData: SqlServerDataProviderBase, IDiscountCalculationData
    {
        private static void PopulateDiscountCalculation(IDataReader dr, DiscountCalculation discountCalculation)
        {
            discountCalculation.DiscountsToApply = (LineDiscCalculationTypes)(int)dr["DISC"];
            discountCalculation.CalculateCustomerDiscounts = (CalculateCustomerDiscountEnums)(int)dr["CALCCUSTOMERDISCS"];
            discountCalculation.CalculatePeriodicDiscounts = (CalculatePeriodicDiscountEnums)(int)dr["CALCPERIODICDISCS"];
            discountCalculation.ClearPeriodicDiscountAfterMinutes = (int)dr["CLEARPERIODICDISCOUNTCACHEMINUTES"];
            discountCalculation.ClearPeriodicDiscountCache = (ClearPeriodicDiscountCacheEnum)(int)dr["CLEARPERIODICDISCOUNTCACHE"];
        }

        public virtual DiscountCalculation Get(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @" SELECT ISNULL(DISC, 0) DISC, ISNULL(CALCPERIODICDISCS, 0) CALCPERIODICDISCS, ISNULL(CALCCUSTOMERDISCS, 0) CALCCUSTOMERDISCS, 
                              ISNULL(CLEARPERIODICDISCOUNTCACHE, 0) CLEARPERIODICDISCOUNTCACHE, 
                              ISNULL(CLEARPERIODICDISCOUNTCACHEMINUTES, 30) CLEARPERIODICDISCOUNTCACHEMINUTES
                       FROM SALESPARAMETERS 
                       WHERE DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var result = Execute<DiscountCalculation>(entry, cmd, CommandType.Text, PopulateDiscountCalculation);
                return (result.Count > 0) ? result[0] : new DiscountCalculation();
            }
        }

        public virtual bool Exists(IConnectionManager entry)
        {
            return RecordExists(entry, "SALESPARAMETERS", "KEY_", 0);
        }

        public virtual void Save(IConnectionManager entry, DiscountCalculation discountCalculation)
        {
            var statement = new SqlServerStatement("SALESPARAMETERS");

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

            statement.AddField("DISC", (int)discountCalculation.DiscountsToApply, SqlDbType.Int);
            statement.AddField("CALCPERIODICDISCS", (int)discountCalculation.CalculatePeriodicDiscounts, SqlDbType.Int);
            statement.AddField("CALCCUSTOMERDISCS", (int)discountCalculation.CalculateCustomerDiscounts, SqlDbType.Int);
            statement.AddField("CLEARPERIODICDISCOUNTCACHE", (int)discountCalculation.ClearPeriodicDiscountCache, SqlDbType.Int);
            statement.AddField("CLEARPERIODICDISCOUNTCACHEMINUTES", discountCalculation.ClearPeriodicDiscountAfterMinutes, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
