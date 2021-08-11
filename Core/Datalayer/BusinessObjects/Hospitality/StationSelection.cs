using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class StationSelection : DataEntity
    {

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(RestaurantID,
                    new RecordIdentifier(SalesType,
                    new RecordIdentifier(Type,
                    new RecordIdentifier(Code, StationID))));
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public StationSelection()
        {
            Type = (int)TypeEnum.All;
            Code = "";
            StationID = "";
            SalesType = "";
            RestaurantID = "";
            HospitalityTypeDescription = "";
            StationDescription = "";
            CodeName = "";
        }

        public RecordIdentifier Type { get; set; }
        public RecordIdentifier Code { get; set; }
        public RecordIdentifier StationID { get; set; }
        public RecordIdentifier SalesType { get; set; }
        public RecordIdentifier RestaurantID { get; set; }
        public string CodeName { get; set; }
        public string HospitalityTypeDescription { get; set; }
        public string StationDescription { get; set; }

        #region Enums
        public enum TypeEnum
        {
            All = 0,
            RetailGroup = 1,
            Item = 2,
            SpecialGroup = 3
        }

        public enum TerminalConnectionEnum
        {
            All = 0,
            Terminal = 1,
            TerminalGroup = 2
        }

        public enum HospitalityTypeConnectionEnum
        {
            All = 0,
            HospitalityType = 1
        }
        #endregion
    }
}
