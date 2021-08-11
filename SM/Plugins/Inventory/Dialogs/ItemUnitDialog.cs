using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class ItemUnitDialog : DialogBase
    {
        private RecordIdentifier itemID;
        private RecordIdentifier oldInventoryUnitID;
        private RecordIdentifier oldSalesUnitID;
        private RecordIdentifier oldPurchaseUnitID;
        private RetailItem item;
        private WeakReference unitAdder;
        
        protected ItemUnitDialog()
        {
            InitializeComponent();
           
        }

        public ItemUnitDialog(RetailItem item)
            :this()
        {
            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "CanAddUnits", null);
            unitAdder = (plugin != null) ? new WeakReference(plugin) : null;
            btnAddUnit1.Visible = btnAddUnit2.Visible = btnAddUnit3.Visible = (unitAdder != null);

            this.item = item;
            itemID = item.ID;
            tbItem.Text = string.Format("{0} ({1})", item, item.ID);

            oldInventoryUnitID = item.InventoryUnitID.PrimaryID;
            string oldInventoryUnitText = item.InventoryUnitName;
            cmbInventoryUnitNew.SelectedData = new DataEntity(oldInventoryUnitID, oldInventoryUnitText);
            tbInventoryUnitCurrent.Text = oldInventoryUnitText;

            oldSalesUnitID = item.SalesUnitID.PrimaryID;
            string oldSalesUnitText = item.SalesUnitName;
            cmbSalesUnitNew.SelectedData = new DataEntity(oldSalesUnitID, oldSalesUnitText);
            tbSalesUnitCurrent.Text = oldSalesUnitText;


            oldPurchaseUnitID = item.PurchaseUnitID.PrimaryID;
            string oldPurchaseUnitText = item.PurchaseUnitName;
            cmbPurchaseUnitNew.SelectedData = new DataEntity(oldPurchaseUnitID, oldPurchaseUnitText);
            tbPurchaseUnitCurrent.Text = oldPurchaseUnitText;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // There has to be a unit conversion rule from newSalesUnit to newInventoryUnit and from newPurchaseUnit to newInventoryUnit (oldInventoryUnit if a new one hasn't been selected). 
            // If the user wants to update the inventory quantity using a conversion factor, then there has to exist a conversion rule from oldInventoryUnit to newInventory unit
            bool newInventoryUnitSelected = oldInventoryUnitID != cmbInventoryUnitNew.SelectedData.ID;

            var invUnitToUse = cmbInventoryUnitNew.SelectedData.ID;

            bool newSalesToInvExists = PluginOperations.UnitConversionExists(item, cmbSalesUnitNew.SelectedData.ID, RetailItem.UnitTypeEnum.Sales, invUnitToUse, newInventoryUnitSelected);
            bool newPurchaseToNewInvExists = true;
            if (cmbPurchaseUnitNew.SelectedData.ID != "")
            {
                newPurchaseToNewInvExists = PluginOperations.UnitConversionExists(item, cmbPurchaseUnitNew.SelectedData.ID, RetailItem.UnitTypeEnum.Purchase, invUnitToUse, newInventoryUnitSelected);
            }

            if (!newSalesToInvExists || !newPurchaseToNewInvExists)
            {
                return;
            }

            if (newInventoryUnitSelected)
            {
                var updateInventoryResult =
                    QuestionDialog.Show(Resources.UpdateInventoryQtyQuestion, Resources.UpdateInventoryQty, Resources.ChangeWithConversion,Resources.KeepUnchanged);

                if (updateInventoryResult == DialogResult.Yes)
                {
                    bool oldInvToNewInvExists = PluginOperations.UnitConversionExists(item, cmbInventoryUnitNew.SelectedData.ID, RetailItem.UnitTypeEnum.Inventory, oldInventoryUnitID);

                    if (!oldInvToNewInvExists)
                    {
                        return;
                    }

                    var inventoryUnitConversionID = new RecordIdentifier(itemID, new RecordIdentifier(oldInventoryUnitID, cmbInventoryUnitNew.SelectedData.ID));
                    var inventoryUnitConversion = Providers.UnitConversionData.Get(PluginEntry.DataModel, inventoryUnitConversionID);

                    try
                    {
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).UpdateInventoryUnit(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), itemID, inventoryUnitConversion.Factor, true);
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                        return;
                    }
                   
                }
            }

            // Finally we update the necessary units 
            if (oldSalesUnitID != cmbSalesUnitNew.SelectedData.ID)
            {
                Providers.RetailItemData.UpdateUnitID(
                        PluginEntry.DataModel,
                        itemID,
                        cmbSalesUnitNew.SelectedData.ID,
                        RetailItem.UnitTypeEnum.Sales);
            }
            if (oldInventoryUnitID != cmbInventoryUnitNew.SelectedData.ID)
            {
                Providers.RetailItemData.UpdateUnitID(
                        PluginEntry.DataModel,
                        itemID,
                        cmbInventoryUnitNew.SelectedData.ID,
                        RetailItem.UnitTypeEnum.Inventory);
            }

            if (oldPurchaseUnitID != cmbPurchaseUnitNew.SelectedData.ID && cmbPurchaseUnitNew.SelectedData.ID != RecordIdentifier.Empty)
            {
                Providers.RetailItemData.UpdateUnitID(
                        PluginEntry.DataModel,
                        itemID,
                        cmbPurchaseUnitNew.SelectedData.ID,
                        RetailItem.UnitTypeEnum.Purchase);
            }
            // Update the unit information of the item so that views will get updated data in NotifyDataChanged
            item.InventoryUnitID = cmbInventoryUnitNew.SelectedData.ID;
            item.InventoryUnitName = cmbInventoryUnitNew.SelectedData.Text;
            item.SalesUnitID = cmbSalesUnitNew.SelectedData.ID;
            item.SalesUnitName = cmbSalesUnitNew.SelectedData.Text;
            item.PurchaseUnitID = cmbPurchaseUnitNew.SelectedData.ID;
            item.PurchaseUnitName = cmbPurchaseUnitNew.SelectedData.Text;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "ItemInventoryUnit", item.ID, item);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "ItemSalesUnit", item.ID, item);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "ItemPurchaseUnit", item.ID, item);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = cmbSalesUnitNew.SelectedData.ID != oldSalesUnitID ||
                            cmbInventoryUnitNew.SelectedData.ID != oldInventoryUnitID ||
                            cmbPurchaseUnitNew.SelectedData.ID != oldPurchaseUnitID;

            DateTime? lastunposted =  Providers.TransactionData.UnpostedTransactionExists(PluginEntry.DataModel);

            if (lastunposted != null && cmbInventoryUnitNew.SelectedData.ID != oldInventoryUnitID)
            {
                MessageDialog.Show(
                    string.Format(
                        Resources.InventoryUnitChangedError,
                        lastunposted));
                btnOK.Enabled = false;
            }
        }

        private void cmbSalesUnitNew_RequestData(object sender, EventArgs e)
        {
            cmbSalesUnitNew.SetData(Providers.UnitData.GetAllUnits(PluginEntry.DataModel), null);
        }

        private void cmbInventoryUnitNew_RequestData(object sender, EventArgs e)
        {
            cmbInventoryUnitNew.SetData(Providers.UnitData.GetAllUnits(PluginEntry.DataModel), null);
        }

        private void btnAddUnit_Click(object sender, EventArgs e)
        {
            if (unitAdder.IsAlive)
            {
                ((IPlugin)unitAdder.Target).Message(this, "AddUnits", null);
            }
        }

        private void cmbPurchaseUnitNew_RequestData(object sender, EventArgs e)
        {
            cmbPurchaseUnitNew.SetData(Providers.UnitData.GetAllUnits(PluginEntry.DataModel), null);
        }
    }
}
