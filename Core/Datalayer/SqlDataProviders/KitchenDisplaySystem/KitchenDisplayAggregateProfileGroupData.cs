using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;
using System.Data;


namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayAggregateProfileGroupData : SqlServerDataProviderBase, IKitchenDisplayAggregateProfileGroupData
    {
        private string BaseSelectString
        {
            get
            {
                return @" SELECT DISTINCT '' AS PROFILEID,
                                          GROUPID,
                                          GROUPDESCRIPTION,
                                          USETIMEHORIZON
					      FROM KITCHENDISPLAYAGGREGATEPROFILEGROUP ";
            }
        }

        private string BaseSelectStringRelation
        {
            get
            {
                return @" SELECT PROFILEID,
                                 GROUPID,
                                 GROUPDESCRIPTION,
                                 USETIMEHORIZON
					      FROM KITCHENDISPLAYAGGREGATEPROFILEGROUP ";
            }
        }

        public virtual AggregateProfileGroup Get(IConnectionManager entry, RecordIdentifier groupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = 
                    BaseSelectString +
                    "WHERE GROUPID = @groupId";

                MakeParam(cmd, "groupId", groupID.PrimaryID);

                var result = Execute<AggregateProfileGroup>(entry, cmd, CommandType.Text, PopulateAggregateGroup);
                return (result.Count == 1) ? result[0] : null;
            }
        }

        public virtual List<AggregateProfileGroup> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSelectString;

                return Execute<AggregateProfileGroup>(entry, cmd, CommandType.Text, PopulateAggregateGroup);
            }
        }

        public virtual List<AggregateProfileGroup> GetForKDS(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = 
                    BaseSelectStringRelation + 
                    "WHERE PROFILEID <> ''";

                return Execute<AggregateProfileGroup>(entry, cmd, CommandType.Text, PopulateAggregateGroup);
            }
        }

        public virtual List<AggregateProfileGroup> GetList(IConnectionManager entry, RecordIdentifier aggregateProfileId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseSelectStringRelation +
                    "WHERE PROFILEID = @profileId";

                MakeParam(cmd, "profileId", aggregateProfileId);

                return Execute<AggregateProfileGroup>(entry, cmd, CommandType.Text, PopulateAggregateGroup);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier groupID)
        {
            DeleteRecord(entry, "KITCHENDISPLAYAGGREGATEPROFILEGROUP", "GROUPID", groupID, Permission.ManageKitchenDisplayProfiles, false);

            Providers.KitchenDisplayAggregateGroupItemData.DeleteByGroup(entry, groupID);
        }

        public virtual void DeleteByAggregateProfile(IConnectionManager entry, RecordIdentifier aggregateProfileID)
        {
            DeleteRecord(entry, "KITCHENDISPLAYAGGREGATEPROFILEGROUP", "PROFILEID", aggregateProfileID, Permission.ManageKitchenDisplayProfiles, false);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier groupID)
        {
            return RecordExists(entry, "KITCHENDISPLAYAGGREGATEPROFILEGROUP", "GROUPID", groupID, false);
        }

        public virtual bool RelationExists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYAGGREGATEPROFILEGROUP", new[] { "GROUPID", "PROFILEID" }, id, false);
        }

        public virtual void Save(IConnectionManager entry, AggregateProfileGroup aggregateProfileGroup)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYAGGREGATEPROFILEGROUP");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayProfiles);

            if (!Exists(entry, aggregateProfileGroup.GroupID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("GROUPID", ((string)aggregateProfileGroup.GroupID).ToUpper());
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("GROUPID", (string)aggregateProfileGroup.GroupID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("GROUPDESCRIPTION", aggregateProfileGroup.GroupDescription);
            statement.AddField("USETIMEHORIZON", aggregateProfileGroup.UseTimeHorizon, SqlDbType.TinyInt);

            Save(entry, aggregateProfileGroup, statement);
        }

        public virtual void CreateGroupRelation(IConnectionManager entry, AggregateProfileGroup aggregateProfileGroup)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYAGGREGATEPROFILEGROUP");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayProfiles);

            statement.StatementType = StatementType.Insert;
            statement.AddKey("GROUPID", ((string)aggregateProfileGroup.GroupID).ToUpper());
            statement.AddKey("PROFILEID", (string)aggregateProfileGroup.ProfileID);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddField("GROUPDESCRIPTION", aggregateProfileGroup.GroupDescription);
            statement.AddField("USETIMEHORIZON", aggregateProfileGroup.UseTimeHorizon, SqlDbType.TinyInt);

            Save(entry, aggregateProfileGroup, statement);
        }

        public virtual void RemoveGroupFromProfile(IConnectionManager entry, RecordIdentifier ID)
        {
            DeleteRecord(entry, "KITCHENDISPLAYAGGREGATEPROFILEGROUP", new[] { "GROUPID", "PROFILEID" }, ID, Permission.ManageKitchenDisplayProfiles, false);
        }

        private void PopulateAggregateGroup(IDataReader dr, AggregateProfileGroup aggregateProfileGroup)
        {
            aggregateProfileGroup.GroupID = (string)dr["GROUPID"];
            aggregateProfileGroup.GroupID.SerializationData = (string)dr["GROUPID"];
            aggregateProfileGroup.ProfileID = (string)dr["PROFILEID"];
            aggregateProfileGroup.ProfileID.SerializationData = (string)dr["PROFILEID"];
            aggregateProfileGroup.GroupDescription = aggregateProfileGroup.Text = (string)dr["GROUPDESCRIPTION"];
            aggregateProfileGroup.UseTimeHorizon = AsBool(dr["USETIMEHORIZON"]);
        }
    }
}