using System;
using System.Windows.Forms;

using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.Administration.DataLayer;
using LSOne.ViewPlugins.Administration.DataLayer.DataEntities;
using LSOne.ViewPlugins.Administration.Properties;
using LSOne.ViewPlugins.Administration.QueryResults;

using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Administration
{
	public class PluginEntry : IPlugin, IRibbonItemHandlers, IPluginDashboardProvider
	{
		internal const string DeleteAuditLogAction = "6cb67720-ac1d-11de-8a39-0800200c9a66";

		internal static IConnectionManager DataModel = null;
		internal static IApplicationCallbacks Framework = null;

		internal static Guid DefaultDataDashBoardItemID = new Guid("EB11BA81-7933-4DBE-80C0-A9126232743A");

		ItemHandler viewAuditLogItemHandler = null;

		private bool CanViewAuditLog
		{
			get
			{
				ViewBase currentSheet = Framework.ViewController.CurrentView;

				if (currentSheet == null)
				{
					return false;
				}
				else
				{
					return ((currentSheet.Attributes & ViewAttributes.Audit) == ViewAttributes.Audit);
				}
			}
		}

		private void CurrentViewChange_Handler(object sender, EventArgs args)
		{
			if (viewAuditLogItemHandler != null)
			{
				viewAuditLogItemHandler.Enabled = CanViewAuditLog;
			}
		}

		private void ViewAuditLog_ItemHandlerKnown(CategoryItem item, ItemHandler hander)
		{
			viewAuditLogItemHandler = hander;
		}

		private void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
		{
			if (args.PageKey == "Tools" && args.CategoryKey == "Administration")
			{
				if (PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings))
				{
					args.Add(new CategoryItem(
							Resources.Options,
							Resources.AdministrativeOptions,
							Resources.AdministrativeOptionsTooltipDescription,
							CategoryItem.CategoryItemFlags.Button,
							Resources.administration_options_16,
							PluginOperations.ShowAdministationOptions,
							"AdministrativeOptions"), 10);
				}

			}
			else if (args.PageKey == "Tools" && args.CategoryKey == "AuditLog")
			{
				if (PluginEntry.DataModel.HasPermission(Permission.SecurityViewAuditLogs))
				{
					args.Add(new CategoryItem(
							Properties.Resources.ViewLogs,
							Properties.Resources.ViewAuditLog,
							Properties.Resources.AuditLogTooltipDescription,
							CategoryItem.CategoryItemFlags.Button, 
							Properties.Resources.view_audit_logs_16,
							null,
							PluginOperations.ShowAuditLog,
							"ViewAuditLog",
							System.Windows.Forms.Shortcut.F6,
							new ItemHandlerKnown(ViewAuditLog_ItemHandlerKnown)), 10);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageAuditLogs))
				{
					args.Add(new CategoryItem(
							Resources.ManageLogs,
							Resources.ManageAuditLogs,
							Resources.ManageAuditLogsTooltipDescription,
							CategoryItem.CategoryItemFlags.Button,
							Resources.manage_audit_logs_16,
							null,
							PluginOperations.ManageAuditLog,
							"ManageAuditLog"), 20);
				}
			}
		}
		
		#region IPlugin Members

		public string Description
		{
			get { return Resources.Administration; }
		}

		public bool ImplementsFeature(object sender, string message, object parameters)
		{
			if (message == "CanViewAdministrationTab")
			{
				if ((string)parameters == "LicenseTab")
				{
					return DataModel.HasPermission(Permission.AdministrationMaintainSettings);
				}

				else if ((string)parameters == "StoreManagementTab")
				{
					return DataModel.HasPermission(Permission.AdministrationMaintainSettings);
				}

				else if ((string)parameters == "InventorySettingsTab")
				{
					return DataModel.HasPermission(Permission.AdministrationMaintainSettings);
				}
				else
				{
					return DataModel.HasPermission(Permission.AdministrationMaintainSettings);
				}
			}
			else if (message == "CanInsertDefaultData")
			{
				if (DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ImportDataPackages))
				{
					if (parameters != null)
					{
						((ContextBarItemConstructionArguments)parameters).Add(new ContextBarItem(Resources.InsertDefaultData, PluginOperations.InsertDefaultData), 900);
					}

					return true;
				}
				else
				{
					return false;
				}
			}

			return false;
		}

		public object Message(object sender, string message, object parameters)
		{
			if (message == "ViewLicenseTab")
			{
				PluginOperations.ShowAdministationOptions("LicenseTab");
			}

			if (message == "ViewStoreManagementTab")
			{
				PluginOperations.ShowAdministationOptions("StoreManagement");
			}

			if (message == "ViewInventorySettingsTab")
			{
				PluginOperations.ShowAdministationOptions("InventorySettingsTab");
			}
			if (message == "ViewSiteServiceTab")
			{
				PluginOperations.ShowAdministationOptions("SiteServiceTab");
			}
            if (message == "ViewMyTimePlanSettings")
            {
                PluginOperations.ShowAdministationOptions("MyTimePlanTab");
            }

            return null;
		}

		public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
		{
			DataModel = dataModel;

			Framework = frameworkCallbacks;

			frameworkCallbacks.ViewController.AddViewChangeHandler(CurrentViewChange_Handler);
			TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;

			frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
			frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
			frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

			frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);
			frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
			frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
			frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(AddRibbonPageCategoryItems);

			// Register data providers
			DataProviderFactory.Instance.Register<IAdministrationData, AdministrationData, ListViewItem>();
			DataProviderFactory.Instance.Register<IAuditingData, AuditingData, AuditLogResult>();
			DataProviderFactory.Instance.Register<IDBFieldData, DBFieldData, DBField>();
		}

		public void Dispose()
		{
			
		}

		public void GetOperations(IOperationList operations)
		{
			operations.AddOperation(Resources.ManageAuditLogs, PluginOperations.ManageAuditLog, Permission.SecurityManageAuditLogs);
			operations.AddOperation(Resources.AdministrativeOptions, PluginOperations.ShowAdministationOptions, Permission.AdministrationMaintainSettings);
			operations.AddOperation(Resources.InsertDefaultData, PluginOperations.InsertDefaultData, LSOne.DataLayer.BusinessObjects.Permission.ImportDataPackages);

			operations.AddOperation("", "ShowNumberSequences", false, true, PluginOperations.ShowNumberSequences, LSOne.DataLayer.BusinessObjects.Permission.AdministrationEditNumberSequences);
		}

		#endregion

		#region IRibbonItemHandlers Members

		public void ClearItemHandlers()
		{
			viewAuditLogItemHandler = null;
		}

		#endregion

		public void RegisterDashBoardItems(DashboardItemArguments args)
		{
			if(DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ImportDataPackages))
			{
				DashboardItem dashboardItem = new DashboardItem(DefaultDataDashBoardItemID, Resources.DataPackImport, true, 0); // Never refreshes the ticket
				args.Add(new DashboardItemPluginResolver(dashboardItem, this), 18); // Priority 18
			}
		}

		public void LoadDashboardItem(IConnectionManager threadedEntry, DashboardItem item)
		{
			// In case if the plugin is registering more than one then we check which one it is though we will never get item from other plugin here.
			if (item.ID == DefaultDataDashBoardItemID)
			{
				item.InformationText = Resources.DefaultDataDashboardText;
				item.SetButton(0, Resources.Import, (sender, args) => PluginOperations.InsertDefaultData(this, null));
				item.State = DashboardItem.ItemStateEnum.Info;
			}
		}
	}
}
