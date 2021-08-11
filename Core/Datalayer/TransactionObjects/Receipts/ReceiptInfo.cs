using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Receipts
{
    public class ReceiptInfo : DataEntity, IReceiptInfo
    {
        public const string XmlElementName = "Receipt";

        public int LineID { get; set; }

        /// <summary>
        /// The unique identifier of the form type that is being printed
        /// </summary>
        public RecordIdentifier FormType { get; set; }

        /// <summary>
        /// The OPOS print string that was printed
        /// </summary>
        public string PrintString { get; set; }

        /// <summary>
        /// The name of the document created with the receipt f.ex. when a PDF file is created
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// The file location of the document created
        /// </summary>
        public string DocumentLocation { get; set; }

        /// <summary>
        /// The width of the form that was used to create this receipt. Necessary for the WinPrinter
        /// </summary>
        public int FormWidth { get; set; }

        /// <summary>
        /// True if this receipt was generated using the email profile and is meant to be sent via email
        /// </summary>
        public bool IsEmailReceipt { get; set; }

        public ReceiptInfo()
        {
            LineID = 0;
            FormType = RecordIdentifier.Empty;
            PrintString = string.Empty;
            DocumentName = string.Empty;
            DocumentLocation = string.Empty;
            FormWidth = 56;
            IsEmailReceipt = false;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            ReceiptInfo item = new ReceiptInfo();
            Populate(item);
            return item;
        }

        protected void Populate(ReceiptInfo receipt)
        {
            base.Populate(receipt);
            receipt.FormType = FormType;
            receipt.LineID = LineID;
            receipt.PrintString = PrintString;
            receipt.DocumentLocation = DocumentLocation;
            receipt.DocumentName = DocumentName;
            receipt.FormWidth = FormWidth;
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
                string encodedPrintString = string.Empty;
                if (!string.IsNullOrWhiteSpace(PrintString))
                {
                    byte[] toEncodeAsBytes = UnicodeEncoding.Unicode.GetBytes(PrintString);
                    encodedPrintString = Convert.ToBase64String(toEncodeAsBytes);
                }

                XElement xReprintInfo = new XElement(XmlElementName,
                    new XElement("LineID", Conversion.ToXmlString(LineID)),
                    new XElement("PrintString", encodedPrintString),
                    new XElement("DocumentLocation", DocumentLocation),
                    new XElement("DocumentName", DocumentName),
                    new XElement("FormType", FormType),
                    new XElement("FormWidth", Conversion.ToXmlString(FormWidth)),
                    new XElement("IsEmailReceipt", Conversion.ToXmlString(IsEmailReceipt))
                );

                xReprintInfo.Add(base.ToXML(errorLogger));
                return xReprintInfo;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "ReceiptInfo.ToXml", ex);
                throw;
            }
        }

        public override void ToClass(XElement xElement, IErrorLog errorLogger = null)
        {
            try
            {
                if (xElement.HasElements)
                {
                    IEnumerable<XElement> orderVariables = xElement.Elements();
                    foreach (XElement elem in orderVariables)
                    {
                        if (!elem.IsEmpty)
                        {
                            try
                            {
                                switch (elem.Name.ToString())
                                {
                                    case "LineID":
                                        LineID = Conversion.XmlStringToInt(elem.Value);
                                        break;
                                    case "PrintString":
                                        string decodedPrintString = string.Empty;
                                        if (!string.IsNullOrWhiteSpace(elem.Value))
                                        {
                                            byte[] decodedBytes = Convert.FromBase64String(elem.Value);
                                            decodedPrintString = UnicodeEncoding.Unicode.GetString(decodedBytes);
                                        };
                                        PrintString = decodedPrintString;
                                        break;
                                    case "DocumentLocation":
                                        DocumentLocation = elem.Value;
                                        break;
                                    case "DocumentName":
                                        DocumentName = elem.Value;
                                        break;
                                    case "FormType":
                                        FormType = Conversion.XmlStringToGuid(elem.Value);
                                        break;
                                    case "FormWidth":
                                        FormWidth = Conversion.XmlStringToInt(elem.Value);
                                        break;
                                    case "IsEmailReceipt":
                                        IsEmailReceipt = Conversion.XmlStringToBool(elem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, elem.Name.ToString(), ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, xElement.Name.ToString(), ex);
            }
        }
    }
}
