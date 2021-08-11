using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster.Dimensions
{
    public class DimensionTemplateData : SqlServerDataProviderBase, IDimensionTemplateData
    {
        private static void PopulateDimensionTemplate(IDataReader dr, DimensionTemplate entity)
        {
            entity.ID = (Guid)dr["ID"];
            entity.Text = (string)dr["DESCRIPTION"];
        }

        public virtual DimensionTemplate Get(IConnectionManager entry, RecordIdentifier templateID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "SELECT ID, DESCRIPTION FROM DIMENSIONTEMPLATE WHERE ID = @id AND DELETED = 0";

                MakeParam(cmd, "id", (Guid)templateID);

                List<DimensionTemplate> dimensions = Execute<DimensionTemplate>(entry, cmd, CommandType.Text, PopulateDimensionTemplate);

                return dimensions.Count > 0 ? dimensions[0] : null;
            }
        }

        public virtual DimensionTemplate GetByName(IConnectionManager entry, RecordIdentifier templateName)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "SELECT ID, DESCRIPTION FROM DIMENSIONTEMPLATE WHERE DESCRIPTION COLLATE SQL_Latin1_General_CP1_CI_AS = @description COLLATE SQL_Latin1_General_CP1_CI_AS AND DELETED = 0";

                MakeParam(cmd, "description", templateName.StringValue);

                List<DimensionTemplate> dimensions = Execute<DimensionTemplate>(entry, cmd, CommandType.Text, PopulateDimensionTemplate);

                return dimensions.Count > 0 ? dimensions[0] : null;
            }
        }

        public virtual List<DimensionTemplate> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "select ID, DESCRIPTION from DIMENSIONTEMPLATE where DELETED = 0";

                return Execute<DimensionTemplate>(entry, cmd, CommandType.Text, PopulateDimensionTemplate);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier templateID)
        {
            DeleteRecord(entry, "DIMENSIONTEMPLATE", "ID", templateID, BusinessObjects.Permission.ColorSizeStyleEdit);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier templateID)
        {
            return RecordExists(entry, "DIMENSIONTEMPLATE", "ID", templateID, false);
        }

        public virtual void Save(IConnectionManager entry, DimensionTemplate template)
        {
            bool isNew = false;

            SqlServerStatement statement = new SqlServerStatement("DIMENSIONTEMPLATE");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageItemDimensions);

            template.Validate();

            if(template.ID.IsEmpty)
            {
                isNew = true;
                template.ID = Guid.NewGuid(); // We generate the Guid here even if the SQL server can and does do it, we do this so we dont need extra execute scalar, to grab the Guid.
            }

            if (isNew || !Exists(entry,template.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", (Guid)template.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (Guid)template.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("DESCRIPTION", template.Text);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual RecordIdentifier SaveAndReturnTemplateID(IConnectionManager entry, DimensionTemplate template)
        {
            bool isNew = false;

            SqlServerStatement statement = new SqlServerStatement("DIMENSIONTEMPLATE");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageItemDimensions);

            template.Validate();

            if (template.ID.IsEmpty)
            {
                isNew = true;
                template.ID = Guid.NewGuid(); // We generate the Guid here even if the SQL server can and does do it, we do this so we dont need extra execute scalar, to grab the Guid.
            }

            if (isNew || !Exists(entry, template.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", (Guid)template.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (Guid)template.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("DESCRIPTION", template.Text);

            entry.Connection.ExecuteStatement(statement);

            return template.ID;
        }
    }
}
