using System;

namespace LSOne.DataLayer.BusinessObjects.UserManagement.ListItems
{
    [Serializable()]
    public class GroupResult
    {
        private Guid groupGuid;
        private string groupName;
        private bool isInGroup;

        public GroupResult()
        {
            groupGuid = Guid.Empty;
            groupName = "";
            isInGroup = false;
        }

        public GroupResult(string groupName, Guid groupGuid)
        {
            this.groupGuid = groupGuid;
            this.groupName = groupName;
            isInGroup = false;
        }

        public GroupResult(string groupName, Guid groupGuid, bool isInGroup)
        {
            this.groupGuid = groupGuid;
            this.groupName = groupName;
            this.isInGroup = isInGroup;
        }

        public Guid GroupGuid
        {
            get { return groupGuid; }
            internal set { groupGuid = value; }
        }

        public string GroupName
        {
            get { return groupName; }
            internal set { groupName = value; }
        }

        public bool IsInGroup
        {
            get { return isInGroup; }
            set { isInGroup = value; }
        }
    }
}
