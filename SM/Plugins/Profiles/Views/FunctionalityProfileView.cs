using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Profiles.Properties;
using LSOne.ViewPlugins.Profiles.ViewPages.Functionality;

namespace LSOne.ViewPlugins.Profiles.Views
{
    public partial class FunctionalityProfileView : ViewBase
    {
        private TabControl.Tab terminalTab;
        private TabControl.Tab itemsTab;
        private TabControl.Tab staffTab;
        private TabControl.Tab startOfDayTab;
        private TabControl.Tab replicationTab;

        private RecordIdentifier profileID;
        private FunctionalityProfile functionalityProfile;
        private bool initialInitialize = false;

        public FunctionalityProfileView(RecordIdentifier profileID)
            : this()
        {
            this.profileID = profileID;
        }

        private FunctionalityProfileView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.FunctionalProfileEdit);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("FunctionalityProfile", profileID, Properties.Resources.FunctionalityProfile, true));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.FunctionalityProfile;
            }
        }

        public override RecordIdentifier ID
        {
            get{return profileID;}
        }

        public string Description
        {
            get
            {
                return Properties.Resources.FunctionalityProfile + ": " + (string)profileID;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                functionalityProfile = Providers.FunctionalityProfileData.Get(PluginEntry.DataModel, profileID);

                terminalTab = new TabControl.Tab(Properties.Resources.Terminal, FunctionalProfileTerminalPage.CreateInstance);
                itemsTab = new TabControl.Tab(Properties.Resources.Items, FunctionalProfileItemsPage.CreateInstance);
                staffTab = new TabControl.Tab(Properties.Resources.Staff, FunctionalProfileStaffPage.CreateInstance);
                startOfDayTab = new TabControl.Tab(Properties.Resources.StartOfDay, FunctionalProfileStartOfDay.CreateInstance);
                replicationTab = new TabControl.Tab(Resources.Replication, FunctionalProfileReplicationPage.CreateInstance);

                tabSheetTabs.AddTab(terminalTab);
                tabSheetTabs.AddTab(itemsTab);
                tabSheetTabs.AddTab(staffTab);
                tabSheetTabs.AddTab(startOfDayTab);
                tabSheetTabs.AddTab(replicationTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, 0);
            }

            if(initialInitialize == false)
            {
                AddParentViewDescriptor(new ParentViewDescriptor(
                        profileID,
                        Properties.Resources.FunctionalityProfiles,
                        null, //Properties.Resources.Profiles16,
                        PluginOperations.ShowFunctionalityProfilesSheet));

                initialInitialize = true;
            }

            HeaderText = Description;
            //HeaderIcon = Properties.Resources.Profiles16;

            tbID.Text = (string)functionalityProfile.ID;
            tbDescription.Text = functionalityProfile.Text;

            if (functionalityProfile.ProfileIsUsed && (Attributes & ViewAttributes.Delete) == ViewAttributes.Delete)
            {
                Attributes &= ~ViewAttributes.Delete;
            }

            tabSheetTabs.SetData(isRevert, RecordIdentifier.Empty, functionalityProfile);
        }

        protected override bool DataIsModified()
        {

            if (tbDescription.Text != functionalityProfile.Text) return true;

            if (tabSheetTabs.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            functionalityProfile.Text = tbDescription.Text;

            tabSheetTabs.GetData();

            Providers.FunctionalityProfileData.Save(PluginEntry.DataModel, functionalityProfile);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "FunctionalityProfile", functionalityProfile.ID, null);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "FunctionalityProfile":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == profileID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteFunctionalityProfile(profileID);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }
    }
}
