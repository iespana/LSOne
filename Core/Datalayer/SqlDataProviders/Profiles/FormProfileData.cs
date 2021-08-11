using System;
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
    public class FormProfileData : SqlServerDataProviderBase, IFormProfileData
    {
        protected string BaseSql = @"SELECT 
                                     PROFILEID, 
                                     ISNULL(DESCRIPTION,'') as DESCRIPTION,
	                                 CAST(CASE WHEN EXISTS (SELECT 1 FROM RBOSTORETABLE s WHERE s.RECEIPTEMAILPROFILEID = F.PROFILEID OR s.RECEIPTPROFILEID = F.PROFILEID)
	                                  	THEN 1
	                                  	ELSE 0
	                                 END AS BIT) AS PROFILEISUSED
                                     FROM POSFORMPROFILE F ";

        private static string ResolveSort(FormProfileSorting sort, bool backwards)
        {
            switch (sort)
            {
                case FormProfileSorting.Description:
                    return string.Format("DESCRIPTION {0}", (backwards ? "DESC" : "ASC"));
            }
            return "";
        }

        /// <summary>
        /// Gets a list of all form profiles
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all form profiles</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "POSFORMPROFILE", "DESCRIPTION", "PROFILEID", "DESCRIPTION");
        }

        /// <summary>
        /// Gets a list of all form profiles sorted
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="sort">How to sort list</param>
        /// <param name="sortBackwards"></param>
        /// <returns>A list of all form profiles sorted</returns>
        public virtual List<FormProfile> GetList(IConnectionManager entry, FormProfileSorting sort, bool sortBackwards)
        {
            string sortString = ResolveSort(sort, sortBackwards);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSql + " WHERE DATAAREAID = @dataAreaId ORDER BY " + sortString;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<FormProfile>(entry, cmd, CommandType.Text, PopulateProfile);
            }
        }

        private static void PopulateProfile(IDataReader dr, FormProfile profile)
        {
            profile.ID = (Guid)dr["PROFILEID"];
            profile.Text = (string)dr["DESCRIPTION"];
            profile.ProfileIsUsed = (bool)dr["PROFILEISUSED"];
        }

        /// <summary>
        /// Gets the styleprofile with id sent as parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="id">id of styleprofile to get</param>
        /// /// <param name="cache">Cachetype</param>
        /// <returns>A Styleprofile </returns>
        public virtual FormProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                if (id == Guid.Empty)
                {
                    id = FormProfile.DefaultProfileID;
                }

                cmd.CommandText = BaseSql + " WHERE DATAAREAID = @dataAreaId AND PROFILEID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (Guid)id);

                return Get<FormProfile>(entry, cmd, (Guid)id, PopulateProfile, cache, UsageIntentEnum.Normal);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<FormProfile>(entry, "POSFORMPROFILE", "PROFILEID", (Guid)id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<FormProfile>(entry, "POSFORMPROFILE", "PROFILEID", (Guid)id, BusinessObjects.Permission.FormProfileEdit);
        }

        /// <summary>
        /// Saves the formprofile
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="profile">profile which list gets saved under</param>
        /// /// <param name="profileLineList">List of FormProfileLines to save</param>
        public virtual void Save(IConnectionManager entry, FormProfile profile, List<FormProfileLine> profileLineList = null)
        {
            var statement = new SqlServerStatement("POSFORMPROFILE");

            ValidateSecurity(entry, BusinessObjects.Permission.FormProfileEdit);
            //profile.Validate();

            bool isNew = false;
            if (profile.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                profile.ID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, profile.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("PROFILEID", (Guid)profile.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("PROFILEID", (Guid)profile.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("DESCRIPTION", profile.Text);

            Save(entry, profile, statement);

            if (profileLineList != null)
            {
                foreach (FormProfileLine line in profileLineList)
                {
                    var lineToSave = line;
                    lineToSave.ID = RecordIdentifier.Empty;
                    lineToSave.ProfileID = profile.ID;
                    lineToSave.ReceiptTypeID = line.ReceiptTypeID;
                    lineToSave.Text = line.Text;
                    lineToSave.FormLayoutID = line.FormLayoutID;
                    lineToSave.LineCountPrPage = line.LineCountPrPage;
                    lineToSave.PrintAsSlip = line.PrintAsSlip;
                    lineToSave.PrintBehavior = line.PrintBehavior;
                    lineToSave.TypeDescription = line.TypeDescription;
                    lineToSave.Text = line.Text;
                    lineToSave.NumberOfCopies = line.NumberOfCopies;
                    Providers.FormProfileLineData.Save(entry, lineToSave);
                }
            }
        }

        #region ISequenceable Members

        public RecordIdentifier SequenceID
        {
            get { return "FORMPROFILE"; }
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSFORMPROFILE", "PROFILEID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
