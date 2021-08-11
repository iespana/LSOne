using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.DataLayer.BusinessObjects.Ledger;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ErrorHandling;
using System.Xml.Linq;
using LSOne.Controls.SupportClasses;
using LSOne.POS.Processes.WinControls;
using Customer = LSOne.DataLayer.BusinessObjects.Customers.Customer;

namespace LSOne.Services.Panels
{
    public partial class CustomerLedgerPanel : UserControl
    {
        private IConnectionManager entry;
        private ISettings settings;
        private IRoundingService roundingService;

        private Customer customer;
        private Parameters paramsData;
        private SiteServiceProfile siteServiceProfile;
        private DecimalLimit priceLimit;
        private Receipt receipt;

        private List<CustomerLedgerEntries> custLedgerLs;

        private Dictionary<string, string> stores;
        private Dictionary<string, string> terminals;

        public CustomerLedgerPanel(IConnectionManager entry, Customer customer)
        {
            if (!DesignMode)
            {
                InitializeComponent();

                this.entry = entry;
                this.customer = customer;
                settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                roundingService = Interfaces.Services.RoundingService(entry);
                paramsData = Providers.ParameterData.Get(entry);
                siteServiceProfile = settings.SiteServiceProfile;
                priceLimit = entry.GetDecimalSetting(DecimalSettingEnum.Prices);
                stores = new Dictionary<string, string>();
                terminals = new Dictionary<string, string>();
                receipt = new ReceiptControlFactory().CreateReceiptControl(pnlReceipt);
                lvLedger.HorizontalScrollbar = true;

                InitializeLabels();
                LoadData(new CustomerLedgerFilter());
            }
        }

        public bool UseCentralCustomer
        {
            get
            {
                return siteServiceProfile == null || siteServiceProfile.CheckCustomer;
            }
        }

        public void LoadData(CustomerLedgerFilter filter)
        {
            try
            {
                decimal maxCredit = 0;

                Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.LoadingCustomerLedger);

                if (UseCentralCustomer)
                {
                    try
                    {
                        ISiteServiceService srv = (ISiteServiceService) entry.Service(ServiceType.SiteServiceService);
                        Customer serverCustomer = srv.GetCustomer(entry, siteServiceProfile, customer.ID, UseCentralCustomer, true);
                        maxCredit = serverCustomer.MaxCredit;
                    }
                    catch (Exception exception)
                    {
                        Interfaces.Services.DialogService(entry).CloseStatusDialog();
                        Interfaces.Services.DialogService(entry).ShowMessage(exception is ClientTimeNotSynchronizedException ? exception.Message : Properties.Resources.CouldNotConnectToSiteService, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return;
                    }
                }
                else
                {
                    maxCredit = customer.MaxCredit;
                }

                lblCreditLimit.Text = roundingService.RoundForDisplay(entry, maxCredit, true, true, customer.Currency);

                decimal custBalance = 0;
                decimal custTotalSales = 0;
                int totalRecords = 0;

                ISiteServiceService service = null;

                try
                {
                    service = (ISiteServiceService) entry.Service(ServiceType.SiteServiceService);
                    custBalance = service.GetCustomerBalance(entry, siteServiceProfile, customer.ID, UseCentralCustomer, false);
                    custTotalSales = service.GetCustomerTotalSales(entry, siteServiceProfile, customer.ID, UseCentralCustomer, false);
                    custLedgerLs = service.GetCustomerLedgerEntriesList(entry, siteServiceProfile, customer.ID, out totalRecords, UseCentralCustomer, filter);
                    service.Disconnect(entry);
                }
                catch (Exception exception)
                {
                    Interfaces.Services.DialogService(entry).CloseStatusDialog();
                    Interfaces.Services.DialogService(entry).ShowMessage(exception is ClientTimeNotSynchronizedException ? exception.Message : Properties.Resources.CouldNotConnectToSiteService, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return;
                }

                lblBalance.Text = roundingService.RoundForDisplay(entry, custBalance, true, true, customer.Currency);
                lblTotal.Text = roundingService.RoundForDisplay(entry, custTotalSales*-1, true, true, customer.Currency);

                lvLedger.ClearRows();

                foreach (CustomerLedgerEntries ledgerEntry in custLedgerLs)
                {
                    Row row = new Row();
                    row.AddText(ledgerEntry.PostingDate.ToString("d", settings.CultureInfo));
                    row.AddText(CustomerLedgerEntries.AsString(ledgerEntry.EntryType));
                    row.AddText(ledgerEntry.Amount.FormatWithLimits(priceLimit, true));
                    row.AddText(ledgerEntry.RemainingAmount.FormatWithLimits(priceLimit, true));
                    row.AddText(CustomerLedgerEntries.AsString(ledgerEntry.Status));

                    if (stores.ContainsKey((string) ledgerEntry.StoreId))
                    {
                        row.AddText(stores[(string) ledgerEntry.StoreId]);
                    }
                    else
                    {
                        ledgerEntry.StoreId = ledgerEntry.StoreId == "" ? RecordIdentifier.Empty : ledgerEntry.StoreId;

                        DataEntity storeDescription = Providers.StoreData.GetStoreEntity(entry, ledgerEntry.StoreId);
                        storeDescription = storeDescription ?? new DataEntity(RecordIdentifier.Empty, Properties.Resources.HeadOffice);

                        stores.Add((string) ledgerEntry.StoreId, storeDescription.Text);

                        row.AddText(storeDescription.Text);
                    }

                    row.AddText(ledgerEntry.DocumentNo.StringValue);
                    row.Tag = ledgerEntry;
                    lvLedger.AddRow(row);
                }

                lvLedger.AutoSizeColumns();
                lvLedger_SelectionChanged(this, EventArgs.Empty);
            }
            finally
            {
                Interfaces.Services.DialogService(entry).CloseStatusDialog();
            }
        }

        public void InitializeLabels()
        {
            lblCreditLimit.Text = "-";
            lblBalance.Text = "-";
            lblTotal.Text = "-";
        }

        private void lvLedger_SelectionChanged(object sender, EventArgs e)
        {
            if (lvLedger.Selection.Count == 1)
            {
                CustomerLedgerEntries selectedEntry = (CustomerLedgerEntries)lvLedger.Selection[0].Tag;

                PosTransaction transaction = LoadTransaction(selectedEntry);
                receipt.ShowTransaction(transaction);
            }
            else
            {
                receipt.ShowTransaction(null);
            }
        }

        private PosTransaction LoadTransaction(CustomerLedgerEntries ledgerEntry)
        {
            if (ledgerEntry.TransactionId.StringValue == "")
                return null;

            try
            {
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                XElement transactionXML = null;
                transactionXML = service.GetCustomerTransactionXML(entry, siteServiceProfile, ledgerEntry.TransactionId, ledgerEntry.StoreId, ledgerEntry.TerminalId, UseCentralCustomer);

                if (transactionXML == null)
                {
                    return null;
                }

                TypeOfTransaction transactionType = (TypeOfTransaction)(Convert.ToInt32(transactionXML.Element("PosTransaction").Attribute("TransactionType").Value));
                PosTransaction transToReturn;

                switch (transactionType)
                {
                    case TypeOfTransaction.Deposit:
                        transToReturn = new DepositTransaction("", "", true);
                        break;
                    case TypeOfTransaction.Payment:
                        transToReturn = new CustomerPaymentTransaction("");
                        break;
                    default:
                        transToReturn = new RetailTransaction("", "", true);
                        break;
                }

                transToReturn.ToClass(transactionXML);
                return transToReturn;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                Interfaces.Services.DialogService(entry).ShowMessage(x is ClientTimeNotSynchronizedException ? x.Message : Properties.Resources.CouldNotConnectToSiteService, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return null;
            }
        }

        private void panelLedgerValues_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, panelLedgerValues.Width - 1, panelLedgerValues.Height - 1);
            p.Dispose();
        }
    }
}
