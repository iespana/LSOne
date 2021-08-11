using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.BarCodes
{
    public class BarcodeMaskSegment
    {
        private BarcodeSegmentType type;

        /// <summary>
        /// Unique id for each segment that is defined in the barcode.
        /// </summary>
        public int SegmentNum { get; set; }

        /// <summary>
        /// The length of the segment.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// The type of the barcodesegment,i.e  price, quantity, or employeeid.
        /// </summary>
        public BarcodeSegmentType Type
        {
            get { return type; }
            set
            {
                type = value;
                SegmentChar = TypeToCharacter(value);
            }
        }

        /// <summary>
        /// The same as the segment type, except now shown as a char.
        /// </summary>
        public string SegmentChar { get; set; }

        /// <summary>
        /// A unique id to identify the mask segments for each barcode mask.
        /// </summary>
        public string MaskId { get; set; }

        /// <summary>
        /// The sequence of the parent barcode mask
        /// </summary>
        public int MaskSequence { get; set; }

        /// <summary>
        /// The sequence of this mask segment
        /// </summary>
        public int Sequence { get; set; }

        public RecordIdentifier UniqueID
        {
            get
            {
                return new RecordIdentifier(MaskId, Sequence);
            }
        }

        /// <summary>
        /// A number of decimals in the segment, if a price or quantity.
        /// </summary>
        public int Decimals { get; set; }

        #region Utilities
        public static string TypeToCharacter(BarcodeSegmentType type)
        {
            switch (type)
            {
                case BarcodeSegmentType.Item:
                    return "I";

                case BarcodeSegmentType.AnyNumber:
                    return "X";

                case BarcodeSegmentType.CheckDigit:
                    return "M";

                case BarcodeSegmentType.SizeDigit:
                    return "Z";

                case   BarcodeSegmentType.ColorDigit:
                    return "C";

                case   BarcodeSegmentType.StyleDigit:
                    return "S";

                case   BarcodeSegmentType.EANLicenseCode:
                    return "L";

                case   BarcodeSegmentType.Price:
                    return "P";

                case   BarcodeSegmentType.Quantity:
                    return "Q";

                case   BarcodeSegmentType.Employee:
                    return "E";

                case   BarcodeSegmentType.Customer:
                    return "D";

                case   BarcodeSegmentType.DataEntry:
                    return "A";

                case   BarcodeSegmentType.SalesPerson:
                    return "R";

                case   BarcodeSegmentType.Pharmacy:
                    return "F";

                case   BarcodeSegmentType.Store:
                    return "O";
                case   BarcodeSegmentType.Terminal:
                    return "T";
                case   BarcodeSegmentType.Receipt:
                    return "B";
                default:
                    return "";
            }
        }
        #endregion
    }
}
