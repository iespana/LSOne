using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.SiteService.Dialogs
{
    public partial class GetCredentialsDialog : DialogBase
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public GetCredentialsDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UserName = tbUserName.Text;
            Password = tbPassword.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void TextEntered(object sender, EventArgs e)
        {
            btnOK.Enabled = tbUserName.Text.Length > 0 && tbPassword.Text.Length > 0;
        }
    }
}
