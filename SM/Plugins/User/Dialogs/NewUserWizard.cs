using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.UserManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.User.Dialogs.WizardPages;
using LSOne.ViewPlugins.User.Properties;

namespace LSOne.ViewPlugins.User.Dialogs
{
    public partial class NewUserWizard : WizardBase
    {
        private (Guid UserID, RecordIdentifier StaffID) userIDAndStaffID;

        public NewUserWizard(IConnectionManager connection) : base(connection)
        {
            InitializeComponent();

            if (connection.Settings.GetSetting(connection, Settings.ActiveDirectoryEnabled).BoolValue)
            {
                AddPage(new NewUserUserTypePage(this));
            }
            else
            {
                // Active directory is not enabled so we dont display the first page at all
                AddPage(new NewUserIdentityPage(this));
            }

            CreateAnotherCheckboxVisible = true;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnFinish(List<IWizardPage> pages, ref bool cancelAction)
        {
            int pageBase = 1;
            NewUserIdentityPage userIdentity;
            NewUserAssignToGroupsPage userGroups;
            NewUserADIdentityPage userADIdentity;
            string login;
            Name name;
            string email = "";

            if (pages[1] is NewUserADIdentityPage)
            {
                userADIdentity = (NewUserADIdentityPage)pages[pageBase];

                login = userADIdentity.LoginName;
                name = userADIdentity.NameRecord;
                email = userADIdentity.Email;
            }
            else
            {
                if (pages[0] is NewUserIdentityPage)
                {
                    pageBase = 0;
                }

                userIdentity = (NewUserIdentityPage)pages[pageBase];

                login = userIdentity.LoginName;
                name = userIdentity.NameRecord;
                email = userIdentity.Email;
            }

            userGroups = (NewUserAssignToGroupsPage)pages[pageBase + 1];

            // Create new POS user
            var userProvider = Providers.UserData;

            if (pages[1] is NewUserADIdentityPage)
            {
                userADIdentity = (NewUserADIdentityPage)pages[pageBase];

                userIDAndStaffID = userProvider.New(PluginEntry.DataModel,
                    userADIdentity.LoginName,
                    "",
                    userADIdentity.NameRecord,
                    userGroups.UserGroupIDs,
                    true,
                    false,
                    "");
            }
            else
            {
                if (pages[0] is NewUserIdentityPage)
                {
                    pageBase = 0;
                }

                userIdentity = (NewUserIdentityPage)pages[pageBase];

                userIDAndStaffID = userProvider.New(PluginEntry.DataModel,
                    userIdentity.LoginName,
                    userIdentity.Password,
                    userIdentity.NameRecord,
                    userGroups.UserGroupIDs,
                    false,
                    false,
                    email);

            }

            userProvider.CreateStaffMemberForPOS(PluginEntry.DataModel, userIDAndStaffID.StaffID, name, userGroups.UserProfileID);

            if (((NewUserAssignToGroupsPage) pages[pageBase + 1]).CopyExistingUserId != RecordIdentifier.Empty)
            {
                List<PermissionsAssignmentResult> items;

                items = Providers.PermissionData.GetPermissionsForUser(PluginEntry.DataModel, (Guid)((NewUserAssignToGroupsPage)pages[pageBase + 1]).CopyExistingUserId, "");
                Guid context = Guid.NewGuid();
                PluginEntry.DataModel.BeginPermissionOverride(context, new HashSet<string>(){Permission.SecurityGrantPermissions});

                bool perfectPermissionClone = true;

                foreach (var item in items)
                {
                    if (PluginEntry.DataModel.HasPermission(item.PermissionCode) || PluginEntry.DataModel.HasPermission(Permission.SecurityGrantHigherPermissions))
                    {
                        if (item.HasPermission == PermissionState.ExclusiveGrant)
                        {
                            Providers.UserData.SetPermission(PluginEntry.DataModel, userIDAndStaffID.UserID, item.PermissionGuid, UserGrantMode.Grant);
                        }
                        else if (item.HasPermission == PermissionState.ExclusiveDeny)
                        {
                            Providers.UserData.SetPermission(PluginEntry.DataModel, userIDAndStaffID.UserID, item.PermissionGuid, UserGrantMode.Deny);
                        }
                    }
                    else
                    {
                        if (item.HasPermission == PermissionState.ExclusiveGrant || item.HasPermission == PermissionState.ExclusiveDeny)
                        {
                            perfectPermissionClone = false;
                        }
                    }
                }
                PluginEntry.DataModel.EndPermissionOverride(context);
                PluginEntry.Framework.RunOperationWithTrigger(new OperationTriggerArguments("CopyUser", userIDAndStaffID.UserID, ((NewUserAssignToGroupsPage)pages[pageBase + 1]).CopyExistingUserId));

                if (perfectPermissionClone == false || ((NewUserAssignToGroupsPage)pages[pageBase + 1]).GroupsPerfectClone == false)
                {
                    MessageDialog.Show(Resources.SecurityRolesWarning, Resources.WarningTitle, MessageBoxIcon.Warning);
                }
            }
        }

        public Guid UserID
        {
            get
            {
                return userIDAndStaffID.UserID;
            }
            internal set
            {
                userIDAndStaffID.UserID = value;
            }
        }
    }
}
