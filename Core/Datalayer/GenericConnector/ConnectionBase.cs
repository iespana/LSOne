using System;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.GenericConnector.Enums;

namespace LSOne.DataLayer.GenericConnector
{
    public abstract class ConnectionBase
    {
        public static string SessionDumpName { get; set; }

        protected Guid userID;
        protected bool isServerUser;
        protected Guid currentContext;
        protected string dataAreaID;
        protected internal string fullConnectionString;
        protected Guid id;

        public ConnectionBase()
        {

        }

        public Guid UserID
        {
            set
            {
                userID = value;
            }
        }

        public bool IsServerUser
        {
            get
            {
                return isServerUser;
            }
            set
            {
                isServerUser = value;
            }
        }

        public string DataAreaId
        {
            get { return dataAreaID; }
        }

        public Guid ID
        {
            get { return id; }
        }

        protected DDStatementType GetStatementType(System.Data.StatementType statementType)
        {
            switch (statementType)
            {
                case StatementType.Insert: return DDStatementType.Insert;
                case StatementType.Update: return DDStatementType.Update;
                case StatementType.Delete: return DDStatementType.Delete;
                default: return DDStatementType.Invalid;
            }
        }

        public void ExecuteStatement(StatementBase statement)
        {
            ExecuteStatement(statement, null);
        }

        public virtual void ExecuteStatement(StatementBase statement, IDbTransaction transaction)
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

            // Inserting into action table...
            if (statement.CreateReplicationAction)
            {
                InsertReplicationAction(statement.TableName, statement.KeyParamCollection, GetStatementType(statement.StatementType), transaction);
            }
        }

        protected abstract void ExecuteNonQuery(IDbCommand cmd, CommandType commandType);
        protected abstract void ExecuteNonQuery(IDbCommand cmd, IDbTransaction transaction, bool repair, CommandType commandType);

        public abstract void DeleteFromTable(IDbCommand cmd, string tableName, CommandType commandType, IDbTransaction transaction);
        public abstract void DeleteFromTable(IDbCommand cmd, string tableName, CommandType commandType);

        protected void InsertReplicationAction(string objectName, IDataParameterCollection parameters, DDStatementType ddStatementType, IDbTransaction transaction)
        {
            string parametersString;
            if (ddStatementType == DDStatementType.Procedure)
            {
                parametersString = FormParametersSP(parameters);
            }
            else
            {
                parametersString = FormParameters(parameters);
            }

            InsertReplicationAction(objectName, parametersString, ddStatementType, false, transaction);
        }

        protected void InsertReplicationAction(string objectName, IDataParameterCollection parameters, DDStatementType ddStatementType)
        {
            InsertReplicationAction(objectName, parameters, ddStatementType, null);
        }

        public void InsertReplicationAction(string objectName, string parameters, DDStatementType ddStatementType)
        {
            InsertReplicationAction(objectName, parameters, ddStatementType, false, null);
        }

        public abstract void InsertReplicationAction(string objectName, string parameters, DDStatementType ddStatementType, bool repair, IDbTransaction transaction);

        private string FormParametersSP(IDataParameterCollection parameters)
        {

            string resString = "";

            foreach (SqlParameter param in parameters)
            {
                string parameterName = param.ParameterName;

                switch (param.SqlDbType)
                {
                    case SqlDbType.NVarChar:
                        string value = param.Value.ToString();
                        resString += parameterName + "=N'" + value.Replace("'", "''") + "'";
                        break;

                    case SqlDbType.UniqueIdentifier:
                        resString += parameterName + "='" + param.Value + "'";
                        break;
                    case SqlDbType.DateTime:
                        resString += parameterName + "='" + GetIsoDateTime((DateTime)param.Value) + "'";
                        break;

                    case SqlDbType.Int:
                    case SqlDbType.TinyInt:
                    case SqlDbType.BigInt:
                    case SqlDbType.Decimal:
                    case SqlDbType.Bit:
                        resString += parameterName + "=" + param.Value;
                        break;
                }
                resString += ",";
            }
            // Removes the trailing space and comma
            return resString.TrimEnd().TrimEnd(',');
        }

        protected abstract string FormParameters(IDataParameterCollection parameters);
      

        protected string GetIsoDateTime(DateTime dateTime)
        {
            return dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
