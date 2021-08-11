using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using System.Collections.Generic;
using LSOne.ViewPlugins.Replenishment.Properties;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Replenishment.Dialogs
{
    public partial class ItemStoreReplenishmentSettingDialog : DialogBase
    {
        private RecordIdentifier itemId;
        private ItemReplenishmentSetting setting;
        private Unit itemInventoryUnit;
        private List<RecordIdentifier> storesInUse;
        private bool deleting;

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public static ItemStoreReplenishmentSettingDialog CreateForAdding(RecordIdentifier itemId, List<RecordIdentifier> storesInUse)
        {
            return new ItemStoreReplenishmentSettingDialog(itemId, storesInUse, false);
        }

        public static ItemStoreReplenishmentSettingDialog CreateForEditing(RecordIdentifier itemId, RecordIdentifier settingId, List<RecordIdentifier> storesInUse)
        {
            return new ItemStoreReplenishmentSettingDialog(itemId, settingId, storesInUse);
        }

        public static ItemStoreReplenishmentSettingDialog CreateForDeleting(RecordIdentifier itemId)
        {
            return new ItemStoreReplenishmentSettingDialog(itemId, null, true);
        }

        private ItemStoreReplenishmentSettingDialog(RecordIdentifier itemId, List<RecordIdentifier> storesInUse, bool deleting)
        {
            InitializeComponent();

            this.deleting = deleting;

            this.storesInUse = storesInUse;

            InitializeComboBoxes();

            if (!PluginEntry.DataModel.IsHeadOffice)
            {
                cmbStore.SelectedData = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
                cmbStore.Enabled = false;

                if(itemId == RecordIdentifier.Empty)
                {
                    btnOK.Enabled = true;
                }
            }

            this.itemId = itemId;

            if (itemId != RecordIdentifier.Empty)
            {
                RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemId);
                itemInventoryUnit = Providers.UnitData.Get(PluginEntry.DataModel, item.InventoryUnitID);

                ntbReorderPoint.DecimalLetters = ntbMaximumInventory.DecimalLetters = itemInventoryUnit.Limit.Max;
                ntbReorderPoint.AllowDecimal = ntbMaximumInventory.AllowDecimal = itemInventoryUnit.Limit.Min != 0 || itemInventoryUnit.Limit.Max != 0;
                ntbReorderPoint.SetValueWithLimit(0, itemInventoryUnit.Limit);
                ntbMaximumInventory.SetValueWithLimit(0, itemInventoryUnit.Limit);
            }
            else
            {
                DecimalLimit limit = new DecimalLimit(0, 2);

                ntbReorderPoint.DecimalLetters = ntbMaximumInventory.DecimalLetters = 2;
                ntbReorderPoint.AllowDecimal = ntbMaximumInventory.AllowDecimal = true;

                ntbReorderPoint.SetValueWithLimit(0, limit);
                ntbMaximumInventory.SetValueWithLimit(0, limit);
            }

            if(deleting)
            {
                cmbReplenishmentMethod.Enabled = false;
                ntbReorderPoint.Enabled = false;
                ntbPurchaseOrderMultiple.Enabled = false;
                ntbMaximumInventory.Enabled = false;
                cmbPurchaseOrderMultipleRounding.Enabled = false;
                cmbBlocked.Enabled = false;

                Header = Properties.Resources.SelectStoreToDeleteOverwriteSettings;
            }


            
        }

        private ItemStoreReplenishmentSettingDialog(RecordIdentifier itemId, RecordIdentifier settingId, List<RecordIdentifier> storesInUse)
            : this(itemId, storesInUse,false)
        {
            try
            {
                setting = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetItemReplenishmentSetting(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), settingId,
                                true);

            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
            string replenishmentMethodText = setting.ReplenishmentMethodText;
            cmbReplenishmentMethod.SelectedIndex = cmbReplenishmentMethod.FindStringExact(replenishmentMethodText);

            ntbReorderPoint.SetValueWithLimit(setting.ReorderPoint, itemInventoryUnit.Limit);
            ntbMaximumInventory.SetValueWithLimit(setting.MaximumInventory, itemInventoryUnit.Limit);
            cmbStore.SelectedData = new DataEntity(setting.StoreId, setting.StoreName);
            ntbPurchaseOrderMultiple.Value = setting.PurchaseOrderMultiple;
            cmbPurchaseOrderMultipleRounding.SelectedIndex = (int)setting.PurchaseOrderMultipleRounding;
            cmbBlocked.SelectedIndex = (int)setting.BlockedForReplenishment;
            dtpBlockedDate.Value = setting.BlockingDate;
        }

        private void InitializeComboBoxes()
        {
            cmbReplenishmentMethod.Items.Add(
                new DataEntity(
                    (int)ReplenishmentMethodEnum.StockLevel,
                    ItemReplenishmentSetting.ReplenishmentMethodToText(ReplenishmentMethodEnum.StockLevel)));
            cmbReplenishmentMethod.Items.Add(
                new DataEntity(
                    (int)ReplenishmentMethodEnum.LikeForLike,
                    ItemReplenishmentSetting.ReplenishmentMethodToText(ReplenishmentMethodEnum.LikeForLike)));
            cmbReplenishmentMethod.SelectedIndex = 0;

            cmbBlocked.Items.Add(
                new DataEntity(
                    (int)BlockedForReplenishmentEnum.NotBlocked,
                    ItemReplenishmentSetting.BlockedForReplenishmentToText(BlockedForReplenishmentEnum.NotBlocked)));
            cmbBlocked.Items.Add(
                new DataEntity(
                    (int)BlockedForReplenishmentEnum.BlockedForReplenishment,
                    ItemReplenishmentSetting.BlockedForReplenishmentToText(BlockedForReplenishmentEnum.BlockedForReplenishment)));
            cmbBlocked.SelectedIndex = 0;

            cmbPurchaseOrderMultipleRounding.Items.Add(
                new DataEntity(
                    (int)PurchaseOrderMultipleRoundingEnum.Nearest,
                    ItemReplenishmentSetting.PurchaseOrderMultipleRoundingToText(PurchaseOrderMultipleRoundingEnum.Nearest)));
            cmbPurchaseOrderMultipleRounding.Items.Add(
                new DataEntity(
                    (int)PurchaseOrderMultipleRoundingEnum.Down,
                    ItemReplenishmentSetting.PurchaseOrderMultipleRoundingToText(PurchaseOrderMultipleRoundingEnum.Down)));
            cmbPurchaseOrderMultipleRounding.Items.Add(
                new DataEntity(
                    (int)PurchaseOrderMultipleRoundingEnum.Up,
                    ItemReplenishmentSetting.PurchaseOrderMultipleRoundingToText(PurchaseOrderMultipleRoundingEnum.Up)));
            cmbPurchaseOrderMultipleRounding.SelectedIndex = 0;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (deleting)
            {
                btnOK.Enabled = (cmbStore.SelectedData != null);
            }
            else
            {
                btnOK.Enabled = cmbStore.SelectedData != null &&
                                cmbStore.SelectedData.ID != "" &&
                                ntbMaximumInventory.Value != 0;

                if (setting != null)
                {
                    btnOK.Enabled = btnOK.Enabled &&
                                    (
                                    cmbStore.SelectedData.ID != setting.StoreId ||
                                    (int)((DataEntity)cmbReplenishmentMethod.SelectedItem).ID != (int)setting.ReplenishmentMethod ||
                                    ntbReorderPoint.Value != (double)setting.ReorderPoint ||
                                    ntbMaximumInventory.Value != (double)setting.MaximumInventory ||
                                    ntbPurchaseOrderMultiple.Value != setting.PurchaseOrderMultiple ||
                                    cmbPurchaseOrderMultipleRounding.SelectedIndex != (int)setting.PurchaseOrderMultipleRounding ||
                                    cmbBlocked.SelectedIndex != (int)setting.BlockedForReplenishment ||
                                    dtpBlockedDate.Value != setting.BlockingDate
                                    );
                }
            }
        }

        public ItemReplenishmentSetting ItemReplenishmentSetting
        {
            get
            {
                return setting;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (setting == null)
            {
                setting = new ItemReplenishmentSetting();
                setting.ID = Guid.NewGuid();
                setting.ItemId = itemId;
            }

            setting.StoreId = cmbStore.SelectedData.ID;
            setting.StoreName = cmbStore.SelectedData.Text;

            if (!deleting)
            {
                setting.ReorderPoint = (decimal)ntbReorderPoint.Value;
                setting.MaximumInventory = (decimal)ntbMaximumInventory.Value;
                setting.ReplenishmentMethod = (ReplenishmentMethodEnum)(int)((DataEntity)cmbReplenishmentMethod.SelectedItem).ID;
                setting.PurchaseOrderMultiple = (int)ntbPurchaseOrderMultiple.Value;
                setting.PurchaseOrderMultipleRounding =
                    (PurchaseOrderMultipleRoundingEnum)cmbPurchaseOrderMultipleRounding.SelectedIndex;
                setting.BlockedForReplenishment = (BlockedForReplenishmentEnum)cmbBlocked.SelectedIndex;
                setting.BlockingDate = dtpBlockedDate.Value;

                // We only save when in single edit mode
                if (itemId != RecordIdentifier.Empty)
                {
                    try
                    {
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveItemReplenishmentSetting(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), setting,
                               true);
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                        return;
                    }
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this,
                                                                            DataEntityChangeType.Edit,
                                                                            "ItemStoreReplenishmentSetting",
                                                                            setting.ID,
                                                                            null);
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> stores = Providers.StoreData.GetList(PluginEntry.DataModel);

            if (storesInUse != null)
            {
                for (int i = 0; i < stores.Count; i++)
                {
                    for (int n = 0; n < storesInUse.Count; n++)
                    {
                        if (stores[i].ID == storesInUse[n])
                        {
                            stores.RemoveAt(i);
                            i--;
                            break;
                        }
                    }
                }
            }

            cmbStore.SetData(stores, null);
        }

        private void cmbReplenishmentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DataEntity)cmbReplenishmentMethod.SelectedItem).ID ==
                (int)ReplenishmentMethodEnum.LikeForLike)
            {
                ntbReorderPoint.Enabled = false;
                ntbMaximumInventory_TextChanged(sender, e);
            }
            else
            {
                ntbReorderPoint.Enabled = true;
            }

            CheckEnabled(sender, e);
        }

        private void ntbMaximumInventory_TextChanged(object sender, EventArgs e)
        {
            if (((DataEntity)cmbReplenishmentMethod.SelectedItem).ID ==
                (int)ReplenishmentMethodEnum.LikeForLike)
            {
                if (itemId == RecordIdentifier.Empty)
                {
                    // Multiedit
                    DecimalLimit limit = new DecimalLimit(0, 2);

                    ntbReorderPoint.SetValueWithLimit((decimal)Math.Max(ntbMaximumInventory.Value - 1, 0), limit);
                }
                else
                {
                    ntbReorderPoint.SetValueWithLimit((decimal)Math.Max(ntbMaximumInventory.Value - 1, 0), itemInventoryUnit.Limit);
                }
            }

            CheckEnabled(sender, e);
        }

        private void cmbBlocked_SelectedIndexChanged(object sender, EventArgs e)
        {
            var blocking = (BlockedForReplenishmentEnum)(int)((DataEntity)cmbBlocked.SelectedItem).ID;
            switch (blocking)
            {
                case BlockedForReplenishmentEnum.NotBlocked:
                    dtpBlockedDate.Enabled = false;
                    break;
                case BlockedForReplenishmentEnum.BlockedForReplenishment:
                    dtpBlockedDate.Enabled = true;
                    break;
            }

            CheckEnabled(sender, e);
        }
    }
}
