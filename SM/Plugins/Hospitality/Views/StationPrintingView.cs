using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    public partial class StationPrintingView : ViewBase
    {
        RecordIdentifier selectedID = "";

        public StationPrintingView(RecordIdentifier stationSelectionID)
            : base()
        {
            selectedID = stationSelectionID;
        }

        public StationPrintingView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;


            HeaderText = Properties.Resources.StationRouting;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting);

            lvStationSelection.ContextMenuStrip = new ContextMenuStrip();
            lvStationSelection.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            
            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting);

        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvStationSelection.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("StationPrinting", lvStationSelection.ContextMenuStrip, lvStationSelection);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("StationPrinting", 0, Properties.Resources.RestaurantStations, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.StationRouting;
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
            LoadItems();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "StationSelection":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedID = changeIdentifier;
                    }

                    LoadItems();
                    break;
            }

        }

        private void LoadItems()
        {
            List<StationSelection> stationSelections;
            ListViewItem listItem;

            lvStationSelection.Items.Clear();

            // Get all restaurant stations
            stationSelections = Providers.StationSelectionData.GetList(PluginEntry.DataModel);

            foreach (var stationSelection in stationSelections)
            {
                listItem = new ListViewItem(stationSelection.HospitalityTypeDescription);
                listItem.SubItems.Add(Enum.GetName(typeof(StationSelection.TypeEnum), (int)stationSelection.Type));
                listItem.SubItems.Add((string)stationSelection.CodeName);
                listItem.SubItems.Add(stationSelection.StationDescription);
                listItem.SubItems.Add(stationSelection.Text);

                listItem.ImageIndex = -1;

                listItem.Tag = stationSelection.ID;

                lvStationSelection.Add(listItem);

                if (selectedID == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }            

            lvStationSelection_SelectedIndexChanged(this, EventArgs.Empty);

            lvStationSelection.BestFitColumns();
        }

        private void lvStationSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = btnsEditAddRemove.RemoveButtonEnabled = lvStationSelection.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewStationSelection(this, EventArgs.Empty);  
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowStationSelection((RecordIdentifier)lvStationSelection.SelectedItems[0].Tag);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteStationSelection((RecordIdentifier)lvStationSelection.SelectedItems[0].Tag);
        }

        private void lvStationSelection_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        protected override void OnClose()
        {
            lvStationSelection.SmallImageList = null;

            base.OnClose();
        }
    }
}
