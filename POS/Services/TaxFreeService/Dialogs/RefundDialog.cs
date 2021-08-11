using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TaxFree;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.DialogPanels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Dialogs
{
    public partial class RefundDialog : TouchBaseForm
    {
        private LinkedList<DialogPageBase> pages;
        private LinkedListNode<DialogPageBase> currentPage;
        private List<Transaction> transactions;

        private PassportPanel passportPanel;
        private TouristInformationPanel touristInformationPanel;
        private FlightInformationPanel flightInformationPanel;
        private ReportPanel reportPanel;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        public Tourist Tourist { get; private set; }

        public RefundDialog(IConnectionManager entry)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            Tourist = new Tourist();

            pages = new LinkedList<DialogPageBase>();

            passportPanel = new PassportPanel(entry, this);
            InitializePanel(passportPanel);
            pages.AddLast(passportPanel);

            touristInformationPanel = new TouristInformationPanel(entry);
            InitializePanel(touristInformationPanel);
            pages.AddLast(touristInformationPanel);

            flightInformationPanel = new FlightInformationPanel();
            InitializePanel(flightInformationPanel);
            pages.AddLast(flightInformationPanel);

            reportPanel = new ReportPanel(entry); 
            InitializePanel(reportPanel);
            pages.AddLast(reportPanel);

            currentPage = pages.First;
            SetButtons(currentPage.Value);
        }

        public RefundDialog(IConnectionManager entry, List<Transaction> transactions)
            : this(entry)
        {
            this.transactions = transactions;
        }

        private void SetButtons(DialogPageBase page)
        {
            btnBack.Enabled = page.BackEnabled;
            btnNext.Enabled = page.NextEnabled;
            btnNext.Text = page.FinishEnabled ? Properties.Resources.Finish : Properties.Resources.Next;
        }

        private void InitializePanel(Control panel)
        {
            panel.Location = new Point(1,1);
            panel.TabIndex = 0;
            Controls.Add(panel);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            currentPage.Value.Visible = false;
            if (currentPage.Previous != null)
            {
                currentPage = currentPage.Previous;
                currentPage.Value.Visible = true;
                SetButtons(currentPage.Value);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(currentPage.Value.FinishEnabled)
            {
                Finish();
                return;
            }

            if (!ValidateData())
                return;

            UpdateData();
            currentPage.Value.Visible = false;
            if (currentPage.Next != null)
            {
                currentPage = currentPage.Next;
                currentPage.Value.Visible = true;
                SetButtons(currentPage.Value);
            }
        }

        private void Finish()
        {
            if (!ValidateData())
                return;

            UpdateData();

            TaxRefund refund = new TaxRefund();
            refund.Created = DateTime.Now;
            reportPanel.GetData(refund);

            List<TaxRefundTransaction> refundTransactions = null;
            decimal totalWithTax = 0M;
            decimal totalTax = 0M;
            TaxRefundRange range = GetRangeForTransactions(dlgEntry, transactions, ref refundTransactions, ref totalWithTax, ref totalTax);
            
            if (range.TaxRefundPercentage == 0M)
            {
                refund.TaxRefundValue = range.TaxRefund;
            }
            else
            {
                refund.TaxRefundValue = totalWithTax * (range.TaxRefundPercentage / 100);
            }
            refund.Total = totalWithTax;
            refund.Tax = totalTax;
            refund.Transactions = refundTransactions;

            IConnectionManagerTransaction tx = dlgEntry.CreateTransaction();
            try
            {
                DataProviderFactory.Instance.Get<ITouristData, Tourist>().Save(tx, Tourist);

                refund.TouristID = Tourist.ID;

                ISiteServiceService storeServer =(ISiteServiceService) dlgEntry.Service(ServiceType.SiteServiceService);
                storeServer.SaveTaxRefund(tx, dlgSettings.SiteServiceProfile, refund);

                tx.Commit();

                // TODO: Print
                using (PrintRefundApplication print = new PrintRefundApplication(Tourist, transactions, refund))
                {
                    print.Print(dlgEntry);
                }

                Close();
            }
            catch(Exception exception)
            {
                tx.Rollback();
                IDialogService service = (IDialogService)dlgEntry.Service(ServiceType.DialogService);
                service.ShowExceptionMessage(exception);
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

        private bool ValidateData()
        {
            return currentPage.Value.ValidateData();
        }

        internal void UpdateTourist(Tourist tourist)
        {
            Tourist = tourist;
            passportPanel.SetData(tourist);
            touristInformationPanel.SetData(tourist);
        }

        private void UpdateData()
        {
            currentPage.Value.GetData(Tourist);
        }

        public static TaxRefundRange GetRangeForTransactions(IConnectionManager entry, List<Transaction> transactions, ref List<TaxRefundTransaction> refundTransactions, ref decimal totalWithTax, ref decimal totalTax)
        {
            refundTransactions = refundTransactions ?? new List<TaxRefundTransaction>();
            totalWithTax = 0;
            totalTax = 0;
            foreach (Transaction transaction in transactions)
            {
                TaxRefundTransaction refundTransaction = new TaxRefundTransaction();
                refundTransaction.StoreID = string.IsNullOrEmpty((string)transaction.StoreID) ? entry.CurrentStoreID : transaction.StoreID;
                refundTransaction.TerminalID = string.IsNullOrEmpty((string)transaction.TerminalID) ? entry.CurrentTerminalID : transaction.TerminalID;
                refundTransaction.TransactionID = transaction.ID;
                refundTransaction.Total = transaction.NetAmount * -1;
                refundTransaction.TotalTax = transaction.NetAmountWithTax * -1;
                refundTransactions.Add(refundTransaction);
                totalWithTax += refundTransaction.TotalTax;
                totalTax += refundTransaction.TotalTax - refundTransaction.Total;
            }
            return DataProviderFactory.Instance.Get<ITaxRefundRangeData, TaxRefundRange>().GetForValue(entry, totalWithTax);
        }
    }
}
