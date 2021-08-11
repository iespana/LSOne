using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    public partial class CentralSuspensionPage : UserControl, ITabView
    {
        private SiteServiceProfile profile;

        public CentralSuspensionPage()
        {
            InitializeComponent();
            
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new CentralSuspensionPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (SiteServiceProfile)internalContext;

            chkUseCentralSuspension.Checked = profile.UseCentralSuspensions;
            chkUserConfirmation.Checked = profile.UserConfirmation;
            chkUseCentralSuspension_CheckedChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            if (chkUseCentralSuspension.Checked != profile.UseCentralSuspensions) return true;
            if (chkUserConfirmation.Checked != profile.UserConfirmation) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.UseCentralSuspensions = chkUseCentralSuspension.Checked;
            profile.UserConfirmation = chkUserConfirmation.Checked;
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


        private void chkUseCentralSuspension_CheckedChanged(object sender, EventArgs e)
        {
            chkUserConfirmation.Checked = chkUserConfirmation.Enabled = chkUseCentralSuspension.CheckState == CheckState.Checked;
        }

    }
}
