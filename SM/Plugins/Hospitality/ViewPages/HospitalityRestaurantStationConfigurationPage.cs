using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityRestaurantStationConfigurationPage : UserControl, ITabView
    {

        private PrintingStation printingStation;

        public HospitalityRestaurantStationConfigurationPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityRestaurantStationConfigurationPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            printingStation = (PrintingStation)internalContext;

            cmbCompressBOMReceipt.SelectedIndex = (int)printingStation.CompressBOMReceipt;
            cmbExcludeFromCompression.SelectedData = Providers.RetailItemData.Get(PluginEntry.DataModel, printingStation.ExcludeFromCompression) ?? new DataEntity("","");
        }

        public bool DataIsModified()
        {
            if (cmbCompressBOMReceipt.SelectedIndex != (int)printingStation.CompressBOMReceipt) return true;
            if ((string)cmbExcludeFromCompression.SelectedData.ID != printingStation.ExcludeFromCompression) return true;

            return false;
        }

        public bool SaveData()
        {
            printingStation.CompressBOMReceipt = (PrintingStation.CompressBOMReceiptEnum)cmbCompressBOMReceipt.SelectedIndex;
            printingStation.ExcludeFromCompression = (string)cmbExcludeFromCompression.SelectedData.ID;

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

        private void cmbExcludeFromCompression_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity)cmbExcludeFromCompression.SelectedData).ID;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(
            PluginEntry.DataModel,
            false,
            initialSearchText,
            SearchTypeEnum.RetailItems,
            textInitallyHighlighted);
        }

        private void cmbExcludeFromCompression_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbExcludeFromCompression_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }
    }
}
