using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.SqlConnector;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.SqlConnector.QueryHelpers;

namespace LSOne.DataLayer.SqlDataProviders.Profiles
{
    public class ShorthandItemData : SqlServerDataProviderBase, IShorthandItemData
    {
        private static List<TableColumn> shortHandColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "PROFILEID ", TableAlias = "p"},
            new TableColumn {ColumnName = "SHORTHANDTYPE ", TableAlias = "p"},
            new TableColumn {ColumnName = "SHORTHAND ", TableAlias = "p"},
            new TableColumn {ColumnName = "MASTERID ", TableAlias = "p"}
        };

        private static void PopulateShortHandItem(IDataReader dr, ShorthandItem item)
        {
            item.ShorthandType = (ShortHandTypeEnum) (byte) dr["SHORTHANDTYPE"];
            item.ProfileID = (string) dr["PROFILEID"];
            item.Text = (string) dr["SHORTHAND"];
            item.ID = (Guid) dr["MASTERID"];

        }

        public void Save(IConnectionManager entry, ShorthandItem item)
        {
            SqlServerStatement statement = new SqlServerStatement("POSTRANSACTIONSERVICEPROFILESHORTHAND");

            ValidateSecurity(entry, BusinessObjects.Permission.TransactionServiceProfileEdit);

            bool isNew = false;

            if (item.ID == Guid.Empty)
            {
                isNew = true;
                item.ID = Guid.NewGuid();
            }
            
            if (isNew && !Exists(entry, item.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("MASTERID", (Guid)item.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("MASTERID", (Guid)item.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("PROFILEID", (string)item.ProfileID);
            statement.AddField("SHORTHAND", item.Text);
            statement.AddField("SHORTHANDTYPE", (int)item.ShorthandType, SqlDbType.TinyInt);

            Save(entry, item, statement);
        }

        public void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            DeleteRecord(entry, "POSTRANSACTIONSERVICEPROFILESHORTHAND", "MASTERID", ID, BusinessObjects.Permission.TransactionServiceProfileEdit);
        }
        

        public bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            return RecordExists(entry, "POSTRANSACTIONSERVICEPROFILESHORTHAND", "MASTERID", ID);
        }

        public bool Exists(IConnectionManager entry, RecordIdentifier ID, string shortHand)
        {
            return RecordExists(entry, "POSTRANSACTIONSERVICEPROFILESHORTHAND", new[] { "PROFILEID", "SHORTHAND" }, new RecordIdentifier(ID, shortHand));
        }

        public List<ShorthandItem> GetList(IConnectionManager entry, RecordIdentifier profileID)
        {
            ValidateSecurity(entry);

            List<ShorthandItem> result;

            List<Condition> internalConditions = new List<Condition>();

            internalConditions.Add(new Condition {Operator = "AND", ConditionValue = "P.PROFILEID = @PROFILEID "});
            internalConditions.Add(new Condition {Operator = "AND", ConditionValue = "P.DATAAREAID = @DATAAREAID "});

            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("POSTRANSACTIONSERVICEPROFILESHORTHAND", "P"),
                    QueryPartGenerator.InternalColumnGenerator(shortHandColumns),
                    QueryPartGenerator.JoinGenerator(new List<Join>()),
                    QueryPartGenerator.ConditionGenerator(internalConditions),
                    string.Empty);


                MakeParam(cmd, "PROFILEID", (string) profileID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                result = Execute<ShorthandItem>(entry, cmd, CommandType.Text, PopulateShortHandItem);

                return result ?? new List<ShorthandItem>();
            }
        }
    }
}
