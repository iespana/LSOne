using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Functionality
{
    public partial class FunctionalProfileStartOfDay : UserControl, ITabView
    {
        private FunctionalityProfile functionalityProfile;

        public FunctionalProfileStartOfDay()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new FunctionalProfileStartOfDay();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            functionalityProfile = (FunctionalityProfile)internalContext;
            chkUseStartOfDay.Checked = functionalityProfile.UseStartOfDay;
        }

        #region ITabPanel Members

        public bool DataIsModified()
        {
            return chkUseStartOfDay.Checked != functionalityProfile.UseStartOfDay;
        }

        public bool SaveData()
        {
            functionalityProfile.UseStartOfDay  = chkUseStartOfDay.Checked;
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
