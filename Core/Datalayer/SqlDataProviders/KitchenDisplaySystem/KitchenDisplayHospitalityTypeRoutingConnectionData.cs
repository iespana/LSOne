using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
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

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayHospitalityTypeRoutingConnectionData : SqlServerDataProviderBase, IKitchenDisplayHospitalityTypeRoutingConnectionData
    {
        private static string BaseSelectString
        {
            get
            {
                return @"   select 
                            kdhc.ID
                            ,UPPER(kdhc.STATIONID) as STATIONID
                            ,kdhc.TYPE
                            ,kdhc.CONNECTIONVALUE
                            ,kdhc.RESTAURANT
                            ,kdhc.SALESTYPE
                            ,kdhc.INCLUDEEXCLUDE
                            ,ISNULL(ht.DESCRIPTION, '') as CONNECTIONDESCRIPTION
                            from KITCHENDISPLAYHOSPITALITYTYPECONNECTION kdhc left outer join " +
                            "HOSPITALITYTYPE ht on kdhc.RESTAURANT = ht.RESTAURANTID AND kdhc.SALESTYPE = ht.SALESTYPE ";
            }
        }

        private static void PopulateRoutingConnection(IDataReader dr, LSOneKitchenDisplayHospitalityTypeRoutingConnection hospitalityTypeRouting)
        {
            if (!(dr["ID"] is DBNull))
            {
                hospitalityTypeRouting.Id = (Guid)dr["ID"];
            }
            hospitalityTypeRouting.StationId = (string)dr["STATIONID"];
            hospitalityTypeRouting.Type = (StationSelection.HospitalityTypeConnectionEnum)dr["TYPE"];
            hospitalityTypeRouting.ConnectionId = (string)dr["CONNECTIONVALUE"];
            hospitalityTypeRouting.Restaurant = (string)dr["RESTAURANT"];
            hospitalityTypeRouting.SalesType = (string)dr["SALESTYPE"];
            hospitalityTypeRouting.ConnectionDescription = (string)dr["CONNECTIONDESCRIPTION"];
            hospitalityTypeRouting.IncludeExclude = (LSOneKitchenDisplayHospitalityTypeRoutingConnection.IncludeEnum)(byte)dr["INCLUDEEXCLUDE"];
        }

        public virtual LSOneKitchenDisplayHospitalityTypeRoutingConnection Get(IConnectionManager entry, RecordIdentifier hospitalityTypeRoutingId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where kdhc.ID = @Id and kdhc.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "Id", (Guid)hospitalityTypeRoutingId, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var result = Execute<LSOneKitchenDisplayHospitalityTypeRoutingConnection>(entry, cmd, CommandType.Text, PopulateRoutingConnection);

                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual List<LSOneKitchenDisplayHospitalityTypeRoutingConnection> GetForKds(IConnectionManager entry, string kdsId, List<string> connectedStations)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where UPPER(kdhc.STATIONID) = @kdsId and kdhc.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdsId", kdsId);

                return Execute<LSOneKitchenDisplayHospitalityTypeRoutingConnection>(entry, cmd, CommandType.Text,
                    PopulateRoutingConnection);
            }
        }

        public virtual List<LSOneKitchenDisplayHospitalityTypeRoutingConnection> GetForKds(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"WITH CONNECTIONS AS 
                                    (
	                                    (SELECT UPPER(kdhc.STATIONID) AS STATIONID,
	                                            TYPE = 1,
	                                            CONCAT(ht.RESTAURANTID, '-', ht.SALESTYPE) as CONNECTIONVALUE,
                                                kdhc.RESTAURANT,
                                                kdhc.SALESTYPE,
	                                            kdhc.INCLUDEEXCLUDE,
	                                            ISNULL(ht.DESCRIPTION, '') AS CONNECTIONDESCRIPTION
                                         FROM KITCHENDISPLAYHOSPITALITYTYPECONNECTION kdhc 
	                                     JOIN HOSPITALITYTYPE ht on kdhc.TYPE = 0)

	                                    UNION

	                                    (SELECT UPPER(kdhc.STATIONID) AS STATIONID,
	                                            TYPE = 1,
	                                            kdhc.CONNECTIONVALUE,
                                                kdhc.RESTAURANT,
                                                kdhc.SALESTYPE,
	                                            kdhc.INCLUDEEXCLUDE,
	                                            ISNULL(ht.DESCRIPTION, '') AS CONNECTIONDESCRIPTION
                                         FROM KITCHENDISPLAYHOSPITALITYTYPECONNECTION kdhc 
	                                     LEFT JOIN HOSPITALITYTYPE ht on kdhc.RESTAURANT = ht.RESTAURANTID AND kdhc.SALESTYPE = ht.SALESTYPE
	                                     WHERE TYPE = 1)
                                    )

                                    SELECT *, ID = NULL
                                    FROM CONNECTIONS c1
                                    WHERE NOT EXISTS (SELECT * 
                                                      FROM CONNECTIONS c2 
				                                      WHERE c1.STATIONID = c2.STATIONID AND c1.CONNECTIONVALUE = c2.CONNECTIONVALUE AND INCLUDEEXCLUDE = 1)";

                return Execute<LSOneKitchenDisplayHospitalityTypeRoutingConnection>(entry, cmd, CommandType.Text,
                    PopulateRoutingConnection);
            }
        }

        public virtual List<DataEntity> HospitalitTypesNotConnectedToKds(IConnectionManager entry, string kdsId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT CONCAT(h.RESTAURANTID, '-', h.SALESTYPE) as ID,
                             h.RESTAURANTID,
                             h.SALESTYPE,
                             ISNULL(h.DESCRIPTION,'') as HOSPITALITYTYPEDESCRIPTION
                      FROM HOSPITALITYTYPE h
                      WHERE NOT EXISTS
                      (SELECT 1
                      FROM KITCHENDISPLAYHOSPITALITYTYPECONNECTION k 
                      WHERE k.RESTAURANT = h.RESTAURANTID and k.SALESTYPE = h.SALESTYPE AND k.TYPE = @hospitalityTypeEnumValue AND UPPER(k.STATIONID) = @kdsId )";

                MakeParam(cmd, "kdsId", kdsId);
                MakeParam(cmd, "hospitalityTypeEnumValue",
                          (int)StationSelection.HospitalityTypeConnectionEnum.HospitalityType, SqlDbType.Int);

                void PopulateHospitalityTypeEntity(IDataReader dr, DataEntity hospitalityTypeEntity)
                {
                    string restaurantID = (string)dr["RESTAURANTID"];
                    string salesType = (string)dr["SALESTYPE"];
                    string ID = (string)dr["ID"];

                    hospitalityTypeEntity.ID = new RecordIdentifier(ID, restaurantID, salesType);
                    hospitalityTypeEntity.Text = (string)dr["HOSPITALITYTYPEDESCRIPTION"];
                }

                return Execute<DataEntity>(entry, cmd, CommandType.Text, PopulateHospitalityTypeEntity);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYHOSPITALITYTYPECONNECTION", "ID", id, Permission.ManageKitchenDisplayStations);
        }

        public virtual void DeleteByStation(IConnectionManager entry, RecordIdentifier stationId)
        {
            DeleteRecord(entry, "KITCHENDISPLAYHOSPITALITYTYPECONNECTION", "STATIONID", stationId, Permission.ManageKitchenDisplayStations);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYHOSPITALITYTYPECONNECTION", "ID", id);
        }

        public virtual void MakeSureAllTypeConnectionExists(IConnectionManager entry, string kdsId)
        {
            var allRoutingConnectionsForKds = GetForKds(entry, kdsId, null);
            var allTypeConnectionExists = allRoutingConnectionsForKds.Any(rule => rule.Type == StationSelection.HospitalityTypeConnectionEnum.All);

            if (!allTypeConnectionExists)
            {
                CreateAllTypeConnection(entry, kdsId);
            }
        }

        private void CreateAllTypeConnection(IConnectionManager entry, string kdsId)
        {
            var newAllTypeConnection = new LSOneKitchenDisplayHospitalityTypeRoutingConnection
            {
                Id = Guid.NewGuid(),
                Type = StationSelection.HospitalityTypeConnectionEnum.All,
                ConnectionId = "",
                Restaurant = "",
                SalesType = "",
                StationId = kdsId
            };
            Save(entry, newAllTypeConnection);
        }

        public virtual void MakeSureAllTypeConnectionDoesntExists(IConnectionManager entry, string kdsId)
        {
            var allRoutingConnectionsForKds = GetForKds(entry, kdsId, null);
            var allTypeConnections = allRoutingConnectionsForKds.Where(rule => rule.Type == StationSelection.HospitalityTypeConnectionEnum.All);

            foreach (var connection in allTypeConnections)
            {
                Delete(entry, connection.Id);
            }
        }

        public virtual void Save(IConnectionManager entry, LSOneKitchenDisplayHospitalityTypeRoutingConnection hospitalityTypeRoutingConnection)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYHOSPITALITYTYPECONNECTION");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            var isNew = false;
            if (hospitalityTypeRoutingConnection.Id == Guid.Empty)
            {
                hospitalityTypeRoutingConnection.Id = Guid.NewGuid();
                isNew = true;
            }

            if (isNew || !Exists(entry, hospitalityTypeRoutingConnection.Id))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)hospitalityTypeRoutingConnection.Id, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)hospitalityTypeRoutingConnection.Id, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("STATIONID", (string)hospitalityTypeRoutingConnection.StationId.ToUpper());
            statement.AddField("TYPE", hospitalityTypeRoutingConnection.Type, SqlDbType.Int);
            statement.AddField("INCLUDEEXCLUDE", (byte)hospitalityTypeRoutingConnection.IncludeExclude, SqlDbType.TinyInt);
            
            statement.AddField("CONNECTIONVALUE", hospitalityTypeRoutingConnection.ConnectionId);

            if(hospitalityTypeRoutingConnection.Type == StationSelection.HospitalityTypeConnectionEnum.All)
            {
                statement.AddField("RESTAURANT", "");
                statement.AddField("SALESTYPE", "");
            }
            else
            {
                statement.AddField("RESTAURANT", (string)hospitalityTypeRoutingConnection.Restaurant);
                statement.AddField("SALESTYPE", (string)hospitalityTypeRoutingConnection.SalesType);
            }

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
