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
    public class TemplateTerminalData : SqlServerDataProviderBase, ITemplateTerminalData
    {
        /// <summary>
        /// Gets a list of all terminals based on selected store
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">Store ID</param>
        /// <returns>A list of all terminals</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"Select TerminalID, ISNULL(Name,'') as Name from 
                              RBOTERMINALTABLE where DATAAREAID = @dataAreaId and StoreID = @storeID order by TerminalID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string) storeID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "Name", "TerminalID");
            }
        }

        public virtual RecordIdentifier GetterminalID(IConnectionManager entry, RecordIdentifier id)
        {
            RecordIdentifier terminalId = "";

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select TERMINALID " +
                    "from WIZARDTEMPLATE " +
                    "where DATAAREAID = @dataAreaID and ID = " + (string)id;

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    // If no record is returned, no default terminal exists.
                    if (dr.Read())
                    {
                        if (dr["TERMINALID"] != DBNull.Value)
                        {
                            terminalId = (string)dr["TERMINALID"];
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

            return terminalId;
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "WIZARDTEMPLATE", "ID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            //DeleteRecord(entry, "WIZARDTEMPLATE", "ID", id, BusinessObjects.Permission.VisualProfileEdit);
        }

        public virtual void Save(IConnectionManager entry, TemplateTerminal template)
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
            statement.AddField("terminalID", (string)template.ID, SqlDbType.NVarChar);
          
            entry.Connection.ExecuteStatement(statement);
        }
    }
}
