using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ErrorHandling;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.DataLayer.TransactionObjects.DiscountItems
{
    /// <summary>
    /// The part of the total discount that belongs to the transaction.
    /// </summary>
    [Serializable]
    public class LoyaltyDiscountItem : TotalDiscountItem, ILoyaltyDiscountItem
    {
        public decimal LoyaltyPoints { get; set; }

        public LoyaltyDiscountItem()
        {
            DiscountType = DiscountTransTypes.LoyaltyDisc;
            LoyaltyPoints = decimal.Zero;    
        }

        public override object Clone()
        {
            LoyaltyDiscountItem item = new LoyaltyDiscountItem();
            Populate(item);
            return item;
        }

        protected void Populate(LoyaltyDiscountItem item)
        {
            base.Populate(item);
            item.LoyaltyPoints = LoyaltyPoints;

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
                XElement xTotalDisc = new XElement("LoyaltyDiscountItem",
                    new XElement("LoyaltyPoints", LoyaltyPoints.ToString())
                );
                

                xTotalDisc.Add(base.ToXML(errorLogger));
                return xTotalDisc;            
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "LoyaltyDiscountItem.ToXML", ex);
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
                                    case "LoyaltyPoints":
                                        LoyaltyPoints = Convert.ToDecimal(xVariable.Value);
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
                                    errorLogger.LogMessage(LogMessageType.Error, "LoyaltyDiscountItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "LoyaltyDiscountItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
