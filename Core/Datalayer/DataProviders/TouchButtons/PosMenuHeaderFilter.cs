using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.TouchButtons
{
    public class PosMenuHeaderFilter
    {
        public string Description { get; set; }
        public bool DescriptionBeginsWith { get; set; }
        public int? MenuType { get; set; }
        public PosMenuHeaderSorting SortBy { get; set; }
        public bool SortBackwards { get; set; }
        public bool? MainMenu { get; set; }
        public int? DeviceType { get; set; }
        public RecordIdentifier StyleID { get; set; }
    }
}
