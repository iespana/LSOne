using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    public partial class AdministrationScaleUnitsPage : ContainerControl, ITabViewV2
    {
        private List<Unit> units;
        private Parameters parameters;
        public AdministrationScaleUnitsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new AdministrationScaleUnitsPage();
        }

        #region ITabPanel Members

        public void InitializeView(RecordIdentifier context, object internalContext)
        {

        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            parameters = (Parameters) internalContext;
            if (units == null)
            {
                units = Providers.UnitData.GetAllUnits(PluginEntry.DataModel);
            }
            ddcbGram.SelectedData = units.Find(u => u.ID.PrimaryID == parameters.ScaleGramUnit);
            ddcbKiloGram.SelectedData = units.Find(u => u.ID.PrimaryID == parameters.ScaleKiloGramUnit);
            ddcbOunce.SelectedData = units.Find(u => u.ID.PrimaryID == parameters.ScaleOunceUnit);
            ddcbPound.SelectedData = units.Find(u => u.ID.PrimaryID == parameters.ScalePoundUnit);
        }

        public bool DataIsModified()
        {
            return ddcbGram.SelectedDataID != parameters.ScaleGramUnit
                   || ddcbKiloGram.SelectedDataID != parameters.ScaleKiloGramUnit
                   || ddcbOunce.SelectedDataID != parameters.ScaleOunceUnit
                   || ddcbPound.SelectedDataID != parameters.ScalePoundUnit;
        }

        public bool SaveData()
        {
            parameters.ScaleGramUnit = ddcbGram.SelectedDataID ?? "";
            parameters.ScaleKiloGramUnit = ddcbKiloGram.SelectedDataID ?? "";
            parameters.ScaleOunceUnit = ddcbOunce.SelectedDataID ?? "";
            parameters.ScalePoundUnit = ddcbPound.SelectedDataID ?? "";
            Providers.ParameterData.Save(PluginEntry.DataModel, parameters);
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            parameters = (Parameters)internalContext;
            units = null;

            LoadData(false, RecordIdentifier.Empty, internalContext);
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }
        #endregion

        private void ddcb_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(units, null);
        }

        private void ddcb_RequestClear(object sender, EventArgs e)
        {

        }
    }
}
