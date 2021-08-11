using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
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
    public partial class UserPermissionPage : UserControl, ITabView
    {
        RecordIdentifier userID;
        LSOne.DataLayer.BusinessObjects.UserManagement.User user;
        Guid selectedPermissionGuid;

        public UserPermissionPage(TabControl owner)
            : this()
        {
            lvPermissions.SmallImageList = PluginEntry.Framework.GetImageList();

            lvPermissions.ContextMenuStrip = new ContextMenuStrip();
            lvPermissions.ContextMenuStrip.Opening += new CancelEventHandler(lvPermissionsContextMenuStrip_Opening);

            selectedPermissionGuid = Guid.Empty;
        }

        public UserPermissionPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new UserPermissionPage((TabControl)sender);
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("UserPermissions", userID.PrimaryID, Properties.Resources.PermissionAssignment));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (!isRevert)
            {
                userID = context;
                user = (LSOne.DataLayer.BusinessObjects.UserManagement.User)internalContext;

                lvPermissions.Columns[1].Width = -2;
            }

            LoadPermissions("");
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (changeHint == DataEntityChangeType.TabMessage && objectName == "UserGroupAssignment")
            {
                LoadPermissions("");
            }
        }

        public bool SaveData()
        {
            return true;
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvPermissions.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion


        private void LoadPermissions(string searchText)
        {
            List<PermissionsAssignmentResult> items;
            ListViewItem listItem;
            ListViewGroup lastGroup = new ListViewGroup("");
            bool canGrantHigher;
            bool isAdminUser; 

            canGrantHigher = PluginEntry.DataModel.HasPermission(Permission.SecurityGrantHigherPermissions);

            items = Providers.PermissionData.GetPermissionsForUser(PluginEntry.DataModel, (Guid)userID, searchText);

            lvPermissions.Items.Clear();
            lvPermissions.Groups.Clear();
            List<UserGroup> groups = Providers.UserGroupData.GetGroupsForUser(PluginEntry.DataModel, user.ID);
            isAdminUser = groups.Find(f => f.IsAdminGroup) != null;

            foreach (PermissionsAssignmentResult item in items)
            {
                listItem = new ListViewItem(item.Description);
                listItem.Tag = item;

                if (lastGroup.Header != item.PermissionGroupName)
                {
                    lastGroup = new ListViewGroup(item.PermissionGroupName);
                    
                    if (isAdminUser && Providers.PermissionGroupData.GetByPermission(PluginEntry.DataModel, item.PermissionGuid).ID == new Guid("0CA8E620-E997-11DA-8AD9-0800200C9A66"))
                    {
                        lastGroup.Tag = true;
                    }
                    lvPermissions.Groups.Add(lastGroup);
                }

                if (item.HasPermission == PermissionState.ExclusiveGrant)
                {
                    listItem.ImageIndex = PluginEntry.GreenLightImageIndex;
                    listItem.SubItems.Add(Properties.Resources.Granted);
                }
                else if (item.HasPermission == PermissionState.InheritFromGroupGrant)
                {
                    listItem.ImageIndex = PluginEntry.GreenLightImageIndex;
                    listItem.SubItems.Add(Properties.Resources.GrantedFromGroups);
                }
                else if (item.HasPermission == PermissionState.InheritFromGroupNoPermission)
                {
                    //listItem.ImageIndex = PluginEntry.RedLightImageIndex;
                    listItem.SubItems.Add(Properties.Resources.DeniedFromGroups);
                }
                else
                {
                    //listItem.ImageIndex = PluginEntry.RedLightImageIndex;
                    listItem.SubItems.Add(Properties.Resources.Denied);
                }

                if (lastGroup.Tag != null)
                {
                    listItem.ForeColor = SystemColors.GrayText;
                    listItem.SubItems[0].ForeColor = SystemColors.GrayText;
                }

                if (!canGrantHigher)
                {
                    /* Since we are not allowed to grant higher than our self then we need to check
                     * if we have this permission.
                    */

                    if (!PluginEntry.DataModel.HasPermission(item.PermissionCode))
                    {
                        listItem.ForeColor = SystemColors.GrayText;
                        listItem.SubItems[0].ForeColor = SystemColors.GrayText;
                    }
                }

                listItem.Group = lastGroup;

                lvPermissions.Add(listItem);

                if (item.PermissionGuid == selectedPermissionGuid)
                {
                    listItem.Selected = true;
                }
            }

            UserPermissionPage_SizeChanged(this, EventArgs.Empty);
        }

        private void lvPermissions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPermissions.SelectedItems.Count == 0 ||
                !PluginEntry.DataModel.HasPermission(Permission.SecurityGrantPermissions))
            {
                btnDeny.Enabled = false;
                btnGrant.Enabled = false;
                btnInherit.Enabled = false;

                selectedPermissionGuid = Guid.Empty;
            }
            else
            {
                selectedPermissionGuid = ((PermissionsAssignmentResult) lvPermissions.SelectedItems[0].Tag).PermissionGuid;

                if (lvPermissions.SelectedItems[0].ForeColor == SystemColors.GrayText && lvPermissions.SelectedItems.Count == 1)
                {
                    btnDeny.Enabled = false;
                    btnGrant.Enabled = false;
                    btnInherit.Enabled = false;
                }
                else
                {
                    bool btnDenyEnabled = false;
                    bool btnGrantEnabled = false;
                    bool btnInheritEnabled = false;
                    foreach (ListViewItem current in lvPermissions.SelectedItems)
                    {
                        if (current.ForeColor == SystemColors.GrayText)
                        {
                            continue;
                        }
                        btnDenyEnabled = btnDenyEnabled || ((PermissionsAssignmentResult) current.Tag).HasPermission != PermissionState.ExclusiveDeny;
                        btnGrantEnabled = btnGrantEnabled || ((PermissionsAssignmentResult)current.Tag).HasPermission != PermissionState.ExclusiveGrant;
                        btnInheritEnabled = btnInheritEnabled || ((PermissionsAssignmentResult) current.Tag).HasPermission == PermissionState.ExclusiveGrant
                                             || ((PermissionsAssignmentResult) current.Tag).HasPermission == PermissionState.ExclusiveDeny;
                    }
                    btnDeny.Enabled = btnDenyEnabled;
                    btnGrant.Enabled = btnGrantEnabled;
                    btnInherit.Enabled = btnInheritEnabled;
                }
            }
        }

        private void btnInherit_Click(object sender, EventArgs e)
        {
            PermissionsAssignmentResult item;
            foreach (ListViewItem current in lvPermissions.SelectedItems)
            {
                if (current.ForeColor == SystemColors.GrayText)
                {
                    continue;
                }
                item = (PermissionsAssignmentResult)current.Tag;
                Providers.UserData.SetPermission(PluginEntry.DataModel, (Guid)userID, item.PermissionGuid, UserGrantMode.Inherit);
            }
            // There is no way we can evaluate locally if inherit gives us any rights so we must
            // refresh.
            //LoadPermissions("");

            List<PermissionsAssignmentResult> items = Providers.PermissionData.
                GetPermissionsForUser(PluginEntry.DataModel, (Guid)userID, "");
            foreach (ListViewItem current in lvPermissions.SelectedItems)
            {
                PermissionsAssignmentResult result = items.Find(f => f.PermissionGuid == ((PermissionsAssignmentResult) current.Tag).PermissionGuid);
                if (result == null)
                {
                    continue;
                }
                if (result.HasPermission == PermissionState.InheritFromGroupGrant)
                {
                    current.ImageIndex = PluginEntry.GreenLightImageIndex;
                    current.SubItems[1].Text = Properties.Resources.GrantedFromGroups;
                }
                else if (result.HasPermission == PermissionState.InheritFromGroupNoPermission)
                {
                    current.ImageIndex = PluginEntry.RedLightImageIndex;
                    current.SubItems[1].Text = Properties.Resources.DeniedFromGroups;
                }
                current.Tag = result;
            }
            lvPermissions_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void btnGrant_Click(object sender, EventArgs e)
        {
            PermissionsAssignmentResult item;
            foreach (ListViewItem current in lvPermissions.SelectedItems)
            {
                if (current.ForeColor == SystemColors.GrayText)
                {
                    continue;
                }
                item = (PermissionsAssignmentResult) current.Tag;
                Providers.UserData.SetPermission(PluginEntry.DataModel, (Guid)userID, item.PermissionGuid, UserGrantMode.Grant);
                item.HasPermission = PermissionState.ExclusiveGrant;

                current.ImageIndex = PluginEntry.GreenLightImageIndex;
                current.SubItems[1].Text = Properties.Resources.Granted;
            }

            lvPermissions_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void btnDeny_Click(object sender, EventArgs e)
        {
            PermissionsAssignmentResult item;
            foreach (ListViewItem current in lvPermissions.SelectedItems)
            {
                if (current.ForeColor == SystemColors.GrayText)
                {
                    continue;
                }
                item = (PermissionsAssignmentResult)current.Tag;
                Providers.UserData.SetPermission(PluginEntry.DataModel, (Guid)userID, item.PermissionGuid, UserGrantMode.Deny);
                item.HasPermission = PermissionState.ExclusiveDeny;

                current.ImageIndex = PluginEntry.RedLightImageIndex;
                current.SubItems[1].Text = Properties.Resources.Denied;
            }

            lvPermissions_SelectedIndexChanged(this, EventArgs.Empty);
        }

        void lvPermissionsContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvPermissions.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            if (lvPermissions.SelectedItems.Count > 0)
            {
                item = new ExtendedMenuItem(
                    btnInherit.Text,
                    10,
                    btnInherit_Click);

                item.Enabled = btnInherit.Enabled;

                menu.Items.Add(item);

                item = new ExtendedMenuItem(
                    btnGrant.Text,
                    Properties.Resources.check_inverted_16,
                    20,
                    btnGrant_Click);

                item.Enabled = btnGrant.Enabled;

                menu.Items.Add(item);

                item = new ExtendedMenuItem(
                    btnDeny.Text,
                    null, //Properties.Resources.notification_inverted_16,
                    30,
                    btnDeny_Click);

                item.Enabled = btnDeny.Enabled;

                menu.Items.Add(item);
            }

            PluginEntry.Framework.ContextMenuNotify("UserPermissionList", lvPermissions.ContextMenuStrip, lvPermissions);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvPermissions_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void UserPermissionPage_SizeChanged(object sender, EventArgs e)
        {
            lvPermissions.Columns[1].Width = lvPermissions.Width - 30 - lvPermissions.Columns[0].Width;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadPermissions(textBox1.Text);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                LoadPermissions(textBox1.Text);
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            LoadPermissions("");
        }
    }
}
