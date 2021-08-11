using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
    /// <summary>
    /// Data provider class for retail subgroups
    /// </summary>
    public class RetailDivisionData : SqlServerDataProviderBase, IRetailDivisionData
    {
        private List<TableColumn> selectionColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "DIVISIONID", TableAlias = "A"},
            new TableColumn {ColumnName = "ISNULL(A.NAME,'')", TableAlias = "", ColumnAlias = "NAME"},
            new TableColumn {ColumnName = "MASTERID", TableAlias = "A"}
        };
       
        private static string GetSortColumn(RetailDivisionSorting sortEnum)
        {
            var sortColumn = "";

            switch (sortEnum)
            {
                case RetailDivisionSorting.RetailDivisionId:
                    sortColumn = "DIVISIONID";
                    break;
                case RetailDivisionSorting.RetailDivisionName:
                    sortColumn = "NAME";
                    break;
            }          

            return sortColumn;
        }

        private static string ResolveSort(RetailDivisionSorting sortEnum, bool sortBackwards)
        {
            var sortString = " Order By " + GetSortColumn(sortEnum) + " ASC";

            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static void PopulateRetailDivision(IDataReader dr, RetailDivision group)
        {
            group.ID = (string)dr["DIVISIONID"];
            group.Text = (string)dr["NAME"];
            group.MasterID = (Guid)dr["MASTERID"];
        }

        private static void PopulateRetailDivisionMasterID(IDataReader dr, MasterIDEntity group)
        {
            group.ReadadbleID = (string)dr["DIVISIONID"];
            group.Text = (string)dr["NAME"];
            group.ID = (Guid)dr["MASTERID"];
        }

        private static void PopulateListMasterID(IDataReader dr, MasterIDEntity group)
        {
            group.ReadadbleID = (string)dr["GROUPID"];
            group.ID = (Guid)dr["MASTERID"];
            group.Text = (string)dr["NAME"];
        }

        /// <summary>
        /// Gets a list of data entities containing ID and name for each retail group, ordered by a chosen field
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">A sort enum that defines how the result should be sorted </param>
        /// <returns>A list of all retail groups, ordered by a chosen field</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, RetailDivisionSorting sortEnum)
        {
            if (sortEnum != RetailDivisionSorting.RetailDivisionId && sortEnum != RetailDivisionSorting.RetailDivisionName)
            {
                throw new NotSupportedException();
            }

            return GetList<DataEntity>(entry, false, "RETAILDIVISION", "NAME", "DIVISIONID", GetSortColumn(sortEnum),false);
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and names for all retail group, ordered by retail group name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities contaning IDs and names of retail groups, ordered by retail group name</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, false, "RETAILDIVISION", "NAME", "DIVISIONID", "NAME");
        }

        public List<RetailDivision> GetDetailedList(IConnectionManager entry, RetailDivisionSorting sortEnum,
                                                           bool backwardsSort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILDIVISION", "A"),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " }),
                    ResolveSort(sortEnum,backwardsSort)
                    ); 

                return Execute<RetailDivision>(entry, cmd, CommandType.Text, PopulateRetailDivision);
            }
        }

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="MasterIDEntity" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        public List<MasterIDEntity> GetMasterIDList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {

                List<TableColumn> columns = new List<TableColumn>();

                columns.Add(new TableColumn { ColumnName = "MASTERID", TableAlias = "A" });
                columns.Add(new TableColumn { ColumnName = "GROUPID", TableAlias = "A" });
                columns.Add(new TableColumn { ColumnName = "ISNULL(A.NAME,'')", TableAlias = "", ColumnAlias = "NAME" });


                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILGROUP", "A"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);


                return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateListMasterID);
            }
        }

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="RetailDivision" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        public List<RetailDivision> Search(IConnectionManager entry, string searchString,
            int rowFrom, int rowTo,
            bool beginsWith, RetailDivisionSorting sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith); 

                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in selectionColumns)
                {
                    columns.Add(selectionColumn);
                }
                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("ROW_NUMBER() OVER(order by {0})", GetSortColumn(sort)),
                    ColumnAlias = "ROW"
                });
               
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = " (a.NAME Like @SEARCHSTRING or a.DIVISIONID Like @SEARCHSTRING) "
                });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "s.ROW between @rowFrom and @rowTo"
                });

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILDIVISION", "A", "s"),
                    QueryPartGenerator.ExternalColumnGenerator(columns,"s"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    ResolveSort(sort,false));

                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
                MakeParam(cmd, "SEARCHSTRING", modifiedSearchString);

                return Execute<RetailDivision>(entry, cmd, CommandType.Text, PopulateRetailDivision);
            }
        }

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="MasterIDEntity" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        public List<MasterIDEntity> SearchMasterID(IConnectionManager entry, string searchString,
            int rowFrom, int rowTo,
            bool beginsWith, RetailDivisionSorting sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith);

                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in selectionColumns)
                {
                    columns.Add(selectionColumn);
                }
                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("ROW_NUMBER() OVER(order by {0})", GetSortColumn(sort)),
                    ColumnAlias = "ROW"
                });

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = " (a.NAME Like @SEARCHSTRING or a.DIVISIONID Like @SEARCHSTRING) "
                });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "s.ROW between @rowFrom and @rowTo"
                });

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILDIVISION", "A", "s"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "s"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    ResolveSort(sort, false));

                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
                MakeParam(cmd, "SEARCHSTRING", modifiedSearchString);

                return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateRetailDivisionMasterID);
            }
        }



        /// <summary>
        /// Gets a retail department with a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="divisionId">The ID of the retail department to get</param>
        /// <returns>A retail group with a given ID, or null if not found</returns>
        public virtual RetailDivision Get(IConnectionManager entry, RecordIdentifier divisionId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "A.DIVISIONID = @divisionId"
                });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILDIVISION", "A"),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                MakeParam(cmd, "divisionId", (string)divisionId);

                var divs = Execute<RetailDivision>(entry, cmd, CommandType.Text, PopulateRetailDivision);

                return (divs.Count > 0) ? divs[0] : null;
            }
        }

       
        /// <summary>
        /// Checks if a retail group with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="divisionId">The ID of the group to check for</param>
        /// <returns>Whether the given group exists</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier divisionId)
        {
            return RecordExists(entry, "RETAILDIVISION", "DIVISIONID", divisionId,false);
        }

        /// <summary>
        /// Deletes a retail group with a given ID
        /// </summary>
        /// <remarks>
        /// Requires the 'Manage retail groups' permission. 
        /// Always pass a <see cref="System.Guid"/> master ID to prevent any replication issues. Due to backwards compatibility, it accepts non-Guid ID.
        /// </remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="divisionMasterID">The masterID of the retail division to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier divisionMasterID)
        {
            ValidateSecurity(entry, Permission.ManageRetailDivisions);

            var statement = new SqlServerStatement("RETAILDIVISION");

            statement.StatementType = StatementType.Update;

            if (divisionMasterID.IsGuid)
            {
                statement.AddCondition("MASTERID", (Guid)divisionMasterID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.AddCondition("DIVISIONID", (string)divisionMasterID);
            }
            statement.AddField("DELETED", true, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves a given retail department to the database
        /// </summary>
        /// <remarks>Requires the 'Manage retail groups' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="division">The retail group to save</param>
        public virtual void Save(IConnectionManager entry, RetailDivision division)
        {
            var statement = new SqlServerStatement("RETAILDIVISION");
            statement.UpdateColumnOptimizer = division;

            ValidateSecurity(entry, Permission.ManageRetailDivisions);

            division.Validate();

            bool isNew = false;
            if (division.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                division.ID = DataProviderFactory.Instance.GenerateNumber<IRetailDivisionData, RetailDivision>(entry); 
            }

            if (isNew || !Exists(entry, division.ID))
            {
                statement.StatementType = StatementType.Insert;
                if (RecordIdentifier.IsEmptyOrNull(division.MasterID ))
                {
                    division.MasterID = Guid.NewGuid();
                }

                statement.AddKey("MASTERID", (Guid) division.MasterID, SqlDbType.UniqueIdentifier);
                statement.AddField("DIVISIONID", (string)division.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                // This can happen if we are getting the retail group from the Integration Framework, where the external caller does not have information
                // about our master IDs
                if (RecordIdentifier.IsEmptyOrNull(division.MasterID))
                {
                    division.MasterID = GetMasterID(entry, division.ID, "RETAILDIVISION", "DIVISIONID");

                }

                statement.AddCondition("MASTERID", (Guid)division.MasterID, SqlDbType.UniqueIdentifier);
            }
            statement.AddField("NAME", division.Text);
            statement.AddField("MODIFIED", DateTime.Now, SqlDbType.DateTime2);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Undeletes the given division. This set the DELETED flag on the division to 0
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="divisionId">The ID of the retail division to undelete</param>
        public virtual void Undelete(IConnectionManager entry, RecordIdentifier divisionId)
        {
            ValidateSecurity(entry, Permission.ManageRetailDivisions);

            var statement = new SqlServerStatement("RETAILDIVISION");
            statement.StatementType = StatementType.Update;
            statement.AddField("DELETED", false, SqlDbType.Bit);

            statement.AddCondition(divisionId.IsGuid ? "MASTERID" : "DIVISIONID", divisionId.DBValue, divisionId.DBType);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Returns true if the division is marked as deleted
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="divisionId">The ID of the division to check</param>
        /// <returns>True if the division is marked as deleted, false otherwise</returns>
        public bool IsDeleted(IConnectionManager entry, RecordIdentifier divisionId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var idColumnName = divisionId.IsGuid ? "MASTERID" : "DIVISIONID";

                cmd.CommandText =
                    $@"select DELETED
                       from RETAILDIVISION
                       where {idColumnName} = @divisionId";

                MakeParam(cmd, "divisionId", divisionId.DBValue, divisionId.DBType);

                return (bool)entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Gets all the items from the database matching the list of <paramref name="itemsToCompare"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items</param>
        /// <returns>List of items</returns>
        public virtual List<RetailDivision> GetCompareList(IConnectionManager entry, List<RetailDivision> itemsToCompare)
        {
            return GetCompareListInBatches(entry, itemsToCompare, "RETAILDIVISION", "DIVISIONID", selectionColumns, null, PopulateRetailDivision);
        }

        #region ISequenceable Members

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The unique sequence ID to search for</param>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        /// <summary>
        /// Returns a unique ID for the class
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "RETAILDIVISION"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RETAILDIVISION", "DIVISIONID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}