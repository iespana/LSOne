using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Tax
{
    /// <summary>
    /// Data provider class for item sales tax groups
    /// </summary>
    public class ItemSalesTaxGroupData : SqlServerDataProviderBase, IItemSalesTaxGroupData
    {
        private static string ResolveItemSalesTaxGroupSort(ItemSalesTaxGroup.SortEnum sortEnum, bool sortBackwards)
        {
            string sortString = "";

            switch (sortEnum)
            {
                case ItemSalesTaxGroup.SortEnum.ID:
                    sortString = " Order By g.TAXITEMGROUP ASC";
                    break;
                case ItemSalesTaxGroup.SortEnum.Description:
                    sortString = " Order By NAME ASC";
                    break;
            }

            if (sortBackwards)
            {
                sortString = sortString.Replace("ACS", "DESC");
            }

            return sortString;
        }

        private static string ResolveTaxCodeInItemSalesTaxGroupSort(TaxCodeInItemSalesTaxGroup.SortEnum sortEnum, bool sortBackwards)
        {
            string sortString = "";

            switch (sortEnum)
            {
                case TaxCodeInItemSalesTaxGroup.SortEnum.SalesTaxCode:
                    sortString = " Order By g.TAXCODE ASC";
                    break;
                case TaxCodeInItemSalesTaxGroup.SortEnum.Description:
                    sortString = " Order By tt.TAXNAME ASC";
                    break;
                case TaxCodeInItemSalesTaxGroup.SortEnum.PercentageAmount:
                    sortString = " Order By TAXVALUE ASC";
                    break;
            }

            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static string BaseSql
        {
            get
            {
                return "Select g.TAXITEMGROUP,ISNULL(g.NAME,'') as NAME, " +
                       "ISNULL(g.RECEIPTDISPLAY,'') as RECEIPTDISPLAY " +
                        "from TAXITEMGROUPHEADING g ";
            }
        }

        private static string BaseDistinctSql
        {
            get
            {
                return "Select distinct g.TAXITEMGROUP,ISNULL(g.NAME,'') as NAME, " +
                       "ISNULL(g.RECEIPTDISPLAY,'') as RECEIPTDISPLAY " +
                        "from TAXITEMGROUPHEADING g ";
            }
        }

        private void PopulateDataEntity(IDataReader dr, DataEntity taxGroup)
        {
            taxGroup.ID = (string)dr["TAXITEMGROUP"];
            taxGroup.Text = (string)dr["NAME"];
        }

        private static void PopulateItemSalesTaxGroup(IDataReader dr, ItemSalesTaxGroup itemSalesTaxGroup)
        {
            itemSalesTaxGroup.ID = (string)dr["TAXITEMGROUP"];
            itemSalesTaxGroup.Text = (string)dr["NAME"];
            itemSalesTaxGroup.ReceiptDisplay = (string)dr["RECEIPTDISPLAY"];
        }

        private static void PopulateTaxCodeInItemSalesTaxGroup(IDataReader dr, TaxCodeInItemSalesTaxGroup line)
        {
            line.TaxItemGroup = (string)dr["TAXITEMGROUP"];
            line.TaxCode = (string)dr["TAXCODE"];
            line.Text = (string)dr["TAXNAME"];
            line.TaxValue = (decimal)dr["TAXVALUE"];
        }

        /// <summary>
        /// Check if Tax Group is in use by retail item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemSalesTaxGroupID">The ID of the item sales tax group to get</param>
        /// <returns>True if Tax Group is in use</returns>
        public virtual bool TaxGroupInUse(IConnectionManager entry, RecordIdentifier itemSalesTaxGroupID)
        {
            List<ItemSalesTaxGroup> taxGroups = new List<ItemSalesTaxGroup>();
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSql +
                    @"join TAXONITEM ti on g.TAXITEMGROUP = ti.TAXITEMGROUP AND g.DATAAREAID = ti.DATAAREAID
                      join RETAILITEM ri on g.TAXITEMGROUP = ri.SALESTAXITEMGROUPID
                      where g.DATAAREAID = @DATAAREAID AND g.TAXITEMGROUP = @taxItemGroup";

                MakeParam(cmd, "taxItemGroup", (string) itemSalesTaxGroupID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                taxGroups = GetList<ItemSalesTaxGroup>(entry, cmd, RecordIdentifier.Empty, PopulateItemSalesTaxGroup, CacheType.CacheTypeNone);
            }
            return taxGroups.Count > 0;
        }

        /// <summary>
        /// Gets a item sales tax group by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemSalesTaxGroupID">The ID of the item sales tax group to get</param>
        /// <param name="cacheType">Cache</param>
        /// <returns>An item sales tax group by a given ID</returns>
        public virtual ItemSalesTaxGroup Get(IConnectionManager entry, RecordIdentifier itemSalesTaxGroupID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSql +
                    "where g.TAXITEMGROUP = @taxItemGroup AND g.DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "taxItemGroup", (string)itemSalesTaxGroupID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Get<ItemSalesTaxGroup>(entry, cmd, itemSalesTaxGroupID, PopulateItemSalesTaxGroup, cacheType, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all item sales tax groups
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities containing IDs and names of all item sales tax groups</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "TAXITEMGROUPHEADING", "NAME", "TAXITEMGROUP", "NAME");
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all item sales tax groups that have tax codes
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities containing IDs and names of all item sales tax groups</returns>
        public virtual List<DataEntity> GetListWithTaxCodes(IConnectionManager entry)
        { 
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseDistinctSql +
                    "join TAXONITEM ti on ti.TAXITEMGROUP = g.TAXITEMGROUP AND ti.DATAAREAID = g.DATAAREAID where g.DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                return GetList<DataEntity>(entry, cmd, RecordIdentifier.Empty, PopulateDataEntity, CacheType.CacheTypeNone);
            }
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all item sales tax groups, ordered by 
        /// a sort enum and a reversed ordering based on a parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">The enum that determines what to sort by</param>
        /// <param name="backwardsSort">Whether the result set is ordered backwards</param>
        /// <param name="cacheType">Cache</param>
        /// <returns>A list of data entities containing IDs and names of all item sales tax groups, meeting the above criteria</returns>
        public virtual List<ItemSalesTaxGroup> GetItemSalesTaxGroups(IConnectionManager entry, ItemSalesTaxGroup.SortEnum sortEnum, bool backwardsSort, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            RecordIdentifier id = new RecordIdentifier("GetItemSalesTaxGroups", (int)sortEnum, backwardsSort);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSql +
                    "where g.DATAAREAID = @DATAAREAID" + ResolveItemSalesTaxGroupSort(sortEnum, backwardsSort);

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return GetList<ItemSalesTaxGroup>(entry, cmd, id, PopulateItemSalesTaxGroup, cacheType);
            }
        }

        /// <summary>
        /// Checks if an item sales tax group with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxItemGroupID">The ID of the item sales tax group to check for</param>
        /// <returns>Whether an item sales tax group with a given ID exists in the database</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier taxItemGroupID)
        {
            return RecordExists(entry, "TAXITEMGROUPHEADING", "TAXITEMGROUP", taxItemGroupID);
        }

        /// <summary>
        /// Saves a given item sales tax group to the database
        /// </summary>
        /// <remarks>Requires the 'Edit sales tax setup' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxItemGroup">The item sales tax group to save</param>
        public virtual void Save(IConnectionManager entry, ItemSalesTaxGroup taxItemGroup)
        {
            var statement = new SqlServerStatement("TAXITEMGROUPHEADING");
            statement.UpdateColumnOptimizer = taxItemGroup;

            ValidateSecurity(entry, Permission.EditSalesTaxSetup);

            bool isNew = false;
            if (taxItemGroup.ID.IsEmpty)
            {
                isNew = true;
                taxItemGroup.ID = DataProviderFactory.Instance.GenerateNumber<IItemSalesTaxGroupData, ItemSalesTaxGroup>(entry);
            }

            if (isNew ||!Exists(entry, taxItemGroup.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("TAXITEMGROUP", (string)taxItemGroup.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("TAXITEMGROUP", (string)taxItemGroup.ID);
            }

            statement.AddField("NAME", taxItemGroup.Text);
            statement.AddField("RECEIPTDISPLAY", taxItemGroup.ReceiptDisplay);

            entry.Connection.ExecuteStatement(statement);

            if (!isNew)
            {
                // Updating the tax data means that we have to invalidate the entire cache
                entry.Cache.InvalidateEntityCache();
            }
        }

        /// <summary>
        /// Deletes an item sales tax group by a given ID
        /// </summary>
        /// <remarks>Requires the 'Edit sales tax setup' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxItemGroupID">The ID of the item sales tax group to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier taxItemGroupID)
        {
            DeleteRecord(entry, "TAXITEMGROUPHEADING", "TAXITEMGROUP", taxItemGroupID, Permission.EditSalesTaxSetup);
            DeleteRecord(entry, "TAXONITEM", "TAXITEMGROUP", taxItemGroupID, Permission.EditSalesTaxSetup);

            // Deletion of tax data means that we have to invalidate the entire cache
            entry.Cache.InvalidateEntityCache();
        }

        /// <summary>
        /// Adds a tax code to an item sales tax group.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="item">Contains IDs of the tax code and the item sales tax group</param>
        public virtual void AddTaxCodeToItemSalesTaxGroup(IConnectionManager entry, TaxCodeInItemSalesTaxGroup item)
        {
            var statement = new SqlServerStatement("TAXONITEM");

            ValidateSecurity(entry, Permission.EditSalesTaxSetup);

            if (!TaxCodeIsInItemSalesTaxGroup(entry, item.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("TAXITEMGROUP", (string)item.TaxItemGroup);
                statement.AddKey("TAXCODE", (string)item.TaxCode);

                entry.Connection.ExecuteStatement(statement);

                // Updating of tax data means that we have to invalidate the entire cache
                entry.Cache.InvalidateEntityCache();
            }
        }

        /// <summary>
        /// Tells if a tax code is in an item sales tax group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">Contains (item sales tax group ID, tax code ID)</param>
        /// <returns>Wether the tax code is in the item sales tax group</returns>
        private static bool TaxCodeIsInItemSalesTaxGroup(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "TAXONITEM", new[] { "TAXITEMGROUP", "TAXCODE" }, id);
        }

        /// <summary>
        /// Removes a tax code from an item sales tax group
        /// </summary>
        /// <remarks>Requires the 'Edit sales tax setup' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">Contains (item sales tax group ID, tax code ID)</param>
        public virtual void RemoveTaxCodeFromItemSalesTaxGroup(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "TAXONITEM", new[] { "TAXITEMGROUP", "TAXCODE" }, id, Permission.EditSalesTaxSetup);

            // Deletion of tax data means that we have to invalidate the entire cache
            entry.Cache.InvalidateEntityCache();
        }

        /// <summary>
        /// Gets a list of tax codes in an item sales tax group. Returns data entities of the tax codes.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemSalesTaxGroupID">The ID of the item sales tax group</param>
        /// <param name="cacheType">Cache</param>
        /// <returns>DataEntities of tax codes in an item sales tax group</returns>
        public virtual List<DataEntity> GetTaxCodesInItemSalesTaxGroupList(IConnectionManager entry, RecordIdentifier itemSalesTaxGroupID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            RecordIdentifier id = new RecordIdentifier("GetTaxCodesInItemSalesTaxGroupList", itemSalesTaxGroupID);

            if (cacheType != CacheType.CacheTypeNone)
            {
                CacheBucket bucket = (CacheBucket)entry.Cache.GetEntityFromCache(typeof(CacheBucket), id);
                if (bucket != null)
                {
                    return (List<DataEntity>)bucket.BucketData;
                }
            }

            List<DataEntity> result = null;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select t.TAXCODE,ISNULL(t.TAXNAME,'') as TAXNAME from TAXTABLE t " +
                                  "where not Exists(Select 'x' from TAXONITEM tt where TAXITEMGROUP = @taxItemGroup and t.TAXCODE = tt.TAXCODE and tt.DATAAREAID = @DATAAREAID) " +
                                  "and t.DATAAREAID = @DATAAREAID order by t.TAXNAME";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "taxItemGroup", (string) itemSalesTaxGroupID);

                result = Execute<DataEntity>(entry, cmd, CommandType.Text, "TAXNAME", "TAXCODE");
            }
            if (result != null && cacheType != CacheType.CacheTypeNone)
            {
                entry.Cache.AddEntityToCache(id, new CacheBucket(id, "", result), cacheType);
            }
            return result;
        }

        /// <summary>
        /// Gets a list of TaxCode-ItemSalesTaxGroup connections for a given item sales tax group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemSalesTaxGroupID">ID of the item sales tax group</param>
        /// <param name="sortEnum">Determines the sort ordering of the results</param>
        /// <param name="backwardsSort">Determines if the results will be in reversed ordering</param>
        /// <param name="cacheType">Cache</param>
        /// <returns>A list of TaxCode-ItemSalesTaxGroup connections for a given item sales tax group</returns>
        public List<TaxCodeInItemSalesTaxGroup> GetTaxCodesInItemSalesTaxGroup(
            IConnectionManager entry, 
            RecordIdentifier itemSalesTaxGroupID, 
            TaxCodeInItemSalesTaxGroup.SortEnum sortEnum, 
            bool backwardsSort,
            CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);
            RecordIdentifier id = new RecordIdentifier("GetTaxCodesInItemSalesTaxGroup", itemSalesTaxGroupID, (int)sortEnum, backwardsSort);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select g.TAXCODE,ISNULL(tt.TAXNAME,'') as TAXNAME , ISNULL(td.TAXVALUE,-1) as TAXVALUE , g.TAXITEMGROUP " +
                    "from TAXONITEM g " +
                    "left outer join TAXTABLE tt on g.TAXCODE = tt.TAXCODE and g.DATAAREAID = tt.DATAAREAID " +
                         "left outer join (select TAXCODE, TAXVALUE, DATAAREAID from TAXDATA " +
                         "where ((TAXFROMDATE = '1900-01-01 00:00:00.000' and TAXTODATE = '1900-01-01 00:00:00.000') or " +
                         "(CONVERT(DATE,GETDATE()) >= TAXFROMDATE and TAXTODATE = '1900-01-01 00:00:00.000') or " +
                         "(CONVERT(DATE,GETDATE()) >= TAXFROMDATE and CONVERT(DATE,GETDATE()) <= TAXTODATE))) " +
                         "td on td.TAXCODE = g.TAXCODE and td.DATAAREAID = g.DATAAREAID " +
                      "where g.TAXITEMGROUP = @taxItemGroup and tt.DATAAREAID = @DATAAREAID" + ResolveTaxCodeInItemSalesTaxGroupSort(sortEnum, backwardsSort);

                MakeParam(cmd, "taxItemGroup", (string)itemSalesTaxGroupID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return GetList<TaxCodeInItemSalesTaxGroup>(entry, cmd, id, PopulateTaxCodeInItemSalesTaxGroup, cacheType);
            }
        }

        /// <summary>
        /// Gets all the items from the database matching the list of <see cref="itemsToCompare" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items <see cref="itemsToCompare"</param>
        /// <returns>List of items</returns>
        public virtual List<ItemSalesTaxGroup> GetCompareList(IConnectionManager entry, List<ItemSalesTaxGroup> itemsToCompare)
        {
            var columns = new List<TableColumn>
            {
                new TableColumn {ColumnName = "TAXITEMGROUP", TableAlias = "T"},
                new TableColumn {ColumnName = "NAME", TableAlias = "T"},
                new TableColumn {ColumnName = "RECEIPTDISPLAY", TableAlias = "T"},
            };

            return GetCompareListInBatches(entry, itemsToCompare, "TAXITEMGROUPHEADING", "TAXITEMGROUP", columns, null, PopulateItemSalesTaxGroup);
        }

        #region ISequenceable Members

        /// <summary>
        /// Wether an item sales tax group with the given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the item sales tax group</param>
        /// <returns>Wether an item sales tax group with the given ID exists</returns>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry,id);
        }

        /// <summary>
        /// Returns the name of the sequence
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "ITEMTAXGROUP"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "TAXONITEM", "TAXITEMGROUP", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}