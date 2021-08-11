using LSOne.ViewPlugins.RMSMigration.Contracts;
using LSOne.ViewPlugins.RMSMigration.Model.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Helper.Import
{
    public class DefaultImport

        : IImportManager
    {
        public event ReportProgressHandler ReportProgress;
        public event SetProgressSizeHandler SetProgressSize;
        private string RMSConnectionString { get; set; }
        public List<ImportLogItem> Import(ILookupManager lookupManager, string rmsConnectionString)
        {
            SetProgressSize(0);
            ReportProgress?.Invoke();
            return new List<ImportLogItem>();
        }
    }
}
