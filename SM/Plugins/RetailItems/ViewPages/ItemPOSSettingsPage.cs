using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    internal partial class ItemPOSSettingsPage : UserControl, ITabViewV2, IMultiEditTabExtension
    {
        //RecordIdentifier lastSelectedDimension;
        RetailItem item;
        //WeakReference owner;
        WeakReference ValditionPeriodItemEditor;

        public ItemPOSSettingsPage(TabControl owner)
            : this()
        {
            //this.owner = new WeakReference(owner);
        }

        public ItemPOSSettingsPage()
        {
            IPlugin plugin;

            InitializeComponent();

            plugin = PluginEntry.Framework.FindImplementor(this, "CanEditValidationPeriod", null);
            ValditionPeriodItemEditor = plugin != null ? new WeakReference(plugin) : null;
            btnEditValidationPeriod.Visible = (ValditionPeriodItemEditor != null);
            cmbValPeriod.Visible = label13.Visible = btnEditValidationPeriod.Visible = !PluginEntry.Framework.IsSiteManagerBasic;
            cmbKeyingInSerialNumber.Enabled = PluginEntry.Framework.CanRunOperation("KeyInSerialNumber");
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.ItemPOSSettingsPage((TabControl)sender);
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)internalContext;

            chkScaleItem.Checked = item.ScaleItem;
            cmbKeyingInPrice.SelectedIndex = (int)item.KeyInPrice;
            cmbKeyingInQuantity.SelectedIndex = (int)item.KeyInQuantity;
            cmbKeyingInSerialNumber.SelectedIndex = (int)item.KeyInSerialNumber;
            chkMustKeyInComment.Checked = item.MustKeyInComment;
            chkZeroPriceValid.Checked = item.ZeroPriceValid;
            chkQtBecomesNegative.Checked = item.QuantityBecomesNegative;
            chkNoDiscount.Checked = item.NoDiscountAllowed;
            chkMustSelectUOM.Checked = item.MustSelectUOM;
            chkReturnable.Checked = item.Returnable;
            chkCanBeSold.Checked = item.CanBeSold;

            DecimalLimit quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
            ntbTareWeight.SetValueWithLimit(item.TareWeight, quantityLimiter);

            dtpDateToBeBlocked.Enabled = true;

            if (item.DateToBeBlocked != Date.Empty)
            {
                dtpDateToBeBlocked.Checked = true;
                dtpDateToBeBlocked.Value = item.DateToBeBlocked.DateTime;
            }
            else
            {
                dtpDateToBeBlocked.Checked = false;
            }

            item.DateToActivateItem.ToDateControl(dtpIssueDate);
            if (item.ValidationPeriodID != null)
            {
                cmbValPeriod.SelectedData = new DataEntity(item.ValidationPeriodID, item.ValidationPeriodDescription);
            }

        }

        public bool DataIsModified()
        {
            Date selectedDate;

            if (item.Dirty) return true;

            item.Dirty = item.Dirty | (chkScaleItem.Checked != item.ScaleItem);
            item.Dirty = item.Dirty | ((decimal)ntbTareWeight.Value != item.TareWeight);
            item.Dirty = item.Dirty | (cmbKeyingInPrice.SelectedIndex != (int)item.KeyInPrice);
            item.Dirty = item.Dirty | (cmbKeyingInQuantity.SelectedIndex != (int)item.KeyInQuantity);
            item.Dirty = item.Dirty | (cmbKeyingInSerialNumber.SelectedIndex != (int)item.KeyInSerialNumber);
            item.Dirty = item.Dirty | (chkMustKeyInComment.Checked != item.MustKeyInComment);
            item.Dirty = item.Dirty | (chkZeroPriceValid.Checked != item.ZeroPriceValid);
            item.Dirty = item.Dirty | (chkQtBecomesNegative.Checked != item.QuantityBecomesNegative);
            item.Dirty = item.Dirty | (chkNoDiscount.Checked != item.NoDiscountAllowed);
            item.Dirty = item.Dirty | (chkMustSelectUOM.Checked != item.MustSelectUOM);
            item.Dirty = item.Dirty | (chkReturnable.Checked != item.Returnable);
            item.Dirty = item.Dirty | (chkCanBeSold.Checked != item.CanBeSold);

            selectedDate = dtpDateToBeBlocked.Checked ? new Date(dtpDateToBeBlocked.Value) : Date.Empty;

            item.Dirty = item.Dirty || (selectedDate != item.DateToBeBlocked);

            selectedDate = dtpIssueDate.Checked ? new Date(dtpIssueDate.Value) : Date.Empty;

            item.Dirty = item.Dirty || (selectedDate != item.DateToActivateItem);

            if (cmbValPeriod.SelectedData.ID != item.ValidationPeriodID)
            {
                item.Dirty = true;
            }

            return item.Dirty;
        }

        public bool SaveData()
        {
            Date selectedDate;

            if (item.Dirty)
            {
                item.ScaleItem = chkScaleItem.Checked;
                item.TareWeight = Convert.ToInt32(ntbTareWeight.Value);
                item.KeyInPrice = (RetailItem.KeyInPriceEnum)cmbKeyingInPrice.SelectedIndex;
                item.KeyInQuantity = (RetailItem.KeyInQuantityEnum)cmbKeyingInQuantity.SelectedIndex;
                item.KeyInSerialNumber = (RetailItem.KeyInSerialNumberEnum)cmbKeyingInSerialNumber.SelectedIndex;
                item.MustKeyInComment = chkMustKeyInComment.Checked;
                item.ZeroPriceValid = chkZeroPriceValid.Checked;
                item.QuantityBecomesNegative = chkQtBecomesNegative.Checked;
                item.NoDiscountAllowed = chkNoDiscount.Checked;
                item.MustSelectUOM = chkMustSelectUOM.Checked;
                item.Returnable = chkReturnable.Checked;
                item.CanBeSold = chkCanBeSold.Checked;
                selectedDate = dtpDateToBeBlocked.Checked ? new Date(dtpDateToBeBlocked.Value) : Date.Empty;

                item.DateToBeBlocked = selectedDate;

                selectedDate = dtpIssueDate.Checked ? new Date(dtpIssueDate.Value) : Date.Empty;

                item.DateToActivateItem = selectedDate;

                item.ValidationPeriodID = (string)cmbValPeriod.SelectedData.ID;
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

        private void cmbValPeriod_RequestData(object sender, EventArgs e)
        {
            cmbValPeriod.SetData(Providers.DiscountPeriodData.GetList(PluginEntry.DataModel), null);
        }

        #endregion

        private void btnEditValidationPeriod_Click(object sender, EventArgs e)
        {
            if (ValditionPeriodItemEditor.IsAlive)
            {
                ((IPlugin)ValditionPeriodItemEditor.Target).Message(this, "CanEditValidationPeriod", cmbValPeriod.SelectedData.ID);
            }
        }

        private void cmbValPeriod_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {

        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            if (internalContext != null)
            {
                cmbValPeriod.SetSelectionToNoChange();
            }
        }

        public void MultiEditCollectData(IDataEntity dataEntity, HashSet<int> changedControlHashes, object param)
        {
            RetailItemMultiEdit itemObject = (RetailItemMultiEdit)dataEntity;

            if (changedControlHashes.Contains(chkScaleItem.GetHashCode()))
            {
                itemObject.ScaleItem = chkScaleItem.Checked;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.ScaleItem;
            }

            if (changedControlHashes.Contains(chkMustKeyInComment.GetHashCode()))
            {
                itemObject.MustKeyInComment = chkMustKeyInComment.Checked;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.MustKeyInComment;
            }

            if (changedControlHashes.Contains(chkZeroPriceValid.GetHashCode()))
            {
                itemObject.ZeroPriceValid = chkZeroPriceValid.Checked;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.ZeroPriceValid;
            }

            if (changedControlHashes.Contains(chkQtBecomesNegative.GetHashCode()))
            {
                itemObject.QuantityBecomesNegative = chkQtBecomesNegative.Checked;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.QuantityBecomesNegative;
            }

            if (changedControlHashes.Contains(chkNoDiscount.GetHashCode()))
            {
                itemObject.NoDiscountAllowed = chkNoDiscount.Checked;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.NoDiscountAllowed;
            }

            if (changedControlHashes.Contains(chkMustSelectUOM.GetHashCode()))
            {
                itemObject.MustSelectUOM = chkMustSelectUOM.Checked;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.MustSelectUOM;
            }

            if (changedControlHashes.Contains(cmbKeyingInPrice.GetHashCode()))
            {
                itemObject.KeyInPrice = (RetailItem.KeyInPriceEnum)cmbKeyingInPrice.SelectedIndex;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.KeyInPrice;
            }

            if (changedControlHashes.Contains(cmbKeyingInQuantity.GetHashCode()))
            {
                itemObject.KeyInQuantity = (RetailItem.KeyInQuantityEnum)cmbKeyingInQuantity.SelectedIndex;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.KeyInQuantity;
            }

            if (changedControlHashes.Contains(cmbKeyingInSerialNumber.GetHashCode()))
            {
                itemObject.KeyInSerialNumber = (RetailItem.KeyInSerialNumberEnum)cmbKeyingInSerialNumber.SelectedIndex;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.KeyInSerialNumber;
            }

            if (changedControlHashes.Contains(dtpDateToBeBlocked.GetHashCode()))
            {
                itemObject.DateToBeBlocked = dtpDateToBeBlocked.Checked ? new Date(dtpDateToBeBlocked.Value) : Date.Empty;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.DateToBeBlocked;
            }

            if (changedControlHashes.Contains(dtpIssueDate.GetHashCode()))
            {
                itemObject.DateToActivateItem = dtpIssueDate.Checked ? new Date(dtpIssueDate.Value) : Date.Empty;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.DateToActivateItem;
            }

            if (changedControlHashes.Contains(cmbValPeriod.GetHashCode()))
            {
                itemObject.ValidationPeriodID = (string)cmbValPeriod.SelectedData.ID;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.ValidationPeriodID;
            }

            if (changedControlHashes.Contains(ntbTareWeight.GetHashCode()))
            {
                itemObject.TareWeight = Convert.ToInt32(ntbTareWeight.Value);
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.TareWeight;
            }

            if (changedControlHashes.Contains(chkReturnable.GetHashCode()))
            {
                itemObject.Returnable = chkReturnable.Checked;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.Returnable;
            }

            if (changedControlHashes.Contains(chkCanBeSold.GetHashCode()))
            {
                itemObject.CanBeSold = chkCanBeSold.Checked;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.CanBeSold;
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

        private void chkScaleItem_CheckedChanged(object sender, EventArgs e)
        {
            ntbTareWeight.Enabled = chkScaleItem.Checked;
        }
    }
}
