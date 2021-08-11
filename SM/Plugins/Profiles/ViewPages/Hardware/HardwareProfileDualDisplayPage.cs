using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls.ScreenIdentity;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using WebBrowser = LSOne.Utilities.Settings.WebBrowser;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    public partial class HardwareProfileDualDisplayPage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfileDualDisplayPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileDualDisplayPage();
        }

        protected override void OnLoad(EventArgs e)
        {
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkDeviceConnected.Checked = profile.DualDisplayConnected;
            tbDescription.Text = profile.DualDisplayDescription;
            ntbReceiptWidth.Value = (double)profile.DualDisplayReceiptPrecentage;
            cmbAdvertType.SelectedIndex = (int)profile.DualDisplayType;
            tbImageRotatorPath.Text = profile.DualdisplayImagePath;
            ntbInterval.Value = profile.DualDisplayImageInterval;
            tbWebPage.Text = profile.DualDisplayBrowserUrl;
            cmbScreens.SelectedIndex = (int) profile.DualDisplayScreen;
            chkDeviceConnected_CheckedChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            if (chkDeviceConnected.Checked != profile.DualDisplayConnected) return true;
            if (tbDescription.Text != profile.DualDisplayDescription) return true;
            if (ntbReceiptWidth.Value != (double)profile.DualDisplayReceiptPrecentage) return true;
            if (cmbAdvertType.SelectedIndex != (int)profile.DualDisplayType) return true;
            if (tbImageRotatorPath.Text != profile.DualdisplayImagePath) return true;
            if (ntbInterval.Value != (double)profile.DualDisplayImageInterval) return true;
            if (tbWebPage.Text != profile.DualDisplayBrowserUrl) return true;
            if (cmbScreens.SelectedIndex != (int) profile.DualDisplayScreen) return true;
            return false;
        }

        public bool SaveData()
        {
            profile.DualDisplayConnected = chkDeviceConnected.Checked;
            profile.DualDisplayDescription = tbDescription.Text;
            profile.DualDisplayReceiptPrecentage = (decimal)ntbReceiptWidth.Value;
            profile.DualDisplayType = (HardwareProfile.DisplayType)cmbAdvertType.SelectedIndex;
            profile.DualdisplayImagePath = tbImageRotatorPath.Text;
            profile.DualDisplayImageInterval = (int)ntbInterval.Value;
            profile.DualDisplayBrowserUrl = tbWebPage.Text;
            profile.DualDisplayScreen = (HardwareProfile.DualDisplayScreens)cmbScreens.SelectedIndex;

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

        private void btnShowWebPage_Click(object sender, EventArgs e)
        {
            WebBrowser.ShowURL(tbWebPage.Text,false);
        }

        private void btnPathSelect_Click(object sender, EventArgs e)
        {
            FolderItem f = FolderItem.ShowFolderDialog(
                Properties.Resources.ImageDirectory, 
                new FolderItem(tbImageRotatorPath.Text));

            if (f != null)
            {
                tbImageRotatorPath.Text = f.AbsolutePath;
            }
        }

        private void OnIdentifyScreensClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var identifier = new ScreenIdentifier();
            identifier.Identify();
        }

        private void chkDeviceConnected_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDeviceConnected.Checked)
            {
                tbDescription.Enabled = ntbReceiptWidth.Enabled = cmbScreens.Enabled = cmbAdvertType.Enabled
                  = true;

                switch ((HardwareProfile.DisplayType)cmbAdvertType.SelectedIndex)
                {
                    case HardwareProfile.DisplayType.Logo:
                        tbImageRotatorPath.Enabled = btnPathSelect.Enabled = ntbInterval.Enabled = tbWebPage.Enabled = btnShowWebPage.Enabled = false;
                        break;
                    case HardwareProfile.DisplayType.ImageRotator:
                    case HardwareProfile.DisplayType.SyncronizedImageRotator:
                        tbImageRotatorPath.Enabled = btnPathSelect.Enabled = ntbInterval.Enabled = true;
                        tbWebPage.Enabled = btnShowWebPage.Enabled = false;
                        break;
                    case HardwareProfile.DisplayType.WebPage: 
                        tbWebPage.Enabled = btnShowWebPage.Enabled = true;
                        tbImageRotatorPath.Enabled = btnPathSelect.Enabled = ntbInterval.Enabled = false;
                        break;
                }
            }else
                tbDescription.Enabled = ntbReceiptWidth.Enabled = cmbScreens.Enabled = cmbAdvertType.Enabled = tbImageRotatorPath.Enabled 
                    = btnPathSelect.Enabled = ntbInterval.Enabled = tbWebPage.Enabled = btnShowWebPage.Enabled = false;
        }

        private void cmbAdvertType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((HardwareProfile.DisplayType)cmbAdvertType.SelectedIndex)
            {
                case HardwareProfile.DisplayType.Logo:
                    tbImageRotatorPath.Enabled = btnPathSelect.Enabled = ntbInterval.Enabled = tbWebPage.Enabled = btnShowWebPage.Enabled = false;
                    break;
                case HardwareProfile.DisplayType.ImageRotator:
                case HardwareProfile.DisplayType.SyncronizedImageRotator:
                    tbImageRotatorPath.Enabled = btnPathSelect.Enabled = ntbInterval.Enabled = true;
                    tbWebPage.Enabled = btnShowWebPage.Enabled = false;
                    break;
                case HardwareProfile.DisplayType.WebPage: 
                    tbWebPage.Enabled = btnShowWebPage.Enabled = true;
                    tbImageRotatorPath.Enabled = btnPathSelect.Enabled = ntbInterval.Enabled = false;
                    break;
            }
        }
    }
}
