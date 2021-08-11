using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ColorPalette;
using LSOne.DataLayer.BusinessObjects.Inventory;

namespace LSOne.Services.Dialogs
{
    /// <summary>
    /// A dialog that allows the user to narrow the list of quotes/customer orders down by creating search criteria
    /// </summary>
    public partial class SearchInventoryTransfer : TouchBaseForm
    {

        /// <summary>
        /// The search critiera creates by the user input into the dialog
        /// </summary>
        public InventoryTransferFilterExtended SearchCriteria
        {
            get => searchCriteria;
            set
            {
                if (value != null)
                {
                    searchCriteria = value;
                }
            }
        }

        private List<Store> Stores;
        private IConnectionManager entry;
        private StoreTransferTypeEnum transferType;
        private RecordIdentifier selectedStoreID;
        private InventoryTransferFilterExtended searchCriteria;

        /// <summary>
        /// the constructor that initializes the dialog
        /// </summary>
        /// <param name="entry"></param>
        public SearchInventoryTransfer(IConnectionManager entry, StoreTransferTypeEnum transferType, InventoryTransferFilterExtended search)
        {
            this.entry = entry;
            this.transferType = transferType;
            InitializeComponent();

            cmbStore.Tag = ConfigurationType.Delivery;

            searchCriteria = search;
            Stores = Providers.StoreData.GetStores(entry);
            btnOk.Enabled = false;

            switch (transferType)
            {
                case StoreTransferTypeEnum.Order:
                    lblIDOrDescription.Text = Properties.Resources.TransferOrder;
                    switch (searchCriteria.TransferFilterType)
                    {
                        case InventoryTransferType.Outgoing:
                            selectedStoreID = searchCriteria.ReceivingStoreID;
                            lblStore.Text = Properties.Resources.ToStore;
                            lblDate.Visible = false;
                            dtDateFrom.Visible = false;
                            lblTo1.Visible = false;
                            dtDateTo.Visible = false;
                            this.Height = Height - dtDateFrom.Height - 37;
                            break;
                        case InventoryTransferType.Incoming:
                            selectedStoreID = searchCriteria.SendingStoreID;
                            lblStore.Text = Properties.Resources.FromStore;
                            break;
                    }
                    break;

                case StoreTransferTypeEnum.Request:
                    lblIDOrDescription.Text = Properties.Resources.TransferRequest;
                    switch (searchCriteria.TransferFilterType)
                    {
                        case InventoryTransferType.Outgoing:
                            selectedStoreID = searchCriteria.SendingStoreID;
                            lblStore.Text = Properties.Resources.FromStore;
                            break;
                        case InventoryTransferType.Incoming:
                            selectedStoreID = searchCriteria.ReceivingStoreID;
                            lblStore.Text = Properties.Resources.FromStore;
                            break;
                    }
                    break;
            }

            DisplaySearchCriteria();
        }

        private void ClearSearchCriteria()
        {
            searchCriteria.DescriptionOrID = "";
            tbIDOrDescription.Text = "";

            searchCriteria.FromDate = null;
            searchCriteria.ToDate = null;

            searchCriteria.SentFrom = null;
            dtDateFrom.Value = DateTime.Now;
            dtDateFrom.Checked = false;

            searchCriteria.SentTo = null;
            dtDateTo.Value = DateTime.Now;
            dtDateTo.Checked = false;

            searchCriteria.ExpectedFrom = null;
            dtDueDateFrom.Value = DateTime.Now;
            dtDueDateFrom.Checked = false;

            searchCriteria.ExpectedTo = null;
            dtDueDateTo.Value = DateTime.Now;
            dtDueDateTo.Checked = false;

            searchCriteria.StoreID = RecordIdentifier.Empty;
            cmbStore.SelectedData = new DataEntity("", "");
        }

        private void UpdateSearchCriteria()
        {
            searchCriteria.DescriptionOrID = tbIDOrDescription.Text;

            if (cmbStore.SelectedData != null)
            {
                selectedStoreID = cmbStore.SelectedData.ID;

                switch (transferType)
                {
                    case StoreTransferTypeEnum.Order:
                        switch (searchCriteria.TransferFilterType)
                        {
                            case InventoryTransferType.Outgoing:
                                searchCriteria.ReceivingStoreID = cmbStore.SelectedData.ID;
                                break;
                            case InventoryTransferType.Incoming:
                                searchCriteria.SendingStoreID = cmbStore.SelectedData.ID;
                                break;
                        }
                        break;

                    case StoreTransferTypeEnum.Request:
                        switch (searchCriteria.TransferFilterType)
                        {
                            case InventoryTransferType.Outgoing:
                                searchCriteria.SendingStoreID = cmbStore.SelectedData.ID;
                                break;
                            case InventoryTransferType.Incoming:
                                searchCriteria.ReceivingStoreID = cmbStore.SelectedData.ID;
                                break;
                        }
                        break;
                }
            }

            if (dtDateFrom.Checked)
            {
                if (transferType == StoreTransferTypeEnum.Order && searchCriteria.TransferFilterType == InventoryTransferType.Outgoing)
                {
                    searchCriteria.FromDate = dtDateFrom.Value;
                }
                else
                {
                    searchCriteria.SentFrom = dtDateFrom.Value;
                }
            }
            else
            {
                searchCriteria.SentFrom = null;
            }

            if (dtDateTo.Checked)
            {
                if (transferType == StoreTransferTypeEnum.Order && searchCriteria.TransferFilterType == InventoryTransferType.Outgoing)
                {
                    searchCriteria.ToDate = dtDateTo.Value;
                }
                else
                {
                    searchCriteria.SentTo = dtDateTo.Value;
                }
            }
            else
            {
                searchCriteria.SentTo = null;
            }

            if (dtDueDateFrom.Checked)
            {
                searchCriteria.ExpectedFrom = dtDueDateFrom.Value;
            }
            else
            {
                searchCriteria.ExpectedFrom = null;
            }

            if (dtDueDateTo.Checked)
            {
                searchCriteria.ExpectedTo = dtDueDateTo.Value;
            }
            else
            {
                searchCriteria.ExpectedTo = null;
            }
        }

        private void DisplaySearchCriteria()
        {
            if (searchCriteria != null)
            {
                tbIDOrDescription.Text = searchCriteria.DescriptionOrID;

                if (!RecordIdentifier.IsEmptyOrNull(selectedStoreID))
                {
                    Store store = Stores.FirstOrDefault(f => string.Equals(((string)f.ID), ((string)selectedStoreID), StringComparison.InvariantCultureIgnoreCase));
                    if (store != null)
                    {
                        cmbStore.SelectedData = new DataEntity(store.ID, store.Text);
                    }
                }

                if (searchCriteria.FromDate != null)
                {
                    dtDateFrom.Value = (DateTime)searchCriteria.FromDate;
                    dtDateFrom.Checked = true;
                }

                if (searchCriteria.ToDate != null)
                {
                    dtDateTo.Value = (DateTime)searchCriteria.ToDate;
                    dtDateTo.Checked = true;
                }

                if (searchCriteria.SentFrom != null)
                {
                    dtDateFrom.Value = (DateTime)searchCriteria.SentFrom;
                    dtDateFrom.Checked = true;
                }

                if (searchCriteria.SentTo != null)
                {
                    dtDateTo.Value = (DateTime)searchCriteria.SentTo;
                    dtDateTo.Checked = true;
                }

                if (searchCriteria.ExpectedFrom != null)
                {
                    dtDueDateFrom.Value = (DateTime)searchCriteria.ExpectedFrom;
                    dtDueDateFrom.Checked = true;
                }

                if (searchCriteria.ExpectedTo != null)
                {
                    dtDueDateTo.Value = (DateTime)searchCriteria.ExpectedTo;
                    dtDueDateTo.Checked = true;
                }
            }
        }

        private void BuddyControlEnter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = (Control)sender;
            touchKeyboard.DelayedEnabled = true;
            touchKeyboard.KeystrokeMode = true;
        }

        private void BuddyControlLeave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;
        }

        private void cmbRequestingStore_RequestData(object sender, EventArgs e)
        {
            cmbStore.SetData(Providers.StoreData.GetList(entry), null);
        }

        private void cmbRequestingStore_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void CheckSearchCriteriaEntered(object sender, EventArgs e)
        {
            bool searchCriteriaEntered = tbIDOrDescription.Text != "" || !RecordIdentifier.IsEmptyOrNull(cmbStore.SelectedDataID)
                                        || dtDateFrom.Checked || dtDateTo.Checked || dtDueDateFrom.Checked || dtDueDateTo.Checked;

            btnOk.Enabled = searchCriteriaEntered;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            UpdateSearchCriteria();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void tbIDOrDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOk_Click(this, new EventArgs());
        }
    }
}
