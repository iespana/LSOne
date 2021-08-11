using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.Tracing;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.GenericConnector
{
    public abstract class StatementBase
    {
        public enum ConditionEnum
        {
            Equals,
            Greater,
            Lesser,
            GreaterOrEqual,
            LesserOrEqual
        }

        protected List<string> Fields { get; set; }
        protected List<string> Conditions { get; set; }
        protected List<string> ConditionOperatorCollection { get; set; }

        protected StatementBase(string tableName, StatementType statementType)
        {
            ConditionOperatorCollection = new List<string>();

            this.TableName = tableName;
            this.StatementType = statementType;

            Fields = new List<string>();
            Conditions = new List<string>();

            CreateReplicationAction = true;
            IgnoreColumnOptimizer = true;
        }

        public string ReplicationFilterCode { get; set; }

        protected internal bool CreateReplicationAction { internal get; set; }

        public IDataParameterCollection KeyParamCollection { get; protected set; }

        /// <summary>
        /// Gets or sets the object containing the optimized column collection for this statement. If set then this will be used when creating the update statement.
        /// If this value is null or if the <see cref="IOptimizedUpdate.ColumnUpdateList"/> is empty it will be ignored.
        /// </summary>
        public IOptimizedUpdate UpdateColumnOptimizer { get; set; }

        public StatementType StatementType { get; set; }

        public string TableName { get; set; }

        public bool IsIdentityInsert { get; set; }

        public abstract DbParameter CreateParam(string name, object value);
        public abstract DbParameter CreateParam(string name, object value, SqlDbType dbType);

        public abstract DbParameter MakeParam(string name, RecordIdentifier value);
        public abstract DbParameter MakeParam(string name, object value, SqlDbType type);

        public abstract DbParameter MakeParamNoCheck(string name, object value, SqlDbType type);
        public bool IgnoreColumnOptimizer { get; set; }
        public void AddField(string name, string value)
        {
            MakeParam(name, value);
            Fields.Add(name);
        }

        public void AddField(string name, object value, SqlDbType type)
        {
            Fields.Add(name);
            MakeParam(name, value, type);
        }

        public void AddFieldNoCheck(string name, object value, SqlDbType type)
        {
            Fields.Add(name);
            MakeParamNoCheck(name, value, type);
        }

        public void UpdateField(string name, object value)
        {
            DbParameter parameter;

            for (int i = 0; i < Command.Parameters.Count; i++)
            {
                parameter = (DbParameter)Command.Parameters[i];

                if (parameter.ParameterName == $"@{name}")
                {
                    parameter.Value = value;
                    return;
                }
            }

        }

        public bool HasField(string name)
        {
            for(int i = 0; i < Command.Parameters.Count; i++)
            {
                if (((DbParameter)Command.Parameters[i]).ParameterName == $"@{name}")
                {
                    return true;
                }
            }

            return false;
        }

        public void AddField(string name, object value, SqlDbType type, int length)
        {
            var prm = MakeParam(name, value, type);
            prm.Size = length;

            Fields.Add(name);
        }

        public void AddKey(string name, string value)
        {
            if (StatementType == StatementType.Insert)
            {
                MakeParam(name, value);
                Fields.Add(name);
                KeyParamCollection.Add(CreateParam(name, value));
            }
        }

        public void AddKey(string name, object value, SqlDbType type)
        {
            if (StatementType == StatementType.Insert)
            {
                var prm = CreateParam(name, value, type);

                MakeParam(name, value, type);
                Fields.Add(name);
                KeyParamCollection.Add(prm);
            }
        }

        public void AddKey(string name, object value, SqlDbType type, int length)
        {
            if (StatementType == StatementType.Insert)
            {
                var prm = MakeParam(name, value, type);
                prm.Size = length;

                Fields.Add(name);

                KeyParamCollection.Add(prm);
            }
        }

        public void AddCondition(string name, string value)
        {
            if (StatementType != StatementType.Insert)
            {
                MakeParam("con" + name, value);
                KeyParamCollection.Add(CreateParam(name, value));
                Conditions.Add(name);

                ConditionOperatorCollection.Add("=");
            }
        }

        public void UpdateCondition(int paramIndex, string name, object value)
        {
            if (StatementType == StatementType.Update)
            {
                DbParameter prm = (DbParameter)KeyParamCollection[name];

                if (prm != null && (paramIndex < Command.Parameters.Count && paramIndex >= 0))
                {
                    prm.Value = value;

                    ((DbParameter)Command.Parameters[paramIndex]).Value = value;
                }
            }
        }

        public void AddCondition(string name, object value, SqlDbType type)
        {
            if (StatementType != StatementType.Insert)
            {
                var prm = CreateParam(name, value, type);

                MakeParam("con" + name, value, type);
                KeyParamCollection.Add(prm);
                Conditions.Add(name);

                ConditionOperatorCollection.Add("=");
            }
        }

        public void AddCondition(string name, object value, SqlDbType type, ConditionEnum condition)
        {
            if (StatementType != StatementType.Insert)
            {
                var prm = CreateParam(name, value, type);

                MakeParam("con" + name, value, type);
                KeyParamCollection.Add(prm);
                Conditions.Add(name);

                string conditionString = "=";
                switch (condition)
                {
                    case ConditionEnum.Equals:
                        conditionString = "=";
                        break;
                    case ConditionEnum.Greater:
                        conditionString = ">";
                        break;
                    case ConditionEnum.Lesser:
                        conditionString = "<";
                        break;
                    case ConditionEnum.GreaterOrEqual:
                        conditionString = ">=";
                        break;
                    case ConditionEnum.LesserOrEqual:
                        conditionString = "<=";
                        break;
                }

                ConditionOperatorCollection.Add(conditionString);
            }
        }

        public void ChangeStatementType(StatementType newStatementType)
        {
            if(newStatementType == StatementType) { return; }

            StatementType = newStatementType;

            if (newStatementType == StatementType.Insert)
            {
                //Set conditions as key fields
                foreach(string condition in Conditions)
                {
                    if (Command != null)
                    {
                        ((DbParameter)Command.Parameters["@con" + condition]).ParameterName = "@" + condition;
                    }

                    if (!Fields.Contains(condition))
                    {
                        Fields.Add(condition);
                    }
                }

                Conditions.Clear();
                ConditionOperatorCollection.Clear();
            }
            else
            {
                //Set key fields as conditions
                foreach (var key in KeyParamCollection)
                {
                    DbParameter param = key as DbParameter;

                    if (Command != null)
                    {
                        ((DbParameter)Command.Parameters["@" + param.ParameterName]).ParameterName = "@con" + param.ParameterName;
                    }
                }

                foreach (var key in KeyParamCollection)
                {
                    DbParameter param = key as DbParameter;

                    if (!Conditions.Contains(param.ParameterName))
                    {
                        Conditions.Add(param.ParameterName);
                        ConditionOperatorCollection.Add("=");

                        if (Fields.Contains(param.ParameterName))
                        {
                            Fields.Remove(param.ParameterName);
                        }
                    }
                }
            }
        }

        public abstract IDbCommand Command
        {
            get;
        }
    }
}
