using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.RetailItems.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewPlugins.RetailItems.ViewPages.LogicHandlers;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Constants;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    public partial class ItemBaseSalesPricePage : UserControl, ITabViewV2, IMultiEditTabExtension, IMessageTabExtension
    {
        WeakReference owner;
        RetailItem item;
        /// <summary>
        /// Conversion factor between purchase unit and sales unit
        /// </summary>
        decimal unitConversionFactor = 1;
        RetailItemMultiEdit.FieldSelectionEnum enteredPriceFields;
        ItemBaseSalesPriceLogic logic;
        Dictionary<string, object> viewStateData;

        CostCalculation costCalculation;
        RetailItemCost weightedAverageCost;
        decimal originalPurchasePrice;

        internal ItemBaseSalesPricePage(TabControl owner, ItemBaseSalesPriceLogic logic)
            : this()
        {
            viewStateData = owner.ViewStateData;
            this.owner = new WeakReference(owner);
            this.logic = logic;
        }

        public ItemBaseSalesPricePage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            ItemBaseSalesPriceLogic logic = (ItemBaseSalesPriceLogic)tab.GetLogicInstance();
   
            return new ViewPages.ItemBaseSalesPricePage((TabControl)sender, logic);
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)internalContext;

            costCalculation = (CostCalculation)PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.CostCalculation, SettingsLevel.System).IntValue;

            // Multi edit rules will be that only following combinations are allowed to change or any one of the fields as single.:
            // Cost price and profit margin
            // Cost price and price
            // Cost price and price with tax

            if (item.ID == RecordIdentifier.Empty)
            {
                // When we enter we enable all the boxes
                ntbCostPrice.Enabled = costCalculation == CostCalculation.Manual;
                ntbProfitMargin.Enabled = true;
                ntbSalesPriceMiscCharges.Enabled = true;
            }

            if (viewStateData.ContainsKey("ItemSalesTaxGroupChanged") && (bool)viewStateData["ItemSalesTaxGroupChanged"] == true)
            {
                pnlTaxGroupChanged.Visible = true;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ViewBase view = GetOwnerView();

            if (view != null)
            {
                view.MultiEditControlChangedStatus += View_MultiEditControlChangedStatus;
            }
        }

        

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            DecimalLimit priceLimiter;
            DecimalLimit quantityLimiter;

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            item = (RetailItem)internalContext;

            if(costCalculation != CostCalculation.Manual)
            {
                originalPurchasePrice = item.PurchasePrice;
                weightedAverageCost = Providers.RetailItemCostData.Get(PluginEntry.DataModel, item.ID, PluginEntry.DataModel.CurrentStoreID);

                if(weightedAverageCost != null)
                {
                    item.PurchasePrice = weightedAverageCost.Cost;
                }

                if(item.SalesPrice != 0 && item.PurchasePrice != 0)
                {
                    item.ProfitMargin = item.PurchasePrice > item.SalesPrice ? 0 : ((item.SalesPrice - item.PurchasePrice) / item.SalesPrice) * 100;
                }
            }

            ntbSalesPrice.SetValueWithLimit(item.SalesPrice, priceLimiter);

            ntbProfitMargin.SetValueWithLimit(item.ProfitMargin, new DecimalLimit(0, 2));

            ntbCostPrice.SetValueWithLimit(item.PurchasePrice, priceLimiter);

            ntbProfitMargin.MaxValue = (ntbCostPrice.Value > 0 ? 99.99 : 100);
            ntbCostPrice.Enabled = costCalculation == CostCalculation.Manual && ntbProfitMargin.Value < 100;

            ntbProfitMargin.Value = (double)item.ProfitMargin;

            if (ntbProfitMargin.Value == 100)
            {
                errorProvider4.Icon = Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                errorProvider4.SetIconAlignment(linkFields1, ErrorIconAlignment.MiddleRight);
                errorProvider4.SetError(ntbCostPrice, Properties.Resources.CostPriceDisabled);
            }

            ntbSalesPriceWithVAT.SetValueWithLimit(item.SalesPriceIncludingTax, priceLimiter);

            ntbSalesPriceMiscCharges.Text = item.SalesMarkup.FormatWithLimits(priceLimiter);
        }

        public bool DataIsModified()
        {
            if (MainRecordDirty())
            {
                item.Dirty = true;
            }


            if (ntbSalesPrice.FullPrecisionValue != item.SalesPrice) item.Dirty = true;
            if (ntbSalesPriceWithVAT.FullPrecisionValue != item.SalesPriceIncludingTax) item.Dirty = true;

            if (ntbSalesPriceMiscCharges.Value != (double)item.SalesMarkup) item.Dirty = true;

            if (costCalculation == CostCalculation.Manual && ntbCostPrice.Value != (double)(item.PurchasePrice * unitConversionFactor)) item.Dirty = true;

            return item.Dirty;
        }

        private bool MainRecordDirty()
        {
            return ntbProfitMargin.Value != (double) item.ProfitMargin;
        }

        public bool SaveData()
        {
            if (ntbSalesPrice.Focused)
            {
                ntbSalesPrice_CalculateAndSetPriceWithTax(this, null);
            }

            if (ntbSalesPriceWithVAT.Focused)
            {
                ntbSalesPriceWithVAT_CalculateAndSetPriceWithoutTax(this, null);
            }

            if (ntbCostPrice.Focused)
            {
                ntbCostPrice_Leave(this, null);
            }

            if (ntbProfitMargin.Focused)
            {
                ntbProfitMargin_Leave(this, EventArgs.Empty);
            }

            if (item.Dirty)
            {
                item.ProfitMargin = (decimal)ntbProfitMargin.Value;
                item.PurchasePrice = costCalculation != CostCalculation.Manual ? originalPurchasePrice : (decimal)ntbCostPrice.Value * (1 / unitConversionFactor);
                item.SalesPrice = ntbSalesPrice.FullPrecisionValue;
                item.SalesPriceIncludingTax = ntbSalesPriceWithVAT.FullPrecisionValue;
                item.SalesMarkup = (decimal)ntbSalesPriceMiscCharges.Value;
            }

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if ((changeHint & DataEntityChangeType.VariableChanged) == DataEntityChangeType.VariableChanged)
            {
                switch (objectName)
                {
                    case "RetailItem":
                        if (changeIdentifier == item.ID && param is RetailGroup)
                        {
                            RetailGroup retailGroup = (RetailGroup)param;
                            ntbProfitMargin.Value = (double)retailGroup.ProfitMargin;
                        }
                        break;                 
                }
            }
            else if((changeHint & DataEntityChangeType.TabMessage) == DataEntityChangeType.TabMessage)
            {
                if(objectName == "")
                {
                    if (viewStateData.ContainsKey("ItemSalesTaxGroupChanged") && (bool)viewStateData["ItemSalesTaxGroupChanged"] == true)
                    {
                        pnlTaxGroupChanged.Visible = true;
                    }
                }
            }

            if (changeHint == DataEntityChangeType.Edit && (objectName == "BarCodeEdit" || objectName == "RetailItemTaxGroupWasChangedPricesRecalculated"))
            {
                LoadData(false, item.ID, item);
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        private void HandleDataProblems()
        {
            bool defaultStoreExists = true;
            bool defaultStoreHasTaxGroup = true;
            bool itemHasTaxGroup = true;

            RecordIdentifier itemSalesTaxGroupID = item.SalesTaxItemGroupID;
            RecordIdentifier defaultStoreID = Providers.StoreData.GetDefaultStoreID(PluginEntry.DataModel);
            RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetStoresSalesTaxGroupID(PluginEntry.DataModel, defaultStoreID);


            defaultStoreExists = defaultStoreID != "";
            defaultStoreHasTaxGroup = defaultStoreTaxGroup != "";
            itemHasTaxGroup = (itemSalesTaxGroupID != "");

            ShowErrorMessages(defaultStoreExists, defaultStoreHasTaxGroup, itemHasTaxGroup);
        }

        private void ShowErrorMessages(bool defaultStoreExists, bool defaultStoreHasTaxGroup, bool itemHasTaxGroup)
        {
            errorProvider1.Icon = System.Drawing.Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
            errorProvider1.SetIconAlignment(linkFields1, ErrorIconAlignment.MiddleRight);


            if (!defaultStoreExists)
            {
                errorProvider1.SetError(linkFields1, Resources.NoDefaultStoreSelected);
                return;
            }

            if (!defaultStoreHasTaxGroup)
            {
                errorProvider1.SetError(linkFields1, Resources.DefaultStoreHasNoTaxGroupAttachedToIt);
                return;
            }

            if (!itemHasTaxGroup)
            {
                errorProvider1.SetError(linkFields1, Resources.ItemHasNoTaxGroupAttachedToIt);
                return;
            }
            errorProvider1.Clear();
        }

        private void ntbSalesPrice_ValueChanged(object sender, EventArgs e)
        {
            if (item.ID == RecordIdentifier.Empty)
            {
                // Multi edit
            }
            else
            {
                if (item.ID != RecordIdentifier.Empty)
                {
                    if (ntbSalesPrice.HasMorePrecisionThanShown)
                    {
                        errorProvider2.Icon = Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                        errorProvider2.SetError(ntbSalesPrice, Resources.MoreDigitsThanShown);
                    }
                    else
                    {
                        errorProvider2.Clear();
                    }
                }
            }
        }

        private void ntbSalesPrice_CalculateAndSetPriceWithTax(object sender, EventArgs e)
        {
            if (ntbSalesPrice.ValueChangedWhileHavingFocus)
            {
                if (item.ID == RecordIdentifier.Empty)
                {
                    // We are in multiedit mode

                }
                else
                {
                    if (ntbSalesPrice.Value < ntbCostPrice.Value)
                    {
                        ntbProfitMargin.Value = 0;
                    }
                    else
                    {
                        if (ntbSalesPrice.Value != 0 && ntbCostPrice.Value != 0)
                        {
                            ntbProfitMargin.Value = ((ntbSalesPrice.Value - ntbCostPrice.Value) / ntbSalesPrice.Value) * 100;
                        }

                    }

                    ntbSalesPrice_CalculateAndSetPriceWithTax(sender, e, false);
                }
            }
        }

        private void ntbSalesPrice_CalculateAndSetPriceWithTax(object sender, EventArgs e, bool forced)
        {
            decimal calculatedPriceWithTax;
            DecimalLimit priceLimiter;

            if (item.ID != RecordIdentifier.Empty)
            {
                // We only do this in single edit more
                HandleDataProblems();
            }

            if (ntbSalesPrice.ValueChangedWhileHavingFocus || sender == this || forced)
            {
                if (item.ID == RecordIdentifier.Empty)
                {
                    // We are in multiedit mode
                }
                else
                {
                    decimal priceWithoutVAT = ntbSalesPrice.FullPrecisionValue;

                    priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                    RecordIdentifier itemSalesTaxGroupID = item.SalesTaxItemGroupID;

                    RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel);

                    calculatedPriceWithTax = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).CalculatePriceWithTax(
                        PluginEntry.DataModel,
                        priceWithoutVAT,
                        itemSalesTaxGroupID,
                        defaultStoreTaxGroup,
                        sender == this,
                        sender == this ? item.SalesPriceIncludingTax : 0.0m,
                        priceLimiter);


                    ntbSalesPriceWithVAT.SetValueWithLimit(calculatedPriceWithTax, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                }

            }

        }

        private void ntbSalesPriceWithVAT_CalculateAndSetPriceWithoutTax(object sender, EventArgs e)
        {
            if (item.ID == RecordIdentifier.Empty)
            {
                // Multi edit
            }
            else
            {
                // We only do this in single edit more
                HandleDataProblems();
                decimal priceWithoutVAT = 0.0M;

                if (ntbSalesPriceWithVAT.ValueChangedWhileHavingFocus)
                {

                    RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel);

                    priceWithoutVAT = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).CalculatePriceFromPriceWithTax(
                        PluginEntry.DataModel,
                        ntbSalesPriceWithVAT.FullPrecisionValue,
                        item.SalesTaxItemGroupID,
                        defaultStoreTaxGroup);

                    if (priceWithoutVAT < (decimal)ntbCostPrice.Value)
                    {
                        ntbProfitMargin.Value = 0;
                    }

                    ntbSalesPrice.SetValueWithLimit(priceWithoutVAT, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                    if (priceWithoutVAT != 0)// && ntbProfitMargin.Value > 0)
                    {
                        ntbProfitMargin.Value = (double)((priceWithoutVAT - (decimal)ntbCostPrice.Value) / priceWithoutVAT) * 100;
                    }
                }
            }
        }

        private void ntbProfitMargin_Leave(object sender, EventArgs e)
        {
            if (item.ID == RecordIdentifier.Empty)
            {
                // Multi edit
            }
            else
            {

                if (ntbProfitMargin.ValueChangedWhileHavingFocus)
                {
                    ntbCostPrice.Enabled = costCalculation == CostCalculation.Manual && (ntbProfitMargin.Value < 100);

                    if (ntbProfitMargin.Value != 0 && ntbProfitMargin.Value != 100)
                    {
                        decimal priceWithoutVAT = (decimal)(ntbCostPrice.Value / (1 - (ntbProfitMargin.Value / 100)));

                        ntbSalesPrice.SetValueWithLimit(priceWithoutVAT, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbSalesPrice_CalculateAndSetPriceWithTax(sender, e, true);
                    }

                    if (ntbProfitMargin.Value == 100)
                    {
                        ntbCostPrice.Value = 0;
                        errorProvider4.Icon = Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                        errorProvider4.SetIconAlignment(linkFields1, ErrorIconAlignment.MiddleRight);
                        errorProvider4.SetError(ntbCostPrice, Resources.CostPriceDisabled);
                    }
                    else
                    {
                        errorProvider4.Clear();
                    }

                }
            }
        }

        private void ntbCostPrice_Leave(object sender, EventArgs e)
        {
            if (item.ID == RecordIdentifier.Empty)
            {
                // Multi edit
            }
            else
            {

                if (ntbCostPrice.ValueChangedWhileHavingFocus)
                {
                    ntbProfitMargin.MaxValue = (ntbCostPrice.Value > 0 ? 99.99 : 100);

                    if (item.ID == RecordIdentifier.Empty)
                    {
                        // We are in multiedit mode
                    }
                    else
                    {
                        if (ntbCostPrice.Value > ntbSalesPrice.Value && ntbSalesPrice.Value != 0)
                        {
                            ntbProfitMargin.SetValueWithLimit(0, new DecimalLimit(0, 2));
                            return;
                        }
                        if (ntbSalesPrice.Value != 0)
                        {
                            decimal profitMargin = (decimal)((ntbSalesPrice.Value - ntbCostPrice.Value) / ntbSalesPrice.Value) * 100;
                            ntbProfitMargin.SetValueWithLimit(profitMargin, new DecimalLimit(0, 2));
                        }
                        else
                        {
                            decimal priceWithoutVAT = (decimal)(ntbCostPrice.Value / (1 - (ntbProfitMargin.Value / 100)));

                            ntbSalesPrice.SetValueWithLimit(priceWithoutVAT, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                            ntbSalesPrice_CalculateAndSetPriceWithTax(sender, e, true);
                        }
                    }
                }
            }
        }

        private ViewBase GetOwnerView()
        {
            Control control = this;

            while(!(control is ViewBase))
            {
                control = control.Parent;

                if(control == null)
                {
                    return null;
                }
            }

            return (ViewBase)control;
        }

        private void View_MultiEditControlChangedStatus(object sender, EventArgs e)
        {
            MutliEditEnablePriceBoxes();
        }

        private void MutliEditEnablePriceBoxes()
        {
            ViewBase view = GetOwnerView();

            if(view == null)
            {
                return;
            }

            bool profit = true;
            bool cost = costCalculation == CostCalculation.Manual;
            bool sales = true;
            bool salesWithTax = true;

            if (view.MultiEditControlIsChanged(ntbProfitMargin))
            {
                sales = false;
                salesWithTax = false;
            }

            if (view.MultiEditControlIsChanged(ntbSalesPrice))
            {
                profit = false;
                salesWithTax = false;
            }

            if (view.MultiEditControlIsChanged(ntbSalesPriceWithVAT))
            {
                sales = false;
                profit = false;
            }

            ntbCostPrice.Enabled = cost;
            ntbProfitMargin.Enabled = profit;
            ntbSalesPrice.Enabled = sales;
            ntbSalesPriceWithVAT.Enabled = salesWithTax;
        }

        private void ntbProfitMargin_TextChanged(object sender, EventArgs e)
        {

            /*if (item != null && item.ID == RecordIdentifier.Empty)
            {
                // We are in multiedit mode
                MutliEditEnablePriceBoxes();
            }*/
        }

        private void ntbSalesPrice_TextChanged(object sender, EventArgs e)
        {
           /* if (item != null && item.ID == RecordIdentifier.Empty)
            {
                // We are in multiedit mode
                MutliEditEnablePriceBoxes();
            }*/
        }

        private void ntbSalesPriceWithVAT_TextChanged(object sender, EventArgs e)
        {
            /*if (item != null && item.ID == RecordIdentifier.Empty)
            {
                // We are in multiedit mode
                MutliEditEnablePriceBoxes();
            }*/
        }

        public void MultiEditCollectData(IDataEntity dataEntity, HashSet<int> changedControlHashes, object param)
        {
            RetailItemMultiEdit itemObject = (RetailItemMultiEdit)dataEntity;

            if (changedControlHashes.Contains(ntbSalesPriceMiscCharges.GetHashCode()))
            {
                itemObject.SalesMarkup = (decimal)ntbSalesPriceMiscCharges.Value;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesMarkup;
            }

            // ------------------------------------------------------------------------
            // Complex fields that interact with each other, we partially handle those in the MultiEditSaveSecondaryRecords 
            // so that we can put different calculation for them per record

            if (changedControlHashes.Contains(ntbCostPrice.GetHashCode()))
            {
                itemObject.PurchasePrice = (decimal)ntbCostPrice.Value * (1 / unitConversionFactor);
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice;
            }

            if (changedControlHashes.Contains(ntbProfitMargin.GetHashCode()))
            {
                itemObject.ProfitMargin = (decimal)ntbProfitMargin.Value;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin;
            }

            if (changedControlHashes.Contains(ntbSalesPrice.GetHashCode()))
            {
                itemObject.SalesPrice = (decimal)ntbSalesPrice.Value;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesPrice;
            }

            if (changedControlHashes.Contains(ntbSalesPriceWithVAT.GetHashCode()))
            {
                itemObject.SalesPriceIncludingTax = (decimal)ntbSalesPriceWithVAT.Value;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax;
            }

            // Bellow we will figure out which other fields they pull with them

            // Rules are as follows......Any one of the four can be edited alone, or two can be edited together as shown in the sceme bellow where
            // 1 will have priority and ruling over 2
            // -----------------------------------------------------------------------------------------------------------------------
            // Cost price              --I(1)  --I(2)   --I(2)
            // Profit margin           --I(2)    I        I
            // Price                           --I(1)     I
            // Price with tax                           --I(1)
            // -----------------------------------------------------------------------------------------------------------------------

            enteredPriceFields = itemObject.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.PriceFieldsCombined;

            
            if (enteredPriceFields == RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice ||
                (enteredPriceFields & (RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice | RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin)) == (RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice | RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin))
            {
                // If (costprice and none of the others) or (cost price and profit margin)
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesPrice;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax;
            }
            else if(enteredPriceFields == RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin)
            {
                // elseif profit margin only
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesPrice;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice;
            }
            else if (enteredPriceFields == RetailItemMultiEdit.FieldSelectionEnum.SalesPrice)
            {
                // elseif profit price only
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax;
            }
            else if (enteredPriceFields == RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax)
            {
                // else if price with tax only
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesPrice;
            }
            else if ((enteredPriceFields & (RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice | RetailItemMultiEdit.FieldSelectionEnum.SalesPrice)) == (RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice | RetailItemMultiEdit.FieldSelectionEnum.SalesPrice))
            {
                // else if cost price and price
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax;
            }
            else if ((enteredPriceFields & (RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice | RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax)) == (RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice | RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax))
            {
                //else if cost price and price with tax
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesPrice;
            }

        }

        public bool MultiEditValidateSaveUnknownControls()
        {
            return false;
        }


        public void MultiEditSaveSecondaryRecords(IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            RetailItemMultiEdit item = (RetailItemMultiEdit)dataEntity;

            if(enteredPriceFields == RetailItemMultiEdit.FieldSelectionEnum.NoFields)
            {
                return;
            }

            // Rules are as follows......Any one of the four can be edited alone, or two can be edited together as shown in the sceme bellow where
            // 1 will have priority and ruling over 2
            // -----------------------------------------------------------------------------------------------------------------------
            // Cost price              --I(1)  --I(2)   --I(2)
            // Profit margin           --I(2)    I        I
            // Price                           --I(1)     I
            // Price with tax                           --I(1)
            // -----------------------------------------------------------------------------------------------------------------------

            // If we (got Sales price or sales price with tax or profit margin or cost price)
            if (((enteredPriceFields & (RetailItemMultiEdit.FieldSelectionEnum.SalesPrice | RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax | RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin | RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice)) != 0))
            {
                // We need to fetch the tax group and the rest of the price info for the item there is no way around that

                if (item.OldPrices == null)
                {
                    item.OldPrices = Providers.RetailItemData.GetItemPrice(threadedConnection, primaryRecordID.PrimaryID);
                }

                // We allready have valid tax group for this item, might be because user entered new Tax group ?
                // But it means we have to use it rather than the one from the database.
                if(item.HasValidSalesTaxItemGroupID)
                {
                    item.OldPrices.SalesTaxItemGroupID = (string)item.SalesTaxItemGroupID;
                }
            }

            if (enteredPriceFields == RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice)
            {
                // If (costprice and none of the others)

                item.PurchasePrice = (decimal)ntbCostPrice.Value * (1 / unitConversionFactor);
                item.SalesPrice = item.OldPrices.SalesPrice;
                item.SalesPriceIncludingTax = item.OldPrices.SalesPriceIncludingTax;

                if (item.OldPrices.PurchasePrice * unitConversionFactor > item.OldPrices.SalesPrice && item.OldPrices.SalesPrice != 0)
                {
                    item.ProfitMargin = 0;
                }
                else
                {
                    item.ProfitMargin = item.OldPrices.ProfitMargin;
                }

                if (item.OldPrices.SalesPrice != 0)
                {
                    item.ProfitMargin = (decimal)(item.OldPrices.SalesPrice - (item.PurchasePrice * unitConversionFactor) / item.OldPrices.SalesPrice) * 100;
                }
                else
                {
                    item.SalesPrice = (decimal)((item.PurchasePrice * unitConversionFactor) / (1 - (item.ProfitMargin / 100)));

                    logic.CalculatePriceWithTax(item, item.OldPrices.SalesTaxItemGroupID);
                }
            }
            else if (enteredPriceFields == RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin)
            {
                // elseif profit margin only
                item.ProfitMargin = (decimal)ntbProfitMargin.Value;
                item.PurchasePrice = item.OldPrices.PurchasePrice;
                item.SalesPrice = item.OldPrices.SalesPrice;
                item.SalesPriceIncludingTax = item.OldPrices.SalesPriceIncludingTax;

                if (item.ProfitMargin != 0 && item.ProfitMargin != 100)
                {
                    item.SalesPrice = (decimal)((item.PurchasePrice * unitConversionFactor) / (1 - (item.ProfitMargin / 100)));

                    logic.CalculatePriceWithTax(item, item.OldPrices.SalesTaxItemGroupID);
                }

                if (item.ProfitMargin == 100)
                {
                    item.PurchasePrice = 0;
                }
            }
            else if (enteredPriceFields == RetailItemMultiEdit.FieldSelectionEnum.SalesPrice)
            {
                // elseif sales price only

                item.ProfitMargin = item.OldPrices.ProfitMargin;
                item.PurchasePrice = item.OldPrices.PurchasePrice;
                item.SalesPrice = (decimal)ntbSalesPrice.Value;
                item.SalesPriceIncludingTax = item.OldPrices.SalesPriceIncludingTax;

                if (item.SalesPrice != 0 && item.PurchasePrice != 0)
                {
                    item.ProfitMargin = logic.CalculateProfitMargin(item.SalesPrice, item.PurchasePrice * unitConversionFactor);
                }

                logic.CalculatePriceWithTax(item, item.OldPrices.SalesTaxItemGroupID);

            }
            else if (enteredPriceFields == RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax)
            {
                // else if price with tax only
                decimal priceWithoutVAT = 0.0M;

                item.ProfitMargin = item.OldPrices.ProfitMargin;
                item.PurchasePrice = item.OldPrices.PurchasePrice;
                item.SalesPrice = item.OldPrices.SalesPrice;
                item.SalesPriceIncludingTax = (decimal)ntbSalesPriceWithVAT.Value;

                RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(threadedConnection);

                priceWithoutVAT = Services.Interfaces.Services.TaxService(threadedConnection).CalculatePriceFromPriceWithTax(
                    threadedConnection,
                    item.SalesPriceIncludingTax,
                    item.OldPrices.SalesTaxItemGroupID,
                    defaultStoreTaxGroup);

                if (priceWithoutVAT < (decimal)item.PurchasePrice * unitConversionFactor)
                {
                    item.ProfitMargin = 0;
                }

                item.SalesPrice = priceWithoutVAT;

                if (priceWithoutVAT != 0)// && ntbProfitMargin.Value > 0)
                {
                    //item.ProfitMargin = (decimal)((priceWithoutVAT - (decimal)item.PurchasePrice) / priceWithoutVAT) * 100;
                    item.ProfitMargin = logic.CalculateProfitMargin(priceWithoutVAT, item.PurchasePrice * unitConversionFactor);
                }

            }
            else if ((enteredPriceFields & (RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice | RetailItemMultiEdit.FieldSelectionEnum.SalesPrice)) == (RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice | RetailItemMultiEdit.FieldSelectionEnum.SalesPrice))
            {
                // else if cost price and price

                // When this is processed then price is processed first and then cost price

                item.ProfitMargin = item.OldPrices.ProfitMargin;
                item.PurchasePrice = (decimal)ntbCostPrice.Value * (1 / unitConversionFactor);
                item.SalesPrice = (decimal)ntbSalesPrice.Value;
                item.SalesPriceIncludingTax = item.OldPrices.SalesPriceIncludingTax;

                if (item.SalesPrice != 0 && item.PurchasePrice != 0)
                {
                    item.ProfitMargin = logic.CalculateProfitMargin(item.SalesPrice, item.PurchasePrice * unitConversionFactor);
                }

                logic.CalculatePriceWithTax(item, item.OldPrices.SalesTaxItemGroupID);

            }
            else if ((enteredPriceFields & (RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice | RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax)) == (RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice | RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax))
            {
                //else if cost price and price with tax

                // When this is processed then price with tax is processed first and then cost price

                decimal priceWithoutVAT = 0.0M;

                item.ProfitMargin = item.OldPrices.ProfitMargin;
                item.PurchasePrice = (decimal)ntbCostPrice.Value * (1 / unitConversionFactor);
                item.SalesPrice = item.OldPrices.SalesPrice;
                item.SalesPriceIncludingTax = (decimal)ntbSalesPriceWithVAT.Value;

                RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(threadedConnection);

                priceWithoutVAT = Services.Interfaces.Services.TaxService(threadedConnection).CalculatePriceFromPriceWithTax(
                    threadedConnection,
                    item.SalesPriceIncludingTax,
                    item.OldPrices.SalesTaxItemGroupID,
                    defaultStoreTaxGroup);

                if (priceWithoutVAT < (decimal)item.PurchasePrice)
                {
                    item.ProfitMargin = 0;
                }

                item.SalesPrice = priceWithoutVAT;

                if (priceWithoutVAT != 0)// && ntbProfitMargin.Value > 0)
                {                    
                    item.ProfitMargin = logic.CalculateProfitMargin(priceWithoutVAT, item.PurchasePrice * unitConversionFactor);
                }
            }
            else if ((enteredPriceFields & (RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice | RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin)) == (RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice | RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin))
            {
                // Cost price and profit margin
                item.ProfitMargin = (decimal)ntbProfitMargin.Value;
                item.PurchasePrice = (decimal)ntbCostPrice.Value;
                item.SalesPrice = item.OldPrices.SalesPrice;
                item.SalesPriceIncludingTax = item.OldPrices.SalesPriceIncludingTax;

                if (item.ProfitMargin != 0 && item.ProfitMargin != 100)
                {
                    item.SalesPrice = (decimal)((item.PurchasePrice * unitConversionFactor) / (1 - (item.ProfitMargin / 100)));

                    logic.CalculatePriceWithTax(item, item.OldPrices.SalesTaxItemGroupID);
                }

                if (item.ProfitMargin == 100)
                {
                    item.PurchasePrice = 0;
                }
            }

            Providers.RetailItemMultiEditData.UpdatePriceFields(item);
        }

        public void MultiEditSaveSecondaryRecordsFinalizer()
        {

        }

        public void MultiEditRevertUnknownControl(Control control, bool isRevertField, ref bool handled)
        {

        }

        private void headerItemPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        public object OnViewPageMessage(object sender, string message, object param, ref bool handled)
        {
            switch (message)
            {
                case "GetItemSalesPrice":
                    handled = true;

                    bool pricesAreWithTax = false;
                    RecordIdentifier defaultStoreID = Providers.StoreData.GetDefaultStoreID(PluginEntry.DataModel);

                    if (!RecordIdentifier.IsEmptyOrNull(defaultStoreID))
                    {
                        pricesAreWithTax = Providers.StoreData.GetPriceWithTaxForStore(PluginEntry.DataModel, defaultStoreID);
                    }

                    return pricesAreWithTax ? (decimal)ntbSalesPriceWithVAT.Value : (decimal)ntbSalesPrice.Value;

            }
            return null;
        }
    }
}
