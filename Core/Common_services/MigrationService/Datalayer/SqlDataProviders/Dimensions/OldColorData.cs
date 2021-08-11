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
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Services.Datalayer.BusinessObjects.Dimensions;
using LSOne.Services.Datalayer.DataProviders.Dimensions;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.SqlDataProviders.Dimensions
{
    /// <summary>
    /// A data provider that gets data for the color dimension
    /// </summary>
    public class OldColorData : SqlServerDataProviderBase, IOldColorData
    {
        /// <summary>
        /// Tells the sequencer which ID should be created
        /// </summary>
        public enum SequenceSelector
        {
            /// <summary>
            /// Creates a sequence ID for a color
            /// </summary>
            SequenceColor,
            /// <summary>
            /// Creates a sequence ID for a color group
            /// </summary>
            SequenceColorGroup
        }

        private SequenceSelector sequenceSelector;

        public OldColorData()
        {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="OldColorData"/> class.
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
            return GetList<DataEntity>(entry, "RBOCOLORGROUPTABLE", "DESCRIPTION", "COLORGROUP", "DESCRIPTION");
        }

        /// <summary>
        /// Gets a list of colors that are available
        /// </summary>
        /// <param name="entry">Entry into the database.</param>
        /// <returns>A list of available colors</returns>
        public virtual List<DataEntity> GetColorList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOCOLORS", "NAME", "COLOR", "COLOR");
        }

        public virtual DataEntity GetGroup(IConnectionManager entry, RecordIdentifier groupID)
        {
            return GetDataEntity<DataEntity>(entry, "RBOCOLORGROUPTABLE", "DESCRIPTION", "COLORGROUP", groupID);
        }

        /// <summary>
        /// Returns true if the color is being used in a dimension combination
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="colorID">The color ID to search for</param>
        /// <returns>Returns true if the color is being used in a dimension combination</returns>
        public virtual bool ColorIsInUse(IConnectionManager entry, RecordIdentifier colorID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select TOP 1 ITEMID from INVENTDIMCOMBINATION where INVENTCOLORID = @colorID and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "colorID", (string) colorID);

                var items = Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMID", "ITEMID");
                return (items.Count > 0);
            }
        }

        /// <summary>
        /// Populates an instance of the class <see cref="ItemWithDescription"/> with information about the color contained in parameter dr
        /// </summary>
        /// <param name="dr">Information retrieved from the database about a color</param>
        /// <param name="color">The class to be populated with information from the database</param>
        private static void PopulateColor(IDataReader dr, OldItemWithDescription color)
        {
            color.ID = (string)dr["COLOR"];
            color.Text = (string)dr["NAME"];
            color.Description = (string)dr["DESCRIPTION"];
        }

        /// <summary>
        /// Get information about a specific color
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="colorID">The color ID</param>
        /// <returns>Returns information about the color through the <see cref="ItemWithDescription"/> class</returns>
        public virtual OldItemWithDescription GetColor(IConnectionManager entry, RecordIdentifier colorID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                cmd.CommandText =
                    "select COLOR,ISNULL(NAME,'') as NAME, ISNULL(DESCRIPTION,'') as DESCRIPTION " +
                    "from RBOCOLORS where DataareaID = @dataAreaId and COLOR = @colorID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "colorID", (string)colorID);

                var items = Execute<OldItemWithDescription>(entry, cmd, CommandType.Text, PopulateColor);
                return (items.Count > 0) ? items[0] : null;
            }
        }

        private static void PopulateColorGroupLine(IDataReader dr, OldStyleGroupLineItem item)
        {
            item.ID = new RecordIdentifier((string)dr["Color"], (string)dr["Colorgroup"]);
            item.Text = (string)dr["NAME"];
            item.NoInBarCode = (string)dr["NOINBARCODE"];
            item.Description = (string)dr["DESCRIPTION"];
            item.ItemName = (string)dr["ColorName"];
            item.SortingIndex = (int) dr["WEIGHT"];
        }

        /// <summary>
        /// Returns a list of colors included in a color group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The color group ID.</param>
        /// <param name="sort">How to sort the result</param>
        /// <returns>A list of <see cref="StyleGroupLineItem"/> with information about the colors</returns>
        public virtual List<OldStyleGroupLineItem> GetGroupLines(IConnectionManager entry, RecordIdentifier groupID, string sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                cmd.CommandText =
                    "select Colorgroup, Color, ISNULL(NAME,'') as NAME, " +
                    "ISNULL(NOINBARCODE,'') as NOINBARCODE, ISNULL(DESCRIPTION,'') as DESCRIPTION,'' as ColorName," +
                    " ISNULL(WEIGHT, 0) AS WEIGHT " +
                    "from RBOCOLORGROUPTRANS " +
                    "where DataareaID = @dataAreaId and Colorgroup = @colorGroup " +
                    "ORDER BY WEIGHT";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "colorGroup", (string)groupID);

                return Execute<OldStyleGroupLineItem>(entry, cmd, CommandType.Text, PopulateColorGroupLine);
            }
        }

        /// <summary>
        /// Returns information about a specific color within a color group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The color group ID</param>
        /// <param name="colorID">The color ID</param>
        /// <returns>Information about a color returned in an instance of <see cref="StyleGroupLineItem"/> </returns>
        public virtual OldStyleGroupLineItem GetGroupLine(IConnectionManager entry, RecordIdentifier groupID, RecordIdentifier colorID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                cmd.CommandText =
                    "select a.Colorgroup, a.Color, ISNULL(a.NAME,'') as NAME, " +
                    "ISNULL(a.NOINBARCODE,'') as NOINBARCODE, ISNULL(a.DESCRIPTION,'') as DESCRIPTION,ISNULL(c.Name,'') as ColorName, " +
                    "ISNULL(a.WEIGHT, 0) AS WEIGHT " +
                    "from RBOCOLORGROUPTRANS a " +
                    "left outer join RBOCOLORS c on a.Color = c.Color and a.DATAAREAID = c.DATAAREAID " +
                    "where a.DataareaID = @dataAreaId and a.Colorgroup = @colorGroup and a.Color = @color";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "colorGroup", (string) groupID);
                MakeParam(cmd, "color", (string) colorID);

                var items = Execute<OldStyleGroupLineItem>(entry, cmd, CommandType.Text, PopulateColorGroupLine);
                return (items.Count > 0) ? items[0] : null;
            }
        }

        /// <summary>
        /// Gets the available colors from a specific color group
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="colorGroup">The color group</param>
        /// <param name="includeColor">If set then the result is limited to groups that include his color ID</param>
        /// <returns>A list of available colors for a specific color group</returns>
        public virtual List<DataEntity> GetAvailableColors(IConnectionManager entry, RecordIdentifier colorGroup,RecordIdentifier includeColor)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string orPart = "";
                if (includeColor != RecordIdentifier.Empty)
                {
                    orPart = " or c.COLOR = @includeColor ";
                }

                cmd.CommandText =
                    "Select c.Color,c.Name,t.COLORGROUP from RBOCOLORS c " +
                    "left outer join RBOCOLORGROUPTRANS t on c.COLOR = t.COLOR and c.DATAAREAID = t.DATAAREAID and (t.COLORGROUP = @colorGroup) " +
                    "where (t.COLORGROUP is NULL" + orPart + " ) and c.DATAAREAID = @dataAreaId " +
                    "order by c.Color";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "colorGroup", (string) colorGroup);

                if (includeColor != RecordIdentifier.Empty)
                {
                    MakeParam(cmd, "includeColor", (string) includeColor);
                }

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "Name", "Color");
            }
        }

        /// <summary>
        /// Returns true if the color group ID is included in a combination
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupLineID">The group ID.</param>
        /// <returns>Returns true if the color group ID is found</returns>
        public virtual bool GroupLineExists(IConnectionManager entry, RecordIdentifier groupLineID)
        {
            return RecordExists(entry, "RBOCOLORGROUPTRANS", new[] { "Color", "Colorgroup" }, groupLineID);
        }

        /// <summary>
        /// Saves a given color item to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="item">The color item to save</param>
        /// <param name="oldID">If empty then a new color item will be created otherwise the
        /// color will be updated</param>
        public virtual void SaveGroupLine(IConnectionManager entry, OldStyleGroupLineItem item, RecordIdentifier oldID)
        {
            var statement = new SqlServerStatement("RBOCOLORGROUPTRANS");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);

            if (oldID == RecordIdentifier.Empty)
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("COLORGROUP", (string)item.ID.SecondaryID);
                statement.AddKey("COLOR", (string)item.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("COLORGROUP", (string)oldID.SecondaryID);
                statement.AddCondition("COLOR", (string)oldID);

                statement.AddField("COLORGROUP", (string)item.ID.SecondaryID);
                statement.AddField("COLOR", (string)item.ID);
            }

            statement.AddField("NAME", item.Name);
            statement.AddField("NOINBARCODE", item.NoInBarCode);
            statement.AddField("Description", item.Description);
            statement.AddField("WEIGHT", item.SortingIndex, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Returns true if the color group already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorGroupID">The unique ID of color group to be checked</param>
        public virtual bool ColorGroupExists(IConnectionManager entry, RecordIdentifier colorGroupID)
        {
            return RecordExists(entry, "RBOCOLORGROUPTABLE", "COLORGROUP", colorGroupID);
        }

        /// <summary>
        /// Returns true if the color exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorID">The unique ID of the color to be checked</param>
        /// <returns>
        /// Returns true if the color exists
        /// </returns>
        public virtual bool ColorExists(IConnectionManager entry, RecordIdentifier colorID)
        {
            return RecordExists(entry, "RBOCOLORS", "COLOR", colorID);
        }

        /// <summary>
        /// Deletes a given color 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorID">The unique ID of the color</param>
        public virtual void DeleteColor(IConnectionManager entry, RecordIdentifier colorID)
        {
            DeleteRecord(entry, "RBOCOLORS", "COLOR", colorID, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);
        }

        /// <summary>
        /// Deletes a given color group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorGroupID">The unique ID of a color group</param>
        public virtual void DeleteColorGroup(IConnectionManager entry, RecordIdentifier colorGroupID)
        {
            DeleteRecord(entry, "RBOCOLORGROUPTABLE", "COLORGROUP", colorGroupID, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);
            DeleteRecord(entry, "RBOCOLORGROUPTRANS", "COLORGROUP", colorGroupID, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);
        }

        /// <summary>
        /// Deletes a color from a color group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupLineID">The unique ID of the color to be deleted</param>
        public virtual void DeleteGroupLine(IConnectionManager entry, RecordIdentifier groupLineID)
        {
            DeleteRecord(entry, "RBOCOLORGROUPTRANS", new[] { "Color", "Colorgroup" }, groupLineID, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);
        }

        /// <summary>
        /// Saves the given color to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="color">The color to be saved</param>
        public virtual void SaveColor(IConnectionManager entry, OldItemWithDescription color)
        {
            var statement = new SqlServerStatement("RBOCOLORS");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);

            bool isNew = false;
            if (color.ID.IsEmpty)
            {
                isNew = true;

                SetSequence(SequenceSelector.SequenceColor);
                color.ID = DataProviderFactory.Instance.GenerateNumber(entry, this);
            }

            if (isNew || !ColorExists(entry,color.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("COLOR", (string)color.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("COLOR", (string)color.ID);
            }

            statement.AddField("NAME", color.Name);
            statement.AddField("DESCRIPTION", color.Description);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves a given color group to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorGroup">The color group to be saved</param>
        public virtual void SaveColorGroup(IConnectionManager entry, DataEntity colorGroup)
        {
            var statement = new SqlServerStatement("RBOCOLORGROUPTABLE");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);

            bool isNew = false;
            if (colorGroup.ID.IsEmpty)
            {
                isNew = true;
                SetSequence(SequenceSelector.SequenceColorGroup);
                colorGroup.ID = DataProviderFactory.Instance.GenerateNumber(entry, this);
            }

            if (isNew || !ColorGroupExists(entry, colorGroup.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("COLORGROUP", (string)colorGroup.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("COLORGROUP", (string)colorGroup.ID);
            }

            statement.AddField("DESCRIPTION", colorGroup.Text);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        /// <summary>
        /// Returns true if the unique ID already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The unique ID to be checked</param>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            switch (sequenceSelector)
            {
                case SequenceSelector.SequenceColor:
                    return ColorExists(entry, id);

                case SequenceSelector.SequenceColorGroup:
                    return ColorGroupExists(entry, id);

                default:
                    return false;
            }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returns a unique ID for either a color or color group depending on <see cref="SequenceSelector"/> set in the constructor.
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get 
            {
                switch (sequenceSelector)
                {
                    case SequenceSelector.SequenceColor:
                        return "COLOR"; 

                    case SequenceSelector.SequenceColorGroup:
                        return "COLORGROUP";

                    default:
                        return "";
                }
                
            }
        }

        #endregion
    }
}
