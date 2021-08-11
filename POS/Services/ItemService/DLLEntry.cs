using System;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services
{
    [Obsolete("The DLLEntry should not be used except when absolutely necessary. A IConnectionManager parameter should be used")]
    internal class DLLEntry
    {
        internal static IConnectionManager DataModel;
        internal static ISettings Settings;
    }
}
