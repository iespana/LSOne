using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.RetailItems.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    public partial class ItemTypeDialog : DialogBase
    {
        /// <summary>
        /// The ID of the current item.
        /// </summary>
        private RecordIdentifier itemID;
        /// <summary>
        /// The current item type of the item.
        /// </summary>
        private ItemTypeEnum itemType;
        /// <summary>
        /// The total number of inventory movements in InventTrans.
        /// </summary>
        private long ledgerLinesCount;

        private readonly IConnectionManager connection;

        private SiteServiceProfile siteServiceProfile;
        private DecimalLimit quantityLimiter;
        private RecordIdentifier newItemTypeId;

        /// <summary>
        /// The new, selected <see cref="ItemTypeEnum">itemType</see>.
        /// </summary>
        /// <remarks>
        /// Property does not directly expose the enumeration type because the default (implicit) value is the first one defined in the enumeration.
        /// </remarks>
        public RecordIdentifier NewItemTypeId
        {
            get { return newItemTypeId ?? RecordIdentifier.Empty; }
        }

        public ItemTypeDialog(IConnectionManager connection, RecordIdentifier originalItemID, ItemTypeEnum currentItemType)
            : this(connection)
        {
            itemID = originalItemID;
            itemType = currentItemType;
        }

        public ItemTypeDialog(IConnectionManager connection)
        {
            InitializeComponent();

            this.connection = connection;
            this.siteServiceProfile = PluginOperations.GetSiteServiceProfile();

            if (connection != null)
            {
                this.quantityLimiter = connection.GetDecimalSetting(DecimalSettingEnum.Quantity);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.CenterToParent(); //force form centering

            //configure item type combobox
            var itemTypes = new object[]
            {
                    ItemTypeHelper.ItemTypeToString(ItemTypeEnum.Item),
                    ItemTypeHelper.ItemTypeToString(ItemTypeEnum.Service),
                    ItemTypeHelper.ItemTypeToString(ItemTypeEnum.AssemblyItem)
            };
            cmbItemType.Items.AddRange(itemTypes);
            cmbItemType.SelectedItem = ItemTypeHelper.ItemTypeToString(itemType);

            if (connection == null || !TestSiteService())
            {
                grbIoH.Visible = false;
                grpNotification.BringToFront();
                lblNotification.Text = Resources.NoConnectionToSiteServiceHeader + Environment.NewLine + Environment.NewLine + Resources.NoConnectionToSiteServiceText;
                return;
            }

            lblItemType.Enabled = true;
            cmbItemType.Enabled = true;
            LoadData();

            if (ledgerLinesCount != 0 && !connection.HasPermission(DataLayer.BusinessObjects.Permission.ManageItemTypes))
            {
                grbIoH.Visible = false;
                grpNotification.BringToFront();
                lblNotification.Text = Resources.NoPermissionForItemType;
            }
            else
            {
                btnOK.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var selectedItemType = ItemTypeHelper.StringToItemType((string)cmbItemType.SelectedItem);
            bool changeAccepted = false;

            //itemtype was changed - save the item, zero out inventory and block replenishment if needed
            if (selectedItemType != itemType)
            {
                IPlugin replenishmentPlugin = PluginEntry.Framework.FindImplementor(this, "CanBlockItemReplenishment", null);
                var service = Services.Interfaces.Services.SiteServiceService(connection);

                 DialogResult confirmationResult;
                if (selectedItemType == ItemTypeEnum.Service)
                {
                    confirmationResult = MessageDialog.Show(
                                                            (replenishmentPlugin != null
                                                                ?Resources.InventoryAdjustmentsToZeroOutInventoryWithReplenishment
                                                                : Resources.InventoryAdjustmentsToZeroOutInventory)
                                                            + Environment.NewLine
                                                            + Resources.NewItemTypeServiceQuestion,
                                                        Resources.NewItemTypeServiceTitle,
                                                        MessageBoxButtons.YesNo,
                                                       MessageBoxIcon.Question);

                    if (confirmationResult == DialogResult.Cancel)
                    {
                        btnCancel.Focus();
                        return;
                    }
                    else
                    {
                        service.SaveItemType(connection, siteServiceProfile, itemID, selectedItemType, true);
                        DataLayer.DataProviders.Providers.RetailItemData.UpdateItemType(PluginEntry.DataModel, itemID, selectedItemType);

                        var stores = DataLayer.DataProviders.Providers.StoreData.GetList(PluginEntry.DataModel);
                        foreach (var store in stores)
                        {
                            object[] parameters = new object[]
                            {
                                itemID,
                                store.ID,
                                new RecordIdentifier(AdjustmentReasonConstants.ItemIsServiceReasonID)
                            };
                            var plugin = PluginEntry.Framework.FindImplementor(this, "CanZeroOutInventory", parameters);
                            if (plugin != null)
                            {
                                plugin.Message(this, "ZeroOutInventory", parameters);
                            }
                        }

                        changeAccepted = true;
                    }

                    if(replenishmentPlugin != null)
                    {
                        replenishmentPlugin.Message(this, "BlockItemReplenishmentThroughService", itemID);
                    }
                }
                else
                {
                    confirmationResult = MessageDialog.Show(
                                                        (replenishmentPlugin != null ? Resources.NewItemTypeRetailText1 + System.Environment.NewLine : "")
                                                            + Resources.NewItemTypeRetailText2 + " " + Resources.NewItemTypeRetailText3,
                                                        Resources.NewItemTypeRetailTitle,
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Information);

                    if(confirmationResult == DialogResult.OK)
                    {
                        service.SaveItemType(connection, siteServiceProfile, itemID, selectedItemType, true);
                        DataLayer.DataProviders.Providers.RetailItemData.UpdateItemType(PluginEntry.DataModel, itemID, selectedItemType);
                        changeAccepted = true;
                    };
                }
            }

            if (changeAccepted)
            {
                newItemTypeId = new RecordIdentifier((int)selectedItemType);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "ItemType", newItemTypeId, null);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Tests the availability of the SiteService.
        /// </summary>
        /// <returns></returns>
        private bool TestSiteService()
        {
            bool serviceAvailable = siteServiceProfile != null;
            if (serviceAvailable)
            {
                IInventoryService service = (IInventoryService)connection.Service(ServiceType.InventoryService);
                ConnectionEnum result = service.TestConnection(connection,
                                                                siteServiceProfile.SiteServiceAddress,
                                                                (ushort)siteServiceProfile.SiteServicePortNumber);
                service.Disconnect(connection);

                serviceAvailable = (result == ConnectionEnum.Success);
            }

            return serviceAvailable;
        }

        /// <summary>
        /// Loads item ledger textbox and inventory on hand grid.
        /// </summary>
        /// <returns>The number of ledger lines (total number of inventory movements in InventTrans for the current item).</returns>
        private void LoadData()
        {
            IInventoryService service = (IInventoryService)connection.Service(ServiceType.InventoryService);

            if (!RecordIdentifier.IsEmptyOrNull(itemID))
            {
                cmbItemType.SelectedValue = itemType;
                //load item ledger lines                                
                ledgerLinesCount = Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).GetLedgerEntryCountForItem(PluginEntry.DataModel, siteServiceProfile, itemID, false);
                ntbLedgerLines.Value = ledgerLinesCount;

                //load inventory on hand grid.
                lvItemInventory.ClearRows();

                RecordIdentifier storeID = (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ViewInventoryForAllStores) || PluginEntry.DataModel.IsHeadOffice)
                                        ? RecordIdentifier.Empty
                                        : PluginEntry.DataModel.CurrentStoreID;

                List<InventoryStatus> items = service.GetInventoryListForItemAndStore(
                                                           connection,
                                                           siteServiceProfile,
                                                           itemID,
                                                           storeID,
                                                           RecordIdentifier.Empty,
                                                           InventorySorting.Store,
                                                           !lvItemInventory.SortedAscending,
                                                           false);

                Style boldCellStyle = new Style(lvItemInventory.DefaultStyle);
                boldCellStyle.Font = new Font(lvItemInventory.DefaultStyle.Font, FontStyle.Bold);

                Style rowStyle;
                foreach (InventoryStatus itemStatus in items)
                {
                    Row row = new Row();

                    rowStyle = (connection.CurrentStoreID == itemStatus.StoreID
                                    ? boldCellStyle
                                    : lvItemInventory.DefaultStyle);

                    itemStatus.ReservedQuantity *= -1;
                    itemStatus.ParkedQuantity *= -1;

                    row.AddCell(new Cell(itemStatus.StoreName, rowStyle));
                    row.AddCell((itemStatus.InventoryQuantity != 0)
                                    ? new NumericCell(itemStatus.InventoryQuantity.FormatWithLimits(quantityLimiter), rowStyle, itemStatus.InventoryQuantity)
                                    : new Cell("-", rowStyle));
                    row.AddCell((itemStatus.OrderedQuantity != 0)
                                    ? new NumericCell(itemStatus.OrderedQuantity.FormatWithLimits(quantityLimiter), rowStyle, itemStatus.OrderedQuantity)
                                    : new Cell("-", rowStyle));
                    row.AddCell((itemStatus.ReservedQuantity != 0)
                                    ? new NumericCell(itemStatus.ReservedQuantity.FormatWithLimits(quantityLimiter), rowStyle, itemStatus.ReservedQuantity)
                                    : new Cell("-", rowStyle));
                    row.AddCell((itemStatus.ParkedQuantity != 0)
                                ? new NumericCell(itemStatus.ParkedQuantity.FormatWithLimits(quantityLimiter), rowStyle, itemStatus.ParkedQuantity)
                                : new Cell("-", rowStyle));
                    lvItemInventory.AddRow(row);
                }

                lvItemInventory.AutoSizeColumns();
            }

            service.Disconnect(connection);
        }
    }
}
