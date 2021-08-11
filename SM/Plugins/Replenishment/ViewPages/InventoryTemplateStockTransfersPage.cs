using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;

namespace LSOne.ViewPlugins.Replenishment.ViewPages
{
    public partial class InventoryTemplateStockTransfersPage: UserControl, ITabView
    {
        InventoryTemplate template;        

        public InventoryTemplateStockTransfersPage()
        {
            InitializeComponent();            
            cmbDefaultReceivingStore.SelectedData = new DataEntity();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new InventoryTemplateStockTransfersPage();
        }

        public bool DataIsModified()
        {
            return cmbDefaultReceivingStore.SelectedDataID != template.DefaultStore ||
                   chkAutoPopulateReceiving.Checked != template.AutoPopulateTransferOrderReceivingQuantity;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            template = (InventoryTemplate)((List<object>)internalContext)[0];

            chkAutoPopulateReceiving.Checked = template.AutoPopulateTransferOrderReceivingQuantity;

            DataEntity storeEntity = new DataEntity();
            if (!string.IsNullOrEmpty((string)template.DefaultStore))
            {
                Store store = Providers.StoreData.Get(PluginEntry.DataModel, template.DefaultStore);
                storeEntity = new DataEntity(store.ID, store.Text);
            }

            cmbDefaultReceivingStore.SelectedData = storeEntity;
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {           
            
        }

        public bool SaveData()
        {
            template.DefaultStore = cmbDefaultReceivingStore.SelectedDataID;
            template.AutoPopulateTransferOrderReceivingQuantity = chkAutoPopulateReceiving.Checked;
            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void cmbDefaultReceivingStore_RequestData(object sender, EventArgs e)
        {
            cmbDefaultReceivingStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbDefaultReceivingStore_RequestClear(object sender, EventArgs e)
        {
            cmbDefaultReceivingStore.SelectedData = new DataEntity();
        }
    }
}