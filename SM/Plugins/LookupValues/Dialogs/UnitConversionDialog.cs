using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.LookupValues.Properties;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    public partial class UnitConversionDialog : DialogBase
    {
        UnitConversion unitConversion;
        bool canSaveToDb;

        public UnitConversionDialog(RecordIdentifier selectedID)
            : this()
        {
            unitConversion = Providers.UnitConversionData.Get(PluginEntry.DataModel, selectedID);

            cmbConversionRuleFor.SelectedIndex = (unitConversion.ItemID == "") ? 0 : 1; // ItemId == "" means we are dealing with all items
            cmbItem.SelectedData = new DataEntity(unitConversion.ItemID, unitConversion.ItemName);
            cmbFromUnit.SelectedData = new DataEntity(unitConversion.FromUnitID, unitConversion.FromUnitName);
            cmbToUnit.SelectedData = new DataEntity(unitConversion.ToUnitID, unitConversion.ToUnitName);
            ntbFactor.Value = Convert.ToDouble(unitConversion.Factor);

            cmbItem.Enabled = false;
            cmbConversionRuleFor.Enabled = false;
            cmbFromUnit.Enabled = false;
            cmbToUnit.Enabled = false;

            // We are editing, we don't want to allow swapping units
            btnSwapUnits.Visible = false;
        }

        public UnitConversionDialog(string retailItemDescription, RecordIdentifier fromUnit, RecordIdentifier toUnit)
            : this()
        {
            Unit unit;

            canSaveToDb = false;

            cmbConversionRuleFor.SelectedIndex = 1;
            cmbConversionRuleFor.Enabled = false;

            cmbItem.SelectedData = new DataEntity(RecordIdentifier.Empty, retailItemDescription);
            cmbItem.Enabled = false;

            if (fromUnit != "")
            {
                unit = Providers.UnitData.Get(PluginEntry.DataModel, fromUnit);

                if (unit != null)
                {
                    cmbFromUnit.SelectedData = new DataEntity(fromUnit, unit.Text);
                }
            }

            cmbFromUnit.Enabled = false;

            if (toUnit != "")
            {
                unit = Providers.UnitData.Get(PluginEntry.DataModel, toUnit);

                if (unit != null)
                {
                    cmbToUnit.SelectedData = new DataEntity(toUnit, unit.Text);
                }
            }

            cmbToUnit.Enabled = false;
        }

        public UnitConversionDialog(DataEntity retailItem,RecordIdentifier fromUnit,RecordIdentifier toUnit)
            : this()
        {
            Unit unit;

            if (retailItem != null)
            {
                cmbConversionRuleFor.SelectedIndex = 1;
                cmbItem.SelectedData = retailItem;
            }

            if (fromUnit != "")
            {
                unit = Providers.UnitData.Get(PluginEntry.DataModel,fromUnit);

                if(unit != null)
                {
                    cmbFromUnit.SelectedData = new DataEntity(fromUnit,unit.Text);
                }
            }

            if (toUnit != "")
            {
                unit = Providers.UnitData.Get(PluginEntry.DataModel,toUnit);

                if(unit != null)
                {
                    cmbToUnit.SelectedData = new DataEntity(toUnit, unit.Text);
                }
            }
        }

        protected UnitConversionDialog()
            : base()
        {
            canSaveToDb = true;

            InitializeComponent();

            cmbConversionRuleFor.SelectedIndex = 0;
        }

        public bool SaveToSiteService { get; set; }

        public RecordIdentifier UnitID
        {
            get { return unitConversion != null ? unitConversion.ID : RecordIdentifier.Empty; }
        }

        public DataEntity Item
        {
            get { return new DataEntity(unitConversion.ItemID, unitConversion.ItemName); }
        }

        public UnitConversion UnitConversion
        {
            get
            {
                return unitConversion;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void btnOK_Click(object sender, EventArgs args)
        {
            if (cmbToUnit.SelectedData.ID == cmbFromUnit.SelectedData.ID)
            {
                errorProvider1.SetError(cmbFromUnit, Resources.SameUnit);

                return;
            }

            if(unitConversion == null)
            {
                unitConversion = new UnitConversion();
                unitConversion.ItemID = (cmbItem.SelectedData == null) ? "" : cmbItem.SelectedData.ID;
                unitConversion.ItemName = (cmbItem.SelectedData == null) ? "" : cmbItem.SelectedData.Text;
                unitConversion.FromUnitID = cmbFromUnit.SelectedData.ID;
                unitConversion.ToUnitID = cmbToUnit.SelectedData.ID;

                if (canSaveToDb)
                {
                    if (Providers.UnitConversionData.Exists(PluginEntry.DataModel, unitConversion.ID))
                    {
                        errorProvider1.SetError(cmbConversionRuleFor, Properties.Resources.UnitConversionExists);
                        unitConversion = null;
                        return;
                    }
                }
            }

            unitConversion.Factor = Convert.ToDecimal(ntbFactor.Value);

            if (canSaveToDb)
            {
                Providers.UnitConversionData.Save(PluginEntry.DataModel, unitConversion);
                if (SaveToSiteService)
                {
                    try
                    { 
                        ((ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService))
                        .SaveUnitConversionRule(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                            unitConversion, true);
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                    }
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        public RecordIdentifier UnitConversionID
        {
            get { return unitConversion.ID; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            bool allItemsStatus = cmbFromUnit.SelectedData != null &&
                                  cmbToUnit.SelectedData != null &&
                                  ntbFactor.Value > 0;

            if (cmbConversionRuleFor.SelectedIndex == 0)
            {
                btnOK.Enabled = allItemsStatus;
            }
            else
            {
                btnOK.Enabled = allItemsStatus && cmbItem.SelectedData != null;
            }

            lblExample.Text = ((cmbFromUnit.SelectedData != null) ? cmbFromUnit.SelectedData.Text : "")  + " x " + ntbFactor.Value.ToString("N2") + " = " + ((cmbToUnit.SelectedData) != null ? cmbToUnit.SelectedData.Text : "");
        }

        private void cmbConversionRuleFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbConversionRuleFor.SelectedIndex == 0)
            {
                lblItem.Enabled = false;
                cmbItem.Enabled = false;

                cmbItem.SelectedData = new DataEntity("", "");
            }
            else if (cmbConversionRuleFor.SelectedIndex == 1)
            {
                lblItem.Enabled = true;
                cmbItem.Enabled = true;
            }

            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbItem_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity)cmbItem.SelectedData).Text;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.RetailItems, textInitallyHighlighted);
        }

        private void cmbFromUnit_RequestData(object sender, EventArgs e)
        {
            cmbFromUnit.SetData(Providers.UnitData.GetList(PluginEntry.DataModel), null);
        }
        
        private void cmbToUnit_RequestData(object sender, EventArgs e)
        {
            cmbToUnit.SetData(Providers.UnitData.GetList(PluginEntry.DataModel), null);
        }

        private void btnSwapUnits_Click(object sender, EventArgs e)
        {
            DataEntity temp = (DataEntity)cmbFromUnit.SelectedData;

            cmbFromUnit.SelectedData = cmbToUnit.SelectedData;
            cmbToUnit.SelectedData = temp;

            CheckEnabled(this, EventArgs.Empty);
        }
    }
}