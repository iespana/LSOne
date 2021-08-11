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

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class MixAndMatchLineDialog : DialogBase
    {
        DiscountOffer offer;
        DiscountOfferLine offerLine;
        DiscountOfferLine.DiscountOfferTypeEnum type;
        DiscountOffer.MixAndMatchDiscountTypeEnum offerType;

        RetailItem retailItem;

        decimal standardPrice;
        decimal standardPriceWithTax;

        bool lineGroupsChanged;

        public MixAndMatchLineDialog(DiscountOffer offer, DiscountOffer.MixAndMatchDiscountTypeEnum offerType, RecordIdentifier lineGuid)
            : this()
        {
            this.offer = offer;
            this.offerType = offerType;

            offerLine = Providers.DiscountOfferLineData.GetMixAndMatchLine(PluginEntry.DataModel, lineGuid, offerType);

            DataEntity givenItem = null;
            switch (offerLine.Type)
            {
                case DiscountOfferLine.DiscountOfferTypeEnum.Variant:
                case DiscountOfferLine.DiscountOfferTypeEnum.Item:
                    cmbType.SelectedIndex = 0;
                    var item = Providers.RetailItemData.Get(PluginEntry.DataModel, offerLine.TargetMasterID);
                    givenItem = new MasterIDEntity { ID = item.MasterID, ReadadbleID = item.ID, Text = item.Text, ExtendedText = item.VariantName };
                    break;

                
                case DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup:
                    cmbType.SelectedIndex = 1;
                    var group = Providers.RetailGroupData.Get(PluginEntry.DataModel, offerLine.TargetMasterID);
                    givenItem = new MasterIDEntity { ID = group.MasterID, ReadadbleID = group.ID, Text = group.Text };
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment:
                    cmbType.SelectedIndex = 2;
                    var dept = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, offerLine.TargetMasterID);
                    givenItem = new MasterIDEntity { ID = dept.MasterID, ReadadbleID = dept.ID, Text = dept.Text };
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup:
                    cmbType.SelectedIndex = 3;
                    var special = Providers.SpecialGroupData.GetSpecialGroup(PluginEntry.DataModel, offerLine.TargetMasterID);
                    givenItem = new MasterIDEntity { ID = special.MasterID, ReadadbleID = special.ID, Text = special.Text };
                    break;       
            }
            
            cmbRelation.SelectedData = givenItem;
            cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
            cmbLineGroup.SelectedData = new DataEntity(offerLine.LineGroup, (string)offerLine.MMGDescription);

            if (offerType == DiscountOffer.MixAndMatchDiscountTypeEnum.LineSpecific)
            {
                lblDiscountType.Enabled = true;
                cmbDiscountType.Enabled = true;
                lblDealPriceDiscountPct.Enabled = true;
                ntbDealPriceDiscountPct.Enabled = true;

                cmbDiscountType.SelectedIndex = (offerLine.DiscountType == DiscountOfferLine.MixAndMatchDiscountTypeEnum.DealPrice) ? 0 : 1;
                ntbDealPriceDiscountPct.Value = (double)offerLine.DiscountPercent;
            }

            btnOK.Enabled = false;
        }

        public MixAndMatchLineDialog(DiscountOffer offer,DiscountOffer.MixAndMatchDiscountTypeEnum offerType)
            : this()
        {
            cmbType.SelectedIndex = 0; // Select item by default
            this.offerType = offerType;            
        
            this.offer = offer;

            if (offerType == DiscountOffer.MixAndMatchDiscountTypeEnum.LineSpecific)
            {
                lblDiscountType.Enabled = true;
                cmbDiscountType.Enabled = true;
                lblDealPriceDiscountPct.Enabled = true;
                ntbDealPriceDiscountPct.Enabled = true;
            }
        }

        public MixAndMatchLineDialog()
            : base()
        {           
            DecimalLimit priceLimiter;


            offerLine = null;
            offer = null;

            InitializeComponent();

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);           

            cmbRelation.SelectedData = new DataEntity("", "");
            cmbLineGroup.SelectedData = new DataEntity("", "");

            ntbStandardPrice.DecimalLetters = priceLimiter.Max;
            ntbStandardPriceWithTax.DecimalLetters = priceLimiter.Max;

            lineGroupsChanged = false;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier OfferID
        {
            get { return offer.ID; }
        }

        public bool LineGroupsChanged
        {
            get { return lineGroupsChanged; }
        }

        private void CheckEnabled(object sender, EventArgs a)
        {
            btnOK.Enabled = Valid() && Changed();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Valid() && Changed())
            {
                if (offerLine == null)
                {
                    offerLine = new DiscountOfferLine();
                    offerLine.OfferMasterID = offer.MasterID;
                    offerLine.OfferID = offer.ID;
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
                        offerLine.ItemRelation = PluginOperations.GetReadableItemID(cmbRelation);
                        offerLine.Text = cmbRelation.SelectedData.Text;
                        break;

                    case 3:
                        offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup;
                        offerLine.TargetMasterID = cmbRelation.SelectedData.ID;
                        offerLine.ItemRelation = PluginOperations.GetReadableItemID(cmbRelation);
                        offerLine.Text = cmbRelation.SelectedData.Text;
                        break;
                }

                if (offerType == DiscountOffer.MixAndMatchDiscountTypeEnum.LineSpecific)
                {
                    offerLine.DiscountType = (cmbDiscountType.SelectedIndex == 0)
                        ? DiscountOfferLine.MixAndMatchDiscountTypeEnum.DealPrice
                        : DiscountOfferLine.MixAndMatchDiscountTypeEnum.DiscountPercent;

                    offerLine.DiscountPercent = (decimal) ntbDealPriceDiscountPct.Value;
                }

                offerLine.LineGroup = cmbLineGroup.SelectedData.ID;

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
            return cmbRelation.SelectedData.ID != "" && cmbLineGroup.SelectedData.ID != "";
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
            else
            {
                newType = DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup;
                changed = changed || (cmbRelation.SelectedData.ID != offerLine.TargetMasterID);
            }

            changed = changed || (newType != offerLine.Type);
            changed = changed || cmbLineGroup.SelectedData.ID != offerLine.LineGroup;

            if (cmbDiscountType.Enabled)
            {
                changed = changed ||
                          (offerLine.DiscountType == DiscountOfferLine.MixAndMatchDiscountTypeEnum.DealPrice ? 0 : 1) !=
                          cmbDiscountType.SelectedIndex;
                changed = changed || offerLine.DiscountPercent != (decimal) ntbDealPriceDiscountPct.Value;
            }

            return changed;
        }


        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            standardPrice = 0.0M;
            standardPriceWithTax = 0.0M;

            cmbRelation.SelectedData = new DataEntity("", "");

            cmbVariantNumber.SelectedData = new RetailItem();

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

                    lblVariantNumber.ForeColor = SystemColors.GrayText;
                    cmbVariantNumber.Enabled = false;

                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);
                    break;

                case 2:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment;

                    lblRelation.Text = Properties.Resources.RetailDepartment + ":";
                    lblRelation.ForeColor = SystemColors.ControlText;
                    cmbRelation.Enabled = true;

                    lblVariantNumber.ForeColor = SystemColors.GrayText;
                    cmbVariantNumber.Enabled = false;

                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);
                    break;

                case 3:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup;

                    lblRelation.Text = Properties.Resources.SpecialGroup + ":";
                    lblRelation.ForeColor = SystemColors.ControlText;
                    cmbRelation.Enabled = true;

                    lblVariantNumber.ForeColor = SystemColors.GrayText;
                    cmbVariantNumber.Enabled = false;

                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);
                    break;                
            }

            cmbRelation.ShowDropDownOnTyping = (type == DiscountOfferLine.DiscountOfferTypeEnum.Item || type == DiscountOfferLine.DiscountOfferTypeEnum.Variant);

            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbRelation_DropDown(object sender, DropDownEventArgs e)
        {
            if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item || type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
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
                    initialSearchText = ((DataEntity)cmbRelation.SelectedData).Text;
                    textInitallyHighlighted = true;
                }
                e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, true, initialSearchText, SearchTypeEnum.RetailItemsMasterID, excludedItemIDs, textInitallyHighlighted, true);
            }
        }
       
        private void cmbVariantNumber_SelectedDataChanged(object sender, EventArgs e)
        {          
            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbRelation_SelectedDataChanged(object sender, EventArgs e)
        {
            bool defaultStoreExists = true;
            bool defaultStoreHasTaxGroup = true;
            bool itemHasTaxGroup = true;
            if (cmbRelation.SelectedData.ID != "")
            {
                if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item)
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

                    // lblVariantNumber.
                    standardPrice = retailItem.SalesPrice;
                    standardPriceWithTax = standardPrice + Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTax(PluginEntry.DataModel, retailItem);
                }

                if (defaultStoreExists && defaultStoreHasTaxGroup && itemHasTaxGroup)
                {
                    errorProvider2.Clear();
                }
                else
                {
                    if (!defaultStoreExists)
                    {
                        errorProvider2.SetError(cmbRelation, Properties.Resources.NoDefaultStoreSelected);
                    }

                    if (!defaultStoreHasTaxGroup)
                    {
                        errorProvider2.SetError(cmbRelation, Properties.Resources.DefaultStoreHasNoTaxGroupAttachedToIt);
                    }

                    if (!itemHasTaxGroup)
                    {
                        errorProvider2.SetError(cmbRelation, Properties.Resources.ItemHasNoTaxGroupAttachedToIt);
                    }
                }
            }
            else
            {
                errorProvider2.Clear();
            }

            ntbStandardPrice.Value = (double)standardPrice;
            ntbStandardPriceWithTax.Value = (double)standardPriceWithTax;

            CheckEnabled(this, EventArgs.Empty);
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

        private void btnEditLineGroups_Click(object sender, EventArgs e)
        {
            Dialogs.MixAndMatchLineGroupsDialog dlg = new MixAndMatchLineGroupsDialog(offer.ID);

            dlg.ShowDialog();

            lineGroupsChanged = lineGroupsChanged | dlg.Changed;
        }

        private void cmbLineGroup_RequestData(object sender, EventArgs e)
        {
            cmbLineGroup.SkipIDColumn = true;

            cmbLineGroup.SetData(Providers.MixAndMatchLineGroupData.GetList(PluginEntry.DataModel, offer.ID), null);
        }

        private void cmbDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DecimalLimit limit;

            if (cmbDiscountType.SelectedIndex == 0)
            {
                lblDealPriceDiscountPct.Text = Properties.Resources.DealPrice;
                limit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            }
            else
            {
                lblDealPriceDiscountPct.Text = Properties.Resources.DiscountPercent;
                limit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);
            }

            ntbDealPriceDiscountPct.DecimalLetters = limit.Max;

            CheckEnabled(this, EventArgs.Empty);
        }

        private List<RecordIdentifier> GetIDsToExclude(DiscountOfferLine.DiscountOfferTypeEnum typeEnum)
        {
            List<DiscountOfferLine> lines = Providers.DiscountOfferLineData.GetMixAndMatchLines(PluginEntry.DataModel, offer.ID, DiscountOffer.MixAndMatchDiscountTypeEnum.DealPrice);
            List<RecordIdentifier> excludedItemIds = (from l in lines
                                                      where l.Type == typeEnum
                                                      select l.TargetMasterID).ToList();
            return excludedItemIds;
        }

        private void cmbVariantNumber_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity)cmbVariantNumber.SelectedData).Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                retailItem.ItemType == ItemTypeEnum.MasterItem ?
                retailItem.MasterID :
                retailItem.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, excludedItemIDs, textInitallyHighlighted, true);
        }
    }
}
