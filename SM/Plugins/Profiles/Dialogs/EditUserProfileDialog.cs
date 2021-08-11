using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    public partial class EditUserProfileDialog : DialogBase
    {
        List<UserProfile> userProfiles;
        List<User> users;
        EditUserProfileBehaviourEnum behaviour;
        RecordIdentifier initialEntity;

        public EditUserProfileDialog(EditUserProfileBehaviourEnum behaviour, List<UserProfile> userProfiles)
        {
            InitializeComponent();

            this.behaviour = behaviour;
            this.userProfiles = userProfiles;
            cmbUserProfileProperty.SelectedData = new DataEntity("", "");

            switch (behaviour)
            {
                case EditUserProfileBehaviourEnum.Store:
                    Header = Properties.Resources.SelectStoreHeaderText;
                    lblUserProfileProperty.Text = Properties.Resources.SelectStoreLabel;

                    if(userProfiles.All(x => x.StoreID == userProfiles[0].StoreID))
                    {
                        Store store = Providers.StoreData.Get(PluginEntry.DataModel, userProfiles[0].StoreID);
                        cmbUserProfileProperty.SelectedData = store != null ? new DataEntity(store.ID, store.Text) : new DataEntity("", "");

                        initialEntity = cmbUserProfileProperty.SelectedData.ID;
                        btnOK.Enabled = false;
                    }
                    break;
                case EditUserProfileBehaviourEnum.VisualProfile:
                    Header = Properties.Resources.SelectVisualProfileHeaderText;
                    lblUserProfileProperty.Text = Properties.Resources.SelectVisualProfileLabel;

                    if (userProfiles.All(x => x.VisualProfileID == userProfiles[0].VisualProfileID))
                    {
                        VisualProfile visualProfile = Providers.VisualProfileData.Get(PluginEntry.DataModel, userProfiles[0].VisualProfileID);
                        cmbUserProfileProperty.SelectedData = visualProfile != null ? new DataEntity(visualProfile.ID, visualProfile.Text) : new DataEntity("", "");

                        initialEntity = cmbUserProfileProperty.SelectedData.ID;
                        btnOK.Enabled = false;
                    }
                    break;
                case EditUserProfileBehaviourEnum.Layout:
                    Header = Properties.Resources.SelectLayoutHeaderText;
                    lblUserProfileProperty.Text = Properties.Resources.SelectLayoutLabel;

                    if (userProfiles.All(x => x.LayoutID == userProfiles[0].LayoutID))
                    {
                        TouchLayout touchLayout = Providers.TouchLayoutData.Get(PluginEntry.DataModel, userProfiles[0].LayoutID);
                        cmbUserProfileProperty.SelectedData = touchLayout != null ? new DataEntity(touchLayout.ID, touchLayout.Text) : new DataEntity("", "");

                        initialEntity = cmbUserProfileProperty.SelectedData.ID;
                        btnOK.Enabled = false;
                    }
                    break;
            }
        }

        public EditUserProfileDialog(List<User> users)
        {
            InitializeComponent();

            this.behaviour = EditUserProfileBehaviourEnum.UserProfile;
            this.users = users;
            cmbUserProfileProperty.SelectedData = new DataEntity("", "");

            if (users.All(x => x.UserProfileID == users[0].UserProfileID))
            {
                UserProfile userProfile = Providers.UserProfileData.Get(PluginEntry.DataModel, users[0].UserProfileID);
                cmbUserProfileProperty.SelectedData = userProfile != null ? new DataEntity(userProfile.ID, userProfile.Text) : new DataEntity("", "");

                initialEntity = cmbUserProfileProperty.SelectedData.ID;
                btnOK.Enabled = false;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            switch (behaviour)
            {
                case EditUserProfileBehaviourEnum.Store:
                    foreach(UserProfile profile in userProfiles)
                    {
                        profile.StoreID = cmbUserProfileProperty.SelectedData.ID;
                        Providers.UserProfileData.Save(PluginEntry.DataModel, profile);
                        PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Edit, "UserProfileStore", profile.ID, cmbUserProfileProperty.SelectedData);
                    }
                    break;
                case EditUserProfileBehaviourEnum.VisualProfile:
                    foreach (UserProfile profile in userProfiles)
                    {
                        profile.VisualProfileID = cmbUserProfileProperty.SelectedData.ID;
                        Providers.UserProfileData.Save(PluginEntry.DataModel, profile);
                        PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Edit, "UserProfileVisualProfile", profile.ID, cmbUserProfileProperty.SelectedData);
                    }
                    break;
                case EditUserProfileBehaviourEnum.Layout:
                    foreach (UserProfile profile in userProfiles)
                    {
                        profile.LayoutID = cmbUserProfileProperty.SelectedData.ID;
                        Providers.UserProfileData.Save(PluginEntry.DataModel, profile);
                        PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Edit, "UserProfileLayout", profile.ID, cmbUserProfileProperty.SelectedData);
                    }
                    break;
                case EditUserProfileBehaviourEnum.UserProfile:
                    foreach (User user in users)
                    {
                        POSUser posUser = Providers.POSUserData.Get(PluginEntry.DataModel, user.StaffID, UsageIntentEnum.Normal);
                        posUser.UserProfileID = cmbUserProfileProperty.SelectedData.ID;
                        Providers.POSUserData.Save(PluginEntry.DataModel, posUser);
                        PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Edit, "UserProfileUser", user.ID, cmbUserProfileProperty.SelectedData);
                    }
                    break;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbUserProfileProperty_RequestClear(object sender, EventArgs e)
        {

        }

        private void cmbUserProfileProperty_RequestData(object sender, EventArgs e)
        {
            switch (behaviour)
            {
                case EditUserProfileBehaviourEnum.Store:
                    cmbUserProfileProperty.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
                    break;
                case EditUserProfileBehaviourEnum.VisualProfile:
                    cmbUserProfileProperty.SetData(Providers.VisualProfileData.GetList(PluginEntry.DataModel), null);
                    break;
                case EditUserProfileBehaviourEnum.Layout:
                    cmbUserProfileProperty.SetData(Providers.TouchLayoutData.GetList(PluginEntry.DataModel), null);
                    break;
                case EditUserProfileBehaviourEnum.UserProfile:
                    cmbUserProfileProperty.SetData(Providers.UserProfileData.GetList(PluginEntry.DataModel), null);
                    break;
            }
        }

        private void cmbUserProfileProperty_SelectedDataChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = cmbUserProfileProperty.SelectedData.ID != initialEntity;
        }
    }

    public enum EditUserProfileBehaviourEnum
    {
        Store,
        VisualProfile,
        Layout,
        // Used when editing the user profile on users
        UserProfile
    }
}
