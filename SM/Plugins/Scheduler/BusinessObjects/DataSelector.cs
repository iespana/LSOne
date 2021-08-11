using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{
    /// <summary>
    /// A generic selector for data to be displayed in a user interface.
    /// </summary>
    public class DataSelector
    {
        public RecordIdentifier GuidId { get; set; }
        public int IntId { get; set; }
        public string Code { get; set; }
        public string Text { get; set; }
        public object Object { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
