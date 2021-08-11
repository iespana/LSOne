using System.Collections.Generic;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.SqlConnector;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    public partial class AdministrationLocalizePage : ContainerControl, ITabViewV2
    {
        private Setting    namingConvention;
        private Setting    addressConvention;

        public AdministrationLocalizePage()
        {
            InitializeComponent();

            GetDataFromDatabase();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.AdministrationLocalizePage();
        }

        private void GetDataFromDatabase()
        {
            namingConvention = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.NamingFormat, SettingsLevel.System);
            addressConvention = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.AddressFormat, SettingsLevel.System);
        }

        private void PopulateControls(bool isRevert)
        {
            if (!isRevert)
            {
                cmbNamingConvention.Items.Add(Properties.Resources.FirstNameFirst);
                cmbNamingConvention.Items.Add(Properties.Resources.LastNameFirst);

                cmbAddressConvention.Items.Add(Properties.Resources.GenericWithState);
                cmbAddressConvention.Items.Add(Properties.Resources.GenericWithoutState);
                cmbAddressConvention.Items.Add(Properties.Resources.US);
                cmbAddressConvention.Items.Add(Properties.Resources.Canadian);
                cmbAddressConvention.Items.Add(Properties.Resources.Indian);
            }


            cmbNamingConvention.SelectedIndex = namingConvention.IntValue - 1;
            cmbAddressConvention.SelectedIndex = addressConvention.IntValue - 1;
        }

        #region ITabPanel Members

        public void InitializeView(RecordIdentifier context, object internalContext)
        {

        }
        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            PopulateControls(isRevert);
        }

        
        public bool DataIsModified()
        {
            if(cmbNamingConvention.SelectedIndex != namingConvention.IntValue - 1) return true;
            if (cmbAddressConvention.SelectedIndex != addressConvention.IntValue - 1) return true;

            return false;
        }

        public bool SaveData()
        {
            if(cmbAddressConvention.SelectedIndex + 1 != addressConvention.IntValue)
            {
                addressConvention.IntValue = cmbAddressConvention.SelectedIndex + 1;
                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, Settings.AddressFormat, SettingsLevel.System, addressConvention);  
            }

            if(cmbNamingConvention.SelectedIndex + 1 != namingConvention.IntValue)
            {
                namingConvention.IntValue = cmbNamingConvention.SelectedIndex + 1;
                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, Settings.NamingFormat, SettingsLevel.System, namingConvention);
            }

            return true;
        }


        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
           
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            GetDataFromDatabase();

            cmbNamingConvention.Items.Clear();
            cmbAddressConvention.Items.Clear();

            PopulateControls(false);
        }


        public void OnClose()
        {

        }
        public void SaveUserInterface()
        {
        }
        #endregion

        

       
    }
}
