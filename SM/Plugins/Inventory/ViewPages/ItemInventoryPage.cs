using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.ViewPlugins.Inventory.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    internal partial class ItemInventoryPage : UserControl, ITabView
    {
        RetailItem item;

        private Dictionary<string, object> viewStateData;

        private SiteServiceProfile siteServiceProfile;
        private ValueTuple<bool, string> connectedToSiteService;
        private bool hasPermissionViewOnHandInventory;

        IInventoryService service;

        public ItemInventoryPage(TabControl owner)
        {
            InitializeComponent();
            connectedToSiteService = ValueTuple.Create(false, "");
            hasPermissionViewOnHandInventory = PluginEntry.DataModel.HasPermission(Permission.ViewOnHandInventory);

            groupBoxInventoryOnHand.Visible = hasPermissionViewOnHandInventory;

            if (hasPermissionViewOnHandInventory)
            {
                connectedToSiteService = PluginOperations.TestSiteService(PluginEntry.DataModel, false);

                if (connectedToSiteService.Item1)
                {
                    siteServiceProfile = PluginOperations.GetSiteServiceProfile();
                }

                cmbRegion.SelectedData = new DataEntity("", Resources.AllRegions);
            }

            viewStateData = owner.ViewStateData;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ItemInventoryPage((TabControl) sender);
        }
    
        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)internalContext;

            if (!isRevert)
            {
                if (string.IsNullOrEmpty(item.InventoryUnitID.ToString()))
                {
                    btnEditInventoryUnits.Enabled = false;
                    cmbInventoryUnit.Enabled = true;
                }
                else
                {
                    btnEditInventoryUnits.Enabled = true;
                    cmbInventoryUnit.Enabled = false;
                }
            }

            // -------------------------------------------------------------------------
            cmbInventoryUnit.SelectedData = new DataEntity(item.InventoryUnitID, item.InventoryUnitName);
            cmbPurchaseUnit.SelectedData = new DataEntity(item.PurchaseUnitID, item.PurchaseUnitName);

            LoadItemsProcess();
        }

        public bool DataIsModified()
        {
            if (cmbInventoryUnit.SelectedData.ID != item.InventoryUnitID)
            {
                item.Dirty = true;
            }

            return item.Dirty ;
        }

        public bool SaveData()
        {
            item.InventoryUnitID = cmbInventoryUnit.SelectedData.ID;
            item.PurchaseUnitID = cmbPurchaseUnit.SelectedData.ID;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            // I check for changeIdentifier == RecordIdentifier.Empty for when you are posting a statement and updating inventory, but dont know 
            // which items you are updating.
            if (objectName == "ItemInventory" && (changeIdentifier == item.ID || changeIdentifier == RecordIdentifier.Empty))
            {
                LoadItemsProcess();
            }
            if (objectName == "ItemInventoryUnit" && changeIdentifier == item.ID && changeHint == DataEntityChangeType.Edit)
            {
                RetailItem changeItem = (RetailItem)param;
                cmbInventoryUnit.SelectedData = new DataEntity(changeItem.InventoryUnitID, changeItem.InventoryUnitName);
            }
            if (objectName == "ItemPurchaseUnit" && changeIdentifier == item.ID && changeHint == DataEntityChangeType.Edit)
            {
                RetailItem changeItem = (RetailItem)param;
                cmbPurchaseUnit.SelectedData = new DataEntity(changeItem.PurchaseUnitID, changeItem.PurchaseUnitName);
            }
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void LoadItemsProcess()
        {
            SpinnerDialog dlg = new SpinnerDialog(Resources.FewSeconds, LoadItems);
            dlg.ShowDialog();
        }

        private void LoadItems()
        {
            if (!hasPermissionViewOnHandInventory)
            {
                return;
            }

            if (!connectedToSiteService.Item1)
            {
                string message = connectedToSiteService.Item2;
                lblNoConnection.BringToFront();
                lblNoConnection.Text = message;
                return;
            }
            List<InventoryStatus> items;
            lvItemInventory.ClearRows();

            service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            RecordIdentifier storeID = (PluginEntry.DataModel.HasPermission(Permission.ViewInventoryForAllStores) || PluginEntry.DataModel.IsHeadOffice)
                ? RecordIdentifier.Empty
                : PluginEntry.DataModel.CurrentStoreID;

            items = item.ItemType == ItemTypeEnum.AssemblyItem
            ? service.GetInventoryListForAssemblyItemAndStore(PluginEntry.DataModel, siteServiceProfile,
                    item.ID, storeID, cmbRegion.SelectedData.ID, InventorySorting.Store, !lvItemInventory.SortedAscending, false)
            : service.GetInventoryListForItemAndStore(PluginEntry.DataModel, siteServiceProfile,
                    item.ID, storeID, cmbRegion.SelectedData.ID, InventorySorting.Store, !lvItemInventory.SortedAscending, false);

            Style boldCellStyle = new Style(lvItemInventory.DefaultStyle);
            boldCellStyle.Font = new Font(lvItemInventory.DefaultStyle.Font, FontStyle.Bold);

            Style rowStyle;
            foreach (InventoryStatus itemStatus in items)
            {
                Row row = new Row();

                rowStyle = (PluginEntry.DataModel.CurrentStoreID == itemStatus.StoreID
                    ? boldCellStyle
                    : lvItemInventory.DefaultStyle);

                if (item.ItemType == ItemTypeEnum.AssemblyItem && itemStatus.HasHeaderItem)
                {
                    row.AddCell(new Cell(itemStatus.StoreName, rowStyle));
                    row.AddText("N/A");
                    row.AddText("N/A");
                    row.AddText("N/A");
                    row.AddText("N/A");
                    row.AddCell(new IconToolTipCell(Resources.info_provider_16, Resources.AssemblyHeaderItemTooltip));
                }
                else
                {

                    itemStatus.ReservedQuantity *= -1;
                    itemStatus.ParkedQuantity *= -1;

                    row.AddCell(new Cell(itemStatus.StoreName, rowStyle));
                    row.AddCell((itemStatus.InventoryQuantity != 0)
                        ? new NumericCell(itemStatus.InventoryQuantityFormatted, rowStyle, itemStatus.InventoryQuantity)
                        : new Cell("-", rowStyle));
                    row.AddCell((itemStatus.OrderedQuantity != 0)
                        ? new NumericCell(itemStatus.OrderedQuantity.FormatWithLimits(itemStatus.QuantityLimiter), rowStyle, itemStatus.OrderedQuantity)
                        : new Cell("-", rowStyle));
                    row.AddCell((itemStatus.ReservedQuantity != 0)
                        ? new NumericCell(itemStatus.ReservedQuantity.FormatWithLimits(itemStatus.QuantityLimiter), rowStyle, itemStatus.ReservedQuantity)
                        : new Cell("-", rowStyle));
                    row.AddCell((itemStatus.ParkedQuantity != 0)
                        ? new NumericCell(itemStatus.ParkedQuantity.FormatWithLimits(itemStatus.QuantityLimiter), rowStyle, itemStatus.ParkedQuantity)
                        : new Cell("-", rowStyle));

                    if (item.ItemType == ItemTypeEnum.AssemblyItem)
                    {
                        row.AddCell(new IconToolTipCell(Resources.info_provider_16, Resources.AssemblyInventoryOnHandTooltip));
                    }
                }

                lvItemInventory.AddRow(row);
            }

            service.Disconnect(PluginEntry.DataModel);

            lvItemInventory.AutoSizeColumns();
        }

        private void btnEditUnits_Click(object sender, EventArgs e)
        {
            ItemUnitDialog unitDialog = new ItemUnitDialog(item);

            if (unitDialog.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {

                item = PluginOperations.GetRetailItem(item.ID);
                if (item == null)
                {
                    return;
                }

                cmbInventoryUnit.SelectedData = new DataEntity(item.InventoryUnitID, item.InventoryUnitName);
                viewStateData["InventoryUnit"] = item.InventoryUnitID;
                LoadItemsProcess();
            }
        }

        private void cmbInventoryUnit_RequestData(object sender, EventArgs e)
        {
            cmbInventoryUnit.SetData(Providers.UnitData.GetList(PluginEntry.DataModel),null);             
        }
        
        private void cmbInventoryUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            viewStateData["InventoryUnit"] = item.InventoryUnitID;
        }

        private void cmbRegion_RequestData(object sender, EventArgs e)
        {
            List<DataLayer.BusinessObjects.StoreManagement.Region> regions = Providers.RegionData.GetList(PluginEntry.DataModel, DataLayer.BusinessObjects.StoreManagement.Region.SortEnum.Description, false);
            regions.Insert(0, new DataLayer.BusinessObjects.StoreManagement.Region("", Resources.AllRegions));
            cmbRegion.SetData(regions, null);
        }

        private void cmbRegion_SelectedDataChanged(object sender, EventArgs e)
        {
            LoadItemsProcess();
        }
    }
}