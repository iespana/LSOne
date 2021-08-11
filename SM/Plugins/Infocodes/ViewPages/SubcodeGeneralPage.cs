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
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Infocodes.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    public partial class SubcodeGeneralPage : UserControl, ITabView
    {
        private InfocodeSubcode infocodeSubcode;
        private RetailItem item;
        private RetailItem headerItem;
        private RecordIdentifier unitID;
        List<Unit> unitList;
        private RecordIdentifier masterID;

        public SubcodeGeneralPage()
        {
            InitializeComponent();
            cmbTriggerCode.Enabled = false;
            cmbPriceHandling.Enabled = false;
            cmbPriceType.Enabled = false;
            ntbAmountPecent.Enabled = false;
            ntbQtyPerUnitOfMeasure.Enabled = false;
            ntbMaxSelection.Enabled = false;
            cmbUnit.Enabled = false;
            chkQtyLinkedToTriggerLine.Enabled = false;

            cmbTriggerCode.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity("", "");

            cmbPriceHandling.SelectedIndex = (int)PriceHandlings.None;
            headerItem = new RetailItem();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.SubcodeGeneralPage();
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            if (tbDescription.Text != infocodeSubcode.Text) return true;
            if (cmbTriggerFunction.SelectedIndex != TriggerFunctionEnumToListbox( infocodeSubcode.TriggerFunction)) return true;
            if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.Item)
            {
                if (item.HeaderItemID != RecordIdentifier.Empty || item.ItemType == ItemTypeEnum.MasterItem || cmbTriggerCode.SelectedData.ID != infocodeSubcode.TriggerCode)
                {
                    if (cmbTriggerCode.SelectedData.ID != headerItem.ID) return true;
                    if (cmbVariant.SelectedData.Text != infocodeSubcode.VariantDescription) return true;
                }
            }
            else
            {
                if (cmbTriggerCode.SelectedData.ID != infocodeSubcode.TriggerCode) return true;
            }
            if (cmbUnit.SelectedData.ID != infocodeSubcode.UnitOfMeasure) return true;
            if (cmbPriceHandling.SelectedIndex != (int)infocodeSubcode.PriceHandling) return true;
            if (cmbPriceType.SelectedIndex != (int)infocodeSubcode.PriceType) return true;
            if (ntbAmountPecent.Value != (double)infocodeSubcode.AmountPercent) return true;
            if (ntbQtyPerUnitOfMeasure.Value != (double)infocodeSubcode.QtyPerUnitOfMeasure) return true;
            if (ntbMaxSelection.Value != (double)infocodeSubcode.MaxSelection) return true;
            if (chkQtyLinkedToTriggerLine.Checked != infocodeSubcode.QtyLinkedToTriggerLine) return true;
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

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

        private int TriggerFunctionEnumToListbox(TriggerFunctions triggerFunc)
        {
            switch (triggerFunc)
            {
                case TriggerFunctions.None: return 0;
                case TriggerFunctions.Item: return 1;
                case TriggerFunctions.Infocode: return 2;
                case TriggerFunctions.TaxGroup: return 3;
                default: return 0;
            }
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            infocodeSubcode = (InfocodeSubcode)internalContext;

            tbID.Text = infocodeSubcode.SubcodeId.ToString();
            tbDescription.Text = infocodeSubcode.Text;
            cmbTriggerFunction.SelectedIndex = TriggerFunctionEnumToListbox(infocodeSubcode.TriggerFunction);
            cmbTriggerCode.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");

            if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.Item)
            {
                item = Providers.RetailItemData.Get(PluginEntry.DataModel, infocodeSubcode.TriggerCode);
                masterID = Providers.RetailItemData.GetMasterIDFromItemID(PluginEntry.DataModel,
                    item.ID);
                if (item != null)
                {
                    cmbTriggerCode.SelectedData = new DataEntity(item.ID, item.Text);

                    if (item.HeaderItemID != RecordIdentifier.Empty)
                    {
                        headerItem = Providers.RetailItemData.Get(PluginEntry.DataModel, item.HeaderItemID);
                        
                        cmbTriggerCode.SelectedData = new DataEntity(headerItem.ID, item.Text);
                        cmbVariant.SelectedData = new DataEntity(item.ID, item.VariantName);

                        lblVariant.Enabled = cmbVariant.Enabled = true;
                    }
                    ReloadAndSetUnitList(item.ID);
                }
            }
            else if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.Infocode)
            {
                Infocode infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, infocodeSubcode.TriggerCode);
                if (infocode != null)
                    cmbTriggerCode.SelectedData = new DataEntity(infocode.ID, infocode.Text);
                cmbUnit.SelectedData = new DataEntity("","");
            }
            else if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.TaxGroup)
            {
                SalesTaxGroup taxGroup = Providers.SalesTaxGroupData.Get(PluginEntry.DataModel, infocodeSubcode.TriggerCode);
                if (taxGroup != null)
                {
                    cmbTriggerCode.SelectedData = new DataEntity(taxGroup.ID, taxGroup.Text);
                }
            }

            cmbPriceHandling.SelectedIndex = (int)infocodeSubcode.PriceHandling;
            cmbPriceType.SelectedIndex = (int)infocodeSubcode.PriceType;
            ntbAmountPecent.Value = (double)infocodeSubcode.AmountPercent;
            ntbQtyPerUnitOfMeasure.Value = (double)infocodeSubcode.QtyPerUnitOfMeasure;
            ntbMaxSelection.Value = (double)infocodeSubcode.MaxSelection;
            chkQtyLinkedToTriggerLine.Checked = infocodeSubcode.QtyLinkedToTriggerLine;
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
           
        }

        public bool SaveData()
        {
            if (item != null && item.ItemType == ItemTypeEnum.MasterItem && cmbVariant.SelectedData.ID == RecordIdentifier.Empty)
            {
                errorProvider1.SetError(cmbVariant, Resources.VariantHasToBeSelected);
                return false;
            }

            if (cmbPriceHandling.SelectedIndex == (int) PriceHandlings.AlwaysCharge && cmbPriceType.SelectedIndex == (int) PriceHandlings.None)
            {
                errorProvider1.SetError(cmbPriceType, Resources.PriceTypeHasToBeSelected);
                return false;
            }

            if(TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) != TriggerFunctions.None && RecordIdentifier.IsEmptyOrNull(cmbTriggerCode.SelectedDataID))
            {
                errorProvider1.SetError(cmbTriggerCode, Resources.TriggerCodeHasToBeSelected);
                return false;
            }

            infocodeSubcode.Text = tbDescription.Text;
            infocodeSubcode.Text = tbDescription.Text;
            infocodeSubcode.TriggerFunction = TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex);
            if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.Item)
            {
                infocodeSubcode.TriggerCode = item.HeaderItemID != RecordIdentifier.Empty || item.ItemType == ItemTypeEnum.MasterItem
                    ? ((bool) (RecordIdentifier)cmbVariant.SelectedData.ID.IsGuid ? ((MasterIDEntity) cmbVariant.SelectedData).ReadadbleID : cmbVariant.SelectedData.ID)
                    : cmbTriggerCode.SelectedData.ID;
            }
            else
            {
                infocodeSubcode.TriggerCode = cmbTriggerCode.SelectedData.ID;
            }
            infocodeSubcode.UnitOfMeasure = cmbUnit.SelectedData.ID;
            infocodeSubcode.PriceHandling = (PriceHandlings)cmbPriceHandling.SelectedIndex;
            infocodeSubcode.PriceType = (PriceTypes)cmbPriceType.SelectedIndex;
            infocodeSubcode.AmountPercent = (decimal)ntbAmountPecent.Value;
            infocodeSubcode.QtyPerUnitOfMeasure = (decimal)ntbQtyPerUnitOfMeasure.Value;
            infocodeSubcode.MaxSelection = (int)ntbMaxSelection.Value;
            infocodeSubcode.QtyLinkedToTriggerLine = chkQtyLinkedToTriggerLine.Checked;
            
            return true;
        }

        public void OnClose()
        {
            
        }

        public void SaveUserInterface()
        {
        }

        #endregion

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
                textInitallyHighlighted, hideItemVariants:true);
            }
        }

        private void cmbTriggerCode_SelectedDataChanged(object sender, EventArgs e)
        {
            if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.Item)
            {
                if (cmbTriggerCode.SelectedData.ID != RecordIdentifier.Empty )
                {
                    item = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbTriggerCode.SelectedData.ID);

                    if (item.ID != headerItem.ID || (item.ItemType == ItemTypeEnum.MasterItem && cmbVariant.SelectedData.ID == RecordIdentifier.Empty))
                    {
                        cmbVariant.SelectedData = new MasterIDEntity() {ID = RecordIdentifier.Empty, ReadadbleID = RecordIdentifier.Empty, Text = ""};
                        cmbVariant.Enabled = lblVariant.Enabled = item.ItemType == ItemTypeEnum.MasterItem;
                        ReloadAndSetUnitList(cmbTriggerCode.SelectedData.ID);
                    }
                    cmbUnit.Enabled = lblUOM.Enabled = true;
                }
            }
            errorProvider1.Clear();
        }

        private void ReloadAndSetUnitList(RecordIdentifier itemID)
        {
            unitID = infocodeSubcode.UnitOfMeasure;//Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemID, RetailItem.UnitTypeEnum.Purchase);

            if ((string)unitID != null)
            {
                unitList = Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, itemID, unitID);

                Unit unit = unitList.Count > 0 ? unitList.Single(p => p.ID == unitID) : new Unit();

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

        private void cmbTriggerFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (infocodeSubcode.TriggerFunction != TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex))
            {
                cmbTriggerCode.SelectedData = new DataEntity("", "");
                cmbVariant.SelectedData = new MasterIDEntity() { ID = RecordIdentifier.Empty, ReadadbleID = RecordIdentifier.Empty, Text = "" };
                cmbTriggerCode.Enabled = lblTriggerCode.Enabled = false;
                cmbVariant.Enabled = lblVariant.Enabled = false;
            }

            if(TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.None)
            {
                cmbPriceHandling.SelectedIndex = (int)PriceHandlings.None;
            }

            cmbUnit.Enabled = lblUOM.Enabled = false;
            cmbPriceHandling.Enabled = false;
            cmbPriceType.Enabled = cmbPriceHandling.SelectedIndex == (int)PriceHandlings.AlwaysCharge;
            ntbAmountPecent.Enabled = false;
            ntbQtyPerUnitOfMeasure.Enabled = false;
            ntbMaxSelection.Enabled = false;
            chkQtyLinkedToTriggerLine.Enabled = false;

            if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.Item)
            {
                cmbTriggerCode.Enabled = lblTriggerCode.Enabled = true;
                cmbPriceHandling.Enabled = true;
                cmbPriceType.Enabled = cmbPriceHandling.SelectedIndex == (int)PriceHandlings.AlwaysCharge;

                cmbUnit.Enabled = lblUOM.Enabled = true;

                ntbQtyPerUnitOfMeasure.Enabled = true;
                ntbMaxSelection.Enabled = true;
                chkQtyLinkedToTriggerLine.Enabled = true;
            }
            else if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) != TriggerFunctions.None)
            {
                cmbUnit.SelectedData = new DataEntity("", "");
                cmbPriceHandling.SelectedIndex = (int)PriceHandlings.None;
                cmbPriceType.SelectedIndex = 0;
                cmbPriceType.Enabled = false;
                ntbAmountPecent.Text = "0";
                ntbQtyPerUnitOfMeasure.Text = "0";
                ntbMaxSelection.Text = "0";
                chkQtyLinkedToTriggerLine.Checked = false;
                cmbTriggerCode.Enabled = lblTriggerCode.Enabled = true;
            }
        }

        private void cmbPriceHandling_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbPriceType.Enabled = false;
            cmbPriceType.SelectedIndex = (int)PriceHandlings.None;
            if (cmbPriceHandling.SelectedIndex == (int)PriceHandlings.AlwaysCharge)
            {
                cmbPriceType.Enabled = true;
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
        
        private void cmbPriceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ntbAmountPecent.Value = 0;
            ntbAmountPecent.AllowDecimal = false;
            ntbAmountPecent.Enabled = ((PriceTypes)cmbPriceType.SelectedIndex == PriceTypes.Percent) || ((PriceTypes)cmbPriceType.SelectedIndex == PriceTypes.Price);
            if ((PriceTypes)cmbPriceType.SelectedIndex == PriceTypes.Price)
                ntbAmountPecent.AllowDecimal = true; ;
        }

        private void cmbTriggerCode_RequestData(object sender, EventArgs e)
        {
            if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.Infocode)
            {
                cmbTriggerCode.SetData(Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new UsageCategoriesEnum[]{UsageCategoriesEnum.None },new InputTypesEnum[] {}, RefTableEnum.All), null);
            }
            else if (TriggerFunctionListboxToEnum(cmbTriggerFunction.SelectedIndex) == TriggerFunctions.TaxGroup)
            {
                cmbTriggerCode.SetData(Providers.SalesTaxGroupData.GetList(PluginEntry.DataModel), null);
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
                item.ItemType == ItemTypeEnum.MasterItem ?
                item.MasterID :
                item.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, new List<RecordIdentifier>(), textInitallyHighlighted, true);
        }

        private void cmbVariant_SelectedDataChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
    }
}
