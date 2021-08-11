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
    public partial class HardwareProfileLineDisplayPage : UserControl, ITabView
    {
        private HardwareProfile profile;
        List<string> OPOSLineDisplayList;

        public HardwareProfileLineDisplayPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileLineDisplayPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            PluginOperations.SetRegistryStrings("LineDisplay", cmbDeviceName);
            OPOSLineDisplayList = PluginOperations.GetRegistryStrings("LineDisplay");
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            cmbDisplay.SelectedIndex = (int)profile.LineDisplayDeviceType;
            cmbDeviceName.Text = profile.DisplayDeviceName;
            tbDescription.Text = profile.DisplayDeviceDescription;
            tbDisplayTotalText.Text = profile.DisplayTotalText;
            tbDisplayBalanceText.Text = profile.DisplayBalanceText;
            tbTillClosedLine1.Text = profile.DisplayClosedLine1;
            tbTillClosedLine2.Text = profile.DisplayClosedLine2;
            ntbCharset.Value = profile.DisplayCharacterSet;
            chkLineDisplayBinaryConversion.Checked = profile.DisplayBinaryConversion;
            ValidateDeviceName();
        }

        public bool DataIsModified()
        {
            if (cmbDisplay.SelectedIndex != (int)profile.LineDisplayDeviceType) return true;
            if (cmbDeviceName.Text != profile.DisplayDeviceName) return true;
            if (tbDescription.Text != profile.DisplayDeviceDescription) return true;
            if (tbDisplayTotalText.Text != profile.DisplayTotalText) return true;
            if (tbDisplayBalanceText.Text != profile.DisplayBalanceText) return true;
            if (tbTillClosedLine1.Text != profile.DisplayClosedLine1) return true;
            if (tbTillClosedLine2.Text != profile.DisplayClosedLine2) return true;
            if ((int)ntbCharset.Value != profile.DisplayCharacterSet) return true;
            if (chkLineDisplayBinaryConversion.Checked != profile.DisplayBinaryConversion) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.LineDisplayDeviceType = (HardwareProfile.LineDisplayDeviceTypes) cmbDisplay.SelectedIndex;
            profile.DisplayDeviceName = cmbDeviceName.Text;
            profile.DisplayDeviceDescription = tbDescription.Text;
            profile.DisplayTotalText = tbDisplayTotalText.Text;
            profile.DisplayBalanceText = tbDisplayBalanceText.Text;
            profile.DisplayClosedLine1 = tbTillClosedLine1.Text;
            profile.DisplayClosedLine2 = tbTillClosedLine2.Text;
            profile.DisplayCharacterSet = (int)ntbCharset.Value;
            profile.DisplayBinaryConversion = chkLineDisplayBinaryConversion.Checked;
            if (profile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.OPOS && string.IsNullOrEmpty(profile.DisplayDeviceName))
            {
                PluginEntry.Framework.LogMessage(LogMessageType.Error, Properties.Resources.NoLineDisplaySelected);
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


        private void ValidateDeviceName()
        {
            if (cmbDeviceName.Enabled &&
                cmbDeviceName.SelectedIndex < 0 &&
              string.IsNullOrEmpty(cmbDeviceName.Text))
            {

                errorProvider1.SetError(cmbDeviceName, Properties.Resources.NoLineDisplaySelected);
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void cmbDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((HardwareProfile.LineDisplayDeviceTypes)cmbDisplay.SelectedIndex)
            {
                case HardwareProfile.LineDisplayDeviceTypes.OPOS:
                    cmbDeviceName.Items.Clear();
                    cmbDeviceName.Items.AddRange(OPOSLineDisplayList.ToArray());
                    cmbDeviceName.Text = "";
                    cmbDeviceName.Enabled = true;
                    break;                    
                case HardwareProfile.LineDisplayDeviceTypes.VirtualDisplay:
                case HardwareProfile.LineDisplayDeviceTypes.None:
                    cmbDeviceName.Text = "";
                    cmbDeviceName.Enabled = false;
                    break;
                default:
                    break;
            }
            ValidateDeviceName();
        }

        private void cmbDeviceName_Leave(object sender, EventArgs e)
        {
            ValidateDeviceName();
        }



        
    }
}
