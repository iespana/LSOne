using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {

        #region Staff Operation
        public virtual void StaffLogOn(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref bool retVal, ref string comment, string password)
        {

        }

        public virtual void StaffLogOff(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref bool retVal)
        {

        }

        public bool ChangePasswordForUser(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier userID, string newPasswordHash, bool needsPasswordChange, DateTime expiresDate, DateTime lastChangeTime)
        {
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.ChangePasswordForUser(userID, newPasswordHash, needsPasswordChange, expiresDate, lastChangeTime, CreateLogonInfo(entry)), false);

            return result;
        }

        public bool UserNeedsToChangePassword(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier userID)
        {
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.UserNeedsToChangePassword(userID, CreateLogonInfo(entry)), false);

            return result;
        }

        public virtual void LockUser(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier userID)
        {
            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }
                server.LockUser(userID, CreateLogonInfo(entry));
                Disconnect(entry);

            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public virtual void GetUserPasswordChangeInfo(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier userID, out string passwordHash, out DateTime expiresDate, out DateTime lastChangeTime)
        {
            passwordHash = "";
            expiresDate = DateTime.MinValue;
            lastChangeTime = DateTime.MinValue;

            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }
                server.GetUserPasswordChangeInfo(userID, out passwordHash, out expiresDate, out lastChangeTime, CreateLogonInfo(entry));
                Disconnect(entry);

            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        #endregion

    }
}
