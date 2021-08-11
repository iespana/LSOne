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
    public partial class HardwareProfileCashChangerPage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfileCashChangerPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileCashChangerPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkDeviceConnected.Checked = profile.CashChangerConnected;
            tbInitSettings.Text = profile.CashChangerInitSettings;
            tbPortSettings.Text = profile.CashChangerPortSettings;
            chkDeviceConnected_CheckedChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            if (chkDeviceConnected.Checked != profile.CashChangerConnected) return true;
            if (tbInitSettings.Text != profile.CashChangerInitSettings) return true;
            if (tbPortSettings.Text != profile.CashChangerPortSettings) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.CashChangerConnected = chkDeviceConnected.Checked;
            profile.CashChangerInitSettings = tbInitSettings.Text;
            profile.CashChangerPortSettings = tbPortSettings.Text;

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
            tbPortSettings.Enabled = tbInitSettings.Enabled = chkDeviceConnected.Checked;
        }
    }
}
