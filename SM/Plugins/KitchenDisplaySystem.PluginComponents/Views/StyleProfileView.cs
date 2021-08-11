using System.Collections.Generic;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class StyleProfileView : ViewBase
    {
        private RecordIdentifier profileId;
        private LSOneKitchenDisplayStyleProfile kitchenDisplayStyleProfile;

        public StyleProfileView(RecordIdentifier profileId)
            : this()
        {
            this.profileId = profileId;
            kitchenDisplayStyleProfile = Providers.KitchenDisplayStyleProfileData.Get(PluginEntry.DataModel, profileId);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles);
        }

        private StyleProfileView()
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
            //contexts.Add(new AuditDescriptor("KITCHENDISPLAYStylePROFILELog", profileId, Properties.Resources.KitchenDisplayStyleProfiles, true));

        }

        public string Description
        {
            get
            {
                return Properties.Resources.StyleProfile + ": " + kitchenDisplayStyleProfile.Text;
            }
        }

        public override string ContextDescription
        {
            get
            {
                return kitchenDisplayStyleProfile.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.StyleProfile + ": " + kitchenDisplayStyleProfile.Text;
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
            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.Order,
                    ViewPages.StyleProfileOrderPage.CreateInstance));

                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.Items, 
                    ViewPages.StyleProfileItemsPage.CreateInstance));

                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.Modifiers,
                    ViewPages.StyleProfileModifiersPage.CreateInstance));

                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.HeaderFooter,
                    ViewPages.StyleProfileHeaderFooterPage.CreateInstance));

                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.AggregateAndHistory,
                    ViewPages.StyleProfileAggregateHistoryPage.CreateInstance));

                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.Buttons,
                    ViewPages.StyleProfileButtonsPage.CreateInstance));
            }

            tbProfileName.Text = kitchenDisplayStyleProfile.Text;
            HeaderText = Description;

            tabSheetTabs.SetData(isRevert, profileId, kitchenDisplayStyleProfile);
        }

        protected override bool DataIsModified()
        {
            if (tbProfileName.Text != kitchenDisplayStyleProfile.Text) return true;
            if (tabSheetTabs.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            kitchenDisplayStyleProfile.Text = tbProfileName.Text;
            tabSheetTabs.GetData();

            Providers.KitchenDisplayStyleProfileData.Save(PluginEntry.DataModel, kitchenDisplayStyleProfile);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "KitchenDisplayStyleProfile", profileId, null);

            return true;
        }

        protected override void OnDelete()
        {
            PluginOperationsHelper.DeleteStyleProfile(profileId);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenDisplayStyleProfile":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == profileId)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }


        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (PluginEntry.Framework.CanRunOperation("UIStyles"))
            {
                if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
                    {
                        arguments.Add(new ContextBarItem(
                            Properties.Resources.Styles,
                            null,
                            PluginOperationsHelper.ShowStylesView), 100);
                    }
                }
            }
        }
    }
}
