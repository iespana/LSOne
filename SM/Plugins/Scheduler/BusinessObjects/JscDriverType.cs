using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{
    public class JscDriverType 
    {
        public RecordIdentifier Id { get; set; }
        public string Name { get; set; }
        public DataSrvType DatabaseServerType { get; set; }
        public string DatabaseParams { get; set; }
        public string ConnectionStringFormat { get; set; }
        public string EnabledFields { get; set; }
    }
}
