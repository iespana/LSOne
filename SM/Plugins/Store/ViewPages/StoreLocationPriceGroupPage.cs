using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    internal partial class StoreLocationPriceGroupPage : UserControl, ITabView
    {
        RecordIdentifier storeID;
        private bool priorityChanged;

        public StoreLocationPriceGroupPage()
        {
            InitializeComponent();

            priorityChanged = false;

            lvPriceGroups.ContextMenuStrip = new ContextMenuStrip();
            lvPriceGroups.ContextMenuStrip.Opening += lvPriceGroups_ContextMenuStripOpening;
        }        

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StoreLocationPriceGroupPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            ListViewItem item;
            List<StoreInPriceGroup> priceGroupLines;

            storeID = context;

            bool editButtonsEnabled = PluginEntry.DataModel.HasPermission(Permission.StoreEdit) &&
                (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty || PluginEntry.DataModel.CurrentStoreID == storeID);

            btnsAddRemove.AddButtonEnabled = editButtonsEnabled;
            //btnsAddRemove.RemoveButtonEnabled = editButtonsEnabled;

            lvPriceGroups.SuspendLayout();
            lvPriceGroups.Items.Clear();

            priceGroupLines = Providers.PriceDiscountGroupData.GetPriceGroupsForStore(PluginEntry.DataModel, storeID);

            foreach (StoreInPriceGroup line in priceGroupLines)
            {
                item = new ListViewItem();
                item.Text = (string)line.PriceGroupID;
                item.SubItems.Add(line.PriceGroupName);
                item.SubItems.Add(Convert.ToString(line.Level));
                item.Tag = line;

                lvPriceGroups.Items.Add(item);
            }

            //lvPriceGroups.ResumeLayout();
            lvPriceGroups.BestFitColumns();
        }

        public bool DataIsModified()
        {
            return priorityChanged;
        }

        public bool SaveData()
        {
            // Handle saving into RBOLOCATIONPRICEGROUP table here
            foreach (ListViewItem item in lvPriceGroups.Items)
            {
                StoreInPriceGroup line = (StoreInPriceGroup)item.Tag;

                if (line.Dirty)
                {
                    Providers.PriceDiscountGroupData.UpdateStoreInPriceGroup(PluginEntry.DataModel, line);
                }
            }

            return true;            
        }


        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {

        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvPriceGroups.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void btnsAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            // Display the SelectPriceGroupDialog
            Dialogs.SelectPriceGroupDialog dialog = new Dialogs.SelectPriceGroupDialog(storeID, true);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Get the PriceGroupLine object from the dialog and add it to the view
                StoreInPriceGroup newLine = dialog.CurrentPriceGroupLine;

                lvPriceGroups.SuspendLayout();

                ListViewItem item = new ListViewItem();
                item.Text = (string)newLine.PriceGroupID;
                item.SubItems.Add(newLine.PriceGroupName);
                item.SubItems.Add(Convert.ToString(newLine.Level));
                item.Tag = newLine;

                lvPriceGroups.Items.Add(item);

                lvPriceGroups.ResumeLayout();
            }            
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (lvPriceGroups.Items.Count >= 2)
            {
                int MoveUpItemIndex = lvPriceGroups.FocusedItem.Index;

                // We can't move the topmost item
                if (MoveUpItemIndex == 0)
                    return;

                ListViewItem selectedItem = (ListViewItem)lvPriceGroups.FocusedItem.Clone();
                ListViewItem itemAbove = (ListViewItem)lvPriceGroups.Items[MoveUpItemIndex - 1].Clone();

                lvPriceGroups.SuspendLayout();

                // Adjust the levels
                int temp = ((StoreInPriceGroup)selectedItem.Tag).Level;
                ((StoreInPriceGroup)selectedItem.Tag).Level = ((StoreInPriceGroup)itemAbove.Tag).Level;
                ((StoreInPriceGroup)itemAbove.Tag).Level = temp;

                // Mark the lines as dirty
                ((StoreInPriceGroup)selectedItem.Tag).Dirty = true;
                ((StoreInPriceGroup)itemAbove.Tag).Dirty = true;

                selectedItem.SubItems[2].Text = Convert.ToString(((StoreInPriceGroup)selectedItem.Tag).Level);
                itemAbove.SubItems[2].Text = Convert.ToString(((StoreInPriceGroup)itemAbove.Tag).Level);

                lvPriceGroups.Items[MoveUpItemIndex - 1] = selectedItem;
                lvPriceGroups.Items[MoveUpItemIndex] = itemAbove;                

                priorityChanged = true;

                lvPriceGroups.Items[MoveUpItemIndex].Selected = false;
                lvPriceGroups.Items[MoveUpItemIndex - 1].Selected = true;
                lvPriceGroups.FocusedItem = lvPriceGroups.Items[MoveUpItemIndex - 1];
                lvPriceGroups.Select();

                lvPriceGroups.ResumeLayout();

            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (lvPriceGroups.Items.Count >= 2)
            {
                int MoveDownItemIndex = lvPriceGroups.FocusedItem.Index;

                // We can't move the lovest item
                if (MoveDownItemIndex == lvPriceGroups.Items.Count - 1)
                    return;

                ListViewItem selectedItem = (ListViewItem)lvPriceGroups.FocusedItem.Clone();
                ListViewItem itemBelow = (ListViewItem)lvPriceGroups.Items[MoveDownItemIndex + 1].Clone();

                lvPriceGroups.SuspendLayout();

                // Adjust the levels
                int temp = ((StoreInPriceGroup)selectedItem.Tag).Level;
                ((StoreInPriceGroup)selectedItem.Tag).Level = ((StoreInPriceGroup)itemBelow.Tag).Level;
                ((StoreInPriceGroup)itemBelow.Tag).Level = temp;

                // Mark the lines as dirty
                ((StoreInPriceGroup)selectedItem.Tag).Dirty = true;
                ((StoreInPriceGroup)itemBelow.Tag).Dirty = true;

                selectedItem.SubItems[2].Text = Convert.ToString(((StoreInPriceGroup)selectedItem.Tag).Level);
                itemBelow.SubItems[2].Text = Convert.ToString(((StoreInPriceGroup)itemBelow.Tag).Level);

                lvPriceGroups.Items[MoveDownItemIndex + 1] = selectedItem;
                lvPriceGroups.Items[MoveDownItemIndex] = itemBelow;                

                priorityChanged = true;

                lvPriceGroups.Items[MoveDownItemIndex].Selected = false;
                lvPriceGroups.Items[MoveDownItemIndex + 1].Selected = true;
                lvPriceGroups.FocusedItem = lvPriceGroups.Items[MoveDownItemIndex + 1];
                lvPriceGroups.Select();

                lvPriceGroups.ResumeLayout();
            }
        }

        private void lvPriceGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsAddRemove.RemoveButtonEnabled = btnsAddRemove.AddButtonEnabled && lvPriceGroups.SelectedItems.Count > 0; 

            if (lvPriceGroups.Items.Count < 2 || lvPriceGroups.SelectedItems.Count == 0)
            {
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = false;
                return;
            }

            if (lvPriceGroups.FocusedItem != null && lvPriceGroups.FocusedItem.Index >= 0)
            {
                btnMoveUp.Enabled = true;
                btnMoveDown.Enabled = true;
            }

            int currItemIndex = lvPriceGroups.FocusedItem.Index;
            if (currItemIndex == 0)
            {
                btnMoveUp.Enabled = false;
            }
            else
            {
                btnMoveDown.Enabled = true;
                btnMoveUp.Enabled = true;
            }

            if (currItemIndex == lvPriceGroups.Items.Count - 1)
            {
                btnMoveDown.Enabled = false;
                btnMoveUp.Enabled = true;
            }     
        }

        private void btnsAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (PluginOperations.DeletePriceGroupLine((StoreInPriceGroup)lvPriceGroups.FocusedItem.Tag))
            {
                // Remove the line from the view as well
                int indexToRemove = lvPriceGroups.FocusedItem.Index;

                lvPriceGroups.SuspendLayout();
                lvPriceGroups.Items.RemoveAt(indexToRemove);
                lvPriceGroups.ResumeLayout();
            }
        }

        void lvPriceGroups_ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            lvPriceGroups.ContextMenuStrip.Items.Clear();


            ExtendedMenuItem item;

            item = new ExtendedMenuItem(
                    Properties.Resources.AddPriceGroup,
                    ContextButtons.GetAddButtonImage(),
                    110,
                    btnsAddRemove_AddButtonClicked);
            item.Enabled = btnsAddRemove.AddButtonEnabled;

            lvPriceGroups.ContextMenuStrip.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.RemovePriceGroup,
                    ContextButtons.GetRemoveButtonImage(),
                    120,
                    btnsAddRemove_RemoveButtonClicked);
            item.Enabled = btnsAddRemove.RemoveButtonEnabled;

            lvPriceGroups.ContextMenuStrip.Items.Add(item);

            lvPriceGroups.ContextMenuStrip.Items.Add(new ExtendedMenuItem("-", 130));

            item = new ExtendedMenuItem(
                    Properties.Resources.MoveUp,
                    ContextButtons.GetMoveUpButtonImage(),
                    140,
                    btnMoveUp_Click);
            item.Enabled = btnsAddRemove.RemoveButtonEnabled && lvPriceGroups.SelectedItems.Count > 0;

            lvPriceGroups.ContextMenuStrip.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.MoveDown,
                    ContextButtons.GetMoveDownButtonImage(),
                    150,
                    btnMoveDown_Click);
            item.Enabled = btnsAddRemove.RemoveButtonEnabled && lvPriceGroups.SelectedItems.Count > 0;

            lvPriceGroups.ContextMenuStrip.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("StoreLocationPriceGroup", lvPriceGroups.ContextMenuStrip, lvPriceGroups);

            e.Cancel = false;
        }
    }
}
