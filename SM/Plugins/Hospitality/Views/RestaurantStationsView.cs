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
    public partial class RestaurantStationsView : ViewBase
    {
        RecordIdentifier selectedID = "";
        private PrintingStation.StationTypeEnum stationType;

        public RestaurantStationsView(RecordIdentifier restaurantStationID, PrintingStation.StationTypeEnum stationType)
            : this(stationType)
        {
            selectedID = restaurantStationID;
        }

        public RestaurantStationsView(PrintingStation.StationTypeEnum stationType)
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            this.stationType = stationType;
            HeaderText = Properties.Resources.PrintingStations;

            lvRestaurantStations.Columns[0].Tag = PrintingStationSorting.PrintingStationId;
            lvRestaurantStations.Columns[1].Tag = PrintingStationSorting.PrintingtStationName;
            

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting);

            lvRestaurantStations.SmallImageList = PluginEntry.Framework.GetImageList();
            lvRestaurantStations.ContextMenuStrip = new ContextMenuStrip();
            lvRestaurantStations.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            
            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting);

            lvRestaurantStations.SortColumn = 1;
            lvRestaurantStations.SortedBackwards = false;
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvRestaurantStations.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsEditAddRemove_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   btnsEditAddRemove_AddButtonClicked);

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsEditAddRemove_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("RestaurantStationsList", lvRestaurantStations.ContextMenuStrip, lvRestaurantStations);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("RestaurantStations", RecordIdentifier.Empty, Properties.Resources.RestaurantStations, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.PrintingStations;
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
            LoadPrintingStations();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "RestaurantStation":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedID = changeIdentifier;
                    }

                    LoadPrintingStations();
                    break;
            }

        }

        private void LoadPrintingStations()
        {
            List<PrintingStation> printingStations;
            ListViewItem listItem;

            lvRestaurantStations.Items.Clear();
            printingStations = Providers.PrintingStationData.GetList(
                PluginEntry.DataModel, 
                (PrintingStationSorting)lvRestaurantStations.Columns[lvRestaurantStations.SortColumn].Tag, 
                lvRestaurantStations.SortedBackwards);

            foreach (var printingStation in printingStations)
            {
                listItem = new ListViewItem((string)printingStation.ID);
                listItem.SubItems.Add(printingStation.Text);

                string stationTypeString = "";

                switch (printingStation.StationType)
                {
                    case PrintingStation.StationTypeEnum.HardwareProfilePrinter:
                        stationTypeString = Properties.Resources.HardwareProfilePrinter;
                        break;
                    case PrintingStation.StationTypeEnum.OPOSPrinter:
                        stationTypeString = Properties.Resources.OPOSPrinter;
                        break;
                    case PrintingStation.StationTypeEnum.WindowsPrinter:
                        stationTypeString = Properties.Resources.WindowsPrinter;
                        break;
                }


                listItem.SubItems.Add(stationTypeString);
                

                listItem.ImageIndex = -1;

                listItem.Tag = printingStation.ID;

                lvRestaurantStations.Add(listItem);

                if (selectedID == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvRestaurantStations.Columns[lvRestaurantStations.SortColumn].ImageIndex = (lvRestaurantStations.SortedBackwards ? 1 : 0);

            lvRestaurantStations_SelectedIndexChanged(this, EventArgs.Empty);

            lvRestaurantStations.BestFitColumns();
        }

        private void lvRestaurantStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = btnsEditAddRemove.RemoveButtonEnabled = lvRestaurantStations.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.CreatePrintingStation();
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            var stationId = (RecordIdentifier) lvRestaurantStations.SelectedItems[0].Tag;

            PluginOperations.ShowPrintingStation(stationId);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteRestaurantStation((RecordIdentifier)lvRestaurantStations.SelectedItems[0].Tag);
        }

        private void lvRestaurantStations_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // We only want to sort based on the first two columns.
            if (e.Column > 1)
            {
                return;
            }

            if (lvRestaurantStations.SortColumn == e.Column)
            {
                lvRestaurantStations.SortedBackwards = !lvRestaurantStations.SortedBackwards;
            }
            else
            {
                lvRestaurantStations.SortedBackwards = false;
            }

            if (lvRestaurantStations.SortColumn != -1)
            {
                lvRestaurantStations.Columns[lvRestaurantStations.SortColumn].ImageIndex = 2;
                lvRestaurantStations.SortColumn = e.Column;

            }

            LoadPrintingStations();
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
            lvRestaurantStations.SmallImageList = null;

            base.OnClose();
        }

        
    }
}
