using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityTypeGeneralPage : UserControl, ITabView
    {
        WeakReference touchButtonsEditor;
        HospitalityType hospitalityType;
        bool accessToOtherRestaurantDirty;
        bool overviewDirty;
        bool suspendEvents;

        public HospitalityTypeGeneralPage()
        {
            InitializeComponent();

            accessToOtherRestaurantDirty = false;
            overviewDirty = false;

            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "CanEditLayouts", null);
            touchButtonsEditor = (plugin != null) ? new WeakReference(plugin) : null;

            btnEditTouchButtons.Visible = (touchButtonsEditor != null);
            btnsEditAddTopPosMenu.Visible = btnsEditAddPosLogonMenu.Visible = PluginEntry.DataModel.HasPermission(Permission.ViewPosMenus) && PluginEntry.DataModel.HasPermission(Permission.EditPosMenus);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityTypeGeneralPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            suspendEvents = true;

            hospitalityType = (HospitalityType)internalContext;

            cmbOverview.SelectedIndex = (int)hospitalityType.Overview;
            cmbAccessToOtherRestaurant.SelectedData = String.IsNullOrEmpty((string)hospitalityType.AccessToOtherRestaurant) ? null : Providers.StoreData.Get(PluginEntry.DataModel, (string)hospitalityType.AccessToOtherRestaurant);
            cmbStaffTakeoverInTransaction.SelectedIndex = (int)hospitalityType.StaffTakeOverInTrans;
            cmbManagerTakeoverInTransaction.SelectedIndex = (int)hospitalityType.ManagerTakeOverInTrans;
            cmbTouchButtonsID.SelectedData = Providers.TouchLayoutData.Get(PluginEntry.DataModel, hospitalityType.LayoutID) ?? new DataEntity("", "");            
            cmbTopPosMenuID.SelectedData = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, hospitalityType.TopPosMenuID) ?? new DataEntity("", "");
            cmbPosLogonMenuID.SelectedData = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, hospitalityType.PosLogonMenuID) ?? new DataEntity("", "");
            chkStayInPosAfterTransaction.Checked = hospitalityType.StayInPosAfterTrans;
            chkSendVoidedItemsToStation.Checked = hospitalityType.SendVoidedItemsToStation;
            chkSendTransfersToStation.Checked = hospitalityType.SendTransfersToStation;
            chkSendOrderNoToStation.Checked = hospitalityType.SendOrderNoToStation;
            chkSendSuspensionsToStation.Checked = hospitalityType.SendSuspensionsToStation;
            cmbStationPrinting.SelectedIndex = (int)hospitalityType.StationPrinting;
            chkPrintTrainingTransaction.Checked = hospitalityType.PrintTrainingTransactions;
            chkDefaultType.Checked = hospitalityType.DefaultType;

            cmbPosLogonMenuID_SelectedDataChanged(this, EventArgs.Empty);
            cmbTopPosMenuID_SelectedDataChanged(this, EventArgs.Empty);

            suspendEvents = false;
        }

        public bool DataIsModified()
        {
            return overviewDirty
                   || cmbOverview.SelectedIndex != (int) hospitalityType.Overview
                   || accessToOtherRestaurantDirty
                   || (cmbAccessToOtherRestaurant.SelectedData != null && cmbAccessToOtherRestaurant.SelectedData.ID.ToString() != (string) hospitalityType.AccessToOtherRestaurant)
                   || cmbStaffTakeoverInTransaction.SelectedIndex != (int) hospitalityType.StaffTakeOverInTrans
                   || cmbManagerTakeoverInTransaction.SelectedIndex != (int) hospitalityType.ManagerTakeOverInTrans
                   || cmbTouchButtonsID.SelectedData != null && cmbTouchButtonsID.SelectedData.ID != hospitalityType.LayoutID
                   || cmbTopPosMenuID.SelectedData != null && cmbTopPosMenuID.SelectedData.ID != hospitalityType.TopPosMenuID
                   || cmbPosLogonMenuID.SelectedData != null && cmbPosLogonMenuID.SelectedData.ID != hospitalityType.PosLogonMenuID
                   || chkStayInPosAfterTransaction.Checked != hospitalityType.StayInPosAfterTrans
                   || chkSendVoidedItemsToStation.Checked != hospitalityType.SendVoidedItemsToStation
                   || chkSendTransfersToStation.Checked != hospitalityType.SendTransfersToStation
                   || chkSendOrderNoToStation.Checked != hospitalityType.SendOrderNoToStation
                   || chkSendSuspensionsToStation.Checked != hospitalityType.SendSuspensionsToStation
                   || cmbStationPrinting.SelectedIndex != (int) hospitalityType.StationPrinting
                   || chkPrintTrainingTransaction.Checked != hospitalityType.PrintTrainingTransactions
                   || chkDefaultType.Checked != hospitalityType.DefaultType;
        }

        public bool SaveData()
        {
            hospitalityType.Overview = (HospitalityType.OverviewEnum)cmbOverview.SelectedIndex;
            hospitalityType.AccessToOtherRestaurant = cmbAccessToOtherRestaurant.SelectedData == null ? "" : cmbAccessToOtherRestaurant.SelectedData.ID;
            hospitalityType.StaffTakeOverInTrans = (HospitalityType.StaffTakeOverInTransEnum)cmbStaffTakeoverInTransaction.SelectedIndex;
            hospitalityType.ManagerTakeOverInTrans = (HospitalityType.ManagerTakeOverInTransEnum)cmbManagerTakeoverInTransaction.SelectedIndex;
            hospitalityType.LayoutID = cmbTouchButtonsID.SelectedData == null ? "" : cmbTouchButtonsID.SelectedData.ID;
            hospitalityType.TopPosMenuID = cmbTopPosMenuID.SelectedData == null ? "" : cmbTopPosMenuID.SelectedData.ID;
            hospitalityType.PosLogonMenuID = cmbPosLogonMenuID.SelectedData == null ? "" : cmbPosLogonMenuID.SelectedData.ID;
            hospitalityType.StayInPosAfterTrans = chkStayInPosAfterTransaction.Checked;
            hospitalityType.SendVoidedItemsToStation = chkSendVoidedItemsToStation.Checked;
            hospitalityType.SendTransfersToStation = chkSendTransfersToStation.Checked;
            hospitalityType.SendOrderNoToStation = chkSendOrderNoToStation.Checked;
            hospitalityType.SendSuspensionsToStation = chkSendSuspensionsToStation.Checked;
            hospitalityType.StationPrinting = (HospitalityType.StationPrintingEnum)cmbStationPrinting.SelectedIndex;
            hospitalityType.PrintTrainingTransactions = chkPrintTrainingTransaction.Checked;
            hospitalityType.DefaultType = chkDefaultType.Checked;

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

        private void cmbAccessToOtherRestaurant_RequestData(object sender, EventArgs e)
        {
            cmbAccessToOtherRestaurant.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbVisualProfile_RequestData(object sender, EventArgs e)
        {
            cmbTouchButtonsID.SetData(Providers.TouchLayoutData.GetList(PluginEntry.DataModel, "NAME"), null);
        }

        private void cmbAccessToOtherRestaurant_SelectedDataChanged(object sender, EventArgs e)
        {
            if (suspendEvents)
                return;

            hospitalityType.AccessToOtherRestaurant = cmbAccessToOtherRestaurant.SelectedData == null ? "" : cmbAccessToOtherRestaurant.SelectedData.ID;
            accessToOtherRestaurantDirty = true;

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.VariableChanged, "HospitalityType.AccessToOtherRestaurant", hospitalityType.AccessToOtherRestaurant, null);
        }

        private void cmbOverview_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suspendEvents)
                return;

            hospitalityType.Overview = (HospitalityType.OverviewEnum)cmbOverview.SelectedIndex;
            overviewDirty = true;

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.VariableChanged, "HospitalityType.Overview", new RecordIdentifier((int)hospitalityType.Overview), null);
        }

        private void cmbTopPosMenuID_RequestData(object sender, EventArgs e)
        {
            cmbTopPosMenuID.SetData(Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, MenuTypeEnum.Hospitality), null);
        }

        private void cmbPosLogonMenuID_RequestData(object sender, EventArgs e)
        {
            cmbPosLogonMenuID.SetData(Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, MenuTypeEnum.Hospitality), null);
        }

        private void btnEditVisualProfile_Click(object sender, EventArgs e)
        {
            if (touchButtonsEditor.IsAlive)
            {
                ((IPlugin)touchButtonsEditor.Target).Message(this, "EditLayout", cmbTouchButtonsID.SelectedData.ID);
            }
        }

        private void btnsEditAddTopPosMenu_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewPosMenu();
        }

        private void btnsEditAddTopPosMenu_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowPosMenusListView(cmbTopPosMenuID.SelectedData.ID);
        }

        private void cmbTopPosMenuID_SelectedDataChanged(object sender, EventArgs e)
        {
            btnsEditAddTopPosMenu.EditButtonEnabled = !cmbTopPosMenuID.SelectedData.ID.IsEmpty;           
        }

        private void btnsEditAddPosLogonMenu_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewPosMenu();
        }

        private void btnsEditAddPosLogonMenu_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowPosMenusListView(cmbPosLogonMenuID.SelectedData.ID);
        }

        private void cmbPosLogonMenuID_SelectedDataChanged(object sender, EventArgs e)
        {
            btnsEditAddPosLogonMenu.EditButtonEnabled = !cmbPosLogonMenuID.SelectedData.ID.IsEmpty;           
        }

        private void btnsEditAddTopPosMenu_Load(object sender, EventArgs e)
        {

        }
    }
}
