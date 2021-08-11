using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.BarCodes
{
    public class BarcodePrintInfo : DataEntity
    {
        /// <summary>
        /// What is the content of the barcode i.e. Receipt ID, Transaction ID, Gift card ID and etc.
        /// </summary>
        public BarcodePrintTypeEnum BarcodePrintType { get; set; }

        /// <summary>
        /// The actual text that should be printed as a barcode
        /// </summary>
        public string BarcodeToPrint { get; set; }

        /// <summary>
        /// The identifier within the print string that tells us that this barcode should be printed
        /// </summary>
        public string BarcodeMarker { get; set; }

        /// <summary>
        /// The space the barcode marker takes in string. Used when removing the marker from the receipt string
        /// </summary>
        public int BarcodeMarkerLength { get; set; }

        public bool Printed { get; set; }

        ///// <summary>
        ///// Is the actual barcode type i.e. Code39 = 108 see OPOSPOSPrinterConstants
        ///// </summary>
        //public int BarcodeSymbology { get; set; }

        ///// <summary>
        ///// If a barcode is not supported then a barcode message is printed instead of the barcode
        ///// </summary>
        //public string BarcodeMessage { get; set; }

        ///// <summary>
        ///// The width and height of the barcode. 
        ///// </summary>
        //public Size BarcodeSize { get; set; }

        ///// <summary>
        ///// Is always set to 1
        ///// </summary>
        //public int BCTextPos { get; set; }
        

        public BarcodePrintInfo()
        {
            Printed = false;
            //BCTextPos = 1;
            //BarcodeMessage = "";
            //BarcodeSymbology = 108;
            //BarcodeSize = new Size(8, 40);
        }

        public BarcodePrintInfo(string barcodePrintMarker, string barcodeToPrint) : this()
        {
            BarcodePrintType = GetBarcodePrintType(barcodePrintMarker);

            BarcodeToPrint = barcodeToPrint;
            BarcodeMarker = barcodePrintMarker;

            BarcodeMarkerLength = BarcodeMarker.Trim().Length + 1;
        }

        public BarcodePrintInfo(BarcodePrintTypeEnum barcodePrintType, string barcodeToPrint) : this()
        {
            BarcodePrintType = barcodePrintType;

            BarcodeToPrint = barcodeToPrint;
            BarcodeMarker = GetBarcodeMarker(barcodePrintType);

            BarcodeMarkerLength = BarcodeMarker.Trim().Length + 1;
        }

        public string GetBarcodeMarker(BarcodePrintTypeEnum barcodePrintType)
        {
            switch (BarcodePrintType)
            {
                case BarcodePrintTypeEnum.Receipt:
                    return BarcodePrintMarkers.ReceiptIDMarker;
                    
                case BarcodePrintTypeEnum.SuspendedSale:
                    return BarcodePrintMarkers.SuspendedTransMarker;
                    
                case BarcodePrintTypeEnum.CreditMemo:
                    return BarcodePrintMarkers.CreditMemoMarker;
                    
                case BarcodePrintTypeEnum.GiftCard:
                    return BarcodePrintMarkers.GiftCardMarker;
                    
                case BarcodePrintTypeEnum.CustomerOrder:
                    return BarcodePrintMarkers.CustomerOrderMarker;
                    
                default:
                    return BarcodePrintMarkers.ReceiptIDMarker;
            }
        }

        public BarcodePrintTypeEnum GetBarcodePrintType(string barcodeMarker)
        {
            switch (barcodeMarker)
            {
                case BarcodePrintMarkers.ReceiptIDMarker:
                    return BarcodePrintTypeEnum.Receipt;

                case BarcodePrintMarkers.CreditMemoMarker:
                    return BarcodePrintTypeEnum.CreditMemo;

                case BarcodePrintMarkers.CustomerOrderMarker:
                    return BarcodePrintTypeEnum.CustomerOrder;

                case BarcodePrintMarkers.GiftCardMarker:
                    return BarcodePrintTypeEnum.GiftCard;

                case BarcodePrintMarkers.SuspendedTransMarker:
                    return BarcodePrintTypeEnum.SuspendedSale;

                default:
                    return BarcodePrintTypeEnum.Receipt;
            }
        }
    }
}
