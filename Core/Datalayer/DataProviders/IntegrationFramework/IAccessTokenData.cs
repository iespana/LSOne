using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.IntegrationFramework;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.IntegrationFramework
{
    public interface IAccessTokenData : IDataProvider<AccessToken>, ISequenceable
    {
        /// <summary>
        /// Retrieves an access token from the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="senderDNS">The dns of the sender computer</param>
        /// <returns>Access token</returns>
        AccessToken Get(IConnectionManager entry, RecordIdentifier senderDNS);
        /// <summary>
        /// Marks the access token as active in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="senderDNS">The dns of the sender computer</param>
        void ActivateAccessToken(IConnectionManager entry, RecordIdentifier senderDNS);

        /// <summary>
        /// Marks the access token as unactive in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="senderDNS">The dns of the sender computer</param>
        void RevokeAccessToken(IConnectionManager entry, RecordIdentifier senderDNS);

        /// <summary>
        /// Retrieves a list of all access tokens in the database 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>List of access tokens</returns>
        List<AccessToken> GetIFTokenList(IConnectionManager entry);

        /// <summary>
        /// Runs a stored procedure and updates a dictionary kept in the integration framework containing all existing access tokens
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="dataSource"></param>
        /// <param name="windowsAuthentication"></param>
        /// <param name="sqlServerLogin"></param>
        /// <param name="sqlServerPassword"></param>
        /// <param name="databaseName"></param>
        /// <param name="connectionType"></param>
        /// <param name="dataAreaID"></param>
        /// <returns></returns>
        List<AccessToken> GetIFTokenListUnsecure(IConnectionManager entry,
                                                        string dataSource,
                                                        bool windowsAuthentication,
                                                        string sqlServerLogin,
                                                        SecureString sqlServerPassword,
                                                        string databaseName,
                                                        ConnectionType connectionType,
                                                        string dataAreaID);
    }
}
