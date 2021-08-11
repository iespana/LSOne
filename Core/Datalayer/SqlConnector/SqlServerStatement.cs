using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.Utilities.Helpers;

namespace LSOne.DataLayer.SqlConnector
{
    public class SqlServerStatement : StatementBase
    {
        private readonly SqlCommand command;
        
        public SqlServerStatement(string tableName)
            : this(tableName, StatementType.Insert)
        {
        }

        public SqlServerStatement(string tableName, bool createReplicationAction)
            : this(tableName)
        {
            CreateReplicationAction = createReplicationAction;
        }

        public SqlServerStatement(string tableName, StatementType statementType)
            : base(tableName,statementType)
        {
            command = new SqlCommand();
            
            using (var cmd = new SqlCommand())
            {
                KeyParamCollection = cmd.Parameters;
            }
        }
       
        public SqlServerStatement(string tableName, StatementType statementType, bool createReplicationAction)
            : this (tableName,statementType)
        {
            CreateReplicationAction = createReplicationAction;
        }

        public override DbParameter CreateParam(string name, object value)
        {
            return new SqlParameter(name, value);
        }

        public override DbParameter CreateParam(string name, object value, SqlDbType type)
        {
            return new SqlParameter(name, value) {SqlDbType = type};
        }

        public override DbParameter MakeParam(string name, RecordIdentifier value)
        {
            return SqlServerParameters.MakeParam(command, name, value);
        }

        public override DbParameter MakeParam(string name, object value, SqlDbType type)
        {
            return SqlServerParameters.MakeParam(command, name, value, type);
        }

        public override DbParameter MakeParamNoCheck(string name, object value, SqlDbType type)
        {
            return SqlServerParameters.MakeParamNoCheck(command, name, value, type);
        }

        private string GetCommandText()
        {
            string sql;

            if (StatementType == StatementType.Insert)
            {
                sql = "insert into " + TableName + "(";
                string sql2 = "values (";

                for (int i = 0; i < Fields.Count; i++)
                {
                    if (i != 0)
                    {
                        sql += ",";
                        sql2 += ",";
                    }

                    sql += AsReserved(Fields[i]);
                    sql2 += "@" + Fields[i];
                }

                sql += ")";
                sql2 += ")";

                sql = sql + " " + sql2;

                if (IsIdentityInsert)
                {
                    sql += ";  SELECT SCOPE_IDENTITY();";
                }
            }
            else if (StatementType == StatementType.Update)
            {
                sql = "update " + TableName + " set ";

                List<string> updateColumns = new List<string>();

                // Check if we are using an update optimizer
                if (UpdateColumnOptimizer != null && UpdateColumnOptimizer.ColumnUpdateValues.Count > 0 && !IgnoreColumnOptimizer)
                {
                    // If we have a field in the ColumnUpdateValues that does not exist in the standard Field list we need to take action
                    List<OptimizedUpdateColumn> errorColumns = new List<OptimizedUpdateColumn>();

                    foreach (OptimizedUpdateColumn column in UpdateColumnOptimizer.ColumnUpdateValues.Values)
                    {
                        updateColumns.Add(column.Name);

                        if (!Fields.Exists(p => p == column.Name))
                        {
                            switch (column.Action)
                            {
                                case OptimizedUpdateColumnAction.Error:
                                    errorColumns.Add(column);
                                    break;

                                case OptimizedUpdateColumnAction.Continue:
                                    updateColumns.Remove(column.Name);
                                    break;

                                default:
                                    throw new InvalidOperationException($"Unexpected action {column.Action} for column {column.Name}");
                            }
                        }
                    }

                    if (errorColumns.Count > 0)
                    {
                        string missingColumns = String.Join(Environment.NewLine, errorColumns.Select(x => x.Name));
                        string errorMessage = 
                            $@"The following columns were assigned to the object you are trying to save, but do not match the columns in the table {TableName}: {Environment.NewLine}{Environment.NewLine}{missingColumns}";

                        throw new DataIntegrityException(errorMessage);
                    }
                }
                else
                {
                    updateColumns = Fields;
                }

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < updateColumns.Count; i++)
                {
                    if (i != 0) sb.Append(",");

                    sb.Append(AsReserved(updateColumns[i]));
                    sb.Append("=@");
                    sb.Append(updateColumns[i]);
                }

                if (Conditions.Count > 0)
                {
                    sb.Append(" where ");

                    for (int i = 0; i < Conditions.Count; i++)
                    {
                        if (i != 0) sb.Append(" and ");

                        sb.Append(AsReserved(Conditions[i]));
                        sb.Append(ConditionOperatorCollection[i]);
                        sb.Append("@con");
                        sb.Append(Conditions[i]);
                    }
                }

                sql += sb.ToString();
            }
            else if (StatementType == StatementType.Delete)
            {
                sql = "delete from " + TableName;

                if (Conditions.Count > 0)
                {
                    sql += " where ";

                    for (int i = 0; i < Conditions.Count; i++)
                    {
                        if (i != 0) sql += " and ";

                        sql += AsReserved(Conditions[i]) + ConditionOperatorCollection[i] + "@con" + Conditions[i];
                    }
                }
            }
            else
            {
                return "";
            }

            return sql;
        }

        private static string AsReserved(string field)
        {
            if (SqlReservedKeywords.IsReserved(field))
                return "[" + field + "]";
            return field;
        }

        public override IDbCommand Command
        {
            get
            {
                string sql = GetCommandText();

                if (sql == "")
                {
                    return null;
                }

                command.CommandText = sql;

                return command;
            }
        }
    }
}