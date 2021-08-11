using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TaxFree;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Enums;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services
{
    public partial class TaxFreeService
    {
        private string doubleLine;
        private string singleLine;
        private string underLine;

        public void SetLogo()
        {
            Printer.LoadCustomLogo(2, @"C:\Temp\taxlogo.bmp", 400);
        }

        public void PrintTaxFreeReport(IConnectionManager entry, RetailTransaction retailTransaction, TaxFreeConfig config)
        {
            doubleLine = "".PadLeft(config.LineWidth, '=');
            singleLine = "".PadLeft(config.LineWidth, '-');
            underLine = "".PadLeft(config.LineWidth, '_');

            StringBuilder reportLayout = new StringBuilder();
            CreateTaxFreeReport(entry, retailTransaction, reportLayout, config);

            //Tax free Worldwide Info and Logo
            Printer.PrintNormal(entry, "<L2>");
            Printer.PrintNormal(entry, PrintTaxInfo(config));

            List<BarcodePrintInfo> barcodePrintInfo = new List<BarcodePrintInfo>();

            //Barcode 1
            string bc1 = CreateBarcode1(config, retailTransaction.OrgTerminal, retailTransaction.ReceiptId);
            barcodePrintInfo.Add(new BarcodePrintInfo(BarcodePrintTypeEnum.Receipt, bc1));

            //Barcode 2
            string bc2 = CreateBarcode2((retailTransaction.ITaxLines.Count() > 1) ? 0 : retailTransaction.NetAmountWithTax, retailTransaction.EndDateTime, bc1);
            barcodePrintInfo.Add(new BarcodePrintInfo(BarcodePrintTypeEnum.Receipt, bc2));

            Printer.PrintReceipt(entry, reportLayout.ToString(), barcodePrintInfo);
        }

        private string PrintTaxInfo(TaxFreeConfig config)
        {
            StringBuilder reportLayout = new StringBuilder();

            //Text 1 - Tax free worldwide details
            //TODO: Add a form layout for this information

            if (string.IsNullOrWhiteSpace(config.Text) || string.IsNullOrWhiteSpace(config.Address) || string.IsNullOrWhiteSpace(config.VatNumber))
                return Properties.Resources.BCErrTaxFreeInfo;

            reportLayout.AppendLine(CenterString(Properties.Resources.OPOS_VatFormIssued, config.LineWidth));
            reportLayout.AppendLine(CenterString(config.Text, config.LineWidth));
            reportLayout.AppendLine(CenterString(config.Address, config.LineWidth));
            reportLayout.AppendLine(CenterString(config.PostcodeCity, config.LineWidth));
            reportLayout.AppendLine(CenterString(config.Country, config.LineWidth));
            reportLayout.AppendLine(CenterString(config.Phone, config.LineWidth));
            reportLayout.AppendLine(CenterString(config.Web, config.LineWidth));
            reportLayout.AppendLine(CenterString(config.VatNumber, config.LineWidth));
            reportLayout.AppendLine();
            return reportLayout.ToString();
        }

        private void CreateTaxFreeReport(IConnectionManager entry, RetailTransaction retailTransaction, StringBuilder reportLayout, TaxFreeConfig config)
        {

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            //BLANK LINE
            reportLayout.AppendLine();

            // Date and time of sale
            //1       10        20        30        40        50   55
            //Date of sale: xx.xx.xxxx              Time: xx:xx
            int col2 = 15;
            int col1 = config.LineWidth - col2;

            reportLayout.Append(FormatString(Properties.Resources.OPOS_Date + " " + retailTransaction.BeginDateTime.ToShortDateString(), col1, PadDirection.Right));
            reportLayout.Append(FormatString(Properties.Resources.OPOS_Time + " " + retailTransaction.BeginDateTime.ToShortTimeString(), col2, PadDirection.Left));
            reportLayout.AppendLine();

            //Text 4 - Tourist Details part 1
            reportLayout.AppendLine();
            reportLayout.AppendLine(Properties.Resources.OPOS_PleaseProvideCCNumber);

            //BLANK LINE
            reportLayout.AppendLine();

            //Credit card number
            reportLayout.AppendLine("_ _ _ _   _ _ _ _   _ _ _ _   _ _ _ _");

            //BLANK LINE
            reportLayout.AppendLine();

            //Donate to charity
            reportLayout.AppendLine(Properties.Resources.OPOS_DonateCharity + " _____");

            //BLANK LINE
            reportLayout.AppendLine();

            //Retailer declaration title
            reportLayout.AppendLine(InvertLine(Properties.Resources.OPOS_RetailerDeclarationHeader, config.LineWidth));

            //BLANK LINE
            reportLayout.AppendLine();

            //Text 3 - Retailer details and declaration part 1
            reportLayout.AppendLine(settings.Store.Text);
            reportLayout.AppendLine(settings.Store.Address.Address1);
            if (settings.Store.Address.Address2.Trim() != "")
            {
                reportLayout.AppendLine(settings.Store.Address.Address2);
            }
            reportLayout.AppendLine(settings.Store.Address.Zip + "  " + settings.Store.Address.City);
            reportLayout.AppendLine(Properties.Resources.OPOS_VatNo + " " + settings.CompanyInfo.TaxNumber);
            reportLayout.AppendLine();
            reportLayout.AppendLine(Properties.Resources.OPOS_SalesReceipt);

            //SEPARATOR
            reportLayout.AppendLine(singleLine);

            // Prices and Items
            //1       10        20        30        40        50   55
            //Description                 Qty                  Amount        
            int col3 = 13;
            col2 = 8;
            col1 = config.LineWidth - col2 - col3;

            reportLayout.Append(FormatString(Properties.Resources.OPOS_Description, col1, PadDirection.Right));
            reportLayout.Append(FormatString(Properties.Resources.OPOS_Qty, col2, PadDirection.Right));
            reportLayout.AppendLine(FormatString(Properties.Resources.OPOS_Amount, col3, PadDirection.Left));
            reportLayout.AppendLine(singleLine);

            var rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            foreach (var sli in retailTransaction.SaleItems.Where(w => !w.Voided))
            {
                string amount = rounding.RoundString(entry,
                                        sli.NetAmountWithTax,
                                        settings.Store.Currency,
                                        false,
                                        CacheType.CacheTypeTransactionLifeTime);

                string qty = rounding.RoundString(entry,
                                        sli.Quantity,
                                        settings.Store.Currency,
                                        false,
                                        CacheType.CacheTypeTransactionLifeTime);

                reportLayout.Append(FormatString(sli.Description, col1, PadDirection.Right));
                reportLayout.Append(FormatString(qty, col2, PadDirection.Right));
                reportLayout.Append(FormatString(amount, col3, PadDirection.Left));
                reportLayout.AppendLine();
            }

            //Price including VAT
            //1       10        20        30        40        50   55
            //Price incl. VAT:             CUR                 Amount
            string total = rounding.RoundString(entry,
                                        retailTransaction.NetAmountWithTax,
                                        settings.Store.Currency,
                                        false,
                                        CacheType.CacheTypeTransactionLifeTime);

            reportLayout.AppendLine(doubleLine);
            reportLayout.Append(FormatString(Properties.Resources.OPOS_PriceInclVAT, col1, PadDirection.Right));
            reportLayout.Append(FormatString(settings.Store.Currency, col2, PadDirection.Right));
            reportLayout.AppendLine(FormatString(total, col3, PadDirection.Left));

            //Price incl VAT in words
            reportLayout.AppendLine(SpellOutNumber(settings, total));
            reportLayout.AppendLine();

            // Seperate Tax sums per Tax groups
            decimal totalrefund = 0;
            decimal totaltax = 0;
            foreach (var taxit in retailTransaction.ITaxLines)
            {
                if (taxit.Percentage == 0)
                    continue;

                string netamount = rounding.RoundString(entry,
                                        taxit.PriceWithTax,
                                        settings.Store.Currency,
                                        false,
                                        CacheType.CacheTypeTransactionLifeTime);

                string amount = rounding.RoundString(entry,
                                        taxit.PriceWithTax - taxit.Amount,
                                        settings.Store.Currency,
                                        false,
                                        CacheType.CacheTypeTransactionLifeTime);

                string taxamount = rounding.RoundString(entry,
                                        taxit.Amount,
                                        settings.Store.Currency,
                                        false,
                                        CacheType.CacheTypeTransactionLifeTime);

                string perc = rounding.RoundForReceipt(entry, taxit.Percentage, 2);

                if (retailTransaction.ITaxLines.Count() > 1)
                {
                    reportLayout.Append(FormatString(Properties.Resources.OPOS_PriceInclVAT + " @  " + FormatString(perc, 5, PadDirection.Left) + "%", col1, PadDirection.Right));
                    reportLayout.Append(FormatString(settings.Store.Currency, col2, PadDirection.Right));
                    reportLayout.Append(FormatString(netamount, col3, PadDirection.Left));
                    reportLayout.AppendLine();
                }

                reportLayout.Append(FormatString(Properties.Resources.OPOS_PriceExclVAT + " @  " + FormatString(perc, 5, PadDirection.Left) + "%", col1, PadDirection.Right));
                reportLayout.Append(FormatString(settings.Store.Currency, col2, PadDirection.Right));
                reportLayout.Append(FormatString(amount, col3, PadDirection.Left));
                reportLayout.AppendLine();
                reportLayout.Append(FormatString(Properties.Resources.OPOS_VAT + " @  " + FormatString(perc, 5, PadDirection.Left) + "%", col1, PadDirection.Right));
                reportLayout.Append(FormatString(settings.Store.Currency, col2, PadDirection.Right));
                reportLayout.Append(FormatString(taxamount, col3, PadDirection.Left));
                reportLayout.AppendLine();
                reportLayout.AppendLine();

                totaltax += taxit.Amount;
                TaxRefundRange taxRange = DataProviderFactory.Instance.Get<ITaxRefundRangeData, TaxRefundRange>().GetForValue(entry, taxit.PriceWithTax, taxit.Percentage);
                if (taxRange == null) 
                    continue;

                if (taxRange.TaxRefund == 0 && taxRange.TaxRefundPercentage > 0)
                {
                    decimal d = taxit.PriceWithTax * (taxRange.TaxRefundPercentage / 100);
                    totalrefund += decimal.Round(d, 0);
                }
                else
                {
                    totalrefund += taxRange.TaxRefund;
                }
            }

            if (retailTransaction.NetAmountWithTax < config.MinTaxRefundLimit)
            {
                totalrefund = 0;
                totaltax = 0;
            }

            // Admin Fee value
            string adfee = rounding.RoundString(entry,
                                        totaltax - totalrefund,
                                        settings.Store.Currency,
                                        false,
                                        CacheType.CacheTypeTransactionLifeTime);

            reportLayout.AppendLine();
            reportLayout.Append(FormatString(Properties.Resources.OPOS_AdminFee, col1, PadDirection.Right));
            reportLayout.Append(FormatString(settings.Store.Currency, col2, PadDirection.Right));
            reportLayout.AppendLine(FormatString(adfee, col3, PadDirection.Left));
            reportLayout.AppendLine(singleLine);

            // Refund amount
            string refund = rounding.RoundString(entry,
                                        totalrefund,
                                        settings.Store.Currency,
                                        false,
                                        CacheType.CacheTypeTransactionLifeTime);

            reportLayout.Append(FormatString(Properties.Resources.OPOS_Refund, col1, PadDirection.Right));
            reportLayout.Append(FormatString(settings.Store.Currency, col2, PadDirection.Right));
            reportLayout.AppendLine(FormatString(refund, col3, PadDirection.Left));
            reportLayout.AppendLine(singleLine);

            // Text 3 – Retailer Details and Declaration part 2 
            reportLayout.AppendLine(Properties.Resources.OPOS_RetailLine1);
            reportLayout.AppendLine(Properties.Resources.OPOS_RetailLine2);
            reportLayout.AppendLine(Properties.Resources.OPOS_RetailLine3);
            reportLayout.AppendLine(Properties.Resources.OPOS_RetailLine4);
            reportLayout.AppendLine(Properties.Resources.OPOS_RetailLine5);
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine(SignLine(Properties.Resources.OPOS_RetailSign, config.LineWidth));

            // Text3 – Tourist Details Title
            reportLayout.AppendLine(InvertLine(Properties.Resources.OPOS_CustHead, config.LineWidth));

            // Text5 – Tourist Details part 2 
            reportLayout.AppendLine(Properties.Resources.OPOS_CustInfo);
            reportLayout.AppendLine();
            reportLayout.AppendLine(SignLine(Properties.Resources.OPOS_CustName, config.LineWidth));
            reportLayout.AppendLine();
            reportLayout.AppendLine(SignLine(Properties.Resources.OPOS_CustAddr, config.LineWidth));
            reportLayout.AppendLine();
            reportLayout.AppendLine(underLine);
            reportLayout.AppendLine();
            reportLayout.AppendLine(SignLine(Properties.Resources.OPOS_CustPost, config.LineWidth));
            reportLayout.AppendLine();
            reportLayout.AppendLine(SignLine(Properties.Resources.OPOS_CustCountry, config.LineWidth));
            reportLayout.AppendLine();
            reportLayout.AppendLine(SignLine(Properties.Resources.OPOS_CustPassport, config.LineWidth));
            reportLayout.AppendLine();
            reportLayout.AppendLine(SignLine(Properties.Resources.OPOS_CustIssue, config.LineWidth));
            reportLayout.AppendLine();
            reportLayout.AppendLine(SignLine(Properties.Resources.OPOS_CustEmail, config.LineWidth));
            reportLayout.AppendLine();
            reportLayout.AppendLine(Properties.Resources.OPOS_CustLine1);
            reportLayout.AppendLine(Properties.Resources.OPOS_CustLine2);
            reportLayout.AppendLine(Properties.Resources.OPOS_CustLine3);
            reportLayout.AppendLine(Properties.Resources.OPOS_CustLine4);
            reportLayout.AppendLine();
            reportLayout.AppendLine(Properties.Resources.OPOS_CustLine5);
            reportLayout.AppendLine(Properties.Resources.OPOS_CustLine6);
            reportLayout.AppendLine(Properties.Resources.OPOS_CustLine7);
            reportLayout.AppendLine(Properties.Resources.OPOS_CustLine8);
            reportLayout.AppendLine();
            reportLayout.AppendLine(Properties.Resources.OPOS_CustLine9);
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine(SignLine(Properties.Resources.OPOS_CustSign, config.LineWidth));

            // Text6 – Customs Section Title 
            reportLayout.AppendLine(InvertLine(Properties.Resources.OPOS_Stamp, config.LineWidth));

            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            
        }

        private string SpellOutNumber(ISettings settings, string number)
        {
            string toReturn = "";
            bool addDash = true;

            for (int i = 0; i < number.Length; i++)
            {
                switch (number[i])
                {
                    case '0':
                        toReturn += Properties.Resources.ZERO + "#1";
                        break;
                    case '1':
                        toReturn += Properties.Resources.ONE + "#1";
                        break;
                    case '2':
                        toReturn += Properties.Resources.TWO + "#1";
                        break;
                    case '3':
                        toReturn += Properties.Resources.THREE + "#1";
                        break;
                    case '4':
                        toReturn += Properties.Resources.FOUR + "#1";
                        break;
                    case '5':
                        toReturn += Properties.Resources.FIVE + "#1";
                        break;
                    case '6':
                        toReturn += Properties.Resources.SIX + "#1";
                        break;
                    case '7':
                        toReturn += Properties.Resources.SEVEN + "#1";
                        break;
                    case '8':
                        toReturn += Properties.Resources.EIGHT + "#1";
                        break;
                    case '9':
                        toReturn += Properties.Resources.NINE + "#1";
                        break;
                    case '.':
                        addDash = settings.CultureInfo.NumberFormat.CurrencyGroupSeparator == ",";
                        if (!addDash)
                        {
                            toReturn = toReturn.Substring(0, toReturn.Length - 1) + ". ";
                        }
                        break;
                    case ',':
                        addDash = settings.CultureInfo.NumberFormat.CurrencyGroupSeparator == ".";
                        if (!addDash)
                        {
                            toReturn = toReturn.Substring(0, toReturn.Length - 1) + ", ";
                        }
                        break;
                }
                toReturn = AddDashSeparator(toReturn, addDash);
            }
            return toReturn;
        }

        private string AddDashSeparator(string number, bool addDash)
        {
            if (addDash) 
                return number.Replace("#1", "-");

            return number.Replace("#1", " ");
        }


        /// <summary>
        /// BARCODE 1  (14 Digits)
        /// Field #          Digit # Len # Description 
        ///     1 #              1 #   1 # Country Code (ISO 3360 1 digit)
        ///     2 #              2 #   1 # Form type
        ///     3 #  3  4  5  6  7 #   5 # Tax Free Customer number 
        ///     4 #           8  9 #   2 # Terminal ID
        ///     5 # 10 11 12 13 14 #   5 # Sequence number 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="termid">Terminal ID</param>
        /// <param name="seqnr">Sequence number </param>
        /// <returns></returns>
        private string CreateBarcode1(TaxFreeConfig config, string termid, string seqnr)
        {
            if (string.IsNullOrWhiteSpace(config.CountryCode))
                return Properties.Resources.BCErrCountryCode;
            if (string.IsNullOrWhiteSpace(config.FormType))
                return Properties.Resources.BCErrFormType;
            if (string.IsNullOrWhiteSpace(config.TaxNumber))
                return Properties.Resources.BCErrTaxNr;
            if (string.IsNullOrWhiteSpace(termid))
                return Properties.Resources.BCErrTerm;
            if (config.TaxNumber.Length != 5)
                return Properties.Resources.BCErrTaxNrLen;

            return string.Format("{0}{1}{2}{3}{4}",
                config.CountryCode, 
                config.FormType,
                config.TaxNumber, 
                termid.Substring(termid.Length - 2, 2),
                seqnr.Substring(seqnr.Length - 5, 5));
        }

        /// <summary>
        /// BARCODE 2  (14 Digits)  
        /// Field #            Digit # Len # Description 
        ///     1 #              1 2 # 2 Checksum
        ///     2 # 3 4 5 6 7 8 9 10 # 8 Sale price
        ///     3 #               11 # 1 Last dig from year
        ///     4 #         12 13 14 # 3 Julian date 
        /// </summary>
        /// <param name="price">Sale price</param>
        /// <param name="regtime">Transaction Register DateTime</param>
        /// <param name="barcode1">Data from barcode 1 for checksum</param>
        /// <returns></returns>
        private string CreateBarcode2(decimal price, DateTime regtime, string barcode1)
        {

            string p = price.ToString("F2");
            p = p.Replace(".", "");
            p = p.Replace(",", "");
            long pp = Convert.ToInt64(p);

            string s = string.Format("{0}{1}{2}",
                pp.ToString("D8"),
                regtime.Year.ToString().Substring(3, 1),
                regtime.DayOfYear.ToString("D3")
                );

            CRC32 crc = new CRC32();
            byte[] b = crc.ComputeHash(barcode1 + s);
            long bb = BitConverter.ToUInt32(b, 0);
            Int64 bbb = bb % 10;
            return string.Format("{0}{1}", bbb.ToString("D2"), s);
        }

        private string FormatString(String tempString, int padSize, PadDirection padDir)
        {
            tempString = GetCorrectStringSize(tempString, padSize, padDir);
            if (padDir == PadDirection.Right)
                return tempString.PadRight(padSize);

            return tempString.PadLeft(padSize);
        }

        private string CenterString(string tempString, int width)
        {
            if (tempString.Length > width)
                return tempString.Substring(0, width);

            int widthmid = width / 2;
            int strmid = tempString.Length / 2;

            return tempString.PadLeft(widthmid - strmid + tempString.Length).PadRight(width);
        }

        private string InvertLine(string tempString, int width)
        {
            return string.Format("{0}B1{1}{0}B0", ((char) 29), CenterString(tempString, width));
        }

        private string SignLine(string tempString, int width)
        {
            return tempString.PadRight(width, '_');
        }

        private string GetCorrectStringSize(string tempString, int size, PadDirection padDir)
        {
            if (tempString.Length <= size) 
                return tempString;

            if (padDir == PadDirection.Left)
                return tempString.Substring(tempString.Length - size, size);
               
            return tempString.Substring(0, size);
        }
    }
}

