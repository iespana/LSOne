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
    public class SuspensionTypeAdditionalInfoData : SqlServerDataProviderBase, ISuspensionTypeAdditionalInfoData
    {
        // TODO set ISNULL where appropriate
        protected string BaseSql
        {
            get
            {
                return "SELECT p.ID, p.SUSPENSIONTYPEID, ISNULL(p.PROMPT,'') AS PROMPT, p.FIELDORDER, p.INFOTYPE, ISNULL(p.INFOTYPESELECTION,0) AS INFOTYPESELECTION , p.REQUIRED, ISNULL(r.DESCRIPTION,'') as INFOCODEDESCRIPTION " +
                    "FROM POSISSUSPENSIONADDINFO p " +
                    "Left outer join RBOINFOCODETABLE r on r.DATAAREAID = p.DATAAREAID and r.INFOCODEID = p.INFOTYPESELECTION ";
            }
        }

        public virtual List<SuspensionTypeAdditionalInfo> GetList(IConnectionManager entry, RecordIdentifier suspensionTypeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, BusinessObjects.Permission.ManageCentralSuspensions);

                cmd.CommandText = BaseSql +
                    "WHERE p.DATAAREAID = @dataareaid AND p.SUSPENSIONTYPEID = @suspendensionTypeID order by p.FIELDORDER";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "suspendensionTypeID", (string)suspensionTypeID);

                return Execute<SuspensionTypeAdditionalInfo>(entry, cmd, CommandType.Text, PopulateSuspensionTypeAdditionInfo);
            }
        }

        private static void PopulateSuspensionTypeAdditionInfo(IDataReader dr, SuspensionTypeAdditionalInfo suspendedTypeAdditionalInfo)
        {
            suspendedTypeAdditionalInfo.AdditionalInfoID = (string)dr["ID"];
            suspendedTypeAdditionalInfo.SuspensionTypeID = (string)dr["SUSPENSIONTYPEID"];
            suspendedTypeAdditionalInfo.Text = (string)dr["PROMPT"];
            suspendedTypeAdditionalInfo.Order = (int)dr["FIELDORDER"];
            suspendedTypeAdditionalInfo.Infotype = (SuspensionTypeAdditionalInfo.InfoTypeEnum)dr["INFOTYPE"];
            suspendedTypeAdditionalInfo.InfoTypeSelectionID = (string)dr["INFOTYPESELECTION"];
            suspendedTypeAdditionalInfo.Required = ((byte)dr["REQUIRED"] != 0);
            suspendedTypeAdditionalInfo.InfoTypeSelectionDescription = (string)dr["INFOCODEDESCRIPTION"];
        }

        public virtual void Save(IConnectionManager entry, SuspensionTypeAdditionalInfo suspendedTypeAdditionalInfo)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ManageCentralSuspensions);

            suspendedTypeAdditionalInfo.Validate();

            var statement = new SqlServerStatement("POSISSUSPENSIONADDINFO");

            var isNew = false;
            if (suspendedTypeAdditionalInfo.AdditionalInfoID == RecordIdentifier.Empty)
            {
                isNew = true;
                suspendedTypeAdditionalInfo.AdditionalInfoID = Guid.NewGuid().ToString();
                suspendedTypeAdditionalInfo.Order = GetRankNumber(entry,suspendedTypeAdditionalInfo.SuspensionTypeID);
            }

            if (isNew || !Exists(entry, suspendedTypeAdditionalInfo.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)suspendedTypeAdditionalInfo.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)suspendedTypeAdditionalInfo.ID);
            }

            statement.AddField("SUSPENSIONTYPEID", (string)suspendedTypeAdditionalInfo.SuspensionTypeID); 
            statement.AddField("PROMPT", suspendedTypeAdditionalInfo.Text);
            statement.AddField("FIELDORDER", suspendedTypeAdditionalInfo.Order, SqlDbType.Int);
            statement.AddField("INFOTYPE", (int)suspendedTypeAdditionalInfo.Infotype, SqlDbType.Int);
            statement.AddField("INFOTYPESELECTION", (string)suspendedTypeAdditionalInfo.InfoTypeSelectionID);
            statement.AddField("REQUIRED", suspendedTypeAdditionalInfo.Required, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "POSISSUSPENSIONADDINFO", new[] { "ID", "SUSPENSIONTYPEID" }, id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier suspendedTypeAdditionalInfo)
        {
            DeleteRecord(entry, "POSISSUSPENSIONADDINFO", new[] { "ID", "SUSPENSIONTYPEID" }, suspendedTypeAdditionalInfo, BusinessObjects.Permission.ManageCentralSuspensions);
        }

        public virtual SuspensionTypeAdditionalInfo Get(IConnectionManager entry, RecordIdentifier id)
        {
            List<SuspensionTypeAdditionalInfo> suspendedTransactionAdditionalInfo;
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, BusinessObjects.Permission.ManageCentralSuspensions);

                cmd.CommandText = BaseSql +
                        " WHERE p.DATAAREAID = @dataareaid AND p.ID = @id order by p.ID";

                MakeParam(cmd,"dataareaid" ,entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                suspendedTransactionAdditionalInfo = Execute<SuspensionTypeAdditionalInfo>(entry, cmd, CommandType.Text, PopulateSuspensionTypeAdditionInfo);              
            }

            return suspendedTransactionAdditionalInfo.Count > 0 ? suspendedTransactionAdditionalInfo[0] : null;
        }

        private static int GetRankNumber(IConnectionManager entry, RecordIdentifier suspensionTypeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, BusinessObjects.Permission.ManageCentralSuspensions);

                cmd.CommandText = "SELECT ISNULL(MAX(FIELDORDER), 0) + 1 FROM POSISSUSPENSIONADDINFO " +
                     " WHERE DATAAREAID = @dataareaid AND SUSPENSIONTYPEID = @suspendensionTypeID";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "suspendensionTypeID", (string)suspensionTypeID);

                return (int)entry.Connection.ExecuteScalar(cmd);
            }
        }
    }
}
