using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    public partial class HardwareProfileKdsPage : UserControl, ITabView
    {
        private HardwareProfile profile;

        public HardwareProfileKdsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HardwareProfileKdsPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            chkUseKitchenDisplay.Checked = profile.UseKitchenDisplay;
        }

        public bool DataIsModified()
        {
            if (chkUseKitchenDisplay.Checked != profile.UseKitchenDisplay) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.UseKitchenDisplay = chkUseKitchenDisplay.Checked;

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
