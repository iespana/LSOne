using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;

namespace LSOne.ViewPlugins.User.Dialogs.WizardPages
{
    public partial class NewUserUserExistsPage : UserControl, IWizardPage
    {
        WizardBase parent;
        LSOne.DataLayer.BusinessObjects.UserManagement.User existingUser;

        public NewUserUserExistsPage()
        {
            InitializeComponent();
        }
        public NewUserUserExistsPage(WizardBase parent, LSOne.DataLayer.BusinessObjects.UserManagement.User existingUser)
            : base()
        {
            this.parent = parent;
            this.existingUser = existingUser;

            InitializeComponent();
                        
            fullNameField1.PopulateNamePrefixes(parent.Connection.Cache.GetNamePrefixes());
            fullNameField1.NameRecord = existingUser.Name;            

            tbLoginName.Text = existingUser.Login;

            fullNameField1.Enabled = false;
            tbLoginName.Enabled = false;
        }

        #region IWizardPage Members

        public bool HasForward
        {
            get { return false; }
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
            wizardOptionButton2.Focus();
        }

        public IWizardPage RequestNextPage()
        {
            return null;
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            return true;
        }

        public void ResetControls()
        {
            
        }

        #endregion


        private void wizardOptionButton2_Click(object sender, EventArgs e)
        {            
            ((NewUserWizard)parent).UserID = existingUser.Guid;
            ((NewUserWizard)parent).DialogResult = DialogResult.OK;
            ((NewUserWizard)parent).Close();
        }
    }
}
