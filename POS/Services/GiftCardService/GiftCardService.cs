using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.WinFormsTouch;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class GiftCardService : IGiftCardService
    {
        #region IGiftCard Members

        public virtual bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            return settings.SiteServiceProfile != null && settings.SiteServiceProfile.UseGiftCards;
        }

        public virtual ITenderLineItem GiftCardChangeBack(IConnectionManager entry, IRetailTransaction retailTransaction, string giftCardTenderId, decimal amount)
        {
            try
            {
                StorePaymentMethod tenderInfo = Providers.StorePaymentMethodData.Get(entry, new RecordIdentifier(entry.CurrentStoreID, giftCardTenderId), CacheType.CacheTypeApplicationLifeTime);
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                string giftCardID = string.Empty;

                if (settings.SiteServiceProfile.IssueGiftCardOption != SiteServiceProfile.IssueGiftCardOptionEnum.Amount)
                {
                    //Display the issue gift card dialog as the user needs to enter either the ID or both ID and amount
                    using (var issueGiftCardDialog = new IssueGiftCardDialog(entry,tenderInfo, false))
                    {
                        issueGiftCardDialog.IsChangeBack = true;
                        issueGiftCardDialog.GiftCardOptions = SiteServiceProfile.IssueGiftCardOptionEnum.ID;
                        issueGiftCardDialog.ShowDialog();
                        giftCardID = (string) issueGiftCardDialog.GiftCardId;

                    }
                }

                ISiteServiceService service = (ISiteServiceService) entry.Service(ServiceType.SiteServiceService);

                service.StaffID = settings.POSUser.ID;
                service.TerminalID = entry.CurrentTerminalID;


                try
                {
                    //Check if the gift card already exists
                    GiftCard existingGiftCard = service.GetGiftCard(entry, settings.SiteServiceProfile, giftCardID, false);
                    if (existingGiftCard != null)
                    {
                        //If the gift card is refillable then add the amount to the gift card
                        if (existingGiftCard.Refillable)
                        {
                            if (!existingGiftCard.Active)
                            {
                                //Activate the gift card if necessary
                                service.ActivateGiftCard(entry, settings.SiteServiceProfile, giftCardID, retailTransaction.TransactionId, retailTransaction.ReceiptId, false);
                            }
                            service.AddToGiftCard(entry, settings.SiteServiceProfile, giftCardID, amount, true);
                            
                        }
                        else
                        {
                            //An existing giftcard ID that is not refillable cannot be used to give change back to customer
                            IDialogService dialogService = (IDialogService) entry.Service(ServiceType.DialogService);
                            dialogService.ShowMessage(Properties.Resources.GiftCardCannotBeUsedAsChangebackCustomerWillNeedToBeIssuedANewGiftCardOrOtherTender, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        }
                    }
                    else
                    {
                        //Create a new gift card for the change back
                        GiftCard newGiftCard = new GiftCard
                        {
                            Active = true,
                            Balance = amount,
                            Currency = retailTransaction.StoreCurrencyCode
                        };

                        //Set the gift card ID if it was selected. The transaction ID should always be set regardless of weather the ID is created
                        //in the Site service or not
                        if (giftCardID != string.Empty)
                        {
                            newGiftCard.ID = giftCardID;
                            newGiftCard.ID.SecondaryID = retailTransaction.TransactionId;
                        }
                        else
                        {
                            newGiftCard.ID = RecordIdentifier.Empty;
                            newGiftCard.ID.SecondaryID = retailTransaction.TransactionId;
                        }
                        newGiftCard.Refillable = settings.SiteServiceProfile.GiftCardRefillSetting == SiteServiceProfile.GiftCardRefillSettingEnum.AlwaysYes;
                        giftCardID =  service.AddNewGiftCard(entry, settings.SiteServiceProfile, newGiftCard, true,"").ToString();
                    }
                }
                catch (Exception e)
                {
                    IDialogService dialogService = (IDialogService) entry.Service(ServiceType.DialogService);
                    dialogService.ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Properties.Resources.CouldNotConnectToStoreServer, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                    
                }

                //Create the gift card tender line and return it 
                GiftCertificateTenderLineItem tenderLine = new GiftCertificateTenderLineItem()
                {
                    TenderTypeId = (string)giftCardTenderId,
                    Description = tenderInfo.Text,
                    OpenDrawer = tenderInfo.OpenDrawer,
                    Amount = amount,
                    ChangeTenderID = (string)tenderInfo.ChangeTenderID,
                    MinimumChangeAmount = tenderInfo.MinimumChangeAmount,
                    AboveMinimumTenderId = (string)tenderInfo.AboveMinimumTenderID,
                    SerialNumber = giftCardID,
                    CurrencyCode = settings.Store.Currency,
                    ExchangeRate = 1M
                };
                //convert from the store-currency to the company-currency...
                tenderLine.CompanyCurrencyAmount = Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                    entry,
                    settings.Store.Currency,
                    settings.CompanyInfo.CurrencyCode,
                    settings.CompanyInfo.CurrencyCode,
                    amount);
                // the exchange rate between the store amount(not the paid amount) and the company currency
                tenderLine.ExchrateMST = Interfaces.Services.CurrencyService(entry).ExchangeRate(entry,settings.Store.Currency) * 100;

                return tenderLine;
                
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                throw;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                Interfaces.Services.DialogService(entry).CloseStatusDialog();
            }
        }

        public virtual IGiftCertificateItem CreateGiftCardFromTenderLine(IConnectionManager entry, IPosTransaction transaction, IGiftCertificateTenderLineItem giftCertificateTenderLine)
        {
            if (giftCertificateTenderLine == null)
            {
                return null;
            }

            if (!(transaction is RetailTransaction))
            {
                return null;
            }

            GiftCertificateTenderLineItem tenderLine = giftCertificateTenderLine as GiftCertificateTenderLineItem;

            //Create the gift certificate item 
            GiftCertificateItem giftCard = new GiftCertificateItem((RetailTransaction)transaction)
            {
                SerialNumber = tenderLine.SerialNumber,
                StoreId = ((RetailTransaction)transaction).StoreId,
                TerminalId = ((RetailTransaction)transaction).TerminalId,
                StaffId = (string)((RetailTransaction)transaction).Cashier.ID,
                TransactionId = ((RetailTransaction)transaction).TransactionId,
                ReceiptId = ((RetailTransaction)transaction).ReceiptId,
                Amount = Math.Abs(tenderLine.Amount),
                Date = tenderLine.IssuedDate,
                Price = Math.Abs(tenderLine.Amount),
                PriceWithTax = Math.Abs(tenderLine.Amount),
                StandardRetailPrice = Math.Abs(tenderLine.Amount),
                Quantity = 1,
                TaxRatePct = 0,
                Description = Properties.Resources.GiftCard,
                Comment = tenderLine.SerialNumber,
                NoDiscountAllowed = true,
                Found = true
            };

            return giftCard;
        }

        public virtual void IssueGiftCard(IConnectionManager entry, IPosTransaction posTransaction, string giftCardTenderId, string prefix, int? numberSequenceLowest = null)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!settings.SiteServiceProfile.UseGiftCards)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.GiftCardsDisabled, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return;
            }

            try
            {
                StorePaymentMethod tenderInfo = Providers.StorePaymentMethodData.Get(entry, new RecordIdentifier(entry.CurrentStoreID, giftCardTenderId), CacheType.CacheTypeApplicationLifeTime);

                decimal amount = 0M;

                using (IssueGiftCardDialog issueGiftCardDialog = new IssueGiftCardDialog(entry,tenderInfo, true, posTransaction))
                {
                    issueGiftCardDialog.GiftCardOptions = settings.SiteServiceProfile.IssueGiftCardOption;

                    if (issueGiftCardDialog.ShowDialog() == DialogResult.OK)
                    {
                        string giftCardID = "";
                        bool retVal = false;

                        GiftCard newGiftCard;
                        ISiteServiceService service = (ISiteServiceService) entry.Service(ServiceType.SiteServiceService);
                        service.StaffID = settings.POSUser.ID;
                        service.TerminalID = entry.CurrentTerminalID;

                        // Check for the two options that create new gift cards
                        switch (settings.SiteServiceProfile.IssueGiftCardOption)
                        {
                                // Activate the gift card and retrieve the balance amount
                            case SiteServiceProfile.IssueGiftCardOptionEnum.ID:
                                giftCardID = (string) issueGiftCardDialog.GiftCardId;
                                GiftCard existingGiftCard;
                                try
                                {
                                    existingGiftCard = service.GetGiftCard(entry, settings.SiteServiceProfile, giftCardID, false);
                                }
                                catch (Exception e)
                                {
                                    Interfaces.Services.DialogService(entry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Properties.Resources.CouldNotConnectToStoreServer, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                                    return;
                                }

                                // Get the amount if the giftcard exists
                                retVal = existingGiftCard != null && !existingGiftCard.Active;
                                amount = retVal ? existingGiftCard.Balance : 0;

                                Interfaces.Services.DialogService(entry).CloseStatusDialog();
                                break;

                                // Create the gift card
                            case SiteServiceProfile.IssueGiftCardOptionEnum.Amount:
                                amount = issueGiftCardDialog.Amount;

                                // Create a new gift card with the entered amount
                                newGiftCard = new GiftCard
                                {
                                    Active = false,
                                    Balance = amount,
                                    Currency = posTransaction.StoreCurrencyCode,
                                    Refillable =
                                        settings.SiteServiceProfile.GiftCardRefillSetting ==
                                        SiteServiceProfile.GiftCardRefillSettingEnum.AlwaysYes,
                                    CreatedDate = Date.Now
                                };
                                // Use invoke to make sure that the storeServerDlg actually paints before the service functions are called
                                try
                                {
                                    giftCardID = service.AddNewGiftCard(entry, settings.SiteServiceProfile, newGiftCard, false, prefix, numberSequenceLowest).ToString();
                                }
                                catch (Exception e)
                                {
                                    Interfaces.Services.DialogService(entry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Properties.Resources.CouldNotConnectToStoreServer, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                                    return;
                                }
                                Interfaces.Services.DialogService(entry).CloseStatusDialog();

                                retVal = true;

                                break;

                            case SiteServiceProfile.IssueGiftCardOptionEnum.IDAndAmount:
                                if (issueGiftCardDialog.RefillingGiftcard)
                                {
                                    RecordIdentifier id = issueGiftCardDialog.GiftCardId;
                                    id.SecondaryID = posTransaction.TransactionId;
                                    try
                                    {
                                        Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.UpdatingGiftCard);
                                        service.AddToGiftCard(entry, settings.SiteServiceProfile, id, issueGiftCardDialog.Amount, false);
                                    }
                                    catch (Exception e)
                                    {
                                        Interfaces.Services.DialogService(entry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Properties.Resources.CouldNotConnectToStoreServer, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                                        return;
                                    }
                                    finally
                                    {
                                        Interfaces.Services.DialogService(entry).CloseStatusDialog();
                                    }
                                    giftCardID = (string) id.PrimaryID;
                                    retVal = true;
                                }
                                else
                                {
                                    newGiftCard = new GiftCard
                                    {
                                        Active = false,
                                        Balance = issueGiftCardDialog.Amount,
                                        Currency = posTransaction.StoreCurrencyCode,
                                        ID = issueGiftCardDialog.GiftCardId
                                    };
                                    newGiftCard.ID.SecondaryID = posTransaction.TransactionId;
                                    newGiftCard.Refillable = issueGiftCardDialog.ExistingGiftCard != null
                                                                ? issueGiftCardDialog.ExistingGiftCard.Refillable
                                                                : settings.SiteServiceProfile.GiftCardRefillSetting == SiteServiceProfile.GiftCardRefillSettingEnum.AlwaysYes;
                                    newGiftCard.CreatedDate = issueGiftCardDialog.ExistingGiftCard != null ? issueGiftCardDialog.ExistingGiftCard.CreatedDate : Date.Now;

                                    giftCardID = (string) issueGiftCardDialog.GiftCardId;
                                    try
                                    {
                                        service.AddNewGiftCard(entry, settings.SiteServiceProfile, newGiftCard, false, prefix, numberSequenceLowest);
                                    }
                                    catch (Exception e)
                                    {
                                        Interfaces.Services.DialogService(entry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Properties.Resources.CouldNotConnectToStoreServer, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                                        return;
                                    }
                                    finally
                                    {
                                        Interfaces.Services.DialogService(entry).CloseStatusDialog();
                                    }
                                    retVal = true;
                                }

                                amount = issueGiftCardDialog.Amount;
                                break;
                        }

                        string comment = retVal ? "" : Properties.Resources.GiftCardInUseOrInvalidID;

                        if (retVal)
                        {

                            // Add the gift card to the transaction.....
                            GiftCertificateItem giftCertificate = new GiftCertificateItem(
                                (RetailTransaction) posTransaction)
                            {
                                SerialNumber = giftCardID,
                                StoreId = posTransaction.StoreId,
                                TerminalId = posTransaction.TerminalId,
                                StaffId = (string) posTransaction.Cashier.ID,
                                TransactionId = posTransaction.TransactionId,
                                ReceiptId = posTransaction.ReceiptId,
                                Amount = amount,
                                Date = DateTime.Now,
                                Price = amount,
                                PriceWithTax = amount,
                                StandardRetailPrice = amount,
                                Quantity = 1,
                                TaxRatePct = 0,
                                Description = Properties.Resources.GiftCard,
                                Comment = giftCardID,
                                NoDiscountAllowed = true,
                                Found = true
                            };

                            // Necessary property settings for the the gift certificate "item"...
                            if (settings.SiteServiceProfile.IssueGiftCardOption == SiteServiceProfile.IssueGiftCardOptionEnum.ID)
                            {
                                giftCertificate.OriginalPrice = amount;
                            }
                            else
                            {
                                giftCertificate.OriginalPrice = issueGiftCardDialog.ExistingGiftCard != null ? issueGiftCardDialog.ExistingGiftCard.Balance : 0;
                            }
                            if (((RetailTransaction)posTransaction).SalesPerson.Exists)
                            {
                                giftCertificate.SalesPerson = (Employee)((RetailTransaction)posTransaction).SalesPerson.Clone();
                            }

                            ((RetailTransaction)posTransaction).Add(giftCertificate);
                         
                        }
                        else
                        {
                            // The card is already in use
                            Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ErrorIssuingGiftCard + " " + comment, MessageBoxButtons.OK, MessageDialogType.Generic);
                        }
                    }
                }


            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                throw;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                Interfaces.Services.DialogService(entry).CloseStatusDialog();
            }
        }

        public virtual bool VoidGiftCard(IConnectionManager entry, string giftCardId, decimal orginalBalance)
        {
            try
            {
                ISiteServiceService service = (ISiteServiceService) entry.Service(ServiceType.SiteServiceService);
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                service.StaffID = entry.CurrentStaffID;
                service.TerminalID = entry.CurrentTerminalID;

                Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.VoidingGiftCard);
                GiftCard giftCard = service.GetGiftCard(entry, settings.SiteServiceProfile, giftCardId, false);
                if (giftCard != null)
                {
                    if (!giftCard.Issued)
                    {
                        service.DeleteGiftCard(entry, settings.SiteServiceProfile, giftCardId, false);
                    }
                    else
                    {
                        giftCard.Balance = orginalBalance;
                        service.AddNewGiftCard(entry, settings.SiteServiceProfile, giftCard, false, "");
                    }
                }                
                Interfaces.Services.DialogService(entry).CloseStatusDialog();
                return true;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                throw;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                Interfaces.Services.DialogService(entry).CloseStatusDialog();
            }
        }

        /// <summary>
        /// Authorize payment with a credit memo
        /// </summary>
        public virtual bool AuthorizeGiftCardPayment(IConnectionManager entry, ref string giftCardId, ref decimal amount, IPosTransaction posTransaction, string paymentId, decimal restrictedAmount)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            bool result = false;

            if (!settings.SiteServiceProfile.UseGiftCards)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.GiftCardsDisabled, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return false;
            }

            try
            {                
                List<StorePaymentMethod> storePaymentTypes = Providers.StorePaymentMethodData.GetRecords(entry, posTransaction.StoreId, true);
                StorePaymentMethod paymentMethod = storePaymentTypes.FirstOrDefault(f => f.PaymentTypeID == paymentId);

                if (paymentMethod == null)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoGiftCardPaymentMethodFound, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                    
                    return false;
                }

                decimal transSalePmtDiff = 0M;
                decimal totalTransAmount = 0M;
                if ((posTransaction is RetailTransaction))
                {
                    transSalePmtDiff = ((RetailTransaction)posTransaction).TransSalePmtDiff;
                    totalTransAmount = ((RetailTransaction)posTransaction).NetAmountWithTax;
                }
                else if (posTransaction is CustomerPaymentTransaction)
                {
                    transSalePmtDiff = ((CustomerPaymentTransaction)posTransaction).TransSalePmtDiff;
                    totalTransAmount = ((CustomerPaymentTransaction)posTransaction).TransSalePmtDiff;
                }

                if (restrictedAmount > 0 && restrictedAmount < transSalePmtDiff)
                {
                    transSalePmtDiff = restrictedAmount;
                }

                //View the gift card dialog to enter the gift card number and to verify the gift card exists.
                using (PayGiftCardDialog dialog = new PayGiftCardDialog(entry, transSalePmtDiff, paymentMethod, giftCardId))
                {
                    bool giftCardValid = false;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        // Make sure that the gift card hasn't been used already                    
                        string enteredGiftCardID = (string)dialog.GiftCardID;
                        giftCardValid = true;

                        if (posTransaction is RetailTransaction)
                        {
                            foreach (TenderLineItem tenderItem in ((RetailTransaction)posTransaction).TenderLines)
                            {
                                if (tenderItem is GiftCertificateTenderLineItem)
                                {
                                    if (((GiftCertificateTenderLineItem)tenderItem).SerialNumber == enteredGiftCardID && !tenderItem.Voided)
                                    {
                                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.GiftCardAlreadyUsed, MessageBoxButtons.OK, MessageDialogType.Attention);

                                        giftCardValid = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (posTransaction is CustomerPaymentTransaction)
                        {
                            foreach (TenderLineItem tenderItem in ((CustomerPaymentTransaction)posTransaction).TenderLines)
                            {
                                if (tenderItem is GiftCertificateTenderLineItem)
                                {
                                    if (((GiftCertificateTenderLineItem)tenderItem).SerialNumber == enteredGiftCardID && !tenderItem.Voided)
                                    {
                                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.GiftCardAlreadyUsed, MessageBoxButtons.OK, MessageDialogType.Attention);

                                        giftCardValid = false;
                                        break;
                                    }
                                }
                            }
                        }

                        // Get information about the payment type used and make sure that this is a legal payment.
                        // At this point the gift card is actually used
                        if (giftCardValid)
                        {
                            StorePaymentMethod tenderInfo = Providers.StorePaymentMethodData.Get(entry, new RecordIdentifier(entry.CurrentStoreID, new RecordIdentifier(paymentId)), CacheType.CacheTypeApplicationLifeTime);                            

                            giftCardValid = Services.Interfaces.Services.TenderService(entry).IsTenderAllowed(entry, posTransaction, settings.Store.Currency, tenderInfo, dialog.RegisteredAmount, true, transSalePmtDiff, totalTransAmount);

                            if (!giftCardValid)
                            {
                                Interfaces.Services.DialogService(entry).ShowMessage(Services.Interfaces.Services.TenderService(entry).ErrorText, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                            }
                        }

                    }

                    if (giftCardValid)
                    {
                        // Attempt to update the gift card at this point and cancel the validation if something goes wrong while updating, like a connection problem
                        // or an unknown error serverside.
                        bool updated = false;
                        bool useWholeGiftcardBalance = false;

                        StorePaymentMethod giftcardPayment = Providers.StorePaymentMethodData.Get(entry, new RecordIdentifier(entry.CurrentStoreID, paymentId), CacheType.CacheTypeApplicationLifeTime);
                        decimal amountToPay = transSalePmtDiff;

                        Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.CommuncatingWithStoreServer);
                        Interfaces.Services.DialogService(entry).CloseStatusDialog();

                        // Are we using another payment method for change ?
                        if (!dialog.GiftCard.Refillable && paymentMethod.MinimumChangeAmount > 0 && (giftcardPayment.AboveMinimumTenderID != RecordIdentifier.Empty || !String.IsNullOrEmpty((string) giftcardPayment.AboveMinimumTenderID)))
                        {
                            if (dialog.GiftCard.Balance - amountToPay > paymentMethod.MinimumChangeAmount)
                            {
                                // Check if the other payment method is the same payment method as the gift card payment method
                                if ((string) giftcardPayment.AboveMinimumTenderID == paymentId)
                                {
                                    // If it's the same method, we should just update the gift card and not empty the whole amount from it
                                    updated = UpdateGiftCertificate(entry, (string)dialog.GiftCardID, dialog.RegisteredAmount, posTransaction);
                                }
                                else
                                {
                                    // At this point, we need to check if we can indeed return in another payment method, since the maximum over tender amount allowed
                                    // might be set to a lower number than the MinimumChangeAmount. 
                                    // This would mean that if we have a maximum over tender amount of 10, but we are about to return 50 as a credit voucher we must stop this
                                    // from happening here.
                                    StorePaymentMethod tenderInfo = Providers.StorePaymentMethodData.Get(entry, new RecordIdentifier(entry.CurrentStoreID, new RecordIdentifier(paymentId)), CacheType.CacheTypeApplicationLifeTime);                                    

                                    giftCardValid = Services.Interfaces.Services.TenderService(entry).IsTenderAllowed(entry, posTransaction, settings.Store.Currency, tenderInfo, dialog.RegisteredAmount - amountToPay, true, transSalePmtDiff, totalTransAmount);

                                    if (!giftCardValid)
                                    {
                                        Interfaces.Services.DialogService(entry).ShowMessage(Services.Interfaces.Services.TenderService(entry).ErrorText, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                                    }
                                    else
                                    {
                                        // Empty the gift card, and the remaining balance should then be given back as the other payment method
                                        updated = UpdateGiftCertificate(entry, (string)dialog.GiftCardID, dialog.GiftCard.Balance, posTransaction);
                                        useWholeGiftcardBalance = true;
                                    }
                                }
                            }
                            else
                            {
                                // Empty the gift card and return the remaining balance 
                                updated = UpdateGiftCertificate(entry, (string)dialog.GiftCardID, dialog.GiftCard.Balance, posTransaction);
                                useWholeGiftcardBalance = giftcardPayment.MaximumOverTenderAmount == 0 || dialog.GiftCard.Balance - amountToPay < giftcardPayment.MaximumOverTenderAmount;
                            }
                        }
                        else
                        {
                            // Update normally
                            updated = UpdateGiftCertificate(entry, (string)dialog.GiftCardID, dialog.UnRoundedRegisteredAmount == 0 ? dialog.RegisteredAmount : dialog.UnRoundedRegisteredAmount, posTransaction);
                        }

                        if (!updated)
                        {
                            result = false;
                            giftCardId = "";
                            amount = 0;
                        }
                        else
                        {
                            result = true;
                            giftCardId = (string) dialog.GiftCardID;
                            amount = useWholeGiftcardBalance ? dialog.GiftCard.Balance : dialog.RegisteredAmount;
                        }
                    }
                    else
                    {
                        result = false;
                        giftCardId = "";
                        amount = 0;
                    }
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                throw;
            }

            return result;

        }

        public virtual bool VoidGiftCardPayment(IConnectionManager entry, string giftCardId, IRetailTransaction retailTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            bool result = false;

            if (!settings.SiteServiceProfile.UseGiftCards)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.GiftCardsDisabled, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return true;
            }
            
            try
            {

                //LSRetailPosis.ApplicationLog.Log("GiftCard.VoidGiftCardPayment", "Cancelling the used marking of the gift card...", LSRetailPosis.LogTraceLevel.Trace);                
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                service.StaffID = settings.POSUser.ID;
                service.TerminalID = entry.CurrentTerminalID;

                ITenderLineItem tenderItem = retailTransaction.TenderLines.FirstOrDefault(w => w is GiftCertificateTenderLineItem
                                                                                               && ((GiftCertificateTenderLineItem) w).SerialNumber == giftCardId
                                                                                               && !w.Voided) ?? new GiftCertificateTenderLineItem();
                // Get the amount that is on the gift card payment line
                decimal giftCardAmount = tenderItem.Amount;

                Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.VoidingGiftCardPayment);
                service.AddToGiftCard(entry, settings.SiteServiceProfile, giftCardId, giftCardAmount, true);
                service.ActivateGiftCard(entry, settings.SiteServiceProfile, giftCardId, retailTransaction.TransactionId, retailTransaction.ReceiptId, true);
                Interfaces.Services.DialogService(entry).CloseStatusDialog();

                // No need to communication with the Store Server since the gift card is not actually updated until the end of the transaction
                result = true;

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                throw;
            }
            finally
            {
                Interfaces.Services.DialogService(entry).CloseStatusDialog();
            }

            return result;
        }

        public virtual void UpdateGiftCardPaymentReceipt(IConnectionManager entry, IGiftCertificateTenderLineItem tenderLine, IRetailTransaction transaction)
        {
            ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            service.UpdateGiftCardPaymentReceipt(entry, settings.SiteServiceProfile, tenderLine.SerialNumber, transaction.TransactionId,
                                                 transaction.StoreId, transaction.TerminalId, transaction.ReceiptId,
                                                 true);
        }

        public virtual bool UpdateGiftCertificate(IConnectionManager entry, string giftCardId, decimal amount, IPosTransaction posTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            bool result = false;
            if (!settings.SiteServiceProfile.UseGiftCards)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.GiftCardsDisabled, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return false;
            }
            
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Marking a gift card as used....", "GiftCard.UpdateGiftCertificate");
                
                try
                {

                    ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                    service.StaffID = settings.POSUser.ID;
                    service.TerminalID = entry.CurrentTerminalID;

                    decimal giftCardAmount = amount;

                    Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.UpdatingGiftCard);
                    GiftCardValidationEnum validationResult = service.UseGiftCard(entry, settings.SiteServiceProfile, ref giftCardAmount, new RecordIdentifier(giftCardId), posTransaction.TransactionId, posTransaction.ReceiptId, true);
                    Interfaces.Services.DialogService(entry).CloseStatusDialog();

                    result = validationResult == GiftCardValidationEnum.ValidationSuccess;

                    HandleGiftCardValidationEnum(entry, validationResult, amount);
                }
                catch (Exception)
                {                    
                    entry.ErrorLogger.LogMessage(LogMessageType.Error, "Error updating the gift card centrally, as used...", "GiftCard.UpdateGiftCertificate");                    

                }
                finally
                {
                    Interfaces.Services.DialogService(entry).CloseStatusDialog();
                }
                if (!result)
                {                    
                    entry.ErrorLogger.LogMessage(LogMessageType.Error, "Error updating the gift card centrally, as used...", "GiftCard.UpdateGiftCertificate");                    
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
            finally
            {
                Interfaces.Services.DialogService(entry).CloseStatusDialog();
            }

            return result;
        }

        public virtual void GiftCardPaid(IConnectionManager entry, IGiftCertificateItem giftCard, RecordIdentifier receiptID)
        {
            try
            {
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                if (settings.SiteServiceProfile.IssueGiftCardOption == SiteServiceProfile.IssueGiftCardOptionEnum.ID)
                {
                    GiftCard giftCardBO = service.GetGiftCard(entry, settings.SiteServiceProfile, giftCard.SerialNumber, false);

                    giftCardBO.ID.SecondaryID = giftCard.TransactionId;
                    service.AddNewGiftCard(entry, settings.SiteServiceProfile, giftCardBO, false,"");
                }
                service.ActivateGiftCard(entry, settings.SiteServiceProfile, giftCard.SerialNumber, giftCard.TransactionId, receiptID, true);
                service.MarkGiftCertificateIssued(entry, settings.SiteServiceProfile, giftCard.SerialNumber, true);
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
            }
            
        }

        public virtual void GetGiftCardBalance(IConnectionManager entry)
        {

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            if (!settings.SiteServiceProfile.UseGiftCards)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.GiftCardsDisabled, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return;
            }

            try
            {
                using (GetGiftCardBalanceDialog getGiftCardBalanceDialog = new GetGiftCardBalanceDialog(entry))
                {
                    getGiftCardBalanceDialog.ShowDialog();
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
            finally
            {
                Interfaces.Services.DialogService(entry).CloseStatusDialog();
            }
        }

        #endregion

        /// <summary>
        /// Displays the appropriate message to the user depending on the result given
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="result">The result from calling the Store Server gift card functions</param>
        /// <param name="amount"></param>
        protected virtual void HandleGiftCardValidationEnum(IConnectionManager entry, GiftCardValidationEnum result, decimal amount)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            switch (result)
            {
                case GiftCardValidationEnum.ValidationBalanceToLow:
                    IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.GiftCardBalanceToLow + " " + rounding.RoundForDisplay(
                        entry,
                        amount, 
                        true, 
                        false,
                        settings.Store.Currency,
                        CacheType.CacheTypeTransactionLifeTime), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;

                case GiftCardValidationEnum.ValidationCardNotActive:                    
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.GiftCardNotActive, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;

                case GiftCardValidationEnum.ValidationCardNotFound:                    
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.GiftCardNotFound, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;

                case GiftCardValidationEnum.ValidationSuccess:
                    //Services.DialogService(entry).ShowMessage(55410, MessageBoxButtons.OK, MessageBoxIcon.None);             
                    break;

                case GiftCardValidationEnum.ValidationUnknownError:                    
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.UnknownErrorOccurred, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;

                case GiftCardValidationEnum.ValidationCardHasZeroBalance:
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.GiftCardHasZeroBalance, MessageBoxButtons.OK, MessageDialogType.Generic);
                    break;
            }
        }

        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618
        }

        public virtual IErrorLog ErrorLog { set; private get; }
    }
}
