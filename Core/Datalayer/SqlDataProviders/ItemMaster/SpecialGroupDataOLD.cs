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
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
    /// <summary>
    /// Data provider class for special groups
    /// </summary>
    public class SpecialGroupDataOLD : SqlServerDataProviderBase
    {
        private static string ResolveItemSort(SpecialGroupItemSorting sort, bool backwards)
        {
            switch (sort)
            {
                case SpecialGroupItemSorting.ItemID:
                    return "a.ITEMID" + (backwards ? " DESC" : " ASC");

                case SpecialGroupItemSorting.ItemName:
                    return "a.ITEMNAME" + (backwards ? " DESC" : " ASC");
            }

            return "";
        }

        private static string ResolveSort(SpecialGroupSorting sort, bool backwards)
        {
            switch (sort)
            {
                case SpecialGroupSorting.GroupID:
                    return "GROUPID"  + (backwards ? " DESC" : " ASC");

                case SpecialGroupSorting.GroupName:
                    return "NAME" + (backwards ? " DESC" : " ASC");
            }

            return "";
        }

        private static void PopulateItemInGroup(IDataReader dr, ItemInGroup item)
        {
            item.ID = (string)dr["ITEMID"];
            item.Text = (string)dr["ITEMNAME"];
            item.Group = (string)dr["GROUPNAME"];
        }

        /// <summary>
        /// Gets a list of of data entities containing IDs and names for all special groups, ordered by a chosen field
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwords">A enum that defines how the result should be sorted</param>
        /// <returns>A list of all special groups, ordered by a chosen field</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, SpecialGroupSorting sortBy, bool sortBackwords)
        {
            if (sortBy != SpecialGroupSorting.GroupID && sortBy != SpecialGroupSorting.GroupName)
            {
                throw new NotSupportedException();
            }

            return GetList<DataEntity>(entry, "RBOSPECIALGROUP", "NAME", "GROUPID", ResolveSort(sortBy, sortBackwords));
        }

        /// <summary>
        /// Gets a list of of data entities containing IDs and names for all special groups, ordered by special group names
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all special groups</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOSPECIALGROUP", "NAME", "GROUPID", "NAME");
        }

        /// <summary>
        /// Gets a data entity containing the id and name of a special group by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="specialGroupId">The ID of the special group to fetch</param>
        /// <returns>A special group entity or null if not found</returns>
        public virtual DataEntity Get(IConnectionManager entry, RecordIdentifier specialGroupId)
        {
            return GetDataEntity<DataEntity>(entry, "RBOSPECIALGROUP", "NAME", "GROUPID", specialGroupId);
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and names of retail items in a given special group. The list is ordered by retail item names, 
        /// and items numbered between recordFrom and recordTo will be returned
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupId">The ID of the special group</param>
        /// <param name="recordFrom">The result number of the first item to be retrieved</param>
        /// <param name="recordTo">The result number of the last item to be retrieved</param>
        /// <param name="sortBy">Defines the column to sort the result set by</param>
        /// <param name="sortedBackwards">Set to true if wanting to sort the result set backwards</param>
        /// <returns>A list of data entities containing IDs and names of retail items in a given special group meeting the above criteria</returns>
        public List<DataEntity> ItemsInSpecialGroup(IConnectionManager entry, RecordIdentifier groupId,
                                                           int recordFrom, int recordTo, SpecialGroupItemSorting sortBy,
                                                           bool sortedBackwards)
        {
            ValidateSecurity(entry);

            string sort = ResolveItemSort(sortBy, sortedBackwards);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select item.ITEMID, item.ITEMNAME " +
                    "From (Select a.ITEMID, ISNULL(a.ITEMNAME,'') AS ITEMNAME, ROW_NUMBER() OVER(order by " + sort +
                    " ) as ROW " +
                    "From INVENTTABLE a " +
                    "Join RBOSPECIALGROUPITEMS sgi on a.DATAAREAID = sgi.DATAAREAID and a.ITEMID = sgi.ITEMID " +
                    "Where sgi.GROUPID = @groupId and sgi.DATAAREAID = @dataAreaId) item " +
                    "Where item.ROW between @recordFrom and @recordTo ";



                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "groupId", groupId);
                MakeParam(cmd, "recordFrom", recordFrom, SqlDbType.Int);
                MakeParam(cmd, "recordTo", recordTo, SqlDbType.Int);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
            }
        }

        /// <summary>
        /// Get a list of retail items not in a selected special group, that contain a given search text.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchText">The text to search for. Searches in item name, the ID field and the name alias field</param>
        /// <param name="numberOfRecords">The number of items to return. Items are ordered by retail item name</param>
        /// <param name="excludedGroupId">The ID of the special group to exclude</param>
        /// <returns>A list of retail items meeting the above criteria</returns>
        public List<ItemInGroup> ItemsNotInSpecialGroup(
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
                    " a.ITEMID, ISNULL(a.ITEMNAME,'') AS ITEMNAME, ISNULL(sg.NAME,'') AS GROUPNAME " +
                    "from INVENTTABLE a " +
                    "left outer join RBOSPECIALGROUPITEMS sgi on a.DATAAREAID = sgi.DATAAREAID and a.ITEMID = sgi.ITEMID " +
                    "left outer join RBOSPECIALGROUP sg on a.DATAAREAID = sg.DATAAREAID and sgi.GROUPID = sg.GROUPID " +
                    "Where a.DATAAREAID = @dataAreaId and (sgi.GROUPID <> @groupId or sgi.GROUPID is NULL) " +
                    "and (a.ITEMID like @searchString or a.ITEMNAME like @searchString  or a.NAMEALIAS like @searchString) " +
                    "Order by a.ITEMNAME ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "groupId", (string) excludedGroupId);
                MakeParam(cmd, "searchString", searchText + "%");

                return Execute<ItemInGroup>(entry, cmd, CommandType.Text, PopulateItemInGroup);
            }
        }

        public virtual bool ItemInSpecialGroup(IConnectionManager entry, RecordIdentifier groupId, RecordIdentifier itemId)
        {
            return RecordExists(entry, "RBOSPECIALGROUPITEMS", new[]{"GROUPID","ITEMID"}, new RecordIdentifier(groupId,itemId));
        }

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="DataEntity" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        public virtual List<DataEntity> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                string modifiedSearchString = (beginsWith ? "" : "%") + searchString + "%";

                cmd.CommandText = @"Select s.* from (
                                    select sg.GROUPID, sg.NAME,
                                    ROW_NUMBER() OVER(order by sg.NAME) AS ROW
                                    from RBOSPECIALGROUP sg
                                    where sg.DATAAREAID = @dataAreaId";
                                    
                
                cmd.CommandText += @" and (sg.NAME Like @searchString)) s
                                    where s.ROW between " + rowFrom + " and " + rowTo;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "searchString", modifiedSearchString);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "GROUPID");
            }
        }

        /// <summary>
        /// Deletes a special group by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="specialGroupId">The ID of the group to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier specialGroupId)
        {
            DeleteRecord(entry, "RBOSPECIALGROUP", "GROUPID", specialGroupId, BusinessObjects.Permission.ManageSpecialGroups);
        }

        /// <summary>
        /// Removes a retail item from a special group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">ID of the item to remove</param>
        /// <param name="groupId">ID of the special group</param>
        public virtual void RemoveItemFromSpecialGroup(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier groupId)
        {
            var recID = new RecordIdentifier(groupId, itemId);

            DeleteRecord(entry, "RBOSPECIALGROUPITEMS", new[] { "GROUPID", "ITEMID" }, recID, BusinessObjects.Permission.ManageSpecialGroups); 
        }

        /// <summary>
        /// Adds a retail item to a special group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">ID of the item to add</param>
        /// <param name="groupId">ID of the special group</param>
        public virtual void AddItemToSpecialGroup(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier groupId)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ItemsEdit);

            if (ItemInSpecialGroup(entry,groupId,itemId)) return;

            var statement = new SqlServerStatement("RBOSPECIALGROUPITEMS") {StatementType = StatementType.Insert};

            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddKey("ITEMID", itemId.ToString());
            statement.AddKey("GROUPID", (string)groupId);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Checks if a special group with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">ID of the special group to check for</param>
        /// <returns>Whether the special group exists or not</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier groupID)
        {
            return RecordExists(entry, "RBOSPECIALGROUP", "GROUPID", groupID);
        }

        /// <summary>
        /// Saves the given special group to the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="specialGroup">Special group to save</param>
        public virtual void Save(IConnectionManager entry, DataEntity specialGroup)
        {
            var statement = new SqlServerStatement("RBOSPECIALGROUP");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageSpecialGroups);

            bool isNew = false;
            if (specialGroup.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                specialGroup.ID = DataProviderFactory.Instance.GenerateNumber<ISpecialGroupData, DataEntity>(entry);
            }

            if (isNew || !Exists(entry, specialGroup.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("GROUPID", (string)specialGroup.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("GROUPID", (string)specialGroup.ID);
            }

            statement.AddField("NAME", specialGroup.Text);
            
            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Gets a list of SpecialGroupItem entities, one for each special group. Each entity contains information about a special group, 
        /// an item with the given ID and whether the item is in the group or not.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">The ID of the item we want to see if is contained in the groups</param>
        /// <returns>A list of data entities that show information about all the special groups and whether the item with the given ID is contained in each group</returns>
        public virtual List<SpecialGroupItem> GetItemsGroupInformation(IConnectionManager entry, RecordIdentifier itemId)
        {
            var groups = GetList(entry);
            var groupResults = new List<SpecialGroupItem>();

            ValidateSecurity(entry);

            foreach (var group in groups)
            {
                bool itemIsInGroup = ItemInSpecialGroup(entry,group.ID, itemId);
                groupResults.Add(new SpecialGroupItem { GroupId = group.ID, GroupName = group.Text, ItemIsInGroup = itemIsInGroup });
            }

            return groupResults;
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
            get { return "SPECIALGROUP"; }
        }

        #endregion
    }
}
