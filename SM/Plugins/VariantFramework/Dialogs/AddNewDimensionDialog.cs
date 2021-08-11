using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.VariantFramework.Properties;

namespace LSOne.ViewPlugins.VariantFramework.Dialogs
{
    public partial class AddNewDimensionDialog : DialogBase
    {
        private RetailItemDimension retailItemDimension;
        private List<DimensionTemplate> dimensionTemplates;
        private List<DimensionAttribute> dimensionAttributes;
        private List<RetailItemDimension> dimensions;
        private Style disabledCell;
        private bool isTemplate;

        public AddNewDimensionDialog()
        {
            InitializeComponent();
            dimensionAttributes = new List<DimensionAttribute>();
            retailItemDimension = new RetailItemDimension();
            dimensionTemplates = new List<DimensionTemplate>();
            disabledCell = new Style(lvDimensions.DefaultStyle) {TextColor = SystemColors.GrayText};
            isTemplate = false;
        }

        public AddNewDimensionDialog(List<RetailItemDimension> dimensions)
            :this()
        {
            this.dimensions = dimensions;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tbDescription.Select();
            lvDimensions.AutoSizeColumns();
            lvDimensionAttributes.AutoSizeColumns();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RetailItemDimension RetailItemDimension
        {
            get { return retailItemDimension; }
        }

        public bool IsTemplate
        {
            get { return isTemplate; }
        }

        public List<DimensionAttribute> DimensionAttributes
        {
            get { return dimensionAttributes; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            bool dimensionValid = DimensionValid();
            if (rbCreateNew.Checked)
            {
                rbSelectFromTemplate.Checked = false;
                lvDimensions.Enabled = lvDimensionAttributes.Enabled = rbSelectFromTemplate.Checked;
                lblDescription.Enabled = tbDescription.Enabled = rbCreateNew.Checked;
                tbDescription.Select();
                tbDescription.Focus();
                btnOK.Enabled = tbDescription.Text.Trim() != "" && dimensionValid;
            }
            else
            {
                rbCreateNew.Checked = false;
                lvDimensions.Enabled = lvDimensionAttributes.Enabled = rbSelectFromTemplate.Checked;
                lblDescription.Enabled = tbDescription.Enabled = rbCreateNew.Checked;
                btnOK.Enabled = lvDimensions.Selection.Count > 0 && lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow)[0].GetStyle() != disabledCell;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rbCreateNew.Checked)
            {
                if (tbDescription.Text.Trim() != "")
                {
                    retailItemDimension.Text = tbDescription.Text.Trim();
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            else
            {
                if (lvDimensions.Selection.Count > 0)
                {
                    DimensionTemplate template = (DimensionTemplate)(lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag);
                    isTemplate = true;
                    retailItemDimension.Text = template.Text;

                    DimensionAttribute attribute;
                    foreach (var row in lvDimensionAttributes.Rows)
                    {
                        attribute = (DimensionAttribute)row.Tag;
                        attribute.ID = Guid.NewGuid();
                        attribute.DimensionID = RecordIdentifier.Empty;
                        dimensionAttributes.Add(attribute);
                    }
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            
        }

        private void lvDimensions_Load(object sender, EventArgs e)
        {
            dimensionTemplates = Providers.DimensionTemplateData.GetList(PluginEntry.DataModel);
            dimensionTemplates =  dimensionTemplates.OrderBy(x=> x.Text).ToList();

            foreach (var template in dimensionTemplates)
            {
                Row row = new Row();
                row.AddText(template.Text);
                row.Tag = template;
                foreach (var dimension in dimensions)
                {
                    if (string.Equals(template.Text, dimension.Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        row[0].SetStyle(disabledCell);
                    }
                }
                
                lvDimensions.AddRow(row);
                lvDimensions.AutoSizeColumns();
            }
            if (lvDimensions.RowCount > 0)
            {
                lvDimensions.Selection.Set(0);
                lvDimensions.Invalidate();
            }

        }

        private void lvDimensions_SelectionChanged(object sender, EventArgs e)
        {
            lvDimensionAttributes.ClearRows();

            RecordIdentifier attributeID = ((DimensionTemplate)(lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag)).ID;
                
            List<DimensionAttribute> attributes = Providers.DimensionAttributeData.GetListForDimensionTemplate(PluginEntry.DataModel, attributeID);

            foreach (var attribute in attributes)
            {
                Row row = new Row();
                row.AddText(attribute.Text);
                row.AddText(attribute.Code);
                row.Tag = attribute;
                lvDimensionAttributes.AddRow(row);
                lvDimensionAttributes.AutoSizeColumns();

            }
            CheckEnabled(this, EventArgs.Empty);
        }

        private bool DimensionValid()
        {
            if (dimensions != null)
            {
                foreach (var dimension in dimensions)
                {
                    if (string.Equals(tbDescription.Text.Trim(), dimension.Text.Trim(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        errorProvider1.SetError(tbDescription, Resources.DimensionDescriptionNotValid);
                        return false;
                    }
                }
            }
            errorProvider1.Clear();
            return true;
        }
    }
}
