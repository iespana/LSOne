using System.Drawing;
using System.Windows.Forms.VisualStyles;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.Forms
{
    public class FormLine : DataEntity
    {
        public FormLineAppendTypeEnum AppendType;
        public string Line;
        public FormLineFontTypeEnum FontType;
        public FormLineTypeEnum PrintLineType;
        public HorizontalAlign TextAlignment;

        public FormLine()
        {
            AppendType = FormLineAppendTypeEnum.AppendLine;
            Line = "";
            FontType = FormLineFontTypeEnum.Normal;
            PrintLineType = FormLineTypeEnum.Text;
            TextAlignment = HorizontalAlign.Right;
        }
    }

    public class FormLineLogo : FormLine
    {
        public string FilePath;
        public LogoAlignEnum LogoAlign;

        public FormLineLogo() : base()
        {
            FilePath = "";
            LogoAlign = LogoAlignEnum.Center;
        }
    }

    public class FormLineBarcode : FormLine
    {
        public BarcodeType TypeOfBarcode;
        public Size BarcodeSize;
        public int BarcodeSymbologyType;
        public string BarcodeMessage;

        public FormLineBarcode() : base()
        {
            PrintLineType = FormLineTypeEnum.Barcode;
            AppendType = FormLineAppendTypeEnum.AppendLine;

            BarcodeMessage = "";
            TypeOfBarcode = BarcodeType.Code39;
            BarcodeSize.Width = 8;
            BarcodeSize.Height = 40;
            BarcodeSymbologyType = 108;
        }

        /// <summary>
        /// Sets a new barcode to be printed
        /// </summary>
        /// <param name="barcode">The barcode text that is to be printed</param>
        /// <param name="typeOfBarcode">The type of barcode to see more: <see cref="BarcodeType"/> </param>
        /// <param name="size">The size of the barcode. If parameter is Empty then the default size is used.</param>
        public FormLineBarcode(string barcode, BarcodeType typeOfBarcode, Size size) : this()
        {
            Line = barcode;
            TypeOfBarcode = typeOfBarcode;
            if (!size.IsEmpty)
            {
                BarcodeSize = size;
            }
        }
    }
}
