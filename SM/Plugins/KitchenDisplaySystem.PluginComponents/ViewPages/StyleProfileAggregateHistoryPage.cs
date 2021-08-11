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
    internal partial class StyleProfileAggregateHistoryPage : UserControl, ITabView
    {
        LSOneKitchenDisplayStyleProfile styleProfile;

        public StyleProfileAggregateHistoryPage()
        {
            InitializeComponent();

            cmbAggregateHeader.Tag = Guid.Empty;
            cmbAggregateBody.Tag = Guid.Empty;
            cmbAggregatePane.Tag = Guid.Empty;
            cmbHistoryHeader.Tag = Guid.Empty;
            cmbHistoryBody.Tag = Guid.Empty;
            cmbHistoryPane.Tag = Guid.Empty;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new StyleProfileAggregateHistoryPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            styleProfile = (LSOneKitchenDisplayStyleProfile)internalContext;

            cmbAggregateHeader.SelectedData = styleProfile.AggregateHeaderStyle;
            cmbAggregateBody.SelectedData = styleProfile.AggregateBodyStyle;
            cmbAggregatePane.SelectedData = styleProfile.AggregatePaneStyle;
            cmbHistoryHeader.SelectedData = styleProfile.HistoryHeaderStyle;
            cmbHistoryBody.SelectedData = styleProfile.HistoryBodyStyle;
            cmbHistoryPane.SelectedData = styleProfile.HistoryPaneStyle;

            btnsAggregateHeader.SetBuddyControl(cmbAggregateHeader);
            btnsAggregateBody.SetBuddyControl(cmbAggregateBody);
            btnsAggregatePane.SetBuddyControl(cmbAggregatePane);
            btnsHistoryHeader.SetBuddyControl(cmbHistoryHeader);
            btnsHistoryBody.SetBuddyControl(cmbHistoryBody);
            btnsHistoryPane.SetBuddyControl(cmbHistoryPane);
        }

        public bool DataIsModified()
        {
            if (cmbAggregateHeader.SelectedData.ID != styleProfile.AggregateHeaderStyle.ID) return true;
            if (cmbAggregateBody.SelectedData.ID != styleProfile.AggregateBodyStyle.ID) return true;
            if (cmbAggregatePane.SelectedData.ID != styleProfile.AggregatePaneStyle.ID) return true;
            if (cmbHistoryHeader.SelectedData.ID != styleProfile.HistoryHeaderStyle.ID) return true;
            if (cmbHistoryBody.SelectedData.ID != styleProfile.HistoryBodyStyle.ID) return true;
            if (cmbHistoryPane.SelectedData.ID != styleProfile.HistoryPaneStyle.ID) return true;

            return false;
        }

        public bool SaveData()
        {
            styleProfile.AggregateHeaderStyle.ID = cmbAggregateHeader.SelectedData.ID;
            styleProfile.AggregateBodyStyle.ID = cmbAggregateBody.SelectedData.ID;
            styleProfile.AggregatePaneStyle.ID = cmbAggregatePane.SelectedData.ID;
            styleProfile.HistoryHeaderStyle.ID = cmbHistoryHeader.SelectedData.ID;
            styleProfile.HistoryBodyStyle.ID = cmbHistoryBody.SelectedData.ID;
            styleProfile.HistoryPaneStyle.ID = cmbHistoryPane.SelectedData.ID;

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
            List<PosStyle> styles = Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME");
            ((DualDataComboBox)sender).SetData(styles, null);
        }

        private void cmb_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity(RecordIdentifier.Empty, "");
        }
    }
}