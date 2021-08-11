using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    /// Data provider class for retail groups
    /// </summary>
    public class RetailGroupData : SqlServerDataProviderBase, IRetailGroupData
    {
        private static  List<TableColumn> itemColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "ITEMID", TableAlias = "A"},
            new TableColumn {ColumnName = "ItemName", TableAlias = "A"},
        };

        private static List<TableColumn> listColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "MASTERID", TableAlias = "A"},
            new TableColumn {ColumnName = "GROUPID", TableAlias = "A"},
            new TableColumn {ColumnName = "TAREWEIGHT", TableAlias = "A"},
            new TableColumn {ColumnName = "ISNULL(A.NAME,'')", TableAlias = "", ColumnAlias = "NAME"},
            new TableColumn {ColumnName = "ISNULL(IID.DEPARTMENTID,'')", TableAlias = "", ColumnAlias = "RETAILDEPARTMENTID"},
            new TableColumn {ColumnName = "ISNULL(IID.NAME,'')", TableAlias = "", ColumnAlias = "RETAILDEPARTMENTNAME"},
            new TableColumn {ColumnName = "DEPARTMENTMASTERID", TableAlias = "A", ColumnAlias = "DEPARTMENTMASTERID"},
        };

        private static  List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = " a.DEPARTMENTMASTERID = iid.MASTERID AND IID.DELETED =0",
                JoinType = "LEFT OUTER",
                Table = "RETAILDEPARTMENT",
                TableAlias = "IID"
            },
        };

        private static  List<TableColumn> selectionColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "MASTERID", TableAlias = "A"},
            new TableColumn {ColumnName = "GROUPID", TableAlias = "A"},
            new TableColumn {ColumnName = "ISNULL(A.NAME,'')", TableAlias = "", ColumnAlias = "NAME"},
            new TableColumn {ColumnName = "ISNULL(A.DEFAULTPROFIT,0)", TableAlias = "", ColumnAlias = "DEFAULTPROFIT"},
            new TableColumn {ColumnName = "ISNULL(A.POSPERIODICID,'')", TableAlias = "", ColumnAlias = "POSPERIODICID"},
            new TableColumn {ColumnName = "A.TAREWEIGHT", TableAlias = "", ColumnAlias = "TAREWEIGHT"},
            new TableColumn {ColumnName = "DEPARTMENTMASTERID", TableAlias = "A", ColumnAlias = "DEPARTMENTMASTERID"},
            new TableColumn {ColumnName = "ISNULL(iid.DEPARTMENTID,'')", TableAlias = "", ColumnAlias = "RETAILDEPARTMENTID"},
            new TableColumn {ColumnName = "ISNULL(iid.NAME,'')", TableAlias = "", ColumnAlias = "RETAILDEPARTMENTNAME"},
            new TableColumn {ColumnName = "ISNULL(tigh.TAXITEMGROUP,'')", TableAlias = "", ColumnAlias = "TAXITEMGROUPID"},
            new TableColumn {ColumnName = "ISNULL(tigh.NAME,'')", TableAlias = "", ColumnAlias = "TAXITEMGROUPNAME"},
            new TableColumn {ColumnName = "ISNULL(PDV.DESCRIPTION, '')", TableAlias = "", ColumnAlias = "VALIDATIONPERIODDISCOUNTDESCRIPTION"}
        };

        private static  List<Join> fixedJoins = new List<Join>
        {
            new Join
            {
                Condition = " a.DEPARTMENTMASTERID = iid.MASTERID AND IID.DELETED =0",
                JoinType = "LEFT OUTER",
                Table = "RETAILDEPARTMENT",
                TableAlias = "iid"
            },
            new Join
            {
                Condition = " a.SALESTAXITEMGROUP = tigh.TAXITEMGROUP",
                JoinType = "LEFT OUTER",
                Table = "TAXITEMGROUPHEADING",
                TableAlias = "tigh"
            },
            new Join
            {
                Condition = " a.POSPERIODICID = PDV.ID",
                JoinType = "LEFT OUTER",
                Table = "POSDISCVALIDATIONPERIOD",
                TableAlias = "pdv"
            },
        };
  
        private static string GetSortColumn(RetailGroupSorting sortEnum, bool advancedSearch = false, string tableAlias = "")
        {
            var sortColumn = "";
            tableAlias = tableAlias == "" ? tableAlias : tableAlias + ".";

            switch (sortEnum)
            {
                case RetailGroupSorting.RetailGroupId:
                    sortColumn = tableAlias + "GROUPID";
                    break;
                case RetailGroupSorting.RetailGroupName:
                    sortColumn = tableAlias + "NAME";
                    break;
                case RetailGroupSorting.RetailDepartmentName:
                    sortColumn = advancedSearch ? "iid.NAME" : "RetailDepartmentName";
                    break;
            }          

            return sortColumn;
        }

        private static string ResolveSort(RetailGroupSorting sortEnum, bool sortBackwards, bool advancedSearch = false, string tableAlias = "")
        {
            var sortString = " Order By " + GetSortColumn(sortEnum, advancedSearch, tableAlias) + " ASC";

            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static void PopulateDetailedList(IDataReader dr, RetailGroup group)
        {
            group.ID = (string)dr["GROUPID"];
            group.MasterID = (Guid) dr["MASTERID"];
            group.Text = (string)dr["NAME"];
            group.TareWeight = (int)dr["TAREWEIGHT"];
            group.RetailDepartmentID = (string)dr["RETAILDEPARTMENTID"];
            if (!RecordIdentifier.IsEmptyOrNull(group.RetailDepartmentID))
            {
                group.RetailDepartmentMasterID = dr["DEPARTMENTMASTERID"] == DBNull.Value ?Guid.Empty: (Guid) dr["DEPARTMENTMASTERID"];
            }
            group.RetailDepartmentName = (string)dr["RETAILDEPARTMENTNAME"];
        }

        private static void PopulateListMasterID(IDataReader dr, MasterIDEntity group)
        {
            group.ReadadbleID = (string)dr["GROUPID"];
            group.ID = (Guid)dr["MASTERID"];
            group.Text = (string)dr["NAME"];
        }

        private static void PopulateRetailGroup(IDataReader dr, RetailGroup group)
        {
            PopulateDetailedList(dr,group);
            group.ItemSalesTaxGroupId = (string)dr["TaxItemGroupId"];
            group.ItemSalesTaxGroupName = (string)dr["TaxItemGroupName"];
            group.ProfitMargin = (dr["DEFAULTPROFIT"] != DBNull.Value) ? Convert.ToDecimal(dr["DEFAULTPROFIT"]) : 0;
            group.ValidationPeriod = (string)dr["POSPERIODICID"];
            group.ValidationPeriodDescription = (string)dr["VALIDATIONPERIODDISCOUNTDESCRIPTION"];
            group.TareWeight = (int)dr["TAREWEIGHT"];
        }

        protected static void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 || entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
        }

        private static void PopulateRetailGroupWithCount(IConnectionManager entry, IDataReader dr, RetailGroup group, ref int rowCount)
        {
            PopulateRowCount(entry, dr, ref rowCount);
            PopulateDetailedList(dr, group);
            group.ItemSalesTaxGroupId = (string)dr["TaxItemGroupId"];
            group.ItemSalesTaxGroupName = (string)dr["TaxItemGroupName"];
            group.ProfitMargin = (dr["DEFAULTPROFIT"] != DBNull.Value) ? Convert.ToDecimal(dr["DEFAULTPROFIT"]) : 0;
            group.ValidationPeriod = (string)dr["POSPERIODICID"];
            group.ValidationPeriodDescription = (string)dr["VALIDATIONPERIODDISCOUNTDESCRIPTION"];
            group.TareWeight = (int)dr["TAREWEIGHT"];
        }

        private static void PopulateItemInGroup(IDataReader dr, ItemInGroup item)
        {
            item.ID = (string)dr["ITEMID"];
            item.Text = (string)dr["ITEMNAME"];
            item.Group = (string)dr["GROUPNAME"];
            item.VariantName = (string)dr["VARIANTNAME"];
        }

        private void InternalAddRetailItemToGroup(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier groupId)
        {
            var masterId = GetMasterID(entry, itemId, "RETAILITEM", "ITEMID");
            var statement = new SqlServerStatement("RETAILITEM");
            var group = Get(entry, groupId);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("MASTERID", (Guid)masterId, SqlDbType.UniqueIdentifier);
            statement.AddField("RETAILGROUPMASTERID", GetMasterID(entry, groupId, "RETAILGROUP", "GROUPID"), SqlDbType.UniqueIdentifier);
            statement.AddField("VALIDATIONPERIODID", group.ValidationPeriod);

            entry.Connection.ExecuteStatement(statement);
        }

        private void InternalAddRetailGroupToRetailDepartment(IConnectionManager entry, RecordIdentifier retailGroupID, RecordIdentifier retailDepartmentID)
        {
            RetailGroup retailGroup = Get(entry, retailGroupID);
            retailGroup.RetailDepartmentID = (string)retailDepartmentID;
            retailGroup.RetailDepartmentMasterID = GetMasterID(entry, retailDepartmentID, "RETAILDEPARTMENT", "DEPARTMENTID");
            retailGroup.MasterID = GetMasterID(entry, retailGroupID, "RETAILGROUP", "GROUPID");
            Save(entry, retailGroup);
        }

        /// <summary>
        /// Gets a list of data entities containing ID and name for each retail group, ordered by a chosen field
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">A sort enum that defines how the result should be sorted </param>
        /// <returns>A list of all retail groups, ordered by a chosen field</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, RetailGroupSorting sortEnum)
        {
            if (sortEnum != RetailGroupSorting.RetailGroupId && sortEnum != RetailGroupSorting.RetailGroupName)
            {
                throw new NotSupportedException();
            }

            return GetList<DataEntity>(entry, false, "RETAILGROUP", "NAME", "GROUPID", GetSortColumn(sortEnum),false);
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and names for all retail group, ordered by retail group name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities contaning IDs and names of retail groups, ordered by retail group name</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, false, "RETAILGROUP", "NAME", "GROUPID", "NAME",false);
        }

        /// <summary>
        /// Gets a list of retail groups, ordered by a given sort column index and a reversed ordering based on a parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">A sort enum that defines how the result should be sorted</param>
        /// <param name="backwardsSort">Whether to reverse the result set or not</param>
        /// <returns>A list of retail groups meeting the above criteria</returns>
        public virtual List<RetailGroup> GetDetailedList(IConnectionManager entry, RetailGroupSorting sortEnum, bool backwardsSort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                
                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILGROUP", "A"),
                    QueryPartGenerator.InternalColumnGenerator(listColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " }),
                    string.Empty
                    );

                return Execute<RetailGroup>(entry, cmd, CommandType.Text, PopulateDetailedList);
            }
        }

        /// <summary>
        /// Gets all the items from the database matching the list of <paramref name="itemsToCompare"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items</param>
        /// <returns>List of items</returns>
        public virtual List<RetailGroup> GetCompareList(IConnectionManager entry, List<RetailGroup> itemsToCompare)
        {
            return GetCompareListInBatches(entry, itemsToCompare, "RETAILGROUP", "GROUPID", selectionColumns, fixedJoins, PopulateRetailGroup);
        }

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="RetailGroup" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="retailDepartmentID">The unique ID of the department to search
        /// for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        public List<RetailGroup> Search(
            IConnectionManager entry, 
            string searchString,
            RecordIdentifier retailDepartmentID, 
            int rowFrom, 
            int rowTo,
            bool beginsWith, 
            RetailGroupSorting sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith);

                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in listColumns)
                {
                    columns.Add(selectionColumn);
                }
                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("ROW_NUMBER() OVER(ORDER BY A.{0})", GetSortColumn(sort)),
                    ColumnAlias = "ROW"
                });

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = " (A.NAME Like @SEARCHSTRING or A.GROUPID Like @SEARCHSTRING) "
                });

                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "S.ROW BETWEEN @rowFrom AND @rowTo"
                });

                List<Join> joins = new List<Join>();
                joins.Add(new Join {
                    Condition = " IID.DEPARTMENTID = a.DEPARTMENTID AND IID.DELETED =0",
                    JoinType = (string)retailDepartmentID != "" ? "INNER": "LEFT OUTER",
                    Table = "RETAILDEPARTMENT",
                    TableAlias = "IID" });

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILGROUP", "A", "S"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.Empty);

                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
                MakeParam(cmd, "SEARCHSTRING", modifiedSearchString);

                return Execute<RetailGroup>(entry, cmd, CommandType.Text, PopulateDetailedList);
            }
        }

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="MasterIDEntity" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="retailDepartmentID">The unique ID of the department to search
        /// for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        public List<MasterIDEntity> SearchMasterID(
            IConnectionManager entry,
            string searchString,
            RecordIdentifier retailDepartmentID,
            int rowFrom,
            int rowTo,
            bool beginsWith,
            RetailGroupSorting sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith);

                List<TableColumn> columns = new List<TableColumn>();

                columns.Add(new TableColumn { ColumnName = "MASTERID", TableAlias = "A" });
                columns.Add(new TableColumn { ColumnName = "GROUPID", TableAlias = "A" });
                columns.Add(new TableColumn { ColumnName = "ISNULL(A.NAME,'')", TableAlias = "", ColumnAlias = "NAME" });

                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("ROW_NUMBER() OVER(ORDER BY A.{0})", GetSortColumn(sort)),
                    ColumnAlias = "ROW"
                });

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = " (A.NAME Like @SEARCHSTRING or A.GROUPID Like @SEARCHSTRING) "
                });

                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "S.ROW BETWEEN @rowFrom AND @rowTo"
                });

                List<Join> joins = new List<Join>();
                joins.Add(new Join
                {
                    Condition = " IID.DEPARTMENTID = a.DEPARTMENTID AND IID.DELETED =0",
                    JoinType = (string)retailDepartmentID != "" ? "INNER" : "LEFT OUTER",
                    Table = "RETAILDEPARTMENT",
                    TableAlias = "IID"
                });

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILGROUP", "A", "S"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.Empty);

                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
                MakeParam(cmd, "SEARCHSTRING", modifiedSearchString);

                return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateListMasterID);
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
                    "ORDER BY NAME");


                return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateListMasterID);
            }
        }

        public List<RetailGroup> AdvancedSearch(
            IConnectionManager entry,
            int rowFrom, 
            int rowTo,
            RetailGroupSorting sort,
            bool sortBackwards,
            out int totalRecordsMatching,
            string idOrDescription = null,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier retailDepartmentID = null,
            RecordIdentifier taxGroupID = null,
            string validationPeriod = null)
        {
            
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in selectionColumns)
                {
                    columns.Add(selectionColumn);
                }
                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("ROW_NUMBER() OVER({0})", ResolveSort(sort, sortBackwards, true, "A")),
                    ColumnAlias = "ROW"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("COUNT(1) OVER ( {0} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )", ResolveSort(sort, sortBackwards, true, "A")),
                    ColumnAlias = "ROW_COUNT"
                });

                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "S.ROW between @rowFrom and @rowTo"
                });
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

                if (idOrDescription != null && idOrDescription.Trim().Length > 0)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "(a.NAME Like @searchString or a.GROUPID Like @searchString )"
                    });
                    idOrDescription = PreProcessSearchText(idOrDescription, true, idOrDescriptionBeginsWith);

                    MakeParamNoCheck(cmd, "searchString", idOrDescription);
                }
                if (retailDepartmentID != null)
                {
                    conditions.Add(new Condition {Operator = "AND", ConditionValue = "iid.DEPARTMENTID = @departmentID"});

                    MakeParamNoCheck(cmd, "departmentID", (string)retailDepartmentID);
                }
                if (taxGroupID != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "tigh.TAXITEMGROUP = @taxGroupID" });

                    MakeParamNoCheck(cmd, "taxGroupID", (string)taxGroupID);
                }
             
                if (validationPeriod != null && validationPeriod.Trim().Length > 0)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "PDV.ID = @validationPeriod" });

                    MakeParamNoCheck(cmd, "validationPeriod", validationPeriod);
                }

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILGROUP","A", "S"),
                      QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
                      QueryPartGenerator.InternalColumnGenerator(columns),
                      QueryPartGenerator.JoinGenerator(fixedJoins),
                      QueryPartGenerator.ConditionGenerator(conditions),
                      QueryPartGenerator.ConditionGenerator(externalConditions),
                      ResolveSort(sort, sortBackwards, false, "S"));

                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);

                totalRecordsMatching = 0;

                return Execute<RetailGroup, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateRetailGroupWithCount);

            }
        }

        public Guid GetMasterID(IConnectionManager entry, RecordIdentifier ID)
        {
            return GetMasterID(entry, ID, "RETAILGROUP", "GROUPID");
        }

        public string GetReadableID(IConnectionManager entry, RecordIdentifier ID)
        {
            return GetReadableIDFromMasterID(entry, ID, "RETAILGROUP", "GROUPID");
        }

        /// <summary>
        /// Gets a list of data entities containing ID and name of a retail items in a given retail group. The list is
        /// ordered by retail item names and items numbered between recordFrom and recordTo will be returned.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupId">Id of the retail group this can be either ID or MasterID</param>
        /// <param name="recordFrom">The result number of the first item to be retrieved</param>
        /// <param name="recordTo">The result number of the last item to be retrieved</param>
        /// <returns>A list of data entities containing IDs and names of retail items in a given retail group meeting the above criteria</returns>
        public List<ItemInGroup> ItemsInGroup(
            IConnectionManager entry, 
            RecordIdentifier groupId, 
            int recordFrom,
            int recordTo)
        {
            ValidateSecurity(entry);

            if(!groupId.IsGuid)
            {
                groupId = GetMasterID(entry, groupId);

                if (groupId == Guid.Empty)
                {
                    return new List<ItemInGroup>();
                }
            }
               
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in itemColumns)
                {
                    columns.Add(selectionColumn);
                }

                columns.Add(new TableColumn { ColumnName = "VARIANTNAME", TableAlias = "A" });
                columns.Add(new TableColumn { ColumnName = "''", ColumnAlias = "GROUPNAME" });
                columns.Add(new TableColumn
                {
                    ColumnName = "ROW_NUMBER() OVER(order by ITEMNAME)", 
                    ColumnAlias = "ROW"
                });

                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "item.ROW between @RECORDFROM and @RECORDTO"
                });
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.RETAILGROUPMASTERID = @GROUPID " });

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILITEM", "A", "item"),
                     QueryPartGenerator.ExternalColumnGenerator(columns, "item"),
                     QueryPartGenerator.InternalColumnGenerator(columns),
                     string.Empty,
                     QueryPartGenerator.ConditionGenerator(conditions),
                     QueryPartGenerator.ConditionGenerator(externalConditions),
                     string.Empty);


                MakeParam(cmd, "GROUPID", (Guid)groupId, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "RECORDFROM", recordFrom, SqlDbType.Int);
                MakeParam(cmd, "RECORDTO", recordTo, SqlDbType.Int);

                return Execute<ItemInGroup>(entry, cmd, CommandType.Text, PopulateItemInGroup);
            }
        }

        /// <summary>
        /// Gets a list of data entities containing ID and name of a retail items in a given retail group. The list is
        /// ordered by retail item names and items numbered between recordFrom and recordTo will be returned.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupId">Id of the retail group this can be either normal ID or MasterID</param>
        /// <returns>A list of data entities containing IDs and names of retail items in a given retail group meeting the above criteria</returns>
        public virtual List<ItemInGroup> ItemsInGroup(IConnectionManager entry, RecordIdentifier groupId)
        {
            ValidateSecurity(entry);

            if (!groupId.IsGuid)
            {
                groupId = GetMasterID(entry, groupId);

                if(groupId == Guid.Empty)
                {
                    return new List<ItemInGroup>();
                }
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in itemColumns)
                {
                    columns.Add(selectionColumn);
                }

                columns.Add(new TableColumn { ColumnName = "VARIANTNAME", TableAlias = "A" });
                columns.Add(new TableColumn { ColumnName = "ISNULL(retailgroup.NAME,'')", ColumnAlias = "GROUPNAME" });
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.RETAILGROUPMASTERID = @GROUPID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
                     QueryPartGenerator.InternalColumnGenerator(columns),
                     string.Empty,
                     QueryPartGenerator.ConditionGenerator(conditions),
                     string.Empty);

                MakeParam(cmd, "GROUPID", (Guid)groupId, SqlDbType.UniqueIdentifier);
                
                return Execute<ItemInGroup>(entry, cmd, CommandType.Text, PopulateItemInGroup);
            }
        }

        /// <summary>
        /// Get a list of retail items not in a selected retail group, that contain a given search text.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchText">The text to search for. Searches in item name, the ID field and the name alias field</param>
        /// <param name="numberOfRecords">The number of items to return. Items are ordered by retail item name</param>
        /// <param name="excludedGroupId">ID of the retail group which the items are not supposed to be in</param>
        /// <returns>A list of items meeting the above criteria</returns>
        public List<ItemInGroup> ItemsNotInRetailGroup(
            IConnectionManager entry,
            string searchText,
            int numberOfRecords,
            RecordIdentifier excludedGroupId)
        {
            ValidateSecurity(entry);

            List<TableColumn> columns = new List<TableColumn>();
            List<Condition> conditions = new List<Condition>();
            conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });


            using (var cmd = entry.Connection.CreateCommand())
            {
                foreach (var selectionColumn in itemColumns)
                {
                    columns.Add(selectionColumn);
                }
                columns.Add(new TableColumn { ColumnName = "ISNULL(retailgroup.NAME,'')", ColumnAlias = "GROUPNAME" });
                columns.Add(new TableColumn { ColumnName = "VARIANTNAME", TableAlias = "A" });
                conditions.Add(new Condition {Operator = "AND", ConditionValue = "ISNULL(A.RETAILGROUPMASTERID, '00000000-0000-0000-0000-000000000000') <> @excludedGroupId " });
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue =
                        "(A.ITEMID like @searchString or A.ITEMNAME like @searchString or A.NAMEALIAS like @searchString)  "
                });
                Join join = new Join
                {
                    Condition = "A.RETAILGROUPMASTERID = retailgroup.MASTERID AND RETAILGROUP.DELETED =0",
                    JoinType = "LEFT OUTER",
                    Table = "RETAILGROUP",
                    TableAlias = "RETAILGROUP"
                };
           
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A", numberOfRecords),
                     QueryPartGenerator.InternalColumnGenerator(columns),
                     join,
                     QueryPartGenerator.ConditionGenerator(conditions),
                     "Order By A.ITEMNAME");
                
                MakeParam(cmd, "excludedGroupId", (string) excludedGroupId);
                MakeParam(cmd, "searchString", searchText + "%");

                return Execute<ItemInGroup>(entry, cmd, CommandType.Text, PopulateItemInGroup);
            }
        }

        /// <summary>
        /// Removes a retail item with a given Id from the retail group it is currently in.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">The ID of the item to be removed</param>
        public virtual void RemoveItemFromRetailGroup(IConnectionManager entry, RecordIdentifier itemId)
        {
            ValidateSecurity(entry, Permission.ItemsEdit);

            var masterId = GetMasterID(entry, itemId, "RETAILITEM", "ITEMID");
            var statement = new SqlServerStatement("RETAILITEM") {StatementType = StatementType.Update};

            statement.AddCondition("MASTERID", (Guid)masterId, SqlDbType.UniqueIdentifier);
            statement.AddField("RetailGroupMasterID", DBNull.Value, SqlDbType.UniqueIdentifier);
            statement.AddField("ValidationPeriodID", "");

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Adds a retail item with a given Id to a retail group with a given id.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">The ID of the item to add</param>
        /// <param name="groupId">The ID of the retail group to add an item to</param>
        public virtual void AddItemToRetailGroup(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier groupId)
        {
            ValidateSecurity(entry, Permission.ItemsEdit);
            InternalAddRetailItemToGroup(entry, itemId, groupId);
        }

        /// <summary>
        /// Adds multiple retail items a retail group with a given id.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemIDs">The ID of the item to add</param>
        /// <param name="groupId">The ID of the retail group to add an item to</param>
        public virtual void AddItemsToRetailGroup(IConnectionManager entry, List<RecordIdentifier> itemIDs, RecordIdentifier groupId)
        {
            ValidateSecurity(entry, Permission.ItemsEdit);
            foreach(RecordIdentifier itemID in itemIDs)
            {
                InternalAddRetailItemToGroup(entry, itemID, groupId);
            }
        }

        /// <summary>
        /// Gets a retail group with a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The ID of the retail group to get</param>
        /// <returns>A retail group with a given ID, or null if not found</returns>
        public virtual RetailGroup Get(IConnectionManager entry, RecordIdentifier groupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                List<Condition> conditions = new List<Condition>();

                if (groupID.IsGuid)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.MASTERID = @groupId " });

                    MakeParam(cmd, "groupId", (Guid)groupID, SqlDbType.UniqueIdentifier);
                }
                else
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.GROUPID = @groupId " });

                    MakeParam(cmd, "groupId", (string)groupID);
                }

                conditions.Add(new Condition {Operator = "AND", ConditionValue = "A.DELETED = 0 "});

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("RETAILGROUP", "A"),
                  QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                  QueryPartGenerator.JoinGenerator(fixedJoins),
                  QueryPartGenerator.ConditionGenerator(conditions),
                  string.Empty
                  );

                var groups = Execute<RetailGroup>(entry, cmd, CommandType.Text, PopulateRetailGroup);

                return (groups.Count > 0) ? groups[0] : null;
            }
        }

        /// <summary>
        /// Returns a list of retail groups that are within a specific retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailDepartmentID">The unique ID of the retail deparment</param>
        /// <param name="sortEnum">Controls which columns will be sorted by in the SQL
        /// statement</param>
        /// <param name="sortBackwards">If true then the ordering will be descending
        /// otherwise it will be ascending</param>
        public virtual List<RetailGroup> GetRetailGroupsInRetailDepartment(
            IConnectionManager entry, 
            RecordIdentifier retailDepartmentID, 
            RetailGroupSorting sortEnum, 
            bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Condition> conditions = new List<Condition>();
                if (retailDepartmentID.IsGuid)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DEPARTMENTMASTERID = @retailDepartmentID " });
                }
                else
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DEPARTMENTID = @retailDepartmentID " });
                }
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });



                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILGROUP", "A"),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    QueryPartGenerator.JoinGenerator(fixedJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );
              
                MakeParam(cmd, "retailDepartmentID", (string) retailDepartmentID);

                return Execute<RetailGroup>(entry, cmd, CommandType.Text, PopulateRetailGroup);
            }
        }

        /// <summary>
        /// Returns a list of retail groups that are not included in the retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="excludedRetailDepartmentID">The unique ID of the retail
        /// department</param>
        /// <param name="searchText">Full or partial name of a retail group to be
        /// found</param>
        /// <param name="sortEnum">Controls which columns will be sorted by in the SQL
        /// statement</param>
        /// <param name="sortBackwards">If true then the ordering will be descending
        /// otherwise it will be ascending</param>
        public List<RetailGroup> GetRetailGroupsNotInRetailDepartment(
            IConnectionManager entry, 
            RecordIdentifier excludedRetailDepartmentID,
            string searchText,
            RetailGroupSorting sortEnum, 
            bool sortBackwards)
        {
            
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Condition> conditions = new List<Condition>();
                conditions.Add( new Condition { Operator = "AND", ConditionValue = "A.DEPARTMENTID <> @excludedRetailDepartmentID " });
                conditions.Add( new Condition { Operator = "AND", ConditionValue = "(A.NAME like @searchString or A.GROUPID like @searchString) " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });


                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("RETAILGROUP", "A"),
                  QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                  QueryPartGenerator.JoinGenerator(fixedJoins),
                  QueryPartGenerator.ConditionGenerator( conditions),
                  ResolveSort(sortEnum, sortBackwards, false, "A")
                  );

                MakeParam(cmd, "excludedRetailDepartmentID", (string) excludedRetailDepartmentID);
                MakeParam(cmd, "searchString", "%" + searchText + "%");

                return Execute<RetailGroup>(entry, cmd, CommandType.Text, PopulateRetailGroup);
            }
        }

        /// <summary>
        /// Adds a specific retail group to the retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupID">The unique ID of the retail group</param>
        /// <param name="retailDepartmentID">The unique ID of the retail department</param>
        public virtual void AddRetailGroupToRetailDepartment(IConnectionManager entry, RecordIdentifier retailGroupID, RecordIdentifier retailDepartmentID)
        {
            InternalAddRetailGroupToRetailDepartment(entry, retailGroupID, retailDepartmentID);
        }

        /// <summary>
        /// Adds multiple retail groups to the retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupIDs">The unique IDs of the retail groups</param>
        /// <param name="retailDepartmentID">The unique ID of the retail department</param>
        public virtual void AddRetailGroupsToRetailDepartment(IConnectionManager entry, List<RecordIdentifier> retailGroupIDs, RecordIdentifier retailDepartmentID)
        {
            foreach(RecordIdentifier group in retailGroupIDs)
            {
                InternalAddRetailGroupToRetailDepartment(entry, group, retailDepartmentID);
            }
        }

        /// <summary>
        /// Clears the retail department value of the specific retail group 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupID">The unique ID of the retail group</param>
        public virtual void RemoveRetailGroupFromRetailDepartment(IConnectionManager entry, RecordIdentifier retailGroupID)
        {
            var retailGroup = Get(entry, retailGroupID);
            retailGroup.RetailDepartmentID = "";
            retailGroup.RetailDepartmentMasterID = RecordIdentifier.Empty;
            Save(entry, retailGroup);
        }

        /// <summary>
        /// Checks if a retail group with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The ID of the group to check for</param>
        /// <returns>Whether the given group exists</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier groupID)
        {
            return RecordExists(entry, "RETAILGROUP", "GROUPID", groupID,false);
        }

        /// <summary>
        /// Deletes a retail group with a given ID
        /// </summary>
        /// <remarks>Requires the 'Manage retail groups' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The ID of the retail group to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier groupID)
        {
            ValidateSecurity(entry, Permission.ManageRetailGroups);

            var statement = new SqlServerStatement("RETAILGROUP");

            statement.StatementType = StatementType.Update;

            var masterID = groupID.IsGuid ? groupID : GetMasterID(entry, groupID);

            statement.AddCondition("MASTERID", (Guid)masterID, SqlDbType.UniqueIdentifier);          
            statement.AddField("DELETED", true, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves a given retail group to the database
        /// </summary>
        /// <remarks>Requires the 'Manage retail groups' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="group">The retail group to save</param>
        public virtual void Save(IConnectionManager entry, RetailGroup group)
        {
            var statement = new SqlServerStatement("RETAILGROUP");
            statement.UpdateColumnOptimizer = group;

            ValidateSecurity(entry, Permission.ManageRetailGroups);

            group.Validate();

            bool isNew = false;
            if (group.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                group.ID = DataProviderFactory.Instance.GenerateNumber<IRetailGroupData, RetailGroup>(entry); 
            }

            if (isNew || !Exists(entry,group.ID))
            {
                statement.StatementType = StatementType.Insert;
                if (RecordIdentifier.IsEmptyOrNull(group.MasterID)) 
                {
                    group.MasterID = Guid.NewGuid();
                }
                statement.AddKey("MASTERID", (Guid)group.MasterID, SqlDbType.UniqueIdentifier);
                statement.AddField("GROUPID", (string)group.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                // This can happen if we are getting the retail group from the Integration Framework, where the external caller does not have information
                // about our master IDs
                if (RecordIdentifier.IsEmptyOrNull(group.MasterID))
                {
                    group.MasterID = GetMasterID(entry, group.ID, "RETAILGROUP", "GROUPID");

                }

                statement.AddCondition("MASTERID", (Guid)group.MasterID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("NAME", group.Text);
            statement.AddField("DEPARTMENTID", (string)group.RetailDepartmentID);
            if (!RecordIdentifier.IsEmptyOrNull(group.RetailDepartmentMasterID))
            {
                statement.AddField("DEPARTMENTMASTERID", (Guid) group.RetailDepartmentMasterID,
                    SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.AddField("DEPARTMENTMASTERID", DBNull.Value, SqlDbType.UniqueIdentifier);
            }
          
            statement.AddField("SALESTAXITEMGROUP", (string)group.ItemSalesTaxGroupId);
            statement.AddField("DEFAULTPROFIT", group.ProfitMargin, SqlDbType.Decimal);
            statement.AddField("POSPERIODICID", group.ValidationPeriod);
            statement.AddField("MODIFIED", DateTime.Now, SqlDbType.DateTime2);
            statement.AddField("TAREWEIGHT", group.TareWeight, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Returns true if the retail group is in the retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupID">The unique ID of the retail group</param>
        /// <param name="retailDepartmentID">The unique ID of the retail department</param>
        public virtual bool RetailGroupInDepartment(IConnectionManager entry, RecordIdentifier retailGroupID, RecordIdentifier retailDepartmentID)
        {
            //TODO This seems redundant
            List<RetailGroup> groupsInDepartment = GetRetailGroupsInRetailDepartment(entry, retailDepartmentID, RetailGroupSorting.RetailDepartmentName, false);

            if (retailGroupID.IsGuid)
            {
                return groupsInDepartment.Any(x => x.MasterID == retailGroupID);
            }
            return groupsInDepartment.Any(x => x.ID == retailGroupID);
        }

        /// <summary>
        /// Undeletes the given group. This set the DELETED flag on the item to 0
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupID">The ID of the retail group to undelete</param>
        public virtual void Undelete(IConnectionManager entry, RecordIdentifier groupID)
        {
            ValidateSecurity(entry, Permission.ManageRetailGroups);

            var statement = new SqlServerStatement("RETAILGROUP");
            statement.StatementType = StatementType.Update;
            statement.AddField("DELETED", false, SqlDbType.Bit);

            var masterID = groupID.IsGuid ? groupID : GetMasterID(entry, groupID);
            statement.AddCondition("MASTERID", (Guid)masterID, SqlDbType.UniqueIdentifier);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Returns true if the group is marked as deleted
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupID">The ID of the group to check</param>
        /// <returns>True if the group is marked as deleted, false otherwise</returns>
        public bool IsDeleted(IConnectionManager entry, RecordIdentifier groupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"select DELETED
                       from RETAILGROUP
                       where MASTERID = @masterID";

                var masterID = groupID.IsGuid ? groupID : GetMasterID(entry, groupID);
                MakeParam(cmd, "masterID", (Guid)masterID, SqlDbType.UniqueIdentifier);

                return (bool)entry.Connection.ExecuteScalar(cmd);
            }
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
            get { return "RETAILGROUP"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RETAILGROUP", "GROUPID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}