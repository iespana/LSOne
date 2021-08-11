using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    internal partial class ItemFuelSettings : UserControl, ITabView, IMultiEditTabExtension
    {
        //RecordIdentifier lastSelectedDimension;
        RetailItem item;
        //WeakReference owner;
        WeakReference ValditionPeriodItemEditor;

        public ItemFuelSettings(TabControl owner)
            : this()
        {
            //this.owner = new WeakReference(owner);
        }

        public ItemFuelSettings()
        {
            IPlugin plugin;
            
            InitializeComponent();

            plugin = PluginEntry.Framework.FindImplementor(this, "CanEditValidationPeriod", null);
            ValditionPeriodItemEditor = plugin != null ? new WeakReference(plugin) : null;
        }

        public static ITabView CreateInstance(object sender,  TabControl.Tab tab)
        {
            return new ViewPages.ItemFuelSettings((TabControl)sender);
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)internalContext;

            chkFuelItem.Checked = item.IsFuelItem;
            tbGradeID.Text = item.GradeID;

        }

        public bool DataIsModified()
        {
            if (item.Dirty) return true;

            item.Dirty = item.Dirty | (chkFuelItem.CanFocus != item.IsFuelItem);

            item.Dirty = item.Dirty || (tbGradeID.Text != item.GradeID);

            return item.Dirty;
        }

        public bool SaveData()
        {

            if (item.Dirty)
            {

                item.IsFuelItem = chkFuelItem.Checked;
                item.GradeID = tbGradeID.Text;
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

            if (changedControlHashes.Contains(chkFuelItem.GetHashCode()))
            {
                itemObject.IsFuelItem = chkFuelItem.Checked;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.IsFuelItem;
            }

            if (changedControlHashes.Contains(tbGradeID.GetHashCode()))
            {
                itemObject.GradeID = tbGradeID.Text;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.GradeID;
            }
        }

        public bool MultiEditValidateSaveUnknownControls()
        {
            return false;
        }

        public void MultiEditSaveSecondaryRecords(DataLayer.GenericConnector.Interfaces.IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            
        }

        public void MultiEditRevertUnknownControl(Control control, bool isRevertField, ref bool handled)
        {
            
        }

        public void MultiEditSaveSecondaryRecordsFinalizer()
        {
            
        }



        #endregion














    }
}
