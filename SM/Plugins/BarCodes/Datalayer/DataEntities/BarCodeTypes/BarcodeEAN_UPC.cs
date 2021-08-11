using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities.BarCodeTypes
{
    internal abstract class BarcodeEAN_UPC : BarCodeType
    {
        public BarcodeEAN_UPC()
            : base()
        {

        }

        public override string DefaultFont
        {
            get
            {
                return "BC UPC HD Wide";
            }
        }

        public override List<DataEntity> GetValidFonts()
        {
            List<DataEntity> items = new List<DataEntity>();

            items.Add(new DataEntity("BC UPC Narrow", "33"));
            items.Add(new DataEntity("BC UPC Medium", "65"));
            items.Add(new DataEntity("BC UPC Wide", "100"));
            items.Add(new DataEntity("BC UPC HD Narrow", "8"));
            items.Add(new DataEntity("BC UPC HD Medium", "15"));
            items.Add(new DataEntity("BC UPC HD Wide", "25"));

            return items;
        }
    }
}
