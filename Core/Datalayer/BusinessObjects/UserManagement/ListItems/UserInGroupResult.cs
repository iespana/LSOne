using System;

namespace LSOne.DataLayer.BusinessObjects.UserManagement.ListItems
{
    [Serializable()]
    public class UserInGroupResult : GroupResult
    {
        private bool isAdminGroup;

        public UserInGroupResult()
            : base()
        {
            isAdminGroup = false;
        }

        public UserInGroupResult(string groupName, Guid groupGuid, bool isAdminGroup, bool isInGroup)
            : base(groupName, groupGuid, isInGroup)
        {
            this.isAdminGroup = isAdminGroup;
        }

        public bool IsAdminGroup
        {
            get { return isAdminGroup; }
            internal set { isAdminGroup = value; }
        }
    }
}
