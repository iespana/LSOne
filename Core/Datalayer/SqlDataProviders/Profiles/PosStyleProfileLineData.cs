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
    /// <summary>
    /// A data provider for the <see cref="PosStyleProfileLine"/> business object
    /// </summary>
    public class PosStyleProfileLineData : SqlServerDataProviderBase, IPosStyleProfileLineData
    {
        private static string BaseSql
        {
            get
            {
                return @"SELECT 
                        S.ID,
                        S.PROFILEID, 
                        ISNULL(S.MENUID,'') as MENUID,
                        ISNULL(S.STYLEID, '') as STYLEID, 
                        ISNULL(S.SYSTEM, 0) as SYSTEM,
                        ISNULL(S.CONTEXTID, '') as CONTEXTID,
                        ISNULL (M.DESCRIPTION, '') AS MENUDESCRIPTION,
                        ISNULL(ST.NAME, '') AS STYLEDESCRIPTION,
                        ISNULL(C.NAME, '') AS CONTEXTDESCRIPTION
                        FROM POSSTYLEPROFILELINE S
                        LEFT OUTER JOIN POSMENUHEADER M ON S.MENUID = M.MENUID AND S.DATAAREAID = M.DATAAREAID
                        LEFT OUTER JOIN POSSTYLE ST ON S.STYLEID =  ST.ID AND S.DATAAREAID = ST.DATAAREAID
                        LEFT OUTER JOIN POSCONTEXT C ON S.CONTEXTID = C.ID AND S.DATAAREAID = C.DATAAREAID";
            }
        }

        private static void PopulateProfile(IDataReader dr, PosStyleProfileLine profile)
        {
            profile.PosStyleProfileLineId = (string)dr["ID"];
            profile.System = (byte)dr["SYSTEM"] != 0;
            profile.ProfileID = (string)dr["PROFILEID"];
            profile.MenuID = (string)dr["MENUID"];
            profile.StyleID = (string)dr["STYLEID"];
            profile.ContextID = (string)dr["CONTEXTID"];
            profile.StyleDescription = (string)dr["STYLEDESCRIPTION"];
            profile.MenuDescription = (string)dr["MENUDESCRIPTION"];
            profile.ContextDescription = (string)dr["CONTEXTDESCRIPTION"];
        }

        /// <summary>
        /// Saves a <see cref="PosStyleProfileLine"/> object to the databasetable 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posStyleProfileLine">The <see cref="PosStyleProfileLine"/> to be saved</param>
        public virtual void Save(IConnectionManager entry, PosStyleProfileLine posStyleProfileLine)
        {
            var statement = new SqlServerStatement("POSSTYLEPROFILELINE");

            ValidateSecurity(entry, BusinessObjects.Permission.StyleProfileEdit);
            posStyleProfileLine.Validate();

            bool isNew = false;
            if (posStyleProfileLine.PosStyleProfileLineId == RecordIdentifier.Empty)
            {
                isNew = true;
                posStyleProfileLine.PosStyleProfileLineId = DataProviderFactory.Instance.GenerateNumber<IPosStyleProfileLineData, PosStyleProfileLine>(entry);
            }

            if (isNew)
                //|| !Exists(entry, posStyleProfileLine.ID) || !Exists(entry, posStyleProfileLine.ProfileID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", (string) posStyleProfileLine.PosStyleProfileLineId);
                statement.AddKey("PROFILEID", (string) posStyleProfileLine.ProfileID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (string) posStyleProfileLine.PosStyleProfileLineId);
                statement.AddCondition("PROFILEID", (string) posStyleProfileLine.ProfileID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("MENUID", (string) posStyleProfileLine.MenuID);
            statement.AddField("STYLEID", (string) posStyleProfileLine.StyleID);
            statement.AddField("CONTEXTID", (string) posStyleProfileLine.ContextID);
            statement.AddField("SYSTEM", posStyleProfileLine.System ? 1 : 0, SqlDbType.TinyInt);

            Save(entry, posStyleProfileLine, statement);
        }

        /// <summary>
        /// Checks if a <see cref="PosStyleProfileLine"/> with the given ID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the profile line to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "POSSTYLEPROFILELINE", "ID", id);
        }

        /// <summary>
        /// Returns a list of <see cref="PosStyleProfileLine"/> with a specific ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the profile to return the lines for</param>
        /// <param name="cache">CacheType</param>
        /// <returns>A list of <see cref="PosStyleProfileLine"/> for the specific ID</returns>
        public virtual List<PosStyleProfileLine> Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " where S.PROFILEID = @id and S.DATAAREAID = @dataAreaId order by PROFILEID";

                MakeParam(cmd, "id", (string)id);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PosStyleProfileLine>(entry, cmd, CommandType.Text, PopulateProfile);
            }
        }

        /// <summary>
        /// Returns a list of <see cref="PosStyleProfileLine"/> with the given profileID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the profile to return the lines for</param>
        /// <param name="cache">CacheType</param>
        /// <param name="sort">Sorting string</param>
        /// <returns>A list of <see cref="PosStyleProfileLine"/> for the specific ID</returns>
        public virtual List<PosStyleProfileLine> GetProfileLines(IConnectionManager entry, RecordIdentifier id, string sort, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " where S.PROFILEID = @id and S.DATAAREAID = @dataAreaId order by " + sort;

                MakeParam(cmd, "id", (string)id);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PosStyleProfileLine>(entry, cmd, CommandType.Text, PopulateProfile);
            }
        }

        public virtual bool ProfileLineExists(IConnectionManager entry, PosStyleProfileLine posStyleProfileLine, RecordIdentifier profileID, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " WHERE S.PROFILEID = @profileID AND S.DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "profileID", (string)profileID);                
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                if (posStyleProfileLine.MenuID != null && posStyleProfileLine.MenuID != RecordIdentifier.Empty)
                {
                    cmd.CommandText += " AND S.MENUID = @menuID ";
                    MakeParam(cmd, "menuID", (string)posStyleProfileLine.MenuID);
                }

                if (posStyleProfileLine.ContextID != null && posStyleProfileLine.ContextID != RecordIdentifier.Empty)
                {
                    cmd.CommandText += " AND S.CONTEXTID = @contextID ";
                    MakeParam(cmd, "contextID", (string)posStyleProfileLine.ContextID);
                }

                if (posStyleProfileLine.StyleID != null && posStyleProfileLine.StyleID != RecordIdentifier.Empty)
                {
                    cmd.CommandText += " AND S.STYLEID = @styleID ";
                    MakeParam(cmd, "styleID", (string)posStyleProfileLine.StyleID);
                }

                var profileLine = Get<PosStyleProfileLine>(entry, cmd, profileID, PopulateProfile, cache, UsageIntentEnum.Normal);

                return (profileLine != null && profileLine.ID != RecordIdentifier.Empty);
            }
        }

        /// <summary>
        /// Returns a <see cref="PosStyleProfileLine"/> with the given profileID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="profileLineID">The id of the profile line to check for</param>
        /// <param name="profileID">The id of the profile of the line</param>
        /// <param name="cache">CacheType</param>
        /// <returns>A <see cref="PosStyleProfileLine"/></returns>
        public virtual PosStyleProfileLine GetProfileLine(IConnectionManager entry, RecordIdentifier profileLineID, RecordIdentifier profileID, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " where S.PROFILEID = @profileID and S.ID = @profileLineID and S.DATAAREAID = @dataAreaId order by PROFILEID";

                MakeParam(cmd, "profileID", (string)profileID);
                MakeParam(cmd, "profileLineID", (string)profileLineID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                
                return Get<PosStyleProfileLine>(entry, cmd,new RecordIdentifier(profileID,profileLineID), PopulateProfile, cache, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Deletes a specific profile line ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="profileLineID">The ID to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier profileLineID)
        {
            DeleteRecord<PosStyleProfileLine>(entry, "POSSTYLEPROFILELINE", "ID", profileLineID, BusinessObjects.Permission.StyleProfileEdit);
        }

        /// <summary>
        /// Deletes all profile lines for a a specific style profile
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="profileID">The profile ID to delete</param>
        public virtual void DeleteLines(IConnectionManager entry, RecordIdentifier profileID)
        {
            var profiles = Get(entry, profileID);
            if (profiles != null)
            {
                foreach (var profile in profiles)
                {
                    var profileLineID = profile.PosStyleProfileLineId;
                    DeleteRecord<PosStyleProfileLine>(entry, "POSSTYLEPROFILELINE", "ID", profileLineID, BusinessObjects.Permission.StyleProfileEdit);
                }
            }
        }

        #region ISequenceable Members

        /// <summary>
        /// Returns true if the sequence exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The sequence ID to check</param>
        /// <returns>True if the sequence exists otherwise false is returned</returns>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Providers.PosContextData.Exists(entry, id);
        }

        /// <summary>
        /// The SequenceID name used for <see cref="PosStyleProfileLine"/>
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "POSSTYLEPROFILELINE"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSSTYLEPROFILELINE", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
