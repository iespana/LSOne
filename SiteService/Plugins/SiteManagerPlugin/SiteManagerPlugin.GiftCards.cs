using System;
using System.Collections.Generic;
using System.Linq;
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
        public virtual RecordIdentifier AddNewGiftCertificate(LogonInfo logonInfo, GiftCard giftCard, string prefix, int? numberSequenceLowest = null)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting, LogLevel.Trace);
                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "User context changed");
                    dataModel.Connection.SetContext(logonInfo.UserID);
                }

                if(giftCard == null)
                {
                    giftCard = new GiftCard();
                    giftCard.CreatedDate = Date.Now;
                }

                //It is possible that the transaction ID is stored in the SecondaryID part of the ID even if the ID itself is empty
                //We need to take the ID apart and store the transaction ID in case it gets updated or overwritten in the Save function
                //as we need it when creating the gift card transaction line
                RecordIdentifier transactionID = string.IsNullOrEmpty((string)giftCard.ID.SecondaryID) ? RecordIdentifier.Empty : giftCard.ID.SecondaryID;
                giftCard.ID = string.IsNullOrEmpty((string)giftCard.ID) ? RecordIdentifier.Empty : giftCard.ID;
                
                                
                Providers.GiftCardData.Save(dataModel, giftCard, prefix, numberSequenceLowest);

                Utils.Log(this, "Gift card saved - ID: " + giftCard.ID);

                var cardLine = new GiftCardLine
                {
                    GiftCardID = giftCard.ID,
                    Amount = giftCard.Balance,
                    Operation = GiftCardLine.GiftCardLineEnum.Create,
                    UserID = logonInfo.UserID,
                    StoreID = logonInfo.storeId ?? "",
                    TerminalID = logonInfo.terminalId ?? "",
                    StaffID = logonInfo.StaffID,
                    TransactionDateTime = DateTime.Now
                };

                if (!string.IsNullOrEmpty((string)transactionID))
                {                    
                    cardLine.TransactionID = transactionID;
                }

                Providers.GiftCardLineData.Save(dataModel, cardLine);

                Utils.Log(this, "Gift card line saved");

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "Original user context restored");
                    dataModel.Connection.RestoreContext();
                }

                return giftCard.ID;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return RecordIdentifier.Empty;
        }

        public virtual List<GiftCard> SearchGiftCards(LogonInfo logonInfo, GiftCardFilter filter, out int itemCount)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.GiftCardData.AdvancedSearch(dataModel, filter, out itemCount);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            itemCount = 0;
            return null;
        }

        public virtual GiftCard GetGiftCard(RecordIdentifier giftcardID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} + {nameof(giftcardID)}: {giftcardID}");
                return Providers.GiftCardData.Get(dataModel, giftcardID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return null;
        }

        public virtual List<GiftCardLine> GetGiftCardLines(RecordIdentifier giftcardID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(giftcardID)}: {giftcardID}");
                return Providers.GiftCardLineData.GetList(dataModel, giftcardID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return new List<GiftCardLine>();
        }

        public virtual void DeleteGiftCertificate(RecordIdentifier giftCardID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(giftCardID)}: {giftCardID}");

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "User context changed");
                    dataModel.Connection.SetContext(logonInfo.UserID);
                }
                                
                Providers.GiftCardData.Delete(dataModel, giftCardID);

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "Original user context restored");
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
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual decimal AddToGiftCertificate(RecordIdentifier giftCardID, decimal amount, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            decimal result = 0.0M;

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(giftCardID)}: {giftCardID}, {nameof(amount)}: {Conversion.ToStr(amount)}");
                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "User context changed");
                    dataModel.Connection.SetContext(logonInfo.UserID);
                }

                result = Providers.GiftCardData.AddToGiftCard(
                    dataModel,
                    giftCardID,
                    amount,
                    logonInfo.storeId ?? "",
                    logonInfo.terminalId ?? "",
                    logonInfo.UserID,
                    logonInfo.StaffID);

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "Original user context restored");
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
                Utils.Log(this, Utils.Done);
            }
            return result;
        }

        public virtual bool ActivateGiftCertificate(RecordIdentifier id, RecordIdentifier transactionID, RecordIdentifier receiptID, LogonInfo logonInfo, ref string comment)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            bool result = false;

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(id)}: {id}, {nameof(transactionID)}: {transactionID}, {nameof(receiptID)}: {receiptID}");
                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "User context changed");
                    dataModel.Connection.SetContext(logonInfo.UserID);
                }

                result = Providers.GiftCardData.Activate(
                    dataModel,
                    id,
                    logonInfo.storeId,
                    logonInfo.terminalId,
                    (logonInfo.UserID == RecordIdentifier.Empty) ? "" : logonInfo.UserID,
                    (logonInfo.StaffID == RecordIdentifier.Empty) ? "" : logonInfo.StaffID,
                    transactionID,
                    receiptID);

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "Original user context restored");
                    dataModel.Connection.RestoreContext();
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                comment = e.ToString();
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return result;
        }

        public virtual bool MarkGiftCertificateIssued(RecordIdentifier id, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            bool result = false;

            try
            {
                Utils.Log(this, $"{Utils.Starting}, {nameof(id)}: {id}");
                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "User context changed");
                    dataModel.Connection.SetContext(logonInfo.UserID);
                }

                result = Providers.GiftCardData.MarkIssued(dataModel, id);

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "Original user context restored");
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
                Utils.Log(this, Utils.Done);
            }

            return result;
        }

        public virtual bool DeactivateGiftCertificate(RecordIdentifier id, RecordIdentifier transactionID, LogonInfo logonInfo, ref string comment)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            bool result = false;

            try
            {
                Utils.Log(this, $"{Utils.Starting}, {nameof(id)}: {id}, {nameof(transactionID)}: {transactionID}");
                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "User context changed");
                    dataModel.Connection.SetContext(logonInfo.UserID);
                }

                result = Providers.GiftCardData.Deactivate(
                    dataModel,
                    id,
                    logonInfo.storeId,
                    logonInfo.terminalId,
                    (logonInfo.UserID == RecordIdentifier.Empty) ? "" : logonInfo.UserID,
                    (logonInfo.StaffID == RecordIdentifier.Empty) ? "" : logonInfo.StaffID,
                    transactionID);

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "Original user context restored");
                    dataModel.Connection.RestoreContext();
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                comment = e.ToString();

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return result;
        }

        public virtual GiftCardValidationEnum UpdateGiftCardPaymentReceipt(RecordIdentifier giftCardID, RecordIdentifier transactionID,
                                                 RecordIdentifier receiptID, RecordIdentifier storeID, RecordIdentifier terminalID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting}, {nameof(giftCardID)}: {giftCardID}, {nameof(transactionID)}: {transactionID}, {nameof(receiptID)}: {receiptID}, {nameof(storeID)}: {storeID}, {nameof(terminalID)}: {terminalID}");
                GiftCard card = Providers.GiftCardData.Get(dataModel, giftCardID);

                if (card == null)
                {
                    Utils.Log(this, "Gift card was not found");
                    return GiftCardValidationEnum.ValidationSuccess;
                }

                List<GiftCardLine> cardLines = Providers.GiftCardLineData.GetList(dataModel, giftCardID);

                if (cardLines != null && cardLines.Count > 0)
                {
                    foreach (GiftCardLine toUpdate in cardLines.Where(
                        w =>
                        w.TransactionID == transactionID && w.StoreID == storeID && w.TerminalID == terminalID &&
                        w.ReceiptID == ""))
                    {
                        toUpdate.ReceiptID = receiptID;
                        Providers.GiftCardLineData.Save(dataModel, toUpdate);
                        Utils.Log(this, "Gift card line updated - ID: " + toUpdate.ID);
                    }
                }
                else
                {
                    Utils.Log(this, "No gift card lines found");
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return GiftCardValidationEnum.ValidationUnknownError;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return GiftCardValidationEnum.ValidationSuccess;
        }

        public virtual GiftCardValidationEnum UseGiftCertificate(ref decimal amount, RecordIdentifier giftCardID,
            RecordIdentifier transactionId, RecordIdentifier receiptId,
            LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting}, {nameof(giftCardID)}: {giftCardID}, {nameof(transactionId)}: {transactionId}, {nameof(receiptId)}: {receiptId}");

                GiftCard card = Providers.GiftCardData.Get(dataModel, giftCardID);

                if (card == null)
                {
                    Utils.Log(this, "Card not found");
                    amount = 0.0M;
                    return GiftCardValidationEnum.ValidationCardNotFound;
                }                

                if (!card.Active)
                {
                    amount = card.Balance;
                    return GiftCardValidationEnum.ValidationCardNotActive;
                }

                if (card.Balance < amount)
                {
                    amount = card.Balance;
                    return GiftCardValidationEnum.ValidationBalanceToLow;
                }

                card.Balance -= amount;
                card.Active = card.Balance != 0 || card.Refillable;
                card.LastUsedDate = Date.Now;

                Providers.GiftCardData.Save(dataModel, card);

                Utils.Log(this, "Gift card amount changes saved");

                var line = new GiftCardLine
                {
                    Amount = amount,
                    GiftCardID = card.ID,
                    Operation = GiftCardLine.GiftCardLineEnum.TakeFromGiftCard,
                    StoreID = logonInfo.storeId ?? "",
                    TerminalID = logonInfo.terminalId ?? "",
                    UserID = logonInfo.UserID,
                    StaffID = logonInfo.StaffID,
                    ReceiptID = receiptId,
                    TransactionID = transactionId,
                    TransactionDateTime = DateTime.Now
                };

                Providers.GiftCardLineData.Save(dataModel, line);

                Utils.Log(this, "Gift card line saved");

                amount = card.Balance;

                return GiftCardValidationEnum.ValidationSuccess;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);

                return GiftCardValidationEnum.ValidationUnknownError;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual GiftCardValidationEnum ValidateGiftCertificate(ref decimal amount, RecordIdentifier giftCardID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting}, {nameof(giftCardID)}: {giftCardID}");

                GiftCard card = Providers.GiftCardData.Get(dataModel, giftCardID);

                if (card == null)
                {
                    Utils.Log(this, "Gift card not found");
                    amount = 0.0M;
                    return GiftCardValidationEnum.ValidationCardNotFound;
                }

                if (!card.Active)
                {
                    amount = card.Balance;
                    return GiftCardValidationEnum.ValidationCardNotActive;
                }

                if (card.Balance == decimal.Zero)
                {
                    amount = card.Balance;
                    return GiftCardValidationEnum.ValidationCardHasZeroBalance;
                }

                if (card.Balance < amount)
                {
                    amount = card.Balance;
                    return GiftCardValidationEnum.ValidationBalanceToLow;
                }

                amount = card.Balance;
                return GiftCardValidationEnum.ValidationSuccess;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);

                return GiftCardValidationEnum.ValidationUnknownError;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }
    }
}