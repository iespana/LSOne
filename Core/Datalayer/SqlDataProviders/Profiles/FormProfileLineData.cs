using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Profiles
{
    public class FormProfileLineData : SqlServerDataProviderBase, IFormProfileLineData
    {
        private static string ResolveSort(FormProfileLineSorting sort, bool backwards)
        {
            switch (sort)
            {
                case FormProfileLineSorting.TypeDescription:
                    return string.Format("TYPEDESCRIPTION {0}", (backwards ? "DESC" : "ASC"));
                case FormProfileLineSorting.Description:
                    return string.Format("DESCRIPTION {0}", (backwards ? "DESC" : "ASC"));
                case FormProfileLineSorting.PrintAsSlip:
                    return string.Format("PRINTASSLIP {0}", (backwards ? "DESC" : "ASC"));
                case FormProfileLineSorting.PrintBehavior:
                    return string.Format("PRINTBEHAVIOUR {0}", (backwards ? "DESC" : "ASC"));
                case FormProfileLineSorting.LineCountPerPage:
                    return string.Format("LINECOUNTPRPAGE {0}", (backwards ? "DESC" : "ASC"));
            }
            return "";
        }

        protected string BaseSql
        {
            get
            {
                return @"SELECT 
                        R.PROFILEID,
                        R.FORMTYPEID,
                        ISNULL(R.FORMLAYOUTID,'') as FORMLAYOUTID,
                        ISNULL(F.DESCRIPTION, '') as DESCRIPTION, 
                        ISNULL(T.DESCRIPTION, '') AS TYPEDESCRIPTION, 
                        ISNULL(F.PRINTASSLIP, 0) AS PRINTASSLIP,
                        ISNULL(F.PRINTBEHAVIOUR, 0) AS PRINTBEHAVIOUR,
                        ISNULL(F.LINECOUNTPRPAGE, 55) AS LINECOUNTPRPAGE,
                        ISNULL(R.ISSYSTEMPROFILELINE, 0) AS ISSYSTEMPROFILELINE,
                        NUMBEROFCOPIES
                        FROM POSFORMPROFILELINES R
                        LEFT OUTER JOIN POSFORMTYPE T ON R.FORMTYPEID = T.ID AND R.DATAAREAID = T.DATAAREAID
                        LEFT OUTER JOIN POSISFORMLAYOUT F ON R.FORMLAYOUTID = F.ID AND R.DATAAREAID = F.DATAAREAID";
            }
        }

        private static void PopulateProfileLine(IDataReader dr, FormProfileLine profileLine)
        {
            profileLine.ProfileID = (Guid)dr["PROFILEID"];
            profileLine.ReceiptTypeID = (Guid)dr["FORMTYPEID"];
            profileLine.FormLayoutID = (string)dr["FORMLAYOUTID"];
            profileLine.Text = (string)dr["DESCRIPTION"];
            profileLine.TypeDescription = (string)dr["TYPEDESCRIPTION"];
            profileLine.PrintAsSlip = (byte)dr["PRINTASSLIP"] != 0;
            profileLine.PrintBehavior = (PrintBehaviors)(int)dr["PRINTBEHAVIOUR"];
            profileLine.LineCountPrPage = (int)dr["LINECOUNTPRPAGE"];
            profileLine.IsSystemProfileLine = AsBool(dr["ISSYSTEMPROFILELINE"]);
            profileLine.NumberOfCopies = (int)dr["NUMBEROFCOPIES"];
        }

        /// <summary>
        /// Saves a form line under a profile
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="profileLine">profile which list gets saved under</param>
        public virtual void Save(IConnectionManager entry, FormProfileLine profileLine)
        {
            var statement = new SqlServerStatement("POSFORMPROFILELINES");

            ValidateSecurity(entry, BusinessObjects.Permission.FormProfileEdit);

            bool isNew = false;
            if (profileLine.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                profileLine.ID = Guid.NewGuid();
            }

            if (isNew || !IDExists(entry, profileLine.ID.PrimaryID, profileLine.ID.SecondaryID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("PROFILEID", (Guid) profileLine.ProfileID, SqlDbType.UniqueIdentifier);
                statement.AddKey("FORMTYPEID", (Guid)profileLine.ReceiptTypeID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("PROFILEID", (Guid) profileLine.ProfileID, SqlDbType.UniqueIdentifier);
                statement.AddCondition("FORMTYPEID", (Guid)profileLine.ReceiptTypeID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("FORMLAYOUTID", (string) profileLine.FormLayoutID);
            statement.AddField("DESCRIPTION", profileLine.Text);
            statement.AddField("NUMBEROFCOPIES", profileLine.NumberOfCopies, SqlDbType.Int);

            Save(entry, profileLine, statement);
        }

        /// <summary>
        /// Gets a list of all form profile names
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all form profiles</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "POSFORMPROFILELINES", "DESCRIPTION", "PROFILEID", "DESCRIPTION");
        }

        /// <summary>
        /// Gets a list of all form profile Lines sorted
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="sort">How to sort list</param>
        /// <param name="sortBackwards"></param>
        /// <returns>A list of all form profile lines sorted</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, FormProfileLineSorting sort, bool sortBackwards)
        {
            return GetList<DataEntity>(entry, "POSFORMPROFILELINES", "DESCRIPTION", "PROFILEID", ResolveSort(sort, sortBackwards));
        }

        public virtual List<FormProfileLine> GetLines(IConnectionManager entry, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " where R.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<FormProfileLine>(entry, cmd, CommandType.Text, PopulateProfileLine);
            }
        }

        /// <summary>
        /// Returns a list of formProfileLines with the given profileID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the profile to return the lines for</param>
        /// <param name="cache">CacheType</param>
        /// <returns>A list of formProfileLines with the given ProfileID</returns>
        public virtual List<FormProfileLine> Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " where R.PROFILEID = @id and R.DATAAREAID = @dataAreaId order by PROFILEID";

                MakeParam(cmd, "id", (Guid)id);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<FormProfileLine>(entry, cmd, CommandType.Text, PopulateProfileLine);
            }
        }

        /// <summary>
        /// Returns a list of formProfileLines with the given profileID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the profile to return the lines for</param>
        /// <param name="sortBackwards"></param>
        /// <param name="cache">CacheType</param>
        /// <param name="sort">Sort string</param>
        /// <returns>A list of formProfileLines with the given ProfileID</returns>
        public virtual List<FormProfileLine> GetProfileLines(IConnectionManager entry, RecordIdentifier id, FormProfileLineSorting sort, bool sortBackwards, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " where R.PROFILEID = @id and R.DATAAREAID = @dataAreaId order by " + ResolveSort(sort, sortBackwards);

                MakeParam(cmd, "id", (Guid)id);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<FormProfileLine>(entry, cmd, CommandType.Text, PopulateProfileLine);
            }
        }

        /// <summary>
        /// Returns a list of formProfileLines with the given typeID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the type to return the lines for</param>
        /// <param name="cache">CacheType</param>
        /// <returns>A list of formProfileLines with the given TypeID</returns>
        public virtual List<FormProfileLine> GetLinesByTypeId(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " where R.FORMTYPEID = @id and R.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "id", (Guid)id);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<FormProfileLine>(entry, cmd, CommandType.Text, PopulateProfileLine);
            }
        }

        /// <summary>
        /// Returns a list of FormProfileLine with the given formLayoutID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the form layout to return the lines for</param>
        /// <param name="cache">CacheType</param>
        /// <returns>A list of formProfileLines with the given FormLayoutID</returns>
        public virtual List<FormProfileLine> GetLinesByFormLayoutId(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " where R.FORMLAYOUTID = @id and R.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "id", id);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<FormProfileLine>(entry, cmd, CommandType.Text, PopulateProfileLine);
            }
        }

        /// <summary>
        /// Returns a FormProfileLine with the given primaryID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="profileId">The id of the profile/param> </param>
        /// <param name="receiptTypeId">The id of the form type</param>
        /// <param name="cache">CacheType</param>
        /// <returns>A FormProfileLine with the given PrimaryID</returns>
        public virtual FormProfileLine GetFormProfileLine(IConnectionManager entry, RecordIdentifier profileId, RecordIdentifier receiptTypeId, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " where R.PROFILEID = @profileId and R.FORMTYPEID = @receiptTypeId and R.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "profileId", (Guid)profileId);
                MakeParam(cmd, "receiptTypeId", (Guid)receiptTypeId);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Get<FormProfileLine>(entry, cmd, new RecordIdentifier(profileId, receiptTypeId), PopulateProfileLine, cache, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Returns a list of all user defined profile lines from a specific profile
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="profileId">The id of the profile/param> </param>
        /// <param name="cache">CacheType</param>
        /// <returns></returns>
        public virtual List<FormProfileLine> GetUserDefinedFormProfileLines(IConnectionManager entry, RecordIdentifier profileId, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " where R.PROFILEID = @profileId and T.SYSTEMTYPE = 0 and R.DATAAREAID = @dataAreaId";


                MakeParam(cmd, "profileId", (Guid)profileId);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<FormProfileLine>(entry, cmd, CommandType.Text, PopulateProfileLine);
            }
        }

        /// <summary>
        /// Deletes a pos FormProfileLine with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="profileID">The ID of the profile</param>
        /// <param name="receiptTypeID">The ID of the form type</param>
        public virtual void DeleteProfileLine(IConnectionManager entry, RecordIdentifier profileID, RecordIdentifier receiptTypeID)
        {
            DeleteRecord(entry, "POSFORMPROFILELINES", new[] { "PROFILEID", "FORMTYPEID" }, new RecordIdentifier(profileID, receiptTypeID), BusinessObjects.Permission.FormProfileEdit);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<FormProfileLine>(entry, "POSFORMPROFILELINES", "PROFILEID", (Guid)id);
        }

        public virtual bool IDExists(IConnectionManager entry, RecordIdentifier profileID, RecordIdentifier receiptTypeID)
        {
            return RecordExists(entry, "POSFORMPROFILELINES", new[] { "PROFILEID", "FORMTYPEID" }, new RecordIdentifier(profileID, receiptTypeID));
        }

        #region ISequenceable Members

        public RecordIdentifier SequenceID
        {
            get { return "FORMPROFILELINES"; }
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSFORMPROFILELINES", "PROFILEID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
