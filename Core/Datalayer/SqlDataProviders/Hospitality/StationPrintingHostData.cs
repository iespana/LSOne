using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    public class StationPrintingHostData : SqlServerDataProviderBase, IStationPrintingHostData
    {
        private static string BaseSelectString
        {
            get
            {
                return "select " +
                    "ID, " +
                    "ISNULL(DESCRIPTION,'') as DESCRIPTION, " +
                    "ISNULL(ADDRESS,'') as ADDRESS, " +
                    "ISNULL(PORT,'') as PORT, " +
                    "ISNULL(CHARSET,'') as CHARSET, " +
	                "ISNULL(LOCKCODE,'') as LOCKCODE, " +
	                "ISNULL(DEBUGLEVELCON,'') as DEBUGLEVELCON, " +
	                "ISNULL(DEBUGLEVELFILE,'') as DEBUGLEVELFILE, " +
	                "ISNULL(DEBUGFILESIZE,'') as DEBUGFILESIZE, " +
                    "ISNULL(DEBUGFILECOUNT,'') as DEBUGFILECOUNT " +
                    "from STATIONPRINTINGHOSTS ";
            }
        }

        private static void PopulateStationPrintingHost(IDataReader dr, StationPrintingHost stprintHost)
        {
            stprintHost.ID = (string)dr["ID"];
            stprintHost.Text = (string)dr["DESCRIPTION"];
            stprintHost.Address = (string)dr["ADDRESS"];
            stprintHost.Port = (int)dr["PORT"];
            stprintHost.CharSet = (int)dr["CHARSET"];
            stprintHost.LockCode = (string)dr["LOCKCODE"];
            stprintHost.DebugLevelConsole = (int)dr["DEBUGLEVELCON"];
            stprintHost.DebugLevelFile = (int)dr["DEBUGLEVELFILE"];
            stprintHost.DebugFileSize = (int)dr["DEBUGFILESIZE"];
            stprintHost.DebugFileCount = (int)dr["DEBUGFILECOUNT"];
        }

        /// <summary>
        /// Gets a list of all Printing Station hosts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of StationPrintingHost objects containing all Printing Station host records</returns>
        public virtual List<StationPrintingHost> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSelectString + "where DATAAREAID = @dataAreaId";
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                return Execute<StationPrintingHost>(entry, cmd, CommandType.Text, PopulateStationPrintingHost);
            }
        }

        /// <summary>
        /// Gets the Printing Station host with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="stprintHostID">The ID of the Printing Station host to get</param>
        /// <returns>The StationPrintingHost with the given ID</returns>
        public virtual StationPrintingHost Get(IConnectionManager entry, RecordIdentifier stprintHostID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId and ID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)stprintHostID);

                var result = Execute<StationPrintingHost>(entry, cmd, CommandType.Text, PopulateStationPrintingHost);
                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Checks if a Printing Station host with the given ID exists.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="stprintHostID">The ID of the Printing Station host to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier stprintHostID)
        {
            return RecordExists(entry, "STATIONPRINTINGHOSTS", "ID", stprintHostID);
        }

        /// <summary>
        /// Deletes the Printing Station host with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="stprintHostID">The ID of the Printing Station host to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier stprintHostID)
        {
            DeleteRecord(entry, "STATIONPRINTINGHOSTS", "ID", stprintHostID, BusinessObjects.Permission.ManageRemoteHosts);
        }

        /// <summary>
        /// Saves a StationPrintingHost into the database. If the Printing Station host does not exists a new one is inserted, otherwise an update to the existsing record is made.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="stprintHost">The StationPrintingHost object to save</param>
        public virtual void Save(IConnectionManager entry, StationPrintingHost stprintHost)
        {
            var statement = new SqlServerStatement("STATIONPRINTINGHOSTS");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageSalesTypes);

            bool isNew = false;
            if (stprintHost.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                stprintHost.ID = DataProviderFactory.Instance.GenerateNumber<IStationPrintingHostData,StationPrintingHost>(entry);
            }

            if (isNew || !Exists(entry, stprintHost.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)stprintHost.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)stprintHost.ID);
            }

            statement.AddField("DESCRIPTION", stprintHost.Text);
            statement.AddField("ADDRESS", stprintHost.Address);
            statement.AddField("PORT", stprintHost.Port, SqlDbType.Int);
            statement.AddField("CHARSET", stprintHost.CharSet, SqlDbType.Int);
            statement.AddField("LOCKCODE", stprintHost.LockCode);
            statement.AddField("DEBUGLEVELCON", stprintHost.DebugLevelConsole, SqlDbType.Int);
            statement.AddField("DEBUGLEVELFILE", stprintHost.DebugLevelFile, SqlDbType.Int);
            statement.AddField("DEBUGFILESIZE", stprintHost.DebugFileSize, SqlDbType.Int);
            statement.AddField("DEBUGFILECOUNT", stprintHost.DebugFileCount, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }
        public RecordIdentifier SequenceID
        {
            get { return "STATIONPRINTINGHOSTS"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "STATIONPRINTINGHOSTS", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
