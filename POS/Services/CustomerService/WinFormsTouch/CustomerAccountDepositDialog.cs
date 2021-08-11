using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.CustomerDepositItem;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    public partial class CustomerAccountDepositDialog : TouchBaseForm
    {
        private readonly IConnectionManager dlgEntry;
        private readonly ISession dlgSession;
        private readonly ISettings dlgSettings;

        private bool valid;
        private bool useScanner;
        private Currency storeCurrency;

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

            SetButtonPanel();
            tbAccountNumber.GotFocus += textBoxGotFocus;
            tbAccountNumber.LostFocus += tbAccountNumberOnLostFocus;

            var decimals = storeCurrency.RoundOffAmount.DigitsBeforeFirstSignificantDigit();
            numCurrentBalance.DecimalLetters = decimals;
            numCurrentBalance.AllowDecimal = true;
            ntbAmount.DecimalLetters = decimals;
            ntbAmount.AllowDecimal = true;
            ntbAmount.GotFocus += textBoxGotFocus;
            ntbAmount.LostFocus += textBoxLostFocus;

            if (!DesignMode)
            {
                tbAccountNumber.StartCharacter = dlgSettings.HardwareProfile.StartTrack1;
                tbAccountNumber.EndCharacter = dlgSettings.HardwareProfile.EndTrack1;
                tbAccountNumber.Seperator = dlgSettings.HardwareProfile.Separator1;
                tbAccountNumber.TrackSeperation = TrackSeperation.Before;
            }

            if (((CustomerPaymentTransaction)Transaction).Customer.ID != RecordIdentifier.Empty)
            {
                SelectedCustomer = ((CustomerPaymentTransaction)Transaction).Customer;

                DisplayCustomer();

                btnGet.Enabled = !(btnOk.Enabled = valid = lblCustomerName.Enabled = true);
            }

            ntbAmount.Text = initialAmount.ToString(CultureInfo.CurrentCulture);
        }

        private void CustomerAccountDepositDialog_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                MSR.MSRMessageEvent += MSR_MSRMessageEvent;
                MSR.EnableForSwipe();

                Scanner.ScannerMessageEvent += ProcessScannedItem;
                Scanner.ReEnableForScan();
                useScanner = false;

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
        }

        private void tbAccountNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbAccountNumber.Text = StringExtensions.TrackBeforeSeparator(tbAccountNumber.Text, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);

                btnGet_Click(null, EventArgs.Empty);
            }
        }

        private void tbAccountNumber_TextChanged(object sender, EventArgs e)
        {
            if (tbAccountNumber.Text != "" && !btnGet.Enabled)
            {
                btnGet.Enabled = true;
            }
            else if (tbAccountNumber.Text == "" && btnGet.Enabled)
            {
                btnGet.Enabled = false;
            }
            if (valid && this.SelectedCustomer != null && tbAccountNumber.Text != (string)SelectedCustomer.ID)
            {
                valid = false;
                ClearCustomer();
            }
        }

        private void tbAccountNumberOnLostFocus(object sender, EventArgs eventArgs)
        {
            ((TextBox)sender).BackColor = valid ? Color.White : Color.FromArgb(255, 160, 160);
        }

        private void textBoxLostFocus(object sender, EventArgs args)
        {
            ((TextBox)sender).BackColor = ColorPalette.White;
        }

        private void textBoxGotFocus(object sender, EventArgs args)
        {
            ((TextBox)sender).BackColor = ColorPalette.TextBoxGotFocus;
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            if (tbAccountNumber.Text.Trim() == "")
            {
                btnSearch_Click(sender, e);
                return;
            }

            CheckAndDisplayCustomer(FindCustomer(tbAccountNumber.Text, new ScanInfo(tbAccountNumber.Text)));
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var customer = Interfaces.Services.CustomerService(dlgEntry).Search(dlgEntry, true);

            CheckAndDisplayCustomer(customer);
        }

        private void scrollBtnPanel_Click(object sender, ScrollButtonEventArguments args)
        {
            if (args.Tag is int)
            {
                ntbAmount.Value = (double)((int)args.Tag);
            }
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
            if (dlgSettings.POSUser.KeyboardCode != "")
            {
                args.CultureName = dlgSettings.POSUser.KeyboardCode;
                args.LayoutName = dlgSettings.POSUser.KeyboardLayoutName;
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
            Interfaces.Services.CalculationService(dlgEntry).CalculateTotals(dlgEntry, Transaction);

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

            List<int> displayValues = new List<int> { 50, 2000, 5000, 10000 };
            IRoundingService rounding = (IRoundingService)dlgEntry.Service(ServiceType.RoundingService);

            displayValues.ForEach(dv =>
            {
                scrollBtnPanel.AddButton(rounding.RoundString(dlgEntry, dv, storeCurrency.ID, true), dv);
            });
        }

        private void MSR_MSRMessageEvent(string track2)
        {
            tbAccountNumber.Text = StringExtensions.TrackBeforeSeparator(track2, dlgSettings.HardwareProfile.StartTrack1, dlgSettings.HardwareProfile.Separator1, dlgSettings.HardwareProfile.EndTrack1);

            btnGet_Click(null, EventArgs.Empty);
        }

        private void ProcessScannedItem(ScanInfo scanInfo)
        {
            try
            {
                if (useScanner)
                {
                    Scanner.DisableForScan();
                }

                BarCode barCode = Services.Interfaces.Services.BarcodeService(dlgEntry).ProcessBarcode(dlgEntry, scanInfo);

                CheckAndDisplayCustomer(FindCustomer(barCode.CustomerId, scanInfo));

            }
            finally
            {
                if (useScanner)
                {
                    Scanner.ReEnableForScan();
                }
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
                    Services.Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.CustomerIsCashCustomer); //This account cannot be charged to. This customer is a cash customer.
                    valid = true;
                }
                else
                {

                    switch (customer.Blocked)
                    {
                        case Customer.BlockedEnum.All:
                            Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.CustomerBlocked);
                            valid = false;
                            break;
                        case Customer.BlockedEnum.Invoice:
                            Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.CustomerLimitedToInvoice);
                            break;
                        case Customer.BlockedEnum.Nothing:
                            valid = true;
                            SelectedCustomer = customer;
                            DisplayCustomer();
                            break;
                    }
                }
                tbAccountNumber.BackColor = ColorPalette.White;
            }
            else
            {
                tbAccountNumber.BackColor = ColorPalette.RedLight;
                tbAccountNumber.Select(0, tbAccountNumber.Text.Length);
            }
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

            return customer;
        }

        /// <summary>
        /// Populates the dialog with customer information from <see cref="SelectedCustomer"/> 
        /// </summary>
        private void DisplayCustomer()
        {
            if (SelectedCustomer != null)
            {
                tbCustomerName.Text = GetCustomerName();

                lblCustomerName.Enabled = valid = btnOk.Enabled = true;
                tbAccountNumber.BackColor = ColorPalette.White;

                tbAccountNumber.Text = (string)SelectedCustomer.ID;

                decimal balance = Providers.CustomerLedgerEntriesData.GetCustomerBalance(dlgEntry, SelectedCustomer.ID);
                DecimalLimit limiter = new DecimalLimit(DecimalExtensions.DigitsBeforeFirstSignificantDigit(storeCurrency.RoundOffSales));
                numCurrentBalance.SetValueWithLimit(balance, limiter);

                ntbAmount.Focus();
            }
        }

        /// <summary>
        /// Clears SelectedCustomer field and the customer details from UI.
        /// </summary>
        private void ClearCustomer()
        {
            SelectedCustomer = null;
            tbCustomerName.Text = tbAccountNumber.Text = "";
            numCurrentBalance.Text = "0";
            Amount = 0M;
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
            
            Interfaces.Services.TransactionService(dlgEntry).AddCustomerToTransaction(dlgEntry, dlgSession, Transaction, SelectedCustomer.ID, false);
        }
    }
}
