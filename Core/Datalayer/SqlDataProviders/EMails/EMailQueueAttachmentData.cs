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
    /// A Data provider that retrieves the data for the business object <see cref="EMailQueueAttachment"/>
	/// </summary>
    public class EMailQueueAttachmentData : SqlServerDataProviderBase, IEMailQueueAttachmentData
	{
		private static string BaseSelectString
		{
			get
			{
			    return
                    @"SELECT EMAILATTACHMENTID
                    , ISNULL(DATAAREAID, '') DATAAREAID
                    , EMAILID
                    , NAME
                    , ATTACHMENT";
			}
		}
        private static void Populate(IDataReader dr, EMailQueueAttachment rec)
		{
            rec.ID = AsInt(dr["EMAILATTACHMENTID"]);
            rec.EMailID = AsInt(dr["EMAILID"]);
            rec.DataAreaID = AsString(dr["DATAAREAID"]);
            rec.Name = AsString(dr["NAME"]);
            rec.Attachment = (byte[])dr["ATTACHMENT"];
		}

	    /// <summary>
	    /// Gets attachments for a specific email
	    /// </summary>
	    /// <param name="entry">The entry into the database</param>
	    /// <param name="emailid">Email id that attachments are attached to</param>
	    /// <returns>An instance of <see cref="EMailQueueAttachment"/></returns>
	    public virtual List<EMailQueueAttachment> GetList(IConnectionManager entry, RecordIdentifier emailid)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					BaseSelectString +
                    @" FROM EMAILQUEUEATTACHMENT 
                    where DATAAREAID = @dataAreaId AND EMAILID = @emailid";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "emailid", (int)emailid, SqlDbType.Int);

                return Execute<EMailQueueAttachment>(entry, cmd, CommandType.Text, Populate);
			}
		}

        public virtual void Save(IConnectionManager entry, EMailQueueAttachment emailQueueAttachment)
		{
            var statement = entry.Connection.CreateStatement("EMAILQUEUEATTACHMENT");

            if (emailQueueAttachment.ID.IsEmpty || (int)emailQueueAttachment.ID == 0)
            {
				statement.StatementType = StatementType.Insert;
				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.IsIdentityInsert = true;
            }
			else
			{
				statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("EMAILATTACHMENTID", (int)emailQueueAttachment.ID, SqlDbType.Int);
			}

            statement.AddField("EMAILID", (int)emailQueueAttachment.EMailID, SqlDbType.Int);
            statement.AddField("NAME", emailQueueAttachment.Name);
            statement.AddField("ATTACHMENT", emailQueueAttachment.Attachment, SqlDbType.VarBinary);

            if (statement.IsIdentityInsert)
                emailQueueAttachment.ID = Convert.ToInt32(entry.Connection.ExecuteScalar(statement.Command));
            else
                entry.Connection.ExecuteStatement(statement);
		}
	}
}

