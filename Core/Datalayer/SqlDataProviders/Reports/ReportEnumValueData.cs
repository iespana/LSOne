using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.DataProviders.Reports;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Reports
{
    public class ReportEnumValueData : SqlServerDataProviderBase, IReportEnumValueData
    {
        private static void PopulateEnum(IDataReader dr, ReportEnumValue enumValue)
        {
            enumValue.ReportID = (Guid)dr["REPORTGUID"];
            enumValue.EnumName = (string)dr["ENUMNAME"];
            enumValue.Text = (string)dr["ENUMTEXT"];
            enumValue.EnumValue = (int)dr["ENUMVALUE"];
            enumValue.LanguageID = (string)dr["LANGUAGEID"];
            enumValue.Label = (string)dr["LABEL"];
        }

        public virtual List<ReportEnumValue> GetEnumValues(IConnectionManager entry, RecordIdentifier reportID, string enumName )
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"select r.REPORTGUID, r.LANGUAGEID, r.ENUMNAME, r.ENUMVALUE, COALESCE(lang2.ENUMTEXT,lang1.ENUMTEXT,r.ENUMTEXT) as ENUMTEXT, COALESCE(lang2.LABEL,lang1.LABEL,r.LABEL) as LABEL
                                    from REPORTENUMS r
                                    left outer join REPORTENUMS lang1 on r.REPORTGUID = lang1.REPORTGUID and r.DATAAREAID = lang1.DATAAREAID and lang1.LANGUAGEID = @baseLanguage and r.ENUMNAME = lang1.ENUMNAME and r.ENUMVALUE = lang1.ENUMVALUE
                                    left outer join REPORTENUMS lang2 on r.REPORTGUID = lang2.REPORTGUID and r.DATAAREAID = lang2.DATAAREAID and lang2.LANGUAGEID = @language and r.ENUMNAME = lang2.ENUMNAME and r.ENUMVALUE = lang2.ENUMVALUE
                                    where r.REPORTGUID = @reportID and r.ENUMNAME = @enumName and r.DATAAREAID = @dataareaID";

                MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "reportID", (Guid)reportID);
                MakeParam(cmd, "enumName", enumName);
                MakeParam(cmd, "baseLanguage", System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
                MakeParam(cmd, "language", System.Threading.Thread.CurrentThread.CurrentUICulture.Name);

                return Execute<ReportEnumValue>(entry, cmd, CommandType.Text, PopulateEnum);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "REPORTENUMS", new string[] { "REPORTGUID", "LANGUAGEID", "ENUMNAME", "ENUMVALUE" }, id);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier reportID, string languageID, string enumName, int enumValue)
        {
            return RecordExists(entry, 
                "REPORTENUMS", 
                new string[] { "REPORTGUID", "LANGUAGEID", "ENUMNAME", "ENUMVALUE" }, 
                new RecordIdentifier(reportID,
                    new RecordIdentifier(languageID,
                    new RecordIdentifier(enumName,enumValue))));
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier reportID, string languageID)
        {
            DeleteRecord(entry, "REPORTENUMS", new string[] { "REPORTGUID", "LANGUAGEID" }, new RecordIdentifier(reportID, languageID), "");
        }

        public virtual void Save(IConnectionManager entry, ReportEnumValue value)
        {
            var statement = entry.Connection.CreateStatement("REPORTENUMS");

            ValidateSecurity(entry, "");

            if(Exists(entry,value.ID))
            {
                Delete(entry, value.ReportID, value.LanguageID);
            }

            value.Validate();

            statement.StatementType = StatementType.Insert;

            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddKey("REPORTGUID", (Guid)value.ReportID, SqlDbType.UniqueIdentifier);
            statement.AddKey("LANGUAGEID", value.LanguageID);
            statement.AddKey("ENUMNAME", value.EnumName);
            statement.AddKey("ENUMVALUE", value.EnumValue, SqlDbType.Int);
            statement.AddField("LABEL", value.Label);
            statement.AddField("ENUMTEXT", value.Text);

            entry.Connection.ExecuteStatement(statement);

            if (value.LanguageID != "en")
            {
                // If we do not have base language then we need to see if base language exists and insert data there as well if 
                // no base language exists
                if (!Exists(entry,value.ReportID, "en",value.EnumName, value.EnumValue))
                {
                    statement = entry.Connection.CreateStatement("REPORTENUMS");

                    statement.StatementType = StatementType.Insert;

                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                    statement.AddKey("REPORTGUID", (Guid)value.ReportID, SqlDbType.UniqueIdentifier);
                    statement.AddKey("LANGUAGEID", "en");
                    statement.AddKey("ENUMNAME", value.EnumName);
                    statement.AddKey("ENUMVALUE", value.EnumValue, SqlDbType.Int);
                    statement.AddField("LABEL", value.Label);
                    statement.AddField("ENUMTEXT", value.Text);

                    entry.Connection.ExecuteStatement(statement);
                }
            }
        }
    }
}
