using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    public partial class DiningTableLayoutView : ViewBase
    {
        private RecordIdentifier diningTableLayoutID = RecordIdentifier.Empty;
        private DiningTableLayout diningTableLayout;
        private HospitalityType hospitalityType;

        public DiningTableLayoutView(RecordIdentifier diningTableLayoutID)
            : this()
        {
            this.diningTableLayoutID = diningTableLayoutID;
        }

        public DiningTableLayoutView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;

            lvRestaurantDiningTables.ContextMenuStrip = new ContextMenuStrip();
            lvRestaurantDiningTables.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageDiningTableLayouts);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("DiningTableLayout", RecordIdentifier.Empty, Properties.Resources.DiningTableLayout, true));
        }

        public string Description
        {
            get
            {
                //return Properties.Resources.SalesTypeText + ": " + salesTypeID + " - " + tbDescription.Text;
                return "";
            }
        }

        public override string ContextDescription
        {
            get
            {
                return tbDescription.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.DiningTableLayout;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return diningTableLayoutID;
            }

            
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {

            }

            diningTableLayout = Providers.DiningTableLayoutData.Get(PluginEntry.DataModel, diningTableLayoutID);
            hospitalityType = Providers.HospitalityTypeData.Get(PluginEntry.DataModel, diningTableLayout.RestaurantID, diningTableLayout.SalesType);

            tbLayoutID.Text = (string)diningTableLayout.LayoutID;
            tbDescription.Text = diningTableLayout.Text;
            ntbNoOfScreens.Value = (double)diningTableLayout.NumberOfScreens;
            ntbNoOfDiningTables.Value = (double)diningTableLayout.NumberOfDiningTables;
            ntbStartnigTableNo.Value = (double)diningTableLayout.StartingTableNumber;
            ntbEndingTableNo.Value = (double)diningTableLayout.EndingTableNumber;
            ntbDiningTableRows.Value = (double)diningTableLayout.DiningTableRows;
            ntbDiningTableColumns.Value = (double)diningTableLayout.DiningTableColumns;

            HeaderText = diningTableLayout.Text;

            LoadRestaurantDiningTables();
        }

        protected override bool DataIsModified()
        {

            if (tbLayoutID.Text != (string)diningTableLayout.LayoutID) return true;
            if (tbDescription.Text != diningTableLayout.Text) return true;
            if (ntbNoOfScreens.Value != (double)diningTableLayout.NumberOfScreens) return true;
            if (ntbNoOfDiningTables.Value != (double)diningTableLayout.NumberOfDiningTables) return true;
            if (ntbStartnigTableNo.Value != (double)diningTableLayout.StartingTableNumber) return true;
            if (ntbEndingTableNo.Value != (double)diningTableLayout.EndingTableNumber) return true;
            if (ntbDiningTableRows.Value != (double)diningTableLayout.DiningTableRows) return true;
            if (ntbDiningTableColumns.Value != (double)diningTableLayout.DiningTableColumns) return true;

            return false;
        }

        protected override bool SaveData()
        {
            diningTableLayout.Text = tbDescription.Text;
            diningTableLayout.NumberOfScreens = (int)ntbNoOfScreens.Value;
            diningTableLayout.NumberOfDiningTables = (int)ntbNoOfDiningTables.Value;
            diningTableLayout.StartingTableNumber = (int)ntbStartnigTableNo.Value;
            diningTableLayout.EndingTableNumber = (int)ntbEndingTableNo.Value;
            diningTableLayout.DiningTableRows = (int)ntbDiningTableRows.Value;
            diningTableLayout.DiningTableColumns = (int)ntbDiningTableColumns.Value;

            Providers.DiningTableLayoutData.Save(PluginEntry.DataModel, diningTableLayout);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "DiningTableLayout", diningTableLayout.ID, diningTableLayout);

            return true;
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteDiningTableLayout(diningTableLayoutID);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            // Removed since the screens are not yet implemented in the POS
            //if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            //{                                              
            //    arguments.Add(new ContextBarItem(Properties.Resources.Screens, null, new ContextbarClickEventHandler((ContextbarClickEventHandler)ShowScreens)), 300);
            //}
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "DiningTableLayout":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == diningTableLayoutID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;

                case "RestaurantDiningTable":
                        LoadRestaurantDiningTables();                    
                    break;

            }
        }

        private void LoadRestaurantDiningTables()
        {
            lvRestaurantDiningTables.Items.Clear();

            List<RestaurantDiningTable> restaurantDiningTables = Providers.RestaurantDiningTableData.GetList(PluginEntry.DataModel, diningTableLayout.RestaurantID, diningTableLayout.Sequence, diningTableLayout.SalesType, diningTableLayout.LayoutID);
            ListViewItem item;

            foreach (RestaurantDiningTable restaurantDiningTable in restaurantDiningTables)
            {
                item = new ListViewItem();
                item.Text = restaurantDiningTable.DineInTableNo.ToString();
                item.SubItems.Add(restaurantDiningTable.Text);
                item.SubItems.Add(restaurantDiningTable.GetDescription(hospitalityType.TableButtonDescription));
                item.SubItems.Add(restaurantDiningTable.NonSmoking ? Properties.Resources.Yes : Properties.Resources.No);
                item.SubItems.Add(restaurantDiningTable.Availability == RestaurantDiningTable.AvailabilityEnum.Available ? Properties.Resources.Available : Properties.Resources.NotAvailable);
                //item.SubItems.Add(restaurantDiningTable.LayoutScreenNo.ToString()); // Removed temporarily since this is not yet implemented in the POS

                item.Tag = restaurantDiningTable.ID;

                lvRestaurantDiningTables.Add(item);
            }

            lvRestaurantDiningTables_SelectedIndexChanged(this, EventArgs.Empty);
            lvRestaurantDiningTables.BestFitColumns();

            UpdateCounterFields();
        }

        private void UpdateCounterFields()
        {
            ntbNoOfScreens.Value = Providers.DiningTableLayoutScreenData.GetNumberOfScreens(PluginEntry.DataModel, diningTableLayoutID);
            ntbNoOfDiningTables.Value = lvRestaurantDiningTables.Items.Count;
            ntbStartnigTableNo.Value = Providers.RestaurantDiningTableData.GetStartingTableNumber(PluginEntry.DataModel, diningTableLayoutID);
            ntbEndingTableNo.Value = Providers.RestaurantDiningTableData.GetEndingTableNumber(PluginEntry.DataModel, diningTableLayoutID);
        }

        private void ShowScreens(object sender, EventArgs args)
        {
            PluginOperations.ShowDiningTableLayoutScreens(diningTableLayout.ID);            
        }

        private void FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == "" || ((DataEntity)e.Data).ID == RecordIdentifier.Empty)
            {
                e.TextToDisplay = "";
            }
            else
            {
                e.TextToDisplay = ((DataEntity)e.Data).ID.ToString() + " - " + ((DataEntity)e.Data).Text;
            }
        }

        private void cmbPriceGroup_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == "" || ((DataEntity)e.Data).ID == RecordIdentifier.Empty)
            {
                e.TextToDisplay = "";
            }
            else
            {
                e.TextToDisplay = ((DataEntity)e.Data).ID.ToString() + " - " + ((DataEntity)e.Data).Text;                
            }
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void lvRestaurantDiningTables_SelectedIndexChanged(object sender, EventArgs e)
        {            
            btnsEditAddRemoveDiningTables.EditButtonEnabled = btnsEditAddRemoveDiningTables.RemoveButtonEnabled = lvRestaurantDiningTables.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageDiningTableLayouts);
        }

        private void btnsEditAddRemoveDiningTables_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.AddRestaurantDiningTables(diningTableLayoutID, (int)(ntbDiningTableColumns.Value * ntbDiningTableRows.Value));
        }

        private void btnsEditAddRemoveDiningTables_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowRestaurantDiningTable((RecordIdentifier)lvRestaurantDiningTables.SelectedItems[0].Tag);
        }

        private void btnsEditAddRemoveDiningTables_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteRestaurantDiningTable((RecordIdentifier)lvRestaurantDiningTables.SelectedItems[0].Tag);
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvRestaurantDiningTables.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnsEditAddRemoveDiningTables_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemoveDiningTables.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   new EventHandler(btnsEditAddRemoveDiningTables_AddButtonClicked));

            item.Enabled = btnsEditAddRemoveDiningTables.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemoveDiningTables_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemoveDiningTables.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("RestaurantDiningTableList", lvRestaurantDiningTables.ContextMenuStrip, lvRestaurantDiningTables);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvRestaurantDiningTables_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemoveDiningTables.EditButtonEnabled)
            {
                btnsEditAddRemoveDiningTables_EditButtonClicked(sender, e);
            }
        }

        protected override void OnClose()
        {
            lvRestaurantDiningTables.SmallImageList = null;

            base.OnClose();
        }
    }
}
