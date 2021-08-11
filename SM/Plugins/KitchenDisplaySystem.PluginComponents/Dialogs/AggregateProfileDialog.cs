using System;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class AggregateProfileDialog : DialogBase
    {
        private AggregateProfile aggregateProfile;

        public AggregateProfileDialog(RecordIdentifier aggregateProfileID)
            : this()
        {
            aggregateProfile = Providers.KitchenDisplayAggregateProfileData.Get(PluginEntry.DataModel, aggregateProfileID);

            tbID.ReadOnly = true;
            tbID.BackColor = ColorPalette.BackgroundColor;
            tbID.ForeColor = ColorPalette.DisabledTextColor;
            tbID.Text = (string)aggregateProfile.ID;
            tbDescription.Text = aggregateProfile.Description;
            tbTime.Text = aggregateProfile.TimeHorizon.ToString();
        }

        public AggregateProfileDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = 
                tbID.Text.Length > 0 &&
                tbDescription.Text.Length > 0 &&
                tbTime.Text.Length > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (aggregateProfile == null)
            {
                aggregateProfile = new AggregateProfile();
            }

            tbID.Text = tbID.Text.Trim();
            if (!tbID.ReadOnly && Providers.KitchenDisplayAggregateProfileData.Exists(PluginEntry.DataModel, tbID.Text))
            {
                errorProvider.SetError(tbID, Properties.Resources.AggregateProfileIDExists);
                return;
            }

            aggregateProfile.ID = tbID.Text;
            aggregateProfile.Description = tbDescription.Text.Trim();
            aggregateProfile.TimeHorizon = int.Parse(tbTime.Text);

            Providers.KitchenDisplayAggregateProfileData.Save(PluginEntry.DataModel, aggregateProfile);
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "KitchenDisplayAggregateProfile", aggregateProfile.ID, null);

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