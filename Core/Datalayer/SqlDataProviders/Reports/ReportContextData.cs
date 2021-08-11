using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Reports;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Reports
{
    public class ReportContextData : SqlServerDataProviderBase, IReportContextData
    {
        private static void PopulateContext(IDataReader dr, ReportContext reportContext)
        {
            reportContext.ID = (Guid)dr["CONTEXTGUID"];
            reportContext.ReportID = (Guid)dr["REPORTGUID"];
            reportContext.Text = (string)dr["CONTEXT"];
            reportContext.Active = (bool)dr["ACTIVE"];
        }

        public virtual List<ReportContext> GetList(IConnectionManager entry, RecordIdentifier reportGUID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT CONTEXTGUID
                                      ,REPORTGUID
                                      ,CONTEXT
                                      ,ACTIVE
                                  FROM REPORTCONTEXTS 
                                  WHERE REPORTGUID = @reportGUID";

                MakeParam(cmd, "reportGUID", (Guid)reportGUID, SqlDbType.UniqueIdentifier);

                return Execute<ReportContext>(entry, cmd, CommandType.Text, PopulateContext);
            }
        }

        public virtual void DeleteAllForReport(IConnectionManager entry, RecordIdentifier reportID,string languageID)
        {
            DeleteRecord(entry, "REPORTCONTEXTS", "REPORTGUID", reportID, "");
            DeleteRecord(entry, "REPORTENUMS", new string[]{"REPORTGUID","LANGUAGEID"}, new RecordIdentifier(reportID,languageID), "");
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "REPORTCONTEXTS", "CONTEXTGUID", id);
        }

        public virtual void Save(IConnectionManager entry, ReportContext context)
        {
            var statement = entry.Connection.CreateStatement("REPORTCONTEXTS");

            ValidateSecurity(entry, "");

            if (context.ID == RecordIdentifier.Empty)
            {
                context.ID = Guid.NewGuid();
            }

            context.Validate();

            if (!Exists(entry, context.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("CONTEXTGUID", (Guid)context.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("CONTEXTGUID", (Guid)context.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("REPORTGUID", (Guid)context.ReportID, SqlDbType.UniqueIdentifier);
            statement.AddField("CONTEXT", context.Text);
            statement.AddField("ACTIVE", context.Active, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
            
            // At last we clear the button cache
            Providers.ReportData.InvalidateReportContextCache(entry);
        }
    }
}
