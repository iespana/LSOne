using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities.BarCodeTypes
{
    internal class BarcodeEAN13 : BarcodeEAN_UPC
    {
        public BarcodeEAN13()
            : base()
        {

        }

        public override string Name
        {
            get
            {
                return "EAN13";
            }
        }

        public override int TypeNumber
        {
            get
            {
                return 7;
            }
        }

        public override int MinimumLength
        {
            get
            {
                return 13;
            }
        }

        public override int MaximumLength
        {
            get
            {
                return 13;
            }
        }

        
    }
}
