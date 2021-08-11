using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities.BarCodeTypes
{
    internal class BarcodeMaxicode : BarCodeType
    {
        public BarcodeMaxicode()
            : base()
        {

        }

        public override string Name
        {
            get
            {
                return "Maxicode";
            }
        }

        public override int TypeNumber
        {
            get
            {
                return 10; // 102 in AX!!
            }
        }
    }
}
