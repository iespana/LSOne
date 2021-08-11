using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Services.Datalayer.BusinessObjects.Dimensions;
using LSOne.Services.Datalayer.DataProviders.Dimensions;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.SqlDataProviders.Dimensions
{
    /// <summary>
    /// Data provider class for dimension groups
    /// </summary>
    public class OldDimensionGroupData : SqlServerDataProviderBase, IOldDimensionGroupData
    {
        public enum OldDimensionEnum
        {
            Serial = 5,
            Size = 8,
            Color = 9,
            Style = 20001
        }

        private static string ResolveSort(OldDimensionGroupSorting sort, bool sortedBackwards)
        {
            var direction = (sortedBackwards ? " DESC" : " ASC");

            switch(sort)
            {
                case OldDimensionGroupSorting.ID:
                    return "DIMGROUPID" + direction;

                case OldDimensionGroupSorting.Name:
                    return "NAME" + direction;

                case OldDimensionGroupSorting.SizeActive:
                    return "ids_size.ACTIVE" + direction;

                case OldDimensionGroupSorting.ColorActive:
                    return "ids_color.ACTIVE" + direction;

                case OldDimensionGroupSorting.StyleActive:
                    return "ids_style.ACTIVE" + direction;

                case OldDimensionGroupSorting.SerialNumberActive:
                    return "ids_serial.ACTIVE" + direction;
                    
            }

            return "";
        }

        private static void PopulateGroup(IDataReader dr, OldDimensionGroup group)
        {
            group.ID = (string)dr["DIMGROUPID"];
            group.Text = (string)dr["NAME"];
            group.ColorActive = ((byte)dr["COLORACTIVE"] == 1);
            group.SizeActive = ((byte)dr["SIZEACTIVE"] == 1);
            group.StyleActive = ((byte)dr["STYLEACTIVE"] == 1);
            group.SerialActive = ((byte)dr["SERIALACTIVE"] == 1);
            group.SerialAllowBlank = ((byte)dr["SERIALALLOWBLANK"] == 1);
            group.PosDisplaySetting = (OldDimensionGroup.PosDisplaySettingEnum)dr["POSDISPLAYSETTING"];
        }

        /// <summary>
        /// Gets a list of data entities containing ID and name for each dimension group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all dimension groups, ordered by name</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "INVENTDIMGROUP", "NAME", "DIMGROUPID", "NAME");
        }


        /// <summary>
        /// Returns true if the item has dimensions 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailItemID">The unique ID of the item</param>
        public virtual bool ItemIsVariantItem(IConnectionManager entry, RecordIdentifier retailItemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select ids.DIMFIELDID, ids.ACTIVE from INVENTTABLE it " +
                                  " left outer join INVENTDIMSETUP ids on it.DATAAREAID = ids.DATAAREAID and it.DIMGROUPID = ids.DIMGROUPID " +
                                  " where it.ITEMID = @itemID and (ids.DIMFIELDID = " + (int)OldDimensionEnum.Size + 
                                  " or ids.DIMFIELDID = " + (int)OldDimensionEnum.Color + 
                                  " or ids.DIMFIELDID = " + (int)OldDimensionEnum.Style + ") and ids.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string) retailItemID);

                var sizeActive = false;
                var colorActive = false;
                var styleActive = false;
                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    while (dr.Read())
                    {
                        switch ((int) dr["DIMFIELDID"])
                        {
                            case (int)OldDimensionEnum.Size:
                                sizeActive = ((byte) dr["ACTIVE"] != 0);
                                break;

                            case (int)OldDimensionEnum.Color:
                                colorActive = ((byte) dr["ACTIVE"] != 0);
                                break;

                            case (int)OldDimensionEnum.Style:
                                styleActive = ((byte) dr["ACTIVE"] != 0);
                                break;
                        }
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }

                return sizeActive || colorActive || styleActive;
            }
        }
      
        /// <summary>
        /// Gets a dimension group from the database by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">ID of the group to fetch</param>
        /// <returns>The dimension group if found, else null</returns>
        public virtual OldDimensionGroup Get(IConnectionManager entry, RecordIdentifier groupID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select g.DIMGROUPID,ISNULL(g.NAME,'') as NAME, ISNULL(g.POSDISPLAYSETTING, 0) as POSDISPLAYSETTING," +
                    "ISNULL(ids_size.ACTIVE,0) as SIZEACTIVE, ISNULL(ids_color.ACTIVE,0) as COLORACTIVE," +
                    "ISNULL(ids_style.ACTIVE,0) as STYLEACTIVE,ISNULL(ids_serial.ACTIVE,0) as SERIALACTIVE," +
                    "ISNULL(ids_serial.ALLOWBLANKISSUE,0) as SERIALALLOWBLANK " +
                    "from INVENTDIMGROUP g " +
                    "left outer join INVENTDIMSETUP ids_size on g.DIMGROUPID = ids_size.DIMGROUPID and g.DATAAREAID = ids_size.DATAAREAID and ids_size.DIMFIELDID = " + (int)OldDimensionEnum.Size + " " +
                    "left outer join INVENTDIMSETUP ids_color on g.DIMGROUPID = ids_color.DIMGROUPID and g.DATAAREAID = ids_color.DATAAREAID and ids_color.DIMFIELDID = " + (int)OldDimensionEnum.Color + " " +
                    "left outer join INVENTDIMSETUP ids_style on g.DIMGROUPID = ids_style.DIMGROUPID and g.DATAAREAID = ids_style.DATAAREAID and ids_style.DIMFIELDID = " + (int)OldDimensionEnum.Style + " " +
                    "left outer join INVENTDIMSETUP ids_serial on g.DIMGROUPID = ids_serial.DIMGROUPID and g.DATAAREAID = ids_serial.DATAAREAID and ids_serial.DIMFIELDID = " + (int)OldDimensionEnum.Serial + " " +
                    "where g.DATAAREAID = @dataAreaId and g.DIMGROUPID = @dimGroupID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "dimGroupID", (string)groupID);

                var result = Execute<OldDimensionGroup>(entry, cmd, CommandType.Text, PopulateGroup);
                return (result.Count) > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Fetches all dimension groups from the database.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortBy">A enum defining how to sort the result</param>
        /// <param name="backwardsSort">Set to true if wanting backwards sort, else false</param>
        /// <returns>List of all dimension groups</returns>
        public virtual List<OldDimensionGroup> GetGroups(IConnectionManager entry, OldDimensionGroupSorting sortBy, bool backwardsSort)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select g.DIMGROUPID,ISNULL(g.NAME,'') as NAME, ISNULL(g.POSDISPLAYSETTING, 0) as POSDISPLAYSETTING," +
                    "ISNULL(ids_size.ACTIVE,0) as SIZEACTIVE, ISNULL(ids_color.ACTIVE,0) as COLORACTIVE," +
                    "ISNULL(ids_style.ACTIVE,0) as STYLEACTIVE,ISNULL(ids_serial.ACTIVE,0) as SERIALACTIVE," +
                    "ISNULL(ids_serial.ALLOWBLANKISSUE,0) as SERIALALLOWBLANK " +
                    "from INVENTDIMGROUP g " +
                    "left outer join INVENTDIMSETUP ids_size on g.DIMGROUPID = ids_size.DIMGROUPID and g.DATAAREAID = ids_size.DATAAREAID and ids_size.DIMFIELDID = " + (int)OldDimensionEnum.Size + " " +
                    "left outer join INVENTDIMSETUP ids_color on g.DIMGROUPID = ids_color.DIMGROUPID and g.DATAAREAID = ids_color.DATAAREAID and ids_color.DIMFIELDID = " + (int)OldDimensionEnum.Color + " " +
                    "left outer join INVENTDIMSETUP ids_style on g.DIMGROUPID = ids_style.DIMGROUPID and g.DATAAREAID = ids_style.DATAAREAID and ids_style.DIMFIELDID = " + (int)OldDimensionEnum.Style+ " " +
                    "left outer join INVENTDIMSETUP ids_serial on g.DIMGROUPID = ids_serial.DIMGROUPID and g.DATAAREAID = ids_serial.DATAAREAID and ids_serial.DIMFIELDID = " + (int)OldDimensionEnum.Serial + " " +
                    "where g.DATAAREAID = @dataAreaId order by " + ResolveSort(sortBy, backwardsSort);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<OldDimensionGroup>(entry, cmd, CommandType.Text, PopulateGroup);
            }
        }

        /// <summary>
        /// Deletes a inventory group by a given id from the database.
        /// </summary>
        /// <remarks>Requires the 'Manage item dimensions' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The group id to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier groupID)
        {
            DeleteRecord(entry, "INVENTDIMGROUP", "DIMGROUPID", groupID, DataLayer.BusinessObjects.Permission.ManageItemDimensions);
            DeleteRecord(entry, "INVENTDIMSETUP", "DIMGROUPID", groupID, DataLayer.BusinessObjects.Permission.ManageItemDimensions);
        }

        /// <summary>
        /// Checks if a dimension group by a given id exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The id to check for</param>
        /// <returns>True if the group existed, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier groupID)
        {
            return RecordExists(entry, "INVENTDIMGROUP", "DIMGROUPID", groupID);
        }

        private static bool SetupItemExists(IConnectionManager entry, RecordIdentifier groupID, OldDimensionEnum dimension)
        {
            return RecordExists(entry, "INVENTDIMSETUP", new[] { "DIMGROUPID", "DIMFIELDID" }, new RecordIdentifier(groupID, (int)dimension));
        }

        private static void SaveSetupItem(IConnectionManager entry, RecordIdentifier groupID, OldDimensionEnum dimension, bool active, bool allowBlank = false)
        {
            var statement = new SqlServerStatement("INVENTDIMSETUP");

            if (!SetupItemExists(entry, groupID, dimension))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddField("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddField("DIMGROUPID", (string)groupID);
                statement.AddField("DIMFIELDID", (int)dimension, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("DIMGROUPID", (string)groupID);
                statement.AddCondition("DIMFIELDID", (int)dimension, SqlDbType.Int);
            }

            statement.AddField("ACTIVE", active ? 1 : 0, SqlDbType.TinyInt);

            if (dimension == OldDimensionEnum.Serial)
            {
                statement.AddField("ALLOWBLANKISSUE", allowBlank ? 1 : 0, SqlDbType.TinyInt);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves a dimension group, inserting if the record does not exist, else updates.
        /// </summary>
        /// <remarks>Requires the 'Manage item dimensions' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="group">The group to save</param>
        public virtual void Save(IConnectionManager entry, OldDimensionGroup group)
        {
            var statement = new SqlServerStatement("INVENTDIMGROUP");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ManageItemDimensions);

            bool isNew = false;
            if (group.ID.IsEmpty)
            {
                isNew = true;
                group.ID = DataProviderFactory.Instance.GenerateNumber<IOldDimensionGroupData, OldDimensionGroup>(entry);
            }

            if (isNew || !Exists(entry, group.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("DIMGROUPID", (string)group.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("DIMGROUPID", (string)group.ID);
            }

            statement.AddField("NAME", group.Text);
            statement.AddField("POSDISPLAYSETTING", (int)group.PosDisplaySetting, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);

            SaveSetupItem(entry,group.ID, OldDimensionEnum.Size, group.SizeActive);  // Size
            SaveSetupItem(entry,group.ID, OldDimensionEnum.Color, group.ColorActive);  // Color
            SaveSetupItem(entry, group.ID, OldDimensionEnum.Style, group.StyleActive);  // Style
            SaveSetupItem(entry, group.ID, OldDimensionEnum.Serial, group.SerialActive,group.SerialAllowBlank);  // Serial
        }

        #region ISequenceable Members

        /// <summary>
        /// Return true if the passed in sequence id exists.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">Sequence ID to check for</param>
        /// <returns>
        /// True if the ID exists, else false
        /// </returns>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        /// <summary>
        /// In this function the ID of the sequence for the given type should be returned. This would be the ID from the Sequence table
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "VARIATIONGROUP"; }
        }

        #endregion

        /// <summary>
        /// Deletes all dimension combinations for a given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailItemID">The unique ID of the item</param>
        public virtual void DeleteDimensionCombinations(IConnectionManager entry, RecordIdentifier retailItemID)
        {
            var statement = new SqlServerStatement("INVENTDIMCOMBINATION");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ManageItemDimensions);

            statement.StatementType = StatementType.Delete;
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("ITEMID", (string)retailItemID);           

            entry.Connection.ExecuteStatement(statement);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            throw new System.NotImplementedException();
        }
    }
}
