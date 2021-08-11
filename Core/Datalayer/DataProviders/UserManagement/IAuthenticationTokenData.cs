using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.UserManagement
{
    public interface IAuthenticationTokenData : IDataProviderBase<AuthenticationToken>
    {
        /// <summary>
        /// Gets all authentication tokens for a given user
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="userID">ID of the user</param>
        /// <returns>All authentication tokens for the given user</returns>
        List<AuthenticationToken> GetTokensForUser(IConnectionManager entry, RecordIdentifier userID);

        AuthenticationToken GetUserFromToken(IConnectionManager entry, AuthenticationToken token);

        /// <summary>
        /// Checks if a token hash is unique
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hash">The token hash to check for</param>
        /// <returns>True if the token has is unique, else false</returns>
        bool IsUnique(IConnectionManager entry, string hash);

        void Insert(IConnectionManager entry, AuthenticationToken token);

        void Delete(IConnectionManager entry, RecordIdentifier tokenID);

    }
}