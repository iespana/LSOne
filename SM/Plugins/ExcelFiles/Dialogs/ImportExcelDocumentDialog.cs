using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.ExcelFiles.Enums;

namespace LSOne.ViewPlugins.ExcelFiles.Dialogs
{
    public partial class ImportExcelDocumentDialog : DialogBase
    {
        ImportSettings settings;

        public ImportExcelDocumentDialog()
        {
            settings = new ImportSettings();

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            cmbCustomers.SelectedIndex = 1;
            cmbRetailGroups.SelectedIndex = 1;
            cmbRetailItems.SelectedIndex = 1;
            cmbVendors.SelectedIndex = 1;
            cmbRetailDepartments.SelectedIndex = 1;
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
            settings.RetailItemImportStrategy = (MergeModeEnum)cmbRetailItems.SelectedIndex;
            settings.RetailGroupImportStrategy = (MergeModeEnum)cmbRetailGroups.SelectedIndex;
            settings.CustomerImportStrategy = (MergeModeEnum)cmbCustomers.SelectedIndex;
            settings.VendorImportStrategy = (MergeModeEnum)cmbVendors.SelectedIndex;
            settings.RetailDepartmentImportStrategy = (MergeModeEnum)cmbRetailDepartments.SelectedIndex;
            settings.CalculateProfitMargins = chkProfitMargin.Checked;

            if(string.IsNullOrWhiteSpace(txtDimAttrSeparator.Text))
            {
                errorProvider1.SetError(txtDimAttrSeparator, Properties.Resources.SeparatorCannotBeEmpty);
                return;
            }

            settings.DimensionsAttributesSeparator = txtDimAttrSeparator.Text[0];

            DialogResult = DialogResult.OK;
            Close();
        }

        internal ImportSettings Settings
        {
            get { return settings; }
        }

        private void cmbRetailItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProfitMargin.Enabled = (cmbRetailItems.SelectedIndex != 3);
            chkProfitMargin.Enabled = (cmbRetailItems.SelectedIndex != 3);
        }
    }
}
