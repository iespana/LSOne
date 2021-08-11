using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Profiles
{
    public class StyleProfileData : SqlServerDataProviderBase, IStyleProfileData
    {
        /// <summary>
        /// Gets a list of all style profiles
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all style profiles</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "POSSTYLEPROFILE", "NAME", "ID", "NAME");
        }

        /// <summary>
        /// Gets a list of all style profiles sorted
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="sort">How to sort list</param>
        /// <returns>A list of all style profiles sorted</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, string sort)
        {
            return GetList<DataEntity>(entry, "POSSTYLEPROFILE", "NAME", "ID", sort);
        }

        private static void PopulateProfile(IDataReader dr, StyleProfile profile)
        {
            profile.ID = (string)dr["ID"];
            profile.Text = (string)dr["NAME"];
        }

        /// <summary>
        /// Gets the styleprofile with id sent as parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="id">id of styleprofile to get</param>
        /// /// <param name="cache">Cachetype</param>
        /// <returns>A Styleprofile </returns>
        public virtual StyleProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                       "select ID, ISNULL(NAME,'') as NAME "+ 
                       "from POSSTYLEPROFILE " +
                       "where DATAAREAID = @dataAreaId and ID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                return Get<StyleProfile>(entry, cmd, id, PopulateProfile, cache, UsageIntentEnum.Normal);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<StyleProfile>(entry, "POSSTYLEPROFILE", "ID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<StyleProfile>(entry, "POSSTYLEPROFILE", "ID", id, BusinessObjects.Permission.StyleProfileEdit);
        }

        /// <summary>
        /// Gets the styleprofile with id sent as parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="profile">profile which list gets saved under</param>
        /// /// <param name="styleLineList">List of PosStyleProfileLines to save</param>
        public virtual void Save(IConnectionManager entry, StyleProfile profile, List<PosStyleProfileLine> styleLineList = null)
        {
            var statement = new SqlServerStatement("POSSTYLEPROFILE");

            ValidateSecurity(entry, BusinessObjects.Permission.StyleProfileEdit);
            profile.Validate();

            bool isNew = false;
            if (profile.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                profile.ID = DataProviderFactory.Instance.GenerateNumber<ISiteServiceProfileData, SiteServiceProfile>(entry);
            }

            if (isNew || !Exists(entry, profile.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)profile.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)profile.ID);
            }

            statement.AddField("NAME", profile.Text);

            Save(entry, profile, statement);
            if (styleLineList != null)
            {
                foreach(var styleLine in styleLineList)
                {
                    var style = styleLine;
                    style.PosStyleProfileLineId = RecordIdentifier.Empty;
                    style.ProfileID = profile.ID;
                    Providers.PosStyleProfileLineData.Save(entry, style);
                }
            }
        }

        #region ISequenceable Members
        public RecordIdentifier SequenceID
        {
            get { return "STYLEPROFILE"; }
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSSTYLEPROFILE", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }
        #endregion
    }
}
