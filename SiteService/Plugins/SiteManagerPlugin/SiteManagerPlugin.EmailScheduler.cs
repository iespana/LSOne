using System;
using System.Collections.Generic;
using LSOne.Services.Interfaces.Enums;
using LSOne.SiteService.Utilities;
using Quartz;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        private static IScheduler emailScheduler = null;

        private static int emailSendInterval;
        private static int emailMaximumAttempts;
        private static int emailMaximumBatch;
        private static EMailTruncateSetting emailTruncateSettings;

        protected virtual void UpdateEMailSchedule(Dictionary<string, string> configurations)
        {
            try
            {
                Utils.Log(this, Utils.Starting);
                Utils.Log(this, "emailsendinterval = " + configurations["EMailSendInterval"], LogLevel.Debug);

                emailSendInterval = Convert.ToInt32(configurations["EMailSendInterval"]);
                if (emailSendInterval > 0)
                {
                    emailMaximumBatch = Convert.ToInt32(configurations["EMailMaximumBatch"]);
                    emailMaximumAttempts = Convert.ToInt32(configurations["EMailMaximumAttempts"]);

                    if (!Enum.TryParse(configurations["EMailTruncateQueue"], out emailTruncateSettings))
                    {
                        emailTruncateSettings = EMailTruncateSetting.Each; // Each
                    }

                    emailScheduler = StartScheduler<EMailJob>(emailScheduler, "Email", TimeSpan.FromMinutes(emailSendInterval));
                }
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }
        }
    }
}