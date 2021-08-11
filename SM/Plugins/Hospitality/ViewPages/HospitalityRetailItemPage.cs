using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;


namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    internal partial class HospitalityRetailItemPage : UserControl, ITabView, IMultiEditTabExtension
    {
        RetailItem item;
        
        WeakReference owner;
       
        public HospitalityRetailItemPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public HospitalityRetailItemPage()
        {
            InitializeComponent(); 
        }

        public static ITabView CreateInstance(object sender,  TabControl.Tab tab)
        {
            return new HospitalityRetailItemPage((TabControl)sender);
        }

        bool MainRecordDirty()
        {
            if ((int)tbProductionTime.Value != item.ProductionTime) return true;
            return false;
        }


        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)internalContext;

            tbProductionTime.Value = item.ProductionTime;
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
                item.ProductionTime = (int)tbProductionTime.Value;
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

        public void MultiEditCollectData(IDataEntity dataEntity, HashSet<int> changedControlHashes, object param)
        {
            RetailItemMultiEdit itemObject = (RetailItemMultiEdit)dataEntity;

            if (changedControlHashes.Contains(tbProductionTime.GetHashCode()))
            {
                itemObject.ProductionTime = (int)tbProductionTime.Value;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.ProductionTime;
            }
        }

        public bool MultiEditValidateSaveUnknownControls()
        {
            return false;
        }

        public void MultiEditSaveSecondaryRecords(IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            
        }

        public void MultiEditSaveSecondaryRecordsFinalizer()
        {
            
        }

        public void MultiEditRevertUnknownControl(Control control, bool isRevertField, ref bool handled)
        {
            
        }

        #endregion
    }
}
