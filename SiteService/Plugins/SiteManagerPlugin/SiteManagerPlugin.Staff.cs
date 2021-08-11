using System;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual void StaffLogOff(ref bool retVal, string staffId, string storeId, string terminalId, LogonInfo logonInfo)
        {
            throw new NotImplementedException();
        }

        public virtual void StaffLogOn(ref bool retVal, ref string comment, string staffId, string storeId, string terminalId, string password, LogonInfo logonInfo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Changes password for given user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <param name="newPasswordHash">The hash of the new password</param>
        /// <param name="lastChangeTime">Sets the last change time for the password</param>
        /// <param name="logonInfo">Login credentials</param>
        /// <param name="needPasswordChange">Sets wether the user needs to change password</param>
        /// <param name="expiresDate">Sets the expire date for the password</param>
        public virtual bool ChangePasswordForUser(RecordIdentifier userID, string newPasswordHash, bool needPasswordChange, DateTime expiresDate, DateTime lastChangeTime, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(userID)}: {userID}");
                return dataModel.ChangePasswordHashForOtherUser(userID, newPasswordHash, needPasswordChange, expiresDate, lastChangeTime);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return false;
        }

        /// <summary>
        /// Indicates wether the given user needs to change his password
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <param name="logonInfo">Login credentials</param>
        /// <returns></returns>
        public virtual bool UserNeedsToChangePassword(RecordIdentifier userID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(userID)}: {userID}");
                return dataModel.UserNeedsPasswordChange(userID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return false;
        }

        /// <summary>
        /// Locks the user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <param name="logonInfo">Login credentials</param>
        public virtual void LockUser(RecordIdentifier userID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(userID)}: {userID}");
                dataModel.LockUser(userID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets the information about the current password satus for the given user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <param name="passwordHash">The hash of the current password</param>
        /// <param name="expiresDate">The date of the password expiration</param>
        /// <param name="lastChangeTime">The date when the password was last changed</param>
        /// <param name="logonInfo">Login credentials</param>
        public virtual void GetUserPasswordChangeInfo(RecordIdentifier userID, out string passwordHash, out DateTime expiresDate, out DateTime lastChangeTime, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            passwordHash = "";
            expiresDate = DateTime.MinValue;
            lastChangeTime = DateTime.MinValue;

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(userID)}: {userID}");
                dataModel.GetUserPasswordInfo(userID, out passwordHash, out expiresDate, out lastChangeTime);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e); 
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }
    }
}