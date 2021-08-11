using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    public partial class HardwareProfileForecourtManagerPage : UserControl, ITabView
    {
        HardwareProfile profile;
        WeakReference unitAdder;

        public HardwareProfileForecourtManagerPage()
        {
            InitializeComponent();
            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "CanAddUnits", null);
            btnAddUnit.Visible = (plugin != null);
            unitAdder = new WeakReference(plugin);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileForecourtManagerPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkCallingBlink.Checked = profile.FCMCallingBlink;
            chkCallingSound.Checked = profile.FCMCallingSound;
            tbControlHostName.Text = profile.FCMControllerHostName;
            ntbFuellingPointColumns.Value = profile.FCMFuellingPointColumns;
            tbImplFileName.Text = profile.FCMImplFileName;
            tbImplFileType.Text = profile.FCMImplFileType;
            cmbLogLevel.SelectedIndex = (profile.FCMLogLevel < 2) ? profile.FCMLogLevel: 1; // This is a legacy issue where you could save too high a log level. The higest should be 1
            ntbScreenExtHeightPercentage.Value = (double)profile.FCMScreenExtHeightPercentage;
            ntbScreenHeightPercentage.Value = (double)profile.FCMScreenHeightPercentage;
            cmbVolumUnit.SelectedData = new DataEntity(profile.FCMVolumeUnitID, profile.FCMVolumeUnitDescription);
            tbServer.Text = profile.FCMServer;
            tbPort.Text = profile.FCMPort;
            tbPosPort.Text = profile.FCMPosPort;
            chkActive.Checked = profile.FCMActive;
            chkActive_CheckedChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            if (profile.FCMCallingBlink != chkCallingBlink.Checked) return true;
            if (profile.FCMCallingSound != chkCallingSound.Checked) return true;
            if (profile.FCMControllerHostName != tbControlHostName.Text) return true;
            if (profile.FCMFuellingPointColumns != (int)ntbFuellingPointColumns.Value) return true;
            if (profile.FCMImplFileName != tbImplFileName.Text) return true;
            if (profile.FCMImplFileType != tbImplFileType.Text) return true;
            if (profile.FCMLogLevel != cmbLogLevel.SelectedIndex) return true;
            if (profile.FCMScreenExtHeightPercentage != (int)ntbScreenExtHeightPercentage.Value) return true;
            if (profile.FCMScreenHeightPercentage != (int)ntbScreenHeightPercentage.Value) return true;
            if (profile.FCMVolumeUnitID != cmbVolumUnit.SelectedData.ID) return true;
            if (profile.FCMServer != tbServer.Text) return true;
            if (profile.FCMPort != tbPort.Text) return true;
            if (profile.FCMPosPort != tbPosPort.Text) return true;
            if (profile.FCMActive != chkActive.Checked) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.FCMCallingBlink = chkCallingBlink.Checked;
            profile.FCMCallingSound = chkCallingSound.Checked;
            profile.FCMControllerHostName = tbControlHostName.Text;
            profile.FCMFuellingPointColumns = (int)ntbFuellingPointColumns.Value;
            profile.FCMImplFileName = tbImplFileName.Text;
            profile.FCMImplFileType = tbImplFileType.Text;
            profile.FCMLogLevel = cmbLogLevel.SelectedIndex;
            profile.FCMScreenExtHeightPercentage = (int)ntbScreenExtHeightPercentage.Value;
            profile.FCMScreenHeightPercentage = (int)ntbScreenHeightPercentage.Value;
            profile.FCMVolumeUnitID = cmbVolumUnit.SelectedData.ID;
            profile.FCMServer = tbServer.Text;
            profile.FCMPosPort = tbPosPort.Text;
            profile.FCMPort = tbPort.Text;
            profile.FCMActive = chkActive.Checked;

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

        private void cmbInventoryUnit_RequestData(object sender, EventArgs e)
        {
            cmbVolumUnit.SetData(Providers.UnitData.GetList(PluginEntry.DataModel),
                null);
        }

        private void btnAddUnit_Click(object sender, EventArgs e)
        {
            ((IPlugin)unitAdder.Target).Message(this, "AddUnits", null);
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            chkCallingSound.Enabled = chkCallingBlink.Enabled = tbServer.Enabled 
                = tbPort.Enabled = tbPosPort.Enabled = tbControlHostName.Enabled 
                = tbImplFileName.Enabled = tbImplFileType.Enabled = cmbVolumUnit.Enabled 
                = btnAddUnit.Enabled = ntbFuellingPointColumns.Enabled 
                = ntbScreenHeightPercentage.Enabled = ntbScreenExtHeightPercentage.Enabled 
                = cmbLogLevel.Enabled = chkActive.Checked;
        }

        
    }
}
