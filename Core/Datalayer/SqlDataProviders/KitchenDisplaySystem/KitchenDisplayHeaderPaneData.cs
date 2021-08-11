using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using static LSOne.Utilities.DataTypes.RecordIdentifier;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayHeaderPaneData : SqlServerDataProviderBase, IKitchenDisplayHeaderPaneData
    {
        private string BaseSelectString
        {
            get
            {
                return @" SELECT ID,
                                 NAME
					      FROM KITCHENDISPLAYHEADERPANE ";
            }
        }

        public virtual HeaderPaneProfile Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = 
                    BaseSelectString + 
                    "WHERE ID = @id";

                MakeParam(cmd, "id", (Guid)id);

                var result = Execute<HeaderPaneProfile>(entry, cmd, CommandType.Text, PopulateHeaderPane);

                if(result.Count == 1)
                {
                    AddLines(entry, result[0]);
                    return result[0];
                }

                return null;
            }
        }

        public virtual List<HeaderPaneProfile> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSelectString;

                List<HeaderPaneProfile> headerPanes = Execute<HeaderPaneProfile>(entry, cmd, CommandType.Text, PopulateHeaderPane);
                headerPanes.ForEach(h => AddLines(entry, h));
                return headerPanes;
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYHEADERPANE", "ID", id, Permission.ManageKitchenDisplayStations, false);

            Providers.KitchenDisplayHeaderPaneLineData.DeleteByHeaderPane(entry, id);
            Providers.KitchenDisplayHeaderPaneLineColumnData.DeleteByHeaderPane(entry, id);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYHEADERPANE", "ID", id, false);
        }

        public virtual void Save(IConnectionManager entry, HeaderPaneProfile headerPane)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYHEADERPANE");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            if (!Exists(entry, headerPane.ID))
            {
                statement.StatementType = StatementType.Insert;
                if (RecordIdentifier.IsEmptyOrNull(headerPane.ID))
                {
                    headerPane.ID = Guid.NewGuid();
                }
                statement.AddKey("ID", (Guid)headerPane.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (Guid)headerPane.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("NAME", headerPane.Name);

            Save(entry, headerPane, statement);
        }

        private void PopulateHeaderPane(IDataReader dr, HeaderPaneProfile headerPane)
        {
            headerPane.ID = headerPane.ProfileId = (Guid)dr["ID"];
            headerPane.Name = headerPane.Text = (string)dr["NAME"];
        }

        private void AddLines(IConnectionManager entry,  HeaderPaneProfile headerPane)
        {
            headerPane.HeaderLines.AddRange(Providers.KitchenDisplayHeaderPaneLineData.GetList(entry, headerPane.ID));
        }
    }
}