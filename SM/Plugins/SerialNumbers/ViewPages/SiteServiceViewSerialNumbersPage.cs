using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.SerialNumbers.ViewPages
{
    public partial class SiteServiceViewSerialNumbersPage : UserControl, ITabView
    {
        private SiteServiceProfile profile;

        public SiteServiceViewSerialNumbersPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new SiteServiceViewSerialNumbersPage();
        }

        public bool DataIsModified()
        {
            if (chkUseSerialNumbers.Checked != profile.UseSerialNumbers) return true;
            
            return false;
        }

        public void GetAuditDescriptors(List<ViewCore.AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, Utilities.DataTypes.RecordIdentifier context, object internalContext)
        {
            profile = (SiteServiceProfile)internalContext;
            chkUseSerialNumbers.Checked = profile.UseSerialNumbers;
        }

        public void OnClose()
        {
        }

        public void OnDataChanged(ViewCore.Enums.DataEntityChangeType changeHint, string objectName, Utilities.DataTypes.RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            profile.UseSerialNumbers = chkUseSerialNumbers.Checked;
            return true;
        }

        public void SaveUserInterface()
        {
        }
        
    }
}
