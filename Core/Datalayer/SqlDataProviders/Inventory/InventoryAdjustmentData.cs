using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    public class InventoryAdjustmentData : SqlServerDataProviderBase, IInventoryAdjustmentData
    {
        protected InventoryAdjustmentSorting secondarySort = InventoryAdjustmentSorting.Description;

        private static string BaseSql { 
            get
            {
                return
                @"
                SELECT DISTINCT a.JOURNALID
                ,ISNULL(a.DESCRIPTION,'') AS DESCRIPTION
                ,ISNULL(a.POSTED, 0) AS POSTED
                ,a.POSTEDDATETIME
                ,a.JOURNALTYPE
                ,a.CREATEDDATETIME
                ,a.PROCESSINGSTATUS
                ,a.DELETEPOSTEDLINES
                ,ISNULL(a.STOREID, '') AS STOREID
                ,ISNULL(st.NAME, '') AS STORENAME
                ,a.CREATEDFROMOMNI
                ,ISNULL(a.TEMPLATEID, '') AS TEMPLATEID
                ,ISNULL(a.MASTERID, '00000000-0000-0000-0000-000000000000') AS MASTERID
                ,CASE WHEN EXISTS(SELECT 1 FROM INVENTJOURNALTRANS IJT WHERE IJT.JOURNALID = a.JOURNALID AND IJT.DATAAREAID = a.DATAAREAID AND IJT.POSTED = 1) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS ISPARTIALLYPOSTED
                FROM INVENTJOURNALTABLE a  
                LEFT OUTER JOIN RBOSTORETABLE st ON st.STOREID = a.STOREID and st.DATAAREAID = a.DATAAREAID ";
            }  
        }

        private static List<TableColumn> stockCountingColumns = new List<TableColumn>
        {

            new TableColumn {ColumnName = "JOURNALID " , TableAlias = "a"},
            new TableColumn {ColumnName = "ISNULL(a.DESCRIPTION,'') " , ColumnAlias = "DESCRIPTION"},
            new TableColumn {ColumnName = "ISNULL(a.POSTED, 0) " , ColumnAlias = "POSTED"},
            new TableColumn {ColumnName = "POSTEDDATETIME " ,TableAlias = "a"},
            new TableColumn {ColumnName = "JOURNALTYPE " , TableAlias = "a"},
            new TableColumn {ColumnName = "PROCESSINGSTATUS " , TableAlias = "a"},
            new TableColumn {ColumnName = "CREATEDDATETIME ", TableAlias = "a" },
            new TableColumn {ColumnName = "DELETEPOSTEDLINES " , TableAlias = "a"},
            new TableColumn {ColumnName = "ISNULL(a.STOREID, '') " , ColumnAlias = "STOREID"},
            new TableColumn {ColumnName = "ISNULL(st.NAME, '') " , ColumnAlias = "STORENAME"},
            new TableColumn {ColumnName = "CREATEDFROMOMNI " , TableAlias = "a"},
            new TableColumn {ColumnName = "ISNULL(a.TEMPLATEID, '') " , ColumnAlias = "TEMPLATEID"},
            new TableColumn {ColumnName = "ISNULL(a.MASTERID, '00000000-0000-0000-0000-000000000000') " , ColumnAlias = "MASTERID"},
            new TableColumn {ColumnName = "CASE WHEN EXISTS(SELECT 1 FROM INVENTJOURNALTRANS IJT WHERE IJT.JOURNALID = a.JOURNALID AND IJT.DATAAREAID = a.DATAAREAID AND IJT.POSTED = 1) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END " , ColumnAlias = "ISPARTIALLYPOSTED"}
        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = "st.STOREID = a.STOREID",
                JoinType = "LEFT",
                Table = "RBOSTORETABLE",
                TableAlias = "st"
            }
        };


        private static string ResolveSort(InventoryAdjustmentSorting sort, bool backwards, string alias, bool counting, bool secondarySortSameAsSort = false)
        {
            string direction = backwards ? " DESC" : " ASC";
            string dot = "";

            string countingString = "NAME";
            if (counting)
            {
                countingString = "STORENAME";
            }

            if (alias != "")
            {
                dot = ".";
            }

            switch (sort)
            {
                case InventoryAdjustmentSorting.ID:
                    return alias + dot + "JOURNALID" + direction;

                case InventoryAdjustmentSorting.Description:
                    if (secondarySortSameAsSort)
                    {
                        return alias + dot + "JOURNALID" + direction;
                    }
                    return alias + dot + "DESCRIPTION" + direction;

                case InventoryAdjustmentSorting.StoreName:
                    return alias + dot + countingString + direction;

                case InventoryAdjustmentSorting.Posted:
                    return alias + dot + "POSTED" + direction;

                case InventoryAdjustmentSorting.PostingDate:
                    return alias + dot + "POSTEDDATETIME" + direction;

                case InventoryAdjustmentSorting.DeletePostedLines:
                    return alias + dot + "DELETEPOSTEDLINES" + direction;

                case InventoryAdjustmentSorting.CreatedDateTime:
                    return alias + dot + "CREATEDDATETIME" + direction;
                case InventoryAdjustmentSorting.ProcessingStatus:
                    return $@"ORDER BY CASE PROCESSINGSTATUS 
                                WHEN {(int)InventoryProcessingStatus.Compressing} THEN 0
                                WHEN {(int)InventoryProcessingStatus.Importing} THEN 1
                                WHEN {(int)InventoryProcessingStatus.None} THEN 2
                                WHEN {(int)InventoryProcessingStatus.Other} THEN 3
                                WHEN {(int)InventoryProcessingStatus.Posting} THEN 4
                                END";
            }

            return "";
        }

        protected virtual void PopulateJournalInfoWithCount(IConnectionManager entry, IDataReader dr,
            InventoryAdjustment journalAdjustment,
            ref int rowCount)
        {
            PopulateJournal(dr, journalAdjustment);
            PopulateRowCount(entry, dr, ref rowCount);
        }

        private static void PopulateJournal(IDataReader dr, InventoryAdjustment inventoryJournalInfo)
        {
            inventoryJournalInfo.ID = (string)dr["JOURNALID"];
            inventoryJournalInfo.Text = (string)dr["DESCRIPTION"];
            inventoryJournalInfo.DeletePostedLines = (int)dr["DELETEPOSTEDLINES"];
            inventoryJournalInfo.JournalType = (InventoryJournalTypeEnum)dr["JOURNALTYPE"];
            inventoryJournalInfo.Posted = (InventoryJournalStatus)(int)dr["POSTED"];
            inventoryJournalInfo.PostedDateTime = Date.FromAxaptaDate(dr["POSTEDDATETIME"]);
            inventoryJournalInfo.CreatedDateTime = (DateTime)dr["CREATEDDATETIME"];
            inventoryJournalInfo.StoreId = (string)dr["STOREID"];
            inventoryJournalInfo.StoreName = (string)dr["STORENAME"];
            inventoryJournalInfo.TemplateID = (string)dr["TEMPLATEID"];
            inventoryJournalInfo.CreatedFromOmni = AsBool(dr["CREATEDFROMOMNI"]);
            inventoryJournalInfo.MasterID = AsGuid(dr["MASTERID"]);
            inventoryJournalInfo.IsPartiallyPosted = AsBool(dr["ISPARTIALLYPOSTED"]);
            inventoryJournalInfo.ProcessingStatus = (InventoryProcessingStatus)dr["PROCESSINGSTATUS"];
        }

        protected virtual void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
        }

        /// <summary>
        /// Gets a list of DataEntity that contains JOURNALID and Description only (for usage in ComboBoxes). The list is sorted by the column specified in the parameter 'sort'.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>Gets a list of DataEntity that contains the Journal Id  and its description</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, InventoryAdjustmentSorting sortBy, bool sortBackwards)
        {
            if (sortBy != InventoryAdjustmentSorting.ID && sortBy != InventoryAdjustmentSorting.Description)
            {
                throw new NotSupportedException();
            }

            return GetList<DataEntity>(entry, "INVENTJOURNALTABLE", "DESCRIPTION", "JOURNALID", ResolveSort(sortBy, sortBackwards, "", false));
        }

        /// <summary>
        /// Gets a list of DataEntity that contains JOURNALID and Description. The list is sorted by JOURNALID.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Gets a list of DataEntity that contains JOURNALID and Description</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "INVENTJOURNALTABLE", "DESCRIPTION", "JOURNALID", "JOURNALID");
        }

        /// <summary>
        /// Checks if a JOURNAL row with a given JOURNALID exists in the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the JOURNAL row to check for</param>
        /// <returns>Whether a JOURNAL row with a given ID exists in the database</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "INVENTJOURNALTABLE", id.DBType == SqlDbType.UniqueIdentifier ? "MASTERID" : "JOURNALID", id);
        }

        /// <summary>
        /// Deletes a JOURNAL row with a given ID
        /// </summary>
        /// <remarks>Requires the 'JournalEdit' permission</remarks>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the Journal row to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "INVENTJOURNALTABLE", id.DBType == SqlDbType.UniqueIdentifier ? "MASTERID" : "JOURNALID", id, new[] { Permission.EditInventoryAdjustments, Permission.StockCounting });
            DeleteRecord(entry, "INVENTJOURNALTRANS", id.DBType == SqlDbType.UniqueIdentifier ? "MASTERID" : "JOURNALID", id, new[] { Permission.EditInventoryAdjustments, Permission.StockCounting });
        }

        /// <summary>
        /// Gets a Journal entry with a given ID
        /// </summary>
        /// <param name="entry">The entry connection into the database</param>
        /// <param name="journalId">The ID of the Journal row to fetch</param>
        /// <returns>A Journal row with a given ID</returns>
        public virtual InventoryAdjustment Get(IConnectionManager entry, RecordIdentifier journalId)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                if (journalId.DBType == SqlDbType.UniqueIdentifier)
                {
                    cmd.CommandText = BaseSql +
                                      @"WHERE a.MASTERID = @JOURNALID AND a.DATAAREAID = @DATAAREAID ";
                    MakeParam(cmd, "JOURNALID", (Guid)journalId, SqlDbType.UniqueIdentifier);
                }
                else
                {
                    cmd.CommandText = BaseSql +
                                      @"WHERE a.JOURNALID = @JOURNALID AND a.DATAAREAID = @DATAAREAID ";
                    MakeParam(cmd, "JOURNALID", (string)journalId);
                }

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                

                var result = Execute<InventoryAdjustment>(entry, cmd, CommandType.Text, PopulateJournal);
                return (result.Count > 0) ? result[0] : null;
            }
        }

        public virtual List<InventoryAdjustment> GetJournalListAdvanced(IConnectionManager entry, InventoryAdjustmentFilter filter, out int totalRecordsMatching)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                totalRecordsMatching = 0;

                List<TableColumn> externalColumns = new List<TableColumn>(stockCountingColumns);

                externalColumns.Add(new TableColumn
                {
                    ColumnName = "ROW_COUNT"
                });

                List<TableColumn> internalColumns = new List<TableColumn>(stockCountingColumns);

                internalColumns.Add(new TableColumn
                {
                    ColumnName = string.Format("COUNT(1) OVER ( ORDER BY {0} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
                    filter.Sort == InventoryAdjustmentSorting.None ? "a.JOURNALID" : (filter.Sort == InventoryAdjustmentSorting.StoreName ? ResolveSort(filter.Sort, filter.SortBackwards, "st", false) : ResolveSort(filter.Sort, filter.SortBackwards, "a", false))),
                    ColumnAlias = "ROW_COUNT"
                });

                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "a.JOURNALTYPE = @JOURNALTYPE" });
                MakeParam(cmd, "JOURNALTYPE", (int)filter.JournalType);

                if (filter.Status != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "a.POSTED = @POSTEDSTATUS" });
                    MakeParam(cmd, "POSTEDSTATUS", (int)filter.Status);
                }

                if (filter.ProcessingStatus != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "a.PROCESSINGSTATUS = @PROCESSINGSTATUS" });
                    MakeParam(cmd, "PROCESSINGSTATUS", (int)filter.ProcessingStatus);
                }

                if (filter.StoreID != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "a.STOREID = @STOREID " });
                    MakeParam(cmd, "STOREID", (string)filter.StoreID);
                }

                if (filter.IdOrDescription.Count > 0)
                {
                    List<Condition> searchConditions = new List<Condition>();
                    for (int index = 0; index < filter.IdOrDescription.Count; index++)
                    {
                        var searchToken = PreProcessSearchText(filter.IdOrDescription[index], true, filter.IdOrDescriptionBeginsWith);

                        if (!string.IsNullOrEmpty(searchToken))
                        {
                            searchConditions.Add(new Condition
                            {
                                ConditionValue =
                                    $@" (a.JOURNALID LIKE @DESCRIPTION{index} 
                                        OR a.DESCRIPTION LIKE @DESCRIPTION{index}) ",
                                Operator = "AND"

                            });

                            MakeParam(cmd, $"DESCRIPTION{index}", searchToken);
                        }
                    }
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue =
                            $" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) "
                    });
                }

                if (filter.CreationDateFrom != null && filter.CreationDateFrom != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "a.CREATEDDATETIME >= CONVERT(datetime, @CREATIONDATEFROM, 103)" });
                    MakeParam(cmd, "CREATIONDATEFROM", filter.CreationDateFrom.DateTime.Date, SqlDbType.DateTime);
                }

                if (filter.CreationDateTo != null && filter.CreationDateTo != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "a.CREATEDDATETIME <= CONVERT(datetime, @CREATIONDATETO, 103)" });
                    MakeParam(cmd, "CREATIONDATETO", filter.CreationDateTo.DateTime.Date, SqlDbType.DateTime);
                }

                if (filter.PostedDateFrom != null && filter.PostedDateFrom != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "a.POSTEDDATETIME >= CONVERT(datetime, @POSTEDDATEFROM, 103)" });
                    MakeParam(cmd, "POSTEDDATEFROM", filter.PostedDateFrom.DateTime.Date, SqlDbType.DateTime);
                }

                if (filter.PostedDateTo != null && filter.PostedDateTo != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "a.POSTEDDATETIME <= CONVERT(datetime, @POSTEDDATETO, 103)" });
                    MakeParam(cmd, "POSTEDDATETO", filter.PostedDateTo.DateTime.Date, SqlDbType.DateTime);
                }

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("INVENTJOURNALTABLE", "a", "ex", filter.PagingSize, true),
                    QueryPartGenerator.ExternalColumnGenerator(externalColumns, "ex"),
                    QueryPartGenerator.InternalColumnGenerator(internalColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "",
                    "ORDER BY " + ResolveSort(filter.Sort, filter.SortBackwards, "ex", true) + ", " + ResolveSort(secondarySort, false, "ex", true, filter.Sort == secondarySort)
                    );

                return Execute<InventoryAdjustment, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateJournalInfoWithCount);
            }
        }

        public virtual AdjustmentStatus GetAdjustmentStatus(IConnectionManager entry, RecordIdentifier journalId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT POSTED, PROCESSINGSTATUS FROM INVENTJOURNALTABLE WHERE JOURNALID = '{journalId.StringValue}' AND DATAAREAID = '{entry.Connection.DataAreaId}'";
                IDataReader result = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                AdjustmentStatus status = new AdjustmentStatus();

                if(result.Read())
                {
                    status.InventoryStatus = (InventoryJournalStatus)((int)result["POSTED"]);
                    status.ProcessingStatus = (InventoryProcessingStatus)((int)result["PROCESSINGSTATUS"]);
                }

                return status;
            }
        }

        public virtual List<InventoryAdjustment> GetJournalList(
            IConnectionManager entry,
            RecordIdentifier storeId,
            InventoryJournalTypeEnum journalType,
            int journalStatus, 
            InventoryAdjustmentSorting sortBy, 
            bool sortedBackwards)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql +
                                  @"WHERE a.DATAAREAID = @DATAAREAID AND a.JOURNALTYPE = @JOURNALTYPE ";

                if (storeId != RecordIdentifier.Empty)
                {
                    cmd.CommandText += "AND a.STOREID = @STOREID";
                    MakeParam(cmd, "STOREID", (string)storeId);
                
                }

                if (journalStatus == 0 || journalStatus == 1)
                {
                    cmd.CommandText += " AND a.POSTED = @POSTED ";
                    MakeParam(cmd, "POSTED", journalStatus, SqlDbType.Int);
                }
                                    
                cmd.CommandText += " ORDER BY " + ResolveSort(sortBy, sortedBackwards, "", false);
                
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "JOURNALTYPE", (int)journalType);
                

                return Execute<InventoryAdjustment>(entry, cmd, CommandType.Text, PopulateJournal);
            }
        }

        /// <summary>
        /// Saves a given Journal entry (row) into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="inventoryJournal">The journal row to save</param>
        public virtual void Save(IConnectionManager entry, InventoryAdjustment inventoryJournal)
        {
            if (inventoryJournal == null)
            {
                return;
            }

            var statement = new SqlServerStatement("INVENTJOURNALTABLE", false);

            if (inventoryJournal.JournalType == InventoryJournalTypeEnum.Counting)
            {
                ValidateSecurity(entry, BusinessObjects.Permission.StockCounting);
            }
            else
            {
                ValidateSecurity(entry, BusinessObjects.Permission.EditInventoryAdjustments);
            }

            if (inventoryJournal.ID == null || inventoryJournal.ID == "" || inventoryJournal.ID.IsEmpty)
            {
                inventoryJournal.ID = DataProviderFactory.Instance.GenerateNumber<IInventoryAdjustmentData, InventoryAdjustment>(entry);
            }

            if (!Exists(entry, inventoryJournal.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("JOURNALID", (string)inventoryJournal.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("JOURNALID", (string)inventoryJournal.ID);
            }

            statement.AddField("DESCRIPTION", inventoryJournal.Text);
            statement.AddField("POSTED", (int) inventoryJournal.Posted, SqlDbType.Int);
            statement.AddField("POSTEDDATETIME", inventoryJournal.PostedDateTime.ToAxaptaSQLDate(), SqlDbType.DateTime);
            statement.AddField("JOURNALTYPE", (int)inventoryJournal.JournalType, SqlDbType.Int);
            statement.AddField("DELETEPOSTEDLINES", inventoryJournal.DeletePostedLines, SqlDbType.Int);
            statement.AddField("CREATEDDATETIME", inventoryJournal.CreatedDateTime, SqlDbType.DateTime);
            statement.AddField("STOREID", (string)inventoryJournal.StoreId ?? "");
            statement.AddField("CREATEDFROMOMNI", inventoryJournal.CreatedFromOmni, SqlDbType.Bit);
            statement.AddField("TEMPLATEID", (string)inventoryJournal.TemplateID ?? "");
            statement.AddField("PROCESSINGSTATUS", (int)inventoryJournal.ProcessingStatus, SqlDbType.Int);
            
            inventoryJournal.MasterID = inventoryJournal.MasterID == null || inventoryJournal.MasterID.DBType != SqlDbType.UniqueIdentifier ? new Guid((string) inventoryJournal.MasterID) : inventoryJournal.MasterID;
            statement.AddField("MASTERID", (Guid)inventoryJournal.MasterID, SqlDbType.UniqueIdentifier);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SetAdjustmentProcessingStatus(IConnectionManager entry, RecordIdentifier journalId, InventoryProcessingStatus status)
        {
            var statement = new SqlServerStatement("INVENTJOURNALTABLE");
            
            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("JOURNALID", (string)journalId);
            
            statement.AddField("PROCESSINGSTATUS", (int)status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Marks an inventory adjustment as posted
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="journalId"></param>
        public virtual void PostAdjustment(IConnectionManager entry, RecordIdentifier journalId)
        {
            var statement = new SqlServerStatement("INVENTJOURNALTABLE");

            if (Exists(entry, journalId))
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);

                if (journalId.DBType == SqlDbType.UniqueIdentifier)
                {
                    statement.AddCondition("MASTERID", new Guid((string)journalId), SqlDbType.UniqueIdentifier);
                }
                else
                {
                    statement.AddCondition("JOURNALID", (string)journalId);
                }

                statement.AddField("POSTED", (int)InventoryJournalStatus.Posted, SqlDbType.Int);
                statement.AddField("POSTEDDATETIME", Date.Now.ToAxaptaSQLDate(), SqlDbType.DateTime);
                entry.Connection.ExecuteStatement(statement);
            }
        }

        /// <summary>
        /// Call a stored procedure to post an adjustment if there are no more unposted lines
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="journalId">ID of the journal to post</param>
        /// <returns>Result of the operation</returns>
        public virtual PostStockCountingResult PostAdjustmentWithCheck(IConnectionManager entry, RecordIdentifier journalId)
        {
            using (SqlCommand cmd = new SqlCommand("spINVENTORY_PostAdjustment"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MakeParam(cmd, "JOURNALID", journalId.StringValue);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                SqlParameter postingResult = MakeParam(cmd, "POSTINGRESULT", "", SqlDbType.Int, ParameterDirection.Output, 4);

                entry.Connection.ExecuteNonQuery(cmd, false);

                return (PostStockCountingResult)((int)postingResult.Value);
            }
        }

        /// <summary>
        /// Compresses all lines on a stock counting journal, that have the same item and unit
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="journalID">Stock counting journal id</param>
        /// <returns>Operation result</returns>
        public virtual CompressAdjustmentLinesResult CompressAllStockCountingLines(IConnectionManager entry, RecordIdentifier journalID)
        {
            using (SqlCommand cmd = new SqlCommand("spINVENTORY_PostAdjustment"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MakeParam(cmd, "JOURNALID", journalID.StringValue);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                //If true, processing status will be checked and set accordingly, otherwise it will assume that this check is done outside the SP (ex. when calling from PostAllLines SP)
                MakeParam(cmd, "SETPROCESSINGSTATUS", true, SqlDbType.Bit);

                SqlParameter compressResult = MakeParam(cmd, "COMPRESSINGRESULT", "", SqlDbType.Int, ParameterDirection.Output, 4);

                entry.Connection.ExecuteNonQuery(cmd, false);

                return (CompressAdjustmentLinesResult)((int)compressResult.Value);
            }
        }

        /// <summary>
        /// Paginated search through inventory journal based on the given <see cref="InventoryJournalSearch">search criteria object</see>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="journalType"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortBackwards"></param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public virtual List<InventoryAdjustment> AdvancedSearch(
            IConnectionManager entry,
            InventoryJournalTypeEnum journalType,
            InventoryJournalSearch searchCriteria,
            InventoryAdjustmentSorting sortBy,
            bool sortBackwards,
            int rowFrom,
            int rowTo,
            out int totalRecords)
        {
            ValidateSecurity(entry);

            totalRecords = 0;

            List<TableColumn> columns = new List<TableColumn>(stockCountingColumns);
            List<Condition> conditions = new List<Condition>();

            List<Condition> externalConditions = new List<Condition>();

            using (var cmd = entry.Connection.CreateCommand())
            {
                columns.Add(new TableColumn
                {
                    ColumnName = $"ROW_NUMBER() OVER(ORDER BY { ResolveSort(sortBy, sortBackwards, "", false)})",
                    ColumnAlias = "ROW"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = $"COUNT(1) OVER ( ORDER BY { ResolveSort(sortBy, sortBackwards, "", false)} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",

                    ColumnAlias = "ROW_COUNT"
                });

                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "ss.ROW BETWEEN @ROWFROM AND @ROWTO"
                });
                MakeParam(cmd, "ROWFROM", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "ROWTO", rowTo, SqlDbType.Int);

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.JOURNALTYPE = @JOURNALTYPE" });
                MakeParam(cmd, "JOURNALTYPE", (int)journalType);

                if (searchCriteria.Status.HasValue)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.POSTED = @POSTEDSTATUS" });
                    MakeParam(cmd, "POSTEDSTATUS", (int)searchCriteria.Status);
                }

                if (!RecordIdentifier.IsEmptyOrNull(searchCriteria.StoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.STOREID = @STOREID " });
                    MakeParam(cmd, "STOREID", (string)searchCriteria.StoreID);
                }

                if (searchCriteria.Description != null && searchCriteria.Description.Count > 0)
                {
                    List<Condition> searchConditions = new List<Condition>();
                    for (int index = 0; index < searchCriteria.Description.Count; index++)
                    {
                        var searchToken = PreProcessSearchText(searchCriteria.Description[index], true, searchCriteria.DescriptionBeginsWith);

                        if (!string.IsNullOrEmpty(searchToken))
                        {
                            searchConditions.Add(new Condition
                            {
                                ConditionValue =
                                    $@" (A.JOURNALID LIKE @DESCRIPTION{index} 
                                        OR A.DESCRIPTION LIKE @DESCRIPTION{index}) ",
                                Operator = "AND"

                            });

                            MakeParam(cmd, $"DESCRIPTION{index}", searchToken);
                        }
                    }
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue =
                            $" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) "
                    });
                }

                if (searchCriteria.CreatedDateFrom != null && searchCriteria.CreatedDateFrom != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.CREATEDDATETIME >= CONVERT(datetime, @CREATIONDATEFROM, 103)" });
                    MakeParam(cmd, "CREATIONDATEFROM", searchCriteria.CreatedDateFrom.DateTime.Date, SqlDbType.DateTime);
                }

                if (searchCriteria.CreatedDateTo != null && searchCriteria.CreatedDateTo != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.CREATEDDATETIME <= CONVERT(datetime, @CREATIONDATETO, 103)" });
                    MakeParam(cmd, "CREATIONDATETO", searchCriteria.CreatedDateTo.DateTime.Date, SqlDbType.DateTime);
                }

                if (searchCriteria.PostedDateFrom != null && searchCriteria.PostedDateFrom != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.POSTEDDATETIME >= CONVERT(datetime, @POSTEDDATEFROM, 103)" });
                    MakeParam(cmd, "POSTEDDATEFROM", searchCriteria.PostedDateFrom.DateTime.Date, SqlDbType.DateTime);
                }

                if (searchCriteria.PostedDateTo != null && searchCriteria.PostedDateTo != Date.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.POSTEDDATETIME <= CONVERT(datetime, @POSTEDDATETO, 103)" });
                    MakeParam(cmd, "POSTEDDATETO", searchCriteria.PostedDateTo.DateTime.Date, SqlDbType.DateTime);
                }

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("INVENTJOURNALTABLE", "A", "ss"),
                   QueryPartGenerator.ExternalColumnGenerator(columns, "ss"),
                   QueryPartGenerator.InternalColumnGenerator(columns),
                   QueryPartGenerator.JoinGenerator(listJoins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                   QueryPartGenerator.ConditionGenerator(externalConditions),
                   $"ORDER BY {ResolveSort(sortBy, sortBackwards, "ss", true)}");

                return Execute<InventoryAdjustment, int>(entry, cmd, CommandType.Text, ref totalRecords, PopulateJournalInfoWithCount);
            }
        }

        public virtual InventoryAdjustment GetOmniInventoryAdjustmentByTemplate(IConnectionManager entry, RecordIdentifier templateID, RecordIdentifier storeID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + @"WHERE a.JOURNALTYPE = 4 
                                                AND a.POSTED = 0 
                                                AND a.CREATEDFROMOMNI = 1 
                                                AND a.TEMPLATEID = @TEMPLATEID 
                                                AND a.STOREID = @STOREID
                                                AND a.DATAAREAID = @DATAAREAID 
                                                ORDER BY a.CREATEDDATETIME DESC";

                MakeParam(cmd, "TEMPLATEID", (string)templateID);
                MakeParam(cmd, "STOREID", (string)storeID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                var result = Execute<InventoryAdjustment>(entry, cmd, CommandType.Text, PopulateJournal);
                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual bool JournalIsPartiallyPosted(IConnectionManager entry, RecordIdentifier journalID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TOP 1 1 FROM INVENTJOURNALTRANS WHERE JOURNALID = @JOURNALID AND DATAAREAID = @DATAAREAID AND POSTED = 1";

                MakeParam(cmd, "JOURNALID", journalID.StringValue);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

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

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public virtual RecordIdentifier SequenceID
        {
            get { return "InventJournal"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "INVENTJOURNALTABLE", "JOURNALID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
