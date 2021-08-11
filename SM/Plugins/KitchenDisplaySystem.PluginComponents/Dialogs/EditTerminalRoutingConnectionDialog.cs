using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class EditTerminalRoutingConnectionDialog : DialogBase
    {
        LSOneKitchenDisplayTerminalRoutingConnection terminalRoutingConnection;

        public EditTerminalRoutingConnectionDialog(RecordIdentifier terminalRoutingConnectionId)
        {
            InitializeComponent();

            terminalRoutingConnection = Providers.KitchenDisplayTerminalRoutingConnectionData.Get(PluginEntry.DataModel, terminalRoutingConnectionId);

            cmbConnection.SelectedData = Providers.TerminalData.Get(PluginEntry.DataModel, terminalRoutingConnection.TerminalID, terminalRoutingConnection.StoreID);
            cmbInclude.SelectedIndex = (int)terminalRoutingConnection.IncludeExclude;

        }
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs args)
        {
            btnOK.Enabled =
                cmbConnection.SelectedDataID != terminalRoutingConnection.ConnectionId ||
                cmbInclude.SelectedIndex != (int)terminalRoutingConnection.IncludeExclude;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TerminalListItem selectedTerminal = (TerminalListItem)cmbConnection.SelectedData;
            terminalRoutingConnection.ConnectionId = selectedTerminal.ID + "-" + selectedTerminal.StoreID;
            terminalRoutingConnection.TerminalID = selectedTerminal.ID;
            terminalRoutingConnection.StoreID = selectedTerminal.StoreID;
            terminalRoutingConnection.IncludeExclude = (LSOneKitchenDisplayTerminalRoutingConnection.IncludeEnum)cmbInclude.SelectedIndex;

            Providers.KitchenDisplayTerminalRoutingConnectionData.Save(PluginEntry.DataModel, terminalRoutingConnection);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Edit, "KitchenDisplayTerminalConnection", terminalRoutingConnection.Id, terminalRoutingConnection);

            Close();
        }

        private void cmbConnection_RequestData(object sender, EventArgs e)
        {
            cmbConnection.SetData(Providers.TerminalData.GetList(PluginEntry.DataModel), null);
        }
    }
}
