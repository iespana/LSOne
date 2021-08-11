using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    internal partial class StyleProfileButtonsPage : UserControl, ITabView
    {
        LSOneKitchenDisplayStyleProfile styleProfile;

        public StyleProfileButtonsPage()
        {
            InitializeComponent();
            cmbDefault.Tag = Guid.Empty;
            cmbNext.Tag = Guid.Empty;
            cmbPrevious.Tag = Guid.Empty;
            cmbBump.Tag = Guid.Empty;
            cmbStart.Tag = Guid.Empty;
            cmbNextScreen.Tag = Guid.Empty;
            cmbPreviousScreen.Tag = Guid.Empty;
            cmbSelectStart.Tag = Guid.Empty;
            cmbSelectBump.Tag = Guid.Empty;
            cmbRecallLastBump.Tag = Guid.Empty;
            cmbHome.Tag = Guid.Empty;
            cmbEnd.Tag = Guid.Empty;
            cmbMark.Tag = Guid.Empty;
            cmbServe.Tag = Guid.Empty;
            cmbTransfer.Tag = Guid.Empty;
            cmbRush.Tag = Guid.Empty;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new StyleProfileButtonsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            styleProfile = (LSOneKitchenDisplayStyleProfile)internalContext;

            cmbDefault.SelectedData = styleProfile.ButtonDefaultStyle;
            cmbNext.SelectedData = styleProfile.ButtonNextStyle;
            cmbPrevious.SelectedData = styleProfile.ButtonPreviousStyle;
            cmbBump.SelectedData = styleProfile.ButtonBumpStyle;
            cmbStart.SelectedData = styleProfile.ButtonStartStyle;
            cmbNextScreen.SelectedData = styleProfile.ButtonNextScreenStyle;
            cmbPreviousScreen.SelectedData = styleProfile.ButtonPreviousScreenStyle;
            cmbSelectStart.SelectedData = styleProfile.ButtonSelectStartStyle;
            cmbSelectBump.SelectedData = styleProfile.ButtonSelectBumpStyle;
            cmbRecallLastBump.SelectedData = styleProfile.ButtonRecallStyle;
            cmbHome.SelectedData = styleProfile.ButtonHomeStyle;
            cmbEnd.SelectedData = styleProfile.ButtonEndStyle;
            cmbMark.SelectedData = styleProfile.ButtonMarkStyle;
            cmbServe.SelectedData = styleProfile.ButtonServeStyle;
            cmbTransfer.SelectedData = styleProfile.ButtonTransferStyle;
            cmbRush.SelectedData = styleProfile.ButtonRushStyle;

            btnsDefault.SetBuddyControl(cmbDefault);
            btnsNext.SetBuddyControl(cmbNext);
            btnsPrevious.SetBuddyControl(cmbPrevious);
            btnsBump.SetBuddyControl(cmbBump);
            btnsStart.SetBuddyControl(cmbStart);
            btnsNextScreen.SetBuddyControl(cmbNextScreen);
            btnsPreviousScreen.SetBuddyControl(cmbPreviousScreen);
            btnsSelectStart.SetBuddyControl(cmbSelectStart);
            btnsSelectBump.SetBuddyControl(cmbSelectBump);
            btnsRecallLastBump.SetBuddyControl(cmbRecallLastBump);
            btnsHome.SetBuddyControl(cmbHome);
            btnsEnd.SetBuddyControl(cmbEnd);
            btnsMark.SetBuddyControl(cmbMark);
            btnsServe.SetBuddyControl(cmbServe);
            btnsTransfer.SetBuddyControl(cmbTransfer);
            btnsRush.SetBuddyControl(cmbRush);
        }

        public bool DataIsModified()
        {            
            if (cmbDefault.SelectedData.ID != styleProfile.ButtonDefaultStyle.ID) return true;
            if (cmbNext.SelectedData.ID != styleProfile.ButtonNextStyle.ID) return true;
            if (cmbPrevious.SelectedData.ID != styleProfile.ButtonPreviousStyle.ID) return true;
            if (cmbBump.SelectedData.ID != styleProfile.ButtonBumpStyle.ID) return true;
            if (cmbStart.SelectedData.ID != styleProfile.ButtonStartStyle.ID) return true;
            if (cmbNextScreen.SelectedData.ID != styleProfile.ButtonNextScreenStyle.ID) return true;
            if (cmbPreviousScreen.SelectedData.ID != styleProfile.ButtonPreviousScreenStyle.ID) return true;
            if (cmbSelectStart.SelectedData.ID != styleProfile.ButtonSelectStartStyle.ID) return true;
            if (cmbSelectBump.SelectedData.ID != styleProfile.ButtonSelectBumpStyle.ID) return true;
            if (cmbRecallLastBump.SelectedData.ID != styleProfile.ButtonRecallStyle.ID) return true;
            if (cmbHome.SelectedData.ID != styleProfile.ButtonHomeStyle.ID) return true;
            if (cmbEnd.SelectedData.ID != styleProfile.ButtonEndStyle.ID) return true;
            if (cmbMark.SelectedData.ID != styleProfile.ButtonMarkStyle.ID) return true;
            if (cmbServe.SelectedData.ID != styleProfile.ButtonServeStyle.ID) return true;
            if (cmbTransfer.SelectedData.ID != styleProfile.ButtonTransferStyle.ID) return true;
            if (cmbRush.SelectedData.ID != styleProfile.ButtonRushStyle.ID) return true;

            return false;
        }

        public bool SaveData()
        {
            styleProfile.ButtonDefaultStyle.ID = cmbDefault.SelectedData.ID;
            styleProfile.ButtonNextStyle.ID = cmbNext.SelectedData.ID;
            styleProfile.ButtonPreviousStyle.ID = cmbPrevious.SelectedData.ID;
            styleProfile.ButtonBumpStyle.ID = cmbBump.SelectedData.ID;
            styleProfile.ButtonStartStyle.ID = cmbStart.SelectedData.ID;
            styleProfile.ButtonNextScreenStyle.ID = cmbNextScreen.SelectedData.ID;
            styleProfile.ButtonPreviousScreenStyle.ID = cmbPreviousScreen.SelectedData.ID;
            styleProfile.ButtonSelectStartStyle.ID = cmbSelectStart.SelectedData.ID;
            styleProfile.ButtonSelectBumpStyle.ID = cmbSelectBump.SelectedData.ID;
            styleProfile.ButtonRecallStyle.ID = cmbRecallLastBump.SelectedData.ID;
            styleProfile.ButtonHomeStyle.ID = cmbHome.SelectedData.ID;
            styleProfile.ButtonEndStyle.ID = cmbEnd.SelectedData.ID;
            styleProfile.ButtonMarkStyle.ID = cmbMark.SelectedData.ID;
            styleProfile.ButtonServeStyle.ID = cmbServe.SelectedData.ID;
            styleProfile.ButtonTransferStyle.ID = cmbTransfer.SelectedData.ID;
            styleProfile.ButtonRushStyle.ID = cmbRush.SelectedData.ID;

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

        private void cmb_RequestData(object sender, EventArgs e)
        {
            var comboBox = (DualDataComboBox)sender;
            var styles = Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME");
            comboBox.SetData(styles, null);
        }

        private void cmb_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity(RecordIdentifier.Empty, "");
        }
    }
}


