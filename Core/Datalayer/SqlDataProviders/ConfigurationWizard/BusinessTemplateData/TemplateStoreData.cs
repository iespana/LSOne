using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard.BusinessTemplate;
using LSOne.DataLayer.DataProviders.ConfigurationWizard.BusinessTemplateData;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ConfigurationWizard.BusinessTemplateData
{
    public class TemplateStoreData : SqlServerDataProviderBase, ITemplateStoreData
    {        
        /// <summary>
        /// Gets a list of all stores
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all stores</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOSTORETABLE", "NAME", "STOREID", "STOREID");
        }

        public virtual RecordIdentifier GetStoreID(IConnectionManager entry, RecordIdentifier id)
        {
            RecordIdentifier storeId = "";

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select STOREID " +
                    "from WIZARDTEMPLATE " +
                    "where DATAAREAID = @dataAreaID and ID = " + (string)id;

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    // If no record is returned, no default store exists.
                    if (dr.Read())
                    {
                        if (dr["STOREID"] != DBNull.Value)
                        {
                            storeId = (string)dr["STOREID"];
                        }
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
            }

            return storeId;
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "WIZARDTEMPLATE", "ID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            //DeleteRecord(entry, "WIZARDTEMPLATE", "ID", id, BusinessObjects.Permission.VisualProfileEdit);
        }

        public virtual void Save(IConnectionManager entry, TemplateStore template)
        {
            bool isNew = false;

            var statement = new SqlServerStatement("WIZARDTEMPLATE");

            ValidateSecurity(entry, BusinessObjects.Permission.ConfigurationWizardEdit);

            if (isNew || !Exists(entry, template.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddKey("ID", (string)template.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)template.ID);
            }
            statement.AddField("STOREID", (string)template.ID, SqlDbType.NVarChar);
            //statement.AddField("TERMINALID", (string)template.TerminalId, SqlDbType.NVarChar);
            //if (template.TEMPLATEIMAGE != null)
            //    statement.AddField("IMAGE", template.TEMPLATEIMAGE, SqlDbType.Image);            

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
