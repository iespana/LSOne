using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    public partial class HardwareProfileKeyPage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfileKeyPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileKeyPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            PluginOperations.SetRegistryStrings("Keylock", cmbDeviceName);
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkDeviceConnected.Checked = profile.KeyLockDeviceType == HardwareProfile.DeviceTypes.OPOS;
            cmbDeviceName.Text = profile.KeyLockDeviceName;
            tbDescription.Text = profile.KeyLockDeviceDescription;
            chkDeviceConnected_CheckedChanged(this, EventArgs.Empty);
            ValidateDeviceName();
        }

        public bool DataIsModified()
        {
            if( chkDeviceConnected.Checked != (profile.KeyLockDeviceType == HardwareProfile.DeviceTypes.OPOS) ) return true;
            if( cmbDeviceName.Text != profile.KeyLockDeviceName) return true;
            if (tbDescription.Text != profile.KeyLockDeviceDescription) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.KeyLockDeviceType = chkDeviceConnected.Checked ? HardwareProfile.DeviceTypes.OPOS : HardwareProfile.DeviceTypes.None;
            profile.KeyLockDeviceName = cmbDeviceName.Text;
            profile.KeyLockDeviceDescription = tbDescription.Text;
               if (profile.KeyLockDeviceType == HardwareProfile.DeviceTypes.OPOS &&
                string.IsNullOrEmpty(profile.KeyLockDeviceName))
            {
                
                PluginEntry.Framework.LogMessage(LogMessageType.Error, Properties.Resources.NoKeySelected);
            }
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
            tbDescription.Enabled = cmbDeviceName.Enabled = chkDeviceConnected.Checked;
            ValidateDeviceName();
        }

        private void ValidateDeviceName()
        {
            if (cmbDeviceName.Enabled &&
                cmbDeviceName.SelectedIndex < 0 &&
              string.IsNullOrEmpty(cmbDeviceName.Text))
            {

                errorProvider1.SetError(cmbDeviceName, Properties.Resources.NoKeySelected);
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void cmbDeviceName_Leave(object sender, EventArgs e)
        {
            ValidateDeviceName();
        }


    }
}
