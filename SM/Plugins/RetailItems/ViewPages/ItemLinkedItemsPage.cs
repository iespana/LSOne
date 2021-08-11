using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    public partial class ItemLinkedItemsPage : UserControl, ITabView
    {
        RetailItem retailItem;
        WeakReference owner;
        bool hasPermission;
        RecordIdentifier selectedLinkedItemID;

        public ItemLinkedItemsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);

            hasPermission = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageLinkedItems);

            if (((ViewBase)owner.Parent.Parent).ReadOnly)
            {
                lvLinkedItems.Enabled = false;
            }


            lvLinkedItems.ContextMenuStrip = new ContextMenuStrip();
            lvLinkedItems.ContextMenuStrip.Opening += lvLinkedItems_ContextMenuStripOpening;

            btns.AddButtonEnabled = hasPermission;
            lvLinkedItems.Enabled = hasPermission;
        }

        public ItemLinkedItemsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ItemLinkedItemsPage((TabControl)sender);
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("LinkedItems", retailItem.ID, Properties.Resources.LinkedItems, false));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            retailItem = (RetailItem)internalContext;

            LoadItems();
        }

        public bool SaveData()
        {
            return true;
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(objectName == "RetailItem")
            {
                LoadItems();
            }
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void LoadItems()
        {
            lvLinkedItems.ClearRows();

            List<LinkedItem> linkedItems = Providers.LinkedItemData.GetLinkedItems(
                PluginEntry.DataModel,
                retailItem.ID,
                LinkedItem.SortEnum.LinkedItemID,
                false );
            Row row;
            foreach (LinkedItem linkedItem in linkedItems)
            {
                row = new Row();
                row.AddText(linkedItem.LinkedItemDescription);
                row.AddText(linkedItem.LinkedItemVariantDescription);
                row.AddText(linkedItem.LinkedItemUnitDescription);
                row.AddText(linkedItem.Blocked ? Properties.Resources.Yes : Properties.Resources.No);
                row.AddCell(new NumericCell(linkedItem.FormattedLinkedItemQuantity, linkedItem.LinkedItemQuantity));
                row.Tag = linkedItem;

                lvLinkedItems.AddRow(row);
            }

            lvLinkedItems.AutoSizeColumns();
        }

        void lvLinkedItems_ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            lvLinkedItems.ContextMenuStrip.Items.Clear();

            ExtendedMenuItem item;

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.Edit + "...",
                    100,
                    btns_EditButtonClicked);

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btns.EditButtonEnabled;
            item.Default = true;
            lvLinkedItems.ContextMenuStrip.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    200,
                    btns_AddButtonClicked);

            item.Enabled = btns.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            lvLinkedItems.ContextMenuStrip.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    btns_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btns.RemoveButtonEnabled;

            lvLinkedItems.ContextMenuStrip.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("LinkedItems", lvLinkedItems.ContextMenuStrip, lvLinkedItems);

            e.Cancel = (lvLinkedItems.ContextMenuStrip.Items.Count == 0);
        }

        private void btns_AddButtonClicked(object sender, EventArgs e)
        {
            Dialogs.LinkedItemsDialog dlg = new Dialogs.LinkedItemsDialog(retailItem);
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedLinkedItemID = dlg.LinkedItemID;
                LoadItems();
            }
        }

        private void btns_EditButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier linkedItemID = ((LinkedItem)lvLinkedItems.Row(lvLinkedItems.Selection.FirstSelectedRow).Tag).ID;

            Dialogs.LinkedItemsDialog dlg = new Dialogs.LinkedItemsDialog(linkedItemID, retailItem);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void btns_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Properties.Resources.DeleteLinkedItemsQuestion, Properties.Resources.DeleteLinkedItems) == DialogResult.Yes)
            {
                for (int i = 0; i < lvLinkedItems.Selection.Count; i++)
                {
                    Providers.LinkedItemData.Delete(PluginEntry.DataModel, ((LinkedItem)lvLinkedItems.Selection[i].Tag).ID);
                }
                LoadItems();
            }
        }

        private void lvLinkedItems_SelectionChanged(object sender, EventArgs e)
        {
            if (lvLinkedItems.Selection.Count > 0)
            {
                btns.EditButtonEnabled = btns.RemoveButtonEnabled = hasPermission;
            }
            else
            {
                btns.EditButtonEnabled = btns.RemoveButtonEnabled = false;
            }
        }

        private void lvLinkedItems_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btns.EditButtonEnabled)
            {
                btns_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}
