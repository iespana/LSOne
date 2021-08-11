using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.Enums.EFT;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System.Collections.Generic;
using System.Xml.Linq;

namespace LSOne.Services.Interfaces.SupportInterfaces.EFT
{
    public interface IEFTInfo
    {
        string AcquirerName { get; set; }
        decimal Amount { get; set; }
        decimal AmountInCents { get; set; }
        string AuthCode { get; set; }
        bool Authorized { get; set; }
        string AuthorizedText { get; set; }
        string AuthSourceCode { get; set; }
        string AuthString { get; set; }
        string BatchCode { get; set; }
        long BatchNumber { get; set; }
        CardEntryTypesEnum CardEntryType { get; set; }
        CardInfo CardInformation { get; set; }
        string CardName { get; set; }
        string CardNumber { get; set; }
        bool CardNumberHidden { get; set; }
        EFTCardTypes CardType { get; set; }
        RecordIdentifier CardTypeId { get; set; }
        string EntrySourceCode { get; set; }
        string ErrorCode { get; set; }
        string EuropayAuthCode { get; set; }
        string ExpDate { get; set; }
        List<string> ExternalCardReceipts { get; set; }
        RecordIdentifier ID { get; set; }
        string IssuerName { get; set; }
        RecordIdentifier IssuerNumber { get; set; }
        RecordIdentifier LineNumber { get; set; }
        string MerchantNumber { get; set; }
        string Message { get; set; }
        string NotAuthorizedText { get; set; }
        string PreviousSequenceCode { get; set; }
        bool ProcessLocally { get; set; }
        string ReceiptLines { get; set; }
        EFTReceiptPrinting ReceiptPrinting { get; set; }
        string ReferralTelNumber { get; set; }
        string ResponseCode { get; set; }
        string SequenceCode { get; set; }
        byte[] Signature { get; set; }
        RecordIdentifier StaffId { get; set; }
        RecordIdentifier StoreID { get; set; }
        string TenderType { get; set; }
        RecordIdentifier TerminalID { get; set; }
        RecordIdentifier TerminalIDCardVendor { get; set; }
        RecordIdentifier TerminalNumber { get; set; }
        int Timeout { get; set; }
        string Track2 { get; set; }
        string TrackSeperator { get; set; }
        Date TransactionDateTime { get; set; }
        RecordIdentifier TransactionID { get; set; }
        int TransactionNumber { get; set; }
        TransactionType TransactionType { get; set; }
        string VisaAuthCode { get; set; }

        /// <summary>
        /// Gets or sets the instance of <see cref="IEFTExtraInfo"/>
        /// </summary>
        IEFTExtraInfo EFTExtraInfo { get; set; }

        /// <summary>
        /// The element containing the serialized <see cref="IEFTExtraInfo"/>
        /// </summary>
        XElement EFTExtraInfoXElement { get; }

        void RebuildExtraInfoXElement();

        object Clone();
        bool ShouldPrint(EFTReceiptPrinting receiptPrinting);
        void ToClass(XElement xItem, IErrorLog errorLogger = null);
        XElement ToXML(IErrorLog errorLogger = null);
    }
}