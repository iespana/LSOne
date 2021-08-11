using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.Controls;
using ListView = System.Windows.Forms.ListView;

namespace LSOne.ViewPlugins.User
{
    public class PluginEntry : IPlugin, IPluginDashboardProvider
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks    Framework = null;
        internal static int ServerUserImageIndex;
        internal static int UserImageIndex;
        internal static int UserDisabledImageIndex;
        internal static int UserGroupImageIndex;
        internal static int GreenLightImageIndex;
        internal static int RedLightImageIndex;
        internal static UserSearchPanelFactory UserSearchProvider;

        internal static Guid UserDashBoardItemID = new Guid("7428a51e-923d-4cb9-9fc1-d091119d0b73");
                
        private void AddUserSearch(object sender, SearchBarConstructionArguments args)
        {
            UserSearchProvider = new UserSearchPanelFactory();

            if (PluginEntry.DataModel.HasPermission(Permission.SecurityViewUsers))
            {
                args.AddItem(Properties.Resources.Users, UserImageIndex, UserSearchProvider, 200);
            }
        }
        
        private void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            ExtendedMenuItem item;
            ListViewItem selectedItem;

            switch (args.Key)
            {
                case "Insert":
                    if (DataModel.HasPermission(Permission.SecurityCreateNewUsers))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                            Properties.Resources.NewUser + "...",
                            Properties.Resources.new_user_16,
                            400,
                            PluginOperations.NewUser_Handler));
                    }
                    break;
                case "UserList":
                    if (((ListView) args.Context).SelectedItems.Count > 0)
                    {
                        selectedItem = ((ListView) args.Context).SelectedItems[0];

                        if (((SearchListViewItem) selectedItem).Key == "User")
                        {
                            item = new ExtendedMenuItem(Properties.Resources.EditUser + "...", 10, EditUser_Handler);
                            item.Default = true;

                            args.AddMenu(item);

                            args.AddSeparator(20);
                        }
                    }
                    break;
                case "UserSearchList":
                case "AllSearchList":
                    if (args.Key == "UserSearchList")
                    {
                        if (DataModel.HasPermission(Permission.SecurityCreateNewUsers))
                        {
                            args.AddSeparator(400);

                            args.AddMenu(new ExtendedMenuItem(
                                Properties.Resources.NewUser + "...",
                                Properties.Resources.new_user_16,
                                410,
                                PluginOperations.NewUser_Handler));
                        }
                    }

                    if (((ListView)args.Context).SelectedItems.Count > 0)
                    {
                        selectedItem = ((ListView)args.Context).SelectedItems[0];

                        if (((SearchListViewItem)selectedItem).Key == "User")
                        {
                            item = new ExtendedMenuItem(Properties.Resources.EditUser + "...", 10, EditUser_Handler);
                            item.Default = true;

                            args.AddMenu(item);

                            args.AddSeparator(20);

                            if (PluginOperations.CanChangePassword((Guid)(RecordIdentifier)selectedItem.Tag))
                            {
                                item = new ExtendedMenuItem(
                                    Properties.Resources.ChangePassword + "...",
                                    100,
                                    ChangePassword_Handler);

                                if (((UserListViewItem)selectedItem).User.IsDomainUser)
                                {
                                    item.Enabled = false;
                                }

                                args.AddMenu(item);
                            }

                            // The following operatons we cannot do on our self
                            if ((RecordIdentifier)selectedItem.Tag != DataModel.CurrentUser.ID)
                            {
                                // We can delete the user if it is not my self and if we have permission
                                if (DataModel.HasPermission(Permission.SecurityEnableDisableUser))
                                {
                                    if (((UserListViewItem)selectedItem).User.Login != "admin")
                                    {
                                        args.AddMenu(new ExtendedMenuItem(
                                            selectedItem.ImageIndex == UserDisabledImageIndex ? Properties.Resources.EnableLogin : Properties.Resources.DisableLogin,
                                            selectedItem.ImageIndex == UserDisabledImageIndex ? Properties.Resources.EnableLoginSmallImage : Properties.Resources.DisableLoginSmallImage,
                                            190,
                                            EnableDisableLogin_Handler));
                                    }
                                }

                                // We can delete the user if it is not my self and if we have permission
                                if (DataModel.HasPermission(Permission.SecurityDeleteUser))
                                {
                                    if (((UserListViewItem)selectedItem).User.Login != "admin")
                                    {
                                        args.AddMenu(new ExtendedMenuItem(
                                            Properties.Resources.Delete + "...",
                                            Properties.Resources.DeleteUserSmallImage,
                                            200,
                                            DeleteUser_Handler));
                                    }
                                }
                            }
                        }
                    }
                    break;

            }
        }

        

        private void EnableDisableLogin_Handler(object sender, EventArgs args)
        {
            ListView lv = (ListView)Framework.GetContextMenuContext();
            ListViewItem selectedItem = lv.SelectedItems[0];

            PluginOperations.SetUserEnabled((Guid)(RecordIdentifier)selectedItem.Tag, (selectedItem.ImageIndex == UserDisabledImageIndex));

            selectedItem.ImageIndex = (selectedItem.ImageIndex == UserDisabledImageIndex) ? UserImageIndex : UserDisabledImageIndex;
        }

        private void DeleteUser_Handler(object sender, EventArgs args)
        {
            ListView lv = (ListView)Framework.GetContextMenuContext();

            PluginOperations.DeleteUser((Guid)(RecordIdentifier)lv.SelectedItems[0].Tag);
        }

      

        private void ChangePassword_Handler(object sender, EventArgs args)
        {
            ListView lv = (ListView)Framework.GetContextMenuContext();

            PluginOperations.ChangePassword((Guid)(RecordIdentifier)lv.SelectedItems[0].Tag);
        }

        private void EditUser_Handler(object sender, EventArgs args)
        {
            ListView lv = (ListView)Framework.GetContextMenuContext();

            PluginOperations.ShowUser((Guid)(RecordIdentifier)lv.SelectedItems[0].Tag);
        }

        #region IPlugin Members

        public string Description
        {
            get
            {
                return Properties.Resources.UserManagement;
            }
        }

        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            if (message == "ViewUser")
            {
                return true;
            }

            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            if (message == "ViewUser")
            {
                PluginOperations.ShowUser((Guid)(RecordIdentifier)parameters);
            }

            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;

            Framework = frameworkCallbacks;

            // Register Icons that this plugin uses to the framework
            // -------------------------------------------------
            ImageList iml = frameworkCallbacks.GetImageList();

            iml.Images.Add(Properties.Resources.user_16);
            UserImageIndex = iml.Images.Count - 1;

            iml.Images.Add(Properties.Resources.disabled_user_16);
            UserDisabledImageIndex = iml.Images.Count - 1;

            iml.Images.Add(Properties.Resources.user_group_16);
            UserGroupImageIndex = iml.Images.Count - 1;

            iml.Images.Add(Properties.Resources.check_inverted_16);
            GreenLightImageIndex = iml.Images.Count - 1;

            iml.Images.Add(Properties.Resources.notification_inverted_16);
            RedLightImageIndex = iml.Images.Count - 1;

            iml.Images.Add(Properties.Resources.SSuser_16);
            ServerUserImageIndex = iml.Images.Count - 1;
            // -------------------------------------------------

            // We want to be able to add to sheet contexts from other plugins
            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);

            frameworkCallbacks.AddSearchBarConstructionHandler(AddUserSearch);
            frameworkCallbacks.AddMenuConstructionConstructionHandler(ConstructMenus);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);
        }

       

        public void Dispose()
        {
      
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.NewUser, PluginOperations.NewUser_Handler, Permission.SecurityCreateNewUsers);
            operations.AddOperation(Properties.Resources.ViewAllUsers, PluginOperations.ShowUsersListView, Permission.SecurityViewUsers);
            operations.AddOperation(Properties.Resources.NewUserGroup, PluginOperations.NewUserGroups_Handler,  new string[]{Permission.SecurityCreateUserGroups, Permission.SecurityManageGroupPermissions});
            operations.AddOperation(Properties.Resources.ViewUserGroups, PluginOperations.ManageUserGroups_Handler, Permission.SecurityManageGroupPermissions);
        }

        #endregion

        #region IPluginDashboardProvider Members
        public void LoadDashboardItem(IConnectionManager threadedEntry, ViewCore.Controls.DashboardItem item)
        {
            //System.Threading.Thread.Sleep(5000);

            int buttonIndex = 0;
            

            // In case if the plugin is registering more than one then we check which one it is though we will never get item from other plugin here.
            if (item.ID == UserDashBoardItemID) 
            {
                IUserData userProvider = Providers.UserData;

                int passwordLockoutThreshold = threadedEntry.Settings.GetSetting(
                    threadedEntry,
                LSOne.DataLayer.GenericConnector.Constants.Settings.PasswordLockoutThreshold,
                LSOne.DataLayer.GenericConnector.Enums.SettingsLevel.System).IntValue;

                int userCount = userProvider.GetUserCount(threadedEntry);
                int lockedOutUserCount = userProvider.GetLockedOutUserCount(threadedEntry, passwordLockoutThreshold);

                if(userCount == 1)
                {
                    item.State = DashboardItem.ItemStateEnum.Error;
                    item.InformationText = Properties.Resources.OnlyAdminSetUp;
                }
                else if(lockedOutUserCount > 0)
                {
                    item.State = DashboardItem.ItemStateEnum.Error;
                    item.InformationText = (lockedOutUserCount == 1) ?
                        Properties.Resources.OneUserIsLockedOut :
                        Properties.Resources.XUsersAreLockedOut.Replace("#1", lockedOutUserCount.ToString());
                }
                else
                {
                    item.State = DashboardItem.ItemStateEnum.Passed;
                    item.InformationText = Properties.Resources.XUsersHaveBeenSetUp.Replace("#1", userCount.ToString());
                    
                }


                if (PluginEntry.DataModel.HasPermission(Permission.SecurityCreateNewUsers))
                {
                    item.SetButton(buttonIndex, Properties.Resources.UsersNew,PluginOperations.NewUser_Handler);

                    buttonIndex++;
                }


                if (PluginEntry.DataModel.HasPermission(Permission.SecurityViewUsers))
                {
                    item.SetButton(buttonIndex, Properties.Resources.UsersManage, PluginOperations.ShowUsersListView);
                }
            }
        }

        
        public void RegisterDashBoardItems(DashboardItemArguments args)
        {
            // Here we often would put a permission check but Manage users has no permission, and sometimes the Dashboard item will show new user and sometimes
            // manage, so we let it slide here and will handle it in LoadDashboardItem since this dashboard item will always be avalible in some form.

            DashboardItem userDashboardItem = new DashboardItem(UserDashBoardItemID, Properties.Resources.Users, true, 60); // 60 min refresh interval

            args.Add(new DashboardItemPluginResolver(userDashboardItem,this),60); // Priority 60
        }
        #endregion
    }
}
