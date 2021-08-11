using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityRestaurantStationMonitorPage : UserControl, ITabView
    {

        private PrintingStation printingStation;

        public HospitalityRestaurantStationMonitorPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityRestaurantStationMonitorPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            printingStation = (PrintingStation)internalContext;

            ntbEndTurnsRedAfterMin.Value = printingStation.EndTurnsRedAfterMin;
        }

        public bool DataIsModified()
        {
            if (ntbEndTurnsRedAfterMin.Value != printingStation.EndTurnsRedAfterMin) return true;
            
            return false;
        }

        public bool SaveData()
        {
            printingStation.EndTurnsRedAfterMin = (int)ntbEndTurnsRedAfterMin.Value;

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
    }
}
