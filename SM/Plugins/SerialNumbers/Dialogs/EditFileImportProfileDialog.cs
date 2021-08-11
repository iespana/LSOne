using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LSOne.ViewPlugins.SerialNumbers.Dialogs
{
    public partial class EditFileImportProfileDialog : DialogBase
    {
        public RecordIdentifier SelectedProfileId
        {
            get
            {
                return cmbCSVImportProfile.SelectedIndex == -1 ? RecordIdentifier.Empty : (RecordIdentifier)cmbCSVImportProfile.SelectedValue;
            }
        }

        public string SelectedProfileText
        {
            get
            {
                return cmbCSVImportProfile.SelectedIndex == -1 ? "" : cmbCSVImportProfile.Text;
            }
        }

        public EditFileImportProfileDialog(string profileSelectionText)
        {
            InitializeComponent();

            LoadCSVImportProfiles(profileSelectionText);
        }

        private void LoadCSVImportProfiles(string profileSelectionText)
        {
            cmbCSVImportProfile.Items.Clear();

            Dictionary<RecordIdentifier, string> importProfiles = Providers.ImportProfileData.GetSelectListOfNonEmptyProfiles(PluginEntry.DataModel).Where(el => el.ImportType == DataLayer.BusinessObjects.Enums.ImportType.SerialNumbers)
                .ToDictionary(k => k.MasterID, v => v.Description);
            cmbCSVImportProfile.DataSource = new BindingSource(importProfiles, null);
            cmbCSVImportProfile.DisplayMember = "Value";
            cmbCSVImportProfile.ValueMember = "Key";

            if (string.IsNullOrEmpty(profileSelectionText))
            {
                cmbCSVImportProfile.SelectedIndex = -1;
            }
            else
            {
                cmbCSVImportProfile.Text = profileSelectionText;
            }
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
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cmbCSVImportProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (cmbCSVImportProfile.SelectedIndex != -1);
        }
    }
}
