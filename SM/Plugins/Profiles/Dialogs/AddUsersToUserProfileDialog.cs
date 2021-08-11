using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    public partial class AddUsersToUserProfileDialog : DialogBase
    {
        UserProfile userProfile;
        List<ListViewItem> selectionListItems = new List<ListViewItem>();
        List<DataEntity> selectionList = new List<DataEntity>();

        public AddUsersToUserProfileDialog(UserProfile userProfile)
        {
            InitializeComponent();
            this.userProfile = userProfile;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            selectionListItems = cmbUsers.SecondaryData as List<ListViewItem>;

			foreach(ListViewItem lvi in selectionListItems)
			{
                DataEntity user = (DataEntity)lvi.Tag;
                selectionList.Add(user);

                POSUser posUser = Providers.POSUserData.Get(PluginEntry.DataModel, user.ID.SecondaryID, UsageIntentEnum.Normal);
				posUser.UserProfileID = userProfile.ID;
				Providers.POSUserData.Save(PluginEntry.DataModel, posUser);
				PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "", user.ID, new DataEntity(userProfile.ID, userProfile.Text));
			}

			DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbUsers_RequestData(object sender, EventArgs e)
        {

        }

        private void cmbUsers_DropDown(object sender, Controls.DropDownEventArgs e)
        {
            e.ControlToEmbed = new EmployeeSearchSelectionPanel(PluginEntry.DataModel, e.DisplayText, UserIdentifierEnum.StaffID, selectionListItems);
        }

        private void cmbUsers_SelectedDataChanged(object sender, EventArgs e)
        {
            selectionListItems = cmbUsers.SecondaryData as List<ListViewItem>;
			if (selectionList == null)
			{
				cmbUsers.SelectedData = new DataEntity(0, "");
			}
			btnOK.Enabled = selectionListItems != null && selectionListItems.Count > 0;
        }
    }
}
