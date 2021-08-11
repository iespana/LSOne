using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    public partial class HardwareProfileDallasKeyPage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfileDallasKeyPage()
        {
            InitializeComponent();
            cmbStopBits.Items.Add(0.ToString());
            cmbStopBits.Items.Add(1.ToString());
            cmbStopBits.Items.Add(2.ToString());
            cmbStopBits.Items.Add(1.5f.ToString());
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileDallasKeyPage();
        }

        protected override void OnLoad(EventArgs e)
        {

        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkDeviceConnected.Checked = profile.DallasKeyConnected;
            txtMessagePrefix.Text = profile.DallasMessagePrefix;
            txtKeyRemovedMessage.Text = profile.DallasKeyRemovedMessage;
            txtPortName.Text = profile.DallasComPort;
            ntbBaudRate.Value = profile.DallasBaudRate;
            cmbParity.SelectedIndex = (int)profile.DallasParity;
            ntbDataBits.Value = profile.DallasDataBits;
            cmbStopBits.SelectedIndex = (int) profile.DallasStopBits;
            chkDeviceConnected_CheckedChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            return chkDeviceConnected.Checked != profile.DallasKeyConnected ||
                   txtMessagePrefix.Text != profile.DallasMessagePrefix ||
                   txtKeyRemovedMessage.Text != profile.DallasKeyRemovedMessage ||
                   txtPortName.Text != profile.DallasComPort ||
                   ntbBaudRate.Value != profile.DallasBaudRate ||
                   cmbParity.SelectedIndex != (int)profile.DallasParity ||
                   ntbDataBits.Value != profile.DallasDataBits ||
                   cmbStopBits.SelectedIndex != (int)profile.DallasStopBits;
        }

        public bool SaveData()
        {
            profile.DallasKeyConnected = chkDeviceConnected.Checked;
            profile.DallasMessagePrefix = txtMessagePrefix.Text;
            profile.DallasKeyRemovedMessage = txtKeyRemovedMessage.Text;
            profile.DallasComPort = txtPortName.Text;
            profile.DallasBaudRate = (int)ntbBaudRate.Value;
            profile.DallasParity = (Parity) cmbParity.SelectedIndex;
            profile.DallasDataBits = (int)ntbDataBits.Value;
            profile.DallasStopBits = (StopBits)cmbStopBits.SelectedIndex;

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
            txtMessagePrefix.Enabled = txtKeyRemovedMessage.Enabled = txtPortName.Enabled = ntbBaudRate.Enabled 
                = cmbParity.Enabled = ntbDataBits.Enabled = cmbStopBits.Enabled = chkDeviceConnected.Checked;
        }

    }
}
