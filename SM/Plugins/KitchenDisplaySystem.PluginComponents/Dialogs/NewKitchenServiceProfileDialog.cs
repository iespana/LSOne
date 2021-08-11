using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class NewKitchenServiceProfileDialog : DialogBase
    {
        RecordIdentifier profileID;

        public NewKitchenServiceProfileDialog()
        {
            InitializeComponent();
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
            KitchenServiceProfile profile = new KitchenServiceProfile();
            profile.Text = tbDescription.Text;

            Providers.KitchenDisplayTransactionProfileData.Save(PluginEntry.DataModel, profile);

            profileID = profile.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (tbDescription.Text.Length > 0);
        }

        public RecordIdentifier ProfileID
        {
            get { return profileID; }
        }
    }
}
