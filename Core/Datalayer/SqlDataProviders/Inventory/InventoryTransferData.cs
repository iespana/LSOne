using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    public class InventoryTransferData : SqlServerDataProviderBase, IInventoryTransferData
    {
        public virtual int CreateStoreTransferLinesFromFilter(IConnectionManager entry, RecordIdentifier transferID, RecordIdentifier storeID, InventoryTemplateFilterContainer filter, StoreTransferTypeEnum transferType)
        {
            using (SqlCommand cmd = new SqlCommand("spINVENTORY_CreateStoreTransferLinesFromFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                string filterDelimiter = ";";

                MakeParam(cmd, "STOREID", storeID);
                MakeParam(cmd, "TRANSFERID", transferID);
                MakeParam(cmd, "RETAILGROUPS", filter.RetailGroups.Count == 0 ? "" : filter.RetailGroups.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y), SqlDbType.VarChar);
                MakeParam(cmd, "RETAILDEPARTMENTS", filter.RetailDepartments.Count == 0 ? "" : filter.RetailDepartments.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y), SqlDbType.VarChar);
                MakeParam(cmd, "VENDORS", filter.Vendors.Count == 0 ? "" : filter.Vendors.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y));
                MakeParam(cmd, "SPECIALGROUPS", filter.SpecialGroups.Count == 0 ? "" : filter.SpecialGroups.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y), SqlDbType.VarChar);
                MakeParam(cmd, "FILTERDELIMITER", filterDelimiter, SqlDbType.VarChar);
                MakeParam(cmd, "TRANSFERTYPE", (int)transferType, SqlDbType.Int);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                SqlParameter insertedRows = MakeParam(cmd, "INSERTEDRECORDS", "", SqlDbType.Int, ParameterDirection.Output, 4);

                entry.Connection.ExecuteNonQuery(cmd, false);

                return (int)insertedRows.Value;
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotImplementedException();
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotImplementedException();
        }

        public virtual void Save(IConnectionManager entry, DataEntity item)
        {
            throw new NotImplementedException();
        }
    }
}
