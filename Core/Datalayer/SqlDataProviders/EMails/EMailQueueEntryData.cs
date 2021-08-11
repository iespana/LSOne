using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.EMails;
using LSOne.DataLayer.DataProviders.EMails;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.EMails
{
	/// <summary>
    /// A Data provider that retrieves the data for the business object <see cref="EMailQueueEntry"/>
	/// </summary>
    public class EMailQueueEntryData : SqlServerDataProviderBase, IEMailQueueEntryData
	{
		private static string BaseSelectString
		{
			get
			{
			    return
                    @"SELECT <top> EMAILID
                  , ISNULL(DATAAREAID, '') DATAAREAID
                  , ISNULL(STOREID, '') STOREID
                  , SENT
                  , PRIORITY
                  , ATTEMPTS
                  , ISNULL(LASTERROR, '') LASTERROR
                  , ISNULL([TO], '') [TO]
                  , ISNULL(CC, '') CC
                  , ISNULL(BCC, '') BCC
                  , ISNULL(SUBJECT, '') SUBJECT
                  , BODY
                  , BODYISHTML
                  , CREATED
                  , MODIFIED";
			}
		}

	    private static string IndexedSelectString
	    {
	        get
	        {
                return @"select ss.* from( 
                    Select s.*, ROW_NUMBER() OVER(order by <ORDER> ) AS ROW  from (
                    select distinct 

	                e.EMAILID
                    , ISNULL(DATAAREAID, '') DATAAREAID
	                , ISNULL(e.STOREID, '') STOREID
	                , e.SENT
	                , e.PRIORITY
	                , e.ATTEMPTS
	                , ISNULL(e.LASTERROR, '') LASTERROR
	                , ISNULL([TO], '') [TO]
	                , ISNULL(CC, '') CC
	                , ISNULL(BCC, '') BCC
	                , ISNULL(SUBJECT, '') SUBJECT
	                , e.CREATED

                    from EMAILQUEUE e
	                ) s
	                ) ss
	                where DATAAREAID = @dataAreaId <SENT> AND ROW between @rowFrom and @rowTo ORDER by <ORDER>";
	        }
	    }

        private static string ResolveSort(EMailSortEnum sort)
        {
            switch (sort)
            {
                case EMailSortEnum.Priority: return "PRIORITY";
                case EMailSortEnum.Sent: return "SENT";
                case EMailSortEnum.Created: return "CREATED";
                case EMailSortEnum.Store: return "STOREID";
                case EMailSortEnum.Attempts: return "ATTEMPTS";
                case EMailSortEnum.Error: return "LASTERROR";
                case EMailSortEnum.To: return "TO";
                case EMailSortEnum.Subject: return "SUBJECT";
            }

            return string.Empty;
        }

        private static void Populate(IDataReader dr, EMailQueueEntry rec)
        {
            PopulateSome(dr, rec);
            rec.Body = AsString(dr["BODY"]);
            rec.BodyIsHTML = AsBool(dr["BODYISHTML"]);
            rec.Modified = AsDateTime(dr["MODIFIED"]);
        }

        private static void PopulateSome(IDataReader dr, EMailQueueEntry rec)
        {
            rec.ID = AsInt(dr["EMAILID"]);
            //rec.DataAreaID = AsString(dr["DATAAREAID"]);
            rec.StoreID = AsString(dr["STOREID"]);
            rec.Sent = AsDateTime(dr["SENT"]);
            rec.Priority = AsInt(dr["PRIORITY"]);
            rec.Attempts = AsInt(dr["ATTEMPTS"]);
            rec.LastError = AsString(dr["LASTERROR"]);
            rec.To = AsString(dr["TO"]);
            rec.CC = AsString(dr["CC"]);
            rec.BCC = AsString(dr["BCC"]);
            rec.Subject = AsString(dr["SUBJECT"]);
            rec.Created = AsDateTime(dr["CREATED"]);
        }

        /// <summary>
        /// Gets the specified entry.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="unsentOnly">If true, only fetch unsent items</param>
        /// <returns>An instance of <see cref="EMailQueueEntry"/></returns>
        public virtual int Count(IConnectionManager entry, bool unsentOnly)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM EMAILQUEUE " +
                                  "where DATAAREAID = @dataAreaId ";
                if (unsentOnly)
                    cmd.CommandText += "AND (SENT is NULL OR SENT < '1901-01-01')";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return (int)entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Gets the specified entry.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="unsentOnly">If true, only fetch unsent items</param>
        /// <param name="topEntries">Maximum number of entries to fetch</param>
        /// <param name="maxAttempts">Only fetch items where Attempts is less than maxAttempts</param>
        /// <returns>An instance of <see cref="EMailQueueEntry"/></returns>
        public virtual List<EMailQueueEntry> GetList(IConnectionManager entry, bool unsentOnly, int topEntries, int maxAttempts)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString.Replace("<top>", string.Format("TOP({0})", topEntries)) +
                    @" FROM EMAILQUEUE 
                    where DATAAREAID = @dataAreaId ";
                if (unsentOnly)
                    cmd.CommandText += "AND (SENT is NULL OR SENT < '1901-01-01')";
                cmd.CommandText += " AND (ATTEMPTS < @attempts) ";
                cmd.CommandText += "ORDER BY PRIORITY DESC,EMAILID ASC";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "attempts", maxAttempts, SqlDbType.Int);

                return Execute<EMailQueueEntry>(entry, cmd, CommandType.Text, Populate);
            }
        }

	    /// <summary>
	    /// Gets the specified entry.
	    /// </summary>
	    /// <param name="entry">The entry into the database</param>
	    /// <param name="ID">ID of email entry</param>
	    /// <returns>An instance of <see cref="EMailQueueEntry"/></returns>
	    public virtual EMailQueueEntry Get(IConnectionManager entry, int ID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString.Replace("<top>", "") +
                    @" FROM EMAILQUEUE 
                    where DATAAREAID = @dataAreaId AND EMAILID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", ID, SqlDbType.Int);

                var res = Execute<EMailQueueEntry>(entry, cmd, CommandType.Text, Populate);
                if (res != null && res.Count > 0)
                    return res[0];

                return null;
            }
        }

	    /// <summary>
	    /// Gets the specified entry.
	    /// </summary>
	    /// <param name="entry">The entry into the database</param>
	    /// <param name="unsentOnly">If true, only fetch unsent items</param>
	    /// <param name="index">Index of first entry</param>
	    /// <param name="maxEntries">Maximum number of entries to fetch</param>
	    /// <param name="sort">Sorting to perform</param>
	    /// <returns>An instance of <see cref="EMailQueueEntry"/></returns>
	    public virtual List<EMailQueueEntry> GetIndexedList(IConnectionManager entry, bool unsentOnly, int index, int maxEntries, EMailSortEnum sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    IndexedSelectString.Replace("<ORDER>", ResolveSort(sort)).
                                        Replace("<SENT>", unsentOnly ? "AND (SENT is NULL OR SENT < '1901-01-01')" : "");

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "rowFrom", index, SqlDbType.Int);
                MakeParam(cmd, "rowTo", index + maxEntries, SqlDbType.Int);

                return Execute<EMailQueueEntry>(entry, cmd, CommandType.Text, PopulateSome);
            }
        }

        public virtual void Truncate(IConnectionManager entry, DateTime created)
        {
            // Get a list of all e-mails that should be deleted
            var emailIDs = new List<int>();
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT EMAILID FROM EMAILQUEUE WHERE DATAAREAID = @dataAreaId AND CREATED < @created " +
                    "AND (SENT is NULL OR SENT < '1901-01-01')";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "created", created, SqlDbType.DateTime);

                using (var reader = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                {
                    while (reader.Read())
                    {
                        emailIDs.Add((int) reader["EMAILID"]);
                    }
                }
            }

            // Delete each in turn
            foreach (var id in emailIDs)
                Delete(entry, id);
        }

        public virtual void Delete (IConnectionManager entry, int emailID)
        {
            // DataAreaID is not necessary here since EMAILID is an identity column
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM EMAILQUEUEATTACHMENT WHERE EMAILID = @emailID";
                MakeParam(cmd, "emailID", emailID, SqlDbType.Int);

                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM EMAILQUEUE WHERE EMAILID = @emailID";
                MakeParam(cmd, "emailID", emailID, SqlDbType.Int);

                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }
        }

        public virtual void Save(IConnectionManager entry, EMailQueueEntry emailQueueEntry)
		{
            var statement = entry.Connection.CreateStatement("EMAILQUEUE");

            DateTime created;
            if (emailQueueEntry.ID.IsEmpty || ((int)emailQueueEntry.ID == 0))
			{
				statement.StatementType = StatementType.Insert;
				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

			    created = DateTime.Now;

			    statement.IsIdentityInsert = true;
            }
			else
			{
				statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("EMAILID", (int)emailQueueEntry.ID, SqlDbType.Int);

			    created = emailQueueEntry.Created;
			}

            statement.AddField("STOREID", (emailQueueEntry.StoreID == null) ? null : (string)emailQueueEntry.StoreID);
            statement.AddField("SENT", ToDateTime(emailQueueEntry.Sent), SqlDbType.DateTime);
            statement.AddField("PRIORITY", emailQueueEntry.Priority, SqlDbType.Int);
            statement.AddField("ATTEMPTS", emailQueueEntry.Attempts, SqlDbType.Int);
            statement.AddField("LASTERROR", emailQueueEntry.LastError);
            statement.AddField("TO", ToString(emailQueueEntry.To, 500));
            statement.AddField("CC", ToString(emailQueueEntry.CC, 500));
            statement.AddField("BCC", ToString(emailQueueEntry.BCC, 500));
            statement.AddField("SUBJECT", ToString(emailQueueEntry.Subject, 200));
            statement.AddField("BODY", emailQueueEntry.Body);
            statement.AddField("BODYISHTML", emailQueueEntry.BodyIsHTML ? 1 : 0, SqlDbType.Int);
            statement.AddField("CREATED", created, SqlDbType.DateTime);
            statement.AddField("MODIFIED", DateTime.Now, SqlDbType.DateTime);

            if (statement.IsIdentityInsert)
            {
                emailQueueEntry.ID = Convert.ToInt32(entry.Connection.ExecuteScalar(statement.Command));
            }
            else
                entry.Connection.ExecuteStatement(statement);
        }

	    public void Update(IConnectionManager entry, RecordIdentifier emailQueueEntryID, 
            DateTime sent, int attempts, string lastError)
	    {
	        using (var cmd = entry.Connection.CreateCommand())
	        {
	            cmd.CommandText = "UPDATE EMAILQUEUE SET ";

	            if (sent != DateTime.MinValue)
	            {
	                cmd.CommandText += " SENT = @sent, ";
	                MakeParam(cmd, "sent", sent, SqlDbType.DateTime);
	            }

	            cmd.CommandText += " ATTEMPTS = @attempts, ";
	            MakeParam(cmd, "attempts", attempts, SqlDbType.Int);

                cmd.CommandText += "LASTERROR = @lastError, ";
                MakeParam(cmd, "lastError", ToString(lastError, 100));

                cmd.CommandText += "MODIFIED = @modified";
                MakeParam(cmd, "modified", DateTime.Now, SqlDbType.DateTime);

                cmd.CommandText += " WHERE EMAILID = @id";
                MakeParam(cmd, "id", emailQueueEntryID);
                
                Execute<EMailQueueEntry>(entry, cmd, CommandType.Text, Populate);
            }
	    }
	}
}

