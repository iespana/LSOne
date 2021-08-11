using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class StoreTransferSendingItemDialog : DialogBase
    {
        private bool showInventory;
        private const int expandedDialogSize = 545;
        private const int collapsedDialogSize = 320;

        private DecimalLimit quantityLimiter;
        private RecordIdentifier inventoryTransferId;
        private RecordIdentifier defaultRegionID;
        private RecordIdentifier sendingStoreID;
        private InventoryTransferOrderLine inventoryTransferOrderLine;
        private InventoryTransferRequestLine inventoryTransferRequestLine;
        private StoreTransferTypeEnum transferType;
        private RetailItem retailItem;
        private List<Unit> units;
        private Unit previousUnit;
        private List<DataLayer.BusinessObjects.StoreManagement.Region> regions;
        private decimal availableInventory;

        private SiteServiceProfile siteServiceProfile;
        private IInventoryService inventoryService;

        private bool lockEvent;
        private bool enterPressed;
        private bool variantWantsFocus;

        public StoreTransferSendingItemDialog(StoreTransferTypeEnum transferType, RecordIdentifier inventoryTransferID, RecordIdentifier sendingStoreId)
        {
            InitializeComponent();

            this.transferType = transferType;
            this.inventoryTransferId = inventoryTransferID;

            cmbItem.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
            txtBarcode.Tag = ControlTypeEnums.BarcodeSearch;

            this.Text = transferType == StoreTransferTypeEnum.Order ? Properties.Resources.TransferOrderLine : Properties.Resources.TransferRequestLine;
            lblQuantitySending.Text = transferType == StoreTransferTypeEnum.Order ? Properties.Resources.SendingQuantityLabel : Properties.Resources.RequestingQuantityLabel;

            units = Providers.UnitData.GetAllUnits(PluginEntry.DataModel);
            regions = Providers.RegionData.GetList(PluginEntry.DataModel, DataLayer.BusinessObjects.StoreManagement.Region.SortEnum.ID, false);
            quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            this.sendingStoreID = sendingStoreId;
            defaultRegionID = Providers.StoreData.Get(PluginEntry.DataModel, sendingStoreId).RegionID;

            DataLayer.BusinessObjects.StoreManagement.Region defaultRegion = regions.Find(x => x.ID == defaultRegionID);

            if (defaultRegion != null)
            {
                cmbRegion.SelectedData = new DataEntity(defaultRegion.ID, defaultRegion.Text);
            }

            ShowInventory = false;
            siteServiceProfile = PluginOperations.GetSiteServiceProfile();
        }

        public StoreTransferSendingItemDialog(InventoryTransferOrderLine orderLine, InventoryTransferOrder transferOrder) : this(StoreTransferTypeEnum.Order, transferOrder.ID, transferOrder.SendingStoreId)
        {
            this.inventoryTransferOrderLine = orderLine;
            chkCreateAnother.Visible = false;

            retailItem = PluginOperations.GetRetailItem(inventoryTransferOrderLine.ItemId);

            if (retailItem == null)
            {
                return;
            }

            if (RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
            {
                cmbItem.SelectedData = new DataEntity(retailItem.ID, retailItem.Text);
                cmbItem_SelectedDataChanged(cmbItem, null);

                BarCode bc = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, retailItem.ID);

                txtBarcode.Text = bc == null ? "" : (string)bc.ID;
            }
            else
            {
                RetailItem headerItem = PluginOperations.GetRetailItem(retailItem.HeaderItemID);
                if (headerItem == null)
                {
                    return;
                }

                cmbItem.SelectedData = new DataEntity(headerItem.ID, headerItem.Text);
                cmbVariant.SelectedData = new DataEntity(
                                        inventoryTransferOrderLine.ItemId,
                                        string.IsNullOrEmpty(inventoryTransferOrderLine.VariantName) ? retailItem.VariantName : inventoryTransferOrderLine.VariantName);

                BarCode bc = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, headerItem.ID);

                txtBarcode.Text = bc == null ? "" : (string)bc.ID;
            }

            cmbItem.Enabled = false;
            txtBarcode.Enabled = false;
            cmbUnit.SelectedData = new DataEntity(inventoryTransferOrderLine.UnitId, inventoryTransferOrderLine.UnitName);
            previousUnit = new Unit() 
            { 
                ID = inventoryTransferOrderLine.UnitId, 
                Text = inventoryTransferOrderLine.UnitName 
            };

            SetQuantityAllowDecimals(inventoryTransferOrderLine.QuantitySent);
        }

        public StoreTransferSendingItemDialog(InventoryTransferRequestLine requestLine, InventoryTransferRequest transferRequest) : this(StoreTransferTypeEnum.Request, transferRequest.ID, transferRequest.ReceivingStoreId)
        {
            this.inventoryTransferRequestLine = requestLine;
            chkCreateAnother.Visible = false;

            retailItem = PluginOperations.GetRetailItem(inventoryTransferRequestLine.ItemId);

            if (retailItem == null)
            {
                return;
            }

            if (RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
            {
                cmbItem.SelectedData = new DataEntity(retailItem.ID, retailItem.Text);
                cmbItem_SelectedDataChanged(cmbItem, null);

                BarCode bc = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, retailItem.ID);

                txtBarcode.Text = bc == null ? "" : (string)bc.ID;
            }
            else
            {
                RetailItem headerItem = PluginOperations.GetRetailItem(retailItem.HeaderItemID);
                if (headerItem == null)
                {
                    return;
                }

                cmbItem.SelectedData = new DataEntity(headerItem.ID, headerItem.Text);
                cmbVariant.SelectedData = new DataEntity(
                                        inventoryTransferRequestLine.ItemId,
                                        string.IsNullOrEmpty(inventoryTransferRequestLine.VariantName) ? retailItem.VariantName : inventoryTransferRequestLine.VariantName);

                BarCode bc = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, headerItem.ID);

                txtBarcode.Text = bc == null ? "" : (string)bc.ID;
            }

            SetQuantityAllowDecimals(requestLine.QuantityRequested);

            cmbItem.Enabled = false;
            txtBarcode.Enabled = false;
            cmbUnit.SelectedData = new DataEntity(inventoryTransferRequestLine.UnitId, inventoryTransferRequestLine.UnitName);
            previousUnit = new Unit()
            {
                ID = inventoryTransferRequestLine.UnitId,
                Text = inventoryTransferRequestLine.UnitName
            };

            SetQuantityAllowDecimals(inventoryTransferRequestLine.QuantityRequested);
        }

        public RecordIdentifier InventoryTransferLineID
        {
            get
            {
                if (inventoryTransferOrderLine != null)
                {
                    return inventoryTransferOrderLine.ID;
                }

                if (inventoryTransferRequestLine != null)
                {
                    return inventoryTransferRequestLine.ID;
                }

                return null;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public bool ShowInventory
        {
            get { return showInventory; }
            set
            {
                showInventory = value;
                pnlInventory.Visible = showInventory;
                SetDialogSize();
            }
        }

        private void SetDialogSize()
        {
            Size = new Size(Width, ShowInventory ? expandedDialogSize : collapsedDialogSize);

            cmbRegion.TabStop =
            lvItemInventory.TabStop = ShowInventory;
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = cmbItem.SelectedData.ID != "" &&
                            (cmbVariant.SelectedData.ID != "" || !cmbVariant.Enabled) &&
                            cmbUnit.SelectedData.ID != "" &&
                            ntbSendingQuantity.Value != 0;

            if (inventoryTransferOrderLine != null)
            {
                RecordIdentifier target = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant);
                btnOK.Enabled = btnOK.Enabled &&
                                (target != inventoryTransferOrderLine.ItemId ||
                                cmbUnit.SelectedData.ID != inventoryTransferOrderLine.UnitId ||
                                ntbSendingQuantity.Value != (double)inventoryTransferOrderLine.QuantitySent);
            }
            else if (inventoryTransferRequestLine != null)
            {
                RecordIdentifier target = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant);
                btnOK.Enabled = btnOK.Enabled &&
                                (target != inventoryTransferRequestLine.ItemId ||
                                cmbUnit.SelectedData.ID != inventoryTransferRequestLine.UnitId ||
                                ntbSendingQuantity.Value != (double)inventoryTransferRequestLine.QuantityRequested);
            }
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            ShowInventory = !ShowInventory;
            btnInventory.Text = ShowInventory ? Properties.Resources.Hide : Properties.Resources.Show;
            LoadInventoryInformation();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(Save())
            {
                if (chkCreateAnother.Visible && chkCreateAnother.Checked)
                {
                    cmbItem.SelectedData = new DataEntity("", "");
                    cmbVariant.SelectedData = new DataEntity("", "");
                    cmbUnit.SelectedData = new DataEntity("", "");
                    retailItem = null;
                    txtBarcode.Text = "";
                    ntbSendingQuantity.Value = 0;
                    lvItemInventory.ClearRows();
                    availableInventory = 0;
                    errorProvider1.Clear();

                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Add,
                        transferType == StoreTransferTypeEnum.Order ? "InventoryTransferOrderLine" : "InventoryTransferRequestLine", inventoryTransferId, null);

                    inventoryTransferRequestLine = null;
                    inventoryTransferOrderLine = null;
                }
                else
                {
                    DialogResult = DialogResult.OK;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", RecordIdentifier.Empty, null);

                    Close();
                }
            }
        }

        private bool Save()
        {
            try
            {
                RecordIdentifier target = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant);

                RetailItem retailItem = GetRetailItem(target);

                if (retailItem.IsAssemblyItem)
                {
                    return SaveAssemblyComponents(retailItem, (decimal)ntbSendingQuantity.Value, cmbUnit.SelectedDataID);
                }
                else
                {
                    return SaveLine(target);
                }
            }
            catch
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                DialogResult = DialogResult.Cancel;
                Close();
            }

            return true;
        }

        private RetailItem GetRetailItem(RecordIdentifier itemID)
        {
            errorProvider1.Clear();
            RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);

            if (item == null)
            {
                try
                {
                    item = Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).GetRetailItem(PluginEntry.DataModel,
                        PluginOperations.GetSiteServiceProfile(),
                        itemID,
                        true);
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                    return null;
                }
            }

            return item;
        }

        private bool SaveAssemblyComponents(RetailItem retailItem, decimal itemQuantity, RecordIdentifier itemUnitID)
        {
            bool lineSaved = false;

            var assemblyQuantity = Providers.UnitConversionData.ConvertQtyBetweenUnits(
                PluginEntry.DataModel, retailItem.ID, itemUnitID, retailItem.SalesUnitID, itemQuantity);

            var assembly = Providers.RetailItemAssemblyData.GetAssemblyForItem(
                PluginEntry.DataModel, retailItem.ID, sendingStoreID, false);

            if (assembly != null)
            {
                foreach (var component in Providers.RetailItemAssemblyComponentData.GetList(PluginEntry.DataModel, assembly.ID))
                {
                    var componentQuantity = component.Quantity * assemblyQuantity;

                    var componentItem = GetRetailItem(component.ItemID);
                    if (componentItem.IsAssemblyItem)
                    {
                        lineSaved |= SaveAssemblyComponents(componentItem, componentQuantity, component.UnitID);
                    }
                    else if(componentItem.ItemType != ItemTypeEnum.Service && componentItem.ItemType != ItemTypeEnum.MasterItem)
                    {
                        if (component.UnitID != componentItem.PurchaseUnitID)
                        {
                            componentQuantity = Providers.UnitConversionData.ConvertQtyBetweenUnits(
                                PluginEntry.DataModel, componentItem.ID, component.UnitID, componentItem.PurchaseUnitID, componentQuantity);
                        }

                        lineSaved |= SaveLine(componentItem.ID, true, componentQuantity, (string)componentItem.PurchaseUnitID);
                    }
                }
            }

            return lineSaved;
        }

        private bool SaveLine(RecordIdentifier target, bool isComponent = false, decimal componentQuantity = 1, string componenUnitID = "")
        {
            inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            InventoryTransferOrderLine orderLineCheck;
            InventoryTransferRequestLine requestLineCheck;
            bool newItem = false;
            bool addToExisting = false;
            bool newOrderLine = inventoryTransferOrderLine == null;
            bool newRequestLine = inventoryTransferRequestLine == null;
            decimal quantityToAdd = 0;
            if (transferType == StoreTransferTypeEnum.Order && newOrderLine)
            {
                InventoryTransferOrderLine checkLine = new InventoryTransferOrderLine
                {
                    InventoryTransferId = inventoryTransferId,
                    ItemId = target,
                    UnitId = !string.IsNullOrEmpty(componenUnitID) ? componenUnitID : cmbUnit.SelectedData.ID
                };

                orderLineCheck = inventoryService.GetTransferOrderLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), checkLine, true);

                if (orderLineCheck != null)
                {
                    if (QuestionDialog.Show(orderLineCheck.ItemName + " " + Properties.Resources.AlreadyIncludedInDocument + "\n\n" + Properties.Resources.AddQuantityToExistingItem) == DialogResult.Yes)
                    {
                        addToExisting = true;
                        inventoryTransferOrderLine = new InventoryTransferOrderLine();
                        inventoryTransferOrderLine.InventoryTransferId = inventoryTransferId;
                        inventoryTransferOrderLine.ID = orderLineCheck.ID;
                        quantityToAdd = orderLineCheck.QuantitySent;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    newItem = true;
                    inventoryTransferOrderLine = new InventoryTransferOrderLine();
                    inventoryTransferOrderLine.InventoryTransferId = inventoryTransferId;
                }
            }
            else if (transferType == StoreTransferTypeEnum.Request && newRequestLine)
            {
                InventoryTransferRequestLine checkLine = new InventoryTransferRequestLine
                {
                    InventoryTransferRequestId = inventoryTransferId,
                    ItemId = target,
                    UnitId = !string.IsNullOrEmpty(componenUnitID) ? componenUnitID : cmbUnit.SelectedData.ID
                };

                requestLineCheck = inventoryService.GetTransferRequestLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), checkLine, true);

                if (requestLineCheck != null)
                {
                    if (QuestionDialog.Show(requestLineCheck.ItemName + " " + Properties.Resources.AlreadyIncludedInDocument + "\n\n" + Properties.Resources.AddQuantityToExistingItem) == DialogResult.Yes)
                    {
                        addToExisting = true;
                        inventoryTransferRequestLine = new InventoryTransferRequestLine();
                        inventoryTransferRequestLine.InventoryTransferRequestId = inventoryTransferId;
                        inventoryTransferRequestLine.ID = requestLineCheck.ID;
                        quantityToAdd = requestLineCheck.QuantityRequested;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    newItem = true;
                    inventoryTransferRequestLine = new InventoryTransferRequestLine();
                    inventoryTransferRequestLine.InventoryTransferRequestId = inventoryTransferId;
                }
            }

            // Make sure we are not changing the item properties to item properties that already exist in the transfer
            if (!newItem && !addToExisting)
            {
                if (transferType == StoreTransferTypeEnum.Order)
                {
                    // If some of the item properties changed we need to check
                    if (inventoryTransferOrderLine.ItemId != target ||
                        inventoryTransferOrderLine.UnitId != cmbUnit.SelectedData.ID)
                    {
                        InventoryTransferOrderLine checkLine = new InventoryTransferOrderLine
                        {
                            InventoryTransferId = inventoryTransferId,
                            ItemId = target,
                            UnitId = !string.IsNullOrEmpty(componenUnitID) ? componenUnitID : cmbUnit.SelectedData.ID
                        };

                        orderLineCheck = inventoryService.GetTransferOrderLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), checkLine, true);

                        if (orderLineCheck != null)
                        {
                            if (QuestionDialog.Show(orderLineCheck.ItemName + " " + Properties.Resources.AlreadyIncludedInDocument + "\n\n" + Properties.Resources.AddQuantityToExistingItem) == DialogResult.Yes)
                            {
                                inventoryTransferOrderLine.ID = orderLineCheck.ID;
                                quantityToAdd = orderLineCheck.QuantitySent;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    // If some of the item properties changed we need to check
                    if (inventoryTransferRequestLine.ItemId != target ||
                        inventoryTransferRequestLine.UnitId != cmbUnit.SelectedData.ID)
                    {
                        InventoryTransferRequestLine checkLine = new InventoryTransferRequestLine
                        {
                            InventoryTransferRequestId = inventoryTransferId,
                            ItemId = target,
                            UnitId = !string.IsNullOrEmpty(componenUnitID) ? componenUnitID : cmbUnit.SelectedData.ID
                        };

                        requestLineCheck = inventoryService.GetTransferRequestLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), checkLine, true);

                        if (requestLineCheck != null)
                        {
                            if (QuestionDialog.Show(requestLineCheck.ItemName + " " + Properties.Resources.AlreadyIncludedInDocument + "\n\n" + Properties.Resources.AddQuantityToExistingItem) == DialogResult.Yes)
                            {
                                inventoryTransferRequestLine.ID = requestLineCheck.ID;
                                quantityToAdd = requestLineCheck.QuantityRequested;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            RecordIdentifier newLineID = RecordIdentifier.Empty;
            
            if (transferType == StoreTransferTypeEnum.Order)
            {
                inventoryTransferOrderLine.ItemId = target;
                inventoryTransferOrderLine.UnitId = isComponent ? componenUnitID : cmbUnit.SelectedData.ID;
                inventoryTransferOrderLine.QuantitySent = isComponent ? componentQuantity + quantityToAdd : (decimal)ntbSendingQuantity.Value + quantityToAdd;
                inventoryTransferOrderLine.QuantityReceived = 0;
                SaveTransferOrderLineResult result = inventoryService.SaveInventoryTransferOrderLine(PluginEntry.DataModel, siteServiceProfile, inventoryTransferOrderLine, false, out newLineID, true);

                if (result != SaveTransferOrderLineResult.Success)
                {
                    PluginOperations.ShowInventoryTransferLineErrorResultMessage(result, true);
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return false;
                }
            }
            else
            {
                inventoryTransferRequestLine.ItemId = target;
                inventoryTransferRequestLine.UnitId = isComponent ? componenUnitID : cmbUnit.SelectedData.ID;
                inventoryTransferRequestLine.QuantityRequested = isComponent ? componentQuantity + quantityToAdd : (decimal)ntbSendingQuantity.Value + quantityToAdd;
                SaveTransferOrderLineResult result = inventoryService.SaveInventoryTransferRequestLine(PluginEntry.DataModel, siteServiceProfile, inventoryTransferRequestLine, out newLineID, true);

                if (result != SaveTransferOrderLineResult.Success)
                {
                    PluginOperations.ShowInventoryTransferLineErrorResultMessage(result, false);
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return false;
                }
            }

            return true;
        }

        private void cmbRegion_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> regions = new List<DataEntity>();
            regions.Add(new DataEntity("", Properties.Resources.AllRegions));
            regions.AddRange(this.regions);
            cmbRegion.SetData(regions, null);
        }

        private void cmbRegion_SelectedDataChanged(object sender, EventArgs e)
        {
            LoadInventoryInformation();
        }

        private void LoadInventoryInformation()
        {
            lvItemInventory.ClearRows();

            try
            {
                if(retailItem != null)
                {
                    inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                    List<InventoryStatus> inventoryStatuses;

                    if (retailItem.IsAssemblyItem)
                    {
                        inventoryStatuses = inventoryService.GetInventoryListForAssemblyItemAndStore(PluginEntry.DataModel, siteServiceProfile, retailItem.ID, null, cmbRegion.SelectedData?.ID, DataLayer.DataProviders.Inventory.InventorySorting.Store, false, true);
                    }
                    else
                    {
                        inventoryStatuses = inventoryService.GetInventoryListForItemAndStore(PluginEntry.DataModel, siteServiceProfile, retailItem.ID, null, cmbRegion.SelectedData?.ID, DataLayer.DataProviders.Inventory.InventorySorting.Store, false, true);
                    }

                    InventoryStatus inventoryFromSendingStore = inventoryStatuses.Find(x => x.StoreID == sendingStoreID);

                    if(inventoryFromSendingStore != null)
                    {
                        availableInventory = inventoryFromSendingStore.InventoryQuantity;

                        //Move at top of the list
                        inventoryStatuses.Remove(inventoryFromSendingStore);
                        inventoryStatuses.Insert(0, inventoryFromSendingStore);
                    }
                    else
                    {
                        //Load inventory for sending store
                        availableInventory = inventoryService.GetInventoryOnHand(PluginEntry.DataModel, siteServiceProfile, retailItem.ID, sendingStoreID, true);
                    }

                    Style boldCellStyle = new Style(lvItemInventory.DefaultStyle);
                    boldCellStyle.Font = new Font(lvItemInventory.DefaultStyle.Font, FontStyle.Bold);

                    Style rowStyle;

                    foreach (InventoryStatus itemStatus in inventoryStatuses)
                    {
                        Row row = new Row();

                        rowStyle = (sendingStoreID == itemStatus.StoreID ? boldCellStyle : lvItemInventory.DefaultStyle);
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

            }
            catch
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
            }
        }

        private void cmbItem_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;

            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbItem.SelectedData).Text;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, true, initialSearchText, SearchTypeEnum.InventoryItems, new List<RecordIdentifier>(), textInitallyHighlighted, true);
        }

        private void cmbItem_SelectedDataChanged(object sender, EventArgs e)
        {
            if (sender is DualDataComboBox)
            {
                GetUnitInfoForItem((DualDataComboBox)sender, false);
            }
            if (ShowInventory)
            {
                LoadInventoryInformation();
            }

            SetQuantityAllowDecimals((decimal)ntbSendingQuantity.Value);
        }

        private void cmbVariant_SelectedDataChanged(object sender, EventArgs e)
        {
            if (sender is DualDataComboBox)
            {
                GetUnitInfoForItem((DualDataComboBox)sender, true);
            }

            SetQuantityAllowDecimals((decimal)ntbSendingQuantity.Value);
        }

        private void GetUnitInfoForItem(DualDataComboBox control, bool isVariant)
        {
            if (!(control is DualDataComboBox))
            {
                return;
            }

            retailItem = PluginOperations.GetRetailItem(control.SelectedData.ID);

            if (retailItem == null)
            {
                return;
            }

            cmbUnit.Enabled = true;
            lblUnit.Enabled = true;
            SetUnitForItem(retailItem.ID);
            if (!isVariant)
            {
                cmbVariant.Enabled = control.SelectedData.ID != "" && retailItem.ItemType == ItemTypeEnum.MasterItem;
                lblVariant.Enabled = cmbVariant.Enabled;
            }

            if (string.IsNullOrEmpty(retailItem.HeaderItemID.StringValue))
            {
                cmbVariant.SelectedData = new DataEntity("", "");
            }

            BarCode bc = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, retailItem.ID);

            txtBarcode.Text = bc == null ? "" : (string)bc.ID;

            CheckEnabled();
        }

        private void SetUnitForItem(RecordIdentifier itemID)
        {
            RecordIdentifier itemInventoryUnit = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemID, RetailItem.UnitTypeEnum.Inventory);

            Unit unit = units.FirstOrDefault(f => f.ID == itemInventoryUnit) ?? new Unit();
            cmbUnit.SelectedData = unit;
            previousUnit = unit;
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            RecordIdentifier itemInventoryUnit = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItem.SelectedData.ID, RetailItem.UnitTypeEnum.Inventory);

            IEnumerable<DataEntity>[] unitData = new IEnumerable<DataEntity>[]
                        {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel,cmbItem.SelectedData.ID,itemInventoryUnit),
                         units};

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbUnit.SelectedData.ID,
                unitData,
                new string[] { Properties.Resources.Convertible, Properties.Resources.All },
                200);

            cmbUnit.SetData(unitData, pnl);
        }

        private void cmbVariant_DropDown(object sender, DropDownEventArgs e)
        {
            List<RecordIdentifier> excludedItemIDs = new List<RecordIdentifier>();

            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbVariant.SelectedData).Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                retailItem.ItemType == ItemTypeEnum.MasterItem ?
                retailItem.MasterID :
                retailItem.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, excludedItemIDs, textInitallyHighlighted, true);
        }

        private void cmbUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            Unit unit = (Unit)cmbUnit.SelectedData;
            RecordIdentifier unitID = unit.ID;

            if (!CheckUnitConversion(unitID))
            {
                cmbUnit.SelectedData = previousUnit;
            }
            else if(ntbSendingQuantity.Value > 0)
            {
                ntbSendingQuantity_Leave(sender, e);
            }

            SetQuantityAllowDecimals((decimal)ntbSendingQuantity.Value);

            CheckEnabled();
        }

        private bool CheckUnitConversion(RecordIdentifier unitIDToCheck)
        {
            if (retailItem == null)
            {
                return false;
            }

            //If the unit selected is one of the units already on the item then there is no need to create a conversion rule
            if (cmbUnit.SelectedData != null)
            {
                Unit unit = (Unit)cmbUnit.SelectedData;
                if (retailItem.SalesUnitID == unit.ID || retailItem.InventoryUnitID == unit.ID || retailItem.PurchaseUnitID == unit.ID)
                {
                    return true;
                }
            }

            bool unitConversionExists = PluginOperations.UnitConversionWithInventoryUnitExists(new DataEntity(retailItem.ID, retailItem.Text), unitIDToCheck);
            if (!unitConversionExists)
            {
                MessageDialog.Show(Properties.Resources.UnitConversionRuleMissingAlert);
                return false;
            }

            return true;
        }

        private void CheckBarCode(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                return;
            }

            RecordIdentifier itemID = RecordIdentifier.Empty;
            IBarcodeService barcodeService = (IBarcodeService)PluginEntry.DataModel.Service(ServiceType.BarcodeService);
            if (barcodeService != null)
            {
                BarCode barCode = barcodeService.ProcessBarcode(PluginEntry.DataModel, BarCode.BarcodeEntryType.ManuallyEntered, txtBarcode.Text);
                if (barCode != null && barCode.InternalType == BarcodeInternalType.Item)
                {
                    itemID = barCode.ItemID;
                }
                else if (Providers.RetailItemData.Exists(PluginEntry.DataModel, txtBarcode.Text))
                {
                    itemID = txtBarcode.Text;
                }
            }
            else if (Providers.RetailItemData.Exists(PluginEntry.DataModel, txtBarcode.Text))
            {
                itemID = txtBarcode.Text;
            }
            string error = string.Empty;
            bool itemValid = PluginOperations.CheckRetailItem(itemID, out error);

            if (!itemValid)
            {
                errorProvider1.SetError(txtBarcode, error);
                if (enterPressed)
                {
                    txtBarcode.Focus();
                }
                enterPressed = false;
                return;
            }

            if (itemID != RecordIdentifier.Empty)
            {
                retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);

                cmbItem.SelectedData = new DataEntity(retailItem.ID, retailItem.Text);
                cmbItem_SelectedDataChanged(cmbItem, e);

                if (retailItem != null && !retailItem.InventoryExcluded)
                {
                    cmbItem.Select();
                }

                if (variantWantsFocus)
                {
                    cmbVariant.Focus();
                    variantWantsFocus = false;
                }
                else
                {
                    ntbSendingQuantity.Focus();
                }
            }
            else
            {
                cmbItem_SelectedDataChanged(sender, e);
            }
        }

        private void txtBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && ActiveControl is TextBox && (ActiveControl as TextBox).Tag != null && (ControlTypeEnums)(ActiveControl as TextBox).Tag == ControlTypeEnums.BarcodeSearch)
            {
                if (!string.IsNullOrEmpty(txtBarcode.Text))
                {
                    enterPressed = true;
                    cmbItem.Select();
                }
            }
        }

        private void txtBarcode_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBarcode.Text))
            {
                txtBarcode.SelectAll();
            }
        }

        private void txtBarcode_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBarcode.Text))
            {
                txtBarcode.SelectAll();
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Tab) || (e.KeyData == (Keys.Tab | Keys.Shift)) || e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                if (e.KeyData == (Keys.Tab | Keys.Shift))
                {
                    lockEvent = true;
                    chkCreateAnother.Select();
                }
                else
                {
                    cmbItem.Select();
                }
            }
        }

        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            if (btnCancel.Focused)
            {
                return;
            }

            if (!lockEvent)
            {
                lockEvent = true;
                CheckBarCode(sender, e);
            }
            lockEvent = false;
        }

        private void ntbSendingQuantity_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void ntbSendingQuantity_Leave(object sender, EventArgs e)
        {
            if(!ShowInventory && retailItem != null)
            {
                inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                availableInventory = inventoryService.GetInventoryOnHand(PluginEntry.DataModel, siteServiceProfile, retailItem.ID, sendingStoreID, true);
            }

            decimal compareQuantity = availableInventory;

            if(!RecordIdentifier.IsEmptyOrNull(cmbUnit.SelectedData.ID) && retailItem.InventoryUnitID != cmbUnit.SelectedData.ID)
            {
                compareQuantity = Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel, retailItem.ID, retailItem.InventoryUnitID, cmbUnit.SelectedData.ID, availableInventory);
            }

            if (retailItem != null && compareQuantity < (decimal)ntbSendingQuantity.Value)
            {
                errorProvider1.SetError(ntbSendingQuantity, transferType == StoreTransferTypeEnum.Order ? Properties.Resources.SendingQuantityHigherThanInventory : Properties.Resources.RequestingQuantityHigherThanInventory);
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        /// <summary>
        /// If the Unit allows decimals then the qty textbox should allow the user to enter decimals
        /// </summary>
        private void SetQuantityAllowDecimals(decimal qtyValue)
        {
            DecimalLimit unitDecimaLimit = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, cmbUnit.SelectedData.Text));
            ntbSendingQuantity.SetValueWithLimit(qtyValue, unitDecimaLimit);
        }
    }
}
