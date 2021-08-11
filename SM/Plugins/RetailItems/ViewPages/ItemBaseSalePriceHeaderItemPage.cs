using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    internal partial class ItemBaseSalePriceHeaderItemPage : UserControl, ITabView
    {

        public ItemBaseSalePriceHeaderItemPage()
        {
            InitializeComponent(); 
        }

        public static ITabView CreateInstance(object sender,  TabControl.Tab tab)
        {
            return new ViewPages.ItemBaseSalePriceHeaderItemPage();
        }
        
        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return false;
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
        

        private void lblMessage_Click(object sender, EventArgs e)
        {

        }
    }
}
