using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.KDSBusinessObjects.Enums;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class DisplaysView : ViewBase
    {
        RecordIdentifier selectedID = "";

        public DisplaysView(RecordIdentifier kitchenDisplaystationId)
            : this()
        {
            selectedID = kitchenDisplaystationId;
        }

        public DisplaysView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;



            lvKitchenDisplayStations.Columns[0].Tag = KitchenDisplayStationSortingEnum.KitchenDisplayStationId;
            lvKitchenDisplayStations.Columns[1].Tag = KitchenDisplayStationSortingEnum.KitchenDisplayStationName;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations);

            lvKitchenDisplayStations.SmallImageList = PluginEntry.Framework.GetImageList();
            lvKitchenDisplayStations.ContextMenuStrip = new ContextMenuStrip();
            lvKitchenDisplayStations.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations);

            lvKitchenDisplayStations.SortColumn = 1;
            lvKitchenDisplayStations.SortedBackwards = false;
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvKitchenDisplayStations.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   new EventHandler(btnsEditAddRemove_AddButtonClicked));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("RestaurantStationsList", lvKitchenDisplayStations.ContextMenuStrip, lvKitchenDisplayStations);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("KITCHENDISPLAYSTATIONSLog", RecordIdentifier.Empty, Properties.Resources.DisplayStations, false));
            
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.DisplayStations;
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
            LoadKitchenDisplayStations();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenDisplayStation":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedID = changeIdentifier;
                    }

                    LoadKitchenDisplayStations();
                    break;
            }

        }

        private void LoadKitchenDisplayStations()
        {
            List<KitchenDisplayStation> restaurantStations;
            ListViewItem listItem;

            lvKitchenDisplayStations.Items.Clear();
            restaurantStations = Providers.KitchenDisplayStationData.GetList(
                PluginEntry.DataModel, 
                (KitchenDisplayStationSortingEnum)lvKitchenDisplayStations.Columns[lvKitchenDisplayStations.SortColumn].Tag, 
                lvKitchenDisplayStations.SortedBackwards);

            foreach (var kitchenDisplayStation in restaurantStations)
            {
                listItem = new ListViewItem((string)kitchenDisplayStation.ID);
                listItem.SubItems.Add(kitchenDisplayStation.Text);
                listItem.SubItems.Add(kitchenDisplayStation.KitchenDisplayFunctionalProfileDescription);
                listItem.SubItems.Add(kitchenDisplayStation.KitchenDisplayStyleProfileDescription);
                listItem.SubItems.Add(kitchenDisplayStation.KitchenDisplayVisualProfileDescription);
                listItem.SubItems.Add(KitchenDisplayStation.GetStationTypeText(kitchenDisplayStation.StationType));
                listItem.SubItems.Add(KitchenDisplayStation.GetDisplayModeText(kitchenDisplayStation.DisplayMode));
                listItem.SubItems.Add(kitchenDisplayStation.NextStationName);
                
                listItem.ImageIndex = -1;

                listItem.Tag = kitchenDisplayStation.ID;

                lvKitchenDisplayStations.Add(listItem);

                if (selectedID == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvKitchenDisplayStations.Columns[lvKitchenDisplayStations.SortColumn].ImageIndex = (lvKitchenDisplayStations.SortedBackwards ? 1 : 0);

            lvKitchenDisplayStations_SelectedIndexChanged(this, EventArgs.Empty);

            lvKitchenDisplayStations.BestFitColumns();
        }

        private void lvKitchenDisplayStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            var permission = PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations);

            btnsEditAddRemove.EditButtonEnabled =
                lvKitchenDisplayStations.SelectedItems.Count == 1 && permission;
            btnsEditAddRemove.RemoveButtonEnabled = 
                lvKitchenDisplayStations.SelectedItems.Count > 0 && permission;
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperationsHelper.CreateDisplayStation();
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            var stationId = (RecordIdentifier) lvKitchenDisplayStations.SelectedItems[0].Tag;

            PluginOperationsHelper.ShowDisplayStationsView(stationId);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvKitchenDisplayStations.SelectedItems.Count == 1)
            {
                PluginOperationsHelper.DeleteDisplayStation((RecordIdentifier)lvKitchenDisplayStations.SelectedItems[0].Tag);
            }
            else
            {
                PluginOperationsHelper.DeleteDisplayStations(lvKitchenDisplayStations.GetSelectedIDs());
            }
        }

        private void lvRestaurantStations_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // We only want to sort based on the first two columns.
            if (e.Column > 1)
            {
                return;
            }

            if (lvKitchenDisplayStations.SortColumn == e.Column)
            {
                lvKitchenDisplayStations.SortedBackwards = !lvKitchenDisplayStations.SortedBackwards;
            }
            else
            {
                lvKitchenDisplayStations.SortedBackwards = false;
            }



            if (lvKitchenDisplayStations.SortColumn != -1)
            {
                lvKitchenDisplayStations.Columns[lvKitchenDisplayStations.SortColumn].ImageIndex = 2;
                lvKitchenDisplayStations.SortColumn = e.Column;

            }

            LoadKitchenDisplayStations();
        }

        private void lvRestaurantStations_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        protected override void OnClose()
        {
            lvKitchenDisplayStations.SmallImageList = null;

            base.OnClose();
        }

        
    }
}
