using System;
using System.Collections.Generic;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.User.Dialogs;
using LSOne.ViewPlugins.User.Properties;

namespace LSOne.ViewPlugins.User.Views
{
    public partial class User : ViewBase
    {
        RecordIdentifier userID;
        LSOne.DataLayer.BusinessObjects.UserManagement.User user;
        bool            mainRecordDirty = false;
        RecordIdentifier posUserID;

        TabControl.Tab groupsTab;
        TabControl.Tab permissionsTab;

        public User()
        {
            InitializeComponent();
        }

        public User(Guid userID)
        {
            posUserID = RecordIdentifier.Empty;


            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.Help |
                ViewAttributes.ContextBar;

            InitializeComponent();

            this.userID = userID;

            if (!PluginEntry.DataModel.HasPermission(Permission.SecurityEditUser))
            {
                // If this permission is missing then everything on the sheet is read only
                userName.Enabled = false;
            }
        }

        protected override string SecondaryRevertText
        {
            get { return Properties.Resources.RevertConditions; }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("User", userID.PrimaryID, Properties.Resources.User, true));

            tabSheetTabs.GetAuditContexts(contexts);

            contexts.Add(new AuditDescriptor("UserLogins", userID.PrimaryID, Properties.Resources.UserLogins));
            contexts.Add(new AuditDescriptor("UserLoginTokens", userID.PrimaryID, Properties.Resources.AuthenticationTokens));
        }

        public override RecordIdentifier ID
        {
            get
            {
                return userID;
            }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.User + ": " + PluginEntry.DataModel.Settings.NameFormatter.Format(user.Name);
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.User;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            // Here we load the data for the sheet
            if (!isRevert)
            {
                user = Providers.UserData.Get(PluginEntry.DataModel, (Guid)userID);
                userID = new RecordIdentifier(userID,user.StaffID);
                
                userName.PopulateNamePrefixes(PluginEntry.DataModel.Cache.GetNamePrefixes());

                groupsTab = new TabControl.Tab(Properties.Resources.UserGroups, ViewPages.UserGroupsPage.CreateInstance);
                permissionsTab = new TabControl.Tab(Properties.Resources.Permissions, ViewPages.UserPermissionPage.CreateInstance);

                tabSheetTabs.AddTab(groupsTab);
                tabSheetTabs.AddTab(permissionsTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, userID);
            }
           

            posUserID = user.StaffID;

            if (isRevert)
            {
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }

            userName.NameRecord = user.Name;

            tbLoginName.Text = user.Login;
            chkIsDomainUser.Checked = user.IsDomainUser;
            chkUserIsDisabled.Checked = user.Disabled;

            HeaderText = Description;

            /*if (user.Disabled)
            {
                HeaderIcon = Properties.Resources.UserDisabledSmallImage;
            }
            else
            {
                HeaderIcon = user.IsServerUser ? Properties.Resources.ServerUser16 : Properties.Resources.UserDisabledSmallImage;
            }*/

            tabSheetTabs.SetData(isRevert, userID,user);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            // We want to listen to UserGroup broadcasts so we will refresh the UserGroup list
            switch (objectName)
            {
                case "User": 
                    if((Guid)changeIdentifier == userID)
                    {
                        if (changeHint == DataEntityChangeType.Delete)
                        {
                            PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                        }
                        else if (changeHint == DataEntityChangeType.Enable || changeHint == DataEntityChangeType.Disable)
                        {
                            user.Disabled = (changeHint == DataEntityChangeType.Disable);

                            //HeaderIcon = user.Disabled ? Properties.Resources.UserDisabledSmallImage : (user.IsServerUser ? Properties.Resources.ServerUser16 : Properties.Resources.UserDisabledSmallImage);

                            PluginEntry.Framework.ViewController.RebuildViewContextBar(this);

                            chkUserIsDisabled.Checked = user.Disabled;

                            InvalidateContextBar();
                        }
                    }
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        protected override bool DataIsModified()
        {
            // Here we are supposed to validate if there is need to save

            mainRecordDirty = true;

            if (userName.NameRecord != user.Name)       return true;
            if (posUserID != user.StaffID)              return true;

            mainRecordDirty = false;

            
            if (tabSheetTabs.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            // Here we are supposed to save
            
            if (mainRecordDirty)
            {
                user.Name = userName.NameRecord;
                user.StaffID = posUserID;

                Providers.UserData.Save(PluginEntry.DataModel, user);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "User", userID, null);
                PluginEntry.Framework.RunOperationWithTrigger(new OperationTriggerArguments("UserChanged", user.ID, user));

                tabSheetTabs.IsModified(); //Call to set the Dirty status of the tabs so they can save properly
            }

            tabSheetTabs.GetData();

            HeaderText = Description;

            return true;
        }
       
        protected override void OnSetupContextBarHeaders(ContextBarHeaderConstructionArguments arguments)
        {
            arguments.Add(new ContextBarHeader(Resources.Authentication, this.GetType().ToString() + ".Authentication"), 5);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".View")
            {

                // We can delete the user if it is not my self and if we have permission
                if (PluginEntry.DataModel.HasPermission(Permission.SecurityDeleteUser) &&
                    userID != (Guid)PluginEntry.DataModel.CurrentUser.ID && user.Login != "admin")
                {
                    arguments.Add(new ContextBarItem(
                                Properties.Resources.Delete,
                                new ContextbarClickEventHandler(DeleteUserHandler_Handler)), 400);
                }
            }
            else if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageGroupPermissions))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.GroupPermissions, new ContextbarClickEventHandler(GroupPermissions_Handler)), 400);
                }

                arguments.Add(new ContextBarItem(Properties.Resources.ViewAllUsers, new ContextbarClickEventHandler((ContextbarClickEventHandler)PluginOperations.ShowUsersListView)), 500);
            }
            else if (arguments.CategoryKey == base.GetType().ToString() + ".Authentication")
            {

                if ((PluginOperations.CanChangePassword((Guid)userID) && !user.IsDomainUser) || PluginEntry.DataModel.HasPermission(Permission.SecurityGrantHigherPermissions))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.ChangePassword, new ContextbarClickEventHandler(ChangePassword_Handler)), 10);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageAuthenticationTokens))
                {
                    arguments.Add(new ContextBarItem(Resources.AuthenticationTokens, new ContextbarClickEventHandler(AuthenticationTokens)), 20);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.SecurityEnableDisableUser) &&
                    userID != (Guid)PluginEntry.DataModel.CurrentUser.ID)
                {
                    if (user.Login != "admin")
                    {

                        if (user.Disabled)
                        {
                            arguments.Add(new ContextBarItem(
                                Properties.Resources.EnableLogin,
                                new ContextbarClickEventHandler(EnableDisableUser_Handler)), 200);
                        }
                        else
                        {
                            arguments.Add(new ContextBarItem(
                                Properties.Resources.DisableLogin,
                                new ContextbarClickEventHandler(EnableDisableUser_Handler)), 200);
                        }
                    }
                }
            }
        }

        private void AuthenticationTokens(object sender, ContextBarClickEventArguments args)
        {
            AuthenticationTokensDialog dlg = new AuthenticationTokensDialog(userID);

            dlg.ShowDialog(PluginEntry.Framework.MainWindow);
        }

       

        private void DeleteUserHandler_Handler(object sender, ContextBarClickEventArguments args)
        {
            bool userDeleted = PluginOperations.DeleteUser((Guid)userID);
            if (userDeleted)
            {
                PluginEntry.Framework.ViewController.DiscardCurrentView(this);
            }
        }

        private void EnableDisableUser_Handler(object sender, ContextBarClickEventArguments args)
        {
            PluginOperations.SetUserEnabled((Guid)userID, user.Disabled);
            user.Disabled = !user.Disabled;
            PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            chkUserIsDisabled.Checked = user.Disabled; 
         
        }

        private void GroupPermissions_Handler(object sender, ContextBarClickEventArguments args)
        {
            PluginOperations.ShowGroupPermissions();
        }

        private void ChangePassword_Handler(object sender, ContextBarClickEventArguments args)
        {
            // Here I could have used the userID instead of args.ContextIdentifier, but I do it like
            // this to demonstrate how things would work if this was another plugin adding a command to the sheet.
            PluginOperations.ChangePassword((Guid)args.ContextID); 
        }

        private void User_SizeChanged(object sender, EventArgs e)
        {
            

            
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }

    }
}
