using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Tax
{
    /// <summary>
    /// Data provider class for tax code values
    /// </summary>
    public class TaxCodeValueData : SqlServerDataProviderBase, ITaxCodeValueData
    {
        private static void PopulateTaxCodeValue(IDataReader dr, TaxCodeValue line)
        {
            line.ID = (Guid)dr["ID"];
            line.TaxCode = (string)dr["TAXCODE"];
            line.FromDate = Date.FromAxaptaDate(dr["TAXFROMDATE"]);
            line.ToDate = Date.FromAxaptaDate(dr["TAXTODATE"]);
            line.Value = (decimal)dr["TAXVALUE"];
        }

        private static string ResolveSort(TaxCodeValue.SortEnum sortEnum, bool backwards)
        {
            string sortString = "";

            switch (sortEnum)
            {
                case TaxCodeValue.SortEnum.FromDate:
                    sortString = " Order by g.TAXFROMDATE ASC";
                    break;
                case TaxCodeValue.SortEnum.ToDate:
                    sortString = " Order by g.TAXTODATE ASC";
                    break;
                case TaxCodeValue.SortEnum.Value:
                    sortString = " Order by g.TAXVALUE ASC";
                    break;
            }

            if (backwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        /// <summary>
        /// Gets a tax code value by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeValueID">ID of the tax code value</param>
        /// <param name="cacheType">Cache type to use. Default is none</param>
        /// <returns>A tax code value with a given ID</returns>
        public virtual TaxCodeValue Get(IConnectionManager entry, RecordIdentifier taxCodeValueID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select g.ID, g.TAXCODE,TAXFROMDATE,TAXTODATE," +
                    "ISNULL(TAXVALUE,0.0) as TAXVALUE " +
                    "from TAXDATA g " +
                    "where g.ID = @ID and g.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "ID", (Guid)taxCodeValueID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Get<TaxCodeValue>(entry, cmd, taxCodeValueID, PopulateTaxCodeValue, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual List<TaxCodeValue> GetList(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select g.ID, g.TAXCODE,TAXFROMDATE,TAXTODATE," +
                    "ISNULL(TAXVALUE,0.0) as TAXVALUE " +
                    "from TAXDATA g " +
                    "where g.DATAAREAID = @dataAreaId";
                
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<TaxCodeValue>(entry, cmd, CommandType.Text, PopulateTaxCodeValue);
            }
        }

        /// <summary>
        /// Gets a list of tax code values for a given tax code ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeID">ID of the tax code to get tax values for</param>
        /// <param name="sortEnum">Determines the sort ordering of the results</param>
        /// <param name="backwardsSort">Determines if the results will be in reversed ordering</param>
        /// <returns>A list of tax code values for a given tax code ID</returns>
        public virtual List<TaxCodeValue> GetTaxCodeValues(IConnectionManager entry, RecordIdentifier taxCodeID, TaxCodeValue.SortEnum sortEnum, bool backwardsSort)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select g.ID, g.TAXCODE,TAXFROMDATE,TAXTODATE," +
                    "ISNULL(TAXVALUE,0.0) as TAXVALUE " +
                    "from TAXDATA g " +
                    "where g.TAXCODE = @taxCode and g.DATAAREAID = @dataAreaId" + ResolveSort(sortEnum, backwardsSort);

                MakeParam(cmd, "taxCode", (string)taxCodeID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<TaxCodeValue>(entry, cmd, CommandType.Text, PopulateTaxCodeValue);
            }
        }

        /// <summary>
        /// Checks if a tax code value for a given tax code ID exists that intersects with the given time range
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeID">The tax code ID</param>
        /// <param name="fromDate">Start of time range</param>
        /// <param name="toDate">End of time range</param>
        /// <returns>Wether a tax code value for the tax code ID exists that intersects with the given time range</returns>
        public virtual bool RangeExists(IConnectionManager entry,RecordIdentifier taxCodeID, Date fromDate, Date toDate)
        {
            IDataReader dr = null;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select 'x' from TAXDATA " +
                    "where ( " +
                    //-- Both fields in the database are open end
                    "(TAXFROMDATE = '1900-01-01 00:00:00.000' and TAXTODATE = '1900-01-01 00:00:00.000') or " +
                    //-- Later field in the database is open end
                    "(TAXFROMDATE <> '1900-01-01 00:00:00.000' and TAXTODATE = '1900-01-01 00:00:00.000') and (@fromDate >= TAXFROMDATE or @toDate >= TAXFROMDATE) or " +
                    //-- Neither  fields are open end
                    "(TAXFROMDATE <> '1900-01-01 00:00:00.000' and TAXTODATE <> '1900-01-01 00:00:00.000' and " +
                        "((@fromDate >= TAXFROMDATE and @fromDate <= TAXTODATE) or " +
                        "(@toDate >= TAXFROMDATE and @toDate <= TAXTODATE) or " +
                        "(TAXFROMDATE >= @fromDate and TAXFROMDATE <= @toDate) " + 
                    "))" +
                    ") and TAXCODE = @taxCode and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "taxCode", (string)taxCodeID);
                MakeParam(cmd, "fromDate", fromDate.ToAxaptaSQLDate(),SqlDbType.DateTime);
                MakeParam(cmd, "toDate", toDate.ToAxaptaSQLDate(), SqlDbType.DateTime);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        return true;
                    }
                }
                finally
                {
                    if(dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a tax code value for a given tax code ID exists that intersects with the given time range. Excludes a single tax code value.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="ignoredTaxCodeLine">The tax code value to ignore</param>
        /// <param name="taxCodeID">The tax code ID</param>
        /// <param name="fromDate">Start of time range</param>
        /// <param name="toDate">End of time range</param>
        /// <returns>Wether a tax code value for the tax code ID exists that intersects with the given time range. Excludes a single tax code value</returns>
        public virtual bool RangeExists(IConnectionManager entry, TaxCodeValue ignoredTaxCodeLine, RecordIdentifier taxCodeID, Date fromDate, Date toDate)
        {
            IDataReader dr = null;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select 'x' from TAXDATA " +
                    "where ( " +
                    //-- Both fields in the database are open end
                    "(TAXFROMDATE = '1900-01-01 00:00:00.000' and TAXTODATE = '1900-01-01 00:00:00.000') or " +
                    //-- Later field in the database is open end
                    "(TAXFROMDATE <> '1900-01-01 00:00:00.000' and TAXTODATE = '1900-01-01 00:00:00.000') and (@fromDate >= TAXFROMDATE or @toDate >= TAXFROMDATE) or " +
                    //-- Neither  fields are open end
                    "(TAXFROMDATE <> '1900-01-01 00:00:00.000' and TAXTODATE <> '1900-01-01 00:00:00.000' and " +
                        "((@fromDate >= TAXFROMDATE and @fromDate <= TAXTODATE) or " +
                        "(@toDate >= TAXFROMDATE and @toDate <= TAXTODATE) or " +
                        "(TAXFROMDATE >= @fromDate and TAXFROMDATE <= @toDate) " +
                    "))" +
                    ") and TAXCODE = @taxCode and DATAAREAID = @dataAreaId and not " +
                    "(TAXCODE = @taxCode and TAXFROMDATE = @oldFromDate and TAXTODATE = @oldToDate)";

                MakeParam(cmd, "taxCode", (string)taxCodeID);
                MakeParam(cmd, "fromDate", fromDate.ToAxaptaSQLDate(), SqlDbType.DateTime);
                MakeParam(cmd, "toDate", toDate.ToAxaptaSQLDate(), SqlDbType.DateTime);
                MakeParam(cmd, "oldFromDate", ignoredTaxCodeLine.FromDate.ToAxaptaSQLDate(), SqlDbType.DateTime);
                MakeParam(cmd, "oldToDate", ignoredTaxCodeLine.ToDate.ToAxaptaSQLDate(), SqlDbType.DateTime);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        return true;
                    }
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

            return false;
        }

        /// <summary>
        /// Wether a tax code value with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeValueID">ID of the tax code value</param>
        /// <returns>Wether a tax code value with a given ID exists in the database</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier taxCodeValueID)
        {
            return RecordExists<TaxCodeValue>(entry, "TAXDATA", "ID", taxCodeValueID);
        }

        /// <summary>
        /// Saves a given tax code value into the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeValue">The tax code value to save</param>
        public virtual void Save(IConnectionManager entry, TaxCodeValue taxCodeValue)
        {
            var statement = new SqlServerStatement("TAXDATA");
            bool isNew = false;

            ValidateSecurity(entry, BusinessObjects.Permission.EditSalesTaxSetup);

            if (taxCodeValue.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                taxCodeValue.ID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, taxCodeValue.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)taxCodeValue.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)taxCodeValue.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("TAXCODE", (string)taxCodeValue.TaxCode);
            statement.AddField("TAXFROMDATE", taxCodeValue.FromDate.ToAxaptaSQLDate().Date, SqlDbType.DateTime);
            statement.AddField("TAXTODATE", taxCodeValue.ToDate.ToAxaptaSQLDate().Date, SqlDbType.DateTime);
            statement.AddField("TAXVALUE", taxCodeValue.Value, SqlDbType.Decimal);

            Save(entry, taxCodeValue, statement);

            if (!isNew)
            {
                // Updating the tax code data means that we have to invalidate the entire cache
                entry.Cache.InvalidateEntityCache();
            }
        }
        
        /// <summary>
        /// Deletes a tax code value with a given ID from the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeValueID">ID of the tax code value</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier taxCodeValueID)
        {
            DeleteRecord<TaxCodeValue>(entry, "TAXDATA", "ID", taxCodeValueID, BusinessObjects.Permission.EditSalesTaxSetup);

            // Deletion of tax data means that we have to invalidate the entire cache
            entry.Cache.InvalidateEntityCache();
        }    
    }
}
