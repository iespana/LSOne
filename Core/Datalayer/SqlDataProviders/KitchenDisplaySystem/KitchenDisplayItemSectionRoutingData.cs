using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;


namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayItemSectionRoutingData : SqlServerDataProviderBase, IKitchenDisplayItemSectionRoutingData
    {
        private string BaseSelectString
        {
            get
            {
                return @"   SELECT kdisr.ID,
                                   kdisr.ITEMTYPE,
                                   kdisr.ITEMMASTERID,
                                   kdisr.SECTIONID,
                                   kdps.CODE as SECTIONCODE,
                                   kdps.DESCRIPTION as SECTIONDESCRIPTION,
                                   ri.ITEMID,
                                   rg.GROUPID as RETAILGROUPID,
                                   sg.GROUPID as SPECIALGROUPID,
                                   ISNULL(ri.ITEMNAME, '') as ITEMNAME,
                                   ISNULL(rg.NAME, '') as RETAILGROUPNAME,
                                   ISNULL(sg.NAME, '') as SPECIALGROUPNAME
                            FROM KITCHENDISPLAYITEMSECTIONROUTING kdisr
                            LEFT JOIN RETAILITEM ri on ri.MASTERID = kdisr.ITEMMASTERID 
                            LEFT JOIN RETAILGROUP rg on rg.MASTERID = kdisr.ITEMMASTERID 
                            LEFT JOIN SPECIALGROUP sg on sg.MASTERID = kdisr.ITEMMASTERID 
                            LEFT JOIN KITCHENDISPLAYPRODUCTIONSECTION kdps on kdps.ID = kdisr.SECTIONID 
                            WHERE (ri.DELETED = 0 OR 
                                  rg.DELETED = 0 OR 
                                  sg.DELETED = 0) ";
            }
        }

        private void PopulateItemSectionRouting(IDataReader dr, KitchenDisplayItemSectionRouting routing)
        {
            routing.ID = (Guid)dr["ID"];
            routing.ItemType = (KitchenDisplayItemSectionRouting.ItemTypeEnum)(byte)dr["ITEMTYPE"];
            routing.ItemMasterId = (Guid)dr["ITEMMASTERID"];

            if (!(dr["SECTIONID"] is DBNull))
            {
                routing.SectionId = (Guid)dr["SECTIONID"];
                routing.SectionCode = (string)dr["SECTIONCODE"];
                routing.SectionDescription = (string)dr["SECTIONDESCRIPTION"];
            }

            switch (routing.ItemType)
            {
                case KitchenDisplayItemSectionRouting.ItemTypeEnum.RetailGroup:
                    routing.ItemId = (string)dr["RETAILGROUPID"];
                    routing.ItemDescription = (string)dr["RETAILGROUPNAME"];
                    break;
                case KitchenDisplayItemSectionRouting.ItemTypeEnum.SpecialGroup:
                    routing.ItemId = (string)dr["SPECIALGROUPID"];
                    routing.ItemDescription = (string)dr["SPECIALGROUPNAME"];
                    break;
                case KitchenDisplayItemSectionRouting.ItemTypeEnum.Item:
                    routing.ItemId = (string)dr["ITEMID"];
                    routing.ItemDescription = (string)dr["ITEMNAME"];
                    break;
            }
        }

        public virtual KitchenDisplayItemSectionRouting Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseSelectString +
                    "AND kdisr.ID = @routingId";

                MakeParam(cmd, "routingId", (Guid)id);

                var result = Execute<KitchenDisplayItemSectionRouting>(entry, cmd, CommandType.Text, PopulateItemSectionRouting);
                return (result.Count == 1) ? result[0] : null;
            }
        }

        public virtual List<KitchenDisplayItemSectionRouting> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSelectString;

                return Execute<KitchenDisplayItemSectionRouting>(entry, cmd, CommandType.Text, PopulateItemSectionRouting);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYITEMSECTIONROUTING", "ID", id, BusinessObjects.Permission.ManageKitchenDisplayStations, false);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYITEMSECTIONROUTING", "ID", id, false);
        }

        public virtual bool Exists(IConnectionManager entry, KitchenDisplayItemSectionRouting.ItemTypeEnum type, RecordIdentifier itemId, RecordIdentifier sectionId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT ID
					                FROM KITCHENDISPLAYITEMSECTIONROUTING 
                                    WHERE ITEMMASTERID = @itemId AND 
                                          SECTIONID = @sectionId AND 
                                          ITEMTYPE = @type";

                MakeParam(cmd, "itemId", (Guid)itemId);
                MakeParam(cmd, "sectionId", (Guid)sectionId);
                MakeParam(cmd, "type", (byte)type);

                List<RecordIdentifier> result = Execute(entry, cmd, CommandType.Text, "ID");
                return result.Count == 1;
            }
        }

        public virtual void Save(IConnectionManager entry, KitchenDisplayItemSectionRouting routing)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYITEMSECTIONROUTING");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageKitchenDisplayStations);

            if (!Exists(entry, routing.ID))
            {
                statement.StatementType = StatementType.Insert;
                if (RecordIdentifier.IsEmptyOrNull(routing.ID))
                {
                    routing.ID = Guid.NewGuid();
                }
                statement.AddKey("ID", (Guid)routing.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (Guid)routing.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("ITEMTYPE", (byte)routing.ItemType, SqlDbType.TinyInt);
            statement.AddField("ITEMMASTERID", (Guid)routing.ItemMasterId, SqlDbType.UniqueIdentifier);
            statement.AddField("SECTIONID", (Guid)routing.SectionId, SqlDbType.UniqueIdentifier);

            Save(entry, routing, statement);
        }

        public virtual void RemoveSection(IConnectionManager entry, RecordIdentifier sectionId)
        {
            // Set section ids to null in item section routings
            SqlServerStatement statement = new SqlServerStatement("KITCHENDISPLAYITEMSECTIONROUTING");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            statement.StatementType = StatementType.Update;
            statement.AddCondition("SECTIONID", (Guid)sectionId, SqlDbType.UniqueIdentifier);
            statement.AddField("SECTIONID", DBNull.Value, SqlDbType.UniqueIdentifier);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}