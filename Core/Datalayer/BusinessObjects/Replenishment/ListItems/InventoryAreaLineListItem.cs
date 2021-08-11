namespace LSOne.DataLayer.BusinessObjects.Replenishment.ListItems
{
    public class InventoryAreaLineListItem : DataEntity
    {
        public string AreaDescription { get; set; }
        public string AreaLineDescription { get; set; }
        public override string Text
        {
            get
            {
                return AreaLineDescription + " - " + AreaDescription.Remove(AreaDescription.IndexOf(" -"));
            }

            set
            {
                base.Text = value;
            }
        }
    }
}
