using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayTerminalRoutingConnectionData : SqlServerDataProviderBase, IKitchenDisplayTerminalRoutingConnectionData
    {        
        private static string BaseSelectString
        {
            get
            {
                return @"   select 
                            kdtc.ID
                            ,UPPER(kdtc.STATIONID) as STATIONID
                            ,kdtc.TYPE
                            ,kdtc.CONNECTIONVALUE
                            ,kdtc.INCLUDEEXCLUDE
                            ,kdtc.TERMINALID
                            ,kdtc.STOREID
                            ,ISNULL(tt.NAME, '') as TERMINALNAME
                            ,ISNULL(tg.DESCRIPTION, '') as TERMINALGROUPNAME
                            ,ISNULL(st.NAME, '') as STORENAME
                            from KITCHENDISPLAYTERMINALCONNECTION kdtc 
                            left outer join RBOTERMINALTABLE tt on tt.TERMINALID = kdtc.TERMINALID and tt.STOREID = kdtc.STOREID and tt.DATAAREAID = kdtc.DATAAREAID 
                            left outer join RBOTERMINALGROUP tg on tg.ID = kdtc.CONNECTIONVALUE and tg.DATAAREAID = kdtc.DATAAREAID
                            left outer join RBOSTORETABLE st on st.STOREID = kdtc.STOREID and st.DATAAREAID = kdtc.DATAAREAID ";
            }
        }

        private static void PopulateRoutingConnection(IDataReader dr, LSOneKitchenDisplayTerminalRoutingConnection terminalRouting)
        {
            if (!(dr["ID"] is DBNull))
            {
                terminalRouting.Id = (Guid)dr["ID"];
            }
            terminalRouting.StationId = (string)dr["STATIONID"];
            terminalRouting.Type = (StationSelection.TerminalConnectionEnum)dr["TYPE"];
            terminalRouting.ConnectionId = (string)dr["CONNECTIONVALUE"];
            terminalRouting.IncludeExclude = (LSOneKitchenDisplayTerminalRoutingConnection.IncludeEnum)(byte)dr["INCLUDEEXCLUDE"];
            terminalRouting.TerminalID = (string)dr["TERMINALID"];            
            terminalRouting.StoreID = (string)dr["STOREID"];
            terminalRouting.TerminalName = (string)dr["TERMINALNAME"];
            terminalRouting.StoreName = (string)dr["STORENAME"];

            switch (terminalRouting.Type)
            {
                case StationSelection.TerminalConnectionEnum.Terminal:
                    terminalRouting.ConnectionDescription = $"{terminalRouting.StoreName} - {terminalRouting.TerminalName}";
                    break;
                case StationSelection.TerminalConnectionEnum.TerminalGroup:
                    terminalRouting.ConnectionDescription = (string)dr["TERMINALGROUPNAME"];
                    break;
            }
        }

        public LSOneKitchenDisplayTerminalRoutingConnection Get(IConnectionManager entry, RecordIdentifier terminalRoutingId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where kdtc.ID = @terminalRoutingId and kdtc.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "terminalRoutingId", (Guid)terminalRoutingId, SqlDbType.UniqueIdentifier);

                var results = Execute<LSOneKitchenDisplayTerminalRoutingConnection>(entry, cmd, CommandType.Text,
                                                                               PopulateRoutingConnection);
                return (results.Count == 1) ? results[0] : null;
            }
        }

        public List<LSOneKitchenDisplayTerminalRoutingConnection> GetForKds(IConnectionManager entry, RecordIdentifier kdsId, List<string> connectedStations)
        {            
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where UPPER(kdtc.STATIONID) = @kdsId and kdtc.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);

                return Execute<LSOneKitchenDisplayTerminalRoutingConnection>(entry, cmd, CommandType.Text,
                    PopulateRoutingConnection);
            }            
        }

        public List<LSOneKitchenDisplayTerminalRoutingConnection> GetForKds(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"WITH CONNECTIONS AS 
                                    (
	                                    (SELECT UPPER(kdtc.STATIONID) AS STATIONID,
			                                    TYPE = 1,
			                                    tt.TERMINALID + '-' + tt.STOREID as CONNECTIONVALUE,
			                                    kdtc.INCLUDEEXCLUDE,
			                                    ISNULL(tt.NAME, '') AS TERMINALNAME,
			                                    ISNULL(st.NAME, '') AS STORENAME,
			                                    kdtc.TERMINALID,
			                                    kdtc.STOREID
	                                    FROM KITCHENDISPLAYTERMINALCONNECTION kdtc
	                                    JOIN RBOTERMINALTABLE tt ON kdtc.TYPE = 0
	                                    JOIN RBOSTORETABLE st ON kdtc.TYPE = 0)

	                                    UNION

	                                    (SELECT UPPER(kdtc.STATIONID) as STATIONID,
			                                    TYPE = 1,
			                                    kdtc.CONNECTIONVALUE,
			                                    kdtc.INCLUDEEXCLUDE,
			                                    ISNULL(tt.NAME, '') as TERMINALNAME,
			                                    ISNULL(st.NAME, '') AS STORENAME,
			                                    kdtc.TERMINALID,
			                                    kdtc.STOREID
	                                    FROM KITCHENDISPLAYTERMINALCONNECTION kdtc
	                                    LEFT JOIN RBOTERMINALTABLE tt on tt.TERMINALID = kdtc.TERMINALID AND tt.STOREID = kdtc.STOREID  
	                                    LEFT JOIN RBOSTORETABLE st on st.STOREID = kdtc.STOREID
	                                    WHERE kdtc.TYPE = 1)
                                    )

                                    SELECT *, ID = NULL
                                    FROM CONNECTIONS c1
                                    WHERE NOT EXISTS (SELECT * 
                                                        FROM CONNECTIONS c2 
				                                        WHERE c1.STATIONID = c2.STATIONID AND c1.CONNECTIONVALUE = c2.CONNECTIONVALUE AND INCLUDEEXCLUDE = 1)";

                return Execute<LSOneKitchenDisplayTerminalRoutingConnection>(entry, cmd, CommandType.Text,
                    PopulateRoutingConnection);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYTERMINALCONNECTION", "ID", id, Permission.ManageKitchenDisplayStations);
        }

        public virtual void DeleteByStation(IConnectionManager entry, RecordIdentifier stationId)
        {
            DeleteRecord(entry, "KITCHENDISPLAYTERMINALCONNECTION", "STATIONID", stationId, Permission.ManageKitchenDisplayStations);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYTERMINALCONNECTION", "ID", id);
        }

        public virtual List<DataEntity> TerminalsNotConnectedToKds(IConnectionManager entry, RecordIdentifier kdsId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT t.TERMINALID,
	                         t.STOREID,
	                         ISNULL(t.NAME,'') AS TERMINALNAME
                      FROM RBOTERMINALTABLE t
                      WHERE t.DATAAREAID = @dataAreaId
                      AND (t.TERMINALID + '-' + t.STOREID) NOT IN 
                      (
	                      SELECT k.CONNECTIONVALUE 
                          FROM KITCHENDISPLAYTERMINALCONNECTION k 
                          WHERE k.DATAAREAID = @dataAreaId AND k.TYPE = @terminalEnumValue AND UPPER(k.STATIONID) = @kdsId
                      )";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);
                MakeParam(cmd, "terminalEnumValue", (int)StationSelection.TerminalConnectionEnum.Terminal, SqlDbType.Int);

                void PopulateTerminalEntity(IDataReader dr, DataEntity terminalEntity)
                {
                    string terminalID = (string)dr["TERMINALID"];
                    string storeID = (string)dr["STOREID"];

                    terminalEntity.ID = new RecordIdentifier($"{terminalID}-{storeID}", terminalID, storeID);
                    terminalEntity.Text = (string)dr["TERMINALNAME"];
                }
                   
                return Execute<DataEntity>(entry, cmd, CommandType.Text, PopulateTerminalEntity);
            }
        }

        public virtual List<DataEntity> TerminalGroupsNotConnectedToKds(IConnectionManager entry, RecordIdentifier kdsId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"Select t.ID as TERMINALGROUPID, ISNULL(t.DESCRIPTION,'') as TERMINALGROUPNAME
                From RBOTERMINALGROUP t
                Where t.DATAAREAID = @dataAreaId
                and t.ID not in 
                (Select k.CONNECTIONVALUE 
                 From KITCHENDISPLAYTERMINALCONNECTION k 
                 Where k.DATAAREAID = @dataAreaId and k.TYPE = @terminalGroupEnumValue and UPPER(k.STATIONID) = @kdsId
                )";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", (string)kdsId);
                MakeParam(cmd, "terminalGroupEnumValue", (int)StationSelection.TerminalConnectionEnum.TerminalGroup,
                          SqlDbType.Int);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "TERMINALGROUPNAME", "TERMINALGROUPID");
            }
        }

        public virtual void MakeSureAllTypeConnectionExists(IConnectionManager entry, string kdsId)
        {
            var allRoutingConnectionsForKds = GetForKds(entry, kdsId, null);
            var allTypeConnectionExists = allRoutingConnectionsForKds.Any(rule => rule.Type == StationSelection.TerminalConnectionEnum.All);

            if (!allTypeConnectionExists)
            {
                CreateAllTypeConnection(entry, kdsId);
            }
        }

        private void CreateAllTypeConnection(IConnectionManager entry, string kdsId)
        {
            var newAllTypeConnection = new LSOneKitchenDisplayTerminalRoutingConnection
            {
                Id = Guid.NewGuid(),
                Type = StationSelection.TerminalConnectionEnum.All,
                ConnectionId = "",
                StationId = kdsId
            };
            Save(entry, newAllTypeConnection);
        }

        public virtual void MakeSureAllTypeConnectionDoesntExists(IConnectionManager entry, RecordIdentifier kdsId)
        {
            var allRoutingConnectionsForKds = GetForKds(entry, kdsId, null);
            var allTypeConnections = allRoutingConnectionsForKds.Where(rule => rule.Type == StationSelection.TerminalConnectionEnum.All);

            foreach (var connection in allTypeConnections)
            {
                Delete(entry, connection.Id);
            }
        }

        public virtual void Save(IConnectionManager entry, LSOneKitchenDisplayTerminalRoutingConnection terminalRoutingConnection)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYTERMINALCONNECTION");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            var isNew = false;
            if (terminalRoutingConnection.Id == Guid.Empty)
            {
                terminalRoutingConnection.Id = Guid.NewGuid();
                isNew = true;
            }

            if (isNew || !Exists(entry, terminalRoutingConnection.Id))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)terminalRoutingConnection.Id, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)terminalRoutingConnection.Id, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("STATIONID", (string)terminalRoutingConnection.StationId.ToUpper());
            statement.AddField("TYPE", terminalRoutingConnection.Type, SqlDbType.Int);
            statement.AddField("CONNECTIONVALUE", (string)terminalRoutingConnection.ConnectionId);
            statement.AddField("INCLUDEEXCLUDE", (byte)terminalRoutingConnection.IncludeExclude, SqlDbType.TinyInt);
            statement.AddField("TERMINALID", (string)terminalRoutingConnection.TerminalID);
            statement.AddField("STOREID", (string)terminalRoutingConnection.StoreID);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
