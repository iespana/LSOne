using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class RestaurantDiningTable : DataEntity
    {

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(RestaurantID,
                    new RecordIdentifier(Sequence,
                    new RecordIdentifier(SalesType,
                    new RecordIdentifier(DiningTableLayoutID, DineInTableNo))));
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }
                

        public RestaurantDiningTable()
            : base()
        {
            RestaurantID = "";
            SalesType = "";
            DineInTableNo = 0;
            Text = "";
            Type = TypeEnum.CasualSeating;
            NonSmoking = false;
            NoOfGuests = 0;
            JoinedTable = 0;
            X1Position = 0;
            X2Position = 0;
            Y1Position = 0;
            Y2Position = 0;
            LinkedToDineInTable = 0;
            DiningTablesJoined = false;
            Sequence = 0;
            Availability = AvailabilityEnum.Available;
            DiningTableLayoutID = "";
            LayoutScreenNo = 0;
            DescriptionOnButton = "";
            Shape = ShapeEnum.Ellipse;
            X1PositionDesign = 0;
            X2PositionDesign = 0;
            Y1PositionDesign = 0;
            Y2PositionDesign = 0;
            LayoutScreenNoDesign = 0;
            JoinedTableDesign = 0;
            DiningTablesJoinedDesign = false;
            DeleteInOtherLayouts = false;

        }

        public RecordIdentifier RestaurantID { get; set; }
        public RecordIdentifier SalesType { get; set; }
        public RecordIdentifier DineInTableNo { get; set; }        
        public TypeEnum Type { get; set; }
        public bool NonSmoking { get; set; }
        public int NoOfGuests { get; set; }
        public int JoinedTable { get; set; }
        public int X1Position { get; set; }
        public int X2Position { get; set; }
        public int Y1Position { get; set; }
        public int Y2Position { get; set; }
        public int LinkedToDineInTable { get; set; }
        public bool DiningTablesJoined { get; set; }
        public RecordIdentifier Sequence { get; set; }
        public AvailabilityEnum Availability { get; set; }
        public RecordIdentifier DiningTableLayoutID { get; set; }
        public RecordIdentifier LayoutScreenNo { get; set; }
        public string DescriptionOnButton { get; set; }
        public ShapeEnum Shape { get; set; }
        public int X1PositionDesign { get; set; }
        public int X2PositionDesign { get; set; }
        public int Y1PositionDesign { get; set; }
        public int Y2PositionDesign { get; set; }
        public RecordIdentifier LayoutScreenNoDesign { get; set; }
        public int JoinedTableDesign { get; set; }
        public bool DiningTablesJoinedDesign { get; set; }
        public bool DeleteInOtherLayouts { get; set; }

        public string GetDescription(HospitalityType.TableButtonDescriptionEnum TableButtonDescr)
        {
            switch (TableButtonDescr)
            {
                case HospitalityType.TableButtonDescriptionEnum.Description: 
                    return this.Text;
                case HospitalityType.TableButtonDescriptionEnum.DescriptionPlusNumber: 
                    return this.Text + " " + this.DineInTableNo.ToString();
                case HospitalityType.TableButtonDescriptionEnum.Number: 
                    return this.DineInTableNo.ToString();
                case HospitalityType.TableButtonDescriptionEnum.NumberPlusDescription: 
                    return this.DineInTableNo.ToString() + " " + this.Text;                
            }

            return this.Text;
        }

        public string GetDescription(HospitalityType.TableButtonDescriptionEnum TableButtonDescr, string descriptionText)
        {
            switch (TableButtonDescr)
            {
                case HospitalityType.TableButtonDescriptionEnum.Description:
                    return descriptionText;
                case HospitalityType.TableButtonDescriptionEnum.DescriptionPlusNumber:
                    return descriptionText + " " + this.DineInTableNo.ToString();
                case HospitalityType.TableButtonDescriptionEnum.Number:
                    return this.DineInTableNo.ToString();
                case HospitalityType.TableButtonDescriptionEnum.NumberPlusDescription:
                    return this.DineInTableNo.ToString() + " " + descriptionText;
            }

            return descriptionText;
        }

        #region enums
        public enum TypeEnum
        {
            CasualSeating = 0,
            Oval = 1,
            Square = 2,
            Rectangle = 3
        }

        public enum AvailabilityEnum
        {
            Available = 0,
            NotAvailable = 1
        }

        public enum ShapeEnum
        {
            Rectangle = 0,
            Square = 1,
            Ellipse = 2
        }
        #endregion
    }
}
