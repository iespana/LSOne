using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    public partial class RetailGroupGeneralPage : UserControl, ITabView
    {

        RetailGroup retailGroup;
        WeakReference ValditionPeriodEditor;
        WeakReference salesTaxGroupEditor;

        public RetailGroupGeneralPage()
        {
            IPlugin plugin;

            InitializeComponent();

            plugin = PluginEntry.Framework.FindImplementor(this, "ViewItemSalesTaxGroups", null);
            salesTaxGroupEditor = plugin != null ? new WeakReference(plugin) : null;
            btnEditItemSalesTaxGroup.Visible = (salesTaxGroupEditor != null);

            plugin = PluginEntry.Framework.FindImplementor(this, "CanEditValidationPeriod", null);
            ValditionPeriodEditor = plugin != null ? new WeakReference(plugin) : null;
            btnEditValidationPeriod.Visible = (ValditionPeriodEditor != null);

        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new RetailGroupGeneralPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            retailGroup = (RetailGroup)internalContext;

            cmbRetailDepartment.SelectedData = new DataEntity(retailGroup.RetailDepartmentID, retailGroup.RetailDepartmentName);
            cmbItemSalesTaxGroup.SelectedData = new DataEntity(retailGroup.ItemSalesTaxGroupId, retailGroup.ItemSalesTaxGroupName);

            ntbProfitMargin.Value = (double)retailGroup.ProfitMargin;
            cmbValidationPeriod.SelectedData = new DataEntity(retailGroup.ValidationPeriod, retailGroup.ValidationPeriodDescription);

            DecimalLimit quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
            ntbTareWeight.SetValueWithLimit(retailGroup.TareWeight, quantityLimiter);
        }

        public bool DataIsModified()
        {
            if (cmbRetailDepartment.SelectedData.ID != retailGroup.RetailDepartmentID) return true;
            if (cmbItemSalesTaxGroup.SelectedData.ID != retailGroup.ItemSalesTaxGroupId) return true;
            if (ntbProfitMargin.FullPrecisionValue != retailGroup.ProfitMargin) return true;
            if (cmbValidationPeriod.SelectedData.ID != retailGroup.ValidationPeriod) return true;
            if ((decimal)ntbTareWeight.Value != retailGroup.TareWeight) return true;

            return false;
        }

        public bool SaveData()
        {
            retailGroup.RetailDepartmentID = cmbRetailDepartment.SelectedData.ID;
            retailGroup.RetailDepartmentMasterID = cmbRetailDepartment.SelectedData.ID == "" ? Guid.Empty:
                Providers.RetailDepartmentData.GetMasterID(PluginEntry.DataModel,
                retailGroup.RetailDepartmentID);
            retailGroup.ItemSalesTaxGroupId = cmbItemSalesTaxGroup.SelectedData.ID;
            retailGroup.ProfitMargin = ntbProfitMargin.FullPrecisionValue;
            retailGroup.ValidationPeriod = (string)cmbValidationPeriod.SelectedData.ID;
            retailGroup.ValidationPeriodDescription = cmbValidationPeriod.SelectedData.Text;
            retailGroup.TareWeight = Convert.ToInt32(ntbTareWeight.Value);

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

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }


        private void cmbRetailDepartment_RequestData(object sender, EventArgs e)
        {
            cmbRetailDepartment.SetData(Providers.RetailDepartmentData.GetList(PluginEntry.DataModel, RetailDepartment.SortEnum.Description),null);
        }

        private void cmbItemSalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbItemSalesTaxGroup.SetData(Providers.ItemSalesTaxGroupData.GetList(PluginEntry.DataModel),null);
        }

        private void btnEditRetailDepartment_Click(object sender, EventArgs e)
        {
            if (cmbRetailDepartment.SelectedData != null)
            {
                PluginOperations.ShowRetailDepartmentListView(cmbRetailDepartment.SelectedData.ID);
            }
            else
            {
                PluginOperations.ShowRetailDepartmentListView(this, EventArgs.Empty);
            }
        }

        private void btnEditItemSalesTaxGroup_Click(object sender, EventArgs e)
        {
            if (salesTaxGroupEditor.IsAlive)
            {
                ((IPlugin)salesTaxGroupEditor.Target).Message(this, "ViewItemSalesTaxGroups", cmbItemSalesTaxGroup.SelectedData.ID);
            }
        }

        private void cmbValidationPeriod_RequestData(object sender, EventArgs e)
        {
            cmbValidationPeriod.SetData(Providers.DiscountPeriodData.GetList(PluginEntry.DataModel), null);
        }

        private void btnEditValidationPeriod_Click(object sender, EventArgs e)
        {
            if (ValditionPeriodEditor.IsAlive)
            {
                ((IPlugin)ValditionPeriodEditor.Target).Message(this, "CanEditValidationPeriod", cmbValidationPeriod.SelectedData.ID);
            }
        }

        private void cmbValidationPeriod_RequestClear(object sender, EventArgs e)
        {
            cmbValidationPeriod.SelectedData = new DataEntity("","");
        }
    }
}
