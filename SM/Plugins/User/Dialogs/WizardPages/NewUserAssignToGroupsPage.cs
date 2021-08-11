using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.BusinessObjects.UserManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.User.Properties;
using LSOne.DataLayer.BusinessObjects.Profiles;

namespace LSOne.ViewPlugins.User.Dialogs.WizardPages
{
    internal partial class NewUserAssignToGroupsPage : UserControl, IWizardPage
    {
        private WizardBase parent;
        bool groupsPerfectClone;
        List<DataLayer.BusinessObjects.UserManagement.User> users;
        IPlugin profilesPlugin;

        public NewUserAssignToGroupsPage()
            : base()
        {
            InitializeComponent();
        }

        public NewUserAssignToGroupsPage(WizardBase parent)
            : base()
        {
            List<UserGroup> userGroups;
            List<UserInGroupResult> myGroups = null;

            
            this.parent = parent;

            InitializeComponent();

            cmbCopyUser.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            cmbCopyUser.SelectedData.ID.SecondaryID = RecordIdentifier.Empty;

            imageList1.Images.Add(Properties.Resources.user_group_16);

            lvSecurityGroups.ContextMenuStrip = new ContextMenuStrip();
            lvSecurityGroups.ContextMenuStrip.Opening += new CancelEventHandler(lvSecurityGroups_ContextMenuStripOpening);

            userGroups = Providers.UserGroupData.AllGroups(PluginEntry.DataModel);


            if (!PluginEntry.DataModel.HasPermission(Permission.SecurityGrantHigherPermissions))
            {
                // We have to make sure that the user cannot grant to groups that the user is not in him self if he is not a administrator.

                myGroups = Providers.UserGroupData.GetUserGroupAssignments(PluginEntry.DataModel, (Guid) PluginEntry.DataModel.CurrentUser.ID);

                /* Check if one of our group is possibly a admin group, if it is then we can grant above 
                 * us regardless of what Permission.SecurityGrantHigherPermissions said*/

                foreach (UserInGroupResult group in myGroups)
                {
                    if (group.IsAdminGroup && group.IsInGroup)
                    {
                        myGroups = null;
                        break;
                    }
                }
            }

            foreach (UserGroup group in userGroups)
            {
                var row = new Row();

                var cell = new CheckBoxCell(group.Name, PluginEntry.Framework.GetImageList().Images[PluginEntry.UserGroupImageIndex], false);
                row.AddCell(cell);
                row.Tag = group;

                lvSecurityGroups.AddRow(row);
            }

            lvSecurityGroups.Columns[0].Width = (short)lvSecurityGroups.Width;
            lvSecurityGroups.Invalidate();

            if (myGroups != null)
            {
                // We did not have permission tp grant above our own rights so we need to limit what
                // can be granted.

                int count = 0;

                foreach (UserInGroupResult group in myGroups)
                {
                    if (!group.IsInGroup)
                    {
                        List<PermissionsAssignmentResult> permissions = Providers.PermissionData.GetPermissionsForGroup(PluginEntry.DataModel, group.GroupGuid, "");

                        List<PermissionsAssignmentResult> permissionsGrantedInGroup = permissions.Where(permission => permission.HasPermission == PermissionState.ExclusiveGrant).ToList();

                        foreach (var permission in permissionsGrantedInGroup)
                        {
                            if (!PluginEntry.DataModel.HasPermission(permission.PermissionCode))
                            {
                                ((CheckBoxCell)lvSecurityGroups.Row(count)[0]).Enabled = false;
                                break;
                            }
                        }
                    }

                    count++;
                }
            }

            cmbUserProfile.SelectedData = new DataEntity("", "");
            profilesPlugin = PluginEntry.Framework.FindImplementor(this, "CanCreateUserProfile", null);
            btnAddUserProfile.Visible = profilesPlugin != null;
        }

        private void CheckState()
        {
            bool hasGroup = false;

            foreach (Row row in lvSecurityGroups.Rows)
            {
                if (((CheckBoxCell)row[0]).Checked)
                {
                    hasGroup = true;
                    break;
                }
            }

            parent.NextEnabled = hasGroup && cmbUserProfile.SelectedData.ID != "" && cmbUserProfile.SelectedData.ID != RecordIdentifier.Empty;
        }

        public List<Guid> UserGroupIDs
        {
            get
            {
                var list = new List<Guid>();

                foreach (var row in lvSecurityGroups.Rows)
                {
                    if (((CheckBoxCell)row[0]).Checked)
                    {
                        list.Add((Guid) ((UserGroup) row.Tag).ID);
                    }
                }

                return list;
            }
        }

        public RecordIdentifier UserProfileID
        {
            get { return cmbUserProfile.SelectedDataID ?? ""; }
        }

        public bool GroupsPerfectClone
        {
            get { return groupsPerfectClone; }
        }

        #region IWizardPage Members

        public bool HasForward
        {
            get
            {
                return false;
            }
        }

        public bool HasFinish
        {
            get
            {
                return true;
            }
        }

        public Control PanelControl
        {
            get
            {
                return this;
            }
        }

        public void Display()
        {
            CheckState();
        }

        public IWizardPage RequestNextPage()
        {
            return null;
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            return true;
        }

        public void ResetControls()
        {
            cmbCopyUser.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            lblSecurityGroups.Enabled = lvSecurityGroups.Enabled = true;
        }

        #endregion

        private void lvSecurityGroups_ToggleGroup(object sender, EventArgs args)
        {
            int selectedRow = lvSecurityGroups.Selection.FirstSelectedRow;
            ((CheckBoxCell)lvSecurityGroups.Row(selectedRow)[0]).Checked = !((CheckBoxCell) lvSecurityGroups.Row(selectedRow)[0]).Checked;
            CheckState();
            lvSecurityGroups.Hide();
            lvSecurityGroups.Show();
            lvSecurityGroups.Select();
        }

        private void lvSecurityGroups_ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            lvSecurityGroups.ContextMenuStrip.Items.Clear();
            int selectedRow = lvSecurityGroups.Selection.FirstSelectedRow;
            if (lvSecurityGroups.RowCount > 0 && !(((UserGroup)lvSecurityGroups.Row(lvSecurityGroups.Selection.FirstSelectedRow).Tag).IsAdminGroup))
            {
                if (((CheckBoxCell)lvSecurityGroups.Row(selectedRow)[0]).Checked)
                {
                    item = new ExtendedMenuItem(Properties.Resources.RemoveFromGroup,
                                                100,
                                                new EventHandler(lvSecurityGroups_ToggleGroup));
                }
                else
                {
                    item = new ExtendedMenuItem(Properties.Resources.AddToGroup,
                                                100,
                                                new EventHandler(lvSecurityGroups_ToggleGroup));
                }
                if (((UserGroup)lvSecurityGroups.Row(selectedRow).Tag).IsAdminGroup)
                {
                    item.Enabled = false;
                }
                lvSecurityGroups.ContextMenuStrip.Items.Add(item);
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void lvSecurityGroups_CellAction(object sender, CellEventArgs args)
        {
            if (args.Cell is CheckBoxCell)
            {
                CheckState();
            }
        }

        private void cmbCopyUser_RequestClear(object sender, EventArgs e)
        {
            cmbCopyUser.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            cmbCopyUser.SelectedData.ID.SecondaryID = RecordIdentifier.Empty;
            cmbCopyUser_SelectedDataChanged(sender, e);
            foreach (var row in lvSecurityGroups.Rows)
            {
                ((CheckBoxCell)row[0]).Checked = false;
            }
            cmbUserProfile.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            parent.NextEnabled = false;
            lvSecurityGroups.Invalidate();
            errorProvider.Clear();
        }

        private void cmbCopyUser_SelectedDataChanged(object sender, EventArgs e)
        {
            lblSecurityGroups.Enabled = lvSecurityGroups.Enabled = cmbCopyUser.SelectedData.ID.SecondaryID == RecordIdentifier.Empty;
            bool found;
            
            if (cmbCopyUser.SelectedData.ID.SecondaryID != RecordIdentifier.Empty)
            {
                DataLayer.BusinessObjects.UserManagement.User selectedUser = users.Single(x => x.ID == cmbCopyUser.SelectedData.ID.SecondaryID);
                List<UserGroup> userGroups = Providers.UserGroupData.GetGroupsForUser(PluginEntry.DataModel, selectedUser.ID);

                POSUser posUser = Providers.POSUserData.Get(PluginEntry.DataModel, selectedUser.StaffID, UsageIntentEnum.Normal);

                if(posUser != null && posUser.UserProfileID.StringValue != "")
                {
                    UserProfile profile = Providers.UserProfileData.Get(PluginEntry.DataModel, posUser.UserProfileID);

                    cmbUserProfile.SelectedData = profile == null 
                                                ? new DataEntity(RecordIdentifier.Empty, "") 
                                                : new DataEntity(profile.ID, profile.Text);
                }

                int groupChecked = 0;
                foreach (var row in lvSecurityGroups.Rows)
                {
                    found = false;
                    foreach (var userGroup in userGroups)
                    {
                        if (userGroup.ID == ((UserGroup) row.Tag).ID)
                        {
                            found = true;
                        }
                    }

                    if (((CheckBoxCell) row[0]).Enabled)
                    {
                        ((CheckBoxCell)row[0]).Checked = found;
                        if (found)
                        {
                            groupChecked++;
                        }
                    }
                    
                }
                parent.NextEnabled = groupChecked > 0 && cmbUserProfile.SelectedData.ID != "" && cmbUserProfile.SelectedData.ID != RecordIdentifier.Empty;
                if (groupChecked == 0)
                {
                    errorProvider.Icon = System.Drawing.Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                    errorProvider.SetError(cmbCopyUser, Resources.CopyUserError);
                }
                else
                {
                    errorProvider.Clear();
                }
                lvSecurityGroups.Invalidate();

                groupsPerfectClone = (userGroups.Count == groupChecked);
            }
        }

        public RecordIdentifier CopyExistingUserId
        {
            get { return cmbCopyUser.SelectedData.ID.SecondaryID; }
        }

        private void cmbCopyUser_RequestData(object sender, EventArgs e)
        {
			List<DataEntity> entities = new List<DataEntity>();
			users = Providers.UserData.AllUsers(PluginEntry.DataModel);
		}

        private void cmbCopyUser_DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new EmployeeSearchPanel(PluginEntry.DataModel, e.DisplayText);
        }

        private void cmbUserProfile_RequestData(object sender, EventArgs e)
        {
            List<UserProfile> userProfiles = Providers.UserProfileData.GetList(PluginEntry.DataModel);
            cmbUserProfile.SetData(userProfiles, null);
        }

        private void cmbUserProfile_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void btnAddUserProfile_Click(object sender, EventArgs e)
        {
            if(profilesPlugin != null)
            {
                RecordIdentifier userProfileID = (RecordIdentifier)profilesPlugin.Message(this, "CreateUserProfile", null);

                if(userProfileID != RecordIdentifier.Empty)
                {
                    UserProfile userProfile = Providers.UserProfileData.Get(PluginEntry.DataModel, userProfileID);
                    cmbUserProfile.SelectedData = new DataEntity(userProfile.ID, userProfile.Text);
                    CheckState();
                }
            }
        }
    }
}
