using System;
using System.ServiceModel;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.CreditMemoItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.WinFormsTouch;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSRetailPosis;

namespace LSOne.Services
{
    public partial class CreditMemoService : ICreditMemoService
    {

        #region ICreditMemo Membersge

        /// <summary>
        /// Issues the credit memo on the HO through the Store Server.         
        /// </summary>
        /// <param name="creditMemoItem">The tender line containing the information to be created on the credit memo</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="entry">The entry into the database</param>
        public virtual void IssueCreditMemo(IConnectionManager entry, ICreditMemoTenderLineItem creditMemoItem, IPosTransaction retailTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!CreditVouchersInUse(settings.SiteServiceProfile))
            {
                LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CreditMemoDisabled, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return;
            }

            ICalculationService calculationService = Interfaces.Services.CalculationService(entry);

            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Issuing a credit memo....", "CreditMemo.IssueCreditMemo");

                bool retVal = false;
                string comment = "";                

                try
                {
                    ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                    service.StaffID = (string)settings.POSUser.ID;
                    service.TerminalID = (string)entry.CurrentTerminalID;

                    CreditVoucher creditVoucher = new CreditVoucher();
                    creditVoucher.Balance = creditMemoItem.Amount * -1;
                    creditVoucher.Currency = retailTransaction.StoreCurrencyCode;
                    creditVoucher.Text = ((CreditMemoTenderLineItem)creditMemoItem).Description;
                    creditVoucher.CreatedDate = Date.Now;

                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.IssuingCreditMemo);

                    RecordIdentifier voucherID = service.IssueCreditVoucher(entry,
                                                                            settings.SiteServiceProfile,
                                                                            creditVoucher,
                                                                            retailTransaction.TransactionId,
                                                                            retailTransaction.ReceiptId,
                                                                            true);
                    if (retailTransaction is RetailTransaction)
                    {
                        ((RetailTransaction)retailTransaction).CreditMemoItem.CreditMemoNumber = (string)voucherID;
                        ((RetailTransaction)retailTransaction).CreditMemoItem.Amount = creditMemoItem.Amount * -1;
                    }
                    else if (retailTransaction is CustomerPaymentTransaction)
                    {
                        ((CustomerPaymentTransaction)retailTransaction).CreditMemoItem.CreditMemoNumber = (string)voucherID;
                        ((CustomerPaymentTransaction)retailTransaction).CreditMemoItem.Amount = creditMemoItem.Amount * -1;
                    }

                    creditMemoItem.SerialNumber = (string)voucherID;
                    creditMemoItem.Comment = (string)voucherID;

                    retVal = true;
                }
                catch (Exception x)
                {
                    // We cannot publish the credit memo to the HO, so we need to take action...
                    if (((RetailTransaction)retailTransaction).TenderLines.Count > 0)
                    {
                        ((RetailTransaction)retailTransaction).TenderLines.RemoveAt(((RetailTransaction)retailTransaction).TenderLines.Count - 1);
                    }

                    calculationService.CalculateTotals(entry, (RetailTransaction)retailTransaction);

                    ((RetailTransaction)retailTransaction).CreditMemoItem = new CreditMemoItem(((RetailTransaction)retailTransaction));

                    entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                    if (x is EndpointNotFoundException)
                    {                        
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CouldNotConnectToSiteService, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                    else if (x is ClientTimeNotSynchronizedException)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(x.Message, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                    else
                    {
                        throw new Exception("52300", x);
                    }
                }
                finally
                {
                    LSOne.Services.Interfaces.Services.DialogService(entry).CloseStatusDialog();
                }

                if (!retVal)
                {
                    // We cannot publich the credit memo to the HO, so we need to take action...
                    entry.ErrorLogger.LogMessage(LogMessageType.Error, "Error storing the credit memo centrally..." + comment, "CreditMemo.IssueCreditMemo");

                    // Remove the credit memo from the transaction and recalculate the transaction
                    if (((RetailTransaction)retailTransaction).TenderLines.Count > 0)
                    {
                        ((RetailTransaction)retailTransaction).TenderLines.RemoveAt(((RetailTransaction)retailTransaction).TenderLines.Count - 1);
                    }
                    calculationService.CalculateTotals(entry, ((RetailTransaction)retailTransaction));                    

                    throw new Exception(Properties.Resources.ErrorIssuingCreditMemo);
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        /// <summary>
        /// Displays a dialog to authorize a credit voucher, and adds the total amount of that credit voucher to the transaction
        /// </summary>
        public virtual void AuthorizeCreditMemoPayment(IConnectionManager entry, ref bool valid, ref string comment, ref string creditMemoId, ref decimal amount, IPosTransaction posTransaction, StorePaymentMethod tenderInfo, decimal amountDue)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            if (!CreditVouchersInUse(settings.SiteServiceProfile))
            {
                LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CreditMemoDisabled, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                valid = false;
                return;
            }

            try
            {
                using (PayCreditMemoDialog dialog = new PayCreditMemoDialog(entry, (PosTransaction)posTransaction, tenderInfo, amountDue, creditMemoId))
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    valid = dialog.CreditMemoValid;
                    creditMemoId = (string)dialog.CreditMemoID;
                    amount = dialog.CreditMemoBalance;
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        /// <summary>
        /// Marks the credit voucher as used so that it cannot be used again
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="creditMemoNumber">The ID of the credit voucher</param>
        /// <param name="amount">The amount to be used. This is typically the whole credit voucher balance</param>
        /// <param name="posTransaction">The current PosTransaction object</param>
        /// <param name="creditMemoTenderLineItem">The credit voucher line on the transaction</param>
        public virtual void UpdateCreditMemo(IConnectionManager entry, string creditMemoNumber, decimal amount, IPosTransaction posTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!CreditVouchersInUse(settings.SiteServiceProfile))
            {
                LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CreditMemoDisabled, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return;
            }

            try
            {
                bool creditVoucherValid = false;
                string comment = "";

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Marking a credit memo as used....", "CreditMemo.UpdateCreditMemo");

                try
                {
                    ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                    InitStoreServerService(entry, service);

                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.UpdatingCreditMemo);
                    CreditVoucher creditVoucher = service.GetCreditVoucher(entry, settings.SiteServiceProfile, creditMemoNumber, false);
                    if (creditVoucher.Currency != settings.Store.Currency)
                    {
                        string message = Properties.Resources.CreditMemoCurrencyAndStoreCurrencyNotSame;
                        message = message.Replace("#1", settings.Store.Currency);
                        message = message.Replace("#2", (string)creditVoucher.Currency);

                        LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(message, Properties.Resources.InconsistentCurrencies, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return;
                    }
                    CreditVoucherValidationEnum result = service.UseCreditVoucher(entry,
                                                                                    settings.SiteServiceProfile,
                                                                                    ref amount,
                                                                                    creditMemoNumber,
                                                                                    posTransaction.TransactionId,
                                                                                    posTransaction.ReceiptId,
                                                                                    true);
                    LSOne.Services.Interfaces.Services.DialogService(entry).CloseStatusDialog();


                    if (result != CreditVoucherValidationEnum.ValidationSuccess)
                    {
                        HandleCreditVoucherValidationEnum(entry, result);
                    }
                    else
                    {
                        creditVoucherValid = true;
                    }
                }
                catch
                {
                    creditVoucherValid = false;
                }

                if (!creditVoucherValid)
                {
                    // Stop the transaction from completing
                    entry.ErrorLogger.LogMessage(LogMessageType.Error, "Error updating the credit memo centrally, as used..." + comment, "CreditMemo.UpdateCreditMemo");

                    throw new Exception(Properties.Resources.UpdateCreditMemoError);                    
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                LSOne.Services.Interfaces.Services.DialogService(entry).CloseStatusDialog();
            }
        }

        /// <summary>
        /// Voids a credit memo payment. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="voided">If this value is set to true, the POS will void the payment line</param>
        /// <param name="comment"></param>
        /// <param name="creditMemoNumber">The ID of the credit voucher to void the payment for</param>
        /// <param name="retailTransaction">The current POS transaction</param>
        public virtual void VoidCreditMemoPayment(IConnectionManager entry, ref bool voided, ref string comment, string creditMemoNumber, IRetailTransaction retailTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!CreditVouchersInUse(settings.SiteServiceProfile))
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CreditMemoDisabled, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                
                return;
            }

            try
            {
                voided = true;
                
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Cancelling the used marking of the credit memo...", "CreditMemo.VoidCreditMemoPayment");
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        /// <summary>
        /// Checks if a given credit voucher is valid, and returns the correct balance if needed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="validated">True if the voucher is valid</param>
        /// <param name="comment"></param>
        /// <param name="amount">The amount to check for. If the voucher is invalid the function returns a 0.0 otherwise this field will contain the true balance of the credit voucher</param>
        /// <param name="creditMemoNumber"></param>
        /// <param name="retailTransaction"></param>
        public virtual void ValidateCreditMemo(IConnectionManager entry, ref bool validated, ref string comment, ref decimal amount, string creditMemoNumber, IRetailTransaction retailTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!CreditVouchersInUse(settings.SiteServiceProfile))
            {
                LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CreditMemoDisabled, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return;
            }

            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Validating a credit memo...", "CreditMemo.ValidateCreditMemo");

                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                InitStoreServerService(entry,service);

                LSOne.Services.Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.ValidatingCreditMemo);
                CreditVoucherValidationEnum result = service.ValidateCreditVoucher(entry, settings.SiteServiceProfile,  ref amount, creditMemoNumber, true);
                LSOne.Services.Interfaces.Services.DialogService(entry).CloseStatusDialog();

                if (result != CreditVoucherValidationEnum.ValidationSuccess)
                {
                    HandleCreditVoucherValidationEnum(entry, result);
                    validated = false;
                }
                else
                {
                    validated = true;
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }


        /// <summary>
        /// Returns the balance of a given credit voucher. This function checks with the HO through the Store Server and retrieves the balance amount.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="creditMemoNumber">The ID of the credit voucher</param>
        /// <param name="balance">The balance of the credit voucher</param>
        /// <param name="siteServiceProfile">The site service profile that should be used for connection</param>
        public virtual void GetCreditmemoBalance(IConnectionManager entry, string creditMemoNumber, ref decimal balance)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!CreditVouchersInUse(settings.SiteServiceProfile))
            {
                LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CreditMemoDisabled, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return;
            }

            try
            {
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                InitStoreServerService(entry, service);
                
                LSOne.Services.Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.ValidatingCreditMemo);

                CreditVoucherValidationEnum result = service.ValidateCreditVoucher(entry, settings.SiteServiceProfile, ref balance, creditMemoNumber, true);

                LSOne.Services.Interfaces.Services.DialogService(entry).CloseStatusDialog();

                if (result != CreditVoucherValidationEnum.ValidationSuccess)
                {
                    HandleCreditVoucherValidationEnum(entry, result);
                }
            }
            catch (Exception x)
            {
                LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ErrorGettingBalance + "\r\n" + x.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }

        }

        public virtual bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction)
        {
            ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            return CreditVouchersInUse(settings.SiteServiceProfile);
        }

        #endregion

        protected virtual bool CreditVouchersInUse(SiteServiceProfile siteServiceProfile)
        {
            return siteServiceProfile != null && siteServiceProfile.UseCreditVouchers;
        }

        /// <summary>
        /// Initializes fields in an ISiteServiceService instance
        /// </summary>
        /// <param name="service">The ISiteServiceService object to initialize</param>
        /// <param name="entry">The entry into the database</param>
        protected virtual void InitStoreServerService(IConnectionManager entry, ISiteServiceService service)
        {
            ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            service.StaffID = (string)settings.POSUser.ID;
            service.TerminalID = (string)entry.CurrentTerminalID;
        }

        /// <summary>
        /// Handles results other than ValidationSuccess and displays the appropriate message if that applies
        /// </summary>
        /// <param name="cvEnum"></param>
        /// <param name="entry">The entry into the database</param>
        public virtual void HandleCreditVoucherValidationEnum(IConnectionManager entry, CreditVoucherValidationEnum cvEnum)
        {
            switch (cvEnum)
            {
                case CreditVoucherValidationEnum.ValidationBalanceToLow:
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CreditMemoBalanceToLow, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;

                case CreditVoucherValidationEnum.ValidationUnknownError:
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CreditMemoUnknownError, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;

                case CreditVoucherValidationEnum.ValidationVoucherNotFound:
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CreditMemoNotFound, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;

                case CreditVoucherValidationEnum.ValidationVoucherHasZeroBalance:
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CreditMemoHasZeroBalance, MessageBoxButtons.OK, MessageDialogType.Generic);
                    break;
            }
        }

        public virtual IErrorLog ErrorLog
        {
            set 
            { 
                
            }
        }

        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618
        }
    }
}