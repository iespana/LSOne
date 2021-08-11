using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
    public class ItemTranslationData : SqlServerDataProviderBase, IItemTranslationData
    {
        private static void ItemTranslationPopulator(IDataReader dr, ItemTranslation itemTranslation)
        {
            itemTranslation.ID = (Guid)dr["ID"];
            itemTranslation.ItemID = (string)dr["ITEMID"];
            itemTranslation.LanguageID = (string)dr["CULTURENAME"];
            itemTranslation.Description = (string)dr["DESCRIPTION"];
        }

        public virtual ItemTranslation Get(IConnectionManager entry, RecordIdentifier itemTranslationID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"Select ID, ITEMID, CULTURENAME, DESCRIPTION
                    From RBOINVENTTRANSLATIONS
                    Where ID = @ID and DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "ID", (Guid)itemTranslationID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Get<ItemTranslation>(entry, cmd,itemTranslationID, ItemTranslationPopulator, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual List<ItemTranslation> GetList(IConnectionManager entry, RecordIdentifier itemID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            RecordIdentifier id = new RecordIdentifier("ItemTranslationData.GetList", itemID);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"Select ID, ITEMID, CULTURENAME, DESCRIPTION
                    From RBOINVENTTRANSLATIONS
                    Where ITEMID = @ITEMID and DATAAREAID = @DATAAREAID
                    Order by CULTURENAME";

                MakeParam(cmd, "ITEMID", (string) itemID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return GetList<ItemTranslation>(entry, cmd, id, ItemTranslationPopulator, cacheType);
            }
        }

        public virtual List<ItemTranslation> GetListByCultureName(IConnectionManager entry, string cultureName, CacheType cacheType = CacheType.CacheTypeNone)
        {
            RecordIdentifier id = new RecordIdentifier("GetListByCultureName", cultureName);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"Select ID, ITEMID, CULTURENAME, DESCRIPTION
                    From RBOINVENTTRANSLATIONS
                    Where CULTURENAME = @cultureName and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "cultureName", cultureName);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return GetList<ItemTranslation>(entry, cmd, id, ItemTranslationPopulator, cacheType);
            }
        }

        public virtual string GetItemTranslationByCultureName(IConnectionManager entry, RecordIdentifier itemId, string cultureName, CacheType cacheType = CacheType.CacheTypeNone)
        {
            RecordIdentifier id = new RecordIdentifier("GetItemTranslationByCultureName", itemId, cultureName);
            if (cacheType != CacheType.CacheTypeNone)
            {
                DataEntity entity = (DataEntity) entry.Cache.GetEntityFromCache(typeof (DataEntity), id);
                if (entity != null)
                {
                    return entity.Text;
                }
            }

            string result = null;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                        @"Select DESCRIPTION
                        From RBOINVENTTRANSLATIONS
                        Where ITEMID = @itemId and DATAAREAID = @dataAreaId and CULTURENAME = @cultureName";

                MakeParam(cmd, "itemId", (string) itemId);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "cultureName", cultureName);

                result = (string) entry.Connection.ExecuteScalar(cmd);
            }

            if (result != null && cacheType != CacheType.CacheTypeNone)
            {
                entry.Cache.AddEntityToCache(id, new DataEntity("", result), cacheType);
            }

            return result;
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier itemTranslationID)
        {
            DeleteRecord(entry, "RBOINVENTTRANSLATIONS", "ID", itemTranslationID, BusinessObjects.Permission.ItemsEdit);
        }

        public virtual void Save(IConnectionManager entry, ItemTranslation itemTranslation)
        {
            var statement = new SqlServerStatement("RBOINVENTTRANSLATIONS");

            if (!Exists(entry, new RecordIdentifier(itemTranslation.ItemID, itemTranslation.LanguageID)))
            {
                itemTranslation.ID = Guid.NewGuid();
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", (Guid)itemTranslation.ID, SqlDbType.UniqueIdentifier);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ITEMID", (string)itemTranslation.ItemID);
                statement.AddCondition("CULTURENAME", (string)itemTranslation.LanguageID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("ITEMID", (string)itemTranslation.ItemID);
            statement.AddField("CULTURENAME", (string)itemTranslation.LanguageID);
            statement.AddField("DESCRIPTION", itemTranslation.Description);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier itemIDAndCultureName)
        {
            return RecordExists(entry, "RBOINVENTTRANSLATIONS", new[] { "ITEMID", "CULTURENAME" }, itemIDAndCultureName);
        }
    }
}
