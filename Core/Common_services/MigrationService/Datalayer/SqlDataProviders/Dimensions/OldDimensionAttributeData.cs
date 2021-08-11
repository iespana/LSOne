using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Services.Datalayer.BusinessObjects.Dimensions;
using LSOne.Services.Datalayer.DataProviders.Dimensions;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.SqlDataProviders.Dimensions
{
    public class OldDimensionAttributeData : SqlServerDataProviderBase, IOldDimensionAttributeData
    {
        private static void PopulateDimensionAttribute(IDataReader dr, OldDimensionAttribute entity)
        {
            entity.ID = (Guid)dr["ID"];
            entity.DimensionID = (Guid)dr["DIMENSIONID"];
            entity.Text = (string)dr["DESCRIPTION"];
            entity.Code = (string)dr["CODE"];
            entity.Sequence = (int)dr["SEQUENCE"];
        }

        public virtual OldDimensionAttribute Get(IConnectionManager entry, RecordIdentifier attributeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "select ID, DIMENSIONID, DESCRIPTION, CODE, SEQUENCE from DIMENSIONATTRIBUTE where ID = @id and DELETED = 0";

                MakeParam(cmd, "id", (Guid)attributeID);

                List<OldDimensionAttribute> dimensionAttributes = Execute<OldDimensionAttribute>(entry, cmd, CommandType.Text, PopulateDimensionAttribute);

                return dimensionAttributes.Count > 0 ? dimensionAttributes[0] : null;
            }
        }

        public virtual List<OldDimensionAttribute> GetListForRetailItemDimension(IConnectionManager entry, RecordIdentifier retailItemDimensionID)
        {
            return GetListForParentObject(entry, retailItemDimensionID);
        }

        public virtual List<OldDimensionAttribute> GetListForDimensionTemplate(IConnectionManager entry, RecordIdentifier dimensionTemplateID)
        {
            return GetListForParentObject(entry, dimensionTemplateID);
        }

        protected virtual List<OldDimensionAttribute> GetListForParentObject(IConnectionManager entry, RecordIdentifier parentObjectID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "select ID, DIMENSIONID, DESCRIPTION, CODE, SEQUENCE from DIMENSIONATTRIBUTE where DIMENSIONID = @id and DELETED = 0";

                MakeParam(cmd, "id", (Guid)parentObjectID);

                return Execute<OldDimensionAttribute>(entry, cmd, CommandType.Text, PopulateDimensionAttribute);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier templateID)
        {
            if (entry.HasPermission(DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit) || entry.HasPermission(DataLayer.BusinessObjects.Permission.ItemsEdit))
            {
                MarkAsDeleted(entry, "DIMENSIONATTRIBUTE", "ID", templateID, "");
            }
            else
            {
                throw new PermissionException(DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier attributeID)
        {
            return RecordExists(entry, "DIMENSIONATTRIBUTE", "ID", attributeID,false);
        }

        public virtual void Save(IConnectionManager entry, OldDimensionAttribute attribute)
        {
            bool isNew = false;

            SqlServerStatement statement = new SqlServerStatement("DIMENSIONATTRIBUTE");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ItemsEdit);

            if (!(entry.HasPermission(DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit) || entry.HasPermission(DataLayer.BusinessObjects.Permission.ItemsEdit)))
            {
                throw new PermissionException(DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);
            }

            attribute.Validate();

            if (attribute.ID.IsEmpty)
            {
                attribute.ID = Guid.NewGuid(); // We generate the Guid here even if the SQL server can and does do it, we do this so we dont need extra execute scalar, to grab the Guid.
                isNew = true;
            }

            if (isNew || !Exists(entry,attribute.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", (Guid)attribute.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (Guid)attribute.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("DIMENSIONID", attribute.DimensionID, SqlDbType.UniqueIdentifier);
            statement.AddField("DESCRIPTION", attribute.Text);
            statement.AddField("CODE", attribute.Code);
            statement.AddField("SEQUENCE", (int)attribute.Sequence, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }


    }
}
