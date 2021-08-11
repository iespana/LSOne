using System;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;

namespace LSOne.ViewPlugins.Scheduler
{
    internal class PluginOperations
    {
        private const string replicationCategoryKey = "General setup";
        private const string replicationItemKey = "Replication";

        private static bool pluginIsValid = true;
        public static void ShowLocationsListView(object sender, EventArgs args)
        {
            if (!AssertValid())
                return;

            PluginEntry.Framework.ViewController.Add(new Views.LocationListView());
        }

        public static void NewJob(object sender, EventArgs args)
        {
            if (!AssertValid())
                return;

            DialogResult result;

            Dialogs.NewJobDialog dlg = new Dialogs.NewJobDialog();

            PluginEntry.Framework.SuspendSearchBarClosing();
            result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);
            PluginEntry.Framework.ResumeSearchBarClosing();
            if (result == DialogResult.OK)
            {
                // We put null in sender so that we also get our own change hint sent.
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Job", dlg.JobId, null);
                PluginEntry.Framework.ViewController.Add(new Views.JobView(dlg.JobId));
            }
       }

        public static void ShowDatabaseMapListView(object sender, EventArgs args)
        {
            if (!AssertValid())
                return;

            PluginEntry.Framework.ViewController.Add(new Views.DatabaseMapListView());
        }


        public static void ShowDatabaseDesignView(object sender, EventArgs args)
        {
            if (!AssertValid())
                return;

            PluginEntry.Framework.ViewController.Add(new Views.DatabaseDesignView());
        }

        public static void ShowSubJobsView(object sender, EventArgs args)
        {
            if (!AssertValid())
                return;

            PluginEntry.Framework.ViewController.Add(new Views.SubJobListView());
        }

        public static void ShowJobsView(object sender, EventArgs args)
        {
            if (!AssertValid())
                return;

            PluginEntry.Framework.ViewController.Add(new Views.JobListView());
        }


        public static void ShowJobLogView(object sender, EventArgs args)
        {
            if (!AssertValid())
                return;

            PluginEntry.Framework.ViewController.Add(new Views.JobLogView());
        }

        public static void ShowReplicationCountersView(object sender, EventArgs args)
        {
            if (!AssertValid())
                return;

            PluginEntry.Framework.ViewController.Add(new Views.ReplicationCountersView());
        }

        public static void ShowSettingsView(object sender, EventArgs args)
        {
            if (!AssertValid())
                return;

            PluginEntry.Framework.ViewController.Add(new Views.SettingsView());
        }
 
        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.GeneralSetup, replicationCategoryKey, null), 100);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == replicationCategoryKey)
            {
                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.Replication))
                {
                    // Add operation item handler
                    args.Add(new Item(Properties.Resources.Replication, replicationItemKey, null), 1990);
                }
            }
          
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == replicationCategoryKey && args.ItemKey == replicationItemKey)
            {
                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.LocationView) && PluginEntry.DataModel.HasPermission(SchedulerPermissions.LocationEdit))
                {
                    args.Add(new ItemButton(Properties.Resources.LocationListButtonText, Properties.Resources.LocationListButtonDescription, new EventHandler(ShowLocationsListView)), 100);
                }

                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobEdit))
                {
                    args.Add(new ItemButton(Properties.Resources.JobButtonText, Properties.Resources.JobButtonDescription, new EventHandler(NewJob)), 200);
                }

                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobView) && PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobEdit))
                {
                    args.Add(new ItemButton(Properties.Resources.JobListButtonText, Properties.Resources.JobListButtonDescription, new EventHandler(ShowJobsView)), 300);
                    args.Add(new ItemButton(Properties.Resources.SubjobListButtonText, Properties.Resources.SubjobListButtonDescription, new EventHandler(ShowSubJobsView)), 400);
                    args.Add(new ItemButton(Properties.Resources.JobLogListButtonText, Properties.Resources.JobLogButtonDescription, new EventHandler(ShowJobLogView)), 450);
                    args.Add(new ItemButton(Properties.Resources.ReplicationCountersListButtonText, Properties.Resources.ReplicationCountersDescription, new EventHandler(ShowReplicationCountersView)), 470);
                }

                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseDesignView) && PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseDesignEdit))
                {
                    args.Add(new ItemButton(Properties.Resources.DatabaseDesignListButtonText, Properties.Resources.DatabaseDesignListButtonDescription, new EventHandler(ShowDatabaseDesignView)), 500);
                }

                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseMapView) && PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseMapEdit))
                {
                    args.Add(new ItemButton(Properties.Resources.DatabaseMapButtonText, Properties.Resources.DatabaseMapButtonDescription, new EventHandler(ShowDatabaseMapListView)), 600);
                }

                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.SettingsView) && PluginEntry.DataModel.HasPermission(SchedulerPermissions.SettingsEdit))
                {
                    args.Add(new ItemButton(Properties.Resources.SettingsButtonText, Properties.Resources.SettingsButtonDescription, new EventHandler(ShowSettingsView)), 9000);
                }

            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Tools, "Tools"), 1000);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {

            if (args.PageKey == "Tools")
            {
                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.Replication))
                {
                    args.Add(new PageCategory(Properties.Resources.Replication, "Replication"), 300);
                }
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Tools" && args.CategoryKey == "Replication")
            {
                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.Replication))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.Jobs,
                            Properties.Resources.Jobs,
                            Properties.Resources.JobsTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            Properties.Resources.run_options_32,
                            new EventHandler(ShowJobsView),
                            "JobsView"), 10);
                
                    args.Add(new CategoryItem(
                            Properties.Resources.SettingsButtonText,
                            Properties.Resources.ReplicationSettings,
                            Properties.Resources.SettingsButtonTooltipDescription,
                            CategoryItem.CategoryItemFlags.DropDown,
                            null,
                            Properties.Resources.replication_32,
                            null,
                            "Settings"), 20);
                }
            }
        }

        //private static bool CheckUpdates()
        //{
        //    using (var updater = JobSchedulerDataContext.CreateUpdater(PluginEntry.GetDbConnection(), PluginEntry.DataModel.Connection.DataAreaId))
        //    {
        //        var updateResult = LSRetail.DD.Common.StoreController.Versioning.DatabaseVersioning.CheckAndPerformUpdates(updater, PluginEntry.Framework.MainWindow, Properties.Resources.ReplicationPlugin);
        //        pluginIsValid = updateResult != DD.Common.StoreController.Versioning.DatabaseVersioning.UpdateStatus.Invalid;
        //        return updateResult == DD.Common.StoreController.Versioning.DatabaseVersioning.UpdateStatus.Updated;
        //    }
        //}


        public static void RunJob( JscJob job, bool forceNormal = false,JscSchedulerLog log = null, JscLocation location = null)
        {
            if (!AssertValid())
                return;

            Cursor.Current = Cursors.WaitCursor;
            job = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJob(PluginEntry.DataModel, job.ID);

            // Make sure that the job can be run
            var validationResult = DataProviderFactory.Instance.Get<IJobData, JscJob>().ValidateForRun(PluginEntry.DataModel, job);
            if (validationResult.Message != JobValidationMessage.OK)
            {
                string msg = string.Format(Properties.Resources.JobRunValidationFailureMsg, validationResult.MessageText);
                MessageBox.Show(PluginEntry.Framework.MainWindow, msg, Properties.Resources.JobRunHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Get confirmation from user
            if (QuestionDialog.Show(string.Format(Properties.Resources.JobRunConfirmMsg, job.Text), Properties.Resources.JobRunHeader) != DialogResult.Yes)
            {
                return;
            }


            IDDService ddService = (IDDService)PluginEntry.DataModel.Service(ServiceType.DDService);
            if (ddService != null)
            {
                ddService.RunJob(job, PluginEntry.Framework.MainWindow as Control, forceNormal, log, location);
            }
            
        }


        private static bool AssertValid()
        {
            if (!pluginIsValid)
            {
                MessageBox.Show(PluginEntry.Framework.MainWindow, Properties.Resources.PluginInvalid, Properties.Resources.ReplicationPlugin, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return  pluginIsValid;
        }


        internal static void LoginHandler(object sender, LoginEventArgs args)
        {
            if (args.LoginEventType == LoginEventArgs.LoginEventTypeEnum.PreLogin)
            {

                args.ForceRelogin();

            }
        }

        private class NotifyTriggerData
        {
            public ServerSettings ServerSettings { get; set; }
            public Guid JobId { get; set; }
        }

        internal static void NotifyTriggerChange(Guid jobId)
        {
            // Get the scheduler server settings. Note that we must do it here since we don't want a potential
            // database access in the thread created below
            var notifyTriggerData = new NotifyTriggerData
            {
                ServerSettings = PluginEntry.SchedulerSettings.ServerSettings,
                JobId = jobId
            };

            // Asynchronously notify the scheduler backend, we don't even care about errors
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(NotifyTriggerChangeThreadProc), notifyTriggerData);
        }

        internal static void NotifyTriggerChangeThreadProc(object state)
        {
            var notifyTriggerData = (NotifyTriggerData)state;
            try
            {
                using (SchedulerClient client = CreateSchedulerClient(notifyTriggerData.ServerSettings))
                {
                    client.InvalidateJobTriggers(notifyTriggerData.JobId);
                }
            }
            catch (Exception ex)
            {
                if (ex is System.ServiceModel.EndpointNotFoundException)
                {
                    string contextMsg = string.Format(Properties.Resources.JobTriggerSchedulerMsg, ex.Message);
                    PluginEntry.Framework.LogMessage(LogMessageType.Warning, contextMsg + ": " + ex.Message);
                }
                else
                {
                    string contextMsg = Properties.Resources.JobTriggerSchedulerExceptionMsg;
                    PluginEntry.Framework.LogMessage(LogMessageType.Error, contextMsg, ex);
                }
            }
        }


        private static SchedulerClient CreateSchedulerClient(ServerSettings serverSettings)
        {
            int port = 0;
            if (!string.IsNullOrWhiteSpace(serverSettings.Port))
            {
                if (!int.TryParse(serverSettings.Port, out port))
                {
                    port = 0;
                }
            }

            return new SchedulerClient(serverSettings.Host, port, serverSettings.NetMode);
        }


        internal static void ShowLocationView(Guid guid)
        {
            PluginEntry.Framework.ViewController.Add(new Views.LocationView(guid));
        }

        internal static void ShowLocationView(object sender, PluginOperationArguments args)
        {
            RecordIdentifier locationID = args.ID;

            PluginEntry.Framework.ViewController.Add(new Views.LocationView(locationID));
        }

        public static void GetLocations(object sender, PluginOperationArguments args)
        {
            if (!AssertValid())
                return;
            var locations = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocations(PluginEntry.DataModel, false).ToList();
            args.Result = locations.ConvertAll(l=> new DataEntity(l.ID, l.Text) );

        }
    }
}
