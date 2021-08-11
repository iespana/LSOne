using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities.BarCodeTypes
{
    internal class BarcodeCode39 : BarCodeType
    {
        public BarcodeCode39()
        {

        }

        public override string Name
        {
            get
            {
                return "Code 39";
            }
        }

        public override string DefaultFont
        {
            get
            {
                return "BC C39 3 to 1 HD Wide";
            }
        }

        public override int TypeNumber
        {
            get
            {
                return 2;
            }
        }

        public override List<DataEntity> GetValidFonts()
        {
            List<DataEntity> items = new List<DataEntity>();

            items.Add(new DataEntity("BC C39 3 to 1 Narrow", "33"));
            items.Add(new DataEntity("BC C39 3 to 1 Medium", "65"));
            items.Add(new DataEntity("BC C39 3 to 1 Wide", "100"));
            items.Add(new DataEntity("BC C39 3 to 1 HD Narrow", "8"));
            items.Add(new DataEntity("BC C39 3 to 1 HD Medium", "15"));
            items.Add(new DataEntity("BC C39 3 to 1 HD Wide", "25"));

            items.Add(new DataEntity("BC C39 2 to 1 Narrow", "33"));
            items.Add(new DataEntity("BC C39 2 to 1 Medium", "65"));
            items.Add(new DataEntity("BC C39 2 to 1 Wide", "100"));
            items.Add(new DataEntity("BC C39 2 to 1 HD Narrow", "8"));
            items.Add(new DataEntity("BC C39 2 to 1 HD Medium", "15"));
            items.Add(new DataEntity("BC C39 2 to 1 HD Wide", "25"));

            return items;
        }
    }
}
