using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Infocodes.Dialogs
{
    public partial class NewSubcodeDialog : DialogBase
    {

        RecordIdentifier infocodeID;
        RecordIdentifier subCode;
        private RecordIdentifier unitID;
        List<Unit> unitList;
        private RetailItem retailItem;


        public NewSubcodeDialog(RecordIdentifier SelectedGroupID)
        {
            infocodeID = SelectedGroupID;            
            InitializeComponent();

            cmbTriggerCode.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity("", "");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier SubCode
        {
            get { return subCode; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            InfocodeSubcode subcode = new InfocodeSubcode();
            subcode.InfocodeId = infocodeID.PrimaryID;
            subcode.Text = tbDescription.Text;
            subcode.TriggerFunction = TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex);
            subcode.PriceHandling = subcode.TriggerFunction == TriggerFunctions.Item ? PriceHandlings.AlwaysCharge : PriceHandlings.None;
            subcode.PriceType = subcode.TriggerFunction == TriggerFunctions.Item ? PriceTypes.FromItem : PriceTypes.None;
            subcode.TriggerCode = retailItem != null && retailItem.ItemType == ItemTypeEnum.MasterItem ? ((MasterIDEntity)cmbVariant.SelectedData).ReadadbleID : cmbTriggerCode.SelectedData.ID;
            subcode.UnitOfMeasure = cmbUnit.SelectedData.ID;

            Providers.InfocodeSubcodeData.Save(PluginEntry.DataModel, subcode);

            subCode = subcode.ID;

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
            errorProvider1.Clear();
            btnOK.Enabled = (tbDescription.Text.Length > 0) && (cmbTriggerFunction.SelectedIndex >= 0);
            if (cmbTriggerFunction.SelectedIndex > 0 && cmbTriggerCode.SelectedDataID == "")
            {
                btnOK.Enabled = false;
            }

            if (retailItem != null && retailItem.ItemType == ItemTypeEnum.MasterItem && ((DataEntity)cmbVariant.SelectedData).Text == "")
            {
                btnOK.Enabled = false;
            }
        }

        private void cmbTriggerFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbTriggerCode.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");

            var triggerFunction = TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex);

            switch (triggerFunction)
            {
                case TriggerFunctions.None:
                    cmbTriggerCode.Enabled = false;
                    cmbUnit.Enabled = false;
                    break;
                case TriggerFunctions.Item:
                    cmbTriggerCode.Enabled = true;
                    cmbUnit.Enabled = true;
                    break;
                case TriggerFunctions.DiscountGroup:
                    break;
                case TriggerFunctions.Customer:
                    break;
                case TriggerFunctions.VAT:
                    break;
                case TriggerFunctions.Infocode:
                    cmbTriggerCode.Enabled = true;
                    cmbUnit.Enabled = false;
                    break;
                case TriggerFunctions.TaxGroup:
                    cmbTriggerCode.Enabled = true;
                    cmbUnit.Enabled = false;
                    break;
                default:
                    break;
            }

            CheckEnabled(this,EventArgs.Empty);
        }

        private void cmbTriggerCode_DropDown(object sender, DropDownEventArgs e)
        {
            if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.Item)
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
                    initialSearchText = ((DataEntity)cmbTriggerCode.SelectedData).ID;
                    textInitallyHighlighted = true;
                }

                e.ControlToEmbed = new SingleSearchPanel(
                PluginEntry.DataModel,
                false,
                initialSearchText,
                SearchTypeEnum.RetailItems,
                textInitallyHighlighted, hideItemVariants: true);
            }
        }

        private void cmbTriggerCode_SelectedDataChanged(object sender, EventArgs e)
        {
            if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.Item)
            {
                cmbVariant.Enabled = lblVariant.Enabled = false;

                if (cmbTriggerCode.SelectedData.ID != "")
                {
                    retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbTriggerCode.SelectedData.ID);
                    cmbVariant.SelectedData = new MasterIDEntity() { ID = RecordIdentifier.Empty, ReadadbleID = RecordIdentifier.Empty, Text = "" };
                    btnOK.Enabled = false;
                    cmbUnit.Enabled = true;
                    cmbVariant.Enabled = lblVariant.Enabled = retailItem.ItemType == ItemTypeEnum.MasterItem;
                    ReloadAndSetUnitList(cmbTriggerCode.SelectedData.ID);
                }
            }
            CheckEnabled(this, EventArgs.Empty);
        }

        private void ReloadAndSetUnitList(RecordIdentifier itemID)
        {
            unitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemID, RetailItem.UnitTypeEnum.Purchase);

            if ((string) unitID != null)
            {
                unitList = Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, itemID, unitID);

                Unit unit = unitList.Count > 0 ? unitList.Single(p => p.ID == retailItem.PurchaseUnitID) : new Unit();

                if (unitList.Count > 0)
                {
                    cmbUnit.SetData(unitList, null);
                }

                if (cmbUnit.SelectedData != unit)
                {
                    cmbUnit.SelectedData = unit;
                }
            }
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
                    {Providers.UnitData.GetUnitForItem(PluginEntry.DataModel,cmbTriggerCode.SelectedData.ID,0,false,UnitTypeEnum.InventoryUnit).Cast<DataEntity>(),
                     Providers.UnitData.GetAllUnits(PluginEntry.DataModel).Cast<DataEntity>()};

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                data,
                new string[] { Properties.Resources.Convertible, Properties.Resources.All },
                250);

            cmbUnit.SetData(data, pnl);
        }

        private TriggerFunctions TriggerFunctionListboxToEnum(int listboxEnum)
        {
            switch (listboxEnum)
            {
                case 0: return TriggerFunctions.None;
                case 1: return TriggerFunctions.Item;
                case 2: return TriggerFunctions.Infocode;
                case 3: return TriggerFunctions.TaxGroup;
                default: return TriggerFunctions.None;
            }
        }

        private void cmbTriggerCode_RequestData(object sender, EventArgs e)
        {
            var triggerFunction = TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex);

            switch (triggerFunction)
            {
                case TriggerFunctions.None:
                    break;
                case TriggerFunctions.Item:
                    break;
                case TriggerFunctions.DiscountGroup:
                    break;
                case TriggerFunctions.Customer:
                    break;
                case TriggerFunctions.VAT:
                    break;
                case TriggerFunctions.Infocode:
                    cmbTriggerCode.SetData(Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new UsageCategoriesEnum[] { UsageCategoriesEnum.None }, new InputTypesEnum[] { },RefTableEnum.All), null);
                    break;
                case TriggerFunctions.TaxGroup:
                    cmbTriggerCode.SetData(Providers.SalesTaxGroupData.GetList(PluginEntry.DataModel), null);
                    break;
                default:
                    break;
            }

            if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.Infocode)
            {
                cmbTriggerCode.SetData(Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new UsageCategoriesEnum[] { UsageCategoriesEnum.None }, new InputTypesEnum[] { },RefTableEnum.All), null);
            }
        }

        private void cmbVariant_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity)cmbVariant.SelectedData).Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                retailItem.ItemType == ItemTypeEnum.MasterItem ?
                retailItem.MasterID :
                retailItem.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, new List<RecordIdentifier>(), textInitallyHighlighted, true);
        }
    

        private void cmbVariant_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbVariant_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new MasterIDEntity() { ID = RecordIdentifier.Empty, ReadadbleID = RecordIdentifier.Empty, Text = "" };
        }
    }
}
