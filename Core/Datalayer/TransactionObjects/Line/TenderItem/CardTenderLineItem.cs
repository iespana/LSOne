using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.DataLayer.TransactionObjects.EFT;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;

namespace LSOne.DataLayer.TransactionObjects.Line.TenderItem
{
    /// <summary>
    /// Used to register card payments.
    /// </summary>
    [Serializable]
    public class CardTenderLineItem: TenderLineItem, ICardTenderLineItem
    {
        #region Enums
        public enum CardEntryTypes
        {
            Swiped = 0,
            Keyed = 1,
            ChipReader = 2
        }

        public enum AuthorizationTypes
        {
            Manual = 0,
            Online = 1,
            Pin = 2
        }
        #endregion

        #region Member variables
        // EFT - Info
        private IEFTInfo eftInfo = new EFTInfo();//Various EFT information
        #endregion

        #region Properties

        /// <summary>
        /// The cardnumber
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// If the cardnumber is hidden
        /// </summary>
        public bool CardNumberHidden { get; set; }

        /// <summary>
        /// The name of the card VISA,Maestro,JCB,etc.
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// The id of the cardtype.
        /// </summary>
        public string CardTypeID { get; set; }

        /// <summary>
        /// The name of the card issuer.
        /// </summary>
        public string IssuerName { get; set; }

        /// <summary>
        /// The expiry date of the card.
        /// </summary>
        public string ExpiryDate { get; set; }

        /// <summary>
        /// The name of the cardholder.
        /// </summary>
        public string CardHolderName { get; set; }

        /// <summary>
        /// The type of entry, swiped,keyed or chipreader
        /// </summary>
        internal CardEntryTypes CardEntryType { get; set; }

        /// <summary>
        /// //The type of card authorization.
        /// </summary>
        internal AuthorizationTypes AuthorizationType { get; set; }

        /// <summary>
        /// The BIN of the card
        /// </summary>
        public string CardBin { get; set; }

        /// <summary>
        /// The merchant id used for authorization.
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// The authorization code received.
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// The authorization message received.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The authorization responce code received.
        /// </summary>
        public string ResponseCode { get; set; }

        /// <summary>
        /// EFT information regarding the EFT transaction
        /// </summary>
        public IEFTInfo EFTInfo
        {
            get { return eftInfo; }
            set { eftInfo = value; }
        }

        public CardTenderLineItem()
            :base()
        {
            CardNumber = "";
            CardName = "";
            CardTypeID = "";
            IssuerName = "";
            ExpiryDate = "";
            CardHolderName = "";
            CardBin = "";
            MerchantID = "";
            AuthCode = "";
            Message = "";
            ResponseCode = "";

            internalTenderType = TenderTypeEnum.CardTender;
        }

        #endregion
        
        

        public override object Clone()
        {
            var item = new CardTenderLineItem();
            Populate(item);
            return item;
        }

        protected void Populate(CardTenderLineItem item)
        {
            base.Populate(item);
            item.CardNumber = CardNumber;
            item.CardNumberHidden = CardNumberHidden;
            item.CardName = CardName;
            item.CardTypeID = CardTypeID;
            item.IssuerName = IssuerName;
            item.ExpiryDate = ExpiryDate;
            item.CardHolderName = CardHolderName;
            item.CardEntryType = CardEntryType;
            item.AuthorizationType = AuthorizationType;
            item.CardBin = CardBin;
            item.MerchantID = MerchantID;
            item.AuthCode = AuthCode;
            item.Message = Message;
            item.ResponseCode = ResponseCode;
            item.internalTenderType = TypeOfTender;

            item.EFTInfo = eftInfo.Clone() as EFTInfo;
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                /*
                * Strings      added as is
                * Int          added as is
                * Bool         added as is
                * 
                * Decimal      added with ToString()
                * DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xCardTender = new XElement("CardTenderLineItem",
                    new XElement("cardNumber", CardNumber),
                    new XElement("cardnumberHidden", CardNumberHidden),
                    new XElement("cardName", CardName),
                    new XElement("cardTypeId", CardTypeID),
                    new XElement("issuerName", IssuerName),
                    new XElement("expiryDate", ExpiryDate),
                    new XElement("cardHolderName", CardHolderName),
                    new XElement("cardEntryType", (int)CardEntryType),
                    new XElement("authorizationType", (int)AuthorizationType),
                    new XElement("cardBin", CardBin),
                    new XElement("merchantId", MerchantID),
                    new XElement("authCode", AuthCode),
                    new XElement("message", Message),
                    new XElement("responseCode", ResponseCode),
                    eftInfo.ToXML(errorLogger))
                ;
                xCardTender.Add(base.ToXML(errorLogger));
                return xCardTender;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "CardTenderLineItem.ToXml", ex);
                }

                throw;
            }          
        }

        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> classVariables = xItem.Elements();
                    foreach (XElement xVariable in classVariables)
                    {
                        if (!xVariable.IsEmpty)
                        {
                            try
                            {
                                switch (xVariable.Name.ToString())
                                {
                                    case "cardNumber":
                                        CardNumber = xVariable.Value;
                                        break;
                                    case "cardnumberHidden":
                                        CardNumberHidden = Conversion.ToBool(xVariable.Value);
                                        break;
                                    case "cardName":
                                        CardName = xVariable.Value;
                                        break;
                                    case "cardTypeId":
                                        CardTypeID = xVariable.Value;
                                        break;
                                    case "issuerName":
                                        IssuerName = xVariable.Value;
                                        break;
                                    case "expiryDate":
                                        ExpiryDate = xVariable.Value;
                                        break;
                                    case "cardHolderName":
                                        CardHolderName = xVariable.Value;
                                        break;
                                    case "cardEntryType":
                                        CardEntryType = (CardEntryTypes)Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "authorizationType":
                                        AuthorizationType = (AuthorizationTypes)Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "cardBin":
                                        CardBin = xVariable.Value;
                                        break;
                                    case "merchantId": 
                                        MerchantID = xVariable.Value;
                                        break;
                                    case "authCode":
                                        AuthCode = xVariable.Value;
                                        break;
                                    case "message": 
                                        Message = xVariable.Value;
                                        break;
                                    case "responseCode":
                                        ResponseCode = xVariable.Value;
                                        break;
                                    case "eftInfo":
                                        eftInfo.ToClass(xVariable,errorLogger);
                                        break;
                                    default:
                                        base.ToClass(xVariable,errorLogger);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, "CardTenderLineItem:" + xVariable.Name, ex);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "CardTenderLineItem.ToClass", ex);
                }

                throw;
            }
        }
    }
}
