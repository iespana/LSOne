using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlConnector
{
    internal class SqlServerTransactionConnectionWrapper : IConnection
    {
        private readonly SqlTransaction transaction;
        private readonly SqlServerConnection connection;

        public SqlServerTransactionConnectionWrapper(IConnection connection, SqlTransaction transaction)
        {
            this.connection = (SqlServerConnection)connection;
            this.transaction = transaction;
        }

        #region IConnection Members

        public string DataAreaId
        {
            get { return connection.DataAreaId; }
        }

        public object NativeConnection
        {
            get { return connection.NativeConnection; }
        }

        public void ExecuteStatement(StatementBase statement)
        {
            connection.ExecuteStatement(statement,transaction);
        }

        public void ExecuteStatement(StatementBase statement, IDbTransaction transaction)
        {
            connection.ExecuteStatement(statement, transaction);
        }

        public void DeleteFromTable(IDbCommand cmd, string tableName, CommandType commandType)
        {
            connection.DeleteFromTable(cmd, tableName, commandType);
        }

        public IDataReader ExecuteReader(string sql, IDbTransaction transaction)
        {
            return connection.ExecuteReader(sql, transaction);
        }

        public IDataReader ExecuteReader(string sql)
        {
            return connection.ExecuteReader(sql);
        }

        public IDataReader ExecuteReader(IDbCommand cmd, IDbTransaction transaction)
        {
            return connection.ExecuteReader(cmd, transaction);
        }

        public IDataReader ExecuteReader(IDbCommand cmd)
        {
            return connection.ExecuteReader(cmd);
        }

        public IDataReader ExecuteReader(IDbCommand cmd, CommandType commandType)
        {
            return connection.ExecuteReader((SqlCommand)cmd, commandType,transaction);
        }

        public IDataReader ExecuteReader(IDbCommand cmd, CommandType commandType, CommandBehavior behavior)
        {
            return connection.ExecuteReader(cmd, commandType, behavior);
        }

        public object ExecuteScalar(IDbCommand cmd)
        {
            return connection.ExecuteScalar((SqlCommand)cmd, transaction);
        }

        public void ExecuteNonQuery(IDbCommand cmd, bool readingOnly)
        {
            connection.ExecuteNonQuery((SqlCommand)cmd, readingOnly, CommandType.StoredProcedure,transaction);
        }

        public void ExecuteNonQuery(IDbCommand cmd, bool readingOnly, CommandType commandType)
        {
            connection.ExecuteNonQuery((SqlCommand)cmd, readingOnly, commandType,transaction);
        }

        public void SetContext(RecordIdentifier userID)
        {
            connection.SetContext(userID);
        }

        public void RestoreContext()
        {
            connection.RestoreContext();
        }

        public IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }
       
        public IDbCommand CreateCommand(string cmd)
        {
            return new SqlCommand(cmd);
        }

        public DbParameter CreateParam(string name, object value)
        {
            return new SqlParameter(name, value);
        }

        public DbParameter CreateParam(string name, object value, SqlDbType type)
        {
            return new SqlParameter(name, value) { SqlDbType = type };
        }

        public DbParameter MakeParam(IDbCommand cmd, string name, object value, SqlDbType type)
        {
            return SqlServerParameters.MakeParam(cmd, name, value, type);
        }

        public DbParameter MakeParam(IDbCommand cmd, string name, object value, SqlDbType type, ParameterDirection direction)
        {
            return SqlServerParameters.MakeParam(cmd, name, value, type, direction);
        }

        public DbParameter MakeParam(IDbCommand cmd, string name, string value)
        {
            return SqlServerParameters.MakeParam(cmd, name, value);
        }

        public DbParameter MakeParamNoCheck(IDbCommand cmd, string name, string value)
        {
            return SqlServerParameters.MakeParamNoCheck(cmd, name, value);
        }

        public DbParameter MakeParam(IDbCommand cmd, string name, Guid value)
        {
            return SqlServerParameters.MakeParam(cmd, name, value);
        }

        public DbParameter MakeParam(IDbCommand cmd, string name, RecordIdentifier value)
        {
            return SqlServerParameters.MakeParam(cmd, name, value);
        }

        public DbParameter MakeParam(IDbCommand cmd, string name, object value, SqlDbType type, ParameterDirection direction, int length)
        {
            return SqlServerParameters.MakeParam(cmd, name, value, type, direction, length);
        }

        public StatementBase CreateStatement(string tableName)
        {
            return new SqlServerStatement(tableName);
        }

        public StatementBase CreateStatement(string tableName, bool createReplicationAction)
        {
            return new SqlServerStatement(tableName, createReplicationAction);
        }

        public StatementBase CreateStatement(string tableName, StatementType statementType)
        {
            return new SqlServerStatement(tableName, statementType);
        }

        public StatementBase CreateStatement(string tableName, StatementType statementType, bool createReplicationAction)
        {
            return new SqlServerStatement(tableName, statementType, createReplicationAction);
        }
        #endregion


        public ServerVersion DatabaseVersion
        {
            get { throw new System.NotImplementedException(); }
        }
        public string DatabaseName
        {
            get { throw new System.NotImplementedException(); }
        }
        public void AddReplicationExclusion(string objectName)
        {
            connection.AddReplicationExclusion(objectName);
        }

        public void RemoveReplicationExclusion(string objectName)
        {
            connection.RemoveReplicationExclusion(objectName);
        }

        public bool DisableReplicationActions
        {
            get { return connection.DisableReplicationActions; }
            set { connection.DisableReplicationActions = value; }
        }
    }
}
