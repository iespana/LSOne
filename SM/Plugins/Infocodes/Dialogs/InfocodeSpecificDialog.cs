using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Infocodes.Dialogs
{
    public partial class InfocodeSpecificDialog : DialogBase
    {
        private RecordIdentifier infocodeID;
        private RecordIdentifier refRelationID;
        private RecordIdentifier refRelation2ID; // Store tender type ID
        private RecordIdentifier refRelation3ID; // Card type ID
        private RefTableEnum tableRefID;
        private UsageCategoriesEnum usageCategory;
        bool newRecord;

        private const int InfocodeIndex = 0;
        private const int InfocodeGroupIndex = 1;

        public InfocodeSpecific InfocodeSpecific { get; set; }

        /// <summary>
        /// Opens a new dialog for creating infocodeSpecific records for store tender card types.
        /// </summary>
        /// <param name="refRelationID">The infocodeSpecific ID(InfocodeID, RefRelation)</param>
        /// <param name="refRelation2ID">The tender type ID</param>
        /// <param name="refRelation3ID">The card type ID</param>
        /// <param name="tableRefID">The table ref ID for the infocode specific</param>        
        /// <param name="usageCategory">The usage category for the infocode specific</param>
        /// <param name="infocodeID"></param>
        /// <param name="newRecord"></param>
        public InfocodeSpecificDialog(
            RecordIdentifier refRelationID, 
            RecordIdentifier refRelation2ID, 
            RecordIdentifier refRelation3ID, 
            RefTableEnum tableRefID, 
            UsageCategoriesEnum usageCategory, 
            RecordIdentifier infocodeID, 
            bool newRecord)
        {
            InitializeComponent();

            this.infocodeID = infocodeID;
            this.refRelationID = refRelationID;
            this.refRelation2ID = refRelation2ID;
            this.refRelation3ID = refRelation3ID;
            this.tableRefID = tableRefID;
            this.usageCategory = usageCategory;
            this.newRecord = newRecord; 

            cmbInfocodeLink.SelectedData = new DataEntity("","");
            cmbSalesType.SelectedData = new DataEntity("", "");
            cmbTriggering.SelectedIndex = 0;
            cmbUnit.SelectedData = new DataEntity("", "");

            chkInputRequired.Checked = false;

            cmbInfocodeType_SelectedDataChanged(this, EventArgs.Empty);

            if (tableRefID == RefTableEnum.Item)
            {
                lblUOM.Visible = true;
                cmbUnit.Visible = true;
            }
            else
            {
                lblUOM.Visible = false;
                cmbUnit.Visible = false;
            }

            if (usageCategory == UsageCategoriesEnum.None)
            {
                cmbInfocodeType.Enabled = true;
                cmbInfocodeType.SelectedIndex = InfocodeIndex;
            }
            else
            {
                cmbInfocodeType.Enabled = false;
                cmbInfocodeType.SelectedIndex = InfocodeGroupIndex;
            }

            if (newRecord == false)
            {
                cmbInfocodeType.Enabled = false;
                cmbInfocodeLink.Enabled = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InfocodeSpecific infocodeSpecific = Providers.InfocodeSpecificData.Get(PluginEntry.DataModel, new RecordIdentifier(this.infocodeID, new RecordIdentifier(this.refRelationID, new RecordIdentifier(new RecordIdentifier(), new RecordIdentifier()))));
            if (infocodeSpecific != null)
            {
                Infocode infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, infocodeSpecific.InfocodeId);
                SalesType salesType = Providers.SalesTypeData.Get(PluginEntry.DataModel, infocodeSpecific.SalesTypeFilter);

                cmbInfocodeLink.SelectedData = infocode == null ? new DataEntity("","") : new DataEntity(infocode.ID, infocode.Text);
                cmbSalesType.SelectedData = salesType == null ? new DataEntity("", "") : new DataEntity(salesType.ID, salesType.Text);
                cmbTriggering.SelectedIndex = (int)infocodeSpecific.Triggering;
                Unit unit = Providers.UnitData.Get(PluginEntry.DataModel, infocodeSpecific.UnitOfMeasure);
                cmbUnit.SelectedData = (unit == null) ? new DataEntity() : new DataEntity(unit.ID, unit.Text);

                chkInputRequired.Checked = infocodeSpecific.InputRequired;
            }
            CheckEnabled(this, EventArgs.Empty);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier InfocodeSpecificID
        {
            get { return new RecordIdentifier(cmbInfocodeLink.SelectedData.ID, new RecordIdentifier(refRelationID, new RecordIdentifier(refRelation2ID, refRelation3ID))); }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = cmbSalesType.Enabled = cmbTriggering.Enabled = cmbUnit.Enabled = cmbInfocodeLink.SelectedData.ID != "";
            cmbInfocodeLink.Enabled = (cmbInfocodeType.SelectedIndex >= 0) && newRecord;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            InfocodeSpecific = new InfocodeSpecific();
            InfocodeSpecific.RefRelation = refRelationID;
            InfocodeSpecific.RefRelation2 = refRelation2ID;
            InfocodeSpecific.RefRelation3 = refRelation3ID;
            InfocodeSpecific.UsageCategory = usageCategory;
            InfocodeSpecific.RefTableId = tableRefID;
            
            InfocodeSpecific.InfocodeId = cmbInfocodeLink.SelectedData.ID;
            InfocodeSpecific.Triggering = (TriggeringEnum)cmbTriggering.SelectedIndex;
            InfocodeSpecific.SalesTypeFilter = cmbSalesType.SelectedData.ID;
            InfocodeSpecific.UnitOfMeasure = cmbUnit.SelectedData.ID;
            InfocodeSpecific.InputRequired = chkInputRequired.Checked;

            Providers.InfocodeSpecificData.Save(PluginEntry.DataModel, InfocodeSpecific);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");

            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbInfocodeLink_RequestData(object sender, EventArgs e)
        {
            if (cmbInfocodeType.SelectedIndex == InfocodeIndex)
            {
                var items = Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new[] { UsageCategoriesEnum.None }, true, refRelationID, tableRefID);
                cmbInfocodeLink.SetData(items, null);
            }
            else if (cmbInfocodeType.SelectedIndex == InfocodeGroupIndex)
            {
                UsageCategoriesEnum[] usageCategories = usageCategory == UsageCategoriesEnum.None ? new[] { UsageCategoriesEnum.CrossSelling, UsageCategoriesEnum.ItemModifier } : new UsageCategoriesEnum[] { usageCategory };
                List<Infocode> infoList = Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, usageCategories, true, new InputTypesEnum[] { InputTypesEnum.Group }, true, refRelationID,0);
                cmbInfocodeLink.SetData(infoList, null);
            }
        }

        private void cmbSalesType_RequestData(object sender, EventArgs e)
        {
            cmbSalesType.SetData(Providers.SalesTypeData.GetList(PluginEntry.DataModel),null);
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
                    {Providers.UnitData.GetUnitForItem(PluginEntry.DataModel,refRelationID,0,false,UnitTypeEnum.InventoryUnit).Cast<DataEntity>(),
                     Providers.UnitData.GetAllUnits(PluginEntry.DataModel).Cast<DataEntity>()};

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                data,
                new string[] { Properties.Resources.Convertible, Properties.Resources.All },
                250);

            cmbUnit.SetData(data, pnl);
        }

        private void cmbInfocodeType_SelectedDataChanged(object sender, EventArgs e)
        {
            switch (cmbInfocodeType.SelectedIndex)
            {
                case InfocodeIndex:
                    lblInputRequired.Enabled = true;
                    chkInputRequired.Enabled = true;
                    break;
                case InfocodeGroupIndex:
                    lblInputRequired.Enabled = false;
                    chkInputRequired.Enabled = false;
                    chkInputRequired.Checked = false;
                    break;
            }

            CheckEnabled(sender, e);
        }

        private void cmbInfocodeLink_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbInfocodeType.SelectedIndex == InfocodeIndex && cmbInfocodeLink.SelectedData.ID != "")
            {
                var selectedInfocode = (Infocode)cmbInfocodeLink.SelectedData;
                chkInputRequired.Checked = selectedInfocode.InputRequired;
            }

            CheckEnabled(sender, e);
        }
    }
}