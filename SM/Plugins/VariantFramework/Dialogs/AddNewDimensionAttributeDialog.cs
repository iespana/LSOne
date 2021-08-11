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
    public partial class AddNewDimensionAttributeDialog : DialogBase
    {
        private DimensionAttribute dimensionAttribute;
        public event EventHandler CreateWithoutClosing;
        private List<DimensionAttribute> attributes;

        public AddNewDimensionAttributeDialog()
        {
            InitializeComponent();
            dimensionAttribute = new DimensionAttribute();
            tbDescription.Focus();
        }

        public AddNewDimensionAttributeDialog(DimensionAttribute attribute, List<DimensionAttribute> attributes)
            :this()
        {
            this.dimensionAttribute = attribute;
            this.attributes = attributes;

            tbDescription.Text = dimensionAttribute.Text;
            tbCode.Text = dimensionAttribute.Code;

            Header = Resources.EditDescriptionAndCode;
            cbCreateAnother.Checked = false;
            cbCreateAnother.CheckState = CheckState.Unchecked;
            cbCreateAnother.Visible = false;
        }
        public AddNewDimensionAttributeDialog(List<DimensionAttribute> attributes)
            :this()
        {
            this.attributes = attributes;
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

        public DimensionAttribute DimensionAttribute
        {
            get { return dimensionAttribute; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            bool descriptionOrCodeValid = DescriptionOrCodeValid();
            btnOK.Enabled = (tbDescription.Text != "") && (tbCode.Text != "") && descriptionOrCodeValid;
        }

        protected virtual void OnCreateWithoutClosing()
        {
            if (CreateWithoutClosing != null)
            {
                CreateWithoutClosing(this, EventArgs.Empty);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((tbDescription.Text != "") && (tbCode.Text != ""))
            {
                dimensionAttribute.Text = tbDescription.Text;
                dimensionAttribute.Code = tbCode.Text;
                if (cbCreateAnother.Checked)
                {
                    OnCreateWithoutClosing();
                    dimensionAttribute = new DimensionAttribute();
                    tbDescription.Text = "";
                    tbCode.Text = "";
                    tbDescription.Select();
                }
                else
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private bool DescriptionOrCodeValid()
        {
            foreach (var attribute in attributes)
            {
                if (attribute != dimensionAttribute)
                {
                    if (string.Equals(tbDescription.Text, attribute.Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        errorProvider1.SetError(tbDescription, Resources.AttributeDescriptionInvalid);
                        return false;
                    }
                    if (string.Equals(tbCode.Text, attribute.Code, StringComparison.CurrentCultureIgnoreCase))
                    {
                        errorProvider1.SetError(tbCode, Resources.AttributeCodeInvalid);
                        return false;
                    }
                }
            }
            errorProvider1.Clear();
            return true;
        }
    }
}
