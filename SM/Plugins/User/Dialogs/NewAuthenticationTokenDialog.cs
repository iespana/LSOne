using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.Services.Interfaces.Constants;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.User.Properties;

namespace LSOne.ViewPlugins.User.Dialogs
{
    public partial class NewAuthenticationTokenDialog : DialogBase
    {
        int numberOfExistingTokens;
        RecordIdentifier userID;
        AuthenticationToken token;

        public NewAuthenticationTokenDialog()
            : base()
        {
            numberOfExistingTokens = 0;
            userID = RecordIdentifier.Empty;

            InitializeComponent();
        }

        public NewAuthenticationTokenDialog(RecordIdentifier userID, int numberOfExistingTokens)
            : base()
        {
            this.numberOfExistingTokens = numberOfExistingTokens;
            this.userID = userID;

            InitializeComponent();
            comboBox1.SelectedIndex = 1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            RecordIdentifier hardwareProfileID = PluginEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.SMHardwareProfile) as RecordIdentifier;
            if (hardwareProfileID != null)
            {
                HardwareProfile profile = Providers.HardwareProfileData.Get(PluginEntry.DataModel, hardwareProfileID);
                if (profile != null)
                {
                    msrTextBox1.TrackSeperation = TrackSeperation.Before;
                    msrTextBox1.StartCharacter = profile.StartTrack1;
                    msrTextBox1.EndCharacter = profile.EndTrack1;
                    msrTextBox1.Seperator = profile.Separator1;
                }
            }

            tbDescription.Text = Properties.Resources.TokenPresetDescription.Replace("#1", (numberOfExistingTokens + 1).ToString());
            tbToken.Focus();
        }
        
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }



        private void CheckEnabled(object sender, EventArgs e)
        {
            var tokenLength = tbToken.Enabled ? tbToken.Text.Length : msrTextBox1.Text.Length;
            btnOK.Enabled = (tbDescription.Text.Length > 0) && (tokenLength > 0);
        }

        public AuthenticationToken Token
        {
            get
            {
                return token;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string rawFeed = tbToken.Enabled ? tbToken.Text : msrTextBox1.Text.Trim();

            while(rawFeed.EndsWith("\r\n"))
            {
                rawFeed = rawFeed.Left(rawFeed.Length - 2);
            }

            token = AuthenticationToken.FromRawFeed(RecordIdentifier.Empty, userID, tbDescription.Text, rawFeed);

            if (!Providers.AuthenticationTokenData.IsUnique(PluginEntry.DataModel, token.TokenHash))
            {
                MessageDialog.Show(Resources.TokenAlreadyExists,MessageBoxButtons.OK);
                return;
            }

            Providers.AuthenticationTokenData.Insert(PluginEntry.DataModel, token);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                msrTextBox1.Enabled = msrTextBox1.Visible = true;
                tbToken.Enabled = tbToken.Visible = false;

                msrTextBox1.Text = tbToken.Text;
            }
            else
            {
                msrTextBox1.Enabled = msrTextBox1.Visible = false;
                tbToken.Enabled = tbToken.Visible = true;

                tbToken.Text = msrTextBox1.Text;
            }
        }

    }
}
