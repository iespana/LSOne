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
using AggregateGroupItem = LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem.AggregateGroupItem;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class AggregateGroupsView : ViewBase
    {
        private RecordIdentifier selectedAggregateGroupID;
        private RecordIdentifier selectedAggregateItemID;

        public AggregateGroupsView(RecordIdentifier aggregateGroupID)
            : this()
        {
            selectedAggregateGroupID = aggregateGroupID;
            LoadAggregateGroups();
        }

        public AggregateGroupsView()
        {
            InitializeComponent();

            HeaderText = Properties.Resources.AggregateGroups;

            Attributes = ViewAttributes.Help |
                         ViewAttributes.Close |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Audit;

            lvAggregateGroups.ContextMenuStrip = new ContextMenuStrip();
            lvAggregateGroups.ContextMenuStrip.Opening += lvAggregateGroups_Opening;

            lvAggregateItems.ContextMenuStrip = new ContextMenuStrip();
            lvAggregateItems.ContextMenuStrip.Opening += lvAggregateItems_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles);
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.AggregateGroups;
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
                case "KitchenDisplayAggregateGroup":
                    LoadAggregateGroups();
                    break;
                case "KitchenDisplayAggregateGroupItem":
                    LoadAggregateItems();
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
                        Properties.Resources.AggregateProfiles,
                        null,
                        PluginOperationsHelper.ShowAggregateProfilesView), 100);
                }
            }
        }

        protected override void LoadData(bool isRevert)
        {
            LoadAggregateGroups();
        }

        private void LoadAggregateGroups()
        {
            RecordIdentifier oldSelectedID = selectedAggregateGroupID;
            List<AggregateProfileGroup> aggregateGroups = Providers.KitchenDisplayAggregateProfileGroupData.GetList(PluginEntry.DataModel);

            lvAggregateGroups.ClearRows();

            foreach (AggregateProfileGroup group in aggregateGroups)
            {
                Row row = new Row();

                row.AddText((string)group.GroupID);
                row.AddText(group.GroupDescription);

                CheckBoxCell cell = new CheckBoxCell(group.UseTimeHorizon);
                cell.Enabled = false;
                cell.CheckBoxAlignment = CheckBoxCell.CheckBoxAlignmentEnum.Center;
                row.AddCell(cell);

                row.Tag = group.GroupID;
                lvAggregateGroups.AddRow(row);

                if (oldSelectedID == group.GroupID)
                {
                    lvAggregateGroups.Selection.Set(lvAggregateGroups.RowCount - 1);
                }
            }

            lvAggregateGroups.AutoSizeColumns();
            lvAggregateGroups_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void LoadAggregateItems()
        {
            RecordIdentifier oldSelectedID = selectedAggregateItemID;

            if (lvAggregateGroups.Selection.Count == 0) return;

            lvAggregateItems.ClearRows();

            List<AggregateGroupItem> aggregateItems = Providers.KitchenDisplayAggregateGroupItemData.GetList(PluginEntry.DataModel, selectedAggregateGroupID);

            foreach (AggregateGroupItem item in aggregateItems)
            {
                Row row = new Row();
                row.AddText(item.TypeAsString());
                row.AddText(item.ItemID);
                row.AddText(item.ItemDescription);

                row.Tag = item.ID;
                lvAggregateItems.AddRow(row);

                if (oldSelectedID == item.ID)
                {
                    lvAggregateItems.Selection.Set(lvAggregateItems.RowCount - 1);
                }
            }

            lvAggregateItems.AutoSizeColumns();
            lvAggregateItems_SelectedIndexChanged(this, EventArgs.Empty);
            
            btnsContextButtonsGroups.EditButtonEnabled = aggregateItems.Count > 0;
        }

        private void lvAggregateGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool aggregateGroupSelected = lvAggregateGroups.Selection.Count > 0;

            selectedAggregateGroupID = aggregateGroupSelected ? (RecordIdentifier)(lvAggregateGroups.Row(lvAggregateGroups.Selection.FirstSelectedRow).Tag) : "";
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = aggregateGroupSelected && !ReadOnly;

            if (aggregateGroupSelected)
            {
                if (!lblGroupHeader.Visible)
                {
                    lblGroupHeader.Visible
                        = lvAggregateItems.Visible
                        = btnsContextButtonsGroups.Visible
                        = true;
                }
                LoadAggregateItems();
            }
            else
            {
                lblGroupHeader.Visible
                    = lvAggregateItems.Visible
                    = btnsContextButtonsGroups.Visible
                    = false;
            }
        }

        private void lvAggregateItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool aggregateItemSelected = lvAggregateItems.Selection.Count > 0;

            selectedAggregateItemID = aggregateItemSelected ? (RecordIdentifier)(lvAggregateItems.Row(lvAggregateItems.Selection.FirstSelectedRow).Tag) : "";
            btnsContextButtonsGroups.RemoveButtonEnabled = aggregateItemSelected && !ReadOnly;
        }

        void lvAggregateGroups_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvAggregateGroups.ContextMenuStrip;
            ExtendedMenuItem item;
            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnEditAggregateGroup_Click))
                           {
                               Enabled = btnsContextButtons.EditButtonEnabled,
                               Image = ContextButtons.GetEditButtonImage(),
                               Default = true
                           };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAddAggregateGroup_Click))
                       {
                           Enabled = btnsContextButtons.AddButtonEnabled,
                           Image = ContextButtons.GetAddButtonImage()
                       };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemoveAggregateGroup_Click))
                       {
                           Enabled = btnsContextButtons.RemoveButtonEnabled,
                           Image = ContextButtons.GetRemoveButtonImage()
                       };

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        void lvAggregateItems_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvAggregateItems.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                Properties.Resources.Edit,
                100,
                new EventHandler(btnEditAggregateItem_Click))
                    {
                        Enabled = btnsContextButtonsGroups.EditButtonEnabled,
                        Image = ContextButtons.GetEditButtonImage(),
                        Default = true
                    };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAddAggregateItem_Click))
                       {
                           Enabled = btnsContextButtonsGroups.AddButtonEnabled,
                           Image = ContextButtons.GetAddButtonImage()
                       };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemoveAggregateItem_Click))
                       {
                           Enabled = btnsContextButtonsGroups.RemoveButtonEnabled,
                           Image = ContextButtons.GetRemoveButtonImage()
                       };

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvAggregateGroups_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEditAggregateGroup_Click(this, EventArgs.Empty);
            }
        }

        private void lvAggregateItems_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtonsGroups.EditButtonEnabled)
            {
                btnEditAggregateItem_Click(this, EventArgs.Empty);
            }
        }

        private void btnEditAggregateGroup_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowAggregateGroupDialog(selectedAggregateGroupID);
        }

        private void btnAddAggregateGroup_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowAggregateGroupDialog();
        }

        private void btnRemoveAggregateGroup_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.DeleteAggregateGroup(selectedAggregateGroupID);
        }

        private void btnEditAggregateItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new AggregateGroupItemDialog(selectedAggregateGroupID, true))
            {
                dlg.ShowDialog();
            }
        }

        private void btnAddAggregateItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new AggregateGroupItemDialog(selectedAggregateGroupID, false))
            {
                dlg.ShowDialog();
            }
        }

        private void btnRemoveAggregateItem_Click(object sender, EventArgs e)
        {
            if (lvAggregateItems.Selection.Count == 1)
            {
                PluginOperationsHelper.DeleteAggregateGroupItem((RecordIdentifier)lvAggregateItems.Selection[0].Tag);
            }
            else
            {
                PluginOperationsHelper.DeleteAggregateGroupItems(lvAggregateItems.Selection.GetSelectedTags<RecordIdentifier>());
            }
        }
    }
}