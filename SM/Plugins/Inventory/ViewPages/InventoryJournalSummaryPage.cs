using System;
using System.Collections.Generic;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using System.Windows.Forms;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class InventoryJournalSummaryPage :  UserControl, ITabView
    {
        private InventoryAdjustment journal;
        private RecordIdentifier journalID;

        public InventoryJournalSummaryPage(TabControl owner)
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new InventoryJournalSummaryPage((TabControl)sender);
        }

        #region ITabView interface
        public bool DataIsModified()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //nothing to do; screen is in read-only mode
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (internalContext != null)
            {
                journal = (InventoryAdjustment)internalContext;
                journalID = context;

                tbStatus.Text = journal.Posted == DataLayer.BusinessObjects.Enums.InventoryJournalStatus.Posted
                                    ? Resources.SearchBar_Value_Posted
                                    : Resources.SearchBar_Value_Active;
                tbID.Text = (string)journal.ID;
                tbDescription.Text = journal.Text;     
                cmbStore.SelectedData = new DataEntity(journal.StoreId, journal.StoreName);
            }
        }

        public void OnClose()
        {
            //nothing to do; screen is in read-only mode
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            //nothing to do; screen is in read-only mode
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveUserInterface()
        {
            //nothing to do; screen is in read-only mode
        }
#endregion
    }
}
