using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class PromotionLineDialog : DialogBase
    {
        DiscountOffer offer;
        PromotionOfferLine offerLine;
        DiscountOfferLine.DiscountOfferTypeEnum type;

        decimal lastKnownOfferPrice;
        decimal lastKnownOfferPriceWithTax;
        decimal lastKnownDiscountPercent;
        decimal lastKnownDiscountAmount;
        decimal lastKnownDiscountAmountWithTax;
        decimal standardPrice;
        decimal standardPriceWithTax;

        RetailItem retailItem;

        public static PromotionLineDialog CreateForEditing(RecordIdentifier offerID, RecordIdentifier lineID)
        {
            return new PromotionLineDialog(offerID, lineID);
        }

        public static PromotionLineDialog CreateForNew(RecordIdentifier offerID)
        {
            return new PromotionLineDialog(offerID);
        }

        private PromotionLineDialog(RecordIdentifier offerID, RecordIdentifier lineID)
            : this()
        {
            DiscountOffer offer = Providers.DiscountOfferData.Get(PluginEntry.DataModel, offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion);

            this.offer = offer;

            offerLine = Providers.DiscountOfferLineData.GetPromotion(PluginEntry.DataModel, lineID);

            switch (offerLine.Type)
            {
                case DiscountOfferLine.DiscountOfferTypeEnum.Variant:
                case DiscountOfferLine.DiscountOfferTypeEnum.Item:
                    cmbType.SelectedIndex = 0;

                    cmbRelation.SelectedData = new MasterIDEntity { ReadadbleID = offerLine.ItemRelation, ID = offerLine.TargetMasterID, Text = offerLine.Text };
                    cmbRelation_SelectedDataChanged(null, EventArgs.Empty);

                    if (offerLine.ItemIsVariant)
                    {
                        cmbVariantNumber.SelectedData = new MasterIDEntity { ReadadbleID = offerLine.ItemRelation, ID = offerLine.TargetMasterID, Text = (offerLine.VariantName == "" ? "< >" : offerLine.VariantName) };
                    }
              
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup:
                    cmbType.SelectedIndex = 1;
                    cmbRelation.SelectedData = new MasterIDEntity() { ID = offerLine.TargetMasterID, Text = offerLine.Text, ReadadbleID = offerLine.ItemRelation };
                    cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment:
                    cmbType.SelectedIndex = 2;
                    cmbRelation.SelectedData = new MasterIDEntity() { ID = offerLine.TargetMasterID, Text = offerLine.Text, ReadadbleID = offerLine.ItemRelation };
                    cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.All:
                    cmbType.SelectedIndex = 3;
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup:
                    cmbType.SelectedIndex = 4;
                    cmbRelation.SelectedData = new MasterIDEntity() { ID = offerLine.TargetMasterID, Text = offerLine.Text, ReadadbleID = offerLine.ItemRelation };
                    cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
                    break;
            }

            if (offerLine.DiscountPercent == 0.0M && offerLine.OfferPrice != 0.0M)
            {
                optPriceValue.Checked = true;

                ntbOfferPrice.SetValueWithLimit(offerLine.OfferPrice, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                ntbOfferPrice_Leave(null, EventArgs.Empty);
            }
            else
            {
                optDiscountValue.Checked = true;

                ntbDiscountPercent.SetValueWithLimit(offerLine.DiscountPercent, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent));
                ntbDiscountPercent_Leave(null, EventArgs.Empty);
            }
        }

        private PromotionLineDialog(RecordIdentifier offerID)
            : this()
        {
            DiscountOffer offer = Providers.DiscountOfferData.Get(PluginEntry.DataModel, offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion);

            this.offer = offer;
            cmbType.SelectedIndex = 0; // Select item by default

            this.offer = offer;
            ntbDiscountPercent.SetValueWithLimit(offer.DiscountPercent, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent));
        }

        private PromotionLineDialog()
            : base()
        {
            DecimalLimit discountPercentLimiter;
            DecimalLimit priceLimiter;

            offerLine = null;
            offer = null;
            retailItem = null;

            InitializeComponent();

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            discountPercentLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);

            cmbRelation.SelectedData = new MasterIDEntity() { ID = RecordIdentifier.Empty, ReadadbleID = RecordIdentifier.Empty, Text = "" };

            ntbDiscountPercent.DecimalLetters = discountPercentLimiter.Max;
            ntbDiscountAmount.DecimalLetters = priceLimiter.Max;
            ntbDiscountAmountWithTax.DecimalLetters = priceLimiter.Max;
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            optDiscountValue.TabStop = true;
            optPriceValue.TabStop = true;
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
            // This has to be done to force fields to update based on other fields.
            btnOK.Focus();
            bool checksPassed = CheckDiscountPercent();
            checksPassed = checksPassed && CheckDiscountAmount();
            checksPassed = checksPassed && CheckDiscountAmountWithTax();

            // We check here in between so we dont get two error providers
            if (!checksPassed)
            {
                return; 
            }

            checksPassed = checksPassed && CheckItems();

            // We check here again in between so we dont get two error providers
            if (!checksPassed)
            {
                return;
            }

            if (!Valid())
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
                    errorProvider3.SetError(cmbType, Properties.Resources.RelationExists);
                    return;
                }

                if (offerLine == null)
                {
                    offerLine = new PromotionOfferLine();
                    offerLine.OfferID = offer.ID;
                    offerLine.OfferMasterID = offer.MasterID;
                }

                switch (cmbType.SelectedIndex)
                {
                    case 0:
                        offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.Item;
                        offerLine.TargetMasterID = cmbRelation.SelectedData.ID;
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
                        offerLine.ItemRelation = PluginOperations.GetReadableItemID(cmbRelation);
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

                if (optPriceValue.Checked)
                {
                    offerLine.OfferPrice = ntbOfferPrice.FullPrecisionValue;
                    offerLine.OfferPriceIncludeTax = ntbOfferPriceWithTax.FullPrecisionValue;

                    offerLine.DiscountPercent = 0.0M;
                    offerLine.DiscountAmount = 0.0M;
                    offerLine.DiscountamountIncludeTax = 0.0M;
                }
                else
                {
                    offerLine.OfferPrice = 0.0M;
                    offerLine.OfferPriceIncludeTax = 0.0M;

                    offerLine.DiscountPercent = ntbDiscountPercent.FullPrecisionValue;
                    offerLine.DiscountAmount = ntbDiscountAmount.FullPrecisionValue;
                    offerLine.DiscountamountIncludeTax = ntbDiscountAmountWithTax.FullPrecisionValue;
                }

                Providers.DiscountOfferLineData.Save(PluginEntry.DataModel, offerLine);

                DialogResult = DialogResult.OK;
                Close();
            }
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
                enabled = enabled && cmbRelation.SelectedData.ID != RecordIdentifier.Empty;
            }
            
            if (optDiscountValue.Checked)
            {
                enabled &= (ntbDiscountPercent.FullPrecisionValue > 0.0M);

                if (!enabled)
                {
                    errorProvider1.SetError(lnkDiscountValueLinks, Properties.Resources.DiscountValuesAreMissing);
                }

                enabled &= ntbDiscountAmount.FullPrecisionValue <= standardPrice;

                if (!enabled)
                {
                    errorProvider1.SetError(lnkDiscountValueLinks,Properties.Resources.AmountMayNotBeHigherThanBasePrice);
                }
            }
            else
            {
                enabled = enabled && (ntbOfferPrice.FullPrecisionValue > 0.0M);

                if (!enabled)
                {
                    errorProvider1.SetError(lnkPriceValueLinks, Properties.Resources.PriceValuesAreMissing);
                }
            }
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
            changed = changed || ((decimal)ntbDiscountPercent.Value != offerLine.DiscountPercent);
            changed = changed || ((decimal)ntbDiscountAmount.Value != offerLine.DiscountAmount);
            changed = changed || ((decimal)ntbDiscountAmountWithTax.Value != offerLine.DiscountamountIncludeTax);
            changed = changed || ((decimal)ntbOfferPrice.Value != offerLine.OfferPrice);
            changed = changed || ((decimal)ntbOfferPriceWithTax.Value != offerLine.OfferPriceIncludeTax);

            return changed;
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = (type == DiscountOfferLine.DiscountOfferTypeEnum.All || cmbRelation.SelectedData.ID != RecordIdentifier.Empty) && Changed();
        }

        private bool DiscountPercentEnabled
        {
            set
            {
                value = value && optDiscountValue.Checked;

                ntbDiscountPercent.Enabled = value;
                ntbDiscountPercent.TabStop = value;
                ntbDiscountPercent.BackColor = value ? SystemColors.Window : Color.FromKnownColor(KnownColor.WhiteSmoke);
                lblDiscountPercent.ForeColor = value ? SystemColors.ControlText : SystemColors.GrayText;
            }
        }

        private bool DiscountAmountWithTaxEnabled
        {
            set
            {
                value = value && optDiscountValue.Checked;

                ntbDiscountAmountWithTax.Enabled = value;
                ntbDiscountAmountWithTax.TabStop = value;
                ntbDiscountAmountWithTax.BackColor = value ? SystemColors.Window : Color.FromKnownColor(KnownColor.WhiteSmoke);
                lblDiscountAmountWithTax.ForeColor = value ? SystemColors.ControlText : SystemColors.GrayText;
            }
        }

        private bool DiscountAmountEnabled
        {
            set
            {
                value = value && optDiscountValue.Checked;

                ntbDiscountAmount.Enabled = value;
                ntbDiscountAmount.TabStop = value;
                ntbDiscountAmount.BackColor = value ? SystemColors.Window : Color.FromKnownColor(KnownColor.WhiteSmoke);
                lblDiscountAmount.ForeColor = value ? SystemColors.ControlText : SystemColors.GrayText;
            }
        }

        private bool OfferPriceEnabled
        {
            set
            {
                value = value && optPriceValue.Checked;

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
                value = value && optPriceValue.Checked;

                ntbOfferPriceWithTax.Enabled = value;
                ntbOfferPriceWithTax.TabStop = value;
                ntbOfferPriceWithTax.BackColor = value ? SystemColors.Window : Color.FromKnownColor(KnownColor.WhiteSmoke);
                lblOfferPriceWithTax.ForeColor = value ? SystemColors.ControlText : SystemColors.GrayText;
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            standardPrice = 0.0M;
            standardPriceWithTax = 0.0M;
           
            cmbRelation.SelectedData = new MasterIDEntity() { ID = RecordIdentifier.Empty, ReadadbleID = RecordIdentifier.Empty, Text = "" };

            cmbVariantNumber.SelectedData = new MasterIDEntity() { ID = RecordIdentifier.Empty, ReadadbleID = RecordIdentifier.Empty, Text = "" };

            switch (cmbType.SelectedIndex)
            {
                case 0:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.Item;
                    lblRelation.Text = Properties.Resources.Item + ":";
                    lblRelation.ForeColor = SystemColors.ControlText;
                    cmbRelation.Enabled = true;

                    lblVariantNumber.ForeColor = SystemColors.GrayText;
                    cmbVariantNumber.Enabled = false;

                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);

                    break;

                case 1:

                    type = DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup;

                    lblRelation.Text = Properties.Resources.RetailGroup + ":";
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

                    lblRelation.Text = Properties.Resources.RetailDepartment + ":";
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

                    lblRelation.Text = Properties.Resources.Item + ":";
                    lblRelation.ForeColor = SystemColors.GrayText;
                    cmbRelation.Enabled = false;

                    ntbDiscountAmount.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    ntbOfferPrice.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    ntbOfferPriceWithTax.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                    lblVariantNumber.ForeColor = SystemColors.GrayText;
                    cmbVariantNumber.Enabled = false;

                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);

                    break;

                case 4:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup;

                    lblRelation.Text = Properties.Resources.SpecialGroup + ":";
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
            CheckEnabled();
        }

        private List<RecordIdentifier> GetIDsToExclude(DiscountOfferLine.DiscountOfferTypeEnum typeEnum)
        {
            List<PromotionOfferLine> lines = Providers.DiscountOfferLineData.GetPromotionLines(PluginEntry.DataModel, offer.ID, PromotionOfferLineSorting.OfferID, false);
            List<RecordIdentifier> excludedItemIds = (from l in lines
                                                      where l.Type == typeEnum
                                                      select l.TargetMasterID).ToList();
            return excludedItemIds;
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

                e.ControlToEmbed = new SingleSearchPanel(
                    PluginEntry.DataModel,
                    true,
                    initialSearchText,
                    SearchTypeEnum.RetailItemsMasterID,
                    new List<RecordIdentifier>(),
                    textInitallyHighlighted,
                    true);
            }
        }
        
        private void cmbVariantNumber_SelectedDataChanged(object sender, EventArgs e)
        {
            errorProvider3.Clear();
            errorProvider2.Clear();
            retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbVariantNumber.SelectedData.ID);

            if (retailItem == null)
            {
                throw new DataIntegrityException(typeof(RetailItem), cmbVariantNumber.SelectedData.ID);
            }

            standardPrice = retailItem.SalesPrice;
            standardPriceWithTax = standardPrice + Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTax(PluginEntry.DataModel, retailItem);

            optDiscountValue.Enabled = true;
            optPriceValue.Enabled = true;

            DiscountPercentEnabled = true;
            DiscountAmountEnabled = true;
            DiscountAmountWithTaxEnabled = true;
            OfferPriceEnabled = true;
            OfferPriceWithTaxEnabled = true;

            ntbStandardPrice.Value = (double)standardPrice;
            ntbStandardPriceWithTax.Value = (double)standardPriceWithTax;

            lastKnownDiscountPercent = 0.0M;
            ntbDiscountPercent_Leave(null, EventArgs.Empty);
            CheckEnabled();
        }

        private void cmbRelation_SelectedDataChanged(object sender, EventArgs e)
        {
            errorProvider3.Clear();
            errorProvider2.Clear();

            if (cmbRelation.SelectedData.ID != RecordIdentifier.Empty)
            {
                if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item)
                {
                    retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbRelation.SelectedData.ID);

                    if (retailItem == null)
                    {
                        throw new DataIntegrityException(typeof(RetailItem), cmbRelation.SelectedData.ID);
                    }

                    cmbVariantNumber.SelectedData = new MasterIDEntity() { ID = RecordIdentifier.Empty, ReadadbleID = RecordIdentifier.Empty, Text = "" }; 

                    cmbVariantNumber.Enabled = retailItem.ItemType == ItemTypeEnum.MasterItem || !RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID);
                    lblVariantNumber.ForeColor = cmbVariantNumber.Enabled
                        ? SystemColors.ControlText
                        : SystemColors.GrayText;

                    standardPrice = retailItem.SalesPrice;
                    standardPriceWithTax = standardPrice + Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTax(PluginEntry.DataModel, retailItem);

                    optDiscountValue.Enabled = true;
                    optPriceValue.Enabled = true;

                    DiscountPercentEnabled = true;
                    DiscountAmountEnabled = true;
                    DiscountAmountWithTaxEnabled = true;
                    OfferPriceEnabled = true;
                    OfferPriceWithTaxEnabled = true;
                }
                else
                {
                    optDiscountValue.Enabled = true;
                    optDiscountValue.Checked = true;
                    optPriceValue.Enabled = false;
                    DiscountPercentEnabled = true;
                    DiscountAmountEnabled = false;
                    DiscountAmountWithTaxEnabled = false;
                    OfferPriceEnabled = false;
                    OfferPriceWithTaxEnabled = false;
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

                    optDiscountValue.Enabled = false;
                    optPriceValue.Enabled = false;
                }
                else
                {
                    optDiscountValue.Enabled = true;
                    optDiscountValue.Checked = true;
                    optPriceValue.Enabled = false;

                    DiscountPercentEnabled = true;
                    DiscountAmountEnabled = false;
                    DiscountAmountWithTaxEnabled = false;
                    OfferPriceEnabled = false;
                    OfferPriceWithTaxEnabled = false;
                }
            }

            ntbStandardPrice.Value = (double)standardPrice;
            ntbStandardPriceWithTax.Value = (double)standardPriceWithTax;

            lastKnownDiscountPercent = 0.0M;
            ntbDiscountPercent_Leave(null, EventArgs.Empty);

            CheckEnabled();
        }

        private bool CheckDiscountPercent()
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item ||
                type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (standardPrice == 0.0M)
                {
                    errorProvider1.SetError(ntbStandardPrice, Properties.Resources.StandardPriceIsZero);
                    return false;
                }
            }
            return true;
        }

        private void ntbDiscountPercent_Leave(object sender, EventArgs e)
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item || type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (standardPrice != 0.0M)
                {
                    errorProvider1.Clear();
                    if (lastKnownDiscountPercent != ntbDiscountPercent.FullPrecisionValue)
                    {
                        lastKnownDiscountPercent = ntbDiscountPercent.FullPrecisionValue;

                        lastKnownDiscountAmount = standardPrice * (lastKnownDiscountPercent / 100.0M);
                        lastKnownDiscountAmountWithTax = standardPriceWithTax * (lastKnownDiscountPercent / 100.0M);

                        ntbDiscountAmount.SetValueWithLimit(lastKnownDiscountAmount, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        ntbDiscountAmountWithTax.SetValueWithLimit(lastKnownDiscountAmountWithTax, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                    }
                }
            }

            CheckEnabled();
        }

        private bool CheckItems()
        {
            RecordIdentifier targetMasterID;
            
            errorProvider2.Clear();
            if (cmbType.SelectedIndex == 0)
            {
                bool targetChanged = true;

                if (cmbVariantNumber.SelectedData != null && cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty)
                {
                    targetMasterID = cmbVariantNumber.SelectedData.ID;
                }
                else
                {
                    targetMasterID = cmbRelation.SelectedData.ID;
                }

                if (offerLine != null)
                {
                    if(offerLine.TargetMasterID == targetMasterID)
                    {
                        targetChanged = false;
                    }
                }

                bool relationexists = targetChanged && Providers.DiscountOfferLineData.RelationExists(PluginEntry.DataModel,
                    offer.MasterID, targetMasterID,
                    DiscountOfferLine.DiscountOfferTypeEnum.Item, offer.OfferType);

                if (relationexists)
                {
                    if (cmbVariantNumber.SelectedData != null && cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty)
                    {
                        errorProvider2.SetError(cmbVariantNumber, Properties.Resources.RelationExists);
                    }
                    else
                    {
                        errorProvider2.SetError(cmbRelation, Properties.Resources.RelationExists);
                    }
                    return false;
                }
            }
            return true;
        }

        private bool CheckDiscountAmount()
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item || type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (standardPrice != 0.0M)
                {
                    if (lastKnownDiscountAmount != ntbDiscountAmount.FullPrecisionValue)
                    {
                        if (standardPrice < ntbDiscountAmount.FullPrecisionValue)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private void ntbDiscountAmount_Leave(object sender, EventArgs e)
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item || type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (standardPrice != 0.0M)
                {
                    errorProvider1.Clear();
                    if (lastKnownDiscountAmount != ntbDiscountAmount.FullPrecisionValue)
                    {
                        lastKnownDiscountAmount = ntbDiscountAmount.FullPrecisionValue;
                        if (standardPrice >= ntbDiscountAmount.FullPrecisionValue)
                        {
                            errorProvider2.Clear();

                            decimal tempOfferPrice = Math.Round(standardPrice - lastKnownDiscountAmount, ntbOfferPrice.DecimalLetters);
                            decimal tempOfferPriceWithTax = Math.Round(Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTaxForAmount(PluginEntry.DataModel, retailItem.SalesTaxItemGroupID, tempOfferPrice) + tempOfferPrice, ntbOfferPriceWithTax.DecimalLetters);

                            lastKnownDiscountAmountWithTax = Math.Round(standardPriceWithTax - tempOfferPriceWithTax, ntbDiscountAmountWithTax.DecimalLetters);
                            lastKnownDiscountPercent = Math.Round((1 - (tempOfferPrice / standardPrice)) * 100.0M, ntbDiscountPercent.DecimalLetters);

                            ntbDiscountPercent.SetValueWithLimit(lastKnownDiscountPercent, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent));
                            ntbDiscountAmountWithTax.SetValueWithLimit(lastKnownDiscountAmountWithTax, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        }
                       
                    }
                }
            }

            CheckEnabled();
        }

        private bool CheckDiscountAmountWithTax()
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item || type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (standardPrice != 0.0M)
                {
                    if (lastKnownDiscountAmountWithTax != (decimal)ntbDiscountAmountWithTax.FullPrecisionValue)
                    {
                        if (standardPriceWithTax < ntbDiscountAmountWithTax.FullPrecisionValue)
                        {
                            errorProvider2.SetError(ntbStandardPrice, Properties.Resources.DiscountExceedsPrice);
                            return false;
                        }
                    }
                }
                else
                {
                    errorProvider1.SetError(ntbOfferPrice, Properties.Resources.StandardPriceIsZero);
                    return false;
                }
            }
            return true;
        }

        private void ntbDiscountAmountWithTax_Leave(object sender, EventArgs e)
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item || type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (standardPrice != 0.0M)
                {
                    if (lastKnownDiscountAmountWithTax != (decimal)ntbDiscountAmountWithTax.FullPrecisionValue)
                    {
                        lastKnownDiscountAmountWithTax = (decimal)ntbDiscountAmountWithTax.FullPrecisionValue;
                        if (standardPriceWithTax >= ntbDiscountAmountWithTax.FullPrecisionValue)
                        {
                            errorProvider2.Clear();

                            decimal tempOfferPriceWithTax = Math.Round(standardPriceWithTax - lastKnownDiscountAmountWithTax, ntbOfferPriceWithTax.DecimalLetters);
                            decimal tempOfferPrice = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemPriceForItemPriceWithTax(PluginEntry.DataModel, retailItem.SalesTaxItemGroupID, tempOfferPriceWithTax);

                            lastKnownDiscountAmount = Math.Round(standardPrice - tempOfferPrice, ntbDiscountAmount.DecimalLetters);

                            lastKnownDiscountPercent = Math.Round((1 - (tempOfferPrice / standardPrice)) * 100.0M, ntbDiscountPercent.DecimalLetters);

                            ntbDiscountPercent.SetValueWithLimit(lastKnownDiscountPercent, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent));
                            ntbDiscountAmount.SetValueWithLimit(lastKnownDiscountAmount, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        }
                    }
                }
            }

            CheckEnabled();
        }

        private void ntbOfferPrice_Leave(object sender, EventArgs e)
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item || type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (lastKnownOfferPrice != ntbOfferPrice.FullPrecisionValue)
                {
                    lastKnownOfferPrice = ntbOfferPrice.FullPrecisionValue;
                    lastKnownOfferPriceWithTax = Math.Round(Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTaxForAmount(PluginEntry.DataModel, retailItem.SalesTaxItemGroupID, lastKnownOfferPrice) + lastKnownOfferPrice, ntbOfferPriceWithTax.DecimalLetters);

                    ntbOfferPriceWithTax.SetValueWithLimit(lastKnownOfferPriceWithTax, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                }
            }

            CheckEnabled();
        }

        private void ntbOfferPriceWithTax_Leave(object sender, EventArgs e)
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item || type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
            {
                if (lastKnownOfferPriceWithTax != ntbOfferPriceWithTax.FullPrecisionValue)
                {
                    lastKnownOfferPriceWithTax = ntbOfferPriceWithTax.FullPrecisionValue;
                    lastKnownOfferPrice = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemPriceForItemPriceWithTax(PluginEntry.DataModel, retailItem.SalesTaxItemGroupID, lastKnownOfferPriceWithTax);

                    ntbOfferPrice.SetValueWithLimit(lastKnownOfferPrice, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                }
            }

            CheckEnabled();
        }

        private void cmbRelation_RequestData(object sender, EventArgs e)
        {
            List<RecordIdentifier> excludedItemIds = GetIDsToExclude(type);

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

        private void OptionButtonCheckedChanged(object sender, EventArgs e)
        {
            errorProvider2.Clear();

            DiscountAmountEnabled = optDiscountValue.Checked;
            DiscountAmountWithTaxEnabled = optDiscountValue.Checked;
            DiscountPercentEnabled = optDiscountValue.Checked;

            OfferPriceEnabled = !optDiscountValue.Checked;
            OfferPriceWithTaxEnabled = !optDiscountValue.Checked;

            optDiscountValue.TabStop = true;
            optPriceValue.TabStop = true;

            if (optDiscountValue.Checked)
            {
                ntbOfferPrice.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                ntbOfferPriceWithTax.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                lastKnownOfferPrice = 0.0M;
                lastKnownOfferPriceWithTax = 0.0M;
            }
            else
            {
                ntbDiscountPercent.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent));
                ntbDiscountAmount.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                ntbDiscountAmountWithTax.SetValueWithLimit(0.0M, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                lastKnownDiscountAmount = 0.0M;
                lastKnownDiscountAmountWithTax = 0.0M;
                lastKnownDiscountPercent = 0.0M;
            }

            CheckEnabled();
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

            return DiscountOfferLine.DiscountOfferTypeEnum.All;
        }

        private void cmbVariantNumber_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new MasterIDEntity() { ID = RecordIdentifier.Empty, ReadadbleID = RecordIdentifier.Empty, Text = "" };
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