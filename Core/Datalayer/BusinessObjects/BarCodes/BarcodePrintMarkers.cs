using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.BarCodes
{
    public static class BarcodePrintMarkers
    {
        public const string ReceiptIDMarker = "<B: ";
        public const string SuspendedTransMarker = "<TB: ";
        public const string CustomerOrderMarker = "<RB: ";
        public const string CreditMemoMarker = "<CB: ";
        public const string GiftCardMarker = "<GB: ";

        public const string BarcodeEndMarker = ">";


        public static bool BarcodeMarkersInText(string text)
        {
            return text.Contains(ReceiptIDMarker)
                   || text.Contains(CustomerOrderMarker)
                   || text.Contains(SuspendedTransMarker)
                   || text.Contains(GiftCardMarker)
                   || text.Contains(CreditMemoMarker);
        }

        public static string[] AllBarcodeMarkers => new[]
        {
            ReceiptIDMarker,
            CustomerOrderMarker,
            SuspendedTransMarker,
            GiftCardMarker,
            CreditMemoMarker
        };
    }
}
