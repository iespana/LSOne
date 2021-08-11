using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.DiscountItems
{
    /// <summary>
    /// A abstract and basic class for the Line- and TotalDiscountLineItem classes.
    /// </summary>
    [Serializable]
    public abstract class DiscountItem : IDiscountItem
    {
        #region Member variables
        private decimal amount;              // The discount amount 
        private decimal amountWithTax;       // The discount amount with tax
        private decimal percentage;          // The discount percentage 
        //Timestamps
        private Date beginDateTime = Date.Empty; //The start date and time of the transaction
        private Date endDateTime = Date.Empty;   //The end date and time of the transaction

        //protected TransactionLog transLog;
        
        #endregion

        #region Properties
        /// <summary>
        /// The discount amount.
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        /// <summary>
        /// The discount amount with tax
        /// </summary>
        public decimal AmountWithTax
        {
            get { return amountWithTax; }
            set { amountWithTax = value; }
        }
        public decimal Percentage
        {
            get { return percentage; }
            set { percentage = value; }
        }
        /// <summary>
        /// The start date and time of the transaction.
        /// </summary>
        public DateTime BeginDateTime
        {
            get { return beginDateTime.DateTime; }
            set { beginDateTime = new Date(value); }
        }
        /// <summary>
        /// The end date and time of the transaction.
        /// </summary>
        public DateTime EndDateTime
        {
            get { return endDateTime.DateTime; }
            set { endDateTime = new Date(value); }
        }
        public string DiscountName { get; set; }
        public RecordIdentifier DiscountID { get; set; }
        public DiscountTransTypes DiscountType { get; set; }
        public string PartnerInfo { get; set; }
        public DiscountOrigin Origin { get; set; }
        #endregion

        public DiscountItem()
        {
            this.beginDateTime = new Date(DateTime.Now);
            DiscountID = string.Empty;
            PartnerInfo = string.Empty;
            DiscountName = string.Empty;
            Origin = DiscountOrigin.POS;
        }

        public abstract object Clone();

        protected void Populate(DiscountItem item)
        {
            item.amount = amount;
            item.amountWithTax = amountWithTax;
            item.percentage = percentage;
            item.DiscountName = DiscountName;
            item.PartnerInfo = PartnerInfo;
            item.DiscountID = (RecordIdentifier)DiscountID.Clone();
            item.DiscountType = DiscountType;
            item.BeginDateTime = BeginDateTime;
            item.EndDateTime = EndDateTime;
            item.Origin = Origin;
        }

        public virtual XElement ToXML(IErrorLog errorLogger = null)
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
                XElement xDiscountItem = new XElement("DiscountItem",
                    new XElement("amount", Conversion.ToXmlString(amount)),
                    new XElement("amountWithTax", Conversion.ToXmlString(amountWithTax)),
                    new XElement("percentage", Conversion.ToXmlString(percentage)),
                    new XElement("discountName", DiscountName),
                    new XElement("partnerInfo", PartnerInfo),
                    new XElement("discountID", (string)DiscountID),
                    new XElement("discountType", Conversion.ToXmlString((int)DiscountType)),
                    new XElement("beginDateTime", beginDateTime.ToXmlString()),
                    new XElement("endDateTime", endDateTime.ToXmlString()),
                    new XElement("origin", Conversion.ToXmlString((int)Origin))
                );

                return xDiscountItem;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "DiscountItem.ToXML", ex);
                throw;
            }
        }

        public virtual void ToClass(XElement xItem, IErrorLog errorLogger = null)
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
                                    case "amount":
                                        amount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "amountWithTax":
                                        amountWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "percentage":
                                        percentage = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "beginDateTime":
                                        beginDateTime = new Date(Conversion.XmlStringToDateTime(xVariable.Value));
                                        break;
                                    case "endDateTime":
                                        endDateTime = new Date(Conversion.XmlStringToDateTime(xVariable.Value));
                                        break;
                                    case "discountName":
                                        DiscountName = xVariable.Value;
                                        break;
                                    case "partnerInfo":
                                        PartnerInfo = xVariable.Value;
                                        break;
                                    case "discountID":
                                        DiscountID = xVariable.Value;
                                        break;
                                    case "discountType":
                                        DiscountType = (DiscountTransTypes)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "origin":
                                        Origin = (DiscountOrigin)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, "DiscountItem:" + xVariable.Name, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "DiscountItem.ToClass", ex);

                throw;
            }
        }
    }
}

