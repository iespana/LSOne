using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.TenderItem
{    
    /// <summary>
    /// A class for gift certificate payments.
    /// </summary>
    [Serializable]
    public class GiftCertificateTenderLineItem : TenderLineItem, IGiftCertificateTenderLineItem
    {
        private string issuingStoreId;      //The id of the store that issued the gift certificate
        private string issuingTerminalId;   //The id of the terminal that issued the gift certificate
        private string serialNumber;        //The serialnumber of the gift certificate
        private Date issuedDate = Date.Empty;        //The date when the gift certificate was issued
        private Date appliedDate = Date.Empty;       //The date when the gift certificate was applied

        #region Properties

        /// <summary>
        /// The id of the store that issued the gift certificate.
        /// </summary>
        public string IssuingStoreId
        {
            get { return issuingStoreId; }
            set { issuingStoreId = value; }
        }
        /// <summary>
        /// The id of the terminal that issued the gift certificate.
        /// </summary>
        public string IssuingTerminalId
        {
            get { return issuingTerminalId; }
            set { issuingTerminalId = value; }
        }
        /// <summary>
        /// The serialnumber of the gift certificate. 
        /// </summary>
        public string SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }
        /// <summary>
        /// The date when the gift certificate was issued.
        /// </summary>
        public DateTime IssuedDate
        {
            get { return issuedDate.DateTime; }
            set { issuedDate = new Date (value, true); }
        }
        /// <summary>
        /// The date when the gift certificate was applied.
        /// </summary>
        public DateTime AppliedDate
        {
            get { return appliedDate.DateTime; }
            set { appliedDate = new Date(value, true); }
        }
        #endregion

        public GiftCertificateTenderLineItem()
        {
            internalTenderType = TenderTypeEnum.GiftCertificateTender;
        }
        
        protected void Populate(GiftCertificateTenderLineItem item)
        {
            base.Populate(item);
            item.issuingStoreId = issuingStoreId;
            item.issuingTerminalId = issuingTerminalId;
            item.serialNumber = serialNumber;
            item.issuedDate = issuedDate;
            item.appliedDate = appliedDate;
            item.internalTenderType = TypeOfTender;
        }

        public override object Clone()
        {
            GiftCertificateTenderLineItem item = new GiftCertificateTenderLineItem();
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
                XElement xGiftCertificate = new XElement("GiftCertificateTenderLineItem",
                    new XElement("issuingStoreId", issuingStoreId),
                    new XElement("issuingTerminalId", issuingTerminalId),
                    new XElement("serialNumber", serialNumber),
                    new XElement("issuedDate", issuedDate.ToXmlString()),
                    new XElement("appliedDate", appliedDate.ToXmlString())
                );

                xGiftCertificate.Add(base.ToXML(errorLogger));
                return xGiftCertificate;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "GiftCertificateTenderLineItem.ToXml", ex);
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
                                        issuingStoreId = xVariable.Value.ToString();
                                        break;
                                    case "issuingTerminalId": 
                                        issuingTerminalId = xVariable.Value.ToString();
                                        break;
                                    case "serialNumber": 
                                        serialNumber = xVariable.Value.ToString();
                                        break;
                                    case "issuedDate":
                                        issuedDate = new Date(Conversion.XmlStringToDateTime(xVariable.Value), true);
                                        break;
                                    case "appliedDate":
                                        appliedDate = new Date(Conversion.XmlStringToDateTime(xVariable.Value), true);
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
                                    errorLogger.LogMessage(LogMessageType.Error, "GiftCertificateTenderLineItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "GiftCertificateTenderLineItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
