using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities.BarCodeTypes
{
    internal class BarcodePDF417 : BarCodeType
    {
        public BarcodePDF417()
            : base()
        {

        }

        public override string Name
        {
            get
            {
                return "PDF417";
            }
        }

        public override int TypeNumber
        {
            get
            {
                return 9; // 101 in AX!!!
            }
        }
    }
}
