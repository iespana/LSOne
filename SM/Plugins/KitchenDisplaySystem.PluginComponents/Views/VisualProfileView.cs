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
    public partial class VisualProfileView : ViewBase
    {
        private RecordIdentifier profileId;
        private KitchenDisplayVisualProfile kitchenDisplayVisualProfile;

        public VisualProfileView(RecordIdentifier profileId)
            : this()
        {
            this.profileId = profileId;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles);
        }

        private VisualProfileView()
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
            contexts.Add(new AuditDescriptor("KITCHENDISPLAYVISUALPROFILELog", profileId, Properties.Resources.VisualProfiles, true));

        }

        public string Description
        {
            get
            {
                return Properties.Resources.VisualProfile + ": " + kitchenDisplayVisualProfile.Text;
            }
        }

        public override string ContextDescription
        {
            get
            {
                return kitchenDisplayVisualProfile.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.VisualProfile + ": " + kitchenDisplayVisualProfile.Text;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return profileId;
            }
        }       

        protected override void LoadData(bool isRevert)
        {
            kitchenDisplayVisualProfile = Providers.KitchenDisplayVisualProfileData.Get(PluginEntry.DataModel, profileId, null);

            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.General, 
                    ViewPages.VisualProfileGeneralPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.Layout,
                    ViewPages.VisualProfileLayoutPage.CreateInstance));

                tabSheetTabs.Broadcast(this, kitchenDisplayVisualProfile.ID, kitchenDisplayVisualProfile);
            }

            tbProfileName.Text = kitchenDisplayVisualProfile.Text;
            HeaderText = Description;

            tabSheetTabs.SetData(isRevert, profileId, kitchenDisplayVisualProfile);
        }

        protected override bool DataIsModified()
        {
            if (tbProfileName.Text != kitchenDisplayVisualProfile.Text) return true;
            if (tabSheetTabs.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            kitchenDisplayVisualProfile.Text = tbProfileName.Text;
            tabSheetTabs.GetData();

            Providers.KitchenDisplayVisualProfileData.Save(PluginEntry.DataModel, kitchenDisplayVisualProfile);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "KitchenDisplayVisualProfile", profileId, null);

            return true;
        }

        protected override void OnDelete()
        {
            PluginOperationsHelper.DeleteVisualProfile(profileId);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenDisplayVisualProfile":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == profileId)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;

            }
        }

    }
}
