using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class TradeAgreemenItemLineDiscountDialog : DialogBase
    {
        RecordIdentifier storeID;
        RecordIdentifier itemID;
        RetailItem item;
        TradeAgreementEntry agreement;
        WeakReference unitEditor;
        WeakReference currencyEditor;

        public TradeAgreemenItemLineDiscountDialog(RecordIdentifier itemID,RecordIdentifier agreementID)
            : this(itemID)
        {
            DecimalLimit priceLimiter;
            Unit unit;

            agreement = Providers.TradeAgreementData.Get(PluginEntry.DataModel, agreementID, TradeAgreementRelation.LineDiscSales);

            
            cmbCurrency.SelectedData = new DataEntity(agreement.Currency, agreement.CurrencyDescription);

            cmbAccountCode.SelectedIndex = (int)agreement.AccountCode;
            cmbAccountSelection.SelectedData = new DataEntity(agreement.AccountRelation,agreement.AccountName);

            
            if (agreement.ItemID != itemID)
            {
                RetailItem variant =  Providers.RetailItemData.Get(PluginEntry.DataModel, agreement.ItemID);
                if (variant != null && !RecordIdentifier.IsEmptyOrNull(variant.HeaderItemID))
                {
                    RetailItem header = Providers.RetailItemData.Get(PluginEntry.DataModel, variant.HeaderItemID);
                    if (header.ID == itemID)
                    {
                        MasterIDEntity variantEntity = new MasterIDEntity
                        {
                            ID = variant.MasterID,
                            Text = variant.VariantName,
                            ReadadbleID = variant.ID
                        };
                        cmbVariantNumber.SelectedData = variantEntity;
                    }
                    else
                    {
                        throw new Exception("Variant does not belong to header item.");
                    }
                }
                else
                {
                    throw new Exception("Agreement does not belong to item.");
                }

            }

            unit = Providers.UnitData.Get(PluginEntry.DataModel, agreement.UnitID);

            if (unit == null)
            {
                unit = new Unit(agreement.UnitID, (string)agreement.UnitID, 0,0);
            }

            cmbUnit.SelectedData = unit;
            cmbUnit_SelectedDataChanged(this, EventArgs.Empty);

            agreement.FromDate.ToDateControl(dtpFromDate);
            agreement.ToDate.ToDateControl(dtpToDate);

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            ntbQuantity.Text = agreement.QuantityAmount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity));
            ntbDiscount.Text = agreement.Amount.FormatWithLimits(priceLimiter);
            ntbDiscountPercentage1.Value = (double)agreement.Percent1;
            ntbDiscountPercentage2.Value = (double)agreement.Percent2;

            CheckEnabled(this, EventArgs.Empty);
        }

        public TradeAgreemenItemLineDiscountDialog(RecordIdentifier itemID)
            : this()
        {
            agreement = null;
            CompanyInfo companyInfo;

            this.itemID = itemID;

            cmbUnit.SelectedData = new Unit();

            if(itemID != RecordIdentifier.Empty)
            {
                // If we are not in multiedit mode
                item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);
                Unit itemSalesUnit = Providers.UnitData.Get(PluginEntry.DataModel, item.SalesUnitID);

                cmbUnit.SelectedData = itemSalesUnit;

                Text += " - " + item.Text;
            }
            
            companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel,false);

            cmbCurrency.SelectedData = new DataEntity(companyInfo.CurrencyCode,companyInfo.CurrencyCodeText);
            cmbAccountSelection.SelectedData = new DataEntity("","");
            cmbAccountCode.SelectedIndex = 2;

            item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);

            if (item.ItemType == ItemTypeEnum.MasterItem)
            {
                cmbVariantNumber.Enabled = true;
                if (!(cmbVariantNumber.SelectedData is MasterIDEntity))
                {
                    cmbVariantNumber.SelectedData = new MasterIDEntity();
                }
                lblVariant.ForeColor = SystemColors.ControlText;
            }
            else
            {
                cmbVariantNumber.Enabled = false;
                lblVariant.ForeColor = SystemColors.GrayText;
            }

            cmbVariantNumber.SelectedData = new MasterIDEntity();

            
        }

        private TradeAgreemenItemLineDiscountDialog()
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

        public TradeAgreementEntry Agreement
        {
            get
            {
                return agreement;
            }
        }

        private void PopulateTAEntry(TradeAgreementEntry taEntry)
        {
            taEntry.Currency = cmbCurrency.SelectedData.ID;
            taEntry.AccountCode = (TradeAgreementEntryAccountCode)cmbAccountCode.SelectedIndex;
            taEntry.AccountRelation = cmbAccountSelection.SelectedData.ID;
            taEntry.UnitID = cmbUnit.SelectedData.ID;
            taEntry.FromDate = Date.FromDateControl(dtpFromDate).GetDateWithoutTime();
            taEntry.QuantityAmount = (decimal)ntbQuantity.Value;
            taEntry.Relation = TradeAgreementEntry.TradeAgreementEntryRelation.LineDiscSales;
            taEntry.ItemCode = TradeAgreementEntry.TradeAgreementEntryItemCode.Table;
            RecordIdentifier target;
            if (cmbVariantNumber.SelectedData != null && cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty)
            {
                target = (cmbVariantNumber.SelectedData as MasterIDEntity).ReadadbleID;

            }
            else
            {
                target = item.ID;
            }
            taEntry.ItemRelation = target;
            taEntry.ToDate = Date.FromDateControl(dtpToDate).GetDateWithoutTime();
            taEntry.Amount = (decimal)ntbDiscount.Value;
            taEntry.Percent1 = (decimal)ntbDiscountPercentage1.Value;
            taEntry.Percent2 = (decimal)ntbDiscountPercentage2.Value;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Date.FromDateControl(dtpFromDate) > Date.FromDateControl(dtpToDate) && !Date.FromDateControl(dtpToDate).IsEmpty)
            {
                errorProvider1.SetError(dtpFromDate, Properties.Resources.FromDateCannotBeHigherThanToDate);
                return;
            }

            if (itemID != RecordIdentifier.Empty)
            {
                // If we are in single edit mode

                if (!PluginOperations.ConversionRuleToInventoryUnitExists(item, cmbUnit.SelectedData.ID))
                {
                    return;
                }

                if (agreement == null)
                {
                    TradeAgreementEntry testEntry = new TradeAgreementEntry();
                    PopulateTAEntry(testEntry);
                    if (Providers.TradeAgreementData.DataContentExists(PluginEntry.DataModel, testEntry.OldID))
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
            }
            else
            {
                // Multiedit!
                PopulateTAEntry(agreement);
            }

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
            cmbCurrency.SetData(Providers.CurrencyData.
                GetList(PluginEntry.DataModel), null);
        }

        private void cmbAccountCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbAccountSelection.Enabled = cmbAccountCode.SelectedIndex != 2;

            cmbAccountSelection.SelectedData = new DataEntity("", "");

            if (cmbAccountCode.SelectedIndex == 2)
            {
                cmbAccountSelection.SetUseInternalDropDown(true);
            }
            else if (cmbAccountCode.SelectedIndex == 0)
            {
                cmbAccountSelection.SetUseInternalDropDown(false);
            }
            else
            {
                cmbAccountSelection.SetUseInternalDropDown(true);
            }

            CheckEnabled(sender, EventArgs.Empty);
        }

        private void cmbAccountSelection_DropDown(object sender, DropDownEventArgs e)
        {
            if (cmbAccountCode.SelectedIndex == 0)
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
                    initialSearchText = ((DataEntity)cmbAccountSelection.SelectedData).ID;
                    textInitallyHighlighted = true;
                }

                e.ControlToEmbed = new SingleSearchPanel(
                    PluginEntry.DataModel,
                    false,
                    initialSearchText,
                    SearchTypeEnum.Customers,
                    textInitallyHighlighted);
            }
            else if (cmbAccountCode.SelectedIndex == 1)
            {
                
            }
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            TabbedDualDataPanel pnl;
            IEnumerable<DataEntity>[] data;

            cmbUnit.SetWidth(200);

            if(itemID != RecordIdentifier.Empty)
            {
                // If we are not in multiedit mode
                RecordIdentifier salesUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemID, RetailItem.UnitTypeEnum.Sales);

                data = new IEnumerable<DataEntity>[]
                        { Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel,itemID,salesUnitID),
                      Providers.UnitData.GetAllUnits(PluginEntry.DataModel).Cast<DataEntity>()};

                pnl = new TabbedDualDataPanel(
                    cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                    data,
                    new string[] { Properties.Resources.Convertible, Properties.Resources.All },
                    250);
            }
            else
            {
                // We are in multiedit mode
                data = new IEnumerable<DataEntity>[]
                        {Providers.UnitData.GetAllUnits(PluginEntry.DataModel).Cast<DataEntity>()};

                pnl = new TabbedDualDataPanel(
                    cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                    data,
                    new string[] { Properties.Resources.All },
                    250);
            }
            

            cmbUnit.SetData(data, pnl);
        }

        private void cmbUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            if(cmbUnit.SelectedData.ID != "")
            {
                ntbQuantity.AllowDecimal = ((Unit)cmbUnit.SelectedData).MaximumDecimals > 0;
                ntbQuantity.DecimalLetters = ((Unit)cmbUnit.SelectedData).MaximumDecimals;
            }

            CheckEnabled(sender, EventArgs.Empty);
        }

        private void cmbAccountSelection_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (agreement == null)
            {
                btnOK.Enabled = cmbCurrency.SelectedData.ID != "" &&
                    cmbAccountCode.SelectedIndex >= 0 &&
                    ((cmbAccountSelection.Enabled && cmbAccountSelection.SelectedData.ID != "") || !cmbAccountSelection.Enabled) &&
                    cmbUnit.SelectedData.ID != "";
            }
            else
            {
                btnOK.Enabled = 
                    (
                        cmbCurrency.SelectedData.ID != "" 
                        && cmbAccountCode.SelectedIndex >= 0 
                        && ((
                                cmbAccountSelection.Enabled 
                                && cmbAccountSelection.SelectedData.ID != ""
                            ) 
                            || !cmbAccountSelection.Enabled
                        ) 
                    && cmbUnit.SelectedData.ID != "") 
                    && (
                        cmbCurrency.SelectedData.ID != agreement.Currency 
                        || (TradeAgreementEntryAccountCode)cmbAccountCode.SelectedIndex != agreement.AccountCode 
                        || cmbAccountSelection.SelectedData.ID != agreement.AccountRelation 
                        || (RecordIdentifier.IsEmptyOrNull( ((MasterIDEntity)cmbVariantNumber.SelectedData).ReadadbleID )
                            ? itemID 
                            : ((MasterIDEntity)cmbVariantNumber.SelectedData).ReadadbleID) != agreement.ItemID 
                        || cmbUnit.SelectedData.ID != agreement.UnitID
                        || Date.FromDateControl(dtpFromDate).GetDateWithoutTime() != agreement.FromDate
                        || Date.FromDateControl(dtpToDate).GetDateWithoutTime() != agreement.ToDate 
                        || (decimal)ntbQuantity.Value != agreement.QuantityAmount 
                        || (decimal)ntbDiscount.Value != agreement.Amount 
                        || (decimal)ntbDiscountPercentage1.Value != agreement.Percent1 
                        || (decimal)ntbDiscountPercentage2.Value != agreement.Percent2
                    );
            }
        }

       

      
        private void cmbVariantNumber_RequestClear(object sender, EventArgs e)
        {
            cmbVariantNumber.SelectedData = new MasterIDEntity();
            
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

        private void cmbAccountSelection_RequestClear(object sender, EventArgs e)
        {

        }

        private void cmbAccountSelection_RequestData(object sender, EventArgs e)
        {
            if (cmbAccountCode.SelectedIndex == 1)
            {
                cmbAccountSelection.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel, PriceDiscountModuleEnum.Customer, PriceDiscGroupEnum.LineDiscountGroup), null);
            }
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void btnAddUnit_Click(object sender, EventArgs e)
        {
            if (unitEditor.IsAlive)
            {
                ((IPlugin)unitEditor.Target).Message(this, "AddUnits", cmbUnit.SelectedData.ID);
            }
        }

        private void btnAddCurrency_Click(object sender, EventArgs e)
        {
            if (currencyEditor.IsAlive)
            {
                ((IPlugin)currencyEditor.Target).Message(this, "AddCurrency", cmbCurrency.SelectedData.ID);
            }
        }

        private void tbSize_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);
        }

        private void tbColor_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);
        }

        private void tbStyle_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);
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
                item.ItemType == ItemTypeEnum.MasterItem ?
                item.MasterID :
                item.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, new List<RecordIdentifier>(), textInitallyHighlighted, true);

        }
    }
}
