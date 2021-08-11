using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {

        public virtual List<CreditVoucher> SearchCreditVouchers(IConnectionManager entry, SiteServiceProfile siteServiceProfile, CreditVoucherFilter filter, out int itemCount, bool closeConnection)
        {
            List<CreditVoucher> result = null;

            itemCount = 0;
            int itemCountCopy = itemCount;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SearchCreditVouchers(CreateLogonInfo(entry), filter, out itemCountCopy), closeConnection);

            itemCount = itemCountCopy;
            return result;
        }

        public virtual CreditVoucher GetCreditVoucher(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier creditvoucherID, bool closeConnection)
        {
            CreditVoucher voucher = null;

            DoRemoteWork(entry, siteServiceProfile, () => voucher = server.GetCreditVoucher(creditvoucherID, CreateLogonInfo(entry)), closeConnection);

            return voucher;

        }

        public virtual List<CreditVoucherLine> GetCreditVoucherLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier creditvoucherID, bool closeConnection)
        {
            List<CreditVoucherLine> lines = null;

            DoRemoteWork(entry, siteServiceProfile, () => lines = server.GetCreditVoucherLines(creditvoucherID, CreateLogonInfo(entry)), closeConnection);

            return lines;
        }


        public virtual RecordIdentifier IssueCreditVoucher(IConnectionManager entry, SiteServiceProfile siteServiceProfile, CreditVoucher voucher, RecordIdentifier transactionId, RecordIdentifier receiptId, bool closeConnection)
        {
            RecordIdentifier result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.IssueCreditVoucher(CreateLogonInfo(entry), voucher, transactionId, receiptId), closeConnection);

            return result;
        }

        public virtual CreditVoucherValidationEnum ValidateCreditVoucher(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref decimal amount, RecordIdentifier creditVoucherID, bool closeConnection)
        {
            CreditVoucherValidationEnum result;

            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                result = (CreditVoucherValidationEnum)server.ValidateCreditVoucher(ref amount, creditVoucherID, CreateLogonInfo(entry));

                if (closeConnection)
                {
                    Disconnect(entry);
                }
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }

            return result;
        }

        public virtual CreditVoucherValidationEnum UseCreditVoucher(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref decimal amount, RecordIdentifier creditVoucherID, RecordIdentifier transactionId, RecordIdentifier receiptId, bool closeConnection)
        {
            CreditVoucherValidationEnum result;

            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                result = (CreditVoucherValidationEnum)server.UseCreditVoucher(ref amount, creditVoucherID, transactionId, receiptId, CreateLogonInfo(entry));

                if (closeConnection)
                {
                    Disconnect(entry);
                }
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }

            return result;
        }

        public virtual void DeleteCreditVoucher(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier creditvoucherID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteCreditVoucher(creditvoucherID, CreateLogonInfo(entry)), closeConnection);
        }

        public virtual decimal AddToCreditVoucher(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier creditVoucherID, decimal amount, bool closeConnection)
        {
            decimal result = 0m;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.AddToCreditVoucher(creditVoucherID, amount, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

    }
}
