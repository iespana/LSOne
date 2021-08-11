using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.User.Properties;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.EventArguments;
using UserManagement = LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;

namespace LSOne.ViewPlugins.User.Views
{
    public partial class UsersView : ViewBase
    {
        RecordIdentifier selectedID = "";
        IPlugin profilesPlugin;
        bool loaded;

        private static Guid BarSettingID = new Guid("5CB37C90-9314-4C9B-9860-C7D0457287C6");
        private Setting searchBarSetting;
        private Timer searchTimer;

        public UsersView(RecordIdentifier userId)
            : base()
        {
            selectedID = userId;
        }

        public UsersView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Resources.Users;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.SecurityCreateNewUsers);
            btnsEditAddRemove.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.SecurityDeleteUser);
            btnsEditAddRemove.EditButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.SecurityEditUser);

            lvUsers.ContextMenuStrip = new ContextMenuStrip();
            lvUsers.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            searchBar1.BuddyControl = lvUsers;
            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.SecurityEditUser);

            searchTimer = new Timer();
            searchTimer.Tick += SearchTimerOnTick;
            searchTimer.Interval = 1;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Users", 0, Resources.Users, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.Users;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if(!isRevert)
            {
                profilesPlugin = PluginEntry.Framework.FindImplementor(this, "CanEditUsersUserProfile", null);
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.User.Views.UsersView.View")
            {
                bool enabled = lvUsers.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.SecurityEnableDisableUser);
                arguments.Add(new ContextBarItem(lvUsers.Selection.Count > 0 && ((UserManagement.User)lvUsers.Selection[0].Tag).Disabled 
                ? Resources.EnableLogin : Resources.DisableLogin, btnActivation_Click) { Enabled = enabled }, 100);
            }
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            searchBar1.AddCondition(new ConditionType(Resources.UserName, "UserName", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.Login, "Login", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.UserGroups, "UserGroup", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.NameOnReceipt, "NameOnReceipt", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.Enabled, "Enabled", ConditionType.ConditionTypeEnum.Checkboxes,Resources.Enabled,true));
            searchBar1.AddCondition(new ConditionType(Resources.UserProfile, "UserProfile", ConditionType.ConditionTypeEnum.Unknown));

            searchBar1_LoadDefault(this, EventArgs.Empty);

            searchTimer.Enabled = true;
            searchTimer.Start();
        }

        private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "UserGroup":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("","");
                    ((DualDataComboBox)args.UnknownControl).RequestData += userGroup_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;

                    break;
                case "UserProfile":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox)args.UnknownControl).RequestData += UserProfile_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;

                    break;
            }
        }

        private void searchBar1_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "UserGroup":
                    args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
                case "UserProfile":
                    args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBar1_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "UserGroup":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
                case "UserProfile":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        private void searchBar1_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "UserGroup":
                    entity = Providers.UserGroupData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "UserProfile":
                    entity = Providers.UserProfileData.Get(PluginEntry.DataModel, args.Selection);
                    break;
            }
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void userGroup_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.UserGroupData.AllGroups(PluginEntry.DataModel), null);
        }

        private void UserProfile_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.UserProfileData.GetList(PluginEntry.DataModel), null);
        }

        private void DualDataComboBox_RequestClear(object sender, EventArgs e)
        {
           ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "UserGroup":
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    ((DualDataComboBox)args.UnknownControl).RequestData -= userGroup_RequestData;
                    break;
                case "UserProfile":
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    ((DualDataComboBox)args.UnknownControl).RequestData -= UserProfile_RequestData;
                    break;
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "User":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedID = changeIdentifier;
                    }

                    LoadItems();
                    break;
            }
        }

        private void LoadItems()
        {
            lvUsers.ClearRows();
            List<SearchParameterResult> results = searchBar1.SearchParameterResults;
            UserSearchFilter filter = new UserSearchFilter();
            foreach (var result in results)
            {
                switch (result.ParameterKey)
                {
                    case "UserName":
                        filter.Username = result.StringValue;
                        filter.UsernameBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Login":
                        filter.Login = result.StringValue;
                        filter.LoginBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "UserGroup":
                        filter.UserGroup = ((DualDataComboBox) result.UnknownControl).SelectedData.ID;
                        break;
                    case "UserProfile":
                        filter.UserProfile = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "NameOnReceipt":
                        filter.NameOnReceipt = result.StringValue;
                        filter.NameOnReceiptBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Enabled":
                        filter.EnabledSet = true;
                        filter.Enabled = result.CheckedValues[0];
                        break;
                }
            }
            
            List<UserManagement.User> users;
            IProfileSettings settings = PluginEntry.DataModel.Settings;

            lvUsers.ClearRows();
            // Get all users
            users = Providers.UserData.Search(PluginEntry.DataModel, filter);
            Dictionary<Guid, UserManagement.User> userDictionary = new Dictionary<Guid, UserManagement.User>();

            foreach (var user in users)
            {
                if (userDictionary.ContainsKey(user.Guid))
                {
                    userDictionary[user.Guid].GroupName += "," + user.GroupName;
                }
                else
                {
                    userDictionary.Add(user.Guid, user);
                }
            }

            foreach (UserManagement.User user in userDictionary.Values)
            {
                Row row = new Row();

                row.AddText(settings.NameFormatter.Format(user.Name));
                row.AddText(user.Login);
                row.AddText(user.GroupName);
                row.AddText(user.NameOnReceipt);
                row.AddText(user.Email);
                row.AddText(user.UserProfileName);
                row.AddText(user.Disabled ? Resources.Disabled : Resources.Enabled);

                row.Tag = user;
                if (user.Disabled)
                {
                    row.AddText( Resources.Disabled);
                    row.BackColor = ColorPalette.RedLight;
                }
                else
                {
                    row.AddText(Resources.Enabled);
                }

                lvUsers.AddRow(row);
                if (selectedID == ((UserManagement.User)row.Tag).StaffID)
                {
                    lvUsers.Selection.Set(lvUsers.RowCount - 1);
                }
            }
            lvUsers.AutoSizeColumns();
            lvUsers_SelectedIndexChanged(this, EventArgs.Empty);
            loaded = true;
        }

        private void lvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.SecurityCreateNewUsers);

            btnsEditAddRemove.EditButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.SecurityEditUser) && lvUsers.Selection.Count == 1;
            // We must have permission and the user cannot be the admin user
            if (PluginEntry.Framework.IsSiteManagerBasic)
            {
                btnsEditAddRemove.EditButtonEnabled = btnsEditAddRemove.EditButtonEnabled && ((UserManagement.User)lvUsers.Selection[0].Tag).Login != "admin";
            }

            btnsEditAddRemove.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.SecurityDeleteUser) && lvUsers.Selection.Count > 0 && ((UserManagement.User)lvUsers.Selection[0].Tag).Login != "admin";

            if (loaded) // this is to avoid having duplicate context bars caused when loading for the first time
            {
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }
        }

        private void EditUserProfile_Click(object sender, EventArgs e)
        {
            List<UserManagement.User> selectedUsers = new List<UserManagement.User>();

            for(int i = 0; i < lvUsers.Selection.Count; i++)
            {
                selectedUsers.Add((UserManagement.User)lvUsers.Selection[i].Tag);
            }

            profilesPlugin.Message(this, "EditUsersUserProfile", selectedUsers);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewUser();
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowUser(((UserManagement.User)lvUsers.Selection[0].Tag).Guid);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteUser(((UserManagement.User)lvUsers.Selection[0].Tag).Guid);
        }

        private void lvUsers_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvUsers.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Add,
                    200,
                    new EventHandler(btnsEditAddRemove_AddButtonClicked));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                    lvUsers.Selection.Count > 0 && ((UserManagement.User)lvUsers.Selection[0].Tag).Disabled
                    ? Resources.EnableLogin : Resources.DisableLogin,
                    410,
                    new EventHandler(btnActivation_Click));
            
            item.Enabled = lvUsers.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.SecurityEnableDisableUser);

            menu.Items.Add(item);

            item = new ExtendedMenuItem(Resources.EditUserProfile, 420, EditUserProfile_Click);
            item.Enabled = profilesPlugin != null && lvUsers.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.SecurityEditUser);
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("UsersList", lvUsers.ContextMenuStrip, lvUsers);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnActivation_Click(object sender, EventArgs e)
        {
            lvUsers.Focus();
            List<UserManagement.User> usersToToggle = new List<UserManagement.User>();
            for (int i = 0; i < lvUsers.Selection.Count; i++)
            {
                usersToToggle.Add((UserManagement.User) lvUsers.Selection[i].Tag);
            }

            foreach (var user in usersToToggle)
            {
                PluginOperations.SetUserEnabled(user.Guid, user.Disabled );
            }
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            LoadItems();
        }

        private void lvUsers_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            PluginOperations.ShowUser(((UserManagement.User)lvUsers.Selection[0].Tag).Guid);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void searchBar1_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, string.Empty);

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar1.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar1_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar1.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }

            ShowTimedProgress(searchBar1.GetLocalizedSavingText());
        }

        private void SearchTimerOnTick(object sender, EventArgs eventArgs)
        {
            searchTimer.Stop();
            searchTimer.Enabled = false;
            LoadItems();
        }
    }
}