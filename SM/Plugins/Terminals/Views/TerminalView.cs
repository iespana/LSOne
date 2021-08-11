using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Terminals.Views
{
    public partial class TerminalView : ViewBase
    {
        private string terminalID;
        private string storeID;
        private Terminal terminal;
        private IPlugin storeEditior = null;
        private IPlugin visualProfileEditor = null;
        private IPlugin hardwareProfileEditor = null;
        private IPlugin layoutEditor = null;
        private IPlugin functionalityProfileEditor;
        private TabControl.Tab settingsTab;
        private TabControl.Tab eftTab;
        private TabControl.Tab lsPayTab;
        private TabControl.Tab customerDisplayTab;
        private EftType eftType = EftType.None;

        private enum EftType
        {
            None,
            LsPay,
            Other
        };

        public TerminalView(string terminalID, string storeID)
            :this()
        {
            this.terminalID = terminalID;
            this.storeID = storeID;
        }

        private TerminalView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;

            if (PluginEntry.DataModel.CurrentStoreID != RecordIdentifier.Empty)
            {
                cmbStore.Enabled = false;
            }

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.TerminalEdit);
        }

        public override string ContextDescription
        {
            get
            {
                return tbDescription.Text;
            }
        }      

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Terminal", terminalID, Properties.Resources.TerminalText, true));
        }

        public string Description
        {
            get
            {
                return Properties.Resources.TerminalText + ": " + terminalID + " - " + tbDescription.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.TerminalText;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        {
                return terminalID;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            terminal = Providers.TerminalData.Get(PluginEntry.DataModel, terminalID, storeID);

            this.ReadOnly = !(PluginEntry.DataModel.HasPermission(Permission.TerminalEdit) &&
                (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty || PluginEntry.DataModel.CurrentStoreID == terminal.StoreID || terminal.StoreID == "" || terminal.StoreID == RecordIdentifier.Empty));

            tbID.Text = terminal.ID.ToString();
            tbDescription.Text = terminal.Text ;

            if (!isRevert)
            {
                if (PluginEntry.DataModel.HasPermission(Permission.TerminalEdit))
                {
                    settingsTab = new TabControl.Tab(Properties.Resources.Settings,
                        ViewPages.TerminalSettingsPage.CreateInstance);
                    eftTab = new TabControl.Tab(Properties.Resources.EftSettings,
                        ViewPages.TerminalEftPage.CreateInstance);
                    lsPayTab = new TabControl.Tab(Properties.Resources.LsPaySettings,
                        ViewPages.TerminalsLsPayPage.CreateInstance);
                    customerDisplayTab = new TabControl.Tab(Properties.Resources.CustomerDisplay,
                        ViewPages.TerminalCustomerDisplayPage.CreateInstance);

                    tabSheetTabs.AddTab(settingsTab);
                    tabSheetTabs.AddTab(lsPayTab);
                    tabSheetTabs.AddTab(eftTab);
                    tabSheetTabs.AddTab(customerDisplayTab);

                    cmbHardwareProfile.Enabled = true;
                    cmbFunctionalProfile.Enabled = true;
                    cmbVisualProfile.Enabled = true;
                    cmbTouchLayout.Enabled = true;
                }
                else
                {
                    cmbHardwareProfile.Enabled = false;
                    cmbFunctionalProfile.Enabled = false;
                    cmbVisualProfile.Enabled = false;
                    cmbTouchLayout.Enabled = false;
                }

                visualProfileEditor = PluginEntry.Framework.FindImplementor(this, "CanEditVisualProfiles", null);
                hardwareProfileEditor = PluginEntry.Framework.FindImplementor(this, "CanEditHardwareProfiles", null);
                functionalityProfileEditor = PluginEntry.Framework.FindImplementor(this, "CanEditFunctionalityProfiles", null);
                layoutEditor = PluginEntry.Framework.FindImplementor(this, "CanEditLayouts", null);
                storeEditior = PluginEntry.Framework.FindImplementor(this, "CanEditStore", null);

                btnEditFunctionalProfile.Visible = functionalityProfileEditor != null;
                btnEditVisualProfile.Visible = visualProfileEditor != null;
                btnEditHardwareProfiles.Visible = hardwareProfileEditor != null;
                btnEditTouchButtons.Visible = layoutEditor != null;
                btnEditStore.Visible = storeEditior != null;

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, terminalID);
            }

            ShowOrHideEftAndLsPayTabs(terminal.HardwareProfileID);

            cmbStore.SelectedData = new DataEntity(terminal.StoreID, terminal.StoreName);
            cmbVisualProfile.SelectedData = new DataEntity(terminal.VisualProfileID, terminal.VisualProfileName);
            cmbHardwareProfile.SelectedData = new DataEntity(terminal.HardwareProfileID, terminal.HardwareProfileName);
            cmbFunctionalProfile.SelectedData = new DataEntity(terminal.FunctionalityProfileID, terminal.FunctionalityProfileName);
            cmbTouchLayout.SelectedData = new DataEntity(terminal.LayoutID, terminal.LayoutName);

            btnEditStore.Enabled = terminal.StoreID != "";

            HeaderText = Description;

            tabSheetTabs.SetData(isRevert, terminalID, terminal);
        }

        protected override bool DataIsModified()
        {
            if (tbDescription.Text != terminal.Text) return true;
            if (cmbStore.SelectedData.ID != terminal.StoreID) return true;
            if (cmbVisualProfile.SelectedData.ID != terminal.VisualProfileID) return true;
            if (cmbHardwareProfile.SelectedData.ID != terminal.HardwareProfileID) return true;
            if (cmbFunctionalProfile.SelectedData.ID != terminal.FunctionalityProfileID) return true;
            if (cmbTouchLayout.SelectedData.ID != terminal.LayoutID) return true;

            if (tabSheetTabs.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            terminal.Text = tbDescription.Text;
            terminal.StoreID = cmbStore.SelectedData.ID.ToString();
            terminal.VisualProfileID = cmbVisualProfile.SelectedData.ID;
            terminal.HardwareProfileID = cmbHardwareProfile.SelectedData.ID;
            terminal.FunctionalityProfileID = cmbFunctionalProfile.SelectedData.ID;
            terminal.LayoutID = cmbTouchLayout.SelectedData.ID;

            tabSheetTabs.GetData();

            Providers.TerminalData.Save(PluginEntry.DataModel, terminal);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Terminal", terminal.ID, null);
            PluginEntry.Framework.SetDashboardItemDirty(new Guid("f58ece32-5f38-45ac-8c67-70b7a762fe8c")); // Inititial configuration dashboard item

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            // We use this one if we want to listen to changes in the Viewstack, like was there a user 
            // changed on a user sheet in the viewstack ? And it matters to our sheet ? if so then no
            // probel we catch it here and react if needed

            switch (objectName)
            {
                case "VisualProfile":
                    if ((changeHint == DataEntityChangeType.Delete || changeHint == DataEntityChangeType.Edit) && changeIdentifier == terminal.VisualProfileID)
                    {
                        LoadData(true);
                    }
                    break;

                case "HardwareProfile":
                    if ((changeHint == DataEntityChangeType.Delete || changeHint == DataEntityChangeType.Edit) && changeIdentifier == terminal.HardwareProfileID)
                    {
                        LoadData(true);
                    }
                    break;

                case "Store":
                    if ((changeHint == DataEntityChangeType.Delete || changeHint == DataEntityChangeType.Edit) && changeIdentifier == terminal.StoreID)
                    {
                        LoadData(true);
                    }
                    break;
                   
                case "Terminal":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier.PrimaryID == terminalID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        public override object Message(string message, object param)
        {
            if (message == "StoreID")
            {
                return terminal.StoreID;
            }
            else if (message == "StoreName")
            {
                return terminal.StoreName;
            }

            return null;
        }

        private void ShowStore(object sender, EventArgs args)
        {
            if (terminal.StoreID != "")
            {
                //PluginOperations.ShowStore(terminal.StoreID);
            }
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            cmbStore.SetData(Providers.StoreData.GetListForTerminal(PluginEntry.DataModel,terminal.ID, terminal.StoreID),
                PluginEntry.Framework.GetImageList().Images[PluginEntry.StoreImageIndex]);
        }

        private void cmbVisualProfile_RequestData(object sender, EventArgs e)
        {
            cmbVisualProfile.SetData(Providers.VisualProfileData.GetList(PluginEntry.DataModel),null);
        }

        private void cmbHardwareProfile_RequestData(object sender, EventArgs e)
        {
            cmbHardwareProfile.SetData(Providers.HardwareProfileData.GetList(PluginEntry.DataModel),null);
        }

        private void cmbTouchLayout_RequestData(object sender, EventArgs e)
        {
            cmbTouchLayout.SetData(Providers.TouchLayoutData.GetList(PluginEntry.DataModel),
                                   null);
        }

        private void btnEditStore_Click(object sender, EventArgs e)
        {
            storeEditior.Message(this, "EditStore", cmbStore.SelectedData.ID);
        }

        private void btnEditVisualProfile_Click(object sender, EventArgs e)
        {
            visualProfileEditor.Message(this, "EditVisualProfile", cmbVisualProfile.SelectedData.ID);
        }

        private void btnEditHardwareProfiles_Click(object sender, EventArgs e)
        {
            hardwareProfileEditor.Message(this, "EditHardwareProfile", cmbHardwareProfile.SelectedData.ID);
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteTerminal(new RecordIdentifier(terminal.ID, terminal.StoreID));
        }

        private void btnEditTouchButtons_Click(object sender, EventArgs e)
        {
            layoutEditor.Message(this, "EditLayout", cmbTouchLayout.SelectedData.ID);
        }

        private void btnEditFunctionalProfile_Click(object sender, EventArgs e)
        {
            functionalityProfileEditor.Message(this, "EditfunctionalityProfile", cmbFunctionalProfile.SelectedData.ID);
        }

        private void cmbFunctionalProfile_RequestData(object sender, EventArgs e)
        {
            cmbFunctionalProfile.SetData(Providers.FunctionalityProfileData.GetList(PluginEntry.DataModel),
                null);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }

        private void cmbTouchLayout_RequestClear(object sender, EventArgs e)
        {
            cmbTouchLayout.SelectedData = new DataEntity("", "");
        }

        private void cmbFunctionalProfile_RequestClear(object sender, EventArgs e)
        {
            cmbFunctionalProfile.SelectedData = new DataEntity("", "");
        }

        private void cmbHardwareProfile_SelectedDataChanged(object sender, EventArgs e)
        {
            ShowOrHideEftAndLsPayTabs(cmbHardwareProfile.SelectedDataID);
        }

        private void ShowOrHideEftAndLsPayTabs(RecordIdentifier hardwareProfileID)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.TerminalEdit))
            {
                var oldEftType = eftType;
                eftType = GetEftTypeFromHardwareProfile(hardwareProfileID);

                switch (eftType)
                {
                    case EftType.LsPay:
                        lsPayTab.Visible = true;
                        eftTab.Visible = false;
                        break;

                    case EftType.Other:
                        lsPayTab.Visible = false;
                        eftTab.Visible = true;
                        break;

                    default:
                        lsPayTab.Visible = false;
                        eftTab.Visible = false;
                        break;
                }

                if (oldEftType != eftType)
                {
                    tabSheetTabs.Select();
                }
            }
        }

        private EftType GetEftTypeFromHardwareProfile(RecordIdentifier hardwareProfileId)
        {
            var hwProfile = Providers.HardwareProfileData.Get(PluginEntry.DataModel, hardwareProfileId);
            if (hwProfile != null)
            {
                if (hwProfile.EftConnected)
                {
                    if (hwProfile.LsPayConnected)
                    {
                        return EftType.LsPay;
                    }

                    return EftType.Other;
                }
            }

            return EftType.None;
        }
    }
}
