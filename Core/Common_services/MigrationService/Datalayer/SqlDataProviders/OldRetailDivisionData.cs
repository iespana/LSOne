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
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Services.Datalayer.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.SqlDataProviders
{
    /// <summary>
    /// Data provider class for retail subgroups
    /// </summary>
    public class OldRetailDivisionData : SqlServerDataProviderBase, IOldRetailDivisionData
    {
        private static string BaseSql
        {
            get
            {
                return "Select a.DIVISIONID, " +
                       "ISNULL(a.NAME,'') AS NAME " +
                       "From RBOINVENTITEMRETAILDIVISION a ";
            }
        }

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

            return GetList<DataEntity>(entry, "RBOINVENTITEMRETAILDIVISION", "NAME", "DIVISIONID", GetSortColumn(sortEnum));
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and names for all retail group, ordered by retail group name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities contaning IDs and names of retail groups, ordered by retail group name</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOINVENTITEMRETAILDIVISION", "NAME", "DIVISIONID", "NAME");
        }

        public List<RetailDivision> GetDetailedList(IConnectionManager entry, RetailDivisionSorting sortEnum,
                                                           bool backwardsSort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseSql +
                    "Where a.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<RetailDivision>(entry, cmd, CommandType.Text, PopulateRetailDivision);
            }
        }

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="RetailGroup" />
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
                var modifiedSearchString = (beginsWith ? "" : "%") + searchString + "%";
               
                cmd.CommandText =
                    " Select s.* from ( " +
                    "Select a.DIVISIONID as DIVISIONID, " +
                    "ISNULL(a.NAME,'') as NAME, " +
                    //"ISNULL(a.NAMEALIAS,'') as NAMEALIAS, " +
                    "ROW_NUMBER() OVER(order by a." + GetSortColumn(sort) + ") AS ROW " +
                    "From RBOINVENTITEMRETAILDIVISION a " +
                    "WHERE a.DATAAREAID = @DATAAREAID ";

                cmd.CommandText +=
                    "AND (a.NAME Like @SEARCHSTRING or a.DIVISIONID Like @SEARCHSTRING)) s " +
                    "WHERE s.ROW between " + rowFrom + " and " + rowTo;

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "SEARCHSTRING", modifiedSearchString);

                return Execute<RetailDivision>(entry, cmd, CommandType.Text, PopulateRetailDivision);
            }
        }

        /*
        /// <summary>
        /// Gets a list of data entities containing ID and name of a retail items in a given retail group. The list is
        /// ordered by retail item names and items numbered between recordFrom and recordTo will be returned.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupId">Id of the retail group</param>
        /// <param name="recordFrom">The result number of the first item to be retrieved</param>
        /// <param name="recordTo">The result number of the last item to be retrieved</param>
        /// <returns>A list of data entities containing IDs and names of retail items in a given retail group meeting the above criteria</returns>
        public List<DataEntity> ItemsInGroup(IConnectionManager entry, RecordIdentifier groupId, int recordFrom,
                                                    int recordTo)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select item.ITEMID, item.ItemName " +
                    "From " +
                    "(Select it.ITEMID, ISNULL(it.ITEMNAME,'') as ItemName, ROW_NUMBER() OVER(order by ITEMNAME) as ROW " +
                    "From RBOINVENTTABLE rit  " +
                    "join INVENTTABLE it on rit.DATAAREAID = it.DATAAREAID and rit.ITEMID = it.ITEMID  " +
                    "Where rit.ITEMGROUP = @GROUPID and it.DATAAREAID = @DATAAREAID) item " +
                    "Where item.ROW between @RECORDFROM and @RECORDTO";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "GROUPID", (string) groupId);
                MakeParam(cmd, "RECORDFROM", recordFrom, SqlDbType.Int);
                MakeParam(cmd, "RECORDTO", recordTo, SqlDbType.Int);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
            }
        }

        /// <summary>
        /// Gets a list of data entities containing ID and name of a retail items in a given retail group. The list is
        /// ordered by retail item names and items numbered between recordFrom and recordTo will be returned.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupId">Id of the retail group</param>
        /// <returns>A list of data entities containing IDs and names of retail items in a given retail group meeting the above criteria</returns>
        public virtual List<RetailItem> ItemsInGroup(IConnectionManager entry, RecordIdentifier groupId)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select item.ITEMID, item.ItemName " +
                    "From " +
                    "(Select it.ITEMID, ISNULL(it.ITEMNAME,'') as ItemName, ROW_NUMBER() OVER(order by ITEMNAME) as ROW " +
                    "From RBOINVENTTABLE rit " +
                    "join INVENTTABLE it on rit.DATAAREAID = it.DATAAREAID and rit.ITEMID = it.ITEMID  " +
                    "Where rit.ITEMGROUP = @GROUPID and it.DATAAREAID = @DATAAREAID) item";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "GROUPID", (string) groupId);

                return Execute<RetailItem>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
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

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select TOP " + numberOfRecords +
                    " a.ITEMID, ISNULL(a.ITEMNAME,'') AS ITEMNAME, ISNULL(retailGroup.NAME,'') AS GROUPNAME " +
                    "From INVENTTABLE a " +
                    "Join RBOINVENTTABLE ra on a.DATAAREAID = ra.DATAAREAID AND a.ITEMID = ra.ITEMID " +
                    "Left outer join RBOINVENTITEMRETAILGROUP retailGroup on retailGroup.DATAAREAID = ra.DATAAREAID and ra.ITEMGROUP = retailGroup.GROUPID " +
                    "Where ra.ITEMGROUP <> @excludedGroupId and (a.ITEMID like @searchString or a.ITEMNAME like @searchString or a.NAMEALIAS like @searchString) " +
                    "and a.DATAAREAID = @DATAAREAID " +
                    "Order By a.ITEMNAME";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
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
            ValidateSecurity(entry, BusinessObjects.Permission.ItemsEdit);

            var statement = new SqlServerStatement("RBOINVENTTABLE") {StatementType = StatementType.Update};

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("ITEMID", itemId.ToString());
            statement.AddField("ITEMGROUP", "");
            statement.AddField("POSPERIODICID", "");
            //statement.AddField("VALIDATIONPERIODDISCOUNTDESCRIPTION", "");

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Adds a retail item with a given Id to a retail group with a given id.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">The ID of the item to add</param>
        /// <param name="groupId">The ID of the retail group to add an item to</param>
        public virtual void AddItemToRetailGroup(IConnectionManager entry, RecordIdentifier itemId, string groupId)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ItemsEdit);

            var statement = new SqlServerStatement("RBOINVENTTABLE");
            var group = Get(entry, groupId);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("ITEMID", itemId.ToString());
            statement.AddField("ITEMGROUP", groupId);
            statement.AddField("POSPERIODICID", group.ValidationPeriod);
            //statement.AddField("VALIDATIONPERIODDISCOUNTDESCRIPTION", group.ValidationPeriodDescription);

            entry.Connection.ExecuteStatement(statement);
        }
        */
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

                cmd.CommandText =
                    BaseSql +
                    "Where a.DIVISIONID = @divisionId AND a.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "divisionId", (string)divisionId);

                var divs = Execute<RetailDivision>(entry, cmd, CommandType.Text, PopulateRetailDivision);

                return (divs.Count > 0) ? divs[0] : null;
            }
        }

        /*
        /// <summary>
        /// Adds a specific retail group to the retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupID">The unique ID of the retail group</param>
        /// <param name="retailDepartmentID">The unique ID of the retail department</param>
        public virtual void AddRetailGroupToRetailDepartment(IConnectionManager entry, RecordIdentifier retailGroupID, RecordIdentifier retailDepartmentID)
        {
            var retailGroup = Get(entry, retailGroupID);
            retailGroup.RetailDepartmentId = (string)retailDepartmentID;
            Save(entry, retailGroup);
        }

        /// <summary>
        /// Clears the retail department value of the specific retail group 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupID">The unique ID of the retail group</param>
        public virtual void RemoveRetailGroupFromRetailDepartment(IConnectionManager entry, RecordIdentifier retailGroupID)
        {
            var retailGroup = Get(entry, retailGroupID);
            retailGroup.RetailDepartmentId = "";
            Save(entry, retailGroup);
        }
        */
        /// <summary>
        /// Checks if a retail group with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="divisionId">The ID of the group to check for</param>
        /// <returns>Whether the given group exists</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier divisionId)
        {
            return RecordExists(entry, "RBOINVENTITEMRETAILDIVISION", "DIVISIONID", divisionId);
        }

        /// <summary>
        /// Deletes a retail group with a given ID
        /// </summary>
        /// <remarks>Requires the 'Manage retail groups' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="divisionId">The ID of the retail group to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier divisionId)
        {
            DeleteRecord(entry, "RBOINVENTITEMRETAILDIVISION", "DIVISIONID", divisionId, DataLayer.BusinessObjects.Permission.ManageRetailDivisions);
        }

        /// <summary>
        /// Saves a given retail department to the database
        /// </summary>
        /// <remarks>Requires the 'Manage retail groups' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="division">The retail group to save</param>
        public virtual void Save(IConnectionManager entry, RetailDivision division)
        {
            var statement = new SqlServerStatement("RBOINVENTITEMRETAILDIVISION");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ManageRetailDivisions);

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

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("DIVISIONID", (string)division.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("DIVISIONID", (string) division.ID);
            }
            statement.AddField("NAME", division.Text);
            
            entry.Connection.ExecuteStatement(statement);
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

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a unique ID for the class
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "RETAILDIVISION"; }
        }

        #endregion
    }
}
