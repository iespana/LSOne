using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Omni;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.SqlDataProviders.Omni
{
    [LSOneUsage(CodeUsage.LSCommerce)]
    public class OmniJournalData : SqlServerDataProviderBase, IOmniJournalData
    {
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier journalID)
        {
            return RecordExists(entry, "OMNIJOURNAL", "JOURNALID", journalID, false);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier journalID)
        {
            DeleteRecord(entry, "OMNIJOURNAL", "JOURNALID", journalID, "", false);
        }

        public virtual OmniJournal Get(IConnectionManager entry, RecordIdentifier journalID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT JOURNALID, 
                                           ISNULL(JOURNALTYPE, -1) JOURNALTYPE,
                                           ISNULL(STOREID, '') STOREID,
                                           ISNULL(TEMPLATEID, '') TEMPLATEID,
                                           ISNULL(STAFFID, '') STAFFID,
                                           ISNULL(STATUS, 0) STATUS,
                                           ISNULL(XMLDATA, '') XMLDATA,
                                           ISNULL(TRANSACTIONID, '') TRANSACTIONID
                                    FROM OMNIJOURNAL
                                    WHERE JOURNALID = @JOURNALID ";
                
                MakeParam(cmd, "JOURNALID", (string)journalID);

                return Get<OmniJournal>(entry, cmd, journalID, PopulateOmniJournalWithXML, GenericConnector.Enums.CacheType.CacheTypeApplicationLifeTime, UsageIntentEnum.Normal);
            }
        }

        public virtual List<OmniJournal> GetOmniJournals(IConnectionManager entry, OmniJournalStatus status)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT JOURNALID, 
                                           ISNULL(JOURNALTYPE, -1) JOURNALTYPE,
                                           ISNULL(STOREID, '') STOREID,
                                           ISNULL(TEMPLATEID, '') TEMPLATEID,
                                           ISNULL(STAFFID, '') STAFFID,
                                           ISNULL(STATUS, 0) STATUS,
                                           ISNULL(TRANSACTIONID, '') TRANSACTIONID
                                    FROM OMNIJOURNAL
                                    WHERE STATUS = @STATUS ORDER BY CREATEDDATE ASC";

                MakeParam(cmd, "STATUS", (int)status);

                return Execute<OmniJournal>(entry, cmd, CommandType.Text, PopulateOmniJournal);
            }
        }

        private static void PopulateOmniJournal(IDataReader dr, OmniJournal journal)
        {
            journal.ID = (string)dr["JOURNALID"];
            journal.JournalType = (int)dr["JOURNALTYPE"];
            journal.StoreID = (string)dr["STOREID"];
            journal.TemplateID = (string)dr["TEMPLATEID"];
            journal.StaffID = (string)dr["STAFFID"];
            journal.Status = (OmniJournalStatus) (int)dr["STATUS"];
            journal.TransactionId = (string)dr["TRANSACTIONID"];
        }

        private static void PopulateOmniJournalWithXML(IDataReader dr, OmniJournal journal)
        {
            PopulateOmniJournal(dr, journal);
            journal.XmlData = (string)dr["XMLDATA"];
        }

        public virtual void Save(IConnectionManager entry, OmniJournal journal)
        {
            SqlServerStatement statement = new SqlServerStatement("OMNIJOURNAL", false);

            bool isNew = false;
            if (RecordIdentifier.IsEmptyOrNull(journal.ID))
            {
                isNew = true;
                journal.ID = DataProviderFactory.Instance.GenerateNumber<IOmniJournalData, OmniJournal>(entry);
            }

            if (isNew || !Exists(entry, journal.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("JOURNALID", (string)journal.ID);                
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("JOURNALID", (string)journal.ID);
            }

            statement.AddField("JOURNALTYPE", journal.JournalType, SqlDbType.Int);
            statement.AddField("STOREID", (string)journal.StoreID);
            statement.AddField("TEMPLATEID", (string)journal.TemplateID);
            statement.AddField("STAFFID", (string)journal.StaffID);
            statement.AddField("STATUS", journal.Status, SqlDbType.Int);
            statement.AddField("XMLDATA", journal.XmlData, SqlDbType.Xml);
            statement.AddField("TRANSACTIONID", journal.TransactionId);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void UpdateOmniJournalStatus(IConnectionManager entry, RecordIdentifier journalID, OmniJournalStatus status)
        {
            SqlServerStatement statement = new SqlServerStatement("OMNIJOURNAL");

            statement.StatementType = StatementType.Update;
            statement.AddCondition("JOURNALID", (string)journalID);
            statement.AddField("STATUS", status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual int ImportOmniLinesFromXML(IConnectionManager entry, RecordIdentifier omniJournalID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand("spINVENTORY_ImportOmniJournalLinesFromXML"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;

                MakeParam(cmd, "OMNIJOURNALID", omniJournalID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                SqlParameter insertedRows = MakeParam(cmd, "INSERTEDRECORDS", "", SqlDbType.Int, ParameterDirection.Output, 4);

                entry.Connection.ExecuteNonQuery(cmd, false);

                return (int)insertedRows.Value;
            }
        }

        public virtual int ImportOmniGoodsReceivingLinesFromXML(IConnectionManager entry, RecordIdentifier omniJournalID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand("spINVENTORY_ImportOmniGoodsReceivingLinesFromXML"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;

                MakeParam(cmd, "OMNIJOURNALID", omniJournalID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                SqlParameter insertedRows = MakeParam(cmd, "INSERTEDRECORDS", "", SqlDbType.Int, ParameterDirection.Output, 4);

                entry.Connection.ExecuteNonQuery(cmd, false);

                return (int)insertedRows.Value;
            }
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return new RecordIdentifier("OMNIJOURNAL"); }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "OMNIJOURNAL", "JOURNALID", sequenceFormat, startingRecord, numberOfRecords);
        }

        public virtual void IncrementRetryCounter(IConnectionManager entry, RecordIdentifier omniJournalID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "UPDATE OMNIJOURNAL SET RETRYCOUNTER = RETRYCOUNTER + 1 WHERE JOURNALID = @JOURNALID ";

                MakeParam(cmd, "JOURNALID", (string)omniJournalID);

                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }
        }
    }
}
