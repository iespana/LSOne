using System;
using System.Collections.Generic;
using System.Drawing;
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
    public partial class HospitalitySetupDineInPage : UserControl, ITabView
    {
        private HospitalitySetup setup;

        public HospitalitySetupDineInPage()
        {
            InitializeComponent();
        }

        #region ITabView Members

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalitySetupDineInPage();
        }

        public bool DataIsModified()
        {
            if ((cmbDineInSalesType.SelectedData != null) && (cmbDineInSalesType.SelectedData.ID.ToString() != setup.DineInSalesType)) return true;
            if (cmbDineInTableSelection.SelectedIndex != (int)setup.DineInTableSelection) return true;
            if (cmbDineInTableLocking.SelectedIndex != (int)setup.DineInTableLocking) return true;

            // Backcolors
            if (wellTableFree.SelectedColor != Color.FromArgb(setup.TableFreeColorB)) return true;
            if (wellTableNotAvail.SelectedColor != Color.FromArgb(setup.TableNotAvailColorB)) return true;
            if (wellTableLocked.SelectedColor != Color.FromArgb(setup.TableLockedColorB)) return true;
            if (wellOrderNotPrinted.SelectedColor != Color.FromArgb(setup.OrderNotPrintedColorB)) return true;
            if (wellOrderPrinted.SelectedColor != Color.FromArgb(setup.OrderPrintedColorB)) return true;
            if (wellOrderStarted.SelectedColor != Color.FromArgb(setup.OrderStartedColorB)) return true;
            if (wellOrderFinished.SelectedColor != Color.FromArgb(setup.OrderFinishedColorB)) return true;
            if (wellOrderConfirmed.SelectedColor != Color.FromArgb(setup.OrderConfirmedColorB)) return true;

            // Forecolors
            if ((cmbTableFreeColorF.SelectedData != null) && (cmbTableFreeColorF.SelectedData.ID.ToString() != setup.TableFreeColorF)) return true;
            if ((cmbTableNotAvailColorF.SelectedData != null) && (cmbTableNotAvailColorF.SelectedData.ID.ToString() != setup.TableNotAvailColorF)) return true;
            if ((cmbTableLockedColorF.SelectedData != null) && (cmbTableLockedColorF.SelectedData.ID.ToString() != setup.TableLockedColorF)) return true;
            if ((cmbOrderNotPrintedColorF.SelectedData != null) && (cmbOrderNotPrintedColorF.SelectedData.ID.ToString() != setup.OrderNotPrintedColorF)) return true;
            if ((cmbOrderPrintedColorF.SelectedData != null) && (cmbOrderPrintedColorF.SelectedData.ID.ToString() != setup.OrderPrintedColorF)) return true;
            if ((cmbOrderStartedColorF.SelectedData != null) && (cmbOrderStartedColorF.SelectedData.ID.ToString() != setup.OrderStartedColorF)) return true;
            if ((cmbOrderFinishedColorF.SelectedData != null) && (cmbOrderFinishedColorF.SelectedData.ID.ToString() != setup.OrderFinishedColorF)) return true;
            if ((cmbOrderConfirmedColorF.SelectedData != null) && (cmbOrderConfirmedColorF.SelectedData.ID.ToString() != setup.OrderConfirmedColorF)) return true;

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //throw new NotImplementedException();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            setup = (HospitalitySetup)internalContext;

            cmbDineInSalesType.SelectedData = Providers.SalesTypeData.GetSalesTypeIdDescription(PluginEntry.DataModel, setup.DineInSalesType);
            cmbDineInTableSelection.SelectedIndex = (int)setup.DineInTableSelection;
            cmbDineInTableLocking.SelectedIndex = (int)setup.DineInTableLocking;

            // Backcolors
            wellTableFree.SelectedColor = Color.FromArgb(setup.TableFreeColorB);
            wellTableNotAvail.SelectedColor = Color.FromArgb(setup.TableNotAvailColorB);
            wellTableLocked.SelectedColor = Color.FromArgb(setup.TableLockedColorB);
            wellOrderNotPrinted.SelectedColor = Color.FromArgb(setup.OrderNotPrintedColorB);
            wellOrderPrinted.SelectedColor = Color.FromArgb(setup.OrderPrintedColorB);
            wellOrderStarted.SelectedColor = Color.FromArgb(setup.OrderStartedColorB);
            wellOrderFinished.SelectedColor = Color.FromArgb(setup.OrderFinishedColorB);
            wellOrderConfirmed.SelectedColor = Color.FromArgb(setup.OrderConfirmedColorB);

            // Forecolors
            cmbTableFreeColorF.SelectedData = String.IsNullOrEmpty(setup.TableFreeColorF) ? null : Providers.PosColorData.GetColor(PluginEntry.DataModel, setup.TableFreeColorF);
            cmbTableNotAvailColorF.SelectedData = String.IsNullOrEmpty(setup.TableNotAvailColorF) ? null : Providers.PosColorData.GetColor(PluginEntry.DataModel, setup.TableNotAvailColorF);
            cmbTableLockedColorF.SelectedData = String.IsNullOrEmpty(setup.TableLockedColorF) ? null : Providers.PosColorData.GetColor(PluginEntry.DataModel, setup.TableLockedColorF);
            cmbOrderNotPrintedColorF.SelectedData = String.IsNullOrEmpty(setup.OrderNotPrintedColorF) ? null : Providers.PosColorData.GetColor(PluginEntry.DataModel, setup.OrderNotPrintedColorF);
            cmbOrderPrintedColorF.SelectedData = String.IsNullOrEmpty(setup.OrderPrintedColorF) ? null : Providers.PosColorData.GetColor(PluginEntry.DataModel, setup.OrderPrintedColorF);
            cmbOrderStartedColorF.SelectedData = String.IsNullOrEmpty(setup.OrderStartedColorF) ? null : Providers.PosColorData.GetColor(PluginEntry.DataModel, setup.OrderStartedColorF);
            cmbOrderFinishedColorF.SelectedData = String.IsNullOrEmpty(setup.OrderFinishedColorF) ? null : Providers.PosColorData.GetColor(PluginEntry.DataModel, setup.OrderFinishedColorF);
            cmbOrderConfirmedColorF.SelectedData = String.IsNullOrEmpty(setup.OrderConfirmedColorF) ? null : Providers.PosColorData.GetColor(PluginEntry.DataModel, setup.OrderConfirmedColorF);
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            //throw new NotImplementedException();
        }

        public bool SaveData()
        {

            setup.DineInSalesType = cmbDineInSalesType.SelectedData == null ? "" : cmbDineInSalesType.SelectedData.ID.ToString();
            setup.DineInTableSelection = (HospitalitySetup.SetupDineInTableSelection)cmbDineInTableSelection.SelectedIndex;
            setup.DineInTableLocking = (HospitalitySetup.SetupDineInTableLocking)cmbDineInTableLocking.SelectedIndex;

            // Backcolors
            setup.TableFreeColorB = wellTableFree.SelectedColor.ToArgb();
            setup.TableNotAvailColorB = wellTableNotAvail.SelectedColor.ToArgb();
            setup.TableLockedColorB = wellTableLocked.SelectedColor.ToArgb();
            setup.OrderNotPrintedColorB = wellOrderNotPrinted.SelectedColor.ToArgb();
            setup.OrderPrintedColorB = wellOrderPrinted.SelectedColor.ToArgb();
            setup.OrderStartedColorB = wellOrderStarted.SelectedColor.ToArgb();
            setup.OrderFinishedColorB = wellOrderFinished.SelectedColor.ToArgb();
            setup.OrderConfirmedColorB = wellOrderConfirmed.SelectedColor.ToArgb();

            // Forecolors
            setup.TableFreeColorF = cmbTableFreeColorF.SelectedData == null ? "" : cmbTableFreeColorF.SelectedData.ID.ToString();
            setup.TableNotAvailColorF = cmbTableNotAvailColorF.SelectedData == null ? "" : cmbTableNotAvailColorF.SelectedData.ID.ToString();
            setup.TableLockedColorF = cmbTableLockedColorF.SelectedData == null ? "" : cmbTableLockedColorF.SelectedData.ID.ToString();
            setup.OrderNotPrintedColorF = cmbOrderNotPrintedColorF.SelectedData == null ? "" : cmbOrderNotPrintedColorF.SelectedData.ID.ToString();
            setup.OrderPrintedColorF = cmbOrderPrintedColorF.SelectedData == null ? "" : cmbOrderPrintedColorF.SelectedData.ID.ToString();
            setup.OrderStartedColorF = cmbOrderStartedColorF.SelectedData == null ? "" : cmbOrderStartedColorF.SelectedData.ID.ToString();
            setup.OrderFinishedColorF = cmbOrderFinishedColorF.SelectedData == null ? "" : cmbOrderFinishedColorF.SelectedData.ID.ToString();
            setup.OrderConfirmedColorF = cmbOrderConfirmedColorF.SelectedData == null ? "" : cmbOrderConfirmedColorF.SelectedData.ID.ToString();

            return true;
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion


        private void cmbDineInSalesType_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbDineInSalesType_RequestData(object sender, EventArgs e)
        {
            cmbDineInSalesType.SetData(Providers.SalesTypeData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbTableFreeColorF_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbTableFreeColorF_RequestData(object sender, EventArgs e)
        {
            cmbTableFreeColorF.SetData(Providers.PosColorData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbTableNotAvailColorF_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbTableNotAvailColorF_RequestData(object sender, EventArgs e)
        {
            cmbTableNotAvailColorF.SetData(Providers.PosColorData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbTableLockedColorF_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbTableLockedColorF_RequestData(object sender, EventArgs e)
        {
            cmbTableLockedColorF.SetData(Providers.PosColorData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbOrderNotPrintedColorF_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbOrderNotPrintedColorF_RequestData(object sender, EventArgs e)
        {
            cmbOrderNotPrintedColorF.SetData(Providers.PosColorData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbOrderPrintedColorF_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbOrderPrintedColorF_RequestData(object sender, EventArgs e)
        {
            cmbOrderPrintedColorF.SetData(Providers.PosColorData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbOrderStartedColorF_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbOrderStartedColorF_RequestData(object sender, EventArgs e)
        {
            cmbOrderStartedColorF.SetData(Providers.PosColorData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbOrderFinishedColorF_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbOrderFinishedColorF_RequestData(object sender, EventArgs e)
        {
            cmbOrderFinishedColorF.SetData(Providers.PosColorData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbOrderConfirmedColorF_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbOrderConfirmedColorF_RequestData(object sender, EventArgs e)
        {
            cmbOrderConfirmedColorF.SetData(Providers.PosColorData.GetList(PluginEntry.DataModel), null);
        }

        private void btnResetColors_Click(object sender, EventArgs e)
        {
            // Backcolors
            wellTableFree.SelectedColor = Color.FromArgb(-16445395);
            wellTableNotAvail.SelectedColor = Color.FromArgb(-12632256);
            wellTableLocked.SelectedColor = Color.FromArgb(-12615808);
            wellOrderNotPrinted.SelectedColor = Color.FromArgb(-9174701);
            wellOrderPrinted.SelectedColor = Color.FromArgb(-1441791);
            wellOrderStarted.SelectedColor = Color.FromArgb(-8566015);
            wellOrderFinished.SelectedColor = Color.FromArgb(-6706687);
            wellOrderConfirmed.SelectedColor = Color.FromArgb(-14075135);

            List<DataEntity> colorData = Providers.PosColorData.GetList(PluginEntry.DataModel);

            // Forecolors
            if (colorData.Count > 0)
            {
                cmbTableFreeColorF.SelectedData = colorData[0];
                cmbTableNotAvailColorF.SelectedData = colorData[0];
                cmbTableLockedColorF.SelectedData = colorData[0];
                cmbOrderNotPrintedColorF.SelectedData = colorData[0];
                cmbOrderPrintedColorF.SelectedData = colorData[0];
                cmbOrderStartedColorF.SelectedData = colorData[0];
                cmbOrderFinishedColorF.SelectedData = colorData[0];
                cmbOrderConfirmedColorF.SelectedData = colorData[0];
            }
        }

    }
}
