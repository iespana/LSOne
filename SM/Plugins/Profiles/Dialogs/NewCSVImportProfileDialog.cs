using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    public partial class NewCSVImportProfileDialog : DialogBase
    {
        RecordIdentifier profileId = RecordIdentifier.Empty;

        public NewCSVImportProfileDialog()
        {
            InitializeComponent();

            cmbImportType.DisplayMember = "Text";
            cmbImportType.ValueMember = "Value";
            cmbImportType.Items.Add(new { Text = ImportTypeHelper.GetImportTypeString(ImportType.StockCounting), Value = (int)ImportType.StockCounting });
            cmbImportType.Items.Add(new { Text = ImportTypeHelper.GetImportTypeString(ImportType.SerialNumbers), Value = (int)ImportType.SerialNumbers });
            cmbImportType.SelectedIndex = -1;
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
            ImportProfile profile = new ImportProfile
            {
                Description = tbDescription.Text,
                ImportType = (ImportType)cmbImportType.SelectedIndex,
                HasHeaders = chkHasHeaders.Checked
            };
            Providers.ImportProfileData.Save(PluginEntry.DataModel, profile);
            profileId = profile.MasterID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CreateProfileCanExecute()
        {
            btnOK.Enabled = (tbDescription.Text.Length > 0) && cmbImportType.SelectedIndex != -1;
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            CreateProfileCanExecute();
        }

        public RecordIdentifier ProfileID
        {
            get { return profileId; }
        }

        private void cmbImportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateProfileCanExecute();
        }
    }
}
