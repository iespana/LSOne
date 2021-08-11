using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.Interfaces.SupportClasses
{
    public class PreTriggerResults
    {
        public PreTriggerResults()
        {
            RunOperation = true;
            ShowMessageDialog = false;
            Message = "";
        }

        public bool RunOperation;
        public bool ShowMessageDialog;
        public string Message;
    }
}
