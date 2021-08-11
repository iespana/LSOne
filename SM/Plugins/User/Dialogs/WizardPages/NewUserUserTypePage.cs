using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;

namespace LSOne.ViewPlugins.User.Dialogs.WizardPages
{
    internal partial class NewUserUserTypePage : UserControl, IWizardPage
    {
        WizardBase parent;
        IWizardPage lastChoice;

        public NewUserUserTypePage(WizardBase parent) : base()
        {
            this.parent = parent;
            lastChoice = null;

            InitializeComponent();
        }

        private void btnActiveDirectory_Click(object sender, EventArgs e)
        {
            if (lastChoice is NewUserADIdentityPage)
            {
                parent.Next();
            }
            else
            {
                lastChoice = new NewUserADIdentityPage(parent);
                parent.AddPage(lastChoice);
            }
        }

        private void btnStandalone_Click(object sender, EventArgs e)
        {
            if (lastChoice is NewUserIdentityPage)
            {
                parent.Next();
            }
            else
            {
                lastChoice = new NewUserIdentityPage(parent);
                parent.AddPage(lastChoice);
            }
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
            
        }

        public IWizardPage RequestNextPage()
        {
            return lastChoice;
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            return true;
        }

        public void ResetControls()
        {
            
        }

        #endregion
    }
}
