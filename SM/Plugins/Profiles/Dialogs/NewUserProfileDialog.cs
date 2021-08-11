using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    public partial class NewUserProfileDialog : DialogBase
    {
        RecordIdentifier profileID = "";
        UserProfile emptyItem;

        public NewUserProfileDialog()
        {
            InitializeComponent();

            emptyItem = new UserProfile(RecordIdentifier.Empty, Properties.Resources.DoNotCopyUserProfile);
            cmbCopyFrom.SelectedData = emptyItem;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UserProfile profile;

            if (cmbCopyFrom.SelectedData.ID != RecordIdentifier.Empty)
            {
                profile = Providers.UserProfileData.Get(PluginEntry.DataModel, cmbCopyFrom.SelectedDataID);
                profile.ID = RecordIdentifier.Empty;
                profile.Text = tbDescription.Text;
            }
            else
            {
                profile = new UserProfile();
                profile.Text = tbDescription.Text;
            }

            Providers.UserProfileData.Save(PluginEntry.DataModel, profile);

            profileID = profile.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = tbDescription.Text.Length > 0;
        }

        public RecordIdentifier ProfileID
        {
            get { return profileID; }
        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            List<UserProfile> list = Providers.UserProfileData.GetList(PluginEntry.DataModel);

            list.Insert(0, emptyItem);

            cmbCopyFrom.SetData(list, null);
        }
    }
}
