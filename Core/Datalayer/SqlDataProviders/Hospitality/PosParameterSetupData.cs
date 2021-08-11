using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    public class PosParameterSetupData : SqlServerDataProviderBase, IPosParameterSetupData
    {
        private static string BaseSelectString
        {
            get
            {
                return "select " +
                    "OPERATIONID," +
                    "ISNULL(PARAMETERCODE,'') as PARAMETERCODE," +
                    "ISNULL(DESCRIPTION,'') as DESCRIPTION " +
                    "from POSPARAMETERSETUP ";
            }
        }

        private static void PopulateParameterSetup(IDataReader dr, PosParameterSetup posParameterSetup)
        {
            posParameterSetup.OperationID = (int)dr["OPERATIONID"];
            posParameterSetup.ParameterCode = (string)dr["PARAMETERCODE"];
            posParameterSetup.Text = (string)dr["DESCRIPTION"];
        }

        /// <summary>
        /// Gets all pos parameter setup lines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of PosParameterSetup objects containing all pos marameter setup lines</returns>
        public virtual List<PosParameterSetup> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PosParameterSetup>(entry, cmd, CommandType.Text, PopulateParameterSetup);
            }
        }

        /// <summary>
        /// Gets a list of parameters for a given operation ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="operationId">The id of the operation</param>
        /// <returns>A list of all parameters for the given operation</returns>
        public virtual List<PosParameterSetup> GetList(IConnectionManager entry, RecordIdentifier operationId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId and OPERATIONID = @operationId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "operationId", (int) operationId, SqlDbType.Int);

                return Execute<PosParameterSetup>(entry, cmd, CommandType.Text, PopulateParameterSetup);
            }
        }
        
        /// <summary>
        /// Gets a pos parameter setup with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posParameterSetupID">The ID of the pos parameter setup to get (OperationID,ParameterCode)</param>
        /// <returns></returns>
        public virtual PosParameterSetup Get(IConnectionManager entry, RecordIdentifier posParameterSetupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId and OPERATIONID = @operationId and PARAMETERCODE = @parameterCode";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "operationId", (int)posParameterSetupID[0]);
                MakeParam(cmd, "parameterCode", (string)posParameterSetupID[1]);

                var result = Execute<PosParameterSetup>(entry, cmd, CommandType.Text, PopulateParameterSetup);

                return result.Count > 0 ? result[0] : null;
            }
        }
    }
}
