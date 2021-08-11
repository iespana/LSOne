﻿using System;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class NewVisualProfileDialog : DialogBase
    {
        KitchenDisplayVisualProfile profile;

        public NewVisualProfileDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier Id
        {
            get { return profile.ID; }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = (tbKitchenProfileName.Text.Length > 0); 
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            profile = new KitchenDisplayVisualProfile();

            profile.Text = tbKitchenProfileName.Text;

            Providers.KitchenDisplayVisualProfileData.Save(PluginEntry.DataModel, profile);
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "KitchenDisplayVisualProfile", profile.ID, profile);

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
