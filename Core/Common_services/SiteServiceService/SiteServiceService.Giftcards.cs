using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        
        public virtual RecordIdentifier AddNewGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GiftCard giftCard, bool closeConnection, string prefix, int? numberSequenceLowest = null)
        {
            RecordIdentifier result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.AddNewGiftCertificate(CreateLogonInfo(entry), giftCard, prefix, numberSequenceLowest), closeConnection);

            return result;
        }

        public virtual List<GiftCard> SearchGiftCards(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GiftCardFilter filter, out int itemCount, bool closeConnection)
        {
            List<GiftCard> result = null;
            itemCount = 0;
            int itemCountCopy = itemCount;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SearchGiftCards(CreateLogonInfo(entry), filter, out itemCountCopy), closeConnection);

            itemCount = itemCountCopy;
            return result;
        }

        public virtual bool ActivateGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftCardID, RecordIdentifier transactionID, RecordIdentifier receiptID, bool closeConnection)
        {
            string comment = "";
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.ActivateGiftCertificate(giftCardID, transactionID, receiptID, CreateLogonInfo(entry), ref comment), closeConnection);

            return result;
        }

        public virtual bool MarkGiftCertificateIssued(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftCardID, bool closeConnection)
        {
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.MarkGiftCertificateIssued(giftCardID, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual bool DeactivateGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftCardID, bool closeConnection)
        {
            string comment = "";
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeactivateGiftCertificate(giftCardID, "", CreateLogonInfo(entry), ref comment), closeConnection);

            return result;
        }

        public virtual GiftCard GetGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftCardID, bool closeConnection)
        {
            GiftCard giftCard = null;

            DoRemoteWork(entry, siteServiceProfile, () => giftCard = server.GetGiftCard(giftCardID, CreateLogonInfo(entry)), closeConnection);

            return giftCard;
        }

        public virtual List<GiftCardLine> GetGiftCardLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftcardID, bool closeConnection)
        {
            List<GiftCardLine> lines = null;

            DoRemoteWork(entry, siteServiceProfile, () => lines = server.GetGiftCardLines(giftcardID, CreateLogonInfo(entry)), closeConnection);

            return lines;
        }

        public virtual void DeleteGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftcardID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteGiftCertificate(giftcardID, CreateLogonInfo(entry)), closeConnection);
        }

        public virtual decimal AddToGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftCardID, decimal amount, bool closeConnection)
        {
            decimal result = 0m;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.AddToGiftCertificate(giftCardID, amount, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual GiftCardValidationEnum ValidateGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref decimal amount, RecordIdentifier giftCardID, bool closeConnection)
        {
            GiftCardValidationEnum result;

            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                result = (GiftCardValidationEnum)((int)server.ValidateGiftCertificate(ref amount, giftCardID, CreateLogonInfo(entry)));

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

        public virtual GiftCardValidationEnum UseGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref decimal amount, RecordIdentifier giftCardID, RecordIdentifier transactionId, RecordIdentifier receiptId, bool closeConnection)
        {
            GiftCardValidationEnum result;

            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                result = (GiftCardValidationEnum)((int)server.UseGiftCertificate(ref amount, giftCardID, transactionId, receiptId, CreateLogonInfo(entry)));

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

        public virtual GiftCardValidationEnum UpdateGiftCardPaymentReceipt(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftCardID,
                                                                   RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID,
                                                                   RecordIdentifier receiptID, bool closeConnection)
        {
            GiftCardValidationEnum result = default(GiftCardValidationEnum);

            DoRemoteWork(entry, siteServiceProfile, () => result = (GiftCardValidationEnum)((int)server.UpdateGiftCardPaymentReceipt(giftCardID, transactionID, receiptID, storeID, terminalID, CreateLogonInfo(entry))), closeConnection);

            return result;
        }

    }
}
