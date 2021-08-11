using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.UserInterface.ListItems
{
    public class UIStyleListItem : DataEntity
    {
        public RecordIdentifier ParentStyleID { get; set; }
        public RecordIdentifier ContextID { get; set; }

        public string ParentStyleDescription { get; set; }
    }
}
