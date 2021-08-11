using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Profiles.Properties;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    /// <summary>
    /// A dialog to create and/or edit a <see cref="PosContext"/>
    /// </summary>
    public partial class GetCredentialsDialog : DialogBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
      
        /// <summary>
        /// Default constructor for the dialog. All variables are initialized
        /// </summary>
        public GetCredentialsDialog()
        {
            InitializeComponent();
           
        }

    

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            UserName = tbUserName.Text;
            Password = tbPassword.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = (tbUserName.Text.Length > 0);
        }
    }
}
