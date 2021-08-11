using System;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.GenericConnector.DataEntities
{
    public class User : IUser, IDataEntity
    {
        // Either all users are audited or none
        static bool writeAudit;

        UserProfileBase profile;

        RecordIdentifier id;
        string text;
        bool activeDirectoryUser;
        bool forcePasswordChange;
        bool sessionClosed;
        RecordIdentifier staffID;

        int daysUntilExpires;   // If zero then the password is not about to expire


        public User(string userName, RecordIdentifier userID, RecordIdentifier staffID, bool isActiveDirectoryUser, UserProfileBase profile, bool needsPasswordChange, int countOfDaysUntilExpires, bool isLocked = false)
        {
            id = userID;
            text = userName;

            this.staffID = staffID;
            this.activeDirectoryUser = isActiveDirectoryUser;
            this.forcePasswordChange = needsPasswordChange;
            this.daysUntilExpires = countOfDaysUntilExpires;

            this.profile = profile;

            if (profile != null)
            {
                GetSettings();
            }

            this.sessionClosed = false;
            IsLocked = isLocked;
        }

        public string UserName
        {
            get
            {
                return Text;
            }
        }

        public bool ActiveDirectoryUser
        {
            get { return activeDirectoryUser; }
        }

        public int DaysUntilPasswordExpires
        {
            get { return daysUntilExpires; }
        }

        public bool ForcePasswordChange
        {
            get { return forcePasswordChange && !activeDirectoryUser; }
            set { forcePasswordChange = value; }
        }

        public bool SessionClosed
        {
            get { return sessionClosed; }
            set { sessionClosed = value; }
        }

        public static bool WriteAudit
        {
            get { return writeAudit; }
            set { writeAudit = value; }
        }

        public bool IsValid
        {
            get { return (!ForcePasswordChange) && (!sessionClosed); }
        }

        public bool IsLocked { get; set; }

        public UserProfileBase Profile
        {
            get { return profile; }
            set
            {
                profile = value;
                GetSettings();
            }
        }

        public RecordIdentifier StaffID
        {
            get { return staffID; }
            set { staffID = value; }
        }

        private void GetSettings()
        {
            writeAudit = profile.Settings.ContainsKey(Settings.WriteAuditing) && profile.Settings[Settings.WriteAuditing] == "1";
        }

        internal bool HasPermission(string permissionUUID)
        {
            return profile.Permissions.ContainsKey(permissionUUID);
        }



        #region IDataEntity Members

        public virtual RecordIdentifier ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public virtual string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        public override string ToString()
        {
            return text;
        }

        public virtual string this[int index]
        {
            get { return (index == 0) ? id.ToString() : ((index == 1) ? text : ""); }
        }

        public virtual object this[string field]
        {
            get
            {
                return null;
            }
            set
            {
                
            }
        }

        #endregion


        public UsageIntentEnum UsageIntent
        {
            get
            {
                return UsageIntentEnum.Normal;
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void ToClass(System.Xml.Linq.XElement xmlAnswer, IErrorLog errorLogger = null)
        {
            throw new NotImplementedException();
        }

        public System.Xml.Linq.XElement ToXML(IErrorLog errorLogger = null)
        {
            throw new NotImplementedException();
        }
    }
}
