using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
    /// <summary>
    /// Data provider class for linked items
    /// </summary>
    public class LinkedItemData : SqlServerDataProviderBase, ILinkedItemData
    {
        private static string BaseSql
        {
            get
            {
                return "SELECT r.ITEMID as ORIGINALITEMID " +
                      ",r.UNIT as LINKEDITEMUNITID " +
                      ",r.LINKEDITEMID as LINKEDITEMID " +
                      ",COALESCE(r.QTY,0) as LINKEDITEMQUANTITY " +
                      ",COALESCE(r.BLOCKED,0) as BLOCKED " +
                      ",COALESCE(it.ITEMNAME,'') as LINKEDITEMDESCRIPTION " +
                      ",COALESCE(it.VARIANTNAME,'') as LINKEDITEMVARIANTDESCRIPTION " +
                      ",COALESCE(u.TXT,'') as LINKEDITEMUNITDESCRIPTION " +
                      ",COALESCE(u.UNITDECIMALS,'') as MAXUNITDECIMALS " +
                      ",COALESCE(u.MINUNITDECIMALS,'') as MINUNITDECIMALS " +
                  "FROM RBOINVENTLINKEDITEM r " +
                  "JOIN RETAILITEM it on it.ITEMID = r.LINKEDITEMID " +
                  "JOIN UNIT u on u.UNITID = r.UNIT and u.DATAAREAID = r.DATAAREAID ";

            }
        }

        /// <summary>
        /// Creates an "order by" string for a SQL statement depending on class settings and
        /// parameters
        /// </summary>
        /// <param name="sortEnum">Controls which columns will be sorted by in the SQL
        /// statement</param>
        /// <param name="sortBackwards">If true then the ordering will be descending
        /// otherwise it will be ascending</param>
        private static string ResolveSort(LinkedItem.SortEnum sortEnum, bool sortBackwards)
        {
            var sortString = " Order by ";

            switch (sortEnum)
            {
                case LinkedItem.SortEnum.LinkedItemID:
                    sortString += "LINKEDITEMID ASC";
                    break;
                case LinkedItem.SortEnum.LinkedItemsUnitID:
                    sortString += "LINKEDITEMSUNITID ASC";
                    break;
                case LinkedItem.SortEnum.Blocked:
                    sortString += "BLOCKED ASC";
                    break;
                case LinkedItem.SortEnum.LinkedItemQuantity:
                    sortString += "LINKEDITEMQUANTITY ASC";
                    break;
            }

            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static void PopulateLinkedItem(IDataReader dr, LinkedItem item)
        {
            item.OriginalItemID = (string)dr["ORIGINALITEMID"];
            item.LinkedItemID = (string)dr["LINKEDITEMID"];
            item.LinkedItemDescription = (string)dr["LINKEDITEMDESCRIPTION"];
            item.LinkedItemVariantDescription = (string)dr["LINKEDITEMVARIANTDESCRIPTION"];
            item.LinkedItemUnitID = (string)dr["LINKEDITEMUNITID"];
            item.LinkedItemUnitDescription = (string)dr["LINKEDITEMUNITDESCRIPTION"];
            item.LinkedItemQuantity = (decimal)dr["LINKEDITEMQUANTITY"];
            item.Blocked = ((int)dr["Blocked"] == 1);

            item.UnitLimiter = new DecimalLimit((int)dr["MINUNITDECIMALS"], (int)dr["MAXUNITDECIMALS"]);
        }

        /// <summary>
        /// Returns a list of linked items
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item to search for</param>
        /// <param name="sortEnum">Controls which columns will be sorted by in the SQL
        /// statement</param>
        /// <param name="sortBackwards">If true then the ordering will be descending
        /// otherwise it will be ascending</param>
        public List<LinkedItem> GetLinkedItems(
            IConnectionManager entry, 
            RecordIdentifier itemID, 
            LinkedItem.SortEnum sortEnum, 
            bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText = BaseSql +
                                  "WHERE r.DATAAREAID = @dataAreaID AND r.ITEMID = @originalItemID  "
                                  + ResolveSort(sortEnum, sortBackwards);


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "originalItemID", (string)itemID);

                return Execute<LinkedItem>(entry, cmd, CommandType.Text, PopulateLinkedItem);
            }
        }

        /// <summary>
        /// Returns information about a linked item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="linkedItemID">The unique ID of the linked item</param>
        /// <param name="cache">Optional parameter to specify if cache may be used</param>
        /// <returns>
        /// Returns an instance of <see cref="LinkedItem"/>
        /// </returns>
        public virtual LinkedItem Get(IConnectionManager entry, RecordIdentifier linkedItemID, CacheType cache = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql +
                                  "WHERE r.DATAAREAID = @dataAreaID AND r.ITEMID = @originalItemID AND r.LINKEDITEMID = @linkedItemID AND r.UNIT = @linkedUnitID ";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "originalItemID", (string)linkedItemID[0]);
                MakeParam(cmd, "linkedItemID", (string)linkedItemID[1]);
                MakeParam(cmd, "linkedUnitID", (string)linkedItemID[2]);

                return Get<LinkedItem>(entry, cmd, linkedItemID, PopulateLinkedItem, cache, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Deletes an instance of the given class
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="linkedItemID">The unique ID of the linked item</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier linkedItemID)
        {
            if(linkedItemID.HasSecondaryID)
            {
                DeleteRecord(entry, "RBOINVENTLINKEDITEM", new[] { "ITEMID", "LINKEDITEMID", "UNIT" }, linkedItemID, BusinessObjects.Permission.ManageLinkedItems);
            }
            else
            {
                DeleteRecord(entry, "RBOINVENTLINKEDITEM", "LINKEDITEMID" , linkedItemID, BusinessObjects.Permission.ManageLinkedItems);
            }
        }

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="linkedItemID">The unique ID of the linked item to check on</param>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier linkedItemID)
        {
            return RecordExists(entry, "RBOINVENTLINKEDITEM", new[] { "ITEMID", "LINKEDITEMID", "UNIT" }, linkedItemID);
        }

        /// <summary>
        /// Saves the given class to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="linkedItem">The linked item to be saved to the database</param>
        public virtual void Save(IConnectionManager entry, LinkedItem linkedItem)
        {
            var statement = new SqlServerStatement("RBOINVENTLINKEDITEM");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageLinkedItems);

            if (!Exists(entry, linkedItem.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ITEMID", (string)linkedItem.ID[0]);
                statement.AddKey("LINKEDITEMID", (string)linkedItem.ID[1]);
                statement.AddKey("UNIT", (string)linkedItem.ID[2]);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ITEMID", (string)linkedItem.ID[0]);
                statement.AddCondition("LINKEDITEMID", (string)linkedItem.ID[1]);
                statement.AddCondition("UNIT", (string)linkedItem.ID[2]);
            }

            statement.AddField("QTY", linkedItem.LinkedItemQuantity, SqlDbType.Decimal);
            statement.AddField("BLOCKED", linkedItem.Blocked, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
