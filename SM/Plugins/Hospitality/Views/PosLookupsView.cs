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
    public partial class PosLookupsView : ViewBase
    {
        RecordIdentifier selectedID = "";

        public PosLookupsView(RecordIdentifier posLookupID)
            : base()
        {
            selectedID = posLookupID;
        }

        public PosLookupsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;


            lvPosLookups.Columns[0].Tag = PosLookupSorting.LookupID;
            lvPosLookups.Columns[1].Tag = PosLookupSorting.Description;

            HeaderText = Properties.Resources.HospitalityMenuGroups;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageMenuGroups);

            lvPosLookups.SmallImageList = PluginEntry.Framework.GetImageList();
            lvPosLookups.ContextMenuStrip = new ContextMenuStrip();
            lvPosLookups.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            
            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageMenuGroups);

            lvPosLookups.SortColumn = 1;
            lvPosLookups.SortedBackwards = false;
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvPosLookups.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("RestaurantStationsList", lvPosLookups.ContextMenuStrip, lvPosLookups);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PosLookups", RecordIdentifier.Empty, Properties.Resources.HospitalityMenuGroups, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.HospitalityMenuGroups;
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
                case "PosLookup":
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
            List<PosLookup> posLookups;
            ListViewItem listItem;

            lvPosLookups.Items.Clear();

            // Get all restaurant stations
            posLookups = Providers.PosLookupData.GetList(PluginEntry.DataModel, (PosLookupSorting)lvPosLookups.Columns[lvPosLookups.SortColumn].Tag, lvPosLookups.SortedBackwards);

            foreach (PosLookup posLookup in posLookups)
            {
                listItem = new ListViewItem((string)posLookup.ID);
                listItem.SubItems.Add(posLookup.Text);              
                listItem.ImageIndex = -1;

                listItem.Tag = posLookup.ID;

                lvPosLookups.Add(listItem);

                if (selectedID == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvPosLookups.Columns[lvPosLookups.SortColumn].ImageIndex = (lvPosLookups.SortedBackwards ? 1 : 0);

            lvPosLookups_SelectedIndexChanged(this, EventArgs.Empty);

            lvPosLookups.BestFitColumns();
        }

        private void lvPosLookups_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = btnsEditAddRemove.RemoveButtonEnabled = lvPosLookups.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageMenuGroups);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewPosLookup(this, EventArgs.Empty);  
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowPosLookup((RecordIdentifier)lvPosLookups.SelectedItems[0].Tag);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeletePosLookup((RecordIdentifier)lvPosLookups.SelectedItems[0].Tag);
        }

        private void lvPosLookups_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // We only want to sort based on the first two columns.
            if (e.Column > 1)
            {
                return;
            }

            if (lvPosLookups.SortColumn == e.Column)
            {
                lvPosLookups.SortedBackwards = !lvPosLookups.SortedBackwards;
            }
            else
            {
                lvPosLookups.SortedBackwards = false;
            }



            if (lvPosLookups.SortColumn != -1)
            {
                lvPosLookups.Columns[lvPosLookups.SortColumn].ImageIndex = 2;
                lvPosLookups.SortColumn = e.Column;

            }

            LoadItems();
        }

        private void lvPosLookups_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        protected override void OnClose()
        {
            lvPosLookups.SmallImageList = null;

            base.OnClose();
        }
    }
}
