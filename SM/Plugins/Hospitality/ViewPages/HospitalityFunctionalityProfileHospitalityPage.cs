using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityFunctionalityProfileHospitalityPage : UserControl, ITabView
    {
        private FunctionalityProfile functionalityProfile;

        public HospitalityFunctionalityProfileHospitalityPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityFunctionalityProfileHospitalityPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            functionalityProfile = (FunctionalityProfile)internalContext;
            chkIsHospitalityProfile.Checked = functionalityProfile.IsHospitality;
            chkSkipTableView.Checked = functionalityProfile.SkipHospitalityTableView;
            chkItemChangesAfterSplit.Checked = functionalityProfile.AllowItemChangesAfterSplitBill;
            SetSkipTableViewAbility(chkIsHospitalityProfile.Checked);

        }

        public bool DataIsModified()
        {
            if (chkIsHospitalityProfile.Checked != functionalityProfile.IsHospitality) return true;
            if (chkSkipTableView.Checked != functionalityProfile.SkipHospitalityTableView) return true;
            if (chkItemChangesAfterSplit.Checked != functionalityProfile.AllowItemChangesAfterSplitBill) return true;

            return false;
        }

        public bool SaveData()
        {
            functionalityProfile.IsHospitality = chkIsHospitalityProfile.Checked;
            functionalityProfile.SkipHospitalityTableView = chkSkipTableView.Checked;
            functionalityProfile.AllowItemChangesAfterSplitBill = chkItemChangesAfterSplit.Checked;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //throw new NotImplementedException();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            //throw new NotImplementedException();
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void SetSkipTableViewAbility(bool enabled)
        {
            label2.Enabled = enabled;
            chkSkipTableView.Enabled = enabled;
        }

        private void chkIsHospitalityProfile_Click(object sender, EventArgs e)
        {
            SetSkipTableViewAbility(chkIsHospitalityProfile.Checked);
        }
    }
}
