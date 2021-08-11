using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.MarkupItem
{

    /// <summary>
    /// A markup item line in a transaction.
    /// </summary>
    
    [Serializable]
    public class MarkupItem : IMarkupItem, ICloneable 
    {
        private string description;                  //The text to display on the receipt
        private decimal amount;                      //The amount of the markup


        public MarkupItem()
            :base()
        {
            description = string.Empty;
            amount = 0;
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public virtual object Clone()
        {
            MarkupItem item = new MarkupItem();
            Populate(item);
            return item;
        }

        protected void Populate(MarkupItem item)
        {
            item.amount = amount;
            item.description = description;
        }

        public XElement ToXML()
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
            XElement xMarkup = new XElement("MarkupItem",
                new XElement("description", description),
                new XElement("amount", Conversion.ToXmlString(amount))
            );
            return xMarkup;
        }

        public void ToClass(XElement xMarkup, IErrorLog errorLogger = null)
        {
            if (xMarkup.HasElements)
            {
                IEnumerable<XElement> markupElements = xMarkup.Elements("MarkupItem");
                foreach (XElement aMarkupItem in markupElements)
                {
                    if (aMarkupItem.HasElements)
                    {
                        IEnumerable<XElement> markupVariables = aMarkupItem.Elements();
                        foreach (XElement markupElem in markupVariables)
                        {
                            if (!markupElem.IsEmpty)
                            {
                                try
                                {
                                    switch (markupElem.Name.ToString())
                                    {
                                        case "description":
                                            description = markupElem.Value;
                                            break;
                                        case "amount":
                                            amount = Conversion.XmlStringToDecimal(markupElem.Value);
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errorLogger?.LogMessage(LogMessageType.Error, ex.Message, ex);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
