using System;
using System.Collections.Generic;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.BarcodeUtilities;

namespace LSOne.Services
{
    public partial class BarcodeService
    {
        private const string BarcodeID = "B:";

        /// <summary>
        /// Parse a receipt barcode to get store and terminal information
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="receiptID">Receipt ID</param>
        /// <returns>Struct containing Store ID, Terminal ID and Receipt ID</returns>
        public BarcodeReceiptParseInfo ParseBarcodeReceipt(IConnectionManager entry, string receiptID)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            BarCode barcode = new BarCode { ID = BarcodeID };
            BarcodeMask barcodeMask;

            Guid permissionOverrideContext = Guid.NewGuid();
            try
            {
                HashSet<string> overrides = new HashSet<string> { Permission.ManageBarcodesMasks, Permission.ManageItemBarcodes };
                entry.BeginPermissionOverride(permissionOverrideContext, overrides);

                // See if we have a special barcode mask for receipt ids
                barcodeMask = Providers.BarcodeMaskData.GetMaskForBarcode(entry, barcode);
            }
            finally
            {
                entry.EndPermissionOverride(permissionOverrideContext);
            }

            BarcodeReceiptParseInfo parseInfo = new BarcodeReceiptParseInfo { ReceiptID = receiptID };

            if (barcodeMask != null)
            {
                barcode.MaskId = (string)barcodeMask.ID;
                barcode.ID = "B:" + receiptID;
                barcode.Prefix = "B:";
                barcode.InternalType = barcodeMask.InternalType;
                barcode.Type = barcodeMask.Type;

                ProcessMaskSegments(entry, barcode);

                if (!string.IsNullOrEmpty(barcode.Store))
                {
                    parseInfo.StoreID = barcode.Store;
                }

                if (!string.IsNullOrEmpty(barcode.Terminal))
                {
                    parseInfo.TerminalID = barcode.Terminal;
                }

                if (!string.IsNullOrEmpty(barcode.ReceiptID))
                {
                    parseInfo.ReceiptID = barcode.ReceiptID;

                    string receiptIdNumberSequence = (string)settings.Terminal.ReceiptIDNumberSequence;
                    NumberSequence numberSeq = null;

                    if (!string.IsNullOrEmpty(receiptIdNumberSequence))
                    {
                        numberSeq = Providers.NumberSequenceData.Get(entry, receiptIdNumberSequence);
                    }

                    if (numberSeq == null)
                    {
                        numberSeq = Providers.NumberSequenceData.Get(entry, TransactionProviders.ReceiptSequence.DefaultReceiptSequence);
                    }

                    if (numberSeq != null)
                    {
                        // Adjust length to match the number sequence
                        while (parseInfo.ReceiptID.Length < numberSeq.Format.Length)
                        {
                            parseInfo.ReceiptID = "0" + parseInfo.ReceiptID;
                        }
                    }
                }
            }

            return parseInfo;
        }

        /// <summary>
        /// Get receipt barcode data based on mask segments and barcode parsed info
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="parseInfo">Parsed info of the barcode</param>
        /// <returns></returns>
        public string GetReceiptBarCodeData(IConnectionManager entry, BarcodeReceiptParseInfo parseInfo)
        {
            List<BarcodeMaskSegment> barcodeMaskSegments = GetMaskSegments(entry);

            if (barcodeMaskSegments != null)
            {
                string barcodeValue = "";
                foreach (BarcodeMaskSegment segment in barcodeMaskSegments)
                {
                    string current = "";
                    switch (segment.Type)
                    {
                        case BarcodeSegmentType.Item:
                        case BarcodeSegmentType.AnyNumber:
                        case BarcodeSegmentType.CheckDigit:
                        case BarcodeSegmentType.SizeDigit:
                        case BarcodeSegmentType.ColorDigit:
                        case BarcodeSegmentType.StyleDigit:
                        case BarcodeSegmentType.EANLicenseCode:
                        case BarcodeSegmentType.Price:
                        case BarcodeSegmentType.Quantity:
                        case BarcodeSegmentType.Employee:
                        case BarcodeSegmentType.Customer:
                        case BarcodeSegmentType.DataEntry:
                        case BarcodeSegmentType.SalesPerson:
                        case BarcodeSegmentType.Pharmacy:
                            break; // Not supported here
                        case BarcodeSegmentType.Store:
                            current = parseInfo.StoreID;
                            break;
                        case BarcodeSegmentType.Terminal:
                            current = parseInfo.TerminalID;
                            break;
                        case BarcodeSegmentType.Receipt:
                            current = parseInfo.ReceiptID;
                            break;
                    }

                    if (current != null)
                    {
                        if (current.Length < segment.Length)
                            current = current.PadLeft(segment.Length, '0');
                        else if (current.Length > segment.Length)
                            current = current.Substring(current.Length - segment.Length);
                        barcodeValue += current;
                    }
                }

                return barcodeValue;
            }

            return parseInfo.ReceiptID;
        }

        /// <summary>
        /// Get the barcode symbology based on the type of barcode
        /// </summary>
        /// <param name="typeOfBarcode">Type of barcode</param>
        /// <param name="barcodeSymbology">Barcode symbology reference</param>
        /// <param name="barcode">Barcode reference</param>
        /// <param name="barcodeMessage">Barcode message reference</param>
        /// <param name="size">Size reference</param>
        public void ManageReceiptBarcode(BarcodeType typeOfBarcode, ref int barcodeSymbology, ref string barcode, ref string barcodeMessage, ref Size size)
        {
            switch (typeOfBarcode)
            {
                case BarcodeType.EAN128:
                    barcodeSymbology = 120; // (int)OPOSPOSPrinterConstants.PTR_BCS_EAN128;
                    barcodeMessage = Properties.Resources.BarcodePrintingRequiresCustomization.Replace("#1", "EAN128");
                    break;
                case BarcodeType.Interleaved2of5:
                    barcodeSymbology = 106; // (int)OPOSPOSPrinterConstants.PTR_BCS_ITF;
                    BarcodeUtilities.AdjustBarcodeI2of5(ref barcode);
                    break;
                case BarcodeType.Code128:
                    barcodeSymbology = 110; // (int)OPOSPOSPrinterConstants.PTR_BCS_Code128;
                    barcodeMessage = Properties.Resources.BarcodePrintingRequiresCustomization.Replace("#1", "Code128");
                    break;
                case BarcodeType.UPCA:
                    barcodeSymbology = 101; // (int)OPOSPOSPrinterConstants.PTR_BCS_UPCA;
                    barcodeMessage = Properties.Resources.BarcodePrintingRequiresCustomization.Replace("#1", "UPCA");
                    break;
                case BarcodeType.UPCE:
                    barcodeSymbology = 102; // (int)OPOSPOSPrinterConstants.PTR_BCS_UPCE;
                    barcodeMessage = Properties.Resources.BarcodePrintingRequiresCustomization.Replace("#1", "UPCE");
                    break;
                case BarcodeType.EAN13:
                    barcodeSymbology = 104; //(int)OPOSPOSPrinterConstants.PTR_BCS_EAN13;
                    BarcodeUtilities.AdjustBarcodeEAN13(ref barcode);
                    break;
                case BarcodeType.EAN8:
                    barcodeSymbology = 8; // (int)OPOSPOSPrinterConstants.PTR_BCS_EAN8;
                    BarcodeUtilities.AdjustBarcodeEAN8(ref barcode);
                    break;
                case BarcodeType.PDF417:
                    barcodeSymbology = 9; //(int)OPOSPOSPrinterConstants.PTR_BCS_PDF417;
                    barcodeMessage = Properties.Resources.BarcodePrintingRequiresCustomization.Replace("#1", "PDF417");
                    break;
                case BarcodeType.MaxiCode:
                    barcodeSymbology = 202; // (int)OPOSPOSPrinterConstants.PTR_BCS_MAXICODE;
                    barcodeMessage = Properties.Resources.BarcodePrintingRequiresCustomization.Replace("#1", "MAXICODE");
                    break;
                default:
                    barcodeSymbology = 108; //(int)OPOSPOSPrinterConstants.PTR_BCS_Code39;
                    size = new Size(8, 40);
                    break;
            }
        }

        private List<BarcodeMaskSegment> GetMaskSegments(IConnectionManager entry)
        {
            Guid permissionOverrideContext = Guid.NewGuid();
            try
            {
                HashSet<string> overrides = new HashSet<string> { Permission.ManageBarcodesMasks, Permission.ManageItemBarcodes };
                entry.BeginPermissionOverride(permissionOverrideContext, overrides);

                BarCode barcode = new BarCode { ID = BarcodeID };
                BarcodeMask barcodeMask = Providers.BarcodeMaskData.GetMaskForBarcode(entry, barcode);

                if (barcodeMask != null)
                {
                    return Providers.BarcodeMaskSegmentData.Get(entry, barcodeMask.ID);
                }
            }
            finally
            {
                entry.EndPermissionOverride(permissionOverrideContext);
            }

            return null;
        }
    }
}
