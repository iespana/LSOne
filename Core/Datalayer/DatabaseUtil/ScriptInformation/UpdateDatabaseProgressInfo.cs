using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.DatabaseUtil.ScriptInformation
{
    /// <summary>
    /// Used intenally to track the current status of the create/update database process. This information is then used by <see cref="DatabaseUtil.DatabaseUtility"/> when broadcasting <see cref="UpdateDatabaseProgressCallback"/>
    /// </summary>
    internal class UpdateDatabaseProgressInfo
    {
        public UpdateDatabaseProgressInfo()
        {
            CurrentScript = 0;
            TotalScripts = 0;
        }

        public int CurrentScript;
        public int TotalScripts;        
    }
}
