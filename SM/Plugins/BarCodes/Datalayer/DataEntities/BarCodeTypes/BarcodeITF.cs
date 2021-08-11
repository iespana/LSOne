using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities.BarCodeTypes
{
    internal class BarcodeITF : BarCodeType
    {
        public BarcodeITF()
            : base()
        {

        }

        public override string Name
        {
            get
            {
                return "Interleaved 2 of 5";
            }
        }

        public override string DefaultFont
        {
            get
            {
                return "BC I25 HD Wide";
            }
        }

        public override int TypeNumber
        {
            get
            {
                return 3;
            }
        }

        public override List<DataEntity> GetValidFonts()
        {
            List<DataEntity> items = new List<DataEntity>();

            items.Add(new DataEntity("BC I25 Narrow", "33"));
            items.Add(new DataEntity("BC I25 Medium", "65"));
            items.Add(new DataEntity("BC I25 Wide", "100"));
            items.Add(new DataEntity("BC I25 HD Narrow", "8"));
            items.Add(new DataEntity("BC I25 HD Medium", "15"));
            items.Add(new DataEntity("BC I25 HD Wide", "25"));

            return items;
        }
    }
}
