using LSOne.Controls.Dialogs.Properties;
using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSOne.Controls.Dialogs
{
    public partial class PayCustomerAccountDialog : TouchBaseForm
    {
        private decimal amountDue;
        private decimal restrictedAmount;
        private StorePaymentMethod tenderInformation;
        private bool valid;
        private bool doEvents;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        private ISession session;
        // transSalePmtDiff contains the original payable amount calculated by CustomerAccountPayment; 
        // this is the correct amount to use for payable amount because it takes into consideration all data: total transaction amount, what was payed/refund so far, 
        // and what can't be payed due to payment limitations in a return transaction due to the setting ForceRefundWithTheSamepaymentType
        private decimal transSalePmtDiff;

        public IRetailTransaction Transaction { get; set; }

        public Customer Customer { get; private set; }

        public decimal RegisteredAmount { get; private set; }

        public PayCustomerAccountDialog()
        {
            InitializeComponent();
        }

        public PayCustomerAccountDialog(IConnectionManager entry, ISession session, decimal amountDue, StorePaymentMethod tenderInformation, IRetailTransaction transaction)
            : this()
        {
            this.session = session;

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            this.transSalePmtDiff = amountDue;
            this.amountDue = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundAmount(DLLEntry.DataModel, 
                                                                                        amountDue, 
                                                                                        DLLEntry.DataModel.CurrentStoreID, 
                                                                                        tenderInformation.ID.SecondaryID, 
                                                                                        CacheType.CacheTypeTransactionLifeTime);

            this.tenderInformation = tenderInformation;
            Transaction = transaction;
            ntbAmount.DecimalLetters =  tenderInformation.Rounding.DigitsBeforeFirstSignificantDigit();
            ntbAmount.AllowDecimal =  ntbAmount.DecimalLetters > 0;
            ntbAmount.AllowNegative = this.amountDue < decimal.Zero;
            ntbAmount.Value = (double)this.amountDue;
            tbCustomer.Text = "";
            SetBanner();

            if (!DesignMode)
            {
                tbCustomer.StartCharacter = DLLEntry.Settings.HardwareProfile.StartTrack1;
                tbCustomer.EndCharacter = DLLEntry.Settings.HardwareProfile.EndTrack1;
                tbCustomer.Seperator = DLLEntry.Settings.HardwareProfile.Separator1;
                tbCustomer.TrackSeperation = TrackSeperation.Before;
            }
           
            if (transaction.Customer.ID != RecordIdentifier.Empty)
            {
                Customer = transaction.Customer;
                btnOk.Enabled = valid = true;
                tbCustomer.Text = transaction.Customer.FirstName != "" ? 
                    transaction.Customer.GetFormattedName(DLLEntry.DataModel.Settings.NameFormatter) :
                    transaction.Customer.Text;

                if (!transaction.CustomerOrder.Empty()
                    &&
                    (transaction.CustomerOrder.CurrentAction == CustomerOrderAction.PartialPickUp
                     || transaction.CustomerOrder.CurrentAction == CustomerOrderAction.FullPickup))
                {
                    tbCustomer.ReadOnly = true;
                    btnSearch.Enabled = false;
                }
            }
            else
            {
                Customer = new Customer();
            }

            tbCustomer.GotFocus += tbCustomerOnGotFocus;
            tbCustomer.LostFocus += tbCustomerOnLostFocus;
            tbCustomer.TextChanged += tbCustomer_TextChanged;
            tbCustomer.KeyDown += tbCustomer_KeyDown;

            doEvents = true;
        }

        private void ProcessScannedItem(ScanInfo scanInfo)
        {
            try
            {
                BarCode barCode = Services.Interfaces.Services.BarcodeService(DLLEntry.DataModel).ProcessBarcode(DLLEntry.DataModel, scanInfo);
                DisplayCustomer(FindCustomer(barCode.CustomerId, scanInfo));
            }
            finally
            {
                 Scanner.ReEnableForScan();
            }
        }

        private void tbCustomerOnLostFocus(object sender, EventArgs eventArgs)
        {
            ValidateCustomer();
        }

        private void tbCustomerOnGotFocus(object sender, EventArgs args)
        {
            tbCustomer.HasError = false;
        }

        private void ValidateCustomer()
        {
            touchErrorProvider.Visible = false;
            touchErrorProvider.ErrorText = "";
            tbCustomer.HasError = false;

            if(tbCustomer.Text == "")
            {
                touchErrorProvider.ErrorText = Properties.Resources.PleaseSelectCustomer;
            }
            else if(!valid)
            {
                touchErrorProvider.ErrorText = Properties.Resources.CustomerNotFound;
            }

            if(touchErrorProvider.ErrorText != "")
            {
                touchErrorProvider.Visible = true;
                tbCustomer.HasError = true;
            }
        }

        private void tbCustomer_TextChanged(object sender, EventArgs e)
        {
            if (valid && doEvents)
            {
                valid = false;
                IPosTransaction tempTransaction = Transaction;
                Services.Interfaces.Services.TransactionService(DLLEntry.DataModel).ClearCustomerFromTransaction(dlgEntry, ref tempTransaction);
                Transaction = (IRetailTransaction) tempTransaction;
                btnOk.Enabled = false;
                ntbAmount.Value = (double) amountDue;
                DisplayCreditInformation();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (tbCustomer.Text.Trim() == "")
            {
                IPosTransaction posTransaction = Transaction;
                DLLEntry.Settings.POSApp.RunOperation(POSOperations.CustomerSearch, "", ref posTransaction); //Adds the customer to the transaction as well.
                Transaction = (IRetailTransaction)posTransaction;

                DisplayCustomer(Transaction.Customer);
                return;
            }
            
            DisplayCustomer(FindCustomer(tbCustomer.Text, new ScanInfo(tbCustomer.Text)));
        }

        /// <summary>
        /// Takes the customer information and checks for blocked values and then displays the customer information in the dialog
        /// </summary>
        /// <param name="customer">The customer to be displayed</param>
        private void DisplayCustomer(Customer customer)
        {
            valid = false;
            if (customer != null && !RecordIdentifier.IsEmptyOrNull(customer.ID))
            {
                if (customer.NonChargableAccount)
                {
                    Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.CustomerIsCashCustomer); //This account cannot be charged to. This customer is a cash customer.
                    valid = true;
                }
                else
                {
                    switch (customer.Blocked)
                    {
                        case BlockedEnum.All:
                            Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.CustomerBlocked);
                            ClearCustomerFromTransaction();
                            break;
                        case BlockedEnum.Invoice:
                            Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.CustomerLimitedToInvoice);
                            ClearCustomerFromTransaction();
                            break;
                        case BlockedEnum.Nothing:
                            Services.Interfaces.Services.TransactionService(DLLEntry.DataModel).AddCustomerToTransaction(DLLEntry.DataModel, session, Transaction, customer.ID, false);
                            Customer = customer;
                            AddCustomer();
                            break;
                    }
                }
            }
            else
            {
                tbCustomer.Select(0, tbCustomer.Text.Length);
                ClearCustomerFromTransaction();
            }

            ValidateCustomer();
        }

        /// <summary>
        /// Searching for a customer both by Customer ID and by a full barcode if a scanner was used
        /// </summary>
        /// <param name="customerID">The customer ID entered into the text box</param>
        /// <param name="scanInfo">The scan info from the barcode</param>
        /// <returns></returns>
        private Customer FindCustomer(RecordIdentifier customerID, ScanInfo scanInfo)
        {
            Customer customer = Providers.CustomerData.Get(DLLEntry.DataModel, customerID, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);

            if (customer == null && scanInfo != null)
            {
                customer = Providers.CustomerData.GetByCardNumber(DLLEntry.DataModel, scanInfo.ScanDataLabel, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);
            }

            //Search by name
            if(customer == null)
            {
                List<CustomerListItem> customers = Providers.CustomerData.Search(DLLEntry.DataModel, tbCustomer.Text, 0, 2, false, DataLayer.DataProviders.Customers.CustomerSorting.ID, false);

                if(customers.Count > 0)
                {
                    if(customers.Count == 1)
                    {
                        customer = Providers.CustomerData.Get(DLLEntry.DataModel, customers[0].ID, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);
                    }
                    else
                    {
                        IPosTransaction posTransaction = Transaction;
                        DLLEntry.Settings.POSApp.RunOperation(POSOperations.CustomerSearch, customerID.StringValue, ref posTransaction); //Adds the customer to the transaction as well.
                        Transaction = (IRetailTransaction)posTransaction;
                        customer = Transaction.Customer;
                    }
                }
            }

            return customer;
        }

        private void ClearCustomerFromTransaction()
        {
            if (Transaction.Customer.ID.StringValue == "")
            {
                return;
            }

            IPosTransaction posTransaction = Transaction;
            DLLEntry.Settings.POSApp.RunOperation(POSOperations.CustomerClear, null, ref posTransaction);
            Transaction = (IRetailTransaction) posTransaction;
            tbCustomer.Text = "";
            valid = false;
            amountDue = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundAmount(DLLEntry.DataModel,
                                                                                        GetPaymentAmount(),
                                                                                        DLLEntry.DataModel.CurrentStoreID,
                                                                                        tenderInformation.ID.SecondaryID,
                                                                                        CacheType.CacheTypeTransactionLifeTime);
            ntbAmount.Value = (double) amountDue;
            SetBanner();

            DisplayCreditInformation();
        }

        private decimal GetPaymentAmount()
        {
            if (Services.Interfaces.Services.TenderRestrictionService(dlgEntry).GetUnconfirmedTenderRestrictionAmount(dlgEntry, dlgSettings, Transaction, tenderInformation, ref restrictedAmount) == TenderRestrictionResult.Continue)
            {
                return restrictedAmount;
            }

            return transSalePmtDiff;
        }

        private void AddCustomer()
        {
            doEvents = false;

            tbCustomer.Text = Transaction.Customer.FirstName != "" ? 
                Transaction.Customer.GetFormattedName(DLLEntry.DataModel.Settings.NameFormatter) :
                Transaction.Customer.Text;

            doEvents = true;

            valid = btnOk.Enabled = true;
            
            Customer = Transaction.Customer;
            transSalePmtDiff = Transaction.TransSalePmtDiff;
            DisplayCreditInformation();

            decimal paymentAmount = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundAmount(DLLEntry.DataModel,
                                                                                        GetPaymentAmount(),
                                                                                        DLLEntry.DataModel.CurrentStoreID,
                                                                                        tenderInformation.ID.SecondaryID,
                                                                                        CacheType.CacheTypeTransactionLifeTime);
            if (paymentAmount != amountDue)
            {
                amountDue = paymentAmount;
                ntbAmount.Value = (double)amountDue;
                SetBanner();
            }
            ntbAmount.Focus();
        }

        private void touchKeyboard1_EnterPressed(object sender, EventArgs e)
        {
            if (ActiveControl is Button)
            {
                ((Button) ActiveControl).PerformClick();
            }
            else if (ActiveControl is ShadeTextBoxTouch)
            {
                SelectNextControl(ActiveControl, true, true, false, true);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var rounding = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel);
            RegisteredAmount = rounding.RoundAmount(DLLEntry.DataModel, (decimal)ntbAmount.Value, DLLEntry.DataModel.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            if (RegisteredAmount != 0 || (amountDue != 0 && amountDue == RegisteredAmount))
            {
                if (ntbAmount.AllowNegative)
                {
                    if (RegisteredAmount > 0)
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Resources.OnlyNegativeAmountsAllowed, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return;
                    }
                    if (RegisteredAmount < amountDue)
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Resources.NegativePaymentsNotBelowBalance, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return;
                    }
                }
            
                DialogResult = DialogResult.OK;
                Close();
            }
            else 
            {
                Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.PleaseEnterAmountToPay, MessageBoxButtons.OK, MessageDialogType.Generic);
            }      
        }

        private void touchKeyboard1_ObtainCultureName(object sender, CultureEventArguments args)
        {
            if (DLLEntry.Settings.UserProfile.KeyboardCode != "")
            {
                args.CultureName = DLLEntry.Settings.UserProfile.KeyboardCode;
                args.LayoutName = DLLEntry.Settings.UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = DLLEntry.Settings.Store.KeyboardCode;
                args.LayoutName = DLLEntry.Settings.Store.KeyboardLayoutName;
            }
        }

        private void SetBanner()
        {
            touchDialogBanner1.BannerText = tenderInformation.Text;
            touchDialogBanner1.BannerText += " - ";
            touchDialogBanner1.BannerText += Resources.AmountDue.Replace("#1",
                                                                ((IRoundingService)DLLEntry.DataModel.Service(ServiceType.RoundingService)).RoundString(DLLEntry.DataModel,
                                                                                                                                          amountDue,
                                                                                                                                          DLLEntry.Settings.Store.Currency,
                                                                                                                                          true,
                                                                                                                                          CacheType.CacheTypeApplicationLifeTime));
        }

        private void ntbAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator && ntbAmount.Text == "")
            {
                ntbAmount.Text = "0" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                ntbAmount.Select(ntbAmount.Text.Length, 0);
            }
        }

        private void PayCustomerAccountDialog_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                MSR.MSRMessageEvent += MSR_MSRMessageEvent;
                MSR.EnableForSwipe();

                Scanner.ScannerMessageEvent += ProcessScannedItem;
                Scanner.ReEnableForScan();

                if (Transaction.Customer.ID != RecordIdentifier.Empty)
                {
                    ntbAmount.Select();
                }

                DisplayCreditInformation();
            }
        }

        private void DisplayCreditInformation()
        {
            IRoundingService rounding = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel);

            if (valid)
            {
                lblCreditLimit.Text = Resources.GatheringInformation;
                lblBalance.Text = Resources.GatheringInformation;

                decimal balance = 0;
                decimal maxCredit = 0;
                bool success = false;
                ISiteServiceService siteServiceService = Services.Interfaces.Services.SiteServiceService(DLLEntry.DataModel);

                Task balanceTask = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        balance = siteServiceService.GetCustomerBalance(DLLEntry.DataModel, dlgSettings.SiteServiceProfile, Customer.ID, true, false);
                        maxCredit = siteServiceService.GetCustomer(DLLEntry.DataModel, dlgSettings.SiteServiceProfile, Customer.ID, true, true).MaxCredit;
                        success = true;
                    }
                    catch { }
                });

                balanceTask.ContinueWith((t) => 
                {
                    if(!Disposing && !IsDisposed)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            lblBalance.Text = success ? rounding.RoundForDisplay(DLLEntry.DataModel, balance + maxCredit, true, false, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime) : Resources.NoInformationAvailable;
                            lblCreditLimit.Text = success ? rounding.RoundForDisplay(DLLEntry.DataModel, maxCredit, true, false, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime) : Resources.NoInformationAvailable;
                        }));
                    }
                }, TaskContinuationOptions.ExecuteSynchronously);
            }
            else
            {
                lblBalance.Text = rounding.RoundForDisplay(DLLEntry.DataModel, 0, true, false, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                lblCreditLimit.Text = rounding.RoundForDisplay(DLLEntry.DataModel, 0, true, false, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
            }
        }

        private void PayCustomerAccountDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!DesignMode)
            {
                MSR.MSRMessageEvent -= MSR_MSRMessageEvent;
                MSR.DisableForSwipe();

                Scanner.ScannerMessageEvent -= ProcessScannedItem;
                Scanner.DisableForScan();
            }

            tbCustomer.GotFocus -= tbCustomerOnGotFocus;
            tbCustomer.LostFocus -= tbCustomerOnLostFocus;
            tbCustomer.TextChanged -= tbCustomer_TextChanged;
            tbCustomer.KeyDown -= tbCustomer_KeyDown;
        }

        void MSR_MSRMessageEvent(string track2)
        {
            tbCustomer.Text = StringExtensions.TrackBeforeSeparator(track2, DLLEntry.Settings.HardwareProfile.StartTrack1, DLLEntry.Settings.HardwareProfile.Separator1, DLLEntry.Settings.HardwareProfile.EndTrack1);
            btnSearch_Click(null, EventArgs.Empty);
        }

        private void tbCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbCustomer.Text = StringExtensions.TrackBeforeSeparator(tbCustomer.Text, DLLEntry.Settings.HardwareProfile.StartTrack1, DLLEntry.Settings.HardwareProfile.Separator1, DLLEntry.Settings.HardwareProfile.EndTrack1);
                btnSearch_Click(null, EventArgs.Empty);
            }
        }

        private void pnlInfo_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, pnlInfo.Width - 1, pnlInfo.Height - 1);
            p.Dispose();
        }
    }
}