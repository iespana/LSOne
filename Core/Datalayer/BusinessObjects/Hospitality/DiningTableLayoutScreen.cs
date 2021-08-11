using System;
using System.Drawing;
using LSOne.Utilities.DataTypes;
#if !MONO
#endif

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class DiningTableLayoutScreen : DataEntity
    {
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(RestaurantID,
                    new RecordIdentifier(Sequence,
                    new RecordIdentifier(SalesType,
                    new RecordIdentifier(LayoutID, ScreenNo))));                    
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }


        public DiningTableLayoutScreen()
            : base()
        {
            RestaurantID = "";
            Sequence = 0;
            SalesType = "";
            LayoutID = "";
            ScreenNo = 0;
            Text = "";
#if !MONO
            BackgroundImage = null;
#endif
        }

        public RecordIdentifier RestaurantID { get; set; }
        public RecordIdentifier Sequence { get; set; }
        public RecordIdentifier SalesType { get; set; }
        public RecordIdentifier LayoutID { get; set; }
        public RecordIdentifier ScreenNo { get; set; }
#if !MONO
        public Image BackgroundImage { get; set; }       
#endif
    }
}
