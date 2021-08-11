using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Security;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlConnector.MiniConnectionManager
{
    internal class MiniSqlServerConnection : SqlServerConnection
    {
        private bool allowAudit;

        internal MiniSqlServerConnection(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, bool allowAudit, bool disableReplicationActionCreation)
            : base(server, windowsAuthentication, login, password, databaseName, connectionType, dataAreaID,disableReplicationActionCreation)
        {
            this.allowAudit = allowAudit;
        }

        internal MiniSqlServerConnection(string fullConnectionString, string dataAreaID, bool allowAudit,bool disableReplicationActionCreation)
            : base(fullConnectionString, dataAreaID,disableReplicationActionCreation)
        {
            this.allowAudit = allowAudit;
        }

        internal MiniSqlServerConnection(SqlServerConnection parent, bool disableReplicationActionCreation)
            : base(parent,disableReplicationActionCreation)
        {

        }

        internal MiniSqlServerConnection(string fullConnectionString, bool disableReplicationActionCreation)
            : base(fullConnectionString, disableReplicationActionCreation)
        {
            
        }

        protected override SqlServerConnection CreateConnection(string fullConnectionString, string dataAreaID, Guid userID, bool isServerUser, bool disableReplicationActionCreation)
        {
            return new MiniSqlServerConnection(fullConnectionString, disableReplicationActionCreation)
            {
                dataAreaID = dataAreaID,
                userID = userID,
                isServerUser = isServerUser,
                DisableReplicationActions = disableReplicationActionCreation
            };
        }

        public override void ExecuteStatement(StatementBase statement, IDbTransaction transaction)
        {
            if (statement.StatementType == StatementType.Delete)
            {
                if (transaction == null)
                {
                    DeleteFromTable(statement.Command, statement.TableName, CommandType.Text);
                }
                else
                {
                    DeleteFromTable(statement.Command, statement.TableName, CommandType.Text, transaction);
                }
            }
            else
            {
                if (transaction == null)
                {
                    ExecuteNonQuery(statement.Command, CommandType.Text);
                }
                else
                {
                    ExecuteNonQuery(statement.Command, transaction, false, CommandType.Text);
                }
            }
        }

        protected override void SetContext()
        {
            if (!allowAudit)
            {
                return;
            }
            if (userID != Guid.Empty && IsConnected)
            {
                var cmd = new SqlCommand("spSECURITY_SetContext_1_0");

                SqlServerParameters.MakeParam(cmd, "UserGUID", userID);

                ExecuteNonQuery(cmd, true); // passing reading only here since setting context only affects the connection and does not write any data

                currentContext = userID;
            }
        }

        public override void SetContext(RecordIdentifier userID)
        {
            // This operation is only allowed if we are a special server user as other users may not hide their audit track and impersonate someone else
            if (!allowAudit)
            {
                return;
            }

            if ((Guid)userID != Guid.Empty && IsConnected)
            {
                var cmd = new SqlCommand("spSECURITY_SetContext_1_0");

                SqlServerParameters.MakeParam(cmd, "UserGUID", (Guid)userID);

                ExecuteNonQuery(cmd, true); // passing reading only here since setting context only affects the connection and does not write any data

                currentContext = (Guid)userID;
            }
        }
    }
}
