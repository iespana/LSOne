using System;
using System.Linq;
using LSOne.Controls.Enums;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.POS.Processes.Common;
using LSOne.Services.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSRetailPosis;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;

namespace LSOne.Services.SplitBill
{
    public class RunSplitBill
    {
        public SplitInfo SplitBillInfo { get; private set; }

        public HospitalityOperationResult Execute(IConnectionManager entry, DiningTable selectedTable, HospitalityType activeHospitalityType, bool forceHospitalityExit = true)
        {
            try
            {
                // This operation is only valid for a retail transaction
                if (!(selectedTable.Transaction is RetailTransaction))
                {
                    POSFormsManager.ShowPOSMessageDialog(Properties.Resources.OperationOnlyValidForRetailTransaction);  // This operation is only valid for a retail transaction
                    return HospitalityOperationResult.Cancel;
                }

                if (((RetailTransaction)selectedTable.Transaction).SaleItems.Count == 0 && !((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FinalizeSplitBill)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ThereAreNoItemsToSplit);
                    return HospitalityOperationResult.Cancel;
                }

                if (((RetailTransaction)selectedTable.Transaction).SaleItems.Count == 0 && ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FinalizeSplitBill)
                {
                    RecreateSplitTransaction(entry, -1, selectedTable.Transaction);
                }

                if (((RetailTransaction) selectedTable.Transaction).SaleItems.Count(c => c.Voided == false) == 0)
                {
                    return HospitalityOperationResult.Cancel;
                }

                if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).SplitBillID.StringValue == "")
                {
                    ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).SplitBillID = Guid.NewGuid();
                }

                ((RetailTransaction)selectedTable.Transaction).SplitID = (Guid)((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).SplitBillID;
                HospitalityResult result = SplitBill(entry, (RetailTransaction)selectedTable.Transaction, activeHospitalityType, selectedTable);

                switch (result.OperationResult)
                {
                    case HospitalityOperationResult.Cancel:
                        ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).ForceHospitalityExit = forceHospitalityExit;                        
                        break;                    
                    case HospitalityOperationResult.Pay:
                        PayGuest(entry, selectedTable.Transaction, result.Guest);
                        break;
                    case HospitalityOperationResult.PrintAll:
                        break;
                    case HospitalityOperationResult.PrintSplit:
                        break;
                    case HospitalityOperationResult.SaveAndExit:
                        //Nothing needs to be done - at least one additional transaction has been created using items from the org transaction
                        break;
                }

                return result.OperationResult;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private HospitalityResult SplitBill(IConnectionManager entry, RetailTransaction retailTransaction, HospitalityType activeHospitalityType, DiningTable selectedTable)
        {
            var operationResult = new HospitalityResult();

            if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).VisualProfile.TerminalType == VisualProfile.HardwareTypes.Touch)
            {
                using (var frmSplit = new SplitBillDialog(entry, 1M, retailTransaction, SplitAction.SplitBill, (string)activeHospitalityType.SplitBillLookupID, activeHospitalityType, selectedTable))
                {
                    POSFormsManager.ShowPOSForm(frmSplit);
                    operationResult = frmSplit.OperationResult;
                    SplitBillInfo = frmSplit.SplitBillInfo;
                }
            }
            return operationResult;
        }
        
        private void PayGuest(IConnectionManager entry, IPosTransaction posTransaction, int guest)
        {
            try
            {
                if (posTransaction.TransactionId == "")
                {
                    return;
                }

                if (guest != 0 && guest != 2)
                { 
                    throw new Exception("SplitBill.PayGuest: Current functionality only supports guest id 0 and 2");
                }

                RecreateSplitTransaction(entry, guest, posTransaction);
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        private void RecreateSplitTransaction(IConnectionManager entry, int guest, IPosTransaction posTransaction)
        {
            try
            {
                RecordIdentifier transactionID;
                if (guest < 0)
                {
                    var splitTrans = TransactionProviders.HospitalityTransactionData.GetLastSplitTransaction(
                        entry,
                        posTransaction.StoreId,
                        posTransaction.TerminalId,
                        ((RetailTransaction)posTransaction).Hospitality.TableInformation.TableID,
                        guest,
                        ((RetailTransaction)posTransaction).Hospitality.ActiveHospitalitySalesType,
                        ((RetailTransaction)posTransaction).SplitID);
                    transactionID = splitTrans == null ? posTransaction.TransactionId : splitTrans.TransactionID;
                }
                else
                {
                    transactionID = posTransaction.TransactionId;
                }

                var hospTransactions = TransactionProviders.HospitalityTransactionData.GetList(entry, CombinedHospTransID(posTransaction, transactionID, guest));

                if (hospTransactions != null && hospTransactions.Count > 0)
                {
                    var transaction = hospTransactions.FirstOrDefault();
                    if (transaction != null && transaction.ID != RecordIdentifier.Empty)
                    {
                        if (transaction.Transaction.SaleItems.Count > 0)
                        {
                            ((RetailTransaction)posTransaction).SplitTransaction = true;
                            ((RetailTransaction)posTransaction).SaleItems.Clear();

                            foreach (var sli in transaction.Transaction.SaleItems)
                            {
                                var cloneItem = (SaleLineItem)sli.Clone();
                                ((RetailTransaction)posTransaction).Add(cloneItem);
                            }

                            Services.Interfaces.Services.CalculationService(entry).CalculateTotals(entry, posTransaction);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a Hospitality Transaction ID which is needed for most functions in TransactionProviders.HospitalityTransactionData.
        /// Creates an ID which has the combination (in this order): TransactionID, StoreID, TerminalID, TableID, Guest, HospitalityType
        /// </summary>
        /// <param name="posTransaction">The StoreID and TerminalID are retrieved from this parameter</param>
        /// <param name="transactionID">The transaction ID used for the combination</param>
        /// <param name="guest">The guest used for the combination</param>
        /// <returns>A combines Hospitality Transaction ID</returns>
        public static RecordIdentifier CombinedHospTransID(IPosTransaction posTransaction, RecordIdentifier transactionID, RecordIdentifier guest)
        {
            var transaction = new HospitalityTransaction();
            transaction.SetFromRetailTransaction((RetailTransaction)posTransaction);
            transaction.Guest = guest;
            transaction.TransactionID = transactionID;

            return transaction.ID;
        }

        public static void UpdateTransactions(IConnectionManager entry, SplitAction action, SplitInfo splitBillInfo)
        {
            splitBillInfo.TableTransaction.SaleItems.Clear();

            foreach (var sli in splitBillInfo.TmpPosTransLine)
            {
                var cloneItem = sli.Clone(splitBillInfo.TableTransaction);
                cloneItem.SplitLineId = cloneItem.LineId;
                if (!((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.AllowItemChangesAfterSplitBill)
                {
                    cloneItem.ExcludedActions = ExcludedActions.ChangeHospitalityMenuType
                                              | ExcludedActions.ChangeItemComment
                                              | ExcludedActions.ChangeUnitOfMeasure
                                              | ExcludedActions.ClearQuantity
                                              | ExcludedActions.SetQuantity
                                              | ExcludedActions.VoidItem;
                }
                splitBillInfo.TableTransaction.Add(cloneItem);
            }

            Services.Interfaces.Services.CalculationService(entry).CalculateTotals(entry, splitBillInfo.TableTransaction);

            splitBillInfo.SplitTransaction.SaleItems.Clear();

            foreach (var sli in splitBillInfo.TmpSplitTransLine)
            {
                var cloneItem = sli.Clone(splitBillInfo.SplitTransaction);
                cloneItem.SplitLineId = cloneItem.LineId;
                if (!((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.AllowItemChangesAfterSplitBill)
                {
                    cloneItem.ExcludedActions = ExcludedActions.ChangeHospitalityMenuType
                                              | ExcludedActions.ChangeItemComment
                                              | ExcludedActions.ChangeUnitOfMeasure
                                              | ExcludedActions.ClearQuantity
                                              | ExcludedActions.SetQuantity
                                              | ExcludedActions.VoidItem;
                }
                if (action == SplitAction.TransferLines)
                {
                    cloneItem.CoverNo = 0;
                }
                splitBillInfo.SplitTransaction.Add(cloneItem);
            }

            Services.Interfaces.Services.CalculationService(entry).CalculateTotals(entry, splitBillInfo.SplitTransaction);
        }

        /// <summary>
        /// Saves the split.
        /// </summary>
        /// <param name="action">The action being done</param>
        /// <param name="splitBillInfo">Info on split bill</param>
        public static void SaveSplit(IConnectionManager entry, SplitAction action, SplitInfo splitBillInfo)
        {
            UpdateTransactions(entry, action, splitBillInfo);
            if (action == SplitAction.SplitBill)
            {
                SaveSplitTransaction(entry, ButtonAppliesTo.Guest, 2, splitBillInfo);
            }
        }

        public static void SaveSplitTransaction(IConnectionManager entry, ButtonAppliesTo appliesTo, int guest, SplitInfo splitBillInfo)
        {
            try
            {
                bool shouldSave = true;

                if (appliesTo == ButtonAppliesTo.Guest)
                {
                    shouldSave = splitBillInfo.ApplicableTransaction(appliesTo).SaleItems.Count > 0 &&
                                 splitBillInfo.ApplicableTransaction(ButtonAppliesTo.Table).SaleItems.Count > 0;
                }
                else if (appliesTo == ButtonAppliesTo.Table)
                {
                    shouldSave = splitBillInfo.ApplicableTransaction(appliesTo).SaleItems.Count > 0 &&
                                 splitBillInfo.ApplicableTransaction(ButtonAppliesTo.Guest).SaleItems.Count > 0;
                }

                if (shouldSave)
                {
                    var transaction = new HospitalityTransaction();
                    transaction.SetFromRetailTransaction(splitBillInfo.ApplicableTransaction(appliesTo));
                    transaction.Guest = guest;
                    TransactionProviders.HospitalityTransactionData.Save(entry, transaction);
                }
                else
                {
                    var transaction = new HospitalityTransaction();
                    transaction.SetFromRetailTransaction(splitBillInfo.ApplicableTransaction(appliesTo));
                    transaction.Guest = guest;
                    TransactionProviders.HospitalityTransactionData.Delete(entry, transaction);
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ex.Message, ex);
                throw;
            }
        }
    }
}
