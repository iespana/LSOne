using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.EndOfDay.DataLayer;
using System;
using System.Collections.Generic;

namespace LSOne.ViewPlugins.EndOfDay
{
    public class PluginEntry : IPlugin,  IPluginDashboardProvider
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        internal static Guid TodaysStatementsForHeadOfficeID = new Guid("48048fc3-9d27-4491-b6d4-61d77b6c84ca");

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.EOD; }
        }

        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            // We want to be able to add to sheet contexts from other plugins            
            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);

            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);

            frameworkCallbacks.AddMenuConstructionConstructionHandler(PluginOperations.ConstructMenus);

            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;

            // Register data providers
            DataProviderFactory.Instance.Register<IReportData, ReportData, DataEntity>();
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.UnpostedStatements, PluginOperations.ShowUnpostedStatements, Permission.RunEndOfDay);
            operations.AddOperation(Properties.Resources.PostedStatements, PluginOperations.ShowPostedStatements, Permission.RunEndOfDay);
            operations.AddOperation(Properties.Resources.TenderDeclaration, PluginOperations.PerformTenderDeclaration,Permission.TenderDeclaration);
        }

        #endregion

        public void LoadDashboardItem(IConnectionManager threadedEntry, DashboardItem item)
        {
            if(item.ID == TodaysStatementsForHeadOfficeID)
            {
                int statementsDone = 0;
                DateTime today = DateTime.Now;

                item.State = DashboardItem.ItemStateEnum.Info;

                List<StatementCountInfo> statements;

                statements = Providers.StatementInfoData.GetPostedStatementCount(threadedEntry,
                    new DateTime(today.Year, today.Month, today.Day, 0, 0, 0),
                    new DateTime(today.Year, today.Month, today.Day, 23, 59, 59));
                    
                foreach(StatementCountInfo statementCountInfo in statements)
                {
                    if(statementCountInfo.StatementCount > 0)
                    {
                        statementsDone++;
                    }
                }

                if(statementsDone == 0)
                {
                    item.InformationText = Properties.Resources.NoStatementsDoneToday;
                }
                else if(statementsDone == statements.Count)
                {
                    item.InformationText = Properties.Resources.StatementsDoneForAllStores;
                }
                else
                {
                    item.InformationText = Properties.Resources.StatementsDoneForXStores.Replace("#1", statementsDone.ToString()).Replace("#2", statements.Count.ToString());
                }

                item.SetButton(0, Properties.Resources.Manage, PluginOperations.ShowUnpostedStatements);
            }
        }

        public void RegisterDashBoardItems(ViewCore.EventArguments.DashboardItemArguments args)
        {
            DashboardItem todaysStatementsForHeadOffice = new DashboardItem(TodaysStatementsForHeadOfficeID, Properties.Resources.TodaysStatements, true, 60); // 1 hour refresh interval

            if (PluginEntry.DataModel.IsCentralDatabase || PluginEntry.DataModel.IsHeadOffice)
            {
                if (PluginEntry.DataModel.HasPermission(Permission.RunEndOfDay))
                {
                    if (DataModel.CurrentStoreID.IsEmpty)
                    {
                        args.Add(new DashboardItemPluginResolver(todaysStatementsForHeadOffice, this), 90); // Priority 90
                    }
                }
            }
        }
    }
}
