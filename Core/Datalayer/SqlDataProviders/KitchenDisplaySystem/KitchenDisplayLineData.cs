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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    /// <summary>
    /// Data provider class for KitchenDisplayLines
    /// </summary>
    public class KitchenDisplayLineData : SqlServerDataProviderBase, IKitchenDisplayLineData
    {
        private static string BaseSelectString
        {
            get { return @"select
                        ID,
                        DISPLAYPROFILEID,
                        LINETYPE,
                        LINENUMBER
                        FROM KITCHENDISPLAYLINE "; }
        }

        private static void PopulateKitchenDisplayLine(IDataReader dr, KitchenDisplayLine displayLine)
        {
            displayLine.ID = (Guid)dr["ID"];
            if (dr["DISPLAYPROFILEID"] != DBNull.Value)
            {
                displayLine.DisplayProfileID = (Guid)dr["DISPLAYPROFILEID"];
            }
            displayLine.Type = (KitchenDisplayStation.DisplayRowTypeEnum)AsInt(dr["LINETYPE"]);
            displayLine.No = (int)dr["LINENUMBER"];
        }

        /// <summary>
        /// Gets a list of all displaylines for the selected displayprofile
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of PosMenuHeader objects containing all pos menu header records</returns>
        public virtual List<KitchenDisplayLine> GetList(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DISPLAYPROFILEID = @displayProfileId and DATAAREAID = @dataAreaId " +
                    "order by LINETYPE, LINENUMBER";

                MakeParam(cmd, "displayProfileId", (Guid)id.PrimaryID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<KitchenDisplayLine>(entry, cmd, CommandType.Text, PopulateKitchenDisplayLine);
            }
        }


        /// <summary>
        /// Gets a list of all displaylines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all displaylines</returns>
        public virtual List<KitchenDisplayLine> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                // As of today (2020-11-20), the KDS display stations display the lines in the order it receives them, regardless of the LINENUMBER value
                // Therefore, ordering correctly here is important
                cmd.CommandText = BaseSelectString + "order by DISPLAYPROFILEID, LINETYPE, LINENUMBER";
                return Execute<KitchenDisplayLine>(entry, cmd, CommandType.Text, PopulateKitchenDisplayLine);
            }
        }

        /// <summary>
        /// Gets as pos menu header with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeaderID">The ID of the pos menu header to get</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>A PosMenuHeader object containing the pos menu header with the given ID</returns>
        public virtual KitchenDisplayLine Get(IConnectionManager entry, RecordIdentifier id, RecordIdentifier displayProfileID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where ID = @id and DISPLAYPROFILEID = @displayProfileId and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "id", (Guid)id.PrimaryID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "displayProfileId", (Guid)displayProfileID.PrimaryID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var results = Execute<KitchenDisplayLine>(entry, cmd, CommandType.Text, PopulateKitchenDisplayLine);

                return (results.Count == 1) ? results[0] : null;
            }
        }

        /// <summary>
        /// Checks if a kitchendisplayline with the given ID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the line to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYLINE", "ID", id);
        }

        /// <summary>
        /// Deletes the kitchendisplayline with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the pos menu header to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<KitchenDisplayLine>(entry, "KITCHENDISPLAYLINE", "ID", id, BusinessObjects.Permission.ManageKitchenDisplayProfiles);
            Providers.KitchenDisplayLineColumnData.DeleteByDisplayLine(entry, id);
        }

        public virtual short MaxLineNo(IConnectionManager entry, KitchenDisplayLine displayLine)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select max(LINENUMBER) from KITCHENDISPLAYLINE where DISPLAYPROFILEID=@dpId AND DATAAREAID=@dataAreaId and LINETYPE=@lt";

                MakeParam(cmd, "dpId", (Guid)displayLine.DisplayProfileID.PrimaryID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "lt", displayLine.Type, SqlDbType.TinyInt);

                object result;
                lock (entry)
                {
                    result = entry.Connection.ExecuteScalar(cmd);
                }

                if (result == DBNull.Value || result == null)
                {
                    return 0;
                }
                short i = Convert.ToInt16(result);
                return i;
            }
        }

        /// <summary>
        /// Saves a kitchendisplayline object into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeader"></param>
        public virtual void Save(IConnectionManager entry, KitchenDisplayLine displayLine)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYLINE");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageKitchenDisplayProfiles);

            bool isNew = false;
            if (displayLine.ID.IsEmpty)
            {
                displayLine.ID = Guid.NewGuid();
                isNew = true;
            }

            if (isNew || !Exists(entry, displayLine.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", ((string)displayLine.ID).ToUpper());
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddField("DISPLAYPROFILEID", ((string)displayLine.DisplayProfileID).ToUpper());
                statement.AddField("LINENUMBER", MaxLineNo(entry, displayLine) + 1, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (string)displayLine.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("DISPLAYPROFILEID", ((string)displayLine.DisplayProfileID).ToUpper());
            }

            statement.AddField("LINETYPE", displayLine.Type, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
