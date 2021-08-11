using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.LookupValues
{
    public class RemoteHostData : SqlServerDataProviderBase, IRemoteHostData
    {
        private static string BaseSelectString
        {
            get
            {
                return "select " +
                    "ID, " +
                    "ISNULL(DESCRIPTION,'') as DESCRIPTION," +
                    "ISNULL(ADDRESS,'') as ADDRESS," +
                    "ISNULL(PORT,'') as PORT " +
                    "from REMOTEHOSTS ";
            }
        }

        private static void PopulateRemoteHost(IDataReader dr, RemoteHost remoteHost)
        {
            remoteHost.ID = (string)dr["ID"];
            remoteHost.Text = (string)dr["DESCRIPTION"];
            remoteHost.Address = (string)dr["ADDRESS"];
            remoteHost.Port = (int)dr["PORT"];
        }

        /// <summary>
        /// Gets a list of all remote hosts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of RemoteHost objects containing all remote host records</returns>
        public virtual List<RemoteHost> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<RemoteHost>(entry, cmd, CommandType.Text, PopulateRemoteHost);
            }
        }

        /// <summary>
        /// Gets the remote host with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="remoteHostID">The ID of the remote host to get</param>
        /// <returns>The RemoteHost with the given ID</returns>
        public virtual RemoteHost Get(IConnectionManager entry, RecordIdentifier remoteHostID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId and ID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string) remoteHostID);

                var result = Execute<RemoteHost>(entry, cmd, CommandType.Text, PopulateRemoteHost);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Checks if a remote host with the given ID exists.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="remoteHostID">The ID of the remote host to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier remoteHostID)
        {
            return RecordExists(entry, "REMOTEHOSTS", "ID", remoteHostID);
        }

        /// <summary>
        /// Deletes the remote host with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="remoteHostID">The ID of the remote host to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier remoteHostID)
        {
            DeleteRecord(entry, "REMOTEHOSTS", "ID", remoteHostID, BusinessObjects.Permission.ManageRemoteHosts);
        }

        /// <summary>
        /// Saves a RemoteHost into the database. If the remote host does not exists a new one is inserted, otherwise an update to the existsing record is made.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="remoteHost">The RemoteHost object to save</param>
        public virtual void Save(IConnectionManager entry, RemoteHost remoteHost)
        {
            var statement = new SqlServerStatement("REMOTEHOSTS");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageSalesTypes);

            bool isNew = false;
            if (remoteHost.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                remoteHost.ID = DataProviderFactory.Instance.GenerateNumber<IRemoteHostData,RemoteHost>(entry);
            }

            if (isNew || !Exists(entry, remoteHost.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)remoteHost.ID);

            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)remoteHost.ID);
            }

            statement.AddField("DESCRIPTION", remoteHost.Text);
            statement.AddField("ADDRESS", remoteHost.Address);
            statement.AddField("PORT", remoteHost.Port, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "REMOTEHOSTS"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "REMOTEHOSTS", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
