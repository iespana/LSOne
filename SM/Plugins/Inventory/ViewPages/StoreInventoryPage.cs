using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class StoreInventoryPage : UserControl, ITabView
    {
        private Store store;

        public StoreInventoryPage(TabControl owner)
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new StoreInventoryPage((TabControl)sender);
        }

        public bool DataIsModified()
        {
            if (store != null &&
                (store.ReturnReasonCodeID != cmbReason.SelectedData.ID ||
                store.StoreTransferDefaultDeliveryTime != (int)ntbDefaultDeliveryTime.Value ||
                store.StoreTransferDeliveryDaysType != (DeliveryDaysTypeEnum)cmbDays.SelectedIndex))
            {
                return true;
            }

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            store = (Store)internalContext;

            ReasonCode code = Providers.ReasonCodeData.GetReasonById(PluginEntry.DataModel, store.ReturnReasonCodeID);

            cmbReason.SelectedData = code != null ? new DataEntity(code.ID, code.Text) : new DataEntity("", "");
            ntbDefaultDeliveryTime.Value = store.StoreTransferDefaultDeliveryTime;
            cmbDays.SelectedIndex = (int)store.StoreTransferDeliveryDaysType;
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            store.ReturnReasonCodeID = cmbReason.SelectedData.ID;
            store.StoreTransferDefaultDeliveryTime = (int)ntbDefaultDeliveryTime.Value;
            store.StoreTransferDeliveryDaysType = (DeliveryDaysTypeEnum)cmbDays.SelectedIndex;
            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void cmbReason_RequestClear(object sender, EventArgs e)
        {

        }

        private void cmbReason_RequestData(object sender, EventArgs e)
        {
            cmbReason.SetData(Providers.ReasonCodeData.GetReasonCodesForReturn(PluginEntry.DataModel), null);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowReasonCodesView(cmbReason.SelectedData.ID);
        }
    }
}
