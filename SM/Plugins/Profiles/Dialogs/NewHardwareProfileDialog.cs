﻿using System;
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
    public partial class NewHardwareProfileDialog : DialogBase
    {
        RecordIdentifier profileID = "";

        public NewHardwareProfileDialog()
        {
            List<DataEntity> profiles;

            InitializeComponent();

            profiles = Providers.HardwareProfileData.GetList(PluginEntry.DataModel,"NAME asc");

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
            HardwareProfile profile;

            if (cmbCopyFrom.SelectedIndex != 0)
            {
                profile = Providers.HardwareProfileData.Get(PluginEntry.DataModel, ((DataEntity)cmbCopyFrom.SelectedItem).ID);
                profile.ID = RecordIdentifier.Empty;
                profile.Text = tbDescription.Text;
            }
            else
            {
                profile = new HardwareProfile();
                profile.Text = tbDescription.Text;
            }

            Providers.HardwareProfileData.Save(PluginEntry.DataModel, profile);

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
