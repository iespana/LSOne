using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.EMails;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.EMail;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.SiteService.Dialogs
{
    public partial class SMTPSettingsDialog : DialogBase
    {
        private EMailSetting emailSetting;
        private EMailMessage emailMsg;
        private int validFields;

        public SMTPSettingsDialog()
        {
            InitializeComponent();
        }

        public SMTPSettingsDialog(EMailSetting emailSetting)
            : this()
        {
            btnSave.Enabled = true;
            this.emailSetting = emailSetting;
            if (emailSetting.StoreID != RecordIdentifier.Empty)
            {
                cmbStore.Enabled = false;
            }

            string storeName = "";
            var store = Providers.StoreData.Get(PluginEntry.DataModel, emailSetting.StoreID);
            if (store != null)
                storeName = store.Text;
            cmbStore.SelectedData = new DataEntity(emailSetting.StoreID, storeName);

            tbSmtpServer.Text = emailSetting.SmtpServer;
            ntbPort.Value = emailSetting.SmtpPort;

            tbEmailAddress.Text = emailSetting.SmtpEMailAddress;
            tbPassword.Text = emailSetting.SmtpPassword;
            tbDisplayName.Text = emailSetting.SmtpDisplayName;

            chkUseSSL.Checked = emailSetting.UseSSL;
            chkTextOnly.Checked = emailSetting.TextOnly;
            tbSignature.Text = emailSetting.Signature;

            // TEMPORARY CODE - hide store setup
            {
                grpStore.Visible = false;

                // Locations
                // grpStore = 12; 75
                // grpServer = 12; 128
                // grpAuthentication = 12; 204
                // grpOptions = 12; 307

                // Move everything up by 128 - 74
                int delta = grpServer.Location.Y - grpStore.Location.Y;
                MoveGroup(grpServer, delta);
                MoveGroup(grpAuthentication, delta);
                MoveGroup(grpOptions, delta);
                ClientSize = new Size(ClientSize.Width, ClientSize.Height - delta);
            }
        }

        private static void MoveGroup(GroupBox grp, int delta)
        {
            grp.Location = new Point(grp.Location.X, grp.Location.Y - delta);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            validFields = 0;
            if (!string.IsNullOrWhiteSpace(tbSmtpServer.Text))
                validFields++;
            if (ntbPort.Value != 0)
                validFields++;
            if (!string.IsNullOrWhiteSpace(tbSmtpServer.Text))
                validFields++;
            if (!string.IsNullOrWhiteSpace(tbEmailAddress.Text))
                validFields++;
            if (!string.IsNullOrWhiteSpace(tbPassword.Text))
                validFields++;

            btnTest.Enabled = (validFields == 5);
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            cmbStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OnClearClick(object sender, EventArgs e)
        {
            cmbStore.SelectedData.ID = RecordIdentifier.Empty;

            tbSmtpServer.Text = "";
            ntbPort.Value = 25;

            tbEmailAddress.Text = "";
            tbPassword.Text = "";
            tbDisplayName.Text = "";

            chkUseSSL.Checked = false;
            chkTextOnly.Checked = false;
            tbSignature.Text = "";
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            emailSetting.StoreID = cmbStore.SelectedData.ID;
            
            emailSetting.SmtpServer = tbSmtpServer.Text;
            emailSetting.SmtpPort = Convert.ToInt32(ntbPort.Value);

            emailSetting.SmtpEMailAddress = tbEmailAddress.Text;
            emailSetting.SmtpPassword = tbPassword.Text;
            emailSetting.SmtpDisplayName = tbDisplayName.Text;

            emailSetting.UseSSL = chkUseSSL.Checked;
            emailSetting.TextOnly = chkTextOnly.Checked;
            emailSetting.Signature = tbSignature.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (emailMsg == null)
                emailMsg = new EMailMessage();

            var server = new SmtpServerData
                {
                    SmtpServer = tbSmtpServer.Text,
                    SmtpPort = Convert.ToInt32(ntbPort.Value),
                    SmtpEMailAddress = tbEmailAddress.Text,
                    SmtpPassword = tbPassword.Text,
                    SmtpDisplayName = tbDisplayName.Text,
                    UseSSL = chkUseSSL.Checked,
                    TextOnly = chkTextOnly.Checked,
                    Signature = tbSignature.Text
                };

            new SendTestEMailDialog(server, emailMsg).ShowDialog();
        }
    }
}
