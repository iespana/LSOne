using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities.BarCodeTypes
{
    internal class BarcodeUPCE : BarcodeEAN_UPC
    {
        public BarcodeUPCE()
            : base()
        {

        }

        public override string Name
        {
            get
            {
                return "UPC E";
            }
        }

        public override int TypeNumber
        {
            get
            {
                return 6;
            }
        }

        public override int MinimumLength
        {
            get
            {
                return 6;
            }
        }

        public override int MaximumLength
        {
            get
            {
                return 6;
            }
        }


    }
}
