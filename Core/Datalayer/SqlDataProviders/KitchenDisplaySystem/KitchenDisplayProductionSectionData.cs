using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders;
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
    public class KitchenDisplayProductionSectionData : SqlServerDataProviderBase, IKitchenDisplayProductionSectionData
    {
        private string BaseSelectString
        {
            get
            {
                return @" SELECT ID,
                                 CODE,
                                 DESCRIPTION
					      FROM KITCHENDISPLAYPRODUCTIONSECTION ";
            }
        }

        public virtual KitchenDisplayProductionSection Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = 
                    BaseSelectString + 
                    "WHERE ID = @id";

                MakeParam(cmd, "id", (Guid)id);

                var result = Execute<KitchenDisplayProductionSection>(entry, cmd, CommandType.Text, PopulateProductionSection);
                return (result.Count == 1) ? result[0] : null;
            }
        }

        public virtual List<KitchenDisplayProductionSection> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSelectString;

                return Execute<KitchenDisplayProductionSection>(entry, cmd, CommandType.Text, PopulateProductionSection);
            }
        }

        public virtual List<MasterIDEntity> GetMasterIDList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSelectString;

                return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateMasterID);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYPRODUCTIONSECTION", "ID", id, Permission.ManageKitchenDisplayStations, false);

            // Remove section from item section routings
            Providers.KitchenDisplayItemSectionRoutingData.RemoveSection(entry, id);

            // Remove section from section station routing
            Providers.KitchenDisplaySectionStationRoutingData.RemoveSection(entry, id);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYPRODUCTIONSECTION", "ID", id, false);
        }

        public virtual bool Exists(IConnectionManager entry, string code)
        {
            return RecordExists(entry, "KITCHENDISPLAYPRODUCTIONSECTION", "CODE", code, false);
        }

        public virtual void Save(IConnectionManager entry, KitchenDisplayProductionSection productionSection)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYPRODUCTIONSECTION");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            if (!Exists(entry, productionSection.ID))
            {
                statement.StatementType = StatementType.Insert;
                if (RecordIdentifier.IsEmptyOrNull(productionSection.ID))
                {
                    productionSection.ID = Guid.NewGuid();
                }
                statement.AddKey("ID", (Guid)productionSection.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (Guid)productionSection.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("CODE", (string)productionSection.Code);
            statement.AddField("DESCRIPTION", productionSection.Description);

            Save(entry, productionSection, statement);
        }

        private void PopulateProductionSection(IDataReader dr, KitchenDisplayProductionSection productionSection)
        {
            productionSection.ID = (Guid)dr["ID"];
            productionSection.Code = (string)dr["CODE"];
            productionSection.Description = (string)dr["DESCRIPTION"];
        }

        private void PopulateMasterID(IDataReader dr, MasterIDEntity section)
        {
            section.ID = (Guid)dr["ID"];
            section.ReadadbleID = (string)dr["CODE"];
            section.Text = (string)dr["DESCRIPTION"];
        }
    }
}