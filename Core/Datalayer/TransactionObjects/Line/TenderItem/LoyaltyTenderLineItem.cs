using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ErrorHandling;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.DataLayer.TransactionObjects.Line.TenderItem
{

    /// <summary>
    /// A class extending the card payments, to payments with loyalty cards.
    /// </summary>
    [Serializable]
    public class LoyaltyTenderLineItem : CardTenderLineItem, ILoyaltyTenderLineItem
    {
        decimal loyaltyPoints;

        public decimal Points
        {
            get {return loyaltyPoints;}
            set { loyaltyPoints = value; }
        }

        public LoyaltyTenderLineItem()
        {
            internalTenderType = TenderTypeEnum.LoyaltyTender;
        }
        protected void Populate(LoyaltyTenderLineItem item)
        {
            base.Populate(item);
            item.loyaltyPoints = loyaltyPoints;
        }

        public override object Clone()
        {
            LoyaltyTenderLineItem item = new LoyaltyTenderLineItem();
            Populate(item);
            return item;
        }

        public override System.Xml.Linq.XElement ToXML(IErrorLog errorLogger = null)
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
                XElement xLoyaltyTender = new XElement("LoyaltyTenderLineItem",
                    new XElement("loyaltyPoints", loyaltyPoints.ToString())
                );

                xLoyaltyTender.Add(base.ToXML(errorLogger));
                return xLoyaltyTender;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "LoyaltyTenderLineItem.ToXML", ex);
                }

                throw ex;
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
                                    case "loyaltyPoints":
                                        loyaltyPoints = Convert.ToDecimal(xVariable.Value.ToString());
                                        break;                                    
                                    default:
                                        base.ToClass(xVariable, errorLogger);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, "LoyaltyTenderLineItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "LoyaltyTenderLineItem.ToClass", ex);
                }

                throw ex;
            }
        }
    } 
}
