using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class ItemRoutingConnectionDialog : DialogBase
    {
        private RecordIdentifier kdsId;
        private LSOneKitchenDisplayItemRoutingConnection itemRoutingConnection;

        private List<List<DataEntity>> selectionLists;
        private List<List<DataEntity>> addLists;
        private List<List<DataEntity>> removeLists;

        public ItemRoutingConnectionDialog(RecordIdentifier kdsId)
        {
            InitializeComponent();

            selectionLists = new List<List<DataEntity>>();
            selectionLists.Add(Providers.KitchenDisplayItemRoutingConnectionData.ItemsConnectedToKds(PluginEntry.DataModel, kdsId));
            selectionLists.Add(Providers.KitchenDisplayItemRoutingConnectionData.RetailGroupsConnectedToKds(PluginEntry.DataModel, kdsId));
            selectionLists.Add(Providers.KitchenDisplayItemRoutingConnectionData.SpecialGroupsConnectedToKds(PluginEntry.DataModel, kdsId));

            addLists = new List<List<DataEntity>>();
            addLists.Add(new List<DataEntity>());
            addLists.Add(new List<DataEntity>());
            addLists.Add(new List<DataEntity>());

            removeLists = new List<List<DataEntity>>();
            removeLists.Add(new List<DataEntity>());
            removeLists.Add(new List<DataEntity>());
            removeLists.Add(new List<DataEntity>());

            this.kdsId = kdsId;
            cmbType.SelectedIndex = 0;
            cmbInclude.SelectedIndex = 0;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier ID
        {
            get { return itemRoutingConnection.Id; }
        }

        private void CheckEnabled()
        {
            bool added = (addLists[0].Count > 0 || addLists[1].Count > 0 || addLists[2].Count > 0);
            bool removed = (removeLists[0].Count > 0 || removeLists[1].Count > 0 || removeLists[2].Count > 0);
            btnOK.Enabled = added || removed;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //i=0 gives the list of retail items
            //i=1 gives the list of retail groups
            //i=2 gives the list of special groups
            for (int i = 0; i < 3; i++)
            {
                foreach (DataEntity item in addLists[i])
                {
                    itemRoutingConnection = new LSOneKitchenDisplayItemRoutingConnection();
                    itemRoutingConnection.Id = Guid.NewGuid();
                    itemRoutingConnection.StationId = (string)kdsId;
                    itemRoutingConnection.Type = SearchTypeToTypeEnum((SearchTypeEnum) i);
                    itemRoutingConnection.ConnectionId = (string)item.ID;
                    itemRoutingConnection.IncludeExclude = (LSOneKitchenDisplayItemRoutingConnection.IncludeEnum)cmbInclude.SelectedIndex;
                    Providers.KitchenDisplayItemRoutingConnectionData.Save(PluginEntry.DataModel, itemRoutingConnection);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                foreach (DataEntity item in removeLists[i])
                {
                    switch (i)
                    {
                        case 0:
                            itemRoutingConnection = Providers.KitchenDisplayItemRoutingConnectionData.GetForKdsAndItemID(PluginEntry.DataModel, kdsId, item);
                            break;
                        case 1:
                            itemRoutingConnection = Providers.KitchenDisplayItemRoutingConnectionData.GetForKdsAndRetailGroupID(PluginEntry.DataModel, kdsId, item);
                            break;
                        case 2:
                            itemRoutingConnection = Providers.KitchenDisplayItemRoutingConnectionData.GetForKdsAndSpecialGroupID(PluginEntry.DataModel, kdsId, item);
                            break;
                    }
                    Providers.KitchenDisplayItemRoutingConnectionData.Delete(PluginEntry.DataModel, itemRoutingConnection.Id);
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbStationSelectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((StationSelection.TypeEnum)cmbType.SelectedIndex + 1)
            {
                case StationSelection.TypeEnum.Item:
                    cmbConnection.SelectionList = new List<IDataEntity>(selectionLists[0]);
                    break;
                case StationSelection.TypeEnum.RetailGroup:
                    cmbConnection.SelectionList = new List<IDataEntity>(selectionLists[1]);
                    break;
                case StationSelection.TypeEnum.SpecialGroup:
                    cmbConnection.SelectionList = new List<IDataEntity>(selectionLists[2]);
                    break;
            }
            cmbConnection.DropDown += cmbConnection_DropDown;
        }

        private void cmbConnection_DropDown(object sender, DropDownEventArgs e)
        {
            List<DataEntity> selectionList = new List<DataEntity>();
            List<DataEntity> addList = new List<DataEntity>();
            List<DataEntity> removeList = new List<DataEntity>();
            switch ((StationSelection.TypeEnum)cmbType.SelectedIndex + 1)
            {
                case StationSelection.TypeEnum.Item:
                    selectionList = selectionLists[0];
                    addList = addLists[0];
                    removeList = removeLists[0];
                    break;
                case StationSelection.TypeEnum.RetailGroup:
                    selectionList = selectionLists[1];
                    addList = addLists[1];
                    removeList = removeLists[1];
                    break;
                case StationSelection.TypeEnum.SpecialGroup:
                    selectionList = selectionLists[2];
                    addList = addLists[2];
                    removeList = removeLists[2];
                    break;
            }
            e.ControlToEmbed = new MultiSearchPanel(PluginEntry.DataModel,
                                                                selectionList,
                                                                addList,
                                                                removeList,
                                                                TypeToSearchTypeEnum(),
                                                                false);
            cmbConnection.ShowDropDownOnTyping = true;
        }

        private StationSelection.TypeEnum SearchTypeToTypeEnum(SearchTypeEnum searchType)
        {
            switch (searchType)
            {
                case SearchTypeEnum.RetailItems:
                    return StationSelection.TypeEnum.Item;
                case SearchTypeEnum.RetailGroups:
                    return StationSelection.TypeEnum.RetailGroup;
                case SearchTypeEnum.SpecialGroups:
                    return StationSelection.TypeEnum.SpecialGroup;
            }
            return StationSelection.TypeEnum.All;
        }

        private SearchTypeEnum TypeToSearchTypeEnum()
        {
            switch ((StationSelection.TypeEnum)cmbType.SelectedIndex + 1)
            {
                case StationSelection.TypeEnum.Item:
                    return SearchTypeEnum.RetailItems;
                case StationSelection.TypeEnum.RetailGroup:
                    return SearchTypeEnum.RetailGroups;
                case StationSelection.TypeEnum.SpecialGroup:
                    return SearchTypeEnum.SpecialGroups;
            }
            return SearchTypeEnum.RetailItems;
        }

        private void cmbConnection_SelectedDataChanged(object sender, EventArgs e)
        {
            switch ((StationSelection.TypeEnum)cmbType.SelectedIndex + 1)
            {
                case StationSelection.TypeEnum.Item:
                    selectionLists[0] = cmbConnection.SelectionList.Cast<DataEntity>().ToList();
                    addLists[0] = cmbConnection.AddList.Cast<DataEntity>().ToList();
                    removeLists[0] = cmbConnection.RemoveList.Cast<DataEntity>().ToList();
                    break;
                case StationSelection.TypeEnum.RetailGroup:
                    selectionLists[1] = cmbConnection.SelectionList.Cast<DataEntity>().ToList();
                    addLists[1] = cmbConnection.AddList.Cast<DataEntity>().ToList();
                    removeLists[1] = cmbConnection.RemoveList.Cast<DataEntity>().ToList();
                    break;
                case StationSelection.TypeEnum.SpecialGroup:
                    selectionLists[2] = cmbConnection.SelectionList.Cast<DataEntity>().ToList();
                    addLists[2] = cmbConnection.AddList.Cast<DataEntity>().ToList();
                    removeLists[2] = cmbConnection.RemoveList.Cast<DataEntity>().ToList();
                    break;
            }

            CheckEnabled();
        }

    }
}
