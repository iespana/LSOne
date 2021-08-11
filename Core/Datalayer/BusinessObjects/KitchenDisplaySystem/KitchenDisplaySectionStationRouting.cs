using LSOne.Utilities.DataTypes;


namespace LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem
{
    public class KitchenDisplaySectionStationRouting : DataEntity
    {
        /// <summary>
        /// ID of the restaurant the routing applies to
        /// </summary>
        public RecordIdentifier RestaurantId { get; set; }

        /// <summary>
        /// Name of the restaurant the routing applies to
        /// </summary>
        public string RestaurantName { get; set; }

        /// <summary>
        /// ID of the section the routing applies to
        /// </summary>
        public RecordIdentifier SectionId { get; set; }

        /// <summary>
        /// Code of the section the routing applies to
        /// </summary>
        public RecordIdentifier SectionCode { get; set; }

        /// <summary>
        /// Description of the section the routing applies to
        /// </summary>
        public string SectionDescription { get; set; }

        /// <summary>
        /// ID of the hospitality type the routing applies to
        /// </summary>
        public RecordIdentifier HospitalityTypeID { get; set; }

        /// <summary>
        /// Description of the hospitality type the routing applies to
        /// </summary>
        public string HospitalityTypeDescription { get; set; }

        /// <summary>
        /// Either display or printing station
        /// </summary>
        public StationTypeEnum StationType { get; set; }

        /// <summary>
        /// ID of the kitchen station that is routed to
        /// </summary>
        public RecordIdentifier StationId { get; set; }

        /// <summary>
        /// Name of the kitchen station that is routed to
        /// </summary>
        public string StationName { get; set; }

        public KitchenDisplaySectionStationRouting()
        {
            ID = RecordIdentifier.Empty;
            RestaurantId = RecordIdentifier.Empty;
            RestaurantName = string.Empty;
            SectionId = RecordIdentifier.Empty;
            SectionCode = RecordIdentifier.Empty;
            SectionDescription = string.Empty;
            HospitalityTypeID = RecordIdentifier.Empty;
            HospitalityTypeDescription = string.Empty;
            StationType = StationTypeEnum.Display;
            StationId = RecordIdentifier.Empty;
            StationName = string.Empty;
        }

        public enum StationTypeEnum
        {
            Display,
            Printer
        }

        public static string StationTypeToString(StationTypeEnum type)
        {
            switch (type)
            {
                case StationTypeEnum.Display:
                    return Properties.Resources.DisplayStation;
                case StationTypeEnum.Printer:
                    return Properties.Resources.PrintingStation;
                default:
                    return string.Empty;
            }
        }
    }
}