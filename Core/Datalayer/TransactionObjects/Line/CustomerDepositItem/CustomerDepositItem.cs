using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.CustomerDepositItem
{

    /// <summary>
    /// A customer deposit item line in a transaction.
    /// </summary>
    [Serializable]
    public class CustomerDepositItem : ICloneable, ICustomerDepositItem
    {
        private string description;                  //The text to display on the receipt
        private decimal amount;                      //The amount of the deposit
        private string comment;                      


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

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public virtual object Clone()
        {
            CustomerDepositItem item = new CustomerDepositItem();
            Populate(item);
            return item;
        }

        protected void Populate(CustomerDepositItem item)
        {
            item.amount = amount;
            item.comment = comment;
            item.description = description;
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
                XElement xCustomerDeposit = new XElement("CustomerDepositItem",
                    new XElement("description", description),
                    new XElement("amount", amount.ToString()),
                    new XElement("comment", comment)
                );
                return xCustomerDeposit;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, ex.Message, ex);
                }
                throw ex;
            }
        }

        public void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            if (xItem.HasElements)
            {
                IEnumerable<XElement> classElements = xItem.Elements("CustomerDepositItem");
                foreach (XElement xClass in classElements)
                {
                    if (xClass.HasElements)
                    {
                        IEnumerable<XElement> classVariables = xClass.Elements();
                        foreach (XElement xVariable in classVariables)
                        {
                            if (!xVariable.IsEmpty)
                            {
                                try
                                {
                                    switch (xVariable.Name.ToString())
                                    {
                                        case "description":
                                            description = xVariable.Value.ToString();
                                            break;
                                        case "amount":
                                            amount = Convert.ToDecimal(xVariable.Value.ToString());
                                            break;
                                        case "comment":
                                            comment = xVariable.Value.ToString();
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (errorLogger != null)
                                    {
                                        errorLogger.LogMessage(LogMessageType.Error, ex.Message, ex);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
