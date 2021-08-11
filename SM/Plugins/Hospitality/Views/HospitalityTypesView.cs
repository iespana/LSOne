using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Hospitality.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    public partial class HospitalityTypesView : ViewBase
    {
        private RecordIdentifier selectedID = "";

        public HospitalityTypesView(RecordIdentifier hospitalityTypeID)
            : base()
        {
            this.selectedID = hospitalityTypeID;
        }

        public HospitalityTypesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Properties.Resources.HospitalityTypes;

            lvHospitalityTypes.Columns[0].Tag = HospitalityTypeSorting.Restaurant;
            lvHospitalityTypes.Columns[1].Tag = HospitalityTypeSorting.SalesType;
            lvHospitalityTypes.Columns[2].Tag = HospitalityTypeSorting.Description;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes);

            lvHospitalityTypes.SmallImageList = PluginEntry.Framework.GetImageList();
            lvHospitalityTypes.ContextMenuStrip = new ContextMenuStrip();
            lvHospitalityTypes.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes);

            lvHospitalityTypes.SortColumn = 0;
            lvHospitalityTypes.SortedBackwards = false;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("HospitalityTypes", RecordIdentifier.Empty, Properties.Resources.HospitalityTypes, false));
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvHospitalityTypes.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("SalesTypesList", lvHospitalityTypes.ContextMenuStrip, lvHospitalityTypes);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.HospitalityTypes;
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
                case "HospitalityType":
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
            List<HospitalityTypeListItem> hosptitalityTypeListItems;
            ListViewItem listItem;

            lvHospitalityTypes.Items.Clear();

            // Get all sales types
            hosptitalityTypeListItems = Providers.HospitalityTypeData.GetHospitalityTypes(PluginEntry.DataModel, (HospitalityTypeSorting)lvHospitalityTypes.Columns[lvHospitalityTypes.SortColumn].Tag, lvHospitalityTypes.SortedBackwards);

            foreach (var hospitalityType in hosptitalityTypeListItems)
            {
                listItem = new ListViewItem(hospitalityType.RestaurantName);
                listItem.SubItems.Add(hospitalityType.SalesTypeDescription);
                listItem.SubItems.Add(hospitalityType.Text);
                listItem.ImageIndex = -1;

                listItem.Tag = hospitalityType.ID;

                lvHospitalityTypes.Add(listItem);

                if (selectedID == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvHospitalityTypes.Columns[lvHospitalityTypes.SortColumn].ImageIndex = (lvHospitalityTypes.SortedBackwards ? 1 : 0);

            lvHospitalityTypes_SelectedIndexChanged(this, EventArgs.Empty);

            lvHospitalityTypes.BestFitColumns();
        }

        private void lvHospitalityTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = btnsEditAddRemove.RemoveButtonEnabled = lvHospitalityTypes.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes);         
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewHospitalityType(null, EventArgs.Empty);
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowHospitalityType((RecordIdentifier)lvHospitalityTypes.SelectedItems[0].Tag);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteHospitalityType((RecordIdentifier)lvHospitalityTypes.SelectedItems[0].Tag);
        }

        private void lvHospitalityTypes_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvHospitalityTypes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvHospitalityTypes.SortColumn == e.Column)
            {
                lvHospitalityTypes.SortedBackwards = !lvHospitalityTypes.SortedBackwards;
            }
            else
            {
                lvHospitalityTypes.SortedBackwards = false;
            }

            if (lvHospitalityTypes.SortColumn != -1)
            {
                lvHospitalityTypes.Columns[lvHospitalityTypes.SortColumn].ImageIndex = 2;
                lvHospitalityTypes.SortColumn = e.Column;
            }

            LoadItems();
        }

        protected override void OnClose()
        {
            lvHospitalityTypes.SmallImageList = null;

            base.OnClose();
        }
    }
}
