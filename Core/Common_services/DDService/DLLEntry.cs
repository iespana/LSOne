using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSRetail.DD.Common;

namespace LSOne.Services
{
    internal class DLLEntry
    {
        internal static IConnectionManager DataModel;
        private static SchedulerSettings schedulerSettings;

        internal static ISettings Settings
        {
            get { return DataModel.Settings != null ? 
                (ISettings)DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication) :
                null; }
        }

        internal static SchedulerSettings SchedulerSettings
        {
            get
            {
                if (schedulerSettings == null)
                {
                    schedulerSettings = DataProviderFactory.Instance.Get<IInfoData, JscInfo>().GetSchedulerSettings(DataModel);
                    if (schedulerSettings == null)
                    {
                        // No settings exist, create a default one
                        schedulerSettings = new SchedulerSettings()
                        {
                            ServerSettings = new ServerSettings
                            {
                                Host = System.Net.Dns.GetHostName(),
                                NetMode = NetMode.TCP,
                            }
                        };
                        DataProviderFactory.Instance.Get<IInfoData, JscInfo>().Save(DataModel, schedulerSettings);
                    }
                }

                return schedulerSettings;
            }
        }
    }
}
