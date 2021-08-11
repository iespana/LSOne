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
    public partial class HardwareProfileDrawerPage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfileDrawerPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileDrawerPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            PluginOperations.SetRegistryStrings("CashDrawer", cmbDeviceName);
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkDrawerConnected.Checked = profile.DrawerDeviceType == HardwareProfile.DeviceTypes.OPOS;
            cmbDeviceName.Text = profile.DrawerDeviceName;
            tbDescription.Text = profile.DrawerDescription;
            tbOpenText.Text = profile.DrawerOpenText;
            chkDrawerConnected_CheckedChanged(this, EventArgs.Empty);
            ValidateDeviceName();
        }

        public bool DataIsModified()
        {
            if (chkDrawerConnected.Checked != (profile.DrawerDeviceType  == HardwareProfile.DeviceTypes.OPOS)) return true;
            if (cmbDeviceName.Text != profile.DrawerDeviceName) return true;
            if (tbDescription.Text != profile.DrawerDescription) return true;
            if (tbOpenText.Text != profile.DrawerOpenText) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.DrawerDeviceType = chkDrawerConnected.Checked ?HardwareProfile.DeviceTypes.OPOS : HardwareProfile.DeviceTypes.None;
            profile.DrawerDeviceName = cmbDeviceName.Text;
            profile.DrawerDescription = tbDescription.Text;
            profile.DrawerOpenText = tbOpenText.Text;
            if (profile.DrawerDeviceType == HardwareProfile.DeviceTypes.OPOS &&
             string.IsNullOrEmpty(profile.DrawerDeviceName))
            {

                PluginEntry.Framework.LogMessage(LogMessageType.Error, Properties.Resources.NoDrawerSelected);
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

        private void chkDrawerConnected_CheckedChanged(object sender, EventArgs e)
        {
            cmbDeviceName.Enabled = tbDescription.Enabled = tbOpenText.Enabled = chkDrawerConnected.Checked;
            ValidateDeviceName();
        }

        private void ValidateDeviceName()
        {
            if (cmbDeviceName.Enabled &&
                cmbDeviceName.SelectedIndex < 0 &&
              string.IsNullOrEmpty(cmbDeviceName.Text))
            {

                errorProvider1.SetError(cmbDeviceName, Properties.Resources.NoDrawerSelected);
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
