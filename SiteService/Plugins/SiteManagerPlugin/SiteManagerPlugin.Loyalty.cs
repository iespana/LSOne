using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual List<LoyaltyMSRCard> GetCustomerMSRCards(List<DataEntity> customers, List<DataEntity> schemas,
            RecordIdentifier cardID, bool? hasCustomer, int tenderType,
            double? status, int statusInequality, int fromRow, int toRow, LoyaltyMSRCardSorting sortBy, bool backwards, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, 
                    $"{Utils.Starting} {nameof(cardID)}: {cardID}, {nameof(hasCustomer)}: {hasCustomer}, {nameof(tenderType)}: {tenderType}, {nameof(status)}: {status}, " +
                    $"{nameof(statusInequality)}: {statusInequality}, {nameof(fromRow)}: {fromRow}, {nameof(toRow)}: {toRow}, {nameof(sortBy)}: {sortBy}, {nameof(backwards)}: {backwards}");

                return Providers.LoyaltyMSRCardData.GetList(dataModel, customers, schemas, cardID, hasCustomer,
                    tenderType, status, (LoyaltyMSRCardInequality)statusInequality, fromRow, toRow, sortBy, backwards);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual List<LoyaltyMSRCardTrans> GetLoyaltyTrans(
            string StoreFilter,
            string TerminalFilter,
            string MSRCardFilter,
            string SchemeFilter,
            int TypeFilter,
            int OpenFilter,
            int EntryTypeFilter,
            string CustomerFilter,
            string receiptID,
            Date dateFrom,
            Date dateTo,
            Date expiredateFrom,
            Date expiredateTo,
            int rowFrom,
            int rowTo, LogonInfo logonInfo,
            bool backwards = false)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this,
                    $"{Utils.Starting} {nameof(StoreFilter)}: {StoreFilter}, {nameof(TerminalFilter)}: {TerminalFilter}, {nameof(MSRCardFilter)}: {MSRCardFilter}, {nameof(SchemeFilter)}: {SchemeFilter}, " +
                    $"{nameof(TypeFilter)}: {TypeFilter}, {nameof(OpenFilter)}: {OpenFilter}, {nameof(EntryTypeFilter)}: {EntryTypeFilter}, {nameof(CustomerFilter)}: {CustomerFilter}, {nameof(receiptID)}: {receiptID}");

                return Providers.LoyaltyMSRCardTransData.GetList(dataModel,
                    StoreFilter,
                    TerminalFilter,
                    MSRCardFilter,
                    SchemeFilter,
                    TypeFilter,
                    OpenFilter,
                    EntryTypeFilter,
                    CustomerFilter,
                    receiptID,
                    dateFrom,
                    dateTo,
                    expiredateFrom,
                    expiredateTo,
                    rowFrom, rowTo + 1, backwards);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual LoyaltyPoints GetPointsExchangeRate(RecordIdentifier schemeID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(schemeID)}: {schemeID}");

                return Providers.LoyaltyPointsData.GetPointsExchangeRate(dataModel, schemeID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual LoyaltyPointStatus GetLoyaltyPointsStatus(RecordIdentifier customerId, LoyaltyPointStatus pointStatus, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(customerId)}: {customerId}");

                Providers.LoyaltyMSRCardTransData.GetLoyaltyPointsStatus(dataModel, customerId, pointStatus);
                LoyaltyMSRCard card = Providers.LoyaltyMSRCardData.Get(dataModel, pointStatus.CardNumber);
                pointStatus.CustomerID = card.CustomerID;
                return pointStatus;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return pointStatus;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void SaveLoyaltyMSRCardTrans(LoyaltyMSRCardTrans loyaltyTrans, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.LoyaltyMSRCardTransData.Save(dataModel, loyaltyTrans);
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

        public virtual LoyaltyMSRCard GetLoyaltyMSRCard(RecordIdentifier cardNumber, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(cardNumber)}: {cardNumber}");

                return Providers.LoyaltyMSRCardData.Get(dataModel, cardNumber);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual int GetLoyaltyMSRCardTransCount(RecordIdentifier cardNumber, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(cardNumber)}: {cardNumber}");

                return Providers.LoyaltyMSRCardTransData.GetListCount(dataModel, cardNumber);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return 0;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual int GetCustomerLoyaltyMSRCardTransCount(RecordIdentifier cardNumber, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(cardNumber)}: {cardNumber}");

                return Providers.LoyaltyMSRCardTransData.NumberOfCustomerTransactionsForCard(dataModel, cardNumber);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return 0;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual LoyaltyMSRCardTrans GetLoyaltyMSRCardTrans(RecordIdentifier loyMsrCardTransID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(loyMsrCardTransID)}: {loyMsrCardTransID}");

                return Providers.LoyaltyMSRCardTransData.Get(dataModel, loyMsrCardTransID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual List<LoyaltyMSRCardTrans> GetLoyaltyMSRCardTransList(
            string StoreFilter,
            string TerminalFilter,
            string MSRCardFilter,
            string SchemeFilter,
            int TypeFilter,
            int OpenFilter,
            int EntryTypeFilter,
            string CustomerFilter,
            string receiptID,
            Date dateFrom,
            Date dateTo,
            Date expiredateFrom,
            Date expiredateTo,
            int rowFrom, int rowTo, LogonInfo logonInfo, bool backwards = false)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this,
                    $"{Utils.Starting} {nameof(StoreFilter)}: {StoreFilter}, {nameof(TerminalFilter)}: {TerminalFilter}, {nameof(MSRCardFilter)}: {MSRCardFilter}, {nameof(SchemeFilter)}: {SchemeFilter}, " +
                    $"{nameof(TypeFilter)}: {TypeFilter}, {nameof(OpenFilter)}: {OpenFilter}, {nameof(EntryTypeFilter)}: {EntryTypeFilter}, {nameof(CustomerFilter)}: {CustomerFilter}, {nameof(receiptID)}: {receiptID}");

                return Providers.LoyaltyMSRCardTransData.GetList
                    (dataModel,
                        StoreFilter,
                        TerminalFilter,
                        MSRCardFilter,
                        SchemeFilter,
                        TypeFilter,
                        OpenFilter,
                        EntryTypeFilter,
                        CustomerFilter,
                        receiptID,
                        dateFrom,
                        dateTo,
                        expiredateFrom,
                        expiredateTo,
                        rowFrom, rowTo, backwards);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual LoyaltyMSRCard.TenderTypeEnum? GetLoyaltyCardType(RecordIdentifier cardNumber, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(cardNumber)}: {cardNumber}");

                return Providers.LoyaltyMSRCardTransData.GetLoyaltyCardType(dataModel, cardNumber);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual LoyaltyCustomer.ErrorCodes UpdateRemainingPoints(RecordIdentifier cardNumber, RecordIdentifier customerID,
            decimal loyaltyPoints, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(cardNumber)}: {cardNumber}, {nameof(customerID)}: {customerID}, {nameof(loyaltyPoints)}: {loyaltyPoints}");

                return Providers.LoyaltyMSRCardTransData.UpdateRemainingPoints(dataModel, cardNumber, customerID, loyaltyPoints);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return 0;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual LoyaltyCustomer.ErrorCodes? SetExpirePoints(RecordIdentifier cardNumber, RecordIdentifier customerID,
            DateTime transDate, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(cardNumber)}: {cardNumber}, {nameof(customerID)}: {customerID}, {nameof(transDate)}: {transDate.ToShortDateString()}");

                return Providers.LoyaltyMSRCardTransData.SetExpirePoints(dataModel, cardNumber, customerID, transDate, logonInfo != null ? logonInfo.UserID : null);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual LoyaltyCustomer GetLoyaltyCustomer(RecordIdentifier customerId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(customerId)}: {customerId}");

                return Providers.LoyaltyCustomerData.Get(dataModel, customerId);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual RecordIdentifier SaveLoyaltyMSRCard(LoyaltyMSRCard loyaltyCard, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.LoyaltyMSRCardData.Save(dataModel, loyaltyCard);
                return loyaltyCard.ID;
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

            return "";
        }

        public virtual void DeleteLoyaltyMSRCard(RecordIdentifier cardID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(cardID)}: {cardID}");

                Providers.LoyaltyMSRCardData.Delete(dataModel, cardID);
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

        public virtual decimal GetMaxLoyaltyMSRCardTransLnNum(string cardNumber, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(cardNumber)}: {cardNumber}");

                return Providers.LoyaltyMSRCardTransData.GetMaxLineNumber(dataModel, cardNumber);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return 0;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual LoyaltySchemes GetLoyaltyScheme(RecordIdentifier schemeId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(schemeId)}: {schemeId}");

                return Providers.LoyaltySchemesData.Get(dataModel, schemeId);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual List<LoyaltySchemes> GetLoyaltySchemes(LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.LoyaltySchemesData.GetList(dataModel);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual List<LoyaltyPoints> GetLoyaltySchemeRules(RecordIdentifier schemeId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(schemeId)}: {schemeId}");

                return Providers.LoyaltyPointsData.GetList(dataModel, schemeId);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual LoyaltyPoints GetLoyaltySchemeRule(RecordIdentifier schemeRuleId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(schemeRuleId)}: {schemeRuleId}");

                return Providers.LoyaltyPointsData.Get(dataModel, schemeRuleId);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual LoyaltySchemes SaveLoyaltyScheme(LoyaltySchemes scheme, RecordIdentifier copyRulesFrom, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(copyRulesFrom)}: {copyRulesFrom}");

                Providers.LoyaltySchemesData.Save(dataModel, scheme);
                Providers.LoyaltyPointsData.CopyRules(dataModel, copyRulesFrom, scheme.ID);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }

            return scheme;
        }

        public virtual void SaveLoyaltySchemeRule(LoyaltyPoints schemeRule, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.LoyaltyPointsData.Save(dataModel, schemeRule);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void UpdateIssuedLoyaltyPointsForCustomer(RecordIdentifier loyalityCardId, RecordIdentifier customerId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(loyalityCardId)}: {loyalityCardId}, {nameof(customerId)}: {customerId}");

                Providers.LoyaltyMSRCardTransData.UpdateIssuedLoyaltyPointsForCustomer(dataModel, loyalityCardId, customerId);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void DeleteLoyaltyScheme(RecordIdentifier loyaltySchemeId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(loyaltySchemeId)}: {loyaltySchemeId}");

                Providers.LoyaltySchemesData.Delete(dataModel, loyaltySchemeId);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void DeleteLoyaltySchemeRule(RecordIdentifier loyaltySchemeRuleId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(loyaltySchemeRuleId)}: {loyaltySchemeRuleId}");

                Providers.LoyaltyPointsData.Delete(dataModel, loyaltySchemeRuleId);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void UpdateCouponCustomerLink(RecordIdentifier couponID, RecordIdentifier customerID, LogonInfo logonInfo)
        {
            throw new NotImplementedException();
        }

        public virtual bool LoyaltyCardExistsForLoyaltyScheme(RecordIdentifier loyaltySchemeID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(loyaltySchemeID)}: {loyaltySchemeID}");

                return Providers.LoyaltyMSRCardData.ExistsForLoyaltyScheme(dataModel, loyaltySchemeID);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }
    }
}