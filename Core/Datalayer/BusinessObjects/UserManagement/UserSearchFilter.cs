using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.UserManagement
{
    public class UserSearchFilter
    {
        public UserSearchFilter()
        {
            UserGroup = RecordIdentifier.Empty;
            UserProfile = RecordIdentifier.Empty;
            Enabled = true;
        }

        public string Username { get; set; }
        public bool UsernameBeginsWith { get; set; }
        public string Login { get; set; }
        public bool LoginBeginsWith { get; set; }
        public RecordIdentifier UserGroup { get; set; }
        public RecordIdentifier UserProfile { get; set; }
        public string NameOnReceipt { get; set; }
        public bool NameOnReceiptBeginsWith { get; set; }
        public int MaxCount { get; set; }
        public bool EnabledSet { get; set; }
        public bool Enabled { get; set; }
    }
}
