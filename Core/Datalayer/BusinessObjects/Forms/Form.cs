using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.Forms
{
    public class Form : DataEntity
    {
        public Form()
            : base()
        {
            HeaderXml = "";
            LineXml = "";
            FooterXml = "";
            PrintAsSlip = false;
            LineCountPerPage = 0;
            UseWindowsPrinter = false;            
            PrintBehavior = PrintBehaviors.AlwaysPrint;
            PromptText = "";
            PromptQuestion = false;
            DefaultFormWidth = 56; // This is a common default width for POS printers
            IsSystemLayout = false;
            WindowsPrinterConfigurationID = RecordIdentifier.Empty;
            NumberOfCopiesToPrint = 1;
        }

        public PrintBehaviors PrintBehavior { get; set; }
        public string HeaderXml { get; set; }
        public string LineXml { get; set; }
        public string FooterXml { get; set; }
        public bool PrintAsSlip { get; set; }
        public int LineCountPerPage { get; set; }
        public bool UseWindowsPrinter { get; set; }
        public FormSystemType SystemType { get; set; }
        public RecordIdentifier FormTypeID { get; set; }
        public bool PromptQuestion { get; set; }
        public string PromptText { get; set; }
        public bool UpperCase { get; set; }
        public int DefaultFormWidth { get; set; }
        public bool IsSystemLayout { get; set; }
        public int NumberOfCopiesToPrint { get; set; }

        /// <summary>
        /// ID of the windows printer configuration
        /// </summary>
        public RecordIdentifier WindowsPrinterConfigurationID { get; set; }
        
        public string FormsText
        {
            get
            {
                switch (SystemType)
                {
                    case FormSystemType.UserDefinedType:
                        return Properties.Resources.UserDefinedType;
                    case FormSystemType.Receipt:
                        return Properties.Resources.Receipt;
                    case FormSystemType.CardReceiptForShop:
                        return Properties.Resources.CardReceiptForShop;
                    case FormSystemType.CardReceiptForCust:
                        return Properties.Resources.CardReceiptForCust;
                    case FormSystemType.CardReceiptForShopReturn:
                        return Properties.Resources.CardReceiptForShopReturn;
                    case FormSystemType.CardReceiptForCustReturn:
                        return Properties.Resources.CardReceiptForCustReturn;
                    case FormSystemType.CardInfo:
                        return Properties.Resources.CardInfo;
                    case FormSystemType.CustAcntReceiptForCust:
                        return Properties.Resources.CustAcntReceiptForCust;
                    case FormSystemType.CustAcntReceiptForShop:
                        return Properties.Resources.CustAcntReceiptForShop;
                    case FormSystemType.CustAcntReceiptForCustReturn:
                        return Properties.Resources.CustAcntReceiptForCustReturn;
                    case FormSystemType.CustAcntReceiptForShopReturn:
                        return Properties.Resources.CustAcntReceiptForShopReturn;
                    case FormSystemType.TenderDeclaration:
                        return Properties.Resources.TenderDeclaration;
                    case FormSystemType.RemoveTender:
                        return Properties.Resources.RemoveTender;
                    case FormSystemType.CustomerAccountDeposit:
                        return Properties.Resources.CustomerAccountDeposit;
                    case FormSystemType.CreditMemo:
                        return Properties.Resources.CreditVoucher;
                    case FormSystemType.CreditMemoBalance:
                        return Properties.Resources.CreditVoucherBalance;
                    case FormSystemType.FloatEntry:
                        return Properties.Resources.FloatEntry;
                    case FormSystemType.SalesOrderReceipt:
                        return Properties.Resources.SalesOrderReceipt;
                    case FormSystemType.SalesInvoiceReceipt:
                        return Properties.Resources.SalesInvoiceReceipt;
                    case FormSystemType.GiftCertificate:
                        return Properties.Resources.GiftCard;
                    case FormSystemType.EFTMessage:
                        return Properties.Resources.EftMessage;
                    case FormSystemType.SuspendedTransaction:
                        return Properties.Resources.SuspendedTransaction;
                    case FormSystemType.VoidedTransaction:
                        return Properties.Resources.VoidedTransaction;
                    case FormSystemType.Invoice:
                        return Properties.Resources.Invoice;
                    case FormSystemType.SafeDrop:
                        return Properties.Resources.SafeDrop;
                    case FormSystemType.BankDrop:
                        return Properties.Resources.BankDrop;
                    case FormSystemType.SafeDropReversal:
                        return Properties.Resources.SafeDropReversal;
                    case FormSystemType.BankDropReversal:
                        return Properties.Resources.BankDropReversal;
                    case FormSystemType.LoyaltyPaymentReceipt:
                        return Properties.Resources.LoyaltyPaymentReceipt;
                    case FormSystemType.SuspendedTransactionPrefix:
                        return Properties.Resources.SuspendedTransactionPrefix;
                    case FormSystemType.GiftReceipt:
                        return Properties.Resources.GiftReceipt;
                    case FormSystemType.CustomerOrderDeposit:
                        return Properties.Resources.CustomerOrderDeposit;
                    case FormSystemType.CustomerOrderInformation:
                        return Properties.Resources.CustomerOrderInformation;
                    case FormSystemType.CustomerOrderPickingList:
                        return Properties.Resources.CustomerOrderPickingList;
                    case FormSystemType.ReceiptEmailSubject:
                        return Properties.Resources.ReceiptEmailSubject;
                    case FormSystemType.ReceiptEmailBody:
                        return Properties.Resources.ReceiptEmailBody;
                    case FormSystemType.QuoteInformation:
                        return Properties.Resources.QuoteInformation;
                    case FormSystemType.OpenDrawer:
                        return Properties.Resources.OpenDrawer;
                    case FormSystemType.GiftCardBalance:
                        return Properties.Resources.GiftCardBalance;
                    case FormSystemType.KitchenSlip:
                        return Properties.Resources.KitchenSlip;
                    case FormSystemType.FiscalInfoSlip:
                        return Properties.Resources.FiscalInfo;
                    default:
                        return "";
                }
            }
        }
        
        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            IEnumerable<XElement> elements = element.Elements();
            foreach (XElement current in elements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "id":
                                ID = current.Value;
                                break;
                            case "description":
                                Text = current.Value;
                                break;
                            case "printBehaviour":
                                PrintBehavior = (PrintBehaviors)Convert.ToInt32(current.Value);
                                break;
                            case "headerXml":
                                HeaderXml = current.Value;
                                break;
                            case "lineXml":
                                LineXml = current.Value;
                                break;
                            case "footerXml":
                                FooterXml = current.Value;
                                break;
                            case "printAsSlip":
                                PrintAsSlip = current.Value != "false";
                                break;
                            case "lineCountPerPage":
                                LineCountPerPage = Convert.ToInt32(current.Value);
                                break;
                            case "useWindowsPrinter":
                                UseWindowsPrinter = current.Value != "false";
                                break;                            
                            case "windowsPrinterConfigurationID":
                                WindowsPrinterConfigurationID = current.Value;
                                break;
                            case "promptQuestion":
                                PromptQuestion = current.Value != "false";
                                break;
                            case "promptText":
                                PromptText = current.Value;
                                break;
                            case "upperCase":
                                UpperCase = current.Value != "false";
                                break;
                            case "defaultFormWidth":
                                DefaultFormWidth = Convert.ToInt32(current.Value);
                                break;
                            case "systemType":
                                SystemType = (FormSystemType)Convert.ToInt32(current.Value);
                                break;
                            case "formTypeID":
                                FormTypeID = new Guid(current.Value);
                                break;
                            case "isSystemLayout":
                                IsSystemLayout = current.Value != "false";
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
                        }
                    }

                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("receiptLayout",
                    new XElement("id", ID),
                    new XElement("description", Text),
                    new XElement("printBehaviour", (int)PrintBehavior),
                    new XElement("headerXml", HeaderXml),
                    new XElement("lineXml", LineXml),
                    new XElement("footerXml", FooterXml),
                    new XElement("printAsSlip", PrintAsSlip),
                    new XElement("lineCountPerPage", LineCountPerPage),
                    new XElement("useWindowsPrinter", UseWindowsPrinter),                    
                    new XElement("windowsPrinterConfigurationID", (string)WindowsPrinterConfigurationID),
                    new XElement("promptQuestion", PromptQuestion),
                    new XElement("promptText", PromptText),
                    new XElement("upperCase", UpperCase),
                    new XElement("defaultFormWidth", DefaultFormWidth),
                    new XElement("systemType", (int)SystemType),
                    new XElement("formTypeID", (string)FormTypeID),
                    new XElement("isSystemLayout", IsSystemLayout)
                    );
            return xml;
        }

        public override object Clone()
        {
            var o = new Form();
            Populate(o);
            return o;
        }

        protected void Populate(Form o)
        {
            o.ID = (RecordIdentifier)ID.Clone();
            o.Text = Text;
            o.PrintBehavior = PrintBehavior;
            o.HeaderXml = HeaderXml;
            o.LineXml = LineXml;
            o.FooterXml = FooterXml;
            o.PrintAsSlip = PrintAsSlip;
            o.LineCountPerPage = LineCountPerPage;
            o.UseWindowsPrinter = UseWindowsPrinter;            
            o.WindowsPrinterConfigurationID = WindowsPrinterConfigurationID;
            o.PromptQuestion = PromptQuestion;
            o.PromptText = PromptText;
            o.UpperCase = UpperCase;
            o.FormTypeID = FormTypeID;
            o.IsSystemLayout = IsSystemLayout;
        }
    }
}
