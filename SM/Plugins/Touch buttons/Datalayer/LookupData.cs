using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using LSRetail.StoreController.SharedDatabase;
using LSRetail.StoreController.SharedDatabase.DataEntities;
using LSRetail.StoreController.SharedDatabase.DataProviders;
using LSRetail.StoreController.SharedDatabase.Interfaces;
using LSRetail.Utilities.DataTypes;

namespace LSRetail.StoreController.TouchButtons.Datalayer
{
    internal class LookupData : DataProviderBase
    {
        /// <summary>
        /// This inner class is only used as structure
        /// </summary>
        internal class Item
        {
            public RecordIdentifier ID;
            public string Name;
            public RecordIdentifier ItemGroupID;
            public string ItemGroupName;

            public Item()
            {
                ID = RecordIdentifier.Empty;
                Name = "";
                ItemGroupID = RecordIdentifier.Empty;
                ItemGroupName = "";
            }
        }

        private static void PopulateItem(SqlDataReader dr, Item item)
        {
            item.ID = (string)dr["ItemId"];
            item.Name = (string)dr["ItemName"];
            item.ItemGroupID = (string)dr["ItemGroupId"];
            item.ItemGroupName = (string)dr["ItemGroupName"];
        }

        public static List<Item> SearchItems(IConnectionManager entry, string text, int maxCount,string sortBy,string asc)
        {
            SqlCommand cmd = new SqlCommand();
            int fromRow = 0;

            ValidateSecurity(entry);

            string search = " where (it.ItemName like @SearchString or it.ItemGroupId like @SearchString or it.ItemId like @SearchString) ";

            cmd.CommandText = "Select I.ItemGroupId, I.ItemId, I.ItemName, I.ItemGroupName " +
                           "from ( " +
                           "    select it.ItemGroupId,it.ItemID,ISNULL(it.ItemName,'') as ItemName,it.DATAAREAID,ISNULL(ig.Name,it.ItemGroupId) as ItemGroupName, ROW_NUMBER() " +
                           "    over (order by it." + sortBy + " " + asc + ") as row " +
                           "    from InventTable it " +
                           "    left outer join INVENTITEMGROUP ig on ig.DATAAREAID = it.DATAAREAID and it.ItemGroupId = ig.ItemGroupId " + search + ") I " +
                           "where I.DATAAREAID=@DATAAREAID and I.row > " + fromRow.ToString() + " and I.row <= " + (fromRow + maxCount + 1 ).ToString();



            MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
            MakeParam(cmd, "SearchString", "%" + text + "%");

            return Execute<Item>(entry, cmd, CommandType.Text, PopulateItem);
        }
    }
}
