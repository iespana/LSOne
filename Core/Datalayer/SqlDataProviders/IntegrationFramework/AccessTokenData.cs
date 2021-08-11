using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.IntegrationFramework;
using LSOne.DataLayer.DataProviders.IntegrationFramework;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.IntegrationFramework
{
    class AccessTokenData : SqlServerDataProviderBase, IAccessTokenData
    {
        private static List<TableColumn> columnList = new List<TableColumn>
        {
            new TableColumn {ColumnName = "SENDERDNS", TableAlias = "A"},
            new TableColumn {ColumnName = "DESCRIPTION", TableAlias = "A"},
            new TableColumn {ColumnName = "USERID", TableAlias = "A"},
            new TableColumn {ColumnName = "LOGIN", TableAlias = "U", ColumnAlias = "USERLOGIN"},
            new TableColumn {ColumnName = "NAME", TableAlias = "STAFF", ColumnAlias = "USERNAME"},
            new TableColumn {ColumnName = "STAFFID", TableAlias = "U", ColumnAlias = "USERSTAFFID"},
            new TableColumn {ColumnName = "STOREID", TableAlias = "A"},
            new TableColumn {ColumnName = "NAME", TableAlias = "S", ColumnAlias = "STORENAME"},
            new TableColumn {ColumnName = "ACTIVE", TableAlias = "A"},
            new TableColumn {ColumnName = "TIMESTAMP", TableAlias = "A"},
            new TableColumn {ColumnName = "COALESCE(A.[TOKEN], '')", TableAlias = "", ColumnAlias = "TOKEN"},
        };

        private static List<Join> joins = new List<Join>
        {
            new Join
            {
                Condition = "a.STOREID = s.STOREID",
                JoinType = "LEFT",
                Table = "RBOSTORETABLE",
                TableAlias = "s"
            },
            new Join
            {
                Condition = "a.USERID = u.GUID",
                JoinType = "LEFT",
                Table = "USERS",
                TableAlias = "u"
            },
            new Join
            {
                Condition = "u.STAFFID = staff.STAFFID",
                JoinType = "LEFT",
                Table = "RBOSTAFFTABLE",
                TableAlias = "staff"
            }
        };

        private static void PopulateToken(IDataReader dr, AccessToken token)
        {
            token.Description = (string)dr["DESCRIPTION"];
            token.UserID = (Guid)dr["USERID"];
            token.UserStaffID = (string)dr["USERSTAFFID"];
            token.UserLogin = (string)dr["USERLOGIN"];
            token.UserName = (string)dr["USERNAME"];
            token.StoreID = (string)dr["STOREID"];
            token.StoreName = (string)dr["STORENAME"];
            token.TimeStamp = (DateTime)dr["TIMESTAMP"];
            token.Active = ((bool)dr["ACTIVE"]);
            token.SenderDNS = (string)dr["SENDERDNS"];
            token.Token = (string)dr["TOKEN"];
        }

        private static void PopulateTokenUnsecure(IDataReader dr, AccessToken token)
        {
            token.Description = (string)dr["DESCRIPTION"];
            token.UserID = (Guid)dr["USERID"];
            token.StoreID = (string)dr["STOREID"];
            token.TimeStamp = (DateTime)dr["TIMESTAMP"];
            token.Active = ((bool)dr["ACTIVE"]);
            token.SenderDNS = (string)dr["SENDERDNS"];
            token.Token = (string)dr["TOKEN"];
        }

        /// <summary>
        /// Saves/updates access token information for the Integration framework to the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="token">Access token to save or update</param>
        public virtual void Save(IConnectionManager entry, AccessToken token)
        {
            var statement = new SqlServerStatement("ACCESSTOKENTABLE");

            ValidateSecurity(entry, Permission.VisualProfileEdit);

            bool isNew = !Exists(entry, token.SenderDNS);

            if (isNew)
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("SENDERDNS", token.SenderDNS);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("SENDERDNS", token.SenderDNS);
            }

            statement.AddField("DESCRIPTION", token.Description);
            statement.AddField("USERID", (Guid)token.UserID, SqlDbType.UniqueIdentifier);
            statement.AddField("STOREID", (string)token.StoreID);
            statement.AddField("TIMESTAMP", token.TimeStamp, SqlDbType.DateTime);
            statement.AddField("ACTIVE", token.Active, SqlDbType.Bit);
            statement.AddField("TOKEN", token.Token);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Deletes an Access token from the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="ID">ID/sender dns of the access token we want to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            var statement = new SqlServerStatement("ACCESSTOKENTABLE");
            ValidateSecurity(entry, Permission.VisualProfileEdit);
            statement.StatementType = StatementType.Delete;

            statement.AddCondition("SENDERDNS", (string)ID);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Checks if an access token already exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="ID">ID/sender dns of the access token</param>
        /// <returns>True or false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            return RecordExists(entry, "ACCESSTOKENTABLE", "SENDERDNS", ID, false);
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            throw new NotImplementedException();
        }

        public virtual AccessToken Get(IConnectionManager entry, RecordIdentifier senderDNS)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in columnList)
                {
                    columns.Add(selectionColumn);
                }

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition {ConditionValue = "a.SENDERDNS = @senderDNS " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("ACCESSTOKENTABLE", "a"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY a.DESCRIPTION");

                MakeParam(cmd, "senderDNS", (string)senderDNS);

                var token = Execute<AccessToken>(entry, cmd, CommandType.Text, PopulateToken);

                return (token.Count > 0) ? token[0] : null;
            }
        }

        public virtual void ActivateAccessToken(IConnectionManager entry, RecordIdentifier senderDNS)
        {
            if (RecordIdentifier.IsEmptyOrNull(senderDNS))
            {
                return;
            }

            ValidateSecurity(entry, Permission.VisualProfileEdit);

            SqlServerStatement statement = new SqlServerStatement("ACCESSTOKENTABLE");
            statement.StatementType = StatementType.Update;

            statement.AddCondition("SENDERDNS", senderDNS.DBValue, senderDNS.DBType);
            statement.AddField("ACTIVE", true, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void RevokeAccessToken(IConnectionManager entry, RecordIdentifier senderDNS)
        {
            if (RecordIdentifier.IsEmptyOrNull(senderDNS))
            {
                return;
            }

            ValidateSecurity(entry, Permission.VisualProfileEdit);

            SqlServerStatement statement = new SqlServerStatement("ACCESSTOKENTABLE");
            statement.StatementType = StatementType.Update;

            statement.AddCondition("SENDERDNS", senderDNS.DBValue, senderDNS.DBType);
            statement.AddField("ACTIVE", false, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Retrieves a list of all access tokens in the database 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>List of access tokens</returns>
        public virtual List<AccessToken> GetIFTokenList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in columnList)
                {
                    columns.Add(selectionColumn);
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("ACCESSTOKENTABLE", "a"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    "",
                    "ORDER BY a.DESCRIPTION");

                return Execute<AccessToken>(entry, cmd, CommandType.Text, PopulateToken);
            }
        }

        /// <summary>
        /// Runs a stored procedure and updates a dictionary kept in the integration framework containing all existing access tokens
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="dataSource"></param>
        /// <param name="windowsAuthentication"></param>
        /// <param name="sqlServerLogin"></param>
        /// <param name="sqlServerPassword"></param>
        /// <param name="databaseName"></param>
        /// <param name="connectionType"></param>
        /// <param name="dataAreaID"></param>
        /// <returns></returns>
        public virtual List<AccessToken> GetIFTokenListUnsecure(IConnectionManager entry, string dataSource, bool windowsAuthentication,
            string sqlServerLogin, SecureString sqlServerPassword, string databaseName, ConnectionType connectionType,
            string dataAreaID)
        {
            using (IDbCommand cmd = new SqlCommand("spGetAccessTokens_1_0")) //No connection exists on this point to create the command.
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    return entry.UnsecureExecuteReader<AccessToken>(dataSource, windowsAuthentication, sqlServerLogin, sqlServerPassword, databaseName, connectionType, dataAreaID, cmd, PopulateTokenUnsecure);
                }
                catch
                {
                    return new List<AccessToken>();
                }
            }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "ACCESSTOKENTABLE", "SENDERDNS", sequenceFormat, startingRecord, numberOfRecords);
        }

        public RecordIdentifier SequenceID { get; }

    }
}
