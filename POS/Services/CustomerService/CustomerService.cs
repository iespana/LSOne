using System;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.POS.Processes.Common;
using LSOne.POS.Processes.WinFormsKeyboard;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Services.WinFormsTouch;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Controls.Dialogs;
using LSOne.DataLayer.TransactionObjects.Line.CustomerDepositItem;
using LSRetailPosis;
using LSOne.Controls.SupportClasses;

namespace LSOne.Services
{
    public partial class CustomerService : ICustomerService
    {
        protected decimal _maxDiscountedPurchases = 0;
        protected decimal _previousDiscountedPurchases = 0;

        /// <summary>
        /// Enter the customer id and add the customer to the transaction
        /// </summary>
        /// <param name="IRetailTransaction">The retail tranaction</param>
        /// <returns>The retail tranaction</returns>
        public virtual IRetailTransaction EnterCustomerId(IConnectionManager entry, IRetailTransaction IRetailTransaction)
        {
            return IRetailTransaction;
        }

        public virtual bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            return settings.SiteServiceProfile != null && settings.SiteServiceProfile.CheckCustomer;
        }

        public virtual AddCustomerResultEnum AddCustomerToTransaction(IConnectionManager entry, RecordIdentifier customerID, IPosTransaction posTransaction, bool displayErrorDialogs)
        {
            AddCustomerResultEnum customerCheckResult = AddCustomerResultEnum.Success;

            if (customerID != "" && customerID != RecordIdentifier.Empty)
            {
                if (posTransaction is IRetailTransaction)
                {
                    Customer customer = Providers.CustomerData.Get(entry, customerID, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime);

                    if(customer == null)
                    {
                        return AddCustomerResultEnum.CustomerIDNotValid;
                    }

                    if (((RetailTransaction)posTransaction).Add(customer))
                    {
                        ((IRetailTransaction)posTransaction).AddInvoicedCustomer(Providers.CustomerData.Get(entry, customer.AccountNumber, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime));
                    }

                    customerCheckResult = CheckCustomer(entry, posTransaction, displayErrorDialogs);

                    //If CheckCustomer returns false then the customer isn't allowed to be added to the transaction. Msg has already been displayed                    
                    if (customerCheckResult != AddCustomerResultEnum.Success && customerCheckResult != AddCustomerResultEnum.SuccessButInvoiceBlockedChargingNotAllowed)
                    {
                        return customerCheckResult;
                    }

                    customerCheckResult = CheckInvoicedCustomer(entry, posTransaction,displayErrorDialogs);

                    //If CheckInvoicedCustomer removed the customer then it isn't allowed to be added to the transaction. Msg has already been displayed
                    if (customerCheckResult != AddCustomerResultEnum.Success && customerCheckResult != AddCustomerResultEnum.SuccessButInvoiceBlockedChargingNotAllowed)
                    {
                        return customerCheckResult;
                    }

                    if (((IRetailTransaction)posTransaction).Customer.UsePurchaseRequest)
                    {
                        ((IRetailTransaction)posTransaction).CustomerPurchRequestId = GetPurchRequestId(entry);
                    }
                }
                else if (posTransaction is CustomerPaymentTransaction)
                {
                    ((CustomerPaymentTransaction)posTransaction).Add(Providers.CustomerData.Get(entry, customerID, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime));
                }
            }
            else
            {
                return AddCustomerResultEnum.CustomerIDNotValid;
            }

            return customerCheckResult;
        }

        /// <summary>
        /// Search for the customer and add him or her to the retail transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The retail tranaction</param>
        /// <param name="returnNewCustomer">If true then when the user creates a new customer through the Search dialog the new customer is returned directly. 
        /// If false then the customer search dialog will use the name of the customer to search again in the list</param>
        /// <param name="initialSearch">Initial text to search for at the beggining of the operation</param>
        /// <returns>The retail tranaction</returns>
        public virtual IPosTransaction Search(IConnectionManager entry, IPosTransaction posTransaction, bool returnNewCustomer, string initialSearch = "")
        {
            string selectedCustomerId = "";

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            // Show the search dialog
            if (settings.VisualProfile.TerminalType == VisualProfile.HardwareTypes.Touch)
            {

                CustomerSearchDialog searchDialog = new CustomerSearchDialog(entry, returnNewCustomer, posTransaction, initialSearch);
                ((Control)settings.POSApp.POSMainWindow).Invoke(ApplicationFramework.POSShowFormDelegate, new object[] { searchDialog });

                // Quit if cancel is pressed...
                if (searchDialog.DialogResult == DialogResult.Cancel)
                {
                    searchDialog.Dispose();
                    return posTransaction;
                }
                selectedCustomerId = (string)searchDialog.SelectedCustomer.ID;
                searchDialog.Dispose();
            }
            else
            {
                throw new NotImplementedException("Keyboard forms in the Customer service are not longer supported");
            }

            //Get information about the selected customer and add it to the transaction
            AddCustomerToTransaction(entry, selectedCustomerId, posTransaction, true); // We dont care about the result of this one

            //TODO:
            //Do something with the result

            return posTransaction;
        }

        public virtual IPosTransaction Get(IConnectionManager entry,ISession session, RecordIdentifier customerID, IPosTransaction IPosTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (customerID == RecordIdentifier.Empty)
            {
                // Get the customer id...
                if (settings.VisualProfile.TerminalType == VisualProfile.HardwareTypes.Touch)
                {
                    string inputText = "";

                    DialogResult result = Interfaces.Services.DialogService(entry).KeyboardInput(ref inputText, Resources.EnterCustomerID, Resources.CustomerID, 0, InputTypeEnum.Normal);

                    // Quit if cancel is pressed...
                    if (result == DialogResult.Cancel)
                    {
                        return IPosTransaction;
                    }

                    customerID = inputText;
                }
                else
                {
                    frmInput inputDialog = new frmInput();
                    inputDialog.PromptText = Properties.Resources.EnterCustomerID; // "Enter a customer id."; 3060
                    POSFormsManager.ShowPOSForm(inputDialog);

                    // Quit if cancel is pressed...
                    if (inputDialog.DialogResult == DialogResult.Cancel)
                    {
                        inputDialog.Dispose();
                        return IPosTransaction;
                    }

                    customerID = inputDialog.InputText;
                    inputDialog.Dispose();
                }

            }

            if (IPosTransaction is IRetailTransaction)
            {
                Customer customer = Providers.CustomerData.Get(entry, customerID, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);                

                if (((IRetailTransaction)IPosTransaction).Add(customer))
                {
                    ((IRetailTransaction)IPosTransaction).AddInvoicedCustomer(Providers.CustomerData.Get(entry, customer.AccountNumber, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime));
                }

                if (((IRetailTransaction)IPosTransaction).Customer.ID != RecordIdentifier.Empty)
                {

                    //If the selected customer has Blocked = ALL then he can't be added to the transaction. Msg has been displayed.
                    AddCustomerResultEnum customerCheckResult = CheckCustomer(entry, IPosTransaction, true);
                    if (customerCheckResult != AddCustomerResultEnum.Success && customerCheckResult != AddCustomerResultEnum.SuccessButInvoiceBlockedChargingNotAllowed)
                    {
                        return IPosTransaction;
                    }

                    //If the invoice customer check returns false then the customer has Blocked = ALL and 
                    //can't be added to the transaction. Msg has been displayed.
                    customerCheckResult = CheckInvoicedCustomer(entry, IPosTransaction, true);
                    if (customerCheckResult != AddCustomerResultEnum.Success && customerCheckResult != AddCustomerResultEnum.SuccessButInvoiceBlockedChargingNotAllowed)
                    {
                        return IPosTransaction;
                    }

                    if (((IRetailTransaction)IPosTransaction).Customer.UsePurchaseRequest)
                    {
                        ((IRetailTransaction)IPosTransaction).CustomerPurchRequestId = GetPurchRequestId(entry);
                    }

                    // Added customer...
                    POSFormsManager.ShowPOSStatusPanelText(Properties.Resources.AddedCustomer + " " + (string)customer.ID +//3061
                                                            " - " + ((IRetailTransaction)IPosTransaction).InvoicedCustomer.CopyName());

                    // Trigger new price,tax and discount for the customer
                    LSOne.Services.Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, IPosTransaction, true);

                    //Get infocode information if needed.
                    Services.Interfaces.Services.InfocodesService(entry).ProcessInfocode(entry, session, (PosTransaction)IPosTransaction, 0, 0, (string)((IRetailTransaction)IPosTransaction).Customer.ID, "", "", InfoCodeLineItem.TableRefId.Customer, "", null, InfoCodeLineItem.InfocodeType.Header, true);
                    Services.Interfaces.Services.InfocodesService(entry).ProcessLinkedInfocodes(entry, session, (PosTransaction)IPosTransaction, InfoCodeLineItem.TableRefId.Customer, InfoCodeLineItem.InfocodeType.Header);
                }
                else
                {
                    // Customer not found...
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CustomerNotFound); //3062
                    POSFormsManager.ShowPOSStatusPanelText(Properties.Resources.CustomerNotFound); //3062
                }
            }
            else if (IPosTransaction.GetType() == typeof(CustomerPaymentTransaction))
            {
                Customer customer = Providers.CustomerData.Get(entry, customerID, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);
                
                ((CustomerPaymentTransaction)IPosTransaction).Add(customer);

                if (((CustomerPaymentTransaction)IPosTransaction).Customer.ID != RecordIdentifier.Empty)
                {
                    // Added customer...
                    POSFormsManager.ShowPOSStatusPanelText(Properties.Resources.AddedCustomer + " " + (string)customer.ID + //3061
                                                            " - " + ((CustomerPaymentTransaction)IPosTransaction).Customer.CopyName());
                    //Get infocode information if needed.
                    Services.Interfaces.Services.InfocodesService(entry).ProcessInfocode(entry, session, (PosTransaction)IPosTransaction, 0, 0, (string)((CustomerPaymentTransaction)IPosTransaction).Customer.ID, "", "", InfoCodeLineItem.TableRefId.Customer, "", null, InfoCodeLineItem.InfocodeType.Header, true);
                    Services.Interfaces.Services.InfocodesService(entry).ProcessLinkedInfocodes(entry, session, (PosTransaction)IPosTransaction, InfoCodeLineItem.TableRefId.Customer, InfoCodeLineItem.InfocodeType.Header);

                }
                else
                {
                    // Customer not found...
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CustomerNotFound); //3062
                    POSFormsManager.ShowPOSStatusPanelText(Properties.Resources.CustomerNotFound); //3062
                }
            }
            return IPosTransaction;
        }

        

        protected virtual AddCustomerResultEnum CheckCustomer(IConnectionManager entry, IPosTransaction IPosTransaction, bool displayDialogs)
        {
            if (((IRetailTransaction)IPosTransaction).Customer.Blocked == BlockedEnum.All)
            {
                //Display a message
                if (displayDialogs)
                {
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CustomerBlockedNoTransactionsAllowed); //This customer has been blocked. No sales or transactions are allowed.  51002              
                }
                
                //Cancel the customer account
                ((IRetailTransaction)IPosTransaction).ClearCustomer();

                return AddCustomerResultEnum.CustomerBlockedNoTransactionAllowed;
            }

            if (((IRetailTransaction)IPosTransaction).Customer.Blocked == BlockedEnum.Invoice)
            {
                //Display message
                if (displayDialogs)
                {
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.InvoicedCustomerBlockedChargingNotAllowed); //This customer has been blocked. This account can not be charged to. 51005               
                }
                return AddCustomerResultEnum.SuccessButInvoiceBlockedChargingNotAllowed;
            }

            return AddCustomerResultEnum.Success;
        }

        protected virtual AddCustomerResultEnum CheckInvoicedCustomer(IConnectionManager entry, IPosTransaction IPosTransaction, bool displayDialogs)
        {
            //If the Invoiced customer has All as blocked then the selected customer can not be added to the transaction
            if (((IRetailTransaction)IPosTransaction).InvoicedCustomer.Blocked == BlockedEnum.All)
            {
                //If Invoiced Customer is blocked then the original customer should be blocked too.
                ((IRetailTransaction)IPosTransaction).Customer.Blocked = BlockedEnum.All;

                //Display the message
                if (displayDialogs)
                {
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.InvoicedCustomerBlockedSalesNotAllowed); //The invoiced customer has been blocked. Charging to this account will not be allowed.  51004              
                }

                //Cancel all customer accounts
                ((IRetailTransaction)IPosTransaction).Customer.ID = RecordIdentifier.Empty;
                ((IRetailTransaction)IPosTransaction).InvoicedCustomer.ID = RecordIdentifier.Empty;

                return AddCustomerResultEnum.InvoicedCustomerBlockedSalesNotAllowed;
            }

            if (((IRetailTransaction)IPosTransaction).InvoicedCustomer.Blocked == BlockedEnum.Invoice)
            {
                //If a similar message has already been displayed for the original customer then don't display it again.
                if (((IRetailTransaction)IPosTransaction).Customer.Blocked != BlockedEnum.Invoice)
                {
                    if (displayDialogs)
                    {
                        LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.InvoicedCustomerBlockedChargingNotAllowed); //This customer has been blocked. This account can not be charged to.  51005                  
                    }
                }

                //If Invoiced Customer is blocked then the original customer should be blocked too.
                ((IRetailTransaction)IPosTransaction).Customer.Blocked = BlockedEnum.Invoice;

                return AddCustomerResultEnum.SuccessButInvoiceBlockedChargingNotAllowed;
            }

            return AddCustomerResultEnum.Success;
        }

        /// <summary>
        /// Search for a customer and return the customer ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="inputRequired">If true then the user has to select a customer.</param>
        /// <param name="customerName"></param>
        /// <param name="returnNewCustomer">If true then when the user creates a new customer through the Search dialog the new customer is returned directly. 
        /// If false then the customer search dialog will use the name of the customer to search again in the list</param>
        /// <returns>Customer ID as a string</returns>
        public virtual string Search(IConnectionManager entry, bool inputRequired, ref Name customerName, bool returnNewCustomer, IPosTransaction transaction)
        {
            bool continueSearch = true;
            Customer customer = null;

            while (customer == null && continueSearch)
            {
                continueSearch = inputRequired;
                customer = Search(entry, returnNewCustomer, transaction);

                if (customer != null)
                {
                    customerName = customer.CopyName();
                    return (string)customer.ID;
                }
            }
            return "";
        }

        /// <summary>
        /// Search for the customer and return the customer's information if one is selected otherwise the function returns null
        /// </summary>        
        /// <param name="entry">The entry into the database</param>
        /// <param name="returnNewCustomer">If true then when the user creates a new customer through the Search dialog the new customer is returned directly. 
        /// If false then the customer search dialog will use the name of the customer to search again in the list</param>
        /// <param name="initialSearch">Initial text to search for at the beggining of the operation</param>
        /// <returns>Customer's information</returns>
        public virtual Customer Search(IConnectionManager entry, bool returnNewCustomer, IPosTransaction transaction, string initialSearch = "")
        {
            string selectedCustomerId = "";

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            using (Form searchDialog = (Form)new CustomerSearchDialog(entry, returnNewCustomer, transaction, initialSearch))
            {
                // Quit if cancel is pressed...
                if (searchDialog.ShowDialog() == DialogResult.Cancel)
                {
                    return null;
                }
                selectedCustomerId = (string)(((CustomerSearchDialog)searchDialog).SelectedCustomer.ID);
            }
            //Get information about the selected customer and return it
            if (selectedCustomerId != "")
            {
                //Get the customer info...
                Customer customer = Providers.CustomerData.Get(entry, selectedCustomerId, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);                
                return customer;
            }
            //No customer was selected
            return null;
        }

        /// <summary>
        /// Sets the customer balance of the customer
        /// </summary>
        /// <param name="IRetailTransaction">The retail tranaction</param>
        /// <returns>The retail tranaction</returns>
        public virtual IRetailTransaction Balance(IConnectionManager entry, IRetailTransaction IRetailTransaction)
        {
            return IRetailTransaction;
        }

        /// <summary>
        /// Sets the customer status of the customer
        /// </summary>
        /// <param name="IRetailTransaction">The retail tranaction</param>
        /// <returns>The retail tranaction</returns>
        public virtual IRetailTransaction Status(IConnectionManager entry, IRetailTransaction IRetailTransaction)
        {
            return IRetailTransaction;
        }

        /// <summary>
        /// Register information about a new customer into the database
        /// </summary>
        /// <returns>Returns true if operations is successful</returns>
        public virtual bool AddNewWithDialog(IConnectionManager entry, ref Customer customer)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            if (settings.TrainingMode)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.NewCustomerNotAllowedInTrainingMode, MessageBoxButtons.OK, MessageDialogType.Generic);
                return false;
            }

            ShowKeyboardInputHandler handler = (ref string text, string promptText, string ghostText, int length, InputTypeEnum type) => Interfaces.Services.DialogService(entry).KeyboardInput(ref text, promptText, ghostText, length, type);            

            var dialog = new AddCustomerDialog(entry, handler);

            dialog.ShowDialog();

            if (dialog.DialogResult == DialogResult.OK)
            {                
                if (settings.SiteServiceProfile.SiteServiceAddress != "" && settings.SiteServiceProfile.CheckCustomer)
                {
                    try
                    {
                        Action action = () =>
                        {
                            var service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                            dialog.NewCustomer = service.SaveCustomer(entry, settings.SiteServiceProfile, dialog.NewCustomer, true, true);
                        };

                        Exception ex;

                        Interfaces.Services.DialogService(entry).ShowSpinnerDialog(action, Resources.CreatingCustomer, Resources.CreatingCustomer, out ex);

                        if (ex != null)
                        {
                            throw ex;
                        }
                    }
                    catch(Exception e)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Resources.CouldNotConnectToStoreServer, MessageBoxButtons.OK, MessageDialogType.Attention);
                        dialog.NewCustomer.LocallySaved = true;
                    }
                }

                Providers.CustomerData.SaveWithAddresses(entry, dialog.NewCustomer);
                customer = dialog.NewCustomer;

                return true;
            }

            return false;
        }

        public bool EditWithDialog(IConnectionManager entry, Customer customer, IPosTransaction transaction)
        {
            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                if (settings.TrainingMode)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.EditCustomerNotAllowedInTrainingMode, MessageBoxButtons.OK, MessageDialogType.Generic);
                    return false;
                }

                ShowKeyboardInputHandler handler = (ref string text, string promptText, string ghostText, int length, InputTypeEnum type) => Interfaces.Services.DialogService(entry).KeyboardInput(ref text, promptText, ghostText, length, type);                

                if (settings.SiteServiceProfile.SiteServiceAddress != "" && settings.SiteServiceProfile.CheckCustomer)
                {
                    try
                    {
                        Interfaces.Services.DialogService(entry).UpdateStatusDialog("");
                        Customer centralCustomerInfo = Interfaces.Services.SiteServiceService(entry).GetCustomer(entry, settings.SiteServiceProfile, customer.ID, settings.SiteServiceProfile.CheckCustomer, true);

                        // If the customer was created locally and has not been transferred to central database this call will return a null
                        // as the customer doesn't exist centrally
                        if (centralCustomerInfo == null)
                        {
                            //Update the customer to NOT be locally saved and save it centrally and locally
                            customer.LocallySaved = false;
                            Interfaces.Services.SiteServiceService(entry).SaveCustomer(entry, settings.SiteServiceProfile, customer, settings.SiteServiceProfile.CheckCustomer, true);
                            Providers.CustomerData.Save(entry, customer);
                        }
                        else
                        {
                            //If the customer is found centrally then use that information as it could have changed 
                            //since the local information was last updated
                            customer = centralCustomerInfo;
                        }                        

                        Interfaces.Services.DialogService(entry).CloseStatusDialog();
                    }
                    catch
                    {
                        //Don't do anything - use the customer information from the local database
                    }
                }

                EditCustomerDialog dlg = new EditCustomerDialog(entry, customer, handler, transaction);
                DialogResult result = dlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    customer = dlg.Customer;
                    if (settings.SiteServiceProfile.SiteServiceAddress != "" && settings.SiteServiceProfile.CheckCustomer)
                    {
                        try
                        {
                            Interfaces.Services.DialogService(entry).UpdateStatusDialog("");
                            customer = Interfaces.Services.SiteServiceService(entry).SaveCustomer(entry, settings.SiteServiceProfile, customer, true, true);
                        }
                        catch
                        {
                            Interfaces.Services.DialogService(entry).ShowMessage(Resources.CouldNotConnectToStoreServer, MessageBoxButtons.OK, MessageDialogType.Attention);
                            customer.LocallySaved = true;
                            
                            ((Control)settings.POSApp.POSMainWindow).Invoke(ApplicationFramework.PosShowStatusBarInfoDelegate, new object[] { Resources.TransferCustomerInformation, null, TaskbarSection.LocalCustomers });
                        }
                    }

                    Providers.CustomerData.SaveWithAddresses(entry, customer);

                    return true;
                }

                return false;
            }
            finally
            {
                Interfaces.Services.DialogService(entry).CloseStatusDialog();
            }
        }

        public bool EditWithDialog(IConnectionManager entry, RecordIdentifier customerID, IPosTransaction transaction)
        {
            Customer customer = Providers.CustomerData.Get(entry, customerID, UsageIntentEnum.Normal);
            return EditWithDialog(entry, customer, transaction);
        }

        /// <summary>
        /// Updates the customer information in the database
        /// </summary>
        /// <param name="customerId">The customer id</param>
        /// <returns>Returns true if operations is successful</returns>
        public virtual bool Update(IConnectionManager entry, string customerId)
        {
            return false;
        }
        /// <summary>
        /// Delete the customer from the database if the customer holds no customer transactions.
        /// </summary>
        /// <param name="customerId">The customer id</param>
        /// <returns>Returns true if operations is successful</returns>
        public virtual bool Delete(IConnectionManager entry, string customerId)
        {
            return false;
        }

        /// <summary>
        /// Show customer transactions for customer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        public virtual void Transactions(IConnectionManager entry, IPosTransaction transaction)
        {

            /************************************************************************************************
            
            Please see Development packs 2017.2 and older for the legacy code that was in this operation            

            ************************************************************************************************* */

        }     


        public virtual void AuthorizeCustomerAccountPayment(IConnectionManager entry, ref CustomerStatusValidationEnum valid, ref string comment, ref string manualAuthenticationCode, string customerId, ref decimal amount, IRetailTransaction transaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Customer.AuthorizeCustomerAccountPayment()", this.ToString());
                //Get the customer information for the customer

                Customer tempCust = Providers.CustomerData.Get(entry, customerId, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime);

                if (tempCust == null)
                {
                    tempCust = new Customer();
                }

                if (!string.IsNullOrEmpty(tempCust.AccountNumber))
                {
                    Customer tempInvCust = Providers.CustomerData.GetTemporaryInvoiceCustomer(entry, tempCust.AccountNumber, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime);
                    
                    if (tempInvCust != null)
                    {
                        if (tempInvCust.Blocked == BlockedEnum.All)
                        {
                            tempCust.Blocked = tempInvCust.Blocked;
                        }
                        else if (tempInvCust.Blocked == BlockedEnum.Invoice && tempCust.Blocked != BlockedEnum.All)
                        {
                            tempCust.Blocked = BlockedEnum.Invoice;
                        }
                    }
                }

                valid = CustomerStatusValidationEnum.Valid;

                // Using the transaction services to validate the transaction
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                service.ValidateCustomerStatus(entry, settings.SiteServiceProfile,  ref valid, ref comment, customerId, ref amount, transaction.StoreCurrencyCode, settings.SiteServiceProfile.CheckCustomer);


                if (comment.Length > 0)
                {
                    if (comment.Substring(0, 1) == "\t")
                    {
                        comment = comment.Remove(0, 1);
                    }

                    if(valid == CustomerStatusValidationEnum.Valid)
                    {
                        valid = CustomerStatusValidationEnum.Invalid;
                    }
                    return;
                }


                if (tempCust.Blocked != BlockedEnum.Nothing)
                {
                    comment = Properties.Resources.CustomerBlockedCannotBeCharged; //It is not authorized to charge to this account 51006
                    valid = CustomerStatusValidationEnum.Invalid;
                    return;
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }

        }

        public virtual void CustomerAccountCreditMemo(IConnectionManager entry, RecordIdentifier customerId, RecordIdentifier receiptId,
            string currency, decimal currencyAmount, decimal amount, decimal currencyAmountDis, decimal amountDis, RecordIdentifier storeId,
            RecordIdentifier terminalId, RecordIdentifier transactionId)
        {

            try
            {
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                service.CustomerAccountCreditMemo(entry, settings.SiteServiceProfile, customerId, receiptId,
                    currency, currencyAmount, amount, currencyAmountDis, amountDis, storeId,
                    terminalId, transactionId, settings.SiteServiceProfile.CheckCustomer);

            }
            catch (Exception e)
            {
                LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(e.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }

        }

        public virtual void CustomerAccountPayment(IConnectionManager entry, RecordIdentifier customerId, RecordIdentifier receiptId,
            string currency, decimal currencyAmount, decimal amount, decimal currencyAmountDis, decimal amountDis, RecordIdentifier storeId,
            RecordIdentifier terminalId, RecordIdentifier transactionId)
        {
            try
            {
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                service.CustomerAccountPayment(entry, settings.SiteServiceProfile, customerId, receiptId,
                    currency, currencyAmount, amount, currencyAmountDis, amountDis, storeId,
                    terminalId, transactionId, settings.SiteServiceProfile.CheckCustomer);
            }
            catch (Exception e)
            {
                LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(e.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        /// <summary>
        /// Make a deposit to customer account
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="transaction">Current <see cref="ICustomerPaymentTransaction"/> transaction</param>
        /// <param name="initialAmount">Initial deposit amount</param>
        public virtual void CustomerAccountDeposit(IConnectionManager entry, ISession session, ref IPosTransaction transaction, decimal initialAmount = 0)
        {
            if (transaction == null)
            {
                return;
            }

            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
          
                if (transaction.GetType() == typeof(RetailTransaction))
                {
                    // If no item lines and no payment lines have been added to the transaction, then we can hijack it and change it
                    // to a CustomerPaymentTransaction.  This is necessary in the case where the user has selected the customer before
                    // pressing the operation button for the Customer Deposit operation.
                    if ((((RetailTransaction)transaction).SaleItems.Count != 0) || ((RetailTransaction)transaction).TenderLines.Count != 0)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Resources.ConcludeTransactionCustomerAccount);
                        return;
                    }
                }
                else if (transaction.GetType() != typeof(CustomerPaymentTransaction))
                {
                    // If the posTransaction is not a CustomerPaymentTransaction then some other transaction
                    // is active and this operation is invalid at this point.

                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoItemsToDiscount);
                    return;
                }

                // Here we hijack the posTransaction and change it to a CustomerPaymentTransaction.
                var tempTrans = new CustomerPaymentTransaction(settings.Store.Currency)
                {
                    TransactionId = transaction.TransactionId,
                    ReceiptId = transaction.ReceiptId,
                    Training = transaction.Training,
                    TerminalId = transaction.TerminalId,
                    StoreId = transaction.StoreId,
                    LastRunOperation = transaction.LastRunOperation
                };
                tempTrans.Cashier.ID = transaction.Cashier.ID;
                tempTrans.Add(transaction.GetType() == typeof(RetailTransaction) 
                                ? ((RetailTransaction)transaction).Customer
                                : ((CustomerPaymentTransaction)transaction).Customer);

                // Only a single deposit is allowed per transaction
                if (((CustomerPaymentTransaction)tempTrans).CustomerDepositItem != null)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.OnlySingleDepositPerTransaction);
                    return;
                }

                //display dialog if only a customer is selected or only amount was entered in the NumPad
                if (RecordIdentifier.IsEmptyOrNull(((CustomerPaymentTransaction)tempTrans).Customer.ID) 
                    || ( !RecordIdentifier.IsEmptyOrNull(((CustomerPaymentTransaction)tempTrans).Customer.ID) && initialAmount == decimal.Zero))
                {
                    using (var dlg = new CustomerAccountDepositDialog(entry, session, settings,
                                                                      tempTrans,
                                                                      initialAmount))
                    {
                        dlg.ShowDialog();

                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            transaction = tempTrans;
                        }
                    }
                }
                else if (!RecordIdentifier.IsEmptyOrNull(((CustomerPaymentTransaction) tempTrans).Customer.ID) && initialAmount != decimal.Zero)
                {
                    // Create the customer deposit item
                    var depositItem = new CustomerDepositItem
                    {
                        Description = Resources.CustomerAccountDeposit,
                        Amount = initialAmount
                    };
                    tempTrans.CustomerDepositItem = depositItem;
                    transaction = tempTrans;
                }
            }
            catch (Exception e)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(e.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                throw;
            }
        }

        public virtual void PaymentIntoCustomerAccount(IConnectionManager entry, ICustomerPaymentTransaction transaction, RecordIdentifier storeId, RecordIdentifier terminalId)
        {
            try
            {
                CustomerPaymentTransaction customerTransaction = (CustomerPaymentTransaction)transaction;

                if(customerTransaction.Training)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoCustomerDepositOnTraining);
                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Transaction is a training transaction - no payment into customer account", "Customer.PaymentIntoCustomerAccount");
                    return;
                }

                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                decimal amountCom = Services.Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                    entry,
                    settings.Store.Currency,
                    settings.CompanyInfo.CurrencyCode,
                    settings.CompanyInfo.CurrencyCode,
                    transaction.Amount);

                ISiteServiceService service = (ISiteServiceService) entry.Service(ServiceType.SiteServiceService);
                service.PaymentIntoCustomerAccount(entry, settings.SiteServiceProfile, customerTransaction.Customer.ID, customerTransaction.ReceiptId,
                    customerTransaction.StoreCurrencyCode, customerTransaction.Amount, amountCom, storeId,
                    terminalId, transaction.TransactionId, settings.SiteServiceProfile.CheckCustomer);

            }
            catch (ClientTimeNotSynchronizedException exception)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(exception.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
            catch (EndpointNotFoundException)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.CouldNotConnectToSiteService, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                throw;
            }

            catch (FaultException)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.SiteServiceAuthenticationFailed, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                throw;
            }

            catch (Exception e)
            {
                LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(e.Message, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        public virtual bool CustomerHasGoneOverDiscountedPurchaseLimit(
            IConnectionManager entry,
            RecordIdentifier customerId,
            IRetailTransaction retailTransaction,
            out decimal maxDiscountedPurchases,
            out decimal currentDiscountedPurchases)
        {
            maxDiscountedPurchases = 0;
            currentDiscountedPurchases = 0;

            if (retailTransaction.Customer == null || retailTransaction.Customer.ID == RecordIdentifier.Empty)
            {
                return false;
            }

            bool transactionHasCustomerDiscountLines =
                (from saleItems in retailTransaction.SaleItems
                 from dl in saleItems.DiscountLines
                 where dl.DiscountType == DiscountTransTypes.Customer
                 select dl).Any();

            if (!transactionHasCustomerDiscountLines) return false;

            var customerGroup = Providers.CustomerGroupData.GetDefaultCustomerGroup(entry, customerId);
            if (customerGroup == null || !customerGroup.UsesDiscountedPurchaseLimit) return false;

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            // Update the two numbers for the new customer
            var siteService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
            try
            {
                siteService.CustomersDiscountedPurchasesStatus(entry, settings.SiteServiceProfile, (string)customerId, out _maxDiscountedPurchases, out _previousDiscountedPurchases, true);
            }
            catch (Exception)
            {
                return true;
            }

            var discountedAmountInCurrentTransaction =
                (from item in retailTransaction.SaleItems
                 where item.DiscountLines.Any(dl => dl.DiscountType == DiscountTransTypes.Customer)
                 select item.NetAmountWithTax).Sum();

            bool customerHasPurchasedTooMuchDiscounted =
                _maxDiscountedPurchases < _previousDiscountedPurchases + discountedAmountInCurrentTransaction;

            maxDiscountedPurchases = _maxDiscountedPurchases;
            currentDiscountedPurchases = _previousDiscountedPurchases + discountedAmountInCurrentTransaction;
            return customerHasPurchasedTooMuchDiscounted;
        }


        protected virtual string GetPurchRequestId(IConnectionManager entry)
        {
            try
            {
                string inputText = "";

                Interfaces.Services.DialogService(entry).KeyboardInput(ref inputText, Resources.EnterPurchaseRequestID, Resources.PurchaseRequestID, 20, InputTypeEnum.Normal);

                return inputText;
            }
            catch (Exception)
            {

            }

            return "";

        }     
        
        public virtual void CustomerTransactionReport(IConnectionManager entry, IPosTransaction transaction)
        {
            throw new NotImplementedException("This opertation has not been implemented");
        }

        public virtual void CustomerBalanceReport(IConnectionManager entry, IPosTransaction transaction)
        {
            throw new NotImplementedException("This opertation has not been implemented");
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
