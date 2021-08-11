namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    public class InventoryTemplateListFilter
    {
        public string Description { get; set; }
        public bool DescriptionBeginsWith { get; set; }
        public TemplateEntryTypeEnum? EntryType { get; set; }

        public InventoryTemplateListFilter()
        {
            DescriptionBeginsWith = true;
            EntryType = null;
        }
    }
}
