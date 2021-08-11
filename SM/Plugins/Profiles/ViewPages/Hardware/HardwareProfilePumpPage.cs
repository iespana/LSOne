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
    public partial class HardwareProfilePumpPage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfilePumpPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfilePumpPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkPumpActive.Checked = profile.Forecourt;
            tbPumpControlIDs.Text = profile.Hostname;
            chkPumpActive_CheckedChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            if (chkPumpActive.Checked != profile.Forecourt) return true;
            if (tbPumpControlIDs.Text != profile.Hostname) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.Forecourt = chkPumpActive.Checked;
            profile.Hostname = tbPumpControlIDs.Text;

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

        private void chkPumpActive_CheckedChanged(object sender, EventArgs e)
        {
            tbPumpControlIDs.Enabled = chkPumpActive.Checked;
        }
    }
}
