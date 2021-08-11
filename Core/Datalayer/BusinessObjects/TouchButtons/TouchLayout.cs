using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.TouchButtons
{
    public class TouchLayout : DataEntity
    {
        public override string Text
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value;
            }
        }

        public TouchLayout()
        {
            Name = "";
            ButtonGrid1 = "";
            ButtonGrid2 = "";
            ButtonGrid3 = "";
            ButtonGrid4 = "";
            ButtonGrid5 = "";
            ReceiptID = "";
            TotalID = "";
            ReceiptItemsLayoutXML = null;
            ReceiptPaymentLayoutXML = null ;
            TotalsLayoutXML = null;
            LayoutXML = null;
            CashChangerLayoutXML = null;
            LogoPictureID = "";
            Guid = Guid.Empty;
            ImportDateTime = null;
        }

        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public RecordIdentifier ButtonGrid1 { get; set; }
        public RecordIdentifier ButtonGrid2 { get; set; }
        public RecordIdentifier ButtonGrid3 { get; set; }
        public RecordIdentifier ButtonGrid4 { get; set; }
        public RecordIdentifier ButtonGrid5 { get; set; }
        public string ReceiptID { get; set; }
        public string TotalID { get; set; }
        public RecordIdentifier LogoPictureID { get; set; }
        public string ImgCustomerLayoutXML { get; set; }
        public string ImgReceiptItemsLayoutXML { get; set; }
        public string ImgReceiptPaymentLayoutXML { get; set; }
        public string ImgTotalsLayoutXML { get; set; }
        public string ImgLayoutXML { get; set; }
        public string ReceiptItemsLayoutXML { get; set; }
        public string ReceiptPaymentLayoutXML { get; set; }
        public string TotalsLayoutXML { get; set; }
        public string LayoutXML { get; set; }
        public string ImgCashChangerLayoutXML { get; set; }
        public string CashChangerLayoutXML { get; set; }
        public Guid Guid { get; set; }
        /// <summary>
        /// Time and date of import or null if layout wasn't imported
        /// </summary>
        public DateTime? ImportDateTime { get; set; }

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
                            case "tillLayoutID":
                                ID = current.Value;
                                break;
                            case "name":
                                Name = current.Value;
                                break;
                            case "width":
                                Width = Convert.ToInt32(current.Value);
                                break;
                            case "height":
                                Height = Convert.ToInt32(current.Value);
                                break;
                            case "buttonGrid1":
                                ButtonGrid1 = current.Value;
                                break;
                            case "buttonGrid2":
                                ButtonGrid2 = current.Value;
                                break;
                            case "buttonGrid3":
                                ButtonGrid3 = current.Value;
                                break;
                            case "buttonGrid4":
                                ButtonGrid4 = current.Value;
                                break;
                            case "buttonGrid5":
                                ButtonGrid5 = current.Value;
                                break;
                            case "receiptID":
                                ReceiptID = current.Value;
                                break;
                            case "totalID":
                                TotalID = current.Value;
                                break;
                            case "logoPictureID":
                                LogoPictureID = current.Value;
                                break;
                            case "imgCustomerLayoutXML":
                                ImgCustomerLayoutXML = current.Value;
                                break;
                            case "imgReceiptItemsLayoutXML":
                                ImgReceiptItemsLayoutXML = current.Value;
                                break;
                            case "imgReceiptPaymentLayoutXML":
                                ImgReceiptPaymentLayoutXML = current.Value;
                                break;
                            case "imgTotalsLayoutXML":
                                ImgTotalsLayoutXML = current.Value;
                                break;
                            case "imgLayoutXML":
                                ImgLayoutXML = current.Value;
                                break;
                            case "receiptItemsLayoutXML":
                                ReceiptItemsLayoutXML = current.Value;
                                break;
                            case "receiptPaymentLayoutXML":
                                ReceiptPaymentLayoutXML = current.Value;
                                break;
                            case "totalsLayoutXML":
                                TotalsLayoutXML = current.Value;
                                break;
                            case "layoutXML":
                                LayoutXML = current.Value;
                                break;
                            case "imgCashChangerLayoutXML":
                                ImgCashChangerLayoutXML = current.Value;
                                break;
                            case "cashChangerLayoutXML":
                                CashChangerLayoutXML = current.Value;
                                break;
                            case "guid":
                                Guid = Guid.Parse(current.Value);
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
            XElement xml = new XElement("tillLayout",
                    new XElement("tillLayoutID", (string)ID),
                    new XElement("name", Name),
                    new XElement("width", Width),
                    new XElement("height", Height),
                    new XElement("buttonGrid1", (string)ButtonGrid1),
                    new XElement("buttonGrid2", (string)ButtonGrid2),
                    new XElement("buttonGrid3", (string)ButtonGrid3),
                    new XElement("buttonGrid4", (string)ButtonGrid4),
                    new XElement("buttonGrid5", (string)ButtonGrid5),
                    new XElement("receiptID", ReceiptID),
                    new XElement("totalID", TotalID),
                    new XElement("logoPictureID", (string)LogoPictureID),
                    new XElement("imgCustomerLayoutXML", ImgCustomerLayoutXML),
                    new XElement("imgReceiptItemsLayoutXML", ImgReceiptItemsLayoutXML),
                    new XElement("imgReceiptPaymentLayoutXML", ImgReceiptPaymentLayoutXML),
                    new XElement("imgTotalsLayoutXML", ImgTotalsLayoutXML),
                    new XElement("imgLayoutXML", ImgLayoutXML),
                    new XElement("receiptItemsLayoutXML", ReceiptItemsLayoutXML),
                    new XElement("receiptPaymentLayoutXML", ReceiptPaymentLayoutXML),
                    new XElement("totalsLayoutXML", TotalsLayoutXML),
                    new XElement("layoutXML", LayoutXML),
                    new XElement("imgCashChangerLayoutXML", ImgCashChangerLayoutXML),
                    new XElement("cashChangerLayoutXML", CashChangerLayoutXML),
                    new XElement("guid", Guid));
            return xml;
        }
    }
}
