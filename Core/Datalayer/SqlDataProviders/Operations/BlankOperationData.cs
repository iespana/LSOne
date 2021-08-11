using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders.Operations;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Operations
{
    public class BlankOperationData : SqlServerDataProviderBase, IBlankOperationData
    {       
        public virtual List<BlankOperation> GetBlankOperations(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                const string sqlString = " SELECT ID, ISNULL(OPERATIONPARAMETER,'') as OPERATIONPARAMETER, "
                                         + " ISNULL(OPERATIONDESCRIPTION,'') as OPERATIONDESCRIPTION, MGRPERMISSIONREQ, OPERATIONCREATEDONPOS "
                                         + " FROM POSISBLANKOPERATIONS "
                                         + " WHERE DATAAREAID = @dataAreaId ORDER BY ID";

                cmd.CommandText = sqlString;
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                return Execute<BlankOperation>(entry, cmd, CommandType.Text, PopulateBlankOperation);
            }
        }
     
        private static void PopulateBlankOperation(IDataReader dr, BlankOperation item)
        {
            item.ID = (string)dr["ID"];
            item.OperationParameter = (string)dr["OPERATIONPARAMETER"];
            item.OperationDescription = (string)dr["OPERATIONDESCRIPTION"];

            if (dr["OPERATIONCREATEDONPOS"] == DBNull.Value)
            {
                item.CreatedOnPOS = false;
            }
            else
            {
                item.CreatedOnPOS = Convert.ToBoolean(dr["OPERATIONCREATEDONPOS"]);
            }
        }

        public virtual BlankOperation Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = " SELECT ID, ISNULL(OPERATIONPARAMETER,'') as OPERATIONPARAMETER, "
                                + " ISNULL(OPERATIONDESCRIPTION,'') as OPERATIONDESCRIPTION, OPERATIONCREATEDONPOS "
                                + " FROM POSISBLANKOPERATIONS " +
                                   "WHERE DATAAREAID = @dataAreaId and ID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", id);
                var blankOperations = Execute<BlankOperation>(entry, cmd, CommandType.Text, PopulateBlankOperation);

                return (blankOperations.Count > 0) ? blankOperations[0] : null;
            }
        }

        public virtual void Save(IConnectionManager entry, BlankOperation blankOperation)
        {
            var statement = new SqlServerStatement("POSISBLANKOPERATIONS");

            ValidateSecurity(entry, BusinessObjects.Permission.AdministrationMaintainSettings);            

            if (!Exists(entry, blankOperation.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", (string)blankOperation.ID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (string)blankOperation.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("OPERATIONPARAMETER", blankOperation.OperationParameter);
            statement.AddField("MGRPERMISSIONREQ", blankOperation.ManagerPermissionRequired ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("OPERATIONDESCRIPTION", blankOperation.OperationDescription);
            statement.AddField("OPERATIONCREATEDONPOS", blankOperation.CreatedOnPOS ? 1 : 0, SqlDbType.TinyInt);
            
            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "Blank operations"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSISBLANKOPERATIONS", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier operationId)
        {
            return RecordExists(entry, "POSISBLANKOPERATIONS", "ID", operationId);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "POSISBLANKOPERATIONS", "ID", (string)id, BusinessObjects.Permission.AdministrationMaintainSettings);
        }
    }
}
