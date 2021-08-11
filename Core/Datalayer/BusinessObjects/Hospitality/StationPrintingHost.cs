using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class StationPrintingHost : DataEntity
    {
        public StationPrintingHost() : base()
        {
            ID = RecordIdentifier.Empty;
            Text = string.Empty;
            Address = string.Empty;
            Port = 0;
            CharSet = 0;
            LockCode = string.Empty;
            DebugLevelConsole = 0;
            DebugLevelFile = 0;
            DebugFileCount = 0;
            DebugFileSize = 0;
        }

        public string Address { get; set; }
        public int Port { get; set; }
        public int CharSet { get; set; }
        public string LockCode { get; set; }
        public int DebugLevelConsole { get; set; }
        public int DebugLevelFile { get; set; }
        public int DebugFileSize { get; set; }
        public int DebugFileCount { get; set; }
    }
}
