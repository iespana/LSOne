using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.Profiles.Views
{
    public partial class VisualProfileView : ViewBase
    {
        private readonly RecordIdentifier profileID;
        private VisualProfile visualProfile;
        private bool initialInitialize;

        private TabControl.Tab generalTab;
        private TabControl.Tab receiptSettingsTab;
        private TabControl.Tab colorPaletteTab;

        public VisualProfileView(RecordIdentifier profileID)
            : this()
        {
            this.profileID = profileID;
        }

        private VisualProfileView()
        {
            InitializeComponent();

            Attributes =
                ViewAttributes.Close |
                ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.Help |
                ViewAttributes.ContextBar;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.VisualProfileEdit);            
        }        

        protected override void LoadData(bool isRevert)
        {
            if (!initialInitialize)
            {
                AddParentViewDescriptor(new ParentViewDescriptor(
                        profileID,
                        Properties.Resources.VisualProfiles,
                        null,
                        PluginOperations.ShowVisualProfilesSheet));

                initialInitialize = true;
            }

            HeaderText = Description;

            visualProfile = Providers.VisualProfileData.Get(PluginEntry.DataModel, profileID);

            tbID.Text = (string)visualProfile.ID;
            tbDescription.Text = visualProfile.Text;                                    

            if (visualProfile.ProfileIsUsed && (Attributes & ViewAttributes.Delete) == ViewAttributes.Delete)
            {
                Attributes &= ~ViewAttributes.Delete;
            }

            if(!isRevert)
            {
                generalTab = new TabControl.Tab(Properties.Resources.General, ViewPages.Visual.VisualProfileGeneralPage.CreateInstance);
                receiptSettingsTab = new TabControl.Tab(Properties.Resources.ReceiptSettings, ViewPages.Visual.VisualProfileReceiptSettingsPage.CreateInstance);
                colorPaletteTab = new TabControl.Tab(Properties.Resources.ColorPalette, ViewPages.Visual.VisualProfileColorPalettePage.CreateInstance);

                tabSheetTabs.AddTab(generalTab);
                tabSheetTabs.AddTab(receiptSettingsTab);
                tabSheetTabs.AddTab(colorPaletteTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, profileID);
            }

            tabSheetTabs.SetData(isRevert, profileID, visualProfile);
        }

        

        protected override bool DataIsModified()
        {
            if (tbDescription.Text != visualProfile.Text) return true;

            if (tabSheetTabs.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            visualProfile.Text = tbDescription.Text;

            tabSheetTabs.GetData();

            Providers.VisualProfileData.Save(PluginEntry.DataModel, visualProfile);

            PluginEntry.Framework.ViewController.NotifyDataChanged(
                this,
                DataEntityChangeType.Edit,
                "VisualProfile",
                visualProfile.ID,
                null);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "VisualProfile":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == profileID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("VisualProfile", profileID, Properties.Resources.VisualProfile, true));
        }
        
        protected override void OnDelete()
        {
            PluginOperations.DeleteVisualProfile(profileID);
        }

        public override RecordIdentifier ID
        {
            get
            {
                // If our sheet would be multi-instance sheet then we would return context identifier UUID here,
                // such as User.GUID that identifies that particular User. For single instance sheets we return 
                // RecordIdentifier.Empty to tell the framework that there can only be one instace of this sheet, which will
                // make the framework make sure there is only one instance in the viewstack.
                return profileID;
            }
        }      

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.VisualProfile;
            }
        }

        private string Description
        {
            get
            {
                return Properties.Resources.VisualProfile + ": " + (string)profileID;
            }
        }                        
    }
}
