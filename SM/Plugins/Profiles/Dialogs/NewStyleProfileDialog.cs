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
    /// <summary>
    /// A dialog to create and/or edit a <see cref="StyleProfile"/>
    /// </summary>
    public partial class NewStyleProfileDialog: DialogBase
    {
        RecordIdentifier profileID = "";

        /// <summary>
        /// Default constructor for the dialog. All variables are intialized
        /// </summary>
        public NewStyleProfileDialog()
        {
            List<DataEntity> profiles;

            InitializeComponent();

            profiles = Providers.StyleProfileData.GetList(PluginEntry.DataModel);

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
            StyleProfile profile;
            List<PosStyleProfileLine> styles = null;

            if (cmbCopyFrom.SelectedIndex != 0)
            {
                profile = Providers.StyleProfileData.Get(PluginEntry.DataModel, ((DataEntity)cmbCopyFrom.SelectedItem).ID);
                profile.ID = RecordIdentifier.Empty;
                profile.Text = tbDescription.Text;
                styles = Providers.PosStyleProfileLineData.Get(PluginEntry.DataModel, ((DataEntity)cmbCopyFrom.SelectedItem).ID);

            }
            else
            {
                profile = new StyleProfile();
                profile.Text = tbDescription.Text;
            }

            Providers.StyleProfileData.Save(PluginEntry.DataModel, profile,styles);

            profileID = profile.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (tbDescription.Text.Length > 0);
        }

        /// <summary>
        /// The unique ID of the <see cref="StyleProfile"/> being created or edited
        /// </summary>
        public RecordIdentifier ProfileID
        {
            get { return profileID; }
        }
    }
}
