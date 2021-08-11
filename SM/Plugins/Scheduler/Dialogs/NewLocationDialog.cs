using System;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewCore.Dialogs;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class NewLocationDialog : DialogBase
    {
        public NewLocationDialog()
        {
            InitializeComponent();
        }

        public JscLocation LocationItem { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LocationItem = null;
            tbName.Clear();

            UpdateAccept();
        }

        private void UpdateAccept()
        {
            btnOK.Enabled = tbName.Text.Length > 0;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            UpdateAccept();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Create the new location object
            LocationItem = new JscLocation();
            LocationItem.Text = tbName.Text.Trim();
            LocationItem.LocationKind = LocationKind.General;
            LocationItem.Enabled = true;
            LocationItem.DDNetMode = NetMode.TCP;
            LocationItem.ExDataAreaId = PluginEntry.DataModel.Connection.DataAreaId;
            
            DataProviderFactory.Instance.Get<ILocationData, JscLocation>().Save(PluginEntry.DataModel, LocationItem);

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

    }

}
