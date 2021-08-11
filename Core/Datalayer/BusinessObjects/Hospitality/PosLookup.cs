using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class PosLookup : DataEntity
    {

        public PosLookup()
            :base(RecordIdentifier.Empty, "")
        {
            ID = RecordIdentifier.Empty;
            Text = "";
            DynamicMenuID = "";
            DynamicMenu2ID = "";
            Grid1MenuID = "";
            Grid2MenuID = "";
        }

        public RecordIdentifier DynamicMenuID { get; set; }
        public RecordIdentifier DynamicMenu2ID { get; set; }
        public RecordIdentifier Grid1MenuID { get; set; }
        public RecordIdentifier Grid2MenuID { get; set; }
    }
}
