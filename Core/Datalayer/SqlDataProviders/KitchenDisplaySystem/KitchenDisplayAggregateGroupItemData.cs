using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;
using System.Data;


namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayAggregateGroupItemData : SqlServerDataProviderBase, IKitchenDisplayAggregateGroupItemData
    {
        private string BaseSelectString
        {
            get
            {
                return @" SELECT ITEMID,
                                 GROUPID,
                                 ITEMDESCRIPTION,
                                 TYPE
					      FROM KITCHENDISPLAYAGGREGATEGROUPITEM ";
            }
        }

        public virtual AggregateGroupItem Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = 
                    BaseSelectString +
                    "WHERE ITEMID = @itemId AND GROUPID = @groupId";

                MakeParam(cmd, "itemId", id.PrimaryID);
                MakeParam(cmd, "groupId", id.SecondaryID);

                var result = Execute<AggregateGroupItem>(entry, cmd, CommandType.Text, PopulateAggregateGroupItem);
                return (result.Count == 1) ? result[0] : null;
            }
        }

        public virtual List<AggregateGroupItem> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSelectString;

                return Execute<AggregateGroupItem>(entry, cmd, CommandType.Text, PopulateAggregateGroupItem);
            }
        }

        public virtual List<AggregateGroupItem> GetList(IConnectionManager entry, RecordIdentifier aggregateGroupId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = 
                    BaseSelectString +
                    "WHERE GROUPID = @groupId";

                MakeParam(cmd, "groupId", aggregateGroupId);

                return Execute<AggregateGroupItem>(entry, cmd, CommandType.Text, PopulateAggregateGroupItem);
            }
        }

        public List<AggregateGroupItem> GetForKds(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ITEMID,
                                           GROUPID,
	                                       ITEMDESCRIPTION,
	                                       TYPE
                                    FROM KITCHENDISPLAYAGGREGATEGROUPITEM
                                    WHERE TYPE = 0

                                    UNION
  
                                    SELECT ri.ITEMID,
                                           kdagi.GROUPID,
		                                   ri.ITEMNAME AS ITEMDESCRIPTION,
		                                   kdagi.TYPE
                                    FROM KITCHENDISPLAYAGGREGATEGROUPITEM kdagi
                                    LEFT JOIN RETAILGROUP rg ON  kdagi.ITEMID = rg.GROUPID
                                    LEFT JOIN RETAILITEM ri ON rg.MASTERID = ri.RETAILGROUPMASTERID
                                    WHERE TYPE = 1 AND ri.ITEMID IS NOT NULL

                                    UNION

                                    SELECT ri.ITEMID,
                                           kdagi.GROUPID,
	                                       ri.ITEMNAME AS ITEMDESCRIPTION,
	                                       kdagi.TYPE
                                    FROM KITCHENDISPLAYAGGREGATEGROUPITEM kdagi 
                                    LEFT JOIN SPECIALGROUPITEMS sgi on sgi.GROUPID = kdagi.ITEMID
                                    LEFT JOIN RETAILITEM ri on ri.ITEMID = sgi.ITEMID
                                    WHERE kdagi.TYPE = 2 AND ri.ITEMID IS NOT NULL";

                return Execute<AggregateGroupItem>(entry, cmd, CommandType.Text, PopulateAggregateGroupItem);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYAGGREGATEGROUPITEM", new[] { "ITEMID" ,"GROUPID" }, id, Permission.ManageKitchenDisplayProfiles, false);
        }

        public virtual void DeleteByGroup(IConnectionManager entry, RecordIdentifier aggregateGroupID)
        {
            DeleteRecord(entry, "KITCHENDISPLAYAGGREGATEGROUPITEM", "GROUPID", aggregateGroupID, Permission.ManageKitchenDisplayProfiles, false);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYAGGREGATEGROUPITEM", new[] { "ITEMID", "GROUPID" }, id, false);
        }

        public virtual void Save(IConnectionManager entry, AggregateGroupItem aggregateGroupItem)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYAGGREGATEGROUPITEM");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayProfiles);

            statement.StatementType = StatementType.Insert;
            statement.AddKey("GROUPID", (string)aggregateGroupItem.GroupID);
            statement.AddKey("ITEMID", aggregateGroupItem.ItemID);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

            statement.AddField("ITEMDESCRIPTION", aggregateGroupItem.ItemDescription);
            statement.AddField("TYPE", (byte)aggregateGroupItem.Type, SqlDbType.TinyInt);

            Save(entry, aggregateGroupItem, statement);
        }

        public virtual void UpdateItem(IConnectionManager entry, AggregateGroupItem aggregateGroupItem, RecordIdentifier oldItemID)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYAGGREGATEGROUPITEM");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayProfiles);

            statement.StatementType = StatementType.Update;
            statement.AddCondition("GROUPID", (string)aggregateGroupItem.GroupID);
            statement.AddCondition("ITEMID", (string)oldItemID);
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);

            statement.AddField("ITEMID", aggregateGroupItem.ItemID);
            statement.AddField("ITEMDESCRIPTION", aggregateGroupItem.ItemDescription);

            Save(entry, aggregateGroupItem, statement);
        }

        public List<DataEntity> ItemsConnected(IConnectionManager entry, RecordIdentifier aggregateGroupID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @" SELECT ITEMID,
                              ITEMDESCRIPTION
                         FROM KITCHENDISPLAYAGGREGATEGROUPITEM 
                         WHERE TYPE = 0 and GROUPID = @groupID";

                MakeParam(cmd, "groupID", (string)aggregateGroupID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMDESCRIPTION", "ITEMID");
            }
        }

        public List<DataEntity> RetailGroupsConnected(IConnectionManager entry, RecordIdentifier aggregateGroupID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @" SELECT ITEMID,
                              ITEMDESCRIPTION
                         FROM KITCHENDISPLAYAGGREGATEGROUPITEM 
                         WHERE TYPE = 1 and GROUPID = @groupID";

                MakeParam(cmd, "groupID", (string)aggregateGroupID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMDESCRIPTION", "ITEMID");
            }
        }

        public List<DataEntity> SpecialGroupsConnected(IConnectionManager entry, RecordIdentifier aggregateGroupID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @" SELECT ITEMID,
                              ITEMDESCRIPTION
                         FROM KITCHENDISPLAYAGGREGATEGROUPITEM 
                         WHERE TYPE = 2 and GROUPID = @groupID";

                MakeParam(cmd, "groupID", (string)aggregateGroupID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMDESCRIPTION", "ITEMID");
            }
        }

        private void PopulateAggregateGroupItem(IDataReader dr, AggregateGroupItem aggregateGroupItem)
        {
            aggregateGroupItem.ItemID = (string)dr["ITEMID"];
            aggregateGroupItem.GroupID = (string)dr["GROUPID"];
            aggregateGroupItem.GroupID.SerializationData = (string)dr["GROUPID"];
            aggregateGroupItem.ItemDescription = aggregateGroupItem.Text = (string)dr["ITEMDESCRIPTION"];
            aggregateGroupItem.Type = (AggregateGroupItem.TypeEnum)(byte)dr["TYPE"];
        }
    }
}