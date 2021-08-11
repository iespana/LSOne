using LSOne.ViewPlugins.RMSMigration.Model.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Contracts
{
    public delegate void ReportProgressHandler();
    public delegate void SetProgressSizeHandler(int count);
    public interface IImportManager
    {
        event ReportProgressHandler ReportProgress;
        event SetProgressSizeHandler SetProgressSize;
        List<ImportLogItem> Import(ILookupManager lookupManager, string rmsConnectionString);
    }
}
