using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    internal partial class InventoryAdminPage : UserControl, ITabViewV2
    {
        Setting maxOverReceiveGoods;
        Setting blindReceivingPurchaseOrder;
        Setting costCalculation;

        public InventoryAdminPage()
        {
            InitializeComponent();

            maxOverReceiveGoods = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.MaxOverReceiveGoods, SettingsLevel.System);
            blindReceivingPurchaseOrder = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.BlindReceivingPurchaseOrder, SettingsLevel.System);
            costCalculation = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.CostCalculation, SettingsLevel.System);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.InventoryAdminPage();
        }

        private int MaxOverReceiveGoods
        {
            get
            {
                return Convert.ToInt32(maxOverReceiveGoods.Value);
            }
            set
            {
                maxOverReceiveGoods.Value = value.ToString();
                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, Settings.MaxOverReceiveGoods, SettingsLevel.System, maxOverReceiveGoods);
            }
        }

        private bool BlindReceivingPurchaseOrder
        {
            get
            {
                return blindReceivingPurchaseOrder.BoolValue;
            }
            set
            {
                blindReceivingPurchaseOrder.BoolValue = value;
                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, Settings.BlindReceivingPurchaseOrder, SettingsLevel.System, blindReceivingPurchaseOrder);
            }
        }

        private int CostCalculation
        {
            get
            {
                return costCalculation.IntValue;
            }
            set
            {
                costCalculation.IntValue = value;
                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, Settings.CostCalculation, SettingsLevel.System, costCalculation);
            }
        }

        #region ITabPanel Members

        public void InitializeView(RecordIdentifier context, object internalContext)
        {

        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            ntbMaxOverReceive.Value = MaxOverReceiveGoods;
            cbBlindReceivingPurchaseOrder.Checked = BlindReceivingPurchaseOrder;
            cmbCostCalculation.SelectedIndex = CostCalculation;
        }

        public bool DataIsModified()
        {
            if (ntbMaxOverReceive.Value != MaxOverReceiveGoods ||
                cbBlindReceivingPurchaseOrder.Checked != BlindReceivingPurchaseOrder ||
                cmbCostCalculation.SelectedIndex != CostCalculation) return true;

            return false;
        }

        public bool SaveData()
        {
            MaxOverReceiveGoods = (int)ntbMaxOverReceive.Value;
            BlindReceivingPurchaseOrder = cbBlindReceivingPurchaseOrder.Checked;
            CostCalculation = cmbCostCalculation.SelectedIndex;
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PurchaseOrderTaxSettings", null, null);

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            maxOverReceiveGoods = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.MaxOverReceiveGoods, SettingsLevel.System);
            ntbMaxOverReceive.Value = MaxOverReceiveGoods;

            blindReceivingPurchaseOrder = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.BlindReceivingPurchaseOrder, SettingsLevel.System);
            cbBlindReceivingPurchaseOrder.Checked = BlindReceivingPurchaseOrder;

            costCalculation = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.CostCalculation, SettingsLevel.System);
            cmbCostCalculation.SelectedIndex = CostCalculation;
        }

        #endregion

        
    }
}
