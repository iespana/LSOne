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
    public partial class HardwareProfileCameraPage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfileCameraPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileCameraPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkCameraOn.Checked = profile.CctvConnected;
            tbCamera.Text = profile.CctvCamera;
            tbServer.Text = profile.CctvHostname;
            ntbPort.Value = (double)profile.CctvPort;
            chkCameraOn_CheckedChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            if( chkCameraOn.Checked != profile.CctvConnected) return true;
            if( tbCamera.Text != profile.CctvCamera) return true;
            if( tbServer.Text != profile.CctvHostname) return true;
            if (ntbPort.Value != (double)profile.CctvPort) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.CctvConnected = chkCameraOn.Checked;
            profile.CctvCamera = tbCamera.Text;
            profile.CctvHostname = tbServer.Text;
            profile.CctvPort = (int)ntbPort.Value;

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

        private void chkCameraOn_CheckedChanged(object sender, EventArgs e)
        {
            tbCamera.Enabled = tbServer.Enabled = ntbPort.Enabled = chkCameraOn.Checked;
        }
    }
}
