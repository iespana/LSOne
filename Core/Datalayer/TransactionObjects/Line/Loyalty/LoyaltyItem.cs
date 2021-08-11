using System;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.Loyalty
{
    [Serializable]
    public class LoyaltyItem : ILoyaltyItem
    {
        public const string XmlElementName = "LoyaltyItem";
        private bool recalculate;

        public LoyaltyItem()
        {
            Clear();
        }

        #region Properties

        /// <summary>
        /// The transaction ID of the loyalty points
        /// </summary>
        public RecordIdentifier TransactionID { get; set; }
        /// <summary>
        /// The Store ID where the points were created
        /// </summary>
        public RecordIdentifier StoreID { get; set; }
        /// <summary>
        /// The Terminal ID where the points were created
        /// </summary>
        public RecordIdentifier TerminalID { get; set; }
        /// <summary>
        /// The receipt ID of the sale where the points were created
        /// </summary>
        public RecordIdentifier ReceiptID { get; set; }
        /// <summary>
        /// The loyalty card number
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// The calculated points for the sale
        /// </summary>
        public decimal CalculatedPoints { get; set; }
        /// <summary>
        /// The aggregated quantity of the item (within the transaction) that created the points. 
        /// </summary>
        public decimal AggregatedItemQuantity { get; set; }

        /// <summary>
        /// If true then the points for this item are on another identical item in the sale
        /// </summary>
        public bool PartOfAggregatedItemPoints { get; set; }

        /// <summary>
        /// The previously accumulated points at the time when the sale was done
        /// </summary>
        public decimal AccumulatedPoints { get; set; }

        /// <summary>
        /// The current value of the accumulated points
        /// </summary>
        public decimal CurrentValue { get; set; }

        /// <summary>
        /// The amount that the calculated points represent
        /// </summary>
        public decimal CalculatedPointsAmount { get; set; }
        /// <summary>
        /// The scheme ID that was used to calculate the points
        /// </summary>
        public RecordIdentifier SchemeID { get; set; }
        /// <summary>
        /// The customer on the loyalty card
        /// </summary>
        public RecordIdentifier CustomerID { get; set; }
        /// <summary>
        /// The expire unit for the points (days, weeks, months, years)
        /// </summary>
        public TimeUnitEnum ExpireUnit { get; set; }
        /// <summary>
        /// The expire value for the points (1,2,3... and etc). Is always used with ExireUnit
        /// </summary>
        public int ExpireValue { get; set; }
        public decimal LineNum { get; set; }
        /// <summary>
        /// What type of points is this line i.e. transaction points (the sum of all items and tenders) or the points for an individual item or tender
        /// </summary>
        public LoyaltyPointsRelation Relation { get; set; }
        /// <summary>
        /// Is the points line voided?
        /// </summary>
        public TransactionStatus EntryStatus { get; set; }
        /// <summary>
        /// The calculated expiration date of the points
        /// </summary>
        public DateTime ExpirationDate { get; set; }
        public RecordIdentifier StaffID { get; set; }
        /// <summary>
        /// The date when the points were calculated
        /// </summary>
        public DateTime DateIssued { get; set; }
        /// <summary>
        /// The scheme rule that was used to calculate the points
        /// </summary>
        public RecordIdentifier RuleID { get; set; }
        /// <summary>
        /// If true then the loyalty points discount needs to be recalculated. If loyalty item isn't a discount item then no change is done
        /// </summary>
        public bool RecalculateDiscount
        {
            get
            {
                return recalculate;
            }
            set
            {
                recalculate = (Relation == LoyaltyPointsRelation.Discount) && value;
            }
        }
        /// <summary>
        /// If points are not zero this property is true otherwise it's false;
        /// </summary>
        public bool PointsAdded
        {
            get { return CalculatedPoints != decimal.Zero && !PartOfAggregatedItemPoints; }
        }

        /// <summary>
        /// If the loyalty item is empty this property is true otherwise it's false;
        /// </summary>
        public bool Empty
        {
            get { return CalculatedPoints == decimal.Zero && CardNumber == ""; }
        }

        /// <summary>
        /// True if scheme information was found and retrieved from the database 
        /// </summary>
        public bool SchemeExists
        {
            get { return ExpireUnit != TimeUnitEnum.None; }
        }

        /// <summary>
        /// Usage limit as percentage of transaction total
        /// </summary>
        public int UsePointsLimit { get; set; }

        #endregion

        public void Clear()
        {
            TransactionID = RecordIdentifier.Empty;
            StoreID = RecordIdentifier.Empty;
            TerminalID = RecordIdentifier.Empty;
            ReceiptID = RecordIdentifier.Empty;
            CardNumber = "";
            CalculatedPoints = decimal.Zero;
            AggregatedItemQuantity = 1M;
            AccumulatedPoints = decimal.Zero;
            CurrentValue = decimal.Zero;
            CalculatedPointsAmount = decimal.Zero;
            SchemeID = RecordIdentifier.Empty;
            CustomerID = RecordIdentifier.Empty;
            ExpireUnit = TimeUnitEnum.None;
            ExpireValue = 0;
            Relation = LoyaltyPointsRelation.Header;
            EntryStatus = TransactionStatus.Normal;
            ExpirationDate = DateTime.MinValue;
            StaffID = RecordIdentifier.Empty;
            DateIssued = DateTime.Now.Date;
            RuleID = RecordIdentifier.Empty;
            PartOfAggregatedItemPoints = false;
            RecalculateDiscount = false;
            UsePointsLimit = 0;
        }

        public virtual object Clone()
        {
            var item = new LoyaltyItem();
            Populate(item);
            return item;
        }

        /// <summary>
        /// Takes a LoyaltyItem and prepares it for a specific relation (tender, item or transaction)
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public ILoyaltyItem CopyForPointsRelation(LoyaltyPointsRelation relation)
        {
            var toCopy = (LoyaltyItem)this.Clone();
            ClearForRelation(toCopy, relation);
            return toCopy;
        }

        private void ClearForRelation(LoyaltyItem toCopy, LoyaltyPointsRelation relation)
        {
            toCopy.RuleID = RecordIdentifier.Empty;
            toCopy.CalculatedPoints = decimal.Zero;
            toCopy.Relation = relation;
        }

        protected void Populate(LoyaltyItem item)
        {
            item.TransactionID = TransactionID;
            item.StoreID = StoreID;
            item.TerminalID = TerminalID;
            item.ReceiptID = ReceiptID;
            item.CardNumber = CardNumber;
            item.CalculatedPoints = CalculatedPoints;
            item.AggregatedItemQuantity = AggregatedItemQuantity;
            item.AccumulatedPoints = AccumulatedPoints;
            item.CurrentValue = CurrentValue;
            item.CalculatedPointsAmount = CalculatedPointsAmount;
            item.SchemeID = SchemeID;
            item.CustomerID = CustomerID;            
            item.ExpireUnit = ExpireUnit;
            item.ExpireValue = ExpireValue;
            item.LineNum = LineNum;
            item.Relation = Relation;
            item.EntryStatus = EntryStatus;
            item.ExpirationDate = ExpirationDate;
            item.StaffID = StaffID;
            item.DateIssued = DateIssued;
            item.RuleID = RuleID;
            item.PartOfAggregatedItemPoints = PartOfAggregatedItemPoints;
            item.RecalculateDiscount = RecalculateDiscount;
            item.UsePointsLimit = UsePointsLimit;
        }

        public XElement ToXML(IErrorLog errorLogger = null)
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

                var xLoyalty = new XElement(XmlElementName,
                    new XElement("loyaltyCardNumber", CardNumber),
                    new XElement("calculatedLoyaltyPoints", Conversion.ToXmlString(CalculatedPoints)),
                    new XElement("accumulatedLoyaltyPoints", Conversion.ToXmlString(AccumulatedPoints)),
                    new XElement("currentValue", Conversion.ToXmlString(CurrentValue)),
                    new XElement("CalculatedPointsAmount", Conversion.ToXmlString(CalculatedPointsAmount)),
                    new XElement("schemeID", SchemeID),
                    new XElement("custID", CustomerID),                    
                    new XElement("expireUnit", Conversion.ToXmlString((int)ExpireUnit)),
                    new XElement("expireValue", Conversion.ToXmlString(ExpireValue)),
                    new XElement("Relation", Conversion.ToXmlString((int)Relation)),
                    new XElement("TransactionID", TransactionID),
                    new XElement("StoreID", StoreID),
                    new XElement("TerminalID", TerminalID),
                    new XElement("ReceiptID", ReceiptID),
                    new XElement("EntryStatus", Conversion.ToXmlString((int)EntryStatus)),
                    new XElement("ExpirationDate", Conversion.ToXmlString(ExpirationDate)),
                    new XElement("StaffID", StaffID),
                    new XElement("LineNum", Conversion.ToXmlString(LineNum)),
                    new XElement("DateIssued", Conversion.ToXmlString(DateIssued)),
                    new XElement("AggregatedItemQuantity", Conversion.ToXmlString(AggregatedItemQuantity)),
                    new XElement("RuleID", RuleID),
                    new XElement("RecalculateDiscount", Conversion.ToXmlString(RecalculateDiscount)),
                    new XElement("PartOfAggregatedItemPoints", Conversion.ToXmlString(PartOfAggregatedItemPoints)),
                    new XElement("UsePointsLimit", Conversion.ToXmlString(UsePointsLimit))
                );

                return xLoyalty;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, ex.Message, ex);
                throw;
            }
        }

        public void ToClass(XElement xLoyalty, IErrorLog errorLogger = null)
        {           
            try
            {
                if (xLoyalty.HasElements)
                {
                    var loyaltyElements = xLoyalty.Elements();
                    foreach (var loyalElem in loyaltyElements)
                    {                        
                        if (!loyalElem.IsEmpty)
                        {
                            try
                            {
                                switch (loyalElem.Name.ToString())
                                {
                                    case "loyaltyCardNumber":
                                        CardNumber = loyalElem.Value;
                                        break;
                                    case "calculatedLoyaltyPoints":
                                        CalculatedPoints = Conversion.XmlStringToDecimal(loyalElem.Value);
                                        break;
                                    case "accumulatedLoyaltyPoints":
                                        AccumulatedPoints = Conversion.XmlStringToDecimal(loyalElem.Value);
                                        break;
                                    case "currentValue":
                                        CurrentValue = Conversion.XmlStringToDecimal(loyalElem.Value);
                                        break;
                                    case "CalculatedPointsAmount":
                                        CalculatedPointsAmount = Conversion.XmlStringToDecimal(loyalElem.Value);
                                        break;
                                    case "schemeID":
                                        SchemeID = loyalElem.Value;
                                        break;
                                    case "custID":
                                        CustomerID = loyalElem.Value;
                                        break;                                   
                                    case "expireUnit":
                                        ExpireUnit = (TimeUnitEnum)Conversion.XmlStringToInt(loyalElem.Value);
                                        break;
                                    case "expireValue":
                                        ExpireValue = Conversion.XmlStringToInt(loyalElem.Value);
                                        break;
                                    case "Relation":
                                        Relation = (LoyaltyPointsRelation) Conversion.XmlStringToInt(loyalElem.Value);
                                        break;
                                    case "TransactionID":
                                        TransactionID = loyalElem.Value;
                                        break;
                                    case "TerminalID":
                                        TerminalID = loyalElem.Value;
                                        break;
                                    case "StoreID":
                                        StoreID = loyalElem.Value;
                                        break;
                                    case "ReceiptID":
                                        ReceiptID = loyalElem.Value;
                                        break;
                                    case "EntryStatus":
                                        EntryStatus = (TransactionStatus)Conversion.XmlStringToInt(loyalElem.Value);
                                        break;
                                    case "ExpirationDate":
                                        ExpirationDate = Conversion.XmlStringToDateTime(loyalElem.Value);
                                        break;
                                    case "StaffID":
                                        StaffID = loyalElem.Value;
                                        break;
                                    case "DateIssued":
                                        DateIssued = Conversion.XmlStringToDateTime(loyalElem.Value);
                                        break;
                                    case "RuleID":
                                        RuleID = loyalElem.Value;
                                        break;
                                    case "LineNum":
                                        LineNum = Conversion.XmlStringToDecimal(loyalElem.Value);
                                        break;
                                    case "AggregatedItemQuantity":
                                        AggregatedItemQuantity = Conversion.XmlStringToDecimal(loyalElem.Value);
                                        break;
                                    case "PartOfAggregatedItemPoints":
                                        PartOfAggregatedItemPoints = Conversion.XmlStringToBool(loyalElem.Value);
                                        break;
                                    case "RecalculateDiscount":
                                        RecalculateDiscount = Conversion.XmlStringToBool(loyalElem.Value);
                                        break;
                                    case "UsePointsLimit":
                                        UsePointsLimit = Conversion.XmlStringToInt(loyalElem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, loyalElem.Name.ToString(), ex);
                            }
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, xLoyalty.Name.ToString(), ex);
            }
        }
    }
}

