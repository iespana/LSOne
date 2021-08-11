using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
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
    public partial class EditItemRoutingConnectionDialog : DialogBase
    {
        LSOneKitchenDisplayItemRoutingConnection itemRoutingConnection;

        public EditItemRoutingConnectionDialog(RecordIdentifier itemRoutingConnectionId)
        {
            InitializeComponent();

            this.itemRoutingConnection = Providers.KitchenDisplayItemRoutingConnectionData.Get(PluginEntry.DataModel, itemRoutingConnectionId);

            cmbType.SelectedIndex = (int)itemRoutingConnection.Type - 1;

            switch (itemRoutingConnection.Type)
            {                                    
                case DataLayer.BusinessObjects.Hospitality.StationSelection.TypeEnum.RetailGroup:
                    cmbConnection.SelectedData = Providers.RetailGroupData.Get(PluginEntry.DataModel, itemRoutingConnection.ConnectionId);
                    break;
                case DataLayer.BusinessObjects.Hospitality.StationSelection.TypeEnum.Item:
                    cmbConnection.SelectedData = Providers.RetailItemData.Get(PluginEntry.DataModel, itemRoutingConnection.ConnectionId);
                    break;
                case DataLayer.BusinessObjects.Hospitality.StationSelection.TypeEnum.SpecialGroup:
                    cmbConnection.SelectedData = Providers.SpecialGroupData.Get(PluginEntry.DataModel, itemRoutingConnection.ConnectionId);
                    break;
            }

            cmbInclude.SelectedIndex = (int)itemRoutingConnection.IncludeExclude;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs args)
        {
            btnOK.Enabled =
                cmbInclude.SelectedIndex != (int)itemRoutingConnection.IncludeExclude ||
                cmbConnection.SelectedDataID != itemRoutingConnection.ConnectionId;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            itemRoutingConnection.ConnectionId = (string)cmbConnection.SelectedData.ID;
            itemRoutingConnection.IncludeExclude = (LSOneKitchenDisplayItemRoutingConnection.IncludeEnum)cmbInclude.SelectedIndex;

            Providers.KitchenDisplayItemRoutingConnectionData.Save(PluginEntry.DataModel, itemRoutingConnection);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Edit, "KitchenDisplayItemConnection", itemRoutingConnection.Id, itemRoutingConnection);

            Close();
        }

        private void cmbConnection_DropDown(object sender, LSOne.Controls.DropDownEventArgs e)
        {
            switch(itemRoutingConnection.Type)
            {
                case DataLayer.BusinessObjects.Hospitality.StationSelection.TypeEnum.RetailGroup:
                    e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, itemRoutingConnection.ConnectionId, "", DataLayer.BusinessObjects.Enums.SearchTypeEnum.RetailGroups);
                    break;
                case DataLayer.BusinessObjects.Hospitality.StationSelection.TypeEnum.Item:
                    e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, itemRoutingConnection.ConnectionId, "", DataLayer.BusinessObjects.Enums.SearchTypeEnum.RetailItems);
                    break;
                case DataLayer.BusinessObjects.Hospitality.StationSelection.TypeEnum.SpecialGroup:
                    e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, itemRoutingConnection.ConnectionId, "", DataLayer.BusinessObjects.Enums.SearchTypeEnum.SpecialGroups);
                    break;
            }
            
        }
    }
}
