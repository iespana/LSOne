using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Replenishment.Dialogs
{
    public partial class EditAreaDialog : DialogBase
    {
        InventoryArea area;

        public EditAreaDialog(InventoryArea area)
        {
            InitializeComponent();
            this.area = area;
            tbDescription.Text = area.Text;
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
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Save()
        {
            try
            {
                area.Text = tbDescription.Text;
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                    .SaveInventoryArea(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), area, true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(tbDescription.Text) && area.Text != tbDescription.Text;
        }
    }
}
