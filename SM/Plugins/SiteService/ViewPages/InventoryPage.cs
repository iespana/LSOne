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
    public partial class InventoryPage : UserControl, ITabView
    {
        private SiteServiceProfile profile;

        public InventoryPage()
        {
            InitializeComponent();

        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new InventoryPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (SiteServiceProfile)internalContext;

            chkUseCentralizedInventoryLookup.Checked = profile.UseInventoryLookup;
        }

        public bool DataIsModified()
        {
            if (chkUseCentralizedInventoryLookup.Checked != profile.UseInventoryLookup) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.UseInventoryLookup = chkUseCentralizedInventoryLookup.Checked;
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
