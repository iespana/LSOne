using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Operations;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Administration.Dialogs
{
    public partial class NewBlankOperationDialog : DialogBase
    {
        BlankOperation currentBO;

        public NewBlankOperationDialog()
        {
            InitializeComponent();
            txtId.Focus();
        }

        public NewBlankOperationDialog(BlankOperation bo) : base()
        {
            InitializeComponent();

            currentBO = bo;
            txtId.Enabled = false;
            txtId.Text = (string)bo.ID;
            txtBOParameter.Focus();
            txtBOParameter.Text = bo.OperationParameter;
            txtBODescription.Text = bo.OperationDescription;
            //chkCreatedOnPOS.Checked = bo.CreatedOnPOS; ;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }       

        private void btnOK_Click(object sender, EventArgs e)
        {
            BlankOperation blankOp;

            blankOp = new BlankOperation();
            blankOp.ID = txtId.Text.Trim();
            blankOp.OperationParameter = txtBOParameter.Text.Trim();
            blankOp.OperationDescription = txtBODescription.Text.Trim();
            //blankOp.CreatedOnPOS = chkCreatedOnPOS.Checked;
            
            //this check is for the case that a new operation is added and an existing ID is entered as operation Id
            if (Providers.BlankOperationData.Exists(PluginEntry.DataModel, txtId.Text) && (currentBO == null))
            {
                if (QuestionDialog.Show(
                Properties.Resources.OverWriteQuestion,
                Properties.Resources.RecordExists) == DialogResult.No)
                {
                    return;
                }
            }
            Providers.BlankOperationData.Save(PluginEntry.DataModel, blankOp);
            DialogResult = DialogResult.OK;
            Close();                                                    
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (currentBO != null)
            {
                btnOK.Enabled = txtId.Text != "" && (txtId.Text != (string) currentBO.ID
                                                     || txtBODescription.Text != currentBO.OperationDescription
                                                     || txtBOParameter.Text != currentBO.OperationParameter);
            }
            else
                btnOK.Enabled = txtId.Text != "";
        }     
    }
}
