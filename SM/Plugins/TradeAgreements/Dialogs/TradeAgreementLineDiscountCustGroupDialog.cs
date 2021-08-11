using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class TradeAgreementLineDiscountCustGroupDialog : DialogBase
    {
        RecordIdentifier storeID;
        RecordIdentifier id;
        TradeAgreementEntryAccountCode type;
        TradeAgreementEntry agreement;
        WeakReference unitEditor;
        WeakReference currencyEditor;
        private RetailItem retailItem;
        public TradeAgreementLineDiscountCustGroupDialog(RecordIdentifier id, RecordIdentifier agreementID, TradeAgreementEntryAccountCode type)
            : this(id,type)
        {
            DecimalLimit priceLimiter;
            //Dimension dimension;
            Unit unit;

            agreement = Providers.TradeAgreementData.Get(PluginEntry.DataModel, agreementID, TradeAgreementRelation.LineDiscSales);


            cmbCurrency.SelectedData = new DataEntity(agreement.Currency, agreement.CurrencyDescription);
            cmbItemCode.SelectedIndex = (int)agreement.ItemCode;
            DataEntity itemSelectionData;
            if (agreement.ItemCode == TradeAgreementEntry.TradeAgreementEntryItemCode.Table)
            {
                RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, agreement.ItemRelation);
                itemSelectionData = new DataEntity(item.ID, item.Text);
            }
            else
            {
                itemSelectionData = new DataEntity(agreement.ItemRelation, agreement.ItemRelationName);
            }
            cmbItemSelection.SelectedData = itemSelectionData;

            cmbItemSelection_SelectedDataChanged(cmbItemSelection, EventArgs.Empty);
            //cmbItem_SelectedDataChanged(cmbItem, EventArgs.Empty);

            //dimension = new Dimension();
            //dimension.DimensionID = agreement.InventDimID;
            //dimension.ColorID = agreement.ColorID;
            //dimension.SizeID = agreement.SizeID;
            //dimension.StyleID = agreement.StyleID;
            //dimension.ColorName = agreement.ColorName;
            //dimension.StyleName = agreement.StyleName;
            //dimension.SizeName = agreement.SizeName;
            //dimension.VariantNumber = Providers.DimensionData.GetVariantIDFromDimID(PluginEntry.DataModel, agreement.InventDimID);
            //cmbVariantNumber.SelectedData = dimension;
            //TODO assign variantitemid
            //if (dimension.VariantNumber != "")
            //{
            //    cmbVariantNumber_SelectedDataChanged(cmbVariantNumber, EventArgs.Empty);
            //}

            unit = Providers.UnitData.Get(PluginEntry.DataModel, agreement.UnitID);

            if (unit == null)
            {
                if (agreement.ItemCode == TradeAgreementEntry.TradeAgreementEntryItemCode.Table)
                {
                    unit = new Unit(agreement.UnitID, (string) agreement.UnitID, 0, 0);
                }
                else
                {
                    RecordIdentifier salesUnit = "";
                    int salesUnitDecimals = 0;
                    string salesUnitText = "";
                    unit = new Unit(salesUnit, salesUnitText, salesUnitDecimals, salesUnitDecimals);

                }
            }
            cmbUnit.SelectedData = unit;
            cmbUnit_SelectedDataChanged(this, EventArgs.Empty);
            

            agreement.FromDate.ToDateControl(dtpFromDate);
            agreement.ToDate.ToDateControl(dtpToDate);

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            ntbQuantity.Text = agreement.QuantityAmount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity));
            ntbDiscount.Text = agreement.Amount.FormatWithLimits(priceLimiter);
         
            ntbPercentage1.Text = agreement.Percent1.FormatWithLimits(priceLimiter);
            ntbPercentage2.Text = agreement.Percent2.FormatWithLimits(priceLimiter);

            CheckEnabled(this, EventArgs.Empty);
        }

        public TradeAgreementLineDiscountCustGroupDialog(RecordIdentifier id, TradeAgreementEntryAccountCode type)
            : this()
        {
            string itemName = "";
            RecordIdentifier salesUnit = "";
            int salesUnitDecimals = 0;
            string salesUnitText = "";
            CompanyInfo companyInfo;

            agreement = null;

            cmbUnit.SelectedData = new Unit();

            this.type = type;
            this.id = id;

            cmbUnit.SelectedData = new Unit(salesUnit, salesUnitText, salesUnitDecimals, salesUnitDecimals);

            Text += " - " + itemName;

            companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel,false);

            cmbCurrency.SelectedData = new DataEntity(companyInfo.CurrencyCode,companyInfo.CurrencyCodeText);
            cmbItemSelection.SelectedData = new DataEntity("","");

            cmbVariantNumber.SelectedData = new Dimension();
        }

        public TradeAgreementLineDiscountCustGroupDialog()
        {
            IPlugin plugin;

            storeID = RecordIdentifier.Empty;

            InitializeComponent();

            cmbCurrency.SelectedData = new DataEntity("", "");

            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddUnits", null);
            unitEditor = (plugin != null) ? new WeakReference(plugin) : null;
            btnAddUnit.Visible = (unitEditor != null);

            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddCurrency", null);
            currencyEditor = (plugin != null) ? new WeakReference(plugin) : null;
            btnAddCurrency.Visible = (currencyEditor != null);

            ntbQuantity.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity).Max;
            ntbDiscount.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices).Max;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier StoreID
        {
            get { return storeID; }
        }

        public RecordIdentifier ID
        {
            get
            {
                return agreement.ID;
            }
        }

        private void PopulateTAEntry(TradeAgreementEntry taEntry)
        {
            taEntry.Currency = cmbCurrency.SelectedData.ID;
            taEntry.AccountCode = type;
            taEntry.AccountRelation = id;
            taEntry.UnitID = cmbUnit.SelectedData.ID;
            taEntry.FromDate = Date.FromDateControl(dtpFromDate).GetDateWithoutTime();
            taEntry.QuantityAmount = (decimal)ntbQuantity.Value;
            taEntry.Relation = TradeAgreementEntry.TradeAgreementEntryRelation.LineDiscSales;
            taEntry.ItemCode = (TradeAgreementEntry.TradeAgreementEntryItemCode)cmbItemCode.SelectedIndex;
            RecordIdentifier target;            

            if ((cmbItemCode.SelectedIndex == (int)TradeAgreementEntry.TradeAgreementEntryItemCode.Table) &&  cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty)
            {
                if (cmbVariantNumber.SelectedData is MasterIDEntity)
                {
                    target = (cmbVariantNumber.SelectedData as MasterIDEntity).ReadadbleID;
                }
                else
                {
                    target = (cmbVariantNumber.SelectedData as DataEntity).ID;
                }

            }
            else
            {
                if (cmbItemSelection.SelectedData is MasterIDEntity)
                {
                    target = (cmbItemSelection.SelectedData as MasterIDEntity).ReadadbleID;

                }
                else
                {
                    target = (cmbItemSelection.SelectedData as DataEntity).ID;
                }
            }


            taEntry.ItemRelation = target;
            taEntry.ToDate = Date.FromDateControl(dtpToDate).GetDateWithoutTime();
            taEntry.Amount = (decimal)ntbDiscount.Value;
            taEntry.Percent1 = (decimal)ntbPercentage1.Value;
            taEntry.Percent2 = (decimal)ntbPercentage2.Value;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Date.FromDateControl(dtpFromDate) > Date.FromDateControl(dtpToDate) && !Date.FromDateControl(dtpToDate).IsEmpty)
            {
                errorProvider1.SetError(dtpFromDate, Properties.Resources.FromDateCannotBeHigherThanToDate);
                return;
            }            

            if (agreement == null)
            {
                TradeAgreementEntry testAgreement = new TradeAgreementEntry();
                PopulateTAEntry(testAgreement);
                if (Providers.TradeAgreementData.DataContentExists(PluginEntry.DataModel, testAgreement.OldID))
                {
                    errorProvider1.SetError(cmbCurrency, Properties.Resources.TradeAgreementExists1);
                    return;
                }

                agreement = new TradeAgreementEntry();
                PopulateTAEntry(agreement);
            }
            else
            {
                PopulateTAEntry(agreement);
                if ((Providers.TradeAgreementData.DataContentExists(PluginEntry.DataModel, agreement.OldID, agreement.ID)))
                {
                    errorProvider1.SetError(cmbCurrency, Properties.Resources.TradeAgreementExists1);
                    return;
                }
            }

            Providers.TradeAgreementData.Save(PluginEntry.DataModel, agreement, Permission.ManageDiscounts);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbCurrency_RequestData(object sender, EventArgs e)
        {
            cmbCurrency.SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            RecordIdentifier salesUnitID;

            cmbUnit.SetWidth(200);

            salesUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItemSelection.SelectedData.ID, RetailItem.UnitTypeEnum.Sales);

            IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
                    {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel,cmbItemSelection.SelectedData.ID,salesUnitID),
                     Providers.UnitData.GetAllUnits(PluginEntry.DataModel)};

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                data,
                new string[] { Properties.Resources.Convertible, Properties.Resources.All },
                250);

            cmbUnit.SetData(data, pnl);
        }

        private void cmbUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            if(cmbUnit.SelectedData.ID != "")
            {
                ntbQuantity.DecimalLetters = ((Unit)cmbUnit.SelectedData).MaximumDecimals;
            }

            CheckEnabled(sender, EventArgs.Empty);
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool valid  =
                    (
                        cmbCurrency.SelectedData.ID != ""
                        && cmbItemCode.SelectedIndex >= 0
                        && (
                            (
                                cmbItemSelection.Enabled
                                && cmbItemSelection.SelectedData.ID != ""
                                && (
                                    cmbUnit.SelectedData.ID != "" 
                                    || !cmbUnit.Enabled
                                    )
                            )
                            || !cmbItemSelection.Enabled
                        )
                    );
          
            if (agreement == null)
            {
                btnOK.Enabled = valid;
                   
            }
            else
            {
                RecordIdentifier target;
                if (cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty)
                {
                    if (cmbVariantNumber.SelectedData is MasterIDEntity)
                    {
                        target = (cmbVariantNumber.SelectedData as MasterIDEntity).ReadadbleID;
                    }
                    else
                    {
                        target = (cmbVariantNumber.SelectedData as DataEntity).ID;
                    }

                }
                else
                {
                    if (cmbItemSelection.SelectedData is MasterIDEntity)
                    {
                        target = (cmbItemSelection.SelectedData as MasterIDEntity).ReadadbleID;

                    }
                    else
                    {
                        target = (cmbItemSelection.SelectedData as DataEntity).ID;
                    }
                }

                btnOK.Enabled = valid
                                && (
                                    cmbCurrency.SelectedData.ID != agreement.Currency
                                    || type != agreement.AccountCode
                                    || id != agreement.AccountRelation
                                    || cmbUnit.SelectedData.ID != agreement.UnitID
                                    || Date.FromDateControl(dtpFromDate) != agreement.FromDate
                                    || Date.FromDateControl(dtpToDate) != agreement.ToDate
                                    || (decimal) ntbQuantity.Value != agreement.QuantityAmount
                                    || (decimal) ntbDiscount.Value != agreement.Amount
                                    || (decimal) ntbPercentage1.Value != agreement.Percent1
                                    || (decimal) ntbPercentage2.Value != agreement.Percent2)
                                    || target != agreement.ItemRelation;
                //((Dimension)cmbVariantNumber.SelectedData).DimensionID != agreement.InventDimID ||
            }
        }

      

     
        private void cmbVariantNumber_RequestClear(object sender, EventArgs e)
        {
            cmbVariantNumber.SelectedData = new Dimension();

            cmbVariantNumber_SelectedDataChanged(cmbVariantNumber, EventArgs.Empty);
        }

        private void cmbVariantNumber_SelectedDataChanged(object sender, EventArgs e)
        {
           

            CheckEnabled(this, EventArgs.Empty);
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void ntbQuantity_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void ntbPrice_TextChanged(object sender, EventArgs e)
        {            
            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbItemCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbItemSelection.Enabled = cmbItemCode.SelectedIndex != 2;

            cmbItemSelection.SelectedData = new DataEntity("", "");

            if (cmbItemCode.SelectedIndex == 0)
            {
                cmbItemSelection.SetUseInternalDropDown(false);
                cmbUnit.Enabled = true;
            }
            else
            {
                cmbItemSelection.SetUseInternalDropDown(true);
                cmbUnit.SelectedData = new Unit();
                cmbVariantNumber.SelectedData = new Dimension();
                cmbVariantNumber.Enabled = false;
                cmbUnit.Enabled = false;
            }

            CheckEnabled(sender, EventArgs.Empty);
        }

        private void cmbItemSelection_DropDown(object sender, DropDownEventArgs e)
        {
            if (cmbItemCode.SelectedIndex == (int)TradeAgreementEntry.TradeAgreementEntryItemCode.Table)
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
                    initialSearchText = ((DataEntity)cmbItemSelection.SelectedData).Text;
                    textInitallyHighlighted = true;
                }

                e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, true, initialSearchText, SearchTypeEnum.RetailItemsMasterID, new List<RecordIdentifier>(), textInitallyHighlighted, true);
                
            }
        }

        private void cmbItemSelection_RequestData(object sender, EventArgs e)
        {
            if (cmbItemCode.SelectedIndex == (int)TradeAgreementEntry.TradeAgreementEntryItemCode.Group)
            {
                cmbItemSelection.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel, PriceDiscountModuleEnum.Item, PriceDiscGroupEnum.LineDiscountGroup),
                    null);
            }
        }

        private void cmbItemSelection_SelectedDataChanged(object sender, EventArgs e)
        {

            if (cmbItemCode.SelectedIndex == 0)
            {
                retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbItemSelection.SelectedData.ID);

                if (retailItem == null)
                {
                    throw new DataIntegrityException(typeof (RetailItem), cmbItemSelection.SelectedData.ID);
                }
                if (!RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
                {

                    cmbItemSelection.SelectedData = new DataEntity(retailItem.HeaderItemID, retailItem.Text);                    
                    
                    // This needs to be a master entity for some logic to work when pressing the OK button
                    cmbVariantNumber.SelectedData = new MasterIDEntity()
                    {
                        ID = retailItem.MasterID,
                        ReadadbleID = retailItem.ID,
                        Text = retailItem.VariantName
                    };

                }
                else
                {

                    cmbVariantNumber.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
                }

                cmbVariantNumber.Enabled = (retailItem.ItemType == ItemTypeEnum.MasterItem || retailItem.HeaderItemID != RecordIdentifier.Empty);
                lblVariant.ForeColor = cmbVariantNumber.Enabled
                    ? SystemColors.ControlText
                    : SystemColors.GrayText;


                if (cmbUnit.Enabled)
                {
                    RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel,
                        cmbItemSelection.SelectedData.ID);
                    if (item != null)
                    {
                        Unit itemSalesUnit = Providers.UnitData.Get(PluginEntry.DataModel, item.SalesUnitID);
                        if (itemSalesUnit != null)
                        {
                            cmbUnit.SelectedData = itemSalesUnit;
                        }
                    }
                }
            }

            CheckEnabled(sender, EventArgs.Empty);
        }

        private void btnAddCurrency_Click(object sender, EventArgs e)
        {
            if (currencyEditor.IsAlive)
            {
                ((IPlugin)currencyEditor.Target).Message(this, "AddCurrency", cmbCurrency.SelectedData.ID);
            }
        }

        private void btnAddUnit_Click(object sender, EventArgs e)
        {
            if (unitEditor.IsAlive)
            {
                ((IPlugin)unitEditor.Target).Message(this, "AddUnits", cmbUnit.SelectedData.ID);
            }
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
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, new List<RecordIdentifier>(), textInitallyHighlighted, true);
        }
    }
}
