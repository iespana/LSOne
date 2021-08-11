using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    public partial class frmJournalSearch : TouchBaseForm
    {
        private string appliedSelectedTransactionId;
        private DateTime appliedSelectedFromDate;
        private bool appliedSelectedFromDateChecked;
        private DateTime appliedSelectedToDate;
        private bool appliedSelectedToDateChecked;
        private TypeOfTransaction? appliedType;
        private string appliedStaff;
        private decimal appliedMinAmount;
        private decimal appliedMaxAmount;
        private List<DataEntity> transactionTypes;

        public string TransactionStore { get; private set; }
        public string TransactionTerminal { get; private set; }
        public string SelectedTransactionId { get; private set; }
        public DateTime? SelectedFromDate => dpFromDate.Checked ? dpFromDate.Value.Date : (DateTime?)null;
        public DateTime? SelectedToDate => dpToDate.Checked ? dpToDate.Value.Date.AddDays(1).AddMilliseconds(-1) : (DateTime?)null;
        public TypeOfTransaction? Type => !RecordIdentifier.IsEmptyOrNull(cmbType.SelectedDataID) ? (TypeOfTransaction)(int)(cmbType.SelectedDataID) : (TypeOfTransaction?)null;
        public string Staff => !RecordIdentifier.IsEmptyOrNull(cmbStaff.SelectedDataID) ? (string)cmbStaff.SelectedDataID : null;
        // ONE-11595
        /* How min/max amount intervals should work:
         *  if fromAmount = toAmount = 0, we ignore the amount filter
         *  if fromAmount = 0, toAmount = something, we filter where amount >= 0 and amount <= something
         *  if fromAmount = something, toAmount = 0, we ignore the toAmount and we filter where amount >= something
         *  if fromAmount = something, toAmount = something else, we take into consideration both limits
         * On other words, the filter should behave like this:
         *  0 0 => null null
         *  0 1 => 0    1
         *  1 0 => 1    null
         *  1 1 => 1    1
         */
        // According to the explanation above, MinAmount is null only if both fromAmount and toAmount are null; otherwise it is the value set into fromAmount
        public decimal? MinAmount => ntbFromAmount.Value == 0 && ntbToAmount.Value == 0 ? (decimal?)null : (decimal)ntbFromAmount.Value;
        // According to the explanation above, MaxAmount is null if toAmount is null; otherwise it is the value set into toAmount
        public decimal? MaxAmount => ntbToAmount.Value == 0 ? (decimal?)null : (decimal)ntbToAmount.Value;

        public frmJournalSearch()
        {
            InitializeComponent();

            transactionTypes = DataLayer.BusinessObjects.Transactions.Transaction.GetTypes().OrderBy(x => x.Text).ToList();

            dpFromDate.MaxDate = DateTime.Now;
            dpToDate.MaxDate = DateTime.Now;

            ResetFilter();

            ntbFromAmount.LostFocus += NtbFromAmount_LostFocus;
            ntbToAmount.LostFocus += NtbToAmount_LostFocus;
        }

        public void ResetFilter()
        {
            txtReceiptId.Text = "";
            dpFromDate.Value = DateTime.Now;
            dpFromDate.Checked = false;
            dpToDate.Value = DateTime.Now;
            dpToDate.Checked = false;
            cmbType.SelectedDataID = RecordIdentifier.Empty;
            cmbStaff.SelectedDataID = RecordIdentifier.Empty;
            ntbFromAmount.Value = 0;
            ntbToAmount.Value = 0;

            SaveFilter();
        }

        private void CheckSearchCriteriaEntered(object sender, EventArgs e)
        {
            bool searchCriteriaEntered = txtReceiptId.Text != ""
                || dpFromDate.Checked
                || dpToDate.Checked
                || !RecordIdentifier.IsEmptyOrNull(cmbType.SelectedDataID)
                || !RecordIdentifier.IsEmptyOrNull(cmbStaff.SelectedDataID)
                || ntbFromAmount.Value != 0
                || ntbToAmount.Value != 0;

            btnOk.Enabled = searchCriteriaEntered;
        }

        private void NtbFromAmount_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ntbFromAmount.Text))
            {
                ntbFromAmount.Value = 0;
            }
        }

        private void NtbToAmount_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ntbToAmount.Text))
            {
                ntbToAmount.Value = 0;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            txtReceiptId.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtReceiptId.Text.Trim() != "" || SelectedFromDate.HasValue || SelectedToDate.HasValue || Type.HasValue || !string.IsNullOrEmpty(Staff) || MinAmount.HasValue || MaxAmount.HasValue)
            {
                SelectedTransactionId = txtReceiptId.Text.Trim();

                BarcodeReceiptParseInfo parseInfo = Interfaces.Services.BarcodeService(DLLEntry.DataModel).ParseBarcodeReceipt(DLLEntry.DataModel, SelectedTransactionId);

                TransactionStore = parseInfo.StoreID;
                TransactionTerminal = parseInfo.TerminalID;
                SelectedTransactionId = parseInfo.ReceiptID;

                SaveFilter();

                this.DialogResult = DialogResult.OK;
                Close();
            }

            SetFormFocus();
        }

        private void SaveFilter()
        {
            appliedSelectedTransactionId = txtReceiptId.Text;
            appliedSelectedFromDate = dpFromDate.Value;
            appliedSelectedFromDateChecked = dpFromDate.Checked;
            appliedSelectedToDate = dpToDate.Value;
            appliedSelectedToDateChecked = dpToDate.Checked;
            appliedType = Type;
            appliedStaff = Staff;
            appliedMinAmount = (decimal)ntbFromAmount.Value;
            appliedMaxAmount = (decimal)ntbToAmount.Value;
        }

        private void frmJournalSearch_Load(object sender, EventArgs e)
        {
            SetFormFocus();
        }

        private void SetFormFocus()
        {
            txtReceiptId.Focus();
        }

        private void txtTransactionId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOk_Click(this, new EventArgs());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RevertFilter();

            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void RevertFilter()
        {
            txtReceiptId.Text = appliedSelectedTransactionId;
            dpFromDate.Value = appliedSelectedFromDate;
            dpFromDate.Checked = appliedSelectedFromDateChecked;
            dpToDate.Value = appliedSelectedToDate;
            dpToDate.Checked = appliedSelectedToDateChecked;
            cmbType.SelectedData = appliedType.HasValue ? transactionTypes.First(x => x.ID == (int)appliedType.Value) : null;
            cmbStaff.SelectedData = !string.IsNullOrEmpty(appliedStaff) ? Providers.POSUserData.Get(DLLEntry.DataModel, appliedStaff, UsageIntentEnum.Minimal, CacheType.CacheTypeApplicationLifeTime) : null;
            ntbFromAmount.Value = (double)appliedMinAmount;
            ntbToAmount.Value = (double)appliedMaxAmount;
        }

        private void cmbType_RequestData(object sender, EventArgs e)
        {
            cmbType.SetData(DataLayer.BusinessObjects.Transactions.Transaction.GetTypes().OrderBy(x => x.Text).ToList(), null);
        }

        private void cmbStaff_RequestData(object sender, EventArgs e)
        {
            cmbStaff.SetData(Providers.POSUserData.GetList(DLLEntry.DataModel, "", UsageIntentEnum.Minimal, CacheType.CacheTypeApplicationLifeTime), null);
        }

        private void cmbType_RequestClear(object sender, EventArgs e)
        {
            cmbType.SelectedData = null;
        }

        private void cmbStaff_RequestClear(object sender, EventArgs e)
        {
            cmbStaff.SelectedData = null;
        }
    }
}