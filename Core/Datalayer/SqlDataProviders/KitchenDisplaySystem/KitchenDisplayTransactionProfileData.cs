using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Enums;
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
    public class KitchenDisplayTransactionProfileData : SqlServerDataProviderBase, IKitchenDisplayTransactionProfileData
    {
        private static string BaseSelectString
        {
            get
            {
                return @"Select ID,
                        NAME, 
                        KITCHENMANAGERSERVER,       
                        KITCHENMANAGERPORT
                        FROM KITCHENDISPLAYTRANSACTIONPROFILE ";
            }
        }

        private static void PopulateKitchenServiceProfile(IDataReader dr, KitchenServiceProfile transactionProfile)
        {
            transactionProfile.ID = (Guid)dr["ID"];
            transactionProfile.Text = (string)dr["NAME"];
            transactionProfile.KitchenServiceAddress = (string)dr["KITCHENMANAGERSERVER"];
            transactionProfile.KitchenServicePort = (int)dr["KITCHENMANAGERPORT"];
        }

        /// <summary>
        /// Gets a list of all profiles
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all profiles</returns>
        /// 
        public virtual List<KitchenServiceProfile> GetList(IConnectionManager entry)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.TransactionServiceProfileEdit);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<KitchenServiceProfile>(entry, cmd, CommandType.Text, PopulateKitchenServiceProfile);

            }
        }

        /// <summary>
        /// Gets an kitchen manager tranaction profile from an ID
        /// </summary>
        /// <param name="entry">entry into the database</param>
        /// <param name="id">Id of the Kitchen manager profile</param>
        /// <param name="cache">Cache type used</param>
        /// <returns>Object of the Kitchen manager tranaction profile with the ID given</returns>
        /// 

        public virtual KitchenServiceProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSelectString +
                    " where ID = @ID and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "ID", (Guid)id, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Get<KitchenServiceProfile>(entry, cmd, id, PopulateKitchenServiceProfile, cache, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Deletes an record from the database
        /// </summary>
        /// <param name="entry">entry into the database</param>
        /// <param name="id">Id of the record</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<KitchenServiceProfile>(entry, "KITCHENDISPLAYTRANSACTIONPROFILE", "ID", id, BusinessObjects.Permission.ManageKitchenServiceProfiles);
        }

        /// <summary>
        /// Checks if an record does exits in the database
        /// </summary>
        /// <param name="entry">entry into the database</param>
        /// <param name="id">Id of the record</param>
        /// <returns></returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<KitchenServiceProfile>(entry, "KITCHENDISPLAYTRANSACTIONPROFILE", "ID", id);
        }

        /// <summary>
        /// Saves the object to the database
        /// </summary>
        /// <param name="entry">entry into the database</param>
        /// <param name="transactionProfile">The profile object</param>
        public virtual void Save(IConnectionManager entry, KitchenServiceProfile transactionProfile)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYTRANSACTIONPROFILE");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageKitchenServiceProfiles);

            bool isNew = false;
            if (transactionProfile.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                transactionProfile.ID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, transactionProfile.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)transactionProfile.ID, SqlDbType.UniqueIdentifier);

            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)transactionProfile.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("NAME", transactionProfile.Text);
            statement.AddField("KITCHENMANAGERSERVER", transactionProfile.KitchenServiceAddress);
            statement.AddField("KITCHENMANAGERPORT", transactionProfile.KitchenServicePort, SqlDbType.Int);

            Save(entry, transactionProfile, statement);
        }
    }
}
