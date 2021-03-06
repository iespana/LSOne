using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IPosEngine
    {
        HostSettings HostSettings { get; }

        /// <summary>
        /// Gets the settings associated with the given staff and terminal.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="staffID">The ID of the staff to get the settings for</param>
        /// <param name="terminalID">The ID of the terminal to get the settings for</param>
        /// <param name="storeID">The ID of the store to get the settings for</param>
        /// <returns></returns>
        ISettings GetSessionSettings(IConnectionManager entry, RecordIdentifier staffID, RecordIdentifier terminalID,
            RecordIdentifier storeID);
        
        /// <summary>
        /// Gets the <see cref="ISettings"/> instance associated with the base POSEngine user. This instance can be used when no user context is available. This is mostly applicable 
        /// when using the engine on a server where a caller does not supply a valid LS One user id.
        /// </summary>
        ISettings DefaultSettings { get; }

        /// <summary>
        /// Sets the <see cref="DefaultSettings"/> instance based on the given parameters. The <see cref="DefaultSettings"/> instance will then be loaded with the profile information
        /// from the given user and store.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="staffID">The ID of the staff to set the settings for</param>
        /// <param name="storeID">The ID of the store to set the settings for</param>
        void SetDefaultSettings(IConnectionManager entry, RecordIdentifier staffID, RecordIdentifier storeID);

        void ReturnConnection(IConnectionManager entry, out IConnectionManager externalEntry);        


        /// <summary>
        /// Gets a connection from the connection pool for the given credentials
        /// </summary>
        /// <param name="userID">The GUID ID of the user that will be logged on to the connection</param>
        /// <param name="userLogin">The logon (i.e "admin" or "101") that will be used to log on to the connection</param>
        /// <param name="password">The LS One user password for the user</param>
        /// <param name="database">The name of the database to connect to</param>
        /// <param name="terminalID">The ID of the terminal the user is operating on</param>
        /// <param name="storeID">The ID of the store</param>        
        /// <returns></returns>
        IConnectionManager GetConnectionManager(RecordIdentifier userID, string userLogin, SecureString password,
            string database, RecordIdentifier terminalID, RecordIdentifier storeID);

        /// <summary>
        /// Gets a connection from the connection pool using credentials provided internally by the engine. 
        /// </summary>
        /// <param name="database">The name of the database to connecto to</param>
        /// <param name="terminalID">The ID of the terminal the user is operating on</param>
        /// <param name="storeID">The ID of the store</param>    
        /// <returns></returns>
        IConnectionManager GetConnectionManager(string database, RecordIdentifier terminalID, RecordIdentifier storeID);

        /// <summary>
        /// Sets the LS One user that will be used when creating connections via the <see cref="GetConnectionManager(string, RecordIdentifier, RecordIdentifier)"/> function
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <param name="userLogin">The login name of the user</param>
        /// <param name="password">The password for the user</param>
        void SetConnectionPoolUser(Guid userID, string userLogin, SecureString password);

        /// <summary>
        /// Gest or sets the location of the LS One service assemblies. This path will be assigned onto every instance if <see cref="IConnectionManager"/> 
        /// generated by <see cref="GetConnectionManager(string, RecordIdentifier, RecordIdentifier)"/> or <see cref="GetConnectionManager(RecordIdentifier, string, SecureString, string, RecordIdentifier, RecordIdentifier)"/>.
        /// </summary>
        /// <remarks>
        /// By default the value of this property is "(main executable's working directory)\Services"
        /// </remarks>
        string ServiceBasePath { get; set; }

        /// <summary>
        /// Gets or sets wether the engine should suppress all POS UI. This is for example used when the engine is running on a server.
        /// </summary>
        bool SuppressUI { get; set; }
    }
}