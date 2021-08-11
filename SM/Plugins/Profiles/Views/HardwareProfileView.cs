using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Profiles.ViewPages.Hardware;

namespace LSOne.ViewPlugins.Profiles.Views
{
    public partial class HardwareProfileView : ViewBase
    {
        private RecordIdentifier profileID;
        private HardwareProfile hardwareProfile;
        private bool initialInitialize = false;

        public HardwareProfileView(RecordIdentifier profileID)
            : this()
        {
            this.profileID = profileID;
        }

        private HardwareProfileView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.HardwareProfileEdit);

            
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("HardwareProfile", profileID, Properties.Resources.HardwareProfile, true));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.HardwareProfile;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return profileID;
            }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.HardwareProfile + ": " + (string)profileID;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Drawer, HardwareProfileDrawerPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.LineDisplay, HardwareProfileLineDisplayPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.DualDisplay, HardwareProfileDualDisplayPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.CardReader, HardwareProfileCardReaderPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Printer, HardwareProfilePrinterPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.BarcodeReader, HardwareProfileBarcodReaderPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Scale, HardwareProfileScalePage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Key, HardwareProfileKeyPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.DallasKey, HardwareProfileDallasKeyPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.CreditCardService, HardwareProfileCardServicePage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.SurveillanceCamera, HardwareProfileCameraPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.PumpConnection, HardwareProfilePumpPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.RFIDReader, HardwareProfileRFIDPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.CashChanger, HardwareProfileCashChangerPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.ForecourtManager, HardwareProfileForecourtManagerPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.FiscalPrinter, HardwareProfileFiscalPrinterPage.CreateInstance));

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, profileID);
            }

            if(!initialInitialize)
            {
                AddParentViewDescriptor(new ParentViewDescriptor(
                           profileID,
                           Properties.Resources.HardwareProfiles,
                           null, //Properties.Resources.Profiles16,
                           PluginOperations.ShowHardwareProfilesSheet));


                initialInitialize = true;
            }

            HeaderText = Description;
            //HeaderIcon = Properties.Resources.Profiles16;

            hardwareProfile = Providers.HardwareProfileData.Get(PluginEntry.DataModel, profileID);

            tbID.Text = (string)hardwareProfile.ID;
            tbDescription.Text = hardwareProfile.Text;

            if (hardwareProfile.ProfileIsUsed && (Attributes & ViewAttributes.Delete) == ViewAttributes.Delete)
            {
                Attributes &= ~ViewAttributes.Delete;
            }

            tabSheetTabs.SetData(isRevert, profileID, hardwareProfile);
        }

        protected override bool DataIsModified()
        {
            if (tbDescription.Text != hardwareProfile.Text) return true;

            if (tabSheetTabs.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            hardwareProfile.Text = tbDescription.Text;

            tabSheetTabs.GetData();

            Providers.HardwareProfileData.Save(PluginEntry.DataModel,hardwareProfile);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "HardwareProfile", hardwareProfile.ID,null);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "HardwareProfile":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == profileID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteHardwareProfile(profileID);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }       
    }
}
