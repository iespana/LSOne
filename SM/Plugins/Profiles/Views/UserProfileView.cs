using System.Collections.Generic;
using LSOne.ViewCore;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.ViewCore.EventArguments;
using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.Profiles.Views
{
    public partial class UserProfileView : ViewBase
    {
        private RecordIdentifier userProfileID;
        private UserProfile userProfile;
        private bool mainRecordIsDirty;

        public UserProfileView(RecordIdentifier userProfileID)
        {
            InitializeComponent();
            this.userProfileID = userProfileID;

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageUserProfiles);

            TabControl.Tab discountsTab = new TabControl.Tab(Properties.Resources.DiscountSettings, ViewPages.User.UserProfileDiscountsPage.CreateInstance);
            TabControl.Tab otherTab = new TabControl.Tab(Properties.Resources.Other, ViewPages.User.UserProfileOtherPage.CreateInstance);
            TabControl.Tab usersTab = new TabControl.Tab(Properties.Resources.Users, ViewPages.User.UserProfileUsersPage.CreateInstance);
            tabSheetTabs.AddTab(discountsTab);
            tabSheetTabs.AddTab(otherTab);
            tabSheetTabs.AddTab(usersTab);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("UserProfile", userProfileID, Properties.Resources.UserProfile, true));

            tabSheetTabs.GetAuditContexts(contexts);
        }

        public override string ContextDescription
        {
            get
            {
                return tbDescription.Text;
            }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.UserProfile + ": " + userProfileID + " - " + tbDescription.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.UserProfile;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return userProfileID;
            }
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();
            base.OnClose();
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.UserProfiles, PluginOperations.ShowUserProfilesSheet), 100);
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "UserProfile":
                    LoadData(true);
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        protected override void LoadData(bool isRevert)
        {
            userProfile = Providers.UserProfileData.Get(PluginEntry.DataModel, userProfileID);

            if (!isRevert)
            {
                tabSheetTabs.Broadcast(this, userProfileID, userProfile);
            }

            tbID.Text = (string)userProfile.ID;
            tbDescription.Text = userProfile.Text;

            HeaderText = Description;

            if (userProfile.ProfileIsUsed && (Attributes & ViewAttributes.Delete) == ViewAttributes.Delete)
            {
                Attributes &= ~ViewAttributes.Delete;
            }

            tabSheetTabs.SetData(isRevert, userProfileID, userProfile);
        }

        protected override bool DataIsModified()
        {
            mainRecordIsDirty = true;

            if (tbDescription.Text != userProfile.Text) return true;
            if (tabSheetTabs.IsModified()) return true;

            mainRecordIsDirty = false;

            return false;
        }

        protected override bool SaveData()
        {
            userProfile.Text = tbDescription.Text;

            if (mainRecordIsDirty)
            {
                tabSheetTabs.GetData();
            }

            Providers.UserProfileData.Save(PluginEntry.DataModel, userProfile);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "UserProfile", userProfile.ID, null);
            return true;
        }
    }
}
