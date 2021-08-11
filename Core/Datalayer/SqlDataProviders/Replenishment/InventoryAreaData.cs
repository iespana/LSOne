using LSOne.DataLayer.DataProviders.Replenishment;
using LSOne.DataLayer.SqlConnector.DataProviders;
using System;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.SqlConnector;
using System.Data;
using LSOne.DataLayer.GenericConnector.Enums;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;

namespace LSOne.DataLayer.SqlDataProviders.Replenishment
{
    public class InventoryAreaData : SqlServerDataProviderBase, IInventoryAreaData
    {
        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            DeleteRecord(entry, "INVENTORYAREA", "MASTERID", ID, "", false);
            DeleteRecord(entry, "INVENTORYAREALINES", "AREAID", ID, "", false);
        }

        public void DeleteLine(IConnectionManager entry, RecordIdentifier lineID)
        {
            DeleteRecord(entry, "INVENTORYAREALINES", "MASTERID", lineID, "", false);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            return RecordExists(entry, "INVENTORYAREA", "MASTERID", ID, false);
        }

        public virtual InventoryArea Get(IConnectionManager entry, RecordIdentifier areaID, UsageIntentEnum usage = UsageIntentEnum.Normal)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT A.MASTERID,
                                               A.DESCRIPTION,
                                               A.TYPE
                                        FROM INVENTORYAREA A 
                                        WHERE A.MASTERID = @id";

                MakeParam(cmd, "id", (Guid)areaID, SqlDbType.UniqueIdentifier);

                InventoryArea inventoryArea = Get<InventoryArea>(entry, cmd, areaID, PopulateInventoryArea, CacheType.CacheTypeNone, usage);

                if (usage != UsageIntentEnum.Minimal && inventoryArea != null)
                {
                    inventoryArea.InventoryAreaLines = GetLinesByArea(entry, inventoryArea.ID);
                }

                return inventoryArea;
            }
        }

        public virtual List<InventoryArea> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT A.MASTERID,
                                               A.DESCRIPTION,
                                               A.TYPE
                                        FROM INVENTORYAREA A ORDER BY A.DESCRIPTION ";

                return Execute<InventoryArea>(entry, cmd, CommandType.Text, PopulateInventoryArea);
            }
        }

        public virtual List<InventoryAreaLineListItem> GetAllAreaLines(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT A.DESCRIPTION AS AREADESCRIPTION, 
                                           L.DESCRIPTION AS LINEDESCRIPTION, 
                                           L.MASTERID 
                                    FROM INVENTORYAREALINES L
                                    LEFT JOIN INVENTORYAREA A ON A.MASTERID = L.AREAID
                                    ORDER BY A.DESCRIPTION, L.DESCRIPTION ";

                return Execute<InventoryAreaLineListItem>(entry, cmd, CommandType.Text, PopulateInventoryAreaListItem);
            }
        }

        private static void PopulateInventoryAreaListItem(IDataReader dr, InventoryAreaLineListItem area)
        {
            area.ID = AsGuid(dr["MASTERID"]);
            area.AreaDescription = (string)dr["AREADESCRIPTION"];
            area.AreaLineDescription = (string)dr["LINEDESCRIPTION"];
        }

        private static void PopulateInventoryArea(IDataReader dr, InventoryArea area)
        {
            area.ID = AsGuid(dr["MASTERID"]);
            area.Text = (string)dr["DESCRIPTION"];
            area.Type = (int)dr["TYPE"];
        }

        private static void PopulateInventoryAreaLine(IDataReader dr, InventoryAreaLine line)
        {
            line.ID = AsGuid(dr["MASTERID"]);
            line.Text = (string)dr["DESCRIPTION"];
            line.AreaID = AsGuid(dr["AREAID"]);
        }

        public virtual void Save(IConnectionManager entry, InventoryArea item)
        {
            var statement = new SqlServerStatement("INVENTORYAREA");

            ValidateSecurity(entry);

            if((Guid)item.ID == Guid.Empty)
            {
                item.ID = Guid.NewGuid();
                statement.StatementType = StatementType.Insert;
                statement.AddKey("MASTERID", (Guid)item.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("MASTERID", (Guid)item.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("DESCRIPTION", item.Text);
            statement.AddField("TYPE", item.Type, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SaveLine(IConnectionManager entry, InventoryAreaLine line)
        {
            var statement = new SqlServerStatement("INVENTORYAREALINES");

            ValidateSecurity(entry);

            if ((Guid)line.ID == Guid.Empty)
            {
                line.ID = Guid.NewGuid();
                statement.StatementType = StatementType.Insert;
                statement.AddKey("MASTERID", (Guid)line.ID, SqlDbType.UniqueIdentifier);
                statement.AddKey("AREAID", line.AreaID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("MASTERID", (Guid)line.ID, SqlDbType.UniqueIdentifier);
                statement.AddCondition("AREAID", line.AreaID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("DESCRIPTION", line.Text);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual InventoryAreaLine GetLine(IConnectionManager entry, RecordIdentifier masterID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT L.MASTERID,
                                           L.DESCRIPTION,
                                           L.AREAID
                                        FROM INVENTORYAREALINES L 
                                        WHERE L.MASTERID = @id";

                Guid id = Guid.Empty;
                Guid.TryParse(masterID.StringValue, out id);

                MakeParam(cmd, "id", id, SqlDbType.UniqueIdentifier);

                return Get<InventoryAreaLine>(entry, cmd, masterID, PopulateInventoryAreaLine, CacheType.CacheTypeNone, UsageIntentEnum.Normal);
            }
        }

        public virtual List<InventoryAreaLine> GetLinesByArea(IConnectionManager entry, RecordIdentifier areaID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT L.MASTERID,
                                           L.DESCRIPTION,
                                           L.AREAID
                                        FROM INVENTORYAREALINES L 
                                        WHERE L.AREAID = @id
                                        ORDER BY L.DESCRIPTION";

                MakeParam(cmd, "id", (Guid)areaID, SqlDbType.UniqueIdentifier);

                return Execute<InventoryAreaLine>(entry, cmd, CommandType.Text, PopulateInventoryAreaLine);
            }
        }

        public virtual bool AreaInUse(IConnectionManager entry, RecordIdentifier areaLineID)
        {
            return RecordExists(entry, "INVENTJOURNALTRANS", "AREA", areaLineID.ToString(), true, false);
        }
    }
}
