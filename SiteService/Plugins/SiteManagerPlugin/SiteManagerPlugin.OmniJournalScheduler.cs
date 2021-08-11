using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.SiteService.Plugins.SiteManager.Jobs;
using LSOne.SiteService.Utilities;

using Quartz;

using System;

namespace LSOne.SiteService.Plugins.SiteManager
{
	public partial class SiteManagerPlugin
	{
		private static IScheduler omniJournalScheduler = null;
		private static int maxRetryCounter = 3;
		private static int updateInterval = 0;

		protected virtual void UpdateOmniJournalSchedule()
		{
			Utils.Log(this, Utils.Starting);

			IConnectionManager entry = null;
			try
			{
				entry = GetConnectionManager();

				maxRetryCounter = entry.Settings.GetSetting(entry, Settings.OmniJobMaxRetryCounter, SettingsLevel.System).IntValue;
				updateInterval = entry.Settings.GetSetting(entry, Settings.OmniJobUpdateInterval, SettingsLevel.System).IntValue;

				Utils.Log(this, $"MaxRetryCounter = {maxRetryCounter}, UpdateInterval = {updateInterval} minutes", LogLevel.Debug);

				if(updateInterval > 0)
				{
					omniJournalScheduler = StartScheduler<OmniJournalJob>(omniJournalScheduler, "OmniJournals", TimeSpan.FromMinutes(updateInterval));
				}
			}
			catch(Exception e)
			{
				if (entry == null)
				{
					Utils.Log(this, "Connection manager is null. Make sure SiteManagerUser and password are correctly configured in the Site Service configuration file.", LogLevel.Error);
				}

				Utils.LogException(this, e);
			}
			finally
			{
				Utils.Log(this, Utils.Done);
			}
		}
	}
}
