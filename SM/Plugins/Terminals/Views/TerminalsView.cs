using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Terminals.Properties;

namespace LSOne.ViewPlugins.Terminals.Views
{
    public partial class TerminalsView : ViewBase
    {
        private List<TerminalListItem> terminals;
        public TerminalsView()
        {
            InitializeComponent();

            //HeaderIcon = Properties.Resources.Transaction16;
            HeaderText = Resources.TerminalsHeaderText;

            Attributes = ViewAttributes.Audit |
                            ViewAttributes.Close |
                            ViewAttributes.ContextBar |
                            ViewAttributes.Help;

            btnContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.TerminalEdit);

            lvTerminals.ContextMenuStrip = new ContextMenuStrip();
            lvTerminals.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            lvTerminals.SetSortColumn(1, false);
            lvTerminals.SortColumn.Tag = TerminalListItem.SortEnum.ID;

            lvTerminals.Columns[0].Tag = TerminalListItem.SortEnum.ID;
            lvTerminals.Columns[1].Tag = TerminalListItem.SortEnum.NAME;
            lvTerminals.Columns[2].Tag = TerminalListItem.SortEnum.STORENAME;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.TerminalEdit);


        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Terminals", RecordIdentifier.Empty, Properties.Resources.Terminals, false));
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
            var currentRowSelected = lvTerminals.Selection.Count > 0 ? lvTerminals.Selection.FirstSelectedRow : -1;

            if (lvTerminals.SortColumn.Tag == null)
            {
                lvTerminals.SortColumn.Tag = TerminalListItem.SortEnum.ID;
            }
            lvTerminals.ClearRows();

            terminals = Providers.TerminalData.GetAllTerminals(PluginEntry.DataModel, lvTerminals.SortedAscending, (TerminalListItem.SortEnum)lvTerminals.SortColumn.Tag);

            foreach (var terminal in terminals)
            {
                var row = new Row();

                row.AddText((string)terminal.ID);
                row.AddText(terminal.Text);
                row.AddText(terminal.StoreName);
                row.Tag = new RecordIdentifier(terminal.ID, terminal.StoreID);
                lvTerminals.AddRow(row);
            }

            if (currentRowSelected != -1)
            {
                lvTerminals.Selection.Set(currentRowSelected);
                lvTerminals.ScrollRowIntoView(currentRowSelected);
            }

            lvTerminals.AutoSizeColumns();
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
                case "Terminal":
                    LoadData(false);
                    break;
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.TerminalEdit))
                {
                    arguments.Add(new ContextBarItem(Resources.Add, ContextButtons.GetAddButtonImage(), btnContextButtons_AddButtonClicked), 10);
                }
            }

        }



        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvTerminals.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.EditCmd,
                    100,
                    btnContextButtons_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnContextButtons.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Add,
                   200,
                   btnContextButtons_AddButtonClicked);

            item.Enabled = btnContextButtons.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    btnContextButtons_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnContextButtons.RemoveButtonEnabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 400);
            menu.Items.Add(item);
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("TerminalContextMenu", lvTerminals.ContextMenuStrip, lvTerminals);

            e.Cancel = (menu.Items.Count == 0);
        }



        private void btnContextButtons_AddButtonClicked(object sender, System.EventArgs e)
        {
            PluginOperations.NewTerminal(this, e);
        }

        private void btnContextButtons_RemoveButtonClicked(object sender, System.EventArgs e)
        {

            PluginOperations.DeleteTerminal((RecordIdentifier)(lvTerminals.Row(lvTerminals.Selection.FirstSelectedRow).Tag));

        }

        private void btnContextButtons_EditButtonClicked(object sender, System.EventArgs e)
        {

            if (lvTerminals.Selection.Count != -1)
            {
                var terminalID = (RecordIdentifier)(lvTerminals.Row(lvTerminals.Selection.FirstSelectedRow).Tag);
                PluginOperations.ShowTerminal(terminalID[0], terminalID[1]);
            }
        }

        private void lvTerminals_SelectionChanged(object sender, System.EventArgs e)
        {
            btnContextButtons.EditButtonEnabled = (lvTerminals.Selection.Count == 1);
            btnContextButtons.RemoveButtonEnabled = (lvTerminals.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.TerminalEdit));
        }

        private void lvTerminals_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (lvTerminals.SortColumn == args.Column)
            {
                lvTerminals.SetSortColumn(args.Column, !lvTerminals.SortedAscending);
                LoadData(false);

            }
            else
            {
                lvTerminals.SetSortColumn(args.Column, true);
                LoadData(false);

            }

        }

        private void lvTerminals_RowDoubleClick(object sender, RowEventArgs args)
        {
            btnContextButtons_EditButtonClicked(sender, EventArgs.Empty);
        }
    }
}
