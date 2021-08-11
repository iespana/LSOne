using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.User.Dialogs.WizardPages
{
    internal partial class NewUserIdentityPage : UserControl, IWizardPage
    {
        WizardBase parent;

        public NewUserIdentityPage(WizardBase parent) : base()
        {
            this.parent = parent;

            InitializeComponent();

            fullNameField1.PopulateNamePrefixes(parent.Connection.Cache.GetNamePrefixes());
        }

        private void CheckState()
        {
            parent.NextEnabled = 
                (tbLoginName.Text.Length > 1) &&
                (tbNewPassword.Text.Length >= 4) &&
                (tbNewPassword.Text == tbConfirmPassword.Text) &&
                (fullNameField1.FirstName.Length > 0 || fullNameField1.LastName.Length > 0) && 
                (string.IsNullOrWhiteSpace(txtEmail.Text) || txtEmail.Text.IsEmail());
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void tbLoginName_Validating(object sender, CancelEventArgs e)
        {
            if(tbLoginName.Text.Length < 2 && tbLoginName.Text.Length != 0)
            {
                errorProvider1.Icon = System.Drawing.Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                errorProvider1.SetError(tbLoginName, Properties.Resources.UserNameHasTobe2Letters);
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void tbNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (tbNewPassword.Text.Length < 4 && tbNewPassword.Text.Length != 0)
            {
                errorProvider2.Icon = System.Drawing.Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                errorProvider2.SetIconAlignment(tbNewPassword, ErrorIconAlignment.MiddleRight);
                errorProvider2.SetError(tbNewPassword, Properties.Resources.PasswordHasTobe4Letters);
            }
            else
            {
                errorProvider2.Clear();
            }

            tbConfirmPassword_Validating(this, e);
        }

        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (tbConfirmPassword.Text != "" && tbConfirmPassword.Text != tbNewPassword.Text)
            {
                errorProvider3.Icon = System.Drawing.Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                errorProvider3.SetError(tbConfirmPassword, Properties.Resources.ConfirmPasswordMustMatch);
            }
            else
            {
                errorProvider3.Clear();
            }
        }

        private void fullNameField1_Validating(object sender, CancelEventArgs e)
        {
            if (fullNameField1.FirstName.Length < 1 && fullNameField1.LastName.Length < 1)
            {
                errorProvider4.Icon = System.Drawing.Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());                
                errorProvider4.SetError(fullNameField1, Properties.Resources.FirstAndLastMustHaveOneLetter);
            }
            else
            {
                errorProvider4.Clear();
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(txtEmail.Text) && !txtEmail.Text.IsEmail())
            {
                errorProvider5.Icon = System.Drawing.Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                errorProvider5.SetError(txtEmail, Properties.Resources.EmailNotValid);
            }
            else
            {
                errorProvider5.Clear();
            }
        }

        public Name NameRecord
        {
            get
            {
                return fullNameField1.NameRecord;
            }
        }

        public string LoginName
        {
            get
            {
                return tbLoginName.Text;
            }
        }

        public string Password
        {
            get
            {
                return tbNewPassword.Text;
            }
        }

        public string Email
        {
            get
            {
                return txtEmail.Text;
            }
        }

        #region IWizardPage Members

        public bool HasForward
        {
            get { return true; }
        }

        public bool HasFinish
        {
            get { return false; }
        }

        public Control PanelControl
        {
            get { return this; }
        }

        public void Display()
        {
            CheckState();

            fullNameField1.Focus();
        }

        public IWizardPage RequestNextPage()
        {
            if (Providers.UserData.
                Exists(PluginEntry.DataModel, tbLoginName.Text))
            {
                // retreive the matching user and pass it to the next form.
                List<LSOne.DataLayer.BusinessObjects.UserManagement.User> users = DataProviderFactory.Instance
                    .Get<IUserData, LSOne.DataLayer.BusinessObjects.UserManagement.User>().
                    FindUsers(
                        PluginEntry.DataModel,
                        "",
                        "",
                        "",
                        "",
                        tbLoginName.Text,
                        0);

                LSOne.DataLayer.BusinessObjects.UserManagement.User existingUser = users[0];

                return new NewUserUserExistsPage(parent, existingUser);                    
            }
            
            return new NewUserAssignToGroupsPage(parent);
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            return true;
        }

        public void ResetControls()
        {
            fullNameField1.NameRecord = new Name();
            txtEmail.Text = "";
            tbLoginName.Text = "";
            tbNewPassword.Text = "";
            tbConfirmPassword.Text = "";
            Display();
        }

        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
