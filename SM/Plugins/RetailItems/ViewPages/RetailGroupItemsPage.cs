using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
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
    public partial class RetailGroupItemsPage : UserControl, ITabView
    {
        RecordIdentifier selectedID = "";
        RecordIdentifier groupMasterID = "";
        List<ItemInGroup> items;
        RetailGroup retailGroup;
        WeakReference owner;

        public RetailGroupItemsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public RetailGroupItemsPage()
            : base()
        {
            InitializeComponent();

            lvItemsList.ContextMenuStrip = new ContextMenuStrip();
            lvItemsList.ContextMenuStrip.Opening += lvItemsList_Opening;

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            itemDataScroll.Reset();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.RetailGroupItemsPage((TabControl)sender);
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("RetailGroups", 0, Properties.Resources.RetailGroups, false));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            groupMasterID = context;
            retailGroup = (RetailGroup)internalContext;
            selectedID = retailGroup.ID;
            LoadLines();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "RetailGroup":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedID = changeIdentifier;
                    }

                    LoadLines();
                    break;
            }
        }

        private void LoadLines()
        {
            lvItemsList.ClearRows();

            if (!RecordIdentifier.IsEmptyOrNull(selectedID))
            {
                items = Providers.RetailGroupData.ItemsInGroup(PluginEntry.DataModel,
                    selectedID,
                    itemDataScroll.StartRecord,
                    itemDataScroll.EndRecord + 1);

                itemDataScroll.RefreshState(items);

                foreach (var item in items)
                {
                   

                    var row = new Row();
                    row.AddText((string)item.ID);

                    row.AddText(item.Text);
                    row.AddText(item.VariantName);

                    row.Tag = item.ID;

                    lvItemsList.AddRow(row);
                }
                
                lvItemsList_SelectionChanged(this, EventArgs.Empty);

                lvItemsList.AutoSizeColumns();
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

        void lvItemsList_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItemsList.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here


            item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    100,
                    btnAddItem_Click)
                {
                    Enabled = btnsContextButtonsItems.AddButtonEnabled,
                    Image = ContextButtons.GetAddButtonImage()
                };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    200,
                    btnRemoveItem_Click)
                {
                    Enabled = btnsContextButtonsItems.RemoveButtonEnabled,
                    Image = ContextButtons.GetRemoveButtonImage()
                };
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 250));

            item = new ExtendedMenuItem(
                Properties.Resources.ViewItem + "...",
                300,
                btnEditItem_Click)
                    {
                        Enabled = btnsContextButtonsItems.EditButtonEnabled,
                        Image = ContextButtons.GetEditButtonImage()
                    };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ItemsInRetailGroupList", lvItemsList.ContextMenuStrip, lvItemsList);

            e.Cancel = (menu.Items.Count == 0);
        }
        
        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            LoadLines();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            Providers.RetailGroupData.RemoveItemFromRetailGroup(PluginEntry.DataModel, (RecordIdentifier)lvItemsList.Selection[0].Tag);
            LoadLines();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            var dlg = new ItemNotInGroupSearchDialog(PluginEntry.DataModel, PluginEntry.Framework, (string)groupMasterID, ItemNotInGroupSearchDialog.GroupEnum.RetailGroup);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                List<RecordIdentifier> itemIDs = dlg.GetItems;

                Providers.RetailGroupData.AddItemsToRetailGroup(PluginEntry.DataModel,
                                                                 itemIDs,
                                                                 selectedID);
                LoadLines();
            }
        }

        private void btnEditItem_Click(object sender, EventArgs e)
        {
            RecordIdentifier itemID = (RecordIdentifier)lvItemsList.Selection[0].Tag;

            PluginOperations.ShowItemSheet(itemID, items.Cast<IDataEntity>());
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
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
               (lvItemsList.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageSpecialGroups);
        }

        private void btnViewItem_Click(object sender, EventArgs e)
        {
            btnEditItem_Click(sender, e);
        }
    }
}
