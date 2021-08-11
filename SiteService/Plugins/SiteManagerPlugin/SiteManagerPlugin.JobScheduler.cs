using LSOne.SiteService.Utilities;
using Quartz;
using Quartz.Impl;
using System;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        protected virtual IScheduler StartScheduler<T>(IScheduler scheduler, string identifier, TimeSpan interval) where T : IJob
        {
            try
            {
                Utils.Log(this, Utils.Starting);
                Utils.Log(this, $"Stopping previous job: {identifier}", LogLevel.Debug);

                scheduler = StopScheduler(scheduler);

                if (interval.TotalSeconds > 0)
                {
                    Utils.Log(this, "Starting quartz scheduler:", LogLevel.Trace);

                    ISchedulerFactory schedFact = new StdSchedulerFactory();

                    scheduler = schedFact.GetScheduler();
                    scheduler.Start();

                    var job = JobBuilder.Create<T>()
                        .WithIdentity($"{identifier}Job", $"{identifier}Group")
                        .Build();

                    var startTime = DateBuilder.NextGivenSecondDate(null, 59);

                    var trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                              .WithIdentity($"{identifier}Trigger", $"{identifier}Group")
                                              .StartAt(startTime)
                                              .WithSimpleSchedule(x => x.WithInterval(interval).RepeatForever())
                                              .Build();

                    var starting = scheduler.ScheduleJob(job, trigger);
                    Utils.Log(this, $"Job {identifier} starting at " + starting.ToString(), LogLevel.Debug);
                }
            }
            catch
            {
                Utils.Log(this, $"Failed to start job: {identifier}.");
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }

            return scheduler;
        }

        protected virtual IScheduler StopScheduler(IScheduler scheduler)
        {
            try
            {
                Utils.Log(this, Utils.Starting);

                if (scheduler != null)
                {
                    try
                    {
                        foreach (IJobExecutionContext job in scheduler.GetCurrentlyExecutingJobs())
                        {
                            Utils.Log(this, "Job " + job.JobDetail.Key + " is still running and will be aborted", LogLevel.Trace);
                        }

                        scheduler.Shutdown(false);
                        scheduler = null;
                    }
                    catch (Exception e)
                    {
                        Utils.LogException(this, e);
                    }
                }
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }

            return scheduler;
        }
    }
}
