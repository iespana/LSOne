using System;
using System.Windows.Forms;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.POS.Processes.WinFormsTouch;

namespace LSOne.Services
{
    public partial class frmTenderRestriction : frmTouchBase
    {
        #region Variables and Properties

        private DialogResult dlgResult;

        public string DisplayMsg
        {
            set {memMessage.Text = value;}
            
        }

        public DialogResult DlgResult
        {
            get {return dlgResult;}
        }
        #endregion



        public frmTenderRestriction(RetailTransaction retailTransaction)
        {
            try
            {
                InitializeComponent();

                btnNo.Text = Properties.Resources.No; 
                btnYes.Text = Properties.Resources.Yes;
                lblContinue.Text = Properties.Resources.DoYouWantToContinue; 
                lblExcluded.Text = Properties.Resources.ItemsExcludedFromPayment;

                for (int i = 0; i < ((RetailTransaction)retailTransaction).SaleItems.Count; i++)
                {
                    SaleLineItem lineItem = (SaleLineItem)((RetailTransaction)retailTransaction).GetItem(i+1);

                    if (lineItem.TenderRestrictionId == "")
                    {
                        lstExcluded.Items.Add(lineItem.Description);
                    }
                }
                btnYes.Focus();
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            dlgResult = DialogResult.No;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            dlgResult = DialogResult.Yes;

        }
    }
}