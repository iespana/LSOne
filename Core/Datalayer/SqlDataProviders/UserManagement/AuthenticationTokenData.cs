using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.UserManagement
{
    public class AuthenticationTokenData : SqlServerDataProviderBase, IAuthenticationTokenData
    {
        private static void PopulateToken(IDataReader dr, AuthenticationToken token)
        {
            token.TokenID = (Guid)dr["GUID"];
            token.UserID = (Guid)dr["USERGUID"];
            token.Text = (string)dr["DESCRIPTION"];
            token.TokenHash = (string)dr["HASH"];
        }

        /// <summary>
        /// Gets all authentication tokens for a given user
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="userID">ID of the user</param>
        /// <returns>All authentication tokens for the given user</returns>
        public virtual List<AuthenticationToken> GetTokensForUser(IConnectionManager entry, RecordIdentifier userID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                 @"Select GUID,USERGUID,DESCRIPTION,HASH
                   from USERLOGINTOKENS
                   where USERGUID = @userGuid and DATAAREAID = @dataAreaID
                   order by DESCRIPTION";

                MakeParam(cmd, "userGuid", (Guid)userID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<AuthenticationToken>(entry, cmd, CommandType.Text, PopulateToken);
            }
        }

        public virtual AuthenticationToken GetUserFromToken(IConnectionManager entry, AuthenticationToken token)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                 @"Select GUID, USERGUID, ISNULL(DESCRIPTION, '') DESCRIPTION, ISNULL(HASH, '') HASH
                   from USERLOGINTOKENS                   
                   where HASH = @TOKENHASH and DATAAREAID = @DATAAREAID";
                   

                MakeParam(cmd, "TOKENHASH", token.TokenHash);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                List<AuthenticationToken> result = Execute<AuthenticationToken>(entry, cmd, CommandType.Text, PopulateToken);

                return result.Count == 0 ? token : result[0];
            }
        }

        /// <summary>
        /// Deletes a authentication token with a given id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="tokenID">The ID of the token to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier tokenID)
        {
            DeleteRecord(entry, "USERLOGINTOKENS", "GUID", tokenID, "");
        }

        /// <summary>
        /// Checks if a authentication token with a given id exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="tokenID">The id of the token to check for</param>
        /// <returns>True if the token exists else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier tokenID)
        {
            return RecordExists(entry, "USERLOGINTOKENS", "GUID", tokenID);
        }

        /// <summary>
        /// Checks if a token hash is unique
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hash">The token hash to check for</param>
        /// <returns>True if the token has is unique, else false</returns>
        public virtual bool IsUnique(IConnectionManager entry, string hash)
        {
            return !RecordExists(entry, "USERLOGINTOKENS", "HASH", hash);
        }

        public virtual void Insert(IConnectionManager entry, AuthenticationToken token)
        {
            //ValidateSecurity(entry, BusinessObjects.Permission.VendorEdit);

            var statement = new SqlServerStatement("USERLOGINTOKENS");

            if (token.TokenID == RecordIdentifier.Empty)
            {
                token.TokenID = Guid.NewGuid();
            }

            token.Validate();

            if (!Exists(entry, token.TokenID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("GUID", (Guid)token.TokenID, SqlDbType.UniqueIdentifier);
                statement.AddKey("USERGUID", (Guid)token.UserID, SqlDbType.UniqueIdentifier);
                statement.AddField("DESCRIPTION", token.Text);
                statement.AddField("HASH", token.TokenHash);
            }

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
