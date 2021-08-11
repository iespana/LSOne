using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplaySectionStationRoutingData : SqlServerDataProviderBase, IKitchenDisplaySectionStationRoutingData
    {
        private string BaseSelectString
        {
            get
            {
                return @"   SELECT routing.ID,
                                   routing.RESTAURANTID,
                                   routing.SECTIONID,
                                   routing.HOSPITALITYTYPESEQUENCE,
                                   routing.HOSPITALITYTYPESALESTYPE,
                                   routing.STATIONTYPE,
                                   routing.STATIONID,
                                   section.CODE as SECTIONCODE,
                                   section.DESCRIPTION as SECTIONDESCRIPTION,
                                   ISNULL(store.NAME, '') as RESTAURANTNAME,
                                   ISNULL(hosType.DESCRIPTION, '') as HOSPITALITYTYPEDESCRIPTION,
                                   ISNULL(printingStation.STATIONNAME, '') as PRINTINGSTATIONNAME,
                                   ISNULL(displayStation.NAME, '') as DISPLAYSTATIONNAME
                            FROM KITCHENDISPLAYSECTIONSTATIONROUTING routing
                            LEFT JOIN RBOSTORETABLE store on store.STOREID = routing.RESTAURANTID 
                            LEFT JOIN KITCHENDISPLAYPRODUCTIONSECTION section on section.ID = routing.SECTIONID 
                            LEFT JOIN RESTAURANTSTATION printingStation on printingStation.ID = routing.STATIONID 
                            LEFT JOIN KITCHENDISPLAYSTATION displayStation on displayStation.ID = routing.STATIONID 
                            LEFT JOIN HOSPITALITYTYPE hosType on hosType.RESTAURANTID = routing.RESTAURANTID AND
                                                                 hosType.SEQUENCE = routing.HOSPITALITYTYPESEQUENCE AND
                                                                 hosType.SALESTYPE = routing.HOSPITALITYTYPESALESTYPE ";
            }
        }

        private void PopulateSectionStationRouting(IDataReader dr, KitchenDisplaySectionStationRouting routing)
        {
            routing.ID = (Guid)dr["ID"];

            if (!(dr["RESTAURANTID"] is DBNull))
            {
                routing.RestaurantId = (string)dr["RESTAURANTID"];
                routing.RestaurantName = (string)dr["RESTAURANTNAME"];
            }
            if (!(dr["SECTIONID"] is DBNull))
            {
                routing.SectionId = (Guid)dr["SECTIONID"];
                routing.SectionCode = (string)dr["SECTIONCODE"];
                routing.SectionDescription = (string)dr["SECTIONDESCRIPTION"];
            }
            if (!(dr["HOSPITALITYTYPESEQUENCE"] is DBNull))
            {
                routing.HospitalityTypeID = new RecordIdentifier((string)dr["RESTAURANTID"], new RecordIdentifier((int)dr["HOSPITALITYTYPESEQUENCE"], (string)dr["HOSPITALITYTYPESALESTYPE"]));
                routing.HospitalityTypeDescription = (string)dr["HOSPITALITYTYPEDESCRIPTION"];
            }
            if (!(dr["STATIONID"] is DBNull))
            {
                routing.StationType = (KitchenDisplaySectionStationRouting.StationTypeEnum)(byte)dr["STATIONTYPE"];
                routing.StationId = (string)dr["STATIONID"];
            }

            switch (routing.StationType)
            {
                case KitchenDisplaySectionStationRouting.StationTypeEnum.Display:
                    routing.StationName = (string)dr["DISPLAYSTATIONNAME"];
                    break;
                case KitchenDisplaySectionStationRouting.StationTypeEnum.Printer:
                    routing.StationName = (string)dr["PRINTINGSTATIONNAME"];
                    break;
            }
        }

        public virtual KitchenDisplaySectionStationRouting Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseSelectString +
                    "WHERE routing.ID = @routingId";

                MakeParam(cmd, "routingId", (Guid)id);

                var result = Execute<KitchenDisplaySectionStationRouting>(entry, cmd, CommandType.Text, PopulateSectionStationRouting);
                return (result.Count == 1) ? result[0] : null;
            }
        }

        public virtual List<KitchenDisplaySectionStationRouting> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSelectString;

                return Execute<KitchenDisplaySectionStationRouting>(entry, cmd, CommandType.Text, PopulateSectionStationRouting);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYSECTIONSTATIONROUTING", "ID", id, BusinessObjects.Permission.ManageKitchenDisplayStations, false);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYSECTIONSTATIONROUTING", "ID", id, false);
        }

        public virtual bool Exists(IConnectionManager entry, KitchenDisplaySectionStationRouting routing)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                columns.Add(new TableColumn { ColumnName = "ID" });

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "RESTAURANTID = @restaurantId" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "STATIONID = @stationId" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "STATIONTYPE = @stationType" });

                MakeParam(cmd, "restaurantId", routing.RestaurantId);
                MakeParam(cmd, "stationId", routing.StationId);
                MakeParam(cmd, "stationType", (byte)routing.StationType);

                if (routing.SectionId.StringValue == string.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "SECTIONID IS NULL" });
                }
                else
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "SECTIONID = @sectionId" });

                    MakeParam(cmd, "sectionId", (Guid)routing.SectionId);
                }

                if (routing.HospitalityTypeID.StringValue == string.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "HOSPITALITYTYPESEQUENCE IS NULL" });
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "HOSPITALITYTYPESALESTYPE IS NULL" });
                }
                else
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "HOSPITALITYTYPESEQUENCE = @hosSequence" });
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "HOSPITALITYTYPESALESTYPE = @hosSalesType" });

                    MakeParam(cmd, "hosSequence", (int)routing.HospitalityTypeID.SecondaryID);
                    MakeParam(cmd, "hosSalesType", routing.HospitalityTypeID.SecondaryID.SecondaryID);
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("KITCHENDISPLAYSECTIONSTATIONROUTING", ""),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                List<RecordIdentifier> result = Execute(entry, cmd, CommandType.Text, "ID");
                return result.Count == 1;
            }
        }

        public virtual void Save(IConnectionManager entry, KitchenDisplaySectionStationRouting routing)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYSECTIONSTATIONROUTING");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            if (!Exists(entry, routing.ID))
            {
                statement.StatementType = StatementType.Insert;
                if (RecordIdentifier.IsEmptyOrNull(routing.ID))
                {
                    routing.ID = Guid.NewGuid();
                }
                statement.AddKey("ID", (Guid)routing.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (Guid)routing.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("RESTAURANTID", (string)routing.RestaurantId);
            statement.AddField("STATIONTYPE", (byte)routing.StationType, SqlDbType.TinyInt);
            statement.AddField("STATIONID",(string)routing.StationId);

            if(routing.SectionId.StringValue != string.Empty)
            {
                statement.AddField("SECTIONID", (Guid)routing.SectionId, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.AddField("SECTIONID", DBNull.Value, SqlDbType.UniqueIdentifier);
            }

            if(routing.HospitalityTypeID.StringValue != string.Empty)
            {
                statement.AddField("HOSPITALITYTYPESEQUENCE", (int)routing.HospitalityTypeID.SecondaryID, SqlDbType.Int);
                statement.AddField("HOSPITALITYTYPESALESTYPE", (string)routing.HospitalityTypeID.SecondaryID.SecondaryID);
            }
            else
            {
                statement.AddField("HOSPITALITYTYPESEQUENCE", DBNull.Value, SqlDbType.Int);
                statement.AddField("HOSPITALITYTYPESALESTYPE", DBNull.Value, SqlDbType.NVarChar);
            }


            Save(entry, routing, statement);
        }

        public virtual void RemoveSection(IConnectionManager entry, RecordIdentifier sectionId)
        {
            // Set section ids to null in section station routings
            SqlServerStatement statement = new SqlServerStatement("KITCHENDISPLAYSECTIONSTATIONROUTING");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            statement.StatementType = StatementType.Update;
            statement.AddCondition("SECTIONID", (Guid)sectionId, SqlDbType.UniqueIdentifier);
            statement.AddField("SECTIONID", DBNull.Value, SqlDbType.UniqueIdentifier);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void RemoveRestaurant(IConnectionManager entry, RecordIdentifier restaurantId)
        {
            // Set restaurant ids to null in section station routings
            SqlServerStatement statement = new SqlServerStatement("KITCHENDISPLAYSECTIONSTATIONROUTING");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            statement.StatementType = StatementType.Update;
            statement.AddCondition("RESTAURANTID", (string)restaurantId);
            statement.AddField("RESTAURANTID", DBNull.Value, SqlDbType.NVarChar);
            statement.AddField("HOSPITALITYTYPESEQUENCE", DBNull.Value, SqlDbType.Int);
            statement.AddField("HOSPITALITYTYPESALESTYPE", DBNull.Value, SqlDbType.NVarChar);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void RemoveHospitalityType(IConnectionManager entry, RecordIdentifier hosTypeId)
        {
            // Set section ids to null in item section routings
            SqlServerStatement statement = new SqlServerStatement("KITCHENDISPLAYSECTIONSTATIONROUTING");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            statement.StatementType = StatementType.Update;
            statement.AddCondition("RESTAURANTID", (string)hosTypeId);
            statement.AddCondition("HOSPITALITYTYPESEQUENCE", (int)hosTypeId.SecondaryID, SqlDbType.Int);
            statement.AddCondition("HOSPITALITYTYPESALESTYPE", (string)hosTypeId.SecondaryID.SecondaryID);
            statement.AddField("HOSPITALITYTYPESEQUENCE", DBNull.Value, SqlDbType.Int);
            statement.AddField("HOSPITALITYTYPESALESTYPE", DBNull.Value, SqlDbType.NVarChar);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void RemoveStation(IConnectionManager entry, RecordIdentifier stationId)
        {
            // Set section ids to null in item section routings
            SqlServerStatement statement = new SqlServerStatement("KITCHENDISPLAYSECTIONSTATIONROUTING");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            statement.StatementType = StatementType.Update;
            statement.AddCondition("STATIONID", (string)stationId);
            statement.AddField("STATIONID", DBNull.Value, SqlDbType.NVarChar);
            statement.AddField("STATIONTYPE", DBNull.Value, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}