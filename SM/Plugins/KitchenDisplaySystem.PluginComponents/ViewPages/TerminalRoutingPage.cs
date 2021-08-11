using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    internal partial class TerminalRoutingPage : UserControl, ITabView
    {
        IRoutable station;
        private RecordIdentifier selectedTerminalConnection;
        private bool allTerminalsInitallyChecked;

        public TerminalRoutingPage()
        {
            InitializeComponent();

            lvTerminalConnections.ContextMenuStrip = new ContextMenuStrip();
            lvTerminalConnections.ContextMenuStrip.Opening += ContextMenuStripTerminal_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new TerminalRoutingPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            station = (IRoutable)internalContext;

            LoadTerminalConnections();
            LoadAllTerminalsCheckBox();
        }

        public bool DataIsModified()
        {
            if (allTerminalsInitallyChecked != chkAllTerminals.Checked) return true;
            else return false;
        }

        public bool SaveData()
        {
            if (allTerminalsInitallyChecked != chkAllTerminals.Checked)
            {
                allTerminalsInitallyChecked = chkAllTerminals.Checked;
                if (chkAllTerminals.Checked)
                {
                    Providers.KitchenDisplayTerminalRoutingConnectionData.MakeSureAllTypeConnectionExists(PluginEntry.DataModel,
                                                                                                (string)station.ID);
                }
                else
                {
                    Providers.KitchenDisplayTerminalRoutingConnectionData.MakeSureAllTypeConnectionDoesntExists(
                        PluginEntry.DataModel, station.ID);
                }
            }

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenDisplayTerminalConnection":
                    LoadTerminalConnections();
                    break;
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void LoadTerminalConnections()
        {
            lvTerminalConnections.ClearRows();

            var terminalConnections = Providers.KitchenDisplayTerminalRoutingConnectionData.GetForKds(PluginEntry.DataModel, station.ID, new List<string>()).Where(x => x.Type != StationSelection.TerminalConnectionEnum.All);

            foreach (var terminalConnection in terminalConnections)
            {

                Row row = new Row();
                row.AddText(terminalConnection.ConnectionId);
                row.AddText(terminalConnection.ConnectionDescription);
                row.AddText(LSOneKitchenDisplayTerminalRoutingConnection.IncludeEnumToString(terminalConnection.IncludeExclude));

                row.Tag = (RecordIdentifier)terminalConnection.Id;

                lvTerminalConnections.AddRow(row);

                if (selectedTerminalConnection == terminalConnection.Id)
                {
                    lvTerminalConnections.Selection.Set(lvTerminalConnections.RowCount - 1);
                }
            }

            lvTerminalConnections.AutoSizeColumns();
        }

        private void LoadAllTerminalsCheckBox()
        {
            var terminalConnections = Providers.KitchenDisplayTerminalRoutingConnectionData.GetForKds(PluginEntry.DataModel, station.ID, new List<string>());

            // Have the all terminals check box checked if a rule exists for all items. We don't want to show that rule since the check box takes care of it.
            bool containsAllTypeConnection =
                terminalConnections.RemoveAll(connection => connection.Type == StationSelection.TerminalConnectionEnum.All) > 0;

            chkAllTerminals.Checked = containsAllTypeConnection;
            allTerminalsInitallyChecked = containsAllTypeConnection;
        }

        private void btnsTerminalConnection_AddButtonClicked(object sender, System.EventArgs e)
        {
            var dlg = new TerminalRoutingConnectionDialog(station.ID);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedTerminalConnection = dlg.ID;
                LoadTerminalConnections();
            }
        }

        private void btnsTerminalConnection_RemoveButtonClicked(object sender, System.EventArgs e)
        {
            if (lvTerminalConnections.Selection.Count == 1)
            {
                PluginOperationsHelper.DeleteTerminalConnection((RecordIdentifier)lvTerminalConnections.Selection[0].Tag);
            }
            else
            {
                PluginOperationsHelper.DeleteTerminalConnections(lvTerminalConnections.Selection.GetSelectedTags<RecordIdentifier>());
            }
        }

        void ContextMenuStripTerminal_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvTerminalConnections.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                   Properties.Resources.Edit,
                   100,
                   btnsTerminalConnection_EditButtonClicked);

            item.Enabled = btnsTerminalConnection.EditButtonEnabled;
            item.Image = ContextButtons.GetEditButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   btnsTerminalConnection_AddButtonClicked);

            item.Enabled = btnsTerminalConnection.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsTerminalConnection_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsTerminalConnection.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("TerminalRoutingConnectionsList", lvTerminalConnections.ContextMenuStrip, lvTerminalConnections);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvTerminalConnections_SelectionChanged(object sender, EventArgs e)
        {
            btnsTerminalConnection.RemoveButtonEnabled = lvTerminalConnections.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations);
            btnsTerminalConnection.EditButtonEnabled = lvTerminalConnections.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations);
        }

        private void btnsTerminalConnection_EditButtonClicked(object sender, EventArgs e)
        {
            using(EditTerminalRoutingConnectionDialog dlg = new EditTerminalRoutingConnectionDialog((RecordIdentifier)lvTerminalConnections.Selection[0].Tag))
            {
                dlg.ShowDialog();
            }
        }

        private void lvTerminalConnections_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if(btnsTerminalConnection.EditButtonEnabled)
            {
                btnsTerminalConnection_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}