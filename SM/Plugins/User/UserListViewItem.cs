using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.User
{
    internal class UserListViewItem : SearchListViewItem
    {
        private LSOne.DataLayer.BusinessObjects.UserManagement.User user;

        public UserListViewItem(ISearchPanelFactory provider, LSOne.DataLayer.BusinessObjects.UserManagement.User user, string text, int imageIndex)
            : base(provider,text, "User" ,imageIndex)
        {
            this.user = user;
            ID = user.Guid;
        }

        public LSOne.DataLayer.BusinessObjects.UserManagement.User User
        {
            get
            {
                return user;
            }
        }
    }
}
