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
    /// A data provider that gets data for the style dimension
    /// </summary>
    public class StyleData : SqlServerDataProviderBase, IStyleData
    {
        /// <summary>
        /// Tells the sequencer which ID should be created
        /// </summary>
        public enum SequenceSelector
        {
            /// <summary>
            /// Creates a sequence ID for a style
            /// </summary>
            SequenceStyle,
            /// <summary>
            /// Creates a sequence ID for a style group
            /// </summary>
            SequenceStyleGroup,
        }

        private SequenceSelector sequenceSelector;

        public StyleData()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleData"/> class.
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
            return GetList<DataEntity>(entry, "RBOSTYLEGROUPTABLE", "DESCRIPTION", "STYLEGROUP", "DESCRIPTION");
        }

        public virtual DataEntity GetGroup(IConnectionManager entry, RecordIdentifier groupID)
        {
            return GetDataEntity<DataEntity>(entry, "RBOSTYLEGROUPTABLE", "DESCRIPTION", "STYLEGROUP", groupID);
        }

        /// <summary>
        /// Gets a list of styles that are available
        /// </summary>
        /// <param name="entry">Entry into the database.</param>
        /// <returns>A list of available styles</returns>
        public virtual List<DataEntity> GetStyleList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOSTYLES", "NAME", "STYLE", "STYLE");
        }

        /// <summary>
        /// Returns true if the style is being used in a dimension combination
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="styleID">The style ID to search for</param>
        /// <returns>Returns true if the style is being used in a dimension combination</returns>
        public virtual bool StyleIsInUse(IConnectionManager entry, RecordIdentifier styleID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select TOP 1 ITEMID from INVENTDIMCOMBINATION where INVENTSTYLEID = @styleID and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "styleID", (string) styleID);

                var items = Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMID", "ITEMID");

                return (items.Count > 0);
            }
        }

        private static void PopulateStyle(IDataReader dr, ItemWithDescription style)
        {
            style.ID = (string)dr["STYLE"];
            style.Text = (string)dr["NAME"];
            style.Description = (string)dr["DESCRIPTION"];
        }

        /// <summary>
        /// Get information about a specific style
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="styleID">The style ID</param>
        /// <returns>Returns information about the style through the <see cref="ItemWithDescription"/> class</returns>
        public virtual ItemWithDescription GetStyle(IConnectionManager entry, RecordIdentifier styleID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                cmd.CommandText =
                    "select STYLE,ISNULL(NAME,'') as NAME, ISNULL(DESCRIPTION,'') as DESCRIPTION " +
                    "from RBOSTYLES where DATAAREAID = @dataAreaId and STYLE = @styleID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "styleID", (string) styleID);

                var items = Execute<ItemWithDescription>(entry, cmd, CommandType.Text, PopulateStyle);
                return (items.Count > 0) ? items[0] : null;
            }
        }


        private static void PopulateStyleGroupLine(IDataReader dr, StyleGroupLineItem item)
        {
            item.ID = new RecordIdentifier((string)dr["STYLE"], (string)dr["STYLEGROUP"]);
            item.Text = (string)dr["NAME"];
            item.NoInBarCode = (string)dr["NOINBARCODE"];
            item.Description = (string)dr["DESCRIPTION"];
            item.ItemName = (string)dr["STYLENAME"];
        }

        /// <summary>
        /// Returns a list of styles included in a style group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The style group ID.</param>
        /// <param name="sort">How to sort the result</param>
        /// <returns>A list of <see cref="StyleGroupLineItem"/> with information about the styles</returns>
        public virtual List<StyleGroupLineItem> GetGroupLines(IConnectionManager entry, RecordIdentifier groupID, string sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                cmd.CommandText =
                    "select STYLEGROUP, STYLE, ISNULL(NAME,'') as NAME, " +
                    "ISNULL(NOINBARCODE,'') as NOINBARCODE, ISNULL(DESCRIPTION,'') as DESCRIPTION,'' as STYLENAME " +
                    "from RBOSTYLEGROUPTRANS " +
                    "where DATAAREAID = @dataAreaId and STYLEGROUP = @styleGroup " +
                    "ORDER BY WEIGHT";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "styleGroup", (string) groupID);

                return Execute<StyleGroupLineItem>(entry, cmd, CommandType.Text, PopulateStyleGroupLine);
            }
        }

        /// <summary>
        /// Returns information about a specific style within a style group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The style group ID</param>
        /// <param name="styleID">The style ID</param>
        /// <returns>Information about a style returned in an instance of <see cref="StyleGroupLineItem"/> </returns>
        public virtual StyleGroupLineItem GetGroupLine(IConnectionManager entry, RecordIdentifier groupID, RecordIdentifier styleID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                cmd.CommandText =
                    "select a.STYLEGROUP, a.STYLE, ISNULL(a.NAME,'') as NAME, " +
                    "ISNULL(a.NOINBARCODE,'') as NOINBARCODE, ISNULL(a.DESCRIPTION,'') as DESCRIPTION,ISNULL(c.NAME,'') as STYLENAME " +
                    "from RBOSTYLEGROUPTRANS a " +
                    "left outer join RBOSTYLES c on a.STYLE = c.STYLE and a.DATAAREAID = c.DATAAREAID " +
                    "where a.DATAAREAID = @dataAreaId and a.STYLEGROUP = @styleGroup and a.STYLE = @style";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "styleGroup", (string) groupID);
                MakeParam(cmd, "style", (string) styleID);

                var items = Execute<StyleGroupLineItem>(entry, cmd, CommandType.Text, PopulateStyleGroupLine);

                return (items.Count > 0) ? items[0] : null;
            }
        }

        /// <summary>
        /// Gets the available styles from a specific style group
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="styleGroup">The style group</param>
        /// <param name="includeStyle">If set then the result is limited to groups that include his style ID</param>
        /// <returns>A list of available styles for a specific style group</returns>
        public virtual List<DataEntity> GetAvailableStyles(IConnectionManager entry, RecordIdentifier styleGroup, RecordIdentifier includeStyle)
        {
            string orPart = "";
            var cmd = entry.Connection.CreateCommand();

            if (includeStyle != RecordIdentifier.Empty)
            {
                orPart = " or c.STYLE = @includeStyle ";
            }

            cmd.CommandText =
                "Select c.STYLE,c.NAME,t.STYLEGROUP from RBOSTYLES c " +
                "left outer join RBOSTYLEGROUPTRANS t on c.STYLE =t.STYLE and c.DATAAREAID = t.DATAAREAID and (t.STYLEGROUP = @styleGroup) " +
                "where (t.STYLEGROUP is NULL" + orPart + " ) and c.DATAAREAID = @dataAreaId " +
                "order by c.STYLE";

            MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
            MakeParam(cmd, "styleGroup", (string)styleGroup);

            if (includeStyle != RecordIdentifier.Empty)
            {
                MakeParam(cmd, "includeStyle", (string)includeStyle);
            }

            return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "STYLE");
        }

        /// <summary>
        /// Returns true if the style group ID is included in a combination
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupLineID">The group ID.</param>
        /// <returns>Returns true if the style group ID is found</returns>
        public virtual bool GroupLineExists(IConnectionManager entry, RecordIdentifier groupLineID)
        {
            return RecordExists(entry, "RBOSTYLEGROUPTRANS", new[] { "STYLE", "STYLEGROUP" }, groupLineID);
        }

        /// <summary>
        /// Saves a given style item to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="item">The style item to save</param>
        /// <param name="oldID">If empty then a new style item will be created otherwise the
        /// style will be updated</param>
        public virtual void SaveGroupLine(IConnectionManager entry, StyleGroupLineItem item, RecordIdentifier oldID)
        {
            var statement = new SqlServerStatement("RBOSTYLEGROUPTRANS");

            ValidateSecurity(entry, BusinessObjects.Permission.ColorSizeStyleEdit);

            if (oldID == RecordIdentifier.Empty)
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("STYLEGROUP", (string)item.ID.SecondaryID);
                statement.AddKey("STYLE", (string)item.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("STYLEGROUP", (string)oldID.SecondaryID);
                statement.AddCondition("STYLE", (string)oldID);

                statement.AddField("STYLEGROUP", (string)item.ID.SecondaryID);
                statement.AddField("STYLE", (string)item.ID);
            }

            statement.AddField("NAME", item.Name);
            statement.AddField("NOINBARCODE", item.NoInBarCode);
            statement.AddField("Description", item.Description);
            statement.AddField("WEIGHT", item.SortingIndex, SqlDbType.Int);
            
            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Returns true if the style group already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleGroupID">The unique ID of style group to be checked</param>
        public virtual bool StyleGroupExists(IConnectionManager entry, RecordIdentifier styleGroupID)
        {
            return RecordExists(entry, "RBOSTYLEGROUPTABLE", "STYLEGROUP", styleGroupID);
        }

        /// <summary>
        /// Returns true if the style exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleID">The unique ID of the style to be checked</param>
        /// <returns>
        /// Returns true if the style exists
        /// </returns>
        public virtual bool StyleExists(IConnectionManager entry, RecordIdentifier styleID)
        {
            return RecordExists(entry, "RBOSTYLES", "STYLE", styleID);
        }

        /// <summary>
        /// Deletes a given style 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleID">The unique ID of the style</param>
        public virtual void DeleteStyle(IConnectionManager entry, RecordIdentifier styleID)
        {
            DeleteRecord(entry, "RBOSTYLES", "STYLE", styleID, BusinessObjects.Permission.ColorSizeStyleEdit);
        }

        /// <summary>
        /// Deletes a given style group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleGroupID">The unique ID of a style group</param>
        public virtual void DeleteStyleGroup(IConnectionManager entry, RecordIdentifier styleGroupID)
        {
            DeleteRecord(entry, "RBOSTYLEGROUPTABLE", "STYLEGROUP", styleGroupID, BusinessObjects.Permission.ColorSizeStyleEdit);
            DeleteRecord(entry, "RBOSTYLEGROUPTRANS", "STYLEGROUP", styleGroupID, BusinessObjects.Permission.ColorSizeStyleEdit);
        }

        /// <summary>
        /// Deletes a style from a style group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupLineID">The unique ID of the style to be deleted</param>
        public virtual void DeleteGroupLine(IConnectionManager entry, RecordIdentifier groupLineID)
        {
            DeleteRecord(entry, "RBOSTYLEGROUPTRANS", new[] { "STYLE", "Stylegroup" }, groupLineID, BusinessObjects.Permission.ColorSizeStyleEdit);
        }

        /// <summary>
        /// Saves the given style to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="style">The style to be saved</param>
        public virtual void SaveStyle(IConnectionManager entry, ItemWithDescription style)
        {
            var statement = new SqlServerStatement("RBOSTYLES");

            ValidateSecurity(entry, BusinessObjects.Permission.ColorSizeStyleEdit);

            bool isNew = false;
            if (style.ID.IsEmpty)
            {
                isNew = true;
                SetSequence(SequenceSelector.SequenceStyle);
                style.ID = DataProviderFactory.Instance.GenerateNumber(entry, this);
            }

            if (isNew || !StyleExists(entry, style.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("STYLE", (string)style.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("STYLE", (string)style.ID);
            }

            statement.AddField("NAME", style.Name);
            statement.AddField("DESCRIPTION", style.Description);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves a given style group to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleGroup">The style group to be saved</param>
        public virtual void SaveStyleGroup(IConnectionManager entry, DataEntity styleGroup)
        {
            var statement = new SqlServerStatement("RBOSTYLEGROUPTABLE");

            ValidateSecurity(entry, BusinessObjects.Permission.ColorSizeStyleEdit);

            bool isNew = false;
            if (styleGroup.ID.IsEmpty)
            {
                isNew = true;
                SetSequence(SequenceSelector.SequenceStyleGroup);
                styleGroup.ID = DataProviderFactory.Instance.GenerateNumber(entry, this);
            }

            if (isNew || !StyleGroupExists(entry, styleGroup.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("STYLEGROUP", (string)styleGroup.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("STYLEGROUP", (string)styleGroup.ID);
            }

            statement.AddField("DESCRIPTION", styleGroup.Text);

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
                case SequenceSelector.SequenceStyle:
                    return StyleExists(entry, id);

                case SequenceSelector.SequenceStyleGroup:
                    return StyleGroupExists(entry, id);

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
                    case SequenceSelector.SequenceStyle:
                        return "STYLE";

                    case SequenceSelector.SequenceStyleGroup:
                        return "STYLEGROUP";

                    default:
                        return "";
                }

            }
        }

        #endregion

    }
}
