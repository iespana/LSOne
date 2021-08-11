using LSOne.Controls.Dialogs;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem;
using LSOne.DataLayer.TransactionObjects.Line.IncomeExpenseItem;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesInvoiceItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesOrderItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.POS.Processes.Common;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.WinFormsTouch;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LSOne.Services
{
    /// <summary>
    /// All the functionality needed for central suspension operations
    /// </summary>
    public partial class CentralSuspensionService : ICentralSuspensionService
    {
        bool continueOperation = true;

        /// <summary>
        /// An logging interface that can be used to log errors
        /// </summary>
        public virtual IErrorLog ErrorLog
        {
            set
            {

            }
        }        

        /// <summary>
        /// Initializes a new instance of the <see cref="CentralSuspensionService"/> class.
        /// </summary>
        public CentralSuspensionService()
        {

        }

        /// <summary>
        /// Initializes the Central suspension service and sets the database connection for the service
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618
        }

        /// <summary>
        /// Test function that can be used when developing the interface to make sure that
        /// the connection has been made
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        public virtual void Test(IConnectionManager entry)
        {
            MessageBox.Show("You have reached the Central Suspension Service");
        }

        #region Suspend Transaction

        /// <summary>
        /// Does all checks needed to make sure that central suspension is allowed, if needed asks for user input and then suspends the transaction to a central database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">The current session</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <param name="retailTransaction">The current retail transaction</param>
        /// <param name="transactionTypeID">The type of suspension the transaction should be suspended as</param>
        /// <param name="msgHandler">A message function that the service uses to display any messages to the user</param>
        /// <param name="keyboardHandler">A message function that the service uses to display a keyboard for the user to enter any data</param>
        /// <param name="forecourtAllowsSuspension">When using forecourt the configuration for whether suspension is allowed is on the hardware profile.</param>
        /// <param name="cultureInfo">The current culture info settings</param>
        /// <returns>The unique ID of the suspension</returns>
        public virtual RecordIdentifier SuspendTransaction(
            IConnectionManager entry,
            ISession session,
            ISettings settings,
            IRetailTransaction retailTransaction,
            RecordIdentifier transactionTypeID,
            ShowMessageHandler msgHandler,
            ShowKeyboardInputHandler keyboardHandler,
            bool forecourtAllowsSuspension,
            CultureInfo cultureInfo)
        {
            // Are there any items to suspend?
            if (AnyItemsToSuspend(retailTransaction, msgHandler) == false)
            {
                return RecordIdentifier.Empty;
            }

            if (forecourtAllowsSuspension == false)
            {
                //If the transaction contains fuel items then the fuel items have to be voided before the transaction is suspended.
                if (retailTransaction.ISaleItems.Count(c => c.Voided == false && c.ItemClassType == SalesTransaction.ItemClassTypeEnum.FuelSalesLineItem) > 0)
                {
                    msgHandler?.Invoke(Properties.Resources.FuelItemsHaveToBeVoided);
                    return RecordIdentifier.Empty;
                }
            }

            //TODO: add the tender lines to income accounts instead of stopping the process
            if (retailTransaction.NoOfPaymentLines > 0 && retailTransaction.ITenderLines.Count(c => c.Voided == false) > 0)
            {
                msgHandler?.Invoke(Properties.Resources.NotPossibleToSuspendVoidPayments);
                return RecordIdentifier.Empty;
            }

            if (!settings.SuppressUI && settings.SiteServiceProfile.UserConfirmation)
            {
                continueOperation = (msgHandler?.Invoke(Properties.Resources.AreYouSureSuspendTransaction, MessageBoxButtons.YesNo, MessageDialogType.Attention) == System.Windows.Forms.DialogResult.Yes);
            }

            if (continueOperation)
            {
                // If we are suppressing UI (i.e on Omni server) then this will never be supported since it requires user input.

                List<SuspendedTransactionAnswer> additionalInfo = null;
                if (!settings.SuppressUI)
                {
                    additionalInfo = GetAdditionalInformation(entry, session, retailTransaction, transactionTypeID, msgHandler, keyboardHandler, cultureInfo);
                }

                // We suspend the transaction now to get the suspended transaction ID.
                RecordIdentifier suspendedId = RecordIdentifier.Empty;
                XElement xmlTransaction = retailTransaction.ToXML();
                var saveSuspendedTransLocally = SaveSuspendedTransaction(entry, settings, retailTransaction, transactionTypeID, msgHandler, xmlTransaction, additionalInfo, ref suspendedId);

                // Get a new transaction ID on the transaction instead of taking it from the suspended transaction
                retailTransaction.TransactionId = (string)DataProviderFactory.Instance.GenerateNumber<IPosTransactionData, PosTransaction>(entry);
                
                // We should not return the "suspendedID" locally at all when we use central suspensions
                // It is just a temporary ID that is only used for suspend/recall, and to avoid collisions with other number sequences
                if (!settings.SiteServiceProfile.UseCentralSuspensions)
                {
                    retailTransaction.SuspendedId = (string)suspendedId;
                }
                
                //xmlTransaction = retailTransaction.ToXML();//nasty

                if (Interfaces.Services.HospitalityService(entry).SendToStationPrinter(entry, retailTransaction, sendAllRemainingItems:true, isPaymentOperation:false))
                {
                    XElement xmlTransactionAfterSendingToStationPrinter = retailTransaction.ToXML();

                    // If the Xml changed when sending it to station printer we need to Re-suspend the transaction with the new xml
                    if (xmlTransaction.Value != xmlTransactionAfterSendingToStationPrinter.Value)
                    {
                        SaveSuspendedTransaction(entry, settings, retailTransaction, transactionTypeID, msgHandler, xmlTransactionAfterSendingToStationPrinter, additionalInfo, ref suspendedId);
                    }
                }

                if (additionalInfo != null)
                {
                    retailTransaction.SuspendTransactionAnswers = additionalInfo;
                }

                // Mark the transaction as suspended.
                retailTransaction.EntryStatus = TransactionStatus.OnHold;

                // This text will be printed on the receipt
                if (saveSuspendedTransLocally)
                    retailTransaction.SuspendDestination = Properties.Resources.Terminal + ": " + (string)entry.CurrentTerminalID;  // Terminal: x
                else
                    retailTransaction.SuspendDestination = Properties.Resources.CentralizedSuspension;  // Transaction suspended centrally

                //TODO: move the Cash changer service into the new service model.
                //// Releasing cash from the cash changer if there is something there
                //if (LSRetailPosis.Settings.HardwareProfiles.CashChanger.DeviceType != LSRetailPosis.DevUtilities.Enums.CashChangerDeviceTypes.None)
                //{
                //    if (LSRetailPosis.ApplicationServices.ICashChanger != null)
                //        LSRetailPosis.ApplicationServices.ICashChanger.Regret(LSRetailPosis.DevUtilities.Enums.CashChangerRegretType.REGRETTYPE_ALL);
                //}                

                return suspendedId;
            }

            return RecordIdentifier.Empty;
        }

        /// <summary>
        /// Saves a suspended transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current settings</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="transactionTypeID">The type of suspension that is being saved</param>
        /// <param name="msgHandler">The handler that will display any information to the user</param>
        /// <param name="xmlTransaction">The transaction as an XML</param>
        /// <param name="additionalInfo">The answers on the suspension</param>
        /// <param name="suspendedId">The ID for the suspension</param>
        /// <returns></returns>
        protected virtual bool SaveSuspendedTransaction(IConnectionManager entry, ISettings settings, IRetailTransaction retailTransaction,
            RecordIdentifier transactionTypeID, ShowMessageHandler msgHandler, XElement xmlTransaction, List<SuspendedTransactionAnswer> additionalInfo,
            ref RecordIdentifier suspendedId)
        {
            bool saveSuspendedTransLocally = true;

            if (settings.SiteServiceProfile.UseCentralSuspensions)
            {
                saveSuspendedTransLocally = false;

                try
                {
                    if (!settings.SuppressUI)
                    {
                        Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.SuspendingTransaction);
                    }

                    ISiteServiceService service = (ISiteServiceService) entry.Service(ServiceType.SiteServiceService);
                    suspendedId = service.SuspendTransaction(
                        entry, 
                        settings.SiteServiceProfile, 
                        suspendedId, 
                        transactionTypeID, 
                        xmlTransaction.ToString(),
                        retailTransaction.NetAmount, 
                        retailTransaction.NetAmountWithTax, 
                        additionalInfo, 
                        true);

                    saveSuspendedTransLocally = suspendedId == RecordIdentifier.Empty;

                    if (!settings.SuppressUI)
                    {
                        Interfaces.Services.DialogService(entry).CloseStatusDialog();
                    }
                }
                catch
                {
                    msgHandler?.Invoke(Properties.Resources.CentralFunctionalityNotWorking);
                    saveSuspendedTransLocally = true;
                }
                finally
                {
                    if (!settings.SuppressUI)
                    {
                        Interfaces.Services.DialogService(entry).CloseStatusDialog();
                    }
                }
            }

            //Central suspension is not in use or an error occurred in communicating with the store server
            if (saveSuspendedTransLocally)
            {
                suspendedId = SuspendTransactionLocally(entry, transactionTypeID, xmlTransaction.ToString(), retailTransaction,
                    additionalInfo, suspendedId);
            }
            return saveSuspendedTransLocally;
        }

        /// <summary>
        /// Returns the number of suspended transactions
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <param name="limitToTerminal">If true then the count is limited to the local terminal otherwise it is limited to the store</param>
        /// <returns>
        /// The number of suspended transactions
        /// </returns>
        public virtual int GetSuspendedTransactionCount(IConnectionManager entry, ISettings settings, bool limitToTerminal)
        {
            if (settings.SiteServiceProfile.UseCentralSuspensions)
            {
                try
                {
                    ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                    return service.GetSuspendedTransCount(entry, 
                        settings.SiteServiceProfile,
                        entry.CurrentStoreID,
                        limitToTerminal ? entry.CurrentTerminalID : RecordIdentifier.Empty, 
                        RecordIdentifier.Empty, 
                        RetrieveSuspendedTransactions.OnlyCentrallySaved,
                        true) 
                        + 
                        Providers.SuspendedTransactionData.GetCount(entry, entry.CurrentStoreID, 
                        limitToTerminal ? entry.CurrentTerminalID : RecordIdentifier.Empty,
                        RecordIdentifier.Empty,
                        RetrieveSuspendedTransactions.OnlyLocallySaved);
                }
                catch
                {
                    // RecordIdentifier.Empty here means that we want to get all suspended transaction no matter which type they have
                    return Providers.SuspendedTransactionData.GetCount(entry, entry.CurrentStoreID,
                        limitToTerminal ? entry.CurrentTerminalID : RecordIdentifier.Empty,
                        RecordIdentifier.Empty,
                        RetrieveSuspendedTransactions.OnlyLocallySaved);
                }
            }
            else
            {
                // RecordIdentifier.Empty here means that we want to get all suspended transaction no matter which type they have
                return Providers.SuspendedTransactionData.GetCount(entry, entry.CurrentStoreID, limitToTerminal ? entry.CurrentTerminalID : RecordIdentifier.Empty, RecordIdentifier.Empty, RetrieveSuspendedTransactions.All);
            }
        }

        /// <summary>
        /// Checks if any additional information should be asked for when the transaction is being suspended
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">The current session within the POS</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="transactionTypeID">What type of suspension is being done</param>
        /// <param name="msgHandler">The handler that will display any information to the user</param>
        /// <param name="keyboardHandler">The handler that will display any keyboard to the user</param>
        /// <param name="cultureInfo">The culture information that should be used in any dialogs</param>
        /// <returns></returns>
        protected virtual List<SuspendedTransactionAnswer> GetAdditionalInformation(
            IConnectionManager entry,
            ISession session,
            IRetailTransaction retailTransaction, 
            RecordIdentifier transactionTypeID, 
            ShowMessageHandler msgHandler, 
            ShowKeyboardInputHandler keyboardHandler, 
            CultureInfo cultureInfo)
        {
            List<SuspendedTransactionAnswer> answerList = new List<SuspendedTransactionAnswer>();

            List<SuspensionTypeAdditionalInfo> addInfoList = Providers.SuspensionTypeAdditionalInfoData.GetList(entry, transactionTypeID);
            if (addInfoList.Count == 0)
            {
                return null;
            }

            int fieldOrder = -1;

            foreach (SuspensionTypeAdditionalInfo addInfo in addInfoList.OrderBy(o => o.Order))
            {
                fieldOrder++;
                switch (addInfo.Infotype)
                {
                    case SuspensionTypeAdditionalInfo.InfoTypeEnum.Text:
                        GetTextInfo(addInfo, answerList, fieldOrder);
                        break;
                    case SuspensionTypeAdditionalInfo.InfoTypeEnum.Customer:
                        GetCustomerInfo(entry, addInfo, answerList, fieldOrder, retailTransaction);
                        break;
                    case SuspensionTypeAdditionalInfo.InfoTypeEnum.Name:
                        GetNameInfo(entry, addInfo, answerList, fieldOrder);
                        break;
                    case SuspensionTypeAdditionalInfo.InfoTypeEnum.Address:
                        GetAddressInfo(entry, addInfo, answerList, fieldOrder);
                        break;
                    case SuspensionTypeAdditionalInfo.InfoTypeEnum.Infocode:
                        GetInfocodeInfo(entry, session, retailTransaction, addInfo, answerList, fieldOrder);
                        break;
                    case SuspensionTypeAdditionalInfo.InfoTypeEnum.Date:
                        GetDateInfo(addInfo, answerList, fieldOrder);
                        break;
                    case SuspensionTypeAdditionalInfo.InfoTypeEnum.Other:
                        break;
                    default:
                        break;
                }
            }
            
            return answerList;
        }

        /// <summary>
        /// Retrieves date information for the suspension
        /// </summary>
        /// <param name="addInfo"></param>
        /// <param name="answerList">The list of answers already on the suspension</param>
        /// <param name="fieldOrder"></param>
        protected virtual void GetDateInfo(SuspensionTypeAdditionalInfo addInfo, List<SuspendedTransactionAnswer> answerList, int fieldOrder)
        {
            using (DatePickerDialog dlgDate = new DatePickerDialog(DateTime.Now.Date))
            {
                dlgDate.InputRequired = addInfo.Required;
                dlgDate.Caption = addInfo.Text;

                if(dlgDate.ShowDialog() == DialogResult.OK)
                {
                    SuspendedTransactionAnswer answer = new SuspendedTransactionAnswer();
                    answer = new SuspendedTransactionAnswer();
                    answer.RecordID = Guid.NewGuid();
                    answer.DateResult = new Date(dlgDate.SelectedDate);

                    PopulateAnswer(answer, fieldOrder, addInfo);
                    answerList.Add(answer);
                }               
            }
        }

        /// <summary>
        /// Populates the answer that was entered by the user
        /// </summary>
        /// <param name="suspendedTransactionAnswer">The suspension answer to be populated</param>
        /// <param name="fieldOrder">The order that this answer is on the suspension</param>
        /// <param name="addInfo">The information that was created</param>
        protected virtual void PopulateAnswer(SuspendedTransactionAnswer suspendedTransactionAnswer, int fieldOrder, SuspensionTypeAdditionalInfo addInfo)
        {
            suspendedTransactionAnswer.FieldOrder = fieldOrder;
            suspendedTransactionAnswer.InformationType = addInfo.Infotype;            
            suspendedTransactionAnswer.Prompt = addInfo.Text;            
        }

        /// <summary>
        /// Searches for and retrieves information about the selected customer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="addInfo"></param>
        /// <param name="answerList">The list of answers already on the suspension</param>
        /// <param name="fieldOrder">The order that this answer is on the suspension</param>
        /// <param name="transaction">Current transaction</param>
        protected virtual void GetCustomerInfo(IConnectionManager entry, SuspensionTypeAdditionalInfo addInfo, List<SuspendedTransactionAnswer> answerList, int fieldOrder, IPosTransaction transaction)
        {
            Name customerName = new Name();
            string customerID = Interfaces.Services.CustomerService(entry).Search(entry, addInfo.Required, ref customerName, true, transaction);
            
            if (customerID != "")
            {
                SuspendedTransactionAnswer customerAnswer = new SuspendedTransactionAnswer();
                customerAnswer.CustomerID = customerID;
                customerAnswer.CustomerName = customerName;                
                PopulateAnswer(customerAnswer, fieldOrder, addInfo);
                answerList.Add(customerAnswer);
            }
        }

        /// <summary>
        /// Retrieves information for the suspension from an infocode
        /// </summary>
        /// <param name="entry">The entry into the database</param>
		/// <param name="session">The current session within the POS</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="addInfo"></param>
        /// <param name="answerList">A list of answers on the suspension</param>
        /// <param name="fieldOrder">The order that this answer is on the suspension</param>
        protected virtual void GetInfocodeInfo(IConnectionManager entry, ISession session, IRetailTransaction retailTransaction, SuspensionTypeAdditionalInfo addInfo, List<SuspendedTransactionAnswer> answerList, int fieldOrder)
        {            
            if (Interfaces.Services.InfocodesService(entry).ProcessCentralSuspensionInfocode(entry,session, retailTransaction, (string)addInfo.InfoTypeSelectionID, addInfo.Required, addInfo.Text))
            {
                if (retailTransaction.IInfocodeLines.Count() > 0)
                {
                    InfoCodeLineItem infocode = retailTransaction.IInfocodeLines.LastOrDefault();
                    //Make sure that the last infocode is the one we were processing if not then the user didn't select any information
                    if (infocode != null && infocode.InfocodeId == (string)addInfo.InfoTypeSelectionID)
                    {
                        SuspendedTransactionAnswer infocodeAnswer = new SuspendedTransactionAnswer();
                        infocodeAnswer.RecordID = Guid.NewGuid();

                        #region Get infocode information for printing
                        string infoComment = "";
                        if (infocode.PrintPromptOnReceipt || infocode.PrintInputOnReceipt || infocode.PrintInputNameOnReceipt)
                        {
                            if (infocode.PrintPromptOnReceipt && infocode.Prompt != null && infocode.Prompt != "")
                            {
                                infoComment = infocode.Prompt;
                            }
                            if (infocode.PrintInputOnReceipt && infocode.Subcode != null && infocode.Subcode != "")
                            {
                                if (infoComment != "")
                                    infoComment += " - " + infocode.Subcode;
                                else
                                    infoComment = infocode.Subcode;
                            }
                            
                            if (infocode.PrintInputOnReceipt && infocode.Subcode == null && infocode.Information2 != "")
                            {
                                if (infoComment != "")
                                    infoComment += " - " + infocode.Information2;
                                else
                                    infoComment = infocode.Information2;

                            }
                            if (infocode.PrintInputNameOnReceipt && infocode.Information != null && infocode.Information != "")
                            {
                                if (infoComment != "")
                                    infoComment += " - " + infocode.Information;
                                else
                                    infoComment = infocode.Information;
                            }
                        }
                        else
                        {
                            infoComment = infocode.Information;
                        }

                        #endregion

                        string string1 = infocode.InfocodeId + " " + infocode.Information;

                        infocodeAnswer.InfoCodeResult = string1;
                        //infocodeAnswer.InfoCodeResult = infoComment;
                        infocodeAnswer.InfoCodeTypeSelection = infocode.InfocodeId;
                        
                        PopulateAnswer(infocodeAnswer, fieldOrder, addInfo);
                        answerList.Add(infocodeAnswer);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves text/description information for the suspension
        /// </summary>
        /// <param name="addInfo">The information created in this answer</param>
        /// <param name="answerList">The list of answers on the suspension</param>
        /// <param name="fieldOrder">The order that this answer is on the suspension</param>
        protected virtual void GetTextInfo(SuspensionTypeAdditionalInfo addInfo, List<SuspendedTransactionAnswer> answerList, int fieldOrder)
        {
            using (TextDialog dlg = new TextDialog(addInfo))
            {
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    SuspendedTransactionAnswer answer = dlg.GetAnswer();

                    PopulateAnswer(answer, fieldOrder, addInfo);
                    answerList.Add(answer);
                }
            }
        }

        /// <summary>
        /// Retrieves information about a name for the suspension
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="addInfo">the information that was created</param>
        /// <param name="answerList">List of answers already created</param>
        /// <param name="fieldOrder">The order that this answer is on the suspension</param>
        protected virtual void GetNameInfo(IConnectionManager entry, SuspensionTypeAdditionalInfo addInfo, List<SuspendedTransactionAnswer> answerList, int fieldOrder)
        {
            using (NameDialog dlg = new NameDialog(entry, addInfo))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    SuspendedTransactionAnswer answer = dlg.GetAnswer();
                    PopulateAnswer(answer, fieldOrder, addInfo);
                    answerList.Add(answer);
                }
            }
        }

        /// <summary>
        /// Get address information for suspension
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="addInfo">Configuration for the additional information</param>
        /// <param name="answerList">The list of answers for this suspension</param>
        /// <param name="fieldOrder">The order that this answer is on the suspension</param>
        protected virtual void GetAddressInfo(IConnectionManager entry, SuspensionTypeAdditionalInfo addInfo, List<SuspendedTransactionAnswer> answerList, int fieldOrder)
        {
            using (AddressDialog dlg = new AddressDialog(entry, addInfo))
            {
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    SuspendedTransactionAnswer answer = dlg.GetAnswer();
                    PopulateAnswer(answer, fieldOrder, addInfo);
                    answerList.Add(answer);
                }
            }
        }

        /// <summary>
        /// Suspends the transaction in the local database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transactionTypeID">The type of suspension that is being done</param>
        /// <param name="xmlTransaction">The current transaction as an XML</param>
        /// <param name="transaction">The current transaction</param>
        /// <param name="answers">The answers that have been created</param>
        /// <param name="suspendedId">The ID for the suspension</param>
        /// <returns></returns>
        protected virtual RecordIdentifier SuspendTransactionLocally(
            IConnectionManager entry, 
            RecordIdentifier transactionTypeID, 
            string xmlTransaction,
            IRetailTransaction transaction, 
            List<SuspendedTransactionAnswer> answers, RecordIdentifier suspendedId)
        {
            try
            {
                SuspendedTransactionType suspendedTransactionType =
                    Providers.SuspendedTransactionTypeData.Get(entry, transactionTypeID);

                SuspendedTransaction transactionToSuspend = new SuspendedTransaction();
                transactionToSuspend.AllowStatementPosting = suspendedTransactionType.EndofDayCode;
                transactionToSuspend.Text = suspendedTransactionType.Text;
                transactionToSuspend.StaffID = entry.CurrentStaffID == RecordIdentifier.Empty ? "" : (string)entry.CurrentStaffID;
                transactionToSuspend.StoreID = entry.CurrentStoreID;
                transactionToSuspend.TerminalID = entry.CurrentTerminalID;
                transactionToSuspend.SuspensionTypeID = suspendedTransactionType.ID;
                transactionToSuspend.TransactionDate = DateTime.Now;
                transactionToSuspend.TransactionXML = xmlTransaction;                
                transactionToSuspend.Balance = transaction.NetAmount;
                transactionToSuspend.BalanceWithTax = transaction.NetAmountWithTax;
                transactionToSuspend.IsLocallySuspended = true;
                transactionToSuspend.ID = suspendedId;

                Providers.SuspendedTransactionData.Save(entry, transactionToSuspend);

                //If answers were entered then save them
                if (answers != null)
                {
                    foreach (var answer in answers)
                    {
                        answer.TransactionID = transactionToSuspend.ID;
                        Providers.SuspendedTransactionAnswerData.Save(entry, answer);
                    }
                }

                return transactionToSuspend.ID;
            }
            catch (Exception e)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException("Central Suspension.SuspendTransactionLocally", e);
                throw e;
            }
        }

        /// <summary>
        /// Checks if there are any items to suspend.
        /// If there are no items to suspend the function displays a message dialog containing such a message and returns false
        /// If there are items to suspend the function returns true.
        /// </summary>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="msgHandler">The handler that will display the message to the user</param>
        /// <returns></returns>
        protected virtual bool AnyItemsToSuspend(IRetailTransaction retailTransaction, ShowMessageHandler msgHandler)
        {
            if (retailTransaction.ISaleItems.Count(c => c.Voided == false) == 0)
            {
                msgHandler?.Invoke(Properties.Resources.NoItemsToSuspend);
                return false;
            }

            return true;
        }

        #endregion

        #region Recall Transaction

        /// <summary>
        /// Retrieves a list of user input for a specific suspended transaction
        /// </summary>
        /// <param name="settings">The current settings</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transactionID">The unique ID of the suspended transaction</param>
        /// <param name="suspendedTransaction">The transaction</param>
        /// <returns>
        /// An list of <see cref="SuspendedTransactionAnswer"/> with information about the user input done when transaction was
        /// suspende
        /// </returns>
        public virtual List<SuspendedTransactionAnswer> GetSuspendedTransactionAnswers(IConnectionManager entry,ISettings settings, RecordIdentifier transactionID, SuspendedTransaction suspendedTransaction)
        {
            List<SuspendedTransactionAnswer> answers = null;

            if (settings.SiteServiceProfile.UseCentralSuspensions && !suspendedTransaction.IsLocallySuspended)
            {
                // We search for answers on the server and if we get nothing then we search locally
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                if (service.TestConnection(entry, entry.SiteServiceAddress, entry.SiteServicePortNumber) == ConnectionEnum.Success)
                {
                    answers = service.GetSuspendedTransactionAnswers(entry, settings.SiteServiceProfile, transactionID, true);

                    if (answers == null || answers.Count == 0)
                    {
                        answers = null;
                    }
                }
            }

            if (answers == null)
            {
                answers = Providers.SuspendedTransactionAnswerData.GetList(entry, transactionID);
            }

            return answers;
        }

        /// <summary>
        /// Recalls the transaction from a central database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <param name="POSTransaction">The current retail transaction</param>
        /// <param name="transactionTypeID">The type of suspension the transaction should be suspended as</param>
        /// <param name="mainFormInfo">Information about the main form size, hight and etc.</param>
        public virtual void RecallTransaction(IConnectionManager entry, ISettings settings, IPosTransaction POSTransaction, RecordIdentifier transactionTypeID, MainFormInfo mainFormInfo)
        {
            SuspendedTransaction selectedTransaction = RecallTransaction(entry, settings, (IRetailTransaction)POSTransaction, transactionTypeID, Interfaces.Services.DialogService(entry).ShowMessage, mainFormInfo);

            if (selectedTransaction != null)
            {
                var recalledTransaction = new RetailTransaction((string)entry.CurrentStoreID, settings.Store.Currency, settings.TaxIncludedInPrice);
                recalledTransaction = UpdateRecalledTransaction(entry, settings, selectedTransaction, recalledTransaction);

                MergeTransactionToMainTransaction(entry, recalledTransaction, (PosTransaction)POSTransaction);

                Interfaces.Services.CalculationService(entry).CalculateTotals(entry, POSTransaction);
            }
        }

        /// <summary>
        /// After the transaction has been selected the store and terminal information are
        /// updated as well as begin/end dates on the transaction, items and tenders.
        /// </summary>
        /// <param name="entry">The entry itno the database</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <param name="retailTransaction">Selected transaction</param>
        /// <param name="SelectedTransaction">Information about the suspended
        /// transaction</param>
        /// <returns>
        /// Updated transaction
        /// </returns>
        public virtual IRetailTransaction UpdateRecalledTransaction(IConnectionManager entry, ISettings settings, IRetailTransaction retailTransaction, SuspendedTransaction SelectedTransaction)
        {
            if (retailTransaction != null)
            {
                // Change the transaction status to normal
                retailTransaction.EntryStatus = TransactionStatus.Normal;

                // Change the terminal id to the one that's retrieving the transaction
                retailTransaction.StoreId = entry.CurrentStoreID.ToString();
                retailTransaction.TerminalId = entry.CurrentTerminalID.ToString();

                // Change the created-date and clear the end-date
                retailTransaction.BeginDateTime = DateTime.Now;
                retailTransaction.EndDateTime = DateTime.MinValue;

                //Give all the sales items a new date and time
                foreach (ISaleLineItem item in retailTransaction.ISaleItems)
                {
                    item.BeginDateTime = retailTransaction.BeginDateTime;
                }

                //Give all the tender items a new date and time
                foreach (ITenderLineItem tender in retailTransaction.ITenderLines)
                {
                    tender.BeginDateTime = retailTransaction.BeginDateTime;
                }                

                //The transaction has been updated and will be used - mark it as recalled in the appropriate location
                bool useLocalFunctionality = true;

                if (settings.SiteServiceProfile.UseCentralSuspensions && !SelectedTransaction.IsLocallySuspended)
                {
                    useLocalFunctionality = false;
                    try
                    {
                        ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                        service.DeleteSuspendedTransaction(entry, settings.SiteServiceProfile, SelectedTransaction.ID, true);                        
                    }
                    catch
                    {
                        useLocalFunctionality = true;
                    }
                }
                
                if (useLocalFunctionality)
                {
                    List<SuspendedTransactionAnswer> answers = Providers.SuspendedTransactionAnswerData.GetList(entry, SelectedTransaction.ID);
                    Providers.SuspendedTransactionData.Delete(entry, SelectedTransaction.ID);

                    foreach (SuspendedTransactionAnswer answer in answers)
                    {
                        Providers.SuspendedTransactionAnswerData.Delete(entry, answer.ID);
                    }
                }
            }
            return retailTransaction;
        }

        /// <summary>
        /// Sums up the total number of locally suspended transactions and transactions suspended over the Store Server (if central suspension is in use)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <param name="msgHandler">Shows messages to the user</param>
        /// <param name="transactionTypeID">Suspension type ID</param>
        /// <param name="service">The Store Server service</param>
        /// <returns></returns>
        protected virtual bool AnyTransactionsToRecall(IConnectionManager entry, ISettings settings, ShowMessageHandler msgHandler, RecordIdentifier transactionTypeID, ISiteServiceService service)
        {
            int transCount = 0;

            if (settings.SiteServiceProfile.UseCentralSuspensions)
            {                
                try
                {
                    transCount = service.GetSuspendedTransCount(entry, settings.SiteServiceProfile, entry.CurrentStoreID, RecordIdentifier.Empty, transactionTypeID, RetrieveSuspendedTransactions.OnlyCentrallySaved, false);
                }
                catch
                {
                    msgHandler(Properties.Resources.CentralFunctionalityNotWorkingRecallLocally);
                }
                finally
                {
                    //LSOne.Services.Interfaces.Services.DialogService(entry).CloseStatusDialog();
                }
            }

            transCount += Providers.SuspendedTransactionData.GetCount(entry, entry.CurrentStoreID, RecordIdentifier.Empty, transactionTypeID, RetrieveSuspendedTransactions.All);

            if (transCount == 0)
            {
                msgHandler(Properties.Resources.NoSuspendedTransactionsToRetrieve);
            }

            return transCount > 0;
        }

        /// <summary>
        /// Returns a list of suspended transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current settings</param>
        /// <param name="transactionTypeID">The type of suspension that is being retrieved</param>
        /// <param name="service">The site service</param>
        /// <returns></returns>
        protected virtual List<SuspendedTransaction> GetSuspendedTransactionList(IConnectionManager entry, ISettings settings, RecordIdentifier transactionTypeID, ISiteServiceService service)
        {
            bool useLocalFunctionality = true;
            List<SuspendedTransaction> suspendedTransactions = new List<SuspendedTransaction>();

            if (settings.SiteServiceProfile.UseCentralSuspensions)
            {                
                useLocalFunctionality = false;

                try
                {
                    suspendedTransactions = service.GetSuspendedTransactionListForStore(entry, settings.SiteServiceProfile, transactionTypeID, entry.CurrentStoreID, true);

                    // Get all local transactions                    
                    List<SuspendedTransaction> localTransactions = Providers.SuspendedTransactionData.GetListForTypeAndStore(entry,
                                                                                                                   transactionTypeID,
                                                                                                                   entry.CurrentStoreID,
                                                                                                                   Date.Empty,
                                                                                                                   Date.Empty,
                                                                                                                   SuspendedTransaction.SortEnum.TransactionID,
                                                                                                                   false);
                    
                    // Remove all transactions from the local list that are the same as the server ones. This case can come up where you have the store server connecting to the
                    // same database that the POS is running on.
                    foreach (SuspendedTransaction trans in suspendedTransactions)
                    {
                        for (int i = 0; i < localTransactions.Count; i++)
                        {
                            if (trans.ID == localTransactions[i].ID || trans.SuspensionTypeID != transactionTypeID)
                            {
                                localTransactions.RemoveAt(i);
                                break;
                            }
                        }
                    }

                    // Merge the lists
                    if (localTransactions.Count > 0)
                    {
                        foreach (SuspendedTransaction transaction in localTransactions)
                        {
                            transaction.IsLocallySuspended = true;
                        }

                        suspendedTransactions.AddRange(localTransactions);

                        suspendedTransactions = (from transactions in suspendedTransactions
                                                 orderby transactions.TransactionDate descending
                                                 select transactions).ToList();
                    }

                }
                catch
                {                    
                    useLocalFunctionality = true;
                }
                finally
                {
                    //LSOne.Services.Interfaces.Services.DialogService(entry).CloseStatusDialog();
                }
            }
            
            if(useLocalFunctionality)
            {                
                suspendedTransactions = Providers.SuspendedTransactionData.GetListForTypeAndStore(entry,transactionTypeID,entry.CurrentStoreID, Date.Empty, Date.Empty, SuspendedTransaction.SortEnum.TransactionDate,true,entry.CurrentTerminalID);
                foreach (SuspendedTransaction transaction in suspendedTransactions)
                {
                    transaction.IsLocallySuspended = true;
                }
            }

            return suspendedTransactions;
        }

        /// <summary>
        /// Gets all suspended transaction answers, both from the store server and from the local database
        /// </summary>
        /// <param name="settings">The current settings</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="suspensionTypeID">The suspension type ID to filter by</param>
        /// <param name="service">the Site service</param>
        /// <returns></returns>
        protected virtual List<SuspendedTransactionAnswer> GetSuspendedTransactionAnswersByType(IConnectionManager entry, ISettings settings,RecordIdentifier suspensionTypeID, ISiteServiceService service)
        {            
            List<SuspendedTransactionAnswer> allAnswers = new List<SuspendedTransactionAnswer>();
            bool useLocalFunctionality = true;

            if (settings.SiteServiceProfile.UseCentralSuspensions)
            {
                useLocalFunctionality = false;

                try
                {
                    allAnswers = service.GetSuspendedTransactionAnswersByType(entry, settings.SiteServiceProfile, suspensionTypeID, true);

                    // Look through
                    List<SuspendedTransactionAnswer> localAnswers = Providers.SuspendedTransactionAnswerData.GetListForSuspensionType(entry, suspensionTypeID);

                    // Remove the same answers from the local list, this can happen if the store server is connected to the same database that the POS is 
                    // connected to.
                    foreach (SuspendedTransactionAnswer answer in allAnswers)
                    {
                        for (int i = 0; i < localAnswers.Count; i++)
                        {
                            if (localAnswers[i].ID == answer.ID)
                            {
                                localAnswers.RemoveAt(i);
                                break;
                            }
                        }
                    }

                    if (localAnswers.Count > 0)
                    {
                        allAnswers.AddRange(localAnswers);
                    }

                }
                catch
                {                    
                    useLocalFunctionality = true;
                }
                finally
                {                    
                    //LSOne.Services.Interfaces.Services.DialogService(entry).CloseStatusDialog();
                }
            }

            if (useLocalFunctionality)
            {
                allAnswers = Providers.SuspendedTransactionAnswerData.GetListForSuspensionType(entry, suspensionTypeID);
            }

            return allAnswers;
        }

        #endregion              

        /// <summary>
        /// Creates the salelines from a serialized transaction.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transactionXml">The serialized transaction data</param>
        /// <param name="transaction">The RetailTransaction to assign to the SaleLineItem objects.</param>
        /// <returns></returns>
        internal static LinkedList<ISaleLineItem> CreateSaleLineItems(IConnectionManager entry, string transactionXml, RetailTransaction transaction)
        {
            XDocument xDoc;
            try
            {
                string storeCurrency = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.Currency;
                xDoc = XDocument.Parse(transactionXml);                
                LinkedList<ISaleLineItem> SaleLines = new LinkedList<ISaleLineItem>();

                foreach (XElement transElem in xDoc.Root.Elements())
                {
                    if (!transElem.IsEmpty)
                    {
                        if (transElem.Name.ToString() == "SaleItems")
                        {
                            if (transElem.HasElements)
                            {
                                IEnumerable<XElement> xSaleItems = transElem.Elements();
                                foreach (XElement xSaleItem in xSaleItems)
                                {
                                    if (xSaleItem.HasElements)
                                    {
                                        SaleLineItem sli = new SaleLineItem(transaction);
                                        switch (xSaleItem.Name.ToString())
                                        {
                                            case "SaleLineItem":
                                                break;
                                            case "DiscountVoucherItem":
                                                sli = new DiscountVoucherItem(transaction, "", "", 0);
                                                SaleLines.Last.Value.DiscountVoucher = (DiscountVoucherItem)sli;                                                
                                                break;
                                            case "FuelSalesLineItem":                                                
                                                sli = new FuelSalesLineItem(new BarCode(), transaction);
                                                break;
                                            case "GiftCertificateItem":
                                                sli = new GiftCertificateItem(transaction);
                                                break;
                                            case "IncomeExpenseItem":
                                                sli = new IncomeExpenseItem(transaction);
                                                break;
                                            case "SalesInvoiceLineItem":
                                                sli = new SalesInvoiceLineItem(transaction);
                                                break;
                                            case "SalesOrderLineItem":
                                                sli = new SalesOrderLineItem(transaction);
                                                break;
                                            case "PharmacySalesLineItem":
                                                sli = new PharmacySalesLineItem(transaction);
                                                break;
                                        }
                                        sli.ToClass(xSaleItem);
                                        SaleLines.AddLast(sli);
                                    }
                                }
                            }
                        }
                    }
                }

                return SaleLines;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creates the payment lines from a serialized transaction.
        /// </summary>
        /// <param name="transactionXml">The serialized transaction data</param>
        /// <param name="transaction">The RetailTransaction to assign to the TenderLineItem objects</param>
        /// <returns></returns>
        internal static LinkedList<TenderLineItem> CreatePaymentLines(string transactionXml, RetailTransaction transaction)
        {            
            XDocument xDoc;
            try
            {
                xDoc = XDocument.Parse(transactionXml);
                LinkedList<TenderLineItem> paymentLines = new LinkedList<TenderLineItem>();

                foreach (XElement transElem in xDoc.Root.Elements())
                {
                    if (!transElem.IsEmpty)
                    {
                        if (transElem.Name.ToString().Trim() == "TenderLines")
                        {
                            if (transElem.HasElements)
                            {
                                IEnumerable<XElement> xTenderItems = transElem.Elements();
                                foreach (XElement xTender in xTenderItems)
                                {
                                    if (xTender.HasElements)
                                    {
                                        TenderLineItem tli = new TenderLineItem();

                                        switch (xTender.Name.ToString())
                                        {
                                            case "TenderLineItem":
                                                break;
                                            case "CardTenderLineItem":
                                                tli = new CardTenderLineItem();
                                                break;
                                            case "ChequeTenderLineItem":
                                                tli = new ChequeTenderLineItem();
                                                break;
                                            case "CorporateCardTenderLineItem":
                                                tli = new CorporateCardTenderLineItem();
                                                break;
                                            case "CouponTenderLineItem":
                                                tli = new CouponTenderLineItem();
                                                break;
                                            case "CreditMemoTenderLineItem":
                                                tli = new CreditMemoTenderLineItem();
                                                break;
                                            case "CustomerTenderLineItem":
                                                tli = new CustomerTenderLineItem();
                                                break;
                                            case "GiftCertificateTenderLineItem":
                                                tli = new GiftCertificateTenderLineItem();
                                                break;
                                            case "LoyaltyTenderLineItem":
                                                tli = new LoyaltyTenderLineItem();
                                                break;
                                            case "TradeInTenderLineItem":
                                                tli = new TradeInTenderLineItem();
                                                break;
                                        }
                                        tli.ToClass(xTender);
                                        tli.Transaction = transaction;
                                        paymentLines.AddLast(tli);
                                    }
                                }
                            }
                        }
                    }
                }

                return paymentLines;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Makes sure that a recall transaction operation is allowed, displays a dialog with all available suspended transactions for the user to choose from.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <param name="retailTransaction">The current retail transaction</param>
        /// <param name="transactionTypeID">The type of suspension the transaction should be
        /// suspended as</param>
        /// <param name="msgHandler">A message function that the service uses to display any
        /// messages to the user</param>
        /// <param name="mainFormInfo">Information about the main form size, hight and
        /// etc.</param>
        /// <returns>
        /// An instance of <see cref="SuspendedTransaction"/> with information about the transaction being recalled
        /// </returns>
        internal SuspendedTransaction RecallTransaction(IConnectionManager entry, ISettings settings, IRetailTransaction retailTransaction, RecordIdentifier transactionTypeID,
            ShowMessageHandler msgHandler, MainFormInfo mainFormInfo)
        {
            SuspendedTransaction selectedTransaction = null;
            ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
            service.TerminalID = entry.CurrentTerminalID;

            if (retailTransaction.NoOfPaymentLines > 0 && retailTransaction.ITenderLines.Count(c => c.Voided == false) > 0)
            {
                msgHandler(Properties.Resources.ConcludeTheCurrentTransaction);
                return selectedTransaction;
            }

            try
            {
                Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.GettingSuspendedTransactions);
                if (!AnyTransactionsToRecall(entry, settings, msgHandler, transactionTypeID, service))
                {
                    return selectedTransaction;
                }

                VisualProfile visualProfile = Providers.VisualProfileData.GetTerminalProfile(entry, entry.CurrentTerminalID, entry.CurrentStoreID);

                // View a list of suspended transactons
                if (visualProfile.TerminalType == 0) //TODO: Use enum from SC when it's been added
                {
                    bool storeServerConnected = false;

                    if (settings.SiteServiceProfile.UseCentralSuspensions)
                    {
                        // Test the connection to see if frmSuspendedTrans should show the "transfer" option or not
                        ConnectionEnum result = service.TestConnection(entry, entry.SiteServiceAddress, entry.SiteServicePortNumber);
                        storeServerConnected = result == ConnectionEnum.Success;
                    }

                    List<SuspendedTransaction> suspendedList = GetSuspendedTransactionList(entry, settings, transactionTypeID, service);
                    if (suspendedList.Count == 0)
                    {
                        Interfaces.Services.DialogService(entry).CloseStatusDialog();
                        msgHandler(Properties.Resources.NoSuspendedTransactionsToRetrieve);
                    }
                    else
                    {
                        List<SuspendedTransactionAnswer> answers = GetSuspendedTransactionAnswersByType(entry, settings, suspendedList[0].SuspensionTypeID, service);
                        SuspendedTransactionsDialog transDialog = new SuspendedTransactionsDialog(entry, settings, suspendedList, answers, storeServerConnected);
                        Interfaces.Services.DialogService(entry).CloseStatusDialog();
                        transDialog.ShowDialog();

                        selectedTransaction = transDialog.SelectedSuspendedTransaction;
                        transDialog.Dispose();
                    }
                }
                else
                {
                    //Display Dynakey dialog
                }
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                Interfaces.Services.DialogService(entry).CloseStatusDialog();
            }
            return selectedTransaction;

            //Note -> function UpdateRecalledTransaction is called directly after this function has finished.
        }

        private void MergeTransactionToMainTransaction(IConnectionManager entry, RetailTransaction mergingTransaction, PosTransaction POSTransaction)
        {
            if ((string)mergingTransaction.Customer.ID != "" && (string)((RetailTransaction)POSTransaction).Customer.ID != "" &&
                ((RetailTransaction)POSTransaction).Customer.ID != mergingTransaction.Customer.ID)
            {
                var dlgResult = Interfaces.Services.DialogService(entry).ShowOverrideCustomer(entry, mergingTransaction.Customer.CopyName(), ((RetailTransaction)POSTransaction).Customer.CopyName());
                if (dlgResult == DialogResult.Yes)
                {
                    foreach (var saleItem in mergingTransaction.SaleItems)
                    {
                        ((RetailTransaction)POSTransaction).Add(saleItem);
                    }
                    ((RetailTransaction)POSTransaction).Add(mergingTransaction.Customer);
                    //needs to be done twise because the price is not updated for the customer until the second time
                    Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, POSTransaction, true);
                    Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, POSTransaction, true);
                    Interfaces.Services.CalculationService(entry).CalculateTotals(entry, POSTransaction);
                    ((RetailTransaction)POSTransaction).LoyaltyItem = mergingTransaction.LoyaltyItem;
                }
                else
                {
                    foreach (var saleItem in mergingTransaction.SaleItems)
                    {
                        ((RetailTransaction)POSTransaction).Add(saleItem);
                    }
                    Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, POSTransaction, true);
                    Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, POSTransaction, true);
                    Interfaces.Services.CalculationService(entry).CalculateTotals(entry, POSTransaction);
                }
            }
            else
            {
                foreach (var saleItem in mergingTransaction.SaleItems)
                {
                    ((RetailTransaction)POSTransaction).Add(saleItem);
                }
                ((RetailTransaction)POSTransaction).Add(mergingTransaction.Customer);
                Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, POSTransaction, true);
                Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, POSTransaction, true);
                Interfaces.Services.CalculationService(entry).CalculateTotals(entry, POSTransaction);
                ((RetailTransaction)POSTransaction).LoyaltyItem = mergingTransaction.LoyaltyItem;
            }

            if (((RetailTransaction)POSTransaction).TotalManualDiscountAmount == 0)
            {
                ((RetailTransaction)POSTransaction).SetTotalDiscAmount(mergingTransaction.TotalManualDiscountAmount);
            }

            if (((RetailTransaction)POSTransaction).TotalManualPctDiscount == 0)
            {
                ((RetailTransaction)POSTransaction).SetTotalDiscPercent(mergingTransaction.TotalManualPctDiscount);
            }

            foreach (var recalledManualPeriodicDiscount in mergingTransaction.ManualPeriodicDiscounts)
            {
                ((RetailTransaction)POSTransaction).ManualPeriodicDiscounts.Add(recalledManualPeriodicDiscount);
            }

            ((RetailTransaction)POSTransaction).CalculateTotalDiscount =
                mergingTransaction.CalculateTotalDiscount || ((RetailTransaction)POSTransaction).CalculateTotalDiscount;

            mergingTransaction.InfoCodeLines.ForEach(((RetailTransaction)POSTransaction).Add);
            POSTransaction.HTMLInformation = mergingTransaction.HTMLInformation;
            ((RetailTransaction)POSTransaction).SaleIsReturnSale = mergingTransaction.SaleIsReturnSale;
            ((RetailTransaction)POSTransaction).SalesPerson = (Employee)mergingTransaction.SalesPerson.Clone();
        }

        private RetailTransaction UpdateRecalledTransaction(IConnectionManager entry, ISettings settings, SuspendedTransaction selectedTransaction, RetailTransaction newTransaction)
        {
            newTransaction = (RetailTransaction)PosTransaction.CreateTransFromXML(selectedTransaction.TransactionXML, newTransaction, null);

            newTransaction.SuspendTransactionAnswers = GetSuspendedTransactionAnswers(entry, settings, selectedTransaction.ID, selectedTransaction);

            return (RetailTransaction)UpdateRecalledTransaction(entry, settings, newTransaction, selectedTransaction);
        }
    }
}