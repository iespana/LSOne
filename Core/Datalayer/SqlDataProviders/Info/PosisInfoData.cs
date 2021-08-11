using System;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders.Info;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Info
{
    public class PosisInfoData : SqlServerDataProviderBase, IPosisInfoData
    {
        public virtual string Get(IConnectionManager entry, string id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ISNULL(TEXT, '') TEXT FROM POSISINFO WHERE ID = @id";
                MakeParam(cmd, "id", id);
                
                using (var reader = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                {
                    if (reader != null && reader.Read())
                    {
                        var res = reader["TEXT"];
                        if (res != null && res != DBNull.Value)
                            return (string) res;
                    }
                }
            }

            return string.Empty;
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM POSISINFO WHERE ID = @id";
                MakeParam(cmd, "id", (string)id);

                entry.Connection.ExecuteNonQuery(cmd, false, CommandType.Text);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotImplementedException();
        }

        public virtual void Save(IConnectionManager entry, DataEntity item)
        {
            ValidateSecurity(entry);

            int count;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) from POSISINFO WHERE ID = @id";
                MakeParam(cmd, "id", (string)item.ID);

                count = (int)entry.Connection.ExecuteScalar(cmd);
            } 

            using (var cmd = entry.Connection.CreateCommand())
            {
                if (count == 0)
                    cmd.CommandText = "INSERT INTO POSISINFO (ID, TEXT) VALUES(@id,@text)";
                else
                    cmd.CommandText = "UPDATE POSISINFO SET TEXT=@text WHERE ID=@id";

                MakeParam(cmd, "id", (string)item.ID);
                MakeParam(cmd, "text", item.Text);

                entry.Connection.ExecuteNonQuery(cmd, false, CommandType.Text);
            }
        }
    }
}
