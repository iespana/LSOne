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
    internal partial class HospitalityTypeRoutingPage : UserControl, ITabView
    {
        IRoutable station;
        private RecordIdentifier selectedHospitalityTypeConnection;
        private bool allHospitalityTypesInitallyChecked;

        public HospitalityTypeRoutingPage()
        {
            InitializeComponent();

            lvHospitalityTypeConnections.ContextMenuStrip = new ContextMenuStrip();
            lvHospitalityTypeConnections.ContextMenuStrip.Opening += ContextMenuStripHospitalityType_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HospitalityTypeRoutingPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            station = (IRoutable)internalContext;

            LoadHospitalityTypeConnections();
            LoadAllHospitalityTypesCheckBox();
        }

        public bool DataIsModified()
        {
            if (allHospitalityTypesInitallyChecked != chkAllHospitalityTypes.Checked) return true;
            else return false;
        }

        public bool SaveData()
        {
            if (allHospitalityTypesInitallyChecked != chkAllHospitalityTypes.Checked)
            {
                allHospitalityTypesInitallyChecked = chkAllHospitalityTypes.Checked;
                if (chkAllHospitalityTypes.Checked)
                {
                    Providers.KitchenDisplayHospitalityTypeRoutingConnectionData.MakeSureAllTypeConnectionExists(PluginEntry.DataModel,
                                                                                                (string)station.ID);
                }
                else
                {
                    Providers.KitchenDisplayHospitalityTypeRoutingConnectionData.MakeSureAllTypeConnectionDoesntExists(
                        PluginEntry.DataModel, (string)station.ID);
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
                case "KitchenDisplayHospitalityTypeConnection":
                    LoadHospitalityTypeConnections();
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

        private void LoadHospitalityTypeConnections()
        {
            lvHospitalityTypeConnections.ClearRows();            

            var hospitalityTypeConnections = Providers.KitchenDisplayHospitalityTypeRoutingConnectionData.GetForKds(PluginEntry.DataModel, (string)station.ID, new List<string>()).Where(x => x.Type != StationSelection.HospitalityTypeConnectionEnum.All);

            foreach (var kitchenDisplayHospitalityTypeRoutingConnection in hospitalityTypeConnections)
            {               
                Row row = new Row();
                row.AddText(kitchenDisplayHospitalityTypeRoutingConnection.ConnectionDescription);
                row.AddText(LSOneKitchenDisplayHospitalityTypeRoutingConnection.IncludeEnumToString(kitchenDisplayHospitalityTypeRoutingConnection.IncludeExclude));

                row.Tag = (RecordIdentifier)kitchenDisplayHospitalityTypeRoutingConnection.Id;

                lvHospitalityTypeConnections.AddRow(row);

                if (selectedHospitalityTypeConnection == kitchenDisplayHospitalityTypeRoutingConnection.Id)
                {
                    lvHospitalityTypeConnections.Selection.Set(lvHospitalityTypeConnections.RowCount - 1);
                }
            }

            lvHospitalityTypeConnections.AutoSizeColumns();
        }

        private void LoadAllHospitalityTypesCheckBox()
        {
            var hospitalityTypeConnections = Providers.KitchenDisplayHospitalityTypeRoutingConnectionData.GetForKds(PluginEntry.DataModel, (string)station.ID, new List<string>());
            // Have the all terminals check box checked if a rule exists for all items. We don't want to show that rule since the check box takes care of it.
            bool containsAllTypeConnection =
                hospitalityTypeConnections.RemoveAll(connection => connection.Type == StationSelection.HospitalityTypeConnectionEnum.All) > 0;

            chkAllHospitalityTypes.Checked = containsAllTypeConnection;
            allHospitalityTypesInitallyChecked = containsAllTypeConnection;
        }

        private void btnsHospitalityTypeConnection_AddButtonClicked(object sender, System.EventArgs e)
        {
            var dlg = new HospitalityTypeRoutingConnectionDialog(station.ID);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedHospitalityTypeConnection = dlg.ID;
                LoadHospitalityTypeConnections();
            }
        }

        private void btnsHospitalityTypeConnection_RemoveButtonClicked(object sender, System.EventArgs e)
        {
            if (lvHospitalityTypeConnections.Selection.Count == 1)
            {
                PluginOperationsHelper.DeleteHospitalityTypeConnection((RecordIdentifier)lvHospitalityTypeConnections.Selection[0].Tag);
            }
            else
            {
                PluginOperationsHelper.DeleteHospitalityTypeConnections(lvHospitalityTypeConnections.Selection.GetSelectedTags<RecordIdentifier>());
            }
        }

        void ContextMenuStripHospitalityType_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvHospitalityTypeConnections.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                   Properties.Resources.Edit,
                   100,
                   btnsHospitalityTypeConnection_EditButtonClicked);

            item.Enabled = btnsHospitalityTypeConnection.EditButtonEnabled;
            item.Image = ContextButtons.GetEditButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   btnsHospitalityTypeConnection_AddButtonClicked);

            item.Enabled = btnsHospitalityTypeConnection.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsHospitalityTypeConnection_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsHospitalityTypeConnection.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("HospitalityTypeRoutingConnectionsList", lvHospitalityTypeConnections.ContextMenuStrip, lvHospitalityTypeConnections);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvHospitalityTypeConnections_SelectionChanged(object sender, EventArgs e)
        {
            btnsHospitalityTypeConnection.RemoveButtonEnabled = lvHospitalityTypeConnections.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations);
            btnsHospitalityTypeConnection.EditButtonEnabled = lvHospitalityTypeConnections.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations);
        }

        private void btnsHospitalityTypeConnection_EditButtonClicked(object sender, EventArgs e)
        {
            using(EditHospitalityTypeRoutingConnectionDialog dlg = new EditHospitalityTypeRoutingConnectionDialog((RecordIdentifier)lvHospitalityTypeConnections.Selection[0].Tag))
            {
                dlg.ShowDialog();
            }
        }

        private void lvHospitalityTypeConnections_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (btnsHospitalityTypeConnection.EditButtonEnabled)
            {
                btnsHospitalityTypeConnection_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}