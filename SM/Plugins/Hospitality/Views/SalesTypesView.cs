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
    public partial class SalesTypesView : ViewBase
    {
        RecordIdentifier selectedID = "";

        public SalesTypesView(RecordIdentifier salesTypeId)
            : base()
        {
            selectedID = salesTypeId;
        }

        public SalesTypesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;


            lvSalesTypes.Columns[0].Tag = SalesTypeSorting.SalesTypeId;
            lvSalesTypes.Columns[1].Tag = SalesTypeSorting.SalesTypeDescription;

            HeaderText = Properties.Resources.SalesTypes;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageSalesTypes);

            lvSalesTypes.SmallImageList = PluginEntry.Framework.GetImageList();
            lvSalesTypes.ContextMenuStrip = new ContextMenuStrip();
            lvSalesTypes.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            
            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageSalesTypes);

            lvSalesTypes.SortColumn = 1;
            lvSalesTypes.SortedBackwards = false;
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvSalesTypes.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("SalesTypesList", lvSalesTypes.ContextMenuStrip, lvSalesTypes);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("SalesTypes", 0, Properties.Resources.SalesTypes, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.SalesTypes;
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
                case "SalesType":
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
            List<DataEntity> salesTypes;
            ListViewItem listItem;

            lvSalesTypes.Items.Clear();

            // Get all sales types
            salesTypes = Providers.SalesTypeData.GetList(PluginEntry.DataModel, (SalesTypeSorting)lvSalesTypes.Columns[lvSalesTypes.SortColumn].Tag, lvSalesTypes.SortedBackwards);

            foreach (var salesType in salesTypes)
            {
                listItem = new ListViewItem((string)salesType.ID);
                listItem.SubItems.Add(salesType.Text);
                listItem.ImageIndex = -1;

                listItem.Tag = salesType.ID;

                lvSalesTypes.Add(listItem);

                if (selectedID == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvSalesTypes.Columns[lvSalesTypes.SortColumn].ImageIndex = (lvSalesTypes.SortedBackwards ? 1 : 0);

            lvSalesTypes_SelectedIndexChanged(this, EventArgs.Empty);

            lvSalesTypes.BestFitColumns();
        }

        private void lvSalesTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = btnsEditAddRemove.RemoveButtonEnabled = lvSalesTypes.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageSalesTypes);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewSalesType(this, EventArgs.Empty);  
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowSalesType((RecordIdentifier)lvSalesTypes.SelectedItems[0].Tag);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteSalesType((RecordIdentifier)lvSalesTypes.SelectedItems[0].Tag);
        }

        private void lvSalesTypes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // We only want to sort based on the first two columns.
            if (e.Column > 1)
            {
                return;
            }

            if (lvSalesTypes.SortColumn == e.Column)
            {
                lvSalesTypes.SortedBackwards = !lvSalesTypes.SortedBackwards;
            }
            else
            {
                lvSalesTypes.SortedBackwards = false;
            }



            if (lvSalesTypes.SortColumn != -1)
            {
                lvSalesTypes.Columns[lvSalesTypes.SortColumn].ImageIndex = 2;
                lvSalesTypes.SortColumn = e.Column;

            }

            LoadItems();
        }

        private void lvSalesTypes_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        protected override void OnClose()
        {
            lvSalesTypes.SmallImageList = null;

            base.OnClose();
        }
    }
}
