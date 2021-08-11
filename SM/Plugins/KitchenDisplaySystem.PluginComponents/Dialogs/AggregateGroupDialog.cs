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
    public partial class AggregateGroupDialog : DialogBase
    {
        private AggregateProfileGroup aggregateGroup;

        public AggregateGroupDialog(RecordIdentifier aggregateGroupID)
            : this()
        {
            aggregateGroup = Providers.KitchenDisplayAggregateProfileGroupData.Get(PluginEntry.DataModel, aggregateGroupID);

            tbID.ReadOnly = true;
            tbID.BackColor = ColorPalette.BackgroundColor;
            tbID.ForeColor = ColorPalette.DisabledTextColor;
            tbID.Text = (string)aggregateGroup.GroupID;
            tbDescription.Text = aggregateGroup.GroupDescription;
            chkTime.Checked = aggregateGroup.UseTimeHorizon;
        }

        public AggregateGroupDialog()
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
                tbDescription.Text.Length > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (aggregateGroup == null)
            {
                aggregateGroup = new AggregateProfileGroup();
            }

            tbID.Text = tbID.Text.Trim();
            if (!tbID.ReadOnly && Providers.KitchenDisplayAggregateProfileGroupData.Exists(PluginEntry.DataModel, new RecordIdentifier(tbID.Text)))
            {
                errorProvider.SetError(tbID, Properties.Resources.AggregateGroupIDExists);
                return;
            }

            aggregateGroup.GroupID = tbID.Text;
            aggregateGroup.GroupDescription = tbDescription.Text.Trim();
            aggregateGroup.UseTimeHorizon = chkTime.Checked;

            Providers.KitchenDisplayAggregateProfileGroupData.Save(PluginEntry.DataModel, aggregateGroup);
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "KitchenDisplayAggregateGroup", aggregateGroup.ID, null);

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