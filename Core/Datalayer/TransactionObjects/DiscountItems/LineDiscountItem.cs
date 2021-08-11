using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.DiscountItems
{
    /// <summary>
    /// A line discount information.
    /// </summary>
    [Serializable]
    public class LineDiscountItem : DiscountItem, ILineDiscountItem
    {
        public LineDiscountItem()
        {
            DiscountType = DiscountTransTypes.LineDisc;
        }
        
        private DiscountTypes lineDiscountType;  //The type of line discount, i.e employee, periodic, etc.
        
        /// <summary>
        /// The type of line discount, i.e employee, periodic, etc.
        /// </summary>
        public DiscountTypes LineDiscountType
        {
            get { return lineDiscountType; }
            set { lineDiscountType = value; }
        }

        public override object Clone()
        {
            LineDiscountItem item = new LineDiscountItem();
            Populate(item);
            return item;
        }

        protected void Populate(LineDiscountItem item)
        {
            base.Populate(item);
            item.lineDiscountType = lineDiscountType;
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
                XElement xLineDiscount = new XElement("LineDiscountItem",
                    new XElement("lineDiscountType", (int)lineDiscountType)
                );

                xLineDiscount.Add(base.ToXML(errorLogger));
                return xLineDiscount;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "LineDiscountItem.ToXml", ex);
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
                                    case "lineDiscountType":
                                        lineDiscountType = (DiscountTypes)Convert.ToInt32(xVariable.Value);
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
                                    errorLogger.LogMessage(LogMessageType.Error, "LineDiscountItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "LineDiscountItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
