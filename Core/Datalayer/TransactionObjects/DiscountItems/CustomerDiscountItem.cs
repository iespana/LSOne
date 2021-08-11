using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.DiscountItems
{
    [Serializable]
    public class CustomerDiscountItem : LineDiscountItem, ICustomerDiscountItem
    {
        public CustomerDiscountItem()
        {
            DiscountType = DiscountTransTypes.Customer;
        }

        private CustomerDiscountTypes customerDiscountType; //The type of a periodic discount, i.e multibuy, mix & match, offer.
        private string itemDiscountGroup;                   //The discount group of the item
        private string customerDiscountGroup;               //The discount group of the customer        

        /// <summary>
        /// The type of a customer discount, i.e line discount, multiline discount
        /// </summary>
        public CustomerDiscountTypes CustomerDiscountType
        {
            get { return customerDiscountType; }
            set { customerDiscountType = value; }
        }
        /// <summary>
        /// The discount group of the item
        /// </summary>
        public string ItemDiscountGroup
        {
            get { return itemDiscountGroup; }
            set { itemDiscountGroup = value; }
        }
        /// <summary>
        /// The discount group of the customer
        /// </summary>
        public string CustomerDiscountGroup
        {
            get { return customerDiscountGroup; }
            set { customerDiscountGroup = value; }
        }

        public override object Clone()
        {
            CustomerDiscountItem item = new CustomerDiscountItem();
            Populate(item);
            return item;
        }

        protected void Populate(CustomerDiscountItem item)
        {
            base.Populate(item);
            item.customerDiscountType = customerDiscountType;
            item.itemDiscountGroup = itemDiscountGroup;
            item.customerDiscountGroup = customerDiscountGroup;
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
                XElement xCustomerDisc = new XElement("CustomerDiscountItem",
                    new XElement("customerDiscountType", (int)customerDiscountType),
                    new XElement("itemDiscountGroup", itemDiscountGroup),
                    new XElement("customerDiscountGroup", customerDiscountGroup)
                );

                xCustomerDisc.Add(base.ToXML(errorLogger));
                return xCustomerDisc;  
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "CustomerDiscountItem.ToXML", ex);
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
                                    case "customerDiscountType":
                                        customerDiscountType = (CustomerDiscountTypes)Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "itemDiscountGroup":
                                        itemDiscountGroup = xVariable.Value;
                                        break;
                                    case "customerDiscountGroup":
                                        customerDiscountGroup = xVariable.Value;
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
                                    errorLogger.LogMessage(LogMessageType.Error, "CustomerDiscountItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "CustomerDiscountItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
