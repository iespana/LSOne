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
    public class KitchenDisplayAggregateProfileData : SqlServerDataProviderBase, IKitchenDisplayAggregateProfileData
    {
        private string BaseSelectString
        {
            get
            {
                return @" SELECT PROFILEID,
                                 DESCRIPTION,
                                 TIMEHORIZON
					      FROM KITCHENDISPLAYAGGREGATEPROFILE ";
            }
        }

        public virtual AggregateProfile Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = 
                    BaseSelectString +
                    "WHERE PROFILEID = @id";

                MakeParam(cmd, "id", id);

                var result = Execute<AggregateProfile>(entry, cmd, CommandType.Text, PopulateAggregateGroup);
                return (result.Count == 1) ? result[0] : null;
            }
        }

        public virtual List<AggregateProfile> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSelectString;

                return Execute<AggregateProfile>(entry, cmd, CommandType.Text, PopulateAggregateGroup);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYAGGREGATEPROFILE", "PROFILEID", id, Permission.ManageKitchenDisplayProfiles, false);

            Providers.KitchenDisplayAggregateProfileGroupData.DeleteByAggregateProfile(entry, id);

            List<AggregateProfileGroup> groups = Providers.KitchenDisplayAggregateProfileGroupData.GetList(entry, id);
            foreach(AggregateProfileGroup group in groups)
            {
                Providers.KitchenDisplayAggregateGroupItemData.DeleteByGroup(entry, group.GroupID);
            }

        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYAGGREGATEPROFILE", "PROFILEID", ((string)id).ToUpper(), false);
        }

        public virtual void Save(IConnectionManager entry, AggregateProfile aggregateProfile)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYAGGREGATEPROFILE");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayProfiles);

            if (!Exists(entry, aggregateProfile.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("PROFILEID", ((string)aggregateProfile.ID).ToUpper());
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("PROFILEID", (string)aggregateProfile.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("DESCRIPTION", aggregateProfile.Description);
            statement.AddField("TIMEHORIZON", aggregateProfile.TimeHorizon, SqlDbType.Int);

            Save(entry, aggregateProfile, statement);
        }

        private void PopulateAggregateGroup(IDataReader dr, AggregateProfile aggregateProfile)
        {
            aggregateProfile.ID = aggregateProfile.ProfileID = (string)dr["PROFILEID"];
            aggregateProfile.ProfileID.SerializationData = (string)dr["PROFILEID"];
            aggregateProfile.Description = aggregateProfile.Text = (string)dr["DESCRIPTION"];
            aggregateProfile.TimeHorizon = (int)dr["TIMEHORIZON"];
        }
    }
}