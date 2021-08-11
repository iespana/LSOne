using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.EFT.Common
{
    public class DLLEntry
    {
        private static ISettings settings;

        internal static IConnectionManager DataModel { get; set; }

        internal static ISettings Settings
        {
            get
            {
                return settings ?? (ISettings)DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            }
            set
            {
                settings = value;
            }
        }
    }
}
