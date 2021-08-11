using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Transactions
{
    public class SuspendedTransactionAnswerData : SqlServerDataProviderBase, ISuspendedTransactionAnswerData
    {
        private static string BaseSql
        {
            get
            {
                return "SELECT ID,ai.TRANSACTIONID,ISNULL(PROMPT,'') as PROMPT,FIELDORDER,INFOTYPE,ai.INFOTYPESELECTION," +
                       "ISNULL(TEXTRESULT1, '') as TEXTRESULT1,ISNULL(TEXTRESULT2, '') as TEXTRESULT2," +
                       "ISNULL(TEXTRESULT3, '') as TEXTRESULT3,ISNULL(TEXTRESULT4, '') as TEXTRESULT4," +
                       "ISNULL(TEXTRESULT5, '') as TEXTRESULT5,ISNULL(TEXTRESULT6, '') as TEXTRESULT6," +
                       "ISNULL(ai.ADDRESSFORMAT, 0) as ADDRESSFORMAT," +
                       "ISNULL(c.NAME,'') as NAME, ISNULL(c.FIRSTNAME,'') as FIRSTNAME," +
                       "ISNULL(c.MIDDLENAME,'') as MIDDLENAME,ISNULL(c.LASTNAME,'') as LASTNAME," +
                       "ISNULL(c.NAMEPREFIX,'') as NAMEPREFIX,ISNULL(c.NAMESUFFIX,'') as NAMESUFFIX," +
                       "ISNULL(DATERESULT, '01.01.1900') as DATERESULT " +
                       "FROM POSISSUSPENDTRANSADDINFO ai " +
                       "left outer join CUSTOMER c on ai.TEXTRESULT1 = c.ACCOUNTNUM and ai.DATAAREAID = c.DATAAREAID ";
            }
        }

        internal static void PopulateSuspendedTransactionAnswer(IDataReader dr, SuspendedTransactionAnswer suspendedTransactionAnswer)
        {
            suspendedTransactionAnswer.RecordID =(Guid)dr["ID"];
            suspendedTransactionAnswer.TransactionID = (string)dr["TRANSACTIONID"];
            suspendedTransactionAnswer.Prompt = (string)dr["PROMPT"];
            suspendedTransactionAnswer.FieldOrder = (int)dr["FIELDORDER"];
            suspendedTransactionAnswer.InformationType = (SuspensionTypeAdditionalInfo.InfoTypeEnum)dr["INFOTYPE"];
            suspendedTransactionAnswer.InfoCodeTypeSelection = (string)dr["INFOTYPESELECTION"];
            suspendedTransactionAnswer.SerializationTextResult1 = (string)dr["TEXTRESULT1"];
            suspendedTransactionAnswer.SerializationTextResult2 = (string)dr["TEXTRESULT2"];
            suspendedTransactionAnswer.SerializationTextResult3 = (string)dr["TEXTRESULT3"];
            suspendedTransactionAnswer.SerializationTextResult4 = (string)dr["TEXTRESULT4"];
            suspendedTransactionAnswer.SerializationTextResult5 = (string)dr["TEXTRESULT5"];
            suspendedTransactionAnswer.SerializationTextResult6 = (string)dr["TEXTRESULT6"];
            
            if (suspendedTransactionAnswer.InformationType == SuspensionTypeAdditionalInfo.InfoTypeEnum.Address)
            {
                suspendedTransactionAnswer.AddressFormat = (Address.AddressFormatEnum)dr["ADDRESSFORMAT"];
            }
            
            suspendedTransactionAnswer.DateResult = Date.FromAxaptaDate(dr["DATERESULT"]);
            suspendedTransactionAnswer.CustomerSingleFieldName = (string)dr["NAME"];
            suspendedTransactionAnswer.CustomerName.First = (string)dr["FIRSTNAME"];
            suspendedTransactionAnswer.CustomerName.Middle = (string)dr["MIDDLENAME"];
            suspendedTransactionAnswer.CustomerName.Last = (string)dr["LASTNAME"];
            suspendedTransactionAnswer.CustomerName.Prefix = (string)dr["NAMEPREFIX"];
            suspendedTransactionAnswer.CustomerName.Suffix = (string)dr["NAMESUFFIX"];
        }

        public SuspendedTransactionAnswer Get(
            IConnectionManager entry, 
            RecordIdentifier suspendedTransactionAnswerID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, BusinessObjects.Permission.ManageCentralSuspensions);

                cmd.CommandText = BaseSql +
                                 "WHERE ai.DATAAREAID = @dataareaid and ID = @ID and TRANSACTIONID = @transactionID ";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)suspendedTransactionAnswerID[0]);
                MakeParam(cmd, "transactionID", (string)suspendedTransactionAnswerID[1]);

                var suspendedTransactions = Execute<SuspendedTransactionAnswer>(entry, cmd, CommandType.Text, PopulateSuspendedTransactionAnswer);
                return suspendedTransactions.Count > 0 ? suspendedTransactions[0] : null;
            }
        }

        public List<SuspendedTransactionAnswer> GetList(
            IConnectionManager entry,
            RecordIdentifier transactionID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, BusinessObjects.Permission.ManageCentralSuspensions);

                cmd.CommandText = BaseSql +
                                 "WHERE ai.DATAAREAID = @dataareaid and TRANSACTIONID = @transactionID Order by FIELDORDER ";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", (string)transactionID);

                return Execute<SuspendedTransactionAnswer>(entry, cmd, CommandType.Text, PopulateSuspendedTransactionAnswer);
            }
        }

        /// <summary>
        /// Gets all answers for a given suspension type.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="suspensionTypeID">The suspension type ID</param>
        /// <returns></returns>
        public virtual List<SuspendedTransactionAnswer> GetListForSuspensionType(IConnectionManager entry, RecordIdentifier suspensionTypeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, BusinessObjects.Permission.ManageCentralSuspensions);

                cmd.CommandText = BaseSql +
                                  "join POSISSUSPENDEDTRANSACTIONS st on st.TRANSACTIONID = ai.TRANSACTIONID and ai.DATAAREAID = st.DATAAREAID " +
                                  "where ai.DATAAREAID = @dataAreaId and st.SUSPENSIONTYPEID = @suspensionTypeId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "suspensionTypeId", (string)suspensionTypeID);

                return Execute<SuspendedTransactionAnswer>(entry, cmd, CommandType.Text, PopulateSuspendedTransactionAnswer);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier suspendedTransactionAnswerID)
        {
            return RecordExists(
                entry,
                "POSISSUSPENDTRANSADDINFO", 
                new[]{"ID", "TRANSACTIONID"}, 
                suspendedTransactionAnswerID);
        }

        public virtual void DeleteForTransaction(IConnectionManager entry, RecordIdentifier transactionID)
        {
            DeleteRecord(entry, "POSISSUSPENDTRANSADDINFO", "TRANSACTIONID", transactionID, BusinessObjects.Permission.ManageCentralSuspensions);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier suspendedTransactionAnswerID)
        {
            DeleteRecord(
                entry,
                "POSISSUSPENDTRANSADDINFO",
                new[] { "ID", "TRANSACTIONID" }, 
                suspendedTransactionAnswerID,
                LSOne.DataLayer.BusinessObjects.Permission.ManageCentralSuspensions);
        }

        public virtual void Save(IConnectionManager entry, SuspendedTransactionAnswer suspendedTransactionAnswer)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ManageCentralSuspensions);

            suspendedTransactionAnswer.Validate();

            var statement = new SqlServerStatement("POSISSUSPENDTRANSADDINFO");

            if (suspendedTransactionAnswer.RecordID == RecordIdentifier.Empty)
            {
                suspendedTransactionAnswer.RecordID = Guid.NewGuid();
            }

            if (!Exists(entry, suspendedTransactionAnswer.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid) suspendedTransactionAnswer.RecordID, SqlDbType.UniqueIdentifier);
                statement.AddKey("TRANSACTIONID", (string) suspendedTransactionAnswer.TransactionID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid) suspendedTransactionAnswer.RecordID, SqlDbType.UniqueIdentifier);
                statement.AddCondition("TRANSACTIONID", (string) suspendedTransactionAnswer.TransactionID);
            }

            statement.AddField("PROMPT", suspendedTransactionAnswer.Prompt);
            statement.AddField("FIELDORDER", suspendedTransactionAnswer.FieldOrder, SqlDbType.Int);
            statement.AddField("INFOTYPE", (int) suspendedTransactionAnswer.InformationType, SqlDbType.Int);
            statement.AddField("INFOTYPESELECTION", suspendedTransactionAnswer.InfoCodeTypeSelection);
            statement.AddField("TEXTRESULT1", suspendedTransactionAnswer.SerializationTextResult1);
            statement.AddField("TEXTRESULT2", suspendedTransactionAnswer.SerializationTextResult2);
            statement.AddField("TEXTRESULT3", suspendedTransactionAnswer.SerializationTextResult3);
            statement.AddField("TEXTRESULT4", suspendedTransactionAnswer.SerializationTextResult4);
            statement.AddField("TEXTRESULT5", suspendedTransactionAnswer.SerializationTextResult5);
            statement.AddField("TEXTRESULT6", suspendedTransactionAnswer.SerializationTextResult6);
            statement.AddField("ADDRESSFORMAT", (int) suspendedTransactionAnswer.AddressFormat, SqlDbType.Int);

            if (suspendedTransactionAnswer.DateResult.IsEmpty)
            {
                statement.AddField("DATERESULT", DBNull.Value, SqlDbType.DateTime);
            }
            else
            {
                statement.AddField("DATERESULT", suspendedTransactionAnswer.DateResult.DateTime, SqlDbType.DateTime);
            }

            entry.Connection.ExecuteStatement(statement);
        }
    }
 }

