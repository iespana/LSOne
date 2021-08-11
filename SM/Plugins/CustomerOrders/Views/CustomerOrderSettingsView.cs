using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.CustomerOrders.ViewPages;

namespace LSOne.ViewPlugins.CustomerOrders.Views
{
    public partial class CustomerOrderSettingsView : ViewBase
    {
        private List<CustomerOrderSettings> settingsList;

        private int selectedTabIndex = 0;

        /// <summary>
        /// Constructor that sets a specific tab to be opened when the view is opened
        /// </summary>
        /// <param name="settingsType">The tab that should be visible when the view is displayed</param>
        public CustomerOrderSettingsView(CustomerOrderType settingsType)
            : this()
        {
            selectedTabIndex = settingsType == CustomerOrderType.CustomerOrder ? 0 : 1;
        }

        public CustomerOrderSettingsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Audit |
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar;

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrderSettings);

            settingsList = new List<CustomerOrderSettings>();
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("CustomerOrderSettings", null, Properties.Resources.CustomerOrderSettings, true));
        }

        protected override string LogicalContextName => Properties.Resources.CustomerOrderSettings;

        public override RecordIdentifier ID => RecordIdentifier.Empty;

        public string Description => Properties.Resources.CustomerOrderSettings;

        protected override void LoadData(bool isRevert)
        {
            settingsList = Providers.CustomerOrderSettingsData.GetList(PluginEntry.DataModel);

            if (!isRevert)
            {
                tabSheetTabs.Broadcast(this, RecordIdentifier.Empty, settingsList);
            }
            HeaderText = Properties.Resources.CustomerOrderSettings;

            tabSheetTabs.SetData(isRevert, RecordIdentifier.Empty, settingsList);

            if (selectedTabIndex != -1)
            {
                tabSheetTabs.SelectedTab = tabSheetTabs[selectedTabIndex];
            }

            tabSheetTabs.SendTabMessage(0, null);
        }

        protected override bool DataIsModified()
        {
            if (tabSheetTabs.IsModified())
            {
                //Data notificatin is used to send the changed information to the view for saving (OnDataChanged)
                //the settingsList needs to be cleared if there is something to be saved
                settingsList.Clear();
                return true;
            }
            return false;
        }

        protected override bool SaveData()
        {
            tabSheetTabs.GetData();
            
            Providers.CustomerOrderSettingsData.Save(PluginEntry.DataModel, settingsList.FirstOrDefault(f => f.SettingsType == CustomerOrderType.CustomerOrder));
            Providers.CustomerOrderSettingsData.Save(PluginEntry.DataModel, settingsList.FirstOrDefault(f => f.SettingsType == CustomerOrderType.Quote));

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "CustomerOrder":
                    if (changeHint == DataEntityChangeType.Edit && changeIdentifier == (int)CustomerOrderType.CustomerOrder)
                    {
                        settingsList.Add((CustomerOrderSettings)param);
                    }
                    break;
                case "Quote":
                    if (changeHint == DataEntityChangeType.Edit && changeIdentifier == (int)CustomerOrderType.Quote)
                    {
                        settingsList.Add((CustomerOrderSettings)param);
                    }
                    break;
                case "Configuration":
                    tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
                    break;
            }
        }

        protected override void OnDelete()
        {
            
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }

        protected override void InitializeView(bool isMultiEdit)
        {
            tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Orders, SettingsPage.CreateInstance));
            tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Quotes, SettingsPage.CreateInstance));
            tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.AdditionalConfigurations, PluginKeys.ConfigurationKey, AdditionalConfigurations.CreateInstance));
        }

        private void tabSheetTabs_TabIndexChanged(object sender, System.EventArgs e)
        {
            selectedTabIndex = tabSheetTabs.SelectedTabIndex;
        }
    }
}
