using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.BusinessObjects.UserManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.User.Properties;

namespace LSOne.ViewPlugins.User.Dialogs
{
    public partial class NewUserGroupDialog : DialogBase
    {
        DataEntity emptyItem;
        public UserGroup NewGroup { get; private set; }

        public NewUserGroupDialog() : base()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            emptyItem = new DataEntity(RecordIdentifier.Empty, Resources.DoNotCopyExistingGroup);

            cmbCopyFrom.SelectedData = emptyItem;
        }
        
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> list = new List<DataEntity> {emptyItem};
            list.AddRange(Providers.UserGroupData.AllGroups(PluginEntry.DataModel));

            cmbCopyFrom.SetData(list, null , true);
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = (tbDescription.Text.Length > 0);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UserGroup group = new UserGroup(Guid.NewGuid(), tbDescription.Text);
            Providers.UserGroupData.New(PluginEntry.DataModel, group);

            if (cmbCopyFrom.SelectedData.ID != RecordIdentifier.Empty)
            {
                List<PermissionsAssignmentResult> permissions = Providers.PermissionData.
                    GetPermissionsForGroup(PluginEntry.DataModel, (Guid)cmbCopyFrom.SelectedData.ID, "");
                foreach (PermissionsAssignmentResult current in permissions)
                {
                    Providers.UserGroupData.SetPermission(PluginEntry.DataModel, (Guid)group.ID, current.PermissionGuid, current.HasPermission == PermissionState.ExclusiveGrant ? GroupGrantMode.Grant : GroupGrantMode.Deny);
                }
            }
            NewGroup = group;
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "UserGroup", null, null);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
