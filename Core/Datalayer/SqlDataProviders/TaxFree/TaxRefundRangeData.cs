using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.DataProviders.TaxFree;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlDataProviders.BarCodes;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.TaxFree
{
    public class TaxRefundRangeData : SqlServerDataProviderBase, ITaxRefundRangeData
    {
        private string BaseSql
        {
            get
            {
                return "SELECT ID, ISNULL(VALUEFROM, 0) AS VALUEFROM, ISNULL(VALUETO, 0) AS VALUETO, " +
                       "ISNULL(TAXVALUE, 0) AS TAXVALUE, ISNULL(TAXREFUND, 0) AS TAXREFUND, ISNULL(TAXREFUNDPERCENTAGE, 0) AS TAXREFUNDPERCENTAGE " +
                       "FROM TAXREFUNDRANGE ";
            }
        }
        protected virtual void Populate(IDataReader dr, TaxRefundRange item)
        {
            item.ID = (Guid) dr["ID"];
            item.ValueFrom = (decimal) dr["VALUEFROM"];
            item.ValueTo = (decimal) dr["VALUETO"];
            item.TaxValue = (decimal) dr["TAXVALUE"];
            item.TaxRefund = (decimal)dr["TAXREFUND"];
            item.TaxRefundPercentage = (decimal)dr["TAXREFUNDPERCENTAGE"];
        }
        
        public virtual TaxRefundRange Get(IConnectionManager entry, RecordIdentifier ID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + "WHERE ID = @id";
                MakeParam(cmd, "id", ID.DBValue, ID.DBType);
                return Get<TaxRefundRange>(entry, cmd, ID, Populate, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual TaxRefundRange GetForValue(IConnectionManager entry, decimal value, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + "WHERE VALUEFROM <= @value AND VALUETO >= @value";
                MakeParam(cmd, "value", value, SqlDbType.Decimal);
                return Get<TaxRefundRange>(entry, cmd, new RecordIdentifier("TaxRefundRange.GetForValue", value), Populate, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual TaxRefundRange GetForValue(IConnectionManager entry, decimal value, decimal taxvalue, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + "WHERE VALUEFROM <= @value AND VALUETO >= @value AND TAXVALUE = @taxvalue";
                MakeParam(cmd, "value", value, SqlDbType.Decimal);
                MakeParam(cmd, "taxvalue", taxvalue, SqlDbType.Decimal);
                return Get<TaxRefundRange>(entry, cmd, new RecordIdentifier("TaxRefundRange.GetForValue", value), Populate, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual List<TaxRefundRange> GetAll(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql;
                return Execute<TaxRefundRange>(entry, cmd, CommandType.Text, Populate);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select ID FROM TAXREFUNDRANGE WHERE ID = @id";

                MakeParam(cmd, "id", ID.DBValue, ID.DBType);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    return dr.Read();
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }
        }

        public virtual bool Exists(IConnectionManager entry, decimal valueFrom, decimal valueTo)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select ID FROM TAXREFUNDRANGE " +
                                  "WHERE (VALUEFROM >= @valueFrom AND VALUETO <= @valueFROM) OR (VALUEFROM >= @valueTo AND VALUETO <= @valueTo)";

                MakeParam(cmd, "valueFrom", valueFrom, SqlDbType.Decimal);
                MakeParam(cmd, "valueTo", valueTo, SqlDbType.Decimal);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    return dr.Read();
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            ValidateSecurity(entry);

            entry.Cache.DeleteEntityFromCache(typeof(TaxRefundRange), ID);

            var statement = entry.Connection.CreateStatement("TAXREFUNDRANGE", StatementType.Delete);

            statement.AddCondition("ID", ID.DBValue, ID.DBType);
            
            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void Save(IConnectionManager entry, TaxRefundRange item)
        {
            ValidateSecurity(entry);
            var statement = entry.Connection.CreateStatement("TAXREFUNDRANGE");
            bool isNew = false;
            if (item.ID == null || item.ID.IsEmpty)
            {
                isNew = true;
                item.ID = Guid.NewGuid();
            }
            if (isNew || !Exists(entry, item.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", item.ID.DBValue, item.ID.DBType);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", item.ID.DBValue, item.ID.DBType);
            }

            statement.AddField("VALUEFROM", item.ValueFrom, SqlDbType.Decimal);
            statement.AddField("VALUETO", item.ValueTo, SqlDbType.Decimal);
            statement.AddField("TAXVALUE", item.TaxValue, SqlDbType.Decimal);
            statement.AddField("TAXREFUND", item.TaxRefund, SqlDbType.Decimal);
            statement.AddField("TAXREFUNDPERCENTAGE", item.TaxRefundPercentage, SqlDbType.Decimal);

            Save(entry, item, statement);
        }
    }
}
