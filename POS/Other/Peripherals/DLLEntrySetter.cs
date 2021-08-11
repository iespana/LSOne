using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Peripherals
{
    public class DLLEntrySetter
    {
        public static void Initialize(IConnectionManager entry, ISettings settings)
        {
            DLLEntry.DataModel = entry;
            DLLEntry.Settings = settings;
        }
    }
}
