using System;
using System.Data;
using System.Data.Common;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.GenericConnector.Interfaces
{
    public enum ServerVersion
    {
        Unknown,
        SQLServer2012,
        SQLServer2008,
        SqlServer2005
    }
    public interface IConnection
    {
        string DataAreaId
        {
            get;
        }

        [Obsolete("Do not use this function unless you have a very special reason such as interfacing with old components that do not support the SC encapsulated datamodel")]
        object NativeConnection
        {
            get;
        }

        ServerVersion DatabaseVersion
        {
            get;
        }

        string DatabaseName { get;  }

        /// <summary>
        /// Sets the audt context for connection. For security reasons then this function does nothing unless the
        /// user that is logged in is server user. Normal users may not switch server contexts.
        /// </summary>
        /// <param name="userID">ID of the user to set as current audit context</param>
        void SetContext(RecordIdentifier userID);

        /// <summary>
        /// Restores the audit context to the connection default context.
        /// </summary>
        void RestoreContext();

        void ExecuteStatement(StatementBase statement);

        void ExecuteStatement(StatementBase statement, IDbTransaction transaction);

        void DeleteFromTable(IDbCommand cmd, string tableName, CommandType commandType);

        #region ExecuteReader

        IDataReader ExecuteReader(string sql, IDbTransaction transaction);

        IDataReader ExecuteReader(string sql);

        IDataReader ExecuteReader(IDbCommand cmd, IDbTransaction transaction);

        IDataReader ExecuteReader(IDbCommand cmd);

        IDataReader ExecuteReader(IDbCommand cmd, CommandType commandType);

        IDataReader ExecuteReader(IDbCommand cmd, CommandType commandType, CommandBehavior behavior);

        object ExecuteScalar(IDbCommand cmd);

        void ExecuteNonQuery(IDbCommand cmd, bool readingOnly);

        void ExecuteNonQuery(IDbCommand cmd, bool readingOnly, CommandType commandType);

        IDbCommand CreateCommand();
        IDbCommand CreateCommand(string command);

        DbParameter CreateParam(string name, object value);
        DbParameter CreateParam(string name, object value, SqlDbType type);

        DbParameter MakeParam(IDbCommand cmd, string name, object value, SqlDbType type);
        DbParameter MakeParam(IDbCommand cmd, string name, object value, SqlDbType type, ParameterDirection direction);
        DbParameter MakeParam(IDbCommand cmd, string name, string value);
        DbParameter MakeParamNoCheck(IDbCommand cmd, string name, string value);
        DbParameter MakeParam(IDbCommand cmd, string name, Guid value);
        DbParameter MakeParam(IDbCommand cmd, string name, RecordIdentifier value);
        DbParameter MakeParam(IDbCommand cmd, string name, object value, SqlDbType type, ParameterDirection direction, int length);

        StatementBase CreateStatement(string tableName);
        StatementBase CreateStatement(string tableName, bool createReplicationAction);
        StatementBase CreateStatement(string tableName, StatementType statementType);
        StatementBase CreateStatement(string tableName, StatementType statementType, bool createReplicationAction);
        #endregion

        /// <summary>
        /// Exclude the object from any replication action generation for the duration of the connection
        /// </summary>
        /// <param name="objectName">Object to exclude</param>
        void AddReplicationExclusion(string objectName);

        /// <summary>
        /// Stop excluding the object from replication action generation 
        /// </summary>
        /// <param name="objectName">Object to stop excluding</param>
        void RemoveReplicationExclusion(string objectName);

        bool DisableReplicationActions { get; set; }
    }
}
