using System;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class HeaderPaneDialog : DialogBase
    {
        private HeaderPaneProfile headerPane;

        public HeaderPaneDialog(RecordIdentifier headerPaneID)
            : this()
        {
            headerPane = Providers.KitchenDisplayHeaderPaneData.Get(PluginEntry.DataModel, headerPaneID);

            tbDescription.Text = headerPane.Name;
        }

        public HeaderPaneDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = tbDescription.Text.Length > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (headerPane == null)
            {
                headerPane = new HeaderPaneProfile();
            }

            headerPane.Name = tbDescription.Text;

            Providers.KitchenDisplayHeaderPaneData.Save(PluginEntry.DataModel, headerPane);
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "KitchenDisplayHeaderPane", headerPane.ID, null);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}