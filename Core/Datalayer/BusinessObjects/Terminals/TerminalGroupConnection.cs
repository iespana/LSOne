using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Terminals
{

    public class TerminalGroupConnection 
    {

        public enum SortEnum
        {
            TerminalId,
            TerminalDescription,
            Location, 
            Description,
            ID
        }

        


        public RecordIdentifier TerminalGroupId { get; set; }
        public string TerminalGroupDescription { get; set; }
        public RecordIdentifier TerminalId { get; set; }
        public RecordIdentifier StoreId { get; set; }
        public string TerminalDescription { get; set; }
        public string StoreDescription { get; set; }
        public string Location { get; set; }

    }
}
