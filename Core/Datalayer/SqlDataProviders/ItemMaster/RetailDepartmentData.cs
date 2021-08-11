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
    /// Data provider class for retail departments
    /// </summary>
    public class RetailDepartmentData : SqlServerDataProviderBase, IRetailDepartmentData
    {
        private List<TableColumn> selectionColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "MASTERID", TableAlias = "dep"},
            new TableColumn {ColumnName = "DEPARTMENTID", TableAlias = "dep"},
            new TableColumn {ColumnName = "NAME", TableAlias = "dep"},
            new TableColumn {ColumnName = "DIVISIONMASTERID", TableAlias = "dep", ColumnAlias = "DIVISIONMASTERID"},
            new TableColumn {ColumnName = "ISNULL(dep.NAMEALIAS,'')", TableAlias = "", ColumnAlias = "NAMEALIAS"},
            new TableColumn {ColumnName = "ISNULL(dep.DIVISIONID,'')", TableAlias = "", ColumnAlias = "DIVISIONID"},
            new TableColumn {ColumnName = "ISNULL(div.NAME,'')", TableAlias = "", ColumnAlias = "DIVISIONNAME"}
        };

        private List<TableColumn> selectionColumnsMinimized = new List<TableColumn>
        {
            new TableColumn {ColumnName = "DEPARTMENTID", TableAlias = "dep"},
            new TableColumn {ColumnName = "NAME", TableAlias = "dep"},
        };

        /// <summary>
        /// Returns the name of the column that the data should be sorted by in a SQL statement
        /// </summary>
        /// <param name="sortEnum">Controls which columns will be sorted by in the SQL statement</param>
        private static string GetSortColumn(RetailDepartment.SortEnum sortEnum)
        {
            var sortColumn = "";

            switch (sortEnum)
            {
                case RetailDepartment.SortEnum.RetailDepartment:
                    sortColumn = "DEPARTMENTID";
                    break;
                case RetailDepartment.SortEnum.Description:
                    sortColumn = "NAME";
                    break;
            }

            return sortColumn;
        }

        /// <summary>
        /// Creates an "order by" string for a SQL statement depending on class settings and
        /// parameters
        /// </summary>
        /// <param name="sortEnum">Controls which columns will be sorted by in the SQL
        /// statement</param>
        /// <param name="sortBackwards">If true then the ordering will be descending
        /// otherwise it will be ascending</param>
        private static string ResolveSort(RetailDepartment.SortEnum sortEnum, bool sortBackwards)
        {
            var sortString = " Order By " + GetSortColumn(sortEnum) + " ASC";

            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static void PopulateListMasterID(IDataReader dr, MasterIDEntity group)
        {
            group.ReadadbleID = (string)dr["DEPARTMENTID"];
            group.ID = (Guid)dr["MASTERID"];
            group.Text = (string)dr["NAME"];
        }

        private static void PopulateDepartmentList(IDataReader dr, DataEntity retailDepartment)
        {
            retailDepartment.ID = (string)dr["DEPARTMENTID"];
            retailDepartment.Text = (string)dr["NAME"];
        }

        private static void PopulateDepartmentListMasterID(IDataReader dr, MasterIDEntity retailDepartment)
        {
            retailDepartment.ID = (Guid)dr["MASTERID"];
            retailDepartment.ReadadbleID = (string)dr["DEPARTMENTID"];
            retailDepartment.Text = (string)dr["NAME"];
        }

        private static void PopulateRetailDepartment(IDataReader dr, RetailDepartment retailDepartment)
        {
            PopulateDepartmentList(dr, retailDepartment);

            retailDepartment.NameAlias = (string)dr["NAMEALIAS"];
            retailDepartment.MasterID = (Guid)dr["MASTERID"];

            retailDepartment.RetailDivisionID = AsString(dr["DIVISIONID"]);
            if (!RecordIdentifier.IsEmptyOrNull(retailDepartment.RetailDivisionID))
            {
                retailDepartment.RetailDivisionMasterID = dr["DIVISIONMASTERID"] == DBNull.Value ? Guid.Empty : (Guid)dr["DIVISIONMASTERID"];
            }
            retailDepartment.RetailDivisionName = AsString(dr["DIVISIONNAME"]);
        }

        private void InternalAddOrRemoveRetailDepartmentFromRetailDivision(IConnectionManager entry, RecordIdentifier departmentID, RecordIdentifier divisionId)
        {
            var statement = new SqlServerStatement("RETAILDEPARTMENT") { StatementType = StatementType.Update };
            var masterID = departmentID.IsGuid ? departmentID : GetMasterID(entry, departmentID);

            statement.AddCondition("MASTERID", (Guid)masterID, SqlDbType.UniqueIdentifier);

            if (RecordIdentifier.IsEmptyOrNull(divisionId))
            {
                statement.AddField("DIVISIONMASTERID", DBNull.Value, SqlDbType.UniqueIdentifier);
                statement.AddField("DIVISIONID", DBNull.Value, SqlDbType.NVarChar);
            }
            else
            {
                statement.AddField("DIVISIONMASTERID", GetMasterID(entry, divisionId, "RETAILDIVISION", "DIVISIONID"), SqlDbType.UniqueIdentifier);
                statement.AddField("DIVISIONID", (string)divisionId);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Get a list of all retail department, sordered by a given sort column index and a revers ordering based on a parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">Enum indicating the column to order the data by</param>
        /// <param name="backwardsSort">Whether to reverse the result set or not</param>
        /// <returns>A list of retail departments meeting the above criteria</returns>
        public virtual List<RetailDepartment> GetDetailedList(IConnectionManager entry, RetailDepartment.SortEnum sortEnum, bool backwardsSort)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Join> joins = new List<Join>();
               
                joins.Add(new Join { Condition = " dep.DIVISIONMASTERID = div.MASTERID AND DIV.DELETED = 0", JoinType = "LEFT OUTER", Table = "RETAILDIVISION", TableAlias = "div" });
                
                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILDEPARTMENT", "dep"),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(new Condition { Operator = "AND", ConditionValue = "dep.DELETED = 0 " }),
                    ResolveSort(sortEnum, backwardsSort)
                    );

                return Execute<RetailDepartment>(entry, cmd, CommandType.Text, PopulateRetailDepartment);
            }
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all retail departments, sorted by a given field.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sort">The field to sort by</param>
        /// <returns>A list of data entities containing IDs and names of all retail departments</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, RetailDepartment.SortEnum sort)
        {
            return GetList<DataEntity>(entry, false, "RETAILDEPARTMENT", "NAME", "DEPARTMENTID", GetSortColumn(sort),false);
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all retail departments, ordered by retail departments name.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities containing IDs and names of all retail departments</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList(entry, RetailDepartment.SortEnum.Description);
        }

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="DataEntity" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="divisionId">Id of the division, if any</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        public List<DataEntity> Search(IConnectionManager entry, string searchString, 
            RecordIdentifier divisionId,
            int rowFrom, int rowTo, bool beginsWith, RetailDepartment.SortEnum sort)
        {
            List<TableColumn> columns = new List<TableColumn>();
            //List<Condition> conditions = new List<Condition>();
            List<Condition> externalConditions = new List<Condition>();
            List<Join> joins = new List<Join>();
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                var modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith);

                foreach (var selectionColumn in selectionColumnsMinimized)
                {
                    columns.Add(selectionColumn);
                }

                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("ROW_NUMBER() OVER(order by dep.{0})", GetSortColumn(sort)),
                    ColumnAlias = "ROW"
                });
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "dep.DELETED = 0 " });

                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = " (dep.NAME Like @searchString or dep.DEPARTMENTID Like @searchString ) "
                });
              
                if (!(divisionId == null || string.IsNullOrEmpty((string)divisionId)))
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = " (dep.DIVISIONID = @divisionId ) "
                    });
                    MakeParam(cmd, "divisionId", (string) divisionId);
                }
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "s.ROW between @rowFrom and @rowTo"
                });

                joins.Add(new Join { Condition = " dep.DIVISIONMASTERID = div.MASTERID AND DIV.DELETED =0", JoinType = "LEFT OUTER", Table = "RETAILDIVISION", TableAlias = "div" });

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILDEPARTMENT", "dep", "s"),
                    QueryPartGenerator.ExternalColumnGenerator(columns,"s"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.Empty);

                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
                MakeParam(cmd, "searchString", modifiedSearchString);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, PopulateDepartmentList);
            }
        }

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="DataEntity" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="divisionId">Id of the division, if any</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        public List<MasterIDEntity> SearchWithMasterID(IConnectionManager entry, string searchString,
            RecordIdentifier divisionId,
            int rowFrom, int rowTo, bool beginsWith, RetailDepartment.SortEnum sort)
        {
            List<TableColumn> columns = new List<TableColumn>();
            //List<Condition> conditions = new List<Condition>();
            List<Condition> externalConditions = new List<Condition>();
            List<Join> joins = new List<Join>();
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                var modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith); 

                columns.Add(new TableColumn { ColumnName = "MASTERID", TableAlias = "dep" });
                columns.Add(new TableColumn { ColumnName = "DEPARTMENTID", TableAlias = "dep" });
                columns.Add(new TableColumn { ColumnName = "NAME", TableAlias = "dep" });

                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("ROW_NUMBER() OVER(order by dep.{0})", GetSortColumn(sort)),
                    ColumnAlias = "ROW"
                });
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "dep.DELETED = 0 " });

                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = " (dep.NAME Like @searchString or dep.DEPARTMENTID Like @searchString ) "
                });

                if (!(divisionId == null || string.IsNullOrEmpty((string)divisionId)))
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = " (dep.DIVISIONID = @divisionId ) "
                    });
                    MakeParam(cmd, "divisionId", (string)divisionId);
                }
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "s.ROW between @rowFrom and @rowTo"
                });

                joins.Add(new Join { Condition = " dep.DIVISIONMASTERID = div.MASTERID AND DIV.DELETED =0", JoinType = "LEFT OUTER", Table = "RETAILDIVISION", TableAlias = "div" });

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILDEPARTMENT", "dep", "s"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "s"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.Empty);

                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
                MakeParam(cmd, "searchString", modifiedSearchString);

                return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateDepartmentListMasterID);
            }
        }

        /// <summary>
        /// Checks if a retail department with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentId">The ID of the retail department to check for</param>
        /// <returns>Whether a retail department with a given ID exists</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier departmentId)
        {
            return RecordExists(entry, "RETAILDEPARTMENT", "DEPARTMENTID", departmentId,false);
        }

        /// <summary>
        /// Deletes a retail department with a given ID
        /// </summary>
        /// <remarks>Requires the 'Manage retail departments' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentId">The ID of the retail department to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier departmentId)
        {
            ValidateSecurity(entry, Permission.ManageRetailDepartments);
            var statement = new SqlServerStatement("RETAILDEPARTMENT");

            statement.StatementType = StatementType.Update;

            var masterID = departmentId.IsGuid ? departmentId : GetMasterID(entry, departmentId);

            statement.AddCondition("MASTERID", (Guid)masterID, SqlDbType.UniqueIdentifier);
            statement.AddField("DELETED", true, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Gets a retail department with a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentId">The ID of the retail department to get</param>
        /// <returns>A retail department with a given ID, or null if not found</returns>
        public virtual RetailDepartment Get(IConnectionManager entry, RecordIdentifier departmentId)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Join> joins = new List<Join>(); List<Condition> conditions = new List<Condition>();

                if (departmentId.IsGuid)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = " dep.MASTERID = @departmentid " });

                    MakeParam(cmd, "departmentid", (Guid)departmentId, SqlDbType.UniqueIdentifier);
                }
                else
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = " dep.DEPARTMENTID = @departmentid " });

                    MakeParam(cmd, "departmentid", (string)departmentId);
                }

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "dep.DELETED = 0 " });

                joins.Add(new Join { Condition = " dep.DIVISIONID = div.DIVISIONID AND DIV.DELETED =0", JoinType = "LEFT OUTER", Table = "RETAILDIVISION", TableAlias = "div" });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILDEPARTMENT", "dep"),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                var result = Execute<RetailDepartment>(entry, cmd, CommandType.Text, PopulateRetailDepartment);
                return (result.Count > 0) ? result[0] : null;
            }
        }

        /// <summary>
        /// Saves a given retail department to the database
        /// </summary>
        /// <remarks>Requires the 'Manage retail departments' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="retailDepartment">The retail department to save</param>
        public virtual void Save(IConnectionManager entry, RetailDepartment retailDepartment)
        {
            var statement = new SqlServerStatement("RETAILDEPARTMENT");
            statement.UpdateColumnOptimizer = retailDepartment;

            ValidateSecurity(entry, BusinessObjects.Permission.ManageRetailDepartments);

            bool isNew = false;
            if (retailDepartment.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                retailDepartment.ID = DataProviderFactory.Instance.GenerateNumber<IRetailDepartmentData, RetailDepartment>(entry);
            }

            if (isNew || !Exists(entry, retailDepartment.ID))
            {
                statement.StatementType = StatementType.Insert;

                if (RecordIdentifier.IsEmptyOrNull(retailDepartment.MasterID ))
                {
                    retailDepartment.MasterID = Guid.NewGuid();
                }
                statement.AddKey("MASTERID", (Guid)retailDepartment.MasterID, SqlDbType.UniqueIdentifier);

                statement.AddField("DEPARTMENTID", (string)retailDepartment.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                // This can happen if we are getting the retail item from the Integration Framework, where the external caller does not have information
                // about our master IDs
                if (RecordIdentifier.IsEmptyOrNull(retailDepartment.MasterID))
                {
                    retailDepartment.MasterID = GetMasterID(entry, retailDepartment.ID);
                }

                statement.AddCondition("MASTERID", (Guid)retailDepartment.MasterID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("NAME", retailDepartment.Text);
            statement.AddField("NAMEALIAS", retailDepartment.NameAlias);

            if (RecordIdentifier.IsEmptyOrNull(retailDepartment.RetailDivisionMasterID) && 
                !RecordIdentifier.IsEmptyOrNull(retailDepartment.RetailDivisionID))
            {
                retailDepartment.RetailDivisionMasterID = GetMasterID(entry, retailDepartment.RetailDivisionID, "RETAILDIVISION", "DIVISIONID");
            }

            if (RecordIdentifier.IsEmptyOrNull(retailDepartment.RetailDivisionMasterID) || (Guid)retailDepartment.RetailDivisionMasterID == Guid.Empty)
            {
                statement.AddField("DIVISIONMASTERID", DBNull.Value, SqlDbType.UniqueIdentifier);
                statement.AddField("DIVISIONID", DBNull.Value, SqlDbType.NVarChar);
            }
            else
            {
                statement.AddField("DIVISIONMASTERID", (Guid)retailDepartment.RetailDivisionMasterID, SqlDbType.UniqueIdentifier);
                statement.AddField("DIVISIONID", (string)retailDepartment.RetailDivisionID);
            }

            statement.AddField("MODIFIED", DateTime.Now, SqlDbType.DateTime2);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Adds or removes a retail department with a given Id from the retail division it is currently in.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentID">The ID of the department to be removed</param>
        /// <param name="divisionId">The ID of the division - null if department should be removed from division</param>
        public virtual void AddOrRemoveRetailDepartmentFromRetailDivision(IConnectionManager entry, RecordIdentifier departmentID, RecordIdentifier divisionId)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ItemsEdit);
            InternalAddOrRemoveRetailDepartmentFromRetailDivision(entry, departmentID, divisionId);
        }

        /// <summary>
        /// Adds or removes multiple departments from the retail division.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentIDs">The IDs of the departments to be removed</param>
        /// <param name="divisionId">The ID of the division - null if department should be removed from division</param>
        public virtual void AddOrRemoveRetailDepartmentsFromRetailDivision(IConnectionManager entry, List<RecordIdentifier> departmentIDs, RecordIdentifier divisionId)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ItemsEdit);
            foreach(RecordIdentifier departmentID in departmentIDs)
            {
                InternalAddOrRemoveRetailDepartmentFromRetailDivision(entry, departmentID, divisionId);
            }
        }

        /// <summary>
        /// Get a list of retail items not in a selected retail group, that contain a given search text.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchText">The text to search for. Searches in item name, the ID field and the name alias field</param>
        /// <param name="numberOfRecords">The number of items to return. Items are ordered by retail item name</param>
        /// <param name="excludedDivisionId">ID of the retail group which the items are not supposed to be in</param>
        /// <returns>A list of items meeting the above criteria</returns>
        public List<RetailDepartment> DepartmentsNotInRetailDivision(
            IConnectionManager entry,
            string searchText,
            int numberOfRecords,
            RecordIdentifier excludedDivisionId)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Join> joins = new List<Join>();
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "dep.DELETED = 0 " });


                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = " (dep.DIVISIONID <> @excludedDivisionId OR dep.DIVISIONID is null) "
                });

                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = " (dep.DEPARTMENTID like @searchString or dep.NAME like @searchString or dep.NAMEALIAS like @searchString) "
                });
                joins.Add(new Join { Condition = " dep.DIVISIONID = div.DIVISIONID AND DIV.DELETED =0", JoinType = "LEFT OUTER", Table = "RETAILDIVISION", TableAlias = "div" });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILDEPARTMENT", "dep",numberOfRecords),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                     "ORDER BY dep.NAME"
                    );

                MakeParam(cmd, "excludedDivisionId", (string)excludedDivisionId);
                MakeParam(cmd, "searchString", searchText + "%");

                return Execute<RetailDepartment>(entry, cmd, CommandType.Text, PopulateRetailDepartment);
            }
        }

        public List<RetailDepartment> DepartmentsInDivision(IConnectionManager entry, RecordIdentifier divisionID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Join> joins = new List<Join>();
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "dep.DELETED = 0 " });


                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = " dep.DIVISIONID = @divisionId OR dep.DIVISIONID is null "
                });

                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = " (dep.DEPARTMENTID like @searchString or dep.NAME like @searchString or dep.NAMEALIAS like @searchString) "
                });
                joins.Add(new Join { Condition = " dep.DIVISIONID = div.DIVISIONID AND DIV.DELETED =0", JoinType = "LEFT OUTER", Table = "RETAILDIVISION", TableAlias = "div" });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILDEPARTMENT", "dep"),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                     "ORDER BY dep.NAME"
                    );

                MakeParam(cmd, "divisionId", divisionID);

                return Execute<RetailDepartment>(entry, cmd, CommandType.Text, PopulateRetailDepartment);
            }
        }

        public Guid GetMasterID(IConnectionManager entry, RecordIdentifier ID)
        {
            return GetMasterID(entry, ID, "RETAILDEPARTMENT", "DEPARTMENTID");
        }

        public string GetReadableID(IConnectionManager entry, RecordIdentifier ID)
        {
            return GetReadableIDFromMasterID(entry, ID, "RETAILDEPARTMENT", "DEPARTMENTID");
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
                columns.Add(new TableColumn { ColumnName = "DEPARTMENTID", TableAlias = "A" });
                columns.Add(new TableColumn { ColumnName = "ISNULL(A.NAME,'')", TableAlias = "", ColumnAlias = "NAME" });


                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILDEPARTMENT", "A"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);


                return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateListMasterID);
            }
        }

        /// <summary>
        /// Undeletes the department. This set the DELETED flag on the department to 0
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="departmentId">The ID of the retail department to undelete</param>
        public virtual void Undelete(IConnectionManager entry, RecordIdentifier departmentId)
        {
            ValidateSecurity(entry, Permission.ManageRetailDepartments);

            var statement = new SqlServerStatement("RETAILDEPARTMENT");
            statement.StatementType = StatementType.Update;
            statement.AddField("DELETED", false, SqlDbType.Bit);

            var masterID = departmentId.IsGuid ? departmentId : GetMasterID(entry, departmentId);
            statement.AddCondition("MASTERID", (Guid)masterID, SqlDbType.UniqueIdentifier);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Returns true if the department is marked as deleted
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="departmentId">The ID of the department to check</param>
        /// <returns>True if the department is marked as deleted, false otherwise</returns>
        public bool IsDeleted(IConnectionManager entry, RecordIdentifier departmentId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"select DELETED
                       from RETAILDEPARTMENT
                       where MASTERID = @masterID";

                var masterID = departmentId.IsGuid ? departmentId : GetMasterID(entry, departmentId);
                MakeParam(cmd, "masterID", (Guid)masterID, SqlDbType.UniqueIdentifier);

                return (bool)entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Gets all the items from the database matching the list of <paramref name="itemsToCompare"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items </param>
        /// <returns>List of items</returns>
        public virtual List<RetailDepartment> GetCompareList(IConnectionManager entry, List<RetailDepartment> itemsToCompare)
        {
            List<Join> joins = new List<Join>();

            joins.Add(new Join { Condition = " dep.DIVISIONMASTERID = div.MASTERID AND DIV.DELETED = 0", JoinType = "LEFT OUTER", Table = "RETAILDIVISION", TableAlias = "div" });

            return GetCompareListInBatches(entry, itemsToCompare, "RETAILDEPARTMENT", "DEPARTMENTID", selectionColumns, joins, PopulateRetailDepartment);
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
            get { return "RETAILDEPARTMENT"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RETAILDEPARTMENT", "DEPARTMENTID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}