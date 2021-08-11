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
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class MultibuyLineDialog : DialogBase
    {
        DiscountOffer offer;
        DiscountOfferLine offerLine;
        DiscountOfferLine.DiscountOfferTypeEnum type;
        RetailItem retailItem;
        RecordIdentifier originalUnitID;
        private RecordIdentifier itemSalesUnitID;

        public MultibuyLineDialog(RecordIdentifier offerID, RecordIdentifier lineGuid)
            : this()
        {
            offer = Providers.DiscountOfferData.Get(PluginEntry.DataModel, offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion);
            offerLine = Providers.DiscountOfferLineData.Get(PluginEntry.DataModel, lineGuid);

            switch (offerLine.Type)
            {
                case DiscountOfferLine.DiscountOfferTypeEnum.Variant:
                case DiscountOfferLine.DiscountOfferTypeEnum.Item:
                    cmbType.SelectedIndex = 0;
                    cmbRelation.SelectedData = new MasterIDEntity
                    {
                        ID = offerLine.TargetMasterID,
                        ReadadbleID = offerLine.ItemRelation,
                        Text = offerLine.Text
                    };
                    cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
                    originalUnitID = offerLine.Unit;
                    cmbUnit.SelectedData = new DataEntity(offerLine.Unit, Providers.UnitData.GetUnitDescription(PluginEntry.DataModel, offerLine.Unit));
                    itemSalesUnitID = Providers.RetailItemData.Get(PluginEntry.DataModel, offerLine.TargetMasterID)?.SalesUnitID;
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup:
                    cmbType.SelectedIndex = 1;
                    cmbRelation.SelectedData = new DataEntity(offerLine.TargetMasterID, offerLine.Text);
                    cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment:
                    cmbType.SelectedIndex = 2;
                    cmbRelation.SelectedData = new DataEntity(offerLine.TargetMasterID, offerLine.Text);
                    cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.All:
                    cmbType.SelectedIndex = 3;
                    break;

                case DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup:
                    cmbType.SelectedIndex = 4;
                    cmbRelation.SelectedData = new DataEntity(offerLine.TargetMasterID, offerLine.Text);
                    cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
                    break;
            }
        }

        public MultibuyLineDialog(RecordIdentifier offerID)
            : this()
        {
            offer = Providers.DiscountOfferData.Get(PluginEntry.DataModel, offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion);

            cmbType.SelectedIndex = 0; // Select item by default
        }

        public MultibuyLineDialog()
            : base()
        {
            itemSalesUnitID = null;
            offerLine = null;
            offer = null;

            InitializeComponent();

            cmbRelation.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier OfferID
        {
            get { return offer.ID; }
        }

        private bool checkItems()
        {
            RecordIdentifier target;
            errorProvider1.Clear();
            errorProvider2.Clear();
            if (cmbType.SelectedIndex == 0)
            {
                target = null;

                if (retailItem != null)
                {
                    if (retailItem.ItemType == ItemTypeEnum.MasterItem)
                    {
                        target = cmbVariantNumber.SelectedData.ID;
                    }
                    else
                    {
                        target = cmbRelation.SelectedData.ID;
                    }
                }

                if (RecordIdentifier.IsEmptyOrNull(target))
                {
                    errorProvider2.SetError(retailItem.ItemType == ItemTypeEnum.MasterItem ? cmbVariantNumber : cmbRelation, Properties.Resources.SelectItem);
                    return false;
                }

                if (RecordIdentifier.IsEmptyOrNull(cmbUnit.SelectedDataID))
                {
                    errorProvider1.SetError(cmbUnit, Properties.Resources.SelectUnit);
                    return false;
                }

                if ((offerLine == null || offerLine.TargetMasterID != target) &&
                    Providers.DiscountOfferLineData.RelationExists(PluginEntry.DataModel, offer.MasterID, target,
                        DiscountOfferLine.DiscountOfferTypeEnum.Item, offer.OfferType))
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


        private void btnOK_Click(object sender, EventArgs e)
        {
            bool checksPassed =  checkItems();
            if (!Valid() || !checksPassed)
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
                    offerLine = new DiscountOfferLine();
                    offerLine.OfferID = offer.ID;
                }

                switch (cmbType.SelectedIndex)
                {
                    case 0:
                        offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.Item;
                        offerLine.Text = cmbRelation.SelectedData.Text;
                        offerLine.Unit = cmbUnit.SelectedDataID;

                        if (cmbVariantNumber.Enabled && !RecordIdentifier.IsEmptyOrNull(cmbVariantNumber.SelectedDataID))
                        {
                            string relation = (string)PluginOperations.GetReadableItemID(cmbVariantNumber);
                            offerLine.TargetMasterID = cmbVariantNumber.SelectedData.ID;
                            offerLine.ItemRelation = relation;
                        }
                        else
                        {
                            string relation = (string)PluginOperations.GetReadableItemID(cmbRelation);
                            offerLine.TargetMasterID = cmbRelation.SelectedData.ID;
                            offerLine.ItemRelation = relation;
                        }
                        break;
                    case 1:
                        offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup;
                        offerLine.TargetMasterID = cmbRelation.SelectedData.ID;
                        offerLine.ItemRelation = PluginOperations.GetReadableItemID(cmbRelation);
                        offerLine.Text = cmbRelation.SelectedData.Text;
                        offerLine.Unit = cmbUnit.SelectedDataID;
                        break;
                    case 2:
                        offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment;
                        offerLine.TargetMasterID = cmbRelation.SelectedData.ID;
                        offerLine.ItemRelation = PluginOperations.GetReadableItemID(cmbRelation);
                        offerLine.Text = cmbRelation.SelectedData.Text;
                        offerLine.Unit = cmbUnit.SelectedDataID;                        
                        break;
                    case 3:
                        offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.All;
                        offerLine.TargetMasterID = Guid.Empty;
                        offerLine.ItemRelation = "";
                        offerLine.Text = "";
                        offerLine.Unit = cmbUnit.SelectedDataID;
                        break;
                    case 4:
                        offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup;
                        offerLine.TargetMasterID = cmbRelation.SelectedData.ID;
                        offerLine.ItemRelation = PluginOperations.GetReadableItemID(cmbRelation);
                        offerLine.Text = cmbRelation.SelectedData.Text;
                        offerLine.Unit = cmbUnit.SelectedDataID;
                        break;
                }

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
                changed = changed || (target != offerLine.TargetMasterID) || originalUnitID != cmbUnit.SelectedDataID;
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

            return changed;
        }
        
        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorProvider2.Clear();
            errorProvider3.Clear();
           
            cmbRelation.SelectedData = new DataEntity("", "");
            cmbVariantNumber.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");

            cmbUnit.Enabled = lblUnit.Enabled = false;
            cmbVariantNumber.Enabled = lblVariantNumber.Enabled = false;

            switch (cmbType.SelectedIndex)
            {
                case 0:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.Item;
                    lblRelation.Text = Properties.Resources.Item + ":";
                    cmbRelation.Enabled = lblRelation.Enabled = true;
                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);
                    break;
                case 1:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup;
                    lblRelation.Text = Properties.Resources.RetailGroup + ":";
                    cmbRelation.Enabled = lblRelation.Enabled = true;
                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);
                    break;
                case 2:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment;
                    lblRelation.Text = Properties.Resources.RetailDepartment + ":";
                    cmbRelation.Enabled = lblRelation.Enabled = true;
                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);
                    break;
                case 3:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.All;
                    lblRelation.Text = Properties.Resources.Item + ":";
                    cmbRelation.Enabled = lblRelation.Enabled = false;
                    cmbRelation_SelectedDataChanged(this, EventArgs.Empty);
                    break;
                case 4:
                    type = DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup;
                    lblRelation.Text = Properties.Resources.SpecialGroup + ":";
                    cmbRelation.Enabled = lblRelation.Enabled = true;
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
                e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, true, initialSearchText, SearchTypeEnum.RetailItemsMasterID, new List<RecordIdentifier>(), textInitallyHighlighted, true);
            }
        }

        private void cmbRelation_SelectedDataChanged(object sender, EventArgs e)
        {
            itemSalesUnitID = null;
            cmbUnit.SelectedData = new DataEntity("", "");
            cmbVariantNumber.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            cmbVariantNumber.Enabled = lblVariantNumber.Enabled = false;

            if (cmbRelation.SelectedData.ID != "")
            {
                if (type == DiscountOfferLine.DiscountOfferTypeEnum.Item)
                {
                    retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbRelation.SelectedData.ID);
                    if (retailItem == null)
                    {
                        throw new DataIntegrityException(typeof(RetailItem), cmbRelation.SelectedData.ID);
                    }

                    itemSalesUnitID = retailItem.SalesUnitID;
                    cmbUnit.SelectedData = Providers.UnitData.Get(PluginEntry.DataModel, itemSalesUnitID);
                    cmbUnit.Enabled = lblUnit.Enabled = true;

                    if (!RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
                    {
                        cmbVariantNumber.SelectedData = new MasterIDEntity
                        {
                            ID = retailItem.MasterID,
                            ReadadbleID = retailItem.ID,
                            Text = retailItem.VariantName
                        };
                    }

                    cmbVariantNumber.Enabled = lblVariantNumber.Enabled = retailItem.ItemType == ItemTypeEnum.MasterItem || !RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID);
                }
            }
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
        
        private List<RecordIdentifier> GetIDsToExclude(DiscountOfferLine.DiscountOfferTypeEnum typeEnum)
        {
            int lineCount;
            List<DiscountOfferLine> lines = Providers.DiscountOfferLineData.GetLines(PluginEntry.DataModel, offer.ID, DiscountLineSortEnum.ProductType,  Int32.MaxValue, out lineCount);
            List<RecordIdentifier> excludedItemIds = (from l in lines
                                                      where l.Type == typeEnum
                                                      select l.TargetMasterID).ToList();
            return excludedItemIds;
        }

        private void cmbVariantNumber_DropDown(object sender, DropDownEventArgs e)
        {
            List<RecordIdentifier> excludedItemIDs = GetIDsToExclude(DiscountOfferLine.DiscountOfferTypeEnum.Item);

            if (offerLine != null)
            {
                excludedItemIDs.Remove(offerLine.TargetMasterID);
            }

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

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            if (itemSalesUnitID != null)
            {
                var itemID = ((MasterIDEntity)cmbRelation.SelectedData).ReadadbleID;
                var unitList = Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, itemID, itemSalesUnitID);
                cmbUnit.SetData(unitList, null);
            }
        }
    }
}
