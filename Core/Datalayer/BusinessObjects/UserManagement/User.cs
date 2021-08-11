using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.UserManagement
{
    [Serializable()]
    public class User : DataEntity
    {
        private bool isDomainUser;
        private bool disabled;

        private Name name;
        RecordIdentifier staffID;

        public User()
            : this("")
        {

        }

        /// <summary>
        /// Use this constructor when creating new users
        /// </summary>
        /// <param name="login"></param>
        public User(string login)
            : base()
        {
            ID = Guid.Empty;
            isDomainUser = false;
            Text = login;
            name = new Name();
            staffID = "";
        }

        /// <summary>
        /// Use this constructor when returning users from the server
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="login"></param>
        /// <param name="isDomainUser"></param>
        /// <param name="disabled"></param>
        public User(Guid guid, string login, bool isDomainUser, bool disabled)
            : base(guid,login)
        {
            this.isDomainUser = isDomainUser;
            name = new Name();
            this.disabled = disabled;
            staffID = "";
        }

        public bool IsServerUser { get; internal set; }

        public Name Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Returns formatted name, this property is by default empty, its used for reporting purposes
        /// and is up to the reporting engine to populate this parameter if it wishes to use it.
        /// </summary>
        public string FormattedName { get; set; }

        public string Login
        {
            get { return Text; }
            set { Text = value; }
        }

        public bool IsDomainUser
        {
            get { return isDomainUser; }
            set { isDomainUser = value; }
        }


        public Guid Guid
        {
            get { return (Guid)ID; }
            set { ID = value; }
        }

        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }
        }


        public RecordIdentifier StaffID
        {
            get { return staffID; }
            set { staffID = value; }
        }

        public string NameOnReceipt { get; set; }

        public string Culture { get; set; }

        public string LayoutName { get; set; }

        public string GroupName { get; set; }

        public string Email { get; set; }

        public string UserProfileName { get; set; }

        public RecordIdentifier UserProfileID { get; set; }

        public override string ToString()
        {
            return (FormattedName != "") ? FormattedName : Text;
        }
    }
}


