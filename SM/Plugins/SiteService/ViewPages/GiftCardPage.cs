using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    public partial class GiftCardPage : UserControl, ITabView
    {
        private SiteServiceProfile profile;

        public GiftCardPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new GiftCardPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (SiteServiceProfile)internalContext;

            chkUseGiftcard.Checked = profile.UseGiftCards;
            cmbGiftCardOption.SelectedIndex = (int)profile.IssueGiftCardOption;
            cmbRefillableGiftcard.SelectedIndex = (int) profile.GiftCardRefillSetting;
            ntbMaximumGiftCardAmount.Value = (double) profile.MaximumGiftCardAmount;
            chkUseGiftcard_CheckedChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            if (chkUseGiftcard.Checked != profile.UseGiftCards) return true;
            if (cmbGiftCardOption.SelectedIndex != (int)profile.IssueGiftCardOption) return true;
            if (cmbRefillableGiftcard.SelectedIndex != (int)profile.GiftCardRefillSetting) return true;
            if (Math.Abs(ntbMaximumGiftCardAmount.Value - (double) profile.MaximumGiftCardAmount) > 0.01) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.UseGiftCards = chkUseGiftcard.Checked;
            profile.IssueGiftCardOption = (SiteServiceProfile.IssueGiftCardOptionEnum)cmbGiftCardOption.SelectedIndex;
            profile.GiftCardRefillSetting = (SiteServiceProfile.GiftCardRefillSettingEnum)cmbRefillableGiftcard.SelectedIndex;
            profile.MaximumGiftCardAmount = (decimal) ntbMaximumGiftCardAmount.Value;
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

        private void chkUseGiftcard_CheckedChanged(object sender, EventArgs e)
        {
            cmbGiftCardOption.Enabled = chkUseGiftcard.Checked;
            cmbRefillableGiftcard.Enabled = chkUseGiftcard.Checked;
            ntbMaximumGiftCardAmount.Enabled = chkUseGiftcard.Checked;
        }
    }
}
