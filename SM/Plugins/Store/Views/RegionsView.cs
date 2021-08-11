using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.ViewCore;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.Controls.Rows;
using LSOne.ViewCore.EventArguments;
using LSOne.Controls;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.Store.Views
{
    public partial class RegionsView : ViewBase
    {
        private RecordIdentifier selectedID = RecordIdentifier.Empty;

        public RegionsView()
        {
            InitializeComponent();
            HeaderText = Properties.Resources.Regions;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.StoreEdit);
            contextBtnsStores.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.StoreEdit) && selectedID != RecordIdentifier.Empty;

            lvRegions.ContextMenuStrip = new ContextMenuStrip();
            lvRegions.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            lvStores.ContextMenuStrip = new ContextMenuStrip();
            lvStores.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Stores_Opening);

            lvRegions.SetSortColumn(0, true);
            lvStores.SetSortColumn(0, true);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.StoreEdit);
        }

        private void ContextMenuStrip_Stores_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvStores.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    100,
                    new EventHandler(contextBtnsStores_AddButtonClicked))
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = contextBtnsStores.AddButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    200,
                    new EventHandler(contextBtnsStores_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = contextBtnsStores.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("RegionStoresList", lvStores.ContextMenuStrip, lvStores);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvRegions.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
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

            PluginEntry.Framework.ContextMenuNotify("RegionsList", lvRegions.ContextMenuStrip, lvRegions);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Regions;
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
                case "Region":
                case "Store":
                    LoadItems();
                    break;
            }
        }

        private void LoadItems()
        {
            DataLayer.BusinessObjects.StoreManagement.Region.SortEnum sortBy = 
                lvRegions.Columns.IndexOf(lvRegions.SortColumn) == 0 
                ? DataLayer.BusinessObjects.StoreManagement.Region.SortEnum.ID 
                : DataLayer.BusinessObjects.StoreManagement.Region.SortEnum.Description;

            List<DataLayer.BusinessObjects.StoreManagement.Region> regions = Providers.RegionData.GetList(PluginEntry.DataModel, sortBy, !lvRegions.SortedAscending);

            RecordIdentifier selectedIDTemp = selectedID;
            lvRegions.ClearRows();
            selectedID = selectedIDTemp;

            foreach (var region in regions)
            {
                Row row = new Row();
                row.AddText((string)region.ID);
                row.AddText(region.Text);

                row.Tag = region;

                lvRegions.AddRow(row);

                if (selectedID == region.ID)
                {
                    lvRegions.Selection.Set(lvRegions.RowCount - 1);
                }
            }

            lvRegions_SelectionChanged(this, EventArgs.Empty);
            lvRegions.AutoSizeColumns(true);
        }

        private void LoadRegionStores()
        {
            lvStores.ClearRows();

            if(selectedID != RecordIdentifier.Empty)
            {
                DataLayer.BusinessObjects.StoreManagement.Region.SortEnum sortBy =
                lvStores.Columns.IndexOf(lvStores.SortColumn) == 0
                ? DataLayer.BusinessObjects.StoreManagement.Region.SortEnum.ID
                : DataLayer.BusinessObjects.StoreManagement.Region.SortEnum.Description;

                List<DataEntity> stores = Providers.RegionData.GetStoresByRegion(PluginEntry.DataModel, selectedID, sortBy, !lvStores.SortedAscending);

                foreach (var store in stores)
                {
                    Row row = new Row();
                    row.AddText((string)store.ID);
                    row.AddText(store.Text);

                    row.Tag = store.ID;

                    lvStores.AddRow(row);
                }
            }

            lvStores_SelectionChanged(this, EventArgs.Empty);
            lvStores.AutoSizeColumns(true);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.EditRegion(RecordIdentifier.Empty);
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.EditRegion(((DataLayer.BusinessObjects.StoreManagement.Region)lvRegions.Selection[0].Tag).ID);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if(lvStores.Rows.Count > 0)
            {
                MessageDialog.Show(Properties.Resources.RegionInUse);
                return;
            }

            if(QuestionDialog.Show(Properties.Resources.RemoveRegionQuestion, Properties.Resources.RemoveRegionQuestionCaption) == DialogResult.Yes)
            {
                Providers.RegionData.Delete(PluginEntry.DataModel, ((DataLayer.BusinessObjects.StoreManagement.Region)lvRegions.Selection[0].Tag).ID);
                LoadItems();
            }
        }

        private void lvRegions_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = lvRegions.Selection.Count == 1 ? ((DataLayer.BusinessObjects.StoreManagement.Region)lvRegions.Selection[0].Tag).ID : RecordIdentifier.Empty;

            btnsEditAddRemove.EditButtonEnabled =
            btnsEditAddRemove.RemoveButtonEnabled = 
            contextBtnsStores.AddButtonEnabled = selectedID != RecordIdentifier.Empty && PluginEntry.DataModel.HasPermission(Permission.StoreEdit);

            LoadRegionStores();
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if(arguments.CategoryKey == this.GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.Stores, PluginOperations.ShowStoresListView), 10);
            }
        }
        private void lvStores_SelectionChanged(object sender, EventArgs e)
        {
            contextBtnsStores.RemoveButtonEnabled = lvStores.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.StoreEdit);
        }

        private void contextBtnsStores_RemoveButtonClicked(object sender, EventArgs e)
        {
            if(QuestionDialog.Show(Properties.Resources.RemoveStoresFromRegionQuestion) == DialogResult.Yes)
            {
                for(int i = 0; i < lvStores.Selection.Count; i++)
                {
                    DataLayer.BusinessObjects.StoreManagement.Store store = Providers.StoreData.Get(PluginEntry.DataModel, (RecordIdentifier)lvStores.Selection[i].Tag);
                    store.RegionID = "";
                    Providers.StoreData.Save(PluginEntry.DataModel, store);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "StoreRegion", store.ID, null);
                }

                LoadItems();
            }
        }

        private void contextBtnsStores_AddButtonClicked(object sender, EventArgs e)
        {
            List<RecordIdentifier> excludedStores = new List<RecordIdentifier>();

            for(int i = 0; i < lvStores.RowCount; i++)
            {
                excludedStores.Add((RecordIdentifier)lvStores.Rows[i].Tag);
            }

            Dialogs.AddStoresToRegionDialog dlg = new Dialogs.AddStoresToRegionDialog((DataLayer.BusinessObjects.StoreManagement.Region)lvRegions.Selection[0].Tag, excludedStores);

            if(dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void lvRegions_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvRegions.SortColumn == args.Column)
            {
                lvRegions.SetSortColumn(args.Column, !lvRegions.SortedAscending);
            }
            else
            {
                lvRegions.SetSortColumn(args.Column, true);
            }

            LoadItems();
        }

        private void lvStores_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvStores.SortColumn == args.Column)
            {
                lvStores.SetSortColumn(args.Column, !lvStores.SortedAscending);
            }
            else
            {
                lvStores.SetSortColumn(args.Column, true);
            }

            LoadRegionStores();
        }
    }
}
