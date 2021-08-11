using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityTypeSplitBillTransferLinesPage : UserControl, ITabView
    {
        private HospitalityType hospitalityType;

        public HospitalityTypeSplitBillTransferLinesPage()
        {
            InitializeComponent();
            btnsEditAddSplitBillLookup.Visible = btnsEditAddTransferLinesLookup.Visible = PluginEntry.DataModel.HasPermission(Permission.ManageMenuGroups);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityTypeSplitBillTransferLinesPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            hospitalityType = (HospitalityType)internalContext;

            cmbSplitBillLookupId.SelectedData = Providers.PosLookupData.Get(PluginEntry.DataModel, hospitalityType.SplitBillLookupID) ?? new DataEntity("", "");
            cmbTransferLinesLookupId.SelectedData = Providers.PosLookupData.Get(PluginEntry.DataModel, hospitalityType.TransferLinesLookupID) ?? new DataEntity("", "");
            cmbGuestButtons.SelectedIndex = (int)hospitalityType.GuestButtons;
            ntbMaxGuestButtonsShown.Value = hospitalityType.MaxGuestButtonsShown;
            ntbMaxGuestsPerTable.Value = hospitalityType.MaxGuestsPerTable;
            ntbMaxNoOfSplits.Value = hospitalityType.MaxNumberOfSplits;
            chkSelectGuestOnSplitting.Checked = hospitalityType.SelectGuestOnSplitting;
            cmbCombineSplitLinesAction.SelectedIndex = (int)hospitalityType.CombineSplitLinesAction;

            btnsEditAddTransferLinesLookup.EditButtonEnabled = cmbTransferLinesLookupId.SelectedData.ID != "";
            btnsEditAddSplitBillLookup.EditButtonEnabled = cmbSplitBillLookupId.SelectedData.ID != "";
        }

        public bool DataIsModified()
        {
            if (cmbSplitBillLookupId.SelectedData != null && cmbSplitBillLookupId.SelectedData.ID != hospitalityType.SplitBillLookupID) return true;            
            if (cmbTransferLinesLookupId.SelectedData != null && cmbTransferLinesLookupId.SelectedData.ID != hospitalityType.TransferLinesLookupID) return true;
            if (cmbGuestButtons.SelectedIndex != (int)hospitalityType.GuestButtons) return true;
            if (ntbMaxGuestButtonsShown.Value != (double)hospitalityType.MaxGuestButtonsShown) return true;
            if (ntbMaxGuestsPerTable.Value != (double)hospitalityType.MaxGuestsPerTable) return true;
            if (ntbMaxNoOfSplits.Value != hospitalityType.MaxNumberOfSplits) return true;
            if (chkSelectGuestOnSplitting.Checked != hospitalityType.SelectGuestOnSplitting) return true;
            if (cmbCombineSplitLinesAction.SelectedIndex != (int)hospitalityType.CombineSplitLinesAction) return true;

            return false;
        }

        public bool SaveData()
        {
            hospitalityType.SplitBillLookupID = cmbSplitBillLookupId.SelectedData != null ? cmbSplitBillLookupId.SelectedData.ID : "";
            hospitalityType.TransferLinesLookupID = cmbTransferLinesLookupId.SelectedData != null ? cmbTransferLinesLookupId.SelectedData.ID : "";
            hospitalityType.GuestButtons = (HospitalityType.GuestButtonsEnum)cmbGuestButtons.SelectedIndex;
            hospitalityType.MaxGuestButtonsShown = (int)ntbMaxGuestButtonsShown.Value;
            hospitalityType.MaxGuestsPerTable = (int)ntbMaxGuestsPerTable.Value;
            hospitalityType.MaxNumberOfSplits = (int) ntbMaxNoOfSplits.Value;
            hospitalityType.SelectGuestOnSplitting = chkSelectGuestOnSplitting.Checked;
            hospitalityType.CombineSplitLinesAction = (HospitalityType.CombineSplitLinesActionEnum)cmbCombineSplitLinesAction.SelectedIndex;

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

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void cmbSplitBillLookupId_RequestData(object sender, EventArgs e)
        {
            cmbSplitBillLookupId.SetData(Providers.PosLookupData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbTransferLinesLookupId_RequestData(object sender, EventArgs e)
        {
            cmbTransferLinesLookupId.SetData(Providers.PosLookupData.GetList(PluginEntry.DataModel), null);
        }

        private void btnAddSplitBillLookup_Click(object sender, EventArgs e)
        {
            PluginOperations.NewPosLookup();
        }

        private void btnAddTransferLinesLookup_Click(object sender, EventArgs e)
        {
            PluginOperations.NewPosLookup();
        }

        private void cmbSplitBillLookupId_SelectedDataChanged(object sender, EventArgs e)
        {
            btnsEditAddSplitBillLookup.EditButtonEnabled = cmbSplitBillLookupId.SelectedData.ID != "";
        }

        private void btnsEditAddSplitBillLookup_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowPosLookup(cmbSplitBillLookupId.SelectedData.ID);
        }

        private void cmbTransferLinesLookupId_SelectedDataChanged(object sender, EventArgs e)
        {
            btnsEditAddTransferLinesLookup.EditButtonEnabled = cmbTransferLinesLookupId.SelectedData.ID != "";
        }

        private void btnsEditAddTransferLinesLookup_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowPosLookup(cmbTransferLinesLookupId.SelectedData.ID);
        }

    }
}
