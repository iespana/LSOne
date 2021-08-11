using System;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.ConfigurationWizard
{
    /// <summary>
    /// Business entity class of Receipts page
    /// </summary>
    public class Receipts : DataEntity
    {
        public Receipts()
        {
            ID = RecordIdentifier.Empty;
            LineNum = null;
            FormLayoutID = RecordIdentifier.Empty;
            Image = null;
            Form = new Form();
        }

        /// <summary>
        /// The layout number; 1, 2 or 3
        /// </summary>
        public int? LineNum { get; set; }

        /// <summary>
        /// The selected layout
        /// </summary>
        public RecordIdentifier FormLayoutID { get; set; }

        /// <summary>
        /// The preview of the receipt
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// Object of standard Form class.
        /// </summary>
        public Form Form { get; set; }

        /// <summary>
        /// Sets all variables in the Receipts class with the values in the xml
        /// </summary>
        /// <param name="xReceipt">The xml element with the receipt setting values</param>
        /// <param name="errorLogger"></param>
        public override void ToClass(XElement xReceipt, IErrorLog errorLogger = null)
        {

            if (xReceipt.Name.ToString() == "receiptLayout")
            {
                var receiptLayoutElements = xReceipt.Elements();
                foreach (var RcptLaytelements in receiptLayoutElements)
                {
                    //No tax layout id -> no receipt setting -> no need to go any further
                    if (RcptLaytelements.Name.ToString() == "id" && RcptLaytelements.Value == "")
                    {
                        return;
                    }
                    if (RcptLaytelements.Value.Length > 0)
                    {
                        try
                        {
                            switch (RcptLaytelements.Name.ToString())
                            {
                                case "id":
                                    Form.ID = RcptLaytelements.Value;
                                    break;
                                case "description":
                                    Form.Text = RcptLaytelements.Value;
                                    break;
                                case "printBehaviour":
                                    Form.PrintBehavior = (PrintBehaviors)Convert.ToInt32(RcptLaytelements.Value);
                                    break;
                                case "headerXml":
                                    Form.HeaderXml = RcptLaytelements.Value;
                                    break;
                                case "lineXml":
                                    Form.LineXml = RcptLaytelements.Value;
                                    break;
                                case "footerXml":
                                    Form.FooterXml = RcptLaytelements.Value;
                                    break;
                                case "printAsSlip":
                                    Form.PrintAsSlip = Convert.ToBoolean(RcptLaytelements.Value);
                                    break;
                                case "lineCountPerPage":
                                    Form.LineCountPerPage = Convert.ToInt32(RcptLaytelements.Value);
                                    break;
                                case "useWindowsPrinter":
                                    Form.UseWindowsPrinter = Convert.ToBoolean(RcptLaytelements.Value);
                                    break;                                
                                case "windowsPrinterConfigurationID":
                                    Form.WindowsPrinterConfigurationID = RcptLaytelements.Value;
                                    break;
                                case "promptQuestion":
                                    Form.PromptQuestion = Convert.ToBoolean(RcptLaytelements.Value); break;
                                case "promptText":
                                    Form.PromptText = RcptLaytelements.Value;
                                    break;
                                case "upperCase":
                                    Form.UpperCase = Convert.ToBoolean(RcptLaytelements.Value);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error, RcptLaytelements.Name.ToString(), ex);
                            }
                        }

                    }
                }
            }            

            if (xReceipt.Name.ToString() == "layoutImage")
            {
                var storeVariables = xReceipt.Attributes();
                foreach (var storeElem in storeVariables)
                {
                    //No till Layout ID -> no touch setting -> no need to go any further
                    if (storeElem.Name.ToString() == "receiptLayoutID" && storeElem.Value == "")
                    {
                        return;
                    }
                    if (storeElem.Value.Length > 0)
                    {
                        try
                        {
                            switch (storeElem.Name.ToString())
                            {
                                case "receiptLayoutID":
                                    FormLayoutID = storeElem.Value;
                                    Image = Convert.FromBase64String(xReceipt.Value);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error, storeElem.Name.ToString(), ex);
                            }
                        }
                    }
                }
            }


        }

        /// <summary>
        /// Creates an xml element from all the variables in the Receipts class
        /// </summary>
        /// <param name="errorLogger"></param>
        /// <returns>An XML element</returns>
        public override XElement ToXML(IErrorLog errorLogger = null)
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
            XElement receipts = null;
            if (Image != null)
            {
                receipts = new XElement("layoutImage", Convert.ToBase64String(Image),
                    new XAttribute("receiptLayoutID", (string)FormLayoutID)
                    );
            }
            if (Form.ID != RecordIdentifier.Empty && Image == null)
            {
                receipts = new XElement("receiptLayout",
                    new XElement("id", Form.ID),
                    new XElement("description", Form.Text),
                    new XElement("printBehaviour", Form.PrintBehavior),
                    new XElement("headerXml", Form.HeaderXml),
                    new XElement("lineXml", Form.LineXml),
                    new XElement("footerXml", Form.FooterXml),
                    new XElement("printAsSlip", Form.PrintAsSlip),
                    new XElement("lineCountPerPage", Form.LineCountPerPage),
                    new XElement("useWindowsPrinter", Form.UseWindowsPrinter),                    
                    new XElement("windowsPrinterConfigurationID", Form.WindowsPrinterConfigurationID),
                    new XElement("promptQuestion", Form.PromptQuestion),
                    new XElement("promptText", Form.PromptText),
                    new XElement("upperCase", Form.UpperCase)
                    );
            }
            return receipts;
        }
    }
}
