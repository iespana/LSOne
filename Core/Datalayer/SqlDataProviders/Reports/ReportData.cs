using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.DataProviders.Reports;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using Permission = LSOne.DataLayer.BusinessObjects.Permission;

namespace LSOne.DataLayer.SqlDataProviders.Reports
{
    

    public class ReportData : SqlServerDataProviderBase, IReportData
    {
        private static Dictionary<ColumnPopulation, List<TableColumn>> selectionColumns = new Dictionary<ColumnPopulation, List<TableColumn>>();
        private static object threadLock = new object();

        private static Dictionary<ColumnPopulation, List<TableColumn>> SelectionColumns
        {
            get
            {
                lock (threadLock)
                {
                    if (selectionColumns.Count == 0)
                    {
                        selectionColumns.Add(ColumnPopulation.Simple, new List<TableColumn>
                        {
                            new TableColumn {ColumnName = "REPORTGUID", TableAlias = "R"},
                            new TableColumn {ColumnName = "COALESCE(LANG2.NAME, LANG1.NAME, R.NAME)", ColumnAlias = "NAME"},
                            new TableColumn {ColumnName = "COALESCE(LANG2.DESCRIPTION, LANG1.DESCRIPTION, R.DESCRIPTION)", ColumnAlias = "DESCRIPTION"},
                            new TableColumn {ColumnName = "REPORTGUID", TableAlias = "R"},
                            new TableColumn {ColumnName = "ISSITESERVICEREPORT", TableAlias = "R"},
                            new TableColumn {ColumnName = "SYSTEMREPORT", TableAlias = "R"},
                            new TableColumn {ColumnName = "REPORTCATEGORY", TableAlias = "R"}
                        });

                        selectionColumns.Add(ColumnPopulation.SiteManager, new List<TableColumn>
                        {
                            new TableColumn {ColumnName = "REPORTGUID", TableAlias = "R"},
                            new TableColumn {ColumnName = "COALESCE(LANG2.NAME, LANG1.NAME, R.NAME)", ColumnAlias = "NAME"},
                            new TableColumn {ColumnName = "COALESCE(LANG2.DESCRIPTION, LANG1.DESCRIPTION, R.DESCRIPTION)", ColumnAlias = "DESCRIPTION"},
                            new TableColumn {ColumnName = "REPORTGUID", TableAlias = "R"},
                            new TableColumn {ColumnName = "ISSITESERVICEREPORT", TableAlias = "R"},
                            new TableColumn {ColumnName = "SYSTEMREPORT", TableAlias = "R"},
                            new TableColumn {ColumnName = "SQLBLOB", TableAlias = "R"},
                            new TableColumn {ColumnName = "SQLBLOBINSTALLED", TableAlias = "R"},
                            new TableColumn {ColumnName = "COALESCE(LANG2.REPORTBLOB, LANG1.REPORTBLOB, R.REPORTBLOB)", ColumnAlias = "REPORTBLOB"},
                            new TableColumn {ColumnName = "COALESCE(LANG2.LANGUAGEID, LANG1.LANGUAGEID, 'en')", ColumnAlias = "LANGUAGEID"},
                            new TableColumn {ColumnName = "REPORTCATEGORY", TableAlias = "R"}
                        });
                    }

                    return selectionColumns;
                }
            }
        }

        private static List<Join> Joins = new List<Join>
        {
            new Join {JoinType = "LEFT OUTER", Table = "REPORTLOCALIZATION", TableAlias = "LANG1", Condition = "R.REPORTGUID = LANG1.REPORTGUID AND R.DATAAREAID = LANG1.DATAAREAID AND LANG1.LANGUAGEID = @baseLanguage"},
            new Join {JoinType = "LEFT OUTER", Table = "REPORTLOCALIZATION", TableAlias = "LANG2", Condition = "R.REPORTGUID = LANG2.REPORTGUID AND R.DATAAREAID = LANG2.DATAAREAID AND LANG2.LANGUAGEID = @language"}
        };

        private static void PopulateListItem(IDataReader dr, ReportListItem item)
        {
            item.ID = (Guid)dr["REPORTGUID"];
            item.Text = (string)dr["NAME"];
            item.Description = (string)dr["DESCRIPTION"];
            item.SystemReport = (bool)dr["SYSTEMREPORT"];
            item.SiteServiceReport = (bool)dr["ISSITESERVICEREPORT"];
            item.ReportCategory = (ReportCategory)((int)dr["REPORTCATEGORY"]);
        }

        private static void PopulateReport(IDataReader dr, Report report)
        {
            report.ID = (Guid)dr["REPORTGUID"];
            report.Text = (string)dr["NAME"];
            report.Description = (string)dr["DESCRIPTION"];
            report.LanguageID = (string)dr["LANGUAGEID"];
            report.ReportData = (byte[])dr["REPORTBLOB"];
            report.SqlDataInstalled = (bool)dr["SQLBLOBINSTALLED"];
            report.SystemReport = (bool)dr["SYSTEMREPORT"];
            report.SiteServiceReport = (bool)dr["ISSITESERVICEREPORT"];
            report.ReportCategory = (ReportCategory)((int)dr["REPORTCATEGORY"]);
            report.SqlData = Encoding.UTF8.GetString((byte[])dr["SQLBLOB"]);
        }

        public virtual Report Get(IConnectionManager entry, RecordIdentifier id)
        {
            List<Report> result;

            ValidateSecurity(entry);
            
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition> { new Condition { Operator = "AND", ConditionValue = "R.REPORTGUID = @reportGuid" } };

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("REPORTS", "R"),
                                    QueryPartGenerator.InternalColumnGenerator(SelectionColumns[ColumnPopulation.SiteManager]),
                                    QueryPartGenerator.JoinGenerator(Joins),
                                    QueryPartGenerator.ConditionGenerator(conditions),
                                    string.Empty);

                MakeParam(cmd, "baseLanguage", System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
                MakeParam(cmd, "language", System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
                MakeParam(cmd, "reportGuid", (Guid)id);

                result = Execute<Report>(entry, cmd, CommandType.Text, PopulateReport);
            }

            return result.Count > 0 ? result[0] : null;
        }

        public virtual List<ReportListItem> GetList(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("REPORTS", "R"),
                                    QueryPartGenerator.InternalColumnGenerator(SelectionColumns[ColumnPopulation.Simple]),
                                    QueryPartGenerator.JoinGenerator(Joins),
                                    string.Empty,
                                    "ORDER BY R.NAME");

                MakeParam(cmd, "baseLanguage", System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
                MakeParam(cmd, "language", System.Threading.Thread.CurrentThread.CurrentUICulture.Name);

                return Execute<ReportListItem>(entry, cmd, CommandType.Text, PopulateListItem);
            }
        }

        public virtual void InsertSQLProcedures(IConnectionManager entry, Report report)
        {
            string sql = "";
            string[] lines = report.SqlData.Split(new string[] { System.Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].ToUpperInvariant() == "GO")
                {
                    if (sql.Trim() != "")
                    {
                        using (var cmd = entry.Connection.CreateCommand())
                        {
                            cmd.CommandText = sql.Trim();

                            entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
                        }
                    }

                    sql = "";
                }
                else
                {
                    sql += lines[i] + Environment.NewLine;
                }
            }

            if (sql.Trim() != "")
            {
                using (var cmd = entry.Connection.CreateCommand())
                {
                    cmd.CommandText = sql.Trim();

                    entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
                }
            }

            report.SqlDataInstalled = true;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"UPDATE REPORTS SET SQLBLOBINSTALLED = 1 WHERE DATAAREAID = @DATAAREAID AND REPORTGUID = @REPORTGUID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "REPORTGUID", (Guid)report.ID);

                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }
        }

        public virtual List<ReportListItem> GetReportContextItems(IConnectionManager entry, ReportContextEnum context, CacheType cacheType = CacheType.CacheTypeNone)
        {
            CacheBucket bucket;
            List<ReportListItem> result;

            if (cacheType != CacheType.CacheTypeNone)
            {
                bucket = (CacheBucket)entry.Cache.GetEntityFromCache(typeof(CacheBucket), "ReportList" + Enum.GetName(typeof(ReportContextEnum), context));

                if (bucket != null)
                {
                    return (List<ReportListItem>)bucket.BucketData;
                }
            }

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Join> joins = new List<Join>
                {
                    new Join { JoinType = "", Table = "REPORTCONTEXTS", TableAlias = "C", Condition = "R.REPORTGUID = C.REPORTGUID AND R.DATAAREAID = C.DATAAREAID" }
                };

                joins.AddRange(Joins);

                List<Condition> conditions = new List<Condition>
                {
                    new Condition {Operator = "AND", ConditionValue = $"C.CONTEXT = '{Enum.GetName(typeof(ReportContextEnum), context)}'"},
                    new Condition {Operator = "AND", ConditionValue = "C.ACTIVE = 1"},
                    new Condition {Operator = "AND", ConditionValue = "C.DATAAREAID = @DATAAREAID"},
                };

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("REPORTS", "R"),
                                    QueryPartGenerator.InternalColumnGenerator(SelectionColumns[ColumnPopulation.Simple]),
                                    QueryPartGenerator.JoinGenerator(joins),
                                    QueryPartGenerator.ConditionGenerator(conditions),
                                    "ORDER BY NAME");

                MakeParam(cmd, "baseLanguage", System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
                MakeParam(cmd, "language", System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                result = Execute<ReportListItem>(entry, cmd, CommandType.Text, PopulateListItem);

                if (cacheType != CacheType.CacheTypeNone)
                {
                    bucket = new CacheBucket("ReportList" + Enum.GetName(typeof(ReportContextEnum), context), "", result);

                    entry.Cache.AddEntityToCache(bucket.ID, bucket, cacheType);
                }

                return result;
            }
        }

        public virtual void InvalidateReportContextCache(IConnectionManager entry)
        {
            entry.Cache.DeleteEntityFromCache(typeof(CacheBucket), "ReportListButton");
            entry.Cache.DeleteEntityFromCache(typeof(CacheBucket), "ReportListReport");
            entry.Cache.DeleteEntityFromCache(typeof(CacheBucket), "ReportListStore");
            entry.Cache.DeleteEntityFromCache(typeof(CacheBucket), "ReportListCustomer");
            entry.Cache.DeleteEntityFromCache(typeof(CacheBucket), "ReportListVendor");
            entry.Cache.DeleteEntityFromCache(typeof(CacheBucket), "ReportListTerminal");
            entry.Cache.DeleteEntityFromCache(typeof(CacheBucket), "ReportListItem");
        }

        
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "REPORTS", "REPORTGUID", id, Permission.ManageReports);
            DeleteRecord(entry, "REPORTLOCALIZATION", "REPORTGUID", id, Permission.ManageReports);
            DeleteRecord(entry, "REPORTCONTEXTS", "REPORTGUID", id, Permission.ManageReports);
            DeleteRecord(entry, "REPORTENUMS", "REPORTGUID", id, Permission.ManageReports);

            // At last we clear the button cache
            InvalidateReportContextCache(entry);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "REPORTS", "REPORTGUID", id);
        }

        private static bool ExistsWithLanguage(IConnectionManager entry, RecordIdentifier id, string languageID)
        {
            return RecordExists(entry, "REPORTLOCALIZATION", new string[]{"REPORTGUID","LANGUAGEID"}, new RecordIdentifier(id,languageID));
        }

        private static void AddToBaseLanguage(IConnectionManager entry, Report report)
        {
            var statement = entry.Connection.CreateStatement("REPORTS");

            statement.StatementType = StatementType.Insert;

            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddKey("REPORTGUID", (Guid)report.ID, SqlDbType.UniqueIdentifier);

            statement.AddField("NAME", report.Text);
            statement.AddField("DESCRIPTION", report.Description);

            UTF8Encoding encoding = new UTF8Encoding();

            byte[] bytes = encoding.GetBytes(report.SqlData);

            statement.AddField("SQLBLOB", bytes, SqlDbType.VarBinary);
            statement.AddField("REPORTBLOB", report.ReportData, SqlDbType.VarBinary);
            statement.AddField("SQLBLOBINSTALLED", report.SqlDataInstalled, SqlDbType.Bit);
            statement.AddField("ISSITESERVICEREPORT", report.SiteServiceReport, SqlDbType.Bit);
            statement.AddField("REPORTCATEGORY", (int)report.ReportCategory, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        private static void UpdateBaseLanguage(IConnectionManager entry, Report report)
        {
            var statement = entry.Connection.CreateStatement("REPORTS");

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("REPORTGUID", (Guid)report.ID, SqlDbType.UniqueIdentifier);

            statement.AddField("NAME", report.Text);
            statement.AddField("DESCRIPTION", report.Description);

            UTF8Encoding encoding = new UTF8Encoding();

            statement.AddField("SQLBLOB", encoding.GetBytes(report.SqlData), SqlDbType.VarBinary);
            statement.AddField("REPORTBLOB", report.ReportData, SqlDbType.VarBinary);
            statement.AddField("SQLBLOBINSTALLED", report.SqlDataInstalled, SqlDbType.Bit);
            statement.AddField("ISSITESERVICEREPORT", report.SiteServiceReport, SqlDbType.Bit);
            statement.AddField("REPORTCATEGORY", (int)report.ReportCategory, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void Save(IConnectionManager entry, Report report)
        {
            ValidateSecurity(entry, Permission.ManageReports);

            bool baseLanguageReportExists = Exists(entry, report.ID);

            if (report.LanguageID != "en" && report.LanguageID != "")
            {
                if (!baseLanguageReportExists)
                {
                    AddToBaseLanguage(entry, report);
                }

                var statement = entry.Connection.CreateStatement("REPORTLOCALIZATION");

                if (ExistsWithLanguage(entry, report.ID, report.LanguageID))
                {
                    statement.StatementType = StatementType.Update;

                    statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                    statement.AddCondition("REPORTGUID", (Guid)report.ID, SqlDbType.UniqueIdentifier);
                    statement.AddCondition("LANGUAGEID", report.LanguageID);
                }
                else
                {
                    statement.StatementType = StatementType.Insert;

                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                    statement.AddKey("REPORTGUID", (Guid)report.ID, SqlDbType.UniqueIdentifier);
                    statement.AddKey("LANGUAGEID", report.LanguageID);
                }

                statement.AddField("NAME", report.Text);
                statement.AddField("DESCRIPTION", report.Description);
                statement.AddField("REPORTBLOB", report.ReportData, SqlDbType.VarBinary);
                
                entry.Connection.ExecuteStatement(statement);

                if (report.SqlData.Trim() != "")
                {
                    // We need to update the SQL in the base language
                    statement = new SqlServerStatement("REPORTS");
                    statement.StatementType = StatementType.Update;

                    statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                    statement.AddCondition("REPORTGUID", (Guid)report.ID, SqlDbType.UniqueIdentifier);

                    UTF8Encoding encoding = new UTF8Encoding();

                    statement.AddField("SQLBLOB", encoding.GetBytes(report.SqlData), SqlDbType.VarBinary);
                    statement.AddField("SQLBLOBINSTALLED", false, SqlDbType.Bit);
                    statement.AddField("REPORTCATEGORY", (int)report.ReportCategory, SqlDbType.Int);

                    entry.Connection.ExecuteStatement(statement);
                }
            }
            else
            {
                if (baseLanguageReportExists)
                {
                    UpdateBaseLanguage(entry,report);
                }
                else
                {
                    AddToBaseLanguage(entry, report);
                }
            }

            // At last we clear the button cache
            InvalidateReportContextCache(entry);
        }
    }
}
