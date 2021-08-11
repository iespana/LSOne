using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services
{
    internal class DLLEntry
    {
        private static ISettings settings;
        internal static IConnectionManager DataModel { get; set; }

        internal static ISettings Settings
        {
            get
            {
                return settings ?? (settings = (ISettings) DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication));
            }
        }
    }
}
