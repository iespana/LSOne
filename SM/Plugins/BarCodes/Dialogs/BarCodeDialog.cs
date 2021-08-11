using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.BarCodes.Properties;

namespace LSOne.ViewPlugins.BarCodes.Dialogs
{ 
    public partial class BarCodeDialog : DialogBase
    {
        RecordIdentifier itemID;
        RecordIdentifier itemMasterID;
        RetailItem retailItem;
        BarCode oldBarCode;        
        bool creatingNew;        
        RecordIdentifier salesUnitID;
        RecordIdentifier inventoryUnitID;
        WeakReference unitEditor;

        public BarCodeDialog(RecordIdentifier itemID, RecordIdentifier itemMasterID, DataEntity selectedBarCodeSetup, bool forceDefaultBarcode)
            : this()
        {
            // Used when creating new bar code

            this.creatingNew = true;
            this.itemID = itemID;
            this.itemMasterID = itemMasterID;
            retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, itemMasterID);

            cmbBarCodeSetup.SelectedData = selectedBarCodeSetup;

            // Show the current item you're creating a barcode for. In this case we are adding a barcode from a specific variant view
            if (retailItem.ItemType == ItemTypeEnum.Item && !retailItem.HeaderItemID.IsEmpty)
            {
                cmbVariantNumber.SelectedData = new DataEntity(retailItem.MasterID, retailItem.VariantName);
            }

            salesUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, this.itemID, RetailItem.UnitTypeEnum.Sales);
            inventoryUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, this.itemID, RetailItem.UnitTypeEnum.Inventory);

            CheckIfCanShowForItem();

            if(chkShowForItem.Enabled && forceDefaultBarcode)
            {
                chkShowForItem.Enabled = false;
                chkShowForItem.Checked = true;
            }
        }

        public BarCodeDialog(RecordIdentifier selectedBarCodeID)
            : this()
        {
            // Used when editing existing barcode

            this.creatingNew = false;

            oldBarCode = Providers.BarCodeData.Get(PluginEntry.DataModel, selectedBarCodeID);
            

            this.itemID = oldBarCode.ItemID;
            itemMasterID = Providers.RetailItemData.GetMasterIDFromItemID(PluginEntry.DataModel, itemID);
            retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, itemMasterID);

            if (retailItem.ItemType == ItemTypeEnum.Item && !retailItem.HeaderItemID.IsEmpty)
            {
                cmbVariantNumber.SelectedData = new DataEntity(retailItem.MasterID, retailItem.VariantName);                
            }

            tbBarCode.Text = (string)oldBarCode.ItemBarCode;
            ntbQuantity.Value = (double)oldBarCode.Quantity;
            cmbBarCodeSetup.SelectedData = new DataEntity(oldBarCode.BarCodeSetupID, oldBarCode.BarCodeSetupDescription);


            chkScanning.Checked = oldBarCode.UseForInput;
            chkShowForItem.Checked = oldBarCode.ShowForItem;
            chkToBePrinted.Checked = oldBarCode.UseForPrinting;

            cmbUnit.SelectedData = new Unit(oldBarCode.UnitID, oldBarCode.UnitDescription, 0, 0);

            salesUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, this.itemID, RetailItem.UnitTypeEnum.Sales);
            inventoryUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, this.itemID, RetailItem.UnitTypeEnum.Inventory);

            if (!oldBarCode.ShowForItem)
            {
                CheckIfCanShowForItem();
            }
        }


        public BarCodeDialog()
        {
            IPlugin unitEditorPlugin;

            this.salesUnitID = null;
            this.oldBarCode = null;

            itemID = RecordIdentifier.Empty;

            InitializeComponent();

            cmbVariantNumber.SelectedData = new DataEntity("", "");
            cmbBarCodeSetup.SelectedData = new DataEntity("", "");

            cmbUnit.SelectedData = new Unit("","",0,0);

            unitEditorPlugin = PluginEntry.Framework.FindImplementor(this, "CanAddUnits", null);

            unitEditor = (unitEditorPlugin != null) ? new WeakReference(unitEditorPlugin) : null;

            btnAddUnit.Visible = unitEditor != null;
        }

        /// <summary>
        /// Gets or sets whether we are editing a barcode from the master item view
        /// </summary>
        public bool EditingBarcodeFromMasterItem { get; set; }

        private void CheckIfCanShowForItem()
        {
            chkShowForItem.Enabled = !Providers.BarCodeData.ShowForItemHasBeenUsed(PluginEntry.DataModel, itemMasterID);
        }

       
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier BarCodeID
        {
            get { return oldBarCode.ItemBarCode; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            

            cmbVariantNumber.Enabled = (creatingNew && retailItem.ItemType == ItemTypeEnum.MasterItem)
                                        || retailItem.ItemType == ItemTypeEnum.MasterItem 
                                        || EditingBarcodeFromMasterItem;// || (retailItem.ItemType == ItemTypeEnum.Item && !retailItem.HeaderItemID.IsEmpty);
        }

        private void btnOK_Click(object sender, EventArgs args)
        {
            RecordIdentifier oldBCode;
            BarCode barCode;
            BarCodeSetup barCodeSetup;

            oldBCode = (oldBarCode == null) ? "" : oldBarCode.ItemBarCode;


            // See if the barcode already exists
            if (creatingNew || oldBCode != tbBarCode.Text)
            {
                if (Providers.BarCodeData.Exists(PluginEntry.DataModel, tbBarCode.Text))
                {
                    if (!Providers.BarCodeData.IsDeleted(PluginEntry.DataModel, tbBarCode.Text))
                    {
                        errorProvider1.SetError(tbBarCode, Properties.Resources.BarCodeExists);
                        return;
                    }
                }
            }

            // Verify that the barcode meets maxlength and minlength criterias
            barCodeSetup = Providers.BarCodeSetupData.Get(PluginEntry.DataModel, cmbBarCodeSetup.SelectedData.ID);

            if (barCodeSetup != null && barCodeSetup.BarCodeMask.Length != barCodeSetup.BarCodeMask.Count(c => c == 'X'))
            {
                if (barCodeSetup.MinimumLength > 0)
                {
                    if (tbBarCode.Text.Length < barCodeSetup.MinimumLength)
                    {
                        errorProvider1.SetError(tbBarCode, Properties.Resources.BarCodeMinLengthError.Replace("#1", barCodeSetup.MinimumLength.ToString()));
                        return;
                    }
                }

                if (barCodeSetup.MaximumLength > 0)
                {
                    if (tbBarCode.Text.Length > barCodeSetup.MaximumLength)
                    {
                        errorProvider1.SetError(tbBarCode, Properties.Resources.BarCodeMaxLengthError.Replace("#1", barCodeSetup.MaximumLength.ToString()));
                        return;
                    }
                }
            }

            if (!RecordIdentifier.IsEmptyOrNull(cmbUnit.SelectedData.ID))
            {
                while (!Providers.UnitConversionData.UnitConversionRuleExists(PluginEntry.DataModel, itemID, cmbUnit.SelectedData.ID, salesUnitID))
                {
                    IPlugin unitConversionAdder = PluginEntry.Framework.FindImplementor(this, "CanAddUnitConversions", null);

                    if (unitConversionAdder != null)
                    {
                        if (salesUnitID == null)
                        {
                            salesUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, this.itemID, RetailItem.UnitTypeEnum.Inventory);
                        }

                        if (QuestionDialog.Show(
                            Resources.SalesUnitConversionQuestion,
                            Properties.Resources.UnitConversionRuleMissing) == System.Windows.Forms.DialogResult.Yes)
                        {
                            DataEntity retailItemDataEntity;

                            RetailItem retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, this.itemID);
                            retailItemDataEntity = new DataEntity(retailItem.ID, retailItem.Text);

                            if (!(bool)unitConversionAdder.Message(this, "AddUnitConversion", new object[] { retailItemDataEntity, salesUnitID, cmbUnit.SelectedData.ID }))
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        MessageDialog.Show(Resources.SalesUnitConversionRuleMissingAlert);
                        return;
                    }
                }
            }

            if (!RecordIdentifier.IsEmptyOrNull(cmbUnit.SelectedData.ID))
            {
                if (!Providers.UnitConversionData.UnitConversionRuleExists(PluginEntry.DataModel, itemID, cmbUnit.SelectedData.ID, inventoryUnitID))
                {
                    decimal soldToSales = Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel, itemID,
                        cmbUnit.SelectedData.ID, salesUnitID, 1);
                    decimal soldToInventory = Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel, itemID,
                        salesUnitID, inventoryUnitID, soldToSales);

                    if (inventoryUnitID == null)
                    {
                        inventoryUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, this.itemID,
                            RetailItem.UnitTypeEnum.Inventory);

                    }
                    UnitConversion unitConversion = new UnitConversion();

                    unitConversion.FromUnitID = inventoryUnitID;
                    unitConversion.ToUnitID = cmbUnit.SelectedData.ID;
                    unitConversion.ItemID = itemID;
                    unitConversion.Factor = soldToInventory;

                    Providers.UnitConversionData.Save(PluginEntry.DataModel, unitConversion);
                }
            }
            

            barCode = new BarCode();
            barCode.ItemID = itemID;
            barCode.ItemBarCode = tbBarCode.Text;
            barCode.UnitID = cmbUnit.SelectedData.ID;

            if(!creatingNew)
            {
                barCode.ItemBarcodeID = oldBarCode.ItemBarcodeID;
            }

            if (cmbVariantNumber.Enabled)
            {
                barCode.ItemID = Providers.RetailItemData.GetItemIDFromMasterID(PluginEntry.DataModel, cmbVariantNumber.SelectedDataID);
            }

            barCode.BarCodeSetupID = cmbBarCodeSetup.SelectedData.ID;
            barCode.ShowForItem = chkShowForItem.Checked;
            barCode.UseForInput = chkScanning.Checked;
            barCode.UseForPrinting = chkToBePrinted.Checked;
            barCode.Quantity = (decimal)ntbQuantity.Value;

           Providers.BarCodeData.Save(PluginEntry.DataModel, barCode);

            oldBarCode = barCode;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            bool enabled;

            errorProvider1.Clear();

            if(cmbVariantNumber.Enabled)
            {
                enabled = tbBarCode.Text.Length > 0 &&
                    cmbBarCodeSetup.SelectedData.ID.StringValue != "" &&
                    cmbVariantNumber.SelectedData.ID.StringValue != "";
            }
            else
            {
                enabled = tbBarCode.Text.Length > 0 &&
                    cmbBarCodeSetup.SelectedData.ID.StringValue != "";
            }

            if (!creatingNew)
            {
                // See if anything changed
                if (tbBarCode.Text == oldBarCode.ItemBarCode &&
                    cmbBarCodeSetup.SelectedData.ID == oldBarCode.BarCodeSetupID &&
                    cmbVariantNumber.SelectedDataID == itemMasterID &&
                    chkScanning.Checked == oldBarCode.UseForInput &&
                    chkShowForItem.Checked == oldBarCode.ShowForItem &&
                    chkToBePrinted.Checked == oldBarCode.UseForPrinting &&
                    cmbUnit.SelectedData.ID == oldBarCode.UnitID &&
                    (decimal)ntbQuantity.Value == oldBarCode.Quantity)
                {
                    enabled = false;
                }
            }            

            if (retailItem.ItemType == ItemTypeEnum.MasterItem || (retailItem.ItemType == ItemTypeEnum.Item && !retailItem.HeaderItemID.IsEmpty))
            {
                enabled =  enabled && (string)cmbVariantNumber.SelectedDataID != "";
            }

            btnOK.Enabled = enabled;
        }             

        private void cmbBarCodeSetup_RequestData(object sender, EventArgs e)
        {
            cmbBarCodeSetup.SetData(Providers.BarCodeSetupData.GetList(PluginEntry.DataModel),
                null);
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            if (salesUnitID == null)
            {
                salesUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, this.itemID, RetailItem.UnitTypeEnum.Sales);
            }

            IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
                    {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel,itemID,salesUnitID),
                     Providers.UnitData.GetAllUnits(PluginEntry.DataModel).Cast<DataEntity>()};

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                data,
                new string[] { Properties.Resources.Convertible, Properties.Resources.All },
                250);

            cmbUnit.SetData(data, pnl);
        }

        private void cmbUnit_RequestClear(object sender, EventArgs e)
        {
            cmbUnit.SelectedData = new Unit("", "", 0, 0);
        }

        private void btnAddUnit_Click(object sender, EventArgs e)
        {
            if (unitEditor.IsAlive)
            {
                ((IPlugin)unitEditor.Target).Message(this, "AddUnits", null);
            }
        }

        private void cmbVariantNumber_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbVariantNumber_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbVariantNumber_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity)cmbVariantNumber.SelectedData).Text;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                retailItem.ItemType == ItemTypeEnum.MasterItem ?
                retailItem.MasterID :
                retailItem.HeaderItemID,
                true, 
                initialSearchText, 
                SearchTypeEnum.RetailItemVariantMasterID, 
                new List<RecordIdentifier>(), 
                textInitallyHighlighted,
                true);
        }
    }
}
