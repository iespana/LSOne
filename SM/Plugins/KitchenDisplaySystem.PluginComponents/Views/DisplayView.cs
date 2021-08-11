using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class DisplayView : ViewBase
    {
        private RecordIdentifier kitchenDisplayStationId;
        private KitchenDisplayStation kitchenDisplayStation;


        public DisplayView(RecordIdentifier kitchenDisplayStationId)
            : this()
        {
            this.kitchenDisplayStationId = kitchenDisplayStationId;
            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations);

            cmbStationType.Items.Clear();
            foreach (var mode in Enum.GetValues(typeof(KitchenDisplayStation.StationTypeEnum)))
            {
                if ((KitchenDisplayStation.StationTypeEnum)mode != KitchenDisplayStation.StationTypeEnum.UpcomingOrdersStation)
                {
                    cmbStationType.Items.Add(
                        KitchenDisplayStation.GetStationTypeText((KitchenDisplayStation.StationTypeEnum)mode));
                }
            }
        }

        private DisplayView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;
            
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("KITCHENDISPLAYSTATIONLog", kitchenDisplayStationId, Properties.Resources.DisplayStation, true));
            contexts.Add(new AuditDescriptor("KITCHENDISPLAYITEMCONNECTIONLog", kitchenDisplayStationId, Properties.Resources.ItemConnection, true));
            contexts.Add(new AuditDescriptor("KITCHENDISPLAYTERMINALCONNECTIONLog", kitchenDisplayStationId, Properties.Resources.TerminalConnection, true));
            contexts.Add(new AuditDescriptor("KITCHENDISPLAYHOSPITALITYCONNECTIONLog", kitchenDisplayStationId, Properties.Resources.HospitalityConnection, true));
        }

        public string Description
        {
            get
            {
                return Properties.Resources.DisplayStation + ": " + kitchenDisplayStationId;
            }
        }

        public override string ContextDescription
        {
            get
            {
                return tbStationName.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.DisplayStation;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return kitchenDisplayStationId;
            }
        }       

        protected override void LoadData(bool isRevert)
        {
            kitchenDisplayStation = Providers.KitchenDisplayStationData.Get(PluginEntry.DataModel, kitchenDisplayStationId);

            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.Settings,
                    ViewPages.KitchenDisplaySettingsPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.Items,
                    ViewPages.ItemRoutingPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.Terminals,
                    ViewPages.TerminalRoutingPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.HospitalityTypes,
                    ViewPages.HospitalityTypeRoutingPage.CreateInstance));

                tabSheetTabs.Broadcast(this, kitchenDisplayStation.ID, kitchenDisplayStation);
            }

            tbID.Text = (string)kitchenDisplayStation.ID;
            tbStationName.Text = kitchenDisplayStation.Text;
            cmbStationType.SelectedIndex = (int)kitchenDisplayStation.StationType;
            tbStationLetter.Text = kitchenDisplayStation.StationLetter;

            tabSheetTabs.SetData(isRevert, kitchenDisplayStation.ID, kitchenDisplayStation);

            HeaderText = Description;
        }

        protected override bool DataIsModified()
        {

            if (tbID.Text != (string)kitchenDisplayStation.ID) return true;
            if (tbStationName.Text != kitchenDisplayStation.Text) return true;
            if (cmbStationType.SelectedIndex != (int)kitchenDisplayStation.StationType) return true;
            if (tbStationLetter.Text != kitchenDisplayStation.StationLetter) return true;
            if (tabSheetTabs.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            kitchenDisplayStation.Text = tbStationName.Text;
            kitchenDisplayStation.StationType = (KitchenDisplayStation.StationTypeEnum)cmbStationType.SelectedIndex;
            kitchenDisplayStation.StationLetter = tbStationLetter.Text;
            tabSheetTabs.GetData();

            Providers.KitchenDisplayStationData.Save(PluginEntry.DataModel, kitchenDisplayStation);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "KitchenDisplayStation", kitchenDisplayStationId, null);

            return true;
        }

        protected override void OnDelete()
        {
            PluginOperationsHelper.DeleteDisplayStation(kitchenDisplayStationId);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenDisplayStation":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == kitchenDisplayStationId)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }
    }
}