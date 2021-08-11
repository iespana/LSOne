using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Security;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Analyzer;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.EventArguments;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlConnector
{
	public class SqlServerConnection : ConnectionBase, IConnection
	{
		private Dictionary<string, bool> replicationExclusions = new Dictionary<string, bool>(StringComparer.CurrentCultureIgnoreCase);

		public bool DisableReplicationActions
		{
			get { return disableReplicationActions; }
			set { disableReplicationActions = value; }
		}

		private SqlConnection connection;

		private IDataReader lastReader;
		private List<SqlServerConnection> extraConnections;
		private bool disableReplicationActions = false;

		internal event ConnectionLostHandler ConnectionLost;

		internal protected SqlServerConnection(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, bool disableReplicationActionCreation, bool retry = false)
			: this(ConnectionString(server, windowsAuthentication, login, password, databaseName, connectionType, retry),disableReplicationActionCreation)
		{
			this.dataAreaID = dataAreaID;
			isServerUser = false;
			disableReplicationActions = disableReplicationActionCreation;
		}

		internal protected SqlServerConnection(string fullConnectionString, string dataAreaID, bool disableReplicationActionCreation)
			: this(fullConnectionString,disableReplicationActionCreation)
		{
			this.dataAreaID = dataAreaID;
			isServerUser = false;
			disableReplicationActions = disableReplicationActionCreation;
		}

		internal protected SqlServerConnection(SqlServerConnection parent, bool disableReplicationActionCreation)
			: this(parent.fullConnectionString,disableReplicationActionCreation)
		{
			dataAreaID = parent.dataAreaID;
			userID = parent.userID;
			isServerUser = parent.isServerUser;
			Open();
		}

		internal protected SqlServerConnection(string fullConnectionString, bool disableReplicationActionCreation)
		{
			userID = Guid.Empty;
			isServerUser = false;
			disableReplicationActions = disableReplicationActionCreation;
			connection = new SqlConnection(fullConnectionString);

			this.fullConnectionString = fullConnectionString;
			lastReader = null;
			extraConnections = null;

		}

		protected virtual SqlServerConnection CreateConnection(string fullConnectionString, string dataAreaID, Guid userID, bool isServerUser, bool disableReplicationActionCreation)
		{
			return new SqlServerConnection(fullConnectionString,disableReplicationActionCreation)
			{
				dataAreaID = dataAreaID,
				userID = userID,
				isServerUser = isServerUser
			};
		}

		protected SqlConnection Connection
		{
			get
			{
				return connection;
			}
			set
			{
				connection = value;
			}
		}

		protected static string ConnectionString(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, bool retry = false)
		{
			string connectionString = "Persist Security Info=False;";

			switch (connectionType)
			{
				case ConnectionType.TCP_IP:
					connectionString += "Network Library=DBMSSOCN;";
					break;
				case ConnectionType.SharedMemory:
					connectionString += "Network Library=dbmslpcn;";
					break;
				case ConnectionType.NamedPipes:
					connectionString += "Network Library=dbnmpntw;";
					break;
			}

			connectionString += "Initial Catalog=" + databaseName + ";";
			connectionString += "Packet Size=4096;";
			connectionString += "Pooling=False;";
			connectionString += retry? "Connect Timeout=120;": "Connect Timeout=15;";
			connectionString += "Workstation ID=" + SystemInformation.ComputerName + ";";
			connectionString += "Data Source=" + server + ";";

			if (windowsAuthentication)
			{
				connectionString += "Trusted_Connection=Yes;";
			}
			else
			{
				connectionString += "User ID=" + login + ";";
				connectionString += "Password=" + SecureStringHelper.ToString(password) + ";";
			}

			return connectionString;

		}

		public void Open()
		{
			connection.Open();

			SetContext();
		}

		public bool IsConnected
		{
			get
			{
				if (connection == null)
				{
					return false;
				}

				return connection.State != ConnectionState.Closed &&
					   connection.State != ConnectionState.Broken;
			}
		}

		private void Repair()
		{
			try
			{
				if (IsConnected)
				{
					connection.Close();
				}

				connection.Dispose();
				connection = null;
			}
			catch
			{
				// We do not care if something went wrong in the dispose function
				// since the connection is allready in bad state
			}

			connection = new SqlConnection(fullConnectionString);
			try
			{
				connection.Open();
			}
			catch (SqlException exception)
			{
				if (exception.Number == 53)
				{
					if (ConnectionLost != null)
					{
						var args = new ConnectionLostEventArguments(exception.Number);

						ConnectionLost(this, args);

						if (args.Retry)
						{
							Repair();
							return;
							//return ExecuteReader(cmd, type, behavior, transaction, true);
						}
					}

					throw new DatabaseException("There was an error executing a SQL query", exception, null);
				}
			}

			SetContext();
		}

		public void Close()
		{
			if (IsConnected)
			{
				connection.Close();
			}

			CleanPool();

			userID = Guid.Empty;
		}

		public void Dispose()
		{
			Close();

			connection.Dispose();
			connection = null;
		}

		protected virtual void SetContext()
		{
			if (userID != Guid.Empty && IsConnected)
			{
				var cmd = new SqlCommand("spSECURITY_SetContext_1_0");

				SqlServerParameters.MakeParam(cmd, "UserGUID", userID);

				ExecuteNonQuery(cmd,true); // passing reading only here since setting context only affects the connection and does not write any data

				currentContext = userID;
			}
		}

		public virtual void SetContext(RecordIdentifier userID)
		{
			// This operation is only allowed if we are a special server user as other users may not hide their audit track and impersonate someone else
			if (!isServerUser)
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

		public void RestoreContext()
		{
			if (currentContext != userID)
			{
				SetContext();
			}
		}

		private void CleanPool()
		{
			if (extraConnections != null)
			{
				for (int n = 0; n < extraConnections.Count; n++)
				{
					var con = extraConnections[n];

					if (con.lastReader == null || con.lastReader.IsClosed)
					{
						con.Close();
						extraConnections.RemoveAt(n);
						con.Dispose();
						n--;
					}
				}

				if (extraConnections.Count == 0)
				{
					extraConnections = null;
				}
			}
		}

		private IDataReader ExecuteReader(SqlCommand cmd, CommandType type, CommandBehavior behavior, IDbTransaction transaction, bool repair)
		{
			if (!IsConnected)
			{
				Open();
			}

			CleanPool();

			cmd.Connection = connection;
			cmd.CommandType = type;

			if (transaction != null)
			{
				cmd.Transaction = (SqlTransaction)transaction;
			}

			if (!string.IsNullOrEmpty(SessionDumpName))
				QueryDumper.DumpQuery(cmd);

			try
			{
				
				return cmd.ExecuteReader(behavior);
			}
			catch (SqlException exception)
			{
				if (exception.Number == 121)
				{
					if (ConnectionLost != null)
					{
						var args = new ConnectionLostEventArguments(exception.Number);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      

						ConnectionLost(this, args);

						if (args.Retry)
						{
							Repair();
							return ExecuteReader(cmd, type, behavior, transaction, true);
						}
					}

					throw new DatabaseException("There was an error executing a SQL query", exception, cmd);
				}

				if ((exception.Number == 11 || exception.Number == 10054) && repair)
				{
					// This is a bad state error which can happen for example if the network cable was
					// unplugged for a second, and also in other cases, this state is not detected by the
					// IsConnected property.

					// We will make one attempt to fix the situation.

					// 121 - Semaphore timeout exception

					Repair();
					
					return ExecuteReader(cmd, type, behavior, transaction, false);
				}
				
				throw new DatabaseException("There was an error executing a SQL query", exception, cmd);
			}
			catch (InvalidOperationException)
			{
				try
				{
                    #if (DEBUG == true)
                        MessageBox.Show("Debug warning, opening another connection, did you forget to close a recordset somewhere ?");
                    #endif

                    var con = CreateConnection(fullConnectionString,dataAreaID,userID,isServerUser,disableReplicationActions);
					   
					con.Open();
					//con.NativeConnection.StateChange += new StateChangeEventHandler(NativeConnection_StateChange);
					//con.NativeConnection.Disposed += new EventHandler(NativeConnection_Disposed);
					//con.lastReader = con.ExecuteReader(cmd, type, behavior, transaction, repair);

					if (extraConnections == null)
					{
						extraConnections = new List<SqlServerConnection>();
					}

					extraConnections.Add(con);

					return con.ExecuteReader(cmd, type, behavior, transaction, repair);
				}
				catch (Exception exception)
				{
					throw new DatabaseException(
						"There was an error executing a SQL query", exception, cmd);
				}
			}
		}

		internal object ExecuteScalar(SqlCommand cmd, SqlTransaction transaction)
		{
			if (!IsConnected)
			{
				Open();
			}

			CleanPool();

			cmd.Connection = connection;

			if (transaction != null)
			{
				cmd.Transaction = transaction;
			}

			if (!string.IsNullOrEmpty(SessionDumpName))
				QueryDumper.DumpQuery(cmd);

			try
			{
				return cmd.ExecuteScalar();
			}
			catch (SqlException exception)
			{
				if (exception.Number == 121)
				{
					if (ConnectionLost != null)
					{
						var args = new ConnectionLostEventArguments(exception.Number);

						ConnectionLost(this, args);

						if (args.Retry)
						{
							Repair();
							return cmd.ExecuteScalar();
						}
					}

					throw new DatabaseException("There was an error executing a SQL query", exception, cmd);
				}

				if ((exception.Number == 11 || exception.Number == 10054))
				{
					// This is a bad state error which can happen for example if the network cable was
					// unplugged for a second, and also in other cases, this state is not detected by the
					// IsConnected property.

					// We will make one attempt to fix the situation.

					Repair();

					return cmd.ExecuteScalar();
				}
				
				throw new DatabaseException("There was an error executing a SQL query", exception, cmd);
			}
		}

		public object ExecuteScalar(IDbCommand cmd)
		{
			return ExecuteScalar((SqlCommand)cmd, null);
		}

		#region ExecuteNonQuery

		protected override void ExecuteNonQuery(IDbCommand cmd, IDbTransaction transaction, bool repair, CommandType commandType)
		{
			if (!IsConnected)
			{
				Open();
			}

			CleanPool();

			cmd.Connection = connection;
			cmd.CommandType = commandType;

			if (transaction != null)
			{
				cmd.Transaction = transaction;
			}

			if (!string.IsNullOrEmpty(SessionDumpName))
				QueryDumper.DumpQuery(cmd);

			try
			{
			   cmd.ExecuteNonQuery();
			}
			catch (SqlException exception)
			{
				if (exception.Number == 121)
				{
					if (ConnectionLost != null)
					{
						var args = new ConnectionLostEventArguments(exception.Number);

						ConnectionLost(this, args);

						if (args.Retry)
						{
							Repair();
							ExecuteNonQuery((SqlCommand)cmd, (SqlTransaction)transaction, false);
						}
					}

					throw new DatabaseException("There was an error executing a SQL query", exception, cmd);
				}
				
				if ((exception.Number == 11 || exception.Number == 10054) && repair)
				{
					// This is a bad state error which can happen for example if the network cable was
					// unplugged for a second, and also in other cases, this state is not detected by the
					// IsConnected property.

					// We will make one attempt to fix the situation.

					Repair();

					ExecuteNonQuery((SqlCommand)cmd, (SqlTransaction)transaction, false);
				}
				else
				{
					throw new DatabaseException("There was an error executing a SQL statement", exception, cmd);
				}
			}
		}

		private void ExecuteNonQuery(SqlCommand cmd, SqlTransaction transaction, bool repair)
		{
			ExecuteNonQuery(cmd, transaction, repair, CommandType.StoredProcedure);
		}

		/* Not used
		private void ExecuteNonQuery(SqlCommand cmd, SqlTransaction transaction)
		{
			ExecuteNonQuery(cmd, transaction, true, CommandType.StoredProcedure);
		} */

		protected override void ExecuteNonQuery(IDbCommand cmd, CommandType commandType)
		{
			ExecuteNonQuery(cmd, null, true, commandType);
		}

		#endregion // ExecuteNonQuery

		#region IConnection Members
		public object NativeConnection
		{
			get { return connection; }
		}

		public ServerVersion DatabaseVersion
		{
			get
			{
				if (connection != null && connection.ServerVersion != null)
				{
					if (connection.ServerVersion.StartsWith("11."))
					{
						return ServerVersion.SQLServer2012;
					}
					if (connection.ServerVersion.StartsWith("10."))
					{
						return ServerVersion.SQLServer2008;
					}
					if(connection.ServerVersion.StartsWith("9."))
					{
						return ServerVersion.SqlServer2005;
					}
				}

				return ServerVersion.Unknown;
			}
		}

		public string DatabaseName
		{
			get
			{
				if (connection != null)
				{
					return connection.Database;
				}
				return string.Empty;
			}
		}

	  

		public override void InsertReplicationAction(string objectName, string parameters, DDStatementType ddStatementType, bool repair, IDbTransaction transaction)
		{
			if (replicationExclusions.ContainsKey(objectName) || disableReplicationActions || userID == Guid.Empty)
				return;

			var cmd = new SqlCommand
				{
					CommandText =
						"Insert into REPLICATIONACTIONS (Action, ObjectName, AuditContext, Parameters, DateCreated, DATAAREAID) "
				};

			cmd.CommandText += "values (@Action, @ObjectName, @AuditContext, @Parameters, @DateCreated, @DATAAREAID)";
			
			SqlServerParameters.MakeParam(cmd, "Action", ddStatementType, SqlDbType.Int);
			SqlServerParameters.MakeParam(cmd, "ObjectName", objectName);
			SqlServerParameters.MakeParam(cmd, "AuditContext", userID);

			SqlServerParameters.MakeParam(cmd, "Parameters", parameters);

			SqlServerParameters.MakeParam(cmd, "DateCreated", DateTime.Now, SqlDbType.DateTime);
			SqlServerParameters.MakeParam(cmd, "DATAAREAID", dataAreaID);

			ExecuteNonQuery(cmd, transaction, repair, CommandType.Text);
		}

		public override void DeleteFromTable(IDbCommand cmd, string tableName, CommandType commandType, IDbTransaction transaction)
		{
			ExecuteNonQuery(cmd, transaction, true, commandType);
		}

		public override void DeleteFromTable(IDbCommand cmd, string tableName, CommandType commandType)
		{
			DeleteFromTable(cmd, tableName, commandType, null);
		}

		/// <summary>
		/// Executes a stored procedure
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="readingOnly">Set to true if you swear that your only using it to read from output parameters then the operation will be excluded from replication</param>
		public void ExecuteNonQuery(IDbCommand cmd, bool readingOnly)
		{
			ExecuteNonQuery(cmd, readingOnly, CommandType.StoredProcedure);
		}

		public void ExecuteNonQuery(IDbCommand cmd, bool readingOnly, CommandType commandType)
		{
			ExecuteNonQuery(cmd, null, true, commandType);

			if (!readingOnly)
			{
				InsertReplicationAction(cmd.CommandText, ((SqlCommand)cmd).Parameters, DDStatementType.Procedure);
			}
		}

		internal void ExecuteNonQuery(SqlCommand cmd, bool readingOnly, CommandType commandType, IDbTransaction transaction)
		{
			ExecuteNonQuery(cmd, transaction, true, commandType);

			if (!readingOnly)
			{
				InsertReplicationAction(cmd.CommandText, cmd.Parameters, DDStatementType.Procedure,transaction);
			}
		}

		#region ExecuteReader

		public IDataReader ExecuteReader(string sql, IDbTransaction transaction)
		{
			return ExecuteReader(new SqlCommand(sql), CommandType.Text, CommandBehavior.Default, transaction, true);
		}

		public IDataReader ExecuteReader(string sql)
		{
			return ExecuteReader(new SqlCommand(sql), CommandType.Text, CommandBehavior.Default, null, true);
		}

		public IDataReader ExecuteReader(IDbCommand cmd, IDbTransaction transaction)
		{
			return ExecuteReader((SqlCommand)cmd, CommandType.StoredProcedure, CommandBehavior.Default, transaction, true);
		}

		public IDataReader ExecuteReader(IDbCommand cmd)
		{
			return ExecuteReader((SqlCommand)cmd, CommandType.StoredProcedure, CommandBehavior.Default, null, true);
		}

		public IDataReader ExecuteReader(IDbCommand cmd, CommandType commandType)
		{
			return ExecuteReader((SqlCommand)cmd, commandType, CommandBehavior.Default, null, true);
		}

		internal IDataReader ExecuteReader(SqlCommand cmd, CommandType commandType, IDbTransaction transaction)
		{
			IDataReader dr = ExecuteReader(cmd, commandType, CommandBehavior.Default, transaction, true);

			return dr;
		}

		public IDataReader ExecuteReader(IDbCommand cmd, CommandType commandType, CommandBehavior behavior)
		{
			return ExecuteReader((SqlCommand)cmd, commandType, behavior, null, true);
		}
		#endregion

		public IDbCommand CreateCommand()
		{
			return new SqlCommand(){ CommandTimeout = 120 };
		}

		public IDbCommand CreateCommand(string cmd)
		{
			return new SqlCommand(cmd){ CommandTimeout = 120 };
		}

		protected override string FormParameters(IDataParameterCollection parameters)
		{
			string resString = "";
			const string sepValue = "|";
			foreach (SqlParameter param in parameters)
			{
				string parameterName = param.ParameterName.TrimStart('@');
				switch (param.SqlDbType)
				{
					case SqlDbType.NVarChar:
						resString += parameterName + sepValue + param.Value;
						break;
					case SqlDbType.DateTime:
						resString += parameterName + sepValue
							+ GetIsoDateTime((DateTime)param.Value);
						break;
					case SqlDbType.Decimal:
						if (CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator.Length > 0)
						{
							string decValue = param.Value.ToString();
							param.Value = decValue.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0], '.');
						}
						resString += parameterName + sepValue + param.Value;
						break;
					case SqlDbType.UniqueIdentifier:
					case SqlDbType.Int:
					case SqlDbType.TinyInt:
					case SqlDbType.BigInt:
					case SqlDbType.Bit:
						resString += parameterName + sepValue + param.Value;
						break;
				}
				resString += sepValue;
			}
			// Removes the trailing space and comma
			return resString.TrimEnd().TrimEnd((sepValue).ToCharArray());
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

		public void AddReplicationExclusion(string objectName)
		{
			if (!replicationExclusions.ContainsKey(objectName))
				replicationExclusions[objectName] = true;
		}

		public void RemoveReplicationExclusion(string objectName)
		{
			if (replicationExclusions.ContainsKey(objectName))
				replicationExclusions.Remove(objectName);   
		}
	}
}
