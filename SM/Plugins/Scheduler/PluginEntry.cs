using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlDDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Scheduler.Controls;
using LSRetail.DD.Common;
using LSOne.ViewCore;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        
        //private static LSRetail.DD.Common.StoreController.ConnectionSettings connectionSettings;

        private static SchedulerSettings schedulerSettings;

        private void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            //if (args.ContextName == "LSRetail.StoreController.Administration.Views.AdministrationView")
            //{
            //    // Here we might want to check permission depending on what you want to do,
            //    // the fact that the Admin sheet is showing allready means that the admin sheet made sure
            //    // the user had permission to show admin sheet, so you would only want to check
            //    // permission here if the additional tab requires different permission than the permission to
            //    // enter the parent sheet, in this case the Administration sheet.
            //    args.Add(new LSRetail.StoreController.Controls.TabControl.Tab(Properties.Resources.JobScheduler, new LSRetail.StoreController.Controls.PanelFactoryHandler(ViewPages.JobSchedulerViewPage.CreateInstance)), 40);
            //}
        }

        private void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            //ExtendedMenuItem item;

            switch (args.Key)
            {
                case "RibbonSettings":
                    if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.LocationEdit) || PluginEntry.DataModel.HasPermission(SchedulerPermissions.LocationView))
                    {
                        args.AddMenu(new ExtendedMenuItem(Properties.Resources.LocationList, 10, new EventHandler(PluginOperations.ShowLocationsListView)));
                    }

                    if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobEdit) || PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobView))
                    {
                        args.AddMenu(new ExtendedMenuItem(Properties.Resources.Subjob, 20, new EventHandler(PluginOperations.ShowSubJobsView)));
                        args.AddMenu(new ExtendedMenuItem(Properties.Resources.JobLog, 30, new EventHandler(PluginOperations.ShowJobLogView)));
                        args.AddMenu(new ExtendedMenuItem(Properties.Resources.ReplicationCounters, 40, new EventHandler(PluginOperations.ShowReplicationCountersView)));
                    }

                    if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseDesignView) || PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseDesignEdit))
                    {
                        args.AddMenu(new ExtendedMenuItem(Properties.Resources.DatabaseDesign, 50, new EventHandler(PluginOperations.ShowDatabaseDesignView)));
                    }

                    if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseMapEdit) || PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseMapView))
                    {
                        args.AddMenu(new ExtendedMenuItem(Properties.Resources.DatabaseMap, 60, new EventHandler(PluginOperations.ShowDatabaseMapListView)));
                    }

                    if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.SettingsEdit) || PluginEntry.DataModel.HasPermission(SchedulerPermissions.SettingsView))
                    {
                        args.AddMenu(new ExtendedMenuItem(Properties.Resources.Settings, 70, new EventHandler(PluginOperations.ShowSettingsView)));
                    }
                    break;
            }
        }

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.JobSchedulerDescription; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            if (message == "Get location select panel")
            {
                return true;
            }
            if (message == "GetDataDirectorJobs")
            {
                return true;
            }
            if (message == "GetDataDirectorLocations")
            {
                return true;
            }
            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            if (message == "Get location select panel")
            {
                return new LocationSelectPanel((IEnumerable<JscLocation>)((object[])parameters)[0], (Guid?)((object[])parameters)[1], false);
            }
            if (message == "GetDataDirectorJobs")
            {
                IEnumerable<JscJob> jobs = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJobs(DataModel, false);
                if (jobs != null)
                {
                    return jobs.Cast<DataEntity>().ToList();
                }
            }
            if (message == "GetDataDirectorLocations")
            {
                IEnumerable<JscLocation> locations = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocations(DataModel, false);
                if (locations != null)
                {
                    BlockingCollection<DataEntity> list = new BlockingCollection<DataEntity>();
                    locations.AsParallel().ForAll(l => list.Add(new DataEntity(new RecordIdentifier(l.ID, l.DDHost), l.Text)));
                    OrderedParallelQuery<DataEntity> ordered = list.AsParallel().OrderBy(de => de.Text);
                    return ordered.ToList();
                }
            }
            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            // Initialize DD AppConfig
            LSRetail.DD.Common.AppConfig.Init(true);

     
            //// We want to be able to add to sheet contexts from other plugins
            //frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(new ContextBarItemConstructionEventHandler(PluginOperations.TaskBarItemCallback));

            // We want to add tabs on tab panels in other plugins
            TabControl.TabPanelConstructionHandler += ConstructTabs;

            // We want to be able to register items to the main application menu
            frameworkCallbacks.AddMenuConstructionConstructionHandler(ConstructMenus);

            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);

            frameworkCallbacks.AddLoginHandler(PluginOperations.LoginHandler);

            // Register data providers
            DataProviderFactory.Instance.Register<IDesignData, DesignData, JscTableDesign>();
            DataProviderFactory.Instance.Register<IInfoData, InfoData, JscInfo>();
            DataProviderFactory.Instance.Register<IJobData, JobData, JscJob>();
            DataProviderFactory.Instance.Register<IReplicationActionData, ReplicationActionData, ReplicationAction>();
            DataProviderFactory.Instance.Register<ILocationData, LocationData, JscLocation>();
        }
        
        public void Dispose()
        {
        }

        public void GetOperations(IOperationList operations)
        {

            operations.AddOperation("Show Location Replication", "ShowLocationView", false, false, PluginOperations.ShowLocationView, String.Empty);
        
            operations.AddOperation("Retrieves a list of all Locations","SchedulerLocations", false, false, PluginOperations.GetLocations,
                string.Empty);

            //operations.AddOperation(Properties.Resources.NewStore, PluginOperations.NewStore, BusinessObjects.Permission.StoreEdit);
            //operations.AddOperation(Properties.Resources.ViewAllStores, PluginOperations.ShowStoresListView);
        }
        #endregion

        //internal static LSRetail.DD.Common.StoreController.ConnectionSettings ConnectionSettings
        //{
        //    get
        //    {
        //        if (connectionSettings == null)
        //        {
        //            connectionSettings = new LSRetail.DD.Common.StoreController.ConnectionSettings();
        //        }
        //        return connectionSettings;
        //    }
        //}

        internal static System.Data.IDbConnection GetDbConnection()
        {
#pragma warning disable 618
            return (IDbConnection)DataModel.Connection.NativeConnection;
#pragma warning restore 618
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
                        DataProviderFactory.Instance.Get<IInfoData, JscInfo>().Save(PluginEntry.DataModel, schedulerSettings);
                    }
                }

                return schedulerSettings;
            }
        }
    }
}
