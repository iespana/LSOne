using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Replenishment.ViewPages
{
    public partial class InventoryTemplateWorksheetSettingsPage : UserControl, ITabView
    {
        InventoryTemplate template;
        bool readOnly;

        public InventoryTemplateWorksheetSettingsPage(TabControl owner)
            : this()
        {
            
        }

        public InventoryTemplateWorksheetSettingsPage()
        {
            DoubleBuffered = true;

            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new InventoryTemplateWorksheetSettingsPage((TabControl)sender);
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            bool isDirty;

            isDirty = chkCalculateSuggestedQuantity.Checked != template.CalculateSuggestedQuantity ||
                      chkSetQuantityToSuggestedQuantity.Checked != template.SetQuantityToSuggestedQuantity ||
                      chkDisplayReorderPoint.Checked != template.DisplayReorderPoint ||
                      chkDisplayMaximumInventory.Checked != template.DisplayMaximumInventory ||
                      chkAddLinesWithZeroSuggestedQuantity.Checked != template.AddLinesWithZeroSuggestedQuantity ||
                      cmbDefaultVendor.SelectedData.ID != template.DefaultVendor ||
                      chkCreateGoodsReceiving.Checked != template.CreateGoodsReceivingDocument ||
                      chkPopulateGoodsReceiving.Checked != template.AutoPopulateGoodsReceivingDocument;

            template.Dirty |= isDirty;

            return isDirty;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            template = (InventoryTemplate)((List<object>)internalContext)[0];

            chkCalculateSuggestedQuantity.Checked = template.CalculateSuggestedQuantity;
            chkSetQuantityToSuggestedQuantity.Checked = template.SetQuantityToSuggestedQuantity;
            chkDisplayReorderPoint.Checked = template.DisplayReorderPoint;
            chkDisplayMaximumInventory.Checked = template.DisplayMaximumInventory;
            chkAddLinesWithZeroSuggestedQuantity.Checked = template.AddLinesWithZeroSuggestedQuantity;
            chkPopulateGoodsReceiving.Checked = template.AutoPopulateGoodsReceivingDocument;
            chkCreateGoodsReceiving.Checked = template.CreateGoodsReceivingDocument;
            cmbDefaultVendor.SelectedData = Providers.VendorData.Get(PluginEntry.DataModel, template.DefaultVendor) ?? new DataEntity(RecordIdentifier.Empty, "");

            readOnly = (bool)((List<object>)internalContext)[1];

            if (readOnly)
            {
                chkAddLinesWithZeroSuggestedQuantity.Enabled
                    = chkCalculateSuggestedQuantity.Enabled
                    = chkDisplayMaximumInventory.Enabled
                    = chkDisplayReorderPoint.Enabled
                    = chkSetQuantityToSuggestedQuantity.Enabled
                    = cmbDefaultVendor.Enabled
                    = chkCreateGoodsReceiving.Enabled
                    = chkPopulateGoodsReceiving.Enabled
                    = false;
            }
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {

        }

        public bool SaveData()
        {
            if (template.Dirty)
            {
                template.CalculateSuggestedQuantity = chkCalculateSuggestedQuantity.Checked;
                template.SetQuantityToSuggestedQuantity = chkSetQuantityToSuggestedQuantity.Checked;
                template.DisplayReorderPoint = chkDisplayReorderPoint.Checked;
                template.DisplayMaximumInventory = chkDisplayMaximumInventory.Checked;
                template.AddLinesWithZeroSuggestedQuantity = chkAddLinesWithZeroSuggestedQuantity.Checked;
                template.DefaultVendor = cmbDefaultVendor.SelectedData.ID;
                template.CreateGoodsReceivingDocument = chkCreateGoodsReceiving.Checked;
                template.AutoPopulateGoodsReceivingDocument = chkPopulateGoodsReceiving.Checked;
            }

            return true;
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void chkCalculateSuggestedQuantity_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkCalculateSuggestedQuantity.Checked)
            {
                chkSetQuantityToSuggestedQuantity.Checked = false;
            }
        }

        private void cmbDefaultVendor_RequestClear(object sender, EventArgs e)
        {

        }

        private void cmbDefaultVendor_RequestData(object sender, EventArgs e)
        {
            cmbDefaultVendor.SetData(Providers.VendorData.GetList(PluginEntry.DataModel), null);
        }

        private void chkCreateGoodsReceiving_CheckedChanged(object sender, EventArgs e)
        {
            if(!readOnly)
            {
                chkPopulateGoodsReceiving.Enabled = chkCreateGoodsReceiving.Checked;

                if (!chkPopulateGoodsReceiving.Enabled)
                {
                    chkPopulateGoodsReceiving.Checked = false;
                }
            }
        }
    }
}