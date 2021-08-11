using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Services.Datalayer.BusinessObjects.Dimensions;
using LSOne.Services.Datalayer.DataProviders.Dimensions;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.SqlDataProviders.Dimensions
{
    public class OldDimensionTemplateData : SqlServerDataProviderBase, IOldDimensionTemplateData
    {
        private static void PopulateDimensionTemplate(IDataReader dr, OldDimensionTemplate entity)
        {
            entity.ID = (Guid)dr["ID"];
            entity.Text = (string)dr["DESCRIPTION"];
        }

        public virtual OldDimensionTemplate Get(IConnectionManager entry, RecordIdentifier templateID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "select ID, DESCRIPTION from DIMENSIONTEMPLATE where ID = @id and DELETED = 0";

                MakeParam(cmd, "id", (Guid)templateID);

                List<OldDimensionTemplate> dimensions = Execute<OldDimensionTemplate>(entry, cmd, CommandType.Text, PopulateDimensionTemplate);

                return dimensions.Count > 0 ? dimensions[0] : null;
            }
        }

        public virtual List<OldDimensionTemplate> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "select ID, DESCRIPTION from DIMENSIONTEMPLATE where DELETED = 0";

                return Execute<OldDimensionTemplate>(entry, cmd, CommandType.Text, PopulateDimensionTemplate);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier templateID)
        {
            DeleteRecord(entry, "DIMENSIONTEMPLATE", "ID", templateID, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier templateID)
        {
            return RecordExists(entry, "DIMENSIONTEMPLATE", "ID", templateID, false);
        }

        public virtual void Save(IConnectionManager entry, OldDimensionTemplate template)
        {
            bool isNew = false;

            SqlServerStatement statement = new SqlServerStatement("DIMENSIONTEMPLATE");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);

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
    }
}
