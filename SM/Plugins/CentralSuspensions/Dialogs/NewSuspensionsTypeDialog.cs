using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.CentralSuspensions.Dialogs
{
    public partial class NewSuspensionsTypeDialog : DialogBase
    {
         RecordIdentifier suspensionTypeID;
        

        public NewSuspensionsTypeDialog()
        {
            suspensionTypeID = RecordIdentifier.Empty;

            InitializeComponent();           
            //btnOK.Enabled = true;
        }

        public RecordIdentifier SuspensionTypeID
        {
            get { return suspensionTypeID; }
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

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            btnOK.Enabled = tbDescription.Text != "" && cmbAllowEOD.SelectedIndex >= 0;
        }

        private void cmbAllowEOD_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }
       
        private void btnOK_Click(object sender, EventArgs e)
        {
            SuspendedTransactionType suspendedTransaction = new SuspendedTransactionType();

            suspendedTransaction.Text = tbDescription.Text;
            suspendedTransaction.EndofDayCode = ((SuspendedTransactionsStatementPostingEnum)cmbAllowEOD.SelectedIndex);
          
            Providers.SuspendedTransactionTypeData.Save(PluginEntry.DataModel, suspendedTransaction);

            suspensionTypeID = suspendedTransaction.ID;


            DialogResult = DialogResult.OK;
            Close();
           
        }


       

        
    }
}
