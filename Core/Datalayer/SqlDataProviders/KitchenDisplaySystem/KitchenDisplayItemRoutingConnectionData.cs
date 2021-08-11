using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LSOne.DataLayer.BusinessObjects.Hospitality.StationSelection;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayItemRoutingConnectionData : SqlServerDataProviderBase, IKitchenDisplayItemRoutingConnectionData
    {
        private static string BaseSelectString
        {
            get
            {
                return @"   select 
                            kdic.ID
                            ,UPPER(kdic.STATIONID) as STATIONID
                            ,kdic.TYPE
                            ,kdic.CONNECTIONVALUE
                            ,kdic.INCLUDEEXCLUDE
                            ,ISNULL(it.ITEMNAME, '') as ITEMNAME
                            ,ISNULL(rg.NAME, '') as RETAILGROUPNAME
                            ,ISNULL(sg.NAME, '') as SPECIALGROUPNAME
                            from KITCHENDISPLAYITEMCONNECTION kdic 
                            left outer join RETAILITEM it on it.ITEMID = kdic.CONNECTIONVALUE 
                            left outer join RETAILGROUP rg on rg.GROUPID = kdic.CONNECTIONVALUE 
                            left outer join SPECIALGROUP sg on sg.GROUPID = kdic.CONNECTIONVALUE ";
            }
        }

        private static void PopulateRoutingConnection(IDataReader dr, LSOneKitchenDisplayItemRoutingConnection itemRouting)
        {
            if (!(dr["ID"] is DBNull))
            {
                itemRouting.Id = (Guid)dr["ID"];
            }
            itemRouting.StationId = (string)dr["STATIONID"];
            itemRouting.Type = (StationSelection.TypeEnum)dr["TYPE"];
            itemRouting.ConnectionId = (string)dr["CONNECTIONVALUE"];
            itemRouting.IncludeExclude = (LSOneKitchenDisplayItemRoutingConnection.IncludeEnum)(byte)dr["INCLUDEEXCLUDE"];

            switch (itemRouting.Type)
            {
                case StationSelection.TypeEnum.Item:
                    itemRouting.ConnectionDescription = (string)dr["ITEMNAME"];
                    break;
                case StationSelection.TypeEnum.RetailGroup:
                    itemRouting.ConnectionDescription = (string)dr["RETAILGROUPNAME"];
                    break;
                case StationSelection.TypeEnum.SpecialGroup:
                    itemRouting.ConnectionDescription = (string)dr["SPECIALGROUPNAME"];
                    break;
            }
        }

        public virtual LSOneKitchenDisplayItemRoutingConnection Get(IConnectionManager entry, RecordIdentifier itemRoutingId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where kdic.ID = @itemRoutingId and kdic.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemRoutingId", (Guid)itemRoutingId, SqlDbType.UniqueIdentifier);

                var results = Execute<LSOneKitchenDisplayItemRoutingConnection>(entry, cmd, CommandType.Text,
                                                                           PopulateRoutingConnection);
                return (results.Count == 1) ? results[0] : null;
            }
        }

        public List<LSOneKitchenDisplayItemRoutingConnection> GetForKds(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"WITH CONNECTIONS AS 
                                    (
	                                    (SELECT UPPER(kdic.STATIONID) as STATIONID,
			                                    TYPE = 2,
			                                    ri.ITEMID as CONNECTIONVALUE,
			                                    kdic.INCLUDEEXCLUDE,
			                                    ISNULL(ri.ITEMNAME, '') as ITEMNAME
	                                    FROM KITCHENDISPLAYITEMCONNECTION kdic 
	                                    JOIN RETAILITEM ri on kdic.TYPE = 0)

	                                    UNION

	                                    (SELECT UPPER(kdic.STATIONID) as STATIONID,
	                                            TYPE = 2,
	                                            ri.ITEMID as CONNECTIONVALUE,
	                                            kdic.INCLUDEEXCLUDE,
	                                            ISNULL(ri.ITEMNAME, '') as ITEMNAME
	                                    FROM KITCHENDISPLAYITEMCONNECTION kdic 
	                                    LEFT JOIN RETAILGROUP rg on rg.GROUPID = kdic.CONNECTIONVALUE
	                                    LEFT JOIN RETAILITEM ri on rg.MASTERID = ri.RETAILGROUPMASTERID
	                                    WHERE kdic.TYPE = 1
                                        AND ri.ITEMID IS NOT NULL)

	                                    UNION

	                                    (SELECT UPPER(kdic.STATIONID) as STATIONID,
	                                            TYPE = 2,
	                                            kdic.CONNECTIONVALUE,
	                                            kdic.INCLUDEEXCLUDE,
	                                            ISNULL(ri.ITEMNAME, '') as ITEMNAME
	                                    FROM KITCHENDISPLAYITEMCONNECTION kdic 
	                                    LEFT JOIN RETAILITEM ri on ri.ITEMID = kdic.CONNECTIONVALUE 
	                                    WHERE kdic.TYPE = 2)

	                                    UNION

	                                    (SELECT UPPER(kdic.STATIONID) as STATIONID,
	                                            TYPE = 2,
	                                            ri.ITEMID as CONNECTIONVALUE,
	                                            kdic.INCLUDEEXCLUDE,
	                                            ISNULL(ri.ITEMNAME, '') as ITEMNAME
	                                    FROM KITCHENDISPLAYITEMCONNECTION kdic 
	                                    LEFT JOIN SPECIALGROUPITEMS sgi on sgi.GROUPID = kdic.CONNECTIONVALUE
	                                    LEFT JOIN RETAILITEM ri on ri.ITEMID = sgi.ITEMID
	                                    WHERE kdic.TYPE = 3
                                        AND ri.ITEMID IS NOT NULL)
                                    )

                                    SELECT *, ID = NULL
                                    FROM CONNECTIONS c1
                                    WHERE NOT EXISTS (SELECT * 
                                                      FROM CONNECTIONS c2 
				                                      WHERE c1.STATIONID = c2.STATIONID AND c1.CONNECTIONVALUE = c2.CONNECTIONVALUE AND INCLUDEEXCLUDE = 1)";

                return Execute<LSOneKitchenDisplayItemRoutingConnection>(entry, cmd, CommandType.Text, PopulateRoutingConnection);
            }
        }

        public List<LSOneKitchenDisplayItemRoutingConnection> GetForKds(IConnectionManager entry, RecordIdentifier kdsId, List<string> connectedStations)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where UPPER(kdic.STATIONID) = @kdsId and kdic.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);

                return Execute<LSOneKitchenDisplayItemRoutingConnection>(entry, cmd, CommandType.Text, PopulateRoutingConnection);
            }
        }

        public LSOneKitchenDisplayItemRoutingConnection GetForKdsAndItemID(IConnectionManager entry,
                                                                             RecordIdentifier kdsId, DataEntity item)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where UPPER(kdic.STATIONID) = @kdsId and kdic.DATAAREAID = @dataAreaId and it.ITEMNAME = @itemName";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);
                MakeParam(cmd, "itemName", item.Text);

                var results = Execute<LSOneKitchenDisplayItemRoutingConnection>(entry, cmd, CommandType.Text,
                                                                           PopulateRoutingConnection);
                if (results == null || results.Count == 0)
                    return null;
                return results[0];
            }
        }

        public virtual LSOneKitchenDisplayItemRoutingConnection GetForKdsAndRetailGroupID(IConnectionManager entry, RecordIdentifier kdsId, DataEntity item)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where UPPER(kdic.STATIONID) = @kdsId and kdic.DATAAREAID = @dataAreaId and rg.NAME = @retailGroupName";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);
                MakeParam(cmd, "retailGroupName", item.Text);

                var results = Execute<LSOneKitchenDisplayItemRoutingConnection>(entry, cmd, CommandType.Text,
                                                                           PopulateRoutingConnection);
                if (results == null || results.Count == 0)
                    return null;
                return results[0];
            }
        }

        public virtual LSOneKitchenDisplayItemRoutingConnection GetForKdsAndSpecialGroupID(IConnectionManager entry, RecordIdentifier kdsId, DataEntity item)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where UPPER(kdic.STATIONID) = @kdsId and kdic.DATAAREAID = @dataAreaId and sg.NAME = @specialGroupName";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);
                MakeParam(cmd, "specialGroupName", item.Text);

                var results = Execute<LSOneKitchenDisplayItemRoutingConnection>(entry, cmd, CommandType.Text,
                                                                           PopulateRoutingConnection);
                if (results == null || results.Count == 0)
                    return null;
                return results[0];
            }
        }

        public List<DataEntity> SearchItemsNotConnectedToKds(
            IConnectionManager entry,
            RecordIdentifier kdsId,
            string searchString,
            int rowFrom,
            int rowTo,
            bool beginsWith)
        {
            string modifiedSearchString = (beginsWith ? "" : "%") + searchString + "%";

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"Select distinct s.ItemId as RETAILITEMID, s.ItemName as ITEMNAME
                    from 
                    (
                     Select i.ITEMID as ItemId, i.ITEMNAME, ROW_NUMBER() OVER(order by i.ITEMNAME) AS ROW
                     from INVENTTABLE i 
                     where i.DATAAREAID = @dataAreaId
                     and  (i.ITEMID Like @searchString or i.ITEMNAME Like @searchString)
                     and i.ITEMID not in 
                     (
                      select CONNECTIONVALUE from KITCHENDISPLAYITEMCONNECTION k where k.TYPE = 2 and UPPER(k.STATIONID) = @kdsId and k.DATAAREAID = @dataAreaId
                     )
                    ) s
                    Where s.ROW between " + rowFrom + " and " + rowTo;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);
                MakeParam(cmd, "searchString", modifiedSearchString);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "RETAILITEMID");
            }
        }

        public List<DataEntity> ItemsConnectedToKds(
            IConnectionManager entry,
            RecordIdentifier kdsId)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"Select distinct s.ItemId as RETAILITEMID, s.ItemName as ITEMNAME
                    from 
                    (
                     Select i.ITEMID as ItemId, i.ITEMNAME
                     from INVENTTABLE i 
                     where i.DATAAREAID = @dataAreaId
                     and i.ITEMID in 
                     (
                      select CONNECTIONVALUE from KITCHENDISPLAYITEMCONNECTION k where k.TYPE = 2 and UPPER(k.STATIONID) = @kdsId and k.DATAAREAID = @dataAreaId
                     )
                    ) s";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "RETAILITEMID");
            }
        }

        public virtual List<DataEntity> RetailGroupsNotConnectedToKds(IConnectionManager entry, RecordIdentifier kdsId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"Select r.GROUPID as RetailGroupId, ISNULL(r.Name,'') as RetailGroupName
                From RBOINVENTITEMRETAILGROUP r
                Where r.DATAAREAID = @dataAreaId
                and r.GROUPID not in 
                (Select k2.CONNECTIONVALUE 
                 From KITCHENDISPLAYITEMCONNECTION k2 
                 Where k2.DATAAREAID = @dataAreaId and k2.TYPE = @retailGroupEnumValue and UPPER(k2.STATIONID) = @kdsId
                )";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);
                MakeParam(cmd, "retailGroupEnumValue", (int)StationSelection.TypeEnum.RetailGroup, SqlDbType.Int);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "RetailGroupName", "RetailGroupId");
            }
        }

        public virtual List<DataEntity> RetailGroupsConnectedToKds(IConnectionManager entry, RecordIdentifier kdsId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"Select r.GROUPID as RetailGroupId, ISNULL(r.Name,'') as RetailGroupName
                From RBOINVENTITEMRETAILGROUP r
                Where r.DATAAREAID = @dataAreaId
                and r.GROUPID in 
                (Select k2.CONNECTIONVALUE 
                 From KITCHENDISPLAYITEMCONNECTION k2 
                 Where k2.DATAAREAID = @dataAreaId and k2.TYPE = @retailGroupEnumValue and UPPER(k2.STATIONID) = @kdsId
                )";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);
                MakeParam(cmd, "retailGroupEnumValue", (int)StationSelection.TypeEnum.RetailGroup, SqlDbType.Int);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "RetailGroupName", "RetailGroupId");
            }
        }

        public virtual List<DataEntity> SpecialGroupsNotConnectedToKds(IConnectionManager entry, RecordIdentifier kdsId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"Select s.GROUPID as SpecialGroupId, ISNULL(s.Name,'') as SpecialGroupName
                From RBOSPECIALGROUP s
                Where s.DATAAREAID = @dataAreaId
                and s.GROUPID not in 
                (Select k2.CONNECTIONVALUE 
                 From KITCHENDISPLAYITEMCONNECTION k2 
                 Where k2.DATAAREAID = @dataAreaId and k2.TYPE = @specialGroupEnumValue and UPPER(k2.STATIONID) = @kdsId
                )";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);
                MakeParam(cmd, "specialGroupEnumValue", (int)StationSelection.TypeEnum.SpecialGroup, SqlDbType.Int);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "SpecialGroupName", "SpecialGroupId");
            }
        }

        public virtual List<DataEntity> SpecialGroupsConnectedToKds(IConnectionManager entry, RecordIdentifier kdsId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"Select s.GROUPID as SpecialGroupId, ISNULL(s.Name,'') as SpecialGroupName
                From RBOSPECIALGROUP s
                Where s.DATAAREAID = @dataAreaId
                and s.GROUPID in 
                (Select k2.CONNECTIONVALUE 
                 From KITCHENDISPLAYITEMCONNECTION k2 
                 Where k2.DATAAREAID = @dataAreaId and k2.TYPE = @specialGroupEnumValue and UPPER(k2.STATIONID) = @kdsId
                )";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);
                MakeParam(cmd, "specialGroupEnumValue", (int)StationSelection.TypeEnum.SpecialGroup, SqlDbType.Int);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "SpecialGroupName", "SpecialGroupId");
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYITEMCONNECTION", "ID", id, Permission.ManageKitchenDisplayStations);
        }

        public virtual void DeleteByStation(IConnectionManager entry, RecordIdentifier stationId)
        {
            DeleteRecord(entry, "KITCHENDISPLAYITEMCONNECTION", "STATIONID", stationId, Permission.ManageKitchenDisplayStations);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYITEMCONNECTION", "ID", id);
        }

        public virtual void MakeSureAllTypeConnectionExists(IConnectionManager entry, string kdsId)
        {
            var allRoutingConnectionsForKds = GetForKds(entry, kdsId, null);
            var allTypeConnectionExists = allRoutingConnectionsForKds.Any(rule => rule.Type == StationSelection.TypeEnum.All);

            if (!allTypeConnectionExists)
            {
                CreateAllTypeConnection(entry, kdsId);
            }
        }

        private void CreateAllTypeConnection(IConnectionManager entry, string kdsId)
        {
            var newAllTypeConnection = new LSOneKitchenDisplayItemRoutingConnection
            {
                Id = Guid.NewGuid(),
                Type = StationSelection.TypeEnum.All,
                ConnectionId = "",
                StationId = kdsId
            };
            Save(entry, newAllTypeConnection);
        }

        public virtual void MakeSureAllTypeConnectionDoesntExists(IConnectionManager entry, RecordIdentifier kdsId)
        {
            var allRoutingConnectionsForKds = GetForKds(entry, kdsId, null);
            var allTypeConnections = allRoutingConnectionsForKds.Where(rule => rule.Type == StationSelection.TypeEnum.All);

            foreach (var connection in allTypeConnections)
            {
                Delete(entry, connection.Id);
            }
        }

        public virtual void Save(IConnectionManager entry, LSOneKitchenDisplayItemRoutingConnection itemRoutingConnection)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYITEMCONNECTION");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            var isNew = false;
            if (itemRoutingConnection.Id == Guid.Empty)
            {
                itemRoutingConnection.Id = Guid.NewGuid();
                isNew = true;
            }

            if (isNew || !Exists(entry, itemRoutingConnection.Id))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)itemRoutingConnection.Id, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)itemRoutingConnection.Id, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("STATIONID", (string)itemRoutingConnection.StationId.ToUpper());
            statement.AddField("TYPE", itemRoutingConnection.Type, SqlDbType.Int);
            statement.AddField("CONNECTIONVALUE", (string)itemRoutingConnection.ConnectionId);
            statement.AddField("INCLUDEEXCLUDE", (byte)itemRoutingConnection.IncludeExclude, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
