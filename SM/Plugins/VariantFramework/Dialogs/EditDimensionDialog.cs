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
    public partial class EditDimensionDialog : DialogBase
    {
        private RetailItemDimension dimension;
        private List<RetailItemDimension> retailItemDimensions;

        public EditDimensionDialog(RetailItemDimension dimension, List<RetailItemDimension> retailItemDimensions)
        {
            InitializeComponent();
            this.dimension = dimension;
            this.retailItemDimensions = retailItemDimensions;
            tbDescription.Text = dimension.Text;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tbDescription.Select();
            tbDescription.Focus();
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
            dimension.Text = tbDescription.Text;
            DialogResult = DialogResult.OK;
            Close();
        }


        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            bool dimensionValid = DimensionValid();
            btnOK.Enabled = tbDescription.Text != "" && dimensionValid;
        }

        private bool DimensionValid()
        {
            if (retailItemDimensions != null)
            {
                foreach (var retailItemDimension in retailItemDimensions)
                {
                    if (retailItemDimension != dimension)
                    {
                        if (string.Equals(tbDescription.Text, retailItemDimension.Text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            errorProvider1.SetError(tbDescription, Resources.DimensionDescriptionNotValid);
                            return false;
                        }
                    }
                }
            }
            errorProvider1.Clear();
            return true;
        }
    }
}
