using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Replenishment.Properties;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Replenishment.Dialogs
{
    public partial class NewInventoryTemplateDialog : DialogBase
    {
        private RecordIdentifier inventoryTemplateID;

        public NewInventoryTemplateDialog()
        {
            inventoryTemplateID = null;

            InitializeComponent();

            DataEntitySelectionList selectionList;

            if (!DesignMode)
            {
                var selectedStores = Providers.StoreData.GetList(PluginEntry.DataModel);
                selectionList = new DataEntitySelectionList(selectedStores);
                cmbStore.SelectedData = selectionList;
                cmbCopyExisting.SelectedData = new DataEntity("", "");

                cmbTemplateType.Items.Add(TemplateEntryTypeEnumHelper.ToString(TemplateEntryTypeEnum.PurchaseOrder));
                cmbTemplateType.Items.Add(TemplateEntryTypeEnumHelper.ToString(TemplateEntryTypeEnum.StockCounting));
                cmbTemplateType.Items.Add(TemplateEntryTypeEnumHelper.ToString(TemplateEntryTypeEnum.TransferStock));
                cmbTemplateType.SelectedIndex = 0;
            }

            if (!PluginEntry.DataModel.IsHeadOffice)
            {
                var list = new List<DataEntity>();
                var storeEntity = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
                list.Add(storeEntity);
                selectionList = new DataEntitySelectionList(list);
                selectionList.SetSelected(storeEntity.ID, true);
                cmbStore.SelectedData = selectionList;
                cmbStore.Enabled = false;
                chkAllStores.Enabled = false;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (PluginEntry.DataModel.IsHeadOffice)
            {
                var selectedStores = ((DataEntitySelectionList)cmbStore.SelectedData).GetSelectedItems();
                btnOK.Enabled = tbDescription.Text != "" && (chkAllStores.Checked || (selectedStores != null && selectedStores.Count > 0));
            }
            else
            {
                btnOK.Enabled = tbDescription.Text != "";
            }
        }

        public RecordIdentifier InventoryTemplateID
        {
            get
            {
                return inventoryTemplateID;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            InventoryTemplate template = new InventoryTemplate();
            template.Text = tbDescription.Text;
            template.AllStores = chkAllStores.Checked;
            template.TemplateEntryType = GetTemplateEntryType();
            inventoryTemplateID = PluginOperations.CreateInventoryTemplate(template, chkAllStores.Checked,((DataEntitySelectionList)cmbStore.SelectedData).GetSelectedItems(), cmbCopyExisting.SelectedData.ID);

            DialogResult = DialogResult.OK;
            Close();
        }

        private TemplateEntryTypeEnum GetTemplateEntryType()
        {
            if (cmbTemplateType.SelectedIndex == 0)
            {
                return TemplateEntryTypeEnum.PurchaseOrder;
            }
            else if (cmbTemplateType.SelectedIndex == 1)
            {
                return TemplateEntryTypeEnum.StockCounting;
            }
            else if (cmbTemplateType.SelectedIndex == 2)
            {
                return TemplateEntryTypeEnum.TransferStock;
            }

            return TemplateEntryTypeEnum.PurchaseOrder;
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> stores = Providers.StoreData.GetList(PluginEntry.DataModel);

            stores.Insert(0, new DataEntity("", Resources.AllStores));

            cmbStore.SetData(stores, null);
        }

        private void cmbStore_RequestClear(object sender, EventArgs e)
        {
            (cmbStore.SelectedData as DataEntitySelectionList).SelectNone();
        }

        private void cmbCopyExisting_RequestData(object sender, EventArgs e)
        {
            try
            {
                cmbCopyExisting.SetData(
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), new InventoryTemplateListFilter(), true),
                    null);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cmbStore.Enabled = !chkAllStores.Checked;
            CheckEnabled(sender,e);
        }

        private void cmbStore_DropDown(object sender, DropDownEventArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel((DataEntitySelectionList)((DualDataComboBox)sender).SelectedData);
            }
        }
    }
}