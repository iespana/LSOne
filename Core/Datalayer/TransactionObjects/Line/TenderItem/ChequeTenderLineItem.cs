using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.TenderItem
{
    /// <summary>
    /// Used to register check payments.
    /// </summary>
    [Serializable]
    public class ChequeTenderLineItem: TenderLineItem
    {
        private string personalIdNumber;        //The personal identification number
        private Date customerBirthDate = Date.Empty;     //The customers date of birth 
        private bool printCheque;               //Does the cheque need to be printed

        #region Properties
        /// <summary>
        /// The personal identification number that must be keyed by an 
        /// individual at the point of sale when paying by personal check.
        /// </summary>
        public string PersonalIdNumber
        {
            get { return personalIdNumber; }
            set { personalIdNumber = value; }
        }
        /// <summary>
        /// The customer's date of birth captured as part of the personal data required 
        /// to verify their identity and authorize their use of a check to pay for a purchase.
        /// </summary>
        public DateTime CustomerBirthDate
        {
            get { return customerBirthDate.DateTime; }
            set { customerBirthDate = new Date(value, true); }
        }

        public bool PrintCheque
        {
            get { return printCheque; }
            set { printCheque = value; }
        }

        /// <summary>
        /// The Check ID number entered in the pay check dialog
        /// </summary>
        public string CheckID { get; set; }
        #endregion

        public ChequeTenderLineItem()
        {
            internalTenderType = TenderTypeEnum.ChequeTender;
        }

        protected void Populate(ChequeTenderLineItem item)
        {
            base.Populate(item);
            item.personalIdNumber = personalIdNumber;
            item.customerBirthDate = customerBirthDate;
            item.printCheque = printCheque;
            item.CheckID = CheckID;
            item.internalTenderType = TypeOfTender;
        }

        public override object Clone()
        {
            ChequeTenderLineItem item = new ChequeTenderLineItem();
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
                XElement xCheque = new XElement("ChequeTenderLineItem",
                    new XElement("personalIdNumber", personalIdNumber),
                    new XElement("customerBirthDate", customerBirthDate.ToXmlString()),
                    new XElement("printCheque", printCheque),
                    new XElement("checkID", CheckID)
                );
                xCheque.Add(base.ToXML(errorLogger));
                return xCheque;

            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "ChequeTenderLineItem.ToXml", ex);
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
                                    case "personalIdNumber":
                                        personalIdNumber = xVariable.Value;
                                        break;
                                    case "customerBirthDate":
                                        customerBirthDate = new Date(Conversion.XmlStringToDateTime(xVariable.Value), true);
                                        break;
                                    case "printCheque":
                                        printCheque = Conversion.ToBool(xVariable.Value);
                                        break;
                                    case "checkID":
                                        CheckID = xVariable.Value;
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
                                    errorLogger.LogMessage(LogMessageType.Error, "ChequeTenderLineItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "ChequeTenderLineItem.ToClass", ex);
                }

                throw ex;
            }
        }

    }
}
