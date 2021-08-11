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
    internal partial class StyleProfileModifiersPage  : UserControl, ITabView
    {
        LSOneKitchenDisplayStyleProfile styleProfile;

        public StyleProfileModifiersPage()
        {
            InitializeComponent();
            cmbInfocode.Tag = PluginEntry.ChitLineGuid;
            cmbComment.Tag = PluginEntry.ChitLineGuid;
            cmbVoided.Tag = PluginEntry.ChitLineGuid;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StyleProfileModifiersPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            styleProfile = (LSOneKitchenDisplayStyleProfile)internalContext;
            cmbInfocode.SelectedData = styleProfile.IncreaseItemModifierStyle;
            cmbComment.SelectedData = styleProfile.CommentModifierStyle;
            cmbVoided.SelectedData = styleProfile.ItemModifierVoidedStyle;

            btnsIncrease.SetBuddyControl(cmbInfocode);
            btnsComment.SetBuddyControl(cmbComment);
            btnsVoided.SetBuddyControl(cmbVoided);
        }

        public bool DataIsModified()
        {
            if (cmbInfocode.SelectedData.ID != styleProfile.IncreaseItemModifierStyle.ID) return true;
            if (cmbComment.SelectedData.ID != styleProfile.CommentModifierStyle.ID) return true;
            if (cmbVoided.SelectedData.ID != styleProfile.ItemModifierVoidedStyle.ID) return true;

            return false;
        }

        public bool SaveData()
        {
            styleProfile.IncreaseItemModifierStyle.ID = cmbInfocode.SelectedData.ID;
            styleProfile.CommentModifierStyle.ID = cmbComment.SelectedData.ID;
            styleProfile.ItemModifierVoidedStyle.ID = cmbVoided.SelectedData.ID;

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


