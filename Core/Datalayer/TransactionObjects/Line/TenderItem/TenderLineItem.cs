using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.TenderItem
{
    /// <summary>
    /// Is used as a basic class for other tender types and for cash payments.
    /// </summary>
    [Serializable]
    public class TenderLineItem : LineItem, ITenderLineItem
    {
        protected TenderTypeEnum internalTenderType;

        public TenderTypeEnum TypeOfTender
        {
            get { return internalTenderType; }
        }


        /// <summary>
        /// If true then this is a deposit line that has already been paid and does not need to be saved again
        /// </summary>
        public bool PaidDeposit { get; set; }

        /// <summary>
        /// Has the points that were calculated for this specific line item
        /// </summary>
        public ILoyaltyItem LoyaltyPoints { get; set; }

        /// <summary>
        /// The transaction this line belongs to
        /// </summary>
        public IPosTransaction Transaction { get; set; }

        /// <summary>
        /// The id for the Tendertype
        /// </summary>
        [StringLength(20)]
        public string TenderTypeId { get; set; }

        /// <summary>
        /// Payment amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The amount in a foreign currency.
        /// </summary>
        public decimal ForeignCurrencyAmount { get; set; }

        /// <summary>
        /// The amount in the company currency as calculated from the entry in CompanyInfo table.
        /// </summary>
        public decimal CompanyCurrencyAmount { get; set; }

        /// <summary>
        /// The exchange rate between any paid amount and the company currency.
        /// </summary>
        public decimal ExchrateMST { get; set; }

        /// <summary>
        /// The exchange rate used to calculate the amount.
        /// </summary>
        public decimal ExchangeRate { get; set; }         
   
        /// <summary>
        /// The code to identify the currency.
        /// </summary>
        [StringLength(3)]
        public string CurrencyCode { get; set; }
            
        /// <summary>
        /// Should the drawer be opened?
        /// </summary>
        public bool OpenDrawer { get; set; }
               
        /// <summary>
        /// If 0 > change - amount >= minimumChangeAmount, then
        /// </summary>     
        public string ChangeTenderID { get; set; }
            
        /// <summary>
        /// changeTenderID will be used as changeID, else tenderTypeID will be used 
        /// </summary>
        public decimal MinimumChangeAmount { get; set; }

        public string AboveMinimumTenderId { get; set; }

        /// <summary>
        /// Is the tender item a change back tender line - needed for Return Transactions
        /// </summary>                                     
        public bool ChangeBack { get; set; }

        /// <summary>
        /// Infocode comment on the tender line
        /// </summary>
        public string Comment { get; set; }                     //Comment field used for infocode comments

        /// <summary>
        /// The amount that the POS has calculated that should be in the drawer
        /// </summary>
        public decimal ExpectedTenderDeclarationAmount { get; set; }

        /// <inheritdoc/>
        public Guid ID { get; set; }
        
        
        public TenderLineItem() : base()
        {
            internalTenderType = TenderTypeEnum.TenderLine;

            TenderTypeId = string.Empty;
            ForeignCurrencyAmount = 0;
            CompanyCurrencyAmount = 0;
            ExchrateMST = 0;
            ExchangeRate = 0;
            CurrencyCode = string.Empty;
            ChangeTenderID = string.Empty;
            MinimumChangeAmount = 0;
            AboveMinimumTenderId = string.Empty;
            ChangeBack = false;
            Comment = string.Empty;
            LoyaltyPoints = new LoyaltyItem();
            PaidDeposit = false;
            ID = Guid.NewGuid();
        }

        protected void Populate(TenderLineItem item)
        {
            base.Populate(item);
            item.Transaction = Transaction;
            item.TenderTypeId = TenderTypeId;
            item.Amount = Amount;
            item.ForeignCurrencyAmount = ForeignCurrencyAmount;
            item.CompanyCurrencyAmount = CompanyCurrencyAmount;
            item.ExchangeRate = ExchangeRate;
            item.ExchrateMST = ExchrateMST;
            item.CurrencyCode = CurrencyCode;
            item.OpenDrawer = OpenDrawer;
            item.ChangeTenderID = ChangeTenderID;
            item.MinimumChangeAmount = MinimumChangeAmount;
            item.AboveMinimumTenderId = AboveMinimumTenderId;
            item.ChangeBack = ChangeBack;
            item.Comment = Comment;
            item.ExpectedTenderDeclarationAmount = ExpectedTenderDeclarationAmount;
            item.LoyaltyPoints = (LoyaltyItem)LoyaltyPoints.Clone();
            item.PaidDeposit = PaidDeposit;
            item.internalTenderType = TypeOfTender;
            item.ID = ID;
        }
        
        public override object Clone()
        {
            var item = new TenderLineItem();
            Populate(item);
            return item;
        }
        
        private List<InfoCodeLineItem> CreateInfocodeLines(XElement xItems)
        {
            var InfocodeLines = new List<InfoCodeLineItem>();

            if (xItems.HasElements)
            {
                var xInfocodeItems = xItems.Elements("InfoCodeLineItem");
                foreach (var xInfocodeItem in xInfocodeItems)
                {
                    if (xInfocodeItem.HasElements)
                    {
                        var newInfocode = new InfoCodeLineItem();
                        newInfocode.ToClass(xInfocodeItem);
                        InfocodeLines.Add(newInfocode);
                    }
                }
            }
            return InfocodeLines;
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
                                    case "PaidDeposit":
                                        PaidDeposit = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "tenderTypeId":
                                        TenderTypeId = xVariable.Value;
                                        break;
                                    case "amount":
                                        Amount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "foreignCurrencyAmount":
                                        ForeignCurrencyAmount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "companyCurrencyAmount":
                                        CompanyCurrencyAmount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "exchrateMST":
                                        ExchrateMST = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "exchangeRate":
                                        ExchangeRate = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "currencyCode":
                                        CurrencyCode = xVariable.Value;
                                        break;
                                    case "openDrawer":
                                        OpenDrawer = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "changeTenderID":
                                        ChangeTenderID = xVariable.Value;
                                        break;
                                    case "minimumChangeAmount":
                                        MinimumChangeAmount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "aboveMinimumTenderId":
                                        AboveMinimumTenderId = xVariable.Value;
                                        break;
                                    case "changeBack":
                                        ChangeBack = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "comment":
                                        Comment = xVariable.Value;
                                        break;
                                    case "InfocodeLines":
                                        InfoCodeLines = CreateInfocodeLines(xVariable);
                                        break;
                                    case "ExpectedTenderDeclarationAmount":
                                        ExpectedTenderDeclarationAmount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "ID":
                                        ID = Conversion.XmlStringToGuid(xVariable.Value);
                                        break;
                                    case LoyaltyItem.XmlElementName:
                                        LoyaltyPoints.ToClass(xVariable);
                                        break;                                    
                                    default:
                                        base.ToClass(xVariable);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, "TenderLineItem:" + xVariable.Name, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "TenderLineItem.ToClass", ex);

                throw;
            }
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
                XElement xTender = new XElement("TenderLineItem",                 
                    new XElement("tenderTypeId", TenderTypeId),
                    new XElement("PaidDeposit", Conversion.ToXmlString(PaidDeposit)),
                    new XElement("amount", Conversion.ToXmlString(Amount)),
                    new XElement("foreignCurrencyAmount", Conversion.ToXmlString(ForeignCurrencyAmount)),
                    new XElement("companyCurrencyAmount", Conversion.ToXmlString(CompanyCurrencyAmount)),
                    new XElement("exchrateMST", Conversion.ToXmlString(ExchrateMST)),
                    new XElement("exchangeRate", Conversion.ToXmlString(ExchangeRate)),
                    new XElement("currencyCode", CurrencyCode),
                    new XElement("openDrawer", Conversion.ToXmlString(OpenDrawer)),
                    new XElement("changeTenderID", ChangeTenderID),
                    new XElement("minimumChangeAmount", Conversion.ToXmlString(MinimumChangeAmount)),
                    new XElement("aboveMinimumTenderId", AboveMinimumTenderId),
                    new XElement("changeBack", Conversion.ToXmlString(ChangeBack)),
                    new XElement("comment", Comment),
                    new XElement("ExpectedTenderDeclarationAmount", Conversion.ToXmlString(ExpectedTenderDeclarationAmount)),
                    new XElement("ID", Conversion.ToXmlString(ID)),
                    LoyaltyPoints.ToXML()
                );

                #region Infocodes
                XElement xInfocodes = new XElement("InfocodeLines");
                foreach (InfoCodeLineItem ici in InfoCodeLines)
                {
                    xInfocodes.Add(ici.ToXML());
                }
                xTender.Add(xInfocodes);
                #endregion

                xTender.Add(base.ToXML());
                return xTender;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "TenderLineItem.ToXml", ex);

                throw;
            }
        }
    }
}
