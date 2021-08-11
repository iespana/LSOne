using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TaxFree;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{


    public partial class TaxFreeService : ITaxFreeService
    {
        #region IService members

        public IErrorLog ErrorLog { set; private get; }

        public void Init(IConnectionManager entry)
        {
            var settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

#pragma warning disable 0612, 0618
            DLLEntry.Init(entry, settings);
#pragma warning restore 0612, 0618

            SetLogo();
        }

        #endregion

        #region ITaxFree Members

        public void TaxRefundMulti(IConnectionManager entry, IPosTransaction transaction)
        {
            DialogResult result = DialogResult.Cancel;
            List<Transaction> selectedTransactions;
            using (var dialog = new TaxRefundTransactionsDialog(entry, transaction))
            {
                result = dialog.ShowDialog();
                selectedTransactions = dialog.SelectedTransaction;
            }

            if (result == DialogResult.OK && selectedTransactions != null)
            {
                using (RefundDialog dialog = new RefundDialog(entry, selectedTransactions))
                {
                    dialog.ShowDialog();
                }
            }
        }

        public bool ShowInJournal
        {
            get { return true; }
        }

        public void CaptureSale(IConnectionManager entry, IPosTransaction transaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            TaxFreeConfig config = DataProviderFactory.Instance.Get<ITaxFreeConfigData, TaxFreeConfig>().Get(entry);

            if (config.PrintoutType == TaxFreePrintoutEnum.Report)
            {
                Transaction trans = Providers.TransactionData.GetRetailTransHeader(entry, transaction.TransactionId, transaction.StoreId, transaction.TerminalId);
                AskCustomerForInformation(entry, trans, config);
            }
            else
            {
                RetailTransaction taxFreeTransaction = new RetailTransaction(transaction.StoreId, settings.Store.Currency, settings.TaxIncludedInPrice);
                taxFreeTransaction.TerminalId = transaction.TerminalId;
                TransactionProviders.PosTransactionData.GetTransaction(entry,
                                                             transaction.TransactionId,
                                                             transaction.StoreId,
                                                             transaction.TerminalId,
                                                             taxFreeTransaction,
                                                             settings.TaxIncludedInPrice);
                PrintOPOSReceipt(entry, taxFreeTransaction, config);
            }
            
        }

        private void PrintOPOSReceipt(IConnectionManager entry, RetailTransaction retailTransaction, TaxFreeConfig config)
        {
            if (config.PrintoutType == TaxFreePrintoutEnum.OPOS)
            {
                PrintTaxFreeReport(entry, retailTransaction, config);
            }
        }

        private void AskCustomerForInformation(IConnectionManager entry, Transaction trans, TaxFreeConfig config)
        {
            if (config.PromptCustomerForInformation)
            {
                using (RefundDialog dialog = new RefundDialog(entry, new List<Transaction>() { trans }))
                {
                    dialog.ShowDialog();
                }
            }
        }

        public bool CancelTaxRefund(IConnectionManager entry, RecordIdentifier refundID)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            ISiteServiceService service = (ISiteServiceService) entry.Service(ServiceType.SiteServiceService);
            TaxRefund refund = service.GetTaxRefund(entry, settings.SiteServiceProfile, refundID);
            IDialogService dialogService = Interfaces.Services.DialogService(entry);
            if (refund == null)
            {
                dialogService.ShowMessage(Resources.TaxRefundLoadError);
            }
            if (refund.Status != TaxRefundStatus.Canceled)
            {
                dialogService.ShowMessage(Resources.AskForForm);
                DialogResult result = dialogService.ShowMessage(Resources.DoYouWantToCancelTaxRefund, MessageBoxButtons.YesNo, MessageDialogType.Attention);
                if (result == DialogResult.No)
                {
                    return false;
                }
                string comment = "";
                dialogService.KeyboardInput(ref comment, Resources.EnterAComment, Resources.Comment, 32767, InputTypeEnum.Normal);
                refund.Comment = comment;
                refund.Status = TaxRefundStatus.Canceled;
                service.SaveTaxRefund(entry, settings.SiteServiceProfile, refund);
                Providers.TaxRefundData.Save(entry, refund);

                return true;
            }
            return true;
        }

        #endregion
    }

}
