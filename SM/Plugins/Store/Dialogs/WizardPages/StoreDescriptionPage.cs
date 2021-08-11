using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;

namespace LSOne.ViewPlugins.Store.Dialogs.WizardPages
{
    internal partial class StoreDescriptionPage : UserControl, IWizardPage
    {
        WizardBase parent;
        IWizardPage lastChoice;

        public StoreDescriptionPage(WizardBase parent)
            : base()
        {
            this.parent = parent;
            lastChoice = null;

            InitializeComponent();

            addressField.DataModel = PluginEntry.DataModel;
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
            tbDescription.Focus();
        }

        public IWizardPage RequestNextPage()
        {
            return lastChoice;
        }

        public void ResetControls()
        {

        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            return true;
        }

        #endregion

        private void btnPhone_Click(object sender, EventArgs e)
        {
            /*if (lastChoice is ActivationCustomerKeyPage)
            {
                ((ActivationCustomerKeyPage)lastChoice).PhoneActivation = true;
                parent.Next();
            }
            else
            {
                lastChoice = new ActivationCustomerKeyPage(parent,true);
                parent.AddPage(lastChoice);
            }*/
        }

        private void btnInternet_Click(object sender, EventArgs e)
        {
            /*if (lastChoice is ActivationCustomerKeyPage)
            {
                ((ActivationCustomerKeyPage)lastChoice).PhoneActivation = false;
                parent.Next();
            }
            else
            {
                lastChoice = new ActivationCustomerKeyPage(parent,false);
                parent.AddPage(lastChoice);
            }*/
        }

        private void btnDoNothing_Click(object sender, EventArgs e)
        {
            parent.Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

        }

       

        
    }
}
