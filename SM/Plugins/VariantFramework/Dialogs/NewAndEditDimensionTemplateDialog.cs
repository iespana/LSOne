using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.VariantFramework.Properties;

namespace LSOne.ViewPlugins.VariantFramework.Dialogs
{
    public partial class NewAndEditDimensionTemplateDialog : DialogBase
    {
        private DimensionTemplate dimensionTemplate;

        public NewAndEditDimensionTemplateDialog()
        {
            InitializeComponent();
            dimensionTemplate = new DimensionTemplate();
        }

        public NewAndEditDimensionTemplateDialog(DimensionTemplate dimensionTemplate)
            :this()
        {
            this.dimensionTemplate = dimensionTemplate;
            tbDescription.Text = dimensionTemplate.Text;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tbDescription.Select();
            tbDescription.Focus();
        }

        public DimensionTemplate DimensionTemplate
        {
            get { return dimensionTemplate; }
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
            dimensionTemplate.Text = tbDescription.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = tbDescription.Text != "";
        }
    }
}
