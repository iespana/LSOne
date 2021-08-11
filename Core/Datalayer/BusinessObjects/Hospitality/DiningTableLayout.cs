using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class DiningTableLayout : DataEntity
    {
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(RestaurantID,
                    new RecordIdentifier(Sequence,
                    new RecordIdentifier(SalesType, LayoutID)));
            }
            set
            {
                RestaurantID = value[0];
                Sequence = value[1];
                SalesType = value[2];
                LayoutID = value[3];
            }
        }

        public DiningTableLayout()
            : base()
        {
            RestaurantID = "";
            Sequence = 0;
            SalesType = "";
            LayoutID = "";
            Text = "";            
            NumberOfScreens = 0;
            StartingTableNumber = 0;
            NumberOfDiningTables = 0;
            EndingTableNumber = 0;
            DiningTableRows = 0;
            DiningTableColumns = 0;
            CurrentLayout = false;
            HospitalityTypeDescription = "";
        }

        [DataMember]
        public RecordIdentifier RestaurantID { get; set; }

        [DataMember]
        public RecordIdentifier Sequence { get; set; }

        [DataMember]
        public RecordIdentifier SalesType { get; set; }

        [DataMember]
        public RecordIdentifier LayoutID { get; set; }

        [DataMember]
        public int NumberOfScreens { get; set; }

        [DataMember]
        public int StartingTableNumber { get; set; }

        [DataMember]
        public int NumberOfDiningTables  { get; set; }

        [DataMember]
        public int EndingTableNumber { get; set; }

        [DataMember]
        public int DiningTableRows { get; set; }

        [DataMember]
        public int DiningTableColumns { get; set; }

        [DataMember]
        public bool CurrentLayout { get; set; }

        [DataMember]
        public string HospitalityTypeDescription { get; set; }

        public override object Clone()
        {
            DiningTableLayout dinTableLayoutDTO = new DiningTableLayout();

            dinTableLayoutDTO.RestaurantID = this.RestaurantID.ToString();
            dinTableLayoutDTO.Sequence = (int)this.Sequence;
            dinTableLayoutDTO.SalesType = this.SalesType.ToString();
            dinTableLayoutDTO.LayoutID = this.LayoutID.ToString();
            dinTableLayoutDTO.NumberOfScreens = this.NumberOfScreens;
            dinTableLayoutDTO.StartingTableNumber = this.StartingTableNumber;
            dinTableLayoutDTO.NumberOfDiningTables = this.NumberOfDiningTables;
            dinTableLayoutDTO.EndingTableNumber = this.EndingTableNumber;
            dinTableLayoutDTO.DiningTableRows = this.DiningTableRows;
            dinTableLayoutDTO.DiningTableColumns = this.DiningTableColumns;
            dinTableLayoutDTO.CurrentLayout = this.CurrentLayout;
            dinTableLayoutDTO.HospitalityTypeDescription = this.HospitalityTypeDescription;

            return dinTableLayoutDTO;
        }

        public override string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return (string)LayoutID;

                    case 1:
                        return Text;

                    case 2:
                        return HospitalityTypeDescription;

                    default:
                        return base[index];
                }


            }
        }
    }
}
