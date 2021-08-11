using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities.BarCodeTypes
{
    internal class BarcodeEAN8 : BarcodeEAN_UPC
    {
        public BarcodeEAN8()
            : base()
        {

        }

        public override string Name
        {
            get
            {
                return "EAN8";
            }
        }

        public override int TypeNumber
        {
            get
            {
                return 8;
            }
        }

        public override int MinimumLength
        {
            get
            {
                return 8;
            }
        }

        public override int MaximumLength
        {
            get
            {
                return 8;
            }
        }


    }
}
