using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.ViewCore;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.CustomerOrders.Properties;
using ContainerControl = LSOne.Controls.ContainerControl;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.CustomerOrders.ViewPages
{
    public partial class AdditionalConfigurations : ContainerControl, ITabView
    {
        private static Guid SortSettingID = new Guid("78DDDA08-85CF-4C6B-835B-05069ED08771");
        private static Guid BarSettingID = new Guid("7EA40593-14F1-4C43-A564-ED7B67F58CA0");
        
        private Setting searchBarSetting;

        private Guid selectedConfig;

        //Search params
        string searchDescription = null;
        bool searchDescriptionBeginsWith = true;
        private bool sortAscending = true;

        ConfigurationType searchType = ConfigurationType.None;

        private List<CustomerOrderAdditionalConfigurations> additionalConfigList;

        public AdditionalConfigurations()
        {
            InitializeComponent();

            additionalConfigList = Providers.CustomerOrderAdditionalConfigData.GetList(PluginEntry.DataModel);
            selectedConfig = Guid.Empty;

            lvAdditionalConfig.ContextMenuStrip = new ContextMenuStrip();
            lvAdditionalConfig.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            
            btnsEditAddRemove.EditButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrderSettings);
            btnsEditAddRemove.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrderSettings);
            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrderSettings);

            searchBar.BuddyControl = lvAdditionalConfig;

            searchBar.FocusFirstInput();
        }

        public AdditionalConfigurations(RecordIdentifier selectionID)
        : this()
        {
            selectedConfig = (Guid)selectionID;
        }

        public static ITabView CreateInstance(object sender, ViewCore.Controls.TabControl.Tab tab)
        {
            return new AdditionalConfigurations();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "Configuration")
            {
                CustomerOrderAdditionalConfigurations config = (CustomerOrderAdditionalConfigurations) param;
                if (changeHint == DataEntityChangeType.Edit)
                {
                    int indx = additionalConfigList.FindIndex(f => f.ID == changeIdentifier);
                    if (indx >= 0)
                    {
                        additionalConfigList.RemoveAt(indx);
                    }
                    additionalConfigList.Add(config);
                }
                else if (changeHint == DataEntityChangeType.Add || changeHint == DataEntityChangeType.MultiAdd)
                {
                    additionalConfigList = Providers.CustomerOrderAdditionalConfigData.GetList(PluginEntry.DataModel);
                }
                else if (changeHint == DataEntityChangeType.Delete)
                {
                    int indx = additionalConfigList.FindIndex(f => f.ID == changeIdentifier);
                    if (indx >= 0)
                    {
                        additionalConfigList.RemoveAt(indx);
                    }
                }

                LoadItems();
            }
        }

        public void SaveUserInterface()
        {
            Setting sortSetting = new Setting(true, string.Empty, string.Empty, SettingType.UISetting);
            sortSetting.Value = lvAdditionalConfig.SortSetting;
            sortSetting.UserSettingExists = true;
            PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, SortSettingID, SettingsLevel.User, sortSetting);
            
        }

        private void LoadItems()
        {
            searchDescription = null;
            searchDescriptionBeginsWith = true;
            searchType = ConfigurationType.None;
            
            lvAdditionalConfig.ClearRows(); 

            if (lvAdditionalConfig.SortColumn == null)
            {
                lvAdditionalConfig.SetSortColumn(lvAdditionalConfig.Columns[1], true);
            }

            #region search parameters
            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        searchDescription = result.StringValue;
                        searchDescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Type":
                        switch (result.ComboSelectedIndex)
                        {
                            case 0:
                                searchType = ConfigurationType.None;
                                break;
                            case 1:
                                searchType = ConfigurationType.Source;
                                break;
                            case 2:
                                searchType = ConfigurationType.Delivery;
                                break;
                            default:
                                searchType = ConfigurationType.None;
                                break;
                        }
                        break;
                }
            }

            #endregion

            Row row;
            foreach (var config in additionalConfigList.Where(Filter))
            {
                row = new Row();
                row.AddText(config.Text);
                row.AddText(config.AsString(config.AdditionalType));
                row.Tag = config;
                lvAdditionalConfig.AddRow(row);
            }

            lvAdditionalConfig.Sort(lvAdditionalConfig.SortColumn, sortAscending);

            lvAdditionalConfig_SelectionChanged(this, EventArgs.Empty);
            lvAdditionalConfig.AutoSizeColumns();
        }

        private bool Filter(CustomerOrderAdditionalConfigurations config)
        {
            bool result = false;
            if (searchDescription != null)
            {
                result = searchDescriptionBeginsWith ? config.Text.StartsWith(searchDescription, StringComparison.InvariantCultureIgnoreCase) : config.Text.Contains(searchDescription);
                if (!result)
                {
                    return false;
                }
            }

            if (searchType != ConfigurationType.None)
            {
                result = config.AdditionalType == searchType;
                if (!result)
                {
                    return false;
                }
            }

            return true;
        }


        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvAdditionalConfig.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsEditAddRemove_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.New,
                   200,
                   btnsEditAddRemove_AddButtonClicked)
            {
                Enabled = btnsEditAddRemove.AddButtonEnabled,
                Image = ContextButtons.GetAddButtonImage()
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsEditAddRemove_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsEditAddRemove.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ConfigurationList", lvAdditionalConfig.ContextMenuStrip, lvAdditionalConfig);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvAdditionalConfig_SelectionChanged(object sender, EventArgs e)
        {
            bool hasEditPermission = PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrderSettings);

            btnsEditAddRemove.EditButtonEnabled = (lvAdditionalConfig.Selection.Count == 1) && hasEditPermission;
            btnsEditAddRemove.RemoveButtonEnabled = (lvAdditionalConfig.Selection.Count == 1) && hasEditPermission;

            
            if (lvAdditionalConfig.Selection.Count == 1)
            {
                selectedConfig = (Guid)((CustomerOrderAdditionalConfigurations) lvAdditionalConfig.Rows[lvAdditionalConfig.Selection.FirstSelectedRow].Tag).ID;
            }
        }

        private void lvAdditionalConfig_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvAdditionalConfig.SortColumn == args.Column)
            {
                sortAscending = !sortAscending;
                lvAdditionalConfig.SetSortColumn(args.Column, !sortAscending);
            }
            else
            {
                sortAscending = true;
                lvAdditionalConfig.SetSortColumn(args.Column, sortAscending);
            }

            LoadItems();
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> typeList = new List<object>();
            typeList.Add(Resources.AllTypes);
            typeList.Add(Resources.Source);
            typeList.Add(Resources.Delivery);

            searchBar.AddCondition(new ConditionType(Resources.SearchDescription, PluginKeys.SearchKey_Description, ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.Type, PluginKeys.SearchKey_Type, ConditionType.ConditionTypeEnum.ComboBox, typeList, 0, 0, false));

            searchBar_LoadDefault(this, EventArgs.Empty);
    
        }

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void searchBar_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            LoadItems();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            throw new NotImplementedException();
        }

        public void OnClose()
        {
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            if (!btnsEditAddRemove.AddButtonEnabled)
            {
                return;
            }

            PluginOperations.EditConfigurationDialog(new CustomerOrderAdditionalConfigurations());
            
            LoadItems();
            
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            if (!btnsEditAddRemove.EditButtonEnabled)
            {
                return;
            }

            CustomerOrderAdditionalConfigurations editedConfig = additionalConfigList.FirstOrDefault(f => f.ID == selectedConfig);

            if (editedConfig != null)
            {
                PluginOperations.EditConfigurationDialog(editedConfig);
            }
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (!btnsEditAddRemove.RemoveButtonEnabled)
            {
                return;
            }

            CustomerOrderAdditionalConfigurations editedConfig = additionalConfigList.FirstOrDefault(f => f.ID == selectedConfig);
            PluginOperations.DeleteConfiguration(editedConfig);
        }
    }

    
}
