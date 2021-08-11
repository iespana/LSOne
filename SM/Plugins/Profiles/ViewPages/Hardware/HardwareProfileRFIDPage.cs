using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    public partial class HardwareProfileRFIDPage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfileRFIDPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileRFIDPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkDeviceConnected.Checked = profile.RFIDScannerConnected;
            tbHardwareName.Text = profile.RFIDScannerDeviceName;
            tbDescription.Text = profile.RfIDScannerDeviceDescription;
            chkDeviceConnected_CheckedChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            if (chkDeviceConnected.Checked != profile.RFIDScannerConnected) return true;
            if (tbHardwareName.Text != profile.RFIDScannerDeviceName) return true;
            if (tbDescription.Text != profile.RfIDScannerDeviceDescription) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.RFIDScannerConnected = chkDeviceConnected.Checked;
            profile.RFIDScannerDeviceName = tbHardwareName.Text;
            profile.RfIDScannerDeviceDescription = tbDescription.Text;

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

        private void chkDeviceConnected_CheckedChanged(object sender, EventArgs e)
        {
            tbDescription.Enabled = tbHardwareName.Enabled = chkDeviceConnected.Checked;
        }
    }
}
