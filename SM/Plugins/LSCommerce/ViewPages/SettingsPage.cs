using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.LSCommerce.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.LSCommerce.ViewPages
{
    internal partial class SettingsPage : UserControl, ITabView
    {
        private OmniLicense omniLicense;

        public SettingsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new SettingsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

            try
            {
                omniLicense = service.GetOmniLicense(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), (string)context, true);
                if (omniLicense != null)
                {
                    tbAppID.Text = (string)omniLicense.AppID;
                    tbDeviceID.Text = (string)omniLicense.DeviceID;
                    tbLicenseKey.Text = (string)omniLicense.Licensekey;
                    tbLicenseKey.Enabled = tbAppID.Enabled = tbDeviceID.Enabled = true;
                }
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
        }

        public bool DataIsModified()
        {
            if (omniLicense != null)
            {
                if (tbLicenseKey.Text != (string) omniLicense.Licensekey)
                {
                    ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                    try
                    {
                        omniLicense.Licensekey = tbLicenseKey.Text;
                        service.SaveOmniLicenses(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), omniLicense, true);
                    }
                    catch (Exception)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                    }
                }
            }
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
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
