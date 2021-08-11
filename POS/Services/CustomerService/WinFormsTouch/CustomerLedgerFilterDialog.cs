using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Ledger;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace LSOne.Services.WinFormsTouch
{
    public partial class CustomerLedgerFilterDialog : TouchBaseForm
    {
        public CustomerLedgerFilter LedgerFilter;

        private List<DataEntity> types;
        private IConnectionManager entry;
        private RecordIdentifier storeID;

        public CustomerLedgerFilterDialog(IConnectionManager entry)
        {
            InitializeComponent();
            types = GetTypes();
            storeID = RecordIdentifier.Empty;
            touchDialogBanner.Location = new Point(1, 1);
            touchDialogBanner.Width = Width - 2;

            this.entry = entry;
            LedgerFilter = new CustomerLedgerFilter();
            btnOK_Enabled(null, EventArgs.Empty);
        }

        private List<DataEntity> GetTypes()
        {
            List<DataEntity> types = new List<DataEntity>();
            types.Add(new DataEntity(0, Properties.Resources.All));
            types.Add(new DataEntity(1, Properties.Resources.PaymentIntoAccount));
            types.Add(new DataEntity(2, Properties.Resources.Invoice));
            types.Add(new DataEntity(3, Properties.Resources.OtherTenders));

            return types;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dtFrom.Checked)
                LedgerFilter.FromDate = new Date(dtFrom.Value.Date);

            if (dtTo.Checked)
                LedgerFilter.ToDate = new Date(dtTo.Value.Date);

            if(cmbStore.SelectedData != null && cmbStore.SelectedData.ID.StringValue != "")
                LedgerFilter.StoreID = cmbStore.SelectedData.ID;

            if (cmbTerminal.SelectedData != null && cmbTerminal.SelectedData.ID.StringValue != "")
                LedgerFilter.TerminalID = cmbTerminal.SelectedData.ID;

            if(txtDocument.Text.Trim() != "")
            {
                LedgerFilter.Receipt = "%" + txtDocument.Text.Trim(); //Contains
            }

            if(cmbType.SelectedData  != null)
            {
                LedgerFilter.Types = (byte)(cmbType.SelectedDataID) == (byte)CustomerLedgerEntries.TypeEnum.Sale ? (byte)4 : (byte)(cmbType.SelectedDataID);
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void btnOK_Enabled(object sender, EventArgs e)
        {
            btnOK.Enabled = txtDocument.Text != "" |
                            !RecordIdentifier.IsEmptyOrNull(cmbType.SelectedDataID) |
                            !RecordIdentifier.IsEmptyOrNull(cmbStore.SelectedDataID) |
                            !RecordIdentifier.IsEmptyOrNull(cmbTerminal.SelectedDataID) |
                            dtFrom.Checked |
                            dtTo.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            cmbStore.SetData(Providers.StoreData.GetList(entry), null);
        }

        private void cmbTerminal_RequestData(object sender, EventArgs e)
        {
            if(cmbStore.SelectedData != null && cmbStore.SelectedData.ID.StringValue != "")
            {
                cmbTerminal.SetData(Providers.TerminalData.GetList(entry, cmbStore.SelectedData.ID), null);
            }
            else
            {
                cmbTerminal.SetData(Providers.TerminalData.GetList(entry), null);
            }
        }

        private void cmbStore_RequestClear(object sender, EventArgs e)
        {
            cmbStore.SelectedData = null;
            storeID = RecordIdentifier.Empty;
        }

        private void cmbTerminal_RequestClear(object sender, EventArgs e)
        {
            cmbTerminal.SelectedData = null;
        }

        private void cmbType_RequestClear(object sender, EventArgs e)
        {
            cmbType.SelectedData = null;
        }

        private void cmbType_RequestData(object sender, EventArgs e)
        {
            cmbType.SetData(types, null);
        }

        private void cmbStore_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbStore.SelectedDataID != storeID && !RecordIdentifier.IsEmptyOrNull(cmbStore.SelectedDataID))
            {
                cmbTerminal_RequestClear(null, EventArgs.Empty);
            }
            storeID = cmbStore.SelectedDataID;
            btnOK_Enabled(null, EventArgs.Empty);
        }
    }
}
