using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    internal partial class StyleProfileItemsPage : UserControl, ITabView
    {
        LSOneKitchenDisplayStyleProfile styleProfile;

        public StyleProfileItemsPage()
        {
            InitializeComponent();
            cmbDefault.Tag = PluginEntry.DefaultChitLineGuid;
            cmbOnTime.Tag = PluginEntry.ChitLineGuid;
            cmbDone.Tag = PluginEntry.ChitLineGuid;
            cmbModified.Tag = PluginEntry.ChitLineGuid;
            cmbVoided.Tag = PluginEntry.ChitLineGuid;
            cmbRush.Tag = PluginEntry.ChitLineGuid;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StyleProfileItemsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            styleProfile = (LSOneKitchenDisplayStyleProfile)internalContext;
            cmbDefault.SelectedData = styleProfile.ItemDefaultStyle;
            cmbOnTime.SelectedData = styleProfile.ItemOnTimeStyle;
            cmbDone.SelectedData = styleProfile.ItemDoneStyle;
            cmbModified.SelectedData = styleProfile.ItemModifiedStyle;
            cmbVoided.SelectedData = styleProfile.ItemVoidedStyle;
            cmbRush.SelectedData = styleProfile.ItemRushStyle;

            btnsDefault.SetBuddyControl(cmbDefault);
            btnsOnTime.SetBuddyControl(cmbOnTime);
            btnsDone.SetBuddyControl(cmbDone);
            btnsModified.SetBuddyControl(cmbModified);
            btnsVoided.SetBuddyControl(cmbVoided);
            btnsRush.SetBuddyControl(cmbRush);
        }

        public bool DataIsModified()
        {
            if (cmbDefault.SelectedData.ID != styleProfile.ItemDefaultStyle.ID) return true;
            if (cmbOnTime.SelectedData.ID != styleProfile.ItemOnTimeStyle.ID) return true;
            if (cmbDone.SelectedData.ID != styleProfile.ItemDoneStyle.ID) return true;
            if (cmbModified.SelectedData.ID != styleProfile.ItemModifiedStyle.ID) return true;
            if (cmbVoided.SelectedData.ID != styleProfile.ItemVoidedStyle.ID) return true;
            if (cmbRush.SelectedData.ID != styleProfile.ItemRushStyle.ID) return true;
            return false;
        }

        public bool SaveData()
        {
            styleProfile.ItemDefaultStyle.ID = cmbDefault.SelectedData.ID;
            styleProfile.ItemOnTimeStyle.ID = cmbOnTime.SelectedData.ID;
            styleProfile.ItemDoneStyle.ID = cmbDone.SelectedData.ID;
            styleProfile.ItemModifiedStyle.ID = cmbModified.SelectedData.ID;
            styleProfile.ItemVoidedStyle.ID = cmbVoided.SelectedData.ID;
            styleProfile.ItemRushStyle.ID = cmbRush.SelectedData.ID;

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
            var comboBox = (DualDataComboBox) sender;
            var styles = Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME");
            comboBox.SetData(styles, null);
        }

        private void cmb_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity(RecordIdentifier.Empty, "");
        }

    }
}


