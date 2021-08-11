using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System.Windows.Forms;

namespace LSOne.Services
{
    /// <summary>
    /// After an input comes into the POS regardless of how it was recieved i.e. OPOS scan, keyboard scan, keyboard input this service is called
    /// and will parse the input to figure out what was scanned. If the result is that the scanned input is an item barcode any information that
    /// is included in the barcode such as price, unit, quantity is added to the <see cref="BarCode"/> object before returning it to the POS for further processing
    /// </summary>
    public partial class BarcodeService : IBarcodeService
    {
        /// <summary>
        /// Access to the error log functionality
        /// </summary>
        public virtual IErrorLog ErrorLog { set; private get; }

        /// <summary>
        /// The quantity of the barcodes that is being scanned i.e. when the user selects 5 * and then scans a barcode then 
        /// the barcode should be handled as if it was scanned 5 times.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Initializes the barcode service. The entry into the database is received from whoever called the service and set
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        public void Init(IConnectionManager entry)
        {
            #pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
            #pragma warning restore 0612, 0618
        }

        /// <summary>
        /// Receives a barcode as a string, creates a <see cref="ScanInfo"/> object and calls the overloaded version of ProcessBarcode that handles the <see cref="ScanInfo"/> object
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="entrytype">What type of barcode was being scanned</param>
        /// <param name="barcode">The string that was scanned</param>
        /// <param name="selectedItemID"></param>
        /// <returns></returns>
        public virtual BarCode ProcessBarcode(IConnectionManager entry, BarCode.BarcodeEntryType entrytype, string barcode, string selectedItemID = "")
        {
            var scanInfo = new ScanInfo(barcode) {EntryType = entrytype};
            return ProcessBarcode(entry, scanInfo, selectedItemID);
        }

        /// <summary>
        /// Processes a barcode that was received and adds any and all information that is in the barcode to a <see cref="ISaleLineItem"/> 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="saleLineItem">The sale line item that should be updated with information from the barcode if applicable</param>
        /// <param name="scanInfo">The information from the scan</param>
        /// <param name="barCode">The barcode that was scanned. If null then a <see cref="ScanInfo"/> object is created from the barcode string and <see cref="IBarcodeService.ProcessBarcode(IConnectionManager,ref ISaleLineItem,ScanInfo,BarCode,string, string)"/> is called to resolve the barcode</param>
        /// <param name="barcode">The scanned barcode as a string</param>
        /// <param name="selectedItemID"></param>
        /// <returns></returns>
        public virtual BarCode ProcessBarcode(IConnectionManager entry, ref ISaleLineItem saleLineItem, ScanInfo scanInfo, BarCode barCode, string barcode, string selectedItemID = "")
        {
            if (barCode == null)
            {
                if (scanInfo == null)
                {
                    scanInfo = new ScanInfo(barcode);
                }

                barCode = ProcessBarcode(entry, scanInfo, selectedItemID);
            }

            // The BarCode has already been populated by the ProcessInput operation, which triggered the ItemSale operation
            if ((barCode.InternalType == BarcodeInternalType.Item) && (barCode.ItemID != null))
            {
                // The entry was a barcode which was found and now we have the item id...
                saleLineItem.ItemId = (string) barCode.ItemID;
                saleLineItem.BarcodeId = (string) barCode.ID;
                saleLineItem.SalesOrderUnitOfMeasure = (string)barCode.UnitID;

                if (barCode.BarcodeQuantity > 0)
                {
                    saleLineItem.Quantity = barCode.BarcodeQuantity;
                }
            }
            else
            {
                // It could be an ItemID
                saleLineItem.ItemId = (string) barCode.ID;
            }

            saleLineItem.EntryType = barCode.EntryType;

            if (barCode.QtySold > 0)
            {
                saleLineItem.UnitQuantity = barCode.QtySold;
            }
            if (barCode.BarcodePrice > 0)
            {
                var settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                saleLineItem.TaxIncludedInItemPrice = settings.Store.KeyedInPriceIncludesTax;
                if (settings.Store.KeyedInPriceIncludesTax)
                {
                    saleLineItem.PriceWithTax = barCode.BarcodePrice;
                }
                else
                {
                    saleLineItem.Price = barCode.BarcodePrice;
                }
                saleLineItem.PriceInBarcode = true;
                saleLineItem.WasChanged = true;
            }

            return barCode;
        }

        /// <summary>
        /// The "main" ProcessBarcode function that will parse the barcode scanned according to the barcode masks that have been configured and 
        /// return information back to the POS what type of barcode was being scanned (see <see cref="BarcodeInternalType"/>
        /// 
        /// This function will first call <see cref="PartnerProcessBarcode"/> where a partner can process the scanned input in a customization and bypass the default code.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="scanInfo">Information about the scanned input</param>
        /// <param name="selectedItemID"></param>
        /// <returns></returns>
        public virtual BarCode ProcessBarcode(IConnectionManager entry, ScanInfo scanInfo, string selectedItemID = "")
        {
            var barCode = PartnerProcessBarcode(entry, scanInfo);
            if (barCode != null)
                return barCode;

            barCode = new BarCode();

            try
            {
                // The BarCode is null and we need to populate it. The operation was triggered by i.e. a button or ItemSearch

                string barcodeid = scanInfo.ScanDataLabel ?? "";

                barCode.ID = barcodeid;
                barCode.ItemBarCode = barcodeid;
                barCode.EntryType = scanInfo.EntryType;
                barCode.TimeStarted = DateTime.Now;

                //If entered barcode contains "," or "."  then it should be processed as an empty barcode
                if (barcodeid.Contains(",") || barcodeid.Contains("."))
                {
                    barcodeid = "";
                }

                if (string.IsNullOrEmpty(barcodeid))
                {
                    barCode.Found = false;
                }
                else if (barcodeid == "<qr>")
                {
                    barCode.InternalType = BarcodeInternalType.Coupon;
                }
                else
                {
                    try
                    {
                        // If the input string that we are trying to identify contains any other caracters than numeric characters....
                        if (VerifyCheckDigit(barCode))
                        {
                            barCode.CheckDigitValidated = true;
                        }
                        else
                        {
                            barCode.CheckDigitValidated = false;
                        }
                    }
                    catch (Exception)
                    {
                    }

                    //Check if barcode is found as it was entered into the system.
                    if (!RecordIdentifier.IsEmptyOrNull(selectedItemID))
                        barCode.ItemID = selectedItemID;

                    Providers.BarCodeData.AddInformationToBarcode(entry, barCode);

                    BarcodeMask barcodeMask = Providers.BarcodeMaskData.GetMaskForBarcode(entry, barCode);

                    //If not found, check if it is a maskable barcode.
                    if (!barCode.Found)
                    {
                        if (barcodeMask != null && barcodeMask.Found)
                        {
                            barCode.MaskId = (string)barcodeMask.ID;
                            barCode.Prefix = barcodeMask.Prefix;
                            barCode.InternalType = barcodeMask.InternalType;
                            barCode.Type = barcodeMask.Type;
                            barCode.BarcodePrice = 0;
                            barCode.BarcodeQuantity = 0;
                            barCode.DataEntry = "";
                            barCode.EmployeeId = "";
                            barCode.CouponId = "";
                            barCode.EANLicenseId = "";
                            barCode.CustomerId = "";

                            switch (barcodeMask.InternalType)
                            {
                                case BarcodeInternalType.Item:
                                {
                                    ProcessMaskSegments(entry, barCode);
                                    ProcessUPCRandomWeightBarcode(entry, barCode);
                                }
                                break;
                                case BarcodeInternalType.Customer:
                                case BarcodeInternalType.Coupon:
                                case BarcodeInternalType.DataEntry:
                                case BarcodeInternalType.Employee:
                                case BarcodeInternalType.SalesPerson:
                                case BarcodeInternalType.Pharmacy:
                                case BarcodeInternalType.Discount:
                                case BarcodeInternalType.CreditMemo:
                                case BarcodeInternalType.GiftCard:
                                case BarcodeInternalType.Customized:
                                {
                                    ProcessMaskSegments(entry, barCode);
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        barCode.InternalType = BarcodeInternalType.Item;

                        //If we have an UPC Type 2 barcode (random weight)
                        if (barcodeMask != null && barcodeMask.Found && barcodeMask.IsUPCAType2())
                        {
                            barCode.MaskId = (string)barcodeMask.ID;
                            barCode.Prefix = barcodeMask.Prefix;
                            barCode.Type = barcodeMask.Type;
                            barCode.BarcodePrice = 0;
                            barCode.BarcodeQuantity = 0;

                            ProcessMaskSegments(entry, barCode);
                            ProcessUPCRandomWeightBarcode(entry, barCode);
                        }
                    }
                }
                return barCode;
            }
            finally
            {
                barCode.TimeFinished = DateTime.Now;
                barCode.TimeElapsed = barCode.TimeFinished - barCode.TimeStarted;
            }
        }

        /// <summary>
        /// Calculates the barcode checkdigit to verify if it is correct.
        /// The checkdigit is assumed to be the last digit in the barcode
        /// </summary>
        /// <param name="barCode">The barcode to be verified</param>
        /// <returns>Returns true if the check digit is correct</returns>
        protected virtual bool VerifyCheckDigit(BarCode barCode)
        {
            //Calculate the checkdigit
            int checkDigit = CalcCheckDigit(((string)barCode.ID).Substring(0, ((string)barCode.ID).Length - 1));
            int lastDigit = Conversion.ToInt(((string)barCode.ID).Substring(((string)barCode.ID).Length - 1, 1), -1);

            if (lastDigit == checkDigit)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes the different barcode segments
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="barCode">The barcode to be parsed</param>
        public virtual void ProcessMaskSegments(IConnectionManager entry, BarCode barCode)
        {
            var segments = Providers.BarCodeData.GetBarcodeSegments(entry, barCode);
            int position = barCode.Prefix.Length;
            foreach (var segment in segments)
            {
                if (position + segment.Length > ((string) barCode.ID).Length)
                {
                    return;
                }
                switch (segment.Type)
                {
                    case BarcodeSegmentType.Item:
                        {
                            var tmp = barCode.ItemBarCode;
                            barCode.ItemBarCode = ((string)barCode.ID).Substring(0, position + segment.Length);
                            barCode.ItemBarCode = (string)barCode.ItemBarCode + '%'; // BarCode.ItemBarcode.PadRight(13, '0');
                            Providers.BarCodeData.AddInformationToBarcode(entry, barCode);
                            if (barCode.Found == false)
                            {
                                barCode.ItemBarCode = ((string)barCode.ItemBarCode).Substring(0, ((string)barCode.ItemBarCode).Length - 1);
                                barCode.ItemBarCode = (string)barCode.ItemBarCode + Convert.ToString(CalcCheckDigit((string)barCode.ItemBarCode));
                                Providers.BarCodeData.AddInformationToBarcode(entry, barCode);
                            }
                            barCode.ItemBarCode = tmp;
                        }
                        break;
                    case BarcodeSegmentType.AnyNumber:
                        break;
                    case BarcodeSegmentType.CheckDigit:
                        break;
                    case BarcodeSegmentType.SizeDigit:
                        //{
                        //    barCode.SizeID = ((string) barCode.ID).Substring(position, segment.Length);
                        //    break;
                        //}
                        break;
                    case BarcodeSegmentType.ColorDigit:
                        //{
                        //    barCode.ColorID = ((string) barCode.ID).Substring(position, segment.Length);
                        //    break;
                        //}
                        break;
                    case BarcodeSegmentType.StyleDigit:
                        //{
                        //    barCode.StyleID = ((string) barCode.ID).Substring(position, segment.Length);
                        //    break;
                        //}
                        break;
                    case BarcodeSegmentType.EANLicenseCode:
                        {
                            barCode.EANLicenseId = ((string) barCode.ID).Substring(position, segment.Length);
                        }
                        break;
                    case BarcodeSegmentType.Price:
                        {
                            string temp = ((string) barCode.ID).Substring(position, segment.Length);
                            barCode.BarcodePrice = Convert.ToDecimal(temp);
                            barCode.BarcodePrice = barCode.BarcodePrice/(decimal) Math.Pow(10, segment.Decimals);
                            barCode.Decimals = segment.Decimals;
                        }
                        break;
                    case BarcodeSegmentType.Quantity:
                        {
                            string temp = ((string) barCode.ID).Substring(position, segment.Length);
                            barCode.BarcodeQuantity = Convert.ToDecimal(temp);
                            barCode.BarcodeQuantity = barCode.BarcodeQuantity/(decimal) Math.Pow(10, segment.Decimals);
                            barCode.Decimals = segment.Decimals;
                        }
                        break;
                    case BarcodeSegmentType.Employee:
                        {
                            barCode.EmployeeId = ((string) barCode.ID).Substring(position, segment.Length);
                        }
                        break;
                    case BarcodeSegmentType.SalesPerson:
                        {
                            barCode.SalespersonId = ((string) barCode.ID).Substring(position, segment.Length);
                        }
                        break;
                    case BarcodeSegmentType.Customer:
                        {
                            barCode.CustomerId = ((string) barCode.ID).Substring(position, segment.Length);
                        }
                        break;
                    case BarcodeSegmentType.DataEntry:
                        {
                            barCode.DataEntry = ((string) barCode.ID).Substring(position, segment.Length);
                        }
                        break;
                    case BarcodeSegmentType.Pharmacy:
                        {
                            barCode.PharmacyPrescriptionId = ((string) barCode.ID).Substring(position, segment.Length);
                        }
                        break;
                    case BarcodeSegmentType.Store:
                        barCode.Store = ((string)barCode.ID).Substring(position, segment.Length);
                        break;
                    case BarcodeSegmentType.Terminal:
                        barCode.Terminal = ((string)barCode.ID).Substring(position, segment.Length);
                        break;
                    case BarcodeSegmentType.Receipt:
                        barCode.ReceiptID = ((string)barCode.ID).Substring(position, segment.Length);
                        break;
                }
                position = position + segment.Length;
            }
        }

        /// <summary>
        /// Calculates the checkdigit for a barcode, without a checkdigit
        /// </summary>
        /// <param name="barcode">barcode without a checkdigit</param>
        /// <returns>The calculated checkdigit</returns>
        protected virtual int CalcCheckDigit(string barcode)
        {
            int even = 0;
            int odd = 0;
            for (int i = 0; i < barcode.Length; i++)
            {
                if (((i + 1)%2) == 0)
                {
                    even += Conversion.ToInt(barcode.Substring((barcode.Length - 1 - i), 1), 0);
                }
                else
                {
                    odd += Conversion.ToInt(barcode.Substring((barcode.Length - 1 - i), 1), 0);
                }
            }

            int total = (odd * 3) + even;
            
            var checkDigit = 10 - (total % 10);
            return checkDigit;
        }

        /// <summary>
        /// Calculates the quantity of the item based on the scanned price and sales price of the scanned item.
        /// Scanned item should be a scale item.
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="barCode">Scanned barcode</param>
        protected virtual void ProcessUPCRandomWeightBarcode(IConnectionManager entry, BarCode barCode)
        {
            if(barCode.IsUPCAType2())
            {
                RetailItem item = Providers.RetailItemData.Get(entry, barCode.ItemID);

                if(item != null && item.ScaleItem)
                {
                    decimal itemUnitPrice = item.SalesPriceIncludingTax;

                    barCode.ScaleItem = true;
                    barCode.BarcodeQuantity = barCode.BarcodePrice / itemUnitPrice;
                    barCode.BarcodePrice = itemUnitPrice;

                    barCode.KeyInQuantity = KeyInQuantities.NotMandatory;
                    barCode.KeyInPrice = KeyInPrices.NotMandatory;
                }
            }
        }
    }
}
