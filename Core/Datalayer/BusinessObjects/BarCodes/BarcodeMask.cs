using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.BarCodes
{
    public class BarcodeMask : DataEntity
    {
        /// <summary>
        /// A mask string
        /// </summary>
        public string Mask { get; set; }

        /// <summary>
        /// The barcode prefix
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// The type of a barcode, i.e EAN 13,UPC-E.
        /// </summary>
        public BarcodeType Type { get; set; }

        /// <summary>
        /// Is the barcode used for a customer,coupon etc.
        /// </summary>
        public BarcodeInternalType InternalType { get; set; }

        /// <summary>
        /// Set true if a BarcodeMask was found, else false.
        /// </summary>
        public bool Found { get; set; }

        /// <summary>
        /// The sequence of this mask. This is an identity column and is used for internal purposes
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Indicates if the barcode mask represents an UPCA type 2 (random weight) barcode
        /// </summary>
        public bool IsUPCAType2()
        {
            return InternalType == BarcodeInternalType.Item && Type == BarcodeType.UPCA && Prefix == "2" && Mask.Length == 12;
        }

    }
}
