using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
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
    public partial class PriceGroupView : ViewBase
    {
        RecordIdentifier selectedID = "";

        internal PriceGroupView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID;
        }

        public PriceGroupView()
        {
            
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            btnsGroup.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.EditPriceGroups);

            lvPriceGroups.SetSortColumn(0, true);

            lvPriceGroups.ContextMenuStrip = new ContextMenuStrip();
            lvPriceGroups.ContextMenuStrip.Opening += lvPriceGroups_Opening;
            
            //HeaderIcon = Properties.Resources.PriceTag16;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.EditPriceGroups);


        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PriceDiscountGroups", 1, Properties.Resources.PriceDiscountGroups, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.PriceGroups;
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
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Stores, ViewPages.PriceDiscountGroupStoresPage.CreateInstance));

                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.SalesPrices, ViewPages.PriceDiscountGroupSalesPricesPage.CreateInstance));

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, selectedID, PriceDiscGroupEnum.PriceGroup);
            }

            HeaderText = Properties.Resources.PriceGroups;

            LoadItems(1, false);
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

        private void LoadItems(int sortBy, bool backwards)
        {
            List<PriceDiscountGroup> items;

            lvPriceGroups.ClearRows();

            items = Providers.PriceDiscountGroupData.GetGroups(PluginEntry.DataModel, PriceDiscountModuleEnum.Customer, PriceDiscGroupEnum.PriceGroup, sortBy, backwards);

            foreach (PriceDiscountGroup item in items)
            {
                Row row = new Row();
                row.AddText((string)item.GroupID);
                row.AddText(item.Text);
                row.AddText(item.IncludeTax ? Properties.Resources.Yes : Properties.Resources.No);
                row.Tag = item;

                lvPriceGroups.AddRow(row);
                if (selectedID == ((PriceDiscountGroup)row.Tag).ID)
                {
                    lvPriceGroups.Selection.Set(lvPriceGroups.Rows.IndexOf(row));
                }
            }

            lvPriceGroups.SetSortColumn(sortBy, !backwards);

            lvPriceGroups_SelectionChanged(this, EventArgs.Empty);
            lvPriceGroups.AutoSizeColumns();
        }
        
        private void loadTab(bool isRevert)
        {
            PriceDiscountGroup group;
            if (lvPriceGroups.Selection.Count > 0)
            {
                group = (PriceDiscountGroup)lvPriceGroups.Selection[0].Tag;
            }
            else
            {
                group = new PriceDiscountGroup();
            }
            selectedID = group.ID;
            tabSheetTabs.SetData(isRevert, selectedID, PriceDiscGroupEnum.PriceGroup); 
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.CustomerPriceDiscDialog dlg = new Dialogs.CustomerPriceDiscDialog(PriceDiscGroupEnum.PriceGroup);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedID = dlg.ID;
                LoadItems(lvPriceGroups.Columns.IndexOf(lvPriceGroups.SortColumn), !lvPriceGroups.SortedAscending);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Dialogs.CustomerPriceDiscDialog dlg;

            selectedID = ((PriceDiscountGroup)lvPriceGroups.Selection[0].Tag).ID;

            dlg = new Dialogs.CustomerPriceDiscDialog(PriceDiscGroupEnum.PriceGroup, selectedID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems(lvPriceGroups.Columns.IndexOf(lvPriceGroups.SortColumn), !lvPriceGroups.SortedAscending);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            selectedID = ((PriceDiscountGroup)lvPriceGroups.Selection[0].Tag).ID;
            bool didDeletedGroup = PluginOperations.DeletePriceDiscountGroup((PriceDiscountGroup)lvPriceGroups.Selection[0].Tag, PriceDiscGroupEnum.PriceGroup);
            if (didDeletedGroup)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Customer", RecordIdentifier.Empty, null);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Store", RecordIdentifier.Empty, null);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "PriceDiscountGroup", selectedID, null);
                LoadItems(lvPriceGroups.Columns.IndexOf(lvPriceGroups.SortColumn), !lvPriceGroups.SortedAscending);
            }
        }

        void lvPriceGroups_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvPriceGroups.ContextMenuStrip;

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


            PluginEntry.Framework.ContextMenuNotify("CustomerPriceDiscountList", lvPriceGroups.ContextMenuStrip, lvPriceGroups);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override List<IDataEntity> GetListSelection()
        {
            List<IDataEntity> selectedGroups = new List<IDataEntity>();
            for (int i = 0; i < lvPriceGroups.Selection.Count; i++)
            {
                selectedGroups.Add((PriceDiscountGroup)lvPriceGroups.Selection[i].Tag);
            }
            return selectedGroups;
        }

        public override ParentViewDescriptor CurrentViewDescriptor()
        {
            return new ParentViewDescriptor(
                    (int)PriceDiscGroupEnum.PriceGroup,
                    LogicalContextName,
                    null,
                    PluginOperations.ShowCustomerPriceGroups);
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        private void lvPriceGroups_HeaderClicked(object sender, ColumnEventArgs args)
        {
            bool ascending = lvPriceGroups.SortColumn != args.Column || !lvPriceGroups.SortedAscending;
            LoadItems(args.ColumnNumber, !ascending);
        }

        private void lvPriceGroups_SelectionChanged(object sender, EventArgs e)
        {
            btnsGroup.EditButtonEnabled =
                btnsGroup.RemoveButtonEnabled =
                lvPriceGroups.Selection.Count > 0 &&
                PluginEntry.DataModel.HasPermission(Permission.EditPriceGroups);

            if (lvPriceGroups.Selection.Count > 0)
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

            SetContextBarItemEnabled(GetType().ToString() + ".TradeAgreements", "PG", btnsGroup.EditButtonEnabled);
        }

        private void lvPriceGroups_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsGroup.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }
    }
}
