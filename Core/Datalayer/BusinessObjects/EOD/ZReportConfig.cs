using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.EOD
{
    public class ZReportConfig : DataEntity
    {
        public bool IncludeFloatInCashSummary { get; set; }
        public bool CombineGrandTotalSalesandReturns { get; set; }
        public bool IncludeTenderDeclaration { get; set; }
        public bool DisplayReturnInfo { get; set; }
        public bool DisplaySuspendedInfo { get; set; }
        public bool DisplayOtherInfoSection { get; set; }
        public bool CombineSaleAndReturnXZReport { get; set; }
        public GrandTotalAmtDisplay GrandTotalAmountDisplay { get; set; }
        public SalesReportAmtdisplay SalesReportAmountDisplay { get; set; }
        public bool DisplayDepositInfo { get; set; }
        public DepositOrderBy OrderByDepositInfo { get; set; }
        public bool DisplayOverShortAmount { get; set; }
        public int ReportWidth { get; set; }
        public bool ShowIndividualDeposits { get; set; }

        /// <summary>
        /// Gets or sets whether the grand totals section on the X/Z report should be printed or not. 
        /// </summary>
        public bool PrintGrandTotals { get; set; }

        public int DefaultPadding => (int) ReportWidth/3;

        public ZReportConfig() : base()
        {
            Clear();
        }

        public void Clear()
        {
            IncludeFloatInCashSummary = true;
            CombineGrandTotalSalesandReturns = true;
            IncludeTenderDeclaration = false;
            DisplayReturnInfo = true;
            DisplaySuspendedInfo = true;
            DisplayOtherInfoSection = true;
            GrandTotalAmountDisplay = GrandTotalAmtDisplay.WithTax;
            SalesReportAmountDisplay = SalesReportAmtdisplay.WithTax;
            CombineSaleAndReturnXZReport = true;
            DisplayDepositInfo = true;
            OrderByDepositInfo = DepositOrderBy.Account;
            DisplayOverShortAmount = false;
            ReportWidth = 55;
            PrintGrandTotals = true;
            ShowIndividualDeposits = true;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            ZReport o = new ZReport();
            Populate(o);
            
            return o;
        }

        protected void Populate(ZReportConfig zReport)
        {
            zReport.ID = (RecordIdentifier)ID.Clone();
            zReport.Text = Text;
            zReport.IncludeFloatInCashSummary = IncludeFloatInCashSummary;
            zReport.IncludeTenderDeclaration = IncludeTenderDeclaration;
            zReport.CombineGrandTotalSalesandReturns = CombineGrandTotalSalesandReturns;
            zReport.CombineSaleAndReturnXZReport = CombineSaleAndReturnXZReport;
            zReport.DisplayOtherInfoSection = DisplayOtherInfoSection;
            zReport.DisplayReturnInfo = DisplayReturnInfo;
            zReport.DisplaySuspendedInfo = DisplaySuspendedInfo;
            zReport.GrandTotalAmountDisplay = GrandTotalAmountDisplay;
            zReport.SalesReportAmountDisplay = SalesReportAmountDisplay;
            zReport.DisplayDepositInfo = DisplayDepositInfo;
            zReport.OrderByDepositInfo = OrderByDepositInfo;
            zReport.ReportWidth = ReportWidth;
            zReport.DisplayOverShortAmount = DisplayOverShortAmount;
            zReport.ShowIndividualDeposits = ShowIndividualDeposits;
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            return new XElement("ZReportConfig",
                                new XElement("IncludeFloatInCashSummary", Conversion.ToXmlString(IncludeFloatInCashSummary)),
                                new XElement("CombineGrandTotalSalesandReturns", Conversion.ToXmlString(CombineGrandTotalSalesandReturns)),
                                new XElement("IncludeTenderDeclaration", Conversion.ToXmlString(IncludeTenderDeclaration)),
                                new XElement("CombineSaleAndReturnInTaxReport", Conversion.ToXmlString(CombineSaleAndReturnXZReport)),
                                new XElement("DisplayOtherInfoSection", Conversion.ToXmlString(DisplayOtherInfoSection)),
                                new XElement("DisplayReturnInfo", Conversion.ToXmlString(DisplayReturnInfo)),
                                new XElement("DisplaySuspendedInfo", Conversion.ToXmlString(DisplaySuspendedInfo)),
                                new XElement("GrandTotalAmountDisplay", Conversion.ToXmlString((int)GrandTotalAmountDisplay)),
                                new XElement("SalesReportAmountDisplay", Conversion.ToXmlString((int)SalesReportAmountDisplay)),
                                new XElement("DisplayDepositInfo", Conversion.ToXmlString(DisplayDepositInfo)),
                                new XElement("OrderByDepositInfo", Conversion.ToXmlString((int)OrderByDepositInfo)),
                                new XElement("ReportWidth", Conversion.ToXmlString(ReportWidth)),
                                new XElement("DisplayOverShortAmount", Conversion.ToXmlString(DisplayOverShortAmount)),
                                new XElement("ShowIndividualDeposits", Conversion.ToXmlString(ShowIndividualDeposits))
                               );
        }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            if (element.HasElements)
            {
                IEnumerable<XElement> elements = element.Elements("ZReportConfig");
                foreach (var current in elements)
                {
                    if (!current.IsEmpty)
                    {
                        try
                        {
                            switch (current.Name.ToString())
                            {
                                case "IncludeFloatInCashSummary":
                                    ID = current.Value;
                                    break;
                                case "CombineGrandTotalSalesandReturns":
                                    CombineGrandTotalSalesandReturns = Conversion.XmlStringToBool(current.Value);
                                    break;
                                case "IncludeTenderDeclaration":
                                    IncludeTenderDeclaration = Conversion.XmlStringToBool(current.Value);
                                    break;
                                case "CombineSaleAndReturnInTaxReport":
                                    CombineSaleAndReturnXZReport = Conversion.XmlStringToBool(current.Value);
                                    break;
                                case "DisplayOtherInfoSection":
                                    DisplayOtherInfoSection = Conversion.XmlStringToBool(current.Value);
                                    break;
                                case "DisplayReturnInfo":
                                    DisplayReturnInfo = Conversion.XmlStringToBool(current.Value);
                                    break;
                                case "DisplaySuspendedInfo":
                                    DisplaySuspendedInfo = Conversion.XmlStringToBool(current.Value);
                                    break;
                                case "GrandTotalAmountDisplay":
                                    GrandTotalAmountDisplay = (GrandTotalAmtDisplay)Conversion.XmlStringToInt(current.Value);
                                    break;
                                case "SalesReportAmountDisplay":
                                    SalesReportAmountDisplay = (SalesReportAmtdisplay)Conversion.XmlStringToInt(current.Value);
                                    break;
                                case "OrderByDepositInfo":
                                    OrderByDepositInfo = (DepositOrderBy)Conversion.XmlStringToInt(current.Value);
                                    break;
                                case "DisplayDepositInfo":
                                    DisplayDepositInfo = Conversion.XmlStringToBool(current.Value);
                                    break;
                                case "ReportWidth":
                                    ReportWidth = Conversion.XmlStringToInt(current.Value);
                                    break;
                                case "DisplayOverShortAmount":
                                    DisplayOverShortAmount = Conversion.XmlStringToBool(current.Value);
                                    break;
                                case "ShowIndividualDeposits":
                                    ShowIndividualDeposits = Conversion.XmlStringToBool(current.Value);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            errorLogger?.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
                        }
                    }
                }
            }
        }
    }
}
