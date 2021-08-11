using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class KitchenServiceProfileView : ViewBase
    {
        private RecordIdentifier profileID;
        private KitchenServiceProfile profile;

        public KitchenServiceProfileView(RecordIdentifier profileID)
            : this()
        {
            this.profileID = profileID;
        }

        private KitchenServiceProfileView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar;

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenServiceProfiles);

            
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("KMPROFILE", profileID, Properties.Resources.KitchenServiceProfile, true));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.KitchenServiceProfile;
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
                return Properties.Resources.KitchenServiceProfile + ": " + profile.Text;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Connection, ViewPages.KitchenServiceProfileConnectionPage.CreateInstance));

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, profileID);
            }

            profile = Providers.KitchenDisplayTransactionProfileData.Get(PluginEntry.DataModel, profileID);
            HeaderText = Description;

            tbDescription.Text = profile.Text;

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

            Providers.KitchenDisplayTransactionProfileData.Save(PluginEntry.DataModel, profile);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "KitchenServiceProfile", profile.ID, profile);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenServiceProfile":
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
            PluginOperationsHelper.DeleteKitchenServiceProfile(profileID);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "KitchenServiceProfile", profileID, null);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }
    }
}
