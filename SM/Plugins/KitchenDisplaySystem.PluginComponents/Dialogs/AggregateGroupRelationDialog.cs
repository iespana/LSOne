using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class AggregateGroupRelationDialog : DialogBase
    {
        private AggregateProfileGroup aggregateGroup;
        private RecordIdentifier aggregateProfileID;

        public AggregateGroupRelationDialog(RecordIdentifier aggregateProfileID)
            : this()
        {
            this.aggregateProfileID = aggregateProfileID;
        }

        public AggregateGroupRelationDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = cmbGroup.SelectedData != null;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            aggregateGroup = (AggregateProfileGroup)cmbGroup.SelectedData;
            aggregateGroup.ProfileID = aggregateProfileID;

            if (Providers.KitchenDisplayAggregateProfileGroupData.RelationExists(PluginEntry.DataModel, aggregateGroup.ID))
            {
                errorProvider.SetError(btnAddGroup, Properties.Resources.AggregateGroupRelationExists);
                return;
            }

            Providers.KitchenDisplayAggregateProfileGroupData.CreateGroupRelation(PluginEntry.DataModel, aggregateGroup);
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "KitchenDisplayAggregateGroupRelation", aggregateGroup.ID, null);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbRelation_RequestData(object sender, EventArgs e)
        {
            cmbGroup.SetData(Providers.KitchenDisplayAggregateProfileGroupData.GetList(PluginEntry.DataModel), null);
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowAggregateGroupDialog();
        }
    }
}