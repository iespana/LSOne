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
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
    /// <summary>
    /// Data provider class for retail departments
    /// </summary>
    public class RetailDepartmentDataOLD : SqlServerDataProviderBase
    {
        private static string BaseSQL
        {
            get
            {
                return "SELECT dep.DEPARTMENTID, " +
                       "ISNULL(dep.NAME,'') AS NAME, " +
                       "ISNULL(dep.NAMEALIAS,'') as NAMEALIAS, " +
                       "ISNULL(div.DIVISIONID,'') as DIVISIONID, " +
                       "ISNULL(div.NAME, '') as DIVISIONNAME " +
                       "FROM RBOINVENTITEMDEPARTMENT dep " +
                       "left outer join RBOINVENTITEMRETAILDIVISION div on dep.DATAAREAID = div.DATAAREAID and dep.DIVISIONID = div.DIVISIONID " +
                       "WHERE dep.DATAAREAID = @dataareaid ";
            }
        }

        /// <summary>
        /// Returns the name of the column that the data should be sorted by in a SQL
        /// statment
        /// </summary>
        /// <param name="sortEnum">Controls which columns will be sorted by in the SQL
        /// statement</param>
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

        private static void PopulateDepartmentList(IDataReader dr, DataEntity retailDepartment)
        {
            retailDepartment.ID = (string)dr["DEPARTMENTID"];
            retailDepartment.Text = (string)dr["NAME"];
        }

        private static void PopulateRetailDepartment(IDataReader dr, RetailDepartment retailDepartment)
        {
            PopulateDepartmentList(dr, retailDepartment);

            retailDepartment.NameAlias = (string)dr["NAMEALIAS"];

            retailDepartment.RetailDivisionID = AsString(dr["DIVISIONID"]);
            retailDepartment.RetailDivisionName = AsString(dr["DIVISIONNAME"]);
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
                cmd.CommandText = BaseSQL + ResolveSort(sortEnum, backwardsSort);

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);

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
            return GetList<DataEntity>(entry, "RBOINVENTITEMDEPARTMENT", "NAME", "DEPARTMENTID", GetSortColumn(sort));
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
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                var modifiedSearchString = (beginsWith ? "" : "%") + searchString + "%";

                cmd.CommandText =
                    "Select s.* from (  " +
                    "Select dep.DEPARTMENTID, ISNULL(dep.NAME,'') as NAME, ROW_NUMBER() OVER (order by dep." + GetSortColumn(sort) + ") AS ROW " +
                    "From RBOINVENTITEMDEPARTMENT dep " +
                    "left outer join RBOINVENTITEMRETAILDIVISION div on div.DATAAREAID = dep.DATAAREAID and div.DIVISIONID = dep.DIVISIONID " +
                    "WHERE dep.DATAAREAID = @dataAreaId AND (dep.NAME Like @searchString or dep.DEPARTMENTID Like @searchString ) <DIVSEARCH>) s " +
                    "WHERE s.ROW between  " + rowFrom + " and " + rowTo;

                bool divisionIdSearch = !(divisionId == null || string.IsNullOrEmpty((string)divisionId));
                cmd.CommandText = cmd.CommandText.Replace("<DIVSEARCH>",
                    divisionIdSearch ?
                    string.Format("AND dep.DIVISIONID = @divisionId") : "");
                if (divisionIdSearch)
                    MakeParam(cmd, "divisionId", (string)divisionId);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "searchString", modifiedSearchString);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, PopulateDepartmentList);
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
            return RecordExists(entry, "RBOINVENTITEMDEPARTMENT", "DEPARTMENTID", departmentId);
        }

        /// <summary>
        /// Deletes a retail department with a given ID
        /// </summary>
        /// <remarks>Requires the 'Manage retail departments' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentId">The ID of the retail department to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier departmentId)
        {
            DeleteRecord(entry, "RBOINVENTITEMDEPARTMENT", "DEPARTMENTID", departmentId, BusinessObjects.Permission.ManageRetailDepartments);
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
                cmd.CommandText = BaseSQL + " AND dep.DEPARTMENTID = @departmentid ";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "departmentid", (string)departmentId);

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
            var statement = new SqlServerStatement("RBOINVENTITEMDEPARTMENT");

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

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("DEPARTMENTID", (string)retailDepartment.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("DEPARTMENTID", (string)retailDepartment.ID);
            }

            statement.AddField("NAME", retailDepartment.Text);
            statement.AddField("NAMEALIAS", retailDepartment.NameAlias);

            entry.Connection.ExecuteStatement(statement);

            AddOrRemoveRetailDepartmentFromRetailDivision(entry, retailDepartment.ID, retailDepartment.RetailDivisionID);
        }

        /// <summary>
        /// Removes a retail item with a given Id from the retail group it is currently in.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentId">The ID of the department to be removed</param>
        /// <param name="divisionId">The ID of the division - null if department should be removed from division</param>
        public virtual void AddOrRemoveRetailDepartmentFromRetailDivision(IConnectionManager entry, RecordIdentifier departmentId, RecordIdentifier divisionId)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ItemsEdit);

            var statement = new SqlServerStatement("RBOINVENTITEMDEPARTMENT") { StatementType = StatementType.Update };

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("DEPARTMENTID", departmentId.ToString());
            if (divisionId == null || string.IsNullOrEmpty((string)divisionId))
                statement.AddField("DIVISIONID", "");
            else
                statement.AddField("DIVISIONID", (string)divisionId);

            entry.Connection.ExecuteStatement(statement);
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
                cmd.CommandText =
                    "select top(" + numberOfRecords +
                    ") DEPARTMENTID, ISNULL(dep.NAME,'') as NAME, ISNULL(dep.NAMEALIAS,'') as NAMEALIAS, ISNULL(div.DIVISIONID,'') as DIVISIONID, ISNULL(div.NAME,'') as DIVISIONNAME " +
                    "from RBOINVENTITEMDEPARTMENT dep " +
                    "left outer join RBOINVENTITEMRETAILDIVISION div on div.DATAAREAID = dep.DATAAREAID AND div.DIVISIONID = dep.DIVISIONID " +
                    "where dep.DIVISIONID <> @excludedDivisionId OR dep.DIVISIONID is null AND dep.DATAAREAID = @dataAreaId " +
                    "and (dep.DEPARTMENTID like @searchString or dep.NAME like @searchString or dep.NAMEALIAS like @searchString) " +
                    "ORDER BY dep.NAME";


                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
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
                cmd.CommandText = @"Select DEPARTMENTID, ISNULL(dep.NAME,'') as NAME, ISNULL(dep.NAMEALIAS,'') as NAMEALIAS, ISNULL(div.DIVISIONID,'') as DIVISIONID, ISNULL(div.NAME,'') as DIVISIONNAME 
                    from RBOINVENTITEMDEPARTMENT dep 
                    left outer join RBOINVENTITEMRETAILDIVISION div on div.DATAAREAID = dep.DATAAREAID AND div.DIVISIONID = dep.DIVISIONID 
                    where dep.DIVISIONID = @divisionId OR dep.DIVISIONID is null AND dep.DATAAREAID = @dataAreaId 
                    ORDER BY dep.NAME";


                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "divisionId", divisionID);

                return Execute<RetailDepartment>(entry, cmd, CommandType.Text, PopulateRetailDepartment);
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
            get { return "RETAILDEPARTMENT"; }
        }

        #endregion
    }
}
