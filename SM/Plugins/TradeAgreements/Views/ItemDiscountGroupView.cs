using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.TradeAgreements.Views
{
    public partial class ItemDiscountGroupView : ViewBase
    {
        private WeakReference itemViewer;

        PriceDiscGroupEnum displayType;
        RecordIdentifier selectedID = "";
        bool lockEvents;

        internal ItemDiscountGroupView(RecordIdentifier selectedID)
            : this((PriceDiscGroupEnum)((int)selectedID.SecondaryID.PrimaryID))
        {
            this.selectedID = selectedID;
        }
        
        internal ItemDiscountGroupView(PriceDiscGroupEnum type)
            : this()
        {
            displayType = type;

            lockEvents = true; // Prevent double loading of the context bar
            cmbShowType.SelectedIndex = (type == PriceDiscGroupEnum.LineDiscountGroup) ? 0 : 1;
            lockEvents = false;
        }

        public ItemDiscountGroupView()
        {
            IPlugin plugin;

            displayType = PriceDiscGroupEnum.PriceGroup;

            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            lvItems.SmallImageList = PluginEntry.Framework.GetImageList();

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += lvItems_Opening;

            //HeaderIcon = Properties.Resources.PriceTag16;

            plugin = PluginEntry.Framework.FindImplementor(this, "CanViewRetailItem", null);
            itemViewer = plugin != null ? new WeakReference(plugin) : null;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.EditItemDiscGroups);
            btnsContextButtons.AddButtonEnabled = !ReadOnly;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PriceDiscountGroups", 0, Properties.Resources.PriceDiscountGroups, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.ItemDiscountGroups;
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
            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Items, ViewPages.ItemDiscountGroupsItemsPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Discounts, ViewPages.ItemDiscountGroupsTradeAgreementsPage.CreateInstance));

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, selectedID, displayType);
            }

            HeaderText = Properties.Resources.ItemDiscountGroups;

            LoadItems(isRevert);
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "RetailItem":
                    selectedID = changeIdentifier;
                    loadTab(true);
                    break;
            }
        }

        private void loadTab(bool isRevert)
        {
            PriceDiscountGroup group;
            if (lvItems.SelectedItems.Count > 0)
            {
                group = (PriceDiscountGroup)lvItems.SelectedItems[0].Tag;
            }
            else
            {
                group = new PriceDiscountGroup();
            }
            selectedID = group.ID;
            tabSheetTabs.SetData(isRevert, selectedID, displayType); 
        }

        protected override bool DataIsModified()
        {
            // Here our sheet is supposed to figure out if something needs to be saved and return
            // true if something needs to be saved, else false.
            return false;
        }

        protected override bool SaveData()
        {
            // Here we would let our sheet save our data.

            // We return true since saving was successful, if we would return false then
            // the viewstack will prevent other sheet from getting shown.
            return true;
        }

        private void LoadItems(bool isRevert)
        {
            List<PriceDiscountGroup> items;
            ListViewItem listItem;

            lvItems.Items.Clear();

            items = Providers.PriceDiscountGroupData.GetGroups(PluginEntry.DataModel,PriceDiscountModuleEnum.Item, displayType,lvItems.SortColumn,lvItems.SortedBackwards);

            foreach (PriceDiscountGroup item in items)
            {
                listItem = new ListViewItem((string)item.GroupID);
                listItem.SubItems.Add(item.Text);

                listItem.ImageIndex = -1;

                listItem.Tag = item;

                lvItems.Add(listItem);

                if (selectedID == ((PriceDiscountGroup)listItem.Tag).ID)
                {
                    listItem.Selected = true;
                }
            }

            loadTab(isRevert);

            lvItems.Columns[lvItems.SortColumn].ImageIndex = (lvItems.SortedBackwards ? 1 : 0);

            lvItems_SelectedIndexChanged(this, EventArgs.Empty);

            lvItems.BestFitColumns();
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = 
                lvItems.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.EditItemDiscGroups);

            if (lvItems.SelectedItems.Count > 0)
            {
                if (tabSheetTabs.Visible == false)
                {
                    tabSheetTabs.Visible = true;

                    lblNoSelection.Visible = false;
                }

                loadTab(true);
            }
            else if (tabSheetTabs.Visible)
            {
                tabSheetTabs.Visible = false;

                lblNoSelection.Visible = true;
            }

            switch (displayType)
            {
                case PriceDiscGroupEnum.MultilineDiscountGroup:
                    SetContextBarItemEnabled(GetType().ToString() + ".TradeAgreements","MLD", btnsContextButtons.EditButtonEnabled);
                    break;
                case PriceDiscGroupEnum.LineDiscountGroup:
                    SetContextBarItemEnabled(GetType().ToString() + ".TradeAgreements", "LD", btnsContextButtons.EditButtonEnabled);
                    break;
            }
        }

        private void lvItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvItems.SortColumn == e.Column)
            {
                lvItems.SortedBackwards = !lvItems.SortedBackwards;
            }
            else
            {
                if (lvItems.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvItems.Columns[lvItems.SortColumn].ImageIndex = 2;
                    lvItems.SortColumn = e.Column;
                }
                lvItems.SortedBackwards = false;
            }

            LoadItems(true);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.ItemDiscDialog dlg = new Dialogs.ItemDiscDialog(displayType);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedID = dlg.GroupId;
                LoadItems(true);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Dialogs.ItemDiscDialog dlg;

            selectedID = ((PriceDiscountGroup)lvItems.SelectedItems[0].Tag).ID;

            dlg = new Dialogs.ItemDiscDialog(displayType, selectedID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems(true);
            }
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            bool didDeletedGroup = PluginOperations.DeletePriceDiscountGroup((PriceDiscountGroup)lvItems.SelectedItems[0].Tag, GetSelectedType());
            if (didDeletedGroup)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Customer", RecordIdentifier.Empty, null);
                LoadItems(true);
            }     
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditText,
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
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);
            
            PluginEntry.Framework.ContextMenuNotify("DiscountGroupList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void cmbShowType_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayType = GetSelectedType();

            if (lvItems.SortColumn < 0)
	        {
                lvItems.SortColumn = 1;
	        }
            
            LoadItems(true);

            if (!lockEvents)
            {
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }
        }

        public override List<IDataEntity> GetListSelection()
        {
            return lvItems.GetSelectedDataEntities();
        }

        private RecordIdentifier GetSelectionID()
        {
            List<IDataEntity> selection = GetListSelection();

            return selection.Count > 0 ? selection[0].ID : RecordIdentifier.Empty;
        }

        private PriceDiscGroupEnum GetSelectedType()
        {
            return cmbShowType.SelectedIndex == 0 ? PriceDiscGroupEnum.LineDiscountGroup : PriceDiscGroupEnum.MultilineDiscountGroup;
        }

        private string GetSelectionText()
        {
            return lvItems.SelectedItems.Count > 0 ? lvItems.SelectedItems[0].SubItems[1].Text : "";
        }

        public override ParentViewDescriptor CurrentViewDescriptor()
        {
            return new ParentViewDescriptor(
                    (int)displayType,
                    LogicalContextName,
                    null,
                    PluginOperations.ShowCustomerDiscountGroups);
        }

        protected override void OnClose()
        {
            lvItems.SmallImageList = null;

            base.OnClose();
        }
    }
}
