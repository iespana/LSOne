using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using LSOne.Utilities.IO;
using System.Linq;

namespace LSOne.DataLayer.SqlConnector.DataProviders
{
	public class SqlServerDataProvider : SqlServerParameters
	{
		protected static List<T> GetList<T>(IConnectionManager entry, string tableName, string nameField, string idField, string orderField, bool hasDataAreaID = true)
			where T : class,new()
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				cmd.CommandText = "Select " + idField + ", ISNULL(" + nameField + ",'') as " + nameField + " from " + tableName +
					" order by " + orderField;

				return Execute<T>(entry, cmd, CommandType.Text, nameField, idField);
			}
		}

		protected static List<T> GetList<T>(IConnectionManager entry, string tableName, string nameField, string idField, string orderField,
			DataPopulator<T> populator)
			where T : class,new()
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				cmd.CommandText = "Select " + idField + ", ISNULL(" + nameField + ",'') as " + nameField + " from " + tableName +
					" order by " + orderField;

				return Execute(entry, cmd, CommandType.Text, populator);
			}
		}

		protected static List<T> GetList<T>(IConnectionManager entry, string tableName, string idField, string orderField)
			where T : class,new()
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				cmd.CommandText = "Select " + idField + " from " + tableName + " order by " + orderField;

				return Execute<T>(entry, cmd, CommandType.Text, idField, idField);
			}
		}

		protected static List<T> GetList<T>(IConnectionManager entry, IDbCommand cmd, RecordIdentifier id, DataPopulator<T> populator, CacheType cacheType) where T: class, new()
		{
			if (cacheType != CacheType.CacheTypeNone)
			{
				CacheBucket bucket = (CacheBucket)entry.Cache.GetEntityFromCache(typeof (CacheBucket), id);
				if (bucket != null)
				{
					return (List<T>) bucket.BucketData;
				}
			}
			List<T> result = Execute<T>(entry, cmd, cmd.CommandType, populator);
			if (result != null && cacheType != CacheType.CacheTypeNone)
			{
				entry.Cache.AddEntityToCache(id, new CacheBucket(id, "", result), cacheType);
			}
			return result;
		}

		protected static T Get<T>(IConnectionManager entry, IDbCommand cmd, CommandType commandType, RecordIdentifier id, object param,
		   DataPopulatorWithEntry<T> populator, CacheType cacheType, UsageIntentEnum usageIntent)
		   where T : class,new()
		{
			if (cacheType != CacheType.CacheTypeNone)
			{
				var entity = (T)entry.Cache.GetEntityFromCache(typeof(T), id);

				if (entity != null)
				{
					if (((IDataEntity)entity).UsageIntent >= usageIntent)
					{
						return entity;
					}
				}
			}

			var result = Execute(entry, cmd, commandType, param, populator);

			if (result.Count > 0)
			{
				if (cacheType != CacheType.CacheTypeNone)
				{
					entry.Cache.AddEntityToCache(id, (IDataEntity)result[0], cacheType);
				}

				return result[0];
			}

			return default(T);
		}

		protected static T Get<T>(IConnectionManager entry, IDbCommand cmd, RecordIdentifier id, DataPopulator<T> populator, CacheType cacheType,
			UsageIntentEnum usageIntent)
			where T : class, IDataEntity, new()
		{
			if (cacheType != CacheType.CacheTypeNone)
			{
				var entity = (T)entry.Cache.GetEntityFromCache(typeof(T), id);

				if (entity != null)
				{
					if (((IDataEntity)entity).UsageIntent >= usageIntent)
					{
						return entity;
					}
				}
			}

			var result = Execute(entry, cmd, CommandType.Text, populator);

			if (result.Count > 0)
			{
				if (cacheType != CacheType.CacheTypeNone)
				{
					entry.Cache.AddEntityToCache(id, (IDataEntity)result[0], cacheType);
				}

				return result[0];
			}

			return default(T);
		}


		protected static T GetDataEntity<T>(IConnectionManager entry, string tableName, string dataField, string idField, RecordIdentifier id)
			where T : IDataEntity, new()
		{
			ValidateSecurity(entry);

			var cmd = entry.Connection.CreateCommand();
			cmd.CommandText =
				"Select " + idField + " , ISNULL(" + dataField + ",'') AS " + dataField + " " +
				"From " + tableName + " " +
				"Where " + idField + "= @id";

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


		protected static void DeleteRecord(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string permission)
		{
			DeleteRecord(entry, tableName, fieldName, id, new[] { permission }, null);
		}

		

		protected static void DeleteRecord(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string permission,
			SqlTransaction transaction)
		{
			DeleteRecord(entry, tableName, fieldName, id, new[] { permission }, transaction);
		}

		protected static void DeleteRecord(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string[] permissions)
		{
			DeleteRecord(entry, tableName, fieldName, id, permissions, null);
		}

		protected static void DeleteRecord(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string[] permissions,
			SqlTransaction transaction)
		{
			ValidateSecurity(entry, permissions);

			var statement = new SqlServerStatement(tableName, StatementType.Delete);

			statement.AddCondition(fieldName, id.DBValue, id.DBType);

			if (transaction != null)
				entry.Connection.ExecuteStatement(statement, transaction);
			else
				entry.Connection.ExecuteStatement(statement);
		}

		protected static void DeleteRecord<T>(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, string permission)
			where T : class
		{
			DeleteRecord<T>(entry, tableName, fieldName, id, new[] { permission }, null);
		}

		protected static void DeleteRecord<T>(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id,
			string[] permissions, SqlTransaction transaction)
			where T : class
		{
			ValidateSecurity(entry, permissions);

			entry.Cache.DeleteEntityFromCache(typeof(T), id);

			var statement = new SqlServerStatement(tableName, StatementType.Delete);

			statement.AddCondition(fieldName, id.DBValue, id.DBType);

			if (transaction != null)
				entry.Connection.ExecuteStatement(statement, transaction);
			else
				entry.Connection.ExecuteStatement(statement);
		}

		protected static void DeleteRecord(IConnectionManager entry, string tableName, string[] fieldNames, RecordIdentifier id, string permission)
		{
			ValidateSecurity(entry, permission);

			var statement = new SqlServerStatement(tableName, StatementType.Delete);

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

		protected static bool RecordExists(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, bool hasDataAreaID = true)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				cmd.CommandText = "Select " + fieldName + " " +
								  "from " + tableName + " where " + fieldName + " = @id";

				MakeParam(cmd, "id", id.DBValue, id.DBType);

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

		protected static bool RecordExists(IConnectionManager entry, string tableName, string[] fieldNames, RecordIdentifier id, bool hasDataAreaID = true, bool hasDeletedFlag = false)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = "Select " + fieldNames[0] + " " +
								  "from " + tableName + " where " + fieldNames[0] + " = @id ";

				MakeParam(cmd, "id", id.DBValue, id.DBType);

				for (int i = 1; i < fieldNames.Length; i++)
				{
					id = id.SecondaryID;
					cmd.CommandText += "and " + fieldNames[i] + " = @id" + i.ToString() + " ";

					MakeParam(cmd, "id" + i.ToString(), id.DBValue, id.DBType);
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


		protected static bool RecordExists(IConnectionManager entry, string tableName, string[] fieldNames, RecordIdentifier id,
			string[] excludeFieldNames, RecordIdentifier excludeID)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = "Select " + fieldNames[0] + " " +
								  "from " + tableName + " where " + fieldNames[0] + " = @id ";

				MakeParam(cmd, "id", id.DBValue, id.DBType);

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

		protected static bool RecordExists(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id,
			string[] excludeFields, RecordIdentifier[] exludeIDs)
		{
			var cmd = entry.Connection.CreateCommand();

			ValidateSecurity(entry);

			cmd.CommandText = "Select " + fieldName + " " +
							  "from " + tableName + " where " + fieldName + " = @id";

			MakeParam(cmd, "id", id.DBValue, id.DBType);

			for (int i = 1; i < excludeFields.Length; i++)
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
        /// Runs a Select count(*) SQL statement on the table         
        /// </summary>        
        /// <param name="entry">The database connection</param>
        /// <param name="tableName">The table to select the rows from</param>        
        /// <param name="hasDataAreaID">If true then the SQL statement will limit the results to a DataAreaID</param>
        /// <returns>The count of rows</returns>
		protected static int Count(IConnectionManager entry, string tableName, bool hasDataAreaID = true)
		{
			try
			{
				IDbCommand cmd = entry.Connection.CreateCommand();
				cmd.CommandText = "SELECT COUNT(*) FROM " + tableName;

				if (hasDataAreaID)
				{
					cmd.CommandText += " WHERE DATAAREAID = @DATAAREAID";
					MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
				}

				return (int)entry.Connection.ExecuteScalar(cmd);
			}
			catch
			{
				return 0;
			}
		}

        /// <summary>
        /// Runs a Select count(*) SQL statement on the table using the fieldName and id values to select the records to count.
        /// Example: SELECT COUNT(*) FROM tableName WHERE fieldName = id
        /// </summary>        
        /// <param name="entry">The database connection</param>
        /// <param name="tableName">The table to select the rows from</param>
        /// <param name="fieldName">The field name that should be within the WHERE statement</param>
        /// <param name="id">The value of the WHERE statement</param>
        /// <param name="hasDataAreaID">If true then the SQL statement will limit the results to a DataAreaID</param>
        /// <returns>The count of rows</returns>
        protected static int Count(IConnectionManager entry, string tableName, string fieldName, RecordIdentifier id, bool hasDataAreaID = true)
        {
            try
            {
                IDbCommand cmd = entry.Connection.CreateCommand();
                cmd.CommandText =  " SELECT COUNT(*) FROM " + tableName + " ";
                cmd.CommandText += " WHERE " + fieldName + " = @id ";

                MakeParam(cmd, "id", id.DBValue, id.DBType);

                if (hasDataAreaID)
                {
                    cmd.CommandText += " AND DATAAREAID = @DATAAREAID ";
                    MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                }

                return (int)entry.Connection.ExecuteScalar(cmd);
            }
            catch
            {
                return 0;
            }
        }

		protected static bool TableExists(IConnectionManager entry, string tableName)
		{
			return TableExists(entry, tableName, false);
		}

		protected static bool TableExists(IConnectionManager entry, string tableName, bool rethrowExceptions)
		{
			try
			{
				var cmd = entry.Connection.CreateCommand();
				cmd.CommandText = string.Format("SELECT CASE WHEN EXISTS((SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}')) THEN 1 ELSE 0 END",
					tableName);

				return (int)entry.Connection.ExecuteScalar(cmd) == 1;
			}
			catch
			{
				if (!rethrowExceptions)
					return false;

				throw;
			}
		}

		protected static bool FieldExists(IConnectionManager entry, string tableName, string fieldName)
		{
			return FieldExists(entry, tableName, fieldName, false);
		}

		protected static bool FieldExists(IConnectionManager entry, string tableName, string fieldName, bool rethrowExceptions)
		{
			try
			{
				var cmd = entry.Connection.CreateCommand();
				cmd.CommandText = string.Format("SELECT CASE WHEN EXISTS((SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' AND COLUMN_NAME = '{1}')) THEN 1 ELSE 0 END",
					tableName, fieldName);

				return (int)entry.Connection.ExecuteScalar(cmd) == 1;
			}
			catch
			{
				if (!rethrowExceptions)
					return false;

				throw;
			}
		}

		protected static List<T> Execute<T>(IConnectionManager entry, IDbCommand cmd, CommandType commandType, string dataField, string idField)
			where T : class,new()
		{
			var result = new List<T>();

			ValidateSecurity(entry);

			IDataReader dr = null;
			try
			{
				dr = entry.Connection.ExecuteReader(cmd, commandType);

				while (dr.Read())
				{
					var entity = (IDataEntity)new T();

					entity.ID = new RecordIdentifier(dr[idField]);
					entity.Text = (dataField == "" ? "" : (string)dr[dataField]);

					result.Add((T)entity);
				}
			}
#if DEBUG
			catch (Exception e)
			{
				File.WriteAllText(
					Path.Combine(
						FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData)
							.Child("LS Retail")
							.AbsolutePath, "commandText.txt"), cmd.CommandText + Environment.NewLine + e);

				throw;
			}
#endif
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

        protected static List<T> Execute<T>(IConnectionManager entry, IDbCommand cmd, CommandType commandType, DataPopulator<T> populator)
            where T : class, new()
        {
            var result = new List<T>();

            ValidateSecurity(entry);

            IDataReader dr = null;
            try
            {
                dr = entry.Connection.ExecuteReader(cmd, commandType);

                while (dr.Read())
                {
                    var item = new T();

                    populator(dr, item);

                    result.Add(item);
                }
            }
#if DEBUG
            catch (Exception e)
            {
                File.WriteAllText(
                    Path.Combine(
                        FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData)
                            .Child("LS Retail")
                            .AbsolutePath, "commandText.txt"), cmd.CommandText + Environment.NewLine + e);

                throw;
            }
#endif
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

        protected static List<T> Execute<T>(IConnectionManager entry, IDbCommand cmd, CommandType commandType, DataPopulator<T> populator, string[] idColumns)
            where T : IDataEntity, new()
        {
            var result = new List<T>();

            ValidateSecurity(entry);

            IDataReader dr = null;
            try
            {
                dr = entry.Connection.ExecuteReader(cmd, commandType);

                while (dr.Read())
                {
                    RecordIdentifier id = idColumns.Reverse()
                                                   .Select(x => new RecordIdentifier(dr[x]))
                                                   .Aggregate((c, n) => new RecordIdentifier(n, c));

                    var item = result.FirstOrDefault(x => x.ID == new RecordIdentifier(id));

                    // Handle special cases, like TradeAgreementEntry when sometimes OldID is used instead of ID
                    if (item == null)
                    {
                        item = result.AsParallel().FirstOrDefault(x => (RecordIdentifier)(x)["OldID"] == new RecordIdentifier(id));
                    }

                    if (item == null)
                    {
                        item = new T();

                        result.Add(item);
                    }

                    populator(dr, item);
                }
            }
#if DEBUG
            catch (Exception e)
            {
                File.WriteAllText(
                    Path.Combine(
                        FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData)
                            .Child("LS Retail")
                            .AbsolutePath, "commandText.txt"), cmd.CommandText + Environment.NewLine + e);

                throw;
            }
#endif
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

        protected static List<T> Execute<T>(IConnectionManager entry, IDbCommand cmd, CommandType commandType, DataPopulatorOut<T> populator)
			where T : class, new()
		{
			var result = new List<T>();

			ValidateSecurity(entry);

			IDataReader dr = null;
			try
			{
				T item;

				dr = entry.Connection.ExecuteReader(cmd, commandType);

				while (dr.Read())
				{
					populator(dr, out item);

					result.Add(item);
				}
			}
#if DEBUG
			catch (Exception e)
			{
				File.WriteAllText(
				   Path.Combine(
					   FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData)
						   .Child("LS Retail")
						   .AbsolutePath, "commandText.txt"), cmd.CommandText + Environment.NewLine + e);

				throw;
			}
#endif
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

		protected static List<T> Execute<T>(IConnectionManager entry, IDbCommand cmd, CommandType commandType, object param,
			DataPopulatorWithEntry<T> populator)
			where T : class,new()
		{
			var result = new List<T>();

			ValidateSecurity(entry);

			IDataReader dr = null;
			try
			{
				dr = entry.Connection.ExecuteReader(cmd, commandType);

				while (dr.Read())
				{
					var item = new T();

					populator(entry, dr, item, param);

					result.Add(item);
				}
			}
#if DEBUG
			catch (Exception e)
			{
				File.WriteAllText(
					Path.Combine(
						FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData)
							.Child("LS Retail")
							.AbsolutePath, "commandText.txt"), cmd.CommandText + Environment.NewLine + e);

				throw;
			}
#endif
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

		protected static List<T> Execute<T, S>(IConnectionManager entry, IDbCommand cmd, CommandType commandType, S param,
			NewableDataPopulatorWithEntry<T, S> populator) where T : class
		{
			var result = new List<T>();

			ValidateSecurity(entry);

			IDataReader dr = null;
			try
			{
				dr = entry.Connection.ExecuteReader(cmd, commandType);

				while (dr.Read())
				{
					var item = populator(entry, dr, param);

					result.Add(item);
				}
			}
#if DEBUG
			catch (Exception e)
			{
				File.WriteAllText(
				   Path.Combine(
					   FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData)
						   .Child("LS Retail")
						   .AbsolutePath, "commandText.txt"), cmd.CommandText + Environment.NewLine + e);

				throw;
			}
#endif
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

		protected static List<RecordIdentifier> Execute(IConnectionManager entry, IDbCommand cmd, CommandType commandType, string IDField)
		{
			List<RecordIdentifier> result = new List<RecordIdentifier>();

			ValidateSecurity(entry);

			IDataReader dr = null;
			try
			{
				dr = entry.Connection.ExecuteReader(cmd, commandType);

				while (dr.Read())
				{
					result.Add(RecordIdentifier.FromObject(dr[IDField]));
				}
			}
#if DEBUG
			catch (Exception e)
			{
				File.WriteAllText(
					Path.Combine(
						FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData)
							.Child("LS Retail")
							.AbsolutePath, "commandText.txt"), cmd.CommandText + Environment.NewLine + e);

				throw;
			}
#endif
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


		protected static void Execute<T, S, K>(IConnectionManager entry, ICollection<T> result, IDbCommand cmd, CommandType commandType,
			S param, K param2, NewableDualDataPopulatorWithEntry<T, S, K> populator)
			where T : class
		{
			ValidateSecurity(entry);

			IDataReader dr = null;
			try
			{
				dr = entry.Connection.ExecuteReader(cmd, commandType);

				while (dr.Read())
				{
					var item = populator(entry, dr, param, param2);

					result.Add(item);
				}
			}
#if DEBUG
			catch (Exception e)
			{
				File.WriteAllText(
				   Path.Combine(
					   FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData)
						   .Child("LS Retail")
						   .AbsolutePath, "commandText.txt"), cmd.CommandText + Environment.NewLine + e);

				throw;
			}
#endif
			finally
			{
				if (dr != null)
				{
					dr.Close();
					dr.Dispose();
				}
			}
		}

		protected static List<T> Execute<T, S>(IConnectionManager entry, IDbCommand cmd, CommandType commandType, ref S param,
			RefDataPopulatorWithEntry<T, S> populator)
			where T : class,new()
		{
			var result = new List<T>();

			ValidateSecurity(entry);

			IDataReader dr = null;
			try
			{
				dr = entry.Connection.ExecuteReader(cmd, commandType);

				while (dr.Read())
				{
					var item = new T();

					populator(entry, dr, item, ref param);

					result.Add(item);
				}
			}
#if DEBUG
			catch (Exception e)
			{
				File.WriteAllText(
				   Path.Combine(
					   FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData)
						   .Child("LS Retail")
						   .AbsolutePath, "commandText.txt"), cmd.CommandText + Environment.NewLine + e);

				throw;
			}
#endif
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

		protected static List<T> Execute<T, S>(IConnectionManager entry, IDbCommand cmd, CommandType commandType, ref S param,object param2,
	RefDataPopulatorWithEntryAndExtraParameter<T, S> populator)
	where T : class, new()
		{
			var result = new List<T>();

			ValidateSecurity(entry);

			IDataReader dr = null;
			try
			{
				dr = entry.Connection.ExecuteReader(cmd, commandType);

				while (dr.Read())
				{
					var item = new T();

					populator(entry, dr, item, ref param,param2);

					result.Add(item);
				}
			}
#if DEBUG
			catch (Exception e)
			{
				File.WriteAllText(
					Path.Combine(
						FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData)
							.Child("LS Retail")
							.AbsolutePath, "commandText.txt"), cmd.CommandText + Environment.NewLine + e);

				throw;
			}
#endif
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

		protected static List<IDataEntity> Execute<T>(IConnectionManager entry, string sql, string dataField, string idField)
			where T : IDataEntity, new()
		{
			var result = new List<IDataEntity>();

			ValidateSecurity(entry);

			IDataReader dr = null;
			try
			{
				dr = entry.Connection.ExecuteReader(sql);

				while (dr.Read())
				{
					result.Add(new T { ID = (Guid)dr[idField], Text = (string)dr[dataField] });
				}
			}
#if DEBUG
			catch (Exception e)
			{
				File.WriteAllText(
					Path.Combine(
						FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData)
							.Child("LS Retail")
							.AbsolutePath, "commandText.txt"), sql + Environment.NewLine + e);

				throw;
			}
#endif
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

		protected static Dictionary<string, object> ExecuteSingleRow(IConnectionManager entry, IDbCommand cmd, CommandType commandType)
		{
			var values = new Dictionary<string, object>();

			ValidateSecurity(entry);

			IDataReader dr = null;
			try
			{
				dr = entry.Connection.ExecuteReader(cmd, commandType);

				if (dr.Read())
				{
					for (int i = 0; i < dr.FieldCount; i++)
					{
						values.Add(dr.GetName(i), dr[i]);
					}
				}
			}
#if DEBUG
			catch (Exception e)
			{
				File.WriteAllText(
					Path.Combine(
						FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData)
							.Child("LS Retail")
							.AbsolutePath, "commandText.txt"), cmd.CommandText + Environment.NewLine + e);

				throw;
			}
#endif
			finally
			{
				if (dr != null)
				{
					dr.Close();
					dr.Dispose();
				}
			}

			return values;
		}

		protected static void ValidateSecurity(IConnectionManager entry)
		{
			if (entry.IsAdmin)
			{
				return;
			}
			if (!entry.CurrentUser.IsValid)
			{
				if (entry.CurrentUser.SessionClosed)
				{
					throw new LSOneException("Session closed");
				}
				else
				{
					throw new PasswordChangeException();
				}
			}
		}

		protected static void ValidateSecurity(IConnectionManager entry, string permissionCode)
		{
			ValidateSecurity(entry);

			if (permissionCode != "")
			{
				if (!entry.HasPermission(permissionCode))
				{
					throw new PermissionException(permissionCode);
				}
			}
		}

		/// <summary>
		/// Checks if the user has any of the permissions in the permissionCodes. If he has none, an exception is thrown
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="permissionCodes">Array of permission codes</param>
		protected static void ValidateSecurity(IConnectionManager entry, string[] permissionCodes)
		{
			if (entry.IsAdmin)
			{
				return;
			}

			if (!entry.CurrentUser.IsValid)
			{
				if (entry.CurrentUser.SessionClosed)
				{
					throw new LSOneException("Session closed");
				}
				else
				{
					throw new PasswordChangeException();
				}
			}

			foreach (string permissionCode in permissionCodes)
			{
				if (permissionCode != "")
				{
					if (entry.HasPermission(permissionCode))
					{
						return;
					}
				}
				else
				{
					return;
				}
			}

			throw new PermissionException();
		}

		protected static void Save<T>(IConnectionManager entry, T entity, StatementBase statement)
			where T : class
		{
			entry.Cache.UpdateEntityInCache((IDataEntity)entity);

			entry.Connection.ExecuteStatement(statement);
		}


		#region Utility mapping methods - from SQL
		protected static DateTime AsDateTime(object result)
		{
			return AsDateTime(result, DateTime.MinValue);
		}

		protected static DateTime AsDateTime(object result, DateTime defaultValue)
		{
			if (result == null || result == DBNull.Value)
			{
				return defaultValue;
			}

			return (DateTime)result;
		}

		protected static bool AsBool(object result)
		{
			return AsBool(result, false);
		}

		protected static bool AsBool(object result, bool defaultValue)
		{
			if (result == null || result == DBNull.Value)
			{
				return defaultValue;
			}

			return Convert.ToBoolean(result);
		}

		protected static int AsInt(object result)
		{
			return AsInt(result, 0);
		}

		protected static int AsInt(object result, int defaultValue)
		{
			if (result == null || result == DBNull.Value)
			{
				return defaultValue;
			}

			return Convert.ToInt32(result);
		}

		protected static decimal AsDecimal(object result)
		{
			return AsDecimal(result, 0);
		}

		protected static decimal AsDecimal(object result, decimal defaultValue)
		{
			if (result == null || result == DBNull.Value)
			{
				return defaultValue;
			}

			return Convert.ToDecimal(result);
		}

		protected static string AsString(object result)
		{
			return AsString(result, string.Empty);
		}

		protected static string AsString(object result, string defaultValue)
		{
			if (result == null || result == DBNull.Value)
			{
				return defaultValue;
			}

			return (string)result;
		}

		protected static Guid AsGuid(object result)
		{
			if (result == null || result == DBNull.Value || string.IsNullOrEmpty(result.ToString()))
			{
				return Guid.Empty;
			}

			return new Guid(result.ToString());
		}

		protected static Image AsImage(object result)
		{
			if (result == null || result == DBNull.Value)
			{
				return null;
			}
			if (result is byte[])
			{
				// Don't do a using around the stream, it will cause image errors later
				var stream = new MemoryStream((Byte[])result);
				try
				{
					return Image.FromStream(stream);
				}
				catch { }
			}

			return null;
		}

		protected static object FromImage(Image image)
		{
			if (image == null)
				return DBNull.Value;

			// Don't do a using around the stream, it will cause an error later
			var ms = new MemoryStream();
			// Default to PNG unless the image is Jpeg
			var fmt = ImageFormat.Png;
			if (image.RawFormat.Guid == ImageFormat.Jpeg.Guid)
				fmt = ImageFormat.Jpeg;
			image.Save(ms, fmt);
			return ms.ToArray();
		}

		/// <summary>
		/// Converts the given object to <see cref="byte"/>. If object is null it will return 0.
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		protected static byte AsByte(object result)
		{
			return AsByte(result, 0);
		}

		/// <summary>
		/// Converts the given object to <see cref="byte"/>. If object is null it will return the <paramref name="defaultValue"/>
		/// </summary>
		/// <param name="result"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		protected static byte AsByte(object result, byte defaultValue)
		{
			if (result == null || result == DBNull.Value)
			{
				return defaultValue;
			}

			return Convert.ToByte(result);
		}

		#endregion

		#region Utility mapping methods - to SQL
		public static object ToDateTime(DateTime dateTime)
		{
			if (dateTime == DateTime.MinValue || dateTime.Year < 1901)
				return DBNull.Value;

			return dateTime;
		}

		public static string ToString(string value, int maxLength)
		{
			if (value != null && value.Length > maxLength)
				return value.Substring(0, maxLength);

			return value;
		}
		#endregion
	}
}
