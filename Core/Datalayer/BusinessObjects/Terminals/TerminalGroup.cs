using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Terminals
{
    public class TerminalGroup : DataEntity
    {

        public enum SortEnum
        {
            
            Description,
            ID
        }

        public RecordIdentifier TerminalId { get; set; }
    }
}
