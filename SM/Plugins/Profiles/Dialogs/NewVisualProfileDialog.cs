using System;
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
    public partial class NewVisualProfileDialog : DialogBase
    {
        string profileID = "";

        public NewVisualProfileDialog()
        {
            //List<DataEntity> profiles;

            InitializeComponent();

            cmbCopyFrom.SelectedData = new DataEntity("","");

            //profiles = Providers.VisualProfileData.GetList(PluginEntry.DataModel,"NAME asc");

            //foreach (DataEntity profile in profiles)
            //{
            //    cmbCopyFrom.Items.Add(new VisualProfile((string)profile.ID,profile.Text));
            //}

            //cmbCopyFrom.SelectedIndex = 0;
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
            VisualProfile profile;

            if (cmbCopyFrom.SelectedData.ID != "")
            {
                profile = Providers.VisualProfileData.Get(PluginEntry.DataModel, cmbCopyFrom.SelectedData.ID);
                profile.ID = RecordIdentifier.Empty;
                profile.Text = tbDescription.Text;
            }
            else
            {
                profile = new VisualProfile();
                profile.Text = tbDescription.Text;
            }

            Providers.VisualProfileData.Save(PluginEntry.DataModel, profile);

            profileID = (string)profile.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        
        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (tbDescription.Text.Length > 0);
        }

        public string ProfileID
        {
            get { return profileID; }
        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            var visualProfiles = Providers.VisualProfileData.GetList(PluginEntry.DataModel, "NAME asc");
            cmbCopyFrom.SetData(visualProfiles, null);
        }
    }
}
