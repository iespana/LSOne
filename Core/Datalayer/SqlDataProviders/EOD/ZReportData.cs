using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.EOD;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.EOD;
using LSOne.DataLayer.DataProviders.Sequences;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.EOD
{    
    public class ZReportData : SqlServerDataProviderBase, IZReportData
    {
        private static string BaseSql
        {
            get
            {
                return " SELECT ZREPORTID, CREATEDDATE, STOREID, TERMINALID, Z.STAFFID, ZGROSSAMOUNT, ZNETAMOUNT, TOTALGROSSAMOUNT, TOTALNETAMOUNT, ENTRYTYPE, Z.DATAAREAID " +
                       " ,ZRETURNGROSSAMOUNT, ZRETURNNETAMOUNT, TOTALRETURNGROSSAMOUNT, TOTALRETURNNETAMOUNT, ISNULL(U.Login, '') AS LOGIN " +
                       " FROM POSZREPORT Z" +
                       " LEFT OUTER JOIN USERS U ON U.STAFFID = Z.STAFFID";
            }
        }        

        /// <summary>
        /// Populates and instance of <seealso cref="ZReport"/>
        /// </summary>
        /// <param name="dr">A reader with the data</param>
        /// <param name="zReport">The instance to populate</param>
        private static void Populate(IDataReader dr, ZReport zReport)
        {
            zReport.ID = Conversion.ToStr(dr["ZREPORTID"]);
            zReport.CreatedDate = Conversion.ToDateTime(dr["CREATEDDATE"]);
            zReport.StoreID = Conversion.ToStr(dr["STOREID"]);
            zReport.TerminalID = Conversion.ToStr(dr["TERMINALID"]);
            zReport.StaffID = Conversion.ToStr(dr["STAFFID"]);
            zReport.NetAmount = Conversion.ToDecimal(dr["ZNETAMOUNT"]);
            zReport.GrossAmount = Conversion.ToDecimal(dr["ZGROSSAMOUNT"]);
            zReport.TotalNetAmount = Conversion.ToDecimal(dr["TOTALNETAMOUNT"]);
            zReport.TotalGrossAmount = Conversion.ToDecimal(dr["TOTALGROSSAMOUNT"]);
            zReport.EntryType = Conversion.ToInt(dr["ENTRYTYPE"]);
            zReport.TotalReturnGrossAmount = Conversion.ToDecimal(dr["TOTALRETURNGROSSAMOUNT"]);
            zReport.TotalReturnNetAmount = Conversion.ToDecimal(dr["TOTALRETURNNETAMOUNT"]);
            zReport.ReturnGrossAmount = Conversion.ToDecimal(dr["ZRETURNGROSSAMOUNT"]);
            zReport.ReturnNetAmount = Conversion.ToDecimal(dr["ZRETURNNETAMOUNT"]);
            zReport.Login = Conversion.ToStr(dr["LOGIN"]);
        }

        /// <summary>
        /// Returns true if the Z Report ID already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="zReportID">The ID to check</param>
        /// <returns>If ID already exists returns true</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier zReportID)
        {
            return RecordExists(entry, "POSZREPORT", "ZREPORTID", zReportID);
        }

        /// <summary>
        /// Returns a list of all Z reports
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of Z reports</returns>
        public virtual List<ZReport> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSql + " WHERE DATAAREAID = @DATAAREAID ";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);                

                return Execute<ZReport>(entry, cmd, CommandType.Text, Populate);                
            }
        }

        /// <summary>
        /// Returns a list of Z reports for a specific store and terminal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The store the Z reports should have been created on</param>
        /// <param name="terminalID">The terminal the Z reports should have been created on</param>
        /// <returns>A list of Z reports</returns>
        public virtual List<ZReport> GetList(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID)
        {            
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSql + " WHERE Z.DATAAREAID = @DATAAREAID " +
                              " AND STOREID = @STOREID " +
                              " AND TERMINALID = @TERMINALID " +
                              " ORDER BY ZREPORTID ";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", storeID.ToString());
                MakeParam(cmd, "TERMINALID", terminalID.ToString());

                return Execute<ZReport>(entry, cmd, CommandType.Text, Populate);
            }
        }

        public virtual void Save(IConnectionManager entry, ZReport zReport)
        {
            var statement = new SqlServerStatement("POSZREPORT");

            ValidateSecurity(entry, BusinessObjects.Permission.AdministrationMaintainSettings);

            bool isNew = false;
            if (zReport.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                zReport.ID = DataProviderFactory.Instance.
                    Get<INumberSequenceData, NumberSequence>()
                    .GenerateNumberFromSequence(entry, new ZReportData());
            }

            if (isNew || !Exists(entry, zReport.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ZREPORTID", zReport.ID.ToString());
                statement.AddKey("STOREID", zReport.StoreID.ToString());
                statement.AddKey("TERMINALID", zReport.TerminalID.ToString());
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ZREPORTID", zReport.ID.ToString());
                statement.AddCondition("STOREID", zReport.StoreID.ToString());
                statement.AddCondition("TERMINALID", zReport.TerminalID.ToString());
            }

            statement.AddField("STAFFID", zReport.StaffID.ToString());
            statement.AddField("ZGROSSAMOUNT", zReport.GrossAmount, SqlDbType.Decimal);
            statement.AddField("ZNETAMOUNT", zReport.NetAmount, SqlDbType.Decimal);
            statement.AddField("TOTALGROSSAMOUNT", zReport.TotalGrossAmount, SqlDbType.Decimal);
            statement.AddField("TOTALNETAMOUNT", zReport.TotalNetAmount, SqlDbType.Decimal);
            statement.AddField("ENTRYTYPE", zReport.EntryType, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);        
        }

        public virtual void CreateNewId(IConnectionManager entry, int initialId)
        {
            var numberSeq = DataProviderFactory.Instance.
                Get<INumberSequenceData, NumberSequence>()
                .Get(entry, SequenceID);

            if (numberSeq.NextRecord != initialId)
            {
                numberSeq.NextRecord = initialId;
                DataProviderFactory.Instance.
                    Get<INumberSequenceData, NumberSequence>()
                    .Save(entry, numberSeq);
            }
        }
     
        #region ISequenceable Members

        /// <summary>
        /// Returns true if the ID already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID to check</param>
        /// <returns>If the ID exists returns true</returns>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        /// <summary>
        /// The ID of the number sequence itself
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "Z-RPT"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSZREPORT", "ZREPORTID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
