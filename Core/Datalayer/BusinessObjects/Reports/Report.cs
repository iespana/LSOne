using System.ComponentModel.DataAnnotations;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Reports
{
    public class Report : ReportListItem
    {
        public Report()
            : base()
        {
            SqlData = "";
            LanguageID = "";
            SMReport = true;
        }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public override RecordIdentifier ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        public byte[] ReportData { get; set; }

        public string SqlData { get; set; }

        public bool SqlDataInstalled { get; set; }

        [StringLength(10)]
        public string LanguageID { get; set; }

        public bool SMReport { get; set; }
    }
}
