using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Controls.Rows;
using LSOne.ViewCore.EventArguments;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class AggregateProfilesView : ViewBase
    {
        private RecordIdentifier selectedAggregateProfileID;
        private RecordIdentifier selectedAggregateGroupID;

        public AggregateProfilesView(RecordIdentifier aggregateProfileID)
            : this()
        {
            selectedAggregateProfileID = aggregateProfileID;
        }

        public AggregateProfilesView()
        {
            InitializeComponent();

            HeaderText = Properties.Resources.AggregateProfiles;

            Attributes = ViewAttributes.Help |
                         ViewAttributes.Close |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Audit;

            lvAggregateProfiles.ContextMenuStrip = new ContextMenuStrip();
            lvAggregateProfiles.ContextMenuStrip.Opening += lvAggregateProfiles_Opening;

            lvAggregateGroups.ContextMenuStrip = new ContextMenuStrip();
            lvAggregateGroups.ContextMenuStrip.Opening += lvAggregateGroups_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles);
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.AggregateProfiles;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenDisplayAggregateProfile":
                    LoadData(false);
                    break;
                case "KitchenDisplayAggregateGroupRelation": case "KitchenDisplayAggregateGroup":
                    LoadAggregateGroups();
                    break;
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
                {
                    arguments.Add(new ContextBarItem(
                        Properties.Resources.AggregateGroups,
                        null,
                        PluginOperationsHelper.ShowAggregateGroupsView), 100);
                }
            }
        }

        protected override void LoadData(bool isRevert)
        {
            LoadAggregateProfiles();
        }

        private void LoadAggregateProfiles()
        {
            RecordIdentifier oldSelectedID = selectedAggregateProfileID;
            List<AggregateProfile> aggregateProfiles = Providers.KitchenDisplayAggregateProfileData.GetList(PluginEntry.DataModel);

            lvAggregateProfiles.ClearRows();

            foreach (AggregateProfile profile in aggregateProfiles)
            {
                Row row = new Row();

                row.AddText((string)profile.ID);
                row.AddText(profile.Description);
                row.AddText(profile.TimeHorizon.ToString());

                row.Tag = profile.ID;
                lvAggregateProfiles.AddRow(row);

                if (oldSelectedID == profile.ID)
                {
                    lvAggregateProfiles.Selection.Set(lvAggregateProfiles.RowCount - 1);
                }
            }

            lvAggregateProfiles.AutoSizeColumns();
            lvAggregateProfiles_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void LoadAggregateGroups()
        {
            RecordIdentifier oldSelectedID = selectedAggregateGroupID;

            if (lvAggregateProfiles.Selection.Count == 0) return;

            lvAggregateGroups.ClearRows();

            List<AggregateProfileGroup> aggregateGroups = Providers.KitchenDisplayAggregateProfileGroupData.GetList(PluginEntry.DataModel, selectedAggregateProfileID);

            foreach (AggregateProfileGroup group in aggregateGroups)
            {
                Row row = new Row();
                row.AddText((string)group.GroupID);
                row.AddText(group.GroupDescription);

                row.Tag = group.ID;
                lvAggregateGroups.AddRow(row);

                if (oldSelectedID == group.ID)
                {
                    lvAggregateGroups.Selection.Set(lvAggregateGroups.RowCount - 1);
                }
            }

            lvAggregateGroups.AutoSizeColumns();
            lvAggregateGroups_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvAggregateProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool aggregateProfileSelected = lvAggregateProfiles.Selection.Count > 0;

            selectedAggregateProfileID = aggregateProfileSelected ? (RecordIdentifier)(lvAggregateProfiles.Row(lvAggregateProfiles.Selection.FirstSelectedRow).Tag) : "";
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = aggregateProfileSelected && !ReadOnly;

            if (aggregateProfileSelected)
            {
                if (!lblGroupHeader.Visible)
                {
                    lblGroupHeader.Visible
                        = lvAggregateGroups.Visible
                        = btnsContextButtonsGroups.Visible
                        = true;
                }
                LoadAggregateGroups();
            }
            else
            {
                lblGroupHeader.Visible
                    = lvAggregateGroups.Visible
                    = btnsContextButtonsGroups.Visible
                    = false;
            }
        }

        private void lvAggregateGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool aggregateGroupSelected = (lvAggregateGroups.Selection.Count > 0);

            selectedAggregateGroupID = aggregateGroupSelected ? (RecordIdentifier)(lvAggregateGroups.Row(lvAggregateGroups.Selection.FirstSelectedRow).Tag) : "";
            btnsContextButtonsGroups.EditButtonEnabled = btnsContextButtonsGroups.RemoveButtonEnabled = aggregateGroupSelected && !ReadOnly;
        }

        void lvAggregateProfiles_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvAggregateProfiles.ContextMenuStrip;
            ExtendedMenuItem item;
            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnEditAggregateProfile_Click))
                        {
                            Enabled = btnsContextButtons.EditButtonEnabled,
                            Image = ContextButtons.GetEditButtonImage(),
                            Default = true
                        };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAddAggregateProfile_Click))
                       {
                           Enabled = btnsContextButtons.AddButtonEnabled,
                           Image = ContextButtons.GetAddButtonImage()
                       };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemoveAggregateProfile_Click))
                       {
                           Enabled = btnsContextButtons.RemoveButtonEnabled,
                           Image = ContextButtons.GetRemoveButtonImage()
                       };

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        void lvAggregateGroups_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvAggregateGroups.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                Properties.Resources.Edit,
                100,
                new EventHandler(btnEditAggregateGroup_Click))
                    {
                        Enabled = btnsContextButtonsGroups.EditButtonEnabled,
                        Image = ContextButtons.GetEditButtonImage(),
                        Default = true
                    };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAddAggregateGroup_Click))
                       {
                           Enabled = btnsContextButtonsGroups.AddButtonEnabled,
                           Image = ContextButtons.GetAddButtonImage()
                       };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemoveAggregateGroup_Click))
                       {
                           Enabled = btnsContextButtonsGroups.RemoveButtonEnabled,
                           Image = ContextButtons.GetRemoveButtonImage()
                       };

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvAggregateProfiles_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEditAggregateProfile_Click(this, EventArgs.Empty);
            }
        }

        private void lvAggregateGroups_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtonsGroups.EditButtonEnabled)
            {
                btnEditAggregateGroup_Click(this, EventArgs.Empty);
            }
        }

        private void btnEditAggregateProfile_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowAggregateProfileDialog(selectedAggregateProfileID);
        }

        private void btnAddAggregateProfile_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowAggregateProfileDialog();
        }

        private void btnRemoveAggregateProfile_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.DeleteAggregateProfile(selectedAggregateProfileID);
        }

        private void btnEditAggregateGroup_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowAggregateGroupsView(selectedAggregateGroupID.PrimaryID);
        }

        private void btnAddAggregateGroup_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowAggregateGroupRelationDialog(selectedAggregateProfileID);
        }

        private void btnRemoveAggregateGroup_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.DeleteAggregateGroupRelation(selectedAggregateGroupID);
        }
    }
}