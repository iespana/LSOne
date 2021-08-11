using System;
using System.Collections.Generic;
using System.Reflection;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.Administration.Views
{
    public partial class AdministrationView : ViewBase
    {
        private TabControl.Tab localizationTab;
        private TabControl.Tab securityTab;
        private string selectedTabKey;
        private Parameters parameters;
        private string lastSelectedTabKey;
        bool canRebuildContextBar = false;

        public bool StoreManagementTabSelected { get { return tabSheetTabs.SelectedTab.Key == "StoreManagement"; } }
        public bool InventoryTabSelected { get { return tabSheetTabs.SelectedTab.Key == "InventorySettingsTab"; } }

        public AdministrationView()
        {
            Attributes = ViewAttributes.Save |
                ViewAttributes.Revert |
                ViewAttributes.Audit |
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar;

            InitializeComponent();

            HeaderText = Description;

            selectedTabKey = "";
        }

        public AdministrationView(string tabKey)
            :this()
        {
            selectedTabKey = tabKey;
        }

        protected override void LoadData(bool isRevert)
        {
            parameters = Providers.ParameterData.Get(PluginEntry.DataModel);

            if (!isRevert)  // Revert can trigger OnInitialize getting called twice
            {
                localizationTab = new TabControl.Tab(Properties.Resources.Localization,ViewPages.AdministrationLocalizePage.CreateInstance);
                securityTab = new TabControl.Tab(Properties.Resources.Security, ViewPages.AdministrationSecurityPage.CreateInstance);

                tabSheetTabs.AddTab(localizationTab);
                tabSheetTabs.AddTab(securityTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, 0);
            }

            tabSheetTabs.SetData(isRevert, 0, parameters);

            if (selectedTabKey != "")
            {
                for (int i = 0; i < tabSheetTabs.TabCount; i++)
                {
                    if (tabSheetTabs[i].Key == selectedTabKey)
                    {
                        tabSheetTabs.SelectedTab = tabSheetTabs[i];
                        break;
                    }
                }           
            }

            canRebuildContextBar = true;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            // The UUID there represents generic setting group
            contexts.Add(new AuditDescriptor("SystemSettings", "c79ae480-7ee1-11db-9fe1-0800200c9a66", Properties.Resources.AdministrativeOptions, false));
            contexts.Add(new AuditDescriptor("Parameters", 0, Properties.Resources.VariousSettings, true));

            tabSheetTabs.GetAuditContexts(contexts);
        }

        public DataEntity SelectedTaxExemptGroup
        {
            get
            {
                if (StoreManagementTabSelected)
                {
                    Type storeManagementTab = tabSheetTabs.SelectedTab.View.GetType();
                    PropertyInfo info = storeManagementTab.GetProperty("SelectedTaxExemptGroup");
                    return (DataEntity)info.GetValue(tabSheetTabs.SelectedTab.View);
                }

                return null;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty; // Empty because this sheet is single instance
            }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.AdministrativeOptions;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.AdministrativeOptions;
            }
        }

        protected override bool DataIsModified()
        {
            return tabSheetTabs.IsModified();
        }

        protected override bool SaveData()
        {
            bool needToPostSaveChangeNote = false;

            needToPostSaveChangeNote = localizationTab.Dirty || securityTab.Dirty;

            tabSheetTabs.GetData();

            if (parameters.Dirty)
            {
                Providers.ParameterData.Save(PluginEntry.DataModel, parameters);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.DataChangedInDatabase, "Parameters", RecordIdentifier.Empty, null);
            }

            if (needToPostSaveChangeNote)
            {
                MessageDialog.Show(Properties.Resources.SaveChangesNote);
            }

            return true;
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == this.GetType().ToString() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ImportDataPackages))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.InsertDefaultData + "...", PluginOperations.InsertDefaultData), 500);
                }
            }

            base.OnSetupContextBarItems(arguments);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (changeAction == DataEntityChangeType.DataChangedInDatabase && objectName == "AdminSettings")
            {
                param = parameters = Providers.ParameterData.Get(PluginEntry.DataModel);

                tabSheetTabs.UpdateDataHandler(0, parameters);
            }

            tabSheetTabs.BroadcastChangeInformation(changeAction, objectName, changeIdentifier, param);
        }

        private void tabSheetTabs_SelectedTabChanged(object sender, System.EventArgs e)
        {
            if(canRebuildContextBar && (lastSelectedTabKey == "StoreManagement" || StoreManagementTabSelected || lastSelectedTabKey == "InventorySettingsTab" || InventoryTabSelected))
            {
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }

            lastSelectedTabKey = tabSheetTabs.SelectedTab.Key;
        }
    }
}
