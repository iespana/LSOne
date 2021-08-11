using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.Enums;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual List<CreditVoucher> SearchCreditVouchers(LogonInfo logonInfo, CreditVoucherFilter filter, out int itemCount)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting, LogLevel.Trace);

                return Providers.CreditVoucherData.Search(dataModel, filter, out itemCount);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            itemCount = 0;
            return new List<CreditVoucher>();
        }

        public virtual CreditVoucher GetCreditVoucher(RecordIdentifier creditvoucherID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(creditvoucherID)}: {creditvoucherID}", LogLevel.Trace);
                return Providers.CreditVoucherData.Get(dataModel, creditvoucherID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return null;
        }

        public virtual List<CreditVoucherLine> GetCreditVoucherLines(RecordIdentifier creditvoucherID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(creditvoucherID)}: {creditvoucherID}", LogLevel.Trace);
                return Providers.CreditVoucherLineData.GetList(dataModel, creditvoucherID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return new List<CreditVoucherLine>();
        }

        public virtual void DeleteCreditVoucher(RecordIdentifier creditvoucherID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(creditvoucherID)}: {creditvoucherID}", LogLevel.Trace);
                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "New user context set", LogLevel.Trace);
                    dataModel.Connection.SetContext(logonInfo.UserID);
                }

                Providers.CreditVoucherData.Delete(dataModel, creditvoucherID);

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "Original user context restored", LogLevel.Trace);
                    dataModel.Connection.RestoreContext();                    
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        public virtual RecordIdentifier IssueCreditVoucher(LogonInfo logonInfo, CreditVoucher voucher, RecordIdentifier transactionId, RecordIdentifier receiptId)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transactionId)}: {transactionId}, {nameof(receiptId)}: {receiptId}", LogLevel.Trace);
                Providers.CreditVoucherData.Save(dataModel, voucher);

                var line = new CreditVoucherLine
                {
                    CreditVoucherID = voucher.ID,
                    Amount = voucher.Balance,
                    Operation = CreditVoucherLine.CreditVoucherLineEnum.Create,
                    UserID = logonInfo.UserID,
                    StoreID = logonInfo.storeId ?? "",
                    TerminalID = logonInfo.terminalId ?? "",
                    StaffID = logonInfo.StaffID,
                    TransactionNumber = transactionId,
                    ReceiptID = receiptId,
                    TransactionDateTime = DateTime.Now
                };

                Providers.CreditVoucherLineData.Save(dataModel, line);
                Utils.Log(this, "Credit voucher line saved", LogLevel.Trace);
                return voucher.ID;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);

                return RecordIdentifier.Empty;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        public virtual CreditVoucherValidationEnum ValidateCreditVoucher(ref decimal amount, RecordIdentifier creditVoucherID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(creditVoucherID)}: {creditVoucherID}", LogLevel.Trace);
                CreditVoucher voucher = Providers.CreditVoucherData.Get(dataModel, creditVoucherID);

                if (voucher == null)
                {
                    Utils.Log(this, "Voucher not found", LogLevel.Trace);
                    amount = 0.0M;
                    return CreditVoucherValidationEnum.ValidationVoucherNotFound;
                }

                Utils.Log(this, "Voucher found", LogLevel.Trace);
                if (voucher.Balance == decimal.Zero)
                {
                    amount = voucher.Balance;
                    return CreditVoucherValidationEnum.ValidationVoucherHasZeroBalance;
                }

                if (voucher.Balance < amount)
                {
                    amount = voucher.Balance;
                    return CreditVoucherValidationEnum.ValidationBalanceToLow;
                }

                amount = voucher.Balance;
                return CreditVoucherValidationEnum.ValidationSuccess;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);

                return CreditVoucherValidationEnum.ValidationUnknownError;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        public virtual CreditVoucherValidationEnum UseCreditVoucher(ref decimal amount, RecordIdentifier creditVoucherID, RecordIdentifier transactionId, RecordIdentifier receiptId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(creditVoucherID)}: {creditVoucherID}, {nameof(transactionId)}: {transactionId}, {nameof(receiptId)}: {receiptId}", LogLevel.Trace);
                var voucher = Providers.CreditVoucherData.Get(dataModel, creditVoucherID);

                if (voucher == null)
                {
                    Utils.Log(this, "Voucher not found", LogLevel.Trace);
                    amount = 0.0M;
                    return CreditVoucherValidationEnum.ValidationVoucherNotFound;
                }

                Utils.Log(this, "Voucher found", LogLevel.Trace);

                if (voucher.Balance < amount)
                {
                    amount = voucher.Balance;
                    return CreditVoucherValidationEnum.ValidationBalanceToLow;
                }

                voucher.Balance -= amount;
                voucher.LastUsedDate = Date.Now;

                Providers.CreditVoucherData.Save(dataModel, voucher);
                Utils.Log(this, "Voucher updated", LogLevel.Trace);

                var line = new CreditVoucherLine
                {
                    Amount = amount,
                    CreditVoucherID = voucher.ID,
                    Operation = CreditVoucherLine.CreditVoucherLineEnum.TakeFromCreditVoucher,
                    StoreID = logonInfo.storeId ?? "",
                    TerminalID = logonInfo.terminalId ?? "",
                    UserID = logonInfo.UserID,
                    StaffID = logonInfo.StaffID,
                    ReceiptID = receiptId,
                    TransactionNumber = transactionId,
                    TransactionDateTime = DateTime.Now
                };

                Providers.CreditVoucherLineData.Save(dataModel, line);
                Utils.Log(this, "Voucher line saved", LogLevel.Trace);

                amount = voucher.Balance;

                return CreditVoucherValidationEnum.ValidationSuccess;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);

                return CreditVoucherValidationEnum.ValidationUnknownError;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        public virtual decimal AddToCreditVoucher(RecordIdentifier creditVoucherID, decimal amount, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            decimal result = 0.0M;

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(creditVoucherID)}: {creditVoucherID}", LogLevel.Trace);
                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "New user context set", LogLevel.Trace);
                    dataModel.Connection.SetContext(logonInfo.UserID);
                }

                result = Providers.CreditVoucherData.AddToCreditVoucher(
                    dataModel,
                    creditVoucherID,
                    amount,
                    logonInfo.storeId ?? "",
                    logonInfo.terminalId ?? "",
                    logonInfo.UserID,
                    logonInfo.StaffID);

                Utils.Log(this, "Voucher updated", LogLevel.Trace);

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "Original user context restored", LogLevel.Trace);
                    dataModel.Connection.RestoreContext();
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);                
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return result;
        }
    }
}