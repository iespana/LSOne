using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Controls.Dialogs
{
    public partial class ChangeBackDialog : TouchBaseForm
    {
        private bool drawerOpened;
        private PosTransaction transaction;

        public string PaymentName { private get; set; }
        public decimal Amount { private get; set; }
        public decimal AmountTendered { private get; set; }
        public decimal Changeback { private get; set; }
        public decimal RoundingDifference { private get; set; }
        public bool UsePaymentInMoneyBack { private get; set; }
        public bool OpenDrawer { private get; set; }

        public ChangeBackDialog()
        {
            InitializeComponent();

            UsePaymentInMoneyBack = false;
            if (!DesignMode)
            {
                CashDrawer.CashDrawerMessageEventX += CashDrawerOnCashDrawerMessageEventX;
            }
        }

        public ChangeBackDialog(PosTransaction transaction)
            : this()
        {
            this.transaction = transaction;
        }

        public new void Dispose()
        {
            base.Dispose();
            drawerTimer.Tick -= timerTick;
        }


        private void CashDrawerOnCashDrawerMessageEventX(string message)
        {
            if (!CashDrawer.DrawerOpen())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void timerTick(object sender, EventArgs args)
        {
            // We start by stopping the timer, making sure only one instance pr time is running
            drawerTimer.Stop();
            try
            {
                // if the drawer has not been opened ...
                if (!drawerOpened)
                {
                    CashDrawer.OpenDrawer();
                    drawerOpened = true;
                    if (transaction != null)
                        transaction.OpenDrawer = true;
                }
                else
                {
                    // If the drawer has been opened, and it can report it's state...
                    if (CashDrawer.CapStatus())
                    {
                        // then we take advantage of that functionality and ask if the drawer has opened
                        if (!CashDrawer.DrawerOpen())
                        {
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                    }
                }
            }
            finally
            {
                drawerTimer.Start();
            }
        }

        private void ChangeBackDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            CashDrawer.CashDrawerMessageEventX -= CashDrawerOnCashDrawerMessageEventX;
        }

        private void ChangeBackDialog_Load(object sender, EventArgs e)
        {
            IRoundingService rounding = (IRoundingService)DLLEntry.DataModel.Service(ServiceType.RoundingService);

            string currencyCode = DLLEntry.Settings.Store.Currency;

            if ((transaction.LastRunOperation == DataLayer.BusinessObjects.Enums.POSOperations.PayCurrency) &&
                (((TenderLineItem)((RetailTransaction)transaction).TenderLines.LastOrDefault()).CurrencyCode != DLLEntry.Settings.Store.Currency))
            {
                currencyCode = ((TenderLineItem)((RetailTransaction)transaction).TenderLines.LastOrDefault()).CurrencyCode;
            }

            touchDialogBanner1.BannerText = PaymentName;
            lblSale.Text = rounding.RoundForDisplay(
                DLLEntry.DataModel,
                Amount,
                true,
                true,
                currencyCode,
                CacheType.CacheTypeTransactionLifeTime);

            lblPaid.Text = rounding.RoundForDisplay(
                DLLEntry.DataModel,
                AmountTendered,
                true,
                true,
                currencyCode,
                CacheType.CacheTypeTransactionLifeTime);

            lblMoneyBackValue.Text = rounding.RoundForDisplay(
                DLLEntry.DataModel,
                Math.Abs(Changeback),
                true,
                true,
                currencyCode,
                CacheType.CacheTypeTransactionLifeTime);

            if (UsePaymentInMoneyBack)
            {
                lblMoneyBack.Text = PaymentName + " " + Properties.Resources.Back;
            }

            lblRounded.Visible = lblRounded.Visible = RoundingDifference != 0M;
            if (lblRounded.Visible)
            {
                lblRounded.Text = rounding.RoundForDisplay(
                    DLLEntry.DataModel,
                    RoundingDifference,
                    true,
                    true,
                    currencyCode,
                    CacheType.CacheTypeTransactionLifeTime);
            }
            else
            {
                Height -= 54;
            }

            if (OpenDrawer)
            {
                drawerTimer.Start();
            }
            else
            {
                drawerTimer.Stop();
            }
        }
    }
}
