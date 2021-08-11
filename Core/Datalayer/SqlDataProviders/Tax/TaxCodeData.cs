using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Tax
{
    /// <summary>
    /// Data provider class for tax codes
    /// </summary>
    public class TaxCodeData : SqlServerDataProviderBase, ITaxCodeData
    {
        private static string BaseSql
        {
            get
            {
                return "Select g.TAXCODE, ISNULL(g.TAXNAME,'') as TAXNAME, ISNULL(g.TAXROUNDOFF,0.0) as TAXROUNDOFF, ISNULL(g.TAXROUNDOFFTYPE,0) as TAXROUNDOFFTYPE, " +
                    "ISNULL(g.TAXBASE, 0) as TAXBASE, ISNULL(g.TAXLIMITBASE, 0) as TAXLIMITBASE, ISNULL(g.TAXCALCMETHOD, 0) as TAXCALCMETHOD, ISNULL(g.TAXONTAX, '') as TAXONTAX, ISNULL(g.TAXUNIT, '') as TAXUNIT, " +
                    "ISNULL(b.TAXNAME,'') as TAXONTAXDESCRIPTION, ISNULL(u.TXT,'') as TAXUNITDESCRIPTION, ISNULL(g.PRINTCODE, '') as PRINTCODE, ISNULL(g.TAXINCLUDEINTAX,0) as TAXINCLUDEINTAX " + 
                    "from TAXTABLE g " +
                    "left outer join TAXTABLE b on g.DATAAREAID = b.DATAAREAID and b.TAXCODE = g.TAXONTAX " +
                    "left outer join UNIT u on u.DATAAREAID = g.DATAAREAID and u.UNITID = g.TAXUNIT ";
            }
        }

        private static string ResolveSort(TaxCode.SortEnum sortEnum, bool backwards)
        {
            string sortString = "";

            switch (sortEnum)
            {
                case TaxCode.SortEnum.SalesTaxCode:
                    sortString = " Order by g.TAXCODE ASC";
                    break;
                case TaxCode.SortEnum.Description:
                    sortString = " Order by TAXNAME ASC";
                    break;
                case TaxCode.SortEnum.RoundOff:
                    sortString = " Order by TAXROUNDOFF ASC";
                    break;
                case TaxCode.SortEnum.RoundingType:
                    sortString = " Order by TAXROUNDOFFTYPE ASC";
                    break;
            }

            if (backwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static void PopulateTaxCode(IDataReader dr, TaxCode taxCode)
        {
            taxCode.ID = (string)dr["TAXCODE"];
            taxCode.Text = (string)dr["TAXNAME"];
            taxCode.TaxRoundOff = (decimal)dr["TAXROUNDOFF"];
            taxCode.TaxRoundOffType = (TaxCode.RoundoffTypeEnum)dr["TAXROUNDOFFTYPE"];
            taxCode.ReceiptDisplay = (string)dr["PRINTCODE"];
        }

        /// <summary>
        /// Gets a tax code with a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeID">ID of the tax code</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns></returns>
        public virtual TaxCode Get(IConnectionManager entry, RecordIdentifier taxCodeID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            TaxCode result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSql +
                    "where g.TAXCODE = @taxCode and g.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "taxCode", (string)taxCodeID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Get<TaxCode>(entry, cmd, taxCodeID, PopulateTaxCode, cacheType,UsageIntentEnum.Normal);
            }

            if (result != null)
            {
                // We dont need to load if it came from the cache
                if (result.TaxCodeLines == null)
                {
                    result.TaxCodeLines = Providers.TaxCodeValueData.GetTaxCodeValues(entry, taxCodeID, 0, false);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a list of all tax codes as data entities
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all tax codes as data entities</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "TAXTABLE", "TAXNAME", "TAXCODE", "TAXNAME");
        }

        /// <summary>
        /// Gets a list of all tax codes
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">Defines the sort ordering of the results</param>
        /// <param name="backwardsSort">Defines if results should be sorted backwards</param>
        /// <param name="usage">Use normal or report if taxcode lines should be included.</param>
        /// <returns>A list of all tax codes</returns>
        public virtual List<TaxCode> GetTaxCodes(IConnectionManager entry, TaxCode.SortEnum sortEnum, bool backwardsSort, UsageIntentEnum usage = UsageIntentEnum.Minimal)
        {
            ValidateSecurity(entry);
            List<TaxCode> result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSql +
                    "where g.DATAAREAID = @dataAreaId" + ResolveSort(sortEnum, backwardsSort);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Execute<TaxCode>(entry, cmd, CommandType.Text, PopulateTaxCode);
            }
            if (usage != UsageIntentEnum.Minimal  && result != null)
            {
                foreach (TaxCode taxCode in result)
                {
                    taxCode.TaxCodeLines = Providers.TaxCodeValueData.GetTaxCodeValues(entry, taxCode.ID, 0, false);
                }
            }
            return result;
        }

        public virtual List<TaxCode> GetTaxCodesForGroup(IConnectionManager entry, RecordIdentifier groupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select g.TAXCODE, " +
                                  "ISNULL(g.TAXNAME,'') as TAXNAME, " +
                                  "ISNULL(g.TAXROUNDOFF,0.0) as TAXROUNDOFF, " +
                                  "ISNULL(g.TAXROUNDOFFTYPE,0) as TAXROUNDOFFTYPE, " +
                                  "ISNULL(g.TAXBASE, 0) as TAXBASE, " +
                                  "ISNULL(g.TAXLIMITBASE, 0) as TAXLIMITBASE, " +
                                  "ISNULL(g.TAXCALCMETHOD, 0) as TAXCALCMETHOD, " +
                                  "ISNULL(g.TAXONTAX, '') as TAXONTAX, " +
                                  "ISNULL(g.TAXUNIT, '') as TAXUNIT, " +
                                  "ISNULL(b.TAXNAME,'') as TAXONTAXDESCRIPTION, " +
                                  "ISNULL(u.TXT,'') as TAXUNITDESCRIPTION, " +
                                  "ISNULL(g.PRINTCODE, '') as PRINTCODE, " +
                                  "ISNULL(g.TAXINCLUDEINTAX,0) as TAXINCLUDEINTAX " +
                                  "from TAXTABLE g " +
                                  "join TAXGROUPDATA tgd on tgd.TAXCODE = g.TAXCODE and tgd.DATAAREAID = g.DATAAREAID and tgd.TAXGROUP = @taxGroupID " +
                                  "left outer join TAXTABLE b on g.DATAAREAID = b.DATAAREAID and b.TAXCODE = g.TAXONTAX " +
                                  "left outer join UNIT u on u.DATAAREAID = g.DATAAREAID and u.UNITID = g.TAXUNIT" +
                                  "where g.DATAAREAID = @dataAreaID";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "taxGroupID", groupID);

                return Execute<TaxCode>(entry, cmd, CommandType.Text, PopulateTaxCode);
            }
        }

        /// <summary>
        /// Wether a tax code with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeID">ID of the taxcode</param>
        /// <returns></returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier taxCodeID)
        {
            return RecordExists<TaxCode>(entry, "TAXTABLE", "TAXCODE", taxCodeID);
        }

        /// <summary>
        /// Deletes a tax code with a given ID from the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeID">ID of the taxcode</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier taxCodeID)
        {
            DeleteRecord(entry, "TAXTABLE", "TAXCODE", taxCodeID, BusinessObjects.Permission.EditSalesTaxSetup);
            DeleteRecord(entry, "TAXDATA", "TAXCODE", taxCodeID, BusinessObjects.Permission.EditSalesTaxSetup);
            DeleteRecord(entry, "TAXGROUPDATA", "TAXCODE", taxCodeID, BusinessObjects.Permission.EditSalesTaxSetup);

            // Multi delete of the TAXDATA subrecords triggers that we have to invalidate the entire cache
            entry.Cache.InvalidateEntityCache();            
        }

        /// <summary>
        /// Saves a given taxcode to the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCode"></param>
        public virtual void Save(IConnectionManager entry, TaxCode taxCode)
        {
            var statement = new SqlServerStatement("TAXTABLE");
            statement.UpdateColumnOptimizer = taxCode;

            ValidateSecurity(entry, BusinessObjects.Permission.EditSalesTaxSetup);

            bool isNew = false;
            if (taxCode.ID.IsEmpty)
            {
                isNew = true;
                taxCode.ID = DataProviderFactory.Instance.GenerateNumber<ITaxCodeData, TaxCode>(entry);
            }

            if (isNew || !Exists(entry, taxCode.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("TAXCODE", (string)taxCode.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("TAXCODE", (string)taxCode.ID);
            }

            statement.AddField("TAXNAME", taxCode.Text);
            statement.AddField("TAXROUNDOFF", taxCode.TaxRoundOff, SqlDbType.Decimal);
            statement.AddField("TAXROUNDOFFTYPE", (int)taxCode.TaxRoundOffType, SqlDbType.Int);
            statement.AddField("PRINTCODE", taxCode.ReceiptDisplay);

            Save(entry, taxCode, statement);

            if (!isNew)
            {
                // Updating the tax data means that we have to invalidate the entire cache
                entry.Cache.InvalidateEntityCache();
            }
        }

        /// <summary>
        /// Gets all the items from the database matching the list of <paramref name="itemsToCompare"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items</param>
        /// <returns>List of items</returns>
        public virtual List<TaxCode> GetCompareList(IConnectionManager entry, List<TaxCode> itemsToCompare)
        {
            var columns = new List<TableColumn>
            {
                new TableColumn {ColumnName = "TAXCODE", TableAlias = "T"},
                new TableColumn {ColumnName = "TAXNAME", TableAlias = "T"},
                new TableColumn {ColumnName = "TAXROUNDOFF", TableAlias = "T"},
                new TableColumn {ColumnName = "TAXROUNDOFFTYPE", TableAlias = "T"},
                new TableColumn {ColumnName = "PRINTCODE", TableAlias = "T"},
            };

            return GetCompareListInBatches(entry, itemsToCompare, "TAXTABLE", "TAXCODE", columns, null, PopulateTaxCode);
        }

        #region ISequenceable Members

        /// <summary>
        /// Wether a tax code with the given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the tax code</param>
        /// <returns>Wether a tax code with the given ID exists</returns>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        /// <summary>
        /// Returns the name of the sequence
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "TAXCODE"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "TAXTABLE", "TAXCODE", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}