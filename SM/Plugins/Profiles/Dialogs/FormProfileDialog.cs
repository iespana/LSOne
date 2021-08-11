using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    public partial class FormProfileDialog: DialogBase
    {
        RecordIdentifier profileID = RecordIdentifier.Empty;

        public FormProfileDialog()
        {
            List<DataEntity> profiles;

            InitializeComponent();

            profiles = Providers.FormProfileData.GetList(PluginEntry.DataModel);

            foreach (DataEntity profile in profiles)
            {
                cmbCopyFrom.Items.Add(profile);
            }

            cmbCopyFrom.SelectedIndex = 0;
        }

        public FormProfileDialog(RecordIdentifier profileID)
            :this()
        {
            cmbCopyFrom.Visible = false;
            lblCopyFrom.Visible = false;
            this.profileID = profileID;

            var profile = Providers.FormProfileData.Get(PluginEntry.DataModel, profileID);

            tbDescription.Text = profile.Text;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            FormProfile profile;

            if (cmbCopyFrom.SelectedIndex != 0)
            {
                profile = Providers.FormProfileData.Get(PluginEntry.DataModel, ((DataEntity)cmbCopyFrom.SelectedItem).ID);
                profile.ID = RecordIdentifier.Empty;
                profile.Text = tbDescription.Text;
                List<FormProfileLine> receipts = Providers.FormProfileLineData.Get(PluginEntry.DataModel, ((DataEntity)cmbCopyFrom.SelectedItem).ID);
                Providers.FormProfileData.Save(PluginEntry.DataModel, profile, receipts);
                profileID = profile.ID;
            }
            else
            {
                profile = profileID != RecordIdentifier.Empty
                              ? Providers.FormProfileData.Get(PluginEntry.DataModel, profileID)
                              : new FormProfile();

                profile.Text = tbDescription.Text;
                Providers.FormProfileData.Save(PluginEntry.DataModel, profile);
                profileID = profile.ID;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (tbDescription.Text.Length > 0);
        }

        public RecordIdentifier ProfileID
        {
            get { return profileID; }
        }
    }
}
