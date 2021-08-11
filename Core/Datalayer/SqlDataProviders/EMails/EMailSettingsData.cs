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
    /// A Data provider that retrieves the data for the business object <see cref="EMailSetting"/>
	/// </summary>
    public class EMailSettingData : SqlServerDataProviderBase, IEMailSettingData
	{
		private static string BaseSelectString
		{
			get
			{
			    return
                    @"SELECT EMAILSETTINGID
                  , ISNULL(DATAAREAID, '') DATAAREAID
                  , ISNULL(STOREID, '') STOREID
                  , SMTPSERVER
                  , SMTPPORT
                  , SMTPEMAILADDRESS
                  , SMTPPASSWORD
                  , SMTPDISPLAYNAME
                  , USESSL
                  , TEXTONLY
                  , SIGNATURE";
			}
		}
        private static void Populate(IDataReader dr, EMailSetting rec)
		{
            rec.ID = AsInt(dr["EMAILSETTINGID"]);
            rec.DataAreaID = AsString(dr["DATAAREAID"]);
            rec.StoreID = AsString(dr["STOREID"]);
            rec.SmtpServer = AsString(dr["SMTPSERVER"]);
            rec.SmtpPort = AsInt(dr["SMTPPORT"]);
            rec.SmtpEMailAddress = AsString(dr["SMTPEMAILADDRESS"]);
            rec.SmtpPassword = AsString(dr["SMTPPASSWORD"]);
            rec.SmtpDisplayName = AsString(dr["SMTPDISPLAYNAME"]);
            rec.UseSSL = AsBool(dr["USESSL"]);
            rec.TextOnly = AsBool(dr["TEXTONLY"]);
            rec.Signature = AsString(dr["SIGNATURE"]);
		}

	    /// <summary>
	    /// Gets all settings
	    /// </summary>
	    /// <param name="entry">The entry into the database</param>
	    /// <returns>An instance of <see cref="EMailSetting"/></returns>
	    public virtual List<EMailSetting> GetList(IConnectionManager entry)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					BaseSelectString +
                    @" FROM EMAILSETTINGS 
                    where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<EMailSetting>(entry, cmd, CommandType.Text, Populate);
			}
		}

        public virtual EMailSetting Get(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString + @" FROM EMAILSETTINGS
                       where DATAAREAID = @dataAreaId ";
                if (string.IsNullOrEmpty((string) storeID))
                {
                    cmd.CommandText += "AND STOREID IS NULL OR STOREID = ''";
                }
                else
                {
                    cmd.CommandText += "AND STOREID = @storeID";
                    MakeParam(cmd, "storeID", storeID);
                }
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var list = Execute<EMailSetting>(entry, cmd, CommandType.Text, Populate);
                if (list != null && list.Count > 0)
                    return list[0];

                return null;
            }
        }

        public virtual void Save(IConnectionManager entry, EMailSetting emailSetting)
		{
            var statement = entry.Connection.CreateStatement("EMAILSETTINGS");

            if (emailSetting.ID.IsEmpty || ((int)emailSetting.ID == 0))
			{
				statement.StatementType = StatementType.Insert;
				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

			    statement.IsIdentityInsert = true;
            }
			else
			{
				statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("EMAILSETTINGID", (int)emailSetting.ID, SqlDbType.Int);
			}

            statement.AddField("STOREID", (string)emailSetting.StoreID);
            statement.AddField("SMTPSERVER", emailSetting.SmtpServer);
            statement.AddField("SMTPPORT", emailSetting.SmtpPort, SqlDbType.Int);
            statement.AddField("SMTPEMAILADDRESS", emailSetting.SmtpEMailAddress);
            statement.AddField("SMTPPASSWORD", emailSetting.SmtpPassword);
            statement.AddField("SMTPDISPLAYNAME", emailSetting.SmtpDisplayName);
            statement.AddField("USESSL", emailSetting.UseSSL ? 1 : 0, SqlDbType.Int);
            statement.AddField("TEXTONLY", emailSetting.TextOnly ? 1 : 0, SqlDbType.Int);
            statement.AddField("SIGNATURE", emailSetting.Signature);

            if (statement.IsIdentityInsert)
                emailSetting.EMailSettingID = Convert.ToInt32(entry.Connection.ExecuteScalar(statement.Command));
            else
                entry.Connection.ExecuteStatement(statement);
		}
	}
}

