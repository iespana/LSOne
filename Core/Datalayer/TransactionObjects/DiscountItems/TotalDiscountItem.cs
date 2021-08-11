using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Services.Interfaces;

namespace LSOne.DataLayer.TransactionObjects.DiscountItems
{
    /// <summary>
    /// The part of the total discount that belongs to the transaction.
    /// </summary>
    [Serializable]
    public class TotalDiscountItem : DiscountItem, ITotalDiscountItem
    {
        public TotalDiscountItem()
        {
            DiscountType = DiscountTransTypes.TotalDisc;
        }

        private bool manullyEntered;    

        /// <summary>
        /// Is set as true if discount was manualy entered.
        /// </summary>
        public bool ManullyEntered
        {
            get { return manullyEntered; }
            set { manullyEntered = value; }
        }

        public override object Clone()
        {
            TotalDiscountItem item = new TotalDiscountItem();
            Populate(item);
            return item;
        }

        protected void Populate(TotalDiscountItem item)
        {
            base.Populate(item);
            item.manullyEntered = manullyEntered;
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
                XElement xTotalDisc = new XElement("TotalDiscountItem",
                    new XElement("manullyEntered", manullyEntered)
                );

                xTotalDisc.Add(base.ToXML(errorLogger));
                return xTotalDisc;            
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "TotalDiscountItem.ToXML", ex);
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
                                    case "manullyEntered":
                                        manullyEntered = Conversion.ToBool(xVariable.Value);
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
                                    errorLogger.LogMessage(LogMessageType.Error, "TotalDiscountItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "TotalDiscountItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
