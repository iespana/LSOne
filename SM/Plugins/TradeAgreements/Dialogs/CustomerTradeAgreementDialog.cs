using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class CustomerTradeAgreementDialog : DialogBase
    {
        RecordIdentifier customerId;
        RecordIdentifier selectedId;

        public CustomerTradeAgreementDialog(RecordIdentifier id)
            : this()
        {
            this.customerId = id;
        }

        public CustomerTradeAgreementDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public RecordIdentifier SelectedId()
        {
            return selectedId;
        }

        private void btnLineDiscount_Click(object sender, EventArgs e)
        {
            Dialogs.TradeAgreementLineDiscountCustGroupDialog dlg = new Dialogs.TradeAgreementLineDiscountCustGroupDialog(customerId, TradeAgreementEntryAccountCode.Customer);
            if (dlg.ShowDialog() == DialogResult.OK)
	        {
                selectedId = dlg.ID;
                this.DialogResult = dlg.DialogResult;
                Close();
	        }
        }

        private void btnMultilineDiscount_Click(object sender, EventArgs e)
        {
            Dialogs.TradeAgreementMultiLineDiscountDialog dlg = new Dialogs.TradeAgreementMultiLineDiscountDialog(customerId, Dialogs.TradeAgreementMultiLineDiscountDialog.MultiLineDiscountDialogMode.Customer);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedId = dlg.ID;
                this.DialogResult = dlg.DialogResult;
                Close();
            }
        }

        private void btnTotalDiscount_Click(object sender, EventArgs e)
        {
            Dialogs.TradeAgreementTotalDiscountDialog dlg = new Dialogs.TradeAgreementTotalDiscountDialog(customerId, TradeAgreementEntryAccountCode.Customer);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedId = dlg.ID;
                this.DialogResult = dlg.DialogResult;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
