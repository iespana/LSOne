using System;
using System.Windows.Forms;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.ViewPlugins.LSCommerce.Properties;

namespace LSOne.ViewPlugins.LSCommerce.Dialogs
{
    public partial class EditLicenseDialog : DialogBase
    {
        private OmniLicense license;
        public EditLicenseDialog(OmniLicense omniLicense)
        {
            InitializeComponent();

            this.license = omniLicense;

            tbAppID.Text = (string)license.AppID;
            tbDeviceID.Text = (string)license.DeviceID;
            tbLicenseKey.Text = (string)license.Licensekey;
            tbLicenseKey.Enabled = tbAppID.Enabled = tbDeviceID.Enabled = true;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void btnOK_Click(object sender, EventArgs args)
        {
            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

            if (!service.OmniLicenseKeyRecordExists(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), tbLicenseKey.Text, true))
            {
                try
                {
                    license.Licensekey = tbLicenseKey.Text;
                    service.SaveOmniLicenses(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), license, true);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                }
            }
            else
            {
                errorProvider1.SetError(tbLicenseKey, "This license key already exists");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void tbLicenseKey_TextChanged(object sender, EventArgs e)
        {
            if (tbLicenseKey.Text != (string)license.Licensekey)
            {
                btnOK.Enabled = true;
                errorProvider1.Clear();
            }
        }
    }
}
