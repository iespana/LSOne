using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality.ListItems
{
    public class HospitalityTypeListItem : DataEntity
    {

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(RestaurantID, new RecordIdentifier(Sequence, SalesType));
            }
            set
            {
                base.ID = value;
            }
        }

        public HospitalityTypeListItem()
            :base()
        {
            RestaurantID = "";
            Sequence = 0;
            SalesType = "";
            Text = "";
            SalesTypeDescription = "";
        }

        public RecordIdentifier RestaurantID { get; set; }
        public RecordIdentifier Sequence { get; set; }
        public RecordIdentifier SalesType { get; set; }

        public string RestaurantName { get; internal set; }
        public string SalesTypeDescription { get; set; }

        public override string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return (string)RestaurantID;

                    case 1:
                        return (string)SalesType;
                    
                    case 2:
                        return Sequence.ToString();

                    case 3:
                        return Text;

                    default:
                        return base[index];
                }


            }
        }
    }
}
