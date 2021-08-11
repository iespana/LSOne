using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Transactions
{
    public class SuspendedTransactionTypeData : SqlServerDataProviderBase, ISuspendedTransactionTypeData
    {
        public virtual List<SuspendedTransactionType> GetList(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ID, ISNULL (DESCRIPTION ,'') AS DESCRIPTION, ALLOWEOD 
                 FROM POSISSUSPENSIONTYPE WHERE DATAAREAID = @dataareaid";

                MakeParam(cmd,"dataareaid", entry.Connection.DataAreaId);

                return Execute<SuspendedTransactionType>(entry, cmd, CommandType.Text, PopulateList);
            }
        }

        private static void PopulateList(IDataReader dr, SuspendedTransactionType suspendedTransactionType)
        {
            suspendedTransactionType.ID = (string)dr["ID"];
            suspendedTransactionType.Text = (string)dr["DESCRIPTION"];
            suspendedTransactionType.EndofDayCode = (SuspendedTransactionsStatementPostingEnum)dr["ALLOWEOD"];
        }

        public virtual void Save(IConnectionManager entry, SuspendedTransactionType suspendedTransactionType)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ManageCentralSuspensions);

            suspendedTransactionType.Validate();

            bool isNew = false;
            var statement = new SqlServerStatement("POSISSUSPENSIONTYPE");

            if (suspendedTransactionType.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                suspendedTransactionType.ID = Guid.NewGuid().ToString();

            }

            if (isNew || !Exists(entry, suspendedTransactionType.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string) suspendedTransactionType.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string) suspendedTransactionType.ID);
            }

            statement.AddField("DESCRIPTION", suspendedTransactionType.Text);
            statement.AddField("ALLOWEOD", suspendedTransactionType.EndofDayCode, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool InUse(IConnectionManager entry, RecordIdentifier suspensionTransactionID)
        {
            return RecordExists(entry, "POSISSUSPENDEDTRANSACTIONS", "SUSPENSIONTYPEID", suspensionTransactionID);
        } 

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier suspensionTransactionID)
        {
            return RecordExists(entry, "POSISSUSPENSIONTYPE", "ID", suspensionTransactionID);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier suspensionTransactionID)
        {
            DeleteRecord(entry, "POSISSUSPENSIONTYPE", "ID", suspensionTransactionID, BusinessObjects.Permission.ManageCentralSuspensions);
        }

        public virtual SuspendedTransactionType Get(IConnectionManager entry , RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, BusinessObjects.Permission.ManageCentralSuspensions);

                cmd.CommandText = @"SELECT ID, ISNULL (DESCRIPTION ,'') AS DESCRIPTION,  ALLOWEOD 
                    FROM POSISSUSPENSIONTYPE
                    WHERE DATAAREAID = @dataareaid and ID = @id order by ID";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                var suspendedTransactionType = Execute<SuspendedTransactionType>(entry, cmd, CommandType.Text, PopulateList);
                return suspendedTransactionType.Count > 0 ? suspendedTransactionType[0] : null;
            }
        }

        






    }
}
