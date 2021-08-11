using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities.BarCodeTypes
{
    internal class BarcodeEAN128 : BarcodeCode128
    {
        public BarcodeEAN128()
            : base()
        {

        }

        public override string Name
        {
            get
            {
                return "EAN128/UCC128";
            }
        }

        public override int TypeNumber
        {
            get
            {
                return 1;
            }
        }


    }
}
