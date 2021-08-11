using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Store.Dialogs
{
    public partial class RegionDialog : DialogBase
    {
        public DataLayer.BusinessObjects.StoreManagement.Region StoreRegion { get; private set; }

        public RegionDialog()
        {
            InitializeComponent();
            StoreRegion = new DataLayer.BusinessObjects.StoreManagement.Region();
        }

        public RegionDialog(RecordIdentifier profileID)
        {
            InitializeComponent();
            StoreRegion = Providers.RegionData.Get(PluginEntry.DataModel, profileID);
            tbDescription.Text = StoreRegion.Text;
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
            StoreRegion.Text = tbDescription.Text;
            Providers.RegionData.Save(PluginEntry.DataModel, StoreRegion);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(tbDescription.Text) && (StoreRegion.ID == RecordIdentifier.Empty || tbDescription.Text.Trim() != StoreRegion.Text);
        }
    }
}
