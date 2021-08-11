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
using LSOne.ViewCore.EventArguments;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.TradeAgreements.Views
{
    public partial class CustomerDiscountGroupView : ViewBase
    {
        PriceDiscGroupEnum displayType;

        RecordIdentifier selectedID = "";
        bool lockEvents;

        internal CustomerDiscountGroupView(RecordIdentifier selectedID)
            : this((PriceDiscGroupEnum)((int)selectedID.SecondaryID.PrimaryID))
        {
            this.selectedID = selectedID;
        }

        internal CustomerDiscountGroupView(PriceDiscGroupEnum type)
            : this()
        {
            displayType = type;

            lockEvents = true; // Prevent double loading of the context bar
            cmbShowType.SelectedIndex = (int)(type - 1);
            lockEvents = false;
        }

        private CustomerDiscountGroupView()
        {
            lockEvents = false;

            displayType = PriceDiscGroupEnum.PriceGroup;

            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            btnsGroup.AddButtonEnabled= PluginEntry.DataModel.HasPermission(Permission.EditCustomerDiscGroups);

            lvItems.SmallImageList = PluginEntry.Framework.GetImageList();

            lvItems.SortColumn = 0;

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += lvItems_Opening;

            //HeaderIcon = Properties.Resources.PriceTag16;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.EditCustomerDiscGroups);


        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PriceDiscountGroups", 1, Properties.Resources.PriceDiscountGroups, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.CustomerDiscountGroups;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void OnSetupContextBarHeaders(ContextBarHeaderConstructionArguments arguments)
        {
            arguments.Add(new ContextBarHeader(Properties.Resources.TradeAgreements, GetType().ToString() + ".TradeAgreements"), 200);

            base.OnSetupContextBarHeaders(arguments);
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Customers, ViewPages.PriceDiscountGroupCustomersPage.CreateInstance));
                if (PluginEntry.DataModel.HasPermission(Permission.ManageTradeAgreementPrices))
                {
                    tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Discounts,
                        ViewPages.PriceDiscountGroupsTradeAgreementsPage.CreateInstance));
                }
                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, selectedID, displayType);
            }

            HeaderText = Properties.Resources.CustomerDiscountGroups;

            LoadItems(1, false, true);
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Customer":
                case "TradeAgreement":
                    loadTab(false);
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeAction, objectName, changeIdentifier, param);
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

        private void LoadItems(int sortBy, bool backwards, bool doBestFit)
        {
            List<PriceDiscountGroup> items;
            ListViewItem listItem;

            lvItems.Items.Clear();

            items = Providers.PriceDiscountGroupData.GetGroups(PluginEntry.DataModel, PriceDiscountModuleEnum.Customer, displayType, sortBy, backwards);

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

            lvItems.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvItems.SortColumn = sortBy;

            lvItems_SelectedIndexChanged(this, EventArgs.Empty);

            if (doBestFit)
            {
                lvItems.BestFitColumns();
            }
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {

            btnsGroup.EditButtonEnabled = 
                btnsGroup.RemoveButtonEnabled = 
                lvItems.SelectedItems.Count > 0 && 
                PluginEntry.DataModel.HasPermission(Permission.EditCustomerDiscGroups);

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
                case PriceDiscGroupEnum.LineDiscountGroup:
                    SetContextBarItemEnabled(GetType().ToString() + ".TradeAgreements", "LDG", btnsGroup.EditButtonEnabled);
                    break;
                case PriceDiscGroupEnum.MultilineDiscountGroup:
                    SetContextBarItemEnabled(GetType().ToString() + ".TradeAgreements", "MLDG", btnsGroup.EditButtonEnabled);
                    break;
                case PriceDiscGroupEnum.TotalDiscountGroup:
                    SetContextBarItemEnabled(GetType().ToString() + ".TradeAgreements", "TDG", btnsGroup.EditButtonEnabled);
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
                }
                lvItems.SortedBackwards = false;
            }

            LoadItems(e.Column, lvItems.SortedBackwards, false);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.CustomerPriceDiscDialog dlg = new Dialogs.CustomerPriceDiscDialog(displayType);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedID = dlg.ID;
                LoadItems(lvItems.SortColumn, lvItems.SortedBackwards, true);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Dialogs.CustomerPriceDiscDialog dlg;

            selectedID = ((PriceDiscountGroup)lvItems.SelectedItems[0].Tag).ID;

            dlg = new Dialogs.CustomerPriceDiscDialog(displayType,selectedID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems(lvItems.SortColumn, lvItems.SortedBackwards, true);
            }
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (btnsGroup.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            selectedID = ((PriceDiscountGroup)lvItems.SelectedItems[0].Tag).ID;
            bool didDeletedGroup = PluginOperations.DeletePriceDiscountGroup((PriceDiscountGroup)lvItems.SelectedItems[0].Tag, GetSelectedType());
            if (didDeletedGroup)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Customer", RecordIdentifier.Empty, null);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Store", RecordIdentifier.Empty, null);
                LoadItems(lvItems.SortColumn, lvItems.SortedBackwards, true);                
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
            item.Enabled = btnsGroup.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);

            item.Enabled = btnsGroup.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsGroup.RemoveButtonEnabled;
            menu.Items.Add(item);


            PluginEntry.Framework.ContextMenuNotify("CustomerPriceDiscountList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void cmbShowType_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayType = GetSelectedType();

            LoadItems(lvItems.SortColumn >= 0 ? lvItems.SortColumn : 1, false, true);

            if (!lockEvents)
            {
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }


            if (tabSheetTabs.TabCount > 0)
            {
                tabSheetTabs.SendTabMessage((int)displayType, selectedID);

                tabSheetTabs.SelectedTab = tabSheetTabs[0];
            }
        }

        public override List<IDataEntity> GetListSelection()
        {
            return lvItems.GetSelectedDataEntities();
        }

        private PriceDiscGroupEnum GetSelectedType()
        {
            return (PriceDiscGroupEnum)(cmbShowType.SelectedIndex + 1);
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
