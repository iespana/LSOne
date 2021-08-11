using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.SiteService.ViewPages;

namespace LSOne.ViewPlugins.SiteService.Views
{
    public partial class SiteServiceProfileView : ViewBase
    {
        private RecordIdentifier profileID;
        private SiteServiceProfile profile;
        private bool initialInitialize = false;

        public SiteServiceProfileView(RecordIdentifier profileID)
            : this()
        {
            this.profileID = profileID;
        }

        public SiteServiceProfileView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar;

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileEdit);

            
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("TransactionServiceProfile", profileID, Properties.Resources.StoreServerProfile, true));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.StoreServerProfile;
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
                return Properties.Resources.StoreServerProfile + ": " + (string)profileID;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Connection, ConnectionPage.CreateInstance));
                //tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Settings, SettingsPage.CreateInstance));
                //tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.CentralReturns, CentralReturnsPage.CreateInstance));

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, profileID);
            }

            if(initialInitialize == false)
            {
                AddParentViewDescriptor(new ParentViewDescriptor(
                        profileID,
                        Properties.Resources.StoreServerProfiles,
                        null, //Properties.Resources.Profiles16,
                        new ShowParentViewHandler(PluginOperations.ShowTransactionServiceProfilesSheet)));

                initialInitialize = true;
            }

            HeaderText = Description;
            //HeaderIcon = Properties.Resources.Profiles16;

            profile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, profileID);

            tbID.Text = (string)profile.ID;
            tbDescription.Text = profile.Text;

            if (profile.ProfileIsUsed && (Attributes & ViewAttributes.Delete) == ViewAttributes.Delete)
            {
                Attributes &= ~ViewAttributes.Delete;
            }

            tabSheetTabs.SetData(isRevert, profileID, profile);
        }

        protected override bool DataIsModified()
        {

            if (tbDescription.Text != profile.Text) return true;
      
            if (tabSheetTabs.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            profile.Text = tbDescription.Text;

            tabSheetTabs.GetData();

            Providers.SiteServiceProfileData.Save(PluginEntry.DataModel, profile);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "TransactionServiceProfile", profile.ID, profile);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "TransactionServiceProfile":
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
            PluginOperations.DeleteTransactionServiceProfile(profileID);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }
    }
}
