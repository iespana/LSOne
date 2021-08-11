using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Localization;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.User
{
    class UserSearchPanelFactory : ISearchPanelFactory
    {
        private UserSearchPanel searchPanel = null;

        #region ISearchPanelProvider Members

        public void GetItemAllowedOperations(SearchListViewItem item, ref bool canEdit, ref bool canDelete, ref bool canRun)
        {
            canEdit = true;

            canDelete = PluginEntry.DataModel.HasPermission(Permission.SecurityDeleteUser);

            if(((Guid)item.ID == (Guid)PluginEntry.DataModel.CurrentUser.ID))
            {
                canDelete = false;
            }

            if (((UserListViewItem)item).User.Login == "admin")
            {
                canDelete = false;
            }
        }

        public void Add()
        {
            PluginOperations.NewUser();
        }

        public void Delete(SearchListViewItem item)
        {
            if (item == null)
                PluginOperations.DeleteUser((Guid)((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]).ID);
            else
                PluginOperations.DeleteUser((Guid)item.ID);
        }

        public void Edit(SearchListViewItem item)
        {
            if (item == null)
                DoubleClickedItem((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]);
            else
                DoubleClickedItem(item);
        }

        public SearchPanelBase GetPanel()
        {
            if (searchPanel == null)
            {
                searchPanel = new UserSearchPanel(this);
                searchPanel.ItemList.DoubleClick += new EventHandler(searchPanel_ItemDoubleClick);
            }
            return searchPanel;
        }

        public void ReceiveChangeInformation(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (searchPanel != null)
            {
                if (objectName == "User")
                {
                    if (changeHint == DataEntityChangeType.Delete)
                    {
                        // We can handle this one locally.
                        foreach (SearchListViewItem item in searchPanel.ItemList.Items)
                        {
                            if (item.Key == "User" && (Guid)item.ID == (Guid)changeIdentifier)
                            {
                                searchPanel.ItemList.Items.Remove(item);
                                break;
                            }
                        }
                    }
                    else
                    {
                        searchPanel.SearchAgain();
                    }
                }
            }
        }

        public void Close()
        {
            searchPanel = null;
        }

        public string SearchContextName
        {
            get { return Properties.Resources.Users; }
        }

        public void SearchAllHandler(string text, List<SearchListViewItem> items,int maxDisplay)
        {
            SearchListViewItem item;
            string loginName = "";
            Name name;
            List<LSOne.DataLayer.BusinessObjects.UserManagement.User> users;
           
            IProfileSettings settings = PluginEntry.DataModel.Settings;

            if (text == null)
            {
                text = "";
            }
            //lastSearch = searchText;

            if (!text.Contains(" ") && !text.Contains(","))
            {
                loginName = text;
            }

            name = NameParser.ParseName(text, false);

            users = Providers.UserData.FindUsers(PluginEntry.DataModel, name.First, name.Middle, name.Last, name.Suffix, loginName, maxDisplay);

            foreach (LSOne.DataLayer.BusinessObjects.UserManagement.User user in users)
            {
                //item.Tag = user.Guid;
                item = new UserListViewItem(this,
                    user,
                    settings.NameFormatter.Format(user.Name),
                        user.Disabled ? PluginEntry.UserDisabledImageIndex : (user.IsServerUser ? PluginEntry.ServerUserImageIndex : PluginEntry.UserImageIndex));

                item.SubItems.Add(user.Login);
                item.ID = user.Guid;

                items.Add(item);
            }
        }

        public void DoubleClickedItem(SearchListViewItem item)
        {
            PluginOperations.ShowUser((Guid)item.ID);
        }

        public int ItemCount
        {
            get
            {
                return searchPanel.ItemList.Items.Count;
            }
        }

        #endregion

        void searchPanel_ItemDoubleClick(object sender, EventArgs e)
        {
            if (searchPanel.ItemList.SelectedItems.Count > 0)
            {
                DoubleClickedItem((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]);
            }
        }
    }
}
