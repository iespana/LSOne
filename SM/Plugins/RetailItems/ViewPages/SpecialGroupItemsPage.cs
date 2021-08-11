using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.RetailItems.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    public partial class SpecialGroupItemsPage : UserControl, ITabView
    {
        RecordIdentifier selectedID = "";
        List<ItemInGroup> items;

        public SpecialGroupItemsPage()
        {
            InitializeComponent();

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            itemDataScroll.Reset();

            lvItemsList.ContextMenuStrip = new ContextMenuStrip();
            lvItemsList.ContextMenuStrip.Opening += lvItemsList_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new SpecialGroupItemsPage();
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            selectedID = context;
            LoadLines();
        }

        private void LoadLines()
        {
            if (selectedID == RecordIdentifier.Empty)
            {
                return;
            }
            
            lvItemsList.ClearRows();

            items = Providers.SpecialGroupData.ItemsInSpecialGroup(PluginEntry.DataModel,
                        selectedID,
                        itemDataScroll.StartRecord,
                        itemDataScroll.EndRecord + 1,
                        (SpecialGroupItemSorting)lvItemsList.Columns.IndexOf(lvItemsList.SortColumn),
                        lvItemsList.SortedAscending);

            var count = Providers.SpecialGroupData.GetItemsInSpecialGroupCount(PluginEntry.DataModel, selectedID);

            itemDataScroll.RefreshState(items, count);

            foreach (var item in items)
            {
                var row = new Row();

                row.AddText((string)item.ID);
                row.AddText(item.Text);
                row.AddText(item.VariantName);

                row.Tag = item.ID;

                lvItemsList.AddRow(row);
            }

            lvItemList_SelectedIndexChanged(this, EventArgs.Empty);
            
            lvItemsList.AutoSizeColumns();
        }

        public void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "SpecialGroup":
                    //selectedID = changeIdentifier;
                    LoadLines();
                    break;
            }
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveUserInterface()
        {
        }

        void lvItemsList_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvItemsList.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit + "...",
                    200,
                    btnEditItem_Click)
                {
                    Enabled = btnsContextButtonsItems.EditButtonEnabled,
                    Image = ContextButtons.GetEditButtonImage(),
                    Default = true
                };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    200,
                    btnAddItem_Click)
                {
                    Enabled = btnsContextButtonsItems.AddButtonEnabled,
                    Image = ContextButtons.GetAddButtonImage()
                };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    btnRemoveItem_Click)
                {
                    Enabled = btnsContextButtonsItems.RemoveButtonEnabled,
                    Image = ContextButtons.GetRemoveButtonImage()
                };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ItemsInSpecialGroupList", lvItemsList.ContextMenuStrip, lvItemsList);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvItemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtonsItems.RemoveButtonEnabled = btnsContextButtonsItems.EditButtonEnabled = 
                (lvItemsList.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManageSpecialGroups);
        }

        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            LoadLines();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            Providers.SpecialGroupData.RemoveItemFromSpecialGroup(PluginEntry.DataModel, 
                                                                        (RecordIdentifier)lvItemsList.Selection[0].Tag, 
                                                                        selectedID);
            LoadLines();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            var dlg = new ItemNotInGroupSearchDialog(PluginEntry.DataModel, PluginEntry.Framework, selectedID, ItemNotInGroupSearchDialog.GroupEnum.SpecialGroup);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                List<RecordIdentifier> itemIDs = dlg.GetItems;

                Providers.SpecialGroupData.AddItemsToSpecialGroup(PluginEntry.DataModel, itemIDs, selectedID);
                LoadLines();
            }
        }

        private void btnEditItem_Click(object sender, EventArgs e)
        {
            RecordIdentifier itemID = ((RecordIdentifier)lvItemsList.Selection[0].Tag).PrimaryID;

            PluginOperations.ShowItemSheet(itemID, items);
        }
     
        public void OnClose()
        {
            
        }

        private void lvItemsList_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvItemsList.SortColumn == args.Column)
            {
                lvItemsList.SetSortColumn(args.Column, !lvItemsList.SortedAscending);
            }
            else
            {
                lvItemsList.SetSortColumn(args.Column, true);
            }

            itemDataScroll.Reset();

            LoadLines();
        }

        private void lvItemsList_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtonsItems.EditButtonEnabled)
            {
                btnEditItem_Click(this, null);
            }
        }

        private void lvItemsList_SelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtonsItems.RemoveButtonEnabled = btnsContextButtonsItems.EditButtonEnabled = btnViewItem.Enabled = 
                (lvItemsList.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManageSpecialGroups);
        }

        private void btnViewItem_Click(object sender, EventArgs e)
        {
            btnEditItem_Click(sender, e);
        }
    }
}