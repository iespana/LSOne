using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.TenderItem
{
    /// <summary>
    /// To register when a payment is made into a customer account.
    /// </summary>
    [Serializable]
    public class CustomerTenderLineItem: TenderLineItem
    {
        private string customerId;       //The accountnumber of the customer making the payment
        private string manualAuthCode;   //If authorization for the cust. account charging was acquired manually.

        /// <summary>
        /// The accountnumber of the customer making the payment
        /// </summary>
        public string CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }

        /// <summary>
        /// The authentication code if the authorization was acquired manually.
        /// </summary>
        public string ManualAuthCode
        {
            get { return manualAuthCode; }
            set { manualAuthCode = value; }
        }

        public CustomerTenderLineItem()
        {
            internalTenderType = TenderTypeEnum.CustomerTender;
        }

        protected void Populate(CustomerTenderLineItem item)
        {
            base.Populate(item);
            item.manualAuthCode = manualAuthCode;
            item.customerId = customerId;
            item.internalTenderType = TypeOfTender;
        }

        public override object Clone()
        {
            CustomerTenderLineItem item = new CustomerTenderLineItem();
            Populate(item);
            return item;
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
                XElement xCustomer = new XElement("CustomerTenderLineItem",
                    new XElement("customerId", customerId),
                    new XElement("manualAuthCode", manualAuthCode)
                );
                xCustomer.Add(base.ToXML(errorLogger));
                return xCustomer;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "CustomerTenderLineItem.ToXML", ex);
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
                                    case "customerId":
                                        customerId = xVariable.Value.ToString();
                                        break;
                                    case "manualAuthCode":
                                        manualAuthCode = xVariable.Value.ToString();
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
                                    errorLogger.LogMessage(LogMessageType.Error, "CustomerTenderLineItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "CustomerTenderLineItem.ToXML", ex);
                }

                throw ex;
            }
        }
    }
}
