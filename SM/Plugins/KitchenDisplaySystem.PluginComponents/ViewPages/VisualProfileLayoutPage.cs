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
using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls;
using static LSOne.DataLayer.KDSBusinessObjects.KitchenDisplayVisualProfile;
using TabControl = LSOne.ViewCore.Controls.TabControl;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    internal partial class VisualProfileLayoutPage : UserControl, ITabView
    {
        KitchenDisplayVisualProfile kitchenDisplayVisualProfile;
        KitchenDisplayUIPreview uiPreview;
        bool initializing;

        public VisualProfileLayoutPage()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                uiPreview = new KitchenDisplayUIPreview();
                pnlPreview.Controls.Add(uiPreview);
                uiPreview.Dock = DockStyle.Fill;
            }
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new VisualProfileLayoutPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            initializing = true;

            kitchenDisplayVisualProfile = (KitchenDisplayVisualProfile)internalContext;

            cmbHeaderProfile.SelectedData = Providers.KitchenDisplayHeaderPaneData.Get(PluginEntry.DataModel, Guid.Parse(kitchenDisplayVisualProfile.HeaderProfileId)) ?? new DataEntity(Guid.Empty, "");
            tbHeaderHeight.Value = (int)(kitchenDisplayVisualProfile.HeaderPaneHeight * 100);

            chkAggregateVisable.Checked = kitchenDisplayVisualProfile.AggregatePaneVisible;
            cbAggregatePosition.SelectedIndex = (int)kitchenDisplayVisualProfile.AggregatePanePosition;
            tbAggregateColumns.Value = kitchenDisplayVisualProfile.AggregatePaneNumberofColumns;
            if (kitchenDisplayVisualProfile.AggregatePanePosition == KitchenDisplayVisualProfile.AggregatePositionEnum.Right ||
               kitchenDisplayVisualProfile.AggregatePanePosition == KitchenDisplayVisualProfile.AggregatePositionEnum.Left)
            {
                tbAggregateSize.Value = (int)(kitchenDisplayVisualProfile.AggregatePaneWidth * 100);
            } 
            else
            {
                tbAggregateSize.Value = (int)(kitchenDisplayVisualProfile.AggregatePaneHeight * 100);
            }

            chkButtonVisable.Checked = kitchenDisplayVisualProfile.ButtonPaneVisible;
            cmbButtonPosistion.SelectedIndex = (int)kitchenDisplayVisualProfile.ButtonPanePosition;
            if(kitchenDisplayVisualProfile.ButtonPanePosition == KitchenDisplayVisualProfile.ButtonPositionEnum.Right ||
               kitchenDisplayVisualProfile.ButtonPanePosition == KitchenDisplayVisualProfile.ButtonPositionEnum.Left)
            {
                tbButtonSize.Value = (int)(kitchenDisplayVisualProfile.ButtonPaneWidth * 100);
            }
            else
            {
                tbButtonSize.Value = (int)(kitchenDisplayVisualProfile.ButtonPaneHeight * 100);
            }

            chkHistoryVisable.Checked = kitchenDisplayVisualProfile.HistoryPaneVisible;
            tbHistorySize.Value = (int)(kitchenDisplayVisualProfile.HistoryPaneWidth * 100);
            cmbHistoryPosition.SelectedIndex = (int)kitchenDisplayVisualProfile.HistoryPanePosition;
            tbHistoryRows.Value = kitchenDisplayVisualProfile.HistoryMaxLastBumpedLines;
            tbHistoryHorizon.Value = kitchenDisplayVisualProfile.HistoryLifespanInMinutes;

            uiPreview.AdjustPreview(kitchenDisplayVisualProfile);

            initializing = false;
        }

        public bool DataIsModified()
        {
            if (cmbHeaderProfile.SelectedData.ID != kitchenDisplayVisualProfile.HeaderProfileId) return true;
            if (tbHeaderHeight.Value != (int)(kitchenDisplayVisualProfile.HeaderPaneHeight * 100)) return true;

            if (chkAggregateVisable.Checked != kitchenDisplayVisualProfile.AggregatePaneVisible) return true;
            if (tbAggregateColumns.Value != kitchenDisplayVisualProfile.AggregatePaneNumberofColumns) return true;
            if (cbAggregatePosition.SelectedIndex != (int)kitchenDisplayVisualProfile.AggregatePanePosition) return true;
            if (cbAggregatePosition.SelectedIndex == (int)KitchenDisplayVisualProfile.AggregatePositionEnum.Right ||
                cbAggregatePosition.SelectedIndex == (int)KitchenDisplayVisualProfile.AggregatePositionEnum.Left)
            {
                if (tbAggregateSize.Value != (int)(kitchenDisplayVisualProfile.AggregatePaneWidth * 100)) return true;
            }
            else
            {
                if (tbAggregateSize.Value != (int)(kitchenDisplayVisualProfile.AggregatePaneHeight * 100)) return true;
            }

            if (chkButtonVisable.Checked != kitchenDisplayVisualProfile.ButtonPaneVisible) return true;
            if (cmbButtonPosistion.SelectedIndex != (int)kitchenDisplayVisualProfile.ButtonPanePosition) return true;
            if (cmbButtonPosistion.SelectedIndex == (int)KitchenDisplayVisualProfile.ButtonPositionEnum.Right ||
                cmbButtonPosistion.SelectedIndex == (int)KitchenDisplayVisualProfile.ButtonPositionEnum.Left)
            {
                if (tbButtonSize.Value != (int)(kitchenDisplayVisualProfile.ButtonPaneWidth * 100)) return true;
            }
            else
            {
                if (tbButtonSize.Value != (int)(kitchenDisplayVisualProfile.ButtonPaneHeight * 100)) return true;
            }

            if (chkHistoryVisable.Checked != kitchenDisplayVisualProfile.HistoryPaneVisible) return true;
            if (tbHistorySize.Value != (int)(kitchenDisplayVisualProfile.HistoryPaneWidth * 100)) return true;
            if (cmbHistoryPosition.SelectedIndex != (int)kitchenDisplayVisualProfile.HistoryPanePosition) return true;
            if (tbHistoryRows.Value != kitchenDisplayVisualProfile.HistoryMaxLastBumpedLines) return true;
            if (tbHistoryHorizon.Value != kitchenDisplayVisualProfile.HistoryLifespanInMinutes) return true;

            return false;
        }

        public bool SaveData()
        {
            uiPreview.PopulateCoordinates(kitchenDisplayVisualProfile);
            GetData(kitchenDisplayVisualProfile);

            return true;
        }

        private void GetData(KitchenDisplayVisualProfile visualProfile)
        {
            visualProfile.HeaderProfileId = cmbHeaderProfile.SelectedData == null ? null : (string)cmbHeaderProfile.SelectedData.ID;
            visualProfile.HeaderPaneHeight = (decimal)tbHeaderHeight.Value / 100;

            visualProfile.AggregatePaneVisible = chkAggregateVisable.Checked;
            visualProfile.AggregatePanePosition = (KitchenDisplayVisualProfile.AggregatePositionEnum)cbAggregatePosition.SelectedIndex;
            visualProfile.AggregatePaneNumberofColumns = (int)tbAggregateColumns.Value;
            if (cbAggregatePosition.SelectedIndex == (int)KitchenDisplayVisualProfile.AggregatePositionEnum.Right ||
                cbAggregatePosition.SelectedIndex == (int)KitchenDisplayVisualProfile.AggregatePositionEnum.Left)
            {
                visualProfile.AggregatePaneWidth = (decimal)tbAggregateSize.Value / 100;
            }
            else
            {
                visualProfile.AggregatePaneHeight = (decimal)tbAggregateSize.Value / 100;
            }

            visualProfile.ButtonPaneVisible = chkButtonVisable.Checked;
            visualProfile.ButtonPanePosition = (KitchenDisplayVisualProfile.ButtonPositionEnum)cmbButtonPosistion.SelectedIndex;
            if (visualProfile.ButtonPanePosition == KitchenDisplayVisualProfile.ButtonPositionEnum.Right ||
                visualProfile.ButtonPanePosition == KitchenDisplayVisualProfile.ButtonPositionEnum.Left)
            {
                visualProfile.ButtonPaneWidth = (decimal)tbButtonSize.Value / 100;
            }
            else
            {
                visualProfile.ButtonPaneHeight = (decimal)tbButtonSize.Value / 100;
            }

            visualProfile.HistoryPaneVisible = chkHistoryVisable.Checked;
            visualProfile.HistoryPaneWidth = (decimal)tbHistorySize.Value / 100;
            visualProfile.HistoryPanePosition = (HistoryPositionEnum)cmbHistoryPosition.SelectedIndex;
            visualProfile.HistoryMaxLastBumpedLines = (int)tbHistoryRows.Value;
            visualProfile.HistoryLifespanInMinutes = (int)tbHistoryHorizon.Value;
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

        private void cmbHeaderProfile_RequestData(object sender, EventArgs e)
        {
            List<HeaderPaneProfile> headerPaneProfiles = Providers.KitchenDisplayHeaderPaneData.GetList(PluginEntry.DataModel);
            cmbHeaderProfile.SetData(headerPaneProfiles.ConvertAll(p => new DataEntity(p.ID, p.Name)), null);
        }

        private void btnsHeaderProfile_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowHeaderPaneDialog();
        }

        private void btnsHeaderProfile_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowHeaderPanesView(sender, e);
        }

        private void UpdatePreview(object sender, EventArgs e)
        {
            if (initializing) return;

            KitchenDisplayVisualProfile updatedProfile = new KitchenDisplayVisualProfile();
            GetData(updatedProfile);
            uiPreview.AdjustPreview(updatedProfile);
        }
    }
}