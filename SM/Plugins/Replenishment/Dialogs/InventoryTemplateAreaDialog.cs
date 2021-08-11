using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Replenishment.Dialogs
{
    public partial class InventoryTemplateAreaDialog : DialogBase
    {
        InventoryTemplate template;
        InventoryAreaLine areaLine;

        //Add new line
        public InventoryTemplateAreaDialog(InventoryTemplate template)
        {
            InitializeComponent();
            this.template = template;
        }

        //Edit existing line
        public InventoryTemplateAreaDialog(InventoryTemplate template, InventoryAreaLine areaLine)
        {
            InitializeComponent();
            this.areaLine = areaLine;
            this.template = template;
            tbDescription.Text = areaLine.Text;
            chkCreateAnother.Visible = false;
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
            Save();

            if (chkCreateAnother.Visible && chkCreateAnother.Checked)
            {
                tbDescription.Text = "";
                tbDescription.Focus();
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void Save()
        {
            try
            {
                if(areaLine == null)
                {
                    if (template.AreaID == Guid.Empty)
                    {
                        InventoryArea newArea = new InventoryArea();
                        newArea.Text = template.Text + " - " + Properties.Resources.Area;

                        newArea.ID = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .SaveInventoryArea(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), newArea, false);

                        //Save the new area header on the template
                        template.AreaID = (Guid)newArea.ID;
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .SaveInventoryTemplate(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template, false);
                    }

                    InventoryAreaLine newLine = new InventoryAreaLine();
                    newLine.AreaID = template.AreaID;
                    newLine.Text = tbDescription.Text;

                    newLine.ID = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .SaveInventoryAreaLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), newLine, true);
                }
                else
                {
                    areaLine.Text = tbDescription.Text;
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .SaveInventoryAreaLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), areaLine, true);
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Add, "InventoryTemplateArea", template.ID, null);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(tbDescription.Text) && (areaLine == null || areaLine.Text != tbDescription.Text);
        }
    }
}
