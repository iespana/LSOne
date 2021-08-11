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
    public partial class HospitalitySetupGeneralPage : UserControl, ITabView
    {
        private HospitalitySetup setup;

        public HospitalitySetupGeneralPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalitySetupGeneralPage();
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            if (chkConfStatPrinting.Checked != setup.ConfirmStationPrinting) return true;
            if (chkLogStatPrinting.Checked != setup.LogStationPrinting) return true;
            if (ntbDaysBOMPrintExist.Value != (double)setup.DaysBOMPrintExist) return true;
            if (ntbDaysBOMMonitorExist.Value != (double)setup.DaysBOMMonitorExist) return true;
            if (chkAutoLogoffAtPOSExit.Checked != setup.AutoLogoffAtPOSExit) return true;
            if ((cmbNormalPOSSalesType.SelectedData != null) && (cmbNormalPOSSalesType.SelectedData.ID.ToString() != setup.NormalPOSSalesType)) return true;
            if (ntbOrdListScrlPageSize.Value != (double)setup.OrdListScrollPageSize) return true;

            if (cmbDineInTableSelection.SelectedIndex != (int)setup.DineInTableSelection) return true;
            if (cmbDineInTableLocking.SelectedIndex != (int)setup.DineInTableLocking) return true;
            if (ntbTableUpdateTimerInterval.Value != (int)setup.TableUpdateTimerInterval) return true;

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            throw new NotImplementedException();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            setup = (HospitalitySetup)internalContext;

            chkConfStatPrinting.Checked = setup.ConfirmStationPrinting;
            chkLogStatPrinting.Checked = setup.LogStationPrinting;
            ntbDaysBOMPrintExist.Value = setup.DaysBOMPrintExist;
            ntbDaysBOMMonitorExist.Value = setup.DaysBOMMonitorExist;
            chkAutoLogoffAtPOSExit.Checked = setup.AutoLogoffAtPOSExit;            
            cmbNormalPOSSalesType.SelectedData = String.IsNullOrEmpty((string)setup.NormalPOSSalesType) ? null : Providers.SalesTypeData.Get(PluginEntry.DataModel, setup.NormalPOSSalesType);
            ntbOrdListScrlPageSize.Value = setup.OrdListScrollPageSize;

            cmbDineInTableSelection.SelectedIndex = (int)setup.DineInTableSelection;
            cmbDineInTableLocking.SelectedIndex = (int)setup.DineInTableLocking;
            ntbTableUpdateTimerInterval.Value = (int)setup.TableUpdateTimerInterval;
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {            
            setup.ConfirmStationPrinting = chkConfStatPrinting.Checked;
            setup.LogStationPrinting = chkLogStatPrinting.Checked;
            setup.DaysBOMPrintExist = (int)ntbDaysBOMPrintExist.Value;
            setup.DaysBOMMonitorExist = (int)ntbDaysBOMMonitorExist.Value;
            setup.AutoLogoffAtPOSExit = chkAutoLogoffAtPOSExit.Checked;
            setup.NormalPOSSalesType = cmbNormalPOSSalesType.SelectedData == null ? "" : cmbNormalPOSSalesType.SelectedData.ID.ToString();
            setup.OrdListScrollPageSize = (int)ntbOrdListScrlPageSize.Value;
            setup.TableUpdateTimerInterval = (int)ntbTableUpdateTimerInterval.Value;

            setup.DineInTableSelection = (HospitalitySetup.SetupDineInTableSelection)cmbDineInTableSelection.SelectedIndex;
            setup.DineInTableLocking = (HospitalitySetup.SetupDineInTableLocking)cmbDineInTableLocking.SelectedIndex;

            return true;
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void cmbNormalPOSSalesType_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbNormalPOSSalesType_RequestData(object sender, EventArgs e)
        {
            cmbNormalPOSSalesType.SetData(Providers.SalesTypeData.GetList(PluginEntry.DataModel), null);
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
