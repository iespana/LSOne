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
    public class RetailItemDimensionData : SqlServerDataProviderBase, IRetailItemDimensionData
    {
        private static void PopulateRetailItemDimension(IDataReader dr, RetailItemDimension entity)
        {
            entity.ID = (Guid)dr["ID"];
            entity.Text = (string)dr["DESCRIPTION"];
            entity.RetailItemMasterID = (Guid)dr["RETAILITEMID"];
            entity.Sequence = (int)dr["SEQUENCE"];
        }

        public virtual RetailItemDimension Get(IConnectionManager entry, RecordIdentifier templateID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "SELECT ID, RETAILITEMID, DESCRIPTION, SEQUENCE FROM RETAILITEMDIMENSION WHERE ID = @id AND DELETED = 0";

                MakeParam(cmd, "id", (Guid)templateID);

                List<RetailItemDimension> dimensions = Execute<RetailItemDimension>(entry, cmd, CommandType.Text, PopulateRetailItemDimension);

                return dimensions.Count > 0 ? dimensions[0] : null;
            }
        }

        public virtual List<RetailItemDimension> GetListForRetailItem(IConnectionManager entry, RecordIdentifier retailItemMasterID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "SELECT ID, RETAILITEMID, DESCRIPTION, SEQUENCE FROM RETAILITEMDIMENSION WHERE RETAILITEMID = @retailItemID AND DELETED = 0 ORDER BY SEQUENCE";

                MakeParam(cmd, "retailItemID", (Guid)retailItemMasterID);

                return Execute<RetailItemDimension>(entry, cmd, CommandType.Text, PopulateRetailItemDimension);
            }
        }

        /// <summary>
        /// Returns a list of all retail item dimensions that are currently being used on variant items belonging to the given item master ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailItemMasterID">The ID of the master item</param>
        /// <returns></returns>
        public virtual List<RetailItemDimension> GetListInUseByRetailItem(IConnectionManager entry, RecordIdentifier retailItemMasterID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    @"select distinct 
	                      d.ID, 
	                      d.RETAILITEMID, 
	                      d.DESCRIPTION, 
	                      d.SEQUENCE
                      from RETAILITEMDIMENSION d
                      join DIMENSIONATTRIBUTE da on da.DIMENSIONID = d.ID     
                      where d.RETAILITEMID = @itemID and 
                              da.ID in 
                              (
                              select distinct rda.DIMENSIONATTRIBUTEID 
                              from RETAILITEMDIMENSIONATTRIBUTE rda
                              join RETAILITEM ri on ri.MASTERID = rda.RETAILITEMID and ri.HEADERITEMID = @itemID and ri.deleted = 0
                              )
                      order by d.SEQUENCE ASC";

                MakeParam(cmd, "itemID", (Guid) retailItemMasterID, SqlDbType.UniqueIdentifier);

                return Execute<RetailItemDimension>(entry, cmd, CommandType.Text, PopulateRetailItemDimension);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier dimensionID)
        {
            MarkAsDeleted(entry, "RETAILITEMDIMENSION", "ID", dimensionID, BusinessObjects.Permission.ItemsEdit);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier dimensionID)
        {
            return RecordExists(entry, "RETAILITEMDIMENSION", "ID", dimensionID, false);
        }

        public virtual void Save(IConnectionManager entry, RetailItemDimension dimension)
        {
            bool isNew = false;

            SqlServerStatement statement = new SqlServerStatement("RETAILITEMDIMENSION");

            string[] permissions = { BusinessObjects.Permission.ItemsEdit, BusinessObjects.Permission.ManagePurchaseOrders };
            ValidateSecurity(entry, permissions);

            dimension.Validate();

            if (dimension.ID.IsEmpty)
            {
                isNew = true;
            }

            if (isNew || !Exists(entry,dimension.ID))
            {
                statement.StatementType = StatementType.Insert;
                
                dimension.ID = isNew ? Guid.NewGuid() : dimension.ID; // We generate the Guid here even if the SQL server can and does do it, we do this so we dont need extra execute scalar, to grab the Guid.

                statement.AddKey("ID", (Guid)dimension.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (Guid)dimension.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("DESCRIPTION", dimension.Text);
            statement.AddField("RETAILITEMID", (Guid)dimension.RetailItemMasterID, SqlDbType.UniqueIdentifier);
            statement.AddField("SEQUENCE", (int)dimension.Sequence, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }


    }
}
