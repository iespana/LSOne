using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services
{
    internal class DLLEntry
    {
        internal static IConnectionManager DataModel;
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
