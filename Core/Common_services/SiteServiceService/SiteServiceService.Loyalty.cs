using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public virtual LoyaltySchemes GetLoyaltyScheme(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile, 
            RecordIdentifier schemeID, 
            bool closeConnection)
        {
            LoyaltySchemes result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetLoyaltyScheme(schemeID, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual List<LoyaltySchemes> GetLoyaltySchemes(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile, 
            bool closeConnection)
        {
            List<LoyaltySchemes> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetLoyaltySchemes(CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual LoyaltyPoints GetLoyaltySchemeRule(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile, 
            RecordIdentifier loyaltySchemeRuleId, 
            bool closeConnection)
        {
            LoyaltyPoints result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetLoyaltySchemeRule(loyaltySchemeRuleId, CreateLogonInfo(entry)), closeConnection);

            return result;
        }


        public virtual List<LoyaltyPoints> GetLoyaltySchemeRules(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile, 
            RecordIdentifier loyaltySchemeId, 
            bool closeConnection)
        {
            List<LoyaltyPoints> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetLoyaltySchemeRules(loyaltySchemeId, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual LoyaltySchemes SaveLoyaltyScheme(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile, 
            LoyaltySchemes scheme, 
            RecordIdentifier copyRulesFrom,
            bool closeConnection)
        {
            LoyaltySchemes result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveLoyaltyScheme(scheme, copyRulesFrom, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual LoyaltySchemes SaveLoyaltyScheme(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile, 
            LoyaltySchemes scheme, 
            bool closeConnection)
        {
            SaveLoyaltyScheme(entry, siteServiceProfile, scheme, RecordIdentifier.Empty, closeConnection);

            return scheme;
        }

        public virtual void SaveLoyaltySchemeRule(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile, 
            LoyaltyPoints schemeRule, 
            bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveLoyaltySchemeRule(schemeRule, CreateLogonInfo(entry)), closeConnection);
        }


        public virtual void DeleteLoyaltyScheme(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile, 
            RecordIdentifier schemeID, 
            bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteLoyaltyScheme(schemeID, CreateLogonInfo(entry)), closeConnection);
        }

        public virtual void DeleteLoyaltySchemeRule(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile, 
            RecordIdentifier schemeRuleID,
            bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteLoyaltySchemeRule(schemeRuleID, CreateLogonInfo(entry)), closeConnection);
        }

        public bool LoyaltyCardExistsForLoyaltyScheme(IConnectionManager entry, SiteServiceProfile siteServiceProfile,RecordIdentifier loyaltySchemeID,bool closeConnection)
        {
            bool exists = false;
            DoRemoteWork(entry, siteServiceProfile, () => exists =  server.LoyaltyCardExistsForLoyaltyScheme(loyaltySchemeID, CreateLogonInfo(entry)), closeConnection);
            return exists;
        }

        public virtual LoyaltyPointStatus GetLoyaltyPointsStatus(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile, 
            LoyaltyPointStatus pointStatus,
            bool closeConnection = true)
        {

            pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.UnknownError;

            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                LoyaltyCustomer.ErrorCodes? returnValue;

                LoyaltyMSRCard loyaltyCard = server.GetLoyaltyMSRCard(pointStatus.CardNumber, CreateLogonInfo(entry));

                if (loyaltyCard == null)
                {
                    pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardNotFound;
                    pointStatus.Comment = Properties.Resources.ErrLoyaltyCardNotFound;
                    Disconnect(entry);
                    return pointStatus;
                }

                if (loyaltyCard.LinkID == "")
                {
                    pointStatus.Points = decimal.Zero;
                    pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.ErrLoyaltyIsNotActivated;
                    pointStatus.Comment = Properties.Resources.ErrLoyaltyIsNotActivated;
                    Disconnect(entry);
                    return pointStatus;
                }

                if (loyaltyCard.LinkID == RecordIdentifier.Empty) //Error
                {
                    pointStatus.Comment = Properties.Resources.LoyaltyCustomerNotFound.Replace("#1", loyaltyCard.CustomerName != "" ? loyaltyCard.CustomerName : (string)loyaltyCard.LinkID);
                    pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCustomerNotFound;

                    Disconnect(entry);
                    return pointStatus;
                }

                returnValue = server.SetExpirePoints(pointStatus.CardNumber, loyaltyCard.CustomerID, DateTime.Now, CreateLogonInfo(entry));

                if (returnValue != LoyaltyCustomer.ErrorCodes.NoErrors) //Error
                {
                    pointStatus.ResultCode = (returnValue == null) ? 
                                             LoyaltyCustomer.ErrorCodes.UnknownError : 
                                             (LoyaltyCustomer.ErrorCodes)returnValue;

                    pointStatus.Comment = pointStatus.ResultCode == LoyaltyCustomer.ErrorCodes.UnknownError ? 
                                          "" : 
                                          LoyaltyCustomer.AsString(pointStatus.ResultCode);

                    Disconnect(entry);

                    return pointStatus;
                }

                pointStatus.LoyaltyTenderType = loyaltyCard.TenderType;

                pointStatus = server.GetLoyaltyPointsStatus(loyaltyCard.CustomerID, pointStatus, CreateLogonInfo(entry));
                
                LoyaltyPoints rule = server.GetPointsExchangeRate(loyaltyCard.SchemeID, CreateLogonInfo(entry));

                pointStatus.CurrentValue = decimal.Zero;
                if (rule != null)
                {
                    //Because earlier loyalty implementations allowed negative points we need to do this here
                    rule.Points = rule.Points < decimal.Zero ? rule.Points * -1 : rule.Points;
                    pointStatus.CurrentValue = rule.Points > decimal.Zero
                                                   ? (rule.QtyAmountLimit / rule.Points * pointStatus.Points)
                                                   : decimal.Zero;
                }

                pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.NoErrors;

            }
            catch (Exception e)
            {
                if(e is EndpointNotFoundException)
                {
                    pointStatus.Comment = Properties.Resources.CouldNotConnectToSiteService;
                }
                else if (e is ClientTimeNotSynchronizedException)
                {
                    pointStatus.Comment = Properties.Resources.ClientTimeNotSynchronized;
                }
                else
                {
                    pointStatus.Comment = Properties.Resources.UnknownServerError;
                }
                
                Disconnect(entry);

                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), e);
                throw;
            }

            if (closeConnection )
            {
                Disconnect(entry);
            }

            return pointStatus;
        }

        public virtual int GetLoyaltyMSRCardTransCount(
            IConnectionManager dataModel, 
            SiteServiceProfile siteServiceProfile, 
            RecordIdentifier cardNumber, 
            bool closeConnection = true)
        {
            int result = 0;

            DoRemoteWork(dataModel, siteServiceProfile, () => result = server.GetLoyaltyMSRCardTransCount(cardNumber, CreateLogonInfo(dataModel)), closeConnection);

            return result;
        }

        public virtual int GetCustomerLoyaltyMSRCardTransCount(
            IConnectionManager dataModel, 
            SiteServiceProfile siteServiceProfile, 
            RecordIdentifier cardNumber, 
            bool closeConnection = true)
        {
            int result = 0;

            DoRemoteWork(dataModel, siteServiceProfile, () => result = server.GetCustomerLoyaltyMSRCardTransCount(cardNumber, CreateLogonInfo(dataModel)), closeConnection);

            return result;
        }


        public virtual decimal GetMaxLoyaltyMSRCardTransLnNum(
            IConnectionManager dataModel, 
            SiteServiceProfile siteServiceProfile, 
            string cardNumber, 
            bool closeConnection = true)
        {
            decimal retVal;

            try
            {
                if (isClosed)
                {
                    Connect(dataModel, siteServiceProfile);
                }

                retVal = server.GetMaxLoyaltyMSRCardTransLnNum(cardNumber, CreateLogonInfo(dataModel));
            }
            catch (Exception e)
            {
                Disconnect(dataModel);
                dataModel.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), e);
                throw;
            }

            if (closeConnection)
            {
                Disconnect(dataModel);
            }

            return retVal;
        }

        public virtual LoyaltyMSRCard GetLoyaltyMSRCard(
            IConnectionManager dataModel, 
            SiteServiceProfile siteServiceProfile, 
            RecordIdentifier cardNumber, 
            bool closeConnection = true)
        {
            LoyaltyMSRCard result = null;

            DoRemoteWork(dataModel, siteServiceProfile, () => result = server.GetLoyaltyMSRCard(cardNumber, CreateLogonInfo(dataModel)), closeConnection);

            return result;
        }

        public virtual RecordIdentifier SaveLoyaltyMSRCard(
            IConnectionManager dataModel, 
            SiteServiceProfile siteServiceProfile, 
            LoyaltyMSRCard loyaltyCard, 
            bool closeConnection = true)
        {
            RecordIdentifier id = null;

            DoRemoteWork(dataModel, siteServiceProfile, () => id = server.SaveLoyaltyMSRCard(loyaltyCard, CreateLogonInfo(dataModel)), closeConnection);

            return id;
        }

        public virtual LoyaltyMSRCardTrans GetLoyaltyMSRCardTrans(
            IConnectionManager dataModel, 
            SiteServiceProfile siteServiceProfile, 
            RecordIdentifier loyMsrCardTransID, 
            bool closeConnection = true)
        {
            LoyaltyMSRCardTrans retVal;

            try
            {
                if (isClosed)
                {
                    Connect(dataModel, siteServiceProfile);
                }

                retVal = server.GetLoyaltyMSRCardTrans(loyMsrCardTransID, CreateLogonInfo(dataModel));
            }
            catch (Exception e)
            {
                Disconnect(dataModel);
                dataModel.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), e);
                throw;
            }

            if (closeConnection)
            {
                Disconnect(dataModel);
            }

            return retVal;
        }

        public virtual void DeleteLoyaltyMSRCard(
            IConnectionManager dataModel, 
            SiteServiceProfile siteServiceProfile, 
            RecordIdentifier cardID, 
            bool closeConnection = true)
        {
            DoRemoteWork(dataModel, siteServiceProfile, () => server.DeleteLoyaltyMSRCard(cardID, CreateLogonInfo(dataModel)), closeConnection);
        }

        public virtual void UpdateIssuedLoyaltyPoints(
            IConnectionManager dataModel, 
            SiteServiceProfile siteServiceProfile, 
            ref LoyaltyCustomer.ErrorCodes valid, 
            ref string comment, 
            RecordIdentifier transactionId, 
            decimal lineNum,
            string cardNumber, 
            DateTime transDate, 
            decimal loyaltyPoints, 
            RecordIdentifier receiptId, 
            bool closeConnection = true)
        {

            valid = LoyaltyCustomer.ErrorCodes.UnknownError;

            try
            {

                if (isClosed)
                {
                    Connect(dataModel, siteServiceProfile);
                }

                RecordIdentifier pCardNumber = new RecordIdentifier(cardNumber);
                
                LoyaltyMSRCard loyaltyCard = server.GetLoyaltyMSRCard(pCardNumber, CreateLogonInfo(dataModel));

                if (loyaltyCard == null || loyaltyCard.TenderType == LoyaltyMSRCard.TenderTypeEnum.Blocked)
                {

                    if (loyaltyCard.TenderType == LoyaltyMSRCard.TenderTypeEnum.Blocked)
                    {
                        valid = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardBlocked;
                        comment = Properties.Resources.ErrLoyaltyCardBlocked;
                    }
                    else
                    {
                        valid = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardNotFound;
                        comment = Properties.Resources.ErrLoyaltyCardNotFound;
                    }

                    Disconnect(dataModel);
                    return;
                }

                if (string.IsNullOrEmpty((string)loyaltyCard.LinkID))
                {
                    valid = LoyaltyCustomer.ErrorCodes.ErrLoyaltyIsNotActivated;
                    comment = Properties.Resources.ErrLoyaltyIsNotActivated;
                    Disconnect(dataModel);
                    return;
                }

                if(loyaltyCard.CustomerID == RecordIdentifier.Empty) //Error
                {
                    comment = string.Format(Properties.Resources.LoyaltyCustomerNotFound, loyaltyCard.CustomerID);
                    valid = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCustomerNotFound;

                    Disconnect(dataModel);
                    return;
                }

                LoyaltyMSRCardTrans loyaltyTrans = new LoyaltyMSRCardTrans();

                RecordIdentifier storeId = dataModel.CurrentStoreID;
                RecordIdentifier terminalId = dataModel.CurrentTerminalID;
                RecordIdentifier staffId = dataModel.CurrentUser.ToString();

                loyaltyTrans.TransactionID = (string)transactionId; //TRANSACTIONID – current transaction ID

                loyaltyTrans.LineNumber = server.GetMaxLoyaltyMSRCardTransLnNum(cardNumber, CreateLogonInfo(dataModel)) + 1;

                loyaltyTrans.ReceiptID = (string)receiptId; //RECEIPTID – current receipt ID
                loyaltyTrans.Points = loyaltyPoints < 0 ? loyaltyPoints * -1 : loyaltyPoints; //POINTS – used points amount (must be positive, in example: 100)
                loyaltyTrans.DateOfIssue = new Date(DateTime.Now); //DATEOFISSUE – current date            
                loyaltyTrans.StoreID = (string)storeId; //STOREID – current store id
                loyaltyTrans.TerminalID = (string)terminalId; //TERMINALID – current terminal id
                loyaltyTrans.CardNumber = cardNumber;//CARDNUMBER – used loyalty card number

                loyaltyTrans.CustomerID = loyaltyCard.CustomerID; //LOYALTYCUSTID – customer ID from loyalty card
                loyaltyTrans.EntryType = LoyaltyMSRCardTrans.EntryTypeEnum.None;

                LoyaltySchemes loyaltyScheme = server.GetLoyaltyScheme(loyaltyCard.SchemeID, CreateLogonInfo(dataModel));

                if (loyaltyScheme == null)
                {
                    throw new Exception(Properties.Resources.LoyaltySchemeNotFound.Replace("#1", (string) loyaltyCard.SchemeID));
                }

                switch (loyaltyScheme.ExpirationTimeUnit)
                {
                    case TimeUnitEnum.Day:
                        loyaltyTrans.ExpirationDate = new Date(loyaltyTrans.DateOfIssue.DateTime.AddDays(loyaltyScheme.ExpireTimeValue));
                        break;
                    case TimeUnitEnum.Week:
                        loyaltyTrans.ExpirationDate = new Date(loyaltyTrans.DateOfIssue.DateTime.AddDays(loyaltyScheme.ExpireTimeValue * 7));
                        break;
                    case TimeUnitEnum.Month:
                        loyaltyTrans.ExpirationDate = new Date(loyaltyTrans.DateOfIssue.DateTime.AddMonths(loyaltyScheme.ExpireTimeValue));
                        break;
                    case TimeUnitEnum.Year:
                        loyaltyTrans.ExpirationDate = new Date(loyaltyTrans.DateOfIssue.DateTime.AddYears(loyaltyScheme.ExpireTimeValue));
                        break;
                }

                loyaltyTrans.SchemeID = loyaltyCard.SchemeID; 
                loyaltyTrans.StaffID = (string)staffId; 

                loyaltyTrans.LoyPointsTransLineNumber = lineNum; 
                loyaltyTrans.Type = LoyaltyMSRCardTrans.TypeEnum.IssuePoints; 
                loyaltyTrans.RemainingPoints = loyaltyPoints < 0 ? loyaltyPoints * -1 : loyaltyPoints; 
                loyaltyTrans.Open = true; 


                server.SaveLoyaltyMSRCardTrans(loyaltyTrans, CreateLogonInfo(dataModel));

                valid = 0;
            }
            catch (Exception e)
            {
                Disconnect(dataModel);
                dataModel.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), e);
                throw;
            }

            if (closeConnection)
            {
                Disconnect(dataModel);
            }
        }


        public virtual LoyaltyPointStatus UpdateUsedLoyaltyPoints(IConnectionManager dataModel,
             SiteServiceProfile siteServiceProfile,
            LoyaltyPointStatus pointStatus,
            RecordIdentifier transactionId, decimal lineNum,
            DateTime transDate,
            decimal loyaltyPoints, RecordIdentifier receiptId,
            bool voidLoyaltyTrans,
            bool closeConnection = true)
        {
            pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.UnknownError;

            try
            {
                if (isClosed)
                {
                    Connect(dataModel, siteServiceProfile);
                }

                LoyaltyMSRCard.TenderTypeEnum? cardType = Providers.LoyaltyMSRCardTransData.GetLoyaltyCardType(dataModel, pointStatus.CardNumber);

                if (cardType != LoyaltyMSRCard.TenderTypeEnum.ContactTender && cardType != LoyaltyMSRCard.TenderTypeEnum.CardTender) //Error
                {
                    //valid = false;
                    switch (cardType)
                    {
                        case null:
                            pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardNotFound;
                            pointStatus.Comment = Properties.Resources.ErrLoyaltyCardNotFound;
                            break;
                        case LoyaltyMSRCard.TenderTypeEnum.Blocked:
                            pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardBlocked;
                            pointStatus.Comment = Properties.Resources.ErrLoyaltyCardBlocked;
                            break;
                        case LoyaltyMSRCard.TenderTypeEnum.NoTender:
                            pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardIsNoTenderCard;
                            pointStatus.Comment = Properties.Resources.ErrLoyaltyCardIsNoTenderCard;
                            break;
                        default:
                            pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.UnknownError;
                            pointStatus.Comment = "";
                            break;
                    }

                    Disconnect(dataModel);
                    return pointStatus;
                }

                pointStatus = GetLoyaltyPointsStatus(dataModel, siteServiceProfile, pointStatus, false);

                if (pointStatus.ResultCode != LoyaltyCustomer.ErrorCodes.NoErrors)
                {
                    Disconnect(dataModel);
                    return pointStatus;
                }

                if (loyaltyPoints < decimal.Zero && pointStatus.Points < Math.Abs(loyaltyPoints))
                {
                    loyaltyPoints = pointStatus.Points > decimal.Zero ? pointStatus.Points * -1 : loyaltyPoints;
                }

                if (pointStatus.Points == decimal.Zero || pointStatus.Points < Math.Abs(loyaltyPoints))
                {
                    pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.ErrNotEnoughLoyaltyPoints;
                    pointStatus.Comment = loyaltyPoints < decimal.Zero ? 
                                          Properties.Resources.ErrLoyaltyPointsCannotBeReturned : 
                                          Properties.Resources.ErrNotEnoughLoyaltyPoints;

                    Disconnect(dataModel);
                    return pointStatus;
                }

                LoyaltyMSRCard loyaltyCard = Providers.LoyaltyMSRCardData.Get(dataModel, pointStatus.CardNumber);


                if (loyaltyCard == null || loyaltyCard.CustomerID == RecordIdentifier.Empty)
                {
                    loyaltyCard = server.GetLoyaltyMSRCard(pointStatus.CardNumber, CreateLogonInfo(dataModel));

                    if (loyaltyCard == null)
                    {
                        pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardNotFound;
                        pointStatus.Comment = Properties.Resources.ErrLoyaltyCardNotFound;
                        return null;
                    }
                }


                if (loyaltyCard.TenderType == LoyaltyMSRCard.TenderTypeEnum.Blocked)
                {
                    pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardBlocked;
                    pointStatus.Comment = Properties.Resources.ErrLoyaltyCardBlocked;

                    Disconnect(dataModel);
                    return pointStatus;
                }

                if(loyaltyCard.CustomerID == RecordIdentifier.Empty) //Error
                {
                    pointStatus.Comment = string.Format(Properties.Resources.LoyaltyCustomerNotFound, loyaltyCard.CustomerID);
                    pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCustomerNotFound;

                    Disconnect(dataModel);
                    return pointStatus;
                }

                LoyaltyMSRCardTrans loyaltyTrans = new LoyaltyMSRCardTrans();

                RecordIdentifier storeId = dataModel.CurrentStoreID;
                RecordIdentifier terminalId = dataModel.CurrentTerminalID;
                RecordIdentifier staffId = dataModel.CurrentUser.ToString();

                loyaltyTrans.CardNumber = (string)pointStatus.CardNumber;

                loyaltyTrans.LineNumber = server.GetMaxLoyaltyMSRCardTransLnNum((string)pointStatus.CardNumber, CreateLogonInfo(dataModel)) + 1;

                loyaltyTrans.TransactionID = (string)transactionId; 
                loyaltyTrans.ReceiptID = (string)receiptId; 

                loyaltyTrans.Points = loyaltyPoints > 0 ? loyaltyPoints * -1 : loyaltyPoints; 

                loyaltyTrans.StoreID = (string)storeId; 
                loyaltyTrans.TerminalID = (string)terminalId; 
                loyaltyTrans.CustomerID = loyaltyCard.CustomerID;

                loyaltyTrans.EntryType = voidLoyaltyTrans ? 
                                         LoyaltyMSRCardTrans.EntryTypeEnum.Voided : 
                                         LoyaltyMSRCardTrans.EntryTypeEnum.None;

                loyaltyTrans.SchemeID = loyaltyCard.SchemeID; 
                loyaltyTrans.StaffID = (string)staffId; 
                loyaltyTrans.LoyPointsTransLineNumber = lineNum;

                loyaltyTrans.Type = LoyaltyMSRCardTrans.TypeEnum.UsePoints;

                server.SaveLoyaltyMSRCardTrans(loyaltyTrans, CreateLogonInfo(dataModel));

                LoyaltyCustomer.ErrorCodes ret = server.UpdateRemainingPoints(pointStatus.CardNumber, loyaltyCard.CustomerID, loyaltyPoints, CreateLogonInfo(dataModel));

                if (ret != LoyaltyCustomer.ErrorCodes.NoErrors)
                {
                    pointStatus.ResultCode = ret;
                    pointStatus.Comment = LoyaltyCustomer.AsString(ret);

                    Disconnect(dataModel);
                    return pointStatus;
                }

                pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.NoErrors;

            }
            catch (Exception e)
            {
                Disconnect(dataModel);
                dataModel.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), e);
                throw;
            }

            if (closeConnection)
                Disconnect(dataModel);

            return pointStatus;
        }

        ///  <summary>
        ///  Update loyalty card customer ID
        ///  </summary>
        /// <param name="dataModel"></param>
        /// <param name="valid">The error code</param>
        ///  0 - OK
        ///  1 - The client is not a loyalty customer
        ///  2 - Loyalty card is not found or is blocked
        ///  7 - Loyalty card is already assigned to another custome
        ///  8 - Loyalty customer not found
        /// -1 - "Other errors"   
        ///  <param name="comment">The error message</param>
        /// <param name="loyaltyCardId"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="customerId"></param>
        /// <param name="closeConnection"></param>
        public virtual void UpdateLoyaltyCardCustomerID(
            IConnectionManager dataModel, 
            SiteServiceProfile siteServiceProfile, 
            ref LoyaltyCustomer.ErrorCodes valid, 
            ref string comment,
            RecordIdentifier loyaltyCardId, 
            RecordIdentifier customerId, 
            bool closeConnection = true)
        {
            valid = LoyaltyCustomer.ErrorCodes.UnknownError;

            try
            {
                if (isClosed)
                {
                    Connect(dataModel, siteServiceProfile);
                }
                
                LoyaltyMSRCard loyaltyCard = server.GetLoyaltyMSRCard(loyaltyCardId, CreateLogonInfo(dataModel));

                // Check if card is blocked or does not exist
                if (loyaltyCard == null || loyaltyCard.TenderType == LoyaltyMSRCard.TenderTypeEnum.Blocked)
                {
                    //error

                    if (loyaltyCard == null)
                    {
                        comment = Properties.Resources.ErrLoyaltyCardNotFound;
                        valid = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardNotFound;
                    }
                    else
                    {
                        comment = Properties.Resources.ErrLoyaltyCardBlocked;
                        valid = LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardBlocked;
                    }

                    Disconnect(dataModel);
                    return;
                }

                if ((loyaltyCard.LinkID.ToString() != "") && ((loyaltyCard.LinkID != customerId) || (loyaltyCard.LinkID == customerId)))
                {
                    //loyalty card is already assigned for customer or another customer/cashier
                    List<LoyaltyMSRCardTrans> loyaltyTrans;

                    loyaltyTrans = server.GetLoyaltyMSRCardTransList(null, null, loyaltyCardId.ToString(), null, -1, -1, -1, null, null, Date.Empty, Date.Empty, Date.Empty, Date.Empty, 0, 0, CreateLogonInfo(dataModel), false);

                    if (loyaltyTrans != null && loyaltyTrans.Count > 0)
                    {
                        comment = loyaltyCard.LinkID != customerId ? 
                            Properties.Resources.ErrLoyaltyCardAlreadyAssigned : 
                            Properties.Resources.ErrCustomerAlreadyAssignedToCard;
                        valid = loyaltyCard.LinkID != customerId ? 
                            LoyaltyCustomer.ErrorCodes.ErrLoyaltyCardAlreadyAssigned : 
                            LoyaltyCustomer.ErrorCodes.ErrCustomerAlreadyAssignedToCard;

                        Disconnect(dataModel);

                        return;
                    }
                }

                LoyaltyCustomer loyaltyCust = null;

                if ((customerId != null) && (customerId.ToString() != String.Empty))
                {
                    loyaltyCust = Providers.LoyaltyCustomerData.Get(dataModel, customerId);
                }

                //Assign Customer ID to the loyalty card
                loyaltyCard.LinkType = LoyaltyMSRCard.LinkTypeEnum.Customer;
                if ((customerId != null) && !string.IsNullOrEmpty(customerId.StringValue))
                {
                    loyaltyCard.LinkID = customerId;
                }
                else
                {
                    loyaltyCard.LinkID = "";
                }

                server.SaveLoyaltyMSRCard(loyaltyCard, CreateLogonInfo(dataModel));

                if ((customerId != null) && !string.IsNullOrEmpty(customerId.StringValue))
                {
                    if (loyaltyCard.StartingPoints != decimal.Zero)
                    {
                        UpdateIssuedLoyaltyPoints(dataModel,
                            siteServiceProfile,
                            ref valid,
                            ref comment,
                            RecordIdentifier.Empty,
                            0,
                            loyaltyCard.CardNumber,
                            DateTime.Now.Date,
                            loyaltyCard.StartingPoints,
                            RecordIdentifier.Empty,
                             true);
                        if (valid != 0)
                        {
                            Disconnect(dataModel);
                            return;
                        }
                    }
                }
                valid = 0;

            }
            catch (Exception e)
            {
                Disconnect(dataModel);

                dataModel.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), e);
                throw;
            }

            if (closeConnection)
            {
                Disconnect(dataModel);
            }

        }

        public virtual List<LoyaltyMSRCard> GetCustomerMSRCards(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile, 
            List<DataEntity> customers, 
            List<DataEntity> schemes, 
            RecordIdentifier cardID, 
            bool? hasCustomer, 
            double? status, 
            LoyaltyMSRCardInequality statusInequality, 
            int tenderType, 
            int fromRow, 
            int toRow, 
            LoyaltyMSRCardSorting sortBy, 
            bool backwards, 
            bool closeConnection)
        {
            List<LoyaltyMSRCard> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetCustomerMSRCards(customers, schemes, cardID, hasCustomer, tenderType, status,
                (int)statusInequality, fromRow, toRow, sortBy, backwards, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual List<LoyaltyMSRCardTrans> GetLoyaltyTrans(
            IConnectionManager entry,
             SiteServiceProfile siteServiceProfile,
            string storeFilter,
            string terminalFilter,
            string msrCardFilter,
            string schemeFilter,
            int typeFilter,
            int openFilter,
            int entryTypeFilter,
            string customerFilter,
            string receiptID,
            Date dateFrom,
            Date dateTo,
            Date expiredateFrom,
            Date expiredateTo,
            int rowFrom,
            int rowTo,
            bool backwards,
            bool closeConnection)
        {
            List<LoyaltyMSRCardTrans> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetLoyaltyTrans(
                storeFilter,
                terminalFilter,
                msrCardFilter,
                schemeFilter,
                typeFilter,
                openFilter,
                entryTypeFilter,
                customerFilter,
                receiptID,
                dateFrom,
                dateTo,
                expiredateFrom,
                expiredateTo,
                rowFrom,
                rowTo,
                CreateLogonInfo(entry),
                backwards
                ), closeConnection);

            return result;
        }

        public virtual LoyaltyPoints GetPointsExchangeRate(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile, 
            RecordIdentifier schemeID, 
            bool closeConnection)
        {
            LoyaltyPoints result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPointsExchangeRate(schemeID, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual void UpdateIssuedLoyaltyPointsForCustomer(
            IConnectionManager dataModel, 
            SiteServiceProfile siteServiceProfile, 
            RecordIdentifier loyaltyCardId, 
            RecordIdentifier customerId, 
            bool closeConnection)
        {
            DoRemoteWork(dataModel, siteServiceProfile, () => server.UpdateIssuedLoyaltyPointsForCustomer(loyaltyCardId, customerId, CreateLogonInfo(dataModel)), closeConnection);
        }
    }
}
