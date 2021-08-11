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
    internal partial class ItemAdditionalInfo : UserControl, ITabViewV2, IMultiEditTabExtension
    {
        RetailItem item;
        
        WeakReference owner;
       
        public ItemAdditionalInfo(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public ItemAdditionalInfo()
        {
            InitializeComponent(); 
        }

        public static ITabView CreateInstance(object sender,  TabControl.Tab tab)
        {
            return new ViewPages.ItemAdditionalInfo((TabControl)sender);
        }

        bool MainRecordDirty()
        {
            if (tbSearchAlias.Text != item.NameAlias) return true;
            if (tbSearchKeywords.Text != item.SearchKeywords) return true;
            if (tbNotes.Text != item.ExtendedDescription) return true;
            return false;
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (isRevert)
            {
                // If we are changing the items tax group we want to trigger update price question
                
            }

            item = (RetailItem)internalContext;

            tbSearchAlias.Text = item.NameAlias;
            tbSearchKeywords.Text = item.SearchKeywords;
            tbNotes.Text = item.ExtendedDescription;
        }

        public bool DataIsModified()
        {
            if (MainRecordDirty())
            {
                item.Dirty = true;
            }     

            return item.Dirty;
        }

        public bool SaveData()
        {
            if (item.Dirty)
            {
                item.NameAlias = tbSearchAlias.Text;
                item.SearchKeywords = tbSearchKeywords.Text;
                item.ExtendedDescription = tbNotes.Text;
            }
            
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

        private void DataFormater(object sender, DropDownFormatDataArgs e)
        {

        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            
        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)internalContext;
        }

        public void MultiEditCollectData(IDataEntity dataEntity, HashSet<int> changedControlHashes, object param)
        {
            RetailItemMultiEdit itemObject = (RetailItemMultiEdit)dataEntity;

            if (changedControlHashes.Contains(tbNotes.GetHashCode()))
            {
                itemObject.ExtendedDescription = tbNotes.Text;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.ExtendedDescription;
            }

            if (changedControlHashes.Contains(tbSearchAlias.GetHashCode()))
            {
                itemObject.NameAlias = tbSearchAlias.Text;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.NameAlias;
            }

            if (changedControlHashes.Contains(tbSearchKeywords.GetHashCode()))
            {
                itemObject.SearchKeywords = tbSearchKeywords.Text;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SearchKeywords;
            }
        }

        public bool MultiEditValidateSaveUnknownControls()
        {
            return false;
        }

        public void MultiEditSaveSecondaryRecords(IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            
        }

        public void MultiEditRevertUnknownControl(Control control, bool isRevertField, ref bool handled)
        {
            
        }

        public void MultiEditSaveSecondaryRecordsFinalizer()
        {

        }
    }
}
