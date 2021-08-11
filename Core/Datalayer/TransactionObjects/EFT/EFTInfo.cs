#if !MONO
#endif
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.Enums.EFT;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using LSOne.Utilities.Attributes;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.TransactionObjects.EFT
{

    /// <summary>
    /// EFTInfo; serializable class.
    /// </summary>
    [Serializable]
    public class EFTInfo : DataEntity, ICloneable, IEFTInfo
    {
        /// <summary>
        /// A list of card receipts aquired from an external EFT terminal to be printed at the end of the transaction.
        /// </summary>
        private List<string> externalCardReceipts;

        private XElement eftExtraInfoXElement;

        public EFTInfo()
        {
            TransactionID = RecordIdentifier.Empty;
            LineNumber = RecordIdentifier.Empty;
            TerminalID = RecordIdentifier.Empty;
            Timeout = 0;
            MerchantNumber = "";
            TerminalNumber = RecordIdentifier.Empty;
            StoreID = RecordIdentifier.Empty;
            TerminalIDCardVendor = RecordIdentifier.Empty;
            Track2 = "";
            CardNumber = "";
            ExpDate = "";
            CardName = "";
            Amount = 0;
            AmountInCents = 0;
            StaffId = RecordIdentifier.Empty;
            TransactionNumber = 0;
            AuthCode = "";
            TenderType = "";
            AcquirerName = "";
            IssuerNumber = RecordIdentifier.Empty;
            CardTypeId = RecordIdentifier.Empty;
            BatchNumber = 0;
            ResponseCode = "";
            AuthSourceCode = "";
            EntrySourceCode = "";
            AuthString = "";
            VisaAuthCode = "";
            EuropayAuthCode = "";
            BatchCode = "";
            SequenceCode = "";
            NotAuthorizedText = "";
            AuthorizedText = "";
            Message = "";
            IssuerName = "";
            ErrorCode = "";
            ReferralTelNumber = "";
            TrackSeperator = "-";
            PreviousSequenceCode = "";
            TransactionDateTime = new Date();
            externalCardReceipts = new List<string>();
            ReceiptPrinting = EFTReceiptPrinting.All;
            Signature = null;

            CardInformation = new CardInfo();            
        }

        #region Properties
        /// <summary>
        /// TransactionID, LineNumber, Terminal, Store
        /// </summary>
        [RecordIdentifierConstruction(typeof(string), typeof(string), typeof(string), typeof(string))]
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(TransactionID, new RecordIdentifier(LineNumber, new RecordIdentifier(TerminalID, StoreID)));
            }
            set
            {
                TransactionID = value.PrimaryID;
                LineNumber = value.SecondaryID.PrimaryID;
                TerminalID = value.SecondaryID.SecondaryID.PrimaryID;
                StoreID = value.SecondaryID.SecondaryID.SecondaryID.PrimaryID;
            }
        }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TransactionID { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TerminalID { get; set; }
        public RecordIdentifier LineNumber { get; set; }
        /// <summary>
        /// The number of seconds to wait for the EFT Server
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// A unique merchant number from VISA/Europay.  Can be read after processing the transaction.
        /// </summary>
        [StringLength(60)]
        public string MerchantNumber { get; set; }

        /// <summary>
        /// Identification of the terminal making the transaction request.
        /// </summary>
        [RecordIdentifierValidation(60)]
        public RecordIdentifier TerminalNumber { get; set; }

        /// <summary>
        /// A unique terminal id from Visa/Europay. Can be read after processing the transaction.
        /// </summary>
        [RecordIdentifierValidation(60)]
        public RecordIdentifier TerminalIDCardVendor { get; set; }

        /// <summary>
        /// The store number (Read only)
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StoreID { get; set; }

        /// <summary>
        /// Track2 of the magnetic stripe of the card - if the card was swept through a card reader
        /// </summary>
        [StringLength(60)]
        public string Track2 { get; set; }

        /// <summary>
        /// The card number if the information was entered manually.  After the transaction has been processed this property holds the card number.
        /// </summary>
        [StringLength(60)]
        public string CardNumber { get; set; }

        /// <summary>
        /// Is the cardnumber hidden, and therefor the POS assumes it needs to ask for the cardnumber
        /// </summary>
        public bool CardNumberHidden { get; set; }

        /// <summary>
        /// The expirydate if the information was entered manually.  After the transaction has been processed this property holds the exp. date.
        /// </summary>
        [StringLength(10)]
        public string ExpDate { get; set; }

        /// <summary>
        /// How did we receive the card number?  Manually or by sweeping the card?
        /// </summary>
        public CardEntryTypesEnum CardEntryType { get; set; }

        /// <summary>
        /// The card name can be read from this property after the transaction, e.g. to be printed on a receipt.
        /// </summary>
        [StringLength(60)]
        public string CardName { get; set; }

        /// <summary>
        /// Amount of the transaction without decimals. NOTE!!! Either use this variable (amount) or amountInCents.  Not both!
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Amount of the transaction with 2 decimals, that is amount * 100.  NOTE!!! Either use this variable (amountInCents) or amount.  Not both!
        /// </summary>
        public decimal AmountInCents { get; set; }
        /// <summary>
        /// // Id of the staff/operator if you want to link the operator to the transaction.
        /// </summary>
        [RecordIdentifierValidation(60)]
        public RecordIdentifier StaffId { get; set; }

        /// <summary>
        /// For a normal transaction the transaction number can be read after the transaction. 
        /// If this is a void transaction this property has to contain the transaction number to void.
        /// </summary>
        public int TransactionNumber { get; set; }

        /// <summary>
        /// True if the transaction was authorized and false if not authorized.
        /// </summary>
        public bool Authorized { get; set; }

        /// <summary>
        /// For normal transaction the authentication code can be read from this property.  
        /// When manually calling for an authenication number we must place it in this property before requesting authorication.
        /// </summary>
        [StringLength(60)]
        public string AuthCode { get; set; }

        /// <summary>
        /// Read this field to see what kind of card was used.
        /// </summary>
        public EFTCardTypes CardType { get; set; }

        /// <summary>
        /// The tender type for the cardName
        /// </summary>
        [StringLength(20)]
        public string TenderType { get; set; }

        /// <summary>
        /// Read this property to print the acquirer on the sales slip.
        /// </summary>
        [StringLength(60)]
        public string AcquirerName { get; set; }

        /// <summary>
        /// The date and time of the transaction
        /// </summary>
        public Date TransactionDateTime { get; set; }

        /// <summary>
        /// The issuer number
        /// </summary>
        public RecordIdentifier IssuerNumber { get; set; }

        /// <summary>
        /// the Card type id
        /// </summary>
        public RecordIdentifier CardTypeId { get; set; }

        /// <summary>
        /// The current batch-number for the terminal.  Can be read after each transaction.
        /// </summary>
        public long BatchNumber { get; set; }

        /// <summary>
        /// A zero filled from the left 6 digit number used. 
        /// </summary>
        [StringLength(60)]
        public string AuthString { get; set; }

        /// <summary>
        /// The contract number used for Visa.
        /// </summary>
        [StringLength(60)]
        public string VisaAuthCode { get; set; }

        /// <summary>
        /// The contract number used for EuroPay.
        /// </summary>
        [StringLength(60)]
        public string EuropayAuthCode { get; set; }

        /// <summary>
        /// A concatinated string specially used for receipt.
        /// 7 letter batch code
        /// </summary>
        [StringLength(60)]
        public string BatchCode { get; set; }

        /// <summary>
        /// A concatenated sequence code (seven letter - batch code) specially used for receipt.
        /// 7 letter batch code
        /// </summary>
        [StringLength(60)]
        public string SequenceCode { get; set; }

        //--------------------------------

        /// <summary>
        /// The response code from Point.
        /// </summary>
        [StringLength(60)]
        public string ResponseCode { get; set; }

        /// <summary>
        /// Sets or returns the current value as described in the Enum TransactionType.
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// Initialized to be empty ("").
        /// </summary>
        [StringLength(60)]
        public string Message { get; set; }

        /// <summary>
        /// Initialized to be empty ("").
        /// </summary>
        [StringLength(60)]
        public string IssuerName { get; set; }

        /// <summary>
        /// Initialized to be empty ("").
        /// </summary>
        [StringLength(60)]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Initialized to be empty ("").
        /// </summary>
        [StringLength(60)]
        public string ReferralTelNumber { get; set; }

        /// <summary>
        /// Specifies how the authorization code was generated. Initial value: "".
        /// </summary>
        [StringLength(60)]
        public string AuthSourceCode { get; set; }

        /// <summary>
        /// Was the card swiped or manually entered?
        /// </summary>
        [StringLength(60)]
        public string EntrySourceCode { get; set; }

        /// <summary>
        /// Whether the card should be processed locally and not sent to the EFT service provider.
        /// </summary>
        public bool ProcessLocally { get; set; }

        /// <summary>
        /// Initialized to be empty ("").
        /// </summary>
        [StringLength(60)]
        public string NotAuthorizedText { get; set; }

        /// <summary>
        /// Initialized to be empty ("").
        /// </summary>
        [StringLength(60)]
        public string AuthorizedText { get; set; }

        /// <summary>
        /// Initialized to "-".
        /// </summary>
        [StringLength(60)]
        public string TrackSeperator { get; set; }

        /// <summary>
        /// Initialized to be empty ("").
        /// </summary>
        [StringLength(60)]
        public string PreviousSequenceCode { get; set; }

        /// <summary>
        /// A list of card receipts aquired from an external EFT terminal to be printed at the end of the transaction.
        /// </summary>
        public List<string> ExternalCardReceipts
        {
            get { return externalCardReceipts; }
            set { externalCardReceipts = value; }
        }

        /// <summary>
        /// Signature captured by a signature capable terminal
        /// </summary>
        public byte[] Signature { get; set; }

        /// <summary>
        /// Receipt lines provided by the EFT provider
        /// </summary>
        public string ReceiptLines { get; set; }

        public EFTReceiptPrinting ReceiptPrinting { get; set; }

        public CardInfo CardInformation { get; set; }
        public IEFTExtraInfo EFTExtraInfo { get; set; }
        public XElement EFTExtraInfoXElement => eftExtraInfoXElement;

        public override object Clone()
        {
            var info = new EFTInfo();
            Populate(info);
            return info;
        }

        protected virtual void Populate(EFTInfo info)
        {
            info.TransactionID = (RecordIdentifier)TransactionID.Clone();
            info.LineNumber = (RecordIdentifier)LineNumber.Clone();
            info.TerminalID = (RecordIdentifier)TerminalID.Clone();
            info.StoreID = (RecordIdentifier)StoreID.Clone();
            info.Timeout = Timeout;
            info.MerchantNumber = MerchantNumber;
            info.TerminalNumber = (RecordIdentifier)TerminalNumber.Clone();
            info.StoreID = StoreID;
            info.TerminalIDCardVendor = (RecordIdentifier)TerminalIDCardVendor.Clone();
            info.Track2 = Track2;
            info.CardNumber = CardNumber;
            info.CardNumberHidden = CardNumberHidden;
            info.ExpDate = ExpDate;
            info.CardEntryType = CardEntryType;
            info.CardName = CardName;
            info.Amount = Amount;
            info.AmountInCents = AmountInCents;
            info.StaffId = (RecordIdentifier)StaffId.Clone();
            info.TransactionNumber = TransactionNumber;
            info.Authorized = Authorized;
            info.AuthCode = AuthCode;
            info.CardType = CardType;
            info.TenderType = TenderType;
            info.AcquirerName = AcquirerName;
            info.TransactionDateTime = TransactionDateTime;
            info.IssuerNumber = (RecordIdentifier)IssuerNumber.Clone();
            info.CardTypeId = (RecordIdentifier)CardTypeId.Clone();
            info.BatchNumber = BatchNumber;
            info.ResponseCode = ResponseCode;
            info.AuthSourceCode = AuthSourceCode;
            info.EntrySourceCode = EntrySourceCode;
            info.ProcessLocally = ProcessLocally;
            info.AuthString = AuthString;
            info.VisaAuthCode = VisaAuthCode;
            info.EuropayAuthCode = EuropayAuthCode;
            info.BatchCode = BatchCode;
            info.SequenceCode = SequenceCode;
            info.NotAuthorizedText = NotAuthorizedText;
            info.AuthorizedText = AuthorizedText;
            info.TransactionType = TransactionType;
            info.Message = Message;
            info.IssuerName = IssuerName;
            info.ErrorCode = ErrorCode;
            info.ReferralTelNumber = ReferralTelNumber;
            info.TrackSeperator = TrackSeperator;
            info.PreviousSequenceCode = PreviousSequenceCode;
            info.externalCardReceipts = CollectionHelper.Clone<string, List<string>>(externalCardReceipts);
            info.Signature = Signature;
            info.ReceiptLines = ReceiptLines;
            info.ReceiptPrinting = ReceiptPrinting;
            info.CardInformation = (CardInfo)CardInformation.Clone();
            info.eftExtraInfoXElement = EFTExtraInfoXElement != null ? new XElement(EFTExtraInfoXElement) : EFTExtraInfo?.ToXml();
        }

        #endregion

        /// <summary>
        /// XMLs a string list
        /// </summary>
        /// <param name="strLinkedList">The string list to XML</param>
        /// <returns>An XElement that represents the string list</returns>
        private XElement ListToXML(List<string> strLinkedList)
        {
            var xLinked = new XElement("LinkedStrList");

            if (strLinkedList == null)
                return xLinked;

            foreach (string str in strLinkedList)
            {
                xLinked.Add(new XElement("str", Conversion.ToXmlString(str)));
            }

            return xLinked;
        }

        /// <summary>
        /// Creates a string list out of an XElement
        /// </summary>
        /// <param name="xItems">The XML element</param>
        /// <param name="errorLogger">Means to log potential errors.</param>
        /// <returns>Returns the string list created from the XML</returns>
        private List<string> ListToClass(XElement xItems, IErrorLog errorLogger = null)
        {
            var strList = new List<string>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> strings = xItems.Elements("LinkedStrList");
                foreach (XElement xVariable in strings)
                {
                    if (xVariable.HasElements)
                    {
                        IEnumerable<XElement> linkedStrings = xVariable.Elements();
                        foreach (XElement strElem in linkedStrings)
                        {
                            if (!strElem.IsEmpty)
                            {
                                try
                                {
                                    switch (strElem.Name.ToString())
                                    {
                                        case "str":
                                            string newStr = strElem.Value;
                                            strList.Add(newStr);
                                            break;
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }
            return strList;
        }
        /// <summary>
        /// XMLs the EFTInfo objects
        /// </summary>
        /// <param name="errorLogger">Means to log potential errors.</param>
        /// <returns></returns>
        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            /*
            * Strings      added as is
            * Int          added as is
            * Bool         added as is
            * 
            * Decimal      added with ToString()
            * DateTime     added with ToString()
            * 
            * Enums        added with an (int) cast   
            * 
           */
            try
            {
                return new XElement("eftInfo",
                    new XElement("timeout", Timeout),
                    new XElement("merchantNumber", MerchantNumber),
                    new XElement("terminalNumber", TerminalNumber),
                    new XElement("terminalId", (string)TerminalIDCardVendor),
                    new XElement("storeNumber", StoreID),
                    new XElement("track2", Track2),
                    new XElement("cardNumber", CardNumber),
                    new XElement("cardNumberHidden", CardNumberHidden),
                    new XElement("expDate", ExpDate),
                    new XElement("cardEntryType", (int)CardEntryType),
                    new XElement("cardName", CardName),
                    new XElement("amount", Amount.ToString()),
                    new XElement("amountInCents", AmountInCents.ToString()),
                    new XElement("staffId", StaffId),
                    new XElement("transactionNumber", TransactionNumber),
                    new XElement("authorized", Authorized),
                    new XElement("authCode", AuthCode),
                    new XElement("cardType", (int)CardType),
                    new XElement("tenderType", TenderType),
                    new XElement("acquirerName", AcquirerName),
                    new XElement("transactionDateTime", TransactionDateTime.ToXmlString()),
                    new XElement("issuerNumber", IssuerNumber),
                    new XElement("cardTypeId", CardTypeId),
                    new XElement("batchNumber", BatchNumber),
                    new XElement("responseCode", ResponseCode),
                    new XElement("authSourceCode", AuthSourceCode),
                    new XElement("entrySourceCode", EntrySourceCode),
                    new XElement("processLocally", ProcessLocally),
                    new XElement("authString", AuthString),
                    new XElement("visaAuthCode", VisaAuthCode),
                    new XElement("europayAuthCode", EuropayAuthCode),
                    new XElement("batchCode", BatchCode),
                    new XElement("sequenceCode", SequenceCode),
                    new XElement("notAuthorizedText", NotAuthorizedText),
                    new XElement("authorizedText", AuthorizedText),
                    new XElement("transactionType", (int)TransactionType),
                    new XElement("message", Message),
                    new XElement("issuerName", IssuerName),
                    new XElement("errorCode", ErrorCode),
                    new XElement("referralTelNumber", ReferralTelNumber),
                    new XElement("trackSeperator", TrackSeperator),
                    new XElement("previousSequenceCode", PreviousSequenceCode),
                    new XElement("cardInformation", CardInformation.ToXML()),
                    new XElement("externalCardReceipts", ListToXML(externalCardReceipts)),
                    new XElement("signature", Signature == null ? "" : Convert.ToBase64String(Signature)),
                    new XElement("receiptLines", ReceiptLines),
                    new XElement("receiptPrinting", (int)ReceiptPrinting),
                    new XElement("eftExtraInfo", EFTExtraInfo?.ToXml())
                );
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "EFTInfo.ToXML", ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Creates an EFTInfo class from an XML
        /// </summary>
        /// <param name="xItem">The XML element to create the class from</param>
        /// <param name="errorLogger">Means to log potential errors.</param>
        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
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
                                case "timeout":
                                    Timeout = Convert.ToInt32(xVariable.Value);
                                    break;
                                case "merchantNumber":
                                    MerchantNumber = xVariable.Value;
                                    break;
                                case "terminalNumber":
                                    TerminalNumber = xVariable.Value;
                                    break;
                                case "terminalId":
                                    TerminalIDCardVendor = xVariable.Value;
                                    break;
                                case "storeNumber":
                                    StoreID = xVariable.Value;
                                    break;
                                case "track2":
                                    Track2 = xVariable.Value;
                                    break;
                                case "cardNumber":
                                    CardNumber = xVariable.Value;
                                    break;
                                case "cardNumberHidden":
                                    CardNumberHidden = ToBool(xVariable.Value);
                                    break;
                                case "expDate":
                                    ExpDate = xVariable.Value;
                                    break;
                                case "cardEntryType":
                                    CardEntryType = (CardEntryTypesEnum)Convert.ToInt32(xVariable.Value);
                                    break;
                                case "cardName":
                                    CardName = xVariable.Value;
                                    break;
                                case "amount":
                                    Amount = Convert.ToDecimal(xVariable.Value);
                                    break;
                                case "amountInCents":
                                    AmountInCents = Convert.ToDecimal(xVariable.Value);
                                    break;
                                case "staffId":
                                    StaffId = xVariable.Value;
                                    break;
                                case "transactionNumber":
                                    TransactionNumber = Convert.ToInt32(xVariable.Value);
                                    break;
                                case "authorized":
                                    Authorized = ToBool(xVariable.Value);
                                    break;
                                case "authCode":
                                    AuthCode = xVariable.Value;
                                    break;
                                case "cardType":
                                    CardType = (EFTCardTypes)Convert.ToInt32(xVariable.Value);
                                    break;
                                case "tenderType":
                                    TenderType = xVariable.Value;
                                    break;
                                case "acquirerName":
                                    AcquirerName = xVariable.Value;
                                    break;
                                case "transactionDateTime":
                                    TransactionDateTime = new Date(Conversion.XmlStringToDateTime(xVariable.Value));
                                    break;
                                case "issuerNumber":
                                    IssuerNumber = Conversion.ToInt(xVariable.Value);
                                    break;
                                case "cardTypeId":
                                    CardTypeId = xVariable.Value;
                                    break;
                                case "batchNumber":
                                    BatchNumber = Convert.ToInt64(xVariable.Value);
                                    break;
                                case "responseCode":
                                    ResponseCode = xVariable.Value;
                                    break;
                                case "authSourceCode":
                                    AuthSourceCode = xVariable.Value;
                                    break;
                                case "entrySourceCode":
                                    EntrySourceCode = xVariable.Value;
                                    break;
                                case "processLocally":
                                    ProcessLocally = ToBool(xVariable.Value);
                                    break;
                                case "authString":
                                    AuthString = xVariable.Value;
                                    break;
                                case "visaAuthCode":
                                    VisaAuthCode = xVariable.Value;
                                    break;
                                case "europayAuthCode":
                                    EuropayAuthCode = xVariable.Value;
                                    break;
                                case "batchCode":
                                    BatchCode = xVariable.Value;
                                    break;
                                case "sequenceCode":
                                    SequenceCode = xVariable.Value;
                                    break;
                                case "notAuthorizedText":
                                    NotAuthorizedText = xVariable.Value;
                                    break;
                                case "authorizedText":
                                    AuthorizedText = xVariable.Value;
                                    break;
                                case "transactionType":
                                    TransactionType = (TransactionType)Convert.ToInt32(xVariable.Value);
                                    break;
                                case "message":
                                    Message = xVariable.Value;
                                    break;
                                case "issuerName":
                                    IssuerName = xVariable.Value;
                                    break;
                                case "errorCode":
                                    ErrorCode = xVariable.Value;
                                    break;
                                case "referralTelNumber":
                                    ReferralTelNumber = xVariable.Value;
                                    break;
                                case "trackSeperator":
                                    TrackSeperator = xVariable.Value;
                                    break;
                                case "previousSequenceCode":
                                    PreviousSequenceCode = xVariable.Value;
                                    break;
                                case "cardInformation":
                                    CardInformation.ToClass(xVariable);
                                    break;
                                case "externalCardReceipts":
                                    externalCardReceipts = ListToClass(xVariable);
                                    break;
                                case "signature":
                                    if (string.IsNullOrEmpty(xVariable.Value))
                                        Signature = null;
                                    else
                                        Signature = Convert.FromBase64String(xVariable.Value);
                                    break;
                                case "receiptLines":
                                    ReceiptLines = xVariable.Value;
                                    break;
                                case "receiptPrinting":
                                    ReceiptPrinting = (EFTReceiptPrinting)Convert.ToInt32(xVariable.Value);
                                    break;
                                case "eftExtraInfo":
                                    eftExtraInfoXElement = xVariable;
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, xVariable.ToString(), ex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strBool"></param>
        /// <returns></returns>
        private bool ToBool(string strBool)
        {
            return (strBool == "true");
        }

        public void RebuildExtraInfoXElement()
        {
            if(EFTExtraInfo != null)
            {
                eftExtraInfoXElement = EFTExtraInfo.ToXml();
            }
        }

        public bool ShouldPrint(EFTReceiptPrinting receiptPrinting)
        {
            return ReceiptPrinting != EFTReceiptPrinting.None &&
                   (ReceiptPrinting == EFTReceiptPrinting.All ||
                    ((ReceiptPrinting & receiptPrinting) == receiptPrinting));
        }
    }
}
