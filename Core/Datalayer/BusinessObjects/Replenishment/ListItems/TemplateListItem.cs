using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Replenishment.ListItems
{
    public class TemplateListItem
    {
        public RecordIdentifier TemplateID { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public string TemplateName { get; set; }
        public string StoreName { get; set; }
    }
}
