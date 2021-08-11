using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.CentralSuspensions.ViewPages
{
    public partial class StoreSuspendTransactionsPage : UserControl, ITabView
    {
        Store store;

        public StoreSuspendTransactionsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StoreSuspendTransactionsPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            store = (Store)internalContext;
            chkAllowEOD.Checked = store.AllowEOD;
        }

        public bool DataIsModified()
        {
            if (chkAllowEOD.Checked != store.AllowEOD) return true;      
            return false;
        }

        public bool SaveData()
        {
            store.AllowEOD = chkAllowEOD.Checked;

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
