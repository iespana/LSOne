using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    public class HospitalityOperationData : SqlServerDataProviderBase, IHospitalityOperationData
    {
        private static string BaseSelectString
        {
            get
            {
                return "select " + 
                    "OPERATIONID," +
                    "ISNULL(OPERATIONNAME,'') as OPERATIONNAME," +
                    "ISNULL(PERMISSIONID,0) as PERMISSIONID," +
                    "ISNULL(CHECKUSERACCESS,0) as CHECKUSERACCESS," +
                    "ISNULL(ALLOWPARAMETER,0) as ALLOWPARAMETER," +                    
                    "ISNULL(NAVOPERATION,'') as NAVOPERATION, " +
                    "ISNULL(PARAMETERTYPE,0) as PARAMETERTYPE " +
                    "from POSISHOSPITALITYOPERATIONS ";
            }
        }

        private static void PopulateHospitalityOperation(IDataReader dr, HospitalityOperation hospitalityOperation)
        {
            hospitalityOperation.ID = (int)dr["OPERATIONID"];
            hospitalityOperation.Text = (string)dr["OPERATIONNAME"];
            hospitalityOperation.PermissionID = (int)dr["PERMISSIONID"];
            hospitalityOperation.CheckUserAccess = (byte)dr["CHECKUSERACCESS"] != 0;
            hospitalityOperation.AllowParameter = (byte)dr["ALLOWPARAMETER"] != 0;            
            hospitalityOperation.NavOperation = (string)dr["NAVOPERATION"];
            hospitalityOperation.ParameterType = (HospitalityOperation.ParameterTypeEnum)((int)dr["PARAMETERTYPE"]);
        }

        /// <summary>
        /// Gets a list of all hospitality operations
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of HospitalityOperation objects containing all hospitality operations</returns>
        public virtual List<HospitalityOperation> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId ";

                // Exclude the following operations:
                // * Next guest  = 501
                // * Mark guest  = 109
                // * Guests      = 500
                // * Print split = 301
                cmd.CommandText += " and OPERATIONID not in (501, 109, 500, 301)";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<HospitalityOperation>(entry, cmd, CommandType.Text, PopulateHospitalityOperation);
            }
        }

        /// <summary>
        /// Gets a hospitality operation with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hospitalityOperationID">The ID of the hospitality operation to get</param>
        /// <returns>The hospitality operation with the given ID</returns>
        public virtual HospitalityOperation Get(IConnectionManager entry, RecordIdentifier hospitalityOperationID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId and OPERATIONID = @operationId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "operationId", (int) hospitalityOperationID);

                var result = Execute<HospitalityOperation>(entry, cmd, CommandType.Text,
                    PopulateHospitalityOperation);

                return result.Count > 0 ? result[0] : null;
            }
        }
    }
}
