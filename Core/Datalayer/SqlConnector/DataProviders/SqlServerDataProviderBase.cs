using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlConnector.DataProviders
{
    public delegate void DataPopulator<T>(IDataReader dr, T item);
    public delegate void DataPopulatorOut<T>(IDataReader dr, out T item);
    public delegate void DataPopulatorWithEntry<T>(IConnectionManager entry, IDataReader dr, T item, object param);    
    public delegate T NewableDataPopulatorWithEntry<T, S>(IConnectionManager entry, IDataReader dr, S param);
    public delegate T NewableDualDataPopulatorWithEntry<T, S, K>(IConnectionManager entry, IDataReader dr, S param, K param2);

    public abstract class SqlServerDataProviderBase : SqlServerDataProvider
    {

        /// <summary>
        /// Gets the master ID of a record with the given readable ID. If the master ID is not found, then return Guid.Empty.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="ID">The value of the readable ID</param>
        /// <param name="table">The table to get the master ID from</param>
        /// <param name="field">The name of the readable ID field</param>
        /// <returns></returns>
        public Guid GetMasterID(IConnectionManager entry, RecordIdentifier ID, string table, string field)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $@"
                SELECT 
                    MASTERID 
                FROM 
                    {table} 
                    WHERE {field} = @ID";

                MakeParam(cmd, "ID", (string)ID);

                object result = entry.Connection.ExecuteScalar(cmd);

                return (result is DBNull || result == null) ? Guid.Empty : (Guid)result;
            }
        }

        public string GetReadableIDFromMasterID(IConnectionManager entry, RecordIdentifier ID, string table, string field)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $@"
                SELECT 
                     {field}
                FROM 
                    {table} 
                    WHERE MASTERID = @ID";

                MakeParam(cmd, "ID", (Guid)ID, SqlDbType.UniqueIdentifier);

                object result = entry.Connection.ExecuteScalar(cmd);

                return (result is DBNull) ? null : (string)result;
            }
        }

        protected virtual List<T> GetCompareListInBatches<T>(IConnectionManager entry, List<T> itemsToCompare,
            string tableName, string idColumn, List<TableColumn> selectionColumns, List<Join> joins,
            DataPopulator<T> populator)
            where T : IDataEntity, new()
        {
            return GetCompareListInBatches(entry, itemsToCompare, tableName, new string[] { idColumn }, selectionColumns, joins, populator);
        }

        protected virtual List<T> GetCompareListInBatches<T>(IConnectionManager entry, List<T> itemsToCompare,
            string tableName, string[] idColumns, List<TableColumn> selectionColumns, List<Join> joins,
            DataPopulator<T> populator)
            where T : IDataEntity, new()
        {
            var res = new List<T>();

            // Normally, MS SQL supports 2100 parameters; but, for some reasons, sometimes, this doesn't work with neither 2100 or 2099 params
            // So just choose 2098 so we are safe (hopefully)
            var maximumNumberOfParametersSupported = 2098;
            var batchSize = maximumNumberOfParametersSupported / idColumns.Count();
            var numberOfBatches = Math.Ceiling((double)itemsToCompare.Count / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var batch = itemsToCompare.Skip(i * batchSize).Take(batchSize).ToList();
                res.AddRange(GetCompareList(entry, batch, tableName, idColumns, selectionColumns, joins, populator));
            }

            return res;
        }

        private List<T> GetCompareList<T>(IConnectionManager entry, List<T> itemsToCompare,
            string tableName, string[] idColumns, List<TableColumn> selectionColumns, List<Join> joins,
            DataPopulator<T> populator)
            where T : IDataEntity, new()
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                var tableAlias = selectionColumns.First(x => String.Compare(x.ColumnName.Trim(), idColumns[0], true) == 0)
                                                 .TableAlias
                                                 .TrimEnd(new[] { '.' })
                                                 .ToUpper();

                var hasComposedPK = idColumns.Count() > 1;

                var condition = hasComposedPK ? GetConditionForComposedPK(itemsToCompare, idColumns, cmd, tableAlias) : 
                                                GetConditionForSimplePK(itemsToCompare, idColumns[0], cmd, tableAlias);

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery(tableName, tableAlias) + " OPTION (RECOMPILE)",
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    joins == null ? string.Empty : QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(new Condition { Operator = "AND", ConditionValue = condition }),
                    string.Empty
                );

                return Execute<T>(entry, cmd, CommandType.Text, populator, idColumns);
            }
        }

        private string GetConditionForSimplePK<T>(List<T> itemsToCompare, string idColumn, IDbCommand cmd, string tableAlias)
            where T : IDataEntity, new()
        {
            string condition = $"{tableAlias}.{idColumn} IN (";

            T last = itemsToCompare.Last();
            for (int i = 0; i < itemsToCompare.Count; i++)
            {
                condition += $"@{idColumn}" + i;
                if (!itemsToCompare[i].Equals(last))
                {
                    condition += ",";
                }
                else
                {
                    condition += ")";
                }
                MakeParam(cmd, idColumn + i, (string)(RecordIdentifier)(itemsToCompare[i])["ID"]);
            }

            return condition;
        }

        private string GetConditionForComposedPK<T>(List<T> itemsToCompare, string[] idColumns, IDbCommand cmd, string tableAlias)
            where T : IDataEntity, new()
        {
            var idColumnsWithAssociatedParams = new Dictionary<string, List<string>>();
            var idColumnsWithAssociatedParamsValues = new Dictionary<string, List<object>>();

            for (var itemToCompareIndex = 0; itemToCompareIndex < itemsToCompare.Count; itemToCompareIndex++)
            {
                var itemToCompare = itemsToCompare[itemToCompareIndex];

                var recordIdentifier = (RecordIdentifier)(itemToCompare)["ID"];

                // Handle special cases, like TradeAgreementEntry when sometimes OldID is used instead of ID
                if (recordIdentifier == RecordIdentifier.Empty)
                {
                    recordIdentifier = (RecordIdentifier)(itemToCompare)["OldID"];
                }

                var columnValues = GetIdValues(recordIdentifier).ToList();

                for (var idColumnIndex = 0; idColumnIndex < idColumns.Length; idColumnIndex++)
                {
                    var idColumn = idColumns[idColumnIndex];

                    var paramName = $"param{idColumn}{itemToCompareIndex}";

                    var columnName = $"{tableAlias}.{idColumn}";

                    var paramValue = columnValues[idColumnIndex].DBValue;

                    if (!idColumnsWithAssociatedParamsValues.ContainsKey(columnName))
                    {
                        idColumnsWithAssociatedParamsValues.Add(columnName, new List<object>());
                    }

                    if (idColumnsWithAssociatedParamsValues[columnName].Contains(paramValue))
                    {
                        continue;
                    }

                    idColumnsWithAssociatedParamsValues[columnName].Add(paramValue);

                    MakeParam(cmd, paramName, paramValue, columnValues[idColumnIndex].DBType);

                    if (!idColumnsWithAssociatedParams.ContainsKey(columnName))
                    {
                        idColumnsWithAssociatedParams.Add(columnName, new List<string>());
                    }

                    idColumnsWithAssociatedParams[columnName].Add($"@{paramName}");
                };
            };

            var partialConditions = idColumnsWithAssociatedParams.Select(x => $"{x.Key} IN ({string.Join(",", x.Value)})");

            var condition = string.Join(" AND ", partialConditions);

            return condition;
        }

        /// <summary>
        /// Generates an "IN" filter like this: "TABLEALIAS.COLUMN IN (@PARAM1, @PARAM2)"
        /// </summary>
        /// <param name="listOfIDs">The IDs to filter by</param>
        /// <param name="filterColumn">The column to use in filter</param>
        /// <param name="cmd">The cmd the condition is added to</param>
        /// <param name="tableAlias">The table alias</param>
        /// <returns></returns>
        public string GetListCondition(List<string> listOfIDs, string filterColumn, IDbCommand cmd, string tableAlias)
        {
            var distinctListOfIDs = listOfIDs.Distinct().ToList();

            string condition = $"{tableAlias}.{filterColumn} IN (";

            for (int i = 0; i < distinctListOfIDs.Count; i++)
            {
                var paramName = $"{tableAlias}{filterColumn}" + i;

                condition += $"@{paramName}";
                condition += i == distinctListOfIDs.Count - 1 ? ")" : ",";

                MakeParam(cmd, paramName, distinctListOfIDs[i]);
            }

            return condition;
        }

        private IEnumerable<RecordIdentifier> GetIdValues(RecordIdentifier recordIdentifier)
        {
            yield return recordIdentifier;

            if (recordIdentifier.SecondaryID != null)
            {
                foreach (var idValue in GetIdValues(recordIdentifier.SecondaryID))
                {
                    yield return idValue;
                };
            }
        }

        protected static new List<T> GetList<T>(IConnectionManager entry, string tableName, string nameField, string idField, string orderField, bool hasDataAreaID = true) 
            where T : class,new()
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select " + idField + ", ISNULL(" + nameField + ",'') as " + nameField + " from " +
                                  tableName;
                   
                if (hasDataAreaID)
                {
                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    cmd.CommandText += " where DATAAREAID = @dataAreaId";
                }
                cmd.CommandText += " order by " + orderField;
                return Execute<T>(entry, cmd, CommandType.Text, nameField, idField);
            }
        }

        protected static  List<T> GetList<T>(IConnectionManager entry, bool showDeleted ,string tableName, string nameField, string idField, string orderField, bool hasDataAreaID = true)
          where T : class, new()
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select " + idField + ", ISNULL(" + nameField + ",'') as " + nameField + " from " +
                                  tableName;
                string condition = string.Empty;
                if (!showDeleted)
                {
                    condition += " Where DELETED = 0";
                }
                if (hasDataAreaID && string.IsNullOrEmpty(condition))
                {
                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    condition  += " where DATAAREAID = @dataAreaId";
                }
                else if (hasDataAreaID)
                {

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    condition += " and DATAAREAID = @dataAreaId";
                }
                cmd.CommandText += condition;
                cmd.CommandText += " order by " + orderField;
                return Execute<T>(entry, cmd, CommandType.Text, nameField, idField);
            }
        }
        protected static new List<T> GetList<T>(IConnectionManager entry, string tableName, string nameField, string idField, string orderField, 
            DataPopulator<T> populator)
            where T : class,new()
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select " + idField + ", ISNULL(" + nameField + ",'') as " + nameField + " from " + tableName + 
                    " where DATAAREAID = @dataAreaId order by " + orderField;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute(entry, cmd, CommandType.Text, populator);
            }
        }

        protected static new List<T> GetList<T>(IConnectionManager entry, string tableName, string idField, string orderField)
            where T : class,new()
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select " + idField + " from " + tableName + " where DATAAREAID = @dataAreaId order by " + orderField;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<T>(entry, cmd, CommandType.Text, idField, idField);
            }
        }

        protected static new void DeleteRecord<T>(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string permission)
            where T : class
        {
            DeleteRecord<T>(entry, tableName, fieldName, id, new[] { permission }, null);
        }

        protected static new void DeleteRecord<T>(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, 
            string[] permissions, SqlTransaction transaction)
            where T : class
        {
            ValidateSecurity(entry, permissions);

            entry.Cache.DeleteEntityFromCache(typeof(T), id);

            var statement = new SqlServerStatement(tableName, StatementType.Delete);

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);

            statement.AddCondition(fieldName, id.DBValue, id.DBType);

            if (transaction != null)
            {
                entry.Connection.ExecuteStatement(statement, transaction);
            }
            else
            {
                entry.Connection.ExecuteStatement(statement);
            }
        }

        /// <summary>
        /// Preprocesses search texts, making brackets [] valid in search and ', making % valid in search and making * work as wild card character.
        /// </summary>
        /// <param name="searchText">The text to pre-process</param>
        /// <param name="applyScope">If set to true then % markers will be added to make begins with and ends with searches</param>
        /// <param name="applyBeginsWith">Makes scoped search be in begins mode rather than contains mode</param>
        /// <returns></returns>
        protected string PreProcessSearchText(string searchText, bool applyScope, bool applyBeginsWith)
        {
            // The order of things here is of most importance.
            // 1. First we need to filter out bracket meta characters since we might be adding some further down the line
            // 2. Then we filter out % putting it insice bracket meta characters.
            // 3. Then we apply the begins with vs not begins with condition
            // 4. And then we replace user input * with a %
            // 5. Finally we replace ' with ''
            string modifiedSearchText = searchText.Replace("[", "[[]").Replace("%", "[%]");

            if (applyScope)
            {
                modifiedSearchText = (applyBeginsWith ? "" : "%") + modifiedSearchText + "%";
            }
            return modifiedSearchText.Replace("*", "%");
        }

        protected static bool RecordExists<T>(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id) 
            where T : class
        {
            var  entity = (T)entry.Cache.GetEntityFromCache(typeof(T), id);

            if (entity != null)
            {
                return true;
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select " + fieldName + " " +
                                  "from " + tableName + " where " + fieldName + " = @id and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "id", id.DBValue, id.DBType);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    return dr.Read();
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }
        }

        protected static new T GetDataEntity<T>(IConnectionManager entry, string tableName, string dataField, string idField, RecordIdentifier id) 
            where T : IDataEntity,new()
        {
            ValidateSecurity(entry);

            var cmd = entry.Connection.CreateCommand();
            cmd.CommandText =
                "Select " + idField + " , ISNULL(" + dataField + ",'') AS " + dataField + " " +
                "From " + tableName + " " +
                "Where DATAAREAID = @dataAreaId and " + idField + "= @id";

            MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
            MakeParam(cmd, "id", id);

            var result = new T();
            IDataReader dr = null;
            try
            {
                dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                if (dr.Read())
                {
                    result.ID = (string)dr[idField];
                    result.Text = (string)dr[dataField];
                }
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
            }

            return result;
        }

        protected static  T GetDataEntity<T>(IConnectionManager entry, string tableName, string dataField, string idField, RecordIdentifier id, bool hasDataAreaID = true)
           where T : IDataEntity, new()
        {
            ValidateSecurity(entry);

            var cmd = entry.Connection.CreateCommand();
            cmd.CommandText =
                "Select " + idField + " , ISNULL(" + dataField + ",'') AS " + dataField + " " +
                "From " + tableName + " ";

            if (hasDataAreaID)
            {
                cmd.CommandText += "Where DATAAREAID = @dataAreaId and " + idField + "= @id";
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
            }
            else
            {
                cmd.CommandText += "Where " + idField + "= @id";
            }

            MakeParam(cmd, "id", id);

            var result = new T();
            IDataReader dr = null;
            try
            {
                dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                if (dr.Read())
                {
                    if (id.IsInteger)
                    {
                        result.ID = (int)dr[idField];
                    }
                    else if (id.IsBigInt)
                    {
                        result.ID = (Int64)dr[idField];
                    }
                    else if (id.IsGuid)
                    {
                        result.ID = (Guid)dr[idField];
                    }
                    else
                    {                        
                        result.ID = (string)dr[idField];
                    }                    

                    result.Text = (string)dr[dataField];
                }
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
            }

            return result;
        }

        protected static new void DeleteRecord(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string permission)
        {
            DeleteRecord(entry, tableName, fieldName, id, new[] { permission }, null);
        }
        protected static void DeleteRecord(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string permission, bool hasDataAreaID = true)
        {
            DeleteRecord(entry, tableName, fieldName, id, new[] { permission }, null, hasDataAreaID);
        }

        protected static new void DeleteRecord(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string permission, 
            SqlTransaction transaction)
        {
            DeleteRecord(entry, tableName, fieldName, id, new[] { permission }, transaction);
        }

        protected static new void DeleteRecord(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string[] permissions)
        {
            DeleteRecord(entry, tableName, fieldName, id, permissions, null);
        }

        protected static void DeleteRecord(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string[] permissions, 
            SqlTransaction transaction, bool hasDataAreaID = true)
        {
            ValidateSecurity(entry, permissions);

            var statement = new SqlServerStatement(tableName, StatementType.Delete);

            if (hasDataAreaID)
            {
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddCondition(fieldName, id.DBValue, id.DBType);

            if (transaction != null)
                entry.Connection.ExecuteStatement(statement, transaction);
            else
                entry.Connection.ExecuteStatement(statement);
        }

        protected static void DeleteRecord(IConnectionManager entry, string tableName, string[] fieldNames, RecordIdentifier id, string permission, bool hasDataAreaID = true)
        {
            DeleteRecord(entry, tableName, fieldNames, id, new string[] {permission}, hasDataAreaID);
        }

        protected static void DeleteRecord(IConnectionManager entry, string tableName, string[] fieldNames, RecordIdentifier id, string[] permission, bool hasDataAreaID = true)
        {
            ValidateSecurity(entry, permission);

            var statement = new SqlServerStatement(tableName, StatementType.Delete);

            if (hasDataAreaID)
            {
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            if (fieldNames.Length > 0)
            {
                statement.AddCondition(fieldNames[0], id.DBValue, id.DBType);
            }

            for (int i = 1; i < fieldNames.Length; i++)
            {
                id = id.SecondaryID;

                statement.AddCondition(fieldNames[i], id.DBValue, id.DBType);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        protected static void MarkAsRestored(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string permission)
        {
            if (id.IsEmpty)
            {
                return;
            }

            ValidateSecurity(entry, permission);

            SqlServerStatement statement = new SqlServerStatement(tableName);
            statement.StatementType = StatementType.Update;

            statement.AddField("DELETED", false, SqlDbType.Bit);
            statement.AddCondition(fieldName, id.DBValue, id.DBType);

            entry.Connection.ExecuteStatement(statement);
        }

        protected static void MarkAsRestored(IConnectionManager entry, string tableName, string[] fieldNames, RecordIdentifier id, string permission)
        {
            if (id.IsEmpty)
            {
                return;
            }

            ValidateSecurity(entry, permission);

            SqlServerStatement statement = new SqlServerStatement(tableName);
            statement.StatementType = StatementType.Update;

            statement.AddField("DELETED", false, SqlDbType.Bit);

            for (int i = 0; i < fieldNames.Length; i++)
            {
                statement.AddCondition(fieldNames[i], id[i].DBValue, id[i].DBType);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        protected static void MarkAsDeleted(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string permission)
        {
            if (id.IsEmpty)
            {
                return;
            }

            ValidateSecurity(entry, permission);

            SqlServerStatement statement = new SqlServerStatement(tableName);
            statement.StatementType = StatementType.Update;

            statement.AddField("DELETED", true, SqlDbType.Bit);
            statement.AddCondition(fieldName, id.DBValue, id.DBType);

            entry.Connection.ExecuteStatement(statement);
        }

        protected static void MarkAsDeleted(IConnectionManager entry, string tableName, string[] fieldNames, RecordIdentifier id, string permission)
        {
            if (id.IsEmpty)
            {
                return;
            }

            ValidateSecurity(entry, permission);

            SqlServerStatement statement = new SqlServerStatement(tableName);
            statement.StatementType = StatementType.Update;

            statement.AddField("DELETED", true, SqlDbType.Bit);
            
            for (int i = 0; i < fieldNames.Length; i++)
            {
                statement.AddCondition(fieldNames[i], id[i].DBValue, id[i].DBType);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        protected static bool RecordExists(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, bool hasDataAreaID = true, bool hasDeletedFlag = false  )
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                if (hasDataAreaID)
                {
                    cmd.CommandText = "Select " + fieldName + " " +
                                      "from " + tableName + " where " + fieldName + " = @id and DATAAREAID = @dataAreaId";

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                }
                else
                {
                    cmd.CommandText = "Select " + fieldName + " " +
                                      "from " + tableName + " where " + fieldName + " = @id";
                }
                if (hasDeletedFlag)
                {
                    cmd.CommandText += " AND DELETED = 0";
                }

                MakeParam(cmd, "id", id.DBValue,id.DBType);
                
                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    return dr.Read();
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }
        }

        protected static new bool RecordExists(IConnectionManager entry, string tableName, string[] fieldNames, RecordIdentifier id, bool hasDataAreaID = true, bool hasDeletedFlag = false)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {

                if (hasDataAreaID)
                {
                    cmd.CommandText = "Select " + fieldNames[0] + " " +
                                      "from " + tableName + " where DATAAREAID = @dataAreaID and " + fieldNames[0] + " = @id ";

                    MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                }
                else
                {
                    cmd.CommandText = "Select " + fieldNames[0] + " " +
                                      "from " + tableName + " where " + fieldNames[0] + " = @id ";
                }

                MakeParam(cmd, "id", id.DBValue, id.DBType);
                
                for (int i = 1; i < fieldNames.Length; i++)
                {
                    id = id.SecondaryID;
                    cmd.CommandText += "and " + fieldNames[i] + " = @id" + i.ToString() + " ";

                    MakeParam(cmd, "id" + i.ToString(), id.DBValue, id.DBType);
                }
                if (hasDeletedFlag)
                {
                    cmd.CommandText += " AND DELETED = 0";
                }

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    return dr.Read();
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }
        }

        protected static new bool RecordExists(IConnectionManager entry, string tableName, string[] fieldNames, RecordIdentifier id,
            string[] excludeFieldNames, RecordIdentifier excludeID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select " + fieldNames[0] + " " +
                                  "from " + tableName + " where DATAAREAID = @dataAreaID and " + fieldNames[0] + " = @id ";

                MakeParam(cmd, "id", id.DBValue, id.DBType);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                for (int i = 1; i < fieldNames.Length; i++)
                {
                    id = id.SecondaryID;
                    cmd.CommandText += "and " + fieldNames[i] + " = @id" + i.ToString() + " ";

                    MakeParam(cmd, "id" + i.ToString(), id.DBValue, id.DBType);
                }

                cmd.CommandText += "and " + excludeFieldNames[0] + " <> @ide ";
                MakeParam(cmd, "ide", excludeID.ToString());

                for (int i = 1; i < excludeFieldNames.Length; i++)
                {
                    excludeID = excludeID.SecondaryID;
                    cmd.CommandText += "and " + excludeFieldNames[i] + " <> @ide" + i.ToString() + " ";

                    MakeParam(cmd, "ide" + i.ToString(), excludeID.DBValue, excludeID.DBType);
                }

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    return dr.Read();
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }
        }

        protected static new bool RecordExists(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, 
            string[] excludeFields, RecordIdentifier[] exludeIDs)
        {
            var cmd = entry.Connection.CreateCommand();

            ValidateSecurity(entry);

            cmd.CommandText = "Select " + fieldName + " " +
                              "from " + tableName + " where " + fieldName + " = @id AND DATAAREAID = @dataAreaID ";

            MakeParam(cmd, "id", id.DBValue, id.DBType);
            MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

            for (int i = 0; i < excludeFields.Length; i++)
            {
                cmd.CommandText += "and " + excludeFields[i] + " <> @id" + i.ToString() + " ";
                MakeParam(cmd, "id" + i.ToString(), exludeIDs[i].DBValue, exludeIDs[i].DBType);
            }

            IDataReader dr = null;
            try
            {
                dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                return dr.Read();
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a list of existing record identifiers between a starting number plus a maximum number of records
        /// 
        /// EXAMPLE:
        /// 
        /// Number sequence Format: XY######
        /// Starting record = 10
        /// Number of records = 10 (meaning that we look between 10 and 20 because we want to generate 10 IDs starting after 10)
        /// Existing data in the specified column is: XY00007, XY00008, XY00009, XY00010, XY00011
        /// 
        /// Data returned: XY00010, XY00011 because it respects the correct number sequence format and falls between the 10-20 range
        /// </summary>
        protected static List<RecordIdentifier> GetExistingRecords(IConnectionManager entry, string tableName, string fieldName, string format, int startingRecord, int numberOfRecords)
        {
            try
            {
                using (var cmd = entry.Connection.CreateCommand())
                {
                    ValidateSecurity(entry);

                    int splitIndex = format.IndexOf('#');
                    string key = format.Substring(0, splitIndex);
                    string numbers = format.Substring(splitIndex + 1);

                    cmd.CommandText = $@"SELECT {fieldName} FROM {tableName} WHERE {fieldName} = CASE WHEN {fieldName} LIKE '{key + numbers.Replace("#", "[0-9]")}' 
                                                               AND CAST(SUBSTRING({fieldName}, {format.Length - numbers.Length + 1}, {numbers.Length}) AS INT) BETWEEN {startingRecord} AND {startingRecord + numberOfRecords} 
                                                               THEN {fieldName} ELSE NULL END";

                    return Execute(entry, cmd, CommandType.Text, fieldName);
                }
            }
            catch //Database column type may not be numeric
            {
                return new List<RecordIdentifier>();
            }
        }
    }
}