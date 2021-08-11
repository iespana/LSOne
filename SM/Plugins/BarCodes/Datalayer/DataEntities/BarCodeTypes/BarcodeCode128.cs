using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities.BarCodeTypes
{
    internal class BarcodeCode128 : BarCodeType
    {
        public BarcodeCode128()
            : base()
        {

        }

        public override string Name
        {
            get
            {
                return "Code 128";
            }
        }

        public override string DefaultFont
        {
            get
            {
                return "BC C128 HD Wide";
            }
        }

        public override int TypeNumber
        {
            get
            {
                return 4;
            }
        }

        public override List<DataEntity> GetValidFonts()
        {
            List<DataEntity> items = new List<DataEntity>();

            items.Add(new DataEntity("BC C128 Narrow", "33"));
            items.Add(new DataEntity("BC C128 Medium", "65"));
            items.Add(new DataEntity("BC C128 Wide", "100"));
            items.Add(new DataEntity("BC C128 HD Narrow", "8"));
            items.Add(new DataEntity("BC C128 HD Medium", "15"));
            items.Add(new DataEntity("BC C128 HD Wide", "25"));

            return items;
        }
    }
}
