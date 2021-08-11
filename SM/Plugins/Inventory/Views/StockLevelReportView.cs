using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.ViewPlugins.Inventory.Reports;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class StockLevelReportView : ViewBase
    {
        bool isHeadOffice;
        WeakReference printingHandler;
        Store store;

        public StockLevelReportView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Help | ViewAttributes.Close | ViewAttributes.ContextBar;

            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "CanDisplayXtraReport", null);
            printingHandler = plugin != null ? new WeakReference(plugin) : null;

            btnShowReport.Visible = (printingHandler != null);

            isHeadOffice = (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty);

            if (isHeadOffice)
            {
                //cmbStore.Enabled = true;
                cmbStore.SelectedData = new DataEntity("", Properties.Resources.AllStores);
            }
            else
            {
                store = Providers.StoreData.Get(PluginEntry.DataModel, (string)PluginEntry.DataModel.CurrentStoreID);
                cmbStore.SelectedData = store;
                cmbStore.Enabled = false;
            }

            cmbFilter.SelectedIndex = 0;
            cmbFilterValue.SelectedData = new DataEntity("", "");
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.StockLevelReport;
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
            HeaderText = Properties.Resources.StockLevelReport;            
            HeaderIcon = null;
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }
        
        private void btnShowReport_Click(object sender, EventArgs e)
        {
            if (printingHandler.IsAlive)
            {
                var filterGroupType = (InventoryGroup)cmbFilter.SelectedIndex;
                var filterGroupID = cmbFilterValue.SelectedData.ID;
                var filterGroupName = cmbFilterValue.SelectedData.Text;
                List<InventoryStatus> inventoryStatusList;
                try
                {
                     inventoryStatusList = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryStatuses(
                                PluginEntry.DataModel,
                                PluginOperations.GetSiteServiceProfile(),
                                cmbStore.SelectedData.ID, filterGroupType, filterGroupID,
                                true);
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                    return;
                }
               

                var report = new StockLevelReport(inventoryStatusList, filterGroupType, filterGroupName, cmbStore.SelectedData.Text);

                ((IPlugin)printingHandler.Target).Message(this, "ShowReport", new object[] { "StockLevelReport", report, Properties.Resources.StockLevelReport, null });
            }
        }        


        private void cmbStoreSelect_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> stores = Providers.StoreData.GetList(PluginEntry.DataModel);            
            cmbStore.SetData(stores, null);            
        }

        private void cmbStoreSelect_RequestClear(object sender, EventArgs e)
        {
            cmbStore.SelectedData = new DataEntity("", Properties.Resources.AllStores);
        }

        private void btnRetailDepartment_Click(object sender, EventArgs e)
        {
            //var inventoryStatusList = Providers.InventoryData.GetInventoryStatuses(PluginEntry.DataModel, cmbStore.SelectedData.ID, InventoryGroup.RetailDepartment, "FOOD");
        }

        private void btnRetailGroup_Click(object sender, EventArgs e)
        {
            //var inventoryStatusList = Providers.InventoryData.GetInventoryStatuses(PluginEntry.DataModel, cmbStore.SelectedData.ID, InventoryGroup.RetailGroup, "FOOD");
        }

        private void btnVendor_Click(object sender, EventArgs e)
        {
            //var inventoryStatusList = Providers.InventoryData.GetInventoryStatuses(PluginEntry.DataModel, cmbStore.SelectedData.ID, InventoryGroup.Vendor, "FOOD");
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbFilterValue.Enabled = (cmbFilter.SelectedIndex != 0);

            cmbFilterValue_RequestClear(this, EventArgs.Empty);
        }

        private void cmbFilterValue_RequestClear(object sender, EventArgs e)
        {
            cmbFilterValue.SelectedData = new DataEntity("", "");
            CheckShowBtnEnabled(sender, e);
        }

        private void cmbFilterValue_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> dataList = new List<DataEntity>() ;
            InventoryGroup selectedGroup = (InventoryGroup)cmbFilter.SelectedIndex;

            try
            {



                switch (selectedGroup)
                {
                    case InventoryGroup.RetailGroup:
                        dataList = Providers.RetailGroupData.GetList(PluginEntry.DataModel);
                        break;
                    case InventoryGroup.RetailDepartment:
                        dataList = Providers.RetailDepartmentData.GetList(PluginEntry.DataModel);
                        break;
                    case InventoryGroup.Vendor:
                        dataList = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).
                        GetVendorList(
                            PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(),
                            true);
                        //Providers.VendorData.GetList(PluginEntry.DataModel);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
           
            cmbFilterValue.SetData(dataList, null);
        }

        private void CheckShowBtnEnabled(object sender, EventArgs e)
        {
            btnShowReport.Enabled = (cmbFilterValue.SelectedData.ID != "" && cmbStore.SelectedData.ID != "");
        }

    }
}
