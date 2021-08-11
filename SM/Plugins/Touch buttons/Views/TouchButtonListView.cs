using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.TouchButtons.Views
{
    public partial class TouchButtonListView : ViewBase
    {
        RecordIdentifier selectedId;

        public TouchButtonListView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit | ViewAttributes.Help | ViewAttributes.Close | ViewAttributes.ContextBar;

            lvLayouts.SmallImageList = PluginEntry.Framework.GetImageList();
            lvLayouts.ContextMenuStrip = new ContextMenuStrip();
            lvLayouts.ContextMenuStrip.Opening += lvActions_Opening;
            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("AllTouchButtonLayouts", RecordIdentifier.Empty, Properties.Resources.TouchButtonLayouts, false));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (lvLayouts != null)
            {
                lvLayouts.Columns[0].Width = 100;
                lvLayouts.Columns[1].Width = lvLayouts.Width - 100 - 35;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.TouchButtonLayouts;
            }
        }       

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty; 
	        }
        }

        public string Description
        {
            get { return Properties.Resources.TouchButtonLayouts; }
        }

        protected override void LoadData(bool isRevert)
        {
            HeaderText = Description;
            //HeaderIcon = Properties.Resources.Profiles16;

            LoadLayouts(0,false);
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }       

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "DefaultData":
                case "TouchButtonLayout":
                    LoadLayouts(lvLayouts.SortColumn, lvLayouts.SortedBackwards);
                    break;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowTouchButtonSheet((RecordIdentifier)lvLayouts.SelectedItems[0].Tag);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewTouchButtonLayout();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvLayouts.SelectedItems.Count == 1)
            {
                PluginOperations.DeleteTouchButtonLayout((RecordIdentifier)lvLayouts.SelectedItems[0].Tag);
            }
            else
            {
                PluginOperations.DeleteTouchButtonLayouts(lvLayouts.GetSelectedIDs());
            }
        }

        void lvActions_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvLayouts.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                    Properties.Resources.Import,
                    500,
                    btnImport_Click);

            item.Enabled = btnImport.Enabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Export,
                    600,
                    btnExport_Click);

            item.Enabled = btnExport.Enabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.ButtonGridMenus,
                    700,
                    btnButtonGridMenus_Click);

            item.Enabled = btnButtonGridMenus.Enabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("TouchButtonLayoutList", lvLayouts.ContextMenuStrip, lvLayouts);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void LoadLayouts(int sortBy, bool backwards)
        {
            ListViewItem item;
            List<TouchLayout> layouts;

            layouts = Providers.TouchLayoutData.GetTouchLayouts(PluginEntry.DataModel, (TouchLayoutSorting)sortBy, backwards);

            lvLayouts.Items.Clear();

            foreach (var layout in layouts)
            {
                item = new ListViewItem(layout.ID.ToString());
                item.SubItems.Add(layout.Text);
                if (layout.ImportDateTime != null)
                {
                    item.SubItems.Add(layout.ImportDateTime.ToString());
                }
                item.Tag = layout.ID;
                item.ImageIndex = -1;

                lvLayouts.Add(item);
            }

            lvLayouts.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvLayouts.SortColumn = sortBy;
            lvActions_SelectedIndexChanged(this, EventArgs.Empty);
            lvLayouts.BestFitColumns();
        }

        private void lvActions_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvLayouts.SortColumn == e.Column)
            {
                lvLayouts.SortedBackwards = !lvLayouts.SortedBackwards;
            }
            else
            {
                if (lvLayouts.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvLayouts.Columns[lvLayouts.SortColumn].ImageIndex = 2;
                }

                lvLayouts.SortedBackwards = false;
            }

            LoadLayouts(e.Column, lvLayouts.SortedBackwards);
        }

        private void lvActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedId = (lvLayouts.SelectedItems.Count > 0) ? (RecordIdentifier)lvLayouts.SelectedItems[0].Tag : RecordIdentifier.Empty;
            btnsContextButtons.EditButtonEnabled = (lvLayouts.SelectedItems.Count == 1) && PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout);
            btnsContextButtons.RemoveButtonEnabled = (lvLayouts.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout);
            btnButtonGridMenus.Enabled = btnsContextButtons.EditButtonEnabled;
            btnExport.Enabled = (lvLayouts.SelectedItems.Count != 0);
        }

        private void lvActions_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        protected override void OnClose()
        {
            lvLayouts.SmallImageList = null;

            base.OnClose();
        }

        private void btnButtonGridMenus_Click(object sender, EventArgs e)
        {
            PluginOperations.EditTouchLayoutButtonGrids(selectedId);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            List<RecordIdentifier> layoutIds = new List<RecordIdentifier>();

            for(int i = 0; i < lvLayouts.SelectedItems.Count; i++)
            {
                layoutIds.Add((RecordIdentifier)lvLayouts.SelectedItems[i].Tag);
            }
            
            PluginOperations.ExportLayouts(layoutIds);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            PluginOperations.ImportLayouts();
        }
    }
}
