using System.ComponentModel.DataAnnotations;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

#if !MONO

#endif

namespace LSOne.DataLayer.BusinessObjects.BarCodes
{
    public class BarCodeSetup : DataEntity
    {
        int minimumLength;
        int maximumLength;
        int barCodeType;
        string fontName;
        string barCodeMask;
        int fontSize;


        public BarCodeSetup()
            : base()
        {
            minimumLength = 0;
            maximumLength = 0;
            barCodeType = 0;
            fontName = "";
            barCodeMask = "";
            fontSize = 0;
        }

        [RecordIdentifierValidation(20, Depth = 1)]
        public override RecordIdentifier ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        public int MinimumLength
        {
            get { return minimumLength; }
            set { minimumLength = value; }
        }

        public int MaximumLength
        {
            get { return maximumLength; }
            set { maximumLength = value; }
        }

        public int BarCodeType
        {
            get { return barCodeType; }
            set { barCodeType = value; }
        }

        [StringLength(32)]
        public string FontName
        {
            get { return fontName; }
            set { fontName = value; }
        }

        [StringLength(30)]
        public string Description
        {
            get { return Text; }
            set { Text = value; }
        }

        [StringLength(22)]
        public string BarCodeMask
        {
            get { return barCodeMask; }
            set { barCodeMask = value; }
        }

        public string BarCodeMaskDescription { get; set; }

        public RecordIdentifier BarCodeMaskID { get; set; }

        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
    }
}
