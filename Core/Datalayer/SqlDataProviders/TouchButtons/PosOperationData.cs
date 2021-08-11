using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.TouchButtons
{
    public class PosOperationData : SqlServerDataProviderBase, IPosOperationData
    {
        private static string BaseSelectString
        {
            get
            {
                return @"SELECT
                         MASTERID,
                         ID,
                         ISNULL(DESCRIPTION,'') as DESCRIPTION,
                         ISNULL(TYPE, 0) as TYPE,
                         ISNULL(LOOKUPTYPE, 0) as LOOKUPTYPE,
                         ISNULL(AUDIT, 0) as AUDIT
                         from OPERATIONS ";
            }
        }

        private static string ResolveSort(PosOperationSorting sort, bool backwards)
        {
            switch (sort)
            {
                case PosOperationSorting.OperationID:
                    return backwards ? "ID DESC" : "ID ASC";

                case PosOperationSorting.OperationName:
                    return backwards ? "DESCRIPTION DESC" : "DESCRIPTION ASC";
            }

            return "";
        }

        private static void PopulatePosOperation(IDataReader dr, PosOperation posOperation)
        {
            posOperation.MasterID = AsGuid(dr["MASTERID"]);
            posOperation.ID = (string) dr["ID"];
            posOperation.OperationDBName = (string) dr["DESCRIPTION"];
            posOperation.Type = (OperationTypeEnum) ((int)dr["TYPE"]);
            posOperation.LookupType = (LookupTypeEnum) ((int) dr["LOOKUPTYPE"]);
            posOperation.Audit = (OperationAuditEnum) (AsInt(dr["AUDIT"]));
        }

        /// <summary>
        /// Returns a list of all pos user operations ordered by the specified field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of PosOperation objects conataining all user pos operation records, ordered by the chosen field</returns>
        public virtual List<PosOperation> GetUserOperations(IConnectionManager entry, PosOperationSorting sortBy = PosOperationSorting.OperationName, bool sortBackwards = false)
        {
            if (sortBy != PosOperationSorting.OperationID && sortBy != PosOperationSorting.OperationName)
            {
                throw new NotSupportedException();
            }

            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText =
                    BaseSelectString +
                    "WHERE TYPE IN (0, 2) " + //All and PosOperations
                    "ORDER BY " + ResolveSort(sortBy, sortBackwards);

                return Execute<PosOperation>(entry, cmd, CommandType.Text, PopulatePosOperation);
            }
        }

        public virtual List<PosOperation> GetList(IConnectionManager entry, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    " ORDER BY DESCRIPTION ";
                
                cmd.CommandType = CommandType.Text;
                return GetList<PosOperation>(entry, cmd, (string) "AllOperations", PopulatePosOperation, cache);
            }
        }

        public virtual List<PosOperation> GetList(IConnectionManager entry, OperationTypeEnum type, PosOperationSorting sortBy = PosOperationSorting.OperationName, bool sortBackwards = false)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "WHERE TYPE IN (0, @type) " +
                    "ORDER BY " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "type", (int)type);
                return Execute<PosOperation>(entry, cmd, CommandType.Text, PopulatePosOperation);
            }
        }

        public virtual List<PosOperation> GetAuditableList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    " WHERE AUDIT = 1";

                return Execute<PosOperation>(entry, cmd, CommandType.Text, PopulatePosOperation);
            }
        }

        public virtual bool OperationsExists(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TOP 1 'x' FROM OPERATIONS WHERE TYPE IN (0, 2)";
                using (var dr = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                    return !((SqlDataReader)dr).HasRows;
            }
        }

        public virtual List<DataEntity> GetPaymentOperations(IConnectionManager entry)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select ID, DESCRIPTION from OPERATIONS where ID >= 200 and ID <= 220 AND ID NOT IN (206, 211, 216) and TYPE = 2 order by DESCRIPTION";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "DESCRIPTION", "ID");
            }
        }  

        /// <summary>
        /// Gets a pos operation with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posOperationID">The ID of the operation to get</param>
        /// <param name="cache">The type of caching to be used</param>
        /// <returns></returns>
        public virtual PosOperation Get(IConnectionManager entry, RecordIdentifier posOperationID, CacheType cache = CacheType.CacheTypeNone)
        {
            if(RecordIdentifier.IsEmptyOrNull(posOperationID))
            {
                return null;
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "WHERE ID = @operationId";

                MakeParam(cmd, "operationId", posOperationID);

                return Get<PosOperation>(entry, cmd, posOperationID, PopulatePosOperation, cache, UsageIntentEnum.Normal);
            }
        }
    }
}
