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
    public partial class HardwareProfileBarcodReaderPage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfileBarcodReaderPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileBarcodReaderPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            PluginOperations.SetRegistryStrings("Scanner", cmbDeviceName);
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkDeviceConnected.Checked = profile.ScannerConnected;
            cmbDeviceName.Text = profile.ScannerDeviceName;
            tbDescription.Text = profile.ScannerDeviceDescription;
            chkDeviceConnected_CheckedChanged(this, EventArgs.Empty);

           ValidateDeviceName();
        }

        public bool DataIsModified()
        {
            if( chkDeviceConnected.Checked != profile.ScannerConnected) return true;
            if( cmbDeviceName.Text != profile.ScannerDeviceName) return true;
            if (tbDescription.Text != profile.ScannerDeviceDescription) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.ScannerConnected = chkDeviceConnected.Checked;
            profile.ScannerDeviceName = cmbDeviceName.Text;
            profile.ScannerDeviceDescription = tbDescription.Text;

               if (profile.ScannerConnected &&
                string.IsNullOrEmpty(profile.ScannerDeviceName))
            {
                
                PluginEntry.Framework.LogMessage(LogMessageType.Error, Properties.Resources.NoPrinterSelected);
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
            cmbDeviceName.Enabled = tbDescription.Enabled = chkDeviceConnected.Checked;
            ValidateDeviceName();
        }

        private void ValidateDeviceName()
        {
            if (cmbDeviceName.Enabled &&
                cmbDeviceName.SelectedIndex < 0 &&
              string.IsNullOrEmpty(cmbDeviceName.Text))
            {

                errorProvider1.SetError(cmbDeviceName, Properties.Resources.NoBarCodeReaderSelected);
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
