using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DDBusinessObjects
{
    public class JscDatabaseDesign
    {
        public RecordIdentifier ID { get; set; }
        public string Description { get; set; }
        public int? CodePage { get; set; }
        public bool Enabled { get; set; }
    }
}
