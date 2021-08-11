using LSOne.DataLayer.BusinessObjects;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public class DatabaseDriverType
    {
        public string DisplayText { get; set; }
        public DataSrvType DataSrvType { get; set; }
        public string DatabaseParams { get; set; }
        public string Format { get; set; }
        public DatabaseStringFields[] EnabledFields { get; set; }

        public override string ToString()
        {
            return DisplayText;
        }
    }

}
