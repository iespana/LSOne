using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ConfigurationWizard;
using LSOne.DataLayer.DataProviders.Sequences;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ConfigurationWizard
{
    /// <summary>
    /// Data prover class for WizardTemplateView.
    /// </summary>
    public class WizardTemplateViewData : SqlServerDataProviderBase, IWizardTemplateViewData
    {
        private static string BaseSql
        {
            get
            {
                return @"SELECT 
                        ISNULL(ID, '') as ID,
                        ISNULL(DESCRIPTION, '') as DESCRIPTION,
                        ISNULL(STATUS, 0) as STATUS,
                        ISNULL(DATAAREAID, '') as DATAAREAID,
                        ISNULL(STOREID, '') as STOREID,
                        ISNULL(TERMINALID, '') as TERMINALID,
                        IMAGE,
                        LASTEXPORTDATE,
                        ISNULL(NAME, 1) as NAME,
                        ISNULL(ADDRESS, 0) as ADDRESS,
                        ISNULL(PRICECALCULATION, 0) as PRICECALCULATION,
                        ISNULL(CHOOSENAMEADDRESS, 0) as CHOOSENAMEADDRESS,
                        ISNULL(CHOOSEPRICECALCULATION, 0) as CHOOSEPRICECALCULATION,
                        ISNULL(VISUALPROFILEID, '') as VISUALPROFILEID,
                        ISNULL(FUNCTIONALITYPROFILEID, '') as FUNCTIONALITYPROFILEID,
                        ISNULL(STORESERVERPROFILEID, '') as STORESERVERPROFILEID,
                        ISNULL(DEFAULTCURRENCY, '') as DEFAULTCURRENCY,
                        ISNULL(CHOOSEPAYMENTS, 0) as CHOOSEPAYMENTS,
                        ISNULL(HARDWAREPROFILEID, '') as HARDWAREPROFILEID,
                        ISNULL(CHOOSEPERIPHERALS, 0) as CHOOSEPERIPHERALS,
                        ISNULL(CHOOSETILLLAYOUTS, 0) as CHOOSETILLLAYOUTS,
                        ISNULL(CHOOSERECEIPTS, 0) as CHOOSERECEIPTS,
                        ISNULL(CHOOSERETAILGROUPS, 0) as CHOOSERETAILGROUPS,
                        ISNULL(CHOOSEPERMISSIONGROUP, 0) as CHOOSEPERMISSIONGROUP 
                        FROM WIZARDTEMPLATE";
            }
        }

        /// <summary>
        /// populates data for template.
        /// </summary>
        /// <param name="entry">entry to database</param>
        /// <param name="dr">data reader</param>
        /// <param name="templateView">object of TemplateView entity</param>
        /// <param name="obj">parameters</param>
        private static void PopulateTemplateView(IConnectionManager entry, IDataReader dr, WizardTemplateView templateView, object obj)
        {
            templateView.ID = Convert.ToString(dr["ID"]);
            templateView.Text = Convert.ToString(dr["DESCRIPTION"]);
            templateView.Status = Convert.ToInt32(dr["STATUS"]);
            templateView.DataAreaID = Convert.ToString(dr["DATAAREAID"]);
            templateView.StoreId = Convert.ToString(dr["STOREID"]);
            templateView.TerminalId = Convert.ToString(dr["TERMINALID"]);
            templateView.TemplateImage = (dr["IMAGE"] == DBNull.Value) ? null : (byte[])dr["IMAGE"];
            templateView.LastExportDate = dr["LASTEXPORTDATE"] == DBNull.Value ? DateTime.Now : (DateTime)dr["LASTEXPORTDATE"];
            templateView.Name = Convert.ToInt32(dr["NAME"]);
            templateView.Address = Convert.ToInt32(dr["ADDRESS"]);
            templateView.PriceCalculation = Convert.ToInt32(dr["PRICECALCULATION"]);
            templateView.ChooseNameAddress = (byte)dr["CHOOSENAMEADDRESS"];
            templateView.ChoosePriceCalculation = (byte)dr["CHOOSEPRICECALCULATION"];
            templateView.VisualProfileID = Convert.ToString(dr["VISUALPROFILEID"]);
            templateView.FunctionalityProfileID = Convert.ToString(dr["FUNCTIONALITYPROFILEID"]);
            templateView.StoreServerProfileID = Convert.ToString(dr["STORESERVERPROFILEID"]);
            templateView.DefaultCurrency = Convert.ToString(dr["DEFAULTCURRENCY"]);
            templateView.ChoosePayments = (byte)dr["CHOOSEPAYMENTS"];
            templateView.HardwareProfileID = Convert.ToString(dr["HARDWAREPROFILEID"]);
            templateView.ChoosePeripherals = (byte)dr["CHOOSEPERIPHERALS"];
            templateView.ChooseTillLayouts = (byte)dr["CHOOSETILLLAYOUTS"];
            templateView.ChooseReceipts = (byte)dr["CHOOSERECEIPTS"];
            templateView.ChooseRetailGroups = (byte)dr["CHOOSERETAILGROUPS"];
            templateView.ChoosePermissionGroup = (byte)dr["CHOOSEPERMISSIONGROUP"];
        }

        /// <summary>
        /// Get all templates from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sort">OrderField</param>
        /// <returns>Template List</returns>
        public virtual List<WizardTemplateView> GetTemplateList(IConnectionManager entry, string sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {                
                cmd.CommandText = BaseSql;
                cmd.CommandText += " ORDER BY " + sort;
                return Execute<WizardTemplateView>(entry, cmd, CommandType.Text, null, PopulateTemplateView);
            }
        }

        /// <summary>
        /// Get all record of selected template from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>Template</returns>
        public virtual WizardTemplateView Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " WHERE ID = @TEMPLATEID and DATAAREAID = @DATAAREAID ORDER BY ID";

                MakeParam(cmd, "TEMPLATEID", (string)id);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                List<WizardTemplateView> result = Execute<WizardTemplateView>(entry, cmd, CommandType.Text, null, PopulateTemplateView);

                return (result.Count > 0) ? result[0] : null;
            }            
        }

        /// <summary>
        /// Chek if template exist into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>boolean result</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "WIZARDTEMPLATE", "ID", id);
        }

        /// <summary>
        /// Delete a record from databse of a template.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "WIZARDTEMPLATE", "ID", id, BusinessObjects.Permission.EditPOSLayout);
        }

        /// <summary>
        /// Save template into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">a oblect which contains record of template</param>
        public virtual void Save(IConnectionManager entry, WizardTemplateView templateView)
        {
            bool isNew = false;

            var statement = new SqlServerStatement("WIZARDTEMPLATE");


            if (templateView.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                templateView.ID = DataProviderFactory.Instance.
                    Get<INumberSequenceData, NumberSequence>()
                    .GenerateNumberFromSequence(entry, new WizardTemplateViewData()); 
            }

            if (isNew || !Exists(entry, templateView.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("ID", (string)templateView.ID);                
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)templateView.ID);    
            }
            statement.AddField("STATUS", templateView.Status, SqlDbType.Int);
            statement.AddField("DESCRIPTION", templateView.Text);
            statement.AddField("STOREID", (string)templateView.StoreId, SqlDbType.NVarChar);
            statement.AddField("TERMINALID", (string)templateView.TerminalId, SqlDbType.NVarChar);
            if (templateView.TemplateImage != null)
            {
                statement.AddField("IMAGE", templateView.TemplateImage, SqlDbType.Image);
            }
            statement.AddField("LASTEXPORTDATE", templateView.LastExportDate, SqlDbType.DateTime);
            if (templateView.Name != null)
            {
                statement.AddField("NAME", templateView.Name, SqlDbType.Int);
            }

            if (templateView.Address != null)
            {
                statement.AddField("ADDRESS", templateView.Address, SqlDbType.Int);
            }

            if (templateView.PriceCalculation != null)
            {
                statement.AddField("PRICECALCULATION", templateView.PriceCalculation, SqlDbType.Int);
            }
            statement.AddField("CHOOSENAMEADDRESS", templateView.ChooseNameAddress, SqlDbType.TinyInt);
            statement.AddField("CHOOSEPRICECALCULATION", templateView.ChoosePriceCalculation, SqlDbType.TinyInt);
            statement.AddField("VISUALPROFILEID", (string)templateView.VisualProfileID, SqlDbType.NVarChar);
            statement.AddField("FUNCTIONALITYPROFILEID", (string)templateView.FunctionalityProfileID, SqlDbType.NVarChar);
            statement.AddField("STORESERVERPROFILEID", (string)templateView.StoreServerProfileID, SqlDbType.NVarChar);
            statement.AddField("DEFAULTCURRENCY", templateView.DefaultCurrency, SqlDbType.NVarChar);
            statement.AddField("CHOOSEPAYMENTS", templateView.ChoosePayments, SqlDbType.TinyInt);
            statement.AddField("HARDWAREPROFILEID", (string)templateView.HardwareProfileID, SqlDbType.NVarChar);
            statement.AddField("CHOOSEPERIPHERALS", templateView.ChoosePeripherals, SqlDbType.TinyInt);
            statement.AddField("CHOOSETILLLAYOUTS", templateView.ChooseTillLayouts, SqlDbType.TinyInt);
            statement.AddField("CHOOSERECEIPTS", templateView.ChooseReceipts, SqlDbType.TinyInt);
            statement.AddField("CHOOSERETAILGROUPS", templateView.ChooseRetailGroups, SqlDbType.TinyInt);
            statement.AddField("CHOOSEPERMISSIONGROUP", templateView.ChoosePermissionGroup, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Save TerminalId and StoreId into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of StoreId and terminalId</param>
        public virtual void SaveBusinessTemplate(IConnectionManager entry, WizardTemplateView templateView)
        {
            bool isNew = false;

            var statement = new SqlServerStatement("WIZARDTEMPLATE");

            ValidateSecurity(entry, BusinessObjects.Permission.ConfigurationWizardEdit);
            
            if (isNew || !Exists(entry, templateView.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("ID", (string)templateView.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)templateView.ID);
            }            
            statement.AddField("STOREID", (string)templateView.StoreId, SqlDbType.NVarChar);
            statement.AddField("TERMINALID", (string)templateView.TerminalId, SqlDbType.NVarChar);
            if (templateView.TemplateImage != null)
            {
                statement.AddField("IMAGE", templateView.TemplateImage, SqlDbType.Image);
            }
            statement.AddField("STATUS", templateView.Status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Save ProfileIds into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of ProfileIds</param>
        public virtual void SaveProfiles(IConnectionManager entry, WizardTemplateView templateView)
        {
            bool isNew = false;

            var statement = new SqlServerStatement("WIZARDTEMPLATE");

            ValidateSecurity(entry, BusinessObjects.Permission.VisualProfileEdit);

            if (templateView.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                templateView.ID = DataProviderFactory.Instance.GenerateNumber<IWizardTemplateViewData,WizardTemplateView>(entry);
            }

            if (isNew || !Exists(entry, templateView.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("ID", (string)templateView.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)templateView.ID);
            }

            statement.AddField("VISUALPROFILEID", templateView.VisualProfileID.StringValue, SqlDbType.NVarChar);
            statement.AddField("FUNCTIONALITYPROFILEID", templateView.FunctionalityProfileID.StringValue, SqlDbType.NVarChar);
            statement.AddField("STORESERVERPROFILEID", templateView.StoreServerProfileID.StringValue, SqlDbType.NVarChar);
            statement.AddField("STATUS", templateView.Status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Save CurrencyId into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of CurrencyId</param>
        public virtual void SaveDefaultCurrency(IConnectionManager entry, WizardTemplateView templateView)
        {
            bool isNew = false;

            var statement = new SqlServerStatement("WIZARDTEMPLATE");

            ValidateSecurity(entry, BusinessObjects.Permission.VisualProfileEdit);


            if (templateView.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                templateView.ID = DataProviderFactory.Instance.GenerateNumber<IWizardTemplateViewData,WizardTemplateView>(entry); 
            }

            if (isNew || !Exists(entry, templateView.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("ID", (string)templateView.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)templateView.ID);
            }

            statement.AddField("DEFAULTCURRENCY", templateView.DefaultCurrency, SqlDbType.NVarChar);
            statement.AddField("CHOOSEPAYMENTS", templateView.ChoosePayments, SqlDbType.TinyInt);
            statement.AddField("STATUS", templateView.Status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Save HardwareProfileId into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of HardwareProfileId</param>
        public virtual void SaveHardwareProfile(IConnectionManager entry, WizardTemplateView templateView)
        {
            bool isNew = false;

            var statement = new SqlServerStatement("WIZARDTEMPLATE");

            ValidateSecurity(entry, BusinessObjects.Permission.VisualProfileEdit);


            if (templateView.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                templateView.ID = DataProviderFactory.Instance.GenerateNumber<IWizardTemplateViewData,WizardTemplateView>(entry); 
            }

            if (isNew || !Exists(entry, templateView.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("ID", (string)templateView.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)templateView.ID);
            }

            statement.AddField("HARDWAREPROFILEID", templateView.HardwareProfileID.StringValue, SqlDbType.NVarChar);
            statement.AddField("CHOOSEPERIPHERALS", templateView.ChoosePeripherals, SqlDbType.TinyInt);
            statement.AddField("STATUS", templateView.Status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Save Permission for RetailGroup into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of RetailPermission</param>
        public virtual void SaveRetailPermission(IConnectionManager entry, WizardTemplateView templateView)
        {
            bool isNew = false;

            var statement = new SqlServerStatement("WIZARDTEMPLATE");

            ValidateSecurity(entry, BusinessObjects.Permission.VisualProfileEdit);


            if (templateView.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                templateView.ID = DataProviderFactory.Instance.GenerateNumber<IWizardTemplateViewData,WizardTemplateView>(entry); 
            }

            if (isNew || !Exists(entry, templateView.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("ID", (string)templateView.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)templateView.ID);
            }

            statement.AddField("CHOOSERETAILGROUPS", templateView.ChooseRetailGroups, SqlDbType.TinyInt);
            statement.AddField("STATUS", templateView.Status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Save Permission for PosUsers into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of PosPermission</param>
        public virtual void SavePosPermission(IConnectionManager entry, WizardTemplateView templateView)
        {
            bool isNew = false;

            var statement = new SqlServerStatement("WIZARDTEMPLATE");

            ValidateSecurity(entry, BusinessObjects.Permission.VisualProfileEdit);


            if (templateView.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                templateView.ID = DataProviderFactory.Instance.GenerateNumber<IWizardTemplateViewData,WizardTemplateView>(entry); 
            }

            if (isNew || !Exists(entry, templateView.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("ID", (string)templateView.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)templateView.ID);
            }

            statement.AddField("CHOOSEPERMISSIONGROUP", templateView.ChoosePermissionGroup, SqlDbType.TinyInt);
            statement.AddField("STATUS", templateView.Status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Save Permission for TillLayout into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of TillLayoutPermission</param>
        public virtual void SaveTillLayoutPermission(IConnectionManager entry, WizardTemplateView templateView)
        {
            bool isNew = false;

            var statement = new SqlServerStatement("WIZARDTEMPLATE");

            ValidateSecurity(entry, BusinessObjects.Permission.VisualProfileEdit);


            if (templateView.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                templateView.ID = DataProviderFactory.Instance.GenerateNumber<IWizardTemplateViewData,WizardTemplateView>(entry); 
            }

            if (isNew || !Exists(entry, templateView.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("ID", (string)templateView.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)templateView.ID);
            }

            statement.AddField("CHOOSETILLLAYOUTS", templateView.ChooseTillLayouts, SqlDbType.TinyInt);
            statement.AddField("STATUS", templateView.Status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Save Permission for FormLayout into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of FormLayoutPermission</param>
        public virtual void SaveFormLayoutPermission(IConnectionManager entry, WizardTemplateView templateView)
        {
            bool isNew = false;

            var statement = new SqlServerStatement("WIZARDTEMPLATE");

            ValidateSecurity(entry, BusinessObjects.Permission.VisualProfileEdit);


            if (templateView.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                templateView.ID = DataProviderFactory.Instance.GenerateNumber<IWizardTemplateViewData,WizardTemplateView>(entry);
            }

            if (isNew || !Exists(entry, templateView.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("ID", (string)templateView.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)templateView.ID);
            }

            statement.AddField("CHOOSERECEIPTS", templateView.ChooseReceipts, SqlDbType.TinyInt);
            statement.AddField("STATUS", templateView.Status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Update the status of a template into database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>current status description</returns>
        public virtual void UpdateStatus(IConnectionManager entry, RecordIdentifier id)
        {
            var statement = new SqlServerStatement("WIZARDTEMPLATE");

            statement.AddField("STATUS", 2, SqlDbType.Int);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("ID", (string)id);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Update the last export date of a template into database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>current status description</returns>
        public virtual void UpdateLastExport(IConnectionManager entry, RecordIdentifier id)
        {
            var statement = new SqlServerStatement("WIZARDTEMPLATE");

            statement.AddField("LASTEXPORTDATE", DateTime.Now, SqlDbType.DateTime);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("ID", (string)id);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Check status of template into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns></returns>
        public virtual string StatusCompleted(IConnectionManager entry, RecordIdentifier id)
        {
            var sb = new System.Text.StringBuilder();

            if (!RecordExists(entry, "WIZARDTEMPLATE", "ID", id))
            {
                sb.Append("WIZARDTEMPLATE");
            }
            RecordIdentifier status = id;
            status.SecondaryID = 2;
            if (RecordExists(entry, "WIZARDTEMPLATE", new[] { "ID", "STATUS" }, status))
            {
                if (sb.Length > 0)
                {
                    sb.Append("~Complete");
                }
                else
                {
                    sb.Append("Complete");
                }
            }

            if (GetList<DataEntity>(entry, "WIZARDTEMPLATE", "STOREID", "ID", "ID").Find(entity => entity.ID.StringValue == id.StringValue && entity.Text == "") != null)
            {
                if (sb.Length > 0)
                {
                    sb.Append("~STORE");
                }
                else
                {
                    sb.Append("STORE");
                }
            }

            if (GetList<DataEntity>(entry, "WIZARDTEMPLATE", "TERMINALID", "ID", "ID").Find(entity => entity.ID.StringValue == id.StringValue && entity.Text == "") != null)
            {
                if (sb.Length > 0)
                {
                    sb.Append("~TERMINAL");
                }
                else
                {
                    sb.Append("TERMINAL");
                }
            }

            if (!RecordExists(entry, "WIZARDTEMPLATECURRENCY", "ID", id))
            {
                if (sb.Length > 0)
                {
                    sb.Append("~WIZARDTEMPLATECURRENCY");
                }
                else
                {
                    sb.Append("WIZARDTEMPLATECURRENCY");
                }
            }
            /*if (!RecordExists(entry, "WIZARDTEMPLATEFORMLAYOUTS", "ID", id))
            {
                if (sb.Length > 0)
                {
                    sb.Append("~WIZARDTEMPLATEFORMLAYOUTS");
                }
                else
                {
                    sb.Append("WIZARDTEMPLATEFORMLAYOUTS");
                }
            }*/
            if (!RecordExists(entry, "WIZARDTEMPLATEPERMISSION", "ID", id))
            {
                if (sb.Length > 0)
                {
                    sb.Append("~WIZARDTEMPLATEPERMISSION");
                }
                else
                {
                    sb.Append("WIZARDTEMPLATEPERMISSION");
                }
            }
            if (!RecordExists(entry, "WIZARDTEMPLATERETAILGROUPS", "ID", id))
            {
                if (sb.Length > 0)
                {
                    sb.Append("~WIZARDTEMPLATERETAILGROUPS");
                }
                else
                {
                    sb.Append("~WIZARDTEMPLATERETAILGROUPS");
                }
            }
            if (!RecordExists(entry, "WIZARDTEMPLATETAX", "ID", id))
            {
                if (sb.Length > 0)
                {
                    sb.Append("~WIZARDTEMPLATETAX");
                }
                else
                {
                    sb.Append("WIZARDTEMPLATETAX");
                }
            }
            if (!RecordExists(entry, "WIZARDTEMPLATETENDERS", "ID", id))
            {
                if (sb.Length > 0)
                {
                    sb.Append("~WIZARDTEMPLATETENDERS");
                }
                else
                {
                    sb.Append("WIZARDTEMPLATETENDERS");
                }
            }
            if (!RecordExists(entry, "WIZARDTEMPLATETILLLAYOUTS", "ID", id))
            {
                if (sb.Length > 0)
                {
                    sb.Append("~WIZARDTEMPLATETILLLAYOUTS");
                }
                else
                {
                    sb.Append("WIZARDTEMPLATETILLLAYOUTS");
                }
            }
            if (!RecordExists(entry, "WIZARDTEMPLATEUNITS", "ID", id))
            {
                if (sb.Length > 0)
                {
                    sb.Append("~WIZARDTEMPLATEUNITS");
                }
                else
                {
                    sb.Append("WIZARDTEMPLATEUNITS");
                }
            }
            if (!RecordExists(entry, "WIZARDTEMPLATEPERIPHERALS", "ID", id))
            {
                if (sb.Length > 0)
                {
                    sb.Append("~WIZARDTEMPLATEPERIPHERALS");
                }
                else
                {
                    sb.Append("WIZARDTEMPLATEPERIPHERALS");
                }
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// Get country list.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Country list</returns>
        public virtual List<DataEntity> GetCountryList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "COUNTRY", "NAME", "COUNTRYID", "NAME");
        }
        
        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "WizardTemplate"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "WIZARDTEMPLATE", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
