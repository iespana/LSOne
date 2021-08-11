using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class StationPrintingRoute : DataEntity
    {

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(StationPrintingRoutesID, RestaurantID);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public StationPrintingRoute()
            : base()
        {
            StationPrintingRoutesID = "";
            Text = "";
            RestaurantID = "";
            Password = "";
        }

        public RecordIdentifier StationPrintingRoutesID { get; set; } // This field is named ID in the database        
        public RecordIdentifier RestaurantID { get; set; }
        public string Password { get; set; }
    }
}
