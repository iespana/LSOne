using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class TerminalRoutingConnectionDialog : DialogBase
    {
        private RecordIdentifier kdsId;
        private LSOneKitchenDisplayTerminalRoutingConnection terminalRoutingConnection;
        DataEntitySelectionList selectionList = null;
        private List<DataEntity> selectedItems;
        private List<DataEntity> entities;

        public TerminalRoutingConnectionDialog(RecordIdentifier kdsId)
        {
            InitializeComponent();
            this.kdsId = kdsId;
            cmbInclude.SelectedIndex = 0;

            entities = Providers.KitchenDisplayTerminalRoutingConnectionData.TerminalsNotConnectedToKds(PluginEntry.DataModel, kdsId);
            selectionList = new DataEntitySelectionList(entities);
            cmbConnection.DropDown += cmbConnection_DropDown;
            cmbConnection.SelectedData = selectionList;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier ID
        {
            get { return terminalRoutingConnection.Id; }
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = (cmbConnection.SelectedData.ID != "");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (var item in selectedItems)
            {
                terminalRoutingConnection = new LSOneKitchenDisplayTerminalRoutingConnection();
                terminalRoutingConnection.Id = Guid.NewGuid();
                terminalRoutingConnection.StationId = (string)kdsId;
                terminalRoutingConnection.ConnectionId = (string)item.ID[0];
                terminalRoutingConnection.TerminalID = (string)item.ID[1];
                terminalRoutingConnection.StoreID = (string)item.ID[2];
                terminalRoutingConnection.IncludeExclude = (LSOneKitchenDisplayTerminalRoutingConnection.IncludeEnum)cmbInclude.SelectedIndex;
                terminalRoutingConnection.Type = StationSelection.TerminalConnectionEnum.Terminal;

                Providers.KitchenDisplayTerminalRoutingConnectionData.Save(PluginEntry.DataModel, terminalRoutingConnection);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbConnection_DropDown(object sender, DropDownEventArgs e)
        {
            DataEntitySelectionList list = ((DualDataComboBox)sender).SelectedData as DataEntitySelectionList;
            if (list != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel(list);
            }
        }

        private void cmbConnection_SelectedDataChanged(object sender, EventArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData != null)
            {
                selectionList = (DataEntitySelectionList)((DualDataComboBox)sender).SelectedData;

                selectedItems = selectionList.GetSelectedItems();

                var names = new List<string>();
                var ids = new List<string>();

                foreach (DataEntity item in selectedItems)
                {
                    ids.Add((string)item.ID);
                    names.Add(item.Text);
                }
            }

            CheckEnabled();
        }

    }
}
