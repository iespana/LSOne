using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class PosColor : DataEntity
    {        

        public PosColor()
        {
            this.Color = -1;
            this.Bold = false;
            this.Text = "";
            this.ColorCode = RecordIdentifier.Empty;
        }

        public PosColor(string colorCode, string description)
            : base(colorCode, description)
        {
            this.Color = -1;
            this.Bold = false;
            this.Text = description;
            this.ColorCode = RecordIdentifier.Empty;
        }

        public RecordIdentifier ColorCode { get; set; }        
        public int Color { get; set; }
        public bool Bold { get; set; }
    }
}
