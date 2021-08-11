using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    public partial class CentralReturnsPage : UserControl, ITabView
    {
        private SiteServiceProfile profile;

        public CentralReturnsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new CentralReturnsPage();
        }

        public bool DataIsModified()
        {
            if (chkUseCentralReturns.Checked != profile.UseCentralReturns) return true;
            
            return false;
        }

        public void GetAuditDescriptors(List<ViewCore.AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, Utilities.DataTypes.RecordIdentifier context, object internalContext)
        {
            profile = (SiteServiceProfile)internalContext;
            chkUseCentralReturns.Checked = profile.UseCentralReturns;
        }

        public void OnClose()
        {
        }

        public void OnDataChanged(ViewCore.Enums.DataEntityChangeType changeHint, string objectName, Utilities.DataTypes.RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            profile.UseCentralReturns = chkUseCentralReturns.Checked;
            return true;
        }

        public void SaveUserInterface()
        {
        }
        
    }
}
