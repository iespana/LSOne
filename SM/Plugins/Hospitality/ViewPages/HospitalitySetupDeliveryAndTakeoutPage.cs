using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalitySetupDeliveryAndTakeoutPage : UserControl, ITabView
    {
        private HospitalitySetup setup;

        public HospitalitySetupDeliveryAndTakeoutPage()
        {
            InitializeComponent();
        }

        #region ITabView Members

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalitySetupDeliveryAndTakeoutPage();
        }

        public bool DataIsModified()
        {
            if (setup.OrderProcessTimeMin != (int)ntbOrderProcessTimeMin.Value) return true;

            if ((cmbDeliverySalesType.SelectedData != null) && (setup.DeliverySalesType != cmbDeliverySalesType.SelectedData.ID.ToString())) return true;
            if ((cmbTakeoutSalesType.SelectedData != null) && (setup.TakeOutSalesType != cmbTakeoutSalesType.SelectedData.ID.ToString())) return true;
            if ((cmbPreOrderSalesType.SelectedData != null) && (setup.PreOrderSalesType != cmbPreOrderSalesType.SelectedData.ID.ToString())) return true;

            if (setup.PopulateDeliveryInfocodes != chkPopulateDeliveryInfo.Checked) return true;
            if (setup.AllowPreOrders != chkAllowPreOrders.Checked) return true;
            if (setup.DisplayTimeAtOrderTaking != chkDisplayTimeAtOrder.Checked) return true;

            if (setup.AdvPreOrdPrintMin != (int)ntbAdvPreOrdPrintMin.Value) return true;
            if ((cmbPosTerminalPrintPreOrders.SelectedData != null) && (setup.PosTerminalPrintPreOrders != cmbPosTerminalPrintPreOrders.SelectedData.ID.ToString())) return true;
            if (setup.TakeoutNoNameNo != txtTakeoutNoNameNo.Text) return true;
            if (setup.CloseTripOnDepart != chkCloseTripOnDepart.Checked) return true;
            if (setup.DelProgressStatusInUse != chkDelProgressStatusInUse.Checked) return true;
            if (setup.DaysDriverTripsExist != (int)ntbDaysDriverTripsExist.Value) return true;

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
         
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            setup = (HospitalitySetup)internalContext;

            ntbOrderProcessTimeMin.Value = setup.OrderProcessTimeMin;

            cmbDeliverySalesType.SelectedData = Providers.SalesTypeData.GetSalesTypeIdDescription(PluginEntry.DataModel, setup.DeliverySalesType);
            cmbTakeoutSalesType.SelectedData = Providers.SalesTypeData.GetSalesTypeIdDescription(PluginEntry.DataModel, setup.TakeOutSalesType);
            cmbPreOrderSalesType.SelectedData = Providers.SalesTypeData.GetSalesTypeIdDescription(PluginEntry.DataModel, setup.PreOrderSalesType);

            chkPopulateDeliveryInfo.Checked = setup.PopulateDeliveryInfocodes;
            chkAllowPreOrders.Checked = setup.AllowPreOrders;
            chkDisplayTimeAtOrder.Checked = setup.DisplayTimeAtOrderTaking;

            ntbAdvPreOrdPrintMin.Value = setup.AdvPreOrdPrintMin;
            // TODO: This page is never used, so this code is never run
            //cmbPosTerminalPrintPreOrders.SelectedData = Providers.TerminalData.Get(PluginEntry.DataModel, setup.PosTerminalPrintPreOrders);
            txtTakeoutNoNameNo.Text = setup.TakeoutNoNameNo;
            chkCloseTripOnDepart.Checked = setup.CloseTripOnDepart;
            chkDelProgressStatusInUse.Checked = setup.DelProgressStatusInUse;
            ntbDaysDriverTripsExist.Value = setup.DaysDriverTripsExist;

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
        
        }

        public bool SaveData()
        {
            setup.OrderProcessTimeMin = (int)ntbOrderProcessTimeMin.Value;

            setup.DeliverySalesType = cmbDeliverySalesType.SelectedData == null ? "" : cmbDeliverySalesType.SelectedData.ID.ToString();
            setup.TakeOutSalesType =  cmbTakeoutSalesType.SelectedData == null ? "" : cmbTakeoutSalesType.SelectedData.ID.ToString();
            setup.PreOrderSalesType = cmbPreOrderSalesType.SelectedData == null ? "" : cmbPreOrderSalesType.SelectedData.ID.ToString();

            setup.PopulateDeliveryInfocodes = chkPopulateDeliveryInfo.Checked;
            setup.AllowPreOrders = chkAllowPreOrders.Checked;
            setup.DisplayTimeAtOrderTaking = chkDisplayTimeAtOrder.Checked;

            setup.AdvPreOrdPrintMin = (int)ntbAdvPreOrdPrintMin.Value;
            setup.PosTerminalPrintPreOrders = cmbPosTerminalPrintPreOrders.SelectedData == null ? "" : cmbPosTerminalPrintPreOrders.SelectedData.ID.ToString();
            setup.TakeoutNoNameNo = txtTakeoutNoNameNo.Text;
            setup.CloseTripOnDepart = chkCloseTripOnDepart.Checked;
            setup.DelProgressStatusInUse = chkDelProgressStatusInUse.Checked;
            setup.DaysDriverTripsExist = (int)ntbDaysDriverTripsExist.Value;

            return true;
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void cmbDeliverySalesType_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbDeliverySalesType_RequestData(object sender, EventArgs e)
        {
            cmbDeliverySalesType.SetData(Providers.SalesTypeData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbTakeoutSalesType_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbTakeoutSalesType_RequestData(object sender, EventArgs e)
        {
            cmbTakeoutSalesType.SetData(Providers.SalesTypeData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbPreOrderSalesType_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbPreOrderSalesType_RequestData(object sender, EventArgs e)
        {
            cmbPreOrderSalesType.SetData(Providers.SalesTypeData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbPosTerminalPrintPreOrders_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbPosTerminalPrintPreOrders_RequestData(object sender, EventArgs e)
        {
            cmbPosTerminalPrintPreOrders.SetData(Providers.TerminalData.GetList(PluginEntry.DataModel), null);
        }
    }
}
