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
    public class OldRetailItemDimensionData : SqlServerDataProviderBase, IOldRetailItemDimensionData
    {
        private static void PopulateRetailItemDimension(IDataReader dr, OldRetailItemDimension entity)
        {
            entity.ID = (Guid)dr["ID"];
            entity.Text = (string)dr["DESCRIPTION"];
            entity.RetailItemMasterID = (Guid)dr["RETAILITEMID"];
            entity.Sequence = (int)dr["SEQUENCE"];
        }

        public virtual OldRetailItemDimension Get(IConnectionManager entry, RecordIdentifier templateID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "select ID, RETAILITEMID, DESCRIPTION, SEQUENCE from RETAILITEMDIMENSION where ID = @id and DELETED = 0";

                MakeParam(cmd, "id", (Guid)templateID);

                List<OldRetailItemDimension> dimensions = Execute<OldRetailItemDimension>(entry, cmd, CommandType.Text, PopulateRetailItemDimension);

                return dimensions.Count > 0 ? dimensions[0] : null;
            }
        }

        public virtual List<OldRetailItemDimension> GetListForRetailItem(IConnectionManager entry, RecordIdentifier retailItemMasterID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "select ID, RETAILITEMID, DESCRIPTION, SEQUENCE from RETAILITEMDIMENSION where RETAILITEMID = @retailItemID and DELETED = 0 order by SEQUENCE";

                MakeParam(cmd, "retailItemID", (Guid)retailItemMasterID);

                return Execute<OldRetailItemDimension>(entry, cmd, CommandType.Text, PopulateRetailItemDimension);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier dimensionID)
        {
            MarkAsDeleted(entry, "RETAILITEMDIMENSION", "ID", dimensionID, DataLayer.BusinessObjects.Permission.ItemsEdit);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier dimensionID)
        {
            return RecordExists(entry, "RETAILITEMDIMENSION", "ID", dimensionID, false);
        }

        public virtual void Save(IConnectionManager entry, OldRetailItemDimension dimmension)
        {
            bool isNew = false;

            SqlServerStatement statement = new SqlServerStatement("RETAILITEMDIMENSION");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ItemsEdit);

            dimmension.Validate();

            if (dimmension.ID.IsEmpty)
            {
                isNew = true;
            }

            if (isNew || !Exists(entry,dimmension.ID))
            {
                statement.StatementType = StatementType.Insert;

                dimmension.ID = Guid.NewGuid(); // We generate the Guid here even if the SQL server can and does do it, we do this so we dont need extra execute scalar, to grab the Guid.

                statement.AddKey("ID", (Guid)dimmension.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (Guid)dimmension.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("DESCRIPTION", dimmension.Text);
            statement.AddField("RETAILITEMID", (Guid)dimmension.RetailItemMasterID, SqlDbType.UniqueIdentifier);
            statement.AddField("SEQUENCE", (int)dimmension.Sequence, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }


    }
}
