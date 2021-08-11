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
    public partial class HardwareProfileCardReaderPage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfileCardReaderPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileCardReaderPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            PluginOperations.SetRegistryStrings("MSR", cmbDeviceName);
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile) internalContext;

            cmbMSR.SelectedIndex = (int) profile.MsrDeviceType;
            cmbDeviceName.Text = profile.MsrDeviceName;
            tbDescription.Text = profile.MsrDeviceDescription;
            tbStartDigit.Text = profile.StartTrack1;
            tbDiffDigit.Text = profile.Separator1;
            tbFinalDigit.Text = profile.EndTrack1;
            ValidateDeviceName();
        }

        public bool DataIsModified()
        {
            if (cmbMSR.SelectedIndex != (int) profile.MsrDeviceType) return true;
            if (cmbDeviceName.Text != profile.MsrDeviceName) return true;
            if (tbDescription.Text != profile.MsrDeviceDescription) return true;
            if (tbStartDigit.Text != profile.StartTrack1) return true;
            if (tbDiffDigit.Text != profile.Separator1) return true;
            if (tbFinalDigit.Text != profile.EndTrack1) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.MsrDeviceType = (HardwareProfile.DeviceTypes)cmbMSR.SelectedIndex;
            profile.MsrDeviceName = cmbDeviceName.Text;
            profile.MsrDeviceDescription = tbDescription.Text;
            profile.StartTrack1 = tbStartDigit.Text;
            profile.Separator1 = tbDiffDigit.Text;
            profile.EndTrack1 = tbFinalDigit.Text;
            if (profile.MsrDeviceType == HardwareProfile.DeviceTypes.OPOS &&
                string.IsNullOrEmpty(profile.MsrDeviceName))
            {

                PluginEntry.Framework.LogMessage(LogMessageType.Error, Properties.Resources.NoCardReaderSelected);
            }
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier,
                                  object param)
        {

        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void ValidateDeviceName()
        {
            if (cmbDeviceName.Enabled &&
                cmbDeviceName.SelectedIndex < 0 &&
                string.IsNullOrEmpty(cmbDeviceName.Text))
            {

                errorProvider1.SetError(cmbDeviceName, Properties.Resources.NoCardReaderSelected);
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

        private void cmbMSR_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDeviceName.Enabled = tbDescription.Enabled = cmbMSR.SelectedIndex == 1;
            ValidateDeviceName();
        }

    }
}