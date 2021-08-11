using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities
{
    public class ReportLine
    {
        public int EntryStatus { get; set; }
        public int Type { get; set; }
        public decimal NumberOfItems { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal LineDiscAmount { get; set; }
        public decimal TotalDiscAmount { get; set; }

    }
}
