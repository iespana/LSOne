using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.SiteService.Dialogs
{
    public partial class NewTransactionServiceProfileDialog : DialogBase
    {
        RecordIdentifier profileID = "";

        public NewTransactionServiceProfileDialog()
        {
            List<DataEntity> profiles;

            InitializeComponent();

            profiles = Providers.SiteServiceProfileData.GetList(PluginEntry.DataModel,"NAME asc");

            foreach (DataEntity profile in profiles)
            {
                cmbCopyFrom.Items.Add(profile);
            }

            cmbCopyFrom.SelectedIndex = 0;
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
            SiteServiceProfile profile;
            List<ShorthandItem> shorthandItems = new List<ShorthandItem>();

            if (cmbCopyFrom.SelectedIndex != 0)
            {
                profile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, ((DataEntity)cmbCopyFrom.SelectedItem).ID);
                profile.ID = RecordIdentifier.Empty;
                profile.Text = tbDescription.Text;

                shorthandItems = Providers.ShortHandItemData.GetList(PluginEntry.DataModel, ((DataEntity)cmbCopyFrom.SelectedItem).ID);
            }
            else
            {
                profile = new SiteServiceProfile();
                profile.Text = tbDescription.Text;
                
            }

            Providers.SiteServiceProfileData.Save(PluginEntry.DataModel, profile);
            foreach (ShorthandItem item in shorthandItems)
            {
                item.ID = Guid.Empty;
                item.ProfileID = profile.ID;
                Providers.ShortHandItemData.Save(PluginEntry.DataModel, item);
            }

            profileID = (string)profile.ID;

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
