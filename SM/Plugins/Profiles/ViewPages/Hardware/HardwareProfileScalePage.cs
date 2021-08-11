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
    public partial class HardwareProfileScalePage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfileScalePage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileScalePage();
        }

        protected override void OnLoad(EventArgs e)
        {
            PluginOperations.SetRegistryStrings("Scale", cmbDeviceName);
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkDeviceConnected.Checked = profile.ScaleConnected;
            cmbDeviceName.Text = profile.ScaleDeviceName;
            tbDescription.Text = profile.ScaleDeviceDescription;
            chkManualInput.Checked = profile.ScaleManualInputAllowed;
            chkDeviceConnected_CheckedChanged(this, EventArgs.Empty);
            ValidateDeviceName();
        }

        public bool DataIsModified()
        {
            if (chkDeviceConnected.Checked != profile.ScaleConnected) return true;
            if (cmbDeviceName.Text != profile.ScaleDeviceName) return true;
            if (tbDescription.Text != profile.ScaleDeviceDescription) return true;
            if (chkManualInput.Checked != profile.ScaleManualInputAllowed) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.ScaleConnected = chkDeviceConnected.Checked;
            profile.ScaleDeviceName = cmbDeviceName.Text;
            profile.ScaleDeviceDescription = tbDescription.Text;
            profile.ScaleManualInputAllowed = chkManualInput.Checked;
               if (profile.ScaleConnected&&
                string.IsNullOrEmpty(profile.ScaleDeviceName))
            {
                
                PluginEntry.Framework.LogMessage(LogMessageType.Error, Properties.Resources.NoScaleSelected);
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
            cmbDeviceName.Enabled = tbDescription.Enabled = chkManualInput.Enabled = chkDeviceConnected.Checked;
            ValidateDeviceName();
        }

        private void cmbDeviceName_Leave(object sender, EventArgs e)
        {
            ValidateDeviceName();
        }

        private void ValidateDeviceName()
        {
            if (cmbDeviceName.Enabled &&
                cmbDeviceName.SelectedIndex < 0 &&
              string.IsNullOrEmpty(cmbDeviceName.Text))
            {

                errorProvider1.SetError(cmbDeviceName, Properties.Resources.NoScaleSelected);
            }
            else
            {
                errorProvider1.Clear();
            }
        }

    }
}
