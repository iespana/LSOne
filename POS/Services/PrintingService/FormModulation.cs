using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Forms;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.CreditMemoItem;
using LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Peripherals.OPOS;
using LSOne.Services.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Form = LSOne.DataLayer.BusinessObjects.Forms.Form;

namespace LSOne.Services
{
	public partial class FormModulation
	{
		#region Member variables
		protected string copyText = string.Empty;
		protected bool isReprint;
		
		protected RecordIdentifier formProfileID;
		protected string formProfileDescription;

		protected RecordIdentifier emailFormProfileID;

		protected IConnectionManager prnEntry;
		protected ISettings prnSettings;
		protected IErrorLog errorLog;

		protected List<FormType> formTypeList;

		public List<BarcodePrintInfo> BarcodePrintInfoList;
		
		#endregion

		public FormModulation(IConnectionManager entry)
		{
			prnEntry = entry;
			prnSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
			errorLog = entry.ErrorLogger;
			formTypeList = new List<FormType>();
			BarcodePrintInfoList = new List<BarcodePrintInfo>();

			formProfileID = prnSettings.Store.FormProfileID;
			emailFormProfileID = prnSettings.Store.EmailFormProfileID;

			if (formProfileID.IsGuid)
			{
				this.formProfileID = formProfileID != Guid.Empty ? formProfileID : FormProfile.DefaultProfileID;
			}

			if (emailFormProfileID.IsGuid)
			{
				this.emailFormProfileID = emailFormProfileID != Guid.Empty ? emailFormProfileID : FormProfile.DefaultProfileID;
			}
		}

		protected virtual DataSet LoadXmlForm(FormSystemType systemType, FormPart formPart, bool useEmailProfile = false)
		{
			return LoadXmlForm(systemType, formPart, useEmailProfile ? emailFormProfileID : formProfileID);
		}

		protected virtual DataSet LoadXmlForm(FormPart formPart, Form form)
		{
			return GetFormContent(formPart, form);
		}

		protected  virtual DataSet LoadXmlForm(FormSystemType systemType, FormPart formPart, RecordIdentifier profileID)
			//, ref string header, ref string details, ref string footer)
		{
			Form form = Providers.FormData.GetProfileForm(prnEntry, profileID, systemType, CacheType.CacheTypeTransactionLifeTime);
			return GetFormContent(formPart, form);
		}

		private DataSet GetFormContent(FormPart formPart, Form form)
		{
			DataSet newDataSet = new DataSet("New DataSet");
			if (form != null)
			{
				string byteString = "";
				switch (formPart)
				{
					case FormPart.Header:
						byteString = form.HeaderXml;
						break;
					case FormPart.Line:
						byteString = form.LineXml;
						break;
					case FormPart.Footer:
						byteString = form.FooterXml;
						break;
				}
				if (byteString.Length > 0)
				{
					int discarded;
					byte[] buffer = HexEncoding.GetBytes(byteString, out discarded);

					if (buffer != null)
					{
						MemoryStream myStream = new System.IO.MemoryStream();
						myStream.Write(buffer, 0, buffer.Length);
						myStream.Position = 0;

						newDataSet.ReadXml(myStream);
						myStream.Close();
					}

					// Adding detail table to the dataset
					DataTable formDetails = new DataTable {TableName = "FormDetails"};

					// Adding columns to items data
					DataColumn col = new DataColumn("ID", typeof(string));
					formDetails.Columns.Add(col);
					col = new DataColumn("Description", typeof(string));
					formDetails.Columns.Add(col);
					col = new DataColumn("UpperCase", typeof(bool));
					formDetails.Columns.Add(col);
					col = new DataColumn("LineCountPrPage", typeof(Int16));
					formDetails.Columns.Add(col);
					col = new DataColumn("DATAAREAID", typeof(string));
					formDetails.Columns.Add(col);

					object[] row = new Object[5];

					row[0] = (string) form.ID;
					row[1] = form.Text;
					row[2] = form.UpperCase;
					row[3] = form.LineCountPerPage;
					row[4] = prnEntry.Connection.DataAreaId;

					formDetails.Rows.Add(row);

					newDataSet.Tables.Add(formDetails);
				}
			}
			return newDataSet;
		}

		protected virtual string CreateWhitespace(int stringLength, char seperator)
		{
			if (stringLength < 0)
			{
				return "";
			}

			return new string(seperator, stringLength);
		}
        //**NUM TO WORDS-NUMEROS A LETRAS**//
        private static string[] _ones =
        {
        "cero",
        "un",
        "dos",
        "tres",
        "cuatro",
        "cinco",
        "seis",
        "siete",
        "ocho",
        "nueve"
        };

        private static string[] _teens =
        {
        "diez",
        "once",
        "doce",
        "trece",
        "catorce",
        "quince",
        "dieciseis",
        "diecisiete",
        "dieciocho",
        "diecinueve"
        };

        private static string[] _tens =
        {
        "",
        "diez",
        "veinte",
        "treinta",
        "cuarenta",
        "cincuenta",
        "sesenta",
        "setenta",
        "ochenta",
        "noventa"
        };

        private static string[] _centenas =
        {
        "",
        "ciento",
        "doscientos",
        "trescientos",
        "cuatrocientos",
        "quinientos",
        "seiscientos",
        "setecientos",
        "ochocientos",
        "novecientos"
        };

        private static string[] _thousands =
        {
        "",
        "mil",
        "millon",
        "billon",
        "trillon",
        "cuatrillon"
        };

        /// <summary>
        /// Converts a numeric value to words suitable for the portion of
        /// a check that writes out the amount.
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="copyReceipt">True if it is duplicate</param>
        /// <returns></returns>
        public static string ConvertToWords(decimal value)
        {
            string digits, temp;
            bool showThousands = false;
            bool allZeros = true;

            // Use StringBuilder to build result
            StringBuilder builder = new StringBuilder();
            // Convert integer portion of value to string
            digits = ((long)value).ToString();
            // Traverse characters in reverse order
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                int ndigit = (int)(digits[i] - '0');
                int column = (digits.Length - (i + 1));

                // Determine if ones, tens, or hundreds column
                switch (column % 3)
                {
                    case 0:        // Ones position
                        showThousands = true;
                        if (i == 0)
                        {
                            // First digit in number (last in loop)
                            temp = String.Format("{0} ", _ones[ndigit]);
                        }
                        else if (digits[i - 1] == '1')
                        {
                            // This digit is part of "teen" value
                            temp = String.Format("{0} ", _teens[ndigit]);
                            // Skip tens position
                            i--;
                        }
                        else if (ndigit != 0)
                        {
                            // Any non-zero digit
                            //if (column == 0)
                            //{
                            temp = String.Format("{0} ", _ones[ndigit]);
                            //}
                            //else
                            //{
                            //    temp = String.Format("{0} ", ndigit == 1 ? "un" : _ones[ndigit]);
                            //}
                        }
                        else
                        {
                            // This digit is zero. If digit in tens and hundreds
                            // column are also zero, don't show "thousands"
                            temp = String.Empty;
                            // Test for non-zero digit in this grouping
                            if (digits[i - 1] != '0' || (i > 1 && digits[i - 2] != '0'))
                                showThousands = true;
                            else
                                showThousands = false;
                        }

                        // Show "thousands" if non-zero in grouping
                        if (showThousands)
                        {
                            if (column > 0)
                            {
                                temp = String.Format("{0}{1}{2}",
                                temp,
                                _thousands[column / 3],
                                allZeros ? " " : ", ");
                            }
                            // Indicate non-zero digit encountered
                            allZeros = false;
                        }
                        builder.Insert(0, temp);
                        break;

                    case 1:        // Tens column
                        if (ndigit > 0)
                        {
                            temp = String.Format("{0}{1}",
                            _tens[ndigit],
                            (digits[i + 1] != '0') ? " y " : " ");
                            builder.Insert(0, temp);
                        }
                        break;

                    case 2:        // Hundreds column
                        if (ndigit > 0)
                        {
                            temp = String.Format("{0} ", _centenas[ndigit]);
                            builder.Insert(0, temp);
                        }
                        break;
                }
            }

            // Append fractional portion/cents
            builder.AppendFormat("Lempiras con {0:00}/100 Centavos", (value - (long)value) * 100);

            // Capitalize first letter
            return String.Format("{0}{1}",
            Char.ToUpper(builder[0]),
            builder.ToString(1, builder.Length - 1));
        }//**END NUM TO WORDS**//

        //**WRAP**//
        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">Text to be word wrapped</param>
        /// <param name="width">Width, in characters, to which the text
        /// should be word wrapped</param>
        /// <returns>The modified text</returns>
        public static string WordWrap(string text, int width)
        {
            int pos, next;
            StringBuilder sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return text;

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(Environment.NewLine, pos);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + Environment.NewLine.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > width)
                            len = BreakLine(text, pos, width);
                        sb.Append(text, pos, len);
                        sb.Append(Environment.NewLine);

                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    } while (eol > pos);
                }
                else sb.Append(Environment.NewLine); // Empty line
            }
            return sb.ToString();
        }

        /// <summary>
        /// Locates position to break the given line so as to avoid
        /// breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <returns>The modified line length</returns>
        private static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            // If no whitespace found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }
        //**END WRAP**//

        protected virtual string GetInfoFromTransaction(FormItemInfo itemInfo, IEFTInfo eftInfo, ITenderLineItem tenderItem, IPosTransaction trans, FormPartEnum formPart)
		{
			string returnValue = string.Empty;

			try
			{
				if (trans != null)
				{
					IRoundingService rounding = (IRoundingService)prnEntry.Service(ServiceType.RoundingService);
					RetailTransaction retailTrans = trans as RetailTransaction;

					string variable = itemInfo.Variable.ToUpperInvariant().Replace(" ", string.Empty);

					bool variableFound = false;
					string variableValue = string.Empty;

					// check if Fiscal Service overrides all functionality
					variableValue = GetActiveFiscalService()?.FiscalGetInfoFromTransaction(variable, prnEntry, prnSettings, trans, out variableFound);
					if(variableFound)
					{
						returnValue = variableValue;
					}
					// Check if partner code overrides base functionality
					else
					{ 
						variableValue = PartnerGetInfoFromTransaction(variable, eftInfo, tenderItem, trans, formPart, out variableFound);

						if (variableFound)
						{
							returnValue = variableValue;
						}
						//Check if it's part of the base functionality
						else
						{
							variableValue = GetDefaultInfoFromTransaction(variable, rounding, trans, retailTrans, tenderItem, eftInfo, out variableFound);
							if(variableFound)
							{
								returnValue = variableValue;
							}
							//Check if is a partner customization (extra variable)
							else
							{ 
								returnValue = GetPartnerInfoFromTransaction(itemInfo, eftInfo, tenderItem, trans, formPart);
							}

						}
					}
				}
			}
			catch (ArgumentOutOfRangeException)
			{
				//TODO: Log the error as warning

				return string.Empty;
			}
			catch (NullReferenceException)
			{
				//TODO: Log the error as warning

				return string.Empty;
			}

			if (returnValue == null)
			{
				returnValue = string.Empty;
			}
			else
			{
				if (itemInfo.Prefix.Length > 0)
				{
					returnValue = itemInfo.Prefix + returnValue;
				}
			}

			return returnValue;
		}

		protected virtual Address GetAddress(RetailTransaction retailTrans, Address.AddressTypes addressType)
		{
			if (retailTrans != null)
			{
				switch(addressType)
				{
					case Address.AddressTypes.Shipping:
						return retailTrans.Customer.DefaultShippingAddress;
					case Address.AddressTypes.Billing:
						return retailTrans.Customer.DefaultBillingAddress;
				}
			}
			return null;
		}

		protected virtual string GetAddressString(RetailTransaction retailTrans, Address.AddressTypes addressType)
		{
			Address address = GetAddress(retailTrans, addressType);
			if (address != null)
			{
				return prnEntry.Settings.LocalizationContext.FormatSingleLine(address, prnEntry.Cache);
			}
			return "";
		}

		protected virtual string GetInfoFromSaleLineItem(IConnectionManager entry, FormItemInfo itemInfo, SaleLineItem saleLine, out bool skipIfEmptyLine)
		{
			skipIfEmptyLine = false;
			string returnValue = "";
			try
			{
				ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
				IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

				if (saleLine != null)
				{
					switch (itemInfo.Variable.ToUpperInvariant().Replace(" ", ""))
					{
						case "TAXID":
							TaxItem taxItem = (saleLine.TaxLines.Count > 0) ? saleLine.TaxLines[0] : null;

                            if (taxItem != null && taxItem.TaxExempt)
                            {
                                break;
                            }

                            if (taxItem != null && !String.IsNullOrEmpty(taxItem.ItemTaxGroupDisplay.Trim()))
							{
								returnValue = taxItem.ItemTaxGroupDisplay;
							}
							else
							{
								returnValue = saleLine.TaxGroupId;
							}
							break;
						case "TAXPERCENT":
							returnValue = rounding.RoundString(entry, saleLine.TaxRatePct, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "ITEMNAME":
							returnValue = (saleLine.IsAssemblyComponent ? "> " : "") + saleLine.Description;
							break;
						case "ITEMNAMEALIAS":
							returnValue = saleLine.DescriptionAlias;
							break;
						case "ITEMID":
							returnValue = saleLine.ItemId;
							break;
						case "ITEMBARCODE":
							returnValue = saleLine.BarcodeId;
							skipIfEmptyLine = true;
							break;
						case "QTY":
							returnValue = rounding.RoundQuantity(entry,
																 saleLine.Quantity,
																 saleLine.SalesOrderUnitOfMeasure,
																 saleLine.SplitItem || saleLine.LimitationSplitParentLineId > 0 || saleLine.LimitationSplitChildLineId > 0,
																 settings.Store.Currency,
																 CacheType.CacheTypeTransactionLifeTime);
							break;
						case "MANUALLYENTEREDWEIGHT":
							returnValue = (saleLine.WeightManuallyEntered) ? Properties.Resources.ManualWeight : ""; 
							break;
						case "SCALEINFO":                            
							returnValue = Interfaces.Services.ScaleService(entry).GetScalePrintInformation(entry, saleLine, 50);
							break;
						case "UNITPRICE":
							returnValue = saleLine.ShouldCalculateAndDisplayAssemblyPrice() ? rounding.RoundPrecision(entry, saleLine.PriceWithTax, settings.FunctionalityProfile.PriceDecimalPlaces, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime) : "";
							break;
						case "TOTALPRICE":
							returnValue = saleLine.ShouldCalculateAndDisplayAssemblyPrice() ? rounding.RoundString(entry, saleLine.GrossAmountWithTax, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime) : "";
							break;
						case "NETTOTALPRICE":
							returnValue = saleLine.ShouldCalculateAndDisplayAssemblyPrice() || saleLine.ShouldDisplayTotalAssemblyComponentPrice(ExpandAssemblyLocation.OnReceipt) ? rounding.RoundString(entry, saleLine.GetCalculatedNetAmount(true), settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime) : "";
							break;
						case "ITEMUNITID":
							returnValue = saleLine.SalesOrderUnitOfMeasure;
							break;
						case "ITEMUNITIDNAME":
							returnValue = saleLine.SalesOrderUnitOfMeasureName;
							break;
						case "LINEDISCOUNTAMOUNT":
							if (saleLine.LoyaltyDiscountWithTax != decimal.Zero || saleLine.LoyaltyDiscount != decimal.Zero)
							{
								returnValue = rounding.RoundString(entry, settings.Store.DisplayAmountsWithTax ? saleLine.LoyaltyDiscountWithTax : saleLine.LoyaltyDiscount, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							}
							else
							{
								returnValue = rounding.RoundString(entry, settings.Store.DisplayAmountsWithTax ? saleLine.LineDiscountWithTax : saleLine.LineDiscount, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							}
							break;
						case "LINEDISCOUNTPERCENT":
							if (saleLine.LoyaltyPctDiscount != decimal.Zero)
							{
								returnValue = rounding.RoundString(entry, saleLine.LoyaltyPctDiscount, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							}
							else
							{
								returnValue = rounding.RoundString(entry, saleLine.LinePctDiscount, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							}
							break;
						case "PERIODICDISCOUNTAMOUNT":
							returnValue = rounding.RoundString(entry, settings.Store.DisplayAmountsWithTax ? saleLine.PeriodicDiscountWithTax : saleLine.PeriodicDiscount, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "PERIODICDISCOUNTNAME":
							returnValue = saleLine.PeriodicDiscountOfferName;
							break;
						case "PERIODICDISCOUNTPERCENT":
							returnValue = rounding.RoundString(entry, saleLine.PeriodicPctDiscount, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "TOTALDISCOUNTAMOUNT":
							returnValue = rounding.RoundString(entry, settings.Store.DisplayAmountsWithTax ? saleLine.TotalDiscountWithTax : saleLine.TotalDiscount, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "TOTALDISCOUNTPERCENT":
							returnValue = rounding.RoundString(entry, saleLine.TotalPctDiscount, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "PHARMACYDOSAGESTRENGTH":
							returnValue = Conversion.ToStr(((PharmacySalesLineItem)saleLine).DosageStrength);
							break;
						case "PHARMACYPRESCRIPTIONNUMBER":
							returnValue = Conversion.ToStr(((PharmacySalesLineItem)saleLine).PrescriptionId);
							break;
						case "PHARMACYDOSAGESTRENGTHUNIT":
							returnValue = ((PharmacySalesLineItem)saleLine).DosageStrengthUnit;
							break;
						case "PHARMACYDOSAGEUNITQTY":
							returnValue = Conversion.ToStr(((PharmacySalesLineItem)saleLine).DosageUnitQuantiy);
							break;
						case "PHARMACYDOSAGETYPE":
							returnValue = ((PharmacySalesLineItem)saleLine).DosageType;
							break;
						case "BATCHID":
							returnValue = saleLine.BatchId;
							break;
						case "BATCHEXPDATE":
							returnValue = saleLine.BatchExpDate.ToShortDateString();
							break;
						case "ITEMGROUP":
							returnValue = saleLine.ItemGroupId;
							break;
						case "VARIANTTEXT":
							returnValue = saleLine.VariantName;
							break;
						case "DIMENSIONCOLORID":
							returnValue = (string)saleLine.Dimension.ColorID;
							break;
						case "DIMENSIONCOLORVALUE":
							returnValue = saleLine.Dimension.ColorName;
							break;
						case "DIMENSIONSIZEID":
							returnValue = (string)saleLine.Dimension.SizeID;
							break;
						case "DIMENSIONSIZEVALUE":
							returnValue = saleLine.Dimension.SizeName;
							break;
						case "DIMENSIONSTYLEID":
							returnValue = (string)saleLine.Dimension.StyleID;
							break;
						case "DIMENSIONSTYLEVALUE":
							returnValue = saleLine.Dimension.StyleName;
							break;
						case "DIMENSIONID":
							returnValue = (string) saleLine.Dimension.VariantNumber;
							if (string.IsNullOrEmpty(returnValue))
								returnValue = saleLine.ItemId;
							break;
						case "DIMENSIONTEXT":
							returnValue = saleLine.Dimension.Text;
							break;
						case "ITEMCOMMENT":
							returnValue = saleLine.Comment;
							break;
						case "SERIALID":
							returnValue = saleLine.SerialId;
							break;
						case "RETURNRECEIPTNUMBER":
							{
								if (saleLine.ReturnedQty != 0m &&
									(saleLine.ReturnReceiptId.Replace('0', ' ').Length > 0))
								{
                                    return Interfaces.Services.BarcodeService(entry).GetReceiptBarCodeData(entry, new BarcodeReceiptParseInfo(saleLine.ReturnStoreId, saleLine.ReturnTerminalId, saleLine.ReturnReceiptId));
								}
								else
									returnValue = "";
							}
							break;
						case "RFID":
							returnValue = saleLine.RFIDTagId;
							break;
						case "TOTALPRICEWITHOUTTAX":
							returnValue = rounding.RoundString(entry, saleLine.GrossAmount, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "UNITPRICEWITHOUTTAX":
							returnValue = rounding.RoundString(entry, saleLine.Price, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "NETTOTALPRICEWITHOUTTAX":
							returnValue = rounding.RoundString(entry, saleLine.NetAmount, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "TAXAMOUNT":
							returnValue = rounding.RoundString(entry, saleLine.TaxAmount, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "SALESPERSONID":
							returnValue = (string)saleLine.SalesPerson.Login;
							break;
						case "SALESPERSONNAME":
							returnValue = saleLine.SalesPerson.Name;
							break;
						case "LINEDISCOUNTNAME":
							foreach (IDiscountItem discount in saleLine.DiscountLines)
							{
								if (discount.DiscountType == DiscountTransTypes.LoyaltyDisc)
								{
									returnValue = discount.DiscountName != "" ? discount.DiscountName : Properties.Resources.LoyaltyDiscount;
									break;
								}
								else
								{
									returnValue = discount.DiscountName != "" ? discount.DiscountName : Properties.Resources.LineDiscount;
									break;
								}
							}
							break;
						case "TOTALDISCOUNTNAME":
							foreach (IDiscountItem discount in saleLine.DiscountLines)
							{
								switch (discount.DiscountType)
								{
									case DiscountTransTypes.Customer:
									case DiscountTransTypes.TotalDisc:
										returnValue = discount.DiscountName != "" ? discount.DiscountName : Properties.Resources.TotalDiscount;
										break;
									case DiscountTransTypes.Periodic:
									case DiscountTransTypes.LineDisc:
									case DiscountTransTypes.LoyaltyDisc:
									default:
										break;
								}
							}
							break;
						case "ORDERED":
							if (!saleLine.Order.Empty())
							{
								returnValue = rounding.RoundQuantity(entry,
																 saleLine.Order.Ordered,
																 saleLine.SalesOrderUnitOfMeasure,
																 settings.Store.Currency,
																 CacheType.CacheTypeTransactionLifeTime);
							}
							skipIfEmptyLine = string.IsNullOrEmpty(returnValue);
							break;
						case "ORDEREDTEXT":
							if (!saleLine.Order.Empty())
							{
								returnValue = Properties.Resources.Ordered;
							}
							skipIfEmptyLine = string.IsNullOrEmpty(returnValue);
							break;
						case "RECEIVED":
							if (!saleLine.Order.Empty())
							{
								returnValue = rounding.RoundQuantity(entry,
																 saleLine.Order.Received,
																 saleLine.SalesOrderUnitOfMeasure,
																 settings.Store.Currency,
																 CacheType.CacheTypeTransactionLifeTime);
							}
							skipIfEmptyLine = string.IsNullOrEmpty(returnValue);
							break;
						case "RECEIVEDTEXT":
							if (!saleLine.Order.Empty())
							{
								returnValue = Properties.Resources.Received;
							}
							skipIfEmptyLine = string.IsNullOrEmpty(returnValue);
							break;
						case "FORDELIVERY":
							if (!saleLine.Order.Empty() && saleLine.Order.ToPickUp != decimal.Zero && !saleLine.Order.FullyReceived)
							{
								returnValue = rounding.RoundQuantity(entry,
																 saleLine.Order.ToPickUp,
																 saleLine.SalesOrderUnitOfMeasure,
																 settings.Store.Currency,
																 CacheType.CacheTypeTransactionLifeTime);
							}
							skipIfEmptyLine = string.IsNullOrEmpty(returnValue);
							break;
						case "FORDELIVERYTEXT":
							if (!saleLine.Order.Empty() && saleLine.Order.ToPickUp != decimal.Zero && !saleLine.Order.FullyReceived)
							{
								returnValue = Properties.Resources.ForDelivery;
							}
							skipIfEmptyLine = string.IsNullOrEmpty(returnValue);
							break;
						case "FULLYRECEIVEDTEXT":
							if (!saleLine.Order.Empty() && (saleLine.Order.FullyReceived || saleLine.Order.Ordered == saleLine.Order.Received))
							{
								returnValue = Properties.Resources.ItemIsFullyReceived;
							}
							skipIfEmptyLine = string.IsNullOrEmpty(returnValue);
							break;
						case "DEPOSITPAID":
							if (!saleLine.Order.Empty())
							{
								returnValue = rounding.RoundString(entry, saleLine.Order.TotalDepositAmount(), settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							}
							skipIfEmptyLine = string.IsNullOrEmpty(returnValue);
							break;
						case "DEPOSITPAIDTEXT":
							if (!saleLine.Order.Empty())
							{
								returnValue = Properties.Resources.DepositPaid;
							}
							skipIfEmptyLine = string.IsNullOrEmpty(returnValue);
							break;
						case "CHANGEDITEM":
							if (saleLine.ChangedForPreparation && !saleLine.Voided && saleLine.Transaction.EntryStatus != TransactionStatus.Voided)
							{
								returnValue = Properties.Resources.Changed;
							}
							skipIfEmptyLine = string.IsNullOrEmpty(returnValue);
							break;
						case "VOIDEDITEM":
							if (saleLine.Voided || saleLine.Transaction.EntryStatus == TransactionStatus.Voided)
							{
								returnValue = Properties.Resources.Voided;
							}
							skipIfEmptyLine = string.IsNullOrEmpty(returnValue);
							break;
						default:
							returnValue = GetPartnerInfoFromSaleLineItem(entry, itemInfo, saleLine, out skipIfEmptyLine);
							break;
					}
					if (returnValue == null)
						returnValue = "";
					else
					{
						if (itemInfo.Prefix.Length > 0)
						{
							if (returnValue.Contains("-") && (itemInfo.Prefix.Length > 0))
							{
								//then we would come out with 2 "--" and then we change it to be a '+', which is the same as ' '.
								//this case happened when returning a transaction that had a discount on it. The discount would be printed as "$--0,14".
								returnValue = returnValue.Replace('-', ' ');
							}
							else  returnValue = itemInfo.Prefix + returnValue;
						}
					}
				}
			}
			catch (NullReferenceException)            {
				returnValue = "";
			}
			return returnValue;
		}

		protected virtual string GetInfoFromTenderLineItem(FormItemInfo itemInfo, ITenderLineItem tenderLine)
		{
			string returnValue = "";
			try
			{
				if (tenderLine != null)
				{
					switch (itemInfo.Variable.ToUpperInvariant().Replace(" ", ""))
					{
						case "TENDERNAME":
							if (tenderLine.TypeOfTender != TenderTypeEnum.DepositTender && tenderLine.Amount < 0)
							{
								returnValue = (tenderLine.TypeOfTender == TenderTypeEnum.CardTender || tenderLine.TypeOfTender == TenderTypeEnum.CorporateCardTender)
                                    ? Properties.Resources.CardRefund          
                                    : Properties.Resources.ChangeBack;
                            }
							else if (tenderLine.TypeOfTender == TenderTypeEnum.DepositTender)
							{
								returnValue = ((DepositTenderLineItem)tenderLine).RedeemedDeposit ? Properties.Resources.RedeemedDeposit : Properties.Resources.DepositPayment;
							}
							else
							{
								returnValue = tenderLine.Description;
							}                         

							break;
						case "TENDERAMOUNT":
							{
								if (tenderLine.TypeOfTender == TenderTypeEnum.DepositTender)
								{
									returnValue = Interfaces.Services.RoundingService(prnEntry).RoundString(prnEntry, tenderLine.Amount, prnSettings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
								}
								else
								{
									returnValue = Interfaces.Services.RoundingService(prnEntry).RoundString(prnEntry, Math.Abs(tenderLine.Amount), prnSettings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
								}
							}
							break;
						case "TENDERDATE":
							returnValue = tenderLine.BeginDateTime.ToShortDateString();
							break;
						case "TENDERCOMMENT":
							returnValue = tenderLine.Comment;
							break;
						case "TENDERDETAILS":                            
							returnValue = Interfaces.Services.TenderService(prnEntry).GetTenderDetails(prnEntry, tenderLine);
							break;
						case "CURRENCYSYMBOL":
							Currency currency = Providers.CurrencyData.Get(prnEntry, prnSettings.Store.Currency);
							returnValue = currency.Symbol;
							break;
						default:
							returnValue = GetPartnerInfoFromTenderLineItem(itemInfo, tenderLine);
							break;
					}
					if (returnValue == null)
					{
						returnValue = "";
					}
					else
					{
						if (itemInfo.Prefix.Length > 0)
						{
							returnValue = itemInfo.Prefix + returnValue;
						}
					}
				}
			}
			catch (NullReferenceException)
			{
				returnValue = "";
			}

			
			string[] returnValues = returnValue.Split(new[]{'\r','\n'},StringSplitOptions.RemoveEmptyEntries);
			foreach (string value in returnValues)
			{
				if (value.Length > itemInfo.Length)
				{
					if ((itemInfo.VertAlign == valign.left) || (itemInfo.VertAlign == valign.center))
					{

						returnValue = value.Substring(0, itemInfo.Length);
					}
					else
					{
						returnValue = value.Substring((value.Length - itemInfo.Length), itemInfo.Length);
					}
				}
			}
			if (returnValues.Length > 1)
			{
				returnValue = returnValues[0];
				for (int i = 1; i < returnValues.Length; i++ )
					returnValue += "\r\n" + returnValues[i];
			}
			return returnValue;
		}        

		protected virtual string GetInfoFromTaxItem(FormItemInfo itemInfo, TaxItem taxLine)
		{
			string returnValue = "";
			try
			{
				IRoundingService rounding = (IRoundingService)prnEntry.Service(ServiceType.RoundingService);

				if (taxLine != null)
				{
					switch (itemInfo.Variable.ToUpperInvariant().Replace(" ", ""))
					{
						case "TAXID":
							returnValue = String.IsNullOrEmpty(taxLine.TaxCodeDisplay) ? (string)taxLine.TaxCode : taxLine.TaxCodeDisplay.Trim();
							break;
						case "TAXGROUP":
							returnValue = String.IsNullOrEmpty(taxLine.ItemTaxGroupDisplay) ? (string)taxLine.ItemTaxGroup : taxLine.ItemTaxGroupDisplay.Trim();
							break;
						case "TAXPERCENTAGE":
							returnValue = rounding.RoundString(prnEntry, taxLine.Percentage, 2, false, prnSettings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "TOTAL":
							returnValue = rounding.RoundString(prnEntry, taxLine.PriceWithTax, prnSettings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "TOTALWITHOUTTAX":
							returnValue = rounding.RoundString(prnEntry, taxLine.PriceWithTax - taxLine.Amount, prnSettings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);                            
							break;
						case "TAXTOTAL":
							returnValue = rounding.RoundString(prnEntry, taxLine.PriceWithTax, prnSettings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "TAXAMOUNT":
							returnValue = rounding.RoundString(prnEntry, taxLine.Amount, prnSettings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "PRICEWITHOUTTAX":
							returnValue = rounding.RoundString(prnEntry, taxLine.PriceWithTax - taxLine.Amount, prnSettings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
							break;
						case "CURRENCYSYMBOL":
							Currency currency = Providers.CurrencyData.Get(prnEntry, prnSettings.Store.Currency);
							returnValue = currency.Symbol;
							break;
						default:
							returnValue = GetPartnerInfoFromTaxItem(itemInfo, taxLine);
							break;
					}
					if (returnValue == null)
					{
						returnValue = "";
					}
					else
					{
						if (itemInfo.Prefix.Length > 0)
						{
							returnValue = itemInfo.Prefix + returnValue;
						}
					}
				}
			}
			catch (NullReferenceException)
			{
				returnValue = "";
			}
			if (returnValue.Length > itemInfo.Length)
			{
				if ((itemInfo.VertAlign == valign.left) || (itemInfo.VertAlign == valign.center))
				{
					returnValue = returnValue.Substring(0, itemInfo.Length);
				}
				else
				{
					returnValue = returnValue.Substring((returnValue.Length - itemInfo.Length), itemInfo.Length);
				}
			}

			

			return returnValue;
		}

		protected virtual string GetInfoFromSuspendedAnswer(FormItemInfo itemInfo, SuspendedTransactionAnswer answer)
		{
			string returnValue = "";
			try
			{
				if (answer != null)
				{
					switch (itemInfo.Variable.ToUpperInvariant().Replace(" ", ""))
					{
						case "SUSPENDEDANSWER":
							{
								string descr = answer.ToString(prnEntry.Settings.LocalizationContext);
								if (answer.InformationType == SuspensionTypeAdditionalInfo.InfoTypeEnum.Date)
								{
									descr = Convert.ToDateTime(descr, prnSettings.CultureInfo.DateTimeFormat).ToShortDateString();
								}

								returnValue = answer.Prompt + ": " + descr + "\r\n";
							}
							break;
					}
					if (itemInfo.Prefix.Length > 0)
					{
						returnValue = itemInfo.Prefix + returnValue;
					}
				}
			}
			catch (NullReferenceException)
			{
				returnValue = "";
			}

			if (returnValue.Length > itemInfo.Length)
			{
				int whiteSpace = itemInfo.CharIndex - 1;
				string tmp = returnValue;
				returnValue = "";
				while (tmp.Length > itemInfo.Length)
				{
					returnValue += tmp.Substring(0, itemInfo.Length);
					returnValue += Environment.NewLine;

					if (whiteSpace > 0)
						returnValue += new string(' ', whiteSpace);

					tmp = tmp.Substring(itemInfo.Length);
				}

				// Add remainder
				if (tmp.Length > 0)
				{
					returnValue += tmp;
					returnValue += Environment.NewLine;
				}
			}

			return returnValue;
		}

		protected virtual string GetAlignmentSettings(string parsedString, FormItemInfo itemInfo)
		{
			// the indication of a logo is passed on unchanged.
			if (parsedString == PrintingService.LogoMarker)
			{
				return parsedString;
			}

			if (parsedString.Length > itemInfo.Length)
			{
				// don't trim strings that contain carriage return, they are considered to be previously handled
				if (!parsedString.Contains("\r\n"))
				{
					// The value seems to need to be trimmed
					switch (itemInfo.VertAlign)
					{
						case valign.right:
							parsedString = parsedString.Substring(parsedString.Length - itemInfo.Length, itemInfo.Length);
							break;
						default:
							parsedString = parsedString.Substring(0, itemInfo.Length);
							break;
					}
				}
			}
			else if (parsedString.Length < itemInfo.Length)
			{
				// The value seems to need to be filled
				int charCountUsableForSpace = itemInfo.Length - parsedString.WordLength();
				switch (itemInfo.VertAlign)
				{
					case valign.left:
						parsedString = parsedString.PadRight(itemInfo.Length, itemInfo.Fill);
						break;
					case valign.center:
						int spaceOnLeftSide = (int)charCountUsableForSpace / 2;
						int spaceOnRightSide = charCountUsableForSpace - spaceOnLeftSide;
						parsedString = parsedString.PadLeft(spaceOnLeftSide + parsedString.Length, itemInfo.Fill);
						parsedString = parsedString.PadRight(spaceOnRightSide + parsedString.Length, itemInfo.Fill);
						parsedString += OPOSConstants.HCenterAligned;
						break;
					case valign.right:
						parsedString = parsedString.PadLeft(charCountUsableForSpace + parsedString.Length, itemInfo.Fill);
						break;
				}
			}
			return parsedString;
		}

		protected virtual string ParseTenderVariable(FormItemInfo itemInfo, ITenderLineItem tenderLineItem)
		{
			string variableString = GetInfoFromTenderLineItem(itemInfo, tenderLineItem);

			// Setting the align if necessary
			return GetAlignmentSettings(variableString, itemInfo);
		}

		protected virtual string ParseItemVariable(IConnectionManager entry, FormItemInfo itemInfo, SaleLineItem saleLineItem, out bool skipIfEmptyLine)
		{
			string parsedString;

			if (itemInfo.IsVariable)
			{
				parsedString = GetInfoFromSaleLineItem(entry, itemInfo, saleLineItem, out skipIfEmptyLine);
			}
			else
			{
				skipIfEmptyLine = false;
				parsedString = itemInfo.ValueString;

				if (itemInfo.Prefix.Length > 0)
				{
					parsedString = itemInfo.Prefix + parsedString;
				}
			}
			// Setting the align if necessary
			return GetAlignmentSettings(parsedString, itemInfo);
		}

		protected virtual string ParseCardTenderVariable(FormItemInfo itemInfo, IEFTInfo eftInfo, IRetailTransaction trans, FormPartEnum formPart, ITenderLineItem tenderLineItem)
		{
			string tmpString;

			if (itemInfo.IsVariable)
			{
				tmpString = GetInfoFromTransaction(itemInfo, eftInfo, tenderLineItem, trans, formPart);
			}
			else
			{
				tmpString = itemInfo.ValueString;

				if (itemInfo.Prefix.Length > 0)
				{
					tmpString = itemInfo.Prefix + tmpString;
				}
			}
			// Setting the align if necessary
			return GetAlignmentSettings(tmpString, itemInfo);
		}

		protected virtual string ParseTenderVariable(FormItemInfo itemInfo, ITenderLineItem tenderLineItem, IRetailTransaction trans, FormPartEnum formPart)
		{
			string tmpString;

			if (itemInfo.IsVariable)
			{
				tmpString = GetInfoFromTransaction(itemInfo, null, tenderLineItem, trans, formPart);
			}
			else
			{
				tmpString = itemInfo.ValueString;

				if (itemInfo.Prefix.Length > 0)
				{
					tmpString = itemInfo.Prefix + tmpString;
				}
			}
			// Setting the align if necessary
			return GetAlignmentSettings(tmpString, itemInfo);
		}

		protected virtual string ParseTaxVariable(FormItemInfo itemInfo, TaxItem taxItem)
		{
			string tmpString;

			if (itemInfo.IsVariable)
			{
				tmpString = GetInfoFromTaxItem(itemInfo, taxItem);
			}
			else
			{
				tmpString = itemInfo.ValueString;

				if (itemInfo.Prefix.Length > 0)
				{
					tmpString = itemInfo.Prefix + tmpString;
				}
			}

			// Setting the align if necessary
			return GetAlignmentSettings(tmpString, itemInfo);
		}

		protected virtual string ParseSuspendedAnswerVariable(FormItemInfo itemInfo, SuspendedTransactionAnswer answer)
		{
			string tmpString;

			if (itemInfo.IsVariable)
			{
				tmpString = GetInfoFromSuspendedAnswer(itemInfo, answer);
			}
			else
			{
				tmpString = itemInfo.ValueString;

				if (itemInfo.Prefix.Length > 0)
				{
					tmpString = itemInfo.Prefix + tmpString;
				}
			}

			return tmpString;
		}

		protected virtual string ParseVariable(string textVariableId, FormItemInfo itemInfo, TenderLineItem tenderItem, IPosTransaction trans, FormPartEnum formPart)
		{
			string tmpString;

			if (itemInfo.IsVariable)
			{
                IEFTInfo eftInfo = tenderItem is ICardTenderLineItem ? ((ICardTenderLineItem)tenderItem).EFTInfo : null;

                if(eftInfo == null && trans.ITenderLines != null)
                {
                    //Search for a card payment
                    foreach(TenderLineItem tenderLineItem in trans.ITenderLines)
                    {
                        if(tenderLineItem is ICardTenderLineItem)
                        {
                            eftInfo = ((ICardTenderLineItem)tenderLineItem).EFTInfo;
                        }
                    }
                }

				tmpString = GetInfoFromTransaction(itemInfo, eftInfo, tenderItem, trans, formPart);
			}
			else
			{
				//Certain text variables can be overridden by the FiscalService because it is most probably a legal requirement
				bool variableFound = false;
				string variableValue = string.Empty;

				// check if Fiscal Service overrides the current text variable
				variableValue = GetActiveFiscalService()?.FiscalGetText(textVariableId, prnEntry, prnSettings, trans, out variableFound);
				if (variableFound)
				{
					tmpString = variableValue;
				}
				else
				{ 
					tmpString = itemInfo.ValueString;

					if (itemInfo.Prefix.Length > 0)
					{
						tmpString = itemInfo.Prefix + tmpString;
					}
				}
			}

			if (tmpString == null)
				tmpString = string.Empty;

			// Setting the align if necessary
			return GetAlignmentSettings(tmpString, itemInfo);
		}

		protected virtual string ReadCardTenderDataSet(DataSet ds, IEFTInfo eftInfo, IRetailTransaction trans, FormPartEnum formPart, ICardTenderLineItem tenderLineItem)
		{
			string returnString = "";

			DataTable lineTable = ds.Tables["line"];
			if (lineTable != null)
			{
				foreach (DataRow dr in lineTable.Select("", "nr asc"))
				{
					string lineString = "";
					string idVariable = (string)dr["ID"];

					switch (idVariable)
					{
						case "CRLF":
							lineString += "\r\n";
							break;
						case "Text":
						case "Non_Variable":
							string drLineId = Conversion.ToStr(dr["line_id"]);
							DataTable charPosTable = ds.Tables["charpos"];
							if (charPosTable != null)
							{
								int nextCharNr = 1;
								foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
								{
									try
									{
										FormItemInfo itemInfo = new FormItemInfo(row);
										// Adding possible whitespace at the beginning of line
										lineString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

										// Parsing the itemInfo
										lineString += itemInfo.ApplyFont(ParseCardTenderVariable(itemInfo, eftInfo, trans, formPart, tenderLineItem));

										// Specifying the position of the next char in the current line - bold take twice as much space
										nextCharNr = itemInfo.CharIndex + (itemInfo.Length * itemInfo.SizeFactor);
									}
									catch (Exception)
									{
									}
								}
							}
							lineString += "\r\n";
							break;
					}
					returnString += lineString;
				}
			}
			return returnString;
		}

		protected virtual string ReadTenderDataSet(DataSet ds, ITenderLineItem tenderLineItem, IRetailTransaction trans, FormPartEnum formPart)
		{
			string returnString = "";

			DataTable lineTable = ds.Tables["line"];
			if (lineTable != null)
			{
				foreach (DataRow dr in lineTable.Select("", "nr asc"))
				{
					string lineString = "";
					string idVariable = (string)dr["ID"];

					switch (idVariable)
					{
						case "CRLF":
							lineString += "\r\n";
							break;
						case "Text":
						case "Non_Variable":
							string drLineId = Conversion.ToStr(dr["line_id"]);
							DataTable charPosTable = ds.Tables["charpos"];
							if (charPosTable != null)
							{
								int nextCharNr = 1;
								foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
								{
									try
									{
										FormItemInfo itemInfo = new FormItemInfo(row);
										// Adding possible whitespace at the beginning of line
										lineString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

										// Parsing the itemInfo
										lineString += itemInfo.ApplyFont(ParseTenderVariable(itemInfo, tenderLineItem, trans, formPart));

										// Specifying the position of the next char in the current line - bold take twice as much space
										nextCharNr = itemInfo.CharIndex + (itemInfo.Length * itemInfo.SizeFactor);

									}
									catch (Exception ex)
									{
										errorLog.LogMessageToFile(LogMessageType.Error, ex.ToString(), "FormModulation.ReadTenderDataSet");
									}
								}
							}
							lineString += "\r\n";
							break;
					}
					returnString += lineString;
				}
			}
			return returnString;
		}

		protected virtual bool PrintCustomerOrderVariable(string idVariable, SaleLineItem saleLineItem)
		{
			if (saleLineItem.Order.Empty())
			{
				return true;
			}

			/*****************************************************************************************************************

				All customer order variables are printed always except "For delivery" and "Fully received" which are only
				printed when needed

			******************************************************************************************************************/


			if (idVariable == "CustomerOrderForDelivery" && (saleLineItem.Order.ToPickUp == decimal.Zero || saleLineItem.Order.FullyReceived)) return false;
			if (idVariable == "CustomerOrderForDeliveryText" && (saleLineItem.Order.ToPickUp == decimal.Zero || saleLineItem.Order.FullyReceived)) return false;

			if (idVariable == "CustomerOrderFullyReceivedText" && !saleLineItem.Order.FullyReceived) return false;

			return true;

			/*****************************************************************************************************************

				If any of the customer ordered variables should only be printed if there is something to print then the lines
				here below can be moved to above the "return true" statement here above.                

			******************************************************************************************************************/

			//if (idVariable == "CustomerOrderOrdered" && (saleLineItem.Order.Ordered == decimal.Zero || saleLineItem.Order.ToPickUp > decimal.Zero || saleLineItem.Order.FullyReceived)) return false;
			//if (idVariable == "CustomerOrderOrderedText" && (saleLineItem.Order.Ordered == decimal.Zero || saleLineItem.Order.ToPickUp > decimal.Zero || saleLineItem.Order.FullyReceived)) return false;

			//if (idVariable == "CustomerOrderReceived" && (saleLineItem.Order.Received == decimal.Zero || saleLineItem.Order.ToPickUp > decimal.Zero || saleLineItem.Order.FullyReceived)) return false;
			//if (idVariable == "CustomerOrderReceivedText" && (saleLineItem.Order.Received == decimal.Zero || saleLineItem.Order.ToPickUp > decimal.Zero || saleLineItem.Order.FullyReceived)) return false;

			//if (idVariable == "CustomerOrderForDelivery" && (saleLineItem.Order.ToPickUp == decimal.Zero || saleLineItem.Order.FullyReceived)) return false;
			//if (idVariable == "CustomerOrderForDeliveryText" && (saleLineItem.Order.ToPickUp == decimal.Zero || saleLineItem.Order.FullyReceived)) return false;

			//if (idVariable == "CustomerOrderDepositPaid" && saleLineItem.Order.TotalDepositAmount() == decimal.Zero) return false;
			//if (idVariable == "CustomerOrderDepositPaidText" && (saleLineItem.Order.TotalDepositAmount() == decimal.Zero || saleLineItem.Order.FullyReceived)) return false;

			//if (idVariable == "CustomerOrderFullyReceivedText" && !saleLineItem.Order.FullyReceived) return false;

			//return true;
		}

		protected virtual void ParseSaleItem(IConnectionManager entry, DataSet ds, ref FormItemInfo itemInfo, StringBuilder lIneStringBuilder, SaleLineItem saleLineItem, FormSystemType systemType)
		{
			// Only non-voided items will be printed
			if (!saleLineItem.Voided || systemType == FormSystemType.KitchenSlip)
			{
				DataTable lineTable = ds.Tables["line"];
				if (lineTable != null)
				{
					foreach (DataRow dr in lineTable.Select("", "nr asc"))
					{
						string idVariable = Conversion.ToStr(dr["ID"]);
						switch (idVariable)
						{
							case "CRLF":
								lIneStringBuilder.Append("\r\n");
								break;
							default:
							{
								bool skipScaleInfo = !saleLineItem.ShouldCalculateAndDisplayAssemblyPrice() || ((!saleLineItem.ScaleItem) && (saleLineItem.Quantity == 1));
								
								bool skipDimension = string.IsNullOrEmpty(saleLineItem.Dimension.ColorName) &&
									string.IsNullOrEmpty(saleLineItem.Dimension.SizeName) &&
									string.IsNullOrEmpty(saleLineItem.Dimension.StyleName);

								string drLineId = Conversion.ToStr(dr["line_id"]);
								if ((idVariable == "PharmacyLine") && (saleLineItem.GetType() != typeof (PharmacySalesLineItem))){/*donothing*/}
								else if ((idVariable == "TotalDiscount") && (saleLineItem.TotalDiscountWithTax == 0)){/*donothing*/}
								else if ((idVariable == "LineDiscount") && (saleLineItem.LineDiscountWithTax == 0 && saleLineItem.LoyaltyDiscountWithTax == 0)){/*donothing*/}
								else if ((idVariable == "PeriodicDiscount") && (saleLineItem.PeriodicDiscountWithTax == 0)){/*donothing*/}
								else if ((idVariable == "Batch") && string.IsNullOrEmpty(saleLineItem.BatchId)){/*donothing*/}
								else if ((idVariable == "SalesPerson") && !saleLineItem.SalesPerson.Exists){/*donothing*/}
								else if ((idVariable == "ScaleInfo") && skipScaleInfo){/*donothing*/}
								else if ((idVariable == "ManualWeight") && (saleLineItem.WeightManuallyEntered == false || saleLineItem.IsAssemblyComponent)){/*donothing*/}
								else if ((idVariable == "Variant") && string.IsNullOrEmpty(saleLineItem.VariantName)){/*donothing*/}
								else if ((idVariable == "Dimension") && skipDimension){/*donothing*/}
								else if (!PrintCustomerOrderVariable(idVariable, saleLineItem)){/*donothing*/}
								else if ((idVariable == "Comment") && string.IsNullOrEmpty(saleLineItem.Comment)){/*donothing*/}
								else if (idVariable == "ExtendedDescription")
								{
									RetailItem item = Providers.RetailItemData.Get(prnEntry, saleLineItem.ItemId);
									if (item != null && !string.IsNullOrEmpty(item.ExtendedDescription))
									{
										bool addNewLine = true;
										int itemsOnLine = 0;
										int emptyItemsOnLine = 0;

										DataTable charPosTable = ds.Tables["charpos"];
										if (charPosTable != null)
										{
											int nextCharNr = 1;
											foreach (
												DataRow row in
													charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'",
														"nr asc"))
											{
												itemsOnLine++;
												try
												{
													itemInfo = new FormItemInfo(row);

													string parsedString = item.ExtendedDescription;
													string[] parsedLines = parsedString.Split(new[] {"\r\n"}, StringSplitOptions.None);

													if (parsedLines.Length > 1)
													{
														for (int i = 0; i < parsedLines.Length; i++)
														{
															if (i > 0)
															{
																lIneStringBuilder.Append("\r\n");
															}

															// Adding possible whitespace at the beginning of line
															lIneStringBuilder.Append(CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' '));

															// Parsing the itemInfo
															lIneStringBuilder.Append(itemInfo.ApplyFont(parsedLines[i]));
														}
													}
													else
													{
														// Adding possible whitespace at the beginning of line
														lIneStringBuilder.Append(CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' '));

														// Parsing the itemInfo
														lIneStringBuilder.Append(itemInfo.ApplyFont(parsedString));
													}

													// Closing the string with a single space command to make sure spaces are always single spaced
													lIneStringBuilder.Append(PrintingService.esc + FormItemInfo.RegularFont);


													// Specifying the position of the next char in the current line - bold take twice as much space
													nextCharNr = itemInfo.CharIndex + (itemInfo.Length*itemInfo.SizeFactor);
												}
												catch (Exception)
												{
												}
											}
										}
										if (addNewLine && itemsOnLine != emptyItemsOnLine)
										{
											lIneStringBuilder.Append("\r\n");
										}
									}




								}
								else
								{

									// options for idVariable:
									// Itemlines
									// TotalDiscount
									// LineDiscount
									bool addNewLine = true;
									int itemsOnLine = 0;
									int emptyItemsOnLine = 0;

									DataTable charPosTable = ds.Tables["charpos"];
									if (charPosTable != null)
									{
										int nextCharNr = 1;
										foreach (
											DataRow row in
												charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'",
													"nr asc"))
										{
											itemsOnLine++;
											try
											{
												itemInfo = new FormItemInfo(row);
												bool skipIfEmptyLine;
												string parsedString = ParseItemVariable(entry, itemInfo, saleLineItem, out skipIfEmptyLine);
												bool continueWithLine = true;
												bool isEmpty = parsedString.Trim().Length == 0;

												if ((idVariable == "ExtraInfo") && isEmpty)
												{
													continueWithLine = false;
													addNewLine = false;
												}
												if (skipIfEmptyLine && isEmpty)
													emptyItemsOnLine++;

												if (continueWithLine)
												{
													addNewLine = true;

													string[] parsedLines = parsedString.Split(new[] {"\r\n"}, StringSplitOptions.None);

													if (parsedLines.Length > 1)
													{
														for (int i = 0; i < parsedLines.Length; i++)
														{
															if (i > 0)
															{
																lIneStringBuilder.Append("\r\n");
															}

															// Adding possible whitespace at the beginning of line
															lIneStringBuilder.Append(CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' '));

															// Parsing the itemInfo
															lIneStringBuilder.Append(itemInfo.ApplyFont(parsedLines[i]));
														}
													}
													else
													{
														// Adding possible whitespace at the beginning of line
														lIneStringBuilder.Append(CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' '));

														// Parsing the itemInfo
														lIneStringBuilder.Append(itemInfo.ApplyFont(parsedString));
													}

													// Closing the string with a single space command to make sure spaces are always single spaced
													lIneStringBuilder.Append(PrintingService.esc + FormItemInfo.RegularFont);

												}
												// Specifying the position of the next char in the current line - bold take twice as much space
												nextCharNr = itemInfo.CharIndex + (itemInfo.Length*itemInfo.SizeFactor);
											}
											catch (Exception)
											{
											}
										}
									}
									if (addNewLine && itemsOnLine != emptyItemsOnLine)
									{
										lIneStringBuilder.Append("\r\n");
									}
								}
								break;
							}
						}
					}
				}
			}
		}

		//Creates a string to print out for each of the items in the transaction. If 
		protected virtual string ReadItemDataSet(IConnectionManager entry, DataSet ds, RetailTransaction trans, FormSystemType systemType)
		{
			FormItemInfo itemInfo = null;
			StringBuilder lIneStringBuilder = new StringBuilder("");
			System.Collections.ArrayList itemArray = null;
			bool found = false;

			if (prnSettings.FunctionalityProfile.AggregateItemsForPrinting)
			{
				itemArray = new System.Collections.ArrayList();

				foreach (ISaleLineItem item in trans.SaleItems)
				{
					foreach (SaleLineItem itemInArray in itemArray)
					{
						//the check for the SalesOrderUnitOfMeasure has to be added because otherwise boxes and pieces will be summed together
						if ((itemInArray.ItemId == item.ItemId) && (itemInArray.SalesOrderUnitOfMeasure == item.SalesOrderUnitOfMeasure) && itemInArray.NetAmountWithTax == item.NetAmountWithTax && itemInArray.Voided == item.Voided)                        
						{
							if (itemInArray.LineDiscountWithTax != item.LineDiscountWithTax)
								break;

							if (itemInArray.LinePctDiscount != item.LinePctDiscount)
								break;

							if (itemInArray.TotalDiscountWithTax != item.TotalDiscountWithTax)
								break;

							if (itemInArray.TotalPctDiscount != item.TotalPctDiscount)
								break;

							if (itemInArray.PeriodicPctDiscount != item.PeriodicPctDiscount)
								break;

							itemInArray.Quantity += item.Quantity;
							itemInArray.GrossAmount += item.GrossAmount;
							itemInArray.GrossAmountWithTax += item.GrossAmountWithTax;
							itemInArray.TotalDiscountWithTax += item.TotalDiscountWithTax;
							itemInArray.LineDiscountWithTax += item.LineDiscountWithTax;
							itemInArray.PeriodicDiscountWithTax += item.PeriodicDiscountWithTax;
							itemInArray.LoyaltyDiscountWithTax += item.LoyaltyDiscountWithTax;                            

							found = true;
							break;
						}
					}

					if (found == false)
					{
						SaleLineItem tempItem;

						if (item is FuelSalesLineItem)
						{
							tempItem = (FuelSalesLineItem)item.Clone();
						}
						else if (item is DiscountVoucherItem)
						{
							tempItem = (DiscountVoucherItem)item.Clone();
						}
						else if (item is CreditMemoItem)
						{
							tempItem = (CreditMemoItem)item.Clone();
						}
						else if (item is GiftCertificateItem)
						{
							tempItem = (GiftCertificateItem)item.Clone();
						}
						else
						{
							tempItem = (SaleLineItem)item.Clone();
						}
						itemArray.Add(tempItem);
					}
					found = false;
				}
			}

			if (prnSettings.FunctionalityProfile.AggregateItemsForPrinting)
			{
				//Go through the sale items and parse each line
				foreach (SaleLineItem saleLineItem in itemArray)
				{
                    if(!saleLineItem.IsAssemblyComponent || saleLineItem.ParentAssembly.ShallDisplayWithComponents(ExpandAssemblyLocation.OnReceipt))
                    {
					    ParseSaleItem(entry, ds, ref itemInfo, lIneStringBuilder, saleLineItem, systemType);
                    }
				}
			}
			else
			{
				//Go through the sale items and parse each line
				if (trans.GetType() == typeof(RetailTransaction))
				{
					foreach (SaleLineItem saleLineItem in trans.SaleItems)
					{
                        if (!saleLineItem.IsAssemblyComponent || saleLineItem.ParentAssembly.ShallDisplayWithComponents(ExpandAssemblyLocation.OnReceipt))
                        {
                            ParseSaleItem(entry, ds, ref itemInfo, lIneStringBuilder, saleLineItem, systemType);
                        }
					}
				}                
			}
			return lIneStringBuilder.ToString();
		}

		protected virtual string ReadDataset(DataSet ds, TenderLineItem tenderItem, IPosTransaction trans, FormPartEnum formPart)
		{
			string tempString = "";
			DataTable lineTable = ds.Tables["line"];
			if (lineTable != null)
			{
				foreach (DataRow dr in lineTable.Select("", "nr asc"))
				{
					string idVariable = Conversion.ToStr(dr["ID"]);
					string drLineId;
					switch (idVariable)
					{
						case "CRLF":
							tempString += "\r\n";
							break;
						case "Text":
						case "Non_Variable":
							string textString = "";
                            int addedVariablesCount = 0;

							drLineId = Conversion.ToStr(dr["line_id"]);
							DataTable charPosTable = ds.Tables["charpos"];

							if (charPosTable != null)
							{
								int nextCharNr = 1;
                                DataRow[] formFields = charPosTable.Select("line_id='" + drLineId + "'", "nr asc");
                                addedVariablesCount = formFields.Length;

                                foreach (DataRow row in formFields)
								{
									try
									{
										FormItemInfo itemInfo = new FormItemInfo(row);

										string parsedString = ParseVariable(idVariable, itemInfo, tenderItem, trans, formPart);

										RetailTransaction retailTrans = trans as RetailTransaction;

										if (retailTrans != null)
										{
											try
											{
												if (charPosTable.Columns.Contains("ConditionalIdentifier"))
												{
													string conditionalIdentifier = row["ConditionalIdentifier"].ToString();

													if (conditionalIdentifier == "LoyaltyText")
													{
														if (retailTrans == null || retailTrans.LoyaltyItem.Empty)
														{
                                                            addedVariablesCount--;
                                                            parsedString = "";
														}
													}

													if (conditionalIdentifier == "CustomerFreeText")
													{
														if (retailTrans == null || retailTrans.Customer == null || RecordIdentifier.IsEmptyOrNull(retailTrans.Customer.ID))
                                                        {
                                                            addedVariablesCount--;
                                                            parsedString = "";
                                                        }
													}

                                                    if (conditionalIdentifier == "SaleTransaction" || conditionalIdentifier == "SaleTransactionCopy")
                                                    {
                                                        bool canPrintCopy = (isReprint && conditionalIdentifier == "SaleTransactionCopy") || (!isReprint && conditionalIdentifier == "SaleTransaction");
                                                        if (IsReturnTransaction(trans) || !canPrintCopy)
                                                        {
                                                            addedVariablesCount--;
                                                            parsedString = "";
                                                        }
                                                    }

                                                    if (conditionalIdentifier == "ReturnTransaction" || conditionalIdentifier == "ReturnTransactionCopy")
                                                    {
                                                        bool canPrintCopy = (isReprint && conditionalIdentifier == "ReturnTransactionCopy") || (!isReprint && conditionalIdentifier == "ReturnTransaction");
                                                        if (!IsReturnTransaction(trans) || !canPrintCopy)
                                                        {
                                                            addedVariablesCount--;
                                                            parsedString = "";
                                                        }
                                                    }
                                                }
											}
											catch (Exception)
											{
												//This is new and might cause errors
											}

											if ((itemInfo.Variable == "Markup description" && retailTrans.MarkupItem.Amount == 0)
												|| (itemInfo.Variable == "Markup amount" && retailTrans.MarkupItem.Amount == 0)
												|| (itemInfo.Variable == "Invoice comment" && retailTrans.InvoiceComment == "")
												|| (itemInfo.Variable == "Transaction comment" && retailTrans.Comment == ""))
                                            {
                                                addedVariablesCount--;
                                                parsedString = "";
                                            }
										}

										// Adding possible whitespace at the beginning of line
										textString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

										// Parsing the itemInfo
										textString += itemInfo.ApplyFont(parsedString);

										// Specifying the position of the next char in the current line - bold take twice as much space
										nextCharNr = itemInfo.CharIndex + (itemInfo.Length*itemInfo.SizeFactor);
									}
									catch (Exception)
									{
									}
								}
							}

							if (addedVariablesCount > 0)
								tempString += textString + "\r\n";

							break;
						case "Taxes":
							charPosTable = ds.Tables["charpos"];
							drLineId = Conversion.ToStr(dr["line_id"]);
							if (charPosTable != null && trans is RetailTransaction)
							{
								//Goes through each tax lines and displays them
								foreach (TaxItem taxItem in ((RetailTransaction)trans).TaxLines)
								{
									int nextCharNr = 1;
									foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
									{
										try
										{
											FormItemInfo itemInfo = new FormItemInfo(row);
											// Adding possible whitespace at the beginning of line
											tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

											// Parsing the itemInfo
											tempString += itemInfo.ApplyFont(ParseTaxVariable(itemInfo, taxItem));

											// Specifying the position of the next char in the current line - bold take twice as much space
											nextCharNr = itemInfo.CharIndex + (itemInfo.Length * itemInfo.SizeFactor);
										}
										catch (Exception)
										{
										}
									}
									tempString += "\r\n";
								}
							}
							break;
						
						case "Oiltax":
							
							if (trans is RetailTransaction && ((RetailTransaction)trans).Oiltax > 0)
							{
								charPosTable = ds.Tables["charpos"];
								drLineId = Conversion.ToStr(dr["line_id"]);
								if (charPosTable != null)
								{
									int nextCharNr = 1;
									foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
									{
										try
										{
											FormItemInfo itemInfo = new FormItemInfo(row);
											// Adding possible whitespace at the beginning of line
											tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

											// Parsing the itemInfo
											tempString += itemInfo.ApplyFont(ParseVariable(idVariable, itemInfo, tenderItem, trans, formPart));

											// Specifying the position of the next char in the current line - bold take twice as much space
											nextCharNr = itemInfo.CharIndex + (itemInfo.Length * itemInfo.SizeFactor);
										}
										catch (Exception)
										{
										}
									}
									tempString += "\r\n";
								}
							}
							break;
						
						case "LoyaltyItem":
							if (trans is RetailTransaction && !((RetailTransaction)trans).LoyaltyItem.Empty)
							{
								if (!((RetailTransaction)trans).LoyaltyItem.Empty)
								{
									charPosTable = ds.Tables["charpos"];
									drLineId = Conversion.ToStr(dr["line_id"]);
									if (charPosTable != null)
									{
										int nextCharNr = 1;
										foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
										{
											try
											{
												FormItemInfo itemInfo = new FormItemInfo(row);
												// Adding possible whitespace at the beginning of line
												tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

												// Parsing the itemInfo
												tempString += itemInfo.ApplyFont(ParseVariable(idVariable, itemInfo, tenderItem, trans, formPart));

												// Specifying the position of the next char in the current line - bold take twice as much space
												nextCharNr = itemInfo.CharIndex + (itemInfo.Length * itemInfo.SizeFactor);
											}
											catch (Exception)
											{
											}
										}
										tempString += "\r\n";
									}
								}
							}
							break;
						case "SumTotalDiscounts":
						case "SumLineDiscounts":
						case "SumTotalTransactionDiscounts":
							if (trans is RetailTransaction)
							{
								if ((idVariable == "SumTotalDiscounts") && (((RetailTransaction)trans).TotalDiscount == decimal.Zero)) {/*donothing*/}
								else if ((idVariable == "SumLineDiscounts") && ((((RetailTransaction)trans).LineDiscount + ((RetailTransaction)trans).PeriodicDiscountAmount) == decimal.Zero)){/*donothing*/}
								else if ((idVariable == "SumTotalTransactionDiscounts") && (((RetailTransaction)trans).TotalDiscount + (((RetailTransaction)trans).LineDiscount + ((RetailTransaction)trans).PeriodicDiscountAmount) == decimal.Zero)) { tempString += "L" + decimal.Zero; }
								else
								{
									charPosTable = ds.Tables["charpos"];
									drLineId = Conversion.ToStr(dr["line_id"]);
									if (charPosTable != null)
									{
										int nextCharNr = 1;
										foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
										{
											try
											{
												FormItemInfo itemInfo = new FormItemInfo(row);
												// Adding possible whitespace at the beginning of line
												tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

												// Parsing the itemInfo
												tempString += itemInfo.ApplyFont(ParseVariable(idVariable, itemInfo, tenderItem, trans, formPart));

												// Specifying the position of the next char in the current line - bold take twice as much space
												nextCharNr = itemInfo.CharIndex + (itemInfo.Length * itemInfo.SizeFactor);
											}
											catch (Exception)
											{
											}
										}
										tempString += "\r\n";
									}
								}

							}
							break;
						case "Customer":
							if (trans is RetailTransaction && ((RetailTransaction) trans).Customer != null && !RecordIdentifier.IsEmptyOrNull(((RetailTransaction)trans).Customer.ID))
							{
								textString = "";

								drLineId = Conversion.ToStr(dr["line_id"]);
								charPosTable = ds.Tables["charpos"];
								if (charPosTable != null)
								{
									int nextCharNr = 1;

									foreach (DataRow row in charPosTable.Select("line_id='" + drLineId + "'", "nr asc"))
									{
										try
										{
											FormItemInfo itemInfo = new FormItemInfo(row);

											string parsedString = ParseVariable(idVariable, itemInfo, tenderItem, trans, formPart);
											
											// Adding possible whitespace at the beginning of line
											textString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

											// Parsing the itemInfo
											textString += itemInfo.ApplyFont(parsedString);

											// Specifying the position of the next char in the current line - bold take twice as much space
											nextCharNr = itemInfo.CharIndex + (itemInfo.Length * itemInfo.SizeFactor);
										}
										catch (Exception)
										{
										}
									}
								}
                                
								tempString += textString + "\r\n";
							}
							break;
						// Go through all the transaction lines
						case "Tenders":
							charPosTable = ds.Tables["charpos"];
							drLineId = Conversion.ToStr(dr["line_id"]);
							if (charPosTable != null)
							{
								// Passes through payment transactions and parses each line separately
								List<ITenderLineItem> tenderLines = new List<ITenderLineItem>();

								if (trans is RetailTransaction)
								{
									tenderLines = ((RetailTransaction)trans).TenderLines;
								}
								else if (trans is TenderDeclarationTransaction)
								{
									tenderLines = new List<ITenderLineItem>(((TenderDeclarationTransaction)trans).TenderLines);
								}
								else if (trans is SafeDropTransaction)
								{
									tenderLines = new List<ITenderLineItem>(((SafeDropTransaction)trans).TenderLines);
								}
								else if (trans is SafeDropReversalTransaction)
								{
									tenderLines = new List<ITenderLineItem>(((SafeDropReversalTransaction)trans).TenderLines);
								}
								else if (trans is BankDropTransaction)
								{
									tenderLines = new List<ITenderLineItem>(((BankDropTransaction)trans).TenderLines);
								}
								else if (trans is BankDropReversalTransaction)
								{
									tenderLines = new List<ITenderLineItem>(((BankDropReversalTransaction)trans).TenderLines);
								}

								foreach (ITenderLineItem tenderLineItem in tenderLines)
								{
									if (tenderLineItem.Voided == false)
									{
										int nextCharNr = 1;
										foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
										{
											try
											{
												FormItemInfo itemInfo = new FormItemInfo(row);
												// If tender is a Change Back tender, then a carriage return is put in front of the next line
												if ((tenderLineItem.Amount < 0) && (nextCharNr == 1))
												{
													tempString += "\r\n";
												}
												// Adding possible whitespace at the beginning of line
												tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

												// Parsing the itemInfo
												tempString += itemInfo.ApplyFont(ParseTenderVariable(itemInfo, (TenderLineItem)tenderLineItem));

												// Specifying the position of the next char in the current line - bold take twice as much space
												nextCharNr = itemInfo.CharIndex + (itemInfo.Length * itemInfo.SizeFactor);
											}
											catch (Exception)
											{
											}
										}
										tempString += "\r\n";
									}
								}
							}
							break;
						case "EFTInfo":
							charPosTable = ds.Tables["charpos"];
							drLineId = Conversion.ToStr(dr["line_id"]);
							if (charPosTable != null)
							{
								// Passes through payment transactions and parses each line separately
								if (trans is RetailTransaction)
								{
									List<ITenderLineItem> tenderLines = ((RetailTransaction) trans).TenderLines;
									foreach (ITenderLineItem tenderLineItem in tenderLines)
									{
										if (!(tenderLineItem is CardTenderLineItem))
											continue;
										if (tenderLineItem.Voided == false)
										{
											int nextCharNr = 1;
											foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
											{
												try
												{
													FormItemInfo itemInfo = new FormItemInfo(row);
													// If tender is a Change Back tender, then a carrage return is put in front of the next line
													if ((tenderLineItem.Amount < 0) && (nextCharNr == 1))
													{
														tempString += "\r\n";
													}
													// Adding possible whitespace at the beginning of line
													tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

													// Parsing the itemInfo
													tempString += itemInfo.ApplyFont(ParseCardTenderVariable(itemInfo,
														((CardTenderLineItem) tenderLineItem).EFTInfo, trans as RetailTransaction, formPart, (TenderLineItem)tenderLineItem));

													// Specifying the position of the next char in the current line - bold take twice as much space
													nextCharNr = itemInfo.CharIndex + (itemInfo.Length*itemInfo.SizeFactor);
												}
												catch (Exception)
												{
												}
											}
											tempString += "\r\n";
										}
									}
								}
							}
							break;
						// Goes through one specific tender submitted with a copy of the tender
						case "Tender":
							charPosTable = ds.Tables["charpos"];
							drLineId = Conversion.ToStr(dr["line_id"]);
							if (charPosTable != null)
							{
								int nextCharNr = 1;
								foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
								{
									try
									{
										FormItemInfo itemInfo = new FormItemInfo(row);
										// If tender is a Change Back tender, then a carriage return is put in front of the next line
										if ((tenderItem.Amount < 0) && (nextCharNr == 1))
										{
											tempString += "\r\n";
										}
										// Adding possible whitespace at the beginning of line
										tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

										// Parsing the itemInfo
										tempString += itemInfo.ApplyFont(ParseTenderVariable(itemInfo, tenderItem));

										// Specifying the position of the next char in the current line - bold take twice as much space
										nextCharNr = itemInfo.CharIndex + (itemInfo.Length * itemInfo.SizeFactor);
									}
									catch (Exception ex)
									{
										errorLog.LogMessageToFile(LogMessageType.Error, ex.ToString(), "FormModulation.ReadDataset");
									}
								}
								tempString += "\r\n";
							}
							break;
						// Goes through one specific tender submitted with a copy of the tender
						case "TenderRounding":
							if (trans is RetailTransaction && (((RetailTransaction)trans).RoundingDifference == 0) && (((RetailTransaction)trans).RoundingSalePmtDiff == 0))
							{
								break;
							}

							charPosTable = ds.Tables["charpos"];
							drLineId = Conversion.ToStr(dr["line_id"]);
							if (charPosTable != null)
							{
								int nextCharNr = 1;

								foreach (DataRow row in charPosTable.Select("line_id='" + drLineId + "'", "nr asc"))
								{
									try
									{
										FormItemInfo itemInfo = new FormItemInfo(row);
										// Adding possible whitespace at the beginning of line
										tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

										// Parsing the itemInfo
										tempString += itemInfo.ApplyFont(ParseVariable(idVariable, itemInfo, tenderItem, trans, formPart));

										// Specifying the position of the next char in the current line - bold take twice as much space
										nextCharNr = itemInfo.CharIndex + (itemInfo.Length * itemInfo.SizeFactor);
									}
									catch (Exception ex)
									{
										errorLog.LogMessageToFile(LogMessageType.Error, ex.ToString(), "FormModulation.ReadDataset");
									}
								}
							}
							tempString += "\r\n";
							break;
						case "TenderRemoval":
							if (!(trans is RemoveTenderTransaction))
							{
								break;
							}

							charPosTable = ds.Tables["charpos"];
							drLineId = Conversion.ToStr(dr["line_id"]);
							if (charPosTable != null)
							{
								int nextCharNr = 1;

								foreach (DataRow row in charPosTable.Select("line_id='" + drLineId + "'", "nr asc"))
								{
									try
									{
										FormItemInfo itemInfo = new FormItemInfo(row);
										// Adding possible whitespace at the beginning of line
										tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

										// Parsing the itemInfo
										tempString += itemInfo.ApplyFont(ParseVariable(idVariable, itemInfo, tenderItem, trans, formPart));

										// Specifying the position of the next char in the current line - bold take twice as much space
										nextCharNr = itemInfo.CharIndex + (itemInfo.Length * itemInfo.SizeFactor);
									}
									catch (Exception ex)
									{
										errorLog.LogMessageToFile(LogMessageType.Error, ex.ToString(), "FormModulation.ReadDataset - TenderRemoval");
									}
								}
							}
							tempString += "\r\n";
							break;
						case "SuspendedAnswer":
							{
								RetailTransaction retailTrans = trans as RetailTransaction;
								if (retailTrans == null)
									break;
								if (retailTrans.SuspendTransactionAnswers == null ||
									retailTrans.SuspendTransactionAnswers.Count == 0)
									break;

								charPosTable = ds.Tables["charpos"];
								drLineId = Conversion.ToStr(dr["line_id"]);

								if (charPosTable != null)
								{
									foreach (SuspendedTransactionAnswer answer in retailTrans.SuspendTransactionAnswers)
									{
										int nextCharNr = 1;
										foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
										{
											try
											{
												FormItemInfo itemInfo = new FormItemInfo(row);

												// Adding possible whitespace at the beginning of line
												tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

												// Parsing the itemInfo
												tempString += itemInfo.ApplyFont(ParseSuspendedAnswerVariable(itemInfo, answer));

												// Specifying the position of the next char in the current line - bold take twice as much space
												nextCharNr = itemInfo.CharIndex + (itemInfo.Length * itemInfo.SizeFactor);
											}
											catch (Exception)
											{
											}
											tempString += "\r\n";
										}
									}
								}
							}
							break;
					}
				}
			}
			return tempString;
		}

		private bool IsReturnTransaction(IPosTransaction trans)
		{
			RetailTransaction retailTrans = trans as RetailTransaction;

			bool isReturn = false;
			foreach (ISaleLineItem saleItem in retailTrans.SaleItems)
			{
				if (saleItem.Quantity < 0 && !saleItem.Voided)
				{
					isReturn = true;
				}
			}

			return isReturn;
		}

		/// <summary>
		/// Returns the reference to the active FiscalService (if any).
		/// </summary>
		/// <returns>A valid reference of <see cref="IFiscalService"/> if there is an implementation of it with <see cref="IFiscalService.IsActive"/> true or null in all the other cases.</returns>
		private IFiscalService GetActiveFiscalService()
		{
			IFiscalService fs = (IFiscalService)prnEntry.Service(ServiceType.FiscalService);
			if (fs == null)
			{
				prnEntry.ErrorLogger.LogMessage(LogMessageType.Error, "Fiscal Service getting instance error, multiple FiscalService dlls found in Services folder may cause this.");
			}

			return fs == null || !fs.IsActive() ? null : fs;
		}

		public virtual string GetTransformedCardTender(FormSystemType systemType, IEFTInfo eftInfo, IRetailTransaction trans, ICardTenderLineItem tenderLineItem)
		{
			StringBuilder returnString = new StringBuilder();
			BarcodePrintInfoList.Clear();

			// Getting a dataset containing the header part of the current form
			DataSet ds = LoadXmlForm(systemType, FormPart.Header);
			returnString.Append(ReadCardTenderDataSet(ds, eftInfo, trans, FormPartEnum.Header, tenderLineItem));

			// Getting a dataset containing the footer part of the current form
			ds = LoadXmlForm(systemType, FormPart.Footer);
			returnString.Append(ReadCardTenderDataSet(ds, eftInfo, trans, FormPartEnum.Footer, tenderLineItem));

			// further modification of the string
			DataTable formDetails = ds.Tables["FormDetails"];
			if (formDetails != null)
			{
				DataRow detailRow = formDetails.Rows[0];
				{
					if (Convert.ToBoolean(detailRow["UpperCase"]))
					{
						return returnString.ToString().ToUpperInvariant();
					}
				}
			}
			return returnString.ToString();
		}

		protected virtual RecordIdentifier GetFormTypeID(FormSystemType systemType)
		{
			if (formTypeList == null || formTypeList.Count == 0)
			{
				formTypeList = Providers.FormTypeData.GetFormTypes(prnEntry, FormTypeSorting.Type, false);
			}

			FormType formType = formTypeList.FirstOrDefault(f => f.SystemType == (int)systemType);

			if (formType != null)
			{
				return formType.ID;
			}

			return RecordIdentifier.Empty;

		}

		public virtual string GetTransformedTender(FormSystemType systemType, ITenderLineItem tenderLineItem, IRetailTransaction trans, bool useEmailReceipt = false)
		{
			StringBuilder returnString = new StringBuilder();
			BarcodePrintInfoList.Clear();

			// Getting a dataset containing the header part of the current form
			DataSet ds = LoadXmlForm(systemType, FormPart.Header, useEmailReceipt);
			returnString.Append(ReadDataset(ds, (TenderLineItem)tenderLineItem, trans, FormPartEnum.Header));

			// Getting a dataset containing the footer part of the current form
			ds = LoadXmlForm(systemType, FormPart.Footer, useEmailReceipt);
			returnString.Append(ReadDataset(ds, (TenderLineItem)tenderLineItem, trans, FormPartEnum.Footer));

			// further modification of the string
			DataTable formDetails = ds.Tables["FormDetails"];
			if (formDetails != null)
			{
				DataRow detailRow = formDetails.Rows[0];
				{
					if (Convert.ToBoolean(detailRow["UpperCase"]))
					{
						return returnString.ToString().ToUpperInvariant();
					}
				}
			}
			
			return returnString.ToString();
		}

		public virtual string GetTransformedTransaction(IConnectionManager entry, FormSystemType systemType, IPosTransaction trans, FormInfo formInfo, Form form = null, bool useEmailProfile = false)
		{
			try
			{
				BarcodePrintInfoList.Clear();

				SetHeaderTextForTrainingMode(entry, formInfo, prnSettings, trans, systemType);

				isReprint = formInfo.Reprint;

				// If DEMO version then add a line to the top: "LS One POS DEMO Version"
				if (((ILicenseService)prnEntry.Service(ServiceType.LicenseService)).LicenseType == Interfaces.LicenseType.Demo)
				{
					formInfo.Header += "\r\n" + PrintingService.esc + FormItemInfo.DoubleWideFont + Properties.Resources.AppDemoVersion + PrintingService.esc + FormItemInfo.RegularFont;
					if (formInfo.Reprint == false)
						formInfo.Header += "\r\n" + "\r\n";
				}

				copyText = formInfo.Reprint ? Properties.Resources.Copy : "";

				// Getting a dataset containing the header part of the current form
				DataSet ds = (form == null) ? LoadXmlForm(systemType, FormPart.Header, useEmailProfile) : LoadXmlForm(FormPart.Header, form);
				formInfo.Header += ReadDataset(ds, null, trans, FormPartEnum.Header);             
				formInfo.HeaderLines = ds.Tables[0].Rows.Count;

				// Getting a dataset containing the line part of the current form
				RetailTransaction retailTrans = trans as RetailTransaction;
				if (retailTrans != null)
				{
					ds = LoadXmlForm(systemType, FormPart.Line, useEmailProfile);
					formInfo.Details = ReadItemDataSet(entry,  ds, retailTrans, systemType);
					formInfo.DetailLines = ds.Tables[0].Rows.Count;
				}
				
				// Getting a dataset containing the footer part of the current form
				ds = (form == null) ? LoadXmlForm(systemType, FormPart.Footer, useEmailProfile) : LoadXmlForm(FormPart.Footer, form); 
				formInfo.Footer = ReadDataset(ds, null, trans, FormPartEnum.Footer);
				formInfo.FooterLines = ds.Tables[0].Rows.Count;

				SetFooterTextForTrainingMode(entry, formInfo, prnSettings, trans, systemType);

				// further modification of the string
				DataTable formDetails = ds.Tables["FormDetails"];
				if (formDetails != null)
				{
					DataRow detailRow = formDetails.Rows[0];
					{
						if (Convert.ToBoolean(detailRow["UpperCase"]))
						{
							formInfo.Header = formInfo.Header.ToUpperInvariant();
							formInfo.Details = formInfo.Details.ToUpperInvariant();
							formInfo.Footer = formInfo.Footer.ToUpperInvariant();
						}
					}
				}
			}
			catch (Exception ex)
			{
				prnEntry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
				throw;
			}

			return formInfo.Header + formInfo.Details + formInfo.Footer;
		}

		/// <summary>
		/// Adds the default/base text to receipt header when the POS is in training mode.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="formInfo"></param>
		/// <param name="prnSettings"></param>
		/// <param name="trans"></param>
		/// <param name="systemType"></param>
		private void SetHeaderTextForTrainingMode(IConnectionManager entry, FormInfo formInfo, ISettings prnSettings, IPosTransaction trans, FormSystemType systemType)
		{
			if (!prnSettings.TrainingMode) return;

			//Fiscal service settings have priority since they're most probably legal requirements
			string trainingModeText = GetActiveFiscalService()?.GetHeaderTextForTrainingMode(entry, formInfo, prnSettings, trans, systemType);

			if (string.IsNullOrEmpty(trainingModeText))
			{
				trainingModeText = GetDefaultTrainingTextForPrint(formInfo);
			}

			formInfo.Header = trainingModeText;
		}

		/// <summary>
		/// Adds the default/base text to receipt footer when the POS is in training mode.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="formInfo"></param>
		/// <param name="prnSettings"></param>
		/// <param name="trans"></param>
		/// <param name="systemType"></param>
		private void SetFooterTextForTrainingMode(IConnectionManager entry, FormInfo formInfo, ISettings prnSettings, IPosTransaction trans, FormSystemType systemType)
		{
			if (!prnSettings.TrainingMode) return;

			//Fiscal service settings have priority since they're most probably legal requirements
			string trainingModeText = GetActiveFiscalService()?.GetFooterTextForTrainingMode(entry, formInfo, prnSettings, trans, systemType);

			if (string.IsNullOrEmpty(trainingModeText))
			{
				trainingModeText = GetDefaultTrainingTextForPrint(formInfo);
			}

			formInfo.Footer = trainingModeText;
		}

		private string GetDefaultTrainingTextForPrint(FormInfo formInfo)
		{
			// If POS is in training mode then add two lines to the top: 
			// T R A I N I N I N G  M O D E
			// This is not a valid receipt
			char esc = Convert.ToChar(Keys.Escape);

			string trainingText = "\r\n**************************************************\r\n";
			trainingText += "\r\n" + esc + OPOSConstants.DoubleWideFont + Properties.Resources.Training + esc + OPOSConstants.RegularFont;
			trainingText += "\r\n" + esc + OPOSConstants.DoubleWideFont + Properties.Resources.ThisIsNotAValidReceipt + esc + OPOSConstants.RegularFont;
			if (!formInfo.Reprint)
				trainingText += "\r\n" + "\r\n";
			trainingText += "\r\n**************************************************\r\n";

			return trainingText;
		}

		/// <summary>
		/// Returns the standard/base values for the defined receipt variables.
		/// </summary>
		/// <returns></returns>
		private string GetDefaultInfoFromTransaction(string variable, IRoundingService rounding, IPosTransaction trans, RetailTransaction retailTrans, ITenderLineItem tenderItem, IEFTInfo eftInfo, out bool variableFound)
		{
			// the method requires at least a valid variable name, a valid rounding service and a valid POS transaction
			if(string.IsNullOrEmpty(variable)) throw new ArgumentNullException(nameof(variable));
			if (rounding is null) throw new ArgumentNullException(nameof(rounding));
			if (trans is null) throw new ArgumentNullException(nameof(trans));

			string returnValue = string.Empty;
			variableFound = true;

			switch (variable)
			{
				case "DATE":
					returnValue = trans.BeginDateTime.ToShortDateString();
					break;
				case "TIME24H":
					returnValue = trans.BeginDateTime.ToString("HH:mm");
					break;
				case "TIME12H":
					returnValue = trans.BeginDateTime.ToString("hh:mm tt");
					break;
				case "TRANSNO":
				case "TRANSACTIONNUMBER":
					returnValue = trans.TransactionId;
					break;
				case "TRANSACTIONSEQUENCENUMBER":
					if (retailTrans != null)
						returnValue = retailTrans.SalesSequenceID;
					break;
				case "RECEIPTNUMBER":
					returnValue = trans.ReceiptId;
					break;
				case "STAFF_ID":
				case "OPERATORID":
				case "EMPLOYEEID":
					returnValue = (string)trans.Cashier.Login;
					break;
				case "EMPLOYEENAME":
				case "OPERATORNAME":
					returnValue = trans.Cashier.Name;
					break;
				case "OPERATORNAMEONRECEIPT":
					returnValue = trans.Cashier.NameOnReceipt;
					break;
				case "SALESPERSONID":
					if (retailTrans != null)
						returnValue = (string)retailTrans.SalesPerson.Login;
					break;
				case "SALESPERSONNAME":
					if (retailTrans != null)
						returnValue = retailTrans.SalesPerson.Name;
					break;
				case "SALESPERSONNAMEONRECEIPT":
					if (retailTrans != null)
						returnValue = retailTrans.SalesPerson.NameOnReceipt;
					break;
				case "TOTAL":
					if (retailTrans != null)
					{
						returnValue = rounding.RoundString(prnEntry,
															retailTrans.NetAmountWithTax,
															prnSettings.Store.Currency,
															false,
															CacheType.CacheTypeTransactionLifeTime);
					}
					else if (trans is FloatEntryTransaction)
					{
						returnValue = rounding.RoundString(prnEntry,
															((FloatEntryTransaction)trans).Amount,
															prnSettings.Store.Currency,
															false,
															CacheType.CacheTypeTransactionLifeTime);
					}
					else if (trans is RemoveTenderTransaction)
					{
						returnValue = rounding.RoundString(prnEntry,
															((RemoveTenderTransaction)trans).Amount,
															prnSettings.Store.Currency, true,
															CacheType.CacheTypeTransactionLifeTime);
					}
					break;
				case "TOTALWITHOUTTAX":
					if (retailTrans != null)
						returnValue = rounding.RoundString(prnEntry,
							retailTrans.NetAmount,
							prnSettings.Store.Currency,
							false,
							CacheType.CacheTypeTransactionLifeTime);
					break;
				case "TOTALPAYMENT":
					if (retailTrans != null)
					{
						returnValue = rounding.RoundString(prnEntry,
							retailTrans.Payment,
							prnSettings.Store.Currency,
							false,
							CacheType.CacheTypeTransactionLifeTime);
					}
					break;
				case "TERMINALID":
					returnValue = (string)prnEntry.CurrentTerminalID;
					break;
				case "CASHIER":
					returnValue =
						prnEntry.Settings.NameFormatter.Format(prnSettings.POSUser.Name);
					break;
				case "CUSTOMERNAME":
					if (retailTrans != null)
						returnValue = retailTrans.Customer.FirstName != ""
							? retailTrans.Customer.GetFormattedName(prnEntry.Settings.NameFormatter)
							: retailTrans.Customer.Text;
					break;
				case "CUSTOMERACCOUNTNUMBER":
					if (retailTrans != null)
						returnValue = (string)retailTrans.Customer.ID;
					break;
				case "CUSTOMERBILLINGADDRESS":
					returnValue = GetAddressString(retailTrans, Address.AddressTypes.Billing);
					break;
				case "CUSTOMERSHIPPINGADDRESS":
					returnValue = GetAddressString(retailTrans, Address.AddressTypes.Shipping);
					break;
				case "CUSTOMERAMOUNT":
					if (retailTrans != null)
						returnValue = rounding.RoundString(prnEntry,
															retailTrans.AmountToAccount,
															prnSettings.Store.Currency,
															false,
															CacheType.CacheTypeTransactionLifeTime);
					break;
				case "CUSTOMERVAT":
					if (retailTrans != null)
						returnValue = retailTrans.Customer.VatNum;
					break;
				case "CUSTOMERTAXOFFICE":
					if (retailTrans != null)
						returnValue = retailTrans.Customer.TaxOffice;
					break;
				case "CUSTOMERBALANCE":
					if (retailTrans != null)
					{
						decimal customerBalance = Providers.CustomerLedgerEntriesData.GetCustomerBalance(prnEntry, retailTrans.Customer.ID);
						returnValue = rounding.RoundString(prnEntry, 
															customerBalance, 
															prnSettings.Store.Currency, 
															false, 
															CacheType.CacheTypeTransactionLifeTime);
					}
					break;
				case "CUSTOMERCREDITLIMIT":
					if (retailTrans != null)
					{
						returnValue = rounding.RoundString(prnEntry, 
															retailTrans.Customer.MaxCredit, 
															prnSettings.Store.Currency, 
															false, 
															CacheType.CacheTypeTransactionLifeTime);
					}
					break;
				case "CUSTOMEREMAIL":
					if (retailTrans != null)
					{
						returnValue = retailTrans.Customer.ReceiptEmailAddress;
					}
					break;
				case "CARDEXPIREDATE":
					returnValue = eftInfo.ExpDate.Substring(0, 2) + "/" + eftInfo.ExpDate.Substring(2, 2);
					break;
				case "CARDNUMBER":
					returnValue = eftInfo.CardNumber;
					break;
				case "CARDNUMBERPARTLYHIDDEN":
					string tempValue = eftInfo.CardNumber.Substring(eftInfo.CardNumber.Length - 6, 6);
					returnValue = tempValue.Insert(2, "-");
					break;
				case "CARDTYPE":
					returnValue = eftInfo != null ? eftInfo.CardName : "";
					break;
				case "CARDAMOUNT":
					decimal amountInCents = Math.Abs(eftInfo.AmountInCents);
					returnValue = rounding.RoundString(prnEntry, 
														Convert.ToDecimal(amountInCents / 100),
														prnSettings.Store.Currency, 
														false, 
														CacheType.CacheTypeTransactionLifeTime);
					break;
				case "CARDAUTHNUMBER":
					returnValue = eftInfo != null ? eftInfo.AuthCode : "";
					break;
				case "BATCHCODE":
					returnValue = eftInfo != null ? eftInfo.BatchCode : "";
					break;
				case "ACQUIRERNAME":
					returnValue = eftInfo != null ? eftInfo.AcquirerName : "";
					break;
				case "VISAAUTHCODE":
					returnValue = eftInfo != null ? eftInfo.VisaAuthCode : "";
					break;
				case "EUROAUTHCODE":
					returnValue = eftInfo != null ? eftInfo.EuropayAuthCode : "";
					break;
				case "EFTSTORECODE":
					returnValue =
						((string)eftInfo.StoreID).PadRight(4, '0')
							.Substring(((string)eftInfo.StoreID).Length - 4, 4);
					break;
				case "EFTTERMINALNUMBER":
					returnValue = ((string)eftInfo.TerminalNumber).PadLeft(4, '0');
					break;
				case "EFTINFOMESSAGE":
					returnValue = eftInfo != null
						? (eftInfo.Authorized ? copyText + eftInfo.AuthorizedText : eftInfo.NotAuthorizedText)
						: "";
					break;
				case "EFTTERMINALID":
					returnValue = ((string)eftInfo.TerminalIDCardVendor).PadLeft(8, '0');
					break;
				case "ENTRYSOURCECODE":
					returnValue = eftInfo != null ? eftInfo.EntrySourceCode : "";
					break;
				case "AUTHSOURCECODE":
					returnValue = eftInfo != null ? eftInfo.AuthSourceCode : "";
					break;
				case "AUTHORIZATIONCODE":
					returnValue = eftInfo != null ? eftInfo.AuthCode : "";
					break;
				case "SEQUENCECODE":
					returnValue = eftInfo != null ? eftInfo.SequenceCode : "";
					break;
				case "EFTMESSAGE":
					returnValue = eftInfo != null ? eftInfo.Message : "";
					break;
				case "MARKUPAMOUNT":
					if (retailTrans != null)
						returnValue = rounding.RoundString(prnEntry,
															retailTrans.MarkupItem.Amount,
															prnSettings.Store.Currency,
															false,
															CacheType.CacheTypeTransactionLifeTime);
					break;
				case "MARKUPDESCRIPTION":
					if (retailTrans != null)
						returnValue = retailTrans.MarkupItem.Description;
					break;
				case "CUSTOMERTENDERAMOUNT":
					returnValue = rounding.RoundString(prnEntry, 
														tenderItem.Amount,
														prnSettings.Store.Currency, 
														false, 
														CacheType.CacheTypeTransactionLifeTime);
					break;
				case "TENDERROUNDING":
					string tenderRounding = "";
					if (retailTrans != null)
					{
						decimal value;
						if (retailTrans.RoundingSalePmtDiff == 0M)
							value = retailTrans.RoundingDifference;
						else
							value = retailTrans.RoundingSalePmtDiff * -1;
						tenderRounding = rounding.RoundString(prnEntry, 
																value,
																prnSettings.Store.Currency, 
																false, 
																CacheType.CacheTypeTransactionLifeTime);
					}
					returnValue = tenderRounding;
					break;
				case "INVOICECOMMENT":
					if (retailTrans != null)
						returnValue = retailTrans.InvoiceComment;
					break;
				case "TRANSACTIONCOMMENT":
					if (retailTrans != null)
						returnValue = retailTrans.Comment;
					break;
				case "LOGO":
					returnValue = PrintingService.LogoMarker;
					break;
				case "RECEIPTNUMBERBARCODE":
					returnValue = BarcodePrintMarkers.ReceiptIDMarker + trans.ReceiptId + BarcodePrintMarkers.BarcodeEndMarker + Environment.NewLine;
					BarcodePrintInfoList.Add(new BarcodePrintInfo(BarcodePrintTypeEnum.Receipt, trans.ReceiptId));
					break;
				case "REPRINTMESSAGE":
					returnValue = copyText;
					break;
				case "STOREID":
					returnValue = trans.StoreId;
					break;
				case "STORENAME":
					returnValue = trans.StoreName;
					break;
				case "STOREADDRESS":
					returnValue = trans.StoreAddress;
					break;
				case "STOREINFO1":
					returnValue = prnSettings.Store.FormInfoField1;
					break;
				case "STOREINFO2":
					returnValue = prnSettings.Store.FormInfoField2;
					break;
				case "STOREINFO3":
					returnValue = prnSettings.Store.FormInfoField3;
					break;
				case "STOREINFO4":
					returnValue = prnSettings.Store.FormInfoField4;
					break;
				case "TERMINALINFO1":
					returnValue = prnSettings.Terminal.FormInfoField1;
					break;
				case "TERMINALINFO2":
					returnValue = prnSettings.Terminal.FormInfoField2;
					break;
				case "TERMINALINFO3":
					returnValue = prnSettings.Terminal.FormInfoField3;
					break;
				case "TERMINALINFO4":
					returnValue = prnSettings.Terminal.FormInfoField4;
					break;
				case "TENDERAMOUNT":
					returnValue = rounding.RoundString(prnEntry, 
														Math.Abs(tenderItem.Amount),
														prnSettings.Store.Currency, 
														false, 
														CacheType.CacheTypeTransactionLifeTime);
					break;
				case "USEDLOYALTYPOINTS":
					decimal usedLoyaltyPoints = decimal.Zero;

					if (retailTrans != null && retailTrans.LoyaltyItem.Relation != LoyaltyPointsRelation.Discount)
					{
						foreach (LoyaltyTenderLineItem loyaltyTender in retailTrans.TenderLines.Where(w => w is LoyaltyTenderLineItem))
						{
							usedLoyaltyPoints += loyaltyTender.Points;
						}
					}
					else if (retailTrans != null && retailTrans.LoyaltyItem.Relation == LoyaltyPointsRelation.Discount)
					{
						usedLoyaltyPoints = retailTrans.LoyaltyItem.CalculatedPoints;
					}

					if (usedLoyaltyPoints != decimal.Zero)
					{
						returnValue = rounding.RoundForReceipt(prnEntry, usedLoyaltyPoints, 0);
					}
					break;
				case "ISSUEDLOYALTYPOINTS":
					if (retailTrans != null && (retailTrans.LoyaltyItem.Relation != LoyaltyPointsRelation.Discount && retailTrans.LoyaltyItem.Relation != LoyaltyPointsRelation.Tender))
					{
						string issuedLoyaltyPoints = "";
						if (!retailTrans.LoyaltyItem.Empty)
						{
							issuedLoyaltyPoints = rounding.RoundForReceipt(prnEntry, retailTrans.LoyaltyItem.CalculatedPoints, 0);
						}
						if (issuedLoyaltyPoints != "0")
						{
							returnValue = issuedLoyaltyPoints;
						}
					}
					break;
				case "ACCUMULATEDLOYALTYPOINTS":
					if (retailTrans != null)
					{
						string accumulatedLoyaltyPoints = "";

						if (!retailTrans.LoyaltyItem.Empty)
						{
							accumulatedLoyaltyPoints = rounding.RoundForReceipt(prnEntry, retailTrans.LoyaltyItem.AccumulatedPoints, 0);
						}
						returnValue = accumulatedLoyaltyPoints;
					}
					break;
				case "LOYALTYCARDNUMBER":
					if (retailTrans != null)
					{
						string loyaltyCardNumber = "";

						if (!retailTrans.LoyaltyItem.Empty)
						{
							loyaltyCardNumber = retailTrans.LoyaltyItem.CardNumber;
						}

						returnValue = loyaltyCardNumber;
					}
					break;
				case "CREDITMEMONUMBER":
					if (retailTrans != null)
					{
						string creditMemoNumber;
						if ((retailTrans.CreditMemoItem == null) ||
							(retailTrans.CreditMemoItem.CreditMemoNumber == null))
							creditMemoNumber = "";
						else
							creditMemoNumber = retailTrans.CreditMemoItem.CreditMemoNumber;

						returnValue = creditMemoNumber;
					}
					break;
				case "CREDITMEMOAMOUNT":
					decimal transAmount = decimal.Zero;
					if (retailTrans != null)
					{
						transAmount = retailTrans.CreditMemoItem != null
							? retailTrans.CreditMemoItem.Amount
							: decimal.Zero;

						returnValue = rounding.RoundString(prnEntry, 
															transAmount,
															prnSettings.Store.Currency, 
															false, 
															CacheType.CacheTypeTransactionLifeTime);
					}

					if (transAmount == decimal.Zero)
					{
						string creditMemoAmount = "";
						if (tenderItem.GetType() == typeof(CreditMemoTenderLineItem))
						{
							creditMemoAmount = rounding.RoundString(prnEntry,
																	((CreditMemoTenderLineItem)tenderItem).Amount, 
																	prnSettings.Store.Currency,
																	true, 
																	CacheType.CacheTypeTransactionLifeTime);
						}

						returnValue = creditMemoAmount;
					}

					break;
				case "CREDITMEMOSERIALNUMBER":
					string creditMemoSerialNumber = "";
					if (tenderItem.GetType() == typeof(CreditMemoTenderLineItem))
					{
						creditMemoSerialNumber = ((CreditMemoTenderLineItem)tenderItem).SerialNumber;
					}

					returnValue = creditMemoSerialNumber;
					break;
				case "CREDITMEMOBARCODE":
					if (tenderItem.GetType() == typeof(CreditMemoTenderLineItem))
					{
						returnValue = BarcodePrintMarkers.CreditMemoMarker + ((CreditMemoTenderLineItem)tenderItem).SerialNumber + BarcodePrintMarkers.BarcodeEndMarker;
						BarcodePrintInfoList.Add(new BarcodePrintInfo(BarcodePrintTypeEnum.CreditMemo, ((CreditMemoTenderLineItem)tenderItem).SerialNumber));
					}

					break;
				case "GIFTCARDNUMBER":
					if (retailTrans != null)
					{
						returnValue = ((GiftCertificateTenderLineItem)tenderItem).SerialNumber;
					}
					break;
				case "GIFTCARDAMOUNT":
					if (retailTrans != null)
					{
						string giftCardAmount = "";
						if (tenderItem.GetType() == typeof(GiftCertificateTenderLineItem))
						{
							giftCardAmount = rounding.RoundString(prnEntry,
																	((GiftCertificateTenderLineItem)tenderItem).Amount, 
																	prnSettings.Store.Currency,
																	true, 
																	CacheType.CacheTypeTransactionLifeTime);
						}
						returnValue = giftCardAmount;
					}

					break;

				case "GIFTCARDBARCODE":
					if (tenderItem.GetType() == typeof(GiftCertificateTenderLineItem))
					{
						returnValue = BarcodePrintMarkers.GiftCardMarker + ((GiftCertificateTenderLineItem)tenderItem).SerialNumber + BarcodePrintMarkers.BarcodeEndMarker;
						BarcodePrintInfoList.Add(new BarcodePrintInfo(BarcodePrintTypeEnum.GiftCard, ((GiftCertificateTenderLineItem)tenderItem).SerialNumber));
					}

					break;
				case "GIFTCARDBALANCE":
					string giftCardBalance = "";
					if (tenderItem.GetType() == typeof(GiftCertificateTenderLineItem))
					{
						ISiteServiceService service = (ISiteServiceService)prnEntry.Service(ServiceType.SiteServiceService);
						Parameters parameters = Providers.ParameterData.Get(prnEntry);
						SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(prnEntry, parameters.SiteServiceProfile);

						var giftCard = service.GetGiftCard(prnEntry, siteServiceProfile, ((GiftCertificateTenderLineItem)tenderItem).SerialNumber, true);
						giftCardBalance = rounding.RoundString(prnEntry,
																giftCard.Balance, 
																giftCard.Currency,
																true, 
																CacheType.CacheTypeTransactionLifeTime);
					}
					returnValue = giftCardBalance;
					break;
				case "OILTAXAMOUNT":
					if (retailTrans != null)
					{
						returnValue = rounding.RoundString(prnEntry, 
															retailTrans.Oiltax,
															prnSettings.Store.Currency, 
															false, 
															CacheType.CacheTypeTransactionLifeTime);
					}
					break;
				case "ALLTENDERCOMMENTS":
					if (retailTrans != null)
					{
						string allTenderComments = "";
						foreach (ITenderLineItem tenderLineItem in retailTrans.TenderLines)
						{
							if (tenderLineItem.Comment != null)
							{
								allTenderComments += tenderLineItem.Comment;
							}
						}
						returnValue = allTenderComments;
					}
					break;
				case "ALLITEMCOMMENTS":
					if (retailTrans != null)
					{
						string allItemComments = "";
						foreach (ISaleLineItem saleLineItem in retailTrans.SaleItems)
						{
							if (saleLineItem.Comment != null)
								allItemComments += saleLineItem.Comment;
						}
						returnValue = allItemComments;
					}
					break;
				case "NUMBEROFSALELINES":
					if (retailTrans != null)
					{
						int count = 0;
						foreach (ISaleLineItem saleLineItem in retailTrans.SaleItems)
						{
							if (!saleLineItem.Voided)
								count++;
						}
						returnValue = rounding.RoundForReceipt(prnEntry, count, 0);
					}
					break;
				case "NUMBEROFSALEITEMS":
					if (retailTrans != null)
					{
						decimal items = 0m;
						foreach (ISaleLineItem saleLineItem in retailTrans.SaleItems)
						{
							if (!saleLineItem.Voided)
								items += saleLineItem.Quantity;
						}
						returnValue = rounding.RoundForReceipt(prnEntry, items, 0);
					}
					break;
				case "TAXTOTAL":
					if (retailTrans != null)
					{
						returnValue = rounding.RoundString(prnEntry,
															retailTrans.TaxAmount,
															prnSettings.Store.Currency,
															false,
															CacheType.CacheTypeTransactionLifeTime);
					}
					break;
				case "TRANSACTIONDESCRIPTION":
					if (trans is RemoveTenderTransaction)
					{
						returnValue = ((RemoveTenderTransaction)trans).Description;
					}
					else if (trans is FloatEntryTransaction)
					{
						returnValue = ((FloatEntryTransaction)trans).Description;
					}
					break;
				case "BANKDROPBAGID":
					if (trans is BankDropTransaction)
					{
						returnValue = ((BankDropTransaction)trans).BankBagNo;
					}
					else if (trans is BankDropReversalTransaction)
					{
						returnValue = ((BankDropReversalTransaction)trans).BankBagNo;
					}
					break;
				case "PREVIOUSLYTENDERED":
					if (trans is FloatEntryTransaction)
					{
						returnValue = rounding.RoundString(prnEntry, 
															((FloatEntryTransaction)trans).PreviouslyTendered,
															prnSettings.Store.Currency, 
															false, 
															CacheType.CacheTypeTransactionLifeTime);
					}
					break;
				case "ADDEDAMOUNT":
					if (trans is FloatEntryTransaction)
					{
						returnValue = rounding.RoundString(prnEntry, 
															((FloatEntryTransaction)trans).AddedAmount,
															prnSettings.Store.Currency, 
															false, 
															CacheType.CacheTypeTransactionLifeTime);
					}
					break;
				case "REASON":
					if (trans is FloatEntryTransaction)
					{
						returnValue = ((FloatEntryTransaction)trans).Description;
					}
					break;
				case "CURRENCYSYMBOL":
					Currency currency = Providers.CurrencyData.Get(prnEntry, prnSettings.Store.Currency);
					returnValue = currency.Symbol;
					break;
				case "TENDERREMOVALAMOUNT":
					if (trans is RemoveTenderTransaction)
					{
						returnValue = rounding.RoundString(prnEntry, 
															((RemoveTenderTransaction)trans).Amount,
															prnSettings.Store.Currency, 
															false, 
															CacheType.CacheTypeTransactionLifeTime);
					}
					break;
				case "TENDERREMOVALREASON":
					if (trans is RemoveTenderTransaction)
					{
						returnValue = ((RemoveTenderTransaction)trans).Description;
					}
					break;
				case "SUSPENDEDDESTINATION":
					if (retailTrans != null)
						returnValue = retailTrans.SuspendDestination;
					break;
				case "SUSPENDEDIDBARCODE":
					if (retailTrans != null)
					{
						returnValue = BarcodePrintMarkers.SuspendedTransMarker + retailTrans.TransactionId + BarcodePrintMarkers.BarcodeEndMarker;
						BarcodePrintInfoList.Add(new BarcodePrintInfo(BarcodePrintTypeEnum.SuspendedSale, retailTrans.TransactionId));
					}
					break;
				case "SUMLINEDISCOUNT":
					decimal sumLine = retailTrans.DisplayAmountsIncludingTax 
											? retailTrans.LineDiscountWithTax + retailTrans.PeriodicDiscountWithTax 
											: retailTrans.LineDiscount + retailTrans.PeriodicDiscountAmount;
					returnValue = rounding.RoundString(prnEntry, 
														sumLine, 
														prnSettings.Store.Currency, 
														false, 
														CacheType.CacheTypeTransactionLifeTime);
					break;
				case "SUMTOTALDISCOUNT":
					decimal sumTotal = retailTrans.DisplayAmountsIncludingTax 
											? retailTrans.TotalDiscountWithTax 
											: retailTrans.TotalDiscount;
					returnValue = rounding.RoundString(prnEntry, 
														sumTotal, 
														prnSettings.Store.Currency, 
														false, 
														CacheType.CacheTypeTransactionLifeTime);
					break;
				case "LINEDISCOUNTNAME":
					returnValue = Properties.Resources.SumLineDiscount;
					break;
				case "TOTALDISCOUNTNAME":
					returnValue = Properties.Resources.SumTotalDiscount;
					break;
				case "SUMTOTALTRANSACTIONDISCOUNT":
					decimal sumTransactionTotal = retailTrans.DisplayAmountsIncludingTax
												  ? retailTrans.LineDiscountWithTax + retailTrans.PeriodicDiscountWithTax + retailTrans.TotalDiscountWithTax
												  : retailTrans.LineDiscount + retailTrans.PeriodicDiscountAmount + retailTrans.TotalDiscount;
					returnValue = rounding.RoundString(prnEntry, sumTransactionTotal, prnSettings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
					break;
				case "TOTALTRANSACTIONDISCOUNTNAME":
					returnValue = Properties.Resources.SumTotalTransactionDiscount;
					break;
				case "REFERENCE":
					if (retailTrans != null && !retailTrans.CustomerOrder.Empty())
					{
						returnValue = (string)retailTrans.CustomerOrder.Reference;
					}
					break;
				case "DELIVERY":
					if (retailTrans != null && !retailTrans.CustomerOrder.Empty())
					{
						returnValue = string.IsNullOrEmpty(retailTrans.CustomerOrder.Delivery.Text) ? Properties.Resources.NotAvailable : retailTrans.CustomerOrder.Delivery.Text;
					}
					break;
				case "SOURCE":
					if (retailTrans != null && !retailTrans.CustomerOrder.Empty())
					{
						returnValue = string.IsNullOrEmpty(retailTrans.CustomerOrder.Source.Text) ? Properties.Resources.NotAvailable : retailTrans.CustomerOrder.Source.Text;
					}
					break;
				case "DELIVERYLOCATION":
					if (retailTrans != null && !retailTrans.CustomerOrder.Empty())
					{
						returnValue = !string.IsNullOrEmpty(retailTrans.CustomerOrder.DeliveryLocationText) ? retailTrans.CustomerOrder.DeliveryLocationText :
									   string.IsNullOrEmpty(retailTrans.CustomerOrder.DeliveryLocation.StringValue) ? Properties.Resources.NotAvailable : (string)retailTrans.CustomerOrder.DeliveryLocation;
					}
					break;
				case "EXPIRATIONDATE":
					if (retailTrans != null && !retailTrans.CustomerOrder.Empty())
					{
						returnValue = (string)retailTrans.CustomerOrder.ExpirationDate.ToShortDateString();
					}
					break;
				case "COMMENT":
					if (retailTrans != null && !retailTrans.CustomerOrder.Empty())
					{
						returnValue = (string)retailTrans.CustomerOrder.Comment;
					}
					break;
				case "DEPOSITAMOUNT":
					if (retailTrans != null && !retailTrans.CustomerOrder.Empty())
					{
						decimal amt = retailTrans.SaleItems.Where(w => !w.Voided).Sum(item => item.Order.DepositAlreadyPaid());
						returnValue = rounding.RoundString(prnEntry, amt, prnSettings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
					}
					break;
				case "REMAININGAMOUNT":
					if (retailTrans != null && !retailTrans.CustomerOrder.Empty())
					{
						decimal amt = retailTrans.TransSalePmtDiff;
						returnValue = rounding.RoundString(prnEntry, amt, prnSettings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
					}
					break;
				case "REFERENCEBARCODE":
					if (retailTrans != null && !retailTrans.CustomerOrder.Empty())
					{
						returnValue = BarcodePrintMarkers.CustomerOrderMarker + retailTrans.CustomerOrder.Reference + BarcodePrintMarkers.BarcodeEndMarker;
						BarcodePrintInfoList.Add(new BarcodePrintInfo(BarcodePrintTypeEnum.CustomerOrder, (string)retailTrans.CustomerOrder.Reference));
					}
					break;
				case "ORDERTYPEDESCRIPTION":
					if (retailTrans != null && !retailTrans.CustomerOrder.Empty())
					{
						returnValue = retailTrans.CustomerOrder.OrderType == CustomerOrderType.CustomerOrder ? Properties.Resources.CustomerOrder : Properties.Resources.Quote;
					}
					break;
				case "TABLEDESCRIPTION":
					if (retailTrans != null && !retailTrans.Hospitality.Empty())
					{
						returnValue = retailTrans.Hospitality.TableInformation.Description;
					}
					break;
				case "ORDERNUMBER":
					if (retailTrans != null && !retailTrans.Hospitality.Empty() && retailTrans.TransactionId.Length >= 2)
					{
						returnValue = retailTrans.TransactionId.Substring(retailTrans.TransactionId.Length - 2, 2);
					}
					break;
				case "MENUTYPES":
					if (retailTrans != null && !retailTrans.Hospitality.Empty())
					{
						List<string> menuTypes = retailTrans.SaleItems.Where(x => x.MenuTypeItem.Description != "").Select(x => x.MenuTypeItem.Description).Distinct().ToList();
						returnValue = string.Join(", ", menuTypes);
					}
					break;
				case "STATION":
					if (retailTrans != null && !retailTrans.Hospitality.Empty())
					{
						returnValue = retailTrans.Hospitality.RestaurantStation;
					}
					break;
				case "TRANSACTIONFISCALSIGNATURE":
				case "FISCALINFO1":
				case "FISCALINFO2":
				case "FISCALINFO3":
				case "FISCALINFO4":
					//the default/base values for Fiscalization are empty
					returnValue = string.Empty;
					break;
				default:
					int trimLeft = 0;
					Address address = null;
					if (variable.StartsWith("ADDRESS"))
						address = GetAddress(retailTrans, Address.AddressTypes.Shipping);
					else if (variable.StartsWith("BILLINGADDRESS"))
					{
						address = GetAddress(retailTrans, Address.AddressTypes.Billing);
						trimLeft = 7;
					}
					else if (variable.StartsWith("SHIPPINGADDRESS"))
					{
						address = GetAddress(retailTrans, Address.AddressTypes.Shipping);
						trimLeft = 8;
					}

					if (address != null)
					{
						string tmpVariable = variable.Substring(trimLeft);

						switch (tmpVariable)
						{
							case "ADDRESSLINE1":
								returnValue = address.Address1;
								break;
							case "ADDRESSLINE2":
								returnValue = address.Address2;
								break;
							case "ADDRESSZIP":
								returnValue = address.Zip;
								break;
							case "ADDRESSCITY":
								returnValue = address.City;
								break;
							case "ADDRESSSTATE":
								returnValue = address.State;
								break;
							case "ADDRESSCOUNTY":
								returnValue = address.County;
								break;
							case "ADDRESSCOUNTRY":
								returnValue = prnEntry.Cache.GetCountryName(address.Country);
								break;
						}
					}

					//variable was not found and there is no valid value for it - it might be a partner customization
					if (returnValue == string.Empty)
					{
						variableFound = false;
					}

					break;
			}

			return returnValue;
		}

		// For text only forms
		public virtual string GetTransformedForm(FormSystemType systemType)
		{
			StringBuilder returnString = new StringBuilder();

			// Getting a dataset containing the headerpart of the current form
			DataSet ds = LoadXmlForm(systemType, FormPart.Header);
			returnString.Append(ReadDataset(ds, null, null, FormPartEnum.Header));

			// further modification of the string
			DataTable formDetails = ds.Tables["FormDetails"];
			if (formDetails != null)
			{
				DataRow detailRow = formDetails.Rows[0];
				{
					if (Convert.ToBoolean(detailRow["UpperCase"]))
					{
						return returnString.ToString().ToUpperInvariant();
					}
				}
			}
			return returnString.ToString();
		}

		public virtual FormInfo GetInfoForForm(FormSystemType systemType, bool copyReceipt, bool displayNoPrintForm, bool useEmailProfile = false)
		{
			return GetInfoForForm(systemType, copyReceipt, displayNoPrintForm, useEmailProfile ? emailFormProfileID : formProfileID);
		}

		public virtual FormInfo GetInfoForForm(FormSystemType systemType, bool copyReceipt, bool displayNoPrintForm, Form form, bool useEmailProfile = false)
		{
			return GetInfoForForm(systemType, copyReceipt, displayNoPrintForm, useEmailProfile ? emailFormProfileID : formProfileID, form);
		}

		public virtual FormInfo GetInfoForForm(FormSystemType systemType, bool copyReceipt, bool displayNoPrintForm, RecordIdentifier profileID)
		{
			Form form = Providers.FormData.GetProfileForm(prnEntry, profileID, systemType, CacheType.CacheTypeTransactionLifeTime);
			return GetInfoForForm(systemType, copyReceipt, displayNoPrintForm, profileID, form);
		}

		private FormInfo GetInfoForForm(FormSystemType systemType, bool copyReceipt, bool displayNoPrintForm, RecordIdentifier profileID, Form form)
		{
			FormInfo formInfo = null;
			ISettings settings = (ISettings)prnEntry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

			if (form != null)
			{
				WindowsPrinterConfiguration winPrinterConfiguration = null;

				if(form.UseWindowsPrinter && form.WindowsPrinterConfigurationID != RecordIdentifier.Empty)
				{
#pragma warning disable 0612, 0618
					winPrinterConfiguration = Providers.WindowsPrinterConfigurationData.Get(DLLEntry.DataModel, form.WindowsPrinterConfigurationID, CacheType.CacheTypeTransactionLifeTime);
#pragma warning restore 0612, 0618
				}

				formInfo = new FormInfo
					{
						Reprint = copyReceipt,
						PrintAsSlip = form.PrintAsSlip,
						PrintBehavior = form.PrintBehavior,
						UseWindowsPrinter = form.UseWindowsPrinter || settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.Windows,
						WindowsPrinterName = form.UseWindowsPrinter 
						? winPrinterConfiguration != null ? winPrinterConfiguration.PrinterDeviceName : ""
						: (settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.Windows ? settings.HardwareProfile.WindowsPrinterConfiguration.PrinterDeviceName : settings.HardwareProfile.PrinterDeviceName),
						FormWidth = form.DefaultFormWidth,
						FormDescription = form.Text,
                        NumberOfCopiesToPrint = form.NumberOfCopiesToPrint,
                        WindowsPrinterConfiguration = winPrinterConfiguration
					};
			}
			else if (displayNoPrintForm)
			{
				string msg = Properties.Resources.NoPrintForm + Environment.NewLine +
					Properties.Resources.NoPrintFormDetails
						.Replace("%1", profileID == null ? Properties.Resources.NoFormProfile : GetFormProfileDescription(profileID))
						.Replace("%2", systemType.ToString());
				Interfaces.Services.DialogService(prnEntry).ShowMessage(msg, MessageBoxButtons.OK,  MessageDialogType.ErrorWarning); // No printform has been defined. Please create a printform or turn of printing in the hardware profile.
			}
			return formInfo;
		}

		protected virtual string GetFormProfileDescription(RecordIdentifier formProfileID)
		{
			if (string.IsNullOrEmpty(formProfileDescription))
			{
				FormProfile profile = Providers.FormProfileData.Get(prnEntry, formProfileID, CacheType.CacheTypeApplicationLifeTime);
				if (profile == null)
				{
					return (string) formProfileID;
				}

				formProfileDescription = profile.Text;
			}

			return formProfileDescription;

		}

		public virtual FormInfo GetInfoForForm(FormSystemType systemType, bool copyReceipt, bool useEmailProfile = false)
		{
			return GetInfoForForm(systemType, copyReceipt, true, useEmailProfile);
		}
	}
}