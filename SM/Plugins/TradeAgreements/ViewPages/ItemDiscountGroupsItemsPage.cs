using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.TradeAgreements.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    public partial class ItemDiscountGroupsItemsPage : UserControl, ITabView
    {
        List<ItemInPriceDiscountGroup> items;
        private WeakReference itemViewer;

        WeakReference owner;
        PriceDiscGroupEnum displayType;
        RecordIdentifier selectedID = "";
        RecordIdentifier groupID = "";

        public ItemDiscountGroupsItemsPage()
        {
            InitializeComponent();

            displayType = PriceDiscGroupEnum.PriceGroup;

            lvItemsList.ContextMenuStrip = new ContextMenuStrip();
            lvItemsList.ContextMenuStrip.Opening += lvItemsList_Opening;
            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            itemDataScroll.Reset();

            var plugin = PluginEntry.Framework.FindImplementor(this, "CanViewRetailItem", null);
            itemViewer = plugin != null ? new WeakReference(plugin) : null;

            btnsContextButtonsItems.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ItemsEdit);

        }

        public ItemDiscountGroupsItemsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.ItemDiscountGroupsItemsPage((TabControl)sender);
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PriceDiscountGroups", 0, Properties.Resources.PriceDiscountGroups, false));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            groupID = (RecordIdentifier)context;
            displayType = (PriceDiscGroupEnum)internalContext;
            
            LoadLines();
        }

        public void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "RetailItem":
                    selectedID = changeIdentifier;
                    LoadLines();
                    break;
            }
        }

        public bool DataIsModified()
        {
            // Here our sheet is supposed to figure out if something needs to be saved and return
            // true if something needs to be saved, else false.
            return false;
        }

        public  bool SaveData()
        {
            // Here we would let our sheet save our data.

            // We return true since saving was successful, if we would return false then
            // the viewstack will prevent other sheet from getting shown.
            return true;
        }

        private void LoadLines()
        {
            lvItemsList.ClearRows();

            var groupType = displayType;
            string itemGroup = groupID.SecondaryID.SecondaryID.ToString();
            int count = 0;
            items = Providers.ItemInPriceDiscountGroupData
                .GetItemList(PluginEntry.DataModel,
                    groupType,
                    itemGroup,
                    itemDataScroll.StartRecord, 
                    itemDataScroll.EndRecord + 1, out count);

            itemDataScroll.RefreshState(items,count);

            Row row;
            foreach (var item in items)
            {
                row = new Row();
                row.AddText((string)item.ID);
                row.AddText(item.Text);
                row.AddText((item.VariantName == "") ? "" : item.VariantName);
                row.Tag = item.ID;

                lvItemsList.AddRow(row);

            }
            lvItemsList_SelectionChanged(this, EventArgs.Empty);
            lvItemsList.AutoSizeColumns();
        }


        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            LoadLines();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            Providers.ItemInPriceDiscountGroupData.
                RemoveItemFromGroup(PluginEntry.DataModel, selectedID, displayType);
            LoadLines();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            string groupId = groupID.SecondaryID.SecondaryID.ToString();
            var dlg = new ItemSearchDialog(PluginEntry.DataModel, PluginEntry.Framework, (int)displayType, groupId, true);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var itemDiscountProvider = Providers.ItemInPriceDiscountGroupData;
                foreach (RecordIdentifier itemId in dlg.GetItemIDs)
                {
                    itemDiscountProvider.AddItemToGroup(PluginEntry.DataModel,
                        itemId,
                        displayType,
                        groupId);
                }
                LoadLines();
            }
        }

        private void lvItemsList_DoubleClick(object sender, EventArgs e)
        {
            btnViewItem_Click(this, null);
        }

        private void btnViewItem_Click(object sender, EventArgs e)
        {
            if (itemViewer != null && itemViewer.IsAlive)
            {
                var list = new List<IDataEntity>();

                foreach (var item in items)
                {
                    list.Add(new DataEntity(item.ID.PrimaryID, ""));
                }

                ((IPlugin) itemViewer.Target).Message(this, "ViewItem",
                    new object[] {((RecordIdentifier) lvItemsList.Row(lvItemsList.Selection.FirstSelectedRow).Tag).PrimaryID,list});            
            }
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }
        void lvItemsList_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvItemsList.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
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

            if ((itemViewer != null && itemViewer.IsAlive))
            {
                menu.Items.Add(new ExtendedMenuItem("-", 400));

                // We can optionally add our own items right here
                item = new ExtendedMenuItem(
                        Properties.Resources.ViewItem + "...",
                        500,
                        lvItemsList_DoubleClick)
                {
                    Default = true,
                    Enabled = lvItemsList.Selection.Count > 0
                };
            }

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ItemsInPriceDiscGroupList", lvItemsList.ContextMenuStrip, lvItemsList);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvItemsList_SelectionChanged(object sender, EventArgs e)
        {
            if (lvItemsList.Selection.Count > 0)
            {
                selectedID = ((RecordIdentifier)lvItemsList.Row(lvItemsList.Selection.FirstSelectedRow).Tag);
                    
            }
            btnsContextButtonsItems.RemoveButtonEnabled = btnViewItem.Enabled =
                (lvItemsList.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(Permission.ItemsEdit);
        }

        private void lvItemsList_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            btnViewItem_Click(this, null);
        }
    }
}
