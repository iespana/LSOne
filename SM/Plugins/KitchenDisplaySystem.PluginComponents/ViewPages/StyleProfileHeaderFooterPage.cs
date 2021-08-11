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
    internal partial class StyleProfileHeaderFooterPage : UserControl, ITabView
    {
        LSOneKitchenDisplayStyleProfile styleProfile;

        public StyleProfileHeaderFooterPage()
        {
            InitializeComponent();

            cmbDefaultHeader.Tag = PluginEntry.ChitHeaderFooterGuid;
            cmbDefaultFooter.Tag = PluginEntry.ChitHeaderFooterGuid;
            cmbTransactComment.Tag = PluginEntry.ChitHeaderFooterGuid;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StyleProfileHeaderFooterPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            styleProfile = (LSOneKitchenDisplayStyleProfile)internalContext;
            cmbDefaultHeader.SelectedData = styleProfile.DefaultHeaderStyle;
            cmbDefaultFooter.SelectedData = styleProfile.DefaultFooterStyle;
            cmbTransactComment.SelectedData = styleProfile.TransactCommentStyle;

            btnsDefaultHeader.SetBuddyControl(cmbDefaultHeader);
            btnsDefaultFooter.SetBuddyControl(cmbDefaultFooter);
            btnsTransactComment.SetBuddyControl(cmbTransactComment);
        }

        public bool DataIsModified()
        {
            if (cmbDefaultHeader.SelectedData.ID != styleProfile.DealHeaderStyle.ID) return true;
            if (cmbDefaultFooter.SelectedData.ID != styleProfile.DefaultFooterStyle.ID) return true;
            if (cmbTransactComment.SelectedData.ID != styleProfile.TransactCommentStyle.ID) return true;

            return false;
        }

        public bool SaveData()
        {
            styleProfile.DefaultHeaderStyle.ID = cmbDefaultHeader.SelectedData.ID;
            styleProfile.DefaultFooterStyle.ID = cmbDefaultFooter.SelectedData.ID;
            styleProfile.TransactCommentStyle.ID = cmbTransactComment.SelectedData.ID;

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
            ((DualDataComboBox)sender).SelectedData = new DataEntity(RecordIdentifier.Empty,"");
        }

    }
}


