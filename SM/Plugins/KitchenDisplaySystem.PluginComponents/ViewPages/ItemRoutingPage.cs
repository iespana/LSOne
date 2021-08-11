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
    internal partial class ItemRoutingPage : UserControl, ITabView
    {
        IRoutable station;
        private RecordIdentifier selectedItemConnection;
        private bool allItemsInitallyChecked;

        public ItemRoutingPage()
        {
            InitializeComponent();

            lvItemConnections.ContextMenuStrip = new ContextMenuStrip();
            lvItemConnections.ContextMenuStrip.Opening += ContextMenuStripItem_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ItemRoutingPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            station = (IRoutable)internalContext;

            LoadItemConnections();
            LoadAllItemsCheckBox();
        }

        public bool DataIsModified()
        {
            if (allItemsInitallyChecked != chkAllItems.Checked) return true;
            else return false;
        }

        public bool SaveData()
        {
            if (allItemsInitallyChecked != chkAllItems.Checked)
            {
                allItemsInitallyChecked = chkAllItems.Checked;
                if (chkAllItems.Checked)
                {
                    Providers.KitchenDisplayItemRoutingConnectionData.MakeSureAllTypeConnectionExists(PluginEntry.DataModel, (string)station.ID);                }
                else
                {
                    Providers.KitchenDisplayItemRoutingConnectionData.MakeSureAllTypeConnectionDoesntExists(PluginEntry.DataModel, (string)station.ID);                }
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
                case "KitchenDisplayItemConnection":
                    LoadItemConnections();
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

        private void LoadItemConnections()
        {                        
            lvItemConnections.ClearRows();

            var itemConnections = Providers.KitchenDisplayItemRoutingConnectionData.GetForKds(PluginEntry.DataModel, station.ID, new List<string>()).Where(x => x.Type != StationSelection.TypeEnum.All);

            foreach (var itemConnection in itemConnections)
            {                
                Row row = new Row();
                row.AddText(itemConnection.TypeDescription);
                row.AddText(itemConnection.ConnectionId);
                row.AddText(itemConnection.ConnectionDescription);
                row.AddText(LSOneKitchenDisplayItemRoutingConnection.IncludeEnumToString(itemConnection.IncludeExclude));

                row.Tag = (RecordIdentifier)itemConnection.Id;

                lvItemConnections.AddRow(row);

                if (selectedItemConnection == itemConnection.Id)
                {
                    lvItemConnections.Selection.Set(lvItemConnections.RowCount - 1);
                }
            }

            lvItemConnections.AutoSizeColumns();
        }
        
        private void LoadAllItemsCheckBox()
        {
            var itemConnections = Providers.KitchenDisplayItemRoutingConnectionData.GetForKds(PluginEntry.DataModel, station.ID, new List<string>());

            // Have the all items check box checked if a rule exists for all items. We don't want to show that rule since the check box takes care of it.
            bool containsAllTypeConnection =
                itemConnections.RemoveAll(connection => connection.Type == StationSelection.TypeEnum.All) > 0;

            chkAllItems.Checked = containsAllTypeConnection;
            allItemsInitallyChecked = containsAllTypeConnection;
        }

        private void btnsItemConnection_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new ItemRoutingConnectionDialog(station.ID);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedItemConnection = dlg.ID;
                LoadItemConnections();
            }
        }

        private void btnsItemConnection_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvItemConnections.Selection.Count == 1)
            {
                PluginOperationsHelper.DeleteItemConnection((RecordIdentifier)lvItemConnections.Selection[0].Tag);
            }
            else
            {                            
                PluginOperationsHelper.DeleteItemConnections(lvItemConnections.Selection.GetSelectedTags<RecordIdentifier>());
            }
        }

        void ContextMenuStripItem_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvItemConnections.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                   Properties.Resources.Edit,
                   100,
                   btnsItemConnection_EditButtonClicked);

            item.Enabled = btnsItemConnection.EditButtonEnabled;
            item.Image = ContextButtons.GetEditButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   btnsItemConnection_AddButtonClicked);

            item.Enabled = btnsItemConnection.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsItemConnection_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsItemConnection.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ItemRoutingConnectionsList", lvItemConnections.ContextMenuStrip, lvItemConnections);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnsItemConnection_EditButtonClicked(object sender, EventArgs e)
        {
            using (EditItemRoutingConnectionDialog dlg = new EditItemRoutingConnectionDialog((RecordIdentifier)lvItemConnections.Selection[0].Tag))
            {
                dlg.ShowDialog();
            }
        }

        private void lvItemConnections_SelectionChanged(object sender, EventArgs e)
        {
            btnsItemConnection.RemoveButtonEnabled = lvItemConnections.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations);
            btnsItemConnection.EditButtonEnabled = lvItemConnections.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations);
        }

        private void lvItemConnections_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if(btnsItemConnection.EditButtonEnabled)
            {
                btnsItemConnection_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}