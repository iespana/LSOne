using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Functionality
{
    public partial class FunctionalProfileStaffPage : UserControl, ITabView
    {

        private FunctionalityProfile functionalityProfile;

        public FunctionalProfileStaffPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new FunctionalProfileStaffPage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            functionalityProfile = (FunctionalityProfile)internalContext;

            chkMustKeyInPriceIfZero.Checked = functionalityProfile.MustKeyInPriceIfZero;
            chkDisplayEmployeeListAtlogin.Checked = functionalityProfile.ShowStaffListAtLogon;
            chkClearUser.Checked = functionalityProfile.ClearUserBetweenLogins;
            chkOnlyDisplayStoreEmployees.Checked = functionalityProfile.LimitStaffListToStore;
            cmbSalesPerson.SelectedIndex = (int)functionalityProfile.SalesPersonPrompt;

            chkDisplayEmployeeListAtlogin_CheckedChanged(null, EventArgs.Empty);
        }

        #region ITabPanel Members

        public bool DataIsModified()
        {
            if (chkMustKeyInPriceIfZero.Checked != functionalityProfile.MustKeyInPriceIfZero) return true;
            if (chkDisplayEmployeeListAtlogin.Checked != functionalityProfile.ShowStaffListAtLogon) return true;
            if (chkClearUser.Checked != functionalityProfile.ClearUserBetweenLogins) return true;
            if (chkOnlyDisplayStoreEmployees.Checked != functionalityProfile.LimitStaffListToStore) return true;
            if (cmbSalesPerson.SelectedIndex != (int)functionalityProfile.SalesPersonPrompt) return true;

            return false;
        }

        public bool SaveData()
        {
            functionalityProfile.MustKeyInPriceIfZero = chkMustKeyInPriceIfZero.Checked;
            functionalityProfile.ShowStaffListAtLogon = chkDisplayEmployeeListAtlogin.Checked;
            functionalityProfile.ClearUserBetweenLogins = chkClearUser.Checked;
            functionalityProfile.LimitStaffListToStore = chkOnlyDisplayStoreEmployees.Checked;
            functionalityProfile.SalesPersonPrompt = (SalesPersonPrompt)cmbSalesPerson.SelectedIndex;

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

        private void chkDisplayEmployeeListAtlogin_CheckedChanged(object sender, EventArgs e)
        {
            chkOnlyDisplayStoreEmployees.Enabled = chkDisplayEmployeeListAtlogin.Checked;
        }
    }
}
