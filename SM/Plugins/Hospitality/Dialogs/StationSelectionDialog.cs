using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Hospitality.ListItems;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class StationSelectionDialog : DialogBase
    {
        private RecordIdentifier stationSelectionID;
        private StationSelection stationSelection;
        private bool editingExisting;
        private bool suppressEvents;

        /// <summary>
        /// Opens the dialog to create a new station selection
        /// </summary>
        public StationSelectionDialog()
        {
            InitializeComponent();
            
            stationSelectionID = RecordIdentifier.Empty;
            stationSelection = new StationSelection();
            editingExisting = false;
            suppressEvents = false;

            LoadData();
        }

        /// <summary>
        /// Opens the dialog for editing an existing restaurant menu type
        /// </summary>
        /// <param name="stationSelectionID">The ID of an existing station selection</param>
        public StationSelectionDialog(RecordIdentifier stationSelectionID)
        {
            InitializeComponent();

            this.stationSelectionID = stationSelectionID;
            stationSelection = Providers.StationSelectionData.Get(PluginEntry.DataModel, stationSelectionID);
            editingExisting = true;
            suppressEvents = false;

            LoadData();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            btnAddHospitalityType.Visible = PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes);
        }

        private void LoadData()
        {
            suppressEvents = true;

            tbDescription.Text = stationSelection.Text;
            DataEntity defaultEntity = new DataEntity("", "");
               
            if (this.stationSelectionID == RecordIdentifier.Empty)
            {
                List<HospitalityTypeListItem> types =
                    Providers.HospitalityTypeData.GetHospitalityTypes(PluginEntry.DataModel);
                if (types.Count == 1)
                {
                    defaultEntity = types[0];
                }
               
            }


            cmbHospitalityType.SelectedData = Providers.HospitalityTypeData.Get(PluginEntry.DataModel, stationSelection.RestaurantID, stationSelection.SalesType) ?? defaultEntity;
            cmbStationSelectionType.SelectedIndex = (int)stationSelection.Type;

            switch ((StationSelection.TypeEnum)(int)stationSelection.Type)
            {
                case StationSelection.TypeEnum.Item:
                    cmbCode.SelectedData = Providers.RetailItemData.Get(PluginEntry.DataModel, stationSelection.Code) ?? new DataEntity("", "");
                    break;

                case StationSelection.TypeEnum.RetailGroup:
                    cmbCode.SelectedData = Providers.RetailGroupData.Get(PluginEntry.DataModel, stationSelection.Code) ?? new DataEntity("", "");
                    break;

                case StationSelection.TypeEnum.SpecialGroup:
                    cmbCode.SelectedData = Providers.SpecialGroupData.Get(PluginEntry.DataModel, stationSelection.Code) ?? new DataEntity("", "");
                    break;

                default:
                    cmbCode.SelectedData = new DataEntity(stationSelection.Code, "");
                    break;
            }

            cmbStation.SelectedData = Providers.PrintingStationData.GetDataEntity(PluginEntry.DataModel, stationSelection.StationID) ?? new DataEntity("", "");

            suppressEvents = false;

            CheckEnabled(this, EventArgs.Empty);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier StationSelectionID
        {
            get { return stationSelectionID; }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (suppressEvents)
            {
                return;
            }

            errorProvider1.Clear();

            cmbCode.Enabled = (StationSelection.TypeEnum)cmbStationSelectionType.SelectedIndex != StationSelection.TypeEnum.All;

            if (editingExisting)
            {
                btnOK.Enabled = IsModified() && 
                                cmbStationSelectionType.SelectedIndex == (int)StationSelection.TypeEnum.All || 
                                (cmbCode.SelectedData.ID != "");
            }
            else
            {
                btnOK.Enabled =
                    cmbHospitalityType.SelectedData != null &&
                    cmbHospitalityType.SelectedData.ID != "" &&
                    cmbStation.SelectedData != null &&
                    cmbStation.SelectedData.ID != RecordIdentifier.Empty &&
                    tbDescription.Text.Length > 0 &&
                    (cmbStationSelectionType.SelectedIndex == (int) StationSelection.TypeEnum.All ||
                     (cmbCode.SelectedData.ID != ""));

                
            }
        }

        private bool IsModified()
        {
            if (cmbHospitalityType.SelectedData != null)
            {
                // Get a (RestaurantID, SalesType) recordidentifier to compare with the station selection record
                RecordIdentifier simpleID = new RecordIdentifier(cmbHospitalityType.SelectedData.ID[0], cmbHospitalityType.SelectedData.ID[2]);

                if (simpleID != new RecordIdentifier(stationSelection.RestaurantID, stationSelection.SalesType))
                {
                    return true;
                }
            }

            if (cmbStationSelectionType.SelectedIndex != (int)stationSelection.Type) return true;
            if (cmbCode.SelectedData.ID != stationSelection.Code) return true;
            if (cmbStation.SelectedData != null && cmbStation.SelectedData.ID != stationSelection.StationID) return true;
            if (cmbStation.SelectedData.ID != stationSelection.StationID) return true;
            if (tbDescription.Text != stationSelection.Text) return true;

            return false;
        }

        private void Save()
        {
            stationSelection.RestaurantID = cmbHospitalityType.SelectedData.ID[0];
            stationSelection.SalesType = cmbHospitalityType.SelectedData.ID[2];
            stationSelection.Type = cmbStationSelectionType.SelectedIndex;
            stationSelection.Code = cmbCode.SelectedData.ID;
            stationSelection.StationID = cmbStation.SelectedData.ID;
            stationSelection.Text = tbDescription.Text;

            Providers.StationSelectionData.Save(PluginEntry.DataModel, stationSelection);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RecordIdentifier tempStationSelectionID = new RecordIdentifier(cmbHospitalityType.SelectedData.ID[0],
                                                        new RecordIdentifier(cmbHospitalityType.SelectedData.ID[2],
                                                        new RecordIdentifier(cmbStationSelectionType.SelectedIndex,
                                                        new RecordIdentifier(cmbCode.SelectedData.ID,cmbStation.SelectedData.ID))));

            // Since all fields except description are primary keys, editing any of them could possibly create a new record 
            // or edit another existing record. 
            
            if (tempStationSelectionID != stationSelection.ID && Providers.StationSelectionData.Exists(PluginEntry.DataModel, stationSelection.ID))
            {

                // At this point we might either be about to create a new record or possibly overwriting an existing one
                if (Providers.StationSelectionData.Exists(PluginEntry.DataModel, tempStationSelectionID))
                {
                    errorProvider1.SetError(cmbHospitalityType, Properties.Resources.StationSelectionExists);
                    errorProvider1.SetError(cmbStationSelectionType, Properties.Resources.StationSelectionExists);
                    errorProvider1.SetError(cmbCode, Properties.Resources.StationSelectionExists);
                    errorProvider1.SetError(cmbStation, Properties.Resources.StationSelectionExists);
                    //errorProvider1.SetError(cmbRoute, Properties.Resources.StationSelectionExists);
                    cmbHospitalityType.Focus();
                    return;
                }

                Providers.StationSelectionData.Delete(PluginEntry.DataModel, stationSelection.ID);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "StationSelection", stationSelection.ID, null);
            }

                Save();

            if (editingExisting)
            {              
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "StationSelection", stationSelection.ID, null);
                
            }
            else
            {                
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "StationSelection", stationSelection.ID, null);
            }

            stationSelectionID = stationSelection.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbStation_RequestData(object sender, EventArgs e)
        {
            var listOfPrintingStations = Providers.PrintingStationData.GetList(PluginEntry.DataModel);
            List<DataEntity> printingStationsDataEntities = new List<DataEntity>(listOfPrintingStations);
            cmbStation.SetData(printingStationsDataEntities, null);
        }

        private void cmbCode_RequestData(object sender, EventArgs e)
        {
            switch ((StationSelection.TypeEnum)cmbStationSelectionType.SelectedIndex)
            {
                case StationSelection.TypeEnum.Item:
                    break;

                case StationSelection.TypeEnum.RetailGroup:
                    cmbCode.SetData(Providers.RetailGroupData.GetList(PluginEntry.DataModel), null);
                    break;

                case StationSelection.TypeEnum.SpecialGroup:
                    cmbCode.SetData(Providers.SpecialGroupData.GetList(PluginEntry.DataModel), null);
                    break;
            }
        }

        private void cmbCode_DropDown(object sender, DropDownEventArgs e)
        {
            if ((StationSelection.TypeEnum)cmbStationSelectionType.SelectedIndex == StationSelection.TypeEnum.Item)
            {
                cmbCode.ShowDropDownOnTyping = true;
                RecordIdentifier initialSearchText;
                bool textInitallyHighlighted;
                if (e.DisplayText != "")
                {
                    initialSearchText = e.DisplayText;
                    textInitallyHighlighted = false;
                }
                else
                {
                    initialSearchText = ((DataEntity)cmbCode.SelectedData).ID;
                    textInitallyHighlighted = true;
                }

                e.ControlToEmbed = new SingleSearchPanel(
                PluginEntry.DataModel,
                false,
                initialSearchText,
                SearchTypeEnum.RetailItems,
                textInitallyHighlighted);
            }
            else
            {
                cmbCode.ShowDropDownOnTyping = false;
            }
        }

        private void btnAddHospitalityType_Click(object sender, EventArgs e)
        {
            PluginOperations.NewHospitalityType();
        }

        private void cmbHospitalityType_RequestData(object sender, EventArgs e)
        {
            cmbHospitalityType.SetWidth(350);
            cmbHospitalityType.SetHeaders(new string[] { Properties.Resources.RestaurantId, Properties.Resources.SalesType, Properties.Resources.Description },
                                          new int[] { 0, 1, 3 }
                                          );

            cmbHospitalityType.SetData(Providers.HospitalityTypeData.GetHospitalityTypes(PluginEntry.DataModel), null); 
        }

        private void cmbStationSelectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbCode.SelectedData = new DataEntity("","");

            CheckEnabled(null, EventArgs.Empty);
        }
        
    }
}
