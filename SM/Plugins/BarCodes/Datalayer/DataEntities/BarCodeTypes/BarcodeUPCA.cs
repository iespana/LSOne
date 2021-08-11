using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities.BarCodeTypes
{
    internal class BarcodeUPCA : BarcodeEAN_UPC
    {
        public BarcodeUPCA()
            : base()
        {

        }

        public override string Name
        {
            get
            {
                return "UPC A";
            }
        }

        public override int TypeNumber
        {
            get
            {
                return 5;
            }
        }

        public override int MinimumLength
        {
            get
            {
                return 12;
            }
        }

        public override int MaximumLength
        {
            get
            {
                return 12;
            }
        }


    }
}
