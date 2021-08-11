using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
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
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.User.ViewPages
{
    public partial class UserGroupsPage : UserControl, ITabView
    {
        RecordIdentifier userID;
        bool lockEvents;
        WeakReference owner;

        public UserGroupsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);

            lockEvents = false;

            if (((ViewBase)owner.Parent.Parent).ReadOnly)
            {
                lvUserGroups.Enabled = false;
            }

            lvUserGroups.ContextMenuStrip = new ContextMenuStrip();
            lvUserGroups.ContextMenuStrip.Opening += lvSecurityGroups_ContextMenuStripOpening;
        }

        public UserGroupsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new UserGroupsPage((TabControl)sender);
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("UsersInGroup", userID.PrimaryID, Properties.Resources.UsersInGroup));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            userID = context;

            FillUserGroups();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            // We want to listen to UserGroup broadcasts so we will refresh the UserGroup list
            switch (objectName)
            {
                case "UserGroup":
                    FillUserGroups();
                    break;
            }
        }

        public bool SaveData()
        {
            return true;
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void lvUserGroups_CellAction(object sender, CellEventArgs args)
        {
            if (lockEvents)
            {
                return;
            }
            
            if (((CheckBoxCell)args.Cell).Checked != ((UserInGroupResult)lvUserGroups.Rows.ElementAt(args.RowNumber).Tag).IsInGroup)
            {
                if (((CheckBoxCell)args.Cell).Checked)
                {
                    // Assign the user to the group
                    Providers.UserGroupData.AddUser(PluginEntry.DataModel, (Guid)userID, ((UserInGroupResult)lvUserGroups.Rows.ElementAt(args.RowNumber).Tag).GroupGuid);
                    ((UserInGroupResult)lvUserGroups.Rows.ElementAt(args.RowNumber).Tag).IsInGroup = true;
                }
                else
                {
                    UserInGroupResult usrInGroup = (UserInGroupResult) lvUserGroups.Rows.ElementAt(args.RowNumber).Tag;

                    List<UserGroup> userGroups = Providers.UserGroupData.AllGroups(PluginEntry.DataModel);
                    foreach (UserGroup group in userGroups.Where(w => w.ID == usrInGroup.GroupGuid))
                    {
                        List<DataLayer.BusinessObjects.UserManagement.User> users = Providers.UserData.GetUsersInGroup(PluginEntry.DataModel, (Guid) group.ID);
                        if (group.IsAdminGroup && users.Count <= 1)
                        {
                            PluginOperations.WarningAdmins((Guid) userID, group.ID, (UserInGroupResult) lvUserGroups.Rows.ElementAt(args.RowNumber).Tag, args, group.Text);
                        }
                        else 
                        {
                            //Delete the user from the group
                            Providers.UserGroupData.RemoveUser(PluginEntry.DataModel, (Guid) userID, ((UserInGroupResult) lvUserGroups.Rows.ElementAt(args.RowNumber).Tag).GroupGuid);
                            ((UserInGroupResult) lvUserGroups.Rows.ElementAt(args.RowNumber).Tag).IsInGroup = false;
                        }
                    }
                }
            }

            // Send change information to other sheets in case if any of them will care
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.None, "UserGroupAssignment", null, null);

            //Let the Permission view know of the changes since changing group may very well change permissions
            if (owner.IsAlive)
            {
                ((TabControl)owner.Target).BroadcastChangeInformation(DataEntityChangeType.TabMessage, "UserGroupAssignment", RecordIdentifier.Empty, null);
            }
        }

        private void FillUserGroups()
        {
            List<UserInGroupResult> groups;
            List<UserInGroupResult> myGroups = null;

            lockEvents = true;

            if (!PluginEntry.DataModel.HasPermission(Permission.SecurityAssignUserToGroup))
            {
                lvUserGroups.Enabled = false;
            }
            else if (!PluginEntry.DataModel.HasPermission(Permission.SecurityGrantHigherPermissions))
            {
                // We have to make sure that the user cannot grant to groups that the user is not in him self if he is not a administrator.
                myGroups = Providers.UserGroupData.GetUserGroupAssignments(PluginEntry.DataModel, (Guid)PluginEntry.DataModel.CurrentUser.ID);

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

            DataLayer.BusinessObjects.UserManagement.User user = Providers.UserData.Get(PluginEntry.DataModel, (Guid)userID);
            groups = Providers.UserGroupData.GetUserGroupAssignments(PluginEntry.DataModel, (Guid)userID);
            // Very important, always start with clear since Initialize can be called again if the user hits revert.
            lvUserGroups.ClearRows();
            bool enabled;

            foreach (UserInGroupResult group in groups)
            {
                var row = new Row();

                enabled = !(group.IsAdminGroup && user.IsServerUser);

                row.Tag = group;
                var cell = new CheckBoxCell(group.GroupName, PluginEntry.Framework.GetImageList().Images[PluginEntry.UserGroupImageIndex], group.IsInGroup);
                ((CheckBoxCell) cell).Enabled = enabled;
                row.AddCell(cell);
                
                lvUserGroups.AddRow(row);
            }

            lvUserGroups.ApplyRelativeColumnSize();
            if (myGroups != null)
            {
                // We did not have permission tp grant above our own rights so we need to limit what
                // can be granted.

                if (user.Guid != (Guid) PluginEntry.DataModel.CurrentUser.ID)
                {
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
                                    ((CheckBoxCell)lvUserGroups.Row(count)[0]).Enabled = false;
                                    break;
                                }
                            }
                        }
                        count++;
                    }
                }
                else
                {
                    foreach (var row in lvUserGroups.Rows)
                    {
                        ((CheckBoxCell) row[0]).Enabled = false;
                    }
                }
            }
            else
            {
                int count = 0;
                foreach (var group in groups)
                {
                    if (group.IsAdminGroup)
                    {
                        ((CheckBoxCell)lvUserGroups.Row(count)[0]).Enabled = false;
                    }
                    count++;

                }
            }
            lockEvents = false;
        }

        private void lvSecurityGroups_GroupPermissions(object sender, EventArgs args)
        {
            UserInGroupResult group;

            if (lvUserGroups.Selection.Count > 0)
            {
                group = (UserInGroupResult)lvUserGroups.Rows.ElementAt(lvUserGroups.Selection.FirstSelectedRow).Tag;

                PluginOperations.ShowGroupPermissions(group.GroupGuid, false);
            }
            else
            {
                PluginOperations.ShowGroupPermissions();
            }
        }

        private void lvSecurityGroups_ToggleGroup(object sender, EventArgs args)
        {
            int rowNumber = lvUserGroups.Selection.FirstSelectedRow;
            CheckBoxCell cell = (CheckBoxCell)lvUserGroups.Rows.ElementAt(lvUserGroups.Selection.FirstSelectedRow)[0];

            cell.Checked = !cell.Checked;
            lvUserGroups.Invalidate();

            lvUserGroups_CellAction(lvUserGroups, new CellEventArgs(0, rowNumber, cell));
        }

        void lvSecurityGroups_ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            lvUserGroups.ContextMenuStrip.Items.Clear();

            if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageGroupPermissions))
            {
                lvUserGroups.ContextMenuStrip.Items.Add(new ExtendedMenuItem(
                    Properties.Resources.GroupPermissions,
                    100,
                    new EventHandler(lvSecurityGroups_GroupPermissions)));
            }

            PluginEntry.Framework.ContextMenuNotify("UserGroupAssignment", lvUserGroups.ContextMenuStrip, lvUserGroups);

            e.Cancel = lvUserGroups.ContextMenuStrip.Items.Count == 0;
        }
    }
}
