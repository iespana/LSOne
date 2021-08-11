using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.EndOfDay.Dialogs
{
    public partial class TenderdeclarationDialog : DialogBase
    {
        private Tenderdeclaration tenderdeclaration = new Tenderdeclaration();
        private RecordIdentifier preferredStore;
        private Currency storeCurrency;
        private DateTime initialDateTime = new DateTime(1, 1, 1);

        public TenderdeclarationDialog(Tenderdeclaration td)
            : this(td.StoreID)
        {
            tenderdeclaration = td;
            cmbStore.SelectedData = Providers.StoreData.Get(PluginEntry.DataModel, td.StoreID);
            cmbTerminal.SelectedData = Providers.TerminalData.Get(PluginEntry.DataModel, td.TerminalID, td.StoreID);
            SetView();
        }
        
        public TenderdeclarationDialog(RecordIdentifier storeID)
            : this()
        {
            SetStore(storeID);
        }

        public TenderdeclarationDialog()
        {
            InitializeComponent();

            //newCountedTime = DateTime.Now;

            RecordIdentifier storeID = PluginEntry.DataModel.CurrentStoreID;

            // storeID == RecordIdentifier.Empty means we are working from head office
            if (storeID != RecordIdentifier.Empty)
            {
                SetStore(storeID);
            }

            tenderdeclaration.CountedTime = initialDateTime;
            lblGroupHeader.Text = "";
            SetView();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Avoid the Microsoft memory leak error on ListViews
            lvCountResult.SmallImageList = null;
        }

        private void SetStore(RecordIdentifier storeID)
        {
            preferredStore = storeID;
            cmbStore.SelectedData = Providers.StoreData.Get(PluginEntry.DataModel,(string)storeID.PrimaryID);
            cmbStore_SelectedDataChanged(null, null);
        }

        public Tenderdeclaration Tenderdeclaration
        {
            get { return tenderdeclaration; }
        }

        public DataEntity SelectedStore
        {
            get { return (DataEntity)cmbStore.SelectedData; }
        }

        public DataEntity SelectedTerminal
        {
            get { return (DataEntity)cmbTerminal.SelectedData; }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void SetView()
        {
            DrawList();
            // Only enable the store selection if no preferred store is set and no items have been counted.
            cmbStore.Enabled = (lvCountResult.Items.Count == 0) && ((preferredStore == null) || ((string)preferredStore.PrimaryID == ""));
            cmbTerminal.Enabled = (cmbStore.Text != string.Empty) && (lvCountResult.Items.Count == 0);
            cmbPayments.Enabled = cmbStore.Text != string.Empty;
            cmbCurrency.Enabled = (cmbPayments.Text != string.Empty) && (cmbPayments.SelectedData != null) && (((Payment)(cmbPayments.SelectedData)).IsForeignCurrency);
            cmbDenominator.Enabled = (cmbCurrency.Text != string.Empty) && (cmbPayments.Enabled) && (cmbPayments.Text != string.Empty);
            btnAdd.Enabled = ntbCountedQuantity.Value != 0;
            btnRemove.Enabled = lvCountResult.Items.Count > 0;
            btnOK.Enabled = lvCountResult.Items.Count > 0;
            lvCountResult.Enabled = lvCountResult.Items.Count > 0;
            ntbCountedQuantity.Enabled = cmbDenominator.Text != string.Empty;
            lblTotalAmountCounted.Visible = lvCountResult.Items.Count > 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lvCountResult.Items.Count < 1)
                MessageDialog.Show(Properties.Resources.NothingCountedYet);
            else
            {
                tenderdeclaration.StoreID = (string)cmbStore.SelectedData.ID;
                tenderdeclaration.TerminalID = (string)cmbTerminal.SelectedData.ID;
                if (tenderdeclaration.CountedTime == initialDateTime)
                    tenderdeclaration.CountedTime = DateTime.Now;

                foreach (var tdLine in tenderdeclaration.TenderDeclarationLines)
                {
                    tdLine.CountedDateTime = tenderdeclaration.CountedTime;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        public RecordIdentifier GetSelectedId()
        {
           // return statement.ID;
            return new RecordIdentifier();
        }

        private void DateValueChanged(object sender, EventArgs e)
        {
            //if (statement != null)
            {
                //if (Date.FromDateControl(dtpPostingDate).DateTime != dtpPostingDate.Value) btnOK.Enabled = true;
                //if (statement.StartingTime != dtpStartingTime.Value) btnOK.Enabled = true;
                //if (statement.EndingTime  != dtpStartingDate.Value) btnOK.Enabled = true;
            }
            //else
            {
                btnOK.Enabled = true;
            }
        }

        private void FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData.ID != "")
            {
                e.TextToDisplay = ((DualDataComboBox)sender).SelectedData.ID + " - " + ((DualDataComboBox)sender).SelectedData.Text;
            }
            else
            {
                e.TextToDisplay = "";
            }
        }

        private void declaredTenderButtons_ButtonPressed(int index, object tag)
        {
            lblGroupHeader.Text = Properties.Resources.EnterCountingFor + ((Payment)tag).Text;
            SetView();
        }

        private void cmbStore_TextChanged(object sender, EventArgs e)
        {
            SetView();
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> stores = Providers.StoreData.GetList(PluginEntry.DataModel);
            cmbStore.SetData(stores, null);
            SetView();
        }

        private void cmbStore_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbStore.SelectedData != null)
            {
                cmbStore.Tag = Providers.StoreData.Get(PluginEntry.DataModel, (string)cmbStore.SelectedData.ID);
                storeCurrency = Providers.CurrencyData.Get(PluginEntry.DataModel, ((Store)cmbStore.Tag).Currency);
                if (storeCurrency != null)
                {
                    cmbCurrency.SelectedData = new DataEntity(storeCurrency.ID, storeCurrency.Text);
                }
                
            }
            SetView();
        }

        private void cmbTerminal_RequestData(object sender, EventArgs e)
        {
            // Populating the list of terminals
            List<DataEntity> terminals = new List<DataEntity>(Providers.TerminalData.GetList(PluginEntry.DataModel, cmbStore.SelectedData.ID));
            cmbTerminal.SetData(terminals, null);
            SetView();
        }

        private void cmbTerminal_SelectedDataChanged(object sender, EventArgs e)
        {
            SetView();
        }

        private void cmbPayments_RequestData(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            // Retrieving a list of payments marked as countable
            List<Payment> payments = Providers.StoreData.GetCountableStorePayments(PluginEntry.DataModel, (string)cmbStore.SelectedData.ID);
            cmbPayments.SetData(payments, null);
            SetView();
            if (payments.Count < 1)
            {
                errorProvider1.SetError(cmbPayments, Properties.Resources.NoCountingRequired);
            }
        }

        private void cmbPayments_SelectedDataChanged(object sender, EventArgs e)
        {
            lblGroupHeader.Text = Properties.Resources.EnterCountingFor + " " + ((DualDataComboBox)sender).SelectedData.Text;
            if (((Payment)((sender as DualDataComboBox).SelectedData)).IsForeignCurrency)
            {
                cmbCurrency.Clear();
                cmbCurrency.SelectedData = new DataEntity();
                cmbCurrency.Enabled = true;
            }
            else
            {
                Currency storeCurrency = Providers.CurrencyData.Get(PluginEntry.DataModel,((Store)cmbStore.Tag).Currency);

                if (storeCurrency != null)
                {
                    cmbCurrency.SelectedData = new DataEntity(storeCurrency.ID, storeCurrency.Text);
                    cmbCurrency.Enabled = false;
                }
                
            }
            cmbDenominator.SelectedData = new DataEntity();
            cmbDenominator.Enabled = true;
            
            SetView();
        }

        private void cmbCurrency_RequestData(object sender, EventArgs e)
        {
            // Retrieving a list of payments marked as countable
            List<Currency> currencies = Providers.CurrencyData.GetNonStoreCurrencies(PluginEntry.DataModel, ((Store)cmbStore.Tag).Currency);
            cmbCurrency.SetData(currencies, null);
            SetView();
        }

        private void cmbCurrency_SelectedDataChanged(object sender, EventArgs e)
        {
            cmbCurrency.Tag = Providers.CurrencyData.Get(PluginEntry.DataModel, cmbCurrency.SelectedData.ID);
            cmbDenominator.SelectedData = new DataEntity();
            SetView();
        }        

        private void cmbDenominator_RequestData(object sender, EventArgs e)
        {
            cmbDenominator.SetWidth(200);

            cmbDenominator.SetHeaders(new string[] { Properties.Resources.Currency, Properties.Resources.Amount, Properties.Resources.Type }, new int[] { 0, 1, 2 });

            List<CashDenominator> denominators = Providers.CashDenominatorData.GetCashDenominators(PluginEntry.DataModel, cmbCurrency.SelectedData.ID, 1, false);

            foreach (CashDenominator cd in denominators)
            {
                cd.FormattedAmount = cd.Amount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                cd.Text = cd.CashType.ToString();
            }

            CashDenominator newCashDec = new CashDenominator();
            //newCashDec.PaymentType = (string)cmbPayments.SelectedData.ID;
            //newCashDec.PaymentText = cmbPayments.SelectedData.Text;
            newCashDec.CurrencyCode = (string)cmbCurrency.SelectedData.ID;
            newCashDec.CashType = CashDenominator.Type.TotalAmount;
            
            denominators.Insert(0, newCashDec);

            cmbDenominator.SetData(denominators, null);
        }

        private void cmbDenominator_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == "" || ((DataEntity)e.Data).ID == RecordIdentifier.Empty)
            {
                e.TextToDisplay = "";
            }
            else
            {
                if (((CashDenominator)e.Data).CashType == CashDenominator.Type.TotalAmount)
                    e.TextToDisplay = ((CashDenominator)e.Data).CurrencyCode + " - " + Properties.Resources.TotalAmount;
                else
                    e.TextToDisplay = ((CashDenominator)e.Data).FormattedAmount + " - " + ((DataEntity)e.Data).Text;
            }

        }

        private void btnAddFast_Click(object sender, EventArgs e)
        {
            try
            {
                TenderdeclarationLine countedCD = new TenderdeclarationLine();
                countedCD.CountedDateTime = DateTime.Now;
                countedCD.PaymentTypeID = (string)cmbPayments.SelectedData.ID;
                countedCD.PaymentTypeText = cmbPayments.SelectedData.Text;
                countedCD.Denominator.CurrencyCode = (string)cmbCurrency.SelectedData.ID;
                if (((CashDenominator)cmbDenominator.SelectedData).CashType == CashDenominator.Type.TotalAmount)
                {
                    countedCD.Denominator.Amount = Convert.ToDecimal(ntbCountedQuantity.Text);
                    countedCD.Quantity = 1;
                }
                else
                {
                    countedCD.Denominator.Amount = (decimal)cmbDenominator.SelectedData.ID;
                    countedCD.Quantity = Convert.ToInt32(ntbCountedQuantity.Text);
                }
                
                countedCD.Denominator.CashType = (CashDenominator.Type)(int)cmbDenominator.SelectedData.ID.SecondaryID;
                countedCD.IsLocalCurrency = (((Payment)cmbPayments.SelectedData).IsForeignCurrency == false);

                Int32 index = 0;
                while (index < tenderdeclaration.TenderDeclarationLines.Count)
                {
                    // are we updating an existing row, and therefor we need to remove the existing line before adding the current?
                    if ( // For lines marked for special cash types, we look for matching amount and denominator code                    
                            (tenderdeclaration.TenderDeclarationLines[index].Denominator.CashType != CashDenominator.Type.TotalAmount)
                            &&
                            (tenderdeclaration.TenderDeclarationLines[index].Denominator.Amount == countedCD.Denominator.Amount)
                            &&
                            (tenderdeclaration.TenderDeclarationLines[index].Denominator.CurrencyCode == countedCD.Denominator.CurrencyCode)
                        )
                    {
                        tenderdeclaration.TenderDeclarationLines.RemoveAt(index);
                    }
                    else if ( // For lines marked as total amount it is enough to find matching currency code
                            (tenderdeclaration.TenderDeclarationLines[index].Denominator.CurrencyCode == countedCD.Denominator.CurrencyCode)
                            &&
                            (tenderdeclaration.TenderDeclarationLines[index].Denominator.CashType == CashDenominator.Type.TotalAmount)
                        )
                    {
                        tenderdeclaration.TenderDeclarationLines.RemoveAt(index);
                    }
                    else
                    {
                        index++;
                    }
                }
                tenderdeclaration.TenderDeclarationLines.Add(countedCD);
            }
            finally
            {
                ntbCountedQuantity.Text = "";
                SetView();
                ntbCountedQuantity.Focus();
            }
        }

        private void DrawList()
        {
            lvCountResult.Items.Clear();

            var finalList = from x in tenderdeclaration.TenderDeclarationLines orderby x.PaymentTypeID, x.Denominator.CurrencyCode, x.Denominator.Amount select x;

            decimal totalAmountCounted = 0;

            foreach (TenderdeclarationLine cd in finalList)
            {
                cd.Denominator.FormattedAmount = cd.Denominator.Amount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                cd.FormattedTotalAmount = (cd.Quantity * cd.Denominator.Amount).FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                Payment payment = Providers.StoreData.GetStorePayment(PluginEntry.DataModel, (string)SelectedStore.ID, (string)cd.PaymentTypeID);
                if (payment != null)
                {
                    cd.PaymentTypeText = payment.Text;
                }
                else
                {
                    cd.PaymentTypeText = Properties.Resources.NotFound;
                }

                ListViewItem item = new ListViewItem(cd.PaymentTypeText);
                item.SubItems.Add(cd.Denominator.CurrencyCode);
                item.SubItems.Add(cd.Denominator.FormattedAmount);
                item.SubItems.Add(cd.Denominator.CashType.ToString());
                item.SubItems.Add(cd.Quantity.ToString());
                
                item.SubItems.Add(cd.FormattedTotalAmount);
                lvCountResult.Add(item);
                if (cd.IsLocalCurrency)
                    totalAmountCounted += cd.Denominator.Amount * cd.Quantity;
            }
            string currText = (storeCurrency == null ? " : " : " (" + storeCurrency + "): ");
            lblTotalAmountCounted.Text = Properties.Resources.TotalForLocalCurr + currText + totalAmountCounted.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
        }
        
        private void txtCountedQuantity_TextChanged(object sender, EventArgs e)
        {
            SetView();
        }

        private void cmbDenominator_SelectedDataChanged(object sender, EventArgs e)
        {
            if (((CashDenominator)cmbDenominator.SelectedData).CashType == CashDenominator.Type.TotalAmount)
            {
                lblQtyTotal.Text = Properties.Resources.TotalAmount;
                ntbCountedQuantity.AllowDecimal = true;
            }
            else
            {
                lblQtyTotal.Text = Properties.Resources.Quantity;
                ntbCountedQuantity.AllowDecimal = false;
            }
            SetView();
            ntbCountedQuantity.Text = "";
            ntbCountedQuantity.Focus();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvCountResult.SelectedItems.Count < 1)
            {
                MessageDialog.Show(Properties.Resources.SelectRemoveCount);
            }
            else
            {
                if (lvCountResult.Items[lvCountResult.SelectedIndices[0]] != null)
                {
                    string payment = lvCountResult.Items[lvCountResult.SelectedIndices[0]].SubItems[0].Text;
                    string currency = lvCountResult.Items[lvCountResult.SelectedIndices[0]].SubItems[1].Text;
                    string amount = lvCountResult.Items[lvCountResult.SelectedIndices[0]].SubItems[2].Text;

                    if (QuestionDialog.Show(Properties.Resources.RemoveConfirm.Replace("#1", currency).Replace("#2", amount), Properties.Resources.RemoveCount) == System.Windows.Forms.DialogResult.Yes)
                    {

                        int inx = 0;
                        while (inx < tenderdeclaration.TenderDeclarationLines.Count)
                        {
                            TenderdeclarationLine ca = tenderdeclaration.TenderDeclarationLines[inx];
                            if ((payment == ca.PaymentTypeText) && (currency == ca.Denominator.CurrencyCode) && (amount == ca.Denominator.FormattedAmount))
                            {
                                Providers.TenderDeclarationLineData.Delete(PluginEntry.DataModel, ca.ID);
                                tenderdeclaration.TenderDeclarationLines.RemoveAt(inx);
                                break;
                            }
                            inx++;
                        }
                        SetView();
                    }
                }
            }
        }

        private void ntbCountedQuantity_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = ntbCountedQuantity.Value != 0;
        }
    }
}
