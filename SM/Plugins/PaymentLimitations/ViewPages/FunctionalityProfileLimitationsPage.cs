using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.PaymentLimitations.ViewPages
{
    public partial class FunctionalityProfileLimitationsPage : UserControl, ITabView
    {
        private FunctionalityProfile functionalityProfile;

        public FunctionalityProfileLimitationsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new FunctionalityProfileLimitationsPage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            functionalityProfile = (FunctionalityProfile)internalContext;
            cmbDisplayItems.SelectedIndex = (int)functionalityProfile.DialogLimitationDisplayType;
            chkDisplayTotalsInPOS.Checked = functionalityProfile.DisplayLimitationsTotalsInPOS;
        }

        #region ITabPanel Members

        public bool DataIsModified()
        {
            if (cmbDisplayItems.SelectedIndex != (int)functionalityProfile.DialogLimitationDisplayType) return true;
            if (chkDisplayTotalsInPOS.Checked != functionalityProfile.DisplayLimitationsTotalsInPOS) return true;
            return false;
        }

        public bool SaveData()
        {
            functionalityProfile.DialogLimitationDisplayType = (FunctionalityProfile.LimitationDisplayType)cmbDisplayItems.SelectedIndex;
            functionalityProfile.DisplayLimitationsTotalsInPOS = chkDisplayTotalsInPOS.Checked;
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

    }
}
