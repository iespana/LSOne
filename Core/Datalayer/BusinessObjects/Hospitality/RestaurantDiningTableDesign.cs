using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class RestaurantDiningTableDesign : DataEntity
    {

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(RestaurantID,
                    new RecordIdentifier(Sequence,
                    new RecordIdentifier(SalesType,
                    new RecordIdentifier(DiningTableLayoutID, DiningTableNo))));
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public RestaurantDiningTableDesign()
            : base()
        {
            RestaurantID = "";
            SalesType = "";
            DiningTableNo = 0;
            Sequence = 0;
            DiningTableLayoutID = "";
            X1PositionDesign = 0;
            X2PositionDesign = 0;
            Y1PositionDesign = 0;
            Y2PositionDesign = 0;
            LayoutScreenNoDesign = 0;
            JoinedTableDesign = 0;
            DiningTablesJoinedDesign = false;
        }

        public RecordIdentifier RestaurantID { get; set; }
        public RecordIdentifier SalesType { get; set; }
        public RecordIdentifier DiningTableNo { get; set; }
        public RecordIdentifier Sequence { get; set; }
        public RecordIdentifier DiningTableLayoutID { get; set; }
        public int X1PositionDesign { get; set; }
        public int X2PositionDesign { get; set; }
        public int Y1PositionDesign { get; set; }
        public int Y2PositionDesign { get; set; }
        public RecordIdentifier LayoutScreenNoDesign { get; set; }
        public int JoinedTableDesign { get; set; }
        public bool DiningTablesJoinedDesign { get; set; }

    }
}
