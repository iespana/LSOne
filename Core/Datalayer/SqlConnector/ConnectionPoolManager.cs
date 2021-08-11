using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;

using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlConnector
{

	internal class UserConnection
	{
		private DateTime returnDateTime;

		public UserConnection()
		{
			returnDateTime = DateTime.Now;
		}

		public UserConnection(IConnectionManager connection)
		{
			Entry = connection;
		}

		public IConnectionManager Entry { get; set; }

		public DateTime ReturnTime
		{
			get { return returnDateTime; }
		}
	}

	public class ConnectionPoolManager
	{
		private byte[] GetBytes(string str)
		{
			byte[] bytes = new byte[str.Length*sizeof (char)];
			Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		private static string GetString(byte[] bytes)
		{
			char[] chars = new char[bytes.Length/sizeof (char)];
			Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return new string(chars);
		}

		private int createdConnections;
		private int count;
		private readonly object poolLock = new object();
		private bool isAdmin;
		private Guid challenge;
		
		private byte[] result;
		private bool valid;
		private RecordIdentifier storeID;

		public ConnectionPoolManager()
		{
			//isAdmin = false;
			challenge = Guid.NewGuid();
			valid = true;
			storeID = RecordIdentifier.Empty;
		}

		public ConnectionPoolManager(RecordIdentifier storeID)
		{
			//isAdmin = false;
			challenge = Guid.NewGuid();
			valid = true;
			this.storeID = storeID;
		}

		public Guid GetChallenge(string hashKey)
		{

			using (HMACSHA256 hmac = new HMACSHA256(GetBytes(hashKey)))
			{
				result = hmac.ComputeHash(GetBytes(challenge.ToString()));
			}
			return challenge;

		}

		public void AdminAuthenticate(byte[] externalBytes)
		{
			if (GetString(externalBytes) == GetString(result))
			{
				isAdmin = true;
			}
			else
			{
				valid = false;
			}
			Clear();

		}

		//private Dictionary<string, Dictionary<string, List<UserConnection>>> ConnectionPool =
		//    new Dictionary<string, Dictionary<string, List<UserConnection>>>();
		private Dictionary<string, List<UserConnection>> ConnectionPool =
			new Dictionary<string, List<UserConnection>>();

		/// <summary>
		/// Gets a connection from the pool
		/// </summary>
		/// <param name="server">The database server to connect to</param>
		/// <param name="windowsAuthentication">If true then windows authentication is used on the sql connection</param>
		/// <param name="serverLogin">The ID of the sql server user. If <paramref name="windowsAuthentication"/> is true then this is ignored</param>
		/// <param name="serverPassword">The password for the sql server user. If <paramref name="windowsAuthentication"/> is true then this is ignored</param>
		/// <param name="databaseName">The name of the database to connect to</param>
		/// <param name="login">The LS One user name</param>
		/// <param name="password">The password for the LS One user</param>
		/// <param name="connectionType">The type of sql server connection to use</param>
		/// <param name="connectionUsage"></param>
		/// <param name="dataAreaID"></param>
		/// <param name="externalReference">If set, this will be used as the identifier for the instance of the connection in the pool</param>
		/// <param name="useAdminConnection">By default the connection is created based on the results of calling <see cref="AdminAuthenticate"/>. Set this to false if you want a normal user connection where all profiles are loaded. This is typically used if the user's profile is not provided from another source</param>
		/// <returns></returns>
		public IConnectionManager GetConnection(
			string server, 
			bool windowsAuthentication, 
			string serverLogin,
			SecureString serverPassword,
			string databaseName, 
			string login, 
			SecureString password, 
			ConnectionType connectionType,
			ConnectionUsageType connectionUsage, 
			string dataAreaID,
			string externalReference = "",
			bool useAdminConnection = true)
		{
			if (valid)
			{
				UserConnection reply = null;
				lock (poolLock)
				{
					string reference;
					if(externalReference == "")
					{
						reference = isAdmin ? databaseName : login;
					}
					else
					{
						reference = externalReference;
					}
					
					if (ConnectionPool.ContainsKey(reference))
					{

						List<UserConnection> connections = ConnectionPool[reference];
						foreach (var userConnection in connections)
						{
							if (IsConnectionAlive(userConnection.Entry))
							{
								reply = userConnection;
								break;
							}
						}
						if (reply != null)
						{
							connections.Remove(reply);
							count--;
							if (connections.Count == 0)
							{
								ConnectionPool.Remove(reference);
							}
						}
					}


				}
				if (reply == null)
				{
					createdConnections++;
					reply = new UserConnection(ConnectionManagerFactory.CreateConnectionManager());
					reply.Entry.CurrentStoreID = storeID;
					if (!isAdmin || !useAdminConnection)
					{
						reply.Entry.Login(server, windowsAuthentication, serverLogin,
							serverPassword,
							databaseName, login, password, connectionType,
							connectionUsage, dataAreaID);
					}
					else
					{
						reply.Entry.AdminLogin(server, windowsAuthentication, serverLogin,
							serverPassword,
							databaseName, login, password, connectionType,
							connectionUsage, dataAreaID);
					}

				}

				return reply.Entry;
			}
			throw new Exception("Authentication Failed,  Please recreate the ConnectionPool");
		}

		public IConnectionManager GetConnection(
			string server, 
			bool windowsAuthentication, 
			string serverLogin,
			SecureString serverPassword,
			string databaseName, 
			Guid login,  
			ConnectionType connectionType,
			ConnectionUsageType connectionUsage, 
			string dataAreaID)
		{
			if (valid)
			{
				UserConnection reply = null;
				lock (poolLock)
				{
					string reference =  databaseName;
					if (ConnectionPool.ContainsKey(reference))
					{

						List<UserConnection> connections = ConnectionPool[reference];
						foreach (var userConnection in connections)
						{
							if (IsConnectionAlive(userConnection.Entry) && userConnection.Entry.CurrentUser.ID == login)
							{
								reply = userConnection;
								break;
							}
						}
						if (reply != null)
						{
							connections.Remove(reply);
							count--;
							if (connections.Count == 0)
							{
								ConnectionPool.Remove(reference);
							}
						}
					}


				}
				if (reply == null)
				{
					reply = new UserConnection(ConnectionManagerFactory.CreateConnectionManager());
					reply.Entry.CurrentStoreID = storeID;

					var loginResult = reply.Entry.AdminLogin(server, windowsAuthentication, serverLogin,
							serverPassword,
							databaseName, login, connectionType,
							connectionUsage, dataAreaID);
					if (loginResult != LoginResult.Success)
					{

						return null;
					}

					createdConnections++;

				}

				return reply.Entry;
			}
			throw new Exception("Authentication Failed,  Please recreate the ConnectionPool");
		}

	   
		public void ReturnConnection(IConnectionManager entry, int maxCount, int maxUserConnectionCount, string externalReference = null)
		{

			if (entry != null && IsConnectionAlive(entry) && entry.IsAdmin == isAdmin)
			{
				//string userReference = isAdmin ? "admin" : entry.CurrentUser.Text;
				lock (poolLock)
				{
					if (maxCount <= count)
					{
						string targetKey = null;
						UserConnection lastReturned = null;
						List<UserConnection> subConnectionPool = null;

						foreach (string connectionPoolKey in ConnectionPool.Keys)
						{
							var connectionPoolValue = ConnectionPool[connectionPoolKey];
							foreach (UserConnection userConnection in connectionPoolValue)
							{
								if (lastReturned == null || lastReturned.ReturnTime > userConnection.ReturnTime)
								{
									targetKey = connectionPoolKey;
									lastReturned = userConnection;
									subConnectionPool = connectionPoolValue;
								}
							}
						}


						if (subConnectionPool != null)
						{
							subConnectionPool.Remove(lastReturned);
							if (subConnectionPool.Count == 0)
							{
								ConnectionPool.Remove(targetKey);
							}

							count--;

						}
					}
					string reference = isAdmin ? entry.Connection.DatabaseName :
						externalReference ?? entry.CurrentUser.Text;
					

					if (ConnectionPool.ContainsKey(reference))
					{
						 List<UserConnection> userPool =
							ConnectionPool[reference];
						
							if (userPool == null)
							{
								userPool = new List<UserConnection>();
							}
					  
						if (maxUserConnectionCount > userPool.Count)
						{
							userPool.Add(new UserConnection(entry));
							count++;
						}
						else
						{
							bool addingAllowed = false;
							foreach (var userConnection in userPool)
							{
								if (!IsConnectionAlive(userConnection.Entry))
								{
									userPool.Remove(userConnection);
									count--;
									addingAllowed = true;
								}
							}
							if (addingAllowed)
							{
								userPool.Add(new UserConnection(entry));
								count++;
							}

						}
					}
					else
					{
						ConnectionPool.Add(reference,
							new List<UserConnection> {new UserConnection(entry)}
							);
						count++;
					}

				}
			}
		
		}


		public void Clear()
		{
			foreach (var connectionList in ConnectionPool)
			{
				foreach (var userConnection in connectionList.Value)
				{
					userConnection.Entry.LogOff();
				}
			}
			ConnectionPool.Clear();


		}

		private bool IsConnectionAlive(IConnectionManager entry)
		{


			try
			{

				using (var cmd = entry.Connection.CreateCommand())
				{
					cmd.CommandText = @" select 1 ";
					entry.Connection.ExecuteScalar(cmd);
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
	}

}