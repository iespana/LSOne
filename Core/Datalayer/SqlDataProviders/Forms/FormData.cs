using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Forms;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.SqlConnector.QueryHelpers;

namespace LSOne.DataLayer.SqlDataProviders.Forms
{
    public class FormData : SqlServerDataProviderBase, IFormData
    {
        private static string ResolveSort(FormSorting sort, bool backwards)
        {
            switch (sort)
            {
                case FormSorting.Type:
                    return string.Format("T.DESCRIPTION {0}", (backwards ? "DESC" : "ASC"));
                case FormSorting.Description:
                    return string.Format("F.DESCRIPTION {0}", (backwards ? "DESC" : "ASC"));
                case FormSorting.System:
                    return string.Format("F.ISSYSTEMLAYOUT {0}", (backwards ? "DESC" : "ASC"));
            }
            return "";
        }

        private static List<TableColumn> FormColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "ID" , ColumnAlias = "ID", TableAlias = "F"},
            new TableColumn {ColumnName = "TITLE" , ColumnAlias = "TITLE", IsNull = true, NullValue = "''", TableAlias = "F"},
            new TableColumn {ColumnName = "DESCRIPTION" , ColumnAlias = "DESCRIPTION", IsNull = true, NullValue = "''", TableAlias = "F"},
            new TableColumn {ColumnName = "UPPERCASE" , ColumnAlias = "UPPERCASE", IsNull = true, NullValue = "0", TableAlias = "F"},
            new TableColumn {ColumnName = "HEADERXML" , ColumnAlias = "HEADERXML", IsNull = true, NullValue = "''", TableAlias = "F"},
            new TableColumn {ColumnName = "LINESXML" , ColumnAlias = "LINESXML", IsNull = true, NullValue = "''", TableAlias = "F"},
            new TableColumn {ColumnName = "FOOTERXML" , ColumnAlias = "FOOTERXML", IsNull = true, NullValue = "''", TableAlias = "F"},
            new TableColumn {ColumnName = "PRINTASSLIP" , ColumnAlias = "PRINTASSLIP", IsNull = true, NullValue = "0", TableAlias = "F"},
            new TableColumn {ColumnName = "LINECOUNTPRPAGE" , ColumnAlias = "LINECOUNTPRPAGE", IsNull = true, NullValue = "0", TableAlias = "F"},
            new TableColumn {ColumnName = "USEWINDOWSPRINTER" , ColumnAlias = "USEWINDOWSPRINTER", IsNull = true, NullValue = "0", TableAlias = "F"},            
            new TableColumn {ColumnName = "WINDOWSPRINTERCONFIGURATIONID" , ColumnAlias = "WINDOWSPRINTERCONFIGURATIONID", IsNull = true, NullValue = "''", TableAlias = "F"},
            new TableColumn {ColumnName = "PROMPTQUESTION" , ColumnAlias = "PROMPTQUESTION", IsNull = true, NullValue = "0", TableAlias = "F"},
            new TableColumn {ColumnName = "PROMPTTEXT" , ColumnAlias = "PROMPTTEXT", IsNull = true, NullValue = "''", TableAlias = "F"},
            new TableColumn {ColumnName = "PRINTBEHAVIOUR" , ColumnAlias = "PRINTBEHAVIOUR", IsNull = true, NullValue = "0", TableAlias = "F"},
            new TableColumn {ColumnName = "DEFAULTFORMWIDTH" , ColumnAlias = "DEFAULTFORMWIDTH", IsNull = true, NullValue = "56", TableAlias = "F"},
            new TableColumn {ColumnName = "FORMTYPEID" , ColumnAlias = "FORMTYPEID", TableAlias = "F"},
            new TableColumn {ColumnName = "ISSYSTEMLAYOUT" , ColumnAlias = "ISSYSTEMLAYOUT", TableAlias = "F"},
            //Other tables
            new TableColumn {ColumnName = "SYSTEMTYPE" , ColumnAlias = "SYSTEMTYPE", IsNull = true, NullValue = "0", TableAlias = "T"},
        };

        private static Join BaseJoin = new Join { Condition = "T.ID = F.FORMTYPEID AND T.DATAAREAID = F.DATAAREAID", JoinType = "LEFT OUTER", Table = "POSFORMTYPE", TableAlias = "T" };

        private static void PopulateForm(IDataReader dr, Form form)
        {
            form.ID = (string)dr["ID"];
            form.Text = (string)dr["DESCRIPTION"];
            form.HeaderXml = (string)dr["HEADERXML"];
            form.LineXml = (string)dr["LINESXML"];
            form.FooterXml = (string)dr["FOOTERXML"];
            form.PrintAsSlip = ((byte)dr["PRINTASSLIP"] != 0);
            form.LineCountPerPage = (int)dr["LINECOUNTPRPAGE"];
            form.UseWindowsPrinter = ((byte)dr["USEWINDOWSPRINTER"] != 0);            
            form.WindowsPrinterConfigurationID = (string)dr["WINDOWSPRINTERCONFIGURATIONID"];
            form.PrintBehavior = (PrintBehaviors)(int)dr["PRINTBEHAVIOUR"];
            form.PromptQuestion = (byte)dr["PROMPTQUESTION"] != 0;
            form.PromptText = (string)dr["PROMPTTEXT"];
            form.UpperCase = (byte)dr["UPPERCASE"] != 0;
            form.SystemType = (FormSystemType)Convert.ToInt32(dr["SYSTEMTYPE"]);
            form.FormTypeID = new Guid((string) dr["FORMTYPEID"]);
            form.DefaultFormWidth = (int)dr["DEFAULTFORMWIDTH"];
            form.DefaultFormWidth = form.DefaultFormWidth == 0 ? 1 : form.DefaultFormWidth;
            form.IsSystemLayout = AsBool(dr["ISSYSTEMLAYOUT"]);
        }

        private static void PopulateFormWithNumberOfCopies(IDataReader dr, Form form)
        {
            PopulateForm(dr, form);
            form.NumberOfCopiesToPrint = (int)dr["NUMBEROFCOPIES"];
        }

        // LSRetail.SiteManager.Plugins.Forms.Dialogs.NewFormDialog
        public virtual List<DataEntity> GetList(IConnectionManager entry, string sort)
        {
            return GetList<DataEntity>(entry, "POSISFORMLAYOUT", "DESCRIPTION", "ID", sort);
        }

        // LSRetail.SiteManager.Plugins.ConfigurationWizard.ViewPages.ReceiptsPage
        public virtual List<DataEntity> GetList(IConnectionManager entry, FormSystemType systemType, string sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT F.ID AS ID,
                                    ISNULL(F.DESCRIPTION,'') as DESCRIPTION
                                    FROM POSISFORMLAYOUT F 
                                    LEFT OUTER JOIN POSFORMTYPE T ON T.ID = F.FORMTYPEID AND T.DATAAREAID = F.DATAAREAID
                                    WHERE DATAAREAID = @dataAreaId
                                    AND T.SYSTEMTYPE = @typeID
                                    ORDER BY " + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "typeID", (int)systemType);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "DESCRIPTION", "ID");
            }
        }

        public virtual Form GetProfileForm(IConnectionManager entry, RecordIdentifier profileID, FormSystemType systemType, CacheType cacheType = CacheType.CacheTypeNone)
        {
            if (cacheType != CacheType.CacheTypeNone)
            {
                var bucket = (CacheBucket)entry.Cache.GetEntityFromCache(typeof(CacheBucket), new RecordIdentifier(profileID, (int)systemType));

                if (bucket != null)
                {
                    return ((Form)bucket.BucketData);
                }
            }
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> columns = new List<TableColumn>();
                List<Join> joins = new List<Join> { BaseJoin };
                List<Condition> conditions = new List<Condition>();

                columns.AddRange(FormColumns);
                columns.Add(new TableColumn { ColumnName = "NUMBEROFCOPIES", ColumnAlias = "NUMBEROFCOPIES", TableAlias = "L" });
                joins.Add(new Join { Condition = "L.FORMLAYOUTID = F.ID AND L.DATAAREAID = F.DATAAREAID", JoinType = "LEFT OUTER", Table = "POSFORMPROFILELINES", TableAlias = "L" });
                joins.Add(new Join { Condition = "P.PROFILEID = L.PROFILEID AND P.DATAAREAID = F.DATAAREAID", JoinType = "LEFT OUTER", Table = "POSFORMPROFILE", TableAlias = "P" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "F.DATAAREAID = @dataAreaId" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.SYSTEMTYPE = @systemType" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "P.PROFILEID = @profileID" });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("POSISFORMLAYOUT", "F"), 
                                                QueryPartGenerator.InternalColumnGenerator(columns), 
                                                QueryPartGenerator.JoinGenerator(joins), 
                                                QueryPartGenerator.ConditionGenerator(conditions),
                                                string.Empty);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "systemType", (int)systemType);
                MakeParam(cmd, "profileID", (Guid)profileID, SqlDbType.UniqueIdentifier);

                var list = Execute<Form>(entry, cmd, CommandType.Text, PopulateFormWithNumberOfCopies);
                if (list.Count == 0)
                {
                    // Else try on the default profile
                    cmd.Parameters.RemoveAt("@profileID");
                    MakeParam(cmd, "profileID", FormProfile.DefaultProfileID, SqlDbType.UniqueIdentifier);

                    list = Execute<Form>(entry, cmd, CommandType.Text, PopulateForm);
                }
                if (list.Count > 0)
                {
                    if (cacheType != CacheType.CacheTypeNone)
                    {
                        CacheBucket bucket = new CacheBucket(new RecordIdentifier(profileID, (int)systemType), "", list[0]);

                        entry.Cache.AddEntityToCache(bucket.ID, bucket, cacheType);
                    }
                    return list[0];
                }
            }
            
            return null;
        }

        
        // LSRetail.SiteManager.Plugins.Forms.Views.FormsView
        public virtual List<Form> GetProfileForms(IConnectionManager entry, RecordIdentifier profileID, FormSorting sortBy, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Join> joins = new List<Join> { BaseJoin };
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "F.DATAAREAID = @dataAreaId" });

                if (profileID != RecordIdentifier.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "PROFILEID = @profileId" });
                    MakeParam(cmd, "profileId", profileID);
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("POSISFORMLAYOUT", "F"),
                                                QueryPartGenerator.InternalColumnGenerator(FormColumns),
                                                QueryPartGenerator.JoinGenerator(joins),
                                                QueryPartGenerator.ConditionGenerator(conditions),
                                                "ORDER BY " + ResolveSort(sortBy, sortBackwards));

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<Form>(entry, cmd, CommandType.Text, PopulateForm);
            }
        }

        public virtual List<Form> GetSystemProfileForms(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Join> joins = new List<Join> { BaseJoin };
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "F.DATAAREAID = @dataAreaId" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "F.ISSYSTEMLAYOUT = 1" });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("POSISFORMLAYOUT", "F"),
                                                QueryPartGenerator.InternalColumnGenerator(FormColumns),
                                                QueryPartGenerator.JoinGenerator(joins),
                                                QueryPartGenerator.ConditionGenerator(conditions),
                                                string.Empty);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                return Execute<Form>(entry, cmd, CommandType.Text, PopulateForm);
            }
        }

        // LSRetail.SiteManager.Plugins.ConfigurationWizard.Views.TemplatesView
        public virtual List<Form> GetLists(IConnectionManager entry, FormSorting sortBy, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Join> joins = new List<Join> { BaseJoin };
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "F.DATAAREAID = @dataAreaId" });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("POSISFORMLAYOUT", "F"),
                                                QueryPartGenerator.InternalColumnGenerator(FormColumns),
                                                QueryPartGenerator.JoinGenerator(joins),
                                                QueryPartGenerator.ConditionGenerator(conditions),
                                                "ORDER BY " + ResolveSort(sortBy, sortBackwards));

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<Form>(entry, cmd, CommandType.Text, PopulateForm);
            }
        }

        public virtual List<Form> GetFormsOfType(IConnectionManager entry, RecordIdentifier formTypeID, FormSorting sortBy, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Join> joins = new List<Join> { BaseJoin };
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "F.DATAAREAID = @dataAreaId" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "F.FORMTYPEID = @formTypeId" });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("POSISFORMLAYOUT", "F"),
                                                QueryPartGenerator.InternalColumnGenerator(FormColumns),
                                                QueryPartGenerator.JoinGenerator(joins),
                                                QueryPartGenerator.ConditionGenerator(conditions),
                                                "ORDER BY " + ResolveSort(sortBy, sortBackwards));

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "formTypeId", formTypeID);

                return Execute<Form>(entry, cmd, CommandType.Text, PopulateForm);
            }
        }

        public virtual List<Form> SearchForms(IConnectionManager entry, string description, bool descriptionBeginsWith, bool? isSystemForm, FormSorting sortBy, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Join> joins = new List<Join> { BaseJoin };
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "F.DATAAREAID = @dataAreaId" });

                if (!string.IsNullOrEmpty(description))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(F.DESCRIPTION LIKE @description OR T.DESCRIPTION LIKE @description)" });
                    MakeParam(cmd, "description", PreProcessSearchText(description, true, descriptionBeginsWith));
                }

                if (isSystemForm.HasValue)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "F.ISSYSTEMLAYOUT = @isSystemLayout" });
                    MakeParam(cmd, "isSystemLayout", isSystemForm.Value, SqlDbType.Bit);
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("POSISFORMLAYOUT", "F"),
                                                QueryPartGenerator.InternalColumnGenerator(FormColumns),
                                                QueryPartGenerator.JoinGenerator(joins),
                                                QueryPartGenerator.ConditionGenerator(conditions),
                                                "ORDER BY " + ResolveSort(sortBy, sortBackwards));

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<Form>(entry, cmd, CommandType.Text, PopulateForm);
            }
        }

        /*
        public virtual int GetHighestUsedID(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select ISNULL(MAX(ID),0) as MaxID from POSISFORMLAYOUT";

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        return (int) dr["MaxID"];
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception)
                {
                    return 0;
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
            }
        }*/

        // A few places
        public virtual Form Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Join> joins = new List<Join> { BaseJoin };
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "F.DATAAREAID = @dataAreaId" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "F.ID = @id" });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("POSISFORMLAYOUT", "F"),
                                                QueryPartGenerator.InternalColumnGenerator(FormColumns),
                                                QueryPartGenerator.JoinGenerator(joins),
                                                QueryPartGenerator.ConditionGenerator(conditions),
                                                string.Empty);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string) id);

                return Get<Form>(entry, cmd, id, PopulateForm, cache, UsageIntentEnum.Normal);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<Form>(entry, "POSISFORMLAYOUT", "ID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<Form>(entry, "POSISFORMLAYOUT", "ID", id, BusinessObjects.Permission.FormsEdit);
        }

        public virtual void Save(IConnectionManager entry, Form form)
        {
            var statement = new SqlServerStatement("POSISFORMLAYOUT");

            ValidateSecurity(entry, BusinessObjects.Permission.FormsEdit);
            form.Validate();

            bool isNew = false;
            if (form.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                form.ID = DataProviderFactory.Instance.GenerateNumber<IFormData, Form>(entry);
            }

            if (isNew || !Exists(entry, form.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)form.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)form.ID);
            }

            statement.AddField("TITLE", form.Text);
            statement.AddField("DESCRIPTION", form.Text);
            statement.AddField("HEADERXML", form.HeaderXml);
            statement.AddField("LINESXML", form.LineXml);
            statement.AddField("FOOTERXML", form.FooterXml);
            statement.AddField("PRINTASSLIP", form.PrintAsSlip ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("LINECOUNTPRPAGE", form.LineCountPerPage, SqlDbType.Int);
            statement.AddField("USEWINDOWSPRINTER", form.UseWindowsPrinter ? 1 : 0, SqlDbType.TinyInt);            
            statement.AddField("WINDOWSPRINTERCONFIGURATIONID", (string)form.WindowsPrinterConfigurationID);
            statement.AddField("PRINTBEHAVIOUR", (int)form.PrintBehavior, SqlDbType.Int);
            statement.AddField("PROMPTQUESTION", form.PromptQuestion, SqlDbType.TinyInt);
            statement.AddField("PROMPTTEXT", form.PromptText, SqlDbType.Text);
            statement.AddField("DEFAULTFORMWIDTH", form.DefaultFormWidth, SqlDbType.Int);
            statement.AddField("FORMTYPEID", (string)form.FormTypeID);

            Save(entry, form, statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "POSISFORMLAYOUT"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSISFORMLAYOUT", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
