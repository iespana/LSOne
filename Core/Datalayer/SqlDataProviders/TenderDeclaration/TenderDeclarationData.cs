using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TenderDeclaration;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.TenderDeclaration
{
    public class TenderDeclarationData : SqlServerDataProviderBase, ITenderDeclarationData
    {
        private static void PopulateTenderDeclaration(IDataReader dr, Tenderdeclaration td)
        {
            td.CountedTime = (DateTime)dr["COUNTEDDATETIME"];
            td.TerminalID = (string)dr["TERMINALID"];
            td.StoreID = (string)dr["STOREID"];
            td.StatementID = (string)dr["STATEMENTID"];
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "SCTENDERDECLARATIONS", "COUNTEDDATETIME", id);
        }
        

        public virtual void Save(IConnectionManager entry, Tenderdeclaration tenderdeclaration)
        {
            var statement = new SqlServerStatement("SCTENDERDECLARATIONS");

            ValidateSecurity(entry, BusinessObjects.Permission.TenderDeclaration);

            if (!Exists(entry, tenderdeclaration.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("COUNTEDDATETIME", tenderdeclaration.CountedTime, SqlDbType.DateTime);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("COUNTEDDATETIME", tenderdeclaration.CountedTime, SqlDbType.DateTime);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("STOREID", tenderdeclaration.StoreID);
            statement.AddField("TERMINALID", tenderdeclaration.TerminalID);
            statement.AddField("STAFFID", (Guid)entry.CurrentUser.ID, SqlDbType.UniqueIdentifier);
            statement.AddField("STATEMENTID", tenderdeclaration.StatementID);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "SCTENDERDECLARATIONS", "COUNTEDDATETIME", id, BusinessObjects.Permission.ManageRetailGroups);
        }

        public virtual Tenderdeclaration Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM SCTENDERDECLARATIONS ";
                cmd.CommandText += " WHERE COUNTEDDATETIME = @countedDateTime";
                cmd.CommandText += " and DATAAREAID = @DataAreaID";
                cmd.CommandText += " order by COUNTEDDATETIME desc";

                MakeParam(cmd, "countedDateTime", (DateTime)id.PrimaryID, SqlDbType.DateTime);
                MakeParam(cmd, "DataAreaID", entry.Connection.DataAreaId);

                var tdList = Execute<Tenderdeclaration>(entry, cmd, CommandType.Text, PopulateTenderDeclaration);
                if (tdList.Count > 0)
                {
                    var td = tdList[0];
                    td.TenderDeclarationLines = Providers.TenderDeclarationLineData.GetTenderDeclarationLines(entry, td.ID);
                    return td;
                }
                 
                return null;
            }
        }

        public virtual List<Tenderdeclaration> GetTenderDeclarations(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select COUNTEDDATETIME, TERMINALID, ISNULL(STATEMENTID,'') AS STATEMENTID, STOREID from SCTENDERDECLARATIONS ";
                cmd.CommandText += " WHERE STOREID = @Store";
                cmd.CommandText += " and DATAAREAID = @DataAreaID";
                cmd.CommandText += " order by COUNTEDDATETIME desc";

                MakeParam(cmd, "Store", storeID);
                MakeParam(cmd, "DataAreaID", entry.Connection.DataAreaId);

                return Execute<Tenderdeclaration>(entry, cmd, CommandType.Text, PopulateTenderDeclaration);
            }
        }

        public List<Tenderdeclaration> GetAllTenderDeclarationsWithoutStatementIDForAPeriod(
            IConnectionManager entry,
            RecordIdentifier storeID,
            DateTime startingTime,
            DateTime endingTime)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select COUNTEDDATETIME, ISNULL(TERMINALID,'') as TERMINALID, ISNULL(STATEMENTID,'') AS STATEMENTID, ISNULL(STOREID,'') as STOREID " +
                    "from SCTENDERDECLARATIONS " +
                    "WHERE STOREID = @Store and DATAAREAID = @DataAreaID and COUNTEDDATETIME between @startingTime and @endingTime and STATEMENTID = '' ";

                MakeParam(cmd, "Store", (string) storeID);
                MakeParam(cmd, "DataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "startingTime", startingTime, SqlDbType.DateTime);
                MakeParam(cmd, "endingTime", endingTime, SqlDbType.DateTime);

                var tds = Execute<Tenderdeclaration>(entry, cmd, CommandType.Text,
                                                                         PopulateTenderDeclaration);

                foreach (var td in tds)
                {
                    td.TenderDeclarationLines = Providers.TenderDeclarationLineData.GetTenderDeclarationLines(entry, td.ID);
                }

                return tds;
            }
        }

        public List<Tenderdeclaration> GetTenderDeclarationsForAStatementID(
            IConnectionManager entry,
            RecordIdentifier storeID,
            string statementID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select COUNTEDDATETIME, ISNULL(TERMINALID,'') as TERMINALID, ISNULL(STATEMENTID,'') AS STATEMENTID, ISNULL(STOREID,'') as STOREID " +
                    "from SCTENDERDECLARATIONS " +
                    "WHERE STOREID = @Store and DATAAREAID = @DataAreaID and STATEMENTID = @statementID";

                MakeParam(cmd, "Store", (string) storeID);
                MakeParam(cmd, "DataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "statementID", statementID);

                return Execute<Tenderdeclaration>(entry, cmd, CommandType.Text, PopulateTenderDeclaration);
            }
        }
    }
}
