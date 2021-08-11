using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Replenishment
{
    public class InventoryTemplateStoreConnectionData : SqlServerDataProviderBase, IInventoryTemplateStoreConnectionData
    {
        private static void PopulateStoreConnection(IDataReader dr, InventoryTemplateStoreConnection storeConnection)
        {
            storeConnection.ID = (Guid)dr["ID"];
            storeConnection.TemplateID = (string)dr["INVENTORYTEMPLATEID"];
            storeConnection.StoreID = (string)dr["STOREID"];
        }

        public List<InventoryTemplateStoreConnection> GetList(IConnectionManager entry,
                                                                                  RecordIdentifier templateId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"Select ID, INVENTORYTEMPLATEID, STOREID
                                  From INVENTORYTEMPLATESTORECONNECTION
                                  where INVENTORYTEMPLATEID = @templateId AND DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "templateId", (string)templateId);

                var storeConnections = Execute<InventoryTemplateStoreConnection>(entry, cmd, CommandType.Text, PopulateStoreConnection);
                return storeConnections;
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier templateStoreConnectionId)
        {
            DeleteRecord(entry, "INVENTORYTEMPLATESTORECONNECTION", "ID", templateStoreConnectionId, 
                LSOne.DataLayer.BusinessObjects.Permission.ManageReplenishment);
        }

        public virtual void DeleteForStore(IConnectionManager entry, RecordIdentifier storeID)
        {
            DeleteRecord(entry, "INVENTORYTEMPLATESTORECONNECTION", "STOREID", storeID,
                LSOne.DataLayer.BusinessObjects.Permission.ManageReplenishment);
        }

        public virtual void DeleteForTemplate(IConnectionManager entry, RecordIdentifier templateID)
        {
            DeleteRecord(entry, "INVENTORYTEMPLATESTORECONNECTION", "INVENTORYTEMPLATEID", templateID,
                LSOne.DataLayer.BusinessObjects.Permission.ManageReplenishment);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier templateStoreConnectionId)
        {
            return RecordExists(entry, "INVENTORYTEMPLATESTORECONNECTION", "ID", templateStoreConnectionId);        
        }

        public virtual void Save(IConnectionManager entry, InventoryTemplateStoreConnection templateStoreConnection)
        {
            var statement = new SqlServerStatement("INVENTORYTEMPLATESTORECONNECTION");

            if (!Exists(entry, templateStoreConnection.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)templateStoreConnection.ID,SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)templateStoreConnection.ID ,SqlDbType.UniqueIdentifier);
            }

            statement.AddField("INVENTORYTEMPLATEID", (string)templateStoreConnection.TemplateID);
            statement.AddField("STOREID", (string)templateStoreConnection.StoreID);
          
            entry.Connection.ExecuteStatement(statement);
        }

    }
}
