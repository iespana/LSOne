using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster.Dimensions
{
    /// <summary>
    /// A data provider that gets data for the size dimension
    /// </summary>
    public class SizeData : SqlServerDataProviderBase, ISizeData
    {
        /// <summary>
        /// Tells the sequencer which ID should be created
        /// </summary>
        public enum SequenceSelector
        {
            /// <summary>
            /// Creates a sequence ID for a size
            /// </summary>
            SequenceSize,
            /// <summary>
            /// Creates a sequence ID for a size group
            /// </summary>
            SequenceSizeGroup,
        }

        private SequenceSelector sequenceSelector;

        public SizeData()
        {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SizeData"/> class.
        /// </summary>
        /// <param name="sequenceSelector">The sequence selector.</param>
        private void SetSequence(SequenceSelector sequenceSelector)
        {
            this.sequenceSelector = sequenceSelector;
        }

        /// <summary>
        /// Returns a list of groups that are available
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of available groups</returns>
        public virtual List<DataEntity> GetGroupList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOSIZEGROUPTABLE", "DESCRIPTION", "SIZEGROUP", "DESCRIPTION");
        }

        public virtual DataEntity GetGroup(IConnectionManager entry, RecordIdentifier groupID)
        {
            return GetDataEntity<DataEntity>(entry, "RBOSIZEGROUPTABLE", "DESCRIPTION", "SIZEGROUP", groupID);
        }
        
        /// <summary>
        /// Gets a list of sizes that are available
        /// </summary>
        /// <param name="entry">Entry into the database.</param>
        /// <returns>A list of available sizes</returns>
        public virtual List<DataEntity> GetSizeList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOSIZES", "NAME", "SIZE_", "SIZE_");
        }

        /// <summary>
        /// Returns true if the size is being used in a dimension combination
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sizeID">The size ID to search for</param>
        /// <returns>Returns true if the size is being used in a dimension combination</returns>
        public virtual bool SizeIsInUse(IConnectionManager entry, RecordIdentifier sizeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select TOP 1 ITEMID from INVENTDIMCOMBINATION where INVENTSIZEID = @sizeID and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "sizeID", (string) sizeID);

                var items = Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMID", "ITEMID");

                return (items.Count > 0);
            }
        }

        private static void PopulateSize(IDataReader dr, ItemWithDescription size)
        {
            size.ID = (string)dr["SIZE_"];
            size.Text = (string)dr["NAME"];
            size.Description = (string)dr["DESCRIPTION"];
        }

        /// <summary>
        /// Get information about a specific size
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sizeID">The size ID</param>
        /// <returns>Returns information about the size through the <see cref="ItemWithDescription"/> class</returns>
        public virtual ItemWithDescription GetSize(IConnectionManager entry, RecordIdentifier sizeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                cmd.CommandText =
                    "select SIZE_,ISNULL(NAME,'') as NAME, ISNULL(DESCRIPTION,'') as DESCRIPTION " +
                    "from RBOSIZES where DataareaID = @dataAreaId and SIZE_ = @sizeID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "sizeID", (string) sizeID);

                var items = Execute<ItemWithDescription>(entry, cmd, CommandType.Text, PopulateSize);
                return (items.Count > 0) ? items[0] : null;
            }
        }

        private static void PopulateSizeGroupLine(IDataReader dr, StyleGroupLineItem item)
        {
            item.ID = new RecordIdentifier((string)dr["Size_"], (string)dr["Sizegroup"]);
            item.Text = (string)dr["NAME"];
            item.NoInBarCode = (string)dr["NOINBARCODE"];
            item.Description = (string)dr["DESCRIPTION"];
            item.ItemName = (string)dr["SizeName"];
            item.SortingIndex = (int) dr["WEIGHT"];
        }

        /// <summary>
        /// Returns a list of sizes included in a size group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The size group ID.</param>
        /// <param name="sort">How to sort the result</param>
        /// <returns>A list of <see cref="StyleGroupLineItem"/> with information about the sizes</returns>
        public virtual List<StyleGroupLineItem> GetGroupLines(IConnectionManager entry, RecordIdentifier groupID, string sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                cmd.CommandText =
                    "select Sizegroup, Size_, ISNULL(NAME,'') as NAME, " +
                    "ISNULL(NOINBARCODE,'') as NOINBARCODE, ISNULL(DESCRIPTION,'') as DESCRIPTION,'' as SizeName, " +
                    "ISNULL(WEIGHT, 0) AS WEIGHT " +
                    "from RBOSIZEGROUPTRANS " +
                    "where DataareaID = @dataAreaId and Sizegroup = @sizeGroup " +
                    "ORDER BY WEIGHT";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "sizeGroup", (string) groupID);

                return Execute<StyleGroupLineItem>(entry, cmd, CommandType.Text, PopulateSizeGroupLine);
            }
        }

        /// <summary>
        /// Returns information about a specific size within a size group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The size group ID</param>
        /// <param name="sizeID">The size ID</param>
        /// <returns>Information about a size returned in an instance of <see cref="StyleGroupLineItem"/> </returns>
        public virtual StyleGroupLineItem GetGroupLine(IConnectionManager entry, RecordIdentifier groupID, RecordIdentifier sizeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                cmd.CommandText =
                    "select a.Sizegroup, a.Size_, ISNULL(a.NAME,'') as NAME, " +
                    "ISNULL(a.NOINBARCODE,'') as NOINBARCODE, ISNULL(a.DESCRIPTION,'') as DESCRIPTION,ISNULL(c.Name,'') as SizeName, " +
                    "ISNULL(a.WEIGHT, 0) AS WEIGHT " +
                    "from RBOSIZEGROUPTRANS a " +
                    "left outer join RBOSIZES c on a.Size_ = c.Size_ and a.DATAAREAID = c.DATAAREAID " +
                    "where a.DataareaID = @dataAreaId and a.Sizegroup = @sizeGroup and a.Size_ = @size";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "sizeGroup", (string) groupID);
                MakeParam(cmd, "size", (string) sizeID);

                var items = Execute<StyleGroupLineItem>(entry, cmd, CommandType.Text, PopulateSizeGroupLine);

                return (items.Count > 0) ? items[0] : null;
            }
        }

        /// <summary>
        /// Gets the available sizes from a specific size group
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="sizeGroup">The size group</param>
        /// <param name="includeSize">If set then the result is limited to groups that include his size ID</param>
        /// <returns>A list of available sizes for a specific size group</returns>
        public virtual List<DataEntity> GetAvailableSizes(IConnectionManager entry, RecordIdentifier sizeGroup, RecordIdentifier includeSize)
        {
            var orPart = "";
            using (var cmd = entry.Connection.CreateCommand())
            {
                if (includeSize != RecordIdentifier.Empty)
                {
                    orPart = " or c.Size_ = @includeSize ";
                }

                cmd.CommandText =
                    "Select c.Size_,c.Name,t.SIZEGROUP from RBOSIZES c " +
                    "left outer join RBOSIZEGROUPTRANS t on c.Size_ =t.Size_ and c.DATAAREAID = t.DATAAREAID and (t.SIZEGROUP = @sizeGroup) " +
                    "where (t.SIZEGROUP is NULL" + orPart + " ) and c.DATAAREAID = @dataAreaId " +
                    "order by c.Size_";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "sizeGroup", (string) sizeGroup);

                if (includeSize != RecordIdentifier.Empty)
                {
                    MakeParam(cmd, "includeSize", (string) includeSize);
                }

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "Name", "Size_");
            }
        }

        /// <summary>
        /// Returns true if the size group ID is included in a combination
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupLineID">The group ID.</param>
        /// <returns>Returns true if the size group ID is found</returns>
        public virtual bool GroupLineExists(IConnectionManager entry, RecordIdentifier groupLineID)
        {
            return RecordExists(entry, "RBOSIZEGROUPTRANS", new[] { "Size_", "Sizegroup" }, groupLineID);
        }

        /// <summary>
        /// Saves a given size item to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="item">The size item to save</param>
        /// <param name="oldID">If empty then a new size item will be created otherwise the
        /// size will be updated</param>
        public virtual void SaveGroupLine(IConnectionManager entry, StyleGroupLineItem item, RecordIdentifier oldID)
        {
            var statement = new SqlServerStatement("RBOSIZEGROUPTRANS");

            ValidateSecurity(entry, BusinessObjects.Permission.ColorSizeStyleEdit);

            if (oldID == RecordIdentifier.Empty)
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("SIZEGROUP", (string)item.ID.SecondaryID);
                statement.AddKey("SIZE_", (string)item.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("SIZEGROUP", (string)oldID.SecondaryID);
                statement.AddCondition("SIZE_", (string)oldID);

                statement.AddField("SIZEGROUP", (string)item.ID.SecondaryID);
                statement.AddField("SIZE_", (string)item.ID);
            }

            statement.AddField("NAME", item.Name);
            statement.AddField("NOINBARCODE", item.NoInBarCode);
            statement.AddField("DESCRIPTION", item.Description);
            statement.AddField("WEIGHT", item.SortingIndex, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Returns true if the size group already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sizeGroupID">The unique ID of size group to be checked</param>
        public virtual bool SizeGroupExists(IConnectionManager entry, RecordIdentifier sizeGroupID)
        {
            return RecordExists(entry, "RBOSIZEGROUPTABLE", "SIZEGROUP", sizeGroupID);
        }

        /// <summary>
        /// Returns true if the size exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sizeID">The unique ID of the size to be checked</param>
        /// <returns>
        /// Returns true if the size exists
        /// </returns>
        public virtual bool SizeExists(IConnectionManager entry, RecordIdentifier sizeID)
        {
            return RecordExists(entry, "RBOSIZES", "SIZE_", sizeID);
        }

        /// <summary>
        /// Deletes a given size 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sizeID">The unique ID of the size</param>
        public virtual void DeleteSize(IConnectionManager entry, RecordIdentifier sizeID)
        {
            DeleteRecord(entry, "RBOSIZES", "SIZE_", sizeID, BusinessObjects.Permission.ColorSizeStyleEdit);
        }

        /// <summary>
        /// Deletes a given size group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sizeGroupID">The unique ID of a size group</param>
        public virtual void DeleteSizeGroup(IConnectionManager entry, RecordIdentifier sizeGroupID)
        {
            DeleteRecord(entry, "RBOSIZEGROUPTABLE", "SIZEGROUP", sizeGroupID, BusinessObjects.Permission.ColorSizeStyleEdit);
            DeleteRecord(entry, "RBOSIZEGROUPTRANS", "SIZEGROUP", sizeGroupID, BusinessObjects.Permission.ColorSizeStyleEdit);
        }

        /// <summary>
        /// Deletes a size from a size group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupLineID">The unique ID of the size to be deleted</param>
        public virtual void DeleteGroupLine(IConnectionManager entry, RecordIdentifier groupLineID)
        {
            DeleteRecord(entry, "RBOSIZEGROUPTRANS", new[] { "Size_", "Sizegroup" }, groupLineID, BusinessObjects.Permission.ColorSizeStyleEdit);
        }

        /// <summary>
        /// Saves the given size to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="size">The size to be saved</param>
        public virtual void SaveSize(IConnectionManager entry, ItemWithDescription size)
        {
            var statement = new SqlServerStatement("RBOSIZES");

            ValidateSecurity(entry, BusinessObjects.Permission.ColorSizeStyleEdit);

            bool isNew = false;
            if (size.ID.IsEmpty)
            {
                isNew = true;
                SetSequence(SequenceSelector.SequenceSize);
                size.ID = DataProviderFactory.Instance.GenerateNumber(entry, this);
            }

            if (isNew || !SizeExists(entry, size.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("SIZE_", (string)size.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("SIZE_", (string)size.ID);
            }

            statement.AddField("NAME", size.Name);
            statement.AddField("DESCRIPTION", size.Description);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves a given size group to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sizeGroup">The size group to be saved</param>
        public virtual void SaveSizeGroup(IConnectionManager entry, DataEntity sizeGroup)
        {
            var statement = new SqlServerStatement("RBOSIZEGROUPTABLE");

            ValidateSecurity(entry, BusinessObjects.Permission.ColorSizeStyleEdit);

            bool isNew = false;
            if (sizeGroup.ID.IsEmpty)
            {
                isNew = true;
                SetSequence(SequenceSelector.SequenceSizeGroup);
                sizeGroup.ID = DataProviderFactory.Instance.GenerateNumber(entry, this);
            }

            if (isNew || !SizeGroupExists(entry, sizeGroup.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("SIZEGROUP", (string)sizeGroup.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("SIZEGROUP", (string)sizeGroup.ID);
            }

            statement.AddField("DESCRIPTION", sizeGroup.Text);

            entry.Connection.ExecuteStatement(statement);
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
            switch (sequenceSelector)
            {
                case SequenceSelector.SequenceSize:
                    return SizeExists(entry, id);

                case SequenceSelector.SequenceSizeGroup:
                    return SizeGroupExists(entry, id);

                default:
                    return false;
            }
        }

        /// <summary>
        /// In this function the ID of the sequence for the given type should be returned. This would be the ID from the Sequence table
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get
            {
                switch (sequenceSelector)
                {
                    case SequenceSelector.SequenceSize:
                        return "SIZE";

                    case SequenceSelector.SequenceSizeGroup:
                        return "SIZEGROUP";

                    default:
                        return "";
                }

            }
        }

        #endregion
    }
}
