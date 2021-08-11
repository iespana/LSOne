using System;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services
{
    [Obsolete("The DLLEntry should not be used except when absolutely necessary. A IConnectionManager parameter should be used")]
    internal static class DLLEntry
    {
        internal static IConnectionManager DataModel { get; private set; }
        internal static ISettings Settings { get; private set; }

        internal static void Init(IConnectionManager entry, ISettings settings)
        {
            DataModel = entry;
            Settings = settings;
        }
    }
}
