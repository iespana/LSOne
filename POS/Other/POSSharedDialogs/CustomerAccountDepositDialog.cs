using LSOne.Controls.Dialogs.Properties;
using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.CustomerDepositItem;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
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
    public partial class CustomerAccountDepositDialog : TouchBaseForm
    {
        private readonly IConnectionManager dlgEntry;
        private readonly ISession dlgSession;
        private readonly ISettings dlgSettings;

        private bool valid;
        private bool doEvents;
        private Currency storeCurrency;
        private StorePaymentMethod tenderInformation;

        /// <summary>
        /// Currently selected customer.
        /// </summary>
        protected Customer SelectedCustomer { get; private set; }

        /// <summary>
        /// Current transaction.
        /// </summary>
        protected IPosTransaction Transaction { get; private set; }

        /// <summary>
        /// Entered amount for deposit on customer account.
        /// </summary>
        protected decimal Amount { get; private set; }

        public CustomerAccountDepositDialog()
        {
            InitializeComponent();
        }

        public CustomerAccountDepositDialog(IConnectionManager entry, ISession session, ISettings settings, IPosTransaction transaction, decimal initialAmount = 0)
            : this()
        {
            dlgEntry = entry;
            dlgSession = session;
            dlgSettings = settings;
            Transaction = transaction;

            storeCurrency = Providers.CurrencyData.Get(entry, dlgSettings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);

            tenderInformation = null;

            SetButtonPanel();

            var decimals = storeCurrency.RoundOffAmount.DigitsBeforeFirstSignificantDigit();
            ntbAmount.DecimalLetters = decimals;
            ntbAmount.AllowDecimal = true;

            if (!DesignMode)
            {
                tbCustomer.StartCharacter = dlgSettings.HardwareProfile.StartTrack1;
                tbCustomer.EndCharacter = dlgSettings.HardwareProfile.EndTrack1;
                tbCustomer.Seperator = dlgSettings.HardwareProfile.Separator1;
                tbCustomer.TrackSeperation = TrackSeperation.Before;
            }

            if (((CustomerPaymentTransaction)Transaction).Customer.ID != RecordIdentifier.Empty)
            {
                SelectedCustomer = ((CustomerPaymentTransaction)Transaction).Customer;
                DisplayCustomer();
                btnOk.Enabled = valid = true;
            }
            else
            {
                DisplayCreditInformation();
            }

            ntbAmount.Text = initialAmount == 0 ? "" : initialAmount.ToString(CultureInfo.CurrentCulture);

            ntbAmount.GotFocus += textBoxGotFocus;
            ntbAmount.LostFocus += textBoxLostFocus;
            tbCustomer.GotFocus += textBoxGotFocus;
            tbCustomer.LostFocus += tbCustomerOnLostFocus;
            tbCustomer.TextChanged += tbCustomer_TextChanged;
            tbCustomer.KeyDown += tbCustomer_KeyDown;

            doEvents = true;
        }

        private void CustomerAccountDepositDialog_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                MSR.MSRMessageEvent += MSR_MSRMessageEvent;
                MSR.EnableForSwipe();

                Scanner.ScannerMessageEvent += ProcessScannedItem;
                Scanner.ReEnableForScan();

                if (SelectedCustomer != null && SelectedCustomer.ID != RecordIdentifier.Empty)
                {
                    ntbAmount.Select();
                }
            }
        }

        private void CustomerAccountDepositDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!DesignMode)
            {
                MSR.MSRMessageEvent -= MSR_MSRMessageEvent;
                MSR.DisableForSwipe();

                Scanner.ScannerMessageEvent -= ProcessScannedItem;
                Scanner.DisableForScan();
            }

            tbCustomer.GotFocus -= textBoxGotFocus;
            tbCustomer.LostFocus -= tbCustomerOnLostFocus;
            ntbAmount.GotFocus -= textBoxGotFocus;
            ntbAmount.LostFocus -= textBoxLostFocus;
            tbCustomer.TextChanged -= tbCustomer_TextChanged;
            tbCustomer.KeyDown -= tbCustomer_KeyDown;
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
                        balance = siteServiceService.GetCustomerBalance(DLLEntry.DataModel, dlgSettings.SiteServiceProfile, SelectedCustomer.ID, true, false);
                        maxCredit = siteServiceService.GetCustomer(DLLEntry.DataModel, dlgSettings.SiteServiceProfile, SelectedCustomer.ID, true, true).MaxCredit;
                        success = true;
                    }
                    finally
                    {
                        if (!Disposing && !IsDisposed)
                        {
                            Invoke(new Action(() =>
                            {
                                lblBalance.Text = success ? rounding.RoundForDisplay(DLLEntry.DataModel, balance, true, false, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime) : Resources.NoInformationAvailable;
                                lblCreditLimit.Text = success ? rounding.RoundForDisplay(DLLEntry.DataModel, maxCredit, true, false, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime) : Resources.NoInformationAvailable;
                            }));
                        }
                    }
                });
            }
            else
            {
                lblBalance.Text = rounding.RoundForDisplay(DLLEntry.DataModel, 0, true, false, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                lblCreditLimit.Text = rounding.RoundForDisplay(DLLEntry.DataModel, 0, true, false, dlgSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
            }
        }

        private void tbCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbCustomer.Text = StringExtensions.TrackBeforeSeparator(tbCustomer.Text, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);
                btnSearch_Click(null, EventArgs.Empty);
            }
        }

        private void tbCustomer_TextChanged(object sender, EventArgs e)
        {
            if (valid && doEvents)
            {
                ClearCustomer();
            }
        }

        private void tbCustomerOnLostFocus(object sender, EventArgs eventArgs)
        {
            ShadeTextBoxTouch control = sender is TextBox ? (ShadeTextBoxTouch)((TextBox)sender).Parent : (ShadeTextBoxTouch)sender;
        }

        private void textBoxLostFocus(object sender, EventArgs args)
        {
            ShadeTextBoxTouch control = sender is TextBox ? (ShadeTextBoxTouch)((TextBox)sender).Parent : (ShadeTextBoxTouch)sender;
        }

        private void textBoxGotFocus(object sender, EventArgs args)
        {
            ShadeTextBoxTouch control = sender is TextBox ? (ShadeTextBoxTouch)((TextBox)sender).Parent : (ShadeTextBoxTouch)sender;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (tbCustomer.Text.Trim() == "")
            {
                Customer customer = Services.Interfaces.Services.CustomerService(dlgEntry).Search(dlgEntry, true, Transaction);
                CheckAndDisplayCustomer(customer);
                return;
            }

            CheckAndDisplayCustomer(FindCustomer(tbCustomer.Text, new ScanInfo(tbCustomer.Text)));
        }

        private void scrollBtnPanel_Click(object sender, ScrollButtonEventArguments args)
        {
            if (tenderInformation == null)
            {
                return;
            }

            IRoundingService rounding = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel);
            decimal amount = rounding.RoundAmount(DLLEntry.DataModel, (decimal)args.Tag, DLLEntry.DataModel.CurrentStoreID, tenderInformation.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            ntbAmount.Value = (double)amount;
        }

        private void touchKeyboard_EnterPressed(object sender, EventArgs e)
        {
            if (ActiveControl is Button)
            {
                ((Button)ActiveControl).PerformClick();
            }
            else if (ActiveControl is TextBox)
            {
                SelectNextControl(ActiveControl, true, true, false, true);
            }
        }

        private void touchKeyboard_ObtainCultureName(object sender, CultureEventArguments args)
        {
            if (dlgSettings.UserProfile.KeyboardCode != "")
            {
                args.CultureName = dlgSettings.UserProfile.KeyboardCode;
                args.LayoutName = dlgSettings.UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = dlgSettings.Store.KeyboardCode;
                args.LayoutName = dlgSettings.Store.KeyboardLayoutName;
            }
        }

        private void ntbAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator && ntbAmount.Text == "")
            {
                ntbAmount.Text = "0" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                ntbAmount.Select(ntbAmount.Text.Length, 0);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Add customer and amount on transaction if a valid amount was entered
            Amount = ntbAmount.Text == "" ? 0 : Convert.ToDecimal(ntbAmount.Text, CultureInfo.CurrentCulture);
            if(Amount == 0)
            {
                btnCancel_Click(sender, e);
                return;
            }

            AddCustomerToTransaction();
            // Create the customer deposit item
            var depositItem = new CustomerDepositItem
            {
                Description = Resources.CustomerAccountDeposit,
                Amount = Amount
            };
            ((CustomerPaymentTransaction)Transaction).CustomerDepositItem = depositItem;
            Services.Interfaces.Services.CalculationService(dlgEntry).CalculateTotals(dlgEntry, Transaction);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearCustomer();
            DialogResult = DialogResult.Cancel;
            Close();
        }
   

        private void SetButtonPanel()
        {
            scrollBtnPanel.Clear();

            tenderInformation = Providers.StorePaymentMethodData.Get(dlgEntry, dlgSettings.Store.ID, PaymentMethodDefaultFunctionEnum.Normal);
            scrollBtnPanel.SetButtonsCurrency((decimal)ntbAmount.Value, tenderInformation, DLLEntry.Settings.Store.Currency, false, TouchScrollButtonsCurrencyHelper.ViewOptions.HeigherAndLowerAmounts, 1M);
            
        }

        private void MSR_MSRMessageEvent(string track2)
        {
            tbCustomer.Text = StringExtensions.TrackBeforeSeparator(track2, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);
            btnSearch_Click(null, EventArgs.Empty);
        }

        private void ProcessScannedItem(ScanInfo scanInfo)
        {
            try
            {
                BarCode barCode = Services.Interfaces.Services.BarcodeService(dlgEntry).ProcessBarcode(dlgEntry, scanInfo);
                CheckAndDisplayCustomer(FindCustomer(barCode.CustomerId, scanInfo));
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        private void ValidateCustomer()
        {
            errorProvider.Visible = false;
            errorProvider.ErrorText = "";
            tbCustomer.HasError = false;

            if (tbCustomer.Text == "")
            {
                errorProvider.ErrorText = Resources.PleaseSelectCustomer;
            }
            else if (!valid)
            {
                errorProvider.ErrorText = Resources.CustomerNotFound;
            }

            if (errorProvider.ErrorText != "")
            {
                errorProvider.Visible = true;
                tbCustomer.HasError = true;
            }
        }

        /// <summary>
        /// Takes the customer information and checks for blocked values and then displays the customer information in the dialog
        /// </summary>
        /// <param name="customer">The customer to be displayed</param>
        private void CheckAndDisplayCustomer(Customer customer)
        {
            valid = false;

            if (customer != null)
            {
                if (customer.NonChargableAccount)
                {
                    Services.Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CustomerIsCashCustomer); //This account cannot be charged to. This customer is a cash customer.
                    valid = true;
                }
                else
                {
                    switch (customer.Blocked)
                    {
                        case BlockedEnum.All:
                            Services.Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CustomerBlocked);
                            valid = false;
                            break;
                        case BlockedEnum.Invoice:
                            Services.Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CustomerLimitedToInvoice);
                            break;
                        case BlockedEnum.Nothing:
                            valid = true;
                            SelectedCustomer = customer;
                            DisplayCustomer();
                            break;
                    }
                }
            }
            else
            {
                tbCustomer.Select(0, tbCustomer.Text.Length);
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
            Customer customer = Providers.CustomerData.Get(dlgEntry, customerID, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);

            if (customer == null && scanInfo != null)
            {
                customer = Providers.CustomerData.GetByCardNumber(dlgEntry, scanInfo.ScanDataLabel, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);
            }

            //Search by name
            if (customer == null)
            {
                List<CustomerListItem> customers = Providers.CustomerData.Search(DLLEntry.DataModel, tbCustomer.Text, 0, 2, false, DataLayer.DataProviders.Customers.CustomerSorting.ID, false);

                if (customers.Count > 0)
                {
                    if (customers.Count == 1)
                    {
                        customer = Providers.CustomerData.Get(DLLEntry.DataModel, customers[0].ID, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);
                    }
                    else
                    {
                        customer = Services.Interfaces.Services.CustomerService(dlgEntry).Search(dlgEntry, true, Transaction, customerID.StringValue);
                    }
                }
            }

            return customer;
        }

        /// <summary>
        /// Populates the dialog with customer information from <see cref="SelectedCustomer"/> 
        /// </summary>
        private void DisplayCustomer()
        {
            if (SelectedCustomer != null)
            {
                doEvents = false;
                tbCustomer.Text = GetCustomerName();
                doEvents = true;

                valid = btnOk.Enabled = true;

                DisplayCreditInformation();

                ntbAmount.Focus();
            }
        }

        /// <summary>
        /// Clears SelectedCustomer field and the customer details from UI.
        /// </summary>
        private void ClearCustomer()
        {
            SelectedCustomer = null;
            valid = false;
            tbCustomer.Text = "";
            Amount = 0M;
            DisplayCreditInformation();
        }

        /// <summary>
        /// Returns the formatted customer name for the selected customer.
        /// </summary>
        /// <returns></returns>
        private string GetCustomerName()
        {
            if(SelectedCustomer != null)
            {
                return SelectedCustomer.FirstName != "" ?
                    SelectedCustomer.GetFormattedName(dlgEntry.Settings.NameFormatter) :
                    SelectedCustomer.Text;
            }

            return string.Empty;
        }

        /// <summary>
        /// Adds the <see cref="SelectedCustomer"/> to the current <see cref="Transaction"/> 
        /// </summary>
        private void AddCustomerToTransaction()
        {
            if (Transaction == null)
                return;
            
            Services.Interfaces.Services.TransactionService(dlgEntry).AddCustomerToTransaction(dlgEntry, dlgSession, Transaction, SelectedCustomer.ID, false);
        }

        private void pnlInfo_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, pnlInfo.Width - 1, pnlInfo.Height - 1);
            p.Dispose();
        }
    }
}
