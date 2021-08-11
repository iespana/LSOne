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
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.User.Dialogs;
using LSOne.Controls;

namespace LSOne.ViewPlugins.User.Views
{
    public partial class GroupPermissions : ViewBase
    {
        private Guid selectedGroup;
        private bool wantsNew;

        /// <summary>
        /// Since this sheet is operation driven and therefor uses instant saving then it will not 
        /// hook into the saving mechanishm of the ViewStack
        /// </summary>

        public GroupPermissions(Guid selectedGroup,bool createNew)
        {
            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            wantsNew = createNew;

            this.selectedGroup = selectedGroup;

            InitializeComponent();

            lvUsers.SmallImageList = lvPermissions.SmallImageList = PluginEntry.Framework.GetImageList();

            lvUsers.ContextMenuStrip = new ContextMenuStrip();
            lvUsers.ContextMenuStrip.Opening += lvUsersContextMenuStrip_Opening;

            lvPermissions.ContextMenuStrip = new ContextMenuStrip();
            lvPermissions.ContextMenuStrip.Opening += lvPermissionsContextMenuStrip_Opening;
        }

        public GroupPermissions()
        {
            wantsNew = false;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (wantsNew)
            {
                wantsNew = false;
                timer1.Enabled = true;
            }
        }       

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;  // We return a empty here because this sheet is single instance
            }
        }

        public string Description
        {
            get
            {
                return this.HeaderText;
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("UserGroups", Guid.Empty, Properties.Resources.UserGroups,true));
            contexts.Add(new AuditDescriptor("GroupPermissions", Guid.Empty, Properties.Resources.PermissionAssignment));
        }

        protected override void LoadData(bool isRevert)
        {
            lnkNew.Enabled = PluginEntry.DataModel.HasPermission(Permission.SecurityCreateUserGroups);

            cmbGroup.Items.AddRange(Providers.UserGroupData.AllGroups(PluginEntry.DataModel).ToArray());

            if (cmbGroup.Items.Count > 0)
            {
                if (selectedGroup != Guid.Empty)
                {
                    for (int i = 0; i < cmbGroup.Items.Count; i++ )
                    {
                        if (((UserGroup)cmbGroup.Items[i]).ID == selectedGroup)
                        {
                            cmbGroup.SelectedIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    cmbGroup.SelectedIndex = 0;
                }
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "User":
                    cmbGroup_SelectedIndexChanged(this, EventArgs.Empty);
                    break;

                case "UserGroupAssignment":
                    cmbGroup_SelectedIndexChanged(this, EventArgs.Empty);
                    break;
            }
        }

        void lvPermissionsContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvPermissions.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            if(lvPermissions.SelectedItems.Count > 0)
            {
                item = new ExtendedMenuItem(
                    btnGrant.Text,
                    Properties.Resources.GreenLightSmall,
                    10,
                    new EventHandler(btnGrant_Click));

                item.Enabled = btnGrant.Enabled;

                menu.Items.Add(item);

                item = new ExtendedMenuItem(
                    btnDeny.Text,
                    Properties.Resources.RedLightSmall,
                    20,
                    new EventHandler(btnDeny_Click));

                item.Enabled = btnDeny.Enabled;

                menu.Items.Add(item);
            }
                
            PluginEntry.Framework.ContextMenuNotify("GroupPermissionList", lvPermissions.ContextMenuStrip, lvPermissions);

            e.Cancel = (menu.Items.Count == 0);
        }

        void lvUsersContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            lvUsers.ContextMenuStrip.Items.Clear();

            /* We can optionally add our own items right here but we do not do it
             * because there are other places where we are displaying almost the same menu*/

            PluginEntry.Framework.ContextMenuNotify("UserList", lvUsers.ContextMenuStrip, lvUsers);

            e.Cancel = (lvUsers.ContextMenuStrip.Items.Count == 0);
        }

        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem listItem;
            ListViewGroup lastGroup = new ListViewGroup("");
            UserGroup userGroup;
            bool canGrantHigher;
            List<PermissionsAssignmentResult> items;
            List<LSOne.DataLayer.BusinessObjects.UserManagement.User> users;
            IProfileSettings settings = PluginEntry.DataModel.Settings;
            string searchText = textBox1.Text;

            canGrantHigher = PluginEntry.DataModel.HasPermission(Permission.SecurityGrantHigherPermissions);

            if (cmbGroup.SelectedIndex >= 0)
            {
                lvPermissions.Items.Clear();
                lvPermissions.Groups.Clear();

                userGroup = (UserGroup)cmbGroup.SelectedItem;

                lnkEdit.Enabled   = !userGroup.Locked && PluginEntry.DataModel.HasPermission(Permission.SecurityEditUserGroups);
                lnkDelete.Enabled = !userGroup.Locked && PluginEntry.DataModel.HasPermission(Permission.SecurityDeleteUserGroups);

                items = Providers.UserGroupData.GetPermissions(PluginEntry.DataModel, (Guid)userGroup.ID, searchText);
                users = Providers.UserData.GetUsersInGroup(PluginEntry.DataModel, (Guid)userGroup.ID);


                foreach (PermissionsAssignmentResult item in items)
                {
                    listItem = new ListViewItem(item.Description);
                    listItem.Tag = item;

                    if (lastGroup.Header != item.PermissionGroupName)
                    {
                        lastGroup = new ListViewGroup(item.PermissionGroupName);
                        lvPermissions.Groups.Add(lastGroup);
                    }

                    if (item.HasPermission == PermissionState.ExclusiveGrant)
                    {
                        listItem.ImageIndex = PluginEntry.GreenLightImageIndex;
                        listItem.SubItems.Add(Properties.Resources.Granted);
                    }
                    else
                    {
                        //listItem.ImageIndex = PluginEntry.RedLightImageIndex;
                        listItem.SubItems.Add(Properties.Resources.Denied);
                    }

                    if (userGroup.IsAdminGroup)
                    {
                        // Nothing we can do with a Admin group except to view it
                        listItem.ForeColor = SystemColors.GrayText;
                        listItem.SubItems[0].ForeColor = SystemColors.GrayText;
                    }
                    else if (!canGrantHigher)
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
                }

                lvUsers.Items.Clear();

                foreach (LSOne.DataLayer.BusinessObjects.UserManagement.User user in users)
                {
                    UserListViewItem item = new UserListViewItem(null, user,settings.NameFormatter.Format(user.Name),
                        user.Disabled ? PluginEntry.UserDisabledImageIndex : (user.IsServerUser ? PluginEntry.ServerUserImageIndex : PluginEntry.UserImageIndex));

                    item.ID = user.Guid;

                    lvUsers.Add(item);
                }
            }
        }

        private void lvPermissions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem selectedItem;

            if (lvPermissions.SelectedItems.Count == 0 || 
                !PluginEntry.DataModel.HasPermission(Permission.SecurityGrantPermissions))
            {
                btnDeny.Enabled = false;
                btnGrant.Enabled = false;
            }
            else
            {
                selectedItem = lvPermissions.SelectedItems[0];
                PermissionsAssignmentResult result = (PermissionsAssignmentResult) selectedItem.Tag;
                if (selectedItem.ForeColor == SystemColors.GrayText)
                {
                    btnDeny.Enabled = false;
                    btnGrant.Enabled = false;
                }
                else
                {
                    btnDeny.Enabled = result.HasPermission == PermissionState.ExclusiveGrant;
                    btnGrant.Enabled = result.HasPermission == PermissionState.ExclusiveDeny;
                }
            }
        }

        private void btnGrant_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem current in lvPermissions.SelectedItems)
            {
                PermissionsAssignmentResult target = (PermissionsAssignmentResult)current.Tag;
                Providers.UserGroupData.SetPermission(
                PluginEntry.DataModel,
                (Guid)((UserGroup)cmbGroup.SelectedItem).ID,
                target.PermissionGuid,
                GroupGrantMode.Grant);

                target.HasPermission = PermissionState.ExclusiveGrant;
                current.ImageIndex = PluginEntry.GreenLightImageIndex;
                current.SubItems[1].Text = Properties.Resources.Granted;               
            }
            
            lvPermissions_SelectedIndexChanged(this, EventArgs.Empty);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "UserGroupPermission", null, null);
        }

        private void btnDeny_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem current in lvPermissions.SelectedItems)
            {
                PermissionsAssignmentResult target = (PermissionsAssignmentResult) current.Tag;
                Providers.UserGroupData.SetPermission(
                PluginEntry.DataModel,
                (Guid)((UserGroup)cmbGroup.SelectedItem).ID,
                target.PermissionGuid,
                GroupGrantMode.Deny);

                target.HasPermission = PermissionState.ExclusiveDeny;
                current.ImageIndex = PluginEntry.RedLightImageIndex;
                current.SubItems[1].Text = Properties.Resources.Denied;         
            }

            lvPermissions_SelectedIndexChanged(this, EventArgs.Empty);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "UserGroupPermission", null, null);
        }

        private void lvUsers_DoubleClick(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count > 0)
            {
                PluginOperations.ShowUser((Guid)((UserListViewItem)lvUsers.SelectedItems[0]).ID);
            }
        }

        private void lnkNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewUserGroupDialog dialog = new NewUserGroupDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                cmbGroup.Items.Add(dialog.NewGroup);
                cmbGroup.SelectedIndex = cmbGroup.Items.Count - 1;
            }
        }

        private void lnkEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int index;
            SimpleValueEditor dlg;
            UserGroup userGroup;

            userGroup = (UserGroup)cmbGroup.SelectedItem;

            dlg = new SimpleValueEditor(Properties.Resources.EditUserGroupCaption + "...", userGroup.Name, 32);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                userGroup.Name = dlg.TextValue;

                Providers.UserGroupData.Edit(PluginEntry.DataModel, userGroup);

                /* How silly, just updating the userGroup.Name and invalidating the combobox does not
                 * seem to be enough to get the Combobox item updated */
                index = cmbGroup.SelectedIndex;

                cmbGroup.SuspendLayout();
                cmbGroup.Items.RemoveAt(index);
                cmbGroup.Items.Insert(index, userGroup);
                cmbGroup.SelectedIndex = index;
                cmbGroup.ResumeLayout();

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "UserGroup", null, null);
            }
        }

        private void lnkDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteUserGroupQuestion, 
                Properties.Resources.DeleteUserGroup) == DialogResult.Yes)
            {

                Providers.UserGroupData.Delete(PluginEntry.DataModel, (Guid)((UserGroup)cmbGroup.SelectedItem).ID);

                cmbGroup.Items.RemoveAt(cmbGroup.SelectedIndex);
                cmbGroup.SelectedIndex = 0;

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "UserGroup", null, null);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            lnkNew_LinkClicked(this, null);
        }

        public void CreateNewGroup()
        {
            lnkNew_LinkClicked(this, null);
        }

        private void pnlBottom_SizeChanged(object sender, EventArgs e)
        {
            lvPermissions.Columns[lvPermissions.Columns.Count - 1].Width = lvPermissions.Width - 30 - lvPermissions.Columns[0].Width;

            lvUsers.Columns[lvUsers.Columns.Count - 1].Width = lvUsers.Width - 30;
        }

        protected override void OnClose()
        {
            lvPermissions.SmallImageList = null;
            lvUsers.SmallImageList = null;

            base.OnClose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmbGroup_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            cmbGroup_SelectedIndexChanged(this, EventArgs.Empty);
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                cmbGroup_SelectedIndexChanged(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowUser((Guid)(RecordIdentifier)lvUsers.SelectedItems[0].Tag);
        }

        private void lvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEditUser.Enabled = lvUsers.SelectedItems.Count > 0;
        }
    }
}
