using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.PeriodicDiscounts.Properties;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class DiscountOfferLineDialog : DialogBase
    {
        DiscountOffer offer;
        DiscountOfferLine offerLine;
        DiscountOfferLine.DiscountOfferTypeEnum type;
        
        decimal lastKnownOfferPrice;
        decimal lastKnownOfferPriceWithTax;
        decimal lastKnownDiscountPercent;
        decimal lastKnownDiscountAmount;
        decimal lastKnownDiscountAmountWithTax;
        decimal standardPrice;
        decimal standardPriceWithTax;

        RetailItem retailItem;
        
        public DiscountOfferLineDialog(DiscountOffer offer,RecordIdentifier lineGuid)
            : this()
        {
            this.offer = offer;

            offerLine = Providers.DiscountOfferLineData.Get(PluginEntry.DataModel,lineGuid);

            switch (offerLine.Type)
            {
                case DiscountOfferLine.DiscountOfferTypeEnum.Item:
                case DiscountOfferLine.DiscountOfferTypeEnum.Variant:
                    cmbType.SelectedIndex = 0;                    
                    cmbRelation.SelectedData = new MasterIDEntity { ID = offerLine.TargetMasterID, ReadadbleID = offerLine.ItemRelation, Text = offerLine.Text };
                    cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
                    break;
              
                case DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup:
                    cmbType.SelectedIndex = 1;
                    cmbRelation.SelectedData = new MasterIDEntity { ID = offerLine.TargetMasterID, ReadadbleID = offerLine.ItemRelation, Text = offerLine.Text };
                    cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment:
                    cmbType.SelectedIndex = 2;
                    cmbRelation.SelectedData = new MasterIDEntity { ID = offerLine.TargetMasterID, ReadadbleID = offerLine.ItemRelation, Text = offerLine.Text };
                    cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.All:
                    cmbType.SelectedIndex = 3;
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup:
                    cmbType.SelectedIndex = 4;
                    cmbRelation.SelectedData = new MasterIDEntity { ID = offerLine.TargetMasterID, ReadadbleID = offerLine.ItemRelation, Text = offerLine.Text };
                    cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
                    break;
            }

            ntbDiscountPercent.SetValueWithLimit(offerLine.DiscountPercent, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent));
            ntbDiscountPercent_Leave(null, EventArgs.Empty);
        }

        public DiscountOfferLineDialog(DiscountOffer offer)
            : this()
        {
            cmbType.SelectedIndex = 0; // Select item by default

            ntbDiscountPercent.SetValueWithLimit(offer.DiscountPercent, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent));
        
            this.offer = offer;
        }

        private DiscountOfferLineDialog()
            : base()
        {
            DecimalLimit discountPercentLimiter;
            DecimalLimit priceLimiter;

            offerLine = null;
            offer = new DiscountOffer();
            retailItem = null;

            InitializeComponent();

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            discountPercentLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);

            cmbRelation.SelectedData = new DataEntity("", "");

            ntbDiscountPercent.DecimalLetters = discountPercentLimiter.Max;
            ntbDiscountAmount.DecimalLetters = priceLimiter.Max;
            ntbStandardPrice.DecimalLetters = priceLimiter.Max;
            ntbStandardPriceWithTax.DecimalLetters = priceLimiter.Max;
            ntbOfferPrice.DecimalLetters = priceLimiter.Max;
            ntbOfferPriceWithTax.DecimalLetters = priceLimiter.Max;

            lastKnownOfferPrice = 0.0M;
            lastKnownOfferPriceWithTax = 0.0M;
            lastKnownDiscountPercent = 0.0M;
            lastKnownDiscountAmount = 0.0M;
            lastKnownDiscountAmountWithTax = 0.0M;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier OfferID
        {
            get { return offer.ID; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!Valid() || !CheckItems())
            {
                return;
            }

            if (Changed())
            {
                if (offerLine == null &&
                    GetCurrentRelationType() == DiscountOfferLine.DiscountOfferTypeEnum.All &&
                    Providers.DiscountOfferLineData.RelationExists(PluginEntry.DataModel,
                                                     offer.MasterID,
                                                     Guid.Empty,
                                                     DiscountOfferLine.DiscountOfferTypeEnum.All,
                                                     DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer))
                {
                    errorProvider3.SetError(cmbType, Resources.RelationExists);
                    return;
                }

                if (offerLine == null)
                {
                    offerLine = new DiscountOfferLine();
                    offerLine.OfferID = OfferID;
                    offerLine.OfferMasterID = offer.MasterID;
                }
                switch (cmbType.SelectedIndex)
                {
                    case 0:
                        offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.Item;

                        offerLine.Text = cmbRelation.SelectedData.Text;

                        if (cmbVariantNumber.Enabled && !RecordIdentifier.IsEmptyOrNull(cmbVariantNumber.SelectedDataID))
                        {
                            offerLine.TargetMasterID = cmbVariantNumber.SelectedData.ID;
                            offerLine.ItemRelation = PluginOperations.GetReadableItemID(cmbVariantNumber);
                        }
                        else
                        {
                            offerLine.TargetMasterID = cmbRelation.SelectedData.ID;
                            offerLine.ItemRelation = PluginOperations.GetReadableItemID(cmbRelation);
                        }

                        break;

                    case 1:
                        offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup;
                        offerLine.TargetMasterID = cmbRelation.SelectedData.ID;
                        offerLine.ItemRelation = PluginOperations.GetReadableItemID(cmbRelation);
                        offerLine.Text = cmbRelation.SelectedData.Text;
                        break;

                    case 2:
                        offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment;
                        offerLine.TargetMasterID = cmbRelation.SelectedData.ID;
                        if (cmbRelation.SelectedData is MasterIDEntity)
                        {
                            offerLine.ItemRelation = (cmbRelation.SelectedData as MasterIDEntity).ReadadbleID;
                        }
                        offerLine.Text = cmbRelation.SelectedData.Text;
                        break;

                    case 3:
                        offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.All;
                        offerLine.TargetMasterID = Guid.Empty;
                        offerLine.ItemRelation = "";
                        offerLine.Text = "";
                        break;

                    case 4:
                        offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup;
                        offerLine.TargetMasterID = cmbRelation.SelectedData.ID;
                        offerLine.ItemRelation = PluginOperations.GetReadableItemID(cmbRelation);
                        offerLine.Text = cmbRelation.SelectedData.Text;
                        break;

                }

                offerLine.DiscountPercent = ntbDiscountPercent.FullPrecisionValue;

                Providers.DiscountOfferLineData.Save(PluginEntry.DataModel, offerLine);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private DiscountOfferLine.DiscountOfferTypeEnum GetCurrentRelationType()
        {
            switch (cmbType.SelectedIndex)
            {
                case 0:
                    return DiscountOfferLine.DiscountOfferTypeEnum.Item;        

                case 1:
                    return DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup;

                case 2:
                    return DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment;

                case 3:
                    return DiscountOfferLine.DiscountOfferTypeEnum.All;

                case 4:
                    return DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup;
            }

            return DiscountOfferLine.DiscountOfferTypeEnum.Item;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool Valid()
        {
            bool enabled = true;

            if (type != DiscountOfferLine.DiscountOfferTypeEnum.All)
            {
                enabled = enabled && cmbRelation.SelectedData.ID != "";
            }

            enabled = enabled && (ntbDiscountPercent.Value > 0.0);
            return enabled;
        }

        private bool Changed()
        {
            bool changed = false;
            if (offerLine == null)
            {
                return true;
            }
            DiscountOfferLine.DiscountOfferTypeEnum newType;
            changed = false;

            if (cmbType.SelectedIndex == 0)
            {
                newType = DiscountOfferLine.DiscountOfferTypeEnum.Item;
                RecordIdentifier target;
                if (cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty)
                {
                    target = cmbVariantNumber.SelectedData.ID;

                }
                else
                {
                    target = cmbRelation.SelectedData.ID;
                }
                changed = changed || (target != offerLine.TargetMasterID);
            }
            else if (cmbType.SelectedIndex == 1)
            {
                newType = DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup;
                changed = changed || (cmbRelation.SelectedData.ID != offerLine.TargetMasterID);
            }
            else if (cmbType.SelectedIndex == 2)
            {
                newType = DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment;
                changed = changed || (cmbRelation.SelectedData.ID != offerLine.TargetMasterID);
            }
            else if (cmbType.SelectedIndex == 3)
            {
                newType = DiscountOfferLine.DiscountOfferTypeEnum.All;
            }
            else
            {
                newType = DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup;
                changed = changed || (cmbRelation.SelectedData.ID != offerLine.TargetMasterID);
            }

            changed = changed || (newType != offerLine.Type);
            changed = changed || (ntbDiscountPercent.FullPrecisionValue != offerLine.DiscountPercent);

            errorProvider1.Clear();
            return changed;
        }

        private bool DiscountPercentEnabled
        {
            set
            {
                ntbDiscountPercent.Enabled = value;
                ntbDiscountPercent.TabStop = value;
                ntbDiscountPercent.BackColor = value ? SystemColors.Window : Color.FromKnownColor(KnownColor.WhiteSmoke);
                lblDiscountPercent.ForeColor = value ? SystemColors.ControlText : SystemColors.GrayText;
            }
        }

        private bool DiscountAmountEnabled
        {
            set
            {
                ntbDiscountAmount.Enabled = value;
                ntbDiscountAmount.TabStop = value;
                ntbDiscountAmount.BackColor = value ? SystemColors.Window : Color.FromKnownColor(KnownColor.WhiteSmoke);
                lblDiscountAmount.ForeColor = value ? SystemColors.ControlText : SystemColors.GrayText;
            }
        }

        private bool DiscountAmountWithTaxEnabled
        {
            set
            {
                ntbDiscountAmountWithTax.Enabled = value;
                ntbDiscountAmountWithTax.TabStop = value;
                ntbDiscountAmountWithTax.BackColor = value ? SystemColors.Window : Color.FromKnownColor(KnownColor.WhiteSmoke);
                lblDiscountAmountWithTax.ForeColor = value ? SystemColors.ControlText : SystemColors.GrayText;
            }
        }

        private bool OfferPriceEnabled
        {
            set
            {
                ntbOfferPrice.Enabled = value;
                ntbOfferPrice.TabStop = value;
                ntbOfferPrice.BackColor = value ? SystemColors.Window : Color.FromKnownColor(KnownColor.WhiteSmoke);
                lblOfferPrice.ForeColor = value ? SystemColors.ControlText : SystemColors.GrayText;
            }
        }

        private bool OfferPriceWithTaxEnabled
        {
            set
            {
                ntbOfferPriceWithTax.Enabled = value;
                ntbOfferPriceWithTax.TabStop = value;
                ntbOfferPriceWithTax.BackColor = value ? SystemColors.Window : Color.FromKnownColor(KnownColor.WhiteSmoke);
                lblOfferPriceWithTax.ForeColor = value ? SystemColors.ControlText : SystemColors.GrayText;
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            errorProvider3.Clear();

            standardPrice = 0.0M;
            standardPriceWithTax = 0.0M;

            cmbRelation.SelectedData = new DataEntity("", "");
            
            cmbVariantNumber.SelectedData = new RetailItem();
            
            switch (cmbType.SelectedIndex)
            {
                case 0:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.Item;
                    lblRelation.Text = Resources.Item + ":";
                    lblRelation.ForeColor = SystemColors.ControlText;
                    cmbRelation.Enabled = true;
                   
                    lblVariantNumber.ForeColor = SystemColors.GrayText;
                    cmbVariantNumber.Enabled = false;

                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);

                    break;

                case 1:
                    
                    type = DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup;

                    lblRelation.Text = Resources.RetailGroup + ":";
                    lblRelation.ForeColor = SystemColors.ControlText;
                    cmbRelation.Enabled = true;

                    ntbDiscountAmount.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    ntbOfferPrice.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    ntbOfferPriceWithTax.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                    lblVariantNumber.ForeColor = SystemColors.GrayText;
                    cmbVariantNumber.Enabled = false;

                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);

                    break;

                case 2:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment;

                    lblRelation.Text = Resources.RetailDepartment + ":";
                    lblRelation.ForeColor = SystemColors.ControlText;
                    cmbRelation.Enabled = true;
                   
                    ntbDiscountAmount.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    ntbOfferPrice.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    ntbOfferPriceWithTax.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                    lblVariantNumber.ForeColor = SystemColors.GrayText;
                    cmbVariantNumber.Enabled = false;

                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);

                    break;

                case 3:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.All;

                    lblRelation.Text = Resources.Item + ":";
                    lblRelation.ForeColor = SystemColors.GrayText;
                    cmbRelation.Enabled = false;

                    ntbDiscountAmount.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    ntbOfferPrice.SetValueWithLimit(0.0M,PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    ntbOfferPriceWithTax.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                    lblVariantNumber.ForeColor = SystemColors.GrayText;
                    cmbVariantNumber.Enabled = false;

                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);

                    break;

                case 4:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup;

                    lblRelation.Text = Resources.SpecialGroup + ":";
                    lblRelation.ForeColor = SystemColors.ControlText;
                    cmbRelation.Enabled = true;

                    ntbDiscountAmount.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    ntbOfferPrice.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    ntbOfferPriceWithTax.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                    lblVariantNumber.ForeColor = SystemColors.GrayText;
                    cmbVariantNumber.Enabled = false;

                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);

                    break;
            }

            cmbRelation.ShowDropDownOnTyping = (type == DiscountOfferLine.DiscountOfferTypeEnum.Item || type == DiscountOfferLine.DiscountOfferTypeEnum.Variant);
        }

        private void cmbRelation_DropDown(object sender, DropDownEventArgs e)
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item || type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
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
                    initialSearchText = ((DataEntity)cmbRelation.SelectedData).Text;
                    textInitallyHighlighted = true;
                }
                e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, true, initialSearchText, SearchTypeEnum.RetailItemsMasterID, new List<RecordIdentifier>(), textInitallyHighlighted,true);
            }
        }

        private void cmbVariantNumber_SelectedDataChanged(object sender, EventArgs e)
        {
            errorProvider3.Clear();
            errorProvider2.Clear();
            errorProvider1.Clear();

            RecordIdentifier selectedItem = RecordIdentifier.IsEmptyOrNull(cmbVariantNumber.SelectedData.ID) ? cmbRelation.SelectedData.ID : cmbVariantNumber.SelectedData.ID;
            retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, selectedItem);

            if (retailItem == null)
            {
                throw new DataIntegrityException(typeof(RetailItem), cmbVariantNumber.SelectedData.ID);
            }

            standardPrice = retailItem.SalesPrice;
            standardPriceWithTax = standardPrice + Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTax(PluginEntry.DataModel, retailItem);

            DiscountPercentEnabled = true;
            DiscountAmountEnabled = true;
            DiscountAmountWithTaxEnabled = true;
            OfferPriceEnabled = true;
            OfferPriceWithTaxEnabled = true;

            ntbStandardPrice.Value = (double)standardPrice;
            ntbStandardPriceWithTax.Value = (double)standardPriceWithTax;

            lastKnownDiscountPercent = 0.0M;
            ntbDiscountPercent_Leave(null, EventArgs.Empty);
        }

        private void cmbRelation_SelectedDataChanged(object sender, EventArgs e)
        {
            bool defaultStoreExists = true;
            bool defaultStoreHasTaxGroup = true;
            bool itemHasTaxGroup = true;            

            if (cmbRelation.SelectedData.ID != "")
            {
                if (type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
                {
                    lblVariantNumber.ForeColor = SystemColors.ControlText;
                    cmbVariantNumber.Enabled = true;
                    cmbVariantNumber.SelectedData = new RetailItem();

                    retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbRelation.SelectedData.ID);

                    standardPrice = retailItem.SalesPrice;
                    standardPriceWithTax = standardPrice + Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTax(PluginEntry.DataModel, retailItem);

                    DiscountPercentEnabled = cmbVariantNumber.SelectedData.ID != "";
                    DiscountAmountEnabled = 
                    DiscountAmountWithTaxEnabled = 
                    OfferPriceEnabled = 
                    OfferPriceWithTaxEnabled = standardPrice != 0.0M && cmbVariantNumber.SelectedData.ID != "";
                }
                else if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item)
                {
                    retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbRelation.SelectedData.ID);
                    
                    if (retailItem == null)
                    {
                        throw new DataIntegrityException(typeof(RetailItem), cmbRelation.SelectedData.ID);
                    }

                    if (!RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
                    {
                        cmbRelation.SelectedData = new DataEntity(retailItem.HeaderItemID, retailItem.Text);
                        cmbVariantNumber.SelectedData = new DataEntity(retailItem.MasterID, retailItem.VariantName);
                    }
                    else
                    {
                        cmbVariantNumber.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
                    }

                    cmbVariantNumber.Enabled = retailItem.ItemType == ItemTypeEnum.MasterItem || !RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID);
                        lblVariantNumber.ForeColor = cmbVariantNumber.Enabled
                            ? SystemColors.ControlText
                            : SystemColors.GrayText;
                    
                    standardPrice = retailItem.SalesPrice;
                    standardPriceWithTax = standardPrice + Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTax(PluginEntry.DataModel, retailItem);

                    DiscountPercentEnabled = true;
                    DiscountAmountEnabled = 
                    DiscountAmountWithTaxEnabled =
                    OfferPriceEnabled =
                    OfferPriceWithTaxEnabled = standardPrice != 0.0M;

                    if (standardPrice != 0.0M)
                    {
                        if (lastKnownDiscountPercent != ntbDiscountPercent.FullPrecisionValue)
                        {
                            lastKnownDiscountPercent = ntbDiscountPercent.FullPrecisionValue;

                            lastKnownOfferPrice =
                                Math.Round(standardPrice - (standardPrice * (lastKnownDiscountPercent / 100.0M)),
                                    ntbOfferPrice.DecimalLetters);
                            lastKnownOfferPriceWithTax =
                                Math.Round(
                                    Services.Interfaces.Services.TaxService(PluginEntry.DataModel)
                                        .GetItemTaxForAmount(PluginEntry.DataModel, retailItem.SalesTaxItemGroupID, lastKnownOfferPrice) +
                                    lastKnownOfferPrice, ntbOfferPriceWithTax.DecimalLetters);

                            lastKnownDiscountAmount = Math.Round(standardPrice - lastKnownOfferPrice,
                                ntbDiscountAmount.DecimalLetters);
                            lastKnownDiscountAmountWithTax = Math.Round(standardPriceWithTax - lastKnownOfferPriceWithTax,
                                ntbDiscountAmountWithTax.DecimalLetters);

                            ntbOfferPrice.SetValueWithLimit(lastKnownOfferPrice,
                                PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                            ntbDiscountAmount.SetValueWithLimit(lastKnownDiscountAmount,
                                PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                            ntbOfferPriceWithTax.SetValueWithLimit(lastKnownOfferPriceWithTax,
                                PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                            ntbDiscountAmountWithTax.SetValueWithLimit(lastKnownDiscountAmountWithTax,
                                PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        }
                    }
                    else
                    {
                        ntbOfferPrice.SetValueWithLimit(0, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbDiscountAmount.SetValueWithLimit(0, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbOfferPriceWithTax.SetValueWithLimit(0, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbDiscountAmountWithTax.SetValueWithLimit(0, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    }
                }
                else
                {
                    DiscountPercentEnabled = true;
                    DiscountAmountEnabled = false;
                    DiscountAmountWithTaxEnabled = false;
                    OfferPriceEnabled = false;
                    OfferPriceWithTaxEnabled = false;
                }

                if (defaultStoreExists && defaultStoreHasTaxGroup && itemHasTaxGroup)
                {
                    errorProvider2.Clear();
                }
                else
                {
                    if (!defaultStoreExists)
                    {
                        errorProvider2.SetError(cmbRelation, Resources.NoDefaultStoreSelected);
                    }

                    if (!defaultStoreHasTaxGroup)
                    {
                        errorProvider2.SetError(cmbRelation, Resources.DefaultStoreHasNoTaxGroupAttachedToIt);
                    }

                    if (!itemHasTaxGroup)
                    {
                        errorProvider2.SetError(cmbRelation, Resources.ItemHasNoTaxGroupAttachedToIt);
                    }
                }
            }
            else
            {
                if (type != DiscountOfferLine.DiscountOfferTypeEnum.All)
                {
                    DiscountPercentEnabled = false;
                    DiscountAmountEnabled = false;
                    DiscountAmountWithTaxEnabled = false;
                    OfferPriceEnabled = false;
                    OfferPriceWithTaxEnabled = false;
                }
                else
                {
                    DiscountPercentEnabled = true;
                    DiscountAmountEnabled = false;
                    DiscountAmountWithTaxEnabled = false;
                    OfferPriceEnabled = false;
                    OfferPriceWithTaxEnabled = false;
                }

                errorProvider2.Clear();
            }

            ntbStandardPrice.Value = (double)standardPrice;
            ntbStandardPriceWithTax.Value = (double)standardPriceWithTax;

            lastKnownDiscountPercent = 0.0M;
        }

        private bool CheckItems()
        {
            RecordIdentifier target;
            errorProvider3.Clear();

            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item)
            {
                if (cmbVariantNumber.SelectedData != null && cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty)
                {
                    target = cmbVariantNumber.SelectedData.ID;
                }
                else
                {
                    target = cmbRelation.SelectedData.ID;
                }

                bool relationExists = Providers.DiscountOfferLineData.RelationExists(PluginEntry.DataModel, offer.MasterID, target,
                    DiscountOfferLine.DiscountOfferTypeEnum.Item, offer.OfferType);

                if ((offerLine == null || offerLine.TargetMasterID != target.PrimaryID) && relationExists)
                {
                    if (cmbVariantNumber.SelectedData != null && cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty)
                    {
                        errorProvider3.SetError(cmbVariantNumber, Resources.RelationExists);
                    }
                    else
                    {
                        errorProvider3.SetError(cmbRelation, Resources.RelationExists);
                    }
                    return false;
                }
            }
            return true;
        }
        
        private void cmbRelation_RequestData(object sender, EventArgs e)
        {
            List<RecordIdentifier> excludedItemIds = GetIDsToExclude(type);

            // We do not need to exclude the id that is already in use by this line
            if (offerLine != null)
            {
                excludedItemIds.Remove(offerLine.TargetMasterID);
            }

            if (type == DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment)
            {
                cmbRelation.SetData(Providers.RetailDepartmentData.GetMasterIDList(PluginEntry.DataModel), null, excludedItemIds);
            }
            else if (type == DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup)
            {
                cmbRelation.SetData(Providers.RetailGroupData.GetMasterIDList(PluginEntry.DataModel), null, excludedItemIds);
            }
            else if (type == DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup)
            {
                cmbRelation.SetData(Providers.SpecialGroupData.GetMasterIDList(PluginEntry.DataModel), null, excludedItemIds);
            }
        }

        private List<RecordIdentifier> GetIDsToExclude(DiscountOfferLine.DiscountOfferTypeEnum typeEnum)
        {
            int lineCount;
            List<DiscountOfferLine> lines = Providers.DiscountOfferLineData.GetLines(PluginEntry.DataModel, OfferID, DiscountLineSortEnum.ProductType, 
                 Int32.MaxValue, out lineCount);

            return (from l in lines
                    where l.Type == typeEnum
                    select l.TargetMasterID).ToList();
        }

        private void cmbVariantNumber_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity(RecordIdentifier.Empty, "");
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

        private void ntbDiscountPercent_Leave(object sender, EventArgs e)
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item ||
                type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (standardPrice != 0.0M)
                {
                    if (lastKnownDiscountPercent != ntbDiscountPercent.FullPrecisionValue)
                    {
                        lastKnownDiscountPercent = ntbDiscountPercent.FullPrecisionValue;

                        lastKnownOfferPrice =
                            Math.Round(standardPrice - (standardPrice * (lastKnownDiscountPercent / 100.0M)),
                                ntbOfferPrice.DecimalLetters);
                        lastKnownOfferPriceWithTax =
                            Math.Round(
                                Services.Interfaces.Services.TaxService(PluginEntry.DataModel)
                                    .GetItemTaxForAmount(PluginEntry.DataModel, retailItem.SalesTaxItemGroupID, lastKnownOfferPrice) +
                                lastKnownOfferPrice, ntbOfferPriceWithTax.DecimalLetters);

                        lastKnownDiscountAmount = Math.Round(standardPrice - lastKnownOfferPrice,
                            ntbDiscountAmount.DecimalLetters);
                        lastKnownDiscountAmountWithTax = Math.Round(standardPriceWithTax - lastKnownOfferPriceWithTax,
                            ntbDiscountAmountWithTax.DecimalLetters);

                        ntbOfferPrice.SetValueWithLimit(lastKnownOfferPrice,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                        ntbDiscountAmount.SetValueWithLimit(lastKnownDiscountAmount,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbOfferPriceWithTax.SetValueWithLimit(lastKnownOfferPriceWithTax,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                        ntbDiscountAmountWithTax.SetValueWithLimit(lastKnownDiscountAmountWithTax,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    }
                }
            }
        }

        private void ntbDiscountAmount_Leave(object sender, EventArgs e)
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item ||
                type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (standardPrice != 0.0M)
                {
                    if (lastKnownDiscountAmount != ntbDiscountAmount.FullPrecisionValue)
                    {
                        lastKnownDiscountAmount = ntbDiscountAmount.FullPrecisionValue;

                        lastKnownOfferPrice = Math.Round(standardPrice - lastKnownDiscountAmount,
                            ntbOfferPrice.DecimalLetters);
                        lastKnownOfferPriceWithTax =
                            Math.Round(
                                Services.Interfaces.Services.TaxService(PluginEntry.DataModel)
                                    .GetItemTaxForAmount(PluginEntry.DataModel, retailItem.SalesTaxItemGroupID, lastKnownOfferPrice) +
                                lastKnownOfferPrice, ntbOfferPriceWithTax.DecimalLetters);

                        lastKnownDiscountAmountWithTax = Math.Round(standardPriceWithTax - lastKnownOfferPriceWithTax,
                            ntbDiscountAmountWithTax.DecimalLetters);

                        lastKnownDiscountPercent = (1 - (lastKnownOfferPrice / standardPrice)) * 100.0M;

                        ntbOfferPrice.SetValueWithLimit(lastKnownOfferPrice,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbDiscountPercent.SetValueWithLimit(lastKnownDiscountPercent,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent));
                        ntbDiscountAmountWithTax.SetValueWithLimit(lastKnownDiscountAmountWithTax,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbOfferPriceWithTax.SetValueWithLimit(lastKnownOfferPriceWithTax,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    }
                }
            }
        }

        private void ntbDiscountAmountWithTax_Leave(object sender, EventArgs e)
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item ||
                type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (standardPrice != 0.0M)
                {
                    if (lastKnownDiscountAmountWithTax != ntbDiscountAmountWithTax.FullPrecisionValue)
                    {
                        lastKnownDiscountAmountWithTax = ntbDiscountAmountWithTax.FullPrecisionValue;

                        lastKnownOfferPriceWithTax = Math.Round(standardPriceWithTax - lastKnownDiscountAmountWithTax,
                            ntbOfferPriceWithTax.DecimalLetters);
                        lastKnownOfferPrice =
                            Services.Interfaces.Services.TaxService(PluginEntry.DataModel)
                                .GetItemPriceForItemPriceWithTax(PluginEntry.DataModel, retailItem.SalesTaxItemGroupID,
                                    lastKnownOfferPriceWithTax);

                        lastKnownDiscountAmount = Math.Round(standardPrice - lastKnownOfferPrice,
                            ntbDiscountAmount.DecimalLetters);

                        lastKnownDiscountPercent = (1 - (lastKnownOfferPrice / standardPrice)) * 100.0M;

                        ntbOfferPrice.SetValueWithLimit(lastKnownOfferPrice,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbDiscountPercent.SetValueWithLimit(lastKnownDiscountPercent,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent));
                        ntbDiscountAmount.SetValueWithLimit(lastKnownDiscountAmount,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbOfferPriceWithTax.SetValueWithLimit(lastKnownOfferPriceWithTax,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    }
                }
            }
        }

        private void ntbOfferPrice_Leave(object sender, EventArgs e)
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item ||
                type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (standardPrice != 0.0M)
                {
                    if (lastKnownOfferPrice != ntbOfferPrice.FullPrecisionValue)
                    {
                        lastKnownOfferPrice = (decimal)ntbOfferPrice.FullPrecisionValue;
                        lastKnownOfferPriceWithTax =
                            Math.Round(
                                Services.Interfaces.Services.TaxService(PluginEntry.DataModel)
                                    .GetItemTaxForAmount(PluginEntry.DataModel, retailItem.SalesTaxItemGroupID, lastKnownOfferPrice) +
                                lastKnownOfferPrice, ntbOfferPriceWithTax.DecimalLetters);

                        lastKnownDiscountAmount = Math.Round(standardPrice - lastKnownOfferPrice,
                            ntbDiscountAmount.DecimalLetters);
                        lastKnownDiscountAmountWithTax = Math.Round(standardPriceWithTax - lastKnownOfferPriceWithTax,
                            ntbDiscountAmountWithTax.DecimalLetters);

                        lastKnownDiscountPercent = (1 - (lastKnownOfferPrice / standardPrice)) * 100.0M;

                        ntbDiscountAmount.SetValueWithLimit(lastKnownDiscountAmount,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbDiscountAmountWithTax.SetValueWithLimit(lastKnownDiscountAmountWithTax,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbDiscountPercent.SetValueWithLimit(lastKnownDiscountPercent,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent));
                        ntbOfferPriceWithTax.SetValueWithLimit(lastKnownOfferPriceWithTax,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    }
                }
            }
        }

        private void ntbOfferPriceWithTax_Leave(object sender, EventArgs e)
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item ||
                type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (standardPrice != 0.0M)
                {
                    if (lastKnownOfferPriceWithTax != ntbOfferPriceWithTax.FullPrecisionValue)
                    {
                        lastKnownOfferPriceWithTax = ntbOfferPriceWithTax.FullPrecisionValue;
                        lastKnownOfferPrice =
                            Services.Interfaces.Services.TaxService(PluginEntry.DataModel)
                                .GetItemPriceForItemPriceWithTax(PluginEntry.DataModel, retailItem.SalesTaxItemGroupID,
                                    lastKnownOfferPriceWithTax);

                        lastKnownDiscountAmount = Math.Round(standardPrice - lastKnownOfferPrice,
                            ntbDiscountAmount.DecimalLetters);
                        lastKnownDiscountAmountWithTax = Math.Round(standardPriceWithTax - lastKnownOfferPriceWithTax,
                            ntbDiscountAmountWithTax.DecimalLetters);

                        lastKnownDiscountPercent = (1 - (lastKnownOfferPrice / standardPrice)) * 100.0M;

                        ntbDiscountAmount.SetValueWithLimit(lastKnownDiscountAmount,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbDiscountAmountWithTax.SetValueWithLimit(lastKnownDiscountAmountWithTax,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbDiscountPercent.SetValueWithLimit(lastKnownDiscountPercent,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent));
                        ntbOfferPrice.SetValueWithLimit(lastKnownOfferPrice,
                            PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    }
                }
            }
        }
    }
}