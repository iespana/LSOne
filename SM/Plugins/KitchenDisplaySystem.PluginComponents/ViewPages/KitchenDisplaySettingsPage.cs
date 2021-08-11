using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls.ScreenIdentity;
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
    internal partial class KitchenDisplaySettingsPage : UserControl, ITabView
    {
        KitchenDisplayStation kds;

        public KitchenDisplaySettingsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.KitchenDisplaySettingsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            kds = (KitchenDisplayStation)internalContext;

            cmbKdsFunctionalProfile.SelectedData = new DataEntity(kds.KitchenDisplayFunctionalProfileId, kds.KitchenDisplayFunctionalProfileDescription);
            cmbKdsStyleProfile.SelectedData = new DataEntity(kds.KitchenDisplayStyleProfileId, kds.KitchenDisplayStyleProfileDescription);
            cmbKdsVisualProfile.SelectedData = new DataEntity(kds.KitchenDisplayVisualProfileId, kds.KitchenDisplayVisualProfileDescription);
            cmbNextStation.SelectedData = new DataEntity(kds.NextStationId, kds.NextStationName);
            cmbScreenNumber.SelectedIndex = (int)kds.ScreenNumber;
            cmbKdsDisplayProfile.SelectedData = new DataEntity(kds.KitchenDisplayProfileId, kds.KitchenDisplayProfileDescription);
            tbMaxRecallFiles.Text = kds.RecallFilesMax.ToString();
            chkFullScreen.Checked = kds.FullScreen;
            cmbHorizontalLocation.SelectedIndex = (int)kds.HorizontalPosition;
            cmbVerticalLocation.SelectedIndex = (int)kds.VerticalPosition;
            chkBumpedPrevoius.Checked = kds.ShowOnlyWhenBumpedOnPriorStations;
            cmbTransferStation.SelectedData = new DataEntity(kds.TransferToStationID, kds.TransferToStationName);
            chkBumpbar.Checked = kds.DiscoverBumpBar;
            cmbBumpbarOperation.SelectedData = new DataEntity((int)kds.DiscoverKey, KitchenDisplayButton.GetButtonText(kds.DiscoverKey));

            cmbKdsAggregateProfile.SelectedData = Providers.KitchenDisplayAggregateProfileData.Get(PluginEntry.DataModel, kds.AggregateProfileId);
            if(cmbKdsAggregateProfile.SelectedData == null)
            {
                cmbKdsAggregateProfile.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            }

            chkBumpbar_CheckedChanged(null, null);
        }

        public bool DataIsModified()
        {
            if (cmbKdsFunctionalProfile.SelectedData.ID != kds.KitchenDisplayFunctionalProfileId) return true;
            if (cmbKdsStyleProfile.SelectedData.ID != kds.KitchenDisplayStyleProfileId) return true;
            if (cmbKdsVisualProfile.SelectedData.ID != kds.KitchenDisplayVisualProfileId) return true;
            if (cmbNextStation.SelectedData.ID != kds.NextStationId) return true;
            if (cmbScreenNumber.SelectedIndex != (int)kds.ScreenNumber) return true;
            if (cmbKdsDisplayProfile.SelectedData.ID != kds.KitchenDisplayProfileId) return true;
            if (cmbKdsAggregateProfile.SelectedData.ID != kds.AggregateProfileId) return true;
            if (tbMaxRecallFiles.Text != kds.RecallFilesMax.ToString()) return true;
            if (chkFullScreen.Checked != kds.FullScreen) return true;
            if (cmbHorizontalLocation.SelectedIndex != (int)kds.HorizontalPosition) return true;
            if (cmbVerticalLocation.SelectedIndex != (int)kds.VerticalPosition) return true;
            if (cmbTransferStation.SelectedData.ID != kds.TransferToStationID) return true;
            if (chkBumpedPrevoius.Checked != kds.ShowOnlyWhenBumpedOnPriorStations) return true;
            if (chkBumpbar.Checked != kds.DiscoverBumpBar) return true;
            if ((int)cmbBumpbarOperation.SelectedData.ID != (int)kds.DiscoverKey) return true;

            return false;
        }

        public bool SaveData()
        {
            kds.KitchenDisplayFunctionalProfileId = (Guid)cmbKdsFunctionalProfile.SelectedData.ID;
            kds.KitchenDisplayStyleProfileId = (Guid)cmbKdsStyleProfile.SelectedData.ID;
            kds.KitchenDisplayVisualProfileId = (Guid)cmbKdsVisualProfile.SelectedData.ID;
            kds.NextStationId = (string)cmbNextStation.SelectedData.ID;
            kds.ScreenNumber = (KitchenDisplayStation.ScreenNumberEnum)cmbScreenNumber.SelectedIndex;
            kds.KitchenDisplayProfileId = (Guid)cmbKdsDisplayProfile.SelectedData.ID;
            kds.AggregateProfileId = (string)cmbKdsAggregateProfile.SelectedData.ID;

            int fileMax;
            if (Int32.TryParse(tbMaxRecallFiles.Text, out fileMax))
            {
                kds.RecallFilesMax = fileMax;
            }
            else
            {
                kds.RecallFilesMax = 100;
            }
            kds.FullScreen = chkFullScreen.Checked;
            kds.HorizontalPosition = (KitchenDisplayStation.HorizontalPositionEnum)cmbHorizontalLocation.SelectedIndex;
            kds.VerticalPosition = (KitchenDisplayStation.VerticalPositionEnum)cmbVerticalLocation.SelectedIndex;
            kds.ShowOnlyWhenBumpedOnPriorStations = chkBumpedPrevoius.Checked;
            kds.TransferToStationID = (string)cmbTransferStation.SelectedData.ID;
            kds.DiscoverBumpBar = chkBumpbar.Checked;
            kds.DiscoverKey = (KitchenDisplayButton.ButtonActionEnum)(int)cmbBumpbarOperation.SelectedData.ID;

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

        private void cmbKdsFunctionalProfile_RequestData(object sender, System.EventArgs e)
        {
            var listOfFunctionalProfiles = Providers.KitchenDisplayFunctionalProfileData.GetList(PluginEntry.DataModel);
            cmbKdsFunctionalProfile.SetData(listOfFunctionalProfiles, null);
        }

        private void cmbKdsStyleProfile_RequestData(object sender, System.EventArgs e)
        {
            var listOfStyleProfiles = Providers.KitchenDisplayStyleProfileData.GetList(PluginEntry.DataModel);
            cmbKdsStyleProfile.SetData(listOfStyleProfiles, null);
        }

        private void cmbKdsVisualProfile_RequestData(object sender, System.EventArgs e)
        {
            var listOfVisualProfiles = Providers.KitchenDisplayVisualProfileData.GetList(PluginEntry.DataModel);
            cmbKdsVisualProfile.SetData(listOfVisualProfiles, null);
        }

        private void cmbNextStation_RequestData(object sender, EventArgs e)
        {
            var listOfStations = Providers.KitchenDisplayStationData.GetList(PluginEntry.DataModel);
            var filteredStations = new List<KitchenDisplayStation>(listOfStations.Count);
            foreach (var station in listOfStations)
            {
                if (station.ID != kds.ID)
                    filteredStations.Add(station);
            }
            cmbNextStation.SetData(filteredStations, null);
        }

        private void cmbKdsDisplayProfile_RequestData(object sender, EventArgs e)
        {
            var listOfDisplayProfiles = Providers.KitchenDisplayProfileData.GetList(PluginEntry.DataModel);
            cmbKdsDisplayProfile.SetData(listOfDisplayProfiles, null);
        }

        private void cmbKdsAggregateProfile_RequestData(object sender, EventArgs e)
        {
            var listOfAggregateProfiles = Providers.KitchenDisplayAggregateProfileData.GetList(PluginEntry.DataModel);
            cmbKdsAggregateProfile.SetData(listOfAggregateProfiles, null);
        }

        private void btnsFunctionalProfile_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.NewFunctionalProfileDialog();
            dlg.ShowDialog();
        }

        private void btnsFunctionalProfile_EditButtonClicked(object sender, EventArgs e)
        {
            var profileId = cmbKdsFunctionalProfile.SelectedData.ID;
            PluginOperationsHelper.ShowFunctionalProfilesView(profileId);
        }

        private void btnStyleProfile_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.NewStyleProfileDialog();
            dlg.ShowDialog();
        }

        private void btnStyleProfile_EditButtonClicked(object sender, EventArgs e)
        {
            var profileId = cmbKdsStyleProfile.SelectedData.ID;
            PluginOperationsHelper.ShowStyleProfilesView(profileId);
        }

        private void btnsVisualProfile_EditButtonClicked(object sender, EventArgs e)
        {
            var profileId = cmbKdsVisualProfile.SelectedData.ID;
            PluginOperationsHelper.ShowVisualProfilesView(profileId);
        }

        private void btnsVisualProfile_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.NewVisualProfileDialog();
            dlg.ShowDialog();
        }

        private void btnsDisplayProfile_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.NewDisplayProfileDialog();
            dlg.ShowDialog();
        }

        private void btnsAggregateProfile_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowAggregateProfileDialog();
        }

        private void btnsDisplayProfile_EditButtonClicked(object sender, EventArgs e)
        {
            var profileId = cmbKdsDisplayProfile.SelectedData.ID;
            PluginOperationsHelper.ShowDisplayProfilesView(profileId);
        }

        private void btnsAggregateProfile_EditButtonClicked(object sender, EventArgs e)
        {
            var profileId = cmbKdsAggregateProfile.SelectedData.ID;
            PluginOperationsHelper.ShowAggregateProfilesView(profileId);
        }

        private void lnkIdentify_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var identifier = new ScreenIdentifier();
            identifier.Identify();
        }

        private void chkFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            bool checkState = chkFullScreen.Checked;
            lblHorizontalLocation.Enabled = !checkState;
            lblVertialLocation.Enabled = !checkState;
            cmbHorizontalLocation.Enabled = !checkState;
            cmbVerticalLocation.Enabled = !checkState;
        }

        private void cmbNextStation_RequestClear(object sender, EventArgs e)
        {
            cmbNextStation.SelectedData = new DataEntity();
        }

        private void cmbStationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var stationType = (KitchenDisplayStation.StationTypeEnum) cmbStationType.SelectedIndex;
            //switch (stationType)
            //{
            //        case KitchenDisplayStation.StationTypeEnum.ExpediterStation:
            //        case KitchenDisplayStation.StationTypeEnum.CustomerFacingStation:
            //            cmbKdsDisplayProfile.SelectedItem = KitchenDisplayStation.GetDisplayModeText(KitchenDisplayStation.DisplayModeEnum.ChitDisplay);
            //            cmbKdsDisplayProfile.Enabled = false;
            //            break;
            //        case KitchenDisplayStation.StationTypeEnum.PrepStation:
            //            cmbKdsDisplayProfile.Enabled = true;
            //            break;
            //}
        }

        private void cmbTransferStation_RequestClear(object sender, EventArgs e)
        {
            cmbTransferStation.SelectedData = new DataEntity();
        }

        private void cmbTransferStation_RequestData(object sender, EventArgs e)
        {
            var listOfStations = Providers.KitchenDisplayStationData.GetList(PluginEntry.DataModel);
            var filteredStations = new List<KitchenDisplayStation>(listOfStations.Count);
            foreach (var station in listOfStations)
            {
                if (station.ID != kds.ID)
                    filteredStations.Add(station);
            }
            cmbTransferStation.SetData(filteredStations, null);
        }

        private void cmbBumpbarOperation_RequestData(object sender, EventArgs e)
        {
            KitchenDisplayFunctionalProfile functionalProfile = Providers.KitchenDisplayFunctionalProfileData.Get(PluginEntry.DataModel, cmbKdsFunctionalProfile.SelectedData.ID);
            if (functionalProfile == null) return;

            List<int> operations = Providers.PosMenuLineData.GetOperations(PluginEntry.DataModel, functionalProfile.ButtonsMenuId);
            List<DataEntity> operationEntities = operations.ConvertAll(o => new DataEntity(o, KitchenDisplayButton.GetButtonText((KitchenDisplayButton.ButtonActionEnum)o)));
            cmbBumpbarOperation.SetData(operationEntities, null);
        }

        private void chkBumpbar_CheckedChanged(object sender, EventArgs e)
        {
            if(chkBumpbar.Checked)
            {
                cmbBumpbarOperation.Enabled = true;
            }
            else
            {
                cmbBumpbarOperation.Enabled = false;

                DataEntity noOperation = new DataEntity((int)KitchenDisplayButton.ButtonActionEnum.NoOperation, KitchenDisplayButton.GetButtonText(KitchenDisplayButton.ButtonActionEnum.NoOperation));
                cmbBumpbarOperation.SelectedData = noOperation;
            }
        }

        private void cmbKdsFunctionalProfile_SelectedDataChanged(object sender, EventArgs e)
        {
            cmbBumpbarOperation.SelectedData = new DataEntity(0, "");
        }
    }
}
