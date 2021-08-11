using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.TenderItem
{
    //Inneignarnóta

    /// <summary>
    /// Tender line used when a customer who bought merchandise returns it
    /// </summary>
    [Serializable]
    public class CreditMemoTenderLineItem : TenderLineItem, ICreditMemoTenderLineItem
    {
        private string issuingStoreId;      //The id of the store that issued the credit memo
        private string issuingTerminalId;   //The id of the terminal that issued the credit memo
        private string serialNumber;        //The serialnumber of the credit memo
        private Date issuedDate = Date.Empty;        //The date when the credit memo was issued
        private Date appliedDate = Date.Empty;       //The date when the credit memo was applied
        private string issuedToName;        //The name of the person the credit memo was issued to
        private string issuedToExtraInfo;  

        #region Properties

        /// <summary>
        /// The id of the store that issued the credit memo.
        /// </summary>
        public string IssuingStoreId
        {
            get { return issuingStoreId; }
            set { issuingStoreId = value; }
        }
        /// <summary>
        /// The id of the terminal that issued the credit memo.
        /// </summary>
        public string IssuingTerminalId
        {
            get { return issuingTerminalId; }
            set { issuingTerminalId = value; }
        }
        /// <summary>
        /// The serialnumber of the credit memo. 
        /// </summary>
        public string SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }
        /// <summary>
        /// The date when the credit memo was issued.
        /// </summary>
        public DateTime IssuedDate
        {
            get { return issuedDate.DateTime; }
            set { issuedDate = new Date(value, true); }
        }
        /// <summary>
        /// The date when the credit memo was applied.
        /// </summary>
        public DateTime AppliedDate
        {
            get { return appliedDate.DateTime; }
            set { appliedDate = new Date(value, true); }
        }

        public string IssuedToName
        {
            get { return issuedToName; }
            set { issuedToName = value; }
        }

        public string IssuedToExtraInfo
        {
            get { return issuedToExtraInfo; }
            set { issuedToExtraInfo = value; }
        }

        #endregion
        

        public CreditMemoTenderLineItem() : base()
        {
            issuingStoreId = "";      
            issuingTerminalId = "";  
            serialNumber = "";           
            issuedToName = "";        
            issuedToExtraInfo = ""; 

            internalTenderType = TenderTypeEnum.CreditMemoTender;
        }

        protected void Populate(CreditMemoTenderLineItem item)
        {
            base.Populate(item);
            item.issuingStoreId = issuingStoreId;
            item.issuingTerminalId = issuingTerminalId;
            item.serialNumber = serialNumber;
            item.issuedDate = issuedDate;
            item.appliedDate = appliedDate;
            item.issuedToName = issuedToName;
            item.issuedToExtraInfo = issuedToExtraInfo;
            item.internalTenderType = TypeOfTender;
        }

        public override object Clone()
        {
            CreditMemoTenderLineItem item = new CreditMemoTenderLineItem();
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
                * DateTime     added withDevUtilities.Utility.DateTimeToXmlString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xCreditMemo = new XElement("CreditMemoTenderLineItem",
                    new XElement("issuingStoreId", issuingStoreId),
                    new XElement("issuingTerminalId", issuingTerminalId),
                    new XElement("serialNumber", serialNumber),
                    new XElement("issuedDate",issuedDate.ToXmlString()),
                    new XElement("appliedDate", appliedDate.ToXmlString()),
                    new XElement("issuedToName", issuedToName),
                    new XElement("issuedToExtraInfo", issuedToExtraInfo)
                );

                xCreditMemo.Add(base.ToXML(errorLogger));
                return xCreditMemo;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "CreditMemoTenderLineItem.ToXml", ex);
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
                                    case "issuingStoreId":
                                        issuingStoreId = xVariable.Value;
                                        break;
                                    case "issuingTerminalId":
                                        issuingTerminalId = xVariable.Value;
                                        break;
                                    case "serialNumber":
                                        serialNumber = xVariable.Value;
                                        break;
                                    case "issuedDate":
                                        issuedDate = new Date(Conversion.XmlStringToDateTime(xVariable.Value), true);
                                        break;                                           
                                    case "appliedDate":
                                        appliedDate = new Date(Conversion.XmlStringToDateTime(xVariable.Value), true);
                                        break;
                                    case "issuedToName":
                                        issuedToName = xVariable.Value;
                                        break;
                                    case "issuedToExtraInfo":
                                        issuedToExtraInfo = xVariable.Value;
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
                                    errorLogger.LogMessage(LogMessageType.Error, "CreditMemoTenderLineItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "CreditMemoTenderLineItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
