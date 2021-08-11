using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Terminals;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Terminals;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Terminals.Properties;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Terminals.Views
{
    public partial class TerminalsGroupView : ViewBase
    {
        private List<TerminalGroup> terminalGroup;
        private List<TerminalGroupConnection> terminalDetailedGroup;
        private RecordIdentifier selectedGroupId;

        public TerminalsGroupView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.ContextBar |
                         ViewAttributes.Close |
                         ViewAttributes.Audit;


            HeaderText = Resources.TerminalGroupsHeaderText;

            lvGroups.ContextMenuStrip = new ContextMenuStrip();
            lvGroups.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            lvDetailedGroups.ContextMenuStrip = new ContextMenuStrip();
            lvDetailedGroups.ContextMenuStrip.Opening += ContextMenuDetailedViewStrip_Opening;

            btnTerminalGroupContextButtons.AddButtonEnabled = false;

            lvDetailedGroups.SetSortColumn(1, false);
            lvDetailedGroups.SortColumn.Tag = TerminalGroupConnection.SortEnum.TerminalId;

            lvGroups.SetSortColumn(0, false);
            lvGroups.SortColumn.Tag = TerminalGroup.SortEnum.ID;

            lvDetailedGroups.Columns[0].Tag = TerminalGroupConnection.SortEnum.TerminalId;
            lvDetailedGroups.Columns[1].Tag = TerminalGroupConnection.SortEnum.TerminalDescription;
            lvDetailedGroups.Columns[2].Tag = TerminalGroupConnection.SortEnum.Location;


            lvGroups.Columns[0].Tag = TerminalGroup.SortEnum.Description;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.TerminalEdit);

            btnTerminalGroupContextButtons.AddButtonEnabled = btnGroupContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.TerminalEdit);

        }


        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("RBOTERMINALGROUP", RecordIdentifier.Empty, Resources.TerminalGroups, false));
            contexts.Add(new AuditDescriptor("TERMINALGROUPCONNECTION", RecordIdentifier.Empty, Resources.TerminalGroupConnection, false));

        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.Terminals;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {

            int selectedRow = -1;
            RecordIdentifier oldSelectedId = selectedGroupId;

            if (lvGroups.SortColumn.Tag == null)
            {
                lvGroups.SortColumn.Tag = TerminalGroup.SortEnum.ID;
            }

            lvGroups.ClearRows();
            lvDetailedGroups.ClearRows();

            terminalGroup = Providers.TerminalGroupData.GetListForTerminalGroup(PluginEntry.DataModel, lvGroups.SortedAscending, (TerminalGroup.SortEnum)lvGroups.SortColumn.Tag);

            foreach (var group in terminalGroup)
            {
                var row = new Row {Tag = @group.ID};

                row.AddText(group.Text);

                if (group.ID == oldSelectedId)
                {
                    selectedRow = lvGroups.RowCount;
                }

                lvGroups.AddRow(row);

                if (selectedRow != -1)
                {
                    lvGroups.Selection.Set(0, selectedRow);
                    lvGroups.ScrollRowIntoView(selectedRow);
                }

            }




        }

        private void LoadDetailedGroups(RecordIdentifier selectedGroupId)
        {

            if (lvDetailedGroups.SortColumn.Tag == null)
            {
                lvDetailedGroups.SortColumn.Tag = TerminalGroupConnection.SortEnum.TerminalId;
            }

            this.selectedGroupId = selectedGroupId;
            lvDetailedGroups.ClearRows();
            terminalDetailedGroup = Providers.TerminalGroupConnectionData.GetTerminalsList(PluginEntry.DataModel, selectedGroupId, lvDetailedGroups.SortedAscending, (TerminalGroupConnection.SortEnum)lvDetailedGroups.SortColumn.Tag);

            foreach (var terminal in terminalDetailedGroup)
            {
                var row = new Row { Tag = new RecordIdentifier(terminal.TerminalId, terminal.StoreId) };

                row.AddText((string)terminal.TerminalId);
                row.AddText(terminal.TerminalDescription);
                row.AddText(terminal.Location);
                lvDetailedGroups.AddRow(row);

            }

        }

        protected override bool DataIsModified()
        {

            return false;

        }

        protected override bool SaveData()
        {


            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "TerminalGroup":
                    LoadData(false);
                    break;

                case "TerminalGroupConnection":
                    LoadDetailedGroups(changeIdentifier);
                    break;
            }


        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {

        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvGroups.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.EditCmd,
                    100,
                    btnGroupContextButtons_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnGroupContextButtons.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Add,
                   200,
                   btnGroupContextButtons_AddButtonClicked) { Enabled = btnGroupContextButtons.AddButtonEnabled, Image = ContextButtons.GetAddButtonImage() };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    btnGroupContextButtons_RemoveButtonClicked) { Image = ContextButtons.GetRemoveButtonImage(), Enabled = btnGroupContextButtons.RemoveButtonEnabled };

            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 400);
            menu.Items.Add(item);
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("TerminalGroup", lvGroups.ContextMenuStrip, lvGroups);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void ContextMenuDetailedViewStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvDetailedGroups.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.Add,
                    100,
                    btnTerminalGroupContextButtons_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnTerminalGroupContextButtons.AddButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Delete,
                   200,
                   btnTerminalGroupContextButtons_RemoveButtonClicked)
                {
                    Enabled = btnTerminalGroupContextButtons.RemoveButtonEnabled,
                    Image = ContextButtons.GetRemoveButtonImage()
                };

            menu.Items.Add(item);


            item = new ExtendedMenuItem("-", 400);
            menu.Items.Add(item);
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("TerminalGroupConnection", lvDetailedGroups.ContextMenuStrip, lvDetailedGroups);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvGroups_SelectionChanged(object sender, EventArgs e)
        {
            if (lvGroups.Selection.Count > 0)
            {

                btnGroupContextButtons.EditButtonEnabled = (lvGroups.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.TerminalEdit));
                btnGroupContextButtons.RemoveButtonEnabled = (lvGroups.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.TerminalEdit));
                btnTerminalGroupContextButtons.AddButtonEnabled = (lvGroups.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.TerminalEdit));

                if (!lblGroupHeader.Visible)
                {
                    lblGroupHeader.Visible = true;
                    lvDetailedGroups.Visible = true;
                    btnTerminalGroupContextButtons.Visible = true;
                    lblNoSelection.Visible = false;
                }


                if (lvGroups.Selection.Count > 0)
                {
                    LoadDetailedGroups(((RecordIdentifier)(lvGroups.Row(lvGroups.Selection.FirstSelectedRow).Tag)));
                }
            }
            else
            {
                lblGroupHeader.Visible = false;
                lvDetailedGroups.Visible = false;
                btnTerminalGroupContextButtons.Visible = false;
                lblNoSelection.Visible = true;
            }

        }

        private void btnGroupContextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.NewGroup(selectedGroupId);
            dlg.ShowDialog();
        }

        private void btnGroupContextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.NewGroup();
            dlg.Show();

        }

        private void btnGroupContextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvGroups.Selection.Count == 1)
            {
                if (QuestionDialog.Show(Resources.DeleteGroupQuestion, Resources.DeleteGroup) == DialogResult.Yes)
                {
                    var groupId = ((RecordIdentifier)(lvGroups.Row(lvGroups.Selection.FirstSelectedRow).Tag));

                    foreach (var terminals in terminalDetailedGroup)
                    {
                        Providers.TerminalGroupConnectionData.Delete(PluginEntry.DataModel, terminals.TerminalId, groupId, "");

                    }


                    Providers.TerminalGroupData.Delete(PluginEntry.DataModel, groupId);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "TerminalGroup", groupId, null);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "TerminalGroupConnection", groupId, null);
                }
            }
        }

        private void btnTerminalGroupContextButtons_AddButtonClicked(object sender, EventArgs e)
        {

            var dlg = new Dialogs.NewTerminalGroupConnection(selectedGroupId);
            dlg.ShowDialog();
        }

        private void btnTerminalGroupContextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvDetailedGroups.Selection.Count > 0)
            {
                int selectedRowNumber;

                var terminalGroupId = ((RecordIdentifier)(lvGroups.Row(lvGroups.Selection.FirstSelectedRow).Tag));

                for (int i = 0; i < lvDetailedGroups.Selection.Count; i++)
                {
                    selectedRowNumber = lvDetailedGroups.Selection.GetSelectedItem(i);

                    var terminalId = ((lvDetailedGroups.Row(selectedRowNumber)).Tag);
                    var storeId = ((RecordIdentifier)((lvDetailedGroups.Row(selectedRowNumber)).Tag)).SecondaryID;
                    Providers.TerminalGroupConnectionData.Delete(PluginEntry.DataModel, terminalId.ToString(), terminalGroupId, storeId);
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete,
                                                                           "TerminalGroupConnection", selectedGroupId,
                                                                           null);
            }
        }

        private void lvDetailedGroups_SelectionChanged(object sender, EventArgs e)
        {

            btnTerminalGroupContextButtons.RemoveButtonEnabled = (lvDetailedGroups.Selection.Count >= 1 && PluginEntry.DataModel.HasPermission(Permission.TerminalEdit));
        }

        private void lvDetailedGroups_HeaderClicked(object sender, ColumnEventArgs args)
        {


            if (lvDetailedGroups.SortColumn == args.Column)
            {
                lvDetailedGroups.SetSortColumn(args.Column, !lvDetailedGroups.SortedAscending);
                LoadDetailedGroups(selectedGroupId);

            }
            else
            {
                lvDetailedGroups.SetSortColumn(args.Column, true);
                LoadDetailedGroups(selectedGroupId);

            }
        }

        private void lvGroups_HeaderClicked(object sender, ColumnEventArgs args)
        {


            if (lvGroups.SortColumn == args.Column)
            {
                lvGroups.SetSortColumn(args.Column, !lvGroups.SortedAscending);
                LoadData(false);

            }
            else
            {
                lvGroups.SetSortColumn(args.Column, true);
                LoadData(false);

            }
        }

        private void lvGroups_DoubleClick(object sender, EventArgs e)
        {
            if (lvGroups.Selection.Count > 0 && btnGroupContextButtons.EditButtonEnabled)
            {
                btnGroupContextButtons_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void btnTerminalGroupContextButtons_Load(object sender, EventArgs e)
        {

        }
    }
}

