using System;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services
{
    [Obsolete("The DLLEntry should not be used except when absolutely necessary. A IConnectionManager parameter should be used")]
    internal class DLLEntry
    {
        internal static IConnectionManager DataModel = null;
        
        internal static ISettings Settings
        {
            get
            {
                return DataModel.Settings != null ?
                    (ISettings)DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication) :
                    null;
            }
        }
    }
}
