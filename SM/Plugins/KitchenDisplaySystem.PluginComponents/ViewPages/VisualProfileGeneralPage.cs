using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using static LSOne.DataLayer.KDSBusinessObjects.KitchenDisplayVisualProfile;
using TabControl = LSOne.ViewCore.Controls.TabControl;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    internal partial class VisualProfileGeneralPage : UserControl, ITabView
    {
        KitchenDisplayVisualProfile kitchenDisplayVisualProfile;

        public VisualProfileGeneralPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.VisualProfileGeneralPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            kitchenDisplayVisualProfile = (KitchenDisplayVisualProfile)internalContext;

            cmbColumns.SelectedIndex = kitchenDisplayVisualProfile.NumberOfColumns - 1;
            cmbRows.SelectedIndex = kitchenDisplayVisualProfile.NumberOfRows - 1;
            tbChitRefresh.Value = kitchenDisplayVisualProfile.ChitRefreshRate;
            cmbChitSize.SelectedIndex = (int)kitchenDisplayVisualProfile.ChitSize;
        }

        public bool DataIsModified()
        {
            if (cmbColumns.SelectedIndex + 1 != kitchenDisplayVisualProfile.NumberOfColumns) return true;
            if (cmbRows.SelectedIndex + 1 != kitchenDisplayVisualProfile.NumberOfRows) return true;
            if (tbChitRefresh.Value != kitchenDisplayVisualProfile.ChitRefreshRate) return true;
            if (cmbChitSize.SelectedIndex != (int)kitchenDisplayVisualProfile.ChitSize) return true;

            return false;
        }

        public bool SaveData()
        {
            kitchenDisplayVisualProfile.NumberOfColumns = cmbColumns.SelectedIndex + 1;
            kitchenDisplayVisualProfile.NumberOfRows = cmbRows.SelectedIndex + 1;
            kitchenDisplayVisualProfile.ChitRefreshRate = (int)tbChitRefresh.Value;
            kitchenDisplayVisualProfile.ChitSize = (ChitSizeEnum)cmbChitSize.SelectedIndex;

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
    }
}


