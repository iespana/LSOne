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
    public partial class HardwareProfileCardServicePage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfileCardServicePage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileCardServicePage();
        }

        protected override void OnLoad(EventArgs e)
        {
            
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkServiceActive.Checked = profile.EftConnected;
            chkLsPayActive.Checked = profile.LsPayConnected;
            tbCreditCardServer.Text = profile.EftServerName;
            tbDescription.Text = profile.EftDescription;
            ntbServerPort.Value = (double)profile.EftServerPort;
            tbCompanyID.Text = profile.EftCompanyID;
            tbUserName.Text = profile.EftUserID;
            tbPassword.Text = profile.EftPassword;
            chkUpdateBatchAtShiftChange.Checked = profile.EftBatchIncrementAtEOS;

            tbMerchantAccount.Text = profile.EftMerchantAccount;
            tbMerchantKey.Text = profile.EftMerchantKey;
            tbCustomField1.Text = profile.EftCustomField1;
            tbCustomField2.Text = profile.EftCustomField2;

            chkServiceActive_CheckedChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            if (chkServiceActive.Checked != profile.EftConnected) return true;
            if (chkLsPayActive.Checked != profile.LsPayConnected) return true;
            if (tbCreditCardServer.Text != profile.EftServerName) return true;
            if (tbDescription.Text != profile.EftDescription) return true;
            if (ntbServerPort.Value != (double)profile.EftServerPort) return true;
            if (tbCompanyID.Text != profile.EftCompanyID) return true;
            if (tbUserName.Text != profile.EftUserID) return true;
            if (tbPassword.Text != profile.EftPassword) return true;
            if (tbMerchantAccount.Text != profile.EftMerchantAccount) return true;
            if (tbMerchantKey.Text != profile.EftMerchantKey) return true;
            if (tbCustomField1.Text != profile.EftCustomField1) return true;
            if (tbCustomField2.Text != profile.EftCustomField2) return true;
            if (chkUpdateBatchAtShiftChange.Checked != profile.EftBatchIncrementAtEOS) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.EftConnected = chkServiceActive.Checked;
            profile.LsPayConnected = chkLsPayActive.Checked;
            profile.EftServerName = tbCreditCardServer.Text;
            profile.EftDescription = tbDescription.Text;
            profile.EftServerPort = (int)ntbServerPort.Value;
            profile.EftCompanyID = tbCompanyID.Text;
            profile.EftUserID = tbUserName.Text;
            profile.EftPassword = tbPassword.Text;
            profile.EftMerchantAccount = tbMerchantAccount.Text;
            profile.EftMerchantKey = tbMerchantKey.Text;
            profile.EftCustomField1 = tbCustomField1.Text;
            profile.EftCustomField2 = tbCustomField2.Text;
            profile.EftBatchIncrementAtEOS = chkUpdateBatchAtShiftChange.Checked;

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

        private void chkServiceActive_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkServiceActive.Checked)
            {
                chkLsPayActive.Checked = false;
            }

            EnableDisableFields();
        }

        private void chkLsPayActive_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableFields();
        }

        private void EnableDisableFields()
        {
            chkLsPayActive.Enabled =
                tbCreditCardServer.Enabled =
                tbDescription.Enabled =
                ntbServerPort.Enabled =
                tbCompanyID.Enabled =
                chkServiceActive.Checked;

            tbCompanyID.Enabled =
                tbUserName.Enabled =
                tbPassword.Enabled =
                chkUpdateBatchAtShiftChange.Enabled =
                tbMerchantAccount.Enabled =
                tbMerchantKey.Enabled =
                tbCustomField1.Enabled =
                tbCustomField2.Enabled =
                (chkServiceActive.Checked && !chkLsPayActive.Checked);
        }
    }
}
