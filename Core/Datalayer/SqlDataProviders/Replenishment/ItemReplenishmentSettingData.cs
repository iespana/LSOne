using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Replenishment
{
    public class ItemReplenishmentSettingData : SqlServerDataProviderBase, IItemReplenishmentSettingData
    {
        private static string BaseSql(bool includeUnitData)
        {
            string join = "";
            string select = "";

            string sql = @"SELECT 
                    i.ID
                    ,i.ITEMID
                    ,ISNULL(it.ITEMNAME, '') as ITEMNAME
                    ,i.STOREID
                    ,ISNULL(s.NAME, '') as STORENAME
                    ,i.REPLENISHMENTMETHOD
                    ,i.REORDERPOINT
                    ,i.MAXIMUMINVENTORY
                    ,i.DATAAREAID
                    ,i.PURCHASEORDERMULTIPLE
                    ,i.PURCHASEORDERMULTIPLEROUNDING
                    ,i.BLOCKEDFORREPLENISHMENT
                    ,ISNULL(i.BLOCKINGDATE, '01.01.1900') as BLOCKINGDATE 
                    <select>
                FROM ITEMREPLENISHMENTSETTING i 
                Left outer join RBOSTORETABLE s on s.STOREID = i.STOREID AND i.DATAAREAID = s.DATAAREAID 
                Left outer join RETAILITEM it on i.ITEMID = it.ITEMID
                <joins> ";

            if (includeUnitData)
            {
                join = @"Left outer join UNIT u on u.UNITID = it.INVENTORYUNITID ";

                select = ",u.UNITID, ISNULL(u.TXT,'') as UnitDescription, ISNULL(u.UNITDECIMALS,0) as UNITDECIMALS,ISNULL(u.MINUNITDECIMALS,0) as MINUNITDECIMALS";
            }

            sql = sql.Replace("<joins>", join);
            sql = sql.Replace("<select>", select);

            return sql;
        }

       

        private static void PopulateItemReplenishmentSettingsWithUnit(IDataReader dr, ItemReplenishmentSetting setting)
        {
            PopulateItemReplenishmentSettings(dr, setting);

            if (!(dr["UNITID"] is DBNull))
            {
                setting.Unit = new Unit();
                setting.Unit.ID = (string)dr["UNITID"];
                setting.Unit.Text = (string)dr["UnitDescription"];
                setting.Unit.MaximumDecimals = (int)dr["UNITDECIMALS"];
                setting.Unit.MinimumDecimals = (int)dr["MINUNITDECIMALS"];
            }
        }

        private static void PopulateItemReplenishmentSettings(IDataReader dr, ItemReplenishmentSetting setting)
        {
            setting.ID = (Guid)dr["ID"];
            setting.ItemId = (string)dr["ITEMID"];
            setting.ItemName = (string)dr["ITEMNAME"];
            setting.StoreId = (string)dr["STOREID"];
            setting.StoreName = (string)dr["STORENAME"];
            setting.ReplenishmentMethod = (ReplenishmentMethodEnum)dr["REPLENISHMENTMETHOD"];
            setting.ReorderPoint = (decimal)dr["REORDERPOINT"];
            setting.MaximumInventory = (decimal)dr["MAXIMUMINVENTORY"];
            setting.PurchaseOrderMultiple = (int)dr["PURCHASEORDERMULTIPLE"];
            setting.PurchaseOrderMultipleRounding = (PurchaseOrderMultipleRoundingEnum)dr["PURCHASEORDERMULTIPLEROUNDING"];
            setting.BlockedForReplenishment = (BlockedForReplenishmentEnum)dr["BLOCKEDFORREPLENISHMENT"];
            setting.BlockingDate = (DateTime)dr["BLOCKINGDATE"];
        }

        public virtual ItemReplenishmentSetting GetForItem(IConnectionManager entry, RecordIdentifier itemId, bool includeUnitData = false)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<ItemReplenishmentSetting> replenishmentSettings;

                string sql = BaseSql(includeUnitData) +
                        "WHERE i.ITEMID = @itemId AND i.STOREID = '' AND i.DATAAREAID = @dataAreaId";
                         
                cmd.CommandText = sql;

                MakeParam(cmd, "itemId", (string)itemId);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                if (includeUnitData)
                {
                    replenishmentSettings = Execute<ItemReplenishmentSetting>(entry, cmd, CommandType.Text, PopulateItemReplenishmentSettingsWithUnit);
                }
                else
                {
                    replenishmentSettings = Execute<ItemReplenishmentSetting>(entry, cmd, CommandType.Text, PopulateItemReplenishmentSettings);
                }

                
                return replenishmentSettings.Count > 0 ? replenishmentSettings[0] : null;
            }
        }

        public virtual List<ItemReplenishmentSetting> GetForItemAndStore(IConnectionManager entry, RecordIdentifier itemId, bool includeUnitData = false)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<ItemReplenishmentSetting> replenishmentSettings;

                string sql = BaseSql(includeUnitData) +
                        "WHERE i.ITEMID = @itemId AND i.DATAAREAID = @dataAreaId Order by STOREID";

                cmd.CommandText = sql;

                MakeParam(cmd, "itemId", (string)itemId);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                if (includeUnitData)
                {
                    replenishmentSettings = Execute<ItemReplenishmentSetting>(entry, cmd, CommandType.Text, PopulateItemReplenishmentSettingsWithUnit);
                }
                else
                {
                    replenishmentSettings = Execute<ItemReplenishmentSetting>(entry, cmd, CommandType.Text, PopulateItemReplenishmentSettings);
                }


                return replenishmentSettings;
            }
        }

        public virtual ItemReplenishmentSetting Get(IConnectionManager entry, RecordIdentifier settingsId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string sql = BaseSql(false) +
                        "WHERE i.ID = @id AND i.DATAAREAID = @dataAreaId";

                cmd.CommandText = sql;

                MakeParam(cmd, "id", (Guid)settingsId);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var replnishmentSettings = Execute<ItemReplenishmentSetting>(entry, cmd, CommandType.Text, PopulateItemReplenishmentSettings);
                return replnishmentSettings.Count > 0 ? replnishmentSettings[0] : null;
            }
        }

        /// <summary>
        /// Returns ItemReplenishmentSettings for an item. If the store is overwriting some of the values then the overwritten values are returned.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="itemId"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public virtual ItemReplenishmentSetting GetItemSettingForStore(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier storeId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string sql = @"Select 
                               i.ID
                              ,i.ITEMID
                              ,ISNULL(it.ITEMNAME, '') as ITEMNAME
                              ,i.STOREID
                              ,ISNULL(s.NAME, '') as STORENAME
                              ,i.REPLENISHMENTMETHOD
                              ,i.REORDERPOINT as REORDERPOINT
                              ,i.MAXIMUMINVENTORY as MAXIMUMINVENTORY
                              ,i.PURCHASEORDERMULTIPLE
                              ,i.PURCHASEORDERMULTIPLEROUNDING
                              ,i.BLOCKEDFORREPLENISHMENT
                              ,ISNULL(i.BLOCKINGDATE, '01.01.1900') as BLOCKINGDATE
                              ,i.DATAAREAID
                            FROM ITEMREPLENISHMENTSETTING i
                                LEFT OUTER JOIN RETAILITEM it on i.ITEMID = it.ITEMID 
                                LEFT OUTER JOIN RBOSTORETABLE s on s.STOREID = i.STOREID AND i.DATAAREAID = s.DATAAREAID 
                            WHERE i.ITEMID = @itemId AND i.DATAAREAID = @dataAreaId AND i.STOREID = @storeId ";

                cmd.CommandText = sql;

                MakeParam(cmd, "itemId", (string)itemId);
                MakeParam(cmd, "storeId", (string)storeId);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var replenishmentSettings = Execute<ItemReplenishmentSetting>(entry, cmd, CommandType.Text, PopulateItemReplenishmentSettings);
                return replenishmentSettings.Count > 0 ? replenishmentSettings[0] : null;
            }
        }

        /// <summary>
        /// Gets ID of a ItemReplenishmentSetting from given ItemID and StoreID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">ID of the RetailTem (not MasterID)</param>
        /// <param name="storeId">ID of the store</param>
        /// <returns></returns>
        public virtual RecordIdentifier GetItemReplenishmentSettingID(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier storeId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select ID from ITEMREPLENISHMENTSETTING where ITEMID = @itemID and STOREID = @storeID";
                                   
                MakeParam(cmd, "itemID",(string)itemId);
                MakeParam(cmd, "storeID", (string)storeId);

                object result = entry.Connection.ExecuteScalar(cmd);

                if (result == null || result is DBNull)
                {
                    return RecordIdentifier.Empty;
                }

                return (RecordIdentifier)(Guid)result;
            }
        }

        public virtual List<ItemReplenishmentSetting> GetListForStores(IConnectionManager entry, RecordIdentifier itemId, bool includeUnitData = false)
        {
            List<ItemReplenishmentSetting> replenishmentSettings;

            using (var cmd = entry.Connection.CreateCommand())
            {
                string sql = BaseSql(includeUnitData) +
                        "WHERE i.ITEMID = @itemId AND i.STOREID <> '' AND i.DATAAREAID = @dataAreaId";

                cmd.CommandText = sql;

                MakeParam(cmd, "itemId", (string)itemId);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                if (includeUnitData)
                {
                    replenishmentSettings = Execute<ItemReplenishmentSetting>(entry, cmd, CommandType.Text, PopulateItemReplenishmentSettingsWithUnit);
                }
                else
                {
                    replenishmentSettings = Execute<ItemReplenishmentSetting>(entry, cmd, CommandType.Text, PopulateItemReplenishmentSettings);
                }

                return replenishmentSettings;
            }
        }

        public virtual ItemReplenishmentSettingsContainer GetReplenishmentSettingsForExcel(IConnectionManager entry, RecordIdentifier itemId)
        {
            var container = new ItemReplenishmentSettingsContainer();

            container.ItemReplenishmentSetting = GetForItemAndStore(entry,itemId,true);

            /*var settingForItem = GetForItem(entry, itemId,true);
            if (settingForItem != null)
            {
                container.ItemReplenishmentSetting.Add(settingForItem);
            }*

            var settingForStores = GetListForStores(entry, itemId,true);
            container.ItemReplenishmentSetting.AddRange(settingForStores);
            */
            return container;
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier settingsId)
        {
            DeleteRecord(entry, "ITEMREPLENISHMENTSETTING", "ID", settingsId, BusinessObjects.Permission.ManageReplenishment);
        }

        /// <summary>
        /// Deletes a record by a itemID and storeID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of the retail item (not MasterID)</param>
        /// <param name="storeID">ID of the store</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID)
        {
            RecordIdentifier id = new RecordIdentifier(itemID, storeID);

            DeleteRecord(entry, "ITEMREPLENISHMENTSETTING", new string[]{ "ITEMID","STOREID" }, id, BusinessObjects.Permission.ManageReplenishment);
        }


        /// <summary>
        /// Deletes a record by a given record ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="settingsId">ID of the records to be deleted</param>
        /// <returns></returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier settingsId)
        {
            return RecordExists(entry, "ITEMREPLENISHMENTSETTING", "ID", settingsId);        
        }



        public virtual void Save(IConnectionManager entry, ItemReplenishmentSetting setting)
        {
            var statement = new SqlServerStatement("ITEMREPLENISHMENTSETTING");
            bool isNew = false;

            if (setting.ID == Guid.Empty || setting.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                setting.ID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, setting.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)setting.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)setting.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("ITEMID", (string)setting.ItemId);
            statement.AddField("STOREID", (string)setting.StoreId);
            statement.AddField("REPLENISHMENTMETHOD", setting.ReplenishmentMethod, SqlDbType.Int);
            statement.AddField("REORDERPOINT", setting.ReorderPoint, SqlDbType.Decimal);
            statement.AddField("MAXIMUMINVENTORY", setting.MaximumInventory, SqlDbType.Decimal);
            statement.AddField("PURCHASEORDERMULTIPLE", setting.PurchaseOrderMultiple, SqlDbType.Int);
            statement.AddField("PURCHASEORDERMULTIPLEROUNDING", setting.PurchaseOrderMultipleRounding, SqlDbType.Int);
            statement.AddField("BLOCKEDFORREPLENISHMENT", setting.BlockedForReplenishment, SqlDbType.Int);
            statement.AddField("BLOCKINGDATE", setting.BlockingDate, SqlDbType.DateTime);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
