using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.BusinessObjects.UserManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.User.Views;

namespace LSOne.ViewPlugins.User
{
    internal class PluginOperations
    {
        public static void ChangePassword(Guid userGuid)
        {
            PluginEntry.Framework.SuspendSearchBarClosing();

            ChangePasswordDialog dlg = new ChangePasswordDialog(PluginEntry.DataModel, PluginEntry.Framework, userGuid);

            dlg.ShowDialog(); // If we had wanted to know the result then we could check the return value here.

            PluginEntry.Framework.ResumeSearchBarClosing();
        }

        public static bool WarningAdmins(RecordIdentifier userID, RecordIdentifier groupID, UserInGroupResult result, CellEventArgs e, string groupName)
        {
            bool retValue = false;
            if (PluginEntry.DataModel.HasPermission(Permission.SecurityDeleteUser))
            {
                if (QuestionDialog.Show(Properties.Resources.DeletingAllAdminsQuestion.Replace("#1", groupName), Properties.Resources.DeletingAllAdmins.Replace("#1", groupName)) == DialogResult.Yes)
                {
                    //delete the admin
                    Providers.UserGroupData.RemoveUser(PluginEntry.DataModel, (Guid)userID, (Guid)groupID);
                    result.IsInGroup = false;

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "UserGroupAssignment", userID, null);
                    retValue = true;
                }
                else
                {
                    ((CheckBoxCell) e.Cell).Checked = true;
                    //DataLayer.UserGroupCommands.AddUser(PluginEntry.DataModel, (Guid)userID, (Guid)groupID);
                    result.IsInGroup = true;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "UserGroupAssignment", userID, null);
                }
            }
            return retValue;
        }

        public static bool CanChangePassword(Guid userGuid)
        {
            return (userGuid == (Guid)PluginEntry.DataModel.CurrentUser.ID) ||
                   (PluginEntry.DataModel.HasPermission(Permission.SecurityResetPassword));
        }

        public static void ShowUser(RecordIdentifier userID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.User((Guid)userID));
        }

        public static void ShowUsersListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.UsersView());
        }

        public static void ShowGroupPermissions()
        {
            ShowGroupPermissions(Guid.Empty, false);
        }

        internal static void ManageUserGroups_Handler(object sender, EventArgs args)
        {
            ShowGroupPermissions(Guid.Empty, false);
        }

        internal static void NewUserGroups_Handler(object sender, EventArgs args)
        {
            ShowGroupPermissions(Guid.Empty, true);
        }


        public static void ShowGroupPermissions(Guid groupGuid, bool createNew)
        {
            if (createNew && PluginEntry.Framework.ViewController.CurrentView is GroupPermissions)
            {
                ((GroupPermissions)PluginEntry.Framework.ViewController.CurrentView).CreateNewGroup();
            }
            else
            {
                PluginEntry.Framework.ViewController.Add(new GroupPermissions(groupGuid, createNew));
            }
        }

        public static bool DeleteUser(Guid userGuid)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (QuestionDialog.Show(
                Properties.Resources.DeleteUserQuestion,
                Properties.Resources.DeleteUser) == DialogResult.Yes)
            {
                Providers.UserData.Delete(PluginEntry.DataModel, userGuid);

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "User", userGuid, null);

                retValue = true;
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.UserDashBoardItemID);

            return retValue;
        }

        public static void SetUserEnabled(Guid userGuid, bool value)
        {
            Providers.UserData.SetEnabled(PluginEntry.DataModel, userGuid, value);

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, value ? DataEntityChangeType.Enable : DataEntityChangeType.Disable, "User", userGuid, null);

            if(value)
            {
                PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.UserDashBoardItemID);
            }
        }

        public static void NewUser()
        {
            DialogResult result;
            Dialogs.NewUserWizard dlg;

            if (PluginEntry.DataModel.HasPermission(Permission.SecurityCreateNewUsers))
            {
                dlg = new Dialogs.NewUserWizard(PluginEntry.DataModel);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "User", dlg.UserID, null);

                    ShowUser(dlg.UserID);

                    PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.UserDashBoardItemID);
                }
            }
        }

        internal static void NewUser_Handler(object sender, EventArgs args)
        {

            NewUser();

        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.Security, "Security", Properties.Resources.security_green_16), 500);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Security")
            {
                args.Add(new Item(Properties.Resources.Users, "Users", null), 100);

                if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageGroupPermissions))
                {
                    args.Add(new Item(Properties.Resources.UserGroups, "UserGroups", null), 200);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Security")
            {
                if (args.ItemKey == "Users")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.SecurityCreateNewUsers))
                    {
                        args.Add(new ItemButton(Properties.Resources.NewUser, Properties.Resources.NewUserDescription, NewUser_Handler), 100);
                    }
                    args.Add(new ItemButton(Properties.Resources.ViewAllUsers, Properties.Resources.ViewAllUsersDescription, ShowUsersListView), 200);
                    args.Add(new SearchItemButton(Properties.Resources.SearchForUser, SearchUsers), 300);
                }
                else if (args.ItemKey == "UserGroups")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.SecurityCreateUserGroups) &&
                        PluginEntry.DataModel.HasPermission(Permission.SecurityManageGroupPermissions))
                    {
                        args.Add(new ItemButton(Properties.Resources.NewUserGroup, Properties.Resources.NewUserGroupDescription, NewUserGroups_Handler), 100);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageGroupPermissions))
                    {
                        args.Add(new ItemButton(Properties.Resources.UserGroups, Properties.Resources.ViewUserGroupsDescription, ManageUserGroups_Handler), 200);
                    }


                }
            }
        }

        private static void FindAllUsers(object sender, EventArgs args)
        {
            PluginEntry.Framework.Search(PluginEntry.UserSearchProvider, "");
        }

        private static void SearchUsers(object sender, SearchEventArgs args)
        {
            PluginEntry.Framework.Search(PluginEntry.UserSearchProvider, args.Text);
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Users, "Users"), 600);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Users")
            {
                args.Add(new PageCategory(Properties.Resources.Users, "Users"), 70);
                args.Add(new PageCategory(Properties.Resources.UserGroups, "UserGroups"), 80);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Users")
            {
                if (args.CategoryKey == "Users")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.SecurityCreateNewUsers))
                    {
                        args.Add(new CategoryItem(
                            Properties.Resources.UsersNew,
                            Properties.Resources.NewUser,
                            Properties.Resources.NewUserTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.EndOfGroup,
                            null,
                            Properties.Resources.new_user_32,
                            new EventHandler(NewUser_Handler),
                            "NewUser"), 10);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.SecurityViewUsers))
                    {
                        args.Add(new CategoryItem(
                            Properties.Resources.Users,
                            Properties.Resources.Users,
                            Properties.Resources.UsersTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.user_16,
                            new EventHandler(ShowUsersListView),
                            "ViewAllUsers"), 20);
                    }

                    /*args.AddEditField(new CategoryItem(
                            Properties.Resources.Search,
                            Properties.Resources.Search,
                            Properties.Resources.UserSearchTooltip,
                            CategoryItem.CategoryItemFlags.None,
                            "SearchUsers"), 100, new SearchEventHandler(SearchUsers));*/


                }
                else if (args.CategoryKey == "UserGroups")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageGroupPermissions))
                    {
                        if (PluginEntry.DataModel.HasPermission(Permission.SecurityCreateUserGroups))
                        {
                            args.Add(new CategoryItem(
                                Properties.Resources.UserGroupsNew,
                                Properties.Resources.NewUserGroup,
                                Properties.Resources.NewUserGroupTooltipDescription,
                                CategoryItem.CategoryItemFlags.Button,
                                Properties.Resources.new_user_group_16,
                                null,
                                new EventHandler(NewUserGroups_Handler),
                                "NewUserGroup"), 10);
                        }

                        if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageGroupPermissions))
                        {
                            args.Add(new CategoryItem(
                            Properties.Resources.UserGroups,
                            Properties.Resources.UserGroups,
                            Properties.Resources.UserGroupsTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.user_group_16,
                            null,
                            new EventHandler(ManageUserGroups_Handler),
                            "ViewUserGroups"), 20);
                        }

                    }
                }
            }
        }

        public static void ShowMigrateUsersDialog(object sender, EventArgs args)
        {
            Dialogs.MigrateUsersDialog dlg = new Dialogs.MigrateUsersDialog();
            dlg.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.Administration.Views.AdministrationView.View")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.MigrateUsers, new ContextbarClickEventHandler(ShowMigrateUsersDialog)), 249);
            }

            if (arguments.CategoryKey == "LSOne.ViewPlugins.Profiles.Views.UserProfileView.Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.Users, ShowUsersListView), 200);
            }
        }
    }
}
