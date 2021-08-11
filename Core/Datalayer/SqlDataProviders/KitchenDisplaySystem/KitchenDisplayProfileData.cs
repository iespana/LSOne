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

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    /// <summary>
    /// Data provider class for KitchenDisplayProvider
    /// </summary>
    public class KitchenDisplayProfileData : SqlServerDataProviderBase, IKitchenDisplayProfileData
    {
        private static string BaseSelectString
        {
            get { return @"select
                        ID,
                        DISPLAYMODE,
                        NAME
                        from KITCHENDISPLAYPROFILE "; }
        }

        private static void PopulateKitchenDisplayProfile(IDataReader dr, KitchenDisplayProfile displayProfile)
        {
            displayProfile.ID = (Guid)dr["ID"];
            displayProfile.DisplayMode = (KitchenDisplayStation.DisplayModeEnum)AsInt(dr["DISPLAYMODE"]);
            displayProfile.Text = (string)dr["NAME"];
        }

        /// <summary>
        /// Gets a list of all DisplayProfiles
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of PosMenuHeader objects containing all pos menu header records</returns>
        public virtual List<KitchenDisplayProfile> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<KitchenDisplayProfile>(entry, cmd, CommandType.Text, PopulateKitchenDisplayProfile);
            }
        }

        /// <summary>
        /// Gets as pos menu header with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeaderID">The ID of the pos menu header to get</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>A PosMenuHeader object containing the pos menu header with the given ID</returns>
        public virtual KitchenDisplayProfile Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where ID = @id and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "id", (Guid)id.PrimaryID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var results = Execute<KitchenDisplayProfile>(entry, cmd, CommandType.Text, PopulateKitchenDisplayProfile);

                return (results.Count == 1) ? results[0] : null;
            }
        }

        /// <summary>
        /// Checks if a kitchenDisplayProfile with the given ID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the line to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYPROFILE", "ID", id);
        }

        /// <summary>
        /// Deletes the kitchenDisplayProfile with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the pos menu header to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<KitchenDisplayProfile>(entry, "KITCHENDISPLAYPROFILE", "ID", id, BusinessObjects.Permission.ManageKitchenDisplayProfiles);

            // TO DO!  Delete all lines with this profile ID
        }

        /// <summary>
        /// Saves a kitchenDisplayProfile object into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeader"></param>
        public virtual void Save(IConnectionManager entry, KitchenDisplayProfile displayProfile)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYPROFILE");
            ValidateSecurity(entry, BusinessObjects.Permission.ManageKitchenDisplayProfiles);

            bool isNew = false;
            if (displayProfile.ID.IsEmpty)
            {
                displayProfile.ID = Guid.NewGuid();
                isNew = true;
            }

            if (isNew || !Exists(entry, displayProfile.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", ((string)displayProfile.ID).ToUpper());
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (string)displayProfile.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("DISPLAYMODE", displayProfile.DisplayMode, SqlDbType.TinyInt);
            statement.AddField("NAME", displayProfile.Text);

            entry.Connection.ExecuteStatement(statement);

            if (isNew)
            {
                //Create a line for line display
                KitchenDisplayLine line = new KitchenDisplayLine();
                line.DisplayProfileID = displayProfile.ID;
                line.Type = KitchenDisplayStation.DisplayRowTypeEnum.LineDisplayRow;
                Providers.KitchenDisplayLineData.Save(entry, line);
            }
        }
    }
}
