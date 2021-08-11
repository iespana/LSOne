using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityRestaurantStationPrintingPage : UserControl, ITabView
    {

        private PrintingStation printingStation;

        public HospitalityRestaurantStationPrintingPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityRestaurantStationPrintingPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            printingStation = (PrintingStation)internalContext;

            cmbCheckPrinting.SelectedIndex = (int)printingStation.CheckPrinting;
            cmbPrinting.SelectedIndex = (int)printingStation.Printing;
            cmbOutputLines.SelectedIndex = (int)printingStation.OutputLines;
            ntbPrintingPriority.Value = printingStation.PrintingPriority;

            string[] stationFilter = printingStation.StationFilter.Split(new char[] { '|' });

            // We check for stationFilter[0] != "" because if you split an empty string, you still get a string array containing one empty string
            cmbStation1.SelectedData = stationFilter.Length >= 1 && stationFilter[0] != "" ? Providers.PrintingStationData.Get(PluginEntry.DataModel, stationFilter[0]) : new DataEntity("", "");
            cmbStation2.SelectedData = stationFilter.Length >= 2 && stationFilter[1] != "" ? Providers.PrintingStationData.Get(PluginEntry.DataModel, stationFilter[1]) : new DataEntity("", "");
            cmbStation3.SelectedData = stationFilter.Length >= 3 && stationFilter[2] != "" ? Providers.PrintingStationData.Get(PluginEntry.DataModel, stationFilter[2]) : new DataEntity("", "");

        }

        public bool DataIsModified()
        {
            if(cmbCheckPrinting.SelectedIndex != (int)printingStation.CheckPrinting) return true;
            if(cmbPrinting.SelectedIndex != (int)printingStation.Printing) return true;
            if(cmbOutputLines.SelectedIndex != (int)printingStation.OutputLines) return true;
            if(ntbPrintingPriority.Value != printingStation.PrintingPriority) return true;            
            if (GetStationFilter() != printingStation.StationFilter) return true;
            return false;
        }

        public bool SaveData()
        {
            printingStation.CheckPrinting = (PrintingStation.CheckPrintingEnum)cmbCheckPrinting.SelectedIndex;
            printingStation.Printing = (PrintingStation.PrintingEnum)cmbPrinting.SelectedIndex;
            printingStation.OutputLines = (PrintingStation.OutputLinesEnum)cmbOutputLines.SelectedIndex;
            printingStation.PrintingPriority = (int)ntbPrintingPriority.Value;
            printingStation.StationFilter = GetStationFilter();

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private string GetStationFilter()
        {
            string filter = "";

            filter += (string)cmbStation1.SelectedData.ID != "" ? (string)cmbStation1.SelectedData.ID + "|" : "";
            filter += (string)cmbStation2.SelectedData.ID != "" ? (string)cmbStation2.SelectedData.ID + "|" : "";
            filter += (string)cmbStation3.SelectedData.ID != "" ? (string)cmbStation3.SelectedData.ID : "";
            filter = filter.TrimEnd(new char[] { '|' });

            return filter;
        }

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbStation_RequestData(object sender, EventArgs e)
        {
             ((DualDataComboBox)sender).SetData(Providers.PrintingStationData.GetList(PluginEntry.DataModel), null);
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }
    }
}
