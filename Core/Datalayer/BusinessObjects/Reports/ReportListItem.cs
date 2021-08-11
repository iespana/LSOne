using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.BusinessObjects.Reports
{
    public class ReportListItem : DataEntity
    {
        public ReportListItem()
            : base()
        {
            Description = "";
        }

        public string Description { get; set; }
        public bool SystemReport { get; set; }
        public bool SiteServiceReport { get; set; }
        public ReportCategory ReportCategory { get; set; }
    }
}
